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
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {
        SqlConnection con = new SqlConnection();
        public winLogin()
        {
            InitializeComponent();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("select Role from Users where username='" + txtUsername.Text + "' and password='" + txtPassword.Text + "'", con);
            DataTable dt = new System.Data.DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                switch (dt.Rows[0]["Role"] as string)
                {
                    case "Admin":
                        {
                            // calls the form’s constructor. 
                            winAdmin winadmin = new winAdmin();
                            winadmin.Show();
                            break;
                        }

                    case "User":
                        {
                            this.Hide();
                            winMain winmain = new winMain();
                            winmain.Show();
                            break;
                        }

                    default:
                        {
                            MessageBox.Show("Contact admin for asisstant", "Login failed");
                            break;
                        }
                }
            }
        }
    }
}
