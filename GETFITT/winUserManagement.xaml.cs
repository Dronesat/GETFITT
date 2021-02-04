using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winUserManagement.xaml
    /// </summary>
    public partial class winUserManagement : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        public class UserListviewItem
        {
            public int Id { get; set; }
            public string Username { get; set; }
        }

        public winUserManagement()
        {
            InitializeComponent();

            //add columns to list view
            lsvUsersAddColumns();

            //Load Users from database
            LoadUsers();
        }

        private void lsvUsersAddColumns()
        {
            //add columns to listview todo exercises
            var gridView = new GridView();
            lsvUsers.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new Binding("Id")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Username",
                DisplayMemberBinding = new Binding("Username")
            });
        }

        private void LoadUsers()
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT id, username FROM Users", conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    foreach (DataRow dr in dt.Rows)
                    {
                        int user_id = Convert.ToInt32(dr["id"].ToString());
                        string username = dr["username"].ToString();

                        lsvUsers.Items.Add(new UserListviewItem { Id = user_id, Username = username });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtUserID.Text, out int delete_id))
            {
                int user_id = Convert.ToInt32(App.Current.Properties["id"].ToString());
                if (delete_id == user_id)
                {
                    MessageBox.Show("You cannot delete your account while logged in", "Error");
                }
                else
                {
                    //confirmation messagebox
                    MessageBoxResult messageBoxResult = MessageBox.Show("Once deleted, data cannot be recovered!", "Confirmation", MessageBoxButton.YesNo);
                    //if yes then execusted
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SqlConnection conn = new SqlConnection(strConn);

                            //open connection
                            conn.Open();

                            //execute sql cmd
                            string strcmd = "DELETE FROM Users WHERE id = '" + delete_id + "'";

                            //execute cmd
                            SqlCommand cmd = new SqlCommand(strcmd, conn);
                            cmd.ExecuteNonQuery();

                            //close connection
                            conn.Close();
                            conn.Dispose();

                            //clear textbox
                            txtUserID.Clear();

                            lsvUsers.Items.Clear();

                            LoadUsers();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Check entered ID", "Error");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
