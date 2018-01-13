using DataMaker.BetterControls;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataMaker.DataClasses
{
    class Tag : DataClass
    {
        [JsonProperty(PropertyName = "replace")]
        [LangDisplayName("tag_replace"), LangDescription("tag_replace")]
        public bool Replace { get; set; } = false;

        [JsonProperty(PropertyName = "values")]
        [LangDisplayName("tag_values"), LangDescription("tag_values")]
        public string[] Values { get; set; }
    }
}
