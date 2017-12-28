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
    /// [��������: ҽ���ֽ�ؼ���˵]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucOrderExecConfirm : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucOrderExecConfirm()
        {
            InitializeComponent();
        }

        protected Neusoft.HISFC.BizLogic.Order.Order orderManagement = new Neusoft.HISFC.BizLogic.Order.Order();
        protected Neusoft.HISFC.BizProcess.Integrate.Fee feeManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        
        protected DateTime dt;
        Hashtable sendFlag;

        bool tab0AllSelect = false;
        bool tab1AllSelect = false;

        #region {13EAF764-E1CA-4d5a-8250-056AD1DEE61B}
        /// <summary>
        /// ����ҵ����
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Manager deptManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        /// <summary>
        /// �����б�
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper deptHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        #endregion

        #region ����
        protected ArrayList al = new ArrayList();
        /// <summary>
        /// ��Ա�б�
        /// </summary>
        public ArrayList alPatients
        {
            get
            {
                if (al == null) al = new ArrayList();
                return al;
            }
            set
            {
                this.al = value;
            }
        }
        protected int intDyas = 1;
        /// <summary>
        /// ��ҩ����
        /// </summary>
        [Description("��ҩ�����������")]
        public int Days
        {
            set
            {
                this.intDyas = value;
                //this.txtDays.Value = (decimal)value;
                this.txtDays.Maximum = this.intDyas;
            }
            get
            {
                return this.intDyas;
            }
        }

        /// <summary>
        /// ��ʾ����Ϣ
        /// </summary>
        [Description("��ʾ���û�����ʾ��Ϣ��")]
        public string Tip
        {
            get
            {
                return this.neuLabel3.Text;
            }
            set
            {
                this.neuLabel3.Text = value;
            }
        }

        #region Ƿ�Ѳ�����ֽ�д�� {4AE738A2-AC87-447c-86E3-ECE465B7C4D1}
        protected EnumLackFee lackfee = EnumLackFee.Ƿ�Ѳ�����ֽ�;
        /// <summary>
        /// Ƿ�Ѳ���
        /// </summary>
        [Description("Ƿ�Ѻ�Ĳ���")]
        public EnumLackFee Ƿ�Ѳ���
        {
            get
            {
                return this.lackfee;
            }
            set
            {
                this.lackfee = EnumLackFee.Ƿ�Ѳ�����ֽ�;
            }
        }
        #endregion
        #region {C88D3BEB-EA3F-455f-BD5D-0A997699CC2C}
        protected bool isSaveErrContinue = true;
        /// <summary>
        /// ���������Ƿ���������������߱���
        /// </summary>
        [Description("���������Ƿ���������������߱���")]
        public bool IsSaveErrContinue
        {
            get
            {
                return this.isSaveErrContinue;
            }
            set
            {
                this.isSaveErrContinue = value;
            }
        }

        #region {BEAB5DD3-9278-4480-BCFC-9E15469B3376} ������ʾ����ҽ�� by guanyx
        private bool isShowNurseFee = false;
        //[Category("�ؼ�����"), Description("�Ƿ���ʾ�ֽ�Ļ���ѣ�True:��ʾ False:����ʾ")]
        //public bool IsShowNurseFee
        //{
        //    get
        //    {
        //        return isShowNurseFee;
        //    }
        //    set
        //    {
        //        isShowNurseFee = value;
        //    }
        //}
        #endregion

        #endregion
        #endregion

        #region ����
        /// <summary>
        /// ��ʼ��FpSpread
        /// </summary>
        private void InitControl()
        {
            this.fpOrderExecBrowser1.IsShowRowHeader = false;
            this.fpOrderExecBrowser2.IsShowRowHeader = false;
            this.TabControl1.SelectedIndex = 1;
            this.TabControl1.SelectedIndex = 0;

            this.fpOrderExecBrowser1.fpSpread.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread_CellDoubleClick);
            this.fpOrderExecBrowser2.fpSpread.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread_CellDoubleClick);

            this.fpOrderExecBrowser1.fpSpread.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSpread_ButtonClicked);
            this.fpOrderExecBrowser2.fpSpread.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSpread_ButtonClicked);

            #region {13EAF764-E1CA-4d5a-8250-056AD1DEE61B}
            this.deptHelper.ArrayObject = this.deptManager.GetDeptmentAllValid();
            #endregion

            #region donggq--20101110--�趨ʱ��Ĭ��Ϊ00��00:00-->23:59:59--{DE1A9E11-EF26-40eb-A8B2-E6039B8CBF50}
            this.dtpBeginTime.Value = this.dtpBeginTime.Value.Date;
            this.dtpEndTime.Value = this.dtpEndTime.Value.Date.AddDays(1).AddSeconds(-1); 
            #endregion

        }
        /// <summary>
        /// ��ѯִ�е�--�ֽ�ҽ��
        /// </summary>
        /// <returns></returns>
        public int RefreshExec()
        {

            //ȡСʱ�շ� {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
            Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlManager = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
            string frequencyID = controlManager.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.MetConstant.Hours_Frequency_ID, true);

            
            string DeptCode = ""; //����
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.orderManagement.Connection);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            ArrayList alOrders = null;
            Neusoft.HISFC.BizProcess.Integrate.RADT pManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();
            try
            {
                #region �ֽ���˹���ҽ��
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڷֽ⣬���Ժ�...");
                Application.DoEvents();
                
                this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                pManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                //�ֽ�Ĳ���ʱ��,����ֽ�ʱÿ��ҽ������ѯϵͳʱ��
                DateTime dt = new DateTime();
                dt = this.orderManagement.GetDateTimeFromSysDateTime();

                //��ÿ�����ߵ�ҽ���ֽ�
                for (int i = 0; i < this.al.Count; i++)
                {
                    //������Ϣ
                    Neusoft.HISFC.Models.RADT.PatientInfo pTemp = al[i] as Neusoft.HISFC.Models.RADT.PatientInfo;
                    if (pTemp == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show("��û��߻�����Ϣ����");
                        return -1;
                    }

                    Neusoft.HISFC.Models.RADT.PatientInfo p = pManager.GetPatientInfomation(pTemp.ID);
                    if (p == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show("��û��߻�����Ϣ����" + pManager.Err);
                        return -1;
                    }
                    if (p.PVisit.InState.ID.ToString() == "O" ||
                        p.PVisit.InState.ID.ToString() == "P" || p.PVisit.InState.ID.ToString() == "N")
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show(p.Name + "���� �ѳ�Ժ �޷�����ҽ���ֽ����");
                        return -1;
                    }

                    //��ҽ�������ڼ���ҽ��״̬Ϊ1 �� 2��ҽ��
                    alOrders = orderManagement.QueryValidOrderWithSubtbl(p.ID, Neusoft.HISFC.Models.Order.EnumType.LONG);
                    if (alOrders == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show(orderManagement.Err);
                        return -1;
                    }
                    //�ֽ⻼�ߵ�ҽ��
                    for (int j = 0; j < alOrders.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order order = (Neusoft.HISFC.Models.Order.Inpatient.Order)alOrders[j];
                        #region ���Ŀ���
                        DeptCode = order.ReciptDept.ID;//��������
                        //ҽ��ʵ���еĻ�����Ժ�������¸�ֵ
                        order.Patient.PVisit.PatientLocation.Dept.ID = p.PVisit.PatientLocation.Dept.ID;
                        order.Patient.PVisit.PatientLocation.NurseCell.ID = p.PVisit.PatientLocation.NurseCell.ID;
                        #endregion

                        if (order.Usage.ID == "03")   //iv.dri
                        {
                            order.Compound.IsNeedCompound = true;
                        }

                        //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                        if (order.Frequency.ID == frequencyID) //Сʱ�շ�ҽ�����ֽ�ʱ�䵽��ǰʱ��
                        {
                            if (orderManagement.DecomposeOrderToNow(order, 0, false, dt) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                ;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show(orderManagement.Err);
                                return -1;
                            }
                        }

                        else
                        {
                            //��ҽ�����зֽ�
                            if (orderManagement.DecomposeOrder(order, this.intDyas, false, dt) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                ;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                MessageBox.Show(orderManagement.Err);
                                return -1;
                            }
                        }
                    }
                }
                
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                #endregion
            }
            catch (Exception ex)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show("�ֽ����" + ex.Message + this.orderManagement.iNum.ToString());
                return -1;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            this.RefreshQuery();
            return 0;
        }
        bool bOnQuery = false;
        /// <summary>
        /// ���²�ѯ��ʾ
        /// </summary>
        /// <returns></returns>
        protected int RefreshQuery()
        {
            if (bOnQuery) return 0;
            bOnQuery = true;
            //this.fpOrderExecBrowser1.BeginInit();
            //this.fpOrderExecBrowser2.BeginInit();
            try
            {
                this.txtName.Items.Clear();
                ArrayList alOrders = null;
                #region ��ѯ��ʾ
                this.fpOrderExecBrowser1.Clear();
                this.fpOrderExecBrowser2.Clear();

                #region {D0618339-0E36-4d35-A8EF-C4F9E352C71B}
                Neusoft.FrameWork.Public.ObjectHelper orderNameHlpr = new Neusoft.FrameWork.Public.ObjectHelper();
                Neusoft.FrameWork.Public.ObjectHelper deptNameHlpr = new Neusoft.FrameWork.Public.ObjectHelper();
                Neusoft.FrameWork.Models.NeuObject objTmp = new Neusoft.FrameWork.Models.NeuObject();
                #endregion

                #region addby xuewj 2010-10-9 ����ҽ�������� {CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                Neusoft.FrameWork.Public.ObjectHelper orderTypeNameHlpr = new Neusoft.FrameWork.Public.ObjectHelper(); 
                #endregion

                for (int i = 0; i < this.al.Count; i++)
                {
                    Neusoft.HISFC.Models.RADT.PatientInfo p = al[i] as Neusoft.HISFC.Models.RADT.PatientInfo;
                    if (feeManagement.IsPatientLackFee(p) == true) //Ƿ�ѻ���
                    {
                        switch (this.lackfee)
                        {
                            case EnumLackFee.���ж�Ƿ��:
                                break;
                            case EnumLackFee.Ƿ�Ѳ�����ֽ�:
                                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(
                                    string.Format("����{0}�Ѿ�Ƿ��,ʣ����{1}.���ܽ��зֽ������", p.Name, p.FT.LeftCost.ToString())));
                                continue;
                                break;
                            case EnumLackFee.Ƿ����ʾ�ʲ�����ֽ�:
                                if (MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(
                                    string.Format("����{0}�Ѿ�Ƿ��,ʣ����{1}.�Ƿ�����ֽ������", p.Name, p.FT.LeftCost.ToString())), "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    continue;
                                }
                                break;

                        }
                    }
                    Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯ" + p.Name + "�����Ժ�...");
                    #region ��ѯ�ֽ����
                    alOrders = orderManagement.QueryExecOrderIsExec(p.ID, "1", false);//��ѯȷ�ϵ�ҩƷ 
                    #region {D0618339-0E36-4d35-A8EF-C4F9E352C71B}
                    //Neusoft.FrameWork.Public.ObjectHelper orderNameHlpr = new Neusoft.FrameWork.Public.ObjectHelper();
                    //Neusoft.FrameWork.Public.ObjectHelper deptNameHlpr = new Neusoft.FrameWork.Public.ObjectHelper();
                    //Neusoft.FrameWork.Models.NeuObject objTmp = new Neusoft.FrameWork.Models.NeuObject();
                    #endregion
                    for (int j = 0; j < alOrders.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.ExecOrder order = alOrders[j] as Neusoft.HISFC.Models.Order.ExecOrder;
                        order.Order.Patient = p;

                        #region {13EAF764-E1CA-4d5a-8250-056AD1DEE61B}

                        string drugDept = this.deptHelper.GetName(order.Order.StockDept.ID);

                        #endregion

                        if (order.Order.OrderType.IsDecompose)
                        {
                            this.fpOrderExecBrowser1.AddRow(order);
                            objTmp = new Neusoft.FrameWork.Models.NeuObject(order.Order.Item.Name, order.Order.Item.Name, "");
                            if (orderNameHlpr.GetObjectFromID(objTmp.ID) == null)
                            {
                                orderNameHlpr.ArrayObject.Add(objTmp);
                            }
                            #region {13EAF764-E1CA-4d5a-8250-056AD1DEE61B}
                            if (!string.IsNullOrEmpty(drugDept))
                            {
                                objTmp = new Neusoft.FrameWork.Models.NeuObject(drugDept, drugDept, "");
                                if (deptNameHlpr.GetObjectFromID(objTmp.ID) == null)
                                {
                                    deptNameHlpr.ArrayObject.Add(objTmp);
                                }
                            }
                            #endregion
                            #region addby xuewj 2010-10-9 ����ҽ�������� {CA8705F5-C25E-4126-BF15-F498AE82AFAE}                            
                            if (orderTypeNameHlpr.GetObjectFromID(order.Order.OrderType.ID) == null)
                            {
                                objTmp=new Neusoft.FrameWork.Models.NeuObject(order.Order.OrderType.ID, this.fpOrderExecBrowser1.orderTypeHelper.GetName(order.Order.OrderType.ID), "");
                                orderTypeNameHlpr.ArrayObject.Add(objTmp);
                            }
                            #endregion
                        }
                    }
                    //��ҩƷ
                    alOrders = orderManagement.QueryExecOrderIsExec(p.ID, "2", false);//��ѯδִ�еķ�ҩƷ
                    for (int j = 0; j < alOrders.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.ExecOrder order = alOrders[j] as Neusoft.HISFC.Models.Order.ExecOrder;
                        order.Order.Patient = p;
                        //��ʾ��Ҫ��ʿվȷ�ϵķ�ҩƷ
                        if ((((Neusoft.HISFC.Models.Fee.Item.Undrug)order.Order.Item).IsNeedConfirm == false ||
                            order.Order.ExeDept.ID == order.Order.ReciptDept.ID ||
                            order.Order.ExeDept.ID == NurseStation.ID)) //��ʿվ�շѻ���ִ�п��ң�������  
                        {
                            if (order.Order.OrderType.IsDecompose) //����ҽ��
                            {
                                #region {BEAB5DD3-9278-4480-BCFC-9E15469B3376} ������ʾ����ҽ�� by guanyx
                                if (this.isShowNurseFee == false)
                                {
                                    if (order.Order.Item.SysClass.ID.ToString() != "UN")
                                    {
                                        this.fpOrderExecBrowser2.AddRow(order);
                                    }
                                }
                                else
                                {
                                    this.fpOrderExecBrowser2.AddRow(order);
                                }
                                //this.fpOrderExecBrowser2.AddRow(order);
                                #endregion
                                objTmp = new Neusoft.FrameWork.Models.NeuObject(order.Order.Item.Name, order.Order.Item.Name, "");
                                if (orderNameHlpr.GetObjectFromID(objTmp.ID) == null)
                                {
                                    orderNameHlpr.ArrayObject.Add(objTmp);
                                }
                                #region addby xuewj 2010-10-9 ����ҽ�������� {CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                                if (orderTypeNameHlpr.GetObjectFromID(order.Order.OrderType.ID) == null)
                                {
                                    objTmp = new Neusoft.FrameWork.Models.NeuObject(order.Order.OrderType.ID, this.fpOrderExecBrowser1.orderTypeHelper.GetName(order.Order.OrderType.ID), "");
                                    orderTypeNameHlpr.ArrayObject.Add(objTmp);
                                }
                                #endregion
                            }
                        }
                    }

                    #region {D0618339-0E36-4d35-A8EF-C4F9E352C71B}
                    //objTmp = new Neusoft.FrameWork.Models.NeuObject("ALL", "ȫ��", "");
                    //orderNameHlpr.ArrayObject.Insert(0, objTmp);
                    //this.txtName.AddItems(orderNameHlpr.ArrayObject);
                    //this.txtName.Tag = "ALL";
                    //objTmp = new Neusoft.FrameWork.Models.NeuObject("ALL", "ȫ��", "");
                    //deptNameHlpr.ArrayObject.Insert(0, objTmp);
                    //this.txtDrugDeptName.AddItems(deptNameHlpr.ArrayObject);
                    //this.txtDrugDeptName.Tag = "ALL";
                    #endregion

                    #endregion
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

                }
                #endregion

                #region {D0618339-0E36-4d35-A8EF-C4F9E352C71B}
                objTmp = new Neusoft.FrameWork.Models.NeuObject("ALL", "ȫ��", "");
                orderNameHlpr.ArrayObject.Insert(0, objTmp);
                this.txtName.AddItems(orderNameHlpr.ArrayObject);
                this.txtName.Tag = "ALL";
                objTmp = new Neusoft.FrameWork.Models.NeuObject("ALL", "ȫ��", "");
                deptNameHlpr.ArrayObject.Insert(0, objTmp);
                this.txtDrugDeptName.AddItems(deptNameHlpr.ArrayObject);
                this.txtDrugDeptName.Tag = "ALL";
                #endregion
                #region addby xuewj 2010-10-9 ����ҽ�������� {CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                orderTypeNameHlpr.ArrayObject.Insert(0, objTmp);
                this.cmbOrderTypeName.AddItems(orderTypeNameHlpr.ArrayObject);
                this.cmbOrderTypeName.Tag = "ALL";
                #endregion
                this.fpOrderExecBrowser1.RefreshComboNo();
                this.fpOrderExecBrowser2.RefreshComboNo();
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

                //{E08AD6B3-4987-44b0-A5A9-B660D24FBC4D}
                //��ʿ���¼���ʱ���ȼ���2�� Ȼ���ڼ���1���ʱ�� ɾ����������ݣ�������ɾ����
                this.fpOrderExecBrowser1.DeleteRow(this.intDyas);
                this.fpOrderExecBrowser2.DeleteRow(this.intDyas);
                this.tabPage1.Text = "ҩƷ" + "��" + this.fpOrderExecBrowser1.GetFpRowCount(0).ToString() + "����";
                this.tabPage2.Text = "��ҩƷ" + "��" + this.fpOrderExecBrowser2.GetFpRowCount(0).ToString() + "����";

                //for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                //{
                //    this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Value = true;
                //}
                //for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                //{
                //    this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexSelection].Value = true;
                //}
            }
            catch { }
            //this.fpOrderExecBrowser1.EndInit();
            //this.fpOrderExecBrowser2.EndInit(); 
            bOnQuery = false;

            return 0;
        }

        /// <summary>
        /// ��ʿվ����
        /// </summary>
        protected Neusoft.FrameWork.Models.NeuObject NurseStation
        {
            get
            {
                return ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse.Clone();
            }
        }

        
        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <returns></returns>
        public int ComfirmExec()
        {
            if (Neusoft.FrameWork.WinForms.Classes.Function.Msg("�Ƿ�ȷ��Ҫ���棿", 422) == DialogResult.No)
            {
                return -1;
            }
            this.btnSave.Enabled = false;
            Neusoft.HISFC.BizProcess.Integrate.Order orderIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Order();
            Neusoft.HISFC.BizProcess.Integrate.RADT radtIntegrate = new Neusoft.HISFC.BizProcess.Integrate.RADT();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ������ݣ����Ժ�...");
            Application.DoEvents();
            dt = this.orderManagement.GetDateTimeFromSysDateTime();

            Neusoft.HISFC.Models.Order.ExecOrder order = null;
            string inpatientNo = "";
            List<Neusoft.HISFC.Models.Order.ExecOrder> alOrders = null;
            Neusoft.HISFC.Models.RADT.PatientInfo patient = null;

            //{B2E4E2ED-08CF-41a8-BF68-B9DF7454F9BB} Ƿ���ж�
            Neusoft.HISFC.Models.Base.MessType messType =  Neusoft.HISFC.Models.Base.MessType.M;
            switch (Ƿ�Ѳ���)
            {
                case EnumLackFee.���ж�Ƿ��:
                    messType = Neusoft.HISFC.Models.Base.MessType.N;
                    break;
                case EnumLackFee.Ƿ�Ѳ�����ֽ�:
                    messType = Neusoft.HISFC.Models.Base.MessType.Y;
                    break;
                case EnumLackFee.Ƿ����ʾ�ʲ�����ֽ�:
                    messType = Neusoft.HISFC.Models.Base.MessType.M;
                    break;
            }

            orderIntegrate.MessageType = messType;
            //{B2E4E2ED-08CF-41a8-BF68-B9DF7454F9BB}
            
            for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].RowCount; i++)
            {
                if (this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Text.ToUpper() == "TRUE")
                {
                    order = this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;                   

                    if (inpatientNo != order.Order.Patient.ID)
                    {
                        if (patient != null) //��һ������
                        {
                            if (orderIntegrate.ComfirmExec(patient, alOrders, NurseStation.ID, dt, true) == -1)
                            {
                                orderIntegrate.fee.Rollback();
                                this.btnSave.Enabled = true;

                                //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                #region {C88D3BEB-EA3F-455f-BD5D-0A997699CC2C}
                                //�����ʾ��ĳ�����߳����Ժ󣬿�ѡ���Ƿ�����������˷ֽ�
                                //MessageBox.Show(orderIntegrate.Err);
                                //return -1;
                                if (this.isSaveErrContinue)
                                {
                                    if (MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(
                                        string.Format("{0}.�Ƿ����ִ�зֽ��������ߵĲ�����", orderIntegrate.Err)), "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        return -1;
                                    }
                                    this.btnSave.Enabled = true;
                                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                                    orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ������ݣ����Ժ�...");
                                    Application.DoEvents();
                                }
                                else
                                {
                                    MessageBox.Show(orderIntegrate.Err);
                                    return -1;
                                }
                                #endregion
                            }
                            //}{B3173852-136F-4c4b-9FAC-E15EB879C619}����������Ū��}��֪��Ϊʲô����ôдҪ������
                            else
                            {
                                orderIntegrate.fee.Commit();
                                //Neusoft.FrameWork.Management.Transaction 
                                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                                //t.BeginTransaction();
                                orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                            }
                        }//{B3173852-136F-4c4b-9FAC-E15EB879C619}}�����}Ӧ�������ﰡ
                        inpatientNo = order.Order.Patient.ID;
                        patient = radtIntegrate.GetPatientInfomation(inpatientNo);
                        alOrders = new List<Neusoft.HISFC.Models.Order.ExecOrder>();

                    }
                    alOrders.Add(order);

                }
            }
            if (patient != null) //��һ������
            {
                if (orderIntegrate.ComfirmExec(patient, alOrders, NurseStation.ID, dt,true) == -1)
                {
                    orderIntegrate.fee.Rollback();
                    this.btnSave.Enabled = true;
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(orderIntegrate.Err);
                    return -1;
                }
                else
                {
                    orderIntegrate.fee.Commit();
                    //Neusoft.FrameWork.Management.Transaction 
                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                    //t.BeginTransaction();
                    orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                }
            }

            alOrders = new List<Neusoft.HISFC.Models.Order.ExecOrder>();
            patient = null;
            inpatientNo = "";
            for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].RowCount; i++)
            {
                if (this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexSelection].Text.ToUpper() == "TRUE")
                {
                    order = this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;
                    if (inpatientNo != order.Order.Patient.ID)
                    {
                        if (patient != null) //��һ������
                        {
                            if (orderIntegrate.ComfirmExec(patient, alOrders, NurseStation.ID, dt,false) == -1)
                            {
                                orderIntegrate.fee.Rollback();
                                this.btnSave.Enabled = true;
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                                #region {C88D3BEB-EA3F-455f-BD5D-0A997699CC2C}
                                //�����ʾ��ĳ�����߳����Ժ󣬿�ѡ���Ƿ�����������˷ֽ�
                                //MessageBox.Show(orderIntegrate.Err);
                                //return -1;
                                if (this.isSaveErrContinue)
                                {
                                    if (MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(
                                        string.Format("{0}.�Ƿ����ִ�зֽ��������ߵĲ�����", orderIntegrate.Err)), "��ʾ", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        return -1;
                                    }
                                    this.btnSave.Enabled = false; ;
                                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                                    orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                    Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ������ݣ����Ժ�...");
                                    Application.DoEvents();
                                }
                                else
                                {
                                    MessageBox.Show(orderIntegrate.Err);
                                    return -1;
                                }
                                #endregion
                            }
                            else
                            {
                                orderIntegrate.fee.Commit();
                                //Neusoft.FrameWork.Management.Transaction 
                                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                                //t.BeginTransaction();
                                orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                                this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                            }
                        }
                        inpatientNo = order.Order.Patient.ID;
                        patient = radtIntegrate.GetPatientInfomation(inpatientNo);
                        alOrders = new List<Neusoft.HISFC.Models.Order.ExecOrder>();

                    }
                    alOrders.Add(order);

                }
            }
            if (patient != null) //��һ������
            {
                if (orderIntegrate.ComfirmExec(patient, alOrders, NurseStation.ID, dt,false) == -1)
                {
                    orderIntegrate.fee.Rollback();
                    this.btnSave.Enabled = true;
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    //Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(orderIntegrate.Err);
                    return -1;
                }
                else
                {
                    orderIntegrate.fee.Commit();
                    //Neusoft.FrameWork.Management.Transaction 
                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                    //t.BeginTransaction();
                    orderIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    radtIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                    this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                }
            }
            orderIntegrate.fee.Commit();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            //Neusoft.FrameWork.Management.PublicTrans.Commit();
            this.btnSave.Enabled = true;
            bOnQuery = false;
            this.RefreshQuery();
            return 0;
        }
        /// <summary>
        /// ���÷�ҩ��־λ
        /// </summary>
        private void SetSendFlag()
        {
            Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            ArrayList al = manager.QueryDepartment(this.NurseStation.ID);
            sendFlag = new Hashtable();
            for (int i = 0; i < al.Count; i++)
            {
                //if (Function.IsHaveDruged(((Neusoft.FrameWork.Models.NeuObject)al[i]).ID))
                //{
                //    sendFlag.Add(((Neusoft.FrameWork.Models.NeuObject)al[i]).ID, 2);//��ʱ����
                //}
                //else
                //{
                //    sendFlag.Add(((Neusoft.FrameWork.Models.NeuObject)al[i]).ID, 0);//������
                //}
            }
        }


        /// <summary>
        /// ѡ�������Ŀ
        /// {ED1068B5-53FD-4bf4-A270-49AE1A70D225}
        /// </summary>
        private void CheckFilteredData()
        {
            for (int i = 0; i < this.CurrentBrowser.fpSpread.Sheets[0].Rows.Count; i++)
            {
                if (this.CurrentBrowser.fpSpread.Sheets[0].Rows[i].BackColor == Color.LightSkyBlue)
                {
                    Neusoft.HISFC.Models.Order.ExecOrder order = new Neusoft.HISFC.Models.Order.ExecOrder();
                    order = this.CurrentBrowser.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;
                    if (order.Order.Combo.ID != "" && order.Order.Combo.ID != "0")
                    {//����Ƚ��������ͬ����Էֿ����˴���Ҫ
                        for (int j = this.CurrentBrowser.fpSpread.Sheets[0].RowCount - 1; j >= 0; j--)
                        {
                            Neusoft.HISFC.Models.Order.ExecOrder objorder = (Neusoft.HISFC.Models.Order.ExecOrder)this.CurrentBrowser.fpSpread.Sheets[0].Rows[j].Tag;
                            if (objorder.Order.Combo.ID == order.Order.Combo.ID && objorder.DateUse == order.DateUse)
                            {
                                this.CurrentBrowser.fpSpread.Sheets[0].Cells[j, 1].Value = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ������Ϲ��˺���
        /// {ED1068B5-53FD-4bf4-A270-49AE1A70D225}
        /// </summary>
        /// <param name="isMatchAll"></param>
        /// <param name="prmsDate">ִ�п�ʼ������ʱ��</param>
        /// <param name="prmsStr">���ơ�ҩ��</param>
        protected void SetFilteredFlag(bool isMatchAll, DateTime[] prmsDate, string[] prmsStr)
        {
            bool isHaveFilter = true;
            bool isAllOrderNames = (prmsStr[0] == "ȫ��");
            bool isAllDeptNames = (prmsStr[1] == "ȫ��");
            #region addby xuewj 2010-10-9 ����ҽ�������� {CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            bool isAllOrderTypeNames = (prmsStr[2] == "ȫ��"); 
            #endregion
            bool isAllTime = (prmsDate[0].ToString() == prmsDate[1].ToString());
            if (isAllOrderNames && isAllDeptNames && isAllTime && isAllOrderTypeNames)//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            {
                isHaveFilter = false;
            }

            Neusoft.HISFC.Models.Order.ExecOrder order = null;
            //��ʼ����ʾ��ҩƷ
            if (this.TabControl1.SelectedIndex == 0)
            {
                bool b = false;
                //�ָ�ԭ������ɫ
                //�����ɫ��ʾ
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (b)
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.Linen;
                    }
                    else
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                    b = !b;
                }
                if (isHaveFilter)
                {
                    for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                    {
                        order = this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;
                        DateTime splitTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 9].Text);
                        if (isMatchAll)
                        {
                            if ((isAllOrderNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 2].Text == prmsStr[0])
                             && (isAllDeptNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 11].Text == prmsStr[1])
                             && (isAllTime || (splitTime >= prmsDate[0] && splitTime <= prmsDate[1]))
                             && (isAllOrderTypeNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexOrderType].Text == prmsStr[2]))//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                            {
                                this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                            }
                        }
                        else
                        {
                            if ((isAllOrderNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 2].Text.Contains(prmsStr[0]))
                             && (isAllDeptNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 11].Text.Contains(prmsStr[1]))
                             && (isAllTime || (splitTime >= prmsDate[0] && splitTime <= prmsDate[1]))
                             && (isAllOrderTypeNames || this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexOrderType].Text == prmsStr[2]))//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                            {
                                this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                            }
                        }
                    }
                }
            }
            //��ҩƷ
            else
            {
                //�ָ�ԭ������ɫ
                bool b = false;
                //�ָ�ԭ������ɫ
                //�����ɫ��ʾ
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (b)
                    {
                        this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.Linen;
                    }
                    else
                    {
                        this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                    b = !b;
                }
                if (isHaveFilter)
                {
                    for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                    {
                        DateTime splitTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 9].Text);
                        if (isMatchAll)
                        {
                            if ((isAllOrderNames || this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 2].Text == prmsStr[0])
                             && (isAllOrderTypeNames || this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexOrderType].Text == prmsStr[2])//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                             && (isAllTime || (splitTime >= prmsDate[0] && splitTime <= prmsDate[1])))
                            {
                                this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                            }
                        }
                        else
                        {
                            if ((isAllOrderNames || this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 2].Text.Contains(prmsStr[0]))
                             && (isAllOrderTypeNames || this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexOrderType].Text == prmsStr[2])//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
                             && (isAllTime || (splitTime >= prmsDate[0] && splitTime <= prmsDate[1])))
                            {
                                this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                            }
                        }
                    }
                }
            }
        }
       
        #endregion 

        #region �¼�
        //���ֽ�ҽ��
        private void fpSpread_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Neusoft.HISFC.Models.Order.ExecOrder order = null;
            order = this.CurrentBrowser.CurrentExecOrder;

            if (order == null) return;
            if (MessageBox.Show("ȷ�ϲ��ֽ�" + order.DateUse.ToString() + "��ҽ��[" + order.Order.Item.Name + "] ?", "��ʾ", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            //����ִ�е�ҽ��Ϊ�����
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.orderManagement.Connection);
            //t.BeginTransaction();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            this.orderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
            obj = this.orderManagement.Operator;
            ArrayList alDel = new ArrayList();
            if (order.Order.Combo.ID == "" || order.Order.Combo.ID == "0")
            {
                if (this.orderManagement.DcExecImmediate((Neusoft.HISFC.Models.Order.Order)order.Order, obj) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(this.orderManagement.Err);
                    return;
                }
                this.CurrentBrowser.fpSpread.Sheets[0].Rows.Remove(this.fpOrderExecBrowser1.fpSpread.Sheets[0].ActiveRowIndex, 1);
            }
            else //���ҽ����������Ϻ���ͬ��ʹ��ʱ����ͬ��ҽ��
            {
                for (int i = this.CurrentBrowser.fpSpread.Sheets[0].RowCount - 1; i >= 0; i--)
                {
                    Neusoft.HISFC.Models.Order.ExecOrder objorder = (Neusoft.HISFC.Models.Order.ExecOrder)this.CurrentBrowser.fpSpread.Sheets[0].Rows[i].Tag;
                    if (objorder.Order.Combo.ID == order.Order.Combo.ID && objorder.DateUse == order.DateUse)
                    {
                        if (this.orderManagement.DcExecImmediate(objorder, obj) <= 0)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                            MessageBox.Show(this.orderManagement.Err);
                            return;
                        }
                        //alDel.Add(i);
                        this.CurrentBrowser.fpSpread.Sheets[0].Rows.Remove(i, 1);
                    }
                }
            }

        
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            this.tabPage1.Text = "��" + this.fpOrderExecBrowser1.GetFpRowCount(0).ToString() + "����";
            this.tabPage2.Text = "��" + this.fpOrderExecBrowser2.GetFpRowCount(0).ToString() + "����";

        }
        /// <summary>
        /// ���ص�ǰ��FpSpreadҳ
        /// </summary>
        protected fpOrderExecBrowser CurrentBrowser
        {
            get
            {
                if (this.TabControl1.SelectedIndex == 0)
                {
                    return this.fpOrderExecBrowser1;
                }
                else
                {
                    return this.fpOrderExecBrowser2;
                }
            }
        }
       

        /// <summary>
        /// ���¼��㰴ť�¼� �Ѿ�����ʹ�� Ĭ��Ϊ1��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCaculate_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("ȷ��Ҫ���¼��㲢�ֽ�ҽ����?\n������Ҫһ��ʱ�䣡", "��ʾ", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.intDyas = (int)(this.txtDays.Value);
                RefreshExec();
            }
        }

        /// <summary>
        /// fpSpread_ButtonClicked�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (this.CurrentBrowser.fpSpread.Sheets[0].RowCount <= 0)
            {
                return;
            }
            if (e.Column == 1)
            {
                string checkValue = this.CurrentBrowser.fpSpread.Sheets[0].Cells[e.Row, e.Column].Text;

                Neusoft.HISFC.Models.Order.ExecOrder order = new Neusoft.HISFC.Models.Order.ExecOrder();
                order = this.CurrentBrowser.CurrentExecOrder;
                if (order.Order.Combo.ID != "" && order.Order.Combo.ID != "0")
                {
                    for (int i = this.CurrentBrowser.fpSpread.Sheets[0].RowCount - 1; i >= 0; i--)
                    {
                        Neusoft.HISFC.Models.Order.ExecOrder objorder = (Neusoft.HISFC.Models.Order.ExecOrder)this.CurrentBrowser.fpSpread.Sheets[0].Rows[i].Tag;
                        if (objorder.Order.Combo.ID == order.Order.Combo.ID && objorder.DateUse == order.DateUse)
                        {
                            this.CurrentBrowser.fpSpread.Sheets[0].Cells[i, 1].Text = checkValue;
                        }
                    }
                }
            }
        }

    
        /// <summary>
        /// �����ѯ������,��ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //�س�����
            if (e.KeyCode != Keys.Enter)
                return;
            string name = this.txtName.Text.Trim();
            if (name == "") return;

            this.SetDrugFlag(name, false);
        }

        /// <summary>
        /// ȫѡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            bool b = false;
            if (this.chkAll.Checked)
            { //ȫѡ
                b = true;
            }
            else
            {//ȡ��
                b = false;
            }
            if (this.TabControl1.SelectedIndex == 0)
            {
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Value = b;
                    tab0AllSelect = b;
                }
            }
            else
            {
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexSelection].Value = b;
                    tab1AllSelect = b;
                }
            }
        }
        /// <summary>
        /// ���¼���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbtnCalculate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg(string.Format("�Ƿ�ֽ�{0}���ҽ����Ϣ��", this.txtDays.Value.ToString())),
                "��ʾ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //{5617A654-9738-4db4-A006-5A80B44F0841} ���¼���ʱ��Ҫ�Ի����б�ֵ
                this.alPatients = this.GetSelectedTreeNodes();

                intDyas = (int)this.txtDays.Value;
                this.RefreshExec();
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TabControl1.SelectedIndex == 0)
            {
                this.chkAll.Checked = tab0AllSelect;
            }
            else
            {
                this.chkAll.Checked = tab1AllSelect;
            }

            #region {8F9EBE06-9117-457a-8FED-5FEB9A9FD619}

            dtpEndTime_ValueChanged(null, null); 

            #endregion

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ComfirmExec();
            this.btnSave.Enabled = true;

        }

        private void txtDays_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime[] prmsDate = new DateTime[] { this.dtpBeginTime.Value, this.dtpEndTime.Value };
            string[] prmsStr = new string[] { this.txtName.Text, this.txtDrugDeptName.Text, this.cmbOrderTypeName.Text };//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            SetFilteredFlag(false, prmsDate, prmsStr);
        }

        private void cmbOrderTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime[] prmsDate = new DateTime[] { this.dtpBeginTime.Value, this.dtpEndTime.Value };
            string[] prmsStr = new string[] { this.txtName.Text, this.txtDrugDeptName.Text, this.cmbOrderTypeName.Text };//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            SetFilteredFlag(false, prmsDate, prmsStr);
        }

        private void txtDrugDeptName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime[] prmsDate = new DateTime[] { this.dtpBeginTime.Value, this.dtpEndTime.Value };
            string[] prmsStr = new string[] { this.txtName.Text, this.txtDrugDeptName.Text, this.cmbOrderTypeName.Name };//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            SetFilteredFlag(false, prmsDate, prmsStr);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            CheckFilteredData();
        }

        private void dtpBeginTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime[] prmsDate = new DateTime[] { this.dtpBeginTime.Value, this.dtpEndTime.Value };
            string[] prmsStr = new string[] { this.txtName.Text, this.txtDrugDeptName.Text, this.cmbOrderTypeName.Text };//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            SetFilteredFlag(false, prmsDate, prmsStr);
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime[] prmsDate = new DateTime[] { this.dtpBeginTime.Value, this.dtpEndTime.Value };
            string[] prmsStr = new string[] { this.txtName.Text, this.txtDrugDeptName.Text, this.cmbOrderTypeName.Text };//{CA8705F5-C25E-4126-BF15-F498AE82AFAE}
            SetFilteredFlag(false, prmsDate, prmsStr);
        }
        #endregion

        #region ��̫̫
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.InitControl();
            TreeView tv = sender as TreeView;
            if (tv != null && this.tv.CheckBoxes == false)
                tv.CheckBoxes = true;
            return null;
        }
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            if (tv != null && tv.CheckBoxes == false)
                tv.CheckBoxes = true;
            return base.OnSetValue(neuObject, e);
        }
        protected override int OnSetValues(ArrayList alValues, object e)
        {
            //{5617A654-9738-4db4-A006-5A80B44F0841} ��ѯʱҲ��Ҫ��������ֵ
            intDyas = (int)this.txtDays.Value;

            this.alPatients = alValues;
            this.RefreshExec();
            return 0;
        }

        protected override int OnSave(object sender, object neuObject)
        {
            return ComfirmExec();
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int Print(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
            p.PrintPreview(this.TabControl1);
            return 0;
        }

        /// <summary>
        /// ���ô�ӡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int SetPrint(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
            p.ShowPageSetup();
            p.PrintPreview(this.TabControl1);
            return 0;
        }
        #endregion

        #region ��ʱ���õĴ��룬��ǰ����ɾ��
        #region {13EAF764-E1CA-4d5a-8250-056AD1DEE61B}
        [System.Obsolete("���Ϲ��ܣ�����ˢ�º��������")]
        private void addComboDept(string name)
        {
            if (this.txtDrugDeptName.FindStringExact(name) >= 0) return;
            this.txtDrugDeptName.Items.Add(name);
        }

        [System.Obsolete("���Ϲ��ܣ�����ˢ�º��������")]
        private void addCombo(string name)
        {
            if (this.txtName.FindStringExact(name) >= 0) return;
            this.txtName.Items.Add(name);
        }
        /// <summary>
        /// ��ָ��ҩ����ҽ��������ʾ
        /// </summary>
        /// <param name="filter">ҩ������</param>
        /// <param name="isMatchAll">�Ƿ���Ҫȫ��ƥ��</param>
        protected void SetDrugDeptFlag(string filter, bool isMatchAll)
        {
            Neusoft.HISFC.Models.Order.ExecOrder order = null;
            for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
            {
                this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Value = false;
            }
            //��ʼ����ʾ��ҩƷ
            if (this.TabControl1.SelectedIndex == 0)
            {
                bool b = false;
                //�ָ�ԭ������ɫ
                //�����ɫ��ʾ
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (b)
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.Linen;
                    }
                    else
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                    b = !b;
                }
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    order = this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;
                    if (isMatchAll)
                    {
                        if (this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 11].Text == filter)
                        {
                            this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;

                        }
                    }
                    else
                    {
                        if (this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 11].Text.IndexOf(filter) != -1)
                        {
                            this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;

                        }
                    }
                }
            }
        }
        /// <summary>
        /// ����ʱ���ѡ����Ŀ{ED1068B5-53FD-4bf4-A270-49AE1A70D225}
        /// </summary>
        private void SelectByTime()
        {
            DateTime beginTime = this.dtpBeginTime.Value;
            DateTime endTime = this.dtpEndTime.Value;

            //ȡ���Ѿ�ѡ�����Ŀ;
            if (this.TabControl1.SelectedIndex == 0)
            {
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Value = false;
                }
            }
            else
            {
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexSelection].Value = false;
                }
            }
            //ѡ��ʱ����ڵķֽ���Ŀ
            if (this.TabControl1.SelectedIndex == 0)
            {
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    DateTime splitTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 9].Text);
                    if (splitTime >= beginTime && splitTime <= endTime)
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser1.ColumnIndexSelection].Value = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    DateTime splitTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 9].Text);
                    if (splitTime >= beginTime && splitTime <= endTime)
                    {
                        this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, this.fpOrderExecBrowser2.ColumnIndexSelection].Value = true;
                    }
                }
            }
        }

        /// <summary>
        /// ���ƶ�ҽ����������ɫ��ʾ
        /// </summary>
        /// <param name="filterStr">����ƥ�������</param>
        /// <param name="isMatchingAll">�Ƿ���ȫ��ƥ��</param>
        protected void SetDrugFlag(string filterStr, bool isMatchingAll)
        {
            Neusoft.HISFC.Models.Order.ExecOrder order = null;
            //��ʼ����ʾ��ҩƷ
            if (this.TabControl1.SelectedIndex == 0)
            {
                bool b = false;
                //�ָ�ԭ������ɫ
                //�����ɫ��ʾ
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (b)
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.Linen;
                    }
                    else
                    {
                        this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                    b = !b;
                }
                for (int i = 0; i < this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    order = this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].Tag as Neusoft.HISFC.Models.Order.ExecOrder;
                    if (isMatchingAll)
                    {
                        if (this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 2].Text == filterStr)
                        {
                            this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                        }
                    }
                    else
                    {
                        if (this.fpOrderExecBrowser1.fpSpread.Sheets[0].Cells[i, 2].Text.IndexOf(filterStr) != -1)
                        {
                            this.fpOrderExecBrowser1.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                        }
                    }
                }
            }
            //��ҩƷ
            else
            {
                //�ָ�ԭ������ɫ
                bool b = false;
                //�ָ�ԭ������ɫ
                //�����ɫ��ʾ
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (b)
                    {
                        this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.Linen;
                    }
                    else
                    {
                        this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                    b = !b;
                }
                for (int i = 0; i < this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows.Count; i++)
                {
                    if (isMatchingAll)
                    {
                        if (this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 2].Text == filterStr)
                        {
                            this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                        }
                    }
                    else
                    {
                        if (this.fpOrderExecBrowser2.fpSpread.Sheets[0].Cells[i, 2].Text.IndexOf(filterStr) != -1)
                        {
                            this.fpOrderExecBrowser2.fpSpread.Sheets[0].Rows[i].BackColor = Color.LightSkyBlue;
                        }
                    }
                }
            }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumLackFee
    {
        ���ж�Ƿ��,
        Ƿ�Ѳ�����ֽ�,
        Ƿ����ʾ�ʲ�����ֽ�
    }
}
