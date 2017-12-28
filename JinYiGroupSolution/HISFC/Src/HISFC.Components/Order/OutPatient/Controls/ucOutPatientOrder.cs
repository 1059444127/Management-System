using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.BizProcess.Interface.Order;

namespace Neusoft.HISFC.Components.Order.OutPatient.Controls
{
    public partial class ucOutPatientOrder : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucOutPatientOrder()
        {
            InitializeComponent();
            this.contextMenu1 = new Neusoft.FrameWork.WinForms.Controls.NeuContextMenuStrip();
            
        }

        #region ����

        private DataSet dtOrder = null; //��ǰDataSet

        protected DataSet dtQur = null;//��ѯ״̬DataSet
        protected DataView dvQur = null;

        private int MaxSort = 0;//���˳���
        protected bool EditGroup = false;//�Ƿ�������ױ༭����
        protected bool bDealULSub = false;//�Ƿ�����鸽��
        protected bool bSingleDealEmrOrder = false;//��ҩƷ�������Ƿ񵥶�����Ӽ�ҽ��
        protected string EmrSubUsage = "";//�Ӽ�ҽ��ִ�з�ʽ
        protected string ULOrderUsage = "";//����ҽ���Ϲܵ�ִ�з�ʽ
        protected bool dirty = false; //�Ƿ��¼ӣ��޸�ʱ��
        protected bool bSaveOrderHistory = false;//�Ƿ񱣴�ҽ���޸ļ�¼
        protected bool bTempVar = false;//�Ƿ��ڿ���״̬�����������β�ѯ���ݿ�
        protected bool bCanInSameRecipe = true;//�Ƿ�������ҩ�ͺ���ҩ��ͬһ���� 1 ���� 0 ������
        protected bool bCanAddOrder = true;//�Ƿ������������Է���Ŀ������ͬһ���� 1 Yes 0 No
        private string varCombID = "";//��ʱ����Ϻű���
        private string varTempUsageID = "zuowy";//��ʱ�÷�
        private string varOrderUsageID = "maokb";//ҽ���÷�
        protected object tempControler;//��ʱ���Ʋ���
        private Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint checkPrint = null;
        public int validDays = 1;//�Һ���Ч����--�շ�ʱҲ��ʹ��
        public bool bPrintViewRecipe = false;//�Ƿ��ӡԤ������
        //{BFDA551D-7569-47dd-85C4-1CA21FE494BD}
        /// <summary>
        /// ҽ��Ȩ����֤
        /// </summary>
        protected bool isCheckPopedom = false; 
        /// <summary>
        /// �Ƿ��޸Ĺ�ҽ��
        /// </summary>
        private bool isEdit = false;

        protected ArrayList alDepts = null;
        private ArrayList alTemp = new ArrayList();//��ʱ���ҽ����Ϣ
        public ArrayList alAllOrder = new ArrayList();//ȫ��ҽ����Ϣ	

        protected Neusoft.HISFC.Models.Order.OutPatient.Order currentOrder = null;
        protected Neusoft.HISFC.Models.Registration.Register myPatientInfo = new Neusoft.HISFC.Models.Registration.Register();
        protected Neusoft.FrameWork.Models.NeuObject currentRoom = null;//��ǰ��̨

        //{6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
        protected Neusoft.ApplyInterface.HisInterface PACSApplyInterface = null;

        /// <summary>
        /// {1EB2DEC4-C309-441f-BCCE-516DB219FD0E} 
        /// </summary>
        private Neusoft.HISFC.BizLogic.Manager.ItemLevel itemLevelManager = new Neusoft.HISFC.BizLogic.Manager.ItemLevel();

        #region ί���¼�
        public delegate void EventButtonHandler(bool b);
        public event EventButtonHandler OrderCanCancelComboChanged;//ҽ���Ƿ����ȡ������¼�
        public event EventButtonHandler OrderCanOperatorChanged;	//ҽ���Ƿ���Ե����������
        public event EventButtonHandler OrderCanSetCheckChanged;//�Ƿ�ɴ�ӡ��鵥�¼�
        public delegate void OrderQtyChangedHandler(Neusoft.HISFC.Models.Registration.Register rInfo, Neusoft.FrameWork.Management.Transaction trans);
        public event OrderQtyChangedHandler SetFeeDisplay;
        #endregion

        #region ҵ���

        /// <summary>
        /// ҽ��ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizLogic.Order.OutPatient.Order OrderManagement = new Neusoft.HISFC.BizLogic.Order.OutPatient.Order();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee feeManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ��ҩƷҵ��
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee itemManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ��ҩƷ�����Ŀҵ��
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee undrugztManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ����ҵ��
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Manager assignManagement = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        /// <summary>
        /// סԺ���ת
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.RADT radtManger = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        /// <summary>
        /// �Һ�ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Registration.Registration regManagement = new Neusoft.HISFC.BizProcess.Integrate.Registration.Registration();

        /// <summary>
        /// ҩƷҵ��
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Pharmacy pManagement = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();

        ///// <summary>
        ///// ��������ҵ��
        ///// </summary>
        //private Neusoft.FrameWork.Management.ControlParam controlManager = new Neusoft.FrameWork.Management.ControlParam();

        protected Neusoft.FrameWork.Public.ObjectHelper orderHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        /// <summary>
        /// �ն�ȷ��ҵ���
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm confrimIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm();
        #endregion
        protected FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
        private string SetingFileName = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath + @".\clinicordersetting.xml";

        ToolTip tooltip = new ToolTip();
        /// <summary>
        /// �Ҽ��˵�
        /// </summary>
        private Neusoft.FrameWork.WinForms.Controls.NeuContextMenuStrip contextMenu1 = null;
        private Neusoft.FrameWork.Public.ObjectHelper helper; //��ǰHelper

        private Forms.frmInputInjectNum formInputInjectNum = null;

        /// <summary>
        /// ҽ����Ϣ����ӿ�{48E6BB8C-9EF0-48a4-9586-05279B12624D}
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.IAlterOrder IAlterOrderInstance = null;
        #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

        Employee empl = FrameWork.Management.Connection.Operator as Employee;
        IReasonableMedicine IReasonableMedicine = null;

        #endregion
        #region {0733E2AD-EB02-4b6f-BCF8-1A6ED5A2EFAD}
        private string hypotestMode = "1";
        #endregion

        /// <summary>
        /// �洢��ϱ仯��ҽ���Ĺ�ϣ��
        /// {F67E089F-1993-4652-8627-300295AAED8C}
        /// </summary>
        private Hashtable hsComboChange = new Hashtable();

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        /// <summary>
        /// ����������
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
        /// <summary>
        /// true�ն��շ� false�����շ�
        /// </summary>
        //private bool accountProcess = false;

        /// <summary>
        /// ��������ʱˢ��������
        /// </summary>
        public event EventHandler OnRefreshGroupTree;

        /// <summary>
        /// ֱ���շѽӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.FeeInterface.IDoctIdirectFee IDoctFee = null;

        /// <summary>
        /// ҽ��վ���Ĵ���ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.Order.IDealSubjob IDealSubjob = null;

        private Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        bool isUseDL = false;//addby xuewj 2010-11-11 �������뵥��ȡ���������ļ� {457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
        #endregion

        #region ����

        protected bool bIsDesignMode = false;
        protected bool bIsShowPopMenu = true;

        /// <summary>
        /// �Ҽ��˵�
        /// </summary>
        public bool IsShowPopMenu
        {
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
                this.ucOutPatientItemSelect1.IsLisDetail = value;
            }
        }

        /// <summary>
        /// �Ƿ���ģʽ
        /// </summary>
        [DefaultValue(false), Browsable(false)]
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

                    this.SetFP();
                    this.QueryOrder();
                }
            }
        }
        
        private void SetFP()
        {
            this.ucOutPatientItemSelect1.Visible = this.bIsDesignMode;
        }

        /// <summary>
        /// ���߻�����Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.Registration.Register Patient
        {
            get
            {
                return this.myPatientInfo;
            }
            set
            {
                this.myPatientInfo = value;
                this.ucOutPatientItemSelect1.PatientInfo = value;
                this.QueryOrder();
            }
        }

        /// <summary>
        /// ��ǰ��̨
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject CurrentRoom
        {
            get
            {
                return this.currentRoom;
            }
            set
            {
                this.currentRoom = value;
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
        
        public Neusoft.FrameWork.Models.NeuObject GetReciptDoc()
        {
            try
            {
                if (this.myReciptDoc == null) this.myReciptDoc = OrderManagement.Operator.Clone();
            }
            catch { }
            return this.myReciptDoc;
        }
        
        /// <summary>
        /// ���߿������,�б��ڹҺſ���
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject SeeDept = null;

        /// <summary>
        /// �Ƿ����������ҩ���
        /// </summary>
        public bool EnabledPass = true ;
        /// <summary>
        /// �Ƿ���ʱҽ��״̬
        /// </summary>
        public bool bOrderHistory = false;


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
                this.ucOutPatientItemSelect1.Init();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitControl();
            //{AB19F92E-9561-4db9-A0CF-20C1355CD5D8}
            InitDirectFee();

            InitDealSubJob();
            try
            {
                #region ��ȡ���Ʋ���
                Neusoft.FrameWork.Management.ControlParam controler = new Neusoft.FrameWork.Management.ControlParam();

                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200019");
                if (this.tempControler != null)
                {
                    this.bCanInSameRecipe = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200020");
                if (this.tempControler != null)
                {
                    this.bCanAddOrder = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200005");
                if (this.tempControler != null)
                {
                    this.bSingleDealEmrOrder = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200006");
                if (this.tempControler != null)
                {
                    this.EmrSubUsage = ((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue;
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200007");
                if (this.tempControler != null)
                {
                    this.ULOrderUsage = ((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue;
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200022");
                if (this.tempControler != null)
                {
                    this.validDays = Neusoft.FrameWork.Function.NConvert.ToInt32(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200000");
                if (this.tempControler != null)
                {
                    this.bDealULSub = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200023");
                if (this.tempControler != null)
                {
                    this.bPrintViewRecipe = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200021");
                if (this.tempControler != null)
                {
                    this.bSaveOrderHistory = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                //ҽ��Ȩ����֤����//{BFDA551D-7569-47dd-85C4-1CA21FE494BD}
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200039");
                if (this.tempControler != null)
                {
                    this.isCheckPopedom = Neusoft.FrameWork.Function.NConvert.ToBoolean(((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue);
                }
                #region {3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
                Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamManager = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
                this.enabledPacs = controlParamManager.GetControlParam<bool>("200202");
                #endregion
                #region {0733E2AD-EB02-4b6f-BCF8-1A6ED5A2EFAD}
                //Ƥ�Դ���ģʽ
                this.tempControler = Classes.Function.controlerHelper.GetObjectFromID("200201");
                if (this.tempControler != null)
                {
                    this.hypotestMode = ((Neusoft.HISFC.Models.Base.Controler)tempControler).ControlerValue.ToString();
                }
                #endregion
                #region addby xuewj 2010-11-11 �������뵥��ȡ���������ļ�{457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
                isUseDL = Neusoft.HISFC.Components.Common.Classes.Function.LoadMenuSet(); 
                #endregion

                //this.accountProcess = ctrlIntegrate.GetControlParam<bool>("S00031", true, false);
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

                this.ucOutPatientItemSelect1.OrderChanged += new ItemSelectedDelegate(ucItemSelect1_OrderChanged);
                ////this.ucOutPatientItemSelect1.CatagoryChanged += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucOutPatientItemSelect1_CatagoryChanged);

                this.neuSpread1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
                this.neuSpread1.Sheets[0].DataAutoSizeColumns = false;
                
                this.neuSpread1.Sheets[0].DataAutoCellTypes = false;
                
                this.neuSpread1.Sheets[0].GrayAreaBackColor = Color.White;
                
                this.neuSpread1.Sheets[0].RowHeader.Columns.Get(0).Width = 15;
                
                this.neuSpread1.Sheets[0].RowHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;

                this.neuSpread1_Sheet1.RowHeader.DefaultStyle.Border = new FarPoint.Win.BevelBorder(FarPoint.Win.BevelBorderType.Raised);
                this.neuSpread1_Sheet1.RowHeader.DefaultStyle.CellType = new FarPoint.Win.Spread.CellType.TextCellType();
                //��ʼ��PACS{3CF92484-7FB7-41d6-8F3F-38E8FF0BF76A}
                if (this.enabledPacs)
                {
                    this.InitPacsInterface();
                }
                ////this.OrderType = Neusoft.HISFC.Models.Order.EnumType.SHORT;
                this.neuSpread1.ActiveSheetIndex = 0;
            }
            catch { }

            base.OnStatusBarInfo(null, "(��ɫ���¿�)(��ɫ���շ�)");
            
            Classes.Function.SethsUsageAndSub();
            #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

            this.InitReasonableMedicine();

            if (this.IReasonableMedicine == null)
            {
                  return;
            }

            int iReturn = 0;
            iReturn = this.IReasonableMedicine.PassInit(empl.ID, empl.Name, empl.Dept.ID, empl.Dept.Name, 10, true);
            //MessageBox.Show(iReturn.ToString());
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
            if (this.enabledPacs == true && this.isInitPacs == false)
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
        public void RelePacsInterface()
        {
            if (this.pacsInterface == null)
            {
                return;
            }
            if (this.isInitPacs == false)
            {
                return;
            }
            if (this.enabledPacs == false)
            {
                return;
            }
            this.pacsInterface.Disconnect();
        }
        #endregion
        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        private void InitControl()
        {
            //Ĭ�ϲ���ģʽ--ҽ������ģʽ
            this.ucOutPatientItemSelect1.OperatorType = Operator.Add;

            this.dtQur = this.InitDataSet();
            this.neuSpread1.Sheets[0].DataSource = this.dtQur.Tables[0];

            this.SetColumnName(0);

            this.ColumnSet();
            this.SetFP();
            this.InitFP();

            #region FarPoint �¼�
            this.neuSpread1.MouseUp += new MouseEventHandler(neuSpread1_MouseUp);
            this.neuSpread1.Sheets[0].Columns[-1].CellType = new FarPoint.Win.Spread.CellType.TextCellType();

            this.neuSpread1.SelectionChanged += new FarPoint.Win.Spread.SelectionChangedEventHandler(neuSpread1_SelectionChanged);
            
            this.neuSpread1.Sheets[0].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(neuSpread1_Sheet1_CellChanged);
            
            #endregion

        }
        //{AB19F92E-9561-4db9-A0CF-20C1355CD5D8}
        /// <summary>
        /// ��ʼ��ֱ���շѽӿ�
        /// </summary>
        private void InitDirectFee()
        {
            if (IDoctFee == null)
            {
                IDoctFee = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.OutPatient.Controls.ucOutPatientOrder), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IDoctIdirectFee)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IDoctIdirectFee;
            }
        }

        /// <summary>
        /// ���Ĵ���ӿ�
        /// </summary>
        private void InitDealSubJob()
        {
            if (IDealSubjob == null)
            {
                IDealSubjob = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.OutPatient.Controls.ucOutPatientOrder), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IDealSubjob)) as Neusoft.HISFC.BizProcess.Interface.Order.IDealSubjob;
            }
        }
        

        /// <summary>
        /// ��ʼ��Fp
        /// </summary>
        private void InitFP()
        {

            this.SetColumnName(0);
                        
            try
            {
                this.SetColumnProperty();
            }
            catch { }
            
        }

        /// <summary>
        /// ��ʼ��dataset
        /// </summary>
        /// <returns></returns>
        private DataSet InitDataSet()
        {
            try
            {
                dtOrder = new DataSet();
                Type dtStr = System.Type.GetType("System.String");
                Type dtDbl = typeof(System.Double);
                Type dtInt = typeof(System.Decimal);
                Type dtBoolean = typeof(System.Boolean);
                Type dtDate = typeof(System.DateTime);
                
                DataTable table = new DataTable("Table");
                table.Columns.AddRange(new DataColumn[]
				{
					new DataColumn("!",dtStr),     //0
					new DataColumn("��",dtStr),     //0
					new DataColumn("ҽ������",dtStr),//1
					new DataColumn("ҽ����ˮ��",dtStr),//2
					new DataColumn("ҽ��״̬",dtStr),//�¿�������ˣ�ִ��
					new DataColumn("��Ϻ�",dtStr),//4
					new DataColumn("��ҩ",dtStr),//5
					new DataColumn("ҽ������",dtStr),//6
					new DataColumn("���",dtStr),     //0
					new DataColumn("����",dtInt),//7
					new DataColumn("������λ",dtStr),//8
					new DataColumn("ÿ������",dtDbl),//9
					new DataColumn("��λ",dtStr),//10
					new DataColumn("����",dtStr),//11
					new DataColumn("Ƶ�α���",dtStr),
					new DataColumn("Ƶ������",dtStr),
					new DataColumn("�÷�����",dtStr),
					new DataColumn("�÷�����",dtStr),//15
					new DataColumn("Ժע����",dtStr),//36
					new DataColumn("��ʼʱ��",dtDate),
					new DataColumn("����ҽ��",dtStr),
					new DataColumn("ִ�п��ұ���",dtStr),
					new DataColumn("ִ�п���",dtStr),
					new DataColumn("�Ӽ�",dtBoolean),
					new DataColumn("��鲿λ",dtStr),//31
					new DataColumn("��������/��鲿λ",dtStr),//32
					new DataColumn("�ۿ���ұ���",dtStr),//33
					new DataColumn("�ۿ����",dtStr),//34
					new DataColumn("��ע",dtStr),//20
					new DataColumn("¼���˱���",dtStr),
					new DataColumn("¼����",dtStr),
					new DataColumn("��������",dtStr),
					new DataColumn("����ʱ��",dtDate),
					new DataColumn("ֹͣʱ��",dtDate),//25
					new DataColumn("ֹͣ�˱���",dtStr),
					new DataColumn("ֹͣ��",dtStr),
					new DataColumn("˳���",dtStr),//28
                    new DataColumn("Ƥ�Դ���",dtStr),
                    new DataColumn("Ƥ��",dtStr)
				});

                dtOrder.Tables.Add(table);

                return dtOrder;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        int[] iColumns;
        int[] iColumnWidth;
        bool[] iColumnVisible;
        /// <summary>
        /// ����������
        /// </summary>
        private void SetColumnProperty()
        {
            if (System.IO.File.Exists(SetingFileName))
            {
                if (iColumnWidth == null || iColumnWidth.Length <= 0)
                {
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.neuSpread1.Sheets[0], SetingFileName);
                    
                    iColumnWidth = new int[40];
                    iColumnVisible = new bool[40];
                    for (int i = 0; i < this.neuSpread1.Sheets[0].Columns.Count; i++)
                    {
                        iColumnWidth[i] = (int)this.neuSpread1.Sheets[0].Columns[i].Width;
                        iColumnVisible[i] = this.neuSpread1.Sheets[0].Columns[i].Visible;
                    }
                }
                else
                {
                    for (int i = 0; i < this.neuSpread1.Sheets[0].Columns.Count; i++)
                    {
                        this.neuSpread1.Sheets[0].Columns[i].Width = iColumnWidth[i];
                        this.neuSpread1.Sheets[0].Columns[i].Visible = iColumnVisible[i];
                    }
                }
            }
            else
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.neuSpread1.Sheets[0], SetingFileName);
            }
        }

        /// <summary>
        /// ����fp����
        /// </summary>
        private void ColumnSet()
        {
            iColumns = new int[40];
            iColumns[0] = this.GetColumnIndexFromName("��");     //Type
            iColumns[1] = this.GetColumnIndexFromName("ҽ������");//OrderType
            iColumns[2] = this.GetColumnIndexFromName("ҽ����ˮ��");//ID
            iColumns[3] = this.GetColumnIndexFromName("ҽ��״̬");//�¿�������ˣ�ִ��State
            iColumns[4] = this.GetColumnIndexFromName("��Ϻ�");//4 ComboNo
            iColumns[5] = this.GetColumnIndexFromName("��ҩ");//5 MainDrug
            iColumns[6] = this.GetColumnIndexFromName("ҽ������");//6 Nameer	
            iColumns[7] = this.GetColumnIndexFromName("����");//7	Qty
            iColumns[8] = this.GetColumnIndexFromName("������λ");//8 PackUnit
            iColumns[9] = this.GetColumnIndexFromName("ÿ������");//9 DoseOnce
            iColumns[10] = this.GetColumnIndexFromName("��λ");//10 doseUnit
            iColumns[11] = this.GetColumnIndexFromName("����");//11 Fu
            iColumns[12] = this.GetColumnIndexFromName("Ƶ�α���"); //FrequencyCode
            iColumns[13] = this.GetColumnIndexFromName("Ƶ������"); //FrequecyName
            iColumns[14] = this.GetColumnIndexFromName("�÷�����"); //UsageCode
            iColumns[15] = this.GetColumnIndexFromName("�÷�����");//15
            iColumns[36] = this.GetColumnIndexFromName("Ժע����");//36
            iColumns[16] = this.GetColumnIndexFromName("��ʼʱ��");
            iColumns[17] = this.GetColumnIndexFromName("ִ�п��ұ���");
            iColumns[18] = this.GetColumnIndexFromName("ִ�п���");
            iColumns[19] = this.GetColumnIndexFromName("�Ӽ�");
            iColumns[20] = this.GetColumnIndexFromName("��ע");//20
            iColumns[21] = this.GetColumnIndexFromName("¼���˱���");
            iColumns[22] = this.GetColumnIndexFromName("¼����");
            iColumns[23] = this.GetColumnIndexFromName("��������");
            iColumns[24] = this.GetColumnIndexFromName("����ʱ��");
            iColumns[25] = this.GetColumnIndexFromName("ֹͣʱ��");//25
            iColumns[26] = this.GetColumnIndexFromName("ֹͣ�˱���");
            iColumns[27] = this.GetColumnIndexFromName("ֹͣ��");
            iColumns[28] = this.GetColumnIndexFromName("˳���");//28
            iColumns[29] = this.GetColumnIndexFromName("����ҽ��");
            iColumns[30] = this.GetColumnIndexFromName("���");
            iColumns[31] = this.GetColumnIndexFromName("��鲿λ");
            iColumns[32] = this.GetColumnIndexFromName("��������/��鲿λ");
            iColumns[33] = this.GetColumnIndexFromName("�ۿ���ұ���");
            iColumns[34] = this.GetColumnIndexFromName("�ۿ����");
            iColumns[35] = this.GetColumnIndexFromName("!");
            iColumns[36] = this.GetColumnIndexFromName("Ƥ�Դ���");
            iColumns[37] = this.GetColumnIndexFromName("Ƥ��");

        }

        private void SetColumnName(int k)
        {
            this.neuSpread1.Sheets[k].Columns.Count = 100;
            int i = 0;
            this.neuSpread1.Sheets[k].Columns[i].Width = 30;//��ϵĿ��
            this.neuSpread1.Sheets[k].Columns[i].Label = ("!");    //0
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��");     //0
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ҽ������");//1
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ҽ����ˮ��");//2
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ҽ��״̬");//�¿�������ˣ�ִ��
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��Ϻ�");//4
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��ҩ");//5
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ҽ������");//6
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��");    //0
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("����");//7
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�ܵ�λ");//8
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ÿ������");//9
            this.neuSpread1.Sheets[k].Columns[i].CellType = this.numberCellType;
            this.numberCellType.DecimalPlaces = 4;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��λ");//10
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("����");//11
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("Ƶ�α���");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("Ƶ������");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�÷�����");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�÷�����");//15
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("Ժע����");//36
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��ʼʱ��");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("����ҽ��");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ִ�п��ұ���");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ִ�п���");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�Ӽ�");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��鲿λ");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��������/��鲿λ");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�ۿ���ұ���");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("�ۿ����");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��ע");//20
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("¼���˱���");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("¼����");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("��������");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("����ʱ��");
            
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ֹͣʱ��");//25
            
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ֹͣ�˱���");
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("ֹͣ��");
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("˳���");//28
            this.neuSpread1.Sheets[k].Columns[i].Visible = false;
            i++;

            this.neuSpread1.Sheets[k].Columns[i].Label = ("Ƥ�Դ���");// 
            i++;
            this.neuSpread1.Sheets[k].Columns[i].Label = ("Ƥ��");// 
            i++;

            this.neuSpread1.Sheets[k].Columns.Count = i;
        }

        /// <summary>
        /// ��ʼ��ҽ����Ϣ����ӿ�ʵ��{48E6BB8C-9EF0-48a4-9586-05279B12624D}
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

        #region ˽�з���

        /// <summary>
        /// ͨ���������������
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private int GetColumnIndexFromName(string Name)
        {
            for (int i = 0; i < this.dtQur.Tables[0].Columns.Count; i++)
            {
                if (this.dtQur.Tables[0].Columns[i].ColumnName == Name) return i;
            }
            MessageBox.Show("ȱ����" + Name);
            return -1;
        }

        /// <summary>
        /// �õ�������
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string GetColumnNameFromIndex(int i)
        {
            return this.dtQur.Tables[0].Columns[i].ColumnName;
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
        
        #region ������ݵ����
        /// <summary>
        /// ���ʵ��toTable
        /// </summary>
        /// <param name="list"></param>
        private void AddObjectsToTable(ArrayList list)
        {
            this.dtQur.Tables[0].Clear();
            foreach (object obj in list)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order order = obj as Neusoft.HISFC.Models.Order.OutPatient.Order;

                this.dtQur.Tables[0].Rows.Add(AddObjectToRow(order, this.dtQur.Tables[0]));

            }
        }

        /// <summary>
        /// ���order��row
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private DataRow AddObjectToRow(object obj, DataTable table)
        {
            DataRow row = table.NewRow();
            Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
            try
            {
                order = obj as Neusoft.HISFC.Models.Order.OutPatient.Order;
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
                row["��λ"] = objItem.DoseUnit;
                row["����"] = order.HerbalQty;//11
            }
            else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
            {
                
            }

            if (order.Note != "")
            {
                row["!"] = order.Note;
            }
            //row["��Ч"] = Neusoft.FrameWork.Function.NConvert.ToInt32(order.OrderType.ID);     //0
            //row["ҽ������"] = order.OrderType.Name;//1
            row["��"] = "";     //0
            row["ҽ������"] = "����ҽ��";//1
            row["ҽ����ˮ��"] = order.ID;//2
            row["ҽ��״̬"] = order.Status;//�¿�������ˣ�ִ��
            row["��Ϻ�"] = order.Combo.ID;//4

            if (order.Item.Specs == null || order.Item.Specs.Trim() == "")
                row["ҽ������"] = order.Item.Name;//6
            else
                row["ҽ������"] = order.Item.Name + "[" + order.Item.Specs + "]";

            //ҽ����ҩ-֪��ͬ����
            if (order.IsPermission) row["ҽ������"] = "���̡�" + row["ҽ������"];

            this.ValidNewOrder(order);
            row["����"] = order.Qty;//7
            row["������λ"] = order.Unit;//8
            row["Ƶ�α���"] = order.Frequency.ID;
            row["Ƶ������"] = order.Frequency.Name;
            row["�÷�����"] = order.Usage.ID;
            row["�÷�����"] = order.Usage.Name;//15
            row["��ʼʱ��"] = order.BeginTime;
            row["ִ�п��ұ���"] = order.ExeDept.ID;
            
            row["ִ�п���"] = order.ExeDept.Name;
            row["�Ӽ�"] = order.IsEmergency;
            row["��鲿λ"] = order.CheckPartRecord;
            row["��������/��鲿λ"] = order.Sample;
            row["�ۿ���ұ���"] = order.StockDept.ID;
            row["�ۿ����"] = order.StockDept.Name;
            row["Ժע����"] = order.InjectCount;
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
            row["Ƥ�Դ���"] = order.HypoTest;
            row["Ƥ��"] = this.GetHypoTestFromCode(order.HypoTest);
            return row;
        }

        /// <summary>
        /// ����Ƥ�Դ���ת��Ƥ������
        /// </summary>
        /// <param name="hypoTestCode"></param>
        /// <returns></returns>
        private string GetHypoTestFromCode(int hypoTestCode)
        {
            string hypoTestName = string.Empty;
            switch (hypoTestCode)
            {
                case 1:
                    {
                        hypoTestName = "����ҪƤ��";
                        break;
                    }
                case 2:
                    {
                        hypoTestName = "��ҪƤ��";
                        break;

                    }
                case 3:
                    {
                        hypoTestName = "Ƥ������";
                        break;
                    }
                case 4:
                    {
                        hypoTestName = "Ƥ������";
                        break;
                    }
                default:
                    {
                        hypoTestName = string.Empty;
                        break;
                    }
            }
            return hypoTestName;
        }

        /// <summary>
        /// ���-����
        /// </summary>
        /// <param name="al"></param>
        private void AddObjectsToFarpoint(ArrayList al)
        {
            if (al == null) return;
            
            int k = 0;
            
            for (int i = 0; i < al.Count; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order order = al[i] as Neusoft.HISFC.Models.Order.OutPatient.Order;
                                
                this.neuSpread1.Sheets[0].Rows.Add(k, 1);
                this.AddObjectToFarpoint(al[i], k, 0, EnumOrderFieldList.Item);

                k++;
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="orderlist"></param>
        private void AddObjectToFarpoint(object obj, int i, int SheetIndex, EnumOrderFieldList orderlist)
        {
            Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
            try
            {
                order = ((Neusoft.HISFC.Models.Order.OutPatient.Order)obj).Clone();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Clone����" + ex.Message);
                return;
            }

            if (this.bTempVar)
            {
                # region �����÷��Զ��������Ժע
                try
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp = this.GetObjectFromFarPoint(i, SheetIndex);
                    //{9F2AA715-42FF-416b-9EB7-0E05DD5307C6}
                    //ԭ���Ĵ�����ж���temp!=null&&order.Usage.ID != "",���������׻���ҩƷ������Ϣ��ά����Ĭ���÷�ʱ,
                    //temp��nullֵ,���²��ᵯ��Ժע�˵�,���ڰ�temp!=null&&order.Usage.ID != ""��ֳ�2��if�ж�
                    if (temp != null)
                    {
                        if (order.Usage.ID != "")
                        {
                            if (temp.Usage.ID != order.Usage.ID)
                            {
                                if (this.varCombID != order.Combo.ID)
                                {
                                    this.varCombID = order.Combo.ID;
                                    varTempUsageID = "zuowy";//��ʱ�÷�
                                    varOrderUsageID = "maokb";//ҽ���÷�
                                }

                                //if (temp.Item.IsPharmacy || temp.Item.SysClass.ID.ToString() == "UL")
                                if (temp.Item.ItemType == EnumItemType.Drug || temp.Item.SysClass.ID.ToString() == "UL")
                                {
                                    if (temp.Usage.ID == this.varTempUsageID && order.Usage.ID == this.varOrderUsageID)
                                    {

                                    }
                                    else
                                    {
                                        this.varTempUsageID = temp.Usage.ID;
                                        this.varOrderUsageID = order.Usage.ID;
                                        order.InjectCount = 0;

                                        #region ����޸����÷�������ԭ������Ժע������0����Ҫɾ������{F67E089F-1993-4652-8627-300295AAED8C}
                                        if (temp.InjectCount > 0)
                                        {
                                            if (temp.ID != null && temp.ID != null)
                                            {
                                                if (!hsComboChange.ContainsKey(temp.ID))
                                                {
                                                    hsComboChange.Add(temp.ID, temp.Combo.ID);
                                                }
                                            }
                                            order.NurseStation.User02 = "C";
                                        }
                                        #endregion

                                        if (Classes.Function.hsUsageAndSub.Contains(order.Usage.ID))
                                        {
                                            ArrayList al = (ArrayList)Classes.Function.hsUsageAndSub[order.Usage.ID];
                                            if (al != null && al.Count > 0)
                                            {
                                                this.AddInjectNum(order);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    //{9F2AA715-42FF-416b-9EB7-0E05DD5307C6}
                    else
                    {
                        if (order.ID == null || order.ID == "")
                        {
                            if (order.Item.ItemType == EnumItemType.Drug && order.Usage.ID != "")
                            {
                                order.InjectCount = 0;
                                if (Classes.Function.hsUsageAndSub.Contains(order.Usage.ID))
                                {
                                    ArrayList al = (ArrayList)Classes.Function.hsUsageAndSub[order.Usage.ID];
                                    if (al != null && al.Count > 0)
                                    {
                                        this.AddInjectNum(order);
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                { }
                #endregion
            }

            if (order.Note != "")//��ʾ
            {
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[35]].Text = order.Note;
            }

            if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
            {
                Neusoft.HISFC.Models.Pharmacy.Item objItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[9]].Text = order.DoseOnce.ToString();//9
                if (order.Item.SysClass.ID.ToString() == Neusoft.HISFC.Models.Base.EnumSysClass.PCC.ToString())//��ҩ����
                {
                    this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[11]].Text = order.HerbalQty.ToString();//11
                }
                if (order.DoseUnit == null || order.DoseUnit == "") order.DoseUnit = objItem.DoseUnit;
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[10]].Text = order.DoseUnit; 

                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[7]].Text = order.Qty.ToString();//7
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[8]].Text = order.Unit;//8

            }
            else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug)) //��ҩƷ
            {
                Neusoft.HISFC.Models.Fee.Item.Undrug objItem = order.Item as Neusoft.HISFC.Models.Fee.Item.Undrug;
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[10]].Text = "";//������λ
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[7]].Text = order.Qty.ToString();//7
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[8]].Text = order.Unit;//8
            }
            else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Base.Item))
            {
                Neusoft.HISFC.Models.Fee.Item.Undrug objItem = order.Item as Neusoft.HISFC.Models.Fee.Item.Undrug;
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[10]].Text = "";//������λ
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[7]].Text = order.Qty.ToString();//7
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[8]].Text = order.Unit;//8
            }
            this.ValidNewOrder(order); //��д��Ϣ

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[0]].Text = "";     //0

            if (order.NurseStation.Memo != null && order.NurseStation.Memo.Length > 0)
            {
                //������ҩ��أ���ʱδ�����Σ�
                //this.AddWarnPicturn(i, 0, neusoft.neHISFC.Components.Function.NConvert.ToInt32(order.NurseStation.Memo));
            }
            else
            {
                this.neuSpread1_Sheet1.Cells[i, iColumns[0]].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
                this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Note = "";
                this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Tag = "";
            }
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[5]].Text = System.Convert.ToInt16(order.Combo.IsMainDrug).ToString();//5
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[1]].Text = "����ҽ��"; //1 ����

            if (order.Item.PackQty == 0)
            {
                order.Item.PackQty = 1;
            }
            
            //ҽ������ 
            if (order.Item.Specs == null || order.Item.Specs.Trim() == "")
            {
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = order.Item.Name.ToString();

                if (order.Item.Price > 0)
                {

                    if (order.NurseStation.User03 == "1") //��С��λ�жϣ�����������������sunm
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + Neusoft.FrameWork.Public.String.FormatNumberReturnString(order.Item.Price / order.Item.PackQty, 2) + "Ԫ/" + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                    else
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + order.Item.Price.ToString() + "Ԫ/" + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                }
                //else if (order.Item.p > 0)//���ü۸�???sunm
                //{
                //    if (order.NurseStation.User03 == "1") //��С��λ
                //    {
                //        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + neusoft.neHISFC.Components.Public.String.FormatNumberReturnString(order.Item.Price4 / order.Item.PackQty, 2) + "Ԫ/" + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                //    }
                //    else
                //    {
                //        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + order.Item.Price4.ToString() + "Ԫ/" + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                //    }
                //}
                else if (order.Unit == "[������]")
                {
                    if (order.NurseStation.User03 == "1")
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + Neusoft.FrameWork.Public.String.FormatNumberReturnString(OutPatient.Classes.Function.GetUndrugZtPrice(order.Item.ID) / order.Item.PackQty, 2) + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                    else
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + OutPatient.Classes.Function.GetUndrugZtPrice(order.Item.ID).ToString() + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                }
            }
            else
            {
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = order.Item.Name.ToString() + "[" + order.Item.Specs + "] ";
                if (order.Item.Price > 0)
                {
                    if (order.NurseStation.User03 == "1")
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + Neusoft.FrameWork.Public.String.FormatNumberReturnString(order.Item.Price / order.Item.PackQty, 2) + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                    else
                    {
                        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + order.Item.Price.ToString() + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                    }
                }
                //else if (order.Item.Price4 > 0)
                //{
                //    if (order.NurseStation.User03 == "1")
                //    {
                //        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + Neusoft.FrameWork.Public.String.FormatNumberReturnString(order.Item.Price / order.Item.PackQty, 2) + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                //    }
                //    else
                //    {
                //        this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "[" + order.Item.Price.ToString() + "Ԫ/" + order.Unit + "]" + "/" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;//6
                //    }
                //}
            }

            //ҽ������֪��ͬ����
            if (order.IsPermission)
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text = "���̡�" + this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[6]].Text;

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[2]].Text = order.ID;//2
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[3]].Text = order.Status.ToString();//�¿�������ˣ�ִ��
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[4]].Text = order.Combo.ID.ToString();//4
            
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[12]].Text = order.Frequency.ID.ToString();
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[13]].Text = order.Frequency.Name;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[14]].Text = order.Usage.ID;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[15]].Text = order.Usage.Name;//15

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[36]].Text = order.InjectCount.ToString();//36
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[16]].Value = order.BeginTime;//��ʼʱ��
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[24]].Value = order.MOTime;//����ʱ��


            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[17]].Text = order.ExeDept.ID;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[18]].Text = order.ExeDept.Name;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[19]].Value = order.IsEmergency;

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[31]].Value = order.CheckPartRecord;//��鲿λ
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[32]].Value = order.Sample.Name;//��������/��鲿λ
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[33]].Value = order.StockDept.ID;//�ۿ����
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[34]].Value = order.StockDept.Name;

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[20]].Text = order.Memo;//20
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[21]].Text = order.Oper.ID;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[22]].Text = order.Oper.Name;

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[29]].Text = order.ReciptDoctor.Name;//����ҽ��
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[23]].Text = order.ReciptDept.Name;//��������

            if (order.EndTime != DateTime.MinValue)
                this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[25]].Value = order.EndTime;//ֹͣʱ�� 25

            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[26]].Text = order.DCOper.ID;
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[27]].Text = order.DCOper.Name;
            if (order.SortID == 0)
            {
                order.SortID = MaxSort + 1;
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
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[28]].Value = order.SortID;//28
            if (!this.EditGroup)
            {
                if (this.myPatientInfo.Pact.PayKind.ID == "02")//����ҽ��-��ʾ���ñ���
                {
                    string feeStr = "";

                    if (order.Item.PriceUnit != "[������]")
                    {
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Columns.Get(0).Width = 15;
                        //feeStr = Neusoft.HISFC.Components.Common.Classes.Function //Neusoft.Common.Class.Function.ShowItemGrade(order.Item.ID);
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Cells[i, 0].Text = feeStr;
                    }
                    else
                    {
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Columns.Get(0).Width = 15;
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Cells[i, 0].Text = "";
                    }
                    #region  �ж��Ƿ������������Ŀ���Է���Ŀ������ͬһ����
                    //if (this.bCanAddOrder == false)
                    //{
                    //    if (this.CheckCanAddOrder(feeStr) < 0)
                    //    {
                    //        MessageBox.Show("��������Ŀ���Է���Ŀ����������һ�Ŵ���");
                    //        this.neuSpread1_Sheet1.Rows.Remove(i, 1);
                    //        return;
                    //    }
                    //}
                    #endregion
                }
                else//��ʾ��Ŀҽ�����
                {
                    //this.neuSpread1.Sheets[SheetIndex].RowHeader.Columns.Get(0).Width = 50F;
                    //if (order.Item.Price > 0 && order.OrderType.IsCharge) this.neuSpread1.Sheets[SheetIndex].RowHeader.Cells[i, 0].Text = Neusoft.HISFC.Components.Common.Classes.Function.ShowItemFlag(order.Item);
                }
            }
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[36]].Value = order.HypoTest;//28
            this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[37]].Value = this.GetHypoTestFromCode(order.HypoTest);//28
            this.neuSpread1.Sheets[SheetIndex].Rows[i].Tag = order;
            return;
        }
        #endregion

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
                int i = iColumns[3];//this.GetColumnIndexFromName("ҽ��״̬");
                int state = int.Parse(this.neuSpread1.Sheets[SheetIndex].Cells[row, i].Text);

                if (GetObjectFromFarPoint(row, SheetIndex).ID != "" && reset)
                {
                    
                    this.neuSpread1.Sheets[SheetIndex].Cells[row, i].Value = state;
                }

                switch (state)
                {
                    case 0:
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(128, 255, 128);
                        break;
                    case 1:
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(106, 174, 242);
                        break;
                    case 2:
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(243, 230, 105);
                        break;
                    case 3:
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.FromArgb(248, 120, 222);
                        break;
                    default:
                        this.neuSpread1.Sheets[SheetIndex].RowHeader.Rows[row].BackColor = Color.Black;
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
        /// ��ѯҽ��
        /// </summary>
        private void QueryOrder()
        {
            try
            {
                this.neuSpread1.Sheets[0].RowCount = 0;
                
                if (this.dtQur != null && this.dtQur.Tables[0].Rows.Count > 0)
                    this.dtQur.Tables[0].Rows.Clear();
                
            }
            catch
            {
                
            }
            if (this.myPatientInfo == null)
            {
                return;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯҽ��,���Ժ�!");
            Application.DoEvents();

            //��ѯ����ҽ������
            ArrayList al = OrderManagement.QueryOrder(this.myPatientInfo.DoctorInfo.SeeNO.ToString());

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾҽ��,���Ժ�!");
            Application.DoEvents();
            if (this.IsDesignMode)
            {
                tooltip.SetToolTip(this.neuSpread1, "����ҽ��");
                tooltip.Active = true;
                this.bTempVar = true;
                try
                {
                    this.neuSpread1.Sheets[0].DataSource = null;

                    this.AddObjectsToFarpoint(al);
                    this.neuSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
                    
                    this.RefreshCombo();
                    this.RefreshOrderState();
                }
                catch (Exception ex)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                tooltip.SetToolTip(this.neuSpread1, "");
                try
                {
                    this.AddObjectsToTable(al);
                    this.dvQur = new DataView(this.dtQur.Tables[0]);

                    this.neuSpread1.Sheets[0].DataSource = dvQur;
                    
                    this.neuSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

                    ////CheckSortID();//���˳���

                    this.RefreshCombo();
                    this.RefreshOrderState();

                }
                catch (Exception ex)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(ex.Message);
                }
            }

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

        }

        protected override int OnQuery(object sender, object neuObject)
        {
            this.QueryOrder();
            return 0;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="order"></param>
        private void ValidNewOrder(Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            if (order.ReciptDept.Name == "" && order.ReciptDept.ID != "") order.ReciptDept.Name = this.GetDeptName(order.ReciptDept);
            if (order.StockDept.Name == "" && order.StockDept.ID != "") order.StockDept.Name = this.GetDeptName(order.StockDept);
            if (order.BeginTime == DateTime.MinValue) order.BeginTime = this.OrderManagement.GetDateTimeFromSysDateTime();
            if (order.MOTime == DateTime.MinValue) order.MOTime = order.BeginTime;
            if (!this.EditGroup)
            {
                if (order.Patient == null || order.Patient.ID == "")
                {
                    order.Patient.ID = this.myPatientInfo.ID;
                    order.SeeNO = this.myPatientInfo.DoctorInfo.SeeNO.ToString();
                    order.RegTime = this.myPatientInfo.DoctorInfo.SeeDate;
                    order.Patient.PID = this.myPatientInfo.PID;
                }
                if (order.InDept.ID == null || order.InDept.ID == "")
                    order.InDept = this.myPatientInfo.DoctorInfo.Templet.Dept;
            }
            if (order.ExeDept == null || order.ExeDept.ID == "")
            {
                //����ִ�п���Ϊ���߿���
                if (!this.EditGroup)
                    order.ExeDept = this.GetReciptDept().Clone();//{56D98B49-A27E-487f-B331-0B9CDB04D4ED}
                else
                    order.ExeDept = ((Neusoft.HISFC.Models.Base.Employee)this.OrderManagement.Operator).Dept.Clone();
            }
            if (order.ExeDept.Name == "" && order.ExeDept.ID != "")
                order.ExeDept.Name = this.GetDeptName(order.ExeDept);
            //����ҽ��
            if (order.ReciptDoctor == null || order.ReciptDoctor.ID == "")
                order.ReciptDoctor = this.GetReciptDoc().Clone();
            //��������
            if (order.ReciptDept == null || order.ReciptDept.ID == "")
                order.ReciptDept = this.GetReciptDept().Clone();
            
            if (order.Oper.ID == null || order.Oper.ID == "")
            {
                order.Oper.ID = this.OrderManagement.Operator.ID;
                order.Oper.Name = this.OrderManagement.Operator.Name;
            }
            
        }

        /// <summary>
        /// ��鿪����Ϣ����ʾ����
        /// </summary>
        /// <param name="strMsg"></param>
        /// <param name="iRow"></param>
        /// <param name="SheetIndex"></param>
        private void ShowErr(string strMsg, int iRow, int SheetIndex)
        {
            this.neuSpread1.ActiveSheetIndex = SheetIndex;
            this.neuSpread1.Sheets[SheetIndex].ClearSelection();
            this.neuSpread1.Sheets[SheetIndex].ActiveRowIndex = iRow;
            this.SelectionChanged();
            this.neuSpread1.Sheets[SheetIndex].AddSelection(iRow, 0, 1, 1);
            MessageBox.Show(strMsg);
        }

        /// <summary>
        /// ѡ��仯
        /// </summary>
        private void SelectionChanged()
        {
                        
            #region ѡ��
            //ÿ��ѡ��仯ǰ���������ʾ
            this.ucOutPatientItemSelect1.Clear();

            //�¿��� ���ܸ���
            if (int.Parse(this.neuSpread1.ActiveSheet.Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, iColumns[3]].Text) == 0)
            {
                
                //����Ϊ��ǰ��
                this.ucOutPatientItemSelect1.CurrentRow = this.neuSpread1.ActiveSheet.ActiveRowIndex;
                this.ActiveRowIndex = this.neuSpread1.ActiveSheet.ActiveRowIndex;
                this.currentOrder = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, this.neuSpread1.ActiveSheetIndex);
                this.ucOutPatientItemSelect1.currOrder = this.currentOrder;
                //���������ѡ��
                if (this.ucOutPatientItemSelect1.currOrder.Combo.ID != "" && this.ucOutPatientItemSelect1.currOrder.Combo.ID != null)
                {
                    int comboNum = 0;//��õ�ǰѡ������
                    for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
                    {
                        string strComboNo = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex).Combo.ID;
                        if (this.ucOutPatientItemSelect1.currOrder.Combo.ID == strComboNo && i != this.neuSpread1.ActiveSheet.ActiveRowIndex)
                        {
                            this.neuSpread1.ActiveSheet.AddSelection(i, 0, 1, 1);
                            comboNum++;
                        }
                    }
                    if (comboNum == 0)
                    {
                        //ֻ��һ��
                        if (OrderCanCancelComboChanged != null) this.OrderCanCancelComboChanged(false);//����ȡ�����

                    }
                    else
                    {
                        if (OrderCanCancelComboChanged != null) this.OrderCanCancelComboChanged(true);//����ȡ�����                            
                    }
                }

                if (OrderCanSetCheckChanged != null) this.OrderCanSetCheckChanged(false);//��ӡ������뵥ʧЧ
                                
            }
            else
            {
                this.ActiveRowIndex = -1;
            }
            #endregion
                        
        }

        /// <summary>
        /// ���ҽ��
        /// </summary>
        /// <param name="k"></param>
        private void ComboOrder(int k)
        {
            
            int iSelectionCount = 0;
            for (int i = 0; i < this.neuSpread1.Sheets[k].Rows.Count; i++)
            {
                if (this.neuSpread1.Sheets[k].IsSelected(i, 0))
                    iSelectionCount++;
            }

            if (iSelectionCount > 1)
            {
                string t = "";//��Ϻ� �޸ĳɶ�����Ϻ�
                int injectNum = 0;//Ժ��ע����
                int iSort = -1;
                string time = "";
                #region {4F5BEF6C-48FE-4abb-84F2-091838D7BA03}
                int kk = 0;
                #endregion

                if (this.ValidComboOrder() == -1) return;//У�����ҽ��

                for (int i = 0; i < this.neuSpread1.Sheets[k].Rows.Count; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order ord = this.GetObjectFromFarPoint(i, k);
                    ord.SortID = this.neuSpread1.Sheets[k].Rows.Count - i;
                    this.neuSpread1.Sheets[k].Cells[i, iColumns[28]].Text = Convert.ToString(this.neuSpread1.Sheets[k].Rows.Count - i);
                    this.neuSpread1.Sheets[k].Cells[i, iColumns[28]].Value = this.neuSpread1.Sheets[k].Rows.Count - i;
                    if (this.neuSpread1.Sheets[k].IsSelected(i, 0))
                    {

                        if (t == "")
                        {
                            t = ord.Combo.ID;
                            time = ord.Frequency.Time;
                        }
                        else
                        {
                            #region ������Ѿ������ҽ������ϱ仯����Ҫɾ������{F67E089F-1993-4652-8627-300295AAED8C}

                            if (ord.ID != null && ord.ID != null)
                            {
                                if (!hsComboChange.ContainsKey(ord.ID))
                                {
                                    hsComboChange.Add(ord.ID, ord.Combo.ID);
                                }
                            }
                            ord.NurseStation.User02 = "C";
                            #endregion

                            ord.Combo.ID = t;
                            ord.Frequency.Time = time;
                        }
                        //Ժ��ע����
                        if (injectNum == 0)
                        {
                            injectNum = ord.InjectCount;
                        }
                        else
                        {
                            ord.InjectCount = injectNum;
                        }
                        #region {4F5BEF6C-48FE-4abb-84F2-091838D7BA03}
                        //if (iSort == -1)
                        //{
                        //    iSort = int.Parse(this.neuSpread1.Sheets[k].Cells[i, iColumns[28]].Text);
                        //}
                        //else
                        //{
                        //    ord.SortID = iSort;
                        //}
                        if (iSort == -1)
                        {
                            iSort = int.Parse(this.neuSpread1.Sheets[k].Cells[i, iColumns[28]].Text);
                        }
                        else
                        {
                            ord.SortID = iSort - kk;

                        }
                        kk++;
                        #endregion

                        this.AddObjectToFarpoint(ord, i, k, EnumOrderFieldList.Item);
                    }
                    #region {4F5BEF6C-48FE-4abb-84F2-091838D7BA03}
                    else
                    {
                        if (kk > 0)
                        {
                            ord.SortID = ord.SortID - iSelectionCount + kk;
                        }
                        this.AddObjectToFarpoint(ord, i, k, EnumOrderFieldList.Item);
                    }
                    #endregion
                }
                
                this.neuSpread1.Sheets[k].ClearSelection();
            }
            else
            {
                MessageBox.Show("��ѡ�������");
            }
            
        }
                
        /// <summary>
        /// У�����ҽ��
        /// </summary>
        /// <returns></returns>
        private int ValidComboOrder()
        {
            Neusoft.HISFC.Models.Order.Frequency frequency = null;//Ƶ��
            Neusoft.FrameWork.Models.NeuObject usage = null;//�÷�
            Neusoft.FrameWork.Models.NeuObject exeDept = null;//ִ�п���
            decimal amount = 0;//����
            int sysclass = -1;//���
            decimal days = 0;//��ҩ����
            string sample = "";//����
            decimal injectCount = 0;//Ժע����
            string jpNum = "";

            ArrayList alItems = new ArrayList();
            
            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.neuSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order o = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex);
                    if (o.ID != "")
                    {
                        Neusoft.HISFC.Models.Order.OutPatient.Order tem = this.OrderManagement.QueryOneOrder(o.ID);
                        if (tem.Status != 0)
                        {
                            MessageBox.Show(o.Item.Name + "�Ѿ��շѣ�����������ã�");
                            return -1;
                        }
                    }
                    if (o.Status != 0)
                    {
                        return -1;
                    }
                    if (o.Item.SysClass.ID.ToString() == "UL")//������Ŀ�ж��Ƿ���Բ��ܣ����ԵĲſ������
                    {
                        alItems.Add(o.Item.ID);
                    }
                    
                    if (frequency == null)
                    {
                        frequency = o.Frequency.Clone();
                        usage = o.Usage.Clone();
                        sysclass = o.Item.SysClass.ID.GetHashCode();
                        exeDept = o.ExeDept.Clone();
                        amount = o.Qty;
                        days = o.HerbalQty;
                        sample = o.Sample.Name;
                        injectCount = o.InjectCount;
                        jpNum = o.ExtendFlag1;
                    }
                    else
                    {
                        if (o.Frequency.ID != frequency.ID)
                        {
                            MessageBox.Show("Ƶ�β�ͬ������������ã�");
                            return -1;
                        }
                        if (o.InjectCount != injectCount)
                        {
                            MessageBox.Show("Ժע������ͬ������������ã�");
                            return -1;
                        }
                        //if (o.Item.IsPharmacy)		//ֻ��ҩƷ�ж��÷��Ƿ���ͬ
                        if (o.Item.ItemType == EnumItemType.Drug)		//ֻ��ҩƷ�ж��÷��Ƿ���ͬ
                        {
                            if (o.Usage.ID != usage.ID)
                            {
                                MessageBox.Show("�÷���ͬ������������ã�");
                                return -1;
                            }
                            if (o.Item.SysClass.ID.ToString() == "PCC" || o.Item.SysClass.ID.ToString() == "C")
                            {
                                if (o.HerbalQty != days)
                                {
                                    MessageBox.Show("��ҩ������ͬ������������ã�");
                                    return -1;
                                }
                            }
                            if (o.ExtendFlag1 != jpNum)
                            {
                                MessageBox.Show("��ƿ����ͬ������������ã�");
                                return -1;
                            }
                        }
                        else
                        {
                            if (o.Item.SysClass.ID.ToString() == "UL")//����
                            {
                                if (o.Qty != amount)
                                {
                                    MessageBox.Show("����������ͬ������������ã�");
                                    return -1;
                                }
                                if (o.Sample.Name != sample)
                                {
                                    MessageBox.Show("����������ͬ������������ã�");
                                    return -1;
                                }
                            }
                        }
                        if (o.Item.SysClass.ID.GetHashCode() != sysclass)
                        {
                            MessageBox.Show("��Ŀ���ͬ������������ã�");
                            return -1;
                        }
                        if (o.ExeDept.ID != exeDept.ID)
                        {
                            MessageBox.Show("ִ�п��Ҳ�ͬ���������ʹ��!", "��ʾ");
                            return -1;
                        }
                        
                    }
                }
            }
            
            ////if (alItems.Count > 0)
            ////{
            ////    if (!fun.IsComboLab(alItems))
            ////    {
            ////        MessageBox.Show("������Ŀ�����ϲ��ܹ���,�������!", "��ʾ");
            ////        return -1;
            ////    }
            ////}

            return 0;

        }

        protected ArrayList GetSelectedRows()
        {

            ArrayList rows = new ArrayList();

            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.neuSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    rows.Add(i);
                }
            }
            return rows;
        }

        /// <summary>
        /// ���Ժ��ע�����
        /// </summary>
        /// <param name="sender"></param>
        private void AddInjectNum(Neusoft.HISFC.Models.Order.OutPatient.Order sender)
        {
            //if ((sender.Item.IsPharmacy == false && sender.Item.SysClass.ID.ToString() != "UL") || sender.Usage.ID == "") return;
            if ((sender.Item.ItemType != EnumItemType.Drug && sender.Item.SysClass.ID.ToString() != "UL") || sender.Usage.ID == "") return;
            if (!Classes.Function.hsUsageAndSub.Contains(sender.Usage.ID))
            {
                return;
            }
            formInputInjectNum = new Forms.frmInputInjectNum();
            formInputInjectNum.Order = sender;
            //if (formInputInjectNum.Order.DoseUnit == null && formInputInjectNum.Order.Item.IsPharmacy)
            if (formInputInjectNum.Order.DoseUnit == null && formInputInjectNum.Order.Item.ItemType == EnumItemType.Drug)
            {
                formInputInjectNum.Order.DoseUnit = ((Neusoft.HISFC.Models.Pharmacy.Item)formInputInjectNum.Order.Item).DoseUnit;
            }
            formInputInjectNum.InjectNum = sender.InjectCount;
            if (sender.InjectCount == 0)
            {
                #region {8D4A8FD5-0231-4701-9990-3B2A83503D95}
                //����Ĭ�ϵ�Ժע����Ϊ����/ÿ����
                int injectNumTmp = Neusoft.FrameWork.Function.NConvert.ToInt32(sender.Item.Qty * ((Neusoft.HISFC.Models.Pharmacy.Item)sender.Item).BaseDose / sender.DoseOnce);
                formInputInjectNum.InjectNum = injectNumTmp;
                #endregion
                DialogResult r = MessageBox.Show("��ҩƷ�Ƿ�ΪԺ��ע�䣿", "[��ʾ]", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (r == DialogResult.No)
                {
                    this.ucOutPatientItemSelect1.ucInputItem1.Focus();
                    return;
                }
            }
            formInputInjectNum.ShowDialog();
            if (this.ucOutPatientItemSelect1.ucOrderInputByType1.Order != null)
            {
                this.ucOutPatientItemSelect1.ucOrderInputByType1.Order.InjectCount = sender.InjectCount;
            }

            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                
                Neusoft.HISFC.Models.Order.OutPatient.Order order = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex);
                if (order == null)
                    continue;
                if (order.Combo.ID == sender.Combo.ID)
                {
                    order.ExtendFlag1 = sender.ExtendFlag1;
                    order.InjectCount = sender.InjectCount;
                    order.NurseStation.User02 = "C";//�޸Ĺ�Ժע

                    #region ֻҪ�Ǳ������ҽ�������Ժע����Ҫɾ��ԭ���ĸ���{F67E089F-1993-4652-8627-300295AAED8C}

                    if (sender.ID != null && sender.ID != null)
                    {
                        if (!hsComboChange.ContainsKey(sender.ID))
                        {
                            hsComboChange.Add(sender.ID, sender.Combo.ID);
                        }
                    }
                    #endregion

                    this.ucOutPatientItemSelect1.currOrder.NurseStation.User02 = "C";
                    this.ucOutPatientItemSelect1.currOrder.ExtendFlag1 = sender.ExtendFlag1;
                    this.AddObjectToFarpoint(order, i, this.neuSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
                }

            }
            #region {66C96B33-F371-4796-ADB4-92C66376327A}
            this.RefreshOrderState();
            #endregion
            
        }

        /// <summary>
        /// �жϷ�ҩҩ����ִ�п���
        /// </summary>
        /// <param name="pManager"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private int CheckOrderStockDeptAndExeDept(Neusoft.HISFC.BizProcess.Integrate.Pharmacy pManager, ref Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            
            //if (order.Item.IsPharmacy)
            if (order.Item.ItemType == EnumItemType.Drug)
            {
                
                Neusoft.HISFC.Models.Pharmacy.Item tempItem = null;

                tempItem = pManager.GetItem(order.Item.ID);

                if (tempItem == null || tempItem.IsStop)
                {
                    MessageBox.Show("ҩƷ:" + tempItem.Name + "�Ѿ�ͣ��", "��ʾ");
                    return -1;
                }

                Neusoft.HISFC.Models.Order.OutPatient.Order temp = new Neusoft.HISFC.Models.Order.OutPatient.Order();
                temp.Item = order.Item;
                temp.ReciptDept = order.ReciptDept;


                #region ��������ָ��Ĭ��ȡҩҩ�� {ABCC78F9-826F-4f03-BB4E-1FDE2A494E1C}

                if (Classes.Function.FillPharmacyItem(pManager, ref temp) == -1)
                {
                    return -1;
                }

                if (Classes.Function.CheckPharmercyItemStock(1,order.Item.ID,order.Item.Name,order.ReciptDept.ID,order.Qty) == false)
                {
                    return -1;
                }

                
                //if (Classes.Function.FillPharmacyItemWithStockDept(pManager, ref temp) == -1)
                //{
                //    return -1;
                //}
                //order.StockDept.ID = temp.StockDept.ID;
                //if (temp.StockDept.Name == "" && temp.StockDept.ID != "")
                //{
                //    order.StockDept.Name = this.GetDeptName(temp.StockDept);
                //}
                #endregion
            }
            return 0;
        }

        /// <summary>
        /// ȡ����ͬ��Ϻŵ�ҽ����Ŀ��ͬʱ����ʱ������ɾ��
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private int GetNumHaveSameComb(Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            if (this.alTemp.Count <= 0)
            {
                return 0;
            }

            if (order == null)
            {
                return 0;
            }

            int count = 0;
            ArrayList al = new ArrayList();
            for (int i = 0; i < alTemp.Count; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order temp
                    = alTemp[i] as Neusoft.HISFC.Models.Order.OutPatient.Order;

                if (temp.Combo.ID == order.Combo.ID)
                {
                    count++;
                    al.Add(temp);
                }
            }

            for (int j = 0; j < al.Count; j++)
            {
                alTemp.Remove(al[j]);
            }

            return count;
        }

        /// <summary>
        /// ������Ʒ��ҽ���������Ƴ�ҽ��
        /// </summary>
        /// <param name="alOrder"></param>
        /// <param name="alOrderAndSub"></param>
        private void RemoveOrderFromArray(ArrayList alOrder, ref ArrayList alOrderAndSub)
        {
            if (alOrder == null || alOrder.Count == 0)
            {
                return;
            }
            if (alOrderAndSub == null || alOrderAndSub.Count == 0)
            {
                return;
            }
            ArrayList alTemp = new ArrayList();
            for (int i = 0; i < alOrderAndSub.Count; i++)
            {
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = alOrderAndSub[i] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;
                for (int j = 0; j < alOrder.Count; j++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp = alOrder[j] as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (temp.ID == item.Order.ID)
                    {
                        item.Item.MinFee.User03 = "1";
                    }
                }
            }
            foreach (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item in alOrderAndSub)
            {
                if (item.Item.MinFee.User03 != "1")
                {
                    alTemp.Add(item);
                }
            }
            alOrderAndSub = alTemp;
        }

        /// <summary>
        /// ����ҽ��˳���
        /// </summary>
        /// <returns></returns>
        private int SaveSortID(int k)
        {
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            for (int i = 0; i < this.neuSpread1.Sheets[k].Rows.Count; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order ord = this.GetObjectFromFarPoint(i, k);
                ord.SortID = this.neuSpread1.Sheets[k].Rows.Count - i;
                int iReturn = -1;
                iReturn = OrderManagement.UpdateOrderSortID(ord.ID, ord.SortID);
                if (iReturn < 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(OrderManagement.Err);
                    return -1;
                }
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            return 0;
        }

        /// <summary>
        /// ҽ��������ʾ��
        /// </summary>
        private void SetOrderFeeDisplay()
        {
            if (!EditGroup && this.myPatientInfo.ID.Length > 0)
            {
                this.neuPanel1.Visible = true;
                this.lblDisplay.Visible = true;
                //{047C2448-B3D3-49eb-A40B-DF75749A4245}
                this.lblPactName.Visible = true;
                //lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                //    "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday) + "  ��ͬ��λ��";// +
                   // this.myPatientInfo.Pact.Name;
                //�������ͳһ�㷨 {BBB677F7-371A-4844-912A-8272BBD351C4} wbo 2010-12-05
                Neusoft.HISFC.Models.RADT.Patient pat = assignManagement.QueryComPatientInfo(this.myPatientInfo.PID.CardNO);
                string age = string.Empty;
                if (pat != null)
                {
                    age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(pat.Birthday);
                }
                lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                    "  ���䣺" + age;//Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.myPatientInfo.Birthday);// +"  ��ͬ��λ��";

                this.lblPactName.Text = "��ͬ��λ��" + this.myPatientInfo.Pact.Name;
                decimal totcost = 0;
                Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
                Neusoft.HISFC.Models.Order.OutPatient.Order orderPre = null;
                for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
                {
                    order = this.GetObjectFromFarPoint(i, 0);
                    if (i > 0)
                    {
                        orderPre = this.GetObjectFromFarPoint(i - 1, 0);
                    }
                    if (order.InjectCount > 0)
                    {
                        if (orderPre != null && order.Combo.ID == orderPre.Combo.ID)
                        {
                            totcost = totcost + 0;
                        }
                        else
                        {
                            ArrayList alSubtbls = (ArrayList)Classes.Function.hsUsageAndSub[order.Usage.ID];
                            if (alSubtbls != null)
                            {
                                for (int m = 0; m < alSubtbls.Count; m++)
                                {
                                    Neusoft.HISFC.Models.Fee.Item.Undrug itemSub = null;
                                    try
                                    {
                                        if (((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID.Substring(0, 1) == "F")
                                        {
                                            itemSub = feeManagement.GetItem(((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID);
                                            if (itemSub.UnitFlag == "1")
                                            {
                                                itemSub.Price = feeManagement.GetUndrugCombPrice(itemSub.ID);
                                            }
                                        }
                                        else
                                        {
                                            itemSub = this.itemManagement.GetItem(((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID);
                                            if (itemSub == null || itemSub.ID == null || itemSub.ID.Length <= 0)
                                            {
                                                totcost = totcost + 0;
                                            }
                                        }
                                    }
                                    catch { }
                                    if (itemSub != null)
                                    {
                                        #region {66C96B33-F371-4796-ADB4-92C66376327A}
                                        //totcost = totcost + itemSub.Price;
                                        totcost = totcost + itemSub.Price * order.InjectCount;
                                        #endregion
                                    }
                                }
                            }

                        }
                    }
                    if (order.HypoTest == 2)
                    {
                        object obj = Classes.Function.controlerHelper.GetObjectFromID("200025");

                        if (obj != null)
                        {
                            string hypoFeeCode = ((Neusoft.HISFC.Models.Base.Controler)obj).ControlerValue;

                            if (hypoFeeCode != null && hypoFeeCode.Length > 0)
                            {
                                Neusoft.HISFC.Models.Fee.Item.Undrug itemHypo = null;

                                try
                                {
                                    if (hypoFeeCode.Substring(0, 1) == "F")
                                    {
                                        itemHypo = feeManagement.GetItem(hypoFeeCode);//���������Ŀ��Ϣ
                                        if (itemHypo.UnitFlag == "1")
                                        {
                                            itemHypo.Price = feeManagement.GetUndrugCombPrice(itemHypo.ID);
                                        }
                                    }
                                    else
                                    {
                                        itemHypo = this.itemManagement.GetItem(hypoFeeCode);
                                        if (itemHypo == null || itemHypo.ID == null || itemHypo.ID.Length <= 0)
                                        {
                                            totcost = totcost + 0;
                                        }
                                    }
                                }
                                catch 
                                { }
                                if (itemHypo != null)
                                {
                                    totcost = totcost + itemHypo.Price;
                                }
                            }
                        }
                    }
                    if (order.NurseStation.User03 == "")//user03Ϊ��,˵����֪��������ʲô��λ Ĭ��Ϊ��С��λ
                    {
                        order.NurseStation.User03 = "1";//Ĭ��
                    }
                    if (order.NurseStation.User03 != "1")//������С��λ !=((Neusoft.HISFC.Models.Pharmacy.Item)order.Item).MinUnit)
                    {
                        totcost = order.Qty * order.Item.Price + totcost;
                    }
                    else
                    {
                        totcost = order.Qty * order.Item.Price / order.Item.PackQty + totcost;
                        totcost = Neusoft.FrameWork.Public.String.FormatNumber(totcost, 2);
                    }

                }
                if (totcost > 0)
                {
                 //{047C2448-B3D3-49eb-A40B-DF75749A4245}   
                    //lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                    //    "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday) + "  ��ͬ��λ��" +
                    //    this.myPatientInfo.Pact.Name + "  �����ܶ" + totcost.ToString();
                    //�������ͳһ�㷨 {BBB677F7-371A-4844-912A-8272BBD351C4} wbo 2010-12-05
                    //lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                    //   "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday) + "  ��ͬ��λ��" +
                    lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                       "  ���䣺" + age;// Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.myPatientInfo.Birthday);// +"  ��ͬ��λ��" +
                       /*this.myPatientInfo.Pact.Name + "  �����ܶ" + */totcost.ToString();
                    this.lblPactName.Text = "��ͬ��λ��" + this.myPatientInfo.Pact.Name;
     
                }
                else
                {
                    //{047C2448-B3D3-49eb-A40B-DF75749A4245}
                    //lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                    //    "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday) + "  ��ͬ��λ��" +
                    //    this.myPatientInfo.Pact.Name;
                    //�������ͳһ�㷨 {BBB677F7-371A-4844-912A-8272BBD351C4} wbo 2010-12-05
                    //lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                    //   "  ���䣺" + this.OrderManagement.GetAge(this.myPatientInfo.Birthday);// +"  ��ͬ��λ��" +
                    lblDisplay.Text = "������" + this.myPatientInfo.Name + "  �Ա�" + this.myPatientInfo.Sex.Name +
                       "  ���䣺" + age;// Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.myPatientInfo.Birthday);// +"  ��ͬ��λ��" +
                       //this.myPatientInfo.Pact.Name;
                    this.lblPactName.Text = "��ͬ��λ��" + this.myPatientInfo.Pact.Name;
     
                }
            }
            else
            {
                this.neuPanel1.Visible = false;
                this.lblDisplay.Visible = false;
                this.lblPactName.Visible = false;
            }
        }

        /// <summary>
        /// �޸Ĳ�ҩ{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// </summary>
        public void ModifyHerbal()
        {
            if (this.neuSpread1_Sheet1.RowCount == 0)
            {
                return;
            }

            ArrayList alModifyHerbal = new ArrayList(); //Ҫ�޸ĵĲ�ҩҽ��

            Neusoft.HISFC.Models.Order.OutPatient.Order orderTemp = this.neuSpread1_Sheet1.Rows[this.neuSpread1_Sheet1.ActiveRowIndex].Tag as
                Neusoft.HISFC.Models.Order.OutPatient.Order;

            if (orderTemp == null)
            {
                return;
            }

            //{F1706DB9-376D-433e-A5A9-1E1EEA46733C}  �����޸Ĳ�ҩҽ��
            if (orderTemp.Item.ItemType == EnumItemType.Drug)
            {
                if (((Neusoft.HISFC.Models.Pharmacy.Item)orderTemp.Item).Type.ID.ToString() != "C")
                {
                    MessageBox.Show("��ѡ���ҩҽ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (string.IsNullOrEmpty(orderTemp.Combo.ID))
            {
                alModifyHerbal.Add(orderTemp);
            }
            else
            {

                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order order = this.neuSpread1_Sheet1.Rows[i].Tag as 
                        Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (order == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(order.Combo.ID))
                    {
                        continue;
                    }
                    if (order.Status != 0)
                    {
                        Neusoft.FrameWork.WinForms.Classes.Function.Msg("ҽ������Ч�������޸ģ�\n�븴��ҽ��������ҽ�����޸ģ�", 411);
                        return;
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
                    uc.refreshGroup += new Neusoft.HISFC.Components.Order.Controls.RefreshGroupTree(uc_refreshGroup);//{7DBD1B62-BBE1-4a0d-A9D7-965975CFAE56}
                    Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                    uc.AlOrder = alModifyHerbal;
                    uc.OpenType = "M"; //�޸�
                    DialogResult r = Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);

                    if (uc.IsCancel == true)
                    {//ȡ����
                        return;
                    }

                    if (uc.OpenType == "M")
                    {//��Ϊ�¼�ģʽ�Ͳ�ɾ����
                        if (this.Del(this.neuSpread1.ActiveSheet.ActiveRowIndex, true) < 0)
                        {//ɾ��ԭҽ�����ɹ�
                            return;
                        }
                    }

                    if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                    {
                        foreach (Neusoft.HISFC.Models.Order.OutPatient.Order info in uc.AlOrder)
                        {
                            //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                            info.DoseOnce = info.Qty;
                            info.Qty = info.Qty * info.HerbalQty;

                            this.AddNewOrder(info, 0);
                        }
                        uc.Clear();
                        this.RefreshCombo();
                    }
                }
            }

        }

        #region {C6E229AC-A1C4-4725-BBBB-4837E869754E}

        /// <summary>
        /// ���״洢
        /// </summary>
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
            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.neuSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order order = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex).Clone();
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
                if (OnRefreshGroupTree != null)
                {
                    this.OnRefreshGroupTree(null, null);
                }
            }
        }

        #endregion

        #endregion

        #region ���з���
        /// <summary>
        /// ���ҽ��ʵ���FarPoint
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.OutPatient.Order GetObjectFromFarPoint(int i, int SheetIndex)
        {
            Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
            if (this.neuSpread1.Sheets[SheetIndex].Rows[i].Tag != null)
            {
                order = this.neuSpread1.Sheets[SheetIndex].Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
            }
            else
            {
                #region ��ֵ
                order = OrderManagement.QueryOneOrder(this.neuSpread1.Sheets[SheetIndex].Cells[i, iColumns[2]].Text);
                #endregion
            }
            
            return order;
        }

        /// <summary>
        /// �����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        public void AddNewOrder(object sender, int SheetIndex)
        {

            dirty = true;
            Neusoft.HISFC.Models.Order.OutPatient.Order newOrder = null;
            if (sender.GetType() == typeof(Neusoft.HISFC.Models.Order.OutPatient.Order))
            {
                newOrder = new Neusoft.HISFC.Models.Order.OutPatient.Order();
                newOrder.Name = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Name;
                newOrder.Memo = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Memo;
                newOrder.Combo = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Combo;
                newOrder.DoseOnce = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).DoseOnce;
                newOrder.DoseUnit = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).DoseUnit;
                newOrder.ExeDept = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).ExeDept.Clone();
                newOrder.Frequency = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Frequency;
                newOrder.StockDept = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).StockDept.Clone();
                newOrder.HerbalQty = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).HerbalQty;
                newOrder.IsEmergency = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).IsEmergency;
                newOrder.Item = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item;
                newOrder.Qty = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Qty;
                newOrder.Note = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Note;

                if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.SysClass.ID.ToString() == "UL")
                {
                    //newOrder.Sample.Name = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Sample.Name;
                    newOrder.Sample.Name = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).CheckPartRecord;

                }

                #region �޸Ĳ�λ---donggq---{60BCEF53-CDFC-410c-9F87-F530FB5E8416}

                else
                {
                    newOrder.CheckPartRecord = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).CheckPartRecord;
                }

                #endregion

                newOrder.Unit = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Unit;
                newOrder.Usage = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Usage;
                newOrder.IsNeedConfirm = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).IsNeedConfirm;
                if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).NurseStation.User03 == "" || ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).NurseStation.User03 == null)
                {
                    newOrder.NurseStation.User03 = "1";//��С��λ
                }
                else
                {
                    newOrder.NurseStation.User03 = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).NurseStation.User03;
                }
                sender = newOrder;

            }
            //�������
            if (sender.GetType() == typeof(Neusoft.HISFC.Models.Order.OutPatient.Order))
            {
                #region �����ӵĶ���
                if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.SysClass.ID.ToString() == "UC")//���
                {
                    //��ӡ������뵥
                    ////this.AddTest(sender);
                }
                else if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.SysClass.ID.ToString() == "MC")//����
                {
                    //��ӻ�������
                    ////this.AddConsultation(sender);
                }
                //if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.IsPharmacy)//ҩƷ
                if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.ItemType == EnumItemType.Drug)//ҩƷ
                {
                    if (((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item).IsAllergy)
                    {
                        if (this.hypotestMode == "1")
                        {
                            if (MessageBox.Show(((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.Name + "�Ƿ���ҪƤ�ԣ�", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                ((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item).IsAllergy = false;
                                ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).HypoTest = 4;
                                ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.Name += "�ۡ���";
                            }
                            else
                            {
                                (sender as Neusoft.HISFC.Models.Order.OutPatient.Order).HypoTest = 2;
                                ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Memo += Order.Classes.Function.TipHypotest;
                            }
                        }
                        else if (this.hypotestMode == "2")//{0733E2AD-EB02-4b6f-BCF8-1A6ED5A2EFAD}
                        {

                            HISFC.Components.Order.OutPatient.Forms.frmHypoTest frmHypotest = new Neusoft.HISFC.Components.Order.OutPatient.Forms.frmHypoTest();

                            frmHypotest.IsEditMode = true;
                            frmHypotest.Hypotest = 1;
                            frmHypotest.ShowDialog();

                            ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).HypoTest = frmHypotest.Hypotest;
                        }
                    }
                    //�ж�ҩƷ�Ƿ���ҩ������ʾ
                    if (((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item).Quality.ID == "S")
                    {
                        MessageBox.Show("��ͬʱ���ӿ����ֹ�����ҩ����!");
                    }
                    if (((Neusoft.HISFC.Models.Pharmacy.Item)((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item).Quality.ID == "P")
                    {
                        MessageBox.Show("���ྫ��ҩƷ��ͬʱ���ӿ����ֹ�����!");
                    }


                }
                #endregion

                Neusoft.HISFC.Models.Order.OutPatient.Order order = sender as Neusoft.HISFC.Models.Order.OutPatient.Order;
                #region �ն�ȷ�ϵ�ҩƷ
                //if (((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
                //{
                //    Neusoft.HISFC.Models.Pharmacy.Item pha = ((Neusoft.HISFC.Models.Order.OutPatient.Order)sender).Item as Neusoft.HISFC.Models.Pharmacy.Item;
                //    if (pha.IsAppend && order.ExeDept.ID.Length <= 0)
                //    {
                //        neusoft.neHISFC.Components.Interface.Forms.frmEasyChoose frmPop = new neusoft.neHISFC.Components.Interface.Forms.frmEasyChoose(this.alDepts);
                //        frmPop.Text = "�ն�ȷ�ϵ�ҩƷ��Ҫ��ִ�п�����ȡ��ʹ�ã���ѡ�񡣡���";
                //        frmPop.StartPosition = FormStartPosition.CenterScreen;
                //        frmPop.SelectedItem += new neusoft.neHISFC.Components.Interface.Forms.SelectedItemHandler(frmPop_SelectedItem);
                //        DialogResult r = frmPop.ShowDialog();
                //        if (r != DialogResult.OK)
                //        {
                //            MessageBox.Show("��ѡ��Ҫ���µĿ������");
                //            return;
                //        }
                //        order.ExeDept = this.objExeDept;
                //    }
                //}
                #endregion
                if (order.NurseStation.User03 == "")
                {
                    order.NurseStation.User03 = "1";//��С��λ
                }
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

                DateTime dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
                if (!this.EditGroup)
                {
                    if (this.myPatientInfo != null)
                    {
                        order.InDept = this.myPatientInfo.DoctorInfo.Templet.Dept;//�Һſ���
                        Neusoft.HISFC.Models.Base.PactInfo pactInfo = this.myPatientInfo.Pact as Neusoft.HISFC.Models.Base.PactInfo;
                        order.Item.Price = Classes.Function.GetPrice(order, this.myPatientInfo, pactInfo);
                    }
                }
                
                //����ҽ������ʱ��
                if (Order.Classes.Function.IsDefaultMoDate == false)
                {
                    if (dtNow.Hour >= 12)
                        order.BeginTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 12, 0, 0);
                    else
                        order.BeginTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);
                }
                else//��Ĭ��ʱ��
                {
                    order.BeginTime = dtNow;
                }
                if (order.User03 != "")//���׵�ʱ����
                {
                    int iDays = Neusoft.FrameWork.Function.NConvert.ToInt32(order.User03);
                    if (iDays > 0)//��ʱ����>0
                    {
                        order.BeginTime = order.BeginTime.AddDays(iDays);
                    }
                }

                order.CurMOTime = DateTime.MinValue;
                order.NextMOTime = DateTime.MinValue;
                order.EndTime = DateTime.MinValue;

                if (order.Sample.Name.Length <= 0 && order.Item.SysClass.ID.ToString() == "UL")
                {
                    order.Sample.Name = order.CheckPartRecord;
                }

                this.currentOrder = order;
                this.neuSpread1.Sheets[SheetIndex].Rows.Add(0, 1);
                this.AddObjectToFarpoint(order, 0, SheetIndex, EnumOrderFieldList.Item);

                RefreshOrderState();
                
                #region ���������뵥��
                //if (order.Item.Price == 0 && order.Unit != "[������]" && order.Item.ID != "999")
                //{
                //    Forms.frmPopShow frm = new Forms.frmPopShow();
                //    frm.Text = "����ĿΪ��������Ŀ��������۸�";
                //    frm.isPrice = true;
                //    frm.ShowDialog();
                //    order.Item.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(frm.ModuleName);

                //}
                # endregion
            }
            else
            {
                MessageBox.Show("������Ͳ���ҽ�����ͣ�");
            }
            dirty = false;
        }

        /// <summary>
        /// ��Ӳ�ҩҽ��{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// </summary>
        /// <param name="alHerbalOrder"></param>
        public void AddHerbalOrders(ArrayList alHerbalOrder)
        {

            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988} //��ҩ������ҩ��������
            using (Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder uc = new Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder(true,Neusoft.HISFC.Models.Order.EnumType.SHORT, this.GetReciptDept().ID))
            {
                uc.Patient = new Neusoft.HISFC.Models.RADT.PatientInfo();//
                uc.refreshGroup += new Neusoft.HISFC.Components.Order.Controls.RefreshGroupTree(uc_refreshGroup);//{7DBD1B62-BBE1-4a0d-A9D7-965975CFAE56}
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                uc.AlOrder = alHerbalOrder;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                {
                    foreach (Neusoft.HISFC.Models.Order.OutPatient.Order info in uc.AlOrder)
                    {
                        //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                        info.DoseOnce = info.Qty;
                        info.Qty = info.Qty * info.HerbalQty;

                        this.AddNewOrder(info, 0);
                    }
                    uc.Clear();
                    this.RefreshCombo();

                }
            }
        }

        /// <summary> 
        /// �����������
        /// </summary>
        public void AddOperation()
        {
            ////���޸�
        }

        /// <summary>
        /// ��Ӽ�顢��������
        /// </summary>
        public void AddTest()
        {
            if (this.Patient == null)
            {
                MessageBox.Show("����ѡ���ߣ�");
                return;
            }
            List<Neusoft.HISFC.Models.Order.OutPatient.Order> alItems = new List<Neusoft.HISFC.Models.Order.OutPatient.Order>();
            //int iActiveSheet = 1;//��鵥Ĭ����ʱҽ��
            for (int i = 0; i < this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].RowCount; i++)
            {
                if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].IsSelected(i, 0))
                {
                    //��alItems���ݸ�Ϊorder����
                    alItems.Add(this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex));
                }
            }
            if (alItems.Count <= 0)
            {
                //û��ѡ����Ŀ��Ϣ
                MessageBox.Show("��ѡ�����ļ����Ϣ!");
                return;
            }

            // {78F4ED37-7A2E-4e57-8D88-F2DA9C702673} xupan
            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order undrug in alItems)
            {
                #region  ��鲿λ������걾�ж� {0A11E21D-2A24-4c70-BD47-709DAE00BB95} wbo 2011-03-17
                //if (undrug.Item.SysClass.ID.ToString() == "UC")
                //{
                //    if (string.IsNullOrEmpty(undrug.Sample.Name))
                //    {
                //        MessageBox.Show("����д��鲿λ");
                //        return;
                //    }
                //}
                //else
                //{
                //    break;
                //}
                if (undrug.Item.SysClass.ID.ToString() == "UC")
                {
                    if (string.IsNullOrEmpty(undrug.CheckPartRecord))
                    {
                        MessageBox.Show("����д��鲿λ");
                        return;
                    }
                }
                if (undrug.Item.SysClass.ID.ToString() == "UL")
                {
                    if (string.IsNullOrEmpty(undrug.Sample.Name))
                    {
                        MessageBox.Show("����д��������");
                        return;
                    }
                }
                #endregion
            }
            // xupan end

            if (this.checkPrint == null)
            {
                this.checkPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint)) as Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint;
                if (this.checkPrint == null)
                {
                    MessageBox.Show("��ýӿ�IcheckPrint����\n������û��ά����صĴ�ӡ�ؼ����ӡ�ؼ�û��ʵ�ֽӿڼ���ӿ�IcheckPrint\n����ϵͳ����Ա��ϵ��");
                    return;
                }
            }
            this.checkPrint.Reset();
            this.checkPrint.ControlValue(Patient, alItems);
            this.checkPrint.Show(); 
        }

        /// <summary>
		/// ��ӻ���
		/// </summary>
		/// <param name="sender"></param>
        public void AddConsultation(object sender)
        {
            ////���޸�
        }

        ///<summary>
        /// ˢ�����
        /// </summary>
        public void RefreshCombo()
        {
            if (this.IsDesignMode) this.neuSpread1.Sheets[0].SortRows(iColumns[28], false, false);
            Order.Classes.Function.DrawCombo(this.neuSpread1.Sheets[0], iColumns[4], 8);
        }

        /// <summary>
        /// reset
        /// </summary>
        public void Reset()
        {
            this.ucOutPatientItemSelect1.Clear();

            this.ucOutPatientItemSelect1.ucInputItem1.Select();
            this.ucOutPatientItemSelect1.ucInputItem1.Focus();
        }

        /// <summary>
        /// ����ҽ��״̬
        /// </summary>
        public void RefreshOrderState()
        {
            try
            {
                for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 0, false);
                }
                
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ˢ��ҽ��״̬ʱ���ֲ���Ԥ֪�������˳������������Ի������Ա��ϵ"));
            }
            this.SetOrderFeeDisplay();
        }
        public void RefreshOrderState(bool reset)
        {
            try
            {
                for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
                {
                    this.ChangeOrderState(i, 0, reset);
                }
                
            }
            catch
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ˢ��ҽ��״̬ʱ���ֲ���Ԥ֪�������˳������������Ի������Ա��ϵ"));
            }
        }

        /// <summary>
        /// ���ҽ���Ϸ���
        /// </summary>
        /// <returns></returns>
        public int CheckOrder()
        {
            Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
            int iCheck = Classes.Function.GetIsOrderCanNoStock();
            bool IsModify = false;
            //����ҩƷ����Ȩ��ά�� {4D5E0EB4-E673-478b-AE8C-6A537F49FC5C}
            int returnValue = 1;
            
            //��ʱҽ��
            for (int i = 0; i < this.neuSpread1.Sheets[0].RowCount; i++)
            {
                order = (Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.Sheets[0].Rows[i].Tag;
                if (order.Status == 0)
                {
                    //δ��˵�ҽ��
                    IsModify = true;
                    //if (order.Item.IsPharmacy)
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
                            if (order.HerbalQty == 0) { ShowErr(order.Item.Name + "��������Ϊ�㣡", i, 0); return -1; }
                        }
                        else
                        {
                            //����
                            if (order.DoseOnce == 0) { ShowErr(order.Item.Name + "ÿ�μ�������Ϊ�㣡", i, 0); return -1; }
                            if (order.DoseUnit == "") { ShowErr(order.Item.Name + "������λ����Ϊ�գ�", i, 0); return -1; }
                        }
                        if (order.Qty == 0) { ShowErr(order.Item.Name + "��������Ϊ�գ�", i, 0); return -1; }
                        if (order.Unit == "") { ShowErr(order.Item.Name + "��λ����Ϊ�գ�", i, 0); return -1; }
                        if (order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 0); return -1; }
                        if (order.Usage.ID == "") { ShowErr(order.Item.Name + "�÷�����Ϊ�գ�", i, 0); return -1; }
                        if ((order.DoseOnce / ((Neusoft.HISFC.Models.Pharmacy.Item)order.Item).BaseDose) > order.Qty
                            && order.Unit == order.DoseUnit)
                        {
                            ShowErr(order.Item.Name + "ÿ�����������Դ���������", i, 0);
                            return -1;
                        }
                        //���ڲ��ɲ�ְ�װ��λ��ҩƷ �жϿ����Ƿ���ȷ
                        if (((Neusoft.HISFC.Models.Pharmacy.Item)order.Item).SplitType == "1")
                        {
                            if (order.NurseStation.User03 != "0")       //��λΪ��С��λ ���������ж�
                            {
                                long minPackQty;
                                System.Math.DivRem((long)order.Qty, (long)order.Item.PackQty, out minPackQty);
                                if (minPackQty != 0)
                                {
                                    ShowErr(string.Format("{0}{1}  �������۳�ʱ��������װ�۳����뿪��{2}����������", order.Item.Name, order.Item.Specs, order.Item.PackQty.ToString()), i, 0);
                                    return -1;
                                }
                            }
                        }
                        //�����

                        if (order.StockDept != null && order.StockDept.ID != "")
                        {
                            decimal storeNum = order.Qty;
                            if (pManagement.GetStorageNum(order.StockDept.ID, order.Item.ID, out storeNum) == 1)
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
                                ShowErr(order.Item.Name + "����ж�ʧ��!" + pManagement.Err, i, 0);
                                return -1;
                            }
                        }
                        else
                        {
                            if (Classes.Function.CheckPharmercyItemStock(iCheck, order.Item.ID, order.Item.Name, order.ReciptDept.ID, order.Qty) == false)
                            {
                                ShowErr(order.Item.Name + "��治��!", i, 0); return -1;
                            }
                        }
                    }
                    else
                    {
                        //��ҩƷ
                        //if (order.Frequency.ID == "") { ShowErr("Ƶ�β���Ϊ�գ�", i, 1); return -1; }
                        if (order.Qty == 0) { ShowErr(order.Item.Name + "��������Ϊ�գ�", i, 0); return -1; }
                        if (order.ExeDept.ID == "") { ShowErr(order.Item.Name + "��ѡ��ִ�п��ң�", i, 0); return -1; }

                        // xupan
                        //if (order.Item.SysClass.ID.ToString() == "UC")
                        //{
                        //    if (string.IsNullOrEmpty(order.Sample.Name))
                        //    {
                        //        ShowErr("��鲿λ����Ϊ�գ�", i, 0);
                        //        return -1;
                        //    }
                        //}
                    }
                    if (Neusoft.FrameWork.Public.String.ValidMaxLengh(order.Memo, 80) == false)
                    {
                        ShowErr(order.Item.Name + "�ı�ע����!", i, 0);
                        return -1;
                    }
                    if (order.Qty > 5000)
                    { ShowErr("����̫��", i, 0); return -1; }
                    if (order.Item.Price == 0)
                    {
                        ShowErr(order.Item.Name + "���۱�����ڣ���", i, 0);
                        return -1;
                    }
                    if (order.ID == "") IsModify = true;
                }
            }

            if (IsModify == false) return -1;//δ����¼���ҽ��

            return 0;

        }

        /// <summary>
        /// ���ҽ��
        /// </summary>
        public void ComboOrder()
        {
            ComboOrder(this.neuSpread1.ActiveSheetIndex);
            this.RefreshCombo();
            //{D96CEC1D-77BF-434f-B440-D1988F73223C}  �����ʾ
            this.ucOutPatientItemSelect1.Clear();
        }
        
        /// <summary>
        /// ȡ�����
        /// </summary>
        public void CancelCombo()
        {
            if (this.neuSpread1.ActiveSheet.SelectionCount <= 1) return;
            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (this.neuSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order o = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex);
                    #region {4F784E81-CB1D-4bd5-AC27-CDE08A79196D}
                    //if (o.ID != null && o.ID != "")
                    //{
                    //    Neusoft.HISFC.Models.Order.OutPatient.Order tmpO = this.OrderManagement.QueryOneOrder(o.ID);
                    //    if (tmpO != null)
                    //    {
                    //        if (tmpO.Status == 0 || tmpO.Status == 6)
                    //        {
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("ҽ��״̬�Ѿ��仯��������ȡ����ϣ�");
                    //            return;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region �ж�������Ѿ��������ҽ������Ҫɾ��ԭ���ĸ���{F67E089F-1993-4652-8627-300295AAED8C}

                    if (o.ID != null && o.ID != "")
                    {
                        
                        #region ҽ�����ĸ��ĵ�ɾ��

                        if (!hsComboChange.ContainsKey(o.ID))
                        {
                            hsComboChange.Add(o.ID, o.Combo.ID);
                        }

                        o.NurseStation.User02 = "C";

                        #endregion
                        
                    }

                    #endregion



                    o.Combo.ID = this.OrderManagement.GetNewOrderComboID();
                    #region {4F5BEF6C-48FE-4abb-84F2-091838D7BA03}
                    //o.SortID = MaxSort + 1;
                    //MaxSort = MaxSort + 1;
                    #endregion
                    this.AddObjectToFarpoint(o, i, this.neuSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
                }
            }
            this.neuSpread1.ActiveSheet.ClearSelection();
            this.RefreshCombo();
            //{D96CEC1D-77BF-434f-B440-D1988F73223C}  �����ʾ
            this.ucOutPatientItemSelect1.Clear();

        }

        /// <summary>
        /// ��þ�����ͬ��Ϻŵ�ҽ��
        /// </summary>
        /// <returns></returns>
        public ArrayList GetOrderHaveSameCombID(string combID)
        {
            if (combID == "" || combID == null)
            {
                return null;
            }
            ArrayList alOrder = new ArrayList();
            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                if (this.neuSpread1_Sheet1.Cells[i, iColumns[4]].Text == combID)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp = this.GetObjectFromFarPoint(i, 0);
                    //Ϊ�� ����
                    if (temp == null)
                    {
                        continue;
                    }
                    //���
                    alOrder.Add(temp);
                }
            }
            return alOrder;
        }

        public void SetEditGroup(bool isEdit)
        {
            this.EditGroup = isEdit;
            this.ucOutPatientItemSelect1.Visible = isEdit;
            if (this.ucOutPatientItemSelect1 != null)
                this.ucOutPatientItemSelect1.EditGroup = isEdit;

            this.neuSpread1.Sheets[0].DataSource = null;

            this.neuSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            
        }

       //zhangyt  2011-02-22
        public void PrintOrder()
        {
            InterfaceInstanceDefault.IRecipePrint.ucOutPatientOrderPrint orderPrint = new InterfaceInstanceDefault.IRecipePrint.ucOutPatientOrderPrint();
            ArrayList alOrder = new ArrayList();
            alOrder = OrderManagement.QueryOrder(this.myPatientInfo.DoctorInfo.SeeNO.ToString());
            if (alOrder.Count == 0)
            {
                MessageBox.Show("û�п��Դ�ӡ��ҽ�������ȱ���ҽ�����ٴ�ӡ��");
                return ;
            }
            orderPrint.setPrintInfo(alOrder);
            orderPrint.SetPatient(this.Patient);
            orderPrint.PrintOrder();

            //this.neuSpread1.PrintSheet(this.neuSpread1_Sheet1);
        }

        #region {DF8058FF-72C0-404f-8F36-6B4057B6F6CD}
        /// <summary>
        /// ճ��ҽ��
        /// </summary>
        public void PasteOrder()
        {
            try
            {

                List<string> orderIdList = Neusoft.HISFC.Components.Order.Classes.HistoryOrderClipboard.OrderList;

                if ((orderIdList == null) || (orderIdList.Count <= 0))
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��������û�п���ճ����ҽ����"));
                    return;
                }

                if (Neusoft.HISFC.Components.Order.Classes.HistoryOrderClipboard.Type == ServiceTypes.C)
                {
                    Neusoft.HISFC.BizLogic.Order.OutPatient.Order fnc = new Neusoft.HISFC.BizLogic.Order.OutPatient.Order();
                    for (int count = 0; count < orderIdList.Count; count++)
                    {
                        Neusoft.HISFC.Models.Order.OutPatient.Order order = fnc.QueryOneOrder(orderIdList[count]);
                        if (order != null && order.ID != "")
                        {
                            //ҽ��״̬����
                            order.Status = 0;
                            order.ID = "";

                            order.MOTime = this.OrderManagement.GetDateTimeFromSysDateTime();
                            order.Combo.ID = "";
                            //��ӵ���ǰ����� ����ҽ�����ͽ��з���
                            this.AddNewOrder(order, 0);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����԰�סԺ��ҽ������Ϊ����ҽ����"));
                    return;
                }

            }
            catch { }
        }
        #endregion

        #region {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}

        /// <summary>
        /// ����ҽ��
        /// �����Ƶ�ҽ�������Ǳ�����ģ���ҽ����ˮ�ŵģ�
        /// ����ճ��ʱ������
        /// </summary>
        public void CopyOrder()
        {
            if (this.neuSpread1_Sheet1.Rows.Count <= 0) return;

            ArrayList list = new ArrayList();

            //��ȡѡ���е�ҽ��ID
            for (int row = 0; row < this.neuSpread1_Sheet1.Rows.Count; row++)
            {
                if (this.neuSpread1_Sheet1.IsSelected(row, 0))
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order ord = this.GetObjectFromFarPoint(row, 0);

                    if (ord == null || string.IsNullOrEmpty(ord.ID))
                    {
                        continue;
                    }
                    else
                    {
                        list.Add(ord.ID);
                    }

                }
            }

            if (list.Count <= 0) return;
            //����ӵ�COPY�б�
            for (int count = 0; count < list.Count; count++)
            {
                HISFC.Components.Order.Classes.HistoryOrderClipboard.Add(list[count]);
            }
            string type = "1";
            HISFC.Components.Order.Classes.HistoryOrderClipboard.Add(type);
            //Ȼ��copy�б�ŵ���������
            HISFC.Components.Order.Classes.HistoryOrderClipboard.Copy();
        }

        #endregion

        #endregion

        #region �¼�

        /// <summary>
        /// ҽ���仯����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changedField"></param>
        protected virtual void ucItemSelect1_OrderChanged(Neusoft.HISFC.Models.Order.OutPatient.Order sender, EnumOrderFieldList changedField)
        {
            dirty = true;
            if (!this.EditGroup && !this.bIsDesignMode)
                return;

            if (!this.EditGroup)//{E679E3A6-9948-41a8-B390-DD9A57347681}�жϲ��ǿ���ҽ��ģʽ�Ͳ�������ӿ�
            {
                #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�
                //{48E6BB8C-9EF0-48a4-9586-05279B12624D}
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

            if (this.ucOutPatientItemSelect1.OperatorType == Operator.Add)
            {

                this.AddNewOrder(sender, this.neuSpread1.ActiveSheetIndex);
                this.neuSpread1.ActiveSheet.ClearSelection();
                this.neuSpread1.ActiveSheet.AddSelection(0, 0, 1, 1);
                this.neuSpread1.ActiveSheet.ActiveRowIndex = 0;
                this.SelectionChanged();
            }
            else if (this.ucOutPatientItemSelect1.OperatorType == Operator.Delete)
            {

            }
            else if (this.ucOutPatientItemSelect1.OperatorType == Operator.Modify)
            {
                //�޸�
                if (this.neuSpread1.ActiveSheet.SelectionCount > 1)
                {
                    ArrayList alRows = GetSelectedRows();
                    for (int i = 0; i < alRows.Count; i++)
                    {
                        if (this.ucOutPatientItemSelect1.CurrentRow == System.Convert.ToInt32(alRows[i]))
                        {
                            this.AddObjectToFarpoint(sender, this.ucOutPatientItemSelect1.CurrentRow, this.neuSpread1.ActiveSheetIndex, changedField);
                        }
                        else
                        {
                            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.GetObjectFromFarPoint(int.Parse(alRows[i].ToString()), this.neuSpread1.ActiveSheetIndex);
                            if (order.Combo.ID == sender.Combo.ID)
                            {
                                if (changedField == EnumOrderFieldList.Item 
                                    || changedField == EnumOrderFieldList.Frequency
                                    || changedField == EnumOrderFieldList.BeginDate
                                    || changedField == EnumOrderFieldList.EndDate
                                    || changedField == EnumOrderFieldList.Emc
                                    //{AA8348EF-8669-4ebf-B863-95469A7A04E2}�����޸ĵ�λ����������е�λ�����ű仯
                                    //|| changedField == EnumOrderFieldList.Unit 
                                    || changedField == EnumOrderFieldList.Fu)
                                {
                                    //��ϵ�һ���޸�
                                    if (order.Item.SysClass.ID.ToString() != "PCC") order.Usage = sender.Usage.Clone();
                                    order.HerbalQty = sender.HerbalQty;
                                    order.Frequency.ID = sender.Frequency.ID;
                                    order.Frequency.Name = sender.Frequency.Name;
                                    order.Frequency.Time = sender.Frequency.Time;
                                    order.BeginTime = sender.BeginTime;
                                    order.EndTime = sender.EndTime;
                                    //{AA8348EF-8669-4ebf-B863-95469A7A04E2}�����޸ĵ�λ����������е�λ�����ű仯
                                    //order.Unit = sender.Unit;
                                    order.IsEmergency = sender.IsEmergency;
                                    
                                    this.AddObjectToFarpoint(order, int.Parse(alRows[i].ToString()), this.neuSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
                                }
                                else if (changedField == EnumOrderFieldList.Usage)
                                {
                                    order.Usage = sender.Usage;
                                    order.Frequency.Usage = sender.Frequency.Usage.Clone();
                                    if (!Classes.Function.hsUsageAndSub.Contains(order.Usage.ID))
                                    {
                                        order.InjectCount = 0;
                                    }
                                    
                                    this.AddObjectToFarpoint(order, int.Parse(alRows[i].ToString()), this.neuSpread1.ActiveSheetIndex, EnumOrderFieldList.Item);
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.AddObjectToFarpoint(sender, this.ucOutPatientItemSelect1.CurrentRow, this.neuSpread1.ActiveSheetIndex, changedField);
                }
                RefreshOrderState();

            }
            dirty = false;

            this.isEdit = true;
        }

        /// <summary>
        /// cellchange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_Sheet1_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
            try
            {
                if (this.bIsDesignMode && dirty == false)
                {
                    int i = 0;
                    switch (GetColumnNameFromIndex(e.Column))
                    {
                        case "�÷�����":
                            i = this.GetColumnIndexFromName("�÷�����");
                            this.neuSpread1.ActiveSheet.Cells[e.Row, i].Text =
                                Order.Classes.Function.HelperUsage.GetName(this.neuSpread1.ActiveSheet.Cells[e.Row, e.Column].Text);
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
        /// ѡ��ҽ���޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }

        #endregion

        #region IToolBar ��Ա

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Del()
        {//{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988} �ع���һ��ɾ������
            return Del(this.neuSpread1.ActiveSheet.ActiveRowIndex, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex">������</param>
        /// <param name="isDirctDel">�Ƿ�ֱ��ɾ��������ʾ��</param>
        /// <returns></returns>
        private int Del(int rowIndex, bool isDirctDel)
        {//{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988} �ع���һ��ɾ������
            #region ȫ��ɾ������
            int j = rowIndex;
            DialogResult r = DialogResult.Yes;
            bool isHavePha = false;
            Neusoft.HISFC.Models.Order.OutPatient.Order order = null;//,temp=null;
            if (j < 0 || this.neuSpread1.ActiveSheet.RowCount == 0)
            {
                MessageBox.Show("����ѡ��һ��ҽ����");
                return 0;
            }
            for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
            {
                //Clear Selected Flag
                this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Tag = "";
            }
            for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
            {
                //��־����ѡ����
                if (this.neuSpread1.Sheets[0].IsSelected(i, 0))
                {

                    this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Tag = "1";
                }
            }
            if (!isDirctDel)
            {
                r = MessageBox.Show("�Ƿ�ɾ����ѡ��ҽ��\n *�˲������ܳ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (r == DialogResult.Yes)
            {
                for (int i = this.neuSpread1_Sheet1.Rows.Count - 1; i >= 0; i--)
                {
                    if (this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Tag != null
                        && this.neuSpread1.Sheets[0].Cells[i, iColumns[0]].Tag.ToString() == "1")
                    {
                        order = (Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.ActiveSheet.Rows[i].Tag;

                        if (order == null)
                        {
                            continue;
                        }
                        if (order.Status == 0)
                        {
                            if (order.ReciptDoctor.ID != this.OrderManagement.Operator.ID)
                            {
                                MessageBox.Show("��ҽ�����ǵ�ǰҽ������,����ɾ��!", "��ʾ");
                                return 0;
                            }
                            if (order.ExtendFlag1 != null)
                            {
                                string[] strSplit = order.ExtendFlag1.Split('|');
                                if (strSplit.Length == 3)
                                {
                                    if (MessageBox.Show("ҽ��" + order.Item.Name + "�Ѿ������˽�ƿ,ȷ��ɾ����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                    {
                                        return 0;
                                    }
                                    for (int kk = 0; kk < this.neuSpread1_Sheet1.Rows.Count; kk++)
                                    {
                                        Neusoft.HISFC.Models.Order.OutPatient.Order tem = this.GetObjectFromFarPoint(kk, 0);

                                        if (tem != null && tem.ExtendFlag1 != null && tem.Combo.ID != order.Combo.ID && tem.ExtendFlag1.Split('|').Length == 3 && tem.ExtendFlag1.Split('|')[1] == order.Combo.ID)
                                        {
                                            tem.NurseStation.User02 = "C";
                                            tem.ExtendFlag1 = tem.ExtendFlag1.Split('|')[0];
                                        }
                                    }
                                }
                            }
                            if (order.ID == "") //��Ȼɾ��
                            {
                                this.neuSpread1.ActiveSheet.Rows.Remove(i, 1);
                            }
                            else //delete from table
                            {
                                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.OrderManagement.Connection);
                                //t.BeginTransaction();
                                OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                feeManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                Neusoft.HISFC.Models.Order.OutPatient.Order temp = OrderManagement.QueryOneOrder(order.ID);
                                if (temp == null)
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                    this.neuSpread1.ActiveSheet.Rows.Remove(i, 1);
                                }
                                else
                                {
                                    if (OrderManagement.DeleteOrder(order.SeeNO, Neusoft.FrameWork.Function.NConvert.ToInt32(order.ID)) <= 0)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                        MessageBox.Show(order.Item.Name + "�����Ѿ��շѣ����˳�������������" + OrderManagement.Err);
                                        return -1;
                                    }
                                    //
                                    int iReturn = -1;
                                    int combCount = this.GetOrderHaveSameCombID(order.Combo.ID).Count;
                                    if (combCount > 1)
                                    {
                                        iReturn = feeManagement.DeleteFeeItemListByRecipeNO(order.ReciptNO, order.SequenceNO.ToString());
                                    }
                                    else if (combCount == 1)
                                    {
                                        iReturn = feeManagement.DeleteFeeItemListByMoOrder(order.ID);
                                    }
                                    if (iReturn < 0)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                        MessageBox.Show(order.Item.Name + "�����Ѿ��շѣ����˳�������������" + feeManagement.Err);
                                        return -1;
                                    }

                                    #region ҽ�����ĸ��ĵ�ɾ��{D256A1B3-F969-4d2c-92C3-9A5508835D5B}
                                    ArrayList alSubAndOrder = feeManagement.QueryFeeDetailbyComoNOAndClinicCode(order.Combo.ID, this.myPatientInfo.ID);
                                    if (alSubAndOrder != null && alSubAndOrder.Count > 0)
                                    {
                                        for (int s = 0; s < alSubAndOrder.Count; s++)
                                        {
                                            Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = alSubAndOrder[s] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;
                                            if (item.Item.IsMaterial)
                                            {
                                                if (feeManagement.DeleteFeeItemListByRecipeNO(item.RecipeNO, item.SequenceNO.ToString()) < 0)
                                                {
                                                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                                    MessageBox.Show(feeManagement.Err, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                                    return -1;
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                                    #region �˻����� ����
                                    //int resultValue = 0;
                                    //if (accountProcess && Patient.IsAccount)
                                    //{
                                    //    //ɾ��ҩƷ������Ϣ
                                    //    if (order.Item.ItemType == EnumItemType.Drug)
                                    //    {
                                    //        if (!order.IsHaveCharged)
                                    //        {
                                    //            resultValue = this.pManagement.DelApplyOut(order);
                                    //            if (resultValue < 0)
                                    //            {
                                    //                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                    //                MessageBox.Show(pManagement.Err);
                                    //                return -1;
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        //ɾ����ҩƷ�ն�������Ϣ
                                    //        if (order.Item.IsNeedConfirm && !order.IsHaveCharged)
                                    //        {
                                    //            resultValue = confrimIntegrate.DelTecApply(order.ReciptNO, order.SequenceNO.ToString());
                                    //            if (resultValue <= 0)
                                    //            {
                                    //                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                    //                MessageBox.Show("ɾ���ն�������Ϣʧ�ܣ�" + confrimIntegrate.Err);
                                    //                return -1;
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    #endregion

                                    order.DCOper.ID = this.OrderManagement.Operator.ID;
                                    order.DCOper.OperTime = this.OrderManagement.GetDateTimeFromSysDateTime();
                                    if (OrderManagement.InsertOrderChangeInfo(order) < 0)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                        MessageBox.Show("����" + order.Item.Name + "�޸ļ�¼����" + OrderManagement.Err);
                                        return -1;
                                    }

                                    this.neuSpread1.ActiveSheet.Rows.Remove(i, 1);
                                    Neusoft.FrameWork.Management.PublicTrans.Commit();

                                    //if (order.Item.IsPharmacy)
                                    if (order.Item.ItemType == EnumItemType.Drug)
                                    {
                                        isHavePha = true;
                                    }

                                    #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
                                    //string isUseDL = feeManagement.GetControlValue("200212", "0");//{457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
                                    if (isUseDL)
                                    {
                                        if (order.ApplyNo != null)
                                        {
                                            if (PACSApplyInterface == null)
                                            {
                                                if (InitPACSApplyInterface() < 0)
                                                {
                                                    MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                                                    return -1;
                                                }
                                            }
                                            PACSApplyInterface.DeleteApply(order.ApplyNo);
                                            //if (PACSApplyInterface.DeleteApply(order.ApplyNo) < 0)
                                            //{
                                            //    MessageBox.Show("���ϵ������뵥ʱ����");
                                            //    return -1;
                                            //}
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("ҽ��:[" + order.Item.Name + "]�Ѿ��շѣ����ܽ���ɾ��������", "��ʾ");
                            continue;
                        }
                    }
                }
                if (this.EnabledPass && isHavePha)
                {
                    ////this.PassSaveCheck(this.GetPhaOrderArray(), 1, true);
                }
                ////SetFeeDisplay(this.Patient, null);
            }
            this.ucOutPatientItemSelect1.Clear();

            this.RefreshCombo();
            this.RefreshOrderState();
            #endregion

            return 0;
        }

        /// <summary>
        /// exit
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
        /// ��ӣ�����
        /// </summary>
        /// <returns></returns>
        public int Add()
        {
            //���ʱ���Ѿ����뻼����Ϣ
            if (this.myPatientInfo == null || this.myPatientInfo.ID == "")
            {
                MessageBox.Show("û��ѡ���ߣ���˫��ѡ����");
                return -1;
            }
            this.ucOutPatientItemSelect1.Clear();
            this.IsDesignMode = true;
            this.InitDrugList();
            this.ucOutPatientItemSelect1.Focus();
            return 0;
        }

        public void SetDrugListVisable(bool isShow)
        {
            this.neuPanel2.Visible = isShow;  
        }

        /// <summary>
        /// {24BDD373-4F2C-4899-88A7-FE2E8386F7CF}
        /// </summary>
        private void InitDrugList()
        {
            this.neuPanel2.Controls.Clear();
            ArrayList alLevelClass = assignManagement.GetConstantList("LEVELCLASS");
            ArrayList alItemLevel = new ArrayList();
            ucDrugList uc=null;
            int i = 0;
            if (alLevelClass.Count > 0)
            {
                foreach (Neusoft.FrameWork.Models.NeuObject neuObj in alLevelClass)
                {
                    SetDrugListVisable(true);
                    this.neuPanel2.Height = 270;
                    alItemLevel = this.itemLevelManager.GetAllItemByFolderAndItemClass("ROOT", neuObj.ID);       
                    if (alItemLevel.Count != 0)
                    {
                        i++;
                        uc = new ucDrugList();
                        uc.DrugItem = neuObj.Name;
                        uc.GetDrugList += new ucDrugList.GetDrugItem(uc_GetDrugList);
                        uc.Init(alItemLevel);
                        this.neuPanel2.Controls.Add(uc);
                        if (i > 4 )
                        {
                            if (i % 4 == 0)
                            {
                                uc.Location = new Point(200 * 3, uc.Height * (i / 4 - 1));
                            }
                            else
                            {
                                uc.Location = new Point(200 * (i % 4 - 1), uc.Height * (i / 4 ));
                            }
                        }
                        else
                        {
                            uc.Location = new Point(200 * (i - 1), uc.Location.Y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
        /// </summary>
        public void Clear()
        {
            this.ucOutPatientItemSelect1.Clear();
        }

        /// <summary>
        /// {24BDD373-4F2C-4899-88A7-FE2E8386F7CF}
        /// </summary>
        /// <param name="drugObj"></param>
        private void uc_GetDrugList(Neusoft.HISFC.Models.Pharmacy.Item drugObj)
        {
            #region �жϿ��
            Neusoft.HISFC.Models.Order.OutPatient.Order orderDrug = new Neusoft.HISFC.Models.Order.OutPatient.Order();
            orderDrug.Item = drugObj;
            orderDrug.ReciptDept = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.Clone();

            if (drugObj.IsStop)
            {
                MessageBox.Show("��ҩƷ�Ѿ�ͣ��");
                return;
            }

            if (Classes.Function.FillPharmacyItem(pManagement, ref orderDrug) == -1)
            {
                return;
            }

            if (Classes.Function.CheckPharmercyItemStockNew(1, orderDrug.Item.ID, orderDrug.Item.Name, orderDrug.ReciptDept.ID, orderDrug.Qty) == false)
            {
                return;
            }
            #endregion
            this.ucOutPatientItemSelect1.CurrentRow = -1;
            this.ucOutPatientItemSelect1.OperatorType = Operator.Add;
            this.ucOutPatientItemSelect1.ucInputItem1.FeeItem = drugObj;
            this.ucOutPatientItemSelect1.ucInputItem1.FeeItem.User02 = Neusoft.HISFC.BizProcess.Integrate.Function.DrugDept.ID;
            this.ucOutPatientItemSelect1.ucInputItem1.FeeItem.User03 = Neusoft.HISFC.BizProcess.Integrate.Function.DrugDept.Name;
            this.ucOutPatientItemSelect1.SetOrder();

            this.ucOutPatientItemSelect1.isDrugListFlag = "1";//Ϊ�˻�ȡ����
        }

        /// <summary>
        /// ���۵Ǽ�
        /// </summary>
        /// <returns></returns>
        public int RegisterEmergencyPatient()
        {
            //���ʱ���Ѿ����뻼����Ϣ
            if (this.myPatientInfo == null || this.myPatientInfo.ID == "")
            {
                MessageBox.Show("û��ѡ���ߣ���˫��ѡ����");
                return -1;
            }
            if (this.myPatientInfo.PVisit.InState.ID.ToString() != "N")
            {
                MessageBox.Show("�û����Ѿ����ۣ�");
                return -1;
            }

            DateTime now = OrderManagement.GetDateTimeFromSysDateTime();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            radtManger.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            if (this.radtManger.RegisterObservePatient(this.myPatientInfo) < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��������״̬ʧ�ܣ�");
                return -1;
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            this.myPatientInfo.PVisit.InState.ID = "R";
            return 0;
        }

        //{1C0814FA-899B-419a-94D1-789CCC2BA8FF}
        /// <summary>
        /// ���صǼ�
        /// </summary>
        /// <returns></returns>
        public int OutEmergencyPatient()
        {

             //���ʱ���Ѿ����뻼����Ϣ
            if (this.myPatientInfo == null || this.myPatientInfo.ID == "")
            {
                MessageBox.Show("û��ѡ���ߣ���˫��ѡ����");
                return -1;
            }
            this.myPatientInfo = regManagement.GetByClinic(myPatientInfo.ID);
            if (myPatientInfo == null)
            {
                MessageBox.Show("��ѯ������Ϣʧ�ܣ�" + regManagement.Err);
                return -1;
            }
            
            if (this.myPatientInfo.PVisit.InState.ID.ToString() == "R")
            {
                MessageBox.Show("�û��߻�δ���ﲻ�ܳ��أ�");
                return -1;
            }

            if (this.myPatientInfo.PVisit.InState.ID.ToString() != "I")
            {
                MessageBox.Show("����δ���۲��������ش���");
                return -1;
            }


            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            if (radtManger.OutObservePatientManager(this.myPatientInfo, EnumShiftType.EO,"����") < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��������״̬ʧ�ܣ�");
                return -1;
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show("���سɹ���");
            return 1;
        }

        //{1C0814FA-899B-419a-94D1-789CCC2BA8FF}
        /// <summary>
        /// ����תסԺ
        /// </summary>
        /// <returns></returns>
        public int InEmergencyPatient()
        {
            //���ʱ���Ѿ����뻼����Ϣ
            if (this.myPatientInfo == null || this.myPatientInfo.ID == "")
            {
                MessageBox.Show("û��ѡ���ߣ���˫��ѡ����");
                return -1;
            }
            this.myPatientInfo = regManagement.GetByClinic(myPatientInfo.ID);
            if (myPatientInfo == null)
            {
                MessageBox.Show("��ѯ������Ϣʧ�ܣ�" + regManagement.Err);
                return -1;
            }
            if (this.myPatientInfo.PVisit.InState.ID.ToString() == "R")
            {
                MessageBox.Show("�û��߻�δ���ﲻ��תסԺ��");
                return -1;
            }

            if (this.myPatientInfo.PVisit.InState.ID.ToString() != "I")
            {
                MessageBox.Show("����δ���۲�����תסԺ����");
                return -1;
            }
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            if (radtManger.OutObservePatientManager(this.myPatientInfo, EnumShiftType.CPI,"����תסԺ") < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��������״̬ʧ�ܣ�");
                return -1;
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show("תסԺ�ɹ���");
            return 1;
        }

        /// <summary>
        /// �˳�ҽ������
        /// </summary>
        /// <returns></returns>
        public int ExitOrder()
        {
            this.SetDrugListVisable(false);
            this.IsDesignMode = false;
            this.bTempVar = false;
            return 0;
        }

        

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            #region ����֮ǰ���ж�
            if (this.bIsDesignMode == false)
            {
                return -1;
            }

            #region �˺ź������ˢ��ҳ�棬����ҽ��վ�Կ��ԶԸû��߿���ҽ�� {721FE3C5-B1BE-43bb-B722-CF8948CB9CF4}
            ArrayList  alRegister=this.regManagement.QueryPatient(this.myPatientInfo.ID);
            if (alRegister == null)
            {
                MessageBox.Show("��ѯ���߹Һ���Ϣ����!");
                return -1;
            }

            if (alRegister.Count == 0)
            {
                MessageBox.Show("�û��߹Һ���Ϣ�����ϣ���ˢ�½���!");
                return -1;
            }
            #endregion
            if (this.CheckOrder() == -1)
            {
                return -1;
            }

            // {C79B428F-5A7B-4aaf-89EB-946679354446} �����Ƿ�Ⱦ��
            //HISFC.BizLogic.HealthRecord.Diagnose diagnoseManager = new Neusoft.HISFC.BizLogic.HealthRecord.Diagnose();
            //ArrayList diagnoseList = diagnoseManager.QueryDiagnoseNoOps(myPatientInfo.ID);
            //foreach (Neusoft.HISFC.Models.HealthRecord.Diagnose diagnose in diagnoseList)
            //{
            //    // �м�¼��Ⱦ�����򵯳�����
            //    if (diagnose.Memo == "1" || diagnose.DiagInfo.Memo == "1")
            //    {
            //        if (string.IsNullOrEmpty(diagnose.Name))
            //        {
            //            MessageBox.Show("��������д���Ϊ " + diagnose.DiagInfo.Name + " �Ĵ�Ⱦ������");
            //        }
            //        else
            //        {
            //            MessageBox.Show("��������д���Ϊ " + diagnose.Name + " �Ĵ�Ⱦ������");
            //        }

            //        UFC.DCP.frmReportManagerClinic frmReportManagerClinic = new Neusoft.UFC.DCP.frmReportManagerClinic();
            //        frmReportManagerClinic.Show();
            //    }
            //}

            //ҽ������ӿ�{48E6BB8C-9EF0-48a4-9586-05279B12624D}
            if (this.IAlterOrderInstance == null)
            {
                this.InitAlterOrderInstance();
            }
            #endregion

            #region �˻��ж�����
            //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
            //bool isAccount = false;
            //if (accountProcess)
            //{
            //    decimal vacancy = 0m;
            //    if (this.Patient.IsAccount)
            //    {

            //        if (feeManagement.GetAccountVacancy(this.Patient.PID.CardNO, ref vacancy) <= 0)
            //        {
            //            MessageBox.Show(feeManagement.Err);
            //            return -1;
            //        }
            //        isAccount = true;
            //    }
            //}
            #endregion


            #region ����
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ���ҽ�������Ժ󡣡���");
            Application.DoEvents();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(OrderManagement.Connection);
            //t.BeginTransaction();
            OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans); //��������
            assignManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            feeManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);//��������
            undrugztManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            regManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            
            string strID = "";
            string strNameNotUpdate = "";//û�и��µ�ҽ������
            string reciptNo = "";//������
            int rep_no = 0; //��������ˮ��
            bool bHavePha = false;//�Ƿ����ҩƷ(����Ԥ��ʹ��)

            Neusoft.HISFC.Models.Order.OutPatient.Order order;
            DateTime now = OrderManagement.GetDateTimeFromSysDateTime();
            #endregion

            #region �жϿ������
            if (this.myPatientInfo.DoctorInfo.SeeNO == -1)
            {
                this.myPatientInfo.DoctorInfo.SeeNO = this.OrderManagement.GetNewSeeNo(this.myPatientInfo.PID.CardNO);//����µĿ������
                if (this.myPatientInfo.DoctorInfo.SeeNO == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return -1;
                }

            }
            #endregion

            #region ����ҽ��
            ArrayList alOrder = new ArrayList(); //����ҽ��
            ArrayList alFeeItem = new ArrayList();//�������
            ArrayList alZLFeeItem = new ArrayList();//����ҽ��
            ArrayList alULFeeItem = new ArrayList();//���鸽��
            ArrayList al = new ArrayList();//��������
            this.alTemp = new ArrayList();//��ʱ����
            ArrayList alOrderChangedInfo = new ArrayList();//ҽ���޸ļ�¼
            bool iReturn = false;
            string errText = "";

            for (int i = 0; i < this.neuSpread1.Sheets[0].Rows.Count; i++)
            {
                order = (Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.Sheets[0].Rows[i].Tag;

                order.SeeNO = this.myPatientInfo.DoctorInfo.SeeNO.ToString();
                if (order.Status == 0)//δ��˵�ҽ��
                {
                    #region ����ҽ��
                    if (order.ID == "") //new �¼ӵ�ҽ��
                    {
                        #region ���Ƥ�Է���
                        if (order.HypoTest == 2)
                        {
                            object obj = Classes.Function.controlerHelper.GetObjectFromID("200025");

                            if (obj != null)
                            {
                                string hypoFeeCode = ((Neusoft.HISFC.Models.Base.Controler)obj).ControlerValue;

                                if (hypoFeeCode != null && hypoFeeCode.Length > 0)
                                {
                                    //���뻮�۱�ʱ���Ӵ�������ˮ�ţ�
                                    Neusoft.HISFC.Models.Fee.Item.Undrug item = null;
                                    Neusoft.HISFC.Models.Fee.Item.UndrugComb undrugzt = null;
                                    try
                                    {
                                        if (hypoFeeCode.Substring(0, 1) == "F")
                                        {
                                            
                                            item = feeManagement.GetItem(hypoFeeCode);//���������Ŀ��Ϣ
                                            if (item.UnitFlag == "1")
                                            {
                                                item.Price = feeManagement.GetUndrugCombPrice(item.ID);
                                            }
                                        }
                                        else
                                        {
                                            item = this.itemManagement.GetItem(hypoFeeCode);
                                            if (item == null || item.ID == null || item.ID.Length <= 0)
                                            {
                                                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                                MessageBox.Show("δ�ҵ�" + order.Name + "����������Ŀ,����Ŀ�����Ѿ�ͣ��,����Ϊ" + hypoFeeCode + this.itemManagement.Err);
                                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                                return -1;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        return -1;
                                    }
                                    if (item != null)
                                    {
                                        item.Qty = 1;
                                    }
                                    Neusoft.HISFC.Models.Order.OutPatient.Order newOrder = order.Clone();
                                    newOrder.ReciptNO = "";
                                    newOrder.SequenceNO = -1;
                                    if (item != null)
                                    {
                                        newOrder.Item = item.Clone();
                                    }
                                    else if (undrugzt != null)
                                    {
                                        newOrder.Item = new Neusoft.HISFC.Models.Base.Item();
                                        newOrder.Item.Qty = 1;
                                        newOrder.Item.ID = undrugzt.ID;
                                        newOrder.Item.Name = undrugzt.Name;
                                        newOrder.ExtendFlag3 = "SUBTBL";//������Ŀ

                                        newOrder.IsNeedConfirm = undrugzt.IsNeedConfirm;
                                        if (undrugzt.IsNeedConfirm)
                                        {
                                            newOrder.ExtendFlag2 = "1";
                                        }
                                        else
                                        {
                                            newOrder.ExtendFlag2 = "0";
                                        }
                                        newOrder.Item.SysClass = undrugzt.SysClass;
                                        newOrder.Unit = "[������]";
                                        newOrder.Item.PriceUnit = "[������]";
                                        newOrder.Item.MinFee.ID = "fh";//�����һ����ֵ
                                        newOrder.Item.Price = this.feeManagement.GetUndrugCombPrice(undrugzt.ID);
                                        
                                    }
                                    newOrder.Qty = 1;
                                    if (item != null)
                                    {
                                        newOrder.Unit = item.PriceUnit;
                                    }
                                    newOrder.Combo = order.Combo;//��Ϻ�
                                    newOrder.ID = Classes.Function.GetNewOrderID(); //ҽ����ˮ��
                                    if (newOrder.ID == "")
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        MessageBox.Show("���ҽ����ˮ�ų���");
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    //newOrder.Item.IsPharmacy = false;
                                    newOrder.Item.ItemType = EnumItemType.UnDrug;
                                    newOrder.InjectCount = order.InjectCount;
                                    newOrder.IsEmergency = order.IsEmergency;
                                    newOrder.IsSubtbl = true;
                                    newOrder.Usage = new Neusoft.FrameWork.Models.NeuObject();
                                    newOrder.SequenceNO = rep_no;
                                    if (newOrder.ExeDept.ID == "")//ִ�п���Ĭ��
                                        newOrder.ExeDept = this.GetReciptDept();
                                    if (this.CheckOrderStockDeptAndExeDept(pManagement, ref newOrder) == -1)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;

                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeitem = Classes.Function.ChangeToFeeItemList(newOrder);
                                    if (feeitem == null)
                                    {
                                        MessageBox.Show("ת���ɷ���ʵ�����");
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    alFeeItem.Add(feeitem);
                                }
                            }
                        }
                        #endregion
                        strID = Classes.Function.GetNewOrderID();
                        if (strID == "")
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            return -1;
                        }
                        
                        order.ID = strID;    //���뵥��
                        order.ReciptNO = reciptNo;
                        order.SequenceNO = 0;
                        order.ReciptSequence = "";
                        //if (order.Item.IsPharmacy)
                        if (order.Item.ItemType == EnumItemType.Drug)
                        {
                            bHavePha = true;
                        }
                        alOrder.Add(order);
                        alTemp.Add(order);
                                               
                        if (this.CheckOrderStockDeptAndExeDept( pManagement, ref order) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            return -1;
                        }

                        #region �˻����ߵĸ�����Ŀ������ϸ�ٻ���{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                        bool isExist = false;
                        if (this.Patient.IsAccount)
                        {
                            if (order.Item is Neusoft.HISFC.Models.Fee.Item.Undrug)
                            {
                                Neusoft.HISFC.Models.Fee.Item.Undrug undrugInfo = this.feeManagement.GetItem(order.Item.ID);
                                if (undrugInfo == null)
                                {
                                    MessageBox.Show("��ѯ������Ŀ��" + order.Item.Name + "����" + this.feeManagement.Err);
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                    return -1;
                                }
                                if (undrugInfo.UnitFlag == "1")
                                {
                                    ArrayList alOrderDetails = Classes.Function.ChangeZtToSingle(order, this.Patient, this.Patient.Pact);
                                    if (alOrderDetails != null)
                                    {
                                        Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList tmpFeeItemList = null;

                                        foreach (Neusoft.HISFC.Models.Order.OutPatient.Order tmpOrder in alOrderDetails)
                                        {
                                            tmpFeeItemList = Classes.Function.ChangeToFeeItemList(tmpOrder);
                                            if (tmpFeeItemList != null)
                                            {
                                                alFeeItem.Add(tmpFeeItemList.Clone());
                                                isExist = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!isExist)
                        {
                            Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList alFeeItemListTmp = Classes.Function.ChangeToFeeItemList(order);
                            if (alFeeItemListTmp == null)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                MessageBox.Show(order.Item.Name + "ҽ��ʵ��ת���ɷ���ʵ�����", "��ʾ");
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                return -1;
                            }
                            alFeeItem.Add(alFeeItemListTmp);
                        }
                        #endregion              
                    }
                    else //update ���µ�ҽ��
                    {
                        #region �����Ҫ���µ�ҽ��
                        Neusoft.HISFC.Models.Order.OutPatient.Order newOrder = OrderManagement.QueryOneOrder(order.ID);
                        //���û��ȡ�����������Ѿ���������ˮ��ȴ�����������������ݿ��������
                        if (newOrder == null || newOrder.Status == 0)
                        {
                            newOrder = order;
                        }
                                                
                        if (newOrder.Status != 0 || newOrder.IsHaveCharged)//��鲢��ҽ��״̬
                        {
                            strNameNotUpdate += "[" + order.Item.Name + "]";

                            continue;
                        }

                        //if (newOrder.Item.IsPharmacy)
                        if (newOrder.Item.ItemType == EnumItemType.Drug)
                        {
                            bHavePha = true;
                        }
                        alOrder.Add(newOrder);
                        alTemp.Add(newOrder);
                        if (this.CheckOrderStockDeptAndExeDept(pManagement, ref newOrder) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            return -1;
                        }

                        Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeitems = Classes.Function.ChangeToFeeItemList(order);
                        if (feeitems == null)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            MessageBox.Show(order.Item.Name + "ҽ��ʵ��ת���ɷ���ʵ�����", "��ʾ");
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            return -1;
                        }
                        alFeeItem.Add(feeitems);
                        
                        #endregion
                    }
                    
                    #endregion
                                        
                }
            }
            #endregion

            #region ��ù�ļ����㷨
            if (this.bDealULSub)
            {
                Classes.Function.ULOrderParms parms = new Order.OutPatient.Classes.Function.ULOrderParms();
                Classes.Function.GetSubByExeType(parms, alOrder, ref alULFeeItem, this.bSingleDealEmrOrder, this.EmrSubUsage, this.ULOrderUsage);

                if (alULFeeItem == null)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("���ü��鸽���㷨����");
                    return -1;
                }

            }
            #endregion

            #region ���Ĵ���

            #region ɾ���޸���ϵ���Ŀ�����ĸ���{F67E089F-1993-4652-8627-300295AAED8C}

            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order temp in alOrder)
            {
                if (temp.NurseStation.User02 == "C")//�޸Ĺ�Ժע
                {
                    

                    for (int i = 0; i < hsComboChange.Count; i++)
                    {
                        if (hsComboChange.ContainsKey(temp.ID))
                        {
                            string comboChange = hsComboChange[temp.ID].ToString();

                            ArrayList alSubAndOrder = feeManagement.QueryFeeDetailbyComoNOAndClinicCode(comboChange, this.myPatientInfo.ID);

                            for (int j = 0; j < alSubAndOrder.Count; j++)
                            {
                                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = alSubAndOrder[j] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;
                                if (item.Item.IsMaterial)
                                {
                                    if (feeManagement.DeleteFeeItemListByRecipeNO(item.RecipeNO, item.SequenceNO.ToString()) < 0)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        MessageBox.Show(feeManagement.Err, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order temp in alOrder)
            {
                if (temp.NurseStation.User02 == "C")//�޸Ĺ�Ժע
                {
                    #region �����ж�
                    if (temp.ExtendFlag1 != null)
                    {
                        string[] strComb = temp.ExtendFlag1.Split('|');

                        if (strComb.Length == 3 && strComb[1] != "")//�����������ע�䣬����
                        {
                            continue;
                        }
                    }
                    int count = this.GetNumHaveSameComb(temp);
                    if (count > 0)//�������,˵������ͻ�û�д����
                    {
                        
                        #region ��ø���
                        ArrayList alSubAndOrder = feeManagement.QueryFeeDetailbyComoNOAndClinicCode(temp.Combo.ID, this.myPatientInfo.ID);
                        if (alSubAndOrder == null)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            errText = string.Format("temp.Combo.ID={0},  this.myPatientInfo.ID={1}", temp.Combo.ID, this.myPatientInfo.ID);
                            MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            return -1;
                        }
                        this.RemoveOrderFromArray(alOrder, ref alSubAndOrder);
                        //Ϊ0˵��û����ӹ�
                        if (alSubAndOrder.Count <= 0)
                        {
                            # region û����ӹ�������Ҫ���
                            if (temp.InjectCount > 0)
                            {
                                #region ��Ӹ���
                                if (!Classes.Function.hsUsageAndSub.Contains(temp.Usage.ID))
                                {
                                    continue;
                                }
                                ArrayList alSubtbls = (ArrayList)Classes.Function.hsUsageAndSub[temp.Usage.ID];
                                if (alSubtbls == null)
                                {
                                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                    MessageBox.Show("���Ժע��������\n" + feeManagement.Err);
                                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                    return -1;
                                }

                                for (int m = 0; m < alSubtbls.Count; m++)
                                {

                                    //rep_no++;//���뻮�۱�ʱ���Ӵ�������ˮ�ţ�
                                    Neusoft.HISFC.Models.Fee.Item.Undrug item = null;
                                    Neusoft.HISFC.Models.Fee.Item.UndrugComb undrugzt = null;
                                    try
                                    {
                                        if (((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID.Substring(0, 1) == "F")
                                        {
                                            item = feeManagement.GetItem(((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID);//���������Ŀ��Ϣ
                                            if (item.UnitFlag == "1")
                                            {
                                                item.Price = feeManagement.GetUndrugCombPrice(item.ID);
                                            }
                                        }
                                        else
                                        {
                                            item = this.itemManagement.GetItem(((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID);
                                            if (item == null || item.ID == null || item.ID.Length <= 0)
                                            {
                                                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                                MessageBox.Show("δ�ҵ�������Ŀ�����Ŀ�Ѿ�ͣ��,����:" + ((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).ID + ((Neusoft.FrameWork.Models.NeuObject)alSubtbls[m]).Name + this.itemManagement.Err);
                                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                                return -1;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        return -1;
                                    }
                                    if (item != null)
                                    {
                                        item.Qty = temp.InjectCount;
                                    }
                                    Neusoft.HISFC.Models.Order.OutPatient.Order newOrder = temp.Clone();
                                    newOrder.ReciptNO = "";
                                    newOrder.SequenceNO = -1;
                                    if (item != null)
                                    {
                                        newOrder.Item = item.Clone();
                                    }
                                    else if (undrugzt != null)
                                    {
                                        newOrder.Item = new Neusoft.HISFC.Models.Base.Item();
                                        newOrder.Item.Qty = temp.InjectCount;
                                        newOrder.Item.ID = undrugzt.ID;
                                        newOrder.Item.Name = undrugzt.Name;
                                        newOrder.ExtendFlag3 = "SUBTBL";//������Ŀ
                                        newOrder.Item.IsNeedConfirm = undrugzt.IsNeedConfirm;
                                        ////if (undrugzt.confirmFlag == Neusoft.HISFC.Models.Fee.ConfirmState.All
                                        ////    || undrugzt.confirmFlag == Neusoft.HISFC.Models.Fee.ConfirmState.Outpatient)
                                        ////{
                                        ////    newOrder.Mark2 = "1";
                                        ////}
                                        ////else
                                        ////{
                                        ////    newOrder.Mark2 = "0";
                                        ////}
                                        newOrder.Item.SysClass = undrugzt.SysClass;
                                        newOrder.Unit = "[������]";
                                        newOrder.Item.PriceUnit = "[������]";
                                        newOrder.Item.MinFee.ID = "fh";//�����һ����ֵ
                                        newOrder.Item.Price = Classes.Function.GetUndrugZtPrice(undrugzt.ID);
                                    }
                                    newOrder.Qty = temp.InjectCount;
                                    if (item != null)
                                    {
                                        newOrder.Unit = item.PriceUnit;
                                    }
                                    newOrder.Combo = temp.Combo;//��Ϻ�
                                    newOrder.ID = Classes.Function.GetNewOrderID();//ҽ����ˮ��
                                    if (newOrder.ID == "")
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        MessageBox.Show("���ҽ����ˮ�ų���");
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    //newOrder.Item.IsPharmacy = false;
                                    newOrder.Item.ItemType = EnumItemType.UnDrug;
                                    newOrder.InjectCount = temp.InjectCount;
                                    newOrder.IsEmergency = temp.IsEmergency;
                                    newOrder.IsSubtbl = true;
                                    newOrder.Usage = new Neusoft.FrameWork.Models.NeuObject();
                                    newOrder.SequenceNO = rep_no;
                                    if (newOrder.ExeDept.ID == "")//ִ�п���Ĭ��
                                        newOrder.ExeDept = (this.OrderManagement.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.Clone();
                                    if (this.CheckOrderStockDeptAndExeDept(pManagement, ref newOrder) == -1)
                                    {
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeitem = Classes.Function.ChangeToFeeItemList(newOrder);
                                    if (feeitem == null)
                                    {
                                        MessageBox.Show("ת���ɷ���ʵ�����");
                                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        return -1;
                                    }
                                    al.Add(feeitem);
                                }

                                #endregion
                            }
                            # endregion
                        }
                        else
                        {
                            # region �Ѿ����� ���»���ɾ��
                            if (temp.InjectCount > 0)
                            {
                                for (int i = 0; i < alSubAndOrder.Count; i++)
                                {
                                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = alSubAndOrder[i] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;

                                    object obj = Classes.Function.controlerHelper.GetObjectFromID("200025");

                                    if (obj != null)
                                    {
                                        string hypoFeeCode = ((Neusoft.HISFC.Models.Base.Controler)obj).ControlerValue;

                                        if (hypoFeeCode != null && hypoFeeCode.Length > 0)
                                        {
                                            if (item.ID != hypoFeeCode)
                                            {
                                                item.Item.Qty = temp.InjectCount;
                                                item.InjectCount = temp.InjectCount;
                                            }
                                        }
                                    }
                                    Classes.Function.CheckFeeItemList(item);
                                }

                                al.AddRange(alSubAndOrder);
                            }
                            else
                            {
                                for (int i = 0; i < alSubAndOrder.Count; i++)
                                {
                                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = alSubAndOrder[i] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;
                                    if (item.Item.IsMaterial)
                                    {
                                        if (feeManagement.DeleteFeeItemListByRecipeNO(item.RecipeNO, item.SequenceNO.ToString()) < 0)
                                        {
                                            MessageBox.Show(feeManagement.Err, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                            return -1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            #endregion

            #region �ϲ��շ�����

            alFeeItem.AddRange(alZLFeeItem);

            alFeeItem.AddRange(alULFeeItem);

            alFeeItem.AddRange(al);
            
            #endregion

            # region �����ź���ˮ�Ź����ɷ���ҵ��㺯��ͳһ����

            try
            {
                //iReturn = feeManagement.SetChargeInfo(this.Patient, alFeeItem, now, ref errText);
                
                //if (iReturn == false)
                //{
                //    MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                //    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                //    return -1;
                //}

                //���Ĵ���ӿ�
                if (IDealSubjob != null)
                {
                    IDealSubjob.DealSubjob(this.Patient, alFeeItem, ref errText);
                }
                

                //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                //if (accountProcess && isAccount)
                //{
                //    iReturn = feeManagement.SetChargeInfoToAccount(this.Patient, alFeeItem, now, ref errText);
                //    if (iReturn == false)
                //    {
                //        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                //        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                //        MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return -1;
                //    }
                //}
                //else
                //{
                    //{AB19F92E-9561-4db9-A0CF-20C1355CD5D8}
                    //ֱ���շ� 1�ɹ� -1ʧ�� 0��ͨ���߲���������������
                    if (IDoctFee != null)
                    {
                        int resultValue = IDoctFee.DoctIdirectFee(this.Patient, alFeeItem, now, ref errText);
                        if (resultValue == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
                        if (resultValue == 0)
                        {
                            iReturn = feeManagement.SetChargeInfo(this.Patient, alFeeItem, now, ref errText);
                            if (iReturn == false)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return -1;
                            }
                        }
                    }
                    else
                    {
                        iReturn = feeManagement.SetChargeInfo(this.Patient, alFeeItem, now, ref errText);
                        if (iReturn == false)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return -1;
                        }
                    }
                //}
                
            }
            catch
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(errText, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            # endregion

            #region ���������źʹ�����ˮ��

            for (int k = 0; k < alOrder.Count; k++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order tempOrder = alOrder[k] as Neusoft.HISFC.Models.Order.OutPatient.Order;
                
                if (tempOrder.ReciptNO == null || tempOrder.ReciptNO == "")
                {
                    foreach (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeitem in alFeeItem)
                    {
                        if (tempOrder.ID == feeitem.Order.ID)
                        {
                            tempOrder.ReciptNO = feeitem.RecipeNO;
                            tempOrder.SequenceNO = feeitem.SequenceNO;
                            tempOrder.ReciptSequence = feeitem.RecipeSequence;
                            
                            break;
                        }
                    }
                }
            }
            #endregion

            # region /*����ҽ�� �������´�����*/

            #region ���ݽӿ�ʵ�ֶ�ҽ����Ϣ���в����ж�
            //{48E6BB8C-9EF0-48a4-9586-05279B12624D}
            if (this.IAlterOrderInstance != null)
            {
                List<Neusoft.HISFC.Models.Order.OutPatient.Order> orderList = new List<Neusoft.HISFC.Models.Order.OutPatient.Order>();
                for (int j = 0; j < alOrder.Count; j++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp
                    = alOrder[j] as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (temp == null)
                    {
                        continue;
                    }
                    orderList.Add(temp);

                }
                if (this.IAlterOrderInstance.AlterOrder(this.myPatientInfo, this.myReciptDoc, this.myReciptDept, ref orderList) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return -1;
                }
            }

            #endregion
            //{AB19F92E-9561-4db9-A0CF-20C1355CD5D8}
            if (IDoctFee != null)
            {
                if (IDoctFee.UpdateOrderFee(this.Patient, alOrder, now, ref errText) < 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("����ҽ���շѱ��ʧ�ܣ�" + errText);
                    return -1;
                }
            }

            for (int j = 0; j < alOrder.Count; j++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order temp
                    = alOrder[j] as Neusoft.HISFC.Models.Order.OutPatient.Order;

                if (temp == null)
                {
                    continue;
                }

                #region ����ҽ����
                if (OrderManagement.UpdateOrder(temp) == -1) //����ҽ����
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show("����ҽ������" + temp.Item.Name + "�����Ѿ��շ�,���˳������������½���!");
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return -1;
                }
                #endregion
                    ////������ҽ����4.0������ordertype���ֵģ�Ŀǰ����û�����ordertype��
                    #region ����ҽ����
                ////if (OrderManagement.UpdateOrder(temp) == -1) //����ҽ����
                ////{
                ////    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                ////    MessageBox.Show("����ҽ������" + temp.Item.Name + "�����Ѿ��շ�,���˳������������½���!");
                ////    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                ////    return -1;
                ////}
                    #endregion
                    #region �����ն˱�
                    ////Neusoft.HISFC.Models.MedTech.Terminal.TerminalApplyInfo apply = new Neusoft.HISFC.Models.MedTech.Terminal.TerminalApplyInfo();
                    ////foreach (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item in alZLFeeItem)
                    ////{
                    ////    if (item.MoOrder == temp.ID)
                    ////    {
                    ////        apply.Item = item;
                    ////        break;
                    ////    }
                    ////}
                    ////apply.Patient = this.myPatientInfo;//������Ϣ
                    ////apply.PatientType = "1";//����
                    ////apply.InsertDate = now;//����ʱ��
                    ////apply.InsertOperator = this.medTechManager.Operator;//������
                    ////apply.OrderExeSequence = Neusoft.FrameWork.Function.NConvert.ToInt32(temp.ID);//ҽ�����뵥��
                    ////apply.User02 = "2";//�ն�ȷ���շ�
                    ////if (medTechManager.CreateApply(apply) < 0)
                    ////{
                    ////    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    ////    MessageBox.Show("�����ն�ȷ�ϱ����" + temp.Item.Name + "�����Ѿ��շ�,���˳������������½���!");
                    ////    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    ////    return -1;
                    ////}
                    #endregion
                
            }
            # endregion

            #region ����ҽ�������¼

            if (this.bSaveOrderHistory)
            {
                for (int j = 0; j < alOrder.Count; j++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp
                        = alOrder[j] as Neusoft.HISFC.Models.Order.OutPatient.Order;

                    if (this.alAllOrder == null || this.alAllOrder.Count <= 0 || temp == null)
                    {
                        continue;
                    }

                    Neusoft.HISFC.Models.Order.OutPatient.Order tem
                        = this.orderHelper.GetObjectFromID(temp.ID) as Neusoft.HISFC.Models.Order.OutPatient.Order;

                    if (tem == null)
                    {
                        continue;
                    }

                    #region �ж��Ƿ���Ҫ����
                    //�޸�����
                    if (tem.Qty != temp.Qty)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //�޸ĵ�λ
                    else if (tem.Unit != temp.Unit)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //�޸�ÿ����
                    else if (tem.DoseOnce != temp.DoseOnce)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //ÿ�ε�λ
                    else if (tem.DoseUnit != temp.DoseUnit)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //��ҩ����
                    else if (tem.HerbalQty != temp.HerbalQty)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //�÷�
                    else if (tem.Usage.ID != temp.Usage.ID)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //Ƶ��
                    else if (tem.Frequency.ID != temp.Frequency.ID)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //ִ�п���
                    else if (tem.ExeDept.ID != temp.ExeDept.ID)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //��ע
                    else if (tem.Memo != temp.Memo)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //��ƿ
                    else if (tem.ExtendFlag1 != temp.ExtendFlag1)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //���
                    else if (tem.Combo.ID != temp.Combo.ID)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //Ժע
                    else if (tem.InjectCount != temp.InjectCount)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //�Ӽ�
                    else if (tem.IsEmergency != temp.IsEmergency)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //Ƥ��
                    else if (tem.HypoTest != temp.HypoTest)
                    {
                        alOrderChangedInfo.Add(temp);
                        continue;
                    }
                    //���鸽��
                    else if (tem.NurseStation.User01 != temp.NurseStation.User01)
                    {
                        alOrderChangedInfo.Add(tem);
                        continue;
                    }
                    #endregion

                }

                //��������¼��
                for (int i = 0; i < alOrderChangedInfo.Count; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order temp
                        = alOrderChangedInfo[i] as Neusoft.HISFC.Models.Order.OutPatient.Order;

                    if (this.OrderManagement.InsertOrderChangeInfo(temp) < 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        MessageBox.Show("����ҽ�������¼����");
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        return -1;
                    }
                }
            }
            #endregion

            #region ���¿�����Ϣ
            if (this.currentRoom != null)
            {
                if (this.assignManagement.UpdateAssign(this.currentRoom.ID, this.myPatientInfo.ID, now, this.OrderManagement.Operator.ID) < 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show("���·����־����");
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return -1;
                }
            }

            if (this.regManagement.UpdateSeeDone(this.myPatientInfo.ID) < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                MessageBox.Show("���¿����־����");
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return -1;
            }

            if (this.regManagement.UpdateDept(this.myPatientInfo.ID, ((Neusoft.HISFC.Models.Base.Employee)this.OrderManagement.Operator).Dept.ID, this.OrderManagement.Operator.ID) < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                MessageBox.Show("���¿�����ҡ�ҽ������");
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return -1;
            }
            
            #endregion

            #region �ύ
            Neusoft.FrameWork.Management.PublicTrans.Commit();

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            if (strNameNotUpdate == "")//�Ѿ��仯��ҽ����Ϣ
            {
                MessageBox.Show("ҽ������ɹ���");
            }
            else
            {
                MessageBox.Show("ҽ������ɹ���\n" + strNameNotUpdate + "ҽ��״̬�Ѿ��������ط����ģ��޷����и��£���ˢ����Ļ��");
            }
            #endregion

            #region ����ҽ�����
            if (this.SaveSortID(0) < 0)
            {
                MessageBox.Show("����ҽ����ų���");
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return -1;
            }
            #endregion

            #region ����Ԥ����Ŀǰû��ʵ�֣�
            //ArrayList alRecipe = new ArrayList();
            //alRecipe = OrderManagement.GetPhaRecipeNoByClinicNoAndSeeNo(this.myPatientInfo.ID, this.myPatientInfo.DoctorInfo.SeeNO.ToString());
            //foreach(Neusoft.FrameWork.Models.NeuObject obj in alRecipe)
            //{
            //    this.PrintRecipe(obj.ID);
            //}
            this.PrintRecipe(this.myPatientInfo.DoctorInfo.SeeNO.ToString());
            #endregion

            #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
            //string isUseDL = feeManagement.GetControlValue("200212", "0");//addby xuewj 2010-11-11 �������뵥��ȡ���������ļ� {457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
            if (isUseDL)
            {
                if (PACSApplyInterface == null)
                {
                    if (InitPACSApplyInterface() < 0)
                    {
                        MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                        return -1;
                    }
                }
                PACSApplyInterface.SaveApplysG(this.Patient.DoctorInfo.SeeNO.ToString(), 0);
            }
            #endregion

            #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ�Զ����

            string err1 = "";
            #region {8C389FCD-3E64-4a90-9830-BE220B952B53} 2010-12-10
           // ArrayList alPass = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("AutoPass", out err1);
            ArrayList alPass = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("Pass","AutoPass", out err1);
            #endregion
            #region {A3814010-0251-4197-8556-E38F47F4AC77}
            //if (alPass == null || alPass.Count == 0)
            //{
            //    //MessageBox.Show(err1);
            //    return -1;
            //}
            //else if (alPass[0] as string == "1")
            //{
            //    this.IReasonableMedicine.ShowFloatWin(false);
            //    this.PassTransOrder(1, false);
            //}
            //else
            //{
            //    return -1;
            //}

            if (alPass != null && alPass.Count > 0)
            {
                if (alPass[0] as string == "1")
                {
                    this.IReasonableMedicine.ShowFloatWin(false);
                    #region {8C389FCD-3E64-4a90-9830-BE220B952B53} 2010-12-20 �޸�
                    //���߻���סԺ��Ϣ�ϴ�
                    this.myPatientInfo.PID.PatientNO = this.myPatientInfo.PID.CardNO;
                    this.myPatientInfo.PVisit.PatientLocation.Dept.ID = this.empl.Dept.ID;
                    this.myPatientInfo.PVisit.PatientLocation.Dept.Name = this.empl.Dept.Name;
                    this.myPatientInfo.PVisit.AdmittingDoctor.ID    = this.empl.ID;
                    this.myPatientInfo.PVisit.AdmittingDoctor.Name  = this.empl.Name;
                    this.IReasonableMedicine.PassSetPatientInfo(this.myPatientInfo, this.empl.ID, this.empl.Name);
                    //������ҩ���
                    //this.PassTransOrder(1, false);
                    this.PassTransOrder(1, true);
                    #endregion
                }
            }
            #endregion

            #endregion

            #region ���ش���
            this.IsDesignMode = false;
            this.bTempVar = false;

            //{F67E089F-1993-4652-8627-300295AAED8C}
            //��������
            this.hsComboChange = new Hashtable();
            #endregion

            this.SetDrugListVisable(false);//{24BDD373-4F2C-4899-88A7-FE2E8386F7CF}
            
            return 0;
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

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <returns></returns>
        public int Retrieve()
        {
            // TODO:  ��� ucOrder.Retrieve ʵ��
            this.QueryOrder();
            return 0;
        }

        /// <summary>
        /// ��ҩ
        /// </summary>
        /// <returns></returns>
        public int HerbalOrder()
        { 
            Neusoft.HISFC.Models.Order.OutPatient.Order ord;
            if (this.neuSpread1.ActiveSheet.ActiveRowIndex >= 0 && this.neuSpread1.ActiveSheet.Rows.Count > 0)
            {
                ord = this.neuSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                #region {071AEF5B-B38D-4061-A460-B0137A01E812}
                //if (ord != null && ord.Status != null && ord.Status == 0)
                if (ord != null && ord.Item.SysClass.ID.ToString() == "PCC" && ord.Status == 0)
                #endregion
                {//{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                    this.ModifyHerbal();
                }
                #region {071AEF5B-B38D-4061-A460-B0137A01E812}
                else
                {
                    using (Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder uc = new Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder(true, Neusoft.HISFC.Models.Order.EnumType.SHORT, this.GetReciptDept().ID))
                    {
                        uc.refreshGroup += new Neusoft.HISFC.Components.Order.Controls.RefreshGroupTree(uc_refreshGroup);
                        uc.Patient = new Neusoft.HISFC.Models.RADT.PatientInfo();//
                       
                        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                        Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                        if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                        {
                            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order info in uc.AlOrder)
                            {
                                //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                                info.DoseOnce = info.Qty;
                                info.Qty = info.Qty * info.HerbalQty;

                                this.AddNewOrder(info, 0);
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
                using (Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder uc = new Neusoft.HISFC.Components.Order.Controls.ucHerbalOrder(true, Neusoft.HISFC.Models.Order.EnumType.SHORT, this.GetReciptDept().ID))
                {
                    uc.refreshGroup += new Neusoft.HISFC.Components.Order.Controls.RefreshGroupTree(uc_refreshGroup);
                    uc.Patient = new Neusoft.HISFC.Models.RADT.PatientInfo();//

                    Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ҩҽ������";
                    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
                    if (uc.AlOrder != null && uc.AlOrder.Count != 0)
                    {
                        foreach (Neusoft.HISFC.Models.Order.OutPatient.Order info in uc.AlOrder)
                        {
                            //{AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                            info.DoseOnce = info.Qty;
                            info.Qty = info.Qty * info.HerbalQty;

                            this.AddNewOrder(info, 0);
                        }
                        uc.Clear();
                        this.RefreshCombo();
                    }
                }
            }
            return 1;
        }

        void uc_refreshGroup()
        {
            OnRefreshGroupTree(null,null);
        }

        #endregion

        #region �˵�

        int ActiveRowIndex = -1;

        /// <summary>
        /// ����Ҽ��˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_MouseUp(object sender, MouseEventArgs e)
        {
            this.contextMenu1.Items.Clear();
            Neusoft.HISFC.Models.Order.OutPatient.Order mnuSelectedOrder = null;
            if (this.bIsShowPopMenu && e.Button == MouseButtons.Right)
            {
                this.contextMenu1.Items.Clear();
                FarPoint.Win.Spread.Model.CellRange c = neuSpread1.GetCellFromPixel(0, 0, e.X, e.Y);
                if (c.Row >= 0)
                {
                    this.neuSpread1.ActiveSheet.ActiveRowIndex = c.Row;
                    this.neuSpread1.ActiveSheet.AddSelection(c.Row, 0, 1, 1);
                    ActiveRowIndex = c.Row;
                }
                else
                {
                    ActiveRowIndex = -1;
                }
                if (ActiveRowIndex < 0)
                {
                    #region {DF8058FF-72C0-404f-8F36-6B4057B6F6CD}
                    if (this.bIsDesignMode)
                    {
                        #region ճ��ҽ��
                        ToolStripMenuItem mnuPasteOrder = new ToolStripMenuItem("ճ��ҽ��");
                        mnuPasteOrder.Click += new EventHandler(mnuPasteOrder_Click);
                        this.contextMenu1.Items.Add(mnuPasteOrder);
                        this.contextMenu1.Show(this.neuSpread1, new Point(e.X, e.Y));
                        #endregion
                    }
                    #endregion
                    return;
                }

                mnuSelectedOrder = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, 0);//(Neusoft.HISFC.Models.Order.Order)this.fpSpread1.ActiveSheet.Rows[ActiveRowIndex].Tag;

                if (mnuSelectedOrder != null && mnuSelectedOrder.Item.SysClass.ID.ToString() == "UL" && mnuSelectedOrder.Status == 0)
                {
                    ////ToolStripMenuItem mnuLisCard = new ToolStripMenuItem();
                    ////mnuLisCard.Text = "��ӡ�������뵥[��ݼ�:F12]";
                    ////mnuLisCard.Click += new EventHandler(mnuLisCard_Click);
                    ////this.contextMenu1.Items.Add(mnuLisCard);
                }
                if (mnuSelectedOrder != null && mnuSelectedOrder.Item.SysClass.ID.ToString() == "UZ" && mnuSelectedOrder.Status == 0)
                {
                    ////ToolStripMenuItem mnuDealCard = new ToolStripMenuItem();
                    ////mnuDealCard.Text = "��ӡ�������뵥[��ݼ�:F12]";
                    ////mnuDealCard.Click += new EventHandler(mnuDealCard_Click);
                    ////this.contextMenu1.Items.Add(mnuDealCard);
                }
                //if (mnuSelectedOrder != null && mnuSelectedOrder.Item.IsPharmacy)
                if (mnuSelectedOrder != null && mnuSelectedOrder.Item.ItemType == EnumItemType.Drug)
                {
                    ////ToolStripMenuItem mnuIMCard = new ToolStripMenuItem();
                    ////mnuIMCard.Text = "��ӡ��Һ���Ƶ�[��ݼ�:F12]";
                    ////mnuIMCard.Click += new EventHandler(mnuIMCard_Click);
                    ////this.contextMenu1.Items.Add(mnuIMCard);
                }
                if (this.bIsDesignMode)
                {
                    #region Ժע����
                    //if (mnuSelectedOrder.Item.IsPharmacy && 
                    //    (mnuSelectedOrder.Status == 0 || mnuSelectedOrder.Status == 4) && 
                    //    mnuSelectedOrder.InjectCount == 0 &&
                    //    Classes.Function.hsUsageAndSub.Contains(mnuSelectedOrder.Usage.ID))
                    if (mnuSelectedOrder.Item.ItemType == EnumItemType.Drug &&
                      (mnuSelectedOrder.Status == 0 || mnuSelectedOrder.Status == 4) &&
                      mnuSelectedOrder.InjectCount == 0 &&
                      Classes.Function.hsUsageAndSub.Contains(mnuSelectedOrder.Usage.ID))
                    {
                        ToolStripMenuItem mnuInjectNum = new ToolStripMenuItem();//Ժע����
                        mnuInjectNum.Click += new EventHandler(mnumnuInjectNum_Click);

                        mnuInjectNum.Text = "���Ժע����[��ݼ�:F5]";
                        this.contextMenu1.Items.Add(mnuInjectNum);
                    }

                    //if (mnuSelectedOrder.Item.IsPharmacy && 
                    //    (mnuSelectedOrder.Status == 0 || mnuSelectedOrder.Status == 4) && 
                    //    mnuSelectedOrder.InjectCount > 0)
                    if (mnuSelectedOrder.Item.ItemType == EnumItemType.Drug &&
                        (mnuSelectedOrder.Status == 0 || mnuSelectedOrder.Status == 4) &&
                        mnuSelectedOrder.InjectCount > 0)
                    {
                        ToolStripMenuItem mnuInjectNum = new ToolStripMenuItem();//Ժע����
                        mnuInjectNum.Click += new EventHandler(mnumnuInjectNum_Click);

                        mnuInjectNum.Text = "�޸�Ժע����[��ݼ�:F5]";
                        this.contextMenu1.Items.Add(mnuInjectNum);
                    }
                    #endregion

                    #region ֹͣ�˵�
                    if (mnuSelectedOrder.Status == 0)
                    { //����
                        ToolStripMenuItem mnuDel = new ToolStripMenuItem();//ֹͣ
                        mnuDel.Click += new EventHandler(mnuDel_Click);
                        mnuDel.Text = "ɾ��ҽ��[" + mnuSelectedOrder.Item.Name + "]";
                        this.contextMenu1.Items.Add(mnuDel);//ɾ��������
                    }
                    #region ����ҽ��{DFA920BD-AEB2-4371-B501-21CB87558147}
                    else if (mnuSelectedOrder.Status == 1)
                    {
                        ToolStripMenuItem mnuCancel = new ToolStripMenuItem();//ֹͣ
                        mnuCancel.Click += new EventHandler(mnuCancel_Click);
                        mnuCancel.Text = "����ҽ��[" + mnuSelectedOrder.Item.Name + "]";
                        this.contextMenu1.Items.Add(mnuCancel);//ɾ��������																							
                    }
                    #endregion
                    #endregion

                    #region ����ҽ��

                    ToolStripMenuItem mnuCopyAs = new ToolStripMenuItem();//����ҽ��Ϊ������
                    mnuCopyAs.Click += new EventHandler(mnuCopyAs_Click);

                    mnuCopyAs.Text = "����" + "[" + mnuSelectedOrder.Item.Name + "]";

                    this.contextMenu1.Items.Add(mnuCopyAs);
                    #endregion

                    #region ����
                    ToolStripMenuItem mnuUp = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuUp.Click += new EventHandler(mnuUp_Click);
                    if (this.neuSpread1.ActiveSheet.ActiveRowIndex <= 0) mnuUp.Enabled = false;
                    this.contextMenu1.Items.Add(mnuUp);
                    #endregion

                    #region ����
                    ToolStripMenuItem mnuDown = new ToolStripMenuItem("���ƶ�");//���ƶ�
                    mnuDown.Click += new EventHandler(mnuDown_Click);
                    if (this.neuSpread1.ActiveSheet.ActiveRowIndex >= this.neuSpread1.ActiveSheet.RowCount - 1 || this.neuSpread1.ActiveSheet.ActiveRowIndex < 0) mnuDown.Enabled = false;
                    this.contextMenu1.Items.Add(mnuDown);
                    #endregion

                    #region �޸ļ۸�
                    if (mnuSelectedOrder.Status == 0)
                    {
                        ToolStripMenuItem mnuChangePrice = new ToolStripMenuItem("�޸ļ۸�");
                        mnuChangePrice.Click += new EventHandler(mnuChangePrice_Click);
                        this.contextMenu1.Items.Add(mnuChangePrice);
                    }
                    #endregion

                    #region ҽ����ƿ
                    ////if (mnuSelectedOrder.Status == 0 && mnuSelectedOrder.Item.IsPharmacy)
                    ////{
                    ////    ToolStripMenuItem mnuResumeOrder = new ToolStripMenuItem("ҽ����ƿ[��ݼ�:F6]");
                    ////    mnuResumeOrder.Click += new EventHandler(mnuResumeOrder_Click);
                    ////    this.contextMenu1.Items.Add(mnuResumeOrder);
                    ////}
                    #endregion

                    #region �����ӱ�
                    ////if (mnuSelectedOrder.Status == 0 && this.JudgeIsPCZ())
                    ////{
                    ////    ToolStripMenuItem mnuChangeQTY = new ToolStripMenuItem("�����ӱ�[��ݼ�:F7]");
                    ////    ////mnuChangeQTY.Click += new EventHandler(mnuChangeQTY_Click);
                    ////    this.contextMenu1.Items.Add(mnuChangeQTY);
                    ////}
                    #endregion

                    #region ������{C6E229AC-A1C4-4725-BBBB-4837E869754E}

                    ToolStripMenuItem mnuSaveGroup = new ToolStripMenuItem("������");//������
                    mnuSaveGroup.Click += new EventHandler(mnuSaveGroup_Click);

                    this.contextMenu1.Items.Add(mnuSaveGroup);
                    #endregion

                    #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ��Ӻ�����ҩ�Ҽ��˵�
                   
                    if (this.IReasonableMedicine != null && this.EnabledPass && this.IReasonableMedicine.PassEnabled)
                    {
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

                    //#region �ش�������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504 
                    //string isUseDL = feeManagement.GetControlValue("200212", "0");
                    //if (isUseDL == "1")
                    //{
                        //if (mnuSelectedOrder.ApplyNo != null && mnuSelectedOrder.ApplyNo != "")
                        //{
                        //    ToolStripMenuItem mnuPACSApply = new ToolStripMenuItem("�ش�������뵥");//���ƶ�
                        //    mnuPACSApply.Click += new EventHandler(mnuPACSApply_Click);
                        //    this.contextMenu1.Items.Add(mnuPACSApply);
                        //}
                    //}
                    //#endregion

                }
                else
                {
                    #region {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
                    //if (this.bOrderHistory)
                    //{
                    //    ToolStripMenuItem mnuCopyOrder = new ToolStripMenuItem("���Ƶ���������");//��ע
                    //    ////mnuCopyOrder.Click += new EventHandler(mnuCopyOrder_Click);
                    //    this.contextMenu1.Items.Add(mnuCopyOrder);
                    //}

                    #region ����ҽ��
                    ToolStripMenuItem mnuCopyOrder = new ToolStripMenuItem("����ҽ��");
                    mnuCopyOrder.Click += new EventHandler(mnuCopyOrder_Click);
                    this.contextMenu1.Items.Add(mnuCopyOrder);
                    #endregion

                    #endregion
                }
                #region ��Ӻ�����ҩ�Ҽ��˵�
                //if (this.EnabledPass && Pass.Pass.PassEnabled && mnuSelectedOrder.Item.IsPharmacy)
                //{
                //    MenuItem menuPass = new MenuItem("������ҩ");
                //    this.contextMenu1.MenuItems.Add(menuPass);

                //    MenuItem menuAllergn = new MenuItem("����ʷ/����״̬");
                //    menuAllergn.Click += new EventHandler(mnuPass_Click);
                //    menuPass.Items.Add(menuAllergn);

                //    if (Pass.Pass.PassGetState("101") != 0)
                //    {
                //        MenuItem menuCPR = new MenuItem("ҩ���ٴ���Ϣ�ο�");
                //        menuCPR.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCPR);
                //    }
                //    if (Pass.Pass.PassGetState("102") != 0)
                //    {
                //        MenuItem menuDIR = new MenuItem("ҩƷ˵����");
                //        menuDIR.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuDIR);
                //    }
                //    if (Pass.Pass.PassGetState("107") != 0)
                //    {
                //        MenuItem menuCHP = new MenuItem("�й�ҩ��");
                //        menuCHP.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCHP);
                //    }
                //    if (Pass.Pass.PassGetState("103") != 0)
                //    {
                //        MenuItem menuCPE = new MenuItem("������ҩ����");
                //        menuCPE.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCPE);
                //    }
                //    if (Pass.Pass.PassGetState("104") != 0)
                //    {
                //        MenuItem menuCHE = new MenuItem("ҩ�����ֵ");
                //        menuCHE.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCHE);
                //    }
                //    if (Pass.Pass.PassGetState("220") != 0)
                //    {
                //        MenuItem menuLIM = new MenuItem("�ٴ�������Ϣ�ο�");
                //        menuLIM.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuLIM);
                //    }
                //    #region ҩƷר����Ϣ
                //    MenuItem menuSpecialInfo = new MenuItem("ר����Ϣ");
                //    menuPass.Items.Add(menuSpecialInfo);

                //    if (Pass.Pass.PassGetState("201") != 0)
                //    {
                //        MenuItem menuDDim = new MenuItem("ҩ��-ҩ���໥����");
                //        menuSpecialInfo.MenuItems.Add(menuDDim);
                //        menuDDim.Click += new EventHandler(mnuPass_Click);
                //    }

                //    if (Pass.Pass.PassGetState("202") != 0)
                //    {
                //        MenuItem menuDFim = new MenuItem("ҩ��-ʳ���໥����");
                //        menuSpecialInfo.Items.Add(menuDFim);
                //        menuDFim.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("203") != 0)
                //    {
                //        MenuItem menuMACH = new MenuItem("����ע�����������");
                //        menuSpecialInfo.Items.Add(menuMACH);
                //        menuMACH.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("204") != 0)
                //    {
                //        MenuItem menuTRI = new MenuItem("����ע�����������");
                //        menuSpecialInfo.Items.Add(menuTRI);
                //        menuTRI.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("205") != 0)
                //    {
                //        MenuItem menuDDCM = new MenuItem("����֢");
                //        menuSpecialInfo.MenuItems.Add(menuDDCM);
                //        menuDDCM.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("206") != 0)
                //    {
                //        MenuItem menuSID = new MenuItem("������");
                //        menuSpecialInfo.Items.Add(menuSID);
                //        menuSID.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("207") != 0)
                //    {
                //        MenuItem menuOLD = new MenuItem("��������ҩ");
                //        menuSpecialInfo.Items.Add(menuOLD);
                //        menuOLD.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("208") != 0)
                //    {
                //        MenuItem menuPED = new MenuItem("��ͯ��ҩ");
                //        menuSpecialInfo.Items.Add(menuPED);
                //        menuPED.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("209") != 0)
                //    {
                //        MenuItem menuPREG = new MenuItem("��������ҩ");
                //        menuSpecialInfo.Items.Add(menuPREG);
                //        menuPREG.Click += new EventHandler(mnuPass_Click);
                //    }
                //    if (Pass.Pass.PassGetState("210") != 0)
                //    {
                //        MenuItem menuACT = new MenuItem("��������ҩ");
                //        menuSpecialInfo.Items.Add(menuACT);
                //        menuACT.Click += new EventHandler(mnuPass_Click);
                //    }
                //    #endregion
                //    if (Pass.Pass.PassGetState("106") != 0)
                //    {
                //        MenuItem menuCENter = new MenuItem("ҽҩ��Ϣ����");
                //        menuCENter.Click += new EventHandler(mnuPass_Click);
                //        menuPass.MenuItems.Add(menuCENter);
                //    }
                //    if (Pass.Pass.PassGetState("13") != 0)
                //    {
                //        MenuItem menuDrug = new MenuItem("ҩƷ�����Ϣ");
                //        menuDrug.Click += new EventHandler(mnuPass_Click);
                //        menuPass.MenuItems.Add(menuDrug);
                //    }
                //    if (Pass.Pass.PassGetState("14") != 0)
                //    {
                //        MenuItem menuUsage = new MenuItem("��ҩ;�������Ϣ");
                //        menuUsage.Click += new EventHandler(mnuPass_Click);
                //        menuPass.MenuItems.Add(menuUsage);
                //    }
                //    if (Pass.Pass.PassGetState("11") != 0)
                //    {
                //        MenuItem menuSystem = new MenuItem("ϵͳ����");
                //        menuSystem.Click += new EventHandler(mnuPass_Click);
                //        menuPass.MenuItems.Add(menuSystem);
                //    }
                //    if (Pass.Pass.PassGetState("12") != 0)
                //    {
                //        MenuItem menuResearch = new MenuItem("��ҩ�о�");
                //        menuResearch.Click += new EventHandler(mnuPass_Click);
                //        menuPass.MenuItems.Add(menuResearch);
                //    }
                //    if (Pass.Pass.PassGetState("3") != 0)
                //    {
                //        MenuItem menuWarn = new MenuItem("����");
                //        menuWarn.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuWarn);

                //        if (this.fpSpread1.Sheets[0].Cells[c.Row, iColumns[0]].Tag != null && this.fpSpread1.Sheets[0].Cells[c.Row, iColumns[0]].Tag.ToString() != "0")
                //        {
                //            menuWarn.Enabled = true;
                //        }
                //        else
                //        {
                //            menuWarn.Enabled = false;
                //        }
                //    }
                //    if (Pass.Pass.PassGetState("3") != 0)
                //    {
                //        MenuItem menuCheck = new MenuItem("���");
                //        menuCheck.Click += new EventHandler(mnuPass_Click);
                //        menuPass.Items.Add(menuCheck);
                //    }

                //}

                #endregion
                this.contextMenu1.Show(this.neuSpread1, new Point(e.X, e.Y));
            }

        }

        /// <summary>
        /// ɾ�������ϡ�ֹͣҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDel_Click(object sender, EventArgs e)
        {
            this.Del();
        }

        #region ����ҽ�����շѺ�ҽ�����������ϣ��������������ٴ򿪣�{DFA920BD-AEB2-4371-B501-21CB87558147}
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCancel_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.GetObjectFromFarPoint(this.neuSpread1_Sheet1.ActiveRowIndex, 0);

            if (order == null)
            {
                return;
            }

            if (order.Status != 1)
            {
                return;
            }

            DialogResult r = MessageBox.Show("�Ƿ�ȷ��Ҫ���ϸ���ҽ��,�˲������ܳ�����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (r == DialogResult.Cancel)
            {
                return;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            this.OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            for (int i = 0; i < this.neuSpread1.ActiveSheet.Rows.Count; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order temp = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex);
                if (temp == null)
                    continue;

                if (temp.Combo.ID == order.Combo.ID)
                {
                    if (this.OrderManagement.UpdateOrderBeCaceled(temp) < 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ҽ��" + temp.Item.Name + "ʧ��");
                        return ;
                    }
                    int oldState = temp.Status;
                    temp.Status = 3;
                    temp.DCOper.ID = this.OrderManagement.Operator.ID;
                    temp.DCOper.OperTime = this.OrderManagement.GetDateTimeFromSysDateTime();
                    this.AddObjectToFarpoint(temp, i, 0, EnumOrderFieldList.Item);
                    if (this.OrderManagement.InsertOrderChangeInfo(temp) < 0)
                    {
                        temp.Status = oldState;
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ҽ��" + order.Item.Name + "�޸���Ϣʧ��");
                        return ;
                        
                    }
                    //{AB19F92E-9561-4db9-A0CF-20C1355CD5D8}
                    if (IDoctFee != null)
                    {
                        string errText = string.Empty;
                        if (IDoctFee.CancelOrder(this.Patient, temp, ref errText) < 0)
                        {
                            temp.Status = oldState;
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(errText);
                            return;
                        }
                    }

                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    
                    #region �������뵥 {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
                    //string isUseDL = feeManagement.GetControlValue("200212", "0");//{457F6C34-7825-4ece-ACFB-B3A9CA923D6D}
                    if (isUseDL)
                    {
                        if (order.ApplyNo != null)
                        {
                            if (PACSApplyInterface == null)
                            {
                                if (InitPACSApplyInterface() < 0)
                                {
                                    MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                                    return;
                                }
                            }
                            PACSApplyInterface.DeleteApply(order.ApplyNo);
                            //if (PACSApplyInterface.DeleteApply(order.ApplyNo) < 0)
                            //{
                            //    MessageBox.Show("���ϵ������뵥ʱ����");
                            //    return -1;
                            //}
                        }
                    }
                    #endregion
                }
            }

            this.RefreshOrderState();
        }
        #endregion

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopyAs_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.neuSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
            if (order == null) return;
            ArrayList al = new ArrayList();
            string ComboNo = this.OrderManagement.GetNewOrderComboID();
            for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
            {
                //{0817AFF8-A0DC-4a06-BEAD-015BC49AE973}
                if (this.neuSpread1.ActiveSheet.IsSelected(i, 0))
                //if (this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex).Combo.ID == order.Combo.ID)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order o = this.GetObjectFromFarPoint(i, this.neuSpread1.ActiveSheetIndex).Clone();
                    //if (o.Item.IsPharmacy)
                    if (o.Item.ItemType == EnumItemType.Drug)
                    {
                        if (Classes.Function.FillPharmacyItem(pManagement, ref o) == -1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (Classes.Function.FillFeeItem(itemManagement, ref o) == -1)
                        {
                            return;
                        }
                    }
                    DateTime dtNow = DateTime.MinValue;

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
                    dtNow = this.OrderManagement.GetDateTimeFromSysDateTime();
                    o.MOTime = dtNow;
                    if (this.GetReciptDept() != null)
                        o.ReciptDept = this.GetReciptDept().Clone();
                    if (this.GetReciptDoc() != null)
                        o.ReciptDoctor = this.GetReciptDoc().Clone();
                    if (this.GetReciptDoc() != null)
                    {
                        o.Oper.ID = this.GetReciptDoc().ID;
                        o.Oper.ID = this.GetReciptDoc().Name;
                    }

                    o.CurMOTime = o.BeginTime;
                    o.NextMOTime = o.BeginTime;

                    al.Add(o);
                }
            }
            for (int i = 0; i < al.Count; i++)
            {
                this.AddNewOrder(al[i], 0);
            }
            ////SetFeeDisplay(this.Patient, null);
            this.RefreshCombo();
            
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuUp_Click(object sender, EventArgs e)
        {
            if (this.neuSpread1.ActiveSheet.ActiveRowIndex <= 0) return;
            int CurrentActiveRow = this.neuSpread1.ActiveSheet.ActiveRowIndex;
            //��������sortid
            for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order ord = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                ord.SortID = this.neuSpread1.ActiveSheet.Rows.Count - i;
                this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Text = Convert.ToString(this.neuSpread1.ActiveSheet.Rows.Count - i);
                this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = this.neuSpread1.ActiveSheet.Rows.Count - i;
            }
            int Sort = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex - 1, this.neuSpread1.ActiveSheetIndex).SortID;
            int oldSort = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, this.neuSpread1.ActiveSheetIndex).SortID;
            string combNo = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex - 1, this.neuSpread1.ActiveSheetIndex).Combo.ID;
            string oldcombNo = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, this.neuSpread1.ActiveSheetIndex).Combo.ID;
            //int tmp = -1;
            if (combNo == oldcombNo)//������ƶ�
            {
                ((Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.ActiveSheet.Rows[this.neuSpread1.ActiveSheet.ActiveRowIndex - 1].Tag).SortID = oldSort;
                this.neuSpread1.ActiveSheet.Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex - 1, iColumns[28]].Value = oldSort;
                ((Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.ActiveSheet.Rows[this.neuSpread1.ActiveSheet.ActiveRowIndex].Tag).SortID = Sort;
                this.neuSpread1.ActiveSheet.Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, iColumns[28]].Value = Sort;
            }
            else
            {
                int combNum = 0;
                int oldcombNum = 0;
                for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order oTmp = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (oTmp.Combo.ID == combNo)
                    {
                        combNum++;
                    }
                    if (oTmp.Combo.ID == oldcombNo)
                    {
                        oldcombNum++;
                    }
                }
                for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order oTmp = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (oTmp.Combo.ID == combNo)
                    {
                        oTmp.SortID = oTmp.SortID - (oldcombNum);
                        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oTmp.SortID;
                    }
                    else if (oTmp.Combo.ID == oldcombNo)
                    {
                        oTmp.SortID = oTmp.SortID + (combNum);
                        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oTmp.SortID;
                    }
                    else
                    {

                    }
                }
                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order oPre = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (oPre.SortID == Sort)
                //    {
                //        oPre.SortID = tmp;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oPre.SortID;
                //    }
                //}
                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order o = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (o.SortID == oldSort)
                //    {
                //        o.SortID = Sort;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = o.SortID;
                //    }
                //}
                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order oPre = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (oPre.SortID == tmp)
                //    {
                //        oPre.SortID = oldSort;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oPre.SortID;
                //    }
                //}
            }
            this.neuSpread1.ActiveSheet.ClearSelection();
            this.neuSpread1.ActiveSheet.AddSelection(CurrentActiveRow - 1, 0, 1, 1);
            this.RefreshCombo();
            
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDown_Click(object sender, EventArgs e)
        {
            if (this.neuSpread1.ActiveSheet.ActiveRowIndex >= this.neuSpread1.ActiveSheet.RowCount - 1) return;
            int CurrentActiveRow = this.neuSpread1.ActiveSheet.ActiveRowIndex;
            //��������sortid
            for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
            {
                Neusoft.HISFC.Models.Order.OutPatient.Order ord = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                ord.SortID = this.neuSpread1.ActiveSheet.Rows.Count - i;
                this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Text = Convert.ToString(this.neuSpread1.ActiveSheet.Rows.Count - i);
                this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = this.neuSpread1.ActiveSheet.Rows.Count - i;
            }
            string combNo = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex + 1, this.neuSpread1.ActiveSheetIndex).Combo.ID;
            string oldcombNo = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, this.neuSpread1.ActiveSheetIndex).Combo.ID;
            int Sort = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex + 1, this.neuSpread1.ActiveSheetIndex).SortID;
            int oldSort = this.GetObjectFromFarPoint(this.neuSpread1.ActiveSheet.ActiveRowIndex, this.neuSpread1.ActiveSheetIndex).SortID;
            //int tmp = -1;
            if (combNo == oldcombNo)//������ƶ�
            {
                ((Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.ActiveSheet.Rows[this.neuSpread1.ActiveSheet.ActiveRowIndex - 1].Tag).SortID = oldSort;
                this.neuSpread1.ActiveSheet.Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex - 1, iColumns[28]].Value = oldSort;
                ((Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1.ActiveSheet.Rows[this.neuSpread1.ActiveSheet.ActiveRowIndex].Tag).SortID = Sort;
                this.neuSpread1.ActiveSheet.Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, iColumns[28]].Value = Sort;
            }
            else
            {
                int combNum = 0;
                int oldcombNum = 0;
                for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order oTmp = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (oTmp.Combo.ID == combNo)
                    {
                        combNum++;
                    }
                    if (oTmp.Combo.ID == oldcombNo)
                    {
                        oldcombNum++;
                    }
                }
                for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order oTmp = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (oTmp.Combo.ID == combNo)
                    {
                        oTmp.SortID = oTmp.SortID + (oldcombNum);
                        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oTmp.SortID;
                    }
                    else if (oTmp.Combo.ID == oldcombNo)
                    {
                        oTmp.SortID = oTmp.SortID - (combNum);
                        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oTmp.SortID;
                    }
                    else
                    {

                    }
                }

                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order oPre = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (oPre.SortID == Sort)
                //    {
                //        oPre.SortID = tmp;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oPre.SortID;
                //    }
                //}
                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order o = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (o.SortID == oldSort)
                //    {
                //        o.SortID = Sort;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = o.SortID;
                //    }
                //}
                ////������һ��
                //for (int i = 0; i < this.neuSpread1.ActiveSheet.RowCount; i++)
                //{
                //    Neusoft.HISFC.Models.Order.OutPatient.Order oPre = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                //    if (oPre.SortID == tmp)
                //    {
                //        oPre.SortID = oldSort;
                //        this.neuSpread1.ActiveSheet.Cells[i, iColumns[28]].Value = oPre.SortID;
                //    }
                //}
            }
            this.neuSpread1.ActiveSheet.ClearSelection();
            this.neuSpread1.ActiveSheet.AddSelection(CurrentActiveRow + 1, 0, 1, 1);
            this.RefreshCombo();
            
        }

        /// <summary>
        /// ��������Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangePrice_Click(object sender, EventArgs e)
        {
            Forms.frmPopShow frm = new Forms.frmPopShow();
            frm.Text = "����ĿΪ��������Ŀ��������۸�";
            frm.isPrice = true;
            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.neuSpread1_Sheet1.Rows[this.neuSpread1_Sheet1.ActiveRowIndex].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
            if (order.Item.Price != 0 && order.Item.User03 != "������")
            {
                MessageBox.Show("����Ŀ������������Ŀ�������޸ļ۸�");
                return;
            }
            frm.ModuleName = order.Item.Price.ToString();
            if (order == null)
            {
                return;
            }
            frm.ShowDialog();
            order.Item.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(frm.ModuleName);
            order.Item.User03 = "������";
            this.ucOutPatientItemSelect1.OperatorType = Operator.Modify;
            this.ucItemSelect1_OrderChanged(order, EnumOrderFieldList.Item);
        }

        /// <summary>
        /// Ժע
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnumnuInjectNum_Click(object sender, EventArgs e)
        {
            if (this.neuSpread1_Sheet1.ActiveRowIndex < 0)
            {
                return;
            }
            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.neuSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;

            this.AddInjectNum(order);
        }

        /// <summary>
        /// ճ��ҽ��{DF8058FF-72C0-404f-8F36-6B4057B6F6CD}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPasteOrder_Click(object sender, EventArgs e)
        {
            this.PasteOrder();
        }

        /// <summary>
        ///  �޸��ش�������뵥
        /// {6FAEEEC2-CF03-4b2e-B73F-92C1C8CAE1C0} ����������뵥 yangw 20100504
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPACSApply_Click(object sender, EventArgs e)
        {
            if (PACSApplyInterface == null)
            {
                if (InitPACSApplyInterface() < 0)
                {
                    MessageBox.Show("��ʼ���������뵥�ӿ�ʱ����");
                    return;
                }
            }
            Neusoft.HISFC.Models.Order.OutPatient.Order order = this.neuSpread1.ActiveSheet.ActiveRow.Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;

            if (order.ApplyNo == null || order.ApplyNo == "")
            {
                MessageBox.Show("��ҽ����δ���棬���ȱ��棡");
                return;
            }

            if (PACSApplyInterface.UpdateApply(order.ApplyNo) < 0)
            {
                MessageBox.Show("�޸��ش�������뵥ʱ����");
                return;
            }
        }

        /// <summary>
        /// ����ҽ��
        /// {7E9CE45E-3F00-4540-8C5C-7FF6AE1FF992}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopyOrder_Click(object sender, EventArgs e)
        {
            this.CopyOrder();
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

        #endregion

        #region ��ݼ�
        /// <summary>
		/// ��ݼ�
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                this.mnumnuInjectNum_Click(null, null);
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
        #endregion

        #region �¼ӵĺ���
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            toolBarService.AddToolButton("����", "����ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("���", "���ҽ��", 9, true, false, null);
            ////toolBarService.AddToolButton("������", "��������", 9, true, false, null);
            toolBarService.AddToolButton("ɾ��", "ɾ��ҽ��", 9, true, false, null);
            toolBarService.AddToolButton("ȡ�����", "ȡ�����ҽ��", 9, true, false, null);
            ////toolBarService.AddToolButton("��ϸ", "������ϸ", 9, true, true, null);
            toolBarService.AddToolButton("�˳�ҽ������", "�˳�ҽ������", 9, true, false, null);
            toolBarService.AddToolButton("����", "����", 9, true, false, null);
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
            else if (e.ClickedItem.Text == "����")
            {
                this.RegisterEmergencyPatient();
            } 
        }

        private object currentObject = null;
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            if (neuObject.GetType() == typeof(Neusoft.HISFC.Models.Registration.Register))
            {
                if (currentObject != neuObject)
                    this.Patient = neuObject as Neusoft.HISFC.Models.Registration.Register;
                currentObject = neuObject;
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
                t[0] = typeof(Neusoft.HISFC.BizProcess.Interface.IRecipePrint);
                t[1] = typeof(Neusoft.HISFC.BizProcess.Interface.Common.ICheckPrint);//������뵥
                //{48E6BB8C-9EF0-48a4-9586-05279B12624D}
                t[2] = typeof(Neusoft.HISFC.BizProcess.Interface.IAlterOrder);
                t[3] = typeof(Neusoft.HISFC.BizProcess.Interface.Common.IPacs);
                t[4] = typeof(Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine);
                t[5] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IDoctIdirectFee);
                t[6] = typeof(Neusoft.HISFC.BizProcess.Interface.Order.IDealSubjob);
                return t;
            }
        }
        
        /// <summary>
        /// ������ӡ
        /// </summary>
        /// <param name="recipeNO"></param>
        public void PrintRecipe(string recipeNO)
        {
            Neusoft.HISFC.BizProcess.Interface.IRecipePrint o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.OutPatient.Controls.ucOutPatientOrder), typeof(Neusoft.HISFC.BizProcess.Interface.IRecipePrint)) as Neusoft.HISFC.BizProcess.Interface.IRecipePrint;
            if (o == null)
            {
                MessageBox.Show("�ӿ�δʵ��");
            }
            else
            {
                o.RecipeNO = recipeNO;
                o.SetPatientInfo(this.myPatientInfo);
                
                o.PrintRecipe();
            }
        }

        #endregion

        /// <summary>
        /// ����Ϊxml�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.neuSpread1.Sheets[0], SetingFileName);
        
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
                    //add TK ��Ϊ����{6684E838-EEB3-45b9-A77A-DC1A1251EC24} 2011-03-02
                    //this.pacsInterface.OprationMode = "2";
                    this.pacsInterface.OprationMode = "1";
                    this.pacsInterface.PacsViewType = "2";

                    this.pacsInterface.ShowResultByPatient(patientID);
                    //this.pacsInterface.ShowResultByPatient("985656"); 
                }
            }
            return 0;
        }

        #region {BF58E89A-37A8-489a-A8F6-5BA038EAE5A7} ������ҩ

        /// <summary>
        /// ��ʼ��IReasonableMedicin
        /// </summary>
        private void InitReasonableMedicine()
        {
            if (this.IReasonableMedicine == null)
            {
                this.IReasonableMedicine = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine)) as Neusoft.HISFC.BizProcess.Interface.Order.IReasonableMedicine;
            }
        }

        /// <summary>
        /// ������ҩϵͳ�в鿴�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!e.RowHeader && !e.ColumnHeader && e.Column == 0 && this.EnabledPass)
            {
                if (!this.IReasonableMedicine.PassEnabled)
                {
                    return;
                }

                int iSheetIndex = 0;
                Neusoft.HISFC.Models.Order.OutPatient.Order info = this.GetObjectFromFarPoint(e.Row, iSheetIndex);
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
                    if (this.neuSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag != null && this.neuSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag.ToString() != "0")
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

        private void neuSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.IReasonableMedicine != null)
            {
                this.PassSetQuery(e);
            }
        }

        /// <summary>
        /// ��ѯҩƷ������ҩ��Ϣ
        /// </summary>
        /// <param name="e"></param>
        public void PassSetQuery(FarPoint.Win.Spread.CellClickEventArgs e)
        {
            #region   2010-12-17 �޸� {8C389FCD-3E64-4a90-9830-BE220B952B53}
            //if (!e.RowHeader && !e.ColumnHeader && (e.Column == 9) && this.EnabledPass)
            if (!e.RowHeader && !e.ColumnHeader && (e.Column == 7 || e.Column ==0  ) && this.EnabledPass)
            #endregion 
            {
                if (!this.IReasonableMedicine.PassEnabled)
                {
                    return;
                }
                int iSheetIndex = 0;
                Neusoft.HISFC.Models.Order.OutPatient.Order info = this.GetObjectFromFarPoint(e.Row, iSheetIndex);
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
                if (e.Column == 7)
                {
                    #region ҩƷ��ѯ
                    try
                    {
                        int iCellLeft, iCellTop, iCellRight, iCellBottom;

                        #region ��ȡ������������ʾλ��
                        //��ȡFarPoint ��Cell[0,0]��Left���� �Թ����������ʾ
                        int iRowHeader = (int)this.Left + (int)this.neuSpread1.Sheets[iSheetIndex].RowHeader.Columns[0].Width;
                        //��ȡFarPoint��Cell[0,0]��Top���� �Թ����������ʾ
                        int iColumnHeader = (int)this.Top + (int)this.neuSpread1.Sheets[iSheetIndex].ColumnHeader.Rows[0].Height;
                        //�����Cell��Left���� �Թ����������ʾ
                        iCellLeft = iRowHeader + (int)this.neuSpread1.Sheets[iSheetIndex].Columns[7].Width;
                        //��ǰ�����Cell��ɼ���ʼ��֮��ļ������
                        int iRowNum = (int)System.Math.Floor(((e.Y - iColumnHeader) / this.neuSpread1.Sheets[iSheetIndex].Rows[0].Height));
                        //�����Cell��Top���� �Թ����������ʾ
                        iCellTop = iColumnHeader + iRowNum * (int)this.neuSpread1.Sheets[iSheetIndex].Rows[0].Height;

                        System.Drawing.Point cellPointClient = new Point(iCellLeft - 50, iCellTop);
                        System.Drawing.Point cellPointScreen = this.PointToScreen(cellPointClient);
                        iCellRight = cellPointScreen.X + (int)this.neuSpread1.Sheets[iSheetIndex].Columns[7].Width;
                        iCellBottom = cellPointScreen.Y + (int)this.neuSpread1.Sheets[iSheetIndex].Rows[iRowNum].Height;
                        #endregion


                        if (this.bIsDesignMode)
                        {
                            this.IReasonableMedicine.PassQueryDrug(info.Item.ID, info.Item.Name, info.DoseUnit, info.Usage.Name, cellPointScreen.X - 90,
                                cellPointScreen.Y, iCellRight - 90, iCellBottom + this.ucOutPatientItemSelect1.Size.Height);
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
                if (e.Column == 0)
                {
                    if (this.neuSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag != null && this.neuSpread1.Sheets[iSheetIndex].Cells[e.Row, e.Column].Tag.ToString() != "0")
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
            List<Neusoft.HISFC.Models.Order.OutPatient.Order> alOrder = new List<Neusoft.HISFC.Models.Order.OutPatient.Order>();
            Neusoft.HISFC.Models.Order.OutPatient.Order order;
            DateTime sysTime = this.OrderManagement.GetDateTimeFromSysDateTime();
            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                order = this.GetObjectFromFarPoint(i, 0);
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
                    order.Frequency = (Neusoft.HISFC.Models.Order.Frequency)helper.GetObjectFromID(order.Frequency.ID);
                }
                order.ApplyNo = this.OrderManagement.GetSequence("Order.Pass.Sequence");
                alOrder.Add(order);
            }
            //for (int i = 0; i < this.fpSpread1_Sheet2.Rows.Count; i++)
            //{
            //    order = this.GetObjectFromFarPoint(i, 1);
            //    if (order == null)
            //    {
            //        continue;
            //    }
            //    if (order.Status == 3)
            //    {
            //        continue;
            //    }
            //    if (order.MOTime.Date != sysTime.Date)
            //    {
            //        continue;
            //    }
            //    if (order.Item.ItemType.ToString() != Neusoft.HISFC.Object.Base.EnumItemType.Drug.ToString())
            //    {
            //        continue;
            //    }
            //    if (this.helper != null)
            //    {
            //        order.Frequency = (Neusoft.HISFC.Object.Order.Frequency)helper.GetObjectFromID(order.Frequency.ID);
            //    }
            //    order.ApplyNO = this.OrderManagement.GetSeqence();
            //    alOrder.Add(order);
            //}
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
        public void PassSaveCheck(List<Neusoft.HISFC.Models.Order.OutPatient.Order> alOrder, int checkType, bool warnPicFlag)
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

            #region �¸ĵ�--{3190F16B-F74C-459d-B58A-7DE0AF3F8E51}

            System.Collections.Generic.Dictionary<string, int> dict = new Dictionary<string, int>();

            Neusoft.HISFC.Models.Order.OutPatient.Order tempOrder;
            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                //string orderId = alOrder[i].ApplyNo;
                tempOrder = this.GetObjectFromFarPoint(i, 0);

                if (tempOrder == null)
                {
                    continue;
                }

                if (tempOrder.Status == 3 || tempOrder.Item.SysClass.ID.ToString().Substring(0, 1) != "P")
                {
                    continue;
                }

                dict.Add(tempOrder.ApplyNo, i);

                //int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);
                //this.AddWarnPicturn(i, 0, iWarn);
            }

            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order tmp in alOrder)
            {
                string orderId = tmp.ApplyNo;

                int idx = -1;
                dict.TryGetValue(orderId, out idx);

                int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);

                if (idx != -1)
                {
                    this.AddWarnPicturn(idx, 0, iWarn);
                }
            }

            #endregion


            ////   ԭ����
            ////

            //Neusoft.HISFC.Models.Order.OutPatient.Order tempOrder;
            //for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            //{
            //    string orderId = alOrder[i].ApplyNo;
            //    tempOrder = this.GetObjectFromFarPoint(i, 0);

            //    if (tempOrder == null)
            //    {
            //        continue;
            //    }

            //    if (tempOrder.Status == 3 || tempOrder.Item.SysClass.ID.ToString().Substring(0, 1) != "P")
            //    {
            //        continue;
            //    }

            //    int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);
            //    this.AddWarnPicturn(i, 0, iWarn);
            //}

            ////
            ////


            //////for (int i = 0; i < this.fpSpread1_Sheet2.Rows.Count; i++)
            //////{
            //////    string orderId = alOrder[this.fpSpread1_Sheet1.RowCount + i].ApplyNO;
            //////    tempOrder = this.GetObjectFromFarPoint(i, 1);
            //////    if (tempOrder == null)
            //////    {
            //////        continue;
            //////    }
            //////    if (tempOrder.Status == 3 || tempOrder.Item.SysClass.ID.ToString().Substring(0, 1) != "P")
            //////    {
            //////        continue;
            //////    }
            //////    int iWarn = this.IReasonableMedicine.PassGetWarnFlag(orderId);
            //////    this.AddWarnPicturn(i, 1, iWarn);
            //////}
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
                FarPoint.Win.Spread.CellType.TextCellType t = new FarPoint.Win.Spread.CellType.TextCellType();
                FarPoint.Win.Picture pic = new FarPoint.Win.Picture();
                pic.Image = System.Drawing.Image.FromFile(picturePath, true);
                pic.TransparencyColor = System.Drawing.Color.Empty;
                t.BackgroundImage = pic;
                this.neuSpread1.Sheets[iSheet].Cells[iRow, 0].CellType = t;			//ҽ������
                this.neuSpread1.Sheets[iSheet].Cells[iRow, 0].Tag = "1";							//���������
                this.neuSpread1.Sheets[iSheet].Cells[iRow, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
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
            int iSheetIndex = 0;
            int iRow = this.neuSpread1.Sheets[iSheetIndex].ActiveRowIndex;
            Neusoft.HISFC.Models.Order.OutPatient.Order info = this.GetObjectFromFarPoint(iRow, iSheetIndex);
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
        /// <summary>
        /// ����ҩƷϵͳҩƷ��ѯ
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
        }
        #endregion
    }
    
}

