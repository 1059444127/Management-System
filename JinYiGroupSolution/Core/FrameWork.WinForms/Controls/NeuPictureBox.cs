using System;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// NeuPictureBox<br></br>
	/// [��������: NeuPictureBox�ؼ�]<br></br>
	/// [�� �� ��: ����ȫ]<br></br>
	/// [����ʱ��: 2006-09-07]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	[ToolboxBitmap(typeof(PictureBox))]
	public class NeuPictureBox : PictureBox ,INeuControl
	{
		public NeuPictureBox()
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
