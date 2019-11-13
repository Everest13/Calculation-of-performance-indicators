using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace Calculation_of_performance_indicators
{
    public partial class Parameters : Form
    {
        MySqlConnection conn;
        Random rand = new Random();
        string kladr_id = ""; //id населенного пункта
        int station_id; //id выбранной станции
        int Nel=0; //количество электромобилей в населенном пункте
        int Nels = 0; //количество электромобилей относящихся к станции
        public Parameters()
        {
            InitializeComponent();
        }
        //загрузка формы
        private void Parameters_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> kladr_localy = kladr_localys();

            foreach (KeyValuePair<string, string> kvp in kladr_localy)
            {
                localyBox.Items.Add(kvp.Value);
            }
        }
        //выбор localy
        private void localyBox_SelectedValueChanged(object sender, EventArgs e)
        {
            stationBox.Items.Clear();
            labelCars.Text = "";
            Nel = 0;
            Dictionary<string, string> kladr_localy = kladr_localys();

            foreach (KeyValuePair<string, string> kvp in kladr_localy)
            {
                if (kvp.Value == localyBox.SelectedItem.ToString())
                {
                    kladr_id = kvp.Key;
                }
            }
            Dictionary<int, string> kladr_station = kladr_stations();
            foreach (KeyValuePair<int, string> kvp in kladr_station)
            {
                stationBox.Items.Add(kvp.Value);
            }
        }
        //выбор station
        private void stationBox_SelectedValueChanged(object sender, EventArgs e)
        {
            labelCars.Text = "";
            Dictionary<int, string> kladr_station = kladr_stations();
            foreach (KeyValuePair<int, string> kvp in kladr_station)
            {
                if (kvp.Value == stationBox.SelectedItem.ToString())
                {
                    station_id = kvp.Key;
                    station_adress(station_id);
                }
            }

            Dictionary<int, object> blank = blanks();
            Nels=blank.Count;
            labelCars.Text = Convert.ToString(Nels);

        }
        //активация кнопки рассчета
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            //проверка на ошибки, вывод формы ошибок
            //вызов метода определения входного потока
            Results newForm = new Results(this);
            newForm.Show();

           // calculate();
        }
      
        //массив kladr_id-localy
        private Dictionary<string, string> kladr_localys()
        {
            connect();
            string sql = "SELECT kladr_id, name FROM localys";
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader localys = command.ExecuteReader();

            Dictionary<string, string> kladr_localys = new Dictionary<string, string>();
            while (localys.Read())
            {
                kladr_localys.Add(localys[0].ToString(), localys[1].ToString());
            }
            localys.Close();
            return kladr_localys;
        }
        //массив kladr_id-stations
        private Dictionary<int, string> kladr_stations()
        {
            connect();
            string sql = "SELECT*FROM stations WHERE kladr_id=" + kladr_id;
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader stations = command.ExecuteReader();

            Dictionary<int, string> kladr_stations = new Dictionary<int, string>();
            while (stations.Read())
            {
                kladr_stations.Add(Convert.ToInt32(stations[0]), stations[1].ToString());
            }
            stations.Close();
            return kladr_stations;
        }
        //массив blanks
        private Dictionary<int, object> blanks()
        {
            connect();
            string sql = "SELECT d1.station_id, d2.blank_id, d2.distance1 as distance, bl.power_recerve, bl.battery_capacity, bl.av_daily_milege, bl.charg_time, bl.need_level FROM distances d1 LEFT JOIN ( SELECT blank_id, min(distance) as distance1 FROM distances WHERE kladr_id=" +
                kladr_id+" GROUP BY blank_id) d2 ON d1.blank_id=d2.blank_id AND d1.distance=d2.distance1 LEFT JOIN blanks bl ON bl.id=d2.blank_id WHERE d2.blank_id IS NOT NULL";

            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader distance = command.ExecuteReader();

            Dictionary<int, object> blanks=new Dictionary<int, object>();
           
            while (distance.Read())
            {
                Nel++;
                if(Convert.ToInt32(distance[0])==station_id) {
                    blank blank_object = new blank();
                    blank_object.blank_id = Convert.ToInt32(distance[1]);
                    blank_object.distance = (float)Convert.ChangeType(distance[2], typeof(float));
                    blank_object.power_recerve = (float)Convert.ChangeType(distance[3], typeof(float));
                    blank_object.battery_capacity = (float)Convert.ChangeType(distance[4], typeof(float));
                    blank_object.av_daily_milege = (float)Convert.ChangeType(distance[5], typeof(float));
                    blank_object.charg_time = Convert.ToInt32(distance[6]);
                    blank_object.need_level = (float)Convert.ChangeType(distance[7], typeof(float));

                    blanks.Add(Convert.ToInt32(distance[1]), blank_object);
                }
            }
            
            distance.Close();

            return blanks;
        }
        public void station_adress(int station_id)
        {
            connect();
            string sql = "SELECT*FROM stations WHERE id=" + station_id;
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader adress = command.ExecuteReader();

            while (adress.Read())
            {
                labelStreet.Text=Convert.ToString(adress[3]);
                labelBuilding.Text = Convert.ToString(adress[4]);
            }
            adress.Close();

        }
        //соедениться с базой
        public void connect()
        {
            string connetion = null;
            connetion = "server=localhost;database=el_Charg;uid=root;pwd=root;";
            conn = new MySqlConnection(connetion);
            try
            {
                conn.Open();
                Console.WriteLine("Connection Open ! ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! ");
            }
        }
        //разедениться с базой
        private void disconnect()
        {
            string connetion = null;
            connetion = "server=localhost;database=el_charg;uid=root;pwd=root;";
            conn = new MySqlConnection(connetion);
            conn.Close();
        }
    
        //метода определения входного потока
        public Dictionary<int, object> calculate()
        {
            Dictionary<int, object> blankk = blanks();
            blank blank_i;
            
            int time2 = Convert.ToInt32(textBoxTime2.Text);
            int time1 = Convert.ToInt32(textBoxTime1.Text);
            if (time2 == 0)
            {
                time2 = 24;
            }

            int[] Nelj = new int[time2]; //для количества лектромобилей в час

            //параметры
            int delta_time=time2-time1;
            float[] limbda = new float[time2]; //интенсивность входного потока
            int n = Convert.ToInt32(textBoxCharges.Text); //количество каналов обслуживания
            float Pch = (float)Convert.ToDouble(textBoxPower.Text); //мощность зарядного устройства
            float[] tch = new float[time2]; //время зарядки
            float[] Mu = new float[time2]; //интенсивность обслуживания
            float middleMu=0; //среднее Mu за сутки
            float[] Ro = new float[time2]; //количество обслуженных заявок

            ///////рассчет интенсивностей
            for (int j = time1; j < time2; j++)
            {
                limbda[j]=0;
                Nelj[j] = 0;
                foreach (KeyValuePair<int, object> kvp in blankk)
                {
                    blank_i = (blank)Convert.ChangeType(kvp.Value, typeof(blank));
                    
                    //float p=rand.Next(0,1); //верояность появления заявки
                    int p = 1;

                    //берем заявку назначенную на время j с вероятностью появления p=1 
                    //которая относится к исследуемой станции 
                    if ((blank_i.charg_time == j) && (p >= 0.5))
                    {
                        //интенсивность входного потока
                        limbda[j] += (blank_i.av_daily_milege + blank_i.distance * 2) / blank_i.power_recerve;
                        Nelj[j]++;
                        //время зарядки
                        tch[j] += blank_i.battery_capacity*80/100/Pch;
                    }
                }                
                //суммируем, получаем входной поток требований на tj
               // limbda[j] += (float)probability(Nelj[j], j);
                float l=limbda[j];
                //интенсивность обслуживания
                if (tch[j]>0)
                {
                    Mu[j] = 1 / (tch[j] / Nelj[j]) * n;
                }
                middleMu+=Mu[j];
            }
            //////////
            //расчитать Mu=0, задать среднее суточное значение
            for (int j = time1; j < time2; j++)
            {
                if (Mu[j] <= 0)
                {
                    Mu[j] = middleMu / time2;
                }
                Ro[j] = limbda[j] / Mu[j];
               //необслуженные заявки
                float M = limbda[j] - Ro[j];
                if (M > 0)
                {
                    if (j == 23)
                    {
                        continue;
                    }
                    float p1 = (float)rand.NextDouble() * (1 - 0) + 0; ; //заявки уходят необслуженными
                    float p2 = (float)rand.NextDouble() * ((1 - p1) - 0) + 0; //заявки приходят в другое время
                    float p3 = 1 - p1 - p2; //заявки остаются ждать
                    limbda[j + 1] += M * p3; //добавляем ждущие заявки на след час
                    limbda[rand.Next(j+1, time2)] += M * p2; //добавляем заявки кот приходят в другое время
                }
            
            }
            //mетод расчета вероятностей состояний
           return eventProbability(Ro, n, time1, time2, limbda, Mu);
        }
        //вычисление факториала
        public int fact(int n)
        {
            int fact_n = 1;
            //факториал
            for (int k = 1; k <= n; k++)
            {
                fact_n = fact_n * k;
            }
            return fact_n;
        }
        //расчет вероятностей состояний
        public Dictionary<int, object> eventProbability(float[] Ro, int n, int time1, int time2, float[] limbda, float[] Mu)
        {
            int m = Convert.ToInt32(textBoxQueue.Text); //количество мест в очереди

            //ПОКАЗАТЕЛИ
            float [] p_0=new float[time2];    //вероятность начальная
            float [] Potk=new float[time2];   //вероятность отказа
            float[] Poch = new float[time2 ];   //вероятность образования очереди
            float[] Q = new float[time2];   //относитнльная пропускная способность
            float[] A = new float[time2];   //абсолютная пропускная способность
            float[] kzan = new float[time2];   //число занятых каналов
            float[] Loch = new float[time2];   //ср число заявок в очереди
            float[] Toch = new float[time2];   //ср время ожтдания в очереди
            float[] Lsist = new float[time2];   //ср число заявок в системе
            float[] Tsist = new float[time2];   //ср время пребывани язаяыки в очереди

            Dictionary<int, object> performanceIndicators = new Dictionary<int, object>();

            //////////////расчет ВЕРОЯТНОСТЕЙ состояния системы на каждый час
            for (int j=time1;j<time2;j++) {

                p_0[j] = 1;
                //расчет p_0
                for (int i = 1; i <= n; i++)
                {
                    p_0[j] += (float)Math.Pow(Ro[j], i) / fact(i);
                }
                p_0[j] += (float)Math.Pow(Ro[j], n + 1)*(1-(float)Math.Pow(Ro[j]/n, m))/ n*fact(n)/(1-Ro[j]/n);
                p_0[j] = (float)Math.Pow(p_0[j], -1);

                float [] p_p=new float[n+m];
                //расчет вероятностей для n каналов
                for (int g = 1; g <= n;g++ )
                {
                    p_p[g] = (float)Math.Pow(Ro[j], g) * p_0[j] / fact(g);
                }
                //расчет вероятностей для n каналов и m мест в очереди
                for (int g = 1; g < m; g++)
                {
                    p_p[n+g] = (float)Math.Pow(Ro[j], n+g) * p_0[j] / fact(n) / (float)Math.Pow(n, g);
                }
                //ATENTION: расчет ПОКАЗАТЕЛЕЙ
                performance_indicators p_i = new performance_indicators();
                //вероятность отказа
                p_i.Potk = (float)Math.Pow(Ro[j], n + m)*p_0[j] / (float)Math.Pow(n, m) / fact(n);
                Potk[j] = (float)Math.Pow(Ro[j], n + m) * p_0[j] / (float)Math.Pow(n, m) / fact(n);
                //вероятность образования очереди
                p_i.Poch = (float)Math.Pow(Ro[j], n) * (1 - (float)Math.Pow(Ro[j] / n, m)) * p_0[j] / fact(n) / (1 - Ro[j] / n);
                Poch[j] = (float)Math.Pow(Ro[j], n) * (1 - (float)Math.Pow(Ro[j] / n, m)) * p_0[j] / fact(n) / (1 - Ro[j] / n);
                //относитнльная пропускная способность
                p_i.Q = 1 - Potk[j];
                Q[j] = 1 - Potk[j];
                //абсолютная пропускная способность
                p_i.A = limbda[j] * Q[j];
                A[j] = limbda[j] * Q[j];
                //число занятых каналов
                p_i.kzan = Ro[j] * Q[j];
                kzan[j] = Ro[j] * Q[j];
                //ср число заявок в очереди
                p_i.Loch=(float)Math.Pow(Ro[j], n+1)*(1-(float)Math.Pow(Ro[j] / n, m)*(m+1-m/n*Ro[j])) * p_0[j] /(float)Math.Pow((1-Ro[j]/n),2);
                Loch[j] = (float)Math.Pow(Ro[j], n + 1) * (1 - (float)Math.Pow(Ro[j] / n, m) * (m + 1 - m / n * Ro[j])) * p_0[j] / (float)Math.Pow((1 - Ro[j] / n), 2);
                //ср время ожтдания в очереди
                p_i.Toch = Loch[j] / limbda[j];
                Toch[j] = Loch[j] / limbda[j];
                if (limbda[j] == 0)
                {
                    p_i.Toch = 0;
                    Toch[j] = 0;
                }
                //ср число заявок в системе
                p_i.Lsist = Loch[j] + kzan[j];
                Lsist[j] = Loch[j] + kzan[j];
                //ср время пребывани язаяыки в очереди
                p_i.Tsist = Lsist[j] / limbda[j];
                Tsist[j] = Lsist[j] / limbda[j];
                if (limbda[j] == 0)
                {
                    p_i.Tsist = 0;
                    Tsist[j] = 0;
                }
                p_i.limbda = limbda[j];
                p_i.Mu = Mu[j];
                p_i.Ro = Ro[j];
                //складываем в словарь
                performanceIndicators.Add(j, p_i);
            }
            ///оптимальные показатели, расчет
            performance_indicators p_opt = new performance_indicators();

            p_opt.Mu = 0;
            p_opt.limbda = 0;
            float p_0_opt = 1;
            for (int i = time1; i < time2; i++)
            {
                p_opt.Mu+= Mu[i];
                p_opt.limbda += limbda[i];   //limbda middle среднее за сутки
            }
            p_opt.Mu = p_opt.Mu / (time2-time1);  //Mu middle среднее за сутки
            p_opt.limbda = p_opt.limbda / (time2 - time1);
            p_opt.Ro = p_opt.limbda  / p_opt.Mu;  //Ro middle среднее за сутки

            for (int i = 1; i <= n; i++)
            {
                p_0_opt += (float)Math.Pow(p_opt.Ro, i) / fact(i);
            }
            p_0_opt += (float)Math.Pow(p_opt.Ro, n + 1) * (1 - (float)Math.Pow(p_opt.Ro / n, m)) / n * fact(n) / (1 - p_opt.Ro / n);
            p_0_opt = (float)Math.Pow(p_0_opt, -1);
            //вероятность отказа
            p_opt.Potk = (float)Math.Pow(p_opt.Ro, n + m) * p_0_opt / (float)Math.Pow(n, m) / fact(n);
            //вероятность образования очереди
            p_opt.Poch = (float)Math.Pow(p_opt.Ro, n) * (1 - (float)Math.Pow(p_opt.Ro / n, m)) * p_0_opt / fact(n) / (1 - p_opt.Ro / n);
            //относитнльная пропускная способность
            p_opt.Q = 1 - p_opt.Potk;
            //абсолютная пропускная способность
            p_opt.A = p_opt.limbda* p_opt.Q;
            //число занятых каналов
            p_opt.kzan = p_opt.Ro* p_opt.Q;
            //ср число заявок в очереди
            p_opt.Loch = (float)Math.Pow(p_opt.Ro, n + 1) * (1 - (float)Math.Pow(p_opt.Ro / n, m) * (m + 1 - m / n * p_opt.Ro)) * p_0_opt / (float)Math.Pow((1 - p_opt.Ro/ n), 2);
            //ср время ожтдания в очереди
            p_opt.Toch= p_opt.Loch/ p_opt.limbda;
            //ср число заявок в системе
            p_opt.Lsist = p_opt.Loch+ p_opt.kzan;
            //ср время пребывани язаяыки в очереди
            p_opt.Tsist = p_opt.Lsist/ p_opt.limbda;
            ////////////////
            performanceIndicators.Add(time2, p_opt);

            return performanceIndicators;
        }
       
        //расчет неожидаемых, случайных заявок
        public double probability(int Nelj, int tj)
        {
            double v = 0.5;
            int time2 = Convert.ToInt32(textBoxTime1.Text);
            int time1 = Convert.ToInt32(textBoxTime1.Text);
            if (time2 == 0)
            {
                time2 = 24;
            }
            double[] probability = new double[time2-time1];
            //расчет вероятностей, в сумме = 1
            for (int j = time1; j < time2; j++)
            {
                probability[j] = rand.NextDouble()*(v-0)+0;
                v -= probability[j];
            }
            // + заявки с других станций
            double Nelos = Nel - Nels;
            Nelos = Nelos * probability[rand.Next(0,23)]; //произведение на случ вероятность
            // + заявки с других часов
            double Neloj = Nels - Nelj;
            Neloj = Neloj * probability[rand.Next(0, 23)];
            // + заявки не зарегистрированные в системе 
            double Nelun = Nel * rand.Next(0,1)*probability[rand.Next(0, 23)];

            return Nelos + Neloj + Nelun;
    }
      class blank
        {
            public int blank_id;
            public float distance;
            public float power_recerve;
            public float battery_capacity;
            public float av_daily_milege;
            public int charg_time;
            public float need_level;
        }
        class station
        {
            public int station_id;
            public string street;
            public string building;
        }
        public class performance_indicators
        {
            public float limbda; //интенсивность входного потока
            public float Mu;    //интенсивность обслуживания
            public float Ro;   //количество обслуженных заявок
            public float Potk;   //вероятность отказа
            public float Poch;   //вероятность образования очереди
            public float Q;   //относитнльная пропускная способность
            public float A;   //абсолютная пропускная способность 
            public float kzan;   //число занятых каналов
            public float Loch;   //ср число заявок в очереди
            public float Toch;   //ср время ожтдания в очереди
            public float Lsist;   //ср число заявок в системе
            public float Tsist;   //ср время пребывани язаяыки в очереди
        }
    }

}

   