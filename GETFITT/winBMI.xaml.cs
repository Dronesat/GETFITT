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
    /// Interaction logic for winBMI.xaml
    /// </summary>
    public partial class winBMI : Window
    {
        //textfile path
        string BMItxt = "Tracker/BMI_Tracker.txt";

        //nhs url
        string underweighturl = "https://www.nhs.uk/live-well/healthy-weight/advice-for-underweight-adults/";
        string normalweighturl = "https://www.nhs.uk/live-well/eat-well/";
        string overweighturl = "https://www.nhs.uk/live-well/eat-well/";
        string obesityurl = "https://www.nhs.uk/conditions/obesity/";

        //declare variables
        double height;
        double weight;
        double result;

        public winBMI()
        {
            InitializeComponent();
        }

        private void btnResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //assign value from textbox to height and weight
                height = Double.Parse(txtHeight.Text);
                weight = Double.Parse(txtWeight.Text);

                //BMI equation
                result = weight / (height * height);

                //display the result to lblResult
                lblResult.Content = String.Format("{0:f}", result);

                //underweight range ( x <= 18.5 )
                if (result <= 18.5)
                {
                    lblType.Content = "Underweight";
                    lblResult.Foreground = new SolidColorBrush(Colors.Blue);
                    lblType.Foreground = new SolidColorBrush(Colors.Blue);
                    string line1 = "Being underweight could be a sign you're not eating enough or you may be ill. ";
                    string line2 = "If you're underweight, check out the NHS website";
                    txtSuggestion.Text = line1 + line2;
                    txtLink.Text = underweighturl;
                }
                //normal weight range ( 18.5 < x < 24.9 )
                if (result > 18.5 && result <= 24.9)
                {
                    lblType.Content = "Healthy Weight";
                    lblResult.Foreground = new SolidColorBrush(Colors.Green);
                    lblType.Foreground = new SolidColorBrush(Colors.Green);
                    string line1 = "Keep up the good work! ";
                    string line2 = "For tips on maintaining a healthy weight, check out the NHS website";
                    txtSuggestion.Text = line1 + line2;
                    txtLink.Text = normalweighturl;
                }
                //overweight range ( 25 <= x <= 29.9 )
                if (result >= 25 && result <= 29.9)
                {
                    lblType.Content = "Overweight";
                    lblResult.Foreground = new SolidColorBrush(Colors.Orange);
                    lblType.Foreground = new SolidColorBrush(Colors.Orange);
                    string line1 = "The best way to lose weight if you're overweight is through a combination of diet and exercise. ";
                    string line2 = "Check out the NHS website to find out more";
                    txtSuggestion.Text = line1 + line2;
                    txtLink.Text = overweighturl;
                }
                //obesity range ( x >= 30 )
                if (result >= 30)
                {
                    lblType.Content = "Obesity";
                    lblResult.Foreground = new SolidColorBrush(Colors.Red);
                    lblType.Foreground = new SolidColorBrush(Colors.Red);
                    string line1 = "The best way to lose weight if you're obese is through a combination of diet and exercise, and, in some cases, medicines. ";
                    string line2 = "Check out the NHS website to find out more";
                    txtSuggestion.Text = line1 + line2;
                    txtLink.Text = obesityurl;
                }
            }

            //Catch any errors
            catch (Exception)
            {
                //Show messagebox
                MessageBox.Show("Check your input", "Error");
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            //clear content in Result 
            lblResult.Content = "";
            lblType.Content = "";
            txtSuggestion.Text = "";

            //clear content in weight,height textbox
            txtHeight.Text = string.Empty;
            txtWeight.Text = string.Empty;

            //Change result foreground to white
            lblResult.Foreground = new SolidColorBrush(Colors.White);
            lblType.Foreground = new SolidColorBrush(Colors.White);
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
                if (File.Exists(BMItxt))
                {
                    //check if label is not empty
                    if (lblResult.Content.ToString() != "")
                    {
                        //confirmation messagebox
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to save this result?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                        //if yes then execusted
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            //use streamwriter to write content from label to textfile (param true is to avoid overwriting)
                            StreamWriter SaveFile = new StreamWriter(BMItxt, true);
                            string bmi = DateTime.Now.ToString("d/M/yyyy") + "," + lblResult.Content.ToString();
                            SaveFile.WriteLine(bmi);
                            SaveFile.Close();
                            MessageBox.Show("Compeleted!");
                        }
                    }
                    //if empty
                    else
                    {
                        MessageBox.Show("No result found! Check your input and make a calculation", "Warning");
                    }
                }
                //file not exist, create new textfile
                else
                {
                    File.CreateText(BMItxt).Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
