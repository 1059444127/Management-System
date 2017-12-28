using System;
using System.Collections;
using System.Data ;

namespace Neusoft.HISFC.BizLogic.Registration
{
	/// <summary>
	/// �Ű������
	/// </summary>
	public class Schema:Neusoft.FrameWork.Management.Database
	{
		/// <summary>
		/// 
		/// </summary>
		public Schema()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region ����
		/// <summary>
		/// �Ǽ�һ���Ű��¼
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public int Insert(Neusoft.HISFC.Models.Registration.Schema schema)
		{
            string sql = "";

            if (this.Sql.GetSql("Registration.Schema.Insert", ref sql) == -1) return -1;

            try
            {
                sql = string.Format(sql, schema.Templet.ID, (int)schema.Templet.EnumSchemaType, schema.SeeDate.ToString(),
                    (int)schema.Templet.Week, schema.Templet.Noon.ID, schema.Templet.Dept.ID, schema.Templet.Dept.Name,
                    schema.Templet.Doct.ID, schema.Templet.Doct.Name, schema.Templet.DoctType.ID, schema.Templet.RegQuota,
                    schema.RegedQTY, Neusoft.FrameWork.Function.NConvert.ToInt32(schema.Templet.IsValid),
                    schema.Templet.StopReason.ID, schema.Templet.StopReason.Name, schema.Templet.Stop.ID, schema.Templet.Stop.OperTime.ToString(),
                    schema.Templet.Memo, schema.Templet.Oper.ID, schema.Templet.Oper.OperTime.ToString(),
                    schema.Templet.Begin.ToString(), schema.Templet.End.ToString(), schema.Templet.TelQuota,
                    schema.TeledQTY, schema.TelingQTY, schema.Templet.SpeQuota, schema.SpedQTY,
                    Neusoft.FrameWork.Function.NConvert.ToInt32(schema.Templet.IsAppend), schema.Templet.RegLevel.ID, schema.Templet.RegLevel.Name,
                    schema.FromTempletID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Insert]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
		}
		#endregion

		#region ɾ��
		/// <summary>
		/// ����IDɾ��һ���Ű��¼
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public int Delete(string ID)
		{
			string sql = "";

			if(this.Sql.GetSql("Registration.Schema.Delete",ref sql) == -1)return -1;

			try
			{
				sql = string.Format(sql,ID);
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Delete]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return -1;
			}

			return this.ExecNoQuery(sql);
		}

        /// <summary>
        /// ����ģ��idɾ���Ű��¼
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DeleteZZ(string templetID)
        {
            string sql = "";

            if (this.Sql.GetSql("ZZ.Registration.Schema.Delete", ref sql) == -1) return -1;

            try
            {
                sql = string.Format(sql, templetID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Delete]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
        }
		#endregion

		#region �޸�
		/// <summary>
		/// ����ID�޸�һ���Ű��¼(��ʹ�õ�)
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public int Update(Neusoft.HISFC.Models.Registration.Schema schema)
		{
			string sql = "";

			if(this.Sql.GetSql("Registration.Schema.Update",ref sql) == -1)return -1;

			try
			{
				sql = string.Format(sql,schema.Templet.RegQuota,Neusoft.FrameWork.Function.NConvert.ToInt32(schema.Templet.IsValid),
					schema.Templet.StopReason.ID,schema.Templet.StopReason.Name,schema.Templet.Stop.ID,
					schema.Templet.Stop.OperTime.ToString(),schema.Templet.Memo,schema.Templet.Oper.ID,
					schema.Templet.Oper.OperTime.ToString(),schema.Templet.TelQuota,
					schema.Templet.SpeQuota,schema.Templet.ID,schema.Templet.RegLevel.ID,schema.Templet.RegLevel.Name) ;
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Update]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return -1;
			}

			return this.ExecNoQuery(sql);
		}

        /// <summary>
        /// ����ID�����Ű���Ϣ(δʹ�õ�)
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public int UpdateUnused(Neusoft.HISFC.Models.Registration.Schema schema)
        {
            string sql = "";

            if (this.Sql.GetSql("Registration.Schema.Update.3", ref sql) == -1) return -1;

            try
            {
                sql = string.Format(sql, schema.Templet.ID, schema.Templet.Noon.ID, schema.Templet.Begin.ToString(),
                        schema.Templet.End.ToString(), schema.Templet.Dept.ID, schema.Templet.Dept.Name,
                        schema.Templet.Doct.ID, schema.Templet.Doct.Name, schema.Templet.DoctType.ID,
                        schema.Templet.RegQuota, schema.Templet.TelQuota, schema.Templet.SpeQuota,
                        Neusoft.FrameWork.Function.NConvert.ToInt32(schema.Templet.IsValid),
                        Neusoft.FrameWork.Function.NConvert.ToInt32(schema.Templet.IsAppend),
                        schema.Templet.StopReason.ID, schema.Templet.StopReason.Name, schema.Templet.Stop.ID,
                        schema.Templet.Stop.OperTime.ToString(), schema.Templet.Memo, schema.Templet.Oper.ID,
                        schema.Templet.Oper.OperTime.ToString(), schema.Templet.RegLevel.ID, schema.Templet.RegLevel.Name);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Update.3]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
        }

        /// <summary>
        /// �����Ű�ģ��ID�����Ű���Ϣ(δʹ�õ�)
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public int UpdateUnusedZZ(Neusoft.HISFC.Models.Registration.SchemaTemplet schemaTemplet)
        {
            string sql = "";

            if (this.Sql.GetSql("ZZ.Registration.Schema.UpdateUnUse", ref sql) == -1) return -1;

            try
            {
                sql = string.Format(sql, schemaTemplet.ID, schemaTemplet.Noon.ID, schemaTemplet.Begin.ToString(),
                        schemaTemplet.End.ToString(), schemaTemplet.Dept.ID, schemaTemplet.Dept.Name,
                        schemaTemplet.Doct.ID, schemaTemplet.Doct.Name, schemaTemplet.DoctType.ID,
                        schemaTemplet.RegQuota, schemaTemplet.TelQuota, schemaTemplet.SpeQuota,
                        Neusoft.FrameWork.Function.NConvert.ToInt32(schemaTemplet.IsValid),
                        Neusoft.FrameWork.Function.NConvert.ToInt32(schemaTemplet.IsAppend),
                        schemaTemplet.StopReason.ID, schemaTemplet.StopReason.Name, schemaTemplet.Stop.ID,
                        schemaTemplet.Stop.OperTime.ToString(), schemaTemplet.Memo, schemaTemplet.Oper.ID,
                        schemaTemplet.Oper.OperTime.ToString(), schemaTemplet.RegLevel.ID, schemaTemplet.RegLevel.Name);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Update.3]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
        }

		/// <summary>
		/// ���¿�������
		/// IsReged:�Ƿ������˹Һ�,IsTeling:�Ƿ�������ԤԼ,IsTeled:�Ƿ�����Һ�
		/// IsSped:�Ƿ�����Һ�
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="IsReged"></param>
		/// <param name="IsTeling"></param>
		/// <param name="IsTeled"></param>
		/// <param name="IsSped"></param>
		/// <returns></returns>
		public int Increase(string ID,bool IsReged,bool IsTeling, bool IsTeled,bool IsSped)
		{
            string sql = "";

            if (this.Sql.GetSql("Registration.Schema.Update.4", ref sql) == -1) return -1;

            int reged = 0, teling = 0, teled = 0, sped = 0, add = 1;

            if (IsReged) reged = 1;
            if (IsTeling)//�绰ԤԼ�������ӿ������
            {
                teling = 1;
                add = 0;
            }
            if (IsTeled) teled = 1;
            if (IsSped) sped = 1;

            try
            {
                sql = string.Format(sql, reged, teled, teling, sped, ID, add);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Update.4]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
		}

		/// <summary>
		///  ���ٿ�������
		///  IsReged:�Ƿ������˹Һ�,IsTeling:�Ƿ�������ԤԼ,IsTeled:�Ƿ�����Һ�
		///  IsSped:�Ƿ�����Һ�
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="IsReged"></param>
		/// <param name="IsTeling"></param>
		/// <param name="IsTeled"></param>
		/// <param name="IsSped"></param>
		/// <returns></returns>
		public int Reduce(string ID,bool IsReged,bool IsTeling, bool IsTeled,bool IsSped)
		{
            string sql = "";

            if (this.Sql.GetSql("Registration.Schema.Update.1", ref sql) == -1) return -1;

            int reged = 0, teling = 0, teled = 0, sped = 0;

            if (IsReged) reged = -1;
            if (IsTeling) teling = -1;
            if (IsTeled) teled = -1;
            if (IsSped) sped = -1;

            try
            {
                sql = string.Format(sql, reged, teled, teling, sped, ID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Update.1]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
		}
        /// <summary>
        ///  ���ٿ�������
        ///  IsReged:�Ƿ������˹Һ�,IsTeling:�Ƿ�������ԤԼ,IsTeled:�Ƿ�����Һ�
        ///  IsSped:�Ƿ�����Һ�
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="IsReged"></param>
        /// <param name="IsTeling"></param>
        /// <param name="IsTeled"></param>
        /// <param name="IsSped"></param>
        /// <returns></returns>
        public int AddLimit(string ID, bool IsReged, bool IsTeling, bool IsTeled, bool IsSped)
        {
            string sql = "";

            if (this.Sql.GetSql("Registration.Schema.Update.1", ref sql) == -1) return -1;

            int reged = 0, teling = 0, teled = 0, sped = 0;

            if (IsReged) reged = 1;
            if (IsTeling) teling = 1;
            if (IsTeled) teled = 1;
            if (IsSped) sped = 1;

            try
            {
                sql = string.Format(sql, reged, teled, teling, sped, ID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Update.1]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return -1;
            }

            return this.ExecNoQuery(sql);
        }
		#endregion

		#region ��ѯ

		/// <summary>
		/// 
		/// </summary>
		protected Neusoft.HISFC.Models.Registration.Schema objSchema ;
		private ArrayList al ;

		/// <summary>
		/// ���ݳ���ʱ�䡢�Ű����ͼ����Ű���Ϣ
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="type"></param>
		/// <param name="deptID"></param>
		/// <returns></returns>
		public ArrayList Query(DateTime seeDate,Neusoft.HISFC.Models.Base.EnumSchemaType type,string deptID)
		{
			string sql = "",where = "";

			if(this.Sql.GetSql("Registration.Schema.Query.1",ref sql) == -1)return null;
			if(this.Sql.GetSql("Registration.Schema.Query.2",ref where ) == -1)return null;

			sql = sql + " " + where;

			try
			{
				sql = string.Format(sql,seeDate.ToString(),(int)type,deptID);
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.2]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null;
			}
			return this.QueryBase(sql);
		}

		#region ѡ��ҽ�������б�ʹ��
        
		/// <summary>
		/// ���ݳ���ʱ�䡢���ҡ������Ű���Ϣ
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="deptID"></param>		
		/// <returns></returns>
		public ArrayList QueryByDept(DateTime seeDate,string deptID)
		{
			string sql = "",where = "";

			if(this.Sql.GetSql("Registration.Schema.Query.1",ref sql) == -1)return null;
			if(this.Sql.GetSql("Registration.Schema.Query.3",ref where ) == -1)return null;

			sql = sql + " " + where;

			try
			{
				sql = string.Format(sql,seeDate.Date.ToString(),deptID);
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.3]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null;
			}
			return this.QueryBase(sql);
		}

		/// <summary>
		/// ���ݳ���ʱ�䡢ҽ��������ҽ���Ű���Ϣ
        /// �ʺϹҺŲ�ѡ�����,ֱ��ѡ��ҽ��,һ��ҽ���ɳ���������
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="doctID"></param>
		/// <returns></returns>
		public ArrayList QueryByDoct(DateTime seeDate,string doctID)
		{
			string sql = "",where = "";

			if(this.Sql.GetSql("Registration.Schema.Query.1",ref sql) == -1)return null;
			if(this.Sql.GetSql("Registration.Schema.Query.4",ref where ) == -1)return null;

			sql = sql + " " + where;

			try
			{
				sql = string.Format(sql,seeDate.Date.ToString(),doctID);
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.4]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null;
			}
			return this.QueryBase(sql);
		}

        /// <summary>
        /// ���ݳ���ʱ�䡢���ҡ�ҽ��������ҽ���Ű���Ϣ
        /// �ʺϹҺ�ʱ��ѡ�����,��ѡ��ҽ��,һ��ҽ���ɳ���������,
        /// ֻ��ʾĳһ��������ҵ��Ű���Ϣ
        /// </summary>
        /// <param name="seeDate"></param>
        /// <param name="deptID"></param>
        /// <param name="doctID"></param>
        /// <returns></returns>
        public ArrayList QueryByDoct(DateTime seeDate, string deptID, string doctID)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Registration.Schema.Query.1", ref sql) == -1) return null;
            if (this.Sql.GetSql("Registration.Schema.Query.14", ref where) == -1) return null;

            sql = sql + " " + where;

            try
            {
                sql = string.Format(sql, seeDate.Date.ToString(), doctID, deptID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Query.14]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }
            return this.QueryBase(sql);
        }
        
		/// <summary>
		/// �����Ӻ��Ű���Ϣʵ��
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="deptID"></param>
		/// <param name="doctID"></param>
		/// <param name="noonID"></param>		
		/// <returns></returns>
		public Neusoft.HISFC.Models.Registration.Schema QueryAppend(DateTime seeDate, string deptID, string doctID, string noonID)
		{
			string sql = "",where = "";

			if(this.Sql.GetSql("Registration.Schema.Query.1",ref sql) == -1)return null;
			if(this.Sql.GetSql("Registration.Schema.Query.5",ref where ) == -1)return null;

			sql = sql + " " + where;

			try
			{
				sql = string.Format(sql,seeDate.ToString("yyyy-MM-dd"),deptID,doctID,noonID	) ;
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.5]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null;
			}

			if(this.QueryBase(sql) == null) return null ;

			if(al == null)return null ;
			if(al.Count == 0) return new Neusoft.HISFC.Models.Registration.Schema() ;

			return (Neusoft.HISFC.Models.Registration.Schema)al[0] ;
		}      
		#endregion

		/// <summary>
		/// ��ѯר���ճ������/����ר�ҵĿ������
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="endDate"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public DataSet QueryDept( DateTime seeDate,DateTime endDate, Neusoft.HISFC.Models.Base.EnumSchemaType type)
		{
			string sql = "" ;
			DataSet ds = new DataSet() ;
 
			if(type == Neusoft.HISFC.Models.Base.EnumSchemaType.Dept)
			{
				if(this.Sql.GetSql("Registration.Schema.Query.6",ref sql) == -1) return null ;
				
				try
				{
					sql = string.Format(sql,seeDate.Date.ToString(), endDate.ToString()) ;
				}
				catch(Exception e)
				{
					this.Err = "[Registration.Schema.Query.6]��ʽ��ƥ��!"+e.Message;
					this.ErrCode = e.Message;
					return null ;
				}

				if(this.ExecQuery(sql, ref ds) == -1) return null; 

				return ds ;
			}
			else 
			{
				if(this.Sql.GetSql("Registration.Schema.Query.7",ref sql) == -1) return null ;
				
				try
				{
					sql = string.Format(sql,seeDate.Date.ToString(), endDate.ToString()) ;
				}
				catch(Exception e)
				{
					this.Err = "[Registration.Schema.Query.7]��ʽ��ƥ��!"+e.Message;
					this.ErrCode = e.Message;
					return null ;
				}

				if(this.ExecQuery(sql, ref ds) == -1) return null; 

				return ds ;
			}
		}

		/// <summary>
		/// ��ѯĳ��ȫ������ר��
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public DataSet QueryDoct( DateTime seeDate,DateTime endDate )
		{
			string sql = "" ;
			DataSet ds = new DataSet() ;

			if(this.Sql.GetSql("Registration.Schema.Query.8",ref sql) == -1) return null ;
				
			try
			{
				sql = string.Format(sql,seeDate.Date.ToString(), endDate.ToString()) ;
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.8]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null ;
			}

			if(this.ExecQuery(sql, ref ds) == -1) return null; 

			return ds ;
		}


        /// <summary>
        /// ��ѯĳ��ȫ������ר��
        /// </summary>
        /// <param name="seeDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet QueryDoctForLED(DateTime seeDate, DateTime endDate)
        {
            string sql = "";
            DataSet ds = new DataSet();

            if (this.Sql.GetSql("Registration.Schema.Query.99", ref sql) == -1) return null;

            try
            {
                sql = string.Format(sql, seeDate.Date.ToString(), endDate.ToString());
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Query.99]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            if (this.ExecQuery(sql, ref ds) == -1) return null;

            return ds;
        }
		/// <summary>
		/// ��ѯĳ�ա�ĳ���ҳ���ר��
		/// </summary>
		/// <param name="seeDate"></param>
		/// <param name="endDate"></param>
		/// <param name="deptID"></param>
		/// <returns></returns>
		public DataSet QueryDoct(DateTime seeDate, DateTime endDate, string deptID)
		{
			string sql = "" ;
			DataSet ds = new DataSet() ;

			if(this.Sql.GetSql("Registration.Schema.Query.9",ref sql) == -1) return null ;
				
			try
			{
				sql = string.Format(sql,seeDate.Date.ToString(), endDate.ToString(), deptID) ;
			}
			catch(Exception e)
			{
				this.Err = "[Registration.Schema.Query.9]��ʽ��ƥ��!"+e.Message;
				this.ErrCode = e.Message;
				return null ;
			}

			if(this.ExecQuery(sql, ref ds) == -1) return null; 

			return ds ;
		}

        /// <summary>
        /// �����Ű���Ż�ȡ�Ű�ʵ��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Registration.Schema GetByID(string ID)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Registration.Schema.Query.1", ref sql) == -1) return null;
            if (this.Sql.GetSql("Registration.Schema.Query.11", ref where) == -1) return null;

            sql = sql + " " + where;

            try
            {
                sql = string.Format(sql, ID);
            }
            catch (Exception e)
            {
                this.Err = "[Registration.Schema.Query.11]��ʽ��ƥ��!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            if (this.QueryBase(sql) == null) return null;

            if (al == null) return null;

            if (al.Count == 0) return new Neusoft.HISFC.Models.Registration.Schema();

            return (Neusoft.HISFC.Models.Registration.Schema)al[0];
        }

		/// <summary>
		/// ��ѯ
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public ArrayList QueryBase(string sql)
		{
			if(this.ExecQuery(sql) == -1) return null;

			this.al = new ArrayList();

			try
			{
				while(this.Reader.Read())
				{
					this.objSchema = new Neusoft.HISFC.Models.Registration.Schema();
					
					this.objSchema.Templet.ID = this.Reader[2].ToString();
					this.objSchema.Templet.EnumSchemaType = (Neusoft.HISFC.Models.Base.EnumSchemaType)(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[3].ToString()));
					this.objSchema.SeeDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[4].ToString());
					this.objSchema.Templet.Week = (DayOfWeek)(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[5].ToString()));
					this.objSchema.Templet.Noon.ID = this.Reader[6].ToString();
					this.objSchema.Templet.Dept.ID = this.Reader[7].ToString();
					this.objSchema.Templet.Dept.Name = this.Reader[8].ToString();
					this.objSchema.Templet.Doct.ID = this.Reader[9].ToString();
					this.objSchema.Templet.Doct.Name = this.Reader[10].ToString();
					this.objSchema.Templet.DoctType.ID = this.Reader[11].ToString();					
					this.objSchema.Templet.RegQuota = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[12].ToString());
					this.objSchema.RegedQTY = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[13].ToString());
					this.objSchema.Templet.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[14].ToString());
					this.objSchema.Templet.StopReason.ID = this.Reader[15].ToString();
					this.objSchema.Templet.StopReason.Name = this.Reader[16].ToString();
					this.objSchema.Templet.Stop.ID = this.Reader[17].ToString();
					this.objSchema.Templet.Stop.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[18].ToString());
					this.objSchema.Templet.Memo = this.Reader[19].ToString();
					this.objSchema.Templet.Oper.ID = this.Reader[20].ToString();
					this.objSchema.Templet.Oper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[21].ToString());
					this.objSchema.Templet.Begin = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[22].ToString()) ;
					this.objSchema.Templet.End = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[23].ToString()) ;
					this.objSchema.Templet.TelQuota = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[24].ToString()) ;
					this.objSchema.TeledQTY = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[25].ToString()) ;
					this.objSchema.TelingQTY = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[26].ToString()) ;
					this.objSchema.Templet.SpeQuota = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[27].ToString()) ;
					this.objSchema.SpedQTY = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[28].ToString()) ;
					this.objSchema.Templet.IsAppend = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[29].ToString()) ;
                    this.objSchema.SeeNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[30].ToString());
                    this.objSchema.Templet.RegLevel.ID = this.Reader[31].ToString();
                    this.objSchema.Templet.RegLevel.Name = this.Reader[32].ToString();
                    this.objSchema.FromTempletID = this.Reader[33].ToString();

					this.al.Add(this.objSchema);
				}

				this.Reader.Close();
			}
			catch(Exception e)
			{
				this.Err = "��ѯ�Ű���Ϣ����!" + e.Message;
				this.ErrCode = e.Message;
				return null;
			}

			return al;
		}


		/// <summary>
		/// ��ȡ�������Ч�Ű���Ϣ
		/// </summary>
		/// <param name="schemaType"></param>		
		/// <param name="current"></param>
		/// <param name="deptID"></param>
		/// <param name="doctID"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Registration.Schema Query(Neusoft.HISFC.Models.Base.EnumSchemaType schemaType,
				DateTime current, string deptID, string doctID)
		{
			#region ������� ̫����Ľ�����,��дsql�ˣ� ����ǰҹ
			/*
			//ԤԼר��
			WHERE see_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')
			AND schema_type = '1'
			AND doct_code = '{1}'
			AND valid_flag = '1'
			AND (tel_lmt > tel_reging OR append_flag = '1')
			ORDER BY noon_code,append_flag,begin_time

			//ԤԼר��
			WHERE see_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')
			AND schema_type = '0'
			AND dept_code = '{1}'
			AND valid_flag = '1'
			AND (tel_lmt > tel_reging OR append_flag = '1')
			ORDER BY noon_code,append_flag,begin_time

			//ר��
			WHERE see_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')
			AND schema_type = '0'
			AND dept_code = '{1}'
			AND valid_flag = '1'
			AND (reg_lmt > reged OR append_flag = '1')
			ORDER BY noon_code,append_flag,begin_time

			//ר��
			WHERE see_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')
			AND schema_type = '1'
			AND doct_code = '{1}'
			AND valid_flag = '1'
			AND (reg_lmt > reged OR append_flag = '1')
			ORDER BY noon_code,append_flag,begin_time

			//����
			WHERE see_date >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')
			AND schema_type = '0'
			AND doct_code = '{1}'
			AND valid_flag = '1'
			AND (spe_lmt > spe_reged OR append_flag = '1')
			ORDER BY noon_code,append_flag,begin_time*/
			#endregion

			string where = " WHERE see_date >= to_date('"+ current.Date.ToString() + "','yyyy-mm-dd hh24:mi:ss')" ;

			//�Ű�����
			where = where +" AND schema_type = '" + ((int)schemaType).ToString() + "'" ;
			
			
			if( schemaType == Neusoft.HISFC.Models.Base.EnumSchemaType.Dept)
			{
				where = where + " AND dept_code = '" + deptID + "'" +
					" AND end_time > to_date('"+current.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
					" AND valid_flag = '1'" +
					" ORDER BY see_date,noon_code,append_flag,begin_time" ;
			}
			else
			{
				//ԤԼ��ר��
				where  = where + " AND doct_code = '" + doctID + "'" +
					" AND end_time > to_date('"+current.ToString() + "','yyyy-mm-dd hh24:mi:ss')" +
					" AND valid_flag = '1'" +
					" ORDER BY see_date,noon_code,append_flag,begin_time" ;
			}			

			string sql = "" ;

			if(this.Sql.GetSql("Registration.Schema.Query.1",ref sql) == -1)return null;

			sql = sql + where ;

			if(this.QueryBase(sql) == null) return null ;

			if(al == null)return null ;

			if(al.Count == 0) return new Neusoft.HISFC.Models.Registration.Schema() ;

			return (Neusoft.HISFC.Models.Registration.Schema)al[0] ;
		}
        /// <summary>
        /// ����ģ��id����end_timeС�ڵ�ǰʱ��ļ�¼��
        /// by niuxy
        /// 2007-05-16
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int QuerySchemaById(string ID)
        {
            string strsql = "";
            int returnValue = this.Sql.GetSql("Registration.Schema.Query.98", ref strsql);
            if (returnValue == -1)
            {
                this.Err = "�������Ϊ[Registration.Schema.Query.98]��sql���ʧ��";
                return -1;
            }
            try
            {
                strsql = string.Format(strsql, ID);
            }
            catch (Exception ex)
            {
                this.Err = "[Registration.Schema.Query.98]��ʽ��ƥ��!" + ex.Message;
                this.ErrCode = ex.Message;
                return -1;    
                
            }

            return  Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(strsql));
           
        }

        /// <summary>
        /// Registration.SchemaWeek.1
        /// </summary>
        /// <returns></returns>
        public DataSet  QuerySchemaForRegister(string fromDate,string toDate,string schemaType,string deptCode,string doctCode )
        {

            DataSet ds = new DataSet ();
            this.ExecQuery("Registration.SchemaForRegister.1", ref ds, fromDate, toDate, schemaType, deptCode, doctCode);
            return ds;

        }


        #region ֣������ {CBC63AD3-E539-40f3-92F4-D8F82AEA63DB}
        
        #endregion

        /// <summary>
        /// ��ѯ�Ű��¼
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="noonID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet QueryDoctSchemaForZZ(string schemaType, string fromDate, string toDate,string noonID)
        {
            DataSet ds = new DataSet();
            this.ExecQuery("ZZ.Registration.Schema.Doct.1",ref ds, schemaType,  fromDate, toDate,noonID);

            return ds;
        }



        #endregion

    }
}
