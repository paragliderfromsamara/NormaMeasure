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
        float[] voltageCalculatedCoeffs = new float[] { 1, 1, 1 };
        string[] ranges = new string[] { "Диапазон 1", "Диапазон 2", "Диапазон 3", "Диапазон 4" };
        private float[,] coeffs;

        public TeraCoeffsForm(TeraResultsForm resultForm, TeraDevice device)
        {
            InitializeComponent();
            this.resultForm = resultForm;
            this.teraDevice = device;
            this.Text = String.Format("Коэффициенты коррекции тераомметра {0}", this.teraDevice.SerialNumber);
            coeffs = getCoeffs();
            FillCoeffs();
            //drawFields();
        }

        public void UpdCoeff(int volt, int range, float value)
        {
            coeffs[volt, range] = value;
            updLabel(volt, range, value);
        }

        private void updLabel(int volt, int range, float value)
        {
            Control[] ctrls = this.Controls.Find(String.Format("coeff_{0}_{1}", volt, range), false);
            if (ctrls.Length > 0)
            {
                Label lb = ctrls[0] as Label;
                lb.Text = coeffs[volt, range] > 0 ? coeffs[volt, range].ToString() : "x";
            }
            if (volt > 0) fillAverageMeasuredVoltageCoeff(volt);
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
                    updLabel(volt, r, coeffs[volt, r]);
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
            for (int i = 0; i < teraDevice.voltageCoeffs.Length; i++) setCurVoltCoeffInTextBox(i+1, teraDevice.voltageCoeffs[i]);
            return v;
        }

        private void fillAverageMeasuredVoltageCoeff(int volt)
        {
            float c = getMeasuredVoltageCoeff(volt);
            Control[] ctrls = this.Controls.Find(String.Format("measured_{0}", volt), false);
            if (ctrls.Length>0)
            {
                Label lb = ctrls[0] as Label;
                lb.Text = c.ToString();
            }
        }

        private void setCurVoltCoeffInTextBox(int volt, float value)
        {
            Control[] ctrls = this.Controls.Find(String.Format("cur_{0}", volt), false);
            if (ctrls.Length > 0)
            {
                TextBox tb = ctrls[0] as TextBox;
                tb.Text = value.ToString();
            }
        }

        private float getMeasuredVoltageCoeff(int volt)
        {
            float c = 0;
            int counter = 0;
            for(int i = 0; i<ranges.Length; i++)
            {
                if (coeffs[volt, i] > 0)
                {
                    counter++;
                    c += coeffs[volt, i];
                } 
            }
            if (counter > 0) c = (float)Math.Round(c/counter, 7);
            else c = 1;
            return c;
        }

        private void TeraCoeffsForm_VisibleChanged(object sender, EventArgs e)
        {
            FillCoeffs();
        }

        private void textBox1000V_TextChanged(object sender, EventArgs e)
        {
            updVoltageCoeff(3);
        }

        private void updVoltageCoeff(int volt)
        {
            voltageCalculatedCoeffs[volt-1] = getVoltageCalcCoeff(volt);
            Control[] ctrls = this.Controls.Find(String.Format("calculated_{0}", volt), false);
            if (ctrls.Length > 0)
            {
                Label lb = ctrls[0] as Label;
                lb.Text = voltageCalculatedCoeffs[volt-1].ToString();
            }
        }

        private float getVoltageCalcCoeff(int volt)
        {
            float tenVoltVal = getVoltageFromTextBox(0);
            float CurrentVolt = getVoltageFromTextBox(volt);
            float v = CurrentVolt/tenVoltVal;
            switch (volt)
            {
                case 1:
                    return (float)Math.Round(v / 10, 7);
                case 2:
                    return (float)Math.Round(v / 50, 7);
                case 3:
                    return (float)Math.Round(v / 100, 7);
                default:
                    return 1;
            }
        }


        private float getVoltageFromTextBox(int volt)
        {
            float v = voltage[volt];
            float d;
            Control[] ctrls = this.Controls.Find(String.Format("textBox_v{0}", volt), false);
            if (ctrls.Length > 0)
            {
                TextBox tb = ctrls[0] as TextBox;
                try
                {
                    v = float.Parse(tb.Text);
                    d = 1/v;
                }
                catch(Exception)
                {
                   MessageBox.Show(String.Format("Значение напряжения {0}В должно быть записано в десятичном числовом формате с разделителем \".\" и быть больше нуля", voltage[volt]));
                   tb.Text = voltage[volt].ToString();
                }
            }
            return v;
        }

        private void textBox_v2_TextChanged(object sender, EventArgs e)
        {
            updVoltageCoeff(2);
        }

        private void textBox_v1_TextChanged(object sender, EventArgs e)
        {
            updVoltageCoeff(1);
        }

        private void textBox_v0_TextChanged(object sender, EventArgs e)
        {
            updVoltageCoeff(1);
            updVoltageCoeff(2);
            updVoltageCoeff(3);
        }

        private float getCoeffFromCurForm(int volt)
        {
            float v = 1;
            try
            {
                Control[] ctrls = this.Controls.Find(String.Format("cur_{0}", volt), false);
                if (ctrls.Length > 0)
                {
                    TextBox tb = ctrls[0] as TextBox;
                    v = float.Parse(tb.Text);
                }
            }catch
            {
                v = 1;
            }
            return v;
        }

        private void saveBut_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Вы уверены, что хотите сохранить коэффициенты в приборе?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                for(int range = 0; range < ranges.Length; range++)
                {
                    this.teraDevice.rangeCoeffs[range] = coeffs[0, range];
                }
                for(int volt = 1; volt<voltage.Length; volt++)
                {
                    teraDevice.voltageCoeffs[volt-1] = getCoeffFromCurForm(volt);
                }
                this.teraDevice.syncCoeffs(false);
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Будут выбраны коэффициенты напряжения вычисленные по сопротивлению эталонов. Вы согласны?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                for(int volt = 1; volt < voltage.Length; volt++)
                {
                    float v = getMeasuredVoltageCoeff(volt);
                    setCurVoltCoeffInTextBox(volt, v);
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Будут выбраны коэффициенты напряжения вычисленные по реальным значениям выходных напряжений трансформатора. Вы согласны?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                for (int volt = 1; volt < voltage.Length; volt++)
                {
                    float v = getVoltageCalcCoeff(volt);
                    setCurVoltCoeffInTextBox(volt, v);
                }
            }
        }
    }
}
