using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Neusoft.HISFC.Models.HealthRecord.EnumServer;
namespace Neusoft.HISFC.BizProcess.Integrate.HealthRecord
{
    public class HealthRecordBase : IntegrateBase
    {
        protected Neusoft.HISFC.BizLogic.HealthRecord.ICD icdMgr = new Neusoft.HISFC.BizLogic.HealthRecord.ICD();
        protected Neusoft.HISFC.BizLogic.HealthRecord.Diagnose diagMgr = new Neusoft.HISFC.BizLogic.HealthRecord.Diagnose();
        public override void SetTrans(System.Data.IDbTransaction trans)
        {
            this.trans = trans;
            icdMgr.SetTrans(trans);
            diagMgr.SetTrans(trans);
        }
        /// <summary>
        /// �����Ӧ��ѯ����ICD��Ϣ 
        /// </summary>
        /// <param name="ICDType">�������ö��</param>
        /// <param name="queryType">��ѯ����ö��</param>
        public ArrayList ICDQuery(ICDTypes ICDType, QueryTypes queryType)
        {
            this.SetDB(icdMgr);
            return icdMgr.Query(ICDType, queryType);
        } 
         /// <summary>
        /// ɾ��һ�����ߵ����в��������Ϣ
        /// </summary>
        /// <param name="InpatientNO">string ����סԺ��ˮ��</param>
        /// <returns>int 0 �ɹ� -1 ʧ��</returns>
        public int DeleteDiagnoseAll(string InpatientNO, Neusoft.HISFC.Models.HealthRecord.EnumServer.frmTypes OperType)
        {
            this.SetDB(diagMgr);
            return diagMgr.DeleteDiagnoseAll(InpatientNO, OperType);
        }
        /// <summary>
        /// ���벡�������Ϣ
        /// </summary>
        /// <param name="Item">Neusoft.HISFC.Models.HealthRecord.Diagnose</param>
        /// <returns>int 0 �ɹ� -1 ʧ��</returns>
        public int InsertDiagnose(Neusoft.HISFC.Models.HealthRecord.Diagnose Item)
        {
            this.SetDB(diagMgr);
            return diagMgr.InsertDiagnose(Item);
        }
         /// <summary>
        /// ��ò�����ϱ��еĻ��������Ϣ,����Ѿ��в����Ļ��߲�ѯ 
        /// </summary>
        /// <param name="InpatientNO">סԺ��ˮ��</param>
        /// <param name="diagType">������� ������ϣ���Ժ��ϵ� ��ѯ���еĿ������� %</param>
        /// <param name="OperType">"DOC"��ѯҽ��վ¼��������Ϣ ��CAS" ��ѯ������¼��������Ϣ</param>
        /// <returns>�����Ϣ����Ԫ����: Neusoft.HISFC.Models.HealthRecord.Diagnose</returns>
        public ArrayList QueryCaseDiagnose(string InpatientNO, string diagType, Neusoft.HISFC.Models.HealthRecord.EnumServer.frmTypes OperType)
        {
            this.SetDB(diagMgr);
            return diagMgr.QueryCaseDiagnose(InpatientNO,diagType, OperType);
        }
        /// <summary>
        /// ���²�����ϼ�¼
        /// </summary>
        /// <param name="dgMc">Neusoft.HISFC.Models.HealthRecord.Diagnose</param>
        /// <returns>int 0 �ɹ� -1 ʧ��</returns>
        public int UpdateDiagnose(Neusoft.HISFC.Models.HealthRecord.Diagnose dg)
        {
            this.SetDB(diagMgr);
            return diagMgr.UpdateDiagnose(dg);
        }
        public int DeleteDiagnoseSingle(string InpatientNO, int happenNO, Neusoft.HISFC.Models.HealthRecord.EnumServer.frmTypes OperType)
        {
            this.SetDB(diagMgr);
            return diagMgr.DeleteDiagnoseSingle(InpatientNO, happenNO, OperType);
        }

        /// <summary>
        /// ��ѯ�������,������������� met_com_diagnose {5F752A30-7971-4b65-A84B-D233EF2A4406}
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <returns></returns>
        public ArrayList QueryDiagnoseNoOps(string inpatientNO)
        {
            this.SetDB(diagMgr);
            return diagMgr.QueryDiagnoseNoOps(inpatientNO);
        }

        #region {6EF7D73B-4350-4790-B98C-C0BD0098516E}

        /// <summary>
        /// ��ѯ���пƳ������
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public ArrayList QueryDeptDiag(string deptID)
        {
            this.SetDB(icdMgr);

            return icdMgr.QueryDeptDiag(deptID);
        }

        #endregion

    }
}
