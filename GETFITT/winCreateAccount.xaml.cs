using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winCreateAccount.xaml
    /// </summary>
    public partial class winCreateAccount : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        public winCreateAccount()
        {
            InitializeComponent();
        }

        //Convert string to sha256
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            //check if txtusername is not empty
            if (txtUsername.Text.ToString() != "")
            {
                //check if txtpassword is not empty
                if (txtPassword.Password.ToString() != "")
                {
                    //trim left and right
                    string _txtUsername = txtUsername.Text.Trim();
                    string _txtPassword = txtPassword.Password.Trim();

                    //encrypt password using sha256
                    string sha256_txtPassword = ComputeSha256Hash(_txtPassword);

                    try
                    {
                        //initialise connection
                        using (SqlConnection conn = new SqlConnection(strConn))
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure", "Confirmation", MessageBoxButton.YesNo);
                            if (messageBoxResult == MessageBoxResult.Yes)
                            {
                                //open connection
                                conn.Open();

                                //sql command
                                SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES ('" + _txtUsername + "','" + sha256_txtPassword + "')", conn);

                                //excute cmd, no result return
                                cmd.ExecuteNonQuery();

                                //close and dispose connection
                                conn.Close();
                                conn.Dispose();

                                MessageBox.Show("Account Created");

                                this.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //password empty
                else
                {
                    MessageBox.Show("Enter Password", "Error");
                }
            }
            //username empty
            else
            {
                MessageBox.Show("Enter Username", "Error");
            }
        }

        private void btnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            winUserManagement winusermanagement = new winUserManagement();
            winusermanagement.Top = 30;
            winusermanagement.Left = 30;
            winusermanagement.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
