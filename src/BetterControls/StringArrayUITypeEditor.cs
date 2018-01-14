using DataMaker.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DataMaker.BetterControls
{
    class StringArrayUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            => UITypeEditorEditStyle.Modal;

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            if ((IWindowsFormsEditorService)
                provider.GetService(typeof(IWindowsFormsEditorService))
                != null)
            {
                var editor = new StringArrayEditor()
                {
                    EditedObject = (string[])value
                };
                MainForm.GetInstance().AddEditor(editor);
                MainForm.GetInstance().RemoveEditor(editor);
                return editor.EditedObject;
            }

            return value;
        }
    }
}
