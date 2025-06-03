
namespace CW
{
    public class сCountriesItem : cDirectoryItem, IDirectoryItem
    {
        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
             {
                new cFieldDefinition { Name = "Code", Type = FieldType.String, PublicName = "Код" },
                new cFieldDefinition { Name = "Name", Type = FieldType.String, PublicName = "Найменування" }
             };
        }

        public сCountriesItem() : base("", "") { }
        public сCountriesItem(string code, string name) : base(code, name) { }

    }
}
