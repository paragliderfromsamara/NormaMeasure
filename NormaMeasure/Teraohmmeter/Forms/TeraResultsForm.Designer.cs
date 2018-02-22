namespace NormaMeasure.Teraohmmeter.Forms
{
    partial class TeraResultsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.measureResultTable = new System.Windows.Forms.DataGridView();
            this.cycle_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stat_measure_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.range = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first_measure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_measure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measureListCB = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.measureResultTable)).BeginInit();
            this.SuspendLayout();
            // 
            // measureResultTable
            // 
            this.measureResultTable.AllowUserToAddRows = false;
            this.measureResultTable.AllowUserToDeleteRows = false;
            this.measureResultTable.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.measureResultTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.measureResultTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measureResultTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.measureResultTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measureResultTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cycle_number,
            this.stat_measure_number,
            this.voltage,
            this.result,
            this.range,
            this.first_measure,
            this.last_measure,
            this.time});
            this.measureResultTable.GridColor = System.Drawing.SystemColors.Control;
            this.measureResultTable.Location = new System.Drawing.Point(12, 93);
            this.measureResultTable.Name = "measureResultTable";
            this.measureResultTable.ReadOnly = true;
            this.measureResultTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measureResultTable.Size = new System.Drawing.Size(962, 371);
            this.measureResultTable.TabIndex = 3;
            // 
            // cycle_number
            // 
            this.cycle_number.HeaderText = "Цикл №";
            this.cycle_number.Name = "cycle_number";
            this.cycle_number.ReadOnly = true;
            this.cycle_number.Width = 50;
            // 
            // stat_measure_number
            // 
            this.stat_measure_number.HeaderText = "Измерение №";
            this.stat_measure_number.Name = "stat_measure_number";
            this.stat_measure_number.ReadOnly = true;
            this.stat_measure_number.Width = 50;
            // 
            // voltage
            // 
            this.voltage.HeaderText = "Напряжение, Вольт";
            this.voltage.Name = "voltage";
            this.voltage.ReadOnly = true;
            // 
            // result
            // 
            this.result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.result.HeaderText = "Результат";
            this.result.Name = "result";
            this.result.ReadOnly = true;
            // 
            // range
            // 
            this.range.HeaderText = "Диапазон";
            this.range.Name = "range";
            this.range.ReadOnly = true;
            this.range.Width = 50;
            // 
            // first_measure
            // 
            this.first_measure.HeaderText = "Начало";
            this.first_measure.Name = "first_measure";
            this.first_measure.ReadOnly = true;
            this.first_measure.Width = 50;
            // 
            // last_measure
            // 
            this.last_measure.HeaderText = "Конец";
            this.last_measure.Name = "last_measure";
            this.last_measure.ReadOnly = true;
            // 
            // time
            // 
            this.time.HeaderText = "Время";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            // 
            // measureListCB
            // 
            this.measureListCB.FormattingEnabled = true;
            this.measureListCB.Location = new System.Drawing.Point(12, 44);
            this.measureListCB.Name = "measureListCB";
            this.measureListCB.Size = new System.Drawing.Size(187, 21);
            this.measureListCB.TabIndex = 4;
            this.measureListCB.SelectedIndexChanged += new System.EventHandler(this.measureListCB_SelectedIndexChanged);
            // 
            // TeraResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 476);
            this.Controls.Add(this.measureListCB);
            this.Controls.Add(this.measureResultTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "TeraResultsForm";
            this.Text = "TeraResultsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TeraResultsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.measureResultTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView measureResultTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cycle_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn stat_measure_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn result;
        private System.Windows.Forms.DataGridViewTextBoxColumn range;
        private System.Windows.Forms.DataGridViewTextBoxColumn first_measure;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_measure;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.ComboBox measureListCB;
    }
}