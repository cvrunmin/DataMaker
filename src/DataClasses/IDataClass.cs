using System;
using System.ComponentModel;
using System.Globalization;
using static DataMaker.Tools;

namespace DataMaker.DataClasses
{
    public interface IDataClass
    {
    }

    public class JsonObjectConventer<T> : ExpandableObjectConverter
    {
        public override bool CanConvertTo
            (ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(T))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo
            (ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(String) &&
                value is T)
            {
                return SerializeToJson((T)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
