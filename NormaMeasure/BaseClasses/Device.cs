using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data;
using NormaMeasure.Utils;
using NormaMeasure.Teraohmmeter;


namespace NormaMeasure.BaseClasses
{             
    public enum DEVICE_TYPE
    {
        TERA = 1,
        SAK = 2,
        MICRO = 3
    }                                                                                                                          
    public class Device
    {
        public string DevicePortName
        {
            get { return this.DevicePort.PortName; }
            set
            {
                try
                {
                    this.DevicePort.PortName = value;
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }
        /// <summary>
        /// Существует ли такое устройство
        /// </summary>
        public bool IsExists = false;
        /// <summary>
        /// Флаг подключения устройства
        /// </summary>
        public bool IsConnected = false;
        /// <summary>
        /// Серийный номер в формате 2014-01
        /// </summary>
        public string SerialNumber;
        /// <summary>
        /// Номер прибора в рамках года выпуска
        /// </summary>
        public int Number;
        /// <summary>
        /// Год выпуска
        /// </summary>
        public int ProductionYear;
        /// <summary>
        /// Название типа устройства
        /// </summary>
        public string DeviceName;

        public DEVICE_TYPE deviceType;
        /// <summary>
        /// COM Порт устройства через который происходит обмен данными
        /// </summary>
        public System.IO.Ports.SerialPort DevicePort;
        /// <summary>
        /// Код типа устройства
        /// </summary>
        public DEVICE_TYPE DeviceType
        {
            get
            {
                return deviceType;
            }
            set
            {
                deviceType = value;
                switch (value)
                {
                    case DEVICE_TYPE.TERA:
                        this.DeviceName = "Тераомметр";
                        break;
                    case DEVICE_TYPE.MICRO:
                        this.DeviceName = "Микроомметр";
                        break;
                    case DEVICE_TYPE.SAK:
                        this.DeviceName = "САК-ТВЧ";
                        break;
                }
                
            }
        }

        protected System.ComponentModel.IContainer components = null;
        
        protected bool isTestApp = Properties.Settings.Default.IsTestApp;
        protected bool isLoadedFromDB = false;
        protected DataRow deviceDataRow
        {
            set
            {
                this.isLoadedFromDB = true;
                fillFromDataRow(value);
            }
        }

        protected virtual void fillFromDataRow(DataRow dataRow) { }

        protected byte[] serialCmd;
        protected byte[] connectCmd;
        protected byte[] disconnectCmd;
        protected string typeName;

        protected virtual void setVariables() { }

        public Device()
        {
            InitDevice();
        }

        public Device(string port_name) : base()
        {
            InitDevice();
            this.DevicePortName = port_name;
            System.Windows.Forms.MessageBox.Show(DevicePortName);
            getSerial();
        }

        public virtual bool ConnectToDevice(MainForm form) { return true; }

        public void Dispose()
        {
            if (components != null) components.Dispose();
        }

        protected virtual void InitDevice()
        {
            this.components = new System.ComponentModel.Container();
            this.DevicePort = new System.IO.Ports.SerialPort(components);
            setVariables();
        }

        public void ClosePort()
        {
           // try
          //  {
                if (!this.isTestApp && !this.DevicePort.IsOpen) this.DevicePort.Open();
         //   }
        //    catch (Exception e)
       //     {
       //         MessageBox.Show(e.Message, "Ошибка связи", MessageBoxButtons.OK, MessageBoxIcon.Error);
       //     }
        }

        public void OpenPort()
        {
            //try
           // {
                if (!this.isTestApp && !this.DevicePort.IsOpen) this.DevicePort.Open();
           // }catch(Exception e)
           // {
           //     MessageBox.Show(e.Message, "Ошибка связи", MessageBoxButtons.OK, MessageBoxIcon.Error);
           // }
        }

        protected void writePort(byte[] ByteArray)
        {
           // try
           // {
                if (this.isTestApp) return;
                this.OpenPort();
                this.DevicePort.Write(ByteArray, 0, ByteArray.Length);
           // }
           // catch (Exception e)
           // {
           //     MessageBox.Show(e.Message, "Ошибка связи", MessageBoxButtons.OK, MessageBoxIcon.Error);
           // }

        }

        /// <summary>
        /// Считывает n количество принятых данных в массив равный n. Функцию открытия/закрытия порта не выполняет
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        protected byte[] receiveByteArray(int n, bool needToClose)
        {
            byte[] arr = new byte[n];
            this.OpenPort();
            for (int i = 0; i < n; i++)
            {
                arr[i] = (byte)DevicePort.ReadByte();
                Thread.Sleep(20);
            }
            if (needToClose) this.ClosePort();
            return arr;
        }

        protected void renamePort(string name)
        {
            if (isTestApp) return;
            this.ClosePort();
            this.DevicePort.PortName = name;
        }

        public string NameWithSerial()
        {
            return this.DeviceName + " №" + this.SerialNumber; 
        }
        protected void getSerial()
        {
            byte[] arr;
            this.ClosePort();
            writePort(this.serialCmd);
            arr = isTestApp ? DemoTera.FakeDevList[Convert.ToInt16(DevicePortName)] : receiveByteArray(2, true);
            if (isValidSerial(arr)) //Проверка валидности номера прибора
            {
                this.Number = arr[1];
                this.ProductionYear = 2000 + arr[0];
                this.SerialNumber = makeSerial();
                Thread.Sleep(200);
                this.IsExists = true;
            }
        }

        /// <summary>
        /// Подключение к устройству
        /// </summary>
        protected bool connect()
        {
            byte[] arr;
            this.ClosePort();
            writePort(this.connectCmd);
            arr = isTestApp ? getFakeDevice() : receiveByteArray(2, true);
            if (isValidSerial(arr)) //Проверка валидности номера прибора
            {
                this.IsExists = this.IsConnected = makeSerial(arr[0], arr[1]) == this.SerialNumber;
                Thread.Sleep(200);
            }
            return IsConnected;
        }

        /// <summary>
        /// Отключение от устройства
        /// </summary>
        protected virtual void disconnect()
        {
            this.ClosePort();
            writePort(this.disconnectCmd);
            this.IsConnected = false;
        }

        protected string makeSerial()
        {
            return String.Format("{0}-{1}{2}", this.ProductionYear, this.Number > 9 ? "" : "0",this.Number);
        }
        protected string makeSerial(byte iY, byte iN)
        {
            string y = "20" + ((iY < 10) ? "0" : "") + iY.ToString();
            string n = ((iN < 10) ? "0" : "") + iN.ToString();
            return y + "-" + n;
        }

        protected void sendFloat(float v)
        {
            byte[] arr = BitConverter.GetBytes(v);
            byte[] sArr = new byte[] { 0x11, 0x12, 0x13, 0x14};// { arr[1], arr[2], arr[3], arr[0] };
            this.writePort(sArr);
        }

        protected float receiveFloat()
        {
            float val = 0;
            Single v = 0;
            byte[] arr = new byte[4];
            byte[] tmp = new byte[4];
            try
            {
                tmp = this.receiveByteArray(4, false);
                arr = new byte[] { tmp[3], tmp[0], tmp[1], tmp[2] };
                v = BitConverter.ToSingle(arr, 0);
                val = ((float)v);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //testString += val.ToString()+ "; " + v.ToString() + String.Format("; {0}-{1}-{2}-{3}", arr[0], arr[1], arr[2], arr[3]) + ";\n";
            return val;
        }

        /// <summary>
        /// Название секции с параметрами устройства в ini файле
        /// </summary>
        /// <returns></returns>
        public string IniSectionName()
        {
            return String.Format("{0}_{1}_{2}", this.DeviceName, this.ProductionYear, this.Number);
        }

        private bool isValidSerial(byte[] arr)
        {
            if (arr.Length == 0) return false;
            return (arr[0] <= 255 && arr[0] >= 0);
        }


        private byte[] getFakeDevice()
        {
            switch(this.DeviceType)
            {
                case DEVICE_TYPE.TERA:
                    return DemoTera.FakeDevList[Convert.ToInt16(this.DevicePortName)];
                default:
                    return new byte[] { 0x00, 0x00 };
            }
           
        }
    }
}
