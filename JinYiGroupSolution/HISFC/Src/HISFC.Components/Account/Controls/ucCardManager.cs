using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.Account;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.BizProcess.Interface.Account;

namespace Neusoft.HISFC.Components.Account.Controls
{
    /// <summary>
    /// ���￨����
    /// </summary>
    public partial class ucCardManager : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucCardManager()
        {
            InitializeComponent();
        }

        #region ����
        /// <summary>
        /// Managerҵ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        
        /// <summary>
        /// Acountҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Fee.Account accountManager = new Neusoft.HISFC.BizLogic.Fee.Account();
        
        /// <summary>
        /// ��ʵ��
        /// </summary>
        private Neusoft.HISFC.Models.Account.AccountCard accountCard = null;

        /// <summary>
        /// ������ʵ��
        /// </summary>
        private Neusoft.HISFC.Models.Account.AccountCardRecord accountCardRecord = null;

        /// <summary>
        /// ���Ʋ���ҵ���
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
        
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBar = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        /// <summary>
        /// �����Ͱ�����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper markTypeHelp = new Neusoft.FrameWork.Public.ObjectHelper();
        
        /// <summary>
        /// ������������Ƿ�̬���һ�����Ϣ
        /// </summary>
        private bool isSelectPatientByEnter = true;

        /// <summary>
        /// �����Ƿ�ֻ�ڱ��ش��������������ķ���
        /// {BCE8D830-5FEA-4681-A08A-4BB48D172E20}
        /// </summary>
        private bool isLocalOperation = true;

       

        #endregion

        #region ����

        #region �����������
        [Category("��������"), Description("�����Ƿ�������룡")]
        public bool IsInputName
        {
            get
            {
                return this.ucRegPatientInfo1.IsInputName;
            }
            set
            {
                this.ucRegPatientInfo1.IsInputName = value;
            }
        }

        [Category("��������"), Description("�Ա��Ƿ�������룡")]
        public bool IsInputSex
        {
            get
            {
                return this.ucRegPatientInfo1.IsInputSex;
            }
            set
            {
                this.ucRegPatientInfo1.IsInputSex = value;
            }
        }

        [Category("��������"), Description("��ͬ��λ�Ƿ�������룡")]
        public bool IsInputPact
        {
            get
            {
                return this.ucRegPatientInfo1.IsInputPact;
            }
            set
            {
                this.ucRegPatientInfo1.IsInputPact = value;
            }
        }

        [Category("��������"), Description("ҽ��֤���Ƿ�������룡")]
        public bool IsInputSiNo
        {
            get 
            {
                return this.ucRegPatientInfo1.IsInputSiNo; 
            }
            set 
            {
                this.ucRegPatientInfo1.IsInputSiNo = value;
            }
        }

        [Category("��������"), Description("���������Ƿ�������룡")]
        public bool IsInputBirthDay
        {
            get 
            {
                return this.ucRegPatientInfo1.IsInputBirthDay; 
            }
            set
            {
                this.ucRegPatientInfo1.IsInputBirthDay = value;
            }
        }

        [Category("��������"), Description("֤�������Ƿ�������룡")]
        public bool IsInputIDEType
        {
            get 
            { 
                return this.ucRegPatientInfo1.IsInputIDEType; 
            }
            set
            {
                this.ucRegPatientInfo1.IsInputIDEType = value;
            }
        }

        [Category("��������"), Description("֤�����Ƿ�������룡")]
        public bool IsInputIDENO
        {
            get 
            {
                return this.ucRegPatientInfo1.IsInputIDENO; 
            }
            set
            {
                this.ucRegPatientInfo1.IsInputIDENO = value;
            }
        }

        #endregion

        [Category("�ؼ�����"), Description("�Ƿ��ձ�¼����ת���뽹�� True:�� False:��")]
        public bool IsMustInputTabIndex
        {
            get
            {
                return this.ucRegPatientInfo1.IsMustInputTabIndex;
            }
            set
            {
                this.ucRegPatientInfo1.IsMustInputTabIndex = value;
            }
        }

        [Category("�ؼ�����"),Description("������������Ƿ�̬���һ�����Ϣ True:�� False:��")]
        public bool IsSelectPatientByEnter
        {
            get 
            { 
                return isSelectPatientByEnter;
            }
            set 
            { 
                isSelectPatientByEnter = value; 
            }
        }

        /// <summary>
        /// �����Ƿ�ֻ�ڱ��ش��������������ķ���
        /// {BCE8D830-5FEA-4681-A08A-4BB48D172E20}
        /// </summary>
        [Category("�ؼ�����"), Description("�����Ƿ�ֻ�ڱ��ش��������������ķ��� True:�� False:��")]
        public bool IsLocalOperation
        {
            get
            {
                return isLocalOperation;
            }
            set
            {
                isLocalOperation = value;
            }
        }
        #endregion

        #region ����

        /// <summary>
        /// ��ʾ��ʾ��Ϣ
        /// </summary>
        /// <param name="consList">��ʾ��Ϣ����</param>
        private void DealConstantList(ArrayList consList)
        {
            if (consList == null || consList.Count <= 0)
            {
                return;
            }

            this.spInfo.RowCount = 0;
            this.spInfo.RowCount = (consList.Count / 3) + (consList.Count % 3 == 0 ? 0 : 1);

            int row = 0;
            int col = 0;

            foreach (Neusoft.FrameWork.Models.NeuObject obj in consList)
            {
                if (col >= 5)
                {
                    col = 0;
                    row++;
                }

                this.spInfo.SetValue(row, col, obj.ID);
                this.spInfo.SetValue(row, col + 1, obj.Name);

                col = col + 2;
            }
        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns>true:�ɹ���falseʧ��</returns>
        private bool Valid()
        {
            if (this.txtMarkNo.Text.Trim() == string.Empty)
            {
                MessageBox.Show("�����뿨�ţ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtMarkNo.Focus();
                return false;
            }
            if (this.cmbMarkType.Tag == null || this.cmbMarkType.Tag.ToString() == string.Empty)
            {
                MessageBox.Show("�����뿨�ź�س�ȷ�ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtMarkNo.Focus();
                this.txtMarkNo.SelectAll();
                return false;
            }

            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(txtMarkNo.Text.Trim(), 20))
            {
                MessageBox.Show("���￨�Ź�����������������￨�ţ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtMarkNo.Focus();
                this.txtMarkNo.SelectAll();
                return false;
            }

            AccountCard card = this.accountManager.GetAccountCard(txtMarkNo.Text.Trim(), this.cmbMarkType.Tag.ToString());
            if (card != null)
            {
                MessageBox.Show("�ÿ��ѱ���������ʹ�ã��뻻����", "����");
                this.txtMarkNo.Focus();
                this.txtMarkNo.SelectAll();
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��������ʹ�õľ��￨
        /// </summary>
        /// <returns>1���ϳɹ���0���������ϲ�����-1ʧ��</returns>
        private int StopPatientCard()
        {
            string tempCardNO = this.ucRegPatientInfo1.CardNO;
            //��tempCardNOΪ�յ�ʱ����������������������Ϊ�յ�ʱ���ǲ����¿�
            //����������������ʱ����½���CardNO�γ��µĻ�����Ϣ
            //�ڲ�����ʱ��ֻ���»�����Ϣ
            if (string.IsNullOrEmpty(tempCardNO)) return 0;
            //���һ�������ʹ�õĿ��ļ���
            List<AccountCard> list = accountManager.GetMarkList(tempCardNO, true);
            if (list.Count == 0) return 0;
            DialogResult digRreslut = MessageBox.Show("�Ƿ�ͣ������ʹ�õľ��￨��", "��ʾ", MessageBoxButtons.OKCancel);
            if (digRreslut == DialogResult.Cancel) return 0;   
            ucCancelMark uc = new ucCancelMark(list);
            uc.StopCardEvent+=new ucCancelMark.EventStopCard(uc_StopCardEvent);
            Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(uc);
            if (uc.FindForm().DialogResult == DialogResult.No) return 0;
            if (uc.FindForm().DialogResult == DialogResult.Cancel) return -1;
            return 1;
            
        }

        /// <summary>
        /// ͣ�þ��￨
        /// </summary>
        /// <param name="markList">������</param>
        /// <returns></returns>
        private bool uc_StopCardEvent(List<AccountCard> markList)
        {
            int resultValue = 0;
            AccountCardRecord tempCardRecord = null;
            
            foreach (AccountCard tempAccountCard in markList)
            {
                //�޸Ŀ�״̬
                resultValue = accountManager.UpdateAccountCardState(tempAccountCard.MarkNO, tempAccountCard.MarkType, false);
                if (resultValue < 0)
                {
                    MessageBox.Show("���ϻ��߾��￨ʧ�ܣ�" + accountManager.Err, "��ʾ");
                    return false;
                }

                #region �γɿ�������¼
                tempCardRecord = new AccountCardRecord();
                tempCardRecord.CardNO = tempAccountCard.Patient.PID.CardNO;//���￨��
                tempCardRecord.MarkNO = tempAccountCard.MarkNO;//���￨��
                tempCardRecord.MarkType = tempAccountCard.MarkType; //������
                tempCardRecord.OperateTypes.ID = (int)MarkOperateTypes.Cancel; //��������
                tempCardRecord.Oper.ID = accountManager.Operator.ID; //������
                #endregion
                //�γɲ�����¼
                resultValue = accountManager.InsertAccountCardRecord(tempCardRecord);
                if (resultValue < 0)
                {
                    MessageBox.Show("���뿨������¼ʧ�ܣ�" + accountManager.Err);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ��������
        /// </summary>
        protected virtual void Save()
        {
            if (!this.Valid()) return;

            #region ��������
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            this.accountManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            #endregion

            #region  ���滼����Ϣ
            int resultValue = 0;
            if (this.ucRegPatientInfo1.CardNO == string.Empty)
            {
                this.ucRegPatientInfo1.McardNO = txtMarkNo.Text;
                resultValue = this.ucRegPatientInfo1.Save();
                if (resultValue <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    return;
                }
            }
            #endregion

            #region ��������ʹ�õľ��￨
            if (StopPatientCard() < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return;
            }
            #endregion

            #region ����

            #region ��ȡ��ʵ��
            accountCard = new Neusoft.HISFC.Models.Account.AccountCard();
            //accountCard.Patient.PID.CardNO = this.ucRegPatientInfo1.CardNO;
            accountCard.Patient = this.ucRegPatientInfo1.GetPatientInfomation();
            accountCard.MarkNO = this.txtMarkNo.Text.Trim();
            accountCard.MarkType = this.cmbMarkType.SelectedItem as FrameWork.Models.NeuObject;
            accountCard.IsValid = true;
            #endregion
            //����������
            string error = string.Empty;
            resultValue = this.BulidCard(accountCard);
            if (resultValue == -1)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(error, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            Neusoft.FrameWork.Management.PublicTrans.Commit();
            //��ӡ��ǩ
            PrintLable();
            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�������ݳɹ���"), Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.ClearData();
        }

        /// <summary>
        /// ��ӡ��ǩ
        /// </summary>
        private void PrintLable()
        {
            IPrintLable iPrintLable = Neusoft.FrameWork.WinForms.Classes.
              UtilInterface.CreateObject(this.GetType(), typeof(IPrintLable)) as IPrintLable;
            if (iPrintLable != null)
            {
                iPrintLable.PrintLable(accountCard);
            }
        }

        
        /// <summary>
        /// �������
        /// </summary>
        private void ClearData()
        {
            this.ucRegPatientInfo1.Clear();
            this.txtMarkNo.Text = string.Empty;
            this.cmbMarkType.Text = string.Empty;
            this.cmbMarkType.Tag = string.Empty;
            this.ucRegPatientInfo1.Focus();
            accountCard = null;
            this.ckIsTreatment.Checked = false;
        }

        /// <summary>
        /// �����µĲ�����
        /// </summary>
        private int BulidCard(AccountCard tempAccountCard)
        {
            try
            {
                if (accountManager.InsertAccountCard(tempAccountCard) == -1)
                {
                    MessageBox.Show("���濨��¼ʧ�ܣ�" + accountManager.Err, "����");
                    return -1;
                }
                accountCardRecord = new Neusoft.HISFC.Models.Account.AccountCardRecord();
                //���뿨�Ĳ�����¼
                accountCardRecord.MarkNO = tempAccountCard.MarkNO;
                accountCardRecord.MarkType.ID = tempAccountCard.MarkType.ID;
                accountCardRecord.CardNO = tempAccountCard.Patient.PID.CardNO;
                accountCardRecord.OperateTypes.ID = (int)Neusoft.HISFC.Models.Account.MarkOperateTypes.Begin;
                accountCardRecord.Oper.ID = (this.accountManager.Operator as Neusoft.HISFC.Models.Base.Employee).ID;
                //�Ƿ���ȡ���ɱ���
                bool bl = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.AccountConstant.IsAcceptCardFee, true, false);
                if (bl)
                {
                    accountCardRecord.CardMoney = controlParamIntegrate.GetControlParam<decimal>(Neusoft.HISFC.BizProcess.Integrate.AccountConstant.AcceptCardFee, true, 0);
                }
                if (accountManager.InsertAccountCardRecord(accountCardRecord) == -1)
                {
                    MessageBox.Show("���濨������¼ʧ�ܣ�"+ accountManager.Err);
                    return -1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ�ܣ�" + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// ��ѯ������Ϣ
        /// </summary>
        protected virtual int QueryPatientInfo()
        {
            Neusoft.HISFC.Models.RADT.PatientInfo patient = this.ucRegPatientInfo1.GetPatientInfomation();
            if (string.IsNullOrEmpty(patient.Name) && string.IsNullOrEmpty(patient.Sex.ID.ToString()) && string.IsNullOrEmpty(patient.Pact.ID)
              && string.IsNullOrEmpty(patient.PID.CaseNO) && string.IsNullOrEmpty(patient.IDCardType.ID) && string.IsNullOrEmpty(patient.IDCard)
              && string.IsNullOrEmpty(patient.SSN))
            {
                return -1;
            }
                    

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ��һ�����Ϣ�����Ժ�...");
            Application.DoEvents();
            //���һ�����Ϣ
            
            List<AccountCard> list = accountManager.GetAccountCard(patient.Name,
                                                                    patient.Sex.ID.ToString(),
                                                                    patient.Pact.ID,
                                                                    patient.PID.CaseNO,
                                                                    patient.IDCardType.ID,
                                                                    patient.IDCard,
                                                                    patient.SSN);
            if (list == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(accountManager.Err);
                return -1;
            }
            try
            {
                if (this.spPatient.Rows.Count > 0)
                {
                    this.spPatient.Rows.Remove(0, this.spPatient.Rows.Count);
                }
                this.spPatient.Rows.Count = list.Count;
                int count = 0,beginIndex = 0,rangCount = 1;
                count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    AccountCard tempCard = list[i];
                    //����
                    this.spPatient.Cells[i, 0].Text = tempCard.Patient.Name;
                    //�Ա�
                    this.spPatient.Cells[i, 1].Text = tempCard.Patient.Sex.Name;
                    //����
                    this.spPatient.Cells[i, 2].Text = this.accountManager.GetAge(tempCard.Patient.Birthday);
                    //����
                    this.spPatient.Cells[i, 3].Text = this.ucRegPatientInfo1.GetName(tempCard.Patient.Nationality.ID, 0);
                    //��ͬ��λ
                    this.spPatient.Cells[i, 4].Text = tempCard.Patient.Pact.Name;
                    //֤������
                    this.spPatient.Cells[i, 5].Text = this.ucRegPatientInfo1.GetName(tempCard.Patient.IDCardType.ID, 1);
                    //֤����
                    this.spPatient.Cells[i, 6].Text = tempCard.Patient.IDCard;
                    this.spPatient.Cells[i, 7].Text = tempCard.Patient.CompanyName;
                    this.spPatient.Cells[i, 8].Text = tempCard.Patient.AddressHome;
                    this.spPatient.Cells[i, 9].Text = tempCard.MarkNO;
                    this.spPatient.Cells[i, 10].Text = markTypeHelp.GetName(tempCard.MarkType.ID);
                    this.spPatient.Rows[i].Tag = tempCard;
                    //����ϲ���Ԫ��
                    if (i < count - 1)
                    {
                        if (tempCard.Patient.PID.CardNO == list[i + 1].Patient.PID.CardNO)
                        {
                            rangCount += 1;
                            if (i == count - 2)
                            {
                                if (rangCount > 1)
                                {
                                    RangFpCell(beginIndex, rangCount);
                                }
                            }
                        }
                        else
                        {
                            if (rangCount > 1)
                            {
                                RangFpCell(beginIndex, rangCount);
                            }
                            beginIndex = i+1;
                            rangCount = 1;
                        }
                    }
                    
                }
                this.neuSpread1.ActiveSheet = spPatient;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return -1;
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            return 1;
        }

        /// <summary>
        /// �ϲ���Ԫ��
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="count"></param>
        private void RangFpCell(int begin, int count)
        {
            for (int col = 0; col < this.spPatient.Columns.Count - 2; col++)
            {
                this.spPatient.Models.Span.Add(begin, col, count, 1);
            }
        }

        /// <summary>
        /// ��ת����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucRegPatientInfo1_OnFoucsOver(object sender, EventArgs e)
        {
            this.txtMarkNo.Focus();
            this.neuSpread1.ActiveSheet = this.spPatient;
        }

        /// <summary>
        /// ���һ�����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucRegPatientInfo1_OnEnterSelectPatient(object sender, EventArgs e)
        {
            if (this.IsSelectPatientByEnter)
            {
                this.QueryPatientInfo();
            }
        }

        /// <summary>
        /// ucRegPatientInfo�ؼ�cmb�õ�����ʱ�������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucRegPatientInfo1_CmbFoucs(object sender, EventArgs e)
        {
            if (sender is Neusoft.FrameWork.WinForms.Controls.NeuComboBox)
            {
                FrameWork.WinForms.Controls.NeuComboBox cmb = sender as FrameWork.WinForms.Controls.NeuComboBox;
                ArrayList al = cmb.alItems;
                DealConstantList(al);
                this.neuSpread1.ActiveSheet = this.spInfo;
            }
            else
            {
                this.neuSpread1.ActiveSheet = this.spPatient;
            }
        }
        #endregion

        #region �¼�
        private void ucCardManager_Load(object sender, EventArgs e)
        {
            ArrayList al = managerIntegrate.GetConstantList("MarkType");
            if (al == null)
            {
                MessageBox.Show("���Ҿ��￨����ʧ��");
                return;
            }
            this.cmbMarkType.AddItems(al);
            markTypeHelp.ArrayObject = al;
            this.ucRegPatientInfo1.CmbFoucs += new HandledEventHandler(ucRegPatientInfo1_CmbFoucs);
            this.ucRegPatientInfo1.OnFoucsOver+=new HandledEventHandler(ucRegPatientInfo1_OnFoucsOver);
            this.ucRegPatientInfo1.OnEnterSelectPatient +=new HandledEventHandler(ucRegPatientInfo1_OnEnterSelectPatient);
            this.ucRegPatientInfo1.IsLocalOperation = this.isLocalOperation;
            this.ucRegPatientInfo1.IsEnableIDEType = false;
            this.ucRegPatientInfo1.IsEnableIDENO = false;
            
        }

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            toolBar.AddToolButton("������Ϣ��ѯ", "������Ϣ��ѯ", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.C��ѯ, true, false, null);
            toolBar.AddToolButton("����", "����", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Q���, true, false, null);
            return toolBar;
        }
        
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "������Ϣ��ѯ":
                    {
                        QueryPatientInfo();
                        break;
                    }
                case "����":
                    {
                        this.ClearData();
                        break;
                    }
            }
            base.ToolStrip_ItemClicked(sender, e);
        }
        
        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return base.OnSave(sender, neuObject);
        }
        
        private void txtMarkNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                accountCard = new AccountCard();
                int resultValue = accountManager.GetCardByRule(this.txtMarkNo.Text.Trim(), ref accountCard);
                if (resultValue < 0)
                {
                    MessageBox.Show(accountManager.Err);
                    this.txtMarkNo.Focus();
                    this.txtMarkNo.SelectAll();
                    this.cmbMarkType.Tag = string.Empty;
                    return;
                }

                if (resultValue == 1)
                {
                    MessageBox.Show("�ÿ��ѱ�ʹ�ã��뻻����");
                    this.txtMarkNo.Focus();
                    this.txtMarkNo.SelectAll();
                    this.cmbMarkType.Tag = string.Empty;
                    return;
                }
                this.txtMarkNo.Text = accountCard.MarkNO;
                this.cmbMarkType.Tag = accountCard.MarkType.ID;
                if (MessageBox.Show("�Ƿ񱣴����ݣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Save();
                }
            }
        }

        private void neuSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (this.neuSpread1.ActiveSheet != spPatient) return;

            if (this.spPatient.ActiveRow.Tag == null)
            {
                this.menuItem2.Enabled = false;
            }
            else
            {
                AccountCard tempAccountCard = this.spPatient.ActiveRow.Tag as AccountCard;
                //1Ϊ���뿨
                if (tempAccountCard != null && tempAccountCard.MarkType.ID == "1")
                {
                    this.menuItem2.Enabled = true;
                }
                else
                {
                    this.menuItem2.Enabled = false;
                }
            }
            this.menu.Show(neuSpread1 as Control, new Point(e.X, e.Y));
        }

        /// <summary>
        /// ��ʾ���߻�����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (this.neuSpread1.ActiveSheet != this.spPatient) return;
            if (this.spPatient.ActiveRow.Tag == null) return;
            AccountCard tempCard = this.spPatient.ActiveRow.Tag as AccountCard;
            if (tempCard.Patient == null)
            {
                MessageBox.Show("��ѯ������Ϣʧ�ܣ�");
                return;
            }
            this.ucRegPatientInfo1.CardNO = tempCard.Patient.PID.CardNO;
            this.txtMarkNo.Focus();
        }

        /// <summary>
        /// ��ӡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (this.neuSpread1.ActiveSheet != this.spPatient) return;
            if (this.spPatient.ActiveRow.Tag == null) return;
            //{63F68506-F49D-4ed5-92BD-28A52AF54626}
            AccountCard tempaccontCard = this.spPatient.ActiveRow.Tag as AccountCard;
            if (tempaccontCard == null) return;
            PictureBox picBox = new PictureBox();
            picBox.Size = new Size(400, 30);
            picBox.Visible = true;
            picBox.BackColor = System.Drawing.Color.White;
            picBox.SizeMode = PictureBoxSizeMode.AutoSize;
            Panel panel = new Panel();
            panel.Controls.Add(picBox);
            panel.Visible = true;
            Class.Code39 code39 = new Neusoft.HISFC.Components.Account.Class.Code39();
            code39.ShowCodeString = true;
            Bitmap bitmap = code39.GenerateBarcode(tempaccontCard.MarkNO);
            picBox.Image = bitmap as Image;
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.PrintPage(0, 0, panel);
        }

        private void ckIsTreatment_CheckedChanged(object sender, EventArgs e)
        {
            bool bl = ckIsTreatment.Checked;
            this.ucRegPatientInfo1.IsTreatment = bl;
            if (bl)
            {
                this.ucRegPatientInfo1.Clear();
                this.txtMarkNo.Focus();
            }
            else
            {
                this.ucRegPatientInfo1.Focus();
            }

        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {

            get
            {
                Type[] vtype = new Type[2];
                vtype[0] = typeof(IPrintLable);
               
                return vtype;
            }
        }

        #endregion
    }
        
}
