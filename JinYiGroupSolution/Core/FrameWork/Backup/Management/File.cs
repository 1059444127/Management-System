using System;

namespace Neusoft.FrameWork.Management
{
	/// <summary>
	/// File �ļ�����
	/// ����dll
	/// </summary>
	public class File
	{
		/// <summary>
		/// ftp ���� 
		/// </summary>
		private FrameWork.Models.NeuObject Ftp=new FrameWork.Models.NeuObject();
		/// <summary>
		/// ���캯��
		/// </summary>
		public File()
		{
		}
		/// <summary>
		/// ���캯��������ftp
		/// </summary>
		/// <param name="Ftp"></param>
		public File(FrameWork.Models.NeuObject Ftp)
		{
			this.Ftp=Ftp;
		}
		/// <summary>
		/// ��ǰftp����
		/// </summary>
		public FrameWork.Models.NeuObject objFtp
		{
			get
			{
				return this.Ftp;
			}
			set
			{
				this.Ftp=value;
			}
		}
		/// <summary>
		/// �����ļ���������
		/// </summary>
		/// <param name="sourceFileName">�����ļ�</param>
		/// <param name="targetFile">Ŀ���ļ�</param>
		/// <returns></returns>
		public int SaveFile(string sourceFileName,FrameWork.Models.NeuFileInfo targetFile)
		{
			return 0;
		}
		/// <summary>
		/// ��÷������ļ�
		/// </summary>
		/// <param name="sourceFile">�������ļ�</param>
		/// <param name="targetFileName">�����ļ�</param>
		/// <returns></returns>
		public int GetFile(FrameWork.Models.NeuFileInfo sourceFile,string targetFileName)
		{
			return 0;
		}
	}
}
