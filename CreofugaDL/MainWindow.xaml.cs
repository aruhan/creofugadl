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
using System.Net.Http;
using Microsoft.Win32;

namespace CreofugaDL
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Uri GetUrl(string url)
        {
            var baseUrl = new Uri(url);
            var id = baseUrl.Segments.Last();
            //return new Uri(String.Format(String.Format baseUrl.Scheme, baseUrl.Host, String.Format("audios/play?id={0}", id));
            return new UriBuilder(baseUrl) {
                Path = "audios/play",
                Query = String.Format("id={0}", id)
            }.Uri;
        }

        public void Download(Uri url, string filepath)
        {
            using (var client = new HttpClient())
            using (var file = System.IO.File.OpenWrite(filepath))
            using (var stream = client.GetStreamAsync(url).Result)
            {
                stream.CopyTo(file);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = GetUrl(this.BaseUrl.Text);

            var dlg = new SaveFileDialog() {
                FileName = this.BaseUrl.Text.Substring(this.BaseUrl.Text.LastIndexOf('/') + 1),
                DefaultExt = ".mp3",
                Filter = "MP3 file (*.mp3)|*.mp3"
            };
            if (dlg.ShowDialog() == true) {
                Download(url, dlg.FileName);
                MessageBox.Show("Download が完了しました");
            }
        }
    }

}
