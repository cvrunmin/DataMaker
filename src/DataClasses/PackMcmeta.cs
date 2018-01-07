using Newtonsoft.Json;
using System.ComponentModel;
using static DataMaker.Tools;

namespace DataMaker.DataClasses
{
    public class PackMcmeta: DataClass
    {
        [JsonProperty(PropertyName = "pack")]
        public Pack Pack { get; set; } = new Pack();
    }

    [TypeConverter(typeof(ExpandableObjectNoStringConverter))]
    public class Pack
    {
        [JsonProperty(PropertyName = "pack_format")]
        public int PackFormat { get; set; } = 1;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return SerializeObjectToJson(this);
        }
    }
}
