using System;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// IFpInputable ��ժҪ˵����
	/// for ucFP������ؼ��ӿ�
	/// </summary>
	public interface IFpInputable
	{
		/// <summary>
		/// ��һ��
		/// </summary>
		void MoveNext();
		/// <summary>
		/// ��һ��
		/// </summary>
		void MovePrevious();
		/// <summary>
		/// ��ҳ
		/// </summary>
		void NextPage();
		/// <summary>
		/// ��ҳ
		/// </summary>
		void PreviousPage();

		/// <summary>
		/// ��õڼ���
		/// </summary>
		/// <param name="row"></param>
		object GetRow(int row);
		
		/// <summary>
		/// ����
		/// </summary>
		/// <param name="filter"></param>
		void Filter(string filter);
		
		/// <summary>
		/// �л����뷨
		/// </summary>
		void ChangeInput();
		
		/// <summary>
		/// ��õ�ǰ��Ŀ
		/// </summary>
		/// <returns></returns>
		object GetSelectedItem();

		/// <summary>
		/// ѡ����Ŀ�¼�
		/// </summary>
		event System.EventHandler SelectedItem;
	}
}
