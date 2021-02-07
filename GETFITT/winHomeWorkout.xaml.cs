using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winHomeWorkout.xaml
    /// </summary>
    public partial class winHomeWorkout : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        //declare array
        string[] arrDay;
        int[] arrNoExercisesPerDay;

        //Graph
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public winHomeWorkout()
        {
            InitializeComponent();

            LoadAllExercises();

            //Load exercise tracking
            WorkoutTracker();

            //Load graph
            ExercisesGraph();

            //load exercises into stopwatch list
            LoadExercisesStopwatch();
        }

        private void WorkoutTracker()
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

        private int NumofExercisesperDay(string date)
        {
            //initialize connection
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                string user_id = App.Current.Properties["id"].ToString();

                //sql command 
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CompletedExercises WHERE user_id = '" + user_id + "' AND date = '" + date + "'", conn);

                //open connection
                conn.Open();

                //execute cmd
                int count = (int)cmd.ExecuteScalar();

                //close connection
                conn.Close();
                conn.Dispose();

                return count;
            }
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

        

        private string LoadMovementPattern(int movementpattern_id)
        {
            //initialize connection
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                //sql command
                SqlCommand cmd = new SqlCommand("SELECT movementpattern FROM MovementPatterns WHERE id = '" + movementpattern_id + "'", conn);

                //open connection
                conn.Open();

                //execute cmd
                string movementpattern = (string)cmd.ExecuteScalar();

                //close connection
                conn.Close();
                conn.Dispose();

                return movementpattern;
            }
        }

        private void LoadAllExercises()
        {
            //create a hashset for movement patterns
            HashSet<string> hshMovementPatterns = new HashSet<string>();

            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Exercises", conn);

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

                        //get items
                        int exercise_id = Convert.ToInt32(dr["id"].ToString());
                        int movementpattern_id = Convert.ToInt32(dr["movementpattern_id"].ToString());
                        string movementpattern = LoadMovementPattern(movementpattern_id);
                        string exercise = dr["exercise"].ToString();
                        string instruction = dr["instruction"].ToString();

                        //add item to datastore
                        newExercise.strExercise_id = exercise_id;
                        newExercise.strMovementPattern_id = movementpattern_id;
                        newExercise.strMovementPattern = movementpattern;
                        newExercise.strExercise = exercise;
                        newExercise.strInstruction = instruction;

                        //add movementpattern to hashset
                        hshMovementPatterns.Add(newExercise.strMovementPattern);

                        //create usercontrol
                        ucExerciseCard newCard = new ucExerciseCard(newExercise);

                        //add to list of Exercises
                        claDataStore.lstExercises.Add(newCard);
                    }
                    //add item in hashset to listbox
                    foreach (string MovementPattern in hshMovementPatterns)
                        lsbMovementPattern.Items.Add(MovementPattern);
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

            //clear wrap panel 
            wraExercise.Children.Clear();

            foreach (ucExerciseCard currentCard in claDataStore.lstExercises)
            {
                if (currentCard.currentExercise.strMovementPattern == lsbItem)
                {
                    //add new card to wrap panel
                    wraExercise.Children.Add(currentCard);
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //// Delete Exercises Stopwatch from database
                int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());

                SqlConnection conn = new SqlConnection(strConn);
                SqlCommand cmd = new SqlCommand("DELETE FROM ExercisesStopwatch WHERE user_id = '" + user_id + "'", conn);

                //open connection
                conn.Open();

                //execute command
                cmd.ExecuteNonQuery();

                //close connection
                conn.Close();
                conn.Dispose();

                //clear listbox and textbox
                lstExercise.Items.Clear();
                lstTime.Items.Clear();
                txtRestTime.Text = "";

                LoadExercisesStopwatch();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(1000);
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Initialize a new instance of the SpeechSynthesizer.  
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            int lstExercisecount = Convert.ToInt32(lstExercise.Items.Count.ToString());
            bool isRestTime = int.TryParse(txtRestTime.Text, out int resttime);

            //check item in listbox
            if (lstExercisecount != 0)
            {
                //check rest time
                if (isRestTime == true && resttime >= 0)
                {
                    //loop through each exercises
                    for (int i = 0; i < lstExercise.Items.Count; i++)
                    {
                        //create a promt for speak string
                        Prompt speak_string = new Prompt(lstExercise.Items[i].ToString());

                        //speak asynchronously
                        synth.SpeakAsync(speak_string);

                        //clear wrap panel 
                        wraExercise.Children.Clear();

                        //display current exercise to wrap panel
                        foreach (ucExerciseCard currentCard in claDataStore.lstExercises)
                        {
                            if (currentCard.currentExercise.strExercise == lstExercise.Items[i].ToString())
                            {
                                //add new card to wrap panel
                                wraExercise.Children.Add(currentCard);
                            }
                        }

                        //display exercise
                        lblExercise.Content = lstExercise.Items[i].ToString();

                        int time = int.Parse(lstTime.Items[i].ToString());

                        //loop time
                        for (int j = time; j >= 0; j--)
                        {
                            //Declare TimeSpan
                            TimeSpan timespan = new TimeSpan(0, 0, j);

                            if (j <= 3 && j > 0)
                            {
                                //create a promt for speak string
                                Prompt countdown_speak = new Prompt(j.ToString());

                                //speak asynchronously
                                synth.SpeakAsync(countdown_speak);
                            }

                            //update label stopwatch
                            lblStopwatch.Content = timespan.ToString(@"mm\:ss");

                            await PutTaskDelay();
                        }

                        //create a promt for speak string
                        Prompt rest_speak = new Prompt("Rest");

                        //speak asynchronously
                        synth.SpeakAsync(rest_speak);

                        //display Rest 
                        lblExercise.Content = "Rest";

                        //loop rest time
                        for (int k = resttime; k >= 0; k--)
                        {
                            //declare TimeSpan
                            TimeSpan timespan1 = new TimeSpan(0, 0, k);

                            if (k <= 3 && k > 0)
                            {
                                //create a promt for speak string
                                Prompt countdown_speak = new Prompt(k.ToString());

                                //speak asynchronously
                                synth.SpeakAsync(countdown_speak);
                            }

                            //update label stopwatch
                            lblStopwatch.Content = timespan1.ToString(@"mm\:ss");

                            await PutTaskDelay();
                        }
                    }
                    //update label Exercise to Finished
                    lblExercise.Content = "Finished";

                    //speak Finished
                    synth.Speak("Workout Finished");

                    synth.Dispose();
                }
                //rest time invalid
                else
                {
                    //empty txtTime 
                    txtRestTime.Text = "";

                    //change textbox color to red
                    txtRestTime.Background = new SolidColorBrush(Colors.Red);

                    //show messagebox
                    MessageBox.Show("Please submit rest time", "Error");

                    //change textbox color to white
                    txtRestTime.Background = new SolidColorBrush(Colors.White);
                }
                
            }
            //no item in listbox
            else
            {
                //change textbox color to red
                lstExercise.Background = new SolidColorBrush(Colors.Red);
                lstTime.Background = new SolidColorBrush(Colors.Red);

                //error messagebox
                MessageBox.Show("Please add exercise and time","Error");

                //change textbox color to white
                lstExercise.Background = new SolidColorBrush(Colors.White);
                lstTime.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnWorkoutPlanner_Click(object sender, RoutedEventArgs e)
        {
            //show winWorkoutPlanner window
            winWorkoutPlanner winworkoutplanner = new winWorkoutPlanner();
            winworkoutplanner.Top = 10;
            winworkoutplanner.Left = 30;
            winworkoutplanner.Show();
        }

        //input exercise id -> output exercise name
        private string GetExerciseName(int exercise_id)
        {
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                //sql command
                SqlCommand cmd = new SqlCommand("SELECT exercise FROM Exercises WHERE id = '" + exercise_id + "'", conn);

                //open connection
                conn.Open();

                //execute command
                string exercise_name = (string)cmd.ExecuteScalar();

                //close connection
                conn.Close();
                conn.Dispose();

                return exercise_name;
            }
        }

        private void LoadExercisesStopwatch()
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());

                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT exercise_id, time FROM ExercisesStopwatch WHERE user_id = '" + user_id + "'", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        int exercise_id = Convert.ToInt32(dr["exercise_id"].ToString());
                        int time = Convert.ToInt32(dr["time"].ToString());

                        //get exercise name
                        string exercise = GetExerciseName(exercise_id);

                        //add item to listbox
                        lstExercise.Items.Add(exercise);
                        lstTime.Items.Add(time);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //clear exercises and time list
            lstExercise.Items.Clear();
            lstTime.Items.Clear();

            //load exercise to list
            LoadExercisesStopwatch();
        }

        private void InsertIntoCompletedExercises(int user_id, int exercise_id, int movementpattern_id, string date)
        {
            try
            {
                //initialise connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("INSERT INTO CompletedExercises VALUES ('" + user_id + "','" + exercise_id + "','" + movementpattern_id + "','" + date + "')", conn);

                    //open connection
                    conn.Open();

                    //excute cmd, no result return
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveCompletedExercises_Click(object sender, RoutedEventArgs e)
        {
            //count item in listbox
            int lstExercisecount = Convert.ToInt32(lstExercise.Items.Count.ToString());

            if (lstExercisecount == 0)
            {
                MessageBox.Show("No exercise completed yet!","Error");
            }
            else if (lstExercisecount > 0)
            {
                try
                {
                    //initialize connection
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());
                        string today_date = DateTime.Today.ToString("yyyy/MM/dd");

                        //sql command
                        SqlCommand cmd = new SqlCommand("SELECT exercise_id, movementpattern_id FROM ExercisesStopwatch WHERE user_id = '" + user_id + "'", conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        //fill datatable
                        da.Fill(dt);

                        //close connection
                        conn.Close();
                        conn.Dispose();

                        foreach (DataRow dr in dt.Rows)
                        {
                            int exercise_id = Convert.ToInt32(dr["exercise_id"].ToString());
                            int movementpattern_id = Convert.ToInt32(dr["movementpattern_id"].ToString());

                            InsertIntoCompletedExercises(user_id, exercise_id, movementpattern_id, today_date);
                        }
                        MessageBox.Show("Saved to CompletedExercises Database");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void lstExercise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get current selected item in the listbox
            string lsbItem = lstExercise.SelectedItem.ToString();

            //clear wrap panel 
            wraExercise.Children.Clear();

            foreach (ucExerciseCard currentCard in claDataStore.lstExercises)
            {
                if (currentCard.currentExercise.strExercise == lsbItem)
                {
                    //add new card to wrap panel
                    wraExercise.Children.Add(currentCard);
                }
            }
        }
    }
}
