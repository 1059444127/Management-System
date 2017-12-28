using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.FrameWork.EPRControl
{
    /// <summary>
    /// ���Ӳ������������ؼ���������Ӳ������ݣ�����չ������ӡ
    /// </summary>
    public partial class emrSubPanel : Panel, Neusoft.FrameWork.EPRControl.IControlPrintable, Neusoft.FrameWork.EPRControl.IGroup
    {
        private ContainerPrintType printType;
        private bool isCanInsert;
        private bool isCanDelete;
        private string title;
        private string groupName;
        private bool isShowInTreeView;

        /// <summary>
        /// �Ƿ���ʾ��TreeView��
        /// </summary>
        public bool IsShowInTreeView
        {
            get { return isShowInTreeView; }
            set { isShowInTreeView = value; }
        }
        
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        [Description("�Ƿ�����������")]
        public bool IsCanInsert
        {
            get { return isCanInsert; }
            set { isCanInsert = value; }
        }

        /// <summary>
        /// �Ƿ����ɾ������
        /// </summary>
        [Description("�Ƿ����ɾ������")]
        public bool IsCanDelete
        {
            get { return isCanDelete; }
            set { isCanDelete = value; }
        }

        /// <summary>
        /// ���ݴ�ӡ��ʽ
        /// </summary>
        [Description("��ӡ��ʽ��������ӡ����ӡ���ݲ����У�˳�������չ�����ڶ����Ŀ�нڵ����룬�ı���ʽ�������չ��ӡ����ӡ���ݷ��У�ÿ����������֮��˳��������չ����һ�������ӡ��������������ı����ݡ�������ӡ������ʲô��ʽ���ʹ�ʲô�������ڱ��ʽ��ӡ��ͼƬ��ӡ��ʽ������Ҫ��Ĵ�ӡ��")]
        public ContainerPrintType PrintType
        {
            get { return printType; }
            set { printType = value; }
        }

        public emrSubPanel()
        {
            InitializeComponent();
        }

        #region IControlPrintable

        public ArrayList arrSortedControl()
        {
            return null;
        }
        public void continuePrint(PrintPageEventArgs e, Rectangle rectangle, Graphics grap)
        {
        }

        public void Print(PrintPageEventArgs e, Rectangle rectangle, Graphics grap)
        {
        }

        public Control PrintControl()
        {
            return null;
        }

        public void SetText(string fileID)
        { }
        #endregion

        #region IGroup ��Ա

        public event Neusoft.FrameWork.EPRControl.GroupChangedEventHandler GroupChanged;

        public event Neusoft.FrameWork.EPRControl.IsGroupChangedEventHandler IsGroupChanged;

        public event Neusoft.FrameWork.EPRControl.NameChangedEventHandler NameChanged;

        public string Snomed
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public string ����
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool �Ƿ���
        {
            get
            {
                return true;
            }
            set
            { 
            }
        }

        public string ��
        {
            get
            {
                return this.groupName;
            }
            set
            {
                this.groupName = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// ���������ؼ���ӡ��ʽ
    /// </summary>
    public enum ContainerPrintType
    {
        ������ӡ = 0, //��ӡ���ݲ����У�˳�������չ�����ڶ����Ŀ�нڵ����룬�ı���ʽ�����
        ��չ��ӡ = 1, //��ӡ���ݷ��У�ÿ����������֮��˳��������չ����һ�������ӡ��������������ı����ݡ�
        ������ӡ = 2    //����ʲô��ʽ���ʹ�ʲô�������ڱ��ʽ��ӡ��ͼƬ��ӡ��ʽ������Ҫ��Ĵ�ӡ��
    }

}
