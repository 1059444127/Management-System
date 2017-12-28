using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.Material.MonthStore
{
    /// <summary>
    /// [��������:���ñ����½�ʱ��]
    /// [�� �� ��:��ά]
    /// [����ʱ��:2008-03]
    /// </summary>
    public partial class ucMonthStore : Neusoft.FrameWork.WinForms.Forms.BaseForm
    {
        public ucMonthStore()
        {
            InitializeComponent();
        }

        #region �����
        /// <summary>
        /// ������
        /// </summary>
        Neusoft.HISFC.BizLogic.Manager.Job jobManager = new Neusoft.HISFC.BizLogic.Manager.Job();

        /// <summary>
        /// ������
        /// </summary>
        Neusoft.HISFC.BizLogic.Material.MetItem metItemManager = new Neusoft.HISFC.BizLogic.Material.MetItem();

        /// <summary>
        /// ���ʹ�����
        /// </summary>
        Neusoft.HISFC.BizLogic.Material.Store matManager = new Neusoft.HISFC.BizLogic.Material.Store();

        /// <summary>
        /// �����½�����
        /// </summary>
        Neusoft.HISFC.Models.Base.Job job = new Neusoft.HISFC.Models.Base.Job();

        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        DateTime sysTime = System.DateTime.MinValue;

        /// <summary>
        /// �����½�Ȩ��
        /// </summary>
        bool isMonthStorePriv = false;

        /// <summary>
        /// Ȩ�޿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// Ȩ����Ա
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privOper = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ���幦��
        /// </summary>
        private Neusoft.HISFC.Models.IMA.EnumModuelType winFun = Neusoft.HISFC.Models.IMA.EnumModuelType.Material;

        /// <summary>
        /// �½�����ַ���
        /// </summary>
        private string monthStoreType = "MAT_MS";
        #endregion

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            this.job = this.jobManager.GetJob(this.monthStoreType);
            if (this.job == null)
            {
                MessageBox.Show(Language.Msg("���������½�����ȡ�����½�����ʧ��"));
                return;
            }
            if (this.job.ID != "")
            {
                if (this.job.Type == "0")
                {
                    this.cmbType.Text = "�ֶ�";
                }
                else
                {
                    this.cmbType.Text = "�Զ�";
                }
                this.dtpLast.Enabled = false;
            }
            else
            {
                this.job.ID = this.monthStoreType;
                this.job.Name = "ȫԺ�½�";
                this.job.State.ID = "M";
                this.job.LastTime = this.sysTime.AddMonths(-1);
                this.job.NextTime = this.sysTime;
                this.job.Type = "0";
                this.job.IntervalDays = 30;
                this.job.Department.ID = "0";

                this.cmbType.Text = "�Զ�";
                this.jobManager.SetJob(this.job);
            }
            this.dtpLast.Value = this.job.LastTime;
            this.dtpNext.Value = this.job.NextTime;
        }

        /// <summary>
        /// �����½�ʱ������
        /// </summary>
        /// <returns></returns>
        private bool JudgeMonthStoreTime()
        {
            DialogResult rs = MessageBox.Show(Language.Msg("�ϴ��½�ʱ��Ϊ" + this.job.LastTime.ToString() + "\nȷ�����ڽ����½���"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (rs == DialogResult.No)
            {
                return false;
            }
            rs = MessageBox.Show(Language.Msg("�Ƿ�����½����ʱ������ ѡ��'��' �����½��ֹʱ�� ѡ��'��' �����½����ʱ��Ϊ��ǰ����"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (rs == DialogResult.Yes)
            {
                ucMonthStoreSet uc = new ucMonthStoreSet();
                uc.SetJob(this.job.Clone(), this.sysTime.AddSeconds(-1));

                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);

                if (uc.Result == DialogResult.Cancel)
                {
                    return false;
                }

                this.job.NextTime = uc.NextTime;
            }
            else
            {
                //��һ�� ��֤�洢�������ж�ʱ���Ƿ��ܹ��½�ʱ ��������ִ��
                this.job.NextTime = this.sysTime.AddSeconds(-1);
            }

            //����Com_Job���е��´��½�ʱ���ֶΣ�ʵ���½�ʱ���ѡ
            if (this.jobManager.SetJob(this.job) != 1)
            {
                MessageBox.Show(Language.Msg("���ݵ�ǰʱ�������½�ʱ�� ��������"));
                return false;
            }

            return true;
        }

        /// <summary>
        /// ִ���½�
        /// </summary>
        /// <returns></returns>
        public int SaveMATMS()
        {
            DialogResult rs = MessageBox.Show(Language.Msg("ȷ�Ͻ����½������"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RightAlign);

            if(rs == DialogResult.No)
            {
                return -1;
            }

            if(!this.SaveMATMSVlaid())
            {
                return -1;
            }

            if(!this.JudgeMonthStoreTime())
            {
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Language.Msg("���ڽ����½ᴦ��.�����Ⱥ�..."));
            Application.DoEvents();

            try
            {
                this.matManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                if(this.matManager.ExePrcForMonthStore(this.privOper.ID) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Function.ShowMsg("�½����ʧ��" + this.matManager.Err);
                    return -1;
                }
            }
            catch (System.Exception ex)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                Function.ShowMsg("�½����ʧ��" + ex.Message);
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();
            Function.ShowMsg("�½�����ɹ�");

            #region ����Com_Job�� �����´��½�ʱ��

            this.job.LastTime = this.job.NextTime;
            this.job.NextTime = this.job.NextTime.AddMonths(1);

            if (this.jobManager.SetJob(this.job) != 1)
            {
                MessageBox.Show(Language.Msg("���ݵ�ǰʱ�������½�ʱ�� ��������"));
                return -1;
            }

            #endregion

            return 1;

        }

        /// <summary>
        /// �ж��Ƿ����δ�����̵㵥[��ʱû�������]
        /// </summary>
        /// <returns></returns>
        private bool SaveMATMSVlaid()
        {
            //�ж�ȫԺ���̵���� 
            ArrayList checkAl = this.metItemManager.QueryCheckStatic("A", "0");
            if (checkAl == null)
            {
                MessageBox.Show(Language.Msg("��ȡ�̵㵥��Ϣ ��������"));
                return false;
            }
            if (checkAl.Count > 0)
            {
                DialogResult rs = MessageBox.Show(Language.Msg("������δ�����̵㵥 �Ƿ���������½�"), "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (rs == DialogResult.No)
                {
                    Neusoft.FrameWork.Models.NeuObject info = new Neusoft.FrameWork.Models.NeuObject();

                    Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(checkAl, ref info);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// �½�����
        /// </summary>
        private void Set()
        {
            DialogResult result;
            //��ʾ�û�ѡ���Ƿ����
            result = MessageBox.Show(Language.Msg("ȷ�Ͻ����Զ�/�ֶ��½�������"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);
            if (result == DialogResult.No)
            {
                return;
            }

            if (this.cmbType.Text == "�ֶ�")
                this.job.Type = "0";
            else
                this.job.Type = "1";

            if (this.jobManager.SetJob(this.job) == -1)
            {
                MessageBox.Show(Language.Msg("Jobʵ�屣��ʧ��"));
            }

            MessageBox.Show(Language.Msg("����ɹ�"));
        }

        /// <summary>
        /// ����
        /// </summary>
        public virtual int Save()
        {
            switch (this.winFun)
            {
                case Neusoft.HISFC.Models.IMA.EnumModuelType.Material:           //����
                case Neusoft.HISFC.Models.IMA.EnumModuelType.All:
                    if (this.isMonthStorePriv)
                    {
                        return this.SaveMATMS();
                    }
                    break;
                case Neusoft.HISFC.Models.IMA.EnumModuelType.Phamacy:          //ҩƷ
                    break;
                case Neusoft.HISFC.Models.IMA.EnumModuelType.Equipment:         //�豸
                    break;
            }

            return 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //�жϲ���Ա�Ƿ�ӵ��Ȩ�ޣ����û������������˴���
                List<Neusoft.FrameWork.Models.NeuObject> alPrivDept = Neusoft.HISFC.Components.Common.Classes.Function.QueryPrivList("0503", true);
                if (alPrivDept == null || alPrivDept.Count == 0)
                    return;

                this.isMonthStorePriv = true;

                this.sysTime = this.matManager.GetDateTimeFromSysDateTime();

                this.Init();
            }

            this.cmbType.Enabled = false;

            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.Save() == 1)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
