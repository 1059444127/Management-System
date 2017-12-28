using System;

namespace Neusoft.FrameWork.Management
{
	/// <summary>
	/// Connection ��ժҪ˵����s
	/// </summary>
	public class Connection
	{
		private static System.Data.IDbConnection connection =null;
		private static System.Data.IDbConnection nearconnection =null;
		private static Neusoft.FrameWork.Management.Sql sql =null;
		private Connection(string ConnectionString)
		{
			
		}
		private Connection()
		{

		}
		/// <summary>
		/// ���ݿ�����ʵ��
		/// </summary>
		public static System.Data.IDbConnection Instance
		{
			get
			{   
				return connection;
			}
			set
			{
				connection = value;
			}
		}

		/// <summary>
		/// �������ݿ�����ʵ��
		/// </summary>
		public static System.Data.IDbConnection NearInstance
		{
			get
			{
				return nearconnection;
			}
			set
			{
				nearconnection = value;
			}
		}
		/// <summary>
		/// sql������
		/// </summary>
		public static Neusoft.FrameWork.Management.Sql Sql
		{
			get
			{
				return sql;
			}
			set
			{
				sql = value;
			}
		}

		private static Neusoft.FrameWork.Models.NeuObject oper =new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ����Ա����ת�ɵ�ǰ����Ա��
		/// </summary>
		public static Neusoft.FrameWork.Models.NeuObject Operator
		{
			get
			{
				return oper;
			}
			set
			{
				oper = value;
                Server.Function.Manager.SetOperation(value);
			}
        }

        ////{7F75F400-8180-485f-B968-E95E472FF9AA}
        //private static Neusoft.FrameWork.Models.NeuObject hospital = new Neusoft.FrameWork.Models.NeuObject();

        ///// <summary>
        ///// ҽԺ��Ϣ
        ///// </summary>
        //public static Neusoft.FrameWork.Models.NeuObject Hospital
        //{
        //    get
        //    {
   
        //        return hospital;
        //    }
        //    set
        //    {
        //        hospital = value;
        //    }
        //}
        //{7F75F400-8180-485f-B968-E95E472FF9AA}
        public static string CoreHospatialCode = "CORE_HIS50";

        #region �������뵥��ʼ�� addby zhangkj {A93EE0CA-F50E-4142-8477-761E257AC974}

        private static Neusoft.FrameWork.Models.NeuObject applyoper = new Neusoft.FrameWork.Models.NeuObject();
        /// <summary>
        /// ����Ա����ת�ɵ�ǰ����Ա��
        /// </summary>
        public static Neusoft.FrameWork.Models.NeuObject ApplyOperator
        {
            get
            {
                return applyoper;
            }
            set
            {
                applyoper = value;
                //Server.Function.Manager.SetOperation(value);
            }
        }
        #endregion
        private static bool bIsWeb = false;
		/// <summary>
		/// �Ƿ�web
		/// </summary>
		public static bool IsWeb 
		{
			get
			{
				return bIsWeb;
			}
			set
			{
				bIsWeb = value;
			}
		}

		private static bool bIsHistory = false;
		/// <summary>
		/// �Ƿ��ѯ��ʷ��
		/// </summary>
		public static bool IsHistory
		{
			get
			{
				return bIsHistory;
			}
			set
			{
				bIsHistory = value;
			}
		}

        /// <summary>
        /// ϵͳ·��url.xml
        /// http:\\localhost\
        /// </summary>
        public static string SystemPath = "";

        /// <summary>
        /// �Ƿ���Կ�
        /// </summary>
        public static bool IsTestDB = false;

        /// <summary>
        /// �������Ӵ�
        /// </summary>
        public static string DataSouceString = "";

        /// <summary>
        /// ����Ա����
        /// </summary>
        public static string ManagerPWD = "";

        /// <summary>
        /// ��Ч���ӳ�ʱʱ��
        /// ��Сʱ����6��
        /// Ϊ0ʱ���ͷ�
        /// </summary>
        public static int TimeOutSecond = 38;

        /// <summary>
        /// ���������
        /// 0������
        /// </summary>
        public static int MaxConnection = 0;

        /// <summary>
        ///�������Ƿ�ʹ��
        /// </summary>
        public static bool IsSocketUsed = false;

        /// <summary>
        /// {38B71167-48DF-4972-9857-3EAFDD6466B0} 
        /// </summary>
        public static string mapPath = "";

        /// <summary>
        /// DESKey {D515E09B-E299-47e0-BF19-EDFDB6E4C775}
        /// HIS���ܽ���deskey����ͬ��lisence��deskey
        /// </summary>
        public static string DESKey = "Core_H_N";

        /// <summary>
        /// ��������ļ�{38B71167-48DF-4972-9857-3EAFDD6466B0}
        /// </summary>
        /// <returns></returns>
        public static int GetSettingPB(string path, out string err)
        {
            int i = path.IndexOf('\0');
            if (i != -1)
            {
                path = path.Substring(0, i);
            }

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(path + "url.xml");
            }
            catch (Exception ex)
            {
                err = ("װ��urlʧ�ܣ�\n" + ex.Message);
                return -1;
            }
            System.Xml.XmlNode node;
            #region �ĳɶ�һ����ַ�б���������ʵ��˫��Ч��  {A5B6BD9E-68A1-45f5-BFE2-7EF0604AAAED}
            bool isUseUrlList = false;
            try
            {
                //У���õ�node
                System.Xml.XmlNode nodeForCheck;
                nodeForCheck = doc.SelectSingleNode("//root/dir");
                if (nodeForCheck == null)
                {
                    isUseUrlList = false;
                }
                else
                {
                    isUseUrlList = true;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                isUseUrlList = false;
            }
            #endregion
            if (isUseUrlList == false)
            {

                #region ԭ�ж���һ·���Ĵ��룬Ϊ�˼��ݱ���
                node = doc.SelectSingleNode("//dir");

                if (node == null)
                {
                    err = ("url����dir������");
                    return -1;
                }

                SystemPath = node.InnerText;

                string serverSettingFileName = "Profile.xml"; //�������ļ���

                try
                {
                    doc.Load(SystemPath + serverSettingFileName);
                }
                catch (Exception ex)
                {
                    err = ("װ��Profile.xmlʧ�ܣ�\n" + ex.Message);

                }
                #endregion
                #region �ĳɶ�һ����ַ�б���������ʵ��˫��Ч�� {A5B6BD9E-68A1-45f5-BFE2-7EF0604AAAED}
            }
            else
            {
                System.Xml.XmlNodeList xnl = doc.SelectNodes("//root/dir");
                if (xnl == null || xnl.Count == 0)
                {
                    err = ("url����dir������");
                    return -1;
                }

                int xnIdx = 0;
                foreach (System.Xml.XmlNode xn in xnl)
                {
                    SystemPath = xn.InnerText;

                    string serverSettingFileName = "HisProfile.xml"; //�������ļ���

                    try
                    {

                        doc.Load(SystemPath + serverSettingFileName);
                        break;
                    }
                    catch (Exception ex)
                    {

                        if (xnIdx == xnl.Count - 1)
                        {
                            err = ("װ��HisProfile.xmlʧ�ܣ�\n" + ex.Message);
                            return -1;
                        }
                        else
                        {
                            xnIdx++;
                            continue;
                        }
                    }
                }

            }
                #endregion
            node = doc.SelectSingleNode("/����/���ݿ�����");

            if (node == null)
            {
                err = ("û���ҵ����ݿ�����!");
                return -1;
            }

            string strDataSource = node.Attributes[0].Value;

            //�ж����Ӵ��Ƿ����{2480BEE8-92D0-484e-8D7E-2E24CC41C0C1}
            node = doc.SelectSingleNode("/����/����");
            if (node == null)
            {
                err = ("û���ҵ��Ƿ������Ϣ!");
                return -1;
            }
            string strCrypto = node.Attributes[0].Value;
            if (strCrypto.Trim().Equals("1"))
            {
                //{D515E09B-E299-47e0-BF19-EDFDB6E4C775}
                //strDataSource = Neusoft.HisDecrypt.Decrypt(strDataSource);
                strDataSource = Neusoft.HisCrypto.DESCryptoService.DESDecrypt(strDataSource,DESKey);
            }
            //END

            #region ���ݿ�����
            node = doc.SelectSingleNode("/����/���ݿ����");
            if (node != null)
            {

                DBType = GetDBType(node.Attributes[0].Value);//���ݿ������ж�.//{08F955BE-6313-47cc-AB3A-14897F4147B8}
            }


            #endregion
            DataSouceString = strDataSource;

            node = doc.SelectSingleNode("/����/����");

            if (node == null)
            {
                err = ("û���ҵ�SQL����!");
                return -1;
            }



            node = doc.SelectSingleNode("/����/����Ա");


            if (node == null)
            {
                err = ("û���ҵ�����Ա����!");
                return -1;
            }
            //{D515E09B-E299-47e0-BF19-EDFDB6E4C775}
            //ManagerPWD = Neusoft.HisDecrypt.Decrypt(node.Attributes[0].Value);
            ManagerPWD = Neusoft.HisCrypto.DESCryptoService.DESDecrypt(node.Attributes[0].Value,DESKey);

            node = doc.SelectSingleNode("/����/��ʽ��");
            if (node != null)
            {
                if (node.Attributes[0].Value == "0")
                {
                    IsTestDB = true;
                }
                else
                {
                    IsTestDB = false;
                }
            }
            node = doc.SelectSingleNode("/����/������");
            if (node != null)
            {
                MaxConnection = FrameWork.Function.NConvert.ToInt32(node.Attributes[0].Value);

                try
                {
                    TimeOutSecond = FrameWork.Function.NConvert.ToInt32(node.Attributes[1].Value);
                }
                catch
                {
                }

                if (TimeOutSecond < 6 && TimeOutSecond > 0)
                    TimeOutSecond = 6;//��Сʱ����
            }
            err = "";


            return 0;


        }
        /// <summary>
        /// ��������ļ�
        /// </summary>
        /// <returns></returns>
        public static int GetSetting(out string err)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(System.Windows.Forms.Application.StartupPath + @"\" + "url.xml");
            }
            catch (Exception ex)
            {
                err = ("װ��urlʧ�ܣ�\n" + ex.Message);
                return -1;
            }
            System.Xml.XmlNode node;
            #region �ĳɶ�һ����ַ�б���������ʵ��˫��Ч��  {A5B6BD9E-68A1-45f5-BFE2-7EF0604AAAED}
            bool isUseUrlList = false;
            try
            {
                //У���õ�node
                System.Xml.XmlNode nodeForCheck;
                nodeForCheck = doc.SelectSingleNode("//root/dir");
                if (nodeForCheck == null)
                {
                    isUseUrlList = false;
                }
                else
                {
                    isUseUrlList = true;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                isUseUrlList = false;
            }
            #endregion
            if (isUseUrlList == false)
            {
          
                #region ԭ�ж���һ·���Ĵ��룬Ϊ�˼��ݱ���
                node = doc.SelectSingleNode("//dir");

                if (node == null)
                {
                    err = ("url����dir������");
                    return -1;
                }

                SystemPath = node.InnerText;

                string serverSettingFileName = "Profile.xml"; //�������ļ���

                try
                {
                    doc.Load(SystemPath + serverSettingFileName);
                }
                catch (Exception ex)
                {
                    err =("װ��Profile.xmlʧ�ܣ�\n" + ex.Message);
                    
                }
                #endregion
                #region �ĳɶ�һ����ַ�б���������ʵ��˫��Ч�� {A5B6BD9E-68A1-45f5-BFE2-7EF0604AAAED}
            }
            else
            {
                System.Xml.XmlNodeList xnl = doc.SelectNodes("//root/dir");
                if (xnl == null || xnl.Count == 0)
                {
                    err  =("url����dir������");
                    return -1;
                }

                int xnIdx = 0;
                foreach (System.Xml.XmlNode xn in xnl)
                {
                    SystemPath = xn.InnerText;

                    string serverSettingFileName = "HisProfile.xml"; //�������ļ���

                    try
                    {

                        doc.Load(SystemPath + serverSettingFileName);
                        break;
                    }
                    catch (Exception ex)
                    {

                        if (xnIdx == xnl.Count - 1)
                        {
                           err =("װ��HisProfile.xmlʧ�ܣ�\n" + ex.Message);
                            return -1;
                        }
                        else
                        {
                            xnIdx++;
                            continue;
                        }
                    }
                }

            }
            #endregion
            node = doc.SelectSingleNode("/����/���ݿ�����");

            if (node == null)
            {
                err = ("û���ҵ����ݿ�����!");
                return -1;
            }

            string strDataSource = node.Attributes[0].Value;

            //�ж����Ӵ��Ƿ����{2480BEE8-92D0-484e-8D7E-2E24CC41C0C1}
            node = doc.SelectSingleNode("/����/����");
            if (node == null)
            {
                err = ("û���ҵ��Ƿ������Ϣ!");
                return -1;
            }
            string strCrypto = node.Attributes[0].Value;
            if (strCrypto.Trim().Equals("1"))
            {
                //{D515E09B-E299-47e0-BF19-EDFDB6E4C775}
                //strDataSource = Neusoft.HisDecrypt.Decrypt(strDataSource);
                strDataSource = Neusoft.HisCrypto.DESCryptoService.DESDecrypt(strDataSource,DESKey);
            }
            //END

            #region ���ݿ�����
            node = doc.SelectSingleNode("/����/���ݿ����");
            if (node != null)
            {

                DBType = GetDBType(node.Attributes[0].Value);//���ݿ������ж�.//{08F955BE-6313-47cc-AB3A-14897F4147B8}
            }


            #endregion
            DataSouceString = strDataSource;

            node = doc.SelectSingleNode("/����/����");

            if (node == null)
            {
                err = ("û���ҵ�SQL����!");
                return -1;
            }

         

            node = doc.SelectSingleNode("/����/����Ա");


            if (node == null)
            {
                err = ("û���ҵ�����Ա����!");
                return -1;
            }
            //{D515E09B-E299-47e0-BF19-EDFDB6E4C775}
            //ManagerPWD = Neusoft.HisDecrypt.Decrypt(node.Attributes[0].Value);
            ManagerPWD = Neusoft.HisCrypto.DESCryptoService.DESDecrypt(node.Attributes[0].Value,DESKey);

            node = doc.SelectSingleNode("/����/��ʽ��");
            if (node != null)
            {
                if (node.Attributes[0].Value == "0")
                {
                    IsTestDB = true;
                }
                else
                {
                    IsTestDB = false;
                }
            }
            node = doc.SelectSingleNode("/����/������");
            if (node != null)
            {
                MaxConnection = FrameWork.Function.NConvert.ToInt32(node.Attributes[0].Value);

                try
                {
                    TimeOutSecond = FrameWork.Function.NConvert.ToInt32(node.Attributes[1].Value);
                }
                catch { }

                if (TimeOutSecond < 6 && TimeOutSecond > 0)
                    TimeOutSecond = 6;//��Сʱ����
            }
            //add start
            #region ����������
            node = doc.SelectSingleNode("/����/������");
            if (node != null)
            {
                if (node.Attributes.Count > 0)
                {
                    if (node.Attributes[0].Value == "0")//0����ʹ��������
                    {
                        IsSocketUsed = true;
                    }
                    else
                    {
                        IsSocketUsed = false;
                    }
                }
            }
            #endregion
            //add end

            err = "";

           
            return 0;


        }

        internal static EnumDBType GetDBType(string dbtype)
        {
            EnumDBType dt;
            try
            {
                switch (dbtype.ToUpper().Trim())
                {
                    case "ORACLE":
                        dt = (EnumDBType)0;
                        break;
                    case "SQLSERVER":
                        dt = (EnumDBType)1;
                        break;
                    case "DB2":
                        dt = (EnumDBType)2;
                        break;
                    case "SYSBASE":
                        dt = (EnumDBType)3;
                        break;
                    case "POSTGRESQL":
                        dt = (EnumDBType)4;
                        break;
                    case "OTHER":
                        dt = (EnumDBType)5;
                        break;
                    default:
                        dt = (EnumDBType)0;
                        break;
                }
            }
            catch
            {
                dt = EnumDBType.ORACLE;
            }

            return dt;
        }
        public static EnumDBType DBType = EnumDBType.ORACLE;
        /// <summary>
        /// ��̨���ݿ�����
        /// </summary>
        public  enum EnumDBType
        {
            ORACLE = 0,
            SQLSERVER,
            DB2,
            SYSBASE,
            POSTGRESQL,
            OTHER
        }
	}
}
