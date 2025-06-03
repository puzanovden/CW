public enum FieldType
{
    String,
    Number,
    Image,
    Reference
}

public class cFieldDefinition
{
    public string Name { get; set; }        
    public FieldType Type { get; set; }      
    public string? ReferenceName { get; set; }
    public string PublicName { get; set; }

}
