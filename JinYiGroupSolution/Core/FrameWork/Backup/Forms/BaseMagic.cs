using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
namespace Neusoft.FrameWork.WinForms.Forms
{
	/// <summary>
	/// Magic���ര�� created by wolf 2004-6-21
	/// ʵ��Magic���ڵĹ���
	/// �̳���BaseForm
	/// </summary>
	public class BaseMagic : BaseForm
	{
		private System.ComponentModel.IContainer components;

		
		public BaseMagic():base()
		{
			initDockingManager();
			InitializeComponent();

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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BaseMagic));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BaseMagic
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(568, 397);
			this.Name = "BaseMagic";
			this.Text = "BaseMagic";

		}
		#endregion

		/// <summary>
		/// ImageList
		/// </summary>
		public System.Windows.Forms.ImageList imageList1;

		/// <summary>
		/// ������
		/// </summary>
		protected Crownwood.Magic.Docking.DockingManager dockingManager;

		/// <summary>
		/// ��ʼ��DockingManager
		/// </summary>
		protected void initDockingManager()
		{
			dockingManager=new Crownwood.Magic.Docking.DockingManager(this,VisualStyle.IDE);
		}

		#region ����
		/// <summary>
		/// ���ÿؼ��ڲ����ⲿ
		/// </summary>
		/// <param name="Control"></param>
		/// <param name="type"> 'type=0 �ڲ�control 'type=1 �ⲿcontrol</param>
		[Obsolete("��SetInOrOut������",true)]
		public void _setInOrOut(Control Control,int type)
		{
			if(type==0)
				this.dockingManager.InnerControl=Control;
			else
				this.dockingManager.OuterControl=Control;
		}
		/// <summary>
		/// �����ڲ��ؼ�
		/// </summary>
		/// <param name="Control"></param>
		[Obsolete("��SetInOrOut������",true)]
		public void _setInOrOut(Control Control)
		{
			this._setInOrOut(Control,0);
		}
		/// <summary>
		///  '���Pad
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="Control"></param>
		/// <param name="size"></param>
		/// <param name="imageIndex"></param>
		[Obsolete("��AddPad������",true)]
		public void _addPad( Content pad,Control Control,System.Drawing.Size size,int imageIndex)
		{
			try
			{

				if(pad ==null)pad=new Content(this.dockingManager);
				if(Control.Text !="")
				{
					pad.Title = Control.Text;
					pad.FullTitle = Control.Text;
				}
				
				pad.Control=Control;
				try
				{
					pad.ImageList=this.imageList1;
					pad.ImageIndex=imageIndex;
				}
				catch
				{
				}
				pad.FloatingSize=size;
				this.dockingManager.Contents.Add(pad);
			}
			catch
			{
			}
		}
		[Obsolete("��AddPad������",true)]
		public void _addPad( Content pad,Control Control,System.Drawing.Size size)
		{
			this._addPad(pad,Control,size,0);
		}
		[Obsolete("��AddPad������",true)]
		public void _addPad( Content pad,Control Control)
		{
			this._addPad(pad,Control,new System.Drawing.Size(200,300),0);
		}
		/// <summary>
		/// ��ʾpad
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="State"></param>
		[Obsolete("��ShowPad������",true)]
		public void _showPad( Content pad,State State)
		{
			WindowContent wc = new WindowContent(this.dockingManager,VisualStyle.IDE);
			wc = this.dockingManager.AddContentWithState(pad,State);
			this.dockingManager.AddContentToWindowContent (pad,wc);
			this.dockingManager.ShowContent(pad);
		}
		/// <summary>
		/// ��ʾ�����б�
		/// </summary>
		[Obsolete("��ShowAllPad������",true)]
		public void _showAllPad()
		{
			this.dockingManager.ShowAllContents();
		}
		#endregion

		/// <summary>
		/// ���ÿؼ��ڲ����ⲿ
		/// </summary>
		/// <param name="Control"></param>
		/// <param name="type"> 'type=0 �ڲ�control 'type=1 �ⲿcontrol</param>
		protected void SetInOrOut(Control Control,int type)
		{
			if(type==0)
				this.dockingManager.InnerControl=Control;
			else
				this.dockingManager.OuterControl=Control;

		}
		/// <summary>
		/// �����ڲ��ؼ�
		/// </summary>
		/// <param name="Control"></param>
		protected void SetInOrOut(Control Control)
		{
			this.SetInOrOut(Control,0);
		}
		/// <summary>
		///  ���Pad,pad������Null���ڲ�����ʵ����
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="Control"></param>
		/// <param name="size"></param>
		/// <param name="imageIndex"></param>
		protected void AddPad( Content pad,Control Control,System.Drawing.Size size,int imageIndex)
		{
			try
			{

				if(pad == null) pad = new Content(this.dockingManager);
				if(Control.Text != "")
				{
					pad.Title = Control.Text;
					pad.FullTitle = Control.Text;
				}
				
				pad.Control = Control;
				if(this.imageList1 != null)
				{
					pad.ImageList = this.imageList1;
					pad.ImageIndex = imageIndex;
				}
			
				pad.FloatingSize = size;
				this.dockingManager.Contents.Add(pad);
			}
			catch
			{

			}
		}
		/// <summary>
		/// ���Pad,pad������Null���ڲ�����ʵ����
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="Control"></param>
		/// <param name="size"></param>
		protected void AddPad( Content pad,Control Control,System.Drawing.Size size)
		{
			this.AddPad(pad,Control,size,0);
		}
		/// <summary>
		/// ���Pad,pad������Null���ڲ�����ʵ����
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="Control"></param>
		protected void AddPad( Content pad,Control Control)
		{
			this.AddPad(pad,Control,new System.Drawing.Size(200,300),0);
		}
		/// <summary>
		/// ��ʾpad��λ��
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="State"></param>
		protected void ShowPad( Content pad,State State)
		{
			WindowContent wc = new WindowContent(this.dockingManager,VisualStyle.IDE);
			wc = this.dockingManager.AddContentWithState(pad,State);
			this.dockingManager.AddContentToWindowContent (pad,wc);
			this.dockingManager.ShowContent(pad);
		}
		/// <summary>
		/// ��ʾ�����б�
		/// </summary>
		protected void ShowAllPad()
		{
			this.dockingManager.ShowAllContents();
		}
	}
}
