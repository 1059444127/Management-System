using System;
using System.Collections;
namespace Neusoft.FrameWork.Public
{

    /// <summary>
    /// TableConvert <br></br>
	/// [��������: TableConvert���ݿ��ת����,�����ݱ�ת������ʷ���ݱ�����ʷ���ݱ�ת�������ݱ�]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    
    public class TableConvert
	{
		public TableConvert()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }

        #region ����
        
        //���߱����ʷ����ձ��arraylist�б�
        private static ArrayList alTables;
        
        #endregion

        #region ����
        #endregion

        #region ����

        /// <summary>
		/// ����û���滻������SQL���,�����Ƿ������ʷ���ݣ�������ڣ�����Ӧ�Ĳ�ѯͳ������SQL���
		/// </summary>
		/// <param name="sql">�����sql���</param>
		/// <returns>0 �ɹ� -1ʧ��</returns>
		public static int ConverTable(ref string sql)
		{
            //�����ѯ������ʷ���ݿ��SQL�����д���
            if (Neusoft.FrameWork.Management.Connection.IsHistory)
            {
                
                //ת�ɶ���ʷ���ݵ�SQL
                return ConvertTableToHistoryTable(ref sql);
            }
			
            return 0;
		}


        /// <summary>
        /// �����ߵı����ʷ��Ķ�����Ϣ���뵽��̬ArrayList����
        /// </summary>
        /// <returns>0 �ɹ� -1ʧ��</returns>
		private static int initTableList()
		{
			try
			{
                //����һ���µ�ArrayList
				alTables = new ArrayList();
				//����COM_TABLELIST
				Neusoft.FrameWork.Management.DataBaseManger manager = new Neusoft.FrameWork.Management.DataBaseManger();
				if(manager.ExecQuery("select tablename,historytablename,memo from com_tablelist where historytablename is not null")==-1) return -1;
				while(manager.Reader.Read())//������
				{
					Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
					obj.ID = manager.Reader[0].ToString();
					obj.Name = manager.Reader[1].ToString();
					obj.Memo = manager.Reader[2].ToString();
					alTables.Add(obj);
				}
				manager.Reader.Close();
			}
			catch//���ĵĴ���
			{

				return -1;
			}
			
            return 0;
		}
        
		/// <summary>
		/// ת��sql����ʷ���ݱ�sql
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>0 �ɹ� -1ʧ��</returns>
		public static int ConvertTableToHistoryTable(ref string sql)
		{
			if(alTables == null)//��һ�����У��������ݱ�
			{
				if(initTableList() == -1) return -1;
			}
			if(alTables == null) return 0;
			foreach(Neusoft.FrameWork.Models.NeuObject obj in alTables)//һ��һ���滻
			{
				if(obj.Name == null || obj.Name.Trim() =="")
				{
				}
				else//����ʷ��Ľ����滻
				{
					sql = sql.Replace(obj.ID.ToUpper(),obj.Name);
					sql = sql.Replace(obj.ID.ToLower(),obj.Name);
				}
			}
			return 0;
		}

		/// <summary>
		/// ת��sql��ʷ���ݱ�����ݱ�
		/// </summary>
		/// <param name="sql"></param>
		/// <returns>0 �ɹ� -1ʧ��</returns>
		public static int ConvertHistoryTableToTable(ref string sql)
		{
			if(alTables == null)//��һ�����У��������ݱ�
			{
				if(initTableList()==-1) return -1;
			}
			if(alTables == null) return 0;
			foreach(Neusoft.FrameWork.Models.NeuObject obj in alTables)//һ��һ���滻
			{
				if(obj.Name == null || obj.Name.Trim() =="")
				{
				}
				else//����ʷ��Ľ����滻
				{
					sql = sql.Replace(obj.Name.ToUpper(),obj.ID);
					sql = sql.Replace(obj.Name.ToLower(),obj.ID);
				}
			}
			return 0;
		}
		
		
        #endregion

    }
}
