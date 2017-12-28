using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.Operation;
using System.Collections;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: �����շ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-20]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucFeeForm :UserControl
    {
        public ucFeeForm()
        {
            InitializeComponent();
            if (!Environment.DesignMode)
                this.Clear();
        }
        #region �ֶ�
        Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
        Neusoft.HISFC.BizProcess.Integrate.RADT radtManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        //{F3C1935C-24E9-47a4-B7AE-4EA237A972C9} 
        Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = null;


        /// <summary>
        /// �Ƿ���ʾ������{2C7FCD3D-D9B4-44f5-A2EE-A7E8C6D85576}
        /// </summary>
        private bool isShowFeeRate = false;


        //{9B275235-0854-461f-8B7B-C4FE6EC6CC0B}
        ucRegistrationTree.EnumListType listType;

        //[Category("�ؼ�����"), Description("�ؼ����ͣ��������շ�")]
        public ucRegistrationTree.EnumListType ListType
        {
            get
            {
                return listType;
            }
            set
            {
                this.listType = value;
                this.InitControlName();
            }
        }


        private bool isReg = false;

        public bool IsReg
        {
            get { return isReg; }
            set { isReg = value; }
        }


       

        #endregion
        #region ����

        #region {52AD1997-8BC0-4f22-97CA-2CF10B10C5F3} ���ò����ܹ���������п� by guanyx
        private int leftWidth = 80;

        [Category("�ؼ�����"), Description("��������� ")]
        public int LeftWidth
        {
            get
            {
                return this.ucInpatientCharge1.LeftWidth;
            }
            set
            {
                this.ucInpatientCharge1.LeftWidth = value;
            }
        }

        #endregion

        [Category("�ؼ�����"), Description("���øÿؼ����ص���Ŀ��� ҩƷ:drug ��ҩƷ undrug ����: all")]
        public Neusoft.HISFC.Components.Common.Controls.EnumShowItemType ������Ŀ���
        {
            get
            {
                return ucInpatientCharge1.������Ŀ���;
            }
            set
            {
                ucInpatientCharge1.������Ŀ��� = value;
            }
        }
        /// <summary>
        /// �ؼ�����
        /// </summary>
        [Category("�ؼ�����"), Description("��û������øÿؼ�����Ҫ����"), DefaultValue(1)]
        public Neusoft.HISFC.Components.Common.Controls.ucInpatientCharge.FeeTypes �ؼ�����
        {
            get
            {
                return this.ucInpatientCharge1.�ؼ�����;
            }
            set
            {
                this.ucInpatientCharge1.�ؼ����� = value;
            }
        }

        /// <summary>
        /// �Ƿ�����շѻ��߻���0���۵���Ŀ
        /// </summary>
        [Category("�ؼ�����"), Description("��û��������Ƿ�����շѻ��߻���"), DefaultValue(false)]
        public bool IsChargeZero
        {
            get
            {
                return this.ucInpatientCharge1.IsChargeZero;
            }
            set
            {
                this.ucInpatientCharge1.IsChargeZero = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾ������{2C7FCD3D-D9B4-44f5-A2EE-A7E8C6D85576}
        /// </summary>
        [Category("�ؼ�����"), Description("�Ƿ���ʾ������"), DefaultValue(false)]
        public bool IsShowFeeRate
        {
            get { return this.ucInpatientCharge1.IsShowFeeRate; }
            set
            {
                 
                this.ucInpatientCharge1.IsShowFeeRate = value;
            }
        }

        [Category("�ؼ�����"), Description("�Ƿ��ж�Ƿ��,Y���ж�Ƿ�ѣ�����������շ�,M���ж�Ƿ�ѣ���ʾ�Ƿ�����շ�,N�����ж�Ƿ��")]
        public Neusoft.HISFC.Models.Base.MessType MessageType
        {
            get
            {
                return this.ucInpatientCharge1.MessageType;
            }
            set
            {
                ucInpatientCharge1.MessageType = value;
            }
        }
        [Category("�ؼ�����"), Description("����Ϊ���Ƿ���ʾ��������")]
        public bool IsJudgeQty
        {
            get
            {
                return this.ucInpatientCharge1.IsJudgeQty;
            }
            set
            {
                this.ucInpatientCharge1.IsJudgeQty = value;
            }
        }

        [Category("�ؼ�����"), Description("ִ�п����Ƿ�Ĭ��Ϊ��½����")]
        public bool DefaultExeDeptIsDeptIn
        {
            get
            {
                return this.ucInpatientCharge1.DefaultExeDeptIsDeptIn;
            }
            set
            {
                this.ucInpatientCharge1.DefaultExeDeptIsDeptIn = value;
            }
        }

        #region donggq--20101118--{E64BCA09-C312-4488-BED3-1B0149E8B3E9}
        [Category("�ؼ�����"), Description("���οؼ�����ͳ�ƴ�����𣬸�ʽ���£�'04','05'")]
        public string ArrFeeGate
        {
            get { return this.ucInpatientCharge1.ArrFeeGate; }
            set { this.ucInpatientCharge1.ArrFeeGate = value; }
        }

        [Category("�ؼ�����"), Description("�Ƿ���طѱ����οؼ�")]
        public bool IsShowItemTree
        {
            get { return this.ucInpatientCharge1.IsShowItemTree; }
            set { this.ucInpatientCharge1.IsShowItemTree = value; }
        } 
        #endregion
        // ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
        public OperationAppllication operationAppllication = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OperationAppllication OperationAppllication
        {
            set
            {
                if (value == null)
                {
                    this.Clear();
                    return;
                }

                this.PatientInfo = value.PatientInfo;
                //PatientInfo = radtManager.GetPatientInfomation(value.PatientInfo.ID);
                if (value.IsHeavy)
                    this.lbOwn.Text = "ͬ��ʹ���Է���Ŀ";
                else
                    this.lbOwn.Text = "��ͬ��ʹ���Է���Ŀ";
                OperationAppllication apply = value;
                

                if (this.listType == ucRegistrationTree.EnumListType.Anaesthesia)
                {
                    if (apply.RoleAl != null && apply.RoleAl.Count != 0)
                    {
                        foreach (ArrangeRole role in apply.RoleAl)
                        {
                            if (role.RoleType.ID.ToString() == EnumOperationRole.Anaesthetist.ToString())//����
                            {
                                this.cmbDoctor.Tag = role.ID;
                                this.cmbDept.Tag = Environment.OperatorDept.ID;
                            }
                        }
                    }
                }
                else 
                {
                    this.cmbDoctor.Tag = apply.OperationDoctor.ID;//{F3C1935C-24E9-47a4-B7AE-4EA237A972C9}
                     //��������{0604764A-3F55-428f-9064-FB4C53FD8136}

                    this.cmbDept.Tag = apply.OperationDoctor.Dept.ID;// this.OperationAppllication.ApplyDoctor.Dept.ID;
                    //this.cmbDept.Text = apply.OperationDoctor.Dept.Name; // this.OperationAppllication.ApplyDoctor.Dept.Name;
                }

                operationAppllication = value;
            }
            // ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
            get
            {
                return this.operationAppllication;
            }
        }

        ////{9B275235-0854-461f-8B7B-C4FE6EC6CC0B}
        private void InitControlName()
        {
            if (this.listType == ucRegistrationTree.EnumListType.Anaesthesia)
            {
                this.neuLabel3.Text = "����ҽ��";
                this.neuLabel1.Text = "�����շ�֪ͨ��";
            }
        }
      
        private Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
        {
            set
            {
                this.ucInpatientCharge1.Clear();

                #region donggq--2010.09.28--{2537B2F9-8656-40ce-8B4E-0006B747BB86}--ҽ����������������ʾ

                if (value.Pact.ID != "01")
                {
                    this.lbName.ForeColor = Color.Red;
                    this.lbAge.ForeColor = Color.Red;
                    this.lbSex.ForeColor = Color.Red;
                    this.lbPatient.ForeColor = Color.Red;
                    this.lbDept.ForeColor = Color.Red;
                    this.lbPayKind.ForeColor = Color.Red;
                }
                else
                {
                    this.lbName.ForeColor = Color.Black;
                    this.lbAge.ForeColor = Color.Black;
                    this.lbSex.ForeColor = Color.Black;
                    this.lbPatient.ForeColor = Color.Black;
                    this.lbDept.ForeColor = Color.Black;
                    this.lbPayKind.ForeColor = Color.Black;
                } 

                #endregion
                this.lbName.Text = value.Name;
                this.lbAge.Text =Neusoft.HISFC.BizProcess.Integrate.Function.GetAge( value.Birthday);
                this.lbSex.Text = value.Sex.Name;
                this.lbPatient.Text = value.PID.PatientNO;
                this.lbDept.Text = value.PVisit.PatientLocation.Dept.Name;
                this.lbPayKind.Text = value.Pact.Name; //Environment.GetPayKind(value.Pact.PayKind.ID).Name;

                #region ҽ����Ϣ

                //Neusoft.HISFC.BizProcess.Integrate.Manager mgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                //Hashtable ZZCityKind = new Hashtable();
                //ZZCityKind["302"] = "֣����ҽ�Ʊ���(��ԭ��)";
                //ZZCityKind["303"] = "֣����ҽ�Ʊ���(������)";
                //ZZCityKind["304"] = "֣����ҽ�Ʊ���(�ܳ���)";
                //ZZCityKind["305"] = "֣����ҽ�Ʊ���(��ˮ��)";
                //ZZCityKind["306"] = "֣����ҽ�Ʊ���(�Ͻ���)";
                //ZZCityKind["308"] = "֣����ҽ�Ʊ���(�ݼ���)";
                //if (value.Pact.ID == "05")
                //{
                //    if (value.SIMainInfo.PersonType.ID == "41")
                //    {
                //        this.lbPayKind.Text += "----֣����ҽ�Ʊ���(����)";
                //    }
                //    else if ((value.SIMainInfo.PersonType.ID == "21" || value.SIMainInfo.PersonType.ID == "11") && !ZZCityKind.ContainsKey(value.SIMainInfo.Fund.Name))
                //    {
                //        this.lbPayKind.Text += "----֣����ҽ�Ʊ���(ְ��)";
                //    }
                //    else
                //    {
                //        try
                //        {
                //            this.lbPayKind.Text += "----" + ZZCityKind[value.SIMainInfo.Fund.Name].ToString();
                //        }
                //        catch 
                //        {

                //        }

                //    }
                //}
                //else if (value.Pact.ID == "08")
                //{
                //    if (value.SIMainInfo.PersonType.ID == "11" || value.SIMainInfo.PersonType.ID == "21")
                //    {
                //        this.lbPayKind.Text += "----֣������·ҽ�Ʊ���(ְ��)";
                //    }
                //    else if (value.SIMainInfo.PersonType.ID == "31")
                //    {
                //        this.lbPayKind.Text += "----֣������·ҽ�Ʊ���(��ͥ)";
                //    }
                //    else
                //    {
                //        this.lbPayKind.Text += "----֣������·ҽ�Ʊ���(����)";
                //    }
                //}
                //else
                //{
                //    this.lbPayKind.Text += "----" + value.Pact.Name;
                //} 

                #endregion


                //this.cmbDept.Tag = Environment.OperatorDept.ID;
                
                //this.ucInpatientCharge1.RecipeDoctCode = Environment.OperatorID;
                ////Ϊ�˵������������Ѻ�Ŀ�������
                //this.ucInpatientCharge1.RecipeDept = Environment.OperatorDept;
                ////Ϊ�˵������������Ѻ�Ŀ�������
                //this.ucInpatientCharge1.RecipeDept = Environment.OperatorDept;

                //this.ucInpatientCharge1.RecipeDoctCode = value.PVisit.AdmittingDoctor.ID;
                //Ϊ�˵������������Ѻ�Ŀ�������
                //this.ucInpatientCharge1.RecipeDept = value.PVisit.AdmittingDoctor


                //{//{F3C1935C-24E9-47a4-B7AE-4EA237A972C9} } ����ҽ��
                //this.cmbDoctor.Tag = value.PVisit.AdmittingDoctor.ID;
                //this.cmbDoctor.Text = value.PVisit.AdmittingDoctor.Name;
                try
                {
                    if (value.PVisit.InState.ID.ToString() != Neusoft.HISFC.Models.Base.EnumInState.I.ToString())
                    {
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���߲�����Ժ״̬�����ܽ����շѲ���"));
                    }


                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message.ToString());
                }
                this.ucInpatientCharge1.PatientInfo = value;
              

                this.cmbDoctor.Focus();
            }
        }

        #endregion

        #region ����
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {

            this.lbPayKind.Text = "";
            this.lbName.Text = "";
            this.lbPatient.Text = "";
            this.lbDept.Text = "";
            this.lbSex.Text = "";
            this.lbOwn.Text = "";
            this.lbAge.Text = string.Empty;
            this.lbDate.Text = Environment.OperationManager.GetDateTimeFromSysDateTime().ToString("yyyy-MM-dd");

            //appObj = new neusoft.HISFC.Object.Operator.OpsApplication();
            this.checkBox1.Checked = false;
            this.checkBox2.Checked = false;
            this.checkBox3.Checked = false;
            this.checkBox4.Checked = false;
            this.checkBox5.Checked = false;

            this.ucInpatientCharge1.Clear();
            this.cmbDoctor.Tag = "";
            
        }
        /// <summary>
        /// ���棬�����շ�
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            //// ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
            this.ucInpatientCharge1.OperationNO = this.operationAppllication.ID;

            #region donggq--2010.09.28--{2537B2F9-8656-40ce-8B4E-0006B747BB86}--ҽ����������������ʾ

            if (this.operationAppllication.PatientInfo.Pact.ID != "01") 
            {
                if (MessageBox.Show("�ò���Ϊҽ��,��ȷ���Ƿ񱣴棿", "������ʾ",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No) 
                {
                    return -1;
                }
            }

            #endregion


            #region donggq--2010.09.30--{DC0950A9-CD29-43b0-9773-35B8AFA8D86A}--�������������Ѻ������Ǽ�

            if (this.ucInpatientCharge1.Save() < 0)
            {
                MessageBox.Show("����ʧ�ܣ�");
                return -1;
            }
            else
            {
                if (!IsReg)
                {

                    #region donggq--2010.10.04--{45A9C879-357A-4945-93A0-DFA7FB967306}

                    Neusoft.HISFC.Models.Base.Employee employee = ((Neusoft.FrameWork.Models.NeuObject)this.cmbDoctor.SelectedItem) as Neusoft.HISFC.Models.Base.Employee;
                    employee.Dept = (Neusoft.FrameWork.Models.NeuObject)this.cmbDept.SelectedItem as Neusoft.HISFC.Models.Base.Department;
                    this.OperationAppllication.OperationDoctor = employee;

                    if (employee != null)
                    {
                        Environment.OperationManager.UpdateApplication(this.OperationAppllication);
                    }

                    #endregion

                    MessageBox.Show(this.ucInpatientCharge1.SucessMsg);



                    Neusoft.HISFC.BizLogic.Operation.OpsRecord recordManager = new Neusoft.HISFC.BizProcess.Integrate.Operation.OpsRecord();
                    Neusoft.HISFC.Models.Operation.OperationRecord record = recordManager.GetOperatorRecord(this.operationAppllication.ID);
                    if (record != null)
                    {
                        this.ucInpatientCharge1.Clear();
                        this.Clear();

                        return 1;
                    }

                    if (this.listType == ucRegistrationTree.EnumListType.Operation)
                    {
                        if (DialogResult.Yes == MessageBox.Show("�Ƿ���������Ǽǣ�", "���ٵǼ���ʾ",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {

                            //Neusoft.HISFC.Components.Operation.ucRegistrationAfterFee operateReg = new Neusoft.HISFC.Components.Operation.ucRegistrationAfterFee();

                            //Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "�����Ǽ�";
                            //operateReg.IsNew = true;
                            //operateReg.OperationAppllication = this.operationAppllication;
                            //operateReg.IsCancled = false;

                            //Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(operateReg);



                            Neusoft.HISFC.Components.Operation.ucRegistrationQuick operateReg = new Neusoft.HISFC.Components.Operation.ucRegistrationQuick();

                            Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "�����Ǽ�";
                            //operateReg.IsNew = true;
                            operateReg.OperationApplication = this.operationAppllication;
                            //operateReg.IsCancled = false;

                            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(operateReg);



                        }
                    }
                }//

            }

            #endregion


           this.ucInpatientCharge1.Clear();
           this.Clear();

            

            return 1;
        }

        /// <summary>
        /// ɾ����ǰ��
        /// </summary>
        /// <returns></returns>
        public int DelRow()
        {
            return this.ucInpatientCharge1.DelRow();
        }
        //{9F3CF1C0-AF96-4d17-96B1-6B34636A42A7}


        public void InsertGroup(string groupID)
        {
            frmChooseGroupDetails frm = new frmChooseGroupDetails();
            frm.GroupID = groupID;
          DialogResult dr =   frm.ShowDialog();
          if (dr == DialogResult.Cancel)
          {
              return;
          }
          else
          {
              Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ���������Ϣ");
              Application.DoEvents();
              this.ucInpatientCharge1.AddGroupDetail(groupID,frm.AlReturnDetails); 



              Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
          }

            frm.Dispose();

            //Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ���������Ϣ");
            //Application.DoEvents();
            ////this.ucInpatientCharge1.AddGroupDetail(groupID); 

            

            //Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        public int Print()
        {
            return this.print.PrintPreview(this);
        }
        #endregion

        #region �¼�
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Environment.DesignMode)
            {
                Neusoft.HISFC.Models.Base.Employee em = (Neusoft.HISFC.Models.Base.Employee)Environment.OperationManager.Operator;
                this.ucInpatientCharge1.Init(em.Dept.ID);
                //�������ҽ�� {F3C1935C-24E9-47a4-B7AE-4EA237A972C9}
                Neusoft.HISFC.BizProcess.Integrate.Manager conMag = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                ArrayList aDoc = conMag.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D);
                if (aDoc != null)
                {
                    this.cmbDoctor.AddItems(aDoc);
                }
                ArrayList alDept = conMag.QueryDeptmentsInHos(true);
                if (alDept != null)
                {
                    this.cmbDept.AddItems(alDept);
                }
            }
        }
        #endregion
        //{F3C1935C-24E9-47a4-B7AE-4EA237A972C9} 
        private void cmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.patientInfo != null)
            //{
                //this.patientInfo.PVisit.AdmittingDoctor.ID = this.cmbDoctor.Tag.ToString();
                //this.patientInfo.PVisit.AdmittingDoctor.Name = this.cmbDoctor.Text;
                //this.ucInpatientCharge1.RecipeDoctCode = this.cmbDoctor.Tag.ToString();
                //this.ucInpatientCharge1.RecipeDept = ((Neusoft.HISFC.Models.Base.Employee)this.cmbDoctor.SelectedItem).Dept;
            //}
           
                //this.ucInpatientCharge1.PatientInfo.PVisit.AdmittingDoctor.ID = this.cmbDoctor.Tag.ToString();
            //this.ucInpatientCharge1.PatientInfo.PVisit.AdmittingDoctor.Name = this.cmbDoctor.Text;
            #region donggq--2010.09.30--{DC0950A9-CD29-43b0-9773-35B8AFA8D86A}--�������������Ѻ������Ǽ�

            try
            {
                this.ucInpatientCharge1.RecipeDoctCode = this.cmbDoctor.Tag.ToString();
                this.ucInpatientCharge1.RecipeDept = ((Neusoft.HISFC.Models.Base.Employee)this.cmbDoctor.SelectedItem).Dept;

            }
            catch 
            {

            }
            #endregion

        }
        //{F3C1935C-24E9-47a4-B7AE-4EA237A972C9} 
        private void cmbDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ucInpatientCharge1.Focus();
            }


        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Neusoft.FrameWork.Models.NeuObject obj = null;
            try
            {
                obj = this.cmbDept.SelectedItem as Neusoft.FrameWork.Models.NeuObject;
            }
            catch (Exception)
            {

                return;
            }



            this.ucInpatientCharge1.RecipeDept = obj;

        }
    }
}
