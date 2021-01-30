using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for ucExerciseCard.xaml
    /// </summary>
    public partial class ucExerciseCard : UserControl
    {
        public Delegate timer_exercise;

        public claExercise currentExercise;

        public ucExerciseCard(claExercise newExercise)
        {
            InitializeComponent();

            //save the pass data on the card
            currentExercise = newExercise;

            //update label 
            lblExerciseName.Content = currentExercise.strExercise;
            lblMovementPattern.Content = currentExercise.strMovementPattern;

            //update image on user control
            string source = @"/Resources/HomeWorkout_Images/" + currentExercise.strMovementPattern + @"/" + currentExercise.strExercise + @".jfif";
            imgImage.Source = new BitmapImage(new Uri(source, UriKind.Relative));
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            //load winHomeExercise window
            winHomeExercise winhomeexercise = new winHomeExercise(currentExercise);
            winhomeexercise.Top = 10;
            winhomeexercise.Left = 30;
            winhomeexercise.Show();
        }
        
        private void btnTimer_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sorry this function still in development","Error");
        }
    }
}
