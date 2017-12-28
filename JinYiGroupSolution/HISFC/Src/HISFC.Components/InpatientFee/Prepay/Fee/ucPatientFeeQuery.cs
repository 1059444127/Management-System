using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;
using System.Collections;
using Neusoft.HISFC.BizLogic.RADT;
using Neusoft.HISFC.Models.RADT;

namespace Neusoft.HISFC.Components.InpatientFee.Fee
{
    public partial class ucPatientFeeQuery : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucPatientFeeQuery()
        {
            InitializeComponent();
        }

        #region ����
        /// <summary>
        /// ����סԺ���תҵ���
        /// </summary>
        ZZLocal.HISFC.BizLogic.InPatientFee.InPatient radtInpatientFee = new ZZLocal.HISFC.BizLogic.InPatientFee.InPatient(); 
        /// <summary>
        /// סԺ���תҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.RADT.InPatient radtManager = new Neusoft.HISFC.BizLogic.RADT.InPatient();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
        /// <summary>
        /// ��Ա��Ϣҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Manager.Person personManager = new Neusoft.HISFC.BizLogic.Manager.Person();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Manager.Constant consManager = new Neusoft.HISFC.BizLogic.Manager.Constant();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Fee.InPatient feeManager = new Neusoft.HISFC.BizLogic.Fee.InPatient();

        /// <summary>
        /// ��ǰ����
        /// </summary>
        Neusoft.HISFC.Models.RADT.PatientInfo currentPatient = new Neusoft.HISFC.Models.RADT.PatientInfo();

        /// <summary>
        /// Tab
        /// </summary>
        protected Hashtable hashTableFp = new Hashtable();

        #region DataTalbe��ر���

        string pathNameMainInfo = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientMainInfo.xml";
        string pathNamePrepay = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientPrepay.xml";
        string pathNameFee = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientFee.xml";
        string pathNameDrugList = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientDrugList.xml";
        string pathNameUndrugList = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientUndrugList.xml";
        string pathNameBalance = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientBalance.xml";
        string pathNameDiagnose = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @".\QueryPatientDiagnose.xml";

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        DataTable dtMainInfo = new DataTable();

        /// <summary>
        /// ��������Ϣ��ͼ
        /// </summary>
        DataView dvMainInfo = new DataView();

        /// <summary>
        /// ҩƷ��ϸ
        /// </summary>
        DataTable dtDrugList = new DataTable();

        /// <summary>
        /// ҩƷ��ϸ��ͼ
        /// </summary>
        DataView dvDrugList = new DataView();

        /// <summary>
        /// ��ҩƷ��Ϣ
        /// </summary>
        DataTable dtUndrugList = new DataTable();

        /// <summary>
        /// ��ҩƷ��Ϣ��ͼ
        /// </summary>
        DataView dvUndrugList = new DataView();

        /// <summary>
        /// Ԥ������Ϣ
        /// </summary>
        DataTable dtPrepay = new DataTable();

        /// <summary>
        /// Ԥ������ͼ
        /// </summary>
        DataView dvPrepay = new DataView();

        /// <summary>
        /// ���û�����Ϣ
        /// </summary>
        DataTable dtFee = new DataTable();

        /// <summary>
        /// ���û�����Ϣ��ͼ
        /// </summary>
        DataView dvFee = new DataView();

        /// <summary>
        /// ���ý�����Ϣ
        /// </summary>
        DataTable dtBalance = new DataTable();

        /// <summary>
        /// ���ý�����Ϣ��ͼ
        /// </summary>
        DataView dvBalance = new DataView();

        #endregion

        /// <summary>
        /// �����б����������ϸʱ���ҿ���̫�� {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private Hashtable htDept = new Hashtable();

        /// <summary>
        /// ��Ա�б����������ϸʱ������Ա̫��  {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private Hashtable htEmpl = new Hashtable();

        /// <summary>
        /// ��С����  {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private Hashtable htMinFee = new Hashtable();

        #endregion

        /// <summary>
        /// סԺ���߻�����Ϣ
        /// </summary>
        public PatientInfo PatientInfo
        {
            get
            {
                return this.currentPatient;
            }
            set
            {
                this.currentPatient = value;

                this.QueryPatientByInpatientNO(currentPatient);
            }
        }

        #region ˽�з���

        private void InitHashTable() 
        {
            foreach (TabPage t in this.neuTabControl1.TabPages) 
            {
                foreach (Control c in t.Controls) 
                {
                    if (c is FarPoint.Win.Spread.FpSpread) 
                    {
                        this.hashTableFp.Add(t, c);
                    }
                }
            }
        }

        /// <summary>
        /// ��ʾ���߻�����Ϣ
        /// </summary>
        /// <param name="patient">�ɹ� 1 ʧ�� -1</param>
        private void SetPatientToFpMain(Neusoft.HISFC.Models.RADT.PatientInfo patient)
        {
            if (patient == null)
            {
                return;
            }

            DataRow row = this.dtMainInfo.NewRow();
            try
            {

                row["סԺ��ˮ��"] = patient.ID;
                row["סԺ��"] = patient.PID.PatientNO;
                row["����"] = patient.Name;
                row["סԺ����"] = patient.PVisit.PatientLocation.Dept.Name;
                row["����"] = patient.PVisit.PatientLocation.Bed.ID;
                row["�������"] = patient.Pact.Name;
                row["Ԥ����(δ��)"] = patient.FT.PrepayCost;
                row["���úϼ�(δ��)"] = patient.FT.TotCost;
                row["���"] = patient.FT.LeftCost;
                row["�Է�"] = patient.FT.OwnCost;
                row["�Ը�"] = patient.FT.PayCost;
                row["����"] = patient.FT.PubCost;
                row["��Ժ����"] = patient.PVisit.InTime;
                row["��Ժ״̬"] = patient.PVisit.InState.Name;

                row["��Ժ����"] = patient.PVisit.OutTime.Date == new DateTime(1, 1, 1).Date ? string.Empty : patient.PVisit.OutTime.ToString();

                row["Ԥ����(�ѽ�)"] = patient.FT.BalancedPrepayCost;
                row["���úϼ�(�ѽ�)"] = patient.FT.BalancedCost;
                //row["ҽ�����"] = patient.PVisit.MedicalType.Name;
                row["��������"] = patient.BalanceDate;

                this.dtMainInfo.Rows.Add(row);
            }
            catch (Exception e) 
            {
                MessageBox.Show(e.Message);

                return;
            }
        }

        /// <summary>
        /// ���������סԺ�Ų�ѯ���߻�����Ϣ
        /// </summary>
        private void QueryPatientByInpatientNO(PatientInfo patients)
        { 
            this.Clear();
            this.dtMainInfo.Rows.Clear();
            Cursor.Current = Cursors.WaitCursor;
            //סԺ������Ϣ
            this.SetPatientToFpMain(patients);
            Cursor.Current = Cursors.Arrow;
            this.SetPatientInfo();
            //���ò�ѯʱ��
            //���ò�ѯʱ��
            DateTime beginTime = this.currentPatient.PVisit.InTime;
            DateTime endTime = this.radtManager.GetDateTimeFromSysDateTime();
            this.QueryAllInfomaition(beginTime, endTime);
            this.fpMainInfo_Sheet1.Columns[13].Width = 180;
        }

        /// <summary>
        /// ��û���ҩƷ��ϸ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        private void QueryPatientDrugList(DateTime beginTime, DateTime endTime)
        {
            ArrayList drugList = this.feeManager.GetMedItemsForInpatient(this.currentPatient.ID, beginTime, endTime);
            if (drugList == null)
            {
                MessageBox.Show(Language.Msg("��û���ҩƷ��ϸ����!") + this.feeManager.Err);
                
                return;
            }
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList obj in drugList)
            {
                DataRow row = dtDrugList.NewRow();
                
                row["ҩƷ����"] = obj.Item.Name;
                Neusoft.HISFC.Models.Pharmacy.Item medItem = (Neusoft.HISFC.Models.Pharmacy.Item)obj.Item;
                row["���"] = medItem.Specs;
                row["����"] = obj.Item.Price;
                row["����"] = obj.Item.Qty;
                row["����"] = obj.Days;
                row["��λ"] = obj.Item.PriceUnit;
                row["���"] = obj.FT.TotCost;
                row["�Է�"] = obj.FT.OwnCost;
                row["����"] = obj.FT.PubCost;
                row["�Ը�"] = obj.FT.PayCost;
                row["�Ż�"] = obj.FT.RebateCost;
                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //row["ִ�п���"] = this.deptManager.GetDeptmentById(obj.ExecOper.Dept.ID).Name;
                //row["���߿���"] = this.deptManager.GetDeptmentById(((Neusoft.HISFC.Models.RADT.PatientInfo)obj.Patient).PVisit.PatientLocation.Dept.ID).Name;
                row["ִ�п���"] = this.GetDeptName(obj.ExecOper.Dept.ID);
                row["���߿���"] = this.GetDeptName(((Neusoft.HISFC.Models.RADT.PatientInfo)obj.Patient).PVisit.PatientLocation.Dept.ID);
                row["�շ�ʱ��"] = obj.FeeOper.OperTime;

                #region //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //Neusoft.HISFC.BizProcess.Integrate.Manager managerIntergrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                //Neusoft.HISFC.Models.Base.Employee empl = new Neusoft.HISFC.Models.Base.Employee();
                //empl = managerIntergrate.GetEmployeeInfo(obj.FeeOper.ID);
                //if (empl.Name == string.Empty)
                //{
                //    row["�շ�Ա"] = obj.FeeOper.ID;
                //}
                //else
                //{
                //    row["�շ�Ա"] = empl.Name;
                //}
                string emplName = this.GetEmplName(obj.FeeOper.ID);
                if (string.IsNullOrEmpty(emplName))
                {
                    row["�շ�Ա"] = obj.FeeOper.ID;
                }
                else
                {
                    row["�շ�Ա"] = emplName;
                }
                #endregion

                
                row["��ҩʱ��"] = obj.ExecOper.OperTime.Date == new DateTime(1, 1, 1).Date ? string.Empty : obj.ExecOper.OperTime.ToString();

                #region //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //Neusoft.HISFC.Models.Base.Employee confirmOper = new Neusoft.HISFC.Models.Base.Employee();
                //confirmOper = managerIntergrate.GetEmployeeInfo(obj.ExecOper.ID);

                //if (confirmOper.Name == string.Empty)
                //{
                //    row["��ҩԱ"] = obj.ExecOper.ID;
                //}
                //else
                //{
                //    row["��ҩԱ"] = confirmOper.Name;
                //}
                string sendDrugOper = this.GetEmplName(obj.ExecOper.ID);
                if (string.IsNullOrEmpty(sendDrugOper))
                {
                    row["��ҩԱ"] = obj.ExecOper.ID;
                }
                else
                {
                    row["��ҩԱ"] = sendDrugOper;
                }
                #endregion

                //row["��Դ"] = obj.FTSource;
                
                dtDrugList.Rows.Add(row);
            }

            this.AddSumInfo(dtDrugList, "ҩƷ����", "�ϼ�:", "���", "�Է�", "����", "�Ը�", "�Ż�");
        }

        /// <summary>
        /// ��ѯ���߷�ҩƷ��ϸ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        private void QueryPatientUndrugList(DateTime beginTime, DateTime endTime)
        {
            ArrayList undrugList = this.feeManager.QueryFeeItemLists(this.currentPatient.ID, beginTime, endTime);
            if (undrugList == null)
            {
                MessageBox.Show(Language.Msg("��û��߷�ҩƷ��ϸ����!") + this.feeManager.Err);

                return;
            }
   
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList obj in undrugList)
            {
                DataRow row = dtUndrugList.NewRow();
                
                row["��Ŀ����"] = obj.Item.Name;
                row["����"] = obj.Item.Price;
                row["����"] = obj.Item.Qty;
                row["��λ"] = obj.Item.PriceUnit;
                row["���"] = obj.FT.TotCost;
                row["�Է�"] = obj.FT.OwnCost;
                row["����"] = obj.FT.PubCost;
                row["�Ը�"] = obj.FT.PayCost;
                row["�Ż�"] = obj.FT.RebateCost;
                row["�շ�ʱ��"] = obj.FeeOper.OperTime;

                #region //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                ////�տ�Ա����
                //Neusoft.HISFC.BizProcess.Integrate.Manager managerIntergrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                //Neusoft.HISFC.Models.Base.Employee empl = new Neusoft.HISFC.Models.Base.Employee();
                //empl = managerIntergrate.GetEmployeeInfo(obj.FeeOper.ID);

                //if (empl.Name == string.Empty)
                //{
                //    row["�շ�Ա"] = obj.FeeOper.ID;
                //}
                //else
                //{
                //    row["�շ�Ա"] = empl.Name;
                //}
                string emplName = this.GetEmplName(obj.FeeOper.ID);
                if (string.IsNullOrEmpty(emplName))
                {
                    row["�շ�Ա"] = obj.FeeOper.ID;
                }
                else
                {
                    row["�շ�Ա"] = emplName;
                }
                #endregion
               
                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //row["ִ�п���"] = this.deptManager.GetDeptmentById(obj.ExecOper.Dept.ID).Name;
                //row["���߿���"] = this.deptManager.GetDeptmentById(((Neusoft.HISFC.Models.RADT.PatientInfo)obj.Patient).PVisit.PatientLocation.Dept.ID).Name;
                row["ִ�п���"] = this.GetDeptName(obj.ExecOper.Dept.ID);
                row["���߿���"] = this.GetDeptName(((Neusoft.HISFC.Models.RADT.PatientInfo)obj.Patient).PVisit.PatientLocation.Dept.ID);

                //row["��Դ"] = obj.FTSource;
               
                dtUndrugList.Rows.Add(row);
            }

            this.AddSumInfo(dtUndrugList, "��Ŀ����", "�ϼ�:", "���", "�Է�", "����", "�Ը�", "�Ż�");
        }

        /// <summary>
        /// ��ѯ����Ԥ������Ϣ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        private void QueryPatientPrepayList(DateTime beginTime, DateTime endTime)
        {
            ArrayList prepayList = this.feeManager.QueryPrepays(this.currentPatient.ID);
            if (prepayList == null)
            {
                MessageBox.Show(Language.Msg("��û���Ԥ������ϸ����!") + this.feeManager.Err);

                return;
            }

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.Prepay prepay in prepayList)
            {
                Neusoft.HISFC.Models.Base.Employee employeeObj = new Neusoft.HISFC.Models.Base.Employee();
                Neusoft.HISFC.Models.Base.Department deptObj = new Neusoft.HISFC.Models.Base.Department();
                DataRow row = dtPrepay.NewRow();

                row["Ʊ�ݺ�"] = prepay.RecipeNO;
                row["Ԥ�����"] = prepay.FT.PrepayCost;
                row["֧����ʽ"] = prepay.PayType.Name;
                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //employeeObj = this.personManager.GetPersonByID(prepay.PrepayOper.ID);
                //row["����Ա"] = employeeObj.Name;
                row["����Ա"] = this.GetEmplName(prepay.PrepayOper.ID);
                row["��������"] = prepay.PrepayOper.OperTime;
                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //deptObj = this.deptManager.GetDeptmentById(((Neusoft.HISFC.Models.RADT.PatientInfo)prepay.Patient).PVisit.PatientLocation.Dept.ID);
                //row["���ڿ���"] = deptObj.Name;
                row["���ڿ���"] = this.GetDeptName(((Neusoft.HISFC.Models.RADT.PatientInfo)prepay.Patient).PVisit.PatientLocation.Dept.ID);
                string tempBalanceStatusName = string.Empty;
                switch (prepay.BalanceState)
                {
                    case "0":
                        tempBalanceStatusName = "δ����";
                        break;
                    case "1":
                        tempBalanceStatusName = "�ѽ���";
                        break;
                    case "2":
                        tempBalanceStatusName = "�ѽ�ת";
                        break;
                }
                row["����״̬"] = tempBalanceStatusName;
                string tempPrepayStateName = string.Empty;
                switch (prepay.PrepayState)
                {
                    case "0":
                        tempPrepayStateName = "��ȡ";
                        break;
                    case "1":
                        tempPrepayStateName = "����";
                        break;
                    case "2":
                        tempPrepayStateName = "����";
                        break;
                }

                //row["��Դ"] = tempPrepayStateName;

                dtPrepay.Rows.Add(row);
            }

            this.AddSumInfo(dtPrepay, "Ʊ�ݺ�", "�ϼ�:", "Ԥ�����");

            dvPrepay.Sort = "Ʊ�ݺ� ASC";
        }

        /// <summary>
        /// ��û���ָ��ʱ����ڵ���С���û�����Ϣ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        private void QueryPatientFeeInfo(DateTime beginTime, DateTime endTime)
        {
            ArrayList feeInfoList = this.feeManager.QueryFeeInfosGroupByMinFeeByInpatientNO(this.currentPatient.ID, beginTime, endTime, "0");
            if (feeInfoList == null)
            {
                MessageBox.Show(Language.Msg("��û��߷��û�����ϸ����!") + this.feeManager.Err);

                return;
            }



            //feeInfoList.AddRange(feeInfoListBalanced);

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo feeInfo in feeInfoList)
            {

                DataRow row = dtFee.NewRow();

                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //row["��������"] = this.feeManager.GetComDictionaryNameByID("MINFEE", feeInfo.Item.MinFee.ID);
                row["��������"] = this.GetConsName(feeInfo.Item.MinFee.ID);
                row["���"] = feeInfo.FT.TotCost;
                row["�Է�"] = feeInfo.FT.OwnCost;
                row["����"] = feeInfo.FT.PubCost;
                row["�Ը�"] = feeInfo.FT.PayCost;
                row["�Żݽ��"] = feeInfo.FT.RebateCost;
                string temp = string.Empty;

                //if (feeInfo.BalanceState == "0")
                //{
                //    temp = "δ����";
                //}
                //else
                //{
                //    temp = "�ѽ���";
                //}
                row["����״̬"] = "δ����";

                dtFee.Rows.Add(row);
            }

            ArrayList feeInfoListBalanced = this.feeManager.QueryFeeInfosGroupByMinFeeByInpatientNO(this.currentPatient.ID, beginTime, endTime, "1");
            if (feeInfoListBalanced == null)
            {
                MessageBox.Show(Language.Msg("��û��߷��û�����ϸ����!") + this.feeManager.Err);

                return;
            }

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo feeInfo in feeInfoListBalanced)
            {

                DataRow row = dtFee.NewRow();

                row["��������"] = this.feeManager.GetComDictionaryNameByID("MINFEE", feeInfo.Item.MinFee.ID);
                row["���"] = feeInfo.FT.TotCost;
                row["�Է�"] = feeInfo.FT.OwnCost;
                row["����"] = feeInfo.FT.PubCost;
                row["�Ը�"] = feeInfo.FT.PayCost;
                row["�Żݽ��"] = feeInfo.FT.RebateCost;
                string temp = string.Empty;

                //if (feeInfo.BalanceState == "0")
                //{
                //    temp = "δ����";
                //}
                //else
                //{
                //    temp = "�ѽ���";
                //}
                row["����״̬"] = "�ѽ���";

                dtFee.Rows.Add(row);
            }
        }

        /// <summary>
        /// ��û��߽�����Ϣ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        private void QueryPatientBalance(DateTime beginTime, DateTime endTime)
        {
            ArrayList balanceList = this.feeManager.QueryBalancesByInpatientNO(this.currentPatient.ID);
            if (balanceList == null)
            {
                MessageBox.Show(Language.Msg("��û��߷��ý������!") + this.feeManager.Err);

                return;
            }
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.Balance balance in balanceList)
            {
                Neusoft.HISFC.Models.Base.Employee employeeObj = new Neusoft.HISFC.Models.Base.Employee();
                string temp = "";
                DataRow row = dtBalance.NewRow();

                row["��Ʊ����"] = balance.Invoice.ID;
                row["Ԥ�����"] = balance.FT.PrepayCost;
                row["�ܽ��"] = balance.FT.TotCost;
                row["�Է�"] = balance.FT.OwnCost;
                row["����"] = balance.FT.PubCost;
                row["�Ը�"] = balance.FT.PayCost;
                row["�Ż�"] = balance.FT.RebateCost;
                row["�������"] = balance.FT.ReturnCost;
                row["���ս��"] = balance.FT.SupplyCost;
                row["����ʱ��"] = balance.BalanceOper.OperTime;
                //{FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 2010-09-27
                //employeeObj = this.personManager.GetPersonByID(balance.BalanceOper.ID);
                //row["����Ա"] = employeeObj.Name;
                row["����Ա"] = this.GetEmplName(balance.BalanceOper.ID);
                row["��������"] = balance.BalanceType.Name;

                switch (balance.CancelType)
                {
                    case Neusoft.HISFC.Models.Base.CancelTypes.Valid:
                        temp = "��������";
                        break;
                    case Neusoft.HISFC.Models.Base.CancelTypes.LogOut:
                        temp = "�����ٻ�";
                        break;
                    case Neusoft.HISFC.Models.Base.CancelTypes.Reprint:
                        temp = "��Ʊ�ش�";
                        break;

                }
                row["����״̬"] = temp;

                dtBalance.Rows.Add(row);
            }

            AddSumInfo(dtBalance, "��Ʊ����", "�ϼ�:", "�ܽ��", "�Է�", "����", "�Ը�", "�Ż�", "�������", "���ս��");
        }

        /// <summary>
        /// ��Ӻϼ�
        /// </summary>
        /// <param name="table">��ǰDataTalbe</param>
        /// <param name="totName">�ϼƵ�����λ��</param>
        /// <param name="disName">�ϼ�������</param>
        /// <param name="sumColName">ͳ���е�����</param>
        public void AddSumInfo(DataTable table, string totName, string disName, params string[] sumColName)
        {
            DataRow sumRow = table.NewRow();

            sumRow[totName] = disName;
            
            foreach (string s in sumColName)
            {
                object sum = table.Compute("SUM(" + s + ")", "");
                sumRow[s] = sum;
            }
            
            table.Rows.Add(sumRow);
        }

        /// <summary>
        /// ��ʼ��DataTable
        /// </summary>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        private int InitDataTable() 
        {
            Type str = typeof(String);
            Type date = typeof(DateTime);
            Type dec = typeof(Decimal);
            Type bo = typeof(bool);

            //����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
            this.fpMainInfo_Sheet1.DataAutoSizeColumns = false;
            this.fpDrugList_Sheet1.DataAutoSizeColumns = false;
            this.fpUndrugList_Sheet1.DataAutoSizeColumns = false;
            this.fpPrepay_Sheet1.DataAutoSizeColumns = false;
            this.fpFee_Sheet1.DataAutoSizeColumns = false;
            this.fpBalance_Sheet1.DataAutoSizeColumns = false;

            #region סԺ������Ϣ
            
            if (System.IO.File.Exists(pathNameMainInfo))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNameMainInfo, dtMainInfo, ref dvMainInfo, this.fpMainInfo_Sheet1);

                dtMainInfo.PrimaryKey = new DataColumn[] { dtMainInfo.Columns["סԺ��ˮ��"] };
                
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpMainInfo_Sheet1, pathNameMainInfo);
                 
            }
            else
            {
                
                dtMainInfo.Columns.AddRange(new DataColumn[]{new DataColumn("סԺ��ˮ��", str),
																new DataColumn("סԺ��", str),
																new DataColumn("����", str),
																new DataColumn("סԺ����", str),
																new DataColumn("����", str),
																new DataColumn("�������", str),
																new DataColumn("Ԥ����(δ��)", dec),
																new DataColumn("���úϼ�(δ��)", dec),
																new DataColumn("���", dec),
																new DataColumn("�Է�", dec),
																new DataColumn("�Ը�", dec),
																new DataColumn("����", dec),
																new DataColumn("��Ժ����", date),
																new DataColumn("��Ժ״̬", str),
																new DataColumn("��Ժ����", str),
																new DataColumn("Ԥ����(�ѽ�)", dec),
																new DataColumn("���úϼ�(�ѽ�)", dec),
																new DataColumn("��������", date)/*,
																new DataColumn("ҽ�����", str)*/});

                dtMainInfo.PrimaryKey = new DataColumn[] { dtMainInfo.Columns["סԺ��ˮ��"] };

                dvMainInfo = new DataView(dtMainInfo);

                this.fpMainInfo_Sheet1.DataSource = dvMainInfo;
                
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpMainInfo_Sheet1, pathNameMainInfo);
            }

            #endregion

            #region ҩƷ��ϸ��Ϣ

            if (System.IO.File.Exists(pathNameDrugList))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNameDrugList, dtDrugList, ref dvDrugList, this.fpDrugList_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpDrugList_Sheet1, pathNameDrugList);
            }
            else
            {
                dtDrugList.Columns.AddRange(new DataColumn[]{new DataColumn("ҩƷ����", str),
																new DataColumn("���", str),
																new DataColumn("����", dec),
																new DataColumn("����", dec),
																new DataColumn("����", dec),
																new DataColumn("��λ", str),
																new DataColumn("���", dec),
																new DataColumn("�Է�", dec),
																new DataColumn("����", dec),
																new DataColumn("�Ը�", dec),
																new DataColumn("�Ż�", dec),
																new DataColumn("ִ�п���",str),
																new DataColumn("���߿���",str),
																new DataColumn("�շ�ʱ��", str),
																new DataColumn("�շ�Ա", str),
																new DataColumn("��ҩʱ��", str),   
																new DataColumn("��ҩԱ", str),
				                                                new DataColumn("��Դ",str)});

                dvDrugList = new DataView(dtDrugList);

                this.fpDrugList_Sheet1.DataSource = dvDrugList;

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpDrugList_Sheet1, pathNameDrugList);
            }

            #endregion

            #region ��ҩƷ��ϸ��Ϣ
            if (System.IO.File.Exists(pathNameUndrugList))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNameUndrugList, dtUndrugList, ref dvUndrugList, this.fpUndrugList_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpUndrugList_Sheet1, pathNameUndrugList);
            }
            else
            {
                dtUndrugList.Columns.AddRange(new DataColumn[]{new DataColumn("��Ŀ����", str),
																  new DataColumn("����", dec),
																  new DataColumn("����", dec),
																  new DataColumn("��λ", str),
																  new DataColumn("���", dec),
																  new DataColumn("�Է�", dec),
																  new DataColumn("����", dec),
																  new DataColumn("�Ը�", dec),
																  new DataColumn("�Ż�", dec),
																  new DataColumn("ִ�п���", str),
																  new DataColumn("���߿���",str),
																  new DataColumn("�շ�ʱ��", date),
																  new DataColumn("�շ�Ա", str),
				                                                  new DataColumn("��Դ", str)});

                dvUndrugList = new DataView(dtUndrugList);

                this.fpUndrugList_Sheet1.DataSource = dvUndrugList;

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpUndrugList_Sheet1, pathNameUndrugList);
            }

            #endregion

            #region Ԥ������Ϣ

            if (System.IO.File.Exists(pathNamePrepay))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNamePrepay, dtPrepay, ref dvPrepay, this.fpPrepay_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpPrepay_Sheet1, pathNamePrepay);
            }
            else
            {
                dtPrepay.Columns.AddRange(new DataColumn[]{new DataColumn("Ʊ�ݺ�", str),
															  new DataColumn("Ԥ�����", dec),
															  new DataColumn("֧����ʽ", str),
															  new DataColumn("����Ա", str),
															  new DataColumn("��������", date),
															  new DataColumn("���ڿ���", str),
															  new DataColumn("����״̬", str),
															  new DataColumn("��Դ", str)});

                dvPrepay = new DataView(dtPrepay);

                this.fpPrepay_Sheet1.DataSource = dvPrepay;
                dvPrepay.Sort = "Ʊ�ݺ� ASC";

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpPrepay_Sheet1, pathNamePrepay);
            }

            #endregion

            #region ��С������Ϣ
            
            if (System.IO.File.Exists(pathNameFee))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNameFee, dtFee, ref dvFee, this.fpFee_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpFee_Sheet1, pathNameFee);
            }
            else
            {
                dtFee.Columns.AddRange(new DataColumn[]{new DataColumn("��������", str),
														   new DataColumn("���", dec),
														   new DataColumn("�Է�", dec),
														   new DataColumn("����", dec),
														   new DataColumn("�Ը�", dec),
														   new DataColumn("�Żݽ��", dec),
														   new DataColumn("����״̬", str)});

                dvFee = new DataView(dtFee);

                this.fpFee_Sheet1.DataSource = dvFee;

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpFee_Sheet1, pathNameFee);
            }

            #endregion

            #region ������Ϣ
            if (System.IO.File.Exists(pathNameBalance))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(pathNameBalance, dtBalance, ref dvBalance, this.fpBalance_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpBalance_Sheet1, pathNameBalance);
            }
            else
            {
                dtBalance.Columns.AddRange(new DataColumn[]{new DataColumn("��Ʊ����", str),
															   new DataColumn("��������", str),
															   new DataColumn("����״̬", str),
															   new DataColumn("Ԥ�����", dec),
															   new DataColumn("�ܽ��", dec),
															   new DataColumn("�Է�", dec),
															   new DataColumn("����", dec),
															   new DataColumn("�Ը�", dec),
															   new DataColumn("�Ż�", dec),
															   new DataColumn("�������", dec),
															   new DataColumn("���ս��", dec),
															   new DataColumn("����ʱ��", date),
															   new DataColumn("����Ա", str)});

                dvBalance = new DataView(dtBalance);

                this.fpBalance_Sheet1.DataSource = dvBalance;

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpBalance_Sheet1, pathNameBalance);
            }
            #endregion

            return 1;
        }

        /// <summary>
        /// ��ʾ���߻�����Ϣ
        /// </summary>
        protected void SetPatientInfo() 
        {
            if (this.currentPatient == null || this.currentPatient.ID == null || this.currentPatient.ID == string.Empty) 
            {
                return;
            } 

            this.lblID.Text = this.currentPatient.PID.PatientNO;//סԺ��
            this.lblName.Text = this.currentPatient.Name;//����;
            this.lblSex.Text = this.currentPatient.Sex.Name;
            //�������������� {4B4A3C43-1A6C-46c8-B49F-7992D8C81C02} wbo 2010-10-20
            //this.lblAge.Text = this.currentPatient.Age;
            try
            {
                this.lblAge.Text = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.currentPatient.Birthday);
            }
            catch (Exception e)
            { }
            this.lblBed.Text = this.currentPatient.PVisit.PatientLocation.Bed.ID;
            this.lblDept.Text = this.currentPatient.PVisit.PatientLocation.Dept.Name;
            this.lblPact.Text = this.currentPatient.Pact.Name;//��ͬ��λ
            this.lblDateIn.Text = this.currentPatient.PVisit.InTime.ToShortDateString();//סԺ����
            if (this.currentPatient.PVisit.OutTime != DateTime.MinValue)
            {
                this.lblOutDate.Text = this.currentPatient.PVisit.OutTime.ToShortDateString();
            }
            this.lblInState.Text = this.currentPatient.PVisit.InState.Name;//��Ժ״̬
            decimal TotCost = this.currentPatient.FT.TotCost + this.currentPatient.FT.BalancedCost;
            //this.lblTotCost.Text = this.currentPatient.FT.TotCost.ToString();
            this.lblTotCost.Text = TotCost.ToString();

            this.lblOwnCost.Text = this.currentPatient.FT.OwnCost.ToString();
            this.lblPubCost.Text = this.currentPatient.FT.PubCost.ToString();
            this.lblPrepayCost.Text = this.currentPatient.FT.PrepayCost.ToString();
            this.lblUnBalanceCost.Text = this.currentPatient.FT.TotCost.ToString();
            this.lblBalancedCost.Text = this.currentPatient.FT.BalancedCost.ToString();
            this.lblFreeCost.Text = this.currentPatient.FT.LeftCost.ToString();
            this.lblDiagnose.Text = this.currentPatient.MainDiagnose;
            this.lblMemo.Text = this.currentPatient.Memo;

            //���뻼��ҽ����Ϣ
            this.lblItemYLCost.Text = this.currentPatient.SIMainInfo.ItemYLCost.ToString();
            this.lblBaseCost.Text = this.currentPatient.SIMainInfo.BaseCost.ToString();
            this.lblItempaycost.Text = this.currentPatient.SIMainInfo.ItemPayCost.ToString();
            this.lblsipubcost.Text = this.currentPatient.SIMainInfo.SiPubCost.ToString();
            this.lblovercost.Text = this.currentPatient.SIMainInfo.OverCost.ToString();
            this.lblovertakeowncost.Text = this.currentPatient.SIMainInfo.OverTakeOwnCost.ToString();
            this.lblofficalcost.Text = this.currentPatient.SIMainInfo.OfficalCost.ToString();
            this.lblpaycost.Text = this.currentPatient.SIMainInfo.PayCost.ToString();
            this.lblcaowncost.Text = this.currentPatient.SIMainInfo.OwnCost.ToString();
        }

        /// <summary>
        /// ��ѯ������Ϣ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        protected void QueryAllInfomaition(DateTime beginTime, DateTime endTime) 
        {
            this.QueryPatientDrugList(beginTime, endTime);
           
            this.QueryPatientUndrugList(beginTime, endTime);

            this.QueryPatientPrepayList(beginTime, endTime);

            this.QueryPatientFeeInfo(beginTime, endTime);

            this.QueryPatientBalance(beginTime, endTime);
        }

        /// <summary>
        /// ����"_"
        /// </summary>
        /// <param name="langth">����</param>
        /// <returns>�ɹ� "---" ʧ�� null</returns>
        private string RetrunSplit(int langth) 
        {
            string s = string.Empty ;

            for (int i = 0; i < langth; i++) 
            {
                s += "_";
            }

            return s;
        }

        /// <summary>
        /// ���
        /// </summary>
        private void Clear() 
        {
            this.lblID.Text = this.RetrunSplit(10);
            this.lblName.Text = this.RetrunSplit(5);
            this.lblSex.Text = this.RetrunSplit(4);
            this.lblAge.Text = this.RetrunSplit(4);
            this.lblBed.Text = this.RetrunSplit(10);
            this.lblDept.Text = this.RetrunSplit(10);
            this.lblPact.Text = this.RetrunSplit(10);
            this.lblDateIn.Text = this.RetrunSplit(10);
            this.lblOutDate.Text = this.RetrunSplit(10);

            this.lblInState.Text = this.RetrunSplit(6);
            this.lblTotCost.Text = this.RetrunSplit(10);
            this.lblOwnCost.Text = this.RetrunSplit(10);
            this.lblPubCost.Text = this.RetrunSplit(10);
            this.lblPrepayCost.Text = this.RetrunSplit(10);
            this.lblUnBalanceCost.Text = this.RetrunSplit(10);
            this.lblBalancedCost.Text = this.RetrunSplit(10);
            this.lblDiagnose.Text = this.RetrunSplit(20);
            this.lblMemo.Text = this.RetrunSplit(30);
            dtMainInfo.Rows.Clear();
            dtDrugList.Rows.Clear();
            dtUndrugList.Rows.Clear();
            dtPrepay.Rows.Clear();
            dtFee.Rows.Clear();
            dtBalance.Rows.Clear();
        }

        private void InitContr()
        {
            this.neuLabel21.Visible = false;
            this.lblOwnCost.Visible = false;
            this.neuLabel23.Visible = false;
            this.lblPubCost.Visible = false;
        }

        /// <summary>
        /// ���ؿ����б�   {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private void InitDept()
        {
            try
            {
                ArrayList arrDept = deptManager.GetDeptmentAll();
                if (arrDept == null)
                {
                    return;
                }

                foreach (Neusoft.HISFC.Models.Base.Department dept in arrDept)
                {
                    this.htDept.Add(dept.ID, dept);
                }
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// ������Ա�б�   {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private void InitEmpl()
        {
            try
            {
                ArrayList arrEmpl = this.personManager.GetEmployeeAll();
                if (arrEmpl == null)
                {
                    return;
                }

                foreach (Neusoft.HISFC.Models.Base.Employee empl in arrEmpl)
                {
                    this.htEmpl.Add(empl.ID, empl);
                }
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// ������С���� {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        private void InitMinFee()
        {
            try
            {
                Neusoft.HISFC.BizLogic.Manager.Constant conManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
                ArrayList arrMinFee = conManager.GetAllList("MINFEE");
                if (arrMinFee == null)
                {
                    return;
                }

                foreach (Neusoft.HISFC.Models.Base.Const cons in arrMinFee)
                {
                    this.htMinFee.Add(cons.ID, cons);
                }
            }
            catch (Exception e)
            { }
        }

        /// <summary>
        /// ���ݲ��ű����ȡ�������� {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        private string GetDeptName(string deptID)
        {
            string deptName = "";
            if(string.IsNullOrEmpty(deptID) || !this.htDept.Contains(deptID))
            {
                return deptName;
            }
            try
            {
                Neusoft.HISFC.Models.Base.Department d = this.htDept[deptID] as Neusoft.HISFC.Models.Base.Department;
                deptName = d.Name;
            }
            catch(Exception e)
            {
                deptName = "";
            }
            return deptName;
        }

        /// <summary>
        /// ������Ա�����ȡ��Ա���� {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        /// <param name="emplID"></param>
        /// <returns></returns>
        private string GetEmplName(string emplID)
        {
            string emplName = "";
            if (string.IsNullOrEmpty(emplID) || !this.htEmpl.Contains(emplID))
            {
                return emplName;
            }
            try
            {
                Neusoft.HISFC.Models.Base.Employee d = this.htEmpl[emplID] as Neusoft.HISFC.Models.Base.Employee;
                emplName = d.Name;
            }
            catch (Exception e)
            {
                emplName = "";
            }
            return emplName;
        }

        /// <summary>
        /// ������С���ñ����ȡ��С�������� {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
        /// </summary>
        /// <param name="consID"></param>
        /// <returns></returns>
        private string GetConsName(string consID)
        {
            string consName = "";
            if (string.IsNullOrEmpty(consID) || !this.htMinFee.Contains(consID))
            {
                return consName;
            }
            try
            {
                Neusoft.HISFC.Models.Base.Const cons = this.htMinFee[consID] as Neusoft.HISFC.Models.Base.Const;
                consName = cons.Name;
            }
            catch (Exception e)
            {
                consName = "";
            }
            return consName;
        }

        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            this.PatientInfo = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;

            return base.OnSetValue(neuObject, e);
        }

        #endregion

        protected override int OnPrint(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();

            print.PrintPage(0, 0, this.neuTabControl1.SelectedTab);
            
            return base.OnPrint(sender, neuObject);
        }
        //��ӡԤ��
        public override int  PrintPreview(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print printview = new Neusoft.FrameWork.WinForms.Classes.Print();

            printview.PrintPreview(0, 0, this.neuTabControl1.SelectedTab);
            return base.OnPrintPreview(sender, neuObject);
        }
       
        public override int Export(object sender, object neuObject)
        {
            object obj = this.hashTableFp[this.neuTabControl1.SelectedTab];

            FarPoint.Win.Spread.FpSpread fp = obj as FarPoint.Win.Spread.FpSpread;

            SaveFileDialog op = new SaveFileDialog();

            op.Title = "��ѡ�񱣴��·��������";
            op.CheckFileExists = false;
            op.CheckPathExists = true;
            op.DefaultExt = "*.xls";
            op.Filter = "(*.xls)|*.xls";

            DialogResult result = op.ShowDialog();

            if (result == DialogResult.Cancel || op.FileName == string.Empty) 
            {
                return -1;
            }

            string filePath = op.FileName;

            bool returnValue = fp.SaveExcel(filePath, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);


            return base.Export(sender, neuObject);
        }

        private void ucPatientFeeQuery_Load(object sender, EventArgs e)
        {
            this.InitDataTable();
            this.InitContr();
            //���ؿ����б�   {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
            this.InitDept();
            //������Ա�б�   {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
            this.InitEmpl();
            //������С�����б�   {FFB47AA4-77E6-4017-9F4E-663E4DE0DF8A} wbo 20100927
            this.InitMinFee();
        }

        private void fpMainInfo_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.fpMainInfo_Sheet1.RowCount <= 0)
            {
                return;
            }
            string inpatientNO = this.fpMainInfo_Sheet1.Cells[e.Row, 0].Text;

            if (inpatientNO == null) 
            {
                return;
            }
            this.currentPatient = this.radtInpatientFee.QueryPatientInfoByInpatientNO(inpatientNO);
           // this.currentPatient = this.radtManager.QueryPatientInfoByInpatientNO(inpatientNO);
            if (this.currentPatient == null || this.currentPatient.ID == null || this.currentPatient.ID == string.Empty) 
            {
                MessageBox.Show(Language.Msg("��ѯ���߻�����Ϣ����!") + this.radtManager.Err);

                return;
            }

            this.SetPatientInfo();
 
            dtDrugList.Rows.Clear();
            dtUndrugList.Rows.Clear();
            dtPrepay.Rows.Clear();
            dtFee.Rows.Clear();
            dtBalance.Rows.Clear();

            //���ò�ѯʱ��
            DateTime beginTime = this.currentPatient.PVisit.InTime;
            DateTime endTime = this.radtManager.GetDateTimeFromSysDateTime();

            this.QueryAllInfomaition(beginTime, endTime);

        }

        private void neuLabel24_Click(object sender, EventArgs e)
        {

        }

        private void lblMemo_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpMainInfo_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpMainInfo_Sheet1, this.pathNameMainInfo);
        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpDrugList_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpDrugList_Sheet1, this.pathNameDrugList);
        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpUndrugList_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpUndrugList_Sheet1, this.pathNameUndrugList);
        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpPrepay_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpPrepay_Sheet1, this.pathNamePrepay);
        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpFee_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpFee_Sheet1, this.pathNameFee);
        }

        /// <summary>
        /// ����FP�п� {513FC45C-4CCB-4a42-B336-D163C341F794} wbo 2010-09-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpBalance_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpBalance_Sheet1, this.pathNameBalance);
        }

    }
}
