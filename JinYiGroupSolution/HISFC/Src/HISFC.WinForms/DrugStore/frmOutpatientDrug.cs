using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;
using System.Collections;

namespace Neusoft.HISFC.WinForms.DrugStore
{
    /// <summary>
    /// Bed<br></br>
    /// [��������: �����䷢ҩ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-11]<br></br>
    /// <˵��
    ///	    1 ��Ҫͨ������Tag���봰�ڹ���  �������ö��Neusoft.HISFC.Components.DrugStore.OutpatientWinFun�ô�����
    ///     2 ��ҩ�ն˵�����ʼ�� ͨ�� ���ݿ� Job��ִ�� 
    ///     3 Ϊ�˿���ͬʱ��½һ����ҩ�նˣ�ʹ��Memo�ֶν����������ơ�MemoΪ��1�� ˵������ʹ����
    ///  />
    /// </summary>
    public partial class frmOutpatientDrug : Neusoft.FrameWork.WinForms.Forms.BaseStatusBar, Neusoft.FrameWork.WinForms.Classes.IPreArrange, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public frmOutpatientDrug()
        {
            InitializeComponent();

            this.ucClinicTree1.OperChangedEvent += new Neusoft.HISFC.Components.DrugStore.Outpatient.ucClinicTree.MyOperChangedHandler(ucClinicTree1_OperChangedEvent);

            this.ucClinicTree1.SaveRecipeEvent += new EventHandler(ucClinicTree1_SaveRecipeEvent);

            this.ucClinicDrug1.MessageEvent += new EventHandler(ucClinicDrug1_MessageEvent);

            this.FormClosed += new FormClosedEventHandler(frmOutpatientDrug_FormClosed);

            this.ProgressRun(true);
     
        }

        private void frmOutpatientDrug_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!this.isCancel)
            {
                if (this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug)
                {
                    Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();                    
                    this.Terminal = drugStoreManager.GetDrugTerminalById(this.Terminal.ID);
                    this.Terminal.Memo = "";
                    if (drugStoreManager.UpdateDrugTerminal(this.Terminal) == -1)
                    {
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������ҩ�ն�Ϊ���ñ��ʧ��"));
                        return;
                    }
                }
            }
        }

        private void ucClinicDrug1_MessageEvent(object sender, EventArgs e)
        {
            this.Msg = sender.ToString();

            this.ShowMsg();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            int iRes;
            System.Math.DivRem(this.iTimerTick, 2, out iRes);
            if (iRes == 0)
            {
                this.tsMsg.Text = "";
            }
            else
            {
                this.tsMsg.Text = this.msg;
            }

            this.iTimerTick++;
            if (this.iTimerTick == 11)
            {
                t.Stop();
                this.iTimerTick = 0;
                this.Msg = "";
            }
        }     

        #region �����

        /// <summary>
        /// �����������ն�����
        /// </summary>
        private Neusoft.HISFC.Components.DrugStore.OutpatientFun funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;

        /// <summary>
        /// �������� ���ε�½ѡ��Ŀ���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject OperDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ������Ա
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject OperInfo = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ��׼���� �ۿ���� 
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject ApproveDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// �����ն�
        /// </summary>
        private Neusoft.HISFC.Models.Pharmacy.DrugTerminal Terminal = new Neusoft.HISFC.Models.Pharmacy.DrugTerminal();

        /// <summary>
        /// ���ڹ���
        /// </summary>
        private Neusoft.HISFC.Components.DrugStore.OutpatientWinFun winFun = Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.��ҩ;

        /// <summary>
        /// �Ƿ�����ҩ����/��ҩ
        /// </summary>
        private bool isOtherDrugDept = false;

        /// <summary>
        /// ��ǰ�����Ƿ����ʧ��
        /// </summary>
        private bool isCancel = false;

        /// <summary>
        /// ʱ����
        /// </summary>
        private System.Windows.Forms.Timer t = null;

        /// <summary>
        /// ʱ����
        /// </summary>
        private int iTimerTick = 0;

        /// <summary>
        /// ��Ϣ��ʾ
        /// </summary>
        private string msg = "";

        #endregion

        #region  ����

        /// <summary>
        /// ���ڹ���
        /// </summary>
        public Neusoft.HISFC.Components.DrugStore.OutpatientWinFun WinFun
        {
            get
            {
                return this.winFun;
            }
            set
            {
                this.winFun = value;

                switch (value)
                {
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.��ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
                        this.isOtherDrugDept = false;
                        break;
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.��ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Send;
                        this.isOtherDrugDept = false;
                        break;
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.ֱ�ӷ�ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.DirectSend;
                        this.isOtherDrugDept = false;
                        break;
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.��ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Back;
                        this.isOtherDrugDept = false;
                        break;
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.����ҩ����ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
                        this.isOtherDrugDept = true;
                        break;
                    case Neusoft.HISFC.Components.DrugStore.OutpatientWinFun.����ҩ����ҩ:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Send;
                        this.isOtherDrugDept = true;
                        break;
                }
            }
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        public string Msg
        {
            set
            {
                this.msg = value;

                this.tsMsg.Text = value;
            }
        }

        #endregion

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public int Init()
        {
            Neusoft.FrameWork.Management.DataBaseManger dataBaseManager = new Neusoft.FrameWork.Management.DataBaseManger();

            this.OperDept = ((Neusoft.HISFC.Models.Base.Employee)dataBaseManager.Operator).Dept;

            if (this.isOtherDrugDept)
            {
                Neusoft.HISFC.BizProcess.Integrate.Manager integrateManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                System.Collections.ArrayList al = integrateManager.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
                foreach (Neusoft.HISFC.Models.Base.Department tempDept in al)
                {
                    if (tempDept.ID == this.OperDept.ID)
                    {
                        al.Remove(tempDept);
                        break;
                    }
                }
                Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
                if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(al, ref info) == 0)
                {
                    return -1;
                }
                else
                {
                    this.OperDept = info;
                }
            }
          
            this.OperInfo = dataBaseManager.Operator;
            this.ApproveDept = ((Neusoft.HISFC.Models.Base.Employee)dataBaseManager.Operator).Dept;

            if (this.InitTerminal() == -1)
            {
                return -1;
            }

            this.InitControlParm();

            return 1;
        }

        /// <summary>
        /// �ն˳�ʼ��  ��ʱд��ʹ����ҩ̨
        /// </summary>
        protected int InitTerminal()
        {
            if (this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug)
                this.Terminal = Neusoft.HISFC.Components.DrugStore.Function.TerminalSelect(this.OperDept.ID, Neusoft.HISFC.Models.Pharmacy.EnumTerminalType.��ҩ̨, true);
            else
                this.Terminal = Neusoft.HISFC.Components.DrugStore.Function.TerminalSelect(this.OperDept.ID, Neusoft.HISFC.Models.Pharmacy.EnumTerminalType.��ҩ����, true);

            if (this.Terminal == null)
            {
                return -1;
            }

            if (this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug)
            {
                if (this.Terminal.Memo == "1")
                {
                    DialogResult rs = MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���ն������������Ե�½�������ٴ�ʹ�ã���ȷ�ϵ�½����ҩ�ն���\n ע�⣺���ǿ�е�½�����������ҩ�嵥��ӡ���ң�"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (rs == DialogResult.No)
                    {
                        this.isCancel = true;
                        return -1;
                    }
                }
                Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();
                this.Terminal.Memo = "1";
                if (drugStoreManager.UpdateDrugTerminal(this.Terminal) == -1)
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������ҩ�ն�Ϊ�����ñ��ʧ��"));
                    this.isCancel = true;
                    return -1;
                }
            }

            this.statusBar1.Panels[3].Text = this.statusBar1.Panels[3].Text + " - " + this.OperDept.Name + this.Terminal.Name + "[" + this.Terminal.ID + "]";

            return 1;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected void InitControlParm()
        {
            this.ucClinicTree1.OperDept = this.OperDept;
            this.ucClinicTree1.OperInfo = this.OperInfo;
            this.ucClinicTree1.ApproveDept = this.ApproveDept;     //��׼���� ʵ�ʿۿ����
            this.ucClinicTree1.SetFunMode(this.funMode);
            this.ucClinicTree1.SetTerminal(this.Terminal);

            this.ucClinicTree1.IsFindToAdd = true;

            this.ucClinicDrug1.IsShowDrugSendInfo = false;      //����ʾ��/��ҩ��Ϣ

            this.ucClinicDrug1.OperDept = this.OperDept;
            this.ucClinicDrug1.OperInfo = this.OperInfo;
            this.ucClinicDrug1.ApproveDept = this.ApproveDept;     //��׼���� ʵ�ʿۿ����
            this.ucClinicDrug1.SetFunMode(this.funMode);
            this.ucClinicDrug1.SetTerminal(this.Terminal);
        }

        #endregion

        /// <summary>
        /// ��Ϣ��ʾ
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMsg()
        {
            t = new Timer();
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
            t.Start();      
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            this.statusBar1.Panels[1].Text = "���ڱ���...";

            this.ucClinicTree1.IsBusySave = true;

            this.ucClinicDrug1.Save();

            this.ucClinicTree1.IsBusySave = false;
        }

        /// <summary>
        /// �˳��ж� �Ƿ�����رմ���
        /// </summary>
        /// <returns></returns>
        public bool EnableExit()
        {
            if (this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug)
            {
                if (this.ucClinicTree1.SpareNode())
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����δ��ҩȷ�ϵĴ��� ������д��������ҩ ������ҩȷ�Ϻ��ٹرմ���"));
                    return false;
                }
            }
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
           
            base.OnLoad(e);

            //��д��仰�����޷����
            this.WindowState = FormWindowState.Maximized;           

            try
            {               
                //���Ʋ���������
                Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
                                             
                object factoryInstance = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory)) as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory;
                if (factoryInstance != null)
                {
                    Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory factory = factoryInstance as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory;

                    Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint = factory.GetInstance(this.Terminal);

                    if (Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint == null)
                    {
                        this.isCancel = true;
                    }
                }
                else
                {
                    //Ĭ�ϲ�������ʾ
                    //MessageBox.Show("δ���ô�������ӡ��ʵ�֣����޷����д������ݴ�ӡ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.isCancel = true;

                    //#region �����ȡ��ǩ��ʽ     

                    //try
                    //{
                    //    #region ��ǩ/�嵥��ӡ �ӿ�ʵ��

                    //    object[] o = new object[] { };
                    //    string factoryValue = ctrlIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Clinic_Print_Label, true, "Neusoft.Report.DrugStore.OutpatientBillPrint");

                    //    System.Runtime.Remoting.ObjectHandle objHandel = System.Activator.CreateInstance("Report", factoryValue, false, System.Reflection.BindingFlags.CreateInstance, null, o, null, null, null);
                    //    object oLabel = objHandel.Unwrap();

                    //    Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory factory = oLabel as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory;
                    //    if (factory != null)
                    //    {
                    //        Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint = factory.GetInstance(this.Terminal);
                    //    }

                    //    if (Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint == null)
                    //    {
                    //        this.isCancel = true;
                    //    }

                    //    #endregion
                       
                    //}
                    //catch (System.TypeLoadException ex)
                    //{
                    //    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    //    MessageBox.Show(Language.Msg("��ǩ�����ռ���Ч\n" + ex.Message));
                    //    this.isCancel = true;
                    //    return;
                    //}

                    //#endregion
                }

                object interfacePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint)) as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;
                if (interfacePrint != null)
                {
                    Neusoft.HISFC.Components.DrugStore.Outpatient.ucClinicDrug.RecipePrint = interfacePrint as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;

                    if (Neusoft.HISFC.Components.DrugStore.Outpatient.ucClinicDrug.RecipePrint == null)
                    {
                        this.isCancel = true;
                    }
                }
                else
                {
                    //Ĭ�ϲ�������ʾ
                    //MessageBox.Show("δ���ô�������ӡ��ʵ�֣����޷����д������ݴ�ӡ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.isCancel = true;

                    //#region ���ﴦ����ӡ�ӿ�ʵ��

                    //object[] o1 = new object[] { };
                    ////���ﴦ����ӡ�ӿ���ʵ��
                    //string recipeValue = ctrlIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Clinic_Print_Recipe, true, "Report.Order.ucRecipePrint");

                    ////����
                    //System.Runtime.Remoting.ObjectHandle objHandel1 = System.Activator.CreateInstance("Report", recipeValue, false, System.Reflection.BindingFlags.CreateInstance, null, o1, null, null, null);
                    //object oLabel1 = objHandel1.Unwrap();

                    //Neusoft.HISFC.Components.DrugStore.Outpatient.ucClinicDrug.RecipePrint = oLabel1 as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;

                    //if (Neusoft.HISFC.Components.DrugStore.Outpatient.ucClinicDrug.RecipePrint == null)
                    //{
                    //    this.isCancel = true;
                    //}

                    //#endregion
                }

                //�ؼ���ʼ�� ���ȵ����걾���ڳ�ʼ����Ϣ���ٵ��ÿؼ���ʼ��
                this.ucClinicTree1.Init();
                this.ucClinicDrug1.Init();

                //�б��ʼ��ˢ��
                this.ucClinicTree1.RefreshOperList(true);
                //����Ļ��ʾ�ӿ����� ��ҩ����ʹ��
                if (this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.Send || this.funMode == Neusoft.HISFC.Components.DrugStore.OutpatientFun.DirectSend)
                {
                    if (this.ucClinicTree1.IsShowFeeData)
                    {
                        this.ucClinicTree1.BeginLEDRefresh(true);
                    }
                }

                //���2��������Զ�ˢ�³��� ����������� ��ô�������ݴ���ӡʱ����ɴ򿪴��ڳ�ʱ���ӳ�
                //��������(�ô�ӡ���ʽ��ӡ��ǩʱ�����)
                if (!this.tsbRefreshWay.Checked)
                {
                    this.ucClinicTree1.BeginProcessRefresh(2000);
                }

                this.ucClinicTree1.SetFocus();
            
                //������ͣ��ť
                this.tsbPause.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.isCancel = true;
            }            
        }

        private void ucClinicTree1_MyTreeSelectEvent(Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe)
        {
            this.ucClinicDrug1.ShowData(drugRecipe);
        }

        private void ucClinicDrug1_EndSave(object sender, EventArgs e)
        {
            this.ucClinicTree1.ChangeNodeLocation();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {            
            if (e.ClickedItem == this.tsbExit)          //�˳�
            {
                if (this.EnableExit())
                {
                    this.Close();
                }
                return;
            }        

            if (e.ClickedItem == this.tsbSave)          //����
            {
                this.Save();
                return;
            }
            if (e.ClickedItem == this.tsbRefresh)       //ˢ��
            {
                this.ucClinicDrug1.Clear();

                this.ucClinicTree1.ShowList();
                return;
            }
            if (e.ClickedItem == this.tsbQuery)         //��ѯ
            {
                this.ucClinicTree1.FindNode();
                return;
            }
            if (e.ClickedItem == this.tsbPrint)         //��ӡ
            {
                if (this.tsbRecipe.Checked || this.tsbDrugList.Checked)
                {
                    this.ucClinicDrug1.Print();
                }
                else
                {
                    this.ucClinicTree1.Print();
                }

                return;
            }
            if (e.ClickedItem == this.tsbPause)         //��ͣ��ӡ
            {
                Neusoft.FrameWork.WinForms.Classes.Print.PausePrintJob(0);
                return;
            }
            if (e.ClickedItem == this.tsbRefreshWay)    //�ֹ�ˢ�� / �Զ�ˢ��
            {
                this.tsbRefreshWay.Checked = !this.tsbRefreshWay.Checked;

                if (this.tsbRefreshWay.Checked)         //�ֹ�ˢ��״̬
                {
                    this.ucClinicTree1.IsAutoPrint = false;
                    this.tsbRefreshWay.Text = "�Զ���ӡ";

                    this.statusBar1.Panels[1].Text = "�ֹ���ӡ״̬ ֹͣ�Զ���ӡ";
                }
                else
                {
                    this.ucClinicTree1.IsAutoPrint = true;
                    this.tsbRefreshWay.Text = "�ֶ���ӡ";
                    this.statusBar1.Panels[1].Text = "�Զ���ӡ״̬..." + (this.ucClinicTree1.IsBusySave ? "���ڱ���" : "����ˢ��");
                   
                }

                //if (this.tsbRefreshWay.Checked)         //�ֹ�ˢ��״̬
                //{
                //    this.ucClinicTree1.EndProcessRefresh();
                //    this.tsbRefreshWay.Text = "�Զ�ˢ��";

                //    this.statusBar1.Panels[1].Text = "�ֹ�ˢ��״̬ ֹͣ�Զ�ˢ��";
                //}
                //else
                //{
                //    this.ucClinicTree1.BeginProcessRefresh(1000);
                //    this.tsbRefreshWay.Text = "�ֹ�ˢ��";
                //}

                return;
            }
            if (e.ClickedItem == this.tsbRecipe)        //����
            {
                this.tsbRecipe.Checked = !this.tsbRecipe.Checked;

                this.ucClinicDrug1.IsPrintRecipe = this.tsbRecipe.Checked;
                return;
            }
            if (e.ClickedItem == this.tsbDrugList)      //��ҩ�嵥
            {
                this.tsbDrugList.Checked = !this.tsbDrugList.Checked;

                this.ucClinicDrug1.IsPrintListing = this.tsbDrugList.Checked;
                return;

            }
        }

        private void ucClinicTree1_ProcessMessageEvent(object sender, string msg)
        {
            try
            {
                if (this.statusBar1.Panels.Count > 0)
                {
                    this.statusBar1.Panels[1].Text = msg;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ucClinicTree1_OperChangedEvent(Neusoft.FrameWork.Models.NeuObject oper)
        {
            this.ucClinicDrug1.OperInfo = oper;
        }

        private void ucClinicTree1_SaveRecipeEvent(object sender, EventArgs e)
        {
            //�������Ա���Żس����� �������β�������¼�
            this.Save();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Divide)                         //�˳�
            {
                this.toolStrip1_ItemClicked(null,new ToolStripItemClickedEventArgs(this.tsbExit));

                return true;
            }
            if (keyData == Keys.E )     //����
            {
                this.toolStrip1_ItemClicked(null,new ToolStripItemClickedEventArgs(this.tsbSave));

                return true;
            }
            if (keyData == Keys.P)
            {
                this.toolStrip1_ItemClicked(null,new ToolStripItemClickedEventArgs(this.tsbPrint));

                return true;
            }
            if (keyData == Keys.Add)
            {
                this.toolStrip1_ItemClicked(null,new ToolStripItemClickedEventArgs(this.tsbRefresh));

                return true;
            }
            if (keyData == Keys.Subtract)
            {
                this.toolStrip1_ItemClicked(null,new ToolStripItemClickedEventArgs(this.tsbPause));

                return true;                
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// �ر�ʱ������Դ����  {1367F373-862B-4ff5-A14A-F0DB46092776}
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            try
            {
                //ֹͣ��ǰ�߳�
                if (this.ucClinicTree1 != null)
                {
                    this.ucClinicTree1.EndProcessRefresh();
                }

            }
            catch
            {
            }

            base.OnClosed(e);
        }


        #region IPreArrange ��Ա

        bool isPreArrange = false;

        public int PreArrange()
        {
            this.isPreArrange = true;

            #region ���ݴ��ڲ��� ���ô��ڹ���

            if (this.Tag != null)
            {
                switch (this.Tag.ToString().ToUpper())
                {
                    case "DRUG":        //��ҩ
                    case "ODRUG":       //����ҩ����ҩ
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
                        this.Text = "������ҩ";
                        break;
                    case "SEND":        //��ҩ
                    case "OSEND":       //����ҩ����ҩ
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Send;
                        this.Text = "���﷢ҩ";
                        break;
                    case "BACK":        //��ҩ
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Back;
                        this.Text = "���ﻹҩ";
                        break;
                    case "DSEND":       //ֱ�ӷ�ҩ (��������ҩ)
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.DirectSend;
                        this.Text = "����ֱ�ӷ�ҩ";
                        break;
                     //��������ҩ���䡢��ҩ���� ʵ����֤�ô�����
                    //case "ODRUG":       //����ҩ����ҩ
                    //    this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
                    //    this.isOtherDrugDept = true;
                    //    this.Text = "����ҩ����ҩ";
                    //    break;
                    //case "OSEND":       //����ҩ����ҩ
                    //    this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Send;
                    //    this.isOtherDrugDept = true;
                    //    this.Text = "����ҩ����ҩ";
                    //    break;
                    default:
                        this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
                        this.Text = "������ҩ";
                        break;
                }
            }

            #endregion

            //�����ڻ�����Ϣ��ȡ ���ؼ���Ϣ��ֵ
            if (this.Init() == -1)
            {
                this.isCancel = true;
                return -1;
            }

            return 1;
        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] printType = new Type[2];
                printType[0] = typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IOutpatientPrintFactory);
                printType[1] = typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint);

                return printType;
            }
        }

        #endregion
    }
}