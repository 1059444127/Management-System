using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Report.Finance.FinIpr
{
    public partial class ucFinIprIutpatientInfo :NeuDataWindow.Controls.ucQueryBaseForDataWindow 
    {
        public ucFinIprIutpatientInfo()
        {
            InitializeComponent();
        }

        string queryStr = string.Empty;
        string strDeptCode = "ALL";
        string strPatientID = string.Empty;
        string strPatientNM = string.Empty;
        ArrayList alDept = new ArrayList();
        Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();

        #region ��ʼ��
        protected override void OnLoad(EventArgs e)
        {

            if (!DesignMode)
            {
                DateTime now = deptManager.GetDateTimeFromSysDateTime();
                //this.dtpBeginTime.Value = now;
                this.dtpBeginTime.Value = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                this.dtpEndTime.Value = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            }
            //����
            ArrayList list = new ArrayList();
            Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
            obj.ID = "ALL";
            obj.Name = "ȫ��";
            alDept.Add(obj);

            list = deptManager.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.I);
            alDept.AddRange(list);
            cmbDept.AddItems(alDept);
            cmbDept.SelectedIndex = 0;
            

           // base.OnLoad(e);
        }
        #endregion 

        #region ��ѯ
       
        protected override int OnRetrieve(params object[] objects)
        {


            if (base.GetQueryTime() == -1)
            {
                return -1;
            }


             base.OnRetrieve(this.dtpBeginTime.Value, this.dtpEndTime.Value);
             Filter();
            return 1;

        }
        #endregion 

        #region ����

        private void neuFilter_SelectedChanged(object sender, EventArgs e)
        {

            Filter();
         

        }

        private void Filter()
        {

            strDeptCode = cmbDept.SelectedItem.ID;
            strPatientID = ntbPatientID.Text.Trim().ToUpper().Replace(@"\", ""); ;
            strPatientNM = ntbPatientNM.Text.Trim().ToUpper().Replace(@"\", "");
            DataView dv = this.dwMain.Dv;
            if (dv == null)
            {
                return;
            }

            if (strDeptCode == "ALL")
            {
                queryStr = "(סԺ�� like '%{1}') and (���� like '%{2}%')";

            }
            else
            {
                queryStr = "((dept_code = '{0}')) and ((סԺ�� like '%{1}') ) and ((���� like '%{2}%') )";
            }

            dv.RowFilter = "";

            string str = string.Format(this.queryStr, strDeptCode, strPatientID, strPatientNM);

            try
            {

                dv.RowFilter = str;


            }
            catch
            {
                MessageBox.Show("�������������ַ�����������ȷ�Ĳ�ѯ��Ϣ��");

                return;
            }
        }



     

      

        #endregion 

       
    }
}