using System.Net;
using System.Windows;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winUpdate.xaml
    /// </summary>
    public partial class winUpdate : Window
    {
        string githubreadme = "https://raw.githubusercontent.com/Dronesat/GETFITT-public/main/README.md";
        string githubstring;

        public winUpdate()
        {
            InitializeComponent();
            githubstring = new WebClient().DownloadString(githubreadme);
            txtReadme.Text = githubstring;
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private void btnDownloadUpdate_Click(object sender, RoutedEventArgs e)
        {
            string downloadlink = getBetween(githubstring, "application:", "applink");
            string _downloadlink = downloadlink.Trim();
            System.Diagnostics.Process.Start(_downloadlink);
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
