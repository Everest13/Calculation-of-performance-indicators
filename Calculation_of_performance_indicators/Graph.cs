using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculation_of_performance_indicators
{
    public partial class Graph : Form
    {
        public Graph(int [] time, float [] arr_graf, string secondValue)
        {
            InitializeComponent();
            this.time = time;
            this.arr_graf = arr_graf;
            this.secondValue = secondValue;
        }
        int[] time;
        float[] arr_graf;
        string secondValue;
        private void Graph_Load(object sender, EventArgs e)
        {

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.ChartAreas[0].AxisX.Minimum = time[0];
            chart1.ChartAreas[0].AxisX.Maximum = time.Count()-1;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
            chart1.ChartAreas[0].AxisX.Title = "time";

            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Title = secondValue;
            chart1.Series[0].Name = secondValue;

            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[1].Name = secondValue+" opt";

            for (
                int i = time[0]; i < time.Count(); i++)
            {
                chart1.Series[0].Points.AddXY(time[i], arr_graf[i]);
                chart1.Series[1].Points.AddXY(time[i], arr_graf[arr_graf.Count()-2]);
            }
                
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
