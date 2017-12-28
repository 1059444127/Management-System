namespace Neusoft.HISFC.Models.PhysicalExamination.Useless.Register 
{
	/// <summary>
	/// BasicExamination <br></br>
	/// [��������: �������״��]<br></br>
	/// [�� �� ��: ��һ��]<br></br>
	/// [����ʱ��: 2006-11-10]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��=''
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [System.Serializable]
    public class BasicExamination : Neusoft.HISFC.Models.PhysicalExamination.Base.PE
	{
		#region ˽�б���
		
		/// <summary>
		/// ��Ŀ�Ķ��Խ��
		/// </summary>
		private Neusoft.HISFC.Models.PhysicalExamination.Management.ItemQualitativeResult itemQualitativeResult;

		/// <summary>
		/// ֵ���ͽ��
		/// </summary>
		private decimal valueResult;

		#endregion

		#region ����

		/// <summary>
		/// ��Ŀ�Ķ��Խ��
		/// </summary>
		public Neusoft.HISFC.Models.PhysicalExamination.Management.ItemQualitativeResult ItemQualitativeResult 
		{
			get 
			{
				return this.itemQualitativeResult;
			}
			set 
			{
				this.itemQualitativeResult = value;
			}
		}

		/// <summary>
		/// ֵ���ͽ��
		/// </summary>
		public decimal ValueResult
		{
			get
			{
				return this.valueResult;
			}
			set
			{
				this.valueResult = value;
			}
		}

		#endregion

		#region ����
		
		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns>�������״��</returns>
		public new BasicExamination Clone()
		{
			BasicExamination basicExamination = base.Clone() as BasicExamination;

			basicExamination.ItemQualitativeResult = this.ItemQualitativeResult.Clone();

			return basicExamination;
		}

		#endregion
	}
}
