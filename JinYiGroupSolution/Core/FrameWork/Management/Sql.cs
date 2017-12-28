using System;
using System.Collections;
using System.Xml;
namespace Neusoft.FrameWork.Management
{
    /// <summary>
    /// Sql<br></br>
    /// [��������: Sql��,����sql����xml,����sql�����滻]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    
    public class Sql:Neusoft.FrameWork.Management.Database
	{
	
		public Sql()
		{
          
		}

        //���캯������
		public Sql(string FileName)
		{
			this.LoadSql(FileName);
		}
		public Sql(System.Data.IDbConnection con)
		{
			this.con=con;

            this.GetSQLTable();

            this.LoadSql(this.con);
        }

        #region ����
        public string FileName;
        public ArrayList alSql = new ArrayList();
        protected int mode = 0;//Ĭ����sql.xml����
        public ArrayList table_name = new ArrayList();
     
        #endregion

        /// <summary>
		/// �������ݿ�
		/// </summary>
		/// <param name="con"></param>
		public int LoadSql(System.Data.IDbConnection con)
		{
			mode=1;//���ݿ�����
			this.con=con;
            if (this.LoadSql() < 0)
            {
                alSql = null;
                table_name = null;
                return -1;
            }
			return 0;

		}
		
		/// <summary>
		/// ����sql�ļ�
		/// </summary>
		/// <returns>0 �ɹ� -1 ʧ��</returns>
		public int LoadSql()
        {
            #region ����SQL
            switch (mode)
			{
				case 0:
				System.Xml.XmlDataDocument doc=new System.Xml.XmlDataDocument();
					try
					{
						doc.Load(FileName);
					}
					catch(Exception ex)
					{
						this.Err=ex.Message;
						this.ErrCode="-1";
						this.WriteErr();
						return -1;
					}
					XmlNodeList nodes;
					nodes=doc.SelectNodes(@"//SQL");
					try
					{
						foreach(XmlNode node in nodes)
						{
							Neusoft.FrameWork.Models.NeuObject objValue=new Neusoft.FrameWork.Models.NeuObject();
							objValue.ID=node.Attributes[0].Value.ToString();
							objValue.Name=node.InnerText.ToString();
							objValue.Name=objValue.Name.Replace("\r"," ");
							//objValue.Name=objValue.Name.Replace("\n"," ");
							objValue.Name=objValue.Name.Replace("\t"," ");
							try
							{
								objValue.Memo=node.Attributes[1].Value.ToString();
							}
							catch{}
							this.alSql.Add(objValue);
						}
					}
					catch(Exception ex)
					{
						this.Err=ex.Message;
						this.ErrCode="-1";
						this.WriteErr();
						return -1;
					}
					break;
				default:
                    for (int i = 0; i < table_name.Count; i++)
                    {
                        Neusoft.FrameWork.Models.NeuObject obj = table_name[i] as Neusoft.FrameWork.Models.NeuObject;
                        if (obj.ID == "1")//��ʼʱ�����
                        {
                            //��ΪҪ���ӶԲ�ͬ���ݿ��֧��,��ͬ���ݿ����SQL���洢���ֶβ�ͬ, ���Բ�����
                            //{476364C9-195A-4ca8-A2D7-6782088016FA}
                            string mySQL = string.Empty;
                            if (Neusoft.FrameWork.Management.Connection.DBType== Connection.EnumDBType.ORACLE)
                            {
                                mySQL = string.Format("select id,name,memo from {0}", table_name[i].ToString());
                            }
                            else if (Neusoft.FrameWork.Management.Connection.DBType == Connection.EnumDBType.DB2)
                            {
                                mySQL = string.Format("select id,db2_sql,memo from {0}", table_name[i].ToString());
                            }
                           
                            else//�Ժ�����
                            {
                                mySQL = string.Format("select id,name,memo from {0}", table_name[i].ToString());
                            }
                            //end ;
                            if (this.ExecQuery(mySQL) == -1) return -1;//��������
                            while (this.Reader.Read())
                            {
                                Neusoft.FrameWork.Models.NeuObject objValue = new Neusoft.FrameWork.Models.NeuObject();
                                objValue.ID = this.Reader[0].ToString();
                                objValue.Name = this.Reader[1].ToString();
                                objValue.Name = objValue.Name.Replace("\r", " ");
                                //objValue.Name=objValue.Name.Replace("\n"," ");
                                objValue.Name = objValue.Name.Replace("\t", " ");
                                try
                                {
                                    objValue.Memo = this.Reader[2].ToString();
                                }
                                catch { }
                                this.alSql.Add(objValue);
                            }
                            this.Reader.Close();
                        }
                    }
					break;
            }
            #endregion
            return 0;
		}
		/// <summary>
		/// ����sql�ļ�
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns></returns>
		public int LoadSql(string FileName)
		{
			this.FileName=FileName;
			LoadSql();
			return 0;
		}
		/// <summary>
		/// ���sql���
		/// </summary>
		/// <param name="index"></param>
		/// <param name="Sql"></param>
		/// <returns></returns>
		public int GetSql(string index,ref string Sql)
		{
			for(int i=0;i<this.alSql.Count;i++)
			{
				if(((Neusoft.FrameWork.Models.NeuObject)this.alSql[i]).ID.Trim()==index.Trim())
				{
					Sql=((Neusoft.FrameWork.Models.NeuObject)this.alSql[i]).Name;
					this.Err="���Sql��䣬����Ϊ��"+index+"\n SqlΪ��"+Sql;
					this.WriteDebug(this.Err);
					return 0;
				}
			}
            for (int i = 0; i < table_name.Count; i++)
            {
                Neusoft.FrameWork.Models.NeuObject obj = table_name[i] as Neusoft.FrameWork.Models.NeuObject;
                if (obj.ID == "0")//��ʼʱ�����
                {
                    //��ΪҪ���ӶԲ�ͬ���ݿ��֧��,��ͬ���ݿ����SQL���洢���ֶβ�ͬ, ���Բ�����
                    //{844EC201-D874-4d1e-B2B3-DBC61DA21599}
                    //string mySQL = string.Format("select id,name,memo from {0} where id='{1}'", table_name[i].ToString(), index);//ԭ���ĳ���
                    string mySQL = string.Empty;
                    if (Neusoft.FrameWork.Management.Connection.DBType == Connection.EnumDBType.ORACLE)
                    {
                        mySQL = string.Format("select id,name,memo from {0} where id='{1}'", table_name[i].ToString(), index);
                    }
                    else if (Neusoft.FrameWork.Management.Connection.DBType == Connection.EnumDBType.DB2)
                    {
                        mySQL = string.Format("select id,db2_sql,memo from {0} where id='{1}'", table_name[i].ToString(), index);
                    }
                    else//�Ժ�����
                    {
                        mySQL = string.Format("select id,name,memo from {0} where id='{1}'", table_name[i].ToString(), index);
                    }
                    //end ;

                    if (this.ExecQuery(mySQL) == -1) return -1;//��������
                    if (this.Reader.Read())
                    {
                        Neusoft.FrameWork.Models.NeuObject objValue = new Neusoft.FrameWork.Models.NeuObject();
                        objValue.ID = this.Reader[0].ToString();
                        objValue.Name = this.Reader[1].ToString();
                        objValue.Name = objValue.Name.Replace("\r", " ");
                        //objValue.Name=objValue.Name.Replace("\n"," ");
                        objValue.Name = objValue.Name.Replace("\t", " ");
                        try
                        {
                            objValue.Memo = this.Reader[2].ToString();
                        }
                        catch { }

                        this.alSql.Add(objValue);
                        Sql = objValue.Name;
                        this.Err = "���Sql��䣬����Ϊ��" + index + "\n SqlΪ��" + Sql;
                        this.WriteDebug(this.Err);
                        this.Reader.Close();
                        return 0;
                    }
                    
                    this.Reader.Close();
                    
                }
            }
			this.Err="û�ҵ�Sql��䣡"+index;
            this.WriteErr();
			return -1;
		}

        /// <summary>
        /// ���������SQL���
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private int GetSQLTable()
        {
            string strSql = "select sqlTableName,isBeginLoad from com_sql_setting "; //ȡ���ص�SQL���

            if (this.ExecQuery(strSql) == -1)
            {
                //û�����������������ֻȡCOM_SQL
                table_name.Add(new Neusoft.FrameWork.Models.NeuObject("1", "COM_SQL", ""));
                return -1;
            }
            try
            {
                while (this.Reader.Read())
                {
                    Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                    obj.ID = this.Reader[1].ToString();
                    obj.Name = this.Reader[0].ToString();
                    table_name.Add(obj);
                }
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
            }
            this.Reader.Close();

            return 0;
        }

	}
}
