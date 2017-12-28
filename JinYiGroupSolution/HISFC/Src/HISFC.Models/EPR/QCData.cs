using System;

namespace Neusoft.HISFC.Models.EPR
{
	/// <summary>
	/// QCData ��ժҪ˵����
	/// ������������ʵ��
	/// ���༰�ӿڣ�object
	/// </summary>
    [Serializable]
	public class QCData
	{
		/// <summary>
		/// 
		/// </summary>
		public QCData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		
		/// <summary>
		/// �����ļ�״̬
		/// </summary>
		protected int myState = 0;
		/// <summary>
		/// ������
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject myCreater = new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ǩ����
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject mySaver = new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// �����
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject mySealer = new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ɾ����
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject myDeleter = new Neusoft.FrameWork.Models.NeuObject();
		
		/// <summary>
		/// ��ǰ״̬
		/// </summary>
		public int State
		{
			get
			{
				return myState;
			}
			set
			{
				myState = value;
			}
		}
		/// <summary>
		/// ������ id code name 
		/// memo ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Creater
		{
			get
			{
				return this.myCreater;
			}
			set
			{
				this.myCreater = value;
			}
		}
		/// <summary>
		/// ǩ���� id code name 
		/// memo ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Saver
		{
			get
			{
				return this.mySaver;
			}
			set
			{
				this.mySaver = value;
			}
		}
		/// <summary>
		/// ����� id code name 
		/// memo ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Sealer
		{
			get
			{
				return this.mySealer;
			}
			set
			{
				this.mySealer = value;
			}
		}
		/// <summary>
		/// ɾ���� id code name 
		/// memo ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Deleter
		{
			get
			{
				return this.myDeleter;
			}
			set
			{
				this.myDeleter = value;
			}
		}

		
		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns></returns>
		public  QCData Clone()
		{
			QCData newObj = new QCData();
			newObj.myCreater = this.myCreater.Clone();
			newObj.mySaver = this.mySaver.Clone();
			newObj.mySealer = this.mySealer.Clone();
			newObj.myDeleter = this.myDeleter.Clone();
			return newObj;
		}
	}
}
