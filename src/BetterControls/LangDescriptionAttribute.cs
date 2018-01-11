using System.ComponentModel;
using static DataMaker.Tools;

namespace DataMaker.BetterControls
{
    class LangDescriptionAttribute : DescriptionAttribute
    {
        public LangDescriptionAttribute(string displayName) 
            : base(displayName) { }

        public override string Description => Lang(base.Description);
    }
}
