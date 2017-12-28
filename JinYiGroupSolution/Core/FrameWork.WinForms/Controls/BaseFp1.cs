using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace neusoft.neuFC.Interface.Controls
{
	/// <summary>
	/// ��������
	/// </summary>
	public class ColumnProperty
	{	
		public string ID="";
		public string Name="";
		public int Width=-1;
		public int Height=-1;
		public Color BackColor=Color.White;
		public Color ForeColor=Color.Black;
		public Color SelectColor=Color.Blue;
		public bool Visible=true;
		public bool Locked=false;
		public Font Font=new Font("Veranda", 8, FontStyle.Regular);
	}
	/// <summary>
	/// FpTreeView ��ժҪ˵����
	/// ��ʾ��TreeView��FP���
	/// </summary>
	public class BaseFp :System.Windows.Forms.UserControl,neusoft.neuFC.Interface.FpInterface
	{
		
		public FarPoint.Win.Spread.FpSpread fpSpread1;
		public FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;

		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		public BaseFp(System.ComponentModel.IContainer container)
		{
			///
			/// Windows.Forms ��׫д�����֧���������
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		public BaseFp()
		{
			///
			/// Windows.Forms ��׫д�����֧���������
			///
			InitializeComponent();

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


		#region �����������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
			this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
			// 
			// fpSpread1
			// 
			this.fpSpread1.Location = new System.Drawing.Point(17, 17);
			this.fpSpread1.Name = "fpSpread1";
			this.fpSpread1.Sheets.Add(this.fpSpread1_Sheet1);
			this.fpSpread1.TabIndex = 0;
			// 
			// fpSpread1_Sheet1
			// 
			this.fpSpread1_Sheet1.SheetName = "Sheet1";
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();

		}
		#endregion
		#region ����
		System.Data.DataSet myDataSet;
		Color myBackColor=Color.White;
		Color myForeColor=Color.Black;
		Color myHeaderForeColor=Color.Black;
		Color myHeaderBackColor=Color.Silver;
		Color myColumnForeColor=Color.Black;
		Color myColumnBackColor=Color.Silver;
		Color myChildHeader1ForeColor=Color.Black;
		Color myChildHeader1BackColor=Color.Silver;
		Color myChildColumn1ForeColor=Color.Black;
		Color myChildColumn1BackColor=Color.Silver;
		Color myChildHeader2ForeColor=Color.Black;
		Color myChildHeader2BackColor=Color.Silver;
		Color myChildColumn2ForeColor=Color.Black;
		Color myChildColumn2BackColor=Color.Silver;
		bool  myHeaderVisibled=true;
		bool  myColumnVisibled=true;
		bool  myChildHeaderVisibled=true;
		bool  myChildColumnVisibled=true;
		bool  myIDColumnVisibled=false;
		int   myDefaultColumnWidth=50;
		int   myDefaultRowHeight=20;
		bool  myDataAutoSizeColumns=false;
		ArrayList alColumnsProperty=new ArrayList();
		#endregion
		#region FpInterface ��Ա
		public void init()
		{
			// TODO:  ��� FpTreeView.init ʵ��
		}

		public void Refresh()
		{
		}
		public System.Data.DataSet fpDateSet
		{
			get
			{
				// TODO:  ��� FpTreeView.fpDateSet getter ʵ��
				return myDataSet;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpDateSet setter ʵ��
				myDataSet=value;
			}
		}
		public System.Drawing.Color fpBackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpBackColor getter ʵ��
				return this.myBackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpBackColor setter ʵ��
				this.myBackColor=value;
			}
		}

		public System.Drawing.Color fpForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpForeColor getter ʵ��
				return this.myForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpForeColor setter ʵ��
				this.myForeColor=value;
			}
		}

		public System.Drawing.Color fpHeaderBackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpHeaderBackColor getter ʵ��
				return this.myHeaderBackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpHeaderBackColor setter ʵ��
				this.myHeaderBackColor=value;
			}
		}

		public System.Drawing.Color fpHeaderForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpHeaderForeColor getter ʵ��
				return this.myHeaderForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpHeaderForeColor setter ʵ��
				this.myHeaderForeColor=value;
			}
		}

		public System.Drawing.Color fpColumnBackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpColumnBackColor getter ʵ��
				return this.myColumnBackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpColumnBackColor setter ʵ��
				this.myColumnBackColor=value;
			}
		}

		public System.Drawing.Color fpColumnForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpColumnForeColor getter ʵ��
				return this.myColumnForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpColumnForeColor setter ʵ��
				this.myColumnForeColor=value;
			}
		}

		public System.Drawing.Color fpChildHeader1BackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildHeader1BackColor getter ʵ��
				return this.myChildHeader1BackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildHeader1BackColor setter ʵ��
				this.myChildHeader1BackColor=value;
			}
		}

		public System.Drawing.Color fpChildHeader1ForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildHeader1ForeColor getter ʵ��
				return this.myChildColumn1ForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildHeader1ForeColor setter ʵ��
				this.myChildHeader1ForeColor=value;
			}
		}

		public System.Drawing.Color fpChildColumn1BackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildColumn1BackColor getter ʵ��
				return this.myChildColumn1BackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildColumn1BackColor setter ʵ��
				this.myChildColumn1BackColor=value;
			}
		}

		public System.Drawing.Color fpChildColumn1ForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildColumn1ForeColor getter ʵ��
				return this.myChildColumn1ForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildColumn1ForeColor setter ʵ��
				this.myChildColumn1ForeColor=value;
			}
		}

		public System.Drawing.Color fpChildHeader2BackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildHeader2BackColor getter ʵ��
				return this.myChildHeader2BackColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildHeader2BackColor setter ʵ��
				this.myChildHeader2BackColor=value;
			}
		}

		public System.Drawing.Color fpChildHeader2ForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildHeader2ForeColor getter ʵ��
				return this.myChildHeader2ForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildHeader2ForeColor setter ʵ��
				this.myChildHeader2ForeColor=value;
			}
		}

		public System.Drawing.Color fpChildColumn2BackColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildColumn2BackColor getter ʵ��
				return this.myChildColumn2BackColor ;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildColumn2BackColor setter ʵ��
				this.myChildColumn2BackColor=value;
			}
		}

		public System.Drawing.Color fpChildColumn2ForeColor
		{
			get
			{
				// TODO:  ��� FpTreeView.fpChildColumn2ForeColor getter ʵ��
				return this.myChildColumn2ForeColor;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildColumn2ForeColor setter ʵ��
				this.myChildColumn2ForeColor=value;
			}
		}

		public bool fpChildHeaderVisible
		{
			get
			{
				return this.myChildHeaderVisibled; 
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildHeaderVisible setter ʵ��
				this.myChildHeaderVisibled=value;
			}
		}

		public bool fpChildColumnVisible
		{
			get
			{
				return this.myChildColumnVisibled;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpChildColumnVisible setter ʵ��
				this.myChildColumnVisibled=value;
			}
		}

		public bool fpHeaderVisible
		{
			get
			{
				return this.myHeaderVisibled;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpHeaderVisible setter ʵ��
				this.myHeaderVisibled=value;
			}
		}

		public bool fpColumnVisible
		{
			get
			{
				return this.myColumnVisibled;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpColumnVisible setter ʵ��
				this.myColumnVisibled=value;
			}
		}

		public bool fpIDColumnVisible
		{
			get
			{
				return this.myIDColumnVisibled;
			}
			set
			{
				// TODO:  ��� FpTreeView.fpIDColumnVisible setter ʵ��
				this.myIDColumnVisibled=value;
			}
		}
		public int DefaultColumnWidth
		{
			get
			{
				return this.myDefaultColumnWidth;
			}
			set
			{
				this.myDefaultColumnWidth=value;
			}
		}
		public int DefaultRowHeight
		{
			get
			{
				return this.myDefaultRowHeight;
			}
			set
			{
				this.myDefaultRowHeight=value;
			}
		}
		public bool DataAutoSizeColumns
		{
			get
			{
				return this.myDataAutoSizeColumns;
			}
			set
			{
				this.myDataAutoSizeColumns=value;
			}
		}
		public ArrayList ColumnsProperty
		{
			get
			{
				return this.alColumnsProperty;
			}
			set
			 {
				 this.alColumnsProperty=value;
			 }
		}
		#endregion

	}
}
