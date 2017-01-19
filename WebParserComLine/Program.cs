using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebArticleParser;

namespace WebParserComLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = @"https://medium.com/defiant/the-young-pope-makes-catholicism-great-again-5631f234604d#.7octnmz13";

            Console.WriteLine($"Loading {url}");
            Console.WriteLine("Text is:\n");

            var webPage = new WebPageText(url);
            Console.WriteLine(webPage.Text);

            Console.ReadKey();
        }
    }
}
