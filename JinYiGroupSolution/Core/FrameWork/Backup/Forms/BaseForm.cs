using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Neusoft.FrameWork.WinForms.Forms
{

	
    /// <summary>
    /// [��������: 	
    /// ���ര�� created by wolf 2004-6-21
    /// ��ӿ��ƴ��ڿؼ�����
    /// ʵ��Ctrl+Alt+W����������ù���]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
	public class BaseForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// 
		/// </summary>
		public BaseForm()
		{

			//
			// Windows ���������֧���������
			//
			InitializeComponent();
			this.AutoScaleMode = AutoScaleMode.None ;
            this.BackColor = Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.Blue);
		}


		~BaseForm()
		{
			GC.Collect();
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

			//���ڹر�ʱ,ǿ�ƽ��������ռ�.cuipeng test this function 2005-4-30
			GC.Collect();
		}
		
		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // BaseForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "BaseForm";
            this.Text = "BaseForm";
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.Closed += new System.EventHandler(this.BaseForm_Closed);
            this.ResumeLayout(false);

		}
		#endregion
      

        //*************������ȫ�ֱ������ݹ�����Ҫ���õ�*****************
		/// <summary>
		/// ���ÿؼ�
		/// </summary>
		protected Neusoft.FrameWork.WinForms.Controls.DesignControl DisignControl;
		private void BaseForm_Load(object sender, System.EventArgs e)
		{
            this.iniForm();
            this.iniControlText(this);
			this.KeyPreview = true;
			this.KeyDown+=new KeyEventHandler(BaseForm_KeyDown);
		}

        /// <summary>
        /// ����Text
        /// </summary>
        /// <param name="control"></param>
        public void iniControlText(object control)
        {
            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD}  ������ʵ�ַ�ʽ��� ����.Net ���ػ�ƽ̨�������������Զ�ת����ʽ�Ա�֤����չ��Ч��
            return;

            if (Neusoft.FrameWork.Management.Language.IsUseLanguage == false)
            {
                return;
            }

            this.ReplaceText(control);
            Control c = control as Control;
            if (c != null && c.Controls.Count>0)
            {
                foreach (Control c1 in c.Controls)
                {
                    this.iniControlText(c1);
                }
            }
        }

        /// <summary>
        /// �滻
        /// </summary>
        protected void ReplaceText(object c)
        {
            try
            {
                if (c.GetType().IsSubclassOf(typeof(TabPage)) 
                    || c.GetType().IsSubclassOf(typeof(Label))
                    || c.GetType().IsSubclassOf(typeof(ButtonBase))
                    || c.GetType() == typeof(TabPage)
                    || c.GetType() == typeof(Label))
                {
                    Control control = c as Control;
                    control.Text = Neusoft.FrameWork.Management.Language.Msg(control.Text);

                }
                else if (c.GetType().IsSubclassOf(typeof(ToolBar)) || c.GetType() == typeof(ToolBar))
                {
                    ToolBar tb = c as ToolBar;
                    foreach (ToolBarButton button in tb.Controls)
                    {
                        button.Text = Neusoft.FrameWork.Management.Language.Msg(button.Text);
                        button.ToolTipText = Neusoft.FrameWork.Management.Language.Msg(button.ToolTipText);
                    }
                }
                else if (c.GetType().IsSubclassOf(typeof(ToolStrip)) || c.GetType() == typeof(ToolStrip))
                {
                    ToolStrip ts = c as ToolStrip;
                    foreach (ToolStripItem button in ts.Items)
                    {
                        button.Text = Neusoft.FrameWork.Management.Language.Msg(button.Text);
                        button.ToolTipText = Neusoft.FrameWork.Management.Language.Msg(button.ToolTipText);
                    }
                }
                else if (c.GetType().IsSubclassOf(typeof(FarPoint.Win.Spread.FpSpread)) || c.GetType() == typeof(FarPoint.Win.Spread.FpSpread))
                {
                    //��ʱ���θò��ִ��� ���´���ſ�����DataSet�󶨵�FarPoint���б��Ᵽ��A��B��C����

                    //FarPoint.Win.Spread.FpSpread fp = c as FarPoint.Win.Spread.FpSpread;
                    //foreach (FarPoint.Win.Spread.SheetView sv in fp.Sheets)
                    //{
                    //    foreach (FarPoint.Win.Spread.Column column in sv.Columns)
                    //    {
                    //        column.Label = Neusoft.FrameWork.Management.Language.Msg(column.Label);
                    //    }
                    //}
                }
            }
            catch { }
            //else if (c.GetType().IsSubclassOf(GroupBox))
            //{

            //}  //else if (c.GetType().IsSubclassOf(Panel))
            //{

            //}//else if (c.GetType().IsSubclassOf(MainMenu))
            //{

            //}
            //else if (c.GetType().IsSubclassOf(MainMenuStrip))
            //{

            //}
            //if (c.GetType().IsSubclassOf(TextBoxBase))
            //{

            //}

        }

        /// <summary>
        /// ��ʼ��Designer
        /// </summary>
        public void iniForm()
        {
            try
            {
                DisignControl = new Neusoft.FrameWork.WinForms.Controls.DesignControl(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
		private void BaseForm_Closed(object sender, System.EventArgs e) 
		{
            try
            {
                this.DisignControl.Dispose();
            }
            catch { }
		}

		private void BaseForm_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Alt && e.Control && e.KeyCode == Keys.W )
			{
				DisignControl.IsDesignMode = !DisignControl.IsDesignMode;
			}
			else if(e.KeyCode == Keys.F4)
			{
				if(DisignControl.IsDesignMode)
					DisignControl.IsShowPropertyForm = true;
			}
		}
	}
	
	
}
