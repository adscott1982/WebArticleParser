using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebArticleParser;

namespace WebParserUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(UrlTextBox.Text);
            if (uri.IsWellFormedOriginalString())
            {
                var webPageText = new WebPageText(uri.AbsoluteUri);
                OutputTextBlock.Text = webPageText.Text;
            }
            else
            {
                UrlTextBox.Text = "";
            }
            
        }
    }
}
