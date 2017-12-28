using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// ListButton 
	/// ���Ű�ť�Ŀؼ�
	/// �����ļ���
	///		<Setting>
	///			<Panel><Control Special="1" Text=""></Control></Panel>
	///			<Panel><Control></Control></Panel>
	///		</Setting>
	/// </summary>
	public class ListButton : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ListButton()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��

		}

		/// <summary> 
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(104, 160);
			this.panel1.TabIndex = 0;
			// 
			// ListButton
			// 
			this.Controls.Add(this.panel1);
			this.Name = "ListButton";
			this.Size = new System.Drawing.Size(104, 160);
			this.Load += new System.EventHandler(this.ListButton_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ListButton_Load(object sender, System.EventArgs e)
		{
		
		}
		private string strFileName="";
		private Color cPanelBackColor;
		private Color cBackColor;
		private Image imgBackImage;
		private Color cForeColor=Color.Black;
		private Color cSelectedBackColor=Color.BurlyWood;
		private Color cSelectedForeColor=Color.Blue;
		private System.Windows.Forms.Panel panel1;
		private Color cSpecialForeColor=Color.Red;
		#region ����
		/// <summary>
		/// �����ļ���
		/// </summary>
		public string FileName
		{
			get
			{
				return this.strFileName;
			}
			set
			{
				this.strFileName=value;
				if(this.strFileName!="")this.LoadXml();
			}
		}
		public Color PanelBackColor
		{
			get
			{
				return this.cPanelBackColor;
			}set
			{
				 this.cPanelBackColor=value;
			}
		}
		public new Color BackColor
		{
			get
			{
				return this.cBackColor;
			}set
			{
				 this.cBackColor=value;
			}
		}
		public  Image BackImage
		{
			get
			{
				return this.imgBackImage;
			}
			set
			{
				this.imgBackImage=value;
			}
		}
		#endregion
		/// <summary>
		/// ����xml�����ļ�
		/// </summary>
		/// <returns></returns>
		private int LoadXml()
		{
			Neusoft.FrameWork.Xml.XML xml=new Neusoft.FrameWork.Xml.XML();
			
			System.Xml.XmlDocument doc=null;
			try
			{
				doc=xml.LoadXml(this.strFileName);
			}
			catch
			{
				return -1;
			}
			if(doc==null) return -1;
			System.Xml.XmlNodeList nodes=doc.SelectNodes("/Setting/Panel");
			System.Xml.XmlNodeList ControlNodes;
			foreach(System.Xml.XmlNode node in nodes)
			{
				this.AddSplitButton();
				ControlNodes=node.SelectNodes("Control");
				foreach(System.Xml.XmlNode ControlNode in ControlNodes)
				{
					try
					{
						this.AddButton(ControlNode.Attributes["Text"].Value,int.Parse(ControlNode.Attributes["Special"].Value),ControlNode.Attributes["Tag"].Value);
					}
					catch{}
				}
				this.AddSplitButton();
				}
				this.btnLastButton.Dock=DockStyle.Bottom;
			return 0;
		}
		private int intButtonHeight=40;
		private int intLastButtonTop=0;
		private int intSplitButtonHeight=20;
		private Button btnLastButton;
		private void AddButton(string Text,int Special,string Tag)
		{
			Button b=new Button();
			b.Location=new Point(0,this.intLastButtonTop );
			
			b.Anchor= System.Windows.Forms.AnchorStyles.Top | AnchorStyles.Left |AnchorStyles.Right;
			b.BackColor=this.cBackColor;
			b.Tag=Tag;
			b.Size=new Size(this.panel1.Width,this.intButtonHeight);
			if(Special==0)
			{
				b.ForeColor=this.cForeColor;
			}
			else if(Special==1)
			{
				b.ForeColor=this.cSpecialForeColor;
			}
			else if(Special==3)
			{
				b.Size=new Size(this.panel1.Width,this.intSplitButtonHeight);
				
			}
			try
			{
				b.Image=this.imgBackImage;
			}
			catch{}

			if(Special==3)			intLastButtonTop=intLastButtonTop+this.intSplitButtonHeight ;
			else intLastButtonTop=intLastButtonTop+this.intButtonHeight ;

			b.Text=Text;
			b.Visible=true;
			this.panel1.Controls.Add(b);
			
			b.Show();
			b.BringToFront();

			
			this.btnLastButton=b;
		}
		private void AddSplitButton()
		{
			this.AddButton("--",3,"");
		}


	}
}
