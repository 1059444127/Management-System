using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace HeNanProvinceSI.Diagnosis
{
    /// <summary>
    /// [��������: ������ҽ�����¼��]<br></br>
    /// [�� �� ��: ��]<br></br>
    /// [����ʱ��: 2009-2-18]<br></br>
    /// �޸ļ�¼
    /// �޸���=''
    ///	�޸�ʱ��=''
    ///	�޸�Ŀ��=''
    ///	�޸�����=''
    /// </summary>
    public partial class ucSIDiagnosis : Form
    {
        public ucSIDiagnosis()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// ����ҵ���
        /// </summary>
        LocalManager localManager = new LocalManager();

        /// <summary>
        /// ����סԺҵ���
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.RADT patientManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// ��ǰ����ʵ��
        /// </summary>
        Neusoft.HISFC.Models.RADT.PatientInfo patient;

        /// <summary>
        /// ���ʵ��
        /// </summary>
        Object.ExtendProperty diagProperty;

        /// <summary>
        /// ����
        /// </summary>
        Object.ICDMedicare mainDiag;

        /// <summary>
        /// ʶ���� 
        /// </summary>
        Object.ICDMedicare regDiag;

        /// <summary>
        /// ������1
        /// </summary>
        Object.ICDMedicare operDiag1;

        /// <summary>
        /// ������2
        /// </summary>
        Object.ICDMedicare operDiag2;

        /// <summary>
        /// ������3
        /// </summary>
        Object.ICDMedicare operDiag3;

        /// <summary>
        /// ���ڸ�ʽ
        /// </summary>
        string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// ҽ����������
        /// </summary>
        Neusoft.FrameWork.Public.ObjectHelper medicareHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        #endregion

        #region ����

        /// <summary>
        /// �������
        /// </summary>
        private void SaveDiag()
        {
            if (this.ValidValue() == false)
            {
                return;
            }

            if (this.GetValue() == false)
            {
                return;
            }

            if (this.localManager.UpdateInpatientOutDiagnoiseInnfo(this.diagProperty, this.patient) <= 0)
            {
                MessageBox.Show("����������ݳ���\n" + this.localManager.Err);
                return;
            }

            MessageBox.Show("�������ݳɹ���");
        }

        /// <summary>
        /// ��ȡ��װ���
        /// </summary>
        /// <returns></returns>
        private bool GetValue()
        {
            this.diagProperty = new HeNanProvinceSI.Object.ExtendProperty();

            if (this.operDiag1 != null)
            {
                diagProperty.OperatorCode1 = this.operDiag1.ID;
            }
            
            if (this.operDiag2 != null)
            {
                diagProperty.OperatorCode2 = this.operDiag2.ID;
            }
            
            if (this.operDiag3 != null)
            {
                diagProperty.OperatorCode3 = this.operDiag3.ID;
            }
            
            if (this.mainDiag != null)
            {
                diagProperty.MainDiagnoseCode = this.mainDiag.ID;
                diagProperty.MainDiagnoseName = this.mainDiag.Name;
            }

            if (this.regDiag != null)
            {
                diagProperty.PrimaryDiagnoseCode = this.regDiag.ID;
                diagProperty.PrimaryDiagnoseName = this.regDiag.Name;
            }

            return true;
        }

        /// <summary>
        /// ��֤��ȷ��
        /// </summary>
        /// <returns></returns>
        private bool ValidValue()
        {
            if (this.patient == null)
            {
                return false;
            }

            //����
            this.mainDiag = this.cmbMainDiag.SelectedItem as Object.ICDMedicare;
            //ʶ���� 
            this.regDiag = this.cmbRecognitionDiag.SelectedItem as Object.ICDMedicare;
            //������
            this.operDiag1 = this.cmbOperation1Diag.SelectedItem as Object.ICDMedicare;
            this.operDiag2 = this.cmbOperation2Diag.SelectedItem as Object.ICDMedicare;
            this.operDiag3 = this.cmbOperation3Diag.SelectedItem as Object.ICDMedicare;

            if (mainDiag == null)
            {
                MessageBox.Show("������ϱ���¼�룡");
                this.cmbMainDiag.Focus();
                return false;
            }

            if (this.patient.SIMainInfo.MedicalType.ID == "24"
                || this.patient.SIMainInfo.MedicalType.ID == "25"
                || this.patient.SIMainInfo.MedicalType.ID == "42")
            {
                if (regDiag == null)
                {
                    MessageBox.Show("�ز����Ҳ��������������ֱ���¼��ʶ����!");
                    this.cmbRecognitionDiag.Focus();
                    return false;
                }
            }

            switch (this.patient.SIMainInfo.MedicalType.ID)
            {
                case "21":
                case "22":
                case "24":
                case "29":
                    if (mainDiag.UseArea == "0" || mainDiag.UseArea == "4")
                    {
                        if (mainDiag.UseArea == "4")
                        {
                            if (mainDiag.ID != "10560")
                            {
                                MessageBox.Show("��ͨסԺ��ת��ҽԺ������סԺ������ҽ�ƻ�������סԺ������¼������Ŀ¼�ϡ���Χ���⡱Ϊ'4'�����ʱʶ��������ǡ�10560����");
                                this.cmbMainDiag.Focus();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("��ͨסԺ��ת��ҽԺ������סԺ������ҽ�ƻ�������סԺ�����������������¼������Ŀ¼�ϡ���Χ���⡱Ϊ��0������ϣ�");
                        this.cmbMainDiag.Focus();
                        return false;
                    }

                    if (!(mainDiag.DisKind == "1" || mainDiag.DisKind == "2"))
                    {
                        MessageBox.Show("��ͨסԺ��ת��ҽԺ������סԺ������ҽ�ƻ�������סԺ�������������¼�����������Ϊ��I�࡯����II�࡯����ϣ�");
                        this.cmbMainDiag.Focus();
                        return false;
                    }
                    break;
                case "25":
                    if (regDiag == null)
                    {
                        MessageBox.Show("ҽ�����Ϊ����ͥ������ʱ��ʶ�������¼��!");
                        this.cmbRecognitionDiag.Focus();
                        return false;
                    }
                    #region {28E32DE0-2126-484d-85B7-F838A736D286}  zjl
                    //if (regDiag.UseArea == "6")
                    if (!(regDiag.UseArea == "6"))
                    #endregion
                    {
                        MessageBox.Show("ҽ�����Ϊ����ͥ������ʱ��ʶ�������Ϊ��ͥ����ר����!");
                        this.cmbRecognitionDiag.Focus();
                        return false;
                    }
                    break;
                case "42":
                case "43":
                case "45":
                    if (regDiag == null)
                    {
                        MessageBox.Show("ҽ�����Ϊ������סԺ������סԺ������ת��סԺ��ʱ����� ʶ���� ����¼��!");
                        this.cmbRecognitionDiag.Focus();
                        return false;
                    }
                    #region {28E32DE0-2126-484d-85B7-F838A736D286} zjl
                    //if (regDiag.UseArea == "5")
                    if (!(regDiag.UseArea == "5"))
                    #endregion
                    {
                        MessageBox.Show("ҽ�����Ϊ������סԺ������סԺ������ת��סԺ��ʱ��ʶ�������Ϊ����ר����!");
                        this.cmbRecognitionDiag.Focus();
                        return false;
                    }
                    if (!(mainDiag.UseArea == "3" || mainDiag.UseArea == "4"))
                    {
                        MessageBox.Show("ҽ�����Ϊ������סԺ������סԺ������ת��סԺ��ʱ���������¼�롰��Χ���⡱Ϊ��3��4�������!");
                        this.cmbMainDiag.Focus();
                        return false;
                    }
                    break;
            }

            return true;

        }

        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        private void ShowPatientInfo()
        {
            if (this.patient != null)
            {
                //����
                this.txtName.Text = this.patient.Name;

                //�Ա�
                this.txtSex.Text = this.patient.Sex.Name;

                //��������
                this.txtBrithday.Text = this.patient.Birthday.ToString(this.DateTimeFormat);

                //����
                this.txtDept.Text = this.patient.PVisit.PatientLocation.Dept.Name;

                //��Ժʱ��
                this.txtInTime.Text = this.patient.PVisit.InTime.ToString(this.DateTimeFormat);

                //��ͬ��λ
                this.txtPact.Text = this.patient.Pact.Name;

                //ҽ�����
                this.txtMedcareType.Text = this.GetInMedicalTypeName(this.patient.SIMainInfo.MedicalType.ID);

                //����
                if (string.IsNullOrEmpty(this.patient.SIMainInfo.OutDiagnose.ID) == false)
                {
                    this.cmbMainDiag.Tag = this.patient.SIMainInfo.OutDiagnose.ID;
                    if (this.cmbMainDiag.SelectedIndex == -1)
                    {
                        this.cmbMainDiag.Tag = null;
                    }
                }

                Object.ExtendProperty diagDroperty = Process.ExtendProperty<Object.ExtendProperty>(Process.EXTEND_PROPERTY_KEY, this.patient.SIMainInfo.ExtendProperty);

                //ʶ����
                if (string.IsNullOrEmpty(diagDroperty.PrimaryDiagnoseCode) == false)
                {
                    this.cmbRecognitionDiag.Tag = diagDroperty.PrimaryDiagnoseCode;
                    if (this.cmbRecognitionDiag.SelectedIndex == -1)
                    {
                        this.cmbRecognitionDiag.Tag = null;
                    }
                }

                //����1
                if (string.IsNullOrEmpty(diagDroperty.OperatorCode1) == false)
                {
                    this.cmbOperation1Diag.Tag = diagDroperty.OperatorCode1;
                    if (this.cmbOperation1Diag.SelectedIndex == -1)
                    {
                        this.cmbOperation1Diag.Tag = null;
                    }
                }

                //����2
                if (string.IsNullOrEmpty(diagDroperty.OperatorCode2) == false)
                {
                    this.cmbOperation2Diag.Tag = diagDroperty.OperatorCode2;
                    if (this.cmbOperation2Diag.SelectedIndex == -1)
                    {
                        this.cmbOperation2Diag.Tag = null;
                    }
                }

                //����3
                if (string.IsNullOrEmpty(diagDroperty.OperatorCode3) == false)
                {
                    this.cmbOperation3Diag.Tag = diagDroperty.OperatorCode3;
                    if (this.cmbOperation3Diag.SelectedIndex == -1)
                    {
                        this.cmbOperation3Diag.Tag = null;
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡҽ���������
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetInMedicalTypeName(string p)
        {
            return this.medicareHelper.GetName(p);
        }

        /// <summary>
        /// ��ʼ����������
        /// </summary>
        private void initData()
        {
            EnumMedicalTypeServiceInhos types = new EnumMedicalTypeServiceInhos();
            this.medicareHelper.ArrayObject = EnumMedicalTypeServiceInhos.List();
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        private void LoadDiags()
        {
            ArrayList mainDiag = this.localManager.GetDiagnose();
            if (mainDiag != null)
            {
                this.cmbMainDiag.AddItems(mainDiag);
            }
            ArrayList recogDiag = this.localManager.GetPDiagnose();
            if (recogDiag != null)
            {
                this.cmbRecognitionDiag.AddItems(recogDiag);
            }
            ArrayList operDiag = this.localManager.GetOperate();
            if (operDiag != null)
            {
                this.cmbOperation1Diag.AddItems(operDiag);
                this.cmbOperation2Diag.AddItems(operDiag);
                this.cmbOperation3Diag.AddItems(operDiag);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        private void ClearShow()
        {
            this.txtBrithday.Text = "";
            this.txtDept.Text = "";
            this.txtInTime.Text = "";
            this.txtMedcareType.Text = "";
            this.txtName.Text = "";
            this.txtPact.Text = "";
            this.txtSex.Text = "";
            this.cmbMainDiag.Tag = null;
            this.cmbOperation1Diag.Tag = null;
            this.cmbOperation2Diag.Tag = null;
            this.cmbOperation3Diag.Tag = null;
            this.cmbRecognitionDiag.Tag = null;
        }

        #endregion

        #region �¼�
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btOK_Click(object sender, EventArgs e)
        {
            this.SaveDiag();
        }

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucSIDiagnosis_Load(object sender, EventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڳ�ʼ�����ڣ����Ժ�");
            Application.DoEvents();

            this.initData();
            this.LoadDiags();

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        /// <summary>
        /// סԺ��
        /// </summary>
        private void ucQueryInpatientNo1_myEvent()
        {
            this.ClearShow();

            if (string.IsNullOrEmpty(this.ucQueryInpatientNo1.InpatientNo))
            {
                MessageBox.Show("δ�ҵ����ߣ�");
                return;
            }

            this.patient = null;
            this.patient = this.patientManager.QueryPatientInfoByInpatientNO(this.ucQueryInpatientNo1.InpatientNo);
            if (string.IsNullOrEmpty(this.patient.ID))
            {
                patient = null;
                MessageBox.Show("���һ�����Ϣ����\n" + this.patientManager.Err);
                return;
            }

            this.patient = this.localManager.GetSIPersonInfo(patient.ID, "0");
            if (this.patient == null)
            {
                this.patient = null;
                MessageBox.Show("�˻��߲���ҽ�����ߣ�");
                return;
            }

            if (string.IsNullOrEmpty(this.patient.ID))
            {
                this.patient = null;
                MessageBox.Show("�˻��߲���ҽ�����ߣ�");
                return;
            }

            this.ShowPatientInfo();

            this.cmbMainDiag.Focus();
        }



        private void cmbMainDiag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void neuButton1_Click(object sender, EventArgs e)
        {
            this.cmbMainDiag.Tag = null;
        }

        private void neuButton2_Click(object sender, EventArgs e)
        {
            this.cmbRecognitionDiag.Tag = null;
        }

        private void neuButton3_Click(object sender, EventArgs e)
        {
            this.cmbOperation1Diag.Tag = null;
        }

        private void neuButton4_Click(object sender, EventArgs e)
        {
            this.cmbOperation2Diag.Tag = null;
        }

        private void neuButton5_Click(object sender, EventArgs e)
        {
            this.cmbOperation3Diag.Tag = null;
        }

        #endregion
    }
}
