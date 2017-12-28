using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.WinForms.DrugStore
{
    /// <summary>
    /// Bed<br></br>
    /// [��������: �����䷢ҩ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-11]<br></br>
    /// <�޸ļ�¼
    ///	    Ϊ�˲��Կؼ�ʹ�� ϣ������ͨ�����ര�ڵ����������������洰��Tag Ŀǰ��������..
    ///  />
    /// </summary>
    public partial class ucOutpatientDrug : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucOutpatientDrug()
        {
            InitializeComponent();
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

        #endregion

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            Neusoft.FrameWork.Management.DataBaseManger dataBaseManager = new Neusoft.FrameWork.Management.DataBaseManger();

            if (this.isOtherDrugDept)
            {
                Neusoft.HISFC.BizProcess.Integrate.Manager integrateManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                System.Collections.ArrayList al = integrateManager.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
                Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
                if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(al, ref info) == 0)
                    return;
                else
                    this.OperDept = info;
            }
            else
            {
                this.OperDept = ((Neusoft.HISFC.Models.Base.Employee)dataBaseManager.Operator).Dept;
            }

            this.OperInfo = dataBaseManager.Operator;
            this.ApproveDept = ((Neusoft.HISFC.Models.Base.Employee)dataBaseManager.Operator).Dept;

            if (this.InitTerminal() == -1)
                return;

            this.InitControlParm();
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
                return -1;
            return 1;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected void InitControlParm()
        {
            this.ucClinicTree1.OperDept = this.OperDept;
            this.ucClinicTree1.OperInfo = this.OperInfo;
            this.ucClinicTree1.ApproveDept = this.OperDept;
            this.ucClinicTree1.SetFunMode(Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug);
            this.ucClinicTree1.SetTerminal(this.Terminal);

            this.ucClinicDrug1.OperDept = this.OperDept;
            this.ucClinicDrug1.OperInfo = this.OperInfo;
            this.ucClinicDrug1.ApproveDept = this.OperDept;
            this.ucClinicDrug1.SetFunMode(Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug);
            this.ucClinicDrug1.SetTerminal(this.Terminal);

            this.funMode = Neusoft.HISFC.Components.DrugStore.OutpatientFun.Drug;
        }

        #endregion

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            //this.statusBar1.Panels[1].Text = "���ڱ���...";

            base.OnStatusBarInfo(null, "���ڱ���...");

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
            try
            {
                this.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.OnLoad(e);
        }

        private void ucClinicTree1_MyTreeSelectEvent(Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe)
        {
            this.ucClinicDrug1.ShowData(drugRecipe);
        }

        private void ucClinicDrug1_EndSave(object sender, EventArgs e)
        {
            this.ucClinicTree1.ChangeNodeLocation();
        }

        //private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    if (e.ClickedItem == this.tsbExit)          //�˳�
        //    {
        //        if (this.EnableExit())
        //            this.Close();
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbSave)          //����
        //    {
        //        this.Save();
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbRefresh)       //ˢ��
        //    {
        //        this.ucClinicTree1.ShowList();
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbQuery)         //��ѯ
        //    {
        //        this.ucClinicTree1.FindNode();
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbPrint)         //��ӡ
        //    {
        //        this.ucClinicTree1.Print();
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbPause)         //��ͣ��ӡ
        //    {
        //        Neusoft.FrameWork.WinForms.Classes.Print.PausePrintJob(0);
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbRefreshWay)    //�ֹ�ˢ�� / �Զ�ˢ��
        //    {
        //        if (this.tsbRefreshWay.Checked)         //�ֹ�ˢ��״̬
        //        {
        //            this.ucClinicTree1.EndProcessRefresh();
        //            this.tsbRefreshWay.Text = "�Զ�ˢ��";
        //        }
        //        else
        //        {
        //            this.ucClinicTree1.BeginProcessRefresh(1000);
        //            this.tsbRefreshWay.Text = "�ֶ�ˢ��";
        //        }
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbRecipe)        //����
        //    {
        //        this.ucClinicDrug1.IsPrintRecipe = this.tsbRecipe.Checked;
        //        return;
        //    }
        //    if (e.ClickedItem == this.tsbDrugList)      //��ҩ�嵥
        //    {
        //        this.ucClinicDrug1.IsPrintListing = this.tsbDrugList.Checked;
        //        return;

        //    }
        //}

        Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("ˢ��", "ˢ���б���ʾ", 0, true, false, null);
            this.toolBarService.AddToolButton("��ͣ��ӡ", "��ͣ��ǰ��ӡ��", 0, true, false, null);
            this.toolBarService.AddToolButton("�ֹ�ˢ��", "�˳�Ӧ�ó������δ��ҩ����ʱ�������˳�", 0, true, false, null);
            this.toolBarService.AddToolButton("����", "��ҩͬʱ��ӡ����", 0, true, false, null);
            this.toolBarService.AddToolButton("��ҩ�嵥", "��ҩͬʱ��ӡ��ҩ�嵥", 0, true, false, null);

            return toolBarService;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "ˢ��":
                    this.ucClinicTree1.ShowList();
                    break;
                case "��ͣ��ӡ":
                    Neusoft.FrameWork.WinForms.Classes.Print.PausePrintJob(0);
                    break;
                case "����":
                    //this.ucClinicDrug1.IsPrintRecipe = this.tsbRecipe.Checked;
                    break;
                case "��ҩ�嵥":
                    //this.ucClinicDrug1.IsPrintListing = this.tsbDrugList.Checked;
                    break;
                case "�ֹ�ˢ��":
                    break;
            }
            base.ToolStrip_ItemClicked(sender, e);
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return 1;
        }

        protected override int OnQuery(object sender, object neuObject)
        {
            this.ucClinicTree1.FindNode();
            return 1;
        }

        protected override int OnPrint(object sender, object neuObject)
        {
            this.ucClinicTree1.Print();
            return 1;
        }

        private void ucClinicTree1_ProcessMessageEvent(object sender, string msg)
        {
            //this.statusBar1.Panels[1].Text = msg;

            base.OnStatusBarInfo(null, msg);
        }
    }
}
