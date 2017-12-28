using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.DrugStore.Inpatient
{
    /// <summary>
    /// [��������: סԺ��ҩ��ѯ {F667C43C-FA2B-4c94-843D-5C540B6F06F7}]<br></br>
    /// [�� �� ��: ��ú�]<br></br>
    /// [�޸�ʱ��: 2010-11-17]<br></br>
    /// <�޸ļ�¼>
    ///    1.����סԺ���з�ҩ��� by Sunjh 2010-11-17 {F667C43C-FA2B-4c94-843D-5C540B6F06F7}
    /// </�޸ļ�¼>
    /// </summary>
    public partial class ucSendDrugByNurse : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucSendDrugByNurse()
        {
            InitializeComponent();
 

            //Ĭ�ϲ���ʾ�˷���Ϣ/��������Ϣ
            this.IsShowQuitBill = false;
            this.IsShowOutBill = false;
        }

        #region ö��

        /// <summary>
        /// �б�ڵ��������
        /// </summary>
        public enum NodeType
        {
            /// <summary>
            /// ����
            /// </summary>
            Patient,
            /// <summary>
            /// ȡҩ����
            /// </summary>
            Dept
        }

        /// <summary>
        /// ȡҩ�������� 
        /// </summary>
        public enum ReciveDrugType
        {
            /// <summary>
            /// ����
            /// </summary>
            Dept,
            /// <summary>
            /// ����վ
            /// </summary>
            NurseCell
        }

        #endregion

        #region �����

        /// <summary>
        /// ҩ��������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();

        /// <summary>
        /// ҩ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// ������Ϣ������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager departmentManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        /// <summary>
        /// ���Ʋ���������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

        /// <summary>
        /// ���ڵ����� 0 ����վ Patient ��ʾ���� 1 ���� Dept ȡҩ����
        /// </summary>
        protected NodeType treeType = NodeType.Patient;

        /// <summary>
        /// ȡҩ��������
        /// </summary>
        protected ReciveDrugType reciveType = ReciveDrugType.Dept;

        /// <summary>
        /// סԺ�Ų�ѯ
        /// </summary>
        protected string InPatientNo = "";

        /// <summary>
        /// ҩƷ����
        /// </summary>
        protected DataSet QualityDataSet = new DataSet();

        /// <summary>
        /// ��������ѯ
        /// </summary>
        protected DataSet DeptDataSet = new DataSet();

        /// <summary>
        /// ����վ��ѯ��DataSet
        /// </summary>
        protected DataSet NurseDtaSet = new DataSet();

        /// <summary>
        /// ��ѯ���� �ɲ�ѯ������վ�����п���
        /// </summary>
        private ArrayList deptInfo = null;

        /// <summary>
        /// ������
        /// </summary>
        protected Neusoft.FrameWork.Public.ObjectHelper deptHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ��Ա������
        /// </summary>
        protected Neusoft.FrameWork.Public.ObjectHelper personHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ��ǰ����Ա��Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee operVar = null;

        /// <summary>
        /// ��ǰ��ѯ����������
        /// </summary>
        private object nowObj = null;

        private string billClassName = "";

        /// <summary>
        /// ��ǰѡ���ѯ�Ĳ���
        /// </summary>
        private int showLevel = 0;

        /// <summary>
        /// �Ƿ񰴰�ҩ��������ʾ
        /// </summary>
        private bool isShowDrugBill = true;

        /// <summary>
        /// �Ƿ񰴲����Ų�ѯ
        /// </summary>
        private bool isQueryByCaseNO = false;

        private Hashtable hsSelected = new Hashtable();

        /// <summary>
        /// �Ƿ����ڱ���
        /// </summary>
        private bool isBusy = false;

        /// <summary>
        /// �Ƿ񰴲�����ʾ��ҩ��Ϣ
        /// </summary>
        private bool ShowByNurseCell = false;

        string deptTemp = "";

        /// <summary>
        /// �Ƿ���ʾ��Ϣ ���³��������� {55B34E3A-7741-4420-A947-29DE881A0119} wbo 2010-12-28
        /// </summary>
        private bool isShowMessage = true;

        #endregion

        #region ����

        /// <summary>
        /// ���ڵ����� 0 ����վ Patient ��ʾ���� 1 ���� Dept ȡҩ����
        /// </summary>
        [Description("������ڵ������������"), Category("����"), DefaultValue(ucSendDrugByNurse.NodeType.Patient)]
        public NodeType TreeType
        {
            get
            {
                return this.treeType;
            }
            set
            {
                this.treeType = value;

                if (value == NodeType.Patient)			//����վʹ�� ��ʾ�����б�/�˷���Ϣ/��������Ϣ
                {              
                    if (this.SpreadDrug.Sheets.Contains(this.sheetViewDetail))
                        this.SpreadDrug.Sheets.Remove(this.sheetViewDetail);
                    //����ʾҩƷ�����б�
                    if (this.neuTabControl1.TabPages.Contains(this.tpQuality))
                        this.neuTabControl1.TabPages.Remove(this.tpQuality);

                    this.IsShowCheck = false;
                    //this.ShowNurse();

                    this.lbTime.Text = "����ʱ�䣺";
                }
                else					//ҩ����ҩ��ѯ ��ʾȡҩ����/ҩƷ�����б�
                {
                    //����ʾ�˷���Ϣ/��������Ϣ�б�
                    if (this.neuTabControl2.TabPages.Contains(this.tpQuitFee))
                        this.neuTabControl2.TabPages.Remove(this.tpQuitFee);
                    if (this.neuTabControl2.TabPages.Contains(this.tpOutBill))
                        this.neuTabControl2.TabPages.Remove(this.tpOutBill);

                    //this.ShowDept();
                    //this.ShowDrugQuality();

                    this.lbTime.Text = "��ҩʱ�䣺";
                }
            }
        }

        /// <summary>
        /// ȡҩ��������
        /// </summary>
        [Description("ȡҩ�������� ���һ���վ"), Category("����"), DefaultValue(ucSendDrugByNurse.ReciveDrugType.Dept)]
        public ReciveDrugType ReciveType
        {
            get
            {
                return this.reciveType;
            }
            set
            {
                this.reciveType = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾ��������ѯTab
        /// </summary>
        [Description("�Ƿ���ʾ��������ѯTab"), Category("����"), DefaultValue(false)]
        public bool IsShowOutBill
        {
            get
            {
                return this.neuTabControl2.TabPages.Contains(this.tpOutBill);
            }
            set
            {
                if (value && !this.neuTabControl2.TabPages.Contains(this.tpOutBill))
                    this.neuTabControl2.TabPages.Add(this.tpOutBill);
                if (!value && this.neuTabControl2.TabPages.Contains(this.tpOutBill))
                    this.neuTabControl2.TabPages.Remove(this.tpOutBill);
            }
        }

        /// <summary>
        /// �Ƿ���ʾ�˷ѵ���ѯTab
        /// </summary>
        [Description("�Ƿ���ʾ�˷ѵ���ѯTab"), Category("����"), DefaultValue(false)]
        public bool IsShowQuitBill
        {
            get
            {
                return this.neuTabControl2.TabPages.Contains(this.tpQuitFee);
            }
            set
            {
                if (value && !this.neuTabControl2.TabPages.Contains(this.tpQuitFee))
                    this.neuTabControl2.TabPages.Add(this.tpQuitFee);
                if (!value && this.neuTabControl2.TabPages.Contains(this.tpQuitFee))
                    this.neuTabControl2.TabPages.Remove(this.tpQuitFee);
            }
        }

        /// <summary>
        /// �Ƿ���ʾ�Ѱ�/δ�ڹ���ѡ���
        /// </summary>
        [Description("�Ƿ���ʾ�Ѱ�/δ�ڹ���ѡ���"), Category("����"), DefaultValue(true)]
        public bool IsShowCheck
        {
            get
            {
                return this.rbSended.Visible;
            }
            set
            {
                this.rbSended.Visible = value;
                this.rbSending.Visible = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾ���˿�
        /// </summary>
        [Description("�Ƿ���ʾ���˿�"), Category("����"), DefaultValue(true)]
        public bool IsShowFilter
        {
            set
            {
                this.lbFilter.Visible = value;
                this.txtFilter.Visible = value;
            }
        }

        /// <summary>
        ///  �Ƿ��в�ѯȨ�� ���в�ѯȨ�޿��ԶԲ�ѯʱ������޸�
        /// </summary>
        [Description("�Ƿ���Ҫ��ѯȨ�� ����ҪȨ�� ���޲�ѯȨ��ʱ���ܶԲ�ѯʱ������޸�"), Category("����"), 
        DefaultValue(true),Browsable(false)]
        public bool IsPrivQuery
        {
            set
            {
                this.dtpBegin.Enabled = value;
                this.dtpEnd.Enabled = value;
            }
        }

        /// <summary>
        /// ��ѯ���� �ɲ�ѯ������վ�����п���
        /// </summary>
        [Description("��ѯ����"), Category("����"), DefaultValue(true),Browsable(false)]
        public ArrayList DeptInfo
        {
            get
            {
                if (this.deptInfo == null)
                    this.deptInfo = new ArrayList();
                return this.deptInfo;
            }
            set
            {
                this.deptInfo = value;
            }
        }

        /// <summary>
        /// �Ƿ񰴰�ҩ��������ʾ Add by Sunjh 2009-2-5 
        /// </summary>
        [Description("�Ƿ񰴰�ҩ��������ʾ"), Category("����"), DefaultValue(true)]
        public bool IsShowDrugBill
        {
            get 
            {
                return isShowDrugBill; 
            }
            set 
            { 
                isShowDrugBill = value; 
            }
        }

        /// <summary>
        /// �Ƿ񰴲�����ʾ��ҩ��Ϣ Add by Sunjh 2010-8-9 
        /// </summary>
        [Description("�Ƿ񰴲�����ʾ��ҩ��Ϣ"), Category("����"), DefaultValue(false)]
        public bool ShowByNurseCell1
        {
            get 
            {
                return ShowByNurseCell;
            }
            set 
            { 
                ShowByNurseCell = value; 
            }
        }

        /// <summary>
        /// �Ƿ���ʾ��Ϣ ���³��������� {55B34E3A-7741-4420-A947-29DE881A0119} wbo 2010-12-28
        /// </summary>
        [Description("�Ƿ���봰����ʾ��ʾ��Ϣ����ʾȡ�������ڲ���ֱ�ӿ���ȡ��"), Category("����")]
        public bool IsShowMessage
        {
            get { return this.isShowMessage; }
            set { this.isShowMessage = value; }
        }

        #endregion

        #region ��������Ϣ

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnQuery(object sender, object neuObject)
        {
            this.Query();

            return 1;
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnPrint(object sender, object neuObject)
        {
            bool isNursePrint = this.ctrlIntegrate.GetControlParam<bool>("P01016", true, false);
            if (isNursePrint)
            {
                this.SetInterface();
                if (this.neuTabControl1.SelectedIndex == 1)
                {
                    this.RePrintDrugBill();
                }                
            }
            else
            {
                this.Print();
            }
            
            return 1;
        }

        protected override int OnSave(object sender, object neuObject)
        {
            bool isNursePrint = this.ctrlIntegrate.GetControlParam<bool>("P01016", true, false);
            if (isNursePrint)
            {
                this.SetInterface();
                if (this.neuTabControl1.SelectedIndex == 0)
                {
                    this.PrintDrugBill();
                }
                //else
                //{
                //    this.RePrintDrugBill();
                //}                
                this.Query();
            }
            //else
            //{
            //    this.Print();
            //}
            return 1;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int Export(object sender, object neuObject)
        {
            object obj = this.hashTableFp[this.neuTabControl2.SelectedTab];

            FarPoint.Win.Spread.FpSpread fp = obj as FarPoint.Win.Spread.FpSpread;

            SaveFileDialog op = new SaveFileDialog();

            op.Title = "��ѡ�񱣴��·��������";
            op.CheckFileExists = false;
            op.CheckPathExists = true;
            op.DefaultExt = "*.xls";
            op.Filter = "(*.xls)|*.xls";

            DialogResult result = op.ShowDialog();

            if (result == DialogResult.Cancel || op.FileName == string.Empty)
            {
                return -1;
            }

            string filePath = op.FileName;

            bool returnValue = fp.SaveExcel(filePath, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);


            return base.Export(sender, neuObject);
        
        }

        #endregion

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��Fp����
        /// </summary>
        protected void FpInit()
        {
            //�Ի���������ʾʱ �Ե�һ�� �ڶ��н�����ͬ��ֵ�ϲ�
            this.sheetViewTot.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            this.sheetViewTot.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);
            //����ϸ������ʾʱ �Ե�һ�н�����ͬ��ֵ�ϲ�
            this.sheetViewDetail.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            //���ö�����ʾ
            this.sheetViewTot.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sheetViewTot.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetViewTot.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetViewDetail.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        }

        /// <summary>
        /// ��ʼ����Ա��Ϣ
        /// </summary>
        protected void OperInit()
        {
            if (this.operVar == null)
                this.operVar = ((Neusoft.HISFC.Models.Base.Employee)this.itemManager.Operator);
        }

        /// <summary>
        /// ���ݳ�ʼ��
        /// </summary>
        protected void DataInit()
        {
            this.dtpEnd.Value = this.itemManager.GetDateTimeFromSysDateTime();
            this.dtpBegin.Value = this.dtpEnd.Value.AddDays(-1);

            //ȡȫԺ������Ϣ
            deptHelper.ArrayObject = this.departmentManager.GetDepartment();

            if (this.reciveType == ReciveDrugType.Dept)
            {
                this.deptInfo = departmentManager.QueryDepartment(this.operVar.Nurse.ID);
                if (this.deptInfo == null)
                {
                    Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
                    info.ID = this.operVar.Dept.ID;
                    info.Name = this.operVar.Dept.Name;
                    this.deptInfo.Add(info);
                }
            }
            else
            {
                this.deptInfo = new ArrayList();

                Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
                info.ID = this.operVar.Nurse.ID;
                info.Name = this.operVar.Nurse.Name;
                this.deptInfo.Add(info);
            }

            //ȡ��Ա��Ϣ
            Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            this.personHelper.ArrayObject = managerIntegrate.QueryEmployeeAll();
        }

        /// <summary>
        /// ��������
        /// </summary>
        protected Hashtable hashTableFp = new Hashtable();

        /// <summary>
        /// ��ʼ����ϣ��
        /// </summary>
        private void InitHashTable()
        {
            foreach (TabPage t in this.neuTabControl2.TabPages)
            {
                foreach (Control c in t.Controls)
                {
                    if (c is FarPoint.Win.Spread.FpSpread)
                    {
                        this.hashTableFp.Add(t, c);
                    }
                }
            }
        }

        /// <summary>
        /// ��ʼ��ҩ���б�
        /// </summary>
        private void DeptInit()
        {
            ArrayList alTemp = this.departmentManager.GetConstantList("PHARMACYTYPE");
            //this.cbbPharmacy.AddItems(alTemp);
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            this.FpInit();

            this.OperInit();

            this.DataInit();

            //this.DeptInit();

            this.tvDept.ImageList = this.tvDept.deptImageList;

            this.ucQueryInpatientNo1.InputType = 0;			//�������� סԺ��
            this.InitHashTable();

            //Function.dtDrugStore = this.GetDept();//��ȡҩ������

            bool isNursePrint = this.ctrlIntegrate.GetControlParam<bool>("P01016", true, false);
            if (isNursePrint)
            {
                this.ucQueryInpatientNo1.Visible = false;
                this.neuPanel2.Visible = false;
                this.neuPanel1.Visible = true;
                this.tpBill.Show();
            }
            else
            {
                this.tpBill.Hide();
            }
            
            ArrayList alNurse = this.departmentManager.QueryNurseStationByDept(this.operVar.Dept, "01");
            foreach (Neusoft.FrameWork.Models.NeuObject infoTemp in alNurse)
            {
                ArrayList deptCell = this.departmentManager.QueryDepartment(infoTemp.ID);
                foreach (Neusoft.FrameWork.Models.NeuObject info in deptCell)
                {
                    if (deptTemp == "")
                    {
                        deptTemp = info.ID;
                    }
                    else
                    {
                        deptTemp = deptTemp + "','" + info.ID;
                    }
                }
            }

            this.neuTabControl1.TabPages.Remove(tpQuality);

        }

        /// <summary>
        /// ���ݼ�����ʾ
        /// </summary>
        public void ShowData()
        {
            if (this.treeType == NodeType.Patient)			//����վʹ�� ��ʾ�����б�/�˷���Ϣ/��������Ϣ
            {
                this.ShowNurse();
            }
            else					//ҩ����ҩ��ѯ ��ʾȡҩ����/ҩƷ�����б�
            {         
                this.ShowDept();
                this.ShowDrugQuality();
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ʾ����վ�б�
        /// </summary>
        public void ShowNurse()
        {
            Neusoft.HISFC.BizProcess.Integrate.RADT radtManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();

            this.OperInit();

            TreeNode deptNode = new TreeNode();
            deptNode.Text = this.operVar.Nurse.Name;
            deptNode.ImageIndex = 0;// (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.T����;
            deptNode.SelectedImageIndex = 0;

            //����ԭ�ȵ�"��ҩ��"tabҳ
            //this.neuTabControl1.TabPages[2].Hide();
            //�ж��Ƿ񰴰�ҩ������ by sunjh 2009-2-5 {0057B27E-5BDD-4211-9CDC-D0F4608D6C79}
            if (this.IsShowDrugBill == true)
            {
                this.tpDept.Text = "��ҩ��";
                ArrayList al = new ArrayList();
                al = drugStoreManager.QueryDrugBillClassList();
                if (al == null)
                {
                    MessageBox.Show(Language.Msg("��ѯ��ҩ�������б����!"));
                    return;
                }

                TreeNode billNode;
                foreach (Neusoft.HISFC.Models.Pharmacy.DrugBillClass billInfo in al)
                {
                    billNode = new TreeNode();
                    billNode.Text = billInfo.Name;
                    billNode.SelectedImageIndex = 1;
                    billNode.ImageIndex = 5;
                    billNode.Tag = billInfo.ID;
                    deptNode.Nodes.Add(billNode);
                }
                this.tvDept.Nodes.Add(deptNode);
                this.tvDept.ExpandAll();
            }
            else
            {
                this.tpDept.Text = "ȡҩ����";
                ArrayList al = new ArrayList();
                al = radtManager.QueryPatient(this.operVar.Dept.ID, Neusoft.HISFC.Models.Base.EnumInState.I);
                if (al == null)
                {
                    MessageBox.Show(Language.Msg("��ѯ���������б����!"));
                    return;
                }

                TreeNode patientNode;

                foreach (Neusoft.HISFC.Models.RADT.PatientInfo patientInfo in al)
                {
                    patientNode = new TreeNode();
                    patientNode.Text = "��" + patientInfo.PVisit.PatientLocation.Bed.Name + "��" + patientInfo.Name;
                    patientNode.SelectedImageIndex = 1;
                    patientNode.ImageIndex = 5;// (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.A��Ա;
                    patientNode.Tag = patientInfo.ID;
                    deptNode.Nodes.Add(patientNode);
                }
                this.tvDept.Nodes.Add(deptNode);
                this.tvDept.ExpandAll();
            }
            
            #region Not used
            //Neusoft.HISFC.Models.RADT.Location loc = new Neusoft.HISFC.Models.RADT.Location();
            //loc.NurseCell = this.operVar.User.Nurse.Clone();
            //loc.Dept = this.operVar.User.Dept.Clone();
            //Neusoft.HISFC.Models.RADT.VisitStatus state = new Neusoft.HISFC.Models.RADT.VisitStatus();
            //state.ID = Neusoft.HISFC.Models.RADT.VisitStatus.enuVisitStatus.I;
            //al = inPatient.PatientQuery(loc, state);    
            #endregion

        }

        /// <summary>
        /// ��ʾ�����б�
        /// </summary>
        public void ShowDept()
        {
            this.OperInit();

            Neusoft.HISFC.BizLogic.Pharmacy.Constant phaConstant = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
            ArrayList al = phaConstant.QueryReciveDrugDept(this.operVar.Dept.ID);
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡȡҩ�����б����" + phaConstant.Err));
                return;
            }

            TreeNode deptNode;
            TreeNode rootNode = new TreeNode("ȡҩ����");
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            rootNode.Tag = "AAAA";
            foreach (Neusoft.FrameWork.Models.NeuObject info in al)
            {
                if (info == null) 
                    continue;
                deptNode = new TreeNode();
                deptNode.Text = info.Name;
                deptNode.ImageIndex = 4;
                deptNode.SelectedImageIndex = 5;
                deptNode.Tag = info.ID;
                rootNode.Nodes.Add(deptNode);
            }
            this.tvDept.Nodes.Add(rootNode);
            this.tvDept.ExpandAll();
        }

        /// <summary>
        /// ��ʾҩƷ�����б�
        /// </summary>
        public void ShowDrugQuality()
        {

        }

        /// <summary>
        /// �������� ��������
        /// </summary>
        public void Query()
        {
            try
            {
                if (this.treeType == NodeType.Dept && this.neuTabControl1.SelectedTab == this.tpQuality && this.showLevel == 1)
                {
                    this.tvDrugType1.SelectedNode = this.tvDrugType1.Nodes[0];
                }

                this.Query(this.nowObj, this.showLevel);

                this.QueryNoExamData();

                this.SetFpColor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.Msg("��ѯִ�г���" + ex.Message));
                return;
            }
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="obj">������Ϣ</param>
        /// <param name="level">0 ������ڵ� 1 ����ӽڵ� 2 ͨ��סԺ�Ų�ѯ</param>
        protected void Query(object obj, int level)
        {            
            if (this.treeType == NodeType.Dept)             //�б���ʾȡҩ���� ��ҩ�����а�ҩ��ѯ
            {
                this.SpreadDrug.ActiveSheet = this.sheetViewTot;

                if (this.SpreadDrug.Sheets.Contains(this.sheetViewDetail))
                    this.SpreadDrug.Sheets.Remove(this.sheetViewDetail);

                this.neuTabControl2.SelectedTab = this.tpDruged;
            }

            if (level == 1 && obj == null) 
                return;

            DataSet ds = new DataSet();
            if (this.treeType == NodeType.Patient)			//�б���ʾ�����黼�� ����������ȡҩ��ѯ
            {
                if (this.IsShowDrugBill == true && !this.isQueryByCaseNO)
                {
                    //�Ըû���վ��Ӧ�İ�ҩ�����в�ѯ
                    string dept = "";
                    foreach (Neusoft.FrameWork.Models.NeuObject info in this.deptInfo)
                    {
                        if (dept == "")
                            dept = info.ID;
                        else
                            dept = dept + "','" + info.ID;
                    }
                    if (level == 0)                             //���ڵ��ѯ ��ѯ���������л��ߵĻ��ܰ�ҩ���
                    {
                        this.NurseDtaSet = new DataSet();
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByTime" };
                        this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, dept, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                    }
                    else                                      //�ӽڵ��ѯ ��ѯָ�����ߵİ�ҩ���
                    {
                        this.NurseDtaSet = new DataSet();
                        string[] strIndex;//new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient.BillClass" };
                        bool isNursePrint = this.ctrlIntegrate.GetControlParam<bool>("P01016", true, false);
                        if (isNursePrint)
                        {
                            if (this.ShowByNurseCell)
                            {
                                //string deptTemp = "";
                                //ArrayList alNurse = this.departmentManager.QueryNurseStationByDept(this.operVar.Dept, "01");
                                //foreach (Neusoft.FrameWork.Models.NeuObject infoTemp in alNurse)
                                //{
                                //    ArrayList deptCell = this.departmentManager.QueryDepartment(infoTemp.ID);
                                //    foreach (Neusoft.FrameWork.Models.NeuObject info in deptCell)
                                //    {
                                //        if (deptTemp == "")
                                //        {
                                //            deptTemp = info.ID;
                                //        }
                                //        else
                                //        {
                                //            deptTemp = deptTemp + "','" + info.ID;
                                //        }
                                //    }
                                //}
                                strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByNurseCell.BillClass.ByStoreDept" };
                                this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, obj as string, deptTemp);
                            }
                            else
                            {
                                strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient.BillClass.ByStoreDept" };
                                this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, obj as string, this.operVar.Dept.ID);
                            }                            
                        }
                        else
                        {
                            strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient.BillClass" };
                            this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, obj as string, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString(), this.operVar.Dept.ID);
                        }
                        
                        if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
                        {
                            try
                            {
                                for (int i = 0; i < NurseDtaSet.Tables[0].Rows.Count; i++)
                                {
                                    if (NurseDtaSet.Tables[0].Rows[i]["�������"] != null)
                                    {
                                        NurseDtaSet.Tables[0].Rows[i]["�������"] = deptHelper.GetName(NurseDtaSet.Tables[0].Rows[i]["�������"].ToString());
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
                    {
                        DataView dv = new DataView(this.NurseDtaSet.Tables[0]);
                        if (this.rbSended.Checked == true)
                        {
                            dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "�Ѱ�");
                        }
                        if (this.rbSending.Checked == true)
                        {
                            dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "δ��");
                        }
                        this.sheetViewTot.DataSource = dv;
                    }

                    this.SetFormat();
                }
                else
                {
                    #region ����վ

                    //�Ըû���վ��Ӧ�Ŀ��ҽ��в�ѯ
                    string dept = "";
                    foreach (Neusoft.FrameWork.Models.NeuObject info in this.deptInfo)
                    {
                        if (dept == "")
                            dept = info.ID;
                        else
                            dept = dept + "','" + info.ID;
                    }
                    if (level == 0)                             //���ڵ��ѯ ��ѯ���������л��ߵĻ��ܰ�ҩ���
                    {
                        this.NurseDtaSet = new DataSet();
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByTime" };
                        this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, dept, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                    }
                    else                                      //�ӽڵ��ѯ ��ѯָ�����ߵİ�ҩ���
                    {
                        this.NurseDtaSet = new DataSet();
                        this.ucQueryInpatientNo1.Text = obj as string;
                        this.ucQueryInpatientNo1.Text = this.ucQueryInpatientNo1.Text.Substring(4);
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient" };
                        this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, obj as string, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                        if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
                        {
                            try
                            {
                                for (int i = 0; i < NurseDtaSet.Tables[0].Rows.Count; i++)
                                {
                                    if (NurseDtaSet.Tables[0].Rows[i]["�������"] != null)
                                    {
                                        NurseDtaSet.Tables[0].Rows[i]["�������"] = deptHelper.GetName(NurseDtaSet.Tables[0].Rows[i]["�������"].ToString());
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
                    {
                        DataView dv = new DataView(this.NurseDtaSet.Tables[0]);
                        if (this.rbSended.Checked == true)
                        {
                            dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "�Ѱ�");
                        }
                        if (this.rbSending.Checked == true)
                        {
                            dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "δ��");
                        }
                        this.sheetViewTot.DataSource = dv;
                    }

                    this.SetFormat();

                    #endregion
                }

                this.SetFpColor();
            }
            else
            {
                if (level == 2)                           //���ݻ���סԺ�Ų�ѯ
                {
                    string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient" };
                    this.itemManager.ExecQuery(strIndex, ref ds, obj as string, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (ds.Tables[0].Columns.Contains("�������"))
                            {
                                dr["�������"] = this.deptHelper.GetName(dr["�������"].ToString());
                            }
                            if (ds.Tables[0].Columns.Contains("������"))
                            {
                                dr["������"] = this.personHelper.GetName(dr["������"].ToString());
                            }
                            if (ds.Tables[0].Columns.Contains("��ҩ��"))
                            {
                                dr["��ҩ��"] = this.personHelper.GetName(dr["��ҩ��"].ToString());
                            }
                        }
                        this.sheetViewTot.DataSource = ds;
                    }
                    this.SetNurseFormat();
                    return;
                }

                #region ����
                if (this.neuTabControl1.SelectedTab == this.tpQuality)
                {
                    #region ����ҩƷ���ʼ���
                    string qualityCode = "";
                    if (level == 0)
                    {

                        string applyState = "0','2','1";
                        if (this.rbSended.Checked)
                            applyState = "2','1";
                        if (this.rbSending.Checked)
                            applyState = "0";
                        this.QualityDataSet = new DataSet();
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOut.ByDrugQuality" };
                        this.itemManager.ExecQuery(strIndex, ref QualityDataSet, this.operVar.Dept.ID, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString(), applyState);
                        if (QualityDataSet != null && QualityDataSet.Tables.Count > 0)
                            this.sheetViewTot.DataSource = QualityDataSet;
                        else
                            return;
                        if (this.QualityDataSet.Tables[0].Rows.Count > 0)
                        {
                            if (this.QualityDataSet.Tables[0].Rows[this.QualityDataSet.Tables[0].Rows.Count - 1][1].ToString() == "�ϼƣ�")
                            {
                                this.QualityDataSet.Tables[0].Rows.RemoveAt(this.QualityDataSet.Tables[0].Rows.Count - 1);
                            }
                        }
                        DataRow row = this.QualityDataSet.Tables[0].NewRow();
                        row[1] = "�ϼƣ�";
                        row["ƴ����"] = "%";
                        row[6] = this.QualityDataSet.Tables[0].Compute("sum(���)", "");
                        this.QualityDataSet.Tables[0].Rows.Add(row);
                    }
                    if (level == 1)
                    {
                        if (QualityDataSet == null || QualityDataSet.Tables.Count <= 0)
                            return;
                        try
                        {
                            if (obj != null) qualityCode = obj as string;
                            DataView dv = new DataView(QualityDataSet.Tables[0]);
                            dv.RowFilter = "ҩƷ���� = " + "'" + qualityCode + "'";
                            this.sheetViewTot.DataSource = dv;
                            if (dv.Table.Rows.Count > 0)
                            {
                                if (dv.Table.Rows[dv.Table.Rows.Count - 1][1].ToString() == "�ϼƣ�")
                                {
                                    dv.Table.Rows.RemoveAt(dv.Table.Rows.Count - 1);
                                }
                            }

                            DataRow row = dv.Table.NewRow();
                            row[1] = "�ϼƣ�";
                            row["ƴ����"] = "%";
                            row["ҩƷ����"] = qualityCode;
                            row[6] = dv.Table.Compute("sum(���)", "ҩƷ���� = " + "'" + qualityCode + "'");
                            dv.Table.Rows.Add(row);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Language.Msg("����ҩƷ���ʳ���!" + ex.Message));
                            return;
                        }
                    }
                    this.SetQualityFormat();
                    #endregion
                }
                else
                {
                    #region ����
                    if (level == 0)
                    {
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByMedDept" };
                        this.itemManager.ExecQuery(strIndex, ref ds, this.operVar.Dept.ID, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                        if (ds != null && ds.Tables.Count > 0)
                            this.sheetViewTot.DataSource = ds;
                        else
                            return;

                        DataRow row = ds.Tables[0].NewRow();
                        row[0] = "�ϼƣ�";
                        row[1] = ds.Tables[0].Compute("sum(ȡҩ���)", "");
                        ds.Tables[0].Rows.Add(row);
                        this.SetMedDeptFormat();
                    }
                    if (level == 1)
                    {
                        string dept = obj as string;
                        DeptDataSet = new DataSet();
                        string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByDept" };
                        this.itemManager.ExecQuery(strIndex, ref DeptDataSet, this.operVar.Dept.ID, dept, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
                        if (DeptDataSet != null && DeptDataSet.Tables.Count > 0)
                            this.sheetViewTot.DataSource = DeptDataSet;

                        DataRow row = this.DeptDataSet.Tables[0].NewRow();
                        row[1] = "�ϼƣ�";
                        row["ƴ����"] = "%";
                        row[7] = this.DeptDataSet.Tables[0].Compute("sum(���)", "");
                        this.DeptDataSet.Tables[0].Rows.Add(row);
                        this.SetTotFormat();
                    }
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// ����ҩ���Ų�ѯ����ҩ��ϸ
        /// </summary>
        /// <param name="drugBill"></param>
        protected void Query(string drugBill)
        {
            this.chkUpdatePrintFlag.Checked = false;
            //�Ըû���վ��Ӧ�İ�ҩ�����в�ѯ
            string dept = "";
            foreach (Neusoft.FrameWork.Models.NeuObject info in this.deptInfo)
            {
                if (dept == "")
                    dept = info.ID;
                else
                    dept = dept + "','" + info.ID;
            }

            this.NurseDtaSet = new DataSet();
            string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByPatient.BillClass.ByNursePrintBill" };
            this.itemManager.ExecQuery(strIndex, ref NurseDtaSet, drugBill);

            if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
            {
                try
                {
                    for (int i = 0; i < NurseDtaSet.Tables[0].Rows.Count; i++)
                    {
                        if (NurseDtaSet.Tables[0].Rows[i]["�������"] != null)
                        {
                            NurseDtaSet.Tables[0].Rows[i]["�������"] = deptHelper.GetName(NurseDtaSet.Tables[0].Rows[i]["�������"].ToString());
                        }
                    }
                }
                catch { }
            }

            if (NurseDtaSet != null && NurseDtaSet.Tables.Count > 0)
            {
                DataView dv = new DataView(this.NurseDtaSet.Tables[0]);
                if (this.rbSended.Checked == true)
                {
                    dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "�Ѱ�");
                }
                if (this.rbSending.Checked == true)
                {
                    dv.RowFilter = string.Format("�Ƿ��ҩ = '{0}'", "δ��");
                }
                this.sheetViewTot.DataSource = dv;
            }

            this.SetFormat();
            this.SetFpColor();            
        }

        /// <summary>
        /// ��ȡδȷ�ϵ��˷������ҽ��������Ϣ
        /// </summary>
        public void QueryNoExamData()
        {
            if (this.treeType == NodeType.Dept) return;
            if (this.nowObj == null) return;
            string inPatientNo = this.nowObj.ToString();
            DataSet ds = new DataSet();
            DataSet dsOutput = new DataSet();
            string[] strIndex = new string[1] { "Pharmacy.Item.GetFeeOrderAffirmInfo.ByPatient" };
            int parm = this.itemManager.ExecQuery(strIndex, ref ds, inPatientNo, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
            if (parm == -1)
            {
                MessageBox.Show(Language.Msg(this.itemManager.Err));
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                this.SpreadQuitFee_Sheet1.DataSource = ds;
            }
            strIndex = new string[1] { "Pharmacy.Item.GetOutputAffirm.ByPatient" };
            parm = this.itemManager.ExecQuery(strIndex, ref dsOutput, inPatientNo, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString());
            if (parm == -1)
            {
                MessageBox.Show(Language.Msg(this.itemManager.Err));
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                this.SpreadOut_Sheet1.DataSource = dsOutput;
                this.SetOutputFormat();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Filter()
        {
            if (this.treeType == NodeType.Dept)
            {
                if (this.neuTabControl1.SelectedTab == this.tpDept && this.tvDept.SelectedNode == this.tvDept.Nodes[0])
                    return;
                DataView dv;
                if (this.neuTabControl1.SelectedTab == this.tpQuality)
                {
                    dv = new DataView(this.QualityDataSet.Tables[0]);
                }
                else
                {
                    dv = new DataView(this.DeptDataSet.Tables[0]);
                }
                //dv.RowFilter = Function.GetFilterStr(dv, this.txtFilter.Text);
                this.sheetViewTot.DataSource = dv;
                this.SetFormat();
            }
            else
            {
                if (this.NurseDtaSet == null || this.NurseDtaSet.Tables.Count <= 0)
                    return;
                DataView dv;
                if (this.neuTabControl2.SelectedTab == this.tpDruged)
                {
                    dv = new DataView(this.NurseDtaSet.Tables[0]);
                    //dv.RowFilter = Function.GetFilterStr(dv, this.txtFilter.Text);
                    this.sheetViewTot.DataSource = dv;
                    this.SetNurseFormat();
                }
            }


            this.SetFpColor();
        }

        /// <summary>
        /// ����ҩ���Ŀ���
        /// </summary>
        /// <returns></returns>
        public DataTable GetDept()
        {
            Neusoft.HISFC.BizLogic.Manager.Department deptMgr = new Neusoft.HISFC.BizLogic.Manager.Department();
            ArrayList al=deptMgr.GetDeptmentByType("P");
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("���Ҵ���",typeof(System.String)));
            dt.Columns.Add(new DataColumn("��������",typeof(System.String)));
            dt.Columns.Add(new DataColumn("ƴ����",typeof(System.String)));
            dt.Columns.Add(new DataColumn("�����",typeof(System.String)));
            dt.Columns.Add(new DataColumn("�Զ�����",typeof(System.String)));
            dt.PrimaryKey=new DataColumn[]{dt.Columns[0]};

            dt.Rows.Clear();
            foreach (Neusoft.HISFC.Models.Base.Department dept in al)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                dr[0] = dept.ID;
                dr[1] = dept.Name;
                dr[2] = dept.SpellCode;
                dr[3] = dept.WBCode;
                dr[4] = dept.UserCode;
                dr.EndEdit();
                dt.Rows.Add(dr);
            }

            return dt;
            
        }

        /// <summary>
        /// ͳ�ƻ���
        /// </summary>
        public void Sum()
        {
            if (this.treeType == NodeType.Patient)	//�Ի���վ��ѯ�����л���ͳ��
                return;
            int iIndex = this.sheetViewTot.Rows.Count;
            if (iIndex == 0)
                return;
            int iSumIndex = 0;
            if (this.neuTabControl1.SelectedTab == this.tpDept)
            {
                if (this.tvDept.SelectedNode != null && this.tvDept.Nodes != null && this.tvDept.SelectedNode == this.tvDept.Nodes[0])
                    iSumIndex = 1;
                else
                    iSumIndex = 6;
            }
            else
            {
                iSumIndex = 4;
            }
            try
            {
                this.sheetViewTot.Rows.Add(iIndex, 1);
                this.sheetViewTot.Cells[iIndex, 1].Text = "�ϼƣ�";
                this.sheetViewTot.Cells[iIndex, iSumIndex].Formula = "SUM(" + (char)(65 + iSumIndex) + "1:" + (char)(65 + iSumIndex) + iIndex.ToString() + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ͳ�Ƴ���!" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        public void Print()
        {
            Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
            p.IsDataAutoExtend = true;//p.ShowPageSetup();
            Neusoft.HISFC.Models.Base.PageSize page = new Neusoft.HISFC.Models.Base.PageSize();
            page.Height = 1060;
            page.Width = 800;
            page.Left = 100;
            page.Name = "Letter";
            p.SetPageSize(page);
            System.Windows.Forms.Panel panel = new Panel();
            panel.BackColor = System.Drawing.Color.White;
            FarPoint.Win.Spread.FpSpread fp = new FarPoint.Win.Spread.FpSpread();
            FarPoint.Win.Spread.SheetView fpView = new FarPoint.Win.Spread.SheetView();
            fp.Sheets.Add(fpView);
            fpView.Columns.Count = this.sheetViewTot.Columns.Count;
            for (int i = 0; i < this.sheetViewTot.Columns.Count; i++)
            {
                fpView.Columns[i].Visible = this.sheetViewTot.Columns[i].Visible;
                fpView.Columns[i].Width = this.sheetViewTot.Columns[i].Width;
                fpView.Columns[i].Label = this.sheetViewTot.Columns[i].Label;
                fpView.Columns[i].BackColor = this.sheetViewTot.Columns[i].BackColor;
            }
            for (int i = 0; i < this.sheetViewTot.Rows.Count; i++)
            {
                fpView.Rows[i].Visible = this.sheetViewTot.Rows[i].Visible;
                fpView.Rows[i].Height = this.sheetViewTot.Rows[i].Height;
                fpView.Rows[i].BackColor = this.sheetViewTot.Rows[i].BackColor;
                for (int j = 0; j < this.sheetViewTot.Columns.Count; j++)
                {
                    fpView.Cells[i, j].Value = this.sheetViewTot.Cells[i, j].Value;
                }
            }
            panel.Controls.Add(fp);
            fp.Dock = System.Windows.Forms.DockStyle.Fill;
            p.PrintPreview(100, 30, panel);
        }

        #region Fp��ʾ���� ����Ч��¼��ʾ��ɫ

        /// <summary>
        /// ����Fp��ʾ����
        /// </summary>
        private void SetFpColor()
        {
            if (this.sheetViewTot.Columns.Count > 16)
            {
                for (int i = 0; i < this.sheetViewTot.Rows.Count; i++)
                {
                    this.sheetViewTot.SetRowLabel(i, 0, " ");
                    if (this.sheetViewTot.Cells[i, 18].Text == "��Ч")
                    {
                        this.sheetViewTot.Rows[i].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.sheetViewTot.Rows[i].ForeColor = System.Drawing.Color.Black;
                    }
                    //if (this.sheetViewTot.Cells[i, 17].Text != null &&
                    //    this.sheetViewTot.Cells[i, 17].Text != "")
                    //{
                    //    this.sheetViewTot.SetRowLabel(i, 0, "��");
                    //    this.sheetViewTot.RowHeader.Cells[i, 0].BackColor = System.Drawing.Color.White;
                    //}
                }
            }
            if (this.sheetViewDetail.Columns.Count > 15)
            {
                for (int i = 0; i < this.sheetViewDetail.Rows.Count; i++)
                {
                    this.sheetViewDetail.SetRowLabel(i, 0, " ");
                    if (this.sheetViewDetail.Cells[i, 16].Text == "��Ч")
                    {
                        this.sheetViewDetail.Rows[i].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.sheetViewDetail.Rows[i].ForeColor = System.Drawing.Color.Black;
                    }

                    if (this.sheetViewDetail.Cells[i, 16].Text != null &&
                        this.sheetViewDetail.Cells[i, 16].Text != "")
                    {
                        this.sheetViewDetail.SetRowLabel(i, 0, "��");
                        this.sheetViewDetail.RowHeader.Cells[i, 0].BackColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        #endregion

        #region FarPoint��ʽ��

        public void SetFormat()
        {
            if (this.treeType == NodeType.Patient)		//����վ��ѯ��
            {
                this.SetNurseFormat();
                return;
            }
            if (this.treeType == NodeType.Dept)
            {
                if (this.neuTabControl1.SelectedTab == this.tpDept)
                {
                    if (this.tvDept.SelectedNode == this.tvDept.Nodes[0])
                    {
                        this.SetMedDeptFormat();
                    }
                    else
                    {
                        this.SetTotFormat();
                    }
                }
                else
                {
                    this.SetQualityFormat();
                }
            }
        }
        /// <summary>
        /// ��ʽ��
        /// </summary>
        private void SetNurseFormat()
        {

            try
            {
                bool isNursePrint = this.ctrlIntegrate.GetControlParam<bool>("P01016", true, false);
                if (isNursePrint)
                {
                    if (this.neuTabControl1.SelectedIndex == 0)
                    {
                        this.sheetViewTot.DefaultStyle.Locked = false;
                        this.sheetViewTot.GrayAreaBackColor = System.Drawing.Color.Honeydew;
                        this.sheetViewTot.SelectionBackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(225)), ((System.Byte)(243)));

                        #region ������
                        FarPoint.Win.Spread.CellType.TextCellType textType = new FarPoint.Win.Spread.CellType.TextCellType();
                        FarPoint.Win.Spread.CellType.CheckBoxCellType chkType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                        this.sheetViewTot.Columns[2].CellType = chkType;
                        this.sheetViewTot.Columns.Get(2).Width = 20F;
                        this.sheetViewTot.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        this.sheetViewTot.Columns[3].CellType = textType;
                        this.sheetViewTot.Columns.Get(3).Width = 60F;
                        this.sheetViewTot.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        this.chkSelectAll.Checked = false;

                        #endregion

                        this.sheetViewTot.Columns.Get(0).Width = 30F;
                        this.sheetViewTot.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;

                        this.sheetViewTot.Columns.Get(1).Width = 50F;
                        this.sheetViewTot.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;

                        this.sheetViewTot.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(4).Width = 160F;
                        this.sheetViewTot.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(5).Width = 100F;
                        this.sheetViewTot.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(6).Width = 0F;		//ÿ����
                        this.sheetViewTot.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(7).Width = 0F;		//��λ
                        this.sheetViewTot.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(8).Width = 45F;		//Ƶ��
                        this.sheetViewTot.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(9).Width = 60F;		//�÷�
                        this.sheetViewTot.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(10).Width = 40F;
                        this.sheetViewTot.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(11).Width = 40F;
                        this.sheetViewTot.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(12).Width = 80F;

                        this.sheetViewTot.Columns.Get(13).Width = 100F;
                        this.sheetViewTot.Columns.Get(14).Width = 80F;
                        this.sheetViewTot.Columns.Get(15).Width = 70F;
                        this.sheetViewTot.Columns.Get(16).Width = 120F;
                        this.sheetViewTot.Columns.Get(17).Width = 60F;
                        try
                        {
                            this.sheetViewTot.Columns.Get(18).Width = 80F;
                        }
                        catch { }
                    }
                    else
                    {
                        this.sheetViewTot.DefaultStyle.Locked = true;
                        this.sheetViewTot.GrayAreaBackColor = System.Drawing.Color.Honeydew;
                        this.sheetViewTot.SelectionBackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(225)), ((System.Byte)(243)));

                        this.sheetViewTot.Columns.Get(0).Width = 30F;
                        this.sheetViewTot.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        this.sheetViewTot.Columns.Get(1).Width = 50F;
                        this.sheetViewTot.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        this.sheetViewTot.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        this.sheetViewTot.Columns.Get(2).Width = 160F;
                        this.sheetViewTot.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(3).Width = 100F;
                        this.sheetViewTot.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(4).Width = 0F;		//ÿ����
                        this.sheetViewTot.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(5).Width = 0F;		//��λ
                        this.sheetViewTot.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(6).Width = 45F;		//Ƶ��
                        this.sheetViewTot.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                        this.sheetViewTot.Columns.Get(7).Width = 60F;		//�÷�
                        this.sheetViewTot.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(8).Width = 40F;
                        this.sheetViewTot.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(9).Width = 40F;
                        this.sheetViewTot.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                        this.sheetViewTot.Columns.Get(10).Width = 80F;

                        this.sheetViewTot.Columns.Get(11).Width = 100F;
                        this.sheetViewTot.Columns.Get(12).Width = 80F;
                        this.sheetViewTot.Columns.Get(13).Width = 70F;
                        this.sheetViewTot.Columns.Get(14).Width = 120F;
                        this.sheetViewTot.Columns.Get(15).Width = 60F;
                        try
                        {
                            this.sheetViewTot.Columns.Get(16).Width = 80F;
                        }
                        catch { }
                    }
                }
                else
                {
                    this.sheetViewTot.DefaultStyle.Locked = true;
                    this.sheetViewTot.GrayAreaBackColor = System.Drawing.Color.Honeydew;
                    this.sheetViewTot.SelectionBackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(225)), ((System.Byte)(243)));

                    this.sheetViewTot.Columns.Get(0).Width = 0F;
                    this.sheetViewTot.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    this.sheetViewTot.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                    this.sheetViewTot.Columns.Get(1).Width = 50F;
                    this.sheetViewTot.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                    this.sheetViewTot.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    this.sheetViewTot.Columns.Get(2).Width = 160F;
                    this.sheetViewTot.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    this.sheetViewTot.Columns.Get(3).Width = 100F;
                    this.sheetViewTot.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                    this.sheetViewTot.Columns.Get(4).Width = 0F;		//ÿ����
                    this.sheetViewTot.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                    this.sheetViewTot.Columns.Get(5).Width = 0F;		//��λ
                    this.sheetViewTot.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    this.sheetViewTot.Columns.Get(6).Width = 45F;		//Ƶ��
                    this.sheetViewTot.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                    this.sheetViewTot.Columns.Get(7).Width = 60F;		//�÷�
                    this.sheetViewTot.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    this.sheetViewTot.Columns.Get(8).Width = 40F;
                    this.sheetViewTot.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    this.sheetViewTot.Columns.Get(9).Width = 40F;
                    this.sheetViewTot.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                    this.sheetViewTot.Columns.Get(10).Width = 80F;

                    this.sheetViewTot.Columns.Get(11).Width = 100F;
                    this.sheetViewTot.Columns.Get(12).Width = 80F;
                    this.sheetViewTot.Columns.Get(13).Width = 70F;
                    this.sheetViewTot.Columns.Get(14).Width = 120F;
                    this.sheetViewTot.Columns.Get(15).Width = 60F;
                    try
                    {
                        this.sheetViewTot.Columns.Get(16).Width = 80F;
                    }
                    catch { }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void SetTotFormat()
        {
            this.sheetViewTot.DefaultStyle.Locked = true;
            this.sheetViewTot.Columns.Get(0).Width = 0F;		//ҩƷ����
            this.sheetViewTot.Columns.Get(1).Width = 80F;			//��ҩ״̬
            this.sheetViewTot.Columns.Get(2).Width = 160F;			//����
            this.sheetViewTot.Columns.Get(3).Width = 100F;			//���
            this.sheetViewTot.Columns.Get(4).Width = 75F;			//���ۼ�
            this.sheetViewTot.Columns.Get(5).Width = 75F;			//����
            this.sheetViewTot.Columns.Get(6).Width = 50F;			//��λ
            this.sheetViewTot.Columns.Get(7).Width = 80F;			//���            
            this.sheetViewTot.Columns.Get(8).Width = 0F;			//ƴ����
            this.sheetViewTot.Columns.Get(9).Width = 0F;			//�����
            this.sheetViewTot.Columns.Get(10).Width = 0F;			//�Զ�����
        }
        private void SetDetailFormat()
        {
            this.sheetViewDetail.DefaultStyle.Locked = true;
            this.sheetViewDetail.Columns.Get(0).Width = 40F;		//����
            this.sheetViewDetail.Columns.Get(1).Width = 80F;		//����
            this.sheetViewDetail.Columns.Get(2).Width = 160F;		//ҩƷ����
            this.sheetViewDetail.Columns.Get(3).Width = 85F;		//���
            this.sheetViewDetail.Columns.Get(4).Width = 75F;		//���ۼ�
            this.sheetViewDetail.Columns.Get(5).Width = 75F;		//ÿ����
            this.sheetViewDetail.Columns.Get(6).Width = 50F;		//��λ
            this.sheetViewDetail.Columns.Get(7).Width = 50F;		//Ƶ��
            this.sheetViewDetail.Columns.Get(8).Width = 50F;		//�÷�
            this.sheetViewDetail.Columns.Get(9).Width = 75F;		//����
            this.sheetViewDetail.Columns.Get(10).Width = 50F;		//��λ  
            this.sheetViewDetail.Columns.Get(11).Width = 90F;		//��ҩ��
            this.sheetViewDetail.Columns.Get(12).Width = 80F;		//��ҩ��
            this.sheetViewDetail.Columns.Get(13).Width = 100F;		//��ҩʱ��
        }

        private void SetAffirmFormat()
        {
            this.SpreadQuitFee_Sheet1.DefaultStyle.Locked = true;
        }
        private void SetOutputFormat()
        {
            this.SpreadOut_Sheet1.DefaultStyle.Locked = true;
            this.SpreadOut_Sheet1.Columns[0].Width = 70F;	//����
            this.SpreadOut_Sheet1.Columns[1].Width = 70F;	//������
            this.SpreadOut_Sheet1.Columns[2].Width = 150F;	//ҩƷ����
            this.SpreadOut_Sheet1.Columns[3].Width = 80F;
            this.SpreadOut_Sheet1.Columns[4].Width = 55F;	//����
            this.SpreadOut_Sheet1.Columns[5].Width = 100F;	//ȡҩҩ��
            this.SpreadOut_Sheet1.Columns[6].Width = 80F;
            this.SpreadOut_Sheet1.Columns[7].Width = 70F;
            this.SpreadOut_Sheet1.Columns[8].Width = 80F;
        }
        private void SetMedDeptFormat()
        {
            this.sheetViewTot.DefaultStyle.Locked = true;
            this.sheetViewTot.Columns[0].Visible = true;
            this.sheetViewTot.Columns[0].Width = 150F;
            this.sheetViewTot.Columns[1].Width = 100F;
            this.sheetViewTot.Columns[2].Width = 100F;
        }
        private void SetQualityFormat()
        {
            this.sheetViewTot.DefaultStyle.Locked = true;
            this.sheetViewTot.Columns[0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sheetViewTot.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sheetViewTot.Columns[1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            this.sheetViewTot.Columns[0].Visible = false;

            this.sheetViewTot.Columns[1].Visible = true;
            this.sheetViewTot.Columns[1].Width = 120F;          //���
            this.sheetViewTot.Columns[2].Width = 140F;          //ҩƷ����
            this.sheetViewTot.Columns[3].Width = 80F;           //���
            this.sheetViewTot.Columns[4].Width = 80F;           //��ҩ��
            this.sheetViewTot.Columns[5].Width = 60F;           //��λ
            this.sheetViewTot.Columns[6].Width = 80F;           //���

            this.sheetViewTot.Columns[7].Visible = false;       //ҩƷ����
            this.sheetViewTot.Columns[7].Width = 60F;
            this.sheetViewTot.Columns[8].Width = 0F;
            this.sheetViewTot.Columns[9].Width = 0F;
            this.sheetViewTot.Columns[10].Width = 0F;
        }

        #endregion

        #region ����ҩ����ӡ�ӿ�

        /// <summary>
        /// ��ҩ����ӡ�ӿ�ʵ����
        /// </summary>
        private void SetInterface()
        {
            //object[] o = new object[] { };
            //try
            //{
            //    //�����ǩ��ӡ�ӿ�ʵ����
            //    Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
            //    string billValue = ctrlIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Inpatient_Print_Fun, true, "Report.DrugStore.InpatientBillPrint");

            //    System.Runtime.Remoting.ObjectHandle objHandel = System.Activator.CreateInstance("Report", billValue, false, System.Reflection.BindingFlags.CreateInstance, null, o, null, null, null);
            //    object oLabel = objHandel.Unwrap();

            //    Function.IDrugPrint = oLabel as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IDrugPrint;
            //    if (Function.IDrugPrint == null)
            //    {
            //        MessageBox.Show("��ҩ���ؼ�ʵ�ִ��� ��ʵ�ֻ���IDrugPrint�ӿ�");
            //        return;
            //    }
                
            //}
            //catch (System.TypeLoadException ex)
            //{
            //    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            //    MessageBox.Show(Language.Msg("��ҩ�������ռ���Ч\n" + ex.Message));
            //    return;
            //}
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="deptCodes"></param>
        /// <param name="storageDeptCode"></param>
        /// <returns></returns>
        private ArrayList QueryData(string deptCodes, string storageDeptCode)
        {
            ArrayList alTemp = new ArrayList();
            //��ѯסԺҩƷ����������Ϣ<����δ����>            
            alTemp = this.itemManager.QueryApplyOutList(this.nowObj.ToString(), deptCodes, storageDeptCode, "0");
            return alTemp;
        }

        /// <summary>
        /// ��ӡ��ҩ��
        /// </summary>
        private void PrintDrugBill()
        {
            //��ѯ����
            //�Ըû���վ��Ӧ�Ŀ��ҽ��в�ѯ
            //string depts = "";
            //foreach (Neusoft.FrameWork.Models.NeuObject info in this.deptInfo)
            //{
            //    if (depts == "")
            //        depts = info.ID;
            //    else
            //        depts = depts + "','" + info.ID;
            //}

            //hsSelected
            if (this.isBusy)
            {
                MessageBox.Show("���ڱ��棬�����ظ����������Ժ�...");
                return;
            }
            this.isBusy = true;
            this.hsSelected = new Hashtable();
            for (int j = 0; j < this.sheetViewTot.RowCount; j++)
            {
                if (this.sheetViewTot.Cells[j, 2].Text == "True")
                {
                    this.hsSelected.Add(this.sheetViewTot.Cells[j, 3].Text, null);
                }
            }

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ��淢�����룬���Ժ�...");

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //��סԺҩ��ѭ�����ɺʹ�ӡÿ��ҩ���İ�ҩ������ҩ���Ų�ͬ��
            ArrayList alTemp = this.departmentManager.GetConstantList("PHARMACYTYPE");
            for (int i = 0; i < alTemp.Count; i++)
            {
                Application.DoEvents();
                Neusoft.FrameWork.Models.NeuObject phaDept = alTemp[i] as Neusoft.FrameWork.Models.NeuObject;
                if (phaDept.Memo != "סԺ")
                {
                    continue;
                }

                ArrayList alAll = new ArrayList();
                if (this.ShowByNurseCell1)
                {
                    alAll = this.QueryData(this.deptTemp, phaDept.ID);
                }
                else
                {
                    alAll = this.QueryData(this.operVar.Dept.ID, phaDept.ID);
                }
                
                ArrayList al = new ArrayList();

                //��ȡ����ѡ������
                for (int k = 0; k < alAll.Count; k++)
                {
                    Neusoft.HISFC.Models.Pharmacy.ApplyOut appoutObj = alAll[k] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;
                    if (this.hsSelected.ContainsKey(appoutObj.ID))
                    {
                        al.Add(appoutObj);
                    }
                }

                if (al == null || al.Count == 0)
                {
                    continue;
                }
                Neusoft.HISFC.Models.Pharmacy.DrugBillClass dbc = new Neusoft.HISFC.Models.Pharmacy.DrugBillClass();
                dbc.ApplyDept.ID = this.operVar.Nurse.ID;
                dbc.ApplyDept.Name = this.operVar.Nurse.Name;
                dbc.ApplyDept.User01 = "��ʿ��ӡ";
                dbc.ApplyState = "0";
                dbc.DrugBillNO = "";

                //���ɰ�ҩ���Ų�д����������
                string tempDrugBill = this.itemManager.GetNewDrugBillNO();
                if (tempDrugBill == null || tempDrugBill == "")
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("��ȡ��ҩ����ʧ��!�����±���.", "��ʾ", MessageBoxButtons.OK);
                    this.isBusy = false;
                    return;
                }
                for (int a = 0; a < al.Count; a++)
                {
                    Neusoft.HISFC.Models.Pharmacy.ApplyOut appTemp = al[a] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;
                    if (this.itemManager.UpdateApplyDrugBillByNumber(tempDrugBill, appTemp.ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show("������ҩ����ʧ��!", "��ʾ", MessageBoxButtons.OK);
                        this.isBusy = false;
                        return;
                    }
                }

                //��ҩ����ҩ�����顢��Һֱ�Ӹ��·�ҩ״̬=5������Ҫҩ���ٴν��д�ӡ
                if (this.itemManager.UpdateApplyDrugBill(phaDept.ID, tempDrugBill) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("���·�ҩ״̬ʧ��!", "��ʾ", MessageBoxButtons.OK);
                    this.isBusy = false;
                    return;
                }
                #region ���°�ҩ���뵵IamRabbit by Sunjh 2010-7-20 {2ACDE2D8-C7A2-4ad4-919F-EFAA6EC58A03}
                Neusoft.HISFC.Models.Pharmacy.ApplyOut appOutTemp = al[0] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;
                Neusoft.HISFC.Models.Pharmacy.DrugMessage drugMessage = new Neusoft.HISFC.Models.Pharmacy.DrugMessage();
                drugMessage.ApplyDept = appOutTemp.ApplyDept;    //���һ��߲���
                drugMessage.DrugBillClass.ID = this.nowObj.ToString();        //��ҩ������
                drugMessage.DrugBillClass.Name = this.billClassName;        //��ҩ������
                drugMessage.SendType = appOutTemp.SendType;     //��������0ȫ��,1-����,2-��ʱ
                drugMessage.SendFlag = 0;                     //״̬0-֪ͨ,1-�Ѱ�
                drugMessage.StockDept = phaDept;   //��ҩ����
                if (this.drugStoreManager.SetDrugMessage(drugMessage) != 1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("���°�ҩ��״̬ʧ��!", "��ʾ", MessageBoxButtons.OK);
                    this.isBusy = false;
                    return;
                }
                #endregion

                dbc.ID = this.nowObj.ToString(); ;
                dbc.Name = this.billClassName;

                //�������ɵİ�ҩ���Ŵ�ӡ��ҩ��
                dbc.DrugBillNO = tempDrugBill;

                //��ʿվֱ�Ӵ�ӡ��ҩ���������ͬʱ��ӡ���ܵ�����ϸ����
                //Function.IDrugPrint.AddAllData(al, dbc);
                //Function.IDrugPrint.Print();

                #region �����Զ����ҩ�����ʹ�ӡ

                //���ݰ�ҩ����ӡ���ý��ж�����ʽ��ӡ
                //Neusoft.FrameWork.Models.NeuObject printObj = new Neusoft.FrameWork.Models.NeuObject();
                //printObj.ID = phaDept.ID;
                //printObj.User01 = this.nowObj.ToString();
                ////ArrayList alPrintSet = drugStoreManager.QueryPrintSetting(printObj);
                //ArrayList alPrintSet = new ArrayList();
                //if (alPrintSet != null && alPrintSet.Count > 0)
                //{
                //    for (int j = 0; j < alPrintSet.Count; j++)
                //    {
                //        Neusoft.HISFC.Models.Pharmacy.DrugBillClass tempBillClass = new Neusoft.HISFC.Models.Pharmacy.DrugBillClass();
                //        tempBillClass = dbc;
                //        Neusoft.FrameWork.Models.NeuObject tempPrintObj = alPrintSet[j] as Neusoft.FrameWork.Models.NeuObject;
                //        tempBillClass.PrintType.ID = tempPrintObj.User03;
                //        Function.IDrugPrint.AddAllData(al, dbc);
                //        Function.IDrugPrint.Print();
                //        //Function.IDrugPrint.Preview();
                //    }
                //}
                //else
                //{
                //    Function.IDrugPrint.AddAllData(al, dbc);
                //    Function.IDrugPrint.Print();
                //    //Function.IDrugPrint.Preview();
                //}

                #endregion

            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            MessageBox.Show("���沢���͵�ҩ���ɹ�!");
            this.isBusy = false;

            //���ô�ӡ�ӿ�
            //Function.IDrugPrint.AddAllData(al, dbc);
            //Function.IDrugPrint.Preview();
        }

        /// <summary>
        /// ���´�ӡ��ҩ��
        /// </summary>
        private void RePrintDrugBill()
        {
            if (this.tvBill.SelectedNode == null)
            {
                MessageBox.Show("û��ѡ���ҩ�����д�ӡ!");
                return;
            }
            ArrayList al = this.itemManager.QueryApplyOutListByNurseBill(this.tvBill.SelectedNode.Text);
            if (al == null || al.Count == 0)
            {
                MessageBox.Show("û�п��Դ�ӡ������!");
                return;
            }

            Neusoft.HISFC.Models.Pharmacy.ApplyOut applyoutObj = al[0] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;                       

            Neusoft.HISFC.Models.Pharmacy.DrugBillClass dbc = new Neusoft.HISFC.Models.Pharmacy.DrugBillClass();
            dbc.ApplyDept.ID = this.operVar.Nurse.ID;
            dbc.ApplyDept.Name = this.operVar.Nurse.Name;
            dbc.ApplyDept.User01 = "��ʿ����";
            dbc.ApplyState = "0";
            dbc.DrugBillNO = this.tvBill.SelectedNode.Text;
            //���ݰ�ҩ����ӡ���ý��ж�����ʽ��ӡ
            Neusoft.FrameWork.Models.NeuObject printObj = new Neusoft.FrameWork.Models.NeuObject();
            printObj.ID = applyoutObj.StockDept.ID;
            Neusoft.HISFC.Models.Pharmacy.DrugBillClass dbcObj = this.drugStoreManager.GetDrugBillClass(applyoutObj.BillClassNO);
            printObj.User01 = dbcObj.ID;//this.nowObj.ToString();
            dbc.ID = applyoutObj.BillClassNO;
            //ArrayList alPrintSet = drugStoreManager.QueryPrintSetting(printObj);
            ArrayList alPrintSet = new ArrayList();
            if (alPrintSet != null && alPrintSet.Count > 0)
            {
                for (int j = 0; j < alPrintSet.Count; j++)
                {
                    Neusoft.HISFC.Models.Pharmacy.DrugBillClass tempBillClass = new Neusoft.HISFC.Models.Pharmacy.DrugBillClass();
                    tempBillClass = dbc;
                    Neusoft.FrameWork.Models.NeuObject tempPrintObj = alPrintSet[j] as Neusoft.FrameWork.Models.NeuObject;
                    tempBillClass.PrintType.ID = tempPrintObj.User03;                 
                    Function.IDrugPrint.AddAllData(al, dbc);
                    Function.IDrugPrint.Print();
                    //Function.IDrugPrint.Preview();
                }
            }
            else
            {
                Function.IDrugPrint.AddAllData(al, dbc);
                Function.IDrugPrint.Print();
                //Function.IDrugPrint.Preview();
            }

            #region �����ʿվ�ȴ�ӡ�ˣ�ҩ���Ͳ���Ҫ��ӡ�� by Sunjh {C0985F9D-F26F-49be-803E-783FD0451FC3}

            if (this.chkUpdatePrintFlag.Checked)
            {
                //ֱ�Ӹ��·�ҩ״̬=5������Ҫҩ���ٴν��д�ӡ
                if (this.itemManager.UpdateApplyDrugBill(applyoutObj.StockDept.ID, this.tvBill.SelectedNode.Text) == -1)
                {
                    MessageBox.Show("���·�ҩ״̬ʧ��!", "��ʾ", MessageBoxButtons.OK);
                    return;
                }
                #region ���°�ҩ���뵵IamRabbit by Sunjh 2010-7-20 {2ACDE2D8-C7A2-4ad4-919F-EFAA6EC58A03}
                Neusoft.HISFC.Models.Pharmacy.DrugMessage drugMessage = new Neusoft.HISFC.Models.Pharmacy.DrugMessage();
                drugMessage.ApplyDept = applyoutObj.ApplyDept;    //���һ��߲���
                drugMessage.DrugBillClass.ID = applyoutObj.BillClassNO;        //��ҩ������
                drugMessage.DrugBillClass.Name = dbcObj.Name; // applyoutObj.BillClassNO;        //��ҩ������
                drugMessage.SendType = applyoutObj.SendType;     //��������0ȫ��,1-����,2-��ʱ
                drugMessage.SendFlag = 0;                     //״̬0-֪ͨ,1-�Ѱ�
                drugMessage.StockDept = applyoutObj.StockDept;   //��ҩ����
                if (this.drugStoreManager.SetDrugMessage(drugMessage) != 1)
                {
                    MessageBox.Show("���°�ҩ��״̬ʧ��!", "��ʾ", MessageBoxButtons.OK);
                    return;
                }
                #endregion
            }
            

            #endregion 
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSelectAll.Checked)
            {
                for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                {
                    this.sheetViewTot.Cells[i, 2].Text = "True";
                    this.sheetViewTot.Rows[i].BackColor = Color.GreenYellow;
                }
            }
            else
            {
                for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                {
                    this.sheetViewTot.Cells[i, 2].Text = "False";
                    this.sheetViewTot.Rows[i].BackColor = Color.White;
                }
            }
        }

        #endregion

        #endregion

        #region �¼�

        private void ucSendDrugQuery_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() != "devenv")
            {
                this.Init();

                this.ShowData();
            }
            /// �Ƿ���ʾ��Ϣ ���³��������� {55B34E3A-7741-4420-A947-29DE881A0119} wbo 2010-12-28
            if (this.isShowMessage)
            {
                MessageBox.Show("�Ѿ����͵�ҩ����ҩƷ�����Ҫȡ�����ڱ���ȡ�����ɣ�\r\n1��ҩƷ���з��� �˵����棬ҩƷ���з���ȡ��\r\n2���벻ҪƵ�����͡�ȡ��������Ӱ��ҩ����ҩ", "������ʾ");
            }
        }

        private void rbSend_CheckedChanged(object sender, EventArgs e)
        {
            this.Query();
        }

        private void tvDept_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent == null)          //���ڵ�
                {
                    if (this.SpreadDrug.Sheets[0].Rows.Count > 0) {
                        this.SpreadDrug.Sheets[0].Rows.Count = 0;
                    }
                    return;//�˴����ε�modified by xizf {F3BB8B98-09BD-4bef-9720-B40AD4892F4B}
                    this.nowObj = null;
                    this.showLevel = 0;
                }
                else
                {
                    if (this.treeType == NodeType.Dept && this.neuTabControl1.SelectedTab == this.tpQuality)
                    {
                        this.nowObj = (e.Node.Tag as Neusoft.FrameWork.Models.NeuObject).ID;
                    }                        
                    else
                    {
                        this.nowObj = e.Node.Tag;
                        this.billClassName = e.Node.Text;
                    }
                        
                    this.showLevel = 1;
                }
                this.Query(this.nowObj, this.showLevel);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.Msg("��ѯͳ��ִ�г���" + ex.Message));
                return;
            }
        }

        private void nlbFilter_TextChanged(object sender, EventArgs e)
        {
            this.Filter();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void ucQueryInpatientNo1_myEvent_1()
        {
            try
            {
                this.isQueryByCaseNO = true;
                this.InPatientNo = this.ucQueryInpatientNo1.InpatientNo;
                this.Query(this.InPatientNo, 2);
                this.nowObj = this.InPatientNo;
                this.showLevel = 1;
                this.QueryNoExamData();
                this.ucQueryInpatientNo1.Text = this.InPatientNo.Substring(4);
                this.isQueryByCaseNO = false;
            }
            catch
            {
                MessageBox.Show(Language.Msg("ͨ��סԺ�Ž���ȡҩ/ȷ����Ϣ��ѯ����!"));
                return;
            }
        }

        private void SpreadDrug_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.treeType == NodeType.Dept)//ֻ�������б���ʾΪ����ʱ����Ч
            {              
                //��ʾ��ϸ��Ϣʱ ����˫�����д���
                if (this.SpreadDrug.ActiveSheet == this.sheetViewDetail)
                    return;

                string drugCode = this.sheetViewTot.Cells[e.Row, 0].Text;
                string class3Meaning = this.sheetViewTot.Cells[e.Row, 1].Text;

                DataSet ds = new DataSet();
                if (this.neuTabControl1.SelectedTab == this.tpQuality)		//��ҩƷ����Tabҳ
                {
                    #region ҩƷ����Tabҳ��ʾ��ϸ

                    string[] strIndex = new string[1] { "Pharmacy.Item.GetAppplyOutTot.ByQuality.Drug" };
                    this.itemManager.ExecQuery(strIndex, ref ds, this.operVar.Dept.ID, drugCode, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString(),class3Meaning);
                    if (ds != null && ds.Tables.Count > 0)
                        this.sheetViewDetail.DataSource = ds;
                    if (!this.SpreadDrug.Sheets.Contains(this.sheetViewDetail))
                        this.SpreadDrug.Sheets.Add(this.sheetViewDetail);
                    this.SpreadDrug.ActiveSheet = this.sheetViewDetail;

                    #region ��ʽ��
                    try
                    {
                        this.sheetViewDetail.DefaultStyle.Locked = true;
                        this.sheetViewDetail.Columns[0].Width = 100F;       //���
                        this.sheetViewDetail.Columns[1].Width = 100F;       //ȡҩ����
                        this.sheetViewDetail.Columns[2].Width = 160F;		//ҩƷ����
                        this.sheetViewDetail.Columns[3].Width = 80F;		//���
                        this.sheetViewDetail.Columns[4].Width = 80F;		//��ҩ��
                        this.sheetViewDetail.Columns[5].Width = 50F;		//��λ
                    }
                    catch { }
                    #endregion

                    #endregion
                }
                else															//��ȡҩ����Tabҳ
                {

                    #region ȡҩ����Tabҳ��ʾ��ϸ

                    if (this.tvDept.SelectedNode != null && this.tvDept.Nodes != null && this.tvDept.SelectedNode == this.tvDept.Nodes[0])
                        return;                    

                    string[] strIndex = new string[1] { "Pharmacy.Item.GetApplyOutTot.ByMedDept.Drug" };

                    if (this.nowObj != null)
                    {
                        this.itemManager.ExecQuery(strIndex, ref ds, this.operVar.Dept.ID, this.nowObj as string,drugCode, this.dtpBegin.Value.ToString(), this.dtpEnd.Value.ToString(),class3Meaning);
                    }

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        this.sheetViewDetail.DataSource = ds;
                    }
                    if (!this.SpreadDrug.Sheets.Contains(this.sheetViewDetail))
                        this.SpreadDrug.Sheets.Add(this.sheetViewDetail);
                    this.SpreadDrug.ActiveSheet = this.sheetViewDetail;
                    this.SetDetailFormat();

                    #endregion
                }

                this.SetFpColor();
            }
        }

        private void neuTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkComb.Enabled = true;
            this.chkIsByPatient.Enabled = true;
            this.chkSelectAll.Enabled = true;
            this.chkUpdatePrintFlag.Enabled = false;

            if (this.neuTabControl1.SelectedIndex == 1)
            {
                this.sheetViewTot.RowCount = 0;
            }

            if (this.neuTabControl1.SelectedTab == this.tpBill)
            {
                this.chkComb.Enabled = false;
                this.chkIsByPatient.Enabled = false;
                this.chkSelectAll.Enabled = false;
                this.chkUpdatePrintFlag.Enabled = true;
                this.tvBill.Nodes.Clear();

                ArrayList alBill = new ArrayList();
                if (this.ShowByNurseCell1)
                {
                    //�Ըû���վ��Ӧ�İ�ҩ�����в�ѯ
                    string dept = "";
                    ArrayList alNurse = this.departmentManager.QueryNurseStationByDept(this.operVar.Dept, "01");
                    foreach (Neusoft.FrameWork.Models.NeuObject infoTemp in alNurse)
                    {
                        ArrayList deptCell = this.departmentManager.QueryDepartment(infoTemp.ID);
                        foreach (Neusoft.FrameWork.Models.NeuObject info in deptCell)
                        {
                            if (dept == "")
                            {
                                dept = info.ID;
                            }
                            else
                            {
                                dept = dept + "','" + info.ID;
                            }                                
                        }                        
                    }
                    alBill = this.itemManager.QueryNursePrintBill(dept);
                }
                else
                {
                    alBill = this.itemManager.QueryNursePrintBill(this.operVar.Dept.ID);
                }
                
                for (int i = 0; i < alBill.Count; i++)
                {
                    Neusoft.FrameWork.Models.NeuObject nodeObj = alBill[i] as Neusoft.FrameWork.Models.NeuObject;
                    this.tvBill.Nodes.Add(nodeObj.ID);
                }
            }
            else if (this.neuTabControl1.SelectedTab == this.tpQuality)
            {
                if (this.tvDrugType1.Nodes.Count > 0)
                {
                    this.tvDrugType1.SelectedNode = this.tvDrugType1.Nodes[0];
                    //���ζ��Ѱڡ�δ�ڵ���ʾ ����ʾ����ʹ��
                    //this.IsShowCheck = true;
                }
            }
            else
            {
                if (this.tvDept.Nodes.Count > 0)
                {
                    this.tvDept.SelectedNode = this.tvDept.Nodes[0];
                    this.IsShowCheck = false;
                }
            }
        }

        private void tvBill_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Query(e.Node.Text);
        }

        private void neuTabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.neuTabControl2.SelectedTab == this.tpQuitFee || this.neuTabControl2.SelectedTab == this.tpOutBill)
            {
                this.QueryNoExamData();
            }
        }

        private void neuTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.Filter();
        }
        //��ӡ
        //protected override void OnPrint(PaintEventArgs e)
        //{
        //    Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
        //    print.PrintPage(0,0,this.neuTabControl2.SelectedTab);
        //    base.OnPrint(e);
        //}
        //��ӡ��ť
        protected override int OnPrintPreview(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.PrintPreview(0,0,this.neuTabControl2.SelectedTab);
            return base.OnPrintPreview(sender, neuObject);
        }

        #endregion

        private void SpreadDrug_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (this.chkIsByPatient.Checked)
            {
                if (this.sheetViewTot.Cells[e.Row, 2].Text == "True")
                {
                    for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                    {
                        if (this.sheetViewTot.Cells[i, 0].Text == this.sheetViewTot.Cells[e.Row, 0].Text)
                        {
                            this.sheetViewTot.Cells[i, 2].Text = "True";
                            this.sheetViewTot.Rows[i].BackColor = Color.GreenYellow;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                    {
                        if (this.sheetViewTot.Cells[i, 0].Text == this.sheetViewTot.Cells[e.Row, 0].Text)
                        {
                            this.sheetViewTot.Cells[i, 2].Text = "False";
                            this.sheetViewTot.Rows[i].BackColor = Color.White;
                        }
                    }
                }
            }
            else if (this.chkComb.Checked)
            {
                if (this.sheetViewTot.Cells[e.Row, 2].Text == "True")
                {
                    for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                    {
                        if (this.sheetViewTot.Cells[i, 24].Text == this.sheetViewTot.Cells[e.Row, 24].Text)
                        {
                            this.sheetViewTot.Cells[i, 2].Text = "True";
                            this.sheetViewTot.Rows[i].BackColor = Color.GreenYellow;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.sheetViewTot.RowCount; i++)
                    {
                        if (this.sheetViewTot.Cells[i, 24].Text == this.sheetViewTot.Cells[e.Row, 24].Text)
                        {
                            this.sheetViewTot.Cells[i, 2].Text = "False";
                            this.sheetViewTot.Rows[i].BackColor = Color.White;
                        }
                    }
                }
            }
            else
            {
                if (this.sheetViewTot.Cells[e.Row, 2].Text == "True")
                {
                    this.sheetViewTot.Rows[e.Row].BackColor = Color.GreenYellow;
                }
                else
                {
                    this.sheetViewTot.Rows[e.Row].BackColor = Color.White;
                }
            }
        }

        private void chkIsByPatient_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsByPatient.Checked)
            {
                this.chkComb.Checked = false;
            }
        }

        private void chkComb_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkComb.Checked)
            {
                this.chkIsByPatient.Checked = false;
            }
        }

    }
}
