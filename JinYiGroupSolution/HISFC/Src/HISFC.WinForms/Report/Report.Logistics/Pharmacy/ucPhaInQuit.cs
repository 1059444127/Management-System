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
    public partial class ucPhaInQuit : NeuDataWindow.Controls.ucQueryBaseForDataWindow
    {
        #region ����

        Neusoft.HISFC.BizLogic.Pharmacy.Constant phaConsManager = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();

        Neusoft.HISFC.Models.Base.Employee empl = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;

        Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        string query = "((trade_name like '{0}%') or (spellcode  like '{0}%') or (wbcode like '{0}%'))";

        #endregion

        public ucPhaInQuit()
        {
            InitializeComponent();
        }

        private void ucPhaInQuit_Load(object sender, EventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���������У����Ժ򡭡�");
            ArrayList alCompany = phaConsManager.QueryCompany("1", true);
            if(alCompany == null)
            {
                MessageBox.Show("��ȡ��Ӧ����Ϣ����" + phaConsManager.Err, "������Ϣ");
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                return;
            }
            cmbSelect.AddItems(alCompany); 
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        #region ����

        private void SetFilter()
        {
            string drug = txtDrug.Text.Trim().Replace(@"\", "").Replace(@"'", "").ToUpper();
            DataView dv = this.dwMain.Dv;
            if (dv == null)
            {
                return;
            }
            if (drug.Equals(""))
            {
                dv.RowFilter = "";
                //this.dwMain.SetFilter("");
                //this.dwMain.Filter();
                return;
            }
            else {
                string str = string.Format(query, drug);
                dv.RowFilter = str;
                
            }
            //string str = string.Format(query, drug);
            //dwMain.SetFilter(str);
            //dwMain.Filter();
        }

        protected override int OnRetrieve(params object[] objects)
        {
            string company = "000";
            string pharmacy = empl.Dept.Name;
            string pharmacyID = empl.Dept.ID;
            string oper = empl.Name;

            if(this.dtpBeginTime.Value > this.dtpEndTime.Value)
            {
                MessageBox.Show("��ʼʱ�䲻�ܴ��ڽ���ʱ�䣡");
                return -1;
            }  
            if(this.cmbSelect.SelectedItem != null)
            {
                company = this.cmbSelect.SelectedItem.ID;
            }

            return base.OnRetrieve(this.dtpBeginTime.Value.Date,this.dtpEndTime.Value.Date,company,pharmacy,pharmacyID,oper);
        }

        #endregion

        #region �¼�

        private void txtDrug_TextChanged(object sender, EventArgs e)
        {
            this.SetFilter();
        }

        #endregion 
    }
}
