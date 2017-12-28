using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ShenYangCitySI
{
    /// <summary>
    /// [��������: ҽ����̬�⺯��������]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2006-10-12]<br></br>
    /// �޸ļ�¼
    /// �޸���='ţ��Ԫ'
    ///	�޸�ʱ��=''
    ///	�޸�Ŀ��='�ḻҽ����Ϣ'
    ///	�޸�����=''
    ///  >
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// ��̬���ʼ������
        /// </summary>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int InitDLL();

        /// <summary>
        /// �ύ����
        /// </summary>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int CommitTrans();

        /// <summary>
        /// ����ع�
        /// </summary>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int RollbackTrans();

        /// <summary>
        /// ������ȡ������Ϣ
        /// </summary>
        /// <param name="readType"></param>
        /// <param name="dataBuffer"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int ReadCard(int readType, StringBuilder dataBuffer);
       
        /// <summary>
        /// �����ʸ����
        /// </summary>
        /// <param name="cardNO"></param>
        /// <param name="SINumber"></param>
        /// <param name="unitNumber"></param>
        /// <param name="sysDate"></param>
        /// <param name="appCode"></param>
        /// <param name="dataBuffer"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int CheckMTQ(string cardNO, string SINumber, string unitNumber, string sysDate, int appCode, StringBuilder dataBuffer);

        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="apprNO"></param>
        /// <param name="inHosNO"></param>
        /// <param name="apprType"></param>
        /// <param name="personNO"></param>
        /// <param name="PID"></param>
        /// <param name="Name"></param>
        /// <param name="sex"></param>
        /// <param name="personType"></param>
        /// <param name="unitNO"></param>
        /// <param name="doctorName"></param>
        /// <param name="diseaseNO"></param>
        /// <param name="diseaseName"></param>
        /// <param name="diagnostics"></param>
        /// <param name="itemNO"></param>
        /// <param name="itemName"></param>
        /// <param name="apprFlag"></param>
        /// <param name="reportDate"></param>
        /// <param name="apprPerson"></param>
        /// <param name="apprDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="transactor"></param>
        /// <param name="transDate"></param>
        /// <param name="remarks"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int GetApprInfo(string apprNO, StringBuilder inHosNO, StringBuilder apprType, StringBuilder personNO, StringBuilder PID,
            StringBuilder Name, StringBuilder sex, StringBuilder personType, StringBuilder unitNO, StringBuilder doctorName, StringBuilder diseaseNO,
            StringBuilder diseaseName, StringBuilder diagnostics, StringBuilder itemNO, StringBuilder itemName, StringBuilder apprFlag, StringBuilder reportDate,
            StringBuilder apprPerson, StringBuilder apprDate, StringBuilder startDate, StringBuilder endDate, StringBuilder transactor, StringBuilder transDate,
            StringBuilder remarks, StringBuilder errorMsg);

        /// <summary>
        /// д������Ϣ
        /// </summary>
        /// <param name="apprNO"></param>
        /// <param name="inHosNO"></param>
        /// <param name="apprType"></param>
        /// <param name="personNO"></param>
        /// <param name="PID"></param>
        /// <param name="Name"></param>
        /// <param name="sex"></param>
        /// <param name="personType"></param>
        /// <param name="unitNO"></param>
        /// <param name="doctorName"></param>
        /// <param name="diseaseNO"></param>
        /// <param name="diseaseName"></param>
        /// <param name="diagnostics"></param>
        /// <param name="itemNO"></param>
        /// <param name="itemName"></param>
        /// <param name="apprFlag"></param>
        /// <param name="reportDate"></param>
        /// <param name="apprPerson"></param>
        /// <param name="apprDate"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="transactor"></param>
        /// <param name="transDate"></param>
        /// <param name="remarks"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int SetApprInfo(StringBuilder apprNO, string inHosNO, string apprType, string personNO, string PID,
            string Name, string sex, string personType, string unitNO, string doctorName, string diseaseNO,
            string diseaseName, string diagnostics, string itemNO, string itemName, string apprFlag, string reportDate,
            string apprPerson, string apprDate, string startDate, string endDate, string transactor, string transDate,
            string remarks, StringBuilder errorMsg);

        /// <summary>
        /// ����Һ�
        /// </summary>
        /// <param name="personAccountInfo"></param>
        /// <param name="transType"></param>
        /// <param name="medType"></param>
        /// <param name="billNO"></param>
        /// <param name="inHosNO"></param>
        /// <param name="sysDate"></param>
        /// <param name="userName"></param>
        /// <param name="diseaseNO"></param>
        /// <param name="diseaseName"></param>
        /// <param name="dataBuffer"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int Registration(string personAccountInfo, int transType, string medType, string billNO, 
            string inHosNO, string sysDate, string userName, string diseaseNO, string diseaseName, StringBuilder dataBuffer);
        
        /// <summary>
        ///סԺ�Ǽ� 
        /// </summary>
        /// <param name="regType">�Ǽ�����  0 ��Ժ�Ǽ� 1 ��Ժ�Ǽ�</param>
        /// <param name="transType">�������� 1������ -1  ������</param>
        /// <param name="inHosNO">סԺ��</param>
        /// <param name="medType">ҽ�����21-��ͨסԺ 22--ת��ҽԺ 25--��ͥ����42--����סԺ43--����סԺ45������ת��סԺ</param>
        /// <param name="treatDate">��Ժ����</param>
        /// <param name="leaveHosDt">��Ժ����</param>
        /// <param name="diseaseName">��Ժ��������</param>
        /// <param name="diseaseNO">��Ժ��������(ҽ�������ṩ�ı�����Ϣ)</param>
        /// <param name="LHDiseaseName">��Ժ��������</param>
        /// <param name="LHDiseaseNO">��Ժ��������(ҽ�������ṩ�ı�����Ϣ</param>
        /// <param name="transactor">������</param>
        /// <param name="transDate">��������</param>
        /// <param name="billNO">��Ժԭ��(ҽ�����ĸ�����)</param>
        /// <param name="errorMsg">������Ϣ</param>
        /// <returns>�ɹ� 0 ʧ�� -1</returns>
        [DllImport("DBLib.dll")]
        public static extern int TreatInfoEntry(int regType, int transType, string inHosNO, string medType, string treatDate, 
            string leaveHosDt, string diseaseName, string diseaseNO, string LHDiseaseName, string LHDiseaseNO, 
            string transactor, string transDate, string billNO, StringBuilder errorMsg);

        /// <summary>
        /// ������ϸ¼�뼰���޸�
        /// </summary>
        /// <param name="inHosNO">�����(סԺ��)</param>
        /// <param name="billNO">���ݺ�</param>
        /// <param name="internalCode">�շ���ĿҽԺ�ڱ�����</param>
        /// <param name="formularyNO">������</param>
        /// <param name="sysDate">��������</param>
        /// <param name="centerCode">�շ���Ŀҽ�����ı���</param>
        /// <param name="itemName">�շ���Ŀ����</param>
        /// <param name="unitPrice">����</param>
        /// <param name="quantity">����</param>
        /// <param name="Amount">���</param>
        /// <param name="doseType">����</param>
        /// <param name="dosage">����</param>
        /// <param name="frequency">Ƶ��</param>
        /// <param name="usage">�÷�</param>
        /// <param name="KeBie">�Ʊ�����</param>
        /// <param name="execDays">ִ������</param>
        /// <param name="feeType">ҽ�������շ����</param>
        /// <param name="selfPayInd">1ȫ���Է�0���Է�</param>
        /// <param name="ErrorMsg">������Ϣ</param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int FormularyEntry(string inHosNO, string billNO, string internalCode, string formularyNO, 
            string sysDate, string centerCode, string itemName, double unitPrice, int quantity, double Amount, string doseType, 
            string dosage, string frequency, string usage, string KeBie, int execDays, string feeType, ref int selfPayInd, 
            StringBuilder ErrorMsg);

        /// <summary>
        /// ҽ������Ԥ����
        /// </summary>
        /// <param name="calcType">�������1����Ժ����2����;���㣩</param>
        /// <param name="medType">ҽ�����(NOTNULL)��11�C��ͨ����12--��������(�涨���ֻ��������Բ�)41-��������43-��������21-סԺ,22--ת��ҽԺ24--����סԺ25--��ͥ����42--����סԺ43--����סԺ45������ת��סԺ</param>
        /// <param name="inHosNO">סԺ�ţ�����ţ�(NOTNULL)</param>
        /// <param name="personAccountInfo">���˼����ʻ���Ϣ(���������ɹܵ��ָ�����|������)</param>
        /// <param name="sysDate">ϵͳʱ��(NOTNULL)</param>
        /// <param name="diseaseNO">��ϴ���(��Ҫ�����������ⲡ��)</param>
        /// <param name="diseaseName">�������(��Ҫ�����������ⲡ��)</param>
        /// <param name="sreimflag">���������־0������1����</param>
        /// <param name="DataBuffer">������(����ִ�гɹ�)�����ԭ��(����ִ��ʧ��)</param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int PreExpenseCalc(string calcType, string medType, string inHosNO, string personAccountInfo, 
            string sysDate, string diseaseNO, string diseaseName, int sreimflag, StringBuilder dataBuffer);

        /// <summary>
        /// ҽ�����߽���
        /// </summary>
        /// <param name="transType">�������͡���-1������(�˷�)1�������ף�(NOTNULL)</param>
        /// <param name="calcType">�������1����Ժ(����)����2����;����</param>
        /// <param name="medType">ҽ�����(NOTNULL)��11�C��ͨ����12--��������(�涨���ֻ��������Բ�)13���Ƕ���ҽ�ƻ�������41-��������43-��������21---סԺ22--ת��ҽԺ24--����סԺ25--��ͥ����26�C��ؼ���42--����סԺ43--����סԺ45������ת��סԺ</param>
        /// <param name="inHosNO">סԺ�ţ�����ţ�(NOTNULL)</param>
        /// <param name="billNO">���ݺ�(��Ʊ��)(NOTNULL)</param>
        /// <param name="personAccountInfo">���˼����ʻ���Ϣ(���������ɹܵ��ָ�����|������)</param>
        /// <param name="userName">����Ա����</param>
        /// <param name="sysDate">ϵͳʱ��(NOTNULL)</param>
        /// <param name="diseaseNO">��ϴ���(��Ҫ�����������ⲡ��)</param>
        /// <param name="diseaseName">�������(��Ҫ�����������ⲡ��)</param>
        /// <param name="sreimflag">���������־0������1����</param>
        /// <param name="dataBuffer"></param>
        /// <returns>������(����ִ�гɹ�)�����ԭ��(����ִ��ʧ��)</returns>
        [DllImport("DBLib.dll")]
        public static extern int ExpenseCalc(int transType, string calcType, string medType, string inHosNO, string billNO, 
             string personAccountInfo, string userName, string sysDate, string diseaseNO, string diseaseName, int sreimflag, StringBuilder dataBuffer);

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="inputString"></param>
        /// <param name="DataBuffer"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int Bussiness(string operType, string inputString,  string DataBuffer);

        /// <summary>
        /// ���µ��ݺ�(��Ʊ��)
        /// </summary>
        /// <param name="operType"></param>
        /// <param name="inputString"></param>
        /// <param name="DataBuffer"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int UpdateInvoiceNo(ref string origInvoiceNO, ref string newInvoiceNO, ref string transactor, ref string transDate, 
            ref int appCode, ref string dataBuffer);

        /// <summary>
        /// �ַ����ֽ⺯��
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        [DllImport("DBLib.dll")]
        public static extern int GetPosValue(int pos, string sourceString);

        [DllImport("DBLib.dll")]
        public static extern int OpenCOM();

        [DllImport("DBLib.dll")]
        public static extern int ReleaseCOM();

    }
}
