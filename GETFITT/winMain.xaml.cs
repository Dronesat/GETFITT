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
using System.Windows.Shapes;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winMain.xaml
    /// </summary>
    public partial class winMain : Window
    {
        public winMain()
        {
            InitializeComponent();
            //display today date
            lblToday.Content = DateTime.Now.ToString("d");
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //show messagebox, confirm exit
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Bye see y again", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
                //close the application
                Application.Current.Shutdown();
        }

        private void btnBMI_Click(object sender, RoutedEventArgs e)
        {
            //open winBMI window         
            winBMI winbmi = new winBMI();
            winbmi.Top = 10;
            winbmi.Left = 30;
            winbmi.Show();
        }

        private void btnBMR_Click(object sender, RoutedEventArgs e)
        {
            //open winBMR window
            winBMR winbmr = new winBMR();
            winbmr.Top = 10;
            winbmr.Left = 30;
            winbmr.Show();
        }

        private void btnWater_Click(object sender, RoutedEventArgs e)
        {
            //open winWater window
            winWater winwater = new winWater();
            winwater.Top = 10;
            winwater.Left = 30;
            winwater.Show();
        }

        private void btnMyFood_Click(object sender, RoutedEventArgs e)
        {
            //open wincreatefood window
            //winCreateFood wincreatefood = new winCreateFood();
            //wincreatefood.Top = 10;
            //wincreatefood.Left = 30;
            //.Show();
            
        }

        private void btnHomeWorkout_Click(object sender, RoutedEventArgs e)
        {
            //open winHomeWorkout window
            winHomeWorkout homeworkout = new winHomeWorkout();
            homeworkout.Top = 10;
            homeworkout.Left = 30;
            homeworkout.Show();
        }

        private void btnTipsUpdate_Click(object sender, RoutedEventArgs e)
        {
            //open winTipsUpdate window
            winTipsUpdate tipsupdate = new winTipsUpdate();
            tipsupdate.Top = 10;
            tipsupdate.Left = 30;
            tipsupdate.Show();
        }

        private void btnTracker_Click(object sender, RoutedEventArgs e)
        {
            //open winTracker window
            winTracker wintracker = new winTracker();
            wintracker.Top = 10;
            wintracker.Left = 30;
            wintracker.Show();
        }
    }
}
