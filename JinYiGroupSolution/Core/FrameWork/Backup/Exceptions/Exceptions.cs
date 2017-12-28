using System;

namespace Neusoft.FrameWork.Exceptions
{
	/// <summary>
	/// Exceptions ��ժҪ˵����
	/// </summary>
	public class ReturnNullValueException : Exception
	{
		public ReturnNullValueException()  : base("��������ֵΪ�գ�")
		{
			
		}

		public ReturnNullValueException(string msg)  : base(msg)
		{
			 
		}
	}

	public class NullValueException : Exception
	{
		public NullValueException(string msg) : base(msg)
		{
		}
	}
}
