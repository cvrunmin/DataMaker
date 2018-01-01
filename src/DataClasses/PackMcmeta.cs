using Newtonsoft.Json;
using System.ComponentModel;

namespace DataMaker.DataClasses
{
    public class PackMcmeta
    {
        [JsonProperty(PropertyName = "pack")]
        public Pack Pack = new Pack();
    }

    public class Pack
    {
        [JsonProperty(PropertyName = "pack_format")]
        public int PackFormat = 1;

        [JsonProperty(PropertyName = "description")]
        public string Description = "";
    }
}
