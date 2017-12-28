using System;
using System.Collections;
//{55BBD9DB-F5C9-4e0a-94E5-9F7FCB121350}
using System.Collections.Generic;
namespace Neusoft.HISFC.BizLogic.Order.OutPatient
{
	/// <summary>
	/// Order ��ժҪ˵����
	/// ����ҽ��
	/// </summary>
	public class Order:Neusoft.FrameWork.Management.Database
	{
		public Order()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		

		#region ������������ɾ��

		/// <summary>
		/// ����һ��
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public int InsertOrder(Neusoft.HISFC.Models.Order.OutPatient.Order order)
		{
			string sql = "Order.OutPatient.Order.Insert";
			if (this.Sql.GetSql(sql,ref sql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			sql = this.myGetSql( sql,order );
			if(sql == null) return -1;
			if(this.ExecNoQuery(sql) <= 0) return -1;
			return 0;
		}

		
		/// <summary>
		/// ����
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		public int UpdateOrder(Neusoft.HISFC.Models.Order.OutPatient.Order order)
		{
            if (this.DeleteOrder(order.SeeNO, Neusoft.FrameWork.Function.NConvert.ToInt32(order.ID)) < 0) return -1;//ɾ�����ɹ�
			return this.InsertOrder(order);
		}
		
		
		/// <summary>
		/// ɾ��
		/// </summary>
		/// <param name="seeNo"></param>
		/// <param name="seqNo"></param>
		/// <returns></returns>
		public int DeleteOrder(string seeNo,int seqNo)
		{
			string sql = "Order.OutPatient.Order.Delete";
			if(this.Sql.GetSql(sql, ref sql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				sql = string.Format(sql,seeNo,seqNo);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(sql);
		}
		#endregion

        #region ����ҽ����������add by sunm

        public int InsertOrderChangeInfo(Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            string sql = "Order.OutPatient.Order.InsertChangeInfo";
            if (this.Sql.GetSql(sql, ref sql) == -1) return -1;
            sql = this.myGetSql(sql, order);
            if (sql == null) return -1;
            if (this.ExecNoQuery(sql) <= 0) return -1;
            return 0;
        }
        /// <summary>
        /// ����ҽ�������¼
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int UpdateOrderChangedInfo(Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            string sql = "Order.OutPatient.Order.UpdateChangeInfo";
            if (this.Sql.GetSql(sql, ref sql) == -1) return -1;
            sql = System.String.Format(sql, order.DCOper.ID, order.SeeNO, order.SequenceNO);
            if (sql == null) return -1;
            if (this.ExecNoQuery(sql) <= 0) return -1;
            return 0;
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int UpdateOrderBeCaceled(Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            string sql = "Order.OutPatient.Order.CancelOrder";
            if (this.Sql.GetSql(sql, ref sql) == -1)
            {
                this.Err = "Can't Find Sql:Order.OutPatient.Order.CancelOrder";
                return -1;
            }
            sql = System.String.Format(sql, order.ID);
            if (sql == null) return -1;
            return this.ExecNoQuery(sql);
        }

        #endregion

        #region ����µĿ������
        /// <summary>
		/// 
		/// </summary>
		/// <param name="cardNo"></param>
		/// <returns></returns>
		public int GetNewSeeNo( string cardNo )
		{
			string sql = "Order.OutPatient.Order.GetNewSeeNo.1";
			if(this.Sql.GetSql(sql,ref sql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				sql = string.Format(sql,cardNo);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.WriteErr();
				return -1;
			}
			return Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(sql));
		}
		#endregion

        /// <summary>
        /// �����ҽ��������
        /// </summary>
        /// <returns></returns>
        public string GetNewOrderComboID()
        {
            string sql = "";
            if (this.Sql.GetSql("Management.Order.GetComboID", ref sql) == -1) return null;
            string strReturn = this.ExecSqlReturnOne(sql);
            if (strReturn == "-1" || strReturn == "") return null;
            return strReturn;
        }

		#region ����ҽ���Ѿ��շ�
		/// <summary>
		/// ����ҽ���Ѿ��շ�
		/// </summary>
		/// <param name="orderID"></param>
		/// <returns></returns>
		public int UpdateOrderCharged(string orderID)
		{
			string sql = "Order.OutPatient.Order.Update.UpdateOrderCharged.2";
			if(this.Sql.GetSql(sql,ref sql) == -1) return -1;
			return this.ExecNoQuery(sql,orderID);
		}
		/// <summary>
		/// ����ҽ���Ѿ��շ�
		/// </summary>
		/// <param name="reciptNo"></param>
		/// <param name="seqNo"></param>
		/// <returns></returns>
		public int UpdateOrderCharged(string reciptNo,string seqNo)
		{
			string sql = "Order.OutPatient.Order.Update.UpdateOrderCharged.1";
			if(this.Sql.GetSql(sql, ref sql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			return this.ExecNoQuery(sql,reciptNo,seqNo,this.Operator.ID);
		}
        /// <summary>
        /// ����ҽ���Ѿ��շ�
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="chargeOperID"></param>
        /// <returns></returns>
        public int UpdateOrderChargedByOrderID(string orderID,string chargeOperID)
        {
            string sql = "Order.OutPatient.Order.Update.UpdateOrderCharged.4";
            if (this.Sql.GetSql(sql, ref sql) == -1) return -1;
            return this.ExecNoQuery(sql, orderID, chargeOperID);
        }
		#endregion

        #region  ����ҽ�����
        /// <summary>
        /// ����ҽ�����
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="sortID"></param>
        /// <returns></returns>
        public int UpdateOrderSortID(string orderID,int sortID)
        {
            string sql = "Order.OutPatient.Order.Update.UpdateOrderSortID.1";
            if(this.Sql.GetSql(sql, ref sql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				sql = string.Format(sql,orderID,sortID);
			}
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(sql);
        }
        #endregion

        #region  ����ҽ��Ƥ�Խ��
        /// <summary>
        /// ����ҽ��Ƥ�Խ��//{26E88889-B2CF-4965-AFD8-6D9BE4519EBF}
        /// </summary>
        /// <param name="sequenceNO"></param>
        /// <returns></returns>
        public int UpdateOrderHyTest(string hytestValue, string sequenceNO)
        {
            string sql = "Order.OutPatient.Order.UpdateHyTest.1";
            if (this.Sql.GetSql(sql, ref sql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            try
            {
                sql = string.Format(sql, hytestValue, sequenceNO);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(sql);
        }
        /// <summary>
        /// ����ҽ��Ƥ�Խ��{55BBD9DB-F5C9-4e0a-94E5-9F7FCB121350}
        /// </summary>
        /// <param name="sequenceNO"></param>
        /// <returns></returns>
        public int UpdateOrderHyTest(string hytestValue,string hytestName, string sequenceNO, string seeNO)
        {
            string sql = "Order.OutPatient.Order.UpdateHyTest.2";
            if (this.Sql.GetSql(sql, ref sql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            try
            {
                sql = string.Format(sql, hytestValue, hytestName,sequenceNO, seeNO);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(sql);
        }
        //{55BBD9DB-F5C9-4e0a-94E5-9F7FCB121350}
        public List<Neusoft.FrameWork.Models.NeuObject> QueryHytoRecord(string cardNO, string beginDtime, string endDtime)
        {
            string strSql = string.Empty;

            int returnValue = this.Sql.GetSql("Order.OutPatient.Order.QueryHyRecord", ref strSql);

            if (returnValue < 0)
            {
                this.Err = "��ѯ��Ӧ[Order.OutPatient.Order.QueryHyRecord]��sql���ʧ��";
                return null;
            }

            try
            {
                strSql = string.Format(strSql, cardNO, beginDtime, endDtime);
            }
            catch (Exception ex)
            {

                this.Err = "��ʽ������\n" + ex.Message;
                return null;
            }

            if (this.ExecQuery(strSql) < 0)
            {
                return null;
            }
            List<Neusoft.FrameWork.Models.NeuObject> orderList = new List<Neusoft.FrameWork.Models.NeuObject>();
            while (this.Reader.Read())
            {

                Neusoft.FrameWork.Models.NeuObject order = new Neusoft.FrameWork.Models.NeuObject();
                order.ID = this.Reader[0].ToString();
                order.Name = this.Reader[1].ToString();
                order.Memo = this.Reader[2].ToString();
                orderList.Add(order);
            }

            this.Reader.Close();

            return orderList;


        }

        // ���ݲ����ţ�������ˮ�ţ���ѯ��Ҫ��Ƥ�Ե���Чҽ��
       /// <summary>
        /// ���ݲ����ţ�������ˮ�ţ���ѯ��Ҫ��Ƥ�Ե���Чҽ��{55BBD9DB-F5C9-4e0a-94E5-9F7FCB121350}
       /// </summary>
       /// <param name="cardNO"></param>
       /// <param name="clinicNO"></param>
       /// <returns></returns>
        public ArrayList QueryOrderByCardNOClinicNO(string cardNO,string clinicNO)
        {
            string sql = "", sqlSelect = "", sqlWhere = "Order.OutPatient.Order.Query.Where.5";
            if (this.myGetSelectSql(ref sqlSelect) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }
            if (this.Sql.GetSql(sqlWhere, ref sqlWhere) == -1) return null;
            sql = sqlSelect + " " + sqlWhere;
            sql = string.Format(sql, cardNO,clinicNO);
            return this.myGetExecOrder(sql);
        }

        /// <summary>
        /// ����������ѯҽ��
        /// </summary>
        /// <param name="seeNO"></param>
        /// <param name="sqeNO"></param>
        /// <returns></returns>{55BBD9DB-F5C9-4e0a-94E5-9F7FCB121350}
        public ArrayList QueryOrderByKey(string seeNO,string sqeNO)
        {
            string sql = "", sqlSelect = "", sqlWhere = "Order.OutPatient.Order.Query.Where.6";
            if (this.myGetSelectSql(ref sqlSelect) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }
            if (this.Sql.GetSql(sqlWhere, ref sqlWhere) == -1) return null;
            sql = sqlSelect + " " + sqlWhere;
            sql = string.Format(sql, seeNO,sqeNO);
            return this.myGetExecOrder(sql);
        }
        #endregion

        #region ��ѯ

        /// <summary>
		/// ��ѯִ��ҽ��--ͨ��������Ų�ѯ
		/// </summary>
		/// <param name="seeNo"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string seeNo )
		{
			string sql ="",sqlSelect = "",sqlWhere = "Order.OutPatient.Order.Query.Where.1";
			if(this.myGetSelectSql(ref sqlSelect) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			if(this.Sql.GetSql(sqlWhere,ref sqlWhere) == -1) return null;
			sql = sqlSelect + " " + sqlWhere;
			sql = string.Format (sql,seeNo);
			return this.myGetExecOrder(sql);
			
		}

        /// <summary>
        /// ���ݴ����Ų�ѯҽ��
        /// </summary>
        /// <param name="recipeNO"></param>
        /// <returns></returns>
        public ArrayList QueryOrderByRecipeNO(string recipeNO)
        {
            string sql = "", sqlSelect = "", sqlWhere = "Order.OutPatient.Order.Query.Where.4";
            if (this.myGetSelectSql(ref sqlSelect) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }
            if (this.Sql.GetSql(sqlWhere, ref sqlWhere) == -1) return null;
            sql = sqlSelect + " " + sqlWhere;
            sql = string.Format(sql, recipeNO);
            return this.myGetExecOrder(sql);
        }

		/// <summary>
		/// ��ѯһ��ҽ��
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Order.OutPatient.Order QueryOneOrder(string id)
		{
			string sql ="",sqlSelect = "",sqlWhere = "Order.OutPatient.Order.Query.Where.2";
			if(this.myGetSelectSql(ref sqlSelect)==-1) return null;
			if(this.Sql.GetSql(sqlWhere,ref sqlWhere) == -1) 
			{
				this.Err = this.Sql.Err;
				return null;
			}
			sql = sqlSelect + " " + sqlWhere;
			sql = string.Format(sql,id);
			ArrayList al = this.myGetExecOrder(sql);
			if(al == null) return null;
			if(al.Count <= 0) return null;
			return al[0] as Neusoft.HISFC.Models.Order.OutPatient.Order;
		}
		/// <summary>
		/// ��ÿ�������б�
		/// </summary>
		/// <param name="cardNo">���￨��</param>
		/// <returns></returns>
		public ArrayList QuerySeeNoListByCardNo(string cardNo)
		{
			string sql = "Order.OutPatient.Order.GetSeeNoList";
			if(this.Sql.GetSql(sql,ref sql) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			try
			{
				sql = string.Format(sql,cardNo);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.WriteErr();
				return null;
			}
			if(this.ExecQuery(sql) == -1) return null;
			ArrayList al = new ArrayList();
			while(this.Reader.Read())
			{
				Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
				obj.ID = this.Reader[0].ToString();
				obj.Name = this.Reader[1].ToString();
				obj.Memo = this.Reader[2].ToString();
				try
				{
					obj.User01 = this.Reader[3].ToString();
					obj.User02  = this.Reader[4].ToString();
					obj.User03 = this.Reader[5].ToString();
				}
				catch{}
				al.Add(obj);
			}		
			this.Reader.Close();
			return al;
		}
		/// <summary>
		/// ��ÿ�������б�
		/// </summary>
		/// <param name="clinicNo"></param>
		/// <param name="cardNo"></param>
		/// <returns></returns>
		public ArrayList QuerySeeNoListByCardNo( string clinicNo, string cardNo )
		{
			string sql = "Order.OutPatient.Order.GetSeeNoList.2";
			if(this.Sql.GetSql(sql,ref sql) == -1) return null;
			try
			{
				sql = string.Format(sql,clinicNo,cardNo);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.WriteErr();
				return null;
			}
			if(this.ExecQuery(sql) == -1) return null;
			ArrayList al = new ArrayList();
			while(this.Reader.Read())
			{
				Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
				obj.ID = this.Reader[0].ToString();
				obj.Name = this.Reader[1].ToString();
				obj.Memo = this.Reader[2].ToString();
				try
				{
					obj.User01 = this.Reader[3].ToString();
					obj.User02  = this.Reader[4].ToString();
					obj.User03 = this.Reader[5].ToString();
				}
				catch{}
				al.Add(obj);
			}		
			this.Reader.Close();
			return al;
		}
		/// <summary>
		/// ��ѯ������Ÿ�������
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ArrayList QuerySeeNoListByName(string name)
		{
			string sql = "Order.OutPatient.Order.GetSeeNoList.Name";
			if(this.Sql.GetSql(sql,ref sql) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			try
			{
				sql = string.Format(sql,name);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.WriteErr();
				return null;
			}
			if(this.ExecQuery(sql) == -1) return null;
			ArrayList al = new ArrayList();
			while(this.Reader.Read())
			{
				Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
				obj.ID = this.Reader[0].ToString();
				obj.Name = this.Reader[1].ToString();
				obj.Memo = this.Reader[2].ToString();
				try
				{
					obj.User01 = this.Reader[3].ToString();
					obj.User02  = this.Reader[4].ToString();
					obj.User03 = this.Reader[5].ToString();
				}
				catch{}
				al.Add(obj);
			}		
			this.Reader.Close();
			return al;
		}

        /// <summary>
        /// ȡ��ҩƷ������ͨ������źͿ����
        /// </summary>
        /// <param name="clinicNo"></param>
        /// <param name="seeNo"></param>
        /// <returns></returns>
        public ArrayList GetPhaRecipeNoByClinicNoAndSeeNo(string clinicNo, string seeNo)
        {
            string sql = "Order.OutPatient.Order.GetPhaRecipeNoByClinicNoAndSeeNo";
            if (this.Sql.GetSql(sql, ref sql) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }
            try
            {
                sql = string.Format(sql, clinicNo, seeNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return null;
            }
            if (this.ExecQuery(sql) == -1) return null;
            ArrayList alRecipe = new ArrayList();
            while (this.Reader.Read())
            {
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                obj.ID = this.Reader[0].ToString();
                
                alRecipe.Add(obj);
            }
            this.Reader.Close();
            return alRecipe;
        }

		#endregion

        #region ���ﲡ��

        #region �����ķ���
        /// <summary>
        /// ���ݴ����ʵ����»��߲������ﲡ��
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="casehistory"></param>
        /// <returns></returns>
       
        //public int SetCaseHistory(Neusoft.HISFC.Models.Registration.Register reg, Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory)
        //{
        //    int iReturn = this.UpdateCaseHistory(reg, casehistory);
        //    if (iReturn == -1)
        //        return -1;
        //    else if (iReturn == 0)
        //        return this.InsertCaseHistory(reg, casehistory);
        //    else
        //        return 1;
        //}
        #endregion

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="casehistory"></param>
        /// <returns></returns>
        public int InsertCaseHistory(Neusoft.HISFC.Models.Registration.Register reg, Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.InsertCase", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return -1;
            }
            try
            {
                strSql = System.String.Format(strSql, 
                                              reg.ID, //������ˮ�ţ����滻
                                              reg.PID.CardNO, 
                                              reg.Name, //��������
                                              reg.Sex.Name, 
                                              reg.Age, 
                                              reg.DoctorInfo.Templet.Dept.ID, 
                                              reg.Pact.PayKind.Name,
                                              casehistory.CaseMain, 
                                              casehistory.CaseNow, 
                                              casehistory.CaseOld, 
                                              casehistory.CaseAllery, 
                                              casehistory.IsAllery == true ? "1" : "0",
                                              casehistory.IsInfect == true ? "1" : "0", 
                                              casehistory.CheckBody, 
                                              casehistory.CaseDiag, 
                                              casehistory.Memo,
                                              this.Operator.ID,casehistory.CaseOper.OperTime.ToString());
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="casehistory"></param>
        /// <returns></returns>
        public int UpdateCaseHistory(Neusoft.HISFC.Models.Registration.Register reg, Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory,string oldOperTime)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.UpdateCase", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return -1;
            }
            try
            {
                /*
                 UPDATE MET_CAS_HISTORY
                    SET    CASEMAIN = '{0}',--����
                           CASENOW = '{1}',--�ֲ�ʷ
                           CASEOLD = '{2}',--����ʷ
                           CASEALLERY = '{3}',--����ʷ
                           ALLERY_FLAG = '{4}',--�Ƿ����
                           INFECT_FLAG = '{5}',--�Ƿ�Ⱦ��
                           CHECKBODY = '{6}',--���� 
                           DIAGNOSE = '{7}',--���
                           MEMO = '{8}',--��ע
                           OPER_CODE = '{9}',--����Ա
                           OPER_DATE = to_date('{10}','YYYY-MM-DD hh24:Mi:SS')--��������
                    WHERE  CLINIC_CODE = '{11}'--������ˮ�� 
                           and oper_date=to_date('{12}','YYYY-MM-DD hh24:Mi:SS')--����ʱ
                 */
                strSql = System.String.Format(strSql, 
                                              casehistory.CaseMain, 
                                              casehistory.CaseNow, 
                                              casehistory.CaseOld, 
                                              casehistory.CaseAllery, 
                                              casehistory.IsAllery == true ? "1" : "0",
                                              casehistory.IsInfect == true ? "1" : "0", 
                                              casehistory.CheckBody, 
                                              casehistory.CaseDiag, 
                                              casehistory.Memo,
                                              this.Operator.ID,
                                              casehistory.CaseOper.OperTime.ToString(),//���β���ʱ��
                                              reg.ID,
                                              oldOperTime //��һ�εĲ���ʱ��
                                              ); //������ˮ�ţ����滻
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ����������ˮ�Ų�ѯһ�����ﲡ��
        /// </summary>
        /// <param name="clinicCode"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory QueryCaseHistoryByClinicCode(string clinicCode)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetCase", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }
            strSql = System.String.Format(strSql, clinicCode);
            ArrayList al = this.GetMyObject(strSql);
            if (al == null)
                return null;
            else if (al.Count == 0)
                return null;
            else
                return al[0] as Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory;
        }

        /// <summary>
        /// ����������ˮ�źͲ���ʱ���ѯһ�����ﲡ��
        /// </summary>
        /// <param name="clinicCode"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory QueryCaseHistoryByClinicCode(string clinicCode,string operTime)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetCase1", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return null;
            }
            strSql = System.String.Format(strSql, clinicCode,operTime);
            ArrayList al = this.GetMyObject(strSql);
            if (al == null)
                return null;
            else if (al.Count == 0)
                return null;
            else
                return al[0] as Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory;
        }

        /// <summary>
        /// ��������Ų�ѯ�������в���
        /// </summary>
        /// <param name="CardNO"></param>
        /// <returns></returns>
        public ArrayList QueryAllCaseHistory(string CardNO)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetAllCase", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return null;
            }
            strSql = System.String.Format(strSql, CardNO);
            return this.GetMyObjectByCardNO(strSql);
        }

        /// <summary>
        /// ͨ�������ȡ����������ʱ��
        /// </summary>
        /// <param name="ClinicCode"></param>
        /// <returns></returns>
        public DateTime QueryMaxOperTimeByClinicCode(string ClinicCode)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetMaxOperDateByClinicCode", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return System.DateTime.MinValue;
            }
            strSql = System.String.Format(strSql, ClinicCode);
            string strReturn = "";
            strReturn = this.ExecSqlReturnOne(strSql);
            if (strReturn != "" && strReturn != null)
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(strReturn);
            }
            else
            {
                return System.DateTime.MinValue;
            }
        }

        #region ˽�к���

        /// <summary>
        /// �õ�����ʵ��
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private ArrayList GetMyObjectByCardNO(string strSql)
        {
            ArrayList al = new ArrayList();
            if (this.ExecQuery(strSql) == -1) return null;
            while (this.Reader.Read())
            {
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                obj.ID = this.Reader[0].ToString();//��ˮ��
                obj.Name = this.Reader[1].ToString();//����
                if (!this.Reader.IsDBNull(2))
                    obj.Memo = this.Reader[2].ToString();
                //User01�ǲ���ʱ�� ·־�� 2007-5-9
                obj.User01 = this.Reader[3].ToString();
                al.Add(obj);
            }
            this.Reader.Close();
            return al;
        }

        /// <summary>
        /// �õ�����ʵ��
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private ArrayList GetMyObject(string strSql)
        {
            ArrayList al = new ArrayList();
            if (this.ExecQuery(strSql) == -1) return null;
            while (this.Reader.Read())
            {
                Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory = new Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory();
                casehistory.CaseMain = this.Reader.GetValue(0).ToString();//����
                casehistory.CaseNow = this.Reader.GetValue(1).ToString();//�ֲ�ʷ
                casehistory.CaseOld = this.Reader.GetValue(2).ToString();//����ʷ
                casehistory.CaseAllery = this.Reader.GetValue(3).ToString();//����ʷ
                casehistory.CheckBody = this.Reader.GetValue(4).ToString();//����
                casehistory.CaseDiag = this.Reader.GetValue(5).ToString();//���
                casehistory.Memo = this.Reader.GetValue(6).ToString();//��ע
                casehistory.Name = this.Reader.GetValue(7).ToString();//����
                casehistory.ID = this.Reader.GetValue(8).ToString();//������ˮ��
                if (!this.Reader.IsDBNull(9))
                    casehistory.IsAllery = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader.GetValue(9).ToString());//�Ƿ����
                if (!this.Reader.IsDBNull(10))
                    casehistory.IsInfect = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader.GetValue(10).ToString());//�Ƿ�Ⱦ��
                //����ʱ��
                casehistory.CaseOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader.GetValue(11));
                al.Add(casehistory);
            }
            this.Reader.Close();
            return al;
        }

        #endregion

        #endregion

        #region ���ﲡ��ģ��

        /// <summary>
        /// ��ȡ����ģ����ˮ��
        /// </summary>
        /// <returns></returns>
        public string GetModuleSeq()
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetModuleSeq", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return "";
            }
            if (this.ExecQuery(strSql) == -1)
            {
                this.Err = "ִ�д���";
                return "";
            }
            string ID = "";
            while (this.Reader.Read())
            {
                ID = this.Reader[0].ToString();
            }
            this.Reader.Close();
            ID = ID.PadLeft(10, '0');
            return ID;
        }

        /// <summary>
        /// ���ݴ����ʵ����»��߲������ﲡ��ģ��
        /// </summary>
        /// <param name="casehistory"></param>
        /// <returns></returns>
        public int SetCaseModule(Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory)
        {
            int i = this.UpdateCaseModule(casehistory);
            if (i == -1)
                return -1;
            else if (i == 0)
                return this.InsertCaseModule(casehistory);
            else
                return 1;
        }

        /// <summary>
        /// ����һ����¼
        /// </summary>
        /// <param name="casehistory"></param>
        /// <returns></returns>
        public int InsertCaseModule(Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.InsertModule", ref strSql) == -1)
            {
                this.Err = "û���ҵ�Order.OutPatient.Case.InsertModule�ֶ�";
                return -1;
            }
            try
            {
                strSql = System.String.Format(strSql, 
                                              casehistory.ID, 
                                              casehistory.Name,
                                              casehistory.DeptID, 
                                              casehistory.CaseMain,
                                              casehistory.CaseNow, 
                                              casehistory.CaseOld, 
                                              casehistory.CaseAllery, 
                                              casehistory.CheckBody, 
                                              casehistory.CaseDiag, 
                                              casehistory.Memo,
                                              casehistory.ModuleType, 
                                              casehistory.DoctID, 
                                              this.Operator.ID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���²���ģ��Type
        /// </summary>
        /// <param name="ModuleType">ģ������</param>
        /// <param name="Module_NO">ģ��ID</param>
        /// <returns></returns>
        public int UpdateCaseModuleType(string ModuleType,string Module_NO)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.UpdateModuleType", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            try
            {
                strSql = System.String.Format(strSql,
                                              ModuleType,Module_NO);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ����һ����¼
        /// </summary>
        /// <param name="casehistory"></param>
        /// <returns></returns>
        public int UpdateCaseModule(Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.UpdateModule", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return -1;
            }
            try
            {   
                strSql = System.String.Format(strSql, 
                                              casehistory.Name,
                                              casehistory.DeptID, 
                                              casehistory.ModuleType, 
                                              casehistory.CaseMain,
                                              casehistory.CaseNow, 
                                              casehistory.CaseOld, 
                                              casehistory.CaseAllery, 
                                              casehistory.CheckBody, 
                                              casehistory.CaseDiag, 
                                              casehistory.Memo,
                                              casehistory.DoctID,
                                              this.Operator.ID, 
                                              casehistory.ID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ɾ��һ����¼
        /// </summary>
        /// <param name="moduleNo"></param>
        /// <returns></returns>
        public int DeleteCaseModule(string moduleNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.DelModule", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return -1;
            }
            try
            {
                strSql = System.String.Format(strSql, moduleNo);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ����ģ����ˮ�Ų�ѯһ����¼
        /// </summary>
        /// <param name="moduleNO"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory QueryCaseModule(string moduleNO)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.OutPatient.Case.GetModule", ref strSql) == -1)
            {
                this.Err = this.Sql.Err; 
                return null;
            }
            strSql = System.String.Format(strSql, moduleNO);
            ArrayList al = this.GetMyModule(strSql);
            if (al == null)
                return null;
            else if (al.Count == 0)
                return new Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory();
            else
                return al[0] as Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory;
        }

        /// <summary>
        /// �������������ģ��
        /// </summary>
        /// <param name="moduletype"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public ArrayList QueryAllCaseModule(string moduletype, string Code)
        {
            string strSql = "";
            if (moduletype == "1")//����
            {
                if (this.Sql.GetSql("Order.OutPatient.Case.GetAllModuleByDeptCode", ref strSql) == -1)
                {
                    this.Err = this.Sql.Err;
                    return null;
                }
            }
            else
            {
                if (this.Sql.GetSql("Order.OutPatient.Case.GetAllModuleByOperId", ref strSql) == -1)
                {
                    this.Err = this.Sql.Err;
                    return null;
                }
            }
            strSql = System.String.Format(strSql, moduletype, Code);
            return this.GetMyModule(strSql);
        }

        #region ˽�к���
        /// <summary>
        /// �õ�����ģ��ʵ��
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        private ArrayList GetMyModule(string strSql)
        {
            ArrayList al = new ArrayList();
            if (this.ExecQuery(strSql) == -1) return null;
            while (this.Reader.Read())
            {
                Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory casehistory = new Neusoft.HISFC.Models.Order.OutPatient.ClinicCaseHistory();
                casehistory.CaseMain = this.Reader.GetValue(0).ToString();//����
                casehistory.CaseNow = this.Reader.GetValue(1).ToString();//�ֲ�ʷ
                casehistory.CaseOld = this.Reader.GetValue(2).ToString();//����ʷ
                casehistory.CaseAllery = this.Reader.GetValue(3).ToString();//����ʷ
                casehistory.CheckBody = this.Reader.GetValue(4).ToString();//����
                casehistory.CaseDiag = this.Reader.GetValue(5).ToString();//���
                casehistory.Memo = this.Reader.GetValue(6).ToString();//��ע
                casehistory.Name = this.Reader.GetValue(7).ToString();//ģ������
                casehistory.ID = this.Reader.GetValue(8).ToString();//ģ����ˮ��
                casehistory.ModuleType = this.Reader.GetValue(9).ToString();//���
                casehistory.DoctID = this.Reader.GetValue(10).ToString();//ҽʦ����
                casehistory.DeptID = this.Reader.GetValue(11).ToString();//����
                al.Add(casehistory);
            }
            this.Reader.Close();
            return al;
        }
        #endregion

        #endregion

        #region ˽�к���

        /// <summary>
		/// ���sql���������
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="order"></param>
		/// <returns></returns>
		protected string myGetSql(string sql,Neusoft.HISFC.Models.Order.OutPatient.Order order)
		{
			#region sql
			//   0--������� ,1 --��Ŀ��ˮ��,2 --�����,3   --������ ,4    --�Һ�����
			//   5 --�Һſ���,6   --��Ŀ����,7   --��Ŀ����, 8  --���, 9  --1ҩƷ��2��ҩƷ
			//   10   --ϵͳ���,   --��С���ô���,   --����,   --��������,   --����
			//    --��װ����,   --�Ƽ۵�λ,   --�Էѽ��0,   --�Ը����0,   --�������0
			//   --��������,   --����ҩ,   --ҩƷ���ʣ���ҩ����ҩ,   --ÿ������
			//     --ÿ��������λ,   --���ʹ���,   --Ƶ��,   --Ƶ������,   --ʹ�÷���
			//     --�÷�����,   --�÷�Ӣ����д,   --ִ�п��Ҵ���,   --ִ�п�������
			//      --��ҩ��־,   --��Ϻ�,   --1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ����
			//     --Ժ��ע�����,   --��ע,   --����ҽ��,   --����ҽ������,   --ҽ������
			//     --����ʱ��,   --����״̬,1������2�շѣ�3ȷ�ϣ�4����,   --������,   --����ʱ��
			//        --�Ӽ����0��ͨ/1�Ӽ�,   --��������,   --����,   --���뵥��
			//     --0���Ǹ���/1�Ǹ���,   --�Ƿ���Ҫȷ�ϣ�1��Ҫ��0����Ҫ,   --ȷ����
			//        --ȷ�Ͽ���,   --ȷ��ʱ��,   --0δ�շ�/1�շ�,   --�շ�Ա
            //       --�շ�ʱ��,   --������,    --��������ˮ��,     --��ҩҩ����    
            //      --������λ�Ƿ�����С��λ 1 �� 0 ���ǣ�      --ҽ�����ͣ�Ŀǰû�У�
			#endregion

			//if(order.Item.IsPharmacy)//ҩƷ
            if(order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
			{
				Neusoft.HISFC.Models.Pharmacy.Item pItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                //{9BAE643C-57BF-4dc5-889E-6B5F6B3E1E38} ���ڽ���������뵥��apply_no�ֶθ�order.ApplyNo20100505 yangw
                System.Object[] s = {order.SeeNO ,Neusoft.FrameWork.Function.NConvert.ToInt32(order.ID),order.Patient.ID,order.Patient.PID.CardNO,order.RegTime,
										order.InDept.ID,pItem.ID,pItem.Name,pItem.Specs,"1",
										order.Item.SysClass.ID,order.Item.MinFee.ID,order.Item.Price,order.Qty,order.HerbalQty,
										pItem.PackQty,pItem.PriceUnit,order.FT.OwnCost ,order.FT.PayCost,order.FT.PubCost,
										pItem.BaseDose,Neusoft.FrameWork.Function.NConvert.ToInt32(pItem.Product.IsSelfMade),pItem.Quality.ID,order.DoseOnce,
										order.DoseUnit,pItem.DosageForm.ID,order.Frequency.ID,order.Frequency.Name,order.Usage.ID,
										order.Usage.Name,order.Usage.Memo,order.ExeDept.ID,order.ExeDept.Name,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.Combo.IsMainDrug),order.Combo.ID,order.HypoTest,
										order.InjectCount,order.Memo,order.ReciptDoctor.ID,order.ReciptDoctor.Name,order.ReciptDept.ID,
										order.MOTime,order.Status,order.DCOper.ID,order.DCOper.OperTime,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsEmergency),order.Sample.Name,order.CheckPartRecord,order.ApplyNo,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsSubtbl),Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsNeedConfirm),order.ConfirmOper.ID,
										order.ConfirmOper.Dept.ID,order.ConfirmOper.OperTime,Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsHaveCharged),order.ChargeOper.ID,
										order.ChargeOper.OperTime,order.ReciptNO,order.SequenceNO,
                                        order.StockDept.ID,order.NurseStation.User03,"",
                                        order.NurseStation.User01,order.ExtendFlag1,
										order.ReciptSequence,order.NurseStation.Memo,order.SortID};

				try
				{
					string sReturn = string.Format(sql,s);
					return sReturn;
				}
				catch(Exception ex)
				{
					this.Err = ex.Message;
					this.WriteErr();
					return null;
				}
			}
			else//��ҩƷ
			{
				Neusoft.HISFC.Models.Fee.Item.Undrug pItem = order.Item as Neusoft.HISFC.Models.Fee.Item.Undrug;
                //{9BAE643C-57BF-4dc5-889E-6B5F6B3E1E38} ���ڽ���������뵥��apply_no�ֶθ�order.ApplyNo 20100505 yangw
                System.Object[] s = {order.SeeNO,Neusoft.FrameWork.Function.NConvert.ToInt32(order.ID),order.Patient.ID,order.Patient.PID.CardNO,order.RegTime,
										order.InDept.ID,pItem.ID,pItem.Name,pItem.Specs,"2",
										order.Item.SysClass.ID,order.Item.MinFee.ID,order.Item.Price,order.Qty,order.HerbalQty,
										pItem.PackQty,pItem.PriceUnit,order.FT.OwnCost ,order.FT.PayCost,order.FT.PubCost,
										"0",0,"",order.DoseOnce,
										order.DoseUnit,"",order.Frequency.ID,order.Frequency.Name,order.Usage.ID,
										order.Usage.Name,order.Usage.Memo,order.ExeDept.ID,order.ExeDept.Name,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.Combo.IsMainDrug),order.Combo.ID,order.HypoTest,
										order.InjectCount,order.Memo,order.ReciptDoctor.ID,order.ReciptDoctor.Name,order.ReciptDept.ID,
										order.MOTime,order.Status,order.DCOper.ID,order.DCOper.OperTime,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsEmergency),order.Sample.Name,order.CheckPartRecord,order.ApplyNo,
										Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsSubtbl),Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsNeedConfirm),order.ConfirmOper.ID,
										order.ConfirmOper.Dept.ID,order.ConfirmOper.OperTime,Neusoft.FrameWork.Function.NConvert.ToInt32(order.IsHaveCharged),order.ChargeOper.ID,
										order.ChargeOper.OperTime,order.ReciptNO,order.SequenceNO,
                                        order.StockDept.ID,order.NurseStation.User03,"",
                                        order.NurseStation.User01,order.ExtendFlag1,
										order.ReciptSequence,order.NurseStation.Memo,order.SortID};
				try
				{
					string sReturn = string.Format(sql,s);
					return sReturn;
				}
				catch(Exception ex)
				{
					this.Err = ex.Message;
					this.WriteErr();
					return null;
				}
	
			}	
			
		}

		
		/// <summary>
		/// ��ò�ѯsql���
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		protected int myGetSelectSql(ref string sql)
		{
			return this.Sql.GetSql("Order.OutPatient.Order.Query.Select",ref sql);
		}
		

		/// <summary>
		/// ���ִ��ҽ����Ϣ
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		protected ArrayList myGetExecOrder(string sql)
		{
			if(this.ExecQuery(sql)==-1) return null;
			ArrayList al = new ArrayList();
			while(this.Reader.Read())
			{
				Neusoft.HISFC.Models.Order.OutPatient.Order order = new Neusoft.HISFC.Models.Order.OutPatient.Order();
				order.SeeNO = this.Reader[0].ToString();
				order.SequenceNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[1].ToString());//��Ŀ��ˮ��
				order.ID = this.Reader[1].ToString();//��Ŀ��ˮ��
				order.Patient.ID  = this.Reader[2].ToString();//�����
				order.Patient.PID.CardNO = this.Reader[3].ToString();//��������
				order.RegTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[4]);//�Һ�����
				order.ReciptDept.ID = this.Reader[5].ToString();//�Һſ��� ����
				if(this.Reader[9].ToString() =="1")//ҩƷ
				{
					Neusoft.HISFC.Models.Pharmacy.Item item = new Neusoft.HISFC.Models.Pharmacy.Item();
					item.ID = this.Reader[6].ToString();
					item.Name = this.Reader[7].ToString();
					item.Specs = this.Reader[8].ToString();
					item.SysClass.ID = this.Reader[10].ToString();
					item.MinFee.ID = this.Reader[11].ToString();
					item.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[12]);
					item.BaseDose = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[20]);
					item.DoseUnit =  this.Reader[24].ToString();
					item.Product.IsSelfMade = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[21]);
					item.Quality.ID = this.Reader[22].ToString();
					item.PackQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[15]);
					item.DosageForm.ID = this.Reader[25].ToString();
					item.PriceUnit = this.Reader[16].ToString();

                    //{6DBBDC62-2303-4d97-85EF-8BA2A622117A} ������� xuc
                    item.SplitType = this.Reader[61].ToString();

					order.Item = item;

				}
				else if(this.Reader[9].ToString() =="2")//��ҩƷ
				{
					Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();
					item.ID = this.Reader[6].ToString();
					item.Name = this.Reader[7].ToString();
					item.Specs = this.Reader[8].ToString();
					item.SysClass.ID = this.Reader[10].ToString();
					item.MinFee.ID = this.Reader[11].ToString();
					item.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[12]);
					item.PackQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[15]);
					item.PriceUnit = this.Reader[16].ToString();
					order.Item = item;

				}else
				{
					this.Err ="��ȡmet_ord_recipedetail������ҩƷ��ҩƷ����drug_flag="+this.Reader[9].ToString();
					this.WriteErr();
					return null;
				}
				order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[13]);
				order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[14]);
				order.Unit = this.Reader[16].ToString();
				order.FT.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[17]);
				order.FT.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[18]);
				order.FT.PubCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[19]);
					
				order.DoseOnce = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[23]);
				order.DoseUnit = this.Reader[24].ToString();
					
				order.Frequency.ID = this.Reader[26].ToString();
				order.Frequency.Name = this.Reader[27].ToString();
				order.Usage.ID  = this.Reader[28].ToString();
				order.Usage.Name  = this.Reader[29].ToString();
				order.Usage.Memo = this.Reader[30].ToString();
				order.ExeDept.ID = this.Reader[31].ToString();
				order.ExeDept.Name = this.Reader[32].ToString();
				order.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[33]);
				order.Combo.ID = this.Reader[34].ToString();
				order.HypoTest = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[35]);
				order.InjectCount = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[36]);
				order.Memo = this.Reader[37].ToString();
				order.ReciptDoctor.ID = this.Reader[38].ToString();
				order.ReciptDoctor.Name = this.Reader[39].ToString();
				order.ReciptDept.ID =this.Reader[40].ToString();
				//order.ReciptDept.Name =this.Reader[41].ToString();
				order.MOTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[41]);
				order.Status = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[42]);
				order.DCOper.ID = this.Reader[43].ToString();
				order.DCOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[44]);
				order.IsEmergency = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[45]);
				order.Sample.Name = this.Reader[46].ToString();
				order.CheckPartRecord = this.Reader[47].ToString();
				order.ApplyNo = this.Reader[48].ToString();
				order.IsSubtbl = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[49]);
				order.IsNeedConfirm = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[50]);
				order.ConfirmOper.ID = this.Reader[51].ToString();
				order.ConfirmOper.Dept.ID = this.Reader[52].ToString();
				order.ConfirmOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[53]);
				order.IsHaveCharged = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[54]);
				order.ChargeOper.ID = this.Reader[55].ToString();
				order.ChargeOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[56]);
				order.ReciptNO = this.Reader[57].ToString();
				order.SequenceNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[58]);
                order.StockDept.ID = this.Reader[59].ToString();
                order.NurseStation.User03 = this.Reader[60].ToString();//��С��λ��־
                //order.OrderType.ID = this.Reader[62].ToString();
                order.NurseStation.User01 = this.Reader[63].ToString();//������Ϻţ����飩
                order.ExtendFlag1 = this.Reader[64].ToString();//��ƿ��Ϣ
                order.ReciptSequence = this.Reader[65].ToString();//�շ�����
                order.NurseStation.Memo = this.Reader[66].ToString();
                order.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[67]);
				#region sql
				//		0--������� 1  --��Ŀ��ˮ��, 2  --�����, 3  --������ 4 -�Һ�����  5,   --�Һſ���
				//       6,   --��Ŀ���� 7,   --��Ŀ���� 8,   --��� 9,   --1ҩƷ��2��ҩƷ 10,   --ϵͳ���
				//       11,   --��С���ô��� 12,   --���� 13,   --�������� 14 ,   --����  15,   --��װ����
				//       16,   --�Ƽ۵�λ 17,   --�Էѽ�� 18  --�Ը���� 19,   --������� 20,   --��������
				//       21,   --����ҩ 2  --ҩƷ���ʣ���ҩ����ҩ 23,   --ÿ������ 24,   --ÿ��������λ 25,   --���ʹ���
				//       26,   --Ƶ�� 27,   --Ƶ������ 28 --ʹ�÷��� 29,   --�÷����� 30,   --�÷�Ӣ����д
				//       31,   --ִ�п��Ҵ��� 32,   --ִ�п������� 33   --��ҩ��־ 34   --��Ϻ� 35   --1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ����
				//       36,   --Ժ��ע����� 37   --��ע  38,   --����ҽ�� 39,   --����ҽ������ 40,   --ҽ������
				//       41,   --����ʱ��   42,   --����״̬,1������2�շѣ�3ȷ�ϣ�4����    43,   --������   44,   --����ʱ��    45,   --�Ӽ����0��ͨ/1�Ӽ�
				//       46,   --��������    47,   --���� 48,   --���뵥��    49,   --0û�и���/1������/2�Ǹ���     50,   --�Ƿ���Ҫȷ�ϣ�1��Ҫ��0����Ҫ
				//       51,   --ȷ����     52,   --ȷ�Ͽ���    53,   --ȷ��ʱ��    54,   --0δ�շ�/1�շ�      55,   --�շ�Ա
				//       56,   --�շ�ʱ��      57,   --������      58    --��������ˮ��
				//  FROM met_ord_recipedetail   --��䴦����ϸ��
				#endregion
				al.Add(order);
			}
			this.Reader.Close();
			return al;
		}


		#endregion

        #region ����÷����÷������ĸ���(add by sunm from 4.0)
        public Hashtable GetUsageAndSub()
        {
            string strSql = "";

            if (this.Sql.GetSql("Order.OutPatient.Order.GetUsageAndSub", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return null;
            }

            if (this.ExecQuery(strSql) < 0)
            {
                this.Err = "Exec Err" + this.Err;
                return null;
            }

            string usageCode = "";
            Hashtable hsUsageAndSub = new Hashtable();

            while (this.Reader.Read())
            {
                usageCode = this.Reader[0].ToString();

                if (!hsUsageAndSub.Contains(usageCode))
                {
                    ArrayList al = new ArrayList();

                    Neusoft.FrameWork.Models.NeuObject o = new Neusoft.FrameWork.Models.NeuObject();

                    o.ID = this.Reader[1].ToString();

                    al.Add(o);

                    hsUsageAndSub.Add(usageCode, al);
                }
                else
                {
                    Neusoft.FrameWork.Models.NeuObject o = new Neusoft.FrameWork.Models.NeuObject();

                    o.ID = this.Reader[1].ToString();

                    (hsUsageAndSub[usageCode] as ArrayList).Add(o);
                }
            }
            this.Reader.Close();
            return hsUsageAndSub;
        }
        #endregion

	}
}
