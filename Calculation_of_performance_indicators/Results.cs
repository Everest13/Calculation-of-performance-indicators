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
    public partial class Results : Form
    {
        private Parameters refForm;
          public Results(Parameters refForm)
          {
              InitializeComponent();
              this.refForm = refForm;
          }

          int[] time_graph;
          float[] limbda_graph;
          float[] Mu_graph;
          float[] Ro_graph;
          float[] Potk_graph;
          float[] Poch_graph;
          float[] Q_graph;
          float[] A_graph;
          float[] kzan_graph;
          float[] Loch_graph;
          float[] Toch_graph;
          float[] Lsist_graph;
          float[] Tsist_graph;

        //при загрузке формы
        private void Results_Load(object sender, EventArgs e)
        {
            Dictionary<int, object> performanceIndicators = refForm.calculate();
            /////////
            Parameters.performance_indicators per_ind = new Parameters.performance_indicators();
            int point2 = 40;
            var first = performanceIndicators.First();
            var last = performanceIndicators.Last();
            int time1 = first.Key;
            int time2 = last.Key-1;
            int j = time1-1;
            int array_lenght=performanceIndicators.Count;

            time_graph = new int[25];
            limbda_graph = new float[25];
            Mu_graph = new float[25];
            Ro_graph = new float[25];
            Potk_graph = new float[25];
            Poch_graph = new float[25];
            Q_graph = new float[25];
            A_graph = new float[25];
            kzan_graph = new float[25];
            Loch_graph = new float[25];
            Toch_graph = new float[25];
            Lsist_graph = new float[25];
            Tsist_graph = new float[25];

            //обнулить массивы
            for (int i = 0; i < 25;i++ )
            {
                time_graph[i] = i;
                limbda_graph[i] = 0;
                Mu_graph[i] = 0;
                Ro_graph[i] = 0;
                Potk_graph[i] = 0;
                Poch_graph[i] = 0;
                Q_graph[i] = 0;
                A_graph[i] = 0;
                kzan_graph[i] = 0;
                Loch_graph[i] = 0;
                Toch_graph[i] = 0;
                Lsist_graph[i] = 0;
                Tsist_graph[i] = 0;
            }
            
            foreach (KeyValuePair<int, object> perInd in performanceIndicators) 
            {
                per_ind = (Parameters.performance_indicators)Convert.ChangeType(perInd.Value, typeof(Parameters.performance_indicators));

                GroupBox groupbox = new GroupBox();
                groupbox.Size = new Size(638, 34);
                groupbox.Location = new Point(17, 19 + point2);

                Label time = new Label();
                time.Size = new Size(26, 13);
                time.Location = new Point(6, 16);
                time.Text = Convert.ToString(perInd.Key);
                // time_graph[j] = perInd.Key;
                groupbox.Controls.Add(time); 

                ////////вывод оптимвльный показателей
                if (perInd.Key == time2+1)
                {
                    groupbox.Text = "Оптимальный показатели";
                    time.Text = "-";
                }

                   Label limbda = new Label();
                   limbda.Size = new Size(37, 13);
                   limbda.Location = new Point(78, 16);
                   limbda.Text=Convert.ToString(per_ind.limbda);
                   limbda_graph[j] = per_ind.limbda;
                groupbox.Controls.Add(limbda);

                   Label Mu = new Label();
                   Mu.Size = new Size(22, 13);
                   Mu.Location = new Point(132, 16);
                   Mu.Text = Convert.ToString(per_ind.Mu);
                   Mu_graph[j] = per_ind.Mu;
                groupbox.Controls.Add(Mu);

                   Label Ro = new Label();
                   Ro.Size = new Size(31, 13);
                   Ro.Location = new Point(172, 16);
                   Ro.Text = Convert.ToString(per_ind.Ro);
                   Ro_graph[j] = per_ind.Ro;
                groupbox.Controls.Add(Ro);

                   Label Potk = new Label();
                   Potk.Size = new Size(29, 13);
                   Potk.Location = new Point(215, 16);
                   Potk.Text = Convert.ToString(per_ind.Potk);
                   Potk_graph[j] = per_ind.Potk;
                groupbox.Controls.Add(Potk);

                   Label Poch = new Label();
                   Poch.Size = new Size(32, 13);
                   Poch.Location = new Point(271, 16);
                   Poch.Text = Convert.ToString(per_ind.Poch);
                   Poch_graph[j] = per_ind.Poch;
                groupbox.Controls.Add(Poch);

                   Label Q = new Label();
                   Q.Size = new Size(30, 13);
                   Q.Location = new Point(327, 16);
                   Q.Text = Convert.ToString(per_ind.Q);
                   Q_graph[j] = per_ind.Q;
                 groupbox.Controls.Add(Q);

                   Label A = new Label();
                   A.Size = new Size(30, 13);
                   A.Location = new Point(371, 18);
                   A.Text = Convert.ToString(per_ind.A);
                   A_graph[j] = per_ind.A;
                groupbox.Controls.Add(A);

                   Label kzan = new Label();
                   kzan.Size = new Size(30, 13);
                   kzan.Location = new Point(405, 16);
                   kzan.Text = Convert.ToString(per_ind.kzan);
                   kzan_graph[j] = per_ind.kzan;
                 groupbox.Controls.Add(kzan);

                   Label Loch = new Label();
                   Loch.Size = new Size(31, 13);
                   Loch.Location = new Point(456, 16);
                   Loch.Text = Convert.ToString(per_ind.Loch);
                   Loch_graph[j] = per_ind.Loch;
                groupbox.Controls.Add(Loch);

                   Label Toch = new Label();
                   Toch.Size = new Size(32, 13);
                   Toch.Location = new Point(508, 16);
                   Toch.Text = Convert.ToString(per_ind.Toch);
                   Toch_graph[j] = per_ind.Toch;
                groupbox.Controls.Add(Toch);

                   Label Lsist = new Label();
                   Lsist.Size = new Size(28, 13);
                   Lsist.Location = new Point(557, 16);
                   Lsist.Text = Convert.ToString(per_ind.Lsist);
                   Lsist_graph[j] = per_ind.Lsist;
                 groupbox.Controls.Add(Lsist);

                   Label Tsist = new Label();
                   Tsist.Size = new Size(29, 13);
                   Tsist.Location = new Point(597, 16);
                   Tsist.Text = Convert.ToString(per_ind.Tsist);
                   Tsist_graph[j] = per_ind.Tsist;
                  groupbox.Controls.Add(Tsist);

                groupBox1.Controls.Add(groupbox);
                point2 += 40;
                j++;
            
            }
            int idrhg = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //отображение графика
        private void button1_Click(object sender, EventArgs e)
        {
           if(limbda_radio.Checked){
                Graph g = new Graph(time_graph, limbda_graph, "limbda");
                g.ShowDialog();
            }
            if(Mu_radio.Checked){
                Graph mu_graph = new Graph(time_graph, Mu_graph, "Mu");
                        mu_graph.ShowDialog();
            }
            if(Ro_radio.Checked){
               Graph ro_graph = new Graph(time_graph, Ro_graph, "Ro");
                        ro_graph.ShowDialog();
            }
              if(Potk_radio.Checked){
               Graph potk_graph = new Graph(time_graph, Potk_graph, "Potk");
                        potk_graph.ShowDialog();
            }
             if(Poch_radio.Checked){
               Graph poch_graph = new Graph(time_graph, Poch_graph, "Poch");
                        poch_graph.ShowDialog();
            }
            if(Q_radio.Checked){
               Graph q_graph = new Graph(time_graph, Q_graph, "Q");
                        q_graph.ShowDialog();
            }
            if(A_radio.Checked){
              Graph a_graph = new Graph(time_graph, A_graph, "A");
                        a_graph.ShowDialog();
            }
            if(kzan_radio.Checked){
              Graph kz_graph = new Graph(time_graph, kzan_graph, "kzan");
                        kz_graph.ShowDialog();
            }
            if(Loch_radio.Checked){
              Graph loch_graph = new Graph(time_graph, Loch_graph, "Loch");
                        loch_graph.ShowDialog();
            }
            if(Toch_radio.Checked){
              Graph toch_graph = new Graph(time_graph, Toch_graph, "Toch");
                        toch_graph.ShowDialog();
            }
            if(Lsist_radio.Checked){
              Graph lsist_graph = new Graph(time_graph, Lsist_graph, "Lsist");
                        lsist_graph.ShowDialog();
            }
            if(Tsist_radio.Checked){
             Graph tsist_graph = new Graph(time_graph, Tsist_graph, "Tsist");
                        tsist_graph.ShowDialog();
            }
        }
    }
}
