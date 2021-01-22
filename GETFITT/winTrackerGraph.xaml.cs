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
using LiveCharts;
using LiveCharts.Wpf;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winTrackerGraph.xaml
    /// </summary>
    public partial class winTrackerGraph : Window
    {
        //declare string textfile path
        string BMRtxt = "Tracker/BMR_Tracker.txt";
        string BMItxt = "Tracker/BMI_Tracker.txt";
        string Weighttxt = "Tracker/Weight_Tracker.txt";

        //BMR graph
        public SeriesCollection SeriesCollectionBMR { get; set; }
        public string[] LabelsBMR { get; set; }
        public Func<double, string> YFormatterBMR { get; set; }

        //BMI graph
        public SeriesCollection SeriesCollectionBMI { get; set; }
        public string[] LabelsBMI { get; set; }
        public Func<double, string> YFormatterBMI { get; set; }

        //Weight graph
        public SeriesCollection SeriesCollectionWeight { get; set; }
        public string[] LabelsWeight { get; set; }
        public Func<double, string> YFormatterWeight { get; set; }
        public winTrackerGraph()
        {
            InitializeComponent();
            BMRGraph(BMRtxt);
            BMIGraph(BMItxt);
            WeightGraph(Weighttxt);
        }
        public void BMRGraph(string textfile)
        {
            //read from file
            string[] arrLines = System.IO.File.ReadAllLines(textfile);

            //list of value and date
            List<double> lstBMRvalue = new List<double>();
            List<string> lstBMRdate = new List<string>();

            //split each line part
            foreach (string line in arrLines)
            {
                //create parts array
                string[] arrParts = line.Split(',');

                //index array
                double BMRvalue = Convert.ToDouble(arrParts[1]);
                string BMRdate = arrParts[0];

                //add value and date to corresponding lists
                lstBMRdate.Add(BMRdate);
                lstBMRvalue.Add(BMRvalue);
            }
            //convert list to array
            string[] arrBMRdate = lstBMRdate.ToArray();
            double[] arrBMRvalue = lstBMRvalue.ToArray();

            //draw graph using livechart
            SeriesCollectionBMR = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BMR",
                    Values = new ChartValues<double>(arrBMRvalue),
                    PointGeometrySize = 10
                },
            };
            LabelsBMR = arrBMRdate;
            YFormatterBMR = value => value + "";
            DataContext = this;
        }
        public void BMIGraph(string textfile)
        {
            //read from file
            string[] arrLines = System.IO.File.ReadAllLines(textfile);

            //list of value and date
            List<double> lstBMIvalue = new List<double>();
            List<string> lstBMIdate = new List<string>();

            //split each line part
            foreach (string line in arrLines)
            {
                //create parts array
                string[] arrParts = line.Split(',');

                double BMIvalue = Convert.ToDouble(arrParts[1]);
                string BMIdate = arrParts[0];

                //add value and date to corresponding lists
                lstBMIdate.Add(BMIdate);
                lstBMIvalue.Add(BMIvalue);
            }
            //convert list to array
            string[] arrBMIdate = lstBMIdate.ToArray();
            double[] arrBMIvalue = lstBMIvalue.ToArray();

            SeriesCollectionBMI = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BMI",
                    Values = new ChartValues<double>(arrBMIvalue),
                    PointGeometrySize = 10
                },
            };
            LabelsBMI = arrBMIdate;
            YFormatterBMI = value => value + "";
            DataContext = this;
        }
        public void WeightGraph(string textfile)
        {
            //read from file
            string[] arrLines = System.IO.File.ReadAllLines(textfile);

            //list of value and date
            List<double> lstWeightkg = new List<double>();
            List<string> lstWeightdate = new List<string>();

            //split each line part
            foreach (string line in arrLines)
            {
                //create parts array
                string[] arrParts = line.Split(',');

                double Weightkg = Convert.ToDouble(arrParts[1]);
                string Weightdate = arrParts[0];

                //add value and date to corresponding lists
                lstWeightdate.Add(Weightdate);
                lstWeightkg.Add(Weightkg);
            }
            //convert list to array
            string[] arrWeightdate = lstWeightdate.ToArray();
            double[] arrWeightkg = lstWeightkg.ToArray();

            SeriesCollectionWeight = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Weight",
                    Values = new ChartValues<double>(arrWeightkg),
                    PointGeometrySize = 10
                },
            };
            LabelsWeight = arrWeightdate;
            YFormatterWeight = value => value + "kg";
            DataContext = this;
        }
    }
}
