using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Order.Controls
{
    public partial class ucPermissionManager : UserControl, Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable
    {
        private string inpatientno = "";

        /// <summary>
        /// סԺ��ˮ��
        /// </summary>
        /// <returns></returns>
        public string InpatientNo
        {
            set
            {
                if (value == null || value == "")
                {
                    inpatientno = "";
                    return;
                }
                inpatientno = value;
                this.ucPatient1.PatientInfo = inpatient.GetPatientInfomation(value);
                this.Retrieve();
            }

        }

        private FarPoint.Win.Spread.CellType.ComboBoxCellType cmbDept = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        private FarPoint.Win.Spread.CellType.ComboBoxCellType cmbDoc = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        private Neusoft.FrameWork.Public.ObjectHelper helper = null;
        private Neusoft.HISFC.BizLogic.Order.Permission manager = new Neusoft.HISFC.BizLogic.Order.Permission();
        private Neusoft.HISFC.BizProcess.Integrate.RADT inpatient = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        private Neusoft.FrameWork.Public.ObjectHelper userHelper = null;
        
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void init()
        {
            try
            {
                Neusoft.HISFC.BizProcess.Integrate.Manager managerDept = new Neusoft.HISFC.BizProcess.Integrate.Manager();
                ArrayList alDepts = managerDept.QueryDeptmentsInHos(true);
                if (alDepts == null)
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����Ժ���ҳ���"));
                    return;
                }
                helper = new Neusoft.FrameWork.Public.ObjectHelper(alDepts);

                userHelper = new Neusoft.FrameWork.Public.ObjectHelper(managerDept.QueryEmployeeAll());
            }
            catch { }

        }

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                ucPermissionInput u = new ucPermissionInput();
                u.Permission = this.neuSpread1.Sheets[0].ActiveRow.Tag as Neusoft.HISFC.Models.Order.Consultation;
                u.Permission.PatientNo = this.ucPatient1.PatientInfo.PID.PatientNO;
                u.Permission.InpatientNo = this.ucPatient1.PatientInfo.ID;
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��Ȩ";
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.MaximizeBox = false;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(u);
                this.Retrieve();

            }
            catch { }
        }

        public int Retrieve()
        {
            this.inpatientno = this.ucPatient1.PatientInfo.ID;
            if (this.inpatientno == "") return -1;
            ArrayList al = manager.QueryPermission(this.inpatientno);
            if (al == null)
            {
                MessageBox.Show(manager.Err);
                return -1;
            }
            
            this.neuSpread1_Sheet1.RowCount = al.Count;
            for (int i = 0; i < al.Count; i++)
            {
                Neusoft.HISFC.Models.Order.Consultation permission = al[i] as Neusoft.HISFC.Models.Order.Consultation;
                if (permission == null)
                {
                    MessageBox.Show("����");
                    return -1;
                }
                //Neusoft.HISFC.Models.Base.Department dept = helper.GetObjectFromID(permission.DeptConsultation.ID) as Neusoft.HISFC.Models.Base.Department;
                //if (dept == null)
                //{
                //    MessageBox.Show("û�ҵ�"+permission.DeptConsultation.ID +"�Ŀ���.");
                //    return -1;
                //}
                this.neuSpread1.Sheets[0].Cells[i, 0].Value = helper.GetObjectFromID(permission.DeptConsultation.ID);
                this.neuSpread1.Sheets[0].Cells[i, 1].Value = permission.DoctorConsultation.Name;
                this.neuSpread1.Sheets[0].Cells[i, 2].Value = permission.BeginTime;
                this.neuSpread1.Sheets[0].Cells[i, 3].Value = permission.EndTime;
                this.neuSpread1.Sheets[0].Cells[i, 4].Value = permission.Name;
                this.neuSpread1.Sheets[0].Cells[i, 5].Value = permission.User01;
                this.neuSpread1.Sheets[0].Cells[i, 6].Value = permission.User02;
                this.neuSpread1.Sheets[0].Rows[i].Tag = permission;
            }
            return 0;
        }





        public ucPermissionManager()
        {
            InitializeComponent();
        }

        private void ucPermissionManager_Load(object sender, EventArgs e)
        {
            //
            // ���β��õĻ�����Ϣ
            //
            this.ucPatient1.S5_Birthday = false;
            this.ucPatient1.S6_Cautioner = false;
            this.ucPatient1.S7_PayKind = false;
            this.ucPatient1.S8_MoneyAlert = false;
            this.ucPatient1.Sa_CautionMoney = false;
            this.ucPatient1.Sb_Bill = false;
            this.ucPatient1.Sc_Available = false;
            this.ucPatient1.Sd_AttendingDoctor = false;
            this.ucPatient1.Se_AdmittingDoctor = false;
            this.ucPatient1.Sf_AdmittingNurse = false;
            this.ucPatient1.Sg_ThisBill = false;

            //
            // ���β��õİ�ť
            //
            this.queryForm.ShowExportButton = false;
            this.queryForm.ShowImportButton = false;
            this.queryForm.ShowSaveButton = false;
            this.queryForm.ShowPrintPreviewButton = false;
            this.queryForm.ShowPrintButton = false;

            this.init();
        }

        void ucQueryInpatientNo1_myEvent()
        {
            //if (/*this.ucQueryInpatientNo1.InpatientNo == null || this.ucQueryInpatientNo1.InpatientNo == ""*/)
            if (this.ucQueryInpatientNo1.Text == "" )
            {
                this.InpatientNo = "";
                this.ucPatient1.PatientInfo = null;
                MessageBox.Show("û�в鵽���ߣ�");
            }
            else
            {
                this.InpatientNo = this.ucQueryInpatientNo1.InpatientNo;
                //this.InpatientNo = this.ucQueryInpatientNo1.Text;
                //Neusoft.HISFC.Models.RADT.PatientInfo p = inpatient.GetPatientInfomation(this.ucQueryInpatientNo1.InpatientNo);
                //this.ucPatient1.PatientInfo = p;
            }
        }


        #region IMaintenanceControlable ��Ա

        //public int Del()
        //{
        //    try
        //    {
        //        if (this.neuSpread1.Sheets[0].ActiveRow == null) return -1;
        //        Neusoft.HISFC.Models.Order.Consultation permission = this.neuSpread1.Sheets[0].ActiveRow.Tag as Neusoft.HISFC.Models.Order.Consultation;
        //        if (permission == null) return -1;
        //        if (MessageBox.Show("ȷʵҪɾ������Ȩ��?\n�ò������ܳ���!", "����", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return 0;
        //        Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(manager.Connection);
        //        t.BeginTransaction();
        //        manager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
        //        if (this.manager.DeletePermission(permission.ID) == -1)
        //        {
        //            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //            MessageBox.Show(manager.Err);
        //            return -1;
        //        }
        //        Neusoft.FrameWork.Management.PublicTrans.Commit();
        //        this.Retrieve();
        //    }
        //    catch { }
        //    return 0;
        //}

        //public int MyAdd()
        //{
        //    try
        //    {
        //        if (this.inpatientno == "") return -1;
        //        ucPermissionInput u = new ucPermissionInput();
        //        Neusoft.HISFC.Models.Order.Consultation permission = new Neusoft.HISFC.Models.Order.Consultation();

        //        permission.InpatientNo = this.inpatientno;
        //        if (this.inpatientno == null || this.inpatientno == "")
        //        {
        //            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������סԺ��"));
        //            return -1;
        //        }
        //        if (this.ucPatient1.PatientInfo.PVisit.PatientLocation.Dept.ID != ((Neusoft.HISFC.Models.Base.Employee)this.manager.Operator).Dept.ID)
        //        {
        //            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�Ǳ��ƻ��߲�����Ȩ"));
        //            return -1;
        //        }
        //        permission.BeginTime = manager.GetDateTimeFromSysDateTime();
        //        permission.EndTime = permission.BeginTime;
        //        u.Permission = permission;
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��Ȩ";
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.MaximizeBox = false;
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(u);
        //        this.Retrieve();
        //    }
        //    catch { }
        //    return 0;
        //}

        //public int Copy()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Cut()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Delete()
        //{
        //    return this.Del();
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Export()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Import()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Init()
        //{
        //    this.init();
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //private bool isDirty = false;
        //public bool IsDirty
        //{
        //    get
        //    {
        //        return this.isDirty;
        //        //throw new Exception("The method or operation is not implemented.");
        //    }
        //    set
        //    {
        //        this.isDirty = value;
        //        //throw new Exception("The method or operation is not implemented.");
        //    }
        //}

        //public int Modify()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int NextRow()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Paste()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int PreRow()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Print()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int PrintConfig()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int PrintPreview()
        //{
        //    return 0;
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Query()
        //{
        //    return this.Retrieve();
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //Neusoft.FrameWork.WinForms.Forms.IMaintenanceForm queryForm;
        //public Neusoft.FrameWork.WinForms.Forms.IMaintenanceForm QueryForm
        //{
        //    get
        //    {
        //        return this.queryForm;
        //        //throw new Exception("The method or operation is not implemented.");
        //    }
        //    set
        //    {
        //        this.queryForm = value;
        //        //throw new Exception("The method or operation is not implemented.");
        //    }
        //}

        //public int Save()
        //{
        //    return 0;
        //    //return this.Add();
        //    //throw new Exception("The method or operation is not implemented.");
        //}

        //public int Add()
        //{
        //    //return this.MyAdd();
        //    //throw new Exception("The method or operation is not implemented.");
        //    try
        //    {
        //        if (this.inpatientno == "") return -1;
        //        ucPermissionInput u = new ucPermissionInput();
        //        Neusoft.HISFC.Models.Order.Consultation permission = new Neusoft.HISFC.Models.Order.Consultation();
        //        permission.InpatientNo = this.inpatientno;
        //        if (this.inpatientno == null || this.inpatientno == "")
        //        {
        //            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������סԺ��"));
        //            return -1;
        //        }
        //        if (this.ucPatient1.PatientInfo.PVisit.PatientLocation.Dept.ID != ((Neusoft.HISFC.Models.Base.Employee)this.manager.Operator).Dept.ID)
        //        {
        //            MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�Ǳ��ƻ��߲�����Ȩ"));
        //            return -1;
        //        }
        //        permission.BeginTime = manager.GetDateTimeFromSysDateTime();
        //        permission.EndTime = permission.BeginTime;
        //        u.Permission = permission;
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��Ȩ";
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopForm.MaximizeBox = false;
        //        Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(u);
        //        this.Retrieve();
        //    }
        //    catch { }
        //    return 0;
        //}

        #endregion

        #region IMaintenanceControlable ��Ա

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Add()
        {
            try
            {
                if (/*this.inpatientno == null ||*/ this.ucQueryInpatientNo1.Text == "")
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("������סԺ��"));
                    return -1;
                }
                //if (this.inpatientno == "") return -1;
                this.inpatientno = this.ucPatient1.PatientInfo.ID;
                ucPermissionInput u = new ucPermissionInput();
                Neusoft.HISFC.Models.Order.Consultation permission = new Neusoft.HISFC.Models.Order.Consultation();
                permission.PatientNo = ucPatient1.PatientInfo.PID.PatientNO;
                permission.InpatientNo = this.inpatientno;
                if (this.ucPatient1.PatientInfo.PVisit.PatientLocation.Dept.ID != ((Neusoft.HISFC.Models.Base.Employee)this.manager.Operator).Dept.ID)
                {
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�Ǳ��ƻ��߲�����Ȩ"));
                    return -1;
                }
                permission.BeginTime = manager.GetDateTimeFromSysDateTime();
                permission.EndTime = permission.BeginTime;
                u.Permission = permission;
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��Ȩ";
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.MaximizeBox = false;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(u);
                this.Retrieve();
            }
            catch { }
            return 0;
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Copy()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Cut()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Delete()
        {
            try
            {
                if (this.neuSpread1.Sheets[0].ActiveRow == null) return -1;
                Neusoft.HISFC.Models.Order.Consultation permission = this.neuSpread1.Sheets[0].ActiveRow.Tag as Neusoft.HISFC.Models.Order.Consultation;
                if (permission == null) return -1;
                if (MessageBox.Show("ȷʵҪɾ������Ȩ��?\n�ò������ܳ���!", "����", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return 0;
                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(manager.Connection);
                //t.BeginTransaction();
                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                manager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                if (this.manager.DeletePermission(permission.ID) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(manager.Err);
                    return -1;
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                this.Retrieve();
            }
            catch { }
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Export()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Import()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Init()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        bool Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.IsDirty
        {
            get
            {
                //throw new Exception("The method or operation is not implemented.");
                return false;
            }
            set
            {
                //throw new Exception("The method or operation is not implemented.");
            }
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Modify()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.NextRow()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Paste()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.PreRow()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Print()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.PrintConfig()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.PrintPreview()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Query()
        {
            this.inpatientno = this.ucQueryInpatientNo1.InpatientNo;
            if (this.inpatientno == "") return -1;
            ArrayList al = manager.QueryPermission(this.inpatientno);
            if (al == null)
            {
                MessageBox.Show(manager.Err);
                return -1;
            }

            this.neuSpread1_Sheet1.RowCount = al.Count;
            for (int i = 0; i < al.Count; i++)
            {
                Neusoft.HISFC.Models.Order.Consultation permission = al[i] as Neusoft.HISFC.Models.Order.Consultation;
                if (permission == null)
                {
                    MessageBox.Show("����");
                    return -1;
                }
                // �������������,�����Ҫ,�����Լ�д������,��permission.DeptConsultation.ID��Ϊ����
                // ȡ��cmbDept���memo���ǿ��ҵ�ȫ��
                this.neuSpread1.Sheets[0].Cells[i, 0].Value = helper.GetName(permission.DeptConsultation.ID);
                this.neuSpread1.Sheets[0].Cells[i, 1].Value = permission.DoctorConsultation.Name;
                this.neuSpread1.Sheets[0].Cells[i, 2].Value = permission.BeginTime;
                this.neuSpread1.Sheets[0].Cells[i, 3].Value = permission.EndTime;
                this.neuSpread1.Sheets[0].Cells[i, 4].Value = permission.Name;
                this.neuSpread1.Sheets[0].Cells[i, 5].Value = userHelper.GetName(permission.User01);
                this.neuSpread1.Sheets[0].Cells[i, 6].Value = permission.User02;
                this.neuSpread1.Sheets[0].Rows[i].Tag = permission;
            }
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        Neusoft.FrameWork.WinForms.Forms.IMaintenanceForm queryForm;
        Neusoft.FrameWork.WinForms.Forms.IMaintenanceForm Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.QueryForm
        {
            get
            {

                return this.queryForm;
                //throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                this.queryForm = value;
            }
        }

        int Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable.Save()
        {
            return 0;
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
