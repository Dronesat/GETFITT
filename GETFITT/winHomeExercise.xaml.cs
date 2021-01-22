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
    /// Interaction logic for winHomeExercise.xaml
    /// </summary>
    public partial class winHomeExercise : Window
    {
        claExercise currentExercise;
        public winHomeExercise(claExercise newExercise)
        {
            InitializeComponent();

            currentExercise = newExercise;

            //update visual display
            lblExercise.Content = currentExercise.strExercise;
            lblMovementPattern.Content = currentExercise.strMovementPattern;
            txtInstruction.Text = currentExercise.strInstruction;

            //update image
            string source = @"/Resources/HomeWorkout_Images/" + currentExercise.strMovementPattern + @"/" + currentExercise.strExercise + @".jfif";
            imgImage.Source = new BitmapImage(new Uri(source, UriKind.Relative));
        }
    }
}
