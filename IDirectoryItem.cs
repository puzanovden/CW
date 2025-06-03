public interface IDirectoryItem
{
    string Code { get; set; }
    string Name { get; set; }

    static abstract List<cFieldDefinition> GetFieldDefinitions();
}