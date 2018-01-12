using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NormaMeasure.Utils;


namespace NormaMeasure.DevicesClasses
{
    public abstract class Device
    {
        public bool IsExists = false;
        public bool IsConnected = false;
        public string SerialNumber;
        public int Number;
        public int ProductionYear;
        public string DeviceName;
        protected System.ComponentModel.IContainer components = null;
        public System.IO.Ports.SerialPort DevicePort;
        protected bool isTestApp;

        protected byte[] serialCmd;
        protected byte[] connectCmd;
        protected byte[] disconnectCmd;
        protected string typeName;

        protected abstract void setVariables();
        protected abstract void configurePort();
        public abstract bool ConnectToDevice(MainForm form);

        public void Dispose()
        {
            if (components != null) components.Dispose();
        }

        protected void baseInit()
        {
            this.isTestApp = Properties.Settings.Default.IsTestApp;
            this.components = new System.ComponentModel.Container();
            this.DevicePort = new System.IO.Ports.SerialPort(components);
            configurePort();
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

        protected void RenamePort(string name)
        {
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
            arr = receiveByteArray(2, true);
            if (arr[0] <= 255 && arr[0] >= 0) //Проверка валидности номера прибора
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
            arr = receiveByteArray(2, true);
            if (arr[0] <= 80 && arr[0] >= 10) //Проверка валидности номера прибора
            {
                this.IsExists = this.IsConnected = makeSerial(arr[0], arr[1]) == this.SerialNumber;
                Thread.Sleep(200);
            }
            return IsConnected;
        }

        /// <summary>
        /// Отключение от устройства
        /// </summary>
        protected void disconnect()
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

        protected float receiveDouble()
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


    }
}
