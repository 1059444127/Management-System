using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Integrate;

namespace Neusoft.UFC.Account.Controls
{
    public partial class ucChargeMark : UserControl
    {
        public ucChargeMark()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// �ʻ�ҵ������
        /// </summary>
        Neusoft.HISFC.Management.Fee.Account accountManager = new Neusoft.HISFC.Management.Fee.Account();

        /// <summary>
        /// �ʻ�ʵ��
        /// </summary>
        Neusoft.HISFC.Object.Account.Account account = null;

        /// <summary>
        /// ���Ʋ���ҵ���
        /// </summary>
        Neusoft.HISFC.Integrate.Common.ControlParam controlParamIntegrate = new Neusoft.HISFC.Integrate.Common.ControlParam();

        /// <summary>
        /// �ʻ���ʵ��
        /// </summary>
        Neusoft.HISFC.Object.Account.AccountCard accountCard = null;
        #endregion

        #region ����

        private bool Valid(string oldMark, string newMark)
        {
            if (oldMark == string.Empty)
            {
                txtOld.Focus();
                MessageBox.Show("�������¿��ţ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (newMark == string.Empty)
            {
                MessageBox.Show("������ԭ���ţ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNew.Focus();
                return false; ;
            }
            if (oldMark == newMark)
            {
                MessageBox.Show("ԭ���ź��¿��Ų�����ͬ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ȷ�ϻ���
        /// </summary>
        protected virtual void confirm()
        {
            if (txtOld.Tag == null || txtOld.Tag.ToString() == string.Empty)
            {
                MessageBox.Show("��������ԭ���ź�س�ȷ�ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string oldMark = this.txtOld.Tag.ToString();
            string newMark = this.txtNew.Text.Trim();
            if (!Valid(oldMark,newMark)) return;

            if (MessageBox.Show("ȷ�Ͻ�ԭ�ſ���" + oldMark  + "�滻Ϊ�´ſ���" + newMark, "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                return;
            }
            try 
	        {

                string resultValue = this.ChangeCard(oldMark, newMark);
                if (resultValue != string.Empty)
                {
                    MessageBox.Show(resultValue, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                MessageBox.Show("�ʻ������ɹ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ucRegPatientInfo1.CardNO = string.Empty;
                this.txtOld.Tag = string.Empty;
	        }
            catch (Exception ex)
	        {
                MessageBox.Show("�ʻ�����ʧ�ܣ�"+ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
	        }    	
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="oldMarkno">�ɿ�</param>
        /// <param name="newMarkno">�¿�</param>
        private string  ChangeCard(string oldMarkno,string newMarkno)
        {

            account = accountManager.GetAccountByMarkNo(oldMarkno);
            string errText = string.Empty;
            if (account == null)
            {
                errText = accountManager.Err;
                return errText;
            }
            //ȡ���ò����жϻ����Ƿ�ͣ���ʻ�
            bool isStopAccount = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.Integrate.AccountConstant.ChangeCardIsStopAccount, true, true);
            if (isStopAccount)
            {
                if (account.IsValid)
                {
                    errText = "��ͣ��ԭ���ʻ���";
                    return errText;
                }
            }

            #region ��������

            Neusoft.NFC.Management.PublicTrans.BeginTransaction();

            //Neusoft.NFC.Management.Transaction trans = new Neusoft.NFC.Management.Transaction(this.accountManager.Connection);
            //trans.BeginTransaction();

            this.accountManager.SetTrans(Neusoft.NFC.Management.PublicTrans.Trans);
            #endregion

            #region ͣ��ԭ��
            //ͣ��ԭ��
            if (accountManager.UpdateAccountCardState(oldMarkno, ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString(),
                        Neusoft.NFC.Function.NConvert.ToInt32(false).ToString()) == -1)
            {
                Neusoft.NFC.Management.PublicTrans.RollBack();
                errText = "ͣ��ԭ��ʧ��";
                return errText;
            }
            #endregion

            #region ����ԭ���Ĳ�����¼
            Neusoft.HISFC.Object.Account.AccountCardRecord accountCardRecord = new Neusoft.HISFC.Object.Account.AccountCardRecord();
            accountCardRecord.MarkNO = oldMarkno;
            accountCardRecord.MarkType.ID = ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString();
            accountCardRecord.CardNO = this.ucRegPatientInfo1.CardNO;
            accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Stop;
            accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Object.Base.Employee).ID;
            if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
            {
                errText = "����ԭ��������¼ʧ��";
                Neusoft.NFC.Management.PublicTrans.RollBack();
                return errText;
            }

            #endregion

            #region �����¿��Ĳ�����¼
            //�����¿��Ĳ�����¼
            accountCardRecord = new Neusoft.HISFC.Object.Account.AccountCardRecord();
            accountCardRecord.MarkNO = newMarkno;
            accountCardRecord.MarkType.ID = ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString();
            accountCardRecord.CardNO = this.ucRegPatientInfo1.CardNO;
            accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Object.Account.MarkOperateTypes.Begin;
            accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Object.Base.Employee).ID;
            //�����Ƿ���ȡ�ɱ���
            if(controlParamIntegrate.GetControlParam<bool>(AccountConstant.IsAcceptChangeCardFee,true,false))
            {
                accountCardRecord.CardMoney = controlParamIntegrate.GetControlParam<decimal>(AccountConstant.AcceptChangeCardFee, true, 0);
            }
            if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
            {
                errText = "�����¿�������¼ʧ�ܣ�";
                Neusoft.NFC.Management.PublicTrans.RollBack();
                return errText;
            }
            #endregion

            #region ����
            
            accountCard = new Neusoft.HISFC.Object.Account.AccountCard();
            accountCard.CardNO = this.ucRegPatientInfo1.CardNO;
            accountCard.MarkNO = newMarkno;
            accountCard.MarkType.ID = ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString();
            accountCard.IsValid = true;

            if (accountCard == null)
            {
                Neusoft.NFC.Management.PublicTrans.RollBack();
                errText = "��ȡ�ſ���¼ʧ�ܣ�";
                return errText;
            }
            if (accountManager.InsertAccountCard(accountCard) == -1)
            {
                MessageBox.Show("���濨ʹ�ü�¼ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Neusoft.NFC.Management.PublicTrans.RollBack();
                errText = "���濨ʹ�ü�¼ʧ�ܣ�";
                return errText;
            }
            Neusoft.NFC.Management.PublicTrans.Commit();
            return string.Empty;
            //if (accountManager.UpdateAccountCardMark(newMarkno, oldMarkno) < 0)
            //{
            //    trans.RollBack();
            //    MessageBox.Show("�ʻ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            #endregion
        }

        /// <summary>
        /// ȷ��ԭ��
        /// </summary>
        protected virtual void confirmOldMark()
        {
            string cardNo = string.Empty;
            string markNo = string.Empty;
            //�жϿ���
            if (accountManager.ValidMarkNO(this.txtOld.Text.Trim(), ref markNo) == -1)
            {
                //���ǿ�����ͨ�����￨�Ų��ҿ����ж�
                cardNo = this.txtOld.Text.Trim().PadLeft(10, '0');
                accountCard = this.accountManager.GetMarkByCardNo(cardNo, ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString());
                if (accountCard == null)
                {
                    MessageBox.Show("�����￨�Ų������ʻ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtOld.Focus();
                    this.txtOld.SelectAll();
                    return;
                }
                this.ucRegPatientInfo1.CardNO = accountCard.CardNO;
                this.txtOld.Tag = accountCard.MarkNO;
                this.txtOld.Text = cardNo;
                this.txtNew.Focus();
            }
            else
            {
                txtOld.Tag = markNo;
                txtOld.Text = markNo;
                //�õ����￨��
                bool bl = accountManager.GetCardNoByMarkNo(markNo, Neusoft.HISFC.Object.Account.MarkTypes.Magcard, ref cardNo);
                if (bl)
                {
                    this.ucRegPatientInfo1.CardNO = cardNo;
                    this.txtNew.Focus();
                }
                else
                {
                    MessageBox.Show(this.accountManager.Err, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtOld.Focus();
                    this.txtOld.SelectAll();
                }
            }
        }

        /// <summary>
        ///ȷ���¿�
        /// </summary>
        protected virtual void confirmNewMark()
        {
            string markNo = string.Empty;
            //�жϿ���
            if (accountManager.ValidMarkNO(this.txtNew.Text.Trim(), ref markNo) == -1)
            {
                MessageBox.Show(accountManager.Err);
                txtNew.Focus();
                txtNew.SelectAll();
                return;
            }
            txtNew.Text = markNo;
            Neusoft.HISFC.Object.Account.AccountCard accountCard = null;
            accountCard = accountManager.GetAccountCard(markNo, ((int)Neusoft.HISFC.Object.Account.MarkTypes.Magcard).ToString());
            if (accountCard != null)
            {
                MessageBox.Show("���ſ������ݿ�����,�����½���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNew.Focus();
                txtNew.SelectAll();
                return;
            }
        }
        #endregion

        #region �¼�
        private void ucChargeMark_Load(object sender, EventArgs e)
        {
            this.FindForm().Text = "���￨����";
            this.ActiveControl = this.txtOld;
        }

        private void txtOld_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.confirmOldMark();
            }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.confirm();
        }

        private void txtNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.confirmNewMark();
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }
        #endregion
    }
}
