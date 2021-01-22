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
    /// Interaction logic for winWater.xaml
    /// </summary>
    public partial class winWater : Window
    {
        public winWater()
        {
            InitializeComponent();
        }
        public void HideBottle()
        {
            //hide images of all bottle
            imgBottle1.Visibility = Visibility.Hidden;
            imgBottle2.Visibility = Visibility.Hidden;
            imgBottle3.Visibility = Visibility.Hidden;
            imgBottle4.Visibility = Visibility.Hidden;
            imgBottle5.Visibility = Visibility.Hidden;
            imgBottle6.Visibility = Visibility.Hidden;
            imgBottle7.Visibility = Visibility.Hidden;
            imgBottle8.Visibility = Visibility.Hidden;
        }
        public void ShowBottle(int show)
        {
            //switch statement to show bottle's images
            switch (show)
            {
                case 1:
                    imgBottle1.Visibility = Visibility.Visible;
                    break;
                case 2:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    break;
                case 3:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    break;
                case 4:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    imgBottle4.Visibility = Visibility.Visible;
                    break;
                case 5:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    imgBottle4.Visibility = Visibility.Visible;
                    imgBottle5.Visibility = Visibility.Visible;
                    break;
                case 6:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    imgBottle4.Visibility = Visibility.Visible;
                    imgBottle5.Visibility = Visibility.Visible;
                    imgBottle6.Visibility = Visibility.Visible;
                    break;
                case 7:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    imgBottle4.Visibility = Visibility.Visible;
                    imgBottle5.Visibility = Visibility.Visible;
                    imgBottle6.Visibility = Visibility.Visible;
                    imgBottle7.Visibility = Visibility.Visible;
                    break;
                case 8:
                    imgBottle1.Visibility = Visibility.Visible;
                    imgBottle2.Visibility = Visibility.Visible;
                    imgBottle3.Visibility = Visibility.Visible;
                    imgBottle4.Visibility = Visibility.Visible;
                    imgBottle5.Visibility = Visibility.Visible;
                    imgBottle6.Visibility = Visibility.Visible;
                    imgBottle7.Visibility = Visibility.Visible;
                    imgBottle8.Visibility = Visibility.Visible;
                    break;
            }
        }
        public string GenerateTextFile()
        {
            //declare and add today's date to string
            string today = DateTime.Today.ToString("d");

            //replace / with _
            today = today.Replace('/', '_');

            //declare and assign name to file
            string textfile = "water_count_" + today + ".txt";

            //return filename
            return textfile;
        }
        public void ReadFileBottle(ref int count)
        {
            //try to get file name
            try
            {
                //call filename method
                string textfile = GenerateTextFile();

                //check if there is a file for today
                if (File.Exists(textfile))
                {
                    //read text file to string
                    string line = File.ReadAllText(textfile);

                    //convert string to int32
                    count = Convert.ToInt32(line);

                    //display count to labelBottleCount
                    lblBottleCount.Content = count.ToString();
                }
                else
                {
                    //show messagebox
                    MessageBox.Show("Let's start the day by having a drink first");
                }
            }
            //catch any errors
            catch (Exception)
            {
                //show messagebox
                MessageBox.Show("Sorry, where was an error", "Error");
            }
        }

        public void WriteFileBottle(int count)
        {
            //try to write to file
            try
            {
                //set filename to string and call filename method
                string textfile = GenerateTextFile();

                //delete content in textfile
                File.WriteAllText(textfile, String.Empty);

                //write count to textfile using streamwriter
                using (StreamWriter txt = File.AppendText(textfile))
                {
                    txt.WriteLine(count);
                    txt.Close();
                }
            }
            //catch any errors
            catch (Exception)
            {
                //show messagebox
                MessageBox.Show("Sorry, where was an error", "Error");
            }
        }
        private void winWater_Load(object sender, EventArgs e)
        {
            //declare count variable and assign value
            int count = 0;

            //call HideBottle method to hide all bottle
            HideBottle();

            //call ReadFileBottle method and pass ref count
            ReadFileBottle(ref count);

            //call ShowBottle method 
            ShowBottle(count);

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //declare max,min,bottle variable
            int MAX_Bottle = 8;
            int MIN_Bottle = 0;
            int bottle = 0;

            try
            {
                //call filename method
                string filename = GenerateTextFile();

                //check if there is a file for today
                if (File.Exists(filename))
                {
                    //call ReadFileBottle method and pass ref count
                    ReadFileBottle(ref bottle);

                    //check if bottle is in max, min range 
                    if (bottle > MIN_Bottle && bottle < MAX_Bottle)
                    {
                        //increment bottle by 1
                        bottle++;

                        //call WriteFileBottle method
                        WriteFileBottle(bottle);

                        //call HideBottle method
                        HideBottle();

                        //call ShowBottle method
                        ShowBottle(bottle);
                    }
                }
                else
                {
                    //increment bottle by 1
                    bottle++;

                    //display to lblBottleCount
                    lblBottleCount.Content = bottle.ToString();

                    //call WriteFileBottle method
                    WriteFileBottle(bottle);

                    //call ShowBottle method
                    ShowBottle(bottle);
                }
            }
            //catch any errors
            catch (Exception)
            {
                //show messagebox
                MessageBox.Show("Sorry, where was an error", "Error");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //close window
            this.Close();
        }
    }
}
