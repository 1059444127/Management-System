using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace Neusoft.HISFC.Components.Order.OutPatient.Forms
{
    public partial class frmOutPatientOrder : Neusoft.FrameWork.WinForms.Forms.frmBaseForm,Neusoft.FrameWork.WinForms.Classes.IPreArrange,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public frmOutPatientOrder()
        {
            InitializeComponent();
            this.iControlable = this.ucOutPatientOrder1 as Neusoft.FrameWork.WinForms.Forms.IControlable;
            this.CurrentControl = this.ucOutPatientOrder1;
            this.panelToolBar.Visible = false;
            InitButton();
        }


        /// <summary>
        /// �˻�������ҵ���  {184209CF-569F-4355-896D-FB33FF6C506F} 
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Fee feeMgr = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        private Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();

        private HISFC.Components.Order.OutPatient.Classes.Function Function = new HISFC.Components.Order.OutPatient.Classes.Function();

        private Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

        private bool isAccountTerimal = false;

        #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ��� by guanyx ����
        //HISFC.BizProcess.Integrate.Registration.Registration regManager = new Neusoft.HISFC.BizProcess.Integrate.Registration.Registration();
        //ZZlocal.Clinic.HISFC.OuterConnector.Triage.Triage triage = new ZZlocal.Clinic.HISFC.OuterConnector.Triage.Triage();
        //ZZlocal.Clinic.HISFC.BizLogic.Registration.QueryDepartStat departStatManager = new ZZlocal.Clinic.HISFC.BizLogic.Registration.QueryDepartStat();
        //private string deptcode = "";
        //private string opercode = "";
        //private string nurseStation = "";
        ///// <summary>
        ///// ִ�з���dll����ֵ
        ///// [0,*]�������С����ã������Բ����ã�
        ///// [1, �����еĻ���cliniccodeֵ]�������С������ã������￪ʼ���������š����ã�����������������ã�
        ///// [2, �����еĻ���cliniccode]�������С������ã������￪ʼ���������š������ã���������������ã�
        ///// [999��]��ʾ�ÿ��ҷ���������ݱ��������
        ///// </summary>
        ////private char[] rtn = new char[20];
        //StringBuilder rtn = new StringBuilder(100);
        #endregion

        #region {FD676895-D54B-4e1a-8AAF-383E925DB518} ҽ���кţ�����޸� by guanyx 
        [DllImport("JZWindowsDll.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int InitDllForm(IntPtr i, char[] inputString);
        //�������
        ZZlocal.Clinic.HISFC.OuterConnector.Triage.Triage triage = new ZZlocal.Clinic.HISFC.OuterConnector.Triage.Triage();
        //�����ϲ������
        ZZlocal.Clinic.HISFC.BizLogic.Registration.QueryDepartStat departStatManager = new ZZlocal.Clinic.HISFC.BizLogic.Registration.QueryDepartStat();
        private string deptcode = "";
        private string opercode = "";
        private string nurseStation = "";
        #endregion 

        /// <summary>
        /// ��Ⱦ���ϱ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.DCP.IDCP dcpInstance = null;

        private void frmOutPatientOrder_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            this.tbFilter.DropDownItemClicked += new ToolStripItemClickedEventHandler(toolStrip1_ItemClicked);

            ////this.AddOrderHandle();
            this.initButton(false);

            this.tbAddOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Yҽ��);
            this.tbComboOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.H�ϲ�);
            this.tbCancelOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȡ��);
            this.tbDelOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sɾ��);
            this.tbOperation.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Z���);
            this.tbSaveOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.B����);
            this.tbCheck.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.H����);
            this.tb1Exit.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.T�˳�);
            this.tbExitOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.T�˳�);
            this.tbGroup.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Z����);
            
            this.tbSeePatient.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.QȨ�����);
            this.tbRefreshPatient.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sˢ��);
            this.tbQueryOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.C��ѯ);
            this.tbPatientTree.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.G�˿�);
            this.tbChooseDrugDept.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.K����);
            #region {777C56C9-D8D7-478c-A10F-EF9B37335A08}
            this.tbPrintOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ);
            this.tbRegisterEmergencyPatient.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.J����);
            this.tbOutEmergencyPatient.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.C��Ժ�Ǽ�);
            this.tbInEmergencyPatient.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.Zת��);
            this.tbDiseaseReport.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.J��������);
            this.tbHerbal.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.M��ϸ);

            #region {DEF4C3EC-A951-4547-80E2-AE5ADC82B606} �кš����Ű�ťby guanyx
            this.tbCallNO.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.X��һ��);
            this.tbPassNO.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.T��ת);
            this.tbBeginOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.T����);
            this.tbEndOrder.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.B����);
            #endregion

            #endregion
            this.panelTree.Height = this.Height - 162;
            this.panelTree.Visible = false;

            //{B17077E6-7E65-45fb-BA25-F2883EB6BA27}  ��֤��̨�����Ҳ�ά��ʱ���ڿ��Թر�
            //this.ucOutPatientTree1.RefreshTreeView();

            this.ucOutPatientTree1.TreeDoubleClick += new HISFC.Components.Order.OutPatient.Controls.ucOutPatientTree.TreeDoubleClickHandler(ucOutPatientTree1_TreeDoubleClick);
            isAccountTerimal = controlIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.SysConst.Use_Account_Process, true, false);
            this.ucOutPatientOrder1.OnRefreshGroupTree += new EventHandler(ucOutPatientOrder1_OnRefreshGroupTree);

            //����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
            this.ChooseDrugDept(false);

            #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx ����
            //try
            //{
            //    deptcode = ((HISFC.Models.Base.Employee)triage.Operator).Dept.ID;
            //    opercode = triage.Operator.ID;
            //    nurseStation = departStatManager.GetUpNurseStation(deptcode);
            //    char[] deptcodeChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(deptcode);
            //    char[] opercodeChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(opercode);
            //    int returnValue = 0;
            //    /*
            //     * 0��ִ�гɹ���
            //     * 1�����ݿ�����ʧ��
            //     * 2������ģ���ʼ��ʧ��
            //     * 3���洢���̵���ʧ��
            //     */
            //    if (!string.IsNullOrEmpty(nurseStation))
            //    {
            //        rtn = new StringBuilder();
            //        char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
            //        returnValue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.GetTreatState(nurseStationChar, opercodeChar, ref rtn);
            //    }
            //    else
            //    {
            //        rtn = new StringBuilder();
            //        returnValue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.GetTreatState(deptcodeChar, opercodeChar, ref rtn);
            //    }

            //    if (returnValue == 0)
            //    {
            //        string clinicCode = "";
            //        if (rtn != null)
            //        {
            //            switch (rtn.ToString().Substring(0, 1))
            //            {
            //                case "0":
            //                    this.tbCallNO.Enabled = true;
            //                    this.tbPassNO.Enabled = false;
            //                    this.tbBeginOrder.Enabled = false;
            //                    this.tbEndOrder.Enabled = false;
            //                    break;
            //                case "1":
            //                    this.tbCallNO.Enabled = false;
            //                    this.tbPassNO.Enabled = true;
            //                    this.tbBeginOrder.Enabled = true;
            //                    this.tbEndOrder.Enabled = false;
            //                    clinicCode = (rtn.ToString()).Substring(2);
            //                    Neusoft.HISFC.Models.Registration.Register p = regManager.GetByClinic(clinicCode);
            //                    MessageBox.Show(p.PID.CardNO + "(" + p.Name + "),���ں����У���������ʼ������ߡ����š�������");
            //                    break;
            //                case "2":
            //                    this.tbCallNO.Enabled = false;
            //                    this.tbPassNO.Enabled = false;
            //                    this.tbBeginOrder.Enabled = false;
            //                    this.tbEndOrder.Enabled = true;
            //                    clinicCode = (rtn.ToString()).Substring(2);
            //                    Neusoft.HISFC.Models.Registration.Register pp = regManager.GetByClinic(clinicCode);
            //                    MessageBox.Show(pp.PID.CardNO + "(" + pp.Name + "),���ھ����У��������������������");
            //                    break;
            //                default:
            //                    this.tbCallNO.Enabled = false;
            //                    this.tbPassNO.Enabled = false;
            //                    this.tbBeginOrder.Enabled = false;
            //                    this.tbEndOrder.Enabled = false;
            //                    MessageBox.Show("���ҷ�����������쳣�������½�����������ϵ��Ϣ�ƣ�");
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //        }
            //    }
            //    else
            //    {
            //        this.tbCallNO.Enabled = false;
            //        this.tbPassNO.Enabled = false;
            //        this.tbBeginOrder.Enabled = false;
            //        this.tbEndOrder.Enabled = false;
            //        MessageBox.Show("ִ�г�ʼ��ʧ�ܣ������½�����棡");
            //    }
            //}
            //catch (Exception ee)
            //{
            //    this.tbCallNO.Enabled = false;
            //    this.tbPassNO.Enabled = false;
            //    this.tbBeginOrder.Enabled = false;
            //    this.tbEndOrder.Enabled = false;
            //    MessageBox.Show("�����ӿ��쳣��" + ee.Message + "�����½�����棡");
            //}
            #endregion

            #region  {FD676895-D54B-4e1a-8AAF-383E925DB518} ҽ���кţ�����޸� by guanyx
            deptcode = ((HISFC.Models.Base.Employee)triage.Operator).Dept.ID;
            opercode = triage.Operator.ID;
            nurseStation = departStatManager.GetUpNurseStation(deptcode);
            string inputString = "";
            char[] inputChar = new char[11];
            if (nurseStation.Length == 4)
            {
                inputString = nurseStation + "," + opercode;
                inputChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(inputString);
            }
            else
            {
                inputString = deptcode + "," + opercode;
                inputChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(inputString);
            }
            InitDllForm(this.Handle, inputChar);
            #endregion

        }

        void ucOutPatientOrder1_OnRefreshGroupTree(object sender, EventArgs e)
        {
            this.tvGroup.RefrshGroup();
        }

        private void ucOutPatientTree1_TreeDoubleClick(object sender, HISFC.Components.Order.OutPatient.Controls.ClickEventArgs e)
        {
            #region {22571B58-A56B-4dc3-A32C-EC17D74423A2}
            //if (this.CurrentControl.GetType() == typeof(HISFC.Components.Order.OutPatient.Controls.ucPatientCase))
            //{
            //    (this.CurrentControl as HISFC.Components.Order.OutPatient.Controls.ucPatientCase).Reg = e.Message;
            //}
            //if (this.CurrentControl.GetType() == typeof(HISFC.Components.Order.OutPatient.Controls.ucOrderHistory))
            //{
            //    (this.CurrentControl as HISFC.Components.Order.OutPatient.Controls.ucOrderHistory).Patient = e.Message;
            //}

            try
            {
                if (this.ucOutPatientTree1.neuTreeView1.Visible)
                {
                    this.tree = this.ucOutPatientTree1.neuTreeView1;
                    TreeViewEventArgs mye = new TreeViewEventArgs(this.ucOutPatientTree1.neuTreeView1.SelectedNode);
                    this.tree_AfterSelect(e.Message, mye);
                    this.Tag = this.ucOutPatientTree1.neuTreeView1.SelectedNode.Tag;
                }
                if (this.ucOutPatientTree1.neuTreeView2.Visible)
                {
                    this.tree = this.ucOutPatientTree1.neuTreeView2;
                    TreeViewEventArgs mye = new TreeViewEventArgs(this.ucOutPatientTree1.neuTreeView2.SelectedNode);
                    this.tree_AfterSelect(e.Message, mye);
                    this.Tag = this.ucOutPatientTree1.neuTreeView2.SelectedNode.Tag;
                }

                if (this.Tag is Neusoft.HISFC.Models.Registration.Register)
                {
                    //�ж��˻����̵ĹҺ��շ����
                    //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                    bool isAccount = false;
                    decimal vacancy = 0m;
                    Neusoft.HISFC.Models.Registration.Register r = (Neusoft.HISFC.Models.Registration.Register)Tag;

                    if (isAccountTerimal && r.IsAccount)
                    {

                        if (feeMgr.GetAccountVacancy(r.PID.CardNO, ref vacancy) <= 0)
                        {
                            MessageBox.Show(feeMgr.Err);
                            return;
                        }
                        isAccount = true;

                    }
                    if (isAccount && r.IsFee == false)
                    {
                        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                        #region �˻���ȡ�Һŷ�

                        if (!feeMgr.CheckAccountPassWord(r))
                        {
                            this.ucOutPatientTree1.neuTreeView1.SelectedNode = null;
                            this.ucOutPatientTree1.PatientInfo = null;
                            return;
                        }

                        if (isAccount && !r.IsFee)
                        {
                            if (vacancy < r.OwnCost)
                            {
                                MessageBox.Show("�˻����㣬�뽻�ѣ�");
                                return;
                            }


                            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                            if (feeMgr.AccountPay(r, r.OwnCost, "�Һ��շ�", (orderManager.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID, string.Empty) < 0)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("���˻����ʧ�ܣ�" + feeMgr.Err);
                                return;
                            }
                            Neusoft.HISFC.BizProcess.Integrate.Registration.Registration registerManager = new Neusoft.HISFC.BizProcess.Integrate.Registration.Registration();
                            r.SeeDOCD = orderManager.Operator.ID;
                            r.SeeDPCD = (orderManager.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID;
                            if (registerManager.UpdateAccountFeeState(r.ID,r.SeeDOCD ,r.SeeDPCD , orderManager.GetDateTimeFromSysDateTime()) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("���¹Һű����շ�״̬����");
                                return;
                            }
                            Neusoft.FrameWork.Management.PublicTrans.Commit();
                            r.IsFee = true;
                        }
                        #endregion
                    }
                }
            }
            catch { }
            finally { }
            #endregion
        }

        private void initButton(bool isDisign)
        {
            this.tbGroup.Enabled = !isDisign;
            this.tbAddOrder.Enabled = !isDisign;
            this.tbComboOrder.Enabled = isDisign;
            this.tbCancelOrder.Enabled = isDisign;
            this.tbCheck.Enabled = isDisign;
            this.tbHerbal.Enabled = isDisign;
            this.tbOperation.Enabled = false;
            this.tbDelOrder.Enabled = isDisign;
            //{1C0814FA-899B-419a-94D1-789CCC2BA8FF}
            //this.tbRegisterEmergencyPatient.Enabled = isDisign;
            //this.tbInEmergencyPatient.Enabled = isDisign;
            //this.tbInEmergencyPatient.Enabled = isDisign;
            this.tbExitOrder.Enabled = isDisign;
            this.tbFilter.Enabled = !isDisign;
            this.tbFilter.Visible = false;
            this.tbQueryOrder.Enabled = !isDisign;
            
            this.tbSaveOrder.Enabled = isDisign;
            this.tbSeePatient.Enabled = !isDisign;
            if (isDisign) //����
            {
                if (tvGroup == null)
                {
                    tvGroup = new Neusoft.HISFC.Components.Common.Controls.tvDoctorGroup();
                    tvGroup.Type = Neusoft.HISFC.Components.Common.Controls.enuType.Order;
                    tvGroup.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.C;
                    tvGroup.Init();
                    tvGroup.SelectOrder += new Neusoft.HISFC.Components.Common.Controls.SelectOrderHandler(tvGroup_SelectOrder);
                }
                tvGroup.Dock = DockStyle.Fill;
                tvGroup.Visible = true;
                
                this.panelTree.Visible = true;
                this.panel2.Visible = true;

                // {4255B9B6-1BA4-4f6a-9FD9-2856220D806D} xupan
                this.neuTextBox1.Dock = DockStyle.Top;
                this.panelTree.Dock = DockStyle.Fill;
                // end

                if (this.btnShow.Visible != true)
                {
                    this.panel2.Width = 170;
                    this.panelTree.Width = 170;
                }
                #region {22571B58-A56B-4dc3-A32C-EC17D74423A2}
                this.panelTree.Controls.Add(tvGroup);
                #endregion
                //this.SetTree(tvGroup);
                this.neuPanel1.Visible = false;
            }
            else
            {
                this.neuPanel1.Visible = true;
                this.panelTree.Visible = false;
                this.panel2.Visible = false;
                if (tvGroup != null) tvGroup.Visible = false;
                this.ucOutPatientOrder1.Patient = new Neusoft.HISFC.Models.Registration.Register();
            }
        }

        /// <summary>
        /// ��ʼ���ڶ���BUTTON
        /// </summary>
        private void InitButton()
        {
            //base.toolBar2.Items.Add(this.tbCallNO);
            //base.toolBar2.Items.Add(this.tbBeginOrder);
            //base.toolBar2.Items.Add(this.tbEndOrder);
            //base.toolBar2.Items.Add(this.tbPassNO);
            //base.toolBar2.Items.Add(this.toolStripSeparator8);
            base.toolBar2.Items.Add(this.tbSeePatient);
            base.toolBar2.Items.Add(this.tbClinicCase);
            base.toolBar2.Items.Add(this.toolStripSeparator7);
            base.toolBar2.Items.Add(this.tb1Exit);
            base.toolBar2.ImageScalingSize = new System.Drawing.Size(32, 32); 
            base.toolBar2.ItemClicked += new ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
        }


        Neusoft.HISFC.Components.Common.Controls.tvDoctorGroup tvGroup = null;//����
        bool isEditGroup = false;

        private void initButtonGroup(bool isEdit)
        {
            this.tbAddOrder.Enabled = !isEdit;
            this.tbSaveOrder.Enabled = isEdit;
            this.tbExitOrder.Enabled = !isEdit;
            this.isEditGroup = isEdit;
            this.tbQueryOrder.Enabled = !isEdit;
            this.tbSeePatient.Enabled = !isEdit;
            //{CF7BCF69-95C3-4dcf-A61C-451E26C56161}
            this.tbComboOrder.Enabled = isEdit;
            this.tbCancelOrder.Enabled = isEdit;
            this.tbDelOrder.Enabled = isEdit;//{11F97F55-F747-4ad9-A74F-086635D5EBD9}
            if (isEdit) //����
            {
                if (tvGroup == null)
                {
                    tvGroup = new Neusoft.HISFC.Components.Common.Controls.tvDoctorGroup();
                    tvGroup.Type = Neusoft.HISFC.Components.Common.Controls.enuType.Order;
                    tvGroup.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.C;
                    tvGroup.Init();
                    tvGroup.SelectOrder += new Neusoft.HISFC.Components.Common.Controls.SelectOrderHandler(tvGroup_SelectOrder);
                }
                tvGroup.Dock = DockStyle.Fill;
                tvGroup.Visible = true;
                this.panelTree.Visible = true;
                this.panel2.Visible = true;
                if (this.btnShow.Visible != true)
                {
                    this.panel2.Width = 170;
                    this.panelTree.Width = 170;
                }
                #region {22571B58-A56B-4dc3-A32C-EC17D74423A2}
                this.panelTree.Controls.Add(tvGroup);
                //this.SetTree(tvGroup);
                #endregion
            }
            else
            {
                this.panelTree.Visible = false;
                this.panel2.Visible = false;
                if (tvGroup != null) tvGroup.Visible = false;
            }
        }

        void tvGroup_SelectOrder(System.Collections.ArrayList alOrders)
        {
            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988} //��ҩ������ҩ��������
            ArrayList alHerbal = new ArrayList(); //��ҩ

            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order order in alOrders)
            {
                if (order.Item.SysClass.ID.ToString() == "PCC") //��ҩ
                {
                    alHerbal.Add(order);
                }
                else
                {
                    this.ucOutPatientOrder1.AddNewOrder(order, 0);
                }
            }
            if (alHerbal.Count > 0)
            {
                this.ucOutPatientOrder1.AddHerbalOrders(alHerbal);
            }
            this.ucOutPatientOrder1.RefreshCombo();
            
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab.Controls.Count > 0)
            {
                this.iQueryControlable = this.tabControl1.SelectedTab.Controls[0] as Neusoft.FrameWork.WinForms.Forms.IQueryControlable;
                this.iControlable = this.tabControl1.SelectedTab.Controls[0] as Neusoft.FrameWork.WinForms.Forms.IControlable;
                this.CurrentControl = this.tabControl1.SelectedTab.Controls[0];
                if (this.CurrentControl.GetType() == typeof(HISFC.Components.Order.OutPatient.Controls.ucPatientCase))
                {
                    (this.CurrentControl as HISFC.Components.Order.OutPatient.Controls.ucPatientCase).Reg = ucOutPatientTree1.PatientInfo;
                    this.neuPanel1.Visible = false;
                }
                
            }
            this.InitButton();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
            deptcode = ((HISFC.Models.Base.Employee)triage.Operator).Dept.ID;
            opercode = triage.Operator.ID;
            nurseStation = departStatManager.GetUpNurseStation(deptcode);
            char[] deptcodeChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(deptcode);
            char[] opercodeChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(opercode);
            #endregion
            if (e.ClickedItem == this.tbAddOrder)//����
            {
                this.ucOutPatientOrder1.Patient = this.ucOutPatientTree1.PatientInfo;
                this.ucOutPatientOrder1.CurrentRoom = this.ucOutPatientTree1.CurrRoom;
                #region {66712C76-62CB-43a1-8DE0-C0C02AB3F9B4}
                this.statusBar1.Panels[1].Text = "(��ɫ���¿�)(��ɫ���շ�)(��ɫ������)";
                //this.statusBar1.Controls[1].AutoSize = StatusBarPanelAutoSize.Contents;
                #endregion
                if (this.ucOutPatientOrder1.Add() == 0)
                {
                    this.initButton(true);

                    #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx ����
                    //this.tbCallNO.Enabled = false; 
                    //this.tbPassNO.Enabled = false;
                    //StringBuilder cardno = new StringBuilder(10);
                    //try
                    //{
                    //    if (!string.IsNullOrEmpty(nurseStation))
                    //    {
                    //        char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
                    //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.BeginTreat(nurseStationChar, opercodeChar, ref cardno);
                    //    }
                    //    else
                    //    {
                    //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.BeginTreat(deptcodeChar, opercodeChar, ref cardno);
                    //    }
                    //}
                    //catch (Exception ee)
                    //{
                    //    MessageBox.Show("RW�����ӿ��쳣��" + ee.Message);
                    //}
                    #endregion
                }

            }
            #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx ����
            //else if (e.ClickedItem == this.tbCallNO)
            //{
            //    StringBuilder cardno = new StringBuilder(10);
            //    int returnvalue = 0;
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(nurseStation))
            //        {
            //            cardno = new StringBuilder();
            //            char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.CallPatient(nurseStationChar, opercodeChar, ref cardno);
            //        }
            //        else
            //        {
            //            cardno = new StringBuilder();
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.CallPatient(deptcodeChar, opercodeChar, ref cardno);
            //        }
            //        if (returnvalue == 0)
            //        {
            //            this.tbCallNO.Enabled = false;
            //            this.tbPassNO.Enabled = true;
            //            this.tbBeginOrder.Enabled = true;
            //            this.tbEndOrder.Enabled = false;
            //            Neusoft.HISFC.Models.Registration.Register p = regManager.GetByClinic(cardno.ToString());
            //            MessageBox.Show("���ں��� " + p.PID.CardNO + "(" + p.Name + ")�������ĵȴ���");
            //        }
            //        else
            //        {
            //            this.tbCallNO.Enabled = true;
            //            this.tbPassNO.Enabled = false;
            //            this.tbBeginOrder.Enabled = false;
            //            this.tbEndOrder.Enabled = false;
            //            MessageBox.Show("���л���ʧ�ܣ������µ�����кš���");
            //        }
            //    }
            //    catch (Exception ee)
            //    {
            //        this.tbCallNO.Enabled = true;
            //        this.tbPassNO.Enabled = false;
            //        this.tbBeginOrder.Enabled = false;
            //        this.tbEndOrder.Enabled = false;
            //        MessageBox.Show("�����ӿ��쳣��" + ee.Message + "�����µ�����кš���");
            //    }
            //}
            //else if (e.ClickedItem == this.tbBeginOrder)
            //{
            //    #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
            //    int returnvalue = 0;
            //    StringBuilder cardno = new StringBuilder(10);
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(nurseStation))
            //        {
            //            cardno = new StringBuilder();
            //            char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.BeginTreat(nurseStationChar, opercodeChar, ref cardno);
            //        }
            //        else
            //        {
            //            cardno = new StringBuilder();
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.BeginTreat(deptcodeChar, opercodeChar, ref cardno);
            //        }
            //        if (returnvalue == 0)
            //        {
            //            this.tbCallNO.Enabled = false;
            //            this.tbPassNO.Enabled = false;
            //            this.tbBeginOrder.Enabled = false;
            //            this.tbEndOrder.Enabled = true;
            //        }
            //        else
            //        {
            //            this.tbCallNO.Enabled = false;
            //            this.tbPassNO.Enabled = true;
            //            this.tbBeginOrder.Enabled = true;
            //            this.tbEndOrder.Enabled = false;
            //            MessageBox.Show("��ʼ����ʧ�ܣ������µ������ʼ�����");
            //        }
            //    }
            //    catch (Exception ee)
            //    {
            //        this.tbCallNO.Enabled = false;
            //        this.tbPassNO.Enabled = true;
            //        this.tbBeginOrder.Enabled = true;
            //        this.tbEndOrder.Enabled = false;
            //        MessageBox.Show("�����ӿ��쳣��" + ee.Message + "�����µ������ʼ�����");
            //    }
            //    #endregion
            //}
            //else if (e.ClickedItem == this.tbEndOrder)
            //{
            //    #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
            //    int returnvalue = 0;
            //    StringBuilder cardno = new StringBuilder(10);
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(nurseStation))
            //        {
            //            cardno = new StringBuilder();
            //            char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(nurseStationChar, opercodeChar, ref cardno);
            //        }
            //        else
            //        {
            //            cardno = new StringBuilder();
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(deptcodeChar, opercodeChar, ref cardno);
            //        }
            //        if (returnvalue == 0)
            //        {
            //            this.tbCallNO.Enabled = true;
            //            this.tbPassNO.Enabled = false;
            //            this.tbBeginOrder.Enabled = false;
            //            this.tbEndOrder.Enabled = false;
            //        }
            //        else
            //        {
            //            this.tbCallNO.Enabled = false;
            //            this.tbPassNO.Enabled = false;
            //            this.tbBeginOrder.Enabled = false;
            //            this.tbEndOrder.Enabled = true;
            //            MessageBox.Show("��������ʧ�ܣ������µ�������������");
            //        }
            //    }
            //    catch (Exception ee)
            //    {
            //        this.tbCallNO.Enabled = false;
            //        this.tbPassNO.Enabled = false;
            //        this.tbBeginOrder.Enabled = false;
            //        this.tbEndOrder.Enabled = true;
            //        MessageBox.Show("�����ӿ��쳣��" + ee.Message + "�����µ�������������");
            //    }
            //    #endregion
            //}
            //else if (e.ClickedItem == this.tbPassNO)
            //{
            //    StringBuilder cardno = new StringBuilder(10);
            //    int returnvalue = 0;
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(nurseStation))
            //        {
            //            cardno = new StringBuilder();
            //            char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.SkipPatient(nurseStationChar, opercodeChar, ref cardno);
            //        }
            //        else
            //        {
            //            cardno = new StringBuilder();
            //            returnvalue = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.SkipPatient(deptcodeChar, opercodeChar, ref cardno);
            //        }
            //        if (returnvalue == 0)
            //        {
            //            this.tbCallNO.Enabled = true;
            //            this.tbPassNO.Enabled = false;
            //            this.tbBeginOrder.Enabled = false;
            //            this.tbEndOrder.Enabled = false;
            //        }
            //        else
            //        {
            //            this.tbCallNO.Enabled = false;
            //            this.tbPassNO.Enabled = true;
            //            this.tbBeginOrder.Enabled = true;
            //            this.tbEndOrder.Enabled = false;
            //            MessageBox.Show("����ʧ�ܣ������µ�������š���");
            //        }
            //    }
            //    catch (Exception ee)
            //    {
            //        this.tbCallNO.Enabled = false;
            //        this.tbPassNO.Enabled = true;
            //        this.tbBeginOrder.Enabled = true;
            //        this.tbEndOrder.Enabled = false;
            //        MessageBox.Show("�����ӿ��쳣��" + ee.Message + "�����µ�������š���");
            //    }
            //}
            #endregion
            else if (e.ClickedItem == this.tbGroup)//����
            {
                if (this.tbGroup.CheckState == CheckState.Checked)
                {
                    this.tbGroup.CheckState = CheckState.Unchecked;
                }
                else
                {
                    this.tbGroup.CheckState = CheckState.Checked;
                }

                if (this.tbGroup.CheckState == CheckState.Checked)
                {
                    this.ucOutPatientOrder1.SetEditGroup(true);
                    this.ucOutPatientOrder1.Patient = null;
                    this.initButtonGroup(true);

                }
                else
                {
                    this.ucOutPatientOrder1.SetEditGroup(false);
                    this.initButtonGroup(false);
                    this.panelTree.Visible = false;
                    this.panel2.Visible = false;
                }

            }
            else if (e.ClickedItem == this.tbHerbal)
            {
                this.ucOutPatientOrder1.HerbalOrder();
            }
            else if (e.ClickedItem == this.tbCheck)
            {
                this.ucOutPatientOrder1.AddTest();
            }

            else if (e.ClickedItem == this.tbDelOrder)//ɾ��
            {
                this.ucOutPatientOrder1.Del();
            }
            else if (e.ClickedItem == this.tbQueryOrder)//��ѯ
            {
                this.ucOutPatientOrder1.Patient = this.ucOutPatientTree1.PatientInfo;
                this.ucOutPatientOrder1.Retrieve();
            }
            else if (e.ClickedItem == this.tbPrintOrder)//��ӡ
            {
                this.ucOutPatientOrder1.PrintOrder();
            }
            else if (e.ClickedItem == this.tbComboOrder)//���
            {
                this.ucOutPatientOrder1.ComboOrder();
            }
            else if (e.ClickedItem == this.tbCancelOrder)//ȡ�����
            {
                this.ucOutPatientOrder1.CancelCombo();
            }
            else if (e.ClickedItem == this.tbExitOrder)//�˳�ҽ��
            {
                if (this.isEditGroup)
                {
                    if (this.tbGroup.CheckState == CheckState.Checked)
                    {
                        this.tbGroup.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        this.tbGroup.CheckState = CheckState.Checked;
                    }
                    this.ucOutPatientOrder1.SetEditGroup(false);
                    this.initButtonGroup(false);
                }
                else
                {
                    #region donggq----{31F003C4-528F-4f97-A52E-E23149688666}
                    if (DialogResult.Yes == MessageBox.Show("ȷ���˳���ҽ����������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {

                        ///ԭ���Ŀ�ʼ
                        if (this.ucOutPatientOrder1.ExitOrder() == 0)
                        {
                            this.initButton(false);
                            #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
                            this.tbCallNO.Enabled = false;
                            this.tbPassNO.Enabled = false;
                            #endregion
                        }
                        ///ԭ���Ľ���

                    }
                    else
                    {
                        return;
                    }
                    #endregion
                }
            }

            else if (e.ClickedItem == this.tbRegisterEmergencyPatient)//����
            {
                if (this.ucOutPatientOrder1.RegisterEmergencyPatient() < 0)
                {
                }
                else
                {
                    MessageBox.Show("���۳ɹ���");

                    ucOutPatientTree1.RefreshTreeView();
                    ucOutPatientTree1.RefreshTreePatientDone();
                }
            }
            //{1C0814FA-899B-419a-94D1-789CCC2BA8FF}
            else if (e.ClickedItem == this.tbOutEmergencyPatient) //����
            {
                if (this.ucOutPatientOrder1.OutEmergencyPatient() > 0)
                {
                    ucOutPatientTree1.RefreshTreeView();
                    ucOutPatientTree1.RefreshTreePatientDone();
                }

            }
            //{1C0814FA-899B-419a-94D1-789CCC2BA8FF}
            else if (e.ClickedItem == this.tbInEmergencyPatient) //תסԺ
            {
                if (this.ucOutPatientOrder1.InEmergencyPatient() > 0)
                {
                    ucOutPatientTree1.RefreshTreeView();
                    ucOutPatientTree1.RefreshTreePatientDone();
                }
            }
            else if (e.ClickedItem == this.tbSeePatient)//���
            {
                if (this.ucOutPatientTree1.DiagOut() < 0)
                {
                }
                else
                {
                    MessageBox.Show("����ɹ���");

                    ucOutPatientTree1.RefreshTreeView();
                    ucOutPatientTree1.RefreshTreePatientDone();
                }
            }
            else if (e.ClickedItem == this.tbRefreshPatient)//ˢ��
            {
                ucOutPatientTree1.RefreshTreeView();
                ucOutPatientTree1.RefreshTreePatientDone();
            }
            else if (e.ClickedItem == this.tbPatientTree)//�б�
            {
                this.neuPanel1.Visible = !this.neuPanel1.Visible;
            }
            else if (e.ClickedItem == this.tbSaveOrder)//����
            {
                //
                if (isEditGroup)
                {
                    SaveGroup();
                }
                else
                {
                    if (this.ucOutPatientOrder1.Save() == -1)
                    {

                    }
                    else
                    {
                        this.initButton(false);
                        ucOutPatientTree1.RefreshTreeView();

                        #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
                        //this.tbCallNO.Enabled = true;
                        //this.tbPassNO.Enabled = false;
                        //StringBuilder cardno = new StringBuilder(10);
                        //try
                        //{
                        //    if (!string.IsNullOrEmpty(nurseStation))
                        //    {
                        //        char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
                        //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(nurseStationChar, opercodeChar, ref cardno);
                        //    }
                        //    else
                        //    {
                        //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(deptcodeChar, opercodeChar, ref cardno);
                        //    }
                        //}
                        //catch (Exception ee)
                        //{
                        //    MessageBox.Show("RW�����ӿ��쳣��" + ee.Message);
                        //}
                        #endregion

                    }
                }
                this.statusBar1.Panels[1].Text = "";
            }
            else if (e.ClickedItem == this.tb1Exit)//�˳�
            {
                if (this.ucOutPatientOrder1.IsDesignMode) //���ڿ���״̬
                {
                    DialogResult result = MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ҽ��Ŀǰ���ڿ���ģʽ���Ƿ񱣴�?"), "��ʾ", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        #region {A06E32ED-0757-4802-AC9F-D1F9615E374C}ҽ��վ�кŹ���  by guanyx
                        //this.tbCallNO.Enabled = true;
                        //this.tbPassNO.Enabled = false;
                        //StringBuilder cardno = new StringBuilder(10);
                        //try
                        //{
                        //    if (!string.IsNullOrEmpty(nurseStation))
                        //    {
                        //        char[] nurseStationChar = ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.StringToChar(nurseStation);
                        //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(nurseStationChar, opercodeChar, ref cardno);
                        //    }
                        //    else
                        //    {
                        //        ZZlocal.Clinic.HISFC.OuterConnector.Triage.TriageManager.TreatFinish(deptcodeChar, opercodeChar, ref cardno);
                        //    }
                        //}
                        //catch (Exception ee)
                        //{
                        //    MessageBox.Show("RW�����ӿ��쳣��" + ee.Message);
                        //}
                        #endregion
                        if (this.ucOutPatientOrder1.Save() == 0)
                            this.Close();

                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else if (e.ClickedItem == this.tbDiseaseReport)     //  {E53A21A7-2B74-4b48-A9F4-9E05F8FA11A2} ��Ⱦ�����濨
            {
                if (this.dcpInstance == null)
                {
                    this.dcpInstance = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.DCP.IDCP)) as Neusoft.HISFC.BizProcess.Interface.DCP.IDCP;
                }

                if (this.dcpInstance != null)
                {
                    Neusoft.HISFC.Models.RADT.Patient patient = this.ucOutPatientTree1.PatientInfo as Neusoft.HISFC.Models.RADT.Patient;

                    this.dcpInstance.RegisterDiseaseReport(patient, Neusoft.HISFC.Models.Base.ServiceTypes.C);
                }
            }
            else if (e.ClickedItem == this.tbLisResultPrint)//{9B06773D-9674-4b20-94B8-47A1B066EA0B},��顢��������ѯ��shangxw 2009-11-10
            {
                Neusoft.HISFC.Models.Registration.Register patient = new Neusoft.HISFC.Models.Registration.Register();
                patient = this.ucOutPatientTree1.PatientInfo;
                //if continue?
                //DialogResult result = MessageBox.Show("�Ƿ��ӡLIS�����", "��ʾ", MessageBoxButtons.YesNo);
                //if (result == DialogResult.No)
                //{
                //    return;
                //}

                //Is "" or Null?
                if (patient == null || patient.PID.CardNO == "" || patient.PID.CardNO == null)
                {
                    MessageBox.Show("��ѡ��һ�����ߣ�");


                    return;
                }

                try
                {


                    #region ֣���޸�---{15C4A9D2-34AF-484b-B65B-BBD3CACABA5C}

                    rm_barprinter_common.In_rm_barprinter_common mobj = new rm_barprinter_common.COClass_n_rm_barprinter_commonClass();

                    if (!string.IsNullOrEmpty(patient.PID.CardNO))
                    {
                        mobj.uf_lis_result(patient.PID.CardNO, "1");
                    }
                    else
                    {
                        MessageBox.Show("Lis����ʧ��");
                        return;
                    }

                    //try
                    //{


                    //    string s = "LisResult";

                    //    System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName(s);
                    //    if (proc.Length > 0)
                    //    {
                    //        for (int i = 0; i < proc.Length; i++)
                    //        {
                    //            proc[i].Kill();
                    //        }
                    //    }

                    //    System.Diagnostics.Process p = new System.Diagnostics.Process();

                    //    p.StartInfo.FileName = Application.StartupPath + @"\LisBin\LisResult.exe";    //��Ҫ�����ĳ�����       
                    //    #region ȡ���ò���
                    //    ArrayList defaultValue = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("lis");
                    //    if ((defaultValue == null) || (defaultValue.Count == 0))
                    //    {
                    //        p.StartInfo.Arguments = " '" + patient.PID.CardNO + "' " + "����" + "";// +" " + "'סԺ'";//��������      

                    //    }
                    //    else
                    //    {
                    //        p.StartInfo.Arguments = " '" + defaultValue[0].ToString() + "' " + "����" + "";// +" " + "'סԺ'";//��������      

                    //    }

                    #endregion
                    //p.StartInfo.Arguments = " '" + defaultValue[0].ToString() + "' " + "����" + "";// +" " + "'סԺ'";//��������      

                    // p.Start();//����     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (e.ClickedItem == this.tbPacsResultPrint)//{17CC6DF8-1883-4d3c-8D24-2E08C93F047F},Lis�����ӡ,shangxw 2009-11-10
            {
                Neusoft.HISFC.Models.Registration.Register patient = new Neusoft.HISFC.Models.Registration.Register();
                patient = this.ucOutPatientTree1.PatientInfo;
                //if continue?
                //DialogResult result = MessageBox.Show("�Ƿ��ӡLIS�����", "��ʾ", MessageBoxButtons.YesNo);
                //if (result == DialogResult.No)
                //{
                //    return;
                //}

                //Is "" or Null?
                if (patient == null || patient.PID.CardNO == "" || patient.PID.CardNO == null)
                {
                    MessageBox.Show("��ѡ��һ�����ߣ�");


                    return;
                }

                try
                {
                    //string patientNo = patient.ID;
                    string patientNo = patient.PID.CardNO;
                    this.ucOutPatientOrder1.ShowPacsResultByPatient(patientNo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (e.ClickedItem == this.tbChooseDrugDept)
            {
                #region ����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
                this.ChooseDrugDept(true);
                #endregion
            }
            else if (e.ClickedItem == this.tbClinicCase)
            {
                #region ������Ӳ��� {54F5B3CC-25C6-4e93-B810-4721578DE378} wbo 2010-12-05
                ShowClinicCase();
                #endregion
            }
        }

        /// <summary>
        /// ������Ӳ��� {54F5B3CC-25C6-4e93-B810-4721578DE378} wbo 2010-12-05
        /// </summary>
        private void ShowClinicCase()
        {
            StringBuilder strPatientInfo = new StringBuilder("");
            try
            {
                #region ƴ��
                Neusoft.HISFC.Models.Registration.Register register = this.ucOutPatientTree1.PatientInfo;
                if (register == null || string.IsNullOrEmpty(register.ID))
                {
                    MessageBox.Show("���߻�����ϢΪ�գ�");
                    return;
                }
                /*
                Args[0]	����Ա����
                Args[1]	����Ա����
                Args[2]	����Ա���ڿ��Ҵ���
                Args[3]	����Ա���ڿ�������
                */
                strPatientInfo.Append(orderManager.Operator.ID + "\r\n");
                strPatientInfo.Append(((Neusoft.HISFC.Models.Base.Employee)orderManager.Operator).Name + "\r\n");
                strPatientInfo.Append(((Neusoft.HISFC.Models.Base.Employee)orderManager.Operator).Dept.ID + "\r\n");
                strPatientInfo.Append(((Neusoft.HISFC.Models.Base.Employee)orderManager.Operator).Dept.Name + "\r\n");
                /*
                 * Args[4]	PATIENT_ID	���˱�ʶ��	C	16	����Ψһ��ʶ�ţ��������û��������ĺ��壬�磺�����ţ�����ŵ�
                Args[5]	NAME	����	C	30	��������
                Args[6]	NAME_PHONETIC	����ƴ��	C	16	��������ƴ������д���ּ���һ���ո�ָ��������ض�
                Args[7]	SEX	�Ա�	C	8	�С�Ů��δ֪��ʹ������
                Args[8]	DATE_OF_BIRTH	��������	D	��	��
                Args[9]	BIRTH_PLACE	������	C	80	ָ��ʡ���أ�ʹ������
                Args[10]	CITIZENSHIP	����	C	28	ʹ�ù��Ҵ��룬ʹ������
                Args[11]	NATION	����	C	10	����淶���ƣ�ʹ������
                Args[12]	ID_NO	���֤��	C	20	
                 */
                strPatientInfo.Append(register.ID + "\r\n");
                strPatientInfo.Append(register.Name + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append(this.GetSex(register.Sex.ToString()) + "\r\n");
                strPatientInfo.Append(register.Birthday.ToString("yyyyMMdd") + "\r\n");
                strPatientInfo.Append(this.GetString(register.AddressHome) + "\r\n");
                strPatientInfo.Append("�й�" + "\r\n");
                strPatientInfo.Append(this.GetString(register.Nationality.Name) + "\r\n");
                strPatientInfo.Append(this.GetString(register.IDCard) + "\r\n");
                /*
                Args[13]	IDENTITY	���	C	16	����ݵǼ���ϵͳ���ɣ�����Ǽ���ϵͳ�ڰ�����Ժʱ���¡�ʹ�ù淶���ƣ����û�����ʹ������
                Args[14]	CHARGE_TYPE	�ѱ�	C	8	����ݵǼ���ϵͳ���ɣ�����Ǽ���ϵͳ�ڰ�����Ժʱ���¡�ʹ�ù淶���ƣ����û����壬ʹ������
                Args[15]	UNIT_IN_CONTRACT	��ͬ��λ	C	40	����������ڵ�λΪ��ҽԺ�ĺ�ͬ��λ����ϵ��λ����ʹ�ô��룬����Ϊ�ա�����ݵǼ���ϵͳ���ɣ�����Ǽ���ϵͳ�ڰ�����Ժʱ���¡�ʹ������
                Args[16]	MAILING_ADDRESS	ͨ�ŵ�ַ	C	80	ָ����ͨ�ŵ�ַ
                Args[17]	ZIP_CODE	��������	C	6	��Ӧͨ�ŵ�ַ����������
                Args[18]	PHONE_NUMBER_HOME	��ͥ�绰����	C	16	��2
                Args[19]	PHONE_NUMBER_BUSINESS	��λ�绰����	C	16	��
                Args[20]	NEXT_OF_KIN	��ϵ������	C	30	���˵���������
                Args[21]	RELATIONSHIP	����ϵ�˹�ϵ	C	16	���ޡ����ӵȣ�ʹ������
                Args[22]	NEXT_OF_KIN_ADDR	��ϵ�˵�ַ	C	80	��
                Args[23]	NEXT_OF_KIN_ZIP_CODE	��ϵ����������	C	6	��
                Args[24]	NEXT_OF_KIN_PHONE	��ϵ�˵绰����	C	20	��
                Args[25]	LAST_VISIT_DATE	�ϴξ�������	D	��	�ɹҺ���ԤԼ��ϵͳ���ݾ����¼��д
                Args[26]	VIP_INDICATOR	��Ҫ�����־	N	1	1-VIP 0-��VIP
                Args[27]	CREATE_DATE	��������	D	��	��
                Args[28]	OPERATOR	����Ա	C	8	����޸ı���¼�Ĳ���Ա����
                 */
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append(this.GetString(register.Pact.Name) + "\r\n");
                strPatientInfo.Append(this.GetString(register.AddressHome) + "\r\n");
                strPatientInfo.Append(this.GetString(register.PhoneHome) + "\r\n");
                strPatientInfo.Append(this.GetString(register.PhoneBusiness) + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append((register.VipFlag == true ? "1" : "0") + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append(((Neusoft.HISFC.Models.Base.Employee)orderManager.Operator).Name + "\r\n");
                /*
                Args[29]	PATIENT_ID	���˱�ʶ	C	16	�ǿ�
                Args[30]	VISIT_ID	���˱�������������	N	4	ȡ�û��߹ҺŴ���
                Args[31]	DEPT_ADMISSION_TO	����������	C	8	��ͳ��Ҫ��Ŀ��Ҵ��룬��2.6�����ֵ�
                Args[32]	ADMISSION_DATE_TIME	����������ڼ�ʱ��	D	��	��
                Args[33]	OCCUPATION	ְҵ	C	40	ʹ������
                Args[34]	MARITAL_STATUS	����״��	C	4	�ѻ顢δ�顢��顢ɥż��ʹ�ù淶���ƣ�
                Args[35]	IDENTITY	���	C	16	ʹ������
                Args[36]	ARMED_SERVICES	��	C	4	���
                Args[37]	DUTY	��	C	4	���
                Args[38]	UNIT_IN_CONTRACT	��ͬ��λ	C	40	������������ϵ��λ���ƣ��û����壬û�о����
                Args[39]	CHARGE_TYPE	�ѱ�	C	20	ʹ�ù淶����
                Args[40]	WORKING_STATUS	��ְ��־	N	1	0-��ְ 1-���� 2-����
                Args[41]	INSURANCE_TYPE	ҽ�����	C	16	����˲���Ϊҽ�����ˣ����¼��ӳ��������֧��������ҽ�����
                Args[42]	INSURANCE_NO	ҽ�Ʊ��պ�	C	18	����˲���Ϊҽ�����ˣ����¼�䱣�պ�
                Args[43]	SERVICE_AGENCY	������λ	C	80	��
                Args[44]	MAILING_ADDRESS	ͨ�ŵ�ַ	C	80	��
                Args[45]	ZIP_CODE	��������	C	10	��
                Args[46]	NEXT_OF_KIN	��ϵ������	C	30	���˵���������
                Args[47]	RELATIONSHIP	����ϵ�˹�ϵ	C	16	���ޡ����ӵȣ�ʹ������
                Args[48]	NEXT_OF_KIN_ADDR	��ϵ�˵�ַ	C	80	��
                Args[49]	NEXT_OF_KIN_ZIPCODE	��ϵ����������	C	6	��
                Args[50]	NEXT_OF_KIN_PHONE	��ϵ�˵绰	C	20	��
                 * Args[51]	INP_NO	סԺ��
                 */
                strPatientInfo.Append(this.GetString(register.ID) + "\r\n");
                strPatientInfo.Append("1" + "\r\n");
                strPatientInfo.Append(((Neusoft.HISFC.Models.Base.Employee)orderManager.Operator).Dept.ID + "\r\n");
                strPatientInfo.Append(this.GetString(register.DoctorInfo.SeeDate.ToString("yyyy-MM-dd HH:mm:ss")) + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append(this.GetString(register.Pact.Name) + "\r\n");
                strPatientInfo.Append(this.GetString(register.Pact.Name) + "\r\n");
                strPatientInfo.Append("0" + "\r\n");
                strPatientInfo.Append(this.GetString(register.Pact.Name) + "\r\n");
                strPatientInfo.Append(this.GetString(register.SSN) + "\r\n");
                strPatientInfo.Append(this.GetString(register.AddressBusiness) + "\r\n");
                strPatientInfo.Append(this.GetString(register.AddressHome) + "\r\n");
                strPatientInfo.Append(this.GetString(register.HomeZip) + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");
                strPatientInfo.Append("" + "\r\n");

                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡ���߻�����Ϣʧ�ܣ�" + e.ToString());
                return;
            }

            try
            {
                #region ��ʾ
                JHEMR.OutpatientEMREdit.Class1 jhEMR = new JHEMR.OutpatientEMREdit.Class1();
                jhEMR.MyDiagnose(strPatientInfo.ToString());
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show("���Ӳ�����ʾʧ�ܣ�" + e.ToString());
                return;
            }
        }


        /// <summary>
        /// ������Ӳ��� {54F5B3CC-25C6-4e93-B810-4721578DE378} wbo 2010-12-05
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        private string GetSex(string sex)
        {
            switch (sex)
            {
                case "M":
                    return "��";
                    break;
                case "F":
                    return "Ů";
                    break;
                default:
                    return "δ֪";
                    break;
            }
        }

        /// <summary>
        /// ������Ӳ��� {54F5B3CC-25C6-4e93-B810-4721578DE378} wbo 2010-12-05
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        private string GetString(object sourceStr)
        {
            if (sourceStr == null)
            {
                return "";
            }
            else
            {
                return sourceStr.ToString();
            }
        }

        /// <summary>
        /// ѡ��ҩ�� ����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
        /// </summary>
        /// <param name="isClear"></param>
        private void ChooseDrugDept(bool isClear)
        {
            try
            {
                if (isClear)
                {
                    this.ucOutPatientOrder1.Clear();
                }
                ZZLocal.HISFC.Components.Pharmacy.ucDrugDeptSelect ucDrugDept = new ZZLocal.HISFC.Components.Pharmacy.ucDrugDeptSelect();
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucDrugDept);
                this.SetToolBarText();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// ����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
        /// </summary>
        private void SetToolBarText()
        {
            if (HISFC.BizProcess.Integrate.Function.DrugDept != null)
            {
                this.tbChooseDrugDept.Text = "ѡ��ҩ��";
                this.tbChooseDrugDept.ToolTipText = "��ǰҩ����" + HISFC.BizProcess.Integrate.Function.DrugDept.Name + "�����ѡ������ҩ��";
            }
            else
            {
                this.tbChooseDrugDept.Text = "ѡ��ҩ��";
                this.tbChooseDrugDept.ToolTipText = "û��ָ��ҩ��������ѡ��ҩ��";
            }
        }

        private void SaveGroup()
        {
            Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager group = new Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager();
            group.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.C;
            try
            {
                group.IsManager = (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).IsManager;
            }
            catch
            { }

            ArrayList al = new ArrayList();
            for (int i = 0; i < this.ucOutPatientOrder1.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                //if (this.ucOutPatientOrder1.neuSpread1.ActiveSheet.IsSelected(i, 0))
                //{
                    Neusoft.HISFC.Models.Order.OutPatient.Order order = this.ucOutPatientOrder1.GetObjectFromFarPoint(i, this.ucOutPatientOrder1.neuSpread1.ActiveSheetIndex).Clone();
                    if (order == null)
                    {
                        MessageBox.Show("���ҽ������");
                    }
                    else
                    {
                        string s = order.Item.Name;
                        string sno = order.Combo.ID;
                        //����ҽ������ Ĭ�Ͽ���ʱ��Ϊ ���
                        order.BeginTime = new DateTime(order.BeginTime.Year, order.BeginTime.Month, order.BeginTime.Day, 0, 0, 0);
                        al.Add(order);
                    }
                //}
            }
            if (al.Count > 0)
            {
                group.alItems = al;
                group.ShowDialog();
                this.tvGroup.RefrshGroup();
            }
        }


        #region IPreArrange ��Ա   {B17077E6-7E65-45fb-BA25-F2883EB6BA27}

        public int PreArrange()
        {
            this.ucOutPatientTree1.InitControl();

            if (this.ucOutPatientTree1.RefreshTreeView() == -1)
            {
                return -1;
            }

            return 1;
        }

        #endregion

        #region IInterfaceContainer ��Ա    {E53A21A7-2B74-4b48-A9F4-9E05F8FA11A2}

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { typeof( Neusoft.HISFC.BizProcess.Interface.DCP.IDCP ) };
            }
        }

        #endregion
    }
}

