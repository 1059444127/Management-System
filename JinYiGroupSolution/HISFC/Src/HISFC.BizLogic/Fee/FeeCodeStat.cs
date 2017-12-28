using System;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.HISFC.Models.Fee;

namespace Neusoft.HISFC.BizLogic.Fee
{
	/// <summary>
	/// FeeCodeStat<br></br>
	/// [��������: ͳ�ƴ���ҵ����]<br></br>
	/// [�� �� ��: ����]<br></br>
	/// [����ʱ��: 2006-09-26]<br></br>
	/// <�޸ļ�¼ 
	///		�޸���='' 
	///		�޸�ʱ��='yyyy-mm-dd' 
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	public class FeeCodeStat : Neusoft.FrameWork.Management.Database
	{
		
		#region ˽�к���
		
		/// <summary>
		/// ���update����insertͳ�ƴ���Ĵ����������
		/// </summary>
		/// <param name="feeCodeStat">ͳ�ƴ���ʵ��</param>
		/// <returns>��������</returns>
		private string[] GetItemParams(Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat)
		{
			string[] args = 
			{
				feeCodeStat.ID,
                feeCodeStat.Name,
                feeCodeStat.ReportType.ID.ToString(),
				feeCodeStat.MinFee.ID,
				feeCodeStat.FeeStat.ID,
				feeCodeStat.StatCate.Name,
				feeCodeStat.StatCate.ID,
				feeCodeStat.ExecDept.ID,
				feeCodeStat.CenterStat,
				feeCodeStat.SortID.ToString(),
				((int)feeCodeStat.ValidState).ToString(),
				this.Operator.ID
			};
			
			return args;
		}
		
		
		/// <summary>
		/// ����SQL���Ͳ������ͳ�ƴ��༯��
		/// </summary>
		/// <param name="sql">SQL���</param>
		/// <param name="args">SQL������</param>
		/// <returns>�ɹ� ͳ�ƴ��༯�� ʧ��: null δ�ҵ�����: ����Ԫ����Ϊ0��ArrayList</returns>
		private ArrayList QueryFeeCodeStatsBySql(string sql, params string[] args)
		{
			ArrayList feeCodeStats = new ArrayList(); //���ô������� 
			
			//ִ��SQL���
			if (this.ExecQuery(sql, args) == -1)
			{
				return null;
			}
			
			try 
			{
				//ѭ����ȡ����
				while (this.Reader.Read())
				{					
					Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat = new Neusoft.HISFC.Models.Fee.FeeCodeStat();					
					
					feeCodeStat.Name = this.Reader[0].ToString();
					feeCodeStat.FeeStat.ID = this.Reader[1].ToString();
					feeCodeStat.MinFee.ID = this.Reader[2].ToString();
                    feeCodeStat.StatCate.Name = this.Reader[3].ToString();
					feeCodeStat.StatCate.ID = this.Reader[4].ToString();//ͳ�ƴ���
					feeCodeStat.ExecDept.ID = this.Reader[5].ToString();//ִ�п���
					feeCodeStat.CenterStat = this.Reader[6].ToString();//ҽ������ͳ�ƴ���
					feeCodeStat.SortID = NConvert.ToInt32(this.Reader[7].ToString());//��ӡ˳��
					feeCodeStat.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[8].ToString()));//��Ч�Ա�ʾ
					feeCodeStat.Oper.ID = this.Reader[9].ToString();//����Ա
					feeCodeStat.Oper.OperTime = NConvert.ToDateTime(this.Reader[10].ToString());//����ʱ��
					feeCodeStat.ID = this.Reader[11].ToString();//�������
                    feeCodeStat.ReportType.ID = this.Reader[12].ToString();
                        //(this.Reader[12].ToString()==string.Empty?this.Reader[12].ToString():string.Empty);
					feeCodeStat.ExecDept.Name =this.Reader[13].ToString();
					feeCodeStat.MinFee.Name = this.Reader[14].ToString(); //��С��������
					
					feeCodeStats.Add(feeCodeStat);
				}
			
				this.Reader.Close();
				
				return feeCodeStats;
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				
				if (!this.Reader.IsClosed)
				{
					this.Reader .Close();
				}

				feeCodeStats = null;
				
				return null;
			}
		}

		#endregion

		#region ���к���
		
		/// <summary>
		/// ���ݴ���ͳ�ƴ���ʵ���ѯͳ�ƴ�����ϸ
		///  </summary>
		/// <param name="feeCodeStat">ͳ�ƴ���ʵ��</param>
		/// <returns>�ɹ�: ͳ�ƴ������� ʧ��: null δ���ҵ����ݷ���Ԫ����Ϊ0��ArrayList</returns>
		public ArrayList QueryFeeCodeStatsByTypeAndName(Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat)
		{
			string sql = string.Empty;//��ѯSQL���
			
			//��������ʵ��Ϊnull,��ô���߱���ѯ����,ֱ�ӷ��ؿ�
			if (feeCodeStat == null)
			{
				this.Err = "�����FeeStatCode��Ϊnull";

				return null;
			}
			
			//�������Ĵ���ʵ�������Ϊ��,��ô���մ�������ѯ
			if (feeCodeStat.Name == string.Empty)
			{
				if (this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.2", ref sql) == -1)
				{
					this.Err = "û���ҵ�����Ϊ:Fee.FeeCodeStat.GetFeeCodeStat.2��SQL���";
		
					return null;
				}
				
				return this.QueryFeeCodeStatsBySql(sql, feeCodeStat.ReportType.ID.ToString());
			}
			else//�����մ��������������ϲ�ѯ
			{
				if (this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.1", ref sql) == -1)
				{
					this.Err = "û���ҵ�����Ϊ:Fee.FeeCodeStat.GetFeeCodeStat.1��SQL���";
		
					return null;
				}
				
				return this.QueryFeeCodeStatsBySql(sql, feeCodeStat.ReportType.ID.ToString(), feeCodeStat.Name);
			}
		}

		/// <summary>
		/// ���ݱ������õ���Ӧ�������ϸ����
		/// </summary>
		/// <param name="reportCode">�������</param>
		/// <returns>�ɹ�: ͳ�ƴ���ʵ������ ʧ��: null δ���ҵ����ݷ���Ԫ����Ϊ0��ArrayList</returns>
		public ArrayList QueryFeeCodeStatByReportCode(string reportCode)
		{
			string sql = string.Empty; //��ѯSQL���
		
			if (this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.3", ref sql) == -1)
			{
				this.Err = "û�в��ҵ�����Ϊ: Fee.FeeCodeStat.GetFeeCodeStat.3��SQL���";

				return null;
			}

			return this.QueryFeeCodeStatsBySql(sql, reportCode);
		}

		/// <summary>
		/// ���뵥��ͳ�ƴ���
		/// </summary>
		/// <param name="feeCodeStat">ͳ�ƴ���ʵ��</param>
		/// <returns></returns>
		public int InsertFeeCodeStat(Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat)
		{
			string sql = string.Empty;//����ͳ�ƴ����SQl���

			if (this.Sql.GetSql("Fee.FeeCodeStat.InsertFeeCodeStat",ref sql) == -1)
			{
				this.Err = "û�в��ҵ�����Ϊ: Fee.FeeCodeStat.InsertFeeCodeStat��SQL���";
				
				return -1;
			}
			
			return this.ExecNoQuery(sql, this.GetItemParams(feeCodeStat));
		}

		/// <summary>
		/// ����ͳ�ƴ���
		/// </summary>
		/// <param name="feeCodeStat">ͳ�ƴ���ʵ��</param>
		/// <returns>�ɹ� 1 ʧ�� -1 ,δ���µ����� 0</returns>
		public int UpdateFeeCodeStat(Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat)
		{
			string sql = string.Empty;//����ͳ�ƴ����SQl���

			if (this.Sql.GetSql("Fee.FeeCodeStat.UpdateFeeCodeStat",ref sql) == -1)
			{
				this.Err = "û�в��ҵ�����Ϊ: Fee.FeeCodeStat.UpdateFeeCodeStat��SQL���";
				
				return -1;
			}
			
			return this.ExecNoQuery(sql, this.GetItemParams(feeCodeStat));
		}

		/// <summary>
		/// ɾ��ͳ�ƴ�����Ϣ
		/// </summary>
		/// <param name="feeCodeStat">ͳ�ƴ���ʵ��</param>
		/// <returns>�ɹ� 1 ʧ�� -1 ,δɾ�������� 0</returns>
		public int DeleteFeeCodeStat(Neusoft.HISFC.Models.Fee.FeeCodeStat feeCodeStat)
		{
			string sql = string.Empty;//ɾ��ͳ�ƴ����SQl���

			if (this.Sql.GetSql("Fee.FeeCodeStat.DeleteFeeCodeStat",ref sql) == -1)
			{
				this.Err = "û�в��ҵ�����Ϊ: Fee.FeeCodeStat.DeleteFeeCodeStat��SQL���";
				
				return -1;
			}
			
			return this.ExecNoQuery(sql, feeCodeStat.ID, feeCodeStat.MinFee.ID, feeCodeStat.ReportType.ID.ToString());
		}

		#endregion

		#region ���Ϻ���
		
		/// <summary>
		/// ���ݴ��뷢Ʊ����ʵ���ѯ��Ʊ���ձ���ϸ
		///  </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		[Obsolete("����,ʹ��QueryFeeCodeStatsByTypeAndName()", true)]
		public ArrayList GetFeeCodeStat(Neusoft.HISFC.Models.Fee.FeeCodeStat obj)
		{
			ArrayList al=new ArrayList();
			string strSql="";
		
			try
			{
				if(obj.Name!="")
				{
					if(this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.1",ref strSql)==0)
					{
						strSql=string.Format(strSql,obj.ReportType,obj.Name);
					}
				}
				else
				{
					if(this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.2",ref strSql)==0)
					{
						strSql=string.Format(strSql,obj.ReportType);
					}
				}
			}
			catch(Exception ex)
			{
				this.Reader.Close();
				this.Err=ex.Message;
				this.ErrCode=ex.Message;
				return null;
			}
			try 
			{
				if(this.ExecQuery(strSql)==-1) return null;
				while(this.Reader.Read())
				{					
					Neusoft.HISFC.Models.Fee.FeeCodeStat objFee=new Neusoft.HISFC.Models.Fee.FeeCodeStat();					
					objFee.Name = this.Reader[0].ToString();
					objFee.FeeStat.ID = this.Reader[1].ToString();
					objFee.MinFee.ID = this.Reader[2].ToString();
					objFee.FeeStat.Name = this.Reader[3].ToString();
					objFee.StatCate.ID = this.Reader[4].ToString();//ͳ�ƴ���
					objFee.ExecDept.ID = this.Reader[5].ToString();//ִ�п���
					objFee.CenterStat = this.Reader[6].ToString();//ҽ������ͳ�ƴ���
					objFee.SortID = Convert.ToInt32(this.Reader[7].ToString());//��ӡ˳��
					objFee.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[8].ToString()));//��Ч�Ա�ʾ
					objFee.User01 = this.Reader[9].ToString();//����Ա
					objFee.User02 = this.Reader[10].ToString();//����ʱ��
					objFee.ID = this.Reader[11].ToString();//�������
					objFee.ReportType.ID = this.Reader[12].ToString();
					objFee.ExecDept.Name =this.Reader[13].ToString();
					objFee.User03 = this.Reader[14].ToString(); //��С��������
					al.Add(objFee);
					objFee = null;
				}
			
			
				this.Reader.Close();
				return al;
			}
			catch(Exception ex)
			{
				this.Err= ex.Message;
				this.ErrCode =ex.Message;
				al = null;
				return al;
			}
			finally
			{
				al = null;
			}

		}

		/// <summary>
		/// ���ݶ��ձ������õ���Ʊ���ձ��ѯ
		/// </summary>
		/// <param name="strReprotCode"></param>
		/// <returns></returns>
		[Obsolete("����,ʹ��QueryFeeCodeStatByReportCode()", true)]
		public ArrayList GetFeeCodeStat(string strReprotCode)
		{
			ArrayList al=new ArrayList();
			string strSql="";
		
			#region SQL
			//			select REPORT_NAME,	   -- �������� 
			//FEE_STAT_CODE	,	   -- ͳ�Ʒ��ô���
			//FEE_CODE,     --��С���ô���
			//FEE_STAT_NAME,	   -- ͳ������
			//FEE_STAT_CATE,	   -- ͳ�ƴ���
			//EXEDEPT_CODE,	   -- ִ�п���
			//CENTER_STATCODE,   -- ҽ������ͳ�ƴ���
			//PRINT_ORDER,	     --	��ӡ˳��
			//VALID_STATE,	     --��Ч�Ա�ʶ 0 ���� 1 ͣ�� 2 ����
			//OPER_CODE	,	       --����Ա
			//OPER_DATE,			     --����ʱ��
			//REPORT_TYPE,
			//REPORT_CODE
			//from fin_com_feecodestat
			//where 
			//PARENT_CODE	='[��������]'--	����ҽ�ƻ�������
			//and CURRENT_CODE ='[��������]'	--	����ҽ�ƻ�������
			
			//and REPORT_CODE = '{1}'	--	������� MZ01 ���﷢Ʊ ZY01 סԺ��Ʊ

				

			#endregion
			try
			{
				if(strReprotCode!="")
				{
					if(this.Sql.GetSql("Fee.FeeCodeStat.GetFeeCodeStat.3",ref strSql)==0)
					{
						strSql=string.Format(strSql,strReprotCode);
					}
				}
			}
			catch(Exception ex)
			{
				this.Err=ex.Message;
				this.ErrCode=ex.Message;
				return null;
			}
			if(this.ExecQuery(strSql)==-1) return null;
			try 
			{

				while(this.Reader.Read())
				{					
					Neusoft.HISFC.Models.Fee.FeeCodeStat objFee=new Neusoft.HISFC.Models.Fee.FeeCodeStat();					
					objFee.Name = this.Reader[0].ToString();
					objFee.FeeStat.ID = this.Reader[1].ToString();
					objFee.MinFee.ID = this.Reader[2].ToString();
					objFee.FeeStat.Name = this.Reader[3].ToString();
					objFee.StatCate.ID = this.Reader[4].ToString();//ͳ�ƴ���
					objFee.ExecDept.ID = this.Reader[5].ToString();//ִ�п���
					objFee.CenterStat = this.Reader[6].ToString();//ҽ������ͳ�ƴ���
					objFee.SortID = Convert.ToInt32(this.Reader[7].ToString());//��ӡ˳��
					objFee.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[8].ToString()));//��Ч�Ա�ʾ
					objFee.User01 = this.Reader[9].ToString();//����Ա
					objFee.User02 = this.Reader[10].ToString();//����ʱ��
					objFee.ID = this.Reader[11].ToString();//�������
					objFee.ReportType.ID = this.Reader[12].ToString();
					objFee.ExecDept.Name =this.Reader[13].ToString(); //ִ�п�������
					al.Add(objFee);
					objFee = null;
				}
				this.Reader.Close();
				return al;
			}
			catch(Exception ex)
			{
				this.ErrCode = ex.Message;
				this.Err     = ex.Message;
				al = null;
				return al;
			}
			finally
			{
				al = null;
			}
		}

		#endregion

        #region ���ݷ��ô����ѯ��С����

        /// <summary>
        /// ����ͳ�ƴ����ѯ��С����
        /// </summary>
        /// <param name="feeCode"></param>
        /// <returns></returns>
        public ArrayList QueryMiniFeeCode(string feeCode)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.FeeCodeStat.QueryMiniFeeCode", ref sql) == -1)
            {
                this.Err = "��ѯ����ΪFee.FeeCodeStat.QueryMiniFeeCode��SQLʧ��!";
                return null;
            }
            sql = string.Format(sql, feeCode);
            if (this.ExecQuery(sql) < 0)
            {
                return null;
            }
            ArrayList al = new ArrayList();
            while (this.Reader.Read())
            {
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                obj.ID = this.Reader[0].ToString();
                obj.Name = this.Reader[1].ToString();
                al.Add(obj);
            }
            return al;
        }

        #endregion
    }
}
