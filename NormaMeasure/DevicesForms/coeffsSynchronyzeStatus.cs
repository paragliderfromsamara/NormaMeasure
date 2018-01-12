using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace NormaMeasure.DevicesForms
{
    public partial class coeffsSynchronyzeStatus : Form
    {
        public coeffsSynchronyzeStatus(bool isDevToPC, string serialNumber)
        {
            InitializeComponent();
            serialNumberLbl.Text = "Серийный номер " + serialNumber;
            loadParamsLbl.Text = (isDevToPC) ? "Загрузка параметров" : "Поиск параметров в БД";
            saveParamsLbl.Text = (isDevToPC) ? "Сохранение параметров в БД" : "Отправка параметров в прибор";
            this.Refresh();
           // Thread.Sleep(1000);
        }

        public void completeStatus(int s)
        {
            switch (s)
            {
                case 1:
                    setLinkLbl.ForeColor = System.Drawing.Color.LimeGreen;
                    break;
                case 2:
                    loadParamsLbl.ForeColor = System.Drawing.Color.LimeGreen;
                    break;
                case 3:
                    saveParamsLbl.ForeColor = System.Drawing.Color.LimeGreen;
                    break;
            }
            this.Refresh();
            Thread.Sleep(150);
        }
    }
}
