using System;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC;
using Neusoft.FrameWork.Models;
using System.Collections;
using System.Collections.Generic;

namespace Neusoft.HISFC.Models.Operation
{
    /// <summary>
    /// OpsApplication<br></br>
    /// [��������: OpsApplication�������뵥��]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-09-19]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    [Serializable]
    public class OperationAppllication : Neusoft.FrameWork.Models.NeuObject,
        Neusoft.HISFC.Models.Base.IDept
    {
        public OperationAppllication()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region �ֶ�
        /// <summary>
        /// ������Ա�Ľ�ɫ��������
        /// �����д��Ԫ��ΪNeusoft.HISFC.Models.Operator.ArrangeRole���Ͷ���
        /// ��ʵ���涨��� ����ҽ����ָ��ҽ��������ҽ�������Զ����Ժϲ�����������е�
        /// ֻ��Ϊ���ر�ǿ�������벿�ֵ���Ϣ�����ص����ඨ�岢��ֵ��Щ����
        /// </summary>		
        public ArrayList RoleAl = new ArrayList();

        /// <summary>
        /// ����ҽ��
        /// </summary>
        public NeuObject ReciptDoctor = new NeuObject();

        private bool isAnesth = false;
        #endregion

        #region ��������ʱ��Ҫ��д������
        //---------------------------------------------------------------
        #region ��ϵͳ��ȡ
        //---------------------------------------
        ///<summary>
        /// �������к�
        ///</summary>
        [Obsolete("Ӧ��ΪID", true)]
        public string OperationNO
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }
        ///<summary>
        ///������Ϣ
        ///</summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
        //---------------------------------------
        #endregion
        ///<summary>
        ///�������
        ///(�п���һ��������Ŷ�Ӧ�����ǰ��ϣ������һ����̬�������洢��Ϣ)
        ///(����Ԫ��ΪNeusoft.HISFC.Models.RADT.Diagnose Diagnose����)
        ///</summary>
        public ArrayList DiagnoseAl = new ArrayList();

        [Obsolete("PatientSouce", true)]
        public string Pasource = string.Empty;

        private string patientSouce = string.Empty;
        ///<summary>
        ///1��������/2סԺ����
        ///</summary>
        public string PatientSouce
        {
            get
            {
                return this.patientSouce;
            }
            set
            {
                this.patientSouce = value;
            }
        }


        [Obsolete("PreDate", true)]
        public DateTime Pre_Date = DateTime.MinValue;

        private DateTime preDate = DateTime.MinValue;
        ///<summary>
        ///����ԤԼʱ��
        ///</summary>
        public DateTime PreDate
        {
            get
            {
                return this.preDate;
            }
            set
            {
                this.preDate = value;
            }
        }

        ///<summary>
        ///����Ԥ����ʱ
        ///</summary>
        ///Why not use TimeSpan?	Robin
        public decimal Duration = 0;

        ///<summary>
        ///������Ϣ
        ///���п���һ��������Ŷ�Ӧ������������ˣ���һ����̬�������洢��Ϣ������Ԫ��ΪOperateInfo���ͣ�
        ///</summary>
        public List<OperationInfo> OperationInfos = new List<OperationInfo>();
        ///<summary>
        ///��������
        ///</summary>
        public Neusoft.FrameWork.Models.NeuObject AnesType = new Neusoft.FrameWork.Models.NeuObject();

        ///<summary>
        ///��������(�����Ⱦ����ͨ)
        ///</summary>
        public string OperateKind = "1";
        ///<summary>
        ///������ģ
        ///</summary>
        public Neusoft.FrameWork.Models.NeuObject OperationType = new Neusoft.FrameWork.Models.NeuObject();

        ///<summary>
        ///�п�����
        ///</summary>
        public Neusoft.FrameWork.Models.NeuObject InciType = new Neusoft.FrameWork.Models.NeuObject();

        ///<summary>
        ///������λ
        ///</summary>
        public Neusoft.FrameWork.Models.NeuObject OpePos = new Neusoft.FrameWork.Models.NeuObject();

        private Neusoft.HISFC.Models.Base.Employee operationDoctor;
        ///<summary>
        ///����ҽ��
        ///</summary>		
        public Neusoft.HISFC.Models.Base.Employee OperationDoctor
        {
            get
            {
                if (this.operationDoctor == null)
                {
                    this.operationDoctor = new Employee();
                }
                return this.operationDoctor;
            }
            set
            {
                this.operationDoctor = value;
            }
        }
        [Obsolete("OperationDoctor", true)]
        public Neusoft.HISFC.Models.Base.Employee Ops_docd = new Employee();

        private Neusoft.HISFC.Models.Base.Employee guideDoctor;
        ///<summary>
        ///ָ��ҽ��
        ///</summary>
        public Employee GuideDoctor
        {
            get
            {
                if (this.guideDoctor == null)
                {
                    this.guideDoctor = new Employee();
                }
                return this.guideDoctor;
            }
            set
            {
                this.guideDoctor = value;
            }
        }
        [Obsolete("GuideDoctor", true)]
        public Neusoft.HISFC.Models.Base.Employee Gui_docd = new Employee();

        ///<summary>
        ///����ҽ��
        ///���п����Ƕ��ˣ���ˣ���һ����̬�������洢��Ϣ������Ԫ��ΪNeusoft.HISFC.Models.RADT.Person���ͣ�
        ///</summary>
        public ArrayList HelperAl = new ArrayList();

        private Employee applyDoctor;
        ///<summary>
        ///����ҽ��
        ///</summary>
        public Employee ApplyDoctor
        {
            get
            {
                if (this.applyDoctor == null)
                {
                    this.applyDoctor = new Employee();
                }
                return this.applyDoctor;
            }
            set
            {
                this.applyDoctor = value;
            }
        }
        [Obsolete("ApplyDoctor", true)]
        public Neusoft.HISFC.Models.Base.Employee Apply_Doct = new Employee();

        ///<summary>
        ///����ʱ��
        ///</summary>
        [Obsolete("ApplyDate", true)]
        public DateTime Apply_Date = DateTime.MinValue;
        private DateTime applyDate = DateTime.MinValue;
        public DateTime ApplyDate
        {
            get
            {
                return this.applyDate;
            }
            set
            {
                this.applyDate = value;
            }
        }

        ///<summary>
        ///���뱸ע
        ///</summary>
        private string applyNote = string.Empty;
        public string ApplyNote
        {
            get
            {
                return this.applyNote;
            }
            set
            {
                this.applyNote = value;
            }
        }

        private string specialItem;
        /// <summary>
        /// ����ѡ��
        /// </summary>
        public string SpecialItem
        {
            get
            {
                return this.specialItem;
            }
            set
            {
                this.specialItem = value;
            }
        }

        private int specialItemIndex;
        /// <summary>
        /// ����ѡ������
        /// </summary>
        public int SpecialItemIndex
        {
            get
            {
                return this.specialItemIndex;
            }
            set
            {
                this.specialItemIndex = value;
            }
        }

        #region ��Ѫ���
        ///<summary>
        ///ѪҺ�ɷ�(ȫѪ��Ѫ����Ѫ���)
        ///</summary>
        public Neusoft.FrameWork.Models.NeuObject BloodType = new Neusoft.FrameWork.Models.NeuObject();
        ///<summary>
        ///Ѫ��
        ///</summary>
        public decimal BloodNum = 0;
        ///<summary>
        ///��Ѫ��λ
        ///</summary>
        public string BloodUnit = "ml";
        #endregion

        ///<summary>
        ///����ע������
        ///</summary>
        public string OpsNote = string.Empty;
        ///<summary>
        ///����ע������
        ///</summary>
        public string AneNote = string.Empty;
        ///<summary>
        ///0��̨/1��̨/2��̨
        ///</summary>
        public string TableType = string.Empty;

        [Obsolete("ApproveDoctor", true)]
        public Neusoft.HISFC.Models.Base.Employee ApprDocd = new Employee();
        private Neusoft.HISFC.Models.Base.Employee approveDoctor = new Employee();
        ///<summary>
        ///����ҽ��
        ///</summary>
        public Neusoft.HISFC.Models.Base.Employee ApproveDoctor
        {
            get
            {
                return this.approveDoctor;
            }
            set
            {
                this.approveDoctor = value;
            }
        }


        [Obsolete("ApproveDate", true)]
        public DateTime ApprDate = DateTime.MinValue;
        private DateTime approveDate = DateTime.MinValue;
        ///<summary>
        ///����ʱ��
        ///</summary>
        public DateTime ApproveDate
        {
            get
            {
                return this.approveDate;
            }
            set
            {
                this.approveDate = value;
            }
        }


        [Obsolete("ApproveNote", true)]
        public string ApprNote = "";
        private string approveNote = string.Empty;
        ///<summary>
        ///������ע
        ///</summary>
        public string ApproveNote
        {
            get
            {
                return this.approveNote;
            }
            set
            {
                this.approveNote = value;
            }
        }

        /// <summary>
        ///����Ա
        /// </summary>
        public Neusoft.HISFC.Models.Base.Employee User = new Employee();
        /// <summary>
        /// ǩ�ּ���
        /// </summary>
        private string folk = string.Empty;
        public string Folk
        {
            get
            {
                return this.folk;
            }
            set
            {
                this.folk = value;
            }
        }
        /// <summary>
        /// ������ϵ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject RelaCode = new Neusoft.FrameWork.Models.NeuObject();
        /// <summary>
        /// �������
        /// </summary>
        private string folkComment = string.Empty;
        public string FolkComment
        {
            get
            {
                return this.folkComment;
            }
            set
            {
                this.folkComment = value;
            }
        }

        /// <summary>
        /// ִ�п���
        /// </summary>
        private Neusoft.HISFC.Models.Base.Department ExecDept = new Neusoft.HISFC.Models.Base.Department();
        //---------------------------------------------------------------
        #endregion

        #region ��������ʱ��Ҫ��д������
        //---------------------------------------------------------------
        ///<summary>
        ///������
        ///</summary>
        public Neusoft.HISFC.Models.Base.Department OperateRoom = new Neusoft.HISFC.Models.Base.Department();
        ///<summary>
        ///�����
        ///</summary>
        public string RoomID = string.Empty;

        ///<summary>
        ///����̨
        ///</summary>
        public OpsTable OpsTable = new OpsTable();
        /// <summary>
        /// �������ϰ��ż�¼����
        /// Ԫ��ΪNeusoft.HISFC.Models.Operator.OpsApparatusRec����
        /// </summary>		
        public ArrayList AppaRecAl = new ArrayList();
        /// <summary>
        /// 1 �о� 0�޾�
        /// </summary>
        private bool isGermCarrying;
        [Obsolete("����ΪIsGermCarrying", true)]
        public bool bGerm
        {
            get
            {
                return this.isGermCarrying;
            }
            set
            {
                this.isGermCarrying = value;
            }
        }
        /// <summary>
        /// �Ƿ��о�
        /// </summary>
        public bool IsGermCarrying
        {
            get
            {
                return this.isGermCarrying;
            }
            set
            {
                this.isGermCarrying = value;
            }
        }
        /// <summary>
        /// 1 �ڲ��鿴���� 2 ҽ���鿴���� 
        /// </summary>
        public string ScreenUp = string.Empty;
        //---------------------------------------------------------------
        /// <summary>
        /// ҽ���Ƿ���Բ鿴�������Ž��(1 ��  2����)
        /// </summary>
        public string DocCanSee;

        #endregion

        #region ����
        /// <summary>
        /// ��ӷ�������
        /// </summary>
        /// <param name="operation">������Ŀ</param>
        /// Robin   2006-12-14
        public void AddOperation(object operation)
        {
            this.AddOperation(operation, false);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="operation">������Ŀ</param>
        /// <param name="mainFlag">�Ƿ�Ϊ����Ŀ</param>
        /// Robin   2006-12-14
        public void AddOperation(object operation, bool mainFlag)
        {
            if (operation.GetType() == typeof(Neusoft.HISFC.Models.Operation.OperationInfo))
            {
                this.OperationInfos.Add(operation as OperationInfo);
            }
            else if (operation.GetType() == typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
            {
                OperationInfo opItem = new OperationInfo();
                opItem.OperationItem = (Neusoft.HISFC.Models.Base.Item)operation;//������Ŀ
                opItem.FeeRate = 1m;//����
                opItem.Qty = 1;//����
                opItem.StockUnit = (operation as Neusoft.HISFC.Models.Base.Item).PriceUnit;//��λ
                opItem.OperateType.ID = (operation as Neusoft.HISFC.Models.Fee.Item.Undrug).OperationType.ID;
                opItem.IsValid = true;
                opItem.IsMainFlag = mainFlag;
                this.OperationInfos.Add(opItem);
            }
        }

        /// <summary>
        /// ���������Ա
        /// </summary>
        /// <param name="id">��ԱID</param>
        /// <param name="name">��Ա����</param>
        /// <param name="foreFlag">¼����</param>
        /// <param name="operationRole">��Ա��ɫ</param>
        /// Robin   2006-12-14
        public void AddRole(string id, string name, string foreFlag, EnumOperationRole operationRole)
        {
            ArrangeRole role = new ArrangeRole();
            role.OperationNo = this.ID;//�����
            role.ID = id;
            role.Name = name;
            role.RoleType.ID = operationRole;//��ɫ����
            role.ForeFlag = foreFlag;
            this.RoleAl.Add(role);
        }

        /// <summary>
        /// �Ƴ�������Ա
        /// </summary>
        /// <param name="id">��ԱID</param>
        /// <param name="operationRole">��ɫ</param>
        /// Robin   2006-12-27
        public int RemoveRole(string id, EnumOperationRole operationRole)
        {
            foreach (ArrangeRole role in this.RoleAl)
            {
                if (role.ID == id && role.RoleType.ID.ToString() == operationRole.ToString())
                {
                    this.RoleAl.Remove(role);
                    return 0;
                }
            }

            return -1;
        }
        #endregion
        #region ��־

        ///<summary>
        ///���뵥״̬(1�������� 2 �������� 3�������� 4�������)
        ///</summary>
        public string ExecStatus = "1";

        ///<summary>
        /// 0 δ������/ 1 ��������
        ///</summary>
        private bool isFinished;
        [Obsolete("����ΪIsFinished", true)]
        public bool bFinished
        {
            get
            {
                return this.IsFinished;
            }
            set
            {
                this.isFinished = value;
            }
        }
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return this.isFinished;
            }
            set
            {
                this.isFinished = value;
            }
        }

        ///<summary>
        ///�Ƿ�����
        ///</summary>		
        public bool IsAnesth
        {
            get
            {
                return this.isAnesth;
            }
            set
            {
                this.isAnesth = value;
            }
        }
        ///<summary>
        ///�Ӽ����� 1��/0��
        ///</summary>
        private bool isUrgent = false;
        public bool IsUrgent
        {
            get
            {
                return this.isUrgent;
            }
            set
            {
                this.isUrgent = value;
            }
        }
        ///<summary>
        ///��Σ 1��/0��
        ///</summary>
        private bool isChange = false;
        public bool IsChange
        {
            get
            {
                return this.isChange;
            }
            set
            {
                this.isChange = value;
            }
        }

        ///<summary>
        ///��֢ 1��/0��
        ///</summary>
        private bool isHeavy = false;
        public bool IsHeavy
        {
            get
            {
                return this.isHeavy;
            }
            set
            {
                this.isHeavy = value;
            }
        }
        ///<summary>
        ///�������� 1��0��
        ///</summary>
        private bool isSpecial = false;
        public bool IsSpecial
        {
            get
            {
                return this.isSpecial;
            }
            set
            {
                this.isSpecial = value;
            }
        }
        ///<summary>
        ///1��Ч/0��Ч
        ///</summary>
        public bool IsValid
        {
            get
            {
                return this.YNValid;
            }
            set
            {
                this.YNValid = value;
            }
        }
        private bool YNValid = true;
        [Obsolete("IsValid", true)]
        public bool bValid
        {
            get
            {
                return this.YNValid;
            }
            set
            {
                this.YNValid = value;
            }
        }
        ///<summary>
        ///1�ϲ�/0��
        ///</summary>
        private bool isUnite = false;
        public bool IsUnite
        {
            get
            {
                return this.isUnite;
            }
            set
            {
                this.isUnite = value;
            }
        }
        /// <summary>
        /// ������ʱָ�����Ƿ���Ҫ��̨��ʿ
        /// </summary>
        public bool IsAccoNurse
        {
            get
            {
                return this.YNAccoNur;
            }
            set
            {
                this.YNAccoNur = value;
            }
        }
        private bool YNAccoNur;
        [Obsolete("IsAccoNurse", true)]
        public bool bAccoNur
        {
            get
            {
                return this.YNAccoNur;
            }
            set
            {
                this.YNAccoNur = value;
            }
        }
        /// <summary>
        /// ������ʱָ�����Ƿ���ҪѲ�ػ�ʿ
        /// </summary>
        public bool IsPrepNurse
        {
            get
            {
                return this.YNPrepNur;
            }
            set
            {
                this.YNPrepNur = value;
            }
        }
        private bool YNPrepNur;
        [Obsolete("IsPrepNurse", true)]
        public bool bPrepNur
        {
            get
            {
                return this.YNPrepNur;
            }
            set
            {
                this.YNPrepNur = value;
            }
        }
        #endregion

        #region IDept ��Ա (�ӿڼ̳�)

        public Neusoft.FrameWork.Models.NeuObject InDept
        {
            get
            {
                // TODO:  ��� OpsApplication.InDept getter ʵ��
                return null;
            }
            set
            {
                // TODO:  ��� OpsApplication.InDept setter ʵ��
            }
        }

        public Neusoft.FrameWork.Models.NeuObject ExeDept
        {
            get
            {
                // TODO:  ��� OpsApplication.ExeDept getter ʵ��
                return this.ExecDept;
            }
            set
            {
                ExecDept = (Neusoft.HISFC.Models.Base.Department)value;
            }
        }

        public Neusoft.FrameWork.Models.NeuObject ReciptDept
        {
            get
            {
                // TODO:  ��� OpsApplication.ReciptDept getter ʵ��
                return null;
            }
            set
            {
                // TODO:  ��� OpsApplication.ReciptDept setter ʵ��
            }
        }

        public Neusoft.FrameWork.Models.NeuObject NurseStation
        {
            get
            {
                // TODO:  ��� OpsApplication.NurseStation getter ʵ��
                return null;
            }
            set
            {
                // TODO:  ��� OpsApplication.NurseStation setter ʵ��
            }
        }

        public Neusoft.FrameWork.Models.NeuObject StockDept
        {
            get
            {
                // TODO:  ��� OpsApplication.StockDept getter ʵ��
                return null;
            }
            set
            {
                // TODO:  ��� OpsApplication.StockDept setter ʵ��
            }
        }

        public Neusoft.FrameWork.Models.NeuObject DoctorDept
        {
            get
            {
                // TODO:  ��� OpsApplication.ReciptDoct getter ʵ��
                return null;
            }
            set
            {
                // TODO:  ��� OpsApplication.ReciptDoct setter ʵ��
            }
        }

        #endregion

        #region ����
        /// <summary>
        /// ����������
        /// </summary>
        /// Robin   2006-12-05
        public string MainOperationName
        {
            get
            {
                string opName = string.Empty;
                if (this.OperationInfos != null && this.OperationInfos.Count > 0)
                {
                    foreach (OperationInfo item in this.OperationInfos)
                    {
                        if (item.IsMainFlag)
                        {
                            opName = item.OperationItem.Name;
                            return opName;
                        }
                    }
                    if (opName.Length == 0)
                        opName = (this.OperationInfos[0] as OperationInfo).OperationItem.Name;
                }

                return opName;
            }
        }
        #endregion

        #region 
        
        //{B9DDCC10-3380-4212-99E5-BB909643F11B}
        /// <summary>
        /// '������𣨾����ѡ�飬ҽ������ʱ��д��'
        /// </summary>
        private string anesWay = string.Empty;

        /// <summary>
        /// '������𣨾����ѡ�飬ҽ������ʱ��д��'
        /// </summary>
        public string AnesWay
        {
            get { return anesWay; }
            set { anesWay = value; }
        }
        #endregion
        public new OperationAppllication Clone()
        {
            return base.Clone() as OperationAppllication;
        }

    }
}
