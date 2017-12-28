using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Neusoft.FrameWork.EPRControl
{
 
  [System.Drawing.ToolboxBitmap(typeof(RichTextBox))]
   public  partial class emrMultiLineTextBox1 : RichTextBox, IGroup
  {
        /// <summary>
        /// Struct Function
        /// </summary>
        public emrMultiLineTextBox1()
        {
            base.HideSelection = false;
            base.Font = new System.Drawing.Font("����", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            try
            {
                mnuCut.Click += onCut;
                mnuCopy.Click += onCopy;
                mnuPaste.Click += onPaste;
                mnuUndo.Click += onUndo;
                mnuRedo.Click += onRedo;
                cMenu.MenuItems.Add(mnuUndo);
                cMenu.MenuItems.Add(mnuRedo);
                cMenu.MenuItems.Add(mnuSplit);
                cMenu.MenuItems.Add(mnuCut);
                cMenu.MenuItems.Add(mnuCopy);
                cMenu.MenuItems.Add(mnuPaste);
               // this.ContextMenu = cMenu;
            }
            catch (Exception ex) { }
        }

    #region "��"

    private string ControlName;
    /// <summary>
    /// ����
    /// </summary>
    private string GroupName = "��";
    private bool blnIsGroup;
    /// <summary>
    /// �Ƿ����
    /// </summary>
    private bool bIsGroup;
    private System.EventArgs e;
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
        if (CheckValue(value, 0) == false) return; 
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
    [TypeConverter(typeof(emrGroup)),Browsable(true), CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("ѡ��ؼ�������")]
    public string �� 
    {
    get { return this.GroupName; }
    set 
      {
        if (CheckValue(value, 1) == false) return; 
            this.GroupName = value.Trim();
        try {
            if (GroupChanged != null) 
             {
                GroupChanged(this, e);
             }
            }

        catch (Exception ex) {}
      }
    }
 
    [CategoryAttribute("���"),Browsable(false), DefaultValueAttribute(""), DescriptionAttribute("�Ƿ��Ǹ����!")]
    public bool �Ƿ��� {
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
            catch (Exception ex) { }
        }
    }
    private bool CheckValue(string value, int i)
    {
        bool right = true;
        if (value == null || value == "") return false;
        if ((value.Trim() == "")) right = false; 
        if ((value.IndexOf("\\") >= 0)) right = false; 
        if ((value.IndexOf("/") >= 0)) right = false; 
        if ((value.IndexOf(">") >= 0)) right = false; 
        if ((value.IndexOf("<") >= 0)) right = false; 
        if ((value.IndexOf("=") >= 0)) right = false; 
        if ((value.IndexOf(".") >= 0)) right = false; 
        if ((value.IndexOf(",") >= 0)) right = false; 
        if ((value.IndexOf("%") >= 0)) right = false; 
        if (i == 0)
        {
            //����
            if ((value == this.GroupName))
            {
                MessageBox.Show("���ƺ��鲻��ͬ����");
                return false;
            }
        }
        else
        {
            if (value == this.ControlName)
            {
                MessageBox.Show("���ƺ��鲻��ͬ����");
                return false;
            }
        }
        if (!right)
            {
            MessageBox.Show("���ܰ����Ƿ��ַ���");
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
    private ContextMenu cMenu = new ContextMenu();
    MenuItem mnuUndo = new MenuItem("����");
    MenuItem mnuRedo = new MenuItem("�ָ�");
    MenuItem mnuSplit = new MenuItem("-");
    MenuItem mnuCut = new MenuItem("����(&X)");
    MenuItem mnuCopy = new MenuItem("����(&C)");
    MenuItem mnuPaste = new MenuItem("ճ��(&P)");

    protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
    {

        if ((e.Button == MouseButtons.Right))
        {
            if (this.SelectedText == "")
            {
                mnuCut.Enabled = false;
            }
            else
            {
                mnuCut.Enabled = true;
            }
       

            if (this.CanUndo)
            {
                mnuUndo.Enabled = true;
            }
            else
            {
                mnuUndo.Enabled = false;
            }
            if (this.CanRedo)
            {
                mnuRedo.Enabled = true;
            }
            else
            {
                mnuRedo.Enabled = false;
            }
        }
            base.OnMouseDown(e);
    }
    private void onUndo(object sender, System.EventArgs e)
    {
        this.Undo();
    }
    private void onRedo(object sender, System.EventArgs e)
    {
        this.Redo();
    }
    private void onCut(object sender, System.EventArgs e)
    {
        this.Cut();
    }
    private void onCopy(object sender, System.EventArgs e)
    {
        this.Copy();
    }
    private void onPaste(object sender, System.EventArgs e)
    {
        this.Paste();
    }
    protected override void OnSizeChanged(System.EventArgs e)
    {
        if ((base.Height < 40))
        {
        base.Multiline = false;
        }
        else
        {
        base.Multiline = true;
        }
    }
       protected override void OnMouseUp(MouseEventArgs mevent)
       {
           if (mevent.Button == MouseButtons.Right)
           {
               base.OnMouseUp(mevent);
               
           }
       }
       protected override void OnMouseMove(MouseEventArgs e)
       {
           if (e.Button == MouseButtons.Right)
           {
               base.OnMouseMove(e);               
           }
       }
  }

}
