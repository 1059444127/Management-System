using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.Models.PhysicalExamination.Base
{
	/// <summary>
	/// Employee <br></br>
	/// <br>����������ʹ��Ч��������</br>
	/// <br>������Ч�Ժ���Ч��</br>
	/// <br>����ҽԺ��Ϣ</br>
	/// <br>����������ͣ�������죯���幫˾��죩</br>
	/// [��������: ��Աʵ��]<br></br>
	/// [�� �� ��: ��һ��]<br></br>
	/// [����ʱ��: 2006-09-12]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��=''
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [System.Serializable]
    public class PE : Neusoft.HISFC.Models.Base.Spell 
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		public PE( ) 
		{
		}

		#region ����

		/// <summary>
		/// �ֹ�����
		/// </summary>
		private string code = "";

		/// <summary>
		/// ��Ч��
		/// </summary>
		private Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumValidity validity = Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumValidity.Valid;

		/// <summary>
		/// ҽԺ
		/// </summary>
		private Hospital hospital = new Hospital();

		/// <summary>
		/// ������������
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment createEnvironment = new OperEnvironment();

		/// <summary>
		/// ʹ��Ч��������
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment invalidEnvironment = new OperEnvironment();

		/// <summary>
		/// ��𣺸�����졢��˾���������
		/// </summary>
		private Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumPEType peType = Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumPEType.Personal;

		#endregion

		#region ����
		
		/// <summary>
		/// �ֹ�����
		/// </summary>
		public string Code
		{
			get
			{
				return this.code;
			}
			set
			{
				this.code = value;
			}
		}

		/// <summary>
		/// ��Ч��
		/// </summary>
		public Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumValidity Validity 
		{
			get 
			{
				return this.validity;
			}
			set 
			{
				this.validity = value;
			}
		}

		/// <summary>
		/// ҽԺ
		/// </summary>
		public Hospital Hospital 
		{
			get 
			{
				return this.hospital;
			}
			set 
			{
				this.hospital = value;
			}
		}

		/// <summary>
		/// ������������
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment CreateEnvironment 
		{
			get 
			{
				return this.createEnvironment;
			}
			set 
			{
				this.createEnvironment = value;
			}
		}

		/// <summary>
		/// ʹ��Ч��������
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment InvalidEnvironment 
		{
			get 
			{
				return this.invalidEnvironment;
			}
			set 
			{
				this.invalidEnvironment = value;
			}
		}

		/// <summary>
		/// ��𣺸�����졢��˾���������
		/// </summary>
		public Neusoft.HISFC.Models.PhysicalExamination.Enum.EnumPEType PEType 
		{
			get 
			{
				return this.peType;
			}
			set 
			{
				this.peType = value;
			}
		}

		#endregion

		#region ����

		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns>������</returns>
		public new PE Clone()
		{
			PE pe = base.Clone() as PE;

			pe.Hospital = this.Hospital.Clone();
			pe.CreateEnvironment = this.CreateEnvironment.Clone();
			pe.InvalidEnvironment = this.InvalidEnvironment.Clone();

			return pe;
		}
		
		#endregion
	}
}
