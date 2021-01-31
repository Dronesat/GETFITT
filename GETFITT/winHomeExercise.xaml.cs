using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Media;
using System.Threading.Tasks;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winHomeExercise.xaml
    /// </summary>
    public partial class winHomeExercise : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        claExercise currentExercise;

        public winHomeExercise(claExercise newExercise)
        {
            InitializeComponent();

            currentExercise = newExercise;

            //update visual display
            lblExercise.Content = currentExercise.strExercise;
            lblMovementPattern.Content = currentExercise.strMovementPattern;
            txtInstruction.Text = currentExercise.strInstruction;

            //update image
            string source = @"/Resources/HomeWorkout_Images/" + currentExercise.strMovementPattern + @"/" + currentExercise.strExercise + @".jfif";
            imgImage.Source = new BitmapImage(new Uri(source, UriKind.Relative));
        }

        private void btnCompleted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //initialise connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string user_id = App.Current.Properties["id"].ToString();
                    int exercise_id = currentExercise.strExercise_id;
                    int movementpattern_id = currentExercise.strMovementPattern_id;
                    string today_date = DateTime.Now.ToString("yyyy/MM/dd");

                    //sql command
                    SqlCommand cmd = new SqlCommand("INSERT INTO CompletedExercises VALUES ('" + user_id + "','" + exercise_id + "','" + movementpattern_id + "','" + today_date + "')", conn);

                    //open connection
                    conn.Open();

                    //excute cmd, no result return
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Saved completed exercise to Database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnToDo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //initialise connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string user_id = App.Current.Properties["id"].ToString();
                    int exercise_id = currentExercise.strExercise_id;
                    int movementpattern_id = currentExercise.strMovementPattern_id;
                    string tomorrow_date = DateTime.Today.AddDays(+1).ToString("yyyy/MM/dd");

                    //sql command
                    SqlCommand cmd = new SqlCommand("INSERT INTO TodoExercises VALUES ('" + user_id + "','" + exercise_id + "','" + movementpattern_id + "','" + tomorrow_date + "')", conn);

                    //open connection
                    conn.Open();

                    //excute cmd, no result return
                    cmd.ExecuteNonQuery();

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    MessageBox.Show("Saved todo exercise to Database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(1000);
        }

        private async void btnAddToStopwatch_Click(object sender, RoutedEventArgs e)
        {
            bool isTime = int.TryParse(txtExerciseTime.Text, out int time);
            if (isTime == true)
            {
                if (time > 0 && time < 100)
                {
                    try
                    {
                        //initialise connection
                        using (SqlConnection conn = new SqlConnection(strConn))
                        {
                            string user_id = App.Current.Properties["id"].ToString();
                            int exercise_id = currentExercise.strExercise_id;
                            int movementpattern_id = currentExercise.strMovementPattern_id;
                            
                            //open connection
                            conn.Open();

                            //sql command
                            SqlCommand cmd = new SqlCommand("INSERT INTO ExercisesStopwatch VALUES ('" + user_id + "' , '"+ exercise_id + "' , '" + movementpattern_id + "' , '" + time + "')", conn);

                            //excute cmd, no result return
                            cmd.ExecuteNonQuery();

                            //close and dispose connection
                            conn.Close();
                            conn.Dispose();

                            txtInstruction.FontSize = 24;
                            txtInstruction.Text = "Press Load to update exercises list";
                            txtExerciseTime.Text = "Saved";

                            await PutTaskDelay();

                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    //change textbox color to red
                    txtExerciseTime.Background = new SolidColorBrush(Colors.Red);

                    //error messagebox
                    MessageBox.Show("Check Time");

                    //put empty string in txtTime
                    txtExerciseTime.Text = "";

                    //change textbox color to white
                    txtExerciseTime.Background = new SolidColorBrush(Colors.White);
                    //focus on txtTime
                    txtExerciseTime.Focus();
                }
            }
            else
            {
                //change textbox color to red
                txtExerciseTime.Background = new SolidColorBrush(Colors.Red);

                //error messagebox
                MessageBox.Show("Check Time");

                //put empty string in txtTime
                txtExerciseTime.Text = "";

                //change textbox color to white
                txtExerciseTime.Background = new SolidColorBrush(Colors.White);
                //focus on txtTime
                txtExerciseTime.Focus();
            }
        }
    }
}