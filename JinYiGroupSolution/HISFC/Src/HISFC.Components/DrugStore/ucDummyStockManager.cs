using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;
using Neusoft.FrameWork.Function;

namespace Neusoft.HISFC.Components.DrugStore
{
    /// <summary>
    /// [��������: �������]<br></br>
    /// [�� �� ��: liangjz]<br></br>
    /// [����ʱ��: 2007-09]<br></br>
    /// <�޸ļ�¼ 
    ///		�޸���='' 
    ///		�޸�ʱ��='yyyy-mm-dd' 
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucDummyStockManager : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucDummyStockManager()
        {
            InitializeComponent();
        }

        #region �����

        /// <summary>
        /// ҵ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// Ȩ�޿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// �������Ұ�����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper produceHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ���Ұ�����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper deptHelpre = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ��������
        /// </summary>
        private Neusoft.HISFC.Models.Base.ServiceTypes type = Neusoft.HISFC.Models.Base.ServiceTypes.C;

        /// <summary>
        /// �����������ݱ�
        /// </summary>
        private DataTable dtDeptPreStock = null;

        /// <summary>
        /// ��������������ͼ
        /// </summary>
        private DataView dvDeptPreStock = null;
        #endregion

        #region ����

        /// <summary>
        /// ��������
        /// </summary>
        [Description("�����б�������� C ���� I סԺ"),Category("����")]
        public Neusoft.HISFC.Models.Base.ServiceTypes Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        protected DateTime DtBegin
        {
            get
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtpBegin.Text);
            }
        }

        /// <summary>
        /// ��ֹʱ��
        /// </summary>
        protected DateTime DtEnd
        {
            get
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtpEnd.Text);
            }
        }

        #endregion

        #region ��������Ϣ

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
            return this.toolBarService;
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnSave(object sender, object neuObject)
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                this.SavePatientPreStock();
            }
            else
            {
                this.SaveDeptPreStock();
            }

            return 1;
        }

        protected override int OnQuery(object sender, object neuObject)
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                if (this.type == Neusoft.HISFC.Models.Base.ServiceTypes.C)
                {
                    this.ShowOutPatientTree();
                }
                else
                {
                    this.ShowInPatientTree();
                }
            }
            else
            {
                this.ShowDeptPreStock();
            }

            return base.OnQuery(sender, neuObject);
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

                    break;
                case "ȫ��ѡ":
                    break;

            }

        }

        #endregion

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int Init()
        {
            Neusoft.HISFC.BizLogic.Pharmacy.Constant consManager = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
            ArrayList alProduce = consManager.QueryCompany("0");
            if (alProduce == null)
            {
                MessageBox.Show(Language.Msg("��ȡ���������б�������" + consManager.Err));
                return -1;
            }
            this.produceHelper = new Neusoft.FrameWork.Public.ObjectHelper(alProduce);

            this.privDept = ((Neusoft.HISFC.Models.Base.Employee)consManager.Operator).Dept;

            Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            ArrayList alDept = managerIntegrate.GetDeptmentAllValid();
            this.deptHelpre = new Neusoft.FrameWork.Public.ObjectHelper(alDept);

            this.InitDataSet();

            DateTime sysTime = consManager.GetDateTimeFromSysDateTime();

            this.dtpEnd.Value = sysTime.Date.AddDays(1);
            this.dtpBegin.Value = sysTime.Date.AddDays(-7);

            this.tvPatient.ImageList = this.tvPatient.deptImageList;

            return 1;
        }

        /// <summary>
        /// DataSet��ʼ��
        /// </summary>
        /// <returns></returns>
        protected void InitDataSet()
        {
            this.dtDeptPreStock = new DataTable();

            System.Type dtStr = System.Type.GetType("System.String");
            System.Type dtDec = System.Type.GetType("System.Decimal");
            System.Type dtDTime = System.Type.GetType("System.DateTime");

            this.dtDeptPreStock.Columns.AddRange(new DataColumn[] {
														new DataColumn("ҩƷ����",    dtStr),//0
														new DataColumn("���",        dtStr),//1
                                                        new DataColumn("���ۼ�",      dtDec),
														new DataColumn("��������",    dtStr),//2
														new DataColumn("ʵ�ʿ��",    dtDec),//3
                                                        new DataColumn("ԭԤ����",    dtDec),
														new DataColumn("Ԥ�ۿ��",    dtDec),//5
														new DataColumn("��λ",        dtStr),//6
                                                        new DataColumn("ҩƷ����",    dtStr),//7
														new DataColumn("ƴ����",      dtStr),//8
														new DataColumn("�����",      dtStr),//9	
														new DataColumn("�Զ�����",    dtStr),//10
                    								});

            this.deptSpread_Sheet1.DataSource = this.dtDeptPreStock;
        }

        #endregion

        /// <summary>
        /// �����ݼ������ݱ�
        /// </summary>
        /// <param name="storage">�������</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int AddDataToDataTable(Neusoft.HISFC.Models.Pharmacy.Storage storage)
        {
            DataRow row = this.dtDeptPreStock.NewRow();
            try
            {
                row["ҩƷ����"] = storage.Item.Name;
                row["���"] = storage.Item.Specs;
                row["���ۼ�"] = storage.Item.PriceCollection.RetailPrice;
                if (storage.Producer.ID != "")
                {
                    row["��������"] = this.produceHelper.GetName(storage.Producer.ID);
                }
                row["ʵ�ʿ��"] = storage.StoreQty;
                row["ԭԤ����"] = storage.PreOutQty;
                row["Ԥ�ۿ��"] = storage.PreOutQty;

                row["��λ"] = storage.Item.MinUnit;

                row["ҩƷ����"] = storage.Item.ID;                                                               
                row["ƴ����"] = storage.Item.NameCollection.SpellCode;
                row["�����"] = storage.Item.NameCollection.WBCode;
                row["�Զ�����"] = storage.Item.UserCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.Msg("���ݿ����Ϣ�������н��и�ֵʱ��������!") + ex.Message);
                return -1;
            }

            this.dtDeptPreStock.Rows.Add(row);

            return 1;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int ShowDeptPreStock()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Language.Msg("���ڼ��ؿ��ҿ����Ϣ,���Ժ�..."));
            Application.DoEvents();

            ArrayList alStock = this.itemManager.QueryStockinfoList(this.privDept.ID);
            if (alStock == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(Language.Msg("��ѯ���ҿ�������Ϣ��������") + this.itemManager.Err);
                return -1;
            }

            this.deptSpread_Sheet1.Rows.Count = 0;
            foreach (Neusoft.HISFC.Models.Pharmacy.Storage info in alStock)
            {
                this.AddDataToDataTable(info);
            }

            this.dtDeptPreStock.AcceptChanges();

            this.dvDeptPreStock = this.dtDeptPreStock.DefaultView;

            this.deptSpread_Sheet1.DataSource = this.dvDeptPreStock;

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            return 1;
        }

        /// <summary>
        /// �������������
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int SaveDeptPreStock()
        {
            this.deptSpread.StopCellEditing();

            this.dvDeptPreStock.RowFilter = "1=1";
            for (int i = 0; i < this.dvDeptPreStock.Count; i++)
            {
                this.dvDeptPreStock[i].EndEdit();
            }

            DataTable dtModify = this.dtDeptPreStock.GetChanges(DataRowState.Modified);
            if (dtModify == null || dtModify.Rows.Count <= 0)
            {
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();

            this.itemManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            foreach (DataRow dr in dtModify.Rows)
            {
                decimal storeQty = NConvert.ToDecimal(dr["ʵ�ʿ��"]);
                decimal preQty = NConvert.ToDecimal(dr["Ԥ�ۿ��"]);
                decimal originalQty = NConvert.ToDecimal(dr["ԭԤ����"]);

                if (preQty > storeQty)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Language.Msg("���治�ܽ��С���" + dr["ҩƷ����"].ToString() + "�����������ܴ���ʵ�ʿ������"), "��ʾ");
                    return -1;
                }

                //�������ģʽ��� �˴�δ���ü��޸�

                //if (this.itemManager.UpdateStockinfoPreOutNum(this.privDept.ID, dr["ҩƷ����"].ToString(), preQty - originalQty) == -1)
                //{
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //    MessageBox.Show(Language.Msg("������� ���¿��ʧ��") + this.itemManager.Err);
                //    return -1;
                //}
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            MessageBox.Show(Language.Msg("����ɹ�"));

            return 1;
        }

        /// <summary>
        /// �����ڼ���ڵ�
        /// </summary>
        /// <param name="alNodePatient">��ҩ���뻼����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int AddDataToTree(List<Neusoft.FrameWork.Models.NeuObject> alNodePatient)
        {
            this.tvPatient.Nodes.Clear();

            string preDeptCode = "";
            TreeNode deptNode = null;           //���ҽڵ�

            foreach (Neusoft.FrameWork.Models.NeuObject info in alNodePatient)
            {
                if (info.Memo == preDeptCode)          //ͬһ����
                {
                    TreeNode node = new TreeNode(info.Name);
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 5;
                    node.Tag = info;

                    deptNode.Nodes.Add(node);
                }
                else
                {
                    deptNode = new TreeNode(this.deptHelpre.GetName(info.Memo));
                    deptNode.ImageIndex = 0;
                    deptNode.SelectedImageIndex = deptNode.ImageIndex;

                    deptNode.Tag = null;

                    this.tvPatient.Nodes.Add(deptNode);

                    TreeNode node = new TreeNode(info.Name);
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 5;
                    node.Tag = info;

                    deptNode.Nodes.Add(node);
                }
            }

            return 1;
        }

        /// <summary>
        /// ��ʾסԺ��������Ԥ���б�
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int ShowInPatientTree()
        {
            List<Neusoft.FrameWork.Models.NeuObject> alInPatientApply = this.itemManager.QueryInPatientApplyOutList(this.privDept.ID, this.DtBegin, this.DtEnd, "0");
            if (alInPatientApply == null)
            {
                MessageBox.Show(Language.Msg("��ȡסԺ���������б�������") + this.itemManager.Err);
                return -1;
            }

            this.AddDataToTree(alInPatientApply);

            return 1;
        }

        /// <summary>
        /// ��ʾ���ﻼ������Ԥ���б�
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int ShowOutPatientTree()
        {
            List<Neusoft.FrameWork.Models.NeuObject> alOutPatientApply = this.itemManager.QueryOutPatientApplyOutList(this.privDept.ID, this.DtBegin, this.DtEnd, "0","1");
            if (alOutPatientApply == null)
            {
                MessageBox.Show(Language.Msg("��ȡ���ﻼ�������б�������") + this.itemManager.Err);
                return -1;
            }

            this.AddDataToTree(alOutPatientApply);

            return 1;
        }

        /// <summary>
        /// ������Ԥ��ҩƷ������Ϣ����Fp
        /// </summary>
        /// <param name="applyOut">Ԥ��������Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int AddDataToFp(Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut)
        {
            int rowCount = this.patientSpread1_Sheet1.Rows.Count;

            this.patientSpread1_Sheet1.Rows.Add(rowCount, 1);

            this.patientSpread1_Sheet1.Cells[rowCount, 0].Value = false;
            this.patientSpread1_Sheet1.Cells[rowCount, 1].Text = applyOut.Item.Name;
            this.patientSpread1_Sheet1.Cells[rowCount, 2].Text = applyOut.Item.Specs;
            this.patientSpread1_Sheet1.Cells[rowCount, 3].Text = applyOut.Item.PriceCollection.RetailPrice.ToString();
            this.patientSpread1_Sheet1.Cells[rowCount, 4].Text = applyOut.Days.ToString();
            this.patientSpread1_Sheet1.Cells[rowCount, 5].Text = applyOut.Operation.ApplyQty.ToString();
            this.patientSpread1_Sheet1.Cells[rowCount, 6].Text = applyOut.Item.MinUnit;
            this.patientSpread1_Sheet1.Cells[rowCount, 7].Text = System.Math.Round((applyOut.Operation.ApplyQty * applyOut.Days * applyOut.Item.PriceCollection.RetailPrice / applyOut.Item.PackQty), 2).ToString();
            this.patientSpread1_Sheet1.Cells[rowCount, 8].Text = applyOut.Operation.ApplyOper.OperTime.ToString();

            this.patientSpread1_Sheet1.Rows[rowCount].Tag = applyOut;

            return 1;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int ShowPatientPreStock(string patientID,string applyDept)
        {
            this.patientSpread1_Sheet1.Rows.Count = 0;

            ArrayList alApplyOut = this.itemManager.GetPatientApply(patientID, this.privDept.ID, applyDept,this.DtBegin, this.DtEnd, "0");
            if (alApplyOut == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������ҩ������Ϣ��������" + this.itemManager.Err));
                return -1;
            }            

            foreach (Neusoft.HISFC.Models.Pharmacy.ApplyOut info in alApplyOut)
            {
                this.AddDataToFp(info);
            }

            return 1;
        }

        /// <summary>
        /// �������������
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int SavePatientPreStock()
        {
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();

            this.itemManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            bool isHaveChecked = false;
            for(int i = 0;i < this.patientSpread1_Sheet1.Rows.Count;i++)
            {
                if (NConvert.ToBoolean(this.patientSpread1_Sheet1.Cells[i, 0].Value))
                {
                    Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut = this.patientSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Pharmacy.ApplyOut;

                    if (this.itemManager.UpdateStockinfoPreOutNum(applyOut,-applyOut.Operation.ApplyQty ,applyOut.Days) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(Language.Msg("������� ���¿��ʧ��") + this.itemManager.Err);
                        return -1;
                    }

                    isHaveChecked = true;
                }
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            if (isHaveChecked)
            {
                MessageBox.Show(Language.Msg("����ɹ�"));
            }
            else
            {
                MessageBox.Show(Language.Msg("��ѡ���豣��Ļ������ҩƷ"));
            }

            return 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper() != "DEVENV")
            {
                this.Init();
            }

            base.OnLoad(e);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (this.dtDeptPreStock.Rows.Count <= 0)
            {
                return;
            }

            if (this.dvDeptPreStock == null)
            {
                return;
            }

            try
            {
                string queryCode = "%" + this.txtFilter.Text.Trim() + "%";
                string filterStr = string.Format("ƴ���� like '{0}' or ����� like '{0}' or �Զ����� like '{0}' or ҩƷ���� like '{0}'", queryCode);
                
                this.dvDeptPreStock.RowFilter = filterStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.deptSpread.Focus();
                this.deptSpread_Sheet1.ActiveRowIndex = 0;
                this.deptSpread_Sheet1.ActiveColumnIndex = 6;
            }
        }

        private void tvPatient_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                Neusoft.FrameWork.Models.NeuObject info = e.Node.Tag as Neusoft.FrameWork.Models.NeuObject;

                this.ShowPatientPreStock(info.ID, info.Memo);
            }
        }
    }
}
