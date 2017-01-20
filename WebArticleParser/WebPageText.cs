using System;
using HtmlAgilityPack;
using System.Linq;

namespace WebArticleParser
{
    public class WebPageText
    {
        private HtmlDocument htmlDoc;
        public string Text { get; private set; }

        public WebPageText(string url)
        {
            LoadWebPage(url);
            LoadText();
            Text = HtmlEntity.DeEntitize(Text);
        }

        private void LoadText()
        {
            var title = this.htmlDoc.DocumentNode.Descendants("title");
            if (title.Count() > 0) this.Text += title.First().InnerText + Environment.NewLine + Environment.NewLine;
            
            var paragraphs = this.htmlDoc.DocumentNode.Descendants("p")
                                                      .Select(n => n.InnerText)
                                                      .Where(t => !string.IsNullOrWhiteSpace(t))
                                                      .Select(s => this.RemoveEscapeCharacters(s));

            foreach (var paragraph in paragraphs)
            {
                var isValid = paragraph.Contains(" ");
                var text = isValid ? paragraph + Environment.NewLine + Environment.NewLine : "";
                this.Text += text;
            }
        }

        private string RemoveEscapeCharacters(string s)
        {
            s = s.Replace("\n", "");
            s = s.Replace("\t", "");

            return s;
        }

        private void LoadWebPage(string url)
        {
            var web = new HtmlWeb();
            this.htmlDoc = web.Load(url);
        }
    }
}
