using System;

namespace Neusoft.HISFC.Models.EPR
{
	/// <summary>
	/// QC ��ժҪ˵����
	/// ��������ʵ��
	/// id �ļ����� name ��������
	/// </summary>
    [Serializable]
	public class QC:Neusoft.FrameWork.Models.NeuObject
	{
		/// <summary>
	    /// id ���� name ��������
		/// </summary>
		public QC()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		protected string strIndex;
		/// <summary>
		/// ָ��������Ϊһ�Ų�������ָ����������,����Ҫ�ļ�(����)
		/// </summary>
		public string Index
		{
			get
			{
				if(strIndex==null || strIndex=="") strIndex = "0";
				return strIndex;
			}
			set
			{
				strIndex = value;
			}
		}
		/// <summary>
		/// ������Ϣ
		/// </summary>
		protected Neusoft.HISFC.Models.RADT.PatientInfo myPatientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();

		/// <summary>
		/// ָ������
		/// </summary>
		protected Neusoft.HISFC.Models.EPR.QCData myQCData = new QCData();
		

		/// <summary>
		/// ������Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
		{
			get
			{
				return this.myPatientInfo;
			}
			set
			{
				this.myPatientInfo = value;
			}
		}

		/// <summary>
		/// ָ������
		/// </summary>
		public QCData QCData
		{
			get
			{
				return this.myQCData;
			}
			set
			{
				this.myQCData = value;
			}
		}

	

		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns></returns>
		public new QC Clone()
		{
			QC newObj = new QC();
			newObj = base.Clone() as QC;
			newObj.myPatientInfo = this.myPatientInfo.Clone();
			newObj.myQCData = this.myQCData.Clone();
			return newObj;
		}
	}
}
