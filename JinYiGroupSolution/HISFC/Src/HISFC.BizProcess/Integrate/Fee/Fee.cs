using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.FrameWork.Management;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.BizProcess.Integrate.FeeInterface;
using System.Collections;
using System.Data;
using Neusoft.HISFC.Models.Fee.Outpatient;
using System.Windows.Forms;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Registration;
using System.Reflection;
using Neusoft.HISFC.BizProcess.Interface.FeeInterface;

namespace Neusoft.HISFC.BizProcess.Integrate
{
    /// <summary>
    /// [��������: ���ϵ����ת������]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2006-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class Fee : IntegrateBase, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {

        #region liuq 2007-8-23 ׷��
        #region �����ڲ��ַ�Ʊ��������

        /// <summary>
        /// ���ﰴ��ִ�п���,��С���õȷַ�Ʊ
        /// </summary>
        /// <param name="payKindCode">���ߵķ������</param>
        /// <param name="feeItemLists">���ߵ����������ϸ</param>
        /// <returns>�ɹ� �ֺõķ�����ϸ,ÿ��ArrayList����һ��Ӧ�����ɷ�Ʊ�ķ�����ϸ ʧ�� null</returns>
        public ArrayList SplitInvoice(string payKindCode, ref ArrayList feeItemLists)
        {

            // ����Ƿ���ִ�п��ҷַ�Ʊ,Ĭ�ϲ�ˢ�²���,Ĭ��ֵΪ false��������ִ�п��ҷַ�Ʊ.
            bool isSplitByExeDept = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.IS_SPLIT_INVOICE_BY_EXEDEPT, false, false);

            //�����Ʊ
            ArrayList exeGroupList = new ArrayList();

            if (isSplitByExeDept)
            {
                //����ִ�п��ҷ���
                exeGroupList = CollectFeeItemListsByExeDeptCode(feeItemLists);
            }
            else
            {
                exeGroupList = feeItemLists;
            }

            //����Ƿ�����С�ַ�Ʊ,Ĭ�ϲ�ˢ�²���,Ĭ��ֵΪ false����������С�ַ�Ʊ.
            bool isSplitByFeeCode = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.IS_SPLIT_INVOICE_BY_FEECODE, false, false);

            ArrayList finalSplitList = new ArrayList();

            if (isSplitByFeeCode)
            {
                foreach (ArrayList groupList in exeGroupList)
                {
                    ArrayList spList = this.SplitInvoiceByFeeCode(payKindCode, groupList);

                    foreach (ArrayList list in spList)
                    {
                        finalSplitList.Add(list);
                    }
                }
            }
            else
            {
                finalSplitList = exeGroupList;
            }

            //feeItemLists = new ArrayList();

            foreach (ArrayList list in finalSplitList)
            {
                foreach (FeeItemList f in list)
                {
                    feeItemLists.Add(f);
                }
            }

            return finalSplitList;
        }



        /// <summary>
        /// ��ö�Ӧ֧����ʽ�İ�����С������Ŀ�ַ�Ʊ����ϸ��Ŀ
        /// </summary>
        /// <param name="payKindCode">���ߵ�֧����ʽ���</param>
        /// <returns></returns>
        private int GetSplitCount(string payKindCode)
        {
            int count = 0;

            switch (payKindCode)
            {
                case "01":
                    count = controlParamIntegrate.GetControlParam<int>(Neusoft.HISFC.BizProcess.Integrate.Const.SPLIT_INVOICE_BY_FEECODE_ZF_COUNT, false, 5);
                    break;
                case "02":
                    count = controlParamIntegrate.GetControlParam<int>(Neusoft.HISFC.BizProcess.Integrate.Const.SPLIT_INVOICE_BY_FEECODE_YB_COUNT, false, 5);
                    break;
                case "03":
                    count = controlParamIntegrate.GetControlParam<int>(Neusoft.HISFC.BizProcess.Integrate.Const.SPLIT_INVOICE_BY_FEECODE_GF_COUNT, false, 5);
                    break;
            }

            return count;
        }

        /// <summary>
        /// ������С���÷���ϸ
        /// </summary>
        /// <param name="payKindCode">���ߵ�֧����ʽ���</param>
        /// <param name="feeItemList">������ϸ</param>
        /// <returns></returns>
        private ArrayList SplitInvoiceByFeeCode(string payKindCode, ArrayList feeItemList)
        {
            ArrayList sortList = this.CollectFeeItemListsByFeeCode(feeItemList);

            ArrayList finalList = new ArrayList();

            foreach (ArrayList list in sortList)
            {
                ArrayList sortFeeCodeList = this.SplitByFeeCodeCount(payKindCode, list);

                foreach (ArrayList fList in sortFeeCodeList)
                {
                    finalList.Add(fList);
                }
            }

            return finalList;
        }

        /// <summary>
        /// ������С����������������ϸ
        /// </summary>
        /// <param name="payKindCode">���ߵ�֧����ʽ���</param>
        /// <param name="feeItemLists">������ϸ</param>
        /// <returns></returns>
        private ArrayList SplitByFeeCodeCount(string payKindCode, ArrayList feeItemLists)
        {
            int count = this.GetSplitCount(payKindCode);

            ArrayList splitArrayList = new ArrayList();
            ArrayList groupList = new ArrayList();

            while (feeItemLists.Count > count)
            {
                groupList = new ArrayList();

                for (int i = 0; i < count; i++)
                {
                    FeeItemList f = feeItemLists[0] as FeeItemList;

                    groupList.Add(f);
                }
                foreach (FeeItemList f in groupList)
                {
                    feeItemLists.Remove(f);
                }
                splitArrayList.Add(groupList);
            }
            if (feeItemLists.Count > 0)
            {
                splitArrayList.Add(feeItemLists);
            }

            return splitArrayList;
        }

        /// <summary>
        /// ������С��������
        /// </summary>
        /// <param name="feeItemLists">������ϸ</param>
        /// <returns>�ɹ� ����õĴ�����ϸ ʧ�� null</returns>
        private ArrayList CollectFeeItemListsByFeeCode(ArrayList feeItemLists)
        {
            feeItemLists.Sort(new SortFeeItemListByFeeCode());

            ArrayList sorList = new ArrayList();

            while (feeItemLists.Count > 0)
            {
                ArrayList sameFeeItemLists = new ArrayList();
                FeeItemList compareItem = feeItemLists[0] as FeeItemList;
                foreach (FeeItemList f in feeItemLists)
                {
                    if (f.Item.MinFee.ID == compareItem.Item.MinFee.ID)
                    {
                        sameFeeItemLists.Add(f);
                    }
                    else
                    {
                        break;
                    }
                }
                sorList.Add(sameFeeItemLists);
                foreach (FeeItemList f in sameFeeItemLists)
                {
                    feeItemLists.Remove(f);
                }
            }

            return sorList;
        }

        /// <summary>
        /// ����ִ�п�������
        /// </summary>
        /// <param name="feeItemLists">������ϸ</param>
        /// <returns>�ɹ� ����õĴ�����ϸ ʧ�� null</returns>
        private ArrayList CollectFeeItemListsByExeDeptCode(ArrayList feeItemLists)
        {
            feeItemLists.Sort(new SortFeeItemListByExeDeptCode());

            ArrayList sorList = new ArrayList();

            while (feeItemLists.Count > 0)
            {
                ArrayList sameFeeItemLists = new ArrayList();
                FeeItemList compareItem = feeItemLists[0] as FeeItemList;
                foreach (FeeItemList f in feeItemLists)
                {
                    if (f.ExecOper.Dept.ID == compareItem.ExecOper.Dept.ID)
                    {
                        sameFeeItemLists.Add(f);
                    }
                    else
                    {
                        break;
                    }
                }
                sorList.Add(sameFeeItemLists);
                foreach (FeeItemList f in sameFeeItemLists)
                {
                    feeItemLists.Remove(f);
                }
            }

            return sorList;
        }

        /// <summary>
        /// ������
        /// </summary>
        private class SortFeeItemListByExeDeptCode : IComparer
        {
            #region IComparer ��Ա

            public int Compare(object x, object y)
            {
                if (x is FeeItemList && y is FeeItemList)
                {
                    return ((FeeItemList)x).ExecOper.Dept.ID.CompareTo(
                        ((FeeItemList)y).ExecOper.Dept.ID);
                }
                else
                {
                    return -1;
                }
            }

            #endregion
        }

        /// <summary>
        /// ������
        /// </summary>
        private class SortFeeItemListByFeeCode : IComparer
        {
            #region IComparer ��Ա

            public int Compare(object x, object y)
            {
                if (x is FeeItemList && y is FeeItemList)
                {
                    return ((FeeItemList)x).Item.MinFee.ID.CompareTo(
                        ((FeeItemList)y).Item.MinFee.ID);
                }
                else
                {
                    return -1;
                }
            }

            #endregion
        }

        #endregion

        #endregion

        #region ���ݽӿ�ʵ�ַַ�Ʊ
        /// <summary>
        /// ���ݽӿ�ʵ�ַַ�Ʊ
        /// </summary>
        /// <param name="register"></param>
        /// <param name="feeItemLists"></param>
        /// <returns></returns>
        public ArrayList SplitInvoice(Neusoft.HISFC.Models.Registration.Register register, ref ArrayList feeItemLists)
        {
            ISplitInvoice myISplitInvoice = null;
            if (this.trans == null)
            {
                myISplitInvoice = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitInvoice)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitInvoice;
            }
            else
            {
                myISplitInvoice = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitInvoice), this.trans) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitInvoice;
            }
            //{698738F7-D127-47b6-868E-4161E3949CF5}
            if (myISplitInvoice == null)
            {
                this.Err = "����ά������ַ�Ʊ�ķ�ʽ";
                return null;
            }
            return myISplitInvoice.SplitInvoice(register, ref feeItemLists);
        }

        #endregion

        #region ����

        /// <summary>
        /// ������ҵ��� {2CEA3B1D-2E59-44ac-9226-7724413173C5} ��ҵ��������ȫ����Ϊ�Ǿ�̬�ı���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Fee.InPatient inpatientManager = new Neusoft.HISFC.BizLogic.Fee.InPatient();

        /// <summary>
        /// item
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Fee.Item itemManager = new Neusoft.HISFC.BizLogic.Fee.Item();

        /// <summary>
        /// ��ҩƷ����
        /// </summary>
        //protected static Neusoft.HISFC.BizLogic.Fee.UndrugComb undrugCombManager = new Neusoft.HISFC.BizLogic.Fee.UndrugComb();

        /// <summary>
        /// /// ��Ʊҵ���
        /// </summary>
        //protected static Neusoft.HISFC.BizLogic.Fee.InvoiceService invoiceServiceManager = new Neusoft.HISFC.BizLogic.Fee.InvoiceService();
        protected  Neusoft.HISFC.BizLogic.Fee.InvoiceServiceNoEnum invoiceServiceManager = new Neusoft.HISFC.BizLogic.Fee.InvoiceServiceNoEnum();

        /// <summary>
        /// ������ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Fee.EmployeeFinanceGroup employeeFinanceGroupManager = new Neusoft.HISFC.BizLogic.Fee.EmployeeFinanceGroup();

        /// <summary>
        /// ������ҵ���
        /// </summary>
        protected  Neusoft.FrameWork.Management.ControlParam controlManager = new Neusoft.FrameWork.Management.ControlParam();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        protected Neusoft.HISFC.BizLogic.Fee.Outpatient outpatientManager = new Neusoft.HISFC.BizLogic.Fee.Outpatient();

        /// <summary>
        /// ����ҽ��ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Order.OutPatient.Order orderOutpatientManager = new Neusoft.HISFC.BizLogic.Order.OutPatient.Order();

        /// <summary>
        /// ҽ��ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();

        /// <summary>
        /// �ն�ԤԼҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Terminal.Terminal terminalManager = new Neusoft.HISFC.BizLogic.Terminal.Terminal();

        ////{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E} ����ҽ���鴦��
        protected Neusoft.HISFC.BizLogic.Order.MedicalTeamForDoct medicalTeamForDoctBizLogic = new Neusoft.HISFC.BizLogic.Order.MedicalTeamForDoct();

        /// <summary>
        /// ҩƷҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizProcess.Integrate.Pharmacy pharmarcyManager = new Pharmacy();

        /// <summary>
        /// ����ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Manager();

        /// <summary>
        /// �Һ�ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Registration.Register registerManager = new Neusoft.HISFC.BizLogic.Registration.Register();

        /// <summary>
        /// ����ҽ��ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Fee.Interface interfaceManager = new Neusoft.HISFC.BizLogic.Fee.Interface();

        /// <summary>
        /// ��ǰҽ�����ѽӿ�
        /// </summary>
        protected  MedcareInterfaceProxy medcareInterfaceProxy = new MedcareInterfaceProxy();

        /// <summary>
        /// ��ͬ��λ��
        /// </summary>
        protected  Neusoft.HISFC.BizLogic.Fee.PactUnitInfo pactManager = new Neusoft.HISFC.BizLogic.Fee.PactUnitInfo();

        /// <summary>
        /// ����ʵ��(liu.xq)
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.RADT radtIntegrate = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// �Ƿ����ҽ�����ѽӿ�
        /// </summary>
        private bool isIgnoreMedcareInterface = false;

        /// <summary>
        /// ���Ʋ���ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam controlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

        /// <summary>
        /// ���ҵ���
        /// </summary>
        protected  Neusoft.HISFC.BizProcess.Integrate.PhysicalExamination.ExamiManager examiIntegrate = new Neusoft.HISFC.BizProcess.Integrate.PhysicalExamination.ExamiManager();
        /// <summary>
        /// Ա��
        /// </summary>
        Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();
        //������Ŀ��ϸҵ���
        Neusoft.HISFC.BizLogic.Fee.UndrugPackAge undrugPackAgeMgr = new Neusoft.HISFC.BizLogic.Fee.UndrugPackAge();

        /// <summary>
        /// ����ҵ���{BF01254E-3C73-43d4-A644-4B258438294E}
        /// </summary>
        protected Neusoft.HISFC.BizLogic.Fee.InvoiceJumpRecord invoiceJumpRecordMgr = new Neusoft.HISFC.BizLogic.Fee.InvoiceJumpRecord();
  
        /// <summary>
        /// �����շ��Ƿ���Ҫ���·�Ʊ��
        /// </summary>
        protected bool isNeedUpdateInvoiceNO = true;

        /// <summary>
        /// �Ƿ������Ժ״̬����סԺ����
        /// </summary>
        protected bool isIgnoreInstate = false;
        /// <summary>
        /// Ƿ����ʾ����
        /// </summary>
        private MessType messType = MessType.Y;

        #region �����ʻ�
        /// <summary>
        /// �����ʻ���ҵ���
        /// </summary>
        protected static Neusoft.HISFC.BizLogic.Fee.Account accountManager = new Neusoft.HISFC.BizLogic.Fee.Account();

        /// <summary>
        /// �˻���������
        /// </summary>
        protected static Neusoft.HISFC.BizProcess.Interface.Account.IPassWord ipassWord = null;
        #endregion
        /// <summary>
        /// �����շ�
        /// </summary>
        //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
        protected static Neusoft.HISFC.BizProcess.Integrate.Material.Material materialManager = new Neusoft.HISFC.BizProcess.Integrate.Material.Material();

        /// <summary>
        /// ��Ȩ�շ�
        /// </summary>
        protected static Neusoft.HISFC.BizLogic.Fee.EmpowerFee empowerFeeManager = new Neusoft.HISFC.BizLogic.Fee.EmpowerFee();

        /// <summary>
        /// �ն�ȷ��ҵ���
        /// </summary>
        protected static Terminal.Confirm confirmIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm();
        #endregion

        /// <summary>
        /// �������ݿ�����
        /// </summary>
        /// <param name="trans">���ݿ�����</param>
        public override void SetTrans(System.Data.IDbTransaction trans)
        {
            this.trans = trans;

            itemManager.SetTrans(trans);
            inpatientManager.SetTrans(trans);
            controlManager.SetTrans(trans);
            invoiceServiceManager.SetTrans(trans);
            employeeFinanceGroupManager.SetTrans(trans);
            //medcareInterfaceProxy.SetTrans(trans);
            outpatientManager.SetTrans(trans);
            orderManager.SetTrans(trans);
            orderOutpatientManager.SetTrans(trans);
            terminalManager.SetTrans(trans);
            pharmarcyManager.SetTrans(trans);
            registerManager.SetTrans(trans);
            interfaceManager.SetTrans(trans);
            managerIntegrate.SetTrans(trans);
            controlParamIntegrate.SetTrans(trans);
            examiIntegrate.SetTrans(trans);
            userManager.SetTrans(trans);
            undrugPackAgeMgr.SetTrans(trans);
            empowerFeeManager.SetTrans(trans);

            #region �����ʻ�
            accountManager.SetTrans(trans);
            #endregion
        }

        #region ����

        /// <summary>
        /// �Ƿ������Ժ״̬����סԺ����
        /// </summary>
        public bool IsIgnoreInstate
        {
            get
            {
                return this.isIgnoreInstate;
            }
            set
            {
                this.isIgnoreInstate = value;
            }
        }

        /// <summary>
        /// �����շ��Ƿ���Ҫ���·�Ʊ��
        /// </summary>
        public bool IsNeedUpdateInvoiceNO
        {
            get
            {
                return this.isNeedUpdateInvoiceNO;
            }
            set
            {
                this.isNeedUpdateInvoiceNO = value;
            }
        }

        /// <summary>
        /// ��ǰҽ�����ѽӿ�
        /// </summary>
        public MedcareInterfaceProxy MedcareInterfaceProxy
        {
            get
            {
                return medcareInterfaceProxy;
            }
        }

        /// <summary>
        /// �Ƿ����ҽ�����ѽӿ�
        /// </summary>
        public bool IsIgnoreMedcareInterface
        {
            set
            {
                this.isIgnoreMedcareInterface = value;
            }
        }
        /// <summary>
        /// �Ƿ��ж�Ƿ�ѣ�Ƿ���Ƿ���ʾ
        /// </summary>
        public MessType MessageType
        {
            set
            {
                messType = value;
            }
            get
            {
                return messType;
            }
        }

        #endregion

        #region ˽�з���

        /// <summary>
        /// �ж��շ���Ҫ�Ĳ����Ƿ�Ϸ�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemList">���߷�����ϸʵ��</param>
        /// <returns>�ɹ�: true ʧ�� false</returns>
        private bool IsValidFee(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList)
        {
            if (patient == null)
            {
                this.Err = Language.Msg("���߻�����Ϣû�и�ֵ");

                return false;
            }

            if (feeItemList == null)
            {
                this.Err = Language.Msg("���߷�����Ϣû�и�ֵ");

                return false;
            }

            if (feeItemList.FT.TotCost == 0)
            {
                this.Err = Language.Msg("�����ܶ��Ϊ�㣺\n���ۻ���������Ϊ0");

                return false;
            }

            if (patient.PVisit.PatientLocation.NurseCell.ID == null || patient.PVisit.PatientLocation.NurseCell.ID.Trim() == string.Empty)
            {
                this.Err = Language.Msg("���ֲ㻼�߻�ʿվ����û�и�ֵ!");

                return false;
            }

            if (feeItemList.ExecOper.Dept.ID == null || feeItemList.ExecOper.Dept.ID == string.Empty)
            {
                this.Err = Language.Msg("���ֲ�ִ�п���û�и�ֵ!");

                return false;
            }

            if (feeItemList.FTRate.ItemRate < 0)
            {
                this.Err = Language.Msg("�շѱ�����ֵ����!");

                return false;
            }
            feeItemList.Item.Price = Math.Round(feeItemList.Item.Price, 4);
            if (!Neusoft.FrameWork.Public.String.IsPrecisionValid(feeItemList.Item.Price, 10, 4))
            {
                this.Err = feeItemList.Item.Name + Language.Msg("�ļ۸񾫶Ȳ�����,��׼�ľ���Ӧ��ΪС����ǰ6λ,С�����4λ");

                return false;
            }
            feeItemList.Item.Qty = Math.Round(feeItemList.Item.Qty, 4);
            if (!Neusoft.FrameWork.Public.String.IsPrecisionValid(feeItemList.Item.Qty, 9, 4))
            {
                this.Err = feeItemList.Item.Name + Language.Msg("���������Ȳ�����,��׼�ľ���Ӧ��ΪС����ǰ5λ,С�����4λ");

                return false;
            }
            feeItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.TotCost, 2);
            if (!Neusoft.FrameWork.Public.String.IsPrecisionValid(feeItemList.FT.TotCost, 10, 2))
            {
                this.Err = feeItemList.Item.Name + Language.Msg("�Ľ��Ȳ�����,��׼�ľ���Ӧ��ΪС����ǰ8λ,С�����2λ");

                return false;
            }

            return true;
        }

        /// <summary>
        /// ������
        /// </summary>
        private class CompareFeeItemList : IComparer
        {
            #region IComparer ��Ա

            public int Compare(object x, object y)
            {
                if (x is Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList && y is Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList)
                {
                    return ((Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList)x).Item.MinFee.ID.CompareTo(
                        ((Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList)y).Item.MinFee.ID);
                }
                else
                {
                    return -1;
                }
            }

            #endregion
        }

        /// <summary>
        /// ���ô�����
        /// </summary>
        /// <param name="feeItemLists">������ϸ����</param>
        /// <param name="trans">���ݿ�����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        private int SetRecipeNO(ref ArrayList feeItemLists, System.Data.IDbTransaction trans)
        {
            this.SetDB(inpatientManager);
            inpatientManager.SetTrans(trans);

            ArrayList existRecipeNOLists = new ArrayList();

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in feeItemLists)
            {
                if (feeItemList.RecipeNO != null && feeItemList.RecipeNO != string.Empty)
                {
                    existRecipeNOLists.Add(feeItemList);
                }
            }

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in existRecipeNOLists)
            {
                feeItemLists.Remove(feeItemList);
            }

            ArrayList sortByMinFeeLists = this.CollectFeeItemLists(feeItemLists);

            foreach (ArrayList list in sortByMinFeeLists)
            {
                string recipeNO = string.Empty;
                int recipeSequenceNO = 1;
                Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList temp = list[0] as Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList;

                //if (temp.Item.IsPharmacy)
                if (temp.Item.ItemType == EnumItemType.Drug)
                {
                    recipeNO = inpatientManager.GetDrugRecipeNO();
                }
                else
                {
                    recipeNO = inpatientManager.GetUndrugRecipeNO();
                }

                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in list)
                {
                    feeItemList.RecipeNO = recipeNO;
                    feeItemList.SequenceNO = recipeSequenceNO;

                    recipeSequenceNO++;
                }
            }

            feeItemLists = new ArrayList();

            feeItemLists.AddRange(existRecipeNOLists);

            foreach (ArrayList list in sortByMinFeeLists)
            {
                feeItemLists.AddRange(list);
            }

            return 1;
        }

        /// <summary>
        /// ������С��������
        /// </summary>
        /// <param name="feeItemLists">������ϸ</param>
        /// <returns>�ɹ� ����õĴ�����ϸ ʧ�� null</returns>
        private ArrayList CollectFeeItemLists(ArrayList feeItemLists)
        {
            feeItemLists.Sort(new CompareFeeItemList());

            ArrayList sorList = new ArrayList();

            while (feeItemLists.Count > 0)
            {
                ArrayList sameFeeItemLists = new ArrayList();
                Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList compareItem = feeItemLists[0] as Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList;
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f in feeItemLists)
                {
                    if (f.Item.MinFee.ID == compareItem.Item.MinFee.ID)
                    {
                        if (f.ExecOper.Dept.ID == compareItem.ExecOper.Dept.ID)
                        {
                            sameFeeItemLists.Add(f);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                sorList.Add(sameFeeItemLists);
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f in sameFeeItemLists)
                {
                    feeItemLists.Remove(f);
                }
            }

            return sorList;
        }

        /// <summary>
        /// ��ҽ����Ϣת��Ϊ������Ϣ
        /// 
        /// {F5477FAB-9832-4234-AC7F-ED49654948B4} ���Ӳ��� ����patient��Ϣ
        /// </summary>
        /// <param name="orderList">ҽ����Ϣ</param>
        /// <returns>�ɹ� ������Ϣ ʧ�� null</returns>
        private ArrayList ConvertOrderToFeeItemList(Neusoft.HISFC.Models.RADT.PatientInfo patient,ArrayList orderList)
        {
            ArrayList feeItemLists = new ArrayList();

            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in orderList)
            {
                Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();

                feeItemList.Item = order.Item.Clone();

                if (order.HerbalQty == 0)
                {
                    order.HerbalQty = 1;
                }

                //{F5477FAB-9832-4234-AC7F-ED49654948B4}
                Neusoft.HISFC.BizLogic.Fee.PactUnitInfo pactManager = new Neusoft.HISFC.BizLogic.Fee.PactUnitInfo();
                decimal price = feeItemList.Item.Price;
                if (pactManager.GetPrice(patient, feeItemList.Item.ItemType, feeItemList.Item.ID, ref price) == -1)
                {
                    MessageBox.Show(Language.Msg("ȡ��Ŀ:") + feeItemList.Item.Name + Language.Msg("�ļ۸����!") + pactManager.Err);

                    return null;
                }
                feeItemList.Item.Price = price;

                feeItemList.Item.Qty = order.Qty * order.HerbalQty;
                //���Ӹ����ĸ�ֵ {AE53ACB5-3684-42e8-BF28-88C2B4FF2360}
                feeItemList.Days = order.HerbalQty;

                feeItemList.Item.PriceUnit = order.Unit;//��λ���¸�
                feeItemList.RecipeOper.Dept = order.ReciptDept.Clone();
                feeItemList.RecipeOper.ID = order.ReciptDoctor.ID;
                feeItemList.RecipeOper.Name = order.ReciptDoctor.Name;
                feeItemList.ExecOper = order.ExecOper.Clone();
                feeItemList.StockOper.Dept = order.StockDept.Clone();
                if (feeItemList.Item.PackQty == 0)
                {
                    feeItemList.Item.PackQty = 1;
                }
                feeItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber((feeItemList.Item.Price * feeItemList.Item.Qty / feeItemList.Item.PackQty), 2);
                feeItemList.FT.OwnCost = feeItemList.FT.TotCost;
                feeItemList.IsBaby = order.IsBaby;
                feeItemList.IsEmergency = order.IsEmergency;
                feeItemList.Order = order.Clone();
                feeItemList.ExecOrder.ID = order.User03;
                feeItemList.NoBackQty = feeItemList.Item.Qty;
                feeItemList.FTRate.OwnRate = 1;
                feeItemList.BalanceState = "0";
                feeItemList.ChargeOper = order.Oper.Clone();
                feeItemList.FeeOper = order.Oper.Clone();
                feeItemList.TransType = TransTypes.Positive;

                #region {10C9E65E-7122-4a89-A0BE-0DF62B65C647} д�븴����Ŀ���롢����
                feeItemList.UndrugComb.ID = order.Package.ID;
                feeItemList.UndrugComb.Name = order.Package.Name; 
                #endregion

                feeItemLists.Add(feeItemList);
            }

            return feeItemLists;
        }

        /// <summary>
        /// �������ʿۿ����
        /// </summary>
        /// <returns></returns>
        //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
        public void GetMatLoadDataDept(Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f)
        {
            //return ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;
            f.StockOper.Dept.ID = f.ExecOper.Dept.ID;
        }

        /// <summary>
        /// סԺ�����շ�
        /// </summary>
        /// <param name="patient">סԺ���߻�����Ϣ</param>
        /// <param name="feeItemLists">���û�ҽ����Ϣʵ��</param>
        /// <param name="payType">�շ�����(���ۻ����շ�)</param>
        /// <param name="transType">������ ������</param>
        /// <returns></returns>
        //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
        private int FeeManager(Neusoft.HISFC.Models.RADT.PatientInfo patient, ref ArrayList feeItemLists, ChargeTypes payType, TransTypes transType)
        {
          
            #region liu.xq
            this.radtIntegrate.SetTrans(this.trans);
            patient = this.radtIntegrate.GetPatientInfomation(patient.ID);

            if (patient.IsStopAcount) 
            {
                this.Err = "�û����Ѿ�����!���ܼ���¼����û��˷�,����סԺ����ϵ!";

                return -1;
            }

            //{02B13899-6FE7-4266-AC64-D3C0CDBBBC3F} Ӥ���ķ����Ƿ������ȡ����������
            string motherPayAllFee = this.controlParamIntegrate.GetControlParam<string>(SysConst.Use_Mother_PayAllFee, false, "0");
            if (motherPayAllFee == "1")//Ӥ���ķ���������������� 
            {
                if (patient.ID.IndexOf("B") > 0) //סԺ��ˮ�ź���B,������Ӥ��
                {
                    string motherInpatientNO = this.radtIntegrate.QueryBabyMotherInpatientNO(patient.ID);
                    if (string.IsNullOrEmpty(motherInpatientNO) || motherInpatientNO == "-1")
                    {
                        this.Err = "û���ҵ�Ӥ����ĸ��סԺ��ˮ��" + this.radtIntegrate.Err;

                        return -1;
                    }

                    patient = this.radtIntegrate.GetPatientInfomation(motherInpatientNO);//������Ļ�����Ϣ�滻Ӥ���Ļ�����Ϣ

                    object obj = feeItemLists[0];
                    if (obj is Neusoft.HISFC.Models.Order.Inpatient.Order)
                    {
                        feeItemLists = this.ConvertOrderToFeeItemList(patient, feeItemLists);
                        for (int i = 0; i < feeItemLists.Count; i++)
                        {
                            Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList = feeItemLists[i] as Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList;
                            feeItemList.IsBaby = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < feeItemLists.Count; i++)
                        {
                            Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList = feeItemLists[i] as Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList;
                            feeItemList.IsBaby = true;
                        }
                    }
                }
                else 
                {
                    object obj = feeItemLists[0];
                    if (obj is Neusoft.HISFC.Models.Order.Inpatient.Order)
                    {
                        feeItemLists = this.ConvertOrderToFeeItemList(patient, feeItemLists);
                    }
                }
            }
            else 
            {
                object obj = feeItemLists[0];
                if (obj is Neusoft.HISFC.Models.Order.Inpatient.Order)
                {
                    feeItemLists = this.ConvertOrderToFeeItemList(patient, feeItemLists);
                }
            }
            ////{02B13899-6FE7-4266-AC64-D3C0CDBBBC3F}���

            #endregion
           

            this.SetDB(inpatientManager);

            if (feeItemLists == null || feeItemLists.Count == 0)
            {
                return -1;
            }

              //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E}����ҽ���鴦��

            List<Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct> medicalTeamForDoctList = new List<Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct>();
           
            medicalTeamForDoctList = this.medicalTeamForDoctBizLogic.QueryQueryMedicalTeamForDoctInfo();

            Hashtable hsMedicalTeam = new Hashtable();

            //��ӹ�ϣ��
            foreach (Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct item in medicalTeamForDoctList)
            {
                string strAdd = item.MedcicalTeam.Dept.ID + "|" + item.Doct.ID;
                if (hsMedicalTeam.Contains(strAdd))
                {
                    continue;
                }

                hsMedicalTeam.Add(strAdd, item);
            }

            


            //ȡ���ϵĵ�һ��Ԫ���ж��Ƿ�����ϸ(FeeItemList����Order)

            long returnValue = 0;
            this.MedcareInterfaceProxy.SetPactTrans(this.trans);
            //������ýӿ�û�г�ʼ��,��ô���ݻ��ߵĺ�ͬ��λ��ʼ�����ýӿ�
            if (medcareInterfaceProxy != null)
            {
                returnValue = MedcareInterfaceProxy.SetPactCode(patient.Pact.ID);

                MedcareInterfaceProxy.SetTrans(this.trans);

                if (returnValue == -1 && this.isIgnoreMedcareInterface == false)
                {
                    this.Err = MedcareInterfaceProxy.ErrMsg;

                    return -1;
                }
                returnValue = MedcareInterfaceProxy.Connect();

                if (returnValue == -1)
                {
                    this.Err = MedcareInterfaceProxy.ErrMsg;

                    return -1;
                }
            }

            //�ж���Ч��
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in feeItemLists)
            {
                //��Ч���ж�
                if (!this.IsValidFee(patient, feeItemList))
                {
                    return -1;
                }
                // //{AC6A5576-BA29-4dba-8C39-E7C5EBC7671E}����ҽ���鴦��
                if (string.IsNullOrEmpty(feeItemList.MedicalTeam.ID))
                {
                    string patientDept = ((Neusoft.HISFC.Models.RADT.PatientInfo)feeItemList.Patient).PVisit.PatientLocation.Dept.ID;
                    //�������ڿ���
                    //string patientDept = feeItemList.RecipeOper.Dept.ID;

                    if (hsMedicalTeam.Contains(patientDept + "|" + feeItemList.RecipeOper.ID))
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct obj = hsMedicalTeam[patientDept + "|" + feeItemList.RecipeOper.ID] as Neusoft.HISFC.Models.Order.Inpatient.MedicalTeamForDoct;
                        feeItemList.MedicalTeam = obj.MedcicalTeam;
                    }
                }
            }

            ////������ýӿ�û�г�ʼ��,��ô���ݻ��ߵĺ�ͬ��λ��ʼ�����ýӿ�
            //if (medcareInterfaceProxy != null)
            //{
            //    medcareInterfaceProxy.SetTrans(this.trans);

            //    returnValue = medcareInterfaceProxy.SetPactCode(patient.Pact.ID);

            //    if (returnValue == -1 && this.isIgnoreMedcareInterface == false)
            //    {
            //        this.Err = medcareInterfaceProxy.ErrMsg;

            //        return -1;
            //    }
            //    returnValue = medcareInterfaceProxy.Connect();

            //    if (returnValue == -1)
            //    {
            //        this.Err = medcareInterfaceProxy.ErrMsg;

            //        return -1;
            //    }

            //}

            //ִ�з��ýӿ�,�Ա����Ƚ��м�������¸�ֵ
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in feeItemLists)
            {
                returnValue = MedcareInterfaceProxy.RecomputeFeeItemListInpatient(patient, feeItemList);

                if (returnValue == -1 && this.isIgnoreMedcareInterface == false)
                {
                    this.Err = MedcareInterfaceProxy.ErrMsg;

                    return -1;
                }

                //Ϊ��ֹ���������ͳһת��Ϊ2λ��
                feeItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.TotCost, 2);
                feeItemList.FT.OwnCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.OwnCost, 2);
                feeItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.PayCost, 2);
                feeItemList.FT.PubCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.PubCost, 2);

                //��ֹ�յ���λ��ֺ��¼Ϊ0
                if (feeItemList.FT.TotCost == 0)
                {
                    return 1;
                }
            }

            //���·��䴦����
            this.SetRecipeNO(ref feeItemLists, this.trans);

            #region �����շѴ���
            //{CEA4E2A5-A045-4823-A606-FC5E515D824D}
            if (transType == TransTypes.Positive)
            {
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f in feeItemLists)
                {
                    //���ʵĿۿ�����ǵ�¼���ң�����ص��б�����ͬ�Ŀ��ң�
                    if (f.Item.ItemType != EnumItemType.Drug)
                    {
                        GetMatLoadDataDept(f);
                    }    
                }
                //�����շѴ���
                if (materialManager.MaterialFeeOutput(feeItemLists) < 0)
                {
                    this.Err = materialManager.Err;
                    return -1;
                }
            }
            else
            {

                //�˿�
                if (materialManager.MaterialFeeOutputBack(feeItemLists) < 0)
                {
                    this.Err = materialManager.Err;
                    return -1;
                }
            }
            #endregion
            //����ϸѭ������
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in feeItemLists)
            {
                //������շѲ���,��ô���������׺ͷ����׵����⸳ֵ
                if (payType == ChargeTypes.Fee)
                {
                    //����Ƿ�����,��Ҫ�жϸ��¿��������Ͱѽ��,�������и�������
                    if (transType == TransTypes.Negative)
                    {
                        //���¿������� Ȼ��ȡ���µĴ�����,ҩƷ�ͷ�ҩƷ�ֱ����(��ͬ)
                        //if (feeItemList.Item.IsPharmacy)
                        if (feeItemList.Item.ItemType == EnumItemType.Drug)
                        {
                            //�����ϸ��Ҫ���¿�������,��ô���и���,���������Դ���˷�����,ǰ̨��IsNeedUpdateNoBackQtyΪFalse
                            //˵���Ѿ����¹���������,����Ͳ��ý��и�����
                            if (feeItemList.IsNeedUpdateNoBackQty)
                            {
                                if (inpatientManager.UpdateNoBackQtyForDrug(feeItemList.RecipeNO, feeItemList.SequenceNO, feeItemList.Item.Qty, feeItemList.BalanceState) < 1)
                                {
                                    this.Err = Language.Msg("����ԭ�м�¼������������!") + feeItemList.Item.Name + Language.Msg("�����Ѿ����˷ѻ����!") + inpatientManager.Err;

                                    return -1;
                                }
                            }

                            //����µĴ�����
                            feeItemList.RecipeNO = inpatientManager.GetDrugRecipeNO();
                        }
                        else
                        {
                            if (feeItemList.IsNeedUpdateNoBackQty)
                            {
                                if (inpatientManager.UpdateNoBackQtyForUndrug(feeItemList.RecipeNO, feeItemList.SequenceNO, feeItemList.Item.Qty, feeItemList.BalanceState) < 1)
                                {
                                    this.Err = Language.Msg("����ԭ�м�¼������������!") + feeItemList.Item.Name + Language.Msg("�����Ѿ����˷ѻ����!") + inpatientManager.Err;

                                    return -1;
                                }
                            }

                            //����µĴ�����
                            feeItemList.RecipeNO = inpatientManager.GetUndrugRecipeNO();
                        }
                        //�γɸ���¼
                        feeItemList.Item.Qty = -feeItemList.Item.Qty;
                        feeItemList.FT.TotCost = -feeItemList.FT.TotCost;
                        feeItemList.FT.OwnCost = -feeItemList.FT.OwnCost;
                        feeItemList.FT.PayCost = -feeItemList.FT.PayCost;
                        feeItemList.FT.PubCost = -feeItemList.FT.PubCost;
                        feeItemList.FT.RebateCost = -feeItemList.FT.RebateCost;
                        feeItemList.TransType = TransTypes.Negative;
                        feeItemList.FeeOper.ID = inpatientManager.Operator.ID;
                        feeItemList.FeeOper.OperTime = inpatientManager.GetDateTimeFromSysDateTime();
                        if (feeItemList.BalanceState == null || feeItemList.BalanceState == string.Empty)
                        {
                            feeItemList.BalanceState = "0";
                        }
                    }

                    //���ֻ���ʱ����շ�ʱ��ͬ��
                    feeItemList.ChargeOper.OperTime = feeItemList.FeeOper.OperTime;
                    feeItemList.Patient.Pact.ID = patient.Pact.ID;
                    feeItemList.Patient.Pact.PayKind.ID = patient.Pact.PayKind.ID;
                    //��������ڶ����շ�ӦΪ0 ��ֱ���շ�Ӧ��Ϊ��ǰ���ߵĽ������+1
                    //feeItemList.BalanceNO = patient.BalanceNO;
                    feeItemList.FeeOper.ID = inpatientManager.Operator.ID;
                    if (feeItemList.FTRate.ItemRate == 0)
                    {
                        feeItemList.FTRate.ItemRate = 1;
                    }
                }

                //����ϸ��¼���շѻ��۱�־��ֵ
                if (payType == ChargeTypes.Fee)
                {
                    feeItemList.PayType = PayTypes.Balanced;
                }
                else
                {
                    feeItemList.PayType = PayTypes.Charged;
                }

                //���봦����ϸ��,�ֱ�ΪҩƷ,��ҩƷ
                //if (feeItemList.Item.IsPharmacy)
                if (feeItemList.Item.ItemType == EnumItemType.Drug)
                {
                    if (inpatientManager.InsertMedItemList(patient, feeItemList) == -1)
                    {
                        this.Err = Language.Msg("����ҩƷ��ϸ����!") + inpatientManager.Err;

                        return -1;
                    }
                }
                else
                {
                    if (inpatientManager.InsertFeeItemList(patient, feeItemList) == -1)
                    {
                        this.Err = Language.Msg("�����ҩƷ��ϸ����!") + inpatientManager.Err;

                        return -1;
                    }
                }

                //������·�����ϸ,�������Ҫ������µı�־,�������(���ڷ��ýӿڲ���)
                if (MedcareInterfaceProxy != null)
                {
                    if (MedcareInterfaceProxy.UpdateFeeItemListInpatient(patient, feeItemList) == -1)
                    {
                        this.Err = MedcareInterfaceProxy.ErrMsg;

                        return -1;
                    }
                }
            }
            ///���ô����ӿڴ���


            //���ڻ��ۺ��շ�����,���ϴ����ͨ��,����Ϊ�շ��뻮�۲�ͬ������,�շ���Ҫ������С����(MinFee.ID)����,������û��ܱ�
            //������סԺ����
            if (payType == ChargeTypes.Fee)
            {
                //decimal freeCost = patient.FT.LeftCost;//���
                //decimal moneyAlert = patient.PVisit.MoneyAlert;//������
                //decimal totCost = 0m;//���ý��
                //decimal surtyCost = 0m;//�������
                //if (this.MessageType != MessType.N)
                //{
                //    //���ҵ������
                //    string resultValue = inpatientManager.GetSurtyCost(patient.ID);
                //    if (resultValue == "-1")
                //    {
                //        this.Err = "���ҵ������ʧ�ܣ�";
                //        return -1;
                //    }
                //    surtyCost = NConvert.ToDecimal(resultValue);
                //}
                //�ϴ��ӿ���ϸ

                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList in feeItemLists)
                {
                    if (feeItemList.FT.TotCost < 0)
                    {

                        returnValue = this.MedcareInterfaceProxy.DeleteUploadedFeeDetailInpatient(patient, feeItemList);
                        if (returnValue != 1)
                        {
                            this.Err = "�����ӿ��ϴ��˷���ϸʧ��" + this.MedcareInterfaceProxy.ErrMsg;
                            return -1;

                        }

                    }
                    else
                    {
                        returnValue = this.MedcareInterfaceProxy.UploadFeeDetailInpatient(patient, feeItemList);
                        if (returnValue != 1)
                        {
                            this.Err = "�����ӿ��ϴ���ϸʧ��" + this.MedcareInterfaceProxy.ErrMsg;
                            return -1;

                        }

                    }
                }


                //��ð���MinFee.ID���������ݼ���

                ArrayList sorList = this.CollectFeeItemLists(feeItemLists);

                int iReturn = 0;
                //����һ������
                FT ftMain = new FT();

                foreach (ArrayList list in sorList)
                {
                    Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList temp = (list[0] as Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList).Clone();
                    temp.FT = new FT();

                    feeItemLists.AddRange(list);

                    foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemTot in list)
                    {
                        temp.FT.TotCost += feeItemTot.FT.TotCost;
                        temp.FT.OwnCost += feeItemTot.FT.OwnCost;
                        temp.FT.PayCost += feeItemTot.FT.PayCost;
                        temp.FT.PubCost += feeItemTot.FT.PubCost;

                        ftMain.TotCost += feeItemTot.FT.TotCost;
                        ftMain.OwnCost += feeItemTot.FT.OwnCost;
                        ftMain.PayCost += feeItemTot.FT.PayCost;
                        ftMain.PubCost += feeItemTot.FT.PubCost;
                    }

                    iReturn = inpatientManager.InsertAndUpdateFeeInfo(patient, temp);

                    if (iReturn <= 0)
                    {
                        this.Err = Language.Msg("������û�����Ϣ����!");

                        return -1;
                    }


                }
                //�ϴ���ϸ
                //returnValue = this.MedcareInterfaceProxy.GetRegInfoInpatient(patient);

                returnValue = MedcareInterfaceProxy.PreBalanceInpatient(patient, ref feeItemLists);
                if (returnValue != 1)
                {
                    this.Err = "�����ӿ�Ԥ����ʧ��" + this.MedcareInterfaceProxy.ErrMsg;
                    return -1;

                }

                //if (patient.SIMainInfo.OwnCost + patient.SIMainInfo.PayCost + patient.SIMainInfo.PubCost > 0)
                //{
                //    ftMain.OwnCost = patient.SIMainInfo.OwnCost;
                //    ftMain.PubCost = patient.SIMainInfo.PubCost;
                //    ftMain.PayCost = patient.SIMainInfo.PayCost;
                //    ftMain.TotCost = patient.SIMainInfo.PayCost + patient.SIMainInfo.OwnCost + patient.SIMainInfo.PubCost;
                //}
                if (patient.Pact.PayKind.ID == "02" || patient.Pact.PayKind.ID == "03")
                {
                    ftMain.OwnCost = patient.SIMainInfo.OwnCost;
                    ftMain.PubCost = patient.SIMainInfo.PubCost;
                    ftMain.PayCost = patient.SIMainInfo.PayCost;
                    ftMain.TotCost = patient.SIMainInfo.PayCost + patient.SIMainInfo.OwnCost + patient.SIMainInfo.PubCost;
                }

                #region Ƿ��������Ϣ�����ж�
                //�ж�Ƿ�� ���������+�������-��ǰ���ý��<������ 2007-08-23 ·־��
                if (transType == TransTypes.Positive)
                {
                    IFeeOweMessage feeOweMessage = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IFeeOweMessage)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IFeeOweMessage;
                    if (feeOweMessage == null)
                    {
                        //{A45EE85D-B1E3-4af0-ACAD-9DAF65610611}
                        #region ���ս���ж�
                        //{56F3CD2F-64A3-4bbe-9ECE-BBF5F6944412}
                        decimal freeCost = 0;
                        //{639C0ACE-3505-4dc5-97FB-87322CE3CC8E}
                        //if (patient.Pact.PayKind.ID == "02" || patient.Pact.PayKind.ID == "03")
                        //{
                        //    freeCost = patient.FT.PrepayCost;//Ԥ����
                        //}
                        //else
                        //{
                        //    freeCost = patient.FT.LeftCost;//���
                        //}
                        freeCost = patient.FT.LeftCost;//���
                        decimal moneyAlert = patient.PVisit.MoneyAlert;//������
                        decimal totCost = 0m;//���ý��
                        decimal surtyCost = 0m;//�������
                        if (this.MessageType != MessType.N)
                        {
                            //���ҵ������
                            string resultValue = inpatientManager.GetSurtyCost(patient.ID);
                            if (resultValue == "-1")
                            {
                                this.Err = "���ҵ������ʧ�ܣ�";
                                return -1;
                            }
                            surtyCost = NConvert.ToDecimal(resultValue);
                        }

                        totCost = ftMain.OwnCost;
                        decimal MessCost = freeCost + surtyCost - totCost - moneyAlert;
                        //����ʱ����жϵ�ʱ�䷶Χ
                        DateTime beginDate = NConvert.ToDateTime(patient.PVisit.BeginDate.ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime endDate = NConvert.ToDateTime(patient.PVisit.EndDate.ToString("yyyy-MM-dd") + " 23:59:59");
                        DateTime now = inpatientManager.GetDateTimeFromSysDateTime();
                        //Ƿ���ж�����
                        string alertType = patient.PVisit.AlertType.ID.ToString();
                        //�Ƿ�Ƿ��
                        bool isOwn = false;
                        //{6005F670-C1C6-43f6-BDD3-D5E37A628438} 20101120 Ƿ���ж�
                        if (patient.Pact.ID == "01")
                        {
                             isOwn = freeCost + surtyCost - totCost < moneyAlert;
                        }
                        else {
                             isOwn = freeCost + surtyCost - (totCost- patient.FT.TotCost) < moneyAlert;
                        }
                        bool isValid = true;

                        //����ʱ����ж������ʱ�䷶Χ�����ж�Ƿ��
                        if (alertType == EnumAlertType.D.ToString())
                        {
                            if (now >= beginDate && now <= endDate)
                            {
                                isValid = false;
                            }
                        }

                        if (isOwn && isValid)
                        {
                            if (MessageType == MessType.Y)
                            {
                                //{6C42FDE7-B167-429e-B89E-37E5845F8946} 20101126 xizf
                                if (patient.Pact.ID == "01")
                                {
                                    this.Err = "��������: " + patient.Name + "\n\n�� �� ��:" + moneyAlert.ToString() + "\n\nԤ �� ��:" + patient.FT.PrepayCost.ToString() + "\n\n�����ܶ�:" + patient.FT.TotCost.ToString() + "\n\n�Է��ܶ�:" + patient.FT.OwnCost.ToString() + "\n\n��  ��:" + freeCost.ToString() + "\n\n���η���:" + totCost.ToString() + " \n\n���㣬���ܽ����շѣ�" + "\n\n" + "�벹��" + (-MessCost).ToString() + "Ԫ";
                                    return -1;
                                }
                                else {
                                    this.Err = "��������: " + patient.Name + "\n\n�� �� ��:" + moneyAlert.ToString() + "\n\nԤ �� ��:" + patient.FT.PrepayCost.ToString() + "\n\n�����ܶ�:" + patient.FT.TotCost.ToString() + "\n\n�Է��ܶ�:" + patient.FT.OwnCost.ToString() + "\n\n��  ��:" + freeCost.ToString() + "\n\n���η���:" + (totCost - patient.FT.TotCost).ToString() + " \n\n���㣬���ܽ����շѣ�" + "\n\n" + "�벹��" + (-MessCost).ToString() + "Ԫ";
                                    return -1;
                                }
                                
                            }
                            if (MessageType == MessType.M)//{639C0ACE-3505-4dc5-97FB-87322CE3CC8E}
                            {
                                #region {403682AB-DF9E-4009-84EC-21CE355E3FA5} ҽ�����߱��η�����ʾ���� by guanyx
                                if (patient.Pact.ID == "01")
                                {
                                    if (MessageBox.Show("��������: " + patient.Name + "\n\n�� �� ��:" + moneyAlert.ToString() + "\n\nԤ �� ��:" + patient.FT.PrepayCost.ToString() + "\n\n�����ܶ�:" + patient.FT.TotCost.ToString() + "\n\n�Է��ܶ�:" + patient.FT.OwnCost.ToString() + "\n\n��    ��:" + freeCost.ToString() + "\n\n���η���:" + totCost.ToString()
                                        + "\n\n�û�������,�Ƿ�����շѣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        this.Err = "��ȡ���շѣ�";
                                        return -1;
                                    }
                                }
                                else
                                {
                                    if (MessageBox.Show("��������: " + patient.Name + "\n\n�� �� ��:" + moneyAlert.ToString() + "\n\nԤ �� ��:" + patient.FT.PrepayCost.ToString() + "\n\n�����ܶ�:" + patient.FT.TotCost.ToString() + "\n\n�Է��ܶ�:" + patient.FT.OwnCost.ToString() + "\n\n��    ��:" + freeCost.ToString() + "\n\n���η���:" + (totCost - patient.FT.TotCost).ToString()
                                        + "\n\n�û�������,�Ƿ�����շѣ�", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        this.Err = "��ȡ���շѣ�";
                                        return -1;
                                    }
                                }
                               
                                #endregion

                                Neusoft.HISFC.BizProcess.Interface.FeeInterface.IShowFrmValidUserPassWord isShowForm  = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IShowFrmValidUserPassWord)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IShowFrmValidUserPassWord;

                                if (isShowForm != null)
                                {
                                    string tempErr = string.Empty;
                                    bool isShow = isShowForm.ShowFrmValidUserPassWord(ref tempErr);
                                    if (!isShow)
                                    {
                                        this.Err = "��ȡ���շѣ�" + tempErr;
                                        return -1;
                                    }

                                    
                                }
                            }
                            freeCost -= totCost;
                        }
                        #endregion
                       

                    }
                    else
                    {
                        string err = string.Empty;
                        //{2518013C-40B2-4693-B494-3DE193C002FF} //�ӿڱ仯
                        bool bl = feeOweMessage.FeeOweMessage(patient, ftMain,feeItemLists,ref messType, ref err);
                        if (!bl)
                        {
                            //{492188AA-397C-4d8d-BABC-E0ECD25FD8F1} ������ʾ
                            //MessageBox.Show(err);
                            this.Err = err;
                            return -1;
                        }
                        else
                        {
                            if (messType == MessType.Y)
                            {
                                this.Err = err;
                                return -1;
                            }
                            if (messType == MessType.M)
                            {
                                if (MessageBox.Show(err, "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                {
                                    this.Err = "��ȡ���շѣ�";
                                    return -1;
                                }
                            }
                        }
                    }
                }
                #endregion

                //���������Ժ״̬,����ֱ���շѻ���,��ô�ڸ���סԺ�����ʱ���ж���Ժ״̬�Ƿ�Ϊ'O'
                if (this.isIgnoreInstate)
                {
                    iReturn = inpatientManager.UpdateInMainInfoFeeForDirQuit(patient.ID, ftMain);
                }
                else
                {
                    if (patient.Pact.PayKind.ID == "02" || patient.Pact.PayKind.ID == "03") //ҽ����������
                    {
                        iReturn = inpatientManager.UpdateInMainInfoFeeYB(patient.ID, ftMain);
                    }
                    else
                    {
                        iReturn = inpatientManager.UpdateInMainInfoFee(patient.ID, ftMain);
                    }
                }

                if (iReturn == -1)
                {
                    this.Err = Language.Msg("����סԺ����ʧ��!") + inpatientManager.Err;

                    return -1;
                }

                //�������Ϊ0 ˵��������in_state <> 0��������ǰ̨��������¼�����.
                if (iReturn == 0)
                {
                    this.Err = patient.Name + Language.Msg("�Ѿ�������ߴ��ڷ���״̬�����ܼ���¼�����!����סԺ����ϵ!");

                    return -1;
                }
            }

            return 1;

        }

        /// <summary>
        /// �շѺ���
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemList">������ϸʵ��</param>
        /// <param name="payType">���� �շѱ�־</param>
        /// <param name="transType">�շ������� �����ױ�־</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        private int FeeManager(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList,
            ChargeTypes payType, TransTypes transType)
        {
            ArrayList temp = new ArrayList();

            temp.Add(feeItemList);

            return this.FeeManager(patient, ref temp, payType, transType);
            //long returnValue = 0;

            //this.SetDB(inpatientManager);

            ////��Ч���ж�
            //if (!this.IsValidFee(patient, feeItemList))
            //{
            //    return -1;
            //}

            ////�����Ŀ�ı���û�и�ֵ,��ôĬ��Ϊ1
            //if (feeItemList.FTRate.ItemRate == 0)
            //{
            //    feeItemList.FTRate.ItemRate = 1;
            //}

            //if (medcareInterfaceProxy != null)
            //{
            //    returnValue = medcareInterfaceProxy.SetPactCode(patient.Pact.ID);

            //    if (returnValue == -1 && this.isIgnoreMedcareInterface == false)
            //    {
            //        this.Err = medcareInterfaceProxy.ErrMsg;

            //        return -1;
            //    }

            //    returnValue = medcareInterfaceProxy.RecomputeFeeItemListInpatient(patient, feeItemList);

            //    if (returnValue == -1 && this.isIgnoreMedcareInterface == false)
            //    {
            //        this.Err = medcareInterfaceProxy.ErrMsg;

            //        return -1;
            //    }
            //}

            ////Ϊ��ֹ���������ͳһת��Ϊ2λ��
            //feeItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.TotCost, 2);
            //feeItemList.FT.OwnCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.OwnCost, 2);
            //feeItemList.FT.PayCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.PayCost, 2);
            //feeItemList.FT.PubCost = Neusoft.FrameWork.Public.String.FormatNumber(feeItemList.FT.PubCost, 2);

            ////��ֹ�յ���λ��ֺ��¼Ϊ0
            //if (feeItemList.FT.TotCost == 0)
            //{
            //    return 1;
            //}

            ////������շѵĻ�,���������ϸ���˷Ѳ���
            //if (payType == ChargeTypes.Fee)
            //{
            //    //����Ǹ�����
            //    if (transType == TransTypes.Negative)
            //    {

            //        //���¿������� Ȼ��ȡ���µĴ�����
            //        if (feeItemList.Item.IsPharmacy)
            //        {
            //            if (feeItemList.IsNeedUpdateNoBackQty)
            //            {
            //                if (inpatientManager.UpdateNoBackQtyForDrug(feeItemList.RecipeNO, feeItemList.SequenceNO, feeItemList.Item.Qty, feeItemList.BalanceState) < 1)
            //                {
            //                    this.Err = Language.Msg("����ԭ�м�¼������������!") + feeItemList.Item.Name + Language.Msg("�����Ѿ����˷ѻ����!") + inpatientManager.Err;

            //                    return -1;
            //                }
            //            }

            //            //����µĴ�����
            //            feeItemList.RecipeNO = inpatientManager.GetDrugRecipeNO();
            //        }
            //        else
            //        {
            //            if (feeItemList.IsNeedUpdateNoBackQty)
            //            {
            //                if (inpatientManager.UpdateNoBackQtyForUndrug(feeItemList.RecipeNO, feeItemList.SequenceNO, feeItemList.Item.Qty, feeItemList.BalanceState) < 1)
            //                {
            //                    this.Err = Language.Msg("����ԭ�м�¼������������!") + feeItemList.Item.Name + Language.Msg("�����Ѿ����˷ѻ����!") + inpatientManager.Err;

            //                    return -1;
            //                }
            //            }

            //            //����µĴ�����
            //            feeItemList.RecipeNO = inpatientManager.GetUndrugRecipeNO();
            //        }
            //        //�γɸ���¼
            //        feeItemList.Item.Qty = -feeItemList.Item.Qty;
            //        feeItemList.FT.TotCost = -feeItemList.FT.TotCost;
            //        feeItemList.FT.OwnCost = -feeItemList.FT.OwnCost;
            //        feeItemList.FT.PayCost = -feeItemList.FT.PayCost;
            //        feeItemList.FT.PubCost = -feeItemList.FT.PubCost;
            //        feeItemList.FT.RebateCost = -feeItemList.FT.RebateCost;
            //        feeItemList.TransType = TransTypes.Negative;
            //        feeItemList.FeeOper.ID = inpatientManager.Operator.ID;
            //        feeItemList.FeeOper.OperTime = inpatientManager.GetDateTimeFromSysDateTime();

            //        if (feeItemList.BalanceState == null || feeItemList.BalanceState == string.Empty)
            //        {
            //            feeItemList.BalanceState = "0";
            //        }
            //    }
            //    //���ֻ���ʱ����շ�ʱ��ͬ��
            //    feeItemList.ChargeOper.OperTime = feeItemList.FeeOper.OperTime;
            //    feeItemList.Patient.Pact.ID = patient.Pact.ID;
            //    feeItemList.Patient.Pact.PayKind.ID = patient.Pact.PayKind.ID;
            //}

            //if (feeItemList.Item.IsPharmacy)
            //{
            //    if (inpatientManager.InsertMedItemList(patient, feeItemList) == -1)
            //    {
            //        this.Err = Language.Msg("����ҩƷ��ϸ����!") + inpatientManager.Err;

            //        return -1;
            //    }
            //}
            //else
            //{
            //    if (inpatientManager.InsertFeeItemList(patient, feeItemList) == -1)
            //    {
            //        this.Err = Language.Msg("�����ҩƷ��ϸ����!") + inpatientManager.Err;

            //        return -1;
            //    }
            //}

            //if (payType == ChargeTypes.Fee)
            //{
            //    int iReturn = inpatientManager.InsertAndUpdateFeeInfo(patient, feeItemList);

            //    if (iReturn <= 0)
            //    {
            //        this.Err = Language.Msg("������û�����Ϣ����!");

            //        return -1;
            //    }

            //    iReturn = inpatientManager.UpdateInMainInfoFee(patient.ID, feeItemList.FT);

            //    if (iReturn == -1)
            //    {
            //        this.Err = Language.Msg("����סԺ����ʧ��!") + inpatientManager.Err;

            //        return -1;
            //    }

            //    //�������Ϊ0 ˵��������in_state <> 0��������ǰ̨��������¼�����.
            //    if (iReturn == 0)
            //    {
            //        this.Err = patient.Name + Language.Msg("�Ѿ�������߳��ڷ���״̬�����ܼ���¼�����!����סԺ����ϵ!");

            //        return -1;
            //    }
            //}

            ////������·�����ϸ
            //if (medcareInterfaceProxy != null)
            //{
            //    if (medcareInterfaceProxy.UpdateFeeItemListInpatient(patient, feeItemList) == -1)
            //    {
            //        this.Err = medcareInterfaceProxy.ErrMsg;

            //        return -1;
            //    }
            //}

            //return 1;
        }

        #endregion

        #region ���з���

        public string GetUndrugCode()
        {
            this.SetDB(itemManager);
            return itemManager.GetUndrugCode();
        }

        #region סԺ
        /// <summary>
        /// �����Ч��,��Ŀ���Ϊ��������Ŀ����
        /// </summary>
        /// <returns>�ɹ�:��Ŀ���� ʧ�ܷ���null</returns>
        public ArrayList QueryOperationItems()
        {
            this.SetDB(itemManager);

            return itemManager.QueryOperationItems();
        }
        /// <summary>
        /// ��÷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="undrugCode"></param>
        /// <returns>�ɹ� ��ҩƷ��Ϣ ʧ�� null</returns>
        public Neusoft.HISFC.Models.Fee.Item.Undrug GetUndrugByCode(string undrugCode)
        {
            this.SetDB(itemManager);

            return itemManager.GetUndrugByCode(undrugCode);
        }
        /// <summary>
        /// ͨ�������ţ��õ�������ϸ
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <returns>null ���� ArrayList Fee.OutPatient.FeeItemListʵ�弯��</returns>
        public ArrayList QueryFeeDetailFromRecipeNO(string recipeNO)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.QueryFeeDetailFromRecipeNO(recipeNO);
        }

        /// <summary>
        /// ������￨����ˮ,Ĭ��Ϊ9λ�ֳ�,ǰ�油0
        /// </summary>
        /// <returns>�ɹ� ���￨�� ʧ�� null</returns>
        public string GetAutoCardNO()
        {
            this.SetDB(outpatientManager);

            return outpatientManager.GetAutoCardNO();
        }

        /// <summary>
        /// ���ݴ����źʹ�����Ŀ��ˮ�Ÿ���Ժע��ȷ������
        /// </summary>
        /// <param name="moOrder">ҽ����ˮ��</param>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSquence">��������ˮ��</param>
        /// <param name="qty">Ժע����</param>
        /// <returns>�ɹ�: >= 1 ʧ��: -1 û�и��µ����ݷ��� 0</returns>
        public int UpdateConfirmInject(string moOrder, string recipeNO, string recipeSquence, int qty)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.UpdateConfirmInject(moOrder, recipeNO, recipeSquence, qty);
        }

        /// <summary>
        /// �жϻ����Ƿ�Ƿ��
        /// </summary>
        /// <param name="patient"></param>
        /// <returns>true Ƿ�� false ��Ƿ��</returns>
        public bool IsPatientLackFee(Neusoft.HISFC.Models.RADT.PatientInfo patient)
        {
            #region ��ѯҽ��Ƿ���жϼ��밴�������õĹ��� {EE219CB3-16EB-4cbd-ACDF-233F3585E355} wbo 2010-12-25
            //if (patient.FT.LeftCost > patient.PVisit.MoneyAlert)
            //{
            //    return false;
            //}
            //����ʱ����жϵ�ʱ�䷶Χ
            DateTime beginDate = NConvert.ToDateTime(patient.PVisit.BeginDate.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime endDate = NConvert.ToDateTime(patient.PVisit.EndDate.ToString("yyyy-MM-dd") + " 23:59:59");
            DateTime now = inpatientManager.GetDateTimeFromSysDateTime();
            //Ƿ���ж�����
            string alertType = patient.PVisit.AlertType.ID.ToString();
            //����ʱ����ж������ʱ�䷶Χ�����ж�Ƿ��
            if (alertType == EnumAlertType.D.ToString())
            {
                if (now >= beginDate && now <= endDate)
                {
                    return false;
                }
            }
            else
            {
                if (patient.FT.LeftCost > patient.PVisit.MoneyAlert)
                {
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// ��ѯ���к�ͬ��λ
        /// </summary>
        /// <returns></returns>
        public ArrayList QueryPactUnitAll()
        {
            this.SetDB(pactManager);

            return pactManager.QueryPactUnitAll();
        }
        /// <summary>
        /// �ύ����
        /// </summary>
        /// ����HIS4.5.0.1��commit��ʽ�޸�
        public void Commit()
        {
            //this.trans.Commit();
            //if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null)
            //{
            //    medcareInterfaceProxy.Commit();
            //}
            if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null && medcareInterfaceProxy.PactCode != "" && medcareInterfaceProxy.PactCode != null)
            {
                if (medcareInterfaceProxy.Commit() < 0) //����ҽ�� 0�ɹ� -1ʧ��
                {
                    medcareInterfaceProxy.Rollback();
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    //this.trans.Rollback();
                }
                else
                {
                    //this.trans.Commit();
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    //{A6CDF67A-DEBE-4ce6-AC8B-CC0CAB9B1B0E}
                    medcareInterfaceProxy.Disconnect();
                }
            }
            else if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null && medcareInterfaceProxy.PactCode == "")
            {
                //this.trans.Commit()
                Neusoft.FrameWork.Management.PublicTrans.Commit();
            }
            else
            {
                //this.trans.Commit();
                Neusoft.FrameWork.Management.PublicTrans.Commit();
            }
        }
        /// <summary>
        /// �ύ���ѽӿں���
        /// </summary>
        /// ���ﵥ����ҩ���˷���ҩʱʹ���ˣ������ط������ҪҲ����ʹ��
        public int MedcareInterfaceCommit()
        {

            if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null && medcareInterfaceProxy.PactCode != "" && medcareInterfaceProxy.PactCode != null)
            {
                if (medcareInterfaceProxy.Commit() < 0) //����ҽ�� 0�ɹ� -1ʧ��
                {
                    medcareInterfaceProxy.Rollback();
                    return -1;
                }
                return 0;
            }
            return 0;
        }
        /// <summary>
        /// �ع����ѽӿں���
        /// </summary>
        public void MedcareInterfaceRollback()
        {
            if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null)
            {
                medcareInterfaceProxy.Rollback();
            }
        }
        /// <summary>
        /// �ع�����
        /// </summary>
        public void Rollback()
        {
            //this.trans.Rollback();
            Neusoft.FrameWork.Management.PublicTrans.RollBack();
            if (!this.isIgnoreMedcareInterface && medcareInterfaceProxy != null)
            {
                medcareInterfaceProxy.Rollback();
            }
        }
        /* {4E6415B8-8BFE-4fe5-8AC2-66DBCC887F71}
        /// <summary>
        /// ��÷�Ʊ��
        /// </summary>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <returns>�ɹ�: ��Ʊ�� ʧ��: null</returns>
        public string GetNewInvoiceNO(Neusoft.HISFC.Models.Fee.EnumInvoiceType invoiceType)
        {
            int leftQty = 0;//��Ʊʣ����Ŀ
            int alarmQty = 0;//��ƱԤ����Ŀ
            string finGroupID = string.Empty;//��Ʊ�����
            string newInvoiceNO = string.Empty;//��õķ�Ʊ��

            alarmQty = Neusoft.FrameWork.Function.NConvert.ToInt32(controlManager.QueryControlerInfo("100002"));

            finGroupID = inpatientManager.GetFinGroupInfoByOperCode(inpatientManager.Operator.ID).ID;

            if (finGroupID == null || finGroupID == string.Empty)
            {
                finGroupID = " ";
            }

            Neusoft.HISFC.BizLogic.Fee.FeeCodeStat feeCodeState = new Neusoft.HISFC.BizLogic.Fee.FeeCodeStat();

            if (this.trans != null)
            {
                feeCodeState.SetTrans(this.trans);
            }

            newInvoiceNO = inpatientManager.GetNewInvoiceNO(feeCodeState.Operator.ID, invoiceType, alarmQty, ref leftQty, finGroupID);

            if (newInvoiceNO == null || newInvoiceNO == string.Empty)
            {
                this.SetDB(inpatientManager);

                return null;
            }

            if (leftQty < alarmQty)
            {
                System.Windows.Forms.MessageBox.Show(Language.Msg("ʣ�෢Ʊ") + leftQty.ToString() + Language.Msg("��,�벹�췢Ʊ!"));
            }

            return newInvoiceNO;
        }
        */
        /// <summary>
        /// ����ȡ��Ʊ
        /// </summary>
        /// <param name="invoiceType">��Ʊ����R:�Һ��վ� C:�����վ� P:Ԥ���վ� I:סԺ�վ� A:�����˻�</param>
        /// <returns></returns>
        public string GetNewInvoiceNO(string invoiceType)
        {
            int leftQty = 0;//��Ʊʣ����Ŀ
            int alarmQty = 0;//��ƱԤ����Ŀ
            string finGroupID = string.Empty;//��Ʊ�����
            string newInvoiceNO = string.Empty;//��õķ�Ʊ��

            alarmQty = Neusoft.FrameWork.Function.NConvert.ToInt32(controlManager.QueryControlerInfo("100002"));

            finGroupID = inpatientManager.GetFinGroupInfoByOperCode(inpatientManager.Operator.ID).ID;

            if (finGroupID == null || finGroupID == string.Empty)
            {
                finGroupID = " ";
            }

            Neusoft.HISFC.BizLogic.Fee.FeeCodeStat feeCodeState = new Neusoft.HISFC.BizLogic.Fee.FeeCodeStat();

            if (this.trans != null)
            {
                feeCodeState.SetTrans(this.trans);
            }

            newInvoiceNO = inpatientManager.GetNewInvoiceNO(feeCodeState.Operator.ID, invoiceType, alarmQty, ref leftQty, finGroupID);

            if (newInvoiceNO == null || newInvoiceNO == string.Empty)
            {
                this.SetDB(inpatientManager);

                return null;
            }

            if (leftQty < alarmQty)
            {
                System.Windows.Forms.MessageBox.Show(Language.Msg("ʣ�෢Ʊ") + leftQty.ToString() + Language.Msg("��,�벹�췢Ʊ!"));
            }

            return newInvoiceNO;
        }

        /// <summary>
        /// ��Ŀ�շ�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemList">������ϸ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int FeeItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList)
        {
            return this.FeeManager(patient, feeItemList, ChargeTypes.Fee, TransTypes.Positive);
        }

        /// <summary>
        /// ��Ŀ�շ�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemLists">������ϸ����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int FeeItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, ref ArrayList feeItemLists)
        {
            return this.FeeManager(patient, ref feeItemLists, ChargeTypes.Fee, TransTypes.Positive);
        }

        /// <summary>
        /// ��Ŀ�˷�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemList">������ϸ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int QuitItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList)
        {
            return this.FeeManager(patient, feeItemList, ChargeTypes.Fee, TransTypes.Negative);
        }

        /// <summary>
        /// ��Ŀ�˷�
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemLists">������ϸ����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int QuitItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, ref ArrayList feeItemLists)
        {
            return this.FeeManager(patient, ref feeItemLists, ChargeTypes.Fee, TransTypes.Negative);
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemList">������ϸ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int ChargeItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItemList)
        {
            return this.FeeManager(patient, feeItemList, ChargeTypes.Charge, TransTypes.Positive);
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <param name="patient">���߻�����Ϣʵ��</param>
        /// <param name="feeItemLists">������ϸ����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int ChargeItem(Neusoft.HISFC.Models.RADT.PatientInfo patient, ref ArrayList feeItemLists)
        {
            return this.FeeManager(patient, ref feeItemLists, ChargeTypes.Charge, TransTypes.Positive);
        }

        /// <summary>
        /// ѭ�����������ϸ
        /// </summary>
        /// <param name="patient">סԺ���߻�����Ϣ</param>
        /// <param name="balanceLists">������ϸ����</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int InsertBalanceList(Neusoft.HISFC.Models.RADT.PatientInfo patient, ArrayList balanceLists)
        {
            this.SetDB(inpatientManager);

            int returnValue = 0;

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.BalanceList balanceList in balanceLists)
            {
                returnValue = inpatientManager.InsertBalanceList(patient, balanceList);
                if (returnValue == -1)
                {
                    return -1;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// ��÷�ƱĬ����ʼ��
        /// </summary>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <returns>�ɹ� ��ƱĬ����ʼ�� ʧ�� null</returns>
        //public string GetInvoiceDefaultStartCode(Neusoft.HISFC.Models.Fee.InvoiceTypeEnumService invoiceType)
        //{
        //    this.SetDB(invoiceServiceManager);

        //    return invoiceServiceManager.GetDefaultStartCode(invoiceType);
        //}

        public string GetInvoiceDefaultStartCode(string invoiceType)
        {
            this.SetDB(invoiceServiceManager);

            return invoiceServiceManager.GetDefaultStartCode(invoiceType);
        }

        /// <summary>
        /// ������з�Ʊ����Ϣ
        /// </summary>
        /// <returns>�ɹ� ��Ʊ����Ϣ ʧ�� null</returns>
        public ArrayList QueryFinaceGroupAll()
        {
            this.SetDB(employeeFinanceGroupManager);

            return employeeFinanceGroupManager.QueryFinaceGroupIDAndNameAll();
        }

        /// <summary>
        /// ��֤��Ʊ�����Ƿ�Ϸ�
        /// </summary>
        /// <param name="startNO">��ʼ��</param>
        /// <param name="endNO">������</param>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <returns>�Ϸ� True, ���Ϸ� false</returns>
        //public bool InvoicesIsValid(int startNO, int endNO, Neusoft.HISFC.Models.Fee.InvoiceTypeEnumService invoiceType)
        //{
        //    this.SetDB(invoiceServiceManager);

        //    return invoiceServiceManager.InvoicesIsValid(startNO, endNO, invoiceType);
        //}
        public bool InvoicesIsValid(int startNO, int endNO, string invoiceType)
        {
            this.SetDB(invoiceServiceManager);

            return invoiceServiceManager.InvoicesIsValid(startNO, endNO, invoiceType);
        }

        /// <summary>
        /// ��÷�Ʊ�����DataSet
        /// </summary>
        /// <param name="invoiceType">��Ʊ����</param>
        /// <param name="ds">��Ʊ�����DataSet</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int GetInvoiceClass(string invoiceType, ref DataSet ds)
        {
            this.SetDB(outpatientManager);
            // TODO: ���벻��ȥ����ʱ��һ��
            return outpatientManager.GetInvoiceClass(invoiceType, ref ds);
        }

        /// <summary>
        /// ��û���ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="deptCode">���ұ���</param>
        /// <returns></returns>
        public ArrayList QueryMedcineList(string inpatientNO, DateTime beginTime, DateTime endTime, string deptCode)
        {
            this.SetDB(inpatientManager);

            return inpatientManager.QueryMedItemListsByInpatientNO(inpatientNO, beginTime, endTime, deptCode);

        }

        /// <summary>
        /// ��û��߷�ҩƷ��Ϣ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="beginTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="deptCode">���ұ���</param>
        /// <returns></returns>
        public ArrayList QueryFeeItemListsByInpatientNO(string inpatientNO, DateTime beginTime, DateTime endTime, string deptCode)
        {
            this.SetDB(inpatientManager);

            return inpatientManager.QueryFeeItemListsByInpatientNO(inpatientNO, beginTime, endTime, deptCode);
        }

        public ArrayList GetMedItemsForInpatient(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return inpatientManager.GetMedItemsForInpatient(inpatientNO, beginTime, endTime);
        }

        public ArrayList QueryFeeItemLists(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            return inpatientManager.QueryFeeItemLists(inpatientNO, beginTime, endTime);
        }

        /// <summary>
        /// ����ҩƷ�ͷ�ҩƷ��ϸ������¼---ͨ������{5C2A9C83-D165-434c-ACA4-86F23E956442}
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="isPharmacy">�Ƿ�ҩƷ Drug(true)�� UnDrug(false)��ҩƷ</param>
        /// <returns>�ɹ�: ҩƷ�ͷ�ҩƷ��ϸ������¼ ʧ��: null</returns>
        public Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList GetItemListByRecipeNO(string recipeNO, int recipeSequence, EnumItemType isPharmacy)
        {
            this.SetDB(inpatientManager);
            return inpatientManager.GetItemListByRecipeNO(recipeNO, recipeSequence, isPharmacy);
        }

        #region ��ҩƷ��Ŀ��Ϣ
        /// <summary>
        /// ������
        /// �������
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Fee.Item.Undrug GetItem(string itemCode)
        {
            this.SetDB(itemManager);
            return itemManager.GetValidItemByUndrugCode(itemCode);
        }

        //{8F86BB0D-9BB4-4c63-965D-969F1FD6D6B2} ȡ����ҩƷ������
        /// <summary>
        /// ȡ����ҩƷ������
        /// </summary>
        /// <param name="itemCode">��ҩƷ�����ʱ���</param>
        /// <param name="price">���ۡ���ҩƷ�ɴ���0</param>
        /// <returns>��ҩƷ������ʵ��</returns>
        public Neusoft.HISFC.Models.Base.Item GetUndrugAndMatItem(string itemCode, decimal price)
        {
            this.SetDB(itemManager);
            if (itemCode.StartsWith("F"))
            {
                return itemManager.GetValidItemByUndrugCode(itemCode);
            }
            else
            {
                Neusoft.HISFC.Models.FeeStuff.MaterialItem matItem = materialManager.GetMetItem(itemCode);
                if (matItem == null)
                {
                    return null;
                }
                matItem.ItemType = EnumItemType.MatItem;
                matItem.Price = price;
                (matItem as Neusoft.HISFC.Models.Base.Item).Specs = matItem.Specs;
                matItem.SysClass.ID = "U";
                return matItem;
            }
        }

        /// <summary>
        /// ��Ŀ�Ƿ�ʹ�ù�
        /// </summary>
        /// <param name="itemCode">��ĿID</param>
        /// <returns>true:ʹ��</returns>
        public bool IsUsed(string itemCode)
        {
            this.SetDB(itemManager);
            return itemManager.IsUsed(itemCode);
        }

        /// <summary>
        /// ɾ����ҩƷ��Ϣ
        /// </summary>
        /// <param name="undrugCode">��ҩƷ����</param>
        /// <returns>�ɹ� 1 ʧ�� -1 δɾ�������� 0</returns>
        public int DeleteUndrugItemByCode(string undrugID)
        {
            this.SetDB(itemManager);
            return itemManager.DeleteUndrugItemByCode(undrugID);
        }

        /// <summary>
        /// ������п��ܵ���Ŀ��Ϣ
        /// </summary>
        /// <returns>�ɹ� ��Ч�Ŀ�����Ŀ��Ϣ, ʧ�� null</returns>
        public ArrayList QueryValidItems()
        {
            this.SetDB(itemManager);
            return itemManager.QueryValidItems();
        }

        /// <summary>
        /// ���������Ŀ��Ϣ
        /// </summary>
        /// <returns>�ɹ� ������Ŀ��Ϣ, ʧ�� null</returns>
        public List<Neusoft.HISFC.Models.Fee.Item.Undrug> QueryAllItemsList()
        {
            this.SetDB(itemManager);
            return itemManager.QueryAllItemList();
        }

        #endregion

        #region ��ҩƷ����

        /// <summary>
        ///  �����ҩƷ�����Ŀ
        /// </summary>
        /// <param name="undrugComb">��ҩƷ�����Ŀʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�в������� 0</returns>
        [Obsolete("����,������Ŀ�ѹ鲢����ҩƷ", true)]
        public int InsertUndrugComb(Neusoft.HISFC.Models.Fee.Item.UndrugComb undrugComb)
        {
            return -1;
            //this.SetDB(undrugCombManager);
            //return undrugCombManager.InsertUndrugComb(undrugComb);
        }

        /// <summary>
        /// ���� ��ҩƷ�����е�����
        /// </summary>
        /// <param name="undrugComb">��ҩƷ�����Ŀʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и��µ����� 0</returns>
        [Obsolete("����,������Ŀ�ѹ鲢����ҩƷ", true)]
        public int UpdateUndrugComb(Neusoft.HISFC.Models.Fee.Item.UndrugComb undrugComb)
        {
            return -1;
            //this.SetDB(undrugCombManager);

            //return undrugCombManager.UpdateUndrugComb(undrugComb);
        }

        /// <summary>
        ///  ɾ����ҩƷ�����Ŀ
        /// </summary>
        /// <param name="undrugComb">��ҩƷ�����Ŀʵ��</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û��ɾ�������� 0</returns>
        [Obsolete("����,������Ŀ�ѹ鲢����ҩƷ", true)]
        public int DeleteUndrugComb(Neusoft.HISFC.Models.Fee.Item.UndrugComb undrugComb)
        {
            return -1;
            //this.SetDB(undrugCombManager);

            //return undrugCombManager.DeleteUndrugComb(undrugComb);
        }

        /// <summary>
        /// ͨ�������Ŀ�����ȡһ�������Ŀ
        /// </summary>
        /// <param name="undrugCombCode">�����Ŀ����</param>
        /// <returns>�ɹ�: һ�������Ŀ ʧ��: null</returns>
        [Obsolete("����,������Ŀ�ѹ鲢����ҩƷ", true)]
        public Neusoft.HISFC.Models.Fee.Item.UndrugComb GetUndrugCombByCode(string undrugCombCode)
        {
            Neusoft.HISFC.Models.Fee.Item.UndrugComb com = new Neusoft.HISFC.Models.Fee.Item.UndrugComb();
            return com;
            //this.SetDB(undrugCombManager);

            //return undrugCombManager.GetUndrugCombByCode(undrugCombCode);
        }

        /// <summary>
        /// ͨ�������Ŀ�����ȡһ����Ч�����Ŀ
        /// </summary>
        /// <param name="undrugCombCode">�����Ŀ����</param>
        /// <returns>�ɹ�: һ����Ч�����Ŀ ʧ��: null</returns>
        [Obsolete("����,������Ŀ�ѹ鲢����ҩƷ", true)]
        public Neusoft.HISFC.Models.Fee.Item.UndrugComb GetUndrugCombValidByCode(string undrugCombCode)
        {
            Neusoft.HISFC.Models.Fee.Item.UndrugComb com = new Neusoft.HISFC.Models.Fee.Item.UndrugComb();
            return com;
            //this.SetDB(undrugCombManager);

            //return undrugCombManager.GetUndrugCombValidByCode(undrugCombCode);
        }
        /// <summary>
        /// ��ȡ������Ŀ���ܼ۸�
        /// </summary>
        /// <param name="undrugCombCode">������Ŀ����</param>
        /// <returns></returns>
        public decimal GetUndrugCombPrice(string undrugCombCode)
        {
            this.SetDB(undrugPackAgeMgr);

            return undrugPackAgeMgr.GetUndrugCombPrice(undrugCombCode);
        }

        #endregion

        /// <summary>
        /// ���·�ҩƷ��ϸ��������
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="qty">��������</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateNoBackQtyForUndrug(string recipeNO, int recipeSequence, decimal qty, string balanceState)
        {
            this.SetDB(inpatientManager);
            return inpatientManager.UpdateNoBackQtyForUndrug(recipeNO, recipeSequence, qty, balanceState);
        }

        /// <summary>
        /// ���·�ҩƷ��ϸ��չ���
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="recipeSequence">��������ˮ��</param>
        /// <param name="extFlag2">��չ���2</param>
        /// <param name="balanceState">����״̬</param>
        /// <returns>�ɹ�: 1 ʧ�� : -1 û�и�������: 0</returns>
        public int UpdateExtFlagForUndrug(string recipeNO, int recipeSequence, string extFlag2, string balanceState)
        {
            this.SetDB(inpatientManager);
            return inpatientManager.UpdateExtFlagForUndrug(recipeNO, recipeSequence, extFlag2, balanceState);
        }

        /// <summary>
        /// ��û��ߺ�ִ�п����Ѿ�ȷ�ϵķ�ҩƷ�շ���ϸ
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <param name="execDeptCode">���Ҵ���</param>
        /// <returns>�ɹ�:���߷�ҩƷ��Ϣ ʧ��:null û���ҵ���¼ ArrayList.Count = 0</returns>
        public ArrayList QueryExeFeeItemListsByInpatientNOAndDept(string inpatientNO, string execDeptCode)
        {
            this.SetDB(inpatientManager);
            return inpatientManager.QueryExeFeeItemListsByInpatientNOAndDept(inpatientNO, execDeptCode);
        }

        #endregion

        #region ����

        #region ����

        /// <summary>
        /// ���ָ�����Ʋ���
        /// </summary>
        /// <param name="controlID">������ID</param>
        /// <param name="defaultValue">Ĭ��ֵ��û���ҵ����ش�ֵ</param>
        /// <returns>���Ʋ���</returns>
        public string GetControlValue(string controlID, string defaultValue)
        {
            string tempValue = string.Empty;

            if (controlerHelper == null || controlerHelper.ArrayObject == null || controlerHelper.ArrayObject.Count <= 0)
            {
                tempValue = controlManager.QueryControlerInfo(controlID);
            }
            else
            {
                NeuObject obj = controlerHelper.GetObjectFromID(controlID);

                if (obj == null)
                {
                    tempValue = controlManager.QueryControlerInfo(controlID);
                }
                else
                {
                    tempValue = ((Neusoft.HISFC.Models.Base.Controler)obj).ControlerValue;
                }
            }

            if (tempValue == null || tempValue == string.Empty)
            {
                return defaultValue;
            }
            else
            {
                return tempValue;
            }
        }

        #endregion

        #region �����շѺ���

        /// <summary>
        /// ��õ�ǰ�ӿڲ��
        /// </summary>
        /// <typeparam name="T">�ӿ�����</typeparam>
        /// <param name="controlCode">��������������</param>
        /// <param name="defalutInstance">Ĭ�ϲ��</param>
        /// <returns>�ɹ�T����ʵ�� ���� ����Ĭ��ʵ��</returns>
        public T GetPlugIns<T>(string controlCode, T defalutInstance)
        {
            string controlValue = controlParamIntegrate.GetControlParam<string>(controlCode, true, string.Empty);

            if (controlValue == string.Empty)
            {
                return defalutInstance;
            }

            string dllName = string.Empty;
            string namesSpaceAndUcName = string.Empty;

            try
            {
                dllName = controlValue.Split('|')[0];
                namesSpaceAndUcName = controlValue.Split('|')[1];

                Assembly assemblyName = System.Reflection.Assembly.LoadFrom(Application.StartupPath + dllName);

                System.Runtime.Remoting.ObjectHandle objPlugin = null;

                objPlugin = System.Activator.CreateInstance(assemblyName.ToString(), namesSpaceAndUcName);

                if (objPlugin == null)
                {
                    MessageBox.Show("����ʧ��!��ȷ����ѡ���dll��uc����ȷ������! ������Ĭ�ϲ��");

                    return defalutInstance;
                }

                object obj = objPlugin.Unwrap();

                defalutInstance = default(T);

                return (T)obj;
            }
            catch (Exception e)
            {
                MessageBox.Show("��ǰ�������ά������! ������Ĭ�ϲ��" + e.Message);

                return defalutInstance;
            }
        }

        /// <summary>
        /// ��û��ߵ�δ�շ���Ŀ��Ϣ
        /// </summary>
        /// <param name="clinicNO">�Һ���ˮ��</param>
        /// <returns>�ɹ�:������ϸ ʧ��:null û������:����Ԫ����Ϊ0��ArrayList</returns>
        public System.Collections.ArrayList QueryChargedFeeItemListsByClinicNO(string clinicNO)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.QueryChargedFeeItemListsByClinicNO(clinicNO);
        }

        /// <summary>
        /// ��û��ߵ����շ���Ŀ��Ϣ
        /// </summary>
        /// <param name="clinicNO">�Һ���ˮ��</param>
        /// <returns>�ɹ�:������ϸ ʧ��:null û������:����Ԫ����Ϊ0��ArrayList</returns>
        public System.Collections.ArrayList QueryFeeItemListsByClinicNO(string clinicNO)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.QueryFeeItemListsByClinicNO(clinicNO);
        }

        /// <summary>
        /// �����Ŀ�۸�
        /// priceObj.ID ���� ��ͬ��λ�ü۸���ʽ����
        /// priceObj.Name ���滼�ߵ�����
        /// priceObj.Memo ������Ϣ
        /// priceObj.User01 ���׼�
        /// priceObj.User02 ����۸�
        /// priceObj.User03 ��ͯ�۸�
        /// </summary>
        /// <param name="priceObj"></param>
        /// <returns>-1 ʧ�� ����:Ӧ��ʹ�õü۸�</returns>
        public decimal GetPrice(NeuObject priceObj)
        {
            decimal unitPrice = 0;
            decimal spPrice = 0;
            decimal chindPrice = 0;
            int age = 0;
            try
            {
                unitPrice = NConvert.ToDecimal(priceObj.User01);
            }
            catch (Exception ex)
            {
                priceObj.Memo = "���׼�ת������" + ex.Message;

                return -1;
            }
            try
            {
                spPrice = NConvert.ToDecimal(priceObj.User02);
            }
            catch (Exception ex)
            {
                priceObj.Memo = "�����ת������" + ex.Message;

                return -1;
            }
            try
            {
                chindPrice = NConvert.ToDecimal(priceObj.User03);
            }
            catch (Exception ex)
            {
                priceObj.Memo = "��ͯ��ת������" + ex.Message;

                return -1;
            }
            try
            {
                age = NConvert.ToInt32(priceObj.Name);
            }
            catch (Exception ex)
            {
                priceObj.Memo = "����ת������" + ex.Message;

                return -1;
            }
            if (priceObj.ID == "�����")
            {
                return spPrice;
            }
            //��ע�͵�{7BFE3521-F843-4789-AC85-DB16F3C428D6} wbo 2011-02-10
            //else if (age <= 14)
            //{
            //    return chindPrice;
            //}
            if (priceObj.ID == "���׼�")//����
            {
                return unitPrice;
            }
            else if (priceObj.ID == "��ͯ��")//��ͯ
            {
                return chindPrice;
            }
            else
            {
                return unitPrice;
            }
        }

        /// <summary>
        /// ���Ʋ���������
        /// </summary>
        public static Neusoft.FrameWork.Public.ObjectHelper controlerHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ת�����Ұ�����
        /// </summary>
        private static Neusoft.FrameWork.Public.ObjectHelper hsInvertDept = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// �ִ����ź������
        /// </summary>
        private static bool isDecSysClassWhenGetRecipeNO = false;

        /// <summary>
        /// ÿ�������ɷ�Ϊ��
        /// </summary>
        public static bool isDoseOnceCanNull = false;

        /// <summary>
        /// ����СƱ�Ŀ���
        /// </summary>
        private static Neusoft.FrameWork.Public.ObjectHelper printRecipeHeler = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ���[������ˮ��]�ʹ�����
        /// </summary>
        /// <param name="feeItemLists">������ϸ</param>
        /// <param name="recipeNO">������</param>
        /// <param name="sequence">������ˮ��</param>
        public void GetRecipeNoAndMaxSeq(ArrayList feeItemLists, ref string recipeNO, ref int sequence)
        {
            if (feeItemLists == null || feeItemLists.Count <= 0)
            {
                return;
            }

            foreach (FeeItemList feeItem in feeItemLists)
            {
                if (feeItem.RecipeNO != null && feeItem.RecipeNO.Length > 0)
                {
                    recipeNO = feeItem.RecipeNO;

                    sequence = NConvert.ToInt32(outpatientManager.GetMaxSeqByRecipeNO(recipeNO));

                    break;
                }
            }
        }

        /// <summary>
        /// ����շ���Ŀ�б��� ϵͳ���ִ�п��ң����� ���ƴ�����
        /// ͬһϵͳ���ͳһִ�п��ң�ͬһ��������Ŀ��������ͬ
        /// ���Ѿ�����ô����ŵ���Ŀ���������·���
        /// </summary>
        /// <param name="feeDetails">������Ϣ</param>
        /// <param name="t">���ݿ�Trans</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>falseʧ�� true�ɹ�</returns>
        public bool SetRecipeNOOutpatient(Register r,ArrayList feeDetails, ref string errText)
        {
            Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitRecipe iSplitRecipe = null;
            //iSplitRecipe = new InterfaceInstanceDefault.ISplitRecipe.ISplitRecipeDefault();
            iSplitRecipe = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitRecipe)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitRecipe;
            if (iSplitRecipe != null)
            {
                //������Ӧ֢��Ϣ
                return iSplitRecipe.SplitRecipe(r, feeDetails,ref errText);
                //if (returnValue < 0)
                //{
                //    return false;
                //}

            }
            else
            {
                #region Ĭ�ϵ�ʵ��
                bool isDealCombNO = false; //�Ƿ����ȴ�����Ϻ�
                int noteCounts = 0;        //��õ��Ŵ���������Ŀ��

                //�Ƿ����ȴ�����Ϻ�
                isDealCombNO = controlParamIntegrate.GetControlParam<bool>(Const.DEALCOMBNO, false, true);

                //��õ��Ŵ���������Ŀ��, Ĭ����Ŀ�� 5
                noteCounts = controlParamIntegrate.GetControlParam<int>(Const.NOTECOUNTS, false, 5);

                //�Ƿ����ϵͳ���
                isDecSysClassWhenGetRecipeNO = controlParamIntegrate.GetControlParam<bool>(Const.DEC_SYS_WHENGETRECIPE, false, false);

                //�Ƿ����ȴ����ݴ��¼
                bool isDecTempSaveWhenGetRecipeNO = controlParamIntegrate.GetControlParam<bool>(Const.���������ȿ��Ƿַ���¼, false, false);

                ArrayList sortList = new ArrayList();
                while (feeDetails.Count > 0)
                {
                    ArrayList sameNotes = new ArrayList();
                    FeeItemList compareItem = feeDetails[0] as FeeItemList;
                    foreach (FeeItemList f in feeDetails)
                    {
                        if (isDecSysClassWhenGetRecipeNO)
                        {
                            if (f.ExecOper.Dept.ID == compareItem.ExecOper.Dept.ID
                                && f.Days == compareItem.Days && (isDecTempSaveWhenGetRecipeNO ? f.RecipeSequence == compareItem.RecipeSequence : true))
                            {
                                sameNotes.Add(f);
                            }
                        }
                        else
                        {
                            if (f.Item.SysClass.ID.ToString() == compareItem.Item.SysClass.ID.ToString()
                                && f.ExecOper.Dept.ID == compareItem.ExecOper.Dept.ID
                                && f.Days == compareItem.Days && (isDecTempSaveWhenGetRecipeNO ? f.RecipeSequence == compareItem.RecipeSequence : true))
                            {
                                sameNotes.Add(f);
                            }
                        }

                    }
                    sortList.Add(sameNotes);
                    foreach (FeeItemList f in sameNotes)
                    {
                        feeDetails.Remove(f);
                    }
                }

                foreach (ArrayList temp in sortList)
                {
                    ArrayList combAll = new ArrayList();
                    ArrayList noCombAll = new ArrayList();
                    ArrayList noCombUnits = new ArrayList();
                    ArrayList noCombFinal = new ArrayList();


                    if (isDealCombNO)//���ȴ�����Ϻţ������е���Ϻ������·���
                    {
                        //��ѡ��û����Ϻŵ���Ŀ
                        foreach (FeeItemList f in temp)
                        {
                            if (f.Order.Combo.ID == null || f.Order.Combo.ID == string.Empty)
                            {
                                noCombAll.Add(f);
                            }
                        }
                        //������������ɾ��û����Ϻŵ���Ŀ
                        foreach (FeeItemList f in noCombAll)
                        {
                            temp.Remove(f);
                        }
                        //���ͬһ���������Ŀ�������·���
                        while (noCombAll.Count > 0)
                        {
                            noCombUnits = new ArrayList();
                            foreach (FeeItemList f in noCombAll)
                            {
                                if (noCombUnits.Count < noteCounts)
                                {
                                    noCombUnits.Add(f);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            noCombFinal.Add(noCombUnits);
                            foreach (FeeItemList f in noCombUnits)
                            {
                                noCombAll.Remove(f);
                            }
                        }
                        //���ʣ�����Ŀ��Ŀ> 0˵��������ϵ���Ŀ
                        if (temp.Count > 0)
                        {
                            while (temp.Count > 0)
                            {
                                ArrayList combNotes = new ArrayList();
                                FeeItemList compareItem = temp[0] as FeeItemList;
                                foreach (FeeItemList f in temp)
                                {
                                    if (f.Order.Combo.ID == compareItem.Order.Combo.ID)
                                    {
                                        combNotes.Add(f);
                                    }
                                }
                                combAll.Add(combNotes);
                                foreach (FeeItemList f in combNotes)
                                {
                                    temp.Remove(f);
                                }
                            }
                        }
                        foreach (ArrayList tempNoComb in noCombFinal)
                        {
                            string recipeNo = null;//������ˮ��
                            int noteSeq = 1;//��������Ŀ��ˮ��

                            string tempRecipeNO = string.Empty;
                            int tempSequence = 0;
                            this.GetRecipeNoAndMaxSeq(tempNoComb, ref tempRecipeNO, ref tempSequence);

                            if (tempRecipeNO != string.Empty && tempSequence > 0)
                            {
                                tempSequence += 1;
                                foreach (FeeItemList f in tempNoComb)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = tempRecipeNO;
                                        f.SequenceNO = tempSequence;
                                        tempSequence++;
                                    }
                                }
                            }
                            else
                            {
                                recipeNo = outpatientManager.GetRecipeNO();
                                if (recipeNo == null || recipeNo == string.Empty)
                                {
                                    errText = "��ô����ų���!";
                                    return false;
                                }
                                foreach (FeeItemList f in tempNoComb)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = recipeNo;
                                        f.SequenceNO = noteSeq;
                                        noteSeq++;
                                    }
                                }
                            }
                        }
                        foreach (ArrayList tempComb in combAll)
                        {
                            string recipeNo = null;//������ˮ��
                            int noteSeq = 1;//��������Ŀ��ˮ��

                            string tempRecipeNO = string.Empty;
                            int tempSequence = 0;
                            this.GetRecipeNoAndMaxSeq(tempComb, ref tempRecipeNO, ref tempSequence);

                            if (tempRecipeNO != string.Empty && tempSequence > 0)
                            {
                                tempSequence += 1;
                                foreach (FeeItemList f in tempComb)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = tempRecipeNO;
                                        f.SequenceNO = tempSequence;
                                        tempSequence++;
                                    }
                                }
                            }
                            else
                            {
                                recipeNo = outpatientManager.GetRecipeNO();
                                if (recipeNo == null || recipeNo == string.Empty)
                                {
                                    errText = "��ô����ų���!";
                                    return false;
                                }
                                foreach (FeeItemList f in tempComb)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = recipeNo;
                                        f.SequenceNO = noteSeq;
                                        noteSeq++;
                                    }
                                }
                            }
                        }
                    }
                    else //�����ȴ�����Ϻ�
                    {
                        ArrayList counts = new ArrayList();
                        ArrayList countUnits = new ArrayList();
                        while (temp.Count > 0)
                        {
                            countUnits = new ArrayList();
                            foreach (FeeItemList f in temp)
                            {
                                if (countUnits.Count < noteCounts)
                                {
                                    countUnits.Add(f);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            counts.Add(countUnits);
                            foreach (FeeItemList f in countUnits)
                            {
                                temp.Remove(f);
                            }
                        }

                        //{B24B174D-F261-4c6b-94C9-EEED0F736013}
                        Hashtable hs = new Hashtable();


                        foreach (ArrayList tempCounts in counts)
                        {
                            string recipeNO = null;//������ˮ��
                            int recipeSequence = 1;//��������Ŀ��ˮ��

                            string tempRecipeNO = string.Empty;
                            int tempSequence = 0;
                            this.GetRecipeNoAndMaxSeq(tempCounts, ref tempRecipeNO, ref tempSequence);
                            //{B24B174D-F261-4c6b-94C9-EEED0F736013}
                            if (hs.Contains(tempRecipeNO))
                            {
                                tempSequence = Neusoft.FrameWork.Function.NConvert.ToInt32((hs[tempRecipeNO] as Neusoft.FrameWork.Models.NeuObject).Name);
                            }
                            else
                            {
                                Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();
                                obj.ID = tempRecipeNO;
                                obj.Name = tempSequence.ToString();
                                hs.Add(tempRecipeNO, obj);
                            }

                            if (tempRecipeNO != string.Empty && tempSequence > 0)
                            {
                                tempSequence += 1;
                                foreach (FeeItemList f in tempCounts)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = tempRecipeNO;
                                        f.SequenceNO = tempSequence;
                                        tempSequence++;
                                    }
                                }
                                //{B24B174D-F261-4c6b-94C9-EEED0F736013}
                                if (hs.Contains(tempRecipeNO))
                                {
                                    (hs[tempRecipeNO] as Neusoft.FrameWork.Models.NeuObject).Name = tempSequence.ToString();
                                }
                            }
                            else
                            {
                                recipeNO = outpatientManager.GetRecipeNO();
                                if (recipeNO == null || recipeNO == string.Empty)
                                {
                                    errText = "��ô����ų���!";
                                    return false;
                                }
                                foreach (FeeItemList f in tempCounts)
                                {
                                    feeDetails.Add(f);
                                    if (f.RecipeNO != null && f.RecipeNO != string.Empty)//�Ѿ����䴦����
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        f.RecipeNO = recipeNO;
                                        f.SequenceNO = recipeSequence;
                                        recipeSequence++;
                                    }
                                }//{B24B174D-F261-4c6b-94C9-EEED0F736013}
                                if (!hs.Contains(tempRecipeNO))
                                {
                                    Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();
                                    obj.ID = recipeNO;
                                    obj.Name = recipeSequence.ToString();
                                    hs.Add(recipeNO, obj);
                                }
                            }


                        }
                    }
                }
                #endregion
            }
            return true;
        }


        /// <summary>
        /// ������ϸ����У��
        /// </summary>
        /// <param name="f">����ʵ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>�ɹ� true ʧ�� false</returns>
        public bool IsFeeItemListDataValid(FeeItemList f, ref string errText)
        {
            string itemName = f.Item.Name;
            if (f == null)
            {
                errText = itemName + "��÷���ʵ�����!";

                return false;
            }
            if (f.Item.ID == null || f.Item.ID == string.Empty)
            {
                errText = itemName + "��Ŀ����û�и�ֵ";

                return false;
            }
            if (f.Item.Name == null || f.Item.Name == string.Empty)
            {
                errText = itemName + "��Ŀ����û�и�ֵ";

                return false;
            }
            //if (f.Item.IsPharmacy)
            if (f.Item.ItemType == EnumItemType.Drug)
            {
                if (f.Item.Specs == null || f.Item.Specs == string.Empty)
                {
                    errText = itemName + "ҩƷ�Ĺ��û�и�ֵ";

                    return false;
                }
                #region ���ݲ���&& !isDoseOnceCanNull ���ж��Ƿ���Ҫ�������ֵ ����ǿ20070828
                if ((f.Order.Frequency.ID == null || f.Order.Frequency.ID == string.Empty) && !isDoseOnceCanNull)
                {
                    errText = itemName + "Ƶ�δ���û�и�ֵ";

                    return false;
                }
                if ((f.Order.Usage.ID == null || f.Order.Usage.ID == string.Empty) && !isDoseOnceCanNull)
                {
                    errText = itemName + "�÷�����û�и�ֵ";

                    return false;
                }
                if (f.Order.DoseOnce == 0 && !isDoseOnceCanNull)
                {
                    errText = itemName + "ÿ������û�и�ֵ";

                    return false;
                }
                if ((f.Order.DoseUnit == null || f.Order.DoseUnit == string.Empty) && !isDoseOnceCanNull)
                {
                    errText = itemName + "ÿ��������λû�и�ֵ";

                    return false;
                }
                #endregion
            }
            if (f.Item.PackQty == 0)
            {
                errText = itemName + "��װ����û�и�ֵ";

                return false;
            }
            if (f.Item.PriceUnit == null || f.Item.PriceUnit == string.Empty)
            {
                errText = itemName + "�Ƽ۵�λû�и�ֵ";

                return false;
            }


            if (f.Item.MinFee.ID == null || f.Item.MinFee.ID == string.Empty)
            {
                errText = itemName + "��С����û�и�ֵ";

                return false;
            }
            if (f.Item.SysClass.ID == null || f.Item.SysClass.Name == string.Empty)
            {
                errText = itemName + "ϵͳ���û�и�ֵ";

                return false;
            }
            if (f.Item.Price == 0)
            {
                errText = itemName + "����û�и�ֵ";

                return false;
            }
            if (f.Item.Price < 0)
            {
                errText = itemName + "���۲���С��0";

                return false;
            }
            if (f.Item.Qty == 0)
            {
                errText = itemName + "����û�и�ֵ";

                return false;
            }
            if (f.Item.Qty < 0)
            {
                errText = itemName + "��������С��0";

                return false;
            }

            if (f.Item.Qty > 99999)
            {
                errText = itemName + "�������ܴ���99999";

                return false;
            }

            if (f.Days == 0)
            {
                errText = itemName + "��ҩ����û�и�ֵ";

                return false;
            }
            if (f.Days < 0)
            {
                errText = itemName + "��ҩ��������С��0";

                return false;
            }
            if (f.FT.OwnCost + f.FT.PayCost + f.FT.PubCost == 0)
            {
                errText = itemName + "��Ŀ���û�и�ֵ";

                return false;
            }
            if (f.FT.OwnCost + f.FT.PayCost + f.FT.PubCost < 0)
            {
                errText = itemName + "��Ŀ���Ϊ��";

                return false;
            }
            ////{8DF48FD8-14E9-464a-A368-256B19A0EE54} �޸��ֻ����
            //if (Neusoft.FrameWork.Public.String.FormatNumber(f.Item.Price * f.Item.Qty / f.Item.PackQty, 2) != Neusoft.FrameWork.Public.String.FormatNumber
            //    (f.FT.OwnCost + f.FT.PayCost + f.FT.PubCost /*+ f.FT.RebateCost*/, 2))
            //{
            //    errText = itemName + "����뵥����������";

            //    return false;
            //}
            if (f.ExecOper.Dept.ID == null || f.ExecOper.Dept.ID == string.Empty)
            {
                errText = itemName + "ִ�п��Ҵ���û�и�ֵ";

                return false;
            }
            if (f.ExecOper.Dept.Name == null || f.ExecOper.Dept.Name == string.Empty)
            {
                errText = itemName + "ִ�п�������û�и�ֵ";

                return false;
            }

            return true;
        }

        #region ɾ�������������ܻ�����Ϣ
        /// <summary>
        /// ���������ˮ�źͷ�Ʊ��Ϻ�ɾ����������Ϣ��
        /// </summary>
        /// <param name="ClinicNO">�����ˮ��</param>
        /// <param name="RecipeNO">��Ʊ��Ϻ�</param>
        /// <returns></returns>
        public int DeleteFeeItemListByClinicNOAndRecipeNO(string ClinicNO, string RecipeNO)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.DeleteFeeItemListByClinicNOAndRecipeNO(ClinicNO, RecipeNO);
        }
        #endregion

        /// <summary>
        /// ��÷�Ʊ��
        /// </summary>
        /// <param name="oper">��Ա������Ϣ</param>
        /// <param name="ctrl">���Ʋ�����</param>
        /// <param name="invoiceNO">��Ʊ���Ժ�</param>
        /// <param name="realInvoiceNO">ʵ�ʷ�Ʊ��</param>
        /// <param name="t">���ݿ�����</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>-1 ʧ�� 1 �ɹ�!</returns>
        public int GetInvoiceNO(Neusoft.HISFC.Models.Base.Employee oper, ref string invoiceNO, ref string realInvoiceNO, ref string errText, Neusoft.FrameWork.Management.Transaction trans)
        {
            string invoiceType = controlParamIntegrate.GetControlParam<string>(Const.GET_INVOICE_NO_TYPE, false, "0");

            NeuObject objInvoice = new NeuObject();

            switch (invoiceType)
            {
                case "2"://��ͨģʽ

                    objInvoice = managerIntegrate.GetConstansObj("MZINVOICE", oper.ID);

                    //û��ά����Ʊ��ʼ��
                    if (objInvoice == null || objInvoice.ID == null || objInvoice.ID == string.Empty)
                    {
                        if (Neusoft.FrameWork.Management.PublicTrans.Trans == null)
                        {
                            //trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                            //trans.BeginTransaction();
                            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        }
                        managerIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                        Neusoft.HISFC.Models.Base.Const con = new Neusoft.HISFC.Models.Base.Const();
                        con.ID = oper.ID;
                        con.Name = "1";//Ĭ�ϴ�1��ʼ
                        con.Memo = "1";//Ĭ�ϴ�1��ʼ
                        con.IsValid = true;
                        con.OperEnvironment.ID = oper.ID;
                        con.OperEnvironment.OperTime = inpatientManager.GetDateTimeFromSysDateTime();

                        int iReturn = managerIntegrate.InsertConstant("MZINVOICE", con);
                        if (iReturn <= 0)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            errText = "�������Ա���Է�Ʊʧ��!" + managerIntegrate.Err;

                            return -1;
                        }
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                        //invoiceNO = objInvoice.Name;
                        //realInvoiceNO = objInvoice.Name;
                        //string invoiceNOTemp = this.GetNewInvoiceNO(Neusoft.HISFC.Models.Fee.EnumInvoiceType.C);
                        //{BCB3B25A-69CD-4dfe-84D2-21D2239A7467}
                        if (Neusoft.FrameWork.Management.PublicTrans.Trans == null) 
                        {
                            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                            this.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        }

                        string invoiceNOTemp = this.GetNewInvoiceNO("C");
                        //{BCB3B25A-69CD-4dfe-84D2-21D2239A7467}
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();

                        if (invoiceNOTemp == null || invoiceNOTemp == string.Empty)
                        {
                            errText = "��÷�Ʊʧ��!" + this.Err;

                            return -1;
                        }

                        invoiceNO = invoiceNOTemp;
                        realInvoiceNO = objInvoice.Name;
                    }
                    else
                    {
                        //string invoiceNOTemp = this.GetNewInvoiceNO(Neusoft.HISFC.Models.Fee.EnumInvoiceType.C);
                        string invoiceNOTemp = this.GetNewInvoiceNO("C");
                        if (invoiceNOTemp == null || invoiceNOTemp == string.Empty)
                        {
                            errText = "��÷�Ʊʧ��!" + this.Err;

                            return -1;
                        }

                        invoiceNO = invoiceNOTemp;
                        realInvoiceNO = objInvoice.Name;
                    }

                    break;

                case "0": //��ҽģʽ
                    objInvoice = managerIntegrate.GetConstansObj("MZINVOICE", oper.ID);

                    //û��ά����Ʊ��ʼ��
                    if (objInvoice == null || objInvoice.ID == null || objInvoice.ID == string.Empty)
                    {
                        //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                        //trans.BeginTransaction(); by niuxy

                        if (Neusoft.FrameWork.Management.PublicTrans.Trans == null)
                        {
                            //trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                            //trans.BeginTransaction();
                            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        }
                        managerIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                        Neusoft.HISFC.Models.Base.Const con = new Neusoft.HISFC.Models.Base.Const();
                        con.ID = oper.ID;
                        con.Name = "1";//Ĭ�ϴ�1��ʼ
                        con.Memo = "1";//Ĭ�ϴ�1��ʼ
                        con.IsValid = true;
                        con.OperEnvironment.ID = oper.ID;
                        con.OperEnvironment.OperTime = inpatientManager.GetDateTimeFromSysDateTime();

                        int iReturn = managerIntegrate.InsertConstant("MZINVOICE", con);
                        if (iReturn <= 0)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            errText = "�������Ա���Է�Ʊʧ��!" + managerIntegrate.Err;

                            return -1;
                        }
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                        invoiceNO = objInvoice.Name;
                        realInvoiceNO = objInvoice.Name;
                    }
                    else
                    {
                        invoiceNO = objInvoice.Name.PadLeft(12, '0');
                        realInvoiceNO = NConvert.ToInt32(objInvoice.Name).ToString();
                    }
                    break;
                case "1": //��ɽҽģʽ!

                    objInvoice = managerIntegrate.GetConstansObj("MZINVOICE", oper.ID);
                    if (objInvoice == null)
                    {
                        errText = "��÷�Ʊ��Ϣ����!" + managerIntegrate.Err;

                        return -1;
                    }

                    Employee empl = managerIntegrate.GetEmployeeInfo(oper.ID);
                    if (empl == null)
                    {
                        errText = "��ò���Ա������Ϣ����!" + managerIntegrate.Err;

                        return -1;
                    }

                    string tmpOperCode = empl.UserCode;
                    oper.UserCode = empl.UserCode;

                    if (oper == null || oper.UserCode == null || oper.UserCode == string.Empty || oper.UserCode.Length > 2)
                    {
                        tmpOperCode = "XX";
                    }
                    else
                    {
                        tmpOperCode = empl.UserCode;
                    }

                    //û��ά����Ʊ��ʼ��
                    if (objInvoice == null || objInvoice.ID == null || objInvoice.ID == string.Empty)
                    {
                        //Neusoft.FrameWork.Management.Transaction 
                        if (Neusoft.FrameWork.Management.PublicTrans.Trans == null)
                        {
                            //trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                            //trans.BeginTransaction();
                            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        }
                        managerIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        inpatientManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                        Neusoft.HISFC.Models.Base.Const con = new Neusoft.HISFC.Models.Base.Const();
                        con.ID = oper.ID;
                        con.Name = "1";//Ĭ�ϴ�1��ʼ
                        con.IsValid = true;
                        con.OperEnvironment.ID = oper.ID;
                        con.OperEnvironment.OperTime = inpatientManager.GetDateTimeFromSysDateTime();
                        con.Memo = con.OperEnvironment.OperTime.Year.ToString().Substring(2, 2) + con.OperEnvironment.OperTime.Month.ToString().PadLeft(2, '0') +
                            con.OperEnvironment.OperTime.Day.ToString().PadLeft(2, '0') + tmpOperCode + "0001";
                        int iReturn = managerIntegrate.InsertConstant("MZINVOICE", con);
                        if (iReturn <= 0)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            errText = "�������Ա���Է�Ʊʧ��!" + managerIntegrate.Err;
                            return -1;
                        }
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                        invoiceNO = con.Memo;
                    }
                    else
                    {
                        invoiceNO = objInvoice.Memo;
                        DateTime now = inpatientManager.GetDateTimeFromSysDateTime();
                        if (invoiceNO == null || invoiceNO == string.Empty)
                        {
                            invoiceNO = now.Year.ToString().Substring(2, 2) + now.Month.ToString().PadLeft(2, '0') +
                                now.Day.ToString().PadLeft(2, '0') + tmpOperCode + "0001";
                        }
                        try
                        {
                            DateTime dtInvoice = new DateTime(2000 + Neusoft.FrameWork.Function.NConvert.ToInt32(invoiceNO.Substring(0, 2)), Neusoft.FrameWork.Function.NConvert.ToInt32(invoiceNO.Substring(2, 2)), Neusoft.FrameWork.Function.NConvert.ToInt32(invoiceNO.Substring(4, 2)));
                            if (now.Date > dtInvoice)
                            {
                                invoiceNO = now.Year.ToString().Substring(2, 2) + now.Month.ToString().PadLeft(2, '0') +
                                    now.Day.ToString().PadLeft(2, '0') + tmpOperCode + "0001";
                            }
                        }
                        catch (Exception ex)
                        {
                            errText = "��Ʊת�����ڳ���!" + ex.Message;
                            return -1;
                        }

                        realInvoiceNO = objInvoice.Name;
                    }

                    break;
            }

            return 1;
        }

        /// <summary>
        /// �����һ�ŷ�Ʊ��
        /// </summary>
        /// <param name="invoiceType">��÷�Ʊ�ŷ�ʽ</param>
        /// <param name="invoiceNO">��ǰ���Է�Ʊ��</param>
        /// <param name="realInvoiceNO">��ǰʵ�ʷ�Ʊ��</param>
        /// <param name="nextInvoiceNO">��һ�ŵ��Է�Ʊ��</param>
        /// <param name="nextRealInvoiceNO">��һ��ʵ�ʷ�Ʊ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>-1 ���� 1 ��ȷ</returns>
        public int GetNextInvoiceNO(string invoiceType, string invoiceNO, string realInvoiceNO, ref string nextInvoiceNO, ref string nextRealInvoiceNO, ref string errText)
        {
            return GetNextInvoiceNO(invoiceType, invoiceNO, realInvoiceNO, ref nextInvoiceNO, ref nextRealInvoiceNO, 1, ref errText);
        }

        /// <summary>
        /// �����N�ŷ�Ʊ��
        /// </summary>
        /// <param name="invoiceType">��÷�Ʊ�ŷ�ʽ</param>
        /// <param name="invoiceNO">��ǰ���Է�Ʊ��</param>
        /// <param name="realInvoiceNO">��ǰʵ�ʷ�Ʊ��</param>
        /// <param name="nextInvoiceNO">��һ�ŵ��Է�Ʊ��</param>
        /// <param name="nextRealInvoiceNO">��һ��ʵ�ʷ�Ʊ��</param>
        /// <param name="count">�¼��ŷ�Ʊ</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>-1 ���� 1 ��ȷ</returns>
        public int GetNextInvoiceNO(string invoiceType, string invoiceNO, string realInvoiceNO, ref string nextInvoiceNO, ref string nextRealInvoiceNO, int count, ref string errText)
        {
            switch (invoiceType)
            {
                case "2"://��ͨģʽ

                    string invoiceNOTemp = string.Empty;

                    for (int i = 0; i < count; i++)
                    {
                        //invoiceNOTemp = this.GetNewInvoiceNO(Neusoft.HISFC.Models.Fee.EnumInvoiceType.C);
                        invoiceNOTemp = this.GetNewInvoiceNO("C");
                        if (invoiceNOTemp == null || invoiceNOTemp == string.Empty)
                        {
                            errText = "��÷�Ʊʧ��!";

                            return -1;
                        }
                    }

                    if (count == 0)
                    {
                        invoiceNOTemp = invoiceNO;
                    }

                    nextInvoiceNO = invoiceNOTemp;
                    nextRealInvoiceNO = nextRealInvoiceNO = (NConvert.ToInt32(realInvoiceNO) + count).ToString();

                    break;

                case "0"://��ҽ��ʽ
                    //��ҽ��ʽ�ķ�Ʊ��,Ϊ������,����ֱ������1����
                    nextInvoiceNO = ((NConvert.ToInt32(invoiceNO) + count).ToString()).PadLeft(12, '0');
                    //��ҽ��ʽ��ʵ�ʷ�Ʊ��,�����Ժ�һ��,ͬ������
                    nextRealInvoiceNO = NConvert.ToInt32(nextInvoiceNO).ToString();

                    break;
                case "1"://��ɽ��ʽ
                    //��Ϊ��ɽ��ʽ�ķ�Ʊ�����λ������Ʊ�����к�,����һ��Ҫ����4λ,���������λΪ���ֲ��ǺϷ���Ʊ
                    if (invoiceNO.Length < 4)
                    {
                        errText = "��Ʊ�ų��Ȳ�����!";

                        return -1;
                    }
                    //�����ɽ��Ʊ�ĳ���
                    int len = invoiceNO.Length;
                    //��÷�Ʊ���˺���λ���ַ���,����Ʊ�����ں��տ�Ա,��ʽΪyymmddxx(��,��,��,����Ա2λ����)
                    string orgInvoice = invoiceNO.Substring(0, len - 4);
                    //��ú���λ��Ʊ���к�
                    string addInvoice = invoiceNO.Substring(len - 4, 4);

                    //�����һ�ŷ�Ʊ��
                    nextInvoiceNO = orgInvoice + (NConvert.ToInt32(addInvoice) + count).ToString().PadLeft(4, '0');
                    //ʵ�ʷ�Ʊ��Ϊ����,ֱ������1����
                    nextRealInvoiceNO = (NConvert.ToInt32(realInvoiceNO) + count).ToString();

                    break;
            }

            return 1;
        }

        /// <summary>
        /// ��ѡ��ϵͳ��Ʊʱ��,�ش����,ֻ���·�Ʊ��ӡ��
        /// </summary>
        /// <param name="invoiceNO">��ǰ��Ʊ��</param>
        /// <param name="realInvoiceNO">��ǰ��Ʊ��ӡ��</param>
        /// <param name="errText">�������</param>
        /// <returns>�ɹ�1  ʧ�� -1</returns>
        public int UpdateOnlyRealInvoiceNO(string invoiceNO, string realInvoiceNO, ref string errText)
        {
            Neusoft.HISFC.Models.Base.Const con = new Neusoft.HISFC.Models.Base.Const();

            con.ID = outpatientManager.Operator.ID;
            realInvoiceNO = (NConvert.ToInt32(realInvoiceNO) + 1).ToString();
            con.Name = realInvoiceNO;
            con.Memo = invoiceNO;

            con.IsValid = true;
            con.OperEnvironment.ID = outpatientManager.Operator.ID;
            con.OperEnvironment.OperTime = outpatientManager.GetDateTimeFromSysDateTime();
            int returnValue = managerIntegrate.UpdateConstant("MZINVOICE", con);
            if (returnValue <= 0)
            {
                errText = "���²���Ա���Է�Ʊʧ��!" + managerIntegrate.Err;

                return -1;
            }

            return returnValue;
        }

        /// <summary>
        /// ��÷�Ʊ��
        /// </summary>
        /// <param name="invoiceNO">��Ʊ���Ժ�</param>
        /// <param name="realInvoiceNO">ʵ�ʷ�Ʊ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>-1 ʧ�� 1 �ɹ�!</returns>
        public int UpdateInvoiceNO(string invoiceNO, string realInvoiceNO, ref string errText)
        {
            string invoiceType = controlParamIntegrate.GetControlParam<string>(Const.GET_INVOICE_NO_TYPE, false, "0");

            int returnValue = 0;
            string nextInvoiceNO = string.Empty;
            string nextRealInvoiceNO = string.Empty;

            Neusoft.HISFC.Models.Base.Const con = new Neusoft.HISFC.Models.Base.Const();

            con.ID = outpatientManager.Operator.ID;
            returnValue = this.GetNextInvoiceNO(invoiceType, invoiceNO, realInvoiceNO, ref nextInvoiceNO, ref nextRealInvoiceNO, ref errText);
            if (returnValue < 0)
            {
                return -1;
            }

            if (invoiceType == "1")
            {
                con.Name = nextRealInvoiceNO;
                con.Memo = nextInvoiceNO;
            }
            else if (invoiceType == "2")
            {
                con.Name = nextRealInvoiceNO;
                con.Memo = nextInvoiceNO;
            }
            else
            {
                con.Name = nextInvoiceNO;
                con.Memo = nextRealInvoiceNO;
            }

            con.IsValid = true;
            con.OperEnvironment.ID = outpatientManager.Operator.ID;
            con.OperEnvironment.OperTime = outpatientManager.GetDateTimeFromSysDateTime();
            returnValue = managerIntegrate.UpdateConstant("MZINVOICE", con);
            if (returnValue <= 0)
            {
                errText = "���²���Ա���Է�Ʊʧ��!" + managerIntegrate.Err;

                return -1;
            }

            return returnValue;
        }
        /// <summary>
        /// ���·�Ʊ�����FIN_OPB_INVOICEINFO
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="balanceFlag"></param>
        /// <param name="balanceNo"></param>
        /// <param name="dtBalanceDate"></param>
        /// <returns></returns>
        public int UpdateInvoiceForDayBalance(System.DateTime dtBegin, System.DateTime dtEnd, string balanceFlag, string balanceNo, System.DateTime dtBalanceDate)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.UpdateInvoiceForDayBalance(dtBegin, dtEnd, balanceFlag, balanceNo, dtBalanceDate);
        }
        /// <summary>
        /// ���·�Ʊ��ϸ��FIN_OPB_INVOICEDETAIL
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="balanceFlag"></param>
        /// <param name="balanceNo"></param>
        /// <param name="dtBalanceDate"></param>
        /// <returns></returns>
        public int UpdateInvoiceDetailForDayBalance(System.DateTime dtBegin, System.DateTime dtEnd, string balanceFlag, string balanceNo, System.DateTime dtBalanceDate)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.UpdateInvoiceForDayBalance(dtBegin, dtEnd, balanceFlag, balanceNo, dtBalanceDate);
        }
        /// <summary>
        /// ����֧�������FIN_OPB_PAYMODE
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <param name="balanceFlag"></param>
        /// <param name="balanceNo"></param>
        /// <param name="dtBalanceDate"></param>
        /// <returns></returns>
        public int UpdatePayModeForDayBalance(System.DateTime dtBegin, System.DateTime dtEnd, string balanceFlag, string balanceNo, System.DateTime dtBalanceDate)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.UpdatePayModeForDayBalance(dtBegin, dtEnd, balanceFlag, balanceNo, dtBalanceDate);
        }
        public static string invoiceType = "0";//��Ʊ����

        /// <summary>
        /// �����������շ�����
        /// </summary>
        /// <param name="feeItemLists"></param>
        /// <returns></returns>
        private ArrayList GetRecipeSequenceForChk(ArrayList feeItemLists)
        {
            ArrayList list = new ArrayList();

            foreach (FeeItemList f in feeItemLists)
            {
                if (list.IndexOf(f.RecipeSequence) >= 0)
                {
                    continue;
                }
                else
                {
                    list.Add(f.RecipeSequence);
                }
            }

            return list;
        }

        /// <summary>
        /// ���Э������
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private ArrayList SplitNostrumDetail(Neusoft.HISFC.Models.Registration.Register rInfo, FeeItemList f,ref string errText)
        {
            List<Neusoft.HISFC.Models.Pharmacy.Nostrum> listDetail = this.pharmarcyManager.QueryNostrumDetail(f.Item.ID);
            ArrayList alTemp = new ArrayList();
            if (listDetail == null)
            {
                errText = "���Э��������ϸ����!" + pharmarcyManager.Err;

                return null;
            }
            decimal price = 0;
            decimal count = 0;
            string feeCode = string.Empty;
            string itemType = string.Empty;
            decimal totCost = 0;
            decimal packQty = 0m;
            FeeItemList feeDetail = null;
            if (f.Order.ID == null || f.Order.ID == string.Empty)
            {
                f.Order.ID = this.orderManager.GetNewOrderID();
                if (f.Order.ID == null || f.Order.ID == string.Empty)
                {
                    errText = "���ҽ����ˮ�ų���!";

                    return null;
                }
            }
            string comboNO = string.Empty;
            if (string.IsNullOrEmpty(f.Order.Combo.ID))
            {
                comboNO = f.Order.Combo.ID;
            }
            else
            {
                comboNO = orderManager.GetNewOrderComboID();
            }
            foreach (Neusoft.HISFC.Models.Pharmacy.Nostrum nosItem in listDetail)
            {
                Neusoft.HISFC.Models.Pharmacy.Item item = pharmarcyManager.GetItem(nosItem.Item.ID);
                if (item == null)
                {
                    errText = "����Э��������ϸ����!";

                    continue;
                }

                feeDetail = new FeeItemList();
                feeDetail.Item = item;
                feeCode = item.MinFee.ID;
                try
                {
                    DateTime nowTime = this.outpatientManager.GetDateTimeFromSysDateTime();
                    int age = (int)((new TimeSpan(nowTime.Ticks - rInfo.Birthday.Ticks)).TotalDays / 365);

                    NeuObject priceObj = new NeuObject();
                    priceObj.ID = rInfo.Pact.PriceForm;
                    priceObj.Name = age.ToString();
                    priceObj.User01 = NConvert.ToDecimal(item.Price).ToString();
                    priceObj.User02 = NConvert.ToDecimal(item.SpecialPrice).ToString();
                    priceObj.User02 = NConvert.ToDecimal(item.ChildPrice).ToString();
                    price = this.GetPrice(priceObj);

                    packQty = item.PackQty;
                    price = Neusoft.FrameWork.Public.String.FormatNumber(
                            NConvert.ToDecimal(price / packQty), 4);
                }
                catch (Exception e)
                {
                    errText = e.Message;

                    return null;
                }
                count = NConvert.ToDecimal(f.Item.Qty) * nosItem.Qty;
                totCost = price * count;

                feeDetail.Patient = f.Patient.Clone();
                feeDetail.Name = feeDetail.Item.Name;
                feeDetail.ID = feeDetail.Item.ID;
                feeDetail.RecipeOper = f.RecipeOper.Clone();
                feeDetail.Item.Price = price;
                feeDetail.Days = NConvert.ToDecimal(f.Days);
                feeDetail.FT.TotCost = totCost;
                feeDetail.Item.Qty = count;
                feeDetail.FeePack = f.FeePack;

                //�Է���ˣ�������Ϲ�����Ҫ���¼���!!!
                feeDetail.FT.OwnCost = totCost;
                feeDetail.ExecOper = f.ExecOper.Clone();
                feeDetail.Item.PriceUnit = item.MinUnit == string.Empty ? "g" : item.MinUnit;
                if (item.IsMaterial)
                {
                    feeDetail.Item.IsNeedConfirm = true;
                }
                else
                {
                    feeDetail.Item.IsNeedConfirm = false;
                }
                feeDetail.Order = f.Order;
                feeDetail.UndrugComb.ID = f.Item.ID;
                feeDetail.UndrugComb.Name = f.Item.Name;
                feeDetail.Order.Combo.ID = f.Order.Combo.ID;
                feeDetail.Item.IsMaterial = f.Item.IsMaterial;
                feeDetail.RecipeSequence = f.RecipeSequence;
                feeDetail.FTSource = f.FTSource;
                feeDetail.FeePack = f.FeePack;
                feeDetail.IsNostrum = true;
                feeDetail.Invoice = f.Invoice;
                feeDetail.InvoiceCombNO = f.InvoiceCombNO;
                feeDetail.NoBackQty = feeDetail.Item.Qty;
                feeDetail.Order.Combo.ID = comboNO;
                //if (this.rInfo.Pact.PayKind.ID == "03")
                //{
                //    Neusoft.HISFC.Models.Base.PactItemRate pactRate = null;

                //    if (pactRate == null)
                //    {
                //        pactRate = this.pactUnitItemRateManager.GetOnepPactUnitItemRateByItem(this.rInfo.Pact.ID, feeDetail.Item.ID);
                //    }
                //    if (pactRate != null)
                //    {
                //        if (pactRate.Rate.PayRate != this.rInfo.Pact.Rate.PayRate)
                //        {
                //            if (pactRate.Rate.PayRate == 1)//�Է�
                //            {
                //                feeDetail.ItemRateFlag = "1";
                //            }
                //            else
                //            {
                //                feeDetail.ItemRateFlag = "3";
                //            }
                //        }
                //        else
                //        {
                //            feeDetail.ItemRateFlag = "2";

                //        }
                //        if (f.ItemRateFlag == "3")
                //        {
                //            feeDetail.OrgItemRate = f.OrgItemRate;
                //            feeDetail.NewItemRate = f.NewItemRate;
                //            feeDetail.ItemRateFlag = "2";
                //        }
                //    }
                //    else
                //    {
                //        if (f.ItemRateFlag == "3")
                //        {

                //            if (rowFindZT["ZF"].ToString() != "1")
                //            {
                //                feeDetail.OrgItemRate = f.OrgItemRate;
                //                feeDetail.NewItemRate = f.NewItemRate;
                //                feeDetail.ItemRateFlag = "2";
                //            }
                //        }
                //        else
                //        {
                //            feeDetail.OrgItemRate = f.OrgItemRate;
                //            feeDetail.NewItemRate = f.NewItemRate;
                //            feeDetail.ItemRateFlag = f.ItemRateFlag;
                //        }
                //    }
                //}

                alTemp.Add(feeDetail);
            }
            if (alTemp.Count > 0)
            {
                if (f.FT.RebateCost > 0)//�м���
                {
                    if (rInfo.Pact.PayKind.ID != "01")
                    {
                        errText = "��ʱ��������Էѻ��߼���!";

                        return null;
                    }
                    //���ⵥ����
                    decimal rebateRate =
                        Neusoft.FrameWork.Public.String.FormatNumber(f.FT.RebateCost / f.FT.OwnCost, 2);
                    decimal tempFix = 0;
                    decimal tempRebateCost = 0;
                    foreach (FeeItemList feeTemp in alTemp)
                    {
                        feeTemp.FT.RebateCost = (feeTemp.FT.OwnCost) * rebateRate;
                        tempRebateCost += feeTemp.FT.RebateCost;
                    }
                    tempFix = f.FT.RebateCost - tempRebateCost;
                    FeeItemList fFix = alTemp[0] as FeeItemList;
                    fFix.FT.RebateCost = fFix.FT.RebateCost + tempFix;
                }
            }
            if (alTemp.Count > 0)
            {
                if (f.SpecialPrice > 0)//�������Է�
                {
                    decimal tempPrice = 0m;
                    string id = string.Empty;
                    foreach (FeeItemList feeTemp in alTemp)
                    {
                        if (feeTemp.Item.Price > tempPrice)
                        {
                            id = feeTemp.Item.ID;
                            tempPrice = feeTemp.Item.Price;
                        }
                    }

                    foreach (FeeItemList fee in alTemp)
                    {
                        if (fee.Item.ID == id)
                        {
                            fee.SpecialPrice = f.SpecialPrice;

                            break;
                        }
                    }
                }
            }

            return alTemp;
        }

        /// <summary>
        /// ���Э������
        /// </summary>
        /// <param name="feeItemLists"></param>
        /// <returns></returns>
        private int SplitNostrumDetail(Register rInfo, ref ArrayList  feeItemLists,ref ArrayList drugList, ref string errText)
        {
            ArrayList itemList = new ArrayList();
            foreach(FeeItemList f in feeItemLists)
            {
                if (f.Item.ItemType == EnumItemType.Drug)
                {
                    if (!f.IsConfirmed)
                    {
                        if (!f.Item.IsNeedConfirm)
                        {
                            drugList.Add(f);
                        }
                    }
                    if (f.IsNostrum)
                    {
                        ArrayList al = SplitNostrumDetail(rInfo, f, ref errText);
                        if (al == null)
                        {
                            return -1;
                        }
                        if (al.Count == 0)
                        {
                            errText = f.Item.Name + "��Э������,����û��ά����ϸ������ϸ�Ѿ�ͣ�ã�������Ϣ����ϵ��";
                            return -1;
                        }
                        itemList.AddRange(al);
                        continue;
                    }
                }
                itemList.Add(f);

            }
            feeItemLists.Clear();
            feeItemLists.AddRange(itemList);
            return 1;
        }

        /// <summary>
        /// �����շѺ���
        /// </summary>
        /// <param name="type">�շ�,���۱�־</param>
        /// <param name="r">���߹ҺŻ�����Ϣ</param>
        /// <param name="invoices">��Ʊ������</param>
        /// <param name="invoiceDetails">��Ʊ��ϸ����</param>
        /// <param name="feeDetails">������ϸ����</param>
        /// <param name="t">Transcation</param>
        /// <param name="payModes">֧����ʽ����</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns></returns>
        public bool ClinicFee(Neusoft.HISFC.Models.Base.ChargeTypes type, Neusoft.HISFC.Models.Registration.Register r,
            ArrayList invoices, ArrayList invoiceDetails, ArrayList feeDetails, ArrayList invoiceFeeDetails, ArrayList payModes, ref string errText)
        {

            Terminal.Confirm confirmIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm();
            Terminal.Booking bookingIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Booking();

            if (this.trans != null)
            {
                confirmIntegrate.SetTrans(this.trans);
                bookingIntegrate.SetTrans(this.trans);
            }

            invoiceType = controlParamIntegrate.GetControlParam<string>(Const.GET_INVOICE_NO_TYPE, false, "0");

            isDoseOnceCanNull = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.DOSE_ONCE_NULL, false, true);

            //�Ƿ�ŷ�Э������
            bool isSplitNostrum = controlParamIntegrate.GetControlParam<bool>(Const.Split_NostrumDetail, false, false);
            
            //����շ�ʱ��
            DateTime feeTime = inpatientManager.GetDateTimeFromSysDateTime();

            //����շѲ���Ա
            string operID = inpatientManager.Operator.ID;

            Neusoft.HISFC.Models.Base.Employee employee = inpatientManager.Operator as Neusoft.HISFC.Models.Base.Employee;
            //����ֵ
            int iReturn = 0;
            //���崦����
            string recipeNO = string.Empty;

            //������շѣ���÷�Ʊ��Ϣ
            if (type == Neusoft.HISFC.Models.Base.ChargeTypes.Fee)//�շ�
            {
                #region �շ�����
                //��Ʊ�Ѿ���Ԥ������������,ֱ�Ӳ���Ϳ�����.

                #region//��÷�Ʊ����,���ŷ�Ʊ��Ʊ�Ų�ͬ,����һ����Ʊ����,ͨ����Ʊ���к�,���Բ�ѯһ���շѵĶ��ŷ�Ʊ.

                string invoiceCombNO = outpatientManager.GetInvoiceCombNO();
                if (invoiceCombNO == null || invoiceCombNO == string.Empty)
                {
                    errText = "��÷�Ʊ��ˮ��ʧ��!" + outpatientManager.Err;

                    return false;
                }

                string invoiceUnion = outpatientManager.GetSequence("Fee.OutPatient.InvoiceUnionID");

                if (string.IsNullOrEmpty(invoiceUnion))
                {
                    errText ="��ȡ��Ʊ���ʧ�ܣ�"+outpatientManager.Err;
                    return false;
                }

                //���������ʾ���
                /////GetSpDisplayValue(myCtrl, t);
                //��һ����Ʊ��
                string mainInvoiceNO = string.Empty;
                string mainInvoiceCombNO = string.Empty;
                foreach (Balance balance in invoices)
                {
                    //����Ʊ��Ϣ,������ֻ����ʾ��
                    if (balance.Memo == "5")
                    {
                        mainInvoiceNO = balance.ID;

                        continue;
                    }

                    //�Էѻ��߲���Ҫ��ʾ����Ʊ,��ôȡ��һ����Ʊ����Ϊ����Ʊ��
                    if (mainInvoiceNO == string.Empty)
                    {
                        mainInvoiceNO = balance.Invoice.ID;
                        mainInvoiceCombNO = balance.CombNO;
                    }
                    balance.InvoiceCombo = invoiceUnion;
                }

                #endregion

                #region //���뷢Ʊ��ϸ��

                foreach (ArrayList tempsInvoices in invoiceDetails)
                {
                    foreach (ArrayList tempDetals in tempsInvoices)
                    {
                        foreach (BalanceList balanceList in tempDetals)
                        {
                            //�ܷ�Ʊ����
                            if (balanceList.Memo == "5")
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(((Balance)balanceList.BalanceBase).CombNO))
                            {
                                ((Balance)balanceList.BalanceBase).CombNO = invoiceCombNO;
                            }
                            balanceList.BalanceBase.BalanceOper.ID = operID;
                            balanceList.BalanceBase.BalanceOper.OperTime = feeTime;
                            balanceList.BalanceBase.IsDayBalanced = false;
                            balanceList.BalanceBase.CancelType = CancelTypes.Valid;
                            balanceList.ID = balanceList.ID.PadLeft(12, '0');

                            //���뷢Ʊ��ϸ�� fin_opb_invoicedetail
                            iReturn = outpatientManager.InsertBalanceList(balanceList);
                            if (iReturn == -1)
                            {
                                errText = "���뷢Ʊ��ϸ����!" + outpatientManager.Err;

                                return false;
                            }
                        }
                    }
                }

                #endregion

                #region Э������
                ArrayList noSplitDrugList = new ArrayList();
                if (isSplitNostrum)
                {

                    if (SplitNostrumDetail(r, ref feeDetails, ref noSplitDrugList, ref errText) < 0)
                    {
                        return false;
                    }
                }

                #endregion

                #region//ҩƷ��Ϣ�б�,���ɴ�����

                ArrayList drugLists = new ArrayList();
                //�������ɴ�����,������д�����,��ϸ�����¸�ֵ.
                if (!this.SetRecipeNOOutpatient(r ,feeDetails, ref errText))
                {
                    return false;
                }

                #endregion

               


                #region//���������ϸ

                foreach (FeeItemList f in feeDetails)
                {
                    //��֤����
                    if (!this.IsFeeItemListDataValid(f, ref errText))
                    {
                        return false;
                    }

                    //���û�д�����,���¸�ֵ
                    if (f.RecipeNO == null || f.RecipeNO == string.Empty)
                    {
                        if (recipeNO == string.Empty)
                        {
                            recipeNO = outpatientManager.GetRecipeNO();
                            if (recipeNO == null || recipeNO == string.Empty)
                            {
                                errText = "��ô����ų���!";

                                return false;
                            }
                        }
                    }

                    #region 2007-8-29 liuq �ж��Ƿ����з�Ʊ����ţ�û����ֵ
                    //{1A5CC61F-01F9-4dee-A6A8-580200C10EB4}
                    if (string.IsNullOrEmpty(f.InvoiceCombNO)|| f.InvoiceCombNO == "NULL")
                    {
                        f.InvoiceCombNO = invoiceCombNO;
                    }
                    #endregion
                    //
                    #region 2007-8-28 liuq �ж��Ƿ����з�Ʊ�ţ�û�г�ʼ��Ϊ12��0
                    if (string.IsNullOrEmpty(f.Invoice.ID))
                    {
                        f.Invoice.ID = mainInvoiceNO.PadLeft(12, '0');
                    }
                    #endregion
                    f.FeeOper.ID = operID;
                    f.FeeOper.OperTime = feeTime;
                    f.PayType = Neusoft.HISFC.Models.Base.PayTypes.Balanced;
                    f.TransType = TransTypes.Positive;
                    f.Patient.PID.CardNO = r.PID.CardNO;

                    //f.Patient = r.Clone();
                    ((Neusoft.HISFC.Models.Registration.Register)f.Patient).DoctorInfo.SeeDate = r.DoctorInfo.SeeDate;
                    if (((Register)f.Patient).DoctorInfo.Templet.Dept.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Dept.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Dept = r.DoctorInfo.Templet.Dept.Clone();
                    }
                    if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Doct = r.DoctorInfo.Templet.Doct.Clone();
                    }
                    if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                    {
                        f.RecipeOper.Dept.ID = r.DoctorInfo.Templet.Doct.User01;
                    }

                    if (f.ChargeOper.OperTime == DateTime.MinValue)
                    {
                        f.ChargeOper.OperTime = feeTime;
                    }
                    if (f.ChargeOper.ID == null || f.ChargeOper.ID == string.Empty)
                    {
                        f.ChargeOper.ID = operID;
                    }
                    //if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    //{
                    //    errText = "��ѡ��ҽ��";

                    //    return false;
                    //}

                    if (f.RecipeOper.ID == null || f.RecipeOper.ID == string.Empty)
                    {
                        f.RecipeOper.ID = ((Register)f.Patient).DoctorInfo.Templet.Doct.ID;
                    }

                    f.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
                    f.FeeOper.ID = operID;
                    f.FeeOper.OperTime = feeTime;
                    f.ExamineFlag = r.ChkKind;

                    //�������Ϊ������죬��ô������Ŀ�������ն���ˡ�
                    if (r.ChkKind == "2")
                    {
                        if (!f.IsConfirmed)
                        {
                            //�����Ŀ��ˮ��Ϊ�գ�˵��û�о����������̣���ô�����ն������Ϣ��
                            if (f.Order.ID == null || f.Order.ID == string.Empty)
                            {
                                f.Order.ID = orderManager.GetNewOrderID();
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    errText = "���ҽ����ˮ�ų���!";
                                    return false;
                                }

                                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                                {
                                    errText = "�����ն�����ȷ�ϱ�ʧ��!";

                                    return false;
                                }
                            }
                        }
                    }
                    else//�������������ĿΪ��Ҫ�ն������Ŀ������ն������Ϣ��
                    {
                        if (!f.IsConfirmed)
                        {
                            if (f.Item.IsNeedConfirm)
                            {
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    f.Order.ID = orderManager.GetNewOrderID();
                                }
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    errText = "���ҽ����ˮ�ų���!";

                                    return false;
                                }

                                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                                {
                                    errText = "�����ն�����ȷ�ϱ�ʧ��!" + confirmIntegrate.Err;

                                    return false;
                                }
                            }
                        }
                    }
                    //û�и�ֵҽ����ˮ��,��ֵ�µ�ҽ����ˮ��
                    if (f.Order.ID == null || f.Order.ID == string.Empty)
                    {
                        f.Order.ID = orderManager.GetNewOrderID();
                        if (f.Order.ID == null || f.Order.ID == string.Empty)
                        {
                            errText = "���ҽ����ˮ�ų���!";

                            return false;
                        }
                    }

                    if (r.ChkKind == "1")//�����������շѱ��
                    {
                        iReturn = examiIntegrate.UpdateItemListFeeFlagByMoOrder("1", f.Order.ID);
                        if (iReturn == -1)
                        {
                            errText = "��������շѱ��ʧ��!" + examiIntegrate.Err;

                            return false;
                        }
                    }

                    //���ɾ�����۱����е������Ŀ����Ŀ��Ϣ,������ϸ.
                    if (f.UndrugComb.ID != null && f.UndrugComb.ID.Length > 0)
                    {
                        iReturn = outpatientManager.DeletePackageByMoOrder(f.Order.ID);
                        if (iReturn == -1)
                        {
                            errText = "ɾ������ʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    //FeeItemList feeTemp = new FeeItemList();
                    //feeTemp = outpatientManager.GetFeeItemList(f.RecipeNO, f.SequenceNO);
                    //{39B2599D-2E90-4b3d-A027-4708A70E45C3}
                    int chargeItemCount = outpatientManager.GetChargeItemCount(f.RecipeNO, f.SequenceNO);
                    if (chargeItemCount == -1)
                    {
                        errText = "��ѯ��Ŀ��Ϣʧ�ܣ�";
                        return false;
                    }

                    if (chargeItemCount == 0)//˵��������
                    {
                        if (f.FTSource != "0" && (f.UndrugComb.ID == null || f.UndrugComb.ID == string.Empty))
                        {
                            errText = f.Item.Name + "�����Ѿ�����������Աɾ��,��ˢ�º����շ�!";

                            return false;
                        }

                        iReturn = outpatientManager.InsertFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���������ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    else
                    {
                        iReturn = outpatientManager.UpdateFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���·�����ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }

                    #region//��дҽ����Ϣ

                    if (f.FTSource == "1")
                    {
                        iReturn = orderOutpatientManager.UpdateOrderChargedByOrderID(f.Order.ID, operID);
                        if (iReturn == -1)
                        {
                            errText = "����ҽ����Ϣ����!" + orderOutpatientManager.Err;

                            return false;
                        }
                    }

                    #endregion

                    //�����ҩƷ,����û�б�ȷ�Ϲ�,���Ҳ���Ҫ�ն�ȷ��,��ô���뷢ҩ�����б�.
                    //if (f.Item.IsPharmacy)
                    if (f.Item.ItemType == EnumItemType.Drug)
                    {
                        if (!f.IsConfirmed)
                        {
                            if (!f.Item.IsNeedConfirm)
                            {
                                drugLists.Add(f);
                            }
                        }
                    }
                    //��Ҫҽ��ԤԼ,�����ն�ԤԼ��Ϣ.
                    if (f.Item.IsNeedBespeak && r.ChkKind != "2")
                    {
                        iReturn = bookingIntegrate.Insert(f);

                        if (iReturn == -1)
                        {
                            errText = "����ҽ��ԤԼ��Ϣ����!" + f.Name + bookingIntegrate.Err;

                            return false;
                        }
                    }

                }

                #endregion

                #region �����������շѱ��

                if (r.ChkKind == "2")//�������
                {
                    ArrayList recipeSeqList = this.GetRecipeSequenceForChk(feeDetails);
                    if (recipeSeqList != null && recipeSeqList.Count > 0)
                    {
                        foreach (string recipeSequenceTemp in recipeSeqList)
                        {
                            iReturn = examiIntegrate.UpdateItemListFeeFlagByRecipeSeq("1", recipeSequenceTemp);
                            if (iReturn == -1)
                            {
                                errText = "��������շѱ��ʧ��!" + examiIntegrate.Err;

                                return false;
                            }

                        }
                    }
                }

                #endregion

                #region//��ҩ������Ϣ

                string drugSendInfo = null;

                if (isSplitNostrum)
                {
                    drugLists.Clear();
                    foreach (FeeItemList item in noSplitDrugList)
                    {
                        foreach (FeeItemList f in feeDetails)
                        {
                            if (item.Order.ID == f.Order.ID)
                            {
                                item.RecipeNO = f.RecipeNO;
                                item.FeeOper = f.FeeOper;
                                break;
                            }
                        }
                    }
                    drugLists.AddRange(noSplitDrugList);
                }

                //���뷢ҩ������Ϣ,���ط�ҩ����,��ʾ�ڷ�Ʊ��
                iReturn = pharmarcyManager.ApplyOut(r, drugLists, string.Empty, feeTime, false, out drugSendInfo);
                if (iReturn == -1)
                {
                    errText = "����ҩƷ��ϸʧ��!" + pharmarcyManager.Err;

                    return false;
                }

                //'�����ҩƷ,��ô���÷�Ʊ����ʾ��ҩ������Ϣ.
                if (drugLists.Count > 0)
                {
                    //{02F6E9D7-E311-49a4-8FE4-BF2AC88B889B}���ε�С�汾���룬���ú��İ汾�Ĵ���
                    //foreach (Balance invoice in invoices)
                    //{
                    //    invoice.DrugWindowsNO = drugSendInfo;
                    //}
                    foreach (Balance invoice in invoices)
                    {
                        string tempInvoiceNo = string.Empty;
                        for (int i = 0; i < drugLists.Count; i++)
                        {
                            Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList oneFeeItem = new FeeItemList();
                            oneFeeItem = drugLists[i] as Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList;
                            //if (oneFeeItem.Item.IsPharmacy)
                            if (oneFeeItem.Item.ItemType == EnumItemType.Drug)
                            {
                                tempInvoiceNo = oneFeeItem.Invoice.ID;
                            }
                            if (invoice.Invoice.ID == tempInvoiceNo)
                            {
                                invoice.DrugWindowsNO = drugSendInfo;
                            }
                        }
                    }
                }

                #region//���뷢Ʊ����

                foreach (Balance balance in invoices)
                {
                    //����Ʊ��Ϣ,������ֻ����ʾ��
                    if (balance.Memo == "5")
                    {
                        mainInvoiceNO = balance.ID;

                        continue;
                    }
                    if (string.IsNullOrEmpty(balance.CombNO))
                    {
                        balance.CombNO = invoiceCombNO;
                    }
                    balance.BalanceOper.ID = operID;
                    balance.BalanceOper.OperTime = feeTime;
                    balance.Patient.Pact = r.Pact;
                    //����־
                    string tempExamineFlag = null;
                    //�������־ 0 ��ͨ���� 1 ������� 2 �������
                    //���û�и�ֵ,Ĭ��Ϊ��ͨ����
                    if (r.ChkKind.Length > 0)
                    {
                        tempExamineFlag = r.ChkKind;
                    }
                    else
                    {
                        tempExamineFlag = "0";
                    }
                    balance.ExamineFlag = tempExamineFlag;
                    balance.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;

                    //=====ȥ��CanceledInvoiceNO=string.Empty ·־��================
                    //balance.CanceledInvoiceNO = string.Empty;
                    //==============================================================

                    balance.IsAuditing = false;
                    balance.IsDayBalanced = false;
                    balance.ID = balance.ID.PadLeft(12, '0');
                    balance.Patient.Pact.Memo = r.User03;//�޶����
                    //�Էѻ��߲���Ҫ��ʾ����Ʊ,��ôȡ��һ����Ʊ����Ϊ����Ʊ��
                    if (mainInvoiceNO == string.Empty)
                    {
                        mainInvoiceNO = balance.Invoice.ID;
                    }
                    if (invoiceType == "0")
                    {
                        string tmpCount = outpatientManager.QueryExistInvoiceCount(balance.Invoice.ID);
                        if (tmpCount == "1")
                        {
                            DialogResult result = MessageBox.Show("�Ѿ����ڷ�Ʊ��Ϊ: " + balance.Invoice.ID +
                                " �ķ�Ʊ!,�Ƿ����?", "��ʾ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.No)
                            {
                                errText = "��Ʊ���ظ���ʱȡ�����ν���!";

                                return false;
                            }
                        }
                    }
                    else if (invoiceType == "1")
                    {
                        string tmpCount = outpatientManager.QueryExistInvoiceCount(balance.PrintedInvoiceNO);
                        if (tmpCount == "1")
                        {
                            DialogResult result = MessageBox.Show("�Ѿ�����Ʊ�ݺ�Ϊ: " + balance.PrintedInvoiceNO +
                                " �ķ�Ʊ!,�Ƿ����?", "��ʾ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.No)
                            {
                                errText = "��Ʊ���ظ���ʱȡ�����ν���!";

                                return false;
                            }
                        }
                    }
                    //���뷢Ʊ����fin_opb_invoice
                    iReturn = outpatientManager.InsertBalance(balance);
                    if (iReturn == -1)
                    {
                        errText = "�����������!" + outpatientManager.Err;

                        return false;
                    }
                }

                string invoiceNo = ((Balance)invoices[invoices.Count - 1]).Invoice.ID;
                string realInvoiceNo = ((Balance)invoices[invoices.Count - 1]).PrintedInvoiceNO;

                if (invoiceType == "2")
                {
                    for (int i = 0; i < invoices.Count; i++)
                    {
                        if (this.isNeedUpdateInvoiceNO)
                        {
                            iReturn = this.UpdateInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                            if (iReturn == -1)
                            {
                                this.Err = errText;

                                return false;
                            }
                        }
                        else
                        {
                            iReturn = this.UpdateOnlyRealInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                            if (iReturn == -1)
                            {
                                this.Err = errText;

                                return false;
                            }
                        }
                    }
                }
                else
                {
                    iReturn = this.UpdateInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                    if (iReturn == -1)
                    {
                        this.Err = errText;

                        return false;
                    }
                }

                #endregion

                #endregion

                #region ����֧����ʽ��Ϣ

                int payModeSeq = 1;

                foreach (BalancePay p in payModes)
                {
                    p.Invoice.ID = mainInvoiceNO.PadLeft(12, '0');
                    p.TransType = TransTypes.Positive;
                    p.Squence = payModeSeq.ToString();
                    p.IsDayBalanced = false;
                    p.IsAuditing = false;
                    p.IsChecked = false;
                    p.InputOper.ID = operID;
                    p.InputOper.OperTime = feeTime;
                    p.InvoiceUnion = invoiceUnion;
                    if (string.IsNullOrEmpty(p.InvoiceCombNO))
                    {
                        //p.InvoiceCombNO = mainInvoiceCombNO;
                        if (string.IsNullOrEmpty(mainInvoiceCombNO))
                        {
                            p.InvoiceCombNO = invoiceCombNO;
                        }
                        else
                        {
                            p.InvoiceCombNO = mainInvoiceCombNO;
                        }
                    }
                    p.CancelType = CancelTypes.Valid;

                    payModeSeq++;

                    //realCost += p.FT.RealCost;

                    iReturn = outpatientManager.InsertBalancePay(p);
                    if (iReturn == -1)
                    {
                        errText = "����֧����ʽ�����!" + outpatientManager.Err;

                        return false;
                    }

                    //{93E6443C-1FB5-45a7-B89D-F21A92200CF6}
                    //if (p.PayType.ID.ToString() == Neusoft.HISFC.Models.Fee.EnumPayType.YS.ToString())
                    if (p.PayType.ID.ToString() == "YS")
                    {
                        //bool returnValue = this.AccountPay(r.PID.CardNO, p.FT.TotCost, p.Invoice.ID, p.InputOper.Dept.ID);

                        

                        //if (!returnValue)
                        //{
                        //    errText = "��ȡ�����˻�ʧ��!" + "\n" + this.Err;

                        //    return false;
                        //}
                        //{DA67A335-E85E-46e1-A672-4DB409BCC11B}
                        int returnValue = this.AccountPay(r, p.FT.TotCost, p.Invoice.ID, (accountManager.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID, "C");
                        if (returnValue < 0)
                        {
                            errText = "��ȡ�����˻�ʧ��!" + "\n" + this.Err;

                            return false;
                        }
                        //if (returnValue == 0)
                        //{
                        //    errText = "ȡ���ʻ�֧��!";
                        //    return false;
                        //}
                    }
                }
                #endregion

                #region//�������ֱ���շѻ��ߺ���컼�ߣ����¿����־

                string noRegRules = controlParamIntegrate.GetControlParam(Const.NO_REG_CARD_RULES, false, "9");

                if (r.PID.CardNO.Substring(0, 1) != noRegRules && r.ChkKind.Length == 0)
                {
                    //���¿����־
                    iReturn = registerManager.UpdateSeeDone(r.ID);

                    if (iReturn <= 0)
                    {
                        errText = "���¿����־����!" + registerManager.Err;

                        return false;
                    }
                }
                //�������������շ�,��ô����Һ���Ϣ,����Ѿ������,��ô����.
                if (r.PID.CardNO.Substring(0, 1) == noRegRules)
                {
                    r.InputOper.OperTime = DateTime.MinValue;
                    r.InputOper.ID = operID;
                    r.IsFee = true;
                    r.TranType = TransTypes.Positive;
                    iReturn = registerManager.Insert(r);
                    if (iReturn == -1)
                    {
                        if (registerManager.DBErrCode != 1)//���������ظ�
                        {
                            errText = "����Һ���Ϣ����!" + registerManager.Err;

                            return false;
                        }
                    }
                }
                //�����ҽ������,���±���ҽ��������Ϣ�� fin_ipr_siinmaininfo
                if (r.Pact.PayKind.ID == "02")
                {
                    //�����ѽ����־
                    r.SIMainInfo.IsBalanced = true;
                    // iReturn = interfaceManager.update(r);
                    //{8F40C4C6-F331-4925-B96E-7C3D5444611C}
                    //if (iReturn < 0)
                    //{
                    //    errText = "����ҽ�����߽�����Ϣ����!" + interfaceManager.Err;
                    //    return false;
                    //}
                    //{8F40C4C6-F331-4925-B96E-7C3D5444611C}
                }

                #endregion

                #region//��Ʊ��ӡ
                //���2007-12-30liuq
                //string invoicePrintDll = null;

                //invoicePrintDll = controlParamIntegrate.GetControlParam<string>(Const.INVOICEPRINT, false, string.Empty);

                //if (invoicePrintDll == null || invoicePrintDll == string.Empty)
                //{
                //    MessageBox.Show("û�����÷�Ʊ��ӡ�������շ���ά��!");

                //    return false;
                //}

                //iReturn = PrintInvoice(invoicePrintDll, r, invoices, invoiceDetails, feeDetails, invoiceFeeDetails, payModes, false, ref errText);
                //if (iReturn == -1)
                //{
                //    return false;
                //}

                #endregion

                #endregion
            }
            else//����
            {
                #region ��������

                string noRegRules = controlParamIntegrate.GetControlParam<string>(Const.NO_REG_CARD_RULES, false, "9");
                if (r.PID.CardNO.Substring(0, 1) == noRegRules)
                {
                    r.InputOper.OperTime = DateTime.MinValue;
                    r.InputOper.ID = outpatientManager.Operator.ID;
                    r.IsFee = true;
                    r.TranType = TransTypes.Positive;
                    iReturn = registerManager.Insert(r);
                    if (iReturn == -1)
                    {
                        if (registerManager.DBErrCode != 1)//���������ظ�
                        {
                            errText = "����Һ���Ϣ����!" + registerManager.Err;

                            return false;
                        }
                    }
                }
                //�����۱�����Ϣ.
                bool returnValue = this.SetChargeInfo(r, feeDetails, feeTime, ref errText);
                if (!returnValue)
                {
                    return false;
                }

                #endregion
            }

            return true;
        }


        /// <summary>
        /// �����շѺ���
        /// </summary>
        /// <param name="type">�շ�,���۱�־</param>
        /// <param name="r">���߹ҺŻ�����Ϣ</param>
        /// <param name="invoices">��Ʊ������</param>
        /// <param name="invoiceDetails">��Ʊ��ϸ����</param>
        /// <param name="feeDetails">������ϸ����</param>
        /// <param name="t">Transcation</param>
        /// <param name="payModes">֧����ʽ����</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns></returns>
        public bool ClinicFeeSaveFee(Neusoft.HISFC.Models.Base.ChargeTypes type, Neusoft.HISFC.Models.Registration.Register r,
            ArrayList invoices, ArrayList invoiceDetails, ArrayList feeDetails, ArrayList invoiceFeeDetails, ArrayList payModes, ref string errText)
        {

            Terminal.Confirm confirmIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Confirm();
            Terminal.Booking bookingIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Terminal.Booking();

            if (this.trans != null)
            {
                confirmIntegrate.SetTrans(this.trans);
                bookingIntegrate.SetTrans(this.trans);
            }

            invoiceType = controlParamIntegrate.GetControlParam<string>(Const.GET_INVOICE_NO_TYPE, false, "0");

            isDoseOnceCanNull = controlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.Const.DOSE_ONCE_NULL, false, true);

            //����շ�ʱ��
            DateTime feeTime = inpatientManager.GetDateTimeFromSysDateTime();

            //����շѲ���Ա
            string operID = inpatientManager.Operator.ID;

            Neusoft.HISFC.Models.Base.Employee employee = inpatientManager.Operator as Neusoft.HISFC.Models.Base.Employee;
            //����ֵ
            int iReturn = 0;
            //���崦����
            string recipeNO = string.Empty;

            //������շѣ���÷�Ʊ��Ϣ
            if (type == Neusoft.HISFC.Models.Base.ChargeTypes.Fee)//�շ�
            {
                #region �շ�����
                //��Ʊ�Ѿ���Ԥ������������,ֱ�Ӳ���Ϳ�����.

                #region//��÷�Ʊ����,���ŷ�Ʊ��Ʊ�Ų�ͬ,����һ����Ʊ����,ͨ����Ʊ���к�,���Բ�ѯһ���շѵĶ��ŷ�Ʊ.

                string invoiceCombNO = outpatientManager.GetInvoiceCombNO();
                if (invoiceCombNO == null || invoiceCombNO == string.Empty)
                {
                    errText = "��÷�Ʊ��ˮ��ʧ��!" + outpatientManager.Err;

                    return false;
                }
                //���������ʾ���
                /////GetSpDisplayValue(myCtrl, t);
                //��һ����Ʊ��
                string mainInvoiceNO = string.Empty;
                string mainInvoiceCombNO = string.Empty;
                foreach (Balance balance in invoices)
                {
                    //����Ʊ��Ϣ,������ֻ����ʾ��
                    if (balance.Memo == "5")
                    {
                        mainInvoiceNO = balance.ID;

                        continue;
                    }

                    //�Էѻ��߲���Ҫ��ʾ����Ʊ,��ôȡ��һ����Ʊ����Ϊ����Ʊ��
                    if (mainInvoiceNO == string.Empty)
                    {
                        mainInvoiceNO = balance.Invoice.ID;
                        mainInvoiceCombNO = balance.CombNO;
                    }
                }

                #endregion

                #region //���뷢Ʊ��ϸ��

                foreach (ArrayList tempsInvoices in invoiceDetails)
                {
                    foreach (ArrayList tempDetals in tempsInvoices)
                    {
                        foreach (BalanceList balanceList in tempDetals)
                        {
                            //�ܷ�Ʊ����
                            if (balanceList.Memo == "5")
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(((Balance)balanceList.BalanceBase).CombNO))
                            {
                                ((Balance)balanceList.BalanceBase).CombNO = invoiceCombNO;
                            }
                            balanceList.BalanceBase.BalanceOper.ID = operID;
                            balanceList.BalanceBase.BalanceOper.OperTime = feeTime;
                            balanceList.BalanceBase.IsDayBalanced = false;
                            balanceList.BalanceBase.CancelType = CancelTypes.Valid;
                            balanceList.ID = balanceList.ID.PadLeft(12, '0');

                            //���뷢Ʊ��ϸ�� fin_opb_invoicedetail
                            iReturn = outpatientManager.InsertBalanceList(balanceList);
                            if (iReturn == -1)
                            {
                                errText = "���뷢Ʊ��ϸ����!" + outpatientManager.Err;

                                return false;
                            }
                        }
                    }
                }

                #endregion

                #region//ҩƷ��Ϣ�б�,���ɴ�����

                ArrayList drugLists = new ArrayList();
                //�������ɴ�����,������д�����,��ϸ�����¸�ֵ.
                if (!this.SetRecipeNOOutpatient(r,feeDetails, ref errText))
                {
                    return false;
                }

                #endregion

                #region//���������ϸ

                foreach (FeeItemList f in feeDetails)
                {
                    //��֤����
                    if (!this.IsFeeItemListDataValid(f, ref errText))
                    {
                        return false;
                    }

                    //���û�д�����,���¸�ֵ
                    if (f.RecipeNO == null || f.RecipeNO == string.Empty)
                    {
                        if (recipeNO == string.Empty)
                        {
                            recipeNO = outpatientManager.GetRecipeNO();
                            if (recipeNO == null || recipeNO == string.Empty)
                            {
                                errText = "��ô����ų���!";

                                return false;
                            }
                        }
                    }

                    #region 2007-8-29 liuq �ж��Ƿ����з�Ʊ����ţ�û����ֵ
                    if (string.IsNullOrEmpty(f.InvoiceCombNO))
                    {
                        f.InvoiceCombNO = invoiceCombNO;
                    }
                    #endregion
                    //
                    #region 2007-8-28 liuq �ж��Ƿ����з�Ʊ�ţ�û�г�ʼ��Ϊ12��0
                    if (string.IsNullOrEmpty(f.Invoice.ID))
                    {
                        f.Invoice.ID = mainInvoiceNO.PadLeft(12, '0');
                    }
                    #endregion
                    f.FeeOper.ID = operID;
                    f.FeeOper.OperTime = feeTime;
                    f.PayType = Neusoft.HISFC.Models.Base.PayTypes.Balanced;
                    f.TransType = TransTypes.Positive;
                    f.Patient.PID.CardNO = r.PID.CardNO;
                    //f.Patient = r.Clone();
                    ((Neusoft.HISFC.Models.Registration.Register)f.Patient).DoctorInfo.SeeDate = r.DoctorInfo.SeeDate;
                    if (((Register)f.Patient).DoctorInfo.Templet.Dept.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Dept.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Dept = r.DoctorInfo.Templet.Dept.Clone();
                    }
                    if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Doct = r.DoctorInfo.Templet.Doct.Clone();
                    }
                    if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                    {
                        f.RecipeOper.Dept.ID = r.DoctorInfo.Templet.Doct.User01;
                    }

                    if (f.ChargeOper.OperTime == DateTime.MinValue)
                    {
                        f.ChargeOper.OperTime = feeTime;
                    }
                    if (f.ChargeOper.ID == null || f.ChargeOper.ID == string.Empty)
                    {
                        f.ChargeOper.ID = operID;
                    }
                    //if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    //{
                    //    errText = "��ѡ��ҽ��";

                    //    return false;
                    //}

                    if (f.RecipeOper.ID == null || f.RecipeOper.ID == string.Empty)
                    {
                        f.RecipeOper.ID = ((Register)f.Patient).DoctorInfo.Templet.Doct.ID;
                    }

                    f.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;
                    f.FeeOper.ID = operID;
                    f.FeeOper.OperTime = feeTime;
                    f.ExamineFlag = r.ChkKind;

                    //�������Ϊ������죬��ô������Ŀ�������ն���ˡ�
                    if (r.ChkKind == "2")
                    {
                        if (!f.IsConfirmed)
                        {
                            //�����Ŀ��ˮ��Ϊ�գ�˵��û�о����������̣���ô�����ն������Ϣ��
                            if (f.Order.ID == null || f.Order.ID == string.Empty)
                            {
                                f.Order.ID = orderManager.GetNewOrderID();
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    errText = "���ҽ����ˮ�ų���!";
                                    return false;
                                }

                                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                                {
                                    errText = "�����ն�����ȷ�ϱ�ʧ��!";

                                    return false;
                                }
                            }
                        }
                    }
                    else//�������������ĿΪ��Ҫ�ն������Ŀ������ն������Ϣ��
                    {
                        if (!f.IsConfirmed)
                        {
                            if (f.Item.IsNeedConfirm)
                            {
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    f.Order.ID = orderManager.GetNewOrderID();
                                }
                                if (f.Order.ID == null || f.Order.ID == string.Empty)
                                {
                                    errText = "���ҽ����ˮ�ų���!";

                                    return false;
                                }

                                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                                {
                                    errText = "�����ն�����ȷ�ϱ�ʧ��!" + confirmIntegrate.Err;

                                    return false;
                                }
                            }
                        }
                    }
                    //û�и�ֵҽ����ˮ��,��ֵ�µ�ҽ����ˮ��
                    if (f.Order.ID == null || f.Order.ID == string.Empty)
                    {
                        f.Order.ID = orderManager.GetNewOrderID();
                        if (f.Order.ID == null || f.Order.ID == string.Empty)
                        {
                            errText = "���ҽ����ˮ�ų���!";

                            return false;
                        }
                    }

                    if (r.ChkKind == "1")//�����������շѱ��
                    {
                        iReturn = examiIntegrate.UpdateItemListFeeFlagByMoOrder("1", f.Order.ID);
                        if (iReturn == -1)
                        {
                            errText = "��������շѱ��ʧ��!" + examiIntegrate.Err;

                            return false;
                        }
                    }

                    //���ɾ�����۱����е������Ŀ����Ŀ��Ϣ,������ϸ.
                    if (f.UndrugComb.ID != null && f.UndrugComb.ID.Length > 0)
                    {
                        iReturn = outpatientManager.DeletePackageByMoOrder(f.Order.ID);
                        if (iReturn == -1)
                        {
                            errText = "ɾ������ʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    FeeItemList feeTemp = new FeeItemList();
                    feeTemp = outpatientManager.GetFeeItemList(f.RecipeNO, f.SequenceNO);
                    if (feeTemp == null)//˵��������
                    {
                        if (f.FTSource != "0" && (f.UndrugComb.ID == null || f.UndrugComb.ID == string.Empty))
                        {
                            errText = f.Item.Name + "�����Ѿ�����������Աɾ��,��ˢ�º����շ�!";

                            return false;
                        }

                        iReturn = outpatientManager.InsertFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���������ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    else
                    {
                        iReturn = outpatientManager.UpdateFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���·�����ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }

                    #region//��дҽ����Ϣ

                    if (f.FTSource == "1")
                    {
                        iReturn = orderOutpatientManager.UpdateOrderChargedByOrderID(f.Order.ID, operID);
                        if (iReturn == -1)
                        {
                            errText = "����ҽ����Ϣ����!" + orderOutpatientManager.Err;

                            return false;
                        }
                    }

                    #endregion

                    //�����ҩƷ,����û�б�ȷ�Ϲ�,���Ҳ���Ҫ�ն�ȷ��,��ô���뷢ҩ�����б�.
                    //if (f.Item.IsPharmacy)
                    if (f.Item.ItemType == EnumItemType.Drug)
                    {
                        if (!f.IsConfirmed)
                        {
                            if (!f.Item.IsNeedConfirm)
                            {
                                drugLists.Add(f);
                            }
                        }
                    }
                    //��Ҫҽ��ԤԼ,�����ն�ԤԼ��Ϣ.
                    if (f.Item.IsNeedBespeak && r.ChkKind != "2")
                    {
                        iReturn = bookingIntegrate.Insert(f);

                        if (iReturn == -1)
                        {
                            errText = "����ҽ��ԤԼ��Ϣ����!" + f.Name + bookingIntegrate.Err;

                            return false;
                        }
                    }

                }

                #endregion

                #region �����������շѱ��

                if (r.ChkKind == "2")//�������
                {
                    ArrayList recipeSeqList = this.GetRecipeSequenceForChk(feeDetails);
                    if (recipeSeqList != null && recipeSeqList.Count > 0)
                    {
                        foreach (string recipeSequenceTemp in recipeSeqList)
                        {
                            iReturn = examiIntegrate.UpdateItemListFeeFlagByRecipeSeq("1", recipeSequenceTemp);
                            if (iReturn == -1)
                            {
                                errText = "��������շѱ��ʧ��!" + examiIntegrate.Err;

                                return false;
                            }

                        }
                    }
                }

                #endregion

                #region//��ҩ������Ϣ

                string drugSendInfo = null;
                //���뷢ҩ������Ϣ,���ط�ҩ����,��ʾ�ڷ�Ʊ��
                iReturn = pharmarcyManager.ApplyOut(r, drugLists, string.Empty, feeTime, false, out drugSendInfo);
                if (iReturn == -1)
                {
                    errText = "����ҩƷ��ϸʧ��!" + pharmarcyManager.Err;

                    return false;
                }
                //�����ҩƷ,��ô���÷�Ʊ����ʾ��ҩ������Ϣ.
                if (drugLists.Count > 0)
                {
                    foreach (Balance invoice in invoices)
                    {
                        invoice.DrugWindowsNO = drugSendInfo;
                    }
                }

                #region//���뷢Ʊ����

                foreach (Balance balance in invoices)
                {
                    //����Ʊ��Ϣ,������ֻ����ʾ��
                    if (balance.Memo == "5")
                    {
                        mainInvoiceNO = balance.ID;

                        continue;
                    }
                    if (string.IsNullOrEmpty(balance.CombNO))
                    {
                        balance.CombNO = invoiceCombNO;
                    }
                    balance.BalanceOper.ID = operID;
                    balance.BalanceOper.OperTime = feeTime;
                    balance.Patient.Pact = r.Pact;
                    //����־
                    string tempExamineFlag = null;
                    //�������־ 0 ��ͨ���� 1 ������� 2 �������
                    //���û�и�ֵ,Ĭ��Ϊ��ͨ����
                    if (r.ChkKind.Length > 0)
                    {
                        tempExamineFlag = r.ChkKind;
                    }
                    else
                    {
                        tempExamineFlag = "0";
                    }
                    balance.ExamineFlag = tempExamineFlag;
                    balance.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Valid;

                    //=====ȥ��CanceledInvoiceNO=string.Empty ·־��================
                    //balance.CanceledInvoiceNO = string.Empty;
                    //==============================================================

                    balance.IsAuditing = false;
                    balance.IsDayBalanced = false;
                    balance.ID = balance.ID.PadLeft(12, '0');
                    balance.Patient.Pact.Memo = r.User03;//�޶����
                    //�Էѻ��߲���Ҫ��ʾ����Ʊ,��ôȡ��һ����Ʊ����Ϊ����Ʊ��
                    if (mainInvoiceNO == string.Empty)
                    {
                        mainInvoiceNO = balance.Invoice.ID;
                    }
                    if (invoiceType == "0")
                    {
                        string tmpCount = outpatientManager.QueryExistInvoiceCount(balance.Invoice.ID);
                        if (tmpCount == "1")
                        {
                            DialogResult result = MessageBox.Show("�Ѿ����ڷ�Ʊ��Ϊ: " + balance.Invoice.ID +
                                " �ķ�Ʊ!,�Ƿ����?", "��ʾ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.No)
                            {
                                errText = "��Ʊ���ظ���ʱȡ�����ν���!";

                                return false;
                            }
                        }
                    }
                    else if (invoiceType == "1")
                    {
                        string tmpCount = outpatientManager.QueryExistInvoiceCount(balance.PrintedInvoiceNO);
                        if (tmpCount == "1")
                        {
                            DialogResult result = MessageBox.Show("�Ѿ�����Ʊ�ݺ�Ϊ: " + balance.PrintedInvoiceNO +
                                " �ķ�Ʊ!,�Ƿ����?", "��ʾ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.No)
                            {
                                errText = "��Ʊ���ظ���ʱȡ�����ν���!";

                                return false;
                            }
                        }
                    }
                    //���뷢Ʊ����fin_opb_invoice
                    iReturn = outpatientManager.InsertBalance(balance);
                    if (iReturn == -1)
                    {
                        errText = "�����������!" + outpatientManager.Err;

                        return false;
                    }
                }

                string invoiceNo = ((Balance)invoices[invoices.Count - 1]).Invoice.ID;
                string realInvoiceNo = ((Balance)invoices[invoices.Count - 1]).PrintedInvoiceNO;

                if (invoiceType == "2")
                {
                    for (int i = 0; i < invoices.Count; i++)
                    {
                        if (this.isNeedUpdateInvoiceNO)
                        {
                            iReturn = this.UpdateInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                            if (iReturn == -1)
                            {
                                this.Err = errText;

                                return false;
                            }
                        }
                        else
                        {
                            iReturn = this.UpdateOnlyRealInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                            if (iReturn == -1)
                            {
                                this.Err = errText;

                                return false;
                            }
                        }
                    }
                }
                else
                {
                    iReturn = this.UpdateInvoiceNO(invoiceNo, realInvoiceNo, ref errText);
                    if (iReturn == -1)
                    {
                        this.Err = errText;

                        return false;
                    }
                }

                #endregion

                #endregion

                #region ����֧����ʽ��Ϣ

                //int payModeSeq = 1;

                //foreach (BalancePay p in payModes)
                //{
                //    p.Invoice.ID = mainInvoiceNO.PadLeft(12, '0');
                //    p.TransType = TransTypes.Positive;
                //    p.Squence = payModeSeq.ToString();
                //    p.IsDayBalanced = false;
                //    p.IsAuditing = false;
                //    p.IsChecked = false;
                //    p.InputOper.ID = operID;
                //    p.InputOper.OperTime = feeTime;
                //    if (string.IsNullOrEmpty(p.InvoiceCombNO))
                //    {
                //        p.InvoiceCombNO = mainInvoiceCombNO;
                //    }
                //    p.CancelType = CancelTypes.Valid;

                //    payModeSeq++;

                //    //realCost += p.FT.RealCost;

                //    iReturn = outpatientManager.InsertBalancePay(p);
                //    if (iReturn == -1)
                //    {
                //        errText = "����֧����ʽ�����!" + outpatientManager.Err;

                //        return false;
                //    }

                //    if (p.PayType.ID.ToString() == Neusoft.HISFC.Models.Fee.EnumPayType.YS.ToString())
                //    {
                //        bool returnValue = this.AccountPay(r.PID.CardNO, p.FT.TotCost, p.Invoice.ID, p.InputOper.Dept.ID);
                //        if (!returnValue)
                //        {
                //            errText = "��ȡ�����˻�ʧ��!" + "\n" + this.Err;

                //            return false;
                //        }
                //    }
                //}
                #endregion

                #region//�������ֱ���շѻ��ߺ���컼�ߣ����¿����־

                string noRegRules = controlParamIntegrate.GetControlParam(Const.NO_REG_CARD_RULES, false, "9");

                if (r.PID.CardNO.Substring(0, 1) != noRegRules && r.ChkKind.Length == 0)
                {
                    //���¿����־
                    iReturn = registerManager.UpdateSeeDone(r.ID);

                    if (iReturn <= 0)
                    {
                        errText = "���¿����־����!" + registerManager.Err;

                        return false;
                    }
                }
                //�������������շ�,��ô����Һ���Ϣ,����Ѿ������,��ô����.
                if (r.PID.CardNO.Substring(0, 1) == noRegRules)
                {
                    r.InputOper.OperTime = DateTime.MinValue;
                    r.InputOper.ID = operID;
                    r.IsFee = true;
                    r.TranType = TransTypes.Positive;
                    iReturn = registerManager.Insert(r);
                    if (iReturn == -1)
                    {
                        if (registerManager.DBErrCode != 1)//���������ظ�
                        {
                            errText = "����Һ���Ϣ����!" + registerManager.Err;

                            return false;
                        }
                    }
                }
                ////�����ҽ������,���±���ҽ��������Ϣ�� fin_ipr_siinmaininfo
                //if (r.Pact.PayKind.ID == "02")
                //{
                //    //�����ѽ����־
                //    r.SIMainInfo.IsBalanced = true;
                //    // iReturn = interfaceManager.update(r);
                //    if (iReturn < 0)
                //    {
                //        errText = "����ҽ�����߽�����Ϣ����!" + interfaceManager.Err;
                //        return false;
                //    }
                //}

                #endregion

                #region ��Ʊ��ӡ

                //string invoicePrintDll = null;

                //invoicePrintDll = controlParamIntegrate.GetControlParam<string>(Const.INVOICEPRINT, false, string.Empty);

                //if (invoicePrintDll == null || invoicePrintDll == string.Empty)
                //{
                //    MessageBox.Show("û�����÷�Ʊ��ӡ�������շ���ά��!");

                //    return false;
                //}

                //iReturn = PrintInvoice(invoicePrintDll, r, invoices, invoiceDetails, feeDetails, invoiceFeeDetails, payModes, false, ref errText);
                //if (iReturn == -1)
                //{
                //    return false;
                //}

                #endregion

                #endregion
            }
            else//����
            {
                #region ��������

                string noRegRules = controlParamIntegrate.GetControlParam<string>(Const.NO_REG_CARD_RULES, false, "9");
                if (r.PID.CardNO.Substring(0, 1) == noRegRules)
                {
                    r.InputOper.OperTime = DateTime.MinValue;
                    r.InputOper.ID = outpatientManager.Operator.ID;
                    r.IsFee = true;
                    r.TranType = TransTypes.Positive;
                    iReturn = registerManager.Insert(r);
                    if (iReturn == -1)
                    {
                        if (registerManager.DBErrCode != 1)//���������ظ�
                        {
                            errText = "����Һ���Ϣ����!" + registerManager.Err;

                            return false;
                        }
                    }
                }
                //�����۱�����Ϣ.
                bool returnValue = this.SetChargeInfo(r, feeDetails, feeTime, ref errText);
                if (!returnValue)
                {
                    return false;
                }

                #endregion
            }

            //������Ӧ֢{E4C0E5CF-D93F-48f9-A53C-9ADCCED97A7E}
            Neusoft.HISFC.BizProcess.Interface.FeeInterface.IAdptIllnessOutPatient iAdptIllnessOutPatient = null;
            iAdptIllnessOutPatient = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IAdptIllnessOutPatient)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IAdptIllnessOutPatient;
            if (iAdptIllnessOutPatient != null)
            {
                //������Ӧ֢��Ϣ
                int returnValue = iAdptIllnessOutPatient.SaveOutPatientFeeDetail(r, ref feeDetails);
                if (returnValue < 0)
                {
                    return false;
                }

            }

            return true;
        }


        /// <summary>
        /// ������߸��»�����Ϣ
        /// </summary>
        /// <param name="r">�Һ���Ϣ</param>
        /// <param name="feeItemLists">������Ϣ</param>
        /// <param name="chargeTime">�շ�ʱ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>true�ɹ� false ʧ��</returns>
        public bool SetChargeInfo(Register r, ArrayList feeItemLists, DateTime chargeTime, ref string errText)
        {
            bool returnValue = false;
            int iReturn = 0;
            string recipeSeq = null;//�շ�����

            recipeSeq = outpatientManager.GetRecipeSequence();
            if (recipeSeq == null || recipeSeq == string.Empty)
            {
                errText = "����շ����кų���!";

                return false;
            }

            //���ô�����
            returnValue = this.SetRecipeNOOutpatient(r,feeItemLists, ref errText);
            if (!returnValue)
            {
                return false;
            }

            foreach (FeeItemList f in feeItemLists)
            {
                //��֤���ݺϷ���
                if (!this.IsFeeItemListDataValid(f, ref errText))
                {
                    return false;
                }
                //���۱���
                f.ChargeOper.ID = outpatientManager.Operator.ID;
                f.ChargeOper.OperTime = chargeTime;
                f.Patient = r.Clone();

                f.Patient.PID.CardNO = r.PID.CardNO;

                ((Neusoft.HISFC.Models.Registration.Register)f.Patient).DoctorInfo.SeeDate = r.DoctorInfo.SeeDate;
                if (((Register)f.Patient).DoctorInfo.Templet.Dept.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Dept.ID == string.Empty)
                {
                    ((Register)f.Patient).DoctorInfo.Templet.Dept = r.DoctorInfo.Templet.Dept.Clone();
                }
                if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                {
                    ((Register)f.Patient).DoctorInfo.Templet.Doct = r.DoctorInfo.Templet.Doct.Clone();
                }
                if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                {
                    f.RecipeOper.Dept.ID = r.User01;
                }

                f.PayType = PayTypes.Charged;
                f.TransType = TransTypes.Positive;
                f.NoBackQty = f.Item.Qty;
                f.ExamineFlag = r.ChkKind;
                if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                {
                    f.RecipeOper.Dept.ID = r.User01;
                }
                if (f.Order.ID == null || f.Order.ID == string.Empty)//û�и�ֵҽ����ˮ��
                {
                    f.Order.ID = orderManager.GetNewOrderID();
                    if (f.Order.ID == null || f.Order.ID == string.Empty)
                    {
                        errText = "���ҽ����ˮ�ų���!";

                        return false;
                    }
                }
                if (f.RecipeSequence == null || f.RecipeSequence == string.Empty)
                {
                    f.RecipeSequence = recipeSeq;
                }
                if (f.InvoiceCombNO == null || f.InvoiceCombNO == string.Empty)
                {
                    f.InvoiceCombNO = "NULL";
                }

                iReturn = outpatientManager.InsertFeeItemList(f);

                #region �������,����컮��ʱ�Ѿ�����,��������
                //if (r.ChkKind == "2")//�������
                //{

                //    Neusoft.HISFC.Models.Terminal.TerminalApply terminalApply = new Neusoft.HISFC.Models.Terminal.TerminalApply();
                //    terminalApply.Item = f;
                //    terminalApply.Patient = r;
                //    terminalApply.InsertOperEnvironment.OperTime = chargeTime;
                //    terminalApply.InsertOperEnvironment.ID = outpatientManager.Operator.ID;

                //    terminalApply.PatientType = "4";

                //    iReturn = terminalManager.InsertMedTechItem(terminalApply);
                //    if (iReturn == -1)
                //    {
                //        errText = "�����ն�����ȷ�ϱ�ʧ��!" + myConfirm.Err;

                //        return false;
                //    }

                //    if (f.Item.IsNeedBespeak)
                //    {
                //        ////iReturn = terminalManager.MedTechApply(f, this.trans);
                //        if (iReturn == -1)
                //        {
                //            errText = "����ҽ��ԤԼ��Ϣ����!" + f.Name + terminalManager.Err;

                //            return false;
                //        }
                //    }
                //}
                #endregion

                if (iReturn == -1)
                {
                    if (outpatientManager.DBErrCode == 1)//�����ظ���ֱ�Ӹ���
                    {
                        iReturn = outpatientManager.UpdateFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���·�����ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    else
                    {
                        errText = "���������ϸʧ��!" + outpatientManager.Err;

                        return false;
                    }
                }
            }

            return true;
        }


        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        #region �����˻�ʹ�õĻ����շѺ���

        /// <summary>
        /// �˻��ն��շѺ���
        /// </summary>
        /// <param name="r">�Һ���Ϣ</param>
        /// <param name="f">������Ϣ</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>true�ɹ� false ʧ��</returns>
        public bool SaveFeeToAccount(Register r, string recipeNO, int sequenceNO, ref string errText)
        {

            FeeItemList f = outpatientManager.GetFeeItemList(recipeNO, sequenceNO);
            if (f == null)
            {
                errText = "��ѯ������Ϣʧ�ܣ�" + outpatientManager.Err;
                return false;
            }
            DateTime feeTime = outpatientManager.GetDateTimeFromSysDateTime();
            string feeOper = outpatientManager.Operator.ID;
            f.FeeOper.ID = feeOper;
            f.FeeOper.OperTime = feeTime;
            f.PayType = PayTypes.Balanced;
            int iReturn;
            iReturn = outpatientManager.UpdateFeeDetailFeeFlag(f);
            if (iReturn <= 0)
            {
                errText = "���·����շѱ��ʧ�ܣ�" + outpatientManager.Err;
                return false;
            }

            if (f.FTSource == "1")
            {
                iReturn = orderOutpatientManager.UpdateOrderChargedByOrderID(f.Order.ID, feeOper);
                if (iReturn == -1)
                {
                    errText = "����ҽ����Ϣ����!" + orderOutpatientManager.Err;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// �˻����ۺ���
        /// </summary>
        /// <param name="r">���߹Һ���Ϣ</param>
        /// <param name="feeItemLists">������Ϣ</param>
        /// <param name="chargeTime">����ʱ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns></returns>
        public bool SetChargeInfoToAccount(Register r, ArrayList feeItemLists, DateTime chargeTime, ref string errText)
        {
            #region ɾ�������
            ArrayList drugLists = new ArrayList();
            ArrayList undrugList = new ArrayList();
            Dictionary<string, string> dicRecipe = new Dictionary<string, string>();
            foreach (FeeItemList f in feeItemLists)
            {
                if (f.Item.ItemType == EnumItemType.Drug)
                {
                    if (!f.IsConfirmed)
                    {
                        if (!f.Item.IsNeedConfirm)
                        {
                            if (pharmarcyManager.DelApplyOut(f.RecipeNO, f.SequenceNO.ToString()) < 0)
                            {
                                errText = "ɾ����ҩ������Ϣϸʧ�ܣ�" + confirmIntegrate.Err;
                                return false;
                            }
                            if (!dicRecipe.ContainsKey(f.RecipeNO))
                            {
                                dicRecipe.Add(f.RecipeNO, f.ExecOper.Dept.ID);
                            }
                            else
                            {
                                if (dicRecipe[f.RecipeNO] != f.ExecOper.Dept.ID)
                                {
                                    dicRecipe.Add(f.RecipeNO, f.ExecOper.Dept.ID);
                                }
                            }
                            drugLists.Add(f);
                        }
                    }
                }
                else
                {
                    if (!f.IsConfirmed)
                    {
                        if (f.Item.IsNeedConfirm)
                        {
                            if (f.Order.ID == null || f.Order.ID == string.Empty)
                            {
                                f.Order.ID = orderManager.GetNewOrderID();
                            }
                            if (f.Order.ID == null || f.Order.ID == string.Empty)
                            {
                                errText = "���ҽ����ˮ�ų���!";

                                return false;
                            }
                            if (confirmIntegrate.DelTecApply(f.RecipeNO, f.SequenceNO.ToString()) < 0)
                            {
                                errText = "ɾ���ն�������Ϣʧ�ܣ�" + confirmIntegrate.Err;
                                return false;
                            }
                            undrugList.Add(f);
                        }
                    }
                }
            }
            #endregion

            #region ɾ��ҩƷ����ͷ��
            foreach (string recipeNO in dicRecipe.Keys)
            {
                if (pharmarcyManager.DeleteDrugStoRecipe(recipeNO, dicRecipe[recipeNO]) < 0)
                {
                    MessageBox.Show("ɾ������ͷ����Ϣʧ�ܣ�" + pharmarcyManager.Err);
                    return false;
                }
            }
            #endregion

            #region ��������

            foreach (FeeItemList f in feeItemLists)
            {
                f.IsAccounted = true;
                f.FT.TotCost = f.FT.OwnCost;
                if (string.IsNullOrEmpty((f.Patient as Register).DoctorInfo.Templet.Doct.ID))
                {
                    (f.Patient as Register).DoctorInfo.Templet.Doct = outpatientManager.Operator;
                }
            }

            bool resultValue = SetChargeInfo(r, feeItemLists, chargeTime, ref errText);
            if (!resultValue) return false;
            #endregion

            #region ����ҩƷ�����
            string drugSendInfo = null;
            //���뷢ҩ������Ϣ,���ط�ҩ����,��ʾ�ڷ�Ʊ��
            if (drugLists.Count > 0)
            {
                foreach (FeeItemList f in drugLists)
                {
                    if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Doct = outpatientManager.Operator;
                    }
                }
                
                int iReturn = pharmarcyManager.ApplyOut(r, drugLists, string.Empty, chargeTime, false, out drugSendInfo);
                if (iReturn == -1)
                {
                    errText = "����ҩƷ��ϸʧ��!" + pharmarcyManager.Err;

                    return false;
                }
            }
            #endregion

            #region �����ն���Ŀ����
            foreach (FeeItemList f in undrugList)
            {
                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                {
                    errText = "�����ն�����ȷ�ϱ�ʧ��!" + confirmIntegrate.Err;

                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// �˻��շ�(�ն˻����շ�ʹ��)
        /// </summary>
        /// <param name="r"></param>
        /// <param name="feeItemLists"></param>
        /// <param name="chargeTime"></param>
        /// <param name="errText"></param>
        /// <returns></returns>
        public bool SaveFeeToAccount(Register r, ArrayList feeItemLists, DateTime feeTime,string accountNO, ref string errText)
        {
            //�ܷ���
            decimal totCost = 0m;
            ArrayList drugLists = new ArrayList();
            ArrayList undrugList = new ArrayList();
            #region ����
            bool returnValue = false;
            int iReturn = 0;
            string recipeSeq = null;//�շ�����

            recipeSeq = outpatientManager.GetRecipeSequence();
            if (recipeSeq == null || recipeSeq == string.Empty)
            {
                errText = "����շ����кų���!";

                return false;
            }

            //���ô�����
            returnValue = this.SetRecipeNOOutpatient(r, feeItemLists, ref errText);
            if (!returnValue)
            {
                return false;
            }

            foreach (FeeItemList f in feeItemLists)
            {
                //��֤���ݺϷ���
                if (!this.IsFeeItemListDataValid(f, ref errText))
                {
                    return false;
                }
                //���۱���
                f.ChargeOper.ID = outpatientManager.Operator.ID;
                f.ChargeOper.OperTime = feeTime;
                f.Patient = r.Clone();

                f.FeeOper.ID = outpatientManager.Operator.ID;
                f.FeeOper.OperTime = feeTime;
                f.CancelType = CancelTypes.Valid;
                f.Patient.PID.CardNO = r.PID.CardNO;
                f.AccountNO = accountNO;//�˺�
                ((Neusoft.HISFC.Models.Registration.Register)f.Patient).DoctorInfo.SeeDate = r.DoctorInfo.SeeDate;
                if (((Register)f.Patient).DoctorInfo.Templet.Dept.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Dept.ID == string.Empty)
                {
                    ((Register)f.Patient).DoctorInfo.Templet.Dept = r.DoctorInfo.Templet.Dept.Clone();
                }
                if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                {
                    ((Register)f.Patient).DoctorInfo.Templet.Doct = r.DoctorInfo.Templet.Doct.Clone();
                }
                if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                {
                    f.RecipeOper.Dept.ID = r.User01;
                }

                f.PayType = PayTypes.Balanced;
                f.TransType = TransTypes.Positive;
                f.NoBackQty = f.Item.Qty;
                f.IsAccounted = true;//�˻��۷ѱ��
                f.ExamineFlag = r.ChkKind;
                if (f.RecipeOper.Dept.ID == null || f.RecipeOper.Dept.ID == string.Empty)
                {
                    f.RecipeOper.Dept.ID = r.User01;
                }
                if (f.Order.ID == null || f.Order.ID == string.Empty)//û�и�ֵҽ����ˮ��
                {
                    f.Order.ID = orderManager.GetNewOrderID();
                    if (f.Order.ID == null || f.Order.ID == string.Empty)
                    {
                        errText = "���ҽ����ˮ�ų���!";

                        return false;
                    }
                }
                if (f.RecipeSequence == null || f.RecipeSequence == string.Empty)
                {
                    f.RecipeSequence = recipeSeq;
                }
                if (f.InvoiceCombNO == null || f.InvoiceCombNO == string.Empty)
                {
                    f.InvoiceCombNO = "NULL";
                }
                //�ж��Ƿ��շ�
                if (outpatientManager.GetItemIsFee(r.ID, f.Order.ID,f.Item.ID) > 0)
                {
                    errText = f.Item.Name + "�����Ѿ��շ�,��ˢ�º����շ�";
                    return false;
                }

                iReturn = outpatientManager.InsertFeeItemList(f);
                if (iReturn == -1)
                {
                    if (outpatientManager.DBErrCode == 1)//�����ظ���ֱ�Ӹ���
                    {
                        iReturn = outpatientManager.UpdateFeeItemList(f);
                        if (iReturn <= 0)
                        {
                            errText = "���·�����ϸʧ��!" + outpatientManager.Err;

                            return false;
                        }
                    }
                    else
                    {
                        errText = "���������ϸʧ��!" + outpatientManager.Err;

                        return false;
                    }
                }
                #region ����ҩƷ��ҩƷ
                if (f.Item.ItemType == EnumItemType.Drug)
                {
                    if (!f.IsConfirmed)
                    {
                        if (!f.Item.IsNeedConfirm)
                        {
                            drugLists.Add(f);
                        }
                    }
                }
                else
                {
                    if (!f.IsConfirmed)
                    {
                        if (f.Item.IsNeedConfirm)
                        {
                            undrugList.Add(f);
                        }
                    }
                }
                #endregion

                totCost += f.FT.TotCost;
            }
            #endregion

            #region ��ȥ�˻�����
            string deptCode = (outpatientManager.Operator as Neusoft.HISFC.Models.Base.Employee).Dept.ID;
            int iResult = this.AccountPay(r, totCost, string.Empty,deptCode,string.Empty);
            if (iResult == -1)
            {
                
                return false;
            }
            #endregion

            #region ��������

            #region ����ҩƷ�����
            string drugSendInfo = null;
            //���뷢ҩ������Ϣ,���ط�ҩ����,��ʾ�ڷ�Ʊ��
            if (drugLists.Count > 0)
            {
                foreach (FeeItemList f in drugLists)
                {
                    if (((Register)f.Patient).DoctorInfo.Templet.Doct.ID == null || ((Register)f.Patient).DoctorInfo.Templet.Doct.ID == string.Empty)
                    {
                        ((Register)f.Patient).DoctorInfo.Templet.Doct = outpatientManager.Operator;
                    }
                }

                iReturn = pharmarcyManager.ApplyOut(r, drugLists, string.Empty, feeTime, false, out drugSendInfo);
                if (iReturn == -1)
                {
                    errText = "����ҩƷ��ϸʧ��!" + pharmarcyManager.Err;

                    return false;
                }
            }
            #endregion

            #region �����ն���Ŀ����
            foreach (FeeItemList f in undrugList)
            {
                Terminal.Result result = confirmIntegrate.ServiceInsertTerminalApply(f, r);

                if (result != Neusoft.HISFC.BizProcess.Integrate.Terminal.Result.Success)
                {
                    errText = "�����ն�����ȷ�ϱ�ʧ��!" + confirmIntegrate.Err;

                    return false;
                }
            }
            #endregion

            #endregion

            return true;
        }
        
        #endregion


        /// <summary>
        /// ��Ʊ��ӡ����
        /// </summary>
        /// <param name="invoicePrintDll">��Ʊ��ӡdllλ��</param>
        /// <param name="rInfo">���߻�����Ϣ</param>
        /// <param name="invoices">��Ʊ����</param>
        /// <param name="invoiceDetails">��Ʊ��ϸ����</param>
        /// <param name="feeDetails">������ϸ����</param>
        /// <param name="alPayModes">֧����ʽ����</param>
        /// <param name="t">���ݿ�����</param>
        /// <param name="isPreView">�Ƿ�Ԥ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int PrintInvoice(string invoicePrintDll, Register rInfo, ArrayList invoices, ArrayList invoiceDetails,
            ArrayList feeDetails, ArrayList alPayModes, bool isPreView, ref string errText)
        {

            int iReturn = 0;//����ֵ
            ArrayList alTempPayModes = new ArrayList();//��ʱ֧����ʽ

            if (alPayModes != null)
            {
                foreach (BalancePay p in alPayModes)
                {
                    alTempPayModes.Add(p);
                }
            }

            if (invoicePrintDll == null || invoicePrintDll == string.Empty)
            {
                errText = "û��ά����Ʊ��ӡ����!��ά��";
                return -1;
            }
            invoicePrintDll = Application.StartupPath + invoicePrintDll;
            ArrayList alPrint = new ArrayList();
            IInvoicePrint iInvoicePrint = null;

            for (int i = 0; i < invoices.Count; i++)
            {
                Balance invoice = invoices[i] as Balance;
                if (invoice.Memo == "5")
                {
                    continue;
                }

                ArrayList invoiceDetailsTemp = ((ArrayList)invoiceDetails[0])[i] as ArrayList;
                object obj = null;
                Assembly a = Assembly.LoadFrom(invoicePrintDll);
                System.Type[] types = a.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.GetInterface("IInvoicePrint") != null)
                    {
                        try
                        {
                            obj = System.Activator.CreateInstance(type);
                            iInvoicePrint = obj as IInvoicePrint;

                            iInvoicePrint.SetTrans(this.trans);
                            //if (invoices.Count > 1 && rInfo.Pact.PayKind.ID == "01")
                            //{
                                string payMode = string.Empty;
                                DealSplitPayMode(alTempPayModes, invoice, ref payMode);
                                iInvoicePrint.SetPayModeType = "1";
                                iInvoicePrint.SplitInvoicePayMode = payMode;
                            //}

                            iReturn = iInvoicePrint.SetPrintValue(rInfo, invoice, invoiceDetailsTemp, feeDetails, isPreView);

                            if (iReturn == -1)
                            {
                                return 0;
                            }

                            alPrint.Add(obj);
                            break;
                        }
                        catch (Exception ex)
                        {
                            errText = ex.Message;

                            return 0;
                        }
                    }
                }
            }
            for (int i = 0; i < alPrint.Count; i++)//foreach(object objPrint in alPrint)
            {
                if (i == 0)
                {
                    iInvoicePrint = alPrint[i] as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IInvoicePrint;
                }
                iReturn = ((IInvoicePrint)alPrint[i]).Print();
                if (iReturn == -1)
                {
                    return 0;
                }
            }

            if (alPrint.Count > 0 && feeDetails.Count > 0)
            {
                try
                {
                    FeeItemList feeTemp = feeDetails[0] as FeeItemList;

                    if (iInvoicePrint != null && printRecipeHeler.GetObjectFromID(((Register)feeTemp.Patient).DoctorInfo.Templet.Doct.ID) == null)
                    {
                        iInvoicePrint.SetPrintOtherInfomation(rInfo, invoices, null, feeDetails);
                        iInvoicePrint.PrintOtherInfomation();
                    }
                }
                catch (Exception ex)
                {
                    errText = ex.Message;

                    return 0;
                }
            }

            return 1;
        }
        /// ��Ʊ��ӡ����
        /// </summary>
        /// <param name="invoicePrintDll">��Ʊ��ӡdllλ��</param>
        /// <param name="rInfo">���߻�����Ϣ</param>
        /// <param name="invoices">��Ʊ����</param>
        /// <param name="invoiceDetails">��Ʊ��ϸ����</param>
        /// <param name="feeDetails">������ϸ����</param>
        /// <param name="invoiceFeeDetails">��Ʊ������ϸ��Ϣ������Ʊ�����ķ�����ϸ��ÿ�������Ӧ�÷�Ʊ�µķ�����ϸ��</param>
        /// <param name="alPayModes">֧����ʽ����</param>
        /// <param name="t">���ݿ�����</param>
        /// <param name="isPreView">�Ƿ�Ԥ��</param>
        /// <param name="errText">������Ϣ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public int PrintInvoice(string invoicePrintDll, Register rInfo, ArrayList invoices, ArrayList invoiceDetails,
            ArrayList feeDetails, ArrayList invoiceFeeDetails, ArrayList alPayModes, bool isPreView, ref string errText)
        {

            int iReturn = 0;//����ֵ
            ArrayList alTempPayModes = new ArrayList();//��ʱ֧����ʽ

            if (alPayModes != null)
            {
                foreach (BalancePay p in alPayModes)
                {
                    alTempPayModes.Add(p);
                }
            }

            if (invoicePrintDll == null || invoicePrintDll == string.Empty)
            {
                errText = "û��ά����Ʊ��ӡ����!��ά��";
                return -1;
            }
            invoicePrintDll = Application.StartupPath + invoicePrintDll;
            ArrayList alPrint = new ArrayList();
            IInvoicePrint iInvoicePrint = null;

            for (int i = 0; i < invoices.Count; i++)
            {
                Balance invoice = invoices[i] as Balance;
                if (invoice.Memo == "5")
                {
                    continue;
                }

                ArrayList invoiceDetailsTemp = ((ArrayList)invoiceDetails[0])[i] as ArrayList;
                ArrayList invoiceFeeDetailsTemp = ((ArrayList)invoiceFeeDetails[0])[i] as ArrayList;

                object obj = null;
                Assembly a = Assembly.LoadFrom(invoicePrintDll);
                System.Type[] types = a.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.GetInterface("IInvoicePrint") != null)
                    {
                        try
                        {
                            obj = System.Activator.CreateInstance(type);
                            iInvoicePrint = obj as IInvoicePrint;

                            iInvoicePrint.SetTrans(this.trans);
                            //if (invoices.Count > 1 && rInfo.Pact.PayKind.ID == "01")
                            //{
                            string payMode = string.Empty;
                            DealSplitPayMode(alTempPayModes, invoice, ref payMode);
                            iInvoicePrint.SetPayModeType = "1";
                            iInvoicePrint.SplitInvoicePayMode = payMode;
                            //}

                            iReturn = iInvoicePrint.SetPrintValue(rInfo, invoice, invoiceDetailsTemp, invoiceFeeDetailsTemp, isPreView);

                            if (iReturn == -1)
                            {
                                return 0;
                            }

                            alPrint.Add(obj);
                            break;
                        }
                        catch (Exception ex)
                        {
                            errText = ex.Message;

                            return 0;
                        }
                    }
                }
            }
            for (int i = 0; i < alPrint.Count; i++)//foreach(object objPrint in alPrint)
            {
                if (i == 0)
                {
                    iInvoicePrint = alPrint[i] as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IInvoicePrint;
                }
                iReturn = ((IInvoicePrint)alPrint[i]).Print();
                if (iReturn == -1)
                {
                    return 0;
                }
            }

            if (alPrint.Count > 0 && feeDetails.Count > 0)
            {
                try
                {
                    FeeItemList feeTemp = feeDetails[0] as FeeItemList;

                    if (iInvoicePrint != null && printRecipeHeler.GetObjectFromID(((Register)feeTemp.Patient).DoctorInfo.Templet.Doct.ID) == null)
                    {
                        iInvoicePrint.SetPrintOtherInfomation(rInfo, invoices, null, feeDetails);
                        iInvoicePrint.PrintOtherInfomation();
                    }
                }
                catch (Exception ex)
                {
                    errText = ex.Message;

                    return 0;
                }
            }

            return 1;
        }

        /// <summary>
        /// ���÷ַ�Ʊ���֧����ʽ
        /// </summary>
        /// <param name="alPayModes"></param>
        /// <param name="invoice"></param>
        /// <param name="payMode"></param>
        private void DealSplitPayMode(ArrayList alPayModes, Balance invoice, ref string payMode)
        {
            #region donggq--20101216--{3354E8E0-97B6-4ac6-B8D4-EA92C9DAD00E}
            //decimal totCost = invoice.FT.PayCost + invoice.FT.PubCost + invoice.FT.OwnCost;
            //decimal cardCost = 0m;
            //foreach (BalancePay p in alPayModes)
            //{
            //    if (p.PayType.ID.ToString() == "CA" && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            payMode += "�ֽ�: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(p.FT.RealCost, 2);
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.TotCost -= totCost;
            //            payMode += "�ֽ�: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost, 2);
            //            break;
            //        }
            //    }
            //    if (p.PayType.ID.ToString() == "PS" && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            payMode += "ҽ����: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(p.FT.RealCost, 2);
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.RealCost -= totCost;
            //            payMode += "ҽ����: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost, 2);
            //            break;
            //        }
            //    }
            //    if ((p.PayType.ID.ToString() == "CD" || p.PayType.ID.ToString() == "DB") && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            cardCost += p.FT.RealCost;
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.RealCost -= totCost;
            //            cardCost += totCost;
            //            //payMode += "ҽ����: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost,2);
            //            break;
            //        }
            //    }
            //    if (p.PayType.ID.ToString() == "CH" && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            payMode += "֧Ʊ: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(p.FT.RealCost, 2);
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.RealCost -= totCost;
            //            payMode += "֧Ʊ: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost, 2);
            //            break;
            //        }
            //    }
            //    if (p.PayType.ID.ToString() == "SB" && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            payMode += "�籣��: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(p.FT.RealCost, 2);
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.RealCost -= totCost;
            //            payMode += "�籣��: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost, 2);
            //            break;
            //        }
            //    }
            //    if (p.PayType.ID.ToString() == "YS" && p.FT.RealCost > 0)
            //    {
            //        if (p.FT.RealCost <= totCost)
            //        {
            //            totCost -= p.FT.RealCost;
            //            payMode += "Ժ���˻�: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(p.FT.RealCost, 2);
            //            p.FT.RealCost = 0;
            //        }
            //        else
            //        {
            //            p.FT.TotCost -= totCost;
            //            payMode += "Ժ���˻�: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(totCost, 2);
            //            break;
            //        }
            //    }
            //}

            //if (cardCost > 0)
            //{
            //    payMode += "���п�: " + Neusoft.FrameWork.Public.String.FormatNumberReturnString(cardCost, 2);
            //}

            foreach (BalancePay p in alPayModes)
            {
                if (p.PayType.ID.ToString() == "CA")
                {
                    payMode += "�ֽ�  ";
                }
                if (p.PayType.ID.ToString() == "PS")
                {
                    payMode += "ҽ����  ";
                }
                if (p.PayType.ID.ToString() == "CD" || p.PayType.ID.ToString() == "DB")
                {
                    payMode += "���п�  ";
                }
                if (p.PayType.ID.ToString() == "CH")
                {
                    payMode += "֧Ʊ  ";
                }
                if (p.PayType.ID.ToString() == "SB")
                {
                    payMode += "�籣��  ";
                }
                if (p.PayType.ID.ToString() == "YS")
                {
                    payMode += "Ժ���˻�  ";
                }
            } 
            #endregion
        }

        /// <summary>
        /// ��ô�����
        /// </summary>
        /// <returns></returns>
        public string GetRecipeNO()
        {
            this.SetDB(outpatientManager);

            return outpatientManager.GetRecipeNO();
        }

        /// <summary>
        /// ͨ��ҽ����Ŀ��ˮ�Ż��������Ŀ��ˮ�ţ��õ�������ϸ
        /// </summary>
        /// <param name="MOOrder">ҽ����Ŀ��ˮ�Ż��������Ŀ��ˮ��</param>
        /// <returns>null ���� ArrayList Fee.OutPatient.FeeItemListʵ�弯��</returns>
        public ArrayList QueryFeeDetailFromMOOrder(string MOOrder)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.QueryFeeDetailFromMOOrder(MOOrder);
        }

        /// <summary>
        /// ����ҽ�����������Ŀ��ˮ��ɾ����ϸ
        /// </summary>
        /// <param name="MOOrder">ҽ�����������Ŀ��ˮ��</param>
        /// <returns>�ɹ�: >= 1 ʧ��: -1 û��ɾ�������ݷ��� 0</returns>
        public int DeleteFeeItemListByMoOrder(string MOOrder)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.DeleteFeeItemListByMoOrder(MOOrder);
        }

        #region ��ѯ��Ʊ��Ϻ��Ƿ��Ѿ��շ�
        /// <summary>
        /// ���ݷ�Ʊ��ϺŲ�ѯ��������Ϣ�Ƿ��Ѿ��շѡ�
        /// </summary>
        /// <param name="RecipeSeq">��Ʊ��Ϻ�</param>
        /// <returns>0 ���շѣ� 1 δ�շ� ��-1 ��ѯ����</returns>
        public int IsFeeItemListByRecipeNO(string RecipeSeq)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.IsFeeItemListByRecipeNO(RecipeSeq);
        }
        #endregion

        /// <summary>
        /// ���ݲ����ź�ʱ��εõ�����δ�շ���ϸ
        /// </summary>
        /// <param name="cardNO">������</param>
        /// <param name="dtFrom">��ʼʱ��</param>
        /// <param name="dtTo">����ʱ��</param>
        /// <returns>�ɹ�:������ϸ ʧ��:null û������:����Ԫ����Ϊ0��ArrayList</returns>
        public ArrayList QueryOutpatientFeeItemLists(string cardNO, DateTime dtFrom, DateTime dtTo)
        {
            return outpatientManager.QueryFeeItemLists(cardNO, dtFrom, dtTo);
        }


        /// <summary>
        /// ���ݲ����ź�ʱ��εõ�����δ�շ���ϸ
        /// </summary>
        /// <param name="cardNO">������</param>
        /// <param name="dtFrom">��ʼʱ��</param>
        /// <param name="dtTo">����ʱ��</param>
        /// <returns>�ɹ�:������ϸ ʧ��:null û������:����Ԫ����Ϊ0��ArrayList</returns>
        public ArrayList QueryOutpatientFeeItemListsZs(string cardNO, DateTime dtFrom, DateTime dtTo)
        {
            return outpatientManager.QueryFeeItemListsForZs(cardNO, dtFrom, dtTo);
        }

        #region ����շ����к�

        /// <summary>
        /// ����շ����к�
        /// </summary>
        /// <returns>�ɹ�:�շ����к� ʧ��:null</returns>
        public string GetRecipeSequence()
        {
            this.SetDB(outpatientManager);
            return outpatientManager.GetRecipeSequence();
        }

        #endregion

        #endregion

        /// <summary>
        /// ��ȡ��ͬ��λ�б�
        /// </summary>
        /// <returns></returns>
        public ArrayList QueryPackList()
        {
            this.SetDB(pactManager);

            return pactManager.QueryPactUnitAll();
        }

        #endregion

        #region ����ҽ��վ���add by sunm

        /// <summary>
        /// �������������ϸ
        /// </summary>
        /// <param name="feeItemList"></param>
        /// <returns></returns>
        public int InsertFeeItemList(FeeItemList feeItemList)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.InsertFeeItemList(feeItemList);
        }

        /// <summary>
        /// �������������ϸ
        /// </summary>
        /// <param name="feeItemList"></param>
        /// <returns></returns>
        public int UpdateFeeItemList(FeeItemList feeItemList)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.UpdateFeeItemList(feeItemList);
        }

        /// <summary>
        /// ���ݴ����źʹ�������ˮ��ɾ��������ϸ
        /// </summary>
        /// <param name="recipeNO"></param>
        /// <param name="recipeSequence"></param>
        /// <returns></returns>
        public int DeleteFeeItemListByRecipeNO(string recipeNO, string recipeSequence)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.DeleteFeeItemListByRecipeNO(recipeNO, recipeSequence);
        }

        /// <summary>
        /// ������Ϻź���ˮ��ɾ��������ϸ
        /// </summary>
        /// <param name="combNO"></param>
        /// <param name="clinicCode"></param>
        /// <returns></returns>
        public int DeleteFeeDetailByCombNoAndClinicCode(string combNO, string clinicCode)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.DeleteFeeDetailByCombNoAndClinicCode(combNO, clinicCode);
        }

        /// <summary>
        /// ͨ��������ˮ�ź���Ϻŵõ�������ϸ
        /// </summary>
        /// <param name="combNO"></param>
        /// <param name="clinicCode"></param>
        /// <returns></returns>
        public ArrayList QueryFeeDetailbyComoNOAndClinicCode(string combNO, string clinicCode)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.QueryFeeDetailbyComoNOAndClinicCode(combNO, clinicCode);
        }

        #endregion

        #region Ժ��ע��

        /// <summary>
        /// ���Ժע��Ϣ�����÷�
        /// </summary>
        /// <param name="usageCode"></param>
        /// <returns></returns>
        public ArrayList GetInjectInfoByUsage(string usageCode)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.GetInjectInfoByUsage(usageCode);
        }

        /// <summary>
        /// ɾ���÷���Ŀ��Ϣ
        /// </summary>
        /// <param name="Usage"></param>
        /// <returns></returns>
        public int DelInjectInfo(string Usage)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.DelInjectInfo(Usage);
        }

        /// <summary>
        /// �����÷���Ŀ��Ϣ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int InsertInjectInfo(NeuObject obj)
        {
            SetDB(outpatientManager);
            return outpatientManager.InsertInjectInfo(obj);
        }

        #endregion

        #region addby xuewj 2009-8-26 ִ�е����� ����Ŀά�� {0BB98097-E0BE-4e8c-A619-8B4BCA715001}

        /// <summary>
        /// ������ִ�е�ά���õ�
        /// </summary>
        /// <param name="nruseID">��ʿվ����</param>
        /// <param name="sysClass">ϵͳ���</param>
        /// <param name="validState">��Ч״̬</param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public int QueryItemOutExecBill(string nruseID, string sysClass, string validState, ref DataSet ds)
        {
            this.SetDB(itemManager);
            return itemManager.QueryItemOutExecBill(nruseID, sysClass, validState, ref ds);
        }

        #endregion

        #endregion

        #region ö��

        /// <summary>
        /// �շѺ�����������
        /// </summary>
        private enum ChargeTypes
        {
            /// <summary>
            /// ����
            /// </summary>
            Charge = 0,

            /// <summary>
            /// �շ�
            /// </summary>
            Fee = 1,

            /// <summary>
            /// ���ۼ�¼ת�շ�
            /// </summary>
            ChargeToFee = 2,

        }

        #endregion

        #region �����ʻ�

        /// <summary>
        /// ���ݴ����źʹ�������ˮ�Ÿ����ѿ��˻���־
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="sequenceNO">��������ˮ��</param>
        /// <param name="isAccounted">true �Ѿ���ȡ�˻� false û�п�ȡ�˻�</param>
        /// <returns>�ɹ� 1 ʧ�� -1 �����ϸ������� 0</returns>
        public int UpdateAccountByRecipeNO(string recipeNO, int sequenceNO, bool isAccounted)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.UpdateAccountFlag(recipeNO, sequenceNO, isAccounted);
        }

        /// <summary>
        /// ����ҽ����ˮ�ź���Ŀ��������ѿ��˻���־
        /// </summary>
        /// <param name="itemCode">��Ŀ����</param>
        /// <param name="moOrder">ҽ����ˮ��</param>
        /// <param name="isAccounted">true �Ѿ���ȡ�˻� false û�п�ȡ�˻�</param>
        /// <returns>�ɹ� 1 ʧ�� -1 �����ϸ������� 0</returns>
        public int UpdateAccountByMoOrderAndItemCode(string itemCode, string moOrder, bool isAccounted)
        {
            this.SetDB(outpatientManager);

            return outpatientManager.UpdateAccountFlag(itemCode, moOrder, isAccounted);
        }


        /// <summary>
        /// �ʻ�֧��
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <param name="money">���</param>
        /// <param name="reMark">��ʶ</param>
        /// <param name="deptCode">���ұ���</param>
        /// <returns> 1 �ɹ� 0ȡ���շ� -1ʧ��</returns>
        public int AccountPay(HISFC.Models.RADT.Patient patient, decimal money, string reMark, string deptCode, string invoiceType)
        {
            this.SetDB(accountManager);
            bool bl = accountManager.AccountPayManager(patient, money, reMark, invoiceType, deptCode, 0);
            if (!bl) return -1;
            this.Err = accountManager.Err;
            return 1;
        }

        /// <summary>
        /// �˷��뻧
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <param name="money">���</param>
        /// <param name="reMark">��ʶ</param>
        /// <param name="deptCode">���ұ���</param>
        /// <returns>1�ɹ� -1ʧ��</returns>
        public int AccountCancelPay(HISFC.Models.RADT.Patient patient, decimal money, string reMark, string deptCode, string invoiceType)
        {
            this.SetDB(accountManager);
            bool bl = accountManager.AccountPayManager(patient, money, reMark, invoiceType, deptCode, 1);
            if (!bl)
            {
                this.Err = accountManager.Err;
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// �õ��ʻ����
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <param name="vacancy">���</param>
        /// <returns>0:�ʻ�ͣ�û��ʻ������� -1��ѯʧ�� 1�ɹ�</returns>
        public int GetAccountVacancy(string cardNO, ref decimal vacancy)
        {
            this.SetDB(accountManager);
            int resultValue = accountManager.GetVacancy(cardNO, ref vacancy);
            this.Err = accountManager.Err;
            return resultValue;
        }

        /// <summary>
        /// �õ��ʻ����
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <param name="vacancy">���</param>
        /// <param name="accountNO">�˺�</param>
        /// <returns>0:�ʻ�ͣ�û��ʻ������� -1��ѯʧ�� 1�ɹ�</returns>
        public int GetAccountVacancy(string cardNO, ref decimal vacancy,ref string accountNO)
        {
            this.SetDB(accountManager);
            int resultValue = accountManager.GetVacancy(cardNO, ref vacancy,ref accountNO);
            this.Err = accountManager.Err;
            return resultValue;
        }

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        private Neusoft.HISFC.BizProcess.Interface.Account.IPassWord GetIPassWord()
        {
            if (ipassWord == null)
            {
                System.Runtime.Remoting.ObjectHandle obj = System.Activator.CreateInstance("HISFC.Components.Account", "Neusoft.HISFC.Components.Account.Controls.ucPassWord");
                if (obj == null)
                {
                    return null; ;
                }
                ipassWord = obj.Unwrap() as Neusoft.HISFC.BizProcess.Interface.Account.IPassWord;
            }
            return ipassWord;
        }

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        /// <summary>
        /// ��֤�ʻ�����
        /// </summary>
        /// <param name="cardNo">���￨��</param>
        /// <returns>true �ɹ���falseʧ��</returns>
        public bool CheckAccountPassWord(HISFC.Models.RADT.Patient patient)
        {
            //GetIPassWord();
            //ipassWord.Patient = patient;
            //Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(ipassWord as Control);
            //if (ipassWord.IsOK)
            //{
            //    if (ipassWord.ValidPassWord)
            //        return true;
            //}
            //return false;

            return true;
        }

  
        /// <summary>
        /// ͨ�������Ų������￨��
        /// </summary>
        /// <param name="markNo">������</param>
        /// <param name="markType">������</param>
        /// <param name="cardNo">���￨��</param>
        /// <returns>bool true �ɹ���false ʧ��</returns>
        public bool GetCardNoByMarkNo(string markNo, ref string cardNo)
        {
            this.SetDB(accountManager);
            bool bl = accountManager.GetCardNoByMarkNo(markNo, ref cardNo);
            this.Err = accountManager.Err;
            return bl;
        }

        /// <summary>
        /// ͨ�������Ų������￨��
        /// </summary>
        /// <param name="markNo">������</param>
        /// <param name="markType">������</param>
        /// <param name="cardNo">���￨��</param>
        /// <returns>bool true �ɹ���false ʧ��</returns>
        public bool GetCardNoByMarkNo(string markNo, NeuObject markType, ref string cardNo)
        {
            this.SetDB(accountManager);
            bool bl = accountManager.GetCardNoByMarkNo(markNo, markType, ref cardNo);
            this.Err = accountManager.Err;
            return bl;
        }

        /// <summary>
        /// �������￨�Ų�������
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <returns>�û�����</returns>
        public string GetPassWordByCardNO(string cardNO)
        {
            this.SetDB(accountManager);
            return accountManager.GetPassWordByCardNO(cardNO);
        }

        /// <summary>
        /// �������￨�Ų��һ�����Ϣ
        /// </summary>
        /// <param name="cardNO">���￨��</param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.RADT.PatientInfo GetPatientInfo(string cardNO)
        {
            this.SetDB(accountManager);
            return accountManager.GetPatientInfo(cardNO);
        }


        /// <summary>
        /// ���ݿ������������
        /// </summary>
        /// <param name="markNo">���뿨��</param>
        /// <param name="accountCard">���ݹ�������Ŀ���Ϣ</param>
        /// <returns>1�ɹ�(�Ѿ�����) 0����Ϊ���� -1ʧ��</returns>
        public int ValidMarkNO(string markNo, ref HISFC.Models.Account.AccountCard accountCard)
        {
            this.SetDB(accountManager);
            return accountManager.GetCardByRule(markNo, ref accountCard);
        }
        #endregion

        #region ��Ʊ����{BF01254E-3C73-43d4-A644-4B258438294E}
        /// <summary>
        /// ���뷢Ʊ���ű�
        /// </summary>
        /// <param name="invoiceJumpRecord"></param>
        /// <returns></returns>
        public int InsertInvoiceJumpRecord(Neusoft.HISFC.Models.Fee.InvoiceJumpRecord invoiceJumpRecord)
        {
  
            //{BF01254E-3C73-43d4-A644-4B258438294E}
            this.SetDB(this.invoiceJumpRecordMgr);
            this.SetDB(invoiceServiceManager);
            //ȥ������
            string happenNO = this.invoiceJumpRecordMgr.GetMaxHappenNO(invoiceJumpRecord.Invoice.AcceptOper.ID, invoiceJumpRecord.Invoice.Type.ID);

            if (happenNO == "-1")
            {
                this.Err = this.invoiceJumpRecordMgr.Err;
                return -1;
            }

            invoiceJumpRecord.HappenNO = int.Parse(happenNO) + 1;
            invoiceJumpRecord.Oper.OperTime = this.invoiceJumpRecordMgr.GetDateTimeFromSysDateTime();

            int returnValue = 0;
            returnValue = this.invoiceJumpRecordMgr.InsertTable(invoiceJumpRecord);

            if (returnValue < 0)
            {
                this.Err = this.invoiceJumpRecordMgr.Err;
                return -1;
            }

            //����ʹ�ú�
            returnValue = this.invoiceServiceManager.UpdateUsedNO(invoiceJumpRecord.NewUsedNO, invoiceJumpRecord.Invoice.AcceptOper.ID, invoiceJumpRecord.Invoice.Type.ID);
            if (returnValue < 0)
            {
                this.Err = this.invoiceServiceManager.Err;
            }

            return 1;

        }
        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {

                Type[] type = new Type[5];
                type[0] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitInvoice);
                type[1] = typeof(IFeeOweMessage);
                type[2] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IAdptIllnessOutPatient);
                type[3] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.ISplitRecipe);

                type[4] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IShowFrmValidUserPassWord);
                return type;
            }
        }

        #endregion

        /// <summary>
        /// ���ݴ������Ŀ�������Զ���ֵ��
        /// </summary>
        /// <param name="pInfo">����ʵ��</param>
        /// <param name="item">��Ŀ��Ϣ���շ�����Ҫ��������Ŀʵ��item.qty��</param>
        /// <param name="execDept">ִ�п��Ҵ���</param>
        /// <returns></returns>
        public int FeeAutoItem(Neusoft.HISFC.Models.RADT.PatientInfo pInfo, Neusoft.HISFC.Models.Base.Item item,
            string execDept)
        {
            Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList ItemList = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
            Neusoft.HISFC.BizLogic.Fee.PactUnitInfo pactUnitManager = new Neusoft.HISFC.BizLogic.Fee.PactUnitInfo();

            pactUnitManager.SetTrans(this.trans);

            string operCode = pactUnitManager.Operator.ID;
            DateTime dtNow = pactUnitManager.GetDateTimeFromSysDateTime();

            ItemList.Item = item;
            //��Ժ����
            ((Neusoft.HISFC.Models.RADT.PatientInfo)ItemList.Patient).PVisit.PatientLocation.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
            //��ʿվ
            ((Neusoft.HISFC.Models.RADT.PatientInfo)ItemList.Patient).PVisit.PatientLocation.NurseCell.ID = pInfo.PVisit.PatientLocation.NurseCell.ID;
            //ִ�п���
            ItemList.ExecOper.Dept.ID = execDept;
            //�ۿ����
            ItemList.StockOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
            //��������
            ItemList.RecipeOper.Dept.ID = pInfo.PVisit.PatientLocation.Dept.ID;
            //����ҽ��
            ItemList.RecipeOper.ID = pInfo.PVisit.AdmittingDoctor.ID; //ҽ��
            //���ݴ����ʵ�崦��۸�
            decimal price = 0;
            //if (pactUnitManager.GetPrice(pInfo, item.IsPharmacy, item.ID, ref price) == -1)
            if (pactUnitManager.GetPrice(pInfo, item.ItemType, item.ID, ref price) == -1)
            {
                this.Err = "ȡ��Ŀ:" + item.Name + "�ļ۸����!" + pactUnitManager.Err;
                return -1;
            }
            item.Price = price;

            //ҩƷĬ�ϰ���С��λ�շ�,��ʾ�۸�ҲΪ��С��λ�۸�,�������ݿ��Ϊ��װ��λ�۸�
            //if (item.IsPharmacy)//ҩƷ
            if (item.ItemType == EnumItemType.Drug)//ҩƷ
            {
                price = Neusoft.FrameWork.Public.String.FormatNumber(item.Price / item.PackQty, 4);
            }

            /* �ⲿ�Ѿ���ֵ���֣��۸���������λ���Ƿ�ҩƷ
             * ItemList.Item.Price = 0;ItemList.Item.Qty;  
             * ItemList.Item.PriceUnit = "��"; 
             * ItemList.Item.IsPharmacy = false;
             */

            ItemList.FT.TotCost = Neusoft.FrameWork.Public.String.FormatNumber(ItemList.Item.Qty * price, 2);
            //ItemList.FT.OwnCost = ItemList.FT.TotCost;

            ItemList.PayType = PayTypes.Balanced;
            ItemList.IsBaby = false;
            ItemList.BalanceNO = 0;
            ItemList.BalanceState = "0";
            //��������
            ItemList.NoBackQty = item.Qty;

            //����Ա
            ItemList.FeeOper.ID = operCode;
            ItemList.ChargeOper.ID = operCode;
            ItemList.ChargeOper.OperTime = dtNow;
            ItemList.FeeOper.OperTime = dtNow;

            #region {3C6A1DD7-7522-418b-89A5-4B973ED320C3}
            ItemList.FT.OwnCost = ItemList.FT.TotCost;
            ItemList.TransType = TransTypes.Positive;
            #endregion

            //�����շѺ���
            return this.FeeItem(pInfo, ItemList);
        }

        //{6FC43DF1-86E1-4720-BA3F-356C25C74F16}
        #region �˻�����
        /// <summary>
        /// ���ݴ�����ִ�п��Ҳ���ҩƷ
        /// </summary>
        /// <param name="recipeNO">������</param>
        /// <param name="deptCode">ִ�п���</param>
        /// <returns></returns>
        public int GetDrugUnFeeCount(string recipeNO, string deptCode)
        {
            this.SetDB(outpatientManager);
            return outpatientManager.GetDrugUnFeeCount(recipeNO, deptCode);
        }

        #endregion

        //{645F3DDE-4206-4f26-9BC5-307E33BD882C}
        #region �ս���շ��ж�

        /// <summary>
        /// �ս���շ��ж�����
        /// </summary>
        /// <param name="feeOperCode">�տ�Ա����</param>
        /// <param name="isInpatient">�Ƿ�סԺ</param>
        /// <param name="errTxt">������Ϣ</param>
        /// <returns></returns>
        public bool AfterDayBalanceCanFee(string feeOperCode,bool isInpatient,ref string errTxt)
        {
            string canFeeType = controlParamIntegrate.GetControlParam<string>("100035", true, "0");
            //���ж�
            if (canFeeType == "0")
            {
                return true;
            }
            else
            {
                bool returnValue = false;
                DateTime now = empowerFeeManager.GetDateTimeFromSysDateTime();
                DateTime begin = Neusoft.FrameWork.Function.NConvert.ToDateTime(now.ToString("yyyy-MM-dd") + " 00:00:00");
                DateTime end = Neusoft.FrameWork.Function.NConvert.ToDateTime(now.ToString("yyyy-MM-dd") + " 23:59:59");
                if (isInpatient)
                {
                    returnValue = empowerFeeManager.QueryIsDayBalance(feeOperCode,begin.ToString(),end.ToString());
                }
                if (returnValue)
                {
                    //�ս��������շ�
                    if (canFeeType == "1")
                    {
                        errTxt = "�ս�󲻿������շ�!";
                        return false;
                    }
                    //�ս��ֻ�в�����Ȩ��ſ��շ�
                    if (canFeeType == "2")
                    {
                        //�Ƿ���Ȩ
                        if (empowerFeeManager.QueryIsEmpower(feeOperCode))
                        {
                            return true;
                        }
                        else
                        {
                            errTxt = "�ս��û�о�����Ȩ�����շѣ�";
                            return false;
                        }
                    }

                }
            }

            return true;
        }
        

        #endregion

        #region ֣���˻�����{C4259A87-6EFC-4f7a-8D7E-3FB0DF9B58E0}
        public int InsertAccountCard(Neusoft.HISFC.Models.Account.AccountCard accountCard)
        {
            this.SetDB(accountManager);
            return accountManager.InsertAccountCard(accountCard);
        }

        public int InsertAccountCardRecord(Neusoft.HISFC.Models.Account.AccountCardRecord accountCardRecord)
        {
            this.SetDB(accountManager);
            return accountManager.InsertAccountCardRecord(accountCardRecord);
        }
        #endregion
        #region ֣����������ȡ�ն�ȷ��״̬ {B98851B0-9C5A-4d68-ABB5-CB48C4DBD34B}
        /// <summary>
        /// ��ȡ�ն�ȷ��״̬
        /// </summary>
        /// <param name="execsql">ִ�е���ˮ��</param>
        /// <returns>false�� û��ȷ�� true���Ѿ�ȷ��</returns>
        public bool GetTecFlag(string execsql) {
            return inpatientManager.GetTecFlag(execsql);
        }
        #endregion

    }

    ///// <summary>
    ///// [��������: �����ʻ�Ԥ�����ӡ]<br></br>
    ///// [�� �� ��: ·־��]<br></br>
    ///// [����ʱ��: 2006-6-22]<br></br>
    ///// <�޸ļ�¼
    /////		�޸���=''
    /////		�޸�ʱ��=''
    /////		�޸�Ŀ��=''
    /////		�޸�����=''
    /////  />
    ///// </summary>
    //public interface IAccountPrint
    //{
    //    /// <summary>
    //    /// ���ô�ӡ����
    //    /// </summary>
    //    /// <param name="account">�ʻ�ʵ��</param>
    //    /// <returns></returns>
    //    int PrintSetValue(Neusoft.HISFC.Models.Account.Account account);
    //    /// <summary>
    //    /// ��ӡ
    //    /// </summary>
    //    /// <returns></returns>
    //    int Print();
    //}

    //public interface IFeeOweMessage
    //{
    //    /// <summary>
    //    /// Ƿ����ʾ
    //    /// </summary>
    //    /// <param name="patient">������Ϣ</param>
    //    /// <param name="ft">������Ϣ</param>
    //    /// <param name="feeItemLists">������ϸ</param>
    //    /// <param name="type">Ƿ����ʾ����</param>
    //    /// <param name="err">��ʾ��Ϣ</param>
    //    /// <returns>true:�ɹ� false:�����ڲ�����</returns>
    //    //{2518013C-40B2-4693-B494-3DE193C002FF} //���Ӵ�����ϸ
    //    bool FeeOweMessage(Neusoft.HISFC.Models.RADT.PatientInfo patient, FT ft,System.Collections.ArrayList feeItemLists,ref Fee.MessType type, ref string err);
    //}
}
