using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Report.Logistics.Pharmacy
{
    public partial class ucPhaAdjustPriceByQuality : NeuDataWindow.Controls.ucQueryBaseForDataWindow
    {
        #region ����

        Neusoft.HISFC.BizProcess.Integrate.Manager inteManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        Neusoft.HISFC.Models.Base.Employee empl = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;

        Neusoft.HISFC.BizLogic.Pharmacy.Item itemPhaManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        #endregion

        public ucPhaAdjustPriceByQuality()
        {
            InitializeComponent();
        }

        private void ucPhaAdjustPriceByQuality_Load(object sender, EventArgs e)
        {
            this.dtpBeginTime.Value = this.dtpBeginTime.Value.Date;
            this.dtpEndTime.Value = this.dtpEndTime.Value.Date.AddDays(1).AddMilliseconds(-1);

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ݼ����У����Ժ򡭡�");
            Application.DoEvents();
            cmbQuality.AddItems(inteManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.DRUGQUALITY));
            List<Neusoft.HISFC.Models.Pharmacy.Item> drugList = new List<Neusoft.HISFC.Models.Pharmacy.Item>();
            drugList = itemPhaManager.QueryItemList();
            if (drugList == null)
            {
                MessageBox.Show("��ʼ��ҩƷ��Ϣ����");
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return;
            }
            else
            {
                cmbDrug.AddItems(new ArrayList(drugList));
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        #region ����

        protected override int OnRetrieve(params object[] objects)
        {
            string quality = "000";
            string drugID = "111";

            if(ckQuality.Checked)
            {
                if (cmbQuality.SelectedItem == null)
                {
                    MessageBox.Show("��ѡ����ϸ���");
                    return -1;
                }
                else
                {
                    quality = cmbQuality.SelectedItem.ID;
                }
            }

            if(ckDrug.Checked)
            {
                if(cmbDrug.SelectedItem == null)
                {
                    MessageBox.Show("��ѡ��ҩƷ��");
                    return -1;
                }
                else
                {
                    drugID = cmbDrug.SelectedItem.ID;
                }
            }

            return base.OnRetrieve(empl.Dept.ID,this.dtpBeginTime.Value,this.dtpEndTime.Value,quality,drugID);
        }

        #endregion

        #region �¼�

        private void ckQuality_CheckedChanged(object sender, EventArgs e)
        {
            if (ckQuality.Checked)
            {
                cmbQuality.Enabled = true;
            }
            else
            {
                cmbQuality.Tag = null;
                cmbQuality.Text = "";
                cmbQuality.Enabled = false;
            }
        }

        private void ckDrug_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDrug.Checked)
            {
                cmbDrug.Enabled = true;
            }
            else
            {
                cmbDrug.Tag = null;
                cmbDrug.Text = "";
                cmbDrug.Enabled = false;
            }
        }

        #endregion
    }
}
