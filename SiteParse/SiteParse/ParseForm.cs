using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using SiteParse.Communication.SqlManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        private static StringBuilder _textResult;
        private static List<Dictionary<string, string>> _tags;
        private static List<string> _listOfTags;
        private static string encode;
        public ParseForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseForm_Load(object sender, EventArgs e)
        {
            _textResult = new StringBuilder();
            _listOfTags = new List<string>();
            ParseBox.ScrollBars = ScrollBars.Vertical;
        }
        /// <summary>
        /// Событие начала парсинга страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void ParseBtn_Click(object sender, EventArgs e)
        {
            ParseBox.Clear();
            string url = urlBox.Text;
            if (url != "")
            {
                GetHtml(url);
            }
        }
        /// <summary>
        /// Получаем Html-код страницы
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        private async void GetHtml(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            String source = Encoding.GetEncoding(GetEncoding(url)).GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var parseDoc = new HtmlDocument();
            parseDoc.LoadHtml(source);
            ParseBox.Text = GetHeadTags(parseDoc);
        }
        /// <summary>
        /// Получаем кодировку страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetEncoding(string url)
        {
            WebRequest req  = WebRequest.Create(url);
            String contentHeader = req.GetResponse().ContentType;
            var contentArr = contentHeader.Split(';');
            if (contentArr.Length > 1)
            {
                return contentArr[1].Replace("charset=", "").Trim();
            }
            return "utf-8";
        }
        /// <summary>
        /// Получаем верхние тэги страницы
        /// </summary>
        /// <param name="parseDoc">Html документ подверженный парсингу</param>
        /// <returns></returns>
        private string GetHeadTags(HtmlDocument parseDoc)
        {
            var nodes = parseDoc.DocumentNode.Descendants();
            _tags = SqlMethods.GetTags();
            foreach(var tag in _tags)
            {
                _listOfTags.Add(tag["name"]);
            }

            foreach (var tag in _tags)
            {
                var currentNodes = nodes.Where(t => t.Name == tag["name"]);

                foreach (var currentNode in currentNodes.Where(t =>t.ParentNode.Name=="body"))
                {
                    GetText(currentNode);
                }
            }

            return _textResult.ToString();
        }
        /// <summary>
        /// Достаём текст
        /// </summary>
        /// <param name="curNode">Текущая ветка</param>
        private void GetText(HtmlNode curNode)
        {
            foreach(var childNode in curNode.ChildNodes)
            {
                if (childNode.Name == "#text" && _listOfTags.Contains(childNode.ParentNode.Name))
                {
                    string text = childNode.InnerHtml;
                    if (!(text.Contains("<!--") || text.Contains("</")))
                    {
                        text = text.Replace("\n", String.Empty).Trim();
                        text = ClearSpecialChars(text);
                        if (text != String.Empty)
                        {
                            _textResult.AppendLine(text);
                        }
                    }
                }
                else
                {
                    GetText(childNode);
                }
            }
        }

        public static string ClearSpecialChars(string text)
        {
            var specialChars = text.Where(ch => !Char.IsLetterOrDigit(ch)).ToList();

            foreach (var specialChar in specialChars)
            {
                text.Replace(specialChar.ToString(), String.Empty);
            }

            return text;
        }

        /// <summary>
        /// По нажатию Ctrl+A выделяем  текст и копируем в буфер обмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseBox_KeyDown(object sender, KeyEventArgs e)
        {
           if ( Control.ModifierKeys==Keys.Control && e.KeyCode==Keys.A)
           {
               ParseBox.SelectAll();
               Clipboard.SetText(ParseBox.Text);
           }
        }



        
    }
}
