using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winHealthMonitor.xaml
    /// </summary>
    public partial class winHealthMonitor : Window, INotifyPropertyChanged
    {
        //connection string
        readonly string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

        //livechart Angular Gauge
        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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
        double result;

        //nhs url
        string underweighturl = "https://www.nhs.uk/live-well/healthy-weight/advice-for-underweight-adults/";
        string normalweighturl = "https://www.nhs.uk/live-well/eat-well/";
        string overweighturl = "https://www.nhs.uk/live-well/eat-well/";
        string obesityurl = "https://www.nhs.uk/conditions/obesity/";

        public winHealthMonitor()
        {
            InitializeComponent();

            //live chart initial value
            Value = 20;
            DataContext = this;
        }

        private void btnBMRInfo_Click(object sender, RoutedEventArgs e)
        {
            //show message box
            string message1 = "Basal Metabolic Rate is the number of calories required to keep your body functioning at rest.";
            string message2 = "BMR is also known as your body’s metabolism; therefore, any increase to your metabolic weight, such as exercise, will increase your BMR.";
            string message3 = "To get your BMR, simply input your height, gender, age and weight. Once you’ve determined your BMR, you can begin to monitor how many calories a day you need to maintain or lose weight.";
            string caption = "About BMR";
            MessageBox.Show(message1 + "\n" + message2 + "\n" + message3, caption);
        }
        private void btnBMIInfo_Click(object sender, RoutedEventArgs e)
        {
            string message1 = "The body mass index (BMI) is a measure that uses your height and weight to work out if your weight is healthy.";
            string message2 = "The BMI calculation divides an adult's weight in kilograms by their height in metres squared. For example, A BMI of 25 means 25kg/m2.";
            string caption = "About BMI";
            MessageBox.Show(message1 + "\n" + message2, caption);
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            bool isHeight = double.TryParse(txtHeight.Text, out double height);
            bool isWeight = double.TryParse(txtWeight.Text, out double weight);
            bool isAge = int.TryParse(txtAge.Text, out int age);

            if (isHeight == true && height > 0 && height <= 250)
            {
                if (isWeight == true && weight > 0 && weight <= 200)
                {
                    if (isAge == true && age > 0 && age <= 100)
                    {
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
                        else if (radioBtnFemale.IsChecked == false && radioBtnMale.IsChecked == false)
                        {
                            MessageBox.Show("Select gender");
                            return;
                        }

                        //switch statement for user to select thier activity
                        switch (lstActivityLevel.SelectedIndex)
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
                                return;
                        }

                        //display BMR result
                        lblBMRResult.Content = String.Format("{0:f}", calories);

                        //BMI equation
                        result = weight / ((height / 100) * (height / 100));

                        //update Angular Gauge with result
                        Value = result;

                        //display the result to lblBMIResult
                        lblBMIResult.Content = String.Format("{0:f}", result);

                        //underweight range ( x <= 18.5 )
                        if (result <= 18.5)
                        {
                            lblType.Content = "Underweight";
                            lblBMIResult.Foreground = new SolidColorBrush(Colors.Blue);
                            lblType.Foreground = new SolidColorBrush(Colors.Blue);
                            string line1 = "Being underweight could be a sign you're not eating enough or you may be ill. ";
                            string line2 = "If you're underweight, check out the NHS website";
                            txtBMISugesstion.Text = line1 + line2;
                            txtBMILink.Text = underweighturl;
                        }
                        //normal weight range ( 18.5 < x < 24.9 )
                        else if (result > 18.5 && result <= 24.9)
                        {
                            lblType.Content = "Healthy Weight";
                            lblBMIResult.Foreground = new SolidColorBrush(Colors.Green);
                            lblType.Foreground = new SolidColorBrush(Colors.Green);
                            string line1 = "Keep up the good work! ";
                            string line2 = "For tips on maintaining a healthy weight, check out the NHS website";
                            txtBMISugesstion.Text = line1 + line2;
                            txtBMILink.Text = normalweighturl;

                        }
                        //overweight range ( 25 <= x <= 29.9 )
                        else if (result >= 25 && result <= 29.9)
                        {
                            lblType.Content = "Overweight";
                            lblBMIResult.Foreground = new SolidColorBrush(Colors.Orange);
                            lblType.Foreground = new SolidColorBrush(Colors.Orange);
                            string line1 = "The best way to lose weight if you're overweight is through a combination of diet and exercise. ";
                            string line2 = "Check out the NHS website to find out more";
                            txtBMISugesstion.Text = line1 + line2;
                            txtBMILink.Text = overweighturl;
                        }
                        //obesity range ( x >= 30 )
                        else if (result >= 30)
                        {
                            lblType.Content = "Obesity";
                            lblBMIResult.Foreground = new SolidColorBrush(Colors.Red);
                            lblType.Foreground = new SolidColorBrush(Colors.Red);
                            string line1 = "The best way to lose weight if you're obese is through a combination of diet and exercise, and, in some cases, medicines. ";
                            string line2 = "Check out the NHS website to find out more";
                            txtBMISugesstion.Text = line1 + line2;
                            txtBMILink.Text = obesityurl;
                        }
                        MessageBox.Show("Completed!");
                    }
                    else
                    {
                        //change textbox color to red
                        txtAge.Background = new SolidColorBrush(Colors.Red);

                        //error message
                        MessageBox.Show("Check input age", "Error");

                        //change textbox color to white
                        txtAge.Background = new SolidColorBrush(Colors.White);

                        //focus on textbox
                        txtAge.Focus();
                    }
                }
                else
                {
                    //change textbox color to red
                    txtWeight.Background = new SolidColorBrush(Colors.Red);

                    //error message
                    MessageBox.Show("Check input weight", "Error");

                    //change textbox color to white
                    txtWeight.Background = new SolidColorBrush(Colors.White);

                    //focus on textbox
                    txtWeight.Focus();
                }
            }
            else
            {
                //change textbox color to red
                txtHeight.Background = new SolidColorBrush(Colors.Red);

                //Error message
                MessageBox.Show("Check input height", "Error");

                //change textbox color to white
                txtHeight.Background = new SolidColorBrush(Colors.White);

                //focus on textbox
                txtHeight.Focus();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if input is not empty
                if (lblBMRResult.Content.ToString() != "" && lblBMIResult.Content.ToString() != "" && txtWeight.Text.ToString() != "")
                {
                    //confirmation messagebox
                    MessageBoxResult messageBoxResult = MessageBox.Show("Save current Weight, BMI and BMR to database?", "Confirmation", MessageBoxButton.YesNo);
                    //if yes then execusted
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        //initialise connection
                        using (SqlConnection conn = new SqlConnection(strConn))
                        {
                            string user_id = App.Current.Properties["id"].ToString();
                            string bmr = lblBMRResult.Content.ToString();
                            string bmi = lblBMIResult.Content.ToString();
                            string weight = txtWeight.Text.ToString();
                            string date = DateTime.Now.ToString("yyyy-MM-dd");

                            //sql command
                            SqlCommand cmd = new SqlCommand("INSERT INTO HealthStatus VALUES ('" + user_id + "','" + weight + "','" + bmi + "','" + bmr + "','" + date + "')", conn);

                            //open connection
                            conn.Open();

                            //excute cmd, no result return
                            cmd.ExecuteNonQuery();

                            //close and release
                            conn.Close();
                            conn.Dispose();

                            MessageBox.Show("Saved to Database");
                        }
                    }
                }
                //empty
                else
                {
                    MessageBox.Show("Check your input or make a calculation", "Warning");
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //close this window
            this.Close();

            //open this window
            winHealthMonitor winhealthmonitor = new winHealthMonitor();
            winhealthmonitor.Top = 30;
            winhealthmonitor.Left = 30;
            winhealthmonitor.Show();
        }
    }
}
