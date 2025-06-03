

namespace CW
{
    public class cManufacturerItem : cDirectoryItem, IDirectoryItem
    {

        public string Image { get; set; }
        public сCountriesItem Countries { get; set; }

        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
             {
                new cFieldDefinition { Name = "Code", Type = FieldType.String, PublicName = "Код" },
                new cFieldDefinition { Name = "Name", Type = FieldType.String, PublicName = "Найменування" },
                new cFieldDefinition { Name = "Countries", Type = FieldType.Reference, PublicName = "Країна", ReferenceName = "Countries" },
                new cFieldDefinition { Name = "Image", Type = FieldType.Image, PublicName = "Зображення" }
             };
        }

        public cManufacturerItem() : base("", "") { }
        public cManufacturerItem(string code, string name) : base(code, name) { }

    }
}



