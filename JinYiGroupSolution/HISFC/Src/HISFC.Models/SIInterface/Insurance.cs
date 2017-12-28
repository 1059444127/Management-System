using System;


namespace Neusoft.HISFC.Models.SIInterface {


	/// <summary>
	/// Insurance ��ժҪ˵����
	/// </summary>
    [Serializable]
    public class Insurance:Neusoft.FrameWork.Models.NeuObject
	{
		public Insurance()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		private Neusoft.FrameWork.Models.NeuObject pactInfo = new Neusoft.FrameWork.Models.NeuObject();
		private Neusoft.FrameWork.Models.NeuObject kind = new Neusoft.FrameWork.Models.NeuObject();
		private string partId;
		private Decimal rate;
		private Decimal beginCost;
		private Decimal endCost;
		private Neusoft.FrameWork.Models.NeuObject operCode =  new Neusoft.FrameWork.Models.NeuObject();
		private DateTime operDate;
		/// <summary>
		/// ��ͬ��λ��Ϣ
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject PactInfo
		{
			set{pactInfo = value;}
			get{return pactInfo;}
		}
		/// <summary>
		/// ��Ա���
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Kind
		{
			set{kind = value;}
			get{return kind;}
		}
		/// <summary>
		/// �ֶ����
		/// </summary>
		public string PartId
		{
			set{partId = value;}
			get{return partId;}
		}
		/// <summary>
		/// �����Ը�����
		/// </summary>
		public Decimal Rate
		{
			set{rate = value;}
			get{return rate;}
		}
		/// <summary>
		/// ���俪ʼ
		/// </summary>
		public Decimal BeginCost
		{
			set{beginCost = value;}
			get{return beginCost;}
		}
		/// <summary>
		/// �������
		/// </summary>
		public Decimal EndCost
		{
			set{endCost = value;}
			get{return endCost;}
		}
		/// <summary>
		/// ����Ա
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject OperCode
		{
			set{operCode = value;}
			get{return operCode;}
		}
		public DateTime OperDate
		{
			set{operDate = value;}
			get{return operDate;}
		}
	}
}
