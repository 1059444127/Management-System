using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.HISFC.Models.HealthRecord.EnumServer;
namespace Neusoft.HISFC.Components.HealthRecord.CaseLend
{
    /// <summary>
    /// ucBorrowCase<br></br>
    /// [��������: ��������]<br></br>
    /// [�� �� ��: �ſ���]<br></br>
    /// [����ʱ��: 2007-04-20]<br></br>
    /// <�޸ļ�¼ 
    ///		�޸���='' 
    ///		�޸�ʱ��='yyyy-mm-dd' 
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucBorrowCase : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucBorrowCase()
        {
            InitializeComponent();
        }


        #region ��������Ϣ

        /// <summary>
        /// ���幤��������
        /// </summary>
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        #region ��ʼ��������
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            toolBarService.AddToolButton("����", "����", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.J���, true, false, null);
            toolBarService.AddToolButton("ɾ��", "ɾ��", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sɾ��, true, false, null); 
            //toolBarService.AddToolButton("�����ϸ", "�����ϸ", 2, true, false, null);
            //toolBarService.AddToolButton("ɾ����ϸ", "ɾ����ϸ", 3, true, false, null);
            //toolBarService.AddToolButton("����", "����", 6, true, false, null);
            return toolBarService;
        }
        #endregion

        #region ���������Ӱ�ť�����¼�
        /// <summary>
        /// ���������Ӱ�ť�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "����":
                    LendCase();
                    break;
                case "ɾ��":
                    {
                        DeleteLend();
                        break;
                    }
                default:
                    break;
            }
        }
        #endregion

        #endregion

        #region ȫ�ֱ���
        private Neusoft.HISFC.BizLogic.HealthRecord.CaseCard card = new Neusoft.HISFC.BizLogic.HealthRecord.CaseCard();
        private Neusoft.HISFC.BizLogic.HealthRecord.Base baseDml = new Neusoft.HISFC.BizLogic.HealthRecord.Base();
        private System.Data.DataTable dt = null;
        private ArrayList Caselist = null;
        //���Ŀ���Ϣ 
        private Neusoft.HISFC.Models.HealthRecord.ReadCard Cardinfo = null;
        //����
        Neusoft.FrameWork.Public.ObjectHelper NationalHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        //�Ա�
        Neusoft.FrameWork.Public.ObjectHelper SexHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        //����
        Neusoft.FrameWork.Public.ObjectHelper DeptHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        #endregion

        /// <summary>
        /// ���
        /// </summary>
        private void LendCase()
        {
            //�����ж��Ƿ��Ѿ�����ˣ���������û�й黹���������
            //������� 
            ArrayList list = GetLendInfo();
            if (list == null || list.Count == 0)
            {
               // MessageBox.Show("û����Ϣ");
                return;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(card.Connection);
            //trans.BeginTransaction();

            card.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            foreach (Neusoft.HISFC.Models.HealthRecord.Lend obj in list)
            {
                if (obj == null)
                {
                    return;
                }
                if (ValidState(obj) == -1)
                {
                    return;
                } 
                if (card.LendCase(obj) < 1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("������ļ�¼ʧ��: " + card.Err);
                    return;
                }
                if (card.UpdateBase(LendType.O, obj.CaseBase.CaseNO) <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("���²�������ʧ��");
                    return;
                }
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show("���ĳɹ�");
            this.ClearCase();
            this.ClearPerson();
            this.caseDetail.RowCount = 0;
            this.caseMain.RowCount = 0;
            this.caseNo.Focus();
        }
        private void caseNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                try
                {
                    if (this.caseNo.Text == "")
                    {
                        this.caseNo.Focus();

                        MessageBox.Show("�����벡����");
                        return;
                    }
                    caseNo.Text = caseNo.Text.PadLeft(10, '0');
                    Caselist = null;
                    Caselist = baseDml.QueryCaseBaseInfoByCaseNO(this.caseNo.Text);
                    if (Caselist == null)
                    {
                        this.caseNo.SelectAll();
                        MessageBox.Show("��ѯ������Ϣ����");
                        return;
                    }
                    if (Caselist.Count == 0)
                    {
                        this.caseNo.SelectAll();
                        MessageBox.Show("û�в鵽�����Ϣ");
                        return;
                    }
                    //�ж��Ƿ��Ѿ������ 
                    Neusoft.HISFC.Models.HealthRecord.Base info = (Neusoft.HISFC.Models.HealthRecord.Base)Caselist[0];
                    if (info.LendStat == "O") //����ĸ O 
                    {
                        this.caseNo.SelectAll();
                        MessageBox.Show("�ò����Ѿ����.");
                        return;
                    }
                    this.AddCaseInfo(Caselist);
                    this.caseNo.SelectAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        private enum Cols
        {
            caseNO,//������
            strName,//����
            sexName,//�Ա�
            nationName,//����
            birthday,//����
            birthArea,//������
            linkPhone,//��ϵ�绰
            linkArea//��ϵ��ַ
        }
        /// <summary>
        /// ������ϸ��Ϣ
        /// </summary>
        private enum DetailCos
        {
            InpatientNO, //סԺ��ˮ��
            patientNO,//סԺ��
            caseNO,//������
            strName,//����
            deptIN, //��Ժ����
            dateIN,//��Ժ����
            DeptOut,//��Ժ����
            dateOut //��Ժ����
        }
        /// <summary>
        /// ���ز�����Ϣ
        /// </summary>
        /// <param name="Caselist"></param>
        /// <returns></returns>
        private int AddCaseInfo(ArrayList Caselist)
        {
            #region ���ز���������Ϣ
            Neusoft.HISFC.Models.HealthRecord.Base info = (Neusoft.HISFC.Models.HealthRecord.Base)Caselist[0]; 
            for (int i = 0; i < this.caseMain.RowCount; i++)
            {
                if (caseMain.Cells[i, (int)Cols.caseNO].Text == info.CaseNO)
                {
                    return 1;
                }
            }
            txName.Text = info.PatientInfo.Name;
            txSex.Text = SexHelper.GetName(info.PatientInfo.Sex.ID.ToString());
            this.caseMain.Rows.Add(0, 1);
            this.caseMain.Cells[0, (int)Cols.caseNO].Text = info.CaseNO;
            this.caseMain.Cells[0, (int)Cols.strName].Text = info.PatientInfo.Name; 
            if (info.PatientInfo.Sex.ID != null)
            {
                this.caseMain.Cells[0, (int)Cols.sexName].Text = SexHelper.GetName(info.PatientInfo.Sex.ID.ToString()); //�Ա�
            }
            this.caseMain.Cells[0, (int)Cols.nationName].Text = NationalHelper.GetName(info.PatientInfo.Nationality.ID);//����
            this.caseMain.Cells[0, (int)Cols.birthArea].Text = info.PatientInfo.AreaCode;//������
            this.caseMain.Cells[0, (int)Cols.birthday].Text = info.PatientInfo.Birthday.ToShortDateString(); //����
            this.caseMain.Cells[0, (int)Cols.linkArea].Text = info.PatientInfo.Kin.RelationAddress;//��ϵ��ַ
            this.caseMain.Cells[0, (int)Cols.linkPhone].Text = info.PatientInfo.Kin.RelationPhone;//��ϵ�绰
            this.caseMain.Rows[0].Tag = info;
            #endregion   
            return 1;
        } 
        /// <summary>
        /// ��ֵ
        /// </summary>
        /// <param name="info"></param>
        private void SetInfo(Neusoft.HISFC.Models.HealthRecord.Base info)
        { 
            if (info.PatientInfo.Sex.ID != null)
            {
                txSex.Text = SexHelper.GetName(info.PatientInfo.Sex.ID.ToString());//
            }
            caseNo.Text = info.CaseNO;
            txName.Text = info.PatientInfo.Name;
            
            txDeptIn.Text = info.InDept.Name;
            txDeptOut.Text = info.OutDept.ID;
            dtInDate.Text = info.PatientInfo.PVisit.InTime.ToString();
            dtOutDate.Text = info.PatientInfo.PVisit.OutTime.ToString();
            dtBirthDate.Text = info.PatientInfo.Birthday.ToString();
        }
        /// <summary>
        /// ��ղ�����Ϣ
        /// </summary>
        private void ClearCase()
        {
            caseNo.Text = "";
            txName.Text = "";
            txSex.Text = "";
            txDeptIn.Text = "";
            txDeptOut.Text = "";
            dtInDate.Text = "";
            dtOutDate.Text = "";
            dtBirthDate.Text = "";
            if (dt != null)
            {
                dt.Clear();
            }
        }
        /// <summary>
        /// �����Ա��Ϣ
        /// </summary>
        private void ClearPerson()
        {
            CardNO.Text = "";
            comPerson.Text = "";
            //			txDays.Text = "";
            comType.Text = "";
            //txReturnTime.Value = Convert.ToDateTime("3000-1-1");
        }
        private void frmLendCard_Load(object sender, System.EventArgs e)
        {
            //InitDateTable();
            LockFp();
            Neusoft.HISFC.BizLogic.Manager.Person person = new Neusoft.HISFC.BizLogic.Manager.Person();
            Neusoft.HISFC.BizProcess.Integrate.Manager managerMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            //��ȡ��Ա�б�
            ArrayList DoctorList = person.GetEmployeeAll();
            this.comPerson.AppendItems(DoctorList);
            txReturnTime.Value = Neusoft.FrameWork.Function.NConvert.ToDateTime(baseDml.GetSysDate()).AddDays(14);
            comPerson.BackColor = System.Drawing.Color.White;
            this.SexHelper.ArrayObject = Neusoft.HISFC.Models.Base.SexEnumService.List();
            ArrayList list = managerMgr.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.NATION);
            this.NationalHelper.ArrayObject = list;
        }
        private void LockFp()
        {
            FarPoint.Win.Spread.CellType.TextCellType txtCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.caseMain.GrayAreaBackColor = System.Drawing.Color.White;
            this.caseDetail.GrayAreaBackColor = System.Drawing.Color.White;
            this.caseMain.Columns[(int)Cols.caseNO].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.strName].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.sexName].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.nationName].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.birthday].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.birthArea].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.linkPhone].CellType = txtCellType;
            this.caseMain.Columns[(int)Cols.linkArea].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.InpatientNO].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.patientNO].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.caseNO].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.strName].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.deptIN].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.dateIN].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.DeptOut].CellType = txtCellType;
            this.caseDetail.Columns[(int)DetailCos.dateOut].CellType = txtCellType;
        }
        private void CardNO_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.caseDetail.RowCount = 0;
                this.caseMain.RowCount = 0;
                if (CardNO.Text == "")
                {
                    CardNO.Focus();
                    MessageBox.Show("�����뿨��");
                    return;
                }
                Cardinfo = null;
                Cardinfo = card.GetCardInfo(this.CardNO.Text);
                if (Cardinfo == null)
                {
                    MessageBox.Show("��ѯ����");
                    return;
                }
                if (Cardinfo.CardID == null || Cardinfo.CardID == "")
                {
                    MessageBox.Show("û�в鵽�ÿ��ŵ������Ϣ");
                    return;
                }
                CardNO.Text = Cardinfo.CardID;
                comPerson.Text = Cardinfo.EmployeeInfo.Name;
                comPerson.Tag = Cardinfo.EmployeeInfo.ID;
                comType.Text = "�ڽ�";
                //				this.txDays.Focus();
                this.comType.Focus();
            }
        }

        private int ValidState(Neusoft.HISFC.Models.HealthRecord.Lend obj)
        {
            if (CardNO.Text == null && CardNO.Text == "")
            {
                MessageBox.Show("��������Ŀ���");
                return -1;
            }
            if (caseNo.Text == null && caseNo.Text == "")
            {
                MessageBox.Show("��������Ŀ���");
                return -1;
            }
            if (comType.Text == "")
            {
                MessageBox.Show("���ķ�ʽ");
                return -1;
            }
            if (this.txReturnTime.Value <= System.DateTime.Now)
            {
                MessageBox.Show("Ԥ�ƹ黹���ڲ���С�ڵ�ǰʱ��");
                return -1;
            }
            return 1;
        } 
        #region ��ȡ������Ϣ
        private ArrayList GetLendInfo()
        {
            ArrayList list = new ArrayList();
            if (this.caseMain.RowCount == 0)
            {
                MessageBox.Show("��ѡ����ĵĲ�����Ϣ");
                return list;
            }
            if (Cardinfo == null)
            {
                MessageBox.Show("��ѡ����ĵĲ�����Ϣ");
                return null;
            }
            if (comType.Text != "�ڽ�" && comType.Text != "���")
            {
                MessageBox.Show("��ѡ����ķ�ʽ");
                return null;
            }
            if (txReturnTime.Value < System.DateTime.Now)
            {
                System.TimeSpan sp = System.DateTime.Now - txReturnTime.Value;
                if (sp.Days > 1)
                {
                    MessageBox.Show("Ԥ�ƹ黹���ڲ���С�ڵ�ǰ����");
                    return null;
                }
            }
            for (int i = 0; i < this.caseMain.RowCount; i++)
            {
                //{B6105962-245E-4106-89F2-3469B2065617}
                //Neusoft.HISFC.Models.HealthRecord.Lend Saveinfo = new Neusoft.HISFC.Models.HealthRecord.Lend();


                Neusoft.HISFC.Models.HealthRecord.Base objBase = (Neusoft.HISFC.Models.HealthRecord.Base)this.caseMain.Rows[i].Tag;

                ArrayList tempList = this.baseDml.QueryCaseBaseInfoByCaseNO(objBase.CaseNO);
                foreach (Neusoft.HISFC.Models.HealthRecord.Base tempObj in tempList)
                {
                    //{B6105962-245E-4106-89F2-3469B2065617}
                    Neusoft.HISFC.Models.HealthRecord.Lend Saveinfo = new Neusoft.HISFC.Models.HealthRecord.Lend();
                    Saveinfo.SeqNO = this.card.GetSequence("Case.CaseCard.LendCase.Seq");
                    if (Saveinfo.SeqNO == null || Saveinfo.SeqNO == "")
                    {
                        MessageBox.Show("��ȡ���ʧ��");
                        return null;
                    }
                    if (tempObj.LendStat == "O") //����ĸ O 
                    {
                        MessageBox.Show("�ò����Ѿ����.");
                        return��null;
                    }
                    Saveinfo.CaseBase.CaseNO = tempObj.CaseNO;
                    Saveinfo.CaseBase.PatientInfo.ID = tempObj.PatientInfo.ID;//סԺ��ˮ��
                    Saveinfo.CaseBase.CaseNO = tempObj.CaseNO;//����סԺ�� 
                    Saveinfo.CaseBase.PatientInfo.Name = tempObj.PatientInfo.Name; //��������
                    Saveinfo.CaseBase.PatientInfo.Sex.ID = tempObj.PatientInfo.Sex.ID;//�Ա�
                    Saveinfo.CaseBase.PatientInfo.Birthday = tempObj.PatientInfo.Birthday;//��������
                    Saveinfo.CaseBase.PatientInfo.PVisit.InTime = tempObj.PatientInfo.PVisit.InTime;//��Ժ����
                    Saveinfo.CaseBase.PatientInfo.PVisit.OutTime = tempObj.PatientInfo.PVisit.OutTime;//��Ժ����
                    Saveinfo.CaseBase.InDept.ID = tempObj.InDept.ID; //��Ժ���Ҵ���
                    Saveinfo.CaseBase.InDept.Name = tempObj.InDept.Name; //��Ժ��������
                    Saveinfo.CaseBase.OutDept.ID = tempObj.OutDept.ID;  //��Ժ���Ҵ���
                    Saveinfo.CaseBase.OutDept.Name = tempObj.OutDept.Name; //��Ժ��������
                    Saveinfo.EmployeeInfo.ID = Cardinfo.EmployeeInfo.ID;//�����˴���
                    Saveinfo.EmployeeInfo.Name = Cardinfo.EmployeeInfo.Name;//����������
                    Saveinfo.EmployeeDept.ID = Cardinfo.DeptInfo.ID; //���������ڿ��Ҵ���
                    Saveinfo.EmployeeDept.Name = Cardinfo.DeptInfo.Name; //���������ڿ�������
                    Saveinfo.LendDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(baseDml.GetSysDate()); //��������
                    Saveinfo.PrerDate = txReturnTime.Value; //Ԥ������
                    if (this.comType.Text == "�ڽ�")
                    {
                        Saveinfo.LendKind = "1"; ; //��������
                    }
                    else if (this.comType.Text == "���")
                    {
                        Saveinfo.LendKind = "2"; ; //��������
                    }
                    Saveinfo.LendStus = "1"; ;//����״̬ 1���/2����
                    Saveinfo.ID = baseDml.Operator.ID; //����Ա����
                    Saveinfo.OperInfo.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(baseDml.GetSysDate()); //����ʱ��
                    Saveinfo.ReturnOperInfo.ID = "";   //�黹����Ա����
                    Saveinfo.ReturnDate = Neusoft.FrameWork.Function.NConvert.ToDateTime("3000-1-1");   //ʵ�ʹ黹����
                    Saveinfo.CardNO = CardNO.Text;//����
                    list.Add(Saveinfo);
                }
            }
            return list;
        }
        #endregion  

        private void comType_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.txReturnTime.Focus();
            }
        }

        private void txReturnTime_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.caseNo.Focus();
            }
        }

        private void fpSpread2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //this.caseDetail.RowCount = 0;
            //if (this.caseMain.Rows.Count == 0)
            //{
            //    return;
            //}
            //Neusoft.HISFC.Models.HealthRecord.Base PatientCaseInfo = (Neusoft.HISFC.Models.HealthRecord.Base)this.caseMain.Rows[this.caseMain.ActiveRowIndex].Tag;
            //SetInfo(PatientCaseInfo);
            //ArrayList tempList = this.baseDml.QueryCaseBaseInfoByCaseNO(PatientCaseInfo.CaseNO);

            //#region ���ز�����ϸ��Ϣ
            //foreach (Neusoft.HISFC.Models.HealthRecord.Base obj in tempList)
            //{
            //    this.caseDetail.Rows.Add(0, 1);
            //    this.caseDetail.Cells[0, (int)DetailCos.InpatientNO].Text = obj.PatientInfo.ID;//סԺ��ˮ��
            //    this.caseDetail.Cells[0, (int)DetailCos.patientNO].Text = obj.PatientInfo.PID.PatientNO;//סԺ��
            //    this.caseDetail.Cells[0, (int)DetailCos.caseNO].Text = obj.CaseNO;//������
            //    this.caseDetail.Cells[0, (int)DetailCos.strName].Text = obj.PatientInfo.Name;
            //    this.caseDetail.Cells[0, (int)DetailCos.deptIN].Text = obj.InDept.Name;//��Ժ����
            //    this.caseDetail.Cells[0, (int)DetailCos.dateIN].Text = obj.PatientInfo.PVisit.InTime.ToShortDateString();//��Ժ����
            //    this.caseDetail.Cells[0, (int)DetailCos.DeptOut].Text = obj.OutDept.Name;//��Ժ����
            //    this.caseDetail.Cells[0, (int)DetailCos.dateOut].Text = obj.PatientInfo.PVisit.OutTime.ToShortDateString();//��Ժ����
            //    this.caseDetail.Rows[0].Tag = obj;
            //}
            //#endregion 

            //this.tabControl1.SelectedIndex = 1;
        }

        private void caseNo_Enter(object sender, EventArgs e)
        {
            this.caseNo.SelectAll();
        }

        private void CardNO_Enter(object sender, EventArgs e)
        {
            this.caseNo.SelectAll();
        }

        private void fpSpread2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            this.caseDetail.RowCount = 0;
            if (this.caseMain.Rows.Count == 0)
            {
                return;
            }
            Neusoft.HISFC.Models.HealthRecord.Base PatientCaseInfo = (Neusoft.HISFC.Models.HealthRecord.Base)this.caseMain.Rows[this.caseMain.ActiveRowIndex].Tag;
            SetInfo(PatientCaseInfo);
            ArrayList tempList = this.baseDml.QueryCaseBaseInfoByCaseNO(PatientCaseInfo.CaseNO);

            #region ���ز�����ϸ��Ϣ
            foreach (Neusoft.HISFC.Models.HealthRecord.Base obj in tempList)
            {
                this.caseDetail.Rows.Add(0, 1);
                this.caseDetail.Cells[0, (int)DetailCos.InpatientNO].Text = obj.PatientInfo.ID;//סԺ��ˮ��
                this.caseDetail.Cells[0, (int)DetailCos.patientNO].Text = obj.PatientInfo.PID.PatientNO;//סԺ��
                this.caseDetail.Cells[0, (int)DetailCos.caseNO].Text = obj.CaseNO;//������
                this.caseDetail.Cells[0, (int)DetailCos.strName].Text = obj.PatientInfo.Name;
                this.caseDetail.Cells[0, (int)DetailCos.deptIN].Text = obj.InDept.Name;//��Ժ����
                this.caseDetail.Cells[0, (int)DetailCos.dateIN].Text = obj.PatientInfo.PVisit.InTime.ToShortDateString();//��Ժ����
                this.caseDetail.Cells[0, (int)DetailCos.DeptOut].Text = obj.OutDept.Name;//��Ժ����
                this.caseDetail.Cells[0, (int)DetailCos.dateOut].Text = obj.PatientInfo.PVisit.OutTime.ToShortDateString();//��Ժ����
                this.caseDetail.Rows[0].Tag = obj;
            }
            #endregion

            this.tabControl1.SelectedIndex = 1;
        }

        private void DeleteLend()
        {
            if (this.caseMain.Rows.Count > 0)
            {
                this.caseMain.Rows.Remove(this.caseMain.ActiveRowIndex, 1);
            }
        }
    }
}
