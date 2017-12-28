using System;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// NeuCheckedListBox<br></br>
	/// [��������: NeuCheckedListBox�ؼ�]<br></br>
	/// [�� �� ��: ����ȫ]<br></br>
	/// [����ʱ��: 2006-09-07]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	[ToolboxBitmap(typeof(CheckedListBox))]
	public class NeuCheckedListBox : CheckedListBox, INeuControl
	{
		public NeuCheckedListBox()
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
		private void t()
		{
			
			T1();
		}

		#endregion

		private void T1( ) {
			int i = 0;
			i++;
		}
	}
}
