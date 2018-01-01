using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DataMaker
{
    static class Program
    {
        public static MainForm FrmMain;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmMain = new MainForm();
            Application.Run(FrmMain);
        }
    }
}
