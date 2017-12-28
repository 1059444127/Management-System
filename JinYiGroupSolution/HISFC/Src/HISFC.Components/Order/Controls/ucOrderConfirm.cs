using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FarPoint.Win.Spread;
using FarPoint.Win;
namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ����˵�˵]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucOrderConfirm : Neusoft.FrameWork.WinForms.Controls.ucBaseControl,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucOrderConfirm()
        {
            InitializeComponent();
        }

        #region ����
        private int LongOrderCount = 0; //����ҽ������
        private int ShortOrderCount = 0;//��ʱҽ������
        string strFileName = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath +
            Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "fpOrderConfirm.xml";
        DataTable dtMain;
        DataSet myDataSet;
        DataTable dtChild1;
        DataTable dtChild2;
        Neusoft.FrameWork.Models.NeuObject OrderId = new Neusoft.FrameWork.Models.NeuObject();
        Neusoft.FrameWork.Models.NeuObject ComboNo = new Neusoft.FrameWork.Models.NeuObject();
        protected FarPoint.Win.Spread.Cell CurrentCellName;
        string PatientId = "";
        /// <summary>
        /// ������Ϣ�б�
        /// </summary>
        protected ArrayList alpatientinfos;
        protected Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();
        protected Neusoft.HISFC.BizProcess.Integrate.Order orderIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Order();
        protected Neusoft.HISFC.BizProcess.Integrate.RADT radtIntegrate = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        protected Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet nurseFeeBill = null;
        protected Neusoft.HISFC.BizProcess.Interface.Order.IApplyFeeSheet nurseApplyFeeBill=null;
        protected Neusoft.HISFC.BizProcess.Integrate.Fee feeIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        /// <summary>
        /// IOP�ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.IHE.IOP iop = null;
        #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
        private Control activeBtn = null;
        private Color foreColor = Color.White;
        private Color backColor = Color.DarkBlue;
        private Neusoft.FrameWork.Management.ControlParam ctlMgr = new Neusoft.FrameWork.Management.ControlParam();
        #endregion
        #endregion
        private Neusoft.HISFC.Models.Base.MessType messType = Neusoft.HISFC.Models.Base.MessType.Y;
        /// <summary>
        /// �Ƿ��ж�Ƿ�ѣ�Ƿ���Ƿ���ʾ
        /// </summary>
        [Category("�ؼ�����"), Description("Y���ж�Ƿ��,����������շ�,M���ж�Ƿ�ѣ���ʾ�Ƿ�����շ�,N�����ж�Ƿ��")]
        public Neusoft.HISFC.Models.Base.MessType MessageType
        {
            set
            {
                messType = value;
            }
            get
            {
                return messType;
            }
        }
        #region ģ��1 ��ʼ��

        /// <summary>
        /// ��ʼ�������ؼ�
        /// </summary>
        private void InitControl()
        {
            this.InitFp();
            ucSubtblManager1 = new ucSubtblManager();
            this.ucSubtblManager1.IsVerticalShow = true;
            this.DockingManager();
            this.ucSubtblManager1.ShowSubtblFlag += new ucSubtblManager.ShowSubtblFlagEvent(ucSubtblManager1_ShowSubtblFlag);
            #region addby xuewj 2010-10-5 ����StatusBarPanel {C0E71DA8-F246-4ff2-98CB-7EC72A767453}
            //base.OnStatusBarInfo(null, "(��ɫ���¿�)(��ɫ�����)(��ɫ��ִ��)(��ɫ������)");
            base.InsertStastusBarPanel(Properties.Resources.ҽ����ҽ��״̬, "", 1); 
            #endregion
            #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
            this.Initcontrolargument();
            ChangeBtnVisble(this.fpSpread.ActiveSheetIndex);
            this.ChangeBtnColor(this.btnAll);
            #endregion
        }

        void ucSubtblManager1_ShowSubtblFlag(string operFlag, bool isShowSubtblFlag, object sender)
        {
            string s = this.CurrentCellName.Text;
            if (!isShowSubtblFlag)
            {
                //����ҽ����־
                if (s.Substring(0, 1) == "@")
                {
                    this.CurrentCellName.Text = s.Substring(1);
                }
            }
            else
            {
                if (s.Substring(0, 1) != "@")
                {
                    this.CurrentCellName.Text = "@" + s;
                }
            }
            if (this.dockingManager != null)
                this.dockingManager.HideAllContents();
        }
     
        
        /// <summary>
        /// ��ʼ��fpTreeView1
        /// </summary>
        private void InitFp()
        {
            this.fpSpread.ChildViewCreated += new FarPoint.Win.Spread.ChildViewCreatedEventHandler(fpSpread_ChildViewCreated);
            
            this.fpSpread.Sheets[0].SheetName = "����ҽ��";
            this.fpSpread.Sheets[1].SheetName = "��ʱҽ��";
            this.fpSpread.Sheets[0].Columns[0].Visible = false;
            this.fpSpread.Sheets[0].Columns[1].Label = "��ˣ۳��ڣ�";
            this.fpSpread.Sheets[0].Columns[2].Label = "��������";
            this.fpSpread.Sheets[0].Columns[3].Label = "����";
            this.fpSpread.Sheets[0].RowCount = 0;
            this.fpSpread.Sheets[0].ColumnCount = 4;
            this.fpSpread.Sheets[0].Columns[1].Width = 100;
            this.fpSpread.Sheets[0].Columns[2].Width = 100;
            this.fpSpread.Sheets[0].GrayAreaBackColor = Color.WhiteSmoke;


            this.fpSpread.Sheets[1].Columns[0].Visible = false;
            this.fpSpread.Sheets[1].Columns[1].Label = "��ˣ���ʱ��";
            this.fpSpread.Sheets[1].Columns[2].Label = "��������";
            this.fpSpread.Sheets[1].Columns[3].Label = "����";
            this.fpSpread.Sheets[1].RowCount = 0;
            this.fpSpread.Sheets[1].ColumnCount = 4;
            this.fpSpread.Sheets[1].GrayAreaBackColor = Color.WhiteSmoke;
            this.fpSpread.Sheets[1].Columns[1].Width = 100;
            this.fpSpread.Sheets[1].Columns[2].Width = 100;
 
            this.fpSpread.Sheets[0].DataAutoSizeColumns = false;
            this.fpSpread.Sheets[1].DataAutoSizeColumns = false;

            this.fpSpread.Sheets[0].Rows.Get(-1).BackColor = Color.LightSkyBlue;
            this.fpSpread.Sheets[1].Rows.Get(-1).BackColor = Color.LightSkyBlue;

            this.fpSpread.Sheets[0].CellChanged+=new FarPoint.Win.Spread.SheetViewEventHandler(ucOrderConfirm_CellChanged);
            this.fpSpread.Sheets[1].CellChanged += new FarPoint.Win.Spread.SheetViewEventHandler(ucOrderConfirm_CellChanged);
            this.fpSpread.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread_CellClick);
            this.fpSpread.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread_CellDoubleClick);
            this.fpSpread.MouseUp += new MouseEventHandler(fpSpread_MouseUp);
            this.fpSpread.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(fpSpread_SheetTabClick);
        }

       
        
        #endregion

        #region ģ��2 ����
    
        /// <summary>
        /// ��ǰҽ������
        /// </summary>
        protected Neusoft.HISFC.Models.Order.EnumType myShowOrderType = Neusoft.HISFC.Models.Order.EnumType.LONG;
        /// <summary>
        /// ��ʾҽ������
        /// </summary>
        public Neusoft.HISFC.Models.Order.EnumType ShowOrderType
        {
            get
            {
                return this.myShowOrderType;
            }
            set
            {
                this.myShowOrderType = value;
                if (this.myShowOrderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    this.fpSpread.ActiveSheetIndex = 0;
                }
                else
                {
                    this.fpSpread.ActiveSheetIndex = 1;
                }
            }
        }

        /// <summary>
        /// ����Ա����
        /// </summary>
        protected Neusoft.HISFC.Models.Base.Employee myOperator;
        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        protected Neusoft.HISFC.Models.Base.Employee Operator
        {
            get
            {
                if (myOperator == null)
                    myOperator = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
                return myOperator;
            }
        }
        #endregion

        #region ģ��4 ����
      
        /// <summary>
        /// ��ѯҽ��
        /// </summary>
        private void QueryOrder()
        {
            if (this.alpatientinfos == null) return;
            this.fpSpread.ChildViewCreated += new FarPoint.Win.Spread.ChildViewCreatedEventHandler(fpSpread_ChildViewCreated);
            
            this.myShowOrderType = Neusoft.HISFC.Models.Order.EnumType.SHORT;//��ʱҽ����ʼ��
            this.fpSpread.Sheets[1].DataSource = CreateDataSetShort(this.alpatientinfos);

            this.myShowOrderType = Neusoft.HISFC.Models.Order.EnumType.LONG;//����ҽ����ʼ��
            this.fpSpread.Sheets[0].DataSource = CreateDataSetLong(this.alpatientinfos);

            this.fpSpread.Sheets[0].Columns[0].Visible = false;
            this.fpSpread.Sheets[0].Columns[2].Locked = true;
            this.fpSpread.Sheets[0].Columns[3].Locked = true;
            this.fpSpread.Sheets[0].GrayAreaBackColor = Color.WhiteSmoke;
            this.fpSpread.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            Classes.Function.DrawCombo(this.fpSpread.Sheets[0], 2, 5, 1);



            this.fpSpread.Sheets[1].Columns[0].Visible = false;
            this.fpSpread.Sheets[1].Columns[2].Locked = true;
            this.fpSpread.Sheets[1].Columns[3].Locked = true;
            this.fpSpread.Sheets[1].GrayAreaBackColor = Color.WhiteSmoke;
            this.fpSpread.Sheets[1].OperationMode = FarPoint.Win.Spread.OperationMode.SingleSelect;

            Classes.Function.DrawCombo(this.fpSpread.Sheets[1], 2, 5, 1);
            ////{92E9D8ED-A768-47b9-9C27-16FEFA990B84}
            SetRowColor(fpSpread.Sheets[1]);
            SetRowColor(fpSpread.Sheets[0]);
            this.ExpandAll();//չ��

            this.refreshView();//ˢ���б���Ϣ

            #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
            this.ChangeBtnColor(this.btnAll);
            this.ChangeBtnSize();
            #endregion
        }
        //{92E9D8ED-A768-47b9-9C27-16FEFA990B84}
        private void SetRowColor(FarPoint.Win.Spread.SheetView sv)
        {
            

            for (int i = 0; i < sv.RowCount; i++)
            {
                Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = this.radtIntegrate.GetPatientInfoByPatientNO(sv.Cells[i, 0].Text);
                string note = string.Empty;
                if (patientInfo != null)
                {
                    note = string.Format("Ԥ �� ��{0}\n" + "�����ܶ{1}\n" + "�Էѽ�{2}\n" + "��    �{3}\n" + "�� �� �ߣ�{4}",
                        patientInfo.FT.PrepayCost.ToString(),
                        patientInfo.FT.TotCost.ToString(),
                        patientInfo.FT.OwnCost.ToString(),
                        patientInfo.FT.LeftCost.ToString(),
                        patientInfo.PVisit.MoneyAlert.ToString());

                }

                string text = sv.Cells[i, 4].Text;
                if (text == "��Ƿ��")
                {
                    sv.Cells[i,4].ForeColor = Color.Red;
                }
                sv.Cells[i, 4].Note = note;
               
               
                
            }
        }

        
      
        /// <summary>
        /// չ��ȫ���ڵ�
        /// </summary>
        private void ExpandAll()
        {
            for (int j = 0; j < this.fpSpread.Sheets.Count; j++)
            {
                for (int i = 0; i < this.fpSpread.Sheets[j].Rows.Count; i++)
                {
                    this.fpSpread.Sheets[j].ExpandRow(i, true);
                    SheetView sv = this.fpSpread.Sheets[j].GetChildView(i, 0);
                    this.SetChildViewStyle(sv);
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iSheet"></param>
        /// <returns></returns>
        private int GetColumnIndex(string name, int iSheet)
        {
            DataTable dt = null;
            if (iSheet == 0)
            {
                dt = dtChild1;
            }
            else
            {
                dt = dtChild2;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == name) return i;
            }
            MessageBox.Show("ȱ����" + Name);
            return -1;
        }


        /// <summary>
        /// ���س���ҽ����dataSet
        /// </summary>
        /// <param name="alPatient"></param>
        /// <returns></returns>
        private DataSet CreateDataSetLong(ArrayList alPatient)
        {
            //���崫��DataSet
            myDataSet = new DataSet();
            myDataSet.EnforceConstraints = false;//�Ƿ���ѭԼ������
            //��������
            System.Type dtStr = System.Type.GetType("System.String");
            System.Type dtBool = System.Type.GetType("System.Boolean");
            System.Type dtInt = System.Type.GetType("System.Int32");
            //�����********************************************************
            //Main Table

            dtMain = myDataSet.Tables.Add("TableMain");
            //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
            dtMain.Columns.AddRange(new DataColumn[] { new DataColumn("ID", dtStr), new DataColumn("���", dtBool), new DataColumn("��������", dtStr), new DataColumn("����", dtStr),new DataColumn("�Ƿ�Ƿ��",dtStr) });
            //ChildTable1

            dtChild1 = myDataSet.Tables.Add("TableChild1");
            dtChild1.Columns.AddRange(new DataColumn[]{new DataColumn("ID",dtStr),new DataColumn("ҽ����ˮ��", dtStr),
					new DataColumn("��Ϻ�", dtStr),new DataColumn("���", dtBool),new DataColumn("ҽ������", dtStr),
					new DataColumn("��", dtStr),new DataColumn("���", dtStr),new DataColumn("ִ�п���", dtStr),//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5}  �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                    new DataColumn("����",dtStr),//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                    new DataColumn("ÿ����", dtStr),
					new DataColumn("Ƶ��", dtStr),new DataColumn("�÷�", dtStr),new DataColumn("����", dtStr),
				    new DataColumn("����", dtStr),
                    new DataColumn("ҽ������", dtStr),new DataColumn("��", dtBool),
					new DataColumn("��ʼʱ��", dtStr),new DataColumn("ֹͣʱ��", dtStr),new DataColumn("����ҽ��", dtStr),
					new DataColumn("����ʱ��", dtStr),new DataColumn("ֹͣҽ��", dtStr),
					new DataColumn("��ע", dtStr),new DataColumn("˳���", dtStr),new DataColumn("��ע",dtStr),new DataColumn("״̬",dtStr),new DataColumn("Ƥ��",dtStr)});
            this.OrderId.ID = "1";
            this.ComboNo.ID = "2";


            this.fpSpread.Sheets[0].RowCount = 0;

            string tempCombNo = "";
            this.LongOrderCount = 0;
            //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
            Neusoft.HISFC.BizProcess.Integrate.Fee feeManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee();
            string isOwnFee = string.Empty;
            for (int i = 0; i < alPatient.Count; i++)
            {
                Neusoft.HISFC.Models.RADT.PatientInfo p = (Neusoft.HISFC.Models.RADT.PatientInfo)alPatient[i];
                //��ѯδ��˵�ҽ��--�жϲ�ѯҽ������
                ArrayList al = orderManager.QueryIsConfirmOrder(p.ID, myShowOrderType, false);
                if (al.Count > 0)
                {
                    //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
                    if (feeManagement.IsPatientLackFee(p) == true) //Ƿ�ѻ���
                    {
                        isOwnFee = "��Ƿ��";
                    }
                    else
                    {
                        isOwnFee = "δǷ��";
                    }
                    this.LongOrderCount = this.LongOrderCount + al.Count;
                    //{92E9D8ED-A768-47b9-9C27-16FEFA990B84}
                    dtMain.Rows.Add(new Object[] { p.ID, false, p.Name, p.PVisit.PatientLocation.Bed.ID.Substring(4), isOwnFee });//�����
                    for (int j = 0; j < al.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order o = al[j] as Neusoft.HISFC.Models.Order.Inpatient.Order;

                        if (o.IsPermission) //�Ѿ���Ȩ�޵�ҩƷ
                            o.Item.Name = "���̡�" + o.Item.Name;

                        # region ͬһ�����ȡһ�ξͿ�����  
                        if (tempCombNo != o.Combo.ID)
                        {
                            int count = this.orderManager.QuerySubtbl(o.Combo.ID).Count;
                            tempCombNo = o.Combo.ID;
                            if (count > 0)
                                o.Item.Name = "@" + o.Item.Name; //��ʾ����
                        }
                        # endregion
                        if (o.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
                        {
                            Neusoft.HISFC.Models.Pharmacy.Item item = o.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                            
                            dtChild1.Rows.Add(new Object[] {o.Patient.ID,o.ID,o.Combo.ID,false,o.Item.Name,
															   "",o.Item.Specs,o.ExeDept.Name,//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5} �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                                                               "",//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                                               o.DoseOnce.ToString()+item.DoseUnit ,
															   o.Frequency.ID,o.Usage.Name,o.Item.Qty ==0 ? "":(o.Item.Qty.ToString()+o.Unit),
															   o.HerbalQty==0 ? "":o.HerbalQty.ToString(),
															   o.OrderType.Name,o.IsEmergency,
                                                               o.BeginTime.ToString("MM-dd HH:mm"),
															   o.EndTime.ToString("MM-dd HH:mm") == "01-01 00:00" ? "":o.EndTime.ToString("MM-dd HH:mm"),
                                                               o.ReciptDoctor.Name,o.MOTime,
															   o.DCOper.Name,o.Memo,o.SortID,o.Note,
                                                               Classes.Function.OrderStatus(o.Status),
                                                                Classes.Function.TransHypotest(o.HypoTest)});

                        }
                        else if (o.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
                        {
                            
                            dtChild1.Rows.Add(new Object[] {o.Patient.ID,o.ID,o.Combo.ID,false,o.Item.Name,
															   "",o.Item.Specs,o.ExeDept.Name,//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5} �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                                                               o.Item.Price.ToString(),//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                                               "" ,
															   o.Frequency.ID,"",o.Item.Qty.ToString()+o.Unit,"",
															   o.OrderType.Name,o.IsEmergency,
                                                               o.BeginTime.ToString("MM-dd HH:mm"),
															   o.EndTime.ToString("MM-dd HH:mm") == "01-01 00:00" ? "":o.EndTime.ToString("MM-dd HH:mm"),
                                                               o.ReciptDoctor.Name,o.MOTime,
															   o.DCOper.Name,o.Memo,o.SortID,o.Note,
                                                               Classes.Function.OrderStatus(o.Status),
                                                                Classes.Function.TransHypotest(o.HypoTest)});
                        }
                    }
                }
            }
            //�������ʾ
            myDataSet.Relations.Add("TableChild1", dtMain.Columns["ID"], dtChild1.Columns["ID"]);
           
            return myDataSet;
        }
        /// <summary>
        /// ������ʱҽ����DataSet
        /// </summary>
        /// <param name="alPatient"></param>
        /// <returns></returns>
        private DataSet CreateDataSetShort(ArrayList alPatient)
        {
            DataTable dtMain;
            DataSet myDataSet;
            //ataTable dtChild1;
            //���崫��DataSet
            myDataSet = new DataSet();
            myDataSet.EnforceConstraints = false;//�Ƿ���ѭԼ������
            //��������
            System.Type dtStr = System.Type.GetType("System.String");
            System.Type dtBool = System.Type.GetType("System.Boolean");
            System.Type dtInt = System.Type.GetType("System.Int32");
            //�����********************************************************
            //Main Table

            dtMain = myDataSet.Tables.Add("TableMain");
            //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
            dtMain.Columns.AddRange(new DataColumn[] { new DataColumn("ID", dtStr), new DataColumn("���", dtBool), new DataColumn("��������", dtStr), new DataColumn("����", dtStr),new DataColumn("�Ƿ�Ƿ��",dtStr)});
            //ChildTable1

            dtChild2 = myDataSet.Tables.Add("TableChild1");
            dtChild2.Columns.AddRange(new DataColumn[]{new DataColumn("ID",dtStr),new DataColumn("ҽ����ˮ��", dtStr),
														  new DataColumn("��Ϻ�", dtStr),new DataColumn("���", dtBool),new DataColumn("ҽ������", dtStr),
														  new DataColumn("��", dtStr),new DataColumn("���", dtStr),new DataColumn("ִ�п���", dtStr),//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5} �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                                                          new DataColumn("����",dtStr),//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                                          new DataColumn("ÿ����", dtStr),
														  new DataColumn("Ƶ��", dtStr),new DataColumn("�÷�", dtStr),new DataColumn("����", dtStr),
														  new DataColumn("����", dtStr), 
                                                          new DataColumn("ҽ������", dtStr),new DataColumn("��", dtBool),
														  new DataColumn("��ʼʱ��", dtStr),new DataColumn("ֹͣʱ��", dtStr),new DataColumn("����ҽ��", dtStr),
														  new DataColumn("����ʱ��", dtStr),new DataColumn("ֹͣҽ��", dtStr),
														  new DataColumn("��ע", dtStr),new DataColumn("˳���", dtStr),new DataColumn("��ע",dtStr),new DataColumn("״̬",dtStr),new DataColumn("Ƥ��",dtStr)});
            this.OrderId.ID = "1";
            this.ComboNo.ID = "2";
           
            
            this.fpSpread.Sheets[1].RowCount = 0;
            
            string tempCombNo = "";
            this.ShortOrderCount = 0;
            Neusoft.HISFC.BizProcess.Integrate.Fee feeManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee ();
            for (int i = 0; i < alPatient.Count; i++)
            {
                //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
                string isOwnFee = string.Empty;
                Neusoft.HISFC.Models.RADT.PatientInfo p = (Neusoft.HISFC.Models.RADT.PatientInfo)alPatient[i];

               
                //��ѯδ��˵�ҽ��--�жϲ�ѯҽ������
                ArrayList al = this.orderManager.QueryIsConfirmOrder(p.ID, myShowOrderType, false);	//��ѯδ��˵�ҽ��
                if (al.Count > 0)
                {
                    //{96C7CE3E-CBD3-4862-A5F2-66DBB4DBF4CB}
                    if (feeManagement.IsPatientLackFee(p) == true) //Ƿ�ѻ���
                    {
                        isOwnFee = "��Ƿ��";
                    }
                    else
                    {
                        isOwnFee = "δǷ��";
                    }
                    this.ShortOrderCount = this.ShortOrderCount + al.Count;

                    //{C3C32101-297D-40c1-97BA-46938537002B}  ��λ�Ž�ȡ
                    string bedNO = p.PVisit.PatientLocation.Bed.ID;
                    if (bedNO.Length > 4)
                    {
                        bedNO = bedNO.Substring( 4 );
                    }
                    //{92E9D8ED-A768-47b9-9C27-16FEFA990B84}
                    dtMain.Rows.Add( new Object[] { p.ID, false, p.Name, bedNO ,isOwnFee} );//�����
                    for (int j = 0; j < al.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order o = al[j] as Neusoft.HISFC.Models.Order.Inpatient.Order;

                        if (o.IsPermission) //
                            o.Item.Name = "���̡�" + o.Item.Name;

                       
                        # region ͬһ�����ȡһ�ξͿ����� 
                        if (tempCombNo != o.Combo.ID)
                        {
                            int count = this.orderManager.QuerySubtbl(o.Combo.ID).Count;
                            tempCombNo = o.Combo.ID;
                            if (count > 0)
                                o.Item.Name = "@" + o.Item.Name; //��ʾ����
                        }
                        # endregion
                        if (o.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
                        {
                            Neusoft.HISFC.Models.Pharmacy.Item item = o.Item as Neusoft.HISFC.Models.Pharmacy.Item;

                            dtChild2.Rows.Add(new Object[] {o.Patient.ID,o.ID,o.Combo.ID,false,o.Item.Name,
															   "",o.Item.Specs,o.ExeDept.Name,//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5} �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                                                               "",//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                                               o.DoseOnce.ToString()+item.DoseUnit ,
															   o.Frequency.ID,o.Usage.Name,o.Item.Qty ==0 ? "":(o.Item.Qty.ToString()+o.Unit),
															   o.HerbalQty==0 ? "":o.HerbalQty.ToString(),              
															   o.OrderType.Name,o.IsEmergency,
                                                                  o.BeginTime.ToString("MM-dd HH:mm"),
															  o.EndTime.ToString("MM-dd HH:mm") == "01-01 00:00" ? "":o.EndTime.ToString("MM-dd HH:mm"),
                                                               o.ReciptDoctor.Name,o.MOTime,
															   o.DCOper.Name,o.Memo,o.SortID,o.Note,
                                                                Classes.Function.OrderStatus(o.Status),
                                                                Classes.Function.TransHypotest(o.HypoTest) });

                        }
                        else if (o.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
                        {
                            dtChild2.Rows.Add(new Object[] {o.Patient.ID,o.ID,o.Combo.ID,false,o.Item.Name,
															   "",o.Item.Specs,	o.ExeDept.Name,//ִ�п����� ��ǰ {AFC6B462-3C6D-4ada-B74E-CAD4E9B402D5} �ٴ���ǰ{8BD9B4E1-DFB7-45c3-B059-9AF6FB155590}
                                                               o.Item.Price.ToString(),//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                                               "" ,
															   o.Frequency.ID,"",o.Item.Qty.ToString()+o.Unit,"",
                                                               o.OrderType.Name,o.IsEmergency,
                                                                o.BeginTime.ToString("MM-dd HH:mm"),
															   o.EndTime.ToString("MM-dd HH:mm") == "01-01 00:00" ? "":o.EndTime.ToString("MM-dd HH:mm"),
                                                               o.ReciptDoctor.Name,o.MOTime,
															   o.DCOper.Name,o.Memo,o.SortID,o.Note,
                                                                 Classes.Function.OrderStatus(o.Status),
                                                                Classes.Function.TransHypotest(o.HypoTest)});
                        }
                        
                    }
                }
            }
            //����
            myDataSet.Relations.Add("TableChild1", dtMain.Columns["ID"], dtChild2.Columns["ID"]);
            
            return myDataSet;
        }

        public void SetChildViewStyle(FarPoint.Win.Spread.SheetView sv)
        {
            this.SetChildViewStyle(sv, true);
        }
        public void SetChildViewStyle(FarPoint.Win.Spread.SheetView sv, bool SetChildViewStyle)
        {
            try
            {
                //Make the header font italic
                sv.ColumnHeader.DefaultStyle.Font = this.fpSpread.Font;
                sv.ColumnHeader.DefaultStyle.Border = new EmptyBorder();
                sv.ColumnHeader.DefaultStyle.BackColor = Color.White;
                sv.ColumnHeader.DefaultStyle.ForeColor = Color.Black;
                //Change the sheet corner color
                sv.SheetCornerStyle.BackColor = Color.White;
                sv.SheetCornerStyle.Border = new EmptyBorder();

                //Clear the autotext
                sv.RowHeader.AutoText = HeaderAutoText.Blank;

                sv.RowHeader.DefaultStyle.BackColor = Color.Honeydew;
                sv.RowHeader.DefaultStyle.ForeColor = Color.Black;

                sv.ColumnHeaderVisible = true;
                sv.RowHeaderVisible = SetChildViewStyle;
                sv.RowHeaderAutoText = HeaderAutoText.Numbers;
                for (int i = 0; i < sv.RowCount; i++) sv.Rows[i].Height = 20;
                sv.CellChanged += new SheetViewEventHandler(sv_CellChanged);
            }
            catch { }

            
            sv.DataAutoSizeColumns = false;
            sv.OperationMode = OperationMode.SingleSelect;

           
            //hide or show the ID column
            sv.Columns[0].Visible = false;
            sv.Columns[1].Visible =false;
            sv.Columns[2].Visible =false;
            sv.Columns[3].Visible =true;
            sv.Columns[3].Width = 32;
            sv.Columns[3].Locked = true;
            sv.Columns[4].Width = 200;
            sv.Columns[4].Locked = true;
            sv.Columns[5].Width = 15;
            sv.Columns[5].Locked = true;
            sv.Columns[6].Width = 62;
            sv.Columns[6].Locked = true;
            #region add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
            //sv.Columns[7].Width = 48;
            //sv.Columns[7].Locked = true;
            //sv.Columns[8].Width = 37;
            //sv.Columns[8].Locked = true;
            //sv.Columns[9].Width = 33;
            //sv.Columns[9].Locked = true;
            //sv.Columns[10].Width = 35;
            //sv.Columns[10].Locked = true;
            //sv.Columns[11].Width = 33;
            //sv.Columns[11].Locked = true;
            //sv.Columns[12].Width = 59;
            //sv.Columns[12].Locked = true;
            //sv.Columns[13].Width = 19;
            //sv.Columns[13].Visible = false;
            //sv.Columns[14].Width = 63;
            //sv.Columns[14].Locked = true;
            //sv.Columns[15].Width = 63;
            //sv.Columns[15].Locked = true;
            //sv.Columns[16].Width = 59;
            //sv.Columns[16].Locked = true;
            //sv.Columns[17].Width = 59;
            //sv.Columns[17].Locked = true;
            //sv.Columns[18].Width = 59;
            //sv.Columns[18].Locked = true;

            sv.Columns[7].Width = 48;
            sv.Columns[7].Locked = true;
            sv.Columns[7].HorizontalAlignment = CellHorizontalAlignment.Right;
            sv.Columns[8].Width = 48;
            sv.Columns[8].Locked = true;
            sv.Columns[9].Width = 37;
            sv.Columns[9].Locked = true;
            sv.Columns[10].Width = 33;
            sv.Columns[10].Locked = true;
            sv.Columns[11].Width = 35;
            sv.Columns[11].Locked = true;
            sv.Columns[12].Width = 33;
            sv.Columns[12].Locked = true;
            sv.Columns[13].Width = 59;
            sv.Columns[13].Locked = true;
            sv.Columns[14].Width = 63;
            sv.Columns[14].Locked = true;
            sv.Columns[15].Width = 19;
            sv.Columns[15].Visible = false;
            sv.Columns[16].Width = 63;
            sv.Columns[16].Locked = true;
            sv.Columns[17].Width = 59;
            sv.Columns[17].Locked = true;
            sv.Columns[18].Width = 59;
            sv.Columns[18].Locked = true;
            sv.Columns[19].Width = 59;
            sv.Columns[19].Locked = true; 
            #endregion
        }


        protected void refreshView()
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < this.fpSpread.Sheets[k].Rows.Count; i++) //����ҽ��-��ʱҽ��
                {
                    this.fpSpread.BackColor = System.Drawing.Color.Azure;
                    try
                    {
                        FarPoint.Win.Spread.SheetView sv = this.fpSpread.Sheets[k].GetChildView(i, 0);
                        if (sv != null)
                        {
                            #region add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                            //sv.Columns[7].Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
                            //sv.Columns[8].Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
                            sv.Columns[8].Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
                            sv.Columns[9].Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold); 
                            #endregion
                            for (int j = 0; j < sv.Rows.Count; j++)
                            {//ҽ����Ŀ
                                #region add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                                //string note = sv.Cells[j, 22].Text;//��ע
                                string note = sv.Cells[j, 23].Text;//��ע
                                //if (sv.Cells.Get(j, 23).Text == "ֹͣ/ȡ��") 
                                if (sv.Cells.Get(j, 24).Text == "ֹͣ/ȡ��") sv.Rows[j].BackColor = Color.FromArgb(255, 222, 222);//ҽ��״̬��ҽ���������
                                sv.SetNote(j, 4, note);
                                if ((bool)sv.Cells[j, 15].Value)
                                //if ((bool)sv.Cells[j, 13].Value)
                                {
                                    sv.Rows[j].Label = "��";
                                    sv.RowHeader.Rows[j].BackColor = System.Drawing.Color.Pink;
                                }
                                int hypotest = 0;
                                //if (sv.Cells[j, 24].Text == "����")
                                if (sv.Cells[j, 25].Text == "����")
                                {
                                    hypotest = 3;
                                }
                                //else if (sv.Cells[j, 24].Text == "����")
                                else if (sv.Cells[j, 25].Text == "����")
                                {
                                    hypotest = 4;
                                } 
                                #endregion
                                //int hypotest = Neusoft.FrameWork.Function.NConvert.ToInt32(sv.Cells[j, 24].Text);//Ƥ��
                                string sTip = "�費��Ƥ��";//Function.TipHypotest;
                                if (sv.Cells[j, 4].Text.Length > 3)
                                {
                                    if ((sv.Cells[j, 4].Text.Substring(sv.Cells[j, 4].Text.Length - 3) == "�ۣ���"
                                    || sv.Cells[j, 4].Text.Substring(sv.Cells[j, 4].Text.Length - 3) == "�ۣ���"))
                                    {
                                        sv.Cells[j, 4].Text = sv.Cells[j, 4].Text.Substring(0, sv.Cells[j, 4].Text.Length - 3);
                                    }
                                }
                                try
                                {
                                    if (sv.Cells[j, 4].Text.Length > 3)
                                        if (sv.Cells[j, 4].Text.Substring(sv.Cells[j, 4].Text.Length - sTip.Length, sTip.Length) == sTip)
                                            sv.Cells[j, 4].Text = sv.Cells[j, 4].Text.Substring(0, sv.Cells[j, 4].Text.Length - sTip.Length);
                                }
                                catch { }
                                sv.Cells[j, 4].ForeColor = Color.Black;
                                if (hypotest == 3)
                                {
                                    sv.Cells[j, 4].Text += "�ۣ���";//Ƥ��
                                    sv.Cells[j, 4].ForeColor = Color.Red;
                                }
                                else if (hypotest == 4)
                                {
                                    sv.Cells[j, 4].Text += "�ۣ���";
                                }
                                else if (hypotest == 2)
                                {
                                }

                                //��ʾ˳���
                                if (sv.RowHeader.Cells[j, 0].Text != "��")
                                    //sv.RowHeader.Cells[j, 0].Text = sv.Cells[j, 21].Text;
                                    sv.RowHeader.Cells[j, 0].Text = sv.Cells[j, 22].Text;//add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("ˢ��ҽ����ע��Ϣ����", "Sorry");
                    }
                }
            }
        }
        
        #endregion

        #region ��̫̫
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            tv = sender as TreeView;
            if (tv != null && tv.CheckBoxes == false)
                tv.CheckBoxes = true;
            this.InitControl();

            return null;
        }
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            if (tv != null && this.tv.CheckBoxes == false)
            {
                tv.CheckBoxes = true;
            }
            #region {07650785-3A5B-4ecf-AFC4-1FD7E6366906} ѡ�л��߼���ʾ����ҽ��by guanyx
            if (e != null && e.Tag.ToString() != "In" )
            {
                ArrayList patientList = new ArrayList();
                patientList.Add((Neusoft.HISFC.Models.RADT.PatientInfo)e.Tag);
                this.SetValues(patientList, e);
                this.QueryOrder();
            }
            #endregion
            return base.OnSetValue(neuObject, e);
        }
        /// <summary>
        /// ���߸�������
        /// </summary>
        /// <param name="alValues"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override int OnSetValues(ArrayList alValues, object e)
        {
            this.alpatientinfos = alValues;
            this.QueryOrder();
            #region {839D3A8A-49FA-4d47-A022-6196EB1A5715}
            if (this.tv != null && this.tv.CheckBoxes)
            {
                foreach (TreeNode parentNode in tv.Nodes)
                {
                    foreach (TreeNode node in parentNode.Nodes)
                    {
                        if (node.Tag is Neusoft.HISFC.Models.RADT.PatientInfo)
                        {
                            Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
                            if (node.Checked)
                            {
                                switch (patientInfo.Sex.ID.ToString())
                                {
                                    case "F":
                                        //��
                                        if (patientInfo.ID.IndexOf("B") > 0)
                                            node.ImageIndex = 10;	//Ӥ��Ů
                                        else
                                            node.ImageIndex = 6;	//����Ů
                                        break;
                                    case "M":
                                        if (patientInfo.ID.IndexOf("B") > 0)
                                            node.ImageIndex = 8;	//Ӥ����
                                        else
                                            node.ImageIndex = 4;	//������
                                        break;
                                    default:
                                        node.ImageIndex = 4;
                                        break;
                                }
                                Neusoft.HISFC.Components.Common.Classes.Function.DelLabel((node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo).ID);
                            }
                        }
                    }
                }
            }
            #endregion
            return 0;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnSave(object sender, object neuObject)
        {
            if (Neusoft.FrameWork.WinForms.Classes.Function.Msg("�Ƿ�ȷ��Ҫ����?", 422) == DialogResult.No)
            {
                return -1;
            }
            this.fpSpread.StopCellEditing();//{7B757642-336B-4384-8DE4-9DFE4E4DCD1F}����ֹͣ�༭
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            this.orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            
            Neusoft.HISFC.Models.Base.Employee empl = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
            for (int i = 0; i < this.fpSpread.Sheets[0].Rows.Count; i++) //����ҽ��
            {
                #region ҽ������
                string strInpatientNo = this.fpSpread.Sheets[0].Cells[i, 0].Text;//��ǰ�Ļ���
                string strName = this.fpSpread.Sheets[0].Cells[i, 2].Text;//��ǰ�Ļ���

                string strComboNo = "";
                //��ǰ���ߵ�ҽ���б�ҳ sv
                FarPoint.Win.Spread.SheetView sv = this.fpSpread.Sheets[0].GetChildView(i, 0);
                ArrayList alOrders = new ArrayList();
                Neusoft.HISFC.Models.RADT.PatientInfo p = radtIntegrate.GetPatientInfomation(strInpatientNo);
                if (sv != null)
                {
                    for (int j = 0; j < sv.Rows.Count; j++)//ҽ����Ŀ
                    {
                        if (sv.Cells[j, 3].Text.ToUpper() == "TRUE")
                        {
                            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڴ�����ҽ��...");
                            Application.DoEvents();
                            string orderid = sv.Cells[j, int.Parse(OrderId.ID)].Text;//ҽ����Ŀ����
                            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.orderManager.QueryOneOrder(orderid);                                                                                 
                            if (order == null)
                            {
                                orderIntegrate.fee.Rollback();
                                //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show("ҽ���Ѿ������仯����ˢ����Ļ��");
                                return -1;
                            }

                            order.Patient.Name = strName;

                            alOrders.Add(order);
                        }

                    }
                    if (orderIntegrate.SaveChecked(p, alOrders, true, empl.Nurse.ID) == -1)
                    {
                        orderIntegrate.fee.Rollback();
                       // Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show(this.orderIntegrate.Err);
                        return -1;
                    }
                    else
                    {
                        orderIntegrate.fee.Commit();
                        //Neusoft.FrameWork.Management.Transaction 
                        Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        this.orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        this.orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        this.radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    }
                }
                #endregion
            }
            #region {2D97BF3B-C09C-433d-9C8C-F80CC2751261}
            DateTime beginDate = this.orderManager.GetDateTimeFromSysDateTime();
            DateTime endDate = beginDate;
            string paramRecipeNo = "";
            string paramOrderNo = "";
            string deptCode=((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;
            #endregion

            #region donggq-20101109-���뵥����-{755769B0-C65F-4eb4-A6BA-80F0E4843B32}

            Neusoft.HISFC.BizProcess.Integrate.Manager mgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            ArrayList alApplys = mgr.GetConstantList("CheckPrintApply");
            ArrayList alExecs = new ArrayList();
            if (alApplys != null && alApplys.Count > 0)
            {
                for (int i = 0; i < alApplys.Count; i++)
                {
                    Neusoft.FrameWork.Models.NeuObject tmp = alApplys[i] as Neusoft.FrameWork.Models.NeuObject;
                    alExecs.Add(tmp.Memo);
                }
            }

            #endregion

            for (int i = 0; i < this.fpSpread.Sheets[1].Rows.Count; i++) //��ʱҽ��
            {
                #region ҽ������
                string strInpatientNo = this.fpSpread.Sheets[1].Cells[i, 0].Text;//��ǰ�Ļ���
                string strName = this.fpSpread.Sheets[1].Cells[i, 2].Text;//��ǰ�Ļ���

                string strComboNo = "";
                //��ǰ���ߵ�ҽ���б�ҳ sv
                FarPoint.Win.Spread.SheetView sv = this.fpSpread.Sheets[1].GetChildView(i, 0);
                ArrayList alOrders = new ArrayList();
                Neusoft.HISFC.Models.RADT.PatientInfo p = radtIntegrate.GetPatientInfomation(strInpatientNo);
                if (sv != null)
                {
                    for (int j = 0; j < sv.Rows.Count; j++)//ҽ����Ŀ
                    {
                        if (sv.Cells[j, 3].Text.ToUpper() == "TRUE")
                        {
                            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڴ�����ʱҽ��...");
                            Application.DoEvents();
                            string orderid = sv.Cells[j, int.Parse(OrderId.ID)].Text;//ҽ����Ŀ����
                            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.orderManager.QueryOneOrder(orderid);                                                       
                            if (order == null)
                            {
                                orderIntegrate.fee.Rollback();
                                //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show("ҽ���Ѿ������仯����ˢ����Ļ��");
                                return -1;
                            }

                            order.Patient.Name = strName;

                            alOrders.Add(order);

                            if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.UnDrug)
                            {
                                #region addby xuewj 2010-10-5 ����ҽ��ȡ����������Ϣ {46F66712-91CF-42bf-BB95-BE6782764AAC}
                                if (order.Item.ID != "999")//����ҽ����������ȡ��Ϣ
                                {
                                    Neusoft.HISFC.Models.Fee.Item.Undrug undrugInfo = this.feeIntegrate.GetItem(order.Item.ID);
                                    if (undrugInfo == null || undrugInfo.ID == "")
                                    {
                                        orderIntegrate.fee.Rollback();
                                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                        MessageBox.Show("��ѯ��ҩƷ" + order.Item.Name + "��Ϣʧ��!" + this.feeIntegrate.Err);
                                        return -1; ;
                                    }

                                    if (undrugInfo.IsNeedConfirm && !paramOrderNo.Contains(order.ID)
                                        && order.ExeDept.ID != order.Patient.PVisit.PatientLocation.Dept.ID
                                        && order.Status == 0)
                                    {
                                        #region donggq-20101109-���뵥����-{755769B0-C65F-4eb4-A6BA-80F0E4843B32}

                                        if (!alExecs.Contains(order.ExeDept.ID))
                                        {
                                            paramOrderNo = "'" + order.ID + "'," + paramOrderNo;
                                        }

                                        #endregion
                                    }
                                } 
                                #endregion
                            }
                        }

                    }
                    orderIntegrate.MessageType = messType;
                    //{2D97BF3B-C09C-433d-9C8C-F80CC2751261}
                    //if (orderIntegrate.SaveChecked(p, alOrders, false, empl.Nurse.ID) == -1)
                    string tempParamRecipeNo="";
                    if (orderIntegrate.SaveCheckedForShort(p, alOrders, false, empl.Nurse.ID, ref tempParamRecipeNo) == -1)
                    {
                        //orderIntegrate.fee.Rollback();
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show(this.orderIntegrate.Err);
                        return -1;
                    }
                    else
                    {
                        orderIntegrate.fee.Commit();
                        //Neusoft.FrameWork.Management.Transaction 
                        Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        this.orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        this.orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        this.radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    }

                    paramRecipeNo += tempParamRecipeNo;//{2D97BF3B-C09C-433d-9C8C-F80CC2751261}

                    #region addby xuewj 2010-03-12 HL7��Ϣ send��op---receiver��of

                    if (this.iop == null)
                    {
                        this.iop = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.IHE.IOP)) as Neusoft.HISFC.BizProcess.Interface.IHE.IOP;
                    }
                    if(this.iop!=null)
                    {
                        this.iop.PlaceOrder(alOrders);
                    }

                    #endregion
                }
                #endregion

                
            }
            endDate = this.orderManager.GetDateTimeFromSysDateTime(); //{2D97BF3B-C09C-433d-9C8C-F80CC2751261}
            orderIntegrate.fee.Commit();

            #region ���ò������˵� {2D97BF3B-C09C-433d-9C8C-F80CC2751261}
            if (paramRecipeNo != "")
            {
                MessageBox.Show("������ӡ�������˵�����ȷ�ϴ�ӡ���Ѿ�λ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paramRecipeNo = paramRecipeNo.Substring(0, paramRecipeNo.Length - 1);//ȥ������Ķ���

                if (this.nurseFeeBill == null)
                {
                    this.nurseFeeBill = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet)) as Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet;
                }
                if (this.nurseFeeBill != null)
                {
                    this.nurseFeeBill.NurseFeeBill(beginDate, endDate, paramRecipeNo);
                }
            }
            #endregion 
            #region ���ò����������뵥 {2D97BF3B-C09C-433d-9C8C-F80CC2751261}
            if (paramOrderNo != "")
            {
                MessageBox.Show("������ӡ�����������뵥����ȷ�ϴ�ӡ���Ѿ�λ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paramOrderNo = paramOrderNo.Substring(0, paramOrderNo.Length - 1);//ȥ������Ķ���

                if (this.nurseApplyFeeBill == null)
                {
                    this.nurseApplyFeeBill = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Order.IApplyFeeSheet)) as Neusoft.HISFC.BizProcess.Interface.Order.IApplyFeeSheet;
                }
                if (this.nurseApplyFeeBill != null)
                {
                    this.nurseApplyFeeBill.NurseFeeBill(paramOrderNo);
                }
            }
            #endregion 
            //Neusoft.FrameWork.Management.PublicTrans.Commit();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            this.QueryOrder();
            return 0;
        }
        #endregion

        #region �¼�
        void sv_CellChanged(object sender, SheetViewEventArgs e)
        {

        }

        /// <summary>
        /// �ú���û��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fpSpread_ChildViewCreated(object sender, FarPoint.Win.Spread.ChildViewCreatedEventArgs e)
        {
            this.SetChildViewStyle(e.SheetView);
        }

        void fpSpread_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
        {
            #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
            this.ChangeBtnVisble(e.SheetTabIndex); 
            #endregion
        }

        void fpSpread_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        void fpSpread_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.Row < 0)
                return;

            //�жϵ�ǰ��ͣ�������Ƿ�����ʾ ��δ��ʾ ����ʾͣ������
            try
            {
                if (e.View.Sheets[0].Columns[2].Label == "��Ϻ�") //��childtable1
                {
                    if (this.content != null && this.content.Visible == false)
                    {
                        if (wc == null && this.dockingManager != null)
                        {
                            wc = this.dockingManager.AddContentWithState(content, Crownwood.Magic.Docking.State.DockRight);
                            this.dockingManager.AddContentToWindowContent(content, wc);
                        }
                        if (this.dockingManager != null)
                            this.dockingManager.ShowContent(this.content);
                    }
                    if (this.ucSubtblManager1 != null && !e.RowHeader && !e.ColumnHeader)		//������б������б���
                    {
                        ucSubtblManager1.OrderID = this.OrderId.Name;
                        ucSubtblManager1.ComboNo = this.ComboNo.Name;
                        this.CurrentCellName = e.View.Sheets[0].Cells[e.View.Sheets[0].ActiveRowIndex, 4];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        FarPoint.Win.Spread.CellClickEventArgs cellClickEvent = null;
        int curRow = 0;
        void fpSpread_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0) return;
            if (e.Column > 0)
            {
                try
                {
                    int active = this.fpSpread.ActiveSheetIndex;   
                    if (e.View.Sheets.Count <= active) active = 0;
                    curRow = active;
                    if (e.View.Sheets[active].Columns[2].Label == "��Ϻ�") //�ӱ�1
                    {
                        if (e.Button == MouseButtons.Left) //���
                        {
                            this.OrderId.Name = e.View.Sheets[active].Cells[e.Row, int.Parse(this.OrderId.ID)].Text;
                            this.ComboNo.Name = e.View.Sheets[active].Cells[e.Row, int.Parse(this.ComboNo.ID)].Text;
                            this.PatientId = e.View.Sheets[active].Cells[e.Row, 0].Text;//סԺ��ˮ��

                            if (e.View.Sheets[active].Cells[e.Row, 3].Text.ToUpper() == "TRUE")
                            {
                                e.View.Sheets[active].Cells[e.Row, 3].Text = "False";
                                e.View.Sheets[active].Cells[e.Row, 3].BackColor = Color.White;
                            }
                            else
                            {
                                e.View.Sheets[active].Cells[e.Row, 3].Text = "True";
                                e.View.Sheets[active].Cells[e.Row, 3].BackColor = Color.Blue;
                            }
                            //������ϵ�ҽ��ѡ����Ϣ
                            for (int i = 0; i < e.View.Sheets[active].RowCount; i++)
                            {
                                if (e.View.Sheets[active].Cells[i, int.Parse(this.ComboNo.ID)].Text == this.ComboNo.Name 
                                    && i != e.Row)
                                {
                                    e.View.Sheets[active].Cells[i, 3].Text = e.View.Sheets[active].Cells[e.Row, 3].Text;
                                    e.View.Sheets[active].Cells[i, 3].BackColor = e.View.Sheets[active].Cells[e.Row, 3].BackColor;
                                }
                            }
                        }
                        else//�Ҽ�
                        {
                            this.OrderId.Name = e.View.Sheets[active].Cells[e.Row, int.Parse(this.OrderId.ID)].Text;
                            string strItemName = e.View.Sheets[active].Cells[e.Row, 5].Text;
                            this.PatientId = e.View.Sheets[active].Cells[e.Row, 0].Text;//סԺ��ˮ�� 
                            cellClickEvent = e;
                            ContextMenu menu = new ContextMenu();
                            MenuItem mnuTip = new MenuItem("��ע");//��ע
                            mnuTip.Click += new EventHandler(mnuTip_Click);

                            MenuItem mnuChangeDept = new MenuItem("�޸�ȡҩ����");//�޸�ȡҩ����
                            mnuChangeDept.Click+=new EventHandler(mnuChangeDept_Click);
                            
                            menu.MenuItems.Add(mnuTip);
                            menu.MenuItems.Add(mnuChangeDept);
                            this.fpSpread.ContextMenu = menu;
                            //Function.PopMenu(menu, obj.Item.ID, false);

                            
                            //ContextMenu menu = new ContextMenu();
                            //MenuItem mnuTip = new MenuItem("��ע");//��ע
                            //MenuItem mnuExecTime = new MenuItem("ִ��ʱ��");
                            
                        }
                    }
                    else if (e.View.Sheets[active].Columns[2].Label == "��������")//����
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if (e.View.Sheets[active].Cells[e.Row, 1].Text.ToUpper() == "TRUE")
                            {
                                e.View.Sheets[active].Cells[e.Row, 1].Text = "false";
                             
                            }
                            else
                            {
                                e.View.Sheets[active].Cells[e.Row, 1].Text = "True";
                             
                            }
                            //�����ӱ��ѡ��
                            try
                            {
                                List<string> alComboNO = new List<string>();//addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
                                FarPoint.Win.Spread.SheetView sv = e.View.Sheets[active].GetChildView(e.Row, 0);//(FarPoint.Win.Spread.SpreadView).GetChildWorkbooks()[e.Row];                                
                                if (sv.Columns[3].Label == "���")
                                {
                                    for (int i = 0; i < sv.Rows.Count; i++)
                                    {
                                        #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
                                        if (sv.Rows[i].Visible)
                                        {
                                            sv.Cells[i, 3].Text = e.View.Sheets[active].Cells[e.Row, 1].Text;
                                            if (!alComboNO.Contains(sv.Cells[i, int.Parse(this.ComboNo.ID)].Text))
                                            {
                                                alComboNO.Add(sv.Cells[i, int.Parse(this.ComboNo.ID)].Text);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8}
                                //������ϵ�ҽ��ѡ����Ϣ
                                for (int i = 0; i < sv.RowCount; i++)
                                {
                                    if (Neusoft.FrameWork.Function.NConvert.ToBoolean(sv.Cells[i, 3].Value) == !Neusoft.FrameWork.Function.NConvert.ToBoolean(e.View.Sheets[active].Cells[e.Row, 1].Text)
                                        && alComboNO.Contains(sv.Cells[i, int.Parse(this.ComboNo.ID)].Text))
                                    {
                                        sv.Cells[i, 3].Text = e.View.Sheets[active].Cells[e.Row, 1].Text;
                                    }
                                }
                                #endregion
                            }
                            catch { }
                            this.OrderId.Name = "";
                            this.ComboNo.Name = "";
                        }

                    }
                }
                catch { }
            }
        }
        //��ע����
        private void mnuTip_Click(object sender, EventArgs e)
        {
            ucTip ucTip1 = new ucTip(); 
            string OrderID = this.OrderId.Name;
            int iHypotest = this.orderManager.QueryOrderHypotest(OrderID);
            if (iHypotest == -1)
            {
                MessageBox.Show(this.orderManager.Err);
                return;
            }
            #region ��ҩƷҽ������ʾƤ��ҳ 
            Neusoft.HISFC.Models.Order.Order o = this.orderManager.QueryOneOrder(this.OrderId.Name);
            //if (o.Item.IsPharmacy == false)
            if (o.Item.ItemType != Neusoft.HISFC.Models.Base.EnumItemType.Drug)
            {
                ucTip1.Hypotest = 1;
            }
            #endregion
            ucTip1.Tip = this.orderManager.QueryOrderNote(OrderID);
            ucTip1.Hypotest = iHypotest;
            ucTip1.OKEvent += new myTipEvent(ucTip1_OKEvent);
            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucTip1);
        }

        /// <summary>
        /// �޸�ִ�п����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangeDept_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.orderManager.QueryOneOrder(this.OrderId.Name);
            Neusoft.FrameWork.Models.NeuObject dept = ucChangeStoreDept.ChangeStoreDept(order);
            if (dept == null) return;
            order.StockDept = dept;
            if (this.orderManager.UpdateOrder(order) < 0)
            {
                MessageBox.Show(this.orderManager.Err);
                return;
            }
        }

        //��ע�¼�
        private void ucTip1_OKEvent(string Tip, int Hypotest)
        {
            if (this.orderManager.UpdateFeedback(this.PatientId, this.OrderId.Name, Tip, Hypotest) == -1)
            {
                MessageBox.Show(this.orderManager.Err);
                this.orderManager.Err = "";
                return;
            }
             
            //SheetView sv=  this.fpSpread.ActiveSheet.GetChildView(this.fpSpread.ActiveSheet.ActiveRowIndex, 0);
            #region add by xuewj 2010-9-26 ������ ���� {5C90E8AB-70B8-45b9-ABEE-577048C0FE43}
            if (Hypotest == 3)
            {
                //cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 24].Text = "����";
                cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 25].Text = "����";
            }
            else if (Hypotest == 4)
            {
                //cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 24].Text = "����";
                cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 25].Text = "����";
            }

            //cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 22].Text = Tip;
            cellClickEvent.View.Sheets[curRow].Cells[cellClickEvent.Row, 23].Text = Tip; 
            #endregion
            Neusoft.HISFC.Models.RADT.PatientInfo p = this.radtIntegrate.GetPatientInfoByPatientNO(this.PatientId);
            refreshView();
     
        }
        void ucOrderConfirm_CellChanged(object sender, FarPoint.Win.Spread.SheetViewEventArgs e)
        {
         
        }
        #endregion

        #region �������ڴ�����
        public Crownwood.Magic.Docking.DockingManager dockingManager;
        private Crownwood.Magic.Docking.Content content;
        private Crownwood.Magic.Docking.WindowContent wc;
        ucSubtblManager ucSubtblManager1 = null;
        public void DockingManager()
        {
            this.dockingManager = new Crownwood.Magic.Docking.DockingManager
                (this, Crownwood.Magic.Common.VisualStyle.IDE);
            this.dockingManager.OuterControl = this.panelMain;		//��OuterControl�����Ŀؼ�����ͣ�����ڵ�Ӱ��

            content = new Crownwood.Magic.Docking.Content(this.dockingManager);
            content.Control = ucSubtblManager1;

            Size ucSize = content.Control.Size;

            content.Title = "���Ĺ���";
            content.FullTitle = "���Ĺ���";
            content.AutoHideSize = ucSize;
            content.DisplaySize = ucSize;
            
           
            this.dockingManager.Contents.Add(content);
        }
        #endregion

        #region IInterfaceContainer ��Ա {2D97BF3B-C09C-433d-9C8C-F80CC2751261}

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] t = new Type[1];
                t[0] = typeof(Neusoft.HISFC.BizProcess.Interface.Order.IFeeSheet);
                t[1] = typeof(Neusoft.HISFC.BizProcess.Interface.Order.IApplyFeeSheet);
                return t;
            }
        }

        #endregion

        #region addby xuewj 2010-10-1 ҽ���������ϵͳ������ {93CA36C4-ABF1-459d-A94A-0AD81F0804C8} 
        /// <summary>
        /// ��ҩ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnP_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnP)
            {
                if (this.FilteOrder("P") == 1)
                {
                    ChangeBtnColor(this.btnP);
                }
            }
        }

        /// <summary>
        /// �г�ҩ���в�ҩ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPCCANDPCZ_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnPCCANDPCZ)
            {
                if (this.FilteOrder("PCC,PCZ") == 1)
                {
                    ChangeBtnColor(this.btnPCCANDPCZ);
                }
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUC_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnUC)
            {
                if (this.FilteOrder("UC") == 1)
                {
                    ChangeBtnColor(this.btnUC);
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUL_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnUL)
            {
                if (this.FilteOrder("UL") == 1)
                {
                    ChangeBtnColor(this.btnUL);
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOther_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnOther)
            {
                if (this.FilteOrder("OTHER") == 1)
                {
                    ChangeBtnColor(this.btnOther);
                }
            }
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e)
        {
            if (this.activeBtn != this.btnAll)
            {
                if (this.FilteOrder("All") == 1)
                {
                    ChangeBtnColor(this.btnAll);
                }
            }
        }

        /// <summary>
        /// ��ť��С����Ӧ
        /// </summary>
        private void ChangeBtnSize()
        {
            bool existOrder = false;
            if (this.fpSpread.Sheets[1].RowCount == 0)
            {
                existOrder = false;
            }
            else
            {
                for (int i = 0; i < this.fpSpread.Sheets[1].RowCount; i++)
                {
                    if (this.fpSpread.Sheets[1].Rows[i].Visible == true)
                    {
                        existOrder = true;
                        break;
                    }
                }
            }
            foreach (Control c in this.panelMain.Controls)
            {
                if (c is Neusoft.FrameWork.WinForms.Controls.NeuButton)
                {
                    if (existOrder)
                    {
                        c.Height = 42;
                    }
                    else
                    {
                        c.Height = 22;
                    }
                }
            }
        }

        /// <summary>
        /// ���ݵ�ǰsheetҳ�������˰�ť�Ƿ���ʾ
        /// </summary>
        /// <param name="sheetView"></param>
        private void ChangeBtnVisble(int sheetIndex)
        {
            if (sheetIndex == 0)
            {
                foreach (Control c in this.panelMain.Controls)
                {
                    if (c is Neusoft.FrameWork.WinForms.Controls.NeuButton)
                    {
                        c.Visible = false;
                    }
                }
            }
            else if (sheetIndex == 1)
            {
                foreach (Control c in this.panelMain.Controls)
                {
                    if (c is Neusoft.FrameWork.WinForms.Controls.NeuButton)
                    {
                        c.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// �ı䱳��ɫ
        /// </summary>
        private void ChangeBtnColor(Control control)
        {
            if (this.activeBtn == null)
            {
                this.activeBtn = control;
                this.activeBtn.BackColor = this.backColor;
                this.activeBtn.ForeColor = this.foreColor;
            }
            else 
            {
                if (this.activeBtn.Name != control.Name)
                {
                    control.BackColor = this.backColor;
                    control.ForeColor = this.foreColor;
                    this.activeBtn.BackColor = Color.White;
                    this.activeBtn.ForeColor = Color.Black;
                    this.activeBtn = control;
                }
            }
        }

        /// <summary>
        /// ����ϵͳ�����˴����ҽ��   PS:ÿ�ζ���Ҫ��ѯҽ����Ϣ���������Ż�
        /// </summary>
        /// <param name="sysClass"></param>
        private int FilteOrder(string sysClass)
        {
            if (this.fpSpread.Sheets[1].RowCount == 0)
            {
                MessageBox.Show("û����Ҫ���˵���ʱҽ��!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return -1;
            }
            int perPatientRowCount = 0;//���˺�ǰ������ʾ��ҽ������
            string[] sysClasses = sysClass.Split(',');
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾҽ��,�����Ե�(*^__^*)...");
            Application.DoEvents();
            for (int i = 0; i < this.fpSpread.Sheets[1].Rows.Count; i++) //��ʱҽ��
            {
                #region ҽ������

                //��ǰ���ߵ�ҽ���б�ҳ sv
                FarPoint.Win.Spread.SheetView sv = this.fpSpread.Sheets[1].GetChildView(i, 0);               
                if (sv != null)
                {
                    perPatientRowCount = 0;
                    for (int j = 0; j < sv.Rows.Count; j++)//ҽ����Ŀ
                    {
                        if (sysClass == "All")
                        {
                            sv.Rows[j].Visible = true;
                        }
                        else
                        {
                            string orderid = sv.Cells[j, int.Parse(OrderId.ID)].Text;//ҽ����Ŀ����
                            Neusoft.HISFC.Models.Order.Inpatient.Order order = this.orderManager.QueryOneOrder(orderid);
                            if (order == null)
                            {
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show("ҽ���Ѿ������仯����ˢ����Ļ��");
                                return -1;
                            }

                            bool isInclude=false;//�Ƿ������ʾ����
                            foreach (string s in sysClasses)
                            {
                                if (s != "")
                                {
                                    if (s == order.Item.SysClass.ID.ToString())//������ʾ����
                                    {
                                        isInclude = true;
                                        break;
                                    }
                                }
                            }

                            if (isInclude)
                            {
                                sv.Rows[j].Visible = true;
                                perPatientRowCount++;
                            }
                            else
                            {
                                if (sysClass == "OTHER")
                                {
                                    //�ų�ҩƷ���в�ҩ���г�ҩ����顢���飨������Ҫ�ɸĳ�����ģʽ��
                                    string sysClassTmp = order.Item.SysClass.ID.ToString();
                                    if (sysClassTmp != "P" && sysClassTmp != "PCC" && sysClassTmp != "PCZ"
                                        && sysClassTmp != "UL" && sysClassTmp != "UC")
                                    {
                                        sv.Rows[j].Visible = true;
                                        perPatientRowCount++;
                                    }
                                    else
                                    {
                                        sv.Rows[j].Visible = false;
                                    }
                                }
                                else
                                {
                                    sv.Rows[j].Visible = false;
                                }
                            }
                        }
                    }

                    //��ǰ���߲����ڷ���������ҽ����������
                    if (perPatientRowCount == 0&&sysClass!="All")
                    {
                        this.fpSpread.Sheets[1].Rows[i].Visible = false;
                    }
                    else
                    {
                        this.fpSpread.Sheets[1].Rows[i].Visible = true;
                        this.fpSpread.Sheets[1].ExpandRow(i, false);
                        this.fpSpread.Sheets[1].ExpandRow(i, true);
                    }
                }
                #endregion
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            this.ChangeBtnSize();
            Classes.Function.DrawCombo(this.fpSpread.Sheets[1], 2, 5, 1);
            return 1;
        }

        /// <summary>
        /// ��ʼ�����Ʋ���
        /// </summary>
        private void Initcontrolargument()
        {
            //����ɫ
            string returnValue = ctlMgr.QueryControlerInfo("200213");

            if (!string.IsNullOrEmpty(returnValue))
            {
                this.backColor = System.Drawing.Color.FromArgb(Neusoft.FrameWork.Function.NConvert.ToInt32(returnValue));
            }

            //ǰ��ɫ
            returnValue = ctlMgr.QueryControlerInfo("200214");

            if (!string.IsNullOrEmpty(returnValue))
            {
                this.foreColor = System.Drawing.Color.FromArgb(Neusoft.FrameWork.Function.NConvert.ToInt32(returnValue));
            }
        }

        #endregion
    }
}
