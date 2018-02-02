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

        TeraDevice teraDevice;
        TeraMeasure handMeasure;
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
            initHandMeasurePage();
            initVerificationPage();
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

        private void initHandMeasurePage()
        {
            this.handMeasure = new TeraMeasure(teraDevice, MEASURE_TYPE.HAND);
            fillHandMeasureParameters();
            buildIsolationMaterialArr();
            switchBringingParams();
            getCameraDiametersByCameraId();
            cleanHandMeasInfo();
        }

        private void fillHandMeasureParameters()
        {

            //Заполняем поля значениями по умолчанию
            temperatureField.Value = handMeasure.Temperature;
            voltageComboBox.Text = handMeasure.Voltage.ToString();
            dischargeDelay.Value = handMeasure.DischargeDelay;
            polarizationDelay.Value = handMeasure.PolarizationDelay;
            cycleTimes.Value = handMeasure.CycleTimes;
            averagingTimes.Value = handMeasure.AveragingTimes;
            bringingLengthMeasCb.SelectedIndex = handMeasure.BringingLengthMeasureId;
            materialHeight.Value = handMeasure.MaterialHeight;
            isDegreeViewCheckBox.Checked = handMeasure.IsDegreeViewMode;
            //minTimeToNorm.Value = handMeasure.MinTimeToNorm;
            normaField.Value = handMeasure.NormaValue;
            materialLength.Value = handMeasure.BringingLength;
            foreach (DataRow r in camera_types.Rows)
            {
                if (r["internal_diameter"].ToString() == handMeasure.InternalCamDiam.ToString() && r["external_diameter"].ToString() == handMeasure.ExternalCamDiam.ToString())
                {
                    cameraTypesCB.Text = r["name"].ToString();
                    break;
                }
            }
            if (String.IsNullOrEmpty(cameraTypesCB.Text)) cameraTypesCB.Text = camera_types.Rows[0]["name"].ToString();
            bringingTypeCB.SelectedValue = handMeasure.BringingTypeId;
            materialTypes.SelectedValue = handMeasure.MaterialId;
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
            int v = Convert.ToInt32(this.normaField.Value);
            this.polTime.Text = (v > 0) ? "Достижение, мин" : "Поляризация, мин";
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
                    this.handMeasure.InternalCamDiam = Convert.ToInt16(r["internal_diameter"].ToString());
                    this.handMeasure.ExternalCamDiam = Convert.ToInt16(r["external_diameter"].ToString());
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

        private void cleanHandMeasInfo()
        {
            this.measureStatus.Text = TeraMeasure.StatusString(MEASURE_STATUS.NOT_STARTED);
            this.measTimeLbl.Text = this.normaLbl.Text = this.cycleCounterLbl.Text = this.statMeasNumbOfLbl.Text = this.midStatMeasValLbl.Text = this.measTimeLbl.Text = "";
            this.measureResultLbl.Text = "0.0 МОм";
        }

        private void startHandMeasureBut_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this.teraMeas.isOnMeasure().ToString());

            if (!this.handMeasure.IsStarted)
            {
                this.teraDevice.OpenPort();
                handMeasure.Temperature = (int)temperatureField.Value;
                handMeasure.Voltage = Convert.ToInt32(voltageComboBox.Text);
                handMeasure.DischargeDelay = Convert.ToInt16(dischargeDelay.Value);
                handMeasure.PolarizationDelay = Convert.ToInt32(this.polarizationDelay.Value);
                handMeasure.IsCyclicMeasure = isCyclicMeasure.Checked;
                handMeasure.CycleTimes = Convert.ToInt32(cycleTimes.Value);
                handMeasure.AveragingTimes = Convert.ToInt32(this.averagingTimes.Value);
                handMeasure.BringingTypeId = bringingTypeCB.SelectedValue.ToString();
                handMeasure.MaterialHeight = Convert.ToInt32(this.materialHeight.Value);
                handMeasure.IsDegreeViewMode = isDegreeViewCheckBox.Checked;
               // handMeasure.MinTimeToNorm = Convert.ToInt32(minTimeToNorm.Value);
                handMeasure.NormaValue = Convert.ToInt32(this.normaField.Value);
                handMeasure.MaterialId = materialTypes.SelectedValue.ToString();
                handMeasure.BringingLength = Convert.ToInt16(materialLength.Value);
                handMeasure.BringingLengthMeasureId = this.bringingLengthMeasCb.SelectedIndex;
                this.switchFieldsMeasureOnOff(this.handMeasure.IsStarted);

            }else
                {
                     handMeasure.StopWithStatus(MEASURE_STATUS.STOPED);
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
        public void updateResultField() //Для обновления поля результата из другого потока в котором проходит испытание
        {
            if (InvokeRequired)
            {
                BeginInvoke(new updateResultFieldDelegate(updateResultField));
                return;
            }
            else
            {
                MeasureResultCollection resultList = handMeasure.ResultCollectionsList[this.handMeasure.Number-1];
                this.cycleCounterLbl.Text = String.Format("Измерение {0}  Цикл {1}", this.handMeasure.Number, this.handMeasure.CycleNumber);
                //Отрисовка для статистических испытаний
                if (handMeasure.IsStatistic)  
                {
                    this.statMeasNumbOfLbl.Text = String.Format("измерено {0} из {1}", handMeasure.StatCycleNumber, handMeasure.AveragingTimes);
                    if (resultList.Count > 0)
                    {
                        MeasureResultTera result = resultList.Last() as MeasureResultTera;
                        this.midStatMeasValLbl.Text = String.Format("промежуточное значение: {0}", isDegreeViewCheckBox.Checked ? absoluteResultView(result.BringingResult) : deegreeResultView(result.BringingResult));
                        if (handMeasure.StatCycleNumber == handMeasure.AveragingTimes)
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
                        if (handMeasure.StatCycleNumber == this.handMeasure.AveragingTimes)
                        {
                            if (isDegreeViewCheckBox.Checked)
                            {
                                this.updateResultFieldText(deegreeResultView(result.BringingResult)); //absoluteResultView(result);
                                this.normaLbl.Text = (this.normaField.Value > 0) ? "норма: " + deegreeResultView((double)this.normaField.Value / 1000) : "";
                            }
                            else
                            {
                                this.updateResultFieldText(absoluteResultView(result.BringingResult)); //absoluteResultView(result);
                                this.normaLbl.Text = (this.normaField.Value > 0) ? "норма: " + absoluteResultView((double)this.normaField.Value / 1000) : "";
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
                    handMeasure.Start();
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
            switch (handMeasure.BringingTypeId)
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
            this.handMeasure.Temperature = Convert.ToInt16(n.Value);
        }

        private void materialTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            this.handMeasure.MaterialId = cb.SelectedValue.ToString();
        }
    }
}