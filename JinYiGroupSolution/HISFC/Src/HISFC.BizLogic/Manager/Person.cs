using System;
using Neusoft.HISFC.Models;
using Neusoft.FrameWork.Models;
using System.Collections;
using Neusoft.FrameWork.Function;

namespace Neusoft.HISFC.BizLogic.Manager {
	/// <summary>
	/// Person ��ժҪ˵����
	/// ��Ա������
	/// </summary>
	public class Person:Neusoft.FrameWork.Management.Database {
		public Person() {
		}
		/// <summary>
		/// �����Ա�б�����Ա���ͷ��� ���� 1ҽ����2��ʿ��3�տ�Ա��4ҩʦ��5��ʦ��0������
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ArrayList GetEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType type) {
			#region �ӿ�˵��
			//��ø�������Ա�б�
			//Manager.Person.GetEmployee.1
			//���룺0 type ��Ա���� 
			//��������Ա��Ϣ
			#endregion
			string strSql="";
			if (this.Sql.GetSql("Manager.Person.GetEmployee.1",ref  strSql) == 0 ) {
				try {
					strSql= string.Format(strSql,type);
				}
				catch(Exception ex) {
					this.Err=ex.Message;
					this.ErrCode=ex.Message;
					return null;
				}
			}
			else {
				return null;
			}
			return this.myPersonQuery(strSql);
		}




		/// <summary>
		/// ������õ���Ա��Ϣͨ������ ͣ�ã������Ĳ�����ʾ
		/// </summary>
		/// <param name="deptcode"></param>
		/// <returns></returns>
		public ArrayList GetEmployee(string deptcode) {			
			return this.GetPersonsByDeptID(deptcode);
		}


		/// <summary>
		/// �����һ�ø�������Ա�б�
		/// </summary>
		/// <param name="type">��Ա���ͱ���</param>
		/// <param name="deptcode">���ұ���</param>
		/// <returns></returns>
		public ArrayList GetEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType type,string deptcode) {
			#region �ӿ�˵��
			//��ø�������Ա�б�
			//Manager.Person.GetEmployee.2
			//���룺0 type ��Ա���� ,1 dept ����id
			//��������Ա��Ϣ
			#endregion
			string strSql="";
			if (this.Sql.GetSql("Manager.Person.GetEmployee.2",ref  strSql) == 0 ) {
				try {
					strSql= string.Format(strSql,type,deptcode);
				}
				catch(Exception ex) {
					this.Err=ex.Message;
					this.ErrCode=ex.Message;
					return null;
				}
			}
			else {
				return null;
			}
			return this.myPersonQuery(strSql);
		}

        /// <summary>
        /// ����֯�ṹ��ȡ��Ա{D375AB84-33F8-4198-80BE-5245206E3077}
        /// </summary>
        /// <param name="type">��Ա���ͱ���</param>
        /// <param name="deptcode">���ұ���</param>
        /// <returns></returns>
        public ArrayList GetEmployeeByZhu(string deptcode)
        {
            #region �ӿ�˵��
            //��ø�������Ա�б�
            //Manager.Person.GetEmployee.2
            //���룺0 type ��Ա���� ,1 dept ����id
            //��������Ա��Ϣ
            #endregion
            string strSql = "";
            if (this.Sql.GetSql("Manager.Person.GetEmployee.zz", ref  strSql) == 0)
            {
                try
                {
                    strSql = string.Format(strSql, deptcode);
                }
                catch (Exception ex)
                {
                    this.Err = ex.Message;
                    this.ErrCode = ex.Message;
                    return null;
                }
            }
            else
            {
                return null;
            }
            return this.myPersonQuery(strSql);
        }
        /// <summary>
        /// ��ȡ�Ű��ר��
        /// </summary>
        /// <param name="type">��Ա���ͱ���</param>
        /// <param name="deptcode">���ұ���</param>
        /// <returns></returns>
        public ArrayList GetEmployeeForScama(Neusoft.HISFC.Models.Base.EnumEmployeeType type,string deptcode)
        {
            #region �ӿ�˵��
            //��ø�������Ա�б�
            //Manager.Person.GetEmployee.3
            //���룺0 type ��Ա���� ,1 dept ����id
            //��������Ա��Ϣ
            #endregion
            string strSql = "";
            if (this.Sql.GetSql("Manager.Person.GetEmployee.3", ref  strSql) == 0)
            {
                try
                {
                    strSql = string.Format(strSql, type, deptcode);
                }
                catch (Exception ex)
                {
                    this.Err = ex.Message;
                    this.ErrCode = ex.Message;
                    return null;
                }
            }
            else
            {
                return null;
            }
            return this.myPersonQuery(strSql);
        }


		/// <summary>
		/// ȡĳһ����վ�ڵ���Ա�б�
		/// </summary>
		/// <param name="nurseCellCode"></param>
		/// <returns></returns>
		public ArrayList GetNurse(string nurseCellCode) {
			#region �ӿ�˵��
			//��ø���ʿվ��ʿ�б�
			//Manager.Person.GetEmployee.2
			//���룺0 ��ʿվid
			//��������Ա��Ϣ
			#endregion
			string strSql="";
			string strWhere="";
			
			//ȡselect���
			if (this.Sql.GetSql("Manager.Person.GetEmployee.All",ref  strSql) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}

			//ȡwhere���
			if (this.Sql.GetSql("Manager.Person.GetEmployee.GetNurse",ref  strWhere) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}

			try {
				strSql= string.Format(strSql + " " + strWhere,nurseCellCode);
			}
			catch(Exception ex) {
				this.Err=ex.Message;
				this.ErrCode=ex.Message;
				return null;
			}
			return this.myPersonQuery(strSql);
		}


		/// <summary>
		/// �����һ�ó��˻�ʿ�ĸ�������Ա�б�
		/// </summary>
		/// <param name="deptCode"></param>
		/// <returns></returns>
		public ArrayList GetAllButNurse(string deptCode) {
			string strSql="";
			string strWhere="";
			
			//ȡselect���
			if (this.Sql.GetSql("Manager.Person.GetEmployee.All",ref  strSql) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}

			//ȡwhere���
			if (this.Sql.GetSql("Manager.Person.GetEmployee.GetAllButNurse",ref  strWhere) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}

			try {
				strSql= string.Format(strSql + " " + strWhere,deptCode);
			}
			catch(Exception ex) {
				this.Err=ex.Message;
				this.ErrCode=ex.Message;
				return null;
			}
			return this.myPersonQuery(strSql);
		}


		/// <summary>
		/// ȡȫ����Ա�б�
		/// </summary>
		/// <returns></returns>
		public ArrayList GetEmployeeAll() {
			#region �ӿ�˵��
			//��ø�������Ա�б�
			//Manager.Person.GetEmployee.All
			//���룺null
			//��������Ա��Ϣ
			#endregion
			string strSql="";
			if (this.Sql.GetSql("Manager.Person.GetEmployee.All",ref  strSql) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}
			return this.myPersonQuery(strSql);
		}


		/// <summary>
		/// ȡ��Ч��Ա�б���ȥͣ�úͷ����ġ�
		/// </summary>
		/// <returns></returns>
		public ArrayList GetUserEmployeeAll() {
			string strSql="";
			if (this.Sql.GetSql("Manager.Person.GetUserEmployeeAll.All",ref  strSql) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}
			return this.myPersonQuery(strSql);
		}


		/// <summary>
		/// ά�����ҷ���ʱ��ȡ��ǰ������û�е���Ա
		/// ��ԱȨ��ά��ʱ�õ�
		/// </summary>
		/// <param name="class1">���ҷ������</param>
		/// <param name="deptCode">���ұ���</param>
		/// <returns></returns>
		public ArrayList GetEmployeeForStat(string class1,string deptCode) {
			string strSql="";
			if (this.Sql.GetSql("Manager.Person.GetEmployeeForStat",ref  strSql) == -1 ) {
				this.Err = this.Sql.Err;
				return null;
			}
			
			try {
				strSql = string.Format(strSql,class1, deptCode);
			}
			catch (Exception ex) {
				this.Err = ex.Message;
				return null;
			}

			return this.myPersonQuery(strSql);
		}


		/// <summary>
		/// ������õ���Ա��Ϣͨ������ ͣ�ã�������Ҳ��ʾ
		/// </summary>
		/// <param name="deptID"></param>
		/// <returns></returns>
		public ArrayList GetPersonsByDeptIDAll(string deptID) {

			string sql = "";
			if(this.Sql.GetSql("Person.GetPersonsByDeptIDAll",ref sql)== -1)
				return null;

			try {
				sql=string.Format(sql,deptID);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return null;
			}

			if(this.ExecQuery(sql) == -1) {
				this.Reader.Close();
				return null;
			}
		    
//			if(this.Reader.HasRows == false)
//				return null;
	
			ArrayList persons = new ArrayList();
			try {
				while(this.Reader.Read()) {
					Neusoft.HISFC.Models.Base.Employee person = new Neusoft.HISFC.Models.Base.Employee();
					person.ID = this.Reader[0].ToString();
					person.Name = this.Reader[1].ToString();
					person.SpellCode = this.Reader[2].ToString();
					person.WBCode = this.Reader[3].ToString();				
					person.Sex.ID = this.Reader[4].ToString();
					person.Birthday = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[5].ToString());

					person.Duty.ID = this.Reader[6].ToString();
					person.Level.ID = this.Reader[7].ToString();
					person.GraduateSchool.ID = this.Reader[8].ToString();
					person.IDCard = this.Reader[9].ToString();
					person.Dept.ID = this.Reader[10].ToString();
					person.Nurse.ID = this.Reader[11].ToString();
					person.EmployeeType.ID = this.Reader[12].ToString();

					person.IsExpert = FrameWork.Function.NConvert.ToBoolean(Reader[13].ToString());
					person.IsCanModify =	FrameWork.Function.NConvert.ToBoolean(Reader[14].ToString());
					person.IsNoRegCanCharge = FrameWork.Function.NConvert.ToBoolean(this.Reader[15].ToString());
					person.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)NConvert.ToInt32(this.Reader[16]);
					person.SortID = FrameWork.Function.NConvert.ToInt32(this.Reader[17].ToString());
					person.Nurse.Name = this.Reader[18].ToString();

					persons.Add(person);
				}     
			}					
			catch(Exception ex) {
				this.Err="�����Ա������Ϣ����"+ex.Message;
				this.WriteErr();
				return null;
			}
			this.Reader.Close();
			return persons;
		}


		/// <summary>
		/// ���ݿ��ұ���ȡ��Ա�б�
		/// </summary>
		/// <param name="deptID">���ұ���</param>
		/// <returns></returns>
		public ArrayList GetPersonsByDeptID(string deptID) {

			string sql = "";
			if(this.Sql.GetSql("Person.SelectPersonsByDeptID",ref sql)== -1)
				return null;

			try {
				sql=string.Format(sql,deptID);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return null;
			}

			if(this.ExecQuery(sql) == -1) {
				this.Reader.Close();
				return null;
			}
		    
//			if(this.Reader.HasRows == false)
//				return null;
	
			ArrayList persons = new ArrayList();
			try {
				while(this.Reader.Read()) {
					Neusoft.HISFC.Models.Base.Employee person = new Neusoft.HISFC.Models.Base.Employee();
					person.ID = this.Reader[0].ToString();
					person.Name = this.Reader[1].ToString();
					person.SpellCode = this.Reader[2].ToString();
					person.WBCode = this.Reader[3].ToString();				
					person.Sex.ID = this.Reader[4].ToString();
					person.Birthday = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[5].ToString());

					person.Duty.ID = this.Reader[6].ToString();
					person.Level.ID = this.Reader[7].ToString();
					person.GraduateSchool.ID = this.Reader[8].ToString();
					person.IDCard = this.Reader[9].ToString();
					person.Dept.ID = this.Reader[10].ToString();
					person.Nurse.ID = this.Reader[11].ToString();
					person.EmployeeType.ID = this.Reader[12].ToString();

					person.IsExpert = FrameWork.Function.NConvert.ToBoolean(Reader[13].ToString());
					person.IsCanModify = FrameWork.Function.NConvert.ToBoolean(Reader[14].ToString());
					person.IsNoRegCanCharge = FrameWork.Function.NConvert.ToBoolean(this.Reader[15].ToString());
					person.ValidState = (HISFC.Models.Base.EnumValidState)NConvert.ToInt32(this.Reader[16].ToString());
					person.SortID = FrameWork.Function.NConvert.ToInt32(this.Reader[17].ToString());
					person.Nurse.Name = this.Reader[18].ToString();

					persons.Add(person);
				}     
			}
			catch(Exception ex) {
				this.Err="�����Ա������Ϣ����"+ex.Message;
				this.WriteErr();
				return null;
			}
			this.Reader.Close();
			return persons;
		}


		/// <summary>
		/// ͨ����Ա�������һ��߻�����Ϣ
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ArrayList GetPersonByName(string name) {
			string sql = "";
			if(this.Sql.GetSql("Person.GetPersonByName.Select.1", ref sql)== -1)
				return null;

			try {
				sql=string.Format(sql, name);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return null;
			}

//			if(this.ExecQuery(sql) == -1) {
//				this.Reader.Close();
//				return null;
//			}
//
//			if(this.Reader.HasRows == false)
//				return null;

			return this.myPersonQuery(sql);
		}


		/// <summary>
		/// ������Ա���룬ȡ��Ա��Ϣ
		/// </summary>
		/// <param name="personID"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Base.Employee GetPersonByID(string personID) {

			string sql = "";
			if(this.Sql.GetSql("Person.SelectByID",ref sql)== -1)
				return null;

			try {
				sql=string.Format(sql,personID);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return null;
			}

//			if(this.ExecQuery(sql) == -1) {
//				this.Reader.Close();
//				return null;
//			}

//			if(this.Reader.HasRows == false)
//				return null;

			ArrayList al = this.myPersonQuery(sql);
			if (al == null) return null;

			//����ҵ����ݣ��򷵻���Ա��Ϣ��û���ҵ��򷵻ؿյ�ʵ��
			if (al.Count == 0) 
				return new Neusoft.HISFC.Models.Base.Employee();
			else
                return al[0] as Neusoft.HISFC.Models.Base.Employee;


		}


		/// <summary>
        /// ������Ա��Ϣ//{6A8C59DC-91FE-4246-A923-06A011918614}
		/// </summary>
		/// <param name="info">����ʵ��</param>
		/// <returns></returns>
		public int Update(Neusoft.HISFC.Models.Base.Employee info) {
			
			string sql = "";
			if(this.Sql.GetSql("Person.UpdatePerson",ref sql)== -1)
				return -1;
			//	   UPDATE com_employee   --Ա�������
			//   	SET 	
			//	       empl_name='{1}',   --Ա������
			//	       spell_code='{2}',   --ƴ����
			//	       wb_code='{3}',   --���
			//	       sex_code='{4}',   --�Ա�
			//	       birthday='{5}',   --��������
			//	       posi_code='{6}',   --ְ�����
			//	       levl_code='{7}',   --ְ������
			//	       education_code='{8}',   --ѧ��
			//	       idenno='{9}',   --���֤��
			//	       dept_code='{10}',   --�������Һ�
			//	       nurse_cell_code='{11}',   --��������վ
			//	       empl_type='{12}',   --��Ա����
			//	       expert_flag='{13}',   --�Ƿ�ר��
			//	       modify_flag='{14}',   --�Ƿ����޸�Ʊ��Ȩ�� 1���� 0������
			//	       noregfee_flag='{15}',   --���Һž��շ�Ȩ�� 0 ������ 1����
			//	       valid_state='{16}',   --��Ч�Ա�־ 0 ��Ч 1 ͣ�� 2 ����
			//	       sort_id= '{17}'   --˳���
			//			  oper_code = '{18}',
			//	       oper_date = sysdate,
            //         user_code='{19}' 
			// 	WHERE 
			// 		empl_code='{0}'    --Ա������
		 
			try {
				sql=string.Format(sql,info.ID,info.Name,info.SpellCode,info.WBCode,
					info.Sex.ID,info.Birthday,info.Duty.ID,info.Level.ID,info.GraduateSchool.ID,
					info.IDCard,info.Dept.ID,info.Nurse.ID,info.EmployeeType.ID,
					FrameWork.Function.NConvert.ToInt32(info.IsExpert),
					FrameWork.Function.NConvert.ToInt32(info.IsCanModify),
					FrameWork.Function.NConvert.ToInt32(info.IsNoRegCanCharge),
					((int)info.ValidState).ToString(),info.SortID,this.Operator.ID,info.UserCode,info.Memo
					);
				
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

			if(this.ExecNoQuery(sql) == -1) return -1;


			return 1;
		}

		/// <summary>
        /// ������Ա��Ϣ//{6A8C59DC-91FE-4246-A923-06A011918614}
		/// </summary>
		/// <param name="info">����ʵ��</param>
		/// <returns></returns>
		public int Insert(Neusoft.HISFC.Models.Base.Employee info) {
			
			string sql = "";
			if(this.Sql.GetSql("Person.InsertPerson",ref sql)== -1)
				return -1;
			//			  INSERT INTO com_employee   --Ա�������
			//          ( empl_code,   --Ա������
			//            empl_name,   --Ա������
			//            spell_code,   --ƴ����
			//            wb_code,   --���
			//            sex_code,   --�Ա�
			//            birthday,   --��������
			//            posi_code,   --ְ�����
			//            levl_code,   --ְ������
			//            education_code,   --ѧ��
			//            idenno,   --���֤��
			//            dept_code,   --�������Һ�
			//            nurse_cell_code,   --��������վ
			//            empl_type,   --��Ա����
			//            expert_flag,   --�Ƿ�ר��
			//            modify_flag,   --�Ƿ����޸�Ʊ��Ȩ�� 1���� 0������
			//            noregfee_flag,   --���Һž��շ�Ȩ�� 0 ������ 1����
			//            valid_state,   --��Ч�Ա�־ 0 ��Ч 1 ͣ�� 2 ����
			//            sort_id )  --˳���
			//			  oper_code = '{18}',
            //            usercode='{19}'  
			//	       oper_date = sysdate
			//WHERE
			//		 
			try {
				sql=string.Format(sql,info.ID,info.Name,info.SpellCode,info.WBCode,
					info.Sex.ID,info.Birthday,info.Duty.ID,info.Level.ID,info.GraduateSchool.ID,
					info.IDCard,info.Dept.ID,info.Nurse.ID,info.EmployeeType.ID,
					FrameWork.Function.NConvert.ToInt32(info.IsExpert),
					FrameWork.Function.NConvert.ToInt32(info.IsCanModify),
					FrameWork.Function.NConvert.ToInt32(info.IsNoRegCanCharge),
					((int)info.ValidState).ToString(),info.SortID,this.Operator.ID,info.UserCode,info.Memo
					);
				
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

			if(this.ExecNoQuery(sql) == -1) return -1;


			return 1;
		}


		/// <summary>
		/// ɾ��һ����Ա��Ϣ
		/// </summary>
		/// <param name="personID"></param>
		/// <returns></returns>
		public int Delete(string personID) {
			string sql = "";
			if(this.Sql.GetSql("Person.DeletePerson",ref sql)== -1)
				return -1;

		 
			try {
				sql=string.Format(sql, personID);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

			if(this.ExecNoQuery(sql) == -1) return -1;

			return 1;
		}
		 

		/// <summary>
		/// ֻ������Ա����� 
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public int UpdateEmploySort(Neusoft.HISFC.Models.Base.Employee info)
		{
			string sql = "";
			if(this.Sql.GetSql("Person.UpdateEmploySort",ref sql)== -1) return -1;
			try 
			{
				sql=string.Format(sql,info.ID,info.SortID);
				
			}
			catch(Exception ex) 
			{
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

			if(this.ExecNoQuery(sql) == -1) return -1;
			return 1;
		}


		/// <summary>
		/// �жϵ�ǰ����Ա������û������
		/// </summary>
		/// <param name="EmployId"></param>
		/// <returns></returns>
		public int  SelectEmployIsExist(string EmployId) {
			int  IsExist=0;
			string strSql = "";
			if (this.Sql.GetSql("Manager.Person.SelectEmployIsExist",ref strSql)==-1) return -1;
			try {
				if(EmployId!="") {
					strSql = string.Format(strSql,EmployId);
					this.ExecQuery(strSql);
					while(this.Reader.Read()) {
						IsExist = 1;
					}
					this.Reader.Close();
				}
			}
			catch(Exception ee) {
				this.Err = ee.Message;
				IsExist = -1;
			}
			return IsExist;
		}

		
		/// <summary>
        /// ˽�к�������ѯ��Ա������Ϣ
		/// </summary>
		/// <param name="SQLPatient"></param>
		/// <returns></returns>
		private ArrayList myPersonQuery(string SQLPatient) {
			ArrayList al=new ArrayList();
			
			if (this.ExecQuery(SQLPatient) == -1) return null;
			try {
		
				while(this.Reader.Read()) {
					Neusoft.HISFC.Models.Base.Employee person = new Neusoft.HISFC.Models.Base.Employee();
					try {
						person.ID = this.Reader[0].ToString();
						person.Name = this.Reader[1].ToString();
						person.SpellCode = this.Reader[2].ToString();
						person.WBCode = this.Reader[3].ToString();				
						person.Sex.ID = this.Reader[4].ToString();
						person.Birthday =Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[5].ToString());

						person.Duty.ID = this.Reader[6].ToString();
						person.Level.ID = this.Reader[7].ToString();
						person.GraduateSchool.ID = this.Reader[8].ToString();
						person.IDCard = this.Reader[9].ToString();
						person.Dept.ID = this.Reader[10].ToString();
						person.Nurse.ID = this.Reader[11].ToString();
						person.EmployeeType.ID = this.Reader[12].ToString();

						person.IsExpert = FrameWork.Function.NConvert.ToBoolean(Reader[13].ToString());
						person.IsCanModify =	FrameWork.Function.NConvert.ToBoolean(Reader[14].ToString());
						person.IsNoRegCanCharge = FrameWork.Function.NConvert.ToBoolean(this.Reader[15].ToString());
                        person.ValidState = (HISFC.Models.Base.EnumValidState)NConvert.ToInt32( this.Reader[16].ToString() );
						person.SortID = FrameWork.Function.NConvert.ToInt32(this.Reader[17].ToString());
                        person.UserCode = this.Reader[18].ToString();
                        //{6A8C59DC-91FE-4246-A923-06A011918614}
                        person.Memo = this.Reader[19].ToString();
					}	
					catch(Exception ex) {
						this.Err="�����Ա������Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					al.Add(person);
				}
			}//�׳�����
			catch(Exception ex) {
				this.Err="�����Ա������Ϣ����"+ex.Message;
				this.ErrCode="-1";
				this.WriteErr();
				return null;
			}
			this.Reader.Close();
			return al;
		}

        /// <summary>
        /// ��ѯ���ݿ�����Ա����(com_employee)��������
        /// </summary>
        /// <returns>����string����MaxEmplID</returns>
        public string GetMaxEmployeeID()
        {
            string MaxEmplID = "";
            string strSql = "";
            if (this.Sql.GetSql("Manager.Person.GetMaxEmployeeID", ref strSql) == -1) return "";
            try
            {
                this.ExecQuery(strSql);
                if(this.Reader.Read())
                {
                    MaxEmplID = this.Reader[0].ToString();
                }
                this.Reader.Close();
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                MaxEmplID = "";
                this.WriteErr();
                return "";
            }
            return MaxEmplID;
        }


        #region donggq--20101118--{45E71A4E-803A-47fd-AC24-9BED6E530F16}--����ǩ��

        /// <summary>
        /// ͨ����Ա���ŷ�������ǩ��
        /// </summary>
        /// <param name="EmplID">��Ա����</param>
        /// <returns>�ɹ� ת��Byte[]���ͼƬ ʧ�� null</returns>
        public byte[] GetEmplDigitalSignByID(string EmplID)
        {
            string strSql = "";
            byte[] pic = null;
            if (this.Sql.GetSql("Person.GetPersonDigitalSignByID", ref strSql) == -1)
            {
                return null;
            }
            try
            {
                if (!string.IsNullOrEmpty(EmplID))
                {
                    strSql = string.Format(strSql, EmplID);
                    pic = db.OutputBlob(strSql);
                }
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return null;
            }
            return pic;
        }

        /// <summary>
        /// ������Ա��չ��Ϣ
        /// </summary>
        /// <param name="emp">��Աʵ��</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int InsertEmpExinfo(Neusoft.HISFC.Models.Base.Employee emp)
        {
            string strSql = "";
            if (this.Sql.GetSql("Person.InsertPersonExinfo", ref strSql) == -1)
            {
                return -1;
            }
            try
            {
                if (emp!=null)
                {
                    strSql = string.Format(strSql, emp.ID);
                    if(this.ExecNoQuery(strSql) == -1)
                    { 
                        this.Err = "����Ա��չ��Ϣ���в�������ʧ�ܣ�";
                        return -1;
                    }
                }
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return 1;
        } 

        /// <summary>
        /// ������Ա����ǩ������
        /// </summary>
        /// <param name="EmplID">��Ա����</param>
        /// <param name="EmplPic">ͼƬ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int UpdateEmplDigitalSignByID(string EmplID, byte[] EmplPic)
        {
            string strSql = "";
            if (this.Sql.GetSql("Person.UpdatePersonDigitalSignByID", ref strSql) == -1)
            {
                return -1;
            }
            try
            {
                if (!string.IsNullOrEmpty(EmplID))
                {
                    strSql = string.Format(strSql, EmplID);
                    if (db.InputBlob(strSql, EmplPic) == -1)
                    {
                        this.Err = "������Ƭ�ֶ�ʧ�ܣ�";
                        return -1;
                    }
                }
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// ɾ����Ա����ǩ������
        /// </summary>
        /// <param name="EmplID">��Ա����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int DeleEmplDigitalSignByID(string EmplID)
        {
            string strSql = "";
            if (this.Sql.GetSql("Person.DeletePersonDigitalSignByID", ref strSql) == -1)
            {
                return -1;
            }
            try
            {
                if (!string.IsNullOrEmpty(EmplID))
                {
                    strSql = string.Format(strSql, EmplID);
                    if (this.ExecNoQuery(strSql) == -1)
                    {
                        this.Err = "ɾ����Ա��չ��Ϣ��������ǩ��ʧ�ܣ�";
                        return -1;
                    }
                }
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return 1;
        } 

        #endregion



	}
}