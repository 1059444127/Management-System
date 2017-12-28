using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Neusoft.FrameWork.Models;
namespace Neusoft.FrameWork.WinForms.Forms {
	/// <summary>
	/// frmEasyChoose ��ժҪ˵����
	/// ���ٲ�ѯ����
	/// writed by cuipeng
	/// 2005-3
	/// </summary>
	public class frmEasyChoose :BaseForm{
		private Neusoft.FrameWork.WinForms.Controls.NeuTextBox txtQueryCode;
		private Neusoft.FrameWork.WinForms.Controls.NeuButton btnOK;
		private Neusoft.FrameWork.WinForms.Controls.NeuButton btnCancel;
		private System.ComponentModel.IContainer components;
		private DataSet myDataSet = new DataSet();

		protected FarPoint.Win.Spread.FpSpread fpData;
		protected FarPoint.Win.Spread.SheetView fpData_Sheet1;
		private Neusoft.FrameWork.WinForms.Controls.NeuCheckBox chbMisty;
		private DataView myDataView;
		private Neusoft.FrameWork.Models.NeuObject myObject = new Neusoft.FrameWork.Models.NeuObject();
		private Neusoft.FrameWork.Public.ObjectHelper objHelper = new Neusoft.FrameWork.Public.ObjectHelper();
		public event SelectedItemHandler SelectedItem;
		/// <summary>
		/// ���ڷ��ص�ʵ������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Object {
			get{ return myObject;}
			set{ 
				if(value == null) return;
				myObject = value;
			}
		}


		public frmEasyChoose(ArrayList arrayList) {
			//
			// Windows ���������֧���������
			//
			InitializeComponent();
            this.BackColor = Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.Blue);
            Neusoft.FrameWork.WinForms.Classes.Function.SetFarPointStyle(fpData);
            this.fpData_Sheet1.RowCount = 0;
			this.components = new System.ComponentModel.Container();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//

			//this.myObject = NeuObject;
			this.InitData(arrayList);
		}


		/// <summary>
		/// ������������ʹ�õ���Դ��
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		/// <summary>
		/// ͨ������Ĳ�ѯ�룬���������б�
		/// </summary>
		private void ChangeItem() {
			if (this.myDataSet.Tables[0].Rows.Count == 0) return;

			try {
				//ȡ��ѯ��
				string queryCode = "";
				if (this.chbMisty.Checked) {
					queryCode = "%" + Neusoft.FrameWork.Public.String.TakeOffSpecialChar(this.txtQueryCode.Text.Trim()) + "%";
				}
				else {
                    //
                    //[2007/02/05] ȡ��ģ��
                    //queryCode = this.txtQueryCode.Text.Trim() + "%";
                    //
                    queryCode = Neusoft.FrameWork.Public.String.TakeOffSpecialChar(this.txtQueryCode.Text.Trim());
				}
                //���ù�������
                string filter = string.Empty;
                if (queryCode.Trim() == "" || queryCode.Trim() == null)
                {
                    filter = "ƴ���� LIKE '%'";//��ʾȫ������
                }
                else
                {

                    filter = "(ƴ���� LIKE '" + queryCode + "') OR " +
                        "(����� LIKE '" + queryCode + "') OR " +
                        "(�Զ����� LIKE '" + queryCode + "') OR " +
                        "(���� LIKE '" + queryCode + "') OR " +
                        "(���� LIKE '" + queryCode + "')";
                }

				this.myDataView.RowFilter = filter;

				//��ʾ��һ������
				this.fpData_Sheet1.ActiveRowIndex= 0;

				//����farpoint����ʾ��ʽ
                //this.SetFormat();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message );
				return;
			}
		}


		/// <summary>
		/// ȡ��ǰ��ѡ�е�����
		/// </summary>
		private void GetItem() {
			//û�������򷵻�
			if(this.fpData_Sheet1.RowCount == 0) return;
			
			//ȡ��ǰ�е����ݡ�
			string ID = this.fpData_Sheet1.Cells[this.fpData_Sheet1.ActiveRowIndex,0].Text;

			//���ݱ���ȡ��Ӧ�Ķ���
			this.myObject = objHelper.GetObjectFromID(ID);
			if (this.myObject == null) {
				MessageBox.Show("û���ҵ���Ч������","�޷��ҵ�����");
				return;
			}
			
			try {
				//�׳��¼��������û�ѡ�еĶ����û���Ҫͨ��������¼�����������
				SelectedItem(this.myObject);
			}
			catch{}
			this.DialogResult = DialogResult.OK;
			//this.Close();
		}


		/// <summary>
		/// �������ݿؼ���ʽ
		/// </summary>
		private void SetFormat() {
			//��ʾ���ݸ�ʽ
			this.fpData_Sheet1.ColumnCount = 6;
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 0 ).Text = Neusoft.FrameWork.Management.Language.Msg( "����" );
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 1 ).Text = Neusoft.FrameWork.Management.Language.Msg( "����" );
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 2 ).Text = Neusoft.FrameWork.Management.Language.Msg( "����" );
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 3 ).Text = Neusoft.FrameWork.Management.Language.Msg( "ƴ����" );
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 4 ).Text = Neusoft.FrameWork.Management.Language.Msg( "�����" );
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 5 ).Text = Neusoft.FrameWork.Management.Language.Msg( "�Զ�����" );
			this.fpData_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.fpData_Sheet1.Columns.Get( 0 ).Label = Neusoft.FrameWork.Management.Language.Msg( "����" );
			this.fpData_Sheet1.Columns.Get(0).Locked = true;
			this.fpData_Sheet1.Columns.Get(0).Width = 54F;
            this.fpData_Sheet1.Columns.Get( 1 ).Label = Neusoft.FrameWork.Management.Language.Msg( "����" );
			this.fpData_Sheet1.Columns.Get(1).Locked = true;
			this.fpData_Sheet1.Columns.Get(1).Width = 139F;
            this.fpData_Sheet1.Columns.Get( 2 ).Label = Neusoft.FrameWork.Management.Language.Msg( "����" );
			this.fpData_Sheet1.Columns.Get(2).Locked = true;
			this.fpData_Sheet1.Columns.Get(2).Width = 112F;
            this.fpData_Sheet1.Columns.Get( 3 ).Label = Neusoft.FrameWork.Management.Language.Msg( "ƴ����" );
			this.fpData_Sheet1.Columns.Get(3).Locked = true;
			this.fpData_Sheet1.Columns.Get(3).Width = 81F;
            this.fpData_Sheet1.Columns.Get( 4 ).Label = Neusoft.FrameWork.Management.Language.Msg( "�����" );
			this.fpData_Sheet1.Columns.Get(4).Locked = true;
			this.fpData_Sheet1.Columns.Get(4).Width = 72F;
            this.fpData_Sheet1.Columns.Get( 5 ).Label = Neusoft.FrameWork.Management.Language.Msg( "�Զ�����" );
			this.fpData_Sheet1.Columns.Get(5).Locked = true;
			this.fpData_Sheet1.Columns.Get(5).Width = 67F;
			this.fpData_Sheet1.GrayAreaBackColor = System.Drawing.Color.Honeydew;
			this.fpData_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
			this.fpData_Sheet1.RowHeader.Columns.Default.Resizable = false;
			this.fpData_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
			this.fpData_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(225)), ((System.Byte)(243)));
		}

        /// <summary>
        /// Fp��ʽ����ʾ
        /// </summary>
        /// <param name="label"></param>
        /// <param name="visible"></param>
        /// <param name="width"></param>
        public virtual void SetFormat(string[] label, bool[] visible, int[] width)
        {
            for (int i = 0; i < this.fpData_Sheet1.Columns.Count; i++)
            {
                if (label != null && label.Length > i)
                {
                    this.fpData_Sheet1.Columns[i].Label = label[i];
                }

                if (visible != null && visible.Length > i)
                {
                    this.fpData_Sheet1.Columns[i].Visible = visible[i];
                }

                if (width != null && width.Length > i)
                {
                    this.fpData_Sheet1.Columns[i].Width = width[i];
                }
            }            
        }


        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="al"></param>
        private void InitData(ArrayList al)
        {
			if(al == null) return;
			objHelper.ArrayObject = al;
			this.myDataSet.Tables.Clear();
			this.myDataSet.Tables.Add();
			
			//��������
			System.Type dtStr   = System.Type.GetType("System.String");

			//��myDataTable�������
			this.myDataSet.Tables[0].Columns.AddRange( new DataColumn[] {
																			new DataColumn("����",     dtStr),
																			new DataColumn("����",     dtStr),
																			new DataColumn("����",     dtStr),
																			new DataColumn("ƴ����",   dtStr),
																			new DataColumn("�����",   dtStr),
																			new DataColumn("�Զ�����", dtStr)
																		});

			//�������е����ݲ��뵽DataSet��
			Neusoft.HISFC.Models.Base.ISpell spell;
			Neusoft.FrameWork.Models.NeuObject obj;
			string spellCode = "";
			string wbCode = "";
			string userCode = "";
			this.fpData_Sheet1.Rows.Count = al.Count;
			for(int i=0; i<al.Count; i++) {
				//ת��NeuObject����
				obj = al[i] as Neusoft.FrameWork.Models.NeuObject;
				//�����������Ͳ���ת����NeuObject�򷵻�
				if (obj == null) return;
				try {
					//ʵ�ֽӿ�
					spell = obj as Neusoft.HISFC.Models.Base.ISpell;
					if (spell != null) {
						spellCode = spell.SpellCode;  //ƴ����
						wbCode    = spell.WBCode;     //�����	
						userCode  = spell.UserCode;   //�Զ�����
					}

					//��myDataSet���������
					this.myDataSet.Tables[0].Rows.Add( new object[] {
																		obj.ID,    //����
																		obj.Name,  //����
																		obj.Memo,  //����
																		spellCode, //ƴ����		
																		wbCode,    //�����		
																		userCode   //�Զ�����	
																	});
					//this.fpData_Sheet1.Rows[i].Tag = obj;
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}

			//ָ��farpoint������Դ����myDataView����������
			this.myDataView = new DataView(this.myDataSet.Tables[0]);
			this.fpData.DataSource = this.myDataView;

			//�������ݿؼ���ʽ
            this.SetFormat();
		}
		

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( frmEasyChoose ) );
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.txtQueryCode = new Neusoft.FrameWork.WinForms.Controls.NeuTextBox();
            this.fpData = new FarPoint.Win.Spread.FpSpread();
            this.fpData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnOK = new Neusoft.FrameWork.WinForms.Controls.NeuButton();
            this.btnCancel = new Neusoft.FrameWork.WinForms.Controls.NeuButton();
            this.chbMisty = new Neusoft.FrameWork.WinForms.Controls.NeuCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.fpData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpData_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtQueryCode
            // 
            this.txtQueryCode.AccessibleDescription = null;
            this.txtQueryCode.AccessibleName = null;
            resources.ApplyResources( this.txtQueryCode, "txtQueryCode" );
            this.txtQueryCode.BackgroundImage = null;
            this.txtQueryCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtQueryCode.Font = null;
            this.txtQueryCode.IsEnter2Tab = false;
            this.txtQueryCode.Name = "txtQueryCode";
            this.txtQueryCode.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.VS2003;
            this.txtQueryCode.TextChanged += new System.EventHandler( this.txtQueryCode_TextChanged );
            this.txtQueryCode.KeyDown += new System.Windows.Forms.KeyEventHandler( this.txtQueryCode_KeyDown );
            this.txtQueryCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.txtQueryCode_KeyPress );
            // 
            // fpData
            // 
            this.fpData.About = "3.0.2004.2005";
            resources.ApplyResources( this.fpData, "fpData" );
            this.fpData.AccessibleName = null;
            this.fpData.BackColor = System.Drawing.SystemColors.Control;
            this.fpData.BackgroundImage = null;
            this.fpData.Font = null;
            this.fpData.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpData.Name = "fpData";
            this.fpData.Sheets.AddRange( new FarPoint.Win.Spread.SheetView[] {
            this.fpData_Sheet1} );
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font( "����", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)) );
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.fpData.TextTipAppearance = tipAppearance1;
            this.fpData.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fpData.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler( this.fpData_CellDoubleClick );
            this.fpData.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.fpData_KeyPress );
            // 
            // fpData_Sheet1
            // 
            this.fpData_Sheet1.Reset();
            this.fpData_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fpData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.fpData_Sheet1.ColumnCount = 6;
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 0 ).Value = "����";
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 1 ).Value = "����";
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 2 ).Value = "����";
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 3 ).Value = "ƴ����";
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 4 ).Value = "�����";
            this.fpData_Sheet1.ColumnHeader.Cells.Get( 0, 5 ).Value = "�Զ�����";
            this.fpData_Sheet1.ColumnHeader.Rows.Get( 0 ).Height = 24F;
            this.fpData_Sheet1.Columns.Get( 0 ).AllowAutoSort = true;
            this.fpData_Sheet1.Columns.Get( 0 ).Label = "����";
            this.fpData_Sheet1.Columns.Get( 0 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 0 ).Width = 79F;
            this.fpData_Sheet1.Columns.Get( 1 ).Label = "����";
            this.fpData_Sheet1.Columns.Get( 1 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 1 ).Width = 148F;
            this.fpData_Sheet1.Columns.Get( 2 ).Label = "����";
            this.fpData_Sheet1.Columns.Get( 2 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 2 ).Width = 64F;
            this.fpData_Sheet1.Columns.Get( 3 ).Label = "ƴ����";
            this.fpData_Sheet1.Columns.Get( 3 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 3 ).Width = 84F;
            this.fpData_Sheet1.Columns.Get( 4 ).Label = "�����";
            this.fpData_Sheet1.Columns.Get( 4 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 4 ).Width = 76F;
            this.fpData_Sheet1.Columns.Get( 5 ).Label = "�Զ�����";
            this.fpData_Sheet1.Columns.Get( 5 ).Locked = true;
            this.fpData_Sheet1.Columns.Get( 5 ).Width = 92F;
            this.fpData_Sheet1.DataAutoHeadings = false;
            this.fpData_Sheet1.DataAutoSizeColumns = false;
            this.fpData_Sheet1.GrayAreaBackColor = System.Drawing.Color.Honeydew;
            this.fpData_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.fpData_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fpData_Sheet1.RowHeader.Columns.Get( 0 ).AllowAutoSort = true;
            this.fpData_Sheet1.RowHeader.Columns.Get( 0 ).Width = 21F;
            this.fpData_Sheet1.SelectionForeColor = System.Drawing.Color.White;
            this.fpData_Sheet1.SheetCornerStyle.Locked = false;
            this.fpData_Sheet1.SheetCornerStyle.Parent = "HeaderDefault";
            this.fpData_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.fpData_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources( this.btnOK, "btnOK" );
            this.btnOK.BackgroundImage = null;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.VS2003;
            this.btnOK.Type = Neusoft.FrameWork.WinForms.Controls.General.ButtonType.None;
            this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources( this.btnCancel, "btnCancel" );
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.VS2003;
            this.btnCancel.Type = Neusoft.FrameWork.WinForms.Controls.General.ButtonType.None;
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            // 
            // chbMisty
            // 
            this.chbMisty.AccessibleDescription = null;
            this.chbMisty.AccessibleName = null;
            resources.ApplyResources( this.chbMisty, "chbMisty" );
            this.chbMisty.BackgroundImage = null;
            this.chbMisty.Checked = true;
            this.chbMisty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbMisty.Font = null;
            this.chbMisty.Name = "chbMisty";
            this.chbMisty.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.VS2003;
            this.chbMisty.CheckedChanged += new System.EventHandler( this.chbMisty_CheckedChanged_2 );
            // 
            // frmEasyChoose
            // 
            this.AcceptButton = this.btnOK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources( this, "$this" );
            this.BackgroundImage = null;
            this.CancelButton = this.btnCancel;
            this.Controls.Add( this.btnCancel );
            this.Controls.Add( this.btnOK );
            this.Controls.Add( this.fpData );
            this.Controls.Add( this.txtQueryCode );
            this.Controls.Add( this.chbMisty );
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.KeyPreview = true;
            this.Name = "frmEasyChoose";
            this.Activated += new System.EventHandler( this.frmEasyChoose_Activated );
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.frmEasyChoose_FormClosed );
            ((System.ComponentModel.ISupportInitialize)(this.fpData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpData_Sheet1)).EndInit();
            this.ResumeLayout( false );
            this.PerformLayout();

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e) {
			this.GetItem();
		}


		private void btnCancel_Click(object sender, System.EventArgs e) {
            //liuke 20091026 del start
            //if (this.SelectedItem != null)
            //{
            //    SelectedItem(this.myObject);
            //}
            //liuke 20091026 del end

			this.Close();
		}


		private void txtQueryCode_TextChanged(object sender, System.EventArgs e) {
			this.ChangeItem();
		}


		private void txtQueryCode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			//�س�ѡ����Ŀ
			if(e.KeyChar == (char)13) {
				this.GetItem();
				return;
			}
			else if(e.KeyChar == (char)27) {
				this.Close();
				return;
			}
		}


		private void txtQueryCode_KeyDown(object sender,System.Windows.Forms.KeyEventArgs e) {
			//�ϼ�ͷѡ����һ����¼
			if(e.KeyCode == Keys.Up) {
				if (this.fpData_Sheet1.ActiveRowIndex > 0) {
					this.fpData_Sheet1.ActiveRowIndex--;
                    this.fpData.ShowRow(0, this.fpData_Sheet1.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
					return;
				}
			}

			//�¼�ͷѡ����һ����¼
			if(e.KeyCode == Keys.Down) {
				if (this.fpData_Sheet1.ActiveRowIndex < this.fpData_Sheet1.RowCount) {
					this.fpData_Sheet1.ActiveRowIndex++;
                    this.fpData.ShowRow(this.fpData.ActiveSheetIndex, this.fpData_Sheet1.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
					return;
				}
			}
		}


		private void chbMisty_CheckedChanged(object sender, System.EventArgs e) {
			this.txtQueryCode.Focus();
		}


		private void fpData_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e) {
			this.GetItem();
		}


		private void fpData_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			//�س�ѡ����Ŀ
			if(e.KeyChar == (char)13) {
				this.GetItem();
				return;
			}
		}

		private void frmEasyChoose_Activated(object sender, System.EventArgs e)
		{
			this.txtQueryCode.Focus();
			this.txtQueryCode.SelectAll();
		}

        private void chbMisty_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void chbMisty_CheckedChanged_2(object sender, EventArgs e)
        {
            this.ChangeItem();
        }

        private void frmEasyChoose_FormClosed(object sender, FormClosedEventArgs e)
        {
            //liuke 20091026 del start
            //if (SelectedItem != null)
            //{
            //    SelectedItem(this.myObject);
            //}
            //liuke 20091026 del end
        }

        protected override void OnLoad(EventArgs e)
        {
            this.btnOK.Text = Neusoft.FrameWork.Management.Language.Msg( "ȷ��" ) + "(&O)";
            this.btnCancel.Text = Neusoft.FrameWork.Management.Language.Msg( "ȡ��" ) + "(&C)";
            this.Text = Neusoft.FrameWork.Management.Language.Msg( "��������" );

            base.OnLoad( e );
        }

	}
}
