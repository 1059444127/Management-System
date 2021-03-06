using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.BizLogic.Privilege.Service;
using Neusoft.HISFC.BizLogic.Privilege.Model;

namespace HIS
{
    public partial class frmLogin : Neusoft.FrameWork.WinForms.Forms.BaseForm 
    {
        public frmLogin()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmLogin_Load);
            
        }

        protected override void OnClosed(EventArgs e)
        {
                Application.Exit();
            base.OnClosed(e);
        }
        void frmLogin_Load(object sender, EventArgs e)
        {
            #region 判断自动下载是否需要更新
            if (System.IO.File.Exists("AutoUpdate.exe") && System.IO.File.Exists("复件 AutoUpdate.exe"))
            {
                System.IO.File.Delete("AutoUpdate.exe");
                System.IO.File.Copy("复件 AutoUpdate.exe", "AutoUpdate.exe");
                System.IO.File.Delete("复件 AutoUpdate.exe");
            }
            #endregion 

            this.DisignControl.IsAllowToNextControl = false;

            //{5313B8E5-709F-4741-A6E3-2186702DAC6C}
            this.lbLicence.Text = "本系统授权给:  " + Program.HosName;

            if (Program.IsTestDB)
            {
                this.lbLicence.Text = this.lbLicence.Text + " - 测试库";
            }
        }

        private void neuButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void neuButton1_Click(object sender, EventArgs e)
        {
            //确定
            if (this.txtUserID.Text.Trim() == "")
            {
                this.txtUserID.Focus();
                Program.isMessageShow = false;
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("请输入用户名！"),Neusoft.FrameWork.Management.Language.Msg("提示"));
                Program.isMessageShow = true;
                return;
            }
            if (this.txtPWD.Text.Trim() == "")
            {
                this.txtUserID.Focus();
                Program.isMessageShow = false;
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("请输入密码！"), Neusoft.FrameWork.Management.Language.Msg("提示"));
                Program.isMessageShow = true;
                return;
            }

            string _account = this.txtUserID.Text.Trim();
            string _pass = this.txtPWD.Text.Trim();

            if (LoginFunction.Login(_account, _pass) == -1)
            {
                this.txtPWD.Focus();
                return;
            }

            this.Hide();

            if (HIS.Program.MainForm == null)
                HIS.Program.MainForm = new frmMain();

            HIS.Program.MainForm.InitMenu(((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).CurrentGroup.ID);
            if (Program.IsTestDB)
            {
                HIS.Program.MainForm.Text = HIS.Program.MainForm.Text + " - 测试库";
            }
            HIS.Program.MainForm.Show();

            ////新增加的代码如下;用来判断用户的状态
            //int retv = 0;//
            ////if ((retv = Program.ShowMainForm(this.txtUserID.Text, this.txtPWD.Text, "", "")) == 0)
            ////{
            ////    this.txtPWD.Clear();
            ////    this.Hide();
            ////}
            //if ((retv =Login(_account,_pass) )== 0)
            //{
            //    this.txtPWD.Clear();
            //    this.Hide();
            //}
            //else
            //{
            //    if (retv == -1)
            //    {
            //        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("用户名或密码不正确，请重试！"), Neusoft.FrameWork.Management.Language.Msg("提示"));
            //        this.txtUserID.Focus();
            //        this.txtUserID.SelectAll();
            //    }
            //    else if (retv == -2)
            //    {
            //        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("用户名已经停用，请更换！"), Neusoft.FrameWork.Management.Language.Msg("提示"));
            //        this.txtUserID.Focus();
            //        this.txtUserID.SelectAll();
            //    }
            //    else
            //    {
            //        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("用户名已经废弃，请更改！"), Neusoft.FrameWork.Management.Language.Msg("提示"));
            //        this.txtUserID.Focus();
            //        this.txtUserID.SelectAll();
            //    }
            //}
            //新增加的代码结束;
        }
        
        private void txtPWD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                this.neuButton1_Click(null, e);
            }
        }

        private void txtUserID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.txtPWD.Focus();
                e.Handled = true;
                //System.Windows.Forms.SendKeys.Send("{tab}");
            }
        }

        private void txtUserID_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            this.txtUserID.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
         
            
            #region 2013.02.20 注释
            ////{57CC110D-2CF8-4704-93F3-3BFA247FB41C}
            //if (System.Configuration.ConfigurationSettings.AppSettings["Theme"] == "1")         //东软蓝
            //{
            //    this.BackgroundImage = HIS.Properties.Resources.东软蓝_登陆界面;
                
            //    this.neuButton1.BackgroundImage = HIS.Properties.Resources.东软蓝_按钮;
            //    this.neuButton2.BackgroundImage = HIS.Properties.Resources.东软蓝_按钮;
            //}
            //else if (System.Configuration.ConfigurationSettings.AppSettings["Theme"] == "2")    //东软青
            //{
            //    this.BackgroundImage = HIS.Properties.Resources.东软青_登陆界面;
                
            //    this.neuButton1.BackgroundImage = HIS.Properties.Resources.东软青_背景;
            //    this.neuButton2.BackgroundImage = HIS.Properties.Resources.东软青_背景;
            //}
            #endregion


            base.OnLoad(e);
        }
    }
}