using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ��վ�����б�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class tvDoctorPatientList:Neusoft.HISFC.Components.Common.Controls.tvPatientList
    {
        public tvDoctorPatientList()
        {
            if (DesignMode) return;
            //if (Neusoft.FrameWork.Management.Connection.Instance == null) return;
            #region {5646474F-BA9A-4fdb-B580-085C4EB757EB}
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
            {
                return;
            }
            #endregion
            try
            {
                this.ShowType = enuShowType.Bed;
                this.Direction = enuShowDirection.Ahead;
                this.RefreshInfo();
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        Neusoft.HISFC.BizProcess.Integrate.RADT manager = null;

        //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
        Neusoft.HISFC.BizLogic.Order.MedicalTeamForDoct medicalTeamForDoctBizlogic = null;

        public  void RefreshInfo()
        {
            this.Nodes.Clear();
            if (manager == null)
                manager = new Neusoft.HISFC.BizProcess.Integrate.RADT();

            ArrayList al = new ArrayList();
            Neusoft.HISFC.Models.Base.Employee per = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;

            //�ڵ�˵��
            al.Add("�ֹܻ���|patient");
            try
            {
                ArrayList al1 = new ArrayList();

                al1 = this.manager.QueryPatientByHouseDoc(per, Neusoft.HISFC.Models.Base.EnumInState.I, per.Dept.ID);
                foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                {
                    al.Add(PatientInfo1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ҷֹܻ��߳���\n��" + ex.Message + this.manager.Err);

            }

            al.Add("�����һ���|DeptPatient");
            addPatientList(al, Neusoft.HISFC.Models.Base.EnumInState.I);

            al.Add("���ﻼ��|ConsultationPatient");

            try
            {
                ArrayList al1 = new ArrayList();
                Neusoft.FrameWork.Management.DataBaseManger dbManager = new Neusoft.FrameWork.Management.DataBaseManger();
                DateTime dt = dbManager.GetDateTimeFromSysDateTime();
                DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
                DateTime dt2 = new DateTime(dt.Year, dt.AddDays(1).Month, dt.AddDays(1).Day, 0, 0, 0, 0);
                al1 = this.manager.QueryPatientByConsultation(dbManager.Operator, dt1, dt2, per.Dept.ID);
                foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                {
                    al.Add(PatientInfo1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("���һ��ﻼ�߳���\n��" + ex.Message + this.manager.Err);
            }

            al.Add("��Ȩ����|PermissionPatient");

            try
            {
                ArrayList al1 = new ArrayList();
                al1 = this.manager.QueryPatientByPermission(Neusoft.FrameWork.Management.Connection.Operator);
                foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                {
                    al.Add(PatientInfo1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("������Ȩ���߳���\n��" + ex.Message + this.manager.Err);
            }

            try
            {
                al.Add("���һ���|QueryPatient");
            }
            catch (Exception ex)
            {
                MessageBox.Show("���һ��߳���\n��" + ex.Message + this.manager.Err);
            }
            //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
            al.Add("ҽ�����ڻ���|TeamPatient");

            if (this.medicalTeamForDoctBizlogic == null)
            {
                this.medicalTeamForDoctBizlogic = new Neusoft.HISFC.BizLogic.Order.MedicalTeamForDoct();
            }


            List<Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct> medicalTeamForDoctList =
                this.medicalTeamForDoctBizlogic.QueryQueryMedicalTeamForDoctInfo(per.Dept.ID, per.ID);
            if (medicalTeamForDoctList == null)
            {
                MessageBox.Show("��ѯҽ����ʧ��!\n" + this.medicalTeamForDoctBizlogic.Err );
            }
             
            if (medicalTeamForDoctList.Count > 0)
            {
                Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct medcialObj = medicalTeamForDoctList[0];

                addPatientListMedialTeam(al, Neusoft.HISFC.Models.Base.EnumInState.I, medcialObj.MedcicalTeam.ID);


            }

            this.SetPatient(al);

        }

        /// <summary>
        /// ����ҽ������վ�õ�����
        /// </summary>
        /// <param name="al"></param>
        private void addPatientList(ArrayList al, Neusoft.HISFC.Models.Base.EnumInState Status)
        {
            ArrayList al1 = new ArrayList();
            try
            {
                Neusoft.HISFC.Models.Base.Employee per = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
                al1 = this.manager.QueryPatient(per.Dept.ID,Status);
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ҿ��һ��߳���\n��" + ex.Message + this.manager.Err);
            }
            //foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
            //{
            //    al.Add(PatientInfo1);
            //}
            al.AddRange(al1);
        }

        //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
        private void addPatientListMedialTeam(ArrayList al, Neusoft.HISFC.Models.Base.EnumInState Status,string medcialTeamCode)
        {
            ArrayList al1 = new ArrayList();
            try
            {
                Neusoft.HISFC.Models.Base.Employee per = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
                al1 = this.manager.PatientQueryByMedicalTeam(medcialTeamCode, Status,per.Dept.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ҿ��һ��߳���\n��" + ex.Message + this.manager.Err);
            }
            //foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
            //{
            //    al.Add(PatientInfo1);
            //}
            al.AddRange(al1);
        }

        #region addby xuewj 2009-8-24 ��ӻ��߲�ѯ���ܣ�����Ȩ�޿����Ƿ��ܲ鿴ȫԺ���� {8B4B8C49-2181-4aeb-95D4-DADFDE26DBC2}

        /// <summary>
        /// ����סԺ��ˮ�Ų�ѯ������Ϣ
        /// </summary>
        /// <param name="patientInfo"></param>
        public void QueryPaitent(string inpatientNO, Neusoft.HISFC.Models.Base.Employee empl)
        {
            if (inpatientNO == "")
            {
                return;
            }

            Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = this.manager.QueryPatientInfoByInpatientNO(inpatientNO);

            if (patientInfo == null)
            {
                MessageBox.Show("��ѯ���߻�����Ϣʧ��!");
                return;
            }

            int returnValue=this.PreArrange(empl);

            int branch = -1;
            branch = GetBrach("QueryPatient");

            if (returnValue == -1)
            {
                //ֻ�ܲ鿴���Ƴ�Ժ����
                if (patientInfo.PVisit.PatientLocation.Dept.ID == empl.Dept.ID)
                //&& (patientInfo.PVisit.InState.ID.ToString() == "P"
                //|| patientInfo.PVisit.InState.ID.ToString() == "D"
                //|| patientInfo.PVisit.InState.ID.ToString() == "T"))
                {
                    this.Nodes[branch].Nodes.Clear();
                    this.AddTreeNode(branch, patientInfo);
                    this.SelectedNode = this.Nodes[branch].Nodes[0];
                }
                else
                {
                    MessageBox.Show("��û��Ȩ�޲鿴�������ҵĻ���ҽ��");
                }
            }
            else
            {
                this.Nodes[branch].Nodes.Clear();
                this.AddTreeNode(branch, patientInfo);
                this.SelectedNode = this.Nodes[branch].Nodes[0];
            }
        }

        /// <summary>
        /// Ȩ���ж�
        /// </summary>
        /// <returns></returns>
        private int PreArrange(Neusoft.HISFC.Models.Base.Employee empl)
        {
            if(Neusoft.HISFC.Components.Common.Classes.Function.ChoosePiv("0001")==false)
            {
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// get selectedNode's index
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int GetBrach(string tag)
        {
            int branch = -1;
            foreach (TreeNode treeNode in this.Nodes)
            {
                if (treeNode.Tag != null && treeNode.Tag.ToString() == tag)
                {
                    branch = treeNode.Index;
                    break;
                }
            }
            return branch;
        }

        #endregion

    }
}
