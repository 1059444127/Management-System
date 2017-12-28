using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.EPRControl
{
    
    [System.Drawing.ToolboxBitmap(typeof(TextBox))]
    public partial class emrTextBox : TextBox, IGroup, IUserControlable,StructInput.IStructable
    {

	    #region "��"

	    private string ControlName;
	    private string GroupName;
	    private bool blnIsGroup;
        private bool isPrint;
	    private System.EventArgs e;

      
	    [CategoryAttribute("���"), Browsable(true), DescriptionAttribute("���ÿؼ����ƣ�Ҳ�ǽ�����ƣ����ܰ���'�ո�\\,-,(,),,.%�������ַ�'")]
	    public string ���� {
		    get {
			    if (this.ControlName == "")
			    {
				    this.ControlName = this.Name;
			    }
			    return this.ControlName;
		    }
		    set {
                if (Module.ValidName(value) == false) return;

			    ControlName = value.Trim();
			    try {
				    if (NameChanged != null) {
					    NameChanged(this, e);
				    }
			        }
			        catch (Exception ex) {

			        }

		    }
	    }

	    [TypeConverter(typeof(emrGroup)), CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("ѡ��ؼ�������")]
	    public string �� {
		    get { return this.GroupName; }
		    set {
			    this.GroupName = value;
			    try {
				    if (GroupChanged != null) {
					    GroupChanged(this, e);
				    }
			        }

			        catch (Exception ex) {

			        }
		    }
	    }
	    private bool bIsGroup;
	  
	    [CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("�Ƿ��Ǹ����!"), Browsable(false)]
	    public bool �Ƿ��� {
		    get { return this.bIsGroup; }
		    set {
			    this.bIsGroup = value;
			    try {
				    if (IsGroupChanged != null) {
					    IsGroupChanged(this, e);
				    }
			    }
			    catch (Exception ex) {

			    }
		    }
	    }
        public event NameChangedEventHandler NameChanged;
        public event IsGroupChangedEventHandler IsGroupChanged;
        public event GroupChangedEventHandler GroupChanged; 
	    #endregion

        #region Snomed ��Ա

        string snomed = "";
        [CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("Snomed����")]
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

        #endregion

	    [Browsable(true)]
	    public new string Text {
		    get { return base.Text; }
		    set { base.Text = value; }
	    }

	    private bool bMust = false;
	    public bool ���� {
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

	  

	    public int Valid(object sender)
	    {

		    if (Module.ValidText(this.Text) == false & bMust)
		    {
			    MessageBox.Show(this.���� + "û����д!");
			    this.Focus();
			    return -1;
		    }
		    return 0;
	    }

        #region IUserControlable ��Ա

        public void Init(object sender, string[] @params)
        {
            return;
        }

        public int Save(object sender)
        {
            return 0;
        }

        public void RefreshUC(object sender, string[] @params)
        {
            return;
        }

        #endregion

        #region IUserControlable ��Ա


        public Control FocusedControl
        {
            get { return this; }
        }

        #endregion

        #region IStructable ��Ա

        private string searchTable;
        [TypeConverter(typeof(StructInput.SearchTableConvert)), Category("���"), Description("���ҵķ����")]
        public string SearchTable
        {
            get
            {
                return this.searchTable;
            }
            set
            {
                this.searchTable = value;
            }
        }

        private StructInput.enumSearchType searchType;
        [TypeConverter(typeof(StructInput.SearchTypeConvert)), Category("���"), Description("���ҷ�ʽ�������������롢Ӣ������ƴ�������"), DefaultValue(StructInput.enumSearchType.CNOMEN)]
        public StructInput.enumSearchType SearchType
        {
            get
            {
                return this.searchType;
            }
            set
            {
                this.searchType = value;
            }
        }

        private bool isExactSearch;
        [Category("���"), Description("�Ƿ�ȷ��ѯ")]
        public bool IsExactSearch
        {
            get
            {
                return this.isExactSearch;
            }
            set
            {
                this.isExactSearch = value;
            }
        }

        public int SelectionIndex
        {
            get
            {
                return this.SelectionStart;
            }
        }

        private int keyWordIndex;
        public int KeyWordIndex
        {
            get
            {
                return this.keyWordIndex;
            }
            set
            {
                this.keyWordIndex = value;
            }
        }

        public string SelectText
        {
            get
            {
                return base.SelectedText;
            }
            set
            {
                base.SelectedText = value;
            }
        }

        public Point GetPositionFromIndex(int index)
        {
            return this.GetPositionFromCharIndex(index);
        }

        public void SelectKeyWord(int start, int length)
        {
            base.Select(start, length);
        }
        #endregion
    }
}