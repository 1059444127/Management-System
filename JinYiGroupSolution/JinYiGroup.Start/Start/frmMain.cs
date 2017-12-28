using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace HIS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        protected override void OnLoad(EventArgs e)
        {
          
            //this.Text = this.Text + " for " + Neusoft.FrameWork.Management.Connection.Instance.GetType().ToString();
            //this.skinEngine1.Active = false;

            this.Text = "�ۺϹ���ϵͳ �� " + Program.HosName;

            //{57CC110D-2CF8-4704-93F3-3BFA247FB41C}
            //if (System.Configuration.ConfigurationSettings.AppSettings["Theme"] == "1")         //������
            //{
            //this.BackgroundImage = HIS.Properties.Resources.������_����;
                
            //}
            //else if (System.Configuration.ConfigurationSettings.AppSettings["Theme"] == "2")    //������
            //{
            this.BackgroundImage = HIS.Properties.Resources.��������_Ʒ������_����;
                
            //}

            
            base.OnLoad(e);
            this.WindowState = FormWindowState.Maximized;
        }
        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //����FarPoint�ڷ����Ĵ����쳣   {90D30CF7-F965-4fcf-A38A-BDE9AC3A896B}
            if (e.Exception.Source == "FarPoint.Win.Spread")
            {
                return;
            }
           
            Neusoft.FrameWork.Management.PublicTrans.RollBack();
            Neusoft.FrameWork.WinForms.Classes.Function.MessageBox(e.Exception);
            //Neusoft.FrameWork.Management.Connection.Instance.Close();
            //Neusoft.FrameWork.Management.Connection.Instance.Open();
        }

     
        /// <summary>
        /// ��ʼ���˵�
        /// </summary>
        public void InitMenu()
        {
            //this.MainMenuStrip = Menu.AddMenu(this);
            //this.Controls.Add(this.MainMenuStrip);

        }

        public void InitMenu(string roleId)
        {
            MenuStrip _strip = HIS.Menu.InitMenu(roleId, this);
          
            this.MainMenuStrip = _strip;
            this.MainMenuStrip.BackColor = Color.WhiteSmoke;
            this.Controls.Add(_strip);
        }

        private void helloToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        protected override void  OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("�Ƿ�Ҫ�˳�ϵͳ��", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //{5BE03DF2-25DE-4e7a-9B47-85CE92911277}  HISϵͳע��ǰ�ж�
                if (Neusoft.HISFC.Components.Manager.Classes.Function.HISLogout() == -1)
                {
                    return;
                }

                foreach (Form fr in this.MdiChildren)
                {
                    fr.Close();
                }

                try
                {
                    //if (Neusoft.FrameWork.Management.Connection.Instance.State != ConnectionState.Closed)
                    //{
                    //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //    Neusoft.FrameWork.Management.Connection.Instance.Close();
                    //}
                }
                catch { }

                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
    
 	         base.OnClosing(e);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
           
        }

        //protected Sunisoft.IrisSkin.SkinEngine skinM = null;

        //public int InitSkinManager()
        //{
        //    if (skinM == null)
        //    {                
        //        skinM = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));

        //        skinM.SerialNumber = "";
        //        skinM.SkinFile = @"D:\his4.5\HIS\Src\HIS\bin\Debug\Ƥ��\MacOSƻ��\MacOS.ssk";
        //    }

        //    if (skinM == null)
        //    {
        //        return -1;
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        //}

        //public string SkinFile
        //{
        //    get
        //    {
        //        return this.skinEngine1.SkinFile;
        //    }
        //    set
        //    {
        //        skinEngine1.SkinFile = value;
        //    }
        //}

        //public bool UseDefaultSkin
        //{
        //    get
        //    {
        //        return skinEngine1.Active;
        //    }
        //    set
        //    {
        //        skinEngine1.Active = value;
        //    }
        //}
        protected override void  OnKeyDown(KeyEventArgs e)
        {
            
            //if(e.Alt && e.Control && e.KeyCode == Keys.T )
            //{
            //   //����ϵͳ��س���
            //    frmMonitor form = new frmMonitor();
            //    form.MdiParent = this;
            //    form.Show();
            //}
 	        base.OnKeyDown(e);
        }

        #region ����
        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);


        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);

            switch (m.Msg)
            {

                case 0x86://WM_NCACTIVATE

                    goto case 0x85;

                case 0x85://WM_NCPAINT
                    {

                        IntPtr hDC = GetWindowDC(m.HWnd);

                        //��DCת��Ϊ.NET��Graphics�Ϳ��Ժܷ����ʹ��Framework�ṩ�Ļ�ͼ������
                        Graphics gs = Graphics.FromHdc(hDC);

                        int ibox = 1;

                        if (this.MaximizeBox) ibox++;
                        if (this.MinimizeBox) ibox++;

                        //�õ���ر���ͼƬ
                        Image imgpm = this.Icon.ToBitmap();//Image.FromFile( Application.StartupPath+ @"\Main.png");

                        int iBoxWidh = 21;

                        //gs.DrawImage(imgbg,3,0,this.Width - (ibox * iBoxWidh),SystemInformation.CaptionHeight + 2);    //��ʾ����ͼƬ

                        //��������ʾ������ɫ
                        int xPos = this.Width - iBoxWidh * ibox - 2 - ibox * 3;

                        Rectangle excludeRect;
                        //�������в���Ҫ����λ��
                        for (int i = 0; i < ibox; i++)
                        {
                            if (i > 0)
                            {
                                xPos = xPos + iBoxWidh + 2;
                            }

                            excludeRect = new Rectangle(xPos, 5, iBoxWidh, iBoxWidh + 1);
                            gs.ExcludeClip(excludeRect);
                        }


                        //��������
                        Rectangle rBackground = new Rectangle(0, 0, this.Width, SystemInformation.CaptionHeight + 3);
                        //������ɫ ���½���
                        System.Drawing.Drawing2D.LinearGradientBrush bBackground
                            = new System.Drawing.Drawing2D.LinearGradientBrush(rBackground, Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.DarkGreen), Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.LightBlue), LinearGradientMode.Vertical);

                        //��������䱳����ɫ
                        gs.FillRectangle(bBackground, rBackground);
                        //��ʾͼ���ļ�
                        gs.DrawImage(imgpm, 4, 4, 24, 24);
                        StringFormat strFmt = new StringFormat();

                        //strFmt.Alignment = StringAlignment.Center;
                        //strFmt.LineAlignment = StringAlignment.Center;

                        //gs.DrawString(this.Text, this.Font, Brushes.BlanchedAlmond, m_rect, strFmt);
                        //���ñ�������
                        Font drawFont = new Font("����", 10, System.Drawing.FontStyle.Bold);
                        //���ñ�����ɫ
                        SolidBrush drawBrush = new SolidBrush(Color.Black);

                        //�ػ�����
                        gs.DrawString(this.Text, drawFont, drawBrush, 30, 8);

                        gs.Dispose();

                        //�ͷ�GDI��Դ

                        ReleaseDC(m.HWnd, hDC);

                        break;

                    }

                case 0xA1://WM_NCLBUTTONDOWN
                    {

                        Point mousePoint = new Point((int)m.LParam);

                        mousePoint.Offset(-this.Left, -this.Top);

                        break;

                    }

            }


        }
        #endregion
    }
}