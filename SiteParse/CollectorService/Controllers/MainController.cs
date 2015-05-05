#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools;
using Tools.Communication.SqlManager;
using Tools.Models;
#endregion

namespace CollectorService.Controllers
{
    public class MainController : Controller
    {
        /// <summary>
        /// Тест
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult Test()
        {
            return  new JsonNetResult("Test");
        }
        /// <summary>
        /// Добавляем данные из плагина в нашу БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult InsertUserInfo(UserInfoModel model)
        {
            if (model.Url == String.Empty || model.Id == String.Empty || model.BrowserType == String.Empty ||
                model.HostUrl == String.Empty) 
                return new JsonNetResult(SolutionResources.EmptyInputData);

            var result = SqlMethods.AddInfoFromPlugin(model.Id, model.Url, model.BrowserType, model.HostUrl);
            return new JsonNetResult(result);
        }
        [HttpPost]
        public JsonNetResult SetLogin(string login,string ip)
        {
            if (login == String.Empty || ip == String.Empty) 
                return new JsonNetResult(SolutionResources.EmptyLogin);
            SqlMethods.UpdateLogin(login,ip);
            return new JsonNetResult("");
        }
        /// <summary>
        /// Получаем логин юзера по ip
        /// </summary>
        /// <param name="ip">ip адрес</param>
        /// <returns></returns>
        [HttpGet]
        public JsonNetResult GetUserLogin(string ip)
        {
            return ip == String.Empty ? new JsonNetResult(SolutionResources.EmptyIP) : new JsonNetResult(SqlMethods.GetUserLogin(ip));
        }
    }
}