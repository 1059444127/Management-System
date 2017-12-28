using System;

namespace Neusoft.FrameWork.Models
{
    /// <summary>
    /// NeuInfo<br></br>
    /// [��������: xml�ӿ� ��Ϣ�ṹ ��ժҪ˵����]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public  class NeuInfo:Neusoft.FrameWork.Models.NeuObject 
	{
		public NeuInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        /// <summary>
        /// ��Ϣ����
        /// </summary>
		public enum infoType
		{
			/// <summary>
			/// ����
			/// </summary>
			Param,
			/// <summary>
			/// ȫ�ֱ���
			/// </summary>
			Global,
			/// <summary>
			/// ��ʱ����
			/// </summary>
			Temp,
			/// <summary>
			///�������� 
			/// </summary>
			Associate,
			/// <summary>
			/// ����
			/// </summary>
			Const,
			/// <summary>
			/// �����б�
			/// </summary>
			PatientList,
			/// <summary>
			/// סԺ�����б�
			/// </summary>
			inDeptList,
			/// <summary>
			/// ��������б�
			/// </summary>
			outDeptList,
			/// <summary>
			/// �б�
			/// </summary>
			List,
			/// <summary>
			/// �¼�
			/// </summary>
			Event
		}
		/// <summary>
		/// ��������
		/// </summary>
		public infoType type=new infoType();
		/// <summary>
		/// sql�������
		/// </summary>
		public string Sql;
		/// <summary>
		/// ����Sql
		/// </summary>
		public string UpdateSql;
		/// <summary>
		/// ��ֵ
		/// </summary>
		public string value;
		/// <summary>
		/// ��ʾ����
		/// </summary>
		public string showType;
	}
}
