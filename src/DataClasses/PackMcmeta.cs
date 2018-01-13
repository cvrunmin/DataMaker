using DataMaker.BetterControls;
using Newtonsoft.Json;
using System.ComponentModel;

namespace DataMaker.DataClasses
{
    public class PackMcmeta: DataClass
    {
        [JsonProperty(PropertyName = "pack")]
        [LangDisplayName("packmcmeta_pack"), LangDescription("packmcmeta_pack")]
        public Pack Pack { get; set; } = new Pack();
    }
    
    public class Pack: DataClass
    {
        [JsonProperty(PropertyName = "pack_format")]
        [LangDisplayName("pack_packformat"), LangDescription("pack_packformat")]
        public int PackFormat { get; set; } = 3;

        [JsonProperty(PropertyName = "description")]
        [LangDisplayName("pack_description"), LangDescription("pack_description")]
        public string Description { get; set; } = "";
    }
}
