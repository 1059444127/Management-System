using System;
using System.Windows.Forms.Design;

namespace Neusoft.FrameWork.WinForms.Controls 
{	
	/// <summary>
	/// DateTimeCellType ��ժҪ˵����
	/// ��ֹ˫���ؼ�ʱ��������ѡ��ؼ�
	/// </summary>
	public class DateTimeCellType:FarPoint.Win.Spread.CellType.DateTimeCellType 
	{
		
		/// <summary>
		/// ucEditor ��ժҪ˵���� ����˫����������ѡ��ؼ�
		/// </summary>
		private class ucEditor:FarPoint.Win.Spread.CellType.ISubEditor 
		{
			public ucEditor() 
			{
				//
				// TODO: �ڴ˴���ӹ��캯���߼�
				//
			}
			#region ISubEditor ��Ա

			public object GetValue() 
			{
				// TODO:  ��� ucEditor.GetValue ʵ��
				return null;
			}
			
			/// <summary>
			/// �ر��¼�
			/// </summary>
			public event System.EventHandler CloseUp;

			/// <summary>
			/// ������ֵ
			/// </summary>
			/// <param name="value"></param>
			public void SetValue(object value) 
			{
				// TODO:  ��� ucEditor.SetValue ʵ��
			}

			/// <summary>
			/// ���λ��
			/// </summary>
			/// <param name="rect"></param>
			/// <returns></returns>
			public System.Drawing.Point GetLocation(System.Drawing.Rectangle rect) 
			{
				// TODO:  ��� ucEditor.GetLocation ʵ��
				return new System.Drawing.Point ();
			}
		
			/// <summary>
			/// ����ӿؼ�
			/// </summary>
			/// <returns></returns>
			public System.Windows.Forms.Control GetSubEditorControl() 
			{
				// TODO:  ��� ucEditor.GetSubEditorControl ʵ��
				return null;
			}
			
			/// <summary>
			/// ��ֵ�仯
			/// </summary>
			public event System.EventHandler ValueChanged;

			/// <summary>
			/// ���
			/// </summary>
			/// <returns></returns>
			public System.Drawing.Size GetPreferredSize() 
			{
				// TODO:  ��� ucEditor.GetPreferredSize ʵ��
				return new System.Drawing.Size ();
			}

			#endregion
		}

		public DateTimeCellType() 
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.SubEditor = new ucEditor();
		}
		/// <summary>
		/// �ӿؼ�
		/// </summary>
		public override FarPoint.Win.Spread.CellType.ISubEditor SubEditor 
		{
			get 
			{
				return null;
			}
			set 
			{
				base.SubEditor = value;
			}
		}

		/// <summary>
		/// ��ʾ�ӿؼ�
		/// </summary>
		public override void ShowSubEditor() 
		{
			base.ShowSubEditor ();
		}


	}
}
