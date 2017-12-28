using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using FarPoint.Win;

namespace Neusoft.FrameWork.WinForms.Classes
{
    /// <summary>
    /// [��������: ���ֲ㺯��]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class Function
    {
        #region �ȴ��Ի���
        private static Neusoft.FrameWork.WinForms.Forms.frmWait frmWaitForm = new Neusoft.FrameWork.WinForms.Forms.frmWait();

        /// <summary>
        /// ��ǰ�ȴ�����
        /// </summary>
        public static Neusoft.FrameWork.WinForms.Forms.frmWait WaitForm
        {
            get
            {
                return frmWaitForm;
            }
            set
            {
                frmWaitForm = value;
            }
        }

        /// <summary>
        /// ��ʾ�ȴ�����
        /// </summary>
        /// <param name="tip"></param>
        public static void ShowWaitForm(string tip, int Progress, bool IsShowCancelButton)
        {
            if (frmWaitForm == null) frmWaitForm = new Neusoft.FrameWork.WinForms.Forms.frmWait();
            if (tip != "") frmWaitForm.Tip = tip;
            if (Progress >= 0) frmWaitForm.Progress = Progress;
            frmWaitForm.IsShowCancelButton = IsShowCancelButton;
            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            if (frmWaitForm.Visible == false)
            {
                frmWaitForm.Show();
            }
        }

        /// <summary>
        /// ��ʾ�ȴ�����
        /// </summary>
        /// <param name="Progress"></param>
        /// <param name="Max"></param>
        public static void ShowWaitForm(int Progress, int Max)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.WaitForm.progressBar1.Maximum = Max;
            ShowWaitForm("", Progress, false);
        }
        /// <summary>
        /// ��ʾ�ȴ�����
        /// </summary>
        /// <param name="tip">��ʾ��Ϣ</param>
        /// <param name="IsShowCancelButton">�Ƿ���ʾȡ����ť</param>
        public static void ShowWaitForm(string tip, bool IsShowCancelButton)
        {
            ShowWaitForm(tip, -1, IsShowCancelButton);
        }
        /// <summary>
        /// ��ʾ�ȴ�����
        /// </summary>
        /// <param name="Progress">��ǰ����������</param>
        public static void ShowWaitForm(int Progress)
        {
            ShowWaitForm("", Progress, false);
        }
        /// <summary>
        /// ��ʾ�ȴ�����
        /// </summary>
        /// <param name="tip">��ǰ��ʾ��Ϣ</param>
        public static void ShowWaitForm(string tip)
        {
            ShowWaitForm(tip, -1, false);
        }
        /// <summary>
        /// �رյȴ�����
        /// </summary>
        public static void HideWaitForm()
        {
            Cursor.Current = System.Windows.Forms.Cursors.Default;
            WaitForm.Hide();
        }
        #endregion

        #region �������ں���
        public static Neusoft.FrameWork.WinForms.Forms.BaseForm PopForm = new Neusoft.FrameWork.WinForms.Forms.BaseForm();
        /// <summary>
        /// ��PopForm������ʾ�����ؼ�
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        /// <param name="borderStyle">���ڱ߿�����</param>
        /// <param name="windowState">����״̬</param>
        /// <returns>System.Windows.Forms.DialogResult</returns>
        public static System.Windows.Forms.DialogResult PopShowControl(Control c, System.Windows.Forms.FormBorderStyle borderStyle, System.Windows.Forms.FormWindowState windowState)
        {
            //������ʱ���ڣ�������ʾ�ؼ�
            PopForm.StartPosition = FormStartPosition.CenterScreen;
            PopForm.FormBorderStyle = borderStyle;		//���ڱ߿�����
            PopForm.WindowState = windowState;			//����״̬
            PopForm.AutoScaleMode = AutoScaleMode.None;
            //�����ؼ�����ӵ���ʱ������
            if (c == null) c = new Control();
            PopForm.Width = c.Width + 8;
            PopForm.Height = c.Height + 34;
            c.Dock = DockStyle.Fill;
            PopForm.Controls.Clear();
            PopForm.Visible = false;
            PopForm.Controls.Add(c);
            PopForm.Text = "��������";//{8004B645-69A3-40a0-9D0B-4C76BB607595}
            //��ʾ��ʱ����
            PopForm.ShowDialog();
            try
            {
                c.Dock = DockStyle.None;
            }
            catch { }            
            return PopForm.DialogResult;
        }


        /// <summary>
        /// ����ʱ�����Ĵ�����ʾ�����ؼ�
        /// Ĭ�ϴ���״̬ΪNormal
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        /// <param name="borderStyle">���ڱ߿�����</param>
        /// <returns>System.Windows.Forms.DialogResult</returns>
        public static System.Windows.Forms.DialogResult PopShowControl(Control c, System.Windows.Forms.FormBorderStyle borderStyle)
        {
            return PopShowControl(c, borderStyle, System.Windows.Forms.FormWindowState.Normal);	//Ĭ�ϴ���״̬ΪNormal
        }


        /// <summary>
        /// ����ʱ�����Ĵ�����ʾ�����ؼ�
        /// Ĭ�ϴ���״̬ΪNormal,
        /// Ĭ�ϴ��ڱ߿�����ΪFixedToolWindow
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        /// <returns>System.Windows.Forms.DialogResult</returns>
        public static System.Windows.Forms.DialogResult PopShowControl(Control c)
        {
            return PopShowControl(c, System.Windows.Forms.FormBorderStyle.FixedToolWindow, System.Windows.Forms.FormWindowState.Normal);	//Ĭ�ϴ��ڱ߿�����ΪFixedToolWindow
        }


        /// <summary>
        /// ����ʱ�����Ĵ�����ʾ�����ؼ�
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        /// <param name="borderStyle">���ڱ߿�����</param>
        /// <param name="windowState">����״̬</param>
        public static void ShowControl(Control c, System.Windows.Forms.FormBorderStyle borderStyle, System.Windows.Forms.FormWindowState windowState)
        {
            //������ʱ���ڣ�������ʾ�ؼ�
            Neusoft.FrameWork.WinForms.Forms.BaseForm frmTemp = new Neusoft.FrameWork.WinForms.Forms.BaseForm();
            frmTemp.StartPosition = FormStartPosition.CenterScreen;	//������ʾ
            frmTemp.FormBorderStyle = borderStyle;		//���ڱ߿�����
            frmTemp.WindowState = windowState;			//����״̬
            frmTemp.Text = c.Text;						//���ڱ���
            frmTemp.AutoScaleMode = AutoScaleMode.None;
            frmTemp.Visible = false;
            //�����ؼ�����ӵ���ʱ������
            if (c == null) c = new Control();
            frmTemp.Width = c.Width + 8;
            frmTemp.Height = c.Height + 34;
            c.Dock = DockStyle.Fill;
            frmTemp.Controls.Add(c);
            //��ʾ��ʱ����
            frmTemp.ShowDialog();
            try
            {
                c.Dock = DockStyle.None;
            }
            catch { }
        }


        /// <summary>
        /// ����ʱ�����Ĵ�����ʾ�����ؼ�
        /// Ĭ�ϴ���״̬ΪNormal
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        /// <param name="borderStyle">���ڱ߿�����</param>
        public static void ShowControl(Control c, System.Windows.Forms.FormBorderStyle borderStyle)
        {
            ShowControl(c, borderStyle, System.Windows.Forms.FormWindowState.Normal);	//Ĭ�ϴ���״̬ΪNormal
        }


        /// <summary>
        /// ����ʱ�����Ĵ�����ʾ�����ؼ�
        /// Ĭ�ϴ���״̬ΪNormal,
        /// Ĭ�ϴ��ڱ߿�����ΪFixedToolWindow
        /// </summary>
        /// <param name="c">����ʾ�Ŀؼ�</param>
        public static void ShowControl(Control c)
        {
            ShowControl(c, System.Windows.Forms.FormBorderStyle.FixedToolWindow, System.Windows.Forms.FormWindowState.Normal);	//Ĭ�ϴ��ڱ߿�����ΪFixedToolWindow
        }


        #endregion

        #region �������Ͳ˵��ﶨ
        protected static ToolBar myToolBar;
        /// <summary>
        /// ���˵��빤������ʾ����һ��
        /// ���ݲ˵������ƺ�ToolButton��Tag����
        /// </summary>
        /// <param name="menu">�˵�</param>
        /// <param name="toolbar">������</param>
        public static void BindingMenuToToolBar(MainMenu menu, ToolBar toolbar)
        {
            myToolBar = toolbar;
            HideAllButton();
            foreach (MenuItem m in menu.MenuItems)
            {
                if (m.Visible) GetMenu(m);
            }

        }
        /// <summary>
        /// ��ò˵���Ŀ
        /// </summary>
        /// <param name="m"></param>
        private static void GetMenu(MenuItem m)
        {
            for (int i = 0; i < m.MenuItems.Count; i++)
            {
                if (m.MenuItems[i].MenuItems.Count > 0 && m.MenuItems[i].Visible) GetMenu(m.MenuItems[i]);
                ShowButton(m.MenuItems[i].Text, m.MenuItems[i].Visible);
            }
        }
        /// <summary>
        /// �������й�������ť
        /// </summary>
        private static void HideAllButton()
        {
            foreach (ToolBarButton b in myToolBar.Buttons)
            {
                b.Visible = false;
            }
        }
        /// <summary>
        /// ��ʾ��ͬ���Ƶİ�ť
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="bVisible"></param>
        private static void ShowButton(string menuName, bool bVisible)
        {
            foreach (ToolBarButton b in myToolBar.Buttons)
            {
                try
                {
                    if (b.Tag.ToString() == menuName)
                    {
                        b.Visible = bVisible;
                    }
                }
                catch { }
            }
        }
        #endregion

        #region �����ʱ�ļ�

        /// <summary>
        /// ���һ����ʱ�ļ�����
        /// ���ݵ�ǰTick
        /// </summary>
        /// <returns></returns>
        public static string GetTempFileName()
        {
            string s = DateTime.Now.Ticks.ToString();
            string strLocalPath = Application.StartupPath;
            if (System.IO.Directory.Exists(strLocalPath + "\\tmp\\") == false)
            {
                System.IO.Directory.CreateDirectory(strLocalPath + "\\tmp\\");
            }
            s = strLocalPath + "\\tmp\\" + s;
            return s;
        }

        /// <summary>
        /// �����ʱĿ¼
        /// </summary>
        public static void ClearTempFolder()
        {
            string strLocalPath = Application.StartupPath;
            if (System.IO.Directory.Exists(strLocalPath + "\\tmp\\"))
            {
                System.IO.Directory.Delete(strLocalPath + "\\tmp\\", true);
            }
            System.IO.Directory.CreateDirectory(strLocalPath + "\\tmp\\");

        }
        #endregion

        #region ������뷨����

        /// <summary>
        /// ���Ĭ�����뷨
        /// combobox�õ���
        /// </summary>
        /// <returns></returns>
        public static int GetInputType()
        {
            string strLocalPath = Application.StartupPath;
            string filename = strLocalPath + "\\profile\\inputSetting.xml";
            if (System.IO.File.Exists(filename))//Ŀ¼����
            {
                //��ȡ�ļ�
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                try
                {
                    System.IO.StreamReader r = new System.IO.StreamReader(filename);
                    string cleandown = r.ReadToEnd();
                    doc.LoadXml(cleandown);
                    r.Close();
                }
                catch { return 0; }
                //�õ��ڵ���ֵ

                try
                {
                    string s;
                    s = doc.SelectSingleNode("/Setting/Input[@id='combobox']").Attributes["value"].Value;
                    int i = int.Parse(s);//ת����������
                    return i;
                }
                catch { return 0; }
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region ����

        /// <summary>
        ///  ����ѡ�����ڴ��ڣ�������ʼ���ں���ֹ����
        /// writed by cuipeng 
        /// 2005-4
        /// </summary>
        /// <param name="dateBegin">���ص���ʼ����</param>
        /// <param name="dateEnd">���ص���ֹ����</param>
        /// <returns>1ѡ�������ڣ�0û��ѡ��</returns>
        public static int ChooseDate(ref DateTime dateBegin, ref DateTime dateEnd)
        {
            Neusoft.FrameWork.WinForms.Forms.frmChooseDate form = new Neusoft.FrameWork.WinForms.Forms.frmChooseDate();

            //֪ͨ������ʾ��ʼ���ں���ֹ����
            form.IsOneDate = false;

            //���������ʼ���ڸ���������ʼ���ڵ�Ĭ��ֵ
            if (dateBegin != DateTime.MinValue)
                form.DateBegin = dateBegin;
            //���������ֹ���ڸ���������ֹ���ڵ�Ĭ��ֵ
            if (dateEnd != DateTime.MinValue)
                form.DateEnd = dateEnd;

            System.Windows.Forms.DialogResult Result = form.ShowDialog();
            //ȡ���ڷ��ص���ʼ���ں���ֹ����
            if (Result == DialogResult.OK)
            {
                dateBegin = form.DateBegin;
                dateEnd = form.DateEnd;
                //ȡ�������ڣ�����1
                return 1;
            }

            //���û��ѡ�����ڣ��򷵻�0
            return 0;
        }


        /// <summary>
        ///  ����ѡ�����ڴ��ڣ������û�ѡ�������
        /// writed by cuipeng 
        /// 2005-4
        /// </summary>
        /// <param name="date">����</param>
        /// <returns>1ѡ�������ڣ�0û��ѡ��</returns>
        public static int ChooseDate(ref DateTime date)
        {
            Neusoft.FrameWork.WinForms.Forms.frmChooseDate form = new Neusoft.FrameWork.WinForms.Forms.frmChooseDate();

            //֪ͨ������ʾһ������
            form.IsOneDate = true;
            form.Init();
            //���������ʼ���ڸ���������ʼ���ڵ�Ĭ��ֵ
            if (date != DateTime.MinValue)
                form.DateBegin = date;

            System.Windows.Forms.DialogResult Result = form.ShowDialog();
            //ȡ���ڷ��ص���ʼ���ں���ֹ����
            if (Result == DialogResult.OK)
            {
                date = form.DateBegin;
                //ȡ�������ڣ�����1
                return 1;
            }

            //���û��ѡ�����ڣ��򷵻�0
            return 0;
        }


        /// <summary>
        /// frmEasyChoose ��ժҪ˵����
        /// ���ٲ�ѯ����
        /// writed by cuipeng
        /// 2005-3
        /// </summary>
        /// <param name="arrayList"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public static int ChooseItem(ArrayList arrayList, ref Neusoft.FrameWork.Models.NeuObject neuObject)
        {
            Neusoft.FrameWork.WinForms.Forms.frmEasyChoose form = new Neusoft.FrameWork.WinForms.Forms.frmEasyChoose(arrayList);

            //���ò�ѯ����
            System.Windows.Forms.DialogResult Result = form.ShowDialog();
            //ȡ���ڷ��ص���ʼ���ں���ֹ����
            if (Result == DialogResult.OK)
            {
                neuObject = form.Object;
                //ȡ�������ݣ��򷵻�1
                return 1;
            }

            //���û��ѡ�����ݣ��򷵻�0
            return 0;
        }

        public static int ChooseItem(ArrayList arrayList, string[] label, bool[] visible, int[] width, ref Neusoft.FrameWork.Models.NeuObject neuObject)
        {
            Neusoft.FrameWork.WinForms.Forms.frmEasyChoose form = new Neusoft.FrameWork.WinForms.Forms.frmEasyChoose(arrayList);
            form.SetFormat(label, visible, width);

            //���ò�ѯ����
            System.Windows.Forms.DialogResult Result = form.ShowDialog();
            //ȡ���ڷ��ص���ʼ���ں���ֹ����
            if (Result == DialogResult.OK)
            {
                neuObject = form.Object;
                //ȡ�������ݣ��򷵻�1
                return 1;
            }

            //���û��ѡ�����ݣ��򷵻�0
            return 0;
        }


        /// <summary>
        /// ���ݴ�������鵯�������г���Ŀ�����û�ѡ������Ŀ
        /// </summary>
        /// <returns></returns>
        public static ArrayList ChooseMultiObject(ArrayList arrayList)
        {
            Neusoft.FrameWork.WinForms.Controls.ucChooseMultiObject uc = new Neusoft.FrameWork.WinForms.Controls.ucChooseMultiObject(arrayList);
            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
            return uc.ArrayObject;
        }
        #endregion

        #region "MessageBox���� "
        /// <summary>
        /// ��װMessageBox���� 
        /// </summary>
        /// <param name="MsgText">Message����</param>
        /// <param name="MsgType">��λ����(������ԭpb����Ĳ���)��һλ����MessageBoxIcon 1��ͷ������ʾ2��ͷ�������3��ͷ������4��ͷ��������5��ͷ������ͼ��
        ///                       ����λΪ11,21,23,31,32 �ֱ����MessageButtons  OK,OKCancel,YesNo,RetryCancel,AbortRetryIgnore</param>
        /// <returns></returns>
        public static System.Windows.Forms.DialogResult Msg(string MsgText, int MsgType)
        {
            //����MessageBox Result
            DialogResult r = new DialogResult();
            MsgText = Neusoft.FrameWork.Management.Language.Msg(MsgText); // by ţ��Ԫ 2008��9��
            switch (MsgType)
            {
                //��ʾ��Ϣ
                case 111:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 121:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    break;
                case 122:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    break;
                case 123:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                    break;
                case 131:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    break;
                case 132:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Information);
                    break;
                //������Ϣ
                case 211:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                case 221:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                    break;
                case 222:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    break;
                case 223:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
                    break;
                case 231:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop);
                    break;
                case 232:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                    break;

                //������Ϣ
                case 311:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 321:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    break;
                case 322:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    break;
                case 323:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    break;
                case 331:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    break;
                case 332:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("����"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                    break;
                //������Ϣ
                case 411:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OK, MessageBoxIcon.Question);
                    break;
                case 421:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    break;
                case 422:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    break;
                case 423:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                    break;
                case 431:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    break;
                case 432:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
                    break;
                //����ͼ����Ϣ
                case 511:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OK, MessageBoxIcon.None);
                    break;
                case 521:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.OKCancel, MessageBoxIcon.None);
                    break;
                case 522:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNo, MessageBoxIcon.None);
                    break;
                case 523:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.RetryCancel, MessageBoxIcon.None);
                    break;
                case 531:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.None);
                    break;
                case 532:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg("��ʾ"), MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
                    break;
                default:
                    r = System.Windows.Forms.MessageBox.Show(MsgText, Neusoft.FrameWork.Management.Language.Msg(Neusoft.FrameWork.Management.Language.Msg("��ʾ")), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

            }
            return r;
        }


        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        /// <param name="ex"></param>
        public static void MessageBox(Exception ex)
        {
            Neusoft.FrameWork.WinForms.Forms.frmMessageBox f = new Neusoft.FrameWork.WinForms.Forms.frmMessageBox();
            f.SetMessage(ex.Message, ex.StackTrace);
            f.Text = ex.Source;
            f.ShowDialog();
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="message"></param>
        public static void MessageBox(string message)
        {
            Neusoft.FrameWork.WinForms.Forms.frmMessageBox f = new Neusoft.FrameWork.WinForms.Forms.frmMessageBox();
            f.SetMessage(message, "");
            f.ShowDialog();
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="message">���û���</param>
        /// <param name="innerMessage">�ڲ�����Ϣ</param>
        public static void MessageBox(string message, string innerMessage)
        {
            Neusoft.FrameWork.WinForms.Forms.frmMessageBox f = new Neusoft.FrameWork.WinForms.Forms.frmMessageBox();
            f.SetMessage(message, innerMessage);
            f.ShowDialog();
        }

        /// <summary>
        ///  ��ʾ��Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <param name="title"></param>
        public static void MessageBox(string message, string innerMessage, string title)
        {
            Neusoft.FrameWork.WinForms.Forms.frmMessageBox f = new Neusoft.FrameWork.WinForms.Forms.frmMessageBox();
            f.SetMessage(message, innerMessage);
            f.Text = title;
            f.ShowDialog();
        }

        /// <summary>
        ///  ��ʾ��Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <param name="messageBoxIcon"></param>
        public static void MessageBox(string message, string innerMessage, MessageBoxIcon messageBoxIcon)
        {
            Neusoft.FrameWork.WinForms.Forms.frmMessageBox f = new Neusoft.FrameWork.WinForms.Forms.frmMessageBox();
            f.SetMessage(message, innerMessage);
            f.ShowDialog();
        }
        #endregion

        #region �رյ�ǰ���ں���
        protected static Form _needCloseForm;
        /// <summary>
        /// 2���رյ�ǰ����
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static int CloseCurrentForm(Form form)
        {
            Timer t = new Timer();
            t.Interval = 2000;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
            form.Hide();
            _needCloseForm = form;
            return 0;
        }

        /// <summary>
        /// 2���رմ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void t_Tick(object sender, EventArgs e)
        {
            _needCloseForm.Close();
            ((Timer)sender).Enabled = false;
            try
            {
                ((Timer)sender).Dispose();
            }
            catch { }
        }
        #endregion

        #region "�����ı��ؼ�����ɫ��ʾ"
        /// <summary>
        ///׷�Ӷ����ı���������ɫ
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void RichTextBoxAppendText(RichTextBox rtb, string text, System.Drawing.Color color)
        {
            rtb.AppendText(text);
            int index = rtb.Text.LastIndexOf(text);
            if (index != -1)
            {
                rtb.Select(index, text.Length);
                rtb.SelectionColor = color;
            }
        }
        #endregion

        #region ������֤
        /// <summary>
        /// ƥ����ʽ��:20030718,030718 
        ///��Χ:1900--2099 
        ///������ʽ
        /// </summary>
        /// <param name="date"></param>
        /// <param name="errorMessage"></param>
        /// <returns>-1 ���� 0 ��ȷ����Ϣ</returns>
        public static int CheckDate(string date, ref string errorMessage)
        {

            try
            {
                //������ʽ��֤
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"((((19){1}|(20){1})d{2})|d{2})[01]{1}d{1}[0-3]{1}d{1}");
                System.Text.RegularExpressions.Match mc = rg.Match(date);
                if (!mc.Success)
                {
                    errorMessage = "��¼����Ч���ڸ�ʽ!";
                    return -1;
                }
                return 0;
            }
            catch
            {
                errorMessage = "��¼����Ч���ڸ�ʽ";
                return -1;
            }
        }
        #endregion

        #region ���֤��֤


        /// <summary>
        /// ��֤���֤��Ч��
        /// </summary>
        /// <param name="cid">���֤����</param>
        /// <param name="errorMessage">������Ϣ����ȷ��15λ����18λ</param>
        /// <returns>-1 ���� 0 ��ȷ</returns>
        public static int CheckIDInfo(string cid, ref string errorMessage)
        {

            string[] aCity = new string[] { null, null, null, null, null, null, null, null, null, null, null, "����", "���", "�ӱ�", "ɽ��", "���ɹ�", null, null, null, null, null, "����", "����", "������", null, null, null, null, null, null, null, "�Ϻ�", "����", "�㽭", "��΢", "����", "����", "ɽ��", null, null, null, "����", "����", "����", "�㶫", "����", "����", null, null, null, "����", "�Ĵ�", "����", "����", "����", null, null, null, null, null, null, "����", "����", "�ຣ", "����", "�½�", null, null, null, null, null, "̨��", null, null, null, null, null, null, null, null, null, "���", "����", null, null, null, null, null, null, null, null, "����" };
            double iSum = 0;
            string info = cid;
            try
            {

                if (cid.Length == 15)
                {
                    info = TransIDFrom15To18(cid);
                }
                //{5260E3A7-AD1A-44df-8C2B-0352FE4AE343}
                info = info.ToLower();
                //������ʽ��֤
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
                System.Text.RegularExpressions.Match mc = rg.Match(info);
                if (!mc.Success)
                {
                    errorMessage = "��¼����Ч�����֤��!";
                    return -1;
                }

                info = info.ToLower();
                info = info.Replace("x", "a");
                //��֤ͷ��λ����
                if (aCity[int.Parse(info.Substring(0, 2))] == null)
                {
                    errorMessage = "���֤�е����Ƿ�";
                    return -1;
                }
                //��֤����
                try
                {
                    DateTime.Parse(info.Substring(6, 4) + "-" + info.Substring(10, 2) + "-" + info.Substring(12, 2));
                }
                catch
                {
                    errorMessage = "���֤�����շǷ�";
                    return -1;
                }
                //��Ȩ�㷨
                for (int i = 17; i >= 0; i--)
                {
                    iSum += (System.Math.Pow(2, i) % 11) * int.Parse(info[17 - i].ToString(), System.Globalization.NumberStyles.HexNumber);

                }
                if (iSum % 11 != 1)
                {
                    errorMessage = ("��¼����Ч�����֤��");
                    return -1;
                }
            }
            catch
            {
                errorMessage = "��¼����Ч�����֤��!";
                return -1;
            }
            if (cid.Length == 15)
            {
                return 0;
            }
            else
            {
                errorMessage = (aCity[int.Parse(info.Substring(0, 2))] + "," + info.Substring(6, 4) + "-" + cid.Substring(10, 2) + "-" + info.Substring(12, 2) + "," + (int.Parse(info.Substring(16, 1)) % 2 == 1 ? "��" : "Ů"));
                return 0;
            }


        }

        /// <summary>
        /// 15λʡ��֤������Ϊ18λ
        /// </summary>
        /// <param name="perID"></param>
        public static string TransIDFrom15To18(string perID)
        {

            int iS = 0;

            //��Ȩ���ӳ��� 
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //У���볣�� 
            string LastCode = "10X98765432";
            //�����֤�� 
            string perIDNew = "";

            try
            {
                perIDNew = perID.Substring(0, 6);
                //���ڵ�6λ����7λ�����ϡ�1������9���������� 
                perIDNew += "19";

                perIDNew += perID.Substring(6, 9);

                //���м�Ȩ��� 
                for (int i = 0; i < 17; i++)
                {
                    iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
                }

                //ȡģ���㣬�õ�ģֵ 
                int iY = iS % 11;
                //��LastCode��ȡ����ģΪ�����ŵ�ֵ���ӵ����֤�����һλ����Ϊ�����֤�š� 
                perIDNew += LastCode.Substring(iY, 1);
            }
            catch { return "��λ����!"; }

            return perIDNew;


        }

        #endregion

        #region ����(��ȡ)����Ĭ�ϲ�������
        /// <summary>
        /// Ĭ����ֵ�����ļ�����
        /// </summary>
        public static string DefaultValueFilePath = System.Windows.Forms.Application.StartupPath + "\\HISDefaultValue.xml";

        /// <summary>
        /// ��������������Ϣ
        /// �ҿ������߼������⣬����document�����Ƕ�ռ�ļ��ģ��㱣��һ���ˣ�����û�أ������޷����档
        /// </summary>
        /// <param name="GroupID">��ID</param>
        /// <param name="FunctionID">����ID</param>
        /// <param name="strErr">������Ϣ</param>
        /// <param name="paramsCollection">����ֵ</param>
        /// <returns></returns>
        public static int SaveDefaultValue(string GroupID, string FunctionID, out string strErr, params string[] paramsCollection)
        {
            strErr = "";
            XmlDocument doc = new XmlDocument();
            XmlElement rootElement;

            try
            {	// ���� �ļ��Ѵ���
                if (System.IO.File.Exists(Function.DefaultValueFilePath))
                {
                    if ((System.IO.File.GetAttributes(Function.DefaultValueFilePath) & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                    {
                        System.IO.File.SetAttributes(Function.DefaultValueFilePath, System.IO.FileAttributes.Normal);
                    }

                    //��ȡ�ļ�
                    using (System.IO.StreamReader rs = new StreamReader(Function.DefaultValueFilePath))
                    {
                        string cleandown = rs.ReadToEnd();
                        doc.LoadXml(cleandown);
                    }

                    rootElement = (System.Xml.XmlElement)doc.SelectSingleNode("/PersonalConfig");
                    if (rootElement == null)
                    {
                        strErr = "XML�����ļ���ʽ���� �����ڸ��ڵ�PersonalConfig";
                        return -1;
                    }

                    System.Xml.XmlElement funElement = (System.Xml.XmlElement)doc.SelectSingleNode(string.Format("/PersonalConfig/Group[@ID='{0}']/Function[@ID='{1}']", GroupID, FunctionID));
                    if (funElement != null)         //���ܽڵ����
                    {
                        funElement.RemoveAll();
                        funElement.SetAttribute("ID", FunctionID);

                        CreateValueElement(doc, funElement, paramsCollection);
                    }
                    else                            //���ܽڵ㲻���� �ж��Ƿ������ڵ�
                    {
                        System.Xml.XmlElement groupElement = (System.Xml.XmlElement)doc.SelectSingleNode(string.Format("/PersonalConfig/Group[@ID='{0}']", GroupID));
                        if (groupElement != null)      //������ڵ� ���ӹ��ܽڵ�
                        {
                            funElement = CreateFunctionElement(doc, FunctionID, paramsCollection);
                        }
                        else
                        {
                            groupElement = CreateGrouptElement(doc, rootElement, GroupID);

                            funElement = CreateFunctionElement(doc, FunctionID, paramsCollection);
                        }
                        groupElement.AppendChild(funElement);
                    }
                }
                else  //�ļ�������
                {
                    //���ø����
                    Neusoft.FrameWork.Xml.XML xmlManager = new Neusoft.FrameWork.Xml.XML();
                    rootElement = xmlManager.CreateRootElement(doc, "PersonalConfig");

                    System.Xml.XmlElement newGroupElement = CreateGrouptElement(doc, rootElement, GroupID);

                    System.Xml.XmlElement newFunElement = CreateFunctionElement(doc, FunctionID, paramsCollection);

                    newGroupElement.AppendChild(newFunElement);
                }

                doc.Save(Function.DefaultValueFilePath);
                return 1;
            }
            catch (Exception ex)
            {
                strErr = "�洢����Ĭ���������÷���δԤ֪����" + ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// ���칦�ܽڵ�
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="functionID">����ID</param>
        /// <param name="paramsCollection">����ֵ</param>
        /// <returns>�ɹ����ع��ܽڵ�</returns>
        private static System.Xml.XmlElement CreateFunctionElement(System.Xml.XmlDocument doc, string functionID, params string[] paramsCollection)
        {
            System.Xml.XmlElement funElement = doc.CreateElement("Function");

            funElement.SetAttribute("ID", functionID);

            CreateValueElement(doc, funElement, paramsCollection);

            return funElement;
        }

        /// <summary>
        /// ����ֵ�ڵ�
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="funElement">���ڵ�</param>
        /// <param name="paramsCollection">����ֵ</param>
        /// <returns>�ɹ�����1 </returns>
        private static int CreateValueElement(System.Xml.XmlDocument doc, System.Xml.XmlElement funElement, params string[] paramsCollection)
        {
            for (int index = 0; index < paramsCollection.Length; index++)
            {
                System.Xml.XmlElement valueElement = doc.CreateElement("Value" + (index + 1).ToString());
                valueElement.InnerText = paramsCollection[index];
                funElement.AppendChild(valueElement);
            }

            return 1;
        }

        /// <summary>
        /// ������ڵ�
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="rootElement">���ڵ�</param>
        /// <param name="groupID">��ID</param>
        /// <returns>������ڵ�</returns>
        private static System.Xml.XmlElement CreateGrouptElement(System.Xml.XmlDocument doc, System.Xml.XmlElement rootElement, string groupID)
        {
            System.Xml.XmlElement groupElement = (System.Xml.XmlElement)doc.CreateElement("Group");

            groupElement.SetAttribute("ID", groupID);

            rootElement.AppendChild(groupElement);

            return groupElement;
        }

        /// <summary>
        /// �������Ĭ�ϲ������� Ĭ����ID Ϊ"HIS"
        /// </summary>
        /// <param name="FunctionID">����ID</param>
        /// <param name="strErr">������Ϣ</param>
        /// <param name="alValues">������Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public static int SaveDefaultValue(string FunctionID, out string strErr, params string[] alValues)
        {
            return Function.SaveDefaultValue("HIS", FunctionID, out strErr, alValues);
        }

        /// <summary>
        /// �������Ĭ�ϲ������� Ĭ����ID Ϊ"HIS"
        /// </summary>
        /// <param name="FunctionID">����ID</param>
        /// <param name="alValues">������Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public static int SaveDefaultValue(string FunctionID, params string[] alValues)
        {
            string strErr;
            return Function.SaveDefaultValue(FunctionID, out strErr, alValues);
        }

        /// <summary>
        /// ��ȡ��������Ĭ������
        /// </summary>
        /// <param name="GroupID">�����</param>
        /// <param name="FunctionID">���ܱ���</param>
        /// <param name="strErr">������Ϣ</param>
        /// <returns>�ɹ�����string���� δ�ҵ����ؿ�ArrayList ʧ�ܷ���null</returns>
        public static ArrayList GetDefaultValue(string GroupID, string FunctionID, out string strErr)
        {
            strErr = "";
            ArrayList al = new ArrayList();
            try
            {
                if (System.IO.File.Exists(Function.DefaultValueFilePath))
                {
                    //����Xml�ļ�
                    XmlDocument doc = new XmlDocument();
                    //��ȡ�ļ�
                    using (System.IO.StreamReader rs = new StreamReader(Function.DefaultValueFilePath))
                    {
                        string cleandown = rs.ReadToEnd();
                        doc.LoadXml(cleandown);
                    }
                    System.Xml.XmlElement funElement = (System.Xml.XmlElement)doc.SelectSingleNode(string.Format("/PersonalConfig/Group[@ID='{0}']/Function[@ID='{1}']", GroupID, FunctionID));
                    if (funElement != null)         //���ܽڵ����
                    {
                        foreach (System.Xml.XmlNode valueNode in funElement.ChildNodes)
                        {
                            al.Add(valueNode.InnerText);
                        }
                    }
                }
                else
                {
                    strErr = "�����ļ������� �������Ա��ϵ";
                }
                return al;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// ��ȡ��������Ĭ������ Ĭ�������Ϊ"HIS"
        /// </summary>
        /// <param name="FunctionID">���ܱ���</param>
        /// <param name="strErr">������Ϣ</param>
        /// <returns>�ɹ�����string���� δ�ҵ����ؿ�ArrayList ʧ�ܷ���null</returns>
        public static ArrayList GetDefaultValue(string FunctionID, out string strErr)
        {
            return Function.GetDefaultValue("HIS", FunctionID, out strErr);
        }

        /// <summary>
        /// ��ȡ��������Ĭ������ Ĭ�������Ϊ"HIS"
        /// </summary>
        /// <param name="FunctionID">���ܱ���</param>
        /// <returns>�ɹ�����string���� δ�ҵ����ؿ�ArrayList ʧ�ܷ���null</returns>
        public static ArrayList GetDefaultValue(string FunctionID)
        {
            string strErr;
            return Function.GetDefaultValue("HIS", FunctionID, out strErr);
        }
        #endregion

        #region ������Ȳ���ָ���Ľڵ�
        public int LaserNode = 0;
        public int CurrentNode = 0;

        /// <summary>
        /// ��������Ȳ���ָ���Ľڵ㣬�ҵ����˳�����ֻ�������ҵ���һ���ڵ㡣
        /// </summary>
        /// <param name="treeNodes">��㼯��</param>
        /// <param name="searchText">���ҵ��ı�</param>
        /// <param name="byValueOrText">�Ƿ����tag��true tag����,false text����</param>
        /// <param name="isExact">�Ƿ�ȷ����</param>
        /// <param name="isSuper">�Ƿ����ִ�Сд��true ���֣�false������</param>
        /// <returns></returns>
        public TreeNode FindTreeNodeByDepth(TreeNodeCollection treeNodes, string searchText, bool byValueOrText, bool isExact, bool isSuper)
        {
            TreeNode treeNodeReturn = null;
            #region ʵ�֣���������Ȳ��ң��ҵ����˳�...

            string strNodeTag = "";
            string strNodeText = "";

            foreach (TreeNode node in treeNodes)
            {
                //ȡ��ǰ�ڵ��ֵ
                GetTreeNodeValueText(node, ref strNodeTag, ref strNodeText);

                if (byValueOrText)
                {
                    #region �Ƚ�tag��ֵ
                    if (isExact) //��ȷ���� 
                    {
                        if (!isSuper) //�����ִ�Сд
                        {
                            strNodeTag = strNodeTag.ToUpper();
                            searchText = searchText.ToUpper();
                        }
                        if (strNodeTag == searchText)
                        {
                            if (LaserNode <= CurrentNode)
                            {
                                treeNodeReturn = node;
                            }
                        }
                    }
                    else //ģ������ 
                    {
                        if (!isSuper) //�����ִ�Сд
                        {
                            strNodeTag = strNodeTag.ToUpper();
                            searchText = searchText.ToUpper();
                        }
                        if (strNodeTag.IndexOf(searchText) >= 0)
                        {
                            if (LaserNode <= CurrentNode)
                            {
                                treeNodeReturn = node;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region �Ƚ�text��ֵ
                    if (isExact) //��ȷ���� 
                    {
                        if (!isSuper) //�����ִ�Сд
                        {
                            strNodeText = strNodeText.ToUpper();
                            searchText = searchText.ToUpper();
                        }
                        if (strNodeText == searchText)
                        {
                            if (LaserNode <= CurrentNode)
                            {
                                treeNodeReturn = node;
                            }
                        }
                    }
                    else
                    {
                        if (!isSuper) //�����ִ�Сд
                        {
                            strNodeText = strNodeText.ToUpper();
                            searchText = searchText.ToUpper();
                        }
                        if (strNodeText.IndexOf(searchText) >= 0)
                        {
                            if (LaserNode <= CurrentNode)
                            {
                                treeNodeReturn = node;
                            }
                        }
                    }
                    #endregion
                }
                CurrentNode++;

                //�ҵ����˳�
                if (treeNodeReturn != null)
                {
                    break;
                }
                else
                {
                    //������Ȳ�ѯ
                    if (node.Nodes.Count > 0)
                    {
                        treeNodeReturn = FindTreeNodeByDepth(node.Nodes, searchText, byValueOrText, isExact, isSuper);
                    }
                }

            }

            #endregion
            return treeNodeReturn;
        }

        /// <summary>
        /// ȡֵ 
        /// </summary>
        /// <param name="p_treeNode"></param>
        /// <param name="p_value"></param>
        /// <param name="p_text"></param>
        private void GetTreeNodeValueText(TreeNode p_treeNode, ref string p_value, ref string p_text)
        {
            if (p_treeNode.Tag != null)
            {
                //ȡ��ֵ
                p_value = p_treeNode.Tag.ToString();
            }
            else
            {
                p_value = "";
            }
            //ȡ���� 
            p_text = p_treeNode.Text;
        }
        #endregion

        #region ��ӿؼ���Panel
        /// <summary>
        /// ����û��ؼ���ָ�������ؼ���
        /// </summary>
        /// <param name="alValues">�ؼ���Ҫ�����Ĳ�����</param>
        /// <param name="sender">�ؼ�����</param>
        /// <param name="Container">�����ؼ�</param>
        /// <param name="PageSize">��ҳ��С</param>
        /// <param name="style">����0 ����ҳ 1 ��ҳ </param>
        public static void AddControlToPanel(ArrayList alValues, Control sender, Control Container, Size PageSize, int style)
        {
            //{822BAB77-BCB4-4363-BF79-F9E7551D8DE4}
            //����ԭ�������� Type sender Ϊ Control sender
            IControlPrintable ic = null;
            try
            {
                ic = (IControlPrintable)sender;
            }
            catch { System.Windows.Forms.MessageBox.Show(sender.Name + "���߱���ӡ�ӿڣ����ܼ���!"); return; }

            Container.Controls.Clear();

            if (alValues.Count <= 0) return;

            Point intControlOffset = new Point(ic.BeginHorizontalBlankWidth, ic.BeginVerticalBlankHeight);

            if (ic.HorizontalNum == 0)
            {
                ic.HorizontalNum = PageSize.Width / ic.ControlSize.Width;
            }
            if (ic.VerticalNum == 0)
            {
                if (style == 0)
                    ic.VerticalNum = alValues.Count;//������ӡ����ɽһʹ�ã�
                else
                    ic.VerticalNum = PageSize.Height / ic.ControlSize.Height;//��ӡֽ�ţ���ͨ��
            }

            //��ӿؼ�
            int i = 0;
            for (int page = 0; page <= alValues.Count / (ic.VerticalNum * ic.HorizontalNum); page++)
            {
                for (int v = 0; v < ic.VerticalNum; v++)
                {
                    intControlOffset.Y = page * PageSize.Height + ic.BeginVerticalBlankHeight + v * ic.ControlSize.Height + v * ic.VerticalBlankHeight;

                    for (int h = 0; h < ic.HorizontalNum; h++)
                    {
                        string name = "control" + i.ToString();
                        intControlOffset.X = ic.BeginHorizontalBlankWidth + h * ic.ControlSize.Width + h * ic.HorizontalBlankWidth;

                        //{822BAB77-BCB4-4363-BF79-F9E7551D8DE4}
                        //����CreateComponent(sender,name) Ϊ CreateComponent(sender.GetType(),name)
                        Control c = (Control)Function.CreateComponent(sender.GetType(), name);
                        if (c == null)
                        {
                            System.Windows.Forms.MessageBox.Show("�޷����ɿؼ���" + sender.GetType().ToString());
                            return;
                        }
                        c.Size = ic.ControlSize;
                        c.Location = new Point(intControlOffset.X, intControlOffset.Y);

                        c.Visible = true;
                        ((IControlPrintable)c).ControlValue = alValues[i];
                        Container.Controls.Add(c);
                        i++;
                        if (i >= alValues.Count) return;
                    }
                }
            }
        }

        /// <summary>
        /// ����û��ؼ���ָ�������ؼ���
        /// </summary>
        /// <param name="alValues"></param>
        /// <param name="sender"></param>
        /// <param name="Container"></param>
        /// <param name="PageSize"></param>
        public static void AddControlToPanel(ArrayList alValues, Type sender, Control Container, Size PageSize)
        {
            #region �ؼ�
            IControlPrintable ic = null;
            try
            {
                ic = (IControlPrintable)Function.CreateComponent(sender, "mytest");
            }
            catch { System.Windows.Forms.MessageBox.Show(sender.Name + "���߱���ӡ�ӿڣ����ܼ���!"); return; }

            Container.Controls.Clear();

            if (alValues.Count <= 0) return;

            Point intControlOffset = new Point(ic.BeginHorizontalBlankWidth, ic.BeginVerticalBlankHeight);

            //��ӿؼ�
            int i = 0;
            int page = 0;
            int MaxHeight = 0;
            int MaxWidth = 0;
            Control c;

            while (true)//��ҳ��for(int page = 0;page <= alValues.Count/(ic.VerticalNum * ic.HorizontalNum);page++)
            {
                while (true)//for(int v=0;v<ic.VerticalNum;v++)//��
                {
                    while (true)//��for(int h =0;h<ic.HorizontalNum;h++)
                    {
                        string name = "control" + i.ToString();

                        c = (Control)Function.CreateComponent(sender, name);
                        if (c == null)
                        {
                            System.Windows.Forms.MessageBox.Show("�޷����ɿؼ���" + sender.GetType().ToString());
                            return;
                        }

                        c.Visible = true;
                        ((IControlPrintable)c).ControlValue = alValues[i];
                        c.Size = ((IControlPrintable)c).ControlSize;
                        Container.Controls.Add(c);

                        //intControlOffset.X = ic.BeginHorizontalBlankWidth  + h*ic.ControlSize.Width  +h*ic.HorizontalBlankWidth ;


                        if (c.Height > MaxHeight) MaxHeight = c.Height;

                        if (i == 0)//��һ��
                        {
                            intControlOffset.X = ic.BeginHorizontalBlankWidth;
                            intControlOffset.Y = ic.BeginVerticalBlankHeight;
                            MaxWidth = c.Width;
                            c.Location = new Point(intControlOffset.X, intControlOffset.Y);
                        }
                        else
                        {
                            if (intControlOffset.X + MaxWidth/*ǰһ�����*/+ c.Width/*��ǰ�Ŀ��*/ > PageSize.Width)
                            {
                                intControlOffset.X = ic.BeginHorizontalBlankWidth;
                                i++;
                                break;
                            }
                            intControlOffset.X += MaxWidth/*ǰһ�����*/+ ic.HorizontalBlankWidth;
                            MaxWidth = c.Width;
                            c.Location = new Point(intControlOffset.X, intControlOffset.Y);
                        }
                        i++;
                        if (i >= alValues.Count) return;

                    }
                    //intControlOffset.Y =page*PageSize.Height + ic.BeginVerticalBlankHeight + v*ic.ControlSize.Height +v*ic.VerticalBlankHeight;
                    if (intControlOffset.Y + MaxHeight/*ǰһҳ���߶�*/+ c.Height/*��ǰ�߶�*/> (page + 1) * PageSize.Height)
                    {
                        page++;
                        intControlOffset.Y = ic.BeginVerticalBlankHeight + page * PageSize.Height;
                    }
                    else
                    {
                        intControlOffset.Y += c.Height + ic.VerticalBlankHeight;
                    }
                    MaxWidth = c.Width;
                    c.Location = new Point(intControlOffset.X, intControlOffset.Y);
                    MaxHeight = 0;
                    if (i >= alValues.Count) return;
                }
            }
            #endregion
        }

        //
        //		/// <summary>
        //		/// ��ӿؼ���panel,���ݿؼ��ķ���ֵ���ж��Ƿ���Ӹÿؼ�
        //		/// </summary>
        //		/// <param name="alValues"></param>
        //		/// <param name="sender"></param>
        //		/// <param name="Container"></param>
        //		/// <param name="PageSize"></param>
        //		public static void AddControlToPanelIfNeed(ArrayList alValues,Control sender,Control Container,Size PageSize)
        //		{
        //			
        //			IControlPrintable ic = null;
        //			try
        //			{
        //				ic = (IControlPrintable)sender;
        //			}
        //			catch{System.Windows.Forms.MessageBox.Show(sender.Name +"���߱���ӡ�ӿڣ����ܼ���!"); return;}
        //			
        //			Container.Controls.Clear();
        //
        //			if( alValues.Count<=0 ) return;
        //
        //			Point intControlOffset = new Point(ic.BeginHorizontalBlankWidth,ic.BeginVerticalBlankHeight);
        //			
        //
        //			//��ӿؼ�
        //			int i = 0;
        //			int page = 0;
        //			int MaxHeight = 0;
        //			int MaxWidth = 0;
        //			Control c;
        //			
        //			while(true)//��ҳ��for(int page = 0;page <= alValues.Count/(ic.VerticalNum * ic.HorizontalNum);page++)
        //			{
        //				while(true)//for(int v=0;v<ic.VerticalNum;v++)//��
        //				{
        //					while(true)//��for(int h =0;h<ic.HorizontalNum;h++)
        //					{
        //						string name ="control"+ i.ToString();
        //						
        //						c = (Control)Function.CreateComponent(sender.GetType(),name);						
        //						if(c == null) 
        //						{
        //							System.Windows.Forms.MessageBox.Show("�޷����ɿؼ���"+sender.GetType().ToString());
        //							return;
        //						}
        //					
        //						c.Visible = true;
        //						((IControlPrintable)c).ControlValue = alValues[i];
        //						if(((IControlPrintable)c).ControlValue==null||((IControlPrintable)c).ControlValue.ToString()=="0")
        //						{						
        //							c.Size = ((IControlPrintable)c).ControlSize;
        //							Container.Controls.Add(c);
        //						 
        //							//intControlOffset.X = ic.BeginHorizontalBlankWidth  + h*ic.ControlSize.Width  +h*ic.HorizontalBlankWidth ;
        //						
        //						
        //							if(c.Height>MaxHeight) MaxHeight =c.Height;
        //
        //							if(i==0)//��һ��
        //							{
        //								intControlOffset.X = ic.BeginHorizontalBlankWidth;
        //								intControlOffset.Y = ic.BeginVerticalBlankHeight;
        //								MaxWidth = c.Width;
        //								c.Location = new Point(intControlOffset.X,intControlOffset.Y);
        //							}
        //							else
        //							{
        //								if(intControlOffset.X + MaxWidth/*ǰһ�����*/+c.Width/*��ǰ�Ŀ��*/ > PageSize.Width) 
        //								{
        //									intControlOffset.X = ic.BeginHorizontalBlankWidth;
        //									i++;
        //									break;
        //								}
        //								intControlOffset.X += MaxWidth/*ǰһ�����*/+ic.HorizontalBlankWidth;
        //								MaxWidth = c.Width;
        //								c.Location = new Point(intControlOffset.X,intControlOffset.Y);
        //							}
        //						}
        //
        //						i++;
        //						if(i>= alValues.Count) return;
        //				
        //					}
        //					//intControlOffset.Y =page*PageSize.Height + ic.BeginVerticalBlankHeight + v*ic.ControlSize.Height +v*ic.VerticalBlankHeight;
        //					if(intControlOffset.Y + MaxHeight/*ǰһҳ���߶�*/+c.Height/*��ǰ�߶�*/>(page+1)*PageSize.Height) 
        //					{
        //						page++;
        //						intControlOffset.Y = ic.BeginVerticalBlankHeight+page*PageSize.Height;
        //					}
        //					else
        //					{
        //						intControlOffset.Y += c.Height + ic.VerticalBlankHeight ;
        //					}
        //					MaxWidth = c.Width;
        //					c.Location = new Point(intControlOffset.X,intControlOffset.Y);
        //					MaxHeight =0;
        //					if(i>= alValues.Count) return;
        //				}
        //			}
        //		}
        //		

        /// <summary>
        /// �������ͣ����佨�����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static object CreateComponent(System.Type type, string Name)
        {
            try
            {
                System.Object c = System.Activator.CreateInstance(type, true);
                try
                {
                    ((Control)c).Name = Name;
                }
                catch
                {
                }
                return c;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��ӿؼ�
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static Control CreateControl(string dllName, string Name)
        {
            if (dllName.Trim() == "") return null;
            System.Runtime.Remoting.ObjectHandle s;
            string objectNameSpace = dllName;
            string objectName = Name;
            string name = objectName.Substring(objectName.LastIndexOf(".") + 1);
            Control control = null;
            try
            {
                s = System.Activator.CreateInstance(objectNameSpace, objectName, true, System.Reflection.BindingFlags.CreateInstance, null, null, null, null, null);
                control = (Control)s.Unwrap();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }

            return control;
        }

        /// <summary>
        /// �������Ը��ؼ�
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertys"></param>
        public static void SetPropertyToControl(Control control, ArrayList propertys)
        {
            foreach (Neusoft.FrameWork.Models.NeuObject obj in propertys)
            {
                if (obj.ID == "")
                {
                }
                else
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(control)[obj.ID];
                    if (prop != null)
                    {
                        bool isContent = prop.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content);
                        object value = prop.GetValue(control);
                        if (isContent)
                        {

                        }
                        else
                        {
                            try
                            {
                                if (prop.PropertyType.ToString() == "System.Drawing.Color")
                                {
                                    obj.Name = obj.Name.Replace("Color [", "");
                                    obj.Name = obj.Name.Substring(0, obj.Name.Length - 1);

                                    if (obj.Name.IndexOf("R") >= 0 && obj.Name.IndexOf("G") >= 0 && obj.Name.IndexOf("B") >= 0)
                                    {

                                    }
                                    else
                                    {
                                        value = Color.FromName(obj.Name);
                                    }
                                }
                                else
                                {
                                    value = prop.Converter.ConvertFromInvariantString(obj.Name);
                                }
                            }
                            catch
                            { }
                            prop.SetValue(control, value);

                        }
                    }
                }
            }
        }


        #endregion

        #region Ini�����ļ�
        [DllImport("kernel32")]
        /*
            * SegName   Ϊ���� 
            * KeyName   Ϊ������
            * Value     Ϊ������ֵ
            * FileName  Ϊ�ļ�����
            * ��ʽ��Ϊ�� [SegName] 
            *            KeyName = Value
            * */
        private static extern bool WritePrivateProfileString(string SegName, string KeyName, string Value, string FileName);
        /*
          * SegName  Ϊ���� 
         * KeyName   Ϊ������
         * Default   Ϊȡ����ʱ��Ĭ��ֵ
         * StrReturn Ϊ����ֵ
         * Size      ΪĿ�Ļ������Ĵ�С
         * FileName  Ϊ�ļ���
         * */
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string SegName, string KeyName, string Default, System.Text.StringBuilder StrReturn, int Size, string FileName);
        [DllImport("kernel32")]
        /*
         * SegName    Ϊ���� 
         * KeyName    Ϊ������
         * Default    ΪĬ��ֵ
         * FileName   Ϊ�ļ���
         * */
        private static extern int GetPrivateProfileInt(string SegName, string KeyName, int Default, string FileName);
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public static string FileName = Application.StartupPath + "\\HISSETTING.INI";
        /// <summary>
        /// ���String���Ͳ���ֵ(���ļ�·��)
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string ReadPrivateProfile(string SegName, string KeyName, string FileName)
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder(1000);
            try
            {
                GetPrivateProfileString(SegName, KeyName, "", name, 1000, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("��������ļ���Ϣ������쿴���ļ��Ƿ���ڣ�");
                return "";
            }
            return name.ToString();
        }
        /// <summary>
        /// ���String���Ͳ���ֵ
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static string ReadPrivateProfile(string SegName, string KeyName)
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder(1000);

            try
            {
                GetPrivateProfileString(SegName, KeyName, "", name, 1000, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("��������ļ���Ϣ������쿴���ļ��Ƿ���ڣ�");
                return "";
            }
            return name.ToString();
        }
        /// <summary>
        /// ���ò���ֵ
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <param name="Value"></param>
        public static void WritePrivateProfile(string SegName, string KeyName, string Value)
        {
            if (System.IO.File.Exists(FileName) == false)
            {
                {
                    System.IO.File.Create(FileName);
                }
            }
            try
            {
                WritePrivateProfileString(SegName, KeyName, Value, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("���ò���ֵ������쿴���ļ��Ƿ���ڣ�");
                return;
            }

            //this.ShowErr("���ò���ֵ�ɹ���");
        }
        /// <summary>
        ///  �������ò���ֵ(���ļ�·��)
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <param name="Value"></param>
        /// <param name="FileName"></param>
        public static void WritePrivateProfile(string SegName, string KeyName, string Value, string FileName)
        {
            if (!System.IO.File.Exists(FileName))
            {
                //�����ڵĻ��������ļ�
                System.IO.File.Create(FileName);
            }
            try
            {
                WritePrivateProfileString(SegName, KeyName, Value, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("���ò���ֵ������쿴���ļ��Ƿ���ڣ�");
                return;
            }

            //this.ShowErr("���ò���ֵ�ɹ���");
        }
        /// <summary>
        /// ���INT�Ͳ���ֵ
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <returns></returns>
        public static int WritePrivateProfileIntType(string SegName, string KeyName)
        {
            int Value;
            try
            {
                Value = GetPrivateProfileInt(SegName, KeyName, 0, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("��������ļ���Ϣ������쿴���ļ��Ƿ���ڣ�");
                return 0;
            }
            return Value;
        }
        /// <summary>
        /// ���ػ��INT�Ͳ���ֵ(�����ļ�·��)
        /// </summary>
        /// <param name="SegName"></param>
        /// <param name="KeyName"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static int WritePrivateProfileIntType(string SegName, string KeyName, string FileName)
        {
            int Value;
            try
            {
                Value = GetPrivateProfileInt(SegName, KeyName, 0, FileName);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("��������ļ���Ϣ������쿴���ļ��Ƿ���ڣ�");
                return 0;
            }
            return Value;
        }
        #endregion

        #region ��ʾToolTip
        /// <summary>
        /// ��ʾtooltip
        /// </summary>
        /// <param name="text"></param>
        /// <param name="time"></param>
        /// <param name="foreColor"></param>
        /// <param name="backColor"></param>
        /// <param name="opacity"></param>
        public static void ShowToolTip(string text, int time, Color foreColor, Color backColor, double opacity)
        {
            Neusoft.FrameWork.WinForms.Forms.frmTip f = new Neusoft.FrameWork.WinForms.Forms.frmTip();
            f.TipText = text;
            f.TopMost = true;
            f.ShowInTaskbar = false;
            f.Show();
            f.Run(time, foreColor, backColor, opacity);
        }
        /// <summary>
        /// ��ʾtooltip
        /// </summary>
        /// <param name="text"></param>
        /// <param name="time"></param>
        public static void ShowToolTip(string text, int time)
        {
            ShowToolTip(text, time, Color.Blue, Color.Yellow, 0.8);

        }

        /// <summary>
        /// ��ʾtooltip
        /// </summary>
        /// <param name="text"></param>
        public static void ShowToolTip(string text)
        {
            ShowToolTip(text, 5);
        }

        #endregion

        #region ����
        /// <summary>
        /// ��ǰ·��
        /// </summary>
        public static string CurrentPath = "";

        /// <summary>
        /// �����Ŀ¼
        /// </summary>
        public static string PluginPath = "Plugins\\";

        /// <summary>
        /// ��������Ŀ¼
        /// </summary>
        public static string SettingPath = "Profile\\";
        /// <summary>
        /// ��ʱĿ¼
        /// </summary>
        public static string TempPath = "Tmp\\";

        /// <summary>
        /// ������Ŀ¼
        /// </summary>
        public static string LanguagePath = "Languages\\";

        /// <summary>
        /// �������ļ�
        /// </summary>
        public static string LanguageFileName = "Language.xml";

        #endregion

        #region ImageList
        /// <summary>
        /// 
        /// </summary>
        public static Image GetImage(EnumImageList imageList)
        {

            if (imageList == EnumImageList.F��Ʊ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ֵ�;
            if (imageList == EnumImageList.B��ҩ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҩ��;
            if (imageList == EnumImageList.B��ҩ̨)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҩ̨;
            if (imageList == EnumImageList.P�̵�����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.T��챨��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��챨��;
            if (imageList == EnumImageList.T���Ǽ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���Ǽ�;
            if (imageList == EnumImageList.PR�˵���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���˵��ڵ�_��16;
            if (imageList == EnumImageList.PR�˵���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���˵��ڵ�_�ر�16;
            if (imageList == EnumImageList.D����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.D����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;

            if (imageList == EnumImageList.B����)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            }
            if (imageList == EnumImageList.B����)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            }
            if (imageList == EnumImageList.C��ѯ)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ѯ;
            }
            if (imageList == EnumImageList.C����)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            }
            if (imageList == EnumImageList.D��ӡ)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡ;
            }
            if (imageList == EnumImageList.D��ӡԤ��)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡԤ��;
            }
            if (imageList == EnumImageList.F����)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Copy;
            }
            if (imageList == EnumImageList.G�˿�)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            }
            if (imageList == EnumImageList.L���)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            }
            if (imageList == EnumImageList.MĬ��)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ĭ��;
            }
            if (imageList == EnumImageList.Q���)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            }
            if (imageList == EnumImageList.Qȡ��)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȡ��;
            }
            if (imageList == EnumImageList.Qȫѡ)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȫѡ;
            }
            if (imageList == EnumImageList.QȨ��)
            {
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ȩ��;
            }
            if (imageList == EnumImageList.R��Ա)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.R��Ա��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��Ա��;
            if (imageList == EnumImageList.Sɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ɾ��;
            if (imageList == EnumImageList.S��һ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��һ��;
            if (imageList == EnumImageList.S����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.Sˢ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ˢ��;
            if (imageList == EnumImageList.T���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.T�˳�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�˳�;
            if (imageList == EnumImageList.X��һ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��һ��;
            if (imageList == EnumImageList.X�½�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�½�;
            if (imageList == EnumImageList.X��Ϣ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��Ϣ;
            if (imageList == EnumImageList.X�޸�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�޸�;
            if (imageList == EnumImageList.YԤ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡԤ��;
            if (imageList == EnumImageList.Z�ݴ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ݴ�;
            if (imageList == EnumImageList.Zע��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ע��;
            if (imageList == EnumImageList.Z����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.Z������Ϣ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��Ϣ����;
            if (imageList == EnumImageList.J���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.J����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.B����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.C��ѯ��ʷ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ѯ��ʷ;
            if (imageList == EnumImageList.H�ϲ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ϲ�;
            if (imageList == EnumImageList.H����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.Yҽ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ҽ��;
            if (imageList == EnumImageList.Z���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.F����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.H���۱���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���۱���;
            if (imageList == EnumImageList.H����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.K����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.L��ʱ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ʱ��;
            if (imageList == EnumImageList.M��ϸ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ϸ;
            if (imageList == EnumImageList.Qȫ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȫ��;
            if (imageList == EnumImageList.Qȷ���շ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�շ�ȷ��;
            if (imageList == EnumImageList.R������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.������;
            if (imageList == EnumImageList.S��������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��������;
            if (imageList == EnumImageList.S�շ���Ŀ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�շ���Ŀ;
            if (imageList == EnumImageList.S�ֶ�¼��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ֹ�¼��;
            if (imageList == EnumImageList.Yҽ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ҽ��;
            if (imageList == EnumImageList.Zִ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȷ��;
            if (imageList == EnumImageList.C�ش�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ӡ;
            if (imageList == EnumImageList.Z�Զ�¼��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�Զ�¼��;
            if (imageList == EnumImageList.B����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.C��Ժ�Ǽ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ɾ����Ա;
            if (imageList == EnumImageList.C��λά��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.D��ӡ��Һ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡ��Һ��;
            if (imageList == EnumImageList.D��ӡִ�е�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡִ�е�;
            if (imageList == EnumImageList.F�ֽ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ֽ�;
            if (imageList == EnumImageList.H����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ʿ;
            if (imageList == EnumImageList.H��ҽʦ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҽ��;
            if (imageList == EnumImageList.J����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�����Ա;
            if (imageList == EnumImageList.T����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.Yҽ�����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.YӤ���Ǽ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ӥ���Ǽ�;
            if (imageList == EnumImageList.Z�ٻ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ٻ�;
            if (imageList == EnumImageList.Zת��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ת��;
            if (imageList == EnumImageList.Zת��ȷ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ת��ȷ��;
            if (imageList == EnumImageList.A����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.YҩƷ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ҩƷ;
            if (imageList == EnumImageList.YԤԼ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ԤԼ;
            if (imageList == EnumImageList.F����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.J����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.J��������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��������;
            if (imageList == EnumImageList.K����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.L��ʷ��Ϣ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ʷ��¼;
            if (imageList == EnumImageList.L¥��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.¥��;
            if (imageList == EnumImageList.L¥��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.¥��;
            if (imageList == EnumImageList.S�豸)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�豸;
            if (imageList == EnumImageList.C�ɹ���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ɹ���;
            if (imageList == EnumImageList.C���ⵥ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���ⵥ;
            if (imageList == EnumImageList.R��ⵥ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ⵥ;
            if (imageList == EnumImageList.S���뵥)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���뵥;

            //�󲹳��
            if (imageList == EnumImageList.B��������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��������;
            if (imageList == EnumImageList.C�˵�ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�˵�ɾ��;
            if (imageList == EnumImageList.C�˵����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�˵����;
            if (imageList == EnumImageList.C��ҩ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҩ;
            if (imageList == EnumImageList.C������Ա)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.������Ա;
            if (imageList == EnumImageList.C���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.C����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.C�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.C����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.C���󵥾�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���󵥾�;
            if (imageList == EnumImageList.D���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.D��ӡ�嵥)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ӡ�嵥;
            if (imageList == EnumImageList.F����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.F�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.G����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.G�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.H�ϲ�ȡ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ϲ�ȡ��;
            if (imageList == EnumImageList.J�ƻ���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ƻ���;
            if (imageList == EnumImageList.J������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.������;
            if (imageList == EnumImageList.J��ɫɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ɫɾ��;
            if (imageList == EnumImageList.J��ɫ���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ɫ���;
            if (imageList == EnumImageList.K����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.K�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.K�����޸�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�����޸�;
            if (imageList == EnumImageList.L�ۼƿ�ʼ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ۼƿ�ʼ;
            if (imageList == EnumImageList.L�ۼ�ȡ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ۼ�ȡ��;
            if (imageList == EnumImageList.L�ۼ�ֹͣ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ۼ�ֹͣ;
            if (imageList == EnumImageList.L���Ϊ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���Ϊ;
            if (imageList == EnumImageList.M��ϸɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ϸɾ��;
            if (imageList == EnumImageList.M��ϸ���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ϸ���;
            if (imageList == EnumImageList.Mģ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ģ��;
            if (imageList == EnumImageList.Mģ��ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ģ��ɾ��;
            if (imageList == EnumImageList.Mģ�����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ģ�����;
            if (imageList == EnumImageList.P����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.P�̵㸽��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�̵㸽��;
            if (imageList == EnumImageList.P�̵�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�̵�;
            if (imageList == EnumImageList.P�̵㿪ʼ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�̵㿪ʼ;
            if (imageList == EnumImageList.P�̵�ȡ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�̵�ȡ��;
            if (imageList == EnumImageList.P�̵�ֹͣ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�̵�ֹͣ;
            if (imageList == EnumImageList.P�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.P��������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��������;
            if (imageList == EnumImageList.Qȫ��ѡ)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȫ��ѡ;
            if (imageList == EnumImageList.Qȫѡȡ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ȫѡȡ��;
            if (imageList == EnumImageList.QȨ��ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ȩ��ɾ��;
            if (imageList == EnumImageList.QȨ�����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ȩ�����;
            if (imageList == EnumImageList.R��Ա�޸�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��Ա�޸�;
            if (imageList == EnumImageList.R����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.S�ϼ�����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ϼ�����;
            if (imageList == EnumImageList.S�豸ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�豸ɾ��;
            if (imageList == EnumImageList.S�豸���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�豸���;
            if (imageList == EnumImageList.T��ת)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ת;
            if (imageList == EnumImageList.Tͣ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ͣ��;
            if (imageList == EnumImageList.W��ɵ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ɵ�;
            if (imageList == EnumImageList.Wδ���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.δ���;
            if (imageList == EnumImageList.Wδ��ɵ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.δ��ɵ�;
            if (imageList == EnumImageList.Yһ�㵥��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.һ�㵥��;
            if (imageList == EnumImageList.Yһ����¼ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.һ����¼ɾ��;
            if (imageList == EnumImageList.Yһ����¼���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.һ����¼���;
            if (imageList == EnumImageList.Yҽ���˳�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ҽ���˳�;
            if (imageList == EnumImageList.Y�ѽ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ѽ��;
            if (imageList == EnumImageList.Z��������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��������;
            if (imageList == EnumImageList.Z���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.���;
            if (imageList == EnumImageList.Zֽ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ֽ��;
            if (imageList == EnumImageList.Z�Ӽ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�Ӽ�;
            if (imageList == EnumImageList.Z����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.Z�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.Z����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.G����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.B����ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����ɾ��;
            if (imageList == EnumImageList.B�������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�������;
            if (imageList == EnumImageList.F����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.B��ҩ̨ɾ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҩ̨ɾ��;
            if (imageList == EnumImageList.B��ҩ̨���)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.��ҩ̨���;
            if (imageList == EnumImageList.Mģ��ִ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.ģ��ִ��;
            if (imageList == EnumImageList.L�б�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�б�;

            if (imageList == EnumImageList.W����_����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_����;
            if (imageList == EnumImageList.W����_�±�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_�±�;
            if (imageList == EnumImageList.W����_�ϱ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_�ϱ�;
            if (imageList == EnumImageList.R����_Ӥ��_Ů)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_Ӥ��_Ů;
            if (imageList == EnumImageList.R����_Ӥ��_��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_Ӥ��_��;
            if (imageList == EnumImageList.R����_����_Ů)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_����_Ů;
            if (imageList == EnumImageList.R����_����_��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_����_��;
            if (imageList == EnumImageList.R����_��ͯ_Ů)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_��ͯ_Ů;
            if (imageList == EnumImageList.R����_��ͯ_��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_��ͯ_��;
            if (imageList == EnumImageList.R����_����_Ů)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_����_Ů;
            if (imageList == EnumImageList.R����_����_��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����_����_��;
            if (imageList == EnumImageList.S�ֹ�¼��ȡ��)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�ֹ�¼��ȡ��;
            if (imageList == EnumImageList.H����)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.����;
            if (imageList == EnumImageList.J������)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.������;
            if (imageList == EnumImageList.T�˷�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�˷�;
            if (imageList == EnumImageList.S�շ�)
                return global::Neusoft.FrameWork.WinForms.Properties.Resources.�շ�;

       



            return global::Neusoft.FrameWork.WinForms.Properties.Resources.Ĭ��;


        }

        #endregion

        #region ���ܽ��ܻ���������by luzhp@neusoft.com 2007-7-18
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="a_strString">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string Encrypt3DES(string a_strString)
        {
            try
            {
                TripleDESCryptoServiceProvider DES = new
                    TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                Encoding encoding = Encoding.UTF8;
                string a_strKey = "his";
                DES.Key = hashMD5.ComputeHash(encoding.GetBytes(a_strKey));
                DES.Mode = CipherMode.ECB;

                ICryptoTransform DESEncrypt = DES.CreateEncryptor();

                byte[] Buffer = encoding.GetBytes(a_strString);
                return Convert.ToBase64String(DESEncrypt.TransformFinalBlock
                    (Buffer, 0, Buffer.Length));
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="a_strString">Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string Decrypt3DES(string a_strString)
        {
            TripleDESCryptoServiceProvider DES = new
                TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            Encoding encoding = Encoding.UTF8;
            string a_strKey = "his";
            DES.Key = hashMD5.ComputeHash(encoding.GetBytes(a_strKey));
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result = "";
            try
            {
                byte[] Buffer = Convert.FromBase64String(a_strString);

                result = encoding.GetString(DESDecrypt.TransformFinalBlock
                    (Buffer, 0, Buffer.Length));
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// ��ȡԶ�������ļ�
        /// </summary>
        /// <param name="configFileName">�����ļ�����</param>
        /// <param name="strErr">������Ϣ</param>
        /// <returns>�ɹ�����Զ�������ļ���Ϣ ʧ�ܷ���null</returns>
        public static System.Xml.XmlDocument GetServerConfigFile(string configFileName, ref string strErr)
        {
            #region ��ȡ�����ļ�·��

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(Application.StartupPath + "\\url.xml");

            System.Xml.XmlNode node = doc.SelectSingleNode("//dir");
            if (node == null)
            {
                strErr = "url����dir������";
                return null;
            }

            string serverPath = node.InnerText;
            string configPath = "//" + configFileName; //Զ�������ļ��� 

            #endregion

            try
            {
                doc.Load(serverPath + configPath);
            }
            catch (System.Net.WebException)
            {

            }
            catch (System.IO.FileNotFoundException)
            {

            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return null;
            }

            return doc;
        }

        #region ת��fp��������
        /// <summary>
        /// ת��fp�������� by Nxy
        /// </summary>
        /// <param name="sheeView"></param>
        public static void ConvertFpHeader(FarPoint.Win.Spread.SheetView sheeView)
        {
            sheeView.SheetName = Neusoft.FrameWork.Management.Language.Msg(sheeView.SheetName);
            int sheeViewCount = sheeView.Columns.Count;

            if (sheeViewCount > 0)
            {
                for (int i = 0; i < sheeViewCount; i++)
                {
                    sheeView.Columns[i].Label = Neusoft.FrameWork.Management.Language.Msg(sheeView.Columns[i].Label);
                }
            }
        }
        #endregion

        public static Color GetSysColor(EnumSysColor sysColor)
        {
            if (sysColor == EnumSysColor.Blue)
            {
                return Color.FromArgb(190, 226, 224);
            }
            else if (sysColor == EnumSysColor.Green)
            {
                return Color.FromArgb(241, 247, 213);
            }
            else if (sysColor == EnumSysColor.LightBlue)
            {
                return Color.FromArgb(230, 245, 244);
            }
            else if (sysColor == EnumSysColor.DarkBlue)
            {
                return Color.FromArgb(0, 155, 180);
            }
            else if (sysColor == EnumSysColor.DarkGreen)
            {
                return Color.FromArgb(183, 210, 0);
            }
            return Color.White;
        }

        #region ����FarPoint��ʽ
        /// <summary>
        /// ͳһ����FarFoint��SheetView�ĸ�ʽ
        /// </summary>
        public static void SetFarPointStyle(FarPoint.Win.Spread.SheetView sheet)
        {
            sheet.VisualStyles = VisualStyles.Off;
            sheet.ColumnHeader.DefaultStyle.BackColor = Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.LightBlue);
            sheet.SheetCornerStyle.BackColor = Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(Neusoft.FrameWork.WinForms.Classes.EnumSysColor.LightBlue);
            if (sheet.RowHeader.ColumnCount > 0)
            {
                sheet.RowHeader.DefaultStyle.BackColor = Color.White;
                sheet.RowHeader.Columns.Get(0).Width = 45;
            }
            if (sheet.RowHeader.Rows.Count > 0)
            {
                sheet.ColumnHeader.Rows.Get(0).Height = 40;
            }
            sheet.GrayAreaBackColor = Color.White;
        }

        /// <summary>
        /// ͳһ����FarFoint�ĸ�ʽ
        /// </summary>
        public static void SetFarPointStyle(FarPoint.Win.Spread.FpSpread spread)
        {
            if (spread == null) return;
            spread.VisualStyles = VisualStyles.Off;
            for (int i = 0; i < spread.Sheets.Count; i++)
            {
                SetFarPointStyle(spread.Sheets[i]);
            }
        }
        #endregion

        #region ����TabControl��ʽ
        /// <summary>
        /// ͳһ����TabControl�ؼ���ʽ
        /// </summary>
        /// <param name="tabControl"></param>
        public static void SetTabControlStyle(TabControl tabControl)
        {
            tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += new DrawItemEventHandler(tabControl_DrawItem);
            tabControl.Margin = new System.Windows.Forms.Padding(0);

            //���ÿؼ���Pageҳ�ļ��
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.Margin = new System.Windows.Forms.Padding(0);
                tabPage.Padding = new System.Windows.Forms.Padding(0);
                tabPage.BackColor = Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(EnumSysColor.Green);
            }
        }

        private static void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            TabControl current = (sender as TabControl);
            Graphics g = e.Graphics;
            Rectangle endPageRect = current.GetTabRect(current.TabPages.Count - 1); //���һ���������ķ�Χ
            Rectangle TitleRect = current.GetTabRect(e.Index);              //��ǰ�������ķ�Χ
            Rectangle HeaderBackRect = Rectangle.Empty;             //��������

            switch (current.Alignment)
            {
                case TabAlignment.Top:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X + endPageRect.Width, endPageRect.Y),
                        new Size(current.Width - endPageRect.X - endPageRect.Width, endPageRect.Height));
                    break;
                case TabAlignment.Bottom:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X + endPageRect.Width, endPageRect.Y),
                        new Size(current.Width - endPageRect.X - endPageRect.Width, endPageRect.Height));
                    break;
                case TabAlignment.Left:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X, endPageRect.Y + endPageRect.Height),
                        new Size(endPageRect.Width, current.Height - endPageRect.Y - endPageRect.Height));
                    break;
                case TabAlignment.Right:
                    HeaderBackRect = new Rectangle(new Point(endPageRect.X, endPageRect.Y + endPageRect.Height),
                        new Size(endPageRect.Width, current.Height - endPageRect.Y - endPageRect.Height));
                    break;
            }



            Brush TitleBackBrush = new SolidBrush(Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(EnumSysColor.Blue));

            g.FillRectangle(TitleBackBrush, TitleRect);

            Font font = e.Font;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Far;
            Color fontcolor = System.Drawing.Color.Black;

            if (current.SelectedIndex == e.Index)    //������Ƶı������ѡ�еı��⣬��ʹ��ѡ�б�������壬ͬʱ����font��fontcolor
            {
                g.DrawRectangle(new Pen(current.TabPages[e.Index].BackColor), TitleRect);    //����ѡ�б���ľ��η���
                font = e.Font;
                fontcolor = Color.Blue;
            }
            Brush fontbrush = new SolidBrush(fontcolor);
            //���Ʊ����ı�
            g.DrawString(current.TabPages[e.Index].Text, font, fontbrush, TitleRect, sf);

            //���Ʊ���
            if (HeaderBackRect != Rectangle.Empty)
            {
                Brush HeaderBackBrush = new SolidBrush(Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(EnumSysColor.Green));
                g.FillRectangle(HeaderBackBrush, HeaderBackRect);
            }

        }
        #endregion

        #region ����ListView��ʽ

        public static void SetListViewStyle(ListView listView)
        {
            if (listView == null) return;
            listView.OwnerDraw = true;
            if (listView.Columns.Count > 0)
            {
                listView.Columns[listView.Columns.Count - 1].Width = listView.Size.Width;
            }
                listView.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(ListView_ColumnHeader);
                listView.DrawItem += new DrawListViewItemEventHandler(ListView_DrawItem);
                listView.DrawSubItem += new DrawListViewSubItemEventHandler(ListView_DrawSubItem);
            
        }

        private static void ListView_ColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {

            ListView current = sender as ListView;

            Graphics g = e.Graphics;
            Rectangle ColumnTitleBack = e.Bounds;
            ColumnTitleBack.Width = current.Size.Width;
            ColumnTitleBack.Height = ColumnTitleBack.Height - 1;

            e.Graphics.FillRectangle(new SolidBrush(Neusoft.FrameWork.WinForms.Classes.Function.GetSysColor(EnumSysColor.LightBlue)), ColumnTitleBack);

            Font font = e.Font;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Far;
            Color fontcolor = System.Drawing.Color.Black;
            g.DrawString(e.Header.Text, font, new SolidBrush(fontcolor), e.Bounds, sf);
        }

        private static void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;	//����ϵͳĬ�Ϸ�ʽ������

        }

        private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;	//����ϵͳĬ�Ϸ�ʽ��������

        }
        #endregion
    }

    /// <summary>
    /// ͼ���б�
    /// </summary>
    public enum EnumImageList
    {
        T�˷�=58,
        S�շ�=59,
        J������ = 60,
        H���� = 61,
        W����_���� = 62,
        W����_�±� = 63,
        W����_�ϱ� = 64,
        R����_Ӥ��_Ů = 65,
        R����_Ӥ��_�� = 66,
        R����_����_Ů = 67,
        R����_����_�� = 68,
        R����_��ͯ_Ů = 69,
        R����_��ͯ_�� = 70,
        R����_����_Ů = 71,
        R����_����_�� = 72,
        S�ֹ�¼��ȡ�� = 73,

        L�б�=74,
        B��ҩ̨ɾ��=75,
        B��ҩ̨���=76,
        Mģ��ִ��=77,
        F����=78,
        B�������=79,
        B����ɾ��=80,
        G���� = 81,
        B��ҩ̨ = 82,
        B��ҩ�� = 83,
        PR�˵��� = 84,
        PR�˵��� = 85,
        T�ײ� = 86,
        T���Ǽ� = 87,
        T��챨�� = 88,
        P�̵����� = 89,
        F��Ʊ = 90,
        F���� = 91,
        D���� = 92,//��92��ʼȫ���ع�����һ����ĸ�Ǻ��ֵĵ�һ���ֵ�ƴ��
        D���� = 93,
        MĬ�� = 94,
        T��� = 95,
        X�޸� = 96,
        Sɾ�� = 97,
        B���� = 98,
        D��ӡ = 99,
        D��ӡԤ�� = 100,
        T�˳� = 101,
        C��ѯ = 102,
        Z������Ϣ = 103,
        A���� = 104,
        B���� = 107,
        B���� = 108,
        B���� = 109,
        C��ѯ��ʷ = 110,
        C���� = 111,
        C��Ժ�Ǽ� = 112,
        C��λά�� = 113,
        D��ӡ��Һ�� = 114,
        D��ӡִ�е� = 115,
        F���� = 116,
        F�ֽ� = 117,
        F���� = 120,
        G�˿� = 121,
        H�ϲ� = 122,
        H���� = 123,
        H���� = 124,
        H���۱��� = 125,
        H���� = 126,
        H��ҽʦ = 127,
        J���� = 128,
        J�������� = 129,
        J���� = 130,
        J��� = 131,
        J���� = 132,
        K���� = 133,
        K���� = 134,
        L��ʷ��Ϣ = 135,
        L��ʱ�� = 136,
        L��� = 137,
        L¥�� = 138,
        L¥�� = 139,
        M��ϸ = 140,
        Q��� = 142,
        Qȡ�� = 143,
        Qȫ�� = 144,
        Qȫѡ = 145,
        QȨ�� = 146,
        Qȷ���շ� = 147,
        R��Ա = 148,
        R��Ա�� = 149,
        R������ = 150,
        S��һ�� = 151,
        S�豸 = 152,
        S���� = 153,
        S�������� = 154,
        S�շ���Ŀ = 155,
        S�ֶ�¼�� = 156,
        Sˢ�� = 157,
        T���� = 161,
        X��һ�� = 162,
        X�½� = 163,
        X��Ϣ = 164,
        YҩƷ = 165,
        Yҽ�� = 166,
        Yҽ�� = 167,
        Yҽ����� = 168,
        YӤ���Ǽ� = 169,
        YԤ�� = 170,
        YԤԼ = 171,
        Z�ݴ� = 172,
        Z�ٻ� = 173,
        Z��� = 174,
        Zִ�� = 175,
        C�ش� = 176,
        Zע�� = 177,
        Zת�� = 178,
        Zת��ȷ�� = 179,
        Z�Զ�¼�� = 180,
        Z���� = 181,
        C�ɹ��� = 182,
        C���ⵥ = 183,
        R��ⵥ = 184,
        S���뵥 = 185,
        B�������� = 186,//�󲹳��
        C�˵�ɾ�� = 187,
        C�˵���� = 188,
        C��ҩ = 189,
        C������Ա = 190,
        C��� = 191,
        C���� = 192,
        C������� = 193,
        C����ɾ�� = 194,
        C���󵥾� = 195,
        D��� = 196,
        D��ӡ�嵥 = 197,
        F����ɾ�� = 198,
        F������� = 199,
        G����ɾ�� = 200,
        G������� = 201,
        H�ϲ�ȡ�� = 202,
        J�ƻ��� = 203,
        J������ = 204,
        J��ɫɾ�� = 205,
        J��ɫ��� = 206,
        K����ɾ�� = 207,
        K������� = 208,
        K�����޸� = 209,
        L�ۼƿ�ʼ = 210,
        L�ۼ�ȡ�� = 211,
        L�ۼ�ֹͣ = 212,
        L���Ϊ = 213,
        M��ϸɾ�� = 214,
        M��ϸ��� = 215,
        Mģ�� = 216,
        Mģ��ɾ�� = 217,
        Mģ����� = 218,
        P���� = 219,
        P�̵㸽�� = 220,
        P�̵� = 221,
        P�̵㿪ʼ = 222,
        P�̵�ȡ�� = 223,
        P�̵�ֹͣ = 224,
        P������� = 225,
        P�������� = 226,
        Qȫ��ѡ = 227,
        Qȫѡȡ�� = 228,
        QȨ��ɾ�� = 229,
        QȨ����� = 230,
        R��Ա�޸� = 231,
        R���� = 232,
        S�ϼ����� = 233,
        S�豸ɾ�� = 234,
        S�豸��� = 235,
        T��ת = 236,
        Tͣ�� = 237,
        W��ɵ� = 238,
        Wδ��� = 239,
        Wδ��ɵ� = 240,
        Yһ�㵥�� = 241,
        Yһ����¼ɾ�� = 242,
        Yһ����¼��� = 243,
        Yҽ���˳� = 244,
        Y�ѽ�� = 245,
        Z�������� = 246,
        Z��� = 247,
        Zֽ�� = 248,
        Z�Ӽ� = 249,
        Z����ɾ�� = 250,
        Z������� = 251,
        Z���� = 252


    }

    /// <summary>
    /// �������Ŀ¼
    /// </summary>
    public enum EnumPlugin
    {
        /// <summary>
        /// ҽ��
        /// </summary>
        SI,

        /// <summary>
        /// LIS
        /// </summary>
        LIS,

        /// <summary>
        /// PACS
        /// </summary>
        PACS,

        /// <summary>
        /// ������ҩ
        /// </summary>
        PASS,

        /// <summary>
        /// ��֤
        /// </summary>
        CA,

        /// <summary>
        /// ����
        /// </summary>
        BANK,

        /// <summary>
        /// ������
        /// </summary>
        TOOLBAR



    }

    public enum EnumSysColor
    {
        Blue = 0,
        DarkBlue = 3,
        LightBlue = 4,
        Green = 1,
        DarkGreen = 2,

    }



}
