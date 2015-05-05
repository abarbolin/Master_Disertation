using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Models
{
    public class UserInfoModel
    {

        /// <summary>
        /// Ссылка на которую преешел опльзователь
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Внешний идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ИНформация о браузере
        /// </summary>
        public string BrowserType { get; set; }

      
        /// <summary>
        /// Ссылка на хост
        /// </summary>
        public string HostUrl { get; set; }

    }      
}
