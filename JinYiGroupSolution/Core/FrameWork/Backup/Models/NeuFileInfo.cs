using System;
using System.IO;
namespace Neusoft.FrameWork.Models
{

    /// <summary>
    /// NeuFileInfo<br></br>
    /// [��������: NeuFileInfo]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />    
    /// </summary>
    public class NeuFileInfo:NeuObject
	{
		public NeuFileInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		/// <summary>
		/// �ļ�·����������ͷ·��
		/// </summary>
		public string FilePath;
		/// <summary>
		/// �ļ�ȫ�� ��ͨ��http����
		/// </summary>
		public string FileFullPath;
		/// <summary>
		/// �ļ�����
		/// </summary>
		public System.IO.FileAttributes FileAttributes=new FileAttributes();

	}
}
