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

        private void SetTheme()
        {
            DarkTheme.Initialize(this);
            lblSizeChanger.BackColor = DarkTheme.HoverColor;
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
        
        #region 单例模式
        private static PropertyEditor propertyEditor;

        /// <summary>
        /// 获取 <see cref="PropertyEditor"/> 的唯一实例
        /// </summary>
        public static PropertyEditor GetInstance()
        {
            if (propertyEditor == null)
                propertyEditor = new PropertyEditor();

            return propertyEditor;
        }
        #endregion

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MainForm.GetInstance().IsChanged = true;
            propertyGrid.Refresh();
        }
    }
}
