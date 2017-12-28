using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Neusoft.NFC.Interface.Controls
{
	
	/// <summary>
	/// BaseFp2 ��ժҪ˵����
	/// </summary>
	public class BaseFp : System.Windows.Forms.UserControl,Neusoft.NFC.Interface.FpInterface
	{
		public FarPoint.Win.Spread.FpSpread fpSpread1;
		public FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BaseFp()
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
			this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
			this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
			this.SuspendLayout();
			// 
			// fpSpread1
			// 
			this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fpSpread1.Location = new System.Drawing.Point(0, 0);
			this.fpSpread1.Name = "fpSpread1";
			this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																				   this.fpSpread1_Sheet1});
			this.fpSpread1.Size = new System.Drawing.Size(384, 400);
			this.fpSpread1.TabIndex = 0;
			this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.fpSpread1_CellDoubleClick);
			// 
			// fpSpread1_Sheet1
			// 
			this.fpSpread1_Sheet1.Reset();
			this.fpSpread1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
			this.fpSpread1_Sheet1.SheetName = "Sheet1";
			// 
			// BaseFp
			// 
			this.Controls.Add(this.fpSpread1);
			this.Name = "BaseFp";
			this.Size = new System.Drawing.Size(384, 400);
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
			this.ResumeLayout(false);

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

		#region ����
		public FarPoint.Win.Spread.FpSpread fpSpread
		{
			get
			{
				return this.fpSpread1;
			}
		}
		public FarPoint.Win.Spread.SheetView  fpCurrentSheetView
		{
			get
			{
				return this.fpSpread1.ActiveSheet;
			}
		}
		#endregion

		#region ����
		public void KeysEvent(System.Windows.Forms.Keys key) {
			switch (key) {
				case Keys.Down:
					//���¼�ͷ,��������
					//if (this.fpSpread1_Sheet1.ActiveRowIndex < 0) 
						this.fpSpread1_Sheet1.ActiveRowIndex++;
					break;
				case Keys.Up:
					//���ϼ�ͷ,��������
					//if (this.fpSpread1_Sheet1.ActiveRowIndex > 0) 
						this.fpSpread1_Sheet1.ActiveRowIndex--;
					break;
				case Keys.Enter:
					break;
			}
		}
		#endregion

		#region FpInterface ��Ա
		public void init()
		{
			// TODO:  ��� FpTreeView.init ʵ��
		}

		public void RefreshSpread()
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
				this.fpSpread1.BackColor=this.myBackColor;
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].DefaultStyle.BackColor=this.myBackColor;
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
				this.fpSpread1.ForeColor=this.myForeColor;
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].DefaultStyle.ForeColor=this.myForeColor;
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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].RowHeader.DefaultStyle.BackColor =this.myHeaderBackColor;

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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].RowHeader.DefaultStyle.ForeColor=this.myHeaderForeColor;

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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].ColumnHeader.DefaultStyle.BackColor=this.myColumnBackColor;

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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].ColumnHeader.DefaultStyle.ForeColor=this.myColumnForeColor;
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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].RowHeaderVisible=this.myHeaderVisibled;
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
				for(int i=0;i<this.fpSpread1.Sheets.Count;i++)
					this.fpSpread1.Sheets[i].ColumnHeader.Visible=this.myColumnVisibled;
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
		protected ArrayList alChild1ColumnsProperty;
		public ArrayList Child1ColumnsProperty
		{
			get
			{
				return this.alChild1ColumnsProperty;
			}
			set
			{
				this.alChild1ColumnsProperty=value;
			}
		}
		protected ArrayList alChild2ColumnsProperty;
		public ArrayList Child2ColumnsProperty
		{
			get
			{
				return this.alChild2ColumnsProperty;
			}
			set
			{
				this.alChild2ColumnsProperty=value;
			}
		}
		#endregion

		public void Export()
		{
			// TODO:  ��� ucMedicineCompare.Export ʵ��
			string Result ="";
			try
			{
				bool ret = false;
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.Filter = "Excel |.xls";
				saveFileDialog1.Title = "��������";
				try
				{
					saveFileDialog1.FileName =fpSpread1_Sheet1.Cells[fpSpread1_Sheet1.ActiveRowIndex,1].Text;
				}
				catch(Exception )
				{
					saveFileDialog1.FileName ="";
				}
				if(saveFileDialog1.ShowDialog()==DialogResult.OK)
				{
					if(saveFileDialog1.FileName != "")
					{
						//��Excel ����ʽ��������
						ret = fpSpread1.SaveExcel(saveFileDialog1.FileName,FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
					}
					if(ret)
					{
						MessageBox.Show("�ɹ���������");
					}
				}
				else
				{
					MessageBox.Show("������ȡ��");
				}
			}
			catch(Exception ee)
			{
				Result = ee.Message;
				MessageBox.Show(Result);
			}
		}

		private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e) {
		
		}

	}

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
}
