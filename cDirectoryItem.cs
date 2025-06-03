
namespace CW
{
    public class cDirectoryItem
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public cDirectoryItem(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
            {
                new cFieldDefinition { Name = "Code", Type = FieldType.String },
                new cFieldDefinition { Name = "Name", Type = FieldType.String },
            };
        }

        public override string ToString() => Name;
    }
}
