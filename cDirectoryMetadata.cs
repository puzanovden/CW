namespace CW
{
    public class cDirectoryMetadata
    {
        public string Name { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public IDirectory Directory { get; set; } = null!;
        public Type ItemType { get; set; } = null!;
    }
}
