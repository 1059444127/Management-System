using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.Fee.Outpatient;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.OutpatientFee.Forms
{
    /// <summary>
    /// frmReprint<br></br>
    /// [��������: ���﷢Ʊ�ش�]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2006-3-16]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class frmReprint : Form
    {
        /// <summary>
        /// ���췽��
        /// </summary>
        public frmReprint()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// ҩƷҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmacyIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        
        /// <summary>
        /// �������ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizLogic.Fee.Outpatient outpatientManager = new Neusoft.HISFC.BizLogic.Fee.Outpatient();

        /// <summary>
        /// �����ۺ�ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee feeIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ��ǰ�շѷ�Ʊ
        /// </summary>
        protected Neusoft.HISFC.Models.Fee.Outpatient.Balance currentBalance = new Neusoft.HISFC.Models.Fee.Outpatient.Balance();

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        protected string invoiceType = string.Empty;

        /// <summary>
        /// ���Ʋ���ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        
        /// <summary>
        /// ��Ʊ��ϸ��Ϣ
        /// </summary>
        protected ArrayList comBalanceLists = new ArrayList();

        /// <summary>
        /// ������ϸ��Ϣ
        /// </summary>
        protected ArrayList comFeeItemLists = new ArrayList();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        ArrayList comBalances = new ArrayList();

        /// <summary>
        /// ֧����Ϣ
        /// </summary>
        ArrayList comBalancePays = new ArrayList();

        #endregion

        #region ����

        /// <summary>
        /// ���
        /// </summary>
        protected virtual void Clear()
        {
            comBalanceLists = new ArrayList();
            comFeeItemLists = new ArrayList();
            comBalances = new ArrayList();
            comBalancePays = new ArrayList();
            currentBalance = new Neusoft.HISFC.Models.Fee.Outpatient.Balance();
            this.tbInvoiceDate.Text = "";
            this.tbInvoiceNo.Text = "";
            this.tbPName.Text = "";
            this.tbOperName.Text = "";
            this.tbOwnCost.Text = "";
            this.tbPayCost.Text = "";
            this.tbPubCost.Text = "";
            this.tbTotCost.Text = "";
            this.fpSpread1_Sheet1.RowCount = 0;
            this.fpSpread1_Sheet1.RowCount = 5;
            this.tbInvoiceNo.Focus();
            this.tbPactInfo.Text = "";
        }

        /// <summary>
        /// ��÷�Ʊ��Ϣ
        /// </summary>
        protected virtual void QueryBalances()
        {

            string invoiceNo = this.tbInvoiceNo.Text.Trim();
            this.Clear();
            invoiceNo = invoiceNo.PadLeft(12, '0');
            comBalances = outpatientManager.QueryBalancesSameInvoiceCombNOByInvoiceNO(invoiceNo);
            if (comBalances == null)
            {
                MessageBox.Show("��÷�Ʊ��Ϣ����!" + outpatientManager.Err);
                currentBalance = null;

                return;
            }
            if (comBalances.Count == 0)
            {
                MessageBox.Show("������ķ�Ʊ���벻����,���֤������");
                currentBalance = null;
                this.tbInvoiceNo.SelectAll();
                this.tbInvoiceNo.Focus();

                return;
            }

            Balance balanceInfo = comBalances[0] as Balance;
            if (balanceInfo.Patient.Pact.ID != "01")
            {
                MessageBox.Show("�����ֽ��ߣ����ܴ�ӡ");
                return;
            }

            decimal totCost = 0, ownCost = 0, payCost = 0, pubCost = 0;
            if (comBalances.Count > 1)
            {
                bool isSelect = false;
                string SeqNo = "";
                foreach (Balance balance in comBalances)
                {
                    if (SeqNo == "")
                    {
                        SeqNo = balance.CombNO;

                        continue;
                    }
                    else
                    {
                        if (SeqNo != balance.CombNO)
                        {
                            isSelect = true;
                        }
                    }
                }

                if (isSelect)
                {
                    Neusoft.HISFC.Components.OutpatientFee.Controls.ucInvoiceSelect ucSelect = new Neusoft.HISFC.Components.OutpatientFee.Controls.ucInvoiceSelect();

                    ucSelect.Add(comBalances);

                    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucSelect);

                    Neusoft.HISFC.Models.Fee.Outpatient.Balance selectInvoice = ucSelect.SelectedBalance;
                    if (selectInvoice == null || selectInvoice.Invoice.ID == null || selectInvoice.Invoice.ID == "")
                    {
                        MessageBox.Show("��û��ѡ��Ʊ������������ѡ��!");
                        currentBalance = null;
                        this.tbInvoiceNo.SelectAll();
                        this.tbInvoiceNo.Focus();

                        return;
                    }

                    comBalances = outpatientManager.QueryBalancesByInvoiceSequence(selectInvoice.CombNO);
                    if (comBalances == null)
                    {
                        MessageBox.Show("��÷�Ʊ��Ϣ����!" + outpatientManager.Err);
                        currentBalance = null;
                        this.tbInvoiceNo.SelectAll();
                        this.tbInvoiceNo.Focus();

                        return;
                    }
                }
                string tempInvoiceNO = "";
                foreach (Balance balance in comBalances)
                {
                    tempInvoiceNO += balance.Invoice.ID + "\n";
                    totCost += balance.FT.TotCost;
                    ownCost += balance.FT.OwnCost;
                    payCost += balance.FT.PayCost;
                    pubCost += balance.FT.PubCost;
                }

                MessageBox.Show("�÷�Ʊ��Ӧ" + comBalances.Count + "��!�ֱ�Ϊ: \n" + tempInvoiceNO + "\n������Ϸ�Ʊ���ջ�!");
            }
            else
            {
                string tempInvoiceNO = "";
                foreach (Balance balance in comBalances)
                {
                    tempInvoiceNO += balance.Invoice.ID + "\n";
                    totCost += balance.FT.TotCost;
                    ownCost += balance.FT.OwnCost;
                    payCost += balance.FT.PayCost;
                    pubCost += balance.FT.PubCost;
                }
            }

            currentBalance = (comBalances[0] as Balance).Clone();
            if (currentBalance.CancelType != Neusoft.HISFC.Models.Base.CancelTypes.Valid)
            {
                MessageBox.Show("������ķ�Ʊ�����Ѿ����ϣ����֤������");
                currentBalance = null;
                this.tbInvoiceNo.SelectAll();
                this.tbInvoiceNo.Focus();

                return;
            }

            this.tbInvoiceNo.Text = currentBalance.Invoice.ID;
            this.tbPName.Text = currentBalance.Patient.Name;

            Neusoft.HISFC.Models.Base.Employee employee = this.managerIntegrate.GetEmployeeInfo(currentBalance.BalanceOper.ID);
            if (employee == null) 
            {
                MessageBox.Show("��õ�ǰ��Ʊ����Ա��Ϣʧ��!" + this.managerIntegrate.Err);
            }

            this.tbOperName.Text = employee.Name;
            this.tbPactInfo.Text = currentBalance.Patient.Pact.Name;


            this.tbTotCost.Text = totCost.ToString();
            this.tbOwnCost.Text = ownCost.ToString();
            this.tbPayCost.Text = payCost.ToString();
            this.tbPubCost.Text = pubCost.ToString();

            this.tbInvoiceDate.Text = currentBalance.BalanceOper.OperTime.ToString();

            if (!FillBalanceLists(currentBalance.CombNO))
            {
                MessageBox.Show("��÷�Ʊ��ϸ��Ϣ����!" + outpatientManager.Err);
                
                return;
            }

            comFeeItemLists = outpatientManager.QueryFeeItemListsByInvoiceSequence(currentBalance.CombNO);
            if (comFeeItemLists == null)
            {
                MessageBox.Show("��û��߷�����ϸ����!");
                
                return;
            }

            this.btOk.Focus();
        }

        /// <summary>
        /// �ش�
        /// </summary>
        /// <returns>�ɹ� true ʧ�� false</returns>
        protected virtual bool Print()
        {
            if (currentBalance == null)
            {
                MessageBox.Show("�÷�Ʊ�Ѿ�����!");
                this.tbInvoiceNo.Focus();
                this.tbInvoiceNo.SelectAll();

                return false;
            }
            if (currentBalance.Invoice.ID == "")
            {
                MessageBox.Show("�����뷢Ʊ��Ϣ!");
                this.tbInvoiceNo.Focus();
                this.tbInvoiceNo.SelectAll();
                
                return false;
            }

            bool isCanQuitOtherOper = this.controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.CAN_REPRINT_OTHER_OPER_INVOICE, true, false);

            if(!isCanQuitOtherOper)//���������ش�
            {
                Balance tmpInvoice = comBalances[0] as Balance;

                if (tmpInvoice == null)
                {
                    MessageBox.Show("��Ʊ��ʽת������!");
                    tbInvoiceNo.SelectAll();
                    tbInvoiceNo.Focus();

                    return false;
                }

                if (tmpInvoice.BalanceOper.ID != this.outpatientManager.Operator.ID)
                {
                    MessageBox.Show("�÷�ƱΪ����Ա" + tmpInvoice.BalanceOper.ID + "��ȡ,��û��Ȩ�޽��ش�!");
                    tbInvoiceNo.SelectAll();
                    tbInvoiceNo.Focus();

                    return false;
                }
            }

            bool isCanReprintDayBalance = this.controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.CAN_REPRINT_DAYBALANCED_INVOICE, true, false);

            if (!isCanReprintDayBalance)//���������ش�
            {
                Balance tmpInvoice = comBalances[0] as Balance;

                if (tmpInvoice == null)
                {
                    MessageBox.Show("��Ʊ��ʽת������!");
                    tbInvoiceNo.SelectAll();
                    tbInvoiceNo.Focus();

                    return false;
                }

                if (tmpInvoice.IsDayBalanced)
                {
                    MessageBox.Show("�÷�Ʊ�Ѿ��ս�,��û��Ȩ�޽��ش�!");
                    tbInvoiceNo.SelectAll();
                    tbInvoiceNo.Focus();

                    return false;
                }
            }

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            outpatientManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            feeIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            pharmacyIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            controlParamIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
          
            int returnValue = 0;
            string currentInvoiceNO = "";
            string currentRealInvoiceNO = "";
            string errText = "";
            DateTime nowTime = new DateTime();
            
            nowTime = outpatientManager.GetDateTimeFromSysDateTime();
            invoiceType = controlParamIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.Const.GET_INVOICE_NO_TYPE, false, "0");

            //ArrayList invoicesPrint = new ArrayList();
            //ArrayList invoiceDetailsPrintTemp = new ArrayList();
            //ArrayList invoiceDetailsPrint = new ArrayList();
            //ArrayList feeDetailsPrint = new ArrayList();

            //��Ʊ
            ArrayList invoicesPrint = new ArrayList();
            //��Ʊ��ϸ
            ArrayList invoiceDetailsPrintTemp = new ArrayList();
            //��Ʊ��ϸ
            ArrayList invoiceDetailsPrint = new ArrayList();
            //��Ʊ������ϸ
            ArrayList invoicefeeDetailsPrintTemp = new ArrayList();
            //��Ʊ������ϸ
            ArrayList invoicefeeDetailsPrint = new ArrayList();
            //ȫ��������ϸ
            ArrayList feeDetailsPrint = new ArrayList();


            //��ø���Ʊ��ˮ��
            string invoiceSeqNegative = outpatientManager.GetInvoiceCombNO();
            if (invoiceSeqNegative == null || invoiceSeqNegative == "")
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��÷�Ʊ��ˮ��ʧ��!" + outpatientManager.Err);

                return false;
            }

            //�������Ʊ��ˮ��
            //string invoiceSeqPositive = outpatientManager.GetInvoiceCombNO();
            //if (invoiceSeqPositive == null || invoiceSeqPositive == "")
            //{
            //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
            //    MessageBox.Show("��÷�Ʊ��ˮ��ʧ��!" + outpatientManager.Err);

            //    return false;
            //}

            Hashtable hsInvoice = new Hashtable();

            Neusoft.HISFC.Models.Base.Employee employee = this.outpatientManager.Operator as Neusoft.HISFC.Models.Base.Employee;

            foreach (Balance invoice in comBalances)
            {
                //returnValue = this.feeIntegrate.GetInvoiceNO(employee, ref currentInvoiceNO, ref currentRealInvoiceNO, ref errText, null);

                //if (returnValue == -1)
                //{
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //    MessageBox.Show(errText);

                //    return false;
                //}

               // hsInvoice.Add(invoice.Invoice.ID, currentInvoiceNO);

                returnValue = outpatientManager.UpdateBalanceCancelType(invoice.Invoice.ID, invoice.CombNO, nowTime, Neusoft.HISFC.Models.Base.CancelTypes.Reprint);
                if (returnValue == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("����ԭʼ��Ʊ��Ϣ����!" + outpatientManager.Err);

                    return false;
                }
                if (returnValue == 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("�÷�Ʊ�Ѿ�����!");

                    return false;
                }
                //���������Ϣ(����¼)
                invoice.PrintTime = invoice.BalanceOper.OperTime;
                Balance invoClone = invoice.Clone();
                invoClone.TransType = Neusoft.HISFC.Models.Base.TransTypes.Negative;
                invoClone.FT.TotCost = -invoClone.FT.TotCost;
                invoClone.FT.OwnCost = -invoClone.FT.OwnCost;
                invoClone.FT.PayCost = -invoClone.FT.PayCost;
                invoClone.FT.PubCost = -invoClone.FT.PubCost;
                invoClone.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Reprint;
                invoClone.CanceledInvoiceNO = invoice.Invoice.ID;
                invoClone.CancelOper.ID = outpatientManager.Operator.ID;
                invoClone.BalanceOper.ID = outpatientManager.Operator.ID;
                invoClone.BalanceOper.OperTime = nowTime;
                invoClone.CancelOper.OperTime = nowTime;
                invoClone.IsAuditing = false;
                invoClone.AuditingOper.ID = "";
                invoClone.AuditingOper.OperTime = DateTime.MinValue;
                invoClone.IsDayBalanced = false;
                invoClone.DayBalanceOper.OperTime = DateTime.MinValue;

                invoClone.CombNO = invoiceSeqNegative;

                returnValue = outpatientManager.InsertBalance(invoClone);
                if (returnValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("���뷢Ʊ������Ϣ����!!" + outpatientManager.Err);
                    return false;
                }
                //invoClone.TransType = Neusoft.HISFC.Models.Base.TransTypes.Positive;
                //invoClone.FT.TotCost = -invoClone.FT.TotCost;
                //invoClone.FT.OwnCost = -invoClone.FT.OwnCost;
                //invoClone.FT.PayCost = -invoClone.FT.PayCost;
                //invoClone.FT.PubCost = -invoClone.FT.PubCost;
                //invoClone.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
                //invoClone.CanceledInvoiceNO = invoice.Invoice.ID;
                //invoClone.CancelOper.OperTime = DateTime.MinValue;
                //invoClone.Invoice.ID = currentInvoiceNO;
                //invoClone.PrintedInvoiceNO = currentRealInvoiceNO;
                //invoClone.BalanceOper.ID = outpatientManager.Operator.ID;
                //invoClone.BalanceOper.OperTime = nowTime;
                //invoClone.CombNO = invoiceSeqPositive;

                //invoicesPrint.Add(invoClone);

                //returnValue = outpatientManager.InsertBalance(invoClone);
                if (returnValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("�����·�Ʊ��Ϣ����!" + outpatientManager.Err);

                    return false;
                }
  
                //����Ʊ��ϸ����Ϣ
                ArrayList alInvoceDetail = outpatientManager.QueryBalanceListsByInvoiceNOAndInvoiceSequence(invoice.Invoice.ID, currentBalance.CombNO);
                if (comBalanceLists == null)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("��÷�Ʊ��ϸ����!" + outpatientManager.Err);

                    return false;
                }
                //���Ϸ�Ʊ��ϸ����Ϣ
                returnValue = outpatientManager.UpdateBalanceListCancelType(invoice.Invoice.ID, currentBalance.CombNO, nowTime, Neusoft.HISFC.Models.Base.CancelTypes.Reprint);
                if (returnValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("���Ϸ�Ʊ��ϸ����!" + outpatientManager.Err);

                    return false;
                }
                invoiceDetailsPrintTemp = new ArrayList();
                foreach (BalanceList d in alInvoceDetail)
                {
                    d.BalanceBase.TransType = Neusoft.HISFC.Models.Base.TransTypes.Negative;
                    d.BalanceBase.FT.OwnCost = -d.BalanceBase.FT.OwnCost;
                    d.BalanceBase.FT.PubCost = -d.BalanceBase.FT.PubCost;
                    d.BalanceBase.FT.PayCost = -d.BalanceBase.FT.PayCost;
                    d.BalanceBase.BalanceOper.OperTime = nowTime;
                    d.BalanceBase.BalanceOper.ID = outpatientManager.Operator.ID;
                    d.BalanceBase.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Reprint;
                    d.BalanceBase.IsDayBalanced = false;
                    d.BalanceBase.DayBalanceOper.ID = "";
                    d.BalanceBase.DayBalanceOper.OperTime = DateTime.MinValue;
                    ((Balance)d.BalanceBase).CombNO = invoiceSeqNegative;
                    //{9D9D4A6E-84D2-4c07-B6F0-5F2C8DB1DFD7}
                    d.FeeCodeStat.SortID = d.InvoiceSquence;

                    returnValue = outpatientManager.InsertBalanceList(d);

                    if (returnValue <= 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("���뷢Ʊ��ϸ������Ϣ����!" + outpatientManager.Err);

                        return false;
                    }
                    //d.BalanceBase.Invoice.ID = currentInvoiceNO;
                    //d.BalanceBase.TransType = Neusoft.HISFC.Models.Base.TransTypes.Positive;
                    //d.BalanceBase.FT.OwnCost = -d.BalanceBase.FT.OwnCost;
                    //d.BalanceBase.FT.PubCost = -d.BalanceBase.FT.PubCost;
                    //d.BalanceBase.FT.PayCost = -d.BalanceBase.FT.PayCost;
                    //d.BalanceBase.BalanceOper.OperTime = nowTime;
                    //d.BalanceBase.BalanceOper.ID = outpatientManager.Operator.ID;
                    //d.BalanceBase.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
                    //((Balance)d.BalanceBase).CombNO = invoiceSeqPositive;
                    //d.BalanceBase.FT.TotCost = d.BalanceBase.FT.OwnCost + d.BalanceBase.FT.PayCost + d.BalanceBase.FT.PubCost;
                   
                    //returnValue = outpatientManager.InsertBalanceList(d);
                    //if (returnValue <= 0)
                    //{
                    //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //    MessageBox.Show("�����·�Ʊ��ϸ��Ϣ����!" + outpatientManager.Err);

                    //    return false;
                    //}

                    invoiceDetailsPrintTemp.Add(d);
                }

                invoiceDetailsPrint.Add(invoiceDetailsPrintTemp);

                //if (invoiceType == "2")
                //{
                //    returnValue = this.feeIntegrate.UpdateOnlyRealInvoiceNO(currentInvoiceNO, currentRealInvoiceNO, ref errText);
                //    if (returnValue <= 0)
                //    {
                //        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //        MessageBox.Show("���²���Ա��Ʊʧ��!" + feeIntegrate.Err);

                //        return false;
                //    }
                //}
                //else
                //{
                //    returnValue = this.feeIntegrate.UpdateInvoiceNO(currentInvoiceNO, currentRealInvoiceNO, ref errText);
                //    if (returnValue <= 0)
                //    {
                //        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //        MessageBox.Show("���²���Ա��Ʊʧ��!" + feeIntegrate.Err);

                //        return false;
                //    }
                //}
            }
            //����֧����Ϣ
            comBalancePays = outpatientManager.QueryBalancePaysByInvoiceSequence(currentBalance.CombNO);
            if (comBalancePays == null)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��û���֧����Ϣ����" + outpatientManager.Err);

                return false;
            }
            returnValue = outpatientManager.UpdateCancelTyeByInvoiceSequence("4", currentBalance.CombNO, Neusoft.HISFC.Models.Base.CancelTypes.Reprint);
            if (returnValue < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("����֧����ʽ����!" + outpatientManager.Err);

                return false;
            }
            foreach (BalancePay p in comBalancePays)
            {
                p.TransType = Neusoft.HISFC.Models.Base.TransTypes.Negative;
                p.FT.TotCost = -p.FT.TotCost;
                p.FT.RealCost = -p.FT.RealCost;
                p.InputOper.OperTime = nowTime;
                p.InputOper.ID = outpatientManager.Operator.ID;
                p.InvoiceCombNO = invoiceSeqNegative;
                p.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Reprint;
                p.IsChecked = false;
                p.CheckOper.ID = "";
                p.CheckOper.OperTime = DateTime.MinValue;
                p.BalanceOper.ID = "";
                p.BalanceOper.OperTime = DateTime.MinValue;
                p.IsDayBalanced = false;
                p.IsAuditing = false;
                p.AuditingOper.OperTime = DateTime.MinValue;
                p.AuditingOper.ID = "";

                returnValue = outpatientManager.InsertBalancePay(p);
                if (returnValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("����֧������Ϣ����!" + outpatientManager.Err);

                    return false;
                }
                //p.TransType = Neusoft.HISFC.Models.Base.TransTypes.Positive;
                //p.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
                //p.FT.TotCost = -p.FT.TotCost;
                //p.FT.RealCost = -p.FT.RealCost;
                //p.InvoiceCombNO = invoiceSeqPositive;
                //p.Invoice.ID = currentInvoiceNO;

                //returnValue = outpatientManager.InsertBalancePay(p);

                //if (returnValue <= 0)
                //{
                //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                //    MessageBox.Show("����֧����Ϣ����!" + outpatientManager.Err);

                //    return false;
                //}
            }

            //���������ϸ��Ϣ
            ArrayList feeItemLists = outpatientManager.QueryFeeItemListsByInvoiceSequence(currentBalance.CombNO);
            if (feeItemLists == null)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("��û��߷�����ϸ����!" + outpatientManager.Err);

                return false;
            }
            returnValue = outpatientManager.UpdateFeeItemListCancelType(currentBalance.CombNO, nowTime, Neusoft.HISFC.Models.Base.CancelTypes.Reprint);
            if (returnValue <= 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("���ϻ�����ϸ����!" + outpatientManager.Err);

                return false;
            }
            
            foreach (FeeItemList f in feeItemLists)
            {
                f.TransType = Neusoft.HISFC.Models.Base.TransTypes.Negative;
                f.FT.OwnCost = -f.FT.OwnCost;
                f.FT.PayCost = -f.FT.PayCost;
                f.FT.PubCost = -f.FT.PubCost;
                f.Item.Qty = -f.Item.Qty;
                f.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Reprint;
                f.FeeOper.ID = outpatientManager.Operator.ID;
                f.FeeOper.OperTime = nowTime;
                f.CancelOper.OperTime = nowTime;
                f.InvoiceCombNO = invoiceSeqNegative;

                returnValue = outpatientManager.InsertFeeItemList(f);
                if (returnValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("���������ϸ������Ϣ����!" + outpatientManager.Err);

                    return false;
                }
            }
            
            //foreach (FeeItemList f in feeItemLists)
            //{

            //    f.TransType = Neusoft.HISFC.Models.Base.TransTypes.Positive;
            //    f.FT.OwnCost = -f.FT.OwnCost;
            //    f.FT.PayCost = -f.FT.PayCost;
            //    f.FT.PubCost = -f.FT.PubCost;
            //    f.Item.Qty = -f.Item.Qty;
            //    f.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
            //    f.FeeOper.ID = outpatientManager.Operator.ID;
            //    f.FeeOper.OperTime = nowTime;
            //    f.CancelOper.OperTime = nowTime;
            //    f.Invoice.ID = currentInvoiceNO;
            //    f.InvoiceCombNO = invoiceSeqPositive;

            //    returnValue = outpatientManager.InsertFeeItemList(f);
            //    if (returnValue <= 0)
            //    {
            //        Neusoft.FrameWork.Management.PublicTrans.RollBack();
            //        MessageBox.Show("���������ϸ��Ϣ����!" + outpatientManager.Err);

            //        return false;
            //    }
            //    feeDetailsPrint.Add(f);
            //}
            #region ����ӡ��Ʊ
            //#region ���ɸ�ֵ��ķ�Ʊ������ϸ
            //foreach (Balance b in invoicesPrint)
            //{

            //    #region ��¡һ��������ϸ��Ϣ�б���Ϊ���������Ҫ���б�Ԫ����ɾ��������
            //    ArrayList feeItemListsClone = new ArrayList();
            //    foreach (FeeItemList f in feeItemLists)
            //    {
            //        feeItemListsClone.Add(f.Clone());
            //    }
            //    #endregion

            //    while (feeItemListsClone.Count > 0)
            //    {
            //        invoicefeeDetailsPrintTemp = new ArrayList();
            //        string compareItem = b.Invoice.ID;
            //        foreach (FeeItemList f in feeItemListsClone)
            //        {
            //            if (f.Invoice.ID == compareItem)
            //            {
            //                invoicefeeDetailsPrintTemp.Add(f);
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //        invoicefeeDetailsPrint.Add(invoicefeeDetailsPrintTemp);
            //        foreach (FeeItemList f in invoicefeeDetailsPrintTemp)
            //        {
            //            feeItemListsClone.Remove(f);
            //        }
            //    }
            //}
            //#endregion

            //string invoicePrintDll = this.feeIntegrate.GetControlValue(Neusoft.HISFC.BizProcess.Integrate.Const.INVOICEPRINT, "0");
            //if (invoicePrintDll == null || invoicePrintDll == "")
            //{
            //    MessageBox.Show("û�����÷�Ʊ��ӡ���������ܴ�ӡ!");

            //    return false;
            //}

            //Neusoft.HISFC.Models.Registration.Register rInfo = new Neusoft.HISFC.Models.Registration.Register();
            //Balance invoiceTemp = ((Balance)comBalances[0]);
            //rInfo.PID.CardNO = invoiceTemp.Patient.PID.CardNO;
            //rInfo.Pact = invoiceTemp.Patient.Pact.Clone();
            //rInfo.Name = invoiceTemp.Patient.Name;
            //rInfo.SSN = invoiceTemp.Patient.SSN;

            //#region 
            //ArrayList alPrintInvoicefeeDetails = new ArrayList();

            //alPrintInvoicefeeDetails.Add(invoicefeeDetailsPrint);
            //ArrayList alPrintInvoices = new ArrayList();

            //alPrintInvoices.Add(invoiceDetailsPrint);
            //#endregion

            //foreach (Balance invo in comBalances)
            //{
            //    if (this.pharmacyIntegrate.UpdateDrugRecipeInvoiceN0(invo.Invoice.ID, hsInvoice[invo.Invoice.ID].ToString()) < 0)
            //    {
            //        MessageBox.Show("���ݾɷ�Ʊ�Ÿ����·�Ʊ�ų���");
            //        Neusoft.FrameWork.Management.PublicTrans.RollBack();

            //        return false;
            //    }
            //}

            //returnValue = this.feeIntegrate.PrintInvoice(invoicePrintDll, rInfo, invoicesPrint, alPrintInvoices, feeDetailsPrint,alPrintInvoicefeeDetails, comBalancePays, false, ref errText);
            //if (returnValue == -1)
            //{
            //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
            //    MessageBox.Show(errText);

            //    return false;
            //}
            #endregion
            Neusoft.FrameWork.Management.PublicTrans.Commit();

            currentBalance = null;
            MessageBox.Show("�����ɹ�!");

            Clear();

            return true;
        }
        /// <summary>
        /// ��ʾ��Ʊ������Ϣ
        /// </summary>
        /// <param name="invoiceCombNO"></param>
        /// <returns>�ɹ� true ʧ�� false</returns>
        private bool FillBalanceLists(string invoiceCombNO)
        {
            comBalanceLists = outpatientManager.QueryBalanceListsByInvoiceSequence(invoiceCombNO);

            if (comBalanceLists == null)
            {
                return false;
            }

            BalanceList balanceList = new BalanceList();
            for (int i = 0; i < comBalanceLists.Count; i++)
            {
                balanceList = comBalanceLists[i] as BalanceList;
                if (i > 4)
                {
                    this.fpSpread1_Sheet1.Rows.Add(i, 1);
                }
                this.fpSpread1_Sheet1.Cells[i, 0].Text = balanceList.FeeCodeStat.Name;
                this.fpSpread1_Sheet1.Cells[i, 1].Text = balanceList.BalanceBase.FT.OwnCost.ToString();
                this.fpSpread1_Sheet1.Cells[i, 2].Text = balanceList.BalanceBase.FT.PayCost.ToString();
                this.fpSpread1_Sheet1.Cells[i, 3].Text = balanceList.BalanceBase.FT.PubCost.ToString();
                this.fpSpread1_Sheet1.Cells[i, 4].Text = (balanceList.BalanceBase.FT.OwnCost + balanceList.BalanceBase.FT.PayCost + balanceList.BalanceBase.FT.PubCost).ToString();
            }

            return true;
        }

        #endregion

        #region �¼�

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
            }

            return base.ProcessDialogKey(keyData);
        }

        private void tbInvoiceNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                QueryBalances();
            }
        }

        private void btExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btOk_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show("�Ƿ�Ҫ���Ϸ�Ʊ?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Print();
            }
        }

        private void frmReprint_Load(object sender, System.EventArgs e)
        {
            try
            {
                string setDefaultInvoice = this.controlParamIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.Const.REPRINT_SET_DEFAULT_INVOICE, false, "0");
                //this.neuPayModes
                if (setDefaultInvoice == "1")//��ҪĬ��
                {
                    string invoiceNO = "";
                    string realInvoiceNO = "";
                    string nextInvoiceNO = "";
                    string nextRealInvoiceNO = "";
                    string errText = "";
                    string invoiceType = string.Empty;

                    Neusoft.HISFC.Models.Base.Employee employee = this.managerIntegrate.GetEmployeeInfo(this.outpatientManager.Operator.ID);
                   
                    invoiceType = this.controlParamIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.Const.GET_INVOICE_NO_TYPE, false, "0");

                    if (invoiceType == "2") //Ĭ��ȡ��Ʊ��ҪTrans֧��,��ʱ������.
                    {
                        return;
                    }

                    int iReturn = this.feeIntegrate.GetInvoiceNO(employee, ref invoiceNO, ref realInvoiceNO, ref errText, null);
                    if (iReturn < 0)
                    {
                        MessageBox.Show(errText);
                        this.tbInvoiceNo.Focus();

                        return;
                    }


                    iReturn = this.feeIntegrate.GetNextInvoiceNO(invoiceType, invoiceNO, realInvoiceNO, ref nextInvoiceNO, ref nextRealInvoiceNO, -1, ref errText);

                    this.tbInvoiceNo.Text = nextInvoiceNO;
                    this.tbInvoiceNo.Focus();
                    this.tbInvoiceNo.SelectAll();
                }
            }
            catch { }
        }

        #endregion
    }
}