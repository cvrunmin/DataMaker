using DataMaker.DataClasses;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class StringArrayEditor : Form, IEditor
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

        public StringArrayEditor()
        {
            InitializeComponent();
            SetTheme();

            for (int i = 65; i < 65 + 26; i++)
                for (int j = 65; j < 65 + 26; j++)
                    betterComboBox1.Items.Add(char.ConvertFromUtf32(i) + char.ConvertFromUtf32(j));
        }

        private void SetTheme()
        {
            DarkTheme.Initialize(this);
            lblTitle.Font = new Font(Font.FontFamily, 18f);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            editedObject = listValues.Items.Cast<string>().ToArray();
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //var instance = Assembly.GetExecutingAssembly().CreateInstance(EditedObject[0].GetType().Name);

            listValues.SelectedIndex = listValues.Items.Add("");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var index = listValues.SelectedIndex;
            if (listValues.SelectedItem != null)
                listValues.Items.Remove(listValues.SelectedItem);
            listValues.SelectedIndex = Math.Max(index - 1, -1);
        }

        private void listValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listValues.SelectedItem != null)
            {
                propertyGrid.SelectedObject = listValues.SelectedItem;
                txtKeyName.Text = listValues.SelectedItem.ToString();
            }
        }

        private void txtKeyName_TextChanged(object sender, EventArgs e)
        {
            if (listValues.SelectedIndex >= 0)
                listValues.Items[listValues.SelectedIndex] = txtKeyName.Text;
        }
    }
}
