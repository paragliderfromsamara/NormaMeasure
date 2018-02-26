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
        private int curWidth, curHeight;
        public TeraResultsForm(TeraForm tera_form)
        {
            InitializeComponent();
            curWidth = this.Width;
            curHeight = this.Height;
            this.Visible = false;
            this.TeraForm = tera_form;
            this.Text = String.Format("Результаты измерений: {0}", tera_form.teraDevice.NameWithSerial());
            setCellsVisibility();

        }

        public void refreshResultsPage(MeasureResultCollection col)
        {
            if (!measureListCB.Items.Contains(col.Name))
            {
                measureListCB.Items.Add(col.Name);
                ResultCollections.Add(col.Clone());
            }else
            {
                for(int i = 0; i<ResultCollections.Count; i++)
                {
                    if (ResultCollections[i].Name == col.Name)
                    {
                        ResultCollections[i] = col.Clone();
                        break;
                    }
                }
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


            //Служебная информация
            measureResultTable.Rows[num].Cells["first_measure"].Value = mr.FirstMeasure;
            measureResultTable.Rows[num].Cells["last_measure"].Value = mr.LastMeasure;
            measureResultTable.Rows[num].Cells["time"].Value = mr.MeasureTime;
            measureResultTable.Rows[num].Cells["range"].Value = mr.Range;
            //Служебная информация
        }

        private void TeraResultsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void TeraResultsForm_ResizeEnd(object sender, EventArgs e)
        {
           // int diffHeight = this.Height - curHeight;
          //  int diffWidth = this.Width - curWidth;
           // measureResultTable.Width = measureResultTable.Width + diffWidth;
           // measureResultTable.Height = measureResultTable.Height + diffHeight;
        }

        private void setCellsVisibility()
        {
            first_measure.Visible = startPointCB.Checked;
            last_measure.Visible = endPointCB.Checked;
            range.Visible = rangeCB.Checked;
            difference.Visible = differenceCB.Checked;
            time.Visible = timesCB.Checked;
           
        }
        private void VisibilityCB_Changed(object sender, EventArgs e)
        {
            setCellsVisibility();
        }

        private void measureListCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            // MessageBox.Show(String.Format("{0} {1}", cb.SelectedIndex, this.measure.ResultCollectionsList.Count));
            MeasureResultCollection col = null;
            foreach (MeasureResultCollection mrc in ResultCollections)
            {
                if (cb.Text == mrc.Name)
                {
                    col = mrc;
                    break;
                }
            }
            if (col == null) return;
            measureResultTable.Rows.Clear();
            foreach (MeasureResult r in col.ResultsList)
            {
                 addRowToDataGrid(r);
            }
        }
    }
}
