using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NormaMeasure.Teraohmmeter.Forms
{
    public partial class TeraCoeffsForm : Form
    {
        private TeraResultsForm resultForm;
        private TeraDevice teraDevice;

        int[] voltage = new int[] { 10, 100, 500, 1000 };
        string[] ranges = new string[] { "Диапазон 1", "Диапазон 2", "Диапазон 3", "Диапазон 4" };
        private float[,] coeffs;

        public TeraCoeffsForm(TeraResultsForm resultForm, TeraDevice device)
        {
            InitializeComponent();
            this.resultForm = resultForm;
            this.teraDevice = device;
            this.Text = String.Format("Коэффициенты коррекции тераомметра {0}", this.teraDevice.SerialNumber);
            coeffs = getCoeffs();
            drawFields();
        }

        public void UpdCoeff(int volt, int range, float value)
        {
            coeffs[volt, range] = value;
            Control[] ctrls = this.Controls.Find(String.Format("coeff_{0}_{1}", volt, range), false);
            if (ctrls.Length > 0)
            {
                Label lb = ctrls[0] as Label;
                lb.Text = coeffs[volt, range].ToString();
            }
        }

        private void drawFields()
        {
            int xPos, yPos;
            int startY = 20;
            int vertOffset = 10;
            Label l;
            xPos = 20;
            yPos = startY+15;
            for(int i=0; i< ranges.Length; i++)
            {
                int j = ranges.Length-1 - i;
                l = new Label();
                l.Parent = this;
                l.Location = new Point(xPos, yPos + vertOffset + (yPos+ vertOffset) *i);
                l.Width = 100;
                l.Text = ranges[j];
                l.Name = "range_name_" + j;
            }

            l = new Label();
            l.Parent = this;
            l.Location = new Point(xPos, yPos + vertOffset + (yPos + vertOffset) * ranges.Length);
            l.Width = 100;
            l.Text = "Коэфф. U посчитанный";
            l.Name = "average_volt";

            l = new Label();
            l.Parent = this;
            l.Location = new Point(xPos, yPos + vertOffset + (yPos + vertOffset) * (ranges.Length+1));
            l.Width = 100;
            l.Text = "Коэфф. U от напряжений";
            l.Name = "average_volt_by_voltages";


            for (int i=0; i< voltage.Length; i++)
            {
                yPos = startY;
                xPos += 110;
               // int j = ranges.Length - 1 - i;
                l = new Label();
                l.Parent = this;
                l.Location = new Point(xPos, yPos);
                l.Width = 70;
                l.Text = String.Format("{0}В", voltage[i]);
                yPos += 15;
                for(int j=ranges.Length-1; j >= 0; j--)
                {
                    Label lb = new Label();
                    lb.Parent = this;
                    lb.Location = new Point(xPos, yPos + vertOffset + (yPos + vertOffset) * j);
                    lb.Width = 70;
                    lb.Text = String.Format("coeff_{0}_{1}", i, ranges.Length - 1 - j);
                    lb.Name = String.Format("coeff_{0}_{1}", i, ranges.Length-1-j);
                }
         
            }
            FillCoeffs();
        }
        private void TeraCoeffsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void FillCoeffs()
        {
            for(int volt=0; volt < voltage.Length; volt++)
            {
                for(int r=ranges.Length-1; r >= 0; r--)
                {
                    Control[] ctrls = this.Controls.Find(String.Format("coeff_{0}_{1}", volt, r), false);
                    if (ctrls.Length>0)
                    {
                        Label lb = ctrls[0] as Label;
                        lb.Text = coeffs[volt, r].ToString();
                    }
                }
            }
        }

        private float[,] getCoeffs()
        {
            float[,] v = new float[voltage.Length, ranges.Length];
            for(int i=0; i<voltage.Length; i++)
            {
                if (voltage[i] == 10)
                {
                    for (int j = 0; j < ranges.Length; j++)
                    {
                        v[i, j] = this.teraDevice.rangeCoeffs[j];
                    }
                }else
                {
                    for (int j = 0; j < ranges.Length; j++)
                    {
                        v[i, j] = -1;
                    }
                }
            }
            return v;
        }

        private void TeraCoeffsForm_VisibleChanged(object sender, EventArgs e)
        {
            FillCoeffs();
        }
    }
}
