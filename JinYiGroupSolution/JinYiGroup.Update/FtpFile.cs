using System;

namespace AutoUpdate
{
	/// <summary>
	/// FtpFile ��ժҪ˵����
	/// </summary>
	public class FtpFile
	{
		public FtpFile()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		public string PrimaryID;// ������         
		public string FileName;	//�ļ���         
		public string LocalDirectory; //�ͻ������Ŀ¼ 
		public byte []FileContent;	//�ļ�����       
		public string FileVersion;	// �汾��         
		public string OperCode;		//����Ա         
		public System.DateTime OperDate;//��������    
	}
	public enum EditType
	{
		None,
		Modify,
		Add,
		Delete
	}
}
