using System;
//using Neusoft.NFC;
using Neusoft.HISFC;
using Neusoft.FrameWork.Models;
namespace Neusoft.HISFC.Models.Order
{
	/// <summary>
	/// Neusoft.HISFC.Models.Order.Order<br></br>
	/// [��������: ҽ������ʵ��]<br></br>
	/// [�� �� ��: ���Ʒ�]<br></br>
	/// [����ʱ��: 2006-09-10]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [Serializable]
    public class Order:Neusoft.FrameWork.Models.NeuObject,
		Neusoft.HISFC.Models.Base.IDept,
		Neusoft.HISFC.Models.Base.IBaby,Neusoft.HISFC.Models.Base.ISort
		{

		/// <summary>
		/// ҽ������ʵ��
		/// ID ҽ����ˮ��
		/// </summary>
		public Order()
		{
			
		}

		#region ����

		#region ˽��

		/// <summary>
		/// ���߻�����Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.RADT.PatientInfo patient= new Neusoft.HISFC.Models.RADT.PatientInfo();
		
		/// <summary>
		/// �Ƿ���ҪƤ�ԡ��¼�20050815
		/// 1 ����ҪƤ�ԡ�������ҪƤ�ԡ��������ԡ���������
		/// </summary>
		private int hypoTest = 1;

		private int injectCount; //Ժ��ע�����
		
		/// <summary>
		/// ����ҽ��
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject  doctor =new Neusoft.FrameWork.Models.NeuObject();

        private Neusoft.FrameWork.Models.NeuObject doctorDept = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ���/ִ�л�ʿ
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject nurse=new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ��¼��
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment oper=new Neusoft.HISFC.Models.Base.OperEnvironment();

		
		/// <summary>
		/// ֹͣ��
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment dcOper=new Neusoft.HISFC.Models.Base.OperEnvironment();

		
		/// <summary>
		/// ִ����
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment execOper = new Neusoft.HISFC.Models.Base.OperEnvironment();
		

		/// <summary>
		/// ֹͣԭ��
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject dcReason = new Neusoft.FrameWork.Models.NeuObject();

		#region ʱ��

		/// <summary>
		/// ҩƷ��Ŀ/��ҩƷ��Ŀ
		/// </summary>
		private Neusoft.HISFC.Models.Base.Item item = new Neusoft.HISFC.Models.Base.Item();
	

		/// <summary>
		/// ҽ������ʱ��
		/// </summary>
		private DateTime dtMOTime;
		
		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		private DateTime beginTime;
		

		/// <summary>
		/// ����ʱ��
		/// </summary>
		private DateTime endTime;

	

		/// <summary>
		/// �´ηֽ�ʱ��
		/// </summary>
		private DateTime nextMOTime;

		/// <summary>
		/// ���ʱ��
		/// </summary>
		private DateTime confirmTime;

		#endregion

		/// <summary>
		/// ״̬��0������ 1����� 2 ִ�� 3 ���ϣ�
		/// </summary>
		private int status;

		/// <summary>
		/// �����Ϣ
		/// </summary>
		private Combo combo=new Combo();

		/// <summary>
		/// �÷�
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject usage=new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ÿ�μ���
		/// </summary>
		private decimal doseOnce;

		/// <summary>
		/// ÿ�μ�����λ
		/// </summary>
		private string doseUnit;

		/// <summary>
		/// Ƶ��
		/// </summary>
		private Frequency frequency=new Frequency();

		
		/// <summary>
		/// ��ҩ����
		/// </summary>
		private decimal herbalQty;

		/// <summary>
		/// ������λ
		/// </summary>
		private string unit;

		/// <summary>
		/// ʹ������
		/// </summary>
		private int usetimes;

		/// <summary>
		/// ִ��״̬
		/// </summary>
		private int execStatus;

		/// <summary>
		/// ��ʱҽ��ִ��ʱ��ʹ��
		/// ��ע1
		/// </summary>
		private string mark1;

		/// <summary>
		/// ��ע2
		/// </summary>
		private string mark2;

		/// <summary>
		/// ��ע3
		/// </summary>
		private string mark3;

		/// <summary>
		/// ��鲿λ��¼
		/// </summary>
		private string checkPartRecord;

		/// <summary>
		/// ���ⱸע �磺�ȵ����÷� ƽƬλ�� ҩƷ����ע������
		/// </summary>
		private string note;

		/// <summary>
		/// ������
		/// </summary>
		private string recipeNO;

		/// <summary>
		/// ������ˮ���
		/// </summary>
		private int sequenceNO;

		/// <summary>
		/// �ͼ�����
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject sample = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ������Ŀ������Ϣ ID ������Ŀ������� NAME ������Ŀ��������
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject package = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ����ҽ��
		/// </summary>
		private NeuObject reciptDoctor = new NeuObject();

		/// <summary>
		/// <br>�Ӽ�</br>
		/// </summary>
		private bool isEmergency;

		/// <summary>
		/// �Ƿ񸽲�
		/// </summary>
		private bool isSubtbl;

		/// <summary>
		/// �Ƿ��������
		/// </summary>
		private bool isHaveSubtbl;

		/// <summary>
		/// �Ƿ��ҩ�����
		/// </summary>
		private bool isStock;

		/// <summary>
		/// �Ƿ���ҽ������ͬ����ҩ
		/// </summary>
		private bool isPermission;

        /// <summary>
        /// ����Ƶ�Σ�ִ�м���
        /// ִ��ʱ�����Ƶ��ִ��ʱ��������д���
        /// </summary>
        private string execDose = "";

        /// <summary>
        /// ��Һ��Ϣ
        /// </summary>
        private Compound compound = new Compound();

        //{E1902932-1839-4a92-8A6A-E42F448FA27F}
        /// <summary>
        /// ���뵥��
        /// </summary>
        private string applyNo;

		#endregion

		#region ��������
		/// <summary>
		/// ��������
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject CreateDept=new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ִ�п���
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject ExecDept=new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ҩ������
		/// </summary>
		protected Neusoft.FrameWork.Models.NeuObject DrugDept=new Neusoft.FrameWork.Models.NeuObject();
		/// <summary>
		/// ҽ��������ţ�ҽ������ͨ����ק����ҽ������λ�ã�
		/// </summary>
		protected int sortid;
		/// <summary>
		/// ����ҽ����־
		/// </summary>
		public bool Reorder;
		/// <summary>
		/// �Ƿ�Ӥ��
		/// </summary>
		protected bool bIsBaby;
		/// <summary>
		/// Ӥ�������Ϣ
		/// </summary>
		protected  string  strBabyNo;
		#endregion

		#endregion

		#region ���ϵ�
		/// <summary>
		/// ִ��ʱ��
		/// </summary>
		[Obsolete("��ExecOper.OperTime",true)]
		public DateTime Date_Exec;

		/// <summary>
		/// ���ηֽ�ʱ��
		/// </summary>
		[Obsolete("��CurMOTime",true)]
		public DateTime Date_CurMO;

		/// <summary>
		/// ���ηֽ�ʱ��
		/// </summary>
		private DateTime curMOTime;
		/// <summary>
		/// ִ����
		/// </summary>
		[Obsolete("��ExecOper.Oper.ID",true)]
		private Neusoft.FrameWork.Models.NeuObject User_Exec=new Neusoft.FrameWork.Models.NeuObject();
		[Obsolete("Oper.Oper.ID����",true)]
		public Neusoft.FrameWork.Models.NeuObject User_REC=new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ֹͣ��
		/// </summary>
		[Obsolete("��DCOper.Oper.ID",true)]
		public Neusoft.FrameWork.Models.NeuObject User_DC=new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ֹͣ����ʱ��
		/// </summary>
		[Obsolete("��DCOper.OperTime",true)]
		public DateTime DcOperTime;
		/// <summary>
		/// ҽ������ʱ��
		/// </summary>
		[Obsolete("��ΪMOTime",true)]
		public DateTime Date_MO;
		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		[Obsolete("��ΪBeginTime",true)]
		public DateTime Date_Bgn;

		/// <summary>
		/// ����ʱ��
		/// </summary>
		[Obsolete("��ΪEndTime",true)]
		public DateTime Date_End;
		/// <summary>
		/// �´ηֽ�ʱ��
		/// </summary>
		[Obsolete("��NextMOTime",true)]
		public DateTime Date_NexMO;

		[Obsolete("��ExtendFlag1����",true)]
		public string Mark1
		{
			get
			{
				return this.mark1;
			}
			set
			{
				this.mark1 = value;
			}
		}
		/// <summary>
		/// ��ע2
		/// </summary>
		[Obsolete("��ExtendFlag2����",true)]
		public string Mark2
		{
			get
			{
				return this.mark2;
			}
			set
			{
				this.mark2 = value;
			}
		}
		/// <summary>
		/// ��ע3
		/// </summary>
		[Obsolete("��ExtendFlag3����",true)]
		public string Mark3
		{
			get
			{
				return this.mark3;
			}set
			 {
				 this.mark3 = value;
			 }
		}
		#endregion

		#region ����
        /// <summary>
        /// ����Ƶ�Σ�ִ�м���
        /// ִ��ʱ�����Ƶ��ִ��ʱ��������д���
        /// </summary>
        public string ExecDose
        {
            get
            {
                return execDose;
            }
            set
            {
                execDose = value;
            }
        }

		/// <summary>
		/// ���߻�����Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.RADT.PatientInfo Patient
		{
			get
			{
				return this.patient;
			}
			set
			{
				this.patient = value;
			}
		}
		
		/// <summary>
		/// �Ƿ���ҪƤ�ԡ��¼�20050815
		/// 1 ����ҪƤ�ԡ�������ҪƤ�ԡ��������ԡ���������
		/// </summary>
		public int HypoTest
		{
			get
			{
				return this.hypoTest;
			}
			set
			{
				this.hypoTest = value;
			}
		}
		
		/// <summary>
		/// Ժ��ע�����
		///�¼�20050815
		/// </summary>
		public int InjectCount
		{
			get
			{
				return injectCount;
			}
			set
			{
				injectCount = value;
			}
		}
		
		/// <summary>
		/// ����ҽ��
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject  Doctor
		{
			get
			{
				return this.doctor;
			}
			set
			{
				this.doctor = value;
			}
		}

		/// <summary>
		/// ���/ִ�л�ʿ
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Nurse
		{
			get
			{
				return this.nurse;
			}
			set
			{
				this.nurse = value;
			}
		}

		/// <summary>
		/// ��¼��
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment Oper
		{
			get
			{
				return this.oper;
			}
			set
			{
				this.oper = value;
			}
		}
		/// <summary>
		/// ֹͣ��
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment DCOper
		{
			get
			{
				return this.dcOper;
			}
			set
			{
				this.dcOper = value;
			}
		}

		/// <summary>
		/// ִ����
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment ExecOper
		{
			get
			{
				return this.execOper;
			}
			set
			{
				this.execOper = value;
			}
		}
		
		
		/// <summary>
		/// ֹͣԭ��
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject DcReason
		{
			get
			{
				return this.dcReason;
			}
			set
			{
				this.dcReason = value;
			}
		}

		#region ʱ��
		/// <summary>
		/// ҩƷ��Ŀ/��ҩƷ��Ŀ
		/// </summary>
		public Neusoft.HISFC.Models.Base.Item Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}
		/// <summary>
		/// ҽ������ʱ��
		/// </summary>
		public DateTime MOTime
		{
			get
			{
				return this.dtMOTime;
			}
			set
			{
				this.dtMOTime = value;
			}
		}

		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		public DateTime BeginTime
		{
			get
			{
				return this.beginTime;
			}
			set
			{
				this.beginTime = value;
			}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public DateTime EndTime
		{
			get
			{
				return this.endTime;
			}
			set
			{
				this.endTime = value;
			}
		}
		
		/// <summary>
		/// ���ηֽ�ʱ��
		/// </summary>
		public DateTime CurMOTime
		{
			get
			{
				return this.curMOTime;
			}
			set
			{
				this.curMOTime = value;
			}
		}

		/// <summary>
		/// �´ηֽ�ʱ��
		/// </summary>
		public DateTime NextMOTime
		{
			get
			{
				return this.nextMOTime;
			}
			set
			{
				this.nextMOTime = value;
			}
		}

		/// <summary>
		/// ���ʱ��
		/// </summary>
		public DateTime ConfirmTime
		{
			get
			{
				return this.confirmTime;
			}
			set
			{
				this.confirmTime = value;
			}
		}

		#endregion

		/// <summary>
		/// ״̬��0������ 1����� 2 ִ�� 3 ���ϣ�
		/// </summary>
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		/// <summary>
		/// �����Ϣ
		/// </summary>
		public Combo Combo
		{
			get
			{
				return this.combo;
			}
			set
			{
				this.combo = value;
			}
		}

		/// <summary>
		/// �÷�
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Usage
		{
			get
			{
				return this.usage;
			}
			set
			{
				this.usage = value;
			}
		}

		/// <summary>
		/// ÿ�μ���
		/// </summary>
		public decimal DoseOnce
		{
			get
			{
				return this.doseOnce;
			}
			set
			{
				this.doseOnce = value;
			}
		}

		/// <summary>
		/// ÿ�μ�����λ
		/// </summary>
		public string DoseUnit
		{
			get
			{
				return this.doseUnit;
			}
			set
			{
				this.doseUnit = value;
			}
		}

		/// <summary>
		/// Ƶ��
		/// </summary>
		public Frequency Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}

		/// <summary>
		/// ����
		/// �շ�ʱ���ǲ�ҩ�� ==����*����
		/// </summary>
		public decimal Qty
		{
			get
			{
				return this.Item.Qty;
			}
			set
			{
				this.Item.Qty = value;
			}
		}

		/// <summary>
		/// ��ҩ����
		/// </summary>
		public decimal HerbalQty
		{
			get
			{
				return this.herbalQty;
			}
			set
			{
				this.herbalQty = value;
			}
		}

		/// <summary>
		/// ������λ
		/// </summary>
		public string Unit
		{
			get
			{
				return this.unit;
			}set
			 {
				 this.unit = value;
			 }
		}

		/// <summary>
		/// ʹ������
		/// </summary>
		public int Usetimes
		{
			get
			{
				return this.usetimes;
			}
			set
			{
				this.usetimes = value;
			}
		}

		/// <summary>
		/// ִ��״̬
		/// </summary>
		public int ExecStatus
		{
			get
			{
				return this.execStatus;
			}set
			 {
				 this.execStatus = value;
			 }
		}

		/// <summary>
		/// ��ʱҽ��ִ��ʱ��ʹ��
		/// </summary>
		public string ExtendFlag1
		{
			get
			{
				return this.mark1;
			}
			set
			{
				this.mark1 = value;
			}
		}

		/// <summary>
		/// ��ע2
		/// </summary>
		public string ExtendFlag2
		{
			get
			{
				return this.mark2;
			}
			set
			{
				this.mark2 = value;
			}
		}

		/// <summary>
		/// ��ע3
		/// </summary>
		public string ExtendFlag3
		{
			get
			{
				return this.mark3;
			}set
			 {
				 this.mark3 = value;
			 }
		}

		/// <summary>
		/// ��鲿λ��¼
		/// </summary>
		public string CheckPartRecord
		{
			get
			{
				return this.checkPartRecord;
			}
			set
			{
				this.checkPartRecord = value;
			}
		}

		/// <summary>
		/// ���ⱸע �磺�ȵ����÷� ƽƬλ�� ҩƷ����ע������
		/// </summary>
		public string Note
		{
			get
			{
				return this.note;
			}
			set
			{
				this.note = value;
			}
		}

		/// <summary>
		/// ������
		/// </summary>
		public string ReciptNO
		{
			get
			{
				return this.recipeNO;
			}
			set
			{
				this.recipeNO = value;
			}
		}

		/// <summary>
		/// ������ˮ���
		/// </summary>
		public int SequenceNO
		{
			get
			{
				return this.sequenceNO;
			}
			set
			{
				this.sequenceNO = value;
			}
		}

		/// <summary>
		/// �ͼ�����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Sample 
		{
			get
			{
				return this.sample;
			}
			set
			{
				this.sample = value;
			}
		}

		/// <summary>
		/// ������Ŀ������Ϣ ID ������Ŀ������� NAME ������Ŀ��������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Package 
		{
			get
			{
				return this.package;
			}
			set
			{
				this.package = value;
			}
		}

		/// <summary>
		/// ����ҽ��
		/// </summary>
		public NeuObject ReciptDoctor 
		{
			get
			{
				return this.reciptDoctor;
			}
			set
			{
				this.reciptDoctor = value;
			}
		}

		#region ��־

		/// <summary>
		/// <br>�Ӽ�</br>
		/// </summary>
		public bool IsEmergency
		{
			get
			{
				return this.isEmergency;
			}
			set
			{
				this.isEmergency = value;
			}
		}
		/// <summary>
		/// �Ƿ񸽲�
		/// </summary>
		public bool IsSubtbl
		{
			get
			{
				return this.isSubtbl;
			}
			set
			{
				this.isSubtbl = value;
			}
		}
		/// <summary>
		/// �Ƿ��������
		/// </summary>
		public bool IsHaveSubtbl
		{
			get
			{
				return this.isHaveSubtbl;
			}
			set
			{
				this.isHaveSubtbl = value;
			}
		}
		/// <summary>
		/// �Ƿ��ҩ�����
		/// </summary>
		public bool IsStock
		{
			get
			{
				return this.isStock;
			}
			set
			{
				this.isStock = value;
			}
		}
		/// <summary>
		/// �Ƿ���ҽ������ͬ����ҩ
		/// </summary>
		public bool IsPermission
		{
			get
			{
				return this.isPermission;
			}
			set
			{
				this.isPermission = value;
			}
		}

		#endregion

        /// <summary>
        /// �Ƿ���Һ
        /// </summary>
        public Compound Compound
        {
            get
            {
                return this.compound;
            }
            set
            {
                this.compound = value;
            }
        }


        //{E1902932-1839-4a92-8A6A-E42F448FA27F}
        /// <summary>
        /// ���뵥��
        /// </summary>
        public string ApplyNo
        {
            get { return applyNo; }
            set { applyNo = value; }
        }


		#endregion

		#region �ӿ�ʵ��
		
		#region IDept ��Ա
		/// <summary>
		/// ������Ժ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject InDept
		{
			get
			{
				// TODO:  ��� Order.InDept getter ʵ��
				return this.Patient.PVisit.PatientLocation.Dept;
			}
			set
			{
				// TODO:  ��� Order.InDept setter ʵ��
				this.Patient.PVisit.PatientLocation.Dept = value;
			}
		}

		/// <summary>
		/// ִ�п���
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject ExeDept
		{
			get
			{
				// TODO:  ��� Order.ExeDept getter ʵ��
				return this.ExecDept ;
			}
			set
			{
				// TODO:  ��� Order.ExeDept setter ʵ��
				this.ExecDept =value;
			}
		}

		/// <summary>
		/// ��������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject ReciptDept
		{
			get
			{
				// TODO:  ��� Order.ReciptDept getter ʵ��
				return this.CreateDept;
			}
			set
			{
				// TODO:  ��� Order.ReciptDept setter ʵ��
				this.CreateDept =value;
			}
		}

		/// <summary>
		/// ����ִ�л�ʿվ
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject NurseStation
		{
			get
			{
				// TODO:  ��� Order.NurseStation getter ʵ��
				return this.Patient.PVisit.PatientLocation.NurseCell;
			}
			set
			{
				// TODO:  ��� Order.NurseStation setter ʵ��
				this.Patient.PVisit.PatientLocation.NurseCell=value;
			}
		}

		/// <summary>
		/// �ۿ����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject StockDept
		{
			get
			{
				// TODO:  ��� Order.StockDept getter ʵ��
				return this.DrugDept ;
			}
			set
			{
				// TODO:  ��� Order.StockDept setter ʵ��
				this.DrugDept = value;
			}
		}

		/// <summary>
		/// ����ҽ�����ڿ���
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject DoctorDept
		{
			get
			{
				// TODO:  ��� Order.ReciptDoct getter ʵ��
				return this.doctorDept;
			}
			set
			{
				// TODO:  ��� Order.ReciptDoct setter ʵ��
				this.doctorDept =value;
			}
		}

		#endregion

		#region IBaby ��Ա
		/// <summary>
		/// Ӥ�����
		/// </summary>
		public string BabyNO
		{
			get
			{
				// TODO:  ��� Order.Neusoft.HISFC.Models.Base.IBaby.BabyNo getter ʵ��
				if(strBabyNo ==null) this.strBabyNo="0";
				return this.strBabyNo;
			}
			set
			{
				// TODO:  ��� Order.Neusoft.HISFC.Models.Base.IBaby.BabyNo setter ʵ��
				strBabyNo = value;
			}
		}

		/// <summary>
		/// �Ƿ�Ӥ��
		/// </summary>
		public bool IsBaby
		{
			get
			{
				// TODO:  ��� Order.Neusoft.HISFC.Models.Base.IBaby.IsBaby getter ʵ��
				return this.bIsBaby;
			}
			set
			{
				// TODO:  ��� Order.Neusoft.HISFC.Models.Base.IBaby.IsBaby setter ʵ��
				this.bIsBaby = value;
			}
		}

		#endregion

		#region ISort ��Ա

		public int SortID
		{
			get
			{
				// TODO:  ��� Order.SortID getter ʵ��
				return this.sortid ;
			}
			set
			{
				// TODO:  ��� Order.SortID setter ʵ��
				this.sortid =value;
			}
		}

		#endregion

		#endregion

		#region ����

		#region ��¡

		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns></returns>
		public new Order Clone()
		{
			// TODO:  ��� Order.Clone ʵ��
			Order obj=base.Clone() as Order;
			obj.Combo=this.Combo.Clone();
			obj.DcReason=this.DcReason.Clone();
			
			obj.Frequency=(Frequency)this.Frequency.Clone();
			
			try{obj.ExeDept=this.ExeDept.Clone();}catch{};
			try{obj.InDept=this.InDept.Clone();}catch{};
			try{obj.NurseStation=this.NurseStation.Clone();}catch{};
			try{obj.ReciptDept=this.ReciptDept.Clone();}catch{};
			try{obj.DoctorDept=this.DoctorDept.Clone();}catch{};
			try{obj.StockDept=this.StockDept.Clone();}catch{};

			obj.Item=this.Item.Clone();
			obj.Nurse=this.Nurse.Clone();
			
			try{obj.Patient=this.Patient.Clone();}catch{};
			
			obj.Usage=this.Usage.Clone();
			obj.oper=this.oper.Clone();
			obj.execOper=this.execOper.Clone();
			obj.dcOper=this.dcOper.Clone();

            obj.compound = this.compound.Clone();
			return obj;
		}


		#endregion

		#endregion
	}
}
