using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// �Զ���ؼ�������
	/// </summary>
	[DefaultProperty("Size")]
	internal class OwnControl
	{
		private Control control;
		private bool IsUseDefaultText;
		private ControlText controlText;

		public OwnControl(bool IsUseDefaultText)
		{
			this.IsUseDefaultText = IsUseDefaultText;
		}

		[Description("�ؼ�"),Browsable(false)]
		public Control Control
		{
			get
			{
				return this.control;
			}
			set
			{
				this.control = value;
			}
		}

		[Description("�ؼ��Ĵ�С��������Ϊ��λ����"),Category("Layout")]
		public Size Size
		{
			get
			{
				return this.control.Size;
			}
			set
			{
				this.control.Size = value;
			}
		}

		[Description("�ؼ����Ͻ��������������λ�á�"),Category("Layout")]
		public Point Location
		{
			get
			{
				return this.control.Location;
			}
			set
			{
				this.control.Location = value;
			}
		}

		[Description("������ʾ�ؼ��ı���ͼ�εı���ɫ��"),Category("Appearance")]
		public Color BackColor
		{
			get
			{
				return this.control.BackColor;
			}
			set
			{
				this.control.BackColor = value;
			}
		}

		[Description("������ʾ�ؼ����ı������塣"),Category("Appearance")]
		public Font Font
		{
			get
			{
				return this.control.Font;
			}
			set
			{
				this.control.Font = value;
			}
		}

		[Description("������ʾ�ؼ��ı���ͼ�ε�ǰ��ɫ��"),Category("Appearance")]
		public Color ForeColor
		{
			get
			{
				return this.control.ForeColor;
			}
			set
			{
				this.control.ForeColor = value;
			}
		}

		[Description("ָʾ�Ƿ�ʹ�øÿؼ���"),Category("Behavior")]
		public bool Enabled
		{
			get
			{
				return this.control.Enabled;
			}
			set
			{
				this.control.Enabled = value;
			}
		}

		[Description("��TAB��˳��ȷ���ÿؼ���ռ�õ�������"),Category("Behavior")]
		public int TabIndex
		{
			get
			{
				return this.control.TabIndex;
			}
			set
			{
				this.control.TabIndex = value;
			}
		}

		[Description("ȷ���ÿؼ��ǿɼ��������ء�"),Category("Behavior")]
		public bool Visible
		{
			get
			{
				return this.control.Visible;
			}
			set
			{
				this.control.Visible = value;
			}
		}

		[Category("Behavior")]
		public ControlText ControlText
		{
			get
			{
				if(this.controlText == null)
				{
					this.controlText = new ControlText(this.control.Text, this.IsUseDefaultText);
					controlText.Text = this.control.Text;
				}
//				controlText.IsUseDefaultText = this.IsUseDefaultText;
				return controlText;
			}
			set
			{
				this.controlText = value;
				this.control.Text = value.Text;
				this.IsUseDefaultText = value.IsUseDefaultText;
			}
		}
	}
}
