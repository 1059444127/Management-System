using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using FarPoint.Win.Spread;
using System.ComponentModel;
using System.Collections.Generic;
using Neusoft.FrameWork.Models;

namespace Neusoft.FrameWork.WinForms.Controls
{
	/// <summary>
	/// NeuSpread<br></br>
	/// [��������: NeuSpread�ؼ�]<br></br>
	/// [�� �� ��: ����ȫ]<br></br>
	/// [����ʱ��: 2006-09-07]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
	[ToolboxBitmap(typeof(FarPoint.Win.Spread.FpSpread))]
	public class NeuSpread : FarPoint.Win.Spread.FpSpread, INeuControl
	{
		public NeuSpread()
		{
            this.VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            this.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            this.BackColor = Color.White;
            foreach (FarPoint.Win.Spread.SheetView view in this.Sheets)
            {
                view.GrayAreaBackColor = Color.White;
            }
		}
		#region �ֶ�

		private StyleType styleType;

		private frmSpreadGridConfig2005 config;
		private static XmlDocument xmlDoc = new XmlDocument();
		/// <summary>
		/// �������ؼ�
		/// </summary>
		private Form parentForm;
		/// <summary>
		/// �Ƿ��û��������е���ʾ״̬
		/// </summary>
		private bool canConfigColumn;
		/// <summary>
		/// ����״̬�ļ���
		/// </summary>
		private string fileName = string.Empty;

		private bool autoSaveConfig;

        private bool manualKey;
        /// <summary>
        /// ���������б��
        /// </summary>
        private Dictionary<int, NeuListBoxPopup> listBoxPopups = new Dictionary<int, NeuListBoxPopup>();
#endregion

#region ����
		/// <summary>
		/// ��������
		/// </summary>
		private frmSpreadGridConfig2005 FormConfig
		{
			get
			{
				if (this.config == null) 
				{
					this.config = new frmSpreadGridConfig2005(this);					
				}

                return this.config;
            }
		}

		/// <summary>
		/// ����״̬�ļ���,��ʱĬ��Ϊ Parent.GetType().ToString()+".xml"
		/// </summary>
		[System.ComponentModel.Description("����״̬�ļ���,��ʱĬ��ΪParent.GetType().ToString()+.xml")]
		public string FileName
		{
			get
			{
				if (this.DesignMode || this.fileName.Length > 0)
				{
					return this.fileName;
				}
				else				
				{
					return this.FindForm().GetType().ToString()+".xml";
				}				
			}
			set
			{
				this.fileName = value;
			}
		}

		/// <summary>
		/// �Ƿ��û��������е���ʾ״̬
		/// </summary>
		public bool IsCanCustomConfigColumn
		{
			get
			{
				return this.canConfigColumn;
			}
			set
			{
				this.canConfigColumn = value;
			}
		}


		/// <summary>
		/// �Ƿ��Զ�������״̬
		/// </summary>
		[Browsable(false)]
		public bool IsAutoSaveGridStatus
		{
			get
			{
				return this.autoSaveConfig;
			}
			set
			{
				this.autoSaveConfig = value;

				if (!this.DesignMode) 
				{
                    this.parentForm = this.FindForm();
					if (this.parentForm != null)
					{
						this.LoadGridStatus();
						if (value)
						{
                            this.parentForm.Load += new EventHandler(this.OnLoad);
							this.parentForm.Closed += new EventHandler(this.OnClosed);
						}
						else
						{
                            this.parentForm.Load -= new EventHandler(this.OnLoad);
							this.parentForm.Closed -= new EventHandler(this.OnClosed);
						}
						
					}
				}
			}
		}

        
#endregion
		#region INeuControl ��Ա

		public Neusoft.FrameWork.WinForms.Controls.StyleType Style
		{
			get
			{				
				return this.styleType;
			}
			set
			{
				this.styleType = value;
				//ˢ�¿ؼ�
				this.Width+=1;
				this.Width-=1;
			}
		}

		#endregion
#region ����
		/// <summary>
		/// �����µ�һ�У��������һ����Ϊ��ǰ��
		/// </summary>
		public void AddRow()
		{
			this.Sheets[0].RowCount += 1;
			this.Sheets[0].ActiveRowIndex = this.Sheets[0].RowCount - 1;
		}


		/// <summary>
		/// ����һ��
		/// </summary>
		/// <param name="rowIndex">������Index</param>
		public void AddRow(int rowIndex)
		{
			this.Sheets[0].Rows.Add(rowIndex,1);
		}
//		public void AddRow(int sheetIndex)
//		{
//			if (this.Sheets.Count > sheetIndex) 
//			{
//				this.Sheets[sheetIndex].RowCount += 1;
//			}
//		}

		/// <summary>
		/// ɾ�����һ��
		/// </summary>
		public void DelRow()
		{
			this.Sheets[0].RowCount -= 1;
		}

		/// <summary>
		/// ɾ��һ��
		/// </summary>
		/// <param name="rowIndex">��Index</param>
		public void DelRow(int rowIndex)
		{
			this.Sheets[0].Rows.Remove(rowIndex,1);
		}

		/// <summary>
		/// ������
		/// </summary>
		/// <param name="rowIndex">��Index</param>
		/// <param name="isLocked">�Ƿ�����</param>
		public void LockRow(int rowIndex,bool isLocked)
		{
			this.Sheets[0].Rows[rowIndex].Locked = isLocked;
		}

		/// <summary>
		/// ������
		/// </summary>
		/// <param name="columnIndex">��Index</param>
		/// <param name="isLocked">�Ƿ�����</param>
		public void LockColumn(int columnIndex, bool isLocked)
		{
			this.Sheets[0].Columns[columnIndex].Locked = isLocked;
		}

		/// <summary>
		/// �������޸�
		/// </summary>
		/// <param name="row">��</param>
		/// <param name="column">��</param>
		/// <param name="isLocked">��������</param>
		public void LockCell(int row, int column, bool isLocked)
		{
			this.Sheets[0].Cells[row,column].Locked = isLocked;
		}
		/// <summary>
		/// �������ʽ״̬,�������ڸ�������ֻ��һ��NeuSpread
		/// </summary>
		/// Robin	2006-09-13
		public void SaveGridStatus()
		{
			this.SaveGridStatus(this.FileName);
		}

		/// <summary>
		/// �������ʽ״̬
		/// <param name="fileName">�ļ���</param>
		/// </summary>
		public void SaveGridStatus(string fileName)
		{
			XmlElement xmlSheet,xmlColumn;

			xmlDoc.RemoveAll();
			XmlElement xmlRoot = xmlDoc.CreateElement("NeuSpread");
			xmlDoc.AppendChild(xmlRoot);			

			foreach (SheetView sheet in this.Sheets) 
			{
				xmlSheet = xmlDoc.CreateElement("Sheet");
				xmlRoot.AppendChild(xmlSheet);
				foreach (Column column in sheet.Columns) 
				{
					xmlColumn = xmlDoc.CreateElement("Column");
					xmlColumn.SetAttribute("Width", column.Width.ToString());
					xmlColumn.SetAttribute("Visible", column.Visible.ToString());
					xmlSheet.AppendChild(xmlColumn);
				}
			}
			
			xmlDoc.Save(fileName);
		}

		/// <summary>
		/// װ�ر���ʽ״̬
		/// </summary>
		/// <param name="fileName"></param>
		public void LoadGridStatus(string fileName)
		{
			int i=0;
			int j;

			if (!System.IO.File.Exists(fileName)) 
			{
//#if DEBUG
//                MessageBox.Show(fileName + " not exsit");				
//#endif
				return;
			}
			xmlDoc.RemoveAll();
			xmlDoc.Load(fileName);

			foreach (XmlNode sheetNode in xmlDoc.DocumentElement.ChildNodes) 
			{
				j = 0;
				SheetView sheet=this.Sheets[i];
				foreach (XmlNode columnNode in sheetNode.ChildNodes) 
				{
					string t=columnNode.Attributes["Visible"].Value;
					sheet.Columns[j].Width=System.Convert.ToInt32(columnNode.Attributes["Width"].Value);
					sheet.Columns[j].Visible = System.Convert.ToBoolean(columnNode.Attributes["Visible"].Value);
					j++;
				}
				i++;
			}
		}
		/// <summary>
		/// װ�ر���ʽ״̬,�������ڸ�������ֻ��һ��NeuSpread
		/// </summary>
		/// Robin	2006-09-13
		public void LoadGridStatus()
		{
			this.LoadGridStatus(this.FileName);
		}

		/// <summary>
		/// ��ʾ���ý���
		/// </summary>
		public virtual void ShowGridConfig()
		{
			this.FormConfig.ShowDialog();
		}

        /// <summary>
        /// ����Excel
        /// </summary>
        /// <returns>�ɹ� 1��ȡ�� 0</returns>
        public virtual int Export()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "(*.xls)|*.xls";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.SaveExcel(dlg.FileName);
                this.SaveExcel(dlg.FileName, FarPoint.Excel.ExcelSaveFlags.SaveBothCustomRowAndColumnHeaders);
                return 1;
            }
            else
                return 0;
        }

        /// <summary>
        /// ����InputMap
        /// </summary>
        /// Robin   2006-12-12
        public void SetInputMap()
        {
            InputMap im;
            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Down, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Up, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Escape, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.F2, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.F3, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = this.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.F4, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            this.manualKey = true;
        }

        /// <summary>
        /// ���������б��
        /// </summary>
        /// <param name="listBoxPopup">�б��</param>
        /// <param name="columnIndex">������</param>
        /// Robin   2006-12-12
        public void AddListBoxPopup(NeuListBoxPopup listBoxPopup, int columnIndex)
        {
            this.listBoxPopups[columnIndex] = listBoxPopup;
            this.Parent.Controls.Add(listBoxPopup);
            listBoxPopup.Visible = false;
            listBoxPopup.BringToFront();
            listBoxPopup.ItemSelected += new EventHandler(listBoxPopup_ItemSelected);
        }

        public void SetActiveCell(int row, int column)
        {
            this.ActiveSheet.ActiveRowIndex = row;
            this.ActiveSheet.ActiveColumnIndex = column;
            this.EditMode=true;            
        }

        /// <summary>
        /// �����б��λ��
        /// </summary>
        /// Robin   2006-12-12
        private void SetLocation()
        {
            Control _cell = this.EditingControl;
            if (_cell == null) 
                return;

            this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].Location= new Point(_cell.Location.X,
                    _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
            this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].Size = new Size(110, 150);
            return;
        }
        #region �¼�

        //  ��ʱ����������������˽�������
        //  Robin   2007-04-18
        //protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        //{
        //    base.OnMouseDown (e);

        //    if (this.canConfigColumn && e.Button == MouseButtons.Right) 
        //    {
        //        this.ShowGridConfig();
        //    }
        //}


		protected  void OnClosed(object sender, EventArgs e)
		{
			if (this.autoSaveConfig) 
			{
				this.SaveGridStatus();
			}
		}
        protected void OnLoad(object sender, EventArgs e)
        {
            if (this.autoSaveConfig)
            {
                this.LoadGridStatus();
            }
        }

        protected override void OnEditModeOn(EventArgs e)
        {
            foreach(NeuListBoxPopup listbox in this.listBoxPopups.Values)
            {
                listbox.Visible = false;
            }
            if(this.listBoxPopups.ContainsKey(this.ActiveSheet.ActiveColumnIndex))
            {
                this.SetLocation();
                this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].Visible = true;
                this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].Filter(this.ActiveSheet.ActiveCell.Text);
            }
            base.OnEditModeOn(e);
        }
        protected override void OnLeaveCell(LeaveCellEventArgs e)
        {
            string _Text;
            if (this.listBoxPopups.ContainsKey(e.Column))
            {
                _Text = this.ActiveSheet.ActiveCell.Text;
                this.listBoxPopups[e.Column].Filter(_Text);

                if (this.listBoxPopups[e.Column].Visible == true)
                    this.listBoxPopups[e.Column].Visible = false;
                if(this.listBoxPopups[e.Column].Items.Count==0)
                {
                    this.ActiveSheet.Cells[e.Row, e.Column].Text = "";
                    this.ActiveSheet.SetTag(e.Row, e.Column, null);
                }
            }
            base.OnLeaveCell(e);
        }
        protected override void OnEditChange(EditorNotifyEventArgs e)
        {
            string _Text;
            //����ʽ
            if (this.listBoxPopups.ContainsKey(e.Column))
            {
                _Text = this.ActiveSheet.ActiveCell.Text;
                this.listBoxPopups[e.Column].Filter(_Text);

                if (this.listBoxPopups[e.Column].Visible == false)
                    this.listBoxPopups[e.Column].Visible = true;

                this.ActiveSheet.SetTag(e.Row, e.Column, null);
            }


            base.OnEditChange(e);
        }

        private void listBoxPopup_ItemSelected(object sender, System.EventArgs e)
        {
            int CurrentRow = this.ActiveSheet.ActiveRowIndex;
            
            if (CurrentRow < 0) 
                return;
            NeuListBoxPopup listBoxPopup = sender as NeuListBoxPopup;
            this.StopCellEditing();
            NeuObject item = listBoxPopup.GetSelectedItem();

            if (item == null)
                return;

            this.ActiveSheet.Cells[CurrentRow, this.ActiveSheet.ActiveColumnIndex].Tag = item;
            this.ActiveSheet.SetValue(CurrentRow, this.ActiveSheet.ActiveColumnIndex, item.Name, false);

            listBoxPopup.Visible = false;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.manualKey)
            {
                if (keyData == Keys.Enter)
                {
                    #region enter
                    if (this.ContainsFocus)
                    {
                        if (this.listBoxPopups.ContainsKey(this.ActiveSheet.ActiveColumnIndex))
                        {
                            this.listBoxPopup_ItemSelected(this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex], null);
                        }

                    }
                    #endregion
                }
                else if (keyData == Keys.Up)
                {
                    #region Up
                    if (this.ContainsFocus)
                    {
                        if (this.listBoxPopups.ContainsKey(this.ActiveSheet.ActiveColumnIndex))
                        {
                            this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].PriorRow();
                        }
                        else
                        {
                            int CurrentRow = this.ActiveSheet.ActiveRowIndex;
                            if (CurrentRow > 0)
                            {
                                this.ActiveSheet.ActiveRowIndex = CurrentRow - 1;
                                this.ActiveSheet.AddSelection(CurrentRow - 1, 0, 1, 0);
                                //FarPoint.Win.Spread.LeaveCellEventArgs e = new FarPoint.Win.Spread.LeaveCellEventArgs
                                //    (new FarPoint.Win.Spread.SpreadView(fpSpread1), 0, 0, CurrentRow - 1, fpSpread1_Sheet1.ActiveColumnIndex);
                                //fpSpread1_LeaveCell(fpSpread1, e);
                            }
                        }
                    }
                    #endregion
                }
                else if (keyData == Keys.Down)
                {
                    #region Down
                    if (this.ContainsFocus)
                    {
                        if (this.listBoxPopups.ContainsKey(this.ActiveSheet.ActiveColumnIndex))
                        {
                            this.listBoxPopups[this.ActiveSheet.ActiveColumnIndex].NextRow();
                        }
                        else
                        {
                            int CurrentRow = this.ActiveSheet.ActiveRowIndex;
                            if (CurrentRow < this.ActiveSheet.RowCount - 1)
                            {
                                this.ActiveSheet.ActiveRowIndex = CurrentRow + 1;
                                this.ActiveSheet.AddSelection(CurrentRow + 1, 0, 1, 0);
                                //FarPoint.Win.Spread.LeaveCellEventArgs e = new FarPoint.Win.Spread.LeaveCellEventArgs
                                //    (new FarPoint.Win.Spread.SpreadView(fpSpread1), 0, 0, CurrentRow + 1, fpSpread1_Sheet1.ActiveColumnIndex);
                                //fpSpread1_LeaveCell(fpSpread1, e);
                            }
                        }
                    }
                    #endregion
                }
                else if (keyData == Keys.Escape)
                {
                    foreach (NeuListBoxPopup listbox in this.listBoxPopups.Values)
                    {
                        listbox.Visible = false;
                    }
                }
            }

            return base.ProcessDialogKey(keyData);
        }
        #endregion

        //private Form GetParentForm(Control control)
        //{
        //    if (control == null) 
        //    {
        //        return null;
        //    }

        //    if (control.Parent is Form) 
        //    {
        //        return control.Parent as Form;
        //    }else
        //    {
        //        return GetParentForm(control.Parent);
        //    }
        //}

#endregion
	}

    /// <summary>
    /// ʵ�ֻس���Fp��
    /// </summary>
    //public class FpEnter : FarPoint.Win.Spread.FpSpread
    //{
    //    /// <summary>
    //    /// ʵ�ֻس���Fp��
    //    /// </summary>
    //    private System.ComponentModel.Container components = null;

    //    public FpEnter(System.ComponentModel.IContainer container)
    //    {
    //        //
    //        // Windows.Forms ��׫д�����֧���������
    //        //
    //        container.Add(this);
    //        InitializeComponent();

    //        //
    //        // TODO: �� InitializeComponent ���ú�����κι��캯������
    //        //
    //        this.Init();
    //    }

    //    public FpEnter()
    //    {
    //        //
    //        // Windows.Forms ��׫д�����֧���������
    //        //
    //        InitializeComponent();

    //        //
    //        // TODO: �� InitializeComponent ���ú�����κι��캯������
    //        //
    //        this.Init();
    //    }

    //    /// <summary> 
    //    /// ������������ʹ�õ���Դ��
    //    /// </summary>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (components != null)
    //            {
    //                components.Dispose();
    //            }
    //        }
    //        base.Dispose(disposing);
    //    }


    //    #region �����������ɵĴ���
    //    /// <summary>
    //    /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
    //    /// �˷��������ݡ�
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        components = new System.ComponentModel.Container();
    //    }
    //    #endregion

    //    #region �¼�����
    //    //  /// <summary>
    //    //  /// ��ǰView
    //    //  /// </summary>
    //    //  public FarPoint.Win.Spread.SheetView SheetView=new SheetView();
    //    /// <summary>
    //    /// ��Ӧ�����¼�
    //    /// </summary>
    //    public delegate int keyDown(Keys key);
    //    /// <summary>
    //    /// ��������б�ѡ����
    //    /// </summary>
    //    public delegate int setItem(neusoft.neuFC.Object.neuObject obj);
    //    /// <summary>
    //    /// �����¼�:Enter,Up,Down,Escape������...
    //    /// </summary>
    //    public event keyDown KeyEnter;
    //    /// <summary>
    //    /// ѡ�������б���Ŀ
    //    /// </summary>
    //    public event setItem SetItem;
    //    #endregion
    //    //��Ⱥ� ����
    //    private int intWidth = 150;
    //    private int intHeight = 200;
    //    //�趨�����б�Ĭ�ϲ�ѡ���κ���
    //    private bool selectNone = false;
    //    #region  ���õ����������Ϊ "" ʱ ,Ĭ�ϲ�ѡ���κ���
    //    public bool SelectNone
    //    {
    //        get
    //        {
    //            return selectNone;
    //        }
    //        set
    //        {
    //            selectNone = value;
    //        }
    //    }
    //    #endregion
    //    #region ����
    //    /// <summary>
    //    /// �����б���
    //    /// </summary>
    //    public neusoft.neuFC.Interface.Controls.ListBox[] Lists = new neusoft.neuFC.Interface.Controls.ListBox[10];
    //    /// <summary>
    //    /// ��cell�õ�����ʱ,�Ƿ���ʾ�����б�
    //    /// </summary>  
    //    public bool ShowListWhenOfFocus
    //    {
    //        get { return this.showListWhenOfFocus; }
    //        set { this.showListWhenOfFocus = value; }
    //    }
    //    /// <summary>
    //    /// ��cell�õ�����ʱ,�Ƿ���ʾ�����б�
    //    /// </summary>
    //    private bool showListWhenOfFocus = false;

    //    #endregion
    //    /// <summary>
    //    /// �趨���е������б����ɼ� 
    //    /// </summary>
    //    /// <returns></returns>
    //    public int SetAllListBoxUnvisible()
    //    {
    //        try
    //        {
    //            if (Lists != null)
    //            {
    //                foreach (neusoft.neuFC.Interface.Controls.ListBox currentList in Lists)
    //                {
    //                    if (currentList != null)
    //                    {
    //                        currentList.Visible = false;
    //                    }
    //                }
    //            }
    //            return 1;
    //        }
    //        catch (Exception ex)
    //        {
    //            MessageBox.Show(ex.Message);
    //            return -1;
    //        }
    //    }
    //    /// <summary>
    //    /// �趨��һ����ʾ/����ʾID ���� �ɹ����� 1 ʧ�ܷ��� 0 
    //    /// </summary>
    //    /// <param name="col"></param>
    //    /// <param name="IsVisiable"></param>
    //    /// <returns></returns>
    //    public int SetIDVisiable(SheetView view, int col, bool IsVisiable)
    //    {
    //        string name = view.SheetName + "_" + col.ToString();
    //        for (int i = 0; i < this.Lists.Length; i++)
    //        {
    //            if (this.Lists[i] != null && (this.Lists[i] as neusoft.neuFC.Interface.Controls.ListBox).Name == name)
    //            {
    //                Lists[i].IsShowID = IsVisiable;
    //                return 1;
    //            }
    //        }
    //        return 0;

    //    }
    //    /// <summary>
    //    /// ���������б�Ŀ�Ⱥ͸߶� 
    //    /// </summary>
    //    public void SetWidthAndHeight(int width, int height)
    //    {
    //        intWidth = width;
    //        intHeight = height;
    //    }
    //    /// <summary>
    //    /// ��ʼ��
    //    /// </summary>
    //    protected void Init()
    //    {
    //        this.InitFp();

    //        //this.Sheets.Add(SheetView);
    //        this.EditChange += new EditorNotifyEventHandler(FpEnter_EditChange);
    //        this.EditModeOn += new EventHandler(FpEnter_EditModeOn);
    //    }
    //    /// <summary>
    //    /// ��ʼ��Fp,�����ض�������Ĭ���¼�
    //    /// </summary>
    //    protected void InitFp()
    //    {
    //        InputMap im;

    //        im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
    //        im.Put(new Keystroke(Keys.Down, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

    //        im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
    //        im.Put(new Keystroke(Keys.Up, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

    //        im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
    //        im.Put(new Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

    //        im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
    //        im.Put(new Keystroke(Keys.Escape, Keys.None), FarPoint.Win.Spread.SpreadActions.None);
    //        //ʼ�մ��ڿɱ༭״̬
    //        base.EditModePermanent = true;
    //        base.EditModeReplace = true;
    //    }

    //    /// <summary>
    //    /// ��Ӧ�����¼�
    //    /// </summary>
    //    /// <param name="keyData"></param>
    //    /// <returns></returns>
    //    protected override bool ProcessDialogKey(Keys keyData)
    //    {
    //        if (this.ContainsFocus)
    //        {
    //            if (keyData == Keys.Enter)
    //            {
    //                if (this.KeyEnter != null)
    //                    this.KeyEnter(Keys.Enter);
    //            }
    //            else if (keyData == Keys.Up)
    //            {
    //                neusoft.neuFC.Interface.Controls.ListBox current = this.getCurrentList(this.ActiveSheet, this.ActiveSheet.ActiveColumnIndex);
    //                if (current != null && current.Visible)
    //                    current.PriorRow();
    //                else
    //                {
    //                    if (this.ActiveSheet.ActiveRowIndex > 0)
    //                        this.ActiveSheet.ActiveRowIndex--;
    //                }

    //                if (this.KeyEnter != null)
    //                    this.KeyEnter(Keys.Up);
    //            }
    //            else if (keyData == Keys.Down)
    //            {
    //                neusoft.neuFC.Interface.Controls.ListBox current = this.getCurrentList(this.ActiveSheet, this.ActiveSheet.ActiveColumnIndex);
    //                if (current != null && current.Visible)
    //                    current.NextRow();
    //                else
    //                {
    //                    if (this.ActiveSheet.ActiveRowIndex < this.ActiveSheet.RowCount - 1)
    //                        this.ActiveSheet.ActiveRowIndex++;
    //                }

    //                if (this.KeyEnter != null)
    //                    this.KeyEnter(Keys.Down);
    //            }
    //            else if (keyData == Keys.Escape)
    //            {
    //                this.noVisible();

    //                if (this.KeyEnter != null)
    //                    this.KeyEnter(Keys.Escape);
    //            }
    //        }
    //        return base.ProcessDialogKey(keyData);
    //    }
    //    /// <summary>
    //    /// ����cell�����б�
    //    /// </summary>
    //    /// <param name="view"></param>
    //    /// <param name="col"></param>
    //    /// <param name="al"></param>
    //    public void SetColumnList(FarPoint.Win.Spread.SheetView view, int col, ArrayList al)
    //    {
    //        string name = view.SheetName + "_" + col.ToString();

    //        for (int i = 0; i < this.Lists.Length - 1; i++)
    //        {
    //            if (this.Lists[i] != null && (this.Lists[i] as neusoft.neuFC.Interface.Controls.ListBox).Name == name)
    //            {
    //                //MessageBox.Show("cell["+row.ToString()+","+col.ToString()+"]�Ѿ����������б�!","��ʾ");
    //                return;
    //            }
    //        }

    //        neusoft.neuFC.Interface.Controls.ListBox obj = new neusoft.neuFC.Interface.Controls.ListBox();
    //        obj.Name = name;
    //        obj.AddItems(al);
    //        //�õ�����б���
    //        int Index = -1;

    //        for (int i = 0; i < this.Lists.Length; i++)
    //        {
    //            if (this.Lists[i] == null)
    //            {
    //                Index = i;
    //                break;
    //            }
    //        }
    //        if (Index == -1)
    //        {
    //            MessageBox.Show("�б��Ѿ����������10", "��ʾ");
    //            return;
    //        }

    //        this.Lists[Index] = obj;
    //        this.Lists[Index].SelectItem += new neusoft.neuFC.Interface.Controls.ListBox.MyDelegate(FpEnter_SelectItem);
    //        this.Controls.Add(this.Lists[Index]);
    //        this.Lists[Index].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
    //        this.Lists[Index].Cursor = Cursors.Hand;
    //        this.Lists[Index].Size = new System.Drawing.Size(intWidth, intHeight);
    //        this.Lists[Index].Visible = false;
    //        this.Lists[Index].SelectNone = selectNone;
    //    }

    //    /// <summary>
    //    /// ѡ����Ŀ�б�
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    private int FpEnter_SelectItem(Keys key)
    //    {
    //        neusoft.neuFC.Object.neuObject obj = new neusoft.neuFC.Object.neuObject();
    //        neusoft.neuFC.Interface.Controls.ListBox current = this.getCurrentList(this.ActiveSheet,
    //         this.ActiveSheet.ActiveColumnIndex);

    //        if (current == null) return -1;

    //        if (current.GetSelectedItem(out obj) == -1) return -1;
    //        if (obj == null) return -1;

    //        if (this.SetItem != null)
    //            this.SetItem(obj);

    //        current.Visible = false;

    //        return 0;
    //    }
    //    /// <summary>
    //    /// ��������Ŀ�б�ģ��Զ����й���
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void FpEnter_EditChange(object sender, EditorNotifyEventArgs e)
    //    {
    //        try
    //        {
    //            neusoft.neuFC.Interface.Controls.ListBox current = this.getCurrentList(this.ActiveSheet,
    //             this.ActiveSheet.ActiveColumnIndex);

    //            if (current == null) return;

    //            string Text = e.EditingControl.Text.Trim();

    //            current.Filter(Text);

    //            this.ActiveSheet.SetTag(this.ActiveSheet.ActiveRowIndex, this.ActiveSheet.ActiveColumnIndex, null);

    //            if (current.Visible == false) current.Visible = true;
    //        }
    //        catch { }
    //    }

    //    /// <summary>
    //    /// ���ÿؼ�λ��
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    private void FpEnter_EditModeOn(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            this.noVisible();

    //            neusoft.neuFC.Interface.Controls.ListBox current = this.getCurrentList(this.ActiveSheet,
    //             this.ActiveSheet.ActiveColumnIndex);

    //            if (current == null) return;

    //            //����λ��
    //            this.setLocal(current);

    //            if (this.showListWhenOfFocus && current.Visible == false)
    //            {
    //                current.Filter(this.ActiveSheet.ActiveCell.Text);
    //                current.Visible = true;
    //            }
    //        }
    //        catch { }
    //    }
    //    /// <summary>
    //    /// ���ÿؼ�λ��
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void setLocal(neusoft.neuFC.Interface.Controls.ListBox obj)
    //    {
    //        Control _cell = base.EditingControl;
    //        if (_cell == null) return;

    //        int y = _cell.Top + _cell.Height + obj.Height;//+SystemInformation.Border3DSize.Height*2;
    //        if (y <= this.Height)
    //            obj.Location = new System.Drawing.Point(_cell.Left, y - obj.Height);
    //        else
    //            obj.Location = new System.Drawing.Point(_cell.Left, _cell.Top - obj.Height);

    //    }
    //    /// <summary>
    //    /// ��ȡ��ǰcell�Ƿ��������б�
    //    /// </summary>
    //    /// <param name="view"></param>
    //    /// <param name="col"></param>
    //    /// <returns></returns>
    //    public neusoft.neuFC.Interface.Controls.ListBox getCurrentList(SheetView view, int col)
    //    {
    //        string name = view.SheetName + "_" + col.ToString();
    //        for (int i = 0; i < this.Lists.Length; i++)
    //        {
    //            if (this.Lists[i] != null && (this.Lists[i] as neusoft.neuFC.Interface.Controls.ListBox).Name == name)
    //                return this.Lists[i];
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// ���ɼ�
    //    /// </summary>
    //    private void noVisible()
    //    {
    //        for (int i = 0; i < this.Lists.Length; i++)
    //        {
    //            if (this.Lists[i] != null)
    //            {
    //                this.Lists[i].Visible = false;
    //            }
    //        }
    //    }
    //}

}
