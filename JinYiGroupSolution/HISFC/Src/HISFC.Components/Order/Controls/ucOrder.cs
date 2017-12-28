using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.BizProcess.Interface.Order;
using FarPoint.Win.Spread;
using ZZLocal.HISFC.BizLogic.Order;

namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ������]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucOrder : Neusoft.FrameWork.WinForms.Controls.ucBaseControl,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucOrder()
        {
            InitializeComponent();
            this.contextMenu1 = new Neusoft.FrameWork.WinForms.Controls.NeuContextMenuStrip();
        }

        #region ����
        public delegate void EventButtonHandler(bool b);
        //public event EventButtonHandler OrderCanComboChanged;//ҽ���Ƿ��������¼�
        public event EventButtonHandler OrderCanCancelComboChanged;//ҽ���Ƿ����ȡ������¼�
        public event EventButtonHandler OrderCanOperatorChanged;	//ҽ���Ƿ���Ե����������/����
        //public event EventButtonHandler OrderCanSaveChanged;	//ҽ���Ƿ񱣴�
        public event EventButtonHandler OrderCanSetCheckChanged;//�Ƿ�ɴ�ӡ��鵥�¼�

        private bool needUpdateDTBegin = true;
        private Neusoft.FrameWork.WinForms.Controls.NeuContextMenuStrip contextMenu1 = null;
        public int CountLongBegin;//����֮ǰ�ĳ���ҽ������
        public int CountShortBegin;//����֮ǰ����ʱҽ������
        public bool EnabledPass = true;//�Ƿ����������ҩ���
        protected bool EditGroup = false;//�Ƿ�������ױ༭����
        private DataSet dataSet = null; //��ǰDataSet
        private DataView dvLong = null;//��ǰDataView
        private DataView dvShort = null;//��ǰDataView
        private int MaxSort = 0; //���Sort
        protected Neusoft.HISFC.Models.RADT.PatientInfo myPatientInfo = null;
        protected Neusoft.HISFC.BizLogic.Order.Order OrderManagement = new Neusoft.HISFC.BizLogic.Order.Order();
        protected Neusoft.HISFC.BizLogic.Order.AdditionalItem AdditionalItemManagement = new Neusoft.HISFC.BizLogic.Order.AdditionalItem();
        //protected Neusoft.HISFC.BizProcess.Integrate.RADT inpatientManagement = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        protected Neusoft.HISFC.BizLogic.Order.PacsBill pacsBillManagement = new Neusoft.HISFC.BizLogic.Order.PacsBill();
        protected bool dirty = false; //�Ƿ��¼ӣ��޸�ʱ��
        protected ArrayList alDepts;
        protected DataSet dsAllLong;
        protected DataSet dsAllShort;
        protected Neusoft.HISFC.Models.Order.Inpatient.Order currentOrder = null;
        private int iDCControl = 1000;//��������ʾ��ֹͣҽ��ʱ����(��λΪ��)
        private Neusoft.FrameWork.Public.ObjectHelper helper; //��ǰHelper
        private string refreshComboFlag = "2";		//ˢ��ҽ������ 0 ˢ�³��� 1 ˢ������ 2 ��������ȫ��ˢ��
        private Order myOrderClass = new Order();
        ToolTip tooltip = new ToolTip();
        private Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint checkPrint = null;
        //{BFDA551D-7569-47dd-85C4-1CA21FE494BD}
        Neusoft.HISFC.BizProcess.Integrate.Pharmacy pManagement = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        Neusoft.HISFC.BizProcess.Integrate.RADT patient = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        private string checkslipno;

        public string Checkslipno
        {
            get { return checkslipno; }
            set { checkslipno = value; }
        }
        /// <summary>
        /// ҽ��Ȩ����֤
        /// </summary>
        protected bool isCheckPopedom = false; 

        /// <summary>
        /// ҽ����Ϣ����ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.IAlterOrder IAlterOrderInstance = null;

        //{6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
        protected Neusoft.ApplyInterface.HisInterface PACSApplyInterface = null;

        /// <summary>
        /// {F38618E9-7421-423d-80A9-401AFED0B855} xuc
        /// ���ˢ����ʾ����ҽ����Ϣ��־
        /// </summary>
        private bool isShowOrderFinished = true;
        #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

        Employee empl = FrameWork.Management.Connection.Operator as Employee;
        IReasonableMedicine IReasonableMedicine = null;

        #endregion

        /// <summary>
        /// Сʱ�Ʒѵ�ҽ��Ƶ�δ��� {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
        /// </summary>
        private string hoursFrequencyID = string.Empty;

        /// <summary>
        /// �Ƿ����õ������뵥 
        /// </summary>
        private bool isUsePACSApplySheet = false;

        #region ���İ�BUG addby xuewj 2009-09-04 {40F651AC-C372-4ca1-AFB2-F5F8B95D1E6D}

        private Hashtable htSubs = new Hashtable();

        #endregion

        #endregion

        #region {49026086-DCA3-4af4-A064-58F7479C324A}
        public event RefreshGroupTree refreshGroup;
        #endregion

        #region ��ʼ��

        /// <summary>
        /// ����Loading
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) return;
            if (Neusoft.FrameWork.Management.Connection.Operator.ID == "") return;

            this.myReciptDoc = null;
            this.myReciptDept = null;
            try
            {
                #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
                this.ucItemSelect1.IsNurseCreate = this.isNurseCreate;
                #endregion
                this.ucItemSelect1.Init();
               
                InitControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            try
            {
                #region ��ȡ���Ʋ���  ��������ʾ��ֹͣҽ��ʱ����(��λΪ��) Ĭ����ʾ1000�����ڵ�
                Neusoft.FrameWork.Management.ControlParam controler = new Neusoft.FrameWork.Management.ControlParam();
                try
                {
                    this.iDCControl = System.Convert.ToInt32(controler.QueryControlerInfo("200013"));
                }
                catch
                {
                    this.iDCControl = 1000;
                }
                #endregion

               
                #region {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamManager = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
                this.hoursFrequencyID = controlParamManager.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.MetConstant.Hours_Frequency_ID, true, "NONE");
                #endregion

                #region ҽ��Ȩ����֤����//{BFDA551D-7569-47dd-85C4-1CA21FE494BD}
                //Neusoft.FrameWork.Public.ObjectHelper controlerHelper = new Neusoft.FrameWork.Public.ObjectHelper();
                //Neusoft.FrameWork.Models.NeuObject tempControler = controlerHelper.GetObjectFromID("200039");
                //if (tempControler != null)
                //{
                //    this.isCheckPopedom = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                //}
                this.isCheckPopedom = controlParamManager.GetControlParam<bool>("200039");
                #endregion

                #region {3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
                this.enabledPacs = controlParamManager.GetControlParam<bool>("200202");
                #endregion

                #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504 
                //this.isUsePACSApplySheet = controlParamManager.GetControlParam<bool>("200212");
                this.isUsePACSApplySheet = Neusoft.HISFC.Components.Common.Classes.Function.LoadMenuSet();//addby xuewj 2010-11-11 �������뵥��ȡ���������ļ� {457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
                #endregion

                try
                {
                    //������п���
                    Neusoft.HISFC.BizProcess.Integrate.Manager deptManagement = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                    alDepts = deptManagement.GetDepartment();
                    //�������Ƶ����Ϣ �����������ҩϵͳ����ҽ��Ƶ��               
                    ArrayList alFrequency = deptManagement.QuereyFrequencyList();
                    if (alFrequency != null)
                        helper = new Neusoft.FrameWork.Public.ObjectHelper(alFrequency);
                }
                catch { }
                
                this.ucItemSelect1.OrderChanged += new ItemSelectedDelegate(ucItemSelect1_OrderChanged);
                this.ucItemSelect1.CatagoryChanged += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucInputItem1_CatagoryChanged);

                this.fpSpread1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
                this.fpSpread1.Sheets[0].DataAutoSizeColumns = false;
                this.fpSpread1.Sheets[1].DataAutoSizeColumns = false;
                this.fpSpread1.Sheets[0].DataAutoCellTypes = false;
                this.fpSpread1.Sheets[1].DataAutoCellTypes = false;

                this.fpSpread1.Sheets[0].GrayAreaBackColor = Color.White;
                this.fpSpread1.Sheets[1].GrayAreaBackColor = Color.White;

                this.fpSpread1.Sheets[0].RowHeader.Columns.Get(0).Width = 15;
                this.fpSpread1.Sheets[1].RowHeader.Columns.Get(0).Width = 15;

                this.fpSpread1.Sheets[0].RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
                this.fpSpread1.Sheets[1].RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;

                this.OrderType = Neusoft.HISFC.Models.Order.EnumType.LONG;
                this.fpSpread1.ActiveSheetIndex = 0;

                this.fpSpread1.Sheets[0].RowHeader.DefaultStyle.Border = new FarPoint.Win.BevelBorder(FarPoint.Win.BevelBorderType.Raised);
                this.fpSpread1.Sheets[0].RowHeader.DefaultStyle.CellType = new FarPoint.Win.Spread.CellType.TextCellType();

                this.fpSpread1.Sheets[1].RowHeader.DefaultStyle.Border = new FarPoint.Win.BevelBorder(FarPoint.Win.BevelBorderType.Raised);
                this.fpSpread1.Sheets[1].RowHeader.DefaultStyle.CellType = new FarPoint.Win.Spread.CellType.TextCellType();

                //
                //��ʼ��PACS{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
                if (this.enabledPacs)
                {
                    this.InitPacsInterface();
                }
            }
            catch { }

            #region addby xuewj 2010-10-5 ����StatusBarPanel {C0E71DA8-F246-4ff2-98CB-7EC72A767453}
            //base.OnStatusBarInfo(null, "(��ɫ���¿�)(��ɫ�����)(��ɫ��ִ��)(��ɫ������)");
            base.InsertStastusBarPanel(Properties.Resources.ҽ����ҽ��״̬, "", 1); 
            #endregion
            #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ
            InitPass();
            #endregion

            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            this.plPatient.Height = 36;
            #endregion
            //base.OnLoad(e);
        }
        private void InitPass()
        {
            #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

            this.InitReasonableMedicine();

            if (this.IReasonableMedicine == null)
            {
                return;
            }

            int iReturn = 0;
            iReturn = this.IReasonableMedicine.PassInit(empl.ID, empl.Name, empl.Dept.ID, empl.Dept.Name, 10, true);
            if (iReturn == -1)
            {
                this.EnabledPass = false;
                MessageBox.Show(IReasonableMedicine.Err);
            }
            if (iReturn == 0)
            {
                this.EnabledPass = false;
                //MessageBox.Show("������ҩ������δ����,���ܽ�����ҩ���,�����µ�½����վ��");
            }

            #endregion
        }
        #region ��ʼ��pacs{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}

        protected bool isInitPacs = false;
        protected bool enabledPacs = false;
        protected Neusoft.HISFC.BizProcess.Interface.Common.IPacs pacsInterface = null;

        /// <summary>
        /// 
        /// </summary>
        protected void InitPacsInterface()
        {
            if (this.enabledPacs==true && this.isInitPacs==false)
            {
                this.pacsInterface = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Common.IPacs)) as Neusoft.HISFC.BizProcess.Interface.Common.IPacs;
                if (this.pacsInterface == null)
                {
                    MessageBox.Show("��ýӿ�IPacs����\n������û��ά����صĿؼ���ؼ�û��ʵ�ֽӿ�Pacs�ӿ�IPacs\n����ϵͳ����Ա��ϵ��");
                    return;
                }
                if (this.pacsInterface.Connect() == 0)
                {
                    this.isInitPacs = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public  void RelePacsInterface()
        {
            if (this.pacsInterface == null)
            {
                 return;
            }
            if (this.isInitPacs==false)
            {
                return;
            }
            if (this.enabledPacs == false)
            {
                return;
            }
            this.pacsInterface.Disconnect ();
        }
        #endregion

        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        private void InitControl()
        {
           
            this.myOrderClass.fpSpread1 = this.fpSpread1;
          
           

            #region ��ʼ��ucItemSelect
            this.ucItemSelect1.LongOrShort = 0;//����Ϊ����ҽ��
            this.ucItemSelect1.OperatorType = Operator.Add;//���ģʽ
            #endregion

            #region ��ʼ����������DataSet ����FarPoint����Դ sheet0 ==���� sheet1 ==��ʱ
            dsAllLong = this.InitDataSet();
            dsAllShort = this.InitDataSet();
            this.myOrderClass.dsAllLong = dsAllLong;

            this.fpSpread1.Sheets[0].DataSource = dsAllLong.Tables[0];
            this.fpSpread1.Sheets[1].DataSource = dsAllShort.Tables[0];
            #endregion

            this.myOrderClass.ColumnSet();
            SetFP();
            InitFP();

          
            
            #region FarPoint �¼�
            this.fpSpread1.MouseUp += new MouseEventHandler(fpSpread1_MouseUp);
            this.fpSpread1.Sheets[0].Columns[-1].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSpread1.Sheets[1].Columns[-1].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSpread1.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(fpSpread1_SelectionChanged);
            this.fpSpread1.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(fpSpread1_SheetTabClick);
            this.fpSpread1.Sheets[0].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(fpSpread1_Sheet1_CellChanged);
            this.fpSpread1.Sheets[1].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(fpSpread1_Sheet1_CellChanged);           
            #endregion

            #region {AFD4A961-4687-4af6-8EFF-A42EDA3FD636}
            this.plPatient.Visible = false;
            #endregion
        }

        private void InitFP()
        {
            this.myOrderClass.SetColumnName(0);
            this.myOrderClass.SetColumnName(1);
            #region "�д�С"
            try
            {
                this.myOrderClass.SetColumnProperty();
            }
            catch { }
            #endregion


        }

        /// <summary>
        /// ��ʼ��DataSet
        /// </summary>
        /// <returns></returns>
        private DataSet InitDataSet()
        {
            dataSet = new DataSet();
            myOrderClass.SetDataSet(ref dataSet);
            return dataSet;
        }

        #endregion

        #region IToolBar ��Ա

        /// <summary>
        /// �˳�ҽ������
        /// </summary>
        /// <returns></returns>
        public int ExitOrder()
        {
            this.IsDesignMode = false;
            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (this.fpSpread1.ActiveSheetIndex == 1)
            {
                this.plPatient.Height = 36;
            }
            #endregion
            return 0;
        }
        /// <summary>
        /// ���ҽ��
        /// </summary>
        /// <returns></returns>
        public int ComboNo()
        {
         
            return 0;
        }
        /// <summary>
        /// ɾ��ҽ��
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            // TODO:  ��� ucOrder.Del ʵ��

            return Delete(this.fpSpread1.ActiveSheet.ActiveRowIndex, true);
        }

        /// <summary>
        /// {D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// </summary>
        /// <param name="rowIndex">ɾ��������</param>
        /// <param name="isDirectDel">�Ƿ�ֱ��ɾ��������ʾ��</param>
        /// <returns></returns>
        private int Delete(int rowIndex, bool isDirectDel)
        {
            int i = rowIndex;
            DialogResult r;
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null, temp = null;
            if (i < 0 || this.fpSpread1.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("����ѡ��һ��ҽ����");
                return 0;
            }
            order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[i].Tag;
            #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
            if (this.isNurseCreate)
            {
                if (order.ReciptDoctor.ID != this.OrderManagement.Operator.ID)
                {
                    MessageBox.Show("��ʿ������ɾ�����˿�����ҽ��!");
                    return -1;
                }
            }
            #endregion           
            if (order.Status == 0 || order.Status == 5)
            {
                //�¼�
                #region δ���ҽ��
                //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                r = DialogResult.OK;
                if (!isDirectDel)
                {
                    r = MessageBox.Show("�Ƿ�ɾ����ҽ��[" + order.Item.Name + "]?\n *�˲������ܳ�����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (r == DialogResult.OK)
                {
                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                    //t.BeginTransaction();
                    pacsBillManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    patient.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    int count = this.fpSpread1.ActiveSheet.RowCount;
                    for (int row = count - 1; row >= 0; row--)
                    {
                        temp = this.fpSpread1.ActiveSheet.Rows[row].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
                        if (temp.Combo.ID == order.Combo.ID)
                        {
                            if (order.ID == "")
                            {
                                //��Ȼɾ��
                                this.fpSpread1.ActiveSheet.Rows.Remove(row, 1);
                            }
                            else
                            {
                                //delete from table
                                //ɾ���Ѿ��еĸ���
                                if (OrderManagement.DeleteOrderSubtbl(temp.Combo.ID) == -1)
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ɾ��������Ŀ��Ϣ����") + OrderManagement.Err);
                                    return -1;
                                }
                                int parm = OrderManagement.DeleteOrder(temp);
                                if (parm == -1)
                                {          
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                    MessageBox.Show(OrderManagement.Err);
                                    return -1;
                                }
                                else
                                {
                                    if (parm == 0)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ҽ��״̬�ѷ����仯 ��ˢ������"));
                                        return -1;
                                    }
                                }
                                if (patient.SelectBQ_Info(((Neusoft.FrameWork.Models.NeuObject)(myPatientInfo)).ID) == "1")
                                {
                                    if (order.Item.SysClass.ID.ToString() == "UF" && order.Item.Name.IndexOf("�Ѿ�����") != -1)
                                    {
                                        if (patient.UpdatePT_Info(((Neusoft.FrameWork.Models.NeuObject)(myPatientInfo)).ID) == -1)
                                        {
                                            Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                            MessageBox.Show(OrderManagement.Err);
                                            return -1;
                                        }
                                    }
                                }
                                else
                                { 
                                }
                                //ɾ������
                                parm = OrderManagement.DeleteOrderSubtbl(temp.Combo.ID);
                                if (parm == -1)
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                    MessageBox.Show(OrderManagement.Err);
                                    return -1;
                                }

                                this.fpSpread1.ActiveSheet.Rows.Remove(row, 1);
                            }
                        }
                    }

                    if (this.pacsBillManagement.DeletePacsBill(order.Combo.ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                        MessageBox.Show(OrderManagement.Err);
                        return -1;
                    }                                        
                    Neusoft.FrameWork.Management.PublicTrans.Commit();

                    //ɾ��һ�к�ѡ����һ�� 
                    if (this.fpSpread1.ActiveSheet.Rows.Count > 0)
                        this.SelectionChanged();
                }
                #endregion
            }
            else if (order.Status != 3)
            {
                string strTip = "";
                //������˹���ִ�й��Ķ�����ֹͣ
                if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    strTip = "ֹͣ";
                }
                else
                {
                    //����ֻ����˹��ſ�������
                    //��������ִ�еĿ�������ҽ�� Edit By liangjz 2005-10
                    //					if(order.Status==2)return 0;
                    //.
                    //{BC016F2C-7292-44d6-AF58-92D5936A8554} ��������
                    //if (order.Status == 2)
                    //{
                    //    MessageBox.Show("�Ѿ�ִ�е�ҽ�������������ϣ�", "��ʾ");
                    //    return -1;
                    //}
                    strTip = "����";
                }

                //����ֹͣ����
                Forms.frmDCOrder f = new Forms.frmDCOrder();
                f.ShowDialog();
                if (f.DialogResult != DialogResult.OK) return 0;

                order.DCOper.OperTime = f.DCDateTime;
                order.DcReason = f.DCReason.Clone();
                order.DCOper.ID = OrderManagement.Operator.ID;
                order.DCOper.Name = OrderManagement.Operator.Name;
                order.EndTime = order.DCOper.OperTime;
                #region {03E0384D-540A-4e5d-B3CA-54E931FFA3EF}
                if (order.EndTime < order.BeginTime)
                {
                    MessageBox.Show("ֹͣʱ�䲻��С�ڿ�ʼʱ��");
                    return -1;
                }
                #endregion
                //{46EF45CD-BC8D-494a-89A4-B2386195EC00}
                //if (f.DCDateTime.Date > OrderManagement.GetDateTimeFromSysDateTime().Date)
                if (f.DCDateTime > OrderManagement.GetDateTimeFromSysDateTime().AddHours(1))
                {
                    //Ԥֹͣʱ��ָ��
                    #region ����Ԥֹͣ
                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.OrderManagement.Connection);
                    //t.BeginTransaction();
                    this.OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    for (int row = 0; row < this.fpSpread1.ActiveSheet.RowCount; row++)
                    {
                        temp = this.fpSpread1.ActiveSheet.Rows[row].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
                        if (temp.Combo.ID == order.Combo.ID)
                        {
                            #region {D1A8C8BD-483D-4d10-B056-D7E4FD3F798E}
                            //ԭ�������ڱ���Ԥֹͣʱû�ж�ͬһ��ϵ�����ҽ�����и��£��ּ���˶δ���
                            ArrayList alTemp = new ArrayList();
                            alTemp = OrderManagement.QueryOrderByCombNO(temp.Combo.ID, false);
                            if (alTemp != null && alTemp.Count > 1)
                            {
                                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order orderTemp in alTemp)
                                {
                                    if (orderTemp.ID == temp.ID) continue;
                                    orderTemp.DCOper = temp.DCOper;
                                    orderTemp.DcReason = temp.DcReason;
                                    orderTemp.EndTime = temp.EndTime;

                                    if (OrderManagement.UpdateOrder(orderTemp) == -1)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                        MessageBox.Show(OrderManagement.Err);
                                        return -1;
                                    }

                                }
                            }
                            #endregion
                            if (OrderManagement.UpdateOrder(temp) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                MessageBox.Show(OrderManagement.Err);
                                return -1;
                            }

                            #region addby xuewj Ԥֹͣ���½��� {92F28465-7AA8-482a-A233-62A518BCD9B0}
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[3]].Value = temp.Status;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[25]].Value = temp.DCOper.OperTime;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[26]].Text = temp.DCOper.ID;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[27]].Text = temp.DCOper.Name;
                            this.fpSpread1.ActiveSheet.Rows[row].Tag = temp;
                            #endregion
                        }
                    }
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    #endregion
                }
                else
                {
                    //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                    Neusoft.HISFC.BizProcess.Integrate.Order orderIntergrate = new Neusoft.HISFC.BizProcess.Integrate.Order();

                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.OrderManagement.Connection);
                    //t.BeginTransaction();
                    this.OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                    orderIntergrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    ArrayList alTemp = new ArrayList();
                    for (int row = 0; row < this.fpSpread1.ActiveSheet.RowCount; row++)
                    {

                        temp = this.fpSpread1.ActiveSheet.Rows[row].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
                        if (temp.Combo.ID == order.Combo.ID)
                        {
                            #region {D028C0B7-014F-4c60-883D-B49A0BD3399A}
                            temp.DcReason = order.DcReason;
                            temp.DCOper = order.DCOper;
                            #endregion
                            #region Сʱҽ��ֹͣ�Ʒ� {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                            if (this.DCHoursOrder(order, orderIntergrate, Neusoft.FrameWork.Management.PublicTrans.Trans) < 0)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                orderIntergrate.fee.Rollback();
                                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(order.Item.Name + "ֹͣʱ�Ƿ�ʧ�ܣ�"));
                                return -1;
                            }
                            #endregion
                            temp.Status = 3;
                            #region ֹͣҽ��

                            #region ����ת��ҽ��
                            //if (order.Item.SysClass.ID.ToString() == "MRD")
                            //{
                            //    Neusoft.HISFC.Models.RADT.Location newLocation = new Neusoft.HISFC.Models.RADT.Location();
                            //    //���¿�����Ϣ
                            //    newLocation.Dept.ID = order.ExeDept.ID;
                            //    newLocation.Dept.Name = order.ExeDept.Name;
                            //    newLocation.Dept.Memo = order.Note;
                            //    Neusoft.HISFC.BizLogic.RADT.InPatient Inpatient = new Neusoft.HISFC.BizLogic.RADT.InPatient();
                            //    Inpatient.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                            //    //ת������/ȡ��
                            //    try
                            //    {
                            //        int parm;
                            //        parm = Inpatient.TransferPatientApply(this.PatientInfo, newLocation, true);
                            //        if (parm == -1)
                            //        {
                            //            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            //            MessageBox.Show(Inpatient.Err, "��ʾ");
                            //            return -1;
                            //        }
                            //        else if (parm == 0)
                            //        {
                            //            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            //            MessageBox.Show("��ת�������ѱ�ȷ��,����ȡ��", "��ʾ");
                            //            return -1;
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            //        MessageBox.Show(ex.Message, "��ʾ");
                            //        return -1;
                            //    }
                            //}
                            #endregion

                            string strReturn = "";
                            if (OrderManagement.DcOrder(temp, true, out strReturn) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                                orderIntergrate.fee.Rollback();
                                MessageBox.Show(OrderManagement.Err);
                                return -1;
                            }

                            //Add By liangjz 20005-08
                            if (strReturn != "")
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                                orderIntergrate.fee.Rollback();
                                MessageBox.Show(strReturn);
                                return -1;
                            }
                            #endregion
                            //������Ϣ����ʿ
                            //Neusoft.Common.Class.Message.SendMessage(this.GetPatient().Patient.Name + "��ҽ����" + temp.Item.Name + "���Ѿ�" + strTip, order.NurseStation.ID);
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[3]].Value = temp.Status;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[25]].Value = temp.DCOper.OperTime;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[26]].Text = temp.DCOper.ID;
                            this.fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[27]].Text = temp.DCOper.Name;
                            this.fpSpread1.ActiveSheet.Rows[row].Tag = temp;
                            continue;
                        }
                    }

                    //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                    orderIntergrate.fee.Commit();
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                }
                this.RefreshOrderState();
            }
            else
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������ҽ������ɾ��,���ϻ�ȡ��!"));
            }

            #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
            if (this.isUsePACSApplySheet)
            {
                if (order.Status != 3 && (order.ApplyNo != null && order.ApplyNo.ToString() != ""))
                {
                    if (PACSApplyInterface == null)
                    {
                        if (InitPACSApplyInterface() < 0)
                        {
                            //MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                            //return -1;
                        }
                    }
                    if (PACSApplyInterface != null)
                    {
                        PACSApplyInterface.DeleteApply(order.ApplyNo);
                        #region {5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
                        order.ApplyNo = "";
                        #endregion
                        for (int row = 0; row < this.fpSpread1.ActiveSheet.RowCount; row++)
                        {
                            temp = this.fpSpread1.ActiveSheet.Rows[row].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
                            if (temp.Combo.ID == order.Combo.ID && temp.ID != order.ID)
                            {
                                PACSApplyInterface.DeleteApply(temp.ApplyNo);
                                temp.ApplyNo = "";//{5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
                            }
                        }
                    }
                }
            }
            #endregion

            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (order.Status == 0 || order.Status == 5)
            {
                this.ShowTempCost();
            } 
            #endregion
            return 0;
        }
        
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        public int Add()
        {
            //{F38618E9-7421-423d-80A9-401AFED0B855}
            if (this.isShowOrderFinished == false)
            {
                //MessageBox.Show("ˢ����Ϣ��δ��ɣ����Ժ��ٵ��������");
                return -1;
            }
            
            CountLongBegin = this.fpSpread1_Sheet1.Rows.Count;
            CountShortBegin = this.fpSpread1_Sheet2.Rows.Count;
            // TODO:  ��� ucOrder.Add ʵ��
            if (this.myPatientInfo == null || this.myPatientInfo.ID == "")
            {
                return -1;
            }
            this.IsDesignMode = true;
            this.OrderType = this.myOrderType;
            #region {190B18B2-9CF0-4b44-BB93-63A15387AD0B}
            if (this.OrderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
            {
                if (this.OrderCanOperatorChanged != null) this.OrderCanOperatorChanged(false);
            }
            #endregion
            this.ucItemSelect1.Focus();
            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (this.fpSpread1.ActiveSheetIndex == 1)
            {
                this.plPatient.Height = 72;
                this.ShowTempCost();
            }
            #endregion
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int Exit(object sender, object neuObject)
        {
            // TODO:  ��� ucOrder.Exit ʵ��
            if (this.IsDesignMode)
            {

            }
            else
            {
                this.FindForm().Close();
            }
           
            return 0;
        }
       
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            if (this.bIsDesignMode == false) return -1;
            if (this.CheckOrder() == -1) return -1;

            #region  {5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
            List<string> itemList = new List<string>();
            #endregion

            //////������Ժ�ж�
            ////if (Function.JudgePatient(InPatient, this.PatientInfo) == -1)
            ////{
            ////    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
            ////    return -1;
            ////}

            //���´��������������޷��ѳ�ʼ�������ڲ�
            if (this.IAlterOrderInstance == null)
            {
                this.InitAlterOrderInstance();
            }

            #region ��������
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            #endregion

            #region ���ĵ�ҽ��
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> alOrder = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();//���泤������
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            string checkMsg = "";
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();

            for (int i = 0; i < this.fpSpread1.Sheets[0].Rows.Count; i++)
            {
                order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.Sheets[0].Rows[i].Tag;
              
                //if (order.Status == 0)
                //{
                //    alOrder.Add(order);                    
                //}

                if (order.Status == 0 || order.Status == 5)
                {
                    #region  {5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
                    if (string.IsNullOrEmpty(order.ApplyNo))
                    {
                        itemList.Add(order.Item.ID);
                    }
                    #endregion
                    string failCause = "";
                    int rtn = Neusoft.HISFC.BizProcess.Integrate.Medical.Ability.CheckPopedom(this.empl.ID, order.Item.ID, order.Item.SysClass.ID.ToString(), true, ref failCause);
                    if (rtn == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(failCause);
                        return -1;
                    }

                    if (order.Status == 0)
                    {                        
                        if (rtn == 0)
                        {
                            order.Status = 5;
                            checkMsg += order.Item.Name + "��Ҫ�ϼ�ҽ����˲ſ�ִ�У�";
                        }
                    }
                    else if (order.Status == 5)
                    {
                        if (rtn == 1)
                        {
                            order.Status = 0;
                            order.ReciptDoctor = Neusoft.FrameWork.Management.Connection.Operator.Clone();
                        }
                        else if(rtn==0)
                        {
                            checkMsg += order.Item.Name + "��Ҫ�ϼ�ҽ����˲ſ�ִ�У�";
                        }
                    }
                    alOrder.Add(order);
                }

                orderList.Add(order);
            }            

            for (int i = 0; i < this.fpSpread1.Sheets[1].Rows.Count; i++)
            {
                order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.Sheets[1].Rows[i].Tag;
                //if (order.Item.IsPharmacy)
                if (order.Item.ItemType == EnumItemType.Drug)
                {
                    if (order.OrderType.ID == "0")
                    {
                        if ((order.Item as Neusoft.HISFC.Models.Pharmacy.Item).BaseDose == 0)
                        {
                            MessageBox.Show(order.Item.Name + "��������Ϊ�㣬û��ά����������", "��ʾ");
                        }
                        else if ((order.DoseOnce / (order.Item as Neusoft.HISFC.Models.Pharmacy.Item).BaseDose) > order.Qty)
                        {
                            //this.fpSpread1_Sheet2.SetRowLabel(i,0,"E");
                            MessageBox.Show(order.Item.Name + "����ҽ��ÿ�������� ÿ����/��������> ����", "��ʾ");
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            return -1;
                        }
                    }
                }
                //if (order.Status == 0)
                //{
                //    alOrder.Add(order);
                //}

                if (order.Status == 0 || order.Status == 5)
                {
                    #region  {5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
                    if (string.IsNullOrEmpty(order.ApplyNo))
                    {
                        itemList.Add(order.Item.ID);
                    }
                    #endregion
                    string failCause = "";
                    int rtn = Neusoft.HISFC.BizProcess.Integrate.Medical.Ability.CheckPopedom(this.empl.ID, order.Item.ID, order.Item.SysClass.ID.ToString(), true, ref failCause);
                    if (rtn == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(failCause);
                        return -1;
                    }

                    if (order.Status == 0)
                    {
                        if (rtn == 0)
                        {
                            order.Status = 5;
                            checkMsg += order.Item.Name + "��Ҫ�ϼ�ҽ����˲ſ�ִ�У�";
                        }
                    }
                    else if (order.Status == 5)
                    {
                        if (rtn == 1)
                        {
                            order.Status = 0;
                            order.ReciptDoctor = Neusoft.FrameWork.Management.Connection.Operator.Clone();
                        }
                        else if (rtn == 0)
                        {
                            checkMsg += order.Item.Name + "��Ҫ�ϼ�ҽ����˲ſ�ִ�У�";
                        }
                    }
                    alOrder.Add(order);
                }

                orderList.Add(order);
            }

            #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�          

            if (this.IAlterOrderInstance != null)
            {
                //{76FBAEE1-C996-41b4-9D77-F6CE457F6518} �����˽ӿ��ڷ���
                if (this.IAlterOrderInstance.AlterOrderOnSaving(this.myPatientInfo, this.myReciptDoc, this.myReciptDept, ref orderList) == -1)
                {
                    return -1;
                }
            }

            #endregion

            string err = "";//������Ϣ
            string strNameNotUpdate = "";//�Ѿ��仯״̬��ҽ��������

            #region ���İ�BUG addby xuewj 2009-09-04 {40F651AC-C372-4ca1-AFB2-F5F8B95D1E6D}

            foreach (string comboID in this.htSubs.Values)
            {
                if (OrderManagement.DeleteOrderSubtbl(comboID) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ɾ��������Ŀ��Ϣ����") + OrderManagement.Err);
                    return -1;
                }
            }

            htSubs.Clear();

            #endregion

            #region  {5D274E04-7B3D-449c-AB72-3DAAC9414D6C}
            if (this.isUsePACSApplySheet&&itemList.Count>0)
            {
                if (PACSApplyInterface == null)
                {
                    if (InitPACSApplyInterface() < 0)
                    {
                        //Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ʼ���������뵥�ӿ�ʱ����"));
                        //return -1;
                    }
                }
                if (PACSApplyInterface != null)
                {
                    string errText = "";
                    int rtn = PACSApplyInterface.JudgeItemCount(itemList, 3, ref errText);
                    if (rtn != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(errText));
                        return -1;
                    }
                }
            }
            #endregion

            if (Neusoft.HISFC.BizProcess.Integrate.Order.SaveOrder(alOrder, this.GetReciptDept().ID,  out err, out strNameNotUpdate,Neusoft.FrameWork.Management.PublicTrans.Trans) == -1)
            {  
                Neusoft.FrameWork.Management.PublicTrans.RollBack(); 
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ҽ������ʧ�ܣ�")+"\n"+err);
                return -1;
            }
            else
            {
                //Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                patient.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                for (int i = 0; i < alOrder.Count; i++)
                {                    
                    if (alOrder[i].Item.SysClass.ID.ToString() == "UF" && alOrder[i].Item.Name.IndexOf("�ǲ���") != -1)
                    {
                        if (i > 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if ((alOrder[j].Item.SysClass.ID.ToString() == "UF" && alOrder[j].Item.Name.IndexOf("�Ѿ�����") != -1)||(alOrder[i].Item.SysClass.ID.ToString() == "UF" && alOrder[i].Item.Name.IndexOf("�ǲ���") != -1))
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�����Ϸ�") + "\n" + err);
                                    return -1;
                                }
                            }
                        }
                                if (patient.UpdatePT_Info(((Neusoft.FrameWork.Models.NeuObject)(myPatientInfo)).ID) != -1)
                                {                                    
                                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                                }
                                else
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                    return -1;
                                }
                    }
                    else
                    {
                        if (alOrder[i].Item.SysClass.ID.ToString() == "UF" && alOrder[i].Item.Name.IndexOf("�Ѿ�����") != -1)
                        {
                            if (i > 0)
                            {
                                for (int j = 0; j < i; j++)
                                {
                                    if ((alOrder[j].Item.SysClass.ID.ToString() == "UF" && alOrder[j].Item.Name.IndexOf("�Ѿ�����") != -1) || (alOrder[i].Item.SysClass.ID.ToString() == "UF" && alOrder[i].Item.Name.IndexOf("�ǲ���") != -1))
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�����Ϸ�") + "\n" + err);
                                        return -1;
                                    }
                                }
                            }
                            if (patient.UpdateBZ_Info(((Neusoft.FrameWork.Models.NeuObject)(myPatientInfo)).ID) != -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.Commit();
                            }
                            else
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                return -1;
                            }
                        }
                    }
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                if (strNameNotUpdate == "")
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ҽ������ɹ���"+checkMsg));
                }
                else
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ������ʧ�ܣ�")+"\n" + strNameNotUpdate 
                        + Neusoft.FrameWork.Management.Language.Msg("ҽ��״̬�Ѿ��������ط����ģ��޷����и��£���ˢ����Ļ��"));
                }
            }
            #endregion

            #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�

            if (this.IAlterOrderInstance != null)
            {
                //{76FBAEE1-C996-41b4-9D77-F6CE457F6518} �����˽ӿ��ڷ���
                if (this.IAlterOrderInstance.AlterOrderOnSaved(this.myPatientInfo, this.myReciptDoc, this.myReciptDept, ref orderList) == -1)
                {
                    return -1;
                }
            }

            #endregion

            #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
            if (this.isUsePACSApplySheet)
            {
                if (PACSApplyInterface == null)
                {
                    if (InitPACSApplyInterface() < 0)
                    {
                        //MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                        //return -1;
                    }
                }
                //PACSApplyInterface.init(Neusoft.FrameWork.Management.Connection.Operator.ID, ((Neusoft.HISFC.Models.Base.Employee)(Neusoft.FrameWork.Management.Connection.Operator)).Dept.ID, Application.StartupPath + "\\lib");
                if (PACSApplyInterface != null)
                {
                    PACSApplyInterface.SaveApplysG(this.myPatientInfo.ID, 1);
                }
            }
            #endregion

            #region ��ʱ��Ϣ
            //{7882B4CC-FA22-4530-9E5E-2E738DF1DEEC}
            this.OnSendMessage(null, "");

            #endregion

            this.IsDesignMode = false;
            this.isEdit = false;
            #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ�Զ����

            if (this.IReasonableMedicine != null && this.EnabledPass)
            {
                string err1 = "";
                #region {8C389FCD-3E64-4a90-9830-BE220B952B53} 2010-12-08 �޸�
                //ArrayList al = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("AutoPass", out err1);
                ArrayList al = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("Pass", "AutoPass", out err1);
                #endregion
                if (al == null || al.Count == 0)
                {
                    //MessageBox.Show(err1);
                    //return -1;
                }
                else if (al[0] as string == "1")
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    #region {8C389FCD-3E64-4a90-9830-BE220B952B53} 2010-12-08 �޸�
                    //���߻���סԺ��Ϣ�ϴ�
                    this.IReasonableMedicine.PassSetPatientInfo(this.myPatientInfo, this.empl.ID, this.empl.Name);
                    //������ҩ���
                    //this.PassTransOrder(1, false);
                    this.PassTransOrder(1, true);
                    #endregion
                }
                else
                {
                    //return -1;
                }
            }
            #endregion
            //if (alOrder != null && alOrder.Count > 0 && this.EnabledPass)
            //{
            //    this.PassSaveCheck(alOrder, 1, true);
            //}

            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (this.fpSpread1.ActiveSheetIndex == 1)
            {
                this.plPatient.Height = 36;
            }
            #endregion
            return 0;
        }

        public int JudgeSpecialOrder()
        {
            int i = this.fpSpread1.ActiveSheet.ActiveRowIndex;
            if (i < 0 || this.fpSpread1.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("����ѡ��һ��ҽ����");
                return -1;
            }
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[i].Tag;
            if (order.Status == 5)
            {
                Neusoft.HISFC.BizProcess.Integrate.Manager personManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                Neusoft.HISFC.Models.Base.Employee doct = new Neusoft.HISFC.Models.Base.Employee();
                doct = personManager.GetEmployeeInfo(Neusoft.FrameWork.Management.Connection.Operator.ID);
                Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlManager = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
                string strLevel = controlManager.GetControlParam<string>("200034", true, "2");
                if (doct.Level.ID == strLevel)
                {
                    order.Status = 0;
                    this.fpSpread1.ActiveSheet.Rows[i].Tag = order;
                    this.fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[3]].Text = order.Status.ToString();
                }
 
            }
            return 0;
        }

        public int HerbalOrder()
        {
            string orderTypeFlag = "1";		//0 ���� 1 ����

            Neusoft.HISFC.Models.Order.Inpatient.Order ord;
            if (this.fpSpread1.ActiveSheet.ActiveRowIndex >= 0 && this.fpSpread1.ActiveSheet.Rows.Count > 0)
            {
                ord = this.fpSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
                #region �����ҩ����{7985420C-9CF9-4dd3-BED4-A5CC0EC9D52C}
                //if (ord != null && ord.Status != null && ord.Status == 0)
                if (ord != null && ord.Item.SysClass.ID.ToString() == "PCC" && ord.Status == 0)
                {//{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                    this.ModifyHerbal();
                    #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
                    this.ShowTempCost();
                    #endregion
                    return 1;
                }
                #endregion
                #region {7985420C-9CF9-4dd3-BED4-A5CC0EC9D52C}
                else
                {
                    using (ucHerbalOrder uc = new ucHerbalOrder(false, this.OrderType, this.GetReciptDept().ID))
                    {
                        uc.Patient = this.myPatientInfo;
                        #region {49026086-DCA3-4af4-A064-58F7479C324A}
                        uc.refreshGroup += new RefreshGroupTree(uc_refreshGroup);
                        #endregion
                        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                        Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                        if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                        {
                            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in uc.AlOrder)
                            {
                                this.AddNewOrder(info, this.OrderType == Neusoft.HISFC.Models.Order.EnumType.LONG ? 0 : 1);
                            }
                            uc.Clear();
                            this.RefreshCombo();
                        }
                    }
                }
                #endregion
            }
            else
            {
                using (ucHerbalOrder uc = new ucHerbalOrder(false, this.OrderType, this.GetReciptDept().ID))
                {
                    uc.Patient = this.myPatientInfo;
                    #region {49026086-DCA3-4af4-A064-58F7479C324A}
                    uc.refreshGroup += new RefreshGroupTree(uc_refreshGroup);
                    #endregion
                    Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                    if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                    {
                        foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in uc.AlOrder)
                        {
                            info.DoseOnce = info.Qty;//{DC0E8BDB-D918-4c14-8474-3D2E6F986A57}

                            this.AddNewOrder(info, this.OrderType == Neusoft.HISFC.Models.Order.EnumType.LONG ? 0 : 1);
                        }
                        uc.Clear();
                        this.RefreshCombo();
                    }
                }
            }

            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            this.ShowTempCost(); 
            #endregion
            return 1;
        }

        /// <summary>
        /// ѡ��ҽ��{D5517722-7128-4d0c-BBC4-1A5558A39A03}���ڵ�½��Ա����ҽ��ʱʹ��
        /// </summary>
        /// <returns></returns>
        public int ChooseDoctor()
        {
            try
            {
                ucChooseDoct uc = new ucChooseDoct();
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "ѡ��";
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                if (uc.ChooseDoct.ID != null && uc.ChooseDoct.ID != "")
                {
                    this.SetReciptDoc(uc.ChooseDoct);
                }
            }
            catch 
            {
                return -1;
            }
            return 1;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            //if (keyData == Keys.F11)
            //{
            //    this.HerbalOrder();
            //}
            return base.ProcessDialogKey(keyData);
        }

        #endregion

        #region ��������

        /// <summary>
        /// �����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        public void AddNewOrder(object sender, int SheetIndex)
        {
            //{47A93CAA-AFF1-4c43-B21E-4C9EC1EF937A}
            #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�
            Neusoft.HISFC.Models.Order.Inpatient.Order orders;
            orders = sender as Neusoft.HISFC.Models.Order.Inpatient.Order;

            if (!this.EditGroup)
            {
                if (this.IAlterOrderInstance == null)
                {
                    this.InitAlterOrderInstance();
                }

                if (this.IAlterOrderInstance != null)
                {
                    if (this.IAlterOrderInstance.AlterOrder(this.myPatientInfo, this.myReciptDoc, this.myReciptDept, ref orders) == -1)
                    {
                        return;
                    }
                }
            }
            #endregion

            int CountLong = this.fpSpread1_Sheet1.Rows.Count;

            int CountShort = this.fpSpread1_Sheet2.Rows.Count;
            dirty = true;

            //�������
            if (sender.GetType() == typeof(Neusoft.HISFC.Models.Order.Inpatient.Order))
            {

                #region ��黥��
                if (CheckMutex(((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.SysClass) == -1)
                    return;
                //}
                #endregion

                #region �����ӵĶ���
                if (((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.SysClass.ID.ToString() == "UC")
                {
                    //���ÿ��ԶԸ���ҽ���ļ�鵥��д
                    //this.IsPrintTest(true);
                }
                else if (((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.SysClass.ID.ToString() == "MC")
                {
                    //����
                    //��ӻ�������
                    this.AddConsultation(sender);
                }
                //if (((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.IsPharmacy)
                if (((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.ItemType == EnumItemType.Drug)
                {
                    //ҩƷ
                    if (((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item).IsAllergy)
                    {
                        if (MessageBox.Show("�Ƿ���ҪƤ�ԣ�", "��ʾ" + ((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.Name, MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            ((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item).IsAllergy = false;
                            ((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).HypoTest = 4;
                            ((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item.Name += "�ۡ���";
                        }
                        else
                        {
                            ((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Memo += Classes.Function.TipHypotest;
                            //��ҪƤ�� 
                            ((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).HypoTest = 2;
                        }
                    }
                    //�ж�ҩƷ�Ƿ���ҩ������ʾ
                    try
                    {
                        if (((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Item).Quality.ID.Substring(0, 1) == "S")
                        {
                            MessageBox.Show("��ͬʱ���ӿ����ֹ�����ҩ����!");
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    
                }
                #endregion

                Neusoft.HISFC.Models.Order.Inpatient.Order order = sender as Neusoft.HISFC.Models.Order.Inpatient.Order;
                if (this.GetReciptDept() != null)
                    order.ReciptDept = this.GetReciptDept().Clone();
                if (this.GetReciptDoc() != null)
                    order.ReciptDoctor = this.GetReciptDoc().Clone();

                if (order.Combo.ID == "")
                {
                    try
                    {
                        order.Combo.ID = this.OrderManagement.GetNewOrderComboID();//�����Ϻ�
                    }
                    catch
                    {
                        MessageBox.Show("���ҽ����Ϻų���");
                    }
                }

                DateTime dtNow = new DateTime();                
                try
                {
                    dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
                    dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, dtNow.Hour, dtNow.Minute, 0);//{8FEB04B3-0A07-4893-A5B8-829D8ADC468B}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //����ҽ������ʱ��
                if (this.needUpdateDTBegin)
                {
                    if (Classes.Function.IsDefaultMoDate == false)
                    {
                        if (dtNow.Hour >= 12)
                            order.BeginTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 12, 0, 0);
                        else
                            order.BeginTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);

                        if (Classes.Function.MoDateDays > 0)
                        {
                            order.BeginTime = new DateTime(dtNow.AddDays(Classes.Function.MoDateDays).Year, dtNow.AddDays(Classes.Function.MoDateDays).Month, dtNow.AddDays(Classes.Function.MoDateDays).Day, 0, 0, 2);
                        }
                    }
                    else
                    {
                        //��Ĭ��ʱ��
                        order.BeginTime = dtNow;
                    }
                }

                if (order.User03 != "")
                {
                    //���׵�ʱ����
                    int iDays = Neusoft.FrameWork.Function.NConvert.ToInt32(order.User03);
                    if (iDays > 0)
                    {
                        //��ʱ����>0
                        order.BeginTime = order.BeginTime.AddDays(iDays);
                    }
                }

                order.CurMOTime = DateTime.MinValue;
                order.NextMOTime = DateTime.MinValue;
                order.EndTime = DateTime.MinValue;

                this.currentOrder = order;

                this.fpSpread1.Sheets[SheetIndex].Rows.Add(0, 1);
                this.AddObjectToFarpoint(order, 0, SheetIndex,EnumOrderFieldList.Item);
                //���õ�ǰ�������Ϊ0
                this.ActiveRowIndex = 0;

                RefreshOrderState();
              
                //��Ӹ�����Ŀ��ϸ��ˢ����Ϻ� Add By liangjz 2005-08
                if (order.Package.ID != "")
                {
                
                    this.RefreshCombo();
                }
            }
            else
            {
                MessageBox.Show("������Ͳ���ҽ�����ͣ�");
            }
            dirty = false;
        }
        /// <summary> 
        /// �����������
        /// </summary>
        public void AddOperation()
        {
            //if (this.PatientInfo == null)
            //{
            //    MessageBox.Show("����ѡ���ߣ�");
            //    return;
            //}
            //frmApply dlgTempApply = new frmApply(Neusoft.Common.Class.Main.var, this.PatientInfo);
            //dlgTempApply.SetClearButtonFasle();
            //dlgTempApply.ISCloseNow = true;
            ////��ʾ��ʱ���봰��(ģʽ)
            //dlgTempApply.ShowDialog();

            ////����Ĵ���Ǳ���
            //if (dlgTempApply.ExeDept != "")
            //{
            //    //����"ȷ��"��ť
            //    Neusoft.FrameWork.Models.NeuObject mainOperation = new Neusoft.FrameWork.Models.NeuObject();//���������
            //    for (int i = 0; i < dlgTempApply.apply.OperateInfoAl.Count; i++)
            //    {
            //        Neusoft.HISFC.Models.Operator.OperateInfo obj = dlgTempApply.apply.OperateInfoAl[i] as Neusoft.HISFC.Models.Operator.OperateInfo;
            //        if (i == 0)
            //        {
            //            mainOperation.ID = obj.OperateItem.ID;
            //            mainOperation.Name = obj.OperateItem.Name;
            //        }
            //        if (obj.bMainFlag)
            //        {
            //            //��������
            //            mainOperation.ID = obj.OperateItem.ID;
            //            mainOperation.Name = obj.OperateItem.Name;
            //            break;
            //        }
            //    }
            //    Neusoft.HISFC.Models.Order.Inpatient.Order order = new Neusoft.HISFC.Models.Order.Inpatient.Order();
            //    Neusoft.HISFC.Models.Fee.Item item = new Neusoft.HISFC.Models.Fee.Item();
            //    Order.Inpatient.OrderType = (Neusoft.HISFC.Models.Order.Inpatient.OrderType)this.ucItemSelect1.SelectedOrderType.Clone();

            //    order.Item = item;
            //    order.Item.SysClass.ID = "UO";

            //    order.Item.ID = mainOperation.ID;
            //    order.Qty = 1;
            //    order.Unit = "��";
            //    order.Item.Name = mainOperation.Name;
            //    order.ExeDept.ID = dlgTempApply.ExeDept; /*ִ�п���*/
            //    order.Frequency.ID = "QD";
            //    //��������ҽ��Ĭ��Ϊ��ǰ����
            //    if (this.ucItemSelect1.alShort.Count > 0)
            //    {
            //        Neusoft.HISFC.Models.Order.Inpatient.OrderType info;
            //        for (int i = 0; i < this.ucItemSelect1.alShort.Count; i++)
            //        {
            //            info = this.ucItemSelect1.alShort[i] as Neusoft.HISFC.Models.Order.Inpatient.OrderType;
            //            if (info == null)
            //                return;
            //            if (info.ID == "SQ")
            //            {  //SQ ��ǰ���� SZ ��ǰ����
            //                Order.Inpatient.OrderType = info;
            //                break;
            //            }
            //        }
            //    }
            //    //this.ValidNewOrder(order);
            //    this.AddNewOrder(order, this.fpSpread1.ActiveSheetIndex);

            //}
        }
       
        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            this.ucItemSelect1.Clear();

            this.ucItemSelect1.ucInputItem1.Select();
            this.ucItemSelect1.ucInputItem1.Focus();
        }

        /// <summary>
        /// ��Ӽ�顢��������
        /// </summary>
        public void AddTest()
        {
            if (this.myPatientInfo == null)
            {
                MessageBox.Show("����ѡ���ߣ�");
                return;
            }
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> alItems = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();
            int iActiveSheet = 1;//��鵥Ĭ����ʱҽ��

            //{47C187AE-F3FC-433c-AA2D-F1C146ED4F92}  ��ѡ����ҽ��ʱ�Ž��м�����뵥����
            this.fpSpread1.ActiveSheetIndex = 1;
            this.OrderType = Neusoft.HISFC.Models.Order.EnumType.SHORT;//{A762E223-39EE-4379-AADB-B5A929F85D41}
            for (int i = 0; i < this.fpSpread1.Sheets[iActiveSheet].RowCount; i++)
            {
                if (this.fpSpread1.Sheets[iActiveSheet].IsSelected(i, 0))
                {
                    //{47C187AE-F3FC-433c-AA2D-F1C146ED4F92}  ��ѡ����ҽ��ʱ�Ž��м�����뵥����
                    Neusoft.HISFC.Models.Order.Inpatient.Order tempOrder = this.GetObjectFromFarPoint(i, iActiveSheet);
                    if (tempOrder.Item.SysClass.ID.ToString() == "UC")         //�����ڼ����Ŀ
                    {
                        //��alItems���ݸ�Ϊorder����
                        alItems.Add(tempOrder);
                    }
                }
            }
            if (alItems.Count <= 0)
            {
                //û��ѡ����Ŀ��Ϣ
                MessageBox.Show("��ѡ�����ļ����Ϣ!");
                return;
            }

            this.checkPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint)) as Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint;
            #region {3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
            //���{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}pacs�ӿ�����
            if (this.isInitPacs)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order temp = null;
                temp = this.GetObjectFromFarPoint(this.fpSpread1.Sheets[iActiveSheet].ActiveRowIndex, iActiveSheet);
                if (temp.Item.SysClass.ID.ToString() == "UC")
                {
                    if (this.pacsInterface == null)
                    {
                        this.InitPacsInterface();
                    }
                    if (this.pacsInterface != null)
                    {
                        this.pacsInterface.OprationMode = "2";
                        this.pacsInterface.SetPatient(this.myPatientInfo);
                        this.pacsInterface.PlaceOrder(temp);
                        this.pacsInterface.ShowForm();

                        return;
                    }
                }
            }
            #endregion
            if (this.checkPrint == null)
            {
                this.checkPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint)) as Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint;
                if (this.checkPrint == null)
                {
                    MessageBox.Show("��ýӿ�IcheckPrint����\n������û��ά����صĴ�ӡ�ؼ����ӡ�ؼ�û��ʵ�ֽӿڼ���ӿ�IcheckPrint\n����ϵͳ����Ա��ϵ��");
                    return ;
                }
            }
            this.checkPrint.Reset();
            this.checkPrint.ControlValue(myPatientInfo, alItems);
            this.checkPrint.Show(); 


            //Neusoft.HISFC.Models.RADT.PatientInfo p = this.GetPatient().Clone();
            //string combo = "";
            //if (alItems.Count > 1)
            //{
            //    combo = (alItems[0] as Neusoft.HISFC.Models.Order.Inpatient.Order).Combo.ID;
            //    for (int i = 1; i < alItems.Count; i++)
            //    {
            //        if (combo != (alItems[i] as Neusoft.HISFC.Models.Order.Inpatient.Order).Combo.ID)
            //        {
            //            MessageBox.Show("����ѡ�����ĿӦ�ÿ�����ͬ�ļ�鵥\n������ѡ��", "��ʾ");
            //            return;
            //        }

            //    }
            //}
            //pacsInterface.frmPacsApply f = new pacsInterface.frmPacsApply(alItems, p);
            //if (f.ShowDialog() == DialogResult.OK)
            //{

            //}
        }
        /// <summary>
        /// ��ӻ���
        /// </summary>
        /// <param name="sender"></param>
        public void AddConsultation(object sender)
        {
            //if (this.PatientInfo == null)
            //{
            //    MessageBox.Show("����ѡ����!");
            //    return;
            //}
            //Neusoft.HISFC.Models.RADT.PatientInfo p = this.GetPatient().Clone();
            //((Neusoft.HISFC.Models.Order.Inpatient.Order)sender).Patient = p;

            //ucConsultation uc = new ucConsultation(sender as Neusoft.HISFC.Models.Order.Inpatient.Order);
            //uc.IsApply = true;
            //uc.DisplayPatientInfo(this.myPatientInfo);
            ////			Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
            //Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(uc);

        }

        
        private void ComboOrder(int k)
        {
            #region ���ҽ��
            int iSelectionCount = 0;
            for (int i = 0; i < this.fpSpread1.Sheets[k].Rows.Count; i++)
            {
                if (this.fpSpread1.Sheets[k].IsSelected(i, 0))
                    iSelectionCount++;
            }

            if (iSelectionCount > 1)
            {
                string t = "";//��Ϻ� �޸ĳɶ�����Ϻ�
                int iSort = -1;
                string time = "";
                #region {99211BBB-0D23-40d3-9E86-902423A7F6CA}
                int iCounter = 0;
                #endregion
                if (this.ValidComboOrder() == -1) return;//У�����ҽ��
                for (int i = 0; i < this.fpSpread1.Sheets[k].Rows.Count; i++)
                {
                    if (this.fpSpread1.Sheets[k].IsSelected(i, 0))
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order o = this.GetObjectFromFarPoint(i, k);
                        #region ���İ�BUG addby xuewj 2009-09-04 {40F651AC-C372-4ca1-AFB2-F5F8B95D1E6D}
                        if (!this.htSubs.ContainsKey(o.ID))
                        {
                            this.htSubs.Add(o.ID, o.Combo.ID);
                        }
                        #endregion
                        if (t == "")
                        {
                            t = o.Combo.ID;
                            time = o.Frequency.Time;
                        }
                        else
                        {
                            o.Combo.ID = t;
                            o.Frequency.Time = time;
                        }

                        if (iSort == -1)
                        {
                            iSort = int.Parse(this.fpSpread1.Sheets[k].Cells[i, this.myOrderClass.iColumns[28]].Text);
                        }
                        else
                        {
                            #region {99211BBB-0D23-40d3-9E86-902423A7F6CA}
                            o.SortID = iSort - iCounter;
                            //o.SortID = iSort - 1;
                            //o.SortID = iSort;
                            #endregion
                        }
                        this.AddObjectToFarpoint(o, i, k, EnumOrderFieldList.Item);
                        #region {99211BBB-0D23-40d3-9E86-902423A7F6CA}
                        iCounter++;
                        #endregion
                    }
                    #region {99211BBB-0D23-40d3-9E86-902423A7F6CA}
                    else
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order ordTmp = this.GetObjectFromFarPoint(i, k);
                        if (ordTmp.Status == 0 && iCounter > 0 && iCounter < iSelectionCount)
                        {
                            ordTmp.SortID = ordTmp.SortID - iSelectionCount + iCounter;
                            this.AddObjectToFarpoint(ordTmp, i, k, EnumOrderFieldList.Item);
                        }
                    }
                    #endregion
                }
                this.fpSpread1.Sheets[k].ClearSelection();
            }
            else
            {
                MessageBox.Show("��ѡ�������");
            }
            #endregion
        }
        /// <summary>
        /// ���ҽ��
        /// </summary>
        public void ComboOrder()
        {
            ComboOrder(this.fpSpread1.ActiveSheetIndex);
            this.RefreshCombo();
        }
        /// <summary>
        /// ȡ�����
        /// </summary>
        public void CancelCombo()
        {
            if (this.fpSpread1.ActiveSheet.SelectionCount <= 1)
            {
                MessageBox.Show("������ȡ����ϵ�������");
                return;
            }
            for (int i = 0; i < this.fpSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.fpSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order o = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex);
                    #region ���İ�BUG addby xuewj 2009-09-04 {40F651AC-C372-4ca1-AFB2-F5F8B95D1E6D}
                    if (!this.htSubs.ContainsKey(o.ID))
                    {
                        this.htSubs.Add(o.ID, o.Combo.ID);
                    }
                    #endregion
                    o.Combo.ID = this.OrderManagement.GetNewOrderComboID();
                    #region {99211BBB-0D23-40d3-9E86-902423A7F6CA}
                    //o.SortID = MaxSort + 1;
                    //MaxSort = MaxSort + 1;
                    #endregion
                    this.AddObjectToFarpoint(o, i, this.fpSpread1.ActiveSheetIndex,EnumOrderFieldList.Item);
                }
            }
            this.fpSpread1.ActiveSheet.ClearSelection();
            this.RefreshCombo();
           
        }
        /// <summary>
        /// �������
        /// </summary>
        public void SaveSortID()
        {
            this.SaveSortID(true);
        }
        /// <summary>
        /// ��ѯʱ��ı��棬���򱣴�
        /// </summary>
        /// <param name="prompt"></param>
        public void SaveSortID(bool prompt)
        {
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    int k = 1;
                    for (int j = 0; j < fpSpread1.Sheets[i].RowCount; j++)
                    {
                        if (OrderManagement.UpdateOrderSortID(fpSpread1.Sheets[i].Cells[j, this.myOrderClass.iColumns[2]].Text, (k++).ToString()) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            MessageBox.Show(OrderManagement.Err);
                            return;
                        }
                    }
                }
            }
            catch { Neusoft.FrameWork.Management.PublicTrans.RollBack();; return; }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
          
            if (prompt) MessageBox.Show("ҽ��˳�򱣴�ɹ���");
        }

        protected void SaveSortID(int row)
        {
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            try
            {
                if (OrderManagement.UpdateOrderSortID(fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[2]].Text, fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[28]].Value.ToString()) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(OrderManagement.Err);
                    return;
                }

                ArrayList al = OrderManagement.QuerySubtbl(fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[4]].Text);
                if (al != null)
                {
                    foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in al)
                    {
                        if (OrderManagement.UpdateOrderSortID(order.ID, fpSpread1.ActiveSheet.Cells[row, this.myOrderClass.iColumns[28]].Value.ToString()) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            MessageBox.Show(OrderManagement.Err);
                            return;
                        }
                    }
                }
            }
            catch { Neusoft.FrameWork.Management.PublicTrans.RollBack();; return; }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
           
        }
        
        protected void CheckSortID()
        {
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    int k = 0;
                    for (int j = 0; j < fpSpread1.Sheets[i].RowCount; j++)
                    {
                        k = k + 1;
                        if (fpSpread1.Sheets[i].Cells[j, this.myOrderClass.iColumns[28]].Value.ToString() != (k).ToString())
                        {
                            if (OrderManagement.UpdateOrderSortID(fpSpread1.Sheets[i].Cells[j, this.myOrderClass.iColumns[2]].Text, (k).ToString()) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                MessageBox.Show(OrderManagement.Err);
                                return;
                            }
                        }
                    }
                }
            }
            catch { Neusoft.FrameWork.Management.PublicTrans.RollBack();; return; }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
           
        }


        /// <summary>
        /// �������ѯ{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}pacs�ӿ�����
        /// </summary>
        /// <param name="patientID"></param>
        /// <returns></returns>
        public int ShowPacsResultByPatient(string patientID)
        {
            if (this.enabledPacs)
            {
                if (this.pacsInterface == null)
                {
                    this.InitPacsInterface();
                    //return-1;
                }

                if (this.enabledPacs == true && this.pacsInterface != null)
                {
                    this.pacsInterface.OprationMode = "2";
                    this.pacsInterface.PacsViewType = "2";

                    this.pacsInterface.ShowResultByPatient(patientID);
                    //this.pacsInterface.ShowResultByPatient("985656"); 
                }
            }
            return 0;
        }

        /// <summary>
        /// ��Ӳ�ҩҽ��{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// </summary>
        /// <param name="alHerbalOrder"></param>
        public void AddHerbalOrders(ArrayList alHerbalOrder)
        {
            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988} //��ҩ������ҩ��������
            using (Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder uc = new Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder(true, Neusoft.HISFC.Models.Order.EnumType.SHORT, this.GetReciptDept().ID))
            {
                uc.IsClinic = false;

                uc.Patient = new Neusoft.HISFC.Models.RADT.PatientInfo();//
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                uc.AlOrder = alHerbalOrder;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                {
                    foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in uc.AlOrder)
                    {
                        //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                        info.DoseOnce = info.Qty;
                        //info.Qty = info.Qty * info.HerbalQty;//{DC0E8BDB-D918-4c14-8474-3D2E6F986A57}

                        this.AddNewOrder(info, 1);
                    }
                    uc.Clear();
                    this.RefreshCombo();
                }
            }
        }

        /// <summary>
        /// �޸Ĳ�ҩ{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// </summary>
        public void ModifyHerbal()
        {
            if (this.fpSpread1.ActiveSheet.RowCount == 0)
            {
                return;
            }

            ArrayList alModifyHerbal = new ArrayList(); //Ҫ�޸ĵĲ�ҩҽ��

            Neusoft.HISFC.Models.Order.Inpatient.Order orderTemp = this.fpSpread1.ActiveSheet.Rows[this.fpSpread1.ActiveSheet.ActiveRowIndex].Tag as
                Neusoft.HISFC.Models.Order.Inpatient.Order;

            if (orderTemp == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(orderTemp.Combo.ID))
            {
                alModifyHerbal.Add(orderTemp);
            }
            else
            {

                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = this.fpSpread1.ActiveSheet.Rows[i].Tag as
                        Neusoft.HISFC.Models.Order.Inpatient.Order;
                    if (order == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(order.Combo.ID))
                    {
                        continue;
                    }
                    //{1A93C0BB-30CD-4097-81F8-F074B22A830E}
                    if (order.Item.SysClass.ID.ToString() != "PCC")
                    {
                        continue;
                    }
                    if (order.Status != 0)
                    {
                        continue;
                    }
                    if (order.Combo.ID == orderTemp.Combo.ID)
                    {
                        alModifyHerbal.Add(order);
                    }
                }
            }

            if (alModifyHerbal.Count > 0)
            {
                using (Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder uc = new Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder(true, Neusoft.HISFC.Models.Order.EnumType.SHORT, this.GetReciptDept().ID))
                {
                    uc.Patient = new Neusoft.HISFC.Models.RADT.PatientInfo();//
                    #region {49026086-DCA3-4af4-A064-58F7479C324A}
                    uc.refreshGroup += new RefreshGroupTree(uc_refreshGroup);
                    #endregion
                    Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                    uc.AlOrder = alModifyHerbal;
                    uc.OpenType = "M"; //�޸�
                    uc.IsClinic = false;
                    DialogResult r = Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);

                    if (uc.IsCancel == true)
                    {//ȡ����
                        return;
                    }

                    if (uc.OpenType == "M")
                    {//��Ϊ�¼�ģʽ�Ͳ�ɾ����
                        if (this.Delete(this.fpSpread1.ActiveSheet.ActiveRowIndex, true) < 0)
                        {//ɾ��ԭҽ�����ɹ�
                            return;
                        }
                    }

                    if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                    {
                        foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in uc.AlOrder)
                        {
                            //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                            info.DoseOnce = info.Qty;
                            //info.Qty = info.Qty * info.HerbalQty;

                            this.AddNewOrder(info, this.fpSpread1.ActiveSheetIndex);
                        }
                        uc.Clear();
                        this.RefreshCombo();
                    }
                }
            }
            else//{1A93C0BB-30CD-4097-81F8-F074B22A830E}
            {
                MessageBox.Show("��˲飬û�в�ҩ��Ϣ��");
                return;
            }

        }

        #region {49026086-DCA3-4af4-A064-58F7479C324A}
        private void uc_refreshGroup()
        {
            this.refreshGroup();
        }
        #endregion


        #region {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}

        /// <summary>
        /// ճ��ҽ��
        /// {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
        /// </summary>
        public void PasteOrder()
        {
            try
            {
                List<string> orderIdList = Classes.HistoryOrderClipboard.OrderList;
                if ((orderIdList == null) || (orderIdList.Count <= 0)) return;

                if (Neusoft.HISFC.Components.Order.Classes.HistoryOrderClipboard.Type == ServiceTypes.I)
                {
                    DateTime mydtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
                    string err = string.Empty;
                    for (int count = 0; count < orderIdList.Count; count++)
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order order = this.OrderManagement.QueryOneOrder(orderIdList[count]);
                        decimal qty = order.Qty;
                        if (order != null)
                        {
                            order.Patient = this.myPatientInfo;
                            if (order.Item.ItemType == EnumItemType.Drug)
                            {
                                if (Neusoft.HISFC.BizProcess.Integrate.Order.FillPharmacyItemWithStockDept(null, ref order, out err) == -1)
                                {
                                    MessageBox.Show(err);
                                    continue;
                                }
                                if (order == null) return;
                            }
                            else if (order.Item.ItemType == EnumItemType.UnDrug)
                            {
                                if (Neusoft.HISFC.BizProcess.Integrate.Order.FillFeeItem(null, ref order, out err) == -1)
                                {
                                    MessageBox.Show(err);
                                    continue;
                                }
                                if (order == null) return;
                            }
                            //ҽ��״̬����
                            order.Status = 0;
                            order.ID = "";
                            order.MOTime = mydtNow;
                            order.Combo.ID = "";
                            order.Qty = qty;
                            //��ӵ���ǰ����а���ҽ�����ͽ��з���
                            if (order.OrderType.IsDecompose)
                            {
                                this.fpSpread1.ActiveSheetIndex = 0;
                            }
                            else
                            {
                                this.fpSpread1.ActiveSheetIndex = 1;
                            }
                            this.AddNewOrder(order, this.fpSpread1.ActiveSheetIndex);
                        }
                    }
                    this.fpSpread1.Sheets[this.fpSpread1.ActiveSheetIndex].ClearSelection();
                }
                else
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����԰������ҽ������ΪסԺҽ����"));
                    return;
                }
            }
            catch { }
        }



        #endregion


        #endregion

        #region �¼�
       
        /// <summary>
        /// ҽ���仯����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changedField"></param>
        protected virtual void ucItemSelect1_OrderChanged(Neusoft.HISFC.Models.Order.Inpatient.Order sender, EnumOrderFieldList changedField)
        {
            dirty = true;
            if (!this.EditGroup && !this.bIsDesignMode)
                return;

            if (!this.EditGroup)//{E679E3A6-9948-41a8-B390-DD9A57347681}�жϲ��ǿ���ҽ��ģʽ�Ͳ�������ӿ�
            {
                #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�

                if (this.IAlterOrderInstance == null)
                {
                    this.InitAlterOrderInstance();
                }

                if (this.IAlterOrderInstance != null)
                {
                    if (this.IAlterOrderInstance.AlterOrder(this.myPatientInfo, this.myReciptDoc, this.myReciptDept, ref sender) == -1)
                    {
                        return;
                    }
                }

                #endregion
            }

            if (this.ucItemSelect1.OperatorType == Operator.Add)
            {
                this.AddNewOrder(sender, this.fpSpread1.ActiveSheetIndex);
                this.fpSpread1.ActiveSheet.ClearSelection();
                this.fpSpread1.ActiveSheet.AddSelection(0, 0, 1, 1);
                this.fpSpread1.ActiveSheet.ActiveRowIndex = 0;
            }
            else if (this.ucItemSelect1.OperatorType == Operator.Delete)
            {

            }
            else if (this.ucItemSelect1.OperatorType == Operator.Modify)
            {
                //�޸�
                if (this.fpSpread1.ActiveSheet.SelectionCount > 1)
                {
                    ArrayList alRows = GetSelectedRows();
                    for (int i = 0; i < alRows.Count; i++)
                    {
                        if (this.ucItemSelect1.CurrentRow == System.Convert.ToInt32(alRows[i]))
                        {
                            this.AddObjectToFarpoint(sender, this.ucItemSelect1.CurrentRow, this.fpSpread1.ActiveSheetIndex,changedField);
                        }
                        else
                        {
                            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.GetObjectFromFarPoint(int.Parse(alRows[i].ToString()), this.fpSpread1.ActiveSheetIndex);
                            if (order.Combo.ID == sender.Combo.ID)
                            {
                                if (changedField == EnumOrderFieldList.Item || changedField == EnumOrderFieldList.Frequency
                                    || changedField == EnumOrderFieldList.Usage
                                    || changedField == EnumOrderFieldList.BeginDate
                                    || changedField == EnumOrderFieldList.EndDate)
                                {
                                    //��ϵ�һ���޸�
                                    if (order.Item.SysClass.ID.ToString() != "PCC") order.Usage = sender.Usage.Clone();

                                    order.Frequency.ID = sender.Frequency.ID;
                                    order.Frequency.Name = sender.Frequency.Name;
                                    order.Frequency.Time = sender.Frequency.Time;
                                    order.Frequency.Usage = sender.Frequency.Usage.Clone();
                                    order.BeginTime = sender.BeginTime;
                                    order.EndTime = sender.EndTime;
                                    this.AddObjectToFarpoint(order, int.Parse(alRows[i].ToString()), this.fpSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.AddObjectToFarpoint(sender, this.ucItemSelect1.CurrentRow, this.fpSpread1.ActiveSheetIndex,changedField);
                }
                RefreshOrderState();
            }
            dirty = false;

            this.isEdit = true;

            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (!sender.OrderType.IsDecompose)
            {
                ShowTempCost();
            }
            #endregion
        }

        #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}

        /// <summary>
        /// ���㵱ǰ�����������
        /// </summary>
        private void ShowTempCost()
        {
            decimal pCost = this.SubTempCost("P");//��ҩ��
            decimal pczCost = this.SubTempCost("PCZ");//�г�ҩ��
            decimal pccCost = this.SubTempCost("PCC");//�в�ҩ��
            decimal ulCost = this.SubTempCost("UL");//�����
            decimal ucCost = this.SubTempCost("UC");//����
            decimal otherCost = this.SubTempCost("OHTER"); //��������
            decimal totCost = pCost + pczCost + pccCost + ulCost + ucCost + otherCost;//�����ܷ���

            this.lbTempTotCost.Text = "��ǰ����������" + decimal.Round(totCost, 2).ToString() +
                (totCost == 0m ? "" : " ����") +
                (pCost == 0m ? "" : (" ��ҩ��" + pCost.ToString())) +
                (pczCost == 0m ? "" : (" �г�ҩ��" + pczCost.ToString())) +
                (pccCost == 0m ? "" : (" �в�ҩ��" + pccCost.ToString())) +
                (ulCost == 0m ? "" : (" ���飺" + ulCost.ToString())) +
                (ucCost == 0m ? "" : (" ��飺" + ucCost.ToString())) +
                (otherCost == 0m ? "" : (" ������" + otherCost.ToString()));
        }

        /// <summary>
        /// ����ϵͳ�����㵱ǰ�����������
        /// <param name="sysClass">ϵͳ���</param>
        /// </summary>
        private decimal SubTempCost(string sysClass)
        {
            decimal tempCost=0m;
            Neusoft.HISFC.Models.Order.Inpatient.Order orderInfo=null;
            for (int i = 0; i < this.fpSpread1.Sheets[1].Rows.Count; i++)
            {
                orderInfo = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.Sheets[1].Rows[i].Tag;
                if (orderInfo == null || (orderInfo.Status != 0 && orderInfo.Status != 5)
                    ||!orderInfo.OrderType.IsCharge||orderInfo.Item.SysClass.ID.ToString()=="UO")
                {
                    continue;
                }

                if (orderInfo.Item.SysClass.ID.ToString() == sysClass)
                {
                    if (sysClass == "P" || sysClass == "PCZ" || sysClass == "PCC")//��ҩ���г�ҩ���в�ҩ
                    {
                        string itemPriceUnit = orderInfo.Item.PriceUnit;
                        if (string.IsNullOrEmpty(itemPriceUnit))
                        {
                            Neusoft.HISFC.Models.Pharmacy.Item itemInfo = this.pManagement.GetItem(orderInfo.Item.ID);
                            if (itemInfo != null && itemInfo.ID != "")
                            {
                                itemPriceUnit = itemInfo.PriceUnit;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        if (itemPriceUnit == orderInfo.Unit)//��װ��λ
                        {
                            if (sysClass == "PCC")
                            {
                                tempCost += ((Neusoft.HISFC.Models.Pharmacy.Item)orderInfo.Item).PriceCollection.RetailPrice * (orderInfo.HerbalQty*orderInfo.Qty / orderInfo.Item.PackQty);
                            }
                            else
                            {
                                tempCost += ((Neusoft.HISFC.Models.Pharmacy.Item)orderInfo.Item).PriceCollection.RetailPrice * orderInfo.Qty;
                            }
                        }
                        else//��С��λ
                        {
                            if (sysClass == "PCC")//��ֹ��ҩ��λά������ʱ �޷���ȷ��ʾ��ҩ�۸�
                            {
                                tempCost += ((Neusoft.HISFC.Models.Pharmacy.Item)orderInfo.Item).PriceCollection.RetailPrice * (orderInfo.HerbalQty*orderInfo.Qty / orderInfo.Item.PackQty);
                            }
                            else
                            {
                                tempCost += ((Neusoft.HISFC.Models.Pharmacy.Item)orderInfo.Item).PriceCollection.RetailPrice * (orderInfo.Qty / orderInfo.Item.PackQty);
                            }
                        }
                    }
                    else
                    {
                        tempCost += orderInfo.Item.Price * orderInfo.Qty;
                    }
                }
                else if (sysClass == "OHTER")
                {
                    if (orderInfo.Item.SysClass.ID.ToString() != "P" &&
                        orderInfo.Item.SysClass.ID.ToString() != "PCZ" &&
                        orderInfo.Item.SysClass.ID.ToString() != "PCC" &&
                        orderInfo.Item.SysClass.ID.ToString() != "UC" &&
                        orderInfo.Item.SysClass.ID.ToString() != "UL")//��ҩ���г�ҩ���в�ҩ
                    {
                        tempCost += orderInfo.Item.Price * orderInfo.Qty;
                    }
                }
            }

            tempCost = decimal.Round(tempCost, 2);
            return tempCost;
        }  
        #endregion
       
        private void fpSpread1_Sheet1_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            try
            {
                if (this.bIsDesignMode && dirty == false)
                {
                    int i = 0;
                    switch (GetColumnNameFromIndex(e.Column))
                    {
                        case "�÷�����":
                            i = this.myOrderClass.GetColumnIndexFromName("�÷�����");
                            this.fpSpread1.ActiveSheet.Cells[e.Row, i].Text =
                                Classes.Function.HelperUsage.GetName(this.fpSpread1.ActiveSheet.Cells[e.Row, e.Column].Text);
                            break;
                        case "ҽ��״̬":
                            RefreshOrderState();
                         
                            break;
                        default:
                            break;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// ˢ��ҽ��״̬
        /// </summary>
        /// <param name="row"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="reset"></param>
        private void ChangeOrderState(int row, int SheetIndex, bool reset)
        {
            try
            {
                int i = this.myOrderClass.iColumns[3];//this.GetColumnIndexFromName("ҽ��״̬");
                int state = int.Parse(this.fpSpread1.Sheets[SheetIndex].Cells[row, i].Text);

                if (GetObjectFromFarPoint(row, SheetIndex).ID != "" && reset)
                {
                    state = OrderManagement.QueryOneOrderState(GetObjectFromFarPoint(row, SheetIndex).ID);
                    this.fpSpread1.Sheets[SheetIndex].Cells[row, i].Value = state;
                }

                switch (state)
                {
                    case 0: //�¿���
                        this.fpSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(128, 255, 128);
                        break;
                    case 1://���
                        this.fpSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(106, 174, 242);
                        break;
                    case 2://ִ��
                        this.fpSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(243, 230, 105);
                        break;
                    case 3://ֹͣ
                        this.fpSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(248, 120, 222);
                        break;
                    default: //�����ҽ��
                        this.fpSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.Black;
                        break;
                }
                if (this.IsDesignMode)
                {
                    this.GetObjectFromFarPoint(row, SheetIndex).Status = state;
                }
            }
            catch { }

        }
        /// <summary>
        /// ѡ��ҽ���޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread1_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }
        private void SelectionChanged()
        {
            #region "�仯"
            //.
            //if ((this.bIsDesignMode && dirty == false) || (this.bIsDesignMode && this.EditGroup))
            //{
                #region ѡ��
                //ÿ��ѡ��仯ǰ���������ʾ Add By liangjz 2005-08
                this.ucItemSelect1.Clear();

                if (this.fpSpread1.ActiveSheet.RowCount <= 0) return;

                //�¿��� ���ܸ���
                if (int.Parse(this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[3]].Text) == 0)
                {
                    #region 
                    //����Ϊ��ǰ��
                    this.ucItemSelect1.CurrentRow = this.fpSpread1.ActiveSheet.ActiveRowIndex;
                    this.ActiveRowIndex = this.fpSpread1.ActiveSheet.ActiveRowIndex;
                    this.currentOrder = this.GetObjectFromFarPoint(this.fpSpread1.ActiveSheet.ActiveRowIndex, this.fpSpread1.ActiveSheetIndex);
                    this.ucItemSelect1.Order = this.currentOrder;
                    //���������ѡ��
                    if (this.ucItemSelect1.Order.Combo.ID != "" && this.ucItemSelect1.Order.Combo.ID != null)
                    {
                        int comboNum = 0;//��õ�ǰѡ������
                        for (int i = 0; i < this.fpSpread1.ActiveSheet.Rows.Count; i++)
                        {
                            string strComboNo = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex).Combo.ID;
                            if (this.ucItemSelect1.Order.Combo.ID == strComboNo && i != this.fpSpread1.ActiveSheet.ActiveRowIndex)
                            {
                                this.fpSpread1.ActiveSheet.AddSelection(i, 0, 1, 1);
                                comboNum++;
                            }
                        }
                        if (comboNum == 0)
                        {
                            //ֻ��һ��
                            if(OrderCanCancelComboChanged!=null) this.OrderCanCancelComboChanged(false);//����ȡ�����
                            
                        }
                        else
                        {
                            if (OrderCanCancelComboChanged != null) this.OrderCanCancelComboChanged(true);//����ȡ�����                            
                        }
                    }

                    if (OrderCanSetCheckChanged != null) this.OrderCanSetCheckChanged(true);//��ӡ������뵥ʧЧ
                    
                    #endregion
                }
                else
                {
                    this.ActiveRowIndex = -1;
                }
                #endregion

                
        //}
            #endregion
        }
        /// <summary>
        /// �ж����ҽ����Ч��
        /// </summary>
        /// <returns></returns>
        private int ValidComboOrder()
        {
            return this.myOrderClass.ValidComboOrder(this.OrderManagement);
        
        }
        private void fpSpread1_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
        {

            

            if (e.SheetTabIndex == 0)
            {
                this.plPatient.BackColor = Color.FromArgb(255, 255, 192);
                this.OrderType = Neusoft.HISFC.Models.Order.EnumType.LONG;
                this.ActiveRowIndex = -1;
                if (this.OrderCanOperatorChanged != null) this.OrderCanOperatorChanged(false);
                #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
                this.plPatient.Height = 36; 
                #endregion
            }
            else
            {
                this.plPatient.BackColor = Color.FromArgb(225, 255, 255);
                this.OrderType = Neusoft.HISFC.Models.Order.EnumType.SHORT;
                this.ActiveRowIndex = -1;
                if (this.bIsDesignMode)
                {
                    if (this.OrderCanOperatorChanged != null) this.OrderCanOperatorChanged(true);
                }
                else
                {
                    if (this.OrderCanOperatorChanged != null) this.OrderCanOperatorChanged(false);

                }

                #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
                if (this.IsDesignMode)
                {
                    this.plPatient.Height = 72;
                    this.ShowTempCost();
                }
                #endregion
            }
            try
            {
                //����ѿ���ҽ��������ʾ  Add By liangjz 2005-08
                this.ucItemSelect1.Clear();
                this.fpSpread1.Sheets[e.SheetTabIndex].ClearSelection();
            }
            catch { }
        }

        #endregion

        #region ����
        protected bool bIsDesignMode = false;
        protected bool bIsShowPopMenu = true;

        /// <summary>
        /// �Ƿ���ʾ�Ҽ��˵�
        /// </summary>
        public bool IsShowPopMenu
        {
            get
            {
                return this.bIsShowPopMenu;
            }
            set
            {
                this.bIsShowPopMenu = value;
            }
        }
        /// <summary>
        /// �Ƿ���ʾ�����Ŀϸ��Ŀ
        /// </summary>
        [DefaultValue(false), Browsable(false)]
        public bool IsLisDetail
        {
            set
            {
                this.ucItemSelect1.IsLisDetail = value;
            }
        }
        /// <summary>
        /// �Ƿ���ģʽ
        /// </summary>
        [DefaultValue(false),Browsable(false)]
        public bool IsDesignMode
        {
            get
            {
                return this.bIsDesignMode;
            }
            set
            {
                if (this.bIsDesignMode != value)
                {
                    this.bIsDesignMode = value;
                    
                    SetFP();
                    this.QueryOrder();
                }
            }
        }

        private void SetFP()
        {
            this.ucItemSelect1.Visible = this.bIsDesignMode;
        }

        /// <summary>
        /// ���߻�����Ϣ
        /// </summary>
        public void SetPatient(Neusoft.HISFC.Models.RADT.PatientInfo value)
        {
            if (!EditGroup)//{D17BD9FB-F362-4755-97FE-08404D477C39} ���2�����׹���ť ������ť����Ӧ
            {
                //{F38618E9-7421-423d-80A9-401AFED0B855}
                this.isShowOrderFinished = false;

                this.myPatientInfo = value;
                this.ucItemSelect1.PatientInfo = value;
                this.QueryOrder();

                //{F38618E9-7421-423d-80A9-401AFED0B855}
                this.isShowOrderFinished = true;
            }
        }
        /// <summary>
        /// Ĭ�ϳ���ҽ��
        /// </summary>
        protected Neusoft.HISFC.Models.Order.EnumType myOrderType = Neusoft.HISFC.Models.Order.EnumType.LONG;
        /// <summary>
        /// ���ó���ҽ������
        /// </summary>
        [DefaultValue(Neusoft.HISFC.Models.Order.EnumType.LONG)]
        public Neusoft.HISFC.Models.Order.EnumType OrderType
        {
            get
            {
                return this.myOrderType;
            }
            set
            {
                try
                {
                    this.myOrderType = value;
                    if (this.myOrderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                    {
                        this.ucItemSelect1.LongOrShort = 0;
                    }
                    else
                    {
                        this.ucItemSelect1.LongOrShort = 1;
                    }
                }
                catch { }
            }
        }

        protected Neusoft.FrameWork.Models.NeuObject myReciptDept = null;
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        [DefaultValue(null)]
        public void SetReciptDept(Neusoft.FrameWork.Models.NeuObject value)
        {
            
                this.myReciptDept = value;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Neusoft.FrameWork.Models.NeuObject GetReciptDept()
        {
            
                try
                {
                    if (this.myReciptDept == null) this.myReciptDept = ((Neusoft.HISFC.Models.Base.Employee)this.GetReciptDoc()).Dept.Clone(); //��������
                }
                catch { }
                return this.myReciptDept;
            
            
        }
        protected Neusoft.FrameWork.Models.NeuObject myReciptDoc = null;
        
        /// <summary>
        /// ��ǰ����ҽ��
        /// </summary>
        public void SetReciptDoc(Neusoft.FrameWork.Models.NeuObject value)
        {
              this.myReciptDoc = value;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Neusoft.FrameWork.Models.NeuObject  GetReciptDoc()
        {
            try
            {
                if (this.myReciptDoc == null) this.myReciptDoc = OrderManagement.Operator.Clone();
            }
            catch { }
            return this.myReciptDoc;
         }

        /// <summary>
        /// �Ƿ��޸Ĺ�ҽ��
        /// </summary>
        private bool isEdit = false;
        /// <summary>
        /// �Ƿ�
        /// </summary>
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }
        private bool bIsShowIndex = false;
        /// <summary>
        /// ��ʾindex
        /// </summary>
        public bool IsShowIndex
        {
            set
            {
                bIsShowIndex = value;
              

            }
        }
        #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
        private bool isNurseCreate = false;
        /// <summary>
        /// �Ƿ�ʿ����
        /// </summary>
        [DefaultValue(false)]
        public bool IsNurseCreate
        {
            set
            {
                this.isNurseCreate = value;
            }
        }
        #endregion
        #endregion

        #region ����
        /// <summary>
        /// ���ʵ��toTable
        /// </summary>
        /// <param name="list"></param>
        private void AddObjectsToTable(ArrayList list)
        {
            if (dsAllLong != null)//�������BY zuowy 2005-9-15
                dsAllLong.Tables[0].Clear();//ԭ��û������
            if (dsAllShort != null)//�������BY zuowy 2005-9-15
                dsAllShort.Tables[0].Clear();//ԭ��û������
            foreach (object obj in list)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order order = obj as Neusoft.HISFC.Models.Order.Inpatient.Order;
                if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    //����ҽ��

                    dsAllLong.Tables[0].Rows.Add(AddObjectToRow(order, dsAllLong.Tables[0]));
                }
                else
                {
                    //��ʱҽ��
                    dsAllShort.Tables[0].Rows.Add(AddObjectToRow(order, dsAllShort.Tables[0]));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private DataRow AddObjectToRow(object obj, DataTable table)
        {
            DataRow row = table.NewRow();
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            try
            {
                order = obj as Neusoft.HISFC.Models.Order.Inpatient.Order;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
            {
                Neusoft.HISFC.Models.Pharmacy.Item objItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                row["��ҩ"] = Neusoft.FrameWork.Function.NConvert.ToInt32(order.Combo.IsMainDrug);//5
                row["ÿ������"] = order.DoseOnce;//9
                row["��λ"] = objItem.DoseUnit;//0415 2307096 wang renyi
                row["����"] = order.HerbalQty;//11
            }
            else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
            {
                //Neusoft.HISFC.Models.Fee.Item.Undrug objItem = order.Item as Neusoft.HISFC.Models.Fee.Item.Undrug;
            }

            if (order.Note != "")
            {
                row["!"] = order.Note;
            }
            row["��Ч"] = Neusoft.FrameWork.Function.NConvert.ToInt32(order.OrderType.ID);     //0
            row["ҽ������"] = order.OrderType.Name;//1
            row["ҽ����ˮ��"] = order.ID;//2
            row["ҽ��״̬"] = order.Status;//�¿�������ˣ�ִ��
            row["��Ϻ�"] = order.Combo.ID;//4

            if (order.Item.Specs == null || order.Item.Specs.Trim() == "")
                row["ҽ������"] = order.Item.Name;//6
            else
                row["ҽ������"] = order.Item.Name + "[" + order.Item.Specs + "]";

            //ҽ����ҩ-֪��ͬ����
            if (order.IsPermission) row["ҽ������"] = "���̡�" + row["ҽ������"];

            ValidNewOrder(order);
            row["����"] = order.Qty;//7
            row["������λ"] = order.Unit;//8
            row["Ƶ�α���"] = order.Frequency.ID;
            row["Ƶ������"] = order.Frequency.Name;
            row["�÷�����"] = order.Usage.ID;
            row["�÷�����"] = order.Usage.Name;//15
            row["��ʼʱ��"] = order.BeginTime;
            row["ִ�п��ұ���"] = order.ExeDept.ID;
            //if(order.ExeDept.Name == "" && order.ExeDept.ID !="" ) order.ExeDept.Name = this.GetDeptName(order.ExeDept);
            row["ִ�п���"] = order.ExeDept.Name;
            row["�Ӽ�"] = order.IsEmergency;
            row["��鲿λ"] = order.CheckPartRecord;
            row["��������/��鲿λ"] = order.Sample;
            row["�ۿ���ұ���"] = order.StockDept.ID;
            row["�ۿ����"] = order.StockDept.Name;

            row["��ע"] = order.Memo;//20
            row["¼���˱���"] = order.Oper.ID;
            row["¼����"] = order.Oper.Name;
            row["����ҽ��"] = order.ReciptDoctor.Name;
            row["��������"] = order.ReciptDept.Name;
            row["����ʱ��"] = order.MOTime;

            if (order.EndTime != DateTime.MinValue)
                row["ֹͣʱ��"] = order.EndTime;//25

            row["ֹͣ�˱���"] = order.DCOper.ID;
            row["ֹͣ��"] = order.DCOper.Name;

            row["˳���"] = order.SortID;//28

            #region {1AF0EB93-27A8-462f-9A1E-E1A3ECA54ADE} ��ҽ�������ϣ������ٶ�
            if (!this.myOrderClass.HtOrder.ContainsKey(order.ID))
            {
                this.myOrderClass.HtOrder.Add(order.ID, order);
            }
            #endregion

            return row;
        }
        /// <summary>
        /// ���-����
        /// </summary>
        /// <param name="al"></param>
        private void AddObjectsToFarpoint(ArrayList al)
        {
            if (al == null) return;
            int j = 0;
            int k = 0;
            DateTime dtNow;
            try
            {
                dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
            }
            catch
            {
                dtNow = System.DateTime.Now;
            }

            for (int i = 0; i < al.Count; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order order = al[i] as Neusoft.HISFC.Models.Order.Inpatient.Order;
                //Edit By liangjz 2005-10
                if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    if (al.Count < 100 || (order.Status != 3 || order.EndTime >= dtNow.AddDays(-this.iDCControl)))
                    {
                        //����ҽ��
                        this.fpSpread1.Sheets[0].Rows.Add(j, 1);
                        this.AddObjectToFarpoint(al[i], j, 0, EnumOrderFieldList.Item);
                        
                        j++;
                    }
                    else
                        continue;
                }
                else
                {
                    //if (al.Count < 100 || (order.Status == 0 || order.Status == 1 || order.BeginTime >= dtNow.AddDays(-this.iDCControl)))
                    //{
                    //    //��ʱҽ��
                    //    this.fpSpread1.Sheets[1].Rows.Add(k, 1);
                    //    this.AddObjectToFarpoint(al[i], k, 1, EnumOrderFieldList.Item);
                        
                    //    k++;
                    //}
                    //else
                    //    continue;
                    if (al.Count < 1000 && order.Status != 4)//ֻ�������Ĳ���ʾ
                    {
                        this.fpSpread1.Sheets[1].Rows.Add(k, 1);
                        this.AddObjectToFarpoint(al[i], k, 1, EnumOrderFieldList.Item);

                        k++;
                    }
                    else
                        continue;
                }

            }
        }
        /// <summary>
        /// ҽ���޸ĸ���
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <param name="SheetIndex"></param>
        private void AddObjectToFarpoint(object obj, int i, int SheetIndex, EnumOrderFieldList orderlist)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            try
            {
                #region addby xuewj 2010-10-5 �޸�bug���������޸ı�ע���ٱ��� ��ϱ��治�� {16BD2A83-FCCC-4701-8F85-5CFB5CD65573}
                //order = ((Neusoft.HISFC.Models.Order.Inpatient.Order)obj).Clone();
                order = (Neusoft.HISFC.Models.Order.Inpatient.Order)obj; 
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clone����" + ex.Message);
                return;
            }


            try
            {

                if (orderlist == EnumOrderFieldList.Item)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[35]].Text = order.Note;
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[1]].Note = order.Note;
                }

                if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
                {
                    //ҩƷ
                    Neusoft.HISFC.Models.Pharmacy.Item objItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.DoseOnce)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[9]].Text = order.DoseOnce.ToString();//9
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Fu)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[11]].Text = order.HerbalQty.ToString();//11
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.MinUnit)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[10]].Text = objItem.DoseUnit;//0415 2307096 wang renyi
                    if (order.OrderType.IsDecompose)
                    {
                        //����
                    }
                    else //��ʱ
                    {
                        if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Qty)
                            this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[7]].Text = order.Qty.ToString();//7
                        if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Unit)
                            this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[8]].Text = order.Unit;//8
                    }
                }
                else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
                {
                    //��ҩƷ
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[10]].Text = "";//������λ
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Qty)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[7]].Text = order.Qty.ToString();//7
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Unit)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[8]].Text = order.Unit;//8
                }
                else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Base.Item))
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[10]].Text = "";//������λ
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Qty)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[7]].Text = order.Qty.ToString();//7
                    if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Unit)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[8]].Text = order.Unit;//8
                }
                this.ValidNewOrder(order); //��д��Ϣ

                if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[0]].Text = "����";//System.Convert.ToInt16(Order.Inpatient.OrderType.Type).ToString();     //0

                }
                else if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.SHORT)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[0]].Text = "��ʱ";     //0
                }

                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[5]].Text = Neusoft.FrameWork.Function.NConvert.ToInt32(order.Combo.IsMainDrug).ToString();//5
                if (orderlist == EnumOrderFieldList.Item)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[1]].Text = order.OrderType.Name; //1 ����

                    //ҽ������ 
                    if (order.Item.Specs == null || order.Item.Specs.Trim() == "")
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[6]].Text = order.Item.Name.ToString();//6
                    else
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[6]].Text = order.Item.Name.ToString() + "[" + order.Item.Specs + "]"; //1 ���� + ���

                    //ҽ������֪��ͬ����
                    if (order.IsPermission)
                        this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[6]].Text = "���̡�" + this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[6]].Text;

                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[2]].Text = order.ID;//2
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[3]].Text = order.Status.ToString();//�¿�������ˣ�ִ��
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[4]].Text = order.Combo.ID.ToString();//4

                }


                if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Frequency)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[12]].Text = order.Frequency.ID.ToString();
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[13]].Text = order.Frequency.Name;
                }
                if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Usage)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[14]].Text = order.Usage.ID;
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[15]].Text = order.Usage.Name;//15
                }
                if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.BeginDate)
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[16]].Text = order.BeginTime.ToString("yyyy-MM-dd HH:mm:ss");//��ʼʱ��
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[24]].Text = order.MOTime.ToString();//����ʱ��

                if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.ExeDept)
                {
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[17]].Text = order.ExeDept.ID;
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[18]].Text = order.ExeDept.Name;
                }
                if (orderlist == EnumOrderFieldList.Item || orderlist == EnumOrderFieldList.Emc)
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[19]].Value = order.IsEmergency;

                #region {5FD4B69E-B020-4bfb-8C34-FEEB3ADCB56B}
                if (order.IsEmergency)
                {
                    this.fpSpread1.Sheets[SheetIndex].Rows[i].BackColor = Color.GreenYellow;
                }
                else
                {
                    this.fpSpread1.Sheets[SheetIndex].Rows[i].BackColor = Color.White;
                }
                #endregion

                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[31]].Text = order.CheckPartRecord;//��鲿λ
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[32]].Text = order.Sample.Name;//��������
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[33]].Text = order.StockDept.ID;//�ۿ����
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[34]].Text = order.StockDept.Name;

                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[20]].Text = order.Memo;//20
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[21]].Text = order.Oper.ID;
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[22]].Text = order.Oper.Name;

                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[29]].Text = order.ReciptDoctor.Name;//����ҽ��
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[23]].Text = order.ReciptDept.Name;//��������

                if (order.EndTime != DateTime.MinValue)
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[25]].Text = order.EndTime.ToString();//ֹͣʱ�� 25
                else
                    this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[25]].Text = "";//ֹͣʱ�� 25

                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[26]].Text = order.DCOper.ID;
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[27]].Text = order.DCOper.Name;
                this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[36]].Text = order.ApplyNo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("��Fp�����Ϣʱ����" + ex.Message, "��ʾ");
            }
            if (order.SortID == 0)
            {
                order.SortID = MaxSort + 1;//���޸�
                MaxSort = MaxSort + 1;
            }
            else
            {
                if (order.SortID > MaxSort)
                {
                    MaxSort = order.SortID;
                }
            }
            if (order.Frequency.Usage.ID == "") order.Frequency.Usage = order.Usage; //�÷�����
            this.fpSpread1.Sheets[SheetIndex].Cells[i, this.myOrderClass.iColumns[28]].Value = order.SortID;//28
            if (!this.EditGroup)
            {

            }
            
            this.fpSpread1.Sheets[SheetIndex].Rows[i].Tag = order;
            return;
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="order"></param>
        private void ValidNewOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order)
        {
            if (order.ReciptDept.Name == "" && order.ReciptDept.ID != "") order.ReciptDept.Name = this.GetDeptName(order.ReciptDept);
            if (order.StockDept.Name == "" && order.StockDept.ID != "") order.StockDept.Name = this.GetDeptName(order.StockDept);
            if (order.BeginTime == DateTime.MinValue) order.BeginTime = this.OrderManagement.GetDateTimeFromSysDateTime();
            if (order.MOTime == DateTime.MinValue) order.MOTime = order.BeginTime;
            
            if (!this.EditGroup && (order.Patient == null || order.Patient.ID == "")) order.Patient = this.myPatientInfo.Clone();
            if (order.ExeDept == null || order.ExeDept.ID == "")
            {
                //����ִ�п���Ϊ���߿���
                if (!this.EditGroup)
                    order.ExeDept = this.myPatientInfo.PVisit.PatientLocation.Dept.Clone();
                else
                    order.ExeDept = ((Neusoft.HISFC.Models.Base.Employee)this.OrderManagement.Operator).Dept.Clone();
            }
            if (order.ExeDept.Name == "" && order.ExeDept.ID != "")
                order.ExeDept.Name = this.GetDeptName(order.ExeDept);


            if (order.Oper.ID == null || order.Oper.ID == "")
            {
                order.Oper.ID = this.OrderManagement.Operator.ID;
                order.Oper.Name = this.OrderManagement.Operator.Name;
            }
        }
        /// <summary>
        /// ���ҽ��ʵ���FarPoint
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.Inpatient.Order GetObjectFromFarPoint(int i, int SheetIndex)
        {
            return this.myOrderClass.GetObjectFromFarPoint(i, SheetIndex, this.OrderManagement);
        }
        private string GetColumnNameFromIndex(int i)
        {
            return dsAllLong.Tables[0].Columns[i].ColumnName;
        }
       
        /// <summary>
        /// ��ÿ�������
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        private string GetDeptName(Neusoft.FrameWork.Models.NeuObject dept)
        {
            for (int i = 0; i < alDepts.Count; i++)
            {
                Neusoft.FrameWork.Models.NeuObject obj = (Neusoft.FrameWork.Models.NeuObject)alDepts[i];
                if (obj.ID == dept.ID)
                {
                    dept.Name = obj.Name;
                    return dept.Name;
                }
            }
            return "";
        }

        public void SetEditGroup(bool isEdit)
        {
            this.EditGroup = isEdit;
            this.ucItemSelect1.Visible = isEdit;
            if (this.ucItemSelect1 != null)
                this.ucItemSelect1.EditGroup = isEdit;

            this.fpSpread1.Sheets[0].DataSource = null;
            this.fpSpread1.Sheets[1].DataSource = null;
            #region {D17BD9FB-F362-4755-97FE-08404D477C39} ���2�����׹���ť ������ť����Ӧ
            this.fpSpread1.Sheets[0].RowCount = 0;
            this.fpSpread1.Sheets[1].RowCount = 0;
            #endregion
            this.fpSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.fpSpread1.Sheets[1].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
        }

        protected ArrayList GetSelectedRows()
        {
          
            ArrayList rows = new ArrayList();
            
           for (int i = 0; i < this.fpSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.fpSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    rows.Add(i);
                }
            }
            return rows;
        }
        ///<summary>
        /// ˢ�����
        /// </summary>
        public void RefreshCombo()
        {
            try
            {
                /*- 
                 *  Edit By liangjz 2005-10 ������ϵ��ظ�ˢ�� �ڳ�����������ʱ��refreshComboFlag����ֵͬ ����ˢ��
                ---*/
                if (this.refreshComboFlag == "0" || this.refreshComboFlag == "2")
                {
                    try
                    {
                        if (!this.IsDesignMode)
                            this.fpSpread1.Sheets[0].SortRows(this.myOrderClass.iColumns[28], true, true);
                        else
                            this.fpSpread1.Sheets[0].SortRows(this.myOrderClass.iColumns[28], false, true);
                    }
                    catch { }

                    Classes.Function.DrawCombo(this.fpSpread1.Sheets[0], this.myOrderClass.iColumns[4], 8);
                }

                if (this.refreshComboFlag == "1" || this.refreshComboFlag == "2")
                {
                    try
                    {
                        if (!this.IsDesignMode)
                            this.fpSpread1.Sheets[1].SortRows(this.myOrderClass.iColumns[28], true, true);
                        else
                            this.fpSpread1.Sheets[1].SortRows(this.myOrderClass.iColumns[28], false, true);
                    }
                    catch { }

                    Classes.Function.DrawCombo(this.fpSpread1.Sheets[1], this.myOrderClass.iColumns[4], 8);

                }
                //��ֵΪĬ��ֵ
                this.refreshComboFlag = "2";
            }
            catch(Exception ex)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ˢ��ҽ�������Ϣʱ���ֲ���Ԥ֪�������˳������������Ի������Ա��ϵ!\n")+ex.Message);
            }
        }
        /// <summary>
        /// ����ҽ��״̬
        /// </summary>
        public void RefreshOrderState()
        {
            try
            {
                for (int i = 0; i < this.fpSpread1.Sheets[0].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 0, false);
                }
                for (int i = 0; i < this.fpSpread1.Sheets[1].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 1, false);
                }
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ˢ��ҽ��״̬ʱ���ֲ���Ԥ֪�������˳������������Ի������Ա��ϵ"));
            }
        }
        public void RefreshOrderState(bool reset)
        {
            try
            {
                for (int i = 0; i < this.fpSpread1.Sheets[0].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 0, reset);
                }
                for (int i = 0; i < this.fpSpread1.Sheets[1].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 1, reset);
                }
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ˢ��ҽ��״̬ʱ���ֲ���Ԥ֪�������˳������������Ի������Ա��ϵ"));
            }
        }
        public void RefreshIsEmergency()//{C222F7C0-2E51-4084-AEA2-A9F1FA41AC8B}
        {
            for (int j = 0; j < this.fpSpread1.Sheets[0].Rows.Count; j++)
            {
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpSpread1.Sheets[0].Cells[j, 24].Text))
                {
                    this.fpSpread1.Sheets[0].Rows[j].BackColor = Color.GreenYellow;
                }
            }

            for (int j = 0; j < this.fpSpread1.Sheets[1].Rows.Count; j++)
            {
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.fpSpread1.Sheets[1].Cells[j, 24].Text))
                {
                    this.fpSpread1.Sheets[1].Rows[j].BackColor = Color.GreenYellow;
                }
            }

        }
        /// <summary>
        /// ���ҽ���Ϸ���
        /// </summary>
        /// <returns></returns>
        public int CheckOrder()
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            int iCheck = Classes.Function.GetIsOrderCanNoStock();
            bool IsModify = true;
            //{BFDA551D-7569-47dd-85C4-1CA21FE494BD}
            int returnValue = 1;
            //����ҽ��
            for (int i = 0; i < this.fpSpread1.Sheets[0].RowCount; i++)
            {
                order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.Sheets[0].Rows[i].Tag;
                if (order.Status == 0)
                {
                    //δ��˵�ҽ��
                    IsModify = true;
                    
                    if (order.Item.ItemType == EnumItemType.Drug)
                    {
                        //����ҩƷ����Ȩ��ά�� {//{BFDA551D-7569-47dd-85C4-1CA21FE494BD}}
                        if (isCheckPopedom)
                        {
                            returnValue = this.pManagement.CheckPopedom(order.ReciptDoctor.ID, (Neusoft.HISFC.Models.Pharmacy.Item)order.Item);

                            if (returnValue < 0)
                            {
                                MessageBox.Show("��û�п���ҩƷ[" + order.Item.Name + "]��Ȩ��");
                                return -1;
                            }
                        }
                        //ҩƷ
                        if (order.Item.SysClass.ID.ToString() == "PCC")
                        {
                            //�в�ҩ
                            if (order.HerbalQty == 0) { ShowErr("��������Ϊ�㣡", i, 1); return -1; }
                        }
                        else
                        {
                            if (order.DoseOnce == 0) { ShowErr("ÿ�μ�������Ϊ�㣡", i, 0); return -1; }
                            if (order.DoseUnit == "") { ShowErr("������λ����Ϊ�գ�", i, 0); return -1; }
                        }
                        if (order.Frequency==null ||  order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 0); return -1; }
                        if (order.Usage==null || order.Usage.ID == "") { ShowErr("�÷�����Ϊ�գ�", i, 0); return -1; }
                        if (((Neusoft.HISFC.Models.Pharmacy.Item)order.Item).Price == 0)
                        {
                            if (order.OrderType.Name.IndexOf("����") == -1)
                            {
                                ShowErr(order.Item.Name + "�۸�Ϊ�㲻������ȡ��", i, 0); return -1;
                            }
                        }

                    }
                    else
                    {
                        //��ҩƷ
                        if (order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 0); return -1; }
                        if (order.Qty == 0) { ShowErr("��������Ϊ�գ�", i, 0); return -1; }
                        if (order.ExeDept.ID == "") { ShowErr("��ѡ��ִ�п��ң�", i, 0); return -1; }
                        if (order.Item.Price == 0)
                        {
                            if (order.OrderType.Name.IndexOf("����") == -1)
                            {
                                ShowErr(order.Item.Name +"�۸�Ϊ�㲻������ȡ��", i, 0); return -1;
                            }
                        }
                    }
                    if (order.EndTime != DateTime.MinValue)
                    {
                        if (order.EndTime < order.BeginTime)
                        {
                            ShowErr("ֹͣʱ�䲻Ӧ���ڿ�ʼʱ��", i, 0);
                            return -1;
                        }
                    }
                    if (Neusoft.FrameWork.Public.String.ValidMaxLengh(order.Memo, 80) == false)
                    {
                        ShowErr(order.Item.Name + "�ı�ע����!", i, 0);
                        return -1;
                    }
                    if (order.Qty > 5000)
                    { ShowErr("����̫��", i, 0); return -1; }
                    if (order.ID == "") IsModify = true;
                }
            }
            //��ʱҽ��
            for (int i = 0; i < this.fpSpread1.Sheets[1].RowCount; i++)
            {
                order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.Sheets[1].Rows[i].Tag;
                if (order.Status == 0)
                {
                    //δ��˵�ҽ��
                    IsModify = true;
                    
                    if (order.Item.ItemType == EnumItemType.Drug)
                    {
                        //����ҩƷ����Ȩ��ά�� {//{BFDA551D-7569-47dd-85C4-1CA21FE494BD}}
                        if (isCheckPopedom)
                        {
                            returnValue = this.pManagement.CheckPopedom(order.ReciptDoctor.ID, (Neusoft.HISFC.Models.Pharmacy.Item)order.Item);
                            //{364E4098-0B5A-494c-9991-5DA77B93527D} û�ҵ�Ϊ0��ά��ʱֻά�������û�ҵ��������ã��ҵ�����û��Ȩ��
                            if (returnValue < 0)
                            {
                                MessageBox.Show("��ȡ[" + order.Item.Name + "]��Ȩ��ʱ����");                                
                                return -1;
                            }
                            else if (returnValue > 0)
                            {
                                MessageBox.Show("��û�п���ҩƷ[" + order.Item.Name + "]��Ȩ�ޣ�");
                                return -1;
                            }
                        }
                        //ҩƷ
                        if (order.Item.SysClass.ID.ToString() == "PCC")
                        {
                            //�в�ҩ
                            if (order.HerbalQty == 0) { ShowErr("��������Ϊ�㣡", i, 1); return -1; }
                        }
                        else
                        {
                            //����
                            if (order.DoseOnce == 0) { ShowErr("ÿ�μ�������Ϊ�㣡", i, 1); return -1; }
                            if (order.DoseUnit == "") { ShowErr("������λ����Ϊ�գ�", i, 1); return -1; }
                        }
                        if (order.Qty == 0) { ShowErr("��������Ϊ�գ�", i, 1); return -1; }
                        if (order.Unit == "") { ShowErr("��λ����Ϊ�գ�", i, 1); return -1; }
                        if (order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 1); return -1; }
                        if (order.Usage.ID == "") { ShowErr("�÷�����Ϊ�գ�", i, 1); return -1; }
                        //�����(����ҽ������)
                        if (order.OrderType.IsCharge)
                        {
                            if (order.StockDept != null && order.StockDept.ID != "")
                            {
                                Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmacyIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
                                decimal storeNum = order.Qty;
                                if (pharmacyIntegrate.GetStorageNum(order.StockDept.ID, order.Item.ID, out storeNum) == 1)
                                {
                                    if (order.Qty > storeNum)
                                    {
                                        if (iCheck == 1)
                                        {
                                            if (MessageBox.Show("ҩƷ��" + order.Item.Name + "���Ŀ�治�����Ƿ����ִ�У�", "��ʾ��治��", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                            {
                                                return -1;
                                            }
                                        }
                                        else
                                        {
                                            ShowErr(order.Item.Name + "��治��!", i, 0);
                                            {
                                                return -1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ShowErr(order.Item.Name + "����ж�ʧ��!" + pharmacyIntegrate.Err, i, 0);
                                    return -1;
                                }
                            }
                            else
                            {
                                if (Classes.Function.CheckPharmercyItemStock(iCheck, order.Item.ID, order.Item.Name, order.ReciptDept.ID, order.Qty) == false)
                                {
                                    ShowErr(order.Item.Name + "��治��!", i, 1); return -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        //��ҩƷ
                        if (order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 1); return -1; }
                        if (order.Qty == 0) { ShowErr("��������Ϊ�գ�", i, 1); return -1; }
                        if (order.ExeDept.ID == "") { ShowErr("��ѡ��ִ�п��ң�", i, 0); return -1; }
                    }
                    if (Neusoft.FrameWork.Public.String.ValidMaxLengh(order.Memo, 80) == false)
                    {
                        ShowErr(order.Item.Name + "�ı�ע����!", i, 0);
                        return -1;
                    }
                    if (order.Qty > 5000)
                    { ShowErr("����̫��", i, 1); return -1; }
                    if (order.ID == "") IsModify = true;
                }
            }            

            if (IsModify == false) return -1;//δ����¼���ҽ��

            return 0;

        }

        /// <summary>
        /// ��鿪����Ϣ����ʾ����
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="iRow"></param>
        /// <param name="SheetIndex"></param>
        private void ShowErr(string strMsg, int iRow, int SheetIndex)
        {
            this.fpSpread1.ActiveSheetIndex = SheetIndex;
            this.fpSpread1.Sheets[SheetIndex].ClearSelection();
            this.fpSpread1.Sheets[SheetIndex].ActiveRowIndex = iRow;
            SelectionChanged();
            this.fpSpread1.Sheets[SheetIndex].AddSelection(iRow, 0, 1, 1);
            MessageBox.Show(strMsg);
        }

        /// <summary>
        /// ��ѯҽ��
        /// </summary>
        private void QueryOrder()
        {
            try
            {
                this.fpSpread1.Sheets[0].RowCount = 0;
                this.fpSpread1.Sheets[1].RowCount = 0;
                if (this.dsAllLong != null && this.dsAllLong.Tables[0].Rows.Count > 0)
                    this.dsAllLong.Tables[0].Rows.Clear();
                if (this.dsAllShort != null && this.dsAllShort.Tables[0].Rows.Count > 0)
                    this.dsAllShort.Tables[0].Rows.Clear();
            }
            catch
            {
                //			    MessageBox.Show ("���ҽ����¼��Ϣ����","��ʾ");
            }
            if (this.myPatientInfo == null)
            {
                return;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯҽ��,���Ժ�!");
            Application.DoEvents();

            //��ѯ����ҽ������
            ArrayList alTemp = OrderManagement.QueryOrder(this.myPatientInfo.ID);
            //���˵�����ҽ��  {FB86E7D8-A148-4147-B729-FD0348A3D670}
            ArrayList al = new ArrayList();
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in alTemp)
            {
                if (info.Status == 4)
                {
                    continue;
                }

                al.Add(info);
            }

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾҽ��,���Ժ�!");
            Application.DoEvents();
            if (this.IsDesignMode)
            {
                tooltip.SetToolTip(this.fpSpread1, "����ʱ����ҽ��ֻ��ʾ��Ч�ģ���ʱҽ��ֻ��ʾ24Сʱ�ڵ�ҽ����");
                tooltip.Active = true;
                try
                {
                    this.fpSpread1.Sheets[0].DataSource = null;
                    this.fpSpread1.Sheets[1].DataSource = null;
                    #region addby xuewj 2010-9-27 ÿ������������λС�� {5406131E-B1F9-497b-BD03-FF202645C29F}
                    ((FarPoint.Win.Spread.CellType.NumberCellType)this.fpSpread1.Sheets[0].Columns[11].CellType).DecimalPlaces = 3;
                    ((FarPoint.Win.Spread.CellType.NumberCellType)this.fpSpread1.Sheets[1].Columns[11].CellType).DecimalPlaces = 3; 
                    #endregion

                    this.AddObjectsToFarpoint(al);
                    this.fpSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
                    this.fpSpread1.Sheets[1].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;

                    

                    this.RefreshCombo();
                    this.RefreshOrderState();
                    this.fpSpread1.Sheets[1].DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));

                }
                catch (Exception ex)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                tooltip.SetToolTip(this.fpSpread1, "");
                try
                {
                    this.AddObjectsToTable(al);
                    dvLong = new DataView(dsAllLong.Tables[0]);
                    dvShort = new DataView(dsAllShort.Tables[0]);
                    this.fpSpread1.Sheets[0].DataSource = dvLong;
                    this.fpSpread1.Sheets[1].DataSource = dvShort;                    
                    this.fpSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
                    this.fpSpread1.Sheets[1].OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;
                    //CheckSortID();//���˳���
                   
                    this.RefreshCombo();
                    this.RefreshOrderState();
                    this.RefreshIsEmergency();//{C222F7C0-2E51-4084-AEA2-A9F1FA41AC8B}
                    SetTip(0);
                    SetTip(1);
                }
                catch (Exception ex)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(ex.Message);
                }
            }

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

        }
        /// <summary>
        /// ���ñ�ע��Ƥ��
        /// </summary>
        /// <param name="k"></param>
        private void SetTip(int k)
        {
            for (int i = 0; i < this.fpSpread1.Sheets[k].RowCount; i++)//��ע
            {
                try
                {
                    if (this.fpSpread1.Sheets[k].Cells[i, this.myOrderClass.iColumns[35]].Value != null)
                    {
                        if (this.fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[35]].Value.ToString() != "")
                        {
                            fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Note = this.fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[35]].Value.ToString();//��ע
                        }
                    }
                }
                catch { }

                #region {1AF0EB93-27A8-462f-9A1E-E1A3ECA54ADE} �ӹ�ϣ���в�����Ϣ������ٶ�
                int hypotest = 0;
                if (this.myOrderClass.HtOrder != null && this.myOrderClass.HtOrder.ContainsKey(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[2]].Text))
                {
                    hypotest = (this.myOrderClass.HtOrder[fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[2]].Text] as Neusoft.HISFC.Models.Order.Inpatient.Order).HypoTest;
                }
                else
                {
                    hypotest = this.OrderManagement.QueryOrderHypotest(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[2]].Text); //Ƥ�� 
                }
                //int hypotest = this.OrderManagement.QueryOrderHypotest(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[2]].Text); //Ƥ�� 
                #endregion

                string sTip = "(��Ƥ��)";
                try
                {
                    if (fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length > 6)
                    {
                        if (fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Substring(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length - 3) == "�ۣ���"
                            || fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Substring(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length - 3) == "�ۣ���")
                            fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text = fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Substring(0, fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length - 3);

                        if (fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Substring(fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length - sTip.Length, sTip.Length) == sTip)
                            fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text = fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Substring(0, fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text.Length - sTip.Length);
                    }
                }
                catch { }
                fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].ForeColor = Color.Black;
                if (hypotest == 3)
                {
                    fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text += "�ۣ���";//Ƥ��
                    fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].ForeColor = Color.Red;
                }
                else if (hypotest == 4)
                {
                    fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text += "�ۣ���";
                }
                else if (hypotest == 2)
                {
                    fpSpread1.Sheets[k].Cells[i, myOrderClass.iColumns[6]].Text += sTip;
                }
            }
        }
        protected override int OnQuery(object sender, object neuObject)
        {
            this.QueryOrder();
            return 0;
        }
        /// <summary>
        /// ����ҽ����ʾ
        /// 0 All,1���� 2����Ч��3 ��Ч��4 δ���
        /// </summary>
        /// <param name="State"></param>
        public void Filter(EnumFilterList State)
        {
            if (this.bIsDesignMode) return;
            if (this.myPatientInfo == null) return;

            try
            {
                if(this.fpSpread1.ActiveSheetIndex == 0)
                    dvLong.RowFilter = "1=2";
                else
                    dvShort.RowFilter = "1=2";
                //��ѯʱ����ܹ���
                switch (State.GetHashCode())
                {
                    case 0://ȫ��
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "";
                        else
                            dvShort.RowFilter = "";
                        break;
                    case 1://����
                        DateTime dt = OrderManagement.GetDateTimeFromSysDateTime();
                        DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                        DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "����ʱ�� >='" + dt1.ToString() + "' and ����ʱ��<='" + dt2.ToString() + "'";
                        else
                            dvShort.RowFilter = "����ʱ�� >='" + dt1.ToString() + "' and ����ʱ��<='" + dt2.ToString() + "'";
                        break;
                    case 2://��Ч
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "ҽ��״̬ ='1' or ҽ��״̬='2'";
                        else
                            dvShort.RowFilter = "ҽ��״̬ ='1' or ҽ��״̬='2'";
                        break;
                    case 3://��Ч
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "ҽ��״̬ = '3'";
                        else
                            dvShort.RowFilter = "ҽ��״̬ = '3'";
                        break;
                    case 4://δ���
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "ҽ��״̬ = '0'";
                        else
                            dvShort.RowFilter = "ҽ��״̬ = '0'";
                        break;
                    default:
                        if (this.fpSpread1.ActiveSheetIndex == 0)
                            dvLong.RowFilter = "";
                        else
                            dvShort.RowFilter = "";
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.RefreshOrderState();
            this.RefreshCombo();//{CCB7A55B-38A3-4ccd-A5C5-F293D5F77913} ע���˸�ɶ���Ƿ����������BUG��
        
        }

        /// <summary>
        /// ��ʱ��Ϣ{7882B4CC-FA22-4530-9E5E-2E738DF1DEEC}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        protected override void OnSendMessage(object sender, string msg)
        {
            #region {839D3A8A-49FA-4d47-A022-6196EB1A5715}
            //��Ϣ����
            msg = "���ߣ�" + this.myPatientInfo.Name + "��ҽ�������仯��������ʱ����\nסԺ�ţ�" + this.myPatientInfo.ID + "\n���ţ�" + this.myPatientInfo.PVisit.PatientLocation.Bed.ID;
            ////��Ϣ����
            //msg = "���ߣ�" + this.myPatientInfo.Name + "��ҽ�������仯��������ʱ����";
            #endregion
            //����
            //Neusoft.FrameWork.Models.NeuObject targetDept = this.myReciptDept;
            Neusoft.FrameWork.Models.NeuObject targetDept = this.myPatientInfo.PVisit.PatientLocation.Dept.Clone();//{06067649-CCAE-4379-A105-65C617029533}

            base.OnSendMessage(targetDept, msg);
        }

        /// <summary>
        /// ��ʼ���������뵥�ӿ�
        /// {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
        /// </summary>
        private int InitPACSApplyInterface()
        {
            try
            {
                PACSApplyInterface = new Neusoft.ApplyInterface.HisInterface();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        #region У��ҽ������ �Ƿ�֪��ͬ�⡢���顢У����
        public int JudgeOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Order.Inpatient.Order order)
        {
            if (patient == null) return -1;
            if (order == null) return -1;
            //�Ƿ����������״̬�¿���ҩƷ
            int iCheck = Classes.Function.GetIsOrderCanNoStock();
            try
            {
                Neusoft.HISFC.Models.Base.Item tempItem = (order.Item) as Neusoft.HISFC.Models.Base.Item;
                Neusoft.HISFC.Models.Pharmacy.Item tempPharmacy = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                int iFlag = -1;
                if (tempItem == null)
                {
                    MessageBox.Show("ҽ����ϸ��Ŀ����ת������");
                    return -1;
                }
                //�����
                //if (order.Item.IsPharmacy && order.OrderType.IsCharge)
                if (order.Item.ItemType == EnumItemType.Drug && order.OrderType.IsCharge)
                {
                    if (Classes.Function.CheckPharmercyItemStock(iCheck, order.Item.ID, order.Item.Name, this.GetReciptDept().ID, order.Qty) == false)
                    {
                        MessageBox.Show(order.Item.Name + "��治��!");
                        return -1;
                    }
                }
                //�ж�ҽ��֪��ͬ��  ֻ���շѵ�ҽ�����ͽ���֪��ͬ���ж�
                if (order.OrderType.IsCharge)
                {
                    //iFlag = Classes.Function.IsCanOrder(patient, tempItem);
                }
                if (iFlag == 0) return -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("��Ŀת������" + ex.Message, "��ʾ");
            }
            return 1;

        }
        #endregion
        #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

        /// <summary>
        /// ��ʼ��IReasonableMedicin
        /// </summary>
        private void InitReasonableMedicine()
        {
            if (this.IReasonableMedicine == null)
            {
                this.IReasonableMedicine = Neusoft.FrameWork.WinForms.Classes.UtilInterface .CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine)) as Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine;
            }
        }

        #endregion
        #region add by xuewj ����ҽ������ {1F2B9330-7A32-4da4-8D60-3A4568A2D1D8}

        /// <summary>
        /// ����ҽ������
        /// </summary>
        public void AddAssayCure()
        {
            if (this.fpSpread1.ActiveSheetIndex == 1 && this.fpSpread1.ActiveSheet.ActiveRowIndex > -1)
            {
                ArrayList alOrder = this.GetSelectedOrders();
                if (alOrder == null)
                {
                    MessageBox.Show("ȡҽ������!");
                    return;
                }
                ucAssayCure uc = new ucAssayCure();
                uc.Orders = alOrder;
                uc.MakeSuccessed += new ucAssayCure.MakeSuccessedHandler(uc_MakeSuccessed);
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "����ҽ������";
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
            }
        }

        /// <summary>
        /// ȡ��ʱҽ��ҳ�е�ҽ����Ŀ,�������ƴ���
        /// </summary>
        /// <returns>null��0ʧ��</returns>
        private ArrayList GetSelectedOrders()
        {
            if (this.fpSpread1.ActiveSheetIndex == 0)//����
            {
                return null;
            }

            ArrayList alOrders = new ArrayList();
            Neusoft.HISFC.Models.Order.Inpatient.Order tempOrder = null;
            for (int i = this.fpSpread1.Sheets[this.fpSpread1.ActiveSheetIndex].RowCount - 1; i > -1; i--)//ֻ������ʱҽ��,����ǳ���,�����ȸ��Ƶ�����
            {
                if (this.fpSpread1.Sheets[this.fpSpread1.ActiveSheetIndex].IsSelected(i, 0))
                {
                    tempOrder = this.GetObjectFromFarPoint(i, 1).Clone();//��ʱҽ��
                    if (tempOrder != null)
                    {
                        if ((tempOrder.Status == 0 || tempOrder.Status == 5)
                            && tempOrder.Item.ItemType == EnumItemType.Drug)//�¿�����ҩƷҽ��
                        {
                            alOrders.Add(tempOrder);
                        }
                    }
                }
            }

            return alOrders;
        }

        /// <summary>
        /// ���ɻ���ҽ��
        /// </summary>
        /// <param name="alOrders"></param>
        private void uc_MakeSuccessed(ArrayList alOrders)
        {
            this.needUpdateDTBegin = false;
            Delete(this.fpSpread1.Sheets[this.fpSpread1.ActiveSheetIndex].ActiveRowIndex, true);//{0AAB51FC-0258-48e7-B3E5-1721F7C53474}
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order orderInfo in alOrders)
            {
                //this.ucItemSelect1.OrderType = orderType;
                //Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();
                //item.Qty = 1.0M;
                //item.PriceUnit = "��";
                //item.ID = "999";//�Զ���
                //item.SysClass.ID = "M";
                //item.Name = orderName + "ҽ��";
                //this.ucItemSelect1.FeeItem = item;
                this.AddNewOrder(orderInfo.Clone(), this.fpSpread1.ActiveSheetIndex);
            }
            this.needUpdateDTBegin = true;
            this.RefreshCombo();
        }

        #endregion

        /// <summary>
        /// ֹͣСʱ�շ�ҽ�� {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
        /// </summary>
        /// <param name="orderTerm"></param>
        /// <returns></returns>
        protected virtual int DCHoursOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, Neusoft.HISFC.BizProcess.Integrate.Order orderIntergrate, IDbTransaction trans)
        {
            int iReturn = 0;
            if (order.Frequency.ID == this.hoursFrequencyID)
            {
                Neusoft.FrameWork.Models.NeuObject nurseStation = ((Neusoft.HISFC.Models.Base.Employee)this.OrderManagement.Operator).Nurse.Clone();
                //ArrayList alMyOrder = orderIntergrate.QueryOrderAndSubtblByOrderTermID(order.ID);
                ArrayList alMyOrder = this.OrderManagement.QuerySubtbl(order.Combo.ID);
                alMyOrder.Add(order);
                ArrayList alNeedFeeExecOrderDrug = new ArrayList();
                ArrayList alNeedFeeExecOrderUnDrug = new ArrayList();
                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order objOrder in alMyOrder)
                {
                    iReturn = this.OrderManagement.DecomposeOrderToNow(objOrder, 0, false, order.EndTime);
                    if (iReturn < 0)
                    {
                        return iReturn;
                    }
                    ArrayList alTmp = new ArrayList();
                    if (objOrder.Item.ItemType == EnumItemType.Drug)
                    {
                        alTmp = this.OrderManagement.QueryUnFeeExecOrderByOrderID(this.myPatientInfo.ID, "1", objOrder.ID, order.NextMOTime, order.EndTime);
                        if (alTmp.Count > 0)
                        {
                            alNeedFeeExecOrderDrug.AddRange(alTmp);
                        }
                    }
                    else
                    {
                        alTmp = this.OrderManagement.QueryUnFeeExecOrderByOrderID(this.myPatientInfo.ID, "2", objOrder.ID, order.NextMOTime, order.EndTime);
                        if (alTmp.Count > 0)
                        {
                            alNeedFeeExecOrderUnDrug.AddRange(alTmp);
                        }
                    }

                }
                if (alNeedFeeExecOrderDrug.Count > 0)
                {
                    List<Neusoft.HISFC.Models.Order.ExecOrder> listFeeOrder = new List<Neusoft.HISFC.Models.Order.ExecOrder>();
                    foreach (Neusoft.HISFC.Models.Order.ExecOrder obj in alNeedFeeExecOrderDrug)
                    {
                        listFeeOrder.Add(obj);
                    }
                    iReturn = orderIntergrate.ComfirmExec(this.myPatientInfo, listFeeOrder, nurseStation.ID, order.EndTime, true);
                    if (iReturn < 0)
                    {
                        if (MessageBox.Show("ȷ��ִ��ҽ�������Ƿ������\n" + order.Item.Name + " : " + orderIntergrate.Err, "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            return iReturn;
                        }
                    }
                }
                if (alNeedFeeExecOrderUnDrug.Count > 0)
                {
                    List<Neusoft.HISFC.Models.Order.ExecOrder> listFeeOrder = new List<Neusoft.HISFC.Models.Order.ExecOrder>();
                    foreach (Neusoft.HISFC.Models.Order.ExecOrder obj in alNeedFeeExecOrderUnDrug)
                    {
                        listFeeOrder.Add(obj);
                    }
                    iReturn = orderIntergrate.ComfirmExec(this.myPatientInfo, listFeeOrder, nurseStation.ID, order.EndTime, false);
                    if (iReturn < 0)
                    {
                        if (MessageBox.Show("ȷ��ִ��ҽ�������Ƿ������\n" + order.Item.Name + " : " + orderIntergrate.Err, "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            return iReturn;
                        }
                    }
                }
            }
            return 1;
        }
        
        #endregion

        #region �˵�
        /// <summary>
        /// ȥ��ǰ��ҽ����TempID
        /// </summary>
        /// <returns></returns>
        public string ActiveTempID
        {
            get
            {
                //return this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[37]].Text;
                return this.fpSpread1.ActiveSheet.ActiveRowIndex.ToString();
            }
        }
        int ActiveRowIndex = -1;
        /// <summary>
        /// Ϊ�Ҽ���Ӳ˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.bIsShowPopMenu && e.Button == MouseButtons.Right  )
            {
                //if (this.fpSpread1.ActiveSheet.RowCount <= 0) return;//{7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
                try
                {
                    this.contextMenu1.Items.Clear();
                    //Pass.Pass.ShowFloatWin(false);
                }
                catch { }

                #region {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
                List<string> orderIdList = Classes.HistoryOrderClipboard.OrderList;
                if ((orderIdList == null) || (orderIdList.Count <= 0))
                {
                }
                else
                {
                    if (this.bIsDesignMode) //������
                    {
                        if (this.EditGroup == false)//������ģʽ
                        {
                            #region ճ��ҽ��
                            ToolStripMenuItem mnuPasteOrder = new ToolStripMenuItem("ճ��ҽ��");
                            mnuPasteOrder.Click += new EventHandler(mnuPasteOrder_Click);
                            this.contextMenu1.Items.Add(mnuPasteOrder);
                            this.contextMenu1.Show(this.fpSpread1, new Point(e.X, e.Y));
                            #endregion
                        }
                    }
                }
                if (this.fpSpread1.ActiveSheet.RowCount <= 0) return;
                #endregion

                FarPoint.Win.Spread.Model.CellRange c = fpSpread1.GetCellFromPixel(0, 0, e.X, e.Y);
                if (c.Row >= 0)
                {
                    this.fpSpread1.ActiveSheet.ActiveRowIndex = c.Row;
                    this.fpSpread1.ActiveSheet.AddSelection(c.Row, 0, 1, 1);
                    ActiveRowIndex = c.Row;
                }
                
                if (ActiveRowIndex < 0)
                {
                    return;
                }
                Neusoft.HISFC.Models.Order.Inpatient.Order mnuSelectedOrder = null;
                mnuSelectedOrder = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[ActiveRowIndex].Tag;

                if (this.bIsDesignMode) //������
                {
                    if (this.EditGroup == false)//������ģʽ
                    {
                        #region ֹͣ�˵�
                        ToolStripMenuItem mnuDel = new ToolStripMenuItem();//ֹͣ
                        mnuDel.Click += new EventHandler(mnuDel_Click);
                        ToolStripMenuItem mnuCancel = new ToolStripMenuItem();//ȡ��
                        mnuCancel.Click += new EventHandler(mnuCancel_Click);

                        if (mnuSelectedOrder.Status == 0 || mnuSelectedOrder.Status == 5)
                        {
                            //����
                            mnuDel.Text = "ɾ��ҽ��{" + mnuSelectedOrder.Item.Name + "]";
                            this.contextMenu1.Items.Add(mnuDel);//ɾ��������
                        }
                        else
                        {
                            if (mnuSelectedOrder.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)
                            {
                                if (mnuSelectedOrder.Status != 3)
                                {
                                    mnuDel.Text = "ֹͣҽ��[" + mnuSelectedOrder.Item.Name + "]";
                                    this.contextMenu1.Items.Add(mnuDel);//ɾ��������

                                    mnuCancel.Text = "ȡ��ҽ��[" + mnuSelectedOrder.Item.Name + "]";
                                    this.contextMenu1.Items.Add(mnuCancel);//ȡ��
                                }
                            }
                            else
                            {
                                //Edit By liangjz  ��������ִ�еĿ�������ҽ��
                                if (mnuSelectedOrder.Status == 1 || mnuSelectedOrder.Status == 2)
                                {
                                    mnuDel.Text = "����ҽ��[" + mnuSelectedOrder.Item.Name + "]";
                                    this.contextMenu1.Items.Add(mnuDel);//ɾ��������
                                }
                            }

                        }

                        #endregion
                    }
                    #region ����ҽ��
                    ToolStripMenuItem mnuCopy = new ToolStripMenuItem();//����ҽ��Ϊ��һ������
                    mnuCopy.Click += new EventHandler(mnuCopy_Click);
                    if (this.OrderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                    {
                        mnuCopy.Text = "����" + "[" + mnuSelectedOrder.Item.Name + "]" + "Ϊ��ʱҽ��";
                    }
                    else
                    {
                        mnuCopy.Text = "����" + "[" + mnuSelectedOrder.Item.Name + "]" + "Ϊ����ҽ��";
                    }
                    this.contextMenu1.Items.Add(mnuCopy);

                    ToolStripMenuItem mnuCopyAs = new ToolStripMenuItem();//����ҽ��Ϊ������
                    mnuCopyAs.Click += new EventHandler(mnuCopyAs_Click);
                    if (this.OrderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                    {
                        mnuCopyAs.Text = "����" + "[" + mnuSelectedOrder.Item.Name + "]" + "Ϊ����ҽ��";
                    }
                    else
                    {
                        mnuCopyAs.Text = "����" + "[" + mnuSelectedOrder.Item.Name + "]" + "Ϊ��ʱҽ��";
                    }
                    this.contextMenu1.Items.Add(mnuCopyAs);
                    #endregion

                    if (this.EditGroup == false)//������ģʽ
                    {
                        #region ҽ�������޸�
                        if (mnuSelectedOrder.Status == 0)
                        {
                            ToolStripMenuItem menuChange = new ToolStripMenuItem();
                            menuChange.Click += new EventHandler(menuChange_Click);
                            menuChange.Text = "�޸�" + "[" + mnuSelectedOrder.Item.Name + "]ҽ������";
                            if (mnuSelectedOrder.Item.Price == 0)
                                menuChange.Enabled = false;
                            else
                                menuChange.Enabled = true;
                            //this.contextMenu1.Items.Add(menuChange);
                        }
                        #endregion

                    }

                    if (this.EditGroup == false)//������ģʽ
                    {
                        #region ҽ������¼�ҽ������ҽ��
                        if (mnuSelectedOrder.Status == 5)
                        {
                            ToolStripMenuItem menuCheckOrder = new ToolStripMenuItem();
                            menuCheckOrder.Click += new EventHandler(menuCheckOrder_Click);
                            menuCheckOrder.Text = "���ҽ��";

                            this.contextMenu1.Items.Add(menuCheckOrder);
                        }
                        #endregion
                    }
                    //{D2BDB9B8-7D50-4a66-8D1C-28EA0420592F} 
                    if (this.EditGroup == false)
                    {
                        if (mnuSelectedOrder.Item.SysClass.ID.ToString() == "UC")
                        {
                            //ToolStripMenuItem checkSlip = new ToolStripMenuItem();
                            //checkSlip.Click += new  EventHandler(checkSlip_Click);
                            //checkSlip.Text = "������뵥";
                            //this.contextMenu1.Items.Add(checkSlip);
                            //ToolStripMenuItem cancelSlip = new ToolStripMenuItem();
                            //cancelSlip.Click+=new EventHandler(cancelSlip_Click);
                            //cancelSlip.Text = "ɾ�����뵥��Ϣ";
                            //this.contextMenu1.Items.Add(cancelSlip);
                        }

                        #region �ش�������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
                        if (this.isUsePACSApplySheet)
                        {
                            if (mnuSelectedOrder.ApplyNo != null && mnuSelectedOrder.ApplyNo != "")
                            {
                                //ToolStripMenuItem mnuPACSApply = new ToolStripMenuItem("�ش�������뵥");//���ƶ�
                                //mnuPACSApply.Click += new EventHandler(mnuPACSApply_Click);
                                //this.contextMenu1.Items.Add(mnuPACSApply);
                            }
                        }
                        #endregion

                    }
                    //{D2BDB9B8-7D50-4a66-8D1C-28EA0420592F}
                    #region ����
                    ToolStripMenuItem mnuUp = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuUp.Click += new EventHandler(mnuUp_Click);
                    if (this.fpSpread1.ActiveSheet.ActiveRowIndex <= 0) mnuUp.Enabled = false;
                    this.contextMenu1.Items.Add(mnuUp);
                    #endregion

                    #region ����
                    ToolStripMenuItem mnuDown = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuDown.Click += new EventHandler(mnuDown_Click);
                    if (this.fpSpread1.ActiveSheet.ActiveRowIndex >= this.fpSpread1.ActiveSheet.RowCount - 1 || this.fpSpread1.ActiveSheet.ActiveRowIndex < 0) mnuDown.Enabled = false;
                    this.contextMenu1.Items.Add(mnuDown);
                    #endregion
                    #region ������{C6E229AC-A1C4-4725-BBBB-4837E869754E}

                    ToolStripMenuItem mnuSaveGroup = new ToolStripMenuItem("������");//������
                    mnuSaveGroup.Click += new EventHandler(mnuSaveGroup_Click);

                    this.contextMenu1.Items.Add(mnuSaveGroup);
                    #endregion
                    #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ��Ӻ�����ҩ�Ҽ��˵�

                    if (this.IReasonableMedicine != null && this.EnabledPass && this.IReasonableMedicine.PassEnabled)
                    {
                        int iSheetIndex = this.OrderType == Neusoft.HISFC.Models.Order.EnumType.SHORT ? 1 : 0;
                        int iRow = this.fpSpread1.Sheets[iSheetIndex].ActiveRowIndex;
                        Neusoft.HISFC.Models.Order.Inpatient.Order info = this.GetObjectFromFarPoint(iRow, iSheetIndex);
                        if (info == null)
                        {
                            this.contextMenu1.Show(this.fpSpread1, new Point(e.X, e.Y));
                            return;
                        }
                        if (info.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                        {
                            this.IReasonableMedicine.ShowFloatWin(false);
                            this.contextMenu1.Show(this.fpSpread1, new Point(e.X, e.Y));
                            return;
                        }
                        this.IReasonableMedicine.ShowFloatWin(false);
                        this.IReasonableMedicine.PassSetDrug(info.Item.ID, info.Item.Name, ((Neusoft.HISFC.Models.Pharmacy.Item)info.Item).DoseUnit,
                            info.Usage.Name);

                        int i = 0;
                        ToolStripMenuItem menuPass = new ToolStripMenuItem("������ҩ");
                        this.contextMenu1.Items.Add(menuPass);

                        ToolStripMenuItem m_al1ergic = new ToolStripMenuItem("����ʷ/����״̬");
                        m_al1ergic.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_al1ergic);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("22") == 0)
                        {
                            m_al1ergic.Enabled = false;
                        }

                        ToolStripMenuItem m_cpr = new ToolStripMenuItem("ҩ���ٴ���Ϣ�ο�");
                        m_cpr.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_cpr);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("101") == 0)
                        {
                            m_cpr.Enabled = false;
                        }

                        ToolStripMenuItem m_directions = new ToolStripMenuItem("ҩƷ˵����");
                        m_directions.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_directions);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("102") == 0)
                        {
                            m_directions.Enabled = false;
                        }

                        ToolStripMenuItem m_chp = new ToolStripMenuItem("�й�ҩ��");
                        m_chp.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_chp);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("107") == 0)
                        {
                            m_chp.Enabled = false;
                        }

                        ToolStripMenuItem m_cpe = new ToolStripMenuItem("������ҩ����");
                        m_cpe.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_cpe);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("103") == 0)
                        {
                            m_cpe.Enabled = false;
                        }

                        ToolStripMenuItem m_checkres = new ToolStripMenuItem("ҩ�����ֵ");
                        m_checkres.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_checkres);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("104") == 0)
                        {
                            m_checkres.Enabled = false;
                        }

                        ToolStripMenuItem m_lmim = new ToolStripMenuItem("�ٴ�������Ϣ�ο�");
                        m_lmim.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_lmim);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("220") == 0)
                        {
                            m_lmim.Enabled = false;
                        }

                        ToolStripMenuItem menuAllergn = new ToolStripMenuItem("-");
                        menuAllergn.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, menuAllergn);
                        i++;

                        #region ҩƷר����Ϣ

                        ToolStripMenuItem menuSpecialInfo = new ToolStripMenuItem("ר����Ϣ");
                        menuPass.DropDownItems.Insert(i, menuSpecialInfo);
                        i++;
                        int j = 0;

                        ToolStripMenuItem m_ddim = new ToolStripMenuItem("ҩ��-ҩ���໥����");
                        menuSpecialInfo.DropDownItems.Insert(j, m_ddim);
                        m_ddim.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("201") == 0)
                        {
                            m_ddim.Enabled = false;
                        }

                        ToolStripMenuItem m_dfim = new ToolStripMenuItem("ҩ��-ʳ���໥����");
                        menuSpecialInfo.DropDownItems.Insert(j, m_dfim);
                        m_dfim.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("202") == 0)
                        {
                            m_dfim.Enabled = false;
                        }

                        ToolStripMenuItem m_line7 = new ToolStripMenuItem("-");
                        menuSpecialInfo.DropDownItems.Insert(j, m_line7);
                        j++;

                        ToolStripMenuItem m_matchres = new ToolStripMenuItem("����ע�����������");
                        menuSpecialInfo.DropDownItems.Insert(j, m_matchres);
                        m_matchres.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("203") == 0)
                        {
                            m_matchres.Enabled = false;
                        }

                        ToolStripMenuItem m_trisselres = new ToolStripMenuItem("����ע�����������");
                        menuSpecialInfo.DropDownItems.Insert(j, m_trisselres);
                        m_trisselres.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("204") == 0)
                        {
                            m_trisselres.Enabled = false;
                        }

                        ToolStripMenuItem m_line8 = new ToolStripMenuItem("-");
                        menuSpecialInfo.DropDownItems.Insert(j, m_line8);
                        j++;

                        ToolStripMenuItem m_ddcm = new ToolStripMenuItem("����֢");
                        menuSpecialInfo.DropDownItems.Insert(j, m_ddcm);
                        m_ddcm.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("205") == 0)
                        {
                            m_ddcm.Enabled = false;
                        }
                        ToolStripMenuItem m_side = new ToolStripMenuItem("������");
                        menuSpecialInfo.DropDownItems.Insert(j, m_side);
                        m_side.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("206") == 0)
                        {
                            m_side.Enabled = false;
                        }

                        ToolStripMenuItem m_line9 = new ToolStripMenuItem("-");
                        menuSpecialInfo.DropDownItems.Insert(j, m_line9);
                        j++;

                        ToolStripMenuItem m_geri = new ToolStripMenuItem("��������ҩ");
                        menuSpecialInfo.DropDownItems.Insert(j, m_geri);
                        m_geri.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("207") == 0)
                        {
                            m_geri.Enabled = false;
                        }
                        ToolStripMenuItem m_pedi = new ToolStripMenuItem("��ͯ��ҩ");
                        menuSpecialInfo.DropDownItems.Insert(j, m_pedi);
                        m_pedi.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("208") == 0)
                        {
                            m_pedi.Enabled = false;
                        }
                        ToolStripMenuItem m_preg = new ToolStripMenuItem("��������ҩ");
                        menuSpecialInfo.DropDownItems.Insert(j, m_preg);
                        m_preg.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("209") == 0)
                        {
                            m_preg.Enabled = false;
                        }

                        ToolStripMenuItem m_lact = new ToolStripMenuItem("��������ҩ");
                        menuSpecialInfo.DropDownItems.Insert(j, m_lact);
                        m_lact.Click += new EventHandler(mnuPass_Click);
                        j++;
                        if (this.IReasonableMedicine.PassGetStateIn("210") == 0)
                        {
                            m_lact.Enabled = false;
                        }

                        #endregion

                        ToolStripMenuItem m_line2 = new ToolStripMenuItem("-");
                        menuPass.DropDownItems.Insert(i, m_line2);
                        i++;

                        ToolStripMenuItem m_centerinfo = new ToolStripMenuItem("ҽҩ��Ϣ����");
                        m_centerinfo.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_centerinfo);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("106") == 0)
                        {
                            m_centerinfo.Enabled = false;
                        }

                        ToolStripMenuItem m_line3 = new ToolStripMenuItem("-");
                        menuPass.DropDownItems.Insert(i, m_line3);
                        i++;

                        ToolStripMenuItem menuDrug = new ToolStripMenuItem("ҩƷ�����Ϣ");
                        menuDrug.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, menuDrug);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("13") == 0)
                        {
                            menuDrug.Enabled = false;
                        }

                        ToolStripMenuItem m_routematch = new ToolStripMenuItem("��ҩ;�������Ϣ");
                        m_routematch.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_routematch);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("14") == 0)
                        {
                            m_routematch.Enabled = false;
                        }

                        ToolStripMenuItem m_hospital_drug = new ToolStripMenuItem("ҽԺҩƷ��Ϣ");
                        m_hospital_drug.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_hospital_drug);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("105") == 0)
                        {
                            m_hospital_drug.Enabled = false;
                        }

                        ToolStripMenuItem m_line4 = new ToolStripMenuItem("-");
                        menuPass.DropDownItems.Insert(i, m_line4);
                        i++;

                        ToolStripMenuItem m_system_set = new ToolStripMenuItem("ϵͳ����");
                        m_system_set.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_system_set);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("11") == 0)
                        {
                            m_system_set.Enabled = false;
                        }

                        ToolStripMenuItem m_line5 = new ToolStripMenuItem("-");
                        menuPass.DropDownItems.Insert(i, m_line5);
                        i++;

                        ToolStripMenuItem m_studydrug = new ToolStripMenuItem("��ҩ�о�");
                        m_studydrug.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_studydrug);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("12") == 0)
                        {
                            m_studydrug.Enabled = false;
                        }

                        ToolStripMenuItem m_line6 = new ToolStripMenuItem("-");
                        menuPass.DropDownItems.Insert(i, m_line6);
                        i++;

                        ToolStripMenuItem m_warn = new ToolStripMenuItem("����");
                        m_warn.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_warn);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("11") == 0)
                        {
                            m_warn.Enabled = false;
                        }

                        ToolStripMenuItem m_checkone = new ToolStripMenuItem("���");
                        m_checkone.Click += new EventHandler(mnuPass_Click);
                        menuPass.DropDownItems.Insert(i, m_checkone);
                        i++;
                        if (this.IReasonableMedicine.PassGetStateIn("3") == 0)
                        {
                            m_checkone.Enabled = false;
                        }

                    }

                    #endregion
                }
                else
                {
                    //{5D9302B2-9B71-4530-86EA-350063AF56F0}
                    if (!this.EditGroup)
                    {
                        #region �ǿ��������²˵���ʾ
                        ToolStripMenuItem mnuTip = new ToolStripMenuItem("��ע");//��ע
                        mnuTip.Click += new EventHandler(mnuTip_Click);
                        this.contextMenu1.Items.Add(mnuTip);

                        ToolStripMenuItem mnuTot = new ToolStripMenuItem("�ۼ�������ѯ");//�ۼ�����
                        mnuTot.Visible = false;//��ʱ�Ȳ���
                        mnuTot.Click += new EventHandler(mnuTot_Click);

                        try
                        {
                            string OrderID = this.fpSpread1.ActiveSheet.Cells[this.ActiveRowIndex, this.myOrderClass.iColumns[2]].Text;
                            //if (this.OrderManagement.QueryOneOrder(OrderID).Item.IsPharmacy)
                            if (this.OrderManagement.QueryOneOrder(OrderID).Item.ItemType == EnumItemType.Drug)
                            {
                                this.contextMenu1.Items.Add(mnuTot);
                            }
                        }
                        catch { }

                        #region �������ѯ{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}pacs�ӿ�����
                        ToolStripMenuItem mnuPacsView = new ToolStripMenuItem("Pacs�����ѯ");//��ע
                        mnuPacsView.Click += new EventHandler(mnuPacsView_Click);
                        this.contextMenu1.Items.Add(mnuPacsView);
                    }
                    #endregion

                    #endregion

                    #region ����
                    ToolStripMenuItem mnuUp = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuUp.Click += new EventHandler(mnuUp_Click);
                    if (this.fpSpread1.ActiveSheet.ActiveRowIndex <= 0) mnuUp.Enabled = false;
                    this.contextMenu1.Items.Add(mnuUp);
                    #endregion

                    #region ����
                    ToolStripMenuItem mnuDown = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuDown.Click += new EventHandler(mnuDown_Click);
                    if (this.fpSpread1.ActiveSheet.ActiveRowIndex >= this.fpSpread1.ActiveSheet.RowCount - 1 || this.fpSpread1.ActiveSheet.ActiveRowIndex < 0) mnuDown.Enabled = false;
                    this.contextMenu1.Items.Add(mnuDown);
                    #endregion

                    //����ģʽ���������ƶ�
                    if (dvLong.RowFilter == "" && dvShort.RowFilter == "")
                    {
                        mnuUp.Enabled = true;
                        mnuDown.Enabled = true;
                    }
                    else
                    {
                        mnuUp.Enabled = false;
                        mnuDown.Enabled = false;
                    }
                    //if (!this.IReasonableMedicine.PassEnabled)
                    //    return;
                    //ToolStripItem muItem = sender as ToolStripItem;
                    //switch (muItem.Text)
                    //{

                    //    #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ��Ӻ�����ҩ�Ҽ��˵�

                    //    #region һ���˵�

                    //    case "����ʷ/����״̬":
                    //        int iReg;
                    //        this.IReasonableMedicine.PassSetPatientInfo(this.myPatientInfo, this.empl.ID, this.empl.Name);
                    //        this.IReasonableMedicine.ShowFloatWin(false);
                    //        iReg = this.IReasonableMedicine.DoCommand(22);
                    //        if (iReg == 2)
                    //        {
                    //            this.PassTransOrder(1, true);
                    //        }
                    //        break;

                    //    case "ҩ���ٴ���Ϣ�ο�":
                    //        this.PassTransDrug(101);
                    //        break;
                    //    case "ҩƷ˵����":
                    //        this.PassTransDrug(102);
                    //        break;
                    //    case "�й�ҩ��":
                    //        this.PassTransDrug(107);
                    //        break;
                    //    case "������ҩ����":
                    //        this.PassTransDrug(103);
                    //        break;
                    //    case "ҩ�����ֵ":
                    //        this.PassTransDrug(104);
                    //        break;
                    //    case "�ٴ�������Ϣ�ο�":
                    //        this.PassTransDrug(220);
                    //        break;

                    //    case "ҽҩ��Ϣ����":
                    //        this.PassTransDrug(106);
                    //        break;

                    //    case "ҩƷ�����Ϣ":
                    //        this.PassTransDrug(13);
                    //        break;
                    //    case "��ҩ;�������Ϣ":
                    //        this.PassTransDrug(14);
                    //        break;
                    //    case "ҽԺҩƷ��Ϣ":
                    //        this.PassTransDrug(105);
                    //        break;

                    //    case "ϵͳ����":
                    //        this.PassTransDrug(11);
                    //        break;

                    //    case "��ҩ�о�":
                    //        this.IReasonableMedicine.ShowFloatWin(false);
                    //        this.PassTransOrder(12, false);
                    //        break;

                    //    case "����":
                    //        this.PassTransDrug(6);
                    //        break;

                    //    case "���":
                    //        this.IReasonableMedicine.ShowFloatWin(false);
                    //        this.PassTransOrder(3, true);
                    //        break;

                    //    #endregion

                    //    #region �����˵�

                    //    case "ҩ��-ҩ���໥����":
                    //        this.PassTransDrug(201);
                    //        break;
                    //    case "ҩ��-ʳ���໥����":
                    //        this.PassTransDrug(202);

                    //        break;
                    //    case "����ע�����������":
                    //        this.PassTransDrug(203);
                    //        break;
                    //    case "����ע�����������":
                    //        this.PassTransDrug(204);
                    //        break;

                    //    case "����֢":
                    //        this.PassTransDrug(205);
                    //        break;
                    //    case "������":
                    //        this.PassTransDrug(206);
                    //        break;

                    //    case "��������ҩ":
                    //        this.PassTransDrug(207);
                    //        break;
                    //    case "��ͯ��ҩ":
                    //        this.PassTransDrug(208);
                    //        break;
                    //    case "��������ҩ":
                    //        this.PassTransDrug(209);
                    //        break;
                    //    case "��������ҩ":
                    //        this.PassTransDrug(210);
                    //        break;

                    //    #endregion

                    //    #endregion
                    //    default:
                    //        break;
                    //}
                }

                #region ��Ӻ�����ҩ�Ҽ��˵�
                //if (this.EnabledPass && Pass.Pass.PassEnabled)
                //{
                //    ToolStripItem menuPass = new ToolStripItem("������ҩ");
                //    this.contextMenu1.Items.Add(menuPass);

                //    ToolStripItem menuAllergn = new ToolStripItem("����ʷ/����״̬");
                //    menuAllergn.Click += new EventHandler(mnuPass_Click);
                //    menuPass.Items.Add(menuAllergn);

                //    if (Pass.Pass.PassGetState("12") != 0)
                //    {
                //        ToolStripItem menuResearch = new ToolStripItem("��ҩ�о�");
                //        menuResearch.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuResearch);
                //    }
                //    if (Pass.Pass.PassGetState("3") != 0)
                //    {
                //        ToolStripItem menuCheck = new ToolStripItem("���");
                //        menuCheck.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCheck);
                //    }
                //    #region ҩƷר����Ϣ
                //    ToolStripItem menuSpecialInfo = new ToolStripItem("ר����Ϣ");
                //    menuPass.Items.Add(menuSpecialInfo);

                //    if (Pass.Pass.PassGetState("201") != 0)
                //    {
                //        ToolStripItem menuDDim = new ToolStripItem("ҩ��-ҩ���໥����");
                //        menuSpecialInfo.Items.Add(menuDDim);
                //        menuDDim.Click += new EventHandler(mnuPass_Click);
                //    }

                //    if (Pass.Pass.PassGetState("202") != 0)
                //    {
                //        ToolStripItem menuDFim = new ToolStripItem("ҩ��-ʳ���໥����");
                //        menuSpecialInfo.Items.Add(menuDFim);
                //        menuDFim.Click += new EventHandler(mnuPass_Click);
                //    }
                //    #endregion

                //    if (Pass.Pass.PassGetState("13") != 0)
                //    {
                //        ToolStripItem menuDrug = new ToolStripItem("ҩƷ�����Ϣ");
                //        menuDrug.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuDrug);
                //    }
                //    if (Pass.Pass.PassGetState("14") != 0)
                //    {
                //        ToolStripItem menuUsage = new ToolStripItem("�÷������Ϣ");
                //        menuUsage.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuUsage);
                //    }
                //}

                #endregion

                this.contextMenu1.Show(this.fpSpread1, new Point(e.X, e.Y));
            }
        }
        /// <summary>
        /// ����Ҽ��˵�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                this.contextMenu1.Items.Clear();
            }
            catch { }
        }
        /// <summary>
        /// ɾ�������ϡ�ֹͣҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDel_Click(object sender, EventArgs e)
        {
            this.Delete();
        }
        /// <summary>
        /// ȡ��ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.Delete();
        }
        /// <summary>
        /// ��ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTip_Click(object sender, EventArgs e)
        {
            ucTip ucTip1 = new ucTip();
            ucTip1.IsCanModifyHypotest = false;
            string OrderID = this.fpSpread1.ActiveSheet.Cells[this.ActiveRowIndex, this.myOrderClass.iColumns[2]].Text;
            int iHypotest = this.OrderManagement.QueryOrderHypotest(OrderID);
            if (iHypotest == -1)
            {
                MessageBox.Show(this.OrderManagement.Err);
                return;
            }
            try
            {
                ucTip1.Tip = this.fpSpread1.ActiveSheet.GetNote(this.ActiveRowIndex, this.myOrderClass.iColumns[6]).ToString();
            }
            catch { }
            int i = this.myOrderClass.iColumns[3];
            int state = Neusoft.FrameWork.Function.NConvert.ToInt32(this.fpSpread1.ActiveSheet.Cells[fpSpread1_Sheet1.ActiveRowIndex, i].Text);
            if (state != 0)
            {
                ucTip1.btnCancel.Enabled = false;
                ucTip1.btnSave.Enabled = false;
            }
            ucTip1.Hypotest = iHypotest;
            ucTip1.OKEvent += new myTipEvent(ucTip1_OKEvent);
            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucTip1);
        }
        /// <summary>
        /// ����ҽ��  �ɳ�������Ϊ��������������Ϊ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (this.fpSpread1.ActiveSheet.RowCount <= 0)
                return;

            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.fpSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
            if (order == null)
                return;

            #region addby xuewj 2010-3-23 {5DE2C8F9-2E5D-43d6-9CAD-A5E0F60AC94B} ��ʿ�����������˿�����ҩƷ

            if (this.isNurseCreate)
            {
                if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
                {
                    MessageBox.Show("��ʿ�����������˿�����ҩƷ!");
                    return ;
                }
            }

            #endregion

            if (ValidCopy() == -1)
            {               
                return;
            }

            #region ��ȡ��ҽ����Ϻ�
            string ComboNo;
            try
            {
                ComboNo = this.OrderManagement.GetNewOrderComboID();
                if (ComboNo == null || ComboNo == "")
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�������з������� ��ȡ��ҽ����ϺŹ����г���"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�������з������� ��ȡ��ҽ����ϺŹ����з����쳣") + ex.Message);
                return;
            }
            #endregion

            DateTime dtNow;
            string err;
            try
            {
                dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���ϵͳ������ʱ�����!"), Neusoft.FrameWork.Management.Language.Msg("��ʾ"));
                return;
            }
            for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order temp = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex);
                if (temp == null)
                    continue;
                //{0817AFF8-A0DC-4a06-BEAD-015BC49AE973}
                if (this.fpSpread1.ActiveSheet.IsSelected(i, 0))
                //if (temp.Combo.ID == order.Combo.ID)
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order o = temp.Clone();
                    o.Patient = this.myPatientInfo.Clone();
                    #region ҩƷ����ҩƷ��Ŀ��ֵ
                    //if (o.Item.IsPharmacy)
                    if (o.Item.ItemType == EnumItemType.Drug)
                    {
                        if (Neusoft.HISFC.BizProcess.Integrate.Order.FillPharmacyItemWithStockDept(null, ref o,out err) == -1)
                        {
                            MessageBox.Show(err);
                            return;
                        }
                        if (o == null) return;
                       
                    }
                    else
                    {
                        if (Neusoft.HISFC.BizProcess.Integrate.Order.FillFeeItem(null, ref o, out err) == -1)
                        {
                            MessageBox.Show(err);
                            return;
                        }
                        if (o == null) return;
                    }
                    #endregion

                    #region ҽ��������Ϣ��ֵ
                    o.OrderType.IsDecompose = !o.OrderType.IsDecompose;//������ʱ����
                    Neusoft.HISFC.Models.Order.OrderType ordertype = o.OrderType;
                    
                    if (o.Item.Price == 0)
                    {
                        Classes.OrderType.CheckChargeableOrderType(ref ordertype, false, true);
                    }
                    else
                    {
                        Classes.OrderType.CheckChargeableOrderType(ref ordertype, true, true);
                    }
                    o.OrderType = ordertype;
                    o.Memo = "";
                    o.Status = 0;
                    o.ID = "";
                    o.SortID = 0;
                    o.Combo.ID = ComboNo;
                    o.EndTime = DateTime.MinValue;
                    o.DCOper.OperTime = DateTime.MinValue;
                    o.DcReason.ID = "";
                    o.DcReason.Name = "";
                    o.DCOper.ID = "";
                    o.DCOper.Name = "";
                    o.ConfirmTime= DateTime.MinValue;
                    o.Nurse.ID = "";
                    o.MOTime = dtNow;

                    if (this.GetReciptDept() != null)
                        o.ReciptDept = this.GetReciptDept().Clone();
                    if (this.GetReciptDoc() != null)
                        o.ReciptDoctor = this.GetReciptDoc().Clone();
                    if (this.GetReciptDoc() != null)
                    {
                        o.Oper.ID = this.GetReciptDoc().ID;
                        #region {D0F3CBFD-C21B-4c8d-A280-E09ADC0FB2AA}
                        //o.Oper.ID = this.GetReciptDoc().Name;
                        o.Oper.Name = this.GetReciptDoc().Name;
                        #endregion
                    }
                    o.NextMOTime = o.BeginTime;
                    o.CurMOTime = o.BeginTime;
                    #endregion

                    if (this.fpSpread1.ActiveSheetIndex == 0)
                    {
                        #region ��������Ϊ����
                        Classes.Function.SetDefaultFrequency(o);
                        //if (o.Item.IsPharmacy)
                        if (o.Item.ItemType == EnumItemType.Drug)
                        {
                            //�Զ������������� ������С��λ��ʾ 
                            try
                            {
                                o.Qty = System.Math.Round(o.DoseOnce / ((Neusoft.HISFC.Models.Pharmacy.Item)o.Item).BaseDose, 0);//??
                            }
                            catch
                            {
                                o.Qty = 0;
                            }
                            
                            o.Unit = ((Neusoft.HISFC.Models.Pharmacy.Item)o.Item).MinUnit;//???
                        }
                        try
                        {
                            this.refreshComboFlag = "1";		//ֻ�������������Ϻ�ˢ�¼���
                            this.AddNewOrder(o, 1);//short
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("����ҽ�������з�������Ԥ֪����" + ex.Message + ex.Source);
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        //��ʱ
                        #region ��������Ϊ����
                        //�ж��Ƿ���Ը���
                        bool b = false;
                        string strSysClass = "";
                        if (o.Item.SysClass.ID.ToString().Length > 1)
                            strSysClass = o.Item.SysClass.ID.ToString().Substring(0, 2);
                        //��ʱҽ������Ϊ����������Ϊ0
                        o.Qty = 0;
                        
                        switch (strSysClass)
                        {
                            case "MR":				//��ҩƷ
                            case "UO":				//����
                            case "UC":				//���
                            case "PC":				//�г�ҩ���в�ҩ
                                b = false;
                                break;
                            default:
                                Classes.Function.SetDefaultFrequency(o);
                                try
                                {
                                    this.refreshComboFlag = "0";		//ֻ�Գ�����Ͻ���ˢ�¼���
                                    this.AddNewOrder(o, 0);//long
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("����ҽ�������з�������Ԥ֪����" + ex.Message + ex.Source);
                                    return;
                                }
                                b = true;
                                break;
                        }
                        if (b == false)
                        {
                            MessageBox.Show(o.Item.SysClass.ToString() + "������Ϊ������");
                            return;
                        }
                        #endregion
                    }
                }
            }
            this.RefreshCombo();
     
        }
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopyAs_Click(object sender, EventArgs e)
        {
            if (this.fpSpread1.ActiveSheet.RowCount <= 0)
                return;

            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.fpSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
            if (order == null)
                return;

            #region addby xuewj 2010-3-23 {5DE2C8F9-2E5D-43d6-9CAD-A5E0F60AC94B} ��ʿ�����������˿�����ҩƷ

            if (this.isNurseCreate)
            {
                if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
                {
                    MessageBox.Show("��ʿ�����������˿�����ҩƷ!");
                    return ;
                }
            }

            #endregion

            if (this.ValidCopy() == -1)
            {
                return;
            }

            #region ��ȡ��ҽ����Ϻ�
            string ComboNo;
            try
            {
                ComboNo = this.OrderManagement.GetNewOrderComboID();
                if (ComboNo == null || ComboNo == "")
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�������з������� ��ȡ��ҽ����ϺŹ����г���"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����ҽ�������з������� ��ȡ��ҽ����ϺŹ����з����쳣") + ex.Message);
                return;
            }
            #endregion

            DateTime dtNow;
            string err;
            try
            {
                dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���ϵͳ������ʱ�����!"), Neusoft.FrameWork.Management.Language.Msg("��ʾ"));
                return;
            }
            ArrayList al = new ArrayList();
            for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order temp = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex);
                if (temp == null)
                    continue;
                //{0817AFF8-A0DC-4a06-BEAD-015BC49AE973}
                //if (temp.Combo.ID == order.Combo.ID)
                if(this.fpSpread1.ActiveSheet.IsSelected(i,0))
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order o = temp.Clone();
                    
                    if(this.myPatientInfo != null)
                        o.Patient = this.myPatientInfo.Clone();

                    #region ҩƷ����ҩƷ��Ŀ��ֵ
                    //if (o.Item.IsPharmacy)
                    if (o.Item.ItemType == EnumItemType.Drug)
                    {
                        if (Neusoft.HISFC.BizProcess.Integrate.Order.FillPharmacyItemWithStockDept(null, ref o, out err) == -1)
                        {
                            MessageBox.Show(err);
                            return;
                        }
                        if (o == null) return;

                    }
                    else
                    {
                        if (Neusoft.HISFC.BizProcess.Integrate.Order.FillFeeItem(null, ref o, out err) == -1)
                        {
                            MessageBox.Show(err);
                            return;
                        }
                        if (o == null) return;
                    }
                    #endregion

                    #region ҽ��������Ϣ��ֵ
                    Neusoft.HISFC.Models.Order.OrderType ordertype = o.OrderType;
                    
                    if (o.Item.Price == 0)
                    {
                        Classes.OrderType.CheckChargeableOrderType(ref ordertype, false);
                    }
                    else
                    {
                        Classes.OrderType.CheckChargeableOrderType(ref ordertype, true);
                    }
                    o.OrderType = ordertype;
                    o.Memo = "";
                    o.Status = 0;
                    o.ID = "";
                    o.SortID = 0;
                    o.Combo.ID = ComboNo;
                    o.EndTime = DateTime.MinValue;
                    o.DCOper.OperTime = DateTime.MinValue;
                    o.DcReason.ID = "";
                    o.DcReason.Name = "";
                    o.DCOper.ID = "";
                    o.DCOper.Name = "";
                    o.ConfirmTime = DateTime.MinValue;
                    o.Nurse.ID = "";
                    o.MOTime = dtNow;

                    #region ���execute_date�ֶ� {F0FBD92A-DFF2-4feb-B3A9-2E4B724A711C}
                    o.ExecOper.OperTime = DateTime.MinValue;
                        //Convert.ToDateTime("0001-01-01");
                    #endregion

                    if (this.GetReciptDept() != null)
                        o.ReciptDept = this.GetReciptDept().Clone();
                    if (this.GetReciptDoc() != null)
                        o.ReciptDoctor = this.GetReciptDoc().Clone();
                    if (this.GetReciptDoc() != null)
                    {
                        o.Oper.ID = this.GetReciptDoc().ID;
                        o.Oper.ID = this.GetReciptDoc().Name;
                    }
                    o.NextMOTime = o.BeginTime;
                    o.CurMOTime = o.BeginTime;
                    #endregion

                    al.Add(o);
                }
            }

            for (int i = 0; i < al.Count; i++)
            {
                if (this.fpSpread1.ActiveSheetIndex == 0)
                { //long
                    try
                    {
                        this.refreshComboFlag = "0";			//ֻ��Գ�����ϺŽ���ˢ�¼���
                        this.AddNewOrder(al[i], 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("����ҽ�������з�������Ԥ֪����" + ex.Message + ex.Source);
                        return;
                    }
                }
                else
                {//��ʱ
                    try
                    {
                        this.refreshComboFlag = "1";			//ֻ���������ϺŽ���ˢ�¼���
                        this.AddNewOrder(al[i], 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("����ҽ�������з�������Ԥ֪����" + ex.Message + ex.Source);
                        return;
                    }
                }
            }
            this.RefreshCombo();
            #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
            if (this.fpSpread1.ActiveSheetIndex == 1)
            {
                this.ShowTempCost();
            }
            #endregion
        }
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuUp_Click(object sender, EventArgs e)
        {
            if (this.fpSpread1.ActiveSheet.ActiveRowIndex <= 0) return;
            int CurrentActiveRow = this.fpSpread1.ActiveSheet.ActiveRowIndex;//��ǰ��

            int Sort = int.Parse(this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex - 1, this.myOrderClass.iColumns[28]].Text);//�����һ�е����

            string ComboNo = this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex - 1, this.myOrderClass.iColumns[4]].Text;//�����һ�е���Ϻ�

            int oldSort = int.Parse(this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[28]].Text);//��õ�ǰ���

            string oldComboNo = this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[4]].Text;//��õ�ǰ��Ϻ�

            int tmp = -1;

            if (ComboNo == oldComboNo)//�����ƶ�
            {
                //�Ի�SortID
                //--��һ��--
                this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex - 1, this.myOrderClass.iColumns[28]].Value = oldSort;//-1
                //--��ǰһ��
                this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[28]].Value = Sort;//-1

            }
            else //��ͬ����ƶ�
            {
                //���һ�SortID
                Sort = -1;
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[4]].Text == ComboNo)
                    {
                        if (Sort == -1) Sort = int.Parse(fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Text) - 1;//���������һ�������-1
                        this.fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = tmp;//-1
                    }
                }
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[4]].Text == oldComboNo)
                    {
                        Sort++;
                        fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = Sort;//���´�����һ����� ���£��������
                        SaveSortID(i);
                    }
                }
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value.ToString() == tmp.ToString())
                    {
                        Sort++;
                        fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = Sort;//������ǰ�ģ����θ������
                        SaveSortID(i);
                    }
                }

            }


            this.fpSpread1.ActiveSheet.ClearSelection();
            this.fpSpread1.ActiveSheet.AddSelection(CurrentActiveRow - 1, 0, 1, 1);

            if (this.fpSpread1.ActiveSheetIndex == 0)
                this.refreshComboFlag = "0";			//ֻ��ˢ�µ�ǰҽ�����͵���Ϻ�
            else
                this.refreshComboFlag = "1";
            this.RefreshCombo();
        }
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDown_Click(object sender, EventArgs e)
        {
            if (this.fpSpread1.ActiveSheet.ActiveRowIndex >= this.fpSpread1.ActiveSheet.RowCount - 1) return;
            int CurrentActiveRow = this.fpSpread1.ActiveSheet.ActiveRowIndex;
            ArrayList alRows = this.GetSelectedRows();
            int Sort = int.Parse(this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex + 1, this.myOrderClass.iColumns[28]].Text);//�����һ�е����

            string ComboNo = this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex + 1, this.myOrderClass.iColumns[4]].Text;//�����һ�е���Ϻ�

            int oldSort = int.Parse(this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[28]].Text);//��õ�ǰ���

            string oldComboNo = this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[4]].Text;//��õ�ǰ��Ϻ�

            int tmp = -1;

            if (ComboNo == oldComboNo)//�����ƶ�
            {
                //�Ի�SortID
                //--��һ��--
                this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex + 1, this.myOrderClass.iColumns[28]].Value = oldSort;//-1
                //--��ǰһ��
                this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, this.myOrderClass.iColumns[28]].Value = Sort;//-1

            }
            else //��ͬ����ƶ�
            {
                //���һ�SortID
                Sort = -1;
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[4]].Text == oldComboNo)
                    {
                        if (Sort == -1) Sort = int.Parse(fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Text) - 1;//���������һ�������-1
                        this.fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = tmp;//-1
                    }
                }
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[4]].Text == ComboNo)
                    {
                        Sort++;
                        fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = Sort;//������ǰ�ģ����θ������
                        SaveSortID(i);
                    }
                }
                //������һ��
                for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
                {
                    if (fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value.ToString() == tmp.ToString())
                    {
                        Sort++;
                        fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[28]].Value = Sort;//���´�����һ����� ���£��������
                        SaveSortID(i);
                    }
                }


            }


            this.fpSpread1.ActiveSheet.ClearSelection();
            this.fpSpread1.ActiveSheet.AddSelection(CurrentActiveRow + 1, 0, 1, 1);

            if (this.fpSpread1.ActiveSheetIndex == 0)
                this.refreshComboFlag = "0";		//ֻ��ˢ�µ�ǰҽ�����͵���Ϻż���
            else
                this.refreshComboFlag = "1";
            this.RefreshCombo();
         
          
        }
        /// <summary>
        /// ��ʾ
        /// </summary>
        /// <param name="Tip"></param>
        /// <param name="Hypotest"></param>
        private void ucTip1_OKEvent(string Tip, int Hypotest)
        {
            this.fpSpread1.ActiveSheet.SetNote(this.ActiveRowIndex, this.myOrderClass.iColumns[6], Tip);
            string orderID = this.fpSpread1.ActiveSheet.Cells[this.ActiveRowIndex, this.myOrderClass.iColumns[2]].Text;
            if (this.OrderManagement.UpdateFeedback(this.myPatientInfo.ID, orderID, Tip, Hypotest) == -1)
            {
                MessageBox.Show(this.OrderManagement.Err);
                this.OrderManagement.Err = "";
            }
            
        }
        /// <summary>
        /// �ۼ�������ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTot_Click(object sender, EventArgs e)
        {
            string OrderID = this.fpSpread1.ActiveSheet.Cells[this.ActiveRowIndex, this.myOrderClass.iColumns[2]].Text;
            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.OrderManagement.QueryOneOrder(OrderID);
            if (order == null) return;
            //Classes.Function.TotalUseDrug(this.GetPatient().ID, order.Item.ID);
        }
        /// <summary>
        /// �޸�ҽ������ 
        /// </summary>
        private void menuChange_Click(object sender, EventArgs e)
        {
            //using (ucSimpleChange uc = new ucSimpleChange())
            //{
            //    Neusoft.HISFC.Models.Order.Inpatient.Order order = this.fpSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;

            //    uc.TitleLabel = "ҽ�������޸�";
            //    uc.InfoLabel = "��Ŀ����:" + order.Item.Name;
            //    uc.OperInfo = "ҽ������";

            //    //��ȡҽ������
            //    Neusoft.HISFC.BizLogic.Manager.OrderType orderType = new Neusoft.HISFC.BizLogic.Manager.OrderType();
            //    ArrayList alOrderType = orderType.GetList();
            //    ArrayList alLong = new ArrayList();
            //    ArrayList alShort = new ArrayList();
            //    foreach (Neusoft.HISFC.Models.Order.Inpatient.OrderType info in alOrderType)
            //    {
            //        if (info.IsDecompose)
            //        {
            //            alLong.Add(info);
            //        }
            //        else
            //        {
            //            alShort.Add(info);
            //        }
            //    }

            //    if (this.fpSpread1.ActiveSheetIndex == 0)		//����
            //        uc.InfoItems = alLong;
            //    else
            //        uc.InfoItems = alShort;

            //    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
            //    try
            //    {
            //        if (uc.IReturn == 1)
            //        {		//ȷ����ť
            //            Neusoft.HISFC.Models.Order.Inpatient.OrderType tempOrderType = uc.ReturnInfo as Neusoft.HISFC.Models.Order.Inpatient.OrderType;

            //            bool isUp = true;
            //            bool isDown = true;
            //            int i = this.fpSpread1.ActiveSheet.ActiveRowIndex;
            //            (this.fpSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order).OrderType = tempOrderType;
            //            this.ucItemSelect1.Order.Inpatient.OrderType = tempOrderType;
            //            this.fpSpread1.ActiveSheet.Cells[i, this.myOrderClass.iColumns[1]].Text = tempOrderType.Name;

            //            int iUp, iDown;
            //            iUp = i;
            //            iDown = i;
            //            while (isUp || isDown)
            //            {
            //                #region ���ϲ��� �絽��ǰһ�л���ϺŲ�ͬ���ñ�־Ϊfalse
            //                if (isUp)
            //                {
            //                    iUp = iUp - 1;
            //                    if (iUp < 0)
            //                        isUp = false;
            //                    else
            //                    {
            //                        if (((Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[iUp].Tag).Combo.ID == order.Combo.ID)
            //                        {
            //                            (this.fpSpread1.ActiveSheet.Rows[iUp].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order).OrderType = tempOrderType;
            //                            this.fpSpread1.ActiveSheet.Cells[iUp, this.myOrderClass.iColumns[1]].Text = tempOrderType.Name;
            //                        }
            //                        else
            //                        {
            //                            isUp = false;
            //                        }
            //                    }
            //                }
            //                #endregion

            //                #region ���²��� ��������һ�л���ϺŲ�ͬ���ñ�־Ϊfalse
            //                if (isDown)
            //                {
            //                    iDown = iDown + 1;
            //                    if (iDown >= this.fpSpread1.ActiveSheet.Rows.Count)
            //                        isDown = false;
            //                    else
            //                    {
            //                        if (((Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[iDown].Tag).Combo.ID == order.Combo.ID)
            //                        {
            //                            (this.fpSpread1.ActiveSheet.Rows[iDown].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order).OrderType = tempOrderType;
            //                            this.fpSpread1.ActiveSheet.Cells[iDown, this.myOrderClass.iColumns[1]].Text = tempOrderType.Name;
            //                        }
            //                        else
            //                        {
            //                            isDown = false;
            //                        }
            //                    }
            //                }
            //                #endregion
            //            }
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}
        }

        /// <summary>
        /// �ж��Ƿ���ϸ���ҩƷ������
        /// </summary>
        /// <returns></returns>
        private int ValidCopy()
        {
            string tempID = string.Empty;
            for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order temp = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex);
                if (temp == null)
                    continue;
                if (this.fpSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    if (tempID == string.Empty)
                    {
                        tempID = temp.Combo.ID;
                    }
                    else if (tempID != temp.Combo.ID)
                    {
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("����һ���ҩƷ������һ����!"), Neusoft.FrameWork.Management.Language.Msg("��ʾ"));
                        return -1;
                    }
                }
            }

            return 1;
        }

        /// <summary>
        /// ������
        /// {C6E229AC-A1C4-4725-BBBB-4837E869754E}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveGroup_Click(object sender, EventArgs e)
        {
            this.SaveGroup();
        }

        #region {C6E229AC-A1C4-4725-BBBB-4837E869754E}

        /// <summary>
        /// ���״洢
        /// </summary>
        private void SaveGroup()
        {
            Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager group = new Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager();
            group.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.I;
            try
            {
                group.IsManager = (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).IsManager;
            }
            catch
            { }

            ArrayList al = new ArrayList();
            for (int i = 0; i < this.fpSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.fpSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex).Clone();
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
                }
            }
            if (al.Count > 0)
            {
                group.alItems = al;
                group.ShowDialog();
                if (refreshGroup != null)
                {
                    this.refreshGroup();
                }
            }
        }

        #endregion
      
        /// <summary>
        /// ����ҩƷϵͳҩƷ��ѯ  Add By liangjz 2005-11
        /// </summary>
        private void mnuPass_Click(object sender, EventArgs e)
        {
            if (!this.IReasonableMedicine.PassEnabled)
                return;
            ToolStripItem muItem = sender as ToolStripItem;
            switch (muItem.Text)
            {

                #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ��Ӻ�����ҩ�Ҽ��˵�

                #region һ���˵�

                case "����ʷ/����״̬":
                    int iReg;
                    this.IReasonableMedicine.PassSetPatientInfo(this.myPatientInfo, this.empl.ID, this.empl.Name);
                    this.IReasonableMedicine.ShowFloatWin(false);
                    iReg = this.IReasonableMedicine.DoCommand(22);
                    if (iReg == 2)
                    {
                        this.PassTransOrder(1, true);
                    }
                    break;

                case "ҩ���ٴ���Ϣ�ο�":
                    this.PassTransDrug(101);
                    break;
                case "ҩƷ˵����":
                    this.PassTransDrug(102);
                    break;
                case "�й�ҩ��":
                    this.PassTransDrug(107);
                    break;
                case "������ҩ����":
                    this.PassTransDrug(103);
                    break;
                case "ҩ�����ֵ":
                    this.PassTransDrug(104);
                    break;
                case "�ٴ�������Ϣ�ο�":
                    this.PassTransDrug(220);
                    break;

                case "ҽҩ��Ϣ����":
                    this.PassTransDrug(106);
                    break;

                case "ҩƷ�����Ϣ":
                    this.PassTransDrug(13);
                    break;
                case "��ҩ;�������Ϣ":
                    this.PassTransDrug(14);
                    break;
                case "ҽԺҩƷ��Ϣ":
                    this.PassTransDrug(105);
                    break;

                case "ϵͳ����":
                    this.PassTransDrug(11);
                    break;

                case "��ҩ�о�":
                    this.IReasonableMedicine.ShowFloatWin(false);
                    this.PassTransOrder(12, false);
                    break;

                case "����":
                    this.PassTransDrug(6);
                    break;

                case "���":
                    this.IReasonableMedicine.ShowFloatWin(false);
                    this.PassTransOrder(3, true);
                    break;

                #endregion

                #region �����˵�

                case "ҩ��-ҩ���໥����":
                    this.PassTransDrug(201);
                    break;
                case "ҩ��-ʳ���໥����":
                    this.PassTransDrug(202);

                    break;
                case "����ע�����������":
                    this.PassTransDrug(203);
                    break;
                case "����ע�����������":
                    this.PassTransDrug(204);
                    break;

                case "����֢":
                    this.PassTransDrug(205);
                    break;
                case "������":
                    this.PassTransDrug(206);
                    break;

                case "��������ҩ":
                    this.PassTransDrug(207);
                    break;
                case "��ͯ��ҩ":
                    this.PassTransDrug(208);
                    break;
                case "��������ҩ":
                    this.PassTransDrug(209);
                    break;
                case "��������ҩ":
                    this.PassTransDrug(210);
                    break;

                #endregion

                #endregion
                default:
                    break;
            }
            //if (!Pass.Pass.PassEnabled) return;
            //ToolStripItem muItem = sender as ToolStripItem;
            //switch (muItem.Text)
            //{
            //    case "����ʷ/����״̬":
            //        int iReg;
            //        Pass.Pass.ShowFloatWin(false);
            //        iReg = Pass.Pass.DoCommand(22);
            //        if (iReg == 2)
            //        {
            //            this.PassTransOrder(1, true);
            //        }
            //        break;
            //    case "��ҩ�о�":
            //        Pass.Pass.ShowFloatWin(false);
            //        this.PassTransOrder(12, false);
            //        break;
            //    case "���":
            //        Pass.Pass.ShowFloatWin(false);
            //        this.PassTransOrder(3, false);
            //        break;
            //    case "ҩƷ�����Ϣ":
            //        this.PassTransDrug(13);
            //        break;
            //    case "�÷������Ϣ":
            //        this.PassTransDrug(14);
            //        break;
            //    case "ҩ��-ҩ���໥����":
            //        this.PassTransDrug(201);
            //        break;
            //    case "ҩ��-ʳ���໥����":
            //        this.PassTransDrug(202);
            //        break;
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// �ϼ�ҽ�����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCheckOrder_Click(object sender, EventArgs e)
        {
            this.JudgeSpecialOrder();
        }
        /// <summary>
        /// ����fp����ʱ���ҵ�����fp������
        /// </summary>
        /// <param name="tempId">��ʱ��</param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private int getOrderRowIndex(string tempId, int sheetIndex)
        {
            for (int i = 0; i < this.fpSpread1.Sheets[sheetIndex].RowCount; i++)
            {
                if (this.fpSpread1.ActiveSheet.Cells[i, myOrderClass.iColumns[37]].Text == tempId)
                {
                    return i;
                }
            }
            return -1;
        }
        #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ
        /// <summary>
        /// ����fp�ϵ�˳����ҵ�alAllOrder�е�ҽ��
        /// </summary>
        /// <param name="id">fp�ϵ�˳���</param>
        /// <returns>alAllOrder�е�ҽ��</returns>
        public Neusoft.HISFC.Models.Order.Inpatient.Order getOrderTermById(string id, int sheetIndex)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.fpSpread1.Sheets[sheetIndex].Rows[id].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;

            //if (sheetIndex == 0)
            //{
            //    for (int i = 0; i < alAllLongOrder.Count; i++)
            //    {
            //        if (((Neusoft.HISFC.Object.Order.Inpatient.Order)alAllLongOrder[i]).Oper.User03 == id)
            //            return alAllLongOrder[i] as Neusoft.HISFC.Object.Order.Inpatient.Order;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < alAllShortOrder.Count; i++)
            //    {
            //        if (((Neusoft.HISFC.Object.Order.Inpatient.Order)alAllShortOrder[i]).Oper.User03 == id)
            //            return alAllShortOrder[i] as Neusoft.HISFC.Object.Order.Inpatient.Order;
            //    }
            //}
            return null;
        }
        private string ActiveTempIDByRowIndex(int rowIndex)
        {
            return this.fpSpread1.ActiveSheet.Cells[rowIndex, this.myOrderClass.iColumns[37]].Text;
        }

        /// <summary>
        /// ������ҩϵͳ�в鿴�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            //if (!e.RowHeader && !e.ColumnHeader && e.Column == this.myOrderClass.iColumns[0] && this.EnabledPass)
            if (!e.ColumnHeader && e.Column == this.myOrderClass.iColumns[35] && this.EnabledPass)
            {
                if (!this.IReasonableMedicine.PassEnabled)
                {
                    return;
                }

                int iSheetIndex = this.OrderType == Neusoft.HISFC.Models.Order.EnumType.SHORT ? 1 : 0;
                //Neusoft.HISFC.Object.Order.Inpatient.Order info = this.getOrderTermById(this.ActiveTempIDByRowIndex(e.Row), iSheetIndex);
                Neusoft.HISFC.Models.Order.Inpatient.Order info = this.GetObjectFromFarPoint(Neusoft.FrameWork.Function.NConvert.ToInt32(this.ActiveTempID), iSheetIndex);

                if (info == null)
                {
                    return;
                }
                if (info.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    return;
                }
                this.IReasonableMedicine.ShowFloatWin(false);
                if (e.Column == 0)
                {
                    //if (this.fpSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag != null && this.fpSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag.ToString() != "0")
                    if (this.fpSpread1.Sheets[iSheetIndex].RowHeader.Cells[e.Row,0].Tag!=null && this.fpSpread1.Sheets[iSheetIndex].RowHeader.Cells[e.Row,0].Tag.ToString()!="0")
                    {
                        this.IReasonableMedicine.PassGetWarnInfo(info.ApplyNo, "1");
                    }
                }
            }
            else
            {
                this.IReasonableMedicine.ShowFloatWin(false);
            }
        }

        /// <summary>
        /// ��ѯҩƷ������ҩ��Ϣ
        /// </summary>
        /// <param name="e"></param>
        public void PassSetQuery(FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!e.RowHeader && !e.ColumnHeader && (e.Column == this.myOrderClass.iColumns[6]) && this.EnabledPass)
            {
                if (e.Button == MouseButtons.Right)
                {
                    return;
                }
                if (!this.IReasonableMedicine.PassEnabled)
                {
                    return;
                }
                int iSheetIndex = this.OrderType == Neusoft.HISFC.Models.Order.EnumType.SHORT ? 1 : 0;
                Neusoft.HISFC.Models.Order.Inpatient.Order info = this.GetObjectFromFarPoint(e.Row, iSheetIndex);
                //Neusoft.HISFC.Object.Order.Inpatient.Order info = this.getOrderTermById(this.ActiveTempIDByRowIndex(e.Row), this.fpSpread1.ActiveSheetIndex);
                if (info == null)
                {
                    return;
                }
                if (info.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    return;
                }
                this.IReasonableMedicine.ShowFloatWin(false);
                if (e.Column == this.myOrderClass.iColumns[6])
                {
                    #region ҩƷ��ѯ
                    try
                    {
                        int iCellLeft, iCellTop, iCellRight, iCellBottom;

                        #region ��ȡ������������ʾλ��
                        //��ȡFarPoint ��Cell[0,0]��Left���� �Թ����������ʾ
                        int iRowHeader = (int)this.Left + (int)this.fpSpread1.Sheets[iSheetIndex].RowHeader.Columns[0].Width;
                        //��ȡFarPoint��Cell[0,0]��Top���� �Թ����������ʾ
                        int iColumnHeader = (int)this.Top + (int)this.fpSpread1.Sheets[iSheetIndex].ColumnHeader.Rows[0].Height;
                        //�����Cell��Left���� �Թ����������ʾ
                        iCellLeft = iRowHeader + (int)this.fpSpread1.Sheets[iSheetIndex].Columns[this.myOrderClass.iColumns[6]].Width;
                        //��ǰ�����Cell��ɼ���ʼ��֮��ļ������
                        int iRowNum = (int)System.Math.Floor(((e.Y - iColumnHeader) / this.fpSpread1.Sheets[iSheetIndex].Rows[0].Height));
                        //�����Cell��Top���� �Թ����������ʾ
                        //if (this.pnlPatient.Visible)
                        //{
                        //    iCellTop = iColumnHeader + iRowNum * (int)this.fpSpread1.Sheets[iSheetIndex].Rows[0].Height + this.pnlPatient.Height;
                        //}
                        //else
                        //{
                        iCellTop = iColumnHeader + iRowNum * (int)this.fpSpread1.Sheets[iSheetIndex].Rows[0].Height;
                        //}

                        System.Drawing.Point cellPointClient = new Point(iCellLeft - 50, iCellTop);
                        System.Drawing.Point cellPointScreen = this.PointToScreen(cellPointClient);
                        iCellRight = cellPointScreen.X + (int)this.fpSpread1.Sheets[iSheetIndex].Columns[this.myOrderClass.iColumns[6]].Width;
                        iCellBottom = cellPointScreen.Y + (int)this.fpSpread1.Sheets[iSheetIndex].Rows[iRowNum].Height;
                        #endregion


                        if (this.bIsDesignMode)
                        {
                            this.IReasonableMedicine.PassQueryDrug(info.Item.ID, info.Item.Name, info.DoseUnit, info.Usage.Name, cellPointScreen.X - 90,
                                cellPointScreen.Y, iCellRight - 90, iCellBottom + this.ucItemSelect1.Size.Height);
                        }
                        else
                        {
                            this.IReasonableMedicine.PassQueryDrug(info.Item.ID, info.Item.Name, info.DoseUnit, info.Usage.Name, cellPointScreen.X - 90,
                                cellPointScreen.Y, iCellRight - 90, iCellBottom);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    #endregion
                }
                if (e.Column == this.myOrderClass.iColumns[8])
                {
                    if (this.fpSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag != null && this.fpSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag.ToString() != "0")
                    {
                        this.IReasonableMedicine.PassGetWarnInfo(info.ApplyNo, "0");
                    }
                }
            }
            else
            {
                this.IReasonableMedicine.ShowFloatWin(false);
            }
        }

        /// <summary>
        /// �������ҩϵͳ���͵�ǰҽ���������
        /// </summary>
        /// <param name="warnPicFlag">�Ƿ���ʾͼƬ������Ϣ</param>
        ///<param name="checkType">��鷽ʽ 1 �Զ���� 12 ��ҩ�о�  3 �ֹ����</param>
        public void PassTransOrder(int checkType, bool warnPicFlag)
        {
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> alOrder = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();
            Neusoft.HISFC.Models.Order.Inpatient.Order order;
            DateTime sysTime = this.OrderManagement.GetDateTimeFromSysDateTime();
            for (int i = 0; i < this.fpSpread1_Sheet1.Rows.Count; i++)
            {
                order = this.GetObjectFromFarPoint(i, 0);
                //order = this.getOrderTermById(this.fpSpread1_Sheet1.Cells[i, this.myOrderClass.iColumns[37]].Text, 0);
                //order = this.GetObjectFromFarPoint(Neusoft.NFC.Function.NConvert.ToInt32(this.ActiveTempID), 0);

                if (order == null)
                {
                    continue;
                }
                if (order.Status == 3)
                {
                    continue;
                }
                if (order.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                {
                    continue;
                }
                if (this.helper != null)
                {
                    if (order.Frequency == null)
                    {
                        return;
                    }
                    order.Frequency = (Neusoft.HISFC.Models.Order.Frequency)helper.GetObjectFromID(order.Frequency.ID);
                }
                order.ApplyNo = this.OrderManagement.GetSequence("Order.Pass.Sequence");
                alOrder.Add(order);
            }
            for (int i = 0; i < this.fpSpread1_Sheet2.Rows.Count; i++)
            {
                order = this.GetObjectFromFarPoint(i, 1);
                //order = this.getOrderTermById(this.fpSpread1_Sheet2.Cells[i, this.myOrderClass.iColumns[37]].Text, 1);
                //order = this.GetObjectFromFarPoint(Neusoft.NFC.Function.NConvert.ToInt32(this.ActiveTempID), 1);

                if (order == null)
                {
                    continue;
                }
                if (order.Status == 3)
                {
                    continue;
                }
                if (order.MOTime.Date != sysTime.Date)
                {
                    continue;
                }
                if (order.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                {
                    continue;
                }
                if (this.helper != null)
                {
                    if (order.Frequency == null)
                    {
                        return;
                    }
                    order.Frequency = (Neusoft.HISFC.Models.Order.Frequency)helper.GetObjectFromID(order.Frequency.ID);
                }
                order.ApplyNo = this.OrderManagement.GetSequence("Order.Pass.Sequence");
                alOrder.Add(order);
            }
            if (alOrder.Count > 0)
            {
                this.PassSaveCheck(alOrder, checkType, warnPicFlag);
            }
        }

        /// <summary>
        /// ������ҩҽ�����
        /// </summary>
        /// <param name="alOrder">�����ҽ���б�</param>
        ///<param name="warnPicFlag">�Ƿ���ʾͼƬ������Ϣ</param>
        public void PassSaveCheck(List<Neusoft.HISFC.Models.Order.Inpatient.Order> alOrder, int checkType, bool warnPicFlag)
        {
            if (!this.IReasonableMedicine.PassEnabled)
            {
                return;
            }
            if (this.IReasonableMedicine.PassSaveCheck(this.myPatientInfo, alOrder, checkType) == -1)
            {
                MessageBox.Show("���ѱ���ҽ�����к�����ҩ������!");
            }
            if (!warnPicFlag)//������ʾ ֱ�ӷ���
            {
                return;
            }
            Neusoft.HISFC.Models.Order.Inpatient.Order tempOrder;
            int oCnt = alOrder.Count;
            int oIdx = 0;
            for (int i = 0; i < this.fpSpread1_Sheet1.Rows.Count; i++)
            {
                if (oIdx < oCnt)
                {

                    string orderId = alOrder[oIdx].ApplyNo;
                    //tempOrder = this.getOrderTermById(this.fpSpread1_Sheet1.Cells[i, this.myOrderClass.iColumns[37]].Text, 0);
                    //tempOrder = this.GetObjectFromFarPoint(Neusoft.NFC.Function.NConvert.ToInt32(this.ActiveTempID), 1);
                    tempOrder = this.GetObjectFromFarPoint(i, 0);


                    if (tempOrder == null)
                    {
                        continue;
                    }

                    if (tempOrder.Status == 3 || tempOrder.Item.SysClass.ID.ToString().Substring(0, 1) != "P")
                    {
                        // i--;
                        continue;
                    }

                    if (orderId == tempOrder.ApplyNo)
                    {
                        oIdx++;
                        int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);
                        //this.AddWarnPicturn(this.getOrderRowIndex(tempOrder.Oper.User03, 0), 0, iWarn);
                        this.AddWarnPicturn(i, 0, iWarn);
                    }
                }
                else
                {
                    break;
                }
            }
            for (int i = 0; i < this.fpSpread1_Sheet2.Rows.Count; i++)
            {
                if (oIdx < oCnt)
                {
                    string orderId = alOrder[oIdx].ApplyNo;
                    //tempOrder = this.getOrderTermById(this.fpSpread1_Sheet1.Cells[i, this.myOrderClass.iColumns[37]].Text, 0);
                    //tempOrder = this.GetObjectFromFarPoint(Neusoft.NFC.Function.NConvert.ToInt32(this.ActiveTempID), 1);
                    #region {8C389FCD-3E64-4a90-9830-BE220B952B53} 2010-12-09 �޸�
                    //tempOrder = this.GetObjectFromFarPoint(i, 0);
                    tempOrder = this.GetObjectFromFarPoint(i, 1);
                    #endregion

                    if (tempOrder == null)
                    {
                        continue;
                    }

                    if (tempOrder.Status == 3 || tempOrder.Item.SysClass.ID.ToString().Substring(0, 1) != "P")
                    {
                        // i--;
                        continue;
                    }

                    if (orderId == tempOrder.ApplyNo)
                    {
                        oIdx++;
                        int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);
                        //this.AddWarnPicturn(this.getOrderRowIndex(tempOrder.Oper.User03, 0), 0, iWarn);
                        this.AddWarnPicturn(i, 1, iWarn);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// ��Ӻ�����ҩ���������־
        /// </summary>
        /// <param name="iRow">������������</param>
        /// <param name="iSheet">������Sheet����</param>
        /// <param name="warnFlag">������־</param>
        public void AddWarnPicturn(int iRow, int iSheet, int warnFlag)
        {
            string picturePath = Application.StartupPath + "\\pic";
            switch (warnFlag)
            {
                case 0:										//0 (��ɫ)������
                    picturePath = picturePath + "\\0.gif";
                    break;
                case 1:										//1 (��ɫ)Σ���ϵͻ��в���ȷ
                    picturePath = picturePath + "\\1.gif";
                    break;
                case 2:										//2 (��ɫ)���Ƽ��������Σ��
                    picturePath = picturePath + "\\2.gif";
                    break;
                case 3:										// 3 (��ɫ)���Խ��ɡ������������Σ��
                    picturePath = picturePath + "\\3.gif";
                    break;
                case 4:										//4 (��ɫ)���û���һ��Σ�� 
                    picturePath = picturePath + "\\4.gif";
                    break;
                default:
                    break;
            }
            if (!System.IO.File.Exists(picturePath))
            {
                return;
            }
            try
            {
                System.Drawing.Color c;
                FarPoint.Win.Spread.CellType.TextCellType t = new FarPoint.Win.Spread.CellType.TextCellType();
                FarPoint.Win.Picture pic = new FarPoint.Win.Picture();
                pic.Image = System.Drawing.Image.FromFile(picturePath, true);
                pic.TransparencyColor = System.Drawing.Color.Empty;
                c = this.fpSpread1.Sheets[iSheet].RowHeader.Cells[iRow, 0].BackColor;
                t.BackgroundImage = pic;
                this.fpSpread1.Sheets[iSheet].RowHeader.Cells[iRow, 0].CellType = t;			//ҽ������
                this.fpSpread1.Sheets[iSheet].RowHeader.Cells[iRow, 0].Tag = "1";							//���������
                this.fpSpread1.Sheets[iSheet].RowHeader.Cells[iRow, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                this.fpSpread1.Sheets[iSheet].RowHeader.Cells[iRow, 0].BackColor = c;
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ú�����ҩ�������ʾ�����г���!" + ex.Message);
            }
        }


        /// <summary>
        /// �������ҩϵͳ���͵�ǰ����ѯҩƷ��Ϣ
        /// </summary>
        /// <param name="checkType">��ѯ��ʽ</param>
        public void PassTransDrug(int checkType)
        {
            if (!this.IReasonableMedicine.PassEnabled)
            {
                return;
            }
            //int iSheetIndex = this.OrderType == Neusoft.HISFC.Object.Order.EnumType.SHORT ? 1 : 0;
            int iSheetIndex = this.fpSpread1.ActiveSheetIndex;
            int iRow = this.fpSpread1.Sheets[iSheetIndex].ActiveRowIndex;
            //Neusoft.HISFC.Object.Order.Inpatient.Order info = this.getOrderTermById(this.ActiveTempID, iSheetIndex);
            Neusoft.HISFC.Models.Order.Inpatient.Order info = this.GetObjectFromFarPoint(Neusoft.FrameWork.Function.NConvert.ToInt32(this.ActiveTempID), iSheetIndex);
            if (info == null)
            {
                return;
            }
            if (info.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
            {
                this.IReasonableMedicine.ShowFloatWin(false);
                return;
            }
            this.IReasonableMedicine.ShowFloatWin(false);
            this.IReasonableMedicine.PassSetDrug(info.Item.ID, info.Item.Name, ((Neusoft.HISFC.Models.Pharmacy.Item)info.Item).DoseUnit,
                info.Usage.Name);
            this.IReasonableMedicine.DoCommand(checkType);
        }

        #endregion
        /// <summary>
        /// �������ѯ{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}pacs�ӿ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPacsView_Click(object sender, EventArgs e)
        {
            this.ShowPacsResultByPatient(this.myPatientInfo.ID);
        }
        //{D2BDB9B8-7D50-4a66-8D1C-28EA0420592F}���뵥
        private void checkSlip_Click(object sender, EventArgs e)
        {
            this.CheckSlip(this.fpSpread1.ActiveSheet.ActiveRowIndex);
        }

        private void cancelSlip_Click(object sender, EventArgs e)
        {
            this.CancelSlip(this.fpSpread1.ActiveSheet.ActiveRowIndex);
        }

        public void CheckSlip(int Index)
        {
            int i = Index;
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            if (i < 0 || this.fpSpread1.ActiveSheet.RowCount == 0)
            { 
                MessageBox.Show("����ѡ��һ��ҽ����");
                return ;
            }
            order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[i].Tag;     
            Neusoft.HISFC.Components.Order.Forms.frmCheckSlip ucCheckSlip= new Neusoft.HISFC.Components.Order.Forms.frmCheckSlip();
            ucCheckSlip.Order = order;
            ucCheckSlip.MyPatientInfo = this.myPatientInfo;
            ucCheckSlip.handler+=new Neusoft.HISFC.Components.Order.Forms.frmCheckSlip.EventHandler(ucCheckSlip_handler);
            ucCheckSlip.ShowDialog();
            
        }

        public void CancelSlip(int Index)
        {
            int i = Index;
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            if (i < 0 || this.fpSpread1.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("����ѡ��һ��ҽ����");
                return;
            }
            order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[i].Tag;
            Neusoft.HISFC.BizLogic.Order.CheckSlip checkSlip = new Neusoft.HISFC.BizLogic.Order.CheckSlip();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            checkSlip.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
             List<Neusoft.HISFC.Models.Order.CheckSlip> list = new List<Neusoft.HISFC.Models.Order.CheckSlip>();
             if ((((Neusoft.FrameWork.Models.NeuObject)(order)).ID).ToString() != "")
             {
                 list = checkSlip.QuerySlip(checkSlip.QueryByMoOrder(((Neusoft.FrameWork.Models.NeuObject)(order)).ID).ToString());
                 if (list.Count != 0)
                 {
                     if (checkSlip.Delete(list[0].ToString()) == -1)
                     {
                         if (checkSlip.UpdateMetIpmOrder(((Neusoft.FrameWork.Models.NeuObject)(order)).ID) == -1)
                         {
                             Neusoft.FrameWork.Management.PublicTrans.RollBack();
                             MessageBox.Show("������뵥ɾ��ʧ��");
                             return;
                         }
                     }
                     Neusoft.FrameWork.Management.PublicTrans.Commit();
                     MessageBox.Show("ɾ���ɹ�");
                 }
                 else
                 {
                     MessageBox.Show("�����뵥��Ϣ");
                 }
             }
             else
             {
                 if (order.ApplyNo.ToString() != "")
                 {
                     list = checkSlip.QuerySlip(order.ApplyNo.ToString());
                     if (checkSlip.Delete(list[0].ToString()) == -1)
                     {
                         Neusoft.FrameWork.Management.PublicTrans.RollBack();
                         MessageBox.Show("������뵥ɾ��ʧ��");
                         return;
                     }
                     Neusoft.FrameWork.Management.PublicTrans.Commit();
                     MessageBox.Show("ɾ���ɹ�");
                 }
                 else 
                 {
                     MessageBox.Show("�����뵥��Ϣ");
                 }
             }
        }

        void ucCheckSlip_handler(Neusoft.HISFC.Models.Order.CheckSlip obj)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = (Neusoft.HISFC.Models.Order.Inpatient.Order)this.fpSpread1.ActiveSheet.Rows[this.fpSpread1.ActiveSheet.ActiveRowIndex].Tag;
            
            order.ApplyNo = obj.CheckSlipNo;
            this.AddObjectToFarpoint(order, 0, this.fpSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
        }
        //{D2BDB9B8-7D50-4a66-8D1C-28EA0420592F}


        /// <summary>
        /// ճ��ҽ��{7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPasteOrder_Click(object sender, EventArgs e)
        {
            this.PasteOrder();
        }

        #endregion

        #region ���仯��Ҫ���⴦��
        private void ucInputItem1_CatagoryChanged(Neusoft.FrameWork.Models.NeuObject sender)
        {
            try
            {
                Neusoft.FrameWork.Models.NeuObject obj = sender as Neusoft.FrameWork.Models.NeuObject;
                if (obj.ID == Neusoft.HISFC.Models.Base.EnumSysClass.MRD.ToString())
                {
                    this.ShowTransferDept();

                }
                else if (obj.ID == Neusoft.HISFC.Models.Base.EnumSysClass.UN.ToString())
                {
                    //����

                }
                else if (obj.ID == Neusoft.HISFC.Models.Base.EnumSysClass.UC.ToString())
                {
                    //���

                }
                else
                {
                    return;
                }


            }
            catch { }
        }
       
        #endregion

        #region ����
        /// <summary>
        /// ��黥��
        /// </summary>
        /// <param name="sysClass"></param>
        /// <returns></returns>
        private int CheckMutex(Neusoft.HISFC.Models.Base.SysClassEnumService sysClass)
        {
            if (sysClass == null) return -1;
            ArrayList al = new ArrayList();
            if (this.fpSpread1.ActiveSheet.RowCount <= 0) return 0;
            for (int i = 0; i < this.fpSpread1.ActiveSheet.RowCount; i++)
            {

                Neusoft.HISFC.Models.Order.Inpatient.Order order = this.GetObjectFromFarPoint(i, this.fpSpread1.ActiveSheetIndex);
                if (order != null)
                {
                    if (order.Item.SysClass.ID.ToString() == sysClass.ID.ToString() && (order.Status == 1 || order.Status == 2))
                    {
                        al.Add(order);
                    }
                }
            }
            if (sysClass.ID.ToString() == "UO")  //���������ҽ�������λ��⣬by zuowy 2005-10-13
                return 0;
            try
            {
                Neusoft.HISFC.Models.Order.EnumMutex mutex = OrderManagement.QueryMutex(sysClass.ID.ToString());//��ѯ����

                if (mutex == Neusoft.HISFC.Models.Order.EnumMutex.SysClass)
                {
                    //ϵͳ��𻥳�
                    if (al.Count == 0) return 0;//���ϵͳ����Ƿ����ظ���
                 
                    //frmNeedDcOrderSelect f = new frmNeedDcOrderSelect();
                    //f.Tip = "�������µ�'" + sysClass.Name + "'ҽ����ѡ��ֹͣ��ǰ��" + sysClass.Name + "ҽ��:";
                    //f.alOrders = al;
                    //f.ShowDialog();
                    //RefreshOrderState(true);
                   
                }
                else if (mutex == Neusoft.HISFC.Models.Order.EnumMutex.All)
                {
                    //ҽ��ȫ������
                    if (MessageBox.Show("�������µ�'" + sysClass.Name + "'ҽ�����Ƿ�ֹͣ��ǰ��ȫ��ҽ��?", "����", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
                        //t.BeginTransaction();
                        OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        if (OrderManagement.DcOrder(this.myPatientInfo.ID, this.OrderManagement.GetDateTimeFromSysDateTime()) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            MessageBox.Show(OrderManagement.Err);
                            return -1;
                        }
                        Neusoft.FrameWork.Management.PublicTrans.Commit();

                        RefreshOrderState(true);
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("��û�����Ϣ����" + ex.Message, "��ʾ");
            }
            return 0;
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.myOrderClass.SaveGrid();
        }
        #endregion

        #region �¼ӵĺ���
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            toolBarService.AddToolButton("����", "����ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("���", "���ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("������", "��������", 9, true, false, null);
            toolBarService.AddToolButton("ɾ��", "ɾ��ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("ȡ�����", "ȡ�����ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("��ϸ", "������ϸ", 9, true, true, null);
            toolBarService.AddToolButton("�˳�ҽ������", "�˳�ҽ������", 9, true, false, null);
            return toolBarService;
        }
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "����")
            {
                this.Add();
            }
            else if (e.ClickedItem.Text == "���")
            {
                this.ComboOrder();
            }
        }

        private object currentObject = null;
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            #region by xizf ������ũ���Է��ʵ���ʾ
            ZZLocal.HISFC.BizLogic.Order.Order local_order = new ZZLocal.HISFC.BizLogic.Order.Order();
            #endregion
            if (neuObject.GetType() == typeof(Neusoft.HISFC.Models.RADT.PatientInfo))
            {
                if(currentObject != neuObject)
                    this.SetPatient(neuObject as Neusoft.HISFC.Models.RADT.PatientInfo);
                currentObject = neuObject;

                if (this.myPatientInfo != null)
                {
                    #region ������ͳһ���㷨 {9BE8D34E-752D-4d32-A37C-87C62A949C55} wbo 2010-10-23
                    //this.lbPatient.Text = "����: " + this.myPatientInfo.Name + " " +
                    //    "�Ա�: " + this.myPatientInfo.Sex.Name + " " + "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday) + "  ��ͬ��λ��" +
                    //this.myPatientInfo.Pact.Name + " �����ܶ�: " + myPatientInfo.FT.TotCost.ToString() + " Ѻ���ܶ�: " + this.myPatientInfo.FT.PrepayCost.ToString() +
                    //" ���: " + this.myPatientInfo.FT.LeftCost.ToString();
                    string strAge = "";
                    try
                    {
                        strAge = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.myPatientInfo.Birthday);
                    }
                    catch (Exception ex)
                    { }
                    this.lbPatient.Text = "����: " + this.myPatientInfo.Name + " " +
                        "�Ա�: " + this.myPatientInfo.Sex.Name + " " + "  ���䣺" + strAge + "  ��ͬ��λ��" +
                    this.myPatientInfo.Pact.Name + " �����ܶ�: " + myPatientInfo.FT.TotCost.ToString() + " Ѻ���ܶ�: " + this.myPatientInfo.FT.PrepayCost.ToString() +
                    " ���: " + this.myPatientInfo.FT.LeftCost.ToString();
                    #endregion
                    #region �����Է��� by xizf 20110113{836418D2-B0E1-41a6-8102-EFB32F411A9D}
                    string temp_ratio =  local_order.QueryZFRatio(this.myPatientInfo.ID,this.myPatientInfo.Pact.ID);
                    this.lbPatient.Text += " �ο��Է���:" + temp_ratio;
                    #endregion

                    #region addby xuewj 2010-10-1 ��ӵ�ǰ����������� {B521EF65-812B-40c8-A774-84A838926355}
                    this.lbTempTotCost.Text = "��ǰ�����������: 0.00"; 
                    #endregion

                    #region {AFD4A961-4687-4af6-8EFF-A42EDA3FD636}
                    this.plPatient.Visible = true;
                    #endregion
                }
                #region {AFD4A961-4687-4af6-8EFF-A42EDA3FD636}
                else
                {
                    this.plPatient.Visible = false;
                }
                #endregion
            }
            return 0;
        }
        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] t = new Type[7];
                t[0] = typeof(Neusoft.HISFC.BizProcess.Interface.IPrintOrder);
                t[1] = typeof(Neusoft.HISFC.BizProcess.Interface.ITransferDeptApplyable);
                t[2] = typeof(Neusoft.HISFC.BizProcess.Interface.Common.ILis);
                t[3] = typeof(Neusoft.HISFC.BizProcess.Interface.IAlterOrder);
                t[4] = typeof(Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint);//������뵥
                t[5] = typeof(Neusoft.HISFC.BizProcess.Interface.Common.IPacs);//pacs{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
                t[6] = typeof(Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine);
                return t;
            }
        }        
        /// <summary>
        /// ��ӡ
        /// </summary>
        public void Print()
        {
            Neusoft.HISFC.BizProcess.Interface.IPrintOrder o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.Controls.ucOrder), typeof(Neusoft.HISFC.BizProcess.Interface.IPrintOrder)) as Neusoft.HISFC.BizProcess.Interface.IPrintOrder;
            if (o == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
                p.PrintPreview(this.panelOrder);
            }
            else
            {
                o.SetPatient(this.myPatientInfo);
                o.ShowPrintSet();
            }
            

        }

        protected override int OnPrint(object sender, object neuObject)
        {
            Print();
            return 0;
        }

        /// <summary>
        /// ��ʾת������
        /// </summary>
        public void ShowTransferDept()
        {
            if (this.ucItemSelect1.SelectedOrderType == null) return;
            Neusoft.HISFC.BizProcess.Interface.ITransferDeptApplyable o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.Controls.ucOrder), typeof(Neusoft.HISFC.BizProcess.Interface.ITransferDeptApplyable)) as Neusoft.HISFC.BizProcess.Interface.ITransferDeptApplyable;
            if (o == null)
            {
                return;
            }
            else
            {
                o.SetPatientInfo(this.myPatientInfo);
                if (o.ShowDialog() == DialogResult.OK)
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                    Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();

                    order.OrderType = (Neusoft.HISFC.Models.Order.OrderType)this.ucItemSelect1.SelectedOrderType.Clone();
                    order.Item = item;
                    order.Item.SysClass.ID = "MRD";
                    order.Item.ID = "999";
                    order.Qty = 1;
                    order.Unit = "��";
                    order.Item.Name = o.Dept.Name+ "[ת��]";
                    order.ExeDept = o.Dept.Clone();
                    order.Frequency.ID = "QD";

                    this.AddNewOrder(order, this.fpSpread1.ActiveSheetIndex);
                }
            }
            
        }

        /// <summary>
        /// ��ʾLIS���
        /// </summary>
        public void ShowLisResult()
        {
            Neusoft.HISFC.BizProcess.Interface.Common.ILis o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.Controls.ucOrder), typeof(Neusoft.HISFC.BizProcess.Interface.Common.ILis)) as Neusoft.HISFC.BizProcess.Interface.Common.ILis;
            if (o == null)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("û��ά��LIS�ӿڣ�"));
            }
            else
            {
                o.ShowResultByPatient(this.myPatientInfo.ID);
            }
        }

        /// <summary>
        /// ��ʼ��ҽ����Ϣ����ӿ�ʵ��
        /// </summary>
        protected void InitAlterOrderInstance()
        {
            if (this.IAlterOrderInstance == null)
            {
                this.IAlterOrderInstance = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.Controls.ucOrder), typeof(Neusoft.HISFC.BizProcess.Interface.IAlterOrder)) as Neusoft.HISFC.BizProcess.Interface.IAlterOrder;
            }

            //TestAlterInsterface t = new TestAlterInsterface();
            //this.IAlterOrderInstance = t as Neusoft.HISFC.BizProcess.Integrate.IAlterOrder;
        }
        
        #endregion

        /// <summary>
        /// �϶�ʱ����Ϊxml��ʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], this.myOrderClass.LONGSETTINGFILENAME);
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[1], this.myOrderClass.SHORTSETTINGFILENAME);
        }

        #region ҽ������   {FB86E7D8-A148-4147-B729-FD0348A3D670} ���Ӻ���

        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <returns></returns>
        public int ReTidyOrder()
        {
            #region {74E478F5-BDDD-4637-9F5A-E251AF9AA72F}
            if (this.myPatientInfo == null)
            {
                MessageBox.Show("����ѡ����!");
                return -1;
            }
            #endregion

            DialogResult rs = MessageBox.Show("ȷ�Ͻ���ҽ������������ҽ����ֹͣ���ؿ���ǰ��Чҽ������������ֹͣҽ��", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.No)
            {
                return 0;
            }

            List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();

            for (int i = 0; i < this.fpSpread1_Sheet1.Rows.Count; i++)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order info = this.fpSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;

                orderList.Add(info);
            }
            int result = this.ReTidyOrder(orderList);
            this.QueryOrder();
            return result;
        }

        internal int ReTidyOrder(List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            //{D05BA7C4-3158-48aa-B581-0211E2CAAD4C} 
            #region ��ȡ����ҽ������ʽ ��ʽһ������ԭҽ�� ��������״̬ҽ��   ��ʽ�����޸�ԭҽ��Ϊ����״̬ ������Чҽ��

            int retidyType = 2;     //Ĭ�Ϸ�ʽ��

            Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
            retidyType = ctrlIntegrate.GetControlParam<int>(Neusoft.HISFC.BizProcess.Integrate.MetConstant.Order_RetidyType,true,2);

            #endregion

            int maxSortID = 3000;

            Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();
            ArrayList alOrder = orderManager.QueryOrder(this.myPatientInfo.ID);
            if (alOrder == null)
            {
                MessageBox.Show("��ѯҽ����Ϣʧ�ܣ�" + orderManager.Err);
                return -1;
            }

            #region addby xuewj 2010-9-20 ���߲�����ҽ��ʱ������ {AE3BD15D-28A6-4df6-8DBB-6DEB898D190C} 
            if (alOrder.Count == 0)
            {
                MessageBox.Show("�û���û����Ҫ������ҽ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            #endregion

            //����һ��ҽ����¼ ���γ�ҽ�����
            Neusoft.HISFC.Models.Order.Inpatient.Order order = alOrder[alOrder.Count - 1] as Neusoft.HISFC.Models.Order.Inpatient.Order;
            maxSortID = order.SortID + 10;

            //�ж��Ƿ������������ ���γɴ�����ҽ���б�
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> longOrderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();
            //��ǰ��Чҽ���б�
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> validOrderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();
            //ֹͣҽ���б�
            List<Neusoft.HISFC.Models.Order.Inpatient.Order> DcOrderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();

            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order obj in alOrder)
            {
                if (obj.OrderType.IsDecompose && (obj.Status == 0))
                {
                    MessageBox.Show("����δ��˵�ҽ�������ܽ���ҽ��������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return -1;
                }
                if (obj.OrderType.IsDecompose)
                {
                    longOrderList.Add(obj);
                    if (obj.Status == 1 || obj.Status == 2)      //ԭ��Чҽ��
                    {
                        validOrderList.Add(obj);
                    }
                    else if (obj.Status == 3)
                    {
                        DcOrderList.Add(obj);
                    }
                }
            }

            ArrayList alUnconfirmOrder = OrderManagement.QueryIsConfirmOrder(this.myPatientInfo.ID, Neusoft.HISFC.Models.Order.EnumType.LONG, false);
            if (alUnconfirmOrder == null)
            {
                MessageBox.Show("��ѯδ���ҽ���ǳ���!\n" + OrderManagement.Err);
                return -1;
            }
            if (alUnconfirmOrder.Count > 0)
            {
                MessageBox.Show("�����¿�������ֹͣ��δ��˵�ҽ�������ܽ���ҽ��������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }

            #region ��ԭ��Чҽ���γ���ҽ��

            List<Neusoft.HISFC.Models.Order.Inpatient.Order> newOrderList = new List<Neusoft.HISFC.Models.Order.Inpatient.Order>();
            string comboNO = string.Empty;
            string comboNOTemp = string.Empty;

            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in validOrderList)
            {
                Neusoft.HISFC.Models.Order.Inpatient.Order newOrderTemp = info.Clone();

                if (newOrderTemp.Combo.ID == comboNO)
                {
                    newOrderTemp.Combo.ID = comboNOTemp;
                }
                else
                {
                    comboNO = newOrderTemp.Combo.ID;
                    comboNOTemp = orderManager.GetNewOrderComboID();
                    newOrderTemp.Combo.ID = comboNOTemp;

                    maxSortID++;
                }

                newOrderTemp.SortID = maxSortID;

                newOrderTemp.ExtendFlag3 = "����ҽ��  ԭҽ����ˮ�ţ�" + newOrderTemp.ID.ToString();

                newOrderTemp.ID = orderManager.GetNewOrderID();

                newOrderList.Add(newOrderTemp);
            }

            #endregion

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            DateTime sysTime = orderManager.GetDateTimeFromSysDateTime();

            //{D05BA7C4-3158-48aa-B581-0211E2CAAD4C}
            if (retidyType == 2)            //��ʽ��  ԭ��Чҽ�����Ϊ����״̬ ������ҽ��ͬԭ��Чҽ��
            {
                #region ��ԭ����Чҽ��ȫ��ͣ��

                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in validOrderList)
                {
                    info.Status = 3;

                    info.DCOper.ID = orderManager.Operator.ID;
                    info.DCOper.Name = orderManager.Operator.Name;
                    info.DCOper.OperTime = sysTime;

                    info.DcReason.ID = "RT";
                    info.DcReason.Name = "ҽ������";

                    info.EndTime = info.DCOper.OperTime;

                    if (orderManager.DcOneOrder(info) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("ֹͣԭ��Чҽ��ʧ��:" + orderManager.Err);
                        return -1;
                    }
                }

                #endregion

                #region ҽ������

                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in longOrderList)
                {
                    info.Status = 4;                //ҽ������״̬
                    info.Oper.ID = orderManager.Operator.ID;
                    info.Oper.Name = orderManager.Operator.Name;
                    info.Oper.OperTime = sysTime;

                    if (orderManager.OrderReform(info.ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ԭҽ��ʧ��:" + orderManager.Err);
                        return -1;
                    }
                }

                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in validOrderList)
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order newOrderTemp = info.Clone();

                    if (newOrderTemp.Combo.ID == comboNO)
                    {
                        newOrderTemp.Combo.ID = comboNOTemp;
                    }
                    else
                    {
                        comboNO = newOrderTemp.Combo.ID;
                        comboNOTemp = orderManager.GetNewOrderComboID();
                        newOrderTemp.Combo.ID = comboNOTemp;

                        maxSortID++;
                    }

                    newOrderTemp.SortID = maxSortID;

                    newOrderTemp.ExtendFlag3 = "����ҽ��  ԭҽ����ˮ�ţ�" + newOrderTemp.ID.ToString();

                    newOrderTemp.ID = orderManager.GetNewOrderID();

                    newOrderList.Add(newOrderTemp);
                }

                #endregion
            }
            else                          //��ʽһ  ԭ��Чҽ����Ϣ���� ��������״̬��ҽ��(��Ϣͬԭ��Чҽ����״̬��ͬ)
            {

                //ֹͣҽ����Ϊ����״̬ {A3B78606-5301-4680-9CF4-08B6545D6608} 20100528
                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in DcOrderList)
                {
                    info.Status = 4;                //ҽ������״̬
                    info.Oper.ID = orderManager.Operator.ID;
                    info.Oper.Name = orderManager.Operator.Name;
                    info.Oper.OperTime = sysTime;

                    if (orderManager.OrderReform(info.ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ԭҽ��ʧ��:" + orderManager.Err);
                        return -1;
                    }
                }

                foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in newOrderList)
                {
                    info.Status = 4;               //ҽ������״̬
                    info.Oper.ID = orderManager.Operator.ID;
                    info.Oper.Name = orderManager.Operator.Name;
                    info.Oper.OperTime = sysTime;
                }
            }
           
            #region ����ҽ�����б���

            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order info in newOrderList)
            {
                if (orderManager.InsertOrder(info) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("������ҽ��ʧ��:" + orderManager.Err);
                    return -1;
                }
            }

            #endregion

            #region ����������¼

            Neusoft.FrameWork.Management.ExtendParam extendManager = new Neusoft.FrameWork.Management.ExtendParam();
            Neusoft.HISFC.Models.Base.ExtendInfo extendInfo = new ExtendInfo();

            extendInfo.DateProperty = sysTime;
            extendInfo.ExtendClass = EnumExtendClass.PATIENT;
            extendInfo.Item.ID = this.myPatientInfo.ID;
            extendInfo.PropertyCode = sysTime.ToString();
            extendInfo.PropertyName = "����ʱ��";

            extendInfo.StringProperty = orderManager.Operator.ID;
            extendInfo.DateProperty = sysTime;

            if (extendManager.InsertComExtInfo(extendInfo) == -1)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("ҽ�����������¼ʧ��:" + extendManager.Err);
                return -1;
            }

            #endregion

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            MessageBox.Show("ҽ�������ɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return 1;
        }

        #endregion

        /// <summary>
        /// ����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
        /// </summary>
        public void Clear()
        {
            this.ucItemSelect1.Clear();
        }

        private void fpSpread1_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader && e.Column == this.myOrderClass.iColumns[35] && this.EnabledPass)
            {
                if (!this.IReasonableMedicine.PassEnabled)
                {
                    return;
                }

                int iSheetIndex = this.OrderType == Neusoft.HISFC.Models.Order.EnumType.SHORT ? 1 : 0;              
                Neusoft.HISFC.Models.Order.Inpatient.Order info = this.GetObjectFromFarPoint(Neusoft.FrameWork.Function.NConvert.ToInt32(this.ActiveTempID), iSheetIndex);

                if (info == null)
                {
                    return;
                }
                if (info.Item.ItemType.ToString() != Neusoft.HISFC.Models.Base.EnumItemType.Drug.ToString())
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    return;
                }
                this.IReasonableMedicine.ShowFloatWin(false);
                if (e.Column == 0)
                {
                    if (this.fpSpread1.Sheets[iSheetIndex].RowHeader.Cells[e.Row, 0].Tag != null && this.fpSpread1.Sheets[iSheetIndex].RowHeader.Cells[e.Row, 0].Tag.ToString() != "0")
                    {
                        this.IReasonableMedicine.PassGetWarnInfo(info.ApplyNo, "0");
                    }
                }
            }
            else
            {
                try
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    this.PassSetQuery(e);
                }
                catch (Exception eee)
                {
                }
            }
                     
        }

       
    }

    /// <summary>
    /// �ӿ�ʵ�ֲ���
    /// </summary>
    public class TestAlterInsterface : Neusoft.HISFC.BizProcess.Interface.IAlterOrder
    {

        #region IAlterOrder ��Ա

        /// <summary>
        /// ҽ����Ϣ����
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="recipeDoc">����ҽ��</param>
        /// <param name="recipeDept">��������</param>
        /// <param name="order">ҽ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int AlterOrder(Neusoft.HISFC.Models.Registration.Register patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// ҽ����Ϣ����
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="recipeDoc">����ҽ��</param>
        /// <param name="recipeDept">��������</param>
        /// <param name="orderList">ҽ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int AlterOrder(Neusoft.HISFC.Models.Registration.Register patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.OutPatient.Order> orderList)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// ҽ����Ϣ����
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="recipeDoc">����ҽ��</param>
        /// <param name="recipeDept">��������</param>
        /// <param name="order">ҽ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int AlterOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref Neusoft.HISFC.Models.Order.Inpatient.Order order)
        {
            if (order.Usage.ID == "03")
            {
                order.StockDept.ID = "0277";
                order.StockDept.Name = "ҩѧ��";
            }

            return 1;
        }

        /// <summary>
        /// ҽ����Ϣ����
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="recipeDoc">����ҽ��</param>
        /// <param name="recipeDept">��������</param>
        /// <param name="orderList">ҽ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int AlterOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in orderList)
            {
                if (order.Status == 0)
                {
                    order.ExtendFlag2 = "Test AlterOrder";
                    order.Note = "Add New Note";         //�����ʾ
                }
            }

            return 1;
        }

        #endregion

        #region IAlterOrder ��Ա


        public int AlterOrderOnSaved(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int AlterOrderOnSaving(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
    
    /// <summary>
    /// ҽ������
    /// </summary>
    public enum EnumFilterList
    {
        All = 0,
        Today = 1,
        Valid = 2,
        Invalid = 3,
        New = 4
    }
}
