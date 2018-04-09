using System;
using System.Windows.Forms;

namespace NormaMeasure.Teraohmmeter
{
    partial class TeraForm
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
            this.components = new System.ComponentModel.Container();
            this.voltageComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startHandMeasureBut = new System.Windows.Forms.Button();
            this.measureSettingsGroup = new System.Windows.Forms.GroupBox();
            this.measureTimesLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cycleTimes = new System.Windows.Forms.NumericUpDown();
            this.averagingTimes = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.dischargeDelay = new System.Windows.Forms.NumericUpDown();
            this.isCyclicMeasure = new System.Windows.Forms.CheckBox();
            this.polTime = new System.Windows.Forms.Label();
            this.polarizationDelay = new System.Windows.Forms.NumericUpDown();
            this.normaField = new System.Windows.Forms.NumericUpDown();
            this.normaFieldLbl = new System.Windows.Forms.Label();
            this.teraDS = new System.Data.DataSet();
            this.camera_types = new System.Data.DataTable();
            this.camera_type_id = new System.Data.DataColumn();
            this.camera_type_name = new System.Data.DataColumn();
            this.camera_type_internal_diameter = new System.Data.DataColumn();
            this.dataColumn1 = new System.Data.DataColumn();
            this.isolation_materials = new System.Data.DataTable();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.bringing_types = new System.Data.DataTable();
            this.bringing_type_id = new System.Data.DataColumn();
            this.bringing_type_name = new System.Data.DataColumn();
            this.materialTypes = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cameraLbl = new System.Windows.Forms.Label();
            this.cameraTypesCB = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bringingTypeCB = new System.Windows.Forms.ComboBox();
            this.materialLengthLbl = new System.Windows.Forms.Label();
            this.materialLength = new System.Windows.Forms.NumericUpDown();
            this.measureResultLbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.measureStatus = new System.Windows.Forms.Label();
            this.normaLbl = new System.Windows.Forms.Label();
            this.statMeasNumbOfLbl = new System.Windows.Forms.Label();
            this.cycleCounterLbl = new System.Windows.Forms.Label();
            this.measTimeLbl = new System.Windows.Forms.Label();
            this.midStatMeasValLbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.temperatureField = new System.Windows.Forms.NumericUpDown();
            this.bringingLengthMeasCb = new System.Windows.Forms.ComboBox();
            this.bringingToLbl = new System.Windows.Forms.Label();
            this.materialHeightLbl = new System.Windows.Forms.Label();
            this.materialHeight = new System.Windows.Forms.NumericUpDown();
            this.isDegreeViewCheckBox = new System.Windows.Forms.CheckBox();
            this.showResultsButton = new System.Windows.Forms.Button();
            this.handMeasurePanel = new System.Windows.Forms.Panel();
            this.measureIdLabel = new System.Windows.Forms.Label();
            this.MeasureTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.selectEtalonMapLbl = new System.Windows.Forms.Label();
            this.teraEtalonMapComboBox = new System.Windows.Forms.ComboBox();
            this.MeasureTimer = new System.Windows.Forms.Timer(this.components);
            this.verificationCalibrationPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.autoCorrCb = new System.Windows.Forms.CheckBox();
            this.corrCoeffLbl = new System.Windows.Forms.Label();
            this.rangeCoeffTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBoxResistance = new System.Windows.Forms.ComboBox();
            this.saveCoeffButton = new System.Windows.Forms.Button();
            this.measureSettingsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cycleTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.averagingTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dischargeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.polarizationDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.normaField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teraDS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_types)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isolation_materials)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bringing_types)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialLength)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.temperatureField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialHeight)).BeginInit();
            this.handMeasurePanel.SuspendLayout();
            this.verificationCalibrationPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // voltageComboBox
            // 
            this.voltageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voltageComboBox.FormattingEnabled = true;
            this.voltageComboBox.Items.AddRange(new object[] {
            "10",
            "100",
            "500",
            "1000"});
            this.voltageComboBox.Location = new System.Drawing.Point(15, 41);
            this.voltageComboBox.Name = "voltageComboBox";
            this.voltageComboBox.Size = new System.Drawing.Size(94, 21);
            this.voltageComboBox.TabIndex = 0;
            this.voltageComboBox.SelectedIndexChanged += new System.EventHandler(this.voltageComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Напряжение, В";
            // 
            // startHandMeasureBut
            // 
            this.startHandMeasureBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startHandMeasureBut.Location = new System.Drawing.Point(202, 365);
            this.startHandMeasureBut.Name = "startHandMeasureBut";
            this.startHandMeasureBut.Size = new System.Drawing.Size(570, 46);
            this.startHandMeasureBut.TabIndex = 2;
            this.startHandMeasureBut.Text = "ПУСК ИЗМЕРЕНИЯ";
            this.startHandMeasureBut.UseVisualStyleBackColor = false;
            this.startHandMeasureBut.Click += new System.EventHandler(this.startHandMeasureBut_Click);
            // 
            // measureSettingsGroup
            // 
            this.measureSettingsGroup.Controls.Add(this.measureTimesLbl);
            this.measureSettingsGroup.Controls.Add(this.label4);
            this.measureSettingsGroup.Controls.Add(this.cycleTimes);
            this.measureSettingsGroup.Controls.Add(this.averagingTimes);
            this.measureSettingsGroup.Controls.Add(this.label3);
            this.measureSettingsGroup.Controls.Add(this.dischargeDelay);
            this.measureSettingsGroup.Controls.Add(this.isCyclicMeasure);
            this.measureSettingsGroup.Controls.Add(this.polTime);
            this.measureSettingsGroup.Controls.Add(this.polarizationDelay);
            this.measureSettingsGroup.Controls.Add(this.voltageComboBox);
            this.measureSettingsGroup.Controls.Add(this.normaField);
            this.measureSettingsGroup.Controls.Add(this.normaFieldLbl);
            this.measureSettingsGroup.Controls.Add(this.label1);
            this.measureSettingsGroup.Location = new System.Drawing.Point(12, 7);
            this.measureSettingsGroup.Name = "measureSettingsGroup";
            this.measureSettingsGroup.Size = new System.Drawing.Size(760, 95);
            this.measureSettingsGroup.TabIndex = 3;
            this.measureSettingsGroup.TabStop = false;
            this.measureSettingsGroup.Text = "Настройки измерителя";
            // 
            // measureTimesLbl
            // 
            this.measureTimesLbl.AutoSize = true;
            this.measureTimesLbl.Location = new System.Drawing.Point(591, 26);
            this.measureTimesLbl.Name = "measureTimesLbl";
            this.measureTimesLbl.Size = new System.Drawing.Size(105, 13);
            this.measureTimesLbl.TabIndex = 10;
            this.measureTimesLbl.Text = "Количество циклов";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(487, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Усреднение";
            // 
            // cycleTimes
            // 
            this.cycleTimes.Location = new System.Drawing.Point(590, 43);
            this.cycleTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cycleTimes.Name = "cycleTimes";
            this.cycleTimes.Size = new System.Drawing.Size(140, 20);
            this.cycleTimes.TabIndex = 9;
            this.cycleTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // averagingTimes
            // 
            this.averagingTimes.Location = new System.Drawing.Point(490, 43);
            this.averagingTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.averagingTimes.Name = "averagingTimes";
            this.averagingTimes.Size = new System.Drawing.Size(94, 20);
            this.averagingTimes.TabIndex = 6;
            this.averagingTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(384, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Время разряда, с";
            // 
            // dischargeDelay
            // 
            this.dischargeDelay.Location = new System.Drawing.Point(387, 42);
            this.dischargeDelay.Name = "dischargeDelay";
            this.dischargeDelay.Size = new System.Drawing.Size(94, 20);
            this.dischargeDelay.TabIndex = 4;
            // 
            // isCyclicMeasure
            // 
            this.isCyclicMeasure.AutoSize = true;
            this.isCyclicMeasure.Location = new System.Drawing.Point(590, 72);
            this.isCyclicMeasure.Name = "isCyclicMeasure";
            this.isCyclicMeasure.Size = new System.Drawing.Size(140, 17);
            this.isCyclicMeasure.TabIndex = 8;
            this.isCyclicMeasure.Text = "Цикличное измерение";
            this.isCyclicMeasure.UseVisualStyleBackColor = true;
            this.isCyclicMeasure.CheckedChanged += new System.EventHandler(this.isCyclicMeasure_CheckedChanged);
            // 
            // polTime
            // 
            this.polTime.AutoSize = true;
            this.polTime.Location = new System.Drawing.Point(281, 25);
            this.polTime.Name = "polTime";
            this.polTime.Size = new System.Drawing.Size(86, 13);
            this.polTime.TabIndex = 3;
            this.polTime.Text = "Выдержка, мин";
            // 
            // polarizationDelay
            // 
            this.polarizationDelay.Location = new System.Drawing.Point(284, 42);
            this.polarizationDelay.Name = "polarizationDelay";
            this.polarizationDelay.Size = new System.Drawing.Size(94, 20);
            this.polarizationDelay.TabIndex = 2;
            // 
            // normaField
            // 
            this.normaField.Location = new System.Drawing.Point(124, 42);
            this.normaField.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.normaField.Name = "normaField";
            this.normaField.Size = new System.Drawing.Size(144, 20);
            this.normaField.TabIndex = 17;
            this.normaField.ValueChanged += new System.EventHandler(this.normaField_ValueChanged);
            // 
            // normaFieldLbl
            // 
            this.normaFieldLbl.AutoSize = true;
            this.normaFieldLbl.Location = new System.Drawing.Point(121, 26);
            this.normaFieldLbl.Name = "normaFieldLbl";
            this.normaFieldLbl.Size = new System.Drawing.Size(72, 13);
            this.normaFieldLbl.TabIndex = 16;
            this.normaFieldLbl.Text = "Норма, МОм";
            // 
            // teraDS
            // 
            this.teraDS.DataSetName = "NewDataSet";
            this.teraDS.Tables.AddRange(new System.Data.DataTable[] {
            this.camera_types,
            this.isolation_materials,
            this.bringing_types});
            // 
            // camera_types
            // 
            this.camera_types.Columns.AddRange(new System.Data.DataColumn[] {
            this.camera_type_id,
            this.camera_type_name,
            this.camera_type_internal_diameter,
            this.dataColumn1});
            this.camera_types.TableName = "camera_types";
            // 
            // camera_type_id
            // 
            this.camera_type_id.ColumnName = "id";
            // 
            // camera_type_name
            // 
            this.camera_type_name.ColumnName = "name";
            // 
            // camera_type_internal_diameter
            // 
            this.camera_type_internal_diameter.ColumnName = "internal_diameter";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "external_diameter";
            // 
            // isolation_materials
            // 
            this.isolation_materials.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn2,
            this.dataColumn3});
            this.isolation_materials.TableName = "isolation_materials";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "id";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "name";
            // 
            // bringing_types
            // 
            this.bringing_types.Columns.AddRange(new System.Data.DataColumn[] {
            this.bringing_type_id,
            this.bringing_type_name});
            this.bringing_types.TableName = "bringing_types";
            // 
            // bringing_type_id
            // 
            this.bringing_type_id.ColumnName = "id";
            // 
            // bringing_type_name
            // 
            this.bringing_type_name.ColumnName = "name";
            // 
            // materialTypes
            // 
            this.materialTypes.DataSource = this.teraDS;
            this.materialTypes.DisplayMember = "isolation_materials.name";
            this.materialTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.materialTypes.FormattingEnabled = true;
            this.materialTypes.Location = new System.Drawing.Point(3, 69);
            this.materialTypes.Name = "materialTypes";
            this.materialTypes.Size = new System.Drawing.Size(148, 21);
            this.materialTypes.TabIndex = 4;
            this.materialTypes.ValueMember = "isolation_materials.id";
            this.materialTypes.SelectedIndexChanged += new System.EventHandler(this.materialTypes_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Материал";
            // 
            // cameraLbl
            // 
            this.cameraLbl.AutoSize = true;
            this.cameraLbl.Location = new System.Drawing.Point(-6, 200);
            this.cameraLbl.Name = "cameraLbl";
            this.cameraLbl.Size = new System.Drawing.Size(151, 13);
            this.cameraLbl.TabIndex = 6;
            this.cameraLbl.Text = "Тип измерительной камеры";
            // 
            // cameraTypesCB
            // 
            this.cameraTypesCB.DataSource = this.teraDS;
            this.cameraTypesCB.DisplayMember = "camera_types.name";
            this.cameraTypesCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraTypesCB.FormattingEnabled = true;
            this.cameraTypesCB.Location = new System.Drawing.Point(0, 215);
            this.cameraTypesCB.Name = "cameraTypesCB";
            this.cameraTypesCB.Size = new System.Drawing.Size(148, 21);
            this.cameraTypesCB.TabIndex = 7;
            this.cameraTypesCB.ValueMember = "camera_types.id";
            this.cameraTypesCB.SelectedIndexChanged += new System.EventHandler(this.cameraTypesCB_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Тип приведения";
            // 
            // bringingTypeCB
            // 
            this.bringingTypeCB.DataSource = this.teraDS;
            this.bringingTypeCB.DisplayMember = "bringing_types.name";
            this.bringingTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bringingTypeCB.FormattingEnabled = true;
            this.bringingTypeCB.Location = new System.Drawing.Point(0, 165);
            this.bringingTypeCB.Name = "bringingTypeCB";
            this.bringingTypeCB.Size = new System.Drawing.Size(148, 21);
            this.bringingTypeCB.TabIndex = 9;
            this.bringingTypeCB.ValueMember = "bringing_types.id";
            this.bringingTypeCB.SelectedIndexChanged += new System.EventHandler(this.bringingTypeCB_SelectedIndexChanged);
            // 
            // materialLengthLbl
            // 
            this.materialLengthLbl.AutoSize = true;
            this.materialLengthLbl.Location = new System.Drawing.Point(-6, 198);
            this.materialLengthLbl.Name = "materialLengthLbl";
            this.materialLengthLbl.Size = new System.Drawing.Size(93, 13);
            this.materialLengthLbl.TabIndex = 10;
            this.materialLengthLbl.Text = "Длина кабеля, м";
            // 
            // materialLength
            // 
            this.materialLength.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.materialLength.Location = new System.Drawing.Point(0, 214);
            this.materialLength.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.materialLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.materialLength.Name = "materialLength";
            this.materialLength.Size = new System.Drawing.Size(148, 20);
            this.materialLength.TabIndex = 11;
            this.materialLength.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // measureResultLbl
            // 
            this.measureResultLbl.AutoSize = true;
            this.measureResultLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.measureResultLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.measureResultLbl.Location = new System.Drawing.Point(60, 80);
            this.measureResultLbl.Name = "measureResultLbl";
            this.measureResultLbl.Size = new System.Drawing.Size(415, 73);
            this.measureResultLbl.TabIndex = 13;
            this.measureResultLbl.Text = "99999.0 ГОм";
            this.measureResultLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.measureStatus);
            this.panel1.Controls.Add(this.normaLbl);
            this.panel1.Controls.Add(this.statMeasNumbOfLbl);
            this.panel1.Controls.Add(this.cycleCounterLbl);
            this.panel1.Controls.Add(this.measTimeLbl);
            this.panel1.Controls.Add(this.midStatMeasValLbl);
            this.panel1.Controls.Add(this.measureResultLbl);
            this.panel1.Location = new System.Drawing.Point(202, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 228);
            this.panel1.TabIndex = 14;
            // 
            // measureStatus
            // 
            this.measureStatus.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.measureStatus.AutoSize = true;
            this.measureStatus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.measureStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.measureStatus.Location = new System.Drawing.Point(3, 9);
            this.measureStatus.Name = "measureStatus";
            this.measureStatus.Size = new System.Drawing.Size(131, 16);
            this.measureStatus.TabIndex = 19;
            this.measureStatus.Text = "Идёт измерение....";
            this.measureStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // normaLbl
            // 
            this.normaLbl.AutoSize = true;
            this.normaLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.normaLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.normaLbl.Location = new System.Drawing.Point(70, 149);
            this.normaLbl.Name = "normaLbl";
            this.normaLbl.Size = new System.Drawing.Size(146, 16);
            this.normaLbl.TabIndex = 18;
            this.normaLbl.Text = "Норма:  100500 МОм";
            this.normaLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statMeasNumbOfLbl
            // 
            this.statMeasNumbOfLbl.AutoSize = true;
            this.statMeasNumbOfLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statMeasNumbOfLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.statMeasNumbOfLbl.Location = new System.Drawing.Point(421, 29);
            this.statMeasNumbOfLbl.Name = "statMeasNumbOfLbl";
            this.statMeasNumbOfLbl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statMeasNumbOfLbl.Size = new System.Drawing.Size(129, 16);
            this.statMeasNumbOfLbl.TabIndex = 17;
            this.statMeasNumbOfLbl.Text = "измерение 4 из 10";
            this.statMeasNumbOfLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cycleCounterLbl
            // 
            this.cycleCounterLbl.AutoSize = true;
            this.cycleCounterLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cycleCounterLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.cycleCounterLbl.Location = new System.Drawing.Point(55, 207);
            this.cycleCounterLbl.Name = "cycleCounterLbl";
            this.cycleCounterLbl.Size = new System.Drawing.Size(102, 16);
            this.cycleCounterLbl.TabIndex = 16;
            this.cycleCounterLbl.Text = "цикл:  100500";
            this.cycleCounterLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // measTimeLbl
            // 
            this.measTimeLbl.AutoSize = true;
            this.measTimeLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.measTimeLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.measTimeLbl.Location = new System.Drawing.Point(3, 207);
            this.measTimeLbl.Name = "measTimeLbl";
            this.measTimeLbl.Size = new System.Drawing.Size(45, 16);
            this.measTimeLbl.TabIndex = 15;
            this.measTimeLbl.Text = "00:00";
            // 
            // midStatMeasValLbl
            // 
            this.midStatMeasValLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.midStatMeasValLbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.midStatMeasValLbl.ForeColor = System.Drawing.Color.SteelBlue;
            this.midStatMeasValLbl.Location = new System.Drawing.Point(267, 6);
            this.midStatMeasValLbl.Name = "midStatMeasValLbl";
            this.midStatMeasValLbl.Size = new System.Drawing.Size(298, 23);
            this.midStatMeasValLbl.TabIndex = 14;
            this.midStatMeasValLbl.Text = "промежуточное значение: 0.0 ГОм";
            this.midStatMeasValLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Температура, С°";
            // 
            // temperatureField
            // 
            this.temperatureField.Location = new System.Drawing.Point(3, 20);
            this.temperatureField.Maximum = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.temperatureField.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.temperatureField.Name = "temperatureField";
            this.temperatureField.Size = new System.Drawing.Size(148, 20);
            this.temperatureField.TabIndex = 19;
            this.temperatureField.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.temperatureField.ValueChanged += new System.EventHandler(this.temperatureField_ValueChanged);
            // 
            // bringingLengthMeasCb
            // 
            this.bringingLengthMeasCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bringingLengthMeasCb.FormattingEnabled = true;
            this.bringingLengthMeasCb.Items.AddRange(new object[] {
            "см",
            "дм",
            "м",
            "км"});
            this.bringingLengthMeasCb.Location = new System.Drawing.Point(81, 259);
            this.bringingLengthMeasCb.Name = "bringingLengthMeasCb";
            this.bringingLengthMeasCb.Size = new System.Drawing.Size(67, 21);
            this.bringingLengthMeasCb.TabIndex = 20;
            // 
            // bringingToLbl
            // 
            this.bringingToLbl.AutoSize = true;
            this.bringingToLbl.Location = new System.Drawing.Point(-3, 262);
            this.bringingToLbl.Name = "bringingToLbl";
            this.bringingToLbl.Size = new System.Drawing.Size(78, 13);
            this.bringingToLbl.TabIndex = 21;
            this.bringingToLbl.Text = "Приведение к";
            // 
            // materialHeightLbl
            // 
            this.materialHeightLbl.AutoSize = true;
            this.materialHeightLbl.Location = new System.Drawing.Point(-3, 245);
            this.materialHeightLbl.Name = "materialHeightLbl";
            this.materialHeightLbl.Size = new System.Drawing.Size(139, 13);
            this.materialHeightLbl.TabIndex = 22;
            this.materialHeightLbl.Text = "Толщина материала, мкм";
            // 
            // materialHeight
            // 
            this.materialHeight.Location = new System.Drawing.Point(0, 260);
            this.materialHeight.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.materialHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.materialHeight.Name = "materialHeight";
            this.materialHeight.Size = new System.Drawing.Size(148, 20);
            this.materialHeight.TabIndex = 23;
            this.materialHeight.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // isDegreeViewCheckBox
            // 
            this.isDegreeViewCheckBox.AutoSize = true;
            this.isDegreeViewCheckBox.Location = new System.Drawing.Point(202, 342);
            this.isDegreeViewCheckBox.Name = "isDegreeViewCheckBox";
            this.isDegreeViewCheckBox.Size = new System.Drawing.Size(257, 17);
            this.isDegreeViewCheckBox.TabIndex = 24;
            this.isDegreeViewCheckBox.Text = "Отображать результат в степенном формате";
            this.isDegreeViewCheckBox.UseVisualStyleBackColor = true;
            // 
            // showResultsButton
            // 
            this.showResultsButton.Location = new System.Drawing.Point(202, 439);
            this.showResultsButton.Name = "showResultsButton";
            this.showResultsButton.Size = new System.Drawing.Size(177, 23);
            this.showResultsButton.TabIndex = 28;
            this.showResultsButton.Text = "Показать результаты";
            this.showResultsButton.UseVisualStyleBackColor = true;
            this.showResultsButton.Click += new System.EventHandler(this.showResultsButton_Click);
            // 
            // handMeasurePanel
            // 
            this.handMeasurePanel.Controls.Add(this.measureIdLabel);
            this.handMeasurePanel.Controls.Add(this.MeasureTitle);
            this.handMeasurePanel.Controls.Add(this.temperatureField);
            this.handMeasurePanel.Controls.Add(this.materialLength);
            this.handMeasurePanel.Controls.Add(this.materialLengthLbl);
            this.handMeasurePanel.Controls.Add(this.label6);
            this.handMeasurePanel.Controls.Add(this.bringingTypeCB);
            this.handMeasurePanel.Controls.Add(this.label7);
            this.handMeasurePanel.Controls.Add(this.materialTypes);
            this.handMeasurePanel.Controls.Add(this.bringingLengthMeasCb);
            this.handMeasurePanel.Controls.Add(this.materialHeight);
            this.handMeasurePanel.Controls.Add(this.cameraTypesCB);
            this.handMeasurePanel.Controls.Add(this.label5);
            this.handMeasurePanel.Controls.Add(this.bringingToLbl);
            this.handMeasurePanel.Controls.Add(this.materialHeightLbl);
            this.handMeasurePanel.Controls.Add(this.cameraLbl);
            this.handMeasurePanel.Location = new System.Drawing.Point(24, 173);
            this.handMeasurePanel.Name = "handMeasurePanel";
            this.handMeasurePanel.Size = new System.Drawing.Size(151, 289);
            this.handMeasurePanel.TabIndex = 27;
            // 
            // measureIdLabel
            // 
            this.measureIdLabel.AutoSize = true;
            this.measureIdLabel.Location = new System.Drawing.Point(0, 102);
            this.measureIdLabel.Name = "measureIdLabel";
            this.measureIdLabel.Size = new System.Drawing.Size(146, 13);
            this.measureIdLabel.TabIndex = 25;
            this.measureIdLabel.Text = "Идентификатор измерения";
            // 
            // MeasureTitle
            // 
            this.MeasureTitle.Location = new System.Drawing.Point(0, 118);
            this.MeasureTitle.Name = "MeasureTitle";
            this.MeasureTitle.Size = new System.Drawing.Size(151, 20);
            this.MeasureTitle.TabIndex = 24;
            this.MeasureTitle.Text = "Измерение 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Тип испытания";
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "Ручные испытания",
            "Поверка",
            "Калибровка"});
            this.comboBoxMode.Location = new System.Drawing.Point(24, 133);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(151, 21);
            this.comboBoxMode.TabIndex = 25;
            this.comboBoxMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxMode_SelectedIndexChanged);
            // 
            // selectEtalonMapLbl
            // 
            this.selectEtalonMapLbl.AutoSize = true;
            this.selectEtalonMapLbl.Location = new System.Drawing.Point(-3, 3);
            this.selectEtalonMapLbl.Name = "selectEtalonMapLbl";
            this.selectEtalonMapLbl.Size = new System.Drawing.Size(87, 13);
            this.selectEtalonMapLbl.TabIndex = 1;
            this.selectEtalonMapLbl.Text = "Карта эталонов";
            // 
            // teraEtalonMapComboBox
            // 
            this.teraEtalonMapComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.teraEtalonMapComboBox.FormattingEnabled = true;
            this.teraEtalonMapComboBox.Location = new System.Drawing.Point(0, 19);
            this.teraEtalonMapComboBox.Name = "teraEtalonMapComboBox";
            this.teraEtalonMapComboBox.Size = new System.Drawing.Size(151, 21);
            this.teraEtalonMapComboBox.TabIndex = 0;
            this.teraEtalonMapComboBox.SelectedIndexChanged += new System.EventHandler(this.teraEtalonMapComboBox_SelectedIndexChanged);
            // 
            // MeasureTimer
            // 
            this.MeasureTimer.Interval = 1000;
            // 
            // verificationCalibrationPanel
            // 
            this.verificationCalibrationPanel.Controls.Add(this.panel2);
            this.verificationCalibrationPanel.Controls.Add(this.label10);
            this.verificationCalibrationPanel.Controls.Add(this.comboBoxResistance);
            this.verificationCalibrationPanel.Controls.Add(this.teraEtalonMapComboBox);
            this.verificationCalibrationPanel.Controls.Add(this.selectEtalonMapLbl);
            this.verificationCalibrationPanel.Location = new System.Drawing.Point(791, 62);
            this.verificationCalibrationPanel.Name = "verificationCalibrationPanel";
            this.verificationCalibrationPanel.Size = new System.Drawing.Size(158, 242);
            this.verificationCalibrationPanel.TabIndex = 27;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.autoCorrCb);
            this.panel2.Controls.Add(this.saveCoeffButton);
            this.panel2.Controls.Add(this.corrCoeffLbl);
            this.panel2.Controls.Add(this.rangeCoeffTextBox);
            this.panel2.Location = new System.Drawing.Point(-4, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(161, 128);
            this.panel2.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 102);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 21);
            this.button1.TabIndex = 29;
            this.button1.Text = "Подбор коэфф. U";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.coeffUForm_Click);
            // 
            // autoCorrCb
            // 
            this.autoCorrCb.AutoSize = true;
            this.autoCorrCb.Location = new System.Drawing.Point(4, 50);
            this.autoCorrCb.Name = "autoCorrCb";
            this.autoCorrCb.Size = new System.Drawing.Size(104, 17);
            this.autoCorrCb.TabIndex = 8;
            this.autoCorrCb.Text = "Автоматически";
            this.autoCorrCb.UseVisualStyleBackColor = true;
            this.autoCorrCb.CheckedChanged += new System.EventHandler(this.autoCorrCb_CheckedChanged);
            // 
            // corrCoeffLbl
            // 
            this.corrCoeffLbl.AutoSize = true;
            this.corrCoeffLbl.Location = new System.Drawing.Point(1, 8);
            this.corrCoeffLbl.Name = "corrCoeffLbl";
            this.corrCoeffLbl.Size = new System.Drawing.Size(134, 13);
            this.corrCoeffLbl.TabIndex = 5;
            this.corrCoeffLbl.Text = "Коэффициент коррекции";
            // 
            // rangeCoeffTextBox
            // 
            this.rangeCoeffTextBox.Location = new System.Drawing.Point(4, 24);
            this.rangeCoeffTextBox.Name = "rangeCoeffTextBox";
            this.rangeCoeffTextBox.Size = new System.Drawing.Size(151, 20);
            this.rangeCoeffTextBox.TabIndex = 4;
            this.rangeCoeffTextBox.TextChanged += new System.EventHandler(this.rangeCoeffTextBox_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(-3, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Резистор";
            // 
            // comboBoxResistance
            // 
            this.comboBoxResistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxResistance.FormattingEnabled = true;
            this.comboBoxResistance.Location = new System.Drawing.Point(0, 69);
            this.comboBoxResistance.Name = "comboBoxResistance";
            this.comboBoxResistance.Size = new System.Drawing.Size(151, 21);
            this.comboBoxResistance.TabIndex = 2;
            this.comboBoxResistance.SelectedIndexChanged += new System.EventHandler(this.comboBoxResistance_SelectedIndexChanged);
            // 
            // saveCoeffButton
            // 
            this.saveCoeffButton.Location = new System.Drawing.Point(4, 73);
            this.saveCoeffButton.Name = "saveCoeffButton";
            this.saveCoeffButton.Size = new System.Drawing.Size(151, 23);
            this.saveCoeffButton.TabIndex = 7;
            this.saveCoeffButton.Text = "Сохранить";
            this.saveCoeffButton.UseVisualStyleBackColor = true;
            this.saveCoeffButton.Click += new System.EventHandler(this.saveCoeffButton_Click);
            // 
            // TeraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 476);
            this.Controls.Add(this.showResultsButton);
            this.Controls.Add(this.verificationCalibrationPanel);
            this.Controls.Add(this.handMeasurePanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.measureSettingsGroup);
            this.Controls.Add(this.comboBoxMode);
            this.Controls.Add(this.isDegreeViewCheckBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.startHandMeasureBut);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TeraForm";
            this.Text = "Ручные измерения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TeraForm_FormClosing);
            this.measureSettingsGroup.ResumeLayout(false);
            this.measureSettingsGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cycleTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.averagingTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dischargeDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.polarizationDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.normaField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teraDS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera_types)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isolation_materials)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bringing_types)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialLength)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.temperatureField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialHeight)).EndInit();
            this.handMeasurePanel.ResumeLayout(false);
            this.handMeasurePanel.PerformLayout();
            this.verificationCalibrationPanel.ResumeLayout(false);
            this.verificationCalibrationPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox voltageComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startHandMeasureBut;
        private System.Windows.Forms.GroupBox measureSettingsGroup;
        private System.Windows.Forms.Label measureTimesLbl;
        private System.Windows.Forms.NumericUpDown cycleTimes;
        private System.Windows.Forms.CheckBox isCyclicMeasure;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown averagingTimes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown dischargeDelay;
        private System.Windows.Forms.Label polTime;
        private System.Windows.Forms.NumericUpDown polarizationDelay;
        private System.Data.DataSet teraDS;
        private System.Data.DataTable camera_types;
        private System.Data.DataColumn camera_type_id;
        private System.Data.DataColumn camera_type_name;
        private System.Data.DataColumn camera_type_internal_diameter;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataTable isolation_materials;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Windows.Forms.ComboBox materialTypes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label cameraLbl;
        private System.Windows.Forms.ComboBox cameraTypesCB;
        private System.Data.DataTable bringing_types;
        private System.Data.DataColumn bringing_type_id;
        private System.Data.DataColumn bringing_type_name;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox bringingTypeCB;
        private System.Windows.Forms.Label materialLengthLbl;
        private System.Windows.Forms.NumericUpDown materialLength;
        private System.Windows.Forms.Label measureResultLbl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label statMeasNumbOfLbl;
        private System.Windows.Forms.Label cycleCounterLbl;
        private System.Windows.Forms.Label measTimeLbl;
        private System.Windows.Forms.Label midStatMeasValLbl;
        private System.Windows.Forms.Label normaLbl;
        private System.Windows.Forms.Label normaFieldLbl;
        private System.Windows.Forms.NumericUpDown normaField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown temperatureField;
        private System.Windows.Forms.ComboBox bringingLengthMeasCb;
        private System.Windows.Forms.Label bringingToLbl;
        private System.Windows.Forms.Label materialHeightLbl;
        private System.Windows.Forms.NumericUpDown materialHeight;
        private System.Windows.Forms.CheckBox isDegreeViewCheckBox;
        private System.Windows.Forms.Label measureStatus;
        private Timer MeasureTimer;
        private Label selectEtalonMapLbl;
        private ComboBox teraEtalonMapComboBox;
        private Label label2;
        private ComboBox comboBoxMode;
        private Panel handMeasurePanel;
        private Panel verificationCalibrationPanel;
        private Label measureIdLabel;
        private TextBox MeasureTitle;
        private Label label10;
        private ComboBox comboBoxResistance;
        private Panel panel2;
        private CheckBox autoCorrCb;
        private Label corrCoeffLbl;
        private TextBox rangeCoeffTextBox;
        private Button showResultsButton;
        private Button button1;
        private Button saveCoeffButton;
    }
}