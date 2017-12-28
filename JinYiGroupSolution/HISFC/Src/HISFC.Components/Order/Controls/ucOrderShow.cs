using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ����ѯ�ؼ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2007-1-17]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucOrderShow : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucOrderShow()
        {
            InitializeComponent();
        }

        Neusoft.HISFC.BizProcess.Integrate.Manager deptManagement = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        Neusoft.HISFC.BizLogic.Order.Order orderManagement = new Neusoft.HISFC.BizLogic.Order.Order();
        Neusoft.FrameWork.Public.ObjectHelper deptHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        string orderId = "";
        protected DataSet dsAllLong;
        protected DataSet dsAllShort;
        private DataSet dataSet = null;							//��ǰDataSet
        private DataView dvLong = null;							//��ǰDataView
        private DataView dvShort = null;						//��ǰDataView
        private string LONGSETTINGFILENAME = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath +
            Neusoft.FrameWork.WinForms.Classes.Function.SettingPath+ "LongOrderQuerySetting.xml";
        private string SHORTSETTINGFILENAME = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath +
            Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "ShortOrderQuerySetting.xml";

        private int sheetIndex = 0;			//��ǰ�Sheetҳ����
        ucSubtblManager ucSubtblManager1 = null;//����ά��
        /// <summary>
        /// Ƥ��ҩ��ע
        /// {17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
        /// </summary>
        ucTip ucTip = null;
        float[] longColumnWidth;
        float[] shortColumnWidth;
        ArrayList alQueryLong = new ArrayList();
        ArrayList alQueryShort = new ArrayList();

        private void ucOrderShow_Load(object sender, System.EventArgs e)
        {
            

        }


        #region ����
        Neusoft.HISFC.Models.RADT.PatientInfo myPatientInfo = null;
        /// <summary>
        /// ���߻�����Ϣ
        /// </summary>
        protected Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
        {
            get
            {
                if (this.myPatientInfo == null)
                    this.myPatientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
                return this.myPatientInfo;
            }
            set
            {
                this.myPatientInfo = value;
            
                this.QueryOrder();
            
            }
        }

        Neusoft.HISFC.Models.Base.Employee oper = null;
        /// <summary>
        /// ����Ա��Ϣ
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Neusoft.HISFC.Models.Base.Employee Oper
        {
            get
            {
                if (oper == null)
                    oper = Neusoft.FrameWork.Management.Connection.Operator as  Neusoft.HISFC.Models.Base.Employee;
                return oper;
            }
            set
            {
                this.oper = value;
            }
        }
        /// <summary>
        /// �Ƿ���ʾ���˹���
        /// </summary>
        public bool IsShowFilter
        {
            set
            {
                this.cmbOderStatus.Visible = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾֹͣ/����ҽ��
        /// </summary>
        private bool showDCOrder = true;
        /// <summary>
        /// �Ƿ���ʾֹͣ/����ҽ��
        /// </summary>
        public bool IsShowDCOrder
        {
            set
            {
                this.showDCOrder = value;
            }
        }

        /// <summary>
        /// �Ƿ������������
        /// </summary>
        protected bool enableSubtbl = true;
        /// <summary>
        /// �Ƿ������������ �����ڻ�ʿվ�ۺ��շ�ʱ��ѯҽ��ʱ�������������
        /// </summary>
        public bool IsEnabledSubtbl
        {
            set
            {
                this.enableSubtbl = value;
            }
        }
        
        #endregion

        #region ��ʼ��
        private void InitFP()
        {

            SetColumnProperty();
        }

        private void SetColumnProperty()
        {
            if (System.IO.File.Exists(LONGSETTINGFILENAME))
            {
                if (this.longColumnWidth == null || this.shortColumnWidth == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpSpread1.Sheets[1], SHORTSETTINGFILENAME);
                    this.longColumnWidth = new float[this.fpSpread1.Sheets[0].Columns.Count];
                    for (int i = 0; i < this.fpSpread1.Sheets[0].Columns.Count; i++)
                    {
                        this.longColumnWidth[i] = this.fpSpread1.Sheets[0].Columns[i].Width;
                    }
                    this.shortColumnWidth = new float[this.fpSpread1.Sheets[1].Columns.Count];
                    for (int i = 0; i < this.fpSpread1.Sheets[1].Columns.Count; i++)
                    {
                        this.shortColumnWidth[i] = this.fpSpread1.Sheets[1].Columns[i].Width;
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < this.fpSpread1.Sheets[0].Columns.Count; i++)
                            this.fpSpread1.Sheets[0].Columns[i].Width = this.longColumnWidth[i];
                        for (int i = 0; i < this.fpSpread1.Sheets[1].Columns.Count; i++)
                            this.fpSpread1.Sheets[1].Columns[i].Width = this.shortColumnWidth[i];
                    }
                    catch
                    { }
                }
            }
            else
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[1], SHORTSETTINGFILENAME);
            }

            this.fpSpread1.Sheets[0].Columns[0].Visible = false;//����ע����ʾ
            this.fpSpread1.Sheets[1].Columns[0].Visible = false;//����ע����ʾ

            this.fpSpread1.Sheets[0].Columns[21].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSpread1.Sheets[0].Columns[20].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSpread1.Sheets[1].Columns[21].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSpread1.Sheets[1].Columns[20].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            Neusoft.HISFC.BizProcess.Integrate.Manager managerMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            ArrayList list = managerMgr.GetDepartment();
            if (list == null)
            {
                MessageBox.Show("��ȡ������Ϣʧ��" + managerMgr.Err);
                list = new ArrayList();
            }

            this.deptHelper.ArrayObject = list;
        }

        private FarPoint.Win.Spread.CellType.DateTimeCellType dateCellType = new FarPoint.Win.Spread.CellType.DateTimeCellType();

        
        private DataSet InitDataSet()
        {
            try
            {
                dataSet = new DataSet();
                Type dtStr = System.Type.GetType("System.String");
                Type dtDbl = typeof(System.Double);
                Type dtInt = typeof(System.Int32);
                Type dtBoolean = typeof(System.Boolean);
                Type dtDate = typeof(System.DateTime);

                DataTable table = new DataTable("Table");

                table.Columns.AddRange(new DataColumn[]
				{
					new DataColumn("!",dtStr),						//0
					new DataColumn("˳���",dtInt),					//35  by zlw 2006-4-18
					new DataColumn("����",dtBoolean),	
					new DataColumn("��Ч",dtStr),					//1
					new DataColumn("ҽ������",dtStr),				//2
					new DataColumn("ҽ����ˮ��",dtStr),				//3
					new DataColumn("ҽ��״̬",dtStr),				//4 �¿�������ˣ�ִ��
					new DataColumn("��Ϻ�",dtStr),					//5
					new DataColumn("��ҩ",dtStr),					//6
					new DataColumn("ҽ������",dtStr),				//8
					new DataColumn("��",dtStr),					    //9
					new DataColumn("��ע",dtStr),					//20
					new DataColumn("����",dtDbl),					//9
					new DataColumn("������λ",dtStr),				//10
					new DataColumn("ÿ����",dtStr),				//11
					new DataColumn("��λ",dtStr),					//12
					new DataColumn("����",dtStr),					//13
					new DataColumn("Ƶ�α���",dtStr),				//14
					new DataColumn("Ƶ��",dtStr),				//15
					new DataColumn("�÷�����",dtStr),				//16
					new DataColumn("�÷�",dtStr),				//17
					new DataColumn("����",dtStr),
					new DataColumn("��ʼʱ��",dtStr),				//18
					new DataColumn("ֹͣʱ��",dtStr),				//19
					new DataColumn("����ҽ��",dtStr),				//21
					new DataColumn("ִ�п��ұ���",dtStr),			//22
					new DataColumn("ִ�п���",dtStr),				//23
					new DataColumn("�Ӽ�",dtStr),					//24
					new DataColumn("��鲿λ",dtStr),				//25
					new DataColumn("��������/��鲿λ",dtStr),				//26
					new DataColumn("�ۿ���ұ���",dtStr),			//27
					new DataColumn("�ۿ����",dtStr),				//28
					new DataColumn("¼���˱���",dtStr),				//29	
					new DataColumn("¼����",dtStr),					//30
					new DataColumn("��������",dtStr),				//31
					new DataColumn("����ʱ��",dtStr),				//32
					new DataColumn("ֹͣ�˱���",dtStr),				//33
					new DataColumn("ֹͣ��",dtStr),					//34
					new DataColumn("Ƥ�Ա�־",dtStr),				//36
					new DataColumn("���ı�־",dtBoolean),
					
				});

                
                dataSet.Tables.Add(table);

                DataColumn[] keys = new DataColumn[1];
                keys[0] = dataSet.Tables[0].Columns["ҽ����ˮ��"];
                this.dataSet.Tables[0].PrimaryKey = keys;

                return dataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        #endregion

        #region ��ʾҽ��
        /// <summary>
        /// ���ʵ��toTable
        /// </summary>
        /// <param name="list"></param>
        private void AddObjectsToTable(ArrayList list)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order;
            this.alQueryLong = new ArrayList();
            this.alQueryShort = new ArrayList();
            foreach (object obj in list)
            {
                order = obj as Neusoft.HISFC.Models.Order.Inpatient.Order;
                if (order == null)
                    continue;

                if (!this.showDCOrder)
                {
                    if (order.Status == 3)		//����ʾ����/ֹͣҽ��
                        continue;
                }
                
                if (order.OrderType.Type == Neusoft.HISFC.Models.Order.EnumType.LONG)//����ҽ��
                {
                    dsAllLong.Tables[0].Rows.Add(AddObjectToRow(order, dsAllLong.Tables[0]));
                    alQueryLong.Add(order.Item.Name);
                }
                else//��ʱҽ��
                {
                    dsAllShort.Tables[0].Rows.Add(AddObjectToRow(order, dsAllShort.Tables[0]));
                    alQueryShort.Add(order.Item.Name);
                }
            
            }
            this.lblInfo.Text ="����:"+this.myPatientInfo.Name+ "  ���㷽ʽ: " + this.myPatientInfo.Pact.Name + "   ʣ����: " + this.myPatientInfo.FT.LeftCost;
        }


        private DataRow AddObjectToRow(object obj, DataTable table)
        {

            DataRow row = table.NewRow();
            
            string strTemp = "";
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            order = obj as Neusoft.HISFC.Models.Order.Inpatient.Order;
            

            if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
            {
                Neusoft.HISFC.Models.Pharmacy.Item objItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                row["��ҩ"] = System.Convert.ToInt16(order.Combo.IsMainDrug);	//6
                row["ÿ����"] = order.DoseOnce.ToString();					//10
                row["��λ"] = objItem.DoseUnit;								//0415 2307096 wang renyi
                row["����"] = order.HerbalQty;								//11
            }
            else if (order.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
            {
                //Neusoft.HISFC.Models.Fee.Item objItem = order.Item as Neusoft.HISFC.Models.Fee.Item;
            }

            if (order.Note != "")
            {
                row["!"] = order.Note;
            }
            row["��Ч"] = System.Convert.ToInt16(order.OrderType.Type);			//0
            row["ҽ������"] = order.OrderType.Name;								//2
            row["ҽ����ˮ��"] = order.ID;										//3
            row["ҽ��״̬"] = order.Status;										//12 �¿�������ˣ�ִ��
            row["��Ϻ�"] = order.Combo.ID;	//5

            if (order.Item.Specs == null || order.Item.Specs.Trim() == "")
            {
                row["ҽ������"] = order.Item.Name;
            }
            else
            {
                row["ҽ������"] = order.Item.Name + "[" + order.Item.Specs + "]";
            }

            //ҽ����ҩ
            if (order.IsPermission) row["ҽ������"] = "��" + row["ҽ������"];

            row["����"] = order.Qty;
            row["������λ"] = order.Unit;
            row["Ƶ�α���"] = order.Frequency.ID;
            row["Ƶ��"] = order.Frequency.Name;
            row["�÷�����"] = order.Usage.ID;
            row["�÷�"] = order.Usage.Name;
            row["����"] = order.Item.SysClass.Name;
            row["��ʼʱ��"] = order.BeginTime;
            row["ִ�п��ұ���"] = order.ExeDept.ID;
            if (order.ExeDept.Name == "" && order.ExeDept.ID != "") order.ExeDept.Name = this.GetDeptName(order.ExeDept);
            row["ִ�п���"] = order.ExeDept.Name;
            if (order.IsEmergency)
            {
                strTemp = "��";
            }
            else
            {
                strTemp = "��";
            }
            row["�Ӽ�"] = strTemp;
            row["��鲿λ"] = order.CheckPartRecord;
            row["��������/��鲿λ"] = order.Sample;
            row["�ۿ���ұ���"] = order.StockDept.ID;
            row["�ۿ����"] = deptHelper.GetName(order.StockDept.ID);

            row["��ע"] = order.Memo;
            row["¼���˱���"] = order.Oper.ID;

            row["¼����"] = order.Oper.Name;
            if (order.ReciptDept.Name == "" && order.ReciptDept.ID != "") order.ReciptDept.Name = this.GetDeptName(order.ReciptDept);
            row["����ҽ��"] = order.ReciptDoctor.Name;
            row["��������"] = order.ReciptDept.Name;
            row["����ʱ��"] = order.MOTime.ToString();
            #region addby xuewj {B8EDA745-62C3-407e-9480-3A9E60647141} δֹͣ��ҽ�� ֹͣʱ�䲻��ʾ
            if (!order.EndTime.ToString().Contains("0001"))
            {
                row["ֹͣʱ��"] = order.EndTime;
            }
            #endregion
            row["ֹͣ�˱���"] = order.DCOper.ID;
            row["ֹͣ��"] = order.DCOper.Name;
            
            row["˳���"] = order.SortID;
            row["Ƥ�Ա�־"] = order.HypoTest;
            row["���ı�־"] = order.IsSubtbl;

            
            return row;
        }


        /// <summary>
        /// ��ѯҽ��
        /// </summary>
        /// 
        private void QueryOrder()
        {
            try
            {
                this.fpSpread1.Sheets[0].RowCount = 0;
                this.fpSpread1.Sheets[1].RowCount = 0;
                this.dsAllLong.Tables[0].Rows.Clear();
                this.dsAllShort.Tables[0].Rows.Clear();
            }
            catch { }
            if (this.myPatientInfo == null)
            {
                return;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯҽ��,���Ժ�!");
            Application.DoEvents();

            if (this.ucSubtblManager1 != null)
            {
                this.ucSubtblManager1.PatientInfo = this.myPatientInfo;
            }

            //��ѯ����ҽ������
            ArrayList alAllOrder = orderManagement.QueryOrder(this.myPatientInfo.ID);
            if (alAllOrder == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(orderManagement.Err);
                return;
            }
            //��ѯ����ҽ������
            ArrayList alSub = this.orderManagement.QueryOrderSubtbl(this.myPatientInfo.ID);
            if (alSub == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(this.orderManagement.Err);
                return;
            }

            try
            {
                dsAllLong.Tables[0].Clear();
                dsAllShort.Tables[0].Clear();
                alAllOrder.AddRange(alSub);

                ArrayList al = new ArrayList();
                
                //������ʾ����ҽ��					
                foreach (Neusoft.HISFC.Models.Order.Order info in alAllOrder)
                {
                    if (info.Status != 4)
                        al.Add(info);
                }
            

                this.AddObjectsToTable(al);
                dvLong = new DataView(dsAllLong.Tables[0]);
                dvShort = new DataView(dsAllShort.Tables[0]);

                //{EACD8AED-FDF6-490a-980C-EC9A89391719} ��ʾǰ�Ƚ����������
                try
                {
                    dvLong.Sort = "˳��� ASC , ��Ϻ� ASC";
                    dvShort.Sort = "˳��� ASC , ��Ϻ� ASC";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("����ʾҽ������˳��š���Ϻ�����������" + ex.Message);
                    return;
                }

                this.fpSpread1.Sheets[0].DataSource = dvLong;
                this.fpSpread1.Sheets[1].DataSource = dvShort;

                
                this.InitFP();

                this.fpSpread1.Sheets[0].Columns[0, this.fpSpread1.Sheets[0].Columns.Count - 1].Locked = true;
                this.fpSpread1.Sheets[1].Columns[0, this.fpSpread1.Sheets[0].Columns.Count - 1].Locked = true;

            }
            catch (Exception ex)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(ex.Message);
                return;
            }

            this.Filter(this.cmbOderStatus.SelectedIndex);
            this.InitQueryCombox(this.sheetIndex);
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }


        ///<summary>
        /// ˢ�����
        /// </summary>
        public void RefreshCombo()
        {
            Classes.Function.DrawCombo(this.fpSpread1.Sheets[0], (int)ColEnum.ColComboNo, (int)ColEnum.ColComboFlag);

            Classes.Function.DrawCombo(this.fpSpread1.Sheets[1], (int)ColEnum.ColComboNo, (int)ColEnum.ColComboFlag);

            this.SetSortID();
        }


        ArrayList alDepts = null;

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


        /// <summary>
        /// ����ҽ��״̬
        /// </summary>
        public void RefreshOrderState()
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
                int i = (int)ColEnum.ColOrderState;//"ҽ��״̬";
                int j = (int)ColEnum.ColSort;//˳������ڵ���
                int state = int.Parse(this.fpSpread1.Sheets[SheetIndex].Cells[row, i].Text);
                
                switch (state)
                {
                    case 0://updated by zlw 006-4-18
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.FromArgb(128, 255, 128);

                        break;
                    case 1:
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.FromArgb(106, 174, 242);
                        break;
                    case 2:
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.FromArgb(243, 230, 105);
                        break;
                    case 3:
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.FromArgb(248, 120, 222);
                        break;
                    case 5:
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.Black;
                        break;
                    default:
                        this.fpSpread1.Sheets[SheetIndex].Cells[row, j].BackColor = Color.White;
                        break;
                }
            }
            catch { }
        }


        /// <summary>
        /// ���ñ�ע��Ƥ��
        /// </summary>
        /// <param name="k"></param>
        private void SetTip(int k)
        {
            for (int i = 0; i < this.fpSpread1.Sheets[k].RowCount; i++)//��ע
            {

                string sHypotest = this.fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColHypoTest].Text;
                if (fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Substring(fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Length - 1, 1) == "+" || fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Substring(fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Length - 1, 1) == "-")
                    fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text = fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Substring(0, fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text.Length - 1);

                fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].ForeColor = Color.Black;

                switch (sHypotest)
                {
                    case "2":
                        fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text += "[��Ƥ��]";//Ƥ��
                        break;
                    case "3":
                        fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text += "+";//Ƥ��
                        fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].ForeColor = Color.Red;
                        break;
                    case "4":
                        fpSpread1.Sheets[k].Cells[i, (int)ColEnum.ColItemName].Text += "-";
                        break;
                }
            }
        }

        private void SetSortID()
        {
            this.fpSpread1.Sheets[0].RowHeaderAutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
            this.fpSpread1.Sheets[1].RowHeaderAutoText = FarPoint.Win.Spread.HeaderAutoText.Blank;
        }
        
        #endregion

        #region IToolBar ��Ա

        public int Retrieve()
        {
            // TODO:  ��� ucOrderShow.Retrieve ʵ��
            this.QueryOrder();
            return 0;
        }

        public int Save()
        {
            if (ifHaveNotSameCom() == 0)
            {
                UpdateOrderSortID();
                

                QueryOrder();
            }
            else
            {
                MessageBox.Show("ͬһ������Ҫ��ͬ,�����Ӱ�������ʾ!");
            }

            return 0;
        }


        #endregion

        #region ����
        /// <summary>
        /// ����ҽ����ʾ
        /// </summary>
        /// <param name="State"></param>
        public void Filter(int State)
        {
            if (this.PatientInfo == null) return;
            if (this.PatientInfo.ID == "") return;
            if (this.dvLong == null || this.dvShort == null)
                return;

            //��ѯʱ����ܹ���
            switch (State)
            {
                case 0://ȫ��
                    dvLong.RowFilter = "";
                    dvShort.RowFilter = "";
                    break;
                case 2://����
                    DateTime dt = orderManagement.GetDateTimeFromSysDateTime();
                    DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    dvLong.RowFilter = "��ʼʱ��>=" + "#" + dt1 + "#" + " and ��ʼʱ��<=" + "#" + dt2 + "#" + " and ���ı�־ = false";
                    dvShort.RowFilter = "��ʼʱ��>=" + "#" + dt1 + "#" + " and ��ʼʱ��<=" + "#" + dt2 + "#" + " and ���ı�־ = false";
                    break;
                case 1://��Ч
                    dvLong.RowFilter = "ҽ��״̬ ='1' or ҽ��״̬ = '2'";
                    dvShort.RowFilter = "ҽ��״̬ ='1' or ҽ��״̬ = '2'";
                    break;
                case 5://��Ч
                    dvLong.RowFilter = "ҽ��״̬ = '3'";
                    dvShort.RowFilter = "ҽ��״̬ = '3'";
                    break;
                case 3://δ���
                    dvLong.RowFilter = "ҽ��״̬ = '0'";
                    dvShort.RowFilter = "ҽ��״̬ = '0'";
                    break;
                case 4://��������
                    DateTime d = orderManagement.GetDateTimeFromSysDateTime();
                    DateTime d1 = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                    DateTime d2 = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
                    dvLong.RowFilter = "ֹͣʱ��>=" + "#" + d1 + "#" + " and ֹͣʱ��<=" + "#" + d2 + "#" + " and ҽ��״̬ = '3'";
                    dvShort.RowFilter = "ֹͣʱ��>=" + "#" + d1 + "#" + " and ֹͣʱ��<=" + "#" + d2 + "#" + " and ҽ��״̬ = '3'";
                    break;
                case 6://δ���
                    dvLong.RowFilter = "ҽ��״̬ = '4'";//������
                    dvShort.RowFilter = "ҽ��״̬ = '4'";//������
                    //Ƥ��ҽ��//{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
                    dvLong.RowFilter = "Ƥ�Ա�־ in ('2','3','4')";
                    dvShort.RowFilter = "Ƥ�Ա�־ in ('2','3','4')";
                    break;
                default:
                    dvLong.RowFilter = "";
                    dvShort.RowFilter = "";
                    break;
            }

            this.InitFP();
            this.RefreshOrderState();
            this.RefreshCombo();
            this.RefreshSubtblDisplay(0);
            this.RefreshSubtblDisplay(1);
            if (this.fpSpread1.Sheets[this.fpSpread1.ActiveSheetIndex].Rows.Count >= 20)
            {
                this.neuLabel2.Visible = true;
                this.txtQuery.Visible = true;
            }
            else
            {
                this.neuLabel2.Visible = false;
                this.txtQuery.Visible = false;
            }
            this.SetTip(0);
            this.SetTip(1);
        }

        /// <summary>
        /// ��Ӳ�ѯ�����б�����
        /// </summary>
        /// <param name="name"></param>
        private void addCombo(string name)
        {
            if (this.txtQuery.FindStringExact(name) >= 0) return;
            this.txtQuery.Items.Add(name);
        }

        private void InitQueryCombox(int index)
        {
            this.txtQuery.Items.Clear();
            string orderName = "";
            if (index == 0)
            {
                for (int i = 0; i < this.alQueryLong.Count; i++)
                {
                    orderName = this.alQueryLong[i].ToString();
                    this.addCombo(orderName);
                }
            }

            if (index == 1)
            {
                for (int i = 0; i < this.alQueryShort.Count; i++)
                {
                    orderName = this.alQueryShort[i].ToString();
                    this.addCombo(orderName);
                }
            }
        }

        #endregion

        #region ��������
        public Crownwood.Magic.Docking.DockingManager dockingManager;
        /// <summary>
        /// ���Ĺ���ؼ�
        /// </summary>
        private Crownwood.Magic.Docking.Content content;

        /// <summary>
        /// Ƥ��ҩ�ؼ�
        /// </summary>
        private Crownwood.Magic.Docking.Content hypoTestContent;

        private Crownwood.Magic.Docking.WindowContent wc;

        private Crownwood.Magic.Docking.WindowContent wc1;

        public void DockingManager()
        {
            this.dockingManager = new Crownwood.Magic.Docking.DockingManager(this, Crownwood.Magic.Common.VisualStyle.IDE);
            this.dockingManager.InnerControl = this.panel1;		//��InnerControlǰ����Ŀؼ�����ͣ�����ڵ�Ӱ��

            content = new Crownwood.Magic.Docking.Content(this.dockingManager);
            content.Control = ucSubtblManager1;

            Size ucSize = content.Control.Size;

            content.Title = "���Ĺ���";
            content.FullTitle = "���Ĺ���";
            content.AutoHideSize = ucSize;
            content.DisplaySize = ucSize;

            //{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
            this.hypoTestContent = new Crownwood.Magic.Docking.Content(this.dockingManager);
            this.hypoTestContent.Control = ucTip;

            Size ucTipSize = this.hypoTestContent.Control.Size;
            this.hypoTestContent.Title = "Ƥ��ҩ����";
            this.hypoTestContent.FullTitle = "Ƥ��ҩ����";
            this.hypoTestContent.AutoHideSize = ucTipSize;
            this.hypoTestContent.DisplaySize = ucTipSize;

            this.dockingManager.Contents.Add(this.hypoTestContent);
            this.dockingManager.Contents.Add(content);
        
        }
        #endregion

        #region ������ʾ
        /// <summary>
        /// ���¸�����ʾ��־
        /// </summary>
        private void RefreshSubtblFlag(string operFlag, bool isShowSubtblFlag, object sender)
        {
            if (this.fpSpread1.Sheets[this.sheetIndex].Rows.Count < 0)
                return;

            int rowIndex = this.fpSpread1.ActiveSheet.ActiveRowIndex;
            string s = this.fpSpread1.Sheets[this.sheetIndex].Cells[rowIndex, (int)ColEnum.ColItemName].Text;       //ҽ������
            string comboNo = this.fpSpread1.Sheets[this.sheetIndex].Cells[rowIndex, (int)ColEnum.ColComboNo].Text;	//��Ϻ�

            #region ˢ�������ʾ"@"
            //���ڲ���ͬһ���ҽ��
            int iUp = rowIndex;
            bool isUp = true;
            int iDown = rowIndex;
            bool isDown = true;

            if (!isShowSubtblFlag)	//������ʾ"@"����
            {
                while (isUp || isDown)
                {
                    #region ���ϲ��� �絽��ǰһ�л���ϺŲ�ͬ���ñ�־Ϊfalse
                    if (isUp)
                    {
                        iUp = iUp - 1;
                        if (iUp < 0)
                            isUp = false;
                        else
                        {
                            if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColComboNo].Text == comboNo)				//ͬһ���
                            {
                                if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColItemName].Text.Substring(0, 1) == "@")	//ҽ�����ƴ���"@"����
                                {
                                    this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColItemName].Text = this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColItemName].Text.Substring(1);
                                }
                            }
                            else		//����ͬһ��� �����ٲ���
                            {
                                isUp = false;
                            }
                        }
                    }
                    #endregion

                    #region ���²��� ��������һ�л���ϺŲ�ͬ���ñ�־Ϊfalse
                    if (isDown)
                    {
                        iDown = iDown + 1;
                        if (iDown >= this.fpSpread1.Sheets[this.sheetIndex].Rows.Count)
                            isDown = false;
                        else
                        {
                            if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColComboNo].Text == comboNo)					//ͬһ���
                            {
                                if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColItemName].Text.Substring(0, 1) == "@")	//ҽ�����ƴ���"@"����
                                {
                                    this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColItemName].Text = this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColItemName].Text.Substring(1);
                                }
                            }
                            else			//����ͬһ��� �����ٲ���
                            {
                                isDown = false;
                            }
                        }
                    }
                    #endregion
                }
                //���±�����¼ҽ����־
                if (s.Substring(0, 1) == "@")
                {
                    this.fpSpread1.Sheets[this.sheetIndex].Cells[rowIndex, (int)ColEnum.ColItemName].Text = s.Substring(1);
                }
            }
            else		//��Ҫ��ʾ"@"����
            {
                bool isAlreadyHave = false;			//������Ƿ��Ѱ���"@"ҽ������
                while (isUp || isDown)
                {
                    #region ���ϲ��� �絽��ǰһ�л���ϺŲ�ͬ���ñ�־Ϊfalse
                    if (isUp)
                    {
                        iUp = iUp - 1;
                        if (iUp < 0)
                            isUp = false;
                        else
                        {
                            if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColComboNo].Text == comboNo)					//ͬһ�����
                            {
                                if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iUp, (int)ColEnum.ColItemName].Text.Substring(0, 1) == "@")		//�Ѿ�����"@"����
                                {
                                    isAlreadyHave = true;
                                    break;
                                }
                            }
                            else
                            {
                                isUp = false;
                            }
                        }
                    }
                    #endregion

                    #region ���²��� ��������һ�л���ϺŲ�ͬ���ñ�־Ϊfalse
                    if (isDown)
                    {
                        iDown = iDown + 1;
                        if (iDown >= this.fpSpread1.Sheets[this.sheetIndex].Rows.Count)
                            isDown = false;
                        else
                        {
                            if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColComboNo].Text == comboNo)
                            {
                                if (this.fpSpread1.Sheets[this.sheetIndex].Cells[iDown, (int)ColEnum.ColItemName].Text.Substring(0, 1) == "@")
                                {
                                    isAlreadyHave = true;
                                    break;
                                }
                            }
                            else
                            {
                                isDown = false;
                            }
                        }
                    }
                    #endregion
                }
                //�������δ����"@"����
                if (!isAlreadyHave && s.Substring(0, 1) != "@")
                {
                    this.fpSpread1.Sheets[this.sheetIndex].Cells[rowIndex, (int)ColEnum.ColItemName].Text = "@" + s;
                }
            }
            #endregion

            #region �ı���渽�ĵ���ʾ ��ӻ�ɾ��
            try
            {
                if (operFlag == "2")					//ɾ��/ֹͣ����
                {
                    #region ����ɾ��/ֹͣ����ʱ�ĸ��Ľ�����ʾ
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = sender as Neusoft.HISFC.Models.Order.Inpatient.Order;
                    if (order == null)
                    {
                        MessageBox.Show("׼��ˢ�´�����渽����ʾʱ�������� ���˳���������");
                        return;
                    }
                    if (order.ID != "")					//�ѱ��渽�� 
                    {
                        if (this.sheetIndex == 0)		//����
                        {
                            string[] tempFind = new string[1];								//Ѱ����ɾ���е�����
                            tempFind[0] = order.ID;
                            DataRow delRow = this.dsAllLong.Tables[0].Rows.Find(tempFind);	//����DataSet���Ƴ�����				
                            this.dsAllLong.Tables[0].Rows.Remove(delRow);					//�Ƴ���

                            if (order.Status != 0)											//�������/ִ�е����� ������ʾ ���ı�״̬
                            {
                                order.Status = 3;
                                //��Ӹı�״̬���� 
                                this.dsAllLong.Tables[0].Rows.Add(this.AddObjectToRow(order, this.dsAllLong.Tables[0]));
                            }
                        }
                        else							//����
                        {
                            string[] tempFind = new string[1];								//Ѱ����ɾ���е�����
                            tempFind[0] = order.ID;
                            DataRow delRow = this.dsAllShort.Tables[0].Rows.Find(tempFind);//����DataSet���Ƴ�����	
                            this.dsAllShort.Tables[0].Rows.Remove(delRow);					//�Ƴ���

                            if (order.Status != 0)											//�������/ִ�е����� ������ʾ ���ı�״̬
                            {
                                order.Status = 3;
                                //��Ӹı�״̬���� 
                                this.dsAllShort.Tables[0].Rows.Add(this.AddObjectToRow(order, this.dsAllShort.Tables[0]));
                            }
                        }
                        //���������Ϣˢ��
                        
                        this.Filter(this.cmbOderStatus.SelectedIndex);
                       
                    }
                    #endregion
                }
                else									//�������
                {
                    if (this.ucSubtblManager1.AddSubInfo != null && this.ucSubtblManager1.AddSubInfo.Count > 0)
                    {
                        this.AddObjectsToTable(this.ucSubtblManager1.AddSubInfo);			//��DataSet�ڼ���������
                        //���������Ϣˢ��
                        
                        this.Filter(this.cmbOderStatus.SelectedIndex);
                       
                    }
                    if (this.ucSubtblManager1.EditSubInfo != null && this.ucSubtblManager1.EditSubInfo.Count > 0)
                    {
                        foreach (Neusoft.HISFC.Models.Order.Order info in this.ucSubtblManager1.EditSubInfo)
                        {
                            if (info == null) continue;
                            int row = 0, col = 0;
                            string find = this.fpSpread1.Search(this.fpSpread1.ActiveSheetIndex, info.ID, false, true, false, false,
                                0, 0, ref row, ref col);
                            if (find == info.ID)
                            {
                                this.fpSpread1.ActiveSheet.Cells[row, (int)ColEnum.ColQty].Text = info.Qty.ToString();
                                this.fpSpread1.ActiveSheet.Cells[row, (int)ColEnum.ColUnit].Text = info.Unit;
                            }
                        }
                    }
                }
            }
            catch (System.Data.ConstraintException)
            {
                MessageBox.Show("�޷��������ͬ���ĸ��ģ������޸ĸ�������");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("��������ʾʱ��������Ԥ֪���� ���˳���������" + ex.Message);
                return;
            }
            #endregion

            this.RefreshSubtblDisplay(this.sheetIndex);
        }

        /// <summary>
        /// ˢ�¸�����ʾ �Ը�����ʾб����
        /// </summary>
        /// <param name="sheetIndex"></param>
        private void RefreshSubtblDisplay(int sheetIndex)
        {
            for (int i = 0; i < this.fpSpread1.Sheets[sheetIndex].RowCount; i++)
            {
                string temp = this.fpSpread1.Sheets[sheetIndex].Cells[i, (int)ColEnum.ColSubtbl].Text;

                if (temp == "True")
                {
                    this.fpSpread1.Sheets[sheetIndex].Cells[i, (int)ColEnum.ColItemName].Font = new Font("����", 10, System.Drawing.FontStyle.Italic);
                    this.fpSpread1.Sheets[sheetIndex].Cells[i, 9].Locked = true;

                }
                else
                {
                    this.fpSpread1.Sheets[sheetIndex].Cells[i, (int)ColEnum.ColItemName].Font = new Font("����", 10, System.Drawing.FontStyle.Bold);
                    this.fpSpread1.Sheets[sheetIndex].Cells[i, 9].Locked = true;
                }
            }

        }

        #endregion

        
        private void fpSpread1_SheetTabClick(object sender, FarPoint.Win.Spread.SheetTabClickEventArgs e)
        {
            
            this.sheetIndex = e.SheetTabIndex;
            this.ucSubtblManager1.Clear();
            if (this.sheetIndex == 0)
            {
                //this.cmbOderStatus.SelectedIndex = 1;
                //{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
                if (this.dockingManager != null)
                {
                    this.dockingManager.HideContent(this.hypoTestContent);
                }
            }

            if (this.sheetIndex == 1)
            {
                //this.cmbOderStatus.SelectedIndex = 2;

                //{07B60769-DFBE-4797-823D-3C07ACD737B4}
                //��ʱҽ������ʾ���Ľ���
                if (this.dockingManager != null)
                {
                    this.dockingManager.HideContent(this.content);
                }
            }
            this.InitQueryCombox(this.sheetIndex);
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!this.enableSubtbl)
                return;

            //�жϵ�ǰ��ͣ�������Ƿ�����ʾ ��δ��ʾ ����ʾͣ������
            try
            {
                //{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
                //Ƥ��ҩ��ʾ
                //��ȡƤ��ҩ���
                this.orderId = e.View.Sheets[e.View.ActiveSheetIndex].Cells[e.Row, (int)ColEnum.ColOrderID].Value.ToString();
                string flag = e.View.Sheets[e.View.ActiveSheetIndex].Cells[e.Row, (int)ColEnum.ColHypoTest].Value.ToString();
                if (flag == "2" || flag == "3" || flag == "4")
                {
                    this.ucTip.Hypotest = Neusoft.FrameWork.Function.NConvert.ToInt32(flag);
                    this.ucTip.Tip = this.orderManagement.QueryOrderNote(orderId);

                    if (this.hypoTestContent != null && this.hypoTestContent.Visible == false)
                    {
                        if (wc1 == null && this.dockingManager != null)
                        {
                            wc1 = this.dockingManager.AddContentWithState(this.hypoTestContent, Crownwood.Magic.Docking.State.DockRight);
                            this.dockingManager.AddContentToWindowContent(this.hypoTestContent, wc1);
                        }
                        if (this.dockingManager != null)
                            this.dockingManager.ShowContent(this.hypoTestContent);
                    }
                }
                else
                {
                    if (this.dockingManager != null)
                    {
                        this.dockingManager.HideContent(this.hypoTestContent);
                    }
                }
                //{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}

                //{07B60769-DFBE-4797-823D-3C07ACD737B4}
                //��ʱҽ������ʾ���Ľ���
                if (e.View.ActiveSheetIndex == 0)
                {
                    if (this.content != null && this.content.Visible == false)
                    {
                        if (wc == null && this.dockingManager != null)
                        {
                            wc = this.dockingManager.AddContentWithState(content, Crownwood.Magic.Docking.State.DockBottom);
                            this.dockingManager.AddContentToWindowContent(content, wc);
                        }
                        if (this.dockingManager != null)
                            this.dockingManager.ShowContent(this.content);
                    }
                    if (this.ucSubtblManager1 != null && !e.RowHeader && !e.ColumnHeader)		//������б������б���
                    {
                        ucSubtblManager1.OrderID = this.fpSpread1.ActiveSheet.Cells[e.Row, (int)ColEnum.ColOrderID].Text;
                        ucSubtblManager1.ComboNo = this.fpSpread1.ActiveSheet.Cells[e.Row, (int)ColEnum.ColComboNo].Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ucSubtblManager1_ShowSubtblFlag(string operFlag, bool isShowSubtblFlag, object sender)
        {
            this.RefreshSubtblFlag(operFlag, isShowSubtblFlag, sender);
        }

        /// <summary>
        /// Ƥ��ҩ����
        /// {17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
        /// </summary>
        /// <param name="Tip"></param>
        /// <param name="Hypotest"></param>
        public void ucTip_OKEvent(string Tip, int Hypotest)
        {
            if (this.orderManagement.UpdateFeedback(this.PatientInfo.ID, this.orderId, Tip, Hypotest) == -1)
            {
                MessageBox.Show(this.orderManagement.Err);
                this.orderManagement.Err = "";
                return;
            }


            this.fpSpread1.ActiveSheet.Cells[this.fpSpread1.ActiveSheet.ActiveRowIndex, (int)ColEnum.ColHypoTest].Value = Hypotest;

            this.SetTip(this.fpSpread1.ActiveSheetIndex);
        }

        #region �Ҽ��˵�
        private void fpSpread1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!this.enableSubtbl)
                return;
            if (e.Button == MouseButtons.Right)
            {
                FarPoint.Win.Spread.Model.CellRange c = this.fpSpread1.GetCellFromPixel(0, 0, e.X, e.Y);
                if (c.Row >= 0)
                {
                    this.fpSpread1.ActiveSheet.ActiveRowIndex = c.Row;
                    this.fpSpread1.ActiveSheet.ClearSelection();
                    this.fpSpread1.ActiveSheet.AddSelection(c.Row, 0, 1, 1);
                }
                if (c.Row < 0) return;
                orderId = this.fpSpread1.ActiveSheet.Cells[c.Row, (int)ColEnum.ColOrderID].Text;

                if (this.contextMenuStrip1.Items.Count > 0)
                    this.contextMenuStrip1.Items.Clear();

                ToolStripMenuItem mnuSetTime = new ToolStripMenuItem("ִ��ʱ��");
                mnuSetTime.Click += new EventHandler(mnuSetTime_Click);
                ToolStripMenuItem menuTip = new ToolStripMenuItem("��ע/Ƥ��");
                menuTip.Click += new EventHandler(menuTip_Click);

                this.contextMenuStrip1.Items.Add(mnuSetTime);
                this.contextMenuStrip1.Items.Add(menuTip);
            }

        }

        private void mnuSetTime_Click(object sender, EventArgs e)
        {
            //frmSetExecTime frm = new frmSetExecTime();
            //frm.SetItem(orderId);
            //frm.ShowDialog();
        }

        private void menuTip_Click(object sender, EventArgs e)
        {
            //ucTip ucTip1 = new ucTip();
            //ucTip1.IsEnabled = true;
            //int iHypotest = this.orderManagement.QueryOrderHypotest(this.orderId);
            //if (iHypotest == -1)
            //{
            //    MessageBox.Show(this.orderManagement.Err);
            //    return;
            //}

            ////��ҩƷҽ������ʾƤ��ҳ
            //Neusoft.HISFC.Models.Order.Order o = this.orderManagement.QueryOneOrder(this.orderId);
            //if (o.Item.isPharmacy == false)
            //{
            //    ucTip1.Hypotest = 1;
            //}
            //ucTip1.Tip = this.orderManagement.QueryOrderNote(this.orderId);
            //ucTip1.Hypotest = iHypotest;
            //ucTip1.OKEvent += new myTipEvent(ucTip1_OKEvent);
            //Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucTip1);

        }
        /// <summary>
        /// ��ע�¼�
        /// </summary>
        private void ucTip1_OKEvent(string Tip, int Hypotest)
        {
            //int rowIndex = this.fpSpread1.Sheets[this.sheetIndex].ActiveRowIndex;
            //if (this.orderManagement.Updatefeedback(this.myPatientInfo.ID, this.orderId, Tip, Hypotest) == -1)
            //{
            //    MessageBox.Show(this.orderManagement.Err);
            //    this.orderManagement.Err = "";
            //    return;
            //}
            //this.fpSpread1.Sheets[this.sheetIndex].Cells[rowIndex, (int)ColEnum.ColHypoTest].Text = Hypotest.ToString();
            //this.SetTip(this.sheetIndex);
            //Neusoft.HISFC.BizLogic.RADT.InPatient pManager = new Neusoft.HISFC.BizLogic.RADT.InPatient();
            //Neusoft.HISFC.Models.RADT.PatientInfo p = pManager.PatientQuery(this.myPatientInfo.ID);
            ////������Ϣ������
            //Neusoft.Common.Class.Message.SendMessage(p.Name + "�е�ҽ��������<" + Tip + ">��Ҫ���ġ�", p.PVisit.PatientLocation.Dept.ID, "22222");
        }
        #endregion

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData.GetHashCode() == Keys.Alt.GetHashCode() + Keys.S.GetHashCode())
            {
                if (this.fpSpread1.ActiveSheetIndex == 0)
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
                else
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[1], SHORTSETTINGFILENAME);
            }
            else if (keyData.GetHashCode() == Keys.F12.GetHashCode())
            {
                this.fpSpread1.ActiveSheetIndex = (this.fpSpread1.ActiveSheetIndex + 1) % 2;
            }
            return base.ProcessDialogKey(keyData);
        }
        
        private void txtQuery_TextChanged(object sender, System.EventArgs e)
        {
            if (this.PatientInfo == null) return;
            if (this.PatientInfo.ID == "") return;
            if (this.dvLong == null || this.dvShort == null)
                return;
            string rowFilter = "ҽ������ like '%{0}%'";
            string textQuery = Neusoft.FrameWork.Public.String.TakeOffSpecialChar(this.txtQuery.Text.Trim());
            rowFilter = System.String.Format(rowFilter, textQuery);
            //����ҽ��
            if (this.fpSpread1.ActiveSheetIndex == 0)
                this.dvLong.RowFilter = rowFilter;
            //��ʱҽ��
            else
                this.dvShort.RowFilter = rowFilter;

            this.InitFP();
            this.RefreshOrderState();
            this.RefreshCombo();
            this.RefreshSubtblDisplay(0);
            this.RefreshSubtblDisplay(1);
        }

        #region  ҽ��
        /// <summary>
        /// ͨ��ҽ��״̬����ҽ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOderStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Filter(this.cmbOderStatus.SelectedIndex);
        }
        /// <summary>
        /// ����ҽ�����
        /// </summary>
        private void UpdateOrderSortID()
        {
            int colorderid = (int)ColEnum.ColOrderID;    //ҽ����ˮ��    
            int sortid = (int)ColEnum.ColSort;
            string OrderID = null;//ҽ�����
            string SortID = null; //˳���
            FarPoint.Win.Spread.SheetView sv = fpSpread1.ActiveSheet;//ȡ�õ�ǰ��������Ч��SHEET
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڸ���ҽ�����...");
            Application.DoEvents();
            for (int i = 0; i < sv.Rows.Count; i++) //����ҽ��
            {
                OrderID = sv.Cells[i, colorderid].Text;//ҽ�����
                SortID = sv.Cells[i, sortid].Text; //˳����
                int Sortid = 0;
                if (sv.Cells[i, 2].Text.ToUpper() == "TRUE")
                {
                    
                    Sortid = Convert.ToInt32(SortID) - 10000;
                    
                    SortID = Sortid.ToString();
                }
                #region ҽ����Ÿ���
                if (orderManagement.UpdateOrderSortID(OrderID, SortID) == -1)
                {
                    MessageBox.Show("���´���!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return;

                }

                #endregion
            }
            
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        
        /// <summary>
        /// ��Ҫʱ�ж��ǲ��Ǻ�����ͬ��Ϻţ���Ų�ͬ��ҽ��
        /// </summary>
        /// <returns></returns>
        private int ifHaveNotSameCom()
        {
            int m = 0;
            for (int i = 0; i < fpSpread1.ActiveSheet.RowCount; i++)
            {
                string sortNum = fpSpread1.ActiveSheet.Cells[i, 1].Text;//��ǰѡ���е����
                string sComNum = fpSpread1.ActiveSheet.Cells[i, 7].Text;//��ǰѡ���е���Ϻ�
                for (int j = 0; j < fpSpread1.ActiveSheet.RowCount; j++)
                {
                    string sortNum1 = fpSpread1.ActiveSheet.Cells[j, 1].Text;//��ǰѡ���е����
                    string sComNum1 = fpSpread1.ActiveSheet.Cells[j, 7].Text;//��ǰѡ���е���Ϻ�
                    if (sComNum1 == sComNum)
                    {
                        if (sortNum1 != sortNum)
                        {
                            m += 1;
                        }
                    }
                }
            }

            if (m >= 1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
     
        

        #endregion

        #region ������
        protected enum ColEnum
        {
            /// <summary>
            /// ��ʿ��ע
            /// </summary>
            ColNurMemo,
            /// <summary>
            /// ˳���
            /// </summary>
            ColSort,//  updated by zlw 2006-4-18
            /// <summary>
            /// ����
            /// </summary>
            ColInforming,
            /// <summary>
            /// ҽ�����ʹ��� ��Ч 0 ����
            /// </summary>
            ColOrderTypeID,
            /// <summary>
            /// ҽ������
            /// </summary>
            ColOrderTypeName,
            /// <summary>
            /// ҽ����ˮ��
            /// </summary>
            ColOrderID,
            /// <summary>
            /// ҽ��״̬
            /// </summary>
            ColOrderState,
            /// <summary>
            /// ��Ϻ�
            /// </summary>
            ColComboNo,
            /// <summary>
            /// ��ҩ
            /// </summary>
            ColMainDrug,
            /// <summary>
            /// ҽ������
            /// </summary>
            ColItemName,
            /// <summary>
            /// ����
            /// </summary>
            ColComboFlag,
            /// <summary>
            /// ��ע
            /// </summary>
            ColMemo,
            /// <summary>
            /// ����
            /// </summary>
            ColQty,
            /// <summary>
            /// ������λ
            /// </summary>
            ColUnit,
            /// <summary>
            /// ÿ����
            /// </summary>
            ColDoseOnce,
            /// <summary>
            /// ��λ
            /// </summary>
            ColDoseUnit,
            /// <summary>
            /// ����
            /// </summary>
            ColHerbalQty,
            /// <summary>
            /// Ƶ�α���
            /// </summary>
            ColFrequencyID,
            /// <summary>
            /// Ƶ��
            /// </summary>
            ColFrequencyName,
            /// <summary>
            /// �÷�����
            /// </summary>
            ColUsageID,
            /// <summary>
            /// �÷�
            /// </summary>
            ColUsageName,
            /// <summary>
            /// ����
            /// </summary>
            ColSysType,
            /// <summary>
            /// ��ʼʱ��
            /// </summary>
            ColOrderBgn,
            /// <summary>
            /// ֹͣʱ��
            /// </summary>
            ColOrderEnd,
            /// <summary>
            /// ����ҽ��
            /// </summary>
            ColDoc,
            /// <summary>
            /// ִ�п��ұ���
            /// </summary>
            ColExeDeptID,
            /// <summary>
            /// ִ�п���
            /// </summary>
            ColExeDeptName,
            /// <summary>
            /// �Ӽ�
            /// </summary>
            ColEmEmergency,
            /// <summary>
            /// ��鲿λ
            /// </summary>
            ColCheckPart,
            /// <summary>
            /// ��������
            /// </summary>
            ColSample,
            /// <summary>
            /// �ۿ���ұ���
            /// </summary>
            ColStockDeptID,
            /// <summary>
            /// �ۿ����
            /// </summary>
            ColStockDeptName,
            /// <summary>
            /// ¼���˱���
            /// </summary>
            ColUseRecID,
            /// <summary>
            /// ¼����
            /// </summary>
            ColUseRecName,
            /// <summary>
            /// ��������
            /// </summary>
            ColRecDept,
            /// <summary>
            /// ����ʱ��
            /// </summary>
            ColRecDate,
            /// <summary>
            /// ֹͣ�˱���
            /// </summary>
            ColDCOperID,
            /// <summary>
            /// ֹͣ��
            /// </summary>
            ColDCOperName,
            /// <summary>
            /// Ƥ�Ա�־
            /// </summary>
            ColHypoTest,
            /// <summary>
            /// ���ı�־
            /// </summary>
            ColSubtbl
        }
        #endregion

        #region ��д
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
          

            try//������п���
            {

                alDepts = deptManagement.GetDepartment();
            }
            catch { }
            //��ʼ��farpoint
            dsAllLong = this.InitDataSet();
            dsAllShort = this.InitDataSet();

            //sheet0 ==���� sheet1 ==��ʱ
            this.fpSpread1.Sheets[0].DataSource = dsAllLong.Tables[0];
            this.fpSpread1.Sheets[1].DataSource = dsAllShort.Tables[0];

            this.fpSpread1.Sheets[0].DataAutoSizeColumns = false;
            this.fpSpread1.Sheets[1].DataAutoSizeColumns = false;

            this.fpSpread1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
            this.fpSpread1.SheetTabClick += new FarPoint.Win.Spread.SheetTabClickEventHandler(fpSpread1_SheetTabClick);
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread1_CellDoubleClick);


            DateTime dt = this.orderManagement.GetDateTimeFromSysDateTime();
            this.InitFP();

            try
            {
                this.fpSpread1.ActiveSheetIndex = 0;
                this.cmbOderStatus.SelectedIndex = 1;//Ĭ��ѡ��Ч��ҽ��
            }
            catch { }

            #region ���Ĺ�����
            ucSubtblManager1 = new ucSubtblManager();
            //Ƥ��ҩ{17A8C36D-DFA8-4d4e-A2AB-893AD5B3073A}
            ucTip = new ucTip();
            ucTip.IsCanModifyHypotest = true;
            this.ucTip.OKEvent += new myTipEvent(ucTip_OKEvent);
            this.DockingManager();
            this.ucSubtblManager1.ShowSubtblFlag += new ucSubtblManager.ShowSubtblFlagEvent(ucSubtblManager1_ShowSubtblFlag);
            #endregion
            //{EB125429-3FD1-4608-A99F-36F03E35299C}�ų��쳣���ж�tv�Ƿ�Ϊ�� by guanyx
            if (this.tv != null)
            {
                this.tv.CheckBoxes = false;
                this.tv.ExpandAll();
            }
            return base.OnInit(sender, neuObject, param);
        }

        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            if(this.tv.CheckBoxes == true)
                this.tv.CheckBoxes = false;
            this.PatientInfo = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;
            
            return base.OnSetValue(neuObject, e);
        }
        #endregion

        private void fpSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            if (this.fpSpread1.ActiveSheetIndex == 0)
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
            else
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[1], SHORTSETTINGFILENAME);
        }

        /// <summary>
        /// ������ҽ����ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReOrderQueryed_Click(object sender, EventArgs e)
        {
            try
            {
                this.fpSpread1.Sheets[0].RowCount = 0;
                this.fpSpread1.Sheets[1].RowCount = 0;
                this.dsAllLong.Tables[0].Rows.Clear();
                this.dsAllShort.Tables[0].Rows.Clear();
            }
            catch { }
            if (this.myPatientInfo == null)
            {
                return;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯ������ҽ��,���Ժ�!");
            Application.DoEvents();

            if (this.ucSubtblManager1 != null)
            {
                this.ucSubtblManager1.PatientInfo = this.myPatientInfo;
            }

            //��ѯ����ҽ������
            ArrayList alAllOrder = orderManagement.QueryOrder(this.myPatientInfo.ID);
            if (alAllOrder == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(orderManagement.Err);
                return;
            }
            //��ѯ����ҽ������
            ArrayList alSub = this.orderManagement.QueryOrderSubtbl(this.myPatientInfo.ID);
            if (alSub == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(this.orderManagement.Err);
                return;
            }

            try
            {
                dsAllLong.Tables[0].Clear();
                dsAllShort.Tables[0].Clear();
                alAllOrder.AddRange(alSub);

                ArrayList al = new ArrayList();

                //������ʾ����ҽ��					
                foreach (Neusoft.HISFC.Models.Order.Order info in alAllOrder)
                {
                    if (info.Status == 4)
                        al.Add(info);
                }


                this.AddObjectsToTable(al);
                dvLong = new DataView(dsAllLong.Tables[0]);
                dvShort = new DataView(dsAllShort.Tables[0]);

                try
                {
                    dvLong.Sort = "˳��� ASC , ��Ϻ� ASC";
                    dvShort.Sort = "˳��� ASC , ��Ϻ� ASC";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("����ʾҽ������˳��š���Ϻ�����������" + ex.Message);
                    return;
                }

                this.fpSpread1.Sheets[0].DataSource = dvLong;
                this.fpSpread1.Sheets[1].DataSource = dvShort;


                this.InitFP();

                this.fpSpread1.Sheets[0].Columns[0, this.fpSpread1.Sheets[0].Columns.Count - 1].Locked = true;
                this.fpSpread1.Sheets[1].Columns[0, this.fpSpread1.Sheets[0].Columns.Count - 1].Locked = true;

            }
            catch (Exception ex)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(ex.Message);
                return;
            }

            this.Filter(this.cmbOderStatus.SelectedIndex);
            this.InitQueryCombox(this.sheetIndex);
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }
    }
}
