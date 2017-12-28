using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.Components.Order.OutPatient.Controls
{
    public partial class ucOutPatientItemSelect : UserControl
    {
        public ucOutPatientItemSelect()
        {
            InitializeComponent();
        }

        #region ��ʼ��
        public void Init()
        {
            if (DesignMode) return;
            if (Neusoft.FrameWork.Management.Connection.Operator.ID == "") return;
            #region ����tip
            tooltip.SetToolTip(this.ucInputItem1, "����ƴ�����ѯ������ҽ��(ESCȡ���б�)");
            tooltip.SetToolTip(this.txtQTY, "����������(�س��������)");
            #endregion
            try
            {
                //this.ucInputItem1.DeptCode = "";//���ҿ���ȫ����Ŀ
                this.ucInputItem1.DeptCode = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;
                this.ucInputItem1.ShowCategory = Neusoft.HISFC.Components.Common.Controls.EnumCategoryType.SysClass;
                
                this.ucOrderInputByType1.ItemSelected += new ItemSelectedDelegate(ucOrderInputByType1_ItemSelected);

                this.ucInputItem1.SelectedItem += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucInputItem1_SelectedItem);
                this.ucInputItem1.CatagoryChanged += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler(ucInputItem1_CatagoryChanged);

                this.txtQTY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQTY_KeyPress);
                this.txtQTY.Leave += new EventHandler(txtQTY_Leave);

                this.ucOrderInputByType1.Leave += new EventHandler(ucOrderInputByType1_Leave);
                this.ucOrderInputByType1.InitControl(null, null, null);

                this.cmbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbUnit_KeyPress);
                this.cmbUnit.TextChanged += new EventHandler(cmbUnit_TextChanged);
                this.ucInputItem1.UndrugApplicabilityarea = Neusoft.HISFC.Components.Common.Controls.EnumUndrugApplicabilityarea.Clinic;
                this.ucInputItem1.Init();//��ʼ����Ŀ�б�
                
                this.ucInputItem1.AlCatagory = this.OrderCatatagory();
            }
            catch { }
            
            this.pValue = this.controlManager.QueryControlerInfo("200004");
        }
        #endregion

        #region ����
        protected ToolTip tooltip = new ToolTip(); //ToolTip
        protected bool dirty = false;//���µ�ʱ��
        public int CurrentRow = -1; //��ǰ��
        public bool EditGroup = false;// �Ƿ�������ױ༭����
        protected bool isLisDetail = false;
        protected bool bPermission = false;//�Ƿ�֪��ͬ����
        protected string pValue = "0";//�Ƿ���Կ�������ҽ��
        /// <summary>
        /// ҽ���仯ʱ����
        /// </summary>
        public event ItemSelectedDelegate OrderChanged;
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        public Operator OperatorType = Operator.Query;

        public event Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler CatagoryChanged;
        /// <summary>
        /// ҩƷҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmacyManager = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        /// <summary>
        /// ��ҩƷҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee itemManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        /// <summary>
        /// ����ҵ���
        /// </summary>
        protected Neusoft.FrameWork.Management.ControlParam controlManager = new Neusoft.FrameWork.Management.ControlParam();

        /// <summary>
        /// {24BDD373-4F2C-4899-88A7-FE2E8386F7CF}
        /// </summary>
        public string isDrugListFlag = string.Empty;
        
        #endregion

        #region ����

        protected Neusoft.HISFC.Models.Order.OutPatient.Order order;
        /// <summary>
        /// ҽ��
        /// </summary>
        public Neusoft.HISFC.Models.Order.OutPatient.Order currOrder
        {
            get
            {
                return this.order;
            }
            set
            {
                if (value == null) return;
                this.order = value;
                dirty = false; //���Ǳ仯ʱ��--����ʱ��
                
                    this.ucOrderInputByType1.IsNew = false;//�޸ľ�ҽ��
                    this.ucOrderInputByType1.Order = value;

                    this.ucInputItem1.FeeItem = this.order.Item;

                    ReadOrder(this.order);//��������ҽ��
                    dirty = true;

            }
                
        }

        protected Neusoft.HISFC.Models.Registration.Register patientInfo = null;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.Registration.Register PatientInfo
        {
            set
            {
                this.patientInfo = value;
            }
        }

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

        #endregion
        
        #region ����

        private ArrayList OrderCatatagory()
        {
            System.Collections.ArrayList al = Neusoft.HISFC.Models.Base.SysClassEnumService.List();
            Neusoft.FrameWork.Models.NeuObject objAll = new Neusoft.FrameWork.Models.NeuObject();
            objAll.ID = "ALL";
            objAll.Name = "ȫ��";
            al.Add(objAll);
            //����Щ����

            System.Collections.ArrayList rAl = new ArrayList();
            foreach (Neusoft.FrameWork.Models.NeuObject obj in al)
            {
                if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "MR")//��ҩƷ��ת�ƣ�ת��
                {

                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "MF")//��ʳ
                {
                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "UN")//������
                {
                }
                else if (obj.ID.Length > 1 && obj.ID.Substring(0, 2) == "UJ")	//����
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
        /// ���ҽ����ʾ
        /// </summary>
        public void Clear()
        {
            try
            {
                this.order = null;
                this.ucInputItem1.txtItemCode.Text = "";			//��Ŀ����
                this.ucInputItem1.txtItemName.Text = "";			//��Ŀ����
                this.txtQTY.Text = "";					//����

                this.cmbUnit.Items.Clear();
                this.ucOrderInputByType1.Clear();
            }
            
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// ��ȡҽ����Ϣ-���ƿؼ���ʾ״̬
        /// </summary>
        /// <param name="myOrder"></param>
        public virtual int ReadOrder(Neusoft.HISFC.Models.Order.Order myOrder)
        {
            if (myOrder == null) return 0;
            //��Ŀ
            if (myOrder.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
            {
                Neusoft.HISFC.Models.Pharmacy.Item item = ((Neusoft.HISFC.Models.Pharmacy.Item)myOrder.Item);
                if (myOrder.Frequency.ID == null || myOrder.Frequency.ID == "")
                    myOrder.Frequency.ID = "PRN";//����ҽ��Ĭ��Ϊ��Ҫʱִ��
                
                this.txtQTY.Text = myOrder.Qty.ToString(); //����
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
                        
                        this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).MinUnit);//min��λ
                        this.cmbUnit.Items.Add((this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Pharmacy.Item).PackUnit);//pack��λ

                        this.cmbUnit.Enabled = true;
                        
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
                    this.cmbUnit.Enabled = this.txtQTY.Enabled;
                }
                else
                {
                    this.cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;//ֻ��ѡ��
                    this.cmbUnit.Enabled = true;
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
                        #region ��װ��λ��ʾ --donggq----{BC5E1C12-B63E-4efb-BA52-2BF30AA5FFF4}

                        if (this.cmbUnit.Items.Count > 1)
                        {
                            this.cmbUnit.SelectedIndex = 1;  myOrder.NurseStation.User03 = "0";
                        }
                        else 
                        {
                            this.cmbUnit.SelectedIndex = 0;  myOrder.NurseStation.User03 = "1";
                        }
                        
                        #endregion

                        myOrder.Unit = this.cmbUnit.Text;
                    }
                }
                else
                {
                    this.cmbUnit.Text = myOrder.Unit;
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
                if (myOrder.Frequency.ID == null || myOrder.Frequency.ID == "")
                    myOrder.Frequency.ID = "QD";//����ҽ��Ĭ��QD

                //this.ShowTotal(true);

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
                    this.txtQTY.ValueChanged -= new System.EventHandler(this.txtQTY_ValueChanged);
                    this.txtQTY.Text = "1.00"; //����
                    this.txtQTY.ValueChanged += new System.EventHandler(this.txtQTY_ValueChanged);
                    myOrder.Qty = 1;
                }
                else
                {
                    this.txtQTY.Text = myOrder.Qty.ToString();
                }
            }
            else
            {
                MessageBox.Show("�޷�ʶ������ͣ�");
                return -1;
            }


            return 0;

        }

        /// <summary>
        /// ������ҽ��
        /// </summary>
        public virtual void SetOrder()
        {
            if (this.DesignMode) return;
            //�������ҽ������
            this.order = new Neusoft.HISFC.Models.Order.OutPatient.Order();//��������ҽ��
            
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
                            if (((Neusoft.HISFC.Models.Base.Item)this.ucInputItem1.FeeItem).PriceUnit != "[������]")
                            {
                                Neusoft.HISFC.Models.Fee.Item.Undrug itemTemp = null;
                                itemTemp = itemManager.GetItem(this.ucInputItem1.FeeItem.ID);

                                this.order.Item = itemTemp;

                                //ִ�п��Ҹ�ֵ ������Ŀͬʱ��ִֵ�п��� 
                                //----Edit By liangjz 07-03
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
                                    //���ø�����Ŀ��ϸ����������롢��������/��鲿λ
                                    this.order.Sample.Name = itemTemp.CheckBody;
                                }
                                else
                                    this.order.CheckPartRecord = itemTemp.CheckBody;
                            }
                            else
                            {
                                Neusoft.HISFC.Models.Fee.Item.Undrug itemTemp = null;
                                itemTemp = (Neusoft.HISFC.Models.Fee.Item.Undrug)this.ucInputItem1.FeeItem;
                                this.order.Item = itemTemp;
                                //���Ҫ���Ƿ�Ϊ�� ��ʱ�ɴ��жϸ���ĿΪ��黹�Ǽ���		
                                if (itemTemp.SysClass.ID.ToString() == "UL")
                                {
                                    //���ø�����Ŀ��ϸ����������롢��������/��鲿λ
                                    this.order.Sample.Name = itemTemp.CheckBody;
                                }
                                else
                                    this.order.CheckPartRecord = itemTemp.CheckBody;
                                this.order.Item.MinFee.ID = "fh";
                            }
                        }
                        catch { MessageBox.Show("ת������!", "ucItemSelect"); }
                    }
                    
                }
            }
            catch { return; }


            //��ʾ������
            if (ReadOrder(this.order) == -1) return;

            //����ҽ������ʱ��
            Neusoft.FrameWork.Management.DataBaseManger manager = new Neusoft.FrameWork.Management.DataBaseManger();
            DateTime dtNow = manager.GetDateTimeFromSysDateTime();
                                    
            this.order.MOTime = dtNow;//����ʱ��
            this.order.BeginTime = dtNow;//��ʼʱ��
            this.order.Item.PriceUnit = this.cmbUnit.Text;
            this.order.Unit = this.cmbUnit.Text;

            this.order.ReciptDept = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.Clone();//��������
            this.order.Oper.ID = Neusoft.FrameWork.Management.Connection.Operator.ID;//¼����
            this.order.Oper.Name = Neusoft.FrameWork.Management.Connection.Operator.Name;
            
            //ҽ������
            //this.order.OrderType = this.cmbOrderType1.alItems[this.cmbOrderType1.SelectedIndex] as Neusoft.HISFC.Models.Order.OrderType;


            if (this.txtQTY.Enabled)
            {
                this.txtQTY.Focus();//focus
                this.txtQTY.Select(0, this.txtQTY.Value.ToString().Length);
            }
            else
            {
                this.ucOrderInputByType1.Focus();
            }
            if (this.cmbUnit.Items.Count > 0) this.cmbUnit.SelectedIndex = 0;//Ĭ��ѡ���һ����
            this.ucOrderInputByType1.IsNew = true;//�µ�

            //��ʼ������Ŀ��Ϣ ����ҽ��Ƶ���÷�
            
            if (this.order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))
            {
                //this.order.Frequency.ID = "PRN";
                this.order.Usage.ID = (this.order.Item as Neusoft.HISFC.Models.Pharmacy.Item).Usage.ID;
                this.order.Usage.Name = Order.Classes.Function.HelperUsage.GetName(this.order.Usage.ID);
            }
            else
            {
                //this.order.Frequency.ID = "QD";
            }

            if (this.order.HerbalQty == 0) this.order.HerbalQty = 1;//���²�ҩ����

            this.ucOrderInputByType1.Order = this.order;//���ݸ�ѡ������
            dirty = true;
            this.myOrderChanged(this.order, EnumOrderFieldList.Item);

        }

        protected void myOrderChanged(object sender, EnumOrderFieldList enumOrderFieldList)
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

                this.currOrder = sender as Neusoft.HISFC.Models.Order.OutPatient.Order;//�ؼ������Ķ���

                this.OrderChanged(order, enumOrderFieldList);
            }
            catch { }
        }

        /// <summary>
        /// ��������ҽ������֣�
        /// </summary>
        private void DealGroupOrder(Neusoft.FrameWork.Models.NeuObject group)
        {
            if (group == null || group.ID.Length <= 0)
            {
                return;
            }
            ArrayList alGroupDetail = null;
            
            try
            {
                ////alGroupDetail = this.groupManager.GetComGroupTailByGroupID(group.ID);
            }
            catch
            {
                MessageBox.Show("���������ϸ��Ϣ����");
                return;
            }
            if (alGroupDetail == null || alGroupDetail.Count <= 0)
            {
                return;
            }
            ////OutPatient.frmGroupDetail frm = new frmGroupDetail();

            ////frm.alGroupDel = alGroupDetail;
            ////frm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ////frm.ShowDialog();
            ////if (frm.alOrderItem.Count <= 0)
            ////{
            ////    return;
            ////}
            ////for (int i = 0; i < frm.alOrderItem.Count; i++)
            ////{
            ////    this.ucItem1.FeeItem = (neusoft.neHISFC.Components.Object.neuObject)frm.alOrderItem[i];
            ////    this.CurrentRow = -1;
            ////    this.SetOrder();
            ////}
        }

        protected virtual void ucOrderInputByType1_ItemSelected(Neusoft.HISFC.Models.Order.OutPatient.Order order, EnumOrderFieldList changedField)
        {
            dirty = true;
            
            this.txtQTY.Text = order.Qty.ToString();
            
            this.myOrderChanged(order, changedField);
            dirty = false;
        }

        #endregion

        #region �¼�

        /// <summary>
        /// �����仯-������һ�������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQTY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.order == null) return;
            if (e == null || e.KeyChar == 13)
            {
                if (this.order.Qty != Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQTY.Value))
                {
                    this.order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQTY.Value);
                    myOrderChanged(this.order, EnumOrderFieldList.Qty);
                }
                if (e != null)
                {
                    //if (this.order.Item.IsPharmacy == false)//��ҩƷ���� �¼�
                    if (this.order.Item.ItemType != EnumItemType.Drug)//��ҩƷ���� �¼�
                        this.ucOrderInputByType1.Focus();
                    else
                        this.cmbUnit.Focus();
                }
            }
        }

        private void txtQTY_Leave(object sender, EventArgs e)
        {
            if (this.order == null) return;
            if (this.order.Qty == Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQTY.Value))
            {
                if (isDrugListFlag == string.Empty)//{24BDD373-4F2C-4899-88A7-FE2E8386F7CF}
                {
                    return;
                }
                else
                {
                    this.txtQTY.Focus();
                    isDrugListFlag = string.Empty;
                }
            }
            this.txtQTY_KeyPress(sender, null);            

        }

        private void txtQTY_Enter(object sender, EventArgs e)
        {
            this.txtQTY.Select(0, this.txtQTY.Value.ToString().Length);
        }

        private void cmbUnit_TextChanged(object sender, EventArgs e)
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
                    #region �ж��Ƿ�����С��λ --donggq----{BC5E1C12-B63E-4efb-BA52-2BF30AA5FFF4}
                    //if (this.order.Item.IsPharmacy)
                    if (this.order.Item.ItemType == EnumItemType.Drug)
                    {
                        if (this.cmbUnit.SelectedIndex == 0)
                        {
                            this.order.NurseStation.User03 = "1";
                        }
                        else
                        {
                            this.order.NurseStation.User03 = "0";
                        }
                    }
                    # endregion
                    this.order.Unit = unit;//���µ�λ
                    myOrderChanged(this.order, EnumOrderFieldList.Unit);
                }
            }
            catch { }
        }

        void ucInputItem1_SelectedItem(Neusoft.FrameWork.Models.NeuObject sender)
        {

            if (this.ucInputItem1.FeeItem == null) return;
            if (!this.EditGroup)		//��ʵ�ֶ������޸Ĺ���ʱ �����֪��ͬ����������ж�
            {
                //�жϵ�ǰ�������Ŀ�Ƿ�֪��ͬ����
                ////this.bPermission = Order.Classes.Function.IsPermission(this.patientInfo,
                ////    (Neusoft.HISFC.Models.Order.OrderType)this.cmbOrderType1.SelectedItem,
                ////    (Neusoft.HISFC.Models.Base.Item)this.ucInputItem1.FeeItem);
            }

            if (this.order != null && this.ucInputItem1.FeeItem as Neusoft.HISFC.Models.Base.Item == this.order.Item) //���ظ�
            {
                this.txtQTY.Focus();
                this.txtQTY.Select(0, this.txtQTY.Value.ToString().Length);
                return;
            }

            //��Ŀ�仯-ָ������
            this.CurrentRow = -1;

            this.OperatorType = Operator.Add;

            //������ҽ��
            this.SetOrder();

            //�����仯
            if (this.txtQTY.Enabled)
            {
                this.txtQTY.Focus();
                this.txtQTY.Select(0, this.txtQTY.Value.ToString().Length);
            }
            else
            {
                this.ucOrderInputByType1.Focus();
            }

        }

        public void SetQtyFocus()
        {
            this.txtQTY.Focus();
            this.txtQTY.Select(0, this.txtQTY.Value.ToString().Length);
        }

        void ucInputItem1_CatagoryChanged(Neusoft.FrameWork.Models.NeuObject sender)
        {
            ////try
            ////{
            ////    Neusoft.FrameWork.Models.NeuObject obj = sender;
            ////    if (obj.ID.Length > 0 && obj.ID.Substring(0, 1) == "M")
            ////    {
            ////        GeChargeableOrderType(false);
            ////    }
            ////    else
            ////    {
            ////        GeChargeableOrderType(true);
            ////    }
            ////}
            ////catch { }
            ////if (CatagoryChanged != null) CatagoryChanged(sender);
        }

        void ucOrderInputByType1_Leave(object sender, EventArgs e)
        {
            this.ucInputItem1.Focus();
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
                if (this.order.Item.ItemType != EnumItemType.Drug)//��ҩƷ���� �¼�
                    this.ucOrderInputByType1.Focus();
                else
                    this.ucOrderInputByType1.Focus();
                #region �ж��Ƿ�����С��λ
                //if (this.order.Item.IsPharmacy)
                if (this.order.Item.ItemType == EnumItemType.Drug)
                {
                    if (this.cmbUnit.SelectedIndex == 0)
                    {
                        this.order.NurseStation.User03 = "1";
                    }
                    else
                    {
                        this.order.NurseStation.User03 = "0";
                    }
                }
                # endregion
            }
        }

        /// <summary>
        /// ��λѡ��仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    #region �ж��Ƿ�����С��λ
                    //if (this.order.Item.IsPharmacy)
                    if (this.order.Item.ItemType == EnumItemType.Drug)
                    {
					    if(this.cmbUnit.SelectedIndex == 0)
					    {
						    this.order.NurseStation.User03 = "1";
					    }
					    else
					    {
                            this.order.NurseStation.User03 = "0";
					    }
                    }
				    # endregion
                    this.order.Unit = unit;//���µ�λ
                    myOrderChanged(this.order, EnumOrderFieldList.Unit);
                }
            }
            catch { }
        }

        #endregion

        private void txtQTY_ValueChanged(object sender, EventArgs e)
        {
            if (this.order == null) return;
            if (this.order.Qty == Neusoft.FrameWork.Function.NConvert.ToDecimal(this.txtQTY.Value)) return;
            this.txtQTY_KeyPress(sender, null);
        }

        
    }
    /// <summary>
    /// ҽ������
    /// </summary>
    public enum Operator
    { Add, Modify, Delete, Query }
}

