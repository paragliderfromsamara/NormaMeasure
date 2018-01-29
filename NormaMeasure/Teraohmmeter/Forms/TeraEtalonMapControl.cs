using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NormaMeasure.Teraohmmeter.DBClasses;

namespace NormaMeasure.Teraohmmeter.Forms
{
    public partial class TeraEtalonMapControl : Form
    {
        private TeraEtalonMap EtalonMap;
        private bool isEditMode = false;
        public TeraEtalonMapControl()
        {
            InitializeComponent();
            this.EtalonMap = new TeraEtalonMap();
            buildForms();
            fillFromEtalonMap();
            createOrSaveButton.Text = "Добавить";
            this.Text = "Новая карта эталонов для ТОмМ-01";
            this.deleteButton.Visible = false;
        }

        public TeraEtalonMapControl(TeraEtalonMap EtalonMap)
        {
            InitializeComponent();
            this.EtalonMap = EtalonMap;
            buildForms();
            isEditMode = true;
            fillFromEtalonMap();
            createOrSaveButton.Text = "Сохранить";
            this.Text = "Изменение карты эталонов для ТОмМ-01";
            this.deleteButton.Visible = true;
        }

        private void buildForms()
        {
            textBoxName.Text = EtalonMap.Name;
            float[][] rList = this.EtalonMap.ResistanceList;
            string[] voltList = new string[] { "10В", "100В", "500В", "1000В" };
            string[] nominals = new string[] { "1МОм", "10МОм", "100МОм", "1ГОм", "10ГОм", "100ГОм", "1ТОм", "10ТОм" };
            int xPos = textBoxName.Location.X;
            int yPos = textBoxName.Location.Y + 50;
            int tabIndex = 100;
            textBoxName.Width = xPos + 90 + (xPos + 50) * (voltList.Length - 1);
            textBoxName.TabIndex = tabIndex;
            for (int i = 0; i < voltList.Length; i++)
            {
                Label l = new Label();
                l.Parent = this;
                l.Location = new Point(xPos + 60 + (xPos + 50) * i, yPos);
                l.Width = 50;
                l.Text = voltList[i];
            }
            for (int res = 0; res < rList.Length; res++)
            {
                Label l = new Label();
                l.Parent = this;
                l.Location = new Point(xPos, yPos + 27 + (22 + 20) * (rList.Length - res - 1));
                l.Width = 60;
                l.Text = nominals[res];
                for (int volt = 0; volt < rList[res].Length; volt++)
                {
                    if (rList[res][volt] == -1) continue;
                    TextBox tb = new TextBox();
                    tb.Parent = this;
                    tb.Size = new Size(50, 22);
                    tb.TabIndex = tabIndex++;
                    tb.Location = new Point(xPos + l.Width + (xPos + tb.Size.Width) * volt, yPos + 25 + (tb.Size.Height + 20) * (rList.Length - res - 1));
                    tb.Name = String.Format("v{0}r{1}", volt, res);
                    tb.Text = (rList[res][volt] / this.EtalonMap.Dividers[res]).ToString();
                    tb.TextChanged += textBoxResistorValue_TextChanged;
                }
            }
            this.Height = yPos + 40 + (44) * rList.Length + buttonsMenu.Height;
            this.Width = 2 * xPos + 60 + (xPos + 50) * rList[0].Length;
            buttonsMenu.Location = new Point(xPos, 22 + this.Height - buttonsMenu.Height * 2);
            createOrSaveButton.TabIndex = tabIndex++;
            deleteButton.TabIndex = tabIndex++;
        }

        private void fillFromEtalonMap()
        {
            textBoxName.Text = EtalonMap.Name;
            float[][] rList = this.EtalonMap.ResistanceList;
            for(int res = 0; res<rList.Length; res++)
            {
                for(int volt =0; volt<rList[res].Length; volt++)
                {
                    Control[] ctrls = this.Controls.Find(String.Format("v{0}r{1}", volt, res), false);
                    if (ctrls.Length > 0)
                    {
                        TextBox tb = ctrls[0] as TextBox;
                        tb.Text = (rList[res][volt]/this.EtalonMap.Dividers[res]).ToString();
                    } 
                }
            }
        }

        private void createOrSaveButton_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                if (EtalonMap.Update())
                {
                    MessageBox.Show("Карта эталонов успешно обновлена", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Не удалось обновить карту эталонов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (EtalonMap.Create())
                {
                    MessageBox.Show("Карта эталонов успешно добавлена", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Close();
                    this.Dispose();
                }else
                {
                    MessageBox.Show("Не удалось добавить карту эталонов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxResistorValue_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            float[][] rList = this.EtalonMap.ResistanceList;
            if (String.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "1";
                return;
            }
            try
            {
                float val = float.Parse(tb.Text);
                bool isFounded = false;
                for (int res = 0; res < rList.Length; res++)
                {
                    for(int volt=0; volt<rList[res].Length; volt++)
                    {
                        string name = String.Format("v{0}r{1}", volt, res);
                        if (String.Equals(name, tb.Name))
                        {
                            if (valid(val, tb)) return;
                            this.EtalonMap.ResistanceList[res][volt] = val*this.EtalonMap.Dividers[res];
                            isFounded = true;
                            //MessageBox.Show(this.EtalonMap.ResistanceList[res][volt].ToString());
                            break;
                        }
                    }
                    if (isFounded) break;
                }
            }
            catch(FormatException ex)
            {
                MessageBox.Show(String.Format("Строка должна иметь числовой формат с разделителем дробной части \"{0}\"", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                fillFromEtalonMap();
            }
   
        }

        private bool valid(float val, TextBox tb)
        {
            if (val < 0)
            {
                MessageBox.Show("Введённое значение не должно быть отрицательным!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.Text = 1.ToString();
            }
            else if (val > 1000)
            {
                MessageBox.Show("Введённое значение слишком велико!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb.Text = 1000.ToString();
            }
            return val > 1000 || val <= 0;

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            this.EtalonMap.Name = tb.Text;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Вы уверены, что хотите удалить карту эталонов?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.No) return;
            if (this.EtalonMap.Delete())
            {
                MessageBox.Show("Карта эталонов успешно удалена", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Не удалось удалить карту эталонов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


}
