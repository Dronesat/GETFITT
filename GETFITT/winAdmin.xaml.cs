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
    /// Interaction logic for winAdmin.xaml
    /// </summary>
    public partial class winAdmin : Window
    {
        linqUsersDataContext datacontext = new linqUsersDataContext(Properties.Settings.Default.UsersConnectionString);
        public winAdmin()
        {
            InitializeComponent();
            if (datacontext.DatabaseExists())
                UsersDataGrid.ItemsSource = datacontext.Users;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                datacontext.SubmitChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
