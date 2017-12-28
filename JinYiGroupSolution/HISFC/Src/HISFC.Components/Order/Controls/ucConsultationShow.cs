using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: �������]<br></br>
    /// [�� �� ��: �·�]<br></br>
    /// [����ʱ��: 2009-07-22]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucConsultationShow : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucConsultationShow()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// ���ϵ����ת������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.RADT managerRADT = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee per = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;

        /// <summary>
        /// ҵ���
        /// </summary>
        private Neusoft.FrameWork.Management.DataBaseManger dbManager = new Neusoft.FrameWork.Management.DataBaseManger();

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        private int Init()
        {
            InitTvPatientList();
            return 1;
        }

        /// <summary>
        /// ��ʼ�������б�
        /// </summary>
        /// <returns></returns>
        private int InitTvPatientList()
        {
            this.tvPatientList.Nodes.Clear();
            TreeNode tnRoot = new TreeNode();
            tnRoot.Name = "ConsultationPatient";
            tnRoot.Text = "���һ��ﻼ��";
            tnRoot.Tag = "ConsultationPatient";
            this.tvPatientList.Nodes.Add(tnRoot);
            DateTime dt = dbManager.GetDateTimeFromSysDateTime();
            DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
            DateTime dt2 = new DateTime(dt.Year, dt.AddDays(1).Month, dt.AddDays(1).Day, 0, 0, 0, 0);
            ArrayList al1 = this.managerRADT.QueryPatientByConsultation(dbManager.Operator, dt1, dt2, per.Dept.ID);
            if (al1 == null)
            {
                MessageBox.Show(this.managerRADT.Err);
            }

            foreach (Neusoft.HISFC.Models.RADT.PatientInfo patientInfo in al1)
            {
                TreeNode tn = new TreeNode();
                tn.Name = patientInfo.ID;
                tn.Text = "��" + patientInfo.PVisit.PatientLocation.Bed.ID.Remove(0, 4) + "��" + patientInfo.Name;
                tn.Tag = patientInfo;
                this.tvPatientList.Nodes["ConsultationPatient"].Nodes.Add(tn);
            }
            this.tvPatientList.ExpandAll();

            //this.tv = this.tvPatientList;
            return 1;
        }

        #endregion

        #region �¼�

        protected override void OnLoad(EventArgs e)
        {
            this.Init();
            base.OnLoad(e);
        }

        private void tvPatientList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Name == "ConsultationPatient")
            {
                return;
            }
            Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = e.Node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
            this.ucConsultation1.IsApply = false;
            this.ucConsultation1.InpatientNo = patientInfo.ID;
        }

        #endregion
    }
}