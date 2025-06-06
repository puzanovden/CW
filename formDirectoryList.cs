
namespace CW
{

    public partial class formDirectoryList : Form
    {
        private BindingSource bindingSource = new BindingSource();
        private IDirectory currentDir;
        private cDirectoryItem SelectedItem;

        private List<cDirectoryItem> originalItems = new(); // для сорт
        private bool sortAscending = true;
        private string? lastSortedColumn;

        public bool ChoseMode;

        private Dictionary<string, Control[]> filterControls = new();

        private void GenerateFilterFields()
        {
            filterPanel.Controls.Clear();
            filterControls.Clear();
            var fields = currentDir.GetFieldDefinitions();


            int top = 10;

            foreach (var field in fields)
            {
                var label = new Label
                {
                    Text = field.PublicName,
                    Left = 10,
                    Top = top,
                    Width = 276,
                    Font = new Font("Segoe UI", 9)
                };
                filterPanel.Controls.Add(label);
                top += 25;

                Control[] inputs;

                if (field.Type == FieldType.Number)
                {
                    var fromBox = new NumericUpDown
                    {
                        Name = "from" + field.Name,
                        Left = 10,
                        Top = top,
                        Width = 125,
                        DecimalPlaces = 2,
                        Minimum = decimal.MinValue,
                        Maximum = decimal.MaxValue,
                        Tag = "from"
                    };

                    var toBox = new NumericUpDown
                    {
                        Name = "to" + field.Name,
                        Left = 151,
                        Top = top,
                        Width = 125,
                        DecimalPlaces = 2,
                        Minimum = decimal.MinValue,
                        Maximum = decimal.MaxValue,
                        Value = 999999999,
                        Tag = "to"
                    };

                    filterPanel.Controls.Add(fromBox);
                    filterPanel.Controls.Add(toBox);
                    inputs = new[] { fromBox, toBox };
                    top += 30;
                }
                else if (field.Type == FieldType.Reference && field.ReferenceName != null)
                {
                    var textBox = new TextBox
                    {
                        Name = "txt" + field.Name,
                        Left = 10,
                        Top = top,
                        Width = 185,
                        ReadOnly = true,
                        BackColor = Color.White
                    };

                    var button = new Button
                    {
                        Text = "...",
                        Left = 200,
                        Top = top - 1,
                        Width = 30,
                        Height = textBox.Height
                    };

                    var buttonClear = new Button
                    {
                        Text = "X",
                        Left = 235,
                        Top = top - 1,
                        Width = 30,
                        Height = textBox.Height
                    };

                    button.Click += (s, e) =>
                    {
                        var dir = cDataBase.DB.GetDirectoryByName(field.ReferenceName);
                        if (dir is IDirectory addable)
                        {
                            var form = new formDirectoryList();
                            form.ChoseMode = true;
                            form.SetData(dir);
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                var selectedItem = form.GetSelectedItem();
                                if (selectedItem != null)
                                {
                                    textBox.Text = selectedItem.Name;
                                    textBox.Tag = selectedItem;
                                }
                            }
                        }
                    };

                    buttonClear.Click += (s, e) =>
                    {
                        textBox.Text = string.Empty;
                        textBox.Tag = null;
                    };

                    filterPanel.Controls.Add(textBox);
                    filterPanel.Controls.Add(button);
                    filterPanel.Controls.Add(buttonClear);

                    inputs = new[] { textBox };
                    top += 30;
                }
                else
                {
                    var textBox = new TextBox
                    {
                        Name = "txt" + field.Name,
                        Left = 10,
                        Top = top,
                        Width = 266
                    };

                    filterPanel.Controls.Add(textBox);
                    inputs = new[] { textBox };
                    top += 30;
                }

                filterControls[field.Name] = inputs;
                top += 5;
            }

            var buttonFilter = new Button
            {
                Text = "Застосувати фільтр",
                Left = 10,
                Top = top + 5,
                Width = 266,
                Height = 30
            };

            buttonFilter.Click += (s, e) => ApplyFilters();
            filterPanel.Controls.Add(buttonFilter);

            filterPanel.Height = buttonFilter.Bottom + 10;
        }

        private void ApplyFilters()
        {
            if (currentDir == null) return;

            // тип T
            var tType = currentDir.GetType().GetGenericArguments()[0];
            var props = tType.GetProperties();
            var fields = currentDir.GetFieldDefinitions();
            var items = currentDir.GetItems();

            var filtered = items.Where(item =>
            {
                foreach (var field in fields)
                {
                    if (!filterControls.TryGetValue(field.Name, out var controls))
                        continue;

                    var prop = props.FirstOrDefault(p => p.Name == field.Name);
                    if (prop == null) continue;

                    object? value = prop.GetValue(item);

                    if (field.Type == FieldType.Number)
                    {
                        var from = controls[0] as NumericUpDown;
                        var to = controls[1] as NumericUpDown;

                        decimal val = value != null ? Convert.ToDecimal(value) : 0;

                        if (from != null && val < from.Value) return false;
                        if (to != null && val > to.Value) return false;
                    }
                    else if (field.Type == FieldType.Reference)
                    {
                        var textBox = controls[0] as TextBox;
                        if (textBox?.Tag is cDirectoryItem selected)
                        {
                            if (value == null || !value.Equals(selected)) return false;
                        }
                    }
                    else
                    {
                        var textBox = controls[0] as TextBox;
                        string input = textBox?.Text.Trim().ToLower() ?? "";

                        if (!string.IsNullOrEmpty(input))
                        {
                            var str = value?.ToString()?.ToLower() ?? "";
                            if (!str.Contains(input)) return false;
                        }
                    }
                }

                return true;
            });

            // до Т
            var castedList = typeof(Enumerable).GetMethod("Cast")!.MakeGenericMethod(tType).Invoke(null, new object[] { filtered });
            var toList = typeof(Enumerable).GetMethod("ToList")!.MakeGenericMethod(tType).Invoke(null, new object[] { castedList });

            bindingSource.DataSource = toList;
            bindingSource.ResetBindings(false);
        }

        public formDirectoryList()
        {
            InitializeComponent();

            directoryList.DataSource = bindingSource;
            directoryList.AutoGenerateColumns = true;
            directoryList.ReadOnly = true;
            directoryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        public void SetData(IDirectory directory)
        {
            currentDir = directory;
            originalItems = currentDir.GetItems().ToList();

            bindingSource.DataSource = directory.GetItems();
            directoryList.DataSource = bindingSource;

            directoryList.Columns.Clear();
            directoryList.AutoGenerateColumns = false;

            foreach (var field in directory.GetFieldDefinitions())
            {
                var column = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = field.Name,
                    HeaderText = field.PublicName,
                    Name = field.Name,
                    SortMode = DataGridViewColumnSortMode.Automatic
                };

                switch (field.Type)
                {
                    case FieldType.Number:
                        column.Width = 100;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        column.DefaultCellStyle.Format = "N2"; 
                        break;

                    case FieldType.String:
                        column.Width = 200;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        break;

                    case FieldType.Reference:
                        column.Width = 150;
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case FieldType.Image:
                        column.Visible = false; 
                        break;

                    default:
                        column.Width = 120;
                        break;
                }

                directoryList.Columns.Add(column);
            }

            var name = cDataBase.DB.GetDisplayName(directory);
            if (!string.IsNullOrEmpty(name))
                this.Text = $"Довідник: {name}";

            GenerateFilterFields();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (currentDir.CreateAndEditItem(this))
            {
                bindingSource.ResetBindings(false);
            }
        }

        private void directoryList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && currentDir != null)
            {

                var item = (cDirectoryItem)directoryList.Rows[e.RowIndex].DataBoundItem;
                if (ChoseMode)
                {
                    SelectedItem = item;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    currentDir.OpenItem(item);
                    bindingSource.ResetBindings(false);

                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (directoryList.CurrentRow?.DataBoundItem is cDirectoryItem selectedItem)
            {
                var form = new formDirectoryItem();
                form.SetData(selectedItem, currentDir.GetFieldDefinitions());
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    bindingSource.ResetBindings(false);
                }
            }
            else
            {
                MessageBox.Show("Виберіть елемент для редагування.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public cDirectoryItem GetSelectedItem()
        {
            return SelectedItem;
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (directoryList.CurrentRow?.DataBoundItem is cDirectoryItem selectedItem)
            {
                var result = MessageBox.Show(
                    $"Видалити елемент \"{selectedItem.Name}\"?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cDataBase.DB.DeleteItem(currentDir.GetTypeItem(), selectedItem);
                    bindingSource.ResetBindings(false);
                }
            }
            else
            {
                MessageBox.Show("Оберіть елемент довідника!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentDir == null) return;

            var filtered = currentDir.Search(textBoxSearch.Text);
            bindingSource.DataSource = filtered;
            bindingSource.ResetBindings(false);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Filter = "Text files|*.txt"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                currentDir?.ImportFromFile(dlg.FileName);
                bindingSource.ResetBindings(false);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Зберегти у Excel",
                FileName = "Export.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExportToExcel(sfd.FileName);
            }
        }

        private void ExportToExcel(string filePath)
        {
            var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add("Довідник");

            for (int col = 0; col < directoryList.Columns.Count; col++)
            {
                if (directoryList.Columns[col].HeaderText == "Зображення") continue;
                ws.Cell(1, col + 1).Value = directoryList.Columns[col].HeaderText;
            }

            for (int row = 0; row < directoryList.Rows.Count; row++)
            {
                for (int col = 0; col < directoryList.Columns.Count; col++)
                {
                    if (directoryList.Columns[col].HeaderText == "Зображення") continue;
                    var val = directoryList.Rows[row].Cells[col].Value;
                    ws.Cell(row + 2, col + 1).Value = val?.ToString();
                }
            }

            wb.SaveAs(filePath);
            MessageBox.Show("Експорт завершено успішно!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void directoryList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var column = directoryList.Columns[e.ColumnIndex];
            var propertyName = column.DataPropertyName;

            if (string.IsNullOrEmpty(propertyName) || originalItems.Count == 0)
                return;

            var itemType = currentDir.GetType().GetGenericArguments()[0]; // тип T
            var prop = itemType.GetProperty(propertyName);
            if (prop == null) return;

            //додаткові
            var sorted = sortAscending || lastSortedColumn != propertyName
                ? originalItems.OrderBy(i =>
                {
                    var val = prop.GetValue(i);
                    return val is cDirectoryItem refItem ? refItem.Name : val;
                })
                : originalItems.OrderByDescending(i =>
                {
                    var val = prop.GetValue(i);
                    return val is cDirectoryItem refItem ? refItem.Name : val;
                });

            sortAscending = lastSortedColumn == propertyName ? !sortAscending : false;
            lastSortedColumn = propertyName;

            var castedList = typeof(Enumerable).GetMethod("Cast")!.MakeGenericMethod(itemType).Invoke(null, new object[] { sorted });
            var toList = typeof(Enumerable).GetMethod("ToList")!.MakeGenericMethod(itemType).Invoke(null, new object[] { castedList });

            bindingSource.DataSource = toList;
            bindingSource.ResetBindings(false);
        }

        private void buttonTempl_Click(object sender, EventArgs e)
        {
            var fields = currentDir?.GetFieldDefinitions();
            if (fields == null) return;

            var header = string.Join("|", fields.Select(f => f.Name));

            Clipboard.SetText(header);
            MessageBox.Show("Шаблон скопійовано в буфер обміну. Вставте його в текстовий файл", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
