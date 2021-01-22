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
    /// Interaction logic for winBMR.xaml
    /// </summary>
    public partial class winBMR : Window
    {
        //declare string textfile path
        string BMRtxt = "Tracker/BMR_Tracker.txt";

        //declare constant for BMR equation
        const double WEIGHTCST = 10;
        const double HEIGHTCST = 6.25;
        const double AGECST = 5;
        const double MENCST = 5;
        const double WOMENCST = 161;

        //declare energy activity constant
        const double sedentary_light = 1.53;
        const double active_moderately = 1.76;
        const double vigorously = 2.25;
        double calories = 0;

        //Declare variable
        double height;
        double weight;
        double age;
        public winBMR()
        {
            InitializeComponent();
        }
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            //show message box
            string message = "Basal Metabolic Rate is the number of calories required to keep your body functioning at rest. BMR is also known as your body’s metabolism; therefore, any increase to your metabolic weight, such as exercise, will increase your BMR. To get your BMR, simply input your height, gender, age and weight. Once you’ve determined your BMR, you can begin to monitor how many calories a day you need to maintain or lose weight.";
            string caption = "About BMR";
            MessageBox.Show(message, caption);
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //assign value from textbox to height and weight
                height = Double.Parse(txtHeight.Text);
                weight = Double.Parse(txtWeight.Text);
                age = Double.Parse(txtAge.Text);

                //declare BMR
                double BMR = 0;
                if (radioBtnFemale.IsChecked == true)
                {
                    //female BMR equation
                    BMR = (WEIGHTCST * weight) + (HEIGHTCST * height) - (AGECST * age) - WOMENCST;
                }
                else if (radioBtnMale.IsChecked == true)
                {
                    //men BMR equation
                    BMR = (WEIGHTCST * weight) + (HEIGHTCST * height) - (AGECST * age) + MENCST;
                }

                //declare switch statement variable = listbox.selectedindex
                int physicalactivitylevel = listBox.SelectedIndex;

                //switch statement for user to select thier activity
                switch (physicalactivitylevel)
                {
                    case 0:
                        calories = BMR * sedentary_light;
                        break;
                    case 1:
                        calories = BMR * active_moderately;
                        break;
                    case 2:
                        calories = BMR * vigorously;
                        break;
                    default:
                        MessageBox.Show("Make sure you select your acitvity");
                        break;
                }

                //check calories value range (0 - 7100)
                if (calories <= 0 || calories >= 7100)
                {
                    //Show messagebox
                    MessageBox.Show("Calories value out of range (0-7100). Check your input", "Error");
                }
                //Display result if there is no error
                else lblResult.Content = String.Format("{0:f}", calories);
            }
            //Catch any errors
            catch (Exception)
            {
                //Show messagebox
                MessageBox.Show("Check your input", "Error");
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //close the window
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if file exist
                if (File.Exists(BMRtxt))
                {
                    if (lblResult.Content.ToString() != "")
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to save this result?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            //use streamwriter to write content from label to textfile (param true is to avoid overwriting)
                            StreamWriter SaveFile = new StreamWriter(BMRtxt, true);
                            string bmr = DateTime.Now.ToString("d/M/yyyy") + "," + lblResult.Content.ToString();
                            SaveFile.WriteLine(bmr);
                            SaveFile.Close();
                            MessageBox.Show("Compeleted!");
                        } 
                    }
                    else
                    {
                        MessageBox.Show("Check your input and make a calculation","Warning");
                    }
                }
                //file not exist, create new textfile
                else
                {
                    File.CreateText(BMRtxt).Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
