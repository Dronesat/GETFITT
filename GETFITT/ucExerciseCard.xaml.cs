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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for ucExerciseCard.xaml
    /// </summary>
    public partial class ucExerciseCard : UserControl
    {
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
    }
}
