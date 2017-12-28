using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.RADT;
namespace Neusoft.HISFC.Components.Common.Controls
{
    public partial class ucPatientProperty : UserControl
    {
        public ucPatientProperty()
        {
            InitializeComponent();
        }

        /// <summary>
		/// ҳ�����ԣ����մ������Ļ�����Ϣ
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
				GetPatientProperty();
			}
		
		}
		private  PatientInfo patientInfo = new PatientInfo();
		private  Neusoft.HISFC.Models.RADT.PatientInfo patient;
        private void GetPatientProperty()
		{

			if (this.Patient != null)
			{
				patientInfo.PatientNo = Patient.PID.ID;//סԺ��
                this.patientInfo.Sex = Patient.Sex.Name;//�Ա�
                #region ������ͳһ���㷨 {9BE8D34E-752D-4d32-A37C-87C62A949C55} wbo 2010-10-23
                //this.patientInfo.Age = Patient.Age;//����
                try
                {
                    this.patientInfo.Age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.patient.Birthday);//����
                }
                catch (Exception ex)
                { }
                #endregion
                this.patientInfo.BedNo = Patient.PVisit.PatientLocation.Bed.Name;//����
				this.patientInfo.InDept = Patient.PVisit.PatientLocation.Dept.Name;//����
				this.patientInfo.PatientName = Patient.Name;//����
				//				this.patientInfo.Pact = Patient.Patient.Pact.Name;//��ͬ��λ
				this.patientInfo.Indate = Patient.PVisit.InTime.ToShortDateString();//סԺ����
				//				this.patientInfo.Paykind = Patient.PayKind.Name;
				//			
				//			this.patientInfo. = Patient.PVisit.In_State.Name;//��Ժ״̬
				this.patientInfo.TotCost = Patient.FT.TotCost.ToString("0.00");//�����ܼ�
                this.patientInfo.PrepayCost = Patient.FT.PrepayCost.ToString("0.00");//Ԥ����
				//				this.patientInfo.BalanceCost = Patient.Fee.Balance_Cost.ToString("0.00");//�ѽ�
                this.patientInfo.LeftCost = Patient.FT.LeftCost.ToString("0.00");//����
				//			this.patientInfo. = Patient.Patient.ClinicDiagnose ;//�������

				if(this.Patient.PVisit.OutTime != DateTime.MinValue)
				{
                    this.patientInfo.OutDate = Patient.PVisit.OutTime.ToShortDateString();//��Ժ����
				}
				else
				{
					this.patientInfo.OutDate = "δ��Ժ";
				}
				//				this.patientInfo.BursaryTotMedFee = Patient.Fee.BursaryTotMedFee.ToString();//����ҩ
                this.patientInfo.DayLimit = Patient.FT.DayLimitCost.ToString();//ҩ�� �����޶�
				this.patientInfo.LimitTot = Patient.FT.DayLimitTotCost.ToString();//���޶��ۼ�
			
				decimal Pub = 0;
				decimal Own = 0;
				Pub = Patient.FT.PubCost;
				Own = Patient.FT.OwnCost + Patient.FT.PayCost;
				//				this.patientInfo.PubCost = Pub.ToString("0.00");//�ܼ���
				this.patientInfo.OwnCost = Own.ToString("0.00"); 
				this.patientInfo.PayCost = Patient.FT.PayCost.ToString("0.00");
				//				this.patientInfo.Available = (Patient.Fee.Left_Cost - Patient.PVisit.MoneyAlert).ToString();
				//				this.patientInfo.Caution = Patient.Caution.Name;
				//				this.patientInfo.AdmittingDoctor = Patient.PVisit.AdmittingDoctor.Name;
				//				this.patientInfo.AdmittingNurse = Patient.PVisit.AdmittingNurse.Name;
				//				this.patientInfo.AttendingDoctor = Patient.PVisit.AttendingDoctor.Name;
				this.patientInfo.InState = Patient.PVisit.InState.Name;
				//			this.patientInfo = Patient.Memo;

			}

			this.propertyGrid1.SelectedObject = patientInfo;
//			propertyGrid1.SelectedObjects = new object[]{patientInfo,Patient.Patient,Patient.PayKind,Patient.PVisit,Patient.SIMainInfo,Patient.Caution,Patient.Diagnoses,Patient.Disease};
		}
	}


	#region ���������
	#region ����Ҫ����PropertyGird�еĶ���Ļ���.

	class IBaseProperty : ICustomTypeDescriptor
	{
		private PropertyDescriptorCollection globalizedProps;

		public String GetClassName()
		{
			return TypeDescriptor.GetClassName(this,true);
		}

		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this,true);
		}

		public String GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		public EventDescriptor GetDefaultEvent() 
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		public PropertyDescriptor GetDefaultProperty() 
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		public object GetEditor(Type editorBaseType) 
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes) 
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			if ( globalizedProps == null) 
			{
				PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this, attributes, true);

				globalizedProps = new PropertyDescriptorCollection(null);

				foreach( PropertyDescriptor oProp in baseProps )
				{
					globalizedProps.Add(new BasePropertyDescriptor (oProp));
				}
			}
			return globalizedProps;
		}

		public PropertyDescriptorCollection GetProperties()
		{
			if ( globalizedProps == null) 
			{
				PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(this, true);
				globalizedProps = new PropertyDescriptorCollection(null);

				foreach( PropertyDescriptor oProp in baseProps )
				{
					globalizedProps.Add(new BasePropertyDescriptor(oProp));
				}
			}
			return globalizedProps;
		}

		public object GetPropertyOwner(PropertyDescriptor pd) 
		{
			return this;
		}
	}
	#endregion

	#region ����Ҫ����PropertyGird�еĶ������������д

	class BasePropertyDescriptor : PropertyDescriptor
	{
		private PropertyDescriptor basePropertyDescriptor; 
  
		public BasePropertyDescriptor(PropertyDescriptor basePropertyDescriptor) : base(basePropertyDescriptor)
		{
			this.basePropertyDescriptor = basePropertyDescriptor;
		}

		public override bool CanResetValue(object component)
		{
			return basePropertyDescriptor.CanResetValue(component);
		}

		public override Type ComponentType
		{
			get { return basePropertyDescriptor.ComponentType; }
		}

		public override string DisplayName
		{
			get 
			{
				string svalue  = "";
				foreach(Attribute attribute in this.basePropertyDescriptor.Attributes)
				{
					if (attribute is showChinese)
					{
						svalue = attribute.ToString();
						break;
					}
				}
				if (svalue == "") return this.basePropertyDescriptor.Name;
				else return svalue;
			}
		}

		public override string Description
		{
			get
			{
				return this.basePropertyDescriptor.Description;
			}
		}

		public override object GetValue(object component)
		{
			return this.basePropertyDescriptor.GetValue(component);
		}

		public override bool IsReadOnly
		{
			get { return this.basePropertyDescriptor.IsReadOnly; }
		}

		public override string Name
		{
			get { return this.basePropertyDescriptor.Name; }
		}

		public override Type PropertyType
		{
			get { return this.basePropertyDescriptor.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			this.basePropertyDescriptor.ResetValue(component);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return this.basePropertyDescriptor.ShouldSerializeValue(component);
		}

		public override void SetValue(object component, object value)
		{
			this.basePropertyDescriptor.SetValue(component, value);
		}
	}
	#endregion


	#region �Զ�������������ʾ��ıߵĺ���
	[AttributeUsage(AttributeTargets.Property)]
	class showChinese : System.Attribute
	{
		private string sChineseChar = "";

		public showChinese(string sChineseChar)
		{
			this.sChineseChar = sChineseChar;
		}

		public string ChineseChar
		{
			get
			{
				return this.sChineseChar;
			}
		}

		public override string ToString()
		{
			return this.sChineseChar;
		}
	}
	#endregion

	#endregion

	#region ����������
	/// <summary>
	/// ������ʾ��������
	/// </summary> 
	 class PatientInfo:IBaseProperty
	{
		#region ���߻�����Ϣ
		private string  Patientno = null; //����סԺ��
		private string  Patienname = null;//��������
		//            private string  pPaykind = null;   //֧����ʽ
		private string  pSex = null;//�����Ա�
		//			private string  pBirthdate = null;//��������
		private string  pAge = null;//��������
		//			private string  pPact = null; //��ͬ��λ
		private string  pIndate = null;//סԺ����
		private string  pDate_Out = null;//��Ժ����
		private string  pInstate = null;//��Ժ״̬
			
		[DescriptionAttribute("����סԺ�š�"),showChinese("A.סԺ��"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string  PatientNo
		{
			get { return Patientno; }
			set { Patientno = value;}
		}

		[DescriptionAttribute("����������"),showChinese("B.����"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string PatientName
		{
			get { return Patienname; }
			set { Patienname = value; }
		}

		[DescriptionAttribute("�����Ա�"),showChinese("C.�Ա�"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string Sex
		{
			get { return pSex; }
			set { pSex = value; }
		}

		//			[DescriptionAttribute("�������ա�"),showChinese("��������:"),CategoryAttribute("���߻�����Ϣ"),ReadOnlyAttribute(false)]
		//			public string BirthDate
		//			{
		//				get { return pBirthdate; }
		//				set { pBirthdate = value; }
		//			}
		[DescriptionAttribute("�������䡣"),showChinese("D.����"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string Age
		{
			get { return pAge; }
			set { pAge = value; }
		}

		

		[DescriptionAttribute("��Ժ���ڡ�"),showChinese("E.��Ժ����"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string Indate
		{
			get { return pIndate; }
			set { pIndate = value; }
		}
		[DescriptionAttribute("��Ժ״̬��"),showChinese("F.״̬"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string InState
		{
			get { return pInstate; }
			set { pInstate = value; }
		}

		[DescriptionAttribute("��Ժ���ڡ�"),showChinese("G.��Ժ����"),CategoryAttribute("1.���߻�����Ϣ"),ReadOnlyAttribute(false)]
		public string OutDate
		{
			get { return pDate_Out; }
			set { pDate_Out = value; }
		}

		//			[DescriptionAttribute("���㷽ʽ��"),showChinese("���㷽ʽ:"),CategoryAttribute("���߻�����Ϣ"),ReadOnlyAttribute(false)]
		//			public string Pact
		//			{
		//				get { return pPact; }
		//				set { pPact = value; }
		//			}
		//			[DescriptionAttribute("֧����ʽ��"),showChinese("֧����ʽ:"),CategoryAttribute("���߻�����Ϣ"),ReadOnlyAttribute(false)]
		//			public string Paykind
		//			{
		//				get { return pPaykind; }
		//				set { pPaykind = value; }
		//			}

		#endregion

		#region ��Ժ��Ϣ
		private string  pIndept = null;//��Ժ����
		private string  pBedno  = null;//����
		//			private string  pAttendingDoctor = null;//����ҽʦ
		//			private string  pAdmittingDoctor = null;//����ҽʦ��סԺҽʦ
		//			private string  pAdmittingNurse = null;//���λ�ʿ
    

		[DescriptionAttribute("��Ժ���ҡ�"),showChinese("H.��Ժ����"),CategoryAttribute("2.סԺ��Ϣ"),ReadOnlyAttribute(false)]
		public string  InDept
		{
			get { return pIndept; }
			set { pIndept = value;}
		}

		[DescriptionAttribute("���ߴ��š�"),showChinese("I.���ߴ���"),CategoryAttribute("2.סԺ��Ϣ"),ReadOnlyAttribute(false)]
		public string  BedNo
		{
			get { return pBedno; }
			set { pBedno = value;}
		}
		//			[DescriptionAttribute("����ҽʦ��"),showChinese("����ҽʦ:"),CategoryAttribute("סԺ��Ϣ"),ReadOnlyAttribute(false)]
		//			public string  AttendingDoctor
		//			{
		//				get { return pAttendingDoctor; }
		//				set { pAttendingDoctor = value;}
		//			}
		//			[DescriptionAttribute("����ҽʦ��סԺҽʦ��"),showChinese("����ҽʦ:"),CategoryAttribute("סԺ��Ϣ"),ReadOnlyAttribute(false)]
		//			public string  AdmittingDoctor
		//			{
		//				get { return pAdmittingDoctor; }
		//				set { pAdmittingDoctor = value;}
		//			}
		//			[DescriptionAttribute("���λ�ʿ��"),showChinese("���λ�ʿ:"),CategoryAttribute("סԺ��Ϣ"),ReadOnlyAttribute(false)]
		//			public string  AdmittingNurse
		//			{
		//				get { return pAdmittingNurse; }
		//				set { pAdmittingNurse = value;}
		//			}
		

		#endregion

		#region ���߷�����Ϣ
		//			private string pCaution = null ;//�������
		private string pTot_Cost = null;//�ܷ���
		private string pPrepay_Cost = null ;// Ԥ����
		//            private string pBalance_Cost = null ;//�ѽ�
		private string pLeft_Cost = null ;//����
		//            private string pBursaryTotMedFee = null;//����ҩ
		private string pDay_Limit = null;//���޶�
		private string pLimitTot = null;//���޶��ۼ�
           
		//            private string pPub_Cost = null;//�ܼ���
		private string pOwn_Cost = null;//�Է�
		private string pPay_Cost = null;//�����Ը����
		//			private string pAvailable = null;//���ý��


		

		[DescriptionAttribute("Ԥ����"),showChinese("J.Ԥ����"),CategoryAttribute("3.���߷�����Ϣ"),ReadOnlyAttribute(false)]
		public string  PrepayCost
		{
			get { return pPrepay_Cost; }
			set { pPrepay_Cost = value;}
		}
		[DescriptionAttribute("�Էѡ�"),showChinese("K.�Է�"),CategoryAttribute("3.���߷�����Ϣ"),ReadOnlyAttribute(false)]
		public string  OwnCost
		{
			get { return pOwn_Cost; }
			set { pOwn_Cost = value;}
		}

		[DescriptionAttribute("�ܷ��á�"),showChinese("L.�ܷ���"),CategoryAttribute("3.���߷�����Ϣ"),ReadOnlyAttribute(false)]
		public string  TotCost
		{
			get { return pTot_Cost; }
			set { pTot_Cost = value;}
		}



		//			[DescriptionAttribute("�ѽᡣ"),showChinese("�ѽ�"),CategoryAttribute("3.���߷�����Ϣ"),ReadOnlyAttribute(false)]
		//			public string  BalanceCost
		//			{
		//				get { return pBalance_Cost; }
		//				set { pBalance_Cost = value;}
		//			}

		[DescriptionAttribute("��"),showChinese("M.���"),CategoryAttribute("3.���߷�����Ϣ"),ReadOnlyAttribute(false)]
		public string  LeftCost
		{
			get { return pLeft_Cost; }
			set { pLeft_Cost = value;}
		}

		#endregion

		#region ������Ϣ
		
		[DescriptionAttribute("���޶"),showChinese("N.���޶�"),CategoryAttribute("4.������Ϣ"),ReadOnlyAttribute(false)]
		public string  DayLimit
		{
			get { return pDay_Limit; }
			set { pDay_Limit = value;}
		}
		[DescriptionAttribute("���޶��ۼơ�"),showChinese("O.���޶��ۼ�"),CategoryAttribute("4.������Ϣ"),ReadOnlyAttribute(false)]
		public string  LimitTot
		{
			get { return pLimitTot; }
			set { pLimitTot = value;}
		}
			
		[DescriptionAttribute("�����Ը���"),showChinese("P.�����Ը����"),CategoryAttribute("4.������Ϣ"),ReadOnlyAttribute(false)]
		public string  PayCost
		{
			get { return pPay_Cost; }
			set { pPay_Cost = value;}
		}
	
		#endregion

	 #endregion
	}	


	}
