using System;
using System.Xml;//ʹ��Xml�����xmlDataSet�����
using System.Data;
namespace Neusoft.FrameWork.Management
{
    /// <summary>
    /// Database ���ݿ������ 
    /// con ��Ҫ����
    /// sql ��Ҫ����
    /// operator ��Ҫ����
    /// wolf 2004-6
    /// </summary>
    [System.Serializable]
    public abstract class Database : FrameWork.Models.NeuManageObject, IDataBase
    {
        /// <summary>
        /// ������ϸ������Ϣ��ʾ�ı߽�ֵ ��С�ڴ�����ֵ���������䣬��ʾ��Ϣ�����¡���
        /// </summary>
        public static int DetailAgeBoundry = 1;

        protected Neusoft.FrameWork.Models.NeuLog Logo;
        protected Neusoft.FrameWork.Models.NeuLog debugLogo;
        protected string ErrorException = "";
        private bool bDebug;
        protected string DebugSqlIndex = "Manager.Logo.GetDebug";
        protected string ErrorSqlIndex = "Manager.Logo.GetError";
        protected string DebugSql = "";
        protected string ErrorSql = "";
        protected IDataBase db = null;
        //protected Neusoft.NFC.Object.NeuManageObject obj = null;
        public Database()
        {
            //if (Neusoft.NFC.Management.Connection.Instance == null)
            //{
            //    this.Operator = Neusoft.NFC.Management.Connection.Operator;
            //    newDatabase("./err.log");
            //    return;
            //}

            //if (Neusoft.NFC.Management.Connection.Instance.GetType() == typeof(System.Data.OracleClient.OracleConnection))
            //{
            //    DBOracle dboracle = new DBOracle();
            //    db = dboracle as IDataBase;
            //    obj = dboracle as NFC.Object.NeuManageObject;
            //}
            //else if (Neusoft.NFC.Management.Connection.Instance.GetType() == typeof(System.Data.OleDb.OleDbConnection))
            //{
            //    DBOle dbole = new DBOle();
            //    db = dbole as IDataBase;
            //    obj = dbole as NFC.Object.NeuManageObject;
            //}
            //else if (Neusoft.NFC.Management.Connection.Instance.GetType() == typeof(Oracle.DataAccess.Client.OracleConnection))
            //{
            //    DBOracleAccess dbole = new DBOracleAccess();
            //    db = dbole as IDataBase;
            //    obj = dbole as NFC.Object.NeuManageObject;
            //}
            ////����DB2���ݿ�֧��// {EE483CDD-A76C-4058-B0D2-8E5C7C7EAE54}
            //else if (Neusoft.NFC.Management.Connection.Instance.GetType() == typeof(IBM.Data.DB2.DB2Connection))
            //{
            //    DB2Access db2Access = new DB2Access();
            //    db = db2Access as IDataBase;
            //    obj = db2Access as NFC.Object.NeuManageObject;
            //}

            //db.con = Neusoft.NFC.Management.Connection.Instance;
            //db.Sql = Neusoft.NFC.Management.Connection.Sql;
            //obj.Operator = Neusoft.NFC.Management.Connection.Operator;
            //this.Operator = Neusoft.NFC.Management.Connection.Operator;
            db = new Server.DBManager() as IDataBase;
            newDatabase("./err.log");

        }

        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        public new FrameWork.Models.NeuObject Operator
        {
            get
            {
                if (base.Operator == null || base.Operator.ID =="")
                    return Neusoft.FrameWork.Management.Connection.Operator;
                return base.Operator; ;
            }
            set
            {
                base.Operator = value;
            }
        }
        ////{7F75F400-8180-485f-B968-E95E472FF9AA}
        ///// <summary>
        ///// ҽԺ��Ϣ
        ///// </summary>
        //public new FrameWork.Models.NeuObject Hospital
        //{
        //    get
        //    {
        //        if (base.Hospital == null || string.IsNullOrEmpty(base.Hospital.ID))
        //        {
        //            return Neusoft.FrameWork.Management.Connection.Hospital;
        //        }
        //        return base.Hospital;
        //    }
        //    set
        //    {
        //        base.Hospital = value;
        //    }
        //}


        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="errLogoFileName">��־�ļ���</param>
        public Database(string errLogoFileName)
        {
            newDatabase(errLogoFileName);
        }
        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }


        /// <summary>
        /// �����ڴ� �ж��Ƿ�debug �ǹ���
        /// </summary>
        /// <param name="errLogoFileName">errLogoFileName</param>
        private void newDatabase(string errLogoFileName)
        {

            Logo = new Neusoft.FrameWork.Models.NeuLog(errLogoFileName);
            string debugLogoFileName = "./debugSql.log";
            if (System.IO.File.Exists(debugLogoFileName))
            {
                debugLogo = new Neusoft.FrameWork.Models.NeuLog(debugLogoFileName);
                bDebug = true;
            }
            else
            {
                bDebug = false;
            }

        }

        /// <summary>
        /// ת��sql���
        /// ͨ������ȫ�ֵ�sql�������з���
        /// </summary>
        public Sql Sql
        {
            get
            {
                return db.Sql;
            }
            set
            {
                db.Sql = value;
            }
        }
        /// <summary>
        /// ����sql
        /// </summary>
        /// <param name="sql"></param>
        public void SetSql(Sql sql)
        {
            this.Sql = sql;
        }

      

        #region д��־

        /// <summary>
        /// д������־
        /// </summary>
        public virtual void WriteErr()
        {
            this.Logo.WriteLog(this.GetType().ToString() + ":" + this.Err + this.ErrCode);
            if (bDebug)
            {
                this.debugLogo.WriteLog("Error:" + this.GetType().ToString() + ":" + this.Err + this.ErrCode);
            }
        }
        /// <summary>
        /// д������־
        /// </summary>
        /// <param name="strDebugInfo"></param>
        public virtual void WriteDebug(string strDebugInfo)
        {
            if (bDebug)
            {
                this.debugLogo.WriteLog("Debug:" + this.GetType().ToString() + ":" + strDebugInfo);
            }

        }

        #endregion

        #region IDataBase ��Ա
        

        /// <summary>
        /// ��ǰ���ݿ�����
        /// </summary>
        public IDbConnection con
        {
            get
            {

                // TODO:  ��� Database.con getter ʵ��
                if (db == null)
                {
                    return null;


                }
                return db.con;
            }
            set
            {
                // TODO:  ��� Database.con setter ʵ��
                if (db == null || db.con != value)
                {
                    //if (value.GetType() == typeof(System.Data.OracleClient.OracleConnection))
                    //{

                    //    DBOracle dboracle = new DBOracle();
                    //    db = dboracle as IDataBase;
                    //    obj = dboracle as NFC.Object.NeuManageObject;
                    //}
                    //else if (value.GetType() == typeof(System.Data.OleDb.OleDbConnection))
                    //{
                    //    DBOle dbole = new DBOle();
                    //    db = dbole as IDataBase;
                    //    obj = dbole as NFC.Object.NeuManageObject;
                    //}
                    //else if (value.GetType() == typeof(Oracle.DataAccess.Client.OracleConnection))
                    //{
                    //    DBOracleAccess dbole = new DBOracleAccess();
                    //    db = dbole as IDataBase;
                    //    obj = dbole as NFC.Object.NeuManageObject;
                    //}
                    ////����DB2֧��{C39315D8-8484-4aa2-93E9-5C50C725EA69}
                    //else if (value.GetType() == typeof(IBM.Data.DB2.DB2Connection))
                    //{
                    //    DB2Access db2Access = new DB2Access();
                    //    db = db2Access as IDataBase;
                    //    obj = db2Access as NFC.Object.NeuManageObject;
                    //}
                    //END
                    db = new Server.DBManager()  as IDataBase;
                }
                db.con = value;
            }
        }
        /// <summary>
        /// ��ǰreader
        /// �ǵ�����ر�Reader
        /// </summary>
        public IDataReader Reader
        {
            get
            {
    
                
                return db.Reader;
            }
        }
        /// <summary>
        /// ִ��Sql��TempReaderִ��
        /// DB2������ִ��
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecQueryByTempReader(string strSql)
        {
            OpenDB();
            int i = db.ExecQueryByTempReader(strSql);
            return i;
        }
        /// <summary>
        /// ��ʱ��Reader
        /// </summary>
        public IDataReader TempReader
        {
            get
            {
               
                return db.TempReader;
            }
        }
        /// <summary>
        /// ���õ�ǰ����
        /// </summary>
        /// <param name="Trans"></param>
        public void SetTrans(IDbTransaction Trans)
        {

            db.SetTrans(Trans);
            this.myTrans = Trans;
        }

        /// <summary>
        /// ����
        /// </summary>
        private System.Data.IDbTransaction myTrans;
        /// <summary>
        /// ��ǰ����
        /// </summary>
        protected System.Data.IDbTransaction Trans
        {
            get
            {
                return this.myTrans;
            }
        }

        private void getErr()
        {
            this.Err = db.Err;
            this.ErrCode = db.ErrCode;
            this.DBErrCode = db.DBErrCode;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="strConnectString"></param>
        /// <returns></returns>
        public int Connect(string strConnectString)
        {
            // TODO:  ��� Database.Connect ʵ��
            int i = db.Connect(strConnectString);
            this.getErr();
            return i;
        }
        /// <summary>
        /// ִ�зǲ�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecNoQuery(string strSql)
        {
            // TODO:  ��� Database.ExecNoQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecNoQuery(strSql);
            CloseDB();
            this.getErr();
            return i;
        }

        /// <summary>
        ///  ִ�зǲ�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecNoQuery(string strSql, params string[] parms)
        {
            // TODO:  ��� Database.ExecNoQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecNoQuery(strSql, parms);
            CloseDB();
            this.getErr();
            return i;
        }

        /// <summary>
        /// ִ�зǲ�ѯSqlͨ��Sql����
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecNoQueryByIndex(string index, params string[] parms)
        {
            string strSql = "";
            if (this.Sql.GetSql(index, ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            OpenDB();
            int i = this.ExecNoQuery(strSql, parms);
            CloseDB();
            this.getErr();
            return i;
        }
        /// <summary>
        ///  ִ�зǲ�ѯsql	
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecQuery(string strSql)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecQuery(strSql);
            this.getErr();
            return i;
        }
        /// <summary>
        ///  ִ�зǲ�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecQuery(string strSql, params string[] parms)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecQuery(strSql, parms);
            this.getErr();
            return i;
        }
        /// <summary>
        ///  ִ�в�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strDataSet"></param>
        /// <returns></returns>
        public int ExecQuery(string strSql, ref string strDataSet)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecQuery(strSql, ref strDataSet);
            this.getErr();
            return i;
        }
       
        /// <summary>
        ///  ִ�в�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strDataSet"></param>
        /// <param name="strXSLFileName"></param>
        /// <returns></returns>
        public int ExecQuery(string strSql, ref string strDataSet, string strXSLFileName)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecQuery(strSql, ref  strDataSet, strXSLFileName);
            this.getErr();
            return i;
        }
        /// <summary>
        ///  ִ�в�ѯsql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="DataSet"></param>
        /// <returns></returns>
        public int ExecQuery(string strSql, ref DataSet DataSet)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecQuery(strSql, ref  DataSet);
            this.getErr();
            return i;
        }
        /// <summary>
        /// ִ�в�ѯsql
        /// </summary>
        /// <param name="indexes"></param>
        /// <param name="dataSet"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecQuery(string[] indexes, ref DataSet dataSet, params string[] parms)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            string strSql = "";  //���SELECT���

            if (indexes.Length == 0)
            {
                this.Err = "��Ч�Ĳ�����sql��������indexes����Ϊ��";
                return -1;
            }

            //ȡSELECT���
            foreach (string index in indexes)
            {
                string s = "";
                if (this.Sql.GetSql(index, ref s) == -1)
                {
                    this.Err = "û���ҵ�" + index + "�ֶ�!";
                    return -1;
                }

                strSql = strSql + " " + s;
            }

            //���ݲ���parms��ʽ��sql��䡣
            try
            {
                strSql = string.Format(strSql, parms);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            int i = this.ExecQuery(strSql, ref  dataSet);
            this.getErr();
            return i;
        }
        /// <summary>
        /// ִ�в�ѯsql
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataSet"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecQuery(string index, ref DataSet dataSet, params string[] parms)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            string[] indexes = { index };
            int i = this.ExecQuery(indexes, ref  dataSet, parms);
            this.getErr();
            return i;
        }

        /// <summary>
        ///  ִ�в�ѯsqlͨ��sqlIndex
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int ExecQueryByIndex(string index, params string[] parms)
        {
            // TODO:  ��� Database.ExecQuery ʵ��
            string strSql = "";
            if (this.Sql.GetSql(index, ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            int i = this.ExecQuery(strSql, parms);
            this.getErr();
            return i;
        }

        /// <summary>
        /// ִ��sql����һ����¼��Ϣ
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public string ExecSqlReturnOne(string strSql)
        {
            // TODO:  ��� Database.ExecSqlReturnOne ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            string i = db.ExecSqlReturnOne(strSql);
            CloseDB();
            return i;
        }

        /// <summary>
        /// ִ��sql����һ����¼��Ϣ
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="defaultstring"></param>
        /// <returns></returns>
        public string ExecSqlReturnOne(string strSql, string defaultstring)
        {
            // TODO:  ��� Database.ExecSqlReturnOne ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            string s= db.ExecSqlReturnOne(strSql, defaultstring);
            CloseDB();
            return s;
        }

        /// <summary>
        /// ִ������Blob�ֶ�sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="ImageData"></param>
        /// <returns></returns>
        public int InputBlob(string strSql, byte[] ImageData)
        {
            // TODO:  ��� Database.InputBlob ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.InputBlob(strSql, ImageData);
            CloseDB();
            this.getErr();
            return i;
        }

        /// <summary>
        /// ��ȡBlob�ֶ�sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public byte[] OutputBlob(string strSql)
        {
            // TODO:  ��� Database.OutputBlob ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();       
            byte[] bb = db.OutputBlob(strSql);
            CloseDB();
            return bb;
        }

        /// <summary>
        /// ִ������Long�ֶ�sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int InputLong(string strSql, string data)
        {
            // TODO:  ��� Database.InputLong ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();   
            int i = db.InputLong(strSql, data);
            CloseDB();
            this.getErr();
            return i;
        }

        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="Return"></param>
        /// <returns></returns>
        public int ExecEvent(string strSql, ref string Return)
        {
            // TODO:  ��� Database.ExecEvent ʵ��
            Neusoft.FrameWork.Public.TableConvert.ConverTable(ref strSql);
            OpenDB();
            int i = db.ExecEvent(strSql, ref  Return);
            CloseDB();
            this.getErr();
            return i;
        }

        /// <summary>
        /// ���ϵͳʱ��
        /// </summary>
        /// <returns></returns>
        public string GetSysDateTime()
        {
            // TODO:  ��� Database.GetSysDateTime ʵ��
            OpenDB();
            string s= db.GetSysDateTime();
            CloseDB();
            return s;
        }

        /// <summary>
        /// ���ϵͳʱ��
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetSysDateTime(string format)
        {
            // TODO:  ��� Database.GetSysDateTime ʵ��
            OpenDB();
            string s= db.GetSysDateTime(format);
            CloseDB();
            return s;
        }

        /// <summary>
        /// ���ϵͳʱ��
        /// </summary>
        /// <returns></returns>
        public DateTime GetDateTimeFromSysDateTime()
        {
            // TODO:  ��� Database.GetDateTimeFromSysDateTime ʵ��
            
            OpenDB();
            DateTime s = db.GetDateTimeFromSysDateTime();
            CloseDB();
            return s;
        }

        /// <summary>
        /// ���ϵͳ����
        /// </summary>
        /// <returns></returns>
        public string GetSysDate()
        {
            // TODO:  ��� Database.GetSysDate ʵ��
            OpenDB();
            string s = db.GetSysDate();
            CloseDB();
            return s;
        }

        /// <summary>
        /// ���ϵͳ����
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetSysDate(string format)
        {
            // TODO:  ��� Database.GetSysDate ʵ��
            OpenDB();
            string s = db.GetSysDate(format);
            CloseDB();
            return s;
        }

        /// <summary>
        /// ���ϵͳ���ڣ�20050505
        /// </summary>
        /// <returns></returns>
        public string GetSysDateNoBar()
        {
            // TODO:  ��� Database.GetSysDateNoBar ʵ��
            OpenDB();
            string s = db.GetSysDateNoBar();
            CloseDB();
            return s;
        }

        /// <summary>
        /// ������к�
        /// </summary>
        /// <param name="GetSqlIndex">���к�Sql</param>
        /// <returns></returns>
        public string GetSequence(string GetSqlIndex)
        {
            // TODO:  ��� Database.GetSequence ʵ��
            string strSQL = "";
            if (this.Sql.GetSql(GetSqlIndex, ref strSQL) == -1)
            {
                this.Err = "SQL����:" + GetSqlIndex + " �����ڣ�";
                return null;
            }
            string strReturn = this.ExecSqlReturnOne(strSQL);
            if (strReturn == "-1")
            {
                this.Err = "ȡ���к�" + GetSqlIndex + "ʱ����" + this.Err;
                return null;
            }

            this.getErr();

            return strReturn;
        }

        /// <summary>
        /// ���ݳ������ڻ������
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public string GetAge(DateTime birthday)
        {
            //ȡϵͳʱ��
            DateTime sysDate = this.GetDateTimeFromSysDateTime();
            return this.GetAge( birthday, sysDate, false );
        }

        /// <summary>
        /// ���ݳ������ڻ������
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public string GetAge(DateTime birthday,bool isDetailFormat)
        {
            //ȡϵͳʱ��
            DateTime sysDate = this.GetDateTimeFromSysDateTime();

            return this.GetAge( birthday, sysDate, isDetailFormat );
        }

        
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="sysDate"></param>
        /// <returns></returns>
        public string GetAge(DateTime birthday, DateTime sysDate)
        {
            return this.GetAge( birthday, sysDate, false );
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="sysDate"></param>
        /// <returns></returns>
        public string GetAge(DateTime birthday, DateTime sysDate,bool detailFormat)
        {
            // TODO:  ��� Database.GetAge ʵ��
            try
            {
                //ȡʱ����
                TimeSpan s = new TimeSpan(sysDate.Ticks - birthday.Ticks);
                int iYear = 0, iMonth = 0, iDay = 0;
                if(sysDate.Year > birthday.Year)
                {
                    //�����´��ڵ�ǰʱ����
                    if (birthday.Month > sysDate.Month)
                    {
                        iYear = sysDate.Year - birthday.Year - 1;
                        if (birthday.Day > sysDate.Day)
                        {
                            iMonth = 11 + sysDate.Month - birthday.Month;
                            iDay = Function.NConvert.ToDateTime(sysDate.Year.ToString() + "-" + sysDate.Month.ToString() + "-" + "01").AddDays(-1).Day + sysDate.Day - birthday.Day;

                        }
                        else
                        {
                            iMonth = 12 + sysDate.Month - birthday.Month;
                            iDay = sysDate.Day - birthday.Day;
                        }
                        //return iYear.ToString() + "��" + iMonth.ToString() + "��"+iDay.ToString()+"��";
                    }

                    else if (birthday.Month == sysDate.Month)
                    {
                        if (birthday.Day > sysDate.Day)
                        {
                            iYear = sysDate.Year - birthday.Year - 1;
                            iMonth = sysDate.Month - birthday.Month + 11;
                            iDay = Function.NConvert.ToDateTime(sysDate.Year.ToString() + "-" + sysDate.Month.ToString() + "-" + "01").AddDays(-1).Day + sysDate.Day - birthday.Day;
                        }
                        else
                        {
                            iYear = sysDate.Year - birthday.Year;
                            iMonth = sysDate.Month - birthday.Month;
                            iDay = sysDate.Day - birthday.Day;
                        }
                    }
                    else
                    {
                        iYear = sysDate.Year - birthday.Year;
                        if (birthday.Day > sysDate.Day)
                        {
                            iMonth = sysDate.Month - birthday.Month - 1;

                            iDay = Function.NConvert.ToDateTime(sysDate.Year.ToString() + "-" + sysDate.Month.ToString() + "-" + "01").AddDays(-1).Day + sysDate.Day - birthday.Day;

                        }
                        else
                        {
                            iMonth = sysDate.Month - birthday.Month;
                            iDay = sysDate.Day - birthday.Day;
                        }

                    }
                }
                else
                {
                    //if (s.TotalDays >= 365)
                    //{
                    //    //���ڵ���365��,������
                    //    iYear = (int)(s.TotalDays / 365);
                    //    return iYear.ToString() + "��";
                    //}
                    //else if (s.TotalDays >= 30)
                    //{
                    //    //С��365���Ҵ��ڵ���30��,������
                    //    iYear = (int)(s.TotalDays / 30);
                    //    return iYear.ToString() + "��";
                    //}
                    //else
                    //{
                    //    //С��30��,������
                    //    iYear = (int)s.TotalDays + 1;
                    //    return iYear.ToString() + "��";
                    //}
                    if (birthday.Day > sysDate.Day)
                    {
                        iMonth = sysDate.Month - birthday.Month - 1;
                        iDay = Function.NConvert.ToDateTime(sysDate.Year.ToString() + "-" + sysDate.Month.ToString() + "-" + "01").AddDays(-1).Day + sysDate.Day - birthday.Day;
                    }
                    else
                    {
                        iMonth = sysDate.Month - birthday.Month;
                        iDay = sysDate.Day - birthday.Day;
                    }
                    
                }

                //{FA29D458-B9A3-493f-AAE8-86D593BEF724}  �������䷵����Ϣ��ʽ ͨ��DetailAgeBoundry���������� �ɿ�����ϸ������ʾ��ʽ �ò����ݱ��� ���ṩ����

                if (detailFormat == true)       //ֱ����ʾ��ϸ��Ϣ
                {
                    return iYear.ToString().PadLeft( 3, ' ' ) + "��" + iMonth.ToString().PadLeft( 2, ' ' ) + "��" + iDay.ToString().PadLeft( 2, ' ' ) + "��";
                }

                if (iYear >= DetailAgeBoundry)  //������ϸ�߽���ʾ����
                {
                    return iYear.ToString().PadLeft( 3, ' ' ) + "��";
                }

                if (iYear == 0)     //��Ϊ0
                {
                    if (iMonth == 0)    //��Ϊ0
                    {
                        if (iDay == 0)  //��Ϊ0
                        {
                            iDay = 1;
                        }
                        return iDay.ToString().PadLeft( 2, ' ' ) + "��";
                    }
                    else
                    {
                        if (iDay == 0)  //��Ϊ0 ֻ��ʾ��
                        {
                            return iMonth.ToString().PadLeft( 2, ' ' ) + "��";
                        }
                    }

                    return iMonth.ToString().PadLeft( 2, ' ' ) + "��" + iDay.ToString().PadLeft( 2, ' ' ) + "��";
                }
                else
                {
                    if (iMonth == 0)
                    {
                        if (iDay == 0)
                        {
                            return iYear.ToString().PadLeft( 3, ' ' ) + "��";
                        }
                        else
                        {
                            return iYear.ToString().PadLeft( 3, ' ' ) + "��" + iDay.ToString().PadLeft( 2, ' ' ) + "��";
                        }
                    }
                    else
                    {
                        if (iDay == 0)
                        {
                            return iYear.ToString().PadLeft( 3, ' ' ) + "��" + iMonth.ToString().PadLeft( 2, ' ' ) + "��"; 
                        }
                    }

                    return iYear.ToString().PadLeft( 3, ' ' ) + "��" + iMonth.ToString().PadLeft( 2, ' ' ) + "��" + iDay.ToString().PadLeft( 2, ' ' ) + "��";
                }
            }
            catch
            {
                return "";
            }
        }

        

        #endregion
        
        #region ���ݿ����� ����

        private void OpenDB()
        {
            //Server.Function manager = new Server.Function();
            //Server.Function.Manager.OpenDB();
        }

        private void CloseDB()
        {
            //Server.Function manager = new Neusoft.NFC.Server.Function();
            //Server.Function.Manager.CloseDB();
        }
        #endregion

        
    }


}
