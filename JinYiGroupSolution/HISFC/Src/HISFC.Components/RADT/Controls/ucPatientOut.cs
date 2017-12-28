using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//{28C63B3A-9C64-4010-891D-46F846EA093D}
using System.Collections;

namespace Neusoft.HISFC.Components.RADT.Controls
{
    /// <summary>
    /// [��������: ��Ժ�Ǽ����]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2006-11-30]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucPatientOut : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucPatientOut()
        {
            InitializeComponent();
        }

        private void ucPatientOut_Load(object sender, EventArgs e)
        {

        }

        #region ����
        Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        Neusoft.HISFC.BizProcess.Integrate.RADT radt = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        Neusoft.HISFC.BizLogic.RADT.InPatient inpatient = new Neusoft.HISFC.BizLogic.RADT.InPatient();
        Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo = null;
        private Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath iClinicPath = null;

        /// <summary>
        /// ����������{28C63B3A-9C64-4010-891D-46F846EA093D}
        /// </summary>
        private Neusoft.FrameWork.Management.ControlParam ctlMgr = new Neusoft.FrameWork.Management.ControlParam();
     
        private bool quitFeeApplyFlag = true;

        /// <summary>
        /// ҩƷҵ���
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmacyIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();

        #endregion

        #region ����
        /// <summary>
        /// ���˷������Ƿ������Ժ�Ǽ�
        /// </summary>
        [Category("�ؼ�����"), Description("�����˷������Ƿ���������Ժ�Ǽ�")]
        public bool QuitFeeApplyFlag
        {
            get
            {
                return quitFeeApplyFlag;
            }
            set
            {
                quitFeeApplyFlag = value;
            }
        }
        #endregion 

        #region ����
        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        private void InitControl()
        {
            try
            {
                this.cmbZg.AddItems(manager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ZG));
                //��Ժ�Ǽǵ�ʱ��Ĭ��Ϊϵͳʱ��
                this.dtOutDate.Value = this.inpatient.GetDateTimeFromSysDateTime();

                this.InitInterface();
               
                
            }
            catch { }

        }


        /// <summary>
        /// ���û�����Ϣ���ؼ�
        /// </summary>
        /// <param name="PatientInfo"></param>
        private void SetPatientInfo(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {

            this.txtPatientNo.Text = patientInfo.PID.PatientNO;		//סԺ��
            this.txtCard.Text = patientInfo.PID.CardNO;	//���￨��
            this.txtPatientNo.Tag = patientInfo.ID;				//סԺ��ˮ��
            this.txtName.Text = patientInfo.Name;						//����
            this.txtSex.Text = patientInfo.Sex.Name;					//�Ա�
            this.txtIndate.Text = patientInfo.PVisit.InTime.ToString();	//��Ժ����
            this.txtDept.Text = patientInfo.PVisit.PatientLocation.Dept.Name;	//��������
            this.txtDept.Tag = patientInfo.PVisit.PatientLocation.Dept.ID;	//���ұ���

            Neusoft.FrameWork.Public.ObjectHelper helper =new Neusoft.FrameWork.Public.ObjectHelper(manager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.PAYKIND));
            this.txtBalKind.Text = helper.GetName(patientInfo.Pact.PayKind.ID);
            

            this.txtBedNo.Text = patientInfo.PVisit.PatientLocation.Bed.ID;	//����
            this.txtFreePay.Text = patientInfo.FT.LeftCost.ToString();		//ʣ��Ԥ����
            this.txtTotcost.Text = patientInfo.FT.TotCost.ToString();		//�ܽ��
            this.cmbZg.Tag = patientInfo.PVisit.ZG.ID;						//ת��
            this.dtOutDate.Value = this.inpatient.GetDateTimeFromSysDateTime();				//��Ժ���� �������޸�Ϊϵͳʱ��

            //��Ժ�Ǽ��޸�ʱ�䴦�� {28C63B3A-9C64-4010-891D-46F846EA093D}

            string rtn = this.ctlMgr.QueryControlerInfo("ZY0002");
            if (rtn == null || rtn == "-1" || rtn == "")
            {
                rtn = "0";
            }
            else
            {
                rtn = "1";
            }

            if (rtn == "1")//
            {
                ArrayList alShiftDataInfo = this.inpatient.QueryPatientShiftInfoNew(this.PatientInfo.ID);

                if (alShiftDataInfo == null)
                {
                    MessageBox.Show("��ȡ������¼��Ϣ����");
                    return;
                }

                bool isExitInfo = false;

                foreach (Neusoft.HISFC.Models.Invalid.CShiftData myCShiftDate in alShiftDataInfo)
                {
                    if (myCShiftDate.ShitType == "BB") //�н����ٻ�
                    {
                        this.dtOutDate.Enabled = true;
                        isExitInfo = true;
                        break;


                    }
                }


                this.dtOutDate.Enabled = isExitInfo;


            }
            else
            {
                this.dtOutDate.Enabled = false;
            }
        }


        /// <summary>
        /// �ӿؼ���ó�Ժ�Ǽ���Ϣ
        /// </summary>
        private void GetOutInfo()
        {
            PatientInfo.PVisit.ZG.ID = this.cmbZg.Tag.ToString();
            PatientInfo.PVisit.ZG.Name = this.cmbZg.Text;
            PatientInfo.PVisit.PreOutTime = this.dtOutDate.Value;
        }


        /// <summary>
        ///����
        /// </summary>
        private void ClearPatintInfo()
        {
            this.cmbZg.Text = "";
            this.cmbZg.Tag = "";
            this.dtOutDate.Value = this.inpatient.GetDateTimeFromSysDateTime();
        }


        /// <summary>
        /// ˢ�»�����Ϣ
        /// </summary>
        /// <param name="inPatientNo"></param>
        public void RefreshList(string inPatientNo)
        {
            try
            {
                PatientInfo = this.inpatient.QueryPatientInfoByInpatientNO(inPatientNo);
                //��������Ѳ��ڱ���,���������
                if (PatientInfo.PVisit.PatientLocation.NurseCell.ID != ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse.ID)
                {
                    MessageBox.Show("�����Ѳ��ڱ�����,��ˢ�µ�ǰ����", "��ʾ");
                    PatientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
                }
            }
            catch { }
            try
            {
                this.SetPatientInfo(PatientInfo);
            }
            catch { }
        }

        string Err = "";
        /// <summary>
        /// ��дУ����
        /// </summary>
        /// <param name="Inpatient_no"></param>
        /// <returns></returns>
        public virtual int Valid(string Inpatient_no)
        {
            return 1;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public virtual int Save()
        {
            if (this.Valid(this.PatientInfo.ID) < 0)
            {
                return -1;
            }
            //������߲��ǵ����Ժ��ʾ
            if (this.dtOutDate.Value.Date < this.PatientInfo.PVisit.InTime.Date)
            {
                MessageBox.Show("��Ժ���ڲ���С����Ժ���ڣ�", "��ʾ");
                return -1;
            }
            else
            {
                if (this.dtOutDate.Value.Date != this.inpatient.GetDateTimeFromSysDateTime().Date)
                {
                    DialogResult dr = MessageBox.Show("�û��ߵĳ�Ժ�����ǣ� " +
                        this.dtOutDate.Value.ToString("yyyy��MM��dd��") + "  �Ƿ������", "��ʾ",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.No)
                    {
                        this.Err = "";
                        return -1;
                    }
                }
            }
            //ȡ�������µ�סԺ������Ϣ
            PatientInfo = this.inpatient.QueryPatientInfoByInpatientNO(this.PatientInfo.ID);
            if (PatientInfo == null)
            {
                this.Err = this.inpatient.Err;
                return -1;
            }
            this.Err = "";
            //��������Ѳ��ڱ���,���������---������ת�ƺ�,���������û�йر�,����ִ������
            if (PatientInfo.PVisit.PatientLocation.NurseCell.ID != ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse.ID)
            {
                this.Err = "�����Ѳ��ڱ�����,��ˢ�µ�ǰ����";
                return -1;
            }

            //���������Ժ״̬�����仯,���������
            if (PatientInfo.PVisit.InState.ID.ToString() != "I")
            {
                this.Err = "������Ϣ�ѷ����仯,��ˢ�µ�ǰ����";
                return -1;
            }

            #region add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}
            if (this.iClinicPath != null)
            {
                if (this.iClinicPath.PatientIsSelectedPath(this.PatientInfo.ID))
                {
                    this.Err = "�û������ٴ�·���У������˳�·��!";
                    return -1;
                }
            }
            #endregion

            #region {6BFCAC25-CC22-48ac-ADDB-76E169375EAB}
            //��ת�顢��Ժʱ��ȸ�ֵ�õ��ӿ��ж�֮ǰ
            //ȡ��Ժ�Ǽ���Ϣ
            this.GetOutInfo();
            #endregion

            #region addby xuewj 2010-10-11 {EFF73DC9-3543-49a4-9751-BC8D95F0BDD3} ֣�󱾵ػ���������ϸ��ʾ

            Neusoft.HISFC.BizProcess.Interface.IPatientOutCheck ipatientOutCheck = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.IPatientOutCheck)) as Neusoft.HISFC.BizProcess.Interface.IPatientOutCheck;
            bool isZZUCheck = false;
            if (ipatientOutCheck != null)//���ػ�ʵ���˽ӿ�
            {
                string err = string.Empty;
                bool bl = ipatientOutCheck.IPatientOutCheck(PatientInfo, ref err);
                if (!bl)
                {
                    //MessageBox.Show(err);
                    return -1;
                }
                isZZUCheck = true;
            }
            else
            {
                Neusoft.HISFC.BizProcess.Interface.IPatientShiftValid obj = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.IPatientShiftValid)) as Neusoft.HISFC.BizProcess.Interface.IPatientShiftValid;
                if (obj != null)
                {
                    string err = string.Empty;
                    bool bl = obj.IsPatientShiftValid(PatientInfo, Neusoft.HISFC.Models.Base.EnumPatientShiftValid.O, ref err);
                    if (!bl)
                    {
                        MessageBox.Show(err);
                        return -1;
                    }
                }

                //{3E83AFA1-C364-4f72-8DFD-1B733CB9379E}
                //���Ӳ�ѯ�����Ƿ���δ��˵���ҩ��¼,Ϊ��Ժ�Ǽ��ж��� Add by ���� 2009.6.10
                int returnValue = this.pharmacyIntegrate.QueryNoConfirmQuitApply(PatientInfo.ID);
                if (returnValue == -1)
                {
                    MessageBox.Show("��ѯ������ҩ������Ϣ����!" + this.pharmacyIntegrate.Err);

                    return -1;
                }
                if (returnValue > 0) //�����뵫��û�к�׼����ҩ��Ϣ
                {
                    //{29F39131-89B4-4128-B4C9-EAB9F07B719F}
                    if (!this.quitFeeApplyFlag)
                    {
                        MessageBox.Show("�û�����δ��˵���ҩ������Ϣ,���ܽ��г�Ժ������");
                        return -1;
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("�û�����δ��˵���ҩ������Ϣ!�Ƿ����?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.No)
                        {
                            return -1;
                        }
                    }
                }
            }
            #endregion
            //{3E83AFA1-C364-4f72-8DFD-1B733CB9379E} �������
            #region {6BFCAC25-CC22-48ac-ADDB-76E169375EAB}
            ////ȡ��Ժ�Ǽ���Ϣ
            //this.GetOutInfo();
            #endregion

            //��Ժ�Ǽ�
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(inpatient.con);
            //t.BeginTransaction();

            radt.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            radt.QuitFeeApplyFlag = quitFeeApplyFlag;
            #region addby xuewj 2010-10-11 {EFF73DC9-3543-49a4-9751-BC8D95F0BDD3}
            int i = 0;
            if (!isZZUCheck)
            {
                i = radt.OutPatient(PatientInfo);
                this.Err = radt.Err;
            }
            else
            {
                // ֮ǰ���жϹ� ����ֻ����λ��Ϣ
                i = radt.OutPatientZZU(PatientInfo);
                Err = radt.Err;
            }
            //int i = radt.OutPatient(PatientInfo);
            //this.Err = radt.Err;
            #endregion
            if( i== -1)��//ʧ��
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return -1;
            }
            else if (i == 0)//ȡ��
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                this.Err = "";
                return 0;
            }

            #region #region add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}
            if (this.iClinicPath != null)
            {
                if (this.iClinicPath.PatientOutByNurse(this.PatientInfo.ID, PatientInfo.PVisit.PreOutTime) == false)
                {
                    this.Err = "�����ٴ�·��ʧ��!";
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    return -1;
                }
            } 
            #endregion

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            //***************��ӡ��Ժ��ҩ��**************

            return 1;
        }
        #endregion

        #region �¼�
        private void button1_Click(object sender, System.EventArgs e)
        {

            //��Ժ�Ǽ��ж��Ƿ������Ժ���{BDE26EF4-91E2-41b1-9E83-A00332249E05}
            if (this.cmbZg.SelectedItem == null)
            {
                MessageBox.Show("��ѡ���Ժ���~~", "��Ժ�Ǽ�", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ((Control)sender).Enabled = false;
                if (this.Save() > 0)//�ɹ�
                {
                    MessageBox.Show(Err);
                    base.OnRefreshTree();
                    ((Control)sender).Enabled = true;
                    return;
                }
                else
                {
                    if (Err != "")
                        MessageBox.Show(Err);
                    ((Control)sender).Enabled = true;
                }
            }
            

        }
        string strInpatientNo;

        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            this.strInpatientNo = (neuObject as Neusoft.FrameWork.Models.NeuObject).ID;
            if (this.strInpatientNo != null || this.strInpatientNo != "")
            {
                try
                {
                    RefreshList(strInpatientNo);
                }
                catch (Exception ex) { this.Err = ex.Message; }

            }
            return 0;
        }
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.InitControl();
            return null;
        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.IPatientShiftValid),
                                              typeof(Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath)// add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}
                };
            }
        }

        #endregion

        #region add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}
        /// <summary>
        /// ��ʼ���ӿ�
        /// </summary>
        private void InitInterface()
        {
            if (this.iClinicPath == null)
            {
                this.iClinicPath = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(
                    typeof(Neusoft.HISFC.Components.RADT.Controls.ucPatientOut),
                    typeof(Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath))
                    as Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath;
            }
        } 
        #endregion
    }
}
