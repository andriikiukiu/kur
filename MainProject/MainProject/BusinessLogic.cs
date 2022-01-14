using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MainProject
{
    struct Parameter
    {
        public string Name { get; private set; }
        public double Value;
        public bool IsOverloaded;
        public DateTime OverloadStartTime;
        public bool Alert;
        private bool IsLogged;
        public Parameter(string name)
        {
            Name = name;
            Value = 0;
            IsOverloaded = false;
            OverloadStartTime = DateTime.Now;
            Alert = false;
            IsLogged = false;
        }

        public void CheckOverload(string maxOrMin, double value)
        {
            switch(maxOrMin.ToLower())
            {
                case "max":
                    if(IsOverloaded)
                    {
                        if (Value > value)
                        {
                            SetAlertValues();
                            break;
                        }
                        IsOverloaded = false;
                        break;
                        
                    }
                    if(Value > value)
                    {
                        IsOverloaded = true;
                        OverloadStartTime = DateTime.Now;
                    }
                    break;
                case "min":
                    if (IsOverloaded)
                    {
                        if (Value < value)
                        {
                            SetAlertValues();
                            break;
                        }
                        IsOverloaded = false;
                        break;

                    }
                    if (Value < value)
                    {
                        IsOverloaded = true;
                        OverloadStartTime = DateTime.Now;
                    }
                    break;
            }
           
        }
        private void SetAlertValues()
        {
            if (DateTime.Now - OverloadStartTime > new TimeSpan(0, 0, 1, 0, 0))
            {
                Alert = true;
                if (!IsLogged)
                {
                    MyLogger.LogError($"{Name} is overloaded");
                }
                return;
            }
            Alert = false;
        }
    }
    class BusinessLogic
    {
        private double _minAvailableRAM = 2000;
        private double _maxCPUTemperature = 75;
        private double _maxMotherBoardTemperature = 50;
        private double _maxProcessLoading = 99.9;
        private double _maxVoltage = 20;


        private Parameter _availableRAM = new Parameter("Available RAM");
        private Parameter _CPUTemperature = new Parameter("CPU Temperature");
        private Parameter _motherBoardTemperature = new Parameter("MotherBoard Temperature");
        private Parameter _processLoading = new Parameter("Processor Loading");
        private Parameter _voltage = new Parameter("Voltage");

        public double AvailableRAM { get { return _availableRAM.Value; } }
        public double CPUTemperature { get { return _CPUTemperature.Value; } }
        public double MotherBoardTemperature { get { return _motherBoardTemperature.Value; } }
        public double ProcessLoading { get { return _processLoading.Value; } }
        public double Voltage { get { return _voltage.Value; } }

        public bool AvailableRAMAlert { get { return _availableRAM.Alert; } }
        public bool CPUTemperatureAlert { get { return _CPUTemperature.Alert; } }
        public bool MotherBoardTemperatureAlert { get { return _motherBoardTemperature.Alert; } }
        public bool ProcessLoadingAlert { get { return _processLoading.Alert; } }
        public bool VoltageAlert { get { return _voltage.Alert; } }


        public double SetMinAvailableRAM
        {
            set
            {
                MyLogger.LogInfo("Minimsal value of Available RAM has been changed!");
                _minAvailableRAM = value;
            }
        }
        public double SetMaxCPUTemperature
        {
            set
            {
                MyLogger.LogInfo("Max value of CPU temperature has been changed!");
                _maxCPUTemperature = value;
            }
        }
        public double SetMaxMotherBoardTemperature
        {
            set
            {
                MyLogger.LogInfo("Max value of Motherboard temperature has been changed!");
                _maxMotherBoardTemperature = value;
            }
        }
        public double SetMaxProcessLoading
        {
            set
            {
                MyLogger.LogInfo("Max value of Processor loading has been changed!");
                _maxProcessLoading = value;
            }
        }
        public double SetMaxVoltage
        {
            set
            {
                MyLogger.LogInfo("Max value of Voltage has been changed!");
                _maxVoltage = value;
            }
        }


        public double GetMinAvailableRAM
        {
            get
            {
                return _minAvailableRAM;
            }
        }
        public double GetMaxCPUTemperature
        {
            get
            {
                return _maxCPUTemperature;
            }
        }
        public double GetMaxMotherBoardTemperature
        {
            get
            {
                return _maxMotherBoardTemperature;
            }
        }
        public double GetMaxProcessLoading
        {
            get
            {
                return _maxProcessLoading;
            }
        }
        public double GetMaxVoltage
        {
            get
            {
                return _maxVoltage;
            }
        }


        SystemData sd = new SystemData();
        public void CheckSystem()
        {
            _availableRAM.Value = sd.GetAvailableRAM();
            _availableRAM.CheckOverload("min", _minAvailableRAM);

            var cpuTemp = sd.GetCPUTemperature();

            if(cpuTemp == -1)
            {
                _CPUTemperature.Value = 0;
               
            }
            else
            {
                _CPUTemperature.Value = cpuTemp;
            }
            _CPUTemperature.CheckOverload("max", _maxCPUTemperature);

            _motherBoardTemperature.Value = sd.GetMotherBoardTemperature();
            _motherBoardTemperature.CheckOverload("max", _maxMotherBoardTemperature);

            _processLoading.Value = sd.GetProcessLoading();
            _processLoading.CheckOverload("max", _maxProcessLoading);

            _voltage.Value = sd.GetVoltage();
            _voltage.CheckOverload("max", _maxVoltage);
        }
    }
}
