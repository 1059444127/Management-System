using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;
using Neusoft.FrameWork.Function;

namespace Neusoft.HISFC.Components.Pharmacy.Pay
{
    /// <summary>
    /// [��������: �����̽��]<br></br>
    /// [�� �� ��: liangjz]<br></br>
    /// [����ʱ��: 2006-12]<br></br>
    /// </summary>
    public partial class ucPay : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucPay()
        {
            InitializeComponent();
        }        

        #region ����

        /// <summary>
        /// ���а�����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper bankHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ��Ա������
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper personHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ������˾������
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper companyHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ҩƷ������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// �����ϸ��Ϣ
        /// </summary>
        private ArrayList payDetail = new ArrayList();

        /// <summary>
        /// ������˾
        /// </summary>
        private Neusoft.HISFC.Models.Pharmacy.Company company;

        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept;

        /// <summary>
        /// ����Ա
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privOper;

        /// <summary>
        /// ��ѯ��ʼʱ��
        /// </summary>
        private DateTime dtBegin = System.DateTime.Now.Date;//System.DateTime.MinValue;

        /// <summary>
        /// ��ѯ��ֹʱ��
        /// </summary>
        private DateTime dtEnd = System.DateTime.Now.Date.AddDays(1);//System.DateTime.MaxValue;

        /// <summary>
        /// ��ѯ����־
        /// </summary>
        private string payFlag;

        /// <summary>
        /// ������λ
        /// </summary>
        private ArrayList alCompany = new ArrayList();

        #endregion

        #region ����
      
        /// <summary>
        /// ������˾
        /// </summary>
        protected Neusoft.HISFC.Models.Pharmacy.Company Company
        {
            set
            {
                if (value == null)
                    this.company = new Neusoft.HISFC.Models.Pharmacy.Company();
                else
                    this.company = value;

                this.lbCompany.Text = "��浥λ��" + this.company.Name;
            }
        }    

        #endregion

        #region ������

        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object NeuObject, object param)
        {
            toolBarService.AddToolButton("ʱ  ��", "���ò�ѯʱ��", Neusoft.FrameWork.WinForms.Classes.EnumImageList.C��ѯ��ʷ, true, false, null);
            toolBarService.AddToolButton("�ѽ���ѯ", "�ѽ�浥�ݲ�ѯ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.A����, true, false, null);
            toolBarService.AddToolButton("δ����ѯ", "δ��浥�ݲ�ѯ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.P�̵�����, true, false, null);
            toolBarService.AddToolButton("������λ", "ѡ���ѯ�Ĺ�����λ", Neusoft.FrameWork.WinForms.Classes.EnumImageList.J����, true, false, null);

            toolBarService.AddToolButton("ѡ��", "�Ե�ǰ�������ݽ��з���ѡ��", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȫѡ, true, false, null);

            return toolBarService;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "ʱ  ��")
            {
                if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseDate(ref this.dtBegin, ref this.dtEnd) == 0)
                {
                    return;
                }
            }
            if (e.ClickedItem.Text == "δ����ѯ")
            {
                this.Query("'0','1'", dtBegin, dtEnd);

                this.payFlag = "'0','1'";
            }
            if (e.ClickedItem.Text == "�ѽ���ѯ")
            {
                this.Query("'2'", dtBegin, dtEnd);

                this.payFlag = "2";
            }
            if (e.ClickedItem.Text == "������λ")
            {
                this.ShowCompany();
            }
            if (e.ClickedItem.Text == "ѡ��")
            {
                for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                {
                    this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColChoose].Value = !Neusoft.FrameWork.Function.NConvert.ToBoolean(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColChoose].Value);
                }
            }
            base.ToolStrip_ItemClicked(sender, e);
        }

        protected override int OnSave(object sender, object NeuObject)
        {
            

            int chooseCount = 0;
            
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                bool isChoose = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColChoose].Value);
                if (isChoose)
                {
                    chooseCount++;
                }
            }

            if (this.rbUnPay.Checked)
            {
                if (this.neuSpread1_Sheet1.RowCount == 0)
                {
                    MessageBox.Show("û��Ҫ���ĵ���");
                    return -1;
                }
                else
                {
                    if (chooseCount == 0)
                    {
                        MessageBox.Show("��ѡ�񵥾�");
                        return -1;
                    }
                }
               
               
            }


            if (this.rbPay.Checked)
            {
                if (chooseCount == 0)
                {
                    //����ʾ
                }
                else
                {
                    MessageBox.Show(Language.Msg("�ѽ�浥�ݲ����ٴα���"), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                return -1;
            }

            this.SavePay();

            this.Query(this.payFlag, this.dtBegin, this.dtEnd);

            return 1;
        }

        public override int Export(object sender, object NeuObject)
        {
            if (this.neuSpread2.Export() == 1)
            {
                MessageBox.Show(Language.Msg("�����ɹ�"));
            }
            return 1;
        }

        protected override int OnQuery(object sender, object neuObject)
        {
            this.Query(this.payFlag, dtBegin, dtEnd);

            return base.OnQuery(sender, neuObject);
        }

        #endregion

        /// <summary>
        /// ���ݳ�ʼ��
        /// </summary>
        protected void Init()
        {
            ArrayList al = new ArrayList();

            #region ����

            Neusoft.HISFC.BizLogic.Manager.Constant constantManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
            al = constantManager.GetList("BANK");
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡ�����б�ʧ��" + constantManager.Err));
                return;
            }
            bankHelper.ArrayObject = al;

            #endregion

            #region ��Ա

            Neusoft.HISFC.BizLogic.Manager.Person personManager = new Neusoft.HISFC.BizLogic.Manager.Person();
            al = personManager.GetEmployeeAll();
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������Ա�б�" + personManager.Err));
                return;
            }
            this.personHelper.ArrayObject = al;

            #endregion

            #region ������λ

            Neusoft.HISFC.BizLogic.Pharmacy.Constant constant = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
            this.alCompany = constant.QueryCompany("1");
            if (this.alCompany == null)
            {
                MessageBox.Show(constant.Err);
                return;
            }
            //{49390DE5-B54F-4b15-A012-208CDF288FF5}  ��ѡ��ȫ��������˾ ���������б�ѡ����
            Neusoft.HISFC.Models.Pharmacy.Company rootCompany = new Neusoft.HISFC.Models.Pharmacy.Company();
            rootCompany.ID = "AAAA";
            rootCompany.Name = "ȫ��������˾";

            this.alCompany.Insert(0, rootCompany);

            this.companyHelper = new Neusoft.FrameWork.Public.ObjectHelper(this.alCompany);

            #endregion

            Neusoft.FrameWork.Management.DataBaseManger dataBaseManager = new Neusoft.FrameWork.Management.DataBaseManger();
            DateTime sysTime = dataBaseManager.GetDateTimeFromSysDateTime().Date.AddDays(1);
            this.dtBegin = sysTime.AddDays(-30);
            this.dtEnd = sysTime;

            this.privOper = dataBaseManager.Operator;

            this.payFlag = "'0','1'";
        }

        /// <summary>
        /// ��ʾ������˾
        /// </summary>
        protected void ShowCompany()
        {         
            Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
            if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(this.alCompany, ref info) == 0)
            {
                return;
            }
            else
            {
                this.Company = (Neusoft.HISFC.Models.Pharmacy.Company)info;

                this.Query(this.payFlag, this.dtBegin, this.dtEnd);
            }
        }

        /// <summary>
        /// ��ʾ����ѡ���б�   //{49390DE5-B54F-4b15-A012-208CDF288FF5}  ��ѡ��ȫ��������˾
        /// </summary>
        protected void ShowBank(int rowIndex)
        {
            Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();
            if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(this.bankHelper.ArrayObject, ref info) == 0)
            {
                return;
            }
            else
            {
                this.neuSpread1_Sheet1.Cells[rowIndex, (int)ColPayHeadSet.ColOpenBank].Text = info.Name;
            }
        }

        /// <summary>
        /// ���ݽ���ǲ�ѯ��������Ϣ
        /// </summary>
        /// <param name="payFlag">����� 0δ����  1�Ѹ��� 2��ɸ���</param>
        /// <param name="dtBegin">��ѯ��ʼʱ��</param>
        /// <param name="dtEnd">��ѯ����ʱ��</param>
        public void Query(string payFlag, DateTime dtBegin, DateTime dtEnd)
        {
            if (this.company == null)
            {
                MessageBox.Show(Language.Msg("��ѡ�񹩻���˾"));
                return;
            }
            ArrayList al = new ArrayList();
            al = this.itemManager.QueryPayList(this.privDept.ID, this.company.ID, payFlag, dtBegin, dtEnd);
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡ��������Ϣʧ��" + this.itemManager.Err));
                return;
            }

            this.payDetail = new ArrayList();
            this.neuSpread1_Sheet1.Rows.Count = 0;
            this.neuSpread2_Sheet1.Rows.Count = 0;
            Neusoft.HISFC.Models.Pharmacy.Pay info;

            for (int i = 0; i < al.Count; i++)
            {
                info = al[i] as Neusoft.HISFC.Models.Pharmacy.Pay;
                if (info == null)
                {
                    MessageBox.Show(Language.Msg("�����" + (i + 1).ToString() + "�н�������Ϣ����"));
                    continue;
                }
                ArrayList alTemp = this.itemManager.QueryPayDetail(info.ID, info.InvoiceNO);
                if (alTemp == null)
                {
                    MessageBox.Show(Language.Msg("��ȡ��" + (i + 1).ToString() + "�н����ϸ��Ϣ����" + this.itemManager.Err));
                    continue;
                }
                if (alTemp.Count >= 0)
                {
                    this.payDetail.Add(alTemp);
                }

                this.AddPayHeadData(info);
            }
        }

        /// <summary>
        /// ���������ϢFarPoint�ڼ�������
        /// </summary>
        /// <param name="pay">�����̽��ʵ��</param>
        protected void AddPayHeadData(Neusoft.HISFC.Models.Pharmacy.Pay pay)
        {
            int rowCount = this.neuSpread1_Sheet1.Rows.Count;

            this.neuSpread1_Sheet1.Rows.Add(rowCount, 1);

            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColChoose].Value = true;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColInvoiceNo].Text = pay.InvoiceNO;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColInvoiceDate].Value = pay.InvoiceTime;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColInvoiceCost].Value = pay.PurchaseCost;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColDiscountCost].Value = pay.DisCountCost;
            //Ӧ�����ͨ��FarPoint��ʽ�Զ�����
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColPaidUpCost].Value = pay.PayCost;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColPayCost].Value = pay.UnPayCost;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColDeliveryCost].Value = pay.DeliveryCost;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColPayType].Value = pay.PayType;

            if (pay.Company.OpenBank == null || pay.Company.OpenBank == "")
            {
                this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColOpenBank].Value = this.company.OpenBank;
            }
            else
            {
                this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColOpenBank].Value = pay.Company.OpenBank;
            }

            if (pay.Company.OpenAccounts == null || pay.Company.OpenAccounts == "")
            {
                this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColOpenAccounts].Value = this.company.OpenAccounts;
            }
            else
            {
                this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColOpenAccounts].Value = pay.Company.OpenAccounts;
            }

            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColDrugDept].Value = this.privDept.Name;
            this.neuSpread1_Sheet1.Cells[rowCount, (int)ColPayHeadSet.ColInListCode].Value = pay.InListNO;

            this.neuSpread1_Sheet1.Rows[rowCount].Tag = pay;
        }

        /// <summary>
        /// ������ϸFarPoint�ڼ�������
        /// </summary>
        /// <param name="al">�����̽��ʵ������</param>
        protected void AddPayDetailData(ArrayList al)
        {
            foreach (Neusoft.HISFC.Models.Pharmacy.Pay pay in al)
            {
                int rowCount = this.neuSpread2_Sheet1.Rows.Count;
                this.neuSpread2_Sheet1.Rows.Add(rowCount, 1);

                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColInvoiceNo].Value = pay.InvoiceNO;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColPayCost].Value = pay.PayCost;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColDeliveryCost].Value = pay.DeliveryCost;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColPayType].Value = pay.PayType;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColOpenBank].Value = pay.Company.OpenBank;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColOpenAccounts].Value = pay.Company.OpenAccounts;
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColPayOper].Value = this.personHelper.GetName(pay.PayOper.ID);
                this.neuSpread2_Sheet1.Cells[rowCount, (int)ColPayDetailSet.ColPayDate].Text = pay.Oper.OperTime.ToString();

                this.neuSpread2_Sheet1.Rows[rowCount].Tag = pay;
            }
        }

        /// <summary>
        /// ��ʾ�����̽����Ϣ
        /// </summary>
        public void ShowPayDetail()
        {
            if (this.payDetail != null && this.payDetail.Count > 0)
            {
                this.neuSpread2_Sheet1.Rows.Count = 0;

                if (this.payDetail.Count <= this.neuSpread1_Sheet1.ActiveRowIndex)
                {
                    return;
                }

                this.AddPayDetailData(this.payDetail[this.neuSpread1_Sheet1.ActiveRowIndex] as ArrayList);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void Clear()
        {
            this.neuSpread1_Sheet1.Rows.Count = 0;
            this.neuSpread2_Sheet1.Rows.Count = 0;
            this.lbCompany.Text = "��浥λ��";
        }

        /// <summary>
        /// ������Ч���ж�
        /// </summary>
        /// <returns>�����Ƿ�������</returns>
        protected bool SaveValid()
        {
            if (this.neuSpread1_Sheet1.Rows.Count <= 0)
            {
                return false;
            }

            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                decimal payCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColPayCost].Value);
                decimal due = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColDue].Value);
                decimal paidUp = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColPaidUpCost].Value);
                if (payCost > due - paidUp)
                {
                    MessageBox.Show(Language.Msg("��Ʊ��" + this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColInvoiceNo].Text + " ���θ���ܴ���δ������"));
                    return false;
                }
                if (this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColPayType].Text == "֧Ʊ")
                {
                    if (this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColOpenBank].Text == "")
                    {
                        MessageBox.Show(Language.Msg("��Ʊ��" + this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColInvoiceNo].Text + " ��������Ϊ֧Ʊʱ����д��������"));
                        return false;
                    }
                    if (this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColOpenAccounts].Text == "")
                    {
                        MessageBox.Show(Language.Msg("��Ʊ��" + this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColInvoiceNo].Text + " ��������Ϊ֧Ʊʱ����д�����ʺ�"));
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int SavePay()
        {
            if (!this.SaveValid())
            {
                return -1;
            }

            DialogResult rs = MessageBox.Show("ȷ�϶Ե�ǰѡ�еķ�Ʊ���н����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.No)
            {
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            this.itemManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            Neusoft.HISFC.Models.Pharmacy.Pay pay;
            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                if (this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColChoose].Value == null || !((bool)this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColChoose].Value))
                {
                    continue;
                }

                pay = this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Pharmacy.Pay;
                if (pay == null)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Language.Msg("������������� ��������ת������"));
                    return -1;
                }
                //�ѽ�� ���ٴδ���
                if (pay.PayState == "2")
                {
                    continue;
                }

                if (pay.DisCountCost <= 0)
                {
                    //�Żݽ��
                    pay.DisCountCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColDiscountCost].Value);
                    pay.UnPayCost = pay.UnPayCost - pay.DisCountCost;		//δ�����
                }
                //�˷�
                pay.DeliveryCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColDeliveryCost].Value);
                pay.Oper.ID = this.privOper.ID;
                pay.Oper.OperTime = this.itemManager.GetDateTimeFromSysDateTime();

                if (this.itemManager.UpdateInsertPayHead(pay) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Language.Msg("���¹����̽����Ϣ����" + this.itemManager.Err));
                    return -1;
                }

                //��������
                pay.PayType = this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColPayType].Text;
                if (pay.PayType == "")
                {
                    pay.PayType = "�ֽ�";
                }

                //��������
                pay.Company.OpenBank = this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColOpenBank].Text;
                //�����ʺ�
                pay.Company.OpenAccounts = this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColOpenAccounts].Text;
                pay.PayOper.ID = this.privOper.ID;
                //���θ���
                pay.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, (int)ColPayHeadSet.ColPayCost].Value);

                if (pay.PayCost == 0)
                {
                    continue;
                }

                pay.UnPayCost = pay.UnPayCost - pay.PayCost;

                if (this.itemManager.Pay(pay.ID, pay) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Language.Msg("���湩���̽����Ϣ����" + this.itemManager.Err));
                    return -1;
                }
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show(Language.Msg("����ɹ�"));
            return 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper() != "DEVENV")
            {
                Neusoft.FrameWork.Models.NeuObject testPrivDept = new Neusoft.FrameWork.Models.NeuObject();
                int parma = Neusoft.HISFC.Components.Common.Classes.Function.ChoosePivDept("0314", ref testPrivDept);

                if (parma == -1)            //��Ȩ��
                {
                    MessageBox.Show(Language.Msg("���޴˴��ڲ���Ȩ��"));
                    return;
                }
                else if (parma == 0)       //�û�ѡ��ȡ��
                {
                    return;
                }

                this.privDept = testPrivDept;

                base.OnStatusBarInfo(null, "�������ң� " + testPrivDept.Name);

                this.Init();

                this.rbPay.Visible = false;
                this.rbUnPay.Visible = false;
            }

            base.OnLoad(e);
        }

        private void fpSpread1_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            // //{49390DE5-B54F-4b15-A012-208CDF288FF5}  ��ѡ��ȫ��������˾
            int rowIndex = this.neuSpread1_Sheet1.ActiveRowIndex;
            if (rowIndex >= 0)
            {
                Neusoft.HISFC.Models.Pharmacy.Pay info = this.neuSpread1_Sheet1.Rows[rowIndex].Tag as Neusoft.HISFC.Models.Pharmacy.Pay;
                if (this.company != null && this.company.ID == "AAAA")
                {
                    this.lbCompany.Text = "��浥λ��" + this.company.Name + "        ��ǰ��" + this.companyHelper.GetName(info.Company.ID);
                }
            }

            this.ShowPayDetail();
        }       

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //{49390DE5-B54F-4b15-A012-208CDF288FF5}  ��ѡ��ȫ��������˾
            if (e.Column == (int)ColPayHeadSet.ColOpenBank)
            {
                this.ShowBank(this.neuSpread1_Sheet1.ActiveRowIndex);
            }
        }

        private void rbUnPay_CheckedChanged(object sender, EventArgs e)
        {
            this.payFlag = "'0','1'";

            this.Query(null, null);
        }

        private void rbPay_CheckedChanged(object sender, EventArgs e)
        {
            this.payFlag = "2";

            this.Query(null, null);
        }

        #region ��ö��
        /// <summary>
        /// ��������Ϣ������
        /// </summary>
        enum ColPayHeadSet
        {
            /// <summary>
            /// �Ƿ񸶿� 0
            /// </summary>
            ColChoose,
            /// <summary>
            /// ��Ʊ�� 1
            /// </summary>
            ColInvoiceNo,
            /// <summary>
            /// ��Ʊ����	2
            /// </summary>
            ColInvoiceDate,
            /// <summary>
            /// ��Ʊ���	3
            /// </summary>
            ColInvoiceCost,
            /// <summary>
            /// �Żݽ��	4
            /// </summary>
            ColDiscountCost,
            /// <summary>
            /// Ӧ�����	5
            /// </summary>
            ColDue,
            /// <summary>
            /// �Ѹ����	6
            /// </summary>
            ColPaidUpCost,
            /// <summary>
            /// ���θ���	7
            /// </summary>
            ColPayCost,
            /// <summary>
            /// �˷�		8
            /// </summary>
            ColDeliveryCost,
            /// <summary>
            /// ��������	9
            /// </summary>
            ColPayType,
            /// <summary>
            /// ��������	10
            /// </summary>
            ColOpenBank,
            /// <summary>
            /// �����ʺ�	11
            /// </summary>
            ColOpenAccounts,
            /// <summary>
            /// ������	12
            /// </summary>
            ColDrugDept,
            /// <summary>
            /// ��ⵥ�ݺ�	13
            /// </summary>
            ColInListCode
        }
        /// <summary>
        /// ��渶����ϸ��Ϣ��������
        /// </summary>
        enum ColPayDetailSet
        {
            /// <summary>
            /// ��Ʊ��
            /// </summary>
            ColInvoiceNo,
            /// <summary>
            /// ������
            /// </summary>
            ColPayCost,
            /// <summary>
            /// �˷�
            /// </summary>
            ColDeliveryCost,
            /// <summary>
            /// ��������
            /// </summary>
            ColPayType,
            /// <summary>
            /// ��������
            /// </summary>
            ColOpenBank,
            /// <summary>
            /// �����ʺ�
            /// </summary>
            ColOpenAccounts,
            /// <summary>
            /// ������
            /// </summary>
            ColPayOper,
            /// <summary>
            /// ��������
            /// </summary>
            ColPayDate
        }
        #endregion             
    }
}
