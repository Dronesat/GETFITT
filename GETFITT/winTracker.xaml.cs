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
using System.IO;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winTracker.xaml
    /// </summary>
    public partial class winTracker : Window
    {
        //declare string textfile path
        string BMRtxt = "Tracker/BMR_Tracker.txt";
        string BMItxt = "Tracker/BMI_Tracker.txt";
        string Weighttxt = "Tracker/Weight_Tracker.txt";
        public winTracker()
        {
            InitializeComponent();
            Startup();
        }
        private void Startup()
        {
            //update listbox at startup
            UpdateListBox(BMItxt);
            UpdateListBox(BMRtxt);
            UpdateListBox(Weighttxt);
        }
        public void UpdateListBox(string textfile)
        {
            try
            {
                //check if file exist
                if (File.Exists(textfile))
                {
                    //use streamreader to display listbox with textfile content    
                    using (StreamReader sr = new StreamReader(textfile))
                    {
                        string line;
                        //while loop to read through textfile output to line
                        while ((line = sr.ReadLine()) != null)
                        {
                            //display to corresponding listbox
                            if(textfile == BMRtxt)
                            {
                                lstBMR.Items.Add(line);
                            }
                            if(textfile == BMItxt)
                            {
                                lstBMI.Items.Add(line);
                            }
                            if(textfile == Weighttxt)
                            {
                                lstWeight.Items.Add(line);
                            }                           
                        }
                        sr.Close();
                    }
                }
                //not exist, create new textfile
                else
                {
                    File.CreateText(textfile).Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddBMR_Click(object sender, RoutedEventArgs e)
        {
            //check textbox not empty
            if(txtBMR.Text != "")
            {
                //check input is double
                if(double.TryParse(txtBMR.Text, out double doublebmr))
                {
                    //add content to listbox
                    string bmr = DateTime.Now.ToString("d/M/yyyy") + "," + doublebmr;
                    lstBMR.Items.Add(bmr);
                    //empty textbox
                    txtBMR.Text = "";
                }
                //not double
                else
                {
                    //chang textbox color to red
                    txtBMR.Background = new SolidColorBrush(Colors.Red);

                    //error message
                    MessageBox.Show("Incorrect Format","Error");

                    //empty textbox
                    txtBMR.Text = "";

                    //change textbox color to white
                    txtBMR.Background = new SolidColorBrush(Colors.White);
                }
            }
            //textbox empty
            else
            {
                //chang textbox color to red
                txtBMR.Background = new SolidColorBrush(Colors.Red);

                //error message
                MessageBox.Show("Enter BMR value","Error");

                //empty textbox
                txtBMR.Text = "";

                //change textbox color to white
                txtBMR.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnAddBMI_Click(object sender, RoutedEventArgs e)
        {
            //check textbox not empty
            if (txtBMI.Text != "")
            {
                //check input is double
                if (double.TryParse(txtBMI.Text, out double doublebmi))
                {
                    //add content to listbox
                    string bmi = DateTime.Now.ToString("d/M/yyyy") + "," + doublebmi;
                    lstBMI.Items.Add(bmi);
                    //empty textbox
                    txtBMI.Text = "";
                }
                //not double
                else
                {
                    //chang textbox color to red
                    txtBMI.Background = new SolidColorBrush(Colors.Red);

                    //error message
                    MessageBox.Show("Incorrect Format","Error");

                    //empty textbox
                    txtBMI.Text = "";

                    //change textbox color to white
                    txtBMI.Background = new SolidColorBrush(Colors.White);
                }
            }
            //textbox empty
            else
            {
                //chang textbox color to red
                txtBMI.Background = new SolidColorBrush(Colors.Red);

                //error message
                MessageBox.Show("Enter BMI value","Error");

                //empty textbox
                txtBMI.Text = "";

                //change textbox color to white
                txtBMI.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnAddWeight_Click(object sender, RoutedEventArgs e)
        {
            //check textbox not empty
            if (txtWeight.Text != "")
            {
                //check input is double
                if (double.TryParse(txtWeight.Text, out double doubleweight))
                {
                    //add content to listbox
                    string weight = DateTime.Now.ToString("d/M/yyyy") + "," + doubleweight;
                    lstWeight.Items.Add(weight);
                    //empty textbox
                    txtWeight.Text = "";
                }
                //not double
                else
                {
                    //chang textbox color to red
                    txtWeight.Background = new SolidColorBrush(Colors.Red);

                    //error message
                    MessageBox.Show("Incorrect Format","Error");

                    //empty textbox
                    txtWeight.Text = "";

                    //change textbox color to white
                    txtWeight.Background = new SolidColorBrush(Colors.White);
                }
            }
            //textbox empty
            else
            {
                //chang textbox color to red
                txtWeight.Background = new SolidColorBrush(Colors.Red);

                //error message
                MessageBox.Show("Enter Weight","Error");

                //empty textbox
                txtWeight.Text = "";

                //change textbox color to white
                txtWeight.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnSaveBMR_Click(object sender, RoutedEventArgs e)
        {
            //try to write file
            try
            {
                //use streamwrite with for loop to write all items in listbox to textfile
                StreamWriter SaveFile = new StreamWriter(BMRtxt);
                foreach (var item in lstBMR.Items)
                {
                    SaveFile.WriteLine(item);
                }

                //close streamwriter
                SaveFile.Close();

                //show messagebox
                MessageBox.Show("Saved to file");
            }
            //catch any error
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveBMI_Click(object sender, RoutedEventArgs e)
        {
            //try to write file
            try
            {
                //use streamwrite with for loop to write all items in listbox to textfile
                StreamWriter SaveFile = new StreamWriter(BMItxt);
                foreach (var item in lstBMI.Items)
                {
                    SaveFile.WriteLine(item);
                }

                //close streamwriter
                SaveFile.Close();

                //show messagebox
                MessageBox.Show("Saved to file");
            }
            //catch any error
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveWeight_Click(object sender, RoutedEventArgs e)
        {
            //try to write file
            try
            {
                //use streamwrite with for loop to write all items in listbox to textfile
                StreamWriter SaveFile = new StreamWriter(Weighttxt);
                foreach (var item in lstWeight.Items)
                {
                    SaveFile.WriteLine(item);
                }

                //close streamwriter
                SaveFile.Close();

                //show messagebox
                MessageBox.Show("Saved to file");
            }
            //catch any error
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void EmptyFile(string filename)
        {
            File.Create(filename).Dispose();
        }

        private void btnDeleteBMR_Click(object sender, RoutedEventArgs e)
        {
            //confirmation messagebox
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure? Once deleted, file can not be recorvered!", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //empty textfile
                EmptyFile(BMRtxt);
                //clear listbox
                lstBMR.Items.Clear();
            }
        }

        private void btnDeleteBMI_Click(object sender, RoutedEventArgs e)
        {
            //confirmation messagebox
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure? Once deleted, file can not be recorvered!", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //empty textfile
                EmptyFile(BMItxt);
                //clear listbox
                lstBMI.Items.Clear();
            }
        }

        private void btnDeleteWeight_Click(object sender, RoutedEventArgs e)
        {
            //confirmation messagebox
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure? Once deleted, file can not be recorvered!", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //empty textfile
                EmptyFile(Weighttxt);
                //clear listbox
                lstWeight.Items.Clear();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnTrackerGraph_Click(object sender, RoutedEventArgs e)
        {
            //show wintrackergraph window
            winTrackerGraph wintrackergraph = new winTrackerGraph();
            wintrackergraph.Top = 10;
            wintrackergraph.Left = 30;
            wintrackergraph.Show();
        }
    }
}
