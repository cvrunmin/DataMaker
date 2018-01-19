using System;
using System.Collections;
using System.Collections.Generic;

namespace DataMaker.Forms
{
    [Obsolete("Fuck hardcode.", true)]
    public interface IEditor
    {
        object[] EditedObject { get; set; }
    }
}
