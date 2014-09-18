using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using SiteParse.Communication.SqlManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        public ParseForm()
        {
            InitializeComponent();
        }


        private string _url = "http://www.gazeta.ru/tech/news/2014/09/17/n_6487517.shtml";

        private void ParseForm_Load(object sender, EventArgs e)
        {
            ParseBox.ScrollBars = ScrollBars.Vertical;
        }

        private  void ParseBtn_Click(object sender, EventArgs e)
        {
           ParseHtml(_url);
        }

        private async void ParseHtml(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            String source = Encoding.GetEncoding("windows-1251").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var parseDoc = new HtmlDocument();
            parseDoc.LoadHtml(source);

            ParseBox.Text = GetTextFromPage(parseDoc);
        }

        private string GetTextFromPage(HtmlDocument parseDoc)
        {
            var textResult = new StringBuilder();
            var nodes = parseDoc.DocumentNode.Descendants();
            var tags = SqlMethods.GetTags();

            foreach (var tag in tags)
            {
                var currentNodes = nodes.Where(t => t.Name == tag["name"]);

                foreach (var currentNode in currentNodes)
                {
                    textResult.AppendLine(currentNode.InnerText);
                }
            }

            return textResult.ToString();
        }



        
    }
}
