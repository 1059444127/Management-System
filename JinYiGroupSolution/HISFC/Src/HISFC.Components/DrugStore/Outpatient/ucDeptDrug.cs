using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.DrugStore.Outpatient
{
    /// <summary>
    /// [��������: ������Ұ�ҩ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007-04]<br></br>
    ///		
    ///  />
    /// </summary>
    public partial class ucDeptDrug : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucDeptDrug()
        {
            InitializeComponent();

        }

        #region �����

        /// <summary>
        /// Ȩ����Ա
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privOper = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// Ȩ�޿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ����ҵ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();

        /// <summary>
        /// ҩƷ����ҵ����
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// �Ƿ��ӡ����
        /// </summary>
        private bool isPrintRecipe = true;

        /// <summary>
        /// ��ӡ��ǩʹ��
        /// </summary>
        private ucClinicTree uc = null;
        #endregion

        #region ����

        /// <summary>
        /// �Ƿ��ӡ����
        /// </summary>
        [Description("��ӡ��ť�Ƿ��ӡ���� ����ΪFalse��ӡ��ǩ"),Category("����"),DefaultValue(true)]
        public bool IsPrintRecipe
        {
            get
            {
                return this.isPrintRecipe;
            }
            set
            {
                this.isPrintRecipe = value;
            }
        }

        #endregion

        #region ��������Ϣ

        ///// <summary>
        ///// �����¼�ί��
        ///// </summary>
        //private event System.EventHandler ToolButtonClicked;
        /// <summary>
        /// ���幤��������
        /// </summary>
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="NeuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object NeuObject, object param)
        {
            //this.ToolButtonClicked += new EventHandler( ToolButton_clicked );
            //���ӹ�����
            this.toolBarService.AddToolButton("ȫѡ", "ѡ��ȫ������", 0, true, false, null);
            this.toolBarService.AddToolButton("ȫ��ѡ", "ȡ������ѡ��", 1, true, false, null);
            return this.toolBarService;
        }

        /// <summary>
        /// ��������ť�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "ȫѡ":
                    this.SetCheck(true);
                    break;
                case "ȫ��ѡ":
                    this.SetCheck(false);
                    break;
            }

        }

        protected override int OnPrint(object sender, object neuObject)
        {
            if (this.isPrintRecipe)
            {
                this.ucClinicDrug1.Print();
            }
            else
            {
                if (this.uc == null)
                {
                    this.uc = new ucClinicTree();
                    this.uc.Init();
                    this.uc.OperDept = this.privDept;
                    this.uc.OperInfo = this.privOper;
                }
                this.uc.terminal.ID = this.cmbSendTerminal.Tag.ToString();

                Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe = this.tvClinicTree1.SelectedNode.Tag as Neusoft.HISFC.Models.Pharmacy.DrugRecipe;

                List<Neusoft.HISFC.Models.Pharmacy.ApplyOut> alList = this.ucClinicDrug1.GetData();

                this.uc.Print(drugRecipe, new ArrayList(alList.ToArray()));
            }

            return base.OnPrint(sender, neuObject);
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        private int Init()
        {
            Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
            ArrayList al = deptManager.GetClinicDepartment();
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡ��������б�������") + deptManager.Err);
                return -1;
            }

            this.cmbRecipeDept.AddItems(al);

            ArrayList alTerminal = this.drugStoreManager.QueryDrugTerminalByDeptCode(this.privDept.ID, "0");
            if (alTerminal == null)
            {
                MessageBox.Show(Language.Msg("��ȡ���ⷿ��ҩ�ն��б�������"));
                return -1;
            }

            this.cmbSendTerminal.AddItems(alTerminal);

            this.cmbSendTerminal.SelectedIndex = 0;

            //if (Function.InitLabelPrintInterface() == -1)
            //{
            //    return -1;
            //}

            return 1;
        }

        private int InitPrintInstance()
        {
            object factoryInstance = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint)) as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;
            if (factoryInstance != null)
            {
                Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint = factoryInstance as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;

                if (Neusoft.HISFC.Components.DrugStore.Function.IDrugPrint == null)
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// ѡ���趨
        /// </summary>
        /// <param name="isCheck">�Ƿ�ѡ��</param>
        protected void SetCheck(bool isCheck)
        {
            foreach (TreeNode node in this.tvClinicTree1.Nodes)
            {
                node.Checked = isCheck;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        protected void Clear()
        {
            this.ucClinicDrug1.Clear();
        }

        /// <summary>
        /// ��ȡ��ǰѡ�л���
        /// </summary>
        /// <returns>�ɹ����ص�ǰѡ�л�����Ϣ ʧ�ܷ���null</returns>
        protected List<Neusoft.HISFC.Models.Pharmacy.DrugRecipe> GetCheckPatient()
        {
            List<Neusoft.HISFC.Models.Pharmacy.DrugRecipe> alCheckPatient = new List<Neusoft.HISFC.Models.Pharmacy.DrugRecipe>();
            //{021613AF-A133-46eb-9680-ACDBDF058FAE}  ���Ұ�ҩ  �Ӷ����ڵ��ȡ����
            foreach (TreeNode node in this.tvClinicTree1.Nodes[0].Nodes)
            {
                if (node.Checked)
                {
                    Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe = node.Tag as Neusoft.HISFC.Models.Pharmacy.DrugRecipe;

                    alCheckPatient.Add(drugRecipe);
                }
            }

            return alCheckPatient;
        }

        /// <summary>
        /// ���ݴ������һ�ȡ�����б�
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int QueryPatientList()
        {
            if (this.cmbRecipeDept.Tag == null || this.cmbRecipeDept.Tag.ToString() == "")
            {
                MessageBox.Show(Language.Msg("��ѡ�񴦷�����"));
                return -1;
            }

            ArrayList alList = this.drugStoreManager.QueryUnSendList(this.cmbRecipeDept.Tag.ToString());
            if (alList == null)
            {
                MessageBox.Show(Language.Msg("��ȡ�����б�������") + this.drugStoreManager.Err);
                return -1;
            }

            TreeNode rootNode = new TreeNode("ȡҩ�����б�", 0, 0);

            this.tvClinicTree1.ShowList(rootNode,alList, false,false);

            this.tvClinicTree1.ExpandAll();

            return 1;
        }

        /// <summary>
        /// ���ҷ�ҩ����
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int SavePatient()
        {
            List<Neusoft.HISFC.Models.Pharmacy.DrugRecipe> alPatient = this.GetCheckPatient();
            if (alPatient == null || alPatient.Count == 0)
            {
                return 1;
            }

            Neusoft.FrameWork.Models.NeuObject sentTerminal = new Neusoft.FrameWork.Models.NeuObject();
            if (this.cmbSendTerminal.Tag == null || this.cmbSendTerminal.Tag.ToString() == "")
            {
                MessageBox.Show(Language.Msg("��ѡ��ҩ�ն�"));
                return -1;
            }
            sentTerminal.ID = this.cmbSendTerminal.Tag.ToString();

            Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
            string state = "1";

            foreach (Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe in alPatient)
            {
                if (drugRecipe.RecipeState == "0" || drugRecipe.RecipeState == "1")
                {
                    state = "0";
                }
                else
                {
                    state = "1";
                }

                #region ���´���״̬

                int parm = this.drugStoreManager.UpdateDrugRecipeState(this.privDept.ID, drugRecipe.RecipeNO, "M1", "0", "1");
                if (parm == -1)
                {
                    MessageBox.Show(Language.Msg("����δ��ӡ״̬Ϊ�Ѵ�ӡ״̬ʧ��!") + this.drugStoreManager.Err);

                    this.QueryPatientList();

                    return -1;
                }

                #endregion

                ArrayList al = itemManager.QueryApplyOutListForClinic(this.privDept.ID, "M1", state, drugRecipe.RecipeNO);
                if (al == null)
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���ݵ�����Ϣ��ȡ������ϸ��Ϣ��������") + itemManager.Err);

                    this.QueryPatientList();

                    return -1;
                }

                if (Function.OutpatientSend(this.ConvertApplyOutToList(al), sentTerminal, this.privDept, this.privOper, true, true) == -1)
                {
                    MessageBox.Show(Language.Msg("�� " + drugRecipe.PatientName + " ���з�ҩ����ʱ��������"));

                    this.QueryPatientList();

                    return -1;
                }

            }

            MessageBox.Show(Language.Msg("����ɹ�"));

            this.QueryPatientList();

            return 1;
        }

        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        private List<Neusoft.HISFC.Models.Pharmacy.ApplyOut> ConvertApplyOutToList(ArrayList al)
        {
            List<Neusoft.HISFC.Models.Pharmacy.ApplyOut> alList = new List<Neusoft.HISFC.Models.Pharmacy.ApplyOut>();
            foreach (Neusoft.HISFC.Models.Pharmacy.ApplyOut info in al)
            {
                alList.Add(info);
            }

            return alList;
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            this.privOper = this.drugStoreManager.Operator;
            this.privDept = ((Neusoft.HISFC.Models.Base.Employee)this.drugStoreManager.Operator).Dept;

            this.Init();

            this.ucClinicDrug1.funModle = OutpatientFun.DirectSend;
            this.ucClinicDrug1.OperDept = this.privDept;
            this.ucClinicDrug1.OperInfo = this.privOper;            

            this.ucClinicDrug1.Init();

            this.tvClinicTree1.AfterCheck += new TreeViewEventHandler(tvClinicTree1_AfterCheck);

            if (this.InitPrintInstance() == -1)
            {
                MessageBox.Show("��ʼ�����ݴ�ӡʵ��ʧ��,���޷����е��ݴ�ӡ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            base.OnLoad(e);
        }

        void tvClinicTree1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.SavePatient();

            return base.OnSave(sender, neuObject);
        }

        protected override int OnQuery(object sender, object neuObject)
        {
            this.QueryPatientList();

            return base.OnQuery(sender, neuObject);
        }

        private void tvClinicTree1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                ArrayList alDeptDrug = this.itemManager.QueryClinicUnSendList(this.cmbRecipeDept.Tag.ToString());
                if (alDeptDrug == null)
                {
                    MessageBox.Show(Language.Msg("��ȡҩƷ������Ϣʧ��"));
                    return;
                }

                this.ucClinicDrug1.ShowData(alDeptDrug);
            }
            if (e.Node != null && e.Node.Tag != null)
            {
                Neusoft.HISFC.Models.Pharmacy.DrugRecipe drugRecipe = e.Node.Tag as Neusoft.HISFC.Models.Pharmacy.DrugRecipe;
                if (drugRecipe != null)
                {
                    this.ucClinicDrug1.ShowData(drugRecipe);
                }
            }
        }

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] printType = new Type[1];
                printType[0] = typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint);

                return printType;
            }
        }

        #endregion
    }
}
