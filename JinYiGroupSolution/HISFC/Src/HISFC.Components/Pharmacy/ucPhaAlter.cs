using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.Pharmacy
{
    /// <summary>
    /// [��������: ҩƷ������]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-12]<br></br>
    /// <�޸ļ�¼>
    ///    1.���������Ϳ��������Ч��У�� by Sunjh 2010-9-6 {9ED65013-E342-48b6-BB6B-6AB2D7CD5058}
    /// </�޸ļ�¼>
    /// </summary>
    public partial class ucPhaAlter : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucPhaAlter()
        {
            InitializeComponent();
        }

        #region �����

        /// <summary>
        /// �ⷿ����
        /// </summary>
        private string deptCode;

        /// <summary>
        /// ���������ļ��� �������Ϣ
        /// </summary>
        private ArrayList alInfo;

        /// <summary>
        /// ������ϸ��Ϣ
        /// </summary>
        private List<Neusoft.FrameWork.Models.NeuObject> expandList = new List<Neusoft.FrameWork.Models.NeuObject>();

        /// <summary>
        /// �Ƿ�ʹ�������б�
        /// </summary>
        private bool isExpandList = false;

        /// <summary>
        /// �������ֵ
        /// </summary>
        public DialogResult rs = DialogResult.Cancel;

        #endregion

        #region ����

        /// <summary>
        /// �ⷿ����
        /// </summary>
        public string DeptCode
        {
            set
            {
                this.deptCode = value;
            }
        }

        /// <summary>
        /// ���������ļ��� �������Ϣ
        /// </summary>
        public ArrayList ApplyInfo
        {
            get
            {
                if (this.alInfo == null)
                    this.alInfo = new ArrayList();
                return this.alInfo;
            }
        }

        /// <summary>
        /// ������ϸ��Ϣ
        /// </summary>
        public List<Neusoft.FrameWork.Models.NeuObject> ExpandList
        {
            get
            {
                return this.expandList;
            }
        }

        /// <summary>
        /// �Ƿ�ʹ�������б�  {F4D82F23-CCDC-45a6-86A1-95D41EF856B8} ������������
        /// </summary>
        public bool IsQueryExpandData
        {
            get
            {
                return this.isExpandList;
            }
            set
            {
                this.isExpandList = value;

                if (value)
                {
                    this.rbAllDept.Visible = false;
                }
                else
                {
                    this.rbAllDept.Visible = true;
                }
            }
        }

        /// <summary>
        /// �������ֵ
        /// </summary>
        public DialogResult Result
        {
            get
            {
                return this.rs;
            }
            set
            {
                this.rs = value;
            }
        }

        /// <summary>
        /// ͳ����ʼʱ��
        /// </summary>
        private DateTime DtBegin
        {
            get
            {
                return NConvert.ToDateTime(this.dtpBegin.Text);
            }
        }

        /// <summary>
        /// ͳ�ƽ�ֹʱ��
        /// </summary>
        private DateTime DtEnd
        {
            get
            {
                return NConvert.ToDateTime(this.dtpEnd.Text);
            }
        }

        /// <summary>
        /// ��߿������
        /// </summary>
        private int MaxAlterDays
        {
            get
            {
                return NConvert.ToInt32(this.txtMaxDays.Text);
            }
        }

        /// <summary>
        /// ��Ϳ������
        /// </summary>
        private int MinAlterDays
        {
            get
            {
                return NConvert.ToInt32(this.txtMinDays.Text);
            }
        }

        #endregion


        /// <summary>
        /// ��������ֵ�Ŀⷿ���� ������Ϣ��ʾ
        /// </summary>
        public void SetData()
        {
            Neusoft.HISFC.BizLogic.Pharmacy.Constant consMgr = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
            Neusoft.HISFC.Models.Pharmacy.DeptConstant deptCons = consMgr.QueryDeptConstant(this.deptCode);
            if (deptCons == null)
            {
                MessageBox.Show(Language.Msg("��ȡ���ҳ�����������! \n" + consMgr.Err));
                return;
            }
            this.dtpEnd.Value = consMgr.GetDateTimeFromSysDateTime().Date.AddDays(1).AddSeconds(-1);
            this.dtpBegin.Value = this.dtpEnd.Value.AddDays(-deptCons.ReferenceDays);
            this.txtMaxDays.Text = deptCons.StoreMaxDays.ToString();
            this.txtMinDays.Text = deptCons.StoreMinDays.ToString();
            this.lbIntervalDays.Text = deptCons.ReferenceDays.ToString() + "��";
        }

        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        /// <returns>�ɹ�����True  ���򷵻�False</returns>
        protected bool SaveValid()
        {
            if (this.MaxAlterDays == 0 || this.MinAlterDays == 0)
            {
                MessageBox.Show("������������߲���Ϊ��");
                return false;
            }
            if (this.MaxAlterDays < this.MinAlterDays)
            {
                MessageBox.Show("��߿����������С����Ϳ������");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ȷ�ϲ������� �����Ĵ���
        /// 
        /// {F4D82F23-CCDC-45a6-86A1-95D41EF856B8} ���ĺ������Ƽ����ú���
        /// </summary>
        protected void QueryDayAlterList()
        {
            if (this.SaveValid())
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڰ��������Ľ��м��� ���Ժ�...");
                Application.DoEvents();

                Neusoft.HISFC.BizLogic.Pharmacy.Item itemMgr = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

                this.alInfo = itemMgr.QueryDrugListByDayAlter(this.deptCode, this.DtBegin, this.DtEnd, this.MaxAlterDays, this.MinAlterDays,this.rbAllDept.Checked);
                if (this.alInfo == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("������������Ϣ��������" + itemMgr.Err);
                    return;
                }

                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
        }

        /// <summary>
        /// ����������Ϣͳ��
        /// 
        ///  {F4D82F23-CCDC-45a6-86A1-95D41EF856B8} ���ĺ������Ƽ����ú���
        /// </summary>
        protected void QueryDeptExpandData()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ���ҩƷ����ͳ�� ���Ժ�...");
            Application.DoEvents();

            Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
            this.expandList = itemManager.FindByExpand(this.deptCode,this.DtBegin, this.DtEnd);
            if (this.expandList == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ȡ����������Ϣ��������") + itemManager.Err);
                return;
            }

            foreach (Neusoft.FrameWork.Models.NeuObject info in this.expandList)
            {
                info.User02 = (System.Math.Ceiling(NConvert.ToDecimal(info.User01) / (this.DtEnd - this.DtBegin).Days * NConvert.ToDecimal(this.txtMinDays.Text))).ToString();
                info.User03 = (System.Math.Ceiling(NConvert.ToDecimal(info.User01) / (this.DtEnd - this.DtBegin).Days  * NConvert.ToDecimal(this.txtMaxDays.Text))).ToString();
            }

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        /// <summary>
        /// ���ڹر�
        /// </summary>
        public void Close()
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }

        /// <summary>
        /// ���ý���
        /// </summary>
        public new void Focus()
        {
            this.dtpBegin.Focus();
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            if (this.DtEnd < this.DtBegin)
            {
                MessageBox.Show("��߿����������С����Ϳ������!");
                this.dtpEnd.Text = this.DtBegin.ToString();
            }
            this.lbIntervalDays.Text = (this.DtEnd - this.DtBegin).Days + "��";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DtEnd < this.DtBegin)
            {
                MessageBox.Show("ͳ����ʼ���ڲ��ܴ���ͳ�ƽ�ֹ����!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //���������Ϳ��������Ч��У�� by Sunjh 2010-9-6 {9ED65013-E342-48b6-BB6B-6AB2D7CD5058}
            if (this.MaxAlterDays < this.MinAlterDays)
            {
                MessageBox.Show("��߿����������С����Ϳ������!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.isExpandList)
            {                
                this.QueryDeptExpandData();
            }
            else
            {
                this.txtMinDays.Enabled = true;
                this.txtMinDays.Enabled = true;
                this.QueryDayAlterList();
            }

            this.rs = DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.rs = DialogResult.Cancel;

            this.Close();
        }
    }
}
