using Newtonsoft.Json;
using System.ComponentModel;

namespace DataMaker.DataClasses
{
    public class PackMcmeta: IDataClass
    {
        [JsonProperty(PropertyName = "pack")]
        public Pack Pack { get; set; } = new Pack();
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Pack
    {
        [JsonProperty(PropertyName = "pack_format")]
        public int PackFormat { get; set; } = 1;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "";
    }
}
