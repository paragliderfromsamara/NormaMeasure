using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NormaMeasure.BaseClasses;

namespace NormaMeasure.Teraohmmeter.Forms
{
    public partial class TeraResultsForm : Form
    {
        private List<MeasureResultCollection> ResultCollections = new List<MeasureResultCollection>();
        private TeraForm TeraForm = null;
        public TeraResultsForm(TeraForm tera_form)
        {
            InitializeComponent();
            this.Visible = false;
            this.TeraForm = tera_form;
            this.Text = String.Format("Результаты измерений: {0}", tera_form.teraDevice.NameWithSerial());

        }

        public void refreshResultsPage(MeasureResultCollection col)
        {
            if (!measureListCB.Items.Contains(col.Name))
            {
                measureListCB.Items.Add(col.Name);
                ResultCollections.Add(col);
            }
            
            if (measureListCB.SelectedIndex == -1 && measureListCB.Items.Count > 0) measureListCB.SelectedIndex = 0;
            if (measureListCB.Text == col.Name)
            {
                if (measureResultTable.Rows.Count != col.Count)
                {
                    if (col.Count < measureResultTable.Rows.Count)
                    {
                        measureResultTable.Rows.Clear();
                    }
                    int dif = col.Count - measureResultTable.Rows.Count;

                    for (int i = dif; i > 0; i--)
                    {
                        MeasureResult mr = col.ResultsList[col.Count - i];
                        addRowToDataGrid(mr);
                    }
                }
            }
        }

        private void addRowToDataGrid(MeasureResult r)
        {
            MeasureResultTera mr = r as MeasureResultTera;
            int num = measureResultTable.Rows.Add(1);
            measureResultTable.Rows[num].Cells["cycle_number"].Value = mr.CycleNumber;
            measureResultTable.Rows[num].Cells["stat_measure_number"].Value = mr.StatCycleNumber;
            measureResultTable.Rows[num].Cells["voltage"].Value = mr.Voltage;
            measureResultTable.Rows[num].Cells["result"].Value = mr.BringingResult;
            measureResultTable.Rows[num].Cells["first_measure"].Value = mr.FirstMeasure;
            measureResultTable.Rows[num].Cells["last_measure"].Value = mr.LastMeasure;
            measureResultTable.Rows[num].Cells["time"].Value = mr.MeasureTime;
            measureResultTable.Rows[num].Cells["range"].Value = mr.MeasureTime;
        }

        private void TeraResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void measureListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            // MessageBox.Show(String.Format("{0} {1}", cb.SelectedIndex, this.measure.ResultCollectionsList.Count));
            MeasureResultCollection col = ResultCollections[cb.SelectedIndex];
            measureResultTable.Rows.Clear();
            foreach (MeasureResult r in col.ResultsList)
            {
                 addRowToDataGrid(r);
            }
        }
    }
}
