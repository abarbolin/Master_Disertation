#region Using
using System;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
#endregion

namespace Tools
{
    /// <summary>
    /// Результат в виде JSON.
    /// </summary>
    public class JsonNetResult : ActionResult
    {
        /// <summary>
        /// Конструктор по-умолчанию.
        /// </summary>
        public JsonNetResult()
        {
            Formatting = Formatting.Indented;
            SerializerSettings = new JsonSerializerSettings();
            SerializerSettings.Converters.Add(new IsoDateTimeConverter
            {
                DateTimeFormat = DefaultDateTimeFormat
            });
        }

        /// <summary>
        /// Конструктор на основе данных.
        /// </summary>
        /// <param name="data">Данные для сериализации.</param>
        public JsonNetResult(object data)
            : this()
        {
            Data = data;
        }

        /// <summary>
        /// Данные для сериализации.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Тип данных - application/json;
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Кодировка.
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Настройки форматирования.
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Настроки сериализации.
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; }

        /// <summary>
        /// Код результата.
        /// </summary>
        public HttpStatusCode? Status { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ?
                ContentType : "application/json; charset=utf-8";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Status != null)
                response.StatusCode = (int)Status.Value;

            if (Data == null)
                return;

            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            var serializer = JsonSerializer.Create(SerializerSettings);
            serializer.Serialize(writer, Data);

            writer.Flush();
        }

        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
    }
}
