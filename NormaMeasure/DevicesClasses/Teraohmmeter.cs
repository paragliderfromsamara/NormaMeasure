﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using MySql.Data.MySqlClient;
using NormaMeasure.Utils;
using NormaMeasure.DevicesSettings;
using NormaMeasure.DevicesForms;
using NormaMeasure.MeasureClasses;


namespace NormaMeasure.DevicesClasses
{
    public class Teraohmmeter : Device
    {
        public NormaMeasure.DevicesForms.TeraForm DeviceForm;
        private DataRow TeraDataRow = null;
        public uint checkSumFromDevice = 0; //проверочная сумма для коэффициентов коррекции из прибора
        public uint checkSumFromDB = 0; //Проверочная сумма из БД
        public float[] rangeCoeffs = new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f }; //коэффициенты коррекции по диапазону
        public float[] voltageCoeffs = new float[] { 0.0f, 0.0f, 0.0f };     //коэффициенты коррекции по напряжению
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

        public Teraohmmeter()
        {
            baseInit();
        }
        public Teraohmmeter(string port_name)
        {
            baseInit();
            //RenamePort(port_name);
            DevicePort.PortName = port_name;
            getSerial();
        }
        protected override void setVariables()
        {
            byte serviceCmdHeader = 0x10;
            byte voltageControlCmdHeader = 0x20;
            byte measureCmdHeader = 0x30;

            this.DeviceName = "Тераомметр";

            this.serialCmd = new byte[] { serviceCmdHeader, 0x00 }; //Запрос серийного номера
            this.connectCmd = new byte[] { serviceCmdHeader, 0x01}; //Установка соединения
            this.disconnectCmd = new byte[] { serviceCmdHeader, 0x02 }; //отключение устройства
            this.getCoeffs = new byte[] { serviceCmdHeader, 0x03 }; //Запрос коэффициентов
            this.getCheckSumCmd = new byte[] { serviceCmdHeader, 0x04 }; //Запрос серийного номера контрольной суммы

            this.startIntegratorCmd = new byte[] { measureCmdHeader, 0x22 }; //запуск интегрирования
            this.retentionVoltageCmd = new byte[] { measureCmdHeader, 0x00 }; //удержание напряжения при установленном времени поляризации, иначе при установленном времени поляризации через 4 секунды произойдёт отключение источника напряжения
            this.setVoltageCmd = new byte[] { voltageControlCmdHeader, 0xC0 }; //запуск источника напряжения, в зависимости от выставляемого напряжения второй байт будет принимать значения для 10В - 0, 100 - 1, 500 - 2, 1000 - 3 
            this.turnOffVoltage = new byte[] { voltageControlCmdHeader, 0x00 }; 
        }

        protected override void configurePort()
        {
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

        /// <summary>
        /// Соединие с тераомметром и открытие его окна
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public override bool ConnectToDevice(MainForm form)
        {
                if (connect())
                {
                    DeviceForm = new DevicesForms.TeraForm(this);
                    DeviceForm.MdiParent = form;
                    DeviceForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(deviceFormClosedEvent);
                    getCheckSumFromDevice(); //запрос проверочной суммы с прибора
                    if (TeraDataRow == null)
                    {
                        if (!checkExistingInDB() || (checkSumFromDevice != checkSumFromDB))
                        {
                            syncCoeffs(true);
                        }
                    }
                    DeviceForm.Show();
                }
                
            return IsConnected;

        }

        private void deviceFormClosedEvent(object sender, System.Windows.Forms.FormClosingEventArgs arg)
        {
            if (IsConnected) this.disconnect();
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
                DBControl dc = new DBControl(TeraSettings.Default.dbName);
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

            }
        }

        private void receiveCoeffs()
        {
            float[] range_coeffs = new float[this.rangeCoeffs.Length];
            float[] voltage_coeffs = new float[this.voltageCoeffs.Length];
            if (Properties.Settings.Default.IsTestApp)
            {
                this.rangeCoeffs = DemoModeEntities.DemoTera.rangeCoeffs();
                this.voltageCoeffs = DemoModeEntities.DemoTera.voltageCoeffs();
                Thread.Sleep(100);
            }else
            {
                //если приложение не в тестовом режиме
                for (int j = 0; j < this.rangeCoeffs.Length; j++)
                {
                    range_coeffs[j] = (float)receiveDouble();
                }
                for (int j = 0; j < this.voltageCoeffs.Length; j++)
                {
                    voltage_coeffs[j] = (float)receiveDouble();
                }
                this.rangeCoeffs = range_coeffs;
                this.voltageCoeffs = voltage_coeffs;
            }

        }

        /// <summary>
        /// Ищет устройство в базе, если есть заполняет переменные класса. Если нет, создаёт добавляет в базу устройство
        /// </summary>
        private bool checkExistingInDB()
        {
            string query = String.Format(TeraSettings.Default.selectDeviceBySerial, this.SerialNumber);
            DataSet data_set = new DataSet();
            DBControl dc = new DBControl(TeraSettings.Default.dbName);
            MySqlDataAdapter da = new MySqlDataAdapter(query, dc.MyConn);

            da.Fill(data_set);

            if (data_set.Tables[0].Rows.Count > 0)
            {
                this.TeraDataRow = data_set.Tables[0].Rows[0];
                fillFromDB();
                //MessageBox.Show("Тераомметр найден в БД");
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
                return false;
            }
        }
        /// <summary>
        /// Заполняет свойства класса из БД
        /// </summary>
        private void fillFromDB()
        {
            this.rangeCoeffs = new float[] {
                                                ServiceFunctions.convertToFloat(TeraDataRow["zero_range_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["first_range_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["second_range_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["third_range_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["third_range_additional_coeff"])
                                            };
            this.voltageCoeffs = new float[]
                                            {
                                                ServiceFunctions.convertToFloat(TeraDataRow["one_hundred_volts_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["five_hundred_volts_coeff"]),
                                                ServiceFunctions.convertToFloat(TeraDataRow["thousand_volts_coeff"])
                                            };
            this.checkSumFromDB = (uint)TeraDataRow["coeffs_check_sum"];
        }

        private void getCheckSumFromDevice()
        {
            byte[] arr;
            writePort(getCheckSumCmd);
            arr = receiveByteArray(2, true);
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
            this.writePort(startIntegratorCmd);
        }

        internal void StopMeasure()
        {
            this.setVoltage(0);
        }

        public TeraMeasureResult CheckResult()
        {
            TeraMeasureResult result = new TeraMeasureResult();
            result.IsReceived = false;
            if (!Properties.Settings.Default.IsTestApp)
            {
                if (result.IsReceived = (this.DevicePort.BytesToRead == 8))
                {
                    result.Status = 0x0F & this.DevicePort.ReadByte();
                    result.Range = this.DevicePort.ReadByte();
                    result.MeasureTime = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                    result.FirstMeasure = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                    result.LastMeasure = this.DevicePort.ReadByte() + this.DevicePort.ReadByte() * 256;
                }
            }
            else
            {
                Random r = new Random();
                result.Status = 0;
                result.Range = 2;
                result.MeasureTime = r.Next(130, 133);
                result.FirstMeasure = r.Next(46, 48);
                result.LastMeasure = r.Next(500, 510);
                result.IsReceived = true;
            }
            return result;
        }
    }
}
