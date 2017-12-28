using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ����Ŀѡ��ؼ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucItemSelect : UserControl
    {
        public ucItemSelect()
        {
            InitializeComponent();
            
        }
        public event Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler CatagoryChanged;
        #region ��ʼ��
        public void Init()
        {
            if (DesignMode) return;
            if (Neusoft.FrameWork.Management.Connection.Operator.ID == "") return;

            #region ����tip
            tooltip.SetToolTip(this.ucInputItem1, "����ƴ�����ѯ������ҽ��(ESCȡ���б�)");
            tooltip.SetToolTip(this.txtDays, "����ҽ��ִ������");
            tooltip.SetToolTip(this.txtQuantity, "����������(�س��������)");
            tooltip.SetToolTip(this.dtBegin, "����ҽ����ʼִ��ʱ��");
            tooltip.SetToolTip(this.dtEnd, "����ҽ������ִ��ʱ��");
            #endregion
            try
            {
                Neusoft.HISFC.Models.Base.Employee p = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
                if (p == null) return;
                this.ucInputItem1.DeptCode = p.Dept.ID;//���ҿ��Լ����ҵ�ҩƷ��Ŀ
                this.ucInputItem1.ShowCategory = Neusoft.HISFC.Components.Common.Controls.EnumCategoryType.SysClass;

                this.ucOrderInputByType1.ItemSelected += new ItemSelectedDelegate(ucOrderInputByType1_ItemSelected);
                this.cmbOrderType1.SelectedIndexChanged += new System.EventHandler(this.cmbOrderType1_SelectedIndexChanged);

                this.ucInputItem1.SelectedItem += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucInputItem1_SelectedItem);
                this.ucInputItem1.CatagoryChanged += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucInputItem1_CatagoryChanged);
                this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
                this.txtQuantity.Leave += new EventHandler(txtQuantity_Leave);

                this.cmbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUnit_KeyPress);
                //this.cmbUnit.TextChanged+=new EventHandler(cmbUnit_TextChanged);    //{8670585E-EE96-47db-86FE-FA2F81EBF459}  ���¼�ѡ��ʱ������������ucInputItem1_SelectedItem���� 20100915
                this.ucOrderInputByType1.Leave += new EventHandler(ucOrderInputByType1_Leave);
                this.ucOrderInputByType1.InitControl(null, null, null);
                this.dtBegin.ValueChanged += new System.EventHandler(this.dtBegin_ValueChanged);
                this.dtEnd.ValueChanged += new EventHandler(dtEnd_ValueChanged);

                this.cmbOrderType1.DropDownStyle = ComboBoxStyle.DropDownList;
                this.ucInputItem1.Init();//��ʼ����Ŀ�б�
            }
            catch { }
            try
            {
                Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                ArrayList alOrderType = manager.QueryOrderTypeList();
                if (alOrderType == null) return;
                //ҽ�����ͻ�û��
                foreach (Neusoft.HISFC.Models.Order.OrderType obj in alOrderType)
                {
                    if (obj.IsDecompose)
                    {
                        alLong.Add(obj);
                    }
                    else
                    {

                        alShort.Add(obj);
                    }
                }
                SetLongOrShort(false);

                ArrayList alResQuality = manager.QueryConstantList("LongOrderResQuality");
                if (alResQuality != null)
                {
                    this.hsResQuality = new Hashtable();
                    foreach (Neusoft.FrameWork.Models.NeuObject info in alResQuality)
                    {
                        this.hsResQuality.Add(info.ID, null);
                    }
                }
            }
            catch { }           
            this.dtEnd.MinDate = DateTime.MinValue;
            this.dtEnd.Value = DateTime.Today.AddDays(1);
            this.dtEnd.Checked = false;
        }
        #endregion

        #region ����
        /// <summary>
        /// ҽ���仯ʱ����
        /// </summary>
        public event ItemSelectedDelegate OrderChanged;//

        /// <summary>
        /// ��ǰ��������
        /// </summary>
        public Operator OperatorType = Operator.Query;

        public int CurrentRow = -1; //��ǰ��
        /// <summary>
        /// �Ƿ���ʾLis��ϸ��Ϣ
        /// </summary>
        public bool IsLisDetail
        {
            set
            {
                this.isLisDetail = value;
            }
        }
        /// <summary>
        /// �Ƿ�������ױ༭����
        /// </summary>
        public bool EditGroup = false;

        protected ArrayList alLong = new ArrayList();//����ҽ������
        protected ArrayList alShort = new ArrayList();//��ʱҽ������        
        protected bool dirty = false;//���µ�ʱ��
        protected ToolTip tooltip = new ToolTip(); //ToolTip
        protected bool isLisDetail = false;
        protected Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmacyManager = null;
        protected Neusoft.HISFC.BizProcess.Integrate.Fee itemManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ����ҽ��������ҩƷ����
        /// </summary>
        System.Collections.Hashtable hsResQuality = new Hashtable();

        //{7F57E64E-D49E-4b3a-9E41-AF668543ECE7}�κ�ҽ���������޸Ŀ�ʼʱ�� by guanyx
        //�����Ƿ�����޸Ŀ�ʼʱ��
        private bool canModifyStartTime = true;

        #endregion

        #region ����
        /// <summary>
        /// ��ǰҽ��
        /// </summary>
        protected Neusoft.HISFC.Models.Order.Inpatient.Order order = null;

        /// <summary>
        /// ��ǰҽ��
        /// </summary>
        [DefaultValue(null)]
        public Neusoft.HISFC.Models.Order.Inpatient.Order Order
        {
            get
            {
                return this.order;
            }
            set
            {
                if (value == null) return;
                this.order = value;
                #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
                if (this.isNurseCreate)
                {
                    if (this.order.ReciptDoctor.ID != Neusoft.FrameWork.Management.Connection.Operator.ID)
                    {
                        MessageBox.Show("��ʿ�������޸����˿�����ҽ��!");
                        return ;
                    }
                }
                #endregion
                dirty = false; //���Ǳ仯ʱ��--����ʱ��
                
                    this.LongOrShort = (int)this.order.OrderType.Type;
                    this.ucOrderInputByType1.IsNew = false;//�޸ľ�ҽ��
                    this.ucOrderInputByType1.Order = value;

                    this.ucInputItem1.FeeItem = this.order.Item;
                    this.cmbOrderType1.Tag = this.order.OrderType.ID;
                    ReadOrder(this.order);//��������ҽ��
                
                dirty = true;
            }
        }

        protected int longOrShort = 0;

        /// <summary>
        /// ���� 0 or��ʱҽ�� 1
        /// </summary>
        public int LongOrShort
        {
            get
            {
                return longOrShort;
            }
            set
            {
                if (DesignMode) return;
                if (longOrShort == value) return;
                if (value == 0)
                {
                    this.SetLongOrShort(false);
                    
                }
                else
                {
                    this.SetLongOrShort(true);   
                }
                longOrShort = value;
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

        #region �¼�
        protected bool bPermission = false;//�Ƿ�֪��ͬ����
     

        protected void ShowTotal(bool b)
        {
            this.label3.Enabled = b;
            this.txtQuantity.Enabled = b;
            this.cmbUnit.Enabled = b;
        }
        
        /// <summary>
        /// ҽ���仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="changedField"></param>
        protected virtual void ucOrderInputByType1_ItemSelected(Neusoft.HISFC.Models.Order.Inpatient.Order order, EnumOrderFieldList changedField)
        {
            dirty = true;
            //��ʱҽ��,��Ժ����ٴ�ҩ��ҩƷ������Ϊ�����Ŀ�Զ�����-2005-6-1�¼�Ϊ����ɽһ
            //if (order.OrderType.IsDecompose == false 
            //    && order.Item.IsPharmacy && order.Frequency.ID != "" &&
            //    Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtDays.Text) > 0)
            
            if (order.OrderType.IsDecompose == false
                && order.Item.ItemType == EnumItemType.Drug && order.Frequency.ID != "" &&
                Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtDays.Text) > 0)
            {
                this.txtQuantity.Text = order.Qty.ToString();
                this.cmbUnit.Text = ((Neusoft.HISFC.Models.Pharmacy.Item)order.Item).MinUnit;
            }
            //{49C1DF42-7050-42b1-B418-BD4A2D733E83}
            //this.dtEnd.Checked = false;
            
            this.myOrderChanged(order,changedField);
            dirty = false;
        }

        protected void myOrderChanged(object sender,EnumOrderFieldList enumOrderFieldList)
        {
            try
            {
                if (this.CurrentRow == -1)
                {
                    this.CurrentRow = 0;
                    this.OperatorType = Operator.Add;//���
                }
                else
                {
                    this.OperatorType = Operator.Modify;
                    
                }

                #region {A3772F6F-C68D-4987-AF2F-FA1A32208488}
                //�˴�Ӧ����order����������Order��ʹ������Order��ִ��һϵ�еĴ��룬���ܵ��´���
                //this.Order = sender as Neusoft.HISFC.Models.Order.Inpatient.Order;//�ؼ������Ķ���
                this.order = sender as Neusoft.HISFC.Models.Order.Inpatient.Order;//�ؼ������Ķ���
                #region {7ED5BB10-74B0-4cfc-9D39-4C7E18E0465C}
                this.ucOrderInputByType1.IsNew = false;//�޸ľ�ҽ��
                this.ucOrderInputByType1.Order = this.order;
                #endregion
                #endregion

                this.OrderChanged(order, enumOrderFieldList);
            }
            catch { }
        }
        /// <summary>
        /// �����仯-������һ�������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.order == null) return;
            if ( e==null || e.KeyChar == 13 )
            {
                if (this.order.Qty != Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQuantity.Value))
                {
                    this.order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQuantity.Value);
                    myOrderChanged(this.order, EnumOrderFieldList.Qty);
                }
                if (this.cmbUnit.Enabled)
                {
                    this.cmbUnit.Focus();
                }
                else
                {
                    //if (this.order.Item.IsPharmacy == false)//��ҩƷ���� �¼�
                    if (this.order.Item.ItemType != EnumItemType.Drug)//��ҩƷ���� �¼�
                        this.ucInputItem1.Focus();
                    else
                        this.ucOrderInputByType1.Focus();
                }
            }
        }
        /// <summary>
        /// ��λkeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.order == null) return;
            if (e == null || e.KeyChar == 13)
            {
                    //if (this.order.Item.IsPharmacy == false)//��ҩƷ���� �¼�
                #region addby xuewj 2010-9-24 ��ҩƷ����������ת {1B93C17C-DB7D-44cc-98AE-2E76DB0532F6}
                //if (this.order.Item.ItemType != EnumItemType.Drug)//��ҩƷ���� �¼�
                //{
                //    if (this.order.Item.IsNeedConfirm)//{31EB54C5-C30E-4130-89F2-BBF57D6BAFF6}
                //    {
                //        this.ucOrderInputByType1.Focus();
                //    }
                //    else
                //    {
                //        this.ucInputItem1.Focus();
                //    }
                //}
                //else
                this.ucOrderInputByType1.Focus(); 
                #endregion
             
            }
        }
        private void txtQuantity_Leave(object sender, EventArgs e)
        {
            if (this.order == null) return;
            if (this.order.Qty == Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQuantity.Value)) return;
            this.txtQuantity_KeyPress(sender, null);

        }

        //{7F57E64E-D49E-4b3a-9E41-AF668543ECE7}�κ�ҽ���������޸Ŀ�ʼʱ�� by guanyx
        /// <summary>
        /// �ж��޸ĵĿ�ʼʱ�����Ч�ԣ���ֻ�����޸�ʱ�䣬�������޸����ڣ�
        /// </summary>
        /// <param name="dt"></param>
        private int CheckBeginTimeValid(DateTime dt,int i)
        {
            Neusoft.HISFC.BizLogic.Order.Order manger = new Neusoft.HISFC.BizLogic.Order.Order();
            DateTime sysdate = Convert.ToDateTime(manger.GetSysDateTime());
            if (dt.Date < sysdate.AddDays(i ).Date)
            {
                this.dtBegin.Value = sysdate;
                MessageBox.Show("��ʼʱ����Ч���벻Ҫ�޸ĵ�ʱ��̫����");
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private void dtBegin_ValueChanged(object sender, System.EventArgs e)
        {
            if (dirty == true)
            {
                if (this.order == null)
                {
                    return;
                }
                //{7F57E64E-D49E-4b3a-9E41-AF668543ECE7}�κ�ҽ���������޸Ŀ�ʼʱ�� by guanyx
                if (this.canModifyStartTime == false)
                {
                    //��ʱʹ�ñ���ʱ���ж�
                    //Ӧ���ڴ˴���ȡϵͳʱ�� ������Ч������ ��ʹ�ñ���ʱ���ж�
                    //ֻ�в�¼ҽ���������ÿ���ʱ��С�ڵ�ǰʱ��
                    //Edit By liangjz 
                    if (this.dtBegin.Value < DateTime.Now && this.order.OrderType.ID != "BL")
                    {
                        this.dtBegin.Value = this.order.BeginTime;
                        return;
                    }
                }
                int days = 0;
                if (this.order.OrderType.ID == "BL")
                {
                    days = -10;
                }
                else
                {
                    days = 0;
                }
                if (this.CheckBeginTimeValid(this.dtBegin.Value, days) == -1)
                {
                    return;
                }
                if (this.order.BeginTime != this.dtBegin.Value)
                {
                    this.dtBegin.Value = new DateTime(this.dtBegin.Value.Year, this.dtBegin.Value.Month, this.dtBegin.Value.Day, this.dtBegin.Value.Hour, this.dtBegin.Value.Minute, 0);//{8FEB04B3-0A07-4893-A5B8-829D8ADC468B}
                    this.order.BeginTime = this.dtBegin.Value;                    
                    myOrderChanged(this.order, EnumOrderFieldList.BeginDate);
                }
            }
              
        }

        private Panel PanelEnd
        {
            get
            {
                return this.panelEndDate;
            }
        }

        /// <summary>
        /// {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
        /// ����ϵͳ���
        /// </summary>
        /// <param name="isShort"></param>
        /// <param name="alSysClass"></param>
        /// <returns></returns>
        private ArrayList FilterSysClassForNurse(bool isShort, ArrayList alSysClass)
        {
            System.Collections.ArrayList al = Neusoft.HISFC.Models.Base.SysClassEnumService.List();
            Neusoft.FrameWork.Models.NeuObject objAll = new Neusoft.FrameWork.Models.NeuObject();
            objAll.ID = "ALL";
            objAll.Name = "ȫ��";
            al.Add(objAll);
            
            //��ʿҽ������Щ����

            System.Collections.ArrayList rAl = new ArrayList();
            foreach (Neusoft.FrameWork.Models.NeuObject obj in al)
            {
                if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "MR")//��ҩƷ��ת�ƣ�ת��
                {

                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "UO")//����
                {
                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "UC")//���
                {
                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "UL")	//����
                {
                }
                else if (obj.ID.Length >= 1 && obj.ID.Substring(0, 1) == "P")//ҩ
                {
                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "MC")//����
                {
                }
                else
                {
                    rAl.Add(obj);
                }
            }
            return rAl;
        }

        /// <summary>
        /// ����ҽ������
        /// </summary>
        /// <param name="b"></param>
        protected void SetLongOrShort(bool isShort)
        {
            dirty = false;

            //����ҽ��ֹͣ����
            this.PanelEnd.Visible = !isShort;
            //.
            this.panelEndDay.Visible = !isShort;

            if (isShort) //��ʱҽ��
            {
                this.cmbOrderType1.AddItems(alShort);
                #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
                if (this.isNurseCreate)
                {
                    this.ucInputItem1.AlCatagory = this.FilterSysClassForNurse(isShort, Classes.Function.OrderCatatagory(isShort));
                }
                else
                {
                    this.ucInputItem1.AlCatagory = Classes.Function.OrderCatatagory(isShort);
                }
                #endregion

            }//����
            else
            {
                this.cmbOrderType1.AddItems(alLong);//��ӳ���ҽ�����
                #region {2A5F9B85-CA08-4476-A5A4-56F34F0C28AC}
                if (this.isNurseCreate)
                {
                    this.ucInputItem1.AlCatagory = this.FilterSysClassForNurse(isShort, Classes.Function.OrderCatatagory(isShort));
                }
                else
                {
                    this.ucInputItem1.AlCatagory = Classes.Function.OrderCatatagory(isShort);
                }
                #endregion
            }
            try
            {
                this.cmbOrderType1.SelectedIndex = 0;
            }
            catch { }
        }
        /// <summary>
        /// ҽ�����ͱ仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOrderType1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.cmbOrderType1.SelectedIndex < 0) return;
            Neusoft.HISFC.Models.Order.OrderType obj = null;
            if (this.LongOrShort == 0) //����ҽ��
            {
                obj = this.alLong[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;
            }
            else //��ʱҽ��
            {
                obj = this.alShort[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;
                //��Ժ��ҩ����ٴ�ҩ������Ҫ��������
            }
        
            if (obj.IsCharge == false)
            {
                this.ucInputItem1.IsCanInputName = false;
            }
            else
            {
                this.ucInputItem1.IsCanInputName = true;
            }

            this.ucInputItem1.Focus();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charge"></param>
        private void GeChargeableOrderType(bool charge)
        {
            //�жϵ�ǰҽ���շ�����
            Neusoft.HISFC.Models.Order.OrderType ordertype = this.cmbOrderType1.SelectedItem as Neusoft.HISFC.Models.Order.OrderType;
            if (ordertype != null)
            {
                if (ordertype.IsCharge == charge)
                    return;
            }
            //�����ϣ����ҵ�һ�����ϵ��շ�����
            foreach (Neusoft.HISFC.Models.Order.OrderType obj in this.cmbOrderType1.alItems)
            {
                if (obj.IsCharge == charge)
                {
                    this.cmbOrderType1.Tag = obj.ID;
                    return;
                }
            }
        }

        #region ��������
        //{8670585E-EE96-47db-86FE-FA2F81EBF459}  ���¼�ѡ��ʱ������������ucInputItem1_SelectedItem���� 20100915
        //private void cmbUnit_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string unit = this.cmbUnit.Text.Trim();
        //        if (Neusoft.FrameWork.Public.String.ValidMaxLengh(unit, 16) == false)
        //        {
        //            MessageBox.Show("��λ����!", "��ʾ");
        //            return;
        //        }
        //        if (this.order.Unit != unit && dirty == true)
        //        {
        //            this.order.Unit = unit;//���µ�λ
        //            myOrderChanged(this.order, EnumOrderFieldList.Unit);
        //        }
        //    }
        //    catch { }
        //}
        #endregion

        /// <summary>
        /// ��ǰѡ���ҽ������
        /// </summary>
        public Neusoft.HISFC.Models.Order.OrderType SelectedOrderType
        {
            get
            {
                return this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;
            }
        }

        /// <summary>
        /// ��ʼʱ��仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtBegin_ValueChanged_1(object sender, System.EventArgs e)
        {
            //��This.orderΪnullʱ�������жϴ��� Add By liangjz 2005-08
            if (this.order == null) return;
            if (this.txtQuantity.Text == "")
                return;
            //{7F57E64E-D49E-4b3a-9E41-AF668543ECE7}�κ�ҽ���������޸Ŀ�ʼʱ�� by guanyx
            if (this.canModifyStartTime == false)
            {
                //****************������Ҫ���ĵĵط�*************************
                //ֻ�в�¼ҽ�����Ե���ʱ��С�ڵ�ǰʱ��  BL ��¼ҽ��
                if (this.dtBegin.Value.Date < DateTime.Today.Date && this.order.OrderType.ID != "BL")
                {
                    this.dtBegin.Value = System.DateTime.Today.Date;
                    return;
                }
            }
            if (this.dtEnd.Value <= this.dtBegin.Value && this.dtEnd.Checked)
            {
                this.dtBegin.Value = this.dtEnd.Value;
                return;
            }
            try
            {
                this.order.BeginTime = this.dtBegin.Value;//��ʼʱ��
                myOrderChanged(this.order,EnumOrderFieldList.BeginDate);
            }
            catch { }
        }

        /// <summary>
        /// ֹͣʱ��仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            //��This.orderΪnullʱ�������жϴ��� Add By liangjz 2005-08
            if (this.order == null) return;
            
            if (this.txtQuantity.Text == "")
                return;

            if (this.dtEnd.Value.Date <= this.dtBegin.Value.Date && this.dtEnd.Checked)
            {
                this.dtEnd.Value = this.dtBegin.Value;

                return;
            }
            try
            {
                if (this.dtEnd.Checked == false)
                {
                    this.order.EndTime = System.DateTime.MinValue;
                }
                else
                {
                    this.order.EndTime = this.dtEnd.Value;//ֹͣʱ��
                }
                dirty = true;
                myOrderChanged(this.order,EnumOrderFieldList.EndDate);
                dirty = false;
            }
            catch { }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDays_TextChanged(object sender, System.EventArgs e)
        {
            //try
            //{
            //    if (this.txtDays.Text == "" || int.Parse(this.txtDays.Text) < 1) return;
            //    if (this.SelectedOrderType.IsDecompose == true)
            //    {
            //        this.dtEnd.Value = this.dtBegin.Value.AddDays(int.Parse(this.txtDays.Text));
            //    }
            //    else
            //    {
            //        //��ʱҽ��,��Ժ����ٴ�ҩ��ҩƷ������Ϊ�����Ŀ�Զ�����-2005-6-1�¼�Ϊ����ɽһ
            //        if (order.OrderType.IsDecompose == false && (order.OrderType.ID == "CD" || order.OrderType.ID == "QL")
            //            && order.Item.isPharmacy && order.Frequency.ID != "" &&
            //            Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtDays.Text) > 0)//&& this.txtQuantity.Enabled==false
            //        {
            //            Neusoft.HISFC.Models.Pharmacy.Item item = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
            //            #region ���ʱ���
            //            if (order.Frequency.Usage.ID == "") order.Frequency.Usage = order.Usage.Clone();
            //            string DeptCode = order.ReciptDept.ID;//��������
            //            Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManagement = new Neusoft.HISFC.BizLogic.Manager.Frequency();
            //            order.Frequency = (Neusoft.HISFC.Models.Order.Frequency)frequencyManagement.Get(order.Frequency, DeptCode);//���ʱ���
            //            if (order.Frequency == null)
            //            {
            //                MessageBox.Show(frequencyManagement.Err);
            //                return;
            //            }
            //            Neusoft.HISFC.Models.Order.Frequency f = frequencyManagement.GetDfqspecial(order.ID, order.Combo.ID);
            //            if (f != null) order.Frequency = f.Clone();
            //            int days = Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtDays.Text);
            //            if (days == 0) days = 1;
            //            #endregion
            //            if (item.OnceDose == 0M)//һ�μ���Ϊ�㣬Ĭ����ʾ��������
            //                order.Qty = item.BaseDose / item.BaseDose * order.Frequency.Times.Length * days;
            //            else
            //                order.Qty = item.OnceDose / item.BaseDose * order.Frequency.Times.Length * days;

            //            this.txtQuantity.Text = order.Qty.ToString();
            //            this.cmbUnit.Text = item.MinUnit;
            //        }

            //    }
            //}
            //catch { }
        }
        private void dtEnd_CloseUp(object sender, System.EventArgs e)
        {
            if (this.dtEnd.Value.Date <= this.dtBegin.Value.Date && this.dtEnd.Checked)
            {
                MessageBox.Show("ҽ����ֹʱ�䲻��С����ʼʱ�䣬�����", "��ʾ");
            }

        }

        void ucInputItem1_CatagoryChanged(Neusoft.FrameWork.Models.NeuObject sender)
        {
            try
            {
                Neusoft.FrameWork.Models.NeuObject obj = sender;
                if (obj.ID.Length > 0 && obj.ID.Substring(0, 1) == "M")
                {
                    GeChargeableOrderType(false);
                }
                else
                {
                    GeChargeableOrderType(true);
                }
            }
            catch { }
            if (CatagoryChanged != null) CatagoryChanged(sender);
        }

        void ucOrderInputByType1_Leave(object sender, EventArgs e)
        {
            this.ucInputItem1.Focus();
        }

        void ucInputItem1_SelectedItem(Neusoft.FrameWork.Models.NeuObject sender)
        {

            if (this.ucInputItem1.FeeItem == null) return;
            if (!this.EditGroup)		//��ʵ�ֶ������޸Ĺ���ʱ �����֪��ͬ����������ж�
            {
                //�жϵ�ǰ�������Ŀ�Ƿ�֪��ͬ����
                this.bPermission = Classes.Function.IsPermission(this.patientInfo,
                    (Neusoft.HISFC.Models.Order.OrderType)this.cmbOrderType1.SelectedItem,
                    (Neusoft.HISFC.Models.Base.Item)this.ucInputItem1.FeeItem);
            }

            if (this.order != null && this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Base.Item == this.order.Item) //���ظ�
            {
                this.txtQuantity.Focus();
                return;
            }

            //��Ŀ�仯-ָ������
            this.CurrentRow = -1;

            this.OperatorType = Operator.Add;

            //������ҽ��
            this.SetOrder();

            //�����仯
            if (this.txtQuantity.Enabled)
            {
                this.txtQuantity.Focus();

            }
            else
            {
                this.ucOrderInputByType1.Focus();
            }

        }
        #endregion

        #region ����
        /// <summary>
        /// ��ȡҽ����Ϣ-���ƿؼ���ʾ״̬
        /// </summary>
        /// <param name="myOrder"></param>
        protected int ReadOrder(Neusoft.HISFC.Models.Order.Order myOrder)
        {
            if (myOrder == null) return 0;
            
            //��Ŀ
            if (myOrder.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
            {
                Neusoft.HISFC.Models.Pharmacy.Item item = ((Neusoft.HISFC.Models.Pharmacy.Item)myOrder.Item);

                if (this.LongOrShort == 0) //����ҽ��������ʾ����
                {
                    this.ShowTotal(false);
                }
                else
                {
                    //ҩƷ ��ʱҽ����Ƶ��Ϊ�գ�Ĭ��Ϊ��Ҫʱ�����prn
                    if (myOrder.Frequency.ID == null || myOrder.Frequency.ID == "")
                        myOrder.Frequency.ID = "PRN";//��ʱҽ��Ĭ��Ϊ��Ҫʱִ��

                    this.ShowTotal(true);
                }

                this.txtQuantity.Text = myOrder.Qty.ToString(); //����
                this.cmbUnit.Items.Clear();

                if (myOrder.Item.ID != "999") //�Զ���ҩƷ
                {
                    if (item.PackQty == 0)//����װ����
                    {
                        MessageBox.Show("��ҩƷ�İ�װ����Ϊ�㣡");
                        return -1;
                    }
                    if (item.BaseDose == 0)//����������
                    {
                        MessageBox.Show("��ҩƷ�Ļ�������Ϊ�㣡");
                        return -1;
                    }
                    if (item.DosageForm.ID == "")//������
                    {
                        MessageBox.Show("��ҩƷ�ļ���Ϊ�գ�");
                        return -1;
                    }
                }
                //��λ
                if ((myOrder.Item as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit != "" && (myOrder.Item as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit != null)//��װ��λ��Ϊ��
                {
                    try
                    {
                        //{8670585E-EE96-47db-86FE-FA2F81EBF459} ���޸İ�װ��λ������С��λ����һ��������Ƶ�λ��ʾ 20100915
                        //#region ���ΰ�װ��λ��ʾ ֻ��ʾ��С��λ �޷��޸ĵ�λ
                        //this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit);//��λ
                        //this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit);//��λ
                        //this.cmbUnit.Enabled = false;  
                        //#endregion
                        //�˴����ư�װ��λ����С��λ�����ʾ
                        //���б�Ҫ�ɷḻ�ж�����
                        if (myOrder is Neusoft.HISFC.Models.Order.Inpatient.Order)
                        {
                            if (item.SysClass.ID.ToString() == "PCZ")
                            {//�г�ҩ,ֻ��ʾ��װ��λ�����Ҳ���ѡ��
                                this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit);//��λ
                                this.cmbUnit.Enabled = false;
                            }
                            if (item.SysClass.ID.ToString() == "PCC")
                            {//�в�ҩĬ����С��λ
                                this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit);//��λ
                                this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit);//��λ
                                this.cmbUnit.Enabled = true;
                            }
                            else
                            {//����Ĭ�ϰ�װ��λ
                                this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit);//��λ
                                this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit);//��λ
                                this.cmbUnit.Enabled = true;
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    if (myOrder.Unit == null || myOrder.Unit == "")
                    {

                    }
                    else
                    {
                        this.cmbUnit.Items.Add(myOrder.Unit);
                    }
                }
                if (myOrder.Item.ID == "999")
                {
                    this.cmbUnit.DropDownStyle = ComboBoxStyle.DropDown;//���Ը���
                    this.cmbUnit.Enabled = this.txtQuantity.Enabled;
                }
                else
                {
                    this.cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;//ֻ��ѡ��
                    //{8670585E-EE96-47db-86FE-FA2F81EBF459} ���޸İ�װ��λ������С��λ����һ��������Ƶ�λ��ʾ 20100915
                    //this.cmbUnit.Enabled = false; 
                }

                if (myOrder.StockDept.ID == null || myOrder.StockDept.ID == "")
                {
                    myOrder.StockDept.ID = item.User02; //�ۿ����,����Ҫ����Ҫע��
                    myOrder.StockDept.Name = item.User03;//�ۿ����
                }

                if (myOrder.Unit == null || myOrder.Unit.Trim() == "")
                {
                    if (this.cmbUnit.Items.Count > 0)
                    {
                        this.cmbUnit.SelectedIndex = 0;
                        myOrder.Unit = this.cmbUnit.Text;
                    }
                }
                else
                {
                    this.cmbUnit.Text = myOrder.Unit;
                }

                //����ʱ��
                if (this.order.BeginTime >= this.dtBegin.MinDate)
                    this.dtBegin.Value = this.order.BeginTime;

                if (this.order.EndTime <= this.dtEnd.MaxDate)
                {
                    if (this.order.EndTime == DateTime.MinValue) //��С���ڲ����ý�������
                        this.dtEnd.Checked = false;
                    else
                    {
                        this.dtEnd.Checked = true;//����С���ڣ����ý�������
                        this.dtEnd.Value = this.order.EndTime;
                    }
                }

            }
            else if (myOrder.Item.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))//��ҩƷ
            {
                Neusoft.HISFC.Models.Fee.Item.Undrug item = ((Neusoft.HISFC.Models.Fee.Item.Undrug)myOrder.Item);

                //���ִ�п���Ϊ��--�������ƿ���
                if (myOrder.ExeDept.ID == "")
                {
                    if (item.ExecDept == "")
                    {
                        myOrder.ExeDept = myOrder.Patient.PVisit.PatientLocation.Dept.Clone();////ִ�п���?????������Ҫ�޸�
                    }
                    else if (item.ExecDepts != null && item.ExecDepts.Count > 0)
                    {
                        try
                        {
                            myOrder.ExeDept.ID = ((Neusoft.HISFC.Models.Fee.Item.Undrug)myOrder.Item).ExecDepts[0].ToString();
                        }
                        catch { }
                    }
                }
                if (myOrder.CheckPartRecord == "" && myOrder.Item.SysClass.ID.ToString() == "UC") //�����岿λ
                {
                    myOrder.CheckPartRecord = item.CheckBody;
                }
                if (myOrder.Sample.Name == "" && myOrder.Item.SysClass.ID.ToString() == "UL") //�����岿λ
                {
                    myOrder.Sample.Name = item.CheckBody;
                }
                if (myOrder.Frequency.ID == "") myOrder.Frequency.ID = "QD";//��ʱҽ��Ĭ��QD

                this.ShowTotal(true);

                this.cmbUnit.Items.Clear();

                if (myOrder.Unit == null || myOrder.Unit.Trim() == "")
                {
                    string unit = ((Neusoft.HISFC.Models.Fee.Item.Undrug)myOrder.Item).PriceUnit;
                    if (unit == null || unit == "") unit = "��";
                    this.cmbUnit.Items.Add(unit);
                    if (this.cmbUnit.Items.Count > 0)
                    {
                        this.cmbUnit.SelectedIndex = 0;
                        myOrder.Unit = this.cmbUnit.Text;
                    }
                }
                else
                {
                    this.cmbUnit.Items.Add(myOrder.Unit);
                    this.cmbUnit.Text = myOrder.Unit;
                }
                if (myOrder.Qty == 0)
                {
                    this.txtQuantity.Text = "1.00"; //����
                    myOrder.Qty = 1;
                }
                else
                {
                    this.txtQuantity.Text = myOrder.Qty.ToString();
                }
               
                //����ʱ��
                if (this.order.BeginTime >= this.dtBegin.MinDate)
                    this.dtBegin.Value = this.order.BeginTime;

                if (this.order.EndTime == DateTime.MinValue) //��С���ڲ����ý�������
                    this.dtEnd.Checked = false;
                else
                {
                    this.dtEnd.Checked = true;//����С���ڣ����ý�������
                    this.dtEnd.Value = this.order.EndTime;
                }
              
            }
            else
            {
                MessageBox.Show("�޷�ʶ������ͣ�");
                return -1;
            }

            
            return 0;

        }

        protected Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = null;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
        {
            set
            {
                //{112B7DB5-0462-4432-AD9D-17A7912FFDBE} 
                bool isRefresh = false;
                //{CE481BFE-9211-48eb-8921-50D04858CB39} ����value != null���ж� Added by Gengxl
                if (value != null && this.patientInfo != null && this.patientInfo.ID != value.ID)
                {
                    isRefresh = true;
                }
                this.patientInfo = value;
                //{112B7DB5-0462-4432-AD9D-17A7912FFDBE}  ������Ϣ
                this.ucInputItem1.Patient = value;

                if (isRefresh)
                {
                    if (this.patientInfo.Pact.PayKind.ID == "02")
                    {
                        Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("����ˢ��ҽ����Ŀ�����..");
                        Application.DoEvents();

                        this.ucInputItem1.RefreshSIFlag();

                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    }
                }
            }
        }

        /// <summary>
        /// ������ҽ��
        /// </summary>
        protected void SetOrder()
        {
            if (this.DesignMode) return;
            //�������ҽ������
            this.order = new Neusoft.HISFC.Models.Order.Inpatient.Order();//��������ҽ��

            dirty = false;
            try
            {
                if (this.ucInputItem1.FeeItem.ID == "999")//�Լ�¼����Ŀ
                {
                    this.order.Item = this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Base.Item;
                }
                else
                {
                    //ҩƷ
                    if (this.ucInputItem1.FeeItem.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
                    {
                        if (pharmacyManager == null) pharmacyManager = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
                        this.order.Item = pharmacyManager.GetItem(this.ucInputItem1.FeeItem.ID);
                        this.order.Item.User01 = this.ucInputItem1.FeeItem.User01;
                        this.order.Item.User02 = this.ucInputItem1.FeeItem.User02;//����ȡҩҩ��
                        this.order.Item.User03 = this.ucInputItem1.FeeItem.User03;
                    }
                    else//��ҩƷ
                    {
                        try
                        {
                            Neusoft.HISFC.Models.Fee.Item.Undrug itemTemp = null;
                            itemTemp = itemManager.GetItem(this.ucInputItem1.FeeItem.ID);                          

                            this.order.Item = itemTemp;

                            //ִ�п��Ҹ�ֵ ������Ŀͬʱ��ִֵ�п��� 
                            //----Edit By liangjz 07-03  {72CEDD06-8C9F-4799-8309-0A55D9567F60}
                            if (itemTemp.ExecDept != null && itemTemp.ExecDept != "")
                            {
                                this.order.ExeDept.ID = itemTemp.ExecDept;
                            }
                            else
                            {
                                this.order.ExeDept = this.order.Patient.PVisit.PatientLocation.Dept.Clone();
                            }
                            //-----

                            //���Ҫ���Ƿ�Ϊ�� ��ʱ�ɴ��жϸ���ĿΪ��黹�Ǽ���		
                            if (itemTemp.SysClass.ID.ToString() == "UL")
                            {
                                //���ø�����Ŀ��ϸ����������롢��������
                                this.order.Sample.Name = itemTemp.CheckBody;
                            }
                            else
                                this.order.CheckPartRecord = itemTemp.CheckBody;

                        }
                        catch { MessageBox.Show("ת������!", "ucItemSelect"); }
                    }
                    //����֪��ͬ����
                    this.order.IsPermission = bPermission;
                }
            }
            catch { return; }
            
            //��ʾ������
            if (ReadOrder(this.order) == -1) return;
            
            //����ҽ������ʱ��
            Neusoft.FrameWork.Management.DataBaseManger manager = new Neusoft.FrameWork.Management.DataBaseManger();
            DateTime dtNow = manager.GetDateTimeFromSysDateTime();
            if (Classes.Function.IsDefaultMoDate == false)
            {
                if (dtNow.Hour >= 12)
                    this.dtBegin.Value = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 12, 0, 0);
                else
                    this.dtBegin.Value = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);

                if (Classes.Function.MoDateDays > 0)
                {
                    this.dtBegin.Value = new DateTime(dtNow.Year, dtNow.Month, dtNow.AddDays(Classes.Function.MoDateDays).Day, 0, 0, 2);
                }
            }
            else
            {
                this.dtBegin.Value = dtNow;
            }


            try//����ֹͣʱ��
            {
                if (this.PanelEnd.Visible)//{A25B1E70-1EA9-40fd-BB6C-050DE67AD4EF} ��ʱҽ������Ҫ��ʾֹͣʱ��
                {
                    this.dtEnd.Value = DateTime.Today.AddDays(1);
                    this.dtEnd.Checked = false;
                }
            }
            catch { }

            this.order.MOTime = dtNow;//����ʱ��
            this.order.BeginTime = this.dtBegin.Value;//��ʼʱ��
            this.order.Item.PriceUnit = this.cmbUnit.Text;
            this.order.Unit = this.cmbUnit.Text;

            this.order.ReciptDept = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.Clone();//��������
            this.order.Oper.ID = Neusoft.FrameWork.Management.Connection.Operator.ID;//¼����
            this.order.Oper.Name = Neusoft.FrameWork.Management.Connection.Operator.Name;
            try
            {
                //*****************��Ҫ���ģ��ж�ҽ�����ͣ��Զ��仯*****************
                ////������շѣ��ı�ҽ������Ϊ����(������Ŀ����)
                //if (this.order.Item.Price == 0 && (this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType).IsCharge
                //    && this.order.Item.PriceUnit != "[������]")
                //{
                //    GeChargeableOrderType(false);
                //}
                //else
                //{
                //}
                ////����ĿΪ���� ����ҽ������Ϊ��ǰ����  Edit By liangjz 2005-10 ��ɽһԺ���� ���ڴ�ӡ��ǰҽ��ִ�е�
                //if (this.order.Item.SysClass.ID.ToString() == "UO" && (this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.FrameWork.Models.NeuObject).ID.ToString() != "SQ")
                //{	//SQ ��ǰ���� SZ ��ǰ����
                //    this.cmbOrderType1.Tag = "SQ";
                //}
            }
            catch { }
            //ҽ������
            this.order.OrderType = this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;

            if (this.order.OrderType.ID == "CZ")        //����ҽ��
            {
                if (this.order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
                {                    
                    string drugQuality = ((Neusoft.HISFC.Models.Pharmacy.Item)this.order.Item).Quality.ID;
                    if (this.hsResQuality.ContainsKey(drugQuality))
                    {
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(this.order.Item.Name + " ������ҩƷ������������ҽ��"),"",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            
            if (this.txtQuantity.Enabled) this.txtQuantity.Focus();//focus
            else this.ucOrderInputByType1.Focus();
            if (this.cmbUnit.Items.Count > 0) this.cmbUnit.SelectedIndex = 0;//Ĭ��ѡ���һ����
            this.ucOrderInputByType1.IsNew = true;//�µ�
            
            //��ʼ������Ŀ��Ϣ ����ҽ��Ƶ��
            Classes.Function.SetDefaultOrderFrequency(this.order);
            if (this.order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
            {
                this.order.Usage.ID = (this.order.Item as Neusoft.HISFC.Models.Pharmacy.Item).Usage.ID;
                this.order.Usage.Name = Classes.Function.HelperUsage.GetName(this.order.Usage.ID);
            }
        
            this.ucOrderInputByType1.Order = this.order;//���ݸ�ѡ������
            dirty = true;
            myOrderChanged(this.order,EnumOrderFieldList.Item);
          
        
        }
        #endregion

        #region  ������ҽ�������޸ĺ��� 
        /// <summary>
        /// ���ҽ����ʾ
        /// </summary>
        public void Clear()
        {
            try
            {
                this.order = null;
                #region
                //����ҽ������ѡ��ҩ�� {CD0DD444-07D0-4e80-9D26-0DB79BA9A177} wbo 2010-10-26
                //this.ucInputItem1.txtItemCode.Text = "";			//��Ŀ����
                //this.ucInputItem1.txtItemName.Text = "";			//��Ŀ����
                this.ucInputItem1.Clear();
                this.ucInputItem1.ItemListVisible = false;
                #endregion
                this.txtQuantity.Text = "";					//����
                this.dtEnd.Checked = false;
                this.cmbUnit.Items.Clear();
                this.ucOrderInputByType1.Clear();

              
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtQuantity_Enter(object sender, EventArgs e)
        {
            this.txtQuantity.Select(0, this.txtQuantity.Value.ToString().Length);
        }

        private void dtEnd_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string unit = this.cmbUnit.Text.Trim();
                if (Neusoft.FrameWork.Public.String.ValidMaxLengh(unit, 16) == false)
                {
                    MessageBox.Show("��λ����!", "��ʾ");
                    return;
                }
                if (this.order.Unit != unit && dirty == true)
                {
                    this.order.Unit = unit;//���µ�λ
                    myOrderChanged(this.order, EnumOrderFieldList.Unit);
                }
            }
            catch { }
        }

        #region addby xuewj 2010-10-3 ҽ�����ͽ�����ת {9128C28D-DF9E-4494-ABAB-BC0F87A3C120}
        private void cmbOrderType1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e == null || e.KeyChar == 13)
            {
                this.ucInputItem1.Focus();
            }
        } 
        #endregion

        private void dtBegin_ValueChanged_2(object sender, EventArgs e)
        {

        }
        ///// <summary>
        ///// ѡ��ҽ�����͵�һ��
        ///// </summary>
        //public void ResetOrderType()
        //{
        //    this.cmbOrderType1.SelectedIndex = 0;
        //}
        ///// <summary>
        ///// ����ҽ������ Add By liangjz 2005-08
        ///// </summary>
        //public void SetOrderType()
        //{
        //    bool isFind = false;
        //    if (this.LongOrShort == 0)		//��Ϊ����ֱ����ʾȫ��Ƶ�Σ������ж�
        //        this.ucOrderInputByType1.SetFrequency(false);
        //    else							//�����������������ʾ��ͬƵ��
        //    {
        //        switch ((this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType).ID)
        //        {
        //            case "LZ":	//��ʱҽ��
        //            case "ZL":	//����ҽ��
        //            case "BL":	//��¼ҽ��
        //            case "SQ":	//��ǰ����
        //            case "SZ":	//��ǰҽ��
        //                this.ucOrderInputByType1.SetFrequency(true);
        //                break;
        //            default:
        //                this.ucOrderInputByType1.SetFrequency(false);
        //                break;
        //        }
        //    }

        //    if (this.order == null)
        //    {
        //        return;
        //    }
        //    if (this.LongOrShort == 0)		//����
        //    {
        //        for (int i = 0; i < this.alLong.Count; i++)
        //        {
        //            if (this.order.OrderType.ID == (this.alLong[i] as Neusoft.HISFC.Models.Order.OrderType).ID)
        //            {
        //                isFind = true;
        //                break;
        //            }
        //        }
        //    }
        //    else if (this.LongOrShort == 1)		//����
        //    {
        //        for (int i = 0; i < this.alShort.Count; i++)
        //        {
        //            if (this.order.OrderType.ID == (this.alShort[i] as Neusoft.HISFC.Models.Order.OrderType).ID)
        //            {
        //                isFind = true;
        //                break;
        //            }
        //        }
        //    }

        //    if (!isFind)
        //        return;

        //    if ((this.order != null && this.order.Status == 0) || (this.order != null && this.order.ID == ""))		//����ˡ�ֹͣҽ��
        //    {
        //        this.order.OrderType = this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;
        //        try
        //        {
        //            //������շѣ��ı�ҽ������Ϊ����(������Ŀ����)
        //            if (this.order.Item.Price == 0 && (this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType).IsCharge
        //                && this.order.Item.PriceUnit != "[������]")
        //            {
        //                GeChargeableOrderType(false);
        //            }
        //            else
        //            {
        //            }
        //        }
        //        catch { }
        //        myOrderChanged(this.Order);
        //    }

        //}
        #endregion
      
    }

    /// <summary>
    /// ҽ������
    /// </summary>
    public enum Operator
    { Add, Modify, Delete, Query }
}
