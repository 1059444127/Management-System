using System;
using System.IO;
using System.Reflection;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// ������
	/// </summary>
	internal class OwnAssembly
	{
		public static Type GetType(string TypeName)
		{
			if(TypeName == null) return null;

			string[] typeName = TypeName.Split(new Char[] {','});
			if(typeName.Length < 2) return null;

			string TypeFullName = typeName[0].Trim();
			if(TypeFullName == "") return null;
			

			string dllName = typeName[1].Trim();
			if(dllName == "") return null;

            Assembly ass = null;
            try
            {
                ass = GetAssembly(dllName);
            }
            catch
            {
                return null;
            }
			if(ass == null) return null;

			Type t = null;

			foreach(Type tt in ass.GetTypes())
			{
				if(TypeFullName == tt.ToString())
				{
					t = tt;
					break;
				}
			}

			return t;
		}

		/// <summary>
		/// ����������DLL��ȡ��Assembly��public static Assembly
		/// </summary>
		/// <param name="DllName">DLL��</param>
		/// <returns>�ɹ����� Assembly��ʧ�ܷ��� null</returns>
		public static Assembly GetAssembly(string DllName)
		{
			if(DllName == null) return null;

			Assembly ass = null;
			string dllName = DllName.Trim();
			dllName = KillEnd(dllName, ".dll");

			try
			{
				ass = Assembly.Load(dllName);

			}
			catch(FileNotFoundException)
			{
				throw (new Exception("�Ҳ�����̬���ӿ⣺" + DllName));
			}
			catch(ArgumentNullException)
			{
				throw (new Exception("��̬���ӿ�Ϊ��" + DllName));
			}

			if(ass == null) 
			{
				throw (new Exception("�Ҳ�����̬���ӿ⣺" + DllName));
			}
			return ass;
		}

		/// <summary>
		/// ������ȥ����β���Ӵ�(�����ִ�Сд)
		/// </summary>
		/// <param name="strName">�ַ���</param>
		/// <param name="strSub">�Ӵ�</param>
		/// <returns>ȥ����β���Ӵ����ַ���</returns>
		public static string KillEnd(string strName, string strSub)
		{
			if(strName.Length < strSub.Length) return strName;

			if(strName.Substring(strName.Length-strSub.Length,strSub.Length).ToLower() == strSub)
			{
				return (strName.Substring(0, strName.Length-strSub.Length));
			}
			else return strName;
		}

	}
}
