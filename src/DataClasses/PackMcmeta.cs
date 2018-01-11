using DataMaker.BetterControls;
using Newtonsoft.Json;
using System.ComponentModel;

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
        [LangDisplayName("displayname_packmcmeta_packformat")]
        [LangDescription("description_packmcmeta_packformat")]
        public int PackFormat { get; set; } = 3;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = "";
    }
}
