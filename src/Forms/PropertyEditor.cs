using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class PropertyEditor : Form
    {
        public PropertyEditor()
        {
            InitializeComponent();
            SetTheme();

            propertyGrid.PropertySort = PropertySort.Alphabetical;
        }

        public void SelectObject()
        {
            propertyGrid.SelectedObject = MainForm.GetInstance().EditedDataClass;
        }

        public void UpdateContent()
        {
            propertyGrid.Refresh();
        }

        private void SetTheme()
        {
            Theme.Initialize(this);
            lblSizeChanger.BackColor = Theme.HoverColor;
        }

        #region 修改窗体大小
        private int initialX;
        private bool isResizing;

        private void lblSizeChanger_MouseDown(object sender, MouseEventArgs e)
        {
            initialX = e.X;
            isResizing = true;
        }

        private void lblSizeChanger_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                Width += initialX + e.X;
                Width = System.Math.Max(MinimumSize.Width, Width);
                propertyGrid.Width = ClientSize.Width - lblSizeChanger.Width;
            }
        }

        private void lblSizeChanger_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
        }
        #endregion

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MainForm.GetInstance().RawEditor.UpdateContent();
        }
    }
}
