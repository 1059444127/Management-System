using System;
using System.Collections;
using System.Data;
using Neusoft.HISFC.Models.Fee.Item;
using Neusoft.FrameWork.Function;
using System.Collections.Generic;

namespace Neusoft.HISFC.BizLogic.Fee
{
	/// <summary>
	/// Item<br></br>
	/// [��������: ��ҩƷ��Ϣҵ����]<br></br>
	/// [�� �� ��: ����]<br></br>
	/// [����ʱ��: 2006-09-25]<br></br>
	/// <�޸ļ�¼ 
	///		�޸���='' 
	///		�޸�ʱ��='yyyy-mm-dd' 
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	public class Item : Neusoft.FrameWork.Management.Database 
	{   
		
		#region ˽�к���

        /// <summary>
        /// ͨ�������Ͳ�����÷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="sqlIndex">SQL����</param>
        /// <param name="args">�����б�</param>
        /// <returns>�ɹ� ��ҩƷ���� ʧ�� null</returns>
        private List<Undrug> QueryUndrugBySeq(string sqlIndex, params string[] args)
        {
            string sql = string.Empty;

            if (this.Sql.GetSql(sqlIndex, ref sql) == -1) 
            {
                this.Err = "û���ҵ�����Ϊ:" + sqlIndex + "��SQL���";

                return null;
            }

            try
            {
                sql = string.Format(sql, args);
            }
            catch (Exception ex) 
            {
                this.Err = ex.Message;

                return null;
            }

            return this.GetItemsBySqlList(sql);
        }

		/// <summary>
		/// ȡ��ҩƷ������Ϣ����
		/// </summary>
		/// <param name="sql">��ǰSql���</param>
		/// <returns>�ɹ����ط�ҩƷ���� ʧ�ܷ���null</returns>
		private ArrayList GetItemsBySql(string sql)
		{
			ArrayList items = new ArrayList(); //���ڷ��ط�ҩƷ��Ϣ������
			
			//ִ�е�ǰSql���
			if (this.ExecQuery(sql) == -1)
			{
				this.Err = this.Sql.Err;

				return null;
			}
			
			try
			{
				//ѭ����ȡ����
				while (this.Reader.Read())
				{   
					Undrug item = new Undrug();
					
					item.ID = this.Reader[0].ToString();//��ҩƷ���� 
					item.Name = this.Reader[1].ToString(); //��ҩƷ���� 
					item.SysClass.ID = this.Reader[2].ToString(); //ϵͳ���
					item.MinFee.ID = this.Reader[3].ToString();  //��С���ô��� 
					item.UserCode = this.Reader[4].ToString(); //������
					item.SpellCode = this.Reader[5].ToString(); //ƴ����
					item.WBCode = this.Reader[6].ToString();    //�����
					item.GBCode = this.Reader[7].ToString();    //���ұ���
					item.NationCode = this.Reader[8].ToString();//���ʱ���
					item.Price = NConvert.ToDecimal(this.Reader[9].ToString()); //Ĭ�ϼ�
					item.PriceUnit = this.Reader[10].ToString();  //�Ƽ۵�λ
					item.FTRate.EMCRate = NConvert.ToDecimal(this.Reader[11].ToString()); // ����ӳɱ���
					item.IsFamilyPlanning = NConvert.ToBoolean(this.Reader[12].ToString()); // �ƻ�������� 
					item.User01 = this.Reader[13].ToString(); //�ض�������Ŀ
					item.Grade  = this.Reader[14].ToString();//�������־
					item.IsNeedConfirm = NConvert.ToBoolean(this.Reader[15].ToString());//ȷ�ϱ�־ 1 ��Ҫȷ�� 0 ����Ҫȷ��
					item.ValidState = this.Reader[16].ToString(); //��Ч�Ա�ʶ ���� 1 ͣ�� 0 ���� 2   
					item.Specs = this.Reader[17].ToString(); //���
					item.ExecDept = this.Reader[18].ToString();//ִ�п���
					item.MachineNO = this.Reader[19].ToString(); //�豸��� �� | ���� 
					item.CheckBody = this.Reader[20].ToString(); //Ĭ�ϼ�鲿λ��걾
					item.OperationInfo.ID = this.Reader[21].ToString(); // �������� 
					item.OperationType.ID = this.Reader[22].ToString(); // ��������
					item.OperationScale.ID = this.Reader[23].ToString(); //������ģ 
					item.IsCompareToMaterial = NConvert.ToBoolean(this.Reader[24].ToString());//�Ƿ���������Ŀ��֮����(1�У�0û��) 
					item.Memo = this.Reader[25].ToString(); //��ע  
					item.ChildPrice = NConvert.ToDecimal(this.Reader[26].ToString()); //��ͯ��
					item.SpecialPrice = NConvert.ToDecimal(this.Reader[27].ToString()); //�����
					item.SpecialFlag = this.Reader[28].ToString(); //ʡ����
					item.SpecialFlag1 = this.Reader[29].ToString(); //������
					item.SpecialFlag2 = this.Reader[30].ToString(); //�Է���Ŀ
					item.SpecialFlag3 = this.Reader[31].ToString();// ������
					item.SpecialFlag4 = this.Reader[32].ToString();// ����		
					item.DiseaseType.ID = this.Reader[35].ToString(); //��������
					item.SpecialDept.ID = this.Reader[36].ToString();  //ר������
					item.MedicalRecord = this.Reader[37].ToString(); //  --��ʷ�����
					item.CheckRequest = this.Reader[38].ToString();//--���Ҫ��
					item.Notice = this.Reader[39].ToString();//--  ע������  
					item.IsConsent = NConvert.ToBoolean(this.Reader[40].ToString());
					item.CheckApplyDept = this.Reader[41].ToString();//������뵥����
					item.IsNeedBespeak = NConvert.ToBoolean(this.Reader[42].ToString());//�Ƿ���ҪԤԼ
					item.ItemArea = this.Reader[43].ToString();//��Ŀ��Χ
					item.ItemException = this.Reader[44].ToString();//��ĿԼ��
                    item.UnitFlag = this.Reader[45].ToString();// []��λ��ʶ
                    item.ApplicabilityArea = this.Reader[46].ToString();
					items.Add(item);
				}//ѭ������

				//�ر�Reader
				this.Reader.Close();
				
				return items;
			}
			catch (Exception e)
			{
				this.Err = "��÷�ҩƷ������Ϣ����" + e.Message;
				this.WriteErr();
				
				//�����û�йر�Reader �ر�֮
				if (!this.Reader.IsClosed)
				{
					this.Reader.Close();
				}

				items = null;

				return null;
			}	
		}

        /// <summary>
        /// ȡ��ҩƷ������Ϣ����
        /// </summary>
        /// <param name="sql">��ǰSql���</param>
        /// <returns>�ɹ����ط�ҩƷ���� ʧ�ܷ���null</returns>
        private List<Undrug> GetItemsBySqlList(string sql)
        {
            List<Undrug> items = new List<Undrug>(); //���ڷ��ط�ҩƷ��Ϣ������

            //ִ�е�ǰSql���
            if (this.ExecQuery(sql) == -1)
            {
                this.Err = this.Sql.Err;

                return null;
            }

            try
            {
                //ѭ����ȡ����
                while (this.Reader.Read())
                {
                    Undrug item = new Undrug();

                    item.ID = this.Reader[0].ToString();//��ҩƷ���� 
                    item.Name = this.Reader[1].ToString(); //��ҩƷ���� 
                    item.SysClass.ID = this.Reader[2].ToString(); //ϵͳ���
                    item.MinFee.ID = this.Reader[3].ToString();  //��С���ô��� 
                    item.UserCode = this.Reader[4].ToString(); //������
                    item.SpellCode = this.Reader[5].ToString(); //ƴ����
                    item.WBCode = this.Reader[6].ToString();    //�����
                    item.GBCode = this.Reader[7].ToString();    //���ұ���
                    item.NationCode = this.Reader[8].ToString();//���ʱ���
                    item.Price = NConvert.ToDecimal(this.Reader[9].ToString()); //Ĭ�ϼ�
                    item.PriceUnit = this.Reader[10].ToString();  //�Ƽ۵�λ
                    item.FTRate.EMCRate = NConvert.ToDecimal(this.Reader[11].ToString()); // ����ӳɱ���
                    item.IsFamilyPlanning = NConvert.ToBoolean(this.Reader[12].ToString()); // �ƻ�������� 
                    item.User01 = this.Reader[13].ToString(); //�ض�������Ŀ
                    item.Grade = this.Reader[14].ToString();//�������־
                    item.IsNeedConfirm = NConvert.ToBoolean(this.Reader[15].ToString());//ȷ�ϱ�־ 1 ��Ҫȷ�� 0 ����Ҫȷ��
                    item.ValidState = this.Reader[16].ToString(); //��Ч�Ա�ʶ ���� 1 ͣ�� 0 ���� 2   
                    item.Specs = this.Reader[17].ToString(); //���
                    item.ExecDept = this.Reader[18].ToString();//ִ�п���
                    item.MachineNO = this.Reader[19].ToString(); //�豸��� �� | ���� 
                    item.CheckBody = this.Reader[20].ToString(); //Ĭ�ϼ�鲿λ��걾
                    item.OperationInfo.ID = this.Reader[21].ToString(); // �������� 
                    item.OperationType.ID = this.Reader[22].ToString(); // ��������
                    item.OperationScale.ID = this.Reader[23].ToString(); //������ģ 
                    item.IsCompareToMaterial = NConvert.ToBoolean(this.Reader[24].ToString());//�Ƿ���������Ŀ��֮����(1�У�0û��) 
                    item.Memo = this.Reader[25].ToString(); //��ע  
                    item.ChildPrice = NConvert.ToDecimal(this.Reader[26].ToString()); //��ͯ��
                    item.SpecialPrice = NConvert.ToDecimal(this.Reader[27].ToString()); //�����
                    item.SpecialFlag = this.Reader[28].ToString(); //ʡ����
                    item.SpecialFlag1 = this.Reader[29].ToString(); //������
                    item.SpecialFlag2 = this.Reader[30].ToString(); //�Է���Ŀ
                    item.SpecialFlag3 = this.Reader[31].ToString();// ������
                    item.SpecialFlag4 = this.Reader[32].ToString();// ����		
                    item.DiseaseType.ID = this.Reader[35].ToString(); //��������
                    item.SpecialDept.ID = this.Reader[36].ToString();  //ר������
                    item.MedicalRecord = this.Reader[37].ToString(); //  --��ʷ�����
                    item.CheckRequest = this.Reader[38].ToString();//--���Ҫ��
                    item.Notice = this.Reader[39].ToString();//--  ע������  
                    item.IsConsent = NConvert.ToBoolean(this.Reader[40].ToString());
                    item.CheckApplyDept = this.Reader[41].ToString();//������뵥����
                    item.IsNeedBespeak = NConvert.ToBoolean(this.Reader[42].ToString());//�Ƿ���ҪԤԼ
                    item.ItemArea = this.Reader[43].ToString();//��Ŀ��Χ
                    item.ItemException = this.Reader[44].ToString();//��ĿԼ��
                    item.UnitFlag = this.Reader[45].ToString(); //[]��λ��ʶ
                    item.ApplicabilityArea = this.Reader[46].ToString();
                    items.Add(item);
                }//ѭ������

                //�ر�Reader
                this.Reader.Close();

                return items;
            }
            catch (Exception e)
            {
                this.Err = "��÷�ҩƷ������Ϣ����" + e.Message;
                this.WriteErr();

                //�����û�йر�Reader �ر�֮
                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }

                items = null;

                return null;
            }
        }

		/// <summary>
		/// ���update����insert��ҩƷ�ֵ��Ĵ����������
		/// </summary>
		/// <param name="undrug">��ҩƷʵ��</param>
		/// <returns>��������</returns>
		private string[] GetItemParams(Undrug undrug)
		{
			string[] args = 
			{	
				undrug.ID, 
				undrug.Name,
				undrug.SysClass.ID.ToString(),
				undrug.MinFee.ID.ToString(),
				undrug.UserCode, 
				undrug.SpellCode,
				undrug.WBCode,
				undrug.GBCode, 
				undrug.NationCode,
				undrug.Price.ToString(),
				undrug.PriceUnit,						
				undrug.FTRate.EMCRate.ToString(),
				NConvert.ToInt32(undrug.IsFamilyPlanning).ToString(),	
				"",  				        
				undrug.Grade,					
				NConvert.ToInt32(undrug.IsNeedConfirm).ToString(),
				Neusoft.FrameWork.Function.NConvert.ToInt32(undrug.ValidState).ToString(),		
				undrug.Specs,					
				undrug.ExecDept,					
				undrug.MachineNO,
				undrug.CheckBody,				
				undrug.OperationInfo.ID,                 
				undrug.OperationType.ID,			
				undrug.OperationScale.ID,
				NConvert.ToInt32(undrug.IsCompareToMaterial).ToString(),		
				undrug.Memo,					
				undrug.Oper.ID ,	
				undrug.ChildPrice.ToString(),            
				undrug.SpecialPrice.ToString(),         
				undrug.SpecialFlag,                   
				undrug.SpecialFlag1,                          
				undrug.SpecialFlag2,
				undrug.SpecialFlag3,                     
				undrug.SpecialFlag4,                  
				"0",     
                "0",
				undrug.DiseaseType.ID ,
				undrug.SpecialDept.ID,
				NConvert.ToInt32(undrug.IsConsent).ToString(),
				undrug.MedicalRecord,                        
				undrug.CheckRequest,                          
				undrug.Notice,					
				undrug.CheckApplyDept,
				NConvert.ToInt32(undrug.IsNeedBespeak).ToString(),
			    undrug.ItemArea,
			    undrug.ItemException,
                undrug.UnitFlag,/*[2007/01/19]��ӵ��ֶ�,��λ��ʶ46*/
                undrug.ApplicabilityArea
			};

			return args;
		}

		/// <summary>
		/// ͨ����Ŀ����������ű�Ϊ0��Ԫ��,ת���ɷ�ҩƷʵ��
		/// </summary>
		/// <param name="items">��ҩƷ��Ŀ����</param>
		/// <returns>�ɹ����ط�ҩƷʵ��,ʧ�ܷ���null</returns>
		private Undrug GetItemFromArrayList(ArrayList items)
		{
			//����������Ϊ��,˵��sql��������ԭ���������
			if (items == null)
			{
				return null;
			}
			//�����õ�����Ԫ��������0,˵�����ҵ�����Ŀ,������ֻ����һ��Ԫ��
			//����ȡ�ű�Ϊ0��Ԫ��,ת��Undrugʵ��
			if (items.Count > 0)
			{	
				Undrug tempUndrug = items[0] as Undrug;
				
				return tempUndrug;
			}
			else//���Ԫ��������0(������С��0),˵���˱���ķ�ҩƷ��Ŀ������
			{
				return null;
			}
		}

        /// <summary>
        /// ͨ����Ŀ����������ű�Ϊ0��Ԫ��,ת���ɷ�ҩƷʵ��
        /// </summary>
        /// <param name="items">��ҩƷ��Ŀ����</param>
        /// <returns>�ɹ����ط�ҩƷʵ��,ʧ�ܷ���null</returns>
        private Undrug GetItemFromList(List<Undrug> items)
        {
            //����������Ϊ��,˵��sql��������ԭ���������
            if (items == null)
            {
                return null;
            }
            //�����õ�����Ԫ��������0,˵�����ҵ�����Ŀ,������ֻ����һ��Ԫ��
            //����ȡ�ű�Ϊ0��Ԫ��,ת��Undrugʵ��
            if (items.Count > 0)
            {
                return items[0];
            }
            else//���Ԫ��������0(������С��0),˵���˱���ķ�ҩƷ��Ŀ������
            {
                return null;
            }
        }

		#endregion
		
		#region ���к���
		
		/// <summary>
		/// �жϸ���Ŀ�Ƿ��Ѿ�ʹ�ù�
		/// </summary>
		/// <param name="undrugCode">��ҩƷ����</param>
		/// <returns>true �Ѿ�ʹ�� false û��ʹ��</returns>
		public bool IsUsed(string undrugCode)
		{
			string sql = null; //���ص�SQL���
			string returnRows = null; //�������Ѿ�ʹ�õĵ�ǰ��ҩƷ��Ŀ
			bool isUsed = false; //�Ƿ����ɾ��

			//��õ�ǰ��ҩƷ��ʹ�ô���SQL���
			if (this.Sql.GetSql("Fee.Item.CanDelete.Select", ref sql) == -1)
			{
				this.Err = "û���ҵ�Fee.Item.CanDelete.Select�ֶ�";

				return false;
			}
			
			//��ʽ��SQL���
			try
			{
				sql = string.Format(sql, undrugCode);
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return false;
			}
			
			//��õ�ǰ��ҩƷ��ʹ�ô���
			returnRows = this.ExecSqlReturnOne(sql);
			
			//���������Ŀ����0,�÷�ҩƷ�Ѿ�ʹ��
			if (NConvert.ToInt32(returnRows) > 0 )
			{
				isUsed = true;
			}
			else//���ص���ĿС�ڵ���0 ˵������Ŀû��ʹ��
			{
				isUsed = false;
			}
			
			return isUsed;
		}
		
		/// <summary>
		/// ���ղ�ѯ������÷�ҩƷ��Ϣ�б�
		/// </summary>
		/// <param name="undrugCode">���Ϊ��ҩƷ����Ϊ��ѯ��һ��Ŀ,Ϊ�ַ���"all"ʱΪ��ѯ������Ŀ</param>
		/// <param name="validState">��ҩƷ״̬: ����(1) ͣ��(0) ����(2) ����(all)</param>
		/// <returns>�ɹ�:���ط�ҩƷʵ������ ʧ��:����null</returns>
		public ArrayList Query(string undrugCode, string validState)
		{
			string sql = string.Empty; //���ȫ����ҩƷ��Ϣ��SELECT���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.Info", ref sql) == -1)
			{
				this.Err = "û���ҵ�Fee.Item.Info�ֶ�!";
				this.WriteErr();

				return null;
			}
			//��ʽ��SQL���
			try
			{
				sql = string.Format(sql, undrugCode, validState);
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return null;
			}

			//����SQL���ȡ��ҩƷ�����鲢��������
			return this.GetItemsBySql(sql);
		}

        /// <summary>
        /// ���ղ�ѯ������÷�ҩƷ��Ϣ�б�
        /// </summary>
        /// <param name="undrugCode">���Ϊ��ҩƷ����Ϊ��ѯ��һ��Ŀ,Ϊ�ַ���"all"ʱΪ��ѯ������Ŀ</param>
        /// <param name="validState">��ҩƷ״̬: ����(1) ͣ��(0) ����(2) ����(all)</param>
        /// <returns>�ɹ�:���ط�ҩƷʵ������ ʧ��:����null</returns>
        public List<Undrug> QueryList(string undrugCode, string validState)
        {
            string sql = string.Empty; //���ȫ����ҩƷ��Ϣ��SELECT���

            //ȡSELECT���
            if (this.Sql.GetSql("Fee.Item.Info", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.Info�ֶ�!";
                this.WriteErr();

                return null;
            }
            //��ʽ��SQL���
            try
            {
                sql = string.Format(sql, undrugCode, validState);
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                return null;
            }

            //����SQL���ȡ��ҩƷ�����鲢��������
            return this.GetItemsBySqlList(sql);
        }
        /// <summary>
        /// ��ѯ�����շ���Ŀ
        /// </summary>
        /// <param name="dept">����</param>
        /// <returns></returns>
        public List<Undrug> QueryList(string dept)
        {
            string sql = string.Empty; //���ȫ����ҩƷ��Ϣ��SELECT���

            //ȡSELECT���
            if (this.Sql.GetSql("Fee.Item.GetDeptAlwaysUsedItemUndrug", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.Info�ֶ�!";
                this.WriteErr();

                return null;
            }
            //��ʽ��SQL���
            try
            {
                sql = string.Format(sql,dept);
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                return null;
            }

            //����SQL���ȡ��ҩƷ�����鲢��������
            return this.GetItemsBySqlList(sql);
        }

		/// <summary>
		/// ���ݷ�ҩƷ�����ø���Ŀ��Ϣ(����Ŀ������Ч)
		/// </summary>
		/// <param name="undrugCode">��ҩƷ����</param>
		/// <returns>�ɹ�:���ط�ҩƷʵ�� ʧ��:����null</returns>
		public Undrug GetValidItemByUndrugCode(string undrugCode)
		{
            return this.GetItemFromList(this.QueryUndrugBySeq("Fee.Item.ValidItem", undrugCode, "1"));
		}

        /// <summary>
        /// ��÷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="undrugCode"></param>
        /// <returns>�ɹ� ��ҩƷ��Ϣ ʧ�� null</returns>
        public Undrug GetUndrugByCode(string undrugCode)
        {
            //���ݱ�������Ч����Ŀ��Ϣ
            ArrayList items = this.Query(undrugCode, "all");

            //����������Ϊ��,˵��sql��������ԭ���������
            if (items == null)
            {
                return null;
            }

            return this.GetItemFromArrayList(items);
        }

		/// <summary>
		/// �����Զ�������÷�ҩƷ��Ϣ
		/// </summary>
		/// <param name="userCode">��Ŀ�Զ�����</param>
		/// <returns>�ɹ����ط�ҩƷ��Ŀʵ�� ʧ�ܷ���null</returns>
		public Undrug GetItemByUserCode(string userCode)
		{
			string sql = null;//SQL���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.Info.UserCode", ref sql) == -1)
			{
				this.Err = "û���ҵ�Fee.Item.UserCode�ֶ�!";

				return null;
			}
			//��ʽ��SQL���
			try
			{
				sql = string.Format(sql, userCode);
			}
			catch(Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return null;
			}

			//����SQL���ȡ��ҩƷ�����鲢��������
			ArrayList items = this.GetItemsBySql(sql);

			return this.GetItemFromArrayList(items);
		}
		
		/// <summary>
		/// �����Ч��,��Ŀ���Ϊ��������Ŀ����
		/// </summary>
		/// <returns>�ɹ�:��Ŀ���� ʧ�ܷ���null</returns>
		public ArrayList QueryOperationItems()
		{
			string sql = null;//SQL���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.GetOperationItemList", ref sql) == -1)
			{
				this.Err = "û���ҵ�Fee.Item.GetOperationItemList�ֶ�!";

				return null;
			}

			//����SQL���ȡ��ҩƷ�����鲢��������
			return this.GetItemsBySql(sql);
		}
		
		/// <summary>
		/// ������п��ܵ���Ŀ��Ϣ
		/// </summary>
		/// <returns>�ɹ� ��Ч�Ŀ�����Ŀ��Ϣ, ʧ�� null</returns>
		public ArrayList QueryValidItems()
		{
			return this.Query("all", "1");
		}

        /// <summary>
        /// ������п��ܵ���Ŀ��Ϣ
        /// </summary>
        /// <returns>�ɹ� ��Ч�Ŀ�����Ŀ��Ϣ, ʧ�� null</returns>
        public List<Undrug> QueryValidItemsList()
        {
            return this.QueryList("all", "1");
        }

        /// <summary>
        /// ��ÿ������п��ܵ���Ŀ��Ϣ
        /// </summary>
        /// <returns>�ɹ� ��Ч�Ŀ�����Ŀ��Ϣ, ʧ�� null</returns>
        public List<Undrug> QueryValidItemsList(string dept)
        {
            return this.QueryList(dept);
        }

        /// <summary>
        /// ���������Ŀ��Ϣ
        /// </summary>
        /// <returns>�ɹ� ������Ŀ��Ϣ, ʧ�� null</returns>
        public List<Undrug> QueryAllItemList()
        {
            return this.QueryList("all", "all");
        }
		
		/// <summary>
		/// ���ȫ�����÷�ҩƷ��Ϣ�������Ŀ��Ϣ
		/// </summary>
		/// <returns>�ɹ�:ȫ�����÷�ҩƷ��Ϣ�������Ŀ��Ϣ ʧ��: null</returns>
		public ArrayList GetAvailableListWithGroup()
		{
			string sql = null; //���ȫ����ҩƷ��Ϣ��SELECT���
			ArrayList items = new ArrayList(); //���ڷ��ط�ҩƷ��Ϣ������
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.Info.GetAvailableListWithGroup", ref sql) == -1)
			{
				this.Err = "û���ҵ�����Ϊ:Fee.Item.Undrug.Info.GetAvailableListWithGroup��Sql���!";

				return null;
			}
		
			//���ִ�в�ѯSQL���,��ô����null
			if (this.ExecQuery(sql) == -1)
			{
				return null;
			}

			try
			{
				//ѭ���������
				while (this.Reader.Read())
				{   
					Undrug item = new Undrug();//��ʱ��ҩƷ��Ϣ

					item.ID = this.Reader[0].ToString();
					item.Name = this.Reader[1].ToString();
					item.SysClass.ID = this.Reader[2].ToString();
					item.UserCode = this.Reader[3].ToString();
					item.SpellCode = this.Reader[4].ToString();
					item.WBCode = this.Reader[5].ToString();
					item.Price = NConvert.ToDecimal(this.Reader[6].ToString());
					item.PriceUnit = this.Reader[7].ToString();
					item.IsNeedConfirm = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[8].ToString());
					item.ExecDept = this.Reader[9].ToString();
					item.MachineNO  = this.Reader[10].ToString();
					item.CheckBody=this.Reader[11].ToString(); 
					item.Memo = this.Reader[12].ToString();
					item.DiseaseType.ID = this.Reader[13].ToString(); 
					item.SpecialDept.ID = this.Reader[14].ToString(); 
					item.MedicalRecord = this.Reader[15].ToString();
					item.CheckRequest = this.Reader[16].ToString();
					item.Notice = this.Reader[17].ToString();
					item.Grade = this.Reader[18].ToString();//-- ���
					
					items.Add(item);
				}//ѭ������

				this.Reader.Close();
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();
				
				//���Readerû�йر�,�ر�֮
				if (!this.Reader.IsClosed)
				{
					this.Reader.Close();
				}

				items = null;
				
				return null;
			}

			return items;
		}

		/// <summary>
		/// ����µķ�ҩƷ����
		/// </summary>
		/// <returns>�µķ�ҩƷ����</returns>
		public string GetUndrugCode()
		{
			string tempUndrugCode = null;//��ʱ��ҩƷ����
			string sql = null;//SQL���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.UndrugCode", ref sql) == -1)
			{
				this.Err = "��÷�ҩƷ��ˮ�Ų�ѯ�ֶ�Fee.Item.UndrugCode����!";

				return null;
			}
			
			tempUndrugCode = this.ExecSqlReturnOne(sql);

			tempUndrugCode = "F" + tempUndrugCode.PadLeft(11, '0');

			return tempUndrugCode;
		}

		/// <summary>
		/// ���ҩƷ�ֵ��(fin_com_undruginfo)�в���һ����¼
		/// </summary>
		/// <param name="item">��ҩƷʵ��</param>
		/// <returns>�ɹ� 1 ʧ�� -1</returns>
		public int InsertUndrugItem(Undrug item)
		{
			string sql = null; //����fin_com_undruginfo��SQL���

			if (this.Sql.GetSql("Fee.Item.InsertItem", ref sql)==-1) 
			{
				this.Err = "�������Ϊ:Fee.Item.InsertItem��SQL���ʧ��!";

				return -1;
			}
			//��ʽ��SQL���
			try
			{  
				//ȡ�����б�
				string[] parms = this.GetItemParams(item);  
				//�滻SQL����еĲ�����
				sql = string.Format(sql, parms);   
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return -1;
			}

			return this.ExecNoQuery(sql);
		}
		
		/// <summary>
		/// ���·�ҩƷ��Ϣ���Է�ҩƷ����Ϊ����
		/// </summary>
		/// <param name="item">��ҩƷʵ��</param>
		/// <returns>�ɹ� 1 ʧ�� -1 ,δ���µ����� 0</returns>
		public int UpdateUndrugItem(Undrug item)
		{
			string sql = null; //����fin_com_undruginfo��SQL���

			if (this.Sql.GetSql("Fee.Item.UpdateItem", ref sql) == -1)
			{
				this.Err = "�������Ϊ:Fee.Item.UpdateItem��SQL���ʧ��!";

				return -1;
			}
			//��ʽ��SQL���
			try
			{  
				//ȡ�����б�
				string[] parms = GetItemParams(item);
				//�滻SQL����еĲ�����
				sql = string.Format(sql, parms);    
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return -1;
			}

			return this.ExecNoQuery(sql);
		}

		/// <summary>
		/// ɾ����ҩƷ��Ϣ
		/// </summary>
		/// <param name="undrugCode">��ҩƷ����</param>
		/// <returns>�ɹ� 1 ʧ�� -1 δɾ�������� 0</returns>
		public int DeleteUndrugItemByCode(string undrugCode)
		{
			string sql = null; //���ݷ�ҩƷ����ɾ��ĳһ��ҩƷ��Ϣ��DELETE���

			if (this.Sql.GetSql("Fee.Item.DeleteItem", ref sql) == -1)
			{
				this.Err = "�������Ϊ:Fee.Item.DeleteItem��SQL���ʧ��!";

				return -1;
			}
			//��ʽ��SQL���
			try
			{
				sql = string.Format(sql, undrugCode);
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return -1;
			}

			return this.ExecNoQuery(sql);
		}

		/// <summary>
		/// ��ҩƷ����ר�� ��ʱ���������Ч�� ����������� ��ֻ���·�ҩƷ�� Ĭ�ϼ� ����ͯ�ۣ� �����
		/// </summary>
		/// <param name="item">�۸�仯��ķ�ҩƷʵ��</param>
		/// <returns>�ɹ� 1 ʧ�� -1 δ���µ����� 0</returns>
		public int AdjustPrice(Undrug item)
		{
			string sql = null; //����SQL���

			if (this.Sql.GetSql("Fee.Item.ItemPriceSave", ref sql) == -1)
			{
				this.Err = "�������Ϊ:Fee.Item.ItemPriceSave��SQL���ʧ��!";

				return -1;
			}
			//��ʽ��SQL���
			try
			{
				//�滻SQL����еĲ�����
				sql = string.Format(sql, item.ID, item.Price, item.ChildPrice, item.SpecialPrice); 
			}
			catch (Exception e)
			{
				this.Err = e.Message;
				this.WriteErr();

				return -1;
			}

            //{58010499-3CA3-4b9d-B537-BBF964F8EB8B}  ���ݱ��ε�����Ŀ���°����˸���ϸ��Ŀ�ĸ�����Ŀ�۸�
            if (this.ExecNoQuery(sql) == -1)
            {
                return -1;
            }

            return this.AdjustZTPrice(item);
		}

        /// <summary>
        /// ��ҩƷ����ʱ ���ݵ��۵ķ�ҩƷ������صĸ�����Ŀ�۸�
        /// 
        /// {58010499-3CA3-4b9d-B537-BBF964F8EB8B}  ���ݱ��ε�����Ŀ���°����˸���ϸ��Ŀ�ĸ�����Ŀ�۸�
        /// </summary>
        /// <param name="adjustPriceItem">�۸�仯��ķ�ҩƷʵ��</param>
        /// <returns>�ɹ�1 ʧ��-1 </returns>
        public int AdjustZTPrice(Undrug adjustPriceItem)
        {
            if (adjustPriceItem.UnitFlag == "1")            //������Ŀ����Ҫ���к�������
            {
                return 1;
            }

            List<Neusoft.FrameWork.Models.NeuObject> ztList = this.QueryZTListByDetailItem(adjustPriceItem);
            if (ztList == null)
            {
                return -1;
            }

            foreach (Neusoft.FrameWork.Models.NeuObject ztInfo in ztList)
            {
                if (this.UpdateZTPrice(ztInfo.ID) == -1)
                {
                    return -1;
                }
            }

            return 1;
        }

        /// <summary>
        /// ���ݸ�����Ŀ��ϸ���¼�����¸�����Ŀ�۸�
        /// 
        /// {58010499-3CA3-4b9d-B537-BBF964F8EB8B}  ���ݱ��ε�����Ŀ���°����˸���ϸ��Ŀ�ĸ�����Ŀ�۸�
        /// </summary>
        /// <param name="undrugZTCode">������Ŀ����</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected int UpdateZTPrice(string undrugZTCode)
        {
            string sql = string.Empty;
            if (this.Sql.GetSql("Fee.Item.UpdateZTPrice", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.UpdateZTPrice�ֶ�!";
                this.WriteErr();
                return -1;
            }

            sql = string.Format(sql, undrugZTCode);

            return this.ExecNoQuery(sql);
        }

        /// <summary>
        /// ���ݷ�ҩƷ��ϸ��Ŀ��ȡ�����˸���ϸ��Ŀ�ĸ�����Ŀ�б�
        /// 
        /// {58010499-3CA3-4b9d-B537-BBF964F8EB8B}  ���ݱ��ε�����Ŀ���°����˸���ϸ��Ŀ�ĸ�����Ŀ�۸�
        /// </summary>
        /// <param name="detailItem">��ҩƷ��ϸ��Ŀ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected List<Neusoft.FrameWork.Models.NeuObject> QueryZTListByDetailItem(Undrug detailItem)
        {
            string sql = string.Empty; //���ȫ������ƻ���SELECT���

            //ȡSELECT���
            if (this.Sql.GetSql("Fee.Item.QueryZTListByDetailItem", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.QueryZTListByDetailItem�ֶ�!";
                this.WriteErr();

                return null;
            }

            try
            {
                sql = string.Format(sql, detailItem.ID);

                if (this.ExecQuery(sql) == -1)
                {
                    return null;
                }

                List<Neusoft.FrameWork.Models.NeuObject> ztList = new List<Neusoft.FrameWork.Models.NeuObject>();
                while (this.Reader.Read())
                {
                    Neusoft.FrameWork.Models.NeuObject tempObj = new Neusoft.FrameWork.Models.NeuObject();

                    tempObj.ID = this.Reader[0].ToString();             //������Ŀ����
                    tempObj.Name = this.Reader[1].ToString();           //������Ŀ����

                    ztList.Add(tempObj);
                }

                return ztList;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
            finally
            {
                if (this.Reader != null && !this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }
            }
        }

        #region addby xuewj 2009-8-26 ִ�е����� ����Ŀά�� {0BB98097-E0BE-4e8c-A619-8B4BCA715001}
        /// <summary>
        /// ��ȡ���ڷ�ҩƷ��Ŀִ�е��еķ�ҩƷ��Ŀ
        /// </summary>
        /// <param name="nruseID">��ʿվ����</param>
        /// <param name="sysClass">ҽ�����</param>
        /// <param name="validState">��ҩƷ״̬: ����(1) ͣ��(0) ����(2) ����(all)</param>
        /// <returns></returns>
        public int QueryItemOutExecBill(string nruseID, string sysClass, string validState, ref DataSet ds)
        {
            string sql = string.Empty; //���ȫ����ҩƷ��Ϣ��SELECT���

            //ȡSELECT���
            if (this.Sql.GetSql("Fee.Item.Info.OutExecBill", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.Info�ֶ�!";
                this.WriteErr();

                return -1;
            }
            //��ʽ��SQL���
            try
            {
                sql = string.Format(sql, nruseID, sysClass, validState);
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                return -1;
            }

            //����SQL���ȡ��ҩƷ�����鲢��������
            return this.ExecQuery(sql, ref ds);
        }

        #endregion

        #endregion

        #region ��������

        /// <summary>
		/// ���·�ҩƷ��Ϣ���Է�ҩƷ����Ϊ����
		/// </summary>
		/// <param name="Item">��ҩƷ������Ϣ</param>
		/// 		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		[Obsolete("����,ʹ��UpdateUndrugItem() ע�ⷵ��TrueΪ�Ѿ�ʹ��,����ɾ��", true)]
		public int UpdateItem(Neusoft.HISFC.Models.Fee.Item.Undrug Item)
		{
			string strSQL="";
			if(this.Sql.GetSql("Fee.Item.Undrug.UpdateItem",ref strSQL)==-1) return -1;
			try
			{  
				string[] strParm = GetItemParams(Item);  //ȡ�����б�
				strSQL=string.Format(strSQL,strParm);    //�滻SQL����еĲ�����
			}
			catch(Exception ex)
			{
				this.Err="����ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}

		/// <summary>
		/// ���ȫ�����÷�ҩƷ��Ϣ
		/// </summary>
		/// <returns></returns>
		[Obsolete("����,ʹ��QueryValidItems() ע�ⷵ��TrueΪ�Ѿ�ʹ��,����ɾ��", true)]
		public ArrayList GetAvailableList()
		{
			return this.Query("all","0");
		}

		/// <summary>
		/// �жϸ���Ŀ�Ƿ��Ѿ�ʹ�ù������ʹ�ù�ֻ��ͣ�ã�����ɾ��
		/// </summary>
		/// <param name="strUndrugCode">��ҩƷ����</param>
		/// <returns></returns>
		[Obsolete("����,ʹ��IsUsed() ע�ⷵ��TrueΪ�Ѿ�ʹ��,����ɾ��", true)]
		public bool CanDelete( string strUndrugCode )
		{
			string strSQL = "";
			string returnRows = "";
			bool   canDelete = false;

			if( this.Sql.GetSql( "Fee.Item.Undrug.CanDelete.Select", ref strSQL ) == -1 )
			{
				this.Err = "û���ҵ�Fee.Item.Undrug.CanDelete.Select�ֶ�";
				return false;
			}
			try
			{
				strSQL = string.Format( strSQL, strUndrugCode );
			}
			catch
			{
				return false;
			}

			returnRows = this.ExecSqlReturnOne( strSQL );
			
			if( Neusoft.FrameWork.Function.NConvert.ToInt32( returnRows ) > 0 )
			{
				canDelete = false;
			}
			else
			{
				canDelete = true;
			}
			
			return canDelete;
		} 
		
		/// <summary>
		/// ���ȫ����ҩƷ��Ϣ�б�
		/// writed by zhouxs
		/// 2004-11-24
		/// </summary>
		/// <input>
		/// itemid ��ҩƷid ���id Ϊall ȫ�� 
		/// itemtype ��ҩƷtype 0 ���� 1 ͣ�� 2 ���� all 
		/// </input>
		/// <returns>��ҩƷ������</returns>
		[Obsolete("����,ʹ��Query()����", true)]
		public ArrayList GetList(string ID,string Type)
		{
			string strSelect ="";  //���ȫ����ҩƷ��Ϣ��SELECT���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.Info",ref strSelect) == -1)
			{
				this.Err="û���ҵ�Fee.Item.Undrug.Info�ֶ�!";
				return null;
			}
			try
			{
				strSelect = string.Format(strSelect,ID,Type);
			}
			catch
			{
				return null;
			}

			//����SQL���ȡ��ҩƷ�����鲢��������
			return this.GetItemsBySql(strSelect);
		}

		/// <summary>
		/// ���ݷ�ҩƷ�����ø���Ŀ��Ϣ(����Ŀ������Ч)
		/// </summary>
		/// <param name="undrugCode">��ҩƷ����</param>
		/// <returns></returns>
		[Obsolete("����,ʹ��GetValidItemByUndrugCode()����", true)]
		public Neusoft.HISFC.Models.Fee.Item.Undrug GetItem(string undrugCode)
		{
			ArrayList al =this.Query(ID,"0");
			if(al==null) return null;
			if(al.Count>0)
			{
				return al[0] as Neusoft.HISFC.Models.Fee.Item.Undrug;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// ��÷�ҩƷ��Ϣ
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		[Obsolete("����,ʹ��Query()����", true)]
		public Neusoft.HISFC.Models.Fee.Item.Undrug GetItemAll(string ID)
		{
			ArrayList al =this.Query(ID,"all");
			if(al==null) return null;
			if(al.Count>0)
			{
				return al[0] as Neusoft.HISFC.Models.Fee.Item.Undrug;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// �ӷ�ҩƷ�����ȡ����������Ϣ
		/// </summary>
		/// <returns></returns>
		[Obsolete("����,ʹ��QueryOperationItems()����", true)]
		public ArrayList GetOperationItemList()
		{
			string strSelect ="";  //���ȫ����ҩƷ��Ϣ��SELECT���
			
			//ȡSELECT���
			if (this.Sql.GetSql("Fee.Item.GetOperationItemList",ref strSelect) == -1)
			{
				this.Err="û���ҵ�Fee.Item.Undrug.GetOperationItemList�ֶ�!";
				return null;
			}
			//����SQL���ȡ��ҩƷ�����鲢��������
			return this.GetItemsBySql(strSelect);
		}

		/// <summary>
		/// ���ҩƷ�ֵ���в���һ����¼����ҩƷ�������oracle�е����к�
		/// </summary>
		/// <param name="Item">��ҩƷ������Ϣ</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		[Obsolete("����,ʹ��InsertUndrugItem()������������", true)]
		public int InsertItem(Neusoft.HISFC.Models.Fee.Item.Undrug Item)
		{
			string strSQL="";
			if(this.Sql.GetSql("Fee.Item.InsertItem",ref strSQL)==-1) return -1;
			try
			{  
				string[] strParm = GetItemParams(Item);  //ȡ�����б�
				strSQL=string.Format(strSQL,strParm);    //�滻SQL����еĲ�����
			}
			catch(Exception ex)
			{
				this.Err="����ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[Obsolete("����", true)]
		public int UpdateAndInsert( Neusoft.HISFC.Models.Fee.Item.Undrug item )
		{
			int i = 0;
			i = this.UpdateUndrugItem( item );

			if( i == 0 )
			{
				return this.InsertUndrugItem( item );
			}
			else if( i > 0 )
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
		/// <summary>
		/// ���÷�ҩƷ��Ϣ
		/// </summary>
		/// <param name="Item">��ҩƷ������Ϣ</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		[Obsolete("����", true)]
		public int SetItem(Neusoft.HISFC.Models.Fee.Item.Undrug Item)
		{
			if(Item.ID == null)  
			{
				if (InsertUndrugItem(Item) == -1) return -1;
				return 1;
			}
			if (this.UpdateUndrugItem(Item)== -1) return -1;
			return 1;
		}

		/// <summary>
		/// ��ҩƷ����ר�� ��ʱ���������Ч�� ����������� ��ֻ���·�ҩƷ�� Ĭ�ϼ� ����ͯ�ۣ� �����
		/// </summary>
		/// <param name="Item"></param>
		/// <returns></returns>
		[Obsolete("���� ,AdjustPrice()����", true)]
		public int ItemPriceSave(Neusoft.HISFC.Models.Fee.Item.Undrug Item )
		{
			try
			{
				string strSQL="";
				if(this.Sql.GetSql("Fee.Item.Undrug.ItemPriceSave",ref strSQL)==-1) return -1;
				strSQL=string.Format(strSQL,Item.ID,Item.Price,Item.ChildPrice,Item.SpecialPrice);    //�滻SQL����еĲ�����
				return this.ExecNoQuery(strSQL);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				return -1;
			}
			
		}
		
		/// <summary>
		/// ɾ����ҩƷ��Ϣ
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		[Obsolete("����,ʹ��DeleteUndrugItemByCode() ע�ⷵ��TrueΪ�Ѿ�ʹ��,����ɾ��", true)]
		public int DeleteItem(string ID)
		{
			string strSQL=""; //���ݷ�ҩƷ����ɾ��ĳһ��ҩƷ��Ϣ��DELETE���
			if(this.Sql.GetSql("Fee.Item.Undrug.DeleteItem",ref strSQL)==-1) return -1;
			try
			{
				strSQL=string.Format(strSQL,ID);
			}
			catch
			{
				this.Err="����������ԣ�Fee.Item.Undrug.DeleteItem";
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}

		/// <summary>
		/// ÿ���Զ��������޶�
		/// </summary>
		/// <returns></returns>
		[Obsolete("����,ʹ��DeleteUndrugItemByCode() ע�ⷵ��TrueΪ�Ѿ�ʹ��,����ɾ��", false)]
		public int ExecProcAddDayLimit() 
		{
			string strSQL = "";
			if (this.Sql.GetSql("Fee.Procedure.prc_DealWithPayKind03DayLimit", ref strSQL) == -1)  
			{
				this.Err = "�Ҳ����洢����prc_DealWithPayKind03DayLimit";
				return -1;
			}

			//ע�����ô洢���̣���Ϊû�з���ֵ�������Ӵ�ʱ�������strReturn=" "
			string strReturn = " ";			
			if (this.ExecEvent(strSQL, ref strReturn)== -1) 
			{
				this.Err=strReturn + "ִ�д洢���̳���!prc_DealWithPayKind03DayLimit" + this.Err;
				this.ErrCode = "PRC_GET_INVOICE";
				this.WriteErr();
				return -1;

			}
			return 1;
		}

		#endregion

        #region ������С���ò�ѯ�����Ŀ
        /// <summary>
        /// ������С���ò�ѯ�����Ŀ
        /// </summary>
        /// <param name="minFeeCode"></param>
        /// <returns></returns>
        public List<Undrug> QueryUndrugByMinFeeCode(string minFeeCode)
        {
            string sql = string.Empty; //���ȫ����ҩƷ��Ϣ��SELECT���
            string sqlwhere = string.Empty;
            //ȡSELECT���
            if (this.Sql.GetSql("Fee.Item.Info", ref sql) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.Info�ֶ�!";
                this.WriteErr();

                return null;
            }

            if (this.Sql.GetSql("Fee.Item.GetInfoByMinCode", ref sqlwhere) == -1)
            {
                this.Err = "û���ҵ�Fee.Item.GetInfoByMinCode�ֶ�!";
                this.WriteErr();

                return null;
            }
            sql += sqlwhere;
            //��ʽ��SQL���
            try
            {
                sql = string.Format(sql, "all", "1", minFeeCode);
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                this.WriteErr();

                return null;
            }
            //����SQL���ȡ��ҩƷ�����鲢��������
            return this.GetItemsBySqlList(sql);
        }

        #endregion
	}
}
