using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalClassLibrary.SQL;

namespace SiteParse.Communication.SqlManager
{
    public class SqlMethods: SqlMethodsBase
    {
        public static List<Dictionary<string, string>> GetTestSql()
        {
            return SQL("select * from Tags");
        }
    }
}
