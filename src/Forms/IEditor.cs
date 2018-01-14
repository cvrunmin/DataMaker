using System.Collections;
using System.Collections.Generic;

namespace DataMaker.Forms
{
    public interface IEditor
    {
        object[] EditedObject { get; set; }
    }
}
