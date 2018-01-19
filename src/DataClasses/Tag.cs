using DataMaker.BetterControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace DataMaker.DataClasses
{
    [Obsolete("Fuck hardcode.", true)]
    class Tag : DataClass
    {
        [JsonProperty(PropertyName = "replace")]
        [LangDisplayName("tag_replace"), LangDescription("tag_replace")]
        public bool Replace { get; set; } = false;

        [JsonProperty(PropertyName = "values")]
        [LangDisplayName("tag_values"), LangDescription("tag_values")]
        //[Editor(typeof(StringArrayUITypeEditor), typeof(UITypeEditor))]
        public string[] Values { get; set; }
    }
}
