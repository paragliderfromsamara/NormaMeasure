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
            fillFromEtalonMap();
            createOrSaveButton.Text = "Добавить";
            this.Text = "Новая карта эталонов для ТОмМ-01";
            this.deleteButton.Visible = false;
        }

        public TeraEtalonMapControl(TeraEtalonMap EtalonMap)
        {
            InitializeComponent();
            this.EtalonMap = EtalonMap;
            isEditMode = true;
            fillFromEtalonMap();
            createOrSaveButton.Text = "Сохранить";
            this.Text = "Изменение карты эталонов для ТОмМ-01";
            this.deleteButton.Visible = true;
        }

        private void fillFromEtalonMap()
        {
            textBoxName.Text = EtalonMap.Name;
            textBox1MOm.Text = EtalonMap.OneMOm.ToString();
            textBox10MOm.Text = EtalonMap.TenMOm.ToString();
            textBox100MOm.Text = EtalonMap.OneHundredMOm.ToString();
            textBox1GOm.Text = (EtalonMap.OneGOm / 1000f).ToString();
            textBox10GOm.Text = (EtalonMap.TenGOm / 1000f).ToString();
            textBox100GOm.Text = (EtalonMap.OneHundredGOm / 1000f).ToString();
            textBox1TOm.Text = (EtalonMap.OneTOm / 1000000f).ToString();
            textBox10TOm.Text = (EtalonMap.TenTOm / 1000000f).ToString();
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
            if (String.IsNullOrEmpty(tb.Text))
            {
                tb.Text = "1";
                return;
            }
            try
            {
                float val = float.Parse(tb.Text);
                if (tb.Name == textBox1MOm.Name)
                {
                    if (valid(val, textBox1MOm)) return;
                    this.EtalonMap.OneMOm = val;
                   // MessageBox.Show(val.ToString());
                }
                else if (tb.Name == textBox10MOm.Name)
                {
                    if (valid(val, textBox10MOm)) return;
                    this.EtalonMap.TenMOm = val;
                }
                else if (tb.Name == textBox100MOm.Name)
                {
                    if (valid(val, textBox100MOm)) return;
                    this.EtalonMap.OneHundredMOm = val;
                }
                else if (tb.Name == textBox1GOm.Name)
                {
                    if (valid(val, textBox1GOm)) return;
                    this.EtalonMap.OneGOm = val * 1000;
                }
                else if (tb.Name == textBox10GOm.Name)
                {
                    if (valid(val, textBox10GOm)) return;
                    this.EtalonMap.TenGOm = val * 1000;
                }
                else if (tb.Name == textBox100GOm.Name)
                {
                    if (valid(val, textBox100GOm)) return;
                    this.EtalonMap.OneHundredGOm = val * 1000;
                }
                else if (tb.Name == textBox1TOm.Name)
                {
                    if (valid(val, textBox1TOm)) return;
                    this.EtalonMap.OneTOm = val * 1000000;
                }
                else if (tb.Name == textBox10TOm.Name)
                {
                    if (valid(val, textBox10TOm)) return;
                    this.EtalonMap.TenTOm = val * 1000000;
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
