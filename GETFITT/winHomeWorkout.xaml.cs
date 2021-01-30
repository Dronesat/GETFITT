using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winHomeWorkout.xaml
    /// </summary>
    public partial class winHomeWorkout : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();
        
        //declare
        int movementpattern_id;
        string movementpattern;

        //declare array
        string[] arrDay;
        int[] arrNoExercisesPerDay;

        //Graph
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter{ get; set; }

        public winHomeWorkout()
        {
            InitializeComponent();

            //populate listbox with all movement patterns
            LoadMovementPattern();

            //Load exercise tracking
            ExercisesTracking();

            //Load graph
            ExercisesGraph();
        }

        private void ExercisesTracking()
        {
            //create list
            List<string> lstDay = new List<string>();
            List<int> lstNoExercisesPerDay = new List<int>();

            int NumberOfDays = 10;

            //update label according to number of days
            lblExerciseTracking.Content = "Completed exercises during the last " + NumberOfDays + " days";

            //day decrement (today--)
            for (DateTime day = DateTime.Now; day > DateTime.Now.AddDays(-NumberOfDays); day = day.AddDays(-1))
            {
                string current_date = day.ToString("d/M");
                string current_date_sql = day.ToString("yyyy/MM/dd");
                int no_of_exercises = NumofExercisesperDay(current_date_sql);

                //add to list
                lstDay.Add(current_date);
                lstNoExercisesPerDay.Add(no_of_exercises);
            }
            //reverse list
            lstDay.Reverse();
            lstNoExercisesPerDay.Reverse();

            //convert list to array
            arrDay = lstDay.ToArray();
            arrNoExercisesPerDay = lstNoExercisesPerDay.ToArray();
        }

        private void ExercisesGraph()
        {
            //draw graph using livechart
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Number of exercises",
                    Values = new ChartValues<int>(arrNoExercisesPerDay),
                    PointGeometrySize = 10
                },
            };
            Labels = arrDay;
            YFormatter = value => value + "";
            DataContext = this;
        }

        private int NumofExercisesperDay(string date)
        {
            //initialize connection
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string user_id = App.Current.Properties["id"].ToString();

                //sql command 
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CompletedExercises WHERE user_id = '"+ user_id + "' AND date = '" + date + "'", conn);

                //open connection
                conn.Open();

                int count = (int)cmd.ExecuteScalar();

                //close connection
                conn.Close();
                conn.Dispose();

                return count;
            }
        }

        //populate listbox with all movement patterns
        private void LoadMovementPattern()
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT * FROM MovementPatterns", conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //populate listbox with all movement patterns
                        lsbMovementPattern.Items.Add(dr["movementpattern"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadMovementPatternID(string mp)
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT id FROM MovementPatterns WHERE movementpattern = '" + mp + "'", conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //add id to movementpattern_id
                        movementpattern_id = Convert.ToInt32(dr["id"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadExercise(int movementpattern_id)
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT id, exercise, instruction FROM Exercises WHERE movementpattern_id = '" + movementpattern_id + "'", conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //create object for class
                        claExercise newExercise = new claExercise();

                        int exercise_id = Convert.ToInt32(dr["id"].ToString());
                        string exercise = dr["exercise"].ToString();
                        string instruction = dr["instruction"].ToString();

                        newExercise.strMovementPattern_id = movementpattern_id;
                        newExercise.strExercise_id = exercise_id;
                        newExercise.strMovementPattern = movementpattern;
                        newExercise.strExercise = exercise;
                        newExercise.strInstruction = instruction;

                        //create usercontrol
                        ucExerciseCard newCard = new ucExerciseCard(newExercise);

                        //add to list of Exercises
                        claDataStore.lstExercises.Add(newCard);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lsbMovementPattern_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get current selected item in the listbox
            string lsbItem = lsbMovementPattern.SelectedItem.ToString();

            movementpattern = lsbItem;

            //get id for selected movement pattern
            LoadMovementPatternID(lsbItem);

            //Load Exercises
            LoadExercise(movementpattern_id);

            //clear the wrap panel
            wraExercise.Children.Clear();

            //for each exercise create and display a user control in the wrap panel
            foreach (ucExerciseCard currentExercise in claDataStore.lstExercises)
            {
                //add new card to wrap panel
                wraExercise.Children.Add(currentExercise);
            }

            //clear lstExercise 
            claDataStore.lstExercises.Clear();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //check if exercise exist or not
            if (txtExercise.Text != "")
            {
                //check if time is exist or not
                if (IntegerExtensions.ParseNullableInt(txtTime.Text) >= 0)
                {
                    //add input from textbox to listbox
                    lstExercise.Items.Add(txtExercise.Text);
                    lstTime.Items.Add(txtTime.Text);

                    //empty string and reset focus
                    txtExercise.Text = "";
                    txtTime.Text = "";
                    txtExercise.Focus();
                }
                //no time
                else
                {
                    //change textbox color to red
                    txtTime.Background = new SolidColorBrush(Colors.Red);

                    //error messagebox
                    MessageBox.Show("Check Time");

                    //put empty string in txtTime
                    txtTime.Text = "";

                    //change textbox color to white
                    txtTime.Background = new SolidColorBrush(Colors.White);
                    //focus on txtTime
                    txtTime.Focus();
                }
            }
            //no exercise
            else
            {
                //change textbox color to red
                txtExercise.Background = new SolidColorBrush(Colors.Red);
                //error messagebox
                MessageBox.Show("Check Exercise");

                //put empty string in txtTime
                txtExercise.Text = "";

                //focus on txtTime
                txtExercise.Focus();

                //change textbox color to white
                txtExercise.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //check if resttime exist or not 
            if (IntegerExtensions.ParseNullableInt(txtRestTime.Text) > 0)
            {
                //check item in listbox
                if (lstTime.Items.Count > 0)
                {
                    MessageBox.Show("Stopwatch starts Now");
                }
                //no item
                else
                {
                    //empty txtTime 
                    txtTime.Text = "";

                    //change textbox color to red
                    txtTime.Background = new SolidColorBrush(Colors.Red);

                    //show messagebox
                    MessageBox.Show("Add time");

                    //change textbox color to white
                    txtTime.Background = new SolidColorBrush(Colors.White);
                }
            }
            //no rest time
            else
            {
                //empty txtTime 
                txtRestTime.Text = "";

                //change textbox color to red
                txtRestTime.Background = new SolidColorBrush(Colors.Red);

                //show messagebox
                MessageBox.Show("Add rest time");

                //change textbox color to white
                txtRestTime.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear listbox and textbox
            lstExercise.Items.Clear();
            lstTime.Items.Clear();
            txtExercise.Text = "";
            txtRestTime.Text = "";
            txtTime.Text = "";
        }
        async Task PutTaskDelay()
        {
            await Task.Delay(1000);
        }
        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //loop through each exercises
            for (int i = 0; i < lstExercise.Items.Count; i++)
            {
                lblExercise.Content = lstExercise.Items[i].ToString();
                int time = int.Parse(lstTime.Items[i].ToString());

                //loop time
                for (int j = time; j >= 0; j--)
                {
                    //Declare TimeSpan
                    TimeSpan timespan = new TimeSpan(0, 0, j);

                    //update label stopwatch
                    lblStopwatch.Content = timespan.ToString(@"mm\:ss");
                    await PutTaskDelay();
                    System.Windows.Forms.Application.DoEvents();
                    if (j < 3)
                    {
                        SystemSounds.Beep.Play();
                    }
                }

                int resttime = IntegerExtensions.ParseInt(txtRestTime.Text);
                for (int k = resttime; k >= 0; k--)
                {
                    //declare TimeSpan
                    TimeSpan timespan1 = new TimeSpan(0, 0, k);

                    //display Rest 
                    lblExercise.Content = "Rest";

                    //update label stopwatch
                    lblStopwatch.Content = timespan1.ToString(@"mm\:ss");
                    await PutTaskDelay();
                    System.Windows.Forms.Application.DoEvents();
                    if (k < 3)
                    {
                        SystemSounds.Beep.Play();
                    }
                }
            }
            //update label Exercise to Finished
            lblExercise.Content = "Finished";
        }

        private void btnWorkoutPlanner_Click(object sender, RoutedEventArgs e)
        {
            //show winWorkoutPlanner window
            winWorkoutPlanner winworkoutplanner = new winWorkoutPlanner();
            winworkoutplanner.Top = 10;
            winworkoutplanner.Left = 30;
            winworkoutplanner.Show();
        }
    }
}
