using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.RADT;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.BizProcess.Integrate;
using Neusoft.HISFC.BizLogic.Operation;
using Neusoft.HISFC.Models.Operation;
using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: �������뵥]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucApplicationForm : UserControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucApplicationForm()
        {

            InitializeComponent();
            if (!this.DesignMode)
            {
                this.Reset();
                this.Init();
            }

        }

        #region �ֶ�
        private Neusoft.HISFC.BizProcess.Integrate.RADT radtmanager = new RADT();
        private Neusoft.FrameWork.Public.ObjectHelper payKindHelper;
        private Neusoft.HISFC.Models.Operation.OperationAppllication operationApplication = new Neusoft.HISFC.Models.Operation.OperationAppllication();
        private Neusoft.HISFC.BizProcess.Integrate.Operation.OpsDiagnose opsDiagnose = new Neusoft.HISFC.BizProcess.Integrate.Operation.OpsDiagnose();
        private bool isNew = true;     //�Ƿ��½�����
        private Neusoft.HISFC.BizProcess.Interface.Operation.IArrangeNotifyFormPrint arrangeFormPrint;
        private System.Windows.Forms.Control contralActive = new Control();
        private bool dirty = false;
        private Neusoft.HISFC.BizLogic.Operation.OpsTableManage opsMgr = new OpsTableManage();
        private Neusoft.HISFC.Models.Base.Employee var = null;
        private bool checkApplyTime = false;
        private bool checkEmergency = true;
        private bool checkDate = true;
        #endregion

        #region ����

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PatientInfo PatientInfo
        {
            set
            {
                this.Reset();

                if (value == null)
                    return;

                #region ��ֵ
                this.lblName.Text = value.Name;
                this.lblGender.Text = value.Sex.Name;
                //Neusoft.FrameWork.Management.DataBaseManger daMgr = new Neusoft.FrameWork.Management.DataBaseManger();
                //int age = daMgr.GetDateTimeFromSysDateTime().Year - value.Birthday.Year;
                this.lblAge.Text = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(value.Birthday);//age.ToString() + "��";
                this.lblID.Text = value.PID.PatientNO;


                //this.lblType.Text = payKindHelper.GetName(value.Pact.PayKind.ID);
                this.lblType.Text = value.Pact.Name;
                this.lblDept.Text = value.PVisit.PatientLocation.Dept.Name;
                this.lblBed.Text = value.PVisit.PatientLocation.Bed.Name;
                this.lblBalance.Text = value.FT.LeftCost.ToString();

               
                //������
                //�������ԱΪ��������Ա,Ĭ��������Ϊ����Ա���ڿ���
                foreach (Department dept in this.cmbExeDept.alItems)
                {
                    if (dept.ID == Environment.OperatorDeptID)
                    {
                        this.cmbExeDept.Tag = dept.ID;
                        break;
                    }
                }
                //û�и�ֵ,��������Ա������������Ա,Ĭ���б��е�һ��
                if (this.cmbExeDept.Tag == null || this.cmbExeDept.Tag.ToString() == "")
                {
                    if (this.cmbExeDept.Items.Count > 0)
                        this.cmbExeDept.SelectedIndex = 0;
                }
                //����ָ��ʱ����������жϵ����Ƿ�����̨,�����Զ���Ϊ��̨
                Department d = this.cmbExeDept.SelectedItem as Department;

                int num = Environment.OperationManager.GetEnableTableNum(d, value.PVisit.PatientLocation.Dept.ID, this.dtOperDate.Value);
                if (num > 0)
                    this.cmbTableType.SelectedIndex = 0;//��̨
                else
                    this.cmbTableType.SelectedIndex = 1;//��̨
                #endregion

                this.operationApplication.PatientInfo = value;
                this.isNew = true;


            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Neusoft.HISFC.Models.Operation.OperationAppllication OperationApplication
        {
            get
            {
                if (this.operationApplication == null)
                    this.operationApplication = new OperationAppllication();

                return this.operationApplication;
            }
            set
            {
                this.operationApplication = value;


                #region ��ֵ


                PatientInfo p = this.radtmanager.GetPatientInfomation(value.PatientInfo.ID);
                if (p == null)
                {
                    MessageBox.Show("�޴˻�����Ϣ!", "��ʾ");
                    return;
                }
                this.PatientInfo = p;// this.operationApplication.PatientInfo;

                if (value.OperateKind == "1")
                { this.cmbOperKind.SelectedIndex = 0; }//����
                else if (value.OperateKind == "2")
                { this.cmbOperKind.SelectedIndex = 1; }//����
                else
                { this.cmbOperKind.SelectedIndex = 2; }//��Ⱦ

                if (value.DiagnoseAl.Count > 0)//��һ���
                {
                    dirty = true;
                    this.txtDiag.Text = (value.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;//���
                    dirty = false;
                    Neusoft.HISFC.Models.HealthRecord.ICD icd = new Neusoft.HISFC.Models.HealthRecord.ICD();
                    icd.ID = (value.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ID;
                    icd.Name = (value.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;
                    this.txtDiag.Tag = icd;

                    if (value.DiagnoseAl.Count >= 2) //�ڶ����
                    {
                        //dirty = true;
                        this.txtDiag2.Text = (value.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;//���
                        //dirty = false;
                        icd = new Neusoft.HISFC.Models.HealthRecord.ICD();
                        icd.ID = (value.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ID;
                        icd.Name = (value.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;
                        this.txtDiag2.Tag = icd;
                        if (value.DiagnoseAl.Count >= 3) //������� 
                        {
                            dirty = true;
                            this.txtDiag3.Text = (value.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;//���
                            dirty = false;
                            icd = new Neusoft.HISFC.Models.HealthRecord.ICD();
                            icd.ID = (value.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ID;
                            icd.Name = (value.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).Name;
                            this.txtDiag3.Tag = icd;

                        }

                    }
                }
                if (value.OperationInfos.Count > 0) //��һ���� 
                {
                    dirty = true;
                    this.txtOperation.Text = (value.OperationInfos[0] as Neusoft.HISFC.Models.Operation.OperationInfo).OperationItem.Name;
                    dirty = false;
                    this.txtOperation.Tag = (Neusoft.HISFC.Models.Operation.OperationInfo)value.OperationInfos[0];//��������

                    if (value.OperationInfos.Count >= 2) //�ڶ����� 
                    {
                        dirty = true;
                        this.txtOperation2.Text = (value.OperationInfos[1] as Neusoft.HISFC.Models.Operation.OperationInfo).OperationItem.Name;
                        dirty = false;
                        this.txtOperation2.Tag = (Neusoft.HISFC.Models.Operation.OperationInfo)value.OperationInfos[1];//��������
                        if (value.OperationInfos.Count >= 3)//��������
                        {
                            dirty = true;
                            this.txtOperation3.Text = (value.OperationInfos[2] as Neusoft.HISFC.Models.Operation.OperationInfo).OperationItem.Name;
                            dirty = false;
                            this.txtOperation3.Tag = (Neusoft.HISFC.Models.Operation.OperationInfo)value.OperationInfos[2];//��������
                        }
                    }
                }

                //����ʽ
                this.cmbAnae.Tag = value.AnesType.ID;
                value.AnesType.Name = this.cmbAnae.Text;

                ////{B9DDCC10-3380-4212-99E5-BB909643F11B}
                this.cmbAnseWay.Tag = value.AnesWay;
                this.dtOperDate.Value = value.PreDate;//��������
                this.cmbExeDept.Tag = value.ExeDept.ID;//ִ�п���
                this.comDept.Tag = value.OperationDoctor.Dept.ID;

                //if (value.TableType == "1")
                //{ this.cmbTableType.SelectedIndex = 0; }//��̨
                //else if (value.TableType == "2")
                //{ this.cmbTableType.SelectedIndex = 1; }//��̨
                //else
                //{ this.cmbTableType.SelectedIndex = 2; }//��̨

                this.cmbDoctor.Tag = value.OperationDoctor.ID;//����
                foreach (Neusoft.HISFC.Models.Operation.ArrangeRole role in value.RoleAl)
                {
                    if (role.RoleType.ID.ToString() == EnumOperationRole.Helper1.ToString())
                    {
                        this.cmbHelper1.Tag = role.ID;//һ��
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.Helper2.ToString())
                    {
                        this.cmbHelper2.Tag = role.ID;//����
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.Helper3.ToString())
                    {
                        this.cmbHelper3.Tag = role.ID;//����
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper1.ToString()) //donggq
                    {
                        this.txtTmpHelper1.Tag = role.ID;
                        this.txtTmpHelper1.Text = role.Name;
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper2.ToString()) //donggq
                    {
                        this.txtTmpHelper2.Tag = role.ID;
                        this.txtTmpHelper2.Text = role.Name;
                    }

                }

                
                this.cmbInfectType.SelectedIndex = int.Parse(value.BloodNum.ToString());//�Ƿ���������

                //this.cmbOrder.Text = value.BloodUnit;//̨��

                //if (value.IsAccoNurse)
                //    this.cbxNeedQX.Checked = true;//��е��ʿ
                //if (value.IsPrepNurse)
                //    this.cbxNeedXH.Checked = true;//Ѳ�ػ�ʿ
                
                //{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
                this.cmbIncitepe.Tag = value.InciType.ID;

                //�Ƿ�ͬ��ʹ���Է���Ŀ
                this.cmbOwn.SelectedIndex = value.IsHeavy ? 0 : 1;

                //�ϲ�����
                this.txtAnaeNote.Text = value.AneNote;

                //����˵��
                this.rtbApplyNote.Text = value.ApplyNote;

                this.cmbApplyDoct.Tag = value.ApplyDoctor.ID;//����ҽ��
                this.lbApplyDate.Text = value.ApplyDate.ToString("yyyy��MM��dd�� HHʱmm��");

                
                #endregion

                this.operationApplication = value;
                this.isNew = false;//�޸�
            }
        }

        protected new bool DesignMode
        {
            get
            {
                return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");


            }
        }

        /// <summary>
        /// ��������Ŀ
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NeuObject MainOperation
        {
            set
            {
                this.txtOperation.Text = value.Name;
                this.txtOperation.Tag = value.ID;
            }
        }

        /// <summary>
        /// �Ƿ�������������
        /// </summary>
        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }
        
        [Category("�ؼ�����"), Description("��������ȵ�ǰʱ���������")]
        public bool CheckApplyTime
        {
            get
            {
                return checkApplyTime;
            }
            set
            {
                checkApplyTime = value;
            }
        }
        [Category("�ؼ�����"), Description("����ʱ�䳬����ֹʱ�䣬�Ƿ���Ҫ��Ϊ����")]
        public bool CheckEmergency
        {
            get
            {
                return checkEmergency;
            }
            set
            {
                checkEmergency = value;
            }
        }
        [Category("�ؼ�����"), Description("�������ղ���������һ������")]
        public bool CheckDate
        {
            get
            {
                return checkDate;
            }
            set
            {
                checkDate = value;
            }
        }
        #endregion

        #region ����

        /// <summary>
        /// ��ʹ��
        /// </summary>
        private void Init()
        {
            var = (Neusoft.HISFC.Models.Base.Employee)opsMgr.Operator;
            //֧������
            //this.payKindHelper = new Neusoft.FrameWork.Public.ObjectHelper(Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.PAYKIND));

            ArrayList alRet;

            //��������
            alRet = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESTYPE);
            this.cmbAnae.AddItems(alRet);
            this.cmbAnae.IsListOnly = true;

            //�������'������𣨾����ѡ�飬ҽ������ʱ��д��//{B9DDCC10-3380-4212-99E5-BB909643F11B}
            alRet = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESWAY);
            this.cmbAnseWay.AddItems(alRet);
            this.cmbAnseWay.IsListOnly = true;


            //������
            alRet = Environment.IntegrateManager.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.OP);
            this.cmbExeDept.AddItems(alRet);
            this.cmbExeDept.IsListOnly = true;
            if (alRet.Count >= 2)
            {
                //this.cmbExeDept.Text = alRet[0].ToString();
                this.cmbExeDept.SelectedIndex = 1;
            }
            
            //���߿���---���ؿ���
            ArrayList deptList = Environment.IntegrateManager.QueryDeptmentsInHos(true);
            this.comDept.AddItems(deptList); 
            
            //��Ⱦ����
            alRet = Environment.IntegrateManager.GetConstantList("INFECTTYPE");
            this.cmbInfectType.AddItems(alRet);

            //����
            alRet = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D);
            this.cmbDoctor.AddItems(alRet);
            this.cmbDoctor.IsListOnly = true;
            //һ��
            this.cmbHelper1.AddItems(alRet);
            this.cmbHelper1.IsListOnly = true;
            //����          
            this.cmbHelper2.AddItems(alRet);
            this.cmbHelper2.IsListOnly = true;
            //������            
            this.cmbHelper3.AddItems(alRet);
            this.cmbHelper3.IsListOnly = true;
            //����ҽ��
            this.cmbApplyDoct.AddItems(alRet);
            this.cmbApplyDoct.IsListOnly = true;

            //�п�����{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
            alRet = Environment.IntegrateManager.GetConstantList(EnumConstant.INCITYPE);

            this.cmbIncitepe.AddItems(alRet);
            this.cmbIncitepe.IsListOnly = true;

            //����ע������
            Neusoft.HISFC.BizProcess.Integrate.Manager ctlMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            try
            {	//��ѯ�����������ʱ��
                string control = ctlMgr.QueryControlerInfo("optime");

                if (control != "" && control != "-1") this.lbNote.Text = "Ҫ����" + control + "ǰ�����������룬����ʹ�ý�̨��";

                if (this.cmbExeDept.Items.Count > 0)
                {
                    ArrayList list = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("ExeDept");
                    if (list.Count == 2)
                    {
                        this.cmbExeDept.Text = list[0].ToString();
                        this.cmbExeDept.Tag = list[1].ToString();
                    }
                }
            }
            catch { }

            #region ���
            ucDiag1 = new Neusoft.HISFC.Components.Common.Controls.ucDiagnose();
            this.Controls.Add(ucDiag1);
            ucDiag1.Size = new Size(456, 312);
            ucDiag1.SelectItem += new Neusoft.HISFC.Components.Common.Controls.ucDiagnose.MyDelegate(ucDiag1_SelectItem);
            ucDiag1.Init();
            ucDiag1.Visible = false;
            #endregion

            #region ����
            ucOpItem1 = new ucOpItem();
            this.Controls.Add(ucOpItem1);
            ucOpItem1.Size = new Size(518, 338);
            ucOpItem1.SelectItem += new ucOpItem.MyDelegate(ucOpItem1_SelectItem);
            ucOpItem1.Init();
            ucOpItem1.Visible = false;
            #endregion 

        }

        /// <summary>
        /// �������ÿؼ�
        /// </summary>
        private void Reset()
        {
            this.operationApplication = new OperationAppllication();

            this.lblName.Text = "";
            this.lblGender.Text = "";
            this.lblAge.Text = "";
            this.lblID.Text = "";
            this.lblType.Text = "";
            this.lblDept.Text = "";
            this.lblBed.Text = "";
            this.lblBalance.Text = "";

            //�������
            this.cmbOperKind.SelectedIndex = 0;//��ͨ

            //dirty = true;
            this.txtDiag.Text = "";//���
            this.txtDiag.Tag = null;
            this.txtDiag2.Text = "";//���
            this.txtDiag2.Tag = null;
            this.txtDiag3.Text = "";//���
            this.txtDiag3.Tag = null;

            this.txtOperation.Text = "";//��������
            this.txtOperation.Tag = null;
            this.txtOperation2.Text = "";//��������
            this.txtOperation2.Tag = null;
            this.txtOperation3.Text = "";//��������
            this.txtOperation3.Tag = null;
            //dirty = false;

            this.cmbAnae.Text = "";//��������
            this.cmbAnae.Tag = null;
            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            this.cmbAnseWay.Text = "";
            this.cmbAnseWay.Tag = null;

            DateTime dtNow = Environment.OperationManager.GetDateTimeFromSysDateTime();
            this.lbApplyDate.Text = dtNow.ToString("yyyy��MM��dd�� HHʱmm��");//��������

            dtNow = dtNow.Date.AddDays(1).AddHours(9); ;//DateTime.Parse(string.Concat(dtNow.Date.AddDays(1).ToString("yyyy-MM-dd"), " 09:00:00"));
            this.dtOperDate.Value = dtNow;//ԤԼʱ��

            this.cmbExeDept.Text = "";//������
            this.cmbExeDept.Tag = null;

            if (this.cmbExeDept.Items.Count >= 2) 
            {
                this.cmbExeDept.SelectedIndex = 1;
            }


            this.rtbApplyNote.Text = "";//��ע
            this.cbxNeedQX.Checked = false;
            this.cbxNeedXH.Checked = false;

            this.cmbDoctor.Text = "";//������
            this.cmbDoctor.Tag = null;

            this.cmbHelper1.Text = "";//һ��
            this.cmbHelper1.Tag = null;

            this.cmbHelper2.Text = "";//����
            this.cmbHelper2.Tag = null;

            this.cmbHelper3.Text = "";//����
            this.cmbHelper3.Tag = null;

            this.txtTmpHelper1.Text = string.Empty;
            this.txtTmpHelper1.Tag = null;

            this.txtTmpHelper2.Text = string.Empty;
            this.txtTmpHelper2.Tag = null;

            //�п�
            this.cmbIncitepe.Text = "";
            this.cmbIncitepe.Tag = null;

            //��Ⱦ����
            this.cmbInfectType.Text = "";
            this.cmbInfectType.Tag = null;
            

            //̨��
            //this.cmbOrder.Text = "";

            //�ϲ�����
            this.txtAnaeNote.Text = string.Empty;
            this.txtAnaeNote.Tag = null;

            //�Է�
            this.cmbOwn.SelectedIndex = 0;

            //����ҽ��
            this.cmbApplyDoct.Text = "";
            this.cmbApplyDoct.Tag = null;

            foreach (Employee person in this.cmbApplyDoct.alItems)
            {
                if (person.ID == Neusoft.FrameWork.Management.Connection.Operator.ID)
                {
                    this.cmbApplyDoct.Tag = Neusoft.FrameWork.Management.Connection.Operator.ID;
                    break;
                }
            }

            

            this.operationApplication = new Neusoft.HISFC.Models.Operation.OperationAppllication();
            this.isNew = true;

        }

        /// <summary>
        /// ʵ�帳ֵ
        /// </summary>
        /// <returns></returns>
        private int GetValue()
        {
            //��¼��ģ������µ����뵥
            if (this.isNew)
                this.operationApplication.ID = Environment.OperationManager.GetNewOperationNo();

            #region ���
            Neusoft.HISFC.Models.HealthRecord.DiagnoseBase diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();

            diag.OperationNo = this.operationApplication.ID;//�����
            //diag.ICD10=(neusoft.HISFC.Object.Case.ICD10)this.txtDiag.Tag;
            diag.ID = (this.txtDiag.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).ID;
            diag.Name = (this.txtDiag.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
            diag.Patient = this.operationApplication.PatientInfo.Clone();//.PatientInfo.Patient.Clone();
            diag.DiagType.ID = "7";//�������
            diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
            diag.DiagDate = opsMgr.GetDateTimeFromSysDateTime();//���ʱ��
            diag.Doctor.ID = var.ID;//���ҽ��
            diag.Doctor.Name = var.Name;//���ҽ��
            diag.Dept.ID = var.Dept.ID;//��Ͽ���
            diag.IsValid = true;//�Ƿ���Ч
            diag.IsMain = true;//�����

            if (operationApplication.DiagnoseAl.Count == 0)
                diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
            else
                diag.HappenNo = (operationApplication.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;

            operationApplication.DiagnoseAl.Clear();
            operationApplication.DiagnoseAl.Add(diag);
            #region �ڶ����
            if (txtDiag2.Tag != null)
            {
                diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();
                diag.OperationNo = this.operationApplication.ID;//�����
                //diag.ICD10=(neusoft.HISFC.Object.Case.ICD10)this.txtDiag.Tag;
                diag.ID = (this.txtDiag2.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).ID;
                diag.Name = (this.txtDiag2.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                diag.Patient = this.operationApplication.PatientInfo.Clone();
                diag.DiagType.ID = "7";//�������
                diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
                diag.DiagDate = opsMgr.GetDateTimeFromSysDateTime();//���ʱ��
                diag.Doctor.ID = var.ID;//���ҽ��
                diag.Doctor.Name = var.Name;//���ҽ��
                diag.Dept.ID = var.Dept.ID;//��Ͽ���
                diag.IsValid = true;//�Ƿ���Ч
                diag.IsMain = false;//�����

                if (operationApplication.DiagnoseAl.Count == 1)
                    diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
                else
                    diag.HappenNo = (operationApplication.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;
                operationApplication.DiagnoseAl.Add(diag);
            }
            #endregion
            #region �������
            if (txtDiag3.Tag != null)
            {
                diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();
                diag.OperationNo = this.operationApplication.ID;//�����
                //diag.ICD10=(neusoft.HISFC.Object.Case.ICD10)this.txtDiag.Tag;
                diag.ID = (this.txtDiag3.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).ID;
                diag.Name = (this.txtDiag3.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                diag.Patient = this.operationApplication.PatientInfo.Clone();
                diag.DiagType.ID = "7";//�������
                diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
                diag.DiagDate = opsMgr.GetDateTimeFromSysDateTime();//���ʱ��
                diag.Doctor.ID = var.ID;//���ҽ��
                diag.Doctor.Name = var.Name;//���ҽ��
                diag.Dept.ID = var.Dept.ID;//��Ͽ���
                diag.IsValid = true;//�Ƿ���Ч
                diag.IsMain = false;//�����

                if (operationApplication.DiagnoseAl.Count == 2)
                    diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
                else
                    diag.HappenNo = (operationApplication.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;
                operationApplication.DiagnoseAl.Add(diag);
            }
            #endregion
            #endregion

            #region ������Ŀ

            this.operationApplication.OperationInfos.Clear();
            //-----------------------------------------------------------------------------
            if (this.txtOperation.Text.Trim() != "" && this.txtOperation.Tag != null)
            {
                if (this.txtOperation.Tag.GetType() == typeof(Neusoft.HISFC.Models.Operation.OperationInfo))
                {
                    Neusoft.HISFC.Models.Operation.OperationInfo obj = (Neusoft.HISFC.Models.Operation.OperationInfo)txtOperation.Tag;
                    operationApplication.OperationInfos.Add(obj);
                }
                else
                {
                    Neusoft.HISFC.Models.Operation.OperationInfo opItem = new Neusoft.HISFC.Models.Operation.OperationInfo();
                    opItem.OperationItem = (Neusoft.HISFC.Models.Base.Item)this.txtOperation.Tag;//������Ŀ
                    opItem.FeeRate = 1m;//����
                    opItem.Qty = 1;//����
                    opItem.StockUnit = (this.txtOperation.Tag as Neusoft.HISFC.Models.Base.Item).PriceUnit;//��λ
                    opItem.OperateType.ID = (this.txtOperation.Tag as Neusoft.HISFC.Models.Fee.Item.Undrug).OperationScale.ID;
                    opItem.IsValid = true;
                    opItem.IsMainFlag = true; 
                    operationApplication.OperationInfos.Add(opItem);
                    operationApplication.OperationType.ID = opItem.OperateType.ID;
                }
            } 
            //-----------------------------------------------------------------------------
            //this.SetOperationItem(this.txtOperation.Tag);

            if (this.txtOperation2.Text.Trim() != "" && this.txtOperation2.Tag != null)
            {
                this.SetOperationItem(this.txtOperation2.Tag);
            }
            if (this.txtOperation3.Text.Trim() != "" && this.txtOperation3.Tag != null)
            {
                this.SetOperationItem(this.txtOperation3.Tag);
            }
            
            #endregion

            //�������--{B9DDCC10-3380-4212-99E5-BB909643F11B}
            this.operationApplication.AnesWay = this.cmbAnseWay.Tag.ToString();

            //����ʽ
            this.operationApplication.AnesType.Name = this.cmbAnae.Text;
            this.operationApplication.AnesType.ID = this.cmbAnae.Tag.ToString();

            #region ����
            Neusoft.HISFC.Models.Operation.ArrangeRole role;
            role = new Neusoft.HISFC.Models.Operation.ArrangeRole();
            role.OperationNo = this.operationApplication.ID;                   //�����
            role.ID = this.cmbDoctor.Tag.ToString();                             //��Ա����
            role.Name = this.cmbDoctor.Text;
            role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.Operator;    //��ɫ����
            role.ForeFlag = "0";                                                        //��ǰ¼��
            this.operationApplication.RoleAl.Clear();
            this.operationApplication.RoleAl.Add(role);
            this.operationApplication.OperationDoctor.ID = role.ID;
            this.operationApplication.OperationDoctor.Name = role.Name;
            #endregion

            #region һ��
            role = new Neusoft.HISFC.Models.Operation.ArrangeRole();
            role.OperationNo = this.operationApplication.ID;//�����
            role.ID = this.cmbHelper1.Tag.ToString();//��Ա����
            role.Name = this.cmbHelper1.Text;
            role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.Helper1;//��ɫ����
            role.ForeFlag = "0";//��ǰ¼��
            this.operationApplication.RoleAl.Add(role);

            Neusoft.FrameWork.Models.NeuObject person;
            person = new Neusoft.FrameWork.Models.NeuObject();

            person.ID = role.ID;
            person.Name = role.Name;
            this.operationApplication.HelperAl.Clear();
            this.operationApplication.HelperAl.Add(person);
            #endregion

            #region ����
            if (this.cmbHelper2.Tag != null && this.cmbHelper2.Tag.ToString() != "")
            {
                role = new Neusoft.HISFC.Models.Operation.ArrangeRole();
                role.OperationNo = this.operationApplication.ID;//�����
                role.ID = this.cmbHelper2.Tag.ToString();//��Ա����
                role.Name = this.cmbHelper2.Text;
                role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.Helper2;//��ɫ����
                role.ForeFlag = "0";//��ǰ¼��
                this.operationApplication.RoleAl.Add(role);

                person = new Neusoft.FrameWork.Models.NeuObject();

                person.ID = role.ID;
                person.Name = role.Name;
                this.operationApplication.HelperAl.Clear();
                this.operationApplication.HelperAl.Add(person);
            }
            #endregion

            #region ����
            if (this.cmbHelper3.Tag != null && this.cmbHelper3.Tag.ToString() != "")
            {
                role = new Neusoft.HISFC.Models.Operation.ArrangeRole();
                role.OperationNo = this.operationApplication.ID;//�����
                role.ID = this.cmbHelper3.Tag.ToString();//��Ա����
                role.Name = this.cmbHelper3.Text;
                role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.Helper3;//��ɫ����
                role.ForeFlag = "0";//��ǰ¼��
                this.operationApplication.RoleAl.Add(role);

                person = new Neusoft.FrameWork.Models.NeuObject();

                person.ID = role.ID;
                person.Name = role.Name;
                this.operationApplication.HelperAl.Clear();
                this.operationApplication.HelperAl.Add(person);
            }

            #endregion

            #region ��ʱ����1

            if (!string.IsNullOrEmpty(this.txtTmpHelper1.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationApplication.ID;
                role.ID = "777777";
                role.Name = this.txtTmpHelper1.Text;
                role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.TmpHelper1;
                role.ForeFlag = "0";
                this.operationApplication.RoleAl.Add(role);

                person = new Neusoft.FrameWork.Models.NeuObject();

                person.ID = role.ID;
                person.Name = role.Name;
                this.operationApplication.HelperAl.Clear();
                this.operationApplication.HelperAl.Add(person);

            }

            #endregion

            #region ��ʱ����2

            if (!string.IsNullOrEmpty(this.txtTmpHelper2.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationApplication.ID;
                role.ID = "777777";
                role.Name = this.txtTmpHelper2.Text;
                role.RoleType.ID = Neusoft.HISFC.Models.Operation.EnumOperationRole.TmpHelper2;
                role.ForeFlag = "0";
                this.operationApplication.RoleAl.Add(role);

                person = new Neusoft.FrameWork.Models.NeuObject();

                person.ID = role.ID;
                person.Name = role.Name;
                this.operationApplication.HelperAl.Clear();
                this.operationApplication.HelperAl.Add(person);

            }

            #endregion

            //ԤԼ����
            this.operationApplication.PreDate = this.dtOperDate.Value;
            //������
            this.operationApplication.OperateRoom.ID = this.cmbExeDept.Tag.ToString();
            this.operationApplication.OperateRoom.Name = this.cmbExeDept.Text;
            this.operationApplication.ExeDept = this.operationApplication.OperateRoom.Clone();
            
            //����̨����
            //int index = this.cmbTableType.SelectedIndex + 1;
            //this.operationApplication.TableType = index.ToString();
            
            //�Ƿ���������
            this.operationApplication.SpecialItem = this.cmbInfectType.Text;
            this.operationApplication.BloodNum = this.cmbInfectType.SelectedIndex;
            if (this.cmbInfectType.SelectedIndex == 0)//��
                this.operationApplication.IsSpecial = false;
            else
                this.operationApplication.IsSpecial = true;

            //̨��
            //this.operationApplication.BloodUnit = this.cmbOrder.Text;

            //�Ƿ���ҪѲ��
            //this.operationApplication.IsPrepNurse = this.cbxNeedXH.Checked;

            //�Ƿ���Ҫ��е
            //this.operationApplication.IsAccoNurse = this.cbxNeedQX.Checked;

            //�Ƿ�ͬ��ʹ���Է���Ŀ
            if (this.cmbOwn.SelectedIndex == 0)
                this.operationApplication.IsHeavy = true;
            else
                this.operationApplication.IsHeavy = false;

            //index = this.cmbOperKind.SelectedIndex + 1;
            this.operationApplication.OperateKind = (this.cmbOperKind.SelectedIndex+1).ToString();//index.ToString();

            //������
            this.operationApplication.User.ID = Environment.OperatorID;
            //����ҽ��
            this.operationApplication.ApplyDoctor.ID = this.cmbApplyDoct.Tag.ToString();
            this.operationApplication.ApplyDoctor.Name = this.cmbApplyDoct.Text;
            //�������
            this.operationApplication.ApplyDoctor.Dept.ID = Environment.OperatorDeptID;
            //������Դ
            this.operationApplication.PatientSouce = "2";//סԺ����
            this.operationApplication.OperationDoctor.Dept.ID = this.comDept.Tag.ToString();
           
            //{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
            this.operationApplication.InciType.ID = this.cmbIncitepe.Tag.ToString();
            //�ϲ�����
            this.operationApplication.AneNote = this.txtAnaeNote.Text.Trim();
            //����˵��
            this.operationApplication.ApplyNote = this.rtbApplyNote.Text.Trim();

            return 0;
        }

        /// <summary>
        /// ����������Ŀ
        /// </summary>
        /// <param name="operationItem"></param>
        private void SetOperationItem(object operationItem)
        {
            if (operationItem.GetType() == typeof(Neusoft.HISFC.Models.Operation.OperationInfo))
            {
                Neusoft.HISFC.Models.Operation.OperationInfo obj = (Neusoft.HISFC.Models.Operation.OperationInfo)operationItem;
                this.operationApplication.OperationInfos.Add(obj);
            }
            else
            {
                Neusoft.HISFC.Models.Operation.OperationInfo operationInfo = new Neusoft.HISFC.Models.Operation.OperationInfo();
                operationInfo.OperationItem = operationItem as Neusoft.HISFC.Models.Base.Item;//������Ŀ
                operationInfo.FeeRate = 1m;//����
                operationInfo.Qty = 1;//����
                operationInfo.StockUnit = (operationItem as Neusoft.HISFC.Models.Base.Item).PriceUnit;//��λ
                operationInfo.OperateType.ID = (operationItem as Neusoft.HISFC.Models.Fee.Item.Undrug).OperationScale.ID;
                operationInfo.IsValid = true;
                operationInfo.IsMainFlag = false;

                this.operationApplication.OperationInfos.Add(operationInfo);
                this.operationApplication.OperationType.ID = operationInfo.OperateType.ID;
            }
        }

        /// <summary>
        /// ��Ч����֤
        /// </summary>
        /// <returns></returns>
        private int Valid()
        {
            if (this.isNew == false)
            {
                if (this.operationApplication.ExecStatus == "3" || this.operationApplication.ExecStatus == "4")
                {
                    MessageBox.Show("�����뵥�Ѱ��Ż�Ǽ�,�����޸�!", "��ʾ");
                    return -1;
                }
                if (this.operationApplication.ExecStatus == "5")
                {
                    MessageBox.Show("�����뵥��ȡ���Ǽ�,�����޸ģ�", "��ʾ");
                    return -1;
                }
                if (this.operationApplication.IsValid == false)
                {
                    MessageBox.Show("�����뵥�Ѿ�����!", "��ʾ");
                    return -1;
                }
            } 
            if (operationApplication.PatientInfo.ID == "")
            {
                MessageBox.Show("��ѡ�����뻼��!", "��ʾ");
                return -1;
            }
            if (this.txtDiag.Text.Length == 0)
            {
                MessageBox.Show("��ǰ���һ����Ϊ��!", "��ʾ");
                txtDiag.Focus();
                return -1;
            }

            string Diag1 = txtDiag.Text;
            string Diag2 = txtDiag2.Text;
            string Diag3 = txtDiag3.Text;
            //.
            if (Diag1 == "")
            { 
                MessageBox.Show("��ǰ���һ����Ϊ�գ�");
                txtDiag.Focus();
                return -1;
            }
            //
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtOperation.Text, 100) == false)
            {
                txtOperation.Focus();
                MessageBox.Show("���������ƹ�����");
                return -1;
            }
            //
            if ((Diag1 == Diag2 && Diag2 != "") || (Diag1 == Diag3 && Diag1 != "") || (Diag3 == Diag2 && Diag2 != ""))
            {
                MessageBox.Show("��ǰ��ϲ����ظ�");
                txtDiag.Focus();
                return -1;
            }
            // TODO: ��Ҫ���벡�����޸�
            if (this.txtOperation.Text.Length == 0)
            {
                MessageBox.Show("���������Ʋ���Ϊ��!", "��ʾ");
                txtOperation.Focus();
                return -1;
            }

            string Oper1 = txtOperation.Text;
            string Oper2 = txtOperation2.Text;
            string Oper3 = txtOperation3.Text;
            if ((Oper1 == Oper2 && Oper2 != "") || (Oper1 == Oper3 && Oper1 != "") || (Oper3 == Oper2 && Oper2 != ""))
            {
                MessageBox.Show("���������Ʋ����ظ�");
                txtOperation.Focus();
                return -1;
            }
            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            //if (this.cmbAnae.Tag == null || this.cmbAnae.Tag.ToString() == "")
            //{
            //    MessageBox.Show("����ʽ����Ϊ��!", "��ʾ");
            //    cmbAnae.Focus();
            //    return -1;
            //}
            if (this.cmbAnseWay.Tag == null || this.cmbAnseWay.Tag.ToString() == "")
            {
                MessageBox.Show("���������Ϊ��!", "��ʾ");
                cmbAnseWay.Focus();
                return -1;
            }

            if (this.cmbExeDept.Tag == null || this.cmbExeDept.Tag.ToString() == "")
            {
                MessageBox.Show("�����Ҳ���Ϊ��!", "��ʾ");
                cmbExeDept.Focus();
                return -1;
            }

            if (this.cmbDoctor.Tag == null || this.cmbDoctor.Tag.ToString() == "")
            {
                MessageBox.Show("���߲���Ϊ��!", "��ʾ");
                cmbDoctor.Focus();
                return -1;
            }

            if (comDept.Text.Trim() == "" || comDept.Tag == null || comDept.Tag.ToString()=="")
            {
                MessageBox.Show("���߿��Ҳ���Ϊ��!", "��ʾ");
                comDept.Focus();
                return -1;
            }

            if (this.cmbHelper1.Tag == null || this.cmbHelper1.Tag.ToString() == "")
            {
                MessageBox.Show("һ������Ϊ��!", "��ʾ");
                cmbHelper1.Focus();
                return -1;
            }

            string helper1 = "";
            string helper2 = "";
            string helper3 = "";
            this.cmbHelper1.Tag.ToString();
            if (this.cmbDoctor.Tag.ToString() == this.cmbHelper1.Tag.ToString())
            {
                MessageBox.Show("������һ�������ظ�!", "��ʾ");
                cmbDoctor.Focus();
                return -1;
            }

            if (cmbHelper2.Tag != null)
            {
                helper2 = this.cmbHelper2.Tag.ToString();
            }
            if (this.cmbHelper3.Tag != null)
            {
                helper3 = this.cmbHelper3.Tag.ToString();
            }
            if ((helper1 == helper2 && helper1 != "") || (helper1 == helper3 && helper3 != "") || (helper2 == helper3 && helper3 != ""))
            {
                MessageBox.Show("һ���������������ظ�");
                cmbHelper1.Focus();
                return -1;
            }

            //if (this.cmbOrder.Text == "")
            //{
            //    MessageBox.Show("��ָ��̨��!", "��ʾ");
            //    cmbOrder.Focus();
            //    return -1;
            //}
            

            //����ָ��ʱ����������жϵ����Ƿ�����̨,�����Զ���Ϊ��̨
            Department d = new Department();

            d.ID = this.cmbExeDept.Tag.ToString();

            //int num = Environment.OperationManager.GetEnableTableNum(d, operationApplication.PatientInfo.PVisit.PatientLocation.Dept.ID, this.dtOperDate.Value);
            //int mm = Environment.OperationManager.SameDeptApplication(this.dtOperDate.Value.ToString(), this.dtOperDate.Value.Date.AddDays(1).ToString(), d.ID, operationApplication.PatientInfo.PVisit.PatientLocation.Dept.ID, cmbOrder.Text.Substring(0, 1));
            //if (mm == -1)
            //{
            //    MessageBox.Show("�ж��Ƿ���Ӧ������̨����" + Environment.OperationManager.Err);
            //    return -1;
            //}
            //if (num <= 0 && this.cmbTableType.SelectedIndex == 0 && isNew)
            //{
            //    MessageBox.Show("����������������̨,���޸�����̨����!", "��ʾ");
            //    cmbTableType.Focus();
            //    return -1;
            //}
            //if (num <= 0 && this.cmbTableType.SelectedIndex == 0 && mm == 1 && isNew)//����̨,����������̨
            //{
            //    MessageBox.Show("����������������̨,���޸�����̨����!", "��ʾ");
            //    cmbTableType.Focus();
            //    return -1;
            //}

            if (string.IsNullOrEmpty(txtAnaeNote.Text) ) // || txtAnaeNote.Tag == null || txtAnaeNote.Tag.ToString() == "")
            {
                MessageBox.Show("�ϲ���������Ϊ��!", "��ʾ");
                txtAnaeNote.Focus();
                return -1;
            }

            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtAnaeNote.Text.Trim(), 50) == false)
            {
                MessageBox.Show("�ϲ���������С��50������!", "��ʾ");
                txtAnaeNote.Focus();
                return -1;
            }

            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.rtbApplyNote.Text.Trim(), 200) == false)
            {
                MessageBox.Show("����˵������С��100������!", "��ʾ");
                rtbApplyNote.Focus();
                return -1;
            }
            if (this.cmbApplyDoct.Tag == null || this.cmbApplyDoct.Tag.ToString() == "")
            {
                MessageBox.Show("����ҽ������Ϊ��!", "��ʾ");
                cmbApplyDoct.Focus();
                return -1;
            }
            if (!checkDate)
            {
                if (((System.DateTime.Now.DayOfWeek == System.DayOfWeek.Saturday || System.DateTime.Now.DayOfWeek == System.DayOfWeek.Sunday) && cmbOperKind.Text == "����") && dtOperDate.Value.DayOfWeek == System.DayOfWeek.Monday)
                {
                    MessageBox.Show("����,���ղ���������һ������");
                    return -1;
                }
            }


            //�ж�����ʱ���Ƿ�Ϸ�
            string rtn = Environment.OperationManager.PreDateValidity(this.dtOperDate.Value);
            if (rtn == "Error")
            {
                MessageBox.Show(Environment.OperationManager.Err, "��������");
                return -1;
            }
            else if (rtn == "Before")
            {
                #region
                if (!CheckApplyTime)
                {
                    MessageBox.Show("����ʱ�䲻��С�ڵ�ǰʱ��!", "��ʾ");
                    //this.dtOperDate.Select();
                    this.dtOperDate.Focus();
                    //this.dtOperDate.ShowUpDown = true;
                    //this.dtOperDate.ShowUpDown = false;
                    return -1;
                }
                #endregion
            }
            else if (rtn == "Over")
            {
                if (this.cmbOperKind.SelectedIndex != 1)
                {
                    if (checkEmergency)
                    {
                        #region ������������������� ������ʾ
                        Neusoft.HISFC.BizProcess.Integrate.Manager dp = new Neusoft.HISFC.BizProcess.Integrate.Manager();

                        Neusoft.HISFC.Models.Base.Department dd = dp.GetDepartment(Environment.OperatorDeptID);
                        if (dd.SpecialFlag != "1")
                        {
                            MessageBox.Show("�ѳ���������������Ľ�ֹʱ��,\n��ԤԼ���������ڽ�����������,���߽���������Ϊ����!", "��ʾ");
                            cmbOperKind.Focus();
                            return -1;
                        }
                        #endregion
                    }
                }
            }
            if (this.cmbInfectType.Text == string.Empty)
            {
                MessageBox.Show("��ѡ���Ƿ�����������");
                this.cmbInfectType.Focus();
                return -1;
            }

            #region У�����
            //{6C784A56-3FFD-47c3-A2A1-6382F7A7C7E1}

            if (this.txtDiag.Text.Trim() != string.Empty &&  this.txtDiag.Tag == null  )
            {
                MessageBox.Show("��¼��ġ���ǰ���һ��������,����������");
                this.txtDiag.Focus();
                return -1;
            }

            if (this.txtDiag2.Text.Trim() != string.Empty &&  this.txtDiag2.Tag == null  )
            {
                MessageBox.Show("��¼��ġ���ǰ��϶���������,����������");
                this.txtDiag2.Focus();
                return -1;
            }

            if (this.txtDiag3.Text.Trim() != string.Empty &&  this.txtDiag3.Tag == null )
            {
                MessageBox.Show("��¼��ġ���ǰ�������������,����������");
                this.txtDiag3.Focus();
                return -1;
            }


           

            if (this.txtOperation.Text.Trim() != string.Empty &&  this.txtOperation.Tag == null )
            {
                MessageBox.Show("��¼��ĵ�һ�����������ơ�������,����������");
                this.txtOperation.Focus();
                return -1;
            }


            if (this.txtOperation2.Text.Trim() != string.Empty &&  this.txtOperation2.Tag == null  )
            {
                MessageBox.Show("��¼��ĵڶ������������ơ�������,����������");
                this.txtOperation2.Focus();
                return -1;
            }

            if (this.txtOperation3.Text.Trim() != string.Empty &&  this.txtOperation3.Tag == null  )
            {
                MessageBox.Show("��¼��ĵ��������������ơ�������,����������");
                this.txtOperation3.Focus();
                return -1;
            }
            #endregion

            return 0;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            PreSave();


            if (Valid() == -1)
            {
                return -1;
            }

            if (this.GetValue() == -1)
            {
                return -1;
            }

            #region �ж��Ƿ�����ظ���������
            if (this.isNew)
            {
                //Ĭ��ȡ��һ�����Ϊͳ����ǰ���
                string strDiagnose = "";
                string strDiagName = "";
                if (operationApplication.DiagnoseAl.Count > 0)
                {
                    foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase MainDiagnose in operationApplication.DiagnoseAl)
                    {
                        if (MainDiagnose.IsValid)
                        {
                            strDiagnose = MainDiagnose.Name + "(" + MainDiagnose.ID.ToString() + ")";
                            strDiagName = MainDiagnose.Name;
                        }
                    }
                }
                int i = Environment.OperationManager.IsExistSameApplication(operationApplication.PatientInfo.ID, strDiagnose, operationApplication.PreDate.ToString());
                if (i == -1) //��ѯ����
                {
                    MessageBox.Show("��ѯ����������Ϣ" + Environment.OperationManager.Err);
                    return -1;
                }
                if (i == 2) //���ظ��������Ϣ 
                {
                    System.Windows.Forms.DialogResult result = MessageBox.Show("����(" + operationApplication.PatientInfo.Name + ")�Ѿ�����(" + strDiagName + ")����������,�Ƿ�Ҫ��������һ��?", "��ʾ", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.No)
                    {
                        return -1;
                    }
                }
            }
            #endregion

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //trans.BeginTransaction();

            Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            opsDiagnose.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            try
            {
                if (this.isNew)//����
                {

                    if (Environment.OperationManager.CreateApplication(operationApplication) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                        return -1;
                    }
                }
                else//�޸�
                {
                    //���ж�״̬
                    OperationAppllication obj = Environment.OperationManager.GetOpsApp(this.operationApplication.ID);
                    if (obj == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("�޸����뵥��Ϣ!", "��ʾ");
                        return -1;
                    }
                    //1����2����3����4���
                    if (obj.ExecStatus == "3" || obj.ExecStatus == "4" || obj.ExecStatus == "2")
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("�����뵥�ѱ����Ż�Ǽ�,���ܽ����޸�!", "��ʾ");
                        return -1;
                    }
                    if (obj.ExecStatus == "5")
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("�����뵥�ѱ�ȡ���Ǽ�,���ܽ����޸�!", "��ʾ");
                        return -1;
                    }

                    if (obj.IsValid == false)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("�����뵥�Ѿ�����!", "��ʾ");
                        return -1;
                    }

                    if (Environment.OperationManager.UpdateApplication(operationApplication) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                        return -1;
                    }
                    
                }

                #region �����Ϣ
                //ArrayList oldDiag = opsDiagnose.QueryOpsDiagnose(operationApplication.PatientInfo.ID, "7");
                //if (oldDiag == null)
                //{
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //    MessageBox.Show("��ѯ�������ʧ��");
                //    return -1;
                //}

                // ArrayList IcdAl = opsDiagnose.QueryOpsDiagnose(operationApplication.PatientInfo.ID, "7");
                //if (IcdAl == null)
                //{
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //    MessageBox.Show("��ѯ�������ʧ��");
                //    return -1;
                //}

                //ArrayList oldDiag = new ArrayList();
                //foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase diag in IcdAl)
                //{
                //    if (diag.OperationNo == operationApplication.ID)
                //        oldDiag.Add(diag);
                //}

                int returnValue = opsDiagnose.DeleteDiagnoseByOperationNO(operationApplication.ID);

                ArrayList oldDiag = new ArrayList();

                bool bIsExist = false;
                //����Ҫ����������Ϣ�б�(OpsApp.DiagnoseAl)
                foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase willAddDiagnose in operationApplication.DiagnoseAl)
                {
                    bIsExist = false;
                    //�����������е�����������ϣ����willAddDiagnose�Ѿ����ڣ�������״̬��
                    //���willAddDiagnose�в����ڣ��������ü�¼�����ݿ���
                    foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase thisDiagnose in oldDiag)
                    {
                        if (thisDiagnose.HappenNo == willAddDiagnose.HappenNo && thisDiagnose.Patient.ID.ToString() == willAddDiagnose.Patient.ID.ToString())
                        {
                            //�Ѿ�����	����				
                            if (opsDiagnose.UpdatePatientDiagnose(willAddDiagnose) == -1) return -1;
                            bIsExist = true;
                        }
                    }
                    //������Ϻ��ֲ����� ����
                    if (bIsExist == false)
                    {
                        if (opsDiagnose.CreatePatientDiagnose(willAddDiagnose) == -1) return -1;
                    }
                }
                #endregion 

                if (this.cmbExeDept.Tag != null)
                {
                    string[] str = new string[2];
                    str[0] = this.cmbExeDept.Text;
                    str[1] = this.cmbExeDept.Tag.ToString();
                    Neusoft.FrameWork.WinForms.Classes.Function.SaveDefaultValue("ExeDept", str);
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                this.ucDiag1.Visible = false;
                this.ucOpItem1.Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "��ʾ");
                return -1;
            }
            //��������,������ʾ
            if (this.operationApplication.OperateKind == "2")
                MessageBox.Show("������Ϊ��������,��绰֪ͨ������!", "��ʾ");
            // TODO: ����Ϣ
            //Neusoft.HISFC.Common.Class.Message.SendMessage(this.lblDept.Text + "���ߣ�" + this.lblName.Text + "������������,�����!", this.operationApplication.ExeDept.ID);
            if (this.isNew)
            {
                MessageBox.Show("����ɹ�!", "��ʾ");
            }
            else
            {
                MessageBox.Show("�����޸ĳɹ�����֪ͨ������!");
            }

            if (MessageBox.Show("�Ƿ��ӡ�������뵥", "��ʾ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Print();
            }

            if (this.isNew)
                this.isNew = false;
            this.Reset();
            return 0;
        }

        /// <summary>
        /// ���ϵ�ǰ�޸����뵥
        /// </summary>
        /// <returns></returns>
        public int Cancel()
        {
            if (this.isNew) return -1;
            if (this.operationApplication.ID.Length == 0)
            {
                MessageBox.Show("��ѡ����������뵥!", "��ʾ");
                return -1;
            }

            this.operationApplication = Environment.OperationManager.GetOpsApp(this.operationApplication.ID);
            if (this.operationApplication == null)
            {
                MessageBox.Show("��ȡ���뵥��Ϣ����!", "��ʾ");
                return -1;
            }

            if (this.operationApplication.ExecStatus == "4")
            {
                MessageBox.Show("�����뵥�ѵǼ�,��������!", "��ʾ");
                return -1;
            }

            if (this.operationApplication.ExecStatus == "5")
            {
                MessageBox.Show("�����뵥��ȡ���Ǽ�,�������ϣ�", "��ʾ");
                return -1;
            }

            if (this.operationApplication.IsValid == false)
            {
                MessageBox.Show("�����뵥�Ѿ�����!", "��ʾ");
                return -1;
            }
            if (MessageBox.Show("�Ƿ����ϵ�ǰ���뵥?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.No) return -1;

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //trans.BeginTransaction();

            Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            try
            {
                if (Environment.OperationManager.CancelApplication(this.operationApplication) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                    return -1;
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
            }
            catch (Exception e)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(e.Message, "��ʾ");
                return -1;
            }

            MessageBox.Show("��绰֪ͨ������!", "��ʾ");
            MessageBox.Show("���ϳɹ�!", "��ʾ");
            return 0;
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        /// <returns></returns>
        public int Print()
        {
            //��ӡԤ��
            if (operationApplication.PatientInfo.ID == "")
                return -1;

            if (this.operationApplication.PreDate == System.DateTime.MinValue)
            {
                #region  �ж�������Ч��
                if (operationApplication.PatientInfo.ID == "")
                {
                    MessageBox.Show("��ѡ�����뻼��!", "��ʾ");
                    return -1;
                }
                if (this.txtDiag.Tag == null)
                {
                    MessageBox.Show("��ǰ��ϲ���Ϊ��!", "��ʾ");
                    return -1;
                }
                if (this.txtOperation.Tag == null)
                {
                    MessageBox.Show("���������Ʋ���Ϊ��!", "��ʾ");
                    return -1;
                }
                //if (this.cmbAnae.Tag == null || this.cmbAnae.Tag.ToString() == "")
                //{
                //    MessageBox.Show("����ʽ����Ϊ��!", "��ʾ");
                //    return -1;
                //}

                //{B9DDCC10-3380-4212-99E5-BB909643F11B}

                if (this.cmbAnseWay.Tag == null || this.cmbAnseWay.Tag.ToString() == "")
                {
                    MessageBox.Show("���������Ϊ��!", "��ʾ");
                    this.cmbAnseWay.Focus();
                    return -1;
                }

                if (this.cmbExeDept.Tag == null || this.cmbExeDept.Tag.ToString() == "")
                {
                    MessageBox.Show("�����Ҳ���Ϊ��!", "��ʾ");
                    return -1;
                }
                if (this.cmbDoctor.Tag == null || this.cmbDoctor.Tag.ToString() == "")
                {
                    MessageBox.Show("���߲���Ϊ��!", "��ʾ");
                    return -1;
                }
                if (this.cmbHelper1.Tag == null || this.cmbHelper1.Tag.ToString() == "")
                {
                    MessageBox.Show("һ������Ϊ��!", "��ʾ");
                    return -1;
                }
                string helper1 = "";
                string helper2 = "";
                string helper3 = "";
                this.cmbHelper1.Tag.ToString();
                if (cmbHelper2.Tag != null)
                {
                    helper2 = this.cmbHelper2.Tag.ToString();
                }
                if (this.cmbHelper3.Tag != null)
                {
                    helper3 = this.cmbHelper3.Tag.ToString();
                }
                if ((helper1 == helper2 && helper1 != "") || (helper1 == helper3 && helper3 != "") || (helper2 == helper3 && helper3 != ""))
                {
                    MessageBox.Show("һ���������������ظ�");
                    return -1;
                }
                if (this.cmbOrder.Text == "")
                {
                    MessageBox.Show("��ָ��̨��!", "��ʾ");
                    return -1;
                }
                //�ж�����ʱ���Ƿ�Ϸ�
                string rtn = Environment.OperationManager.PreDateValidity(this.dtOperDate.Value);
                if (rtn == "Error")
                {
                    MessageBox.Show(Environment.OperationManager.Err, "��������");
                    return -1;
                }
                else if (rtn == "Before")
                {
                    MessageBox.Show("����ʱ�䲻��С�ڵ�ǰʱ��!", "��ʾ");
                    return -1;
                }
                #endregion

            }
            if (GetValue() == -1)
                return -1;

            #region ɾ��ԭ���������뵥��Ϣ �� ��������֪ͨ������
            //			ucCreateAppPrint ucCreateAppPrint1 = new ucCreateAppPrint();
            //			neusoft.HISFC.Object.RADT.PatientInfo patient=patientMgr.PatientQuery(operationApplication.PatientInfo.Patient.ID);
            //			if(patient==null)return -1;
            //			neusoft.HISFC.Object.Operator.OpsApplication t=operationApplication.Clone();
            //			t.PatientInfo=patient;
            //
            //			ucCreateAppPrint1.ControlValue = t;
            //Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
            //p.ControlBorder = neusoft.neNeusoft.HISFC.Components.Interface.Classes.enuControlBorder.None;

            //p.PrintPreview(10, 40, ucCreateAppPrint1);

            #endregion


            if (this.arrangeFormPrint == null)
            {
                this.arrangeFormPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangeNotifyFormPrint)) as Neusoft.HISFC.BizProcess.Interface.Operation.IArrangeNotifyFormPrint;
                if (this.arrangeFormPrint == null)
                {
                    MessageBox.Show("��ýӿ�IArrangeNotifyFormPrint��������ϵͳ����Ա��ϵ��");

                    return -1;
                }
            }

            this.arrangeFormPrint.OperationApplicationForm = this.operationApplication.Clone();
            this.arrangeFormPrint.IsPrintExtendTable = false;
            this.arrangeFormPrint.Print();
            //this.arrangeFormPrint.PrintPreview();

            return 0;
        }

        #region ����
        Neusoft.HISFC.Components.Operation.ucOpItem ucOpItem1 = null; 
        int ucOpItem1_SelectItem(Keys key)
        {
            this.ProcessOps();
            this.txtOperation.Focus();
            return 1;
        }
        private int ProcessOps()
        {
            Neusoft.HISFC.Models.Fee.Item.Undrug item = null;
            if (this.ucOpItem1.GetItem(ref item) == -1)
            {
                //MessageBox.Show("��ȡ��Ŀ����!","��ʾ");
                return -1;
            }
            dirty = true;
            this.contralActive.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;
            dirty = false;

            this.contralActive.Tag = item;
            this.ucOpItem1.Visible = false;

            return 0;
        }
        private void txtOperation_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation2_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation2;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation3_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation3;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                this.txtOperation2.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                this.txtOperation3.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                ////{B9DDCC10-3380-4212-99E5-BB909643F11B}
                //this.cmbAnae.Focus();
                this.cmbAnseWay.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation.Focused)
            {
                return;
            }
            string text = this.txtOperation.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation.Location.X, txtOperation.Location.Y + txtOperation.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation.Tag = null;
        }

        private void txtOperation2_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation2.Focused)
            {
                return;
            }
            string text = this.txtOperation2.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation2.Location.X, txtOperation2.Location.Y + txtOperation2.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation2.Tag = null;
        }

        private void txtOperation3_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation3.Focused)
            {
                return;
            }
            string text = this.txtOperation3.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation3.Location.X, txtOperation3.Location.Y + txtOperation3.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation3.Tag = null;
        }

        #endregion

        #region ���
        Neusoft.HISFC.Components.Common.Controls.ucDiagnose ucDiag1 = null;
        int ucDiag1_SelectItem(Keys key)
        {
            this.ProcessDiag();
            this.txtDiag.Focus();
            return 1;
        } 
        private int ProcessDiag()
        {
            if (this.cbxCustom.Checked)
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = new Random().Next(100000, 999999).ToString();
                item.Name = this.contralActive.Text;

                dirty = true;
                this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                dirty = false;

                this.contralActive.Tag = item;

                return 0;


                #endregion
               
            }
            else
            {
                Neusoft.HISFC.Models.HealthRecord.ICD item = null;
                if (this.ucDiag1.GetItem(ref item) == -1)
                {
                    //MessageBox.Show("��ȡ��Ŀ����!","��ʾ");
                    return -1;
                }
                dirty = true;
                this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                dirty = false;

                this.contralActive.Tag = item;
                this.ucDiag1.Visible = false;
            }

            return 0;
        }
        private void txtDiag_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag;
            this.ucOpItem1.Visible = false;
        }

        private void txtDiag2_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag2;
            this.ucOpItem1.Visible = false;
        }

        private void txtDiag3_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag3;
            this.ucOpItem1.Visible = false;
        }

        private void txtDiag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucDiag1.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtDiag2.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }

        private void txtDiag2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucDiag1.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtDiag3.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }

        private void txtDiag3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucDiag1.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtOperation.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }

        private void txtDiag_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag.Focused)
            {
                return;
            }
            contralActive = this.txtDiag;
            string text = this.txtDiag.Text;
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag.Location.X, txtDiag.Location.Y + txtDiag.Height + 2);
            ucDiag1.BringToFront();
            if (this.ucDiag1.Visible == false) this.ucDiag1.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag.Tag = null;
            ucDiag1.BringToFront();
        }

        private void txtDiag2_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag2.Focused)
            {
                return;
            }
            contralActive = this.txtDiag2;
            string text = this.txtDiag2.Text;
            ucDiag1.BringToFront();
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag2.Location.X, txtDiag2.Location.Y + txtDiag2.Height + 2);
            if (this.ucDiag1.Visible == false) this.ucDiag1.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag2.Tag = null;
        }

        private void txtDiag3_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag3.Focused)
            {
                return;
            }
            contralActive = this.txtDiag3;
            string text = this.txtDiag3.Text;
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag2.Location.X, txtDiag3.Location.Y + txtDiag3.Height + 2);
            ucDiag1.BringToFront();
            if (this.ucDiag1.Visible == false) this.ucDiag1.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag3.Tag = null;
        }
        #endregion 

        #endregion

        #region �¼�
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!this.DesignMode)
            //{
            //    this.Reset();
            //    this.Init();
            //}
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            //if (keyData.GetHashCode() == Keys.Enter.GetHashCode())
            //{
            //    this.ucOpItem1.Visible = false;
            //    this.ucDiag1.Visible = false;
                //if (txtDiag.Focused)
                //{
                //    if ( !string.IsNullOrEmpty( txtDiag.Text.Trim()) )
                //    {
                //        #region donggq

                //        Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                //        item.ID = new Random().Next(100000, 999999).ToString();
                //        item.Name = this.contralActive.Text;

                //        dirty = true;
                //        this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                //        dirty = false;

                //        this.contralActive.Tag = item;

                //        #endregion
                //    }
                //}

                //if (txtDiag2.Focused)
                //{
                //    if ( !string.IsNullOrEmpty( txtDiag2.Text.Trim()))
                //    {
                //        #region donggq

                //        Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                //        item.ID = new Random().Next(100000, 999999).ToString();
                //        item.Name = this.contralActive.Text;

                //        dirty = true;
                //        this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                //        dirty = false;

                //        this.contralActive.Tag = item;

                //        #endregion
                //    }
                //}


                //if (txtDiag3.Focused)
                //{
                //    if ( !string.IsNullOrEmpty( txtDiag3.Text.Trim()))
                //    {
                //        #region donggq

                //        Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                //        item.ID = new Random().Next(100000, 999999).ToString();
                //        item.Name = this.contralActive.Text;

                //        dirty = true;
                //        this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                //        dirty = false;

                //        this.contralActive.Tag = item;

                //        #endregion
                //    }
                //}

                //if (txtOperation.Focused)
                //{
                //    if (txtOperation.Text.Trim() == "")
                //    {
                //        txtOperation.Tag = null;
                //    }
                //}
                //if (txtOperation3.Focused)
                //{
                //    if (txtOperation3.Text.Trim() == "")
                //    {
                //        txtOperation3.Tag = null;
                //    }
                //}
                //if (txtOperation2.Focused)
                //{
                //    if (txtOperation2.Text.Trim() == "")
                //    {
                //        txtOperation2.Tag = null;
                //    }
                //}
            //}

            if (keyData.GetHashCode() == Keys.Escape.GetHashCode())
            {
                this.ucOpItem1.Visible = false;
                this.ucDiag1.Visible = false;
                if (txtDiag.Focused)
                {
                    if (txtDiag.Text.Trim() == "")
                    {
                        txtDiag.Tag = null;
                    }
                }
                if (txtDiag3.Focused)
                {
                    if (txtDiag3.Text.Trim() == "")
                    {
                        txtDiag3.Tag = null;
                    }
                }
                if (txtDiag2.Focused)
                {
                    if (txtDiag2.Text.Trim() == "")
                    {
                        txtDiag2.Tag = null;
                    }
                }
                if (txtOperation.Focused)
                {
                    if (txtOperation.Text.Trim() == "")
                    {
                        txtOperation.Tag = null;
                    }
                }
                if (txtOperation3.Focused)
                {
                    if (txtOperation3.Text.Trim() == "")
                    {
                        txtOperation3.Tag = null;
                    }
                }
                if (txtOperation2.Focused)
                {
                    if (txtOperation2.Text.Trim() == "")
                    {
                        txtOperation2.Tag = null;
                    }
                }
            }
            return base.ProcessDialogKey(keyData);
        }
        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangeNotifyFormPrint) };
            }
        }

        #endregion 

        private void cmbAnae_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtOperDate.Focus();
            }
        }

        private void dtOperDate_KeyDown(object sender, KeyEventArgs e)
        {//
            if (e.KeyCode == Keys.Enter)
            {
                cmbExeDept.Focus();
            }
        }

        private void cmbExeDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbTableType.Focus();
            }
            
        }

        private void cmbTableType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbDoctor.Focus();
            }
        }

        private void cmbDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comDept.Focus();
            }
        }

        private void comDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbInfectType.Focus();
            }
        }
        
        private void cmbSpecial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper1.Focus();
            }
        }

        private void cmbHelper1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper2.Focus();
            }
        }

        private void cmbHelper2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper3.Focus();
            }
        }

        private void cmbHelper3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbOrder.Focus(); 
            }
        }

        private void cmbOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cmbIncitepe.Focus();
            }
        }

        private void cbxNeedQX_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbxNeedXH.Focus();
            }
        }

        private void cbxNeedXH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTmpHelper1.Focus();
            }

        }

        private void txtTmpHelper1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtTmpHelper2.Focus();
            }
        }

        private void txtTmpHelper2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtAnaeNote.Focus();
            }
        }

        private void txtAnaeNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbOwn.Focus();
            }
        }

        private void cmbOwn_KeyDown(object sender, KeyEventArgs e)
        {//rtbApplyNote
            if (e.KeyCode == Keys.Enter)
            {
                cmbApplyDoct.Focus();
            }
        }

        private void ucApplicationForm_Load(object sender, EventArgs e)
        {
            this.cbxCustom.Checked = true;
        }

        ////{B9DDCC10-3380-4212-99E5-BB909643F11B}
        private void cmbAnseWay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.cmbAnae.Focus();
            }
        }
        //{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
        private void cmbIncitepe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbxNeedQX.Focus();
            }
        }



        //////////////
        private void cbxCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxCustom.Checked)
            {
                this.txtDiag.IsEnter2Tab = true;
                this.txtDiag.TextChanged -= new EventHandler(txtDiag_TextChanged);
                this.txtDiag.KeyDown -= new KeyEventHandler(txtDiag_KeyDown);

                this.txtDiag2.IsEnter2Tab = true;
                this.txtDiag2.TextChanged-=new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown-=new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.IsEnter2Tab = true;
                this.txtDiag3.TextChanged-=new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown-=new KeyEventHandler(txtDiag3_KeyDown);



                this.txtOperation.IsEnter2Tab = true;
                this.txtOperation.TextChanged -= new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown -= new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.IsEnter2Tab = true;
                this.txtOperation2.TextChanged -= new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown -= new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.IsEnter2Tab = true;
                this.txtOperation3.TextChanged -= new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown -= new KeyEventHandler(txtOperation3_KeyDown);
            }
            else
            {
                this.txtDiag.IsEnter2Tab = false;
                this.txtDiag2.IsEnter2Tab = false;
                this.txtDiag3.IsEnter2Tab = false;

                this.txtOperation.IsEnter2Tab = false;
                this.txtOperation2.IsEnter2Tab = false;
                this.txtOperation3.IsEnter2Tab = false;

                this.txtDiag.TextChanged -= new EventHandler(txtDiag_TextChanged);
                this.txtDiag.KeyDown -= new KeyEventHandler(txtDiag_KeyDown);

                this.txtDiag2.TextChanged -= new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown -= new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.TextChanged -= new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown -= new KeyEventHandler(txtDiag3_KeyDown);


                this.txtOperation.TextChanged -= new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown -= new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.TextChanged -= new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown -= new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.TextChanged -= new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown -= new KeyEventHandler(txtOperation3_KeyDown);


                this.txtDiag.TextChanged += new EventHandler(txtDiag_TextChanged);
                this.txtDiag.KeyDown += new KeyEventHandler(txtDiag_KeyDown);

                this.txtDiag2.TextChanged += new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown += new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.TextChanged += new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown += new KeyEventHandler(txtDiag3_KeyDown);


                this.txtOperation.TextChanged += new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown += new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.TextChanged += new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown += new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.TextChanged += new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown += new KeyEventHandler(txtOperation3_KeyDown);
            }
        }


        private void PreSave() 
        {
            if (!this.cbxCustom.Checked) 
            {
                return;
            }

            if (!string.IsNullOrEmpty(txtDiag.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtDiag.Text;

                dirty = true;
                this.txtDiag.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                dirty = false;

                this.txtDiag.Tag = item;

                #endregion
            }

            if (!string.IsNullOrEmpty(txtDiag2.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID)) 
                {
                    item.ID = GetCustomOpitemNo();
                }

                item.Name = this.txtDiag2.Text;

                dirty = true;
                this.txtDiag2.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                dirty = false;

                this.txtDiag2.Tag = item;

                #endregion
            }


            if (!string.IsNullOrEmpty(txtDiag3.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtDiag3.Text;

                dirty = true;
                this.txtDiag3.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                dirty = false;

                this.txtDiag3.Tag = item;

                #endregion
            }


            if (!string.IsNullOrEmpty(txtOperation.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation.Text;

                dirty = true;
                this.txtOperation.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;
                dirty = false;

                this.txtOperation.Tag = item;
                
                #endregion
            }

            if (!string.IsNullOrEmpty(txtOperation2.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();



                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation2.Text;

                dirty = true;
                this.txtOperation2.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;
                dirty = false;

                this.txtOperation2.Tag = item;

                #endregion
            }

            if (!string.IsNullOrEmpty(txtOperation3.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation3.Text;

                dirty = true;
                this.txtOperation3.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;
                dirty = false;

                this.txtOperation3.Tag = item;

                #endregion
            }

        }



        public string GetCustomOpitemNo()
        {
            Neusoft.HISFC.BizProcess.Integrate.Operation.Operation op = new Neusoft.HISFC.BizProcess.Integrate.Operation.Operation();

            string strSql = "SELECT  Seq_local_itemno.NEXTVAL FROM dual";
            string val = string.Empty;

            if (op.ExecQuery(strSql) == -1)
            {
                return val;
            }
            try
            {
                if (op.Reader.Read())
                {
                    return op.Reader.GetValue(0).ToString();
                }
                else
                {
                    return val;
                }
            }
            catch
            {
                return val;
            }

            return val;
        }

       




        //////////////





    }
}
