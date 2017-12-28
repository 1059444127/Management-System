using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Order.OutPatient.Forms
{
    public partial class frmInputInjectNum : Neusoft.FrameWork.WinForms.Forms.BaseForm
    {
        public frmInputInjectNum()
        {
            InitializeComponent();
            this.neuNumericUpDown1.ReadOnly = true;
        }

        #region ����
        private bool isHaveSetted = false;
        private bool isSpring = false;
        /// <summary>
        /// ������Ҫ���ĵ�ҽ��
        /// </summary>
        protected Neusoft.HISFC.Models.Order.OutPatient.Order order = null;
        #endregion

        #region ����
        /// <summary>
        /// ��û�����Ժע����
        /// </summary>
        public int InjectNum
        {
            get
            {
                return (int)this.neuNumericUpDown1.Value;
            }
            set
            {

                this.neuNumericUpDown1.Value = (decimal)value;
            }
        }


        /// <summary>
        /// ��ǰҽ��
        /// </summary>
        public Neusoft.HISFC.Models.Order.OutPatient.Order Order
        {
            get
            {
                return this.order;
            }
            set
            {
                order = value;
            }
        }
        #endregion

        /// <summary>
        /// load
        /// </summary>xz
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmInputInjectNum_Load(object sender, System.EventArgs e)
        {
            this.neuNumericUpDown1.Select(0, this.neuNumericUpDown1.Value.ToString().Length);
            if (this.order.ExtendFlag1 == null)
            {
                this.txtTimes.Text = "1";//Ĭ��
            }
            else
            {
                this.txtTimes.TextChanged -= new EventHandler(txtTimes_TextChanged);
                this.txtTimes.Text = "1";
                this.txtTimes.TextChanged += new EventHandler(txtTimes_TextChanged);

                if (this.order.ExtendFlag1.Length >= 1)
                {
                    decimal times = 1;

                    try
                    {
                        times = System.Convert.ToDecimal(this.order.ExtendFlag1.Substring(0, 1));
                        this.isHaveSetted = true;
                    }
                    catch
                    {
                        this.isHaveSetted = false;
                    }

                    this.txtTimes.Text = times.ToString();
                }
            }
            this.neuNumericUpDown1.Focus();
            this.lblDoseOnce.Text = "ÿ�� " + order.DoseOnce.ToString() + order.DoseUnit;
            //{27DBE032-6896-4b8f-9CBC-EDC47F499B50} ����ҽ��ʱ��ʾԺע����
            this.SetInjectDays();
        }

        /// <summary>
        /// {27DBE032-6896-4b8f-9CBC-EDC47F499B50} ����ҽ��ʱ��ʾԺע����
        /// </summary>
        private void SetInjectDays()
        {
            //��������
            decimal qty = this.order.Item.Qty;
            //Ժע����
            decimal injectTimes = this.neuNumericUpDown1.Value;
            //��������
            decimal baseDose = ((Neusoft.HISFC.Models.Pharmacy.Item)this.order.Item).BaseDose;
            //ÿ�μ���
            decimal doseOnce = this.order.DoseOnce;
            //ÿ��Ƶ��
            int frequencyDayCount = (this.order.Frequency.Time.Split('-')).Length;
            //����ó�Ժע����
            int injectDays = (int)Math.Ceiling(injectTimes / frequencyDayCount);
            //���Ժע����
            int maxDays = (int)((qty * baseDose) / (doseOnce * frequencyDayCount));

            this.lblInjectDays.Text = "Ժע������" + injectDays;
        }


        /// <summary>
        /// ȷ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {


                if (this.InjectNum < 0)
                {
                    MessageBox.Show("Ժע���������㣡");
                    this.neuNumericUpDown1.Select(0, this.neuNumericUpDown1.Value.ToString().Length);
                    this.neuNumericUpDown1.Focus();
                    return;
                }
                if (this.InjectNum > 98)
                {
                    MessageBox.Show("Ժע��������");
                    this.neuNumericUpDown1.Select(0, this.neuNumericUpDown1.Value.ToString().Length);
                    this.neuNumericUpDown1.Focus();
                    return;
                }
                order.InjectCount = this.InjectNum;
                order.NurseStation.User02 = "C";//�޸Ĺ�
                if (this.isHaveSetted)
                {
                    order.ExtendFlag1 = order.ExtendFlag1.Remove(0, 1);
                    order.ExtendFlag1 = this.txtTimes.Text + order.ExtendFlag1;
                }
                else
                {
                    if (this.txtTimes.Text.Trim() == "")
                    {
                        MessageBox.Show("ע��ƿ������Ϊ��!");
                        this.txtTimes.Focus();
                        return;
                    }
                    int price = 0;
                    try
                    {
                        price = Neusoft.FrameWork.Function.NConvert.ToInt32(this.txtTimes.Text.Trim());
                    }
                    catch
                    {
                        MessageBox.Show("����ע��ƿ���ĸ�ʽ����ȷ�����������룡");
                        this.txtTimes.Focus();
                        return;
                    }
                    if (price <= 0)
                    {
                        MessageBox.Show("�����ע��ƿ������С�ڻ����0��");
                        this.txtTimes.Focus();
                        return;
                    }
                    if (price > 9)
                    {
                        MessageBox.Show("�����ע��ƿ�����ܴ���9��");
                        this.txtTimes.Focus();
                        return;
                    }
                    this.order.ExtendFlag1 = this.txtTimes.Text.Trim() + this.order.ExtendFlag1;
                }
            }
            catch
            {
                MessageBox.Show("���Ժע��������");
                this.Close();
            }
            this.Close();
        }



        /// <summary>
        /// �¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuNumericUpDown1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.isSpring == false)
                {
                    this.neuNumericUpDown1.Focus();
                    this.isSpring = true;
                }
                else
                {
                    this.btnOK_Click(null, null);
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimes_TextChanged(object sender, System.EventArgs e)
        {
            decimal times = 1;

            if (this.txtTimes.Text.Trim() == "")
            {
                return;
            }

            try
            {
                times = System.Convert.ToDecimal(this.txtTimes.Text.Trim());
            }
            catch
            {
                MessageBox.Show("������ĵ����ָ�ʽ����ȷ�����������룡");
                this.txtTimes.Focus();
                return;
            }
            if (times > 9)
            {
                MessageBox.Show("������ĵ����ֹ������������룡");
                this.txtTimes.Focus();
                return;
            }
        }

        /// <summary>
        /// {27DBE032-6896-4b8f-9CBC-EDC47F499B50} ����ҽ��ʱ��ʾԺע����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.SetInjectDays();
        }


    }
}

