 using System;
using System.Collections;
namespace Neusoft.HISFC.BizLogic.EPR
{
	/// <summary>
	/// GetFile ��ժҪ˵����
	/// ����ļ�
	/// </summary>
	public class DataFile:Neusoft.FrameWork.Management.Database
	{
		public DataFile(string type)
		{
			try
			{
				dtParam = this.ParamManager.Get(type) as Neusoft.HISFC.Models.File.DataFileParam;
				if(dtParam==null) return;
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;				
				return;
			}
		}
		public DataFile()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        public Neusoft.HISFC.BizLogic.EPR.DataFileParam ParamManager = new Neusoft.HISFC.BizLogic.EPR.DataFileParam();
        public Neusoft.HISFC.BizLogic.EPR.DataFileInfo FileManager = new Neusoft.HISFC.BizLogic.EPR.DataFileInfo();
		private Neusoft.HISFC.Models.File.DataFileParam  dtParam= null;
		private Neusoft.HISFC.Models.File.DataFileInfo dtFile =null;
		
		#region ����
		/// <summary>
		/// ��ǰ�ļ��б�
		/// </summary>
		public ArrayList alFiles;
		/// <summary>
		/// ��ǰģ���б�
		/// </summary>
		public ArrayList alModuals;
	
		/// <summary>
		/// ��������ʾ����
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public int SetType(string type)
		{
			try
			{
				dtParam = this.ParamManager.Get(type) as Neusoft.HISFC.Models.File.DataFileParam;
				if(dtParam==null) return -1;
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;				
				return -1;
			}
			return 0;
		}
		/// <summary>
		/// �������
		/// </summary>
		/// <returns></returns>
		public int GetModuals(bool isAll)
		{
			try
			{
			
				alModuals = FileManager.GetList(dtParam,1,isAll);
				return alModuals.Count;
			}
			catch{return -1;}
		}
		/// <summary>
		/// �������
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public int GetFiles(params string[] param)
		{
			try
			{
				this.dtFile = new Neusoft.HISFC.Models.File.DataFileInfo();
				this.dtFile.Param =(Neusoft.HISFC.Models.File.DataFileParam)this.dtParam.Clone();
				this.dtFile.Param.ID = string.Format(this.dtParam.ID,param);
				this.dtFile.Param.Type = this.dtParam.Type;
				alFiles = FileManager.GetList(this.dtFile.Param);
				return alFiles.Count;
			}
			catch{return -1;}
		}
		/// <summary>
		/// ��ò���
		/// </summary>
		public Neusoft.HISFC.Models.File.DataFileParam DataFileParam
		{
			get
			{
				if(this.dtParam==null)this.dtParam=new Neusoft.HISFC.Models.File.DataFileParam();
				return this.dtParam;
			}
			set
			{
				this.dtParam = value;
			}
		}
		/// <summary>
		/// ����ļ���Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.File.DataFileInfo DataFileInfo
		{
			get
			{
				if(this.dtFile==null)this.dtFile =new Neusoft.HISFC.Models.File.DataFileInfo();
				return this.dtFile;
			}
			set
			{
				this.dtFile = value;
			}
		}

		
		#endregion

		#region �ڵ����
		/// <summary>
		/// �����㵽���ݿ�
		/// </summary>
		/// <param name="Table"></param>
		/// <param name="dt"></param>
		/// <param name="ParentText"></param>
		/// <param name="NodeText"></param>
		/// <param name="NodeValue"></param>
		/// <returns></returns>
		public int SaveNodeToDataStore(string Table,Neusoft.HISFC.Models.File.DataFileInfo dt,string ParentText,string NodeText,string NodeValue)
		{			
			string strSql ="";
			string sql="";

			if(this.Sql.GetSql("Management.DataFile.Select",ref strSql)==-1) return -1;
			try
			{
				Neusoft.FrameWork.Public.String.FormatString (strSql,out sql,Table,dt.ID,dt.Type,dt.DataType,dt.Name,dt.Index1,dt.Index2,ParentText,NodeText,NodeValue,this.Operator.ID);
			}
			catch(Exception ex)
			{
				this.Err ="Management.DataFile.Select��ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			if(this.ExecQuery(sql)==-1) return -1;
			if(this.Reader.Read())//�м�¼��ִ�и���
			{
				if(NodeValue == this.Reader[0].ToString())//��ͬ����
				{
					this.Reader.Close();
					return 0;
				}
				else
				{
					if(this.Sql.GetSql("Management.DataFile.UpdateToDataStore",ref strSql)==-1) return -1;
				}
			}
			else//�޼�¼��ִ�в���
			{
				if(this.Sql.GetSql("Management.DataFile.InsertToDataStore",ref strSql)==-1) return -1;
			}
			try
			{
				Neusoft.FrameWork.Public.String.FormatString (strSql,out sql,Table,dt.ID,dt.Type,dt.DataType,dt.Name,dt.Index1,dt.Index2,ParentText,NodeText,NodeValue,this.Operator.ID);
			}
			catch(Exception ex){
				this.Err ="SaveNodeToDataStore��ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			try
			{
				this.Reader.Close();
			}
			catch{}
			return this.ExecNoQuery(sql);
		}
		/// <summary>
		/// ɾ����㡡
		/// </summary>
		/// <param name="Table"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public int DeleteAllNodeFromDataStore(string Table,Neusoft.HISFC.Models.File.DataFileInfo dt)
		{
			string strSql ="",sql="";
			if(this.Sql.GetSql("Management.DataFile.DeleteAllNodeFromDataStore",ref strSql)==-1) return -1;
			try
			{
				sql = string.Format(strSql,Table,dt.ID);
			}
			catch(Exception ex)
			{
				this.Err ="DeleteNode��ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(sql);
		}
		
		/// <summary>
		/// ��ýڵ�����
		/// </summary>
		/// <param name="Table"></param>
		/// <param name="inpatientNo"></param>
		/// <param name="nodeName"></param>
		/// <returns></returns>
		public string GetNodeValueFormDataStore(string Table,string inpatientNo,string nodeName)
		{
			string strSql ="",sql="";
			if(this.Sql.GetSql("Management.DataFile.GetNodeValueFormDataStore",ref strSql)==-1) return "-1";
			try
			{
				sql = string.Format(strSql,Table,inpatientNo,nodeName);
			}
			catch(Exception ex)
			{
				this.Err ="GetNodeValueFormDataStore��ֵʱ�����"+ex.Message;
				this.WriteErr();
				return "-1";
			}
			return this.ExecSqlReturnOne(sql);
		}
		#endregion

        //#region ���ֶβ���

        ///// <summary>
        ///// ���ļ����뵽���ݿ���
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData">������ļ�����</param>
        ///// <returns></returns>
        //public int ImportToDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt,byte[] fileData)
        //{
        //    string strSql ="",sql="";
        //    if(dt.ID == null||dt.ID =="") return -1;

        //    if(this.Sql.GetSql("Management.DataFile.ImportToDatabase",ref strSql)==-1) return -1;
        //    try
        //    {
        //        sql = string.Format(strSql,dt.ID);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err ="ImportToDatabase��ֵʱ�����"+ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }
			
        //    return this.InputBlob(sql,fileData);
        //}

        ///// <summary>
        ///// ����ļ� 
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData"></param>
        ///// <returns></returns>
        //public int ExportFromDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt,ref byte[] fileData)
        //{
        //    string strSql ="",sql="";
        //    if(dt.ID == null||dt.ID =="") return -1;

        //    if(this.Sql.GetSql("Management.DataFile.ExportFromDatabase",ref strSql)==-1) return -1;
        //    try
        //    {
        //        sql = string.Format(strSql,dt.ID);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err ="ExportFromDatabase��ֵʱ�����"+ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }
			
        //    fileData = this.OutputBlob(sql);
        //    return 0;
        //}

        ///// <summary>
        ///// ���ļ����뵽���ݿ���
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData">������ļ�����</param>
        ///// <returns></returns>
        //public int ImportToDatabaseModual(Neusoft.HISFC.Models.File.DataFileInfo dt, byte[] fileData)
        //{
        //    string strSql = "", sql = "";
        //    if (dt.ID == null || dt.ID == "") return -1;

        //    if (this.Sql.GetSql("Management.DataFile.ImportToDatabase", ref strSql) == -1) return -1;
        //    try
        //    {
        //        sql = string.Format(strSql, dt.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Err = "ImportToDatabase��ֵʱ�����" + ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }

        //    return this.InputBlob(sql, fileData);
        //}

        ///// <summary>
        ///// ����ļ� 
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData"></param>
        ///// <returns></returns>
        //public int ExportFromDatabaseModual(Neusoft.HISFC.Models.File.DataFileInfo dt, ref byte[] fileData)
        //{
        //    string strSql = "", sql = "";
        //    if (dt.ID == null || dt.ID == "") return -1;

        //    if (this.Sql.GetSql("Management.DataFile.ExportFromDatabase", ref strSql) == -1) return -1;
        //    try
        //    {
        //        sql = string.Format(strSql, dt.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Err = "ExportFromDatabase��ֵʱ�����" + ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }

        //    fileData = this.OutputBlob(sql);
        //    return 0;
        //}

        ///// <summary>
        ///// ���ļ����뵽���ݿ���
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData">������ļ�����</param>
        ///// <returns></returns>
        //public int ImportToDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt,string fileData)
        //{
        //    string strSql ="",sql ="";
        //    if(dt.ID == null||dt.ID =="") return -1;

        //    if(this.Sql.GetSql("Management.DataFile.ImportToDatabase",ref strSql)==-1) return -1;
			
        //    try
        //    {
        //        sql = string.Format(strSql,dt.ID);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err ="ImportToDatabase��ֵʱ�����"+ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }
        //    return this.InputLong(sql,fileData);
        //}

        ///// <summary>
        ///// ����ļ� 
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="fileData"></param>
        ///// <returns></returns>
        //public int ExportFromDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt,ref string fileData)
        //{
        //    string strSql ="",sql="";
        //    if(dt.ID == null||dt.ID =="") return -1;

        //    if(this.Sql.GetSql("Management.DataFile.ExportFromDatabase",ref strSql)==-1) return -1;
        //    try
        //    {
        //        sql = string.Format(strSql,dt.ID);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err ="ExportFromDatabase��ֵʱ�����"+ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }
			
        //    fileData = this.ExecSqlReturnOne(sql);
        //    if(fileData =="-1") return -1;
        //    return 0;
        //}

        //#endregion
        #region ���ֶβ���

        /// <summary>
        /// ���ļ����뵽���ݿ���
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileData">������ļ�����</param>
        /// <returns></returns>
        public int ImportToDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt, byte[] fileData)
        {
            string strSql = "", sql = "";
            if (dt.ID == null || dt.ID == "") return -1;
            if (dt.Type.Trim() == "0")//����
            {
                if (this.Sql.GetSql("Management.DataFile.ImportToDatabase.byte", ref strSql) == -1) return -1;
            }
            else if (dt.Type.Trim() == "1") //ģ��
            {
                if (this.Sql.GetSql("Management.DataFile.ImportToDatabase.Modual.byte", ref strSql) == -1) return -1;
            }
            else
            {
                this.Err = "δ֪�ļ�����";
                return -1;
            }

            try
            {
                sql = string.Format(strSql, dt.ID);
            }
            catch (Exception ex)
            {
                this.Err = "ImportToDatabase��ֵʱ�����" + ex.Message;
                this.WriteErr();
                return -1;
            }

            return this.InputBlob(sql, fileData);
        }

        /// <summary>
        /// ����ļ� 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public int ExportFromDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt, ref byte[] fileData)
        {
            string strSql = "", sql = "";
            if (dt.ID == null || dt.ID == "") return -1;

            if (dt.Type.Trim() == "0")//����
            {
                if (this.Sql.GetSql("Management.DataFile.ExportFromDatabase.byte", ref strSql) == -1) return -1;
            }
            else if (dt.Type.Trim() == "1") //ģ��
            {
                if (this.Sql.GetSql("Management.DataFile.ExportFromDatabase.Modual.byte", ref strSql) == -1) return -1;
            }
            else
            {
                this.Err = "δ֪�ļ�����";
                return -1;
            }

            try
            {
                sql = string.Format(strSql, dt.ID);
            }
            catch (Exception ex)
            {
                this.Err = "ExportFromDatabase��ֵʱ�����" + ex.Message;
                this.WriteErr();
                return -1;
            }

            fileData = this.OutputBlob(sql);
            return 0;
        }



        /// <summary>
        /// ���ļ����뵽���ݿ���
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileData">������ļ�����</param>
        /// <returns></returns>
        public int ImportToDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt, string fileData)
        {

            string strSql = "", sql = "";
            if (dt.ID == null || dt.ID == "") return -1;

            if (dt.Type.Trim() == "0")//����
            {
                if (this.Sql.GetSql("Management.DataFile.ImportToDatabase", ref strSql) == -1) return -1;
            }
            else if (dt.Type.Trim() == "1") //ģ��
            {
                if (this.Sql.GetSql("Management.DataFile.ImportToDatabase.Modual", ref strSql) == -1) return -1;
            }
            else
            {
                this.Err = "δ֪�ļ�����";
                return -1;
            }

            try
            {
                sql = string.Format(strSql, dt.ID);
            }
            catch (Exception ex)
            {
                this.Err = "ImportToDatabase��ֵʱ�����" + ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.InputLong(sql, fileData);

        }

        /// <summary>
        /// ����ļ� 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public int ExportFromDatabase(Neusoft.HISFC.Models.File.DataFileInfo dt, ref string fileData)
        {
            string strSql = "", sql = "";
            if (dt.ID == null || dt.ID == "") return -1;
            if (dt.Type.Trim() == "0")//����
            {
                if (this.Sql.GetSql("Management.DataFile.ExportFromDatabase", ref strSql) == -1) return -1;
            }
            else if (dt.Type.Trim() == "1") //ģ��
            {
                if (this.Sql.GetSql("Management.DataFile.ExportFromDatabase.Modual", ref strSql) == -1) return -1;
            }
            else
            {
                this.Err = "δ֪�ļ�����";
                return -1;
            }

            try
            {
                sql = string.Format(strSql, dt.ID);
            }
            catch (Exception ex)
            {
                this.Err = "ExportFromDatabase��ֵʱ�����" + ex.Message;
                this.WriteErr();
                return -1;
            }

            fileData = this.ExecSqlReturnOne(sql);
            if (fileData == "-1") return -1;
            return 0;
        }

        #endregion
	}
}
