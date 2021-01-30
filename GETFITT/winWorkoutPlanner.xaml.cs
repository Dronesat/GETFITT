using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winWorkoutPlanner.xaml
    /// </summary>
    public partial class winWorkoutPlanner : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        public class TodoListviewItem
        {
            public int Id { get; set; }
            public string MovementPattern { get; set; }
            public string Exercise { get; set; }
        }

        public class CompletedListviewItem
        {
            public int Id { get; set; }
            public string MovementPattern { get; set; }
            public string Exercise { get; set; }
        }

        //generate date
        string tomorrow_date = DateTime.Today.AddDays(+1).ToString("yyyy/MM/dd");
        string today_date = DateTime.Today.ToString("yyyy/MM/dd");

        bool isToday = true;

        string datePicker_date;

        public winWorkoutPlanner()
        {
            InitializeComponent();

            //load To do exercises for today
            LoadToDoExercises(today_date);

            //Load completed exercises for today
            LoadCompletedExercises(today_date);

            //add columns to listview
            lsvTodoAddColumns();
            lsvCompletedAddColumns();

            datePicker.DisplayDateEnd = DateTime.Today;
        }

        private void lsvCompletedAddColumns()
        {
            //add columns to listview todo exercises
            var gridView = new GridView();
            lsvCompletedExercises.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new Binding("Id")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Movement Pattern",
                DisplayMemberBinding = new Binding("MovementPattern")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Exercise Name",
                DisplayMemberBinding = new Binding("Exercise")
            });
        }
        private void lsvTodoAddColumns()
        {
            //add columns to listview completed exercises
            var gridView = new GridView();
            lsvTodoExercises.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new Binding("Id")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Movement Pattern",
                DisplayMemberBinding = new Binding("MovementPattern")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Exercise Name",
                DisplayMemberBinding = new Binding("Exercise")
            });
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

        //input movement pattern id -> output movement pattern name
        private string GetMovementPatternName(int movementpattern_id)
        {     
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                //sql command
                SqlCommand cmd = new SqlCommand("SELECT movementpattern FROM MovementPatterns WHERE id = '" + movementpattern_id + "'", conn);

                //open connection
                conn.Open();

                //execute command
                string movementpattern_name = (string)cmd.ExecuteScalar();

                //close connection
                conn.Close();
                conn.Dispose();

                return movementpattern_name;
            }
        }

        private void LoadToDoExercises(string date)
        { 
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());

                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT id, exercise_id, movementpattern_id FROM TodoExercises WHERE user_id = '"+user_id+"' AND date = '"+ date + "' ", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //get exercise id and movement pattern id
                        int id = Convert.ToInt32(dr["id"].ToString());
                        int exercise_id = Convert.ToInt32(dr["exercise_id"].ToString());
                        int movementpattern_id = Convert.ToInt32(dr["movementpattern_id"].ToString());

                        //get movement pattern and exercise foreach movement pattern id nad exercise id
                        string movementpattern = GetMovementPatternName(movementpattern_id);
                        string exercise = GetExerciseName(exercise_id);

                        //add item to TodoExercise listview
                        lsvTodoExercises.Items.Add(new TodoListviewItem { Id = id, MovementPattern = movementpattern, Exercise = exercise });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTodo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedindex = Convert.ToInt32(cmbTodo.SelectedIndex.ToString());

            //today
            if (selectedindex == 0)
            {
                //clear listview
                lsvTodoExercises.Items.Clear();

                //load to do exercises to listview
                LoadToDoExercises(today_date);

                if (lsvTodoExercises.Items.Count == 0)
                {
                    MessageBox.Show("No result found for today", "Message");
                }

                isToday = true;
            }

            //tomorrow
            if (selectedindex == 1)
            {
                //clear listview
                lsvTodoExercises.Items.Clear();

                //load to do exercises to listview
                LoadToDoExercises(tomorrow_date);

                if(lsvTodoExercises.Items.Count == 0)
                {
                    MessageBox.Show("No result found for tomorrow", "Message");
                }

                isToday = false;
            }
        }

        private void DeleteTodo(string date)
        {
            if (int.TryParse(txtTodoID.Text, out int delete_id))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(strConn);

                    //open connection
                    conn.Open();

                    //execute sql cmd
                    string strcmd = "DELETE FROM TodoExercises WHERE id = '" + delete_id + "' AND date = '" + date + "'";

                    //execute cmd
                    SqlCommand cmd = new SqlCommand(strcmd, conn);
                    cmd.ExecuteNonQuery();

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    //clear textbox
                    txtTodoID.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Check entered ID", "Error");
            }
        }
        private void btnDeleteTodo_Click(object sender, RoutedEventArgs e)
        {
            //today
            if (isToday == true)
            {
                //delete today exericse
                DeleteTodo(today_date);

                //clear listview
                lsvTodoExercises.Items.Clear();

                //load listview
                LoadToDoExercises(today_date);
            }

            //tomorrow
            if(isToday == false)
            {
                //delete tomorrow exercise
                DeleteTodo(tomorrow_date);
                
                //clear listview
                lsvTodoExercises.Items.Clear();

                //load tomorrow exercise
                LoadToDoExercises(tomorrow_date);
            }
        }

        private void LoadCompletedExercises(string date)
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());

                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT id, exercise_id, movementpattern_id FROM CompletedExercises WHERE user_id = '" + user_id + "' AND date = '" + date + "' ", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //get exercise id and movement pattern id
                        int id = Convert.ToInt32(dr["id"].ToString());
                        int exercise_id = Convert.ToInt32(dr["exercise_id"].ToString());
                        int movementpattern_id = Convert.ToInt32(dr["movementpattern_id"].ToString());

                        //get movement pattern and exercise for each movement pattern id nad exercise id
                        string movementpattern = GetMovementPatternName(movementpattern_id);
                        string exercise = GetExerciseName(exercise_id);

                        //add item to TodoExercise listview
                        lsvCompletedExercises.Items.Add(new TodoListviewItem { Id = id, MovementPattern = movementpattern, Exercise = exercise });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //get user selected date
            DateTime selecteditem = Convert.ToDateTime(datePicker.SelectedDate.ToString());
            datePicker_date = Convert.ToString(selecteditem.ToString("yyyy/MM/dd"));

            //clear completed exercises listview
            lsvCompletedExercises.Items.Clear();

            //load compleeted exercises with current input date
            LoadCompletedExercises(datePicker_date);

            if (lsvCompletedExercises.Items.Count == 0)
            {
                MessageBox.Show("No result found","Message");
            }
        }

        private void DeleteCompleted(string date)
        {
            if (int.TryParse(txtCompletedID.Text, out int delete_id))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(strConn);

                    //open connection
                    conn.Open();

                    //execute sql cmd
                    string strcmd = "DELETE FROM CompletedExercises WHERE id = '" + delete_id + "' AND date = '" + date + "'";

                    //execute cmd
                    SqlCommand cmd = new SqlCommand(strcmd, conn);
                    cmd.ExecuteNonQuery();

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    //clear textbox
                    txtCompletedID.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Check entered ID", "Error");
            }
        }

        private void btnDeleteCompleted_Click(object sender, RoutedEventArgs e)
        {
            //delete completed exercises with input date
            DeleteCompleted(datePicker_date);

            //clear listview
            lsvCompletedExercises.Items.Clear();

            //load completed exercises with input date
            LoadCompletedExercises(datePicker_date);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
