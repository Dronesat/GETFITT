using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace GETFITT
{
    /// <summary>
    /// Interaction logic for winTrackerGraph.xaml
    /// </summary>
    public partial class winTrackerGraph : Window
    {
        //connection string
        string strConn = ConfigurationManager.ConnectionStrings["dbGETFITTConnectionString"].ToString();

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

        //declare array
        double[] arrBMR;
        double[] arrBMI;
        double[] arrWeight;
        string[] arrDate;

        public winTrackerGraph()
        {
            InitializeComponent();
            LoadFromDatabase();
            BMRGraph();
            BMIGraph();
            WeightGraph();
            LoadDataGrid();
        }
        private void LoadDataGrid()
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string user_id = App.Current.Properties["id"].ToString();

                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT * FROM HealthStatus WHERE user_id = '" + user_id + "'", conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    //populate dataGridEditor with datatable
                    dataGridEditor.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadFromDatabase()
        {
            try
            {
                //initialize connection
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string user_id = App.Current.Properties["id"].ToString();

                    //sql command
                    SqlCommand cmd = new SqlCommand("SELECT * FROM HealthStatus WHERE user_id = '" + user_id + "'", conn);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    //fill datatable
                    da.Fill(dt);

                    //close and release
                    conn.Close();
                    conn.Dispose();

                    //create list
                    List<double> lstBMR = new List<double>();
                    List<double> lstBMI = new List<double>();
                    List<double> lstWeight = new List<double>();
                    List<string> lstDate = new List<string>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        double bmi = Convert.ToDouble(dr["bmi"].ToString());
                        double bmr = Convert.ToDouble(dr["bmr"].ToString());
                        double weight = Convert.ToDouble(dr["weight"].ToString());
                        DateTime datetime = Convert.ToDateTime(dr["date"].ToString());
                        string date = Convert.ToString(datetime.ToShortDateString());

                        //add to list
                        lstBMI.Add(bmi);
                        lstBMR.Add(bmr);
                        lstWeight.Add(weight);
                        lstDate.Add(date);
                    }
                    //convert list to array
                    arrBMI = lstBMI.ToArray();
                    arrBMR = lstBMR.ToArray();
                    arrWeight = lstWeight.ToArray();
                    arrDate = lstDate.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BMRGraph()
        {
            //draw graph using livechart
            SeriesCollectionBMR = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BMR",
                    Values = new ChartValues<double>(arrBMR),
                    PointGeometrySize = 10
                },
            };
            LabelsBMR = arrDate;
            YFormatterBMR = value => value + "";
            DataContext = this;
        }

        private void BMIGraph()
        {
            SeriesCollectionBMI = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "BMI",
                    Values = new ChartValues<double>(arrBMI),
                    PointGeometrySize = 10
                },
            };
            LabelsBMI = arrDate;
            YFormatterBMI = value => value + "";
            DataContext = this;
        }

        private void WeightGraph()
        {
            SeriesCollectionWeight = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Weight",
                    Values = new ChartValues<double>(arrWeight),
                    PointGeometrySize = 10
                },
            };
            LabelsWeight = arrDate;
            YFormatterWeight = value => value + "kg";
            DataContext = this;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtDeleteID.Text, out int delete_id))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(strConn);

                    //open connection
                    conn.Open();

                    //execute sql cmd
                    string strcmd = "DELETE FROM HealthStatus WHERE id = '" + delete_id + "'";

                    //execute cmd
                    SqlCommand cmd = new SqlCommand(strcmd, conn);
                    cmd.ExecuteNonQuery();

                    //close connection
                    conn.Close();
                    conn.Dispose();

                    //update datagrid 
                    LoadDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Checked entered ID", "Error");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
