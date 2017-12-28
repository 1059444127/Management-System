using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.WinForms.WorkStation.Controls
{
    public partial class tvPatientList : Neusoft.HISFC.Components.Common.Controls.tvPatientList
    {
        public tvPatientList()
        {
            InitializeComponent();

            //
            this.AddPatient();
        }

        private void AddPatient()
        { 
                ArrayList al = new ArrayList();
                al.Add("�ֹܻ���|patient");
                
                try
                {
                    ArrayList al1 = new ArrayList();
                    al1 = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QueryPatientByEmpl( Neusoft.FrameWork.Management.Connection.Operator.ID, ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID );
                    foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                    {
                        al.Add(PatientInfo1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show( "���ҷֹܻ��߳���\n��" + ex.Message + Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.Err );
                }

                al.Add("�����һ���|DeptPatient");
                try
                {
                    ArrayList al1 = new ArrayList();
                    al1 = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QueryPatientByDept( ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID );
                    foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                    {
                        al.Add(PatientInfo1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show( "���ҿ��һ��߳���\n��" + ex.Message + Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.Err );
                }

              

                al.Add("���ڳ�Ժ����|OutPatient");

                try
                {
                    ArrayList al1 = new ArrayList();
                    al1 = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QueryPatientByDept( ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID, 7 );
                    foreach (Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo1 in al1)
                    {
                        al.Add(PatientInfo1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("�������ڳ�Ժ���߳���\n��" + ex.Message + Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.Err);
                }

                this.SetPatient(al);
              
           
        }

    }
}
