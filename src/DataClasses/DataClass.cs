using System;
using System.ComponentModel;
using static DataMaker.Tools;

namespace DataMaker.DataClasses
{
    [TypeConverter(typeof(ExpandableObjectNoStringConverter))]
    public class DataClass
    {
        public override string ToString() => SerializeObjectToJson(this);
    }

    /// <summary>
    /// 不允许被转换为String，或从String转换
    /// 这样可以让 Json.Net 正确序列化
    /// </summary>
    public class ExpandableObjectNoStringConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return false;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return false;
            else
                return base.CanConvertTo(context, destinationType);
        }
    }
}
