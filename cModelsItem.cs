namespace CW
{
    public class cModelsItem : cDirectoryItem, IDirectoryItem
    {
        public сCountriesItem Counrty { get; set; }
        public cModelsItem() : base("", "") { }
        //public string Image { get; set; }

        public cModelsItem(string code, string name) : base(code, name) { }
        

        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
             {
                new cFieldDefinition { Name = "Code", Type = FieldType.String, PublicName = "Код" },
                new cFieldDefinition { Name = "Name", Type = FieldType.String, PublicName = "Найменування" },
                new cFieldDefinition { Name = "Counrty", Type = FieldType.Reference, PublicName = "Країна", ReferenceName = "Countries" },
              //  new cFieldDefinition { Name = "Image", Type = FieldType.Image, PublicName = "Зображення" }
             };
        }

    }
}
