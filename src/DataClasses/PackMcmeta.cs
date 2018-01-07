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
    public class Pack: DataClass
    {
        [JsonProperty(PropertyName = "pack_format")]
        public int PackFormat { get; set; } = 1;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "";
    }
}
