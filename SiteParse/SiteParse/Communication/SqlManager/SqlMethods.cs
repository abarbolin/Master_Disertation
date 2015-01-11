
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using ExternalClassLibrary.SQL;

namespace SiteParse.Communication.SqlManager
{
    public class SqlMethods : SqlMethodsBase
    {
        /// <summary>
        /// Метод получения тэгов
        /// </summary>
        /// <returns>Список тэгов</returns>
        public static List<Dictionary<string, string>> GetTags()
        {
            return SQL("select name from Tags");
        }
        /// <summary>
        /// Добавляем информацию о странице в БД
        /// </summary>
        /// <param name="url">Адрес страницы</param>
        /// <param name="lemmCount">Кол-во лемм</param>
        /// <returns></returns>
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

            return GetScalar(sb.ToString(), url, lemmCount.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// Добавляем лемму для конкретной страницы
        /// </summary>
        /// <param name="lemma">Лемма слова</param>
        /// <param name="pageId">Идентификатор страницы</param>
        /// <param name="freq"> Частота встречи слова</param>
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

            Exec(sb.ToString(), lemma, pageId.ToString(CultureInfo.InvariantCulture), freq.Replace(',','.'));
        }
        /// <summary>
        /// Проверяем есть ли такая url, за текущую дату
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int ExistsPage(string url)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" SELECT COUNT(*) From");
            sb.AppendLine(" Pages pg ");
            sb.AppendLine(" WHERE pg.url = @0");
            sb.AppendLine("AND pg.[date] = CONVERT(varchar, GETDATE(), 102)");

            return  (int)GetScalar(sb.ToString(), url);
        }

        public static List<Dictionary<string,string>> GetVectorForPage(int idPage1, int idPage2)
        {

            var command = new SqlCommand("GetVectorForPages") { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@idPage1", idPage1);
            command.Parameters.AddWithValue("@idPage2", idPage2);
            return SQLByCommand(command);
        }
    }
}
