using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Communication.SqlManager;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        public ParseForm()
        {
            InitializeComponent();
        }

        private void ParseForm_Load(object sender, EventArgs e)
        {
            var s = SqlMethods.GetTestSql();
        }
    }
}
