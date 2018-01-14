using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace DataMaker.Forms
{
    public partial class ArrayEditor : Form, IEditor
    {
        private object[] editedObject;
        public object[] EditedObject
        {
            get => editedObject;
            set
            {
                editedObject = value;
                listValues.Items.Clear();

                foreach (var i in editedObject)
                {
                    listValues.Items.Add(i);
                }
            }
        }

        public ArrayEditor()
        {
            InitializeComponent();
            SetTheme();
        }

        private void SetTheme()
        {
            DarkTheme.Initialize(this);
            lblTitle.Font = new Font(Font.FontFamily, 18f);
        }

        private void btnSubmit_Click(object sender, System.EventArgs e)
        {
            editedObject = listValues.Items.Cast<string>().ToArray();
            Close();
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            listValues.Items.Add("new");
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (listValues.SelectedItem != null)
                listValues.Items.Remove(listValues.SelectedItem);
        }

        private void listValues_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listValues.SelectedItem != null)
            {
                propertyGrid.SelectedObject = listValues.SelectedItem;
                txtKeyName.Text = listValues.SelectedItem.ToString();
            }
        }

        private void txtKeyName_TextChanged(object sender, System.EventArgs e)
        {
            listValues.Items[listValues.SelectedIndex] = txtKeyName.Text;
        }
    }
}
