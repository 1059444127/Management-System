using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Components.Account.Forms
{
    public partial class frmFinPassWord : Neusoft.FrameWork.WinForms.Forms.BaseForm
    {
        public frmFinPassWord()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "tbEdit":
                    {
                        this.ucFindAccountPassWord1.EditPassWord();
                        break;
                    }
                case "tbClear":
                    {
                        this.ucFindAccountPassWord1.Clear();
                        break;
                    }
                case "tbClose":
                    {
                        this.Close();
                        break;
                    }
            }
        }

        private void frmFinPassWord_Load(object sender, EventArgs e)
        {
            #region Ȩ������
            Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager user = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager();
            NeuObject dept = (user.Operator as HISFC.Models.Base.Employee).Dept;
            //�ж��Ƿ����ע������Ȩ��
            bool blpri = user.JudgeUserPriv(user.Operator.ID, dept.ID, "5101", "01");
            if (!blpri)
            {
                MessageBox.Show("��������ע���ʻ�����Ȩ�ޣ�");
                this.ucFindAccountPassWord1.Enabled = false;
            }
            #endregion
        }
    }
}