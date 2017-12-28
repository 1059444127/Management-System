using System;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.HISFC.Models.Base;
namespace Neusoft.HISFC.BizLogic.Manager {
	/// <summary>
	/// Department ��ժҪ˵����
	/// </summary>
	public class Department:Neusoft.FrameWork.Management.Database {
		private string statCode = "01";	//���ҺͲ�����Ӧ��ϵ
		public Department() {
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}


		#region"����"	
        /// <summary>
        /// ��ȡ�Һ��б�
        /// </summary>
        /// <returns></returns>
	    
		public ArrayList GetRegDepartment()
		{
			string sqlSelect ="", sqlWhere="";

			if(this.Sql.GetSql("Department.SelectAllDept",ref sqlSelect)== -1)
				return null;


			if(this.Sql.GetSql("Department.Where.GetRegDept",ref sqlWhere)== -1)
				return null;

			sqlSelect = sqlSelect+" "+ sqlWhere;
			
			return this.myGetDepartment(sqlSelect) ;
		}

		/// <summary>
		/// �����������б�
		/// </summary>
		/// <returns></returns>
		public ArrayList GetClinicDepartment() 
		{
            return GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.C);
			//GetDeptment(DepartmentType.enuDepartmentType.C);

		}
		#endregion

		#region "סԺ"
		/// <summary>
		/// ���סԺ����
		/// </summary>
		/// <returns></returns>
		public ArrayList GetInHosDepartment() {
            return GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.I);
		}

		public ArrayList GetInHosDepartment(bool bTag) {
            return GetDeptmentIn(Neusoft.HISFC.Models.Base.EnumDepartmentType.I);
		}
		
		#endregion


		/// <summary>
		/// ���ݿ������ͻ�ÿ����б�
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>
		public ArrayList GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType Type) {
			string sqlSelect ="",sqlWhere="";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sqlSelect)== -1)
				return null;
			if(this.Sql.GetSql("Department.Where.2",ref sqlWhere)== -1)
				return null;
			sqlSelect = sqlSelect+" "+ sqlWhere;
			sqlSelect=string.Format(sqlSelect,Type);
			#region 
			//			Neusoft.HISFC.Models.Base.Department objDept;
			//			switch(type)
			//			{
			//				case 1://����
			//					//����sql���	
			//					/*SELECT DEPT_code,dept_name,DEPT_TYPE,'',DEPT_PY FROM r_department WHERE ( 
			//						R_DEPARTMENT.DEPT_TYPE ='0001' ) */
			//					if(this.Sql.GetSql("Department.1",ref sql)==-1) return alDept;
			//					break;
			//				case 2://סԺ
			//					//����sql���	
			//					/*SELECT DEPT_code,dept_name,DEPT_TYPE,'',DEPT_PY FROM r_department WHERE ( 
			//						R_DEPARTMENT.DEPT_TYPE ='0002' ) */
			//					if(this.Sql.GetSql("Department.2",ref sql)==-1) return alDept;
			//					break;
			//				default:break;
			//			}
			//			//ִ��sql���
			//			this.ProgressBarText="���ڲ���סԺ����...";
			//			this.ProgressBarValue=0;
			//			this.ExecQuery(sql);
			//			try
			//			{
			//				while (this.Reader.Read())
			//				{
			//					objDept=new Neusoft.HISFC.Models.Base.Department();
			//					objDept.ID=this.Reader[0].ToString();
			//					objDept.Name =this.Reader[1].ToString();
			//
			//					objDept.Class.ID =this.Reader[2].ToString();
			//					objDept.Class.Name=this.Reader[3].ToString();
			//					objDept.Spell.SpellCode=this.Reader[4].ToString();
			//					objDept.Memo=this.Reader[4].ToString();
			//					alDept.Add(objDept);
			//					this.ProgressBarValue++;
			//				}
			//				this.Reader.Close();
			//			}//�׳�����
			//			catch(Exception ex)
			//			{
			//				this.Err=ex.Message;
			//				this.ErrCode="-1";
			//			}
			//			this.ProgressBarValue=-1;
			#endregion 
			return this.myGetDepartment(sqlSelect);
		}
        /// <summary>
        /// ���ݴ���������ͻ�ÿ����б�
        /// </summary>
        /// <param name="type">������</param>
        /// <returns></returns>
        public ArrayList GetDeptmentByType(string type)
        {
            string sqlSelect = "", sqlWhere = "";
            if (this.Sql.GetSql("Department.SelectAllDept", ref sqlSelect) == -1)
                return null;
            if (this.Sql.GetSql("Department.Where.2", ref sqlWhere) == -1)
                return null;
            sqlSelect = sqlSelect + " " + sqlWhere;
            sqlSelect = string.Format(sqlSelect,type);
          
            return this.myGetDepartment(sqlSelect);
        }

        public ArrayList GetDeptmentIn(Neusoft.HISFC.Models.Base.EnumDepartmentType Type)
        {
			string sqlSelect ="",sqlWhere="";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sqlSelect)== -1)
				return null;
			if(this.Sql.GetSql("Department.Where.2",ref sqlWhere)== -1)
				return null;
			sqlSelect = sqlSelect+" "+ sqlWhere;
			sqlSelect=string.Format(sqlSelect,Type);
			#region 
			//			Neusoft.HISFC.Models.Base.Department objDept;
			//			switch(type)
			//			{
			//				case 1://����
			//					//����sql���	
			//					/*SELECT DEPT_code,dept_name,DEPT_TYPE,'',DEPT_PY FROM r_department WHERE ( 
			//						R_DEPARTMENT.DEPT_TYPE ='0001' ) */
			//					if(this.Sql.GetSql("Department.1",ref sql)==-1) return alDept;
			//					break;
			//				case 2://סԺ
			//					//����sql���	
			//					/*SELECT DEPT_code,dept_name,DEPT_TYPE,'',DEPT_PY FROM r_department WHERE ( 
			//						R_DEPARTMENT.DEPT_TYPE ='0002' ) */
			//					if(this.Sql.GetSql("Department.2",ref sql)==-1) return alDept;
			//					break;
			//				default:break;
			//			}
			//			//ִ��sql���
			//			this.ProgressBarText="���ڲ���סԺ����...";
			//			this.ProgressBarValue=0;
			//			this.ExecQuery(sql);
			//			try
			//			{
			//				while (this.Reader.Read())
			//				{
			//					objDept=new Neusoft.HISFC.Models.Base.Department();
			//					objDept.ID=this.Reader[0].ToString();
			//					objDept.Name =this.Reader[1].ToString();
			//
			//					objDept.Class.ID =this.Reader[2].ToString();
			//					objDept.Class.Name=this.Reader[3].ToString();
			//					objDept.Spell.SpellCode=this.Reader[4].ToString();
			//					objDept.Memo=this.Reader[4].ToString();
			//					alDept.Add(objDept);
			//					this.ProgressBarValue++;
			//				}
			//				this.Reader.Close();
			//			}//�׳�����
			//			catch(Exception ex)
			//			{
			//				this.Err=ex.Message;
			//				this.ErrCode="-1";
			//			}
			//			this.ProgressBarValue=-1;
			#endregion 
			return this.myGetDepartmentIn(sqlSelect);
		}

		/// <summary>
		/// ���ݿ������Ի�ÿ����б�
		/// </summary>
		/// <param name="deptProperty"></param>
		/// <returns></returns>
		public ArrayList GetDeptment(string deptProperty) {
			string sqlSelect ="",sqlWhere="";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sqlSelect)== -1)
				return null;
			if(this.Sql.GetSql("Department.Where.3",ref sqlWhere)== -1)
				return null;
			sqlSelect = sqlSelect+" "+ sqlWhere;
			sqlSelect=string.Format(sqlSelect,deptProperty);
			return this.myGetDepartment(sqlSelect);
		}
		/// <summary>
		/// ȡ���˻�ʿվ����Ŀ����б�
		/// </summary>
		/// <returns></returns>
		public ArrayList GetDeptNoNurse() {
			string sqlSelect ="",sqlWhere="";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sqlSelect)== -1)
				return null;
			if(this.Sql.GetSql("Department.Where.GetDeptNoNurse",ref sqlWhere)== -1)
				return null;
			sqlSelect = sqlSelect+" "+ sqlWhere;
			return this.myGetDepartment(sqlSelect);
		}
		/// <summary>
		/// ��ÿ��Ҹ��ݲ���
		/// </summary>
		/// <param name="NurseStation">����</param>
		/// <returns>�����б�</returns>
		public ArrayList GetDeptFromNurseStation(NeuObject NurseStation) {
			ArrayList al=new ArrayList();
			string sql="";
			if(this.Sql.GetSql("Manager.Department.GetDeptFromNurseStation",ref sql)==-1) return al;
			try {
				sql=string.Format(sql, statCode, NurseStation.ID);
			}
			catch{
				this.Err = "GetDeptFromNurseStation�Ĳ�������";
				this.WriteErr();
				return al;
			}
			if(this.ExecQuery(sql)==-1) return al;
			
				while(this.Reader.Read()) {
					Neusoft.HISFC.Models.Base.Department dept=new Neusoft.HISFC.Models.Base.Department();
					dept.ID = this.Reader[0].ToString();
					dept.Name = this.Reader[1].ToString();
					dept.SpellCode = this.Reader[2].ToString();
					dept.WBCode =this.Reader[3].ToString();
					if(dept.ID!="AAAA") { //���Ǹ�
						al.Add(dept);
					}
				}
			
			this.Reader.Close();
			return al;
		}
        /// <summary>
        /// ��ÿ��Ҹ��ݲ���(������)
        /// </summary>
        /// <param name="NurseStation">����</param>
        /// <returns>�����б�</returns>
        public ArrayList GetDeptFromNurseStationForArray(NeuObject NurseStation)
        {
            ArrayList al = new ArrayList();
            string sql = "";
            if (this.Sql.GetSql("Manager.Department.GetDeptFromNurseStationForArray", ref sql) == -1) return al;
            try
            {
                sql = string.Format(sql, "14", NurseStation.ID);
            }
            catch
            {
                this.Err = "Manager.Department.GetDeptFromNurseStationForArray�Ĳ�������";
                this.WriteErr();
                return al;
            }
            if (this.ExecQuery(sql) == -1) return al;

            while (this.Reader.Read())
            {
                Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
                dept.ID = this.Reader[0].ToString();
                dept.Name = this.Reader[1].ToString();
                dept.SpellCode = this.Reader[2].ToString();
                dept.WBCode = this.Reader[3].ToString();
                if (dept.ID != "AAAA")
                { //���Ǹ�
                    al.Add(dept);
                }
            }

            this.Reader.Close();
            return al;
        }
		/// <summary>
		/// ͨ�����һ�û�ʿվ
		/// </summary>
		/// <param name="DeptInfo"></param>
		/// <returns></returns>
		public ArrayList GetNurseStationFromDept(NeuObject DeptInfo) {
			ArrayList al=new ArrayList();
			string sql="";
			if(this.Sql.GetSql("Manager.Department.GetNurseStationFromDept",ref sql)==-1) return al;
			try {
				sql=string.Format(sql, statCode, DeptInfo.ID);
			}
			catch {
				this.Err = "GetNurseStationFromDept�Ĳ�������";
				this.WriteErr();
				return al;
			}
			if(this.ExecQuery(sql)==-1) return null;
			
				while(this.Reader.Read()) {
					Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();
				
					obj.ID = this.Reader[0].ToString();
					obj.Name = this.Reader[1].ToString();
					if(obj.ID!="AAAA") { //���Ǹ�
						al.Add(obj);
					}
					
				}
			this.Reader.Close();
			return al;
		}
        /// <summary>
        /// ͨ�����һ�û�ʿվ
        /// </summary>
        /// <param name="DeptInfo">������Ϣ</param>
        ///  <param name="MystatCode">�������</param>
        /// <returns>ArrayList</returns>
        public ArrayList GetNurseStationFromDept(NeuObject DeptInfo,string MystatCode)
        {
            ArrayList al = new ArrayList();
            string sql = "";
            if (this.Sql.GetSql("Manager.Department.GetNurseStationFromDept", ref sql) == -1) return al;
            try
            {
                sql = string.Format(sql, MystatCode, DeptInfo.ID);
            }
            catch
            {
                this.Err = "GetNurseStationFromDept�Ĳ�������";
                this.WriteErr();
                return al;
            }
            if (this.ExecQuery(sql) == -1) return null;

            while (this.Reader.Read())
            {
                Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();

                obj.ID = this.Reader[0].ToString();
                obj.Name = this.Reader[1].ToString();
                if (obj.ID != "AAAA")
                { //���Ǹ�
                    al.Add(obj);
                }

            }
            this.Reader.Close();
            return al;
        }
		/// <summary>
		/// ������л���վ
		/// </summary>
		/// <returns></returns>
		public ArrayList GetNurseAll() {

            return GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.N);
		}
		/// <summary>
		/// ��ȡ���еĿ��� �������õģ�ͣ�õģ������ģ� ��Ϣά��ר��
		/// </summary>
		/// <returns></returns>
		public ArrayList GetDeptAllUserStopDisuse() {
			string sql = "";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sql)== -1)
				return null;
			return this.myGetDepartment(sql);
		}
		/// <summary>
		/// ArrayList Filled Bu Base.Departments
		/// ����������õĿ���
		/// </summary>
		/// <returns></returns>
		public ArrayList GetDeptmentAll() {
			//Filled By Neusoft.HISFC.Models.Base.Department
			string sql = "";
			string sql1 = "";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sql)== -1)
				return null;
			if(this.Sql.GetSql("Department.SelectAllDeptUserWhere",ref sql1)==-1)
				return null;
			sql = sql + " " + sql1;
			return this.myGetDepartment(sql);
		}

		/// <summary>
		/// ȡ���Ҳ�����Ϣ
		/// </summary>
		/// <returns></returns>
		public ArrayList GetDeptmentAllIn() {
			//Filled By Neusoft.HISFC.Models.Base.Department
			string sql = "";
			string sql1 = "";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sql)== -1)
				return null;
			if(this.Sql.GetSql("Department.SelectAllDeptUserWhere",ref sql1)==-1)
				return null;
			sql = sql + " " + sql1;
			//			return this.myGetDepartment(sql);
			return this.myGetDepartmentIn(sql);
		}

		/// <summary>
		/// ά�����ҷ���ʱ��ȡ��ǰ������û�еĿ���
		/// writed by cuipeng 2005.3
		/// </summary>
		/// <param name="statCode">���ҷ������</param>
		/// <returns></returns>
		public ArrayList GetDeptmentForStat(string statCode) {
			//Filled By Neusoft.HISFC.Models.Base.Department
			string sql = "";
			if(this.Sql.GetSql("Department.GetDeptmentForStat",ref sql)== -1)
				return null;

			try {
				sql = string.Format(sql,statCode);
			}
			catch (Exception ex){
				this.Err = ex.Message;
				return null;
			}

			return this.myGetDepartment(sql);
		}
		
		/// <summary>
		/// ������п��ң������������͡����ұ�������
		/// </summary>
		/// <returns></returns>
		public ArrayList GetDeptmentAllOrderByDeptType() {
			//Filled By Neusoft.HISFC.Models.Base.Department
			string sql = "";
			if(this.Sql.GetSql("Department.SelectAllDept",ref sql)== -1)
				return null;

			string sqlOrder = "";
			if(this.Sql.GetSql("Department.SelectAllDept.Order",ref sqlOrder)== -1)
				return null;

			return this.myGetDepartment(sql + sqlOrder);
		}

		/// <summary>
		/// ����sql���ȡ�����б�
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		private ArrayList myGetDepartment(string sql) {
			if(this.ExecQuery(sql) == -1) return null;

			ArrayList result = new ArrayList();   
			while(this.Reader.Read()) {
				Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
				dept.ID = (string)this.Reader[0];
				dept.Name = (string)this.Reader[1];
				try {
					dept.SpellCode =(string)this.Reader[2];
				}
				catch{}
				try {
					dept.WBCode =(string)this.Reader[3];
				}
				catch{}
				//dept.DeptType =	 new DepartmentType();
				dept.DeptType.ID = (string)this.Reader[5];
				dept.IsRegDept = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[6]);
				dept.IsStatDept = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[7]);
				dept.SpecialFlag = this.Reader[8].ToString();
				dept.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)Convert.ToInt32(this.Reader[9]);
				
				if(!(this.Reader.IsDBNull(10)))
					dept.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[10]);
				//�Զ�����
				dept.UserCode = this.Reader[11].ToString();
				//�������
				dept.ShortName = this.Reader[12].ToString();
				result.Add(dept);

			}
			this.Reader.Close();
			return result;
		}

		/// <summary>
		/// ȡ���Ҳ�����Ϣ
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		private ArrayList myGetDepartmentIn(string sql) {
			if(this.ExecQuery(sql) == -1) return null;

			ArrayList result = new ArrayList();   
			while(this.Reader.Read()) {
				Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
				try {
					dept.ID = (string)this.Reader[0];
				
					dept.Name = (string)this.Reader[12];
				
					dept.SpellCode =(string)this.Reader[2];
				
					dept.WBCode =(string)this.Reader[3];
				
				
					//�������
					dept.Memo = this.Reader[1].ToString();
					result.Add(dept);
				}
				catch{}

			}
			this.Reader.Close();
			return result;
		}
		/// <summary>
		/// ��ȡ���Ŀ��ұ����
		/// </summary>
		/// <returns></returns>
		public string GetMaxDeptID()
		{
			string DeptID = "";
			string sql = "";
			if(this.Sql.GetSql("Department.GetMaxDeptID",ref sql)== -1)
				return null;
			DeptID = this.ExecSqlReturnOne(sql) ;
			return DeptID ;
		}
		/// <summary>
		/// ͨ�����ұ����ÿ�������
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Base.Department GetDeptmentById(string id) {
		
			string sql = "";
			if(this.Sql.GetSql("Department.SelectDepartmentByID",ref sql)== -1)
				return null;

			try {
				sql=string.Format(sql,id);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return null;
			}

			if(this.ExecQuery(sql) == -1) return null;

			if(this.Reader.Read()) {
				Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
				dept.ID = (string)this.Reader[0];
				dept.Name = (string)this.Reader[1];

				if(!(this.Reader.IsDBNull(2)))
					dept.SpellCode = this.Reader.GetString(2);
				if(!(this.Reader.IsDBNull(3)))
					dept.WBCode = this.Reader.GetString(3);
				if(!(this.Reader.IsDBNull(4)))
					dept.EnglishName = this.Reader.GetString(4); 

				//dept.DeptType =	 new DepartmentType();
				dept.DeptType.ID = (string)this.Reader[5];

				if(!(this.Reader.IsDBNull(6))) {
					if(this.Reader[6].ToString() == "0")
						dept.IsRegDept = false;//Convert.ToBoolean(this.Reader[6]);
					else
						dept.IsRegDept = true;
				}
				if(!(this.Reader.IsDBNull(7))) {
					//	dept.IsRegDept = Convert.ToBoolean(this.Reader[7]);
					if(this.Reader[7].ToString() == "0")
						dept.IsStatDept = false;
					else
						dept.IsStatDept = true;
				}

				dept.SpecialFlag = this.Reader[8].ToString();
				dept.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)Convert.ToInt32(this.Reader[9]);
				
				if(!(this.Reader.IsDBNull(10)))
					dept.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[10].ToString());
				//�Զ�����
				dept.UserCode = this.Reader[11].ToString();
				dept.ShortName = this.Reader[12].ToString();
				this.Reader.Close();
				return dept;  

			}

			return null;


        }
        #region ���¿�����Ϣ
        /// <summary>
		/// ���¿�����Ϣ
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public int Update(Neusoft.HISFC.Models.Base.Department info) {

			string sql = "";
			if(this.Sql.GetSql("Department.UpdateDept",ref sql)== -1)
				return -1;

			//			 UPDATE com_department    
			//    SET dept_name='{1}',   
			//        spell_code='{2}',   
			//        wb_code='{3}',   
			//        dept_ename='{4}',   
			//        dept_type='{5}',          
			//        regdept_flag='{6}',   
			//        tatdept_flag='{7}',   
			//        dept_pro='{8}',          
			//        valid_state='{9}',   
			//        sort_id= {10}, 
			//        oper_code = '{11}',
			//        oper_date = sysdate             
			//  WHERE dept_code='{0}'
			try
            {
                #region donggq--20101124--{0DC97329-2084-4c3d-9BA4-91AEB8F6FCE7}
                
                sql =string.Format(sql,info.ID,info.Name,info.SpellCode,info.WBCode,info.EnglishName,
					info.DeptType.ID,Convert.ToInt32(info.IsRegDept),Convert.ToInt32(info.IsStatDept),info.SpecialFlag,((int)info.ValidState).ToString(),info.SortID,this.Operator.ID,info.UserCode,info.ShortName,info.Memo);
                
                #endregion
            }
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}
            return this.ExecNoQuery(sql);
        }
        #endregion

        #region ���������Ϣ
        /// <summary>
		/// ���������Ϣ
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public int Insert(Neusoft.HISFC.Models.Base.Department info) {
			
			string sql = "";
			if(this.Sql.GetSql("Department.InsertDept",ref sql)== -1)
				return -1;

			try
            {
                #region donggq--20101124--{0DC97329-2084-4c3d-9BA4-91AEB8F6FCE7}
                
                sql =string.Format(sql,info.ID,info.Name,info.SpellCode,info.WBCode,info.EnglishName,
					info.DeptType.ID,Convert.ToInt32(info.IsRegDept),Convert.ToInt32(info.IsStatDept),info.SpecialFlag,
                    ((int)info.ValidState).ToString(),info.SortID,this.Operator.ID,info.UserCode,info.ShortName,info.Memo);
                
                #endregion
            }
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

            return this.ExecNoQuery(sql);
        }

        #endregion 

        #region donggq--20101124--{0DC97329-2084-4c3d-9BA4-91AEB8F6FCE7}

        /// <summary>
        /// ArrayList Filled Bu Base.Departments
        /// ������ÿ��ҵĿ��ҵص�
        /// </summary>
        /// <returns>���ҵص�</returns>
        public string GetDeptAddress(string deptId)
        {
            string sql = "";
            string val = "";
            if (this.Sql.GetSql("Department.GetDeptAddress", ref sql) == -1)
            {
                return null;
            }
            sql = string.Format(sql, deptId);
            if (this.ExecQuery(sql) == -1) 
            { 
                return null; 
            }

            if (this.Reader.Read())
            {
                try
                {
                    val = this.Reader[1].ToString();
                }
                catch(Exception ex) 
                {
				    this.ErrCode=ex.Message;
                    this.Err = "QueryDeptsAddress����" + ex.Message;
				    this.WriteErr();
                }
             
            }
            
            this.Reader.Close();

            return val;

        }

        #endregion


        /// <summary>
		/// ɾ��������Ϣ
		/// </summary>
		/// <param name="deptId"></param>
		/// <returns></returns>
		public int Delete(string deptId) {

			string sql = "";
			if(this.Sql.GetSql("Department.DeleteDept",ref sql)== -1)
				return -1;

			try {
				sql=string.Format(sql,deptId);
			}
			catch(Exception ex) {
				this.ErrCode=ex.Message;
				this.Err="�ӿڴ���"+ex.Message;
				this.WriteErr();
				return -1;
			}

            return this.ExecNoQuery(sql);
		}
		
        /// <summary>
        /// ���������Ƿ��Ѿ�����
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
		public int  SelectDepartMentIsExist(string DepartmentId) {
			int  IsExist=0;
			string strSql = "";
			if (this.Sql.GetSql("Manager.Person.SelectDepartMentIsExist",ref strSql)==-1) return -1;
			try {
				if(DepartmentId!="") {
					strSql = string.Format(strSql,DepartmentId);
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
        /// ArrayList Filled Bu Base.Departments
        /// ����������õĿ���
        /// </summary>
        /// <returns></returns>
        public ArrayList QueryValidDept()
        {
            string sql = "";
            string sql1 = "";
            if (this.Sql.GetSql("Department.SelectAllDept", ref sql) == -1)
            {
                return null;
            }
            if (this.Sql.GetSql("Department.SelectAllDeptUserWhere", ref sql1) == -1)
            {
                return null;
            }
            sql = sql + " " + sql1;

            return this.ExecSqlForDepartmentLis(sql);
        }
        /// <summary>
        /// ����sql���ȡ�����б�
        /// </summary>
        /// <param name="sql">ִ��Sql����ȡ������Ϣ</param>
        /// <returns>�ɹ����ؿ������� ʧ�ܷ���null</returns>
        private ArrayList ExecSqlForDepartmentLis(string sql)
        {
            if (this.ExecQuery(sql) == -1)
            {
                return null;
            }

            ArrayList result = new ArrayList();

            try
            {
                while (this.Reader.Read())
                {
                    Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
                    dept.ID = (string)this.Reader[0];
                    dept.Name = (string)this.Reader[1];
                    dept.SpellCode = this.Reader[2].ToString();

                    dept.WBCode = this.Reader[3].ToString();

                    dept.DeptType.ID = this.Reader[5].ToString();
                    dept.Memo = (string)this.Reader[5];
                    dept.IsRegDept = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[6]);
                    dept.IsStatDept = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[7]);

                    dept.SpecialFlag = this.Reader[8].ToString();
                    dept.ValidState = (Neusoft.HISFC.Models.Base.EnumValidState)Convert.ToInt32(this.Reader[9]);

                    dept.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[10]);

                    //�Զ�����
                    dept.UserCode = this.Reader[11].ToString();
                    //�������
                    dept.ShortName = this.Reader[12].ToString();

                    //if (this.Reader.FieldCount > 13)
                    //{
                    //    dept.Remark = this.Reader[13].ToString();
                    //    dept.OwnCode.ID = this.Reader[14].ToString();
                    //}

                    result.Add(dept);
                }
            }
            catch (Exception e)
            {
                this.Err = e.Message;
                return null;
            }
            finally
            {
                this.Reader.Close();
            }
            return result;
        }
	}
}
