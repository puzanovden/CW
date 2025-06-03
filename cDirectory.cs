using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using System.Reflection;
using System.Reflection.Metadata;

namespace CW
{
    public class cDirectory<T> : IDirectory where T : cDirectoryItem, IDirectoryItem, new()
    {
        public List<T> Items { get; private set; } = new();
        public List<cFieldDefinition> fieldDefinitions;
        private int currentLastCode = 0;
        public cDirectory()
        {
            fieldDefinitions = T.GetFieldDefinitions();
        }

        public List<cFieldDefinition> GetFieldDefinitions()
        {
            return T.GetFieldDefinitions();
        }

        public void AddItem(string name)
        {
            var item = new T
            {
                Code = GenerateNextCode(),
                Name = name
            };

            Items.Add(item);
        }
        private T AddAndReturnItem(string name)
        {
            var item = new T
            {
                Code = GenerateNextCode(),
                Name = name
            };

            Items.Add(item);
            return item;
        }

        private string GenerateNextCode()
        {
            if (Items.Count == 0)
            {
                currentLastCode = 1;
                return "000001";
            }
            else
            {
                if (currentLastCode == 0)
                {
                    currentLastCode = Items.Max(i => int.Parse(i.Code));
                }
                return (++currentLastCode).ToString("D6");
            }
        }
        public void OpenItem(cDirectoryItem item)
        {
            var form = new formDirectoryItem();
            form.SetData(item, fieldDefinitions);
            form.ShowDialog();
        }

        public bool CreateAndEditItem(Form owner)
        {
            var item = new T
            {
                Code = GenerateNextCode(),
                Name = ""
            };

            var form = new formDirectoryItem();
            form.SetData(item,fieldDefinitions);

            var result = form.ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                Items.Add(item);
                return true;
            }

            return false;
        }

        public T GetTypeItem() => new T();

        private T GetByName(string name)
        {
            return Items.FirstOrDefault(i => i.Name == name);
        }

        cDirectoryItem IDirectory.GetOrAddByName(string name)
        {
            var item = GetByName(name);
            if (item == null)
            {
                item = AddAndReturnItem(name);
            }
            return item;
        }

        Type IDirectory.GetTypeItem()
        {
            return typeof(T);
        }
        IEnumerable<cDirectoryItem> IDirectory.GetItems()
        {
            return Items;
        }
        IEnumerable<cDirectoryItem> IDirectory.Search(string query, string[]? fields = null)
        {
            var lowered = query.ToLower().Trim();
            var props = typeof(T).GetProperties();

            var searchFields = fields ?? new[] { "Code", "Name" };

            return Items.Where(item =>
            {
                foreach (var field in searchFields)
                {
                    var prop = props.FirstOrDefault(p => p.Name == field);
                    if (prop == null) continue;

                    var value = prop.GetValue(item)?.ToString()?.ToLower();
                    if (!string.IsNullOrEmpty(value) && value.Contains(lowered))
                        return true;
                }

                return false;
            }).ToList();
        }
        public void SaveToFile(string filePath)
        {
            using var writer = new StreamWriter(filePath);

            var props = typeof(T).GetProperties();
            var fieldNames = props.Select(p => p.Name).ToArray();

            writer.WriteLine(string.Join("|", fieldNames));

            foreach (var item in Items)
            {
                var values = fieldNames.Select(name =>
                {
                    var prop = props.FirstOrDefault(p => p.Name == name);
                    var value = prop?.GetValue(item);

                    if (value is cDirectoryItem refItem)
                        return $"{{{refItem.Code}}}";

                    return value?.ToString() ?? "";
                });

                writer.WriteLine(string.Join("|", values));
            }
        }

        public void ImportFromFile(string path)
        {
            if (!File.Exists(path)) return;

            using var reader = new StreamReader(path);

            var headerLine = reader.ReadLine();
            if (headerLine == null) return;

            var headers = headerLine.Split('|');
          
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = line.Split('|');
                if (values != null)
                {
                    var indName = Array.IndexOf(headers, "Name");
                    if (indName == -1)
                    {
                        MessageBox.Show("Немає поля з найменуванням. Файл не завантажено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;


                    }
                    var newItem = GetByName(values[indName]);
                    if (newItem == null)
                    {
                        newItem = AddAndReturnItem(values[indName]);
                        
                    }
                    if (newItem == null) continue;
                    FillValues(newItem, headers, values);
                    //ЗаполнтьПоля(елемент масивимен, масивзначень)
                }
              }
            }
        
        private void FillValues(T newItem, string[] headers, string[] values)
        {
            var props = typeof(T).GetProperties().ToDictionary(p => p.Name);
            var fields = fieldDefinitions;
            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var fieldName = headers[j];
                if (fieldName == "Code" || fieldName == "Name") continue;

                if (props.TryGetValue(fieldName, out var prop))
                {
                    try
                    {
                        var raw = values[j];

                        if (typeof(cDirectoryItem).IsAssignableFrom(prop.PropertyType))
                        {
                            string refName = fields.FirstOrDefault(f => f.Name == fieldName)?.ReferenceName ?? fieldName;

                            var itemValue = cDataBase.DB.GetDirectoryByName(refName).GetOrAddByName(raw);

                            prop.SetValue(newItem, itemValue); 
                        }
                        else
                        {
                            var converted = Convert.ChangeType(raw, prop.PropertyType);
                            prop.SetValue(newItem, converted);
                        }
                    }
                    catch
                    {
                        // ігнор
                    }
                }
            }
        }
        public void LoadFromFile(string path, bool loadData)
        {
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);
            if (lines.Length < 2) return;

            var headers = lines[0].Split('|');
            var props = typeof(T).GetProperties().ToDictionary(p => p.Name);

            if (loadData)
            {
                Items.Clear(); // тільки в першому проході
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split('|');
                T item;

                if (loadData)
                {
                    item = new T();

                    for (int j = 0; j < headers.Length && j < values.Length; j++)
                    {
                        var name = headers[j];
                        var val = values[j];

                        if (!props.TryGetValue(name, out var prop)) continue;

                        // пропуск посиланнь
                        if (typeof(cDirectoryItem).IsAssignableFrom(prop.PropertyType)) continue;

                        try
                        {
                            var converted = Convert.ChangeType(val, prop.PropertyType);
                            prop.SetValue(item, converted);
                        }
                        catch { }
                    }

                    Items.Add(item);
                }
                else
                {
                    // другий обробка посилань
                    if (i - 1 >= Items.Count) continue;
                    item = Items[i - 1];

                    for (int j = 0; j < headers.Length && j < values.Length; j++)
                    {
                        var name = headers[j];
                        var val = values[j];

                        if (!props.TryGetValue(name, out var prop)) continue;

                        if (typeof(cDirectoryItem).IsAssignableFrom(prop.PropertyType) && val.StartsWith("{") && val.EndsWith("}"))
                        {
                            var code = val.Trim('{', '}');

                            var refName = fieldDefinitions.FirstOrDefault(f => f.Name == name)?.ReferenceName ?? name;
                            var dir = cDataBase.DB.GetDirectoryByName(refName);

                            if (dir is IDirectory addable)
                            {
                                var refItem = addable.GetItems().FirstOrDefault(x => x.Code == code);
                                if (refItem != null)
                                {
                                    prop.SetValue(item, refItem);
                                }
                            }
                        }
                    }
                }
            }


        }
        public void DeleteItem(cDirectoryItem itemToDelete)
        {
            if (itemToDelete is T item)
            {
                Items.Remove(item);
            }
        }


    }
}
