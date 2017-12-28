using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

namespace Neusoft.HISFC.Components.Nurse
{
    public partial class ucRegister : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        #region ������
        /// <summary>
        /// ������ù�����
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Fee patientMgr = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        /// <summary>
        /// Ժע������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Nurse.Inject InjMgr = new Neusoft.HISFC.BizLogic.Nurse.Inject();
        /// <summary>
        /// 
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Pharmacy drugMgr = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        /// <summary>
        /// 
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Registration.Registration regMgr = new Neusoft.HISFC.BizProcess.Integrate.Registration.Registration();
        /// <summary>
        /// ���Һ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager DeptMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        /// <summary>
        /// ��Ա����
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager PsMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        /// <summary>
        /// �Һ�ʵ��
        /// </summary>
        private Neusoft.HISFC.Models.Registration.Register reg = null;
        /// <summary>
        /// ����ҵ��㺯��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager conMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        /// <summary>
        /// ҩ������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Pharmacy storeMgr = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();
        private ArrayList al = new ArrayList();
        /// <summary>
        /// ���Ƶ�������
        /// </summary>
        private ArrayList alPrint = null;
        /// <summary>
        /// ע�䵥������
        /// </summary>
        private ArrayList alInject = null;
        /// <summary>
        /// ��������
        /// </summary>
        private Hashtable htSamples = new Hashtable();
        /// <summary>
        /// ҽ������
        /// </summary>
        private Hashtable htDoctors = new Hashtable();
        /// <summary>
        /// �Ƿ��һ�εǼ�
        /// </summary>
        private bool IsFirstTime = false;
        /// <summary>
        /// Ժע����
        /// </summary>
        private int countInject = 0;
        /// <summary>
        /// ���ע��˳���
        /// </summary>
        private int maxInjectOrder = 0;
        #region ע��˳���
        /// <summary>
        /// �Ƿ��Զ�����ע��˳���
        /// </summary>
        private bool IsAutoOrder = true;
        /// <summary>
        /// ��ǰע��˳���
        /// </summary>
        private int currentOrder = 0;
        #endregion

        /// <summary>
        /// �Ƿ���ʾ���ߵ���ɵǼǵ�ȫ������
        /// {24A47206-F111-4817-A7B4-353C21FC7724}
        /// </summary>
        private bool isShowAllInject = false;

        /// <summary>
        /// �����洢Ƶ��ʵ����ֵ�
        /// {24A47206-F111-4817-A7B4-353C21FC7724}
        /// </summary>
        private Dictionary<string, Neusoft.HISFC.Models.Order.Frequency> dicFrequency = new Dictionary<string, Neusoft.HISFC.Models.Order.Frequency>();

        /// <summary>
        /// 
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.Nurse.IGetInjectOrderNo IGetOrderNo = null;

        #region {EDC3F829-A686-409e-A4F4-4D5B8C2F3799} ����ע������������ by guanyx
        private event System.EventHandler ReadCardEvent;
        #endregion

        #endregion

        #region ����

        /// <summary>
        /// �Ƿ���ʾ���ߵ���ɵǼǵ�ȫ������
        /// {24A47206-F111-4817-A7B4-353C21FC7724}
        /// </summary>
        [Description("�Ƿ���ʾ���ߵ���ɵǼǵ�ȫ������"), Category("����"), DefaultValue("false")]
        public bool IsShowAllInject
        {
            get
            {
                return isShowAllInject;
            }
            set
            {
                isShowAllInject = value;
            }
        }

        #endregion

        #region ��ʼ��
        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            this.dtpStart.Value = this.InjMgr.GetDateTimeFromSysDateTime().AddDays(-7);
            this.dtpEnd.Value = this.InjMgr.GetDateTimeFromSysDateTime();
            this.lbCue.Text = "";
            this.initDoctor();
            this.txtCardNo.Focus();
            this.InitOrder();
        }
        /// <summary>
        /// ��ʼ��ҽ��
        /// </summary>
        private void initDoctor()
        {
            Neusoft.HISFC.BizProcess.Integrate.Manager doctMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();

            al = doctMgr.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D);
            if (al != null)
            {
                foreach (Neusoft.HISFC.Models.Base.Employee p in al)
                {
                    this.htDoctors.Add(p.ID, p.Name);
                }
            }
        }
        #endregion

        #region ������

        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("ȫѡ", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȫѡ, true, false, null);
            this.toolBarService.AddToolButton("ȡ��", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȡ��, true, false, null);
            this.toolBarService.AddToolButton("��ӡƿǩ", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ, true, false, null);
            this.toolBarService.AddToolButton( "��ӡǩ����", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ, true, false, null );
            this.toolBarService.AddToolButton( "��ӡע�䵥", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ, true, false, null );
            this.toolBarService.AddToolButton( "��ӡ���߿�", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ, true, false, null );
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B} ���Ӵ�ӡ������
            this.toolBarService.AddToolButton("��ӡ������", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.D��ӡ, true, false, null);
            //{26E88889-B2CF-4965-AFD8-6D9BE4519EBF}
            this.toolBarService.AddToolButton("�޸�Ƥ��", "", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.X�޸�, true, false, null);
            #region {EDC3F829-A686-409e-A4F4-4D5B8C2F3799} ����ע������������ by guanyx
            this.ReadCardEvent += new EventHandler(ucRegister_ReadCardEvent);
            this.toolBarService.AddToolButton("����", "��Ժ�ڿ�", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Z�ٻ�, true, false, this.ReadCardEvent);
            #endregion
            return this.toolBarService;
        }

        #region {EDC3F829-A686-409e-A4F4-4D5B8C2F3799} ����ע������������ by guanyx
        private string cardno = "";
        private bool isNewCard = false;
        ZZlocal.Clinic.HISFC.OuterConnector.ICCard.ICReader icreader = new ZZlocal.Clinic.HISFC.OuterConnector.ICCard.ICReader();
        /// <summary>
        /// ��������
        /// {EDC3F829-A686-409e-A4F4-4D5B8C2F3799} ����ע������������ by guanyx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucRegister_ReadCardEvent(object sender, EventArgs e)
        {
            if (icreader.GetConnect())
            {
                cardno = icreader.ReaderICCard();
                if (cardno == "0000000000")
                {
                    isNewCard = true;
                    MessageBox.Show("�ÿ�δд�뿨�ţ����ֹ����뻼�߿��Ų��á��س�����ȡ������Ϣ��");
                }
                else
                {
                    this.txtCardNo.Text = cardno;
                    this.txtCardNo_KeyDown(this.txtCardNo, new KeyEventArgs(Keys.Enter));
                }
                icreader.CloseConnection();
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ�");
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnQuery(object sender, object neuObject)
        {
            this.Query();
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text.Trim())
            {
                case "ȫѡ":
                    this.SelectedAll(true);
                    break;
                case "ȡ��":
                    this.SelectedAll(false);
                    break;
                case "��ӡƿǩ":
                    this.PrintCure();
                    break;
                case "��ӡǩ����":
                    this.PrintItinerate();
                    break;
                case "��ӡע�䵥":
                    this.PrintInject();
                    break;
                case "��ӡ���߿�":
                    this.PrintPatient();
                    break;
                //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B} ���Ӵ�ӡ������
                case "��ӡ������":
                    this.PrintNumber();
                    break;
                //{26E88889-B2CF-4965-AFD8-6D9BE4519EBF}
                case "�޸�Ƥ��":
                    this.ModifyHytest();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ����
        /// <summary>
        /// ��ȡҽ�����Ƹ��ݴ���
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetDoctByID(string ID)
        {
            IDictionaryEnumerator dict = htDoctors.GetEnumerator();
            while (dict.MoveNext())
            {
                if (dict.Key.ToString() == ID)
                    return dict.Value.ToString();
            }

            return "";
        }
        /// <summary>
        /// ���ø�ʽ
        /// </summary>
        private void SetFP()
        {
            FarPoint.Win.Spread.CellType.TextCellType txtOnly = new FarPoint.Win.Spread.CellType.TextCellType();
            txtOnly.ReadOnly = true;
            this.neuSpread1_Sheet1.Columns[2].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[3].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[3].Visible = false;
            this.neuSpread1_Sheet1.Columns[4].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[5].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[6].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[7].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[8].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[9].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[10].CellType = txtOnly;
            this.neuSpread1_Sheet1.Columns[11].CellType = txtOnly;
            //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
            this.neuSpread1_Sheet1.Columns[13].CellType = txtOnly;
        }
        /// <summary>
        /// ��ȡ���
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetNoon(DateTime dt)
        {
            string strNoon = "����";
            if (Neusoft.FrameWork.Function.NConvert.ToInt32(dt.ToString("HH")) >= 12)
            {
                strNoon = "����";
            }
            return strNoon;
        }
        /// <summary>
        /// ѹ����ʾ
        /// </summary>
        private void LessShow()
        {

        }
        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNum(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!Char.IsNumber(str, i))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// ������ɫ(��������ʾ���һ��clinicҽ��)
        /// </summary>
        /// <returns></returns>
        private int ShowColor()
        {
            //ȡ�����clinic_code
            int maxClinic = 0;
            if (this.neuSpread1_Sheet1.RowCount <= 0) return -1;
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item =
                    (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                if (Neusoft.FrameWork.Function.NConvert.ToInt32(item.ID) > maxClinic)
                {
                    maxClinic = Neusoft.FrameWork.Function.NConvert.ToInt32(item.ID);
                }
            }
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item =
                    (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                if (item.ID == maxClinic.ToString())
                {
                    //					this.fpSpread1_Sheet1.Rows[i].BackColor = System.Drawing.Color.LightYellow;
                    this.neuSpread1_Sheet1.Rows[i].ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.neuSpread1_Sheet1.SetValue(i, 0, false);
                }
            }
            return 0;
        }
        /// <summary>
        /// ֻ��ʾ���һ�ε�
        /// </summary>
        /// <returns></returns>
        private int ShowLastOnly()
        {
            //ȡ�����clinic_code
            int maxClinic = 0;
            if (this.neuSpread1_Sheet1.RowCount <= 0) return -1;
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item =
                    (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                if (Neusoft.FrameWork.Function.NConvert.ToInt32(item.ID) > maxClinic)
                {
                    maxClinic = Neusoft.FrameWork.Function.NConvert.ToInt32(item.ID);
                }
            }
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item =
                    (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                if (item.ID != maxClinic.ToString())
                {
                    //this.fpSpread1_Sheet1.Rows[i].BackColor = System.Drawing.Color.LightYellow;
                    this.neuSpread1_Sheet1.SetValue(i, 0, false);
                    this.neuSpread1_Sheet1.Rows[i].Remove();
                }
            }
            return 0;
        }
        /// <summary>
        /// ������ע��˳��
        /// </summary>
        /// <returns></returns>
        private int GetMaxInjectOrder()
        {
            if (this.neuSpread1_Sheet1.RowCount <= 0) return 0;
            this.neuSpread1.StopCellEditing();
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                if (this.neuSpread1_Sheet1.GetText(i, 0).ToUpper() == "FALSE" ||
                    this.neuSpread1_Sheet1.GetText(i, 0) == "") continue;
                if (Neusoft.FrameWork.Function.NConvert.ToInt32(this.neuSpread1_Sheet1.Cells[i, 1].Text) > maxInjectOrder)
                {
                    maxInjectOrder = Neusoft.FrameWork.Function.NConvert.ToInt32(this.neuSpread1_Sheet1.Cells[i, 1].Text);
                }
            }
            return maxInjectOrder;
        }

        /// <summary>
        /// ���ò�����Ϣ
        /// </summary>
        /// <param name="reg"></param>
        private void SetPatient(Neusoft.HISFC.Models.Registration.Register reg)
        {

            if (reg == null || reg.ID == "")
            {
                return;
            }
            else
            {
                int iAge = Neusoft.FrameWork.Function.NConvert.ToInt32(System.DateTime.Now.ToString("yyyy"))
                    - Neusoft.FrameWork.Function.NConvert.ToInt32(reg.Birthday.ToString("yyyy"));
                this.txtName.Text = reg.Name;
                this.txtSex.Text = reg.Sex.Name;
                this.txtBirthday.Text = reg.Birthday.ToString("yyyy-MM-dd");
                this.txtAge.Text = this.InjMgr.GetAge(reg.Birthday);//iAge.ToString();
                this.txtCardNo.Text = reg.PID.CardNO;
            }
        }

        /// <summary>
        /// ȫѡ
        /// </summary>
        private void SelectAll(bool isSelected)
        {
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                //{FAC1693A-3EBA-44b3-A1E3-6D6750A98D80}
                //this.neuSpread1_Sheet1.SetValue(i, 0, isSelected, false);
                this.neuSpread1_Sheet1.Cells[i, 0].Value = isSelected;
            }
        }
		

        #endregion

        #region  ��ӡ
        /// <summary>
        /// ��ӡ���߿�
        /// </summary>
        private void PrintPatient()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPatientPrint patientPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPatientPrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPatientPrint;
            if (patientPrint == null)
            {
                patientPrint = new Nurse.Print.ucPrintPatient() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPatientPrint;
                //Nurse.Print.ucPrintPatient uc = new Nurse.Print.ucPrintPatient();
            }
            patientPrint.Init(al);
        }
        /// <summary>
        /// ��ӡƿǩ
        /// </summary>
        private void PrintCure()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            //			this.maxInjectOrder = 0;
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectCurePrint curePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectCurePrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectCurePrint;
            if (curePrint == null)
            {
                curePrint = new Nurse.Print.ucPrintCure() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectCurePrint;
                //Nurse.Print.ucPrintCure uc = new Nurse.Print.ucPrintCure();
            }
            curePrint.Init(al);
            //			uc.Init(alJiePing);
        }
        /// <summary>
        /// ��ӡע�䵥.
        /// </summary>
        private void PrintInject()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            //			this.maxInjectOrder = 0;
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPrint injectPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPrint;
            if (injectPrint == null)
            {
                injectPrint = new Nurse.Print.ucPrintInject() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectPrint;
                //Nurse.Print.ucPrintCure uc = new Nurse.Print.ucPrintCure();
            }
            //			if(alJiePing.Count > 0 )
            //			{
            //				al.AddRange(alJiePing);
            //			}
            injectPrint.Init(al);
        }
        /// <summary>
        /// ��ӡǩ����
        /// </summary>
        private void PrintItinerate()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            //			this.maxInjectOrder = 0;
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint itineratePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint;
            if (itineratePrint == null)
            {
                itineratePrint = new Nurse.Print.ucPrintItinerate() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint;
                //Nurse.Print.ucPrintItinerate uc = new Nurse.Print.ucPrintItinerate();
            }
            itineratePrint.Init(al);
        }
        /// <summary>
        /// ����ֽ�ŵ�ǩ����
        /// </summary>
        private void PrintItinerateLarge()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            //			this.maxInjectOrder = 0;
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint itineratePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint;
            if (itineratePrint == null)
            {
                itineratePrint = new Nurse.Print.ucPrintItinerateLarge() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectItineratePrint;
                //Nurse.Print.ucPrintItinerate uc = new Nurse.Print.ucPrintItinerate();
            }
            //			if(alJiePing.Count > 0 )
            //			{
            //				al.AddRange(alJiePing);
            //			}
            itineratePrint.Init(al);
        }

        /// <summary>
        /// {30E1EF7D-1236-4e38-A8E3-7567C9E33B0B} ���Ӻ�����
        /// </summary>
        private void PrintNumber()
        {
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
            ArrayList al = this.GetPrintInjectList();
            //			this.maxInjectOrder = 0;
            if (al == null || al.Count <= 0)
            {
                MessageBox.Show("û��ѡ������!");
                return;
            }
            //{637EDB0D-3F39-4fde-8686-F3CD87B64581} ��ӡ��Ϊ�ӿڷ�ʽ
            Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectNumberPrint numberPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectNumberPrint)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectNumberPrint;
            if (numberPrint == null)
            {
                numberPrint = new Nurse.Print.ucPrintNumber() as Neusoft.HISFC.BizProcess.Interface.Nurse.IInjectNumberPrint;
                //Nurse.Print.ucPrintNumber uc = new Nurse.Print.ucPrintNumber();
            }
            numberPrint.Init(al);
        }

        /// <summary>
        /// ��ȡҪ��ӡ������
        /// {30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
        /// </summary>
        /// <returns></returns>
        private ArrayList GetPrintInjectList()
        {
            ArrayList al = new ArrayList();
            ArrayList alJiePing = new ArrayList();
            this.neuSpread1.StopCellEditing();
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                if (this.neuSpread1_Sheet1.GetValue(i, 0).ToString().ToUpper() == "FALSE")
                    continue;
                Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList detail =
                    (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                Neusoft.HISFC.Models.Order.OutPatient.Order orderinfo =
                    (Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1_Sheet1.Cells[i, 11].Tag;
                Neusoft.HISFC.Models.Nurse.Inject info = new Neusoft.HISFC.Models.Nurse.Inject();

                info.Patient.Name = reg.Name;
                info.Patient.Sex.ID = reg.Sex.ID;
                info.Patient.Birthday = reg.Birthday;
              
                //zhangyt 2011-02-27  ������
                info.Patient.PID.CardNO = reg.PID.CardNO;

                info.Patient.Name = reg.Name;
                info.Item = detail;
                info.Item.InjectCount = Neusoft.FrameWork.Function.NConvert.ToInt32(this.neuSpread1_Sheet1.Cells[i, 2].Text);
                info.OrderNO = this.txtOrder.Text.ToString();
                info.Item.Order.Combo.ID = this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString();
                //ҽ������
                Neusoft.HISFC.Models.Base.Employee ps = this.PsMgr.GetEmployeeInfo(detail.RecipeOper.ID);
                info.Item.Order.Doctor.Name = ps.Name;
                info.Item.Order.Doctor.ID = ps.ID;
                info.Item.Name = detail.Item.Name;
                string strOrder = "";
                if (this.neuSpread1_Sheet1.GetValue(i, 1) == null || this.neuSpread1_Sheet1.GetValue(i, 1).ToString() == "")
                {
                    strOrder = "";
                }
                else
                {
                    strOrder = this.neuSpread1_Sheet1.GetValue(i, 1).ToString();
                }
                info.InjectOrder = strOrder;
                al.Add(info);
                //�жϽ�ƿ,���������ӵ�alJiePing��
                if (orderinfo.ExtendFlag1 == null || orderinfo.ExtendFlag1.Length < 1)
                    orderinfo.ExtendFlag1 = "1|";
                //				string[] str = orderinfo.Mark1.Split('|');
                int inum = Neusoft.FrameWork.Function.NConvert.ToInt32(orderinfo.ExtendFlag1.Substring(0, 1));
                info.Memo = inum.ToString();
                //neusoft.neNeusoft.HISFC.Components.Function.NConvert.ToInt32(str[0]);
                //				if(inum > 1)
                //				{
                //					for(int m = 1 ; m < inum ; m++ )
                //					{
                //						Neusoft.HISFC.Models.Nurse.Inject inj = new Neusoft.HISFC.Models.Nurse.Inject();
                //						inj = info.Clone();
                //						inj.InjectOrder = (this.GetMaxInjectOrder() + 1).ToString();
                //						maxInjectOrder++;
                //						alJiePing.Add(inj);
                //					}
                //				}

                //{EB016FFE-0980-479c-879E-225462ECA6D0}
                info.PrintNo = detail.User02;
            }
            return al;
        }

        /// <summary>
        /// ��ȡ�����ȵ�ʹ�÷���
        /// </summary>
        /// <param name="IsInit">�Ƿ��ʼ��</param>
        /// <returns></returns>
        private Neusoft.HISFC.Models.Base.Const GetFirstUsage()
        {
            Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList info = new Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList();
            if (this.neuSpread1_Sheet1.RowCount <= 0) return new Neusoft.HISFC.Models.Base.Const();

            int FirstCodeNum = 10000;
            Neusoft.HISFC.Models.Base.Const retobj = new Neusoft.HISFC.Models.Base.Const();
            try
            {
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    info = (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                    Neusoft.FrameWork.Models.NeuObject obj = this.conMgr.GetConstant("SPECIAL", info.Order.Usage.ID);
                    Neusoft.HISFC.Models.Base.Const conobj = (Neusoft.HISFC.Models.Base.Const)obj;

                    if (conobj.SortID < FirstCodeNum)
                    {
                        FirstCodeNum = conobj.SortID;
                        retobj = conobj;
                    }
                }
            }
            catch
            {
                return retobj;
            }

            return retobj;
        }
        #endregion	

        #region ע��˳��ŵĴ���
        /// <summary>
        /// ����Ĭ��ע��˳��
        /// </summary>
        private void SetInject()
        {

            #region  û�����ݾͲ�����,ֱ�ӷ���
            if (this.neuSpread1_Sheet1.RowCount <= 0) return;
            #endregion

            #region ���û��߽����ע��˳���
            if (this.chkIsOrder.Checked)
            {
                this.SetOrder();
            }
            else
            {
                this.txtOrder.Text = "0";
                //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
                for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                {
                    this.neuSpread1_Sheet1.Cells[i, 14].Text = this.txtOrder.Text;
                }
            }
            #endregion

            #region ����ÿ����Ŀ��ע��˳��
            int InjectOrder = 1;
            this.neuSpread1_Sheet1.SetValue(0, 1, 1, false);
            for (int i = 1; i < this.neuSpread1_Sheet1.RowCount; i++)
            {

                if (this.neuSpread1_Sheet1.Cells[i, 7].Text == null || this.neuSpread1_Sheet1.Cells[i, 7].Text.Trim() == "")
                {
                    InjectOrder++;
                    this.neuSpread1_Sheet1.SetValue(i, 1, InjectOrder, false);
                }
                else if (this.neuSpread1_Sheet1.Cells[i, 7].Text != null && this.neuSpread1_Sheet1.Cells[i, 7].Text.Trim() != ""
                    //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                    && this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[i, 13].Text == this.neuSpread1_Sheet1.Cells[i - 1, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[i - 1, 13].Text)
                {
                    this.neuSpread1_Sheet1.SetValue(i, 1, InjectOrder, false);
                }
                else
                {
                    InjectOrder++;
                    this.neuSpread1_Sheet1.SetValue(i, 1, InjectOrder, false);
                }
            }
            #endregion

        }

        /// <summary>
        /// ��ʼ��ע��˳���
        /// </summary>
        private void InitOrder()
        {
            //��ȡ�Ƿ��Զ�����ע��˳��
            try
            {
                bool isAutoInjectOrder = false;
                isAutoInjectOrder = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.conMgr.QueryControlerInfo("900005"));
                if (isAutoInjectOrder)
                {
                    this.chkIsOrder.Checked = true;
                    this.SetOrder();
                    this.lbLastOrder.Text = "�������һ��ע���:" +
                        (Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtOrder.Text.Trim()) - 1).ToString();
                }
                else
                {
                    this.chkIsOrder.Checked = false;
                    this.lbLastOrder.Text = "�������Զ�����ע��˳���!";
                    this.txtOrder.Text = "0";
                }
                //XmlDocument doc = new XmlDocument();
                //doc.Load(Application.StartupPath + "/Setting/NurseSetting.xml");
                //XmlNode node = doc.SelectSingleNode("//�Ƿ�����ע��˳��");

                //if (node != null && node.Attributes["isAutoInjectOrder"].Value.ToString() == "false")
                //{
                //    this.chkIsOrder.Checked = false;
                //    this.lbLastOrder.Text = "�������Զ�����ע��˳���!";
                //    this.txtOrder.Text = "0";
                //}
                //else
                //{
                //    this.chkIsOrder.Checked = true;
                //    this.SetOrder();
                //    this.lbLastOrder.Text = "�������һ��ע���:" +
                //        (Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtOrder.Text.Trim()) - 1).ToString();
                //}
            }
            catch //�������ļ�
            {
                this.chkIsOrder.Checked = false;
                this.lbLastOrder.Text = "�������Զ�����ע��˳���!";
                this.txtOrder.Text = "0";
            }


        }
        /// <summary>
        /// ����ע���
        /// </summary>
        private void SetOrder()
        {
            if (!this.chkIsOrder.Checked)
            {
                this.txtOrder.Text = "0";
                this.lbLastOrder.Text = "���ڱ���û�������Զ��������!";
                return;
            }
            //����Զ�����,���õ�һ�����,����ֵthis.currentOrder
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B} ��Ϊͨ���ӿ�ʵ�֣����û��������ԭ�����������
            this.CreateInterface();
            if (IGetOrderNo != null)
            {
                string orderNo = IGetOrderNo.GetOrderNo(this.reg);
                this.txtOrder.Text = orderNo;
                if (this.neuSpread1_Sheet1.Rows.Count == 0)
                {
                    return;
                }
                string comboAndInjectTime = this.neuSpread1_Sheet1.Cells[0, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[0, 13].Text;
                for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                {
                    string rowComboAndInjectTime = this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[i, 13].Text;
                    if (comboAndInjectTime != rowComboAndInjectTime)
                    {
                        comboAndInjectTime = rowComboAndInjectTime;
                        orderNo = IGetOrderNo.GetSamePatientNextOrderNo(orderNo);
                    }
                    this.neuSpread1_Sheet1.Cells[i, 14].Text = orderNo;
                }
                return;
            }
            else
            {
                Neusoft.HISFC.Models.Nurse.Inject info = this.InjMgr.QueryLast();
                if (info != null && info.Booker.OperTime != System.DateTime.MinValue)
                {
                    if (info.Booker.OperTime.ToString("yyyy-MM-dd")
                        == this.InjMgr.GetDateTimeFromSysDateTime().ToString("yyyy-MM-dd"))
                    {
                        this.txtOrder.Text = (Neusoft.FrameWork.Function.NConvert.ToInt32(info.OrderNO) + 1).ToString();
                    }
                    else
                    {
                        this.txtOrder.Text = "1";
                    }
                }
                else
                {
                    this.txtOrder.Text = "1";
                }
                //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
                for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                {
                    this.neuSpread1_Sheet1.Cells[i, 14].Text = this.txtOrder.Text;
                }
            }
        }

        /// <summary>
        /// �����ӿ�
        /// {30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
        /// </summary>
        private void CreateInterface()
        {
            if (this.IGetOrderNo == null)
            {
                this.IGetOrderNo = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Nurse.IGetInjectOrderNo)) as Neusoft.HISFC.BizProcess.Interface.Nurse.IGetInjectOrderNo;
            }
        }
        #endregion

        #region ����

        /// <summary>
        /// ȷ�ϱ���( 1.met_nuo_inject�����¼  2.fin_ipb_feeitemlist������ȷ��Ժע������ȷ�ϱ�־)
        /// </summary>
        private int Save()
        {
            if (this.neuSpread1_Sheet1.Rows.Count <= 0)
            {
                MessageBox.Show("û��Ҫ���������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            
            this.neuSpread1.StopCellEditing();
            int selectNum = 0;
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                if (this.neuSpread1_Sheet1.GetValue(i, 0).ToString().ToUpper() == "FALSE" || this.neuSpread1_Sheet1.GetValue(i, 0).ToString() == "")
                {
                    selectNum++;
                }
            }
            if (selectNum >= this.neuSpread1_Sheet1.RowCount)
            {
                MessageBox.Show("��ѡ��Ҫ���������", "��ʾ");
                return -1;
            }
            alInject = new ArrayList();
            alPrint = new ArrayList();
            #region �ж�������кŵ���Ч��
            if (this.txtOrder.Text == null || this.txtOrder.Text.Trim().ToString() == "")
            {
                MessageBox.Show("û���������˳���!");
                this.txtOrder.Focus();
                return -1;
            }
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}ͨ���ӿڴ�ӡ˳��ţ���У������
            //else if (this.IsNum(this.txtOrder.Text.Trim().ToString()) == false)
            //{
            //    MessageBox.Show("����˳��ű���Ϊ����!");
            //    this.txtOrder.Focus();
            //    return -1;
            //}
            else if (this.InjMgr.QueryInjectOrder(this.txtOrder.Text.Trim().ToString()).Count > 0)
            {
                if (MessageBox.Show("�ö��к��Ѿ�ʹ��,�Ƿ����!", "��ʾ", System.Windows.Forms.MessageBoxButtons.YesNo,
                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    this.txtOrder.Focus();
                    return -1;
                }
            }
            //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}ͨ���ӿڴ�ӡ˳��ţ���У������
            //if (Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtOrder.Text) <= 0)
            //{
            //    MessageBox.Show("����˳��ű�����ڣ�!");
            //    this.txtOrder.Focus();
            //    return -1;
            //}
            #endregion

            #region ���ע��˳��ŵ���Ч�ԣ������ͬ�ģ�ע��˳���Ҳ������ͬ��
            for (int i = 1; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                if (this.neuSpread1_Sheet1.Cells[i, 7].Tag != null && this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() != "" &&
                    //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                    this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[i, 13].Text == this.neuSpread1_Sheet1.Cells[i - 1, 7].Tag.ToString() + this.neuSpread1_Sheet1.Cells[i - 1, 13].Text
                    && this.neuSpread1_Sheet1.GetValue(i, 1).ToString() != this.neuSpread1_Sheet1.GetValue(i - 1, 1).ToString()
                    )
                {
                    MessageBox.Show("��ͬ��ŵ�ע��˳��ű�����ͬ!", "��" + (i + 1).ToString() + "��");
                    return -1;
                }
            }
            #endregion

            #region ���Ժע��������Ч�ԣ������ͬ�ģ�ע��˳���Ҳ������ͬ��
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                string strnum = this.neuSpread1_Sheet1.Cells[i, 2].Text;
                if (strnum == null || strnum == "")
                {
                    MessageBox.Show("Ժע��������Ϊ��!", "��" + (i + 1).ToString() + "��");
                    return -1;
                }
                if (!this.IsNum(strnum))
                {
                    MessageBox.Show("Ժע��������Ϊ����!", "��" + (i + 1).ToString() + "��");
                    return -1;
                }
                string completenum = this.neuSpread1_Sheet1.Cells[i, 3].Text;
                if (this.neuSpread1_Sheet1.GetValue(i, 0).ToString().ToUpper() == "TRUE")
                {
                    if (Neusoft.FrameWork.Function.NConvert.ToInt32(strnum) <= Neusoft.FrameWork.Function.NConvert.ToInt32(completenum))
                    {
                        MessageBox.Show("Ժע��������!", "��" + (i + 1).ToString() + "��");
                        return -1;
                    }
                }
                //				if(this.fpSpread1_Sheet1.Cells[i,7].Tag != null && this.fpSpread1_Sheet1.Cells[i,7].Tag.ToString() != "" &&
                //					this.fpSpread1_Sheet1.Cells[i,7].Tag.ToString() == this.fpSpread1_Sheet1.Cells[i-1,7].Tag.ToString()
                //					&& this.fpSpread1_Sheet1.GetValue(i,2).ToString() != this.fpSpread1_Sheet1.GetValue(i-1,2).ToString()
                //					)
                //				{
                //					MessageBox.Show("��ͬ��ŵ�Ժע����������ͬ!","��"+ (i+1).ToString() +"��");
                //					return -1;
                //				}
            }
            #endregion

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction SQLCA = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //SQLCA.BeginTransaction();

            try
            {
                this.InjMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                this.patientMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                this.drugMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                this.DeptMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                this.PsMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                DateTime confirmDate = this.InjMgr.GetDateTimeFromSysDateTime();

                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    if (this.neuSpread1_Sheet1.GetText(i, 0).ToUpper() == "FALSE" ||
                        this.neuSpread1_Sheet1.GetText(i, 0) == "") continue;

                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList detail =
                        (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;

                    //					#region �ж��Ƿ���Ҫ��ӡע�䵥
                    //					if(detail.ConfirmedInject == 0)
                    //					{
                    //						IsFirstTime = true;
                    //						countInject = detail.InjectCount;
                    //					}
                    //					#endregion

                    Neusoft.HISFC.Models.Nurse.Inject info = new Neusoft.HISFC.Models.Nurse.Inject();
                    #region ʵ��ת����������Ŀ�շ���ϸʵ��FeeItemList��->ע��ʵ��Inject��
                    info.Patient.Name = reg.Name;
                    info.Patient.Sex.ID = reg.Sex.ID;
                    info.Patient.Birthday = reg.Birthday;
                    info.Patient.PID.CardNO= reg.PID.CardNO;

                    //info.Item = (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)detail.Item;
                    info.Item = detail;
                    info.Item.ID = detail.Item.ID;
                    info.Item.Name = detail.Item.Name;

                    //info.Item.Item.MinFee.ID = detail.Item.MinFee.ID;
                    //info.Item.Item.Price = detail.Item.Price;
                    
               

                    

                    info.Item.InjectCount = Neusoft.FrameWork.Function.NConvert.ToInt32(this.neuSpread1_Sheet1.Cells[i, 2].Text);
                    //������������
                    //Neusoft.HISFC.Models.Base.Department dept = this.DeptMgr.GetDepartment(detail.Order.DoctorDept.ID);
                    Neusoft.HISFC.Models.Base.Department dept = this.DeptMgr.GetDepartment(detail.RecipeOper.Dept.ID);
                    info.Item.Order.DoctorDept.Name = dept.Name;
                    info.Item.Order.DoctorDept.ID = dept.ID;
                    //ҽ������
                    Neusoft.HISFC.Models.Base.Employee ps = this.PsMgr.GetEmployeeInfo(detail.RecipeOper.ID);
                    info.Item.Order.Doctor.Name = ps.Name;
                    info.Item.Order.Doctor.ID = ps.ID;
                    //�Ƿ�Ƥ��
                    if (this.neuSpread1_Sheet1.Cells[i, 11].Tag.ToString().ToUpper() == "TRUE")
                    {
                        info.Hypotest = "1";
                    }
                    else
                    {
                        info.Hypotest = "0";
                    }
                    #endregion

                    info.ID = this.InjMgr.GetSequence("Nurse.Inject.GetSeq");
                    //{30E1EF7D-1236-4e38-A8E3-7567C9E33B0B}
                    //info.OrderNO = this.txtOrder.Text.ToString();
                    info.OrderNO = this.neuSpread1_Sheet1.Cells[i, 14].Text;
                    //{24A47206-F111-4817-A7B4-353C21FC7724}
                    info.PrintNo = detail.User02;
                    info.Item.Order.Combo.ID = this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString();
                    info.Booker.ID = Neusoft.FrameWork.Management.Connection.Operator.ID;
                    info.Booker.OperTime = confirmDate;
                    info.Item.ExecOper.ID = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;
                    string strOrder = "";
                    if (this.neuSpread1_Sheet1.GetValue(i, 1) == null || this.neuSpread1_Sheet1.GetValue(i, 1).ToString() == "")
                    {
                        strOrder = "";
                    }
                    else
                    {
                        strOrder = this.neuSpread1_Sheet1.GetValue(i, 1).ToString();
                    }
                    info.InjectOrder = strOrder;

                    //��ע--(ȡҽ����ע)
                    Neusoft.HISFC.Models.Order.OutPatient.Order orderinfo =
                        (Neusoft.HISFC.Models.Order.OutPatient.Order)this.neuSpread1_Sheet1.Cells[i, 11].Tag;
                    if (orderinfo != null)
                    {
                        info.Memo = orderinfo.ExtendFlag1;
                    }

                    #region ��met_nuo_inject�У������¼
                    if (this.InjMgr.Insert(info) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(this.InjMgr.Err, "��ʾ");
                        return -1;
                    }
                    #endregion

                    #region ��fin_ipb_feeitemlist�У���������

                    if (this.patientMgr.UpdateConfirmInject(detail.Order.ID, detail.RecipeNO, detail.SequenceNO.ToString(), 1) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(this.patientMgr.Err, "��ʾ");
                        return -1;
                    }
                    #endregion
                    info.Item.InjectCount = info.Item.InjectCount;
                    //���ƿ�ĲŴ�ӡ���Ƶ�---��д��-------------�˶γ�����,��Ϊ�ɲ���Աѡ���Ƿ��ӡ
                    if (info.Item.Order.Usage.ID == "03" || info.Item.Order.Usage.ID == "04")
                    {
                        alPrint.Add(info);
                    }
                    alInject.Add(info);
                    this.lbLastOrder.Text = "�������һ��ע���:" + info.OrderNO;

                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();

            }
            catch (Exception e)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(e.Message, "��ʾ");
                return -1;
            }

            MessageBox.Show("����ɹ�!", "��ʾ");
            this.Clear();
            this.lbCue.Text = "";

            this.txtCardNo.SelectAll();
            this.txtRecipe.Text = "";
            this.txtCardNo.Text = "";
            this.txtCardNo.Focus();
            return 0;
        }
        /// <summary>
        /// ���
        /// </summary>
        private void Clear()
        {
            if (this.neuSpread1_Sheet1.RowCount > 0)
                this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);
            this.txtOrder.Text = "";
            this.txtRecipe.Text = "";
        }
        /// <summary>
        /// ��ѯ
        /// </summary>
        private void Query()
        {
            //this.Clear();
            if (this.neuSpread1_Sheet1.RowCount > 0)
                this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);
            string cardNo;
            cardNo = this.txtCardNo.Text.Trim().PadLeft(10, '0');
            //��ȡҽ�������Ĵ�����Ϣ��û��ȫ��ִ����ģ�
            DateTime dtFrom = this.dtpStart.Value.Date;
            DateTime dtTo = this.dtpEnd.Value.Date.AddDays(1);

            if (/*this.txtRecipe.Text == null ||*/ this.txtRecipe.Text.Trim() == "")
            {
                al = this.patientMgr.QueryOutpatientFeeItemListsZs(cardNo, dtFrom, dtTo);

                //al = this.patientMgr.QueryOutpatientFeeItemLists(cardNo, dtFrom, dtTo);
            }
            else
            {
                al = this.patientMgr.QueryFeeDetailFromRecipeNO(this.txtRecipe.Text.Trim());

                if (al == null)
                {
                    return;
                }

                if (al.Count > 0)
                {
                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList item = (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)al[0];
                    reg = this.regMgr.GetByClinic(item.Patient.ID);
                    if (reg == null || reg.ID == "")
                    {
                        MessageBox.Show("û�в�����Ϊ:" + item.Patient.ID + "�Ļ���!", "��ʾ");

                        this.txtCardNo.Focus();
                        return;
                    }

                    this.txtCardNo.Text = item.Patient.PID.CardNO;
                    this.SetPatient(reg);
                    this.txtRecipe.Text = "";
                    this.Query();
                    return;
                }

            }

            this.Query(al);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void Query(ArrayList al)
        {
            if (al == null || al.Count == 0)
            {
                MessageBox.Show("�û���û����Ҫȷ�ϵ�ҽ����Ϣ!", "��ʾ");
                this.txtCardNo.Focus();
                return;
            }
            this.AddDetail(al);
            if (this.neuSpread1_Sheet1.RowCount <= 0)
            {
                MessageBox.Show("��ʱ�����û�иû�����Ϣ!", "��ʾ");
                this.txtCardNo.Focus();
                return;
            }
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                int confirmNum = Neusoft.FrameWork.Function.NConvert.ToInt32(this.neuSpread1_Sheet1.Cells[i, 3].Text);
                if (confirmNum == 0)
                {
                    this.lbCue.Text = "�״�Ժע,��˶�Ժע����!";
                    this.lbCue.ForeColor = System.Drawing.Color.Magenta;
                }
            }
            this.SelectAll(true);
            this.SetComb();

            #region �˴����һ��ҽ����ע
            if (this.neuSpread1_Sheet1.RowCount > 0)
            {
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList info
                        = (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList)this.neuSpread1_Sheet1.Rows[i].Tag;
                    //
                    Neusoft.HISFC.BizProcess.Integrate.Order orderMgr = new Neusoft.HISFC.BizProcess.Integrate.Order();
                    Neusoft.HISFC.Models.Order.OutPatient.Order orderinfo = new Neusoft.HISFC.Models.Order.OutPatient.Order();

                    orderinfo = orderMgr.GetOneOrder(info.Order.ID);
                    if (orderinfo != null && orderinfo.Memo != null)
                    {
                        this.neuSpread1_Sheet1.SetText(i, 12, orderinfo.Memo);
                        string strHypoTest = "";
                        if (orderinfo.HypoTest == 1)
                        {
                            strHypoTest = "��";
                        }
                        else if (orderinfo.HypoTest == 2)
                        {
                            strHypoTest = "��";
                        }
                        else if (orderinfo.HypoTest == 3)
                        {
                            strHypoTest = "����";
                        }
                        else if (orderinfo.HypoTest == 4)
                        {
                            strHypoTest = "����";
                        }
                        this.neuSpread1_Sheet1.Cells[i, 11].Text = strHypoTest;
                        this.neuSpread1_Sheet1.Cells[i, 11].Tag = orderinfo;
                    }
                    else
                    {
                        this.neuSpread1_Sheet1.Cells[i, 11].Tag = new Neusoft.HISFC.Models.Order.OutPatient.Order();
                    }
                }
            #endregion

            }

            this.SetFP();
            this.ShowColor();
            this.txtOrder.Focus();
        }
        /// <summary>
        /// �����Ŀ��ϸ
        /// </summary>
        /// <param name="detail"></param>
        private void AddDetail(ArrayList details)
        {
            if (this.neuSpread1_Sheet1.RowCount > 0) this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);
            //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
            List<Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList> tmpFeeList = new List<Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList>();
            if (details != null)
            {
                foreach (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList detail in al)
                {
                    #region  ��ҩƷ����ʾ
                    //��ҩƷ����ʾ
                    //if (detail.Item.IsPharmacy == false) continue;
                    if (detail.Item.ItemType != HISFC.Models.Base.EnumItemType.Drug) continue;
                    #endregion

                    #region ����Ժע�Ĳ���ʾ
                    //����Ժע�Ĳ���ʾ-------Ȩ��֮��
                    Neusoft.HISFC.BizProcess.Integrate.Manager con = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                    Neusoft.FrameWork.Models.NeuObject obj = con.GetConstant("SPECIAL", detail.Order.Usage.ID);
                    if (obj == null || obj.ID == null || obj.ID == "")
                    {
                        continue;
                    }
                    //					neusoft.neNeusoft.HISFC.Components.Object.neuObject obj = con.Get(Neusoft.HISFC.Models.Base.enuConstant.USAGE,detail.UsageInfo.ID);
                    //					if(obj.Memo == null ||obj.Memo == "" || obj.Memo != this.Tag.ToString().Trim()) continue;
                    #endregion

                    #region  Ժע���� <= ��ȷ��Ժע���� �Ĳ���ʾ

                    //Ժע���� <= ��ȷ��Ժע���� �Ĳ���ʾ
                    if (detail.InjectCount != 0 && detail.InjectCount <= detail.ConfirmedInjectCount && !this.cbFinish.Checked)
                    {
                        continue;
                    }
                    #endregion

                    #region �Ƿ���ʾ0������
                    if (!chkNullNum.Checked && detail.InjectCount == 0)
                    {
                        continue;
                    }
                    #endregion

                    #region ֻ��ʾ���һ������
                    //					if(this.chkLast.Checked )
                    //					{
                    //						this.ShowLastOnly();
                    //					}
                    #endregion

                    #region �����Ѿ��Ǽǵ�QD����ʾ��ע�����ε�BID����ʾ����ǰ���ע��һ�ε�BID������ʾ��(���ݽ���ĵǼ�ʱ��)
                    DateTime dt = Neusoft.FrameWork.Function.NConvert.ToDateTime(
                        this.InjMgr.GetDateTimeFromSysDateTime().ToString("yyyy-MM-dd 00:00:00"));
                    //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                    ArrayList alTodayInject = this.InjMgr.Query(detail.Patient.PID.CardNO, detail.RecipeNO, detail.SequenceNO.ToString(), dt);
                    Neusoft.HISFC.Models.Order.Frequency frequence = this.dicFrequency[detail.Order.Frequency.ID];
                    string[] injectTime = frequence.Time.Split('-');
                    //������Ѿ�ȫ��ע����Ϻ�����
                    if (alTodayInject.Count >= injectTime.Length)
                    {
                        continue;
                    }
                    if (this.isShowAllInject)
                    {
                        for (int i = alTodayInject.Count; i < injectTime.Length; i++)
                        {
                            Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList newDetail = detail.Clone();
                            newDetail.User03 = injectTime[i];
                            tmpFeeList.Add(newDetail);
                        }
                    }
                    else
                    {
                        //δ���ϴ�ע��ʱ��Ļ��������ٴεǼ�
                        if (alTodayInject.Count > 0)
                        {
                            DateTime lastInjectTime = FrameWork.Function.NConvert.ToDateTime(dt.ToString("yyyy-MM-dd ") + injectTime[alTodayInject.Count - 1] + ":00");
                            if (this.InjMgr.GetDateTimeFromSysDateTime() < lastInjectTime)
                            {
                                continue;
                            }
                        }
                        detail.User03 = injectTime[alTodayInject.Count];
                        tmpFeeList.Add(detail);
                    }
                    //if (detail.Order.Frequency.ID == "QD")
                    //{
                    //    ArrayList alTemp = this.InjMgr.Query(detail.Patient.PID.CardNO, detail.RecipeNO,
                    //        detail.SequenceNO.ToString(), dt);
                    //    if (alTemp != null)
                    //    {
                    //        if (alTemp.Count > 0) continue;
                    //    }
                    //}
                    //if (detail.Order.Frequency.ID == "BID")
                    //{
                    //    ArrayList alTemp = this.InjMgr.Query(detail.Patient.PID.CardNO, detail.RecipeNO,
                    //        detail.SequenceNO.ToString(), dt);
                    //    if (alTemp != null && alTemp.Count > 0)
                    //    {
                    //        if (alTemp.Count >= 2) continue;
                    //        //��ǰ���ע��һ�ε�BID������ʾ
                    //        Neusoft.HISFC.Models.Nurse.Inject item = (Neusoft.HISFC.Models.Nurse.Inject)alTemp[0];
                    //        bool bl1 = true;
                    //        bool bl2 = true;
                    //        if (Neusoft.FrameWork.Function.NConvert.ToInt32(item.Booker.OperTime.ToString("HH")) > 12) bl1 = false;
                    //        if (Neusoft.FrameWork.Function.NConvert.ToInt32(this.InjMgr.GetDateTimeFromSysDateTime().ToString("HH")) > 12) bl2 = false;
                    //        if (bl1 == bl2) continue;
                    //    }
                    //}
                    #endregion
                    //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                    //this.AddDetail(detail);
                }
                //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                //����
                tmpFeeList.Sort(new FeeItemListSort());
                //��ȡ��ӡ���
                this.CreateInterface();
                if (this.IGetOrderNo != null)
                {
                    this.IGetOrderNo.SetPrintNo(new ArrayList(tmpFeeList.ToArray()));
                }
                foreach (Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeItem in tmpFeeList)
                {
                    this.AddDetail(feeItem);
                }

                if (this.neuSpread1_Sheet1.RowCount > 0)
                {
                    this.LessShow();
                }
            }
            #region {A9925B9E-1918-461e-BEFE-3104E86E0B4F} δ�����Ʒѵ�ҽ����ʾ������ɫ
            for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
            {
                if (this.neuSpread1_Sheet1.Cells[i, 2].Text == "0")
                {
                    this.neuSpread1_Sheet1.Rows[i].ForeColor = Color.Blue;
                }
            }
            #endregion
        }
        /// <summary>
        /// �����ϸ
        /// </summary>
        /// <param name="detail"></param>
        private void AddDetail(Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList info)
        {
            this.neuSpread1_Sheet1.Rows.Add(this.neuSpread1_Sheet1.RowCount, 1);
            int row = this.neuSpread1_Sheet1.RowCount - 1;
            this.neuSpread1_Sheet1.Rows[row].Tag = info;

            #region "���ڸ�ֵ"
            #region ��ȡƤ����Ϣ
            //��ȡƤ����Ϣ
           // Neusoft.HISFC.Models.Pharmacy.Item drug = this.drugMgr.GetItem(info.ID);
            Neusoft.HISFC.Models.Pharmacy.Item drug = this.drugMgr.GetItem(info.Item.ID);
            if (drug == null)
            {
                MessageBox.Show("��ȡҩƷƤ����Ϣʧ��!");
                this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);
                return;
            }
            string strTest = "��";
            if (drug.IsAllergy)
            {
                strTest = "��";
            }
            //
            #endregion
            Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department(); 
               dept = this.DeptMgr.GetDepartment(info.RecipeOper.Dept.ID);
            info.Order.DoctorDept.Name = dept.Name;

            this.neuSpread1_Sheet1.SetValue(row, 1, "", false);//ע��˳���
            this.neuSpread1_Sheet1.SetValue(row, 2, info.InjectCount.ToString(), false);//Ժע����
            this.neuSpread1_Sheet1.SetValue(row, 3, info.ConfirmedInjectCount.ToString(), false);//�Ѿ�ȷ�ϵ�Ժע����
            this.neuSpread1_Sheet1.SetValue(row, 4, this.GetDoctByID(info.RecipeOper.ID), false);//����ҽ��
            this.neuSpread1_Sheet1.Cells[row, 4].Tag = info.Order.Doctor.ID;
            this.neuSpread1_Sheet1.SetValue(row, 5, dept.Name, false);//�Ʊ�
            this.neuSpread1_Sheet1.Cells[row, 5].Tag = info.Order.DoctorDept.ID;
            this.neuSpread1_Sheet1.SetValue(row, 6, info.Item.Name, false);//ҩƷ����
            this.neuSpread1_Sheet1.Cells[row, 7].Tag = info.Order.Combo.ID;//��Ϻ�
            this.neuSpread1_Sheet1.SetValue(row, 8, info.Order.DoseOnce.ToString() + info.Order.DoseUnit, false);//ÿ����
            this.neuSpread1_Sheet1.SetValue(row, 9, info.Order.Frequency.ID, false);//Ƶ��
            this.neuSpread1_Sheet1.Cells[row, 9].Tag = info.Order.Frequency.ID.ToString();
            this.neuSpread1_Sheet1.SetValue(row, 10, info.Order.Usage.Name, false);//�÷�
            this.neuSpread1_Sheet1.SetValue(row, 11, strTest, false);//Ƥ�ԣ�
            this.neuSpread1_Sheet1.Cells[row, 11].Tag = drug.IsAllergy.ToString().ToUpper();
            //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
            this.neuSpread1_Sheet1.Cells[row, 13].Text = info.User03;
            #endregion
        }
        /// <summary>
        /// ������Ϻ�
        /// </summary>
        private void SetComb()
        {
            int myCount = this.neuSpread1_Sheet1.RowCount;
            int i;
            //��һ��
            this.neuSpread1_Sheet1.SetValue(0, 7, "��");
            //�����
            this.neuSpread1_Sheet1.SetValue(myCount - 1, 7, "��");
            //�м���
            for (i = 1; i < myCount - 1; i++)
            {
                int prior = i - 1;
                int next = i + 1;
                string currentRowCombNo = this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString();
                string priorRowCombNo = this.neuSpread1_Sheet1.Cells[prior, 7].Tag.ToString();
                string nextRowCombNo = this.neuSpread1_Sheet1.Cells[next, 7].Tag.ToString();

                //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                string currentRowInjectTime = this.neuSpread1_Sheet1.Cells[i, 13].Text.ToString();
                string priorRowInjectTime = this.neuSpread1_Sheet1.Cells[prior, 13].Text.ToString();
                string nextRowInjectTime = this.neuSpread1_Sheet1.Cells[next, 13].Text.ToString();

                #region """""
                bool bl1 = true;
                bool bl2 = true;
                //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                if (currentRowCombNo + currentRowInjectTime != priorRowCombNo + priorRowInjectTime)
                    bl1 = false;
                if (currentRowCombNo + currentRowInjectTime != nextRowCombNo + nextRowInjectTime)
                    bl2 = false;
                //  ��
                if (bl1 && bl2)
                {
                    this.neuSpread1_Sheet1.SetValue(i, 7, "��");
                }
                //  ��
                if (bl1 && !bl2)
                {
                    this.neuSpread1_Sheet1.SetValue(i, 7, "��");
                }
                //  ��
                if (!bl1 && bl2)
                {
                    this.neuSpread1_Sheet1.SetValue(i, 7, "��");
                }
                //  ""
                if (!bl1 && !bl2)
                {
                    this.neuSpread1_Sheet1.SetValue(i, 7, "");
                }
                #endregion
            }
            //��û����ŵ�ȥ��
            for (i = 0; i < myCount; i++)
            {
                if (this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() == "")
                {
                    this.neuSpread1_Sheet1.SetValue(i, 7, "");
                }
            }
            //�ж���ĩ�� ����ţ���ֻ���Լ�һ�����ݵ����
            if (myCount == 1)
            {
                this.neuSpread1_Sheet1.SetValue(0, 7, "");
            }
            //ֻ����ĩ���У���ô��Ҫ�ж���Ű�
            if (myCount == 2)
            {
                if (this.neuSpread1_Sheet1.Cells[0, 7].Tag.ToString().Trim() != this.neuSpread1_Sheet1.Cells[1, 7].Tag.ToString().Trim())
                {
                    this.neuSpread1_Sheet1.SetValue(0, 7, "");
                    this.neuSpread1_Sheet1.SetValue(1, 7, "");
                }
            }
            if (myCount > 2)
            {
                if (this.neuSpread1_Sheet1.GetValue(1, 7).ToString() != "��"
                    && this.neuSpread1_Sheet1.GetValue(1, 7).ToString() != "��")
                {
                    this.neuSpread1_Sheet1.SetValue(0, 7, "");
                }
                if (this.neuSpread1_Sheet1.GetValue(myCount - 2, 7).ToString() != "��"
                    && this.neuSpread1_Sheet1.GetValue(myCount - 2, 7).ToString() != "��")
                {
                    this.neuSpread1_Sheet1.SetValue(myCount - 1, 7, "");
                }
            }

        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        private void Print()
        {
            if (this.alPrint == null || this.alPrint.Count <= 0)
            {
                MessageBox.Show("û����Ҫ��ӡ������!");
                return;
            }
            Nurse.Print.ucPrintCure uc = new Nurse.Print.ucPrintCure();
            uc.Init(alPrint);

            if (this.IsFirstTime)
            {
                Nurse.Print.ucPrintInject uc2 = new Nurse.Print.ucPrintInject();
                uc2.Init(alInject);
            }
            alPrint.Clear();
            alInject.Clear();
        }

        /// <summary>
        /// ȫѡ
        /// </summary>
        private void SelectedAll(bool isSelected)
        {
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                //{FAC1693A-3EBA-44b3-A1E3-6D6750A98D80}
                //this.neuSpread1_Sheet1.SetValue(i, 0, isSelected, false);
                this.neuSpread1_Sheet1.Cells[i, 0].Value = isSelected;
            }
        }

        private void SelectedComb(bool isSelect)
        {
            
            int row = this.neuSpread1_Sheet1.ActiveRowIndex;
            string combID = this.neuSpread1_Sheet1.Cells[row, 7].Tag.ToString();
            //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
            string injectTime = this.neuSpread1_Sheet1.Cells[row, 13].Text;
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                //{24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
                if (this.neuSpread1_Sheet1.Cells[i, 7].Tag.ToString() == combID && this.neuSpread1_Sheet1.Cells[i, 13].Text == injectTime) 
                {
                    this.neuSpread1_Sheet1.Cells[i, 0].Value = isSelect;
                }
            }

        }

        /// <summary>
        /// �޸�Ƥ����Ϣ//{26E88889-B2CF-4965-AFD8-6D9BE4519EBF}
        /// </summary>
        private void ModifyHytest()
        {
            ArrayList al = new ArrayList();
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
            {
                bool isSelected = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.neuSpread1_Sheet1.Cells[i, 0].Value);
                if (isSelected)
                {
                    Neusoft.HISFC.Models.Order.OutPatient.Order orderinfo = this.neuSpread1_Sheet1.Cells[i, 11].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    if (orderinfo.HypoTest == 1) continue;
                    al.Add(orderinfo);
                }

            }

            if (al.Count == 0)
            {
                return;
            }
            Forms.frmHypoTest frmHypoTest = new Neusoft.HISFC.Components.Nurse.Forms.frmHypoTest();
            frmHypoTest.AlOrderList = al;
            DialogResult d = frmHypoTest.ShowDialog();
            if (d == DialogResult.OK)
            {
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    bool isSelected = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.neuSpread1_Sheet1.Cells[i, 0].Value);
                    if (!isSelected)
                    {
                        continue;
                    }
                    Neusoft.HISFC.Models.Order.OutPatient.Order orderinfo = this.neuSpread1_Sheet1.Cells[i, 11].Tag as Neusoft.HISFC.Models.Order.OutPatient.Order;
                    string strHypoTest = "";
                    if (orderinfo.HypoTest == 1)
                    {
                        strHypoTest = "��";
                    }
                    else if (orderinfo.HypoTest == 2)
                    {
                        strHypoTest = "��";
                    }
                    else if (orderinfo.HypoTest == 3)
                    {
                        strHypoTest = "����";
                    }
                    else if (orderinfo.HypoTest == 4)
                    {
                        strHypoTest = "����";
                    }
                    this.neuSpread1_Sheet1.Cells[i, 11].Text = strHypoTest;

                }
            }


        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ucRegister()
        {
            InitializeComponent();
        }

        private void ucDept_Load(object sender, EventArgs e)
        {
            this.Init();
            this.SetFP();
            //{24A47206-F111-4817-A7B4-353C21FC7724} ��ʼ��������
            this.InitHelper();
            //{EB016FFE-0980-479c-879E-225462ECA6D0} ƿǩ����
            this.ucCureReprint1.Init();
        }

        /// <summary>
        /// ��ʼ��������
        /// {24A47206-F111-4817-A7B4-353C21FC7724}
        /// </summary>
        private void InitHelper()
        {
            //Ƶ��
            ArrayList alFrequency = this.conMgr.QuereyFrequencyList();
            foreach (Neusoft.HISFC.Models.Order.Frequency frequency in alFrequency)
            {
                this.dicFrequency.Add(frequency.ID, frequency);
            }
        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtRecipe.Text = string.Empty;
                if (this.txtCardNo.Text.Trim() == "")
                {
                    MessageBox.Show("�����벡����!", "��ʾ");
                    this.txtCardNo.Focus();
                    return;
                }
                string cardNo = this.txtCardNo.Text.Trim().PadLeft(10, '0');
                ArrayList alRegs = this.regMgr.Query(cardNo, this.dtpStart.Value);
                if (alRegs == null || alRegs.Count == 0)
                {
                    MessageBox.Show("û�в�����Ϊ:" + cardNo + "�Ļ���!", "��ʾ");
                    this.txtCardNo.Focus();
                    return;
                }
                reg = alRegs[0] as Neusoft.HISFC.Models.Registration.Register;
                if (reg == null || reg.ID == "")
                {
                    MessageBox.Show("û�в�����Ϊ:" + cardNo + "�Ļ���!", "��ʾ");

                    this.txtCardNo.Focus();
                    return;
                }

                this.txtCardNo.Text = cardNo;
                this.SetPatient(reg);

                //�ֽ�ע����Ŀ
                if (al != null)
                {
                    this.Query();
                    this.SetInject();
                }
                this.txtCardNo.Focus();
                this.txtRecipe.SelectAll();
            }
        }

        private void txtRecipe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.txtCardNo.Text = string.Empty;
                if (this.txtRecipe.Text.Trim() == "")
                {
                    this.txtRecipe.Focus();
                    return;
                }
                this.Query();
                this.SetInject();
                this.txtRecipe.Focus();
                this.txtRecipe.SelectAll();
            }
        }

        private void txtOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtCardNo.Focus();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            int altKey = Keys.Alt.GetHashCode();

            if (keyData == Keys.F1)
            {
                this.SelectAll(true);
                return true;
            }
            if (keyData == Keys.F2)
            {
                this.SelectAll(false);
                return true;
            }
            if (keyData.GetHashCode() == altKey + Keys.S.GetHashCode())
            {
                if (this.Save() == 0)
                {
                    this.Print();
                }
                return true;
            }
            if (keyData.GetHashCode() == altKey + Keys.Q.GetHashCode())
            {
                this.Query();
                return true;
            }
            if (keyData.GetHashCode() == altKey + Keys.P.GetHashCode())
            {
                //
                return true;
            }
            if (keyData.GetHashCode() == altKey + Keys.X.GetHashCode())
            {
                this.FindForm().Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void neuSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int row = this.neuSpread1_Sheet1.ActiveRowIndex;
            bool isSelect = Convert.ToBoolean(this.neuSpread1_Sheet1.Cells[row, 0].Value);
            this.SelectedComb(isSelect);
        }

        /// <summary>
        /// ����
        /// {24A47206-F111-4817-A7B4-353C21FC7724} ���߿��ԵǼ�ȫ������ע�䴦��
        /// </summary>
        public class FeeItemListSort : IComparer<Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList>
        {
            public int Compare(Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList x, Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList y)
            {
                //�Ȱ��մ�������
                if (x.Order.ReciptNO != y.Order.ReciptNO)
                {
                    return y.Order.ReciptNO.CompareTo(x.Order.ReciptNO);
                }
                //��ע��ʱ������
                if (x.User03 != y.User03)
                {
                    return y.User03.CompareTo(x.User03);
                }
                //����Ϻ�����
                if (x.Order.Combo.ID != y.Order.Combo.ID)
                {
                    return y.Order.Combo.ID.CompareTo(x.Order.Combo.ID);
                }
                //���������
                if (x.Order.SequenceNO != y.Order.SequenceNO)
                {
                    return y.Order.SequenceNO.CompareTo(x.Order.SequenceNO);
                }
                //ҩƷ����
                if (x.Item.ID != y.Item.ID)
                {
                    return y.Item.ID.CompareTo(x.Item.ID);
                }
                return 0;
            }
        }
    }
}
