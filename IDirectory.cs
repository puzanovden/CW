namespace CW
{
    public interface IDirectory
    {
        void AddItem(string name);
        void OpenItem(cDirectoryItem item);
        bool CreateAndEditItem(Form owner);
        Type GetTypeItem();
        IEnumerable<cDirectoryItem> GetItems();
        cDirectoryItem GetOrAddByName(string name);
        List<cFieldDefinition> GetFieldDefinitions();
        IEnumerable<cDirectoryItem> Search(string query, string[]? fields = null);
        void ImportFromFile(string path);
        void DeleteItem(cDirectoryItem itemToDelete);
    }
}