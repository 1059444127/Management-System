using System;

namespace Neusoft.HISFC.Models.EPR
{
	/// <summary>
	/// Record ��ժҪ˵����
	/// id code
	/// name EMRName ��������
	/// </summary>
    [Serializable]
	public class Record:Neusoft.HISFC.Models.Base.Record
	{
		public Record()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		
		protected string myEMRType = "";
		/// <summary>
		/// ��������
		/// </summary>
		public string EMRType 
		{
			get
			{
				return myEMRType;
			}
			set
			{
				myEMRType = value;
			}
		}

		protected Neusoft.FrameWork.Models.NeuObject myType = new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ��־��������-����ID�Ϳ���
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Type
		{
			get
			{
				return this.myType;
			}
			set
			{
				this.myType = value;
				if(this.myType.ID == enuRecordType.EMROperation.GetHashCode().ToString())
				{
					this.myType.Name = "��������";
				}
				if(this.myType.ID == enuRecordType.EMRModify.GetHashCode().ToString())
				{
					this.myType.Name = "�����޸�";
				}
				if(this.myType.ID == enuRecordType.System.GetHashCode().ToString())
				{
					this.myType.Name = "ϵͳ����";
				}
			}
		}
		protected string myNodeName ="";
		/// <summary>
		/// �ڵ�����
		/// </summary>
		public string NodeName
		{
			get
			{
				return this.myNodeName;
			}
			set
			{
				this.myNodeName =value;
			}
		}
	}
	/// <summary>
	/// ��־����
	/// </summary>
	public enum enuRecordType
	{
		/// <summary>
		/// ��������
		/// </summary>
		EMROperation =1,//��������
		/// <summary>
		/// �����޸�
		/// </summary>
		EMRModify = 2,//�����޸�
		/// <summary>
		/// ϵͳ����
		/// </summary>
		System = 3 //ϵͳ����
	
	}
	
}
