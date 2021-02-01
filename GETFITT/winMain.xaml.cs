using System;
using System.Windows;

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
            lblToday.Content = "Today is: " + DateTime.Now.ToString("dd/MM/yy");

            //lblName.Content = "Welcome back, " + user.strUsername + " !";
            lblName.Content = "Welcome back, " + App.Current.Properties["username"] + " !";

            WelcomeMessage();
        }

        private void WelcomeMessage()
        {
            int hourOfDay = Convert.ToInt32(DateTime.Now.ToString("HH"));
            if (hourOfDay >= 0 && hourOfDay < 12)
                lblWelcome.Content = "Good Morning, are you feeling good today :)";
            else if (hourOfDay >= 12 && hourOfDay < 18)
                lblWelcome.Content = "Good Afternoon";
            else if (hourOfDay >= 18 && hourOfDay < 21)
                lblWelcome.Content = "Good Evening,";
            else if (hourOfDay >= 21 && hourOfDay < 24)
                lblWelcome.Content = "Good Night";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnHomeWorkout_Click(object sender, RoutedEventArgs e)
        {
            //open winHomeWorkout window
            winHomeWorkout homeworkout = new winHomeWorkout();
            homeworkout.Top = 0;
            homeworkout.Left = 0;
            homeworkout.Show();
        }

        private void btnTracker_Click(object sender, RoutedEventArgs e)
        {
            //open winTracker window
            winTrackerGraph wintracker = new winTrackerGraph();
            wintracker.Top = 30;
            wintracker.Left = 30;
            wintracker.Show();
        }

        private void btnHealthMonitor_Click(object sender, RoutedEventArgs e)
        {
            winHealthMonitor winhealthmonitor = new winHealthMonitor();
            winhealthmonitor.Top = 30;
            winhealthmonitor.Left = 30;
            winhealthmonitor.Show();
        }

        private void btnSwitchAcc_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            winLogin winlogin = new winLogin();
            winlogin.Top = 30;
            winlogin.Left = 30;
            winlogin.Show();
        }

        private void btnWorkoutTracker_Click(object sender, RoutedEventArgs e)
        {
            //open winWorkoutPlanner window
            winWorkoutPlanner winworkoutplanner = new winWorkoutPlanner();
            winworkoutplanner.Top = 30;
            winworkoutplanner.Left = 30;
            winworkoutplanner.Show();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //open winupdate window
            winUpdate winupdate = new winUpdate();
            winupdate.Top = 30;
            winupdate.Left = 30;
            winupdate.Show();
        }
    }
}
