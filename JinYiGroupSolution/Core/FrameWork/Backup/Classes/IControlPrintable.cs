using System;
using System.Collections;
using System.Drawing;
namespace Neusoft.FrameWork.WinForms.Classes
{
	/// <summary>
	/// IControlPrintable ��ժҪ˵����
	/// �û��ؼ���ӡ�ӿ�
	/// ��IContainer�ӿڸ���
	/// </summary>
	public interface IControlPrintable
	{
		/// <summary>
		/// �ؼ���Ҫ�Ĵ�С
		/// </summary>
		Size ControlSize{get;} 
		/// <summary>
		/// �Ƿ�����Զ���չ��С
		/// </summary>
		bool IsCanExtend{get;set;}
		/// <summary>
		/// ��ǰ�ؼ�����
		/// </summary>
		ArrayList Components{get;set;}
		/// <summary>
		/// ��������
		/// </summary>
		int HorizontalNum{get;set;}
		/// <summary>
		/// ��������
		/// </summary>
		int VerticalNum{get;set;}
		/// <summary>
		/// ��ֵ
		/// </summary>
		object ControlValue{set;get;}
		/// <summary>
		/// �Ƿ���ʾ����
		/// </summary>
		bool IsShowGrid{get;set;}
		/// <summary>
		/// ���ż����С
		/// </summary>
		int HorizontalBlankWidth{get;set;}
		/// <summary>
		/// ���ż����С
		/// </summary>
		int VerticalBlankHeight{get;set;}
		int BeginHorizontalBlankWidth{get;set;}
		int BeginVerticalBlankHeight{get;set;}
	}
}
