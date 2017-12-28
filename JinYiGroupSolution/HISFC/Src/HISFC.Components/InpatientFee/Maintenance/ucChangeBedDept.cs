using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Models;
using Neusoft.FrameWork.Management;
namespace Neusoft.HISFC.Components.InpatientFee.Maintenance
{
    public partial class ucChangeBedDept : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucChangeBedDept()
        {
            InitializeComponent();
        }

        #region ����

        private Neusoft.HISFC.BizProcess.Integrate.Manager mamagerInteger=new Neusoft.HISFC.BizProcess.Integrate.Manager();
        private Neusoft.HISFC.BizProcess.Integrate.RADT radtInteger = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        Neusoft.HISFC.Models.RADT.PatientInfo patient = null;
        Neusoft.HISFC.Models.Base.Bed bed = new Neusoft.HISFC.Models.Base.Bed();
        NeuObject deptObj = new NeuObject();
        NeuObject newdeptObj = new NeuObject();
        bool isFresh = true;

        /// <summary>
        /// adt�ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.IHE.IADT adt = null;
        #endregion 

        #region ����
        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected virtual void Init()
        {
            #region ��ʼ�������б�
            ArrayList al = mamagerInteger.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.I);
            if (al != null)
            {
                this.cmbDept.AddItems(al);
                listDept.Items.AddRange(al.ToArray());
            }
            #endregion

            #region ����
            lvBed.View = View.Details;
            lvBed.Columns.Add("������",160, HorizontalAlignment.Center);
            lvBed.Columns.Add("����",100,HorizontalAlignment.Center);
            lvBed.Columns.Add("�ȼ�",160,HorizontalAlignment.Center);
            lvBed.Columns.Add("����",100,HorizontalAlignment.Center);
            lvBed.Columns.Add("ͣ��", 100,HorizontalAlignment.Center);
            #endregion

            this.ActiveControl = txtPatientNo;
            txtPatientNo.myEvent += new Neusoft.HISFC.Components.Common.Controls.myEventDelegate(txtPatientNo_myEvent);
        }

        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        /// <param name="patientInfo"></param>
        private void SetInofo(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {

            try
            {
                this.txtPatientNo.Text = patientInfo.PID.PatientNO;//סԺ��
                this.txtCardNo.Text = patientInfo.PID.CardNO;//������
                this.txtdept.Text = patientInfo.PVisit.PatientLocation.Dept.Name; //סԺ����
                deptObj.ID = patientInfo.PVisit.PatientLocation.Dept.ID;
                deptObj.Name = patientInfo.PVisit.PatientLocation.Dept.Name;
                this.txtdept.Tag = deptObj;//��ǰ���߿���
                this.txtIndate.Text = patientInfo.PVisit.InTime.ToString("yyyy-MM-dd");//סԺ����
                this.txtName.Text = patientInfo.Name;//����
                this.txtSex.Text = patientInfo.Sex.Name; //�Ա�
                this.txtpaykind.Text = patientInfo.Pact.Name; //�������
                this.txtBedNo.Text = patientInfo.PVisit.PatientLocation.Bed.ID.Length > 4 ?
                                    patientInfo.PVisit.PatientLocation.Bed.ID.Substring(4) : "";//������
                this.txtzrys.Text = patientInfo.PVisit.ConsultingDoctor.Name;//����ҽʦ
                this.txtzzys.Text = patientInfo.PVisit.AttendingDoctor.Name;//����ҽʦ
                this.txtzrhs.Text = patientInfo.PVisit.AdmittingNurse.Name; //���λ�ʿ
                this.txtzyys.Text = patientInfo.PVisit.AdmittingDoctor.Name; //סԺҽʦ
                this.lblDept.Text = patientInfo.PVisit.PatientLocation.Dept.Name;
                this.lblDept.Tag = deptObj;
                this.lblBed.Text = patientInfo.PVisit.PatientLocation.Bed.ID.Length > 4 ?
                                    patientInfo.PVisit.PatientLocation.Bed.ID.Substring(4) : "";
                this.lblBed.Tag = patientInfo.PVisit.PatientLocation.Bed;
                this.listDept.ClearSelected();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            
        }

        /// <summary>
        /// ���
        /// </summary>
        private void Clear()
        {
            foreach (Control c in this.neuGroupBox1.Controls)
            {
                if (c.GetType() == typeof(Neusoft.FrameWork.WinForms.Controls.NeuTextBox))
                {
                    c.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// ��ȡ������Ժ���ߺͿ�λ����
        /// </summary>
        protected virtual void GetPatientBedByDept()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ������ݣ����Ժ�^^");
            Application.DoEvents();
            Neusoft.FrameWork.Models.NeuObject deptObj = new Neusoft.FrameWork.Models.NeuObject();
            deptObj.ID = cmbDept.Tag.ToString();
            deptObj.Name = cmbDept.Text;
            
            //����
            RefreshListPatient(deptObj);
            //����
            GetBedByDeptCode(deptObj);
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        /// <summary>
        /// ���ݿ���ˢ�»����б�
        /// </summary>
        /// <param name="deptObj"></param>
        private void RefreshListPatient(Neusoft.FrameWork.Models.NeuObject deptObj)
        {
            Neusoft.HISFC.Models.RADT.InStateEnumService enumState=new Neusoft.HISFC.Models.RADT.InStateEnumService();
            //��Ժ״̬
            enumState.ID = Neusoft.HISFC.Models.Base.EnumInState.I.ToString();
            ArrayList al = radtInteger.QueryPatientByDeptCode(deptObj.ID, enumState);
            //ԤԼ��Ժ״̬
            enumState.ID = Neusoft.HISFC.Models.Base.EnumInState.P.ToString();
            ArrayList al1 = radtInteger.QueryPatientByDeptCode(deptObj.ID, enumState);
            if (al == null)
                al = al1;
            else
            {
                if (al1 != null)
                    al.AddRange(al1);
            }
            listPatient.Items.Clear();
            if (al != null)
                this.listPatient.Items.AddRange(al.ToArray());
        }

        /// <summary>
        /// ���ݲ������ҿ�λ����
        /// </summary>
        /// <param name="nurseCellId"></param>
        protected virtual void GetBedByDeptCode(Neusoft.FrameWork.Models.NeuObject deptObj)
        {
            ArrayList alNurseCell = mamagerInteger.QueryNurseStationByDept(deptObj);
            ArrayList alBed = new ArrayList();
             
            foreach (NeuObject obj in alNurseCell)
            {
                ArrayList temp = mamagerInteger.QueryUnoccupiedBed(obj.ID);
                if (temp != null && temp.Count > 0)
                    alBed.AddRange(temp);
            }
            lvBed.Items.Clear();
            if (alBed != null)
            {
                ListViewItem lvi;
                for (int i = 0; i < alBed.Count; i++)
                {
                    bed = alBed[i] as Neusoft.HISFC.Models.Base.Bed;
                    lvi = new ListViewItem();
                    lvi.Text = bed.ID.Length>4 ? bed.ID.Substring(4) : "";
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, bed.Sex.Name));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, bed.BedGrade.Name));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, bed.SickRoom.ID));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, bed.IsValid ? "��" : "��"));
                    lvi.Tag = bed;
                    this.lvBed.Items.Add(lvi);
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        protected virtual int Save()
        {
            if (patient == null)
            {
                MessageBox.Show("��ѡ���ߣ�", "��ʾ");
                return -1;
            }

            #region ��û�������λ��            
            Neusoft.HISFC.Models.RADT.Location newlocation = new Neusoft.HISFC.Models.RADT.Location();
            newdeptObj = this.lblDept.Tag as NeuObject;
            newlocation.Dept.ID = newdeptObj.ID;
            newlocation.Dept.Name = newdeptObj.Name;
            bed=this.lblBed.Tag as Neusoft.HISFC.Models.Base.Bed;
            newlocation.Bed = bed;
            newlocation.NurseCell= bed.NurseStation;
            #endregion 
            
            //Neusoft.FrameWork.Management.Transaction tran = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            radtInteger.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            mamagerInteger.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            int resultValue = radtInteger.ChangeDept(patient, newlocation);
            if (resultValue < 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(this.radtInteger.Err);
                return -1;
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            if (isFresh)
            {
                deptObj = this.txtdept.Tag as NeuObject;
                RefreshListPatient(deptObj);
            }
            GetBedByDeptCode(newdeptObj);


            #region addby xuewj 2010-3-15

            Neusoft.HISFC.Models.RADT.PatientInfo p = patient.Clone();
            p.PVisit.PatientLocation = newlocation;

            if (this.adt == null)
            {
                this.adt = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.IHE.IADT)) as Neusoft.HISFC.BizProcess.Interface.IHE.IADT;
            }
            if (this.adt != null)
            {
                adt.TransferPatient(p);
            }

            #endregion

            MessageBox.Show("ת�ơ����ɹ���", "��ʾ");
            this.cmbDept.Tag = newdeptObj.ID;
            return 1;
        }

        protected virtual bool IsPatientStateValid(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            //�ж��Ƿ��Ժ
            if (patientInfo.PVisit.InState.ID.ToString() == "N" || patientInfo.PVisit.InState.ID.ToString() == "O")
            {
                MessageBox.Show(Language.Msg("�û����Ѿ���Ժ!"));
                this.txtPatientNo.Focus();
                this.txtPatientNo.TextBox.SelectAll();
                return false;
            }

            //�ж�û�н���
            if (patientInfo.PVisit.InState.ID.ToString() == "R")
            {
                MessageBox.Show(Language.Msg("�û��߻�û�н���,��ȥ����������շ�"));

                this.txtPatientNo.Focus();
                this.txtPatientNo.TextBox.SelectAll();

                return false;
            }

            return true;
        }

        #endregion

        #region �¼�
        private void ucChangeBedDept_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        /// <summary>
        /// Ŀ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listDept_DoubleClick(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.Base.Department dept=this.listDept.SelectedItem as Neusoft.HISFC.Models.Base.Department;
            newdeptObj.ID = dept.ID;
            newdeptObj.Name = dept.Name;
            this.lblDept.Text = dept.Name;
            this.lblDept.Tag = newdeptObj;
            this.GetBedByDeptCode(newdeptObj);
            if (lvBed.Items.Count > 0)
            {
                this.lblBed.Text = (this.lvBed.Items[0].Text);
                this.lblBed.Tag = this.lvBed.Items[0].Tag as Neusoft.HISFC.Models.Base.Bed;
            }
        }

        /// <summary>
        /// ѡ����ҹ�����Ժ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetPatientBedByDept();
        }

        /// <summary>
        /// ѡ��ת�Ʋ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listPatient_DoubleClick(object sender, EventArgs e)
        {
            object obj = this.listPatient.SelectedItem;
            if (obj == null) return;
            this.Clear();
            patient = obj as Neusoft.HISFC.Models.RADT.PatientInfo;
            this.SetInofo(patient);
            isFresh = true;
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return base.OnSave(sender, neuObject);
        }

        private void lvBed_DoubleClick(object sender, EventArgs e)
        {
            if (lvBed.Items.Count == 0) return;
            if (lvBed.SelectedItems.Count == 0) return;
            ListViewItem lvi = lvBed.SelectedItems[0];
            lblBed.Text = lvi.Text;
            lblBed.Tag = lvi.Tag;
        }

        private void txtPatientNo_myEvent()
        {
            if (this.txtPatientNo.InpatientNo == null || this.txtPatientNo.InpatientNo.Trim() == string.Empty)
            {
                MessageBox.Show(Language.Msg("û�и�סԺ��,����֤������") + this.txtPatientNo.Err);
                this.txtPatientNo.Focus();
                this.txtPatientNo.TextBox.SelectAll();
                return;
            }
            this.patient = this.radtInteger.GetPatientInfomation(this.txtPatientNo.InpatientNo);
            if (this.patient == null)
            {
                MessageBox.Show(Language.Msg("��û��߻�����Ϣ����!") + this.txtPatientNo.Err);
                this.txtPatientNo.Focus();
                this.txtPatientNo.TextBox.SelectAll();
                return;
            }
            this.SetInofo(patient);
            deptObj.ID = patient.PVisit.PatientLocation.Dept.ID;
            deptObj.Name = patient.PVisit.PatientLocation.Dept.Name;
            this.GetBedByDeptCode(deptObj);
            isFresh = false;
        }
        #endregion
    }
}
