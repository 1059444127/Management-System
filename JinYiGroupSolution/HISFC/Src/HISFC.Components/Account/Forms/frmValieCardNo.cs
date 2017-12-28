using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.UFC.Account.Forms
{
    public partial class frmValieCardNo : Neusoft.NFC.Interface.Forms.BaseForm
    {
        public frmValieCardNo(string markno,string cardtype)
        {
            InitializeComponent();
            this.markNO = markno;
            this.cardType = cardtype;
        } 

        #region ����
        /// <summary>
        /// ��ʵ��
        /// </summary>
        private Neusoft.HISFC.Object.Account.AccountCard accountCard = null;
        /// <summary>
        /// �����ʻ�ҵ���
        /// </summary>
        private Neusoft.HISFC.Management.Fee.Account accountManager = new Neusoft.HISFC.Management.Fee.Account();
        /// <summary>
        /// ������ʵ��
        /// </summary>
        private Neusoft.HISFC.Object.Account.AccountCardRecord accountCardRecord = new Neusoft.HISFC.Object.Account.AccountCardRecord();

        private Neusoft.HISFC.Integrate.Fee feeIntegrate = new Neusoft.HISFC.Integrate.Fee();
        /// <summary>
        /// ������
        /// </summary>
        private string markNO = string.Empty;
        /// <summary>
        /// ���￨��
        /// </summary>
        private string cardNo = string.Empty;

        /// <summary>
        /// ������
        /// </summary>
        private string cardType ;
        /// <summary>
        /// �Ƿ��½����µ������
        /// </summary>
        private bool isNewCardNO = false;
        #endregion

        #region ����
        /// <summary>
        /// ������
        /// </summary>
        private string MarkNO
        {
            get
            {
                return markNO;
            }
        }
        /// <summary>
        /// ���￨��
        /// </summary>
        public string CardNO
        {
            get
            {
                return cardNo;
            }
            set
            {
                cardNo = value;
            }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string CardType
        {
            get
            {
                return cardType;
            }
            set
            {
                cardType = value;
            }
        }
        /// <summary>
        /// �Ƿ��½����￨��
        /// </summary>
        public bool IsNewCardNO
        {
            get
            {
                return isNewCardNO;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// ʹ�����в�������
        /// </summary>
        private void GetOldCard()
        {
            if (txtCardNo.Text.Trim() == null)
            {
                MessageBox.Show("�����뻼�߿��ţ�", "��ʾ");
                return ;
            }
            string cardNO = txtCardNo.Text.Trim();
            cardNO = cardNO.PadLeft(10, '0');
            this.txtCardNo.Text = cardNO;
            accountCard = accountManager.GetMarkByCardNo(cardNO,this.CardType);
            if(accountManager.Err!=null)
            {
                MessageBox.Show("���ҿ���Ϣʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            //�ò��������дſ�
            if (accountCard != null)
            {
                DialogResult result = MessageBox.Show("����Ĳ�����" + cardNO + "�����ڴſ�" + accountCard.MarkNO + "����Ҫ�滻�ã�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    if (this.ReplaceAccountCard(accountCard) == -1)
                    {
                        return;
                    }
                    else
                    {
                        this.CardNO = cardNO;
                        this.DialogResult = DialogResult.Yes;
                    }
                }
                else
                {
                    txtCardNo.Focus();
                    txtCardNo.SelectAll();
                }
            }
            //���û�ʹ��
            else
            {
                //if (BulidAccountCard() == -1) return;     
                this.CardNO = cardNO;
                this.DialogResult = DialogResult.Yes;
            }

        }

        private int ReplaceAccountCard(Neusoft.HISFC.Object.Account.AccountCard accountCard)
        {
            Neusoft.NFC.Management.PublicTrans.BeginTransaction();

            //Neusoft.NFC.Management.Transaction trans = new Neusoft.NFC.Management.Transaction(accountManager.Connection);
            //trans.BeginTransaction();

            accountManager.SetTrans(Neusoft.NFC.Management.PublicTrans.Trans);
            try
            {
                //ͣ�øÿ�
                accountCard.IsValid = false;
                if (accountManager.UpdateAccountCardState(accountCard.MarkNO, accountCard.MarkType, Neusoft.NFC.Function.NConvert.ToInt32(accountCard.IsValid).ToString()) == -1)
                {
                    Neusoft.NFC.Management.PublicTrans.RollBack();
                    MessageBox.Show("����ԭ��״̬ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return -1;
                }

                //����ԭ���Ĳ�����¼
                accountCardRecord.MarkNO = accountCard.MarkNO;
                accountCardRecord.MarkType.ID = accountCard.MarkType.ID;
                accountCardRecord.CardNO = accountCard.CardNO;
                accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Stop;
                accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Object.Base.Employee).ID;
                if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
                {
                    MessageBox.Show("���濨������¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Neusoft.NFC.Management.PublicTrans.RollBack();
                    return -1;
                }


                #region ����
                ////�²���һ������
                //accountCard = this.GetAccountCard();
                //if (accountCard == null)
                //{
                //    MessageBox.Show("��ȡ�ſ���¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //if (accountManager.InsertAccountCard(accountCard) == -1)
                //{
                //    MessageBox.Show("���濨ʹ�ü�¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    trans.RollBack();
                //    return -1;
                //}

                ////���뿨������¼
                //accountCardRecord.MarkNO = accountCard.MarkNO;
                //accountCardRecord.MarkType.ID = accountCard.MarkType.ID;
                //accountCardRecord.CardNO = accountCard.CardNO;
                //accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Begin;
                //accountCardRecord.Oper.ID = this.accountManager.Operator.ID;
                //if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
                //{
                //    MessageBox.Show("���濨������¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    trans.RollBack();
                //    return -1;
                //}
                #endregion 

                Neusoft.NFC.Management.PublicTrans.Commit();
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        #region ��������
        ///// <summary>
        ///// ���û�ʹ�ôſ���������
        ///// </summary>
        ///// <param name="accountCard"></param>
        ///// <returns></returns>
        //private int BulidAccountCard()
        //{
        //    Neusoft.NFC.Management.Transaction trans = new Neusoft.NFC.Management.Transaction(accountManager.Connection);
        //    trans.BeginTransaction();
        //    accountManager.SetTrans(trans.Trans);
        //    accountCard=GetAccountCard();
        //    if(accountCard==null)
        //    {
        //         MessageBox.Show("��ȡ�ſ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return -1;
        //    }
        //    if (accountManager.InsertAccountCard(accountCard) == -1)
        //    {
        //        trans.RollBack();
        //        MessageBox.Show("���濨ʹ�ü�¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return -1;
        //    }
        //    accountCardRecord.MarkNO = accountCard.MarkNO;
        //    accountCardRecord.CardNO = accountCard.CardNO;
        //    accountCardRecord.MarkType.ID = this.cardType;
        //    accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Begin;
        //    accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Object.Base.Employee).ID;
        //    if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
        //    {
        //        trans.RollBack();
        //        MessageBox.Show("���濨������¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return -1;
        //    }
        //    trans.Commit();
        //    return 1;
        //}

        ///// <summary>
        ///// �����µĲ�����
        ///// </summary>
        //private void GetNewCard()
        //{
        //    accountCard = GetAccountCard();
        //    Neusoft.NFC.Management.Transaction trans = new Neusoft.NFC.Management.Transaction(accountManager.Connection);
        //    trans.BeginTransaction();
        //    accountManager.SetTrans(trans.Trans);
        //    try
        //    {
        //        if (accountManager.InsertAccountCard(accountCard) == -1)
        //        {
        //            trans.RollBack();
        //            MessageBox.Show("���濨ʹ�ü�¼ʧ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        } 
        //        //���뿨�Ĳ�����¼
        //        accountCardRecord.MarkNO = accountCard.MarkNO;
        //        accountCardRecord.MarkType.ID = accountCard.MarkType.ID;
        //        accountCardRecord.CardNO = accountCard.CardNO;
        //        accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Begin;
        //        accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Object.Base.Employee).ID;
        //        if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
        //        {
        //            MessageBox.Show("���濨������¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            trans.RollBack();
        //            return ;
        //        }
        //        trans.Commit();
        //        this.CardNO = accountCard.CardNO;
        //        this.DialogResult = DialogResult.Yes;
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.RollBack();
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        /// <summary>
        /// �滻�ſ�����
        /// </summary>
        /// <param name="accountCard"></param>
        /// <returns></returns>
        #endregion

        /// <summary>
        /// ȡ��ʵ��
        /// </summary>
        /// <returns></returns>
        private Neusoft.HISFC.Object.Account.AccountCard GetAccountCard()
        {
            try
            {
                accountCard = new Neusoft.HISFC.Object.Account.AccountCard();
                //if (this.rdbnew.Checked)
                //{
                //    //�Զ���ȡ�����
                //    string cardNo = feeIntegrate.GetAutoCardNO();
                //    cardNo = cardNo.PadLeft(10, '0');
                //    accountCard.CardNO = cardNo;
                //}
                //else
                //{
                accountCard.CardNO = txtCardNo.Text.Trim();
                //}
                accountCard.MarkNO = this.MarkNO;
                accountCard.MarkType.ID = this.CardType;
                accountCard.IsValid = true;
                return accountCard;
            }
            catch
            {
                return null;
            }
        }

        #endregion
 
        #region �¼�
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdbold_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbold.Checked)
            {
                this.txtCardNo.Enabled = true;
                this.txtCardNo.Focus();
            }
            else
            {
                this.txtCardNo.Enabled = false;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbnew.Checked)
                {
                    this.isNewCardNO = true;
                    this.DialogResult = DialogResult.Yes;
                    //this.GetNewCard();
                }
                else
                {
                    this.isNewCardNO = false;
                    this.GetOldCard();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ�"+ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.btnOk_Click(this, null);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                btnCancel_Click(null, null);
            }
            base.OnKeyDown(e);
        }

        private void rdbnew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnOk_Click(this, null);
            }
        }
        #endregion

        

  
    }
}