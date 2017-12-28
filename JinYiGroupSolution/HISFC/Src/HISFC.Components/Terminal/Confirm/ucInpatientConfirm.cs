using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FarPoint.Win.Spread;
namespace Neusoft.HISFC.Components.Terminal.Confirm
{
    public partial class ucInpatientConfirm : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucInpatientConfirm()
        {
            InitializeComponent();
        }

        #region ˽�б���
        public bool seeAll = false;
        public bool clearConfirm = false;
        Neusoft.HISFC.Models.Base.MessType messType =  Neusoft.HISFC.Models.Base.MessType.N;
        private bool addFirstRow = false;
        Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm terminalMgr = new Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm();
        Neusoft.HISFC.Components.Common.Controls.EnumShowItemType itemType = Neusoft.HISFC.Components.Common.Controls.EnumShowItemType.DeptItem;
        Neusoft.ApplyInterface.HisInterface PACSApplyInterface = null;
        Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam cpMgr = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
        #region {5E5299D8-95A2-4498-B2F1-52D00E4FB11A}
        Neusoft.HISFC.Components.PacsApply.HisInterface PACSApplyInterfaceNew = null;
        #endregion
        #endregion

        #region  ����
        /// <summary>
        /// ����Ƿ������
        /// </summary>
        [Category("�ؼ�����"), Description("Ƿ���ж���ʾ����")]
        public Neusoft.HISFC.Models.Base.MessType MessType
        {
            get
            {
                return messType;
            }
            set
            {
                messType = value;
            }
        }
        /// <summary>
        /// ��ʾ����Ŀ���
        /// </summary>
        [Category("�ؼ�����"), Description("��ʾ����Ŀ���")]
        public Neusoft.HISFC.Components.Common.Controls.EnumShowItemType ItemType
        {
            get
            {
                return itemType;
            }
            set
            {
                itemType = value;
            }
        }
        /// <summary>
        /// �鿴���п����ն�ȷ����Ŀ
        /// </summary>
        [Category("�ؼ�����"), Description("�鿴���п����ն�ȷ����Ŀ")]
        public bool SeeAll
        {
            get
            {
                return seeAll;
            }
            set
            {
                seeAll = value;
            }
        }
        /// <summary>
        /// ���������Ƿ�ɾ����ȷ����Ŀ
        /// </summary>
        [Category("�ؼ�����"), Description("���������Ƿ�ɾ����ȷ����Ŀ")]
        public bool ClearConfirm
        {
            get
            {
                return clearConfirm;
            }
            set
            {
                clearConfirm = value;
            }
        }


        /// <summary>
        /// �������Ƿ��ڵ�һ��
        /// </summary>
        [Category("�ؼ�����"), Description("�������Ƿ��ڵ�һ��")]
        public bool AddFirstRow
        {
            get
            {
                return addFirstRow;
            }
            set
            {
                addFirstRow = value;
            }
        }
        #endregion

        #region ����

        private enum Cols
        {
            IsExec, //0
            Except,
            ItemCode,//1
            ItemName,//2
            doc_name,//3
            ItemQty,//4
            ItemAlreadConfirmQty,//5
            ItemConfirmQty,//6
            Unit,//7
            Price,//8
            TotCost,//9
            OrderType,//10
            //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
            ItemStatus,//10��Ŀ״̬��δȷ�ϡ�����ȷ�ϡ�ȫ��ȷ�ϣ�ͨ�������������ۺϱȽ�
            Machine,//11��Ŀʹ���豸����ҽ���������в���
            Operator//12��ʦ��Ĭ���ǵ�ǰ����Ա�������޸�
            //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.RADT.PatientInfo myPatient = new Neusoft.HISFC.Models.RADT.PatientInfo();

        /// <summary>
        /// ����Ա
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee oper = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Clone();

        /// <summary>
        /// ҽ��ҵ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Order orderManager = new Neusoft.HISFC.BizProcess.Integrate.Order();

        /// <summary>
        /// ����ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Fee feeManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// �ն�ҵ��
        /// </summary>
        private Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm terminalManager = new Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm();

        /// <summary>
        /// �ն�ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm confirmIntergrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm();
        //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
        /// <summary>
        /// ҽ������ҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Terminal.TerminalCarrier terminalCarrier = new Neusoft.HISFC.BizLogic.Terminal.TerminalCarrier();                        

        private ArrayList alExecOrder = new ArrayList();

        private DataTable dtExecOrder = new DataTable();

        private DataView dvExecOrder = new DataView();

        private string filePath = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath + @"\Profile\TecExecOrder.xml";

        /// <summary>
        /// ToolBarService
        /// </summary>
        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        /// <summary>
        /// ��Ŀѡ��ؼ�
        /// </summary>
        public Neusoft.HISFC.Components.Common.Controls.ucItemList ucItemList;

        public Neusoft.HISFC.BizProcess.Integrate.RADT radt = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        private int currentRow = 0;
        private int currentColumn = 0;
        private DateTime dtNow = DateTime.Now;

        //�Ƿ�ʹ�õ������뵥
        private string isUseDL = "0";

        #region addby xuewj 2010-9-21 {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�

        /// <summary>
        /// �Ƿ��ӡ�������˵�
        /// </summary>
        private bool isPrintFeeSheet = false;

        [Category("�ؼ�����"),Description("�Ƿ��ӡ�������˵�")]
        public bool IsPrintFeeSheet
        {
            get { return isPrintFeeSheet; }
            set { isPrintFeeSheet = value; }
        }

        /// <summary>
        /// ���˵���ӡ�ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet nurseFeeBill = null;

        #endregion

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ����շ���Ŀ��Ϣ...");
            Application.DoEvents();

            //this.ucItemList = new ucItemList(Neusoft.HISFC.Components.Common.Controls.EnumShowItemType.Undrug);
            //this.ucItemList = new Neusoft.HISFC.Components.Common.Controls.ucItemList(itemType);

            this.ucItemList = new Neusoft.HISFC.Components.Common.Controls.ucItemList();

            // �����Ŀ�б�
            this.Controls.Add(this.ucItemList);
            // ������Ŀ�б��ɼ�

            this.ucItemList.enuShowItemType = itemType;
            this.ucItemList.Init(string.Empty);


            this.ucItemList.Visible = false;
            // ʹ��Ŀ�б���ǰ

            this.ucItemList.BringToFront();
            // �շ���Ŀ�б�ѡ����Ŀ�¼�
            this.ucItemList.SelectItem += new Neusoft.HISFC.Components.Common.Controls.ucItemList.MyDelegate(ucItemList_SelectItem);
            this.ucQueryInpatientNo1.myEvent += new Neusoft.HISFC.Components.Common.Controls.myEventDelegate(ucQueryInpatientNo1_myEvent);//{C30E2F4A-3BC4-4b98-973F-C734537F4EA4}

            this.InitFp();

            this.fpExecOrder.KeyEnter += new Neusoft.FrameWork.WinForms.Controls.NeuFpEnter.keyDown(fpExecOrder_KeyEnter);

            #region addby xuewj 2010-11-11 �������뵥��ȡ���������ļ�{457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
            //isUseDL = cpMgr.GetControlParam<string>("200212");
            isUseDL = Neusoft.FrameWork.Function.NConvert.ToInt32(Neusoft.HISFC.Components.Common.Classes.Function.LoadMenuSet()).ToString(); 
            #endregion
            this.QueryTermalDept();
        }

        //{D614378E-677D-4a84-891B-D0E2D47D3E00}

        private ArrayList alTermalDept = new ArrayList();

        private int QueryTermalDept()
        {
            //Neusoft.HISFC.BizProcess.Integrate.Manager managerInt = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.HISFC.BizLogic.Manager.DepartmentStatManager dsmManager = new Neusoft.HISFC.BizLogic.Manager.DepartmentStatManager();

            alTermalDept = dsmManager.LoadDepartmentStatAndByNodeKind( "98","1");

            //this.alTermalDept = managerInt.GetConstantList("Termin");
            
            return 1;
        }

        void ucQueryInpatientNo1_myEvent() //{C30E2F4A-3BC4-4b98-973F-C734537F4EA4}
        {
            Neusoft.HISFC.Models.RADT.PatientInfo patient = radt.QueryPatientInfoByInpatientNO(this.ucQueryInpatientNo1.InpatientNo);
            if (patient != null)
            {
                this.myPatient = patient;
                lblName.Text = "����������" + this.myPatient.Name;
                lblPatientNO.Text = "סԺ�ţ�" + this.myPatient.PID.PatientNO;
                lblDept.Text = "���߿��ң�" + this.myPatient.PVisit.PatientLocation.Dept.Name;
                lblFreeCost.Text = "������" + this.myPatient.FT.LeftCost.ToString();
                lblPack.Text = "��ͬ��λ��" + this.myPatient.Pact.Name;

                if (this.seeAll)
                {
                    alExecOrder = this.orderManager.QueryExecOrderByDept(patient.ID, "2", false, "all");
                }
                else
                {
                    alExecOrder = this.orderManager.QueryExecOrderByDept(patient.ID, "2", false, oper.Dept.ID);
                }
                if (alExecOrder != null)
                {
                    this.fpExecOrder.Sheets[0].RowCount = 0;
                    this.AddExecOrderToFp(alExecOrder);
                }
            }
            else
            {
                MessageBox.Show("�޴˻���!");
                this.fpExecOrder.Sheets[0].RowCount = 0;
                lblName.Text = "����������" ;
                lblPatientNO.Text = "סԺ�ţ�";
                lblDept.Text = "���߿��ң�";
                lblFreeCost.Text = "������";
                lblPack.Text = "��ͬ��λ��" ;
                return;
            }
        }

        /// <summary>
        /// ��ʼ�����
        /// </summary>
        private void InitFp()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Neusoft.FrameWork.Management.Language.Msg("���ڳ�ʼ��������Ժ�....."));
            try
            {
                if (System.IO.File.Exists(this.filePath))
                {

                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(this.filePath, this.dtExecOrder, ref this.dvExecOrder, this.fpExecOrder_Sheet1);

                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpExecOrder_Sheet1, this.filePath);
                }
                else
                {
                    this.dtExecOrder.Columns.AddRange(new DataColumn[]
                    {
                        new DataColumn("ִ�б��",typeof(bool)),
                        new DataColumn("ִ�п���",typeof(string)),
                        new DataColumn("��Ŀ����",typeof(string)),
                        new DataColumn("��Ŀ����",typeof(string)),
                        //���ӿ���ҽ����ʾ {C675754A-973E-4619-BCCD-B459CCD3BD0D} hzl 2010-11-16 
                        new DataColumn("����ҽ��",typeof(string)),
                        new DataColumn("������",typeof(decimal)),
                        new DataColumn("��ȷ������",typeof(decimal)),
                        new DataColumn("ȷ������",typeof(decimal)),
                        new DataColumn("��λ",typeof(string)),
                        new DataColumn("����",typeof(decimal)),
                        new DataColumn("�ܶ�",typeof(decimal)),
                        new DataColumn("SOURCE",typeof(string)),
                        //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                        new DataColumn("״̬",typeof(string)),
                        new DataColumn("��Ӧ�豸",typeof(string)),
                        new DataColumn("��ʦ",typeof(string))
                        //by yuyun 08-7-7
                    });

                    this.dvExecOrder = new DataView(this.dtExecOrder);

                    this.fpExecOrder.DataSource = this.dvExecOrder;

                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpExecOrder_Sheet1, this.filePath);
                }
            }
            catch
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            this.fpExecOrder_Sheet1.DefaultStyle.Locked = true;

            this.fpExecOrder_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.IsExec].Locked = false;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.Except].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemName].Locked = false;
            //���ӿ���ҽ����ʾ {C675754A-973E-4619-BCCD-B459CCD3BD0D} hzl 2010-11-16
            this.fpExecOrder_Sheet1.Columns[(int)Cols.doc_name].Locked = false;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemQty].Locked = true;
            //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
            //this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemQty].Visible = false;
            //by yuyun 08-7-7
            #region {DEF0DE5C-96BF-4f48-80DC-297B9B16315A}
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemCode].Locked = true;
            #endregion
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemAlreadConfirmQty].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemConfirmQty].Locked = false;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.Unit].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.Price].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.TotCost].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.OrderType].Locked = true;
            //by yuyun 08-7-8{52355595-B401-4db9-82BC-A3650F11D2CC}
            this.fpExecOrder_Sheet1.Columns[(int)Cols.ItemStatus].Locked = true;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.Machine].Locked = false;
            this.fpExecOrder_Sheet1.Columns[(int)Cols.Operator].Locked = false;
            //�ڼ�ʦ���м�����Ա�б�ѡ��
            ArrayList al = new ArrayList();
            Neusoft.HISFC.BizProcess.Integrate.Manager ztManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            al = ztManager.QueryEmployeeAll();
            //.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.T);
            this.fpExecOrder.SetColumnList(this.fpExecOrder_Sheet1, (int)Cols.Operator, al);
            //�豸�м���ҽ���豸���ݣ�ֻ��ʾ��ǰ����Ա���ڿ��ҵ�ҽ���豸
            al = this.terminalCarrier.GetDesigns(this.oper.Dept.ID);
            if (al.Count > 0)
            {
                ArrayList alTer = new ArrayList();
                foreach (HISFC.Models.Terminal.TerminalCarrier terObj in al)
                {
                    alTer.Add(new Neusoft.FrameWork.Models.NeuObject(terObj.CarrierCode, terObj.CarrierName, ""));
                }
                this.fpExecOrder.SetColumnList(this.fpExecOrder_Sheet1, (int)Cols.Machine, alTer);
            }
            this.fpExecOrder.SetItem += new Neusoft.FrameWork.WinForms.Controls.NeuFpEnter.setItem(fpExecOrder_SetItem);
            //by yuyun 08-7-8
        }
        //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
        private int fpExecOrder_SetItem(Neusoft.FrameWork.Models.NeuObject obj)
        {
            this.fpExecOrder_Sheet1.Cells[this.fpExecOrder_Sheet1.ActiveRowIndex, this.fpExecOrder_Sheet1.ActiveColumnIndex].Text = obj.Name;
            this.fpExecOrder_Sheet1.Cells[this.fpExecOrder_Sheet1.ActiveRowIndex, this.fpExecOrder_Sheet1.ActiveColumnIndex].Tag = obj;
            return 0;
        }
        //{23016A93-22CE-4fe6-9CF4-1F9E90B3DD08}
        private bool IsExist(Neusoft.HISFC.Models.Order.ExecOrder order)
        {

            for (int i = 0; i < this.alTermalDept.Count; i++)
            {
                Neusoft.HISFC.Models.Base.DepartmentStat obj = alTermalDept[i] as Neusoft.HISFC.Models.Base.DepartmentStat;
                if (order.Order.ExeDept.ID == obj.DeptCode)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ���ҽ�����ݵ����
        /// </summary>
        /// <param name="alExecOrder"></param>
        /// <returns></returns>
        protected virtual int AddExecOrderToFp(ArrayList alExecOrder)
        {
            try
            {
                foreach (Neusoft.HISFC.Models.Order.ExecOrder order in alExecOrder)
                {
                    #region {5197289A-AB55-410b-81EE-FC7C1B7CB5D7}
                    //���������ն�ȷ��ҽ������ʿ�ֽ⣬��û�д�û�б��������Ҳ��ʾ����������
                    if (order.Order.OrderType.IsDecompose)
                    {
                        if (!this.orderManager.CheckLongUndrugIsConfirm(order.ID))
                        {
                            continue;
                        }
                    }
                    //{23016A93-22CE-4fe6-9CF4-1F9E90B3DD08}
                    if (this.seeAll)
                    {
                        if (this.IsExist(order)) //�����ڣ�����
                        {
                            continue;
                        }
                    }

                    #endregion

                    this.fpExecOrder.Sheets[0].Rows.Add(0, 1);
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemName].Locked = true;//{15387EC4-F638-4084-A4F4-0A6FF4AF13D9} ����ҽ������
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Except].Text = order.Order.ExeDept.Name;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.IsExec].Value = order.IsExec;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemCode].Text = order.Order.Item.ID;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemName].Text = order.Order.Item.Name;
                    //���ӿ���ҽ����ʾ {C675754A-973E-4619-BCCD-B459CCD3BD0D} hzl 2010-11-16
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.doc_name].Text = order.Order.ReciptDoctor.Name;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemQty].Text = order.Order.Qty.ToString();

                    decimal AlreadyQty = terminalMgr.GetAlreadConfirmNum(order.Order.ID, order.ID);
                    if (AlreadyQty < 0)
                    {
                        MessageBox.Show("��ȡ��ȷ����Ŀ����ʧ��" + terminalMgr.Err);
                        return -1;
                    }
                    decimal LeaveQty = order.Order.Qty - AlreadyQty;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemAlreadConfirmQty].Text = AlreadyQty.ToString();
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemConfirmQty].Text = LeaveQty.ToString();
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Unit].Text = order.Order.Unit;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Price].Text = order.Order.Item.Price.ToString();
                    if (order.Order.Item.Price == 0 && order.Order.Unit != "[������]")//by zhouxs 2007-10-28
                    {
                        this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Price].Locked = false;
                    }//end zhouxs
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.TotCost].Text = Convert.ToString(order.Order.Item.Price * order.Order.Qty);
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.OrderType].Text = "ORDER";
                    //by yuyun 08-7-8{07D1BACB-8E4F-4ac8-8254-81763D0F0699}
                    if (AlreadyQty > 0 && AlreadyQty < order.Order.Qty)
                    {
                        this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemStatus].Text = "����ȷ��";
                        this.fpExecOrder.Sheets[0].Rows[0].BackColor = Color.LightBlue;//������ȷ�ϵ���Ŀ����ɫ����
                    }
                    else
                    {
                        this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.ItemStatus].Text = "δȷ��";
                    }
                    //ҽ���豸
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Machine].Text = "";
                    ////////////////////
                    //��ʦ
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Operator].Text = this.oper.Name;
                    this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.Operator].Tag = this.oper.ID;
                    //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}

                    this.fpExecOrder.Sheets[0].Rows[0].Tag = order;
                }
                #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
                if (!this.seeAll)
                {
                    this.fpExecOrder.Sheets[0].RowCount += 1;
                    decimal totCost = this.SumCost();
                    int activeRowIndex = this.fpExecOrder.Sheets[0].RowCount - 1;
                    this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.TotCost].Text = totCost.ToString();
                    this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.Unit].Text = "�ϼ�:";
                    this.fpExecOrder.Sheets[0].Rows[activeRowIndex].Locked = true;
                    if (this.fpExecOrder.Sheets[0].RowCount == 1)
                    {
                        this.AddNewRow();
                    }
                }
                else
                {
                    if (this.fpExecOrder.Sheets[0].RowCount > 0)
                    {
                        this.fpExecOrder.Sheets[0].RowCount += 1;
                        decimal totCost = this.SumCost();
                        int activeRowIndex = this.fpExecOrder.Sheets[0].RowCount - 1;
                        this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.TotCost].Text = totCost.ToString();
                        this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.Unit].Text = "�ϼ�:";
                        this.fpExecOrder.Sheets[0].Rows[activeRowIndex].Locked = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ر���ʽ���������TecExecOrder.xml�ļ�" + ex.Message);

                return -1;
            }
            return 0;
        }

        #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
        /// <summary>
        /// ���㵱ǰѡ����Ŀ���ܽ��
        /// </summary>
        /// <returns></returns>
        protected virtual decimal SumCost()
        {
            string tempPrice = "";
            decimal totCost = 0m;
            for (int i = 0; i < this.fpExecOrder_Sheet1.RowCount; i++)
            {
                if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.OrderType].Text != "")
                {
                    tempPrice = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.TotCost].Text;
                    totCost += Neusoft.FrameWork.Function.NConvert.ToDecimal(tempPrice);
                }
            }

            return totCost;
        } 
        #endregion

        /// <summary>
        /// ѡ����Ŀ�б�����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int ucItemList_SelectItem(Keys key)
        {
            this.InsertItem();
            this.fpExecOrder.Focus();//add by xuewj 2010-9-27 ˫����Ŀ�б� ��������fp {E10F354A-2EB3-4caf-AE16-F87F78464DF5}
            return 0;
        }

        /// <summary>
        /// �˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override Neusoft.FrameWork.WinForms.Forms.ToolBarService Init(object sender, object neuObject, object param)
        {
            base.Init(sender, neuObject, param);

            this.toolBarService.AddToolButton("����", "������Ŀ�Ļ�����Ϣ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.T���, true, false, null);
            this.toolBarService.AddToolButton("ˢ��", "ˢ�»�����Ϣ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sˢ��, true, false, null);
            #region {53061BA2-33FC-469d-9773-9A9454023612}
            this.toolBarService.AddToolButton("ɾ��", "ɾ�������Ĳ�����Ŀ�Ļ�����Ϣ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sɾ��, true, false, null);
            #endregion
            //by yuyun 08-7-8{C46CFCDE-132B-47c0-ADAC-4AD73DA2FD90}
            //this.toolBarService.AddToolButton("��������", "��������", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Dҽ��, true, false, null);
            //by yuyun 08-7-8
            return toolBarService;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "����":
                    if (this.seeAll)
                    {
                        MessageBox.Show("��ѯȫԺȷ����Ϣʱ ���������շ���Ŀ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    this.AddNewRow();
                    break;
                #region {53061BA2-33FC-469d-9773-9A9454023612}
                case "ɾ��":
                    DeleteItemEvent();
                    break;
                #endregion
                case "ˢ��":
                    this.Clear();
                    this.tv.Refresh();
                    break;
                //by yuyun 08-7-8{CFBF5A32-66FF-468e-A26E-624DE186BD9C}
                //case "��������":
                //    this.Clear();
                //    //todo
                //    break;
                //by yuyun 08-7-8
            }
            base.ToolStrip_ItemClicked(sender, e);
        }
        #region {53061BA2-33FC-469d-9773-9A9454023612}
        /// <summary>
        /// ɾ������Ŀ�¼�

        /// </summary>

        void DeleteItemEvent()
        {
            this.MakeItemListDisappear();
            this.DeleteNew();
        }

        /// <summary>
        /// ɾ��һ��(��ǰ��)����Ŀ

        /// </summary>
        public void DeleteNew()
        {
            // ���û�м�¼���򷵻�
            if (this.fpExecOrder_Sheet1.RowCount == 0)
            {
                return;
            }
            // ֻ��ɾ������Ŀ

            if (this.GetItem(this.fpExecOrder_Sheet1.ActiveRowIndex, (int)Cols.OrderType) != "NEW")
            {
                MessageBox.Show("����Ŀ������ɾ��", "ҽ���ն�ȷ��");
                //this.Focus();
                //this.CellFocus(this.fpExecOrder_Sheet1.ActiveRowIndex, (int)Cols.OrderType);
                return;
            }
            // ɾ��
            this.DeleteRow(this.fpExecOrder_Sheet1.ActiveRowIndex, true);
            #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
            decimal totCost = this.SumCost();
            int activeRowIndex = this.fpExecOrder.Sheets[0].RowCount - 1;
            this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.TotCost].Text = totCost.ToString(); 
            #endregion
        }
        /// <summary>
        /// ��ָ�����к�ɾ��һ��

        /// [����1: int row - Ҫɾ�����к�]
        /// [����2: bool confirm - �Ƿ���Ҫ�û�ȷ��]
        /// </summary>
        /// <param name="row">Ҫɾ�����к�</param>
        /// <param name="confirm">�Ƿ���Ҫȷ��</param>
        public void DeleteRow(int row, bool confirm)
        {
            // ����ǿռ�¼����ֱ��ɾ����������Ҫȷ��ɾ��

            if (confirm && (!this.IsNull(row)))
            {
                // ���ȡ��ɾ������ô����

                if (MessageBox.Show("�Ƿ�ɾ����ǰ�У�", "ҽ���ն�ȷ��",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    this.Focus();
                    return;
                }
            }
            //{8C0239CE-4272-4f30-B547-9C3C27D694E4} ֹͣ�༭ģʽ������ɾ��ʱ�����˵�ǰѡ���ı�
            this.fpExecOrder.EditMode = false;
            // ɾ��ָ������
            this.fpExecOrder_Sheet1.RemoveRows(row, 1);
            // ���ý��㵽��һ��

            //this.fpExecOrder_Sheet1.Focus();
            //if (row - 1 >= 0)
            //{
            //    this.CurrentRow = row - 1;
            //    // ���ý��㵽��Ŀ����

            //    this.CurrentColumn = (int)DisplayField.ItemName;
            //}
        }
        /// <summary>
        /// �ж�ָ�����Ƿ�Ϊ�գ������Ŀ���14��Ϊ�վ���Ϊ�ǿռ�¼��

        /// [����: int row - �к�]
        /// [����: bool,true - ��, false - �ǿ�]
        /// </summary>
        /// <param name="row">ָ�����к�</param>
        /// <returns>true��Ϊ��/false����Ϊ��</returns>
        public bool IsNull(int row)
        {
            // �����Ŀ����Ϊ�գ�������ĿΪ��

            if (this.fpExecOrder_Sheet1.Cells[row, (int)Cols.ItemCode].Text.Equals(""))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// ��ȡһ��CELL��ֵ

        /// [����1: int row - �к�]
        /// [����2: int column - �к�]
        /// [����: string, �ı�ֵ]
        /// </summary>
        /// <param name="row">��</param>
        /// <param name="column">��</param>
        /// <returns>CELL������ı�</returns>
        public string GetItem(int row, int column)
        {
            return this.fpExecOrder_Sheet1.Cells[row, column].Text;
        }
        /// <summary>
        /// ���շ���Ŀѡ��ؼ����ɼ�

        /// </summary>
        private void MakeItemListDisappear()
        {
            if (this.ucItemList.Visible == true)
            {
                this.ucItemList.Visible = false;
            }
        }
        #endregion
        private void Clear()
        {
            this.myPatient = new Neusoft.HISFC.Models.RADT.PatientInfo();
            lblName.Text = "����������";
            lblPatientNO.Text = "סԺ�ţ�";
            lblDept.Text = "���߿��ң�";
            lblFreeCost.Text = "������";
            this.fpExecOrder.Sheets[0].RowCount = 0;
        }

        /// <summary>
        /// ��ѡ��
        /// </summary>
        /// <param name="neuObject"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override int OnSetValue(object neuObject, TreeNode e)
        {

            Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
            if (neuObject != null)
            {
                if (neuObject.GetType() == patientInfo.GetType())
                {
                    patientInfo = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;
                    this.myPatient = patientInfo;
                    lblName.Text = "����������" + this.myPatient.Name;
                    lblPatientNO.Text = "סԺ�ţ�" + this.myPatient.PID.PatientNO;
                    lblDept.Text = "���߿��ң�" + this.myPatient.PVisit.PatientLocation.Dept.Name;
                    lblFreeCost.Text = "������" + this.myPatient.FT.LeftCost.ToString();
                    if (this.seeAll)
                    {
                        alExecOrder = this.orderManager.QueryExecOrderByDept(patientInfo.ID, "2", false, "all");
                    }
                    else
                    {
                        alExecOrder = this.orderManager.QueryExecOrderByDept(patientInfo.ID, "2", false, oper.Dept.ID);
                    }
                    if (alExecOrder != null)
                    {
                        this.fpExecOrder.Sheets[0].RowCount = 0;
                        this.AddExecOrderToFp(alExecOrder);
                    }
                }
            }
            else
            {

                return -1;
            }

            return base.OnSetValue(neuObject, e);
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            this.Init();
            base.OnLoad(e);
            InputMap im;
            im = fpExecOrder.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = fpExecOrder.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Down, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = fpExecOrder.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Up, Keys.None), FarPoint.Win.Spread.SpreadActions.None);
        }

        /// <summary>
        /// ����»�������
        /// </summary>
        private void AddNewRow()
        {
            if (this.myPatient == null || this.myPatient.ID == "")
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��û��ѡ���ߣ�"));
                return;
            }
            #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
            //int rowcount = this.fpExecOrder.Sheets[0].RowCount;
            int rowcount = this.fpExecOrder.Sheets[0].RowCount - 1; 
            #endregion
            if (AddFirstRow)
            {
                this.fpExecOrder.Sheets[0].Rows.Add(0, 1);
                this.fpExecOrder.Sheets[0].Cells[0, (int)Cols.OrderType].Text = "NEW";
                #region {15181E09-0842-4e3f-A91C-A25F3930CA28}
                this.fpExecOrder.Sheets[0].SetActiveCell(0, (int)Cols.ItemName, true);
                #endregion

            }
            else
            {
                this.fpExecOrder.Sheets[0].Rows.Add(rowcount, 1);
                this.fpExecOrder.Sheets[0].Cells[rowcount, (int)Cols.OrderType].Text = "NEW";
                #region {15181E09-0842-4e3f-A91C-A25F3930CA28}
                this.fpExecOrder.Sheets[0].SetActiveCell(rowcount, (int)Cols.ItemName, true);
                #endregion
            }
            #region {15181E09-0842-4e3f-A91C-A25F3930CA28}
            this.fpExecOrder.EditMode = true;
            //this.fpExecOrder.Focus();
            #endregion
            this.currentRow = rowcount;
        }

        /// <summary>
        /// ����»������ݵ����
        /// </summary>
        private void InsertItem()
        {
            Neusoft.HISFC.Models.Base.Item item = null;
            Neusoft.HISFC.Models.Fee.Item.Undrug undrug = null;
            int intReturn = this.ucItemList.GetSelectItem(out item);

            if (item == null || item.ID == "")
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("û���ҵ���Ŀ"));
                return;
            }
            undrug = feeManager.GetUndrugByCode(item.ID);
            if (intReturn > 0)
            {

                this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.IsExec].Value = true;
                this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.ItemCode].Text = undrug.ID;
                this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.ItemName].Text = undrug.Name;
                this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Unit].Text = undrug.PriceUnit;
                if (undrug.UnitFlag == "0")
                {
                    this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price].Text = undrug.Price.ToString();
                }
                else if (undrug.UnitFlag == "1")
                {
                    //{85F65949-AD94-422f-93C9-588D0072F2AA} ����Ҫ����ȡ�۸�
                    //this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price].Text = feeManager.GetUndrugCombPrice(item.ID).ToString();
                    this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price].Text = undrug.Price.ToString();
                }
                this.UnDisplayUcItemList();
                this.fpExecOrder.Sheets[0].ActiveRow.Tag = item;
            }
        }


        /// <summary>
        /// ��Ŀѡ���б�����
        /// </summary>
        public void UnDisplayUcItemList()
        {
            if (this.ucItemList.Visible)
            {
                this.ucItemList.Visible = false;
            }
        }


        /// <summary>
        /// �������ת����order
        /// </summary>
        /// <returns></returns>
        private ArrayList GetFeeOrder()
        {
            int rowCount = this.fpExecOrder.Sheets[0].RowCount;
            ArrayList alFeeOrder = new ArrayList();
            for (int i = 0; i < rowCount; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value))
                {
                    Neusoft.HISFC.Models.Base.OperEnvironment o = new Neusoft.HISFC.Models.Base.OperEnvironment();
                    o.ID = oper.ID;
                    o.Name = oper.Name;
                    o.Dept = oper.Dept;
                    o.OperTime = this.dtNow;
                    if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.OrderType].Text == "ORDER")
                    {
                        Neusoft.HISFC.Models.Order.ExecOrder order = new Neusoft.HISFC.Models.Order.ExecOrder(); ;
                        order = (Neusoft.HISFC.Models.Order.ExecOrder)this.fpExecOrder.Sheets[0].Rows[i].Tag;
                        Neusoft.HISFC.Models.Fee.Item.Undrug undrug = new Neusoft.HISFC.Models.Fee.Item.Undrug();
                        undrug = this.feeManager.GetUndrugByCode(order.Order.Item.ID);
                        order.Order.Item.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.ItemConfirmQty].Value);
                        obj = order.Order;
                        obj.Item.MinFee = undrug.MinFee;
                        obj.Item.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Price].Value);
                        obj.ExecOper = o;
                        obj.ExecOper.Dept = order.Order.ExeDept.Clone();
                        obj.Oper = o;
                        obj.User03 = order.ID;
                        //ִ���豸
                        //by yuyun 08-7-7{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                        if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Machine].Value != null)
                        {
                            obj.Item.User01 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Machine].Value.ToString();
                        }
                        //ִ�м�ʦ
                        if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Operator].Value != null)
                        {
                            obj.Item.User02 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Operator].Value.ToString();
                        }
                        alFeeOrder.Add(obj);
                    }

                }
            }

            return alFeeOrder;
        }

        /// <summary>
        /// ����е������շ�����ת����FeeItemList
        /// </summary>cc
        /// <returns></returns>
        private ArrayList GetNewFeeItemList()
        {
            int rowCount = this.fpExecOrder.Sheets[0].RowCount;
            ArrayList alFeeItemList = new ArrayList();
            for (int i = 0; i < rowCount; i++)
            {
                //{A22E7A8E-DE5E-40fc-8273-F774B286B7C8}����������Ŀ�����۴���
                //Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value))
                {
                    //if (Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder_Sheet1.Cells[i, (int)Cols.ItemQty].Text) <= 0)
                    //{
                    //    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����շ���Ŀ����д����"));
                    //    return null;
                    //}
                    Neusoft.HISFC.Models.Base.OperEnvironment o = new Neusoft.HISFC.Models.Base.OperEnvironment();
                    o.ID = oper.ID;
                    o.Name = oper.Name;
                    o.Dept = oper.Dept;
                    o.OperTime = this.dtNow;
                    //------------------------
                    if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.OrderType].Text == "NEW")
                    {
                        Neusoft.HISFC.Models.Base.Item item = new Neusoft.HISFC.Models.Base.Item(); ;
                        Neusoft.HISFC.Models.Fee.Item.Undrug undrug = new Neusoft.HISFC.Models.Fee.Item.Undrug();
                        item = this.fpExecOrder.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Base.Item;
                        #region {B56C131D-2600-421c-9D51-12A1C214CA1E}
                        if (item == null)
                        {
                            continue;
                        }
                        #endregion
                        undrug = this.feeManager.GetUndrugByCode(item.ID);
                        //{F6A2DCD5-10B9-4fac-8ACB-D4B6FE9D684F}��סԺ�����˵���Ŀ��unitflagΪ��ʱ���÷���û���ϵ���ʾȷ�ϳɹ���
                        if (string.IsNullOrEmpty(undrug.UnitFlag))
                        {
                            MessageBox.Show(undrug.Name + "����Ŀ�Ȳ�����ϸ��Ŀ��Ҳ����������Ŀ�����ܶ�����мƷѣ�������ά������Ŀ��Ϣ��");
                            return null;
                        }
                        //----------------------------------------------
                        if (undrug.UnitFlag == "0")
                        {
                            //{A22E7A8E-DE5E-40fc-8273-F774B286B7C8}����������Ŀ�����۴���
                            Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();

                            obj.Item = item;
                            obj.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.ItemConfirmQty].Text);
                            obj.ReciptDept = oper.Dept;
                            obj.ReciptDoctor.ID = oper.ID;
                            obj.ExecOper = o;
                            obj.Unit = item.PriceUnit;
                            obj.Oper = o;
                            obj.ExeDept = oper.Dept;//by yuyun 08-8-12 ����ȷ�Ͽ���
                            //{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                            obj.Item.User01 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Machine].Text;//ִ���豸
                            obj.Item.User02 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Operator].Text;//ִ�м�ʦ
                            alFeeItemList.Add(obj);
                        }
                        else if (undrug.UnitFlag == "1")
                        {
                            Neusoft.HISFC.BizProcess.Integrate.Manager ztManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                            ztManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                            ArrayList alZtDetail = ztManager.QueryUndrugPackageDetailByCode(undrug.ID);
                            foreach (Neusoft.HISFC.Models.Fee.Item.Undrug undrugitem in alZtDetail)
                            {
                                //{A22E7A8E-DE5E-40fc-8273-F774B286B7C8}����������Ŀ�����۴���
                                Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();

                                Neusoft.HISFC.Models.Fee.Item.Undrug myUndrug = this.feeManager.GetUndrugByCode(undrugitem.ID);
                                obj.Item = myUndrug as Neusoft.HISFC.Models.Base.Item;
                                obj.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.ItemConfirmQty].Text);

                                //{6EFEC5EC-2258-4d3e-877B-179215E2F783} ���¼�����ϸ����
                                obj.Item.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.ItemConfirmQty].Text) * undrugitem.Qty;

                                obj.ReciptDept = oper.Dept;
                                obj.ReciptDoctor.ID = oper.ID;
                                obj.Unit = undrugitem.PriceUnit;
                                obj.ExecOper = o;
                                obj.ExeDept = oper.Dept;//by yuyun 08-8-12 {58B76F7C-A35D-4cbb-8948-8163EA3C5191}
                                obj.Oper = o;
                                //{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                                obj.Item.User01 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Machine].Text;//ִ���豸
                                obj.Item.User02 = this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.Operator].Text;//ִ�м�ʦ
                                alFeeItemList.Add(obj);
                            }
                        }

                    }
                }
            }

            return alFeeItemList;
        }

        /// <summary>
        /// �������ת����execorder
        /// </summary>
        /// <returns></returns>
        private ArrayList GetExecOrder()
        {
            int rowCount = this.fpExecOrder.Sheets[0].RowCount;
            ArrayList alNeedExecOrder = new ArrayList();
            for (int i = 0; i < rowCount; i++)
            {

                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value))
                {
                    Neusoft.HISFC.Models.Base.OperEnvironment o = new Neusoft.HISFC.Models.Base.OperEnvironment();
                    o.ID = oper.ID;
                    o.Name = oper.Name;
                    o.OperTime = this.dtNow;
                    o.Dept = oper.Dept;
                    if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.OrderType].Text == "ORDER")
                    {
                        Neusoft.HISFC.Models.Order.ExecOrder order = new Neusoft.HISFC.Models.Order.ExecOrder(); ;
                        order = (Neusoft.HISFC.Models.Order.ExecOrder)this.fpExecOrder.Sheets[0].Rows[i].Tag;

                        order.Order.ExecOper = o;
                        order.ExecOper = o;
                        order.ExecOper.Dept = order.Order.ExeDept.Clone();
                        order.IsExec = true;
                        order.ChargeOper = o;
                        order.IsCharge = true;
                        order.Order.User03 = order.ID;
                        alNeedExecOrder.Add(order);
                    }

                }
            }

            return alNeedExecOrder;
        }
        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        /// <returns></returns>
        private bool ValidState()
        {
            for (int i = 0; i < this.fpExecOrder_Sheet1.RowCount; i++)
            {
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value))
                {
                    if (Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder_Sheet1.Cells[i, (int)Cols.ItemConfirmQty].Text) <= 0)
                    {
                        MessageBox.Show("��������Ҫȷ�ϵ�����");
                        return false;
                    }
                    if (this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.OrderType].Text == "ORDER")
                    {
                        decimal totQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder_Sheet1.Cells[i, (int)Cols.ItemQty].Text);
                        decimal alreadQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder_Sheet1.Cells[i, (int)Cols.ItemAlreadConfirmQty].Text);
                        decimal confirmQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder_Sheet1.Cells[i, (int)Cols.ItemConfirmQty].Text);
                        if (totQty < alreadQty + confirmQty)
                        {
                            MessageBox.Show("��ȷ��������������ȷ���������ܴ���" + (totQty - alreadQty).ToString());
                            return false;
                        }
                    }
                    if (this.fpExecOrder_Sheet1.Cells[i, (int)Cols.OrderType].Text == "NEW")
                    {
                        #region {B56C131D-2600-421c-9D51-12A1C214CA1E}
                        Neusoft.HISFC.Models.Base.Item item = new Neusoft.HISFC.Models.Base.Item(); ;
                        item = this.fpExecOrder.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Base.Item;
                        if (item == null)
                        {
                            MessageBox.Show("��������Ҫȷ�ϵ���Ŀ��");
                            return false;
                        }
                        #endregion
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// ����ת����feeitemlist
        /// </summary>
        /// <param name="alOrder"></param>
        /// <returns></returns>
        private ArrayList ChangeOrderToFeeItemList(ArrayList alOrder)
        {
            ArrayList alFeeItemList = new ArrayList();
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in alOrder)
            {
                Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();

                feeItemList.Item = order.Item.Clone();
                feeItemList.Item.PriceUnit = order.Unit;//��λ���¸�
                feeItemList.RecipeOper.Dept = order.ReciptDept.Clone();
                feeItemList.RecipeOper.ID = order.ReciptDoctor.ID;
                feeItemList.RecipeOper.Name = order.ReciptDoctor.Name;
                feeItemList.ExecOper = order.ExecOper.Clone();
                feeItemList.ExecOper.Dept = order.ExeDept.Clone();
                feeItemList.StockOper.Dept = order.StockDept.Clone();
                if (feeItemList.Item.PackQty == 0)
                {
                    feeItemList.Item.PackQty = 1;
                }
                feeItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber((feeItemList.Item.Price * feeItemList.Item.Qty / feeItemList.Item.PackQty), 2);
                feeItemList.FT.OwnCost = feeItemList.FT.TotCost;
                feeItemList.IsBaby = order.IsBaby;
                feeItemList.IsEmergency = order.IsEmergency;
                feeItemList.Order = order.Clone();
                feeItemList.ExecOrder.ID = order.User03;
                feeItemList.NoBackQty = feeItemList.Item.Qty;
                feeItemList.FTRate.OwnRate = 1;
                feeItemList.BalanceState = "0";
                feeItemList.ChargeOper = order.Oper.Clone();
                feeItemList.FeeOper = order.Oper.Clone();
                feeItemList.TransType = Neusoft.HISFC.Models.Base.TransTypes.Positive;
                feeItemList.Item.User01 = order.Item.User01;
                feeItemList.Item.User02 = order.Item.User02;
                alFeeItemList.Add(feeItemList);
            }
            return alFeeItemList;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        private int Save()
        {
            #region ������Trans
            //{58B76F7C-A35D-4cbb-8948-8163EA3C5191}
            this.dtNow = this.terminalManager.GetDateTimeFromSysDateTime();
            ArrayList alOrder = new ArrayList();
            ArrayList alFeeItemList = new ArrayList();
            ArrayList alNeedExecOrder = new ArrayList();
            int iReturn = 0;
            #region addby xuewj 2010-9-21 {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�
            string paramRecipeNO = "";//�շѴ�����
            DateTime beginDate = terminalManager.GetDateTimeFromSysDateTime(); 
            #endregion
            //Neusoft.HISFC.BizProcess.Integrate.Terminal.Result result = Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.None;
            if (!ValidState())
            {
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.terminalManager.Connection);
            //t.BeginTransaction();
            this.feeManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.confirmIntergrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            terminalMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.feeManager.MessageType = messType;

            #endregion

            #region ȡ����Ҫ����������ArrayList

            alOrder = this.GetFeeOrder();
            if (alOrder == null)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return -1;
            }
            alFeeItemList = this.GetNewFeeItemList();
            if (alFeeItemList == null)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return -1;
            }

            alFeeItemList = this.ChangeOrderToFeeItemList(alFeeItemList);
            alNeedExecOrder = this.GetExecOrder();
            #endregion

            #region ��ָ�����Ŀ{856164A9-000A-482f-B9F4-2A2FF44F96B3}

            Neusoft.HISFC.BizProcess.Integrate.Manager managerPack = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.HISFC.BizProcess.Integrate.Fee tempManagerFee = new Neusoft.HISFC.BizProcess.Integrate.Fee();

            ArrayList alOrderTemp = new ArrayList();

            for (int i = 0; i < alOrder.Count; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order oldOrder = alOrder[i] as Neusoft.HISFC.Models.Order.Inpatient.Order;
                Neusoft.HISFC.Models.Fee.Item.Undrug ug = tempManagerFee.GetItem((oldOrder as Neusoft.HISFC.Models.Order.Inpatient.Order).Item.ID);
                if (ug.UnitFlag == "1")
                {
                    ArrayList al = managerPack.QueryUndrugPackageDetailByCode(oldOrder.Item.ID);
                    foreach (Neusoft.HISFC.Models.Fee.Item.Undrug undrug in al)
                    {
                        Neusoft.HISFC.Models.Fee.Item.Undrug tmpUndrug = tempManagerFee.GetItem(undrug.ID);
                        Neusoft.HISFC.Models.Order.Inpatient.Order myorder = null;
                        decimal qty = oldOrder.Qty;
                        myorder = oldOrder.Clone();
                        myorder.Item = tmpUndrug.Clone();
                        myorder.Qty = qty * undrug.Qty;//����==������Ŀ����*С��Ŀ����
                        myorder.Item.Qty = qty * undrug.Qty;//����==������Ŀ����*С��Ŀ����
                        myorder.Package.ID = oldOrder.Item.ID;//������Ŀ����
                        myorder.Package.Name = oldOrder.Item.Name; //������Ŀ����
                        alOrderTemp.Add(myorder);
                    }
                }
                else
                {
                    alOrderTemp.Add(alOrder[i]);
                }
            }
            alOrder = alOrderTemp;

            #endregion

            #region �����շѣ������¿�������

            if ((alOrder == null || alOrder.Count == 0) && (alFeeItemList == null || alFeeItemList.Count == 0))
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("û��ѡ�񱣴��ҽ�����ݣ�"));
                return 100;
            }

            if (alFeeItemList != null && alOrder.Count > 0)
            {
                #region �ж��Ƿ��Ѿ�ȷ�Ϲ�����������̨����ȷ�����ε���� modified by xizf20101220 {B98851B0-9C5A-4d68-ABB5-CB48C4DBD34B}
                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order temporder in alOrder)
                {
                    string exec_sqn = temporder.User03;
                    if (this.feeManager.GetTecFlag(exec_sqn)) {
                        MessageBox.Show("ĳЩ��Ŀ�����Ѿ�ȷ��,������סԺ������");
                        return -1;
                    }
                
                }
                #endregion
                iReturn = this.feeManager.FeeItem(this.myPatient, ref alOrder);
                if (iReturn < 0)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ȡ���߷���ʧ�ܣ�" + this.feeManager.Err));
                    return iReturn;
                }
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItem in alOrder)
                {
                    //{07D1BACB-8E4F-4ac8-8254-81763D0F0699}
                    iReturn = this.feeManager.UpdateNoBackQtyForUndrug(feeItem.RecipeNO, feeItem.SequenceNO, feeItem.Item.Qty, feeItem.BalanceState);
                    if (iReturn < 0)
                    {
                        //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                        feeManager.Rollback();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���·�����ϸ��������ʧ�ܣ�" + this.feeManager.Err));
                        return -1;
                    }
                    iReturn = this.feeManager.UpdateExtFlagForUndrug(feeItem.RecipeNO, feeItem.SequenceNO, "5", feeItem.BalanceState);
                    if (iReturn < 0)
                    {
                        //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                        feeManager.Rollback();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���·�����ϸ��չ���ʧ�ܣ�" + this.feeManager.Err));
                        return -1;
                    }

                    #region addby xuewj 2010-9-21 {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�
                    if (isPrintFeeSheet)
                    {
                        if (feeItem.RecipeNO != "" && !paramRecipeNO.Contains(feeItem.RecipeNO))
                        {
                            paramRecipeNO = "'" + feeItem.RecipeNO + "'," + paramRecipeNO;
                        }
                    } 
                    #endregion
                }

            }
            if (alFeeItemList != null && alFeeItemList.Count > 0)
            {
                iReturn = this.feeManager.FeeItem(this.myPatient, ref alFeeItemList);
                if (iReturn < 0)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ȡ���߷���ʧ�ܣ�" + this.feeManager.Err));
                    return iReturn;
                }

                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItem in alFeeItemList)
                {
                    //{07D1BACB-8E4F-4ac8-8254-81763D0F0699}
                    iReturn = this.feeManager.UpdateNoBackQtyForUndrug(feeItem.RecipeNO, feeItem.SequenceNO, feeItem.Item.Qty, feeItem.BalanceState);
                    if (iReturn < 0)
                    {
                        //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                        feeManager.Rollback();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���·�����ϸ��������ʧ�ܣ�" + this.feeManager.Err));
                        return -1;
                    }
                    iReturn = this.feeManager.UpdateExtFlagForUndrug(feeItem.RecipeNO, feeItem.SequenceNO, "5", feeItem.BalanceState);
                    if (iReturn < 0)
                    {
                        //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                        feeManager.Rollback();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���·�����ϸ��չ���ʧ�ܣ�" + this.feeManager.Err));
                        return -1;
                    }

                    #region addby xuewj 2010-9-21 {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�
                    if (isPrintFeeSheet)
                    {
                        if (feeItem.RecipeNO != "" && !paramRecipeNO.Contains(feeItem.RecipeNO))
                        {
                            paramRecipeNO = "'" + feeItem.RecipeNO + "'," + paramRecipeNO;
                        }
                    } 
                    #endregion
                }

            }

            #endregion

            #region �����ն�
            //Ŀǰ�������նˣ���Ҫʱ�޸ı����ִ���
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItem in alOrder)
            {
                Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail detail = new Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail();

                #region ����ȷ����ϸ
                string applySequence = "";
                int sReturn = terminalMgr.GetNextSequence(ref applySequence);
                if (sReturn == -1)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show("��ȡȷ����ˮ��ʧ��");
                    return -1;
                }
                detail.MoOrder = feeItem.Order.ID;//ҽ����ˮ�� 0
                detail.ExecMoOrder = feeItem.ExecOrder.ID;//ҽ����ִ�е���ˮ��1 
                detail.Sequence = applySequence;//2
                detail.Apply.Item.ID = feeItem.Item.ID;//3
                detail.Apply.Item.Name = feeItem.Item.Name;//4
                detail.Apply.Item.ConfirmedQty = feeItem.Item.Qty;//5
                detail.Apply.ConfirmOperEnvironment.ID = this.oper.ID;//6
                detail.Apply.ConfirmOperEnvironment.Dept.ID = this.oper.Dept.ID;//7
                #region {3EF2F4C8-D9CF-4e8c-87A8-5DA22B2597C8}
                detail.Apply.ConfirmOperEnvironment.OperTime = this.dtNow;//8
                //detail.Apply.ConfirmOperEnvironment.OperTime = System.DateTime.Now;//8
                #endregion
                detail.Status.ID = "0";//9 0-������1-ȡ����2-�˷�
                detail.Apply.Patient.ID = feeItem.Patient.ID;
                detail.Apply.Item.RecipeNO = feeItem.RecipeNO;
                detail.Apply.Item.SequenceNO = feeItem.SequenceNO;
                //{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                detail.ExecDevice = feeItem.Item.User01;
                detail.Oper.ID = feeItem.Item.User02;

                #endregion
                if (terminalMgr.InsertInpatientConfirmDetail(detail) == -1)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����ն�ȷ����ϸʧ�ܣ�" + this.confirmIntergrate.Err));
                    return -1;
                }
            }

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItem in alFeeItemList)
            {
                Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail detail = new Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail();

                #region ����ȷ����ϸ
                string applySequence = "";
                int sReturn = terminalMgr.GetNextSequence(ref applySequence);
                if (sReturn == -1)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show("��ȡȷ����ˮ��ʧ��");
                    return -1;
                }
                detail.MoOrder = feeItem.Order.ID;//ҽ����ˮ�� 0
                detail.ExecMoOrder = feeItem.ExecOrder.ID;//ҽ����ִ�е���ˮ��1 
                detail.Sequence = applySequence;//2
                detail.Apply.Item.ID = feeItem.Item.ID;//3
                detail.Apply.Item.Name = feeItem.Item.Name;//4
                detail.Apply.Item.ConfirmedQty = feeItem.Item.Qty;//5
                detail.Apply.ConfirmOperEnvironment.ID = this.oper.ID;//6

                //�޸�������������Ŀ��ȷ�Ͽ��ҵ�bug by yuyun {58B76F7C-A35D-4cbb-8948-8163EA3C5191}
                detail.Apply.ConfirmOperEnvironment.Dept.ID = this.oper.Dept.ID;//7
                detail.Apply.ConfirmOperEnvironment.OperTime = this.dtNow;//8
                //---------------------------------------------------------------------------------

                detail.Status.ID = "0";//9 0-������1-ȡ����2-�˷�
                detail.Apply.Patient.ID = feeItem.Patient.ID;
                detail.Apply.Item.RecipeNO = feeItem.RecipeNO;
                detail.Apply.Item.SequenceNO = feeItem.SequenceNO;
                //{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                detail.ExecDevice = feeItem.Item.User01;
                detail.Oper.ID = feeItem.Item.User02;
                #endregion
                if (terminalMgr.InsertInpatientConfirmDetail(detail) == -1)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����ն�ȷ����ϸʧ�ܣ�" + this.confirmIntergrate.Err));
                    return -1;
                }
            }
            #endregion

            #region ����ҽ��ȷ�Ϻ��շ�

            foreach (Neusoft.HISFC.Models.Order.ExecOrder execOrder in alNeedExecOrder)
            {
                execOrder.ExecOper.OperTime = dtNow;
                execOrder.ChargeOper.OperTime = dtNow;

                iReturn = this.orderManager.UpdateRecordExec(execOrder);
                if (iReturn < 0)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ��ȷ����Ϣʧ�ܣ�" + this.orderManager.Err));
                    return iReturn;
                }
                iReturn = this.orderManager.UpdateChargeExec(execOrder);
                if (iReturn < 0)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ���շ���Ϣʧ�ܣ�" + this.orderManager.Err));
                    return iReturn;
                }
                iReturn = this.orderManager.UpdateOrderStatus(execOrder.Order.ID, 2);
                if (iReturn < 0)
                {
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
                    feeManager.Rollback();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ������ִ����Ϣʧ�ܣ�" + this.orderManager.Err));
                    return iReturn;
                }
            }

            #endregion

            //Neusoft.FrameWork.Management.PublicTrans.Commit();
            //{3EE6172A-301B-4d16-91C7-E5D8AC94D942}
            feeManager.Commit();
            for (int i = this.fpExecOrder.Sheets[0].RowCount - 1; i >= 0; i--)
            {

                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value))
                {
                    if (clearConfirm)
                    {
                        this.fpExecOrder.Sheets[0].Rows.Remove(i, 1);
                    }
                    else
                    {
                        this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Value = false;
                        this.fpExecOrder.Sheets[0].Cells[i, (int)Cols.IsExec].Locked = true;
                        this.fpExecOrder_Sheet1.Rows[i].BackColor = System.Drawing.Color.Azure;
                    }
                }
            }

            #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
            
            if (!string.IsNullOrEmpty(isUseDL) && isUseDL == "1")
            {
                try
                {
                    if (PACSApplyInterface == null)
                    {
                        PACSApplyInterface = new Neusoft.ApplyInterface.HisInterface();
                    }
                    if (PACSApplyInterface != null)
                    {
                        foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f in alOrder)
                        {
                            if (f.Item.SysClass.ID.ToString() == "UC" && f.Order.ID != null)
                            {
                                try
                                {
                                    string applyNo = string.Empty;
                                    terminalMgr.GetApplyNoByOrderNo(f.Order.ID, ref applyNo);
                                    Neusoft.HISFC.Models.Order.Inpatient.Order order = orderManager.QueryOneOrder(f.Order.ID);
                                    applyNo = order.ApplyNo;
                                    int a = PACSApplyInterface.Charge(applyNo, "1");
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("���µ������뵥�շѱ�־ʱ����\n" + e.Message);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            #endregion

            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ȷ�ϳɹ���"));
            this.ucQueryInpatientNo1.Focus();
            #region  ��ӡ
            if (this.IsPrint)
            {
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList obj in alOrder)
                {
                    if (OnPrint(new object(), obj) == -1)
                    {
                        return -1;
                    }
                }
            }
            #endregion

            #region addby xuewj 2010-9-21 {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�
            if (this.isPrintFeeSheet)
            {
                if (paramRecipeNO != "")
                {
                    MessageBox.Show("������ӡ�������˵�����ȷ�ϴ�ӡ���Ѿ�λ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        paramRecipeNO = paramRecipeNO.Substring(0, paramRecipeNO.Length - 1);
                    if (this.nurseFeeBill == null)
                    {
                        this.nurseFeeBill = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet)) as Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet;
                    }
                    if (this.nurseFeeBill != null)
                    {
                        DateTime endDate = this.terminalManager.GetDateTimeFromSysDateTime();
                        this.nurseFeeBill.NurseFeeBill(beginDate, endDate, paramRecipeNO);
                    }

                }
            }
            #endregion
            return 0;
        }

        /// <summary>
        /// ��ʾ������뵥
        /// </summary>
        private void ShowPacsApply(int rowIndex)
        {
            if (!string.IsNullOrEmpty(isUseDL) && isUseDL == "1")
            {
                #region {5E5299D8-95A2-4498-B2F1-52D00E4FB11A} UpdateApply��Ҫʹ��Neusoft.HISFC.Components.PacsApply.HisInterface,�Ժ���Ҫ�������뵥�ع���Neusoft.ApplyInterface.HisInterface��
                //if (PACSApplyInterface == null)
                //{
                //    PACSApplyInterface = new Neusoft.ApplyInterface.HisInterface();
                //}
                //int rowIndex = this.fpExecOrder.Sheets[0].ActiveRowIndex;
                if (rowIndex == -1)
                {
                    return;
                }
                if (PACSApplyInterfaceNew == null)
                {
                    PACSApplyInterfaceNew = new Neusoft.HISFC.Components.PacsApply.HisInterface(Neusoft.FrameWork.Management.Connection.Operator.ID, (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID);
                }                
                #endregion
                
                if (this.fpExecOrder.Sheets[0].Cells[rowIndex, (int)Cols.OrderType].Text == "ORDER")
                {
                    Neusoft.HISFC.Models.Order.ExecOrder exeOrder = new Neusoft.HISFC.Models.Order.ExecOrder(); ;
                    exeOrder = (Neusoft.HISFC.Models.Order.ExecOrder)this.fpExecOrder.Sheets[0].Rows[rowIndex].Tag;
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = orderManager.QueryOneOrder(exeOrder.Order.ID);
                    if (order == null || order.Item.SysClass.ID.ToString() != "UC")
                        return;
                    if (!string.IsNullOrEmpty(order.ApplyNo))
                    {
                        #region {5E5299D8-95A2-4498-B2F1-52D00E4FB11A}
                        //if (PACSApplyInterface.UpdateApply(order.ApplyNo) < 0)
                        if (PACSApplyInterfaceNew.UpdateApply(order.ApplyNo) < 0)
                        #endregion
                        {
                            MessageBox.Show("��ѯ�������뵥ʧ�ܣ�");
                        }
                    }
                }
            }
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return base.OnSave(sender, neuObject);
        }

        #endregion

        #region �¼�

        /// <summary>
        /// ���EditChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpExecOrder_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string source = this.fpExecOrder.Sheets[0].Cells[e.Row, (int)Cols.OrderType].Text.ToString();
            if (source == "NEW")//e.Row == this.currentRow &&
            {
                if (e.Column == 3)
                {
                    System.Windows.Forms.Control cellControl = this.fpExecOrder.EditingControl;
                    //����λ��
                    this.ucItemList.Location = new System.Drawing.Point(cellControl.Location.X, cellControl.Location.Y + cellControl.Height + 40);
                    ucItemList.BringToFront();
                    // ������Ŀ
                    this.ucItemList.Filter(this.fpExecOrder_Sheet1.ActiveCell.Text);
                    this.ucItemList.Visible = true;
                    // ���浱ǰ�У����ڱ�֤�ƶ����¼�ͷ���ı䵱ǰ��¼
                    this.fpExecOrder_Sheet1.ActiveRowIndex = e.Row;
                    this.currentRow = e.Row;
                }
                else
                {
                    this.UnDisplayUcItemList();
                }
            }
        }

        /// <summary>
        /// ���س�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int fpExecOrder_KeyEnter(Keys key)
        {
            if (key == Keys.Up)
            {
                if (this.ucItemList.Visible)
                {

                    this.ucItemList.PriorRow();
                    this.fpExecOrder_Sheet1.ActiveRowIndex = this.currentRow;
                }
                return 0;
            }
            if (key == Keys.Escape)
            {
                if (this.ucItemList.Visible)
                {

                    this.ucItemList.Visible = false;
                }
                return 0;
            }
            if (key == Keys.Down)
            {
                if (this.ucItemList.Visible)
                {
                    this.ucItemList.NextRow();
                    this.fpExecOrder_Sheet1.ActiveRowIndex = this.currentRow;
                }
                return 0;
            }

            if (key == Keys.Enter)
            {
                if (this.fpExecOrder.Sheets[0].ActiveColumnIndex == (int)Cols.ItemName)
                {
                    this.InsertItem();
                    this.fpExecOrder_Sheet1.ActiveColumnIndex = (int)Cols.ItemConfirmQty;
                    return 0;
                }
                #region ����ȷ������
                if (this.fpExecOrder.Sheets[0].ActiveColumnIndex == (int)Cols.ItemConfirmQty)
                {
                    decimal price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price].Text);
                    decimal qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.ItemConfirmQty].Text);
                    this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.TotCost].Text = Convert.ToString(price * qty);
                    #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
                    decimal totCost = this.SumCost();
                    int activeRowIndex = this.fpExecOrder.Sheets[0].RowCount - 1;
                    this.fpExecOrder.Sheets[0].Cells[activeRowIndex, (int)Cols.TotCost].Text = totCost.ToString(); 
                    #endregion

                    if (price == 0)//by zhouxs 2007-10-28
                    {
                        this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder_Sheet1.ActiveRowIndex, (int)Cols.Price].Locked = false;
                        this.fpExecOrder.Sheets[0].SetActiveCell(this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price);
                        return 0;//end zhouxs
                    }

                    #region addby xuewj 2010-9-27 {C3F7C1B0-97BA-4001-A0B8-6AAB8785C90D} ���Ӻϼ�
                    //if (this.fpExecOrder_Sheet1.ActiveRowIndex == this.fpExecOrder_Sheet1.Rows.Count - 1)
                    if (this.fpExecOrder_Sheet1.ActiveRowIndex == this.fpExecOrder_Sheet1.Rows.Count - 2) 
                    #endregion
                    {
                        fpExecOrder_Sheet1.SetActiveCell(this.fpExecOrder_Sheet1.ActiveRowIndex, (int)Cols.ItemConfirmQty);
                        if (qty > 0)
                        {
                            if (DialogResult.Yes.Equals(MessageBox.Show("�Ƿ��������շ���Ŀ?", "ҽ���ն�ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
                            {
                                this.AddNewRow();
                            }
                        }
                    }
                    else
                    {
                        fpExecOrder_Sheet1.SetActiveCell(this.fpExecOrder_Sheet1.ActiveRowIndex + 1, (int)Cols.ItemConfirmQty);
                    }
                    return 0;
                }
                #endregion
                if (this.fpExecOrder.Sheets[0].ActiveColumnIndex == (int)Cols.Price)
                {
                    decimal price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Price].Text);
                    decimal qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.ItemConfirmQty].Text);
                    this.fpExecOrder.Sheets[0].Cells[this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.TotCost].Text = Convert.ToString(price * qty);
                    this.fpExecOrder.Sheets[0].SetActiveCell(this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.ItemConfirmQty);
                    return 0;
                }
                //by yuyun 08-7-8{810581A3-6DF5-49af-8A5F-D7F843CBEA89}
                if (this.fpExecOrder.Sheets[0].ActiveColumnIndex == (int)Cols.Machine)
                {
                    this.fpExecOrder.Sheets[0].SetActiveCell(this.fpExecOrder.Sheets[0].ActiveRowIndex, (int)Cols.Operator);
                }

            }

            return 0;
        }


        #endregion

        protected override int OnPrint(object sender, object neuObject)
        {
            #region {019B78E5-8076-4d17-8CEE-4F2FC66AD0D3}
            return base.OnPrint(sender, neuObject);
            #endregion
            if (this.terminalInterface == null)
            {
                this.terminalInterface = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(TerminalInterface)) as TerminalInterface;
                if (this.terminalInterface == null)
                {
                    MessageBox.Show("��ýӿ�TerminalInterface����\n������û��ά����صĴ�ӡ�ؼ����ӡ�ؼ�û��ʵ�ֽӿ�TerminalInterface\n����ϵͳ����Ա��ϵ��");
                    return -11;
                }
            }
            if (this.terminalInterface.Reset() == -1)
            {
                return -1;
            }
            //for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
            //{
            if (this.terminalInterface.ControlValue(neuObject) == -1)
            {
                return -1;
            }
            if (this.terminalInterface.Print() == -1)
            {
                return -1;
            }
            return base.OnPrint(sender, neuObject);
        }
        private void ucInpatientConfirm_Load(object sender, EventArgs e)
        {
            if (this.tv != null)
            {
                try
                {
                    tvInpatientConfirm t = (tvInpatientConfirm)tv;
                    if (this.seeAll)
                    {
                        t.OperDept = "all";
                        //t.Init();  
                    }
                    t.Init();

                }
                catch
                { }
            }
        }


        #region IInterfaceContainer ��Ա
        TerminalInterface terminalInterface = null;
        public Type[] InterfaceTypes
        {
            get { return new Type[] { typeof(TerminalInterface) ,
		typeof(Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet)// {9300A7AC-DA0F-472d-B2CF-7F509CB8BE72} �ն�ȷ�ϵ��ü��˵�
            }; }
        }

        #endregion

        private void fpExecOrder_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            ShowPacsApply(e.Row);
        }
    }
}

