using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Tools.SQL;

namespace Tools.Communication.SqlManager
{
    public class SqlMethods : SqlMethodsBase
    {

        public static List<Dictionary<string, string>> GetSites()
        {
            return SQL("select id, url from Pages");
        }

        /// <summary>
        /// Метод получения тэгов
        /// </summary>
        /// <returns>Список тэгов</returns>
        public static List<Dictionary<string, string>> GetTags()
        {
            return SQL("select name from Tags");
        }

        public static int GetHostId(string hostName)
        {
            if (hostName == "") return 0;
            var sb = new StringBuilder();
            int idHost;
            //Если есть, то достаём IdHost
            sb.AppendLine(" SELECT TOP 1");
            sb.AppendLine("h.id");
            sb.AppendLine("FROM");
            sb.AppendLine("Hosts AS h");
            sb.AppendLine("WHERE h.name =@0");
            try
            {
                    
                    idHost = Convert.ToInt32(GetScalar(sb.ToString(), hostName));

                    sb.Clear();
                    sb.AppendLine(" UPDATE Hosts");
                    sb.AppendLine(" SET count_view = count_view+1");
                    sb.AppendLine(" WHERE id =@0");

                Exec(sb.ToString(), idHost.ToString());
            }
            catch (Exception)
            {
                idHost = 0;
            }
            if (idHost > 0)
            {
                return idHost;
            }

            sb.Clear();
            sb.AppendLine(" INSERT INTO Hosts");
            sb.AppendLine("(");
            sb.AppendLine("name,");
            sb.AppendLine("count_view");
            sb.AppendLine(")");
            sb.AppendLine("OUTPUT INSERTED.id");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("	@0,");
            sb.AppendLine("	1 ");
            sb.AppendLine(")");

            idHost = Convert.ToInt32(GetScalar(sb.ToString(), hostName));

            return idHost;
        }
        /// <summary>
        /// Добавляем информацию о странице в БД
        /// </summary>
        /// <param name="url">Адрес страницы</param>
        /// <param name="lemmCount">Кол-во лемм</param>
        /// <param name="hostName">Имя хоста</param>
        /// <param name="idUser">Идентификатор пользователя</param>
        /// <returns>Идентификатор страницы </returns>
        public static object AddPage(string url, int lemmCount, int idUser,string hostName)
        {
            if (idUser <= 0) return -1;
            int idPage;
            var sb = new StringBuilder();
            //ПОлучаем id хоста и если такой есть, то обновляем кол-во просмотров
            var idHost = GetHostId(hostName);
            if (idHost > 0)
            {
                //Если пользователь уже есть, тогда смотрим, посещал он эту страницу уже или нет
                var idUserPage = ExistsUserPage(idUser, url);

                if (idUserPage > 0)
                {
                    sb.Clear();
                    sb.AppendLine(" SELECT uap.id_page ");
                    sb.AppendLine("FROM user_and_pages AS uap ");
                    sb.AppendLine("WHERE uap.id = @0 ");

                    idPage = Convert.ToInt32(GetScalar(sb.ToString(), idUserPage.ToString()));


                    return idPage;
                }


                //Иначе вставляем новую страницу

                sb.Clear();
                sb.AppendLine(" INSERT INTO Pages");
                sb.AppendLine("(");
                sb.AppendLine("[url],");
                sb.AppendLine("count_lemm,");
                sb.AppendLine("id_host");
                sb.AppendLine(")");
                sb.AppendLine("OUTPUT INSERTED.id");
                sb.AppendLine("VALUES");
                sb.AppendLine("(");
                sb.AppendLine("@0,");
                sb.AppendLine("@1,");
                sb.AppendLine("@2");
                sb.AppendLine(")");

                var cnt = lemmCount.ToString(CultureInfo.InvariantCulture);
                var hst = Convert.ToString(idHost > 0 ? idHost : 0);

                idPage = Convert.ToInt32(GetScalar(sb.ToString(), url, cnt, hst));

                InsertToUserAndPages(idUser, idPage);

                return idPage;
            }
            return -1;

        }
        /// <summary>
        /// Проверяем есть ли юзер в БД
        /// </summary>
        /// <returns></returns>
        public static int ExistsUser(string userId)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("TOP 1 u.id");
            sb.AppendLine("FROM");
            sb.AppendLine("Users AS u");
            sb.AppendLine("WHERE u.ip =@0");

            return Convert.ToInt32(GetScalar(sb.ToString(), userId));
        }
        /// <summary>
        /// Получаем нового юзера или достаём id старого
        /// </summary>
        /// <param name="externalUserId"></param>
        /// <param name="browserInfo"></param>
        /// <returns></returns>
        public static int GetUserId(string externalUserId, string browserInfo)
        {
            var userId = ExistsUser(externalUserId);
            return userId <= 0 ? Convert.ToInt32(InsertNewUser(externalUserId, browserInfo)) : userId;
        }

        public static object AddInfoFromPlugin(string externalUserId, string url, string browserInfo,string hostName)
        {
            try
            {
                 var userId = GetUserId(externalUserId, browserInfo); 
                 AddPage(url, 0, userId,hostName); 
    
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
            return "Success";

        }
        /// <summary>
        /// Метод проверяет есть ли страница у пользователя, 
        /// если есть, то обновляет кол-во просмотров
        /// </summary>
        /// <returns></returns>
        public static int ExistsUserPage(int idUser, string url)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" SELECT TOP 1 uap.id FROM Users AS u ");
            sb.AppendLine("INNER JOIN user_and_pages AS uap");
            sb.AppendLine("  ON uap.id_user = u.id");
            sb.AppendLine("INNER JOIN Pages AS p");
            sb.AppendLine(" ON p.id = uap.id_page");
            sb.AppendLine("WHERE p.url =@1 AND uap.id_user = @0");

            var exists = Convert.ToInt32(GetScalar(sb.ToString(), idUser.ToString(), url));

            if (exists <= 0) return exists;

            sb.Clear();
            sb.AppendLine(" UPDATE user_and_pages");
            sb.AppendLine(" SET count_view = count_view+1");
            sb.AppendLine(" WHERE id =@0");
            Exec(sb.ToString(), exists.ToString());

            return exists;
        }
        /// <summary>
        /// Добавляем данные в таблицу-связку юзеров и страниц
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="idPage"></param>
        public static void InsertToUserAndPages(int idUser, int idPage)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" INSERT INTO user_and_pages");
            sb.AppendLine("(");
            sb.AppendLine("id_user,");
            sb.AppendLine("id_page,");
            sb.AppendLine("count_view");
            sb.AppendLine(")");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("	@0,");
            sb.AppendLine("	@1,");
            sb.AppendLine(" 1 ");
            sb.AppendLine(")");

            Exec(sb.ToString(), idUser.ToString(), idPage.ToString());
        }

        /// <summary>
        /// Добавляем нового пользователя
        /// </summary>
        /// <param name="userId">Внешний идентификатор пользователя</param>
        /// <param name="browserInfo"> Информация о браузере </param>
        /// <returns></returns>
        public static object InsertNewUser(string userId, string browserInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO Users");
            sb.AppendLine("(");
            sb.AppendLine("browser_info,");
            sb.AppendLine("ip");
            sb.AppendLine(")");
            sb.AppendLine("OUTPUT INSERTED.id");
            sb.AppendLine("VALUES");
            sb.AppendLine("(");
            sb.AppendLine("	@0,");
            sb.AppendLine("	@1");
            sb.AppendLine(")");

            return GetScalar(sb.ToString(), browserInfo, userId);
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

            Exec(sb.ToString(), lemma, pageId.ToString(CultureInfo.InvariantCulture), freq.Replace(',', '.'));
        }
        /// <summary>
        /// Обновляем логин
        /// </summary>
        public static void UpdateLogin(string login,string ip)
        {
            var sb = new StringBuilder();

            sb.AppendLine(" UPDATE Users ");
            sb.AppendLine(" SET [login]=@0");
            sb.AppendLine(" WHERE ip=@1");

            Exec(sb.ToString(), login, ip);

        }
        /// <summary>
        /// Возвращает всех пользователей
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetAllUsers()
        {
            return SQL("SELECT u.id FROM Users u");
        }
        /// <summary>
        /// Возвращает странички пользователя и кол-во лемм найденных на каждой странице
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetUserPagesAndCountLemm(int userId)
        {
            var sb = new StringBuilder();
            sb.AppendLine(" SELECT p.URL,");
            sb.AppendLine(" p.count_lemm");
            sb.AppendLine("FROM Pages AS p");
            sb.AppendLine("INNER JOIN user_and_pages AS uap ON (uap.id_page = p.id)");
            sb.AppendLine("WHERE uap.id_user = @0 ");

            return SQL(sb.ToString(), userId.ToString());
        }


          
        /// <summary>
        /// Получаем логин юзера по ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetUserLogin(string ip)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT u.[login] FROM Users u WHERE u.ip=@0");

            return GetScalar(sb.ToString(), ip).ToString();

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

            return (int)GetScalar(sb.ToString(), url);
        }

        public static List<Dictionary<string, string>> GetVectorForPage(int idPage1, int idPage2)
        {

            var command = new SqlCommand("GetVectorForPages") { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@idPage1", idPage1);
            command.Parameters.AddWithValue("@idPage2", idPage2);
            return SQLByCommand(command);
        }
    }
}
