using System;
using System.Windows.Forms;
using System.Configuration;
using Tools.SQL;

namespace SiteParse
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SqlMethodsBase.ConnectString = ConfigurationManager.ConnectionStrings["connString"].ToString();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParseForm(){StartPosition = FormStartPosition.CenterScreen});
        }
    }
}
