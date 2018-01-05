using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class PropertyEditor : Form
    {
        private object selectedObject;

        public PropertyEditor(object obj)
        {
            InitializeComponent();
            SetTheme();

            selectedObject = obj;

            propertyGrid.SelectedObject = selectedObject;
            propertyGrid.PropertySort = PropertySort.Categorized;
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
                Width = System.Math.Max(MinimumSize.Width , Width);
                propertyGrid.Width = ClientSize.Width - lblSizeChanger.Width;
            }
        }

        private void lblSizeChanger_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
        }
        #endregion
    }
}
