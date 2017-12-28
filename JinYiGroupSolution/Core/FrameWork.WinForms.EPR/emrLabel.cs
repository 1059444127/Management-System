using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Neusoft.FrameWork.EPRControl
{
   

    [System.Drawing.ToolboxBitmap(typeof(Label))]
    public partial class emrLabel : Label, IGroup
    {
	  

	    #region "��"

	    private string ControlName;
	    private string NodeName;
	    private string GroupName = "��";
	    private bool blnIsGroup;
	    private System.EventArgs e;
        private bool bIsGroup;

       
        public event NameChangedEventHandler NameChanged;
        public event IsGroupChangedEventHandler IsGroupChanged;
        public event GroupChangedEventHandler GroupChanged;   

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
			    if (CheckValue(value, 0) == false) return; 
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
	    public string ��
        {
		    get { return this.GroupName; }
		    set 
                {
			        if (CheckValue(value, 1) == false) return; 

			        this.GroupName = value.Trim();
			        try {
				        if (GroupChanged != null) {
					        GroupChanged(this, e);
				        }
			        }

			        catch (Exception ex) {

			        }
		        }
	    }
	  
	    [CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("�Ƿ��Ǹ����!"), Browsable(true)]
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
	    private bool CheckValue(string value, Int16 i)
	    {
		    bool right = true;
            if (value == null || value == "") return false;
		    if (value.Trim() == "") right = false; 
		    if (value.IndexOf("\\") >= 0) right = false; 
		    if (value.IndexOf("/") >= 0) right = false; 
		    if (value.IndexOf(">") >= 0) right = false; 
		    if (value.IndexOf("<") >= 0) right = false; 
		    if (value.IndexOf("=") >= 0) right = false; 
		    if (value.IndexOf(".") >= 0) right = false; 
		    if (value.IndexOf(",") >= 0) right = false; 
		    if (value.IndexOf("%") >= 0) right = false; 
		    if (i == 0)
		    {
			    //����
			    if ((value == this.GroupName))
			    {
				    MessageBox.Show("���ƺ��鲻��ͬ����","��ʾ",MessageBoxButtons.OK);
				    return false;
			    }
		    }
		    else
		    {
			    if (value == this.ControlName)
			    {
                    MessageBox.Show("���ƺ��鲻��ͬ����", "��ʾ", MessageBoxButtons.OK);
				    return false;
			    }
		    }
		    if ((!right))
		    {
                MessageBox.Show("���ܰ����Ƿ��ַ���", "��ʾ", MessageBoxButtons.OK);
		    }
		    return right;
	    }
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
    }
}
