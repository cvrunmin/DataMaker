using System.Reflection;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class FieldsEditor : Form
    {
        public FieldsEditor()
        {
            InitializeComponent();
            SetTheme();

            propertyGrid.PropertySort = PropertySort.Alphabetical;

            // 设置propertyGrid的description zone大小
            // 参考 http://karmian.org/net-tips-tricks-examples-articles/how-resize-description-area-propertygrid.
            foreach (Control i in propertyGrid.Controls)
                if (i.GetType().Name == "DocComment")
                {
                    var fieldInfo = i.GetType().BaseType.GetField(
                        "userSized",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    fieldInfo.SetValue(i, true);
                    i.Height = 200;
                    return;
                }
        }

        public void SelectObject()
        {
            //propertyGrid.SelectedObject = MainForm.GetInstance().EditedDataClass;
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
        private static FieldsEditor propertyEditor;

        /// <summary>
        /// 获取 <see cref="FieldsEditor"/> 的唯一实例
        /// </summary>
        public static FieldsEditor GetInstance()
        {
            if (propertyEditor == null)
                propertyEditor = new FieldsEditor();

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
