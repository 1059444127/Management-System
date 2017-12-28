using System;

namespace Neusoft.HISFC.Models.Fee.Outpatient
{
	/// <summary>
	/// BalancePay<br></br>
	/// [��������: ����֧����ʽ��]<br></br>
	/// [�� �� ��: ����]<br></br>
	/// [����ʱ��: 2006-09-13]<br></br>
	/// <�޸ļ�¼ 
	///		�޸���='' 
	///		�޸�ʱ��='yyyy-mm-dd' 
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    /// 
    [System.Serializable]
	public class BalancePay : BalancePayBase
	{
		#region ����
		
		/// <summary>
		/// ������ˮ��
		/// </summary>
		private string squence;

		/// <summary>
		/// ��Ʊ��ţ�һ�ν���������ŷ�Ʊ��combNO
		/// </summary>
		private string invoiceCombNO;

        /// <summary>
        /// ��Ʊ���
        /// </summary>
        private string invoiceUnion;

  
		#endregion

		#region ����
		
		/// <summary>
		/// ������ˮ��
		/// </summary>
		public string Squence
		{
			get
			{
				return this.squence;
			}
			set
			{
				this.squence = value;
			}
		}

		/// <summary>
		/// ��Ʊ��ţ�һ�ν���������ŷ�Ʊ��combNO
		/// </summary>
		public string InvoiceCombNO
		{
			get
			{
				return this.invoiceCombNO;
			}
			set
			{
				this.invoiceCombNO = value;
			}
		}

        /// <summary>
        /// ��Ʊ���
        /// </summary>
        public string InvoiceUnion
        {
            get { return invoiceUnion; }
            set { invoiceUnion = value; }
        }
		#endregion

		#region ����
		
		#region ��¡
		
		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns>���ص�ǰ����ʵ��</returns>
		public new BalancePay Clone()
		{
			return base.Clone() as BalancePay;
		} 

		#endregion

		#endregion
	}
}
