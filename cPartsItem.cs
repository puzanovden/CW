

namespace CW
{
    public class cPartsItem : cDirectoryItem, IDirectoryItem
    {
        public string Description { get; set; }
        public string PartCode { get; set; }
        public float Price { get; set; }
        public cModelsItem Model { get; set; }
        public cManufacturerItem Manufacturer { get; set; }

        public string Image { get; set; }
        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
             {
                new cFieldDefinition { Name = "Code", Type = FieldType.String, PublicName = "Код" },
                new cFieldDefinition { Name = "Name", Type = FieldType.String, PublicName = "Найменування" },
                new cFieldDefinition { Name = "PartCode", Type = FieldType.String, PublicName = "Код запчастини" },
                new cFieldDefinition { Name = "Description", Type = FieldType.String, PublicName = "Опис" },
                new cFieldDefinition { Name = "Price", Type = FieldType.Number, PublicName = "Ціна" },
                new cFieldDefinition { Name = "Model", Type = FieldType.Reference, PublicName = "Модель", ReferenceName = "Models"},
                new cFieldDefinition { Name = "Image", Type = FieldType.Image, PublicName = "Зображення"},
                new cFieldDefinition { Name = "Manufacturer", Type = FieldType.Reference, PublicName = "Виробник", ReferenceName = "Manufactures"}
             };
        }

        public cPartsItem() : base("", "") { }
        public cPartsItem(string code, string name) : base(code, name) { }

    }
}



