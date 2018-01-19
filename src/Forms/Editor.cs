using DataMaker.Parsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMaker.Forms
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();

            DarkTheme.Initialize(this);
        }

        public void SetEditor(string path)
        {
            var content = File.ReadAllText(path);
            frameParser1.SetParser(content);
        }
    }
}
