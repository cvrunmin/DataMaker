using System;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace DataMaker.BetterControls
{
    class InfoPropertyTab : PropertyTab
    {
        public override string TabName => "lalala";

        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            throw new NotImplementedException();
        }
    }
}
