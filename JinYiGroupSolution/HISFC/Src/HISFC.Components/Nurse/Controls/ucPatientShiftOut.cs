using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.Components.Nurse.Controls
{
    /// <summary>
    /// [��������: ת�����룬ȡ���ؼ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2006-11-30]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucPatientShiftOut : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Neusoft.HISFC.BizProcess.Interface.ITransferDeptApplyable
    {
        public ucPatientShiftOut()
        {
            InitializeComponent();
        }

        #region ����
        Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        Neusoft.HISFC.BizLogic.RADT.InPatient inpatient = new Neusoft.HISFC.BizLogic.RADT.InPatient();

        Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = null;
        private bool isCancel = false;
        
        #endregion

        #region ����
        /// <summary>
        /// �Ƿ�ȡ������
        /// </summary>
        public bool IsCancel
        {
            get
            {
                return this.isCancel;
            }
            set
            {
                this.isCancel = value;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        private void InitControl()
        {
           
            try
            {
                ArrayList al = Neusoft.HISFC.Models.Base.SexEnumService.List();
                this.cmbNewDept.AddItems(manager.QueryDeptmentsInHos(true));
            }
            catch { }

        }


        /// <summary>
        /// ��������Ϣ��ʾ�ڿؼ���
        /// </summary>
        private void ShowPatientInfo()
        {
            this.txtPatientNo.Text = this.patientInfo.PID.PatientNO;		//סԺ��
            this.txtPatientNo.Tag = this.patientInfo.ID;							//סԺ��ˮ��
            this.txtName.Text = this.patientInfo.Name;								//��������
            this.txtSex.Text = this.patientInfo.Sex.Name;					//�Ա�
            this.txtOldDept.Text = this.patientInfo.PVisit.PatientLocation.Dept.Name;//Դ��������
            this.cmbBedNo.Text = this.patientInfo.PVisit.PatientLocation.Bed.ID.Length > 4 ? this.patientInfo.PVisit.PatientLocation.Bed.ID.Substring(4) : "";	//����
            //���廼��Locationʵ��
            Neusoft.HISFC.Models.RADT.Location newLocation = new Neusoft.HISFC.Models.RADT.Location();
            //ȡ����ת��������Ϣ
            newLocation = this.inpatient.QueryShiftNewLocation(this.patientInfo.ID, this.patientInfo.PVisit.PatientLocation.Dept.ID);
            this.patientInfo.User03 = newLocation.User03;
            if (this.patientInfo.User03 == null)
                this.patientInfo.User03 = "1";//����

            if (newLocation == null)
            {
                MessageBox.Show(this.inpatient.Err);
                return;
            }

            this.cmbNewDept.Tag = newLocation.Dept.ID;	//�¿�������
            this.txtNote.Text = newLocation.Memo;		//��ע
            //���û��ת������,������¿��ұ���
            if (newLocation.Dept.ID == "")
            {
                this.cmbNewDept.Text = null;
            }
            if (this.patientInfo.User03 != null && this.patientInfo.User03 == "0")
                this.label8.Visible = true;
            else
                this.label8.Visible = false;
        }


        /// <summary>
        /// ����
        /// </summary>
        public void ClearPatintInfo()
        {
            this.cmbNewDept.Text = "";
            this.cmbNewDept.Tag = "";
        }


        /// <summary>
        /// ˢ��
        /// </summary>
        /// <param name="patientInfo"></param>
        public void RefreshList(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            try
            {
                //��������Ϣ��ʾ�ڿؼ���
                this.ShowPatientInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        

        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            this.patientInfo = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;
            RefreshList(this.patientInfo);
            return 0;
        }

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.InitControl();
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.cmbNewDept.Tag == null || this.cmbNewDept.Tag.ToString() == "")
            {
                MessageBox.Show("��ѡ��Ҫת��Ŀ���!");
                return;
            }
            Neusoft.FrameWork.Models.NeuObject dept = new Neusoft.FrameWork.Models.NeuObject();
            dept.ID = this.cmbNewDept.Tag.ToString();
            dept.Name = this.cmbNewDept.Text;
            dept.Memo = this.txtNote.Text;

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);

            Neusoft.HISFC.BizProcess.Integrate.RADT radt = new Neusoft.HISFC.BizProcess.Integrate.RADT();
            //t.BeginTransaction();

            //radt.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            //{F0BF027A-9C8A-4bb7-AA23-26A5F3539586}
            if (radt.ShiftOut(this.patientInfo, dept,null,this.patientInfo.User03, this.isCancel) == -1)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
            }
            else
            {
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                if (bSaveAndClose)
                {
                    dialogResult = DialogResult.OK;
                    this.FindForm().Close();
                    return;
                }
            }
            MessageBox.Show(radt.Err);
            
            base.OnRefreshTree();//ˢ����
        }
        #endregion

        #region ITransferDeptApplyable ��Ա
        bool bSaveAndClose = false;
        DialogResult dialogResult = DialogResult.None;
        public Neusoft.FrameWork.Models.NeuObject Dept
        {
            get { return this.cmbNewDept.SelectedItem; }
        }

        public void SetPatientInfo(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            this.InitControl();
            this.patientInfo = patientInfo.Clone();
            RefreshList(this.patientInfo);
           
        }

        public DialogResult ShowDialog()
        {
            bSaveAndClose = true;
            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(this);
            return dialogResult;
            
        }

        #endregion
    }
}
