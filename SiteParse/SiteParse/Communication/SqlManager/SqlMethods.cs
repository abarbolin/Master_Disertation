using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalClassLibrary.SQL;

namespace SiteParse.Communication.SqlManager
{
    public class SqlMethods : SqlMethodsBase
    {
        public static List<Dictionary<string, string>> GetTags()
        {
            return SQL("select name from Tags");
        }


        public static object AddPage(string url, int lemmCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" INSERT INTO Pages");
            sb.AppendLine("(");
            sb.AppendLine("[url],");
            sb.AppendLine("count_lemm");
            sb.AppendLine(")");
            sb.AppendLine("OUTPUT INSERTED.id");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("@0,");
            sb.AppendLine("@1");
            sb.AppendLine(")");

            return GetScalar(sb.ToString(), url, lemmCount.ToString());
        }


        public static void AddWord(string lemma, int pageId, string freq)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" INSERT INTO Words");
            sb.AppendLine("(");
            sb.AppendLine("word,");
            sb.AppendLine("id_page,");
            sb.AppendLine("frequency");
            sb.AppendLine(" )");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("@0,");
            sb.AppendLine("@1,");
            sb.AppendLine("@2");
            sb.AppendLine(") ");

            Exec(sb.ToString(), lemma, pageId.ToString(), freq.Replace(',','.'));
        }
    }
}
