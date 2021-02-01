using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        public winLogin()
        {
            InitializeComponent();
        }

        private void btnCreateAcc_Click(object sender, RoutedEventArgs e)
        {
            //open winCreateAccount window
            winCreateAccount wincreateaccount = new winCreateAccount();
            wincreateaccount.Top = 30;
            wincreateaccount.Top = 30;
            wincreateaccount.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //trim left and right
            string _txtUsername = txtUsername.Text.Trim();
            string _txtPassword = txtPassword.Password.Trim();

            //initialise connection
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT id, username, password FROM Users WHERE username ='" + _txtUsername + "'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //check row count
                if (dt.Rows.Count > 0)
                {
                    //input password equal one store on database
                    if (ComputeSha256Hash(_txtPassword).ToString() == dt.Rows[0]["password"].ToString())
                    {
                        //Get user id
                        SqlDataAdapter da_id = new SqlDataAdapter("SELECT id FROM Users WHERE username = '" + _txtUsername + "' AND password = '" + ComputeSha256Hash(_txtPassword).ToString() + "'", conn);
                        DataTable dt_id = new DataTable();
                        da_id.Fill(dt_id);

                        //close and release
                        conn.Close();
                        conn.Dispose();

                        foreach (DataRow dr in dt_id.Rows)
                        {
                            string id = dr["id"].ToString();

                            //set user id to global
                            App.Current.Properties["id"] = id;
                        }

                        //set username string global
                        App.Current.Properties["username"] = _txtUsername;


                        //open winmain
                        winMain winmain = new winMain();
                        winmain.Top = 30;
                        winmain.Left = 30;
                        winmain.Show();

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                //row not found
                else
                {
                    MessageBox.Show("Login Fail");
                }
            }
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
