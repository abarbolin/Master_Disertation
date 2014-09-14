using System.Collections.Generic;
using System.Data.SqlClient;

namespace ExternalClassLibrary.SQL
{
    public class SqlMethodsBase
    {
        /// <summary>
        /// Открывает соединение и возвращает SqlConnection
        /// </summary>
        /// <returns></returns>
        protected static SqlConnection Connect()
        {
            var connect = new SqlConnection(ConnectString);
            connect.Open();
            return connect;
        }

        /// <summary>
        /// Формирует SqlCommand с параметрами
        /// </summary>
        /// <param name="sql">sql запрос с параметрами, например "select * from table where id = @0"</param>
        /// <param name="connection">Открытый SqlConnection</param>
        /// <param name="p">Параметры, которые подставляются вместо @0,@1,..</param>
        /// <returns></returns>
        private static SqlCommand RetutnCommand(string sql, SqlConnection connection, params string[] p)
        {
            var cmd = new SqlCommand(sql,
                connection);
            for (int i = 0; i < p.Length; i++)
                cmd.Parameters.AddWithValue("@" + i, p[i]);
            return cmd;
        }

        /// <summary>
        /// Возвращает список словарей вида key;value
        /// Где key - имя стобца, value - его значение
        /// Каждый элемент списка - строка таблицы, вернувшейся по запросу sql
        /// </summary>
        /// <param name="sql">sql запрос с параметрами, например "select * from table where id = @0"</param>
        /// <param name="p">Параметры, которые подставляются вместо @0,@1,..</param>
        /// <returns></returns>
        protected static List<Dictionary<string, string>> SQL(string sql, params string[] p)
        {
            using (var connection = Connect())
            {
                return GetDictionary(RetutnCommand(sql, connection, p));
            }
        }


        protected static Dictionary<string, Dictionary<string, string>> SQLbyColumn(string sql, string column, params string[] p)
        {
            using (var connection = Connect())
            {
                return GetDictionary(RetutnCommand(sql, connection, p), column);
            }
        }

        /// <summary>
        /// Вместо запроса - выполнение команды типа delete, update с параметрами
        /// </summary>
        /// <param name="sql">sql запрос с параметрами, например "delete * from table where id = @0"</param>
        /// <param name="p">Параметры, которые подставляются вместо @0,@1,..</param>
        protected static void Exec(string sql, params string[] p)
        {
            using (var connection = Connect())
            {
                RetutnCommand(sql, connection, p).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Возвращение единственного значения
        /// </summary>
        /// <param name="sql">sql запрос с параметрами, например "select top 1 name from table where id = @0"</param>
        /// <param name="p">Параметры, которые подставляются вместо @0,@1,..</param>
        /// <returns></returns>
        protected static object GetScalar(string sql, params string[] p)
        {
            using (var connection = Connect())
            {
                return RetutnCommand(sql, connection, p).ExecuteScalar();
            }
        }

        /// <summary>
        /// Вытаскиваем словарь словарей с доступом по column
        /// Словарь вида key;key,value
        /// Например преобразование code элемента в id
        /// По code элемента вытаскиваем id, то есть получается code;id,value
        /// Где code - число, id - строка-ключ, по которой вытаскиваем идентификатор из value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, string>> GetDictionary(SqlCommand command, string column)
        {
            var dictList = new Dictionary<string, Dictionary<string, string>>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dict = new Dictionary<string, string>();
                    // Собираем словарь поля со значениями
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dict.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                    }
                    // Добавляем в наш словарь словарей - словарь с ключем равным значению в column
                    dictList[dict[column]] = dict;
                }

                return dictList;
            }
        }

        /// <summary>
        /// Возвращаем список словарей, где каждый элемент списка это строка sql запроса
        /// Словарь вида key;value
        /// Где key - название столбца, value - значение
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static List<Dictionary<string, string>> GetDictionary(SqlCommand command)
        {
            var dictList = new List<Dictionary<string, string>>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dict = new Dictionary<string, string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dict.Add(reader.GetName(i), reader[reader.GetName(i)].ToString());
                    }
                    dictList.Add(dict);
                }

                return dictList;
            }
        }

        /// <summary>
        /// Удаление спец. символов на уровне sql
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isAlias"></param>
        /// <returns></returns>
        protected static string ClearSpecialChars(string element, bool isAlias)
        {
            string strOut = "REPLACE(REPLACE(REPLACE(REPLACE(" + element + ",CHAR(13),''),CHAR(10), ' '),'\\','\\\\'),'\"','\\\"')";
            if (!isAlias)
            {
                strOut += " as " + element;
            }
            return strOut;
        }


        public static string ConnectString { get; set; }

    }
}