using System;
using System.Collections;

namespace Neusoft.HISFC.BizLogic.Nurse
{
	/// <summary>
	/// ������Ϣ������
	/// </summary>
	public class Queue : Neusoft.FrameWork.Management.Database
    {
        #region ԭ����

        //public Queue()
        //{
        //    //
        //    // TODO: �ڴ˴���ӹ��캯���߼�
        //    //
        //}
        //#region ��������
        //protected ArrayList al = null;
        //protected Neusoft.HISFC.Models.Nurse.Queue queue = null;
        //#endregion

        ///// <summary>
        ///// ��ò�������б�
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //protected string [] myGetParmInsertQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        //{
        //    string[] strParm={	
        //                         obj.Dept.ID,//����
        //                            obj.Name,//��������
        //        obj.Noon.ID,//���
        //        obj.User01,//�������
        //        obj.Order.ToString(),//˳��
        //        obj.IsValid?"1":"0",//�Ƿ���Ч
        //        obj.Memo,//��ע
        //        obj.OperID,//����Ա
        //        obj.QueueDate.ToString(),
        //        obj.Doctor.ID,
        //        obj.ID,
        //        obj.Room.ID,
        //        obj.Room.Name

        //                     };

        //    return strParm;

        //} 
        ///// <summary>
        ///// ����޸Ķ��в����б�
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //protected string [] myGetParmUpdateQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        //{
        //    string[] strParm={	
        //                         obj.ID,
        //                         obj.Dept.ID,
        //                         obj.Name,
        //        obj.Noon.ID,
        //        obj.User01,
        //        obj.Order.ToString(),
        //        obj.IsValid?"1":"0",
        //        obj.Memo,
        //        obj.OperID,
        //        obj.QueueDate.ToString(),
        //        obj.Doctor.ID,
        //        obj.Room.ID,
        //        obj.Room.Name

        //                     };
        //    return strParm;
        //} 

        ///// <summary>
        ///// ��ô�����
        ///// </summary>
        ///// <returns></returns>
        //public string GetQueueNo()
        //{
        //    return this.GetSequence("Nurse.GetRecipeNo.Select");
        //}
        ///// <summary>
        ///// �������
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>

        //public int InsertQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        //{
        //    string strSQL = "";
        //        //ȡ���������SQL���
        //    string[] strParam ;
        //    if(this.Sql.GetSql("Nurse.Queue.InsertQueue",ref strSQL) == -1) 
        //    {
        //        this.Err = "û���ҵ��ֶ�!";
        //        return -1;
        //    }
        //    try
        //    {
        //        if (obj.ID == null) return -1;
        //        obj.ID = "T"+this.GetQueueNo();
        //        strParam = this.myGetParmInsertQueue(obj); 
			
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err = "��ʽ��SQL���ʱ����:" + ex.Message;
        //        this.WriteErr();
        //        return -1;
        //    }
        //    return this.ExecNoQuery(strSQL,strParam);
			
			
        //}

        ///// <summary>
        ///// ����޸Ķ��в����б�
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public int UpdateQueue(Neusoft.HISFC.Models.Nurse.Queue obj) 
        //{
        //    string strSql="";			
        //    string[] strParam ;
        //    if(this.Sql.GetSql("Nurse.Queue.UpdateQueue",ref strSql)==-1) return -1;
        //    try
        //    {
        //        //��ȡ�����б�
        //        strParam = this.myGetParmUpdateQueue(obj);
        //        strSql = string.Format(strSql,strParam);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err=ex.Message;
        //        this.ErrCode=ex.Message;
        //        return -1;
        //    }
            
        //    return this.ExecNoQuery(strSql);
        //}

        ///// <summary>
        ///// ɾ������
        ///// </summary>
        ///// <param name="queueNo"></param>
        ///// <returns></returns>
        //public int DelQueue(string queueNo)
        //{
        //    string strSql = "";
        //    if (this.Sql.GetSql("Nurse.DelQueue.1",ref strSql)==-1) return -1;
        //    try
        //    {
        //        strSql = string.Format(strSql,queueNo);
        //    }
        //    catch(Exception ex)
        //    {
        //        this.Err=ex.Message;
        //        this.ErrCode=ex.Message;
        //        return -1;
        //    }			
        //    return this.ExecNoQuery(strSql);
        //}

        ///// <summary>
        ///// ����ʿվ/��������/����ѯ���������Ϣ
        ///// </summary>
        ///// <param name="nurseID"></param>
        ///// <param name="queueDate"></param>
        ///// <param name="noonID"></param>
        ///// <returns></returns>
        //public ArrayList Query(string nurseID,DateTime queueDate,string noonID)
        //{
        //    string sql = "", where = "";

        //    if(this.Sql.GetSql("Nurse.Queue.Query.1",ref sql) == -1)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
        //        return null;
        //    }
			
        //    if(this.Sql.GetSql("Nurse.Queue.Query.2",ref where) == -1)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.2]";
        //        return null;
        //    }

        //    try
        //    {
        //        where = string.Format(where,nurseID,queueDate.ToString(),noonID);
        //    }
        //    catch(Exception e)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]" + e.Message;
        //        this.ErrCode = e.Message;
        //        return null;
        //    }

        //    sql = sql + " " + where ;

        //    return this.Query(sql);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nurseID"></param>
        ///// <param name="queueDate"></param>
        ///// <returns></returns>
        //public ArrayList Query(string nurseID,string queueDate)
        //{
        //    string sql = "", where = "";
        //    string strBegin = queueDate+" "+"00:00:00",strEnd = queueDate+" "+"23:59:59"; 

        //    if(this.Sql.GetSql("Nurse.Queue.Query.1",ref sql) == -1)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
        //        return null;
        //    }
			
        //    if(this.Sql.GetSql("Nurse.Queue.Query.3",ref where) == -1)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.3]";
        //        return null;
        //    }
        //    try
        //    {
        //        where = string.Format(where,nurseID,strBegin,strEnd);
        //    }
        //    catch(Exception e)
        //    {
        //        this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]" + e.Message;
        //        this.ErrCode = e.Message;
        //        return null;
        //    }

        //    sql = sql + " " + where ;

        //    return this.Query(sql);
        //}
        ///// <summary>
        ///// ����sql��ѯ������Ϣ
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //protected ArrayList Query(string sql)
        //{
        //    if(this.ExecQuery(sql) == -1)
        //    {
        //        this.Err = "����sql����!"+sql;
        //        this.ErrCode = "����sql����!"+sql;
        //        return null;
        //    }

        //    this.al = new ArrayList();

        //    try
        //    {
        //        while(this.Reader.Read())
        //        {
        //            this.queue = new Neusoft.HISFC.Models.Nurse.Queue();

        //            //������ʿվ
        //            this.queue.Dept.ID = this.Reader[0].ToString();
        //            //���д���
        //            this.queue.ID = this.Reader[1].ToString();
        //            //��������
        //            this.queue.Name = this.Reader[2].ToString();
        //            //������
        //            this.queue.Noon.ID = this.Reader[3].ToString();
        //            //��ʾ˳��
        //            if(!this.Reader.IsDBNull(5))
        //                this.queue.Order = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[5].ToString());
        //            //�Ƿ���Ч
        //            this.queue.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[6].ToString());
        //            //��ע
        //            this.queue.Memo = this.Reader[7].ToString();
        //            //����Ա
        //            this.queue.OperID = this.Reader[8].ToString();
        //            //����ʱ��
        //            this.queue.OperDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[9].ToString());
        //            //��������
        //            this.queue.QueueDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[10].ToString());
        //            //����ҽ��
        //            this.queue.Doctor.ID = this.Reader[11].ToString();
        //            this.queue.Room.ID = this.Reader[12].ToString();
        //            this.queue.Room.Name = this.Reader[13].ToString();

        //            this.al.Add(this.queue);
        //        }
				
        //        this.Reader.Close();
        //    }
        //    catch(Exception e)
        //    {
        //        if(!this.Reader.IsClosed)this.Reader.Close();
        //        this.Err = "��ѯ���ﻤʿվ���������Ϣ����!"+e.Message;
        //        this.ErrCode = e.Message;
        //        return null;
        //    }

        //    return this.al;
        //}

        #endregion

        public Queue()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region ��������
        protected ArrayList al = null;
        protected Neusoft.HISFC.Models.Nurse.Queue queue = null;
        #endregion

        /// <summary>
        /// ��ò�������б�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string[] myGetParmInsertQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        {
            string[] strParm ={	
								    obj.Dept.ID,//����1
									obj.Name,//��������2
									obj.Noon.ID,//���3
									obj.User01,//�������4
									obj.Order.ToString(),//˳��5
									obj.IsValid?"1":"0",//�Ƿ���Ч6
									obj.Memo,//��ע7
									obj.Oper.ID,//����Ա8
									obj.QueueDate.ToString(),//����ʱ��9
									obj.Doctor.ID,//ҽ������10
									obj.ID,//���к�11
									obj.SRoom.ID,//���Ҵ���12
									obj.SRoom.Name,//��������13
									obj.Console.ID,//��̨����14
									obj.Console.Name,//��̨����15
									obj.ExpertFlag,//ר��16
								    obj.AssignDept.ID,
									obj.AssignDept.Name
							 };

            return strParm;

        }
        /// <summary>
        /// ����޸Ķ��в����б�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string[] myGetParmUpdateQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        {
            string[] strParm ={	
								obj.ID,//���к�
								obj.Dept.ID,//���Ҵ���
								obj.Name,//��������
								obj.Noon.ID,//���
								obj.User01,//�������
								obj.Order.ToString(),//˳��
								obj.IsValid?"1":"0",//�Ƿ���Ч
								obj.Memo,//��ע
								obj.Oper.ID,//����Ա
								obj.QueueDate.ToString(),//����ʱ��
								obj.Doctor.ID,//ҽ������
								obj.SRoom.ID,//���Ҵ���
								obj.SRoom.Name,//��������
								obj.Console.ID,//��̨
								obj.Console.Name,//��̨����
								 obj.ExpertFlag,//ר��16
								 obj.AssignDept.ID,
								 obj.AssignDept.Name
							 };
            return strParm;
        }

        /// <summary>
        /// ��ô�����
        /// </summary>
        /// <returns></returns>
        public string GetQueueNo()
        {
            return this.GetSequence("Nurse.GetRecipeNo.Select");
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        public int InsertQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        {
            string strSQL = "";
            //ȡ���������SQL���
            string[] strParam;
            if (this.Sql.GetSql("Nurse.Queue.InsertQueue", ref strSQL) == -1)
            {
                this.Err = "û���ҵ��ֶ�!";
                return -1;
            }
            try
            {
                if (obj.ID == null) return -1;
                obj.ID = "T" + this.GetQueueNo();
                strParam = this.myGetParmInsertQueue(obj);

            }
            catch (Exception ex)
            {
                this.Err = "��ʽ��SQL���ʱ����:" + ex.Message;
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSQL, strParam);


        }

        /// <summary>
        /// ����޸Ķ��в����б�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int UpdateQueue(Neusoft.HISFC.Models.Nurse.Queue obj)
        {
            string strSql = "";
            string[] strParam;
            if (this.Sql.GetSql("Nurse.Queue.UpdateQueue", ref strSql) == -1) return -1;
            try
            {
                //��ȡ�����б�
                strParam = this.myGetParmUpdateQueue(obj);
                strSql = string.Format(strSql, strParam);
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
        /// ɾ������
        /// </summary>
        /// <param name="queueNo"></param>
        /// <returns></returns>
        public int DelQueue(string queueNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Nurse.DelQueue.1", ref strSql) == -1) return -1;
            try
            {
                strSql = string.Format(strSql, queueNo);
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
        /// ����ʿվ/��������/����ѯ���������Ϣ
        /// </summary>
        /// <param name="nurseID"></param>
        /// <param name="queueDate"></param>
        /// <param name="noonID"></param>
        /// <returns></returns>
        public ArrayList Query(string nurseID, DateTime queueDate, string noonID)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Nurse.Queue.Query.1", ref sql) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
                return null;
            }

            if (this.Sql.GetSql("Nurse.Queue.Query.2", ref where) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.2]";
                return null;
            }

            try
            {
                where = string.Format(where, nurseID, queueDate.ToString(), noonID);
            }
            catch (Exception e)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            sql = sql + " " + where;

            return this.Query(sql);
        }
        /// <summary>
        /// ����ʿվ/��������/���/���� ��ѯ���������Ϣ
        /// </summary>
        /// <param name="nurseID"></param>
        /// <param name="queueDate"></param>
        /// <param name="noonID"></param>
        /// <returns></returns>
        public ArrayList Query(string nurseID, DateTime queueDate, string noonID, string deptCode)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Nurse.Queue.Query.1", ref sql) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
                return null;
            }

            if (this.Sql.GetSql("Nurse.Queue.Query.5", ref where) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.5]";
                return null;
            }

            try
            {
                where = string.Format(where, nurseID, queueDate.ToString(), noonID, deptCode);
            }
            catch (Exception e)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1+5]" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            sql = sql + " " + where;

            return this.Query(sql);
        }
        /// <summary>
        /// �������к�ȥ������Ϣ
        /// </summary>
        /// <param name="queueID">���к�</param> 
        /// <returns></returns>
        public ArrayList QueryByQueueID(string queueID)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Nurse.Queue.Query.1", ref sql) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
                return null;
            }

            if (this.Sql.GetSql("Nurse.Queue.Query.7", ref where) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.7]";
                return null;
            }

            try
            {
                where = string.Format(where, queueID);
            }
            catch (Exception e)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1+7]" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            sql = sql + " " + where;

            return this.Query(sql);
        }

        /// <summary>
        /// ��ѯmet_nuo_assignrecord���Ƿ��з��������������Ƿ�����
        /// </summary>
        /// <param name="roomID">���Ҵ���</param>
        /// <returns></returns>
        public int QueryQueueByQueueID(string roomID, string currentDateStr)
        {
            string strsql = string.Empty;
            if (this.Sql.GetSql("Nurse.Room.GetQueueByQueueID", ref strsql) == -1)
            {
                this.Err = "�õ�Nurse.Room.GetQueueByQueueIDʧ��";
                return -1;
            }

            try
            {
                strsql = string.Format(strsql, roomID, currentDateStr);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }

            return Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(strsql));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nurseID"></param>
        /// <param name="queueDate"></param>
        /// <returns></returns>
        public ArrayList Query(string nurseID, string queueDate)
        {
            string sql = "", where = "";
            string strBegin = queueDate + " " + "00:00:00", strEnd = queueDate + " " + "23:59:59";

            if (this.Sql.GetSql("Nurse.Queue.Query.1", ref sql) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
                return null;
            }

            if (this.Sql.GetSql("Nurse.Queue.Query.3", ref where) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.3]";
                return null;
            }
            try
            {
                where = string.Format(where, nurseID, strBegin, strEnd);
            }
            catch (Exception e)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            sql = sql + " " + where;

            return this.Query(sql);
        }
        /// <summary>
        /// ����sql��ѯ������Ϣ
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected ArrayList Query(string sql)
        {
            if (this.ExecQuery(sql) == -1)
            {
                this.Err = "����sql����!" + sql;
                this.ErrCode = "����sql����!" + sql;
                return null;
            }

            this.al = new ArrayList();

            try
            {
                while (this.Reader.Read())
                {
                    this.queue = new Neusoft.HISFC.Models.Nurse.Queue();

                    //������ʿվ
                    this.queue.Dept.ID = this.Reader[0].ToString();
                    //���д���
                    this.queue.ID = this.Reader[1].ToString();
                    //��������
                    this.queue.Name = this.Reader[2].ToString();
                    //������
                    this.queue.Noon.ID = this.Reader[3].ToString();

                    //��������[2007/03/27]
                    this.queue.User01 = this.Reader[4].ToString();

                    //��ʾ˳��
                    if (!this.Reader.IsDBNull(5))
                        this.queue.Order = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[5].ToString());
                    //�Ƿ���Ч
                    this.queue.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[6].ToString());
                    //��ע
                    this.queue.Memo = this.Reader[7].ToString();
                    //����Ա
                    this.queue.Oper.ID = this.Reader[8].ToString();
                    //����ʱ��
                    this.queue.Oper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[9].ToString());
                    //��������
                    this.queue.QueueDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[10].ToString());
                    //����ҽ��
                    this.queue.Doctor.ID = this.Reader[11].ToString();
                    //����
                    this.queue.SRoom.ID = this.Reader[12].ToString();
                    this.queue.SRoom.Name = this.Reader[13].ToString();
                    //��̨
                    this.queue.Console.ID = this.Reader[14].ToString();
                    this.queue.Console.Name = this.Reader[15].ToString();
                    //ר�ұ�־
                    this.queue.ExpertFlag = this.Reader[16].ToString();
                    //�������
                    this.queue.AssignDept.ID = this.Reader[17].ToString();
                    this.queue.AssignDept.Name = this.Reader[18].ToString();
                    this.queue.WaitingCount = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[19]);
                    this.al.Add(this.queue);
                }

                this.Reader.Close();
            }
            catch (Exception e)
            {
                if (!this.Reader.IsClosed) this.Reader.Close();
                this.Err = "��ѯ���ﻤʿվ���������Ϣ����!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            return this.al;
        }

        /// <summary>
        ///  ���ݻ���վ��ʱ�� ��ȡһ�����ٴ��������Ķ���ʵ��
        /// </summary>
        /// <param name="nurseID"></param>
        /// <param name="queueDate"></param>
        /// <param name="noonID"></param>
        /// <returns></returns>
        public ArrayList QueryMinCount(string nurseID, DateTime queueDate, string noonID)
        {
            string sql = "", where = "";

            if (this.Sql.GetSql("Nurse.Queue.Query.1", ref sql) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1]";
                return null;
            }

            if (this.Sql.GetSql("Nurse.Queue.Query.4", ref where) == -1)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.4]";
                return null;
            }

            try
            {
                where = string.Format(where, nurseID, queueDate.ToString(), noonID);
            }
            catch (Exception e)
            {
                this.Err = "��ѯ���������Ϣ����![Nurse.Queue.Query.1+4]" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            sql = sql + " " + where;
            ArrayList al = new ArrayList();
            al = this.Query(sql);

            if (al == null || al.Count <= 0) return null;
            return al;
            //			Neusoft.HISFC.Models.Nurse.Queue info = (Neusoft.HISFC.Models.Nurse.Queue)al[0];
            //			return info;
        }


        /// <summary>
        /// ��ѯ������ж�����̨�� ����,������Ϣ
        /// </summary>
        /// <returns></returns>
        public ArrayList Query(string nurse, Neusoft.HISFC.Models.Nurse.EnuTriageStatus status,
            DateTime dtfrom, DateTime dtto, string noon)
        {
            string sql = "";

            if (this.Sql.GetSql("Nurse.Assign.Auto.Query.3", ref sql) == -1)
            {
                this.Err = "��ѯsql����,����Ϊ[Nurse.Assign.Auto.Query.3]";
                this.ErrCode = "��ѯsql����,����Ϊ[Nurse.Assign.Auto.Query.3]";
                return null;
            }
            try
            {
                sql = string.Format(sql, nurse, status, dtfrom.ToString(), dtto.ToString(), noon);
            }
            catch (Exception e)
            {
                this.Err = "�ַ�ת������!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            if (this.ExecQuery(sql) == -1)
            {
                this.Err = "����sql����!" + sql;
                this.ErrCode = "����sql����!" + sql;
                return null;
            }
            this.al = new ArrayList();

            try
            {
                while (this.Reader.Read())
                {
                    this.queue = new Neusoft.HISFC.Models.Nurse.Queue();
                    this.queue.ID = this.Reader[0].ToString();
                    this.queue.Console.ID = this.Reader[1].ToString();
                    this.queue.WaitingCount = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[2]);
                    this.al.Add(this.queue);
                }

                this.Reader.Close();
            }
            catch (Exception e)
            {
                if (!this.Reader.IsClosed) this.Reader.Close();
                this.Err = "��ѯ������Ϣ����!" + e.Message;
                this.ErrCode = e.Message;
                return null;
            }

            return this.al;
        }
        /// <summary>
        /// ����������̨������ڶ������ڲ�ѯ��̨�Ƿ�����
        /// </summary>
        ///  
        /// <param name="consoleCode">��̨</param>
        /// <param name="noonID">���</param>
        /// <param name="queueDate">����ʱ��</param>
        /// <returns>false��ȡsql��������̨�ѱ�ʹ�� true ��û�б�ʹ��</returns>
        public bool QueryUsed(string consoleCode,string noonID,string queueDate)
        {
            string sqlStr = string.Empty;
            int returnValue = this.Sql.GetSql("Nurse.Room.GetQueueByConsolecodeNoonIdQDate", ref sqlStr);
            if (returnValue < 0)
            {
                this.Err = "��ѯsql����,����Ϊ[Nurse.Room.GetQueueByConsolecodeNoonIdQDate]";
                this.ErrCode = "��ѯsql����,����Ϊ[Nurse.Room.GetQueueByConsolecodeNoonIdQDate]";
                return false;
            }
            try
            {
                sqlStr = string.Format(sqlStr,consoleCode, noonID, queueDate);
            }
            catch (Exception e)
            {
                
                this.Err = "�ַ�ת������!" + e.Message;
                this.ErrCode = e.Message;
            }
            int myReturn = Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(sqlStr));
            if (myReturn > 0)
            {
                
                this.Err = "����̨�Ѿ���ʹ�ã���ѡ��������̨";

                return false;
            }
            else if (myReturn < 0)
            {
                this.Err = "��ѯʧ��";
                return false;
            }
            return true;

        }

        /// <summary>
        /// �ж��Ƿ��л���
        /// </summary>
        /// <param name="roomID">���ұ��</param>
        /// <param name="seatID">��̨���</param>
        /// <param name="queueID">���б��</param>
        /// <param name="noonID">�����</param>
        /// <returns>true,�л���</returns>
        public bool ExistPatient(string roomID, string seatID, string queueID, string noonID)
        {
            string strsql = "";
            if (this.Sql.GetSql("Nurse.Queue.Query.6", ref strsql) == -1)
            {
                return true;
            }

            try
            {
                strsql = string.Format(strsql, roomID, seatID, queueID, noonID);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return true;
            }
            try
            {
                this.ExecSqlReturnOne(strsql);
            }
            catch (Exception ex)
            {

                string aaaa = ex.Message;
            }
                    
               
            string retv = this.ExecSqlReturnOne(strsql);

            if (retv == null || retv.Trim().Equals("0"))
            {
                return false;
            }

            return true;
        }
    }
}
