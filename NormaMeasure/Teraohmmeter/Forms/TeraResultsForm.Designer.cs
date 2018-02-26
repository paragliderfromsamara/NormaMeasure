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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.measureResultTable = new System.Windows.Forms.DataGridView();
            this.measureListCB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.differenceCB = new System.Windows.Forms.CheckBox();
            this.timesCB = new System.Windows.Forms.CheckBox();
            this.endPointCB = new System.Windows.Forms.CheckBox();
            this.rangeCB = new System.Windows.Forms.CheckBox();
            this.startPointCB = new System.Windows.Forms.CheckBox();
            this.cycle_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stat_measure_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.range = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first_measure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_measure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.difference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.measureResultTable)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // measureResultTable
            // 
            this.measureResultTable.AllowUserToAddRows = false;
            this.measureResultTable.AllowUserToDeleteRows = false;
            this.measureResultTable.AllowUserToResizeRows = false;
            this.measureResultTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.measureResultTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.measureResultTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.measureResultTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Menu;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.measureResultTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.measureResultTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.measureResultTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cycle_number,
            this.stat_measure_number,
            this.voltage,
            this.result,
            this.range,
            this.first_measure,
            this.last_measure,
            this.difference,
            this.time});
            this.measureResultTable.GridColor = System.Drawing.SystemColors.Control;
            this.measureResultTable.Location = new System.Drawing.Point(12, 93);
            this.measureResultTable.Name = "measureResultTable";
            this.measureResultTable.ReadOnly = true;
            this.measureResultTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.measureResultTable.Size = new System.Drawing.Size(962, 371);
            this.measureResultTable.TabIndex = 3;
            // 
            // measureListCB
            // 
            this.measureListCB.FormattingEnabled = true;
            this.measureListCB.Location = new System.Drawing.Point(12, 9);
            this.measureListCB.Name = "measureListCB";
            this.measureListCB.Size = new System.Drawing.Size(187, 21);
            this.measureListCB.TabIndex = 4;
            this.measureListCB.SelectedIndexChanged += new System.EventHandler(this.measureListCB_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.differenceCB);
            this.groupBox1.Controls.Add(this.timesCB);
            this.groupBox1.Controls.Add(this.endPointCB);
            this.groupBox1.Controls.Add(this.rangeCB);
            this.groupBox1.Controls.Add(this.startPointCB);
            this.groupBox1.Location = new System.Drawing.Point(231, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(743, 78);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Управление таблицей";
            // 
            // differenceCB
            // 
            this.differenceCB.AutoSize = true;
            this.differenceCB.Location = new System.Drawing.Point(10, 48);
            this.differenceCB.Name = "differenceCB";
            this.differenceCB.Size = new System.Drawing.Size(69, 17);
            this.differenceCB.TabIndex = 4;
            this.differenceCB.Text = "Разница";
            this.differenceCB.UseVisualStyleBackColor = true;
            this.differenceCB.CheckedChanged += new System.EventHandler(this.VisibilityCB_Changed);
            // 
            // timesCB
            // 
            this.timesCB.AutoSize = true;
            this.timesCB.Location = new System.Drawing.Point(93, 48);
            this.timesCB.Name = "timesCB";
            this.timesCB.Size = new System.Drawing.Size(99, 17);
            this.timesCB.TabIndex = 3;
            this.timesCB.Text = "Длительность";
            this.timesCB.UseVisualStyleBackColor = true;
            this.timesCB.CheckedChanged += new System.EventHandler(this.VisibilityCB_Changed);
            // 
            // endPointCB
            // 
            this.endPointCB.AutoSize = true;
            this.endPointCB.Location = new System.Drawing.Point(93, 25);
            this.endPointCB.Name = "endPointCB";
            this.endPointCB.Size = new System.Drawing.Size(94, 17);
            this.endPointCB.TabIndex = 2;
            this.endPointCB.Text = "Конечн. сост.";
            this.endPointCB.UseVisualStyleBackColor = true;
            this.endPointCB.CheckedChanged += new System.EventHandler(this.VisibilityCB_Changed);
            // 
            // rangeCB
            // 
            this.rangeCB.AutoSize = true;
            this.rangeCB.Location = new System.Drawing.Point(193, 25);
            this.rangeCB.Name = "rangeCB";
            this.rangeCB.Size = new System.Drawing.Size(77, 17);
            this.rangeCB.TabIndex = 1;
            this.rangeCB.Text = "Диапазон";
            this.rangeCB.UseVisualStyleBackColor = true;
            this.rangeCB.CheckedChanged += new System.EventHandler(this.VisibilityCB_Changed);
            // 
            // startPointCB
            // 
            this.startPointCB.AutoSize = true;
            this.startPointCB.Location = new System.Drawing.Point(10, 25);
            this.startPointCB.Name = "startPointCB";
            this.startPointCB.Size = new System.Drawing.Size(77, 17);
            this.startPointCB.TabIndex = 0;
            this.startPointCB.Text = "Нач. сост.";
            this.startPointCB.UseVisualStyleBackColor = true;
            this.startPointCB.CheckedChanged += new System.EventHandler(this.VisibilityCB_Changed);
            // 
            // cycle_number
            // 
            this.cycle_number.HeaderText = "№";
            this.cycle_number.Name = "cycle_number";
            this.cycle_number.ReadOnly = true;
            // 
            // stat_measure_number
            // 
            this.stat_measure_number.HeaderText = "Измерение №";
            this.stat_measure_number.Name = "stat_measure_number";
            this.stat_measure_number.ReadOnly = true;
            // 
            // voltage
            // 
            this.voltage.HeaderText = "U, Вольт";
            this.voltage.Name = "voltage";
            this.voltage.ReadOnly = true;
            // 
            // result
            // 
            this.result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.result.HeaderText = "R";
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.Width = 321;
            // 
            // range
            // 
            this.range.HeaderText = "Диапазон";
            this.range.Name = "range";
            this.range.ReadOnly = true;
            // 
            // first_measure
            // 
            this.first_measure.HeaderText = "Начало";
            this.first_measure.Name = "first_measure";
            this.first_measure.ReadOnly = true;
            // 
            // last_measure
            // 
            this.last_measure.HeaderText = "Конец";
            this.last_measure.Name = "last_measure";
            this.last_measure.ReadOnly = true;
            // 
            // difference
            // 
            this.difference.HeaderText = "Разница";
            this.difference.Name = "difference";
            this.difference.ReadOnly = true;
            // 
            // time
            // 
            this.time.HeaderText = "Время";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            // 
            // TeraResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 476);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.measureListCB);
            this.Controls.Add(this.measureResultTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "TeraResultsForm";
            this.Text = "TeraResultsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TeraResultsForm_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.TeraResultsForm_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.measureResultTable)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView measureResultTable;
        private System.Windows.Forms.ComboBox measureListCB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox differenceCB;
        private System.Windows.Forms.CheckBox timesCB;
        private System.Windows.Forms.CheckBox endPointCB;
        private System.Windows.Forms.CheckBox rangeCB;
        private System.Windows.Forms.CheckBox startPointCB;
        private System.Windows.Forms.DataGridViewTextBoxColumn cycle_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn stat_measure_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn result;
        private System.Windows.Forms.DataGridViewTextBoxColumn range;
        private System.Windows.Forms.DataGridViewTextBoxColumn first_measure;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_measure;
        private System.Windows.Forms.DataGridViewTextBoxColumn difference;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
    }
}