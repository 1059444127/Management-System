using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

namespace neusoft.neuFC.Interface.Controls
{
	/// <summary>
	/// tvPatientList ��ժҪ˵����
	/// �����б�ؼ�
	/// </summary>
	public class tvDeptList :System.Windows.Forms.TreeView 
	{
		
		public tvDeptList()
		{
			///
			/// Windows.Forms ��׫д�����֧���������
			///
			InitializeComponent();
			init();//��ʼ��
		}

		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;
		#region �����������ɵĴ���


		public tvDeptList(System.ComponentModel.IContainer container)
		{
			///
			/// Windows.Forms ��׫д�����֧���������
			///
			container.Add(this);
			InitializeComponent();
			init();//��ʼ��
			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
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

		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(tvDeptList));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tvDeptList
			// 
			this.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvPatientList_AfterCheck);

		}
		#endregion
	
		
		/// <summary>
		/// ��ʾ������Ϣ - ���ͣ�����icu.ccu��״̬��
		/// </summary>
		public enum enuShowType
		{
			/// <summary>
			/// ����ʾ
			/// </summary>
			None = 0,
			/// <summary>
			/// ����
			/// </summary>
			DeptType = 1,
			/// <summary>
			/// ����,icu,ccu��
			/// </summary>
			DeptProperty = 3
		}
		/// <summary>
		/// ��ʾ��Ϣ����ǰ�棬����
		/// </summary>
		public enum enuShowDirection
		{
			/// <summary>
			/// ��ʾǰ��
			/// </summary>
			Ahead,
			/// <summary>
			/// ��ʾ����
			/// </summary>
			Behind
		}
		/// <summary>
		/// ѡ������
		/// </summary>
		public enum enuChecked
		{
			/// <summary>
			/// ��ѡ
			/// </summary>
			None,
			/// <summary>
			/// ��ѡ
			/// </summary>
			Radio,
			/// <summary>
			/// ��ѡ
			/// </summary>
			MultiSelect
		}
		private ArrayList myDepts=new ArrayList();
		private enuShowType myShowType=0;
		private enuChecked myChecked=enuChecked.None;
		private enuShowDirection myDirection=enuShowDirection.Behind;

		public const int RootImageIndex=0;
		public const int RootSelectedImageIndex=1;
		public const int BranchImageIndex=2;
		public const int BranchSelectedImageIndex=3;
		public const int DeptImageIndex=4;
		public const int DeptSelectedImageIndex=5;
		
		public enuShowType ShowType
		{
			get
			{
				return this.myShowType;
			}
			set
			{
				this.myShowType=value;
			}
		}
		/// <summary>
		/// �������飬�����ָ�object
		/// </summary>
		public ArrayList alDepartments
		{
			get
			{
				return this.myDepts;
			}
			set
			{
				this.myDepts=value;
				this.RefreshList();
			}
		}
		/// <summary>
		/// ��ʾѡ������
		/// </summary>
		public enuChecked Checked
		{
			get
			{
				return this.myChecked;
			}
			set
			{
				this.myChecked=value;
			}
		}
		/// <summary>
		/// ��ʾ������Ϣλ��
		/// </summary>
		public enuShowDirection Direction
		{
			get
			{
				return this.myDirection;
			}
			set
			{
				this.myDirection=value;
			}
		}
		protected  bool bIsShowCount=true;
		/// <summary>
		/// �Ƿ���ʾnodeCount
		/// </summary>
		public bool IsShowCount
		{
			get
			{
				return this.bIsShowCount;
			}
			set
			{
				this.bIsShowCount=value;
			}
		}
		#region ����
		private void RefreshList()
		{
			this.Nodes.Clear();
			int Branch=0;
			for(int i=0 ;i<this.myDepts.Count;i++)
			{
				System.Windows.Forms.TreeNode newNode=new System.Windows.Forms.TreeNode();
				neusoft.neuFC.Object.neuObject obj=new neusoft.neuFC.Object.neuObject();
				//����ΪҶ
				if(this.myDepts[i].GetType()==typeof(neusoft.HISFC.Object.Base.Department))//����
				{
					try
					{
						neusoft.HISFC.Object.Base.Department Info=(neusoft.HISFC.Object.Base.Department)this.myDepts[i];
						obj.ID = Info.ID;
						obj.Name = Info.Name;
						try
						{
							obj.Memo = Info.DeptType.Name;
						}
						catch
						{//�޿�����Ϣ

						}
						obj.User01 = Info.DeptPro;
						obj.User02 = Info.EnglishName;
						this.AddInfo(Branch,obj,Info);
					}
					catch{}
				}
				else if(this.myDepts[i].GetType().ToString()=="neusoft.neuFC.Object.neuObject")
				{
					obj = (neusoft.neuFC.Object.neuObject)this.myDepts[i];
					this.AddInfo(Branch,obj,obj);
				}
				else//Ϊ��
				{	
					//�ָ��ַ��� text|tag ��ʶ���
					string all=this.myDepts[i].ToString();
					string[] s=all.Split('|');
					newNode.Text=s[0];
					try
					{
						newNode.Tag=s[1];
					}
					catch{newNode.Tag="";}
					try
					{
						newNode.ImageIndex = BranchImageIndex;
						newNode.SelectedImageIndex= BranchSelectedImageIndex;
					}
					catch{}
					Branch=this.Nodes.Add(newNode);
				}
			}
			if(this.bIsShowCount)
			{
				foreach(System.Windows.Forms.TreeNode node in this.Nodes)
				{
				
					if(node.Tag.GetType().ToString()=="System.String" )//���
					{
						int count=0;
						count=node.GetNodeCount(false);
						node.Text=node.Text+"("+count.ToString()+")";
					}
				}
			}
			this.ExpandAll();
		}
		//obj.id ,name,memo=bed,user01=dept,user02=status user03=sex 
		private void AddInfo(int Branch,neusoft.neuFC.Object.neuObject neuObj,object obj)
		{
			string strText=neuObj.Name;
			string strMemo="";
			switch(this.myShowType.GetHashCode())
			{
				case 1://����
					strMemo="��"+neuObj.Memo+"��";
					break;
				case 3://����
					if(neuObj.User01!="" || neuObj.User01!=null) strMemo="��"+neuObj.User01+"��";
					break;
				case 4://����+����
					strMemo="��"+neuObj.User01+"��"+"��"+neuObj.Memo+"��";
					break;
				default:
					break;
			}
			try
			{
				if(strMemo.Trim()!="")
				{
					if(this.myDirection==enuShowDirection.Behind)
					{
						strText=strText+strMemo;
					}
					else
					{
						strText=strMemo+strText;
					}
				}
			}
			catch{}
			int ImageIndex = DeptImageIndex;
			this.AddTreeNode(Branch,strText,obj,ImageIndex);
		}
		private void AddTreeNode(int root,string Name,object obj,int ImageIndex)
		{
			System.Windows.Forms.TreeNode Node=new System.Windows.Forms.TreeNode();
			try
			{
				Node.Text=Name;
				Node.Tag=obj;
				try
				{
					Node.ImageIndex=ImageIndex;
					Node.SelectedImageIndex=ImageIndex+1;
				}
				catch{}
				try
				{
					this.SelectedNode=this.Nodes[root];
					
				}
				catch
				{
					this.Nodes.Add(new System.Windows.Forms.TreeNode("����"));
					this.SelectedNode=this.Nodes[0];
				}
				this.SelectedNode.Nodes.Add(Node);
			}
			catch{}
		}
		#endregion
		private void init()
		{
			this.ImageList=this.imageList1;
		}
		private bool bControlChecked=false;
		private void tvPatientList_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(this.CheckBoxes && this.bControlChecked==false)
			{
				foreach(System.Windows.Forms.TreeNode node in e.Node.Nodes)
				{
					node.Checked=e.Node.Checked;
				}
			}
		}
	}	
}
