using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
namespace Neusoft.NFC.Interface.Controls
{
	/// <summary>
	/// �����Farpoint�ؼ�
	/// </summary>
	public class ucFP : System.Windows.Forms.UserControl
	{
		protected FarPoint.Win.Spread.FpSpread fpSpread1;
		protected FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
		
		/// <summary>
		/// ���̰����¼�����
		/// </summary>
		public event System.Windows.Forms.KeyPressEventHandler KeyPressEvent;
		/// <summary> 
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucFP()
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
			this.fpSpread1.EditModePermanent = true;
			this.fpSpread1.EditModeReplace = true;
			this.fpSpread1.Location = new System.Drawing.Point(0, 0);
			this.fpSpread1.Name = "fpSpread1";
			this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
																				   this.fpSpread1_Sheet1});
			this.fpSpread1.Size = new System.Drawing.Size(536, 360);
			this.fpSpread1.TabIndex = 0;
			this.fpSpread1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fpSpread1_KeyPress);
			this.fpSpread1.EditModeOn += new System.EventHandler(this.fpSpread1_EditModeOn);
			this.fpSpread1.EditModeOff += new System.EventHandler(this.fpSpread1_EditModeOff);
			this.fpSpread1.ComboSelChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.fpSpread1_ComboSelChange);
			this.fpSpread1.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.fpSpread1_Change);
			this.fpSpread1.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(this.fpSpread1_EditChange);
			// 
			// fpSpread1_Sheet1
			// 
			this.fpSpread1_Sheet1.Reset();
			this.fpSpread1_Sheet1.SheetName = "Sheet1";
			// 
			// fpInput
			// 
			this.Controls.Add(this.fpSpread1);
			this.Name = "fpInput";
			this.Size = new System.Drawing.Size(536, 360);
			this.Load += new System.EventHandler(this.fpInput_Load);
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region ����
		/// <summary>
		/// �Ƿ��Զ���ת
		/// </summary>
		public bool IsAutoSkip = true;
		
		/// <summary>
		/// ��ǰ��
		/// </summary>
		public int CurrentColumn = 0;

		/// <summary>
		/// �Ƿ��л�����
		/// </summary>
		public bool IsHaveSumRow = true;

		/// <summary>
		/// ��ǰ��
		/// </summary>
		public int CurrentRow = 0;

		/// <summary>
		/// ��ǰSheetView
		/// </summary>
		public FarPoint.Win.Spread.SheetView SheetView
		{
			get
			{
				return this.fpSpread1_Sheet1;
			}
		}
		/// <summary>
		/// ��ǰfp
		/// </summary>
		public FarPoint.Win.Spread.FpSpread FpSpread
		{
			get
			{
				return this.fpSpread1;
			}
		}
		#endregion

		protected override bool ProcessDialogKey(Keys keyData)
		{
			
			return base.ProcessDialogKey (keyData);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			
			if(keyData == Keys.Escape)
			{
				if(_control !=null && _control.Visible ==true)//����пؼ�
				{
					_control.Hide();//���ؿؼ�
				}
			}
			if(_control != null && _control.Visible)
			{
				#region ��ǰ�б���ʾʱ��
				IFpInputable _editor = _control as IFpInputable;
				if(_editor != null)
				{
					if(keyData == Keys.Up )
					{
						_editor.MovePrevious();
					}
					else if(keyData == Keys.Down  )
					{
						_editor.MoveNext();
					}
					else if(keyData == Keys.PageUp  )
					{
						_editor.PreviousPage();
					}
					else if(keyData == Keys.PageDown  )
					{
						_editor.NextPage();
					}
					else if(keyData == Keys.Enter  )
					{
						object obj = _editor.GetSelectedItem();
						if(obj != null && obj.ToString() !="" )
						{
							try
							{
								this.SheetView.ActiveCell.Tag = obj;
								this.SheetView.ActiveCell.Text = obj.ToString();
							}
							catch{}
						}
						_control.Hide();
					}
				}
				#endregion
			}
			else//û���б�ʱ�򣬽�������ѡ��
			{
				if(keyData == Keys.Up )
				{
					if(this.fpSpread1.Sheets[0].ActiveRowIndex >0)
						this.fpSpread1.Sheets[0].SetActiveCell(this.fpSpread1.Sheets[0].ActiveRowIndex -1,this.fpSpread1.Sheets[0].ActiveColumnIndex);
				}
				else if(keyData == Keys.Down  )
				{
					if(this.fpSpread1.Sheets[0].ActiveRowIndex <this.fpSpread1.Sheets[0].RowCount -1)
						this.fpSpread1.Sheets[0].SetActiveCell(this.fpSpread1.Sheets[0].ActiveRowIndex +1,this.fpSpread1.Sheets[0].ActiveColumnIndex);
				}
				else if(keyData == Keys.Left || keyData == Keys.Right )
				{
					if(_control!=null) _control.Show();//����пؼ�û��ʾ����ʾ����
				}
			}
			if(this.IsAutoSkip && (keyData == Keys.Enter))
			{
				this.skip();
			
			}

			if(KeyPressEvent!=null)
			{
				KeyPressEventArgs e = new KeyPressEventArgs((char)keyData.GetHashCode());
				KeyPressEvent(this,e);
			}
			
			
			return base.ProcessCmdKey (ref msg, keyData);
		}

		private void skip()
		{
			int SumRow = 0;
			if(IsHaveSumRow == true)
				SumRow = 1;
			if(CurrentColumn>= this.fpSpread1.Sheets[0].ColumnCount-1 )
			{
			
				//��ת����һ��
				if(CurrentRow>=this.fpSpread1.Sheets[0].RowCount-1-SumRow)
					this.fpSpread1.Sheets[0].Rows.Add(CurrentRow+1,1);
				this.fpSpread1.Sheets[0].SetActiveCell(CurrentRow+1,0,false);
			}
			else//��ת����һ��
			{
				try
				{
					this.fpSpread1.Sheets[0].SetActiveCell(CurrentRow,CurrentColumn+1,false);
				}
				catch
				{
					//��ת����һ��
					if(CurrentRow>=this.fpSpread1.Sheets[0].RowCount-1 - SumRow)
						this.fpSpread1.Sheets[0].Rows.Add(CurrentRow+1,1);
					this.fpSpread1.Sheets[0].SetActiveCell(CurrentRow +1,0,false);
				}
			}
			
			if(this.fpSpread1.Sheets[0].Columns[this.fpSpread1.Sheets[0].ActiveColumnIndex].Visible ==false 
				|| this.fpSpread1.Sheets[0].Columns[this.fpSpread1.Sheets[0].ActiveColumnIndex].Width == 0
				|| this.fpSpread1.Sheets[0].Columns[this.fpSpread1.Sheets[0].ActiveColumnIndex].Locked == true)
			{
				this.CurrentColumn = this.fpSpread1.Sheets[0].ActiveColumnIndex;
				this.CurrentRow = this.fpSpread1.Sheets[0].ActiveRowIndex;
				this.skip();
			}

		}

		private void fpInput_Load(object sender, System.EventArgs e)
		{
			this.initFP();
		}
		/// <summary>
		/// ��ʼ��fp
		/// </summary>
		public void initFP()
		{
			try
			{
				this.fpSpread1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
				InputMap im;
				im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused);
				im.Put(new Keystroke(Keys.Enter,Keys.None),FarPoint.Win.Spread.SpreadActions.None);
		
				im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				im.Put(new Keystroke(Keys.Down,Keys.None),FarPoint.Win.Spread.SpreadActions.None);

				im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				im.Put(new Keystroke(Keys.Up,Keys.None),FarPoint.Win.Spread.SpreadActions.None);

				im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				im.Put(new Keystroke(Keys.Escape,Keys.None),FarPoint.Win.Spread.SpreadActions.None);

				//			im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				//			im.Put(new Keystroke(Keys.F2,Keys.None),FarPoint.Win.Spread.SpreadActions.None);
				//
				//			im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				//			im.Put(new Keystroke(Keys.F3,Keys.None),FarPoint.Win.Spread.SpreadActions.None);
				//
				//			im=fpSpread1.GetInputMap(InputMapMode.WhenAncestorOfFocused );
				//			im.Put(new Keystroke(Keys.F4,Keys.None),FarPoint.Win.Spread.SpreadActions.None);
			}
			catch{}
		}

		#region farpoint �¼�
		
		private void fpSpread1_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
		{
			//this.ParentForm.Text = "EditChange";
			if(_control!=null)
			{
				_control.BringToFront();
				_control.Show();

				IFpInputable _editor = _control as IFpInputable;
				if(_editor!=null)
				{
					try
					{
						_editor.Filter(e.EditingControl.Text);
					}
					catch{}
				}
			}
		}

		private void fpSpread1_EditModeOn(object sender, System.EventArgs e)
		{
			//this.ParentForm.Text = "EditModeOn";
			this.SetLocation();
			if(_control!=null)
			{
				IFpInputable _editor = _control as IFpInputable;
				if(_editor!=null)
				{
					try
					{
						_editor.Filter(this.fpSpread1.EditingControl.Text);
					}
					catch{}
				}
			}
			for(int i=0;i<this.alControls.Count;i++)
			{
				if(this.alControls[i] != null && this.alControls[i]!=_control)
				{
					((Control)this.alControls[i]).Hide();
				}
			}
		}

		private void fpSpread1_EditModeOff(object sender, System.EventArgs e)
		{
			//this.ParentForm.Text = "EditModeOff";
			//if(_control!=null) _control.Hide();
			if(this.fpSpread1.ContainsFocus==false)
			{
				for(int i=0;i<this.alControls.Count;i++)
				{
					if(this.alControls[i] != null)
					{
						if(((Control)this.alControls[i]).ContainsFocus==false)
							((Control)this.alControls[i]).Hide();
					}
				}
			}
		}

		private void fpSpread1_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
		{
			//this.ParentForm.Text = "Change";
			
		}

		private void fpSpread1_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
		{
			//this.ParentForm.Text = "ComboSelChange";
		}
		
		#endregion

		#region ��������
		/// <summary>
		/// ����cellControl
		/// </summary>
		/// <param name="controls"></param>
		public void SetCellControl(params Control[] controls)
		{
			alControls = new ArrayList();
			for(int i=0;i<controls.Length;i++)
			{
				alControls.Add(controls[i]);
				this.Controls.Add(controls[i]);
				try
				{
					if(controls[i]!= null)
					{
						controls[i].Visible = false;
						controls[i].BringToFront();
						IFpInputable ifp = controls[i] as IFpInputable;
						if(ifp!=null)
							ifp.SelectedItem+=new EventHandler(ifp_SelectedItem);
					}
				}
				catch{}
			}

		}
		#endregion

		#region ����
		protected Control _control;//��ǰ�ؼ�
		protected ArrayList alControls = null;

		private void SetLocation()
		{
			Control _cell = this.fpSpread1.EditingControl;
			if(_cell == null) return;
			
			CurrentColumn = this.fpSpread1.Sheets[0].ActiveColumnIndex;
			CurrentRow = this.fpSpread1.Sheets[0].ActiveRowIndex;

			if(alControls == null || alControls.Count <this.fpSpread1.Sheets[0].ActiveColumnIndex+1) return;
			

			_control = alControls[this.fpSpread1.Sheets[0].ActiveColumnIndex] as Control;

			if(_control == null) return;

			int y = 0;
			if(_control.GetType().ToString().IndexOf("ListBox")>=0)
			{
				_control.Location=new Point(_cell.Location.X,
					_cell.Location.Y+_cell.Height+SystemInformation.Border3DSize.Height*2);				
				_control.Size=new Size(_cell.Width+SystemInformation.Border3DSize.Width*2,150);
			}
			else
			{
				y = _cell.Location.Y+_cell.Height +_control.Height +7;

				if(y <= this.Height)
					_control.Location=new Point(_cell.Location.X+20,y - _control.Height );
				else
					_control.Location=new Point(_cell.Location.X+20,_cell.Location.Y-_control.Height-7);
			}
			

		}
		#endregion

		private void fpSpread1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}

		private void EditingControl_KeyDown(object sender, KeyEventArgs e)
		{
				
		}

		private void ifp_SelectedItem(object sender, EventArgs e)
		{
			try
			{
				this.SheetView.ActiveCell.Tag = sender;
				this.SheetView.ActiveCell.Text = sender.ToString();
				_control.Hide();
			}
			catch{}
		}
	}
}
