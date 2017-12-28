using System;
using System.Data;
namespace Neusoft.FrameWork.Management
{
	/// <summary>
	/// IDataBase ��ժҪ˵����
	/// </summary>
	public interface IDataBase
    {

        #region �ֶ�
        string Err
        {
            get;
            set;
        }
        string ErrCode
        {
            get;
            set;
        }
        int DBErrCode
        {
            get;
            set;
        }
        #endregion

        #region ����

        /// <summary>
        /// ����
        /// </summary>
        System.Data.IDbConnection con
        {
            get;
            set;
        }

        /// <summary>
        /// sql���
        /// </summary>
        Neusoft.FrameWork.Management.Sql Sql
        {
            get;
            set;
        }



        /// <summary>
        /// Reader
        /// </summary>
        System.Data.IDataReader Reader
        {
            get;
        }

        /// <summary>
        /// TempReader
        /// </summary>
        System.Data.IDataReader TempReader
        {
            get;
        }
        #endregion

        #region ����
        /// <summary>
        /// �豸����
        /// </summary>
        /// <param name="Trans">���������</param>
        void SetTrans(System.Data.IDbTransaction Trans);

		/// <summary>
		///  �������� 
		/// </summary>
		/// <param name="strConnectString">���ӵ��ַ���</param>
		/// <returns>0 �ɹ� -1 ʧ��</returns>
		int Connect(string strConnectString);

		/// <summary>
		/// ִ�зǲ�ѯ���
		/// </summary>
		/// <param name="strSql">ִ��sql���</param>
		/// <returns>ִ��sql���Ӱ������� 0ִ�е�����,-1û��ִ���д��󣬶���update,insert,del�ⶼΪ-1��>0�ɹ�������</returns>
		int ExecNoQuery(string strSql);
		
		/// <summary>
        /// ִ�зǲ�ѯ���
		/// </summary>
		/// <param name="strSql">ִ�е�sql���</param>
		/// <param name="parms">������ַ�������</param>
        /// <returns>ִ��sql���Ӱ�������0ִ�е�����,-1û��ִ���д��󣬶���update,insert,del�ⶼΪ-1��>0�ɹ�������</returns>
		int ExecNoQuery(string strSql,params string[] parms) ;
	
		
		/// <summary>
		/// ִ�в�ѯ���,����Reader
		/// </summary>
		/// <param name="strSql">ִ��sql���</param>
		/// <returns>0 �ɹ� -1 ʧ��</returns>
		int ExecQuery(string strSql) ;
			
		/// <summary>
		/// ִ�в�ѯ��䣬����Reader
		/// </summary>
		/// <param name="strSql">ԭʼsql���</param>
		/// <param name="parms">��Ҫ�滻�Ĳ�������</param>
		/// <returns>����ִ��״̬ ��1ʧ�� 0 �ɹ� </returns>
		int ExecQuery(string strSql,params string[] parms) ;
			
		
		/// <summary>
		/// ִ��sql��� ����
		/// </summary>
		/// <param name="strSql">�����sql���</param>
		/// <param name="strDataSet">����DataSet xml</param>
		/// <returns>����ִ��״̬ -1ʧ�� 0 �ɹ�</returns>
		int ExecQuery(string strSql,ref string strDataSet) ;

        /// <summary>
        /// Ĭ����Reader,��ʱ����Ҫ��TempReader
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>����ִ��״̬ -1ʧ�� 0 �ɹ�</returns>
		int ExecQueryByTempReader(string strSql) ;
			
		/// <summary>
		/// ִ��sql
		/// </summary>
		/// <param name="strSql">�����sql���</param>
		/// <param name="strDataSet"></param>
		/// <returns>����ִ��״̬ -1 ʧ�� 0 �ɹ�</returns>
		int ExecQuery(string strSql,ref string strDataSet,string strXSLFileName) ;
			
		
		/// <summary>
		/// ִ��sql������DataSet
		/// </summary>
		/// <param name="strSql">����sql���</param>
		/// <param name="DataSet">������dataset</param>
		/// <returns>����ִ��״̬ -1 ʧ�� 0 �ɹ�</returns>
		int ExecQuery(string strSql,ref DataSet DataSet) ;
		

		/// <summary>
		/// ִ��sql������DataSet
		/// writed by cuipeng 
		/// 2005-08
		/// </summary>
		/// <param name="indexes">SQL�����xml�е�����λ��</param>
		/// <param name="dataSet">���ص�DataSet</param>
		/// <param name="parms">��������,���û�в�������null</param>
		/// <returns>0�ɹ���-1����</returns>
		int ExecQuery(string[] indexes, ref DataSet dataSet, params string[] parms) ;

		

		/// <summary>
		/// ִ��sql������DataSet
		/// </summary>
		/// <param name="index">SQL�����xml�е�����λ��</param>
		/// <param name="dataSet">���ص�dataSet</param>
		/// <param name="parms">��������,���û�в�������null</param>
		/// <returns>0�ɹ���-1����</returns>
		int ExecQuery(string index, ref DataSet dataSet, params string[] parms) ;
		
		

		/// <summary>
		/// ִ��sql��䣬����һ����¼
		/// </summary>
		/// <param name="strSql">ִ��sql���</param>
		/// <returns> 0 �ɹ� -1����</returns>
		 string ExecSqlReturnOne(string strSql) ;
		
		
		
		/// <summary>
		/// ִ��sql��䣬����һ����¼ ,���û�м�¼������Ĭ���ַ���
		/// </summary>
		/// <param name="strSql">ִ��sql���</param>
		/// <param name="defaultstring">������ַ���</param>
		/// <returns></returns>
		string ExecSqlReturnOne(string strSql,string defaultstring) ;
			
		

		/// <summary>
		/// �������ݿ��Blob��������,��ָ��sql����Ϊlength=1�Ĳ���
		/// </summary>
		/// <param name="strSql">�����sql���</param>
		/// <param name="ImageData">������ֽڱ���</param>
		/// <returns> 0 �ɹ� -1 ʧ��</returns>
		int InputBlob(string strSql,byte[] ImageData) ;
		
			
		
		/// <summary>
		/// ���blob
		/// </summary>
		/// <param name="strSql">�����sql���</param>
		/// <returns>���ص��ֽ�</returns>
		byte[] OutputBlob(string strSql) ;
			
			
		
		/// <summary>
		/// ���볤�ַ���
		/// ���>4000���ȵ��ַ���
		/// </summary>
		/// <param name="strSql">�����sql�ַ���</param>
		/// <param name="data">���ַ���</param>
		/// <returns></returns>
		 int InputLong(string strSql,string data) ;
			
		
		
		/// <summary>
		/// ִ�д洢����
		/// <example>PRC_HIEBILL_CHARGE_ext,arg_checkopercode,22,1,;0},
		///		arg_exec_Sqn,22,1,{1},arg_yearcode,22,1,{2},return_code,30,2,{3},return_result,22,2,{4}</example>
		/// </summary>
		/// <param name="strSql">�洢����-����,���ͣ��������,��ֵ<br>22 varchar 30 double 33 int 6 DATETIME </br></param>
		/// <param name="Return">�洢���̷���ֵ ���ŷָ�</param>
		/// <returns>0 �ɹ� -1 ʧ��</returns>
		 int ExecEvent(string strSql,ref string Return) ;
			

		#region ���ʱ��
		/// <summary>
		/// ���ϵͳʱ��/����
		/// </summary>
		/// <returns>DateTime from Oracle</returns>
		 string GetSysDateTime() ;
		
		
		 string GetSysDateTime(string format) ;
			
		
		 DateTime GetDateTimeFromSysDateTime() ;
			
		
		/// <summary>
		/// ���ϵͳ���� -
		/// </summary>
		/// <returns>Date yyyy-mm-dd</returns>
		 string GetSysDate() ;
			
		
		/// <summary>
		/// ���ϵͳ���� yyyy?mm?dd
		/// </summary>
		/// <returns>Date</returns>
		 string GetSysDate(string format ) ;
			
		
		/// <summary>
		/// ���ϵͳ����yyymmdd
		/// </summary>
		/// <returns>Date yyyymmdd</returns>
		 string GetSysDateNoBar() ;
		
		
		#endregion

        #endregion

     }
}
