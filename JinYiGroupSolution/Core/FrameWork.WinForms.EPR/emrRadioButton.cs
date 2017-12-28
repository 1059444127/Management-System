using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
namespace Neusoft.FrameWork.EPRControl
{
    [System.Drawing.ToolboxBitmap(typeof(RadioButton))]
    public partial class emrRadioButton : RadioButton, IGroup
    {

        #region "��"

        private string ControlName;
        private string GroupName;
        private bool blnIsGroup;
        private System.EventArgs e;
        private bool bIsGroup;
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
                if (Module.ValidName(value) == false) return;

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
