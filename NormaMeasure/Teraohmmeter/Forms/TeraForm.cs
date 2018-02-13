using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;

using NormaMeasure.DBClasses;
using NormaMeasure.BaseClasses;
using NormaMeasure.Utils;
using NormaMeasure.Teraohmmeter.DBClasses;

namespace NormaMeasure.Teraohmmeter
{
    public partial class TeraForm : Form
    {
        private MainForm mainForm; 
        double[][] isolationMaterialCoeffsArr;
        private bool measureIsActive = false;
        private string curEtalonMapId = String.Empty; 

        protected delegate void updateServiceFieldDelegate(string serviceInfo);
        protected delegate void updateResultFieldDelegate();
        protected delegate void updateCycleNumberFieldDelegate(string cycleNumb);
        protected delegate void updateStatMeasInfoDelegate(string[] statMeasInfo);
        protected delegate void refreshMeasureTimerDelegate(int seconds);
        protected delegate void switchFieldsMeasureOnOffDelegate(bool flag);
        protected delegate void updateResultFieldTextDelegate(string text);
        protected delegate void updateMeasureStatusDelegate(MEASURE_STATUS status);
        protected delegate void updateCorrCoeffFieldDelegate(double coeff);


        TeraDevice teraDevice;
        TeraMeasure measure;
        public TeraForm(TeraDevice tera_device, MainForm f)
        {
            InitializeComponent();
            this.mainForm = f;
            this.teraDevice = tera_device;
            this.Text = teraDevice.NameWithSerial();
            fillTeraDS();
        }

        public void InitAndShow()
        {
            this.Width = this.Width - verificationCalibrationPanel.Width;
            verificationCalibrationPanel.Parent = handMeasurePanel.Parent;
            verificationCalibrationPanel.Location = handMeasurePanel.Location;
            comboBoxMode.SelectedIndex = 0;
            //initHandMeasurePage();
            //initVerificationPage();
            this.Show();
        }

        private void fillTeraDS()
        {
            DBControl dc = new DBControl();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da = new MySqlDataAdapter(TeraSettings.Default.selectMaterials, dc.MyConn);
            da.Fill(isolation_materials);
            da = new MySqlDataAdapter(TeraSettings.Default.selectCameraTypes, dc.MyConn);
            da.Fill(camera_types);
            da = new MySqlDataAdapter(TeraSettings.Default.selectBringingTypes, dc.MyConn);
            da.Fill(bringing_types);
            dc.Dispose();
            da.Dispose();
        }

        private void initPage()
        {
            this.measure = new TeraMeasure(teraDevice, MEASURE_TYPE.HAND);
            fillHandMeasureParameters();
            buildIsolationMaterialArr();
            switchBringingParams();
            getCameraDiametersByCameraId();
            cleanMeasInfo();
  
        }

        private void fillHandMeasureParameters()
        {

            //Заполняем поля значениями по умолчанию
            temperatureField.Value = measure.Temperature;
            voltageComboBox.Text = measure.Voltage.ToString();
            dischargeDelay.Value = measure.DischargeDelay;
            polarizationDelay.Value = measure.PolarizationDelay;
            cycleTimes.Value = measure.CycleTimes;
            averagingTimes.Value = measure.AveragingTimes;
            bringingLengthMeasCb.Text = measure.BringingLengthMeasure;
            materialHeight.Value = measure.MaterialHeight;
            isDegreeViewCheckBox.Checked = measure.IsDegreeViewMode;
            //minTimeToNorm.Value = handMeasure.MinTimeToNorm;
            normaField.Value = measure.NormaValue;
            materialLength.Value = measure.BringingLength;
            foreach (DataRow r in camera_types.Rows)
            {
                if (r["internal_diameter"].ToString() == measure.InternalCamDiam.ToString() && r["external_diameter"].ToString() == measure.ExternalCamDiam.ToString())
                {
                    cameraTypesCB.Text = r["name"].ToString();
                    break;
                }
            }
            if (String.IsNullOrEmpty(cameraTypesCB.Text)) cameraTypesCB.Text = camera_types.Rows[0]["name"].ToString();
            bringingTypeCB.SelectedValue = measure.BringingTypeId;
            materialTypes.SelectedValue = measure.MaterialId;
        }

        /// <summary>
        /// Выкачивает из БД коэффициенты температуры изоляционных материалов и забивает их в isolationMaterialCoeffsArr
        /// </summary>
        private void buildIsolationMaterialArr()
        {
            int materialsNumb = this.materialTypes.Items.Count;
            double[][] resultArr = new double[materialsNumb][];
            DBControl dc = new DBControl(TeraSettings.Default.dbName);
            MySqlDataAdapter da = new MySqlDataAdapter();
            for (int i = 0; i < materialsNumb; i++)
            {
                string q = String.Format(TeraSettings.Default.selectIsolationMaterialCoeffs, i + 1);
                DataSet ds = new DataSet();
                da = new MySqlDataAdapter(q, dc.MyConn);
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                double[] coeffsArr = new double[dt.Rows.Count];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    coeffsArr[j] = Convert.ToDouble(dt.Rows[j]["coeff_val"].ToString());
                }
                resultArr[i] = coeffsArr;
            }
            this.isolationMaterialCoeffsArr = resultArr;
            /*
            string s = "";
            for(int i =0; i< defArr.Length; i++)
            {
                if (i > 0) s += "\n";
                for(int j=0; j< this.materialTypes.Items.Count; j++)
                {
                    s += string.Format("  {0}", this.isolationMaterialCoeffsArr[j][i]); 
                }
            }
            MessageBox.Show(s);
            */

        }

        private void normaField_ValueChanged(object sender, EventArgs e)
        {
            //int v = Convert.ToInt32(this.normaField.Value);
            //this.polTime.Text = (v > 0) ? "Выдержка, мин" : "Поляризация, мин";
        }

        private void isCyclicMeasure_CheckedChanged(object sender, EventArgs e)
        {
            this.cycleTimes.Enabled = !this.isCyclicMeasure.Checked;
        }

        private void cameraTypesCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCameraDiametersByCameraId();
        }

        private void getCameraDiametersByCameraId()
        {
            string cId = cameraTypesCB.SelectedValue.ToString();
            foreach (DataRow r in camera_types.Rows)
            {
                if (r["id"].ToString() == cId)
                {
                    this.measure.InternalCamDiam = Convert.ToInt16(r["internal_diameter"].ToString());
                    this.measure.ExternalCamDiam = Convert.ToInt16(r["external_diameter"].ToString());
                    break;
                }
            }
        }


        private void bringingTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            switchBringingParams();
        }

        /// <summary>
        /// В зависимости от режима измерения скрывает или показывает настройки измеряемых параметров
        /// </summary>
        private void switchBringingParams()
        {
            switch (bringingTypeCB.SelectedValue.ToString())
            {
                case "1":
                    materialHeightLbl.Visible = materialHeight.Visible = bringingToLbl.Visible = bringingLengthMeasCb.Visible = cameraLbl.Visible = cameraTypesCB.Visible = materialLength.Visible = materialLengthLbl.Visible = false;
                    break;
                case "2":
                    bringingToLbl.Visible = bringingLengthMeasCb.Visible = materialLength.Visible = materialLengthLbl.Visible = true;
                    materialHeightLbl.Visible = materialHeight.Visible = cameraLbl.Visible = cameraTypesCB.Visible = false;
                    break;
                case "3":
                    cameraLbl.Visible = materialHeightLbl.Visible = materialHeight.Visible = cameraTypesCB.Visible = true;
                    bringingToLbl.Visible = bringingLengthMeasCb.Visible = materialLength.Visible = materialLengthLbl.Visible = false;
                    break;
                case "4":
                    cameraLbl.Visible = cameraTypesCB.Visible = true;
                    materialHeightLbl.Visible = materialHeight.Visible = bringingToLbl.Visible = bringingLengthMeasCb.Visible = materialLength.Visible = materialLengthLbl.Visible = false;
                    break;
            }
        }

        private void cleanMeasInfo()
        {
            this.measureStatus.Text = TeraMeasure.StatusString(MEASURE_STATUS.NOT_STARTED);
            this.measTimeLbl.Text = this.normaLbl.Text = this.cycleCounterLbl.Text = this.statMeasNumbOfLbl.Text = this.midStatMeasValLbl.Text = this.measTimeLbl.Text = "";
            this.measureResultLbl.Text = "0.0 МОм";
        }

        private void startHandMeasureBut_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.teraMeas.isOnMeasure().ToString());
            
            if (!this.measure.IsStarted)
            {
                if (!checkMeasurePossibility()) return;
                this.teraDevice.OpenPort();
                    measure.Name = MeasureTitle.Text;
                    measure.Temperature = (int)temperatureField.Value;
                    measure.Voltage = Convert.ToInt32(voltageComboBox.Text);
                    measure.DischargeDelay = Convert.ToInt16(dischargeDelay.Value);
                    measure.PolarizationDelay = Convert.ToInt32(this.polarizationDelay.Value);
                    measure.IsCyclicMeasure = isCyclicMeasure.Checked;
                    measure.CycleTimes = Convert.ToInt32(cycleTimes.Value);
                    measure.AveragingTimes = Convert.ToInt32(this.averagingTimes.Value);
                    measure.BringingTypeId = bringingTypeCB.SelectedValue.ToString();
                    measure.MaterialHeight = Convert.ToInt32(this.materialHeight.Value);
                    measure.IsDegreeViewMode = isDegreeViewCheckBox.Checked;
                    // handMeasure.MinTimeToNorm = Convert.ToInt32(minTimeToNorm.Value);
                    measure.NormaValue = Convert.ToInt32(this.normaField.Value);
                    measure.MaterialId = materialTypes.SelectedValue.ToString();
                    measure.BringingLength = Convert.ToInt16(materialLength.Value);
                    measure.BringingLengthMeasure = this.bringingLengthMeasCb.SelectedText;
                    measure.CorrectionMode = autoCorrCb.Checked ? MEASURE_TYPE.AUTO : MEASURE_TYPE.HAND;
                    measure.RangeCoeff = autoCorrCb.Checked ? 1 : ServiceFunctions.convertToFloat(rangeCoeffTextBox.Text);
                    this.switchFieldsMeasureOnOff(this.measure.IsStarted);
            }else
                {
                     measure.StopWithStatus(MEASURE_STATUS.STOPED);
                     Thread.Sleep(300);
                     if (!Properties.Settings.Default.IsTestApp)
                     {
                         this.teraDevice.DevicePort.DiscardInBuffer();
                        // handMeasure.Stop();
                         switchFieldsMeasureOnOff(true);
                     }
                     else switchFieldsMeasureOnOff(true);
            }
                //else { this.mForm.currentDevice.stopMeasure();}//this.switchFieldsMeasureOnOff(this.teraMeas.isOnMeasure());
                //this.switchFieldsMeasureOnOff(this.teraMeas.isOnMeasure());
            }

        /// <summary>
        /// Проверяет возможность проведения измерения перед запуском
        /// </summary>
        /// <returns></returns>
        private bool checkMeasurePossibility()
        {
            switch(measure.Type)
            {
                case MEASURE_TYPE.CALIBRATION:
                case MEASURE_TYPE.VERIFICATION:
                    if (this.measure.EtalonMap == null)
                    {
                        string m = (measure.Type == MEASURE_TYPE.CALIBRATION) ? "калибровку" : "поверку";
                        MessageBox.Show("Чтобы произвести " + m + " необходимо выбрать карту эталонов", "Не выбрана карта эталонов", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    return true;
                case MEASURE_TYPE.HAND:
                    if (String.IsNullOrWhiteSpace(MeasureTitle.Text))
                    {
                        MessageBox.Show("Введите " + measureIdLabel.Text.ToLower() + " чтобы начать измерение", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else return true;
                default:
                    return true;
            }
        }

        public void updateResultField() //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateResultFieldDelegate(updateResultField));
                return;
            }
            else
            {
                MeasureResultCollection resultList = this.measure.CurrentCollection;
                refreshResultsPage();
                this.cycleCounterLbl.Text = String.Format("{0}  Цикл {1}", resultList.Name, this.measure.CycleNumber);
                //Отрисовка для статистических испытаний
                if (measure.IsStatistic)  
                {
                    this.statMeasNumbOfLbl.Text = String.Format("измерено {0} из {1}", measure.StatCycleNumber, measure.AveragingTimes);
                    if (resultList.Count > 0)
                    {
                        MeasureResultTera result = resultList.Last() as MeasureResultTera;
                        this.midStatMeasValLbl.Text = String.Format("промежуточное значение: {0}", isDegreeViewCheckBox.Checked ? absoluteResultView(result.BringingResult) : deegreeResultView(result.BringingResult));
                        if (measure.StatCycleNumber == measure.AveragingTimes)
                        {
                            double avVal = resultList.AverageBringing();
                            if (isDegreeViewCheckBox.Checked)
                            {
                                this.updateResultFieldText(deegreeResultView(avVal));
                            }
                            else
                            {
                                this.updateResultFieldText(deegreeResultView(avVal));
                            }
                                
                        }else
                        {
                            this.measureResultLbl.Text = "подождите...";
                        }
                    }
                }else
                //для обычных испытаний
                {
                    if (resultList.Count > 0)
                    {
                        MeasureResultTera result = resultList.Last() as MeasureResultTera;
                        if (measure.StatCycleNumber == this.measure.AveragingTimes)
                        {
                            if (this.measure.Type == MEASURE_TYPE.CALIBRATION || this.measure.Type == MEASURE_TYPE.VERIFICATION)
                            {
                               this.updateResultFieldText(absoluteResultView(result.BringingResult)); //absoluteResultView(result);
                               this.normaLbl.Text = (this.measure.NormaValue > 0) ? "эталон: " + absoluteResultView((double)this.measure.NormaValue / 1000) + " " + result.DeviationPercent.ToString() + "%" : "";
                            }
                            else
                            {
                                if (isDegreeViewCheckBox.Checked)
                                {
                                    this.updateResultFieldText(deegreeResultView(result.BringingResult)); //absoluteResultView(result);
                                    this.normaLbl.Text = (this.measure.NormaValue > 0) ? "норма: " + deegreeResultView((double)this.measure.NormaValue / 1000) : "";
                                }
                                else
                                {
                                    this.updateResultFieldText(absoluteResultView(result.BringingResult)); //absoluteResultView(result);
                                    this.normaLbl.Text = (this.measure.NormaValue > 0) ? "норма: " + absoluteResultView((double)this.measure.NormaValue / 1000) : "";
                                }
                            }

                           
                        }
                        else
                        {
                            this.measureResultLbl.Text = "подождите...";
                        }
                    }
                }
            }
        }

        public void updateResultFieldText(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateResultFieldTextDelegate(updateResultFieldText), new object[] { text });
                return;
            }
            else
            {
                this.measureResultLbl.Text = text;
            }
        }

        public void switchFieldsMeasureOnOff(bool flag) //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new switchFieldsMeasureOnOffDelegate(switchFieldsMeasureOnOff), new object[] { flag });
                return;
            }
            else
            {
                if (flag)
                {
                    this.teraDevice.ClosePort();
                }
                else
                {
                    measure.Start();
                }
            }
        }


        /// <summary>
        /// Обновляем поля статистических испытаний
        /// </summary>
        /// <param name="statMeasInfo"></param>
        public void updateStatMeasInfo(string[] statMeasInfo)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateStatMeasInfoDelegate(updateStatMeasInfo), new object[] { statMeasInfo });
                return;
            }
            else
            {
                this.statMeasNumbOfLbl.Text = String.Format("измерение {0}", statMeasInfo[0]);
                this.midStatMeasValLbl.Text = String.Format("промежуточное значение: {0}", statMeasInfo[1]);
            }
        }

        public void updateServiceField(string serviceInfo) //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateServiceFieldDelegate(updateServiceField), new object[] { serviceInfo });
                return;
            }
            else
            {
                //this.serviceParameters.Text = serviceInfo;
            }
        }

        public void updateCycleNumberField(string cycleNumb) //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateCycleNumberFieldDelegate(updateCycleNumberField), new object[] { cycleNumb });
                return;
            }
            else
            {
                this.cycleCounterLbl.Text = "Цикл: " + cycleNumb;

            }
        }

        public void updateCoeffField(double coeff) //Для обновления поля коэффициента коррекции
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateCorrCoeffFieldDelegate(updateCoeffField), new object[] { coeff });
                return;
            }
            else
            {
                rangeCoeffTextBox.Text = ((float)coeff).ToString();
                corrCoeffLbl.Text = BitConverter.GetBytes((float)coeff).Length.ToString();
            }
        }

        public void updateMeasureStatus(MEASURE_STATUS status) //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateMeasureStatusDelegate(updateMeasureStatus), new object[] { status });
                return;
            }
            else
            {
                bool isActive = status != MEASURE_STATUS.NOT_STARTED && status != MEASURE_STATUS.FINISHED && status != MEASURE_STATUS.STOPED;
                this.measureStatus.Text = TeraMeasure.StatusString(status);
                this.startHandMeasureBut.Enabled = !(status == MEASURE_STATUS.DISCHARGE);
                this.measureIsActive = isActive;
                this.measureSettingsGroup.Enabled = !isActive;
                startHandMeasureBut.Text = !isActive ? "ПУСК ИЗМЕРЕНИЯ" : "ОСТАНОВИТЬ ИЗМЕРЕНИЯ";
            }
        }

        public void RefreshMeasureTimer(int seconds) //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new refreshMeasureTimerDelegate(RefreshMeasureTimer), new object[] { seconds });
                return;
            }
            else
            {
                this.measTimeLbl.Text = ServiceFunctions.TimerTime(seconds);
            }
        }


        public string absoluteResultView(double r)
        {
            return MeasureResultTera.AbsResultViewWithMeasure(r) + getBringingName();
        }
        private string deegreeResultView(double r)
        {
            return MeasureResultTera.DegResultViewWithMeasure(r) + getBringingName();
        }
        private string getBringingName()
        {
            switch (measure.BringingTypeId)
            {
                case "2":
                    return "∙" + bringingLengthMeasCb.Text;
                case "3":
                    return "∙м";
                default:
                    return "";
            }
        }

        private void TeraForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.measureIsActive)
            {
                MessageBox.Show(String.Format("{0} находится в процессе измерений!!! \nЧтобы закрыть окно, необходимо завершить измерение.", this.teraDevice.NameWithSerial()), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            
        }

        private void initVerificationPage()
        {
            fillEtalonMapComboBox();
        }

        /// <summary>
        /// Заполняем список эталонов
        /// </summary>
        public void fillEtalonMapComboBox()
        {
            MainForm f = this.MdiParent as MainForm;
            List<TeraEtalonMap> maps = new List<TeraEtalonMap>();
            teraEtalonMapComboBox.Items.Clear();
            foreach (TeraEtalonMap m in f.TeraEtalonMaps) { teraEtalonMapComboBox.Items.Add(m.AlterName); } 
            if (teraEtalonMapComboBox.Items.Count > 0)
            {
                teraEtalonMapComboBox.Enabled = true;
                if (String.IsNullOrWhiteSpace(curEtalonMapId))
                {
                    teraEtalonMapComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                curEtalonMapId = String.Empty;
                teraEtalonMapComboBox.Items.Add("Карты эталонов отсутствуют");
                teraEtalonMapComboBox.Refresh();
                teraEtalonMapComboBox.SelectedIndex = 0;
                teraEtalonMapComboBox.Enabled = false;
            }


            
        }

        private void temperatureField_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            this.measure.Temperature = Convert.ToInt16(n.Value);
        }

        private void materialTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            this.measure.MaterialId = cb.SelectedValue.ToString();
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            switch(cb.SelectedIndex)
            {
                case 0:
                    this.measure = new TeraMeasure(teraDevice, MEASURE_TYPE.HAND);
                    verificationCalibrationPanel.Visible = false;
                    handMeasurePanel.Visible = true;
                    measureSettingsGroup.Enabled = true;
                    averagingTimes.Enabled = cycleTimes.Enabled = polarizationDelay.Enabled = isCyclicMeasure.Enabled = dischargeDelay.Enabled = voltageComboBox.Enabled = normaField.Enabled = true;
                    initPage();
                    break;
                case 1:
                    this.measure = new TeraMeasure(teraDevice, MEASURE_TYPE.VERIFICATION);
                    verificationCalibrationPanel.Visible = true;
                    handMeasurePanel.Visible = false;
                    isCyclicMeasure.Enabled = dischargeDelay.Enabled = voltageComboBox.Enabled = normaField.Enabled = false;
                    cycleTimes.Enabled = polarizationDelay.Enabled = true;
                    cycleTimes.Value = 10;
                    isCyclicMeasure.Checked = false;
                    handMeasurePanel.Visible = false;
                    //measureSettingsGroup.Enabled = false;
                    fillEtalonMapComboBox();
                    break;
                case 2:
                    this.measure = new TeraMeasure(teraDevice, MEASURE_TYPE.CALIBRATION);
                    verificationCalibrationPanel.Visible = true;
                    //measureSettingsGroup.Enabled = false;
                    isCyclicMeasure.Enabled = averagingTimes.Enabled = isCyclicMeasure.Checked = dischargeDelay.Enabled = voltageComboBox.Enabled = normaField.Enabled = false;
                    setCorrField(autoCorrCb.Checked);
                    averagingTimes.Value = 1;
                    voltageComboBox.SelectedText = "10";
                    handMeasurePanel.Visible = false;
                    fillEtalonMapComboBox();
                    break;
            }
        }

        private void refreshResultsPage()
        {
            foreach(MeasureResultCollection c in measure.ResultCollectionsList)
            {
                if (!measResultsListComboBox.Items.Contains(c.Name))
                {
                    measResultsListComboBox.Items.Add(c.Name);
                }
            }
            if (measResultsListComboBox.SelectedIndex == -1 && measResultsListComboBox.Items.Count > 0) measResultsListComboBox.SelectedIndex = 0;
            if (measResultsListComboBox.SelectedIndex == measResultsListComboBox.Items.Count-1)
            {
                if (measureResultDataGridView1.Rows.Count != measure.ResultCollectionsList.Last().Count)
                {
                    MeasureResultCollection col = measure.ResultCollectionsList.Last();
                    int dif =col.Count - measureResultDataGridView1.Rows.Count;
                    for(int i = dif; i > 0; i--)
                    {
                        MeasureResult mr = col.ResultsList[col.Count - i];
                        addRowToDataGrid(mr);
                    }
                }
            }
        }


        private void measResultsListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            MeasureResultCollection col = this.measure.ResultCollectionsList[cb.SelectedIndex];
            measureResultDataGridView1.Rows.Clear();
            foreach (MeasureResult r in col.ResultsList)
            {
                addRowToDataGrid(r);
            }
        }

        private void addRowToDataGrid(MeasureResult r)
        {
            MeasureResultTera mr = r as MeasureResultTera;
            int num = measureResultDataGridView1.Rows.Add(1);
            measureResultDataGridView1.Rows[num].Cells["cycle_number"].Value = mr.CycleNumber;
            measureResultDataGridView1.Rows[num].Cells["stat_measure_number"].Value = mr.StatCycleNumber;
            measureResultDataGridView1.Rows[num].Cells["voltage"].Value = mr.Voltage;
            measureResultDataGridView1.Rows[num].Cells["result"].Value = mr.BringingResult;
        }

        private void teraEtalonMapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == -1 || (this.mainForm.TeraEtalonMaps.Length == 0)) return;
            this.measure.EtalonMap = this.mainForm.TeraEtalonMaps[cb.SelectedIndex];
            comboBoxResistance.Items.Clear();
            if (this.measure.Type == MEASURE_TYPE.CALIBRATION)
            {
                int range = 0;
                foreach(int i in this.measure.EtalonMap.CalibrationEtalonNumbers)
                {
                    range++;
                    comboBoxResistance.Items.Add(String.Format("{0} Диапазон ({1})", range, MeasureResultTera.AbsResultViewWithMeasure((double)this.measure.EtalonMap.Etalons[i]/1000)));
                }
            }else
            {
                foreach (int i in this.measure.EtalonMap.Etalons)
                {
                    comboBoxResistance.Items.Add(MeasureResultTera.AbsResultViewWithMeasure((double)i /(double)1000));
                }
            }
            comboBoxResistance.SelectedIndex = 0;
        }

        private void comboBoxResistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            measure.EtalonId = cb.SelectedIndex;
            if (this.measure.Type == MEASURE_TYPE.CALIBRATION)
            {
                rangeCoeffTextBox.Text = this.teraDevice.rangeCoeffs[cb.SelectedIndex].ToString();
            }
        }

        private void autoCorrCb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            bool f = cb.Checked;
            setCorrField(f);
        }

        private void setCorrField(bool f)
        {
            cycleTimes.Enabled = polarizationDelay.Enabled = f;
            //isCyclicMeasure.Enabled = !f;
            rangeCoeffTextBox.Enabled = !f;
            isCyclicMeasure.Checked = !f;
            polarizationDelay.Value = !f ? 0 : 3;
            cycleTimes.Value = f ? 10 : 1;
            averagingTimes.Value = 1;

        }

        private void rangeCoeffTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            float c;
            try
            {
                c = float.Parse(tb.Text);
            }catch(Exception)
            {
                c= 1;
            }
            measure.RangeCoeff = c;
        }
    }
}