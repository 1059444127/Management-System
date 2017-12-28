using System.IO;
namespace Neusoft.FrameWork.Models
{  
    /// <summary>
    /// NeuLogo<br></br>
    /// [��������: NeuLogo]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
	public class NeuLog
	{
		public NeuLog()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			newLog();
		}
		public NeuLog(string strFileName)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.strFileName=strFileName;
			newLog();
		}
		private void newLog()
		{
			if(!Neusoft.FrameWork.Management.Connection.IsWeb)
			{
				if(! File.Exists(this.strFileName))
				{
					System.IO.File.CreateText(this.strFileName);
				}
			}
        }

        #region ����
        private string strFileName = ".\\log.txt";
        private System.IO.TextWriter output;
        #endregion

        #region ����
        /// <summary>
		/// ����/��ȡ�ļ���
		/// </summary>
		public string FileName
		{
			get
			{
				return strFileName;
			}
			set
			{
				strFileName=value;
			}
        }
        #endregion

        #region ����
        /// <summary>
		/// д��־
		/// </summary>
		/// <param name="str"></param>
		public void WriteLog(string str)
		{
			if(!Neusoft.FrameWork.Management.Connection.IsWeb)
			{
				try
				{
					output=File.AppendText(strFileName);
					output.WriteLine(System.DateTime.Now+"\n"+str);
					output.Close();
				}
				catch {}
			}
        }
        #endregion
    }
}
