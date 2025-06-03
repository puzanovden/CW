
namespace CW
{
    public class cDataBase
    {
        public static cDataBase DB { get; } = new cDataBase();

        private readonly Dictionary<string, cDirectoryMetadata> directories = new();

        private cDataBase()
        {
            Add("Parts", "Запчастини", new cDirectory<cPartsItem>(), typeof(cPartsItem));
            Add("Models", "Моделі", new cDirectory<cModelsItem>(), typeof(cModelsItem));
            Add("Analogs", "Аналоги", new cDirectory<cAnalogItem>(), typeof(cAnalogItem));
            Add("Manufactures", "Виробники", new cDirectory<cManufacturerItem>(), typeof(cManufacturerItem));
            Add("Countries", "Країни", new cDirectory<сCountriesItem>(), typeof(сCountriesItem));


        }

        private void Add(string name, string displayName, IDirectory directory, Type type)
        {
            directories[name] = new cDirectoryMetadata
            {
                Name = name,
                DisplayName = displayName,
                Directory = directory,
                ItemType = type
            };
        }

        public cDirectoryMetadata? GetMetadata(string name)
            => directories.TryGetValue(name, out var meta) ? meta : null;

        public IDirectory? GetDirectory(string name)
            => GetMetadata(name)?.Directory;

        public IEnumerable<cDirectoryMetadata> GetAll()
            => directories.Values;

        public IDirectory? GetDirectoryByName(string name)
        {
            return GetMetadata(name)?.Directory;
        }

        public void SaveAll(string folderPath)
        {
            foreach (var meta in directories.Values)
            {
                var fileName = Path.Combine(folderPath, $"{meta.Name}.txt");

                var saveMethod = meta.Directory.GetType().GetMethod("SaveToFile");
                saveMethod?.Invoke(meta.Directory, new object[] { fileName });
            }
        }

        public string? GetDisplayName(IDirectory directory)
        {
            return directories.Values
                .FirstOrDefault(meta => meta.Directory == directory)
                ?.DisplayName;
        }

        public void LoadAll(string folderPath)
        {
            foreach (var meta in directories.Values)
            {
                LoadData(meta, folderPath, true);
            }
            foreach (var meta in directories.Values)
            {
                LoadData(meta, folderPath, false);
            }
        }

        private void LoadData(cDirectoryMetadata meta, string folderPath, bool DataOrRef)
        {
            var path = Path.Combine(folderPath, meta.Name + ".txt");

            if (File.Exists(path))
            {
                var method = meta.Directory.GetType().GetMethod("LoadFromFile");
                method?.Invoke(meta.Directory, new object[] { path, DataOrRef });
            }
        }

        public void DeleteItem(Type itemType, cDirectoryItem itemToDelete)
        {
            if (itemToDelete == null || itemType == null)
                return;

            var meta = directories.Values.FirstOrDefault(m => m.ItemType == itemType);
            if (meta == null)
                return;

            var directory = meta.Directory;
            if (directory == null)
                return;

            foreach (var otherMeta in directories.Values)
            {
                if (otherMeta.ItemType == itemType)
                    continue;

                var items = otherMeta.Directory.GetItems();
                var props = otherMeta.ItemType.GetProperties();

                foreach (var otherItem in items)
                {
                    foreach (var prop in props)
                    {
                        if (!typeof(cDirectoryItem).IsAssignableFrom(prop.PropertyType))
                            continue;

                        var refValue = prop.GetValue(otherItem) as cDirectoryItem;
                        if (ReferenceEquals(refValue, itemToDelete))
                        {
                            MessageBox.Show(
                                $"Неможливо видалити \"{itemToDelete.Name}\" — на нього є посилання в довіднику \"{otherMeta.DisplayName}\".",
                                "Видалення заблоковано",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            directory.DeleteItem(itemToDelete);
        }
    }
}