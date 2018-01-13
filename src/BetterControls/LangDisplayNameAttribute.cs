using System.ComponentModel;
using static DataMaker.Tools;

namespace DataMaker.BetterControls
{
    class LangDisplayNameAttribute : DisplayNameAttribute
    {
        public LangDisplayNameAttribute(string displayName) 
            : base(displayName) { }

        public override string DisplayName => Lang("displayname_" + base.DisplayName);
    }
}
