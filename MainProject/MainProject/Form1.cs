using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace MainProject
{
    
    public partial class Form1 : Form
    {
        private double _availableRAM;
        private double _CPUTemperature;
        private double _motherBoardTemperature;
        private double _processLoading;
        private double _voltage;


        BusinessLogic bl = new BusinessLogic();
        System.Timers.Timer timer = new System.Timers.Timer();
        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            
            SystemData systemdata = new SystemData();
            var x = systemdata.GetProcessLoading();

        }

        private void InitializeTimer()
        {
            timer.Interval = 10;
            timer.Elapsed += CheckSystem;
            timer.Start();
        }

        public void CheckSystem(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            bl.CheckSystem();
            _availableRAM = bl.AvailableRAM;
            _CPUTemperature = bl.CPUTemperature;
            _motherBoardTemperature = bl.MotherBoardTemperature;
            _processLoading = bl.ProcessLoading;
            _voltage = bl.Voltage;

            CheckAlerts();

            UpdateControls();



            timer.Start();
        }

        private void UpdateControls()
        {
            
            textBox1.Invoke( new Action(() => textBox1.Text = _processLoading.ToString()));
            progressBar1.Invoke(new Action(() => progressBar1.Value = (int)_processLoading));

            textBox2.Invoke(new Action(() => textBox2.Text = _CPUTemperature.ToString()));
            progressBar5.Invoke(new Action(() => progressBar5.Value = (int)_CPUTemperature));

            textBox3.Invoke(new Action(() => textBox3.Text = _motherBoardTemperature.ToString()));
            progressBar2.Invoke(new Action(() => progressBar2.Value = (int)_motherBoardTemperature));

            textBox4.Invoke(new Action(() => textBox4.Text = _voltage.ToString()));
            progressBar3.Invoke(new Action(() => progressBar3.Value = (int)_voltage));

            textBox5.Invoke(new Action(() => textBox5.Text = _availableRAM.ToString()));
            progressBar4.Invoke(new Action(() => progressBar4.Value = (int)_availableRAM));
        }

        private void InitializeProgressBars()
        {
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;

            progressBar2.Maximum = 50;
            progressBar2.Step = 1;

            progressBar3.Maximum = 50;
            progressBar3.Step = 1;

            progressBar4.Maximum = 4000;
            progressBar4.Step = 10;

            progressBar5.Maximum = 75;
            progressBar5.Step = 1;
        }

        private void ShowMaxValues()
        {
            textBox6.Text = bl.GetMaxProcessLoading.ToString();
            textBox7.Text = bl.GetMaxCPUTemperature.ToString();
            textBox8.Text = bl.GetMaxMotherBoardTemperature.ToString();
            textBox9.Text = bl.GetMaxVoltage.ToString();
            textBox10.Text = bl.GetMinAvailableRAM.ToString();
        }

        public void CheckAlerts()
        {
            if(bl.AvailableRAMAlert)
            {
                MessageBox.Show("Available RAM is too low!!!");
            }
            if (bl.CPUTemperatureAlert)
            {
                MessageBox.Show("Available Temperature is too high!!!");
            }
            if (bl.MotherBoardTemperatureAlert)
            {
                MessageBox.Show("Available Temperature is too high!!!");
            }
            if (bl.ProcessLoadingAlert)
            {
                MessageBox.Show("Processor loading is too high!!!");
            }
            if (bl.VoltageAlert)
            {
                MessageBox.Show("ERROR!!!");
            }
        }

        public void UpdateMinAvailableRAM(double value)
        {
            bl.SetMinAvailableRAM = value;
        }
        public void UpdateMaxMotherBoardTemperature(double value)
        {
            bl.SetMaxMotherBoardTemperature = value;
        }
        public void UpdateMaxCPUTemperature(double value)
        {
            bl.SetMaxCPUTemperature = value;
        }
        public void UpdateMaxProcessLoading(double value)
        {
            bl.SetMaxProcessLoading = value;
        }
        public void UpdateMaxVoltage(double value)
        {
            bl.SetMaxVoltage = value;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CheckSystem(null, null);
            InitializeProgressBars();
            ShowMaxValues();
            UpdateControls();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateMaxProcessLoading(Convert.ToDouble(textBox6.Text));
            UpdateMaxCPUTemperature(Convert.ToDouble(textBox7.Text));
            UpdateMaxMotherBoardTemperature(Convert.ToDouble(textBox8.Text));
            UpdateMaxVoltage(Convert.ToDouble(textBox9.Text));
            UpdateMinAvailableRAM(Convert.ToDouble(textBox10.Text));
        }
    }
}
