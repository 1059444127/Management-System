using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.Models.RADT;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.BizLogic.Operation;

namespace Neusoft.WinForms.Report.Operation
{
    /// <summary>
    /// [��������: ������]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-01]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public static class Environment
    {
        #region �ֶ�

        private static Neusoft.HISFC.BizProcess.Integrate.Manager integrateManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        private static Neusoft.HISFC.BizProcess.Integrate.RADT radtManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        private static Neusoft.HISFC.BizProcess.Integrate.Operation.Operation operationManager = new Neusoft.HISFC.BizProcess.Integrate.Operation.Operation();
        private static Neusoft.HISFC.BizLogic.Operation.OpsTableManage tableManager = new Neusoft.HISFC.BizLogic.Operation.OpsTableManage();
        private static Neusoft.HISFC.BizProcess.Integrate.Operation.OpsRecord recordManager = new Neusoft.HISFC.BizProcess.Integrate.Operation.OpsRecord();
        private static Neusoft.HISFC.BizProcess.Integrate.Operation.AnaeRecord anaeManager = new Neusoft.HISFC.BizProcess.Integrate.Operation.AnaeRecord();
        private static System.Collections.ArrayList alAnes;     //��������
        private static System.Collections.ArrayList alPayKind;  //�������
        private static System.Collections.ArrayList alDept;     //�����б�
        //private static Neusoft.FrameWork.Public.ObjectHelper payKindHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        private static Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
        private static Neusoft.HISFC.BizProcess.Integrate.Operation.OperationReport reportManager = new Neusoft.HISFC.BizProcess.Integrate.Operation.OperationReport();
        #endregion

        #region ����
        public static string OperatorID
        {
            get
            {
                return Neusoft.FrameWork.Management.Connection.Operator.ID;
            }
        }

        public static string OperatorDeptID
        {
            get
            {
                return (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID;
            }
        }

        /// <summary>
        /// ���������ڿ���
        /// </summary>
        public static NeuObject OperatorDept
        {
            get
            {
                return (Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).Dept;
            }
        }

        public static Neusoft.HISFC.BizProcess.Integrate.Manager IntegrateManager
        {
            get
            {
                return integrateManager;
            }
        }

        public static Neusoft.HISFC.BizProcess.Integrate.RADT RadtManager
        {
            get
            {
                return radtManager;
            }
        }

        public static Neusoft.HISFC.BizProcess.Integrate.Operation.Operation OperationManager
        {
            get
            {
                return operationManager;
            }
        }

        public static Neusoft.HISFC.BizLogic.Operation.OpsTableManage TableManager
        {
            get
            {
                return tableManager;
            }
        }

        public static OpsRecord RecordManager
        {
            get
            {
                return recordManager;
            }
        }

        public static AnaeRecord AnaeManager
        {
            get
            {
                return anaeManager;
            }
        }

        public static bool DesignMode
        {
            get
            {
                return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");


            }
        }

        public static Neusoft.FrameWork.WinForms.Classes.Print Print
        {
            get
            {
                return print;
            }
        }

        public static Neusoft.HISFC.BizProcess.Integrate.Operation.OperationReport ReportManager
        {
            get
            {
                return reportManager;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// �õ�����
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static NeuObject GetAnes(string id)
        {
            if (alAnes == null)
            {
                alAnes = IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESTYPE);
            }

            foreach (NeuObject obj in alAnes)
            {
                if (obj.ID == id)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// �õ��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Robin   2006-12-12
        public static NeuObject GetPayKind(string id)
        {
            if (alPayKind == null)
            {
                alPayKind = IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.PAYKIND);
            }

            foreach (NeuObject obj in alPayKind)
            {
                if (obj.ID == id)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// �õ�����
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns>����</returns>
        /// Robin   2006-12-13
        public static NeuObject GetDept(string id)
        {
            if (alDept == null)
            {
                alDept = IntegrateManager.GetDepartment();
            }

            foreach (NeuObject obj in alDept)
            {
                if (obj.ID == id)
                    return obj;
            }

            return null;
        }

        public static int GetPatientInfomation(Neusoft.HISFC.Models.Operation.OperationAppllication operationAppllication)
        {
            operationAppllication.PatientInfo = RadtManager.GetPatientInfomation(operationAppllication.PatientInfo.ID);

            return 0;
        }

        #endregion
    }
}
