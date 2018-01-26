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

using NormaMeasure.DBClasses;
using NormaMeasure.BaseClasses;
using NormaMeasure.Teraohmmeter;
using NormaMeasure.Teraohmmeter.Forms;
using NormaMeasure.Teraohmmeter.DBClasses;

namespace NormaMeasure
{
    public partial class MainForm : Form
    {
        public bool isTestApp = Properties.Settings.Default.IsTestApp;
        public List<Device> ConnectedDevices = new List<Device>();
        public TeraEtalonMap[] TeraEtalonMaps;


        public MainForm()
        {
            initDataBases();
            InitializeComponent();
            if (this.isTestApp) { this.Text += " (Демонстрационный режим)"; }
            Thread.CurrentThread.CurrentCulture = new CultureInfo("my");
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            searchConnectedDevices(); //Ищем подключенные устройства
            fillDeviceList();
            fillMenuStripItems();
        }

        /// <summary>
        /// Заполняем динамичные пункты главного меню программы
        /// </summary>
        private void fillMenuStripItems()
        {
            fillTeraEtalonMapItems();
        }

        /// <summary>
        /// Инициализируем базы данных
        /// </summary>
        private void initDataBases()
        {
            DBMigration tdbm = new DBMigration();
        }

        /// <summary>
        /// Заполнение списка устройств
        /// </summary>
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

        /// <summary>
        /// Открытие окна управления устройством
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Поиск подключенных устройств
        /// </summary>
        private void searchConnectedDevices()
        {
            string[] ports = isTestApp ? DemoTera.FakePortNumbers() : System.IO.Ports.SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                foreach (string pName in ports)
                {
                    try
                    {
                        TeraDevice t;
                        t = new TeraDevice(pName);
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
        }

        /// <summary>
        /// Заполняем список карт эталонов тераомметра
        /// </summary>
        private void fillTeraEtalonMapItems()
        {
            this.TeraEtalonMaps = new TeraEtalonMap().GetAll();
            ToolStripItem tsi = teraEtalonMapsToolStripMenuItem.DropDownItems.Add("Добавить");
            tsi.Name = "tera_etalon_map_add";
            tsi.Click += TeraMapsToolStripMenuItem_DropDownItemClick;
            for(int i=0; i<TeraEtalonMaps.Length; i++)
            {
                TeraEtalonMap tep = this.TeraEtalonMaps[i];
                tsi = teraEtalonMapsToolStripMenuItem.DropDownItems.Add(tep.AlterName);
                tsi.Name = "tera_etalon_map_" + i.ToString();
                tsi.Click += TeraMapsToolStripMenuItem_DropDownItemClick;
            }
        }

        /// <summary>
        /// Открытие окна редактирования/создания карты эталонов для ТОмМ-01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeraMapsToolStripMenuItem_DropDownItemClick(object sender, EventArgs e)
        {
            TeraEtalonMapControl temc;
            ToolStripItem i = sender as ToolStripItem;
            string f = i.Name.Replace("tera_etalon_map_", "");
            if (f == "add")
            {
                temc = new TeraEtalonMapControl();
            }else
            {
                temc = new TeraEtalonMapControl(this.TeraEtalonMaps[Convert.ToInt16(f)]);
            }
            temc.FormClosing += TeraEtalonMapControl_Closing;
            temc.MdiParent = this;
            temc.Show();
            i.Enabled = false;
        }


        /// <summary>
        /// Событие выполняемое при закрытии окна управления картой эталонов для ТОмМ-01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeraEtalonMapControl_Closing(object sender, FormClosingEventArgs e)
        {
            TeraEtalonMapControl t = sender as TeraEtalonMapControl;
            teraEtalonMapsToolStripMenuItem.DropDownItems.Clear();
            fillTeraEtalonMapItems();
        }


    }
}
