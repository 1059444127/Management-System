using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FarPoint.Win.Spread;

namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// {49026086-DCA3-4af4-A064-58F7479C324A}
    /// </summary>
    public delegate void RefreshGroupTree();
    /// <summary>
    /// [��������: ��ҩҽ������]<br></br>
    /// [�� �� ��: dorian]<br></br>
    /// [����ʱ��: 2007-10]<br></br>
    /// <�޸ļ�¼
    ///  />
    /// </summary>
    public partial class ucHerbalOrder : UserControl
    {
        public ucHerbalOrder()
        {
            InitializeComponent();
        }

        public ucHerbalOrder(bool isClinic, Neusoft.HISFC.Models.Order.EnumType orderType, string deptCode)
            : this()
        {
            this.isClinic = isClinic;
            this.DeptCode = deptCode;
            this.OrderType = orderType;
            this.fpEnter1_Sheet1.Rows.Count = 0;
            this.fpEnter1_Sheet1.Rows.Add(0, 1);

            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnDel.Click += new EventHandler(btnDel_Click);
            this.Load += new EventHandler(ucHerbalOrder_Load);
        }

        #region ����
        /// <summary>
        /// �Ƿ�����ʹ��
        /// </summary>
        private bool isClinic = false;

        /// <summary>
        /// �Ƿ�����ʹ��
        /// </summary>
        public bool IsClinic
        {
            set
            {
                this.isClinic = value;
            }
        }

        /// <summary>
        /// ҽ����� 0 ���� 1 ����
        /// </summary>
        private Neusoft.HISFC.Models.Order.EnumType orderType;

        /// <summary>
        /// ҽ����� 0 ���� 1 ����
        /// </summary>
        public Neusoft.HISFC.Models.Order.EnumType OrderType
        {
            set
            {
                this.orderType = value;
                if (this.alLong == null || this.alShort == null)
                {
                    try
                    {
                        this.DataInit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (value == Neusoft.HISFC.Models.Order.EnumType.LONG)
                {
                    this.cmbOrderType.DataSource = alLong;
                    this.cmbOrderType.DisplayMember = "Name";
                    this.cmbOrderType.ValueMember = "ID";
                    this.orderTypeHelper.ArrayObject = this.alLong;
                }
                else
                {
                    this.cmbOrderType.DataSource = alShort;
                    this.cmbOrderType.DisplayMember = "Name";
                    this.cmbOrderType.ValueMember = "ID";
                    this.orderTypeHelper.ArrayObject = this.alShort;
                }
            }
        }

        /// <summary>
        /// ����ҽ������
        /// </summary>
        private string deptCode = "";

        /// <summary>
        /// ����ҽ�����ڿ���
        /// </summary>
        public string DeptCode
        {
            set
            {
                this.deptCode = value;
            }
        }

        /// <summary>
        /// ������Ĳ�ҩҽ����Ϣ
        /// </summary>
        ArrayList alOrder = new ArrayList();

        /// <summary>
        /// ������Ĳ�ҩҽ����Ϣ
        /// </summary>
        public ArrayList AlOrder
        {
            get
            {
                if (this.alOrder == null)
                    this.alOrder = new ArrayList();
                return this.alOrder;
            }
            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
            set
            {
                this.alOrder = value;
                this.SetValue(AlOrder);
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.RADT.PatientInfo patient = null;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo Patient
        {
            set
            {
                if (value == null)
                {
                    MessageBox.Show("������Ϣ��ֵ����");
                    return;
                }
                this.patient = value;
            }
        }
        //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// <summary>
        /// �������� A ���� M�޸�
        /// </summary>
        private string openType = string.Empty;

        public string OpenType
        {
            get { return openType; }
            set { openType = value; }
        }

        //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// <summary>
        /// ���滹��ȡ��
        /// </summary>
        private bool isCancel = true;
        //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        /// <summary>
        /// ���滹��ȡ��
        /// </summary>
        public bool IsCancel
        {
            get { return isCancel; }
            set { isCancel = value; }
        }

        #endregion

        #region �����
        /// <summary>
        /// ����ҽ������
        /// </summary>
        ArrayList alLong = null;

        /// <summary>
        /// ��ʱҽ������
        /// </summary>
        ArrayList alShort = null;

        Neusoft.FrameWork.Public.ObjectHelper orderTypeHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ��Ŀ��Ϣ
        /// </summary>
        ArrayList alItem = null;

        /// <summary>
        /// �÷���Ϣ
        /// </summary>
        ArrayList alUsage = null;

        /// <summary>
        /// ����Ƶ��
        /// </summary>
        Neusoft.FrameWork.Public.ObjectHelper frequencyHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// �����ҩ��ʽ
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.Pharmacy itemManager = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        #endregion

        #region {49026086-DCA3-4af4-A064-58F7479C324A}
        /// <summary>
        /// ��������ʹ��-ˢ�������б�
        /// </summary>
        public event RefreshGroupTree refreshGroup;
        #endregion

        /// <summary>
        /// ���ݼ��س�ʼ��
        /// </summary>
        protected void DataInit()
        {
            #region ҽ��������
            Neusoft.HISFC.BizProcess.Integrate.Manager integrateManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

            ArrayList alOrderType = (integrateManager.QueryOrderTypeList());//ҽ������
            foreach (Neusoft.HISFC.Models.Order.OrderType obj in alOrderType)
            {
                if (obj.IsDecompose)
                {
                    if (alLong == null)
                        alLong = new ArrayList();
                    alLong.Add(obj);
                }
                else
                {
                    if (alShort == null)
                        alShort = new ArrayList();
                    alShort.Add(obj);
                }
            }
            #endregion

            #region Ƶ�μ���
            //ArrayList List = integrateManager.QuereyFrequencyList();
            ArrayList List = Classes.Function.HelperFrequency.ArrayObject;//{57C91E41-6803-4c83-9B61-00F75FD04255}
            //this.cmbFrequency.DataSource = List;
            this.cmbFrequency.ShowID = true;
            this.cmbFrequency.AddItems(List);
            this.cmbFrequency.IsListOnly = true;
            //this.cmbFrequency.DisplayMember = "ID";
            //this.cmbFrequency.ValueMember = "ID";
            this.frequencyHelper.ArrayObject = List;
            #endregion

            #region ��ҩ��ʽ
            ArrayList memoAl = new ArrayList();
            Neusoft.FrameWork.Models.NeuObject obj1 = new Neusoft.FrameWork.Models.NeuObject();
            obj1.ID = "0";
            obj1.Name = "�Լ�";
            memoAl.Add(obj1);
            obj1 = new Neusoft.FrameWork.Models.NeuObject();
            obj1.ID = "1";
            obj1.Name = "����";
            memoAl.Add(obj1);
            obj1 = new Neusoft.FrameWork.Models.NeuObject();
            obj1.ID = "2";
            obj1.Name = "����";
            memoAl.Add(obj1);
            obj1 = new Neusoft.FrameWork.Models.NeuObject();
            obj1.ID = "3";
            obj1.Name = "������";
            memoAl.Add(obj1);
            this.cmbMemo.AddItems(memoAl);
            //this.cmbMemo.DataSource = memoAl;
            this.cmbMemo.DisplayMember = "Name";
            this.cmbMemo.ValueMember = "ID";
            this.cmbMemo.SelectedIndex = 0;
            #endregion

            #region ��ҩ��Ŀ
            if (this.alItem == null)
                this.alItem = new ArrayList();
            this.alItem = this.itemManager.QueryItemAvailableList(this.deptCode, "C");
            if (this.alItem == null)
            {
                MessageBox.Show("��ȡ��ҩ��Ŀ�б����");
                return;
            }
            #endregion

            #region �÷�
            this.alUsage = Classes.Function.HelperUsage.ArrayObject;
            if (this.alUsage == null)
            {
                MessageBox.Show("��ȡ�÷��б����!");
                return;
            }
            #endregion

            this.fpEnter1.SetWidthAndHeight(150, 100);
            this.fpEnter1.SetIDVisiable(this.fpEnter1_Sheet1, (int)ColumnSet.ColTradeName, false);
            this.fpEnter1.SetColumnList(this.fpEnter1_Sheet1, (int)ColumnSet.ColTradeName, this.alItem);

            this.fpEnter1.SetColumnList(this.fpEnter1_Sheet1, (int)ColumnSet.ColUsage, this.alUsage);

            this.fpEnter1.ShowListWhenOfFocus = true;
            this.fpEnter1.SetItem += new Neusoft.FrameWork.WinForms.Controls.NeuFpEnter.setItem(fpEnter1_SetItem);
            this.fpEnter1.KeyEnter += new Neusoft.FrameWork.WinForms.Controls.NeuFpEnter.keyDown(fpEnter1_KeyEnter);
            this.cmbFrequency.KeyPress += new KeyPressEventHandler(cmbFrequency_KeyPress);//{F5BE708C-7A46-40f9-A534-A81B454538F2}
            this.txtNum.KeyPress += new KeyPressEventHandler(txtNum_KeyPress);//{F5BE708C-7A46-40f9-A534-A81B454538F2}
            this.cmbMemo.KeyPress += new KeyPressEventHandler(cmbMemo_KeyPress);//{F5BE708C-7A46-40f9-A534-A81B454538F2}
            FarPoint.Win.Spread.InputMap im;

            im = this.fpEnter1.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Down, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.fpEnter1.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Up, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.fpEnter1.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Escape, Keys.None), FarPoint.Win.Spread.SpreadActions.None);
        }

        void cmbMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.fpEnter1.Focus();
                if (this.fpEnter1_Sheet1.RowCount == 0)
                {
                    this.fpEnter1_Sheet1.Rows.Add(0, 2);
                    this.fpEnter1_Sheet1.SetActiveCell(1, 0);
                    this.fpEnter1_Sheet1.Rows.Remove(0, 1);
                }
                else
                {
                    if (this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.Rows.Count - 1, 0].Text != "")
                    {
                        this.fpEnter1_Sheet1.Rows.Add(this.fpEnter1_Sheet1.Rows.Count, 2);
                        this.fpEnter1_Sheet1.SetActiveCell(this.fpEnter1_Sheet1.Rows.Count, 0);
                        this.fpEnter1_Sheet1.Rows.Remove(this.fpEnter1_Sheet1.Rows.Count - 1, 1);
                    }
                    else
                    {
                        this.fpEnter1_Sheet1.Rows.Remove(this.fpEnter1_Sheet1.Rows.Count - 1, 1);
                        this.fpEnter1_Sheet1.Rows.Add(this.fpEnter1_Sheet1.Rows.Count, 2);
                        this.fpEnter1_Sheet1.SetActiveCell(this.fpEnter1_Sheet1.Rows.Count, 0);
                        this.fpEnter1_Sheet1.Rows.Remove(this.fpEnter1_Sheet1.Rows.Count - 1, 1);
                    }
                }
            }
        }

        /// <summary>
        /// {F5BE708C-7A46-40f9-A534-A81B454538F2}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.cmbMemo.Focus();
            }
        }
        /// <summary>
        /// {F5BE708C-7A46-40f9-A534-A81B454538F2}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbFrequency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.txtNum.Focus();
            }
        }

        /// <summary>
        /// ���б��ȡ��ѡ����Ŀ
        /// </summary>
        /// <returns>�ɹ�����1 �����أ�1</returns>
        protected int GetSelectItem()
        {
            int currentRow = this.fpEnter1_Sheet1.ActiveRowIndex;
            if (currentRow < 0) return 0;
            if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColTradeName)
            {
                //��ȡѡ�е���Ϣ
                Neusoft.FrameWork.WinForms.Controls.PopUpListBox listBox = this.fpEnter1.getCurrentList(this.fpEnter1_Sheet1, (int)ColumnSet.ColTradeName);
                Neusoft.FrameWork.Models.NeuObject item = null;
                int rtn = listBox.GetSelectedItem(out item);
                if (item == null) return -1;
                this.SetSelectItem(item);
                return 0;
            }
            if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColUsage)
            {
                //��ȡѡ�е���Ϣ
                Neusoft.FrameWork.WinForms.Controls.PopUpListBox listBox = this.fpEnter1.getCurrentList(this.fpEnter1_Sheet1, (int)ColumnSet.ColUsage);
                Neusoft.FrameWork.Models.NeuObject item = null;
                int rtn = listBox.GetSelectedItem(out item);
                if (item == null) return -1;
                this.SetSelectItem(item);
                return 0;
            }
            return 0;
        }
        /// <summary>
        /// �������б�����ѡ�����Ŀ
        /// </summary>
        /// <param name="obj">�ɵ����б�����ѡ�����Ŀ</param>
        /// <returns>�ɹ�����1 �����أ�1</returns>
        protected int SetSelectItem(Neusoft.FrameWork.Models.NeuObject obj)
        {
            if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColTradeName)
            {
                Neusoft.HISFC.Models.Pharmacy.Item item = this.itemManager.GetItem(obj.ID);
                if (item == null)
                {
                    MessageBox.Show("��ȡҩƷ��Ϣʧ��!" + this.itemManager.Err);
                    return -1;
                }
                item.User02 = obj.User02;		//ȡҩҩ������
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColTradeName].Text = obj.Name;
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColSpecs].Text = item.Specs;
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColPrice].Text = item.PriceCollection.RetailPrice.ToString();
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUnit].Text = item.MinUnit;
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUsage].Text = item.Usage.Name;
                this.fpEnter1_Sheet1.Rows[this.fpEnter1_Sheet1.ActiveRowIndex].Tag = item;

                this.fpEnter1_Sheet1.ActiveColumnIndex = (int)ColumnSet.ColNum;
                return 1;
            }
            if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColUsage)
            {
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUsage].Text = obj.Name;
                this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUsage].Tag = obj;

                if (this.fpEnter1_Sheet1.ActiveRowIndex == this.fpEnter1_Sheet1.Rows.Count - 1)
                {
                    this.fpEnter1_Sheet1.Rows.Add(this.fpEnter1_Sheet1.Rows.Count, 1);
                    this.fpEnter1_Sheet1.ActiveRowIndex = this.fpEnter1_Sheet1.Rows.Count - 1;
                }
                else
                {
                    this.fpEnter1_Sheet1.ActiveRowIndex = this.fpEnter1_Sheet1.ActiveRowIndex + 1;
                }
                this.fpEnter1_Sheet1.ActiveColumnIndex = (int)ColumnSet.ColTradeName;
                return 1;
            }
            return 1;
        }
        /// <summary>
        /// ���������ʾ
        /// </summary>
        public void Clear()
        {
            this.alOrder = new ArrayList();
            this.fpEnter1_Sheet1.Rows.Count = 0;
            this.fpEnter1_Sheet1.Rows.Count = 1;
            this.txtNum.Text = "";
            this.dtEnd.Checked = false;
        }

        /// <summary>
        /// ҽ����Ч�Լ��
        /// </summary>
        /// <returns>�޴��󷵻�1 �����أ�1</returns>
        protected int Valid()
        {
            if (this.patient == null)
            {
                MessageBox.Show("������Ϣδ��ȷ��ֵ");
                return -1;
            }
            if (this.cmbOrderType.Text == "")
            {
                MessageBox.Show("��ѡ��ҽ������");
                this.cmbOrderType.Select();
                this.cmbOrderType.Focus();
                return -1;
            }
            if (this.cmbFrequency.Text == "" || this.cmbFrequency.Tag == null)//{57C91E41-6803-4c83-9B61-00F75FD04255}
            {
                MessageBox.Show("��ѡ�񱾼���ҩ��ҩƵ��");
                this.cmbFrequency.Text = string.Empty;
                this.cmbFrequency.Select();
                this.cmbFrequency.Focus();

                return -1;
            }
            if (this.txtNum.Text == "")
            {
                MessageBox.Show("��ѡ���ҩ����");
                this.txtNum.Select();
                this.txtNum.Focus();
                return -1;
            }
            if (Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtNum.Text) == 0)
            {
                MessageBox.Show("����ֻ��Ϊ����0������");
                return -1;
            }
            if (this.cmbMemo.Text == "")
            {
                MessageBox.Show("��ѡ�񱾼�ҩ�ü�ҩ��ʽ");
                this.cmbMemo.Select();
                this.cmbMemo.Focus();
                return -1;
            }
            if (this.dtEnd.Checked)
            {
                if (Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtBegin.Text) >= Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtEnd.Text))
                {
                    MessageBox.Show("ҽ��ֹͣʱ�䲻�ܴ��ڵ���ҽ����ʼʱ��");
                    return -1;
                }
            }
            for (int i = 0; i < this.fpEnter1_Sheet1.Rows.Count; i++)
            {
                if (this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColTradeName].Text == "")
                    continue;
                if (this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text == "")
                {
                    MessageBox.Show("�������" + (i + 1).ToString() + "�в�ҩÿ����");
                    this.fpEnter1_Sheet1.ActiveRowIndex = i;
                    return -1;
                }
                if (this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColUsage].Text == "")
                {
                    MessageBox.Show("�������" + (i + 1).ToString() + "�в�ҩ�÷�");
                    this.fpEnter1_Sheet1.ActiveRowIndex = i;
                    return -1;
                }
                #region addby xuewj 2010-03-22 {792D952F-1649-43bd-A12E-603F59AD3CDC} ÿ�μ��������ַ��ж�
                string colNum = this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text;
                if (colNum != Neusoft.FrameWork.Public.String.TakeOffSpecialChar(colNum))
                {
                    MessageBox.Show("��" + (i + 1).ToString() + "�в�ҩÿ�������������ַ�������������!");
                    this.fpEnter1_Sheet1.ActiveRowIndex = i;
                    return -1;
                }
                #endregion
            }
            return 1;
        }
        /// <summary>
        /// ҽ������
        /// </summary>
        protected int Save()
        {
            if (this.Valid() == -1)
                return -1;
            Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();

            string comboID = "";
            try
            {
                comboID = orderManager.GetNewOrderComboID();//�����Ϻ�;
            }
            catch (Exception ex)
            {
                MessageBox.Show("��ȡҽ����Ϻų���" + ex.Message);
                return -1;
            }
            Neusoft.FrameWork.Models.NeuObject usageObj = null;
            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
            this.AlOrder = new ArrayList();
            for (int i = 0; i < this.fpEnter1_Sheet1.Rows.Count; i++)
            {
                if (!this.isClinic)
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order order;

                    order = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                    order.Item = this.fpEnter1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Pharmacy.Item;
                    if (order.Item == null)
                        continue;
                    //������Ϣ
                    order.Patient = this.patient;
                    //ҽ����Ϻ�
                    order.Combo.ID = comboID;
                    //ҽ������
                    order.OrderType = this.orderTypeHelper.GetObjectFromID(this.cmbOrderType.SelectedValue.ToString()) as Neusoft.HISFC.Models.Order.OrderType;
                    //�÷�
                    usageObj = this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColUsage].Tag as Neusoft.FrameWork.Models.NeuObject;
                    order.Usage.ID = usageObj.ID;
                    order.Usage.Name = usageObj.Name;

                    //��λ  {AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                    order.Unit = (order.Item as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit;

                    //����
                    order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtNum.Text);
                    //��ҩ��ʽ
                    order.Memo = this.cmbMemo.Text;
                    //Ƶ��
                    order.Frequency = this.frequencyHelper.GetObjectFromID(this.cmbFrequency.Tag.ToString()) as Neusoft.HISFC.Models.Order.Frequency;//{57C91E41-6803-4c83-9B61-00F75FD04255}
                    //ÿ����
                    if (this.orderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                    {
                        order.DoseOnce = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text);
                    }
                    else
                    {
                        order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text);
                    }
                    order.BeginTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtBegin.Text);
                    if (this.dtEnd.Checked)
                        order.EndTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtEnd.Text);
                    //ȡҩҩ��
                    order.StockDept.ID = order.Item.User02;

                    this.alOrder.Add(order);
                }
                else if (this.isClinic)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order order;

                    order = new Neusoft.HISFC.Models.Order.OutPatient.Order();
                    order.Item = this.fpEnter1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Pharmacy.Item;
                    if (order.Item == null)
                        continue;
                    //������Ϣ
                    order.Patient = this.patient;
                    //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                    if (this.openType == "M") //�޸�
                    {
                        //ҽ����Ϻ�
                        order.Combo.ID = comboID;
                    }
                    else  //����
                    {
                        //ҽ����Ϻ�
                        order.Combo.ID = comboID;
                    }
                    //ҽ������
                    //order.OrderType = this.orderTypeHelper.GetObjectFromID(this.cmbOrderType.SelectedValue.ToString()) as Neusoft.HISFC.Models.Order.OrderType;
                    //�÷�
                    usageObj = this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColUsage].Tag as Neusoft.FrameWork.Models.NeuObject;
                    order.Usage.ID = usageObj.ID;
                    order.Usage.Name = usageObj.Name;

                    //��λ {AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                    order.Unit = (order.Item as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit;

                    //����
                    order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtNum.Text);
                    //��ҩ��ʽ
                    order.Memo = this.cmbMemo.Text;
                    //Ƶ��
                    order.Frequency = this.frequencyHelper.GetObjectFromID(this.cmbFrequency.Tag.ToString()) as Neusoft.HISFC.Models.Order.Frequency;//{57C91E41-6803-4c83-9B61-00F75FD04255}
                    //ÿ����
                    if (this.orderType == Neusoft.HISFC.Models.Order.EnumType.LONG)
                    {
                        order.DoseOnce = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text);
                    }
                    else
                    {
                        order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpEnter1_Sheet1.Cells[i, (int)ColumnSet.ColNum].Text);
                    }
                    order.BeginTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtBegin.Text);
                    if (this.dtEnd.Checked)
                        order.EndTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtEnd.Text);
                    //ȡҩҩ��
                    order.StockDept.ID = order.Item.User02;

                    this.alOrder.Add(order);
                }
            }
            return 1;
        }
        //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        protected int SetValue(ArrayList alHerbalOrder)
        {

            foreach (object obj in alHerbalOrder)
            {
                this.fpEnter1_Sheet1.AddRows(0, 1);
                if (obj.GetType().ToString() == "Neusoft.HISFC.Models.Order.OutPatient.Order") //����
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order order = obj as Neusoft.HISFC.Models.Order.OutPatient.Order;

                    Neusoft.HISFC.Models.Pharmacy.Item item = this.itemManager.GetItem(order.Item.ID);
                    if (item == null)
                    {
                        MessageBox.Show("��ȡҩƷ��Ϣʧ��!" + this.itemManager.Err);
                        return -1;
                    }

                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColTradeName].Text = item.Name; //obj.Name;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColSpecs].Text = item.Specs; //item.Specs;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColPrice].Text = item.PriceCollection.RetailPrice.ToString();
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUnit].Text = item.MinUnit;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUsage].Text = order.Usage.Name;
                    this.txtNum.Text = order.HerbalQty.ToString();
                    this.cmbMemo.Text = order.Memo.ToString();//{F5BE708C-7A46-40f9-A534-A81B454538F2}
                    //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                    //this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColNum].Text = order.Qty.ToString();
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColNum].Text = (order.Qty / order.HerbalQty).ToString();
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColTradeName].Text = order.Item.Name;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUsage].Tag = order.Usage;
                    this.cmbFrequency.Text = order.Frequency.ID;//ִ�п���
                    item.User02 = order.StockDept.ID;
                    this.fpEnter1_Sheet1.Rows[0].Tag = item;
                }
                else
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order order = obj as Neusoft.HISFC.Models.Order.Inpatient.Order;

                    Neusoft.HISFC.Models.Pharmacy.Item item = this.itemManager.GetItem(order.Item.ID);
                    if (item == null)
                    {
                        MessageBox.Show("��ȡҩƷ��Ϣʧ��!" + this.itemManager.Err);
                        return -1;
                    }

                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColTradeName].Text = item.Name; //obj.Name;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColSpecs].Text = item.Specs; //item.Specs;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColPrice].Text = item.PriceCollection.RetailPrice.ToString();
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUnit].Text = item.MinUnit;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUsage].Text = order.Usage.Name;
                    this.txtNum.Text = order.HerbalQty.ToString();
                    this.cmbMemo.Text = order.Memo.ToString();//{F5BE708C-7A46-40f9-A534-A81B454538F2}
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColNum].Text = order.Qty.ToString();
                    //this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColNum].Text = (order.Qty / order.HerbalQty).ToString();
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColTradeName].Text = order.Item.Name;
                    this.fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUsage].Tag = order.Usage;
                    this.cmbFrequency.Text = order.Frequency.ID;//ִ�п���
                    item.User02 = order.StockDept.ID;
                    this.fpEnter1_Sheet1.Rows[0].Tag = item;
                }
            }
            return 1;
        }


        #region �¼�
        private void ucHerbalOrder_Load(object sender, EventArgs e)
        {
            this.fpEnter1.Select();
            this.fpEnter1.Focus();
            this.fpEnter1_Sheet1.ActiveColumnIndex = (int)ColumnSet.ColTradeName;

            //{1BC3713E-0307-44df-80EB-44288BB06727}
            if (this.ParentForm != null)
            {
                this.ParentForm.ControlBox = false;
            }
        }

        private int fpEnter1_KeyEnter(Keys key)
        {
            if (key == Keys.Enter)
            {
                if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColTradeName)
                {
                    if (this.GetSelectItem() == -1)
                    {
                        MessageBox.Show("���б��ȡ��ѡ����Ŀ����");
                        return -1;
                    }
                    return 1;
                }
                if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColNum)
                {
                    //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
                    if (fpEnter1_Sheet1.RowCount > 0 && fpEnter1_Sheet1.ActiveRowIndex > 0)
                    {//���ǵ�һ�У��͵�һ��һ��
                        Neusoft.FrameWork.Models.NeuObject obj = fpEnter1_Sheet1.Cells[0, (int)ColumnSet.ColUsage].Tag as Neusoft.FrameWork.Models.NeuObject;
                        if (obj == null)
                        {
                            MessageBox.Show("�������һ�е��÷���");
                            return 0;
                        }
                        this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUsage].Text = obj.Name;
                        this.fpEnter1_Sheet1.Cells[this.fpEnter1_Sheet1.ActiveRowIndex, (int)ColumnSet.ColUsage].Tag = obj;

                        if (this.fpEnter1_Sheet1.ActiveRowIndex == this.fpEnter1_Sheet1.Rows.Count - 1)
                        {
                            this.fpEnter1_Sheet1.Rows.Add(this.fpEnter1_Sheet1.Rows.Count, 1);
                            this.fpEnter1_Sheet1.ActiveRowIndex = this.fpEnter1_Sheet1.Rows.Count - 1;
                        }
                        else
                        {
                            this.fpEnter1_Sheet1.ActiveRowIndex = this.fpEnter1_Sheet1.ActiveRowIndex + 1;
                        }
                        this.fpEnter1_Sheet1.ActiveColumnIndex = (int)ColumnSet.ColTradeName;
                        return 1;
                    }
                    this.fpEnter1_Sheet1.ActiveColumnIndex = (int)ColumnSet.ColUsage;
                    return 1;
                }
                if (this.fpEnter1_Sheet1.ActiveColumnIndex == (int)ColumnSet.ColUsage)
                {
                    if (this.GetSelectItem() == -1)
                    {
                        MessageBox.Show("���б��ȡ��ѡ����Ŀ����");
                        return -1;
                    }
                }
            }
            return 0;
        }

        private int fpEnter1_SetItem(Neusoft.FrameWork.Models.NeuObject obj)
        {
            if (this.SetSelectItem(obj) == -1)
            {
                MessageBox.Show("������ѡ����Ŀʧ��");
            }
            return 0;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.IsCancel = false;
            if (this.Save() == 1)
            {
                if (this.ParentForm != null)
                {
                    this.ParentForm.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
            this.AlOrder = new ArrayList();
            if (this.ParentForm != null)
            {
                this.ParentForm.Close();
            }
        }

        private void btnDel_Click(object sender, System.EventArgs e)
        {
            if (this.fpEnter1_Sheet1.Rows.Count > 0)
            {
                this.fpEnter1_Sheet1.RemoveRows(this.fpEnter1_Sheet1.ActiveRowIndex, 1);
                this.fpEnter1.SetAllListBoxUnvisible();
            }
        }

        #endregion

        #region ������
        private enum ColumnSet
        {
            /// <summary>
            /// ҩƷ����
            /// </summary>
            ColTradeName,
            /// <summary>
            /// ���
            /// </summary>
            ColSpecs,
            /// <summary>
            /// �۸�
            /// </summary>
            ColPrice,
            /// <summary>
            /// ����
            /// </summary>
            ColNum,
            /// <summary>
            /// ��λ
            /// </summary>
            ColUnit,
            /// <summary>
            /// �÷�
            /// </summary>
            ColUsage
        }
        #endregion

        //{D42BEEA5-1716-4be4-9F0A-4AF8AAF88988}
        private void btnNewRecipe_Click(object sender, EventArgs e)
        {
            this.Clear();
            this.OpenType = "A";
        }

        /// <summary>
        /// ������
        /// {DC0E8BDB-D918-4c14-8474-3D2E6F986A57}
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveGroup_Click(object sender, EventArgs e)
        {
            if (this.Save() == 1)
            {
                Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager group = new Neusoft.HISFC.Components.Common.Forms.frmOrderGroupManager();
                if (this.isClinic)
                {
                    group.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.C;
                }
                else
                {
                    group.InpatientType = Neusoft.HISFC.Models.Base.ServiceTypes.I;
                }

                try
                {
                    group.IsManager = (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).IsManager;
                }
                catch
                { }

                if (this.alOrder.Count > 0)
                {
                    group.alItems = this.alOrder;
                    group.ShowDialog();
                }

                #region {49026086-DCA3-4af4-A064-58F7479C324A}
                this.refreshGroup();
                #endregion
            }
        }

    }
}
