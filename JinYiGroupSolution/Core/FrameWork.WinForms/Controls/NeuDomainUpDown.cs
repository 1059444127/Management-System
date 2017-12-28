using System;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// NeuDomainUpDown<br></br>
	/// [��������: NeuDomainUpDown�ؼ�]<br></br>
	/// [�� �� ��: ����ȫ]<br></br>
	/// [����ʱ��: 2006-09-07]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	[ToolboxBitmap(typeof(DomainUpDown))]
	public class NeuDomainUpDown : DomainUpDown, INeuControl
	{
		public NeuDomainUpDown()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		private StyleType styleType;

		#region INeuControl ��Ա

		public Neusoft.FrameWork.WinForms.Controls.StyleType Style
		{
			get
			{				
				return this.styleType;
			}
			set
			{
				this.styleType = value;
			}
		}

		#endregion
	}
}
