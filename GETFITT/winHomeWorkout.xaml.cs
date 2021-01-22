using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winHomeWorkout.xaml
    /// </summary>
    public partial class winHomeWorkout : Window
    {
        public winHomeWorkout()
        {
            InitializeComponent();

            //creating hashset for the MovementPattern
            HashSet<string> hshMovementPatterns = new HashSet<string>();

            //read from homeworkout.txt 
            string[] arrLines = System.IO.File.ReadAllLines(@"HomeWorkout.txt");

            //split each line part
            foreach (string line in arrLines)
            {
                //create parts array
                string[] arrParts = line.Split(',');

                //create object for class
                claExercise newExercise = new claExercise();

                newExercise.strMovementPattern = arrParts[0];
                newExercise.strExercise = arrParts[1];
                newExercise.strInstruction = arrParts[2];

                //add the movementpatter to the hashset
                hshMovementPatterns.Add(newExercise.strMovementPattern);

                //create usercontrol
                ucExerciseCard newCard = new ucExerciseCard(newExercise);

                //add to list of Exercises
                claDataStore.lstExercises.Add(newCard);
            }
            foreach (string MovementPattern in hshMovementPatterns)
                lsbMovementPattern.Items.Add(MovementPattern);
        }

        private void btnSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear the wrap panel 
                wraExercise.Children.Clear();
                foreach (ucExerciseCard currentCard in claDataStore.lstExercises)
                {
                    if (currentCard.currentExercise.strMovementPattern == lsbMovementPattern.SelectedItem.ToString())
                    {
                        wraExercise.Children.Add(currentCard);
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a Movement Pattern", "Warning");
            }
        }
        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            //clear the wrap panel
            wraExercise.Children.Clear();

            //for each pet create and display a user control in the wrap panel
            foreach (ucExerciseCard currentExercise in claDataStore.lstExercises)
            {
                //add new card to wrap panel
                wraExercise.Children.Add(currentExercise);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //check if exercise exist or not
            if(txtExercise.Text != "")
            {
                //check if time is exist or not
                if (IntegerExtensions.ParseNullableInt(txtTime.Text) >= 0)
                {
                    //add input from textbox to listbox
                    lstExercise.Items.Add(txtExercise.Text);
                    lstTime.Items.Add(txtTime.Text);

                    //empty string and reset focus
                    txtExercise.Text = "";
                    txtTime.Text = "";
                    txtExercise.Focus();
                }
                //no time
                else 
                {
                    //change textbox color to red
                    txtTime.Background = new SolidColorBrush(Colors.Red);

                    //error messagebox
                    MessageBox.Show("Check Time");

                    //put empty string in txtTime
                    txtTime.Text = "";

                    //change textbox color to white
                    txtTime.Background = new SolidColorBrush(Colors.White);
                    //focus on txtTime
                    txtTime.Focus();
                }
            }
            //no exercise
            else
            {
                //change textbox color to red
                txtExercise.Background = new SolidColorBrush(Colors.Red);
                //error messagebox
                MessageBox.Show("Check Exercise");

                //put empty string in txtTime
                txtExercise.Text = "";

                //focus on txtTime
                txtExercise.Focus();

                //change textbox color to white
                txtExercise.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //check if resttime exist or not 
            if (IntegerExtensions.ParseNullableInt(txtRestTime.Text) > 0)
            {
                //check item in listbox
                if (lstTime.Items.Count > 0)
                {
                    MessageBox.Show("Stopwatch starts Now");
                }
                //no item
                else
                {
                    //empty txtTime 
                    txtTime.Text = "";

                    //change textbox color to red
                    txtTime.Background = new SolidColorBrush(Colors.Red);

                    //show messagebox
                    MessageBox.Show("Add time");

                    //change textbox color to white
                    txtTime.Background = new SolidColorBrush(Colors.White);
                }
            }
            //no rest time
            else
            {
                //empty txtTime 
                txtRestTime.Text = "";

                //change textbox color to red
                txtRestTime.Background = new SolidColorBrush(Colors.Red);

                //show messagebox
                MessageBox.Show("Add rest time");

                //change textbox color to white
                txtRestTime.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //clear listbox and textbox
            lstExercise.Items.Clear();
            lstTime.Items.Clear();
            txtExercise.Text = "";
            txtRestTime.Text = "";
            txtTime.Text = "";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //loop through each exercises
            for (int i = 0; i < lstExercise.Items.Count; i++)
            {
                lblExercise.Content = lstExercise.Items[i].ToString();
                int time = int.Parse(lstTime.Items[i].ToString());

                //loop time
                for (int j = time; j >= 0; j--)
                {
                    //Declare TimeSpan
                    TimeSpan timespan = new TimeSpan(0, 0, j);

                    //update label stopwatch
                    lblStopwatch.Content = timespan.ToString(@"mm\:ss");
                    Thread.Sleep(1000);
                    System.Windows.Forms.Application.DoEvents();
                    if (j < 3)
                    {
                        SystemSounds.Beep.Play();
                    }
                }

                int resttime = IntegerExtensions.ParseInt(txtRestTime.Text);
                for (int k = resttime; k >= 0; k--)
                {
                    //declare TimeSpan
                    TimeSpan timespan1 = new TimeSpan(0, 0, k);

                    //display Rest 
                    lblExercise.Content = "Rest";

                    //update label stopwatch
                    lblStopwatch.Content = timespan1.ToString(@"mm\:ss");
                    Thread.Sleep(1000);
                    System.Windows.Forms.Application.DoEvents();
                    if (k < 3)
                    {
                        SystemSounds.Beep.Play();
                    }
                }
            }
            //update label Exercise to Finished
            lblExercise.Content = "Finished";
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
