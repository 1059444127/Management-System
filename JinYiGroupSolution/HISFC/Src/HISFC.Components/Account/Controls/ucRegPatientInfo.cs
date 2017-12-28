using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;
using Neusoft.HISFC.Models.Base;
using System.Xml;
using System.Collections;
using Neusoft.HISFC.BizLogic.Fee;
using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Components.Account.Controls
{
    public partial class ucRegPatientInfo : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucRegPatientInfo()
        {
            InitializeComponent();
        }

        #region �Զ����¼�

        /// <summary>
        /// ���������ComBoxʱ�������¼�
        /// </summary>
        public event HandledEventHandler CmbFoucs;

        /// <summary>
        /// ����ǰUC���㵽�����һ��ʱ�������¼�
        /// </summary>
        public event HandledEventHandler OnFoucsOver;

        /// <summary>
        /// �����뻼����Ϣ�س�ʱ���һ�����Ϣ
        /// </summary>
        public event HandledEventHandler OnEnterSelectPatient;

        #endregion

        #region ����

        #region ҵ������
        /// <summary>
        /// Managerҵ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        /// <summary>
        /// �������ҵ����
        /// </summary>
        private Neusoft.HISFC.BizLogic.Fee.Outpatient outpatientManager = new Neusoft.HISFC.BizLogic.Fee.Outpatient();

        /// <summary>
        /// Acountҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Fee.Account accountManager = new Neusoft.HISFC.BizLogic.Fee.Account();

        /// <summary>
        /// ���ת
        /// </summary>
        private HISFC.BizProcess.Integrate.RADT radtManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// ��ͬ��λҵ���
        /// </summary>
        private PactUnitInfo pactManager = new PactUnitInfo();

        /// <summary>
        /// ���Ʋ���ҵ���
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();
 
        #endregion

        #region ����

        /// <summary>
        /// ����״̬ʵ��
        /// </summary>
        HISFC.Models.RADT.MaritalStatusEnumService maritalService = new Neusoft.HISFC.Models.RADT.MaritalStatusEnumService();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = null;

        /// <summary>
        /// ���￨��
        /// </summary>
        private string cardNO = string.Empty;

        /// <summary>
        /// ����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper NationHelp = null;

        /// <summary>
        /// ֤������
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper IdCardTypeHelp = null;

        /// <summary>
        /// �Ƿ���
        /// </summary>
        private bool isTreatment = false;

        /// <summary>
        /// MPI�ӿ�
        /// </summary>
        //Neusoft.HISFC.BizProcess.Interface.Platform.IEmpiCommutative iEmpi = null;


        /// <summary>
        /// �����Ƿ�ֻ�ڱ��ش��������������ķ���
        /// {BCE8D830-5FEA-4681-A08A-4BB48D172E20}
        /// </summary>
        private bool isLocalOperation = true;

        private string mcardNO = string.Empty;

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        /// <summary>
        /// ���뿨����� 0��ձ�ʾ�þ��￨����������  ������0Ϊ�࿨
        /// </summary>
        private bool cardWay = false;

        #endregion

        #region ������Ʊ���

        #region ����¼����Ŀ
        /// <summary>
        /// �Ƿ������������
        /// </summary>
        private bool isInputName = true;

        /// <summary>
        /// �Ƿ���������Ա�
        /// </summary>
        private bool isInputSex = false;

        /// <summary>
        /// �Ƿ���������ͬ��λ
        /// </summary>
        private bool isInputPact = false;

        /// <summary>
        /// �Ƿ��������ҽ��֤��
        /// </summary>
        private bool isInputSiNo = false;

        /// <summary>
        /// �Ƿ���������������
        /// </summary>
        private bool isInputBirthDay = false;

        /// <summary>
        /// �Ƿ��������֤������
        /// </summary>
        private bool isInputIDEType = false;

        /// <summary>
        /// �Ƿ��������֤����
        /// </summary>
        private bool isInputIDENO = false;

        #endregion

        #region �Ƿ�����޸�
        /// <summary>
        /// ������Դ�Ƿ�����޸�
        /// </summary>
        private bool isEnablePact = true;

        /// <summary>
        /// ҽ��֤���Ƿ�����޸�
        /// </summary>
        private bool isEnableSiNO = true;

        /// <summary>
        /// �Ƿ�����޸�֤������
        /// </summary>
        private bool isEnableIDEType = true;

        /// <summary>
        /// �Ƿ�����޸�֤����
        /// </summary>
        private bool isEnableIDENO = true;

        /// <summary>
        /// Vip�Ƿ����
        /// </summary>
        private bool isEnableVip = true;

        /// <summary>
        /// ���������Ƿ����
        /// </summary>
        private bool isEnableEntry = true;
        #endregion

        /// <summary>
        /// ����¼��ؼ�
        /// </summary>
        private Hashtable InputHasTable = new Hashtable();

        /// <summary>
        /// �Ƿ�����޸Ŀؼ�
        /// </summary>
        private List<Control> EnableControlList = new List<Control>();

        /// <summary>
        /// �Ƿ��ձ�¼����ת���뽹��
        /// </summary>
        private bool isMustInputTabInde = false;

        /// <summary>
        /// ��������ؼ����TabIndex
        /// </summary>
        int inpubMaxTabIndex = 0;

        ///// <summary>
        ///// ������������·��
        ///// </summary>
        //string filePath = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/CasDeptDefaultValue.xml"; 
        #endregion

        #endregion

        #region ����
        /// <summary>
        /// ���￨��
        /// </summary>
        public string CardNO
        {
            set
            {
                if (this.DesignMode) return;
                cardNO = value;
                if (value != string.Empty)
                {
                    SetInfo(cardNO);
                }
            }
            get
            {
                return cardNO;
            }
        }

        //������￨��
        public string McardNO
        {
            get { return mcardNO; }
            set { mcardNO = value; }
        }

        [Category("�ؼ�����"), Description("�Ƿ��﷢�� True:�� false:��")]
        public bool IsTreatment
        {
            get { return isTreatment; }
            set { isTreatment = value; }
        }

        [Category("�ؼ�����"), Description("�Ƿ��ձ�¼����ת���뽹��")]
        public bool IsMustInputTabIndex
        {
            get
            {
                return isMustInputTabInde;
            }
            set
            {
                isMustInputTabInde = value;
            }
        }

        /// <summary>
        /// �����Ƿ�ֻ�ڱ��ش��������������ķ���
        /// {BCE8D830-5FEA-4681-A08A-4BB48D172E20}
        /// </summary>
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

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        [Category("�ؼ�����"), Description("false:���￨���������� true:���￨���������Ų�ͬ")]
        public bool CardWay
        {
            get
            {
                return cardWay;
            }
            set
            {
                cardWay = value;
            }
        }

        #region �����������
        [Category("�ؼ�����"), Description("�����Ƿ�������룡")]
        public bool IsInputName
        {
            get
            {
                return isInputName;
            }
            set
            {
                isInputName = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblName, this.txtName, value);
            }
        }

        [Category("�ؼ�����"), Description("�Ա��Ƿ�������룡")]
        public bool IsInputSex
        {
            get
            {
                return isInputSex;
            }
            set
            {
                isInputSex = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblSex, this.cmbSex, value);
            }
        }

        [Category("�ؼ�����"), Description("��ͬ��λ�Ƿ�������룡")]
        public bool IsInputPact
        {
            get
            {
                return isInputPact;
            }
            set
            {
                isInputPact = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblPact, this.cmbPact, value);
            }
        }

        [Category("�ؼ�����"), Description("ҽ��֤���Ƿ�������룡")]
        public bool IsInputSiNo
        {
            get { return isInputSiNo; }
            set
            {
                isInputSiNo = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblSiNO, this.txtSiNO, value);
            }
        }

        [Category("�ؼ�����"), Description("���������Ƿ�������룡")]
        public bool IsInputBirthDay
        {
            get { return isInputBirthDay; }
            set
            {
                isInputBirthDay = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblBirthDay, this.dtpBirthDay, value);
            }
        }

        [Category("�ؼ�����"), Description("֤�������Ƿ�������룡")]
        public bool IsInputIDEType
        {
            get { return isInputIDEType; }
            set
            {
                isInputIDEType = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblCardType, this.cmbCardType, value);
            }
        }

        [Category("�ؼ�����"), Description("֤�����Ƿ�������룡")]
        public bool IsInputIDENO
        {
            get { return isInputIDENO; }
            set
            {
                isInputIDENO = value;
                this.AddOrRemoveUnitAtMustInputLists(this.lblIDNO, this.txtIDNO, value);
            }
        }
        #endregion

        #region �Ƿ�����޸Ŀ���
        [Category("�޸Ŀ���"), Description("������Դ�Ƿ�����޸�")]
        public bool IsEnablePact
        {
            get { return isEnablePact; }
            set
            {
                isEnablePact = value;
                AddOrRemoveUnitAtEnableLists(this.cmbPact, value);
            }
        }

        [Category("�޸Ŀ���"), Description("ҽ��֤���Ƿ�����޸�")]
        public bool IsEnableSiNO
        {
            get { return isEnableSiNO; }
            set
            {
                isEnableSiNO = value;
                AddOrRemoveUnitAtEnableLists(this.txtSiNO, value);
            }
        }

        [Category("�޸Ŀ���"), Description("�Ƿ�����޸�֤������")]
        public bool IsEnableIDEType
        {
            get { return isEnableIDEType; }
            set
            {
                isEnableIDEType = value;
                AddOrRemoveUnitAtEnableLists(this.cmbCardType, value);
            }
        }

        [Category("�޸Ŀ���"), Description("�Ƿ�����޸�֤����")]
        public bool IsEnableIDENO
        {
            get { return isEnableIDENO; }
            set
            {
                isEnableIDENO = value;
                AddOrRemoveUnitAtEnableLists(this.txtIDNO, value);
            }
        }

        [Category("�޸Ŀ���"), Description("�Ƿ�����޸�Vip��ʶ")]
        public bool IsEnableVip
        {
            get
            {
                return isEnableVip;
            }
            set
            {
                isEnableVip = value;
                AddOrRemoveUnitAtEnableLists(this.ckVip, value);
            }
        }

        [Category("�޸Ŀ���"), Description("�������������Ƿ�����޸�")]
        public bool IsEnableEntry
        {
            get
            {
                return isEnableEntry;
            }
            set
            {
                isEnableEntry = value;
                AddOrRemoveUnitAtEnableLists(this.ckEncrypt, value);
            }
        }

        public bool IsShowTitle
        {
            set
            {
                lblshow.Visible = value;
            }
            get
            {
                return lblshow.Visible;
            }
        }
        #endregion

        #endregion

        #region ˽�з���
        /// <summary>
        /// ��ʼ�������б�
        /// </summary>
        /// <returns></returns>
        protected virtual int Init()
        {
            try
            {
                //�Ա��б�
                this.cmbSex.AddItems(Neusoft.HISFC.Models.Base.SexEnumService.List());
                this.cmbSex.Text = "��";

                //����
                this.cmbNation.AddItems(managerIntegrate.GetConstantList(EnumConstant.NATION));
                this.cmbNation.Text = "����";
                NationHelp = new Neusoft.FrameWork.Public.ObjectHelper(this.cmbNation.alItems);
                //����״̬
                this.cmbMarry.AddItems(HISFC.Models.RADT.MaritalStatusEnumService.List());

                //����
                this.cmbCountry.AddItems(managerIntegrate.GetConstantList(EnumConstant.COUNTRY));
                this.cmbCountry.Text = "�й�";

                //ְҵ��Ϣ
                this.cmbProfession.AddItems(managerIntegrate.GetConstantList(EnumConstant.PROFESSION));

                //������λ
                this.cmbWorkAddress.AddItems(managerIntegrate.GetConstantList(EnumConstant.WORKNAME));

                //��ϵ����Ϣ
                this.cmbRelation.AddItems(managerIntegrate.GetConstantList(EnumConstant.RELATIVE));

                //��ϵ�˵�ַ��Ϣ
                this.cmbLinkAddress.AddItems(managerIntegrate.GetConstantList(EnumConstant.AREA));

                //��ͥסַ��Ϣ
                this.cmbHomeAddress.AddItems(managerIntegrate.GetConstantList(EnumConstant.AREA));

                //����
                this.cmbDistrict.AddItems(managerIntegrate.GetConstantList(EnumConstant.DIST));

                //����
                this.dtpBirthDay.Value = this.accountManager.GetDateTimeFromSysDateTime();//��������

                //����
                this.cmbArea.AddItems(managerIntegrate.GetConstantList(EnumConstant.AREA));

                //��ͬ��λ{B71C3094-BDC8-4fe8-A6F1-7CEB2AEC55DD}
                //this.cmbPact.AddItems(managerIntegrate.GetConstantList(EnumConstant.PACTUNIT));
                this.cmbPact.AddItems(managerIntegrate.QueryPactUnitAll());
                this.cmbPact.Tag = "1";
                //this.cmbPact.Text = "�ֽ�";
                //֤������
                this.cmbCardType.AddItems(managerIntegrate.QueryConstantList("IDCard"));
                IdCardTypeHelp = new Neusoft.FrameWork.Public.ObjectHelper(this.cmbCardType.alItems);

                Neusoft.FrameWork.Management.ControlParam ctlParam = new Neusoft.FrameWork.Management.ControlParam();

                //ȡ������ 0 ��ʾ�ò�����������
                string returnValue = ctlParam.QueryControlerInfo("800006");

                if (string.IsNullOrEmpty(returnValue))
                {
                    returnValue = "0";
                }

                this.McardNO = returnValue;
                CmbEvent();
                SetInputMenu();
                

                //MPI�ӿ�{BCE8D830-5FEA-4681-A08A-4BB48D172E20}
                //this.iEmpi = Neusoft.HISFC.BizProcess.Integrate.PlatformInstance.GetIEmpiInstance();
            }
            catch (Exception e)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(e.Message);

                return -1;
            }

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            return 1;
        }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        /// <param name="cardno">���￨��</param>
        private void SetInfo(string cardno)
        {
            if (cardno == string.Empty || cardno == null) return;
            this.patientInfo = radtManager.QueryComPatientInfo(cardno);
            //����ƽ̨ Ƕ��������{BCE8D830-5FEA-4681-A08A-4BB48D172E20} 
            if (this.patientInfo == null && isLocalOperation == false)
            {
                //if (iEmpi != null )
                //{
                //    this.patientInfo = iEmpi.GetBasePatientinfo(Neusoft.HISFC.BizProcess.Interface.Platform.HisDomain.Outpatient, cardno);                    
                //}
            }

            if (this.patientInfo != null)
            {
                SetPatient();
            }
            else
            {
                this.Clear();
            }
        }

        /// <summary>
        /// ��ʾ���߻�����Ϣ
        /// </summary>
        /// 
        private void SetPatient()
        {
            //modify by sung 2009-2-24 {DCAA485E-753C-41ed-ABCF-ECE46CD41B33}
            if (this.patientInfo.IsEncrypt)
            {
                patientInfo.Name = Neusoft.FrameWork.WinForms.Classes.Function.Decrypt3DES(patientInfo.NormalName);
            }
            this.txtName.Text = patientInfo.Name;//��������

            //this.txtName.Text = this.patientInfo.Name;               //����
            //if (this.patientInfo.IsEncrypt)
            //{

            //    this.txtName.Tag = this.patientInfo.DecryptName;         //��ʵ����                  
            //}
            //else
            //{
            //    this.txtName.Tag = null;
            //}
            this.cmbSex.Text = this.patientInfo.Sex.Name;            //�Ա�
            this.cmbSex.Tag = this.patientInfo.Sex.ID;               //�Ա�
            this.cmbPact.Text = this.patientInfo.Pact.Name;          //��ͬ��λ����
            this.cmbPact.Tag = this.patientInfo.Pact.ID;             //��ͬ��λID
            this.cmbArea.Tag = this.patientInfo.AreaCode;            //����
            this.cmbCountry.Tag = this.patientInfo.Country.ID;       //����
            this.cmbNation.Tag = this.patientInfo.Nationality.ID;    //����
            this.dtpBirthDay.Value = this.patientInfo.Birthday;      //��������
            //{BE0CBF3B-9CE8-42ca-8448-08CCF11755DF}
            //this.txtAge.Text = this.accountManager.GetAge(this.patientInfo.Birthday);//����
            if (this.patientInfo.Birthday != DateTime.MinValue)
            {
               //string Ages = this.accountManager.GetAge(this.patientInfo.Birthday);
               //ϯ�ڷ�{920FA2E4-CD97-4a2d-B999-BA0CF9494758}
               string age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.patientInfo.Birthday);
               this.txtAge.Text = age.Substring(0, age.Length - 1);
            try
            {
                string unitName = age.Substring(age.Length - 2, 1);
                if (unitName != "��" && unitName != "��" && unitName != "��")
                {
                    unitName = "��";
                }
                this.cmbAgeUnit.Text = unitName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
              // this.cmbAgeUnit.Text = Ages.Substring(Ages.Length - 1, 1);
            }
            
            this.cmbDistrict.Text = this.patientInfo.DIST;            //����
            this.cmbProfession.Tag = this.patientInfo.Profession.ID; //ְҵ
            this.txtIDNO.Text = this.patientInfo.IDCard;             //���֤��
            this.cmbWorkAddress.Text = this.patientInfo.CompanyName; //������λ
            this.txtWorkPhone.Text = this.patientInfo.PhoneBusiness; //��λ�绰
            this.cmbMarry.Tag = this.patientInfo.MaritalStatus.ID.ToString();//����״��
            this.cmbHomeAddress.Text = this.patientInfo.AddressHome;  //��ͥסַ
            this.txtHomePhone.Text = this.patientInfo.PhoneHome;     //��ͥ�绰
            this.txtLinkMan.Text = this.patientInfo.Kin.Name;        //��ϵ�� 
            this.cmbRelation.Tag = this.patientInfo.Kin.Relation.ID; //��ϵ�˹�ϵ
            this.cmbLinkAddress.Text = this.patientInfo.Kin.RelationAddress;//��ϵ�˵�ַ
            this.txtLinkPhone.Text = this.patientInfo.Kin.RelationPhone;//��ϵ�˵绰
            this.ckEncrypt.Checked = this.patientInfo.IsEncrypt; //�Ƿ��������
            this.ckVip.Checked = this.patientInfo.VipFlag;//�Ƿ�vip
            this.txtMatherName.Text = this.patientInfo.MatherName;//ĸ������
            this.cmbCardType.Tag = this.patientInfo.IDCardType.ID; //֤������
            this.txtSiNO.Text = this.patientInfo.SSN;//��ᱣ�պ�
            #region added by zhaoyang 2008-10-13
            txtLinkManDoorNo.Text = this.patientInfo.Kin.RelationDoorNo;//��ϵ�˵�ַ���ƺ�
            txtHomeAddrDoorNo.Text = this.patientInfo.AddressHomeDoorNo;//��ͥ��ַ���ƺ�
            txtEmail.Text = this.patientInfo.Email;//�����ʼ�
            #endregion
        }

        /// <summary>
        /// ��ȡ�����ַ���
        /// </summary>
        /// <param name="patient"></param>
        private void GetPatienName(Neusoft.HISFC.Models.RADT.PatientInfo patient)
        {
            //ѡ�����
            if (ckEncrypt.Checked)
            {
                string name = string.Empty;
                if (this.txtName.Tag == null || this.txtName.Tag.ToString() == string.Empty)
                {
                    name = this.txtName.Text;
                }
                else
                {
                    name = this.txtName.Tag.ToString();
                }
                string encryptStr = Neusoft.FrameWork.WinForms.Classes.Function.Encrypt3DES(name);

                patientInfo.Name = "******";
                patientInfo.NormalName = encryptStr;
                patientInfo.DecryptName = name;
            }
            else
            {
                patientInfo.Name = this.txtName.Text;
            }
        }

        private void CmbEvent()
        {
            foreach (Control c in this.panelControl.Controls)
            {
                c.Enter += new EventHandler(c_Enter);
            }
        }

        /// <summary>
        /// �ؼ���ý���ʱ��Ӧ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void c_Enter(object sender, EventArgs e)
        {
            if (sender == this.txtName || sender == this.txtLinkMan || sender == cmbHomeAddress || sender == cmbLinkAddress || sender == this.txtMatherName || sender == this.cmbWorkAddress)
                InputLanguage.CurrentInputLanguage = CHInput;

            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
            if (CmbFoucs != null)
                this.CmbFoucs(sender, null);

        }

        /// <summary>
        /// ���ݿؼ�����,�ж��Ƿ��ڱ�������ؼ��б��м������ɾ���ÿؼ�
        /// </summary>
        /// <param name="nameControl">���ƿؼ�</param>
        /// <param name="inputControl">����ؼ�</param>
        /// <param name="isMustInput">�Ƿ��������</param>
        private void AddOrRemoveUnitAtMustInputLists(Control nameControl, Control inputControl, bool isMustInput)
        {
            if (isMustInput)
            {
                if (!InputHasTable.ContainsKey(nameControl))
                {
                    InputHasTable.Add(nameControl, inputControl);
                    nameControl.ForeColor = Color.Blue;
                }
            }
            else
            {
                if (InputHasTable.ContainsKey(nameControl))
                {
                    InputHasTable.Remove(nameControl);
                    nameControl.ForeColor = Color.Black;
                }
            }
            inpubMaxTabIndex = 0;
            foreach (DictionaryEntry de in InputHasTable)
            {
                Control c = de.Value as Control;
                //��ȡ����tabIndex
                if (inpubMaxTabIndex < c.TabIndex)
                {
                    inpubMaxTabIndex = c.TabIndex;
                }
            }
        }

        /// <summary>
        /// ���ݿؼ�����,�ж��Ƿ��ڱ�������ؼ��б��м������ɾ���ÿؼ�
        /// </summary>
        /// <param name="enableControl">����ؼ�</param>
        /// <param name="isEnable">�Ƿ���Ա༭</param>
        private void AddOrRemoveUnitAtEnableLists(Control enableControl, bool isEnable)
        {
            if (isEnable)
            {
                if (EnableControlList.Contains(enableControl))
                {
                    EnableControlList.Remove(enableControl);
                    enableControl.Enabled = true;
                }
            }
            if (!isEnable)
            {
                if (!EnableControlList.Contains(enableControl))
                {
                    EnableControlList.Add(enableControl);
                    enableControl.Enabled = false;
                }
            }
        }

        #region ������Ĭ��ֵ����
        ///// <summary>
        ///// ���没��Ĭ�Ͽ���
        ///// </summary>
        ///// <param name="deptCode"></param>
        //private void SaveCasDeptdefautValue(string deptCode)
        //{
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        this.CreateXml();
        //    }
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filePath);
        //    XmlNode xn = doc.SelectSingleNode("//DefaultValue");
        //    xn.InnerText = deptCode;
        //    doc.Save(filePath);
        //}

        ///// <summary>
        ///// ����xml
        ///// </summary>
        //private void CreateXml()
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml("<setting>  </setting>");
        //    XmlNode xn = doc.DocumentElement;
        //    XmlComment xc = doc.CreateComment("���ﻼ����Ϣ¼�벡����Ĭ��ֵ");
        //    XmlElement xe = doc.CreateElement("DefaultValue");
        //    xn.AppendChild(xc);
        //    xn.AppendChild(xe);
        //    doc.Save(filePath);
        //}

        ///// <summary>
        ///// ��ȡ����Ĭ��ֵ
        ///// </summary>
        ///// <returns></returns>
        //private string ReadCaseDept()
        //{
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        this.CreateXml();
        //        return string.Empty;
        //    }
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filePath);
        //    XmlNode xn = doc.SelectSingleNode("//DefaultValue");
        //    return xn.InnerText;
        //}
        #endregion
        #endregion

        #region ����

        /// <summary>
        /// ��ý���
        /// </summary>
        /// <returns></returns>
        public new bool Focus()
        {
            return this.txtName.Focus();
        }

        /// <summary>
        /// �������
        /// </summary>
        public virtual void Clear()
        {
            foreach (Control c in this.panelControl.Controls)
            {
                if (c is Neusoft.FrameWork.WinForms.Controls.NeuComboBox ||
                    c is FrameWork.WinForms.Controls.NeuTextBox)
                {
                    c.Text = string.Empty;
                    c.Tag = string.Empty;
                }
            }
            this.txtAge.ReadOnly = false;
            this.ckEncrypt.Checked = false;
            this.CardNO = string.Empty;
            this.cmbCountry.Text = "�й�";
            this.cmbSex.Text = "��";
            this.cmbSex.Tag = "M";
            this.cmbNation.Text = "����";
            this.cmbPact.Tag = "1";
            this.dtpBirthDay.Value = this.accountManager.GetDateTimeFromSysDateTime();//��������
            
            this.ckVip.Checked = false;
        }

        /// <summary>
        /// ���ݺ���У��
        /// </summary>
        /// <returns></returns>
        public virtual bool InputValid()
        {
            //�жϱ�������Ŀؼ��Ƿ��Ѿ�������Ϣ
            foreach (DictionaryEntry d in this.InputHasTable)
            {
                if (d.Value is Neusoft.FrameWork.WinForms.Controls.NeuComboBox)
                {
                    if (((Control)d.Value).Tag == null || ((Control)d.Value).Tag.ToString() == string.Empty || ((Control)d.Value).Text.Trim() == string.Empty)
                    {
                        MessageBox.Show(((Control)d.Key).Text.Replace(':', ' ') + Language.Msg("����������Ϣ!"));
                        ((Control)d.Value).Focus();

                        return false;
                    }
                }
                else
                {
                    if (((Control)d.Value).Text == string.Empty)
                    {
                        MessageBox.Show(((Control)d.Key).Text.Replace(':', ' ') + Language.Msg("����������Ϣ!"));
                        ((Control)d.Value).Focus();

                        return false;
                    }
                }
            }

            if (!this.ckEncrypt.Checked && this.txtName.Text == "******")
            {
                MessageBox.Show(Language.Msg("�û�������û�м��ܣ���������ȷ�Ļ���������"));
                this.txtName.Focus();
                this.txtName.SelectAll();
                return false;
            }

            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtName.Text, 20))
            {
                MessageBox.Show(Language.Msg("����¼�볬����"));
                this.txtName.Focus();
                return false;
            }

            //�ж��ַ�����ҽ��֤��
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtSiNO.Text, 20))
            {
                MessageBox.Show(Language.Msg("ҽ��֤��¼�볬����"));
                this.txtSiNO.Focus();
                return false;
            }
            //�ж��ַ���������
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.cmbDistrict.Text, 50))
            {
                MessageBox.Show(Language.Msg("����¼�볬����"));
                this.cmbDistrict.Focus();
                return false;
            }
            //�ж��ַ�����֤����
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtIDNO.Text, 20))
            {
                MessageBox.Show(Language.Msg("֤����¼�볬����"));
                this.txtIDNO.Focus();
                return false;
            }

            //�ж����֤��
            if (this.cmbCardType.Tag != null && this.cmbCardType.Tag.ToString() == "01" && this.txtIDNO.Text.Trim() != string.Empty)
            {
                string err = string.Empty;
                if (Neusoft.FrameWork.WinForms.Classes.Function.CheckIDInfo(this.txtIDNO.Text.Trim(), ref err) < 0)
                {
                    MessageBox.Show(err);
                    this.txtIDNO.Focus();
                    return false;
                }
            }

            //�ж��ַ�����������λ
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.cmbWorkAddress.Text, 50))
            {
                MessageBox.Show(Language.Msg("������λ¼�볬����"));
                this.cmbWorkAddress.Focus();
                return false;
            }

            //�жϵ�λ�绰����
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtWorkPhone.Text, 30))
            {
                MessageBox.Show(Language.Msg("��λ�绰����¼�볬��"));
                this.txtWorkPhone.Focus();
                return false;
            }

            //�жϼ�ͥ��ַ����
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.cmbHomeAddress.Text, 50))
            {
                MessageBox.Show(Language.Msg("��ͥ��ַ¼�볬��"));
                this.cmbHomeAddress.Focus();
                return false;
            }

            //�жϼ�ͥ�绰���볤��
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtHomePhone.Text, 30))
            {
                MessageBox.Show(Language.Msg("��ͥ�绰����¼�볬��"));
                this.txtHomePhone.Focus();
                return false;
            }

            //�ж���ϵ�绰���볤��
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtLinkPhone.Text, 30))
            {
                MessageBox.Show(Language.Msg("��ϵ�˵绰����¼�볬��"));
                this.txtLinkPhone.Focus();
                return false;
            }
            //�ж���ϵ��ϵ�˳���
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtLinkMan.Text, 12))
            {
                MessageBox.Show(Language.Msg("��ϵ��¼�볬��"));
                this.txtLinkMan.Focus();
                return false;
            }
            //��ϵ�˵�ַ
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.cmbLinkAddress.Text, 12))
            {
                MessageBox.Show(Language.Msg("��ϵ�˵�ַ¼�볬��"));
                this.cmbLinkAddress.Focus();
                return false;
            }

            //ĸ������
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtMatherName.Text, 12))
            {
                MessageBox.Show(Language.Msg("ĸ������¼�볬��"));
                this.txtMatherName.Focus();
                return false;
            }

            if (this.dtpBirthDay.Value.Date > this.accountManager.GetDateTimeFromSysDateTime().Date)
            {
                MessageBox.Show(Language.Msg("�������ڴ��ڵ�ǰ����,����������!"));
                this.dtpBirthDay.Focus();
                return false;
            }

            #region added by zhaoyang 2008-10-13
            if (string.IsNullOrEmpty(txtEmail.Text) == false)
            {
                //if (NFC.Public.String.isMail(txtEmail.Text.Trim()) == false)
                //{
                //    txtEmail.Focus();
                //    txtEmail.SelectAll();
                //    MessageBox.Show("�������������ʽ��������Ļ��������롣");
                //    return false;
                //}
                if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtEmail.Text, 50))
                {
                    MessageBox.Show(Language.Msg("��������¼�볬��!"));
                    txtEmail.Focus();
                    txtEmail.SelectAll();
                    return false;
                }
            }
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtHomeAddrDoorNo.Text, 40))
            {
                MessageBox.Show(Language.Msg("���ƺ�¼�볬����"));
                txtHomeAddrDoorNo.SelectAll();
                txtHomeAddrDoorNo.Focus();
                return false;
            }

            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtLinkManDoorNo.Text, 40))
            {
                MessageBox.Show(Language.Msg("���ƺ�¼�볬����"));
                txtLinkManDoorNo.SelectAll();
                txtLinkManDoorNo.Focus();
                return false;
            }
            #endregion
            return true;
        }

        /// <summary>
        /// ͨ����ͬ��λ����,��ý������ʵ��
        /// </summary>
        /// <param name="pactID">��ͬ��λ����</param>
        /// <returns>�ɹ�: �������ʵ�� ʧ��: null</returns>
        private PayKind GetPayKindFromPactID(string pactID)
        {
            Neusoft.HISFC.Models.Base.PactInfo pact = this.pactManager.GetPactUnitInfoByPactCode(pactID);
            if (pact == null)
            {
                MessageBox.Show(Language.Msg("��ú�ͬ��λ��ϸ����!"));

                return null;
            }

            return pact.PayKind;
        }

        /// <summary>
        /// ��û���ʵ��
        /// </summary>
        /// <returns></returns>
        public Neusoft.HISFC.Models.RADT.PatientInfo GetPatientInfomation()
        {
            //ˢ�»��߻�����Ϣ
            patientInfo = managerIntegrate.QueryComPatientInfo(cardNO);
            //����ƽ̨ Ƕ��������{BCE8D830-5FEA-4681-A08A-4BB48D172E20}
            if (this.patientInfo == null && isLocalOperation == false)
            {
                //if (iEmpi != null)
                //{
                //    this.patientInfo = iEmpi.GetBasePatientinfo(Neusoft.HISFC.BizProcess.Interface.Platform.HisDomain.Outpatient, cardNO);
                //}
            }
            if (patientInfo == null)
            {
                patientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
            }

            patientInfo.PID.CardNO = cardNO;//���￨��
            if (this.cmbPact.Tag != null && this.cmbPact.Tag.ToString() != string.Empty)
                patientInfo.Pact.PayKind = GetPayKindFromPactID(this.cmbPact.Tag.ToString());//�������
            patientInfo.Pact.ID = this.cmbPact.Tag.ToString();//��ͬ��λ  
            patientInfo.Pact.Name = this.cmbPact.Text.ToString();//��ͬ��λ����
            if (!this.isTreatment)
            {
                this.GetPatienName(patientInfo); //��������
                patientInfo.IsEncrypt = this.ckEncrypt.Checked; //�Ƿ����
            }
            else
            {
                this.patientInfo.Name = "������";
                patientInfo.IsEncrypt = false;
            }
            patientInfo.Sex.ID = this.cmbSex.Tag.ToString();//�Ա�
            patientInfo.AreaCode = this.cmbArea.Tag.ToString();//����
            patientInfo.Country.ID = this.cmbCountry.Tag.ToString();//����
            patientInfo.Nationality.ID = this.cmbNation.Tag.ToString();//����
            patientInfo.Birthday = this.dtpBirthDay.Value;//��������
            patientInfo.Age = outpatientManager.GetAge(this.dtpBirthDay.Value);//����
            patientInfo.DIST = this.cmbDistrict.Text.ToString();//����
            patientInfo.Profession.ID = this.cmbProfession.Tag.ToString();//ְҵ
            patientInfo.IDCard = this.txtIDNO.Text;//֤����
            patientInfo.IDCardType.ID = this.cmbCardType.Tag.ToString();//֤������
            patientInfo.CompanyName = this.cmbWorkAddress.Text.Trim();//������λ
            patientInfo.PhoneBusiness = this.txtWorkPhone.Text.Trim();//��λ�绰
            patientInfo.MaritalStatus.ID = this.cmbMarry.Tag.ToString();//����״�� 
            patientInfo.AddressHome = this.cmbHomeAddress.Text.ToString();//��ͥסַ
            patientInfo.PhoneHome = this.txtHomePhone.Text.Trim();//��ͥ�绰
            patientInfo.Kin.Name = this.txtLinkMan.Text.Trim();//��ϵ�� 
            patientInfo.Kin.Relation.ID = this.cmbRelation.Tag.ToString();//����ϵ�˹�ϵ
            patientInfo.Kin.RelationAddress = this.cmbLinkAddress.Text;//��ϵ�˵�ַ
            patientInfo.Kin.RelationPhone = this.txtLinkPhone.Text.Trim();  //��ϵ�˵绰
            patientInfo.VipFlag = this.ckVip.Checked; //�Ƿ�vip
            patientInfo.MatherName = this.txtMatherName.Text;//ĸ������
            patientInfo.IsTreatment = this.IsTreatment;//�Ƿ���
            patientInfo.SSN = this.txtSiNO.Text;//��ᱣ�պ�
            #region added by zhaoyang 2008-10-13
            patientInfo.Kin.RelationDoorNo = this.txtLinkManDoorNo.Text.Trim();//��ϵ�˵�ַ���ƺ�
            patientInfo.AddressHomeDoorNo = txtHomeAddrDoorNo.Text.Trim();//��ͥ��ַ���ƺ�
            patientInfo.Email = txtEmail.Text.Trim();//��������
            #endregion
            return patientInfo;
        }

        /// <summary>
        /// ���滼������
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            //��ͨ���߾��￨����
            if (!IsTreatment)
            {
                if (!InputValid()) return -1;

            }
            this.patientInfo = this.GetPatientInfomation();

            if (patientInfo.Pact.PayKind.ID == "02")
            {
                //��������ҽ��������ʱ�������Ա�ҽ��֤�Ų���Ϊ��
                if (this.txtName.Text == string.Empty || this.txtSiNO.Text == string.Empty
                    || this.cmbSex.Tag == null || this.cmbSex.Tag.ToString() == string.Empty)
                {
                    MessageBox.Show("�û�����ҽ�����ߣ��������Ա�ҽ��֤�Ų���Ϊ�գ�", "��ʾ");
                    return -1;
                }
            }

            if (string.IsNullOrEmpty(patientInfo.PID.CardNO))
            {
                //����ƽ̨ ����������{BCE8D830-5FEA-4681-A08A-4BB48D172E20}
                //if (this.iEmpi != null && isLocalOperation == false)
                //{
                //    if (iEmpi.GetDomainID(Neusoft.HISFC.BizProcess.Interface.Platform.HisDomain.Outpatient, patientInfo, false, ref cardNO) == -1)
                //    {
                //        MessageBox.Show("���������Ļ�ȡ�²������ŷ�������" + iEmpi.Message);
                //        return -1;
                //    }
                //    if (string.IsNullOrEmpty(cardNO))
                //    {
                //        cardNO = outpatientManager.GetAutoCardNO();
                //        cardNO = cardNO.PadLeft(HISFC.BizProcess.Integrate.Common.ControlParam.GetCardNOLen(), '0');
                //    }
                //}
                //else
                //{
                //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                if (!this.cardWay)
                {
                    cardNO = this.McardNO;
                }
                else
                {
                    cardNO = outpatientManager.GetAutoCardNO();
                }

                //cardNO = cardNO.PadLeft(HISFC.BizProcess.Integrate.Common.ControlParam.GetCardNOLen(), '0');
                cardNO = cardNO.PadLeft(10, '0');
                //}
            }
            else
            {
                cardNO = patientInfo.PID.CardNO;
            }
            patientInfo.PID.CardNO = cardNO;

            if (radtManager.RegisterComPatient(patientInfo) < 0)
            {
                MessageBox.Show(radtManager.Err);
                return -1;
            }
            //����ƽ̨ ����������{BCE8D830-5FEA-4681-A08A-4BB48D172E20}
            //if (this.iEmpi != null && isLocalOperation == false)
            //{
            //    if (iEmpi.CreateOrUpdatePatient(Neusoft.HISFC.BizProcess.Interface.Platform.HisDomain.Outpatient, patientInfo) == -1)
            //    {
            //        MessageBox.Show("��������»�����������Ϣ����" + iEmpi.Message);
            //        return -1;
            //    }
            //}
            return 1;
        }

        /// <summary>
        /// ����ID�������
        /// </summary>
        /// <param name="ID">����ID</param>
        /// <param name="aMod">0:���� 1:֤������</param>
        /// <returns></returns>
        public string GetName(string ID, int aMod)
        {
            if (aMod == 0)
            {
                return NationHelp.GetName(ID);
            }
            else
            {
                return IdCardTypeHelp.GetName(ID);
            }
        }

        /// <summary>
        /// ���ݱ�������ؼ���ת���뽹��
        /// </summary>
        private void SetMustInputFocus(Control currentControl)
        {
            if (currentControl == null)
            {
                SendKeys.Send("{Tab}");
                return;
            }
            //������Ӧ���ȵ������������Ŀؼ�
            Control tempControl = this.NextFocusControl(currentControl);
            if (tempControl != null && tempControl.CanFocus)
            {
                tempControl.Focus();
            }
            else
            {
                //�������һ�������ʱ�򴥷����¼�
                if (this.OnFoucsOver != null)
                {
                    this.OnFoucsOver(null, null);
                }
                else
                {
                    SendKeys.Send("{Tab}");
                }
            }
        }

        /// <summary>
        /// ���ݵ�ǰ��TabIndex������һ��Ӧ�õõ�����Ŀؼ�
        /// </summary>
        /// <param name="CurrentTabIndex"></param>
        /// <returns></returns>
        private Control NextFocusControl(Control currentContol)
        {
            Control tempControl = null;
            foreach (DictionaryEntry de in InputHasTable)
            {
                Control c = de.Value as Control;
                if (currentContol.TabIndex < c.TabIndex)
                {
                    if (tempControl == null)
                    {
                        tempControl = c;
                        continue;
                    }
                    if (tempControl != null && tempControl.TabIndex > c.TabIndex)
                    {
                        tempControl = c;
                    }
                }
            }
            return tempControl;
        }

        /// <summary>
        /// ��ȡ��ǰ�н���ؼ�
        /// </summary>
        /// <returns></returns>
        private Control GetCurrentFoucsControl()
        {
            foreach (Control c in panelControl.Controls)
            {
                if (c.Focused)
                {
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// ���ÿؼ�enable����
        /// </summary>
        /// <param name="isEnabled"></param>
        public void SetControlEnable(bool isEnabled)
        {
            foreach (Control c in this.panelControl.Controls)
            {
                c.Enabled = isEnabled;
            }
        }

        /// <summary>
        /// ��ʾ��
        /// </summary>
        /// <param name="title"></param>
        public void SetTitle(string title)
        {
            this.lblshow.Text = title;
        }
        #endregion

        #region ���뷨

        /// <summary>
        /// Ĭ�ϵ��������뷨
        /// </summary>
        private InputLanguage CHInput = null;

        /// <summary>
        /// ��ʼ�����뷨�˵�
        /// </summary>
        private void SetInputMenu()
        {

            for (int i = 0; i < InputLanguage.InstalledInputLanguages.Count; i++)
            {
                InputLanguage t = InputLanguage.InstalledInputLanguages[i];
                System.Windows.Forms.ToolStripMenuItem m = new ToolStripMenuItem();
                m.Text = t.LayoutName;
                m.Click += new EventHandler(m_Click);
                neuContextMenuStrip1.Items.Add(m);
            }

            this.ReadInputLanguage();
        }

        /// <summary>
        /// ��ȡ��ǰĬ�����뷨
        /// </summary>
        private void ReadInputLanguage()
        {
            if (!System.IO.File.Exists(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/feeSetting.xml"))
            {
               // Neusoft.UFC.Common.Classes.Function.CreateFeeSetting();

            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/feeSetting.xml");
                XmlNode node = doc.SelectSingleNode("//IME");

                CHInput = GetInputLanguage(node.Attributes["currentmodel"].Value);

                if (CHInput != null)
                {
                    foreach (ToolStripMenuItem m in neuContextMenuStrip1.Items)
                    {
                        if (m.Text == CHInput.LayoutName)
                        {
                            m.Checked = true;
                        }
                    }
                }

                //��ӵ�������

            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡĬ���������뷨����!" + e.Message);
                return;
            }
        }

        /// <summary>
        /// �������뷨���ƻ�ȡ���뷨
        /// </summary>
        /// <param name="LanName"></param>
        /// <returns></returns>
        private InputLanguage GetInputLanguage(string LanName)
        {
            foreach (InputLanguage input in InputLanguage.InstalledInputLanguages)
            {
                if (input.LayoutName == LanName)
                {
                    return input;
                }
            }
            return null;
        }

        /// <summary>
        /// ���浱ǰ���뷨
        /// </summary>
        private void SaveInputLanguage()
        {
            if (!System.IO.File.Exists(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/feeSetting.xml"))
            {
               // Neusoft.UFC.Common.Classes.Function.CreateFeeSetting();
            }
            if (CHInput == null)
                return;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/feeSetting.xml");
                XmlNode node = doc.SelectSingleNode("//IME");

                node.Attributes["currentmodel"].Value = CHInput.LayoutName;

                doc.Save(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/feeSetting.xml");
            }
            catch (Exception e)
            {
                MessageBox.Show("����Ĭ���������뷨����!" + e.Message);
                return;
            }
        }

        private void m_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem m in this.neuContextMenuStrip1.Items)
            {
                if (sender == m)
                {
                    m.Checked = true;
                    this.CHInput = this.GetInputLanguage(m.Text);
                    //�������뷨
                    this.SaveInputLanguage();
                }
                else
                {
                    m.Checked = false;
                }
            }
        }

        #endregion

        #region �¼�
        private void ucPatientInfo_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() != "devenv")
            {
                #region Ȩ���ж�
                Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager user = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager();
                NeuObject dept = (accountManager.Operator as HISFC.Models.Base.Employee).Dept;
                //�ж��Ƿ��м���Ȩ��
                this.IsEnableEntry = user.JudgeUserPriv(accountManager.Operator.ID, dept.ID, "5001", "01");

                //VipȨ������
                this.IsEnableVip = user.JudgeUserPriv(accountManager.Operator.ID, dept.ID, "5002", "01");


                #endregion

                this.Init();
                this.ActiveControl = this.txtName;
            }
        }

        private void dtpBirthDay_ValueChanged(object sender, EventArgs e)
        {
            //�������ͳһ���㷨 {04CF4C0D-DE0A-426c-8724-76CA4CDBC267} wbo 2010-11-14
            //string age = this.accountManager.GetAge(this.dtpBirthDay.Value);
            string age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.dtpBirthDay.Value);
            this.txtAge.TextChanged -= new EventHandler(txtAge_TextChanged);
            this.txtAge.Text = age.Substring(0, age.Length -1);
            this.txtAge.TextChanged += new EventHandler(txtAge_TextChanged);
            this.cmbAgeUnit.SelectedIndexChanged -= new EventHandler(cmbAgeUnit_SelectedIndexChanged);
            //���BUG�е�... {9A74FC53-BC06-4237-89FE-E1A71806A594} wbo 2010-11-14
            //this.cmbAgeUnit.Text = age.Substring(age.Length - 1, 1);
            try
            {
                string unitName = age.Substring(age.Length - 2, 1);
                if (unitName != "��" && unitName != "��" && unitName != "��")
                {
                    unitName = "��";
                }
                this.cmbAgeUnit.Text = unitName;
            }
            catch (Exception ex)
            { }
            this.cmbAgeUnit.SelectedIndexChanged += new EventHandler(cmbAgeUnit_SelectedIndexChanged);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
                if (txtMatherName.Focused)
                {
                    if (OnFoucsOver != null)
                    {
                        OnFoucsOver(null, null);
                        return true;
                    }
                }
                if (this.txtIDNO.Focused)
                {
                    if (this.cmbCardType.Tag.ToString() == "01")
                    {
                        string error = string.Empty;
                        string idNO = this.txtIDNO.Text.Trim();
                        if (idNO != string.Empty)
                        {
                            if (Neusoft.FrameWork.WinForms.Classes.Function.CheckIDInfo(idNO, ref error) < 0)
                            {
                                MessageBox.Show(error);
                                return true;
                            }
                            //�������֤�Ż�ȡ�����Ա�
                            Neusoft.FrameWork.Models.NeuObject obj = Class.Function.GetSexFromIdNO(idNO, ref error);
                            if (obj == null)
                            {
                                MessageBox.Show(error);
                                return true;
                            }
                            this.cmbSex.Tag = obj.ID;
                            //���ݻ������֤�Ż�ȡ����
                            string birthdate = Class.Function.GetBirthDayFromIdNO(idNO, ref error);
                            if (birthdate == "-1")
                            {
                                MessageBox.Show(error);
                                return true;
                            }
                            this.dtpBirthDay.Value = FrameWork.Function.NConvert.ToDateTime(birthdate);
                        }
                    }
                }

                
                Control currentContol = this.GetCurrentFoucsControl();
               
                if (isMustInputTabInde)
                {
                    SetMustInputFocus(currentContol);
                }
                else
                {
                    SendKeys.Send("{Tab}");
                }
                #region ��ѯ������Ϣ
                if (inpubMaxTabIndex == currentContol.TabIndex)
                {
                    if (OnEnterSelectPatient != null)
                    {
                        this.OnEnterSelectPatient(null, null);
                    }
                }
                #endregion

                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="age"></param>
        /// <param name="ageUnit"></param>
        /// <returns></returns>
        private void ConvertBirthdayByAge()
        {
            DateTime current = this.accountManager.GetDateTimeFromSysDateTime();

            DateTime birthday = current;
            string ageUnit = this.cmbAgeUnit.Text;

            string strAge = this.txtAge.Text.Trim();
            if (string.IsNullOrEmpty(strAge))
            {
                this.txtAge.Text = "1";
                strAge = "1";
            }

            int age = Neusoft.FrameWork.Function.NConvert.ToInt32(strAge);

            if (ageUnit == "��")
            {
                birthday = current.AddYears(-age);
            }
            else if (ageUnit == "��")
            {
                birthday = current.AddMonths(-age);
            }
            else if (ageUnit == "��")
            {
                birthday = current.AddDays(-age);
            }
            this.dtpBirthDay.ValueChanged -=new EventHandler(dtpBirthDay_ValueChanged);
            this.dtpBirthDay.Value = birthday;
            this.dtpBirthDay.ValueChanged += new EventHandler(dtpBirthDay_ValueChanged);

        }

        private void cmbAgeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ConvertBirthdayByAge();
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            this.ConvertBirthdayByAge();
        }

        #endregion
        


    }
}
