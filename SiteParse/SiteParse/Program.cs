using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalClassLibrary.SQL;
using Microsoft.SqlServer.Server;
using System.Configuration;

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
            SqlMethodsBase.ConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ToString();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParseForm());
        }
    }
}
