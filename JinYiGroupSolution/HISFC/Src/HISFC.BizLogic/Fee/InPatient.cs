using System;
using System.Data;
using Neusoft.HISFC.Models;
using System.Collections;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Fee.Inpatient;
using Neusoft.HISFC.Models.Fee;
using Neusoft.HISFC.Models.RADT;
using Neusoft.FrameWork.Function;
using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.BizLogic.Fee
{
    /// <summary>
    /// ���ù�����-סԺ 
    /// <br>1��	AddPatientAccount(BAR/ACK)</br>
    ///	<br>2��	PurgePatientAccount(BAR/ACK)</br>
    ///	<br><strike>3��	PostDetailFinancialTransactions(DFT)</strike></br>
    ///	<br>4��	UpdateAccount(BAR/ACK)</br>
    ///	<br>5��	EndAccount(BAR/ACK)</br>
    /// </summary>
    public class InPatient : Neusoft.FrameWork.Management.Database
    {

        #region ˽�з���

        #region ������²���

        /// <summary>
        /// ���µ������
        /// </summary>
        /// <param name="sqlIndex">SQL�������</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�: >= 1 ʧ�� -1 û�и��µ����� 0</returns>
        private int UpdateSingleTable(string sqlIndex, params string[] args)
        {
            string sql = string.Empty;//Update���

            //���Where���
            if (this.Sql.GetSql(sqlIndex, ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + sqlIndex + "��SQL���";

                return -1;
            }

            return this.ExecNoQuery(sql, args);
        }

        /// <summary>
        /// ����Ψһֵ
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:���ص�ǰΨһֵ ʧ��:null</returns>
        private string ExecSqlReturnOne(string index, params string[] args)
        {
            string sql = string.Empty;//SQL���

            if (this.Sql.GetSql(index, ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + index + "��SQL���";

                return null;
            }

            try
            {
                sql = string.Format(sql, args);
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                return null;
            }

            return base.ExecSqlReturnOne(sql);
        }

        #endregion

        #region Ԥ����

        /// <summary>
        /// ���Ԥ���������ַ�������
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: Ԥ���������ַ������� ʧ��: null</returns>
        private string[] GetPrepayParams(Prepay prepay)
        {
            return this.GetPrepayParams(prepay.Patient, prepay);
        }

        /// <summary>
        /// ���Ԥ���������ַ�������
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: Ԥ���������ַ������� ʧ��: null</returns>
        private string[] GetPrepayParams(PatientInfo patient, Prepay prepay)
        {
            string[] args ={
							   //סԺ��ˮ��
							   patient.ID,
							   //�������
							   prepay.ID,
							   //��������
							   patient.Name,
							   //Ԥ�����
							   prepay.FT.PrepayCost.ToString(),
							   //������ʽ
							   prepay.PayType.ID.ToString(),
							   // ���Ҵ���
							   patient.PVisit.PatientLocation.Dept.ID,
							   //Ԥ�����վݺ���
							   prepay.RecipeNO,
							   //����ʱ��
							   prepay.BalanceOper.OperTime.ToString(),
							   //�����־ 0:δ���㣻1:�ѽ���
							   prepay.BalanceState,
							   //Ԥ����״̬0:��ȡ��1:����;2:����3:�ٻ�
							   prepay.PrepayState,
							   //��������
							   prepay.Bank.Name,
							   //�����ʻ�
							   prepay.Bank.Account,
							   //���㷢Ʊ��
							   prepay.Invoice.ID,
							   //�������
							   prepay.BalanceNO.ToString(),
							   //�����˴���
							   prepay.BalanceOper.ID,
							   //�Ͻɱ�־��1�� 0��
							   System.Convert.ToInt16(prepay.IsTurnIn).ToString(),
							   //���������
							   prepay.FinGroup.ID,
							   //������λ
							   prepay.Bank.WorkName,
							   //0��תѺ��1תѺ��2תѺ���Ѵ�ӡ
							   prepay.TransferPrepayState,
							   //תѺ�����Ա
							   prepay.TransPrepayOper.ID,
							   //תѺ��ʱ��
							   prepay.TransferPrepayTime.ToString(),
							   //������ˮ�Ż�֧Ʊ�Ż��Ʊ��
							   prepay.Bank.InvoiceNO,
							   //����Ա
							   prepay.PrepayOper.ID,
							   //��������
							   prepay.PrepayOper.OperTime.ToString(),
							   //����������
							   prepay.AuditingNO,
							   //תѺ��ʱ�������
							   prepay.TransferPrepayBalanceNO.ToString(),
							   //ԭ��Ʊ����
							   prepay.OrgInvoice.ID,
							   //����Ա����
							   prepay.PrepayOper.Dept.ID,
                               //Ԥ����Դ 1 �������� 2 �����ٻ�
                               prepay.PrepaySourceState
						   };

            return args;
        }

        /// <summary>
        /// ��ȡ����fin_ipb_inprepay��ȫ�����ݵ�sql
        /// </summary>
        /// <returns>�ɹ�: ����SQL��� ʧ�� null</returns>
        private string GetSqlForSelectAllPrepay()
        {
            string sql = string.Empty;//SQL���

            if (this.Sql.GetSql("Fee.Inpatient.Prepay.Get.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.Inpatient.Prepay.Get.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ͨ��Where������ѯԤ������Ϣ
        /// </summary>
        /// <param name="whereIndex">Where����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:����Ԥ����ʵ�弯�� ʧ�� null</returns>
        private ArrayList QueryPrepays(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetSqlForSelectAllPrepay();

            return this.QueryPrepaysBySql(sql + " " + where, args);
        }

        /// <summary>
        /// ����SQL��ѯԤ����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:����Ԥ����ʵ�弯�� ʧ�� null</returns>
        private ArrayList QueryPrepaysBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList prepays = new ArrayList();//Ԥ���𼯺�
            Prepay prepay = null;//Ԥ����ʵ��

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    prepay = new Prepay();

                    prepay.ID = this.Reader[0].ToString(); //0�������
                    prepay.Name = this.Reader[1].ToString();//1����
                    prepay.FT.PrepayCost = NConvert.ToDecimal(this.Reader[2].ToString());//2Ԥ�����
                    prepay.PayType.ID = this.Reader[3].ToString();//3������ʽ
                    prepay.Dept.ID = this.Reader[4].ToString();//4���Ҵ���
                    prepay.Patient.PVisit.PatientLocation.Dept.ID = this.Reader[4].ToString();//4���Ҵ���
                    prepay.RecipeNO = this.Reader[5].ToString();//5Ԥ�����վݺ���
                    prepay.BalanceOper.OperTime = NConvert.ToDateTime(this.Reader[6].ToString());//6����ʱ��
                    prepay.BalanceState = this.Reader[7].ToString();//7�����־	0:δ���㣻1:�ѽ���
                    prepay.PrepayState = this.Reader[8].ToString();//8Ԥ����״̬0:��ȡ��1:����;2:���� 3���ٻ�
                    prepay.Bank.Name = this.Reader[9].ToString();//9��������
                    prepay.Bank.Account = this.Reader[10].ToString();//10�����ʻ�
                    prepay.Invoice.ID = this.Reader[11].ToString();//11���㷢Ʊ��
                    prepay.BalanceNO = NConvert.ToInt32(this.Reader[12].ToString());//12�������
                    prepay.BalanceOper.ID = this.Reader[13].ToString();//13�����˴���
                    prepay.IsTurnIn = NConvert.ToBoolean(this.Reader[14].ToString());//14�Ͻɱ�־��1�� 0��
                    prepay.FinGroup.ID = this.Reader[15].ToString();//15���������
                    prepay.Bank.WorkName = this.Reader[16].ToString();//16������λ
                    prepay.TransferPrepayState = this.Reader[17].ToString();//17 0��תѺ��1תѺ��2תѺ���Ѵ�ӡ
                    prepay.TransPrepayOper.ID = this.Reader[18].ToString();//18תѺ�����Ա
                    prepay.TransferPrepayTime = NConvert.ToDateTime(this.Reader[19].ToString());//19תѺ��ʱ��
                    prepay.Bank.InvoiceNO = this.Reader[20].ToString();//20pos������ˮ�Ż�֧Ʊ�Ż��Ʊ��
                    prepay.PrepayOper.ID = this.Reader[21].ToString();//21����Ա	
                    prepay.PrepayOper.OperTime = NConvert.ToDateTime(this.Reader[22].ToString());//22��������
                    prepay.AuditingNO = this.Reader[23].ToString();//23����������
                    prepay.TransferPrepayBalanceNO = NConvert.ToInt32(this.Reader[24].ToString());//24תѺ��ʱ�������
                    prepay.OrgInvoice.ID = this.Reader[25].ToString();//25ԭʼ��Ʊ��
                    prepay.PrepayOper.Dept.ID = this.Reader[26].ToString();//26����Ա���ڿ���
                    prepay.PayType.Name = this.Reader[27].ToString();//֧����ʽ����
                    //{9B8D83F8-CB0F-48fb-8ECD-0BA4462A952A}
                    prepay.Memo = this.Reader[28].ToString(); //�ս���
                    prepays.Add(prepay);
                }//ѭ������

                this.Reader.Close();

                return prepays;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        #endregion

        #region ���������ϸ

        /// <summary>
        /// ͨ����ҩƷʵ����ʵ����������
        /// </summary>
        /// <param name="patient">��Ա������Ϣ</param>
        /// <param name="feeItemList">��ҩƷ���û�����Ϣ</param>
        /// <returns>��ҩƷʵ����ʵ����������</returns>
        //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
        private string[] GetFeeItemListParams(PatientInfo patient, FeeItemList feeItemList)
        {
            feeItemList.Patient = patient.Clone();

            string[] args = 
				{
					feeItemList.RecipeNO,//0 ������
					feeItemList.SequenceNO.ToString(),//��������ˮ��
					feeItemList.Item.MinFee.ID,//1��С���ô���
					((int)feeItemList.TransType).ToString(),//2 ��������,1�����ף�2������
					feeItemList.Patient.ID,//3סԺ��ˮ��
					feeItemList.Patient.Name,//4����
					feeItemList.Patient.Pact.PayKind.ID,//5�������
					feeItemList.Patient.Pact.ID,//6��ͬ��λ
					((PatientInfo)feeItemList.Patient).PVisit.PatientLocation.Dept.ID,// 7��Ժ���Ҵ���
					((PatientInfo)feeItemList.Patient).PVisit.PatientLocation.NurseCell.ID,//8��ʿվ����
					feeItemList.RecipeOper.Dept.ID,//9�������Ҵ���
					feeItemList.ExecOper.Dept.ID,//10ִ�п��Ҵ���
					feeItemList.StockOper.Dept.ID,//11�ۿ���Ҵ���
					feeItemList.RecipeOper.ID,//12����ҽʦ����
					feeItemList.Item.ID,//13��Ŀ����
					feeItemList.Compare.CenterItem.ID,//14���Ĵ���
                    feeItemList.Item.Name,
					feeItemList.Item.Price.ToString(),//15����
					feeItemList.Item.Qty.ToString(),//16����
					feeItemList.UndrugComb.ID,//17���״���
					feeItemList.UndrugComb.Name,//18��������
					feeItemList.FT.TotCost.ToString(),//19���ý��
					feeItemList.FT.OwnCost.ToString(),//20�Էѽ��
					feeItemList.FT.PayCost.ToString(),//21�Ը����
					feeItemList.FT.PubCost.ToString(),//22���ѽ��
					feeItemList.FT.RebateCost.ToString(),//23�Żݽ��
					((int)feeItemList.PayType).ToString(),//25����״̬
					NConvert.ToInt32(feeItemList.IsBaby).ToString(),//26�Ƿ�Ӥ����
					NConvert.ToInt32(feeItemList.IsEmergency).ToString(),//27�������ȱ�־
					((Neusoft.HISFC.Models.Order.Inpatient.Order)feeItemList.Order).OrderType.ID,//28��Ժ���Ʊ��
					feeItemList.Invoice.ID,//29���㷢Ʊ��
					feeItemList.BalanceNO.ToString(),//30�������
					feeItemList.AuditingNO,//31������
					feeItemList.ChargeOper.ID,//32������
					feeItemList.ChargeOper.OperTime.ToString(),//33��������
					feeItemList.MachineNO,//34�豸��
					feeItemList.FeeOper.ID,//35�Ʒ���
					feeItemList.FeeOper.OperTime.ToString(),//36�Ʒ�����
					feeItemList.AuditingNO,//37�������
					feeItemList.Order.ID,//39ҽ����ˮ��
					feeItemList.ExecOrder.ID,//40ҽ��ִ�е���ˮ��
					feeItemList.ExecOper.ID,//43ִ����
					feeItemList.ExecOper.OperTime.ToString(),//44ִ������
					feeItemList.Item.PriceUnit,//��ǰ��λ
					feeItemList.SendSequence.ToString(),// ���ⵥ���к�
					feeItemList.UpdateSequence.ToString(),//�ۿ���ˮ��	
					feeItemList.NoBackQty.ToString(),//46��������
					feeItemList.BalanceState,//47����״̬
					feeItemList.FTRate.ItemRate.ToString(),//48�շѱ���
					feeItemList.FeeOper.Dept.ID,//49 �շ�Ա����
					feeItemList.Item.SpecialFlag,		//50 ��չ���
					feeItemList.Item.SpecialFlag1,		//51 ��չ���1
					feeItemList.Item.SpecialFlag2,		//52 ��չ���2
					//feeItemList.User01,//53 ��չ����
                    //{D6A25CA7-331A-4034-BBC6-A6FF821E290C}
                    feeItemList.ExtCode,
					feeItemList.User02,//54 ��չ����Ա����
					feeItemList.User03, //55 ��չ����
                    ((int)feeItemList.Item.ItemType).ToString() ,//0��ҩƷ��2����
                    //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
                    feeItemList.MedicalTeam.ID,
                    // ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
                    feeItemList.OperationNO,
                    feeItemList.CancelRecipeNO //{9C9F3289-C6E9-4c52-9077-FBF8ABDB4DE5}
                   
				};

            return args;
        }

        /// <summary>
        /// ͨ��ҩƷʵ����ʵ����������
        /// </summary>
        /// <param name="patient">��Ա������Ϣ</param>
        /// <param name="medItemList">ҩƷ���û�����Ϣ</param>
        /// <returns>ҩƷʵ����ʵ����������</returns>
        private string[] GetMedItemListParams(PatientInfo patient, FeeItemList medItemList)
        {
            medItemList.Patient = patient.Clone();

            string[] args = 
				{
					medItemList.RecipeNO,
					medItemList.SequenceNO.ToString(),
					((int)medItemList.TransType).ToString(),
					medItemList.Patient.ID,
					medItemList.Patient.Name,
					medItemList.Patient.Pact.PayKind.ID,
					medItemList.Patient.Pact.ID,
					((Neusoft.HISFC.Models.RADT.PatientInfo)medItemList.Patient).PVisit.PatientLocation.Dept.ID,
					((Neusoft.HISFC.Models.RADT.PatientInfo)medItemList.Patient).PVisit.PatientLocation.NurseCell.ID,
					medItemList.RecipeOper.Dept.ID,
					medItemList.ExecOper.Dept.ID,
					medItemList.StockOper.Dept.ID,
					medItemList.RecipeOper.ID,
					medItemList.Item.ID,
					medItemList.Item.MinFee.ID,
					medItemList.Compare.CenterItem.ID,
					medItemList.Item.Name,
					medItemList.Item.Specs,
					((Neusoft.HISFC.Models.Pharmacy.Item)medItemList.Item).Type.ID,
					((Neusoft.HISFC.Models.Pharmacy.Item)medItemList.Item).Quality.ID,
					NConvert.ToInt32(((Neusoft.HISFC.Models.Pharmacy.Item)medItemList.Item).Product.IsSelfMade).ToString(),
					medItemList.Item.Price.ToString(),
					medItemList.Item.PriceUnit,
					medItemList.Item.PackQty.ToString(),
					medItemList.Item.Qty.ToString(),
					medItemList.Days.ToString(),
					medItemList.FT.TotCost.ToString(),
					medItemList.FT.OwnCost.ToString(),
					medItemList.FT.PayCost.ToString(),
					medItemList.FT.PubCost.ToString(),
					medItemList.FT.RebateCost.ToString(),
                    ((int)medItemList.PayType).ToString(),
                    NConvert.ToInt32(medItemList.IsBaby).ToString(),
                    NConvert.ToInt32(medItemList.IsEmergency).ToString(),
                    ((Models.Order.Inpatient.Order)medItemList.Order).OrderType.ID,
					medItemList.Invoice.ID,
					medItemList.BalanceNO.ToString(),
					medItemList.ApproveNO,
					medItemList.ChargeOper.ID,
					medItemList.ChargeOper.OperTime.ToString(),
					medItemList.ExecOper.ID,
					medItemList.ExecOper.OperTime.ToString(),
					medItemList.AuditingNO,
					medItemList.Order.ID,
					medItemList.ExecOrder.ID,
					medItemList.FeeOper.ID,
                    medItemList.FeeOper.OperTime.ToString(),
                    medItemList.SendSequence.ToString(),
                    medItemList.UpdateSequence.ToString(),
					medItemList.NoBackQty.ToString(),
					medItemList.BalanceState,
					medItemList.FTRate.OwnRate.ToString(),	
					medItemList.FeeOper.Dept.ID,		
					medItemList.Item.SpecialFlag,	
					medItemList.Item.SpecialFlag1,
					medItemList.Item.SpecialFlag2,
                    medItemList.ExtCode,
                    medItemList.ExtOper.ID,
                    medItemList.ExtOper.OperTime.ToString(),
                      //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
                    medItemList.MedicalTeam.ID,
                    //{0604764A-3F55-428f-9064-FB4C53FD8136}
                    medItemList.OperationNO,
                    medItemList.CancelRecipeNO  //{9C9F3289-C6E9-4c52-9077-FBF8ABDB4DE5}
				};

            return args;
        }

        /// <summary>
        /// ��ø��»�����Ϣ�����ֶμ���
        /// </summary>
        /// <param name="feeItemList">������Ϣ</param>
        /// <returns>���»�����Ϣ�����ֶμ���</returns>
        private string[] GetUpdateChargeItemParams(FeeItemList feeItemList)
        {
            string[] args = 
				{
					feeItemList.RecipeNO,
					feeItemList.SequenceNO.ToString(),
					feeItemList.Item.Qty.ToString(),
					feeItemList.FT.TotCost.ToString(),
					feeItemList.FT.OwnCost.ToString(),
					feeItemList.FT.PayCost.ToString(),
					feeItemList.FT.PubCost.ToString(),
					feeItemList.ChargeOper.ID,
					feeItemList.ChargeOper.OperTime.ToString()
				};

            return args;
        }

        /// <summary>
        /// ��ø��»��ۺ��շ���Ϣ�����ֶμ���
        /// </summary>
        /// <param name="feeItemList">������Ϣ</param>
        /// <returns>���»��ۺ��շ���Ϣ�����ֶμ���</returns>
        private string[] GetUpdateChargeItemToFeeParmas(FeeItemList feeItemList)
        {
            string[] args = 
				{
					feeItemList.RecipeNO,
					feeItemList.SequenceNO.ToString(),
					feeItemList.FeeOper.ID,
					feeItemList.FeeOper.OperTime.ToString(),
					((int)feeItemList.PayType).ToString(),
					feeItemList.FeeOper.Dept.ID	
				};

            return args;
        }

        /// <summary>
        /// FeeItemListʵ��ת����FeeInfoʵ��
        /// </summary>
        /// <param name="feeItemList">��ϸʵ��</param>
        /// <returns>�ɹ� FeeInfoʵ�� ʧ��: -1</returns>
        private FeeInfo ConvertFeeItemListToFeeInfo(FeeItemList feeItemList) 
        {
            FeeInfo feeInfo = new FeeInfo();
            
            feeInfo.RecipeNO = feeItemList.RecipeNO;
			feeInfo.Item = feeItemList.Item;
            feeInfo.TransType = feeItemList.TransType;
			feeInfo.Patient = feeItemList.Patient;
			feeInfo.RecipeOper = feeItemList.RecipeOper;
			feeInfo.ExecOper = feeItemList.ExecOper;
		    feeInfo.StockOper = feeItemList.StockOper;
			feeInfo.FT = feeItemList.FT;
			feeInfo.ChargeOper = feeItemList.ChargeOper;
			feeInfo.FeeOper = feeItemList.FeeOper;
			feeInfo.BalanceOper = feeItemList.BalanceOper;
			feeInfo.Invoice = feeItemList.Invoice;
            feeInfo.BalanceNO = feeItemList.BalanceNO;
			feeInfo.BalanceState = feeItemList.BalanceState;
			feeInfo.AuditingNO = feeItemList.AuditingNO;
			feeInfo.IsBaby = feeItemList.IsBaby;

            return feeInfo;
        }

        /// <summary>
        /// ��÷��û�����Ϣʵ���ֶ�����
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeInfo">���û�����Ϣʵ��</param>
        /// <returns>�û�����Ϣʵ���ֶ�����</returns>
        private string[] GetFeeInfoParams(PatientInfo patient, FeeInfo feeInfo)
        {
            feeInfo.Patient = patient.Clone();

            if (feeInfo.FeeOper.Dept.ID == null || feeInfo.FeeOper.Dept.ID == string.Empty)
            {
                feeInfo.FeeOper.Dept.ID = this.GetDeptByEmplId(feeInfo.FeeOper.ID);
            }

            string[] args = 
				{
					feeInfo.RecipeNO,
					feeInfo.Item.MinFee.ID,
					((int)feeInfo.TransType).ToString(),
					feeInfo.Patient.ID,
					feeInfo.Patient.Name,
					feeInfo.Patient.Pact.PayKind.ID,
					feeInfo.Patient.Pact.ID,
					((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.Dept.ID,
					((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.NurseCell.ID,
					feeInfo.RecipeOper.Dept.ID,
					feeInfo.ExecOper.Dept.ID,
					feeInfo.StockOper.Dept.ID,
					feeInfo.RecipeOper.ID,
					feeInfo.FT.TotCost.ToString(),
					feeInfo.FT.OwnCost.ToString(),
					feeInfo.FT.PayCost.ToString(),
					feeInfo.FT.PubCost.ToString(),
					feeInfo.FT.RebateCost.ToString(),
					feeInfo.ChargeOper.ID,
					feeInfo.ChargeOper.OperTime.ToString(),
					feeInfo.FeeOper.ID,
					feeInfo.FeeOper.OperTime.ToString(),
					feeInfo.BalanceOper.ID,
					feeInfo.BalanceOper.OperTime.ToString(),
					feeInfo.Invoice.ID,
					feeInfo.BalanceNO.ToString(),
					feeInfo.BalanceState,
					feeInfo.AuditingNO,
					NConvert.ToInt32(feeInfo.IsBaby).ToString(),
					feeInfo.FeeOper.Dept.ID,	//����Ա����
					feeInfo.ExtFlag,			//��չ���
					feeInfo.ExtFlag1,			//��չ���1
					feeInfo.ExtFlag2,			//��չ���2
					feeInfo.ExtCode,			//��չ����
					feeInfo.ExecOper.ID,		//��չ������Ա
					feeInfo.ExecOper.OperTime.ToString()	
				};

            return args;
        }

        /// <summary>
        /// ��÷��û�����Ϣʵ���ֶ�����(������)
        /// </summary>
        /// <param name="feeInfo">���û�����Ϣʵ��</param>
        /// <returns>���û�����Ϣʵ���ֶ�����</returns>
        private string[] GetFeeInfoUpdateParams(FeeInfo feeInfo)
        {
            string[] args = 
				{
					feeInfo.FT.TotCost.ToString(),
					feeInfo.FT.OwnCost.ToString(),
					feeInfo.FT.PayCost.ToString(),
					feeInfo.FT.PubCost.ToString(),
					feeInfo.RecipeNO,
					feeInfo.Item.MinFee.ID,
					feeInfo.ExecOper.Dept.ID,
					feeInfo.FT.RebateCost.ToString()
				};

            return args;
        }

        #endregion

        #region ���������Ϣ

        /// <summary>
        /// ��ý�����ϸ��Ϣ��������
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="balanceList">���߽�����ϸ��Ϣ</param>
        /// <returns>������ϸ��Ϣ��������</returns>
        private string[] GetBalanceListParams(PatientInfo patient, BalanceList balanceList)
        {
            ((Balance)balanceList.BalanceBase).Patient = patient.Clone();

            if (((Balance)balanceList.BalanceBase).BalanceOper.Dept.ID == null || ((Balance)balanceList.BalanceBase).BalanceOper.Dept.ID == string.Empty)
            {
                ((Balance)balanceList.BalanceBase).BalanceOper.Dept.ID = this.GetDeptByEmplId(((Balance)balanceList.BalanceBase).BalanceOper.ID);
            }

            string[] args = 
				{
					((Balance)balanceList.BalanceBase).Invoice.ID,
					((int)((Balance)balanceList.BalanceBase).TransType).ToString(),
					((Balance)balanceList.BalanceBase).Patient.ID,
					((Balance)balanceList.BalanceBase).Patient.Name,
					((Balance)balanceList.BalanceBase).Patient.Pact.PayKind.ID,
					((Balance)balanceList.BalanceBase).Patient.Pact.ID,
					((PatientInfo)((Balance)balanceList.BalanceBase).Patient).PVisit.PatientLocation.Dept.ID,
					balanceList.FeeCodeStat.StatCate.ID,
					balanceList.FeeCodeStat.StatCate.Name,
					balanceList.FeeCodeStat.SortID.ToString(),
					((Balance)balanceList.BalanceBase).FT.TotCost.ToString(),
					((Balance)balanceList.BalanceBase).FT.OwnCost.ToString(),
					((Balance)balanceList.BalanceBase).FT.PayCost.ToString(),
					((Balance)balanceList.BalanceBase).FT.PubCost.ToString(),
					((Balance)balanceList.BalanceBase).FT.RebateCost.ToString(),
					((Balance)balanceList.BalanceBase).FT.OwnCost.ToString(),
					((Balance)balanceList.BalanceBase).BalanceOper.ID,
					((Balance)balanceList.BalanceBase).BalanceOper.OperTime.ToString(),
					((Balance)balanceList.BalanceBase).BalanceType.ID.ToString(),
					((Balance)balanceList.BalanceBase).ID,
					NConvert.ToInt32(((Balance)balanceList.BalanceBase).Patient.IsBaby).ToString(),
					((Balance)balanceList.BalanceBase).AuditingNO,
					((Balance)balanceList.BalanceBase).BalanceOper.Dept.ID
				};

            return args;
        }

        /// <summary>
        /// ��ý�����Ϣ��������
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="balance">���߽�����Ϣ</param>
        /// <returns>������Ϣ��������</returns>
        private string[] GetBalanceParams(PatientInfo patient, Balance balance)
        {
            balance.Patient = patient.Clone();

            if (balance.BalanceOper.Dept.ID == null || balance.BalanceOper.Dept.ID == string.Empty)
            {
                balance.BalanceOper.Dept.ID = this.GetDeptByEmplId(balance.BalanceOper.ID);
            }

            string[] args = 
				{
					balance.Invoice.ID,
					((int)balance.TransType).ToString(),
					balance.Patient.ID,
					balance.ID,
					balance.Patient.Pact.PayKind.ID,
					balance.Patient.Pact.ID,
					balance.FT.PrepayCost.ToString(),
					balance.FT.TotCost.ToString(),
					balance.FT.OwnCost.ToString(),
					balance.FT.PayCost.ToString(),
					balance.FT.PubCost.ToString(),
					balance.FT.RebateCost.ToString(),
					balance.FT.DerateCost.ToString(),
					((int)balance.CancelType).ToString(),
					balance.FT.SupplyCost.ToString(),
					balance.FT.ReturnCost.ToString(),
					balance.FT.TransferPrepayCost.ToString(),
					balance.BeginTime.ToString(),
					balance.EndTime.ToString(),
					balance.BalanceType.ID.ToString(),
					balance.BalanceOper.ID,
					balance.BalanceOper.OperTime.ToString(),
					balance.FinanceGroup.ID,
					balance.PrintTimes.ToString(),
					"0",
					"0",
					"0",
					"0",
					"0",
					balance.AuditingNO,
					NConvert.ToInt32(balance.IsMainInvoice).ToString(),
					NConvert.ToInt32(balance.IsLastBalance).ToString(),
					balance.Patient.Name,
					balance.BalanceOper.Dept.ID,
					balance.FT.AdjustOvertopCost.ToString()
				};

            return args;
        }

        #endregion

        #region ����֧����Ϣ

        /// <summary>
        /// ���֧����Ϣ��������
        /// </summary>
        /// <param name="balancePay">֧��ʵ��</param>
        /// <returns>֧����Ϣ��������</returns>
        private string[] GetBalancePayParams(BalancePay balancePay)
        {
            string[] args = 
				{
					balancePay.Invoice.ID,
					((int)balancePay.TransType).ToString(),
					balancePay.TransKind.ID,
					balancePay.PayType.ID.ToString(),
					balancePay.FT.TotCost.ToString(),
					balancePay.Qty.ToString(),
					balancePay.Bank.ID,
					balancePay.Bank.Name,
					balancePay.Bank.Account,
					balancePay.BalanceNO.ToString(),
					balancePay.RetrunOrSupplyFlag,
					balancePay.BalanceOper.ID,
					balancePay.BalanceOper.OperTime.ToString()
				};

            return args;
        }

        #endregion

        #region ���뵣����Ϣ

        /// <summary>
        /// ��õ�����Ϣ��������
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <returns>������Ϣ��������</returns>
        private string[] GetPatientSurtyParmas(PatientInfo patient)
        {
            string[] args =
				{
                  patient.ID,//סԺ��ˮ��
                  patient.PVisit.PatientLocation.Dept.ID,//���Ҵ���
                  patient.Surety.SuretyPerson.ID,//�����˱���
                  patient.Surety.SuretyPerson.Name,//����������
                  patient.Surety.SuretyCost.ToString(),//�������
                  patient.Surety.SuretyType.ID.ToString(),//��������
                  patient.Surety.ApplyPerson.ID,//����˱���
                  patient.Surety.ApplyPerson.Name,//���������
                  patient.Surety.Mark,//��ע
                  patient.Surety.Oper.ID,//����Ա
                  //{43B68F1F-988A-44ff-9C95-2EDCB7ACC5A9}
                  patient.Surety.PayType.ID,
                  patient.Surety.State,
                  patient.Surety.Bank.ID,
                  patient.Surety.Bank.Account,
                  patient.Surety.Bank.WorkName,
                  patient.Surety.Bank.InvoiceNO,
                  patient.Surety.InvoiceNO,
                  patient.Surety.OldInvoiceNO

				};
            return args;
        }

        #endregion

        #region �����ת��Ϣ

        /// <summary>
        /// ��ý�ת���Լ���
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="feeInfo">���û���ʵ��</param>
        /// <returns>��ת���Լ���</returns>
        private string[] GetCarryFowardFeeParams(PatientInfo patient, FeeInfo feeInfo)
        {
            feeInfo.Patient = patient.Clone();

            string[] args = 
				{
					feeInfo.RecipeNO,
					feeInfo.Item.MinFee.ID,
					((int)feeInfo.TransType).ToString(),
					feeInfo.Patient.ID,
					feeInfo.Patient.Name,
					feeInfo.Patient.Pact.PayKind.ID,
					feeInfo.Patient.Pact.ID,
					((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.Dept.ID,
					((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.NurseCell.ID,
					feeInfo.StockOper.Dept.ID,
					feeInfo.ExecOper.Dept.ID,
					feeInfo.StockOper.Dept.ID,
					feeInfo.RecipeOper.ID,
					feeInfo.FT.TotCost.ToString(),
					feeInfo.ChargeOper.ID,
					feeInfo.ChargeOper.OperTime.ToString(),
					feeInfo.FeeOper.ID,
					feeInfo.FeeOper.OperTime.ToString(),
					feeInfo.BalanceOper.ID,
					feeInfo.BalanceOper.OperTime.ToString(),
					feeInfo.BalanceNO.ToString(),
					feeInfo.BalanceState,
					feeInfo.AuditingNO,
					NConvert.ToInt32(feeInfo.IsBaby).ToString()
				};

            return args;
        }

        #endregion

        #region ���ò�ѯ

        /// <summary>
        /// ��÷�������Ϣ
        /// </summary>
        /// <param name="index">SQL����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:��÷�������Ϣ ʧ��:null</returns>
        private ArrayList QueryFeeInfoGroups(string index, params string[] args)
        {
            string sql = string.Empty;//SQL���

            if (this.Sql.GetSql(index, ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ: " + index + "��SQL���";

                return null;
            }

            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList feeInfos = new ArrayList();//FeeInfo����
            FeeInfo feeInfo = null;//����ʵ��

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    feeInfo = new FeeInfo();

                    feeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    feeInfo.FT.TotCost = NConvert.ToDecimal(this.Reader[1].ToString());
                    feeInfo.FT.OwnCost = NConvert.ToDecimal(this.Reader[2].ToString());
                    feeInfo.FT.PubCost = NConvert.ToDecimal(this.Reader[3].ToString());
                    feeInfo.FT.PayCost = NConvert.ToDecimal(this.Reader[4].ToString());
                    feeInfo.FT.RebateCost = NConvert.ToDecimal(this.Reader[5].ToString());

                    feeInfos.Add(feeInfo);
                }

                this.Reader.Close();

                return feeInfos;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        #endregion

        #region ��ϸ��ѯ

        /// <summary>
        /// ��ȡ����fin_com_itemlist��ȫ�����ݵ�sql
        /// </summary>
        /// <returns>�ɹ�:����fin_com_itemlist��ȫ�����ݵ�sql ʧ��:null</returns>
        private string GetFeeItemsSelectSql()
        {
            string sql = string.Empty;//SQly���

            if (this.Sql.GetSql("Fee.SelectAllFromFeeItem.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.SelectAllFromFeeItem.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ��÷�����Ŀ��Ϣ
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�: ��÷�����Ŀ��Ϣ ʧ��: null</returns>
        private ArrayList QueryFeeItemListsBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList feeItemLists = new ArrayList();//������ϸ��Ϣ����
            FeeItemList itemList = null;//������ϸʵ��

            try
            {
                while (this.Reader.Read())
                {
                    itemList = new FeeItemList();

                    itemList.RecipeNO = this.Reader[0].ToString();//0 ������
                    itemList.SequenceNO = NConvert.ToInt32(this.Reader[1].ToString());//1��������Ŀ��ˮ��
                    itemList.TransType = (TransTypes)NConvert.ToInt32(Reader[2].ToString());//2��������,1�����ף�2������
                    itemList.ID = this.Reader[3].ToString();//3סԺ��ˮ��
                    itemList.Patient.ID = this.Reader[3].ToString();//3סԺ��ˮ��
                    itemList.Name = this.Reader[4].ToString();//4����
                    itemList.Patient.Name = this.Reader[4].ToString();//4����
                    itemList.Patient.Pact.PayKind.ID = this.Reader[5].ToString();//5�������
                    itemList.Patient.Pact.ID = this.Reader[6].ToString();//6��ͬ��λ
                    itemList.UpdateSequence = NConvert.ToInt32(this.Reader[7].ToString());//7���¿�����ˮ��(����)
                    ((PatientInfo)itemList.Patient).PVisit.PatientLocation.Dept.ID = this.Reader[8].ToString();//8��Ժ���Ҵ���
                    ((PatientInfo)itemList.Patient).PVisit.PatientLocation.NurseCell.ID = this.Reader[9].ToString();//9��ʿվ����
                    itemList.RecipeOper.Dept.ID = this.Reader[10].ToString();//10�������Ҵ���
                    itemList.ExecOper.Dept.ID = this.Reader[11].ToString();//11ִ�п��Ҵ���
                    itemList.StockOper.Dept.ID = this.Reader[12].ToString();//12�ۿ���Ҵ���
                    itemList.RecipeOper.ID = this.Reader[13].ToString();//13����ҽʦ����
                    itemList.Item.ID = this.Reader[14].ToString();//14��Ŀ����
                    itemList.Item.MinFee.ID = this.Reader[15].ToString();//15��С���ô���
                    itemList.Compare.CenterItem.ID = this.Reader[16].ToString();//16���Ĵ���
                    itemList.Item.Name = this.Reader[17].ToString();//17��Ŀ����
                    itemList.Item.Price = NConvert.ToDecimal(this.Reader[18].ToString());//18����
                    itemList.Item.Qty = NConvert.ToDecimal(this.Reader[19].ToString());//19����
                    itemList.Item.PriceUnit = this.Reader[20].ToString();//20��ǰ��λ
                    itemList.UndrugComb.ID = this.Reader[21].ToString();//21���״���
                    itemList.UndrugComb.Name = this.Reader[22].ToString();//22��������
                    itemList.FT.TotCost = NConvert.ToDecimal(this.Reader[23].ToString());//23���ý��
                    itemList.FT.OwnCost = NConvert.ToDecimal(this.Reader[24].ToString());//24�Էѽ��
                    itemList.FT.PayCost = NConvert.ToDecimal(this.Reader[25].ToString());//25�Ը����
                    itemList.FT.PubCost = NConvert.ToDecimal(this.Reader[26].ToString());//26���ѽ��
                    itemList.FT.RebateCost = NConvert.ToDecimal(this.Reader[27].ToString());//27�Żݽ��
                    itemList.SendSequence = NConvert.ToInt32(this.Reader[28].ToString());//28���ⵥ���к�
                    itemList.PayType = (PayTypes)NConvert.ToInt32(this.Reader[29].ToString());//29�շ�״̬
                    itemList.IsBaby = NConvert.ToBoolean(this.Reader[30].ToString());//30�Ƿ�Ӥ����
                    ((Models.Order.Inpatient.Order)itemList.Order).OrderType.ID = this.Reader[32].ToString();
                    itemList.Invoice.ID = this.Reader[33].ToString();//33���㷢Ʊ��
                    itemList.BalanceNO = NConvert.ToInt32(this.Reader[34].ToString());//34�������
                    itemList.ChargeOper.ID = this.Reader[36].ToString();//36������
                    itemList.ChargeOper.OperTime = NConvert.ToDateTime(this.Reader[37].ToString());//37��������
                    itemList.MachineNO = this.Reader[39].ToString();//39�豸��
                    itemList.ExecOper.ID = this.Reader[40].ToString();//40ִ���˴���
                    itemList.ExecOper.OperTime = NConvert.ToDateTime(this.Reader[41].ToString());//41ִ������
                    itemList.FeeOper.ID = this.Reader[42].ToString();//42�Ʒ���
                    itemList.FeeOper.OperTime = NConvert.ToDateTime(this.Reader[43].ToString());//43�Ʒ�����
                    itemList.AuditingNO = this.Reader[45].ToString();//45������
                    itemList.Order.ID = this.Reader[46].ToString();//46ҽ����ˮ��
                    itemList.ExecOrder.ID = this.Reader[47].ToString();//47ҽ��ִ�е���ˮ��
                    //itemList.Item.IsPharmacy = false;
                    //itemList.Item.ItemType = //HISFC.Models.Base.EnumItemType.UnDrug;
                    itemList.NoBackQty = NConvert.ToDecimal(this.Reader[48].ToString());//48��������
                    itemList.BalanceState = this.Reader[49].ToString();//49����״̬
                    itemList.FTRate.ItemRate = NConvert.ToDecimal(this.Reader[50].ToString());//50�շѱ���
                    itemList.FeeOper.Dept.ID = this.Reader[51].ToString();//51�շ�Ա����
                    itemList.FTSource = this.Reader[54].ToString();
                    if (itemList.Item.PackQty == 0) 
                    {
                        itemList.Item.PackQty = 1;
                    }
                    itemList.Item.ItemType = (Neusoft.HISFC.Models.Base.EnumItemType)(NConvert.ToInt32(this.Reader[58]));
                    //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
                    itemList.MedicalTeam.ID = this.Reader[59].ToString();
                    // ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
                    itemList.OperationNO = this.Reader[60].ToString();
                    //��ȡ�˷Ѵ����ţ���������ˮ��{F7581BFE-6680-4e3f-90FA-3F07778E8821}
                    itemList.CancelRecipeNO = this.Reader[61].ToString();
                    itemList.CancelSequenceNO = NConvert.ToInt32(this.Reader[1].ToString());
                    //{D6A25CA7-331A-4034-BBC6-A6FF821E290C}
                    itemList.ExtCode = this.Reader[55].ToString();
                    feeItemLists.Add(itemList);
                }

                this.Reader.Close();

                return feeItemLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��÷�����Ŀ��Ϣ
        /// </summary>
        /// <param name="whereIndex">where��������</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�: ��÷�����Ŀ��Ϣ ʧ��: null</returns>
        private ArrayList QueryFeeItemLists(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetFeeItemsSelectSql();

            return this.QueryFeeItemListsBySql(sql + " " + where, args);
        }

        /// <summary>
        /// ��ȡ����fin_com_medicinelist��ȫ�����ݵ�sql
        /// </summary>
        /// <returns>�ɹ�: ��ȡ����fin_com_medicinelist��ȫ�����ݵ�sql ʧ��:null</returns>
        public string GetMedItemListSelectSql()
        {
            string sql = string.Empty;

            if (this.Sql.GetSql("Fee.SelectAllFromMedItem.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.SelectAllFromMedItem.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ���ҩƷ������Ŀ��Ϣ
        /// </summary>
        /// <param name="sql">SQl���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:���ҩƷ������Ŀ��Ϣ ʧ��: null</returns>
        public ArrayList QueryMedItemListsBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList medItemLists = new ArrayList();//ҩƷ��ϸ����
            FeeItemList itemList = null;//ҩƷ��ϸʵ��

            try
            {
                while (this.Reader.Read())
                {
                    itemList = new FeeItemList();

                    Neusoft.HISFC.Models.Pharmacy.Item pharmacyItem = new Neusoft.HISFC.Models.Pharmacy.Item();
                    itemList.Item = pharmacyItem;

                    itemList.RecipeNO = this.Reader[0].ToString();//0 ������
                    itemList.SequenceNO = NConvert.ToInt32(this.Reader[1].ToString());//1��������Ŀ��ˮ��
                    itemList.TransType = (TransTypes)NConvert.ToInt32(this.Reader[2].ToString());//2��������,1�����ף�2������
                    itemList.ID = this.Reader[3].ToString();//3סԺ��ˮ��
                    itemList.Patient.ID = this.Reader[3].ToString();//3סԺ��ˮ��
                    itemList.Name = this.Reader[4].ToString();//4����
                    itemList.Patient.Name = this.Reader[4].ToString();//4����
                    itemList.Patient.Pact.PayKind.ID = this.Reader[5].ToString();//5�������
                    itemList.Patient.Pact.ID = this.Reader[6].ToString();//6��ͬ��λ
                    itemList.UpdateSequence = NConvert.ToInt32(this.Reader[7].ToString());//7���¿�����ˮ��(����)
                    ((PatientInfo)itemList.Patient).PVisit.PatientLocation.Dept.ID = this.Reader[8].ToString();//8��Ժ���Ҵ���
                    ((PatientInfo)itemList.Patient).PVisit.PatientLocation.NurseCell.ID = this.Reader[9].ToString();//9��ʿվ����
                    itemList.RecipeOper.Dept.ID = this.Reader[10].ToString();//10�������Ҵ���
                    itemList.ExecOper.Dept.ID = this.Reader[11].ToString();//11ִ�п��Ҵ���
                    itemList.StockOper.Dept.ID = this.Reader[12].ToString();//12�ۿ���Ҵ���
                    itemList.RecipeOper.ID = this.Reader[13].ToString();//13����ҽʦ����
                    itemList.Item.ID = this.Reader[14].ToString();//14��Ŀ����
                    itemList.Item.MinFee.ID = this.Reader[15].ToString();//15��С���ô���
                    itemList.Compare.CenterItem.ID = this.Reader[14].ToString();//16���Ĵ���
                    itemList.Item.Name = this.Reader[17].ToString();//17��Ŀ����
                    itemList.Item.Price = NConvert.ToDecimal(this.Reader[18].ToString());//18����1
                    itemList.Item.Qty = NConvert.ToDecimal(this.Reader[19].ToString());//9����
                    itemList.Item.PriceUnit = this.Reader[20].ToString();//20��ǰ��λ
                    itemList.Item.PackQty = NConvert.ToDecimal(this.Reader[21].ToString());//21��װ����
                    itemList.Days = NConvert.ToDecimal(this.Reader[22].ToString());//22����
                    itemList.FT.TotCost = NConvert.ToDecimal(this.Reader[23].ToString());//23���ý��
                    itemList.FT.OwnCost = NConvert.ToDecimal(this.Reader[24].ToString());//24�Էѽ��
                    itemList.FT.PayCost = NConvert.ToDecimal(this.Reader[25].ToString());//25�Ը����
                    itemList.FT.PubCost = NConvert.ToDecimal(this.Reader[26].ToString());//26���ѽ��
                    itemList.FT.RebateCost = NConvert.ToDecimal(this.Reader[27].ToString());//27�Żݽ��
                    itemList.SendSequence = NConvert.ToInt32(this.Reader[28].ToString());//28���ⵥ���к�
                    itemList.PayType = (PayTypes)NConvert.ToInt32(this.Reader[29].ToString());//29�շ�״̬
                    itemList.IsBaby = NConvert.ToBoolean(this.Reader[30].ToString());//30�Ƿ�Ӥ����
                    ((Models.Order.Inpatient.Order)itemList.Order).OrderType.ID = this.Reader[32].ToString();//32��Ժ���Ʊ��
                    itemList.Invoice.ID = this.Reader[33].ToString();//33���㷢Ʊ��
                    itemList.BalanceNO = NConvert.ToInt32(this.Reader[34].ToString());//34�������
                    itemList.ChargeOper.ID = this.Reader[36].ToString();//36������
                    itemList.ChargeOper.OperTime = NConvert.ToDateTime(this.Reader[37].ToString());//37��������
                    pharmacyItem.Product.IsSelfMade = NConvert.ToBoolean(this.Reader[38].ToString());//38���Ʊ�ʶ
                    pharmacyItem.Quality.ID = this.Reader[39].ToString();//39ҩƷ����
                    itemList.ExecOper.ID = this.Reader[40].ToString();//40��ҩ�˴���
                    itemList.ExecOper.OperTime = NConvert.ToDateTime(this.Reader[41].ToString());//41��ҩ����
                    itemList.FeeOper.ID = this.Reader[42].ToString();//42�Ʒ���
                    itemList.FeeOper.OperTime = NConvert.ToDateTime(this.Reader[43].ToString());//43�Ʒ�����
                    itemList.AuditingNO = this.Reader[45].ToString();//45������
                    itemList.Order.ID = this.Reader[46].ToString();//46ҽ����ˮ��
                    itemList.ExecOrder.ID = this.Reader[47].ToString();//47ҽ��ִ�е���ˮ��
                    pharmacyItem.Specs = this.Reader[48].ToString();//���
                    pharmacyItem.Type.ID = this.Reader[49].ToString();//49ҩƷ���
                    //pharmacyItem.IsPharmacy = true;
                    pharmacyItem.ItemType = HISFC.Models.Base.EnumItemType.Drug;
                    itemList.NoBackQty = NConvert.ToDecimal(this.Reader[50].ToString());//50��������
                    itemList.BalanceState = this.Reader[51].ToString();//51����״̬
                    itemList.FTRate.ItemRate = NConvert.ToDecimal(this.Reader[52].ToString());//52�շѱ���
                    itemList.FTRate.OwnRate = itemList.FTRate.ItemRate;

                    itemList.FeeOper.Dept.ID = this.Reader[53].ToString();//53�շ�Ա����
                    //itemList.Item.IsPharmacy = true;
                    itemList.Item.ItemType = HISFC.Models.Base.EnumItemType.Drug;
                    itemList.FTSource = this.Reader[56].ToString();
                    //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
                    itemList.MedicalTeam.ID = this.Reader[57].ToString();
                    // ��������{0604764A-3F55-428f-9064-FB4C53FD8136}
                    itemList.OperationNO = this.Reader[58].ToString();
                    //��ȡ�˷Ѵ����ţ���������ˮ��{F7581BFE-6680-4e3f-90FA-3F07778E8821}
                    itemList.CancelRecipeNO = this.Reader[62].ToString();
                    itemList.CancelSequenceNO = NConvert.ToInt32(this.Reader[1].ToString());
                    //{D6A25CA7-331A-4034-BBC6-A6FF821E290C}
                    itemList.ExtCode = this.Reader[57].ToString();

                    medItemLists.Add(itemList);
                }

                this.Reader.Close();

                return medItemLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ���ҩƷ������Ŀ��Ϣ
        /// </summary>
        /// <param name="whereIndex">where��������</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�: ���ҩƷ������Ŀ��Ϣ ʧ��: null</returns>
        private ArrayList QueryMedItemLists(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetMedItemListSelectSql();

            return this.QueryMedItemListsBySql(sql + " " + where, args);
        }

        /// <summary>
        /// ��ȡ����feeinfo��sql
        /// </summary>
        /// <returns>�ɹ�:����SQL��� ʧ��:null</returns>
        private string GetSqlForSelectFeeInfo()
        {
            string sql = string.Empty;//Sql���

            if (this.Sql.GetSql("GetSqlForSelectFeeInfo.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:GetSqlForSelectFeeInfo.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ��ȡfeeinfo�еĸ��ֶε�ֵ ע��˳��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:��÷��û�����Ϣ ʧ��:null</returns>
        private ArrayList QueryFeeInfosBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList feeInfos = new ArrayList();//���û���ʵ�弯��
            FeeInfo feeInfo = null;//���û���ʵ��

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    feeInfo = new FeeInfo();

                    feeInfo.RecipeNO = this.Reader[0].ToString();//0������
                    feeInfo.Item.MinFee.ID = this.Reader[1].ToString();//1��С���ô���
                    feeInfo.TransType = (TransTypes)NConvert.ToInt32(Reader[2].ToString());//2��������
                    feeInfo.Patient.Pact.PayKind.ID = this.Reader[3].ToString();//3�������
                    feeInfo.Patient.Pact.ID = this.Reader[4].ToString();//4��ͬ��λ
                    ((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.Dept.ID = this.Reader[5].ToString();//5��Ժ���Ҵ���
                    ((PatientInfo)feeInfo.Patient).PVisit.PatientLocation.NurseCell.ID = this.Reader[6].ToString();//6��ʿվ����
                    ((Neusoft.HISFC.Models.RADT.PatientInfo)feeInfo.Patient).PVisit.PatientLocation.NurseCell.ID = this.Reader[6].ToString();//6��ʿվ����
                    feeInfo.RecipeOper.Dept.ID = this.Reader[7].ToString();//7�������Ҵ���
                    feeInfo.ExecOper.Dept.ID = this.Reader[8].ToString();//8ִ�п��Ҵ���
                    feeInfo.StockOper.Dept.ID = this.Reader[9].ToString();//9�ۿ���Ҵ���
                    feeInfo.RecipeOper.ID = this.Reader[10].ToString();//10����ҽʦ����
                    feeInfo.FT.TotCost = NConvert.ToDecimal(this.Reader[11].ToString());//11���ý��
                    feeInfo.FT.OwnCost = NConvert.ToDecimal(this.Reader[12].ToString());//12�Էѽ��
                    feeInfo.FT.PayCost = NConvert.ToDecimal(this.Reader[13].ToString());//13�Ը����
                    feeInfo.FT.PubCost = NConvert.ToDecimal(this.Reader[14].ToString());//14���ѽ��
                    feeInfo.FT.RebateCost = NConvert.ToDecimal(this.Reader[15].ToString());//15�Żݽ��
                    feeInfo.ChargeOper.ID = this.Reader[16].ToString();//16������
                    feeInfo.ChargeOper.OperTime = NConvert.ToDateTime(this.Reader[17].ToString());//17��������
                    feeInfo.FeeOper.ID = this.Reader[18].ToString();//18�Ʒ���
                    feeInfo.FeeOper.OperTime = NConvert.ToDateTime(this.Reader[19].ToString());//19�Ʒ�����
                    feeInfo.BalanceOper.ID = this.Reader[20].ToString();//20�����˴���
                    feeInfo.BalanceOper.OperTime = NConvert.ToDateTime(this.Reader[21].ToString());//21����ʱ��
                    feeInfo.Invoice.ID = this.Reader[22].ToString();//22���㷢Ʊ��
                    feeInfo.BalanceNO = NConvert.ToInt32(this.Reader[23].ToString());//23�������
                    feeInfo.BalanceState = this.Reader[24].ToString();//24�����־
                    feeInfo.AuditingNO = this.Reader[25].ToString();//25������
                    feeInfo.IsBaby = NConvert.ToBoolean(this.Reader[26].ToString());//26Ӥ�����
                    feeInfo.FeeOper.Dept.ID = this.Reader[27].ToString();//27�շ�Ա����
                    feeInfo.ExtFlag = this.Reader[28].ToString();//28��չ���
                    feeInfo.ExtFlag1 = this.Reader[29].ToString();//��չ���1
                    feeInfo.ExtFlag2 = this.Reader[30].ToString();//��չ���2
                    feeInfo.ExtCode = this.Reader[31].ToString();//��չ����
                    feeInfo.ExecOper.ID = this.Reader[32].ToString();//��չ����Ա
                    feeInfo.ExecOper.OperTime = NConvert.ToDateTime(this.Reader[33].ToString());//��չ����

                    feeInfos.Add(feeInfo);
                }//ѭ������

                this.Reader.Close();

                return feeInfos;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��÷��û�����Ϣ
        /// </summary>
        /// <param name="whereIndex">where��������</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�: ��÷��û�����Ϣ ʧ��: null</returns>
        private ArrayList QueryFeeInfos(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetSqlForSelectFeeInfo();

            return this.QueryFeeInfosBySql(sql + " " + where, args);
        }

        #endregion

        #region �����ѯ

        /// <summary>
        /// ��ȡ����fin_ipb_balanceHead��ȫ�����ݵ�sql
        /// </summary>
        /// <returns>�ɹ�:����SQL��� ʧ��:null</returns>
        private string GetSqlForSelectAllInfoFromBalanceHead()
        {
            string sql = string.Empty;//SQL���

            if (this.Sql.GetSql("Fee.GetSqlForSelectAllInfoFromBalanceHead.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetSqlForSelectAllInfoFromBalanceHead.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ��ý�����Ϣ
        /// </summary>
        /// <param name="sql">��ѯ��SQL���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:����ͷ����Ϣ ʧ��:null û�в��ҵ�����ArrayList.Count = 0</returns>
        private ArrayList QueryBalancesBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList balances = new ArrayList();//����ͷʵ�弯��
            Balance balance = null;//����ͷʵ��

            try
            {
                //��ʼѭ����ȡ����
                while (this.Reader.Read())
                {
                    balance = new Balance();

                    balance.Invoice.ID = this.Reader[0].ToString();//0��Ʊ����
                    balance.TransType = (TransTypes)NConvert.ToInt32(Reader[1].ToString());//1��������
                    balance.Patient.ID = this.Reader[2].ToString();//2סԺ��ˮ��
                    balance.ID = this.Reader[3].ToString();//3�������
                    balance.Patient.Pact.PayKind.ID = this.Reader[4].ToString();//4�������
                    balance.Patient.Pact.ID = this.Reader[5].ToString();//5��ͬ����
                    balance.FT.PrepayCost = NConvert.ToDecimal(this.Reader[6].ToString());//6Ԥ�����
                    balance.FT.TransferPrepayCost = NConvert.ToDecimal(this.Reader[7].ToString());//7ת��Ԥ�����
                    balance.FT.TotCost = NConvert.ToDecimal(this.Reader[8].ToString());//8���ý��
                    balance.FT.OwnCost = NConvert.ToDecimal(this.Reader[9].ToString());//9�Էѽ��
                    balance.FT.PayCost = NConvert.ToDecimal(this.Reader[10].ToString());//10�Ը����
                    balance.FT.PubCost = NConvert.ToDecimal(this.Reader[11].ToString());//11���ѽ��
                    balance.FT.RebateCost = NConvert.ToDecimal(this.Reader[12].ToString());//12�Żݽ��
                    balance.FT.DerateCost = NConvert.ToDecimal(this.Reader[13].ToString());//13������
                    balance.FT.TransferTotCost = NConvert.ToDecimal(this.Reader[14].ToString());//14ת����ý��
                    balance.FT.SupplyCost = NConvert.ToDecimal(this.Reader[15].ToString());//15���ս��
                    balance.FT.ReturnCost = NConvert.ToDecimal(this.Reader[16].ToString());//16�������
                    balance.FT.TransferPrepayCost = NConvert.ToDecimal(this.Reader[17].ToString());//17תѺ��
                    balance.BeginTime = NConvert.ToDateTime(this.Reader[18].ToString());//18��ʼ����
                    balance.EndTime = NConvert.ToDateTime(this.Reader[19].ToString());//19��ֹ����
                    balance.BalanceType.ID = this.Reader[20].ToString();//20��������
                    balance.BalanceOper.ID = this.Reader[21].ToString();//21�����˴���
                    balance.BalanceOper.OperTime = NConvert.ToDateTime(this.Reader[22].ToString());//22����ʱ��
                    balance.FinanceGroup.ID = this.Reader[23].ToString();//23���������
                    balance.PrintTimes = NConvert.ToInt32(this.Reader[24].ToString());//24��ӡ����
                    balance.AuditingNO = this.Reader[25].ToString();//25������
                    balance.CancelType = (CancelTypes)NConvert.ToInt32(Reader[26].ToString());//26���ϱ�־
                    balance.IsMainInvoice = NConvert.ToBoolean(this.Reader[27].ToString());//27����Ʊ���
                    balance.IsLastBalance = NConvert.ToBoolean(this.Reader[28].ToString());//28����������������
                    balance.Name = this.Reader[29].ToString();//29 ��������
                    balance.Patient.Name = this.Reader[29].ToString();//29 ��������
                    balance.BalanceOper.Dept.ID = this.Reader[30].ToString();//30����Ա����
                    balance.FT.AdjustOvertopCost = NConvert.ToDecimal(this.Reader[31].ToString());//31��������������޶����
                    balance.CancelOper.ID = this.Reader[32].ToString();//32���ϲ���Ա
                    balance.CancelOper.OperTime = NConvert.ToDateTime(this.Reader[33].ToString());//33����ʱ��

                    balances.Add(balance);
                }//ѭ������

                this.Reader.Close();

                return balances;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��ý�����Ϣ
        /// </summary>
        /// <param name="whereIndex">Where����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:����ͷ����Ϣ ʧ��:null û�в��ҵ�����ArrayList.Count = 0</returns>
        private ArrayList QueryBalances(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetSqlForSelectAllInfoFromBalanceHead();

            return this.QueryBalancesBySql(sql + " " + where, args);
        }

        /// <summary>
        /// ��ȡ����fin_ipb_balancelist��ȫ�����ݵ�sql
        /// </summary>
        /// <returns>�ɹ�:����SQL��� ʧ��:null</returns>
        private string GetSqlForSelectAllInfoFromBalanceList()
        {
            string sql = string.Empty;

            if (this.Sql.GetSql("Fee.GetSqlForSelectAllInfoFromBalanceList.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetSqlForSelectAllInfoFromBalanceList.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ��ý�����ϸ��Ϣ
        /// </summary>
        /// <param name="sql">Sql���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:��ý�����ϸ��Ϣ ʧ��:null û�в��ҵ�����ArrayList.Count = 0</returns>
        private ArrayList QueryBalanceListsBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList balanceLists = new ArrayList();//������ϸʵ�弯��
            BalanceList balanceList = null;//������ϸʵ��

            try
            {
                //ѭ������
                while (this.Reader.Read())
                {
                    balanceList = new BalanceList();

                    ((Balance)balanceList.BalanceBase).Invoice.ID = this.Reader[0].ToString();//0��Ʊ����
                    ((Balance)balanceList.BalanceBase).TransType = (TransTypes)NConvert.ToInt32(this.Reader[1].ToString());//1��������
                    ((Balance)balanceList.BalanceBase).Patient.ID = this.Reader[2].ToString();//2סԺ��ˮ��
                    ((Balance)balanceList.BalanceBase).Patient.Pact.PayKind.ID = this.Reader[4].ToString();//4�������
                    ((Balance)balanceList.BalanceBase).Patient.Pact.ID = this.Reader[5].ToString();//5��ͬ����6
                    balanceList.FeeCodeStat.StatCate.ID = this.Reader[7].ToString();//7ͳ�ƴ���
                    balanceList.FeeCodeStat.StatCate.Name = this.Reader[8].ToString();//8ͳ�ƴ�������
                    balanceList.FeeCodeStat.SortID = NConvert.ToInt32(this.Reader[9].ToString());//9��ӡ˳���
                    ((Balance)balanceList.BalanceBase).FT.TotCost = NConvert.ToDecimal(this.Reader[10].ToString());//10���ý��
                    ((Balance)balanceList.BalanceBase).FT.OwnCost = NConvert.ToDecimal(this.Reader[11].ToString());//11�Էѽ��
                    ((Balance)balanceList.BalanceBase).FT.PayCost = NConvert.ToDecimal(this.Reader[12].ToString());//12�Ը����
                    ((Balance)balanceList.BalanceBase).FT.PubCost = NConvert.ToDecimal(this.Reader[13].ToString());//13���ѽ��
                    ((Balance)balanceList.BalanceBase).FT.RebateCost = NConvert.ToDecimal(this.Reader[14].ToString());//14�Żݽ��
                    ((Balance)balanceList.BalanceBase).BalanceOper.ID = this.Reader[15].ToString();//15�����˴���
                    ((Balance)balanceList.BalanceBase).BalanceOper.OperTime = NConvert.ToDateTime(this.Reader[16].ToString());//16����ʱ��
                    ((Balance)balanceList.BalanceBase).BalanceType.ID = this.Reader[17].ToString();//17��������
                    ((Balance)balanceList.BalanceBase).ID = this.Reader[18].ToString();//18�������
                    balanceList.ID = this.Reader[18].ToString();//18�������
                    //blist.IsBaby=NConvert.ToBoolean(this.Reader[19].ToString());
                    ((Balance)balanceList.BalanceBase).AuditingNO = this.Reader[20].ToString();// 20������
                    ((Balance)balanceList.BalanceBase).BalanceOper.Dept.ID = this.Reader[21].ToString();// 21����Ա����

                    balanceLists.Add(balanceList);
                }

                this.Reader.Close();

                return balanceLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��ý�����ϸ��Ϣ
        /// </summary>
        /// <param name="whereIndex">Where����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:������ϸ��Ϣ ʧ��:null û�в��ҵ�����ArrayList.Count = 0</returns>
        private ArrayList QueryBalanceLists(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetSqlForSelectAllInfoFromBalanceList();

            return this.QueryBalanceListsBySql(sql + " " + where, args);
        }

        #endregion

        #region ֧����ʽ

        /// <summary>
        /// ��ȡ����balancepay��sql
        /// </summary>
        /// <returns>�ɹ�: ����SQL��� ʧ��:null</returns>
        private string GetSqlForSelectBalancePay()
        {
            string sql = string.Empty;

            if (this.Sql.GetSql("Fee.GetSqlForSelectBalancePay.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetSqlForSelectBalancePay.1��SQL���";

                return null;
            }

            return sql;
        }

        /// <summary>
        /// ��ý���ʵ����Ϣ
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:֧����Ϣ���� ʧ��:null</returns>
        private ArrayList QueryBalancePaysBySql(string sql, params string[] args)
        {
            if (this.ExecQuery(sql, args) == -1)
            {
                return null;
            }

            ArrayList balancePays = new ArrayList();//֧����ʽ����
            BalancePay balancePay = null;//֧����ʽʵ��

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    balancePay = new BalancePay();

                    balancePay.Invoice.ID = this.Reader[0].ToString();//0��Ʊ��
                    balancePay.TransType = (TransTypes)NConvert.ToInt32(this.Reader[1].ToString());//1��������
                    balancePay.TransKind.ID = this.Reader[2].ToString();//2��������
                    balancePay.PayType.ID = this.Reader[3].ToString();//3֧����ʽ
                    balancePay.BalanceNO = NConvert.ToInt32(this.Reader[4].ToString());//4�������
                    balancePay.FT.TotCost = NConvert.ToDecimal(this.Reader[5].ToString());//5���
                    balancePay.Qty = NConvert.ToInt32(this.Reader[6].ToString());//6����
                    //ת��Ԥ����
                    balancePay.Bank.ID = this.Reader[8].ToString();//8��������
                    balancePay.Bank.WorkName = this.Reader[9].ToString();//10�����е�λ
                    balancePay.Bank.Account = this.Reader[10].ToString();//9�������ʺ�
                    balancePay.Bank.InvoiceNO = this.Reader[11].ToString();//11֧Ʊ��
                    balancePay.RetrunOrSupplyFlag = this.Reader[12].ToString();//12���ز��ձ��
                    balancePay.BalanceOper.ID = this.Reader[13].ToString();//������
                    balancePay.BalanceOper.OperTime = NConvert.ToDateTime(this.Reader[14].ToString());//14��������
                    balancePay.Bank.Name = this.Reader[15].ToString();//15����������

                    balancePays.Add(balancePay);
                }//ѭ������

                this.Reader.Close();

                return balancePays;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ���֧����Ϣ
        /// </summary>
        /// <param name="whereIndex">where����</param>
        /// <param name="args">����</param>
        /// <returns>�ɹ�:���֧����Ϣ���� ʧ��: null</returns>
        private ArrayList QueryBalancePays(string whereIndex, params string[] args)
        {
            string sql = string.Empty;//SELECT���
            string where = string.Empty;//WHERE���

            //���Where���
            if (this.Sql.GetSql(whereIndex, ref where) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:" + whereIndex + "��SQL���";

                return null;
            }

            sql = this.GetSqlForSelectBalancePay();

            return this.QueryBalancePaysBySql(sql + " " + where, args);
        }

        #endregion

        #endregion

        #region ���з���

        #region ������ɾ�Ĳ���

        /// <summary>
        /// ��ӻ�����Ŀ����-�����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="feeItemList">���ķ�����Ŀ��Ϣ</param>
        ///  <param name="Insurence">ҽ������Ŀ�����Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertFeeItemList(PatientInfo patient, FeeItemList feeItemList, Neusoft.HISFC.Models.Insurance.IInsurence Insurence)
        {
            if (feeItemList.Patient.Pact.ID == null || feeItemList.Patient.Pact.ID.Trim() == string.Empty)
            {
                feeItemList.Patient.Pact.ID = patient.Pact.ID;
            }

            if (feeItemList.Patient.Pact.PayKind.ID == null || feeItemList.Patient.Pact.PayKind.ID.Trim() == string.Empty)
            {
                feeItemList.Patient.Pact.PayKind.ID = patient.Pact.PayKind.ID;
            }

            return this.UpdateSingleTable("AddPatientAccount.1", this.GetFeeItemListParams(patient, feeItemList));
        }

        /// <summary>
        /// ��ӻ�����Ŀ����-�����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="feeItemList">���ķ�����Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertFeeItemList(PatientInfo patient, FeeItemList feeItemList)
        {
            return this.InsertFeeItemList(patient, feeItemList, null);
        }

        /// <summary>
        /// ��ӻ�����Ŀ����-�����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="feeItemList">���ķ�����Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertFeeItemList(FeeItemList feeItemList)
        {
            return this.InsertFeeItemList(((PatientInfo)feeItemList.Patient), feeItemList);
        }

        /// <summary>
        /// ��ӻ�����Ŀ����-����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="medItemList">ҩƷ������Ŀ��Ϣ</param>
        /// <param name="Insurance">ҽ������Ŀ�����Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertMedItemList(PatientInfo patient, FeeItemList medItemList, Neusoft.HISFC.Models.Insurance.IInsurence Insurance)
        {
            if (medItemList.Patient.Pact.ID == null || medItemList.Patient.Pact.ID.Trim() == string.Empty)
            {
                medItemList.Patient.Pact.ID = patient.Pact.ID;
            }

            if (medItemList.Patient.Pact.PayKind.ID == null || medItemList.Patient.Pact.PayKind.ID.Trim() == string.Empty)
            {
                medItemList.Patient.Pact.PayKind.ID = patient.Pact.PayKind.ID;
            }

            return this.UpdateSingleTable("AddPatientAccount.2", this.GetMedItemListParams(patient, medItemList));
        }

        ///<summary>
        /// ��ӻ�����Ŀ����-����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="medItemList">ҩƷ������Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertMedItemList(PatientInfo patient, FeeItemList medItemList)
        {
            return this.InsertMedItemList(patient, medItemList, null);
        }

        ///<summary>
        /// ��ӻ�����Ŀ����-����ҩƷ������ϸ����Ϣ
        /// </summary>
        /// <param name="medItemList">ҩƷ������Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertMedItemList(FeeItemList medItemList)
        {
            return this.InsertMedItemList(((PatientInfo)medItemList.Patient), medItemList);
        }

        /// <summary>
        /// ���·�ҩƷ������ϸ����ⵥ�źͿۿ����ˮ��
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="updateSequence">���¿����ˮ��</param>
        /// <param name="sendSequence">���ⵥ���к�</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateFeeItemSequence(string recipeNO, int recipeSequence, int updateSequence, int sendSequence)
        {
            return this.UpdateSingleTable("UpdateFeeItemSequence.1", recipeNO, recipeSequence.ToString(), updateSequence.ToString(), sendSequence.ToString());
        }

        /// <summary>
        /// ��ҩʱ���¸���ҩƷ������ϸ����ⵥ�źͿۿ����ˮ��
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="updateSequence">���¿����ˮ��</param>
        /// <param name="sendSequence">���ⵥ���к�</param>
        /// <param name="sendDrugDept">��ҩ����</param>
        /// <param name="sendDrugOperCode">��ҩ�˴���</param>
        /// <param name="sendDrugTime">��ҩʱ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateMedItemExecInfo(string recipeNO, int recipeSequence, int updateSequence, int sendSequence, string sendDrugDept, string sendDrugOperCode, DateTime sendDrugTime)
        {
            return this.UpdateSingleTable("UpdateMedItemSequence.1", recipeNO, recipeSequence.ToString(), updateSequence.ToString(), sendSequence.ToString(),
                sendDrugDept, sendDrugOperCode, sendDrugTime.ToString());
        }
        /// <summary>
        /// ��ҩʱ���¸���ҩƷ������ϸ����ⵥ�źͿۿ����ˮ��
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="updateSequence">���¿����ˮ��</param>
        /// <param name="sendSequence">���ⵥ���к�</param>
        /// <param name="sendState">�Ƿ�ҩ</param>
        /// <param name="sendDrugDept">��ҩ����</param>
        /// <param name="sendDrugOperCode">��ҩ�˴���</param>
        /// <param name="sendDrugTime">��ҩʱ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateMedItemExecInfo(string recipeNO, int recipeSequence, int updateSequence, int sendSequence,int sendState, string sendDrugDept, string sendDrugOperCode, DateTime sendDrugTime)
        {
            return this.UpdateSingleTable("UpdateMedItemSequence.2", recipeNO, recipeSequence.ToString(), updateSequence.ToString(), sendSequence.ToString(),
                sendState.ToString(), sendDrugDept, sendDrugOperCode, sendDrugTime.ToString());
        }    

        /// <summary>
        /// ɾ��������Ϣ
        /// </summary>
        /// <param name="itemList">��Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int DeleteChargeInfo(FeeItemList itemList)
        {
            //if (itemList.Item.IsPharmacy)
            if (itemList.Item.ItemType == HISFC.Models.Base.EnumItemType.Drug)
            {
                return this.UpdateSingleTable("Fee.DeleteDrugCharge.1", itemList.RecipeNO, itemList.SequenceNO.ToString());
            }
            else
            {
                return this.UpdateSingleTable("Fee.DeleteUndrugCharge.1", itemList.RecipeNO, itemList.SequenceNO.ToString());
            }
        }

        /// <summary>
        /// ���Ļ��ۺ����Ϣ
        /// </summary>
        /// <param name="itemList">��Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateChargeInfo(FeeItemList itemList)
        {
            //if (itemList.Item.IsPharmacy)
            if (itemList.Item.ItemType == EnumItemType.Drug)
            {
                return this.UpdateSingleTable("Fee.UpdateChargeInfoForDrug.1", this.GetUpdateChargeItemParams(itemList));
            }
            else
            {
                return this.UpdateSingleTable("Fee.UpdateChargeInfoForUnDrug.1", this.GetUpdateChargeItemParams(itemList));
            }
        }

        /// <summary>
        /// ���ۺ�����շѱ���շ����շ�ʱ�������Ϣ
        /// </summary>
        /// <param name="itemList">��Ŀ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateChargeItemToFee(FeeItemList itemList)
        {
            //if (itemList.Item.IsPharmacy)
            if(itemList.Item.ItemType == EnumItemType.Drug)
            {
                return this.UpdateSingleTable("UpdateDrugItem.2", this.GetUpdateChargeItemToFeeParmas(itemList));
            }
            else
            {
                return this.UpdateSingleTable("UpdateUndrugItem.2", this.GetUpdateChargeItemToFeeParmas(itemList));
            }
        }

        /// <summary>
        /// ���·�����ϢΪ����״̬ Written By ���峬--------------��ʱû��,����Ч���������ֱ��ȫ��update
        /// </summary>
        /// <param name="feeInfo">������Ϣ</param>
        /// <param name= "balance">������Ϣ</param>>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateFeeInfoBalanced(FeeInfo feeInfo, Balance balance)
        {
            return this.UpdateSingleTable("UpdateAccoutBalanced.1", feeInfo.RecipeNO, feeInfo.ExecOper.Dept.ID, feeInfo.Item.MinFee.ID,
                balance.ID, balance.Oper.ID, balance.Oper.OperTime.ToString());
        }

        /// <summary>
        /// ���»���ҩƷ��ϸ�����������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="isApp">�Ƿ�����</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateEmergencyForDrug(string recipeNO, int recipeSequence, bool isApp)
        {
            return this.UpdateSingleTable("Fee.UpdateEmergencyForDrug", recipeNO, recipeSequence.ToString(), NConvert.ToInt32(isApp).ToString());
        }

        /// <summary>
        /// ���»��߷�ҩƷ��ϸ�����������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="isApp">�Ƿ�����</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateEmergencyForUndrug(string recipeNO, int recipeSequence, bool isApp)
        {
            return this.UpdateSingleTable("Fee.UpdateEmergencyForUndrug", recipeNO, recipeSequence.ToString(), NConvert.ToInt32(isApp).ToString());
        }

        /// <summary>
        /// ���»��߷�����Ϣ-�����
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateInMainInfoFee(string inpatientNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.InPatient.UpdateAccount.10", inpatientNO, ft.OwnCost.ToString(), ft.PubCost.ToString(),
                ft.PayCost.ToString(), ft.TotCost.ToString(), ft.RebateCost.ToString());
        }
      
        /// <summary>
        /// ���»��߷�����Ϣ-�����(ҽ��)
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateInMainInfoFeeYB(string inpatientNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.InPatient.UpdateAccount.8", inpatientNO, ft.OwnCost.ToString(), ft.PubCost.ToString(),
                ft.PayCost.ToString(), ft.TotCost.ToString(), ft.RebateCost.ToString());
        }

        /// <summary>
        /// ���»��߷�����Ϣ-�����
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateInMainInfoFeeForDirQuit(string inpatientNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.InPatient.UpdateAccount.ForDirQuit", inpatientNO, ft.OwnCost.ToString(), ft.PubCost.ToString(),
                ft.PayCost.ToString(), ft.TotCost.ToString(), ft.RebateCost.ToString());
        }

        /// <summary>
        /// ����ҩƷ��ϸ��������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="qty">��������</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateNoBackQtyForDrug(string recipeNO, int recipeSequence, decimal qty, string balanceState)
        {
            return this.UpdateSingleTable("Fee.UpdateNoBackNumForDrug.1", recipeNO, recipeSequence.ToString(), qty.ToString(), balanceState);
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ��������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="qty">��������</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateNoBackQtyForUndrug(string recipeNO, int recipeSequence, decimal qty, string balanceState)
        {
            return this.UpdateSingleTable("Fee.UpdateNoBackNumForUndrug.1", recipeNO, recipeSequence.ToString(), qty.ToString(), balanceState);
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ��չ���
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="extFlag2">��չ���2</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateExtFlagForUndrug(string recipeNO, int recipeSequence, string extFlag2, string balanceState)
        {
            return this.UpdateSingleTable("Fee.UpdateExtFlagForUndrug.1", recipeNO, recipeSequence.ToString(), extFlag2, balanceState);
        }

        /// <summary>
        /// ������������ý��----Ϊ����ɽҽҽ��Ԥ������  ������������ֱ�ӵ���Ft�и���ֵ
        /// </summary>
        /// <param name="patient">��Ա������Ϣ</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateInmaininfoFeeForMedicare(PatientInfo patient, FT ft)
        {
            return this.UpdateSingleTable("Fee.UpdateInmaininfoFeeForMedicare", patient.ID, ft.TotCost.ToString(), ft.OwnCost.ToString(), ft.PayCost.ToString(), ft.PubCost.ToString());
        }

        /// <summary>
        /// �������յ���ӡ���
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdatePrintFlag(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.UpdateSingleTable("Fee.InPatient.UpdatePrintFlag", inpatientNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        ///����FeeInfo��Ϣ
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="feeItemList">������ϸ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertFeeInfo(PatientInfo patient, FeeItemList feeItemList)
        {
            return this.UpdateSingleTable("Fee.Inpatient.AddPatientAccount.2", this.GetFeeInfoParams(patient, this.ConvertFeeItemListToFeeInfo(feeItemList)));
        }
        
        /// <summary>
        ///����FeeInfo��Ϣ
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="feeInfo">���û�����Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�������: 0</returns>
        public int InsertFeeInfo(PatientInfo patient, FeeInfo feeInfo)
        {
            return this.UpdateSingleTable("Fee.Inpatient.AddPatientAccount.2", this.GetFeeInfoParams(patient, feeInfo));
        }

        /// <summary>
        /// ���뻼�߷��û�����Ϣ,��������ظ�,��ô���¸������û�����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="feeItemList">������ϸ��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertAndUpdateFeeInfo(PatientInfo patient, FeeItemList feeItemList)
        {
            int returnValue = 0;

            returnValue = this.InsertFeeInfo(patient, feeItemList);

            //���������Ϣ�������ظ�,��ô���¸������û�����Ϣ
            if (returnValue == -1 && this.DBErrCode == 1)
            {
                return this.UpdateSingleTable("Fee.Inpatient.AddPatientAccount.3", this.GetFeeInfoParams(patient, this.ConvertFeeItemListToFeeInfo(feeItemList)));
            }

            return returnValue;
        }

        /// <summary>
        /// ���뻼�߷��û�����Ϣ,��������ظ�,��ô���¸������û�����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="feeInfo">���û�����Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertAndUpdateFeeInfo(PatientInfo patient, FeeInfo feeInfo)
        {
            int returnValue = 0;

            returnValue = this.InsertFeeInfo(patient, feeInfo);

            //���������Ϣ�������ظ�,��ô���¸������û�����Ϣ
            if (returnValue == -1 && this.DBErrCode == 1)
            {
                return this.UpdateSingleTable("Fee.Inpatient.AddPatientAccount.3", this.GetFeeInfoUpdateParams(feeInfo));
            }

            return returnValue;
        }

        /// <summary>
        /// ��ӽ�����ϸ��¼
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="balanceList">������ϸ��¼��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertBalanceList(PatientInfo patient, BalanceList balanceList)
        {
            return this.UpdateSingleTable("AddBalanceList.1", this.GetBalanceListParams(patient, balanceList));
        }

        /// <summary>
        /// ��ӽ����¼
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="balance">�����¼��Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertBalance(PatientInfo patient, Balance balance)
        {
            return this.UpdateSingleTable("AddBalanceHead.1", this.GetBalanceParams(patient, balance));
        }

        /// <summary>
        /// ��ӽ���֧����Ϣ
        /// </summary>
        /// <param name="balancePay">֧����Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertBalancePay(BalancePay balancePay)
        {
            return this.UpdateSingleTable("AddBalancePay.1", this.GetBalancePayParams(balancePay));
        }

        /// <summary>
        /// ���½����������������������־
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="transType">��������</param>
        /// <param name="laseFalg">�������־</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int UpdateBalanceHeadLastFlag(string invoiceNO, string transType, string laseFalg)
        {
            return this.UpdateSingleTable("Fee.UpdateBalanceHeadLastFlag", invoiceNO, transType, laseFalg);
        }

        #endregion

        #region Ԥ����

        /// <summary>
        /// ����Ԥ�������(prepayʵ���е�Patient����û�и�ֵ)
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�в������� 0</returns>
        public int InsertPrepay(PatientInfo patient, Prepay prepay)
        {
            return this.UpdateSingleTable("Fee.Inpatient.Prepay.1", this.GetPrepayParams(patient, prepay));
        }

        /// <summary>
        ///  ����Ԥ�������(prepayʵ���е�Patient�����Ѿ���ֵ)
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�в������� 0</returns>
        public int InsertPrepay(Prepay prepay)
        {
            return this.InsertPrepay(prepay.Patient, prepay);
        }

        /// <summary>
        /// ���»�������Ԥ������Ϣ
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepay(PatientInfo patient, Prepay prepay)
        {
            return this.UpdateSingleTable("Fee.Inpatient.UpdateAccount11", patient.ID, prepay.FT.PrepayCost.ToString());
        }

        /// <summary>
        /// ���»�������Ԥ������Ϣ(prepayʵ���е�Patient�����Ѿ���ֵ,����Patient.ID��ֵ)
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepay(Prepay prepay)
        {
            return this.UpdatePrepay(prepay.Patient, prepay);
        }

        /// <summary>
        /// ����תѺ��Ʊ�ź�תѺ��״̬
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdateForgift(PatientInfo patient, Prepay prepay)
        {
            //{0E4732FC-5C33-4106-9784-2673D7D88316} תѺ���ӡת��Ԥ����Ʊ��
            //return this.UpdateSingleTable("UpdateForgift.1", patient.ID, prepay.ID, prepay.Invoice.ID, this.Operator.ID);
            return this.UpdateSingleTable("UpdateForgift.1", patient.ID, prepay.ID, prepay.RecipeNO, this.Operator.ID);
        }

        /// <summary>
        /// ����תѺ��Ʊ�ź�תѺ��״̬(prepayʵ���е�Patient�����Ѿ���ֵ,����Patient.ID��ֵ)
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdateForgift(Prepay prepay)
        {
            return this.UpdateForgift(prepay.Patient, prepay);
        }

        /// <summary>
        /// ���µ���Ԥ����-Ϊ����״̬ Written By ���峬
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepayBalanced(PatientInfo patient, Prepay prepay)
        {
            return this.UpdateSingleTable("Fee.Inpatient.Prepay.2", patient.ID, prepay.ID, prepay.BalanceOper.OperTime.ToString(),
                prepay.BalanceOper.ID, prepay.BalanceNO.ToString(), prepay.Invoice.ID);
        }

        /// <summary>
        /// ���µ���Ԥ����-Ϊ����״̬(prepayʵ���е�Patient�����Ѿ���ֵ,����Patient.ID��ֵ)
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepayBalanced(Prepay prepay)
        {
            return this.UpdatePrepayBalanced(prepay.Patient, prepay);
        }

        /// <summary>
        /// ����Ԥ���𷴻���־-Ϊ�Ѿ������򲹴�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepayHaveReturned(PatientInfo patient, Prepay prepay)
        {
            return this.UpdateSingleTable("Fee.Inpatient.Prepay.3", patient.ID, prepay.ID, prepay.PrepayState);
        }

        /// <summary>
        /// ����Ԥ���𷴻���־-Ϊ�Ѿ������򲹴�(prepayʵ���е�Patient�����Ѿ���ֵ,����Patient.ID��ֵ)
        /// </summary>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepayHaveReturned(Prepay prepay)
        {
            return this.UpdatePrepayHaveReturned(prepay.Patient, prepay);
        }

        /// <summary>
        /// ����Ԥ�����·�Ʊ��,�ͽ������.
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int UpdatePrepayForReprint(PatientInfo patient, Prepay prepay)
        {
            return this.UpdateSingleTable("Fee.Inpatient.Prepay.4", patient.ID, prepay.ID, prepay.Invoice.ID, prepay.BalanceNO.ToString());
        }

        /// <summary>
        /// Ԥ������ȡ,����-�������0��ȡ1����
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <param name="flag">������� 0��ȡ 1����</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        private int PrepayManager(PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.Prepay prepay, string flag)
        {
            if ((flag != "0") && (flag != "1"))
            {
                this.Err = "����������Ͳ���ȷ!";
                this.ErrCode = "����������Ͳ���ȷ!";

                return -1;
            }
            //����
            if (flag == "1")
            {
                prepay.FT.PrepayCost = -prepay.FT.PrepayCost;
                prepay.PrepayOper.ID = this.Operator.ID;
                prepay.PrepayOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(GetSysDateTime());
                prepay.PrepayState = "1";

                if (UpdatePrepayHaveReturned(patient, prepay) < 1)
                {
                    this.Err = this.Err + "������¼�Ѿ�������߽��й���������,����״̬����!";

                    return -1;
                }
            }
            //����Ԥ����
            if (this.InsertPrepay(patient, prepay) == -1)
            {
                this.Err = this.Err + "����Ԥ�������!����ΪAddPatientAccount";

                return -1;
            }
            //����Ԥ����
            if (UpdatePrepay(patient, prepay) == -1)
            {
                this.Err = this.Err + "����סԺ����ʧ��!����ΪUpdateAccount";

                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Ԥ������ȡ
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int PrepayManager(PatientInfo patient, Prepay prepay)
        {
            return this.PrepayManager(patient, prepay, "0");
        }
        /// <summary>
        /// Ԥ���𷵻�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="prepay">Ԥ����ʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� -1 û�и������� 0</returns>
        public int PrepayManagerReturn(PatientInfo patient, Prepay prepay)
        {
            return this.PrepayManager(patient, prepay, "1");
        }

        #endregion

        #region Ԥ�����ѯ

        /// <summary>
        /// Ԥ�����ѯ-ͨ��סԺ��ˮ�Ų�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��InpatientNo</param>
        /// <returns>�ɹ�:����Ԥ����ʵ�弯�� ʧ�� null</returns>
        public ArrayList QueryPrepays(string inpatientNO)
        {
            return this.QueryPrepays("Fee.Inpatient.Prepay.Get.3", inpatientNO);
        }

        /// <summary>
        /// ��ѯ������ЧԤ�����¼--For����-ͨ��סԺ��ˮ�Ų�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:����Ԥ����ʵ�弯�� ʧ�� null</returns>
        public ArrayList QueryPrepaysBalanced(string inpatientNO)
        {
            return this.QueryPrepays("Fee.QueryPrepayForBalance.1", inpatientNO);
        }

        /// <summary>
        ///  ��ȡδ��ӡתѺ��-ͨ��סԺ��ˮ�Ų�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�: δ��ӡתѺ�𼯺� ʧ�� null</returns>
        public ArrayList QueryForegif(string inpatientNO)
        {
            return this.QueryPrepays("Fee.Inpatient.Prepay.Get.2", inpatientNO);
        }

        /// <summary>
        /// ��ȡת��Ԥ�����ܽ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:ת��Ԥ�����ܽ�� ʧ��: -1</returns>
        public decimal GetTotChangePrepayCost(string inpatientNO)
        {
            return NConvert.ToDecimal(this.ExecSqlReturnOne("Fee.GetTotChangePrepayCost.1", inpatientNO));
        }

        /// <summary>
        /// ��ȡĳ�˽����Ԥ�����¼
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:����Ԥ����ʵ�弯�� ʧ�� null</returns>
        public ArrayList QueryPrepaysByInpatientNOAndBalanceNO(string inpatientNO, string balanceNO)
        {
            return this.QueryPrepays("Fee.GetPrepayByBalNo.1", inpatientNO, balanceNO);
        }

        /// <summary>
        /// ��ѯԤ������Ϣ
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="happenNO"></param>
        /// <returns></returns>
        public Prepay QueryPrePay(string inpatientNO, string happenNO)
        {
            ArrayList al = QueryPrepays("Fee.Inpatient.Prepay.Get.4", inpatientNO, happenNO);
            if (al == null || al.Count == 0)
            {
                return null;
            }
            return al[0] as Prepay;
        }
        #endregion

        #region ��ô�����

        /// <summary>
        /// ����·�ҩƷ������
        /// </summary>
        /// <returns>�ɹ�:��ҩƷ������ ʧ��:null</returns>
        public string GetUndrugRecipeNO()
        {
            string recipeNO = this.GetSequence("GetUndrugNewNoteNo.1");

            if (recipeNO != null)
            {
                return "F" + recipeNO.PadLeft(13, '0');
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �����ҩƷ������
        /// </summary>
        /// <returns>�ɹ�:ҩƷ������ ʧ��:null</returns>
        public string GetDrugRecipeNO()
        {
            string recipeNO = this.GetSequence("GetDrugNewNoteNo.1");

            if (recipeNO != null)
            {
                return "Y" + recipeNO.PadLeft(13, '0');
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ��÷�Ʊ��

        /// <summary>
        /// ����µķ�Ʊ�� Written By ���峬 ���ô洢����
        /// ���ݲ���Ա��ID�Ͳ�������� ����Ʊ��Ԥ����Ŀ ����ʣ�෢Ʊ��Ŀ ���ֲ��÷�Ʊ��Ŀ-Ԥ����Ŀ�ж���ʾ��ȡ��Ʊ
        /// ����Ʊ��Ԥ����Ŀ�Ķ�����ܱȽϲ��̶�����ҵ��㲻���ж� ���ص�ʣ����ĿĬ��10000 ֻ��С��Ԥ����Ŀ�Ż᷵��
        /// ��ʵ��Ŀ,�����������ֲ�ֻ������ж�,����Ƚϼ�Ҳ�Ƚ�ͨ��.
        /// </summary>
        /// <param name="OperatorID">����Ա����</param>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <param name="alarmQty">Ʊ��Ԥ����Ŀ</param>
        /// <param name="leftQty">ʣ�෢Ʊ��Ŀ</param>
        /// <param name="finGroupCode">���������</param>
        /// <returns>��Ʊ�� </returns>
        ///{18CCD3AA-4DA2-4d7c-BE29-54A7751B183F}
        [Obsolete("����",true)]
        public string GetNewInvoiceNO(string OperatorID, EnumInvoiceType invoiceType, int alarmQty, ref int leftQty, string finGroupCode)
        {
            string sql = string.Empty;//SQL���
            string returnString = string.Empty;//���ز���
            string[] temp;//��ʱ�洢���̷����ַ���
            string invoiceNO = string.Empty;//��Ʊ��
            int errCode = 0;//���ش������
            string errText = string.Empty;//�洢���̷��ش�����Ϣ

            if (this.Sql.GetSql("Fee.Inpatient.Invoice.Get.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.Inpatient.Invoice.Get.1��SQL���";

                return null;
            }
            try
            {
                sql = string.Format(sql, invoiceType.ToString(), OperatorID, leftQty, invoiceNO, errText, errCode, finGroupCode);
            }
            catch (Exception e)
            {
                this.Err = e.Message;

                return null;
            }

            if (this.ExecEvent(sql, ref returnString) == -1)
            {
                this.Err = "ִ�д洢���̳���!PRC_GET_INVOICE";
                this.WriteErr();

                return null;
            }

            temp = returnString.Split(',');
            invoiceNO = temp[0];
            errText = temp[1];

            errCode = NConvert.ToInt32(temp[2]);

            //������ش������Ϊ100˵��û���ҵ���Ʊ��
            if (errCode == 100)
            {
                this.Err ="����ȡ��Ʊ��";
                this.ErrCode = errCode.ToString();

                return null;
            }
            //������ش������Ϊ101˵���ҵ���Ʊ�ŵ���ʣ�෢Ʊ�ŵ���Ԥ����Ŀ
            if (errCode == 101)
            {
                leftQty = NConvert.ToInt32(errText);
            }
            else//���������÷�Ʊ��,��ôʣ�෢Ʊ��Ĭ��Ϊ10000
            {
                leftQty = 10000;
            }

            return invoiceNO;
        }

        #endregion

        #region ��÷�Ʊ��

        /// <summary>
        /// ����µķ�Ʊ�� Written By ���峬 ���ô洢����
        /// ���ݲ���Ա��ID�Ͳ�������� ����Ʊ��Ԥ����Ŀ ����ʣ�෢Ʊ��Ŀ ���ֲ��÷�Ʊ��Ŀ-Ԥ����Ŀ�ж���ʾ��ȡ��Ʊ
        /// ����Ʊ��Ԥ����Ŀ�Ķ�����ܱȽϲ��̶�����ҵ��㲻���ж� ���ص�ʣ����ĿĬ��10000 ֻ��С��Ԥ����Ŀ�Ż᷵��
        /// ��ʵ��Ŀ,�����������ֲ�ֻ������ж�,����Ƚϼ�Ҳ�Ƚ�ͨ��.
        /// </summary>
        /// <param name="OperatorID">����Ա����</param>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <param name="alarmQty">Ʊ��Ԥ����Ŀ</param>
        /// <param name="leftQty">ʣ�෢Ʊ��Ŀ</param>
        /// <param name="finGroupCode">���������</param>
        /// <returns>��Ʊ�� </returns>
        //{18CCD3AA-4DA2-4d7c-BE29-54A7751B183F}
        public string GetNewInvoiceNO(string OperatorID, string invoiceType, int alarmQty, ref int leftQty, string finGroupCode)
        {
            string sql = string.Empty;//SQL���
            string returnString = string.Empty;//���ز���
            string[] temp;//��ʱ�洢���̷����ַ���
            string invoiceNO = string.Empty;//��Ʊ��
            int errCode = 0;//���ش������
            string errText = string.Empty;//�洢���̷��ش�����Ϣ

            if (this.Sql.GetSql("Fee.Inpatient.Invoice.Get.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.Inpatient.Invoice.Get.1��SQL���";

                return null;
            }
            try
            {
                sql = string.Format(sql, invoiceType.ToString(), OperatorID, leftQty, invoiceNO, errText, errCode, finGroupCode);
            }
            catch (Exception e)
            {
                this.Err = e.Message;

                return null;
            }

            if (this.ExecEvent(sql, ref returnString) == -1)
            {
                this.Err = "ִ�д洢���̳���!PRC_GET_INVOICE";
                this.WriteErr();

                return null;
            }

            temp = returnString.Split(',');
            invoiceNO = temp[0];
            errText = temp[1];

            errCode = NConvert.ToInt32(temp[2]);

            //������ش������Ϊ100˵��û���ҵ���Ʊ��
            if (errCode == 100)
            {
                this.Err = "����ȡ��Ʊ��";
                this.ErrCode = errCode.ToString();

                return null;
            }
            //������ش������Ϊ101˵���ҵ���Ʊ�ŵ���ʣ�෢Ʊ�ŵ���Ԥ����Ŀ
            if (errCode == 101)
            {
                leftQty = NConvert.ToInt32(errText);
            }
            else//���������÷�Ʊ��,��ôʣ�෢Ʊ��Ĭ��Ϊ10000
            {
                leftQty = 10000;
            }

            return invoiceNO;
        }

        #endregion

        #region ���ò�ѯ

        /// <summary>
        /// ͨ����С���ô�������С��������
        /// </summary>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:��С�������� ʧ��: null</returns>
        public string GetMinFeeNameByCode(string minFeeCode)
        {
            return this.ExecSqlReturnOne("GetFeeNameByFeeCode.1", minFeeCode);
        }

        /// <summary>
        ///  ���Ӥ��������ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�:���Ӥ��������ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeForBaby(string inpatientNO, DateTime beginTime, DateTime endTime, string balanceState)
        {
            return this.QueryFeeInfoGroups("GetFeeInfosGroupByMinFeeForBaby.1", inpatientNO, beginTime.ToString(), endTime.ToString(), balanceState);
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeByInpatientNO(string inpatientNO, DateTime beginTime, DateTime endTime, string balanceState)
        {
            return this.QueryFeeInfoGroups("GetFeeInfosGroupbyMinFee.1", inpatientNO, beginTime.ToString(), endTime.ToString(), balanceState);
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// GetFeeInfosGroupbyMinFee.1
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeByInpatientNO(string inpatientNO, DateTime beginTime, string balanceState)
        {
            DateTime endTime = this.GetDateTimeFromSysDateTime();

            return this.QueryFeeInfosGroupByMinFeeByInpatientNO(inpatientNO, beginTime, endTime, balanceState);
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷��飩סԺ��ˮ��,�������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeByInpatientNO(string inpatientNO, int balanceNO)
        {
            return this.QueryFeeInfoGroups("GetFeeInfosGroupbyMinFee.2", inpatientNO, balanceNO.ToString());
        }

        /// <summary>
        /// ��÷�����ϢFeeInfoUnionת����ã���С���÷��飩-סԺ��ˮ��,����״̬
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceState">balanceState</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosAndChangeCostGroupByMinFeeByInpatientNO(string inpatientNO, string balanceState)
        {
            return this.QueryFeeInfoGroups("GetFeeInfosAndChangeCostGroupByMinFee.1", inpatientNO, balanceState);
        }

        /// <summary>
        ///  ��÷�����ϢFeeInfo����С���÷���)
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceState">����״̬</param>
        /// <param name="transType">�������ͣ�1������2������3ҩ�ѵ���</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeByInpatientNO(string inpatientNO, string balanceState, TransTypes transType)
        {
            return this.QueryFeeInfoGroups("Fee.InPatient.GetMinFeeInfoByPatientID", inpatientNO, balanceState, ((int)transType).ToString());
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷���)
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeForAdjustOverTopByInpatientNO(string inpatientNO)
        {
            return this.QueryFeeInfoGroups("Fee.InPatient.GetMinFeeForAdjustOverTop", inpatientNO);
        }

        /// <summary>
        /// ��ȡ���޶����������ϸ
        /// ������tot_cost=0,own_cost!=0
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosGroupByMinFeeForAdjustByInpatientNO(string inpatientNO)
        {
            return this.QueryFeeInfoGroups("Fee.InPatient.GetMinFeeInfoForAdjust", inpatientNO);
        }

        /// <summary>
        /// ����סԺ�ţ���ʼ����ʱ�����δ��ӡ�����յ�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosForPrintBill(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryFeeInfoGroups("Fee.InPatient.GetFeeInfosForPrintBill", inpatientNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ָ��ʱ�䵽����δ��ӡ�����յ�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosForPrintBill(string inpatientNO, DateTime beginTime)
        {
            DateTime endTime = this.GetDateTimeFromSysDateTime();

            return this.QueryFeeInfosForPrintBill(inpatientNO, beginTime, endTime);
        }

        /// <summary>
        /// ����סԺ�ţ���ʼ����ʱ������Ѵ�ӡ�����յ�--���ڲ���
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:��÷�����ϢFeeInfo����С���÷��飩 ʧ��:null</returns>
        public ArrayList QueryFeeInfosForPrinted(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryFeeInfoGroups("Fee.InPatient.GetFeeInfosForPrinted", inpatientNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ��û��ߵķ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="flag">"All"����, "Yes"���ϴ� "No"δ�ϴ�</param>
        /// <returns>�ɹ�:��÷�����Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemLists(string inpatientNO, DateTime beginTime, DateTime endTime, string flag)
        {
            string upload = string.Empty;//�Ƿ��ϴ����

            if (flag.ToUpper() == "ALL")//����
            {
                upload = "%";
            }
            else if (flag.ToUpper() == "YES")
            {
                upload = "1";
            }
            else
            {
                upload = "0";
            }

            return this.QueryFeeItemLists("Fee.GetMedItemsForInpatient.Where.Upload", inpatientNO, beginTime.ToString(), endTime.ToString(), upload);
        }

        /// <summary>
        /// ��û��߷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:��÷�����Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemLists(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryFeeItemLists("Fee.GetMedItemsForInpatient.Where.1", inpatientNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ��÷�ҩƷ������Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:��÷�����Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsForCharged(string inpatientNO)
        {
            return this.QueryFeeItemLists("Fee.GetUndrugChargeItems.1", inpatientNO);
        }

        /// <summary>
        /// ��û��ߵ�ҩƷ������Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="flag">"All"����, "Yes"���ϴ� "No"δ�ϴ�</param>
        /// <returns>�ɹ�:���ҩƷ������Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemLists(string inpatientNO, DateTime beginTime, DateTime endTime, string flag)
        {
            string upload = string.Empty;//�ϴ���־

            if (flag.ToUpper() == "ALL")//����
            {
                upload = "%";
            }
            else if (flag.ToUpper() == "YES")
            {
                upload = "1";
            }
            else
            {
                upload = "0";
            }

            return this.QueryMedItemLists("Fee.GetMedItemsForInpatient.Where.Upload", inpatientNO, beginTime.ToString(), endTime.ToString(), upload);
        }


        /// <summary>
        /// ��û���ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:���ҩƷ������Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList GetMedItemsForInpatient(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryMedItemLists("Fee.GetMedItemsForInpatient.Where.1", inpatientNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ���ҩƷ������Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�:���ҩƷ������Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListCharged(string inpatientNO)
        {
            return this.QueryMedItemLists("Fee.GetDrugChargeItems.1", inpatientNO);
        }

        /// <summary>
        /// �����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="isBalanced">�Ƿ����</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime, bool isBalanced)
        {
            return this.QueryFeeItemLists("Fee.GetForQuitUndrug.1", inpatientNO, beginTime.ToString(), endTime.ToString(), NConvert.ToInt32(isBalanced).ToString());
        }

        /// <summary>
        /// �����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryFeeItemListsCanQuit(inpatientNO, beginTime, endTime, false);
        }

        /// <summary>
        /// �����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isBalanced"></param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime, bool isBalanced, string minFeeCode)
        {
            return this.QueryFeeItemLists("Fee.GetForQuitUndrug.2", inpatientNO, beginTime.ToString(), endTime.ToString(),
                NConvert.ToInt32(isBalanced).ToString(), minFeeCode);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <param name="sendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalanced">�Ƿ����</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime, string sendDrugState, bool isBalanced)
        {
            return this.QueryMedItemLists("Fee.GetForQuitDrug.1", inpatientNO, beginTime.ToString(), endTime.ToString(), sendDrugState, NConvert.ToInt32(isBalanced).ToString());
        }

        /// <summary>
        /// ���ݷ���״̬��ȡһ��ʱ�䷶Χ��δ����Ĺ��ɹ��˷ѵ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <param name="sendDrugState">�Ƿ�ҩ</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime, string sendDrugState)
        {
            return this.QueryMedItemListsCanQuit(inpatientNO, beginTime, endTime, sendDrugState, false);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <param name="sendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalanced">�Ƿ����</param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsCanQuit(string inpatientNO, DateTime beginTime, DateTime endTime, string sendDrugState, bool isBalanced, string minFeeCode)
        {
            return this.QueryMedItemLists("Fee.GetForQuitDrug.3", inpatientNO, beginTime.ToString(), endTime.ToString(), sendDrugState, NConvert.ToInt32(isBalanced).ToString(), minFeeCode);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <param name="sendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalanced">�Ƿ����</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsCanQuitByInvoiceNO(string invoiceNO, DateTime beginTime, DateTime endTime, string sendDrugState, bool isBalanced)
        {
            return this.QueryMedItemLists("Fee.GetForQuitDrug.2", invoiceNO, beginTime.ToString(), endTime.ToString(), sendDrugState, NConvert.ToInt32(isBalanced).ToString());
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">��ֹʱ��</param>
        /// <param name="sendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalanced">�Ƿ����</param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:�����Ƿ������ȡ���߿��˷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsCanQuitByInvoiceNO(string invoiceNO, DateTime beginTime, DateTime endTime, string sendDrugState, bool isBalanced, string minFeeCode)
        {
            return this.QueryMedItemLists("Fee.GetForQuitDrug.4", invoiceNO, beginTime.ToString(), endTime.ToString(), sendDrugState, NConvert.ToInt32(isBalanced).ToString(), minFeeCode);
        }

        /// <summary>
        /// ����û��߷�ҩƷ�շ���Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="deptCode">���Ҵ���</param>
        /// <returns>�ɹ�:���߷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsByInpatientNO(string inpatientNO, DateTime beginTime, DateTime endTime, string deptCode)
        {
            return this.QueryFeeItemLists("Fee.GetPatientUndrug.1", inpatientNO, beginTime.ToString(), endTime.ToString(), deptCode);
        }

        /// <summary>
        /// ��û���ҩƷ�շ���Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="deptCode">���Ҵ���</param>
        /// <returns>�ɹ�:����ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsByInpatientNO(string inpatientNO, DateTime beginTime, DateTime endTime, string deptCode)
        {
            return this.QueryMedItemLists("Fee.GetPatientDrug.1", inpatientNO, beginTime.ToString(), endTime.ToString(), deptCode);
        }

        /// <summary>
        /// ��û��ߺ�ִ�п��ҵķ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="execDeptCode">���Ҵ���</param>
        /// <returns>�ɹ�:���߷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListsByInpatientNOAndDept(string inpatientNO, string execDeptCode)
        {
            return this.QueryFeeItemLists("Fee.GetFeeItemListByInpatientAndDept.1", inpatientNO, execDeptCode);
        }

        /// <summary>
        /// ��û��ߺ�ִ�п����Ѿ�ȷ�ϵķ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="execDeptCode">���Ҵ���</param>
        /// <returns>�ɹ�:���߷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryExeFeeItemListsByInpatientNOAndDept(string inpatientNO, string execDeptCode)
        {
            return this.QueryFeeItemLists("Fee.GetExeFeeItemListByInpatientAndDept.1", inpatientNO, execDeptCode);
        }

        /// <summary>
        /// ��û��ߺ�ִ�п��ҵ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="execDeptCode">���Ҵ���</param>
        /// <returns>�ɹ�:����ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsByInpatientNOAndDept(string inpatientNO, string execDeptCode)
        {
            return this.QueryMedItemLists("Fee.GetMedItemListByInpatientAndDept.1", inpatientNO, execDeptCode);
        }



        //{F8137B37-C1B1-4fe1-8008-00F17B4FE40E}
        /// <summary>
        /// ����ҩƷ״̬���һ���ҩƷ��Ŀ
        /// </summary>
        /// <param name="inpatientNO">סԺ��</param>
        /// <param name="sendDrugState">ҩƷ״̬</param>
        /// <returns></returns>
        public ArrayList QueryMedItemLists(string inpatientNO, string sendDrugState)
        {
            return this.QueryMedItemLists("Fee.GetForQuitDrug.5", inpatientNO, sendDrugState);
        }

        /// <summary>
        /// ����ҩƷ�ͷ�ҩƷ��ϸ������¼---ͨ������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="isPharmacy">�Ƿ�ҩƷ Drug(true)�� UnDrug(false)��ҩƷ</param>
        /// <returns>�ɹ�: ҩƷ�ͷ�ҩƷ��ϸ������¼ ʧ��: null</returns>
        public FeeItemList GetItemListByRecipeNO(string recipeNO, int recipeSequence, EnumItemType isPharmacy)
        {
            ArrayList feeItemLists = new ArrayList();

            if (isPharmacy == EnumItemType.Drug)
            {
                feeItemLists = this.QueryMedItemLists("Fee.GetFeeItemListByNoteNoAndNoteSequence.1", recipeNO, recipeSequence.ToString());
            }
            else
            {
                feeItemLists = this.QueryFeeItemLists("Fee.GetFeeItemListByNoteNoAndNoteSequence.1", recipeNO, recipeSequence.ToString());
            }

            if (feeItemLists == null)
            {
                return null;
            }

            if (feeItemLists.Count == 0)
            {
                this.Err = "û���ҵ�������Ϣ";

                return null;
            }

            return (FeeItemList)feeItemLists[0];
        }

        /// <summary>
        /// ֱ�ӽ����˷�,ͨ����Ʊ�Ż��ҩƷ������ϸ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:����ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListByInvoiceNO(string invoiceNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryMedItemLists("Fee.GetMedItemFromInvoice.1", invoiceNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ֱ�ӽ����˷�,ͨ����Ʊ�Ż�÷�ҩƷ������ϸ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�:���߷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListByInvoiceNO(string invoiceNO, DateTime beginTime, DateTime endTime)
        {
            return this.QueryFeeItemLists("Fee.GetUndrugItemFromInvoice.1", invoiceNO, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ��ȡ���û�����Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:���û�����Ϣ ʧ��:null û���ҵ�����:ArrayList.Count = 0</returns>
        public ArrayList QueryFeeInfosByInpatientNOAndBalanceNO(string inpatientNO, string balanceNO)
        {
            return this.QueryFeeInfos("Fee.GetFeeInfoBalanceByBalNo.1", inpatientNO, balanceNO);
        }

        /// <summary>
        /// ��ѯ���߷��÷��������Ϣ,���ص��ǰ�������ʵ�������,����ʵ���ڲ������е���Ϣ����ֵ
        /// ����ʱע��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="balanceFlag">0,δ���㣬1�ѽ��� ��ALL ȫ��</param>
        /// <returns>�ɹ�:������Ϣ ʧ��:null û���ҵ�����:ArrayList.Count = 0</returns>
        public ArrayList QueryFeeItemListSum(string inpatientNO, DateTime beginTime, DateTime endTime, string balanceFlag)
        {
            //�����Ҫ����,��ͬʱ����SQL���,			
            ArrayList feeItemLists = new ArrayList();
            string sql = string.Empty;
            FeeItemList feeItemList = null;

            if (this.Sql.GetSql("Fee.Inpatient.GetPatientFeeItemsSum", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.Inpatient.GetPatientFeeItemsSum��SQL���";

                return null;
            }
            if (balanceFlag == "" || balanceFlag == "All")
            {
                balanceFlag = "ALL";
            }
            try
            {
                if (this.ExecQuery(sql, inpatientNO, beginTime.ToString(), endTime.ToString(), balanceFlag) == -1)
                {
                    return null;
                }
                while (this.Reader.Read())
                {
                    feeItemList = new FeeItemList();

                    feeItemList.ID = inpatientNO;
                    feeItemList.Item.Name = this.Reader[0].ToString();
                    feeItemList.Item.MinFee.ID = this.Reader[1].ToString();
                    feeItemList.Item.Price = NConvert.ToDecimal(this.Reader[2].ToString());
                    feeItemList.Item.Qty = NConvert.ToDecimal(this.Reader[3].ToString());
                    feeItemList.Item.PriceUnit = this.Reader[4].ToString();
                    feeItemList.FT.TotCost = NConvert.ToDecimal(this.Reader[5].ToString());
                    feeItemList.FT.OwnCost = NConvert.ToDecimal(this.Reader[6].ToString());
                    feeItemList.FT.PayCost = NConvert.ToDecimal(this.Reader[7].ToString());
                    feeItemList.FT.PubCost = NConvert.ToDecimal(this.Reader[8].ToString());
                    feeItemList.Item.Specs = this.Reader[9].ToString();
                    feeItemList.Item.ID = this.Reader[10].ToString();
                    feeItemList.Item.Memo = this.Reader[11].ToString();

                    feeItemLists.Add(feeItemList);
                }

                this.Reader.Close();

                return feeItemLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��ȡ��ת���ø����÷���
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�:��ת���ø����÷��� ʧ��:null</returns>
        public FeeInfo GetChangeCostTotal(string inpatientNO, string balanceState)
        {
            ArrayList temp = this.QueryFeeInfoGroups("Fee.GetChangeCostTotal.1", inpatientNO, balanceState);

            if (temp == null || temp.Count == 0)
            {
                return null;
            }

            return (FeeInfo)temp[0];
        }



        #endregion

        #region ��ս���״̬����

        /// <summary>
        /// ��ս���״̬����,��������嵥�󣬸��»��ߵ�in_state = 'C', ���еط��������ڼ���¼����
        /// ����Ҫ�򿪴�״̬ʱ�����뻼��ΪC״̬������Ϊ'B'
        /// </summary>
        /// <param name="inpatientNO">����סԺ��ˮ��</param>
        /// <param name="state">C �� B ��(���߾�Ϊ��Ժ�Ǽ�δ����״̬</param>
        /// <returns>-1 ʧ�� 0 û�м�¼��1�ɹ�</returns>
        public int UpdateCloseAccount(string inpatientNO, string state)
        {
            return this.UpdateSingleTable("Fee.Inpatient.UpdateCloseAccount.Update", inpatientNO, state);
        }

        #region "�ʿ���"

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>0�ɹ� -1 ʧ��</returns>
        public int CloseAccount(string inpatientNO)
        {
            return this.UpdateSingleTable("Fee.Inpatient.CloseAccountNo.1", inpatientNO);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="inpatientNO">������ˮ��</param>
        /// <returns>0�ɹ� -1 ʧ��</returns>
        public int OpenAccount(string inpatientNO)
        {
            return this.UpdateSingleTable("Fee.Inpatient.OpenAccountNo.1", inpatientNO);
        }

        /// <summary>
        /// ��ѯ�����ʿ���
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>���ʱ�־1����0����</returns>
        public string GetStopAccount(string inpatientNO)
        {
            return this.ExecSqlReturnOne("Fee.Inpatient.QueryStopAccount.1", inpatientNO);
        }

        #endregion

        #endregion

        #region ���½�����Ϣ

        /// <summary>
        ///  ����סԺ���������Ϣ
        /// </summary>
        /// <param name="patient">���߻�����Ϣ</param>
        /// <param name="balanceTime">����ʱ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateInMainInfoBalanced(PatientInfo patient, DateTime balanceTime, int balanceNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.UpdateInMaininfoBalanced.1", patient.ID, balanceTime.ToString(), ft.PrepayCost.ToString(),
                ft.TransferPrepayCost.ToString(), ft.TotCost.ToString(), ft.OwnCost.ToString(), ft.PubCost.ToString(), ft.PayCost.ToString(),
                ft.RebateCost.ToString(), balanceNO.ToString(), patient.PVisit.InState.ID.ToString(), ft.TransferTotCost.ToString(), ft.TransferPrepayCost.ToString());
        }

        /// <summary>
        /// ����µĽ������
        /// </summary>
        /// <param name="inpatientNO">����סԺ��ˮ��</param>
        /// <returns>�ɹ�:���½�����ż����ν��������� ʧ��:null</returns>
        public string GetNewBalanceNO(string inpatientNO)
        {
            return this.ExecSqlReturnOne("Fee.Inpatient.GetNewBalanceNo.No1", inpatientNO);
        }

        /// <summary>
        /// ��ת��Ԥ�������Ϊ����״̬
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateChangePrepayBalanced(string inpatientNO, int balanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateChangePrepayBalanced.1", inpatientNO, this.Operator.ID, this.GetSysDate(), balanceNO.ToString());
        }

        /// <summary>
        /// ��Ժ���㽫����δ��Ԥ�������Ϊ����״̬-------------- ��ʱû���õ��������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateAllPrepayBalanced(string inpatientNO, int balanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateAllPrepayBalanced.1", inpatientNO, this.Operator.ID, this.GetSysDate(), balanceNO.ToString());
        }

        /// <summary>
        /// ��������ת�����Ϊ����
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateAllChangeCostBalanced(string inpatientNO, int balanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateAllChangeCostBalanced.1", inpatientNO, this.Operator.ID, this.GetSysDateTime(), balanceNO.ToString());
        }

        /// <summary>
        /// ������ķ��û�����Ϣ��Ϊ����״̬--��Ժ����ȫupdateʹ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="balanceTime">����ʱ��</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateFeeInfoBalanced(string inpatientNO, int balanceNO, DateTime balanceTime, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateFeeInfoBalanced.1", inpatientNO, balanceNO.ToString(), invoiceNO, this.Operator.ID, balanceTime.ToString());
        }

        /// <summary>
        /// ������ķ��û�����Ϣ��Ϊ����״̬-��;���㰴ʱ�����С����update
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="balanceTime">����ʱ��</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateFeeInfoBalanced(string inpatientNO, int balanceNO, DateTime balanceTime, string invoiceNO, DateTime beginTime, DateTime endTime, string minFeeCode)
        {
            return this.UpdateSingleTable("Fee.UpdateFeeInfoBalanced.2", inpatientNO, balanceNO.ToString(), invoiceNO, this.Operator.ID, balanceTime.ToString(),
                beginTime.ToString(), endTime.ToString(), minFeeCode);
        }

        /// <summary>
        /// ������ķ�ҩƷ������ϸ��Ϣ��Ϊ����״̬--��Ժ����ȫupdateʹ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateFeeItemListBalanced(string inpatientNO, int balanceNO, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateItemListBalanced.1", inpatientNO, balanceNO.ToString(), invoiceNO);
        }

        /// <summary>
        /// ������ķ�ҩƷ������ϸ��Ϣ��Ϊ����״̬--��;���㰴ʱ�����С����update
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateFeeItemListBalanced(string inpatientNO, int balanceNO, string invoiceNO, DateTime beginTime, DateTime endTime, string minFeeCode)
        {
            return this.UpdateSingleTable("Fee.UpdateItemListBalanced.2", inpatientNO, balanceNO.ToString(), invoiceNO, beginTime.ToString(), endTime.ToString(), minFeeCode);
        }

        /// <summary>
        /// �������ҩƷ������ϸ��Ϣ��Ϊ����״̬--��Ժ����ȫupdateʹ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateMedItemListBalanced(string inpatientNO, int balanceNO, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateMedItemListBalanced.1", inpatientNO, balanceNO.ToString(), invoiceNO);
        }

        /// <summary>
        /// �������ҩƷ������ϸ��Ϣ��Ϊ����״̬--��;���㰴ʱ�����С����update
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="minFeeCode">��С���ô���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        public int UpdateMedItemListBalanced(string inpatientNO, int balanceNO, string invoiceNO, DateTime beginTime, DateTime endTime, string minFeeCode)
        {
            return this.UpdateSingleTable("Fee.UpdateMedItemListBalanced.2", inpatientNO, balanceNO.ToString(), invoiceNO, beginTime.ToString(), endTime.ToString(), minFeeCode);
        }

        #endregion

        #region �����ѯ

        /// <summary>
        /// ͨ��סԺ��ˮ�Ų�ѯ���߽���ͷ����Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByInpatientNO(string inpatientNO)
        {
            return this.QueryBalances("Fee.GetBalanceHeadInfoByInpatientNo.Select.1", inpatientNO);
        }

        /// <summary>
        /// ����סԺ��ˮ�Ż�ȡδ���ϵķ�Ʊ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="flag">ALL ȫ����O ��Ժ���㣬I ��;����</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByInpatientNO(string inpatientNO, string flag)
        {
            string whereIndex = string.Empty;//Where����

            if (flag == string.Empty || flag == "ALL")
            {
                whereIndex = "Fee.GetBalanceHeadInfoByInpatientNo.Select.2";
            }
            else if (flag == "0")
            {
                whereIndex = "Fee.GetBalanceHeadInfoByInpatientNo.Select.3";
            }
            else
            {
                whereIndex = "Fee.GetBalanceHeadInfoByInpatientNo.Select.4";
            }

            return this.QueryBalances(whereIndex, inpatientNO);
        }

        /// <summary>
        /// ͨ����Ʊ�����������ͷ�������Ϣ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByInvoiceNO(string invoiceNO)
        {
            return this.QueryBalances("GetBalanceInfoByInvoice.1", invoiceNO);
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByTime(DateTime beginTime, DateTime endTime)
        {
            return this.QueryBalances("GetBalanceInfoByDate.Where", beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="isPositive">�Ƿ�ֻ��ʾ������</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByTime(DateTime beginTime, DateTime endTime, bool isPositive)
        {
            if (isPositive)
            {
                return this.QueryBalances("GetBalanceInfoByDate.Where.1", beginTime.ToString(), endTime.ToString());
            }
            else
            {
                return this.QueryBalances("GetBalanceInfoByDate.Where", beginTime.ToString(), endTime.ToString());
            }
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="isPositive">�Ƿ���ʾ������</param>
        /// <param name="operCode">����Ա</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByTime(DateTime beginTime, DateTime endTime, bool isPositive, string operCode)
        {
            string select = string.Empty;
            string whereMain = string.Empty;
            string whereSec = string.Empty;

            select = this.GetSqlForSelectAllInfoFromBalanceHead();

            if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref whereMain) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where��SQL���";

                return null;
            }
            select = select + " " + whereMain;

            if (isPositive)
            {
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where.1", ref whereSec) == -1)
                {
                    this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where.1��SQL���";

                    return null;
                }

                select = select + " " + whereSec;

            }

            if (operCode != "ALL")
            {
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where.2", ref whereSec) == -1)
                {
                    this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where.2��SQL���";

                    return null;
                }

                select = select + " " + whereSec;
            }

            return this.QueryBalancesBySql(select, beginTime.ToString(), endTime.ToString(), operCode);
        }

        /// <summary>
        /// ����Աʵ����ϸ,�������������ϵ�����
        /// </summary>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="operCode">����Ա ALL ȫ��</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByTime(DateTime beginTime, DateTime endTime, string operCode)
        {

            string select = string.Empty;
            string whereMain = string.Empty;
            string whereSec = string.Empty;

            select = this.GetSqlForSelectAllInfoFromBalanceHead();

            if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref whereMain) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where��SQL���";

                return null;
            }

            select = select + " " + whereMain;

            if (this.Sql.GetSql("GetBalanceInfoByDate.Where.NoReprint", ref whereSec) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where��SQL���";

                return null;
            }

            select = select + " " + whereSec;

            if (operCode != "ALL")
            {
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where.2", ref whereSec) == -1)
                {
                    this.Err = "û���ҵ�����Ϊ:GetBalanceInfoByDate.Where.2��SQL���";

                    return null;
                }

                select = select + " " + whereSec;
            }

            return this.QueryBalancesBySql(select, beginTime.ToString(), endTime.ToString());
        }

        /// <summary>
        /// ͨ���������סԺ��ˮ�ż�������ͷ��Ʊ�������Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�: ���߽���ͷ����Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalancesByBalanceNO(string inpatientNO, int balanceNO)
        {
            return this.QueryBalances("GetBalanceInfoBybalNo.1", balanceNO.ToString(), inpatientNO);
        }

        /// <summary>
        /// ͨ����Ʊ�������������ϸ������Ϣ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�: ���߽�����ϸ��Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalanceListsByInvoiceNO(string invoiceNO)
        {
            return this.QueryBalanceLists("GetBalanceListInfoByInvoice.1", invoiceNO);
        }

        /// <summary>
        /// ���ݷ�Ʊ�źͽ�����Ż�ȡ������ϸ��Ϣ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�: ���߽�����ϸ��Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalanceListsByInvoiceNOAndBalanceNO(string invoiceNO, int balanceNO)
        {
            return this.QueryBalanceLists("GetBalanceListInfoByInvoiceAndbalance.Where", invoiceNO, balanceNO.ToString());
        }


        /// <summary>
        /// ����סԺ��ˮ�źͽ�����Ż�ȡ������ϸ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�: ���߽�����ϸ��Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalanceListsByInpatientNOAndBalanceNO(string inpatientNO, int balanceNO)
        {
            return this.QueryBalanceLists("Fee.GetBalanceListByBalNo.1", inpatientNO, balanceNO.ToString());
        }

        /// <summary>
        /// ͨ��סԺ��ˮ�ţ���Ʊ�Ÿ��������,��ȡ������ϸ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�: ���߽�����ϸ��Ϣ ʧ��:Null û�в��ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryBalanceListsByInpatientNOAndBalanceNO(string inpatientNO, string invoiceNO, int balanceNO)
        {
            return this.QueryBalanceLists("Fee.GetBalanceListByInvoiceAndBalNo.1", inpatientNO, invoiceNO, balanceNO.ToString());
        }

        /// <summary>
        /// ��ȡĳ�˽���֧����Ϣ
        /// </summary>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <param name="balanceNO">�������</param>
        /// <returns>�ɹ�:֧����Ϣ���� ʧ��: null</returns>
        public ArrayList QueryBalancePaysByInvoiceNOAndBalanceNO(string invoiceNO, int balanceNO)
        {
            return this.QueryBalancePays("Fee.GetBalancePayByInvoiceAndBalNo.1", invoiceNO, balanceNO.ToString());
        }

        /// <summary>
        /// ��ȡ�����ϴ���;�����ʱ�䣬��Ϊ���ν���Ŀ�ʼʱ��,��û�н��й��нᣬȡ��Ժʱ��
        /// </summary>
        /// <param name="patient">����ʵ��</param>
        /// <returns>�ɹ�: �����ϴ���;�����ʱ�䣬��Ϊ���ν���Ŀ�ʼʱ��,��û�н��й��нᣬȡ��Ժʱ�� ʧ��: "-1"</returns>
        public string GetLastMidBalanceDate(PatientInfo patient)
        {
            return this.ExecSqlReturnOne("Fee.GetLastMidBalanceDate", patient.ID);
        }

        #endregion

        #region �����ٻ�

        /// <summary>
        /// ���½���ͷ���Ƿ�����
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="wasteFlag">���ϱ��</param>
        /// <param name="balanceTime">����ʱ��</param>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateBalanceHeadWasteFlag(string inpatientNO, int balanceNO, string wasteFlag, DateTime balanceTime, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateBalanceHeadWasteFlag.1", inpatientNO, balanceNO.ToString(), wasteFlag, this.Operator.ID, balanceTime.ToString(), invoiceNO);
        }

        /// <summary>
        /// �����ٻظ�������
        /// </summary>
        /// <param name="patient">��Ա������Ϣ</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateInmaininfoBalanceRecall(PatientInfo patient, int balanceNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.UpdateInmaininfoBalanceRecall.1", patient.ID, ft.PrepayCost.ToString(), ft.TotCost.ToString(),
                ft.OwnCost.ToString(), ft.PubCost.ToString(), ft.PayCost.ToString(), ft.RebateCost.ToString(), ft.TransferTotCost.ToString(),
                ft.TransferTotCost.ToString(), ft.TransferPrepayCost.ToString(), balanceNO.ToString(), patient.PVisit.InState.ID.ToString());
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ,Ϊ��Ʊ�����ṩ,ֻ���½������,�ͷ�Ʊ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="orgInvoiceNO">ԭʼ��Ʊ��</param>
        /// <param name="newInvoiceNO">�·�Ʊ��</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateFeeItemListsBalanceNOForReprint(string inpatientNO, string orgInvoiceNO, string newInvoiceNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateFeeItemBalanceNoForReprint.1", inpatientNO, orgInvoiceNO, newInvoiceNO, newBalanceNO.ToString());
        }

        /// <summary>
        /// ����ҩƷ��ϸ,Ϊ��Ʊ�����ṩ,ֻ���½������,�ͷ�Ʊ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="orgInvoiceNO">ԭʼ��Ʊ��</param>
        /// <param name="newInvoiceNO">�·�Ʊ��</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateMedItemListsBalanceNOForReprint(string inpatientNO, string orgInvoiceNO, string newInvoiceNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateMedItemBalanceNoForReprint.1", inpatientNO, orgInvoiceNO, newInvoiceNO, newBalanceNO.ToString());
        }

        /// <summary>
        /// ��������,Ϊֱ���˷�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceCost">������</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="balanceTime">����ʱ��</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateMainInfoForDirQuitFee(string inpatientNO, decimal balanceCost, int balanceNO, DateTime balanceTime)
        {
            return this.UpdateSingleTable("Fee.UpdateMainInfoForDirQuitFee.1", inpatientNO, balanceCost.ToString(), balanceNO.ToString(), balanceTime.ToString());
        }

        /// <summary>
        /// �����ٻظ��»��߷�ҩƷ������ϸ�������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="orgBalanceNO">ԭʼ�������</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateFeeItemListsBalanceNO(string inpatientNO, int orgBalanceNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateFeeItemBalanceNo.1", inpatientNO, orgBalanceNO.ToString(), newBalanceNO.ToString());
        }

        /// <summary>
        /// �����ٻظ��»���ҩƷ������ϸ�������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="orgBalanceNO">ԭʼ�������</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateMedItemListsBalanceNO(string inpatientNO, int orgBalanceNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateMedItemBalanceNo.1", inpatientNO, orgBalanceNO.ToString(), newBalanceNO.ToString());
        }

        /// <summary>
        /// ���»�������������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateInMainInfoBalanceNO(string inpatientNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateInmaininfoBalanceNo.1", inpatientNO, newBalanceNO.ToString());
        }


        //{402C1A7D-6874-441e-B335-37B408C41C16}
        /// <summary>
        /// ����;�������תѺ��ʱ�����ٻظ�������
        /// </summary>
        /// <param name="patient">��Ա������Ϣ</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="ft">������Ϣ</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateInmaininfoMidBalanceRecall(PatientInfo patient, int balanceNO, FT ft)
        {
            return this.UpdateSingleTable("Fee.UpdateInmaininfoMidBalanceRecall.1", patient.ID, ft.PrepayCost.ToString(), ft.TotCost.ToString(),
                ft.OwnCost.ToString(), ft.PubCost.ToString(), ft.PayCost.ToString(), ft.RebateCost.ToString(), ft.TransferTotCost.ToString(),
                ft.TransferTotCost.ToString(), ft.TransferPrepayCost.ToString(), balanceNO.ToString(), patient.PVisit.InState.ID.ToString());
        }

        #endregion

        #region �������޶�

        /// <summary>
        /// ���¹��ѻ��߹���ҩ�ۼƺ͹���ҩ������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="cost">���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateBursaryTotMedFee(string inpatientNO, decimal cost)
        {
            return this.UpdateSingleTable("Fee.UpdateBursaryTotMedFee.1", inpatientNO, cost.ToString());
        }

        /// <summary>
        /// �������޶��ۼ�        
        /// ͬʱҪ�������޶�겿�֣�ֻ���������ܶ�������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="cost">���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateLimitTot(string inpatientNO, decimal cost)
        {
            return this.UpdateSingleTable("Fee.UpdateLimitTot.1", inpatientNO, cost.ToString());
        }

        /// <summary>
        /// �����������޶�����޶��ۼƺͳ����� ---�������޶���
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="cost">���</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateInMainInfoForChangeDayLimit(string inpatientNO, decimal cost)
        {
            return this.UpdateSingleTable("Fee.UpdateInmaininfoForChangeDayLimit", inpatientNO, cost.ToString());
        }

        /// <summary>
        /// ���»��߳�����--����ÿ���Է�ҩ��������
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="cost">���޶������</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateLimitOverTop(string inpatientNO, decimal cost)
        {
            return this.UpdateSingleTable("Fee.Inpatient.UpdateLimitOverTop", inpatientNO, cost.ToString());
        }

        /// <summary>
        /// ���¹������޶�,ͨ������������޶�,�������ݵ�������ϵͳ���ݵ���.
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="overtopCost">������</param>
        /// <param name="dayLimitTotCost">���޶��ܶ�</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����: 0</returns>
        public int UpdateLimitOverTopAndTot(string inpatientNO, decimal overtopCost, decimal dayLimitTotCost)
        {
            return this.UpdateSingleTable("Fee.Inpatient.UpdateLimitOverTopAndTot", inpatientNO, overtopCost.ToString(), dayLimitTotCost.ToString());
        }


        #endregion

        #region ֱ���˷�

        /// <summary>
        /// ���ҩƷ��ϸ(ֱ���˷���)
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�:ҩƷ��ϸ ʧ��:null û���ҵ�����ArrayList.Count = 0</returns>
        public ArrayList QueryMedItemListsForDirQuit(string inpatientNO, string invoiceNO)
        {
            string sql = string.Empty;//��ѯSQL���

            if (this.Sql.GetSql("Fee.GetDirQuitFeeMedList.Select.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetDirQuitFeeMedList.Select.1��SQL���";

                return null;
            }

            if (this.ExecQuery(sql, inpatientNO, invoiceNO) == -1)
            {
                return null;
            }

            try
            {
                ArrayList feeItemLists = new ArrayList();//������ϸ����
                FeeItemList feeItemList = null;//������ϸʵ��

                while (this.Reader.Read())
                {
                    feeItemList = new FeeItemList();

                    feeItemList.RecipeNO = this.Reader[0].ToString();
                    feeItemList.SequenceNO = NConvert.ToInt32(this.Reader[1].ToString());
                    feeItemList.Item.ID = this.Reader[2].ToString();
                    feeItemList.Item.Name = this.Reader[3].ToString();
                    feeItemList.Item.Specs = this.Reader[4].ToString();
                    feeItemList.Item.Price = NConvert.ToDecimal(this.Reader[5].ToString());
                    feeItemList.NoBackQty = NConvert.ToDecimal(this.Reader[6].ToString());
                    feeItemList.Item.PriceUnit = this.Reader[7].ToString();
                    feeItemList.FT.TotCost = NConvert.ToDecimal(this.Reader[8].ToString());
                    feeItemList.FT.OwnCost = NConvert.ToDecimal(this.Reader[9].ToString());
                    feeItemList.FT.PayCost = NConvert.ToDecimal(this.Reader[10].ToString());
                    feeItemList.FT.PubCost = NConvert.ToDecimal(this.Reader[11].ToString());
                    feeItemList.FeeOper.OperTime = NConvert.ToDateTime(this.Reader[12].ToString());
                    feeItemList.Memo = this.Reader[13].ToString();
                    feeItemList.Item.SpellCode = this.Reader[14].ToString();
                    feeItemList.Item.MinFee.ID = this.Reader[15].ToString();
                    feeItemList.Memo = this.Reader[16].ToString();

                    feeItemLists.Add(feeItemList);
                }

                this.Reader.Close();

                return feeItemLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ��÷�ҩƷ��ϸ(ֱ���˷���)
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�:��ҩƷ��ϸ ʧ��:null û���ҵ�����ArrayList.Count = 0</returns>
        //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
        public ArrayList QueryFeeItemListsForDirQuit(string inpatientNO, string invoiceNO)
        {
            string sql = string.Empty;

            if (this.Sql.GetSql("Fee.GetDirQuitFeeItemList.Select.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetDirQuitFeeItemList.Select.1��SQL���";

                return null;
            }

            if (this.ExecQuery(sql, inpatientNO, invoiceNO) == -1)
            {
                return null;
            }

            try
            {
                ArrayList feeItemLists = new ArrayList();//������ϸ����
                FeeItemList feeItemList = null;//������ϸʵ��

                while (this.Reader.Read())
                {
                    feeItemList = new FeeItemList();

                    feeItemList.RecipeNO = this.Reader[0].ToString();
                    feeItemList.SequenceNO = NConvert.ToInt32(this.Reader[1].ToString());
                    feeItemList.Item.ID = this.Reader[2].ToString();
                    feeItemList.Item.Name = this.Reader[3].ToString();
                    feeItemList.Item.Price = NConvert.ToDecimal(this.Reader[4].ToString());
                    feeItemList.NoBackQty = NConvert.ToDecimal(this.Reader[5].ToString());
                    feeItemList.Item.PriceUnit = this.Reader[6].ToString();
                    feeItemList.FT.TotCost = NConvert.ToDecimal(this.Reader[7].ToString());
                    feeItemList.FT.OwnCost = NConvert.ToDecimal(this.Reader[8].ToString());
                    feeItemList.FT.PayCost = NConvert.ToDecimal(this.Reader[9].ToString());
                    feeItemList.FT.PubCost = NConvert.ToDecimal(this.Reader[10].ToString());
                    feeItemList.ExecOper.Dept.ID = this.Reader[11].ToString();
                    feeItemList.FeeOper.OperTime = NConvert.ToDateTime(this.Reader[12].ToString());
                    //feeItemList.Item.SpellCode = this.Reader[13].ToString();
                    feeItemList.Item.MinFee.ID = this.Reader[13].ToString();
                    feeItemList.Memo = this.Reader[14].ToString();
                    feeItemList.UpdateSequence = NConvert.ToInt32(this.Reader[15]);
                    feeItemLists.Add(feeItemList);
                }

                this.Reader.Close();

                return feeItemLists;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        #endregion

        #region ������Ϣ

        /// <summary>
        /// �������߼�������ܶ�
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public decimal GetTotDerateCost(string inpatientNO)
        {
            return NConvert.ToDecimal(this.ExecSqlReturnOne("Fee.GetTotDerateCost.1", inpatientNO));
        }

        /// <summary>
        /// ���¼�����ϢΪ����״̬ Written By ���峬
        /// </summary>
        /// <param name="inpatientNO">����סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">��Ʊ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int UpdateDerateBalanced(string inpatientNO, int balanceNO, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.InPatient.UpdateDerateBalance.1", inpatientNO, balanceNO.ToString(), invoiceNO);
        }

        /// <summary>
        /// ��Ӽ�����ü�¼
        /// </summary>
        /// <returns></returns>
        public int AddDerateFee(Neusoft.HISFC.Models.Fee.DerateFee DerateFee)
        {
            return 0;
        }

        #endregion

        #region ������Ϣ

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в�����߸��µ�����: 0</returns>
        public int InsertSurty(PatientInfo patient)
        {
            return this.UpdateSingleTable("Fee.Inpatient.InsertSurety", this.GetPatientSurtyParmas(patient));
        }


        /// <summary>
        /// ���ҵ������
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <returns></returns>
        public string GetSurtyCost(string InpatientNo)
        {
            //string sqlStr = string.Empty;
            //if (this.Sql.GetSql("Fee.Inpatient.SelectSuretyCost", ref sqlStr) == -1)
            //{
            //    this.Err = "����SQL��" + "Fee.Inpatient.SelectSuretyCost" + "���ʧ�ܣ�";
            //    return "-1";
            //}
            //try
            //{
            //    sqlStr = string.Format(sqlStr, InpatientNo);
            //}
            //catch (Exception ex)
            //{
            //    this.Err = ex.Message;
            //    return "-1";
            //}
            return this.ExecSqlReturnOne("Fee.Inpatient.SelectSuretyCost",InpatientNo);
        }


        //�������{0374EA05-782E-4609-9CDC-03236AB97906}

        private string QuerytSurtyInfoBase()
        {
            string strBaseSql = string.Empty;
            int returnValue = this.Sql.GetSql("Fee.Inpatient.SelectSurety.Base", ref strBaseSql);

            if (returnValue < 0)
            {
                this.Err = "��ѯ����Ϊ:[Fee.Inpatient.SelectSurety.Base]��sql���ʧ��";
                return null;
            }

            return strBaseSql;

           
        }

        /// <summary>
        /// ����where������ѯ��Ϣ{0374EA05-782E-4609-9CDC-03236AB97906}
        /// </summary>
        /// <param name="whereSqlIndex"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private ArrayList QuerySureTyDetailBySql(string whereSqlIndex, params string[] args)
        {
            string strBaseSql = this.QuerytSurtyInfoBase();

            if (strBaseSql == null)
            {
                return null;
            }
            string strWhereSql = string.Empty;
            int returnValue = this.Sql.GetSql(whereSqlIndex, ref strWhereSql);

            string strSql = string.Empty;
            if (returnValue < 0)
            {
                this.Err = "��ѯ����Ϊ:" + whereSqlIndex + "��sql���ʧ��";
                return null;
            }
            try
            {
                strSql = string.Format(strBaseSql + " " + strWhereSql, args);
            }
            catch (Exception ex)
            {

                this.Err = "��ʽ��sqlʧ�ܣ�" + ex.Message;
                return null;
            }

            return this.ExcuteBySql(strSql);


        }

        /// <summary>
        /// {0374EA05-782E-4609-9CDC-03236AB97906}
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private ArrayList ExcuteBySql(string strSql)
        {
            int returnValue = this.ExecQuery(strSql);
            if (returnValue < 0)
            {
                this.Err = "��ѯ������Ϣ����!" + this.Err;
                return null;
            }
            ArrayList al = new ArrayList();
           
            while (this.Reader.Read())
            {
                Neusoft.HISFC.Models.RADT.PatientInfo p = new PatientInfo();
                p.ID = this.Reader[0].ToString();
                p.Surety.HappenNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[1].ToString());
                p.PVisit.PatientLocation.Dept.ID = this.Reader[2].ToString();
                p.Surety.SuretyPerson.ID = this.Reader[3].ToString();
                p.Surety.SuretyPerson.Name = this.Reader[4].ToString();
                p.Surety.SuretyCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                p.Surety.SuretyType.ID = this.Reader[6].ToString();
                p.Surety.ApplyPerson.ID = this.Reader[7].ToString();
                p.Surety.ApplyPerson.Name = this.Reader[8].ToString();
                p.Surety.Memo = this.Reader[9].ToString();
                p.Surety.Oper.ID = this.Reader[10].ToString();
                p.Surety.Oper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[11].ToString());
                p.Surety.PayType.ID = this.Reader[12].ToString();
                p.Surety.State = this.Reader[13].ToString();
                p.Surety.Bank.ID = this.Reader[14].ToString();
                p.Surety.Bank.Account = this.Reader[15].ToString();
                p.Surety.Bank.WorkName = this.Reader[16].ToString();
                p.Surety.Bank.InvoiceNO = this.Reader[17].ToString();//СƱ����pos����ˮ
                p.Surety.InvoiceNO = this.Reader[18].ToString();
                p.Surety.OldInvoiceNO = this.Reader[19].ToString();
                al.Add(p);

                
 
            }
            return al;

        }

        /// <summary>
        /// ����סԺ�źŲ�ѯ������Ϣ{0374EA05-782E-4609-9CDC-03236AB97906}
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <returns></returns>
        public ArrayList QuerySuretyDetailByInpatientNO(string inpatientNO)
        {
            return this.QuerySureTyDetailBySql("Fee.Inpatient.SelectSurety.ByInpatientNO", inpatientNO);
        }

        /// <summary>
        /// ������Ч���{0374EA05-782E-4609-9CDC-03236AB97906}
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="happenNO"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public int UpdateSurtyState(string inpatientNO,string happenNO,string state)
        {
            return this.UpdateSingleTable("Fee.Inpatient.UpdateSurety.State", state,inpatientNO,happenNO);
        }

        #endregion

        #region �������

        /// <summary>
        /// ��ý������
        /// </summary>
        /// <param name="inpatientNO">����סԺ��ˮ��</param>
        /// <returns>�ɹ�: ������� ʧ��:null</returns>
        public PayKind GetPayKind(string inpatientNO)
        {
            PayKind payKind = new PayKind();

            payKind.ID = this.ExecSqlReturnOne("Fee.Inpatient.GetPayKind.1", inpatientNO);
            if (payKind.ID == null || payKind.ID == string.Empty)
            {
                this.Err = "��ý������������!";

                return null;
            }

            payKind.Name = this.ExecSqlReturnOne("Fee.Inpatient.GetPayKind.2", payKind.ID);
            if (payKind.Name == null || payKind.Name == string.Empty)
            {
                this.Err = "��ý���������Ƴ���!";

                return null;
            }

            return payKind;
        }

        #endregion

        #region ��ת

        /// <summary>
        /// �����תǷ�Ѳ��ַ��ò����ת��
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="feeInfo">������Ϣ</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1</returns>
        public int InsertCarryFowardFee(PatientInfo patient, FeeInfo feeInfo)
        {
            return this.UpdateSingleTable("AddPatientCarryFowardFee.1", this.GetCarryFowardFeeParams(patient, feeInfo));
        }

        #endregion

        #region ������

        /// <summary>
        /// ��ȡ��Ա���������Ͳ���������
        /// </summary>
        /// <param name="operCode">��Ա����</param>
        /// <returns>�ɹ�:��Ա���������Ͳ��������� ʧ��:null</returns>
        public NeuObject GetFinGroupInfoByOperCode(string operCode)
        {
            string sql = string.Empty;

            NeuObject group = new NeuObject();
            if (this.Sql.GetSql("GetOperGrpId.1", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:GetOperGrpId.1��SQL���";

                return null;
            }

            if (this.ExecQuery(sql, operCode) == -1)
            {
                return null;
            }
            try
            {
                while (this.Reader.Read())
                {
                    group.ID = this.Reader[0].ToString();
                    group.Name = this.Reader[1].ToString();
                }

                this.Reader.Close();

                return group;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        #endregion

        #region ��ͬ��λ

        /// <summary>
        /// ���ö���պ�ͬ��λ��Ŀ���Żݱ���
        /// </summary>
        /// <param name="itemList">������ϸʵ��</param>
        /// <returns>�ɹ�:�Żݱ��� ʧ��:null</returns>
        public string GetRebateRateEnuDate(FeeItemList itemList)
        {
            return this.ExecSqlReturnOne("Fee.GetRebateRateEnuDate.1", itemList.Patient.Pact.ID, itemList.Item.ID, this.GetSysDate("YYYY/MM/DD"));
        }

        /// <summary>
        /// ��ʱ��ת�������ڼ�---����
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>�ɹ�:����? ʧ��: null</returns>
        public string TransferDateTimetoWeekDay(DateTime time)
        {
            return this.ExecSqlReturnOne("Fee.TransferDateTimetoWeekDay.1", time.ToString());
        }

        /// <summary>
        /// ��ʱ��ת�������ڼ�---���� ���� Ĭ��ϵͳ��ǰʱ��
        /// </summary>
        /// <returns>�ɹ�:����? ʧ��: null</returns>
        public string TransferDateTimetoWeekDay()
        {
            return this.TransferDateTimetoWeekDay(this.GetDateTimeFromSysDateTime());
        }

        /// <summary>
        ///  ���ö�����ں�ͬ��λ��Ŀ���Żݱ���
        /// </summary>
        /// <param name="itemList">������ϸʵ��</param>
        /// <returns>�ɹ�:ö�����ں�ͬ��λ��Ŀ���Żݱ��� ʧ��:null</returns>
        public string GetRebateRateEnuWeek(FeeItemList itemList)
        {
            return this.ExecSqlReturnOne("Fee.GetRebateRateEnuWeek.1", itemList.Patient.Pact.ID, itemList.Item.ID, this.TransferDateTimetoWeekDay());
        }

        /// <summary>
        /// ���ʱ�䷶Χ��ͬ��λ��Ŀ���Żݱ���
        /// </summary>
        /// <param name="itemList">������ϸʵ��</param>
        /// <returns>�ɹ�: ʱ�䷶Χ��ͬ��λ��Ŀ���Żݱ��� ʧ��: null</returns>
        public string GetRebateRateBetweenDate(FeeItemList itemList)
        {
            return this.ExecSqlReturnOne("Fee.GetRebateBetweenDate.1", itemList.Patient.Pact.ID, itemList.Item.ID, this.GetSysDateTime());
        }

        #endregion

        #region ����Ӧ��

        /// <summary>
        /// ��ȡ��Ʊ������Ŀ---��ʱΪ֪��Ȩʹ��
        /// </summary>
        /// <param name="reportCode">�������</param>
        /// <returns>�ɹ�:��Ʊ������Ŀ ʧ��:null û���ҵ�����:ArrayList.Count = 0</returns>
        public ArrayList QueryFeeStatsByReportCode(string reportCode)
        {
            string sql = string.Empty;

            ArrayList temp = new ArrayList();
            if (this.Sql.GetSql("Fee.GetStatCodeAndNameByReportCode", ref sql) == -1)
            {
                this.Err = "û���ҵ�����Ϊ:Fee.GetStatCodeAndNameByReportCode��SQL���";

                return null;
            }

            if (this.ExecQuery(sql, reportCode) == -1)
            {
                return null;
            }

            try
            {
                while (this.Reader.Read())
                {
                    NeuObject obj = new NeuObject();

                    obj.ID = this.Reader[0].ToString();
                    obj.Name = this.Reader[1].ToString();

                    temp.Add(obj);
                }

                this.Reader.Close();

                return temp;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }

        /// <summary>
        /// ���������ͨ��id����name
        /// </summary>
        /// <param name="type">��������</param>
        /// <param name="code">�������</param>
        /// <returns>�ɹ�:����ֵ ʧ��: null</returns>
        public string GetComDictionaryNameByID(string type, string code)
        {
            return this.ExecSqlReturnOne("Fee.GetComDictionaryNameById", type, code);
        }

        #endregion

        #region ֱ�ӽ���

        /// <summary>
        /// �����ʱסԺ����
        /// </summary>
        /// <param name="patientNO">סԺ��</param>
        /// <returns>�ɹ�:��ʱסԺ���� ʧ��: null</returns>
        public string GetTempPatientNO(string patientNO)
        {
            return this.ExecSqlReturnOne("Fee.GetTempPatientNo.1", patientNO);
        }

        #endregion

        #region �˷�����

        /// <summary>
        /// ����ҽ���ţ�����ϺŲ�ѯ������Ŀ{F4912030-EF65-4099-880A-8A1792A3B449}
        /// </summary>
        /// <param name="moOrder">ҽ����</param>
        /// <param name="packageCode">������Ŀ����</param>
        /// <returns>ͬһ�շѸ�����Ŀ�Ĵ����ţ��ʹ�����ˮ�ż���</returns>
        public ArrayList QueryRecipesByMoOrder(string moOrder, string packageCode) 
        {
            string sql = string.Empty;

            if (this.Sql.GetSql("Fee.Inpatient.QueryRecipesByMoOrder.Query.1", ref sql) == -1) 
            {
                this.Err = "û���ҵ�����Ϊ:Fee.Inpatient.QueryRecipesByMoOrder.Query.1��SQl���";

                return null;
            }

            sql = string.Format(sql, moOrder, packageCode);

            if (this.ExecQuery(sql) == -1) 
            {
                return null;
            }
            ArrayList recipeNOs = new ArrayList();
            Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    obj = new NeuObject();

                    obj.ID = this.Reader[0].ToString();
                    obj.Name = this.Reader[1].ToString();

                    recipeNOs.Add(obj);
                }

                this.Reader.Close();

                return recipeNOs;
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                return null;
            }
        }//{F4912030-EF65-4099-880A-8A1792A3B449}����

        #endregion

        #endregion

        #region ��������


        /// <summary>
        /// ���������ͨ��type����name and id up by lisy   
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        [Obsolete("����", true)]
        public ArrayList GetComDictionaryName(string Type)
        {
            string strSql = "";

            if (this.Sql.GetSql("Fee.GetComDictionaryName", ref strSql) == -1) return null;
            try
            {
                //����0����1id
                strSql = string.Format(strSql, Type);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
            this.ExecQuery(strSql);
            try
            {
                ArrayList al = new ArrayList();
                while (Reader.Read())
                {
                    Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();

                    obj.Name = Reader[0].ToString();
                    obj.ID = Reader[1].ToString();

                    al.Add(obj);
                }
                Reader.Close();
                return al;
            }
            catch (Exception ex)
            {
                this.ErrCode = "-1";
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
        }
        /// <summary>
        /// ���������ͨ��id����name   ͨ��name����id����ʱ����д,����̫��
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        [Obsolete("���� GetComDictionaryNameByID", true)]
        public string GetComDictionaryNameById(string Type, string Code)
        {
            string strSql = "";

            if (this.Sql.GetSql("Fee.GetComDictionaryNameById", ref strSql) == -1) return null;
            try
            {
                //����0����1id
                strSql = string.Format(strSql, Type, Code);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
            return this.ExecSqlReturnOne(strSql);
        }


        /// <summary>
        /// ��ȡ��Ʊ������Ŀ---��ʱΪ֪��Ȩʹ��
        /// </summary>
        /// <param name="ReportCode"></param>
        /// <returns></returns>
        [Obsolete("����,QueryFeeStatsByReportCode", true)]
        public ArrayList GetStatCodeAndNameByReportCode(string ReportCode)
        {
            string strSql = "";
            ArrayList al = new ArrayList();
            if (this.Sql.GetSql("Fee.GetStatCodeAndNameByReportCode", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, ReportCode);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            this.ExecQuery(strSql);
            while (this.Reader.Read())
            {
                Neusoft.FrameWork.Models.NeuObject Obj = new NeuObject();
                try
                {
                    Obj.ID = this.Reader[0].ToString();
                    Obj.Name = this.Reader[1].ToString();
                }
                catch (Exception ex)
                {
                    this.Err = ex.Message;
                    this.ErrCode = ex.Message;
                    return null;
                }

                al.Add(Obj);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// �����ʱסԺ����
        /// </summary>
        [Obsolete("����,GetTempPatientNO", true)]
        public string GetTempPatientNo(string parm)
        {
            string pNo = "";
            string strSql = "";
            if (this.Sql.GetSql("Fee.GetTempPatientNo.1", ref strSql) == -1) return null;
            strSql = System.String.Format(strSql, parm);
            pNo = this.ExecSqlReturnOne(strSql);
            //			pNo="LS"+pNo.PadLeft(8,'0');
            return pNo;
        }

        /// <summary>
        /// ����dataset
        /// </summary>
        /// <returns></returns>
        [Obsolete("����", true)]
        public DataSet GetDataSetBalanceHeadInfo()
        {
            DataSet dts = new DataSet();
            string strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
            this.ExecQuery(strSql, ref dts);
            //			this.r
            return dts;
        }


        /// <summary>
        /// ��ȡ��Ա���������Ͳ���������
        /// </summary>
        /// <param name="OperId">��Աid</param>
        /// <returns>������ʵ����id����name����</returns>
        [Obsolete("����,GetFinGroupInfoByOperCode()", true)]
        public NeuObject GetOperGrp(string OperId)
        {
            string strSql = "";
            //			string strDataSet = "";
            NeuObject OperGrp = new NeuObject();
            if (this.Sql.GetSql("GetOperGrpId.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, OperId);
            }
            catch (Exception ex)
            {
                this.ErrCode = "Fee.Inpateint.GetOperGrp" + ex.Message;
                this.Err = ex.Message;
                return null;
            }
            this.ExecQuery(strSql);
            if (this.Reader == null) return null;
            while (this.Reader.Read())
            {
                try
                {
                    OperGrp.ID = this.Reader[0].ToString();
                    OperGrp.Name = this.Reader[1].ToString();
                }
                catch (Exception ex)
                {
                    this.Err = "��ѯ��Ա�����鸳ֵʱ�����!" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
            }
            return OperGrp;
        }


        /// <summary>
        /// �����תǷ�Ѳ��ַ��ò����ת��
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="FeeInfo">������Ϣ</param>
        /// <returns>0�ɹ�-1ʧ��</returns>
        [Obsolete("����,InsertCarryFowardFee()", true)]
        public int AddPatientCarryFowardFee(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo)
        {
            string strSql = "";
            if (this.Sql.GetSql("AddPatientCarryFowardFee.1", ref strSql) == -1) return -1;
            //0 ������1��С���ô���2 ��������,1�����ף�2������ 3סԺ��ˮ�� 4���� 5������� 6��ͬ��λ 7��Ժ���Ҵ���8��ʿվ����
            //9�������Ҵ��� 10ִ�п��Ҵ���11�ۿ���Ҵ���12����ҽʦ����13Ƿ�ѽ��14������
            //15��������16�Ʒ���17�Ʒ�����18�����˴���19����ʱ��20�������21�����־ 0:δ���㣻1:�ѽ��� 2:�ѽ�ת
            //22������23Ӥ�����

            try
            {
                strSql = string.Format(strSql, FeeInfo.RecipeNO, FeeInfo.Item.MinFee.ID, FeeInfo.TransType, PatientInfo.ID, PatientInfo.Name,
                    PatientInfo.Pact.PayKind.ID, PatientInfo.Pact.ID, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.Dept.ID, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.NurseCell.ID, 
                    FeeInfo.StockOper.Dept.ID,
                    FeeInfo.ExecOper.Dept.ID, FeeInfo.StockOper.Dept.ID, FeeInfo.RecipeOper.ID, FeeInfo.FT.TotCost.ToString(), FeeInfo.ChargeOper.ID,
                    FeeInfo.ChargeOper.OperTime.ToString(), FeeInfo.FeeOper.ID, FeeInfo.FeeOper.OperTime.ToString(), FeeInfo.BalanceOper.ID, FeeInfo.BalanceOper.OperTime.ToString(),
                    FeeInfo.BalanceNO.ToString(), FeeInfo.BalanceState, FeeInfo.AuditingNO, System.Convert.ToInt16(FeeInfo.IsBaby).ToString()
                    );
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        /// <summary>
        /// ������л��������� Written By Wangrc
        /// ����PayKind����
        /// </summary>
        /// <returns></returns>
        [Obsolete("����", true)]
        public ArrayList GetPayKindList()
        {
            string strSql = "";
            ArrayList al = new ArrayList();
            if (this.Sql.GetSql("Fee.Inpatient.GetPayKindList.1", ref strSql) != 0) return null;
            this.ExecQuery(strSql);
            //0������ 1����������

            while (this.Reader.Read())
            {
                Neusoft.HISFC.Models.Base.PayKind payKind = new Neusoft.HISFC.Models.Base.PayKind();
                try
                {
                    payKind.ID = this.Reader[0].ToString();
                    payKind.Name = this.Reader[1].ToString();
                }


                catch (Exception ex)
                {
                    this.Err = "��ѯ����������ֵʱ�����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                }
                al.Add(payKind);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <returns></returns>
        [Obsolete("����,ʹ��InsertSurty", true)]
        public int AddPatientSurty(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo)
        {
            #region
            //��ӵ�����Ϣ
            //AddPatientSurty.1
            //0סԺ��ˮ��2���Ҵ���3�����˴���4����������5�������6�������� 7�����˴���8����������
            //10����Ա11��������
            #endregion
            string strSql = "";
            if (this.Sql.GetSql("AddPatientSurty.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, PatientInfo.ID, PatientInfo.PVisit.PatientLocation.Dept.ID,
                    PatientInfo.Caution.ID, PatientInfo.Caution.Name, PatientInfo.Caution.Money.ToString(),
                    PatientInfo.Caution.Type, PatientInfo.Caution.AuditingOper.ID, PatientInfo.Caution.AuditingOper.Name,
                    this.Operator.ID, this.GetSysDateTime());
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }


        /// <summary>
        /// ��ӽ���ʵ����¼
        /// AddBalancePay.1
        /// </summary>
        /// <param name="BalancePay">ʵ����Ϣ</param>
        /// <returns>0�ɹ�-1ʧ��</returns>
        [Obsolete("����,ʹ��InsertBalancePay", true)]
        public int AddBalancePay(Neusoft.HISFC.Models.Fee.Inpatient.BalancePay BalancePay)
        {
            string strSql = "";
            if (this.Sql.GetSql("AddBalancePay.1", ref strSql) == -1) return -1;
            //0��Ʊ����1��������2��������0 Ԥ���� 1 ����� 3֧����ʽ 4���5����6��������7������������8���������ʺ�
            //10���ػ��ձ�־11�����˴���12��������9�������
            try
            {
                strSql = string.Format(strSql, BalancePay.Invoice.ID, BalancePay.TransType, BalancePay.TransKind, BalancePay.PayType.ID,
                    BalancePay.FT.TotCost.ToString(), BalancePay.Qty.ToString(), BalancePay.Bank.ID, BalancePay.Bank.Name, BalancePay.Bank.Account,
                    BalancePay.BalanceNo,
                    BalancePay.RetrunOrSupplyFlag, BalancePay.BalanceOper.ID, BalancePay.BalanceOper.OperTime.ToString());
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        [Obsolete("����,ʹ��InsertBalance", true)]
        public int AddBalanceHead(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.Balance Balance)
        {
            return 0;
            //return this.AddBalanceHead(PatientInfo,Balance,null);
        }


        /// <summary>
        /// ��ӽ����¼ Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="Balance">������Ϣ</param>
        /// <param name="IInsuranceBalance">ҽ��������Ϣ</param>
        /// <returns></returns>
        [Obsolete("����", true)]
        public int AddBalanceHead(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.Balance Balance, Neusoft.HISFC.Models.Insurance.IInsuraceBalace IInsuranceBalance)
        {
            #region
            //��ӻ��߽����¼
            //AddBalanceHead.1
            //���������0��Ʊ���� ��תʱΪ��ˮ��1��������,1�����ף�2������2סԺ��ˮ��3�������4�������5��ͬ����6Ԥ�����
            //7���ý��8�Էѽ��9�Ը����10���ѽ��11�Żݽ��12������14���ս��15�������16תѺ��17��ʼ����
            //18��ֹ����19��������20�����˴���21����ʱ��
            //22���������23��ӡ����24�����˻�֧��25����Ա����26����27�Ϻ��28�����ֽ�֧��29������13���ϱ��30����Ʊ���31ʣ�ౣ�����ս�����32��������
            //33����Ա����
            //������������
            #endregion
            string strSql = "";
            decimal AcountPay = 0;
            decimal CashPay = 0;
            decimal LargePay = 0;
            decimal MiltrayPay = 0;
            decimal OfficePay = 0;
            if (IInsuranceBalance == null)
            {
                AcountPay = 0;
                CashPay = 0;
                LargePay = 0;
                MiltrayPay = 0;
                OfficePay = 0;
            }
            else
            {
                AcountPay = IInsuranceBalance.AcountPay;
                CashPay = IInsuranceBalance.CashPay;
                LargePay = IInsuranceBalance.LargePay;
                MiltrayPay = IInsuranceBalance.MiltrayPay;
                OfficePay = IInsuranceBalance.OfficePay;
            }
            string PactCode = "";
            string PayKindCode = "";
            if (Balance.Patient.Pact.ID == null || Balance.Patient.Pact.ID.Trim() == "")
            {
                PactCode = PatientInfo.Pact.ID;
            }
            else
            {
                PactCode = Balance.Patient.Pact.ID;
            }
            if (Balance.Patient.Pact.PayKind.ID == null || Balance.Patient.Pact.PayKind.ID.Trim() == "")
            {
                PayKindCode = PatientInfo.Pact.PayKind.ID;
            }
            else
            {
                PayKindCode = Balance.Patient.Pact.PayKind.ID;
            }

            string BalanceOperDeptCode = ""; //����Ա����
            //����Ա����
            if (Balance.Oper.ID == null || Balance.Oper.ID == "")
            {
                BalanceOperDeptCode = "";
            }
            else
            {
                if (Balance.Oper.Dept.ID == null || Balance.Oper.Dept.ID == "")
                {

                    BalanceOperDeptCode = this.GetDeptByEmplId(Balance.Oper.ID);
                }
                else
                {
                    BalanceOperDeptCode = Balance.Oper.Dept.ID;
                }
            }


            if (this.Sql.GetSql("AddBalanceHead.1", ref strSql) == -1) return -1;
            try
            {

                strSql = string.Format(strSql, Balance.Invoice.ID, Balance.TransType, PatientInfo.ID, Balance.ID, PatientInfo.Pact.PayKind.ID, PatientInfo.Pact.ID,
                    Balance.FT.PrepayCost.ToString(), Balance.FT.TotCost.ToString(), Balance.FT.OwnCost.ToString(), Balance.FT.PayCost.ToString(),
                    Balance.FT.PubCost.ToString(), Balance.FT.RebateCost.ToString(), Balance.FT.DerateCost.ToString(), ((int)Balance.CancelType).ToString(),
                    Balance.FT.SupplyCost.ToString(), Balance.FT.ReturnCost.ToString(), Balance.FT.TransferPrepayCost.ToString(),
                    Balance.BeginTime.ToString(), Balance.EndTime.ToString(), Balance.BalanceType.ID.ToString(), Balance.Oper.ID, Balance.Oper.OperTime.ToString(),
                    Balance.FinanceGroup.ID, Balance.PrintTimes.ToString(), AcountPay.ToString(), OfficePay.ToString(), LargePay.ToString(), MiltrayPay.ToString(),
                    CashPay.ToString(), Balance.AuditingNO, NConvert.ToInt32(Balance.IsMainInvoice), NConvert.ToInt32(Balance.IsLastBalance).ToString(),
                    PatientInfo.Name,
                    //����Ա����
                    BalanceOperDeptCode,
                    //��������������޶����
                    Balance.FT.AdjustOvertopCost.ToString());
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }


            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ��ӽ�����ϸ��¼ Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="BalanceList">������ϸ��¼��Ϣ</param>
        /// <returns>0 �ɹ� -1 ʧ��</returns>
        [Obsolete("����,��InsertBalanceList()", true)]
        public int AddBalanceList(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.BalanceList BalanceList)
        {
            #region
            //��ӻ��߽�����ϸ��¼
            //AddBalanceList.1
            //���������0��Ʊ����1��������2סԺ��ˮ��3����4�������5��ͬ��λ6��Ժ���Ҵ���7ͳ�ƴ���8ͳ�ƴ�������9��ӡ˳���
            //10���ý��11�Էѽ��12�Ը����13���ѽ��14�Żݽ��15Ϊ�˴���16�����˴���17����ʱ��18��������19�������
            //20Ӥ����־21������22����Ա����
            //������������
            #endregion
            string strSql = "";
            string PactCode = "";
            string PayKindCode = "";
            if (((Balance)BalanceList.BalanceBase).Patient.Pact.ID == null || ((Balance)BalanceList.BalanceBase).Patient.Pact.ID.Trim() == "")
            {
                PactCode = PatientInfo.Pact.ID;
            }
            else
            {
                PactCode = ((Balance)BalanceList.BalanceBase).Patient.Pact.ID;
            }
            if (((Balance)BalanceList.BalanceBase).Patient.Pact.PayKind.ID == null || ((Balance)BalanceList.BalanceBase).Patient.Pact.PayKind.ID.Trim() == "")
            {
                PayKindCode = PatientInfo.Pact.PayKind.ID;
            }
            else
            {
                PayKindCode = ((Balance)BalanceList.BalanceBase).Patient.Pact.PayKind.ID;
            }

            string BalanceOperDeptCode = ""; //����Ա����
            //����Ա����
            if (((Balance)BalanceList.BalanceBase).Oper.ID == null || ((Balance)BalanceList.BalanceBase).Oper.ID == "")
            {
                BalanceOperDeptCode = "";
            }
            else
            {
                if (((Balance)BalanceList.BalanceBase).Oper.Dept.ID == null || ((Balance)BalanceList.BalanceBase).Oper.Dept.ID == "")
                {

                    BalanceOperDeptCode = this.GetDeptByEmplId(((Balance)BalanceList.BalanceBase).Oper.ID);
                }
                else
                {
                    BalanceOperDeptCode = ((Balance)BalanceList.BalanceBase).Oper.Dept.ID;
                }
            }


            if (this.Sql.GetSql("AddBalanceList.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, ((Balance)BalanceList.BalanceBase).Invoice.ID,
                    ((int)((Balance)BalanceList.BalanceBase).TransType).ToString(),
                    PatientInfo.ID,
                    PatientInfo.Name, PayKindCode,
                    PactCode,
                    PatientInfo.PVisit.PatientLocation.Dept.ID,
                    BalanceList.FeeCodeStat.ID,
                    BalanceList.FeeCodeStat.Name,
                    ((Balance)BalanceList.BalanceBase).User01,
                    ((Balance)BalanceList.BalanceBase).FT.TotCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).FT.OwnCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).FT.PayCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).FT.PubCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).FT.RebateCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).FT.OwnCost.ToString(),
                    ((Balance)BalanceList.BalanceBase).Oper.ID, ((Balance)BalanceList.BalanceBase).Oper.OperTime.ToString(), ((Balance)BalanceList.BalanceBase).BalanceType.ID.ToString(), ((Balance)BalanceList.BalanceBase).ID,
                    BalanceList.User02,
                    ((Balance)BalanceList.BalanceBase).AuditingNO,
                    //�շ�Ա����
                    BalanceOperDeptCode);

            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }


            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ��ѯ���߷��÷��������Ϣ,���ص��ǰ�������ʵ�������,����ʵ���ڲ������е���Ϣ����ֵ
        /// ����ʱע��
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="begin">��ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <param name="balanceFlag">0,δ���㣬1�ѽ��� ��ALL ȫ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListSum()", true)]
        public ArrayList GetPatientFeeItemsSum(string InpatientNo, DateTime begin, DateTime end, string balanceFlag)
        {
            //�����Ҫ����,��ͬʱ����SQL���,			
            ArrayList al = new ArrayList();
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.GetPatientFeeItemsSum", ref strSql) == -1)
            {
                this.Err = "Can't Find Sql  Fee.Inpatient.GetPatientFeeItemsSum";
                return null;
            }
            if (balanceFlag == "" || balanceFlag == "All")
            {
                balanceFlag = "ALL";
            }
            try
            {
                strSql = string.Format(strSql, InpatientNo, begin.ToString(), end.ToString(), balanceFlag);
                if (this.ExecQuery(strSql) == -1) return null;
                while (this.Reader.Read())
                {
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList list = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                    list.ID = InpatientNo;
                    list.Item.Name = Reader[0].ToString();
                    list.Item.MinFee.ID = Reader[1].ToString();
                    list.Item.Price = NConvert.ToDecimal(Reader[2].ToString());
                    list.Item.Qty = NConvert.ToDecimal(this.Reader[3].ToString());
                    list.Item.PriceUnit = this.Reader[4].ToString();
                    list.FT.TotCost = NConvert.ToDecimal(this.Reader[5].ToString());
                    list.FT.OwnCost = NConvert.ToDecimal(this.Reader[6].ToString());
                    list.FT.PayCost = NConvert.ToDecimal(this.Reader[7].ToString());
                    list.FT.PubCost = NConvert.ToDecimal(this.Reader[8].ToString());
                    list.Item.Specs = this.Reader[9].ToString();
                    list.Item.ID = this.Reader[10].ToString();
                    list.Item.Memo = this.Reader[11].ToString();//����ԭ����Ŀ�ı��� Add by Wangyu

                    al.Add(list);
                }
                return al;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// ���»��߷�����Ϣ-����FeeInfo����Ϣ Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="FeeInfo">������Ϣ</param>
        /// <returns><br>0 �ɹ�</br><br>-1 ʧ��</br></returns>
        [Obsolete("����,��InsertAndUpdateFeeInfo()", true)]
        public int UpdateAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.FeeInfo FeeInfo)
        {
            #region "�ӿ�˵��"
            //���»��߷��û��ܱ�ķ�����Ϣ 
            //Fee.Inpatient.AddPatientAccount.2
            //���������0 ������1��С���ô���2 ��������,1�����ף�2������ 3סԺ��ˮ�� 4���� 5������� 6��ͬ��λ 7��Ժ���Ҵ���8��ʿվ����
            //9�������Ҵ��� 10ִ�п��Ҵ���11�ۿ���Ҵ���12����ҽʦ����13���ý��14�Էѽ��15�Ը����16���ѽ��17�Żݽ��18������
            //19��������20�Ʒ���21�Ʒ�����22�����˴���23����ʱ��24���㷢Ʊ��25�������26�����־ 0:δ���㣻1:�ѽ��� 2:�ѽ�ת
            //27������28Ӥ�����
            //������������
            #endregion
            string strSql = "";
            //����帺��¼���շ�ȡֵ����
            string PactId = "";
            if (FeeInfo.Patient.Pact.ID == null || FeeInfo.Patient.Pact.ID == "")
            {
                PactId = PatientInfo.Pact.ID;
            }
            else
            {
                PactId = FeeInfo.Patient.Pact.ID;
            }
            string PayKind = "";
            if (FeeInfo.Patient.Pact.PayKind.ID == null || FeeInfo.Patient.Pact.PayKind.ID == "")
            {
                PayKind = PatientInfo.Pact.PayKind.ID;
            }
            else
            {
                PayKind = FeeInfo.Patient.Pact.PayKind.ID;
            }
            string FeeOperDeptCode = ""; //�շ�Ա����
            //����Ա����
            if (FeeInfo.FeeOper.ID == null || FeeInfo.FeeOper.ID == "")
            {
                FeeOperDeptCode = "";
            }
            else
            {
                if (FeeInfo.FeeOper.Dept.ID == null || FeeInfo.FeeOper.Dept.ID == "")
                {
                    FeeOperDeptCode = this.GetDeptByEmplId(FeeInfo.FeeOper.ID);
                }
                else
                {
                    FeeOperDeptCode = FeeInfo.FeeOper.Dept.ID;
                }
            }
            int IntResult = 0;
            if (this.Sql.GetSql("Fee.Inpatient.AddPatientAccount.2", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, FeeInfo.RecipeNO, FeeInfo.Item.MinFee.ID, FeeInfo.TransType, PatientInfo.ID, PatientInfo.Name,
                    PayKind, PactId, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.Dept.ID, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.NurseCell.ID, FeeInfo.RecipeOper.Dept.ID, FeeInfo.ExecOper.Dept.ID,
                    FeeInfo.StockOper.Dept.ID, FeeInfo.RecipeOper.ID, FeeInfo.FT.TotCost.ToString(), FeeInfo.FT.OwnCost.ToString(),
                    FeeInfo.FT.PayCost.ToString(), FeeInfo.FT.PubCost.ToString(), FeeInfo.FT.RebateCost.ToString(), FeeInfo.ChargeOper.ID,
                    FeeInfo.ChargeOper.OperTime.ToString(), FeeInfo.FeeOper.ID, FeeInfo.FeeOper.OperTime.ToString(), FeeInfo.BalanceOper.ID, FeeInfo.BalanceOper.OperTime.ToString(),
                    FeeInfo.Invoice.ID, FeeInfo.BalanceNO.ToString(), FeeInfo.BalanceState, FeeInfo.AuditingNO,
                    System.Convert.ToInt16(FeeInfo.IsBaby).ToString(),
                    FeeOperDeptCode,			//����Ա����
                    FeeInfo.ExtFlag,			//��չ���
                    FeeInfo.ExtFlag1,			//��չ���1
                    FeeInfo.ExtFlag2,			//��չ���2
                    FeeInfo.ExtCode,			//��չ����
                    FeeInfo.ExecOper.ID,		//��չ������Ա
                    FeeInfo.ExecOper.OperTime.ToString()				//��չ����
                    );
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }

            IntResult = this.ExecNoQuery(strSql);
            if (IntResult == -1 && this.DBErrCode == 1)
            {
                if (this.Sql.GetSql("Fee.Inpatient.AddPatientAccount.3", ref strSql) == 1) return -1;
                try
                {
                    #region "�ӿ�˵��"
                    //���»��߷��û��ܱ�ķ�����Ϣ 
                    //Fee.Inpatient.AddPatientAccount.3
                    //���������0  tot_cost,1 own_cost,2 pay_cost,3 pub_cost
                    //4 ������,5��С����,6.ִ�п���7�Żݷ���
                    //������������
                    #endregion
                    strSql = string.Format(strSql, FeeInfo.FT.TotCost.ToString(), FeeInfo.FT.OwnCost.ToString(), FeeInfo.FT.PayCost.ToString(),
                        FeeInfo.FT.PubCost.ToString(), FeeInfo.RecipeNO, FeeInfo.Item.MinFee.ID, FeeInfo.ExecOper.Dept.ID, FeeInfo.FT.RebateCost.ToString());
                }
                catch (Exception e)
                {
                    this.Err = e.Message;
                    this.ErrCode = e.Message;
                    return -1;
                }
                IntResult = this.ExecNoQuery(strSql);

            }
            return IntResult;
        }

        /// <summary>
        /// ���feeinfo����insert����
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="FeeInfo"></param>
        /// <returns></returns>
        [Obsolete("����,��InsertFeeInfo()", true)]
        public int AddFeeInfoRecord(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo)
        {
            string strSql = "";
            //����帺��¼���շ�ȡֵ����
            string PactId = "";
            if (FeeInfo.Patient.Pact.ID == null || FeeInfo.Patient.Pact.ID.Trim() == "")
            {
                PactId = PatientInfo.Pact.ID;
            }
            else
            {
                PactId = FeeInfo.Patient.Pact.ID;
            }
            string FeeOperDeptCode = ""; //�շ�Ա����
            //����Ա����
            if (FeeInfo.FeeOper.ID == null || FeeInfo.FeeOper.ID == "")
            {
                FeeOperDeptCode = "";
            }
            else
            {
                if (FeeInfo.FeeOper.Dept.ID == null || FeeInfo.FeeOper.Dept.ID == "")
                {
                    FeeOperDeptCode = this.GetDeptByEmplId(FeeInfo.FeeOper.ID);
                }
                else
                {
                    FeeOperDeptCode = FeeInfo.FeeOper.Dept.ID;
                }
            }



            if (this.Sql.GetSql("Fee.Inpatient.AddPatientAccount.2", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, FeeInfo.RecipeNO, FeeInfo.Item.MinFee.ID, FeeInfo.TransType, PatientInfo.ID, PatientInfo.Name,
                    FeeInfo.Patient.Pact.PayKind.ID, PactId, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.Dept.ID, ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeInfo.Patient).PVisit.PatientLocation.NurseCell.ID, FeeInfo.RecipeOper.Dept.ID, FeeInfo.ExecOper.Dept.ID,
                    FeeInfo.StockOper.Dept.ID, FeeInfo.RecipeOper.ID, FeeInfo.FT.TotCost.ToString(), FeeInfo.FT.OwnCost.ToString(),
                    FeeInfo.FT.PayCost.ToString(), FeeInfo.FT.PubCost.ToString(), FeeInfo.FT.RebateCost.ToString(), FeeInfo.ChargeOper.ID,
                    FeeInfo.ChargeOper.OperTime.ToString(), FeeInfo.FeeOper.ID, FeeInfo.FeeOper.OperTime.ToString(), FeeInfo.BalanceOper.ID, FeeInfo.BalanceOper.OperTime.ToString(),
                    FeeInfo.Invoice.ID, FeeInfo.BalanceNO.ToString(), FeeInfo.BalanceState, FeeInfo.AuditingNO,
                    System.Convert.ToInt16(FeeInfo.IsBaby).ToString(),
                    FeeOperDeptCode,			//����Ա����
                    FeeInfo.ExtFlag,			//��չ���
                    FeeInfo.ExtFlag1,			//��չ���1
                    FeeInfo.ExtFlag2,			//��չ���2
                    FeeInfo.ExtCode,			//��չ����
                    FeeInfo.ExecOper.ID,		//��չ������Ա
                    FeeInfo.ExecOper.OperTime.ToString());			//��չ����
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }

            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ��������
        /// </summary>
        /// <param name="NoteNo"></param>
        /// <param name="SequenceNO"></param>
        /// <param name="BackNum">�����˵�����</param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateNoBackQtyForUndrug()", true)]
        public int UpdateNoBackNumForUndrug(string NoteNo, decimal SequenceNO, decimal BackNum, string BalanceState)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateNoBackNumForUndrug.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, NoteNo, SequenceNO, BackNum, BalanceState);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ����ҩƷ��ϸ��������
        /// </summary>
        /// <param name="NoteNo"></param>
        /// <param name="SequenceNO"></param>
        /// <param name="BackNum"></param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateNoBackQtyForDrug()", true)]
        public int UpdateNoBackNumForDrug(string NoteNo, int SequenceNO, decimal BackNum, string BalanceState)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateNoBackNumForDrug.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, NoteNo, SequenceNO, BackNum, BalanceState);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���»��߷�����Ϣ-�����
        /// </summary>
        /// 		/// <param name="InpatientNo">������Ϣ</param>
        /// <param name="FT">������Ϣ</param>
        /// <returns>0ʧ��(û����) >0 �ɹ� -1 ����</returns>
        [Obsolete("����,��UpdateInMainInfoFee()", true)]
        public int UpdateAccount(string InpatientNo, Models.Base.FT FT)
        {
            #region "�ӿ�˵��"
            //���»�������ķ�����Ϣ 
            //Fee.InPatient.UpdateAccount.10
            //���������0 inpatientNo סԺ��ˮ��,1 OwnCost,2 pub_cost,3 Pay_cost
            //			 4 tot_cost5 �Żݽ��
            //������������
            #endregion
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.UpdateAccount.10", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, FT.OwnCost.ToString(), FT.PubCost.ToString(),
                    FT.PayCost.ToString(), FT.TotCost.ToString(), FT.RebateCost.ToString());
            }
            catch
            {
                this.Err = "�����������Fee.InPatient.UpdateAccount.10";
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���·�����ϢΪ����״̬ Written By ���峬--------------��ʱû��,����Ч���������ֱ��ȫ��update
        /// </summary>
        /// <param name="FeeInfo">������Ϣ</param>
        /// <param name= "Balance">������Ϣ</param>>
        /// <returns></returns>
        [Obsolete("����,��UpdateFeeInfoBalanced()", true)]
        public int UpdateAccoutBalanced(Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo, Neusoft.HISFC.Models.Fee.Inpatient.Balance Balance)
        {
            #region
            //���·�����ϢΪ����״̬
            //UpdateAccoutBalanced.1
            //���������0  ������,1 ִ�п���,2��С���� 3�������4����Ա5����ʱ��
            //������������
            #endregion

            string strSql = "";
            if (this.Sql.GetSql("UpdateAccoutBalanced.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, FeeInfo.RecipeNO, FeeInfo.ExecOper.Dept.ID, FeeInfo.Item.MinFee.ID, Balance.ID,
                    Balance.Oper.ID, Balance.Oper.OperTime.ToString());
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���ۺ�����շѱ���շ����շ�ʱ�������Ϣ
        /// </summary>
        /// <param name="ItemList"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateChargeItemToFee()", true)]
        public int UpdateChargeItem(Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList)
        {

            string strSql = "";
            //����Ա����
            string FeeOperDeptCode = "";
            if (ItemList.FeeOper.ID == null || ItemList.FeeOper.ID == "")
            {
                FeeOperDeptCode = "";
            }
            else
            {
                if (ItemList.FeeOper.Dept.ID == null || ItemList.FeeOper.Dept.ID == "")
                {

                    FeeOperDeptCode = this.GetDeptByEmplId(ItemList.FeeOper.ID);
                }
                else
                {
                    FeeOperDeptCode = ItemList.FeeOper.Dept.ID;
                }
            }

            //0 ������ 1��������ˮ��2�Ʒ���3�Ʒ�ʱ��4��ҩ״̬5�շ�Ա����

            if (ItemList.Item.IsPharmacy)
            {
                if (this.Sql.GetSql("UpdateDrugItem.2", ref strSql) == -1) return -1;
            }
            else
            {
                if (this.Sql.GetSql("UpdateUndrugItem.2", ref strSql) == -1) return -1;
            }
            try
            {
                strSql = string.Format(strSql, ItemList.RecipeNO, ItemList.SequenceNO.ToString(),
                    ItemList.FeeOper.ID, ItemList.FeeOper.OperTime.ToString(), ((int)ItemList.PayType).ToString(),
                    //�շ�Ա����
                    FeeOperDeptCode
                    );
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }

            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="MedItemList"></param>
        /// <returns></returns>
        [Obsolete("����,��InsertMedItemList()", true)]
        public int AddPatientMedAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.FeeItemList MedItemList)
        {
            return this.InsertMedItemList(PatientInfo, MedItemList, null);
        }

        /// <summary>
        /// ��ӻ��� ��Ŀ����-����ҩƷ������ϸ����Ϣ  Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="MedItemList">ҩƷ������Ŀ��Ϣ</param>
        /// <param name="Insurance"></param>
        /// <returns>0 �ɹ� -1 ʧ��</returns>
        [Obsolete("����,��InsertMedItemList()", true)]
        public int AddPatientMedAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.FeeItemList MedItemList, Neusoft.HISFC.Models.Insurance.IInsurence Insurance)
        {
            string strSql = "";
            string ApprNo = "";
            string CenterCode = "";
            bool IsEmergency = false;

            string FeeOperDeptCode = ""; //�շ�Ա����
            //����Ա����
            if (MedItemList.FeeOper.ID == null || MedItemList.FeeOper.ID == "")
            {
                FeeOperDeptCode = "";
            }
            else
            {
                if (MedItemList.FeeOper.Dept.ID == null || MedItemList.FeeOper.Dept.ID == "")
                {
                    FeeOperDeptCode = this.GetDeptByEmplId(MedItemList.FeeOper.ID);
                }
                else
                {
                    FeeOperDeptCode = MedItemList.FeeOper.Dept.ID;
                }
            }


            if (Insurance == null)
            {
                ApprNo = null;
                CenterCode = null;
                IsEmergency = false;
            }
            else
            {
                ApprNo = Insurance.ApprNo;
                CenterCode = Insurance.CenterCode;
                IsEmergency = Insurance.IsEmergency;
            }
            #region
            //��ӻ���ҩƷ������ϸ��ķ�����Ϣ
            //0 ������1��������ˮ��2 ��������,1�����ף�2������ 3סԺ��ˮ�� 4���� 5������� 6��ͬ��λ 7��Ժ���Ҵ���8��ʿվ����
            //9�������Ҵ��� 10ִ�п��Ҵ���11ȡҩ���Ҵ���12����ҽʦ����13ҩƷ����14��С���ô���15ҽ��������Ŀ����16ҩƷ����17���
            //18ҩƷ���19ҩƷ����20���Ʊ�ʶ21����22��ǰ��λ23��װ��24����25����26���ý��27�Էѽ��28�Ը����29���ѽ��30�Żݽ��
            //31��ҩ����ˮ��32�ۿ���ˮ��33��ҩ״̬��0 ���� 2��ҩ 1���ѣ�34�Ƿ�Ӥ����ҩ 0 ���� 1 ��35�������ȱ�־36��Ժ��ҩ��� 0 �� 1 ��
            //37���㷢Ʊ��38�������39������40������41��������42��ҩ��43��ҩ���� 44�Ʒ�ʱ��45������46ҽ����ˮ��47ҽ��ִ�е���ˮ��
            //48�Ʒ���49ҩƷ��������50����״̬51�շѱ��� 52 �շ�Ա����
            #endregion
            if (this.Sql.GetSql("AddPatientAccount.2", ref strSql) == -1) return -1;
            if (MedItemList.Item.GetType() != typeof(Neusoft.HISFC.Models.Pharmacy.Item)) return -1;
            Neusoft.HISFC.Models.Pharmacy.Item Obj = (Neusoft.HISFC.Models.Pharmacy.Item)MedItemList.Item;
            try
            {


                //����帺��¼���շ�ȡֵ����
                string PactId = "";
                if (MedItemList.Patient.Pact.ID == null || MedItemList.Patient.Pact.ID.Trim() == "")
                {
                    PactId = PatientInfo.Pact.ID;
                }
                else
                {
                    PactId = MedItemList.Patient.Pact.ID;
                }
                string PayKindId = "";
                if (MedItemList.Patient.Pact.PayKind.ID == null || MedItemList.Patient.Pact.PayKind.ID.Trim() == "")
                {
                    PayKindId = PatientInfo.Pact.PayKind.ID;
                }
                else
                {
                    PayKindId = MedItemList.Patient.Pact.PayKind.ID;
                }
                //���Ŀǰȫ����������С������ȡ
                if (MedItemList.Item.PriceUnit == "")
                {
                    MedItemList.Item.PriceUnit = Obj.MinUnit;
                }
                string[] s = 
				{
					MedItemList.RecipeNO,
					MedItemList.SequenceNO.ToString(),
					((int)MedItemList.TransType).ToString(),
					PatientInfo.ID,
					PatientInfo.Name,
					PayKindId,
					PactId,
					((Neusoft.HISFC.Models.RADT.PatientInfo)MedItemList.Patient).PVisit.PatientLocation.Dept.ID,
					((Neusoft.HISFC.Models.RADT.PatientInfo)MedItemList.Patient).PVisit.PatientLocation.NurseCell.ID,
					MedItemList.RecipeOper.Dept.ID,//10
					MedItemList.ExecOper.Dept.ID,
					MedItemList.StockOper.Dept.ID,
					MedItemList.RecipeOper.ID,
					MedItemList.Item.ID,
					MedItemList.Item.MinFee.ID,
					CenterCode,
					MedItemList.Item.Name,
					MedItemList.Item.Specs,
					Obj.Type.ID,
					Obj.Quality.ID.ToString(),
					NConvert.ToInt32(Obj.Product.IsSelfMade).ToString(),//20
					MedItemList.Item.Price.ToString(),MedItemList.Item.PriceUnit,MedItemList.Item.PackQty.ToString(),MedItemList.Item.Qty.ToString(),
					MedItemList.Days.ToString(),MedItemList.FT.TotCost.ToString(),MedItemList.FT.OwnCost.ToString(),
					MedItemList.FT.PayCost.ToString(),MedItemList.FT.PubCost.ToString(),MedItemList.FT.RebateCost.ToString(),
					MedItemList.SendSequence.ToString(),MedItemList.UpdateSequence.ToString(),
					((int)MedItemList.PayType).ToString(),System.Convert.ToInt16(MedItemList.IsBaby).ToString(),
					System.Convert.ToInt16(IsEmergency).ToString(),((Models.Order.Inpatient.Order)MedItemList.Order).OrderType.ID,
					MedItemList.Invoice.ID,MedItemList.BalanceNO.ToString(),ApprNo,MedItemList.ChargeOper.ID,
					MedItemList.ChargeOper.OperTime.ToString(),MedItemList.ExecOper.ID,MedItemList.ExecOper.OperTime.ToString(),MedItemList.FeeOper.OperTime.ToString(),
					MedItemList.AuditingNO,MedItemList.Order.ID,MedItemList.Order.ID,MedItemList.FeeOper.ID,MedItemList.NoBackQty.ToString(),
					MedItemList.BalanceState,MedItemList.FTRate.OwnRate.ToString(),	//51 �շѱ���
					FeeOperDeptCode,		//52 �շ�Ա����
					MedItemList.Item.SpecialFlag,	//53 ��չ���
					MedItemList.Item.SpecialFlag1,	//54 ��չ���1
					MedItemList.Item.SpecialFlag2	//55 ��չ���2 MedItemList.ExtCode,	//56 ��չ����MedItemList.ExtOperCode,//57 ��չ����Ա����MedItemList.ExecOper.OperTime.ToString().ToString() //58 ��չ����
					
				};

                strSql = string.Format(strSql, s);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="FeeItemList"></param>
        /// <returns></returns>
        [Obsolete("����,��InsertFeeItemList()", true)]
        public int AddPatientAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.FeeItemList FeeItemList)
        {
            return this.InsertFeeItemList(PatientInfo, FeeItemList, null);
        }


        /// <summary>
        /// ��ӻ��� ��Ŀ����-���·�ҩƷ������ϸ����Ϣ  Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="FeeItemList">���ķ�����Ŀ��Ϣ</param>
        ///  <param name="Insurence">ҽ������Ŀ�����Ϣ</param>
        /// <returns><br>0 �ɹ�</br><br>-1 ʧ��</br></returns>
        [Obsolete("����,��InsertFeeItemList()", true)]
        public int AddPatientAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.FeeItemList FeeItemList, Neusoft.HISFC.Models.Insurance.IInsurence Insurence)
        {

            string strSql = "";
            string ApprNo = "";  //������
            string CenterCode = ""; //���Ĵ���
            bool IsEmergency = false; //������
            string FeeOperDeptCode = ""; //�շ�Ա����
            //����Ա����
            if (FeeItemList.FeeOper.ID == null || FeeItemList.FeeOper.ID == "")
            {
                FeeOperDeptCode = "";
            }
            else
            {
                if (FeeItemList.FeeOper.Dept.ID == null || FeeItemList.FeeOper.Dept.ID == "")
                {

                    FeeOperDeptCode = this.GetDeptByEmplId(FeeItemList.FeeOper.Dept.ID);
                }
                else
                {
                    FeeOperDeptCode = FeeItemList.FeeOper.Dept.ID;
                }
            }


            if (Insurence == null)
            {
                ApprNo = null;
                CenterCode = null;
                IsEmergency = false;
            }
            else
            {
                ApprNo = Insurence.ApprNo;
                CenterCode = Insurence.CenterCode;
                IsEmergency = Insurence.IsEmergency;
            };

            //			Neusoft.HISFC.Models.Fee.Item.Undrug Obj = (Neusoft.HISFC.Models.Fee.Item.Undrug)FeeItemList.Item;
            if (this.Sql.GetSql("AddPatientAccount.1", ref strSql) == -1) return -1;
            try
            {
                //����帺��¼���շ�ȡֵ����
                string PactId = "";

                if (FeeItemList.Patient.Pact.ID == null || FeeItemList.Patient.Pact.ID.Trim() == "")
                {
                    PactId = PatientInfo.Pact.ID;
                }
                else
                {
                    PactId = FeeItemList.Patient.Pact.ID;
                }
                string PayKindId = "";
                if (FeeItemList.Patient.Pact.PayKind.ID == null || FeeItemList.Patient.Pact.PayKind.ID.Trim() == "")
                {
                    PayKindId = PatientInfo.Pact.PayKind.ID;
                }
                else
                {
                    PayKindId = FeeItemList.Patient.Pact.PayKind.ID;
                }
                string[] s = {
								 #region 
								 //��ӻ��߷�ҩƷ������ϸ��ķ�����Ϣ 
								 //AddPatientAccount.1
								 //���������0 ������1��С���ô���2 ��������,1�����ף�2������ 3סԺ��ˮ�� 4���� 5������� 6��ͬ��λ 7��Ժ���Ҵ���8��ʿվ����
								 //9�������Ҵ��� 10ִ�п��Ҵ���11�ۿ���Ҵ���12����ҽʦ����13��Ŀ����14���Ĵ���15����16����17���״���18��������
								 //19���ý��20�Էѽ��21�Ը����22���ѽ��23�Żݽ��25����״̬26�Ƿ�Ӥ����27�������ȱ�־28��Ժ���Ʊ��
								 //29���㷢Ʊ��30�������31������32������33��������34�豸��35�Ʒ���36�Ʒ�����38������39ҽ����ˮ��40ҽ��ִ�е���ˮ��
								 //42��������ˮ��43ִ����44ִ������45��Ŀ����24��ǰ��λ37���ⵥ��ˮ��41�ۿ���ˮ��46��������47����״̬48�շѱ��� 49 �շ�Ա����
								 //������������
								 #endregion
								 // 0 ������
								 FeeItemList.RecipeNO,
								 //1��С���ô���
								 FeeItemList.Item.MinFee.ID,
								 //��������,1�����ף�2������
								 ((int)FeeItemList.TransType).ToString(),
								 //סԺ��ˮ��
								 PatientInfo.ID,
								 //����
								 PatientInfo.Name,
								 //�������
								 PayKindId,
								 //��ͬ��λ
								 PactId,
								 //��Ժ���Ҵ���
								 ((Neusoft.HISFC.Models.RADT.PatientInfo)FeeItemList.Patient).PVisit.PatientLocation.Dept.ID,
								 "",FeeItemList.RecipeOper.Dept.ID,FeeItemList.ExecOper.Dept.ID,FeeItemList.StockOper.Dept.ID,
								 FeeItemList.RecipeOper.ID,FeeItemList.Item.ID,CenterCode,FeeItemList.Item.Price.ToString(),FeeItemList.Item.Qty.ToString(),
								 FeeItemList.UndrugComb.ID,FeeItemList.UndrugComb.Name,FeeItemList.FT.TotCost.ToString(),FeeItemList.FT.OwnCost.ToString(),
								 FeeItemList.FT.PayCost.ToString(),FeeItemList.FT.PubCost.ToString(),FeeItemList.FT.RebateCost.ToString(),
								 FeeItemList.Item.PriceUnit,
								 ((int)FeeItemList.PayType).ToString(),System.Convert.ToInt16(FeeItemList.IsBaby).ToString(),
								 System.Convert.ToInt16(IsEmergency).ToString(),((Neusoft.HISFC.Models.Order.Inpatient.Order)FeeItemList.Order).OrderType.ID,
								 FeeItemList.Invoice.ID,FeeItemList.BalanceNO.ToString(),ApprNo,FeeItemList.ChargeOper.ID,
								 FeeItemList.ChargeOper.OperTime.ToString(),FeeItemList.MachineNO,FeeItemList.FeeOper.ID,FeeItemList.FeeOper.OperTime.ToString(),
								 FeeItemList.SendSequence.ToString(),
								 FeeItemList.AuditingNO,FeeItemList.Order.ID,FeeItemList.Order.ID,FeeItemList.UpdateSequence.ToString(),FeeItemList.SequenceNO.ToString(),
								 FeeItemList.ExecOper.ID,FeeItemList.ExecOper.OperTime.ToString(),FeeItemList.Item.Name,FeeItemList.NoBackQty.ToString(),FeeItemList.BalanceState,
								 FeeItemList.FTRate.OwnRate.ToString(),
								 FeeOperDeptCode,			//49 �շ�Ա����
								 FeeItemList.Item.SpecialFlag,		//50 ��չ���
								 FeeItemList.Item.SpecialFlag1,		//51 ��չ���1
								 FeeItemList.Item.SpecialFlag2,		//52 ��չ���2
								 FeeItemList.User01,		//53 ��չ����
								 FeeItemList.User02,	//54 ��չ����Ա����
								 FeeItemList.User03 //55 ��չ����
							 };

                strSql = string.Format(strSql, s);

            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);

        }

        /// <summary>
        /// ��÷�ҩƷ��ϸ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsForDirQuit()", true)]
        public ArrayList GetDirQuitFeeItemList(string inpatientNo, string invoiceNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.GetDirQuitFeeItemList.Select.1", ref strSql) == -1)
            {
                this.ErrCode = "-1";
                this.Err = "û���ҵ�Fee.GetDirQuitFeeItemList.Select.1";
                this.WriteErr();
                return null;
            }
            try
            {
                strSql = string.Format(strSql, inpatientNo, invoiceNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = "-1";
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
            this.ExecQuery(strSql);
            try
            {
                ArrayList al = new ArrayList();
                while (Reader.Read())
                {
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList obj = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();

                    obj.RecipeNO = Reader[0].ToString();
                    obj.SequenceNO = NConvert.ToInt32(Reader[1].ToString());
                    obj.Item.ID = Reader[2].ToString();
                    obj.Item.Name = Reader[3].ToString();
                    obj.Item.Price = NConvert.ToDecimal(Reader[4].ToString());
                    obj.NoBackQty = NConvert.ToDecimal(Reader[5].ToString());
                    obj.Item.PriceUnit = Reader[6].ToString();
                    obj.FT.TotCost = NConvert.ToDecimal(Reader[7].ToString());
                    obj.FT.OwnCost = NConvert.ToDecimal(Reader[8].ToString());
                    obj.FT.PayCost = NConvert.ToDecimal(Reader[9].ToString());
                    obj.FT.PubCost = NConvert.ToDecimal(Reader[10].ToString());
                    obj.ExecOper.Dept.ID = Reader[11].ToString();
                    if (!Reader.IsDBNull(12))
                        obj.FeeOper.OperTime = NConvert.ToDateTime(Reader[12].ToString());
                    obj.Item.SpellCode = Reader[13].ToString();
                    obj.Item.MinFee.ID = Reader[14].ToString();
                    obj.Memo = Reader[15].ToString();
                    al.Add(obj);
                }
                Reader.Close();
                return al;
            }
            catch (Exception ex)
            {
                this.ErrCode = "-1";
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
        }

        /// <summary>
        /// ���ҩƷ��ϸ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListsForDirQuit()", true)]
        public ArrayList GetDirQuitFeeMedList(string inpatientNo, string invoiceNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.GetDirQuitFeeMedList.Select.1", ref strSql) == -1)
            {
                this.ErrCode = "-1";
                this.Err = "û���ҵ�Fee.GetDirQuitFeeMedList.Select.1";
                this.WriteErr();
                return null;
            }
            try
            {
                strSql = string.Format(strSql, inpatientNo, invoiceNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = "-1";
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
            this.ExecQuery(strSql);
            try
            {
                ArrayList al = new ArrayList();
                while (Reader.Read())
                {
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList obj = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();

                    obj.RecipeNO = Reader[0].ToString();
                    obj.SequenceNO = NConvert.ToInt32(Reader[1].ToString());
                    obj.Item.ID = Reader[2].ToString();
                    obj.Item.Name = Reader[3].ToString();
                    obj.Item.Specs = Reader[4].ToString();
                    obj.Item.Price = NConvert.ToDecimal(Reader[5].ToString());
                    obj.NoBackQty = NConvert.ToDecimal(Reader[6].ToString());
                    obj.Item.PriceUnit = Reader[7].ToString();
                    obj.FT.TotCost = NConvert.ToDecimal(Reader[8].ToString());
                    obj.FT.OwnCost = NConvert.ToDecimal(Reader[9].ToString());
                    obj.FT.PayCost = NConvert.ToDecimal(Reader[10].ToString());
                    obj.FT.PubCost = NConvert.ToDecimal(Reader[11].ToString());

                    if (!Reader.IsDBNull(12))
                        obj.FeeOper.OperTime = NConvert.ToDateTime(Reader[12].ToString());
                    obj.Memo = Reader[13].ToString();
                    obj.Item.SpellCode = Reader[14].ToString();
                    obj.Item.MinFee.ID = Reader[15].ToString();
                    obj.Memo = Reader[16].ToString();
                    al.Add(obj);
                }
                Reader.Close();
                return al;
            }
            catch (Exception ex)
            {
                if (Reader.IsClosed == false)
                {
                    Reader.Close();
                }
                this.ErrCode = "-1";
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
        }

        /// <summary>
        /// �����������޶�����޶��ۼƺͳ����� ---�������޶���
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="difCost"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateInMainInfoForChangeDayLimit()", true)]
        public int UpdateInmaininfoForChangeDayLimit(string InpatientNo, decimal difCost)
        {
            string strSql = "";
            // 0 סԺ��ˮ��1�����ͱ��ǰ���Ĳ�ֵ
            if (this.Sql.GetSql("Fee.UpdateInmaininfoForChangeDayLimit", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, difCost.ToString());
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);

        }

        /// <summary>
        /// ���»�������������
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="NewBalNo"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateInMainInfoBalanceNO()", true)]
        public int UpdateInmaininfoBalanceNo(string InpatientNo, string NewBalNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateInmaininfoBalanceNo.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, NewBalNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// �����ٻظ��»���ҩƷ������ϸ�������
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="OldbalNo"></param>
        /// <param name="NewbalNo"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateMedItemListsBalanceNO()", true)]
        public int UpdateMedItemBalanceNo(string InpatientNo, string OldbalNo, string NewbalNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateMedItemBalanceNo.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, OldbalNo, NewbalNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// �����ٻظ��»��߷�ҩƷ������ϸ�������
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="OldbalNo"></param>
        /// <param name="NewbalNo"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateFeeItemListsBalanceNO()", true)]
        public int UpdateFeeItemBalanceNo(string InpatientNo, string OldbalNo, string NewbalNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateFeeItemBalanceNo.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, OldbalNo, NewbalNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        ///  ����ҩƷ��ϸ,Ϊ��Ʊ�����ṩ,ֻ���½������,�ͷ�Ʊ��
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="oldInvoiceNo"></param>
        /// <param name="newBalNo"></param>
        /// <param name="newInvoiceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateMedItemListsBalanceNOForReprint()", true)]
        public int UpdateMedItemBalanceNoForReprint(string inpatientNo, string oldInvoiceNo, string newBalNo, string newInvoiceNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateMedItemBalanceNoForReprint.1", ref strSql) == -1)
                return -1;
            try
            {
                strSql = string.Format(strSql, inpatientNo, oldInvoiceNo, newInvoiceNo, newBalNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }

            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ,Ϊ��Ʊ�����ṩ,ֻ���½������,�ͷ�Ʊ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="orgInvoiceNO">ԭʼ��Ʊ��</param>
        /// <param name="newInvoiceNO">�·�Ʊ��</param>
        /// <param name="newBalanceNO">�½������</param>
        /// <returns>�ɹ�:1 ʧ��: -1 û�и��µ�����: 0</returns>
        [Obsolete("����,��UpdateFeeItemListsBalanceNOForReprint()", true)]
        public int UpdateFeeItemBalanceNoForReprint(string inpatientNO, string orgInvoiceNO, string newInvoiceNO, int newBalanceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateFeeItemBalanceNoForReprint.1", inpatientNO, orgInvoiceNO, newInvoiceNO, newBalanceNO.ToString());
        }

        /// <summary>
        /// ��ȡĳ�˽���balancepay��Ϣ
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <param name="BalNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancePaysByInvoiceNOAndBalanceNO()", true)]
        public ArrayList GetBalancePayByInvoiceAndBalNo(string InvoiceNo, string BalNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectBalancePay();
            if (this.Sql.GetSql("Fee.GetBalancePayByInvoiceAndBalNo.1", ref strSql2) == -1) return null;
            strSql1 += strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InvoiceNo, BalNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryBalancePaysBySql(strSql1);
        }

        /// <summary>
        /// ͨ����Ʊ�Ÿ��������,����balanceList�Ķ�Ӧ��Ϣ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="invoiceNo"></param>
        /// <param name="balanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalanceListsByInpatientNOAndBalanceNO()", true)]
        public ArrayList GetBalanceListByInvoiceAndBalNo(string inpatientNo, string invoiceNo, string balanceNo)
        {
            string strSql1 = "";
            string strSql2 = "";

            strSql1 = this.GetSqlForSelectAllInfoFromBalanceList();
            if (this.Sql.GetSql("Fee.GetBalanceListByInvoiceAndBalNo.1", ref strSql2) == -1)
                return null;
            strSql1 += strSql2;
            try
            {
                strSql1 = string.Format(strSql1, inpatientNo, invoiceNo, balanceNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }

            return this.QueryBalanceListsBySql(strSql1);
        }

        /// <summary>
        /// ��ȡĳ�˽���balancelist��Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalanceListsByInpatientNOAndBalanceNO()", true)]
        public ArrayList GetBalanceListByBalNo(string InpatientNo, string BalanceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllInfoFromBalanceList();
            if (this.Sql.GetSql("Fee.GetBalanceListByBalNo.1", ref strSql2) == -1) return null;
            strSql1 += strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, BalanceNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryBalanceListsBySql(strSql1);
        }

        /// <summary>
        /// ��ȡĳ�˽����Ԥ�����¼
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryPrepaysByInpatientNOAndBalanceNO()", true)]
        public ArrayList GetPrepayByBalNo(string InpatientNo, string BalanceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllPrepay();
            if (this.Sql.GetSql("Fee.GetPrepayByBalNo.1", ref strSql2) == -1) return null;
            strSql1 += strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, BalanceNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryPrepaysBySql(strSql1);
        }

        /// <summary>
        /// ��ȡĳ�˽����feeinfo��Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosByInpatientNOAndBalanceNO()", true)]
        public ArrayList GetFeeInfoBalanceByBalNo(string InpatientNo, string BalanceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectFeeInfo();
            if (this.Sql.GetSql("Fee.GetFeeInfoBalanceByBalNo.1", ref strSql2) == -1) return null;
            strSql1 += strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, BalanceNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeInfosBySql(strSql1);
        }

        /// <summary>
        /// ���ݷ�Ʊ�źͽ�����Ż�ȡ
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="balanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalanceListsByInvoiceNOAndBalanceNO()", true)]
        public ArrayList GetBalanceListByInvoiceAndBalance(string invoiceNo, string balanceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllInfoFromBalanceList();
            if (this.Sql.GetSql("GetBalanceListInfoByInvoiceAndbalance.Where", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, invoiceNo, balanceNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalanceListsBySql(strSql1);

        }

        /// <summary>
        /// ͨ����Ʊ�������������ϸ������Ϣ
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalanceListsByInvoiceNO()", true)]
        public ArrayList GetBalanceListInfoByInvoice(string invoiceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllInfoFromBalanceList();
            if (this.Sql.GetSql("GetBalanceListInfoByInvoice.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, invoiceNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalanceListsBySql(strSql1);

        }

        /// <summary>
        /// ͨ���������סԺ��ˮ�ż�������ͷ��Ʊ�������Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="balNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByBalanceNO()", true)]
        public ArrayList GetBalanceInfoBybalNo(string InpatientNo, int balNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllInfoFromBalanceHead();
            if (this.Sql.GetSql("GetBalanceInfoBybalNo.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, balNo.ToString(), InpatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalancesBySql(strSql1);
        }

        /// <summary>
        /// ����Աʵ����ϸ,�������������ϵ�����
        /// </summary>
        /// <param name="Begin">��ʼʱ��</param>
        /// <param name="End">����ʱ��</param>
        /// <param name="Oper">����Ա ALL ȫ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByTime()", true)]
        public ArrayList GetBalanceInfoDetailByDate(string Begin, string End, string Oper)
        {
            string strSql = "";
            string strSql1 = "", strSql2 = "", strSql3 = "";
            try
            {
                strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref strSql1) == -1) return null;

                strSql1 = string.Format(strSql1, Begin, End);
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where.NoReprint", ref strSql2) == -1) return null;
                strSql = strSql + " " + strSql2;
                if (Oper != "ALL")
                {
                    if (this.Sql.GetSql("GetBalanceInfoByDate.Where.2", ref strSql3) == -1) return null;
                    strSql3 = string.Format(strSql3, Oper);
                    strSql = strSql + " " + strSql3;
                }

            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            return this.QueryBalancesBySql(strSql);
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="Begin">��ʼʱ��</param>
        /// <param name="End">����ʱ��</param>
        /// <param name="Tag">�Ƿ���ʾ������</param>
        /// <param name="Oper">����Ա</param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByTime()", true)]
        public ArrayList GetBalanceInfoByDate(string Begin, string End, bool Tag, string Oper)
        {
            string strSql = "";
            string strSql1 = "", strSql2 = "", strSql3 = "";
            try
            {
                strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref strSql1) == -1) return null;

                strSql1 = string.Format(strSql1, Begin, End);
                strSql = strSql + strSql1;
                if (Tag)
                {
                    if (this.Sql.GetSql("GetBalanceInfoByDate.Where.1", ref strSql2) == -1) return null;
                    strSql = strSql + " " + strSql2;
                }
                if (Oper != "ALL")
                {
                    if (this.Sql.GetSql("GetBalanceInfoByDate.Where.2", ref strSql3) == -1) return null;
                    strSql3 = string.Format(strSql3, Oper);
                    strSql = strSql + " " + strSql3;
                }
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            return this.QueryBalancesBySql(strSql);
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="Begin">��ʼʱ��</param>
        /// <param name="End">����ʱ��</param>
        /// <param name="Tag">�Ƿ�ֻ��ʾ������</param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByTime()", true)]
        public ArrayList GetBalanceInfoByDate(string Begin, string End, bool Tag)
        {
            string strSql = "";
            string strSql1 = "", strSql2 = "";
            try
            {
                strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
                if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref strSql1) == -1) return null;
                strSql = strSql + strSql1;
                if (Tag)
                {

                    strSql = string.Format(strSql, Begin, End);

                    if (this.Sql.GetSql("GetBalanceInfoByDate.Where.1", ref strSql2) == -1) return null;
                    strSql = strSql + " " + strSql2;
                }
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            return this.QueryBalancesBySql(strSql);
        }

        /// <summary>
        /// ����ʱ�������Ʊͷ
        /// </summary>
        /// <param name="Begin"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByTime()", true)]
        public ArrayList GetBalanceInfoByDate(string Begin, string End)
        {
            string strSql = "";
            string strSql1 = "";
            strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
            if (this.Sql.GetSql("GetBalanceInfoByDate.Where", ref strSql1) == -1) return null;
            strSql = strSql + strSql1;
            try
            {
                strSql = string.Format(strSql, Begin, End);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalancesBySql(strSql);
        }

        /// <summary>
        /// ͨ����Ʊ�����������ͷ�������Ϣ
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByInvoiceNO()", true)]
        public ArrayList GetBalanceInfoByInvoice(string InvoiceNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetSqlForSelectAllInfoFromBalanceHead();
            if (this.Sql.GetSql("GetBalanceInfoByInvoice.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InvoiceNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalancesBySql(strSql1);
        }

        /// <summary>
        /// ����סԺ��ˮ�Ż�ȡδ���ϵķ�Ʊ��
        /// </summary>
        /// <param name="inpatientNo">סԺ��ˮ��</param>
        /// <param name="flag">ALL ȫ����O ��Ժ���㣬I ��;����</param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByInpatientNO()", true)]
        public ArrayList GetBalanceHeadInfoByInpatientNo(string inpatientNo, string flag)
        {
            string strSql = "";
            string strSqlWhere = "";
            strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
            if (flag == "" || flag == "ALL")
            {
                if (this.Sql.GetSql("Fee.GetBalanceHeadInfoByInpatientNo.Select.2", ref strSqlWhere) == -1)
                    return null;
            }
            else if (flag == "O")
            {
                if (this.Sql.GetSql("Fee.GetBalanceHeadInfoByInpatientNo.Select.3", ref strSqlWhere) == -1)
                    return null;
            }
            else
            {
                if (this.Sql.GetSql("Fee.GetBalanceHeadInfoByInpatientNo.Select.4", ref strSqlWhere) == -1)
                    return null;
            }
            strSql = strSql + strSqlWhere;
            try
            {
                strSql = string.Format(strSql, inpatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalancesBySql(strSql);

        }

        /// <summary>
        /// ͨ��סԺ��ˮ�Ų�ѯ���߽���ͷ����Ϣ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryBalancesByInpatientNO()", true)]
        public ArrayList GetBalanceHeadInfoByInpatientNo(string inpatientNo)
        {
            string strSql = "";
            string strSqlWhere = "";
            strSql = this.GetSqlForSelectAllInfoFromBalanceHead();
            if (this.Sql.GetSql("Fee.GetBalanceHeadInfoByInpatientNo.Select.1", ref strSqlWhere) == -1)
                return null;
            strSql = strSql + strSqlWhere;
            try
            {
                strSql = string.Format(strSql, inpatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.QueryBalancesBySql(strSql);

        }

        /// <summary>
        /// ֱ�ӽ����˷�,ͨ����Ʊ�Ż�÷�ҩƷ������ϸ
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListByInvoiceNO()", true)]
        public ArrayList GetUndrugItemFromInvoice(string invoiceNo, DateTime dtBegin, DateTime dtEnd)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetUndrugItemFromInvoice.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, invoiceNo, dtBegin, dtEnd);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }

        /// <summary>
        /// ֱ�ӽ����˷�,ͨ����Ʊ�Ż��ҩƷ������ϸ
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListByInvoiceNO()", true)]
        public ArrayList GetMedItemFromInvoice(string invoiceNo, DateTime dtBegin, DateTime dtEnd)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetMedItemFromInvoice.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, invoiceNo, dtBegin, dtEnd);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            //ReadDetailUndrug
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// ������ķ�ҩƷ������ϸ��Ϣ��Ϊ����״̬--��;���㰴ʱ�����С����update
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceNo"></param>
        /// <param name="InvoiceNo"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="FeeCode"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateFeeItemListBalanced()", true)]
        public int UpdateItemListBalanced(string InpatientNo, int BalanceNo, string InvoiceNo, DateTime dt1, DateTime dt2, string FeeCode)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateItemListBalanced.2", ref strSql) == -1) return -1;
            try
            {
                //0 סԺ��ˮ��1�������2���㷢Ʊ��3��ʼʱ��4����ʱ��
                strSql = string.Format(strSql, InpatientNo, BalanceNo, InvoiceNo, dt1, dt2, FeeCode);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ������ķ�ҩƷ������ϸ��Ϣ��Ϊ����״̬--��Ժ����ȫupdateʹ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="balanceNO">�������</param>
        /// <param name="invoiceNO">���㷢Ʊ��</param>
        /// <returns>�ɹ�:>=1 ʧ��: -1 û�и��µ�����:0</returns>
        [Obsolete("����,��UpdateFeeItemListBalanced()", true)]
        public int UpdateItemListBalanced(string inpatientNO, int balanceNO, string invoiceNO)
        {
            return this.UpdateSingleTable("Fee.UpdateItemListBalanced.1", inpatientNO, balanceNO.ToString(), invoiceNO);
        }

        /// <summary>
        /// ����µĽ������Written By ���峬
        /// </summary>
        /// <param name="InpatientNo">����סԺ��ˮ��</param>
        /// <returns>���½�����ż����ν���������</returns>
        /// GetNewBalanceNo.No1
        [Obsolete("����,��GetNewBalanceNO()", true)]
        public string GetNewBalanceNo(string InpatientNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.GetNewBalanceNo.No1", ref strSql) == -1) return null;

            try
            {
                strSql = string.Format(strSql, InpatientNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.ExecSqlReturnOne(strSql);

        }

        /// <summary>
        ///  ����סԺ���������Ϣ
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="dtBalance"></param>
        /// <param name="balNo"></param>
        /// <param name="Fee"></param>
        /// <returns></returns>
        [Obsolete("����,��UpdateInMainInfoBalanced()", true)]
        public int UpdateInMaininfoBalanced(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, DateTime dtBalance, int balNo, Neusoft.HISFC.Models.Base.FT Fee)
        {
            //0 סԺ��ˮ��1����ʱ��2Ԥ����3תѺ��4������5�Էѽ��6pub���7pay���8�Żݽ�� 9�������10��Ժ״

            //̬11ת�����12ת��Ԥ����
            string strSql = "";
            if (this.Sql.GetSql("Fee.UpdateInMaininfoBalanced.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, PatientInfo.ID, dtBalance.ToString(), Fee.PrepayCost.ToString(), Fee.TransferPrepayCost.ToString(),
                    Fee.TotCost.ToString(), Fee.OwnCost.ToString(), Fee.PubCost.ToString(), Fee.PayCost.ToString(), Fee.RebateCost.ToString(),
                    balNo, PatientInfo.PVisit.InState.ID, Fee.TransferTotCost.ToString(), Fee.TransferPrepayCost.ToString());

            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ��ѯ�����ʿ���
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <returns>���ʱ�־1����0����</returns>
        [Obsolete("����,��GetStopAccount()", true)]
        public string QueryStopAccount(string InpatientNo)
        {
            string strSql;
            strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.QueryStopAccount.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            return this.ExecSqlReturnOne(strSql);

        }

        /// <summary>
        /// ����ҩƷ�ͷ�ҩƷ��ϸ������¼---ͨ������
        /// </summary>
        /// <param name="NoteNo"></param>
        /// <param name="SequenceNO"></param>
        /// <param name="IsPharmacy"></param>
        /// <returns></returns>
        [Obsolete("����,��GetItemListByRecipeNO()", true)]
        public Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList GetFeeItemListByNoteNoAndNoteSequence(string NoteNo, int SequenceNO, bool IsPharmacy)
        {

            //0 ������1��������ˮ��
            string strSql1 = "";
            string strSql2 = "";
            ArrayList alItem = new ArrayList();
            Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
            if (IsPharmacy)
            {
                strSql1 = this.GetMedItemListSelectSql();
                if (this.Sql.GetSql("Fee.GetFeeItemListByNoteNoAndNoteSequence.1", ref strSql2) == -1) return null;
                strSql2 = string.Format(strSql2, NoteNo, SequenceNO);
            }
            else
            {

                strSql1 = this.GetFeeItemsSelectSql();
                if (this.Sql.GetSql("Fee.GetFeeItemListByNoteNoAndNoteSequence.1", ref strSql2) == -1) return null;
                strSql2 = string.Format(strSql2, NoteNo, SequenceNO);
            }
            strSql1 = strSql1 + strSql2;
            if (IsPharmacy)
            {

                alItem = this.QueryMedItemListsBySql(strSql1);

            }
            else
            {
                alItem = this.QueryFeeItemListsBySql(strSql1);
            }
            if (alItem == null) return null;
            if (alItem.Count == 0)
            {
                this.Err = "û���ҵ�����Ŀ";
                return null;
            }
            ItemList = (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList)alItem[0];
            return ItemList;
        }

        /// <summary>
        /// ��û��ߺ�ִ�п��ҵ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListsByInpatientNOAndDept()", true)]
        public ArrayList GetMedItemListByInpatientAndDept(string InpatientNo, string DeptCode)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetMedItemListByInpatientAndDept.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, DeptCode);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// ��û��ߺ�ִ�п��ҵķ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="DeptCode"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsByInpatientNOAndDept()", true)]
        public ArrayList GetFeeItemListByInpatientAndDept(string InpatientNo, string DeptCode)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetFeeItemListByInpatientAndDept.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, DeptCode);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }

        /// <summary>
        /// ��û���ҩƷ�շ���Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="Dept"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListsByInpatientNO()", true)]
        public ArrayList GetPatientDrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, string Dept)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetPatientDrug.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, Dept);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// ����û��߷�ҩƷ�շ���Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="Dept"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsByInpatientNO()", true)]
        public ArrayList GetPatientUndrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, string Dept)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetPatientUndrug.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, Dept);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ Edit by liangjz
        /// </summary>
        /// <param name="InvoiceNo">��Ʊ��</param>
        /// <param name="dtBegin">��ʼʱ��</param>
        /// <param name="dtEnd">��ֹʱ��</param>
        /// <param name="SendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalance">�Ƿ����</param>
        /// <param name="minFeeID">��С���ô���</param>
        /// <returns>���ض�̬���飬ʧ�ܷ���null</returns>
        [Obsolete("����,��QueryMedItemListsCanQuitByInvoiceNO()", true)]
        public ArrayList GetForQuitDrugByInvoice(string InvoiceNo, DateTime dtBegin, DateTime dtEnd, string SendDrugState, bool isBalance, string minFeeID)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitDrug.4", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InvoiceNo, dtBegin, dtEnd, SendDrugState, balanceState, minFeeID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ Edit by liangjz
        /// </summary>
        /// <param name="InvoiceNo">��Ʊ��</param>
        /// <param name="dtBegin">��ʼʱ��</param>
        /// <param name="dtEnd">��ֹʱ��</param>
        /// <param name="SendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalance">�Ƿ����</param>
        /// <returns>���ض�̬���飬ʧ�ܷ���null</returns>
        [Obsolete("����,��QueryMedItemListsCanQuitByInvoiceNO()", true)]
        public ArrayList GetForQuitDrugByInvoice(string InvoiceNo, DateTime dtBegin, DateTime dtEnd, string SendDrugState, bool isBalance)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitDrug.2", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InvoiceNo, dtBegin, dtEnd, SendDrugState, balanceState);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ Edit by liangjz
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="dtBegin">��ʼʱ��</param>
        /// <param name="dtEnd">��ֹʱ��</param>
        /// <param name="SendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalance">�Ƿ����</param>
        /// <param name="minFeeID">��С���ô���</param>
        /// <returns>���ض�̬���飬ʧ�ܷ���null</returns>
        [Obsolete("����,��QueryMedItemListsCanQuit()", true)]
        public ArrayList GetForQuitDrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, string SendDrugState, bool isBalance, string minFeeID)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitDrug.3", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, SendDrugState, balanceState, minFeeID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// ���ݷ���״̬��ȡһ��ʱ�䷶Χ��δ����Ĺ��ɹ��˷ѵ�ҩƷ��Ŀ Edit by liangjz
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="SendDrugState"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListsCanQuit()", true)]
        public ArrayList GetForQuitDrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, string SendDrugState)
        {
            return this.QueryMedItemListsCanQuit(InpatientNo, dtBegin, dtEnd, SendDrugState, false);
        }

        /// <summary>
        /// �����Ƿ���㡢�Ƿ񷢷���ȡһ��ʱ�䷶Χ�ڿɹ��˷ѵ�ҩƷ��Ŀ Edit by liangjz
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="dtBegin">��ʼʱ��</param>
        /// <param name="dtEnd">��ֹʱ��</param>
        /// <param name="SendDrugState">�Ƿ�ҩ</param>
        /// <param name="isBalance">�Ƿ����</param>
        /// <returns>���ض�̬���飬ʧ�ܷ���null</returns>
        [Obsolete("����,��QueryMedItemListsCanQuit()", true)]
        public ArrayList GetForQuitDrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, string SendDrugState, bool isBalance)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitDrug.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, SendDrugState, balanceState);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// �����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="isBalance"></param>
        /// <param name="minFeeID">��С���ô���</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsCanQuit()", true)]
        public ArrayList GetForQuitUndrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, bool isBalance, string minFeeID)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitUndrug.2", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, balanceState, minFeeID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }
        /// <summary>
        /// ��ȡһ��ʱ�䷶Χ�ڹ��ɹ��˷ѵķ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsCanQuit()", true)]
        public ArrayList GetForQuitUndrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd)
        {
            return this.QueryFeeItemListsCanQuit(InpatientNo, dtBegin, dtEnd, false);
        }

        /// <summary>
        /// �����Ƿ������ȡ���߿��˷ѷ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="isBalance"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsCanQuit()", true)]
        public ArrayList GetForQuitUndrug(string InpatientNo, DateTime dtBegin, DateTime dtEnd, bool isBalance)
        {
            string strSql1 = "";
            string strSql2 = "";
            string balanceState;
            if (isBalance)	//�ѽ���
                balanceState = "1";
            else	//δ����
                balanceState = "0";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetForQuitUndrug.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo, dtBegin, dtEnd, balanceState);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }

        /// <summary>
        /// ���ҩƷ������Ϣ
        /// </summary>
        /// <returns></returns>
        [Obsolete("����,��QueryMedItemListCharged()", true)]
        public ArrayList GetDrugChargeItems(string InpatientNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetDrugChargeItems.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryMedItemListsBySql(strSql1);
        }

        /// <summary>
        /// ��û��ߵ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNo">סԺ��ˮ��</param>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        /// <param name="flag">"All"����, "Yes"���ϴ� "No"δ�ϴ�</param>
        /// <returns>null����, ArrayList.Count = 0û�з���Ҫ���¼, >=1 ��Ч��¼</returns>
        [Obsolete("����,��QueryMedItemLists()", true)]
        public ArrayList GetMedItemsForInpatient(string inpatientNo, DateTime beginDate, DateTime endDate, string flag)
        {
            string strSql = "";
            string strSqlWhere = "";
            string sUpload = "";
            strSql = GetMedItemListSelectSql();
            if (this.Sql.GetSql("Fee.GetMedItemsForInpatient.Where.Upload", ref strSqlWhere) == -1)
                return null;
            if (flag.ToUpper() == "ALL")//����
            {
                sUpload = "%";
            }
            else if (flag.ToUpper() == "Yes")
            {
                sUpload = "1";
            }
            else
            {
                sUpload = "0";
            }
            try
            {
                strSqlWhere = string.Format(strSqlWhere, inpatientNo, beginDate, endDate, sUpload);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            strSql = strSql + strSqlWhere;
            return QueryMedItemListsBySql(strSql);
        }

        /// <summary>
        /// ��÷�ҩƷ������Ϣ
        /// </summary>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemListsForCharged()", true)]
        public ArrayList GetUndrugChargeItems(string InpatientNo)
        {
            string strSql1 = "";
            string strSql2 = "";
            strSql1 = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetUndrugChargeItems.1", ref strSql2) == -1) return null;
            strSql1 = strSql1 + strSql2;
            try
            {
                strSql1 = string.Format(strSql1, InpatientNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.QueryFeeItemListsBySql(strSql1);
        }

        /// <summary>
        /// ��û��߷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeItemLists()", true)]
        public ArrayList GetFeeItemsForInpatient(string inpatientNo, DateTime beginDate, DateTime endDate)
        {
            string strSql = "";
            string strSqlWhere = "";
            strSql = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetMedItemsForInpatient.Where.1", ref strSqlWhere) == -1)
                return null;
            try
            {
                strSqlWhere = string.Format(strSqlWhere, inpatientNo, beginDate, endDate);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            strSql = strSql + strSqlWhere;
            return this.QueryFeeItemListsBySql(strSql);
        }
        /// <summary>
        /// ��û��ߵķ�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNo">סԺ��ˮ��</param>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        /// <param name="flag">"All"����, "Yes"���ϴ� "No"δ�ϴ�</param>
        /// <returns>null����, ArrayList.Count = 0û�з���Ҫ���¼, >=1 ��Ч��¼</returns>
        [Obsolete("����,��QueryFeeItemLists()", true)]
        public ArrayList GetFeeItemsForInpatient(string inpatientNo, DateTime beginDate, DateTime endDate, string flag)
        {
            string strSql = "";
            string strSqlWhere = "";
            string sUpload = "";
            strSql = this.GetFeeItemsSelectSql();
            if (this.Sql.GetSql("Fee.GetMedItemsForInpatient.Where.Upload", ref strSqlWhere) == -1)
                return null;
            if (flag.ToUpper() == "ALL")//����
            {
                sUpload = "%";
            }
            else if (flag.ToUpper() == "Yes")
            {
                sUpload = "1";
            }
            else
            {
                sUpload = "0";
            }
            try
            {
                strSqlWhere = string.Format(strSqlWhere, inpatientNo, beginDate, endDate, sUpload);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            strSql = strSql + strSqlWhere;
            return this.QueryFeeItemListsBySql(strSql);
        }

        /// <summary>
        /// ����סԺ�ţ���ʼ����ʱ������Ѵ�ӡ�����յ�--���ڲ���
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="begin">��ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosForPrinted()", true)]
        public ArrayList GetFeeInfosForPrinted(string InpatientNo, string begin, string end)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetFeeInfosForPrinted", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, begin, end);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }


            this.ExecQuery(strSql);
            //			Neusoft.HISFC.Models.Base.FT Fee;
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    if (!this.Reader.IsDBNull(1)) FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    if (!this.Reader.IsDBNull(2)) FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    if (!this.Reader.IsDBNull(3)) FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    if (!this.Reader.IsDBNull(4)) FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    if (!this.Reader.IsDBNull(5)) FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ָ��ʱ�䵽����δ��ӡ�����յ�
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="begin">��ʼʱ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosForPrintBill()", true)]
        public ArrayList GetFeeInfosForPrintBill(string InpatientNo, string begin)
        {
            return null;
        }
        /// <summary>
        /// ����סԺ�ţ���ʼ����ʱ�����δ��ӡ�����յ�
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="begin">��ʼʱ��</param>
        /// <param name="end">����ʱ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosForPrintBill()", true)]
        public ArrayList GetFeeInfosForPrintBill(string InpatientNo, string begin, string end)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetFeeInfosForPrintBill", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, begin, end);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }


            this.ExecQuery(strSql);
            //			Neusoft.HISFC.Models.Base.FT Fee;
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    if (!this.Reader.IsDBNull(1)) FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    if (!this.Reader.IsDBNull(2)) FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    if (!this.Reader.IsDBNull(3)) FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    if (!this.Reader.IsDBNull(4)) FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    if (!this.Reader.IsDBNull(5)) FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ��ȡ���޶����������ϸ
        /// ������tot_cost=0,own_cost!=0
        /// </summary>
        /// <param name="InPatientNo">סԺ��ˮ��</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeForAdjustByInpatientNO", true)]
        public ArrayList GetMinFeeInfoForAdjust(string InPatientNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetMinFeeInfoForAdjust", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InPatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            this.ExecQuery(strSql);
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InPatientNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeForAdjustOverTopByInpatientNO", true)]
        public ArrayList GetMinFeeForAdjustOverTop(string InPatientNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetMinFeeForAdjustOverTop", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InPatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            this.ExecQuery(strSql);
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ����סԺ�ţ�����״̬���������Ͳ�ѯ������Ϣ
        /// </summary>
        /// <param name="InPatientNo">סԺ��ˮ��</param>
        /// <param name="BalanceState">����״̬</param>
        /// <param name="TransType">�������ͣ�1������2������3ҩ�ѵ���</param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeByInpatientNO()", true)]
        public ArrayList GetMinFeeInfoByPatientID(string InPatientNo, string BalanceState, string TransType)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetMinFeeInfoByPatientID", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InPatientNo, BalanceState, TransType);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            this.ExecQuery(strSql);
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ��÷�����ϢFeeInfoUnionת����ã���С���÷��飩-סԺ��ˮ��,����״̬
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosAndChangeCostGroupByMinFeeByInpatientNO()", true)]
        public ArrayList GetFeeInfosAndChangeCostGroupByMinFee(string InpatientNo, string BalanceState)
        {
            string strSql = "";
            if (this.Sql.GetSql("GetFeeInfosAndChangeCostGroupByMinFee.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, BalanceState);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }

            this.ExecQuery(strSql);
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;

        }

        /// <summary>
        /// ����סԺ��ˮ�źͽ�����ż���feeinfo
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="BalanceNo"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeByInpatientNO()", true)]
        public ArrayList GetFeeInfoGroupbyMinFee(string InpatientNo, int BalanceNo)
        {
            string strSql = "";

            //0סԺ��ˮ��1�������
            if (this.Sql.GetSql("GetFeeInfosGroupbyMinFee.2", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, BalanceNo.ToString());
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }


            this.ExecQuery(strSql);
            //			Neusoft.HISFC.Models.Base.FT Fee;
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    if (!this.Reader.IsDBNull(1)) FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    if (!this.Reader.IsDBNull(2)) FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    if (!this.Reader.IsDBNull(3)) FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    if (!this.Reader.IsDBNull(4)) FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    if (!this.Reader.IsDBNull(5)) FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// GetFeeInfosGroupbyMinFee.1
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dt1"></param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeByInpatientNO()", true)]
        public ArrayList GetFeeInfosGroupbyMinFee(string InpatientNo, DateTime dt1, string BalanceState)
        {
            DateTime dt2 = this.GetDateTimeFromSysDateTime();
            return this.QueryFeeInfosGroupByMinFeeByInpatientNO(InpatientNo, dt1, dt2, BalanceState);
        }

        /// <summary>
        /// ��÷�����ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// GetFeeInfosGroupbyMinFee.1
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeByInpatientNO()", true)]
        public ArrayList GetFeeInfosGroupbyMinFee(string InpatientNo, DateTime dt1, DateTime dt2, string BalanceState)
        {
            string strSql = "";
            if (this.Sql.GetSql("GetFeeInfosGroupbyMinFee.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, dt1, dt2, BalanceState);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }


            this.ExecQuery(strSql);
            //			Neusoft.HISFC.Models.Base.FT Fee;
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    if (!this.Reader.IsDBNull(1)) FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    if (!this.Reader.IsDBNull(2)) FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    if (!this.Reader.IsDBNull(3)) FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    if (!this.Reader.IsDBNull(4)) FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    if (!this.Reader.IsDBNull(5)) FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;

        }

        /// <summary>
        ///  ���Ӥ��������ϢFeeInfo����С���÷��飩-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��,����״̬
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="BalanceState"></param>
        /// <returns></returns>
        [Obsolete("����,��QueryFeeInfosGroupByMinFeeForBaby()", true)]
        public ArrayList GetFeeInfosGroupByMinFeeForBaby(string InpatientNo, DateTime dt1, DateTime dt2, string BalanceState)
        {
            string strSql = "";
            if (this.Sql.GetSql("GetFeeInfosGroupByMinFeeForBaby.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, InpatientNo, dt1, dt2, BalanceState, "1");
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }


            this.ExecQuery(strSql);
            //			Neusoft.HISFC.Models.Base.FT Fee;
            Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo;
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                FeeInfo = new FeeInfo();
                try
                {
                    FeeInfo.Item.MinFee.ID = this.Reader[0].ToString();
                    if (!this.Reader.IsDBNull(1)) FeeInfo.FT.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                    if (!this.Reader.IsDBNull(2)) FeeInfo.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
                    if (!this.Reader.IsDBNull(3)) FeeInfo.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3].ToString());
                    if (!this.Reader.IsDBNull(4)) FeeInfo.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[4].ToString());
                    if (!this.Reader.IsDBNull(5)) FeeInfo.FT.RebateCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[5].ToString());
                }
                catch (Exception ex)
                {
                    this.Err = "��÷�����Ϣ����С���÷��飩��ֵ����" + ex.Message;
                    this.ErrCode = ex.Message;
                    this.WriteErr();
                    return null;
                }
                al.Add(FeeInfo);
            }
            this.Reader.Close();
            return al;

        }

        /// <summary>
        /// ͨ����С���ô�������С��������
        /// </summary>
        /// <param name="FeeCode"></param>
        /// <returns></returns>
        [Obsolete("����,��GetMinFeeNameByCode()", true)]
        public string GetFeeNameByFeeCode(string FeeCode)
        {
            string strSql = "";
            if (this.Sql.GetSql("GetFeeNameByFeeCode.1", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, FeeCode);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.ErrCode = ex.Message;
                return null;
            }
            return this.ExecSqlReturnOne(strSql);
        }

        /// <summary>
        /// ������з�����Ϣ
        /// </summary>
        /// <returns></returns>
        [Obsolete("����()", true)]
        public ArrayList GetFeeInfos()
        {
            return null;
        }
        /// <summary>
        /// ��÷�����Ϣ-���ݻ��ߵ�סԺ��ˮ��
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="strDataSet"></param>
        /// <returns></returns>
        [Obsolete("����()", true)]
        public ArrayList GetFeeInfos(string InpatientNo, ref string strDataSet)
        {
            return null;
        }
        /// <summary>
        /// ��÷�����Ϣ-סԺ��ˮ�ţ����÷�����ʼʱ�䣬����ʱ��
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="strDataSet"></param>
        /// <returns></returns>
        [Obsolete("����()", true)]
        public ArrayList GetFeeInfos(string InpatientNo, DateTime dt1, DateTime dt2, ref string strDataSet)
        {
            return null;
        }

        /// <summary>
        /// ����µķ�Ʊ�� Written By ���峬 ���ô洢����
        /// ���ݲ���Ա��ID�Ͳ�������� ����Ʊ��Ԥ����Ŀ ����ʣ�෢Ʊ��Ŀ ���ֲ��÷�Ʊ��Ŀ-Ԥ����Ŀ�ж���ʾ��ȡ��Ʊ
        /// ����Ʊ��Ԥ����Ŀ�Ķ�����ܱȽϲ��̶�����ҵ��㲻���ж� ���ص�ʣ����ĿĬ��10000 ֻ��С��Ԥ����Ŀ�Ż᷵��
        /// ��ʵ��Ŀ,�����������ֲ�ֻ������ж�,����Ƚϼ�Ҳ�Ƚ�ͨ��.
        /// </summary>
        /// <param name="OperatorID">����Ա����</param>
        /// <param name="InvoiceType">��Ʊ����</param>
        /// <param name="InAlarm">Ʊ��Ԥ����Ŀ</param>
        /// <param name="InLeftNum">ʣ�෢Ʊ��Ŀ</param>
        /// <param name="FinGrpId">���������</param>
        /// <returns>��Ʊ�� </returns>
        [Obsolete("����, ʹ��GetNewInvoiceNO()", true)]
        public string GetNewInvoiceNo(string OperatorID, Neusoft.HISFC.Models.Fee.EnumInvoiceType InvoiceType, int InAlarm, ref int InLeftNum,
            string FinGrpId)
        {
            string strSql = "";
            string strReturn = "";
            string[] s;
            string InvoiceNo = "";
            int InCode = 0;
            string strText = "";
            InLeftNum = 10000;
            #region �ӿ�˵��
            //��÷�Ʊ��
            //����:0 opratorID
            //����:0 ��һ���·�Ʊ��
            #endregion
            if (this.Sql.GetSql("Fee.Inpatient.Invoice.Get.1", ref strSql) == -1)
            {
                this.Err = "��÷�Ʊ�ŵ�sql���û�ҵ�����Fee.Inpatient.Invoice.Get.1û�ҵ�!";
                this.ErrCode = "Fee.Inpatient.Invoice.Get.1û�ҵ�!";
                this.WriteErr();
                return null;
            }
            try
            {
                strSql = string.Format(strSql, ((int)InvoiceType).ToString(), OperatorID, InAlarm, InvoiceNo, strText, InCode, FinGrpId);
            }
            catch
            {
                this.Err = "��������";
                return null;
            }
            if (this.ExecEvent(strSql, ref strReturn) == -1)
            {
                this.Err = "ִ�д洢���̳���!PRC_GET_INVOICE";
                this.ErrCode = "PRC_GET_INVOICE";
                this.WriteErr();
                return null;

            };

            s = strReturn.Split(',');
            InvoiceNo = s[0];
            strText = s[1];
            try
            {
                InCode = NConvert.ToInt32(s[2]);
                if (InCode == 100)//û���ҵ���Ʊ��
                {
                    this.Err = strText;
                    this.ErrCode = "100";
                    return null;
                }
                if (InCode == 101)//�ҵ���Ʊ�ŵ���ʣ�෢Ʊ�ŵ���Ԥ����Ŀ
                {
                    InLeftNum = NConvert.ToInt32(strText);
                }
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
            }

            return InvoiceNo;
        }

        /// <summary>
        /// �����ҩƷ������ Written By ���峬
        /// </summary>
        /// <returns>ҩƷ������</returns>
        [Obsolete("����, ʹ��GetDrugRecipeNO()", true)]
        public string GetDrugNewNoteNo()
        {
            string strSql = "";
            string strNoteNo = "";
            if (this.Sql.GetSql("GetDrugNewNoteNo.1", ref strSql) == -1) return "";
            if (this.ExecQuery(strSql) == -1) return "";
            try
            {
                this.Reader.Read();
                strNoteNo = (this.Reader[0].ToString());
                strNoteNo = strNoteNo.PadLeft(13, '0');
                strNoteNo = "y" + strNoteNo;
                this.Reader.Close();
                return strNoteNo;
            }
            catch (Exception ex)
            {
                if (!Reader.IsClosed)
                {
                    Reader.Close();
                }
                this.ErrCode = ex.Message;
                this.Err = "����´����ų���" + this.GetType().ToString() + ex.Message;
                this.WriteErr();
                return "";
            }

        }

        /// <summary>
        /// ����·�ҩƷ������ Written By ���峬
        /// </summary>GetUndrugNewNoteNo.1
        /// <returns>��ҩƷ������</returns>
        [Obsolete("����, ʹ��GetUndrugRecipeNO()", true)]
        public string GetUndrugNewNoteNo()
        {
            string strSql = "";
            string strNoteNo = "";
            if (this.Sql.GetSql("GetUndrugNewNoteNo.1", ref strSql) == -1) return null;
            if (this.ExecQuery(strSql) == -1) return null;
            try
            {
                this.Reader.Read();
                strNoteNo = (this.Reader[0].ToString());
                strNoteNo = strNoteNo.PadLeft(13, '0');
                strNoteNo = "f" + strNoteNo;
                this.Reader.Close();
                return strNoteNo;
            }
            catch (Exception ex)
            {
                if (!Reader.IsClosed)
                {
                    Reader.Close();
                }
                this.ErrCode = ex.Message;
                this.Err = "����´����ų���" + this.GetType().ToString() + ex.Message;
                this.WriteErr();
                return "";
            }

        }


        /// <summary>
        /// �Ƿ�Ӥ�����ܴ���
        /// </summary>
        /// <param name="FeeInfo"></param>
        /// <returns></returns>
        [Obsolete("����", true)]
        public bool ifBabyShared(Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo)
        {
            #region �ӿ�˵��
            //��ú�ͬ��λ�Ƿ�����Ӥ������
            //����:0 ��ͬ����
            //������0 �� 1��
            #endregion
            string strSql = "";
            int i = 0;
            if (this.Sql.GetSql("Fee.Inpatient.FeeRate.2", ref strSql) == -1) return false;
            strSql = string.Format(strSql, FeeInfo.Patient.Pact.ID);
            if (this.ExecQuery(strSql) == -1) return false;
            try
            {
                this.Reader.Read();
                i = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[0].ToString());

            }
            catch { }
            finally
            {
                this.Reader.Close();
            }
            return System.Convert.ToBoolean(i);
        }

        /// <summary>
        /// �����Ŀ���ñ���(����Ӥ���Ĵ����ж�)
        /// </summary>
        /// <param name="FeeInfo">���߻�����Ϣ</param>
        /// <param name="Item">��Ŀ��Ϣ</param>
        /// <returns>���ñ�������</returns>
        [Obsolete("����", true)]
        public Neusoft.HISFC.Models.Base.FTRate GetFeeRate(Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo FeeInfo, Neusoft.HISFC.Models.Base.Item Item)
        {
            Neusoft.HISFC.Models.Base.FTRate Rate = new Neusoft.HISFC.Models.Base.FTRate();
            string strSql = "";

            Rate.OwnRate = 1;
            Rate.PayRate = 0;
            Rate.PubRate = 0;
            if (FeeInfo.IsBaby)//Ӥ��
            {
                if (this.ifBabyShared(FeeInfo) == false)
                {
                    return Rate;
                }
            }
            #region �ӿ�˵��
            //�����Ŀ�ı���
            //����:0 ��ͬ���룬1��С���ñ��룬2 0 ��ҩƷ 1 ҩƷ,3 ��Ŀ����
            //������0 OwnRate,1 PayRate,2 PubRate
            #endregion
            if (this.Sql.GetSql("Fee.Inpatient.FeeRate.1", ref strSql) == -1) return null;
            strSql = string.Format(strSql, FeeInfo.Patient.Pact.ID, FeeInfo.Item.MinFee.ID, Neusoft.FrameWork.Function.NConvert.ToInt32(Item.IsPharmacy.ToString()), Item.ID);
            if (this.ExecQuery(strSql) == -1) return Rate;
            try
            {
                this.Reader.Read();
                Rate.OwnRate = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[0].ToString());
                Rate.PayRate = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1].ToString());
                Rate.PubRate = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2].ToString());
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = "��ȡ��������" + this.GetType().ToString() + ex.Message;
                this.WriteErr();
            }
            finally
            {
                this.Reader.Close();
            }
            return Rate;
        }

        /// <summary>
        /// ��ѯ������ЧԤ�����¼--For����
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <returns></returns>
        [Obsolete("����,ʹ��QueryPrepaysBalanced()", true)]
        public ArrayList QueryPrepayForBalance(string InpatientNo)
        {
            string strSql1 = this.GetSqlForSelectAllPrepay();
            string strSql2 = "";

            if (this.Sql.GetSql("Fee.QueryPrepayForBalance.1", ref strSql2) == -1) return null;
            try
            {
                strSql2 = string.Format(strSql2, InpatientNo);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return null;
            }
            strSql1 += strSql2;
            return this.QueryPrepaysBySql(strSql1);

        }

        /// <summary>
        /// ���»�������Ԥ������Ϣ
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="Prepay"></param>
        /// <returns></returns>
        [Obsolete("����,ʹ��UpdatePrepay()", true)]
        public int UpdateAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.Prepay Prepay)
        {
            //0סԺ��ˮ��1Ԥ�����
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.UpdateAccount11", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, PatientInfo.ID, Prepay.FT.PrepayCost.ToString());

            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }


        /// <summary>
        ///  ��ӻ��� Ԥ�������  Written By ���峬
        /// </summary>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="Prepay">Ԥ�������</param>
        /// <returns><br>0 �ɹ�</br><br>-1 ʧ��</br></returns>
        [Obsolete("����,ʹ��InsertPrepay()", true)]
        public int AddPatientAccount(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Models.Fee.Inpatient.Prepay Prepay)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.Prepay.1", ref strSql) == -1)
            {
                return -1;
            }
            #region �ӿ�˵��
            //��ӻ���Ԥ������Ϣ ����Ԥ������סԺ����Fee.Inpatient.Prepay.1
            //�������
            //0סԺ��ˮ��  1�������  2����  3Ԥ����� 4������ʽ 5���Ҵ���  6Ԥ�����վݺ��� 7����ʱ�� 8�����־ 0:δ���㣻1:�ѽ���
            //9Ԥ����״̬0:��ȡ��1:����;2:���� 10�������� 11�����ʻ� 12���㷢Ʊ�� 13������� 14�����˴��� 15�Ͻɱ�־��1�� 0�� 
            //16��������� 17������λ 18 0��תѺ��1תѺ��2תѺ���Ѵ�ӡ 19תѺ�����Ա 20תѺ��ʱ�� 21 pos������ˮ�Ż�֧Ʊ�Ż��Ʊ��
            //22 ����Ա 23�������� 24 ����������25תѺ��ʱ������� 26ԭ��Ʊ���� 27����Ա����
            #endregion
            if (Prepay.PrepayOper.OperTime == DateTime.MinValue)
            {
                this.ErrCode = "Ԥ�����շ�ʱ�����û�и�ֵ!";
                this.Err = "Ԥ�����շ�ʱ�����û�и�ֵ!";
                return -1;
            }
            if ((Prepay.PrepayOper.ID == null) || (Prepay.PrepayOper.ID == ""))
            {
                this.ErrCode = "Ԥ�������Աû�и�ֵ!";
                this.Err = "Ԥ�������Աû�и�ֵ!";
                return -1;
            }
            string OperDeptCode = ""; //����Ա����
            if (Prepay.PrepayOper.Dept.ID == null || Prepay.PrepayOper.Dept.ID == "")
            {
                OperDeptCode = this.GetDeptByEmplId(Prepay.PrepayOper.ID);
            }
            else
            {
                OperDeptCode = Prepay.PrepayOper.Dept.ID;
            }

            try
            {

                string[] s ={
							   //סԺ��ˮ��
							   PatientInfo.ID,
							   //�������
							   Prepay.ID,
							   //Ԥ�����
							   PatientInfo.Name,Prepay.FT.PrepayCost.ToString(),
							   //������ʽ
							   Prepay.PayType.ID.ToString(),
							   // ���Ҵ���
							   PatientInfo.PVisit.PatientLocation.Dept.ID,
							   //Ԥ�����վݺ���
							   Prepay.Invoice.ID,
							   //����ʱ��
							   Prepay.BalanceTime.ToString(),
							   //�����־ 0:δ���㣻1:�ѽ���
							   Prepay.BalanceState,
							   //Ԥ����״̬0:��ȡ��1:����;2:����
							   Prepay.PrepayState,
							   //��������
							   Prepay.Bank.Name,
							   //�����ʻ�
							   Prepay.Bank.Account,
							   //���㷢Ʊ��
							   Prepay.Invoice.ID,
							   //�������
							   Prepay.BalanceNO.ToString(),
							   //�����˴���
							   Prepay.BalanceOper.ID,
							   //�Ͻɱ�־��1�� 0��
							   System.Convert.ToInt16(Prepay.IsTurnIn).ToString(),
							   //���������
							   Prepay.FinGroup.ID,
							   //������λ
							   Prepay.Bank.WorkName,
							   //0��תѺ��1תѺ��2תѺ���Ѵ�ӡ
							   Prepay.TransferPrepayState,
							   //תѺ�����Ա
							   Prepay.TransPrepayOper.ID,
							   //תѺ��ʱ��
							   Prepay.TransferPrepayTime.ToString(),
							   //������ˮ�Ż�֧Ʊ�Ż��Ʊ��
							   Prepay.Bank.InvoiceNO,
							   //����Ա
							   Prepay.PrepayOper.ID,
							   //��������
							   Prepay.PrepayOper.OperTime.ToString(),
							   //����������
							   Prepay.AuditingNO,
							   //תѺ��ʱ�������
							   Prepay.TransferPrepayBalanceNO.ToString(),//
							   //ԭ��Ʊ����
							   Prepay.OrgInvoice.ID,
							   //����Ա����
							   OperDeptCode
						   };

                strSql = string.Format(strSql, s);
            }
            catch (Exception ex)
            {
                this.ErrCode = ex.Message;
                this.Err = "��Ϣδ����������" + ex.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }


        #endregion




        #region "��ȡ��Ա����"
        /// <summary>
        /// ͨ����Աid��ȡ���Ҵ���
        /// </summary>
        /// <param name="PersonId">��Աid</param>
        /// <returns></returns>
        public string GetDeptByEmplId(string PersonId)
        {
            Neusoft.HISFC.BizLogic.Manager.Person Person = new Neusoft.HISFC.BizLogic.Manager.Person();
            //Person.SetTrans(this.commond.Transaction);
            Neusoft.HISFC.Models.Base.Employee p = new Employee();
            //p=Person.GetPersonByID(PersonId);
            if (p == null)
            {
                this.Err = "������Ա���ҳ���!" + Person.Err;
                return null;
            }

            return p.Dept.ID;
        }
        #endregion
        #region ҽ�����ò�ѯ
        /// <summary>
        /// ��ȡҽ�������ϴ��ܷ���
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <param name="TotCost">�ϴ��ܷ���</param>
        /// <returns></returns>
        public int GetUploadTotCost(string InpatientNo, ref decimal TotCost)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.InPatient.GetUploadTotCost", ref strSql) == -1)
            {
                this.Err = "û���ҵ�SQL���Fee.InPatient.GetUploadTotCost";
                return -1;
            }
            try
            {
                strSql = string.Format(strSql, InpatientNo);

                this.ExecQuery(strSql);
                while (Reader.Read())
                {
                    TotCost = Convert.ToDecimal(Reader[0]);
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }

            return 0;
        }
        #endregion

        #region "���ҵ��"


        /// <summary>
        /// ��������Ԥ�����¼
        /// </summary>
        /// <param name="Prepay">Ԥ����ʵ��(ԭ�͵�Ԥ������Ϣ)</param>
        /// <param name="PatientInfo">������Ϣ</param>
        /// <param name="NewReciptNo">�²����ķ�Ʊ��</param>
        /// <param name="ReturnInvoiceNo">����¼��Ʊ��</param>
        /// <param name="ReturnPrepay">����¼ʵ�巵���Թ���ӡ��췢Ʊʹ��</param>
        /// <returns>0�ɹ�-1ʧ��</returns>

        public int PrepaySignOperation(Neusoft.HISFC.Models.Fee.Inpatient.Prepay Prepay, Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, string NewReciptNo, string ReturnInvoiceNo, ref Neusoft.HISFC.Models.Fee.Inpatient.Prepay ReturnPrepay)
        {
            //�γɸ���¼ʵ��

            Prepay.OrgInvoice.ID = Prepay.Invoice.ID;
            //����¼�Ƿ�ʹ���µķ�Ʊ��
            if (ReturnInvoiceNo.Length > 1) Prepay.RecipeNO = ReturnInvoiceNo;
            Prepay.FT.PrepayCost = -Prepay.FT.PrepayCost;
            Prepay.PrepayOper.ID = this.Operator.ID;
            Prepay.PrepayOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.GetSysDateTime());
            Prepay.PrepayState = "2";
            ReturnPrepay = Prepay.Clone();
            //���븺��¼
            if (this.InsertPrepay(PatientInfo, Prepay) < 1)
            {
                this.Err = "����Ԥ�������!";
                return -1;
            }
            //����ԭ�м�¼
            if (UpdatePrepayHaveReturned(PatientInfo, Prepay) < 1)
            {
                this.Err = "������¼�Ѿ�������߽��й���������,����״̬����!" + this.Err;
                return -1;
            }
            //�γ�����¼ʵ��
            Prepay.FT.PrepayCost = -Prepay.FT.PrepayCost;
            Prepay.PrepayState = "0";
            Prepay.RecipeNO = NewReciptNo;
            //��������¼
            if (this.InsertPrepay(PatientInfo, Prepay) < 1)
            {
                this.Err = "����Ԥ�������!";
                return -1;
            }

            return 0;
        }
        /// <summary>
        /// ���ۺ��շ�
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="ItemList"></param>
        /// <returns></returns>
        public int FeeAfterCharge(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList)
        {
            if (this.UpdateChargeItemToFee(ItemList) == -1)
            {
                this.Err = "���»��ۼ�¼ʧ��!";
                return -1;
            }
            //ItemList.Item.MinFee.ID=ItemList.Item.MinFee.ID;
            //			if (this.UpdateAccount(PatientInfo,ItemList)==-1)
            //			{
            //				this.Err="������û��ܱ�ʧ��!"+this.DBErrCode.ToString();
            //				return -1;
            //			}
            int parm = this.UpdateInMainInfoFee(PatientInfo.ID, ItemList.FT);
            if (parm == -1)
            {
                this.Err = "����סԺ����ʧ��!";
                return -1;
            }
            if (parm == 0)
            {
                this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// �շ�
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="ItemList"></param>
        /// <returns></returns>
        public int FeeManager(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList)
        {
            return this.FeeManager(PatientInfo, ItemList, "0");
        }
        /// <summary>
        /// �˷�
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="ItemList"></param>
        /// <returns></returns>
        public int FeeManagerReturn(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList)
        {
            return this.FeeManager(PatientInfo, ItemList, "1");
        }
        /// <summary>
        /// ֻΪֱ���˷ѷ���
        /// 0�շ� 1�˷�   �˷�ʱ������ʵ��ΪҪ�˷ѵ�����¼(����¼�����ڴ���)  �շѵ�ʱ��ΪҪ�շѵĸ�ֵ��������¼ʵ��
        /// isUpdate == true ���±��˷ѵĿ������� == false ������
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="ItemList"></param>
        /// <param name="Flag"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public int DirQuitFee(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList, string Flag, bool isUpdate)
        {
            //�жϷ����ܶ�
            if (ItemList.FT.TotCost == 0)
            {
                this.Err = "�����ܶ�Ϊ0,��ȡ������,������ȷ��!";
                return -1;
            }
            //�жϻ�ʿվ����
            //ֱ���˷ѻ��߿���û�л�ʿվ
            //			if(PatientInfo.PVisit.PatientLocation.NurseCell.ID==null||PatientInfo.PVisit.PatientLocation.NurseCell.ID.Trim()=="")
            //			{
            //				this.Err="���ֲ㻼�߻�ʿվ����û�и�ֵ!";
            //				return -1;
            //			}
            //�ж�ִ�п��Ҵ���
            if (ItemList.ExecOper.Dept.ID == null || ItemList.ExecOper.Dept.ID == "")
            {
                this.Err = "���ֲ�ִ�п���û�и�ֵ!";
                return -1;
            }
            //�ж��շѱ���
            if (ItemList.FTRate.ItemRate < 0)
            {
                this.Err = "�շѱ�����ֵ����!";
                return -1;
            }
            if (ItemList.FTRate.ItemRate == 0) ItemList.FTRate.ItemRate = 1;

            //���㻼�߷��ñ���
            //			if(this.ComputePatientFee(PatientInfo,ref ItemList)==-1)
            //			{
            //				this.Err="������ñ�������"+this.Err;
            //				return -1;
            //			}
            //			
            if (Flag == "1")
            {
                //���¿������� Ȼ��ȡ���µĴ�����
                //if (ItemList.Item.IsPharmacy)
                if(ItemList.Item.ItemType == EnumItemType.Drug)
                {
                    if (isUpdate)
                    {
                        if (this.UpdateNoBackQtyForDrug(ItemList.RecipeNO, ItemList.SequenceNO, ItemList.Item.Qty, ItemList.BalanceState) == -1)
                        {
                            this.Err = "����ԭ�м�¼������������!����ͬʱ�������˽���ͬһ�����˷�!";
                            return -1;
                        }
                    }
                    //����µĴ�����
                    ItemList.RecipeNO = GetDrugRecipeNO();
                }
                else
                {
                    if (isUpdate)
                    {
                        if (this.UpdateNoBackQtyForUndrug(ItemList.RecipeNO, ItemList.SequenceNO, ItemList.Item.Qty, ItemList.BalanceState) == -1)
                        {
                            this.Err = "����ԭ�м�¼������������!����ͬʱ�������˽���ͬһ�����˷�!";
                            return -1;
                        }
                    }
                    //����µĴ�����
                    ItemList.RecipeNO = GetUndrugRecipeNO();
                }

                //�γɸ���¼
                ItemList.Item.Qty = -ItemList.Item.Qty;
                ItemList.FT.TotCost = -ItemList.FT.TotCost;
                ItemList.FT.OwnCost = -ItemList.FT.OwnCost;
                ItemList.FT.PayCost = -ItemList.FT.PayCost;
                ItemList.FT.PubCost = -ItemList.FT.PubCost;
                ItemList.FT.RebateCost = -ItemList.FT.RebateCost;
                ItemList.TransType = TransTypes.Negative;
                ItemList.FeeOper.ID = this.Operator.ID;
                ItemList.FeeOper.OperTime = this.GetDateTimeFromSysDateTime();
                if (ItemList.BalanceState == null) ItemList.BalanceState = "0";
                ItemList.BalanceOper.OperTime = this.GetDateTimeFromSysDateTime();
            }
            else
            {
                //if (ItemList.Item.IsPharmacy)
                if(ItemList.Item.ItemType == EnumItemType.Drug)
                {
                    ItemList.RecipeNO = GetDrugRecipeNO();
                }
                else
                {
                    ItemList.RecipeNO = this.GetUndrugRecipeNO();
                }

                ItemList.FeeOper.OperTime = this.GetDateTimeFromSysDateTime();
                ItemList.BalanceState = "1";
                ItemList.BalanceOper.OperTime = this.GetDateTimeFromSysDateTime();
            }

            //if (ItemList.Item.IsPharmacy)
            if(ItemList.Item.ItemType == EnumItemType.Drug)
            {
                if (this.InsertMedItemList(PatientInfo, ItemList) == -1)
                {
                    this.Err = "����ҩƷ��ϸ����!";
                    return -1;
                }
            }
            else
            {
                if (this.InsertFeeItemList(PatientInfo, ItemList) == -1)
                {
                    this.Err = "�����ҩƷ��ϸ����!";
                    return -1;
                }
            }
            //ItemList.Item.MinFee.ID=ItemList.Item.MinFee.ID;
            //���û��ܱ�
            //			if (this.UpdateAccount(PatientInfo,ItemList)==-1)
            //			{
            //				this.Err="������û��ܱ�ʧ��!";
            //				return -1;
            //			}

            //���Ѹ������޶�
            if (PatientInfo.Pact.PayKind.ID == "03")
            {
                //if (ItemList.Item.IsPharmacy == true || ItemList.FT.PubCost != 0)
                if(ItemList.Item.ItemType == EnumItemType.Drug || ItemList.FT.PubCost != 0)
                {
                    //���¹������޶��ۼ�
                    if (this.UpdateBursaryTotMedFee(PatientInfo.ID, ItemList.FT.TotCost) < 1)
                    {
                        this.Err = "���¹��ѻ������޶��ۼ�ʧ��!";
                        return -1;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 0�շ� 1�˷�   �˷�ʱ������ʵ��ΪҪ�˷ѵ�����¼(����¼�����ڴ���)  �շѵ�ʱ��ΪҪ�շѵĸ�ֵ��������¼ʵ��
        /// </summary>
        /// <param name="PatientInfo"></param>
        /// <param name="ItemList"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        private int FeeManager(Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList, string Flag)
        {
            //�жϷ����ܶ�
            if (ItemList.FT.TotCost == 0)
            {
                this.Err = "���ֲ�����ܶ�û�и�ֵ";
                return -1;
            }



            //�жϻ�ʿվ����
            if (PatientInfo.PVisit.PatientLocation.NurseCell.ID == null || PatientInfo.PVisit.PatientLocation.NurseCell.ID.Trim() == "")
            {
                this.Err = "���ֲ㻼�߻�ʿվ����û�и�ֵ!";
                return -1;
            }
            //�ж�ִ�п��Ҵ���
            if (ItemList.ExecOper.Dept.ID == null || ItemList.ExecOper.Dept.ID == "")
            {
                this.Err = "���ֲ�ִ�п���û�и�ֵ!";
                return -1;
            }
            //�ж��շѱ���
            if (ItemList.FTRate.ItemRate < 0)
            {
                this.Err = "�շѱ�����ֵ����!";
                return -1;
            }
            if (ItemList.FTRate.ItemRate == 0) ItemList.FTRate.ItemRate = 1;


            //��totcost��ȡ
            //			ItemList.FT.TotCost=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost,2);
            if (ItemList.User01 != "1" && Flag == "0") //���Ի��߷��ñ����޸�,���µ��õ�ʱ����Ҫ�ڼ�����ñ���
            {
                //���㻼�߷��ñ���
                if (this.ComputePatientFee(PatientInfo, ItemList) == -1)
                {
                    this.Err = "������ñ�������" + this.Err;
                    return -1;
                }

            }
            //Ϊ��ֹ���������ͳһת��Ϊ2λ��
            ItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost, 2);
            ItemList.FT.OwnCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.OwnCost, 2);
            ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.PayCost, 2);
            ItemList.FT.PubCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.PubCost, 2);
            //��ֹ�յ���λ��ֺ��¼Ϊ0
            if (ItemList.FT.TotCost == 0) return 0;
            //				//ҽ��Ԥ����
            //				Neusoft.HISFC.Models.Base.FT InmaininfoFee = new Neusoft.HISFC.Models.Base.FT();
            //				if(PatientInfo.Pact.ID=="2")  //����ҽ��
            //				{
            //					Neusoft.HISFC.BizLogic.Fee.Interface InterFace = new Interface();
            //					InterFace.SetTrans(this.command.Transaction);
            //
            //				
            //					//Ԥ����
            //					if(InterFace.CalculateSiFee(PatientInfo,ItemList,InmaininfoFee)==false)
            //					{
            //						this.Err=InterFace.Err;
            //						return -1;
            //					}
            //
            //				}


            if (Flag == "1")
            {
                //���¿������� Ȼ��ȡ���µĴ�����
                //if (ItemList.Item.IsPharmacy)
                if(ItemList.Item.ItemType == EnumItemType.Drug)
                {
                    if (ItemList.Memo != "BACKFEE")
                    {
                        if (this.UpdateNoBackQtyForDrug(ItemList.RecipeNO, ItemList.SequenceNO, ItemList.Item.Qty, ItemList.BalanceState) < 1)
                        {
                            this.Err = "����ԭ�м�¼������������!" + ItemList.Item.Name + "�����Ѿ����˷ѻ����!";
                            return -1;
                        }
                    }
                    //����µĴ�����
                    ItemList.RecipeNO = GetDrugRecipeNO();
                }
                else
                {
                    if (ItemList.Memo != "BACKFEE")
                    {
                        if (this.UpdateNoBackQtyForUndrug(ItemList.RecipeNO, ItemList.SequenceNO, ItemList.Item.Qty, ItemList.BalanceState) < 1)
                        {
                            this.Err = "����ԭ�м�¼������������!" + ItemList.Item.Name + "�����Ѿ����˷ѻ����!";
                            return -1;
                        }
                    }
                    //����µĴ�����
                    ItemList.RecipeNO = GetUndrugRecipeNO();
                }
                //�γɸ���¼
                ItemList.Item.Qty = -ItemList.Item.Qty;
                ItemList.FT.TotCost = -ItemList.FT.TotCost;
                ItemList.FT.OwnCost = -ItemList.FT.OwnCost;
                ItemList.FT.PayCost = -ItemList.FT.PayCost;
                ItemList.FT.PubCost = -ItemList.FT.PubCost;
                ItemList.FT.RebateCost = -ItemList.FT.RebateCost;
                ItemList.TransType = TransTypes.Negative;
                ItemList.FeeOper.ID = this.Operator.ID;
                ItemList.FeeOper.OperTime = this.GetDateTimeFromSysDateTime();

                if (ItemList.BalanceState == null) ItemList.BalanceState = "0";
            }
            else                                                            //Add By Maokb
            {
                ItemList.Patient.Pact.ID = PatientInfo.Pact.ID;     //Add By Maokb
                ItemList.Patient.Pact.PayKind.ID = PatientInfo.Pact.PayKind.ID;       //Add By maokb
            }

            ItemList.ChargeOper.OperTime = ItemList.FeeOper.OperTime; //���ֻ���ʱ����շ�ʱ��ͬ��

            //if (ItemList.Item.IsPharmacy)
            if(ItemList.Item.ItemType == EnumItemType.Drug)
            {
                if (this.InsertMedItemList(PatientInfo, ItemList) == -1)
                {
                    this.Err = "����ҩƷ��ϸ����!";
                    return -1;
                }
            }
            else
            {
                if (this.InsertFeeItemList(PatientInfo, ItemList) == -1)
                {
                    this.Err = "�����ҩƷ��ϸ����!";
                    return -1;
                }
            }
            //ItemList.Item.MinFee.ID=ItemList.Item.MinFee.ID;
            //���û��ܱ�
            //			if (this.UpdateAccount(PatientInfo,ItemList)==-1)
            //			{
            //				this.Err="������û��ܱ�ʧ��!"+this.Err;
            //				return -1;
            //			}
            //�����������
            //			if(PatientInfo.Pact.ID=="2")
            //			{
            //				if(this.UpdateInmaininfoFeeForMedicare(PatientInfo,InmaininfoFee)==-1)
            //				{
            //					this.Err="ҽ����������ʧ��"+this.Err;
            //					return -1;
            //				}
            //			}
            //			else
            //			{

            //Moidfy By ���� ���Ͼ͵�2005��12��26����,�����Ƶ�ʥ����...
            //�޸ĸ�������SQL��䣬����Where in_state <> 'O'�Ͳ�����in_state <> C���������Ʋ���
            //������ߴ��ڽ���״̬�����Ʋ�����¼�����. �� C����״̬������¼������˷���
            //��֤�����嵥����ȷ.
            int iReturn = this.UpdateInMainInfoFee(PatientInfo.ID, ItemList.FT);

            if (iReturn == -1)
            {
                this.Err = "����סԺ����ʧ��!";
                return -1;
            }
            //�������Ϊ0 ˵��������in_state <> 0��������ǰ̨��������¼�����.
            if (iReturn == 0)
            {
                this.Err = PatientInfo.Name + "�Ѿ�������߳��ڷ���״̬�����ܼ���¼�����!����סԺ����ϵ!";
                return -1;
            }
            //Modify���
            //			}
            //���¹���ҩ�ۼ�
            if (PatientInfo.Pact.PayKind.ID == "03" && ItemList.ExtFlag != "1")
            {
                //��Ϊ��ҩƷ��ϸ����Ҳ��ҩƷ--by Maokb
                //�˴�ʹ��PubCost!=0,��Ϊ�Է�ҩ������ʱ��,TotCost��0,����PubCost!=0
                if ((ItemList.Item.MinFee.ID == "001" || ItemList.Item.MinFee.ID == "002" || ItemList.Item.MinFee.ID == "003") && ItemList.FT.PubCost != 0)
                {
                    //���¹���ҩ�ۼ�
                    /*
                     * ����ԭ����PubCost�����£����û�����Ҳ��������
                     * this.UpdateBursaryTotMedFee(PatientInfo.ID,ItemList.Fee.Pub_Cos
                     * ���Ǻ��û���ѯ���ʵ��ҩ�޶��ָ���ǳ���ҩƷ�ܶ�֣�������TotCost������
                     * */
                    if (this.UpdateBursaryTotMedFee(PatientInfo.ID, ItemList.FT.TotCost) < 1)
                    {
                        this.Err = "���¹��ѻ������޶��ۼ�ʧ��!" + this.Err;
                        return -1;
                    }
                }
            }
            return 0;
        }
        #endregion
        /// <summary>
        /// ���㻼�߷���---����������
        /// </summary>
        /// <param name="Pinfo"></param>
        /// <param name="ItemList"></param>
        /// <returns></returns>
        public int ComputePatientFee(Neusoft.HISFC.Models.RADT.PatientInfo Pinfo, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList)
        {
            if (ItemList.Item.MinFee.ID == null || ItemList.Item.MinFee.ID.Trim() == "") ItemList.Item.MinFee.ID = ItemList.Item.MinFee.ID;
            if (ItemList.FT.TotCost == 0)
            {
                this.Err = "�����ܶ�Ϊ0,��ȡû������!";
                return -1;
            }
            //���������������ֻ��Tot_cost���ã�����Cost��Ч
            //��������Щ�շ��и�own_cost��ֵ���ڴ�ȡ��
            ItemList.FT.OwnCost = 0;
            ItemList.FT.PayCost = 0;
            ItemList.FT.PubCost = 0;

            //�Էѻ���ֱ�Ӹ�ֵ����
            if (Pinfo.Pact.PayKind.ID == "01")
            {
                ItemList.FT.OwnCost = ItemList.FT.TotCost;
                ItemList.FT.PayCost = 0;
                ItemList.FT.PubCost = 0;

                return 0;
            }
            //�Է���Ŀ���ԷѴ���
            if (ItemList.ExtFlag == "1")
            {//�����������ҩƷ,��תΪ�Է���Ŀ,��־Ϊ1
                //��������������Ŀ��������ǰΪ�Էѣ���־1
                ItemList.FT.OwnCost = ItemList.FT.TotCost;
                ItemList.FT.PayCost = 0;
                ItemList.FT.PubCost = 0;
                return 0;
            }

            //ҽ�����ߣ���ʱ�ԷѴ���--����
            if (Pinfo.Pact.ID == "2")//Pinfo.Pact.PayKind.ID=="02"||
            {
                #region Delete By ��֪��˭
                //ҽ�����ߴ������

                Fee.Interface Interface = new Interface();
                //Interface.SetTrans(this.command.Transaction);

                if (Interface.ComputeRate(Pinfo.Pact.ID, ref ItemList) == -1)
                {
                    this.Err = "��ȡҽ�����߷��ñ����������!";
                    return -1;
                }
                return 0;

                #endregion
                //				ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //				ItemList.FT.PayCost=0;
                //				ItemList.FT.PubCost=0;
            }
            #region ���ѻ���
            if (Pinfo.Pact.PayKind.ID == "03")
            {
                //���ѻ���--��������ҩ���Է��ر�����-��ʳ-��Ʒ-������-���Է�·��---���жϺ�ͬ��λǰ����

                /*�Թ��ѻ���
                 * 1.����ñ���
                 * 2.��λ���Ƿ񳬱�
                 * 3.ҩƷ�Ƿ񳬱�
                 * */
                //����ñ���

                Neusoft.HISFC.Models.Base.FTRate Rate = this.ComputeFeeRate(Pinfo.Pact.ID, ItemList.Item);
                if (Rate == null)
                {
                    return -1;
                }
                if (Pinfo.CaseState == "S" && Pinfo.PVisit.InState.ID.ToString() == "B")//��������Ǽǻ��ߣ����ά��ȡ�˱���
                {
                    Rate.OwnRate = 0;
                    Rate.PayRate = Pinfo.FT.FTRate.PayRate;
                    this.ComputeCost(ItemList, Rate);
                    return 0;
                }


                //�ж��Ƿ��Ǽ໤�������ǣ�ȡ�໤����׼
                if (this.IsIcuBedList(ItemList.Item.ID))
                {
                    //��λ�޶�
                    decimal IcuLimit = Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.AirLimitCost * ItemList.Item.Qty, 2);
                    //��������
                    if (Pinfo.FT.BedOverDeal == "1")
                    {
                        if (IcuLimit >= ItemList.FT.TotCost)
                        {
                            //�໤����׼���ڼ໤���ѣ�������
                            this.ComputeCost(ItemList, Rate);
                            return 0;
                        }
                        else
                        {
                            //���꣬���겿���Է�
                            ItemList.FT.OwnCost = ItemList.FT.TotCost - IcuLimit;
                            this.ComputeCost(ItemList, Rate);
                            return 0;
                        }
                    }
                    //���겻�� ȫ���
                    if (Pinfo.FT.BedOverDeal == "0")
                    {
                        this.ComputeCost(ItemList, Rate);
                        return 0;
                    }
                    //���겻�ƣ������޶��ڣ�ʣ�µ����
                    if (Pinfo.FT.BedOverDeal == "2")
                    {
                        //����
                        if (ItemList.FT.TotCost > IcuLimit)
                        {
                            ItemList.FT.TotCost = IcuLimit;
                        }
                        this.ComputeCost(ItemList, Rate);
                        return 0;
                    }
                }

                // ����Ǵ�λ��
                if (ItemList.Item.MinFee.ID == "A02")
                {

                    //��λ�޶�
                    decimal BedLimitCost = Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.BedLimitCost * ItemList.Item.Qty, 2);
                    //��������
                    if (Pinfo.FT.BedOverDeal == "1")
                    {
                        //������
                        if (ItemList.FT.TotCost <= BedLimitCost)
                        {
                            this.ComputeCost(ItemList, Rate);
                            return 0;
                        }
                        else
                        {//���겿��תΪ�Է�
                            ItemList.FT.OwnCost = ItemList.FT.TotCost - BedLimitCost;
                            this.ComputeCost(ItemList, Rate);
                            return 0;
                        }
                    }
                    //���겻�� ȫ���
                    if (Pinfo.FT.BedOverDeal == "0")
                    {
                        this.ComputeCost(ItemList, Rate);
                        return 0;
                    }
                    //���겻�ƣ������޶��ڣ�ʣ�µ����
                    if (Pinfo.FT.BedOverDeal == "2")
                    {
                        //����
                        if (ItemList.FT.TotCost > BedLimitCost)
                        {
                            ItemList.FT.TotCost = BedLimitCost;
                        }
                        this.ComputeCost(ItemList, Rate);
                        return 0;
                    }
                }
                #region Delete By Maokb 05-10-18
                // �����ҩƷ
                //				if(ItemList.Item.MinFee.ID == "001"||ItemList.Item.MinFee.ID == "002"||ItemList.Item.MinFee.ID =="003")
                //				{
                //					
                //					if(Pinfo.FT.OvertopCost < 0)
                //					{
                //						//���������
                //						if(ItemList.FT.TotCost + Pinfo.FT.OvertopCost < 0)
                //						{
                //							this.ComputeCost(ItemList,Rate);
                //							//�������ó�����
                //							Pinfo.FT.OvertopCost = Pinfo.FT.OvertopCost + ItemList.FT.TotCost;
                //							return 0;
                //						}
                //						else
                //						{//���뱾��Ŀǰ�����꣬���뱾��Ŀ�󳬱꣬���겿��תΪ�Է�
                //							ItemList.FT.OwnCost = ItemList.FT.TotCost + Pinfo.FT.OvertopCost ;
                //							this.ComputeCost(ItemList,Rate);
                //							//�������ó�����
                //							Pinfo.FT.OvertopCost = Pinfo.FT.OvertopCost + ItemList.FT.TotCost;
                //							return 0;
                //						}
                //					}
                //					else
                //					{//���뱾��Ŀǰ�Ѿ�����,ȫ���Է�
                //						ItemList.FT.OwnCost = ItemList.FT.TotCost;
                //						ItemList.FT.PayCost = 0;
                //						ItemList.FT.PubCost = 0;
                //						//�������ó�����
                //						Pinfo.FT.OvertopCost = Pinfo.FT.OvertopCost + ItemList.FT.TotCost;
                //						return 0;
                //					}
                //					
                //
                //				}
                #endregion
                // ����
                this.ComputeCost(ItemList, Rate);
                return 0;

                #region Delete By Maokb--05-10-15
                //				// �Է���Ŀ -- By Maokb
                //				/*�����Է���Ŀ���Բ��жϺ�ͬ��λֱ�Ӹ�ֵ����
                //				 * ������С�����ж��Ƿ����Է���Ŀ
                //				 * 1.����2.δ�����
                //				 * */
                //				if(ItemList.Item.MinFee.ID=="A03")
                //				{
                //					//����ǳ��꣬ȫ���Է�
                //					ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //					ItemList.FT.PayCost=0;
                //					ItemList.FT.PubCost=0;
                //
                //					return 0;
                //				}
                //				//�����ͬ��λʵ��
                //				Neusoft.HISFC.Models.Base.PactInfo PactUnitInfo = new Neusoft.HISFC.Models.Base.PactInfo();
                //				Neusoft.HISFC.Management.Base.PactInfo PactManagment = new PactUnitInfo();
                //				PactManagment.SetTrans(this.command.Transaction);
                //				PactUnitInfo = PactManagment.GetPactUnitInfoByPactCode(Pinfo.Patient.Pact.ID);
                //				if(PactUnitInfo==null)
                //				{
                //					this.Err="�������ߵĺ�ͬ��λ��Ϣʧ��";
                //					return -1;
                //				}
                //				#region ��λ�ѵ�ԭ��ȡ���� Delete By Maokb
                //			
                //				//			//���ѺͿյ����ն������¼�
                //				//			if(ItemList.Item.MinFee.ID=="A02")//����
                //				//			{
                //				//				if(ItemList.Memo=="Wangrc") return 0;
                //				//				//û�г��괲�������
                //				//				if(Pinfo.FT.BedLimitCost==0)
                //				//				{
                //				//					ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.PayCost=0;
                //				//					ItemList.FT.PubCost=0;
                //				//					return 0;
                //				//				}
                //				//				//���겻�� ȫ���
                //				//				if(Pinfo.FT.BedOverDeal=="0")
                //				//				{
                //				//					ItemList.FT.PubCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.OwnCost=0;
                //				//					ItemList.FT.PayCost=0;
                //				//					return 0;
                //				//				}
                //				//				//zhangjunyi@Neusoft.com 2005/8/23 ���� ���겻�Ƶ��㷨 ��ǰû�д����������
                //				//				//���겻�� 
                //				//				if((ItemList.FT.TotCost > Pinfo.FT.BedLimitCost)&&Pinfo.FT.BedOverDeal=="2") 
                //				//				{
                //				//					//���겻��
                //				//					ItemList.FT.OwnCost = 0;
                //				//					ItemList.FT.PubCost = Pinfo.FT.BedLimitCost;
                //				//					ItemList.FT.PayCost = 0;
                //				//					return 0;
                //				//				}
                //				//
                //				//				//С�ڳ�����ȫ����������
                //				//				if(ItemList.FT.TotCost <= Pinfo.FT.BedLimitCost)
                //				//				{
                //				//					ItemList.FT.OwnCost=0;
                //				//					ItemList.FT.PubCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.PayCost=0;
                //				//				}
                //				//				else //��ֳ�������¼ ���겿��Ϊ�Է�
                //				//				{
                //				//					//��¡
                //				//					Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList CloneItem = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                //				//					CloneItem = ItemList.Clone();
                //				//					//���
                //				//					CloneItem.FT.PubCost=Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.BedLimitCost*CloneItem.Item.Qty,2);
                //				//					CloneItem.FT.OwnCost=0;
                //				//					CloneItem.FT.PayCost=0;
                //				//					CloneItem.FT.TotCost=CloneItem.FT.PubCost;
                //				//
                //				//					//���Ƶ���
                //				//					if(CloneItem.FT.TotCost!=ItemList.FT.TotCost)
                //				//					{
                //				//						CloneItem.Item.Name=CloneItem.Item.Name+"(��������)";
                //				//					}
                //				//					CloneItem.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(CloneItem.FT.TotCost/CloneItem.Item.Qty,2);
                //				//					CloneItem.Memo="Wangrc";
                //				//					//�����շѺ���
                //				//					if(this.FeeManager(Pinfo,CloneItem)==-1) return -1;
                //				//
                //				//					//��ֵ���һ��
                //				//					ItemList.Memo="Wangrc";
                //				//
                //				//					//��С����
                //				//					ItemList.Item.MinFee.ID="A10";
                //				//					ItemList.Item.MinFee.ID="A10";
                //				//					//������
                //				//					ItemList.RecipeNO=this.GetUndrugNewNoteNo();
                //				//					//���
                //				//					ItemList.FT.TotCost=ItemList.FT.TotCost-Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.BedLimitCost*ItemList.Item.Qty,2);;
                //				//					ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.PayCost=0;
                //				//					ItemList.FT.PubCost=0;
                //				//					//��������
                //				//					ItemList.Item.Name=ItemList.Item.Name+"(���겿��)";
                //				//					ItemList.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost/ItemList.Item.Qty,2);
                //				//				}
                //				//				return 0;
                //				//			}
                //				//			if(ItemList.Item.MinFee.ID=="A03") //�յ�
                //				//			{
                //				//				if(ItemList.Memo=="Wangrc") return 0;
                //				//				//û�г���յ�
                //				//				if(Pinfo.FT.AirLimitCost==0)
                //				//				{
                //				//					ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.PayCost=0;
                //				//					ItemList.FT.PubCost=0;
                //				//					return 0;
                //				//				}
                //				//				//С�ڳ�����ȫ����������
                //				//				if(ItemList.FT.TotCost <= Pinfo.FT.AirLimitCost)
                //				//				{
                //				//					ItemList.FT.OwnCost=0;
                //				//					ItemList.FT.PayCost=0;
                //				//					ItemList.FT.PubCost=ItemList.FT.TotCost;
                //				//		
                //				//				}
                //				//				else//��ֳ�������¼ ���겿��Ϊ�Է�
                //				//				{
                //				//					//��¡
                //				//					Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList CloneItem = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                //				//					CloneItem = ItemList.Clone();
                //				//
                //				//                    //���
                //				//					CloneItem.FT.PubCost=Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.AirLimitCost*CloneItem.Item.Qty,2);
                //				//					CloneItem.FT.OwnCost=0;
                //				//					CloneItem.FT.PayCost=0;
                //				//					CloneItem.FT.TotCost=CloneItem.FT.PubCost;
                //				//
                //				//                    //���Ƶ���
                //				//					if(CloneItem.FT.TotCost !=ItemList.FT.TotCost)
                //				//					{
                //				//						CloneItem.Item.Name=CloneItem.Item.Name+"(��������)";
                //				//					}
                //				//					CloneItem.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(CloneItem.FT.TotCost/CloneItem.Item.Qty,2);
                //				//					CloneItem.Memo="Wangrc";
                //				//					//�����շѺ���
                //				//					if(this.FeeManager(Pinfo,CloneItem)==-1) return -1;
                //				//
                //				//					//��ֵ���һ��
                //				//					ItemList.Memo="Wangrc";
                //				//					//��С����
                //				//					ItemList.Item.MinFee.ID="A11";
                //				//					ItemList.Item.MinFee.ID="A11";
                //				//				    //������
                //				//					ItemList.RecipeNO=this.GetUndrugNewNoteNo();
                //				//					//���
                //				//					ItemList.FT.TotCost=ItemList.FT.TotCost-Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.AirLimitCost*CloneItem.Item.Qty,2);
                //				//					ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //				//					ItemList.FT.PayCost=0;
                //				//					ItemList.FT.PubCost=0;
                //				//					//���Ƶ���
                //				//					ItemList.Item.Name=ItemList.Item.Name+"(���겿��)";
                //				//					ItemList.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost/ItemList.Item.Qty,2);
                //				//				}
                //				//				return 0;
                //				//			}
                //				#endregion
                //			
                //				#region ��ȡ��λ��
                //				/*��ȡ��λ�ѵĹ����������ģ�
                //				 * 1.�ж��Ƿ��ǹ��ѻ���
                //				 * 2.�жϴ�λ���Ƿ񳬱꣬���겿��תΪ�Է�
                //				 *  1)
                //				 * 3.�ж��Ƿ��ǳ��겻��-���ǣ�ȫ������
                //				 * 4.�ж��Ƿ��ǳ��겻��-���ǣ����������޶��ڲ��֣������������
                //				 * 
                //				 * */
                //				if(Pinfo.Pact.PayKind.ID == "03")
                //				{
                //					if(ItemList.Memo=="Wangrc") return 0;
                //					//����
                //					if(ItemList.Item.MinFee.ID == "A02")
                //					{//��λ��
                //
                //						//��λ�޶�
                //						decimal BedLimitCost=Neusoft.FrameWork.Public.String.FormatNumber(Pinfo.FT.BedLimitCost * ItemList.Item.Qty,2);
                //
                //						//��������
                //						if(Pinfo.FT.BedOverDeal =="1")
                //						{
                //							//������
                //							if(ItemList.FT.TotCost <= BedLimitCost)
                //							{
                //								ItemList.FT.OwnCost = 0;
                //								ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost * PactUnitInfo.Rate.PayRate,2);
                //								ItemList.FT.PubCost =ItemList.FT.TotCost - ItemList.FT.PayCost;
                //								return 0;
                //							}
                //							else
                //							{
                //								//��¡
                //								Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList CloneItem = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                //								CloneItem = ItemList.Clone();
                //
                //								//���
                //								CloneItem.FT.TotCost = BedLimitCost;
                //								CloneItem.FT.PayCost=Neusoft.FrameWork.Public.String.FormatNumber(BedLimitCost * PactUnitInfo.Rate.PayRate,0);							
                //								CloneItem.FT.OwnCost=0;
                //								CloneItem.FT.PubCost=ItemList.FT.TotCost - ItemList.FT.PayCost;
                //							
                //
                //								//���Ƶ���
                //								if(CloneItem.FT.TotCost !=ItemList.FT.TotCost)
                //								{
                //									CloneItem.Item.Name=CloneItem.Item.Name+"(��������)";
                //								}
                //								CloneItem.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(CloneItem.FT.TotCost/CloneItem.Item.Qty,2);
                //								CloneItem.Memo="Wangrc";
                //								//�����շѺ���
                //								if(this.FeeManager(Pinfo,CloneItem)==-1) return -1;
                //
                //								//��ֵ���һ��
                //								ItemList.Memo="Wangrc";
                //								//��С����
                //								ItemList.Item.MinFee.ID="A03";
                //								ItemList.Item.MinFee.ID="A03";
                //								//������
                //								ItemList.RecipeNO=this.GetUndrugNewNoteNo();
                //								//���
                //								ItemList.FT.TotCost=ItemList.FT.TotCost-CloneItem.FT.TotCost;
                //								ItemList.FT.OwnCost=ItemList.FT.TotCost;
                //								ItemList.FT.PayCost=0;
                //								ItemList.FT.PubCost=0;
                //								//���Ƶ���
                //								ItemList.Item.Name=ItemList.Item.Name+"(���겿��)";
                //								ItemList.Item.Price=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost/ItemList.Item.Qty,2);	
                //	
                //								return 0;
                //
                //							}
                //						}
                //					 
                //						//���겻�� ȫ���
                //						if(Pinfo.FT.BedOverDeal=="0")
                //						{
                //							
                //							ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost * PactUnitInfo.Rate.PayRate,2);
                //							ItemList.FT.OwnCost = 0;
                //							ItemList.FT.PubCost = ItemList.FT.TotCost - ItemList.FT.PayCost;						
                //							return 0;
                //						}
                //						//���겻�ƣ������޶��ڣ�ʣ�µ����
                //						if(Pinfo.FT.BedOverDeal =="2")
                //						{
                //							//����
                //							if(ItemList.FT.TotCost >	BedLimitCost) 
                //							{
                //								ItemList.FT.TotCost =BedLimitCost; 
                //								ItemList.FT.OwnCost =0;
                //								ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost * PactUnitInfo.Rate.PayRate,2);
                //								ItemList.FT.PubCost = ItemList.FT.TotCost - ItemList.FT.PayCost;
                //								return 0;
                //							}
                //							else
                //							{//������
                //								ItemList.FT.OwnCost = 0;
                //								ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost * PactUnitInfo.Rate.PayRate,2);
                //								ItemList.FT.PubCost =ItemList.FT.TotCost - ItemList.FT.PayCost;
                //								return 0;
                //							}
                //						}					
                //					}
                //				}			
                //				#endregion
                //                //��������---ȡ��������-----
                //				//ԭ��,��Ŀ-��С����-��ͬ����
                //				Neusoft.HISFC.Management.Base.PactItemRate PactItemRate = new PactUnitItemRate();
                //				PactItemRate.SetTrans(this.command.Transaction);
                //				Neusoft.HISFC.Models.Base.PactItemRate ObjPactItemRate = new Neusoft.HISFC.Models.Base.PactItemRate();
                //				//��Ŀ
                //				ObjPactItemRate= PactItemRate.GetOnepPactUnitItemRateByItem(Pinfo.Patient.Pact.ID,ItemList.Item.ID);
                //				if(ObjPactItemRate ==null)
                //				{
                //					//��С����
                //					ObjPactItemRate= PactItemRate.GetOnepPactUnitItemRateByItem(Pinfo.Patient.Pact.ID,ItemList.Item.MinFee.ID);
                //					if(ObjPactItemRate ==null)
                //					{
                //						//ȡ��ͬ��λ�ı���
                //						try
                //						{
                //							ObjPactItemRate=new Neusoft.HISFC.Models.Base.PactItemRate();
                //							ObjPactItemRate.Rate.PayRate=PactUnitInfo.Rate.PayRate;
                //							ObjPactItemRate.Rate.OwnRate=PactUnitInfo.Rate.OwnRate;
                //						}
                //						catch(Exception ex)
                //						{
                //							this.Err=ex.Message;
                //							return -1;
                //						}
                //					}
                //				}
                //			
                //				//		#region  zhangjunyi@Neusoft.com 2005/08/23 ɾ�� ��Ϊ�±߲���  ɾ��ԭ�� ���Է�ҩû�а� ȫ���ԷѴ���
                //				//����
                //				ItemList.FT.OwnCost=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost*ObjPactItemRate.Rate.OwnRate,2);
                //				ItemList.FT.PayCost=Neusoft.FrameWork.Public.String.FormatNumber((ItemList.FT.TotCost - ItemList.FT.OwnCost)*ObjPactItemRate.Rate.PayRate,2);
                //				ItemList.FT.PubCost=ItemList.FT.TotCost-ItemList.FT.OwnCost-ItemList.FT.PayCost;	
                //				//	#endregion 
                //				#region ���жϱ���ǰ�ж��Է���Ŀ Delete By Maokb
                //				//			if(ItemList.Item.MinFee.ID=="A03"  ||ItemList.Item.MinFee.ID=="A10"||ItemList.Item.MinFee.ID=="A11"  ) //�Է�ҩ �ͳ��괲 ȫ���Է�
                //				//			{
                //				//				//����
                //				//				ItemList.FT.OwnCost = ItemList.FT.TotCost;
                //				//				ItemList.FT.PayCost=0;
                //				//				ItemList.FT.PubCost= 0;
                //				//			}
                //				//			else
                //				//			{
                //				//				//����
                //				//				ItemList.FT.OwnCost=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost*ObjPactItemRate.Rate.OwnRate,2);
                //				//				ItemList.FT.PayCost=Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost*ObjPactItemRate.Rate.PayRate,2);
                //				//				ItemList.FT.PubCost=ItemList.FT.TotCost-ItemList.FT.OwnCost-ItemList.FT.PayCost; 
                //				//			}
                //				#endregion
                #endregion
            }
            #endregion
            return 0;

        }
        /// <summary>
        ///  �����ܷ��õĸ�����ɲ��ֵ�ֵ
        /// </summary>
        /// <param name="ItemList"></param>
        /// <param name="rate">������֮��ı���</param>
        /// <returns>-1ʧ�ܣ�0�ɹ�</returns>
        private int ComputeCost(Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList, Neusoft.HISFC.Models.Base.FTRate rate)
        {
            if (ItemList.FT.OwnCost == 0)
            {
                ItemList.FT.OwnCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.FT.TotCost * rate.OwnRate, 2);
                ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber((ItemList.FT.TotCost - ItemList.FT.OwnCost) * rate.PayRate, 2);
                ItemList.FT.PubCost = ItemList.FT.TotCost - ItemList.FT.OwnCost - ItemList.FT.PayCost;
            }
            else
            {
                ItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber((ItemList.FT.TotCost - ItemList.FT.OwnCost) * rate.PayRate, 2);
                ItemList.FT.PubCost = ItemList.FT.TotCost - ItemList.FT.OwnCost - ItemList.FT.PayCost;
            }
            return 0;

        }

        /// <summary>
        /// ���ʡ��ҽ�ı���
        /// </summary>
        /// <returns></returns>
        public string[] GetProPayCode()
        {
            string[] Code = new string[50];
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.GetProPayCode", ref strSql) == -1)
            {
                this.Err = "Can't Find Sql";
                return null;
            }
            if (this.ExecQuery(strSql) == -1)
            {
                this.Err = "ִ�г���!";
                return null;
            }
            int i = 0;
            while (this.Reader.Read())
            {
                Code[i] = this.Reader[0].ToString();
                i++;
            }
            this.Reader.Close();
            return Code;
        }
        /// <summary>
        /// ����й�ҽ�ı���
        /// </summary>
        /// <returns></returns>
        public string[] GetCityPayCode()
        {
            string[] Code = new string[50];
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.GetCityPayCode", ref strSql) == -1)
            {
                this.Err = "Can't Find Sql";
                return null;
            }
            if (this.ExecQuery(strSql) == -1)
            {
                this.Err = "ִ�г���!";
                return null;
            }
            int i = 0;
            while (this.Reader.Read())
            {
                Code[i] = this.Reader[0].ToString();
                i++;
            }
            this.Reader.Close();
            return Code;
        }
        /// <summary>
        /// ���߷��ñ���
        /// </summary>
        /// <param name="PactID">��ͬ��λ����</param>
        /// <param name="item">ҩƷ��ҩƷ��Ϣ</param>
        /// <returns>ʧ��null���ɹ� Neusoft.HISFC.Models.Fee.FtRate</returns>
        public Neusoft.HISFC.Models.Base.FTRate ComputeFeeRate(string PactID, Neusoft.HISFC.Models.Base.Item item)
        {
            //Neusoft.HISFC.Management.Base.PactItemRate PactItemRate = new PactUnitItemRate();
            //PactItemRate.SetTrans(this.command.Transaction);
            //
            Neusoft.HISFC.Models.Base.PactItemRate ObjPactItemRate = null;
            //			try
            //			{
            //				//��Ŀ
            //				ObjPactItemRate= PactItemRate.GetOnepPactUnitItemRateByItem(PactID,item.ID);
            //				if(ObjPactItemRate ==null)
            //				{
            //					//��С����
            //					ObjPactItemRate= PactItemRate.GetOnepPactUnitItemRateByItem(PactID,item.MinFee.ID);
            //					if(ObjPactItemRate ==null)
            //					{
            //						//ȡ��ͬ��λ�ı���
            //						Neusoft.HISFC.Management.Base.PactInfo PactManagment = new PactUnitInfo();
            //						//PactManagment.SetTrans(this.command.Transaction);
            //						Neusoft.HISFC.Models.Base.PactInfo PactUnitInfo = PactManagment.GetPactUnitInfoByPactCode(PactID);
            //						if(PactUnitInfo == null) {
            //							this.Err = PactManagment.Err;
            //							return null;
            //						}
            //						try
            //						{
            //							ObjPactItemRate=new Neusoft.HISFC.Models.Base.PactItemRate();
            //							ObjPactItemRate.Rate.PayRate=PactUnitInfo.Rate.PayRate;
            //							ObjPactItemRate.Rate.OwnRate=PactUnitInfo.Rate.OwnRate;
            //						}
            //						catch(Exception ex)
            //						{
            //							this.Err=ex.Message;
            //							return null;
            //						}
            //					}
            //				}
            //			}
            //			catch(Exception ee)
            //			{
            //				this.Err = ee.Message;
            //				return null;
            //			}
            return ObjPactItemRate.Rate;
        }

        /// <summary>
        /// ����Ԥ�������
        /// </summary>
        /// <param name="info">������Ϣ</param>
        /// <returns>1:�ɹ� -1:ʧ��</returns>
        public int GetFeePreBalance(Neusoft.HISFC.Models.RADT.PatientInfo info)
        {
            string strSql = string.Empty;
            if (this.Sql.GetSql("Fee.InpatientFee.GetFeePreBalance", ref strSql) == -1)
            {
                this.Err = "����Sql���Fee.InpatientFee.GetFeePreBalance����";
                return -1;
            }
            try
            {
                strSql = string.Format(strSql, info.ID);
                if (this.ExecQuery(strSql) == -1)
                {
                    this.Err = "��ʽ��SQL���ʧ�ܣ�";
                }
                while (this.Reader.Read())
                {
                    info.SIMainInfo.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[0]);
                    info.SIMainInfo.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[1]);
                    info.SIMainInfo.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[2]);
                    info.SIMainInfo.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[3]);
                    info.SIMainInfo.OverCost = 0;
                    info.SIMainInfo.OfficalCost = 0;
                }
                return 1;
            }
            catch
            {
                this.Err = "��������ʧ�ܣ�";
                return -1;
            }
            finally
            {
                this.Reader.Close();
            }
        }

        /// <summary>
        /// �������޶�����
        /// </summary>
        /// <param name="pInfo">���߻�����Ϣ</param>
        /// <returns>-1ʧ�ܣ�0�ɹ�</returns>
        public int AdjustOverLimitFee(Neusoft.HISFC.Models.RADT.PatientInfo pInfo)
        {
            int parm = 0;
            //�ж��Ƿ��й���ҩ������
            if (pInfo.Pact.PayKind.ID == "03")
            {
                if (pInfo.FT.OvertopCost == 0) return 0;

                //�ж��Ƿ��ǳ��꣬����г��꣬�������겿��
                if (pInfo.FT.OvertopCost > 0)
                {
                    #region �ӹ���תΪ�Է�
                    //��Ҫ�жϸ����ַ��õĽ�����������ָ�ֵ
                    ArrayList alFee = new ArrayList();
                    //�������߷����б����
                    alFee = this.QueryFeeInfosGroupByMinFeeForAdjustOverTopByInpatientNO(pInfo.ID);
                    //��ҩ��
                    decimal WCost = 0m;
                    //��ҩ��
                    decimal PCost = 0m;
                    //��ҩ��
                    decimal CCost = 0m;
                    //��ҩ�ĸ���¼���
                    decimal WShouldCost = 0m;
                    //��ҩ�ĸ���¼���
                    decimal PShouldCost = 0m;
                    //��ҩ�ĸ���¼���
                    decimal CShouldCost = 0m;

                    if (alFee == null)
                    {
                        this.Err = this.Err + "�������߷��ó���!";
                        return -1;
                    }
                    //ѭ���õ�������ҩ�Ѹ�����ֵ
                    for (int i = 0; i < alFee.Count; i++)
                    {
                        Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo fInfo;
                        fInfo = (Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo)alFee[i];
                        if (fInfo.Item.MinFee.ID == "001") WCost = fInfo.FT.TotCost;
                        if (fInfo.Item.MinFee.ID == "002") PCost = fInfo.FT.TotCost;
                        if (fInfo.Item.MinFee.ID == "003") CCost = fInfo.FT.TotCost;
                    }
                    //���㸺��¼���������
                    if (pInfo.FT.OvertopCost <= WCost)
                    {
                        //��ҩ����¼���
                        WShouldCost = pInfo.FT.OvertopCost;
                    }
                    else
                    {
                        //��ҩ����¼���
                        WShouldCost = WCost;
                        //�����ҩ����
                        if (pInfo.FT.OvertopCost - WCost <= PCost)
                        {
                            //��ҩ����¼���
                            PShouldCost = (pInfo.FT.OvertopCost - WCost);
                        }
                        else
                        {

                            PShouldCost = PCost;
                            //������ҩ����¼���
                            if (pInfo.FT.OvertopCost - WCost - PCost <= CCost)
                            {
                                //��ҩ����¼���
                                CShouldCost = (pInfo.FT.OvertopCost - WCost - PCost);
                            }
                            else
                            {
                                this.Err = "��������ڷ���ҩƷ�����ܶ�!���ܴ��ڲ�������!";
                                return -1;
                            }
                        }
                    }
                    //ȡ��ǰʱ��
                    DateTime dtNow = this.GetDateTimeFromSysDateTime();

                    //ȡ���߷��ñ���					
                    Neusoft.HISFC.Models.Base.PactInfo PactUnitInfo = new Neusoft.HISFC.Models.Base.PactInfo();
                    Neusoft.HISFC.BizLogic.Fee.PactUnitInfo PactManagment = new Neusoft.HISFC.BizLogic.Fee.PactUnitInfo();//Base.PactInfo();
                    //PactManagment.SetTrans(this.command.Transaction);
                    PactUnitInfo = PactManagment.GetPactUnitInfoByPactCode(pInfo.Pact.ID);


                    // �����Է�ҩ����
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                    Neusoft.HISFC.Models.Pharmacy.Item PhaItem = new Neusoft.HISFC.Models.Pharmacy.Item();
                    ItemList.Item = PhaItem;
                    // ��ֵ
                    //ItemList.RecipeNO= this.GetDrugNewNoteNo();  //������
                    //ItemList.SequenceNO=1; // ��������ˮ��
                    //ItemList.Item.MinFee.ID="A01";//��С���ô���
                    //ItemList.Item.MinFee.ID="A01";
                    //ItemList.Item.ID="FA01";      //��Ŀ����
                    //ItemList.Item.Name="�Է�ҩ����"; //��Ŀ����

                    //����Ҫע�⣬������3�������
                    //ItemList.TransType="3";  //��������-3Ϊ����

                    ((PatientInfo)ItemList.Patient).PVisit.PatientLocation.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;//��Ժ����
                    //ItemList.NurseStation.ID=pInfo.PVisit.PatientLocation.NurseCell.ID; //��ʿվ
                    ItemList.ExecOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.StockOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.RecipeOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.RecipeOper.ID = pInfo.PVisit.AdmittingDoctor.ID; //ҽ��
                    //ItemList.Item.Price=pInfo.FT.OvertopCost;
                    ItemList.Item.Qty = 1;//����
                    //ItemList.NoBackQty=0;
                    ItemList.Item.PriceUnit = "��";
                    //ItemList.FT.TotCost=pInfo.FT.OvertopCost;
                    //ItemList.FT.OwnCost=pInfo.FT.OvertopCost;
                    //ItemList.Item.IsPharmacy = true;
                    ItemList.Item.ItemType = EnumItemType.Drug;
                    ItemList.PayType = PayTypes.Balanced;
                    ItemList.IsBaby = false;
                    ItemList.BalanceNO = 0;
                    ItemList.BalanceState = "0";
                    ItemList.NoBackQty = 1;
                    ItemList.ChargeOper.ID = this.Operator.ID;
                    ItemList.ChargeOper.OperTime = dtNow; //����ʱ��
                    ItemList.FeeOper.ID = this.Operator.ID;
                    ItemList.FeeOper.OperTime = dtNow;
                    ItemList.ChargeOper.OperTime = dtNow;
                    ItemList.Item.PackQty = 1;

                    //��ҩ
                    if (WShouldCost > 0)
                    {
                        ItemList.FT.OwnCost = WShouldCost;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(WShouldCost * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(WShouldCost + ItemList.FT.PayCost);

                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//
                        ItemList.Item.ID = "Y001";
                        ItemList.Item.Name = "��ҩ��(���ѵ��Է�)";
                        //��С����
                        ItemList.Item.MinFee.ID = "001";
                        ItemList.Item.MinFee.ID = "001";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}
                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }

                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }

                    }
                    //��ҩ
                    if (PShouldCost > 0)
                    {
                        ItemList.FT.OwnCost = PShouldCost;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(PShouldCost * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(PShouldCost + ItemList.FT.PayCost);

                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//PShouldCost;//
                        ItemList.Item.ID = "Y002";
                        ItemList.Item.Name = "��ҩ��(���ѵ��Է�)";
                        //��С����
                        ItemList.Item.MinFee.ID = "002";
                        ItemList.Item.MinFee.ID = "002";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}

                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }


                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }

                    }
                    //��ҩ
                    if (CShouldCost > 0)
                    {
                        ItemList.FT.OwnCost = CShouldCost;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(CShouldCost * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(CShouldCost + ItemList.FT.PayCost);
                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//CShouldCost;  //
                        ItemList.Item.ID = "Y003";
                        ItemList.Item.Name = "��ҩ��(���ѵ��Է�)";
                        //��С����
                        ItemList.Item.MinFee.ID = "003";
                        ItemList.Item.MinFee.ID = "003";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}

                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }


                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }
                    }
                    return 0;
                    #endregion
                }
                //���������С��0���������Է�ҩ����Ҫ���ԷѲ��ֵ���Ϊ���Ѳ���
                if (pInfo.FT.OvertopCost < 0)
                {
                    #region ���Է�תΪ����
                    //�鿴�Ƿ�����Ҫ�������Է�ҩ
                    ArrayList al;
                    al = this.QueryFeeInfosGroupByMinFeeForAdjustByInpatientNO(pInfo.ID);
                    if (al == null)
                    {
                        return -1;
                    }
                    if (al.Count == 0)
                    {
                        return 0;
                    }
                    //�޶�ʣ��
                    decimal overTop = pInfo.FT.OvertopCost;
                    //��ҩ���ԷѲ���
                    decimal WOwnCost = 0m;
                    //��ҩ���ԷѲ���
                    decimal COwnCost = 0m;
                    //��ҩ���ԷѲ���
                    decimal POwnCost = 0m;
                    //��ҩ�ѵ�������
                    decimal WAdjust = 0m;
                    //��ҩ�ѵ�������
                    decimal PAdjust = 0m;
                    //��ҩ�ѵ�������
                    decimal CAdjust = 0m;
                    //ѭ���õ�������ҩ�Ѹ�����ֵ
                    for (int i = 0; i < al.Count; i++)
                    {
                        Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo fInfo;
                        fInfo = (Neusoft.HISFC.Models.Fee.Inpatient.FeeInfo)al[i];
                        if (fInfo.Item.MinFee.ID == "001") WOwnCost = fInfo.FT.OwnCost;
                        if (fInfo.Item.MinFee.ID == "002") POwnCost = fInfo.FT.OwnCost;
                        if (fInfo.Item.MinFee.ID == "003") COwnCost = fInfo.FT.OwnCost;
                    }
                    //������Ϊ0������
                    if (WOwnCost == 0 && POwnCost == 0 && COwnCost == 0) return 0;
                    //������������
                    if (WOwnCost > 0)
                    {
                        if (WOwnCost + overTop >= 0)
                        {
                            WAdjust = overTop;
                            overTop = 0;
                        }
                        else
                        {
                            WAdjust = -WOwnCost;
                            overTop = overTop + WOwnCost;
                        }
                    }
                    if (POwnCost > 0 && overTop < 0)
                    {
                        if (POwnCost + overTop >= 0)
                        {
                            PAdjust = overTop;
                            overTop = 0;
                        }
                        else
                        {
                            PAdjust = -POwnCost;
                            overTop = overTop + POwnCost;
                        }
                    }
                    if (COwnCost > 0 && overTop < 0)
                    {
                        if (COwnCost + overTop >= 0)
                        {
                            CAdjust = overTop;
                            overTop = 0;
                        }
                        else
                        {
                            CAdjust = -COwnCost;
                        }
                    }
                    //ȡ��ǰʱ��
                    DateTime dtNow = this.GetDateTimeFromSysDateTime();

                    //ȡ���߷��ñ���					
                    Neusoft.HISFC.Models.Base.PactInfo PactUnitInfo = new Neusoft.HISFC.Models.Base.PactInfo();
                    Neusoft.HISFC.BizLogic.Fee.PactUnitInfo PactManagment = new PactUnitInfo();
                    //PactManagment.SetTrans(this.command.Transaction);
                    PactUnitInfo = PactManagment.GetPactUnitInfoByPactCode(pInfo.Pact.ID);


                    // �����Է�ҩ����
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
                    Neusoft.HISFC.Models.Pharmacy.Item Drug = new Neusoft.HISFC.Models.Pharmacy.Item();
                    ItemList.Item = Drug;
                    // ��ֵ
                    //ItemList.RecipeNO= this.GetDrugNewNoteNo();  //������
                    //ItemList.SequenceNO=1; // ��������ˮ��
                    //ItemList.Item.MinFee.ID="A01";//��С���ô���
                    //ItemList.Item.MinFee.ID="A01";
                    //ItemList.Item.ID="FA01";      //��Ŀ����
                    //ItemList.Item.Name="�Է�ҩ����"; //��Ŀ����


                    //����ע����Ҫ�ٴ��޸ĵģ���������ֶα���ǹ��ѱ�������
                    //ItemList.TransType=;  //��������-3Ϊ����



                    ((PatientInfo)ItemList.Patient).PVisit.PatientLocation.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;//��Ժ����
                    //ItemList.NurseStation.ID=pInfo.PVisit.PatientLocation.NurseCell.ID; //��ʿվ
                    ItemList.ExecOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.StockOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.RecipeOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
                    ItemList.RecipeOper.ID = pInfo.PVisit.AdmittingDoctor.ID; //ҽ��
                    //ItemList.Item.Price=pInfo.FT.OvertopCost;
                    ItemList.Item.Qty = 1;//����
                    //ItemList.NoBackQty=0;
                    ItemList.Item.PriceUnit = "��";
                    //ItemList.FT.TotCost=pInfo.FT.OvertopCost;
                    //ItemList.FT.OwnCost=pInfo.FT.OvertopCost;
                    //ItemList.Item.IsPharmacy = true;
                    ItemList.Item.ItemType = EnumItemType.Drug;
                    ItemList.PayType = PayTypes.Balanced;
                    ItemList.IsBaby = false;

                    ItemList.BalanceNO = 0;
                    ItemList.BalanceState = "0";
                    ItemList.NoBackQty = 1;
                    ItemList.ChargeOper.ID = this.Operator.ID;
                    ItemList.ChargeOper.OperTime = dtNow; //����ʱ��
                    ItemList.FeeOper.ID = this.Operator.ID;
                    ItemList.FeeOper.OperTime = dtNow;
                    ItemList.ChargeOper.OperTime = dtNow;
                    ItemList.Item.PackQty = 1;

                    if (WAdjust < 0)
                    {//������ҩ��
                        ItemList.FT.OwnCost = WAdjust;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(WAdjust * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(WAdjust + ItemList.FT.PayCost);

                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//-WAdjust; //
                        ItemList.Item.ID = "Y001";
                        ItemList.Item.Name = "��ҩ��(�Էѵ�����)";
                        //��С����
                        ItemList.Item.MinFee.ID = "001";
                        ItemList.Item.MinFee.ID = "001";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}

                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }


                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }
                    }
                    if (PAdjust < 0)
                    {//������ҩ��
                        ItemList.FT.OwnCost = PAdjust;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(PAdjust * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(PAdjust + ItemList.FT.PayCost);

                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//-PAdjust;//
                        ItemList.Item.ID = "Y002";
                        ItemList.Item.Name = "��ҩ��(�Էѵ�����)";
                        //��С����
                        ItemList.Item.MinFee.ID = "002";
                        ItemList.Item.MinFee.ID = "002";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}

                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }

                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }
                    }
                    if (CAdjust < 0)
                    {//������ҩ��
                        ItemList.FT.OwnCost = CAdjust;
                        ItemList.FT.PayCost = -Neusoft.FrameWork.Public.String.FormatNumber(CAdjust * PactUnitInfo.Rate.PayRate, 2);
                        ItemList.FT.TotCost = 0;
                        ItemList.FT.PubCost = -(CAdjust + ItemList.FT.PayCost);
                        //ItemList.Item.Qty=-ItemList.Item.Qty;
                        ItemList.Item.Price = 0;//-CAdjust;  //
                        ItemList.Item.ID = "Y003";
                        ItemList.Item.Name = "��ҩ��(�Էѵ�����)";
                        //��С����
                        ItemList.Item.MinFee.ID = "003";
                        ItemList.Item.MinFee.ID = "003";
                        ItemList.RecipeNO = GetDrugRecipeNO();
                        ItemList.SequenceNO = 1;

                        //������ϸ��
                        if (this.InsertMedItemList(pInfo, ItemList) == -1)
                        {

                            return -1;

                        }
                        //���û��ܱ�
                        //						if(this.UpdateAccount(pInfo,ItemList)==-1)
                        //						{
                        //							
                        //							return -1 ;
                        //						}

                        parm = this.UpdateInMainInfoFee(pInfo.ID, ItemList.FT);
                        if (parm == -1)
                        {
                            this.Err = "����סԺ����ʧ��!";
                            return -1;
                        }
                        if (parm == 0)
                        {
                            this.Err = "�����ѽ�����ߴ��ڷ���״̬�������շѣ�����סԺ����ϵ!";
                            return -1;
                        }

                        //���¹������޶��ۼƺͳ�����
                        if (this.UpdateLimitOverTop(pInfo.ID, -ItemList.FT.OwnCost) < 1)
                        {
                            return -1;
                        }
                    }

                    #endregion
                }
            }
            return 0;
        }
        /// <summary>
        /// ���¸����ϴι̶����üƷ�ʱ��
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <param name="PreFixFeeDateTime"></param>
        /// <returns></returns>
        public int UpdateFixFeeDateByPerson(string InpatientNo, string PreFixFeeDateTime)
        {
            string strSql = "";
            if (this.Sql.GetSql("FixFee.SetPatientPreFixFeeDate", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, InpatientNo, PreFixFeeDateTime);
            }
            catch (Exception e)
            {
                this.Err = "�����ϴι̶����üƷ�ʱ��FixFee.SetUpdatePreFixFeeDateTime!" + e.Message;
                WriteErr();
                return -1;
            }

            if (this.ExecNoQuery(strSql) == -1) return -1;
            return 0;
        }
        #region ������ϸ�������ݸ����������
        /// <summary>
        /// �ӻ�����ϸ�в�ѯ���߷���
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Base.FT SumCostFromDetail(string InpatientNo)
        {
            //�����ַ��� �洢SQL���
            string strSql = "";
            string strReturn = "";

            //��ȡSQL���
            if (this.Sql.GetSql("Fee.Report.SumCostFromDetail", ref strSql) == -1)
            {
                this.Err = "û���ҵ� Fee.Report.SumCostFromDetail �ֶ�!";
                this.ErrCode = "-1";
                return null;
            }
            //��ʽ���ַ���
            strSql = string.Format(strSql, InpatientNo, "1",
                "1", "1", "1", "1", "1", "1", "1", "1");

            if (this.ExecEvent(strSql, ref strReturn) == -1)
            {
                this.Err = "ִ�д洢���̳���!Fee.Report.SumCostFromDetail";
                this.ErrCode = "Fee.Report.SumCostFromDetail";
                this.WriteErr();
                return null;
            }

            string[] s = strReturn.Split(',');
            Neusoft.HISFC.Models.Base.FT obj = new Neusoft.HISFC.Models.Base.FT();

            try
            {
                obj.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[0]), 2);
                obj.OwnCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[1]), 2);
                obj.PubCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[2]), 2);
                obj.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[3]), 2);
                obj.LeftCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[4]), 2);
                obj.DrugFeeTotCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[5]), 2);
                obj.OvertopCost = Neusoft.FrameWork.Public.String.FormatNumber(NConvert.ToDecimal(s[6]), 2);
            }
            catch (Exception ex)
            {
                this.ErrCode = "-1";
                this.Err += ex.Message;
                return null;
            }
            return obj;
        }
        /// <summary>
        /// ���ݻ��߷�����ϸ�����»�������������
        /// </summary>
        /// <param name="InpatientNo">סԺ��ˮ��</param>
        /// <returns></returns>
        public int UpdateInMainInfoCost(string InpatientNo)
        {
            Neusoft.HISFC.Models.Base.FT ft = this.SumCostFromDetail(InpatientNo);
            if (ft == null) return -1;
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.UpdateInMainInfoCost", ref strSql) == -1)
            {
                this.Err = "�Ҳ���SQL��� Fee.Inpatient.UpdateInMainInfoCost";
                return -1;
            }
            try
            {
                strSql = string.Format(strSql, InpatientNo,
                    ft.TotCost.ToString(),
                    ft.OwnCost.ToString(),
                    ft.PayCost.ToString(),
                    ft.PubCost.ToString(),
                    ft.LeftCost.ToString(),
                    ft.OvertopCost.ToString(),
                    ft.DrugFeeTotCost.ToString());
                return this.ExecNoQuery(strSql);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        public int UpdatePatientsCost(ArrayList al)
        {
            if (al == null) return 0;
            foreach (Neusoft.HISFC.Models.RADT.PatientInfo info in al)
            {
                if (this.UpdateInMainInfoCost(info.ID) == -1)
                    return -1;
            }
            return 1;
        }

        /// <summary>
        /// ����ҽ�����߻�����Ϣ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateSiPatientInfo(Neusoft.HISFC.Models.RADT.PatientInfo info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Fee.Inpatient.UpdateSiPatientInfo", ref strSql) == -1)
            {
                this.Err = "Can't Find Sql:Fee.Inpatient.UpdateSiPatientInfo";
                return -1;
            }
            strSql = System.String.Format(strSql, info.SSN, info.ID);
            return this.ExecNoQuery(strSql);
        }
        #endregion

        #region �໤��---
        static ArrayList _IcuBedList = null;
        /// <summary>
        /// �໤���б�
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static ArrayList IcuBedList(System.Data.OracleClient.OracleTransaction t)
        {

            //�������Ϊnull,���ʾ��һ�μ���,�����ݿ�����ȡ
            if (_IcuBedList == null)
            {
                _IcuBedList = new ArrayList();
                Neusoft.HISFC.BizLogic.Manager.Constant cons = new Neusoft.HISFC.BizLogic.Manager.Constant();
                cons.SetTrans(t);
                _IcuBedList = cons.GetList("FIN_ICUBED");
                if (_IcuBedList == null)
                    return null;
                else
                    return _IcuBedList;
            }
            else
            {
                return _IcuBedList;
            }

        }
        /// <summary>
        /// ����Ŀ�Ƿ��Ǽ໤��
        /// </summary>
        /// <param name="BedID"></param>
        /// <returns></returns>
        public bool IsIcuBedList(string BedID)
        {
            ArrayList al = null;//IcuBedList(this.command.Transaction);
            if (al == null) return false;
            foreach (Neusoft.HISFC.Models.Base.Const cons in al)
            {
                if (cons.ID == BedID)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        //{1E64A9A8-F0CC-449d-B16C-1C8B6D226839}
        #region  ��������
        /// <summary>
        /// ��ѯ��������
        /// </summary>
        /// <param name="patientNO">סԺ��ˮ��</param>
        /// <param name="nurseCode">����վ����</param>
        /// <returns></returns>
        public ArrayList QueryPatientFeeGroup(string patientNO, string nurseCode)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Inpatient.FeeGroup.Select", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.Inpatient.FeeGroup.Select��SQL���ʧ�ܣ�";
                return null;
            }
            sql = string.Format(sql, patientNO, nurseCode);
            return GetPatientFeeGroupBySql(sql);
        }

        /// <summary>
        /// ����SQL��ȡ����������Ϣ
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private ArrayList GetPatientFeeGroupBySql(string sql)
        {
            if (this.ExecQuery(sql) == -1)
            {
                this.Err = "��ѯ������������ʧ�ܣ�";
                return null;
            }
            ArrayList al = new ArrayList();
            FeeGroup feeGroup = null;
            while (this.Reader.Read())
            {
                feeGroup = new FeeGroup();
                feeGroup.ID = this.Reader[0].ToString();
                feeGroup.Patient.ID = this.Reader[1].ToString();
                feeGroup.NurseCell.ID = this.Reader[2].ToString();
                feeGroup.Item.ID = this.Reader[3].ToString();
                feeGroup.Item.Name  = this.Reader[4].ToString();
                feeGroup.DrugFlag = this.Reader[5].ToString();
                feeGroup.Item.Qty = NConvert.ToDecimal(this.Reader[6].ToString());
                feeGroup.Days = NConvert.ToDecimal(this.Reader[7]);
                feeGroup.Item.PriceUnit = this.Reader[8].ToString();
                feeGroup.FeeDate = NConvert.ToDateTime(this.Reader[9]);
                feeGroup.ExecDept.ID = this.Reader[10].ToString();
                feeGroup.Package.ID = this.Reader[11].ToString();
                feeGroup.Package.Name = this.Reader[12].ToString();
                feeGroup.Oper.ID = this.Reader[13].ToString();
                feeGroup.Oper.OperTime = NConvert.ToDateTime(this.Reader[14]);
                al.Add(feeGroup);
            }
            return al;
        }

        /// <summary>
        /// ��÷���������ˮ��
        /// </summary>
        /// <returns></returns>
        public string GetFeeGroupID()
        {
            return this.GetSequence("Fee.Inpatient.FeeGroup.ID");
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="feeGroup"></param>
        /// <returns></returns>
        public int InsertFeeGroup(FeeGroup feeGroup)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Inpatient.FeeGroup.Insert", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.Inpatient.FeeGroup.Insert��SQL���ʧ�ܣ�";
                return -1;
            }

            sql = string.Format(sql,feeGroup.ID,feeGroup.Patient.ID,feeGroup.NurseCell.ID,feeGroup.Item.ID,
                                    feeGroup.Item.Name,feeGroup.DrugFlag,feeGroup.Item.Qty.ToString(),feeGroup.Days.ToString(),
                                    feeGroup.Item.PriceUnit,feeGroup.FeeDate.ToString(),feeGroup.ExecDept.ID,feeGroup.Package.ID,
                                    feeGroup.Package.Name,feeGroup.Oper.ID,feeGroup.Oper.OperTime.ToString());
            return this.ExecNoQuery(sql);
        }

        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="nurcellCode"></param>
        /// <returns></returns>
        public int DeleteFeeGroup(string inpatientNO, string nurcellCode)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Inpatient.FeeGroup.Delete", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.Inpatient.FeeGroup.Delete��SQL���ʧ�ܣ�";
                return -1;
            }
            sql = string.Format(sql, inpatientNO, nurcellCode);
            return this.ExecNoQuery(sql);
        }

        /// <summary>
        /// ɾ����������{D6A25CA7-331A-4034-BBC6-A6FF821E290C}
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="nurcellCode"></param>
        /// <returns></returns>
        public int DeleteFeeGroup(string groupID)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Inpatient.FeeGroup.DeleteByGroupID", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.Inpatient.FeeGroup.Delete��SQL���ʧ�ܣ�";
                return -1;
            }
            sql = string.Format(sql, groupID);
            return this.ExecNoQuery(sql);
        }

        /// <summary>
        /// ���·��������ѷ�ʱ��
        /// </summary>
        /// <param name="feeGroupId">����������Ŀ����</param>
        /// <param name="feeDate">�շ�ʱ��</param>
        /// <returns></returns>
        public int UpdateFeeGroupFeeDate(string feeGroupId, DateTime feeDate)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Inpatient.FeeGroup.UpdateFeeDate", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.Inpatient.FeeGroup.Insert��SQL���ʧ�ܣ�";
                return -1;
            }
            sql = string.Format(sql, feeGroupId, feeDate.ToString());
            return this.ExecNoQuery(sql);
        }

        #endregion

        #region ֣������{D6A25CA7-331A-4034-BBC6-A6FF821E290C}
        /// <summary>
        /// ��ѯ���շ���
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="fromdate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet QueryDayFeeByInpaientNO(string inpatientNO,string fromdate,string toDate,string deptCode,bool isLong)
        {
            string strSql = string.Empty;

           int returnValue = this.Sql.GetSql("Fee.QueryPaientDayFee.1", ref strSql);
           if (returnValue < 0)
           {
               this.Err = "��ѯFee.QueryPaientDayFee.1��Ӧ��sql���ʧ��";
               return null;

           }
           try
           {
               strSql = string.Format(strSql, inpatientNO, fromdate, toDate,deptCode,Neusoft.FrameWork.Function.NConvert.ToInt32(isLong));
           }
           catch (Exception)
           {

               this.Err = "��ʽ��sql���ʧ��";
           }
            DataSet ds = new DataSet ();
            returnValue = this.ExecQuery(strSql, ref ds);
            if (returnValue <0)
            {
                return null;

            }

            return ds;
        }

        #region MyRegion
        public int QueryMedicinlistForQuery(string inpatientNO,string fromDate,string toDate,ref DataSet ds)
        {
             ds = new DataSet();

            string strSql = @"select drug_name ҩƷ����,
                               a.specs ���,
                               a.unit_price ����,
                               a.qty ����,
                               a.days ����,
                               a.current_unit ��λ,
                               a.tot_cost ���,
                               a.own_cost �Է�,
                               a.pub_cost ����,
                               a.pay_cost �Ը�,
                               a.eco_cost �Ż�,
                               nvl((select b.dept_name
                                     from com_department b
                                    where b.dept_code = a.execute_deptcode),
                                   a.execute_deptcode) ִ�п���,
                               nvl((select b.dept_name
                                     from com_department b
                                    where b.dept_code = a.inhos_deptcode),
                                   a.inhos_deptcode) ���߿���,
                               
                               a.fee_date �շ�ʱ��,
                               nvl((select c.empl_name
                                     from com_employee c
                                    where c.empl_code = a.fee_opercode),
                                   a.fee_opercode),
                               a.senddrug_date,
                               nvl((select c.empl_name
                                     from com_employee c
                                    where c.empl_code = a.senddrug_opercode),
                                   a.senddrug_opercode)

                          from fin_ipb_medicinelist a

                         where inpatient_no = '{0}'
                           AND fee_date >= to_date('{1}', 'YYYY-MM-DD hh24:mi:ss')
                           and fee_date <= to_date('{2}', 'yyyy-mm-dd hh24:mi:ss')
                         order by fee_date
                        ";
            strSql = string.Format(strSql, inpatientNO, fromDate, toDate);

           return  this.ExecQuery(strSql, ref ds);
          
        }
        #endregion
        #endregion

        #region ֣����������ȡ�ն�ȷ��״̬ {B98851B0-9C5A-4d68-ABB5-CB48C4DBD34B}
        /// <summary>
        /// ��ȡ�ն�ȷ��״̬
        /// </summary>
        /// <param name="execsql">ִ�е���ˮ��</param>
        /// <returns>false:û��ȷ�Ϲ� true���Ѿ�ȷ��</returns>
        public bool GetTecFlag(string execsql)
        {
            bool ret = false;
            string sql = string.Empty;
            sql = @" select count(c.current_sequence)
                       from met_tec_inpatientconfirm c
                      where c.exec_sqn = '{0}'";
            try
            {
                sql = string.Format(sql, execsql);
            }
            catch (Exception ex)
            {

                this.Err = "��ȡȷ��״̬����" + ex.ToString();
            }
            DataSet ds = new DataSet();
            this.ExecQuery(sql, ref ds);
            string val = string.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                val = item.ItemArray.GetValue(0).ToString();
            }
            if ("0" != val)
            {
                ret = true;
            }
            return ret;
        }
        #endregion

    }
}
