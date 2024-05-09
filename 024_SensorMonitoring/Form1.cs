using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _024_SensorMonitoring
{
  public partial class Form1 : Form
  {
        private SerialPort sPort = null;
        private int xCount = 100; // 차트 하나에 표시되는 데이터 개수

        // 시물레이션에서 사용
        private Timer t = new Timer();
        private Random r = new Random();
        private bool simulationFlag = false;
        public Form1()
        {
            InitializeComponent();

            foreach (var port in SerialPort.GetPortNames())
                comboBox1.Items.Add(port);
            comboBox1.Text = "Select Port";

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 1023;

            ChartSetting();
            Initsetting();
        }

        private void ChartSetting()
        {
            chart1.Titles.Add("조도");
            chart2.Titles.Add("온도/습도");

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = xCount;
            chart1.ChartAreas[0].AxisX.Interval = xCount / 4;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle

            chart1.ChartAreas[0].AxisY.Maximum = 0;
            chart1.ChartAreas[0].AxisY.Minimum = 800;
            chart1.ChartAreas[0].AxisY.Interval = 200;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle

            // Series  디자인
            chart1.Series.Clear();
            chart1.Series.Add("lumi");

            chart2.Series.Clear();
            chart2.Series.Add("temp");
            chart2.Series.Add("humi");

            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Color = Color.LightGreen;
            chart1.Series[0].BorderWidth = 2;

            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.Series[0].Color = Color.LightBlue;
            chart2.Series[0].BorderWidth = 2;

            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[1].Color = Color.Orange;
            chart2.Series[1].BorderWidth = 2;
        }

        private void Initsetting()
        {
            this.Text = "Arduino Sensor Monitoring System";
            btnPortValue.BackColor = Color.Blue;
            btnPortValue.ForeColor = Color.White;
            btnPortValue.Text = "온도\n습도\n조도";
            btnPortValue.Font =
                new Font("맑은 고딕", 12, FontStyle.Bold);

            label1.Text = "Connection Time : ";
            txtCount.TextAlign = HorizontalAlignment.Center;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = false;
        }

        private void 시작ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simulationFlag = true;
            t.Interval = 1000;
            t.Tick += T_Tick;
            t.Start();

        }

        private void T_Tick(object sender, EventArgs e)
        {
            int value = r.Next(800); // 조도
            int temp = r.Next(10, 35); // 온도
            int humi = r.Next(30, 80); // 습도

            string s = string.Format("{0}\t{1}\t{2}", temp, humi, value);
            ShowValue(s);
        }

        private void ShowValue(string s)
        {
            listBox1.Items.Add(s);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

            string[] sub = new string[3];
            sub = s.Split('\t');

            int temp = int.Parse(sub[0]);
            int humi = int.Parse(sub[1]);
            int value = int.Parse(sub[2]);

            progressBar1.Value = lumi;
            chart1.Series[0].Points.Add(lumi);

            chart2.Series[0].Points.Add(temp);
            chart2.Series[1].Points.Add(humi);


        }

        private void 끝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            t.Stop();
        }
    }
}
