using System;
using System.Collections;


namespace Neusoft.HISFC.Models.Admin {


	/// <summary>
	/// ��ԱȨ����ϸ
	/// </summary>
    /// 
    [System.Serializable]
	public class UserPowerDetail: Neusoft.FrameWork.Models.NeuObject {
		private System.String pkID ;
		private Neusoft.FrameWork.Models.NeuObject dept =new Neusoft.FrameWork.Models.NeuObject();
		private Neusoft.FrameWork.Models.NeuObject user =new Neusoft.FrameWork.Models.NeuObject();
		private System.String class1Code ;
		private System.String class2Code ;
		//private System.String class3Code ;
		private System.String grantDept ;
		private System.String grantEmpl ;
		private System.Boolean grantFlag ;
		private System.String roleCode;
		//private ArrayList Privileges;

		public PowerLevelClass3 PowerLevelClass = new PowerLevelClass3();

		/// <summary>
		/// ��дID���û�����
		/// </summary>
		public new string ID {
			get{ return this.user.ID; }
			set{ this.user.ID = value; }
		}

		/// <summary>
		/// ��дName���û�����
		/// </summary>
		public new string Name {
			get{ return this.user.Name; }
			set{ this.user.Name = value; }
		}

		/// <summary>
		/// ���������
		/// </summary>
		public System.String PkID {
			get {
				return this.pkID;
			}
			set {
				this.pkID = value;
				this.ID = value;
			}
		}

		/// <summary>
		/// Ȩ�޲���
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject Dept {
			get {
				return this.dept;
			}
			set {
				this.dept = value;
			}
		}

		/// <summary>
		/// �û�����
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject User {
			get {
				return this.user;
			}
			set {
				this.user = value;
			}
		}

		/// <summary>
		/// һ��Ȩ�޷����룬Ȩ������
		/// </summary>
		public System.String Class1Code {
			get {
				return this.class1Code;
			}
			set {
				this.class1Code = value;
			}
		}

		/// <summary>
		/// ����Ȩ�޷�����
		/// </summary>
		public System.String Class2Code {
			get {
				return this.class2Code;
			}
			set {
				this.class2Code = value;
				this.PowerLevelClass.Class2Code = value;
			}
		}

		//		/// <summary>
		//		/// ����Ȩ�޷�����
		//		/// </summary>
		//		public System.String Class3Code
		//		{
		//			get
		//			{
		//				return this.class3Code;
		//			}
		//			set
		//			{
		//				this.class3Code = value;
		//			}
		//		}

		/// <summary>
		/// ��Ȩ����
		/// </summary>
		public System.String GrantDept {
			get {
				return this.grantDept;
			}
			set {
				this.grantDept = value;
			}
		}

		/// <summary>
		/// ��Ȩ��
		/// </summary>
		public System.String GrantEmpl {
			get {
				return this.grantEmpl;
			}
			set {
				this.grantEmpl = value;
			}
		}

		/// <summary>
		/// �Ƿ��������Ȩ��0��1��
		/// </summary>
		public System.Boolean GrantFlag {
			get {
				return this.grantFlag;
			}
			set {
				this.grantFlag = value;
			}
		}

		/// <summary>
		/// ��ɫ
		/// </summary>
		public System.String RoleCode {
			get {
				return this.roleCode;
			}
			set {
				this.roleCode = value;
			}
		}
	}
}