using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Material.MonthStore
{
    /// <summary>
    /// [��������:���ñ����½�ʱ��]
    /// [�� �� ��: ��ά]
    /// [����ʱ��: 2008-03]
    /// </summary>
    public partial class ucMonthStoreSet : UserControl
    {
        public ucMonthStoreSet()
        {
            InitializeComponent();
        }

        private DialogResult result = DialogResult.Cancel;

        /// <summary>
        /// �������
        /// </summary>
        public DialogResult Result
        {
            get
            {
                return this.result;
            }
        }

        /// <summary>
        /// �´�ִ��ʱ��
        /// </summary>
        public DateTime NextTime
        {
            get
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtpEnd.Text);
            }
        }

        /// <summary>
        /// �ر�
        /// </summary>
        private void Close()
        {
            if(this.ParentForm != null)
            {
                this.ParentForm.Close();
            }
        }

        /// <summary>
        /// Job����
        /// </summary>
        /// <param name="job"></param>
        public void SetJob(Neusoft.HISFC.Models.Base.Job job, DateTime maxDate)
        {
            this.lbInfo.Text = string.Format("�ϴ��½����ʱ��Ϊ{0} ", job.LastTime);

            this.dtpEnd.Value = maxDate;

            this.dtpEnd.MinDate = job.LastTime.AddSeconds(1);

            this.dtpEnd.MaxDate = maxDate;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.Cancel;
            this.Close();
        }
    }
}