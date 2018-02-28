using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

using MySql.Data.MySqlClient;
using NormaMeasure.Utils;
using NormaMeasure.BaseClasses;
using NormaMeasure.DBClasses;


namespace NormaMeasure.Teraohmmeter
{
    public class TeraDevice : Device
    {
        public TeraForm DeviceForm;

        public uint checkSumFromDevice = 0; //проверочная сумма для коэффициентов коррекции из прибора
        public uint checkSumFromDB = 0; //Проверочная сумма из БД
        public float[] rangeCoeffs = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }; //коэффициенты коррекции по диапазону
        public float[] voltageCoeffs = new float[] { 1.0f, 1.0f, 1.0f };     //коэффициенты коррекции по напряжению
        /// <summary>
        /// Массив интегрирующих емкостей. Адрес в массиве == номеру диапазона
        /// </summary>
        public double[] capacitiesList = new double[] { 91.1, 11.1, 1.1, 0.1 }; //массив интегрирующих емкостей
        public double zeroResistance = 0.0003; //нулевое сопротивление, зависит от номинала защитного резистора
        public double refVoltage = 4.8; //опорное напряжение


        byte[] getCheckSumCmd;//Запрос серийного номера контрольной суммы
        byte[] getCoeffs;//Запрос коэффициентов
        byte[] startIntegratorCmd; //запуск интегрирования
        byte[] retentionVoltageCmd;//удержание напряжения при установленном времени поляризации, иначе при установленном времени поляризации через 4 секунды произойдёт отключение источника напряжения
        byte[] setVoltageCmd; //запуск источника напряжения, в зависимости от выставляемого напряжения второй байт будет принимать значения для 10В - 0 
        byte[] turnOffVoltage; //Выключает напряжение
        byte[] sendCoeffsCmd;

        public TeraDevice() : base()
        {
            
        }
        public TeraDevice(string port_name) : base(port_name)
        {
           
        }

        protected override void InitDevice()
        {
            base.InitDevice();
            this.DevicePort.BaudRate = 9600;
            this.DevicePort.Parity = System.IO.Ports.Parity.None;
            this.DevicePort.ParityReplace = 63;
            this.DevicePort.DataBits = 8;
            this.DevicePort.ReadTimeout = 200;
            this.DevicePort.WriteTimeout = 200;
            this.DevicePort.WriteBufferSize = 2048;
            this.DevicePort.ReadBufferSize = 4096;
            this.DevicePort.ReceivedBytesThreshold = 1;
            this.DevicePort.DiscardNull = false;
           
            
            //this.devicePort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DevicePort_DataReceived);
        }

        protected override void fillFromDataRow(DataRow dataRow)
        {
            this.rangeCoeffs = new float[] {
                                                ServiceFunctions.convertToFloat(dataRow["zero_range_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["first_range_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["second_range_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["third_range_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["third_range_additional_coeff"])
                                            };
            this.voltageCoeffs = new float[]
                                            {
                                                ServiceFunctions.convertToFloat(dataRow["one_hundred_volts_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["five_hundred_volts_coeff"]),
                                                ServiceFunctions.convertToFloat(dataRow["thousand_volts_coeff"])
                                            };
            this.checkSumFromDB = (uint)dataRow["coeffs_check_sum"];
        }
        protected override void setVariables()
        {
            byte serviceCmdHeader = 0x10;
            byte voltageControlCmdHeader = 0x20;
            byte measureCmdHeader = 0x30;

            this.DeviceType = DEVICE_TYPE.TERA;

            this.serialCmd = new byte[] { serviceCmdHeader, 0x00 }; //Запрос серийного номера
            this.connectCmd = new byte[] { serviceCmdHeader, 0x01}; //Установка соединения
            this.disconnectCmd = new byte[] { serviceCmdHeader, 0x02 }; //отключение устройства
            this.getCoeffs = new byte[] { serviceCmdHeader, 0x03 }; //Запрос коэффициентов
            this.getCheckSumCmd = new byte[] { serviceCmdHeader, 0x04 }; //Запрос серийного номера контрольной суммы
            this.sendCoeffsCmd = new byte[] { serviceCmdHeader, 0x05 }; //Запись коэффициентов в прибор



            this.startIntegratorCmd = new byte[] { measureCmdHeader, 0x22 }; //запуск интегрирования
            this.retentionVoltageCmd = new byte[] { measureCmdHeader, 0x00 }; //удержание напряжения при установленном времени поляризации, иначе при установленном времени поляризации через 4 секунды произойдёт отключение источника напряжения
            this.setVoltageCmd = new byte[] { voltageControlCmdHeader, 0xC0 }; //запуск источника напряжения, в зависимости от выставляемого напряжения второй байт будет принимать значения для 10В - 0, 100 - 1, 500 - 2, 1000 - 3 
            this.turnOffVoltage = new byte[] { voltageControlCmdHeader, 0x00 };

            
        }

        /// <summary>
        /// Соединие с тераомметром и открытие его окна
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public override bool ConnectToDevice(MainForm form)
        {
            if (DeviceForm == null)
            {
                if (connect())
                {
                    DeviceForm = new TeraForm(this, form);
                    DeviceForm.MdiParent = form;
                    DeviceForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(deviceFormClosedEvent);
                    //DeviceForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(deviceFormClosedEvent);
                    getCheckSumFromDevice(); //запрос проверочной суммы с прибора
                    //loadOrCreateFromDB();
                    if (!loadOrCreateFromDB() || (checkSumFromDevice != checkSumFromDB))
                    {
                        syncCoeffs(true);
                    }
                    DeviceForm.InitAndShow();
                }
            }else
            {
                DeviceForm.WindowState = System.Windows.Forms.FormWindowState.Normal; //Разворачиваем окно, если оно свёрнуто
                DeviceForm.Activate(); //Делаем активным
            }
            return IsConnected;

        }

        protected override void disconnect()
        {
            base.disconnect();
            this.DeviceForm.Dispose();
            this.DeviceForm = null;

        }

        private void deviceFormClosedEvent(object sender, System.Windows.Forms.FormClosedEventArgs arg)
        {
            if (IsConnected) this.disconnect();
            Thread.Sleep(600);
        }

        /// <summary>
        /// Синхронизирует коэффициенты с устройством. Если fromDevToPC == true то с устройства на ПК, если false то наоборот
        /// </summary>
        /// <param name="isDevToPC"></param>
        public void syncCoeffs(bool fromDevToPC)
        {
            coeffsSynchronyzeStatus stsForm = new coeffsSynchronyzeStatus(fromDevToPC, this.SerialNumber);
            stsForm.Show();
            if (fromDevToPC)
            {
                DBControl dc = new DBControl();
                string query = TeraSettings.Default.updateRangeAndVoltageCoeffs;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dc.MyConn;
                this.writePort(getCoeffs);
                stsForm.completeStatus(1);
                Thread.Sleep(200);
                this.receiveCoeffs();
                this.ClosePort();
                stsForm.completeStatus(2);
                cmd.CommandText = String.Format(query, this.SerialNumber, this.rangeCoeffs[0], this.rangeCoeffs[1], this.rangeCoeffs[2], this.rangeCoeffs[3], this.rangeCoeffs[4], this.voltageCoeffs[0], this.voltageCoeffs[1], this.voltageCoeffs[2], this.checkSumFromDevice);
                dc.MyConn.Open();
                cmd.ExecuteNonQuery();
                dc.MyConn.Close();
                stsForm.completeStatus(3);
                Thread.Sleep(1000);
                stsForm.Close();
            }
            else
            {
                List<byte> list = new List<byte>();
                byte[] arrToDev;
                byte[] tmp; 
                byte length = 0;
                foreach (byte b in sendCoeffsCmd) list.Add(b);
                foreach (float f in rangeCoeffs)
                {
                    byte[] fByte = BitConverter.GetBytes(f);
                    tmp = new byte[] { fByte[1], fByte[2], fByte[3], fByte[0] };
                    length++;
                    foreach (byte b in tmp) list.Add(b);
                }
                foreach (float f in voltageCoeffs)
                {
                    byte[] fByte = BitConverter.GetBytes(f);
                    tmp = new byte[] { fByte[1], fByte[2], fByte[3], fByte[0] };
                    length++;
                    foreach (byte b in tmp) list.Add(b);
                }
                arrToDev = list.ToArray();
                if (length <= 16)
                {
                    arrToDev[0] |= length;
                    this.writePort(arrToDev);
                    Thread.Sleep(600);
                    this.ClosePort();
                }
                string s = "";
                foreach (byte b in arrToDev) s += " " + b + " \n";
                System.Windows.Forms.MessageBox.Show(s);
                stsForm.Close();
                //float coeffs
            }
        }

        private void receiveCoeffs()
        {
            float[] range_coeffs = new float[this.rangeCoeffs.Length];
            float[] voltage_coeffs = new float[this.voltageCoeffs.Length];
            if (Properties.Settings.Default.IsTestApp)
            {
                this.rangeCoeffs = DemoTera.rangeCoeffs();
                this.voltageCoeffs = DemoTera.voltageCoeffs();
                Thread.Sleep(100);
            }else
            {
                //если приложение не в тестовом режиме
                for (int j = 0; j < this.rangeCoeffs.Length; j++)
                {
                    range_coeffs[j] = (float)receiveFloat();
                }
                for (int j = 0; j < this.voltageCoeffs.Length; j++)
                {
                    voltage_coeffs[j] = (float)receiveFloat();
                }
                this.rangeCoeffs = range_coeffs;
                this.voltageCoeffs = voltage_coeffs;
            }
        }

        private void sendCoeffs()
        {

        }

        /// <summary>
        /// Ищет устройство в базе, если есть заполняет переменные класса. Если нет, создаёт добавляет в базу устройство
        /// </summary>
        private bool loadOrCreateFromDB()
        {
            string query = String.Format(TeraSettings.Default.selectDeviceBySerial, this.SerialNumber);
            DataSet data_set = new DataSet();
            DBControl dc = new DBControl(DBSettings.Default.DBName);
            MySqlDataAdapter da = new MySqlDataAdapter(query, dc.MyConn);
            
            da.Fill(data_set);
            if (data_set.Tables[0].Rows.Count > 0)
            {
                this.deviceDataRow = data_set.Tables[0].Rows[0];
                return true;
            }
            else
            {
                query = String.Format(TeraSettings.Default.insertDevice, this.SerialNumber);
                MySqlCommand cmd = new MySqlCommand(query, dc.MyConn);
                dc.MyConn.Open();
                cmd.ExecuteNonQuery();
                dc.MyConn.Close();
                this.checkSumFromDB = this.checkSumFromDevice;
                this.isLoadedFromDB = true;
                return false;
            }
        }

        private void getCheckSumFromDevice()
        {
            byte[] arr;
            if (Properties.Settings.Default.IsTestApp)
            {
                int idx = Convert.ToInt16(DevicePortName);
                arr = new byte[] { DemoTera.FakeDevList[idx][2], DemoTera.FakeDevList[idx][3] };
            }
            else
            {
                writePort(getCheckSumCmd);
                arr = receiveByteArray(2, true);
            }
            this.checkSumFromDevice = (uint)arr[0] + (uint)arr[1] * 256;
        }

        /// <summary>
        /// Установка напряжения, если 0 то выключаем
        /// </summary>
        /// <param name="volt"></param>
        public void setVoltage(byte volt)
        {
            byte[] newCmd;
            byte b1 = setVoltageCmd[0];
            byte b2 = setVoltageCmd[1];
            b2 |= volt;
            newCmd = new byte[] { b1, b2 };
            this.writePort(newCmd);
        }

        /// <summary>
        /// Посылает флаг удержания напряжения, чтобы источник не выключился
        /// </summary>
        internal void RetentionVoltage()
        {
            this.writePort(retentionVoltageCmd);
        }

        /// <summary>
        /// Запуск интегрирования
        /// </summary>
        internal void StartIntegrator()
        {
            if (!Properties.Settings.Default.IsTestApp) this.DevicePort.DiscardInBuffer();
            this.writePort(startIntegratorCmd);

        }

        internal void StopMeasure()
        {
            this.setVoltage(0);
        }


        public void DoMeasure(ref MeasureResultTera result)
        {
            this.StartIntegrator();
            int maxMeasTime = 400;
            int time = 0;
            do
            {
                if (!Properties.Settings.Default.IsTestApp)
                {
                    if (this.DevicePort.BytesToRead == 8)
                    {
                        result.StatusId = 0x0F & this.DevicePort.ReadByte();
                        result.Range = this.DevicePort.ReadByte();
                        result.MeasureTime = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                        result.FirstMeasure = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                        result.LastMeasure = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                    }
                }
                else
                {
                    Random r = new Random();
                    result.StatusId = 0;
                    result.Range = 2;
                    result.MeasureTime = r.Next(130, 133);
                    Thread.Sleep(50);
                    result.FirstMeasure = r.Next(46, 48);
                    Thread.Sleep(50);
                    result.LastMeasure = r.Next(500, 510);
                    Thread.Sleep(50);
                }
                if (result.IsCompleted) break;
                Thread.Sleep(100);
                time++;
            } while (time < maxMeasTime);
        }

        
    }
}
