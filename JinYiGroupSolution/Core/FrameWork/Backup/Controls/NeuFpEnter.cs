using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using FarPoint.Win.Spread;
namespace Neusoft.FrameWork.WinForms.Controls
{
    public partial class NeuFpEnter : FarPoint.Win.Spread.FpSpread
    {
        public NeuFpEnter()
        {
            InitializeComponent();
        }

        #region �¼�����
        //  /// <summary>
        //  /// ��ǰView
        //  /// </summary>
        //  public FarPoint.Win.Spread.SheetView SheetView=new SheetView();
        /// <summary>
        /// ��Ӧ�����¼�
        /// </summary>
        public delegate int keyDown(Keys key);
        /// <summary>
        /// ��������б�ѡ����
        /// </summary>
        public delegate int setItem(FrameWork.Models.NeuObject obj);
        /// <summary>
        /// �����¼�:Enter,Up,Down,Escape������...
        /// </summary>
        public event keyDown KeyEnter;
        /// <summary>
        /// ѡ�������б���Ŀ
        /// </summary>
        public event setItem SetItem;
        #endregion
        //��Ⱥ� ����
        private int intWidth = 150;
        private int intHeight = 200;
        //�趨�����б�Ĭ�ϲ�ѡ���κ���
        private bool selectNone = false;
        private Hashtable hs = new Hashtable();//�Ĵ�������к���
        #region  ���õ����������Ϊ "" ʱ ,Ĭ�ϲ�ѡ���κ���
        public bool SelectNone
        {
            get
            {
                return selectNone;
            }
            set
            {
                selectNone = value;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// �����б���
        /// </summary>
        public Neusoft.FrameWork.WinForms.Controls.PopUpListBox[] Lists = new Neusoft.FrameWork.WinForms.Controls.PopUpListBox[10];
        /// <summary>
        /// ��cell�õ�����ʱ,�Ƿ���ʾ�����б�
        /// </summary>  
        public bool ShowListWhenOfFocus
        {
            get { return this.showListWhenOfFocus; }
            set { this.showListWhenOfFocus = value; }
        }
        /// <summary>
        /// ��cell�õ�����ʱ,�Ƿ���ʾ�����б�
        /// </summary>
        private bool showListWhenOfFocus = false;

        #endregion
        /// <summary>
        /// ������Щ�����в���ҪҲ�������¼�
        /// </summary>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public int SetSpecalCol(int iCol)
        {
            if (!hs.Contains(iCol))
            {
                hs.Add(iCol, true);
            }
            return 1;
        }
        /// <summary>
        /// �趨���е������б����ɼ� 
        /// </summary>
        /// <returns></returns>
        public int SetAllListBoxUnvisible()
        {
            try
            {
                if (Lists != null)
                {
                    foreach (Neusoft.FrameWork.WinForms.Controls.PopUpListBox currentList in Lists)
                    {
                        if (currentList != null)
                        {
                            currentList.Visible = false;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// �趨��һ����ʾ/����ʾID ���� �ɹ����� 1 ʧ�ܷ��� 0 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="IsVisiable"></param>
        /// <returns></returns>
        public int SetIDVisiable(SheetView view, int col, bool IsVisiable)
        {
            string name = view.SheetName + "_" + col.ToString();
            for (int i = 0; i < this.Lists.Length; i++)
            {
                if (this.Lists[i] != null && (this.Lists[i] as Neusoft.FrameWork.WinForms.Controls.PopUpListBox).Name == name)
                {
                    Lists[i].IsShowID = IsVisiable;
                    return 1;
                }
            }
            return 0;

        }
        /// <summary>
        /// ���������б�Ŀ�Ⱥ͸߶� 
        /// </summary>
        public void SetWidthAndHeight(int width, int height)
        {
            intWidth = width;
            intHeight = height;
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected void Init()
        {
            this.InitFp();

            //this.Sheets.Add(SheetView);
            this.EditChange += new EditorNotifyEventHandler(FpEnter_EditChange);
            this.EditModeOn += new EventHandler(FpEnter_EditModeOn);
        }
        /// <summary>
        /// ��ʼ��Fp,�����ض�������Ĭ���¼�
        /// </summary>
        protected void InitFp()
        {
            InputMap im;

            im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Down, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Up, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            im = base.GetInputMap(InputMapMode.WhenAncestorOfFocused);
            im.Put(new Keystroke(Keys.Escape, Keys.None), FarPoint.Win.Spread.SpreadActions.None);
            //ʼ�մ��ڿɱ༭״̬
            base.EditModePermanent = true;
            base.EditModeReplace = true;
        }

        /// <summary>
        /// ��Ӧ�����¼�
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.ContainsFocus)
            {
                if (keyData == Keys.Enter)
                {
                    if (this.KeyEnter != null)
                        this.KeyEnter(Keys.Enter);
                }
                else if (keyData == Keys.Up)
                {
                    Neusoft.FrameWork.WinForms.Controls.PopUpListBox current = this.getCurrentList(this.ActiveSheet, this.ActiveSheet.ActiveColumnIndex);
                    if (current != null && current.Visible)
                        current.PriorRow();
                    else
                    {
                        if (!hs.Contains(ActiveSheet.ActiveColumnIndex))
                        {
                            if (this.ActiveSheet.ActiveRowIndex > 0)
                                this.ActiveSheet.ActiveRowIndex--;
                        }
                    }

                    if (this.KeyEnter != null)
                        this.KeyEnter(Keys.Up);
                }
                else if (keyData == Keys.Down)
                {
                    Neusoft.FrameWork.WinForms.Controls.PopUpListBox current = this.getCurrentList(this.ActiveSheet, this.ActiveSheet.ActiveColumnIndex);
                    if (current != null && current.Visible)
                        current.NextRow();
                    else
                    {
                        if (!hs.Contains(ActiveSheet.ActiveColumnIndex))
                        {
                            if (this.ActiveSheet.ActiveRowIndex < this.ActiveSheet.RowCount - 1)
                                this.ActiveSheet.ActiveRowIndex++;
                        }
                    }

                    if (this.KeyEnter != null)
                        this.KeyEnter(Keys.Down);
                }
                else if (keyData == Keys.Escape)
                {
                    this.noVisible();

                    if (this.KeyEnter != null)
                        this.KeyEnter(Keys.Escape);
                }
            }
            return base.ProcessDialogKey(keyData);
        }
        /// <summary>
        /// ����cell�����б�
        /// </summary>
        /// <param name="view"></param>
        /// <param name="col"></param>
        /// <param name="al"></param>
        public void SetColumnList(FarPoint.Win.Spread.SheetView view, int col, ArrayList al)
        {
            string name = view.SheetName + "_" + col.ToString();

            for (int i = 0; i < this.Lists.Length - 1; i++)
            {
                if (this.Lists[i] != null && (this.Lists[i] as Neusoft.FrameWork.WinForms.Controls.PopUpListBox).Name == name)
                {
                    //MessageBox.Show("cell["+row.ToString()+","+col.ToString()+"]�Ѿ����������б�!","��ʾ");
                    return;
                }
            }

            Neusoft.FrameWork.WinForms.Controls.PopUpListBox obj = new Neusoft.FrameWork.WinForms.Controls.PopUpListBox();
            obj.Name = name;
            obj.AddItems(al);
            //�õ�����б���
            int Index = -1;

            for (int i = 0; i < this.Lists.Length; i++)
            {
                if (this.Lists[i] == null)
                {
                    Index = i;
                    break;
                }
            }
            if (Index == -1)
            {
                MessageBox.Show("�б��Ѿ����������10", "��ʾ");
                return;
            }

            this.Lists[Index] = obj;
            this.Lists[Index].SelectItem += new Neusoft.FrameWork.WinForms.Controls.PopUpListBox.MyDelegate(FpEnter_SelectItem);
            this.Controls.Add(this.Lists[Index]);
            this.Lists[Index].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lists[Index].Cursor = Cursors.Hand;
            this.Lists[Index].Size = new System.Drawing.Size(intWidth, intHeight);
            this.Lists[Index].Visible = false;
            this.Lists[Index].SelectNone = selectNone;
        }

        /// <summary>
        /// ѡ����Ŀ�б�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int FpEnter_SelectItem(Keys key)
        {
            FrameWork.Models.NeuObject obj = new FrameWork.Models.NeuObject();
            Neusoft.FrameWork.WinForms.Controls.PopUpListBox current = this.getCurrentList(this.ActiveSheet,
             this.ActiveSheet.ActiveColumnIndex);

            if (current == null) return -1;

            if (current.GetSelectedItem(out obj) == -1) return -1;
            if (obj == null) return -1;

            if (this.SetItem != null)
                this.SetItem(obj);

            current.Visible = false;

            return 0;
        }
        /// <summary>
        /// ��������Ŀ�б�ģ��Զ����й���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpEnter_EditChange(object sender, EditorNotifyEventArgs e)
        {
            try
            {
                Neusoft.FrameWork.WinForms.Controls.PopUpListBox current = this.getCurrentList(this.ActiveSheet,
                 this.ActiveSheet.ActiveColumnIndex);

                if (current == null) return;

                string Text = e.EditingControl.Text.Trim();

                current.Filter(Text);

                this.ActiveSheet.SetTag(this.ActiveSheet.ActiveRowIndex, this.ActiveSheet.ActiveColumnIndex, null);

                if (current.Visible == false) current.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// ���ÿؼ�λ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FpEnter_EditModeOn(object sender, EventArgs e)
        {
            try
            {
                this.noVisible();

                Neusoft.FrameWork.WinForms.Controls.PopUpListBox current = this.getCurrentList(this.ActiveSheet,
                 this.ActiveSheet.ActiveColumnIndex);

                if (current == null) return;

                //����λ��
                this.setLocal(current);

                if (this.showListWhenOfFocus && current.Visible == false)
                {
                    current.Filter(this.ActiveSheet.ActiveCell.Text);
                    current.Visible = true;
                }
            }
            catch { }
        }
        /// <summary>
        /// ���ÿؼ�λ��
        /// </summary>
        /// <param name="obj"></param>
        private void setLocal(Neusoft.FrameWork.WinForms.Controls.PopUpListBox obj)
        {
            Control _cell = base.EditingControl;
            if (_cell == null) return;

            //int y = _cell.Top + _cell.Height + obj.Height;//+SystemInformation.Border3DSize.Height*2;
            //if (y <= this.Height)
            //    obj.Location = new System.Drawing.Point(_cell.Left, y - obj.Height);
            //else
            //    obj.Location = new System.Drawing.Point(_cell.Left, _cell.Top - obj.Height);

            int topHeight = _cell.Top;
            int bottomHeight = this.Height - _cell.Top - _cell.Height;

            if (bottomHeight > obj.Height)        //����ʾ���²�
            {
                obj.Location = new Point(_cell.Left + 20, _cell.Top + _cell.Height + 5);
            }
            else if (topHeight > obj.Height)      //����ʾ���ϲ�
            {
                obj.Location = new Point(_cell.Left + 20, _cell.Top - obj.Height - 5);
            }
            else                                               //ƽ����ʾ
            {
                obj.Location = new Point(_cell.Left + _cell.Width + 10, _cell.Top);
            }


        }
        /// <summary>
        /// ��ȡ��ǰcell�Ƿ��������б�
        /// </summary>
        /// <param name="view"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Neusoft.FrameWork.WinForms.Controls.PopUpListBox getCurrentList(SheetView view, int col)
        {
            string name = view.SheetName + "_" + col.ToString();
            for (int i = 0; i < this.Lists.Length; i++)
            {
                if (this.Lists[i] != null && (this.Lists[i] as Neusoft.FrameWork.WinForms.Controls.PopUpListBox).Name == name)
                    return this.Lists[i];
            }
            return null;
        }

        /// <summary>
        /// ���ɼ�
        /// </summary>
        private void noVisible()
        {
            for (int i = 0; i < this.Lists.Length; i++)
            {
                if (this.Lists[i] != null)
                {
                    this.Lists[i].Visible = false;
                }
            }
        }
    }
}
