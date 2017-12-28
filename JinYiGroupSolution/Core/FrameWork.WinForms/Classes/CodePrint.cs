using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
namespace Neusoft.FrameWork.WinForms.Classes
{
	/// <summary>
	/// �����
	/// created by wolf  2006-5
	/// 
	/// </summary>
	public class CodePrint
	{
		private CodePrint()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		[DllImport("bbdll.dll", SetLastError=false, CharSet=CharSet.None)]
		private static extern void Code128BarcodeToClipboard(string csMessage);
	
		[DllImport("bbdll.dll", SetLastError=false, CharSet=CharSet.None)]
		private static extern void Code39BarcodeToClipboard(string csMessage);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Image GetCode128(string str)
		{
			Code128BarcodeToClipboard(str);
			return (System.Windows.Forms.Clipboard.GetDataObject().GetData(DataFormats.Bitmap)) as Image;
					
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="picture"></param>
		public static void GetCode128(string str,System.Windows.Forms.PictureBox picture)
		{
			picture.Image = GetCode128(str);
		}
		/// <summary>
		/// ���Code39��ͼ��
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Image GetCode39(string str)
		{

			Code39BarcodeToClipboard(str);
			return (System.Windows.Forms.Clipboard.GetDataObject().GetData(DataFormats.Bitmap)) as Image;
					
		}
		/// <summary>
		/// ��Code39���뻭��ͼƬ����
		/// </summary>
		/// <param name="str"></param>
		/// <param name="picture"></param>
		public static void GetCode39(string str,System.Windows.Forms.PictureBox picture)
		{
			picture.Image = GetCode39(str);
		}
	}
}
