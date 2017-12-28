using System;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.Models.Fee.Item;

namespace Neusoft.HISFC.Models.Terminal
{
	/// <summary>
	/// MedTechItem <br></br>
	/// [��������: ҽ��ԤԼ��Ŀ��Ϣ]<br></br>
	/// [�� �� ��: sunxh]<br></br>
	/// [����ʱ��: 2005-3-3]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��=''
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [Serializable]
    public class MedTechItem : Neusoft.FrameWork.Models.NeuObject
	{
		public MedTechItem()
		{
			//
			// TODO: �ڴ˴����ӹ��캯���߼�
			//
		}

		#region ����
		
		/// <summary>
		/// ��Ŀ
		/// </summary>
		private Neusoft.HISFC.Models.Fee.Item.Undrug item = new Undrug();
		
		/// <summary>
		/// ��Ŀ��չ��Ϣ
		/// </summary>
		private ItemExtend itemExtend = new ItemExtend();
		
		#endregion

		#region ����

		/// <summary>
		/// ��Ŀ��Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.Fee.Item.Undrug Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}
		
		/// <summary>
		/// ��Ŀ��չ��Ϣ
		/// </summary>
		public ItemExtend ItemExtend
		{
			get
			{
				return this.itemExtend;
			}
			set
			{
				this.itemExtend = value;
			}
		}

		#endregion

		#region ����

		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns>ҽ��ԤԼ��Ŀ��Ϣ</returns>
		public new MedTechItem Clone()
		{
			MedTechItem medTechItem = base.Clone() as MedTechItem;
			
			medTechItem.Item = this.Item.Clone();
			medTechItem.itemExtend = this.itemExtend.Clone();
			
			return medTechItem;
		}

		#endregion
	}
}