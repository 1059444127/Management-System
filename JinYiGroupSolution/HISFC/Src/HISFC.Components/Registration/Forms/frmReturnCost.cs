using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Registration.Forms
{
    /// <summary>
    /// �Һ����㴰��{F0661633-4754-4758-B683-CB0DC983922B}
    /// </summary>
    public partial class frmReturnCost : Form
    {
        public frmReturnCost()
        {
            InitializeComponent();
        }

        #region ����
        /// <summary>
        /// �Һ�ʵ��
        /// </summary>
        private Neusoft.HISFC.Models.Registration.Register regObj;
        #endregion

        #region ����
        /// <summary>
        /// �Һ�ʵ��
        /// </summary>
        public Neusoft.HISFC.Models.Registration.Register RegObj
        {
            get
            {
                return regObj;
            }
            set
            {
                regObj = value;
                this.SetCost();
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// У����
        /// </summary>
        /// <returns></returns>
        protected virtual int ValidValue()
        {
            //Ӧ�ս��
            decimal willCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.ntxtWillCost.Text);

            //ʵ�ս��
            decimal realCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.ntxtRealCost.Text);

            //������
            decimal changeCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.ntxtChangeCost.Text);

            //ʵ�ս��С��Ӧ�ս��
            if (realCost < willCost)
            {
                MessageBox.Show("ʵ�ս���С��Ӧ�ս��");
                return -1;

            }

            return 1;


        }

        /// <summary>
        /// ��ֵ
        /// </summary>
        private void SetCost()
        {

            if (!this.ntxtRealCost.Focused)
            {
                this.ntxtRealCost.Focus();
            }
            this.ntxtWillCost.Text = this.RegObj.OwnCost.ToString();


        }
        #endregion

        #region �¼�
        private void btOK_Click(object sender, EventArgs e)
        {
            //У������
            int retureValue = this.ValidValue();

            if (retureValue < 0)
            {
                return;
            }

            this.DialogResult = DialogResult.OK;


            this.Close();
        }

        private void ntxtRealCost_TextChanged(object sender, EventArgs e)
        {

            decimal willCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.ntxtWillCost.Text);
            decimal realCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.ntxtRealCost.Text);
            this.ntxtChangeCost.Text = (realCost - willCost ).ToString();
        }

        private void ntxtRealCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btOK_Click(sender, new EventArgs());
            }
        }
        #endregion

    }
}