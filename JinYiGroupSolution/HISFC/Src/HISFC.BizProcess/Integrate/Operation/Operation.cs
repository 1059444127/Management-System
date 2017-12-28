using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Neusoft.HISFC.BizProcess.Integrate.Operation
{
    /// <summary>
    /// [��������: ����ҵ��]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-31]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class Operation : Neusoft.HISFC.BizLogic.Operation.Operation
    {

#region �ֶ�
        private Neusoft.HISFC.BizLogic.RADT.InPatient inPatientManager = new Neusoft.HISFC.BizLogic.RADT.InPatient();
        private Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Manager();
        private Neusoft.HISFC.BizProcess.Integrate.RADT radtManager = new RADT();

        private Neusoft.HISFC.BizLogic.Operation.OpsDiagnose diagMgr = new Neusoft.HISFC.BizLogic.Operation.OpsDiagnose();
        //��Ͽ�����ʵ��
        // TODO: ����ҵ��㣬��Ҫ�ؼ�
        //public Neusoft.HISFC.BizLogic.HealthRecord.Diagnose DiagnoseManager = new Neusoft.HISFC.BizLogic.HealthRecord.Diagnose();

#endregion
        /// <summary>
        /// ���ݻ���סԺ�Ż�õ�ǰ��Ժ��סԺ��ˮ��
        /// </summary>
        /// <param name="PatientNo"></param>
        /// <returns></returns>
        private string GetInPatientNo(string PatientNo)
        {
            string strInpatientNo = string.Empty;
            ArrayList al = new ArrayList();
            try
            {
                al = this.inPatientManager.QueryInpatientNOByPatientNO(PatientNo);
                if (al == null)
                {
                    return strInpatientNo;
                }

                foreach (Neusoft.FrameWork.Models.NeuObject obj in al)
                {
                    //Ѱ�һ�����Ժ״̬������סԺ�����¼
                    if (obj.Memo == "I")
                    {
                        strInpatientNo = obj.ID.ToString();
                        break;
                    }
                }
            }
            catch
            {
                return strInpatientNo;
            }
            return strInpatientNo;
        }


        public new void SetTrans(System.Data.IDbTransaction trans)
        {
            base.SetTrans(trans);
            // TODO: ����������
            //this.DiagnoseManager.SetTrans(trans);
            this.inPatientManager.SetTrans(trans);
            this.manager.SetTrans(trans);
            this.radtManager.SetTrans(trans);
            diagMgr.SetTrans(trans);
        }

        protected override string GetEmployeeName(string id)
        {
            return this.manager.GetEmployeeInfo(id).Name;
        }

        protected override Neusoft.HISFC.Models.RADT.PatientInfo GetPatientInfo(string id)
        {
            return this.radtManager.GetPatientInfomation(id);
        }
        /// <summary>
        /// ����������Ż�����������Ϣ�б�
        /// </summary>
        /// <param name="OperatorNo">�������뵥����</param>
        /// <returns>���ߵ�������϶�������</returns>
        public override ArrayList GetIcdFromApp(Neusoft.HISFC.Models.Operation.OperationAppllication opsApp)
        {
            ArrayList IcdAl = new ArrayList();
            ArrayList rtnAl = new ArrayList();

            //����סԺ��ˮ��strInPatientNo			
            switch (opsApp.PatientSouce)
            {
                case "1"://��������
                    break;
                case "2"://סԺ����
                    string strInPatientNo = string.Empty;//����סԺ��ˮ�� 
                    strInPatientNo = opsApp.PatientInfo.ID.ToString();
                    try
                    {
                        //TODO:����ҵ���
                        IcdAl = diagMgr.QueryOpsDiagnose(strInPatientNo, "7");//"7"Ϊ��ǰ�������
                        foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase diag in IcdAl)
                        {
                            if (diag.OperationNo == opsApp.ID)
                                rtnAl.Add(diag);
                        }
                    }
                    catch
                    {
                        return rtnAl;
                    }
                    break;
            }
            return rtnAl;
        }

    }
}
