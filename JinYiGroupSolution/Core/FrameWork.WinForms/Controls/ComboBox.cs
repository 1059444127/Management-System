using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// ComboBox ��ժҪ˵����
    /// </summary>
    [ToolboxBitmap( typeof( System.Windows.Forms.ComboBox ) )]
    public class NeuComboBox : System.Windows.Forms.ComboBox, INeuControl
    {
        private System.ComponentModel.IContainer components = null;

        public NeuComboBox(System.ComponentModel.IContainer container)
        {
            //
            // Windows.Forms ��׫д�����֧���������
            //
            //container.Add(this);
            InitializeComponent();

            // TODO: �� InitializeComponent ���ú�����κι��캯������
            //
            SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, false );
        }

        public NeuComboBox()
        {
            //
            // Windows.Forms ��׫д�����֧���������
            //
            InitializeComponent();
            this.SelectedIndexChanged += new EventHandler( NeuComboBox_SelectedIndexChanged );
            SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, false );
        }

        /// <summary> 
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                #region Add By Liangjz

                if (this.alItems != null)
                {
                    this.alItems.Clear();
                    this.alItems = null;
                }
                if (this.frmPop != null)
                {
                    this.frmPop.Dispose();
                    this.frmPop = null;
                }


                #endregion

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
            this.SuspendLayout();
            // 
            // NeuComboBox
            // 
            this.Leave += new System.EventHandler( this.ComboBox_Leave );
            this.ResumeLayout( false );

        }
        #endregion

        #region "myCode"
        public ArrayList alItems = new ArrayList();
        protected bool isItemOnly = false;
        protected bool bShowCustomerList = false;
        protected Neusoft.FrameWork.WinForms.Forms.frmEasyChoose frmPop;
        protected bool bShowID;
        //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
        //private bool isTextChanged = false;
        protected Button btn = new Button();
        protected void SetButton()
        {
            //btn.Text ="��";
            btn.Visible = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.TabStop = false;
            btn.BringToFront();
            btn.BackColor = Color.Silver;
            //			Rectangle rc = ClientRectangle;
            //			
            ////			int width = ARROW_WIDTH;
            //			int left = rc.Right - width - 2;
            //			int top = rc.Top + 1;
            //			int height = rc.Height - 2;
            //			
            //			btn.Size = new Size(width,height);
            //			btn.Location = new Point(left ,top);

            btn.Size = new Size( 18, this.Height - 2 );
            btn.Location = new Point( this.Width - 18, 1 );
            btn.Click += new EventHandler( btn_Click );
            this.Controls.Add( btn );
        }


        public int AppendItems(ArrayList Items)
        {
            base.Items.Clear();

            ArrayList items = Items.Clone() as ArrayList;

            if (alItems == null)
                alItems = new ArrayList();

            for (int i = 0; i < items.Count; i++)
            {
                alItems.Insert( alItems.Count, items[i] );
            }

            Neusoft.FrameWork.Models.NeuObject objItem;
            try
            {
                for (int i = 0; i < alItems.Count; i++)
                {
                    objItem = new Neusoft.FrameWork.Models.NeuObject();
                    objItem = (Neusoft.FrameWork.Models.NeuObject)alItems[i];
                    if (this.bShowID)
                        base.Items.Add( objItem.ID );
                    else
                        base.Items.Add( objItem.Name );
                }
            }
            catch
            {
                return -1;
            }
            //��ʼ������
            frmPop = new Neusoft.FrameWork.WinForms.Forms.frmEasyChoose( this.alItems );
            frmPop.Text = "��ѡ����Ŀ";
            frmPop.StartPosition = FormStartPosition.CenterScreen;
            frmPop.SelectedItem += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler( frmPop_SelectedItem );
            //��ȡ�������뷨�����ļ�
            SpellCode = Neusoft.FrameWork.WinForms.Classes.Function.GetInputType();

            return 0;
        }
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public void ClearItems()
        {
            this.alItems = new ArrayList();
            this.Items.Clear();
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int AddItems(ArrayList items)
        {
            if (items == null)
                return -1;

            base.Items.Clear();
            alItems = new ArrayList();
            return this.AppendItems( items );

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddItems(System.Collections.Generic.List<Neusoft.FrameWork.Models.NeuObject> list)
        {
            this.alItems = new ArrayList();
            foreach (Neusoft.FrameWork.Models.NeuObject obj in list)
            {
                this.alItems.Add( obj );
            }

            return this.AddItems( alItems );
        }
        /// <summary>
        /// �����Ĵ���
        /// </summary>
        public Form PopForm
        {
            get
            {
                return this.frmPop;
            }
            set
            {

            }
        }

        private bool isPopForm = true;

        /// <summary>
        /// �����Ĵ���
        /// </summary>
        [Description( "�Ƿ���ʾ�����б�" )]
        public bool IsPopForm
        {
            get
            {
                return this.isPopForm;
            }
            set
            {
                this.isPopForm = value;
            }
        }
        /// <summary>
        /// �Ƿ���ʾID
        /// </summary>
        [Obsolete( "��IsShowID����,�Ƿ��������б���ʾID������Name����ʾ", false )]
        public bool ShowID
        {
            get
            {
                return this.bShowID;
            }
            set
            {
                this.bShowID = value;
            }
        }
        /// <summary>
        /// �Ƿ��������б���ʾID������Name����ʾ
        /// </summary>
        [Description( "�Ƿ��������б���ʾID������Name����ʾ" )]
        public bool IsShowID
        {
            get
            {
                return this.bShowID;
            }
            set
            {
                this.bShowID = value;
            }
        }
        #endregion

        #region ����
        protected bool isLike = true;
        /// <summary>
        /// �Ƿ�ģ����ѯ
        /// </summary>
        [Description( "����ʱ���Ƿ�����ģ����ѯ��������Ŀ" )]
        public bool IsLike
        {
            get
            {
                return this.isLike;
            }
            set
            {
                this.isLike = value;
            }
        }
        /// <summary>
        /// ѡ�����Ŀ
        /// </summary>
        public new Neusoft.FrameWork.Models.NeuObject SelectedItem
        {
            get
            {
                if (this.SelectedIndex < 0)
                    return null;
                return this.alItems[this.SelectedIndex] as Neusoft.FrameWork.Models.NeuObject;
            }
        }

        /// <summary>
        /// ֻ�ܴ������б�����ѡ��
        /// Ĭ��Ϊfalse
        /// </summary>
        [Description( "�Ƿ������û���������б����ݣ�����������ֻ���б�����" )]
        public bool IsListOnly
        {
            set
            {
                this.isItemOnly = value;
            }
            get
            {
                return isItemOnly;
            }
        }
        /// <summary>
        ///  ��ʾ�Զ����б�
        /// </summary>
        [Description( "�Ƿ���ʾ�û��б�������ԭʼ�����б�" )]
        public bool IsShowCustomerList
        {
            get
            {
                return this.bShowCustomerList;
            }
            set
            {
                this.bShowCustomerList = value;

            }

        }
        /// <summary>
        /// ��ʾ�Զ����б�
        /// </summary>
        public bool ShowCustomerList
        {
            get
            {
                return this.bShowCustomerList;
            }
            set
            {
                this.bShowCustomerList = value;
            }

        }

        /// <summary>
        /// ���û���õ�ǰTag
        /// </summary>
        public new object Tag
        {
            get
            {
                if (this.Text.Trim() == "")
                    base.Tag = "";
                return base.Tag;
            }
            set
            {

                int i;
                try
                {
                    if (value == null || value.ToString().Trim() == "")
                    {
                        base.Tag = value;
                        base.Text = null;
                        return;
                    }
                }
                catch
                {
                }
                //ͨ���ı�����Tag�Ͳ����¸�ֵ��
                if (isTextChangeSetTag == false)
                {
                    try
                    {
                        for (i = 0; i < alItems.Count; i++)
                        {
                            try
                            {
                                string sValue = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).ID.ToString();
                                if (value.ToString() == sValue)
                                {
                                    #region ԭ��������{2C15D3C3-50E7-47b8-B56A-C82E0498FBE9} ��NAME��ֵ��TEXT������ edit by zl 2010-12-22
                                    if (bShowID)
                                        base.Text = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).ID;//���¸�Text
                                    else
                                    {
                                        //base.Text = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).Name;//���¸�Text
                                        base.SelectedIndex = i;
                                    }
                                    #endregion
                                    //base.Text = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).ID;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                string s = ex.Message;
                            }
                        }

                    }
                    catch
                    {
                    }
                }
                base.Tag = value;
            }



        }

        public bool IsFlat
        {
            get
            {
                return false;
            }
            set
            {
            }

        }

        public bool ToolBarUse
        {
            get
            {
                return false;
            }
            set
            {
            }

        }
        public bool IsEnter2Tab
        {
            get
            {
                return false;
            }
            set
            {
            }

        }
        public Image ArrowBackImage
        {
            set
            {
                btn.BackgroundImage = value;
            }
        }
        public Color ArrowBackColor
        {
            get
            {
                return btn.BackColor;
            }
            set
            {
                btn.BackColor = value;
            }
        }
        #endregion

        #region ����
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            isTextChangeSetTag = true;
            try
            {
                base.Tag = ((Neusoft.FrameWork.Models.NeuObject)(alItems[this.SelectedIndex])).ID;
            }
            catch
            {
            }
            isTextChangeSetTag = false;
            //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
            //isTextChanged = false;
            base.OnSelectedIndexChanged( e );
        }
        bool isTextChangeSetTag = false;
        void NeuComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                e.Handled = true;
                this.ValidText();
            }
            else if (e.KeyCode == Keys.F2)
            {
                e.Handled = true;
                iSpellCode++;
                if (iSpellCode >= 4)
                    iSpellCode = 0;
                SpellCode = this.iSpellCode;
            }
            else if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                ShowSelectDialog();
                return;
            }
            //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
            //else
            //{
            //    isTextChanged = true;
            //}
            base.OnKeyDown( e );
        }

        //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
        /// <summary>
        /// text�Ƿ����仯
        /// </summary>
        /// <returns></returns>
        private bool isChangeText()
        {
            string name = string.Empty;
            if (this.Tag != null && !string.IsNullOrEmpty(this.Tag.ToString()))
            {
                string id = this.Tag.ToString();
                for (int i = 0; i < alItems.Count; i++)
                {
                    Neusoft.FrameWork.Models.NeuObject o = (Neusoft.FrameWork.Models.NeuObject)alItems[i];
                    if (o.ID == id)
                    {
                        name = o.Name;
                        break;
                    }
                }

            }
            if (!string.IsNullOrEmpty(name) && name == this.Text.Trim())
            {
                return false;
            }
            return true;
        }
        

        private void ValidText()
        {
            //{07252C73-80B1-42b6-A8FE-44AF3707CF54}
            if (base.Text.Trim() == "")
                return;
            //if (base.Text == " " || base.Text == "  " || base.Text == "   ")
            //{
            //    ShowSelectDialog();
            //    return;
            //}
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
                return;//���ֻ��ѡ��ģ�����Ҫ�ж�dd

            try
            {
                //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
                if (!isChangeText()) return;
                for (int i = 0; i < alItems.Count; i++)
                {
                    #region ����ƥ��
                    Neusoft.FrameWork.Models.NeuObject o = (Neusoft.FrameWork.Models.NeuObject)alItems[i];
                    try
                    {
                        if (o.Name.ToUpper().Trim() == base.Text.ToUpper().Trim())
                        {
                            showSelectText( o );
                            return;
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (o.ID.ToUpper().Trim() == base.Text.ToUpper().Trim())
                        {
                            showSelectText( o );
                            return;
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (o.Memo != null && o.Memo.ToUpper().Trim() == base.Text.ToUpper().Trim())
                        {
                            showSelectText( o );
                            return;
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        Neusoft.HISFC.Models.Base.ISpell Spell = o as Neusoft.HISFC.Models.Base.ISpell;
                        switch (iSpellCode)
                        {
                            case 0:
                                if ((Spell.SpellCode != null && Spell.SpellCode.ToUpper().Trim() == base.Text.ToUpper().Trim())
                                    || (Spell.WBCode != null && Spell.WBCode.ToUpper().Trim() == base.Text.ToUpper().Trim())
                                    || (Spell.UserCode != null && Spell.UserCode.ToUpper().Trim() == base.Text.ToUpper().Trim()))
                                {
                                    showSelectText( o );
                                    return;
                                }
                                break;
                            case 1:
                                if (Spell.SpellCode != null && Spell.SpellCode.ToUpper().Trim() == base.Text.ToUpper().Trim())
                                {
                                    showSelectText( o );
                                    return;
                                }
                                break;
                            case 2:
                                if (Spell.WBCode != null && Spell.WBCode.ToUpper().Trim() == base.Text.ToUpper().Trim())
                                {
                                    showSelectText( o );
                                    return;
                                }
                                break;
                            case 3:
                                if (Spell.UserCode != null && Spell.UserCode.ToUpper().Trim() == base.Text.ToUpper().Trim())
                                {
                                    showSelectText( o );
                                    return;
                                }
                                break;
                        }
                    }
                    catch
                    {
                    }
                    #endregion
                }

                if (this.isItemOnly) //û���ҵ� ��ֻ��ѡ��
                {
                    if (this.isLike)//ģ����ѯ
                    {
                        #region ����ƥ��
                        for (int i = 0; i < alItems.Count; i++)
                        {
                            Neusoft.FrameWork.Models.NeuObject o = (Neusoft.FrameWork.Models.NeuObject)alItems[i];
                            try
                            {
                                if (o.Name.ToUpper().Trim().IndexOf( base.Text.ToUpper().Trim() ) >= 0)
                                {
                                    showSelectText( o );
                                    return;
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (o.ID.ToUpper().Trim().IndexOf( base.Text.ToUpper().Trim() ) >= 0)
                                {
                                    showSelectText( o );
                                    return;
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                if (o.Memo != null && o.Memo != "")
                                {
                                    if (o.Memo != null && o.Memo.ToUpper().Trim().IndexOf( base.Text.ToUpper().Trim() ) >= 0)
                                    {
                                        showSelectText( o );
                                        return;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        #endregion
                    }
                    base.Text = "";//����
                }
            }
            catch
            {
                if (this.isItemOnly)
                    base.Text = "";
            }
        }
        private void showSelectText(Neusoft.FrameWork.Models.NeuObject o)
        {
            if (this.bShowID)
                base.Text = o.ID;
            else
                base.Text = o.Name;
            for (int i = 0; i < alItems.Count; i++)
            {
                try
                {
                    string sValue = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).ID.ToString();
                    if (o.ID == sValue)
                    {
                        this.SelectedIndex = i;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }
        /// <summary>
        /// ��ǰ������
        /// </summary>
        public int SpellCode
        {
            set
            {
                this.iSpellCode = value;
                QueryType = "ƴ��+���+�Զ���";
                switch (iSpellCode)
                {
                    case 0:
                        QueryType = "ƴ��+���+�Զ���";
                        this.BackColor = Color.FromArgb( 255, 255, 255 );
                        break;
                    case 1:
                        QueryType = "ƴ����";
                        this.BackColor = Color.FromArgb( 255, 225, 225 );
                        break;
                    case 2:
                        this.BackColor = Color.FromArgb( 255, 200, 200 );
                        QueryType = "�����";
                        break;
                    case 3:
                        this.BackColor = Color.FromArgb( 255, 150, 150 );
                        QueryType = "�Զ�����";
                        break;
                    default:
                        this.BackColor = Color.FromArgb( 255, 255, 255 );
                        break;
                }
                tooltip.SetToolTip( this, QueryType );
                //System.Windows.Forms.Cursor.Position = this.PointToScreen(new Point(this.Parent.Left,this.Parent.Top));
                tooltip.Active = true;
            }

        }
        protected ToolTip tooltip = new ToolTip();
        protected int iSpellCode = 0;
        protected string QueryType = "ƴ����";
        private void ComboBox_Leave(object sender, EventArgs e)
        {
            if (base.Text == "")
                return;
            //�ı��仯�������ж�
            //if (isTextChanged)
            //{B185DD6A-4CFE-469c-A7AB-6187C4C698EA}
            if(isChangeText())
                this.ValidText();


        }

        /// <summary>
        /// ��ʾ����ѡ����
        /// </summary>
        protected void ShowSelectDialog()
        {
            if (IsPopForm)
            {
                try
                {
                    frmPop.ShowDialog();
                }
                catch
                {
                    SetPopForm();
                }
                if (base.Text.Trim() == string.Empty)//{DCC02C4A-AB2F-4790-BFCD-AB360D748C83}
                {
                    base.Text = string.Empty;
                }
            }
        }
        protected void SetPopForm()
        {
            frmPop = new Neusoft.FrameWork.WinForms.Forms.frmEasyChoose( this.alItems );
            frmPop.Text = "��ѡ����Ŀ";
            frmPop.StartPosition = FormStartPosition.CenterScreen;
            frmPop.SelectedItem += new Neusoft.FrameWork.WinForms.Forms.SelectedItemHandler( frmPop_SelectedItem );
            frmPop.ShowDialog();
        }
        #endregion

        #region ��ʾ�����б�


        protected bool bShowOrHide = true;
        private void ShowSelectItem()
        {
            if (this.alItems == null || this.alItems.Count <= 0)
                return;


            return;
        }

        private void lst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetInfo();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.CloseForm();
            }
        }
        private void GetInfo()
        {


        }
        private void CloseForm()
        {

            try
            {
                if (frmPop.Visible)
                    this.frmPop.Hide();
            }
            catch
            {

            }
        }

        private void ComboBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.bShowCustomerList)
            {
                if (e.X > this.Width - 20)
                    ShowSelectItem();
            }
        }
        #endregion

        private void frmPop_SelectedItem(Neusoft.FrameWork.Models.NeuObject sender)
        {
            if (sender != null)
            {
                if (this.bShowID)
                {
                    base.Text = sender.ID;
                }
                else
                {
                    base.Text = sender.Name;
                    //{4491B5E5-775A-4c9f-A856-86B0767C9C42}
                    for (int i = 0; i < alItems.Count; i++)
                    {
                        try
                        {
                            string sValue = ((Neusoft.FrameWork.Models.NeuObject)alItems[i]).ID.ToString();
                            if (sender.ID == sValue)
                            {
                                this.SelectedIndex = i;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message;
                        }
                    }
                }
            }
        }

        private void poplst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GetInfo();
            }
            catch
            {
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {
            ShowSelectItem();
        }

        #region INeuControl ��Ա

        public StyleType Style
        {
            get
            {
                return StyleType.Flat;
            }
            set
            {

            }
        }

        #endregion
    }
}
