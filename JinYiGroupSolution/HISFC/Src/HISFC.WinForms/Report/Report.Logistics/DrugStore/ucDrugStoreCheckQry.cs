using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Neusoft.WinForms.Report.Logistics.DrugStore
{
    public partial class ucDrugStoreCheckQry :NeuDataWindow.Controls.ucQueryBaseForDataWindow                                             
    {
        public ucDrugStoreCheckQry()
        {
            InitializeComponent();
        }

        string deptCode = string.Empty;
        ArrayList alDept = new ArrayList();

        #region ��ʼ��
        protected override void OnLoad()
        {


            ///<summary>
            /// ��ʼ������
            ///<summary>

            Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
            Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            obj.ID = "ALL";
            obj.Name = "ȫ��";
            alDept.Add(obj);

            ArrayList dept = managerIntegrate.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.PI);
            alDept.AddRange(dept);
            alDept.AddRange(managerIntegrate.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P));

            this.neuDept.AddItems(alDept);
            this.neuDept.SelectedIndex = 0;


            base.OnLoad();
        }
        #endregion

        #region ����
        protected override int OnRetrieve(params object[] objects)
        {
          

            if (base.GetQueryTime() == -1)
            {
                return -1;
            }

            if (neuDept.Items.Count > 0)
            {
                deptCode = neuDept.SelectedItem.ID;

            }

            if (this.tcDocstatic.SelectedTab.Text == "�̵���ܱ���")
            {
                this.MainDWDataObject = "d_sto_checkstatic";
                dw1.DataWindowObject = "d_sto_checkstatic";
                //this.MainDWLabrary = "Report\pharmacy.pbd;Report\pharmacy.pbl";

                return this.dw1.Retrieve(this.beginTime, this.endTime, deptCode);

            }
            //if (this.tcDocstatic.SelectedTab.Text == "�̵���ϸ����")
            //{
            //    this.mainDWDataObject = "d_sto_checkdetail";
            //    dwMain.DataWindowObject = "d_sto_checkdetail";

            //    string filterString = "ALL";
            //    if (this.neuDept.Text != "ȫ  ��")
            //    { 
            //        filterString = this.neuDept.Tag.ToString();
            //    }
            //    return this.dw2.Retrieve(this.beginTime, this.endTime,doctCode);
            //}

            return 1;

        }
        #endregion 

        private void dw1_Click(object sender, EventArgs e)
        {
            int row = this.dw1.CurrentRow;
            if (row > 0)
            {
                string strCheckCode = string.Empty;
                strCheckCode = this.dw1.GetItemString(row, "���ݺ�");
                //this.dw2.Retrieve

            }

        }

        //private void dw1_Click(object sender, EventArgs e)
        //{
        //    int row = 0;

        //    row = this.dw1.CurrentRow;
        //    if (row > 0)
        //    { 

        //    }
        //}

        //private void dw1_DoubleClick(object sender, EventArgs e)
        //{
        //    this.dw1.GetRowStatus(
        //    if (this.cbInOutType.SelectedItem.ID == "IN")
        //    {
        //        return;
        //    }
        //    if (this.neuTabControl1.Contains(tabDetail) == false)
        //    {
        //        this.neuTabControl1.TabPages.Add(tabDetail);
        //    }
        //    this.neuTabControl1.SelectedIndex = 1;
        //    QueryDetail();
        //}


        //private void QueryDetail()
        //{
        //    try
        //    {
        //        Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Language.Msg("���ڲ�ѯ�����Ժ�..."));
        //        Application.DoEvents();

        //        string checkCode;

        //        checkCode = this.dw1.GetItemString();

        //        DataSet ds = new DataSet();
        //        emp = (Neusoft.HISFC.Models.Base.Employee)dept.Operator;
        //        deptCode = this.nfpStat_Sheet1.Cells[this.nfpStat_Sheet1.ActiveRowIndex, 6].Text.ToString(); //this.nfpStat_Sheet1.Cells[this.nfpStat_Sheet1.ActiveRowIndex, this.nfpStat_Sheet1.Columns["���ұ���"].Index].Text;

        //        string phaname = this.nfpStat_Sheet1.Cells[this.nfpStat_Sheet1.ActiveRowIndex, 0].Text.ToString();
        //        if (this.itemManager.ExecQuery("Neusoft.Pha.DispensePerPatient", ref ds, this.dtpFrom.Value.ToString(), this.dtpTo.Value.ToString(), this.emp.Dept.ID, phaname, deptCode) == -1)
        //        {
        //            MessageBox.Show(Language.Msg("���ݲ�ѯʧ�ܣ��������Ա��ϵ��" + this.itemManager.Err));
        //            return;
        //        }

        //        this.nfpDetail.DataSource = ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        //    }
        //}
    }
}



 