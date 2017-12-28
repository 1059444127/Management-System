using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Manager.Notice
{
	/// <summary>
	/// ucNotice ��ժҪ˵����
	/// </summary>
	public class ucNotice : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.RichTextBox rtbNotice;
		private System.Windows.Forms.Panel panelTop;
		private Neusoft.FrameWork.WinForms.Controls.NeuComboBox cmbDept;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelBottom;
		private System.Windows.Forms.Label label3;
        private Neusoft.FrameWork.WinForms.Controls.NeuComboBox cmbNoticeDept;
		private System.Windows.Forms.Label label4;
        private Neusoft.FrameWork.WinForms.Controls.NeuComboBox cmbSysGroup;
		private System.Windows.Forms.CheckBox ckDeptAll;
		private System.Windows.Forms.CheckBox ckGroupAll;
		private System.Windows.Forms.DateTimePicker dtNoticeDate;
		private System.ComponentModel.IContainer components;

		public ucNotice()
		{
			// �õ����� Windows.Forms ���������������ġ�
			InitializeComponent();

			// TODO: �� InitializeComponent ���ú�����κγ�ʼ��
			
			this.Load += new EventHandler(ucNotice_Load);
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
            this.components = new System.ComponentModel.Container();
            this.rtbNotice = new System.Windows.Forms.RichTextBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.ckDeptAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDept = new Neusoft.FrameWork.WinForms.Controls.NeuComboBox(this.components);
            this.cmbSysGroup = new Neusoft.FrameWork.WinForms.Controls.NeuComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.ckGroupAll = new System.Windows.Forms.CheckBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.dtNoticeDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbNoticeDept = new Neusoft.FrameWork.WinForms.Controls.NeuComboBox(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbNotice
            // 
            this.rtbNotice.AcceptsTab = true;
            this.rtbNotice.BackColor = System.Drawing.Color.White;
            this.rtbNotice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNotice.Location = new System.Drawing.Point(0, 32);
            this.rtbNotice.Name = "rtbNotice";
            this.rtbNotice.Size = new System.Drawing.Size(635, 323);
            this.rtbNotice.TabIndex = 0;
            this.rtbNotice.Text = "";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.ckDeptAll);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.cmbDept);
            this.panelTop.Controls.Add(this.cmbSysGroup);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.ckGroupAll);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(635, 32);
            this.panelTop.TabIndex = 1;
            // 
            // ckDeptAll
            // 
            this.ckDeptAll.Location = new System.Drawing.Point(211, 6);
            this.ckDeptAll.Name = "ckDeptAll";
            this.ckDeptAll.Size = new System.Drawing.Size(74, 22);
            this.ckDeptAll.TabIndex = 2;
            this.ckDeptAll.Text = "ȫ������";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "���տ��ң�";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDept
            // 
            this.cmbDept.ArrowBackColor = System.Drawing.Color.Silver;
            this.cmbDept.IsFlat = false;
            this.cmbDept.IsLike = true;
            this.cmbDept.Location = new System.Drawing.Point(72, 6);
            this.cmbDept.Name = "cmbDept";
            this.cmbDept.PopForm = null;
            this.cmbDept.ShowCustomerList = false;
            this.cmbDept.ShowID = false;
            this.cmbDept.Size = new System.Drawing.Size(126, 20);
            this.cmbDept.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.cmbDept.TabIndex = 0;
            this.cmbDept.Tag = "";
            this.cmbDept.ToolBarUse = false;
            // 
            // cmbSysGroup
            // 
            this.cmbSysGroup.ArrowBackColor = System.Drawing.Color.Silver;
            this.cmbSysGroup.IsFlat = false;
            this.cmbSysGroup.IsLike = true;
            this.cmbSysGroup.Location = new System.Drawing.Point(405, 6);
            this.cmbSysGroup.Name = "cmbSysGroup";
            this.cmbSysGroup.PopForm = null;
            this.cmbSysGroup.ShowCustomerList = false;
            this.cmbSysGroup.ShowID = false;
            this.cmbSysGroup.Size = new System.Drawing.Size(137, 20);
            this.cmbSysGroup.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.cmbSysGroup.TabIndex = 0;
            this.cmbSysGroup.Tag = "";
            this.cmbSysGroup.ToolBarUse = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(322, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "���չ����飺";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ckGroupAll
            // 
            this.ckGroupAll.Location = new System.Drawing.Point(549, 5);
            this.ckGroupAll.Name = "ckGroupAll";
            this.ckGroupAll.Size = new System.Drawing.Size(85, 22);
            this.ckGroupAll.TabIndex = 2;
            this.ckGroupAll.Text = "ȫ��������";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.dtNoticeDate);
            this.panelBottom.Controls.Add(this.label3);
            this.panelBottom.Controls.Add(this.cmbNoticeDept);
            this.panelBottom.Controls.Add(this.label4);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 355);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(635, 33);
            this.panelBottom.TabIndex = 2;
            // 
            // dtNoticeDate
            // 
            this.dtNoticeDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtNoticeDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtNoticeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtNoticeDate.Location = new System.Drawing.Point(405, 4);
            this.dtNoticeDate.Name = "dtNoticeDate";
            this.dtNoticeDate.Size = new System.Drawing.Size(137, 21);
            this.dtNoticeDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(6, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 22);
            this.label3.TabIndex = 3;
            this.label3.Text = "�������ң�";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbNoticeDept
            // 
            this.cmbNoticeDept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbNoticeDept.ArrowBackColor = System.Drawing.Color.Silver;
            this.cmbNoticeDept.IsFlat = false;
            this.cmbNoticeDept.IsLike = true;
            this.cmbNoticeDept.Location = new System.Drawing.Point(72, 4);
            this.cmbNoticeDept.Name = "cmbNoticeDept";
            this.cmbNoticeDept.PopForm = null;
            this.cmbNoticeDept.ShowCustomerList = false;
            this.cmbNoticeDept.ShowID = false;
            this.cmbNoticeDept.Size = new System.Drawing.Size(126, 20);
            this.cmbNoticeDept.Style = Neusoft.FrameWork.WinForms.Controls.StyleType.Fixed3D;
            this.cmbNoticeDept.TabIndex = 2;
            this.cmbNoticeDept.Tag = "";
            this.cmbNoticeDept.ToolBarUse = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(322, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "�������ڣ�";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucNotice
            // 
            this.Controls.Add(this.rtbNotice);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "ucNotice";
            this.Size = new System.Drawing.Size(635, 388);
            this.panelTop.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// ������Ϣ������
		/// </summary>
        Neusoft.HISFC.BizLogic.Manager.Notice noticeManager = new Neusoft.HISFC.BizLogic.Manager.Notice();

		/// <summary>
		/// ������Ϣʵ��
		/// </summary>
        private Neusoft.HISFC.Models.Base.Notice notice = null;
		/// <summary>
		/// ���β����ķ�����Ϣ
		/// </summary>
        public Neusoft.HISFC.Models.Base.Notice Notice
		{
			get
			{
				if (this.notice == null)
					this.notice = new Neusoft.HISFC.Models.Base.Notice();
				return this.notice;
			}
			set
			{
				if (value == null)
					value = new Neusoft.HISFC.Models.Base.Notice();
				this.notice = value;
				this.ShowNotice(this.notice);
			}
		}

		/// <summary>
		/// ����Ա��Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.Base.Employee oper = null;
		/// <summary>
		/// ����Ա��Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.Base.Employee Oper
		{
			set
			{
				this.oper = value;
			}
		}

		/// <summary>
		/// ֻ������ʾ
		/// </summary>
		public bool OnlyShow
		{
			set
			{
				this.panelTop.Visible = !value;
				this.panelBottom.Visible = !value;
				this.rtbNotice.ReadOnly = value;
			}
		}


		/// <summary>
		/// ���ݳ�ʼ��
		/// </summary>
		protected void Init()
		{
			#region ���ؿ���
			Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
			ArrayList alDept = deptManager.GetDeptmentAll();
			if (alDept == null)
			{
				MessageBox.Show("��ȡ�����б����" + deptManager.Err);
				return;
			}
			this.cmbDept.AddItems(alDept);
			this.cmbNoticeDept.AddItems(alDept);
			#endregion

			#region ���ع�����
			Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            //ArrayList alSysGroup = sysGroupManager.GetList();
            //if (alSysGroup == null)
            //{
            //    MessageBox.Show("��ȡ�������б����" + sysGroupManager.Err);
            //    return;
            //}
            //this.cmbSysGroup.AddItems(alSysGroup);
			#endregion
		}

		/// <summary>
		/// �����ʾ
		/// </summary>
		public void Clear()
		{
			this.cmbDept.Tag = null;
			this.cmbDept.Text = "";
			this.ckDeptAll.Checked = false;
			this.cmbSysGroup.Tag = null;
			this.cmbSysGroup.Text = "";
			this.ckGroupAll.Checked = false;
			this.rtbNotice.Text = "";
			if (this.oper != null)
			{
				this.cmbNoticeDept.Tag = this.oper.Dept.ID;
				this.cmbNoticeDept.Text = this.oper.Dept.Name;
			}
		}
		/// <summary>
		/// ������ʾ������Ϣ
		/// </summary>
		/// <param name="notice">������Ϣʵ��</param>
		protected void ShowNotice(Neusoft.HISFC.Models.Base.Notice notice)
		{
			if (notice == null)
				return;
			if (notice.ID == "")
				this.Clear();

			//��ʹ��Rtf���� �������ַ������ȹ��� 
			this.rtbNotice.Text = notice.NoticeInfo;
			if (notice.Dept.ID == "AAAA")						//���տ���
			{
				this.ckDeptAll.Checked = true;
				this.cmbDept.Tag = null;
				this.cmbDept.Text = "";
			}
			else
			{
				if (notice.Dept.ID != "")
					this.cmbDept.Tag = notice.Dept.ID;
			}
			if (notice.Group.ID == "AAAA")						//���չ�����
			{
				this.ckGroupAll.Checked = true;
				this.cmbSysGroup.Tag = null;
				this.cmbSysGroup.Text = "";
			}
			else
			{
				if (notice.Group.ID != "")
					this.cmbSysGroup.Tag = notice.Group.ID;
			}
			this.cmbNoticeDept.Tag = notice.NoticeDept.ID;		//��������
			if (notice.NoticeDate != DateTime.MinValue)
                this.dtNoticeDate.Value = notice.NoticeDate;		//��������
		}

		protected int ValidSave()
		{
			if (!this.ckDeptAll.Checked && this.cmbDept.Text == "")
			{
				MessageBox.Show("����д���տ���");
				return -1;
			}
			if (!this.ckGroupAll.Checked && this.cmbSysGroup.Text == "")
			{
				MessageBox.Show("����д���չ�����");
				return -1;
			}
			if (this.rtbNotice.Text == "")
			{
				MessageBox.Show("����д������Ϣ");
				return -1;
			}
			return 1;
		}
		/// <summary>
		/// ���淢����Ϣ
		/// </summary>
		/// <returns>�ɹ�����1 ʧ�ܷ��أ�1</returns>
		public int SaveNotice(string noticeTitle)
		{
			if (this.ValidSave() == -1)
				return -1;

			if (this.notice == null)
				this.notice = new Neusoft.HISFC.Models.Base.Notice();

			//��ʹ��Rtf���� �������ַ������ȹ��� 
			//��ʹ��Rtf���� �����ҵ��������ֵ��ʽ ��ʹ��string.format ֱ��ʹ��execnoquery(sql,parms..)
			this.notice.NoticeInfo = this.rtbNotice.Text;
			if (this.ckDeptAll.Checked)
				this.notice.Dept.ID = "AAAA";
			else
				this.notice.Dept.ID = this.cmbDept.Tag.ToString();
			if (this.ckGroupAll.Checked)
				this.notice.Group.ID = "AAAA";
			else
				this.notice.Group.ID = this.cmbSysGroup.Tag.ToString();
			this.notice.NoticeDept.ID = this.cmbNoticeDept.Tag.ToString();
			this.notice.NoticeDept.Name = this.cmbNoticeDept.Text;
			this.notice.NoticeDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtNoticeDate.Text);
			this.notice.NoticeTitle = noticeTitle;
			
			if (noticeManager.SetNotice(this.notice) != 1)
			{
				MessageBox.Show("���淢����Ϣʧ��" + noticeManager.Err);
				return -1;
			}

			MessageBox.Show("����ɹ�");
			return 1;
		}

		/// <summary>
		/// ɾ��������Ϣ
		/// </summary>
		/// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
		public int DelNotice()
		{	
			if (this.notice != null)
			{
				if (MessageBox.Show("ȷ�Ͻ���ɾ��������?","",System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					if (this.noticeManager.DeleteNotice(this.notice.ID) != 1)
					{
						MessageBox.Show("ɾ��������Ϣʧ��" + noticeManager.Err);
						return -1;
					}
					MessageBox.Show("ɾ���ɹ�");
					return 1;
				}
				else
				{
					return -1;
				}
			}
			return 0;
		}

		/// <summary>
		/// ��ӡ
		/// </summary>
		public void Print()
		{
			if (this.rtbNotice.Text != "")
			{
				Neusoft.FrameWork.WinForms.Classes.Print p=new Neusoft.FrameWork.WinForms.Classes.Print();
				Control c = this;
				this.OnlyShow = true;
				p.PrintPreview(50,30,c);
				this.OnlyShow = false;
			}
		}

		private void ucNotice_Load(object sender, EventArgs e)
		{
            try
            {
                this.Init();
            }
            catch { }
		}


	}
}
