using System;
using System.Data.OracleClient;
namespace AutoUpdate
{
	/// <summary>
	/// DataBase ��ժҪ˵����
	/// </summary>
	public class DataBase
	{
		public DataBase()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		/// <summary>
		/// ���� ˽��
		/// </summary>
		protected System.Data.OracleClient.OracleConnection con;
		/// <summary>
		/// ���� ˽��
		/// </summary>
		protected System.Data.OracleClient.OracleCommand command;
	}
}
