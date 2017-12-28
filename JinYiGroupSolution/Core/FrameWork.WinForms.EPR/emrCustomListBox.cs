using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace Neusoft.FrameWork.EPRControl
{
    [System.Drawing.ToolboxBitmap(typeof(System.Windows.Forms.ListBox))]
    public partial class emrCustomListBox : System.Windows.Forms.ListBox, IGroup
    {
        //public emrCustomListBox()
        //{
        //    InitializeComponent();
        //}

        public emrCustomListBox(IContainer container)
        {
            container.Add(this);

            init();
           
        }

        #region IGroup ��Ա
        private string ControlName;
        private string GroupName;
        private bool blnIsGroup;
        private System.EventArgs e;
        private bool bIsGroup;
        private bool isPrint;
        public event NameChangedEventHandler NameChanged;
        public event IsGroupChangedEventHandler IsGroupChanged;
        public event GroupChangedEventHandler GroupChanged;

        [CategoryAttribute("���"), Browsable(true), DescriptionAttribute("���ÿؼ����ƣ�Ҳ�ǽ�����ƣ����ܰ���'�ո�\\,-,(,),,.%�������ַ�'")]
        public string ����
        {
            get
            {
                if (this.ControlName == "")
                {
                    this.ControlName = this.Name;
                }
                return this.ControlName;
            }
            set
            {
                if (Module.ValidName(value) == false)
                    return;

                ControlName = value.Trim();
                try
                {
                    if (NameChanged != null)
                    {
                        NameChanged(this, e);
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }
        [TypeConverter(typeof(emrGroup)), CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("ѡ��ؼ�������")]
        public string ��
        {
            get { return this.GroupName; }
            set
            {
                this.GroupName = value;
                try
                {
                    if (GroupChanged != null)
                    {
                        GroupChanged(this, e);
                    }
                }

                catch (Exception ex)
                {

                }
            }
        }
        [CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("�Ƿ��Ǹ����!"), Browsable(false)]
        public bool �Ƿ���
        {
            get { return this.bIsGroup; }
            set
            {
                this.bIsGroup = value;
                try
                {
                    if (IsGroupChanged != null)
                    {
                        IsGroupChanged(this, e);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        string snomed = "";
        public string Snomed
        {
            get
            {
                return snomed;
            }
            set
            {
                snomed = value;

            }
        }
        private bool bMust = false;
        public bool ����
        {
            get { return bMust; }
            set { bMust = value; }
        }
        [CategoryAttribute("���"), Browsable(true), DescriptionAttribute("�Ƿ��ӡ")]
        public bool IsPrint
        {
            get { return isPrint; }
            set
            {
                this.isPrint = value;
            }

        }
        #endregion

         #region  ˽�б���
       PopUpListBox listBox = new PopUpListBox();
        bool keyEnterVisiable = false;//����ؼ�ʱ�����б��ɼ�
        Neusoft.FrameWork.Models.NeuObject selectObj; //��ǰѡ�е���Ŀ
        private Neusoft.FrameWork.Public.ObjectHelper objHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        bool IsExist = false; //�Ƿ��Ѿ�����
        private System.Windows.Forms.Control parentControl; //�����ؼ�
        int LocaltionX; //λ��X
        int LocaltionY;//λ��Y
        bool isFind = false;//���������Ҳ�������ʱ�������
        private bool specalFlag = true; //����ѡ����Ŀ��ɼ����ɼ��Ĵ���
        #endregion

        #region ����
        [Description("����ؼ�ʱ�����б��ɼ�")]
        public bool EnterVisiable
        {
            get
            {
                return keyEnterVisiable;
            }
            set
            {
                keyEnterVisiable =  value;
            }
        }
        [Description("������Ŀ��")]
        public int ListBoxWidth
        {
            get
            {
                return listBox.Width;
            }
            set
            {
                listBox.Width = value;
            }
        }
        [Description("������Ŀ��")]
        public int ListBoxHeight
        {
            get
            {
                return listBox.Height;
            }
            set
            {
                listBox.Height = value;
            }
        }
        [Description("ģ����ѯ")]
        public bool OmitFilter
        {
            get
            {
                return listBox.OmitFilter;
            }
            set
            {
                listBox.OmitFilter = value;
            }
        }
        [Description("�Ƿ�Ĭ�ϲ�ѡ���κ���")]
        public bool SelectNone
        {
            get
            {
                return listBox.SelectNone;
            }
            set
            {
                listBox.SelectNone = value;
            }
        }
        [Description("���������Ҳ�������ʱ�������")]
        public bool IsFind
        {
            get
            {
                return isFind;
            }
            set
            {
                isFind = value;
            }
        }
        [Description("�Ƿ���ʾ�б�� ID")]
        public bool ShowID
        {
            get
            {
                return listBox.IsShowID;
            }
            set
            {
                listBox.IsShowID = value;
            }
        }
        /// <summary>
        /// ����Tag
        /// </summary>
        [Description("����Tag")]
        public new object Tag
        {
            get
            {
                return base.Tag;
            }
            set
            {
                base.Tag = value;
                this.Text = "";
                this.listBox.Visible = false;
                if (base.Tag != null)
                {
                    string str = objHelper.GetName(base.Tag.ToString());
                    if (!Neusoft.FrameWork.Public.String.StringEqual(this.Text, str))
                    {
                        this.Text = str;
                    }
                }
                this.listBox.Visible = false;
            }
        }
        /// <summary>
        /// ��ǰѡ�е���Ŀ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject SelectedItem
        {
            get
            {
                return selectObj;
            }
            set
            {
                selectObj = value; ;
            }
        }
        #endregion

        #region �����к���
        /// <summary>
        /// ɸѡ�¼�
        /// </summary>
        public emrCustomListBox()
        {
            init();
        }

        private void init()
        {
            listBox.Width = this.Width;
            listBox.Height = 100;
            listBox.SelectNone = true;
            //parentControl = this;
            this.TextChanged += new EventHandler(NeuListTextBox_TextChanged);
            this.Enter += new EventHandler(NeuListTextBox_Enter);
            this.KeyDown += new KeyEventHandler(NeuListTextBox_KeyDown);
            this.Leave += new EventHandler(NeuListTextBox_Leave);
            //Controls.Add(listBox);
            //����
            listBox.Hide();
            //���ñ߿�
            listBox.BorderStyle = BorderStyle.FixedSingle;
            //listBox.BringToFront();
            //�����¼�
            listBox.SelectItem += new PopUpListBox.MyDelegate(listBox_SelectItem); //new Neusoft.FrameWork.WinForms.Controls.PopUpListBox.MyDelegate(ICDListBox_SelectItem);
            this.EnterVisiable = true;
        }
        #region ������Ŀ
        /// <summary>
        /// ������Ŀ
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int AddItems(ArrayList list)
        {
            if (list == null)
            {
                return -1;
            }
            objHelper.ArrayObject = list;
            return listBox.AddItems(list);
        }
        #endregion 

        #region �������
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public void Reset()
        {
            this.Tag = null;
            this.Text = "";
        }
        #endregion 
        #endregion

        #region  ˽�к���

        #region �������� ���ػ�ɼ� ʱ
        void ParentForm_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                this.listBox.Visible = false;
            }
            catch { };
        }
        #endregion 
        #region �뿪�ؼ�ʱ
        void NeuListTextBox_Leave(object sender, EventArgs e)
        {
            if (!listBox.Focused)
            {
                specalFlag = true;
                this.listBox.Visible = false;
                if (isFind) // ����textֵУ������, �����������ر������������
                {
                    if (this.Text == null || this.Text == "")
                    {//û������ ֱ�����
                        this.Tag = null;
                        return;
                    }
                    else
                    {//������ ,���� 
                        string tagID = objHelper.GetID(this.Text);
                        if (tagID == null || tagID == "")
                        {//û���ҵ����
                            this.Tag = null;
                        }
                    }
                }
            }
            
        }
        #endregion 
        int listBox_SelectItem(Keys key)
        {
            //this.Focus();
            specalFlag = false;
            return GetSelectItem();
        }
        int GetSelectItem()
        {
            int rtn = listBox.GetSelectedItem(out selectObj);
            if (selectObj == null)
            {
                this.Reset();
                return -1;
            }
            if (selectObj.ID != "")
            {
                base.Tag = selectObj.ID;
                this.Text = selectObj.Name;
            }
            else
            {
                base.Tag = null;
                if (isFind)
                {
                    listBox.Text = "";
                }
            }
            //listBox.Focus(); //��ý���
            this.listBox.Visible = false;
            this.Focus();
            return rtn;
        }
        /// <summary>
        /// ����ؼ�ʱ �����б��Ƿ�ɼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NeuListTextBox_Enter(object sender, EventArgs e)
        { 
            AddControl();
            SetLocation();
            if (specalFlag)
            {
                this.listBox.Visible = this.EnterVisiable;
                this.listBox.Filter(this.Text.Trim()); 
            }
        }
        /// <summary>
        /// ɸѡ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NeuListTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Focused)
                {
                    SetLocation();
                    this.listBox.Visible = true;
                    //if (Text.Trim() != "")
                    //{
                    this.listBox.Filter(this.Text.Trim());
                    //}
                }
            }
            catch { }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NeuListTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                listBox.PriorRow();
            }
            else if (e.KeyData == Keys.Down)
            {
                listBox.NextRow();
            }
        }
        /// <summary>
        /// ���ؿؼ�
        /// </summary>
        void AddControl()
        {
            if (!IsExist)
            {
                this.FindForm().VisibleChanged += new EventHandler(ParentForm_VisibleChanged);
                this.FindForm().SizeChanged += new EventHandler(ParentForm_SizeChanged);
            }
                LocaltionX = 0;
                LocaltionY = 0;
                parentControl = GetParent(this);
                parentControl.Controls.Add(listBox);
                IsExist = true;
                Form f = this.FindForm().ParentForm;
                if (f != null)
                {
                    if (f.IsMdiContainer)
                    {
                        this.LocaltionX += System.Math.Abs(f.Location.X);
                        this.LocaltionY += 32; //�˵��Ŀ��
                    }
                }
                if (listBox.Width < this.Width)
                {
                    listBox.Width = this.Width;
                }
                if (this.parentControl.Width < LocaltionX + listBox.Width)
                {
                    //if ((parentControl.Width - listBox.Width - LocaltionX) > 0)
                    //{
                    //    LocaltionX = LocaltionX - (parentControl.Width - listBox.Width - LocaltionX);
                    //}
                    //else
                    //{
                        LocaltionX = LocaltionX - System.Math.Abs(listBox.Width + LocaltionX - parentControl.Width);
                    //}
                }

                if (this.parentControl.Height < LocaltionY + listBox.Height + this.Height)
                {
                    if (parentControl.Height - listBox.Height - this.Height > 0)
                    {
                        LocaltionY = LocaltionY  - listBox.Height - this.Height - 2;
                    }
                }
            //}
            listBox.BringToFront();
        }

        void ParentForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.listBox.Visible = false;
            }
            catch { };
        }
        /// <summary>
        /// ��ȡ�����ؼ�
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private Control GetParent(Control control)
        {
            try
            {
                if (control.Parent != null)
                {
                    LocaltionX += control.Location.X;
                    LocaltionY += control.Location.Y;
                    return GetParent(control.Parent);
                }
                else
                {
                    return control;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return control;
            }
        }
        /// <summary>
        /// ����λ��
        /// </summary>
        void SetLocation()
        {
            listBox.Location = new System.Drawing.Point(LocaltionX + 2, LocaltionY + Height + 2);
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (listBox.Visible)
                {
                    GetSelectItem();
                }
            }
            if (keyData == Keys.Escape)
            {
                listBox.Visible = false;
            }

            return base.ProcessDialogKey(keyData);
        }
        #endregion
    }
}
