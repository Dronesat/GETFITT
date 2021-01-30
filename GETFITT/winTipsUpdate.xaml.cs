using System.Net;
using System.Windows;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winTipsUpdate.xaml
    /// </summary>
    public partial class winTipsUpdate : Window
    {
        //declare url and version number
        double currentversion = 0;
        string tipsurl = "https://pastebin.com/raw/AZqNeXVh";
        string changelogurl = "https://pastebin.com/raw/1iPDsmgz";
        string latestversionurl = "https://pastebin.com/raw/HA4WXQ5B";
        string latestversiondownloadurl = "https://pastebin.com/raw/HUMJ0c3m";

        public winTipsUpdate()
        {
            InitializeComponent();
            UpdateTips();
            btnDownload.Visibility = Visibility.Hidden;
        }
        private void UpdateTips()
        {
            //display txtTips with data from web
            string tips = new WebClient().DownloadString(tipsurl);
            txtTips.Text = tips;
        }
        private void UpdateChangeLog()
        {
            //display txtChangeLog with data from web
            string changelog = new WebClient().DownloadString(changelogurl);
            txtChangeLog.Text = changelog;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            UpdateChangeLog();
            string latestversion_string = new WebClient().DownloadString(latestversionurl);
            double.TryParse(latestversion_string, out double latestversion);
            //new version available
            if (latestversion > currentversion)
            {
                btnDownload.Visibility = Visibility.Visible;
                MessageBox.Show("New update available!" + " (Current: " + currentversion + " Latest: " + latestversion + ")");
            }
            //not available
            else
            {
                MessageBox.Show("No update found");
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string downloadlink = new WebClient().DownloadString(latestversiondownloadurl);
            System.Diagnostics.Process.Start(downloadlink);
        }
    }
}
