

namespace CW
{
    public class cAnalogItem : cDirectoryItem, IDirectoryItem
    {
        public cPartsItem Part { get; set; }
        public cPartsItem AnalogPart { get; set; }


        public string Image { get; set; }
        public static List<cFieldDefinition> GetFieldDefinitions()
        {
            return new List<cFieldDefinition>
             {
                new cFieldDefinition { Name = "Code", Type = FieldType.String, PublicName = "Код" },
                new cFieldDefinition { Name = "Name", Type = FieldType.String, PublicName = "Найменування" },
                new cFieldDefinition { Name = "Part", Type = FieldType.Reference, PublicName = "Запчастина", ReferenceName = "Parts"},
                new cFieldDefinition { Name = "AnalogPart", Type = FieldType.Reference, PublicName = "Аналог", ReferenceName = "Parts"},

             };
        }

        public cAnalogItem() : base("", "") { }
        public cAnalogItem(string code, string name) : base(code, name) { }

    }
}



