using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using NormaMeasure.DevicesClasses;
using NormaMeasure.DataBaseClasses;
namespace NormaMeasure
{
    public partial class MainForm : Form
    {
        public bool isTestApp = Properties.Settings.Default.IsTestApp;
        public List<Device> ConnectedDevices = new List<Device>();

        public MainForm()
        {
            initDataBases();
            InitializeComponent();
            if (this.isTestApp) { this.Text += " (Демонстрационный режим)"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo("my");
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            searchConnectedDevices(); //Ищем подключенные устройства
            fillDeviceList();
            
        }

        private void initDataBases()
        {
            TeraDBMigration tdbm = new TeraDBMigration();
        }

        private void fillDeviceList()
        {
            deviceListMenuItem.DropDownItems.Clear();
            if (ConnectedDevices.Count > 0)
            {
                int i = 0;
                foreach (Device d in ConnectedDevices)
                {
                    deviceListMenuItem.DropDownItems.Add(d.NameWithSerial());
                    deviceListMenuItem.DropDownItems[deviceListMenuItem.DropDownItems.Count - 1].Name = "device_" + i.ToString();
                    deviceListMenuItem.DropDownItems[deviceListMenuItem.DropDownItems.Count - 1].Click += new EventHandler(openDeviceWindow);
                    i++;
                }
            }
        }

        private void openDeviceWindow(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            try
            {
                Device d = ConnectedDevices[deviceListMenuItem.DropDownItems.IndexOf(item)];
                d.ConnectToDevice(this);
            }
            catch
            {

            }
            //int idx = deviceListMenuItem.DropDownItems.IndexOf(item);

            //MessageBox.Show((ConnectedDevices[deviceListMenuItem.DropDownItems.IndexOf(item)].NameWithSerial()));
            //string test_id = item.Name.Replace("toolStripItem_", "");
        }

        private void searchConnectedDevices()
        {
            string[] ports = isTestApp ? DemoModeEntities.DemoTera.FakePortNumbers() : System.IO.Ports.SerialPort.GetPortNames();
            string v = "";
            if (ports.Length > 0)
            {
                foreach (string pName in ports)
                {
                    try
                    {
                        Teraohmmeter t;
                        t = new Teraohmmeter(pName);
                        v += t.NameWithSerial() + "; ";
                        if (t.IsExists)
                        {
                            ConnectedDevices.Add(t);
                        }// MessageBox.Show(t.NameWithSerial() + " порт " + pName);
                        else t.Dispose();

                    }
                    catch
                    {
                        continue;
                    }
                }
            } else MessageBox.Show("Нет портов");
            MessageBox.Show(v);
        }

    }
}
