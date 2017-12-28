using System;
using System.Collections;
using Neusoft.FrameWork.Function;

namespace Neusoft.HISFC.BizLogic.Order
{
	/// <summary>
	/// ��������
	/// written by zuowy 
	/// 2005-8-20
	/// </summary>
	public class PacsBill:Neusoft.FrameWork.Management.Database 
	{
		public PacsBill()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}	


		#region ��ɾ��

		#endregion

		#region ����
		/// <summary>
		/// 
		/// </summary>
		/// <param name="PacsBill"></param>
		/// <returns></returns>
		public int SavePacsBill(Neusoft.HISFC.Models.Order.PacsBill PacsBill)
		{
			return 0;
		}

		/// <summary>
		/// �����µļ�鵥
		/// </summary>
		/// <param name="PacsBill"></param>
		/// <returns></returns>
		public int InsertPacsBill( Neusoft.HISFC.Models.Order.PacsBill PacsBill ) 
		{
			// �����µļ�鵥
			// Management.Order.InsertPacsBill
			// ���� 12 ���� 0
			string strSql = "";
			if(this.Sql.GetSql("Management.Order.InsertPacsBill",ref strSql) == -1) return -1;
			strSql = this.GetPacsBillInfo(strSql,PacsBill);

			if(strSql == null) 
			{
				this.Err = "��ʽ��Sql���ʱ����";
				this.WriteErr();
				return -1;	 
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ���¼�鵥��Ϣ
		/// </summary>
		/// <param name="pacsbill"></param>
		/// <returns></returns>
		public int UpdatePacsBill(Neusoft.HISFC.Models.Order.PacsBill pacsbill) 
		{			
			// ���¼�鵥
			// Management.Order.UpdatePacsBill
			// ���� 12 ���� 0
			string strSql = "";
			if(this.Sql.GetSql("Management.Order.UpdatePacsBill",ref strSql) == -1) return -1;
			strSql = this.GetPacsBillInfo(strSql,pacsbill);

			if(strSql == null) 
			{ 
				this.Err = "��ʽ��Sql���ʱ����";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ɾ��һ����¼
		/// </summary>
		/// <param name="PacsID"></param>
		/// <returns></returns>
		public int DeletePacsBill(string PacsID) 
		{
			string strSql = "";
			if(this.Sql.GetSql("Management.Order.deletePacsBill",ref strSql) == -1)  
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			strSql = string.Format(strSql,PacsID);
			if(strSql == null)
				return -1;
			return this.ExecNoQuery(strSql);
		}
		#endregion

		#region ����
		/// <summary>
		/// ����ʱ���ж�
		/// </summary>
		/// <param name="pacsbill"></param>
		public int SetPacsBill(Neusoft.HISFC.Models.Order.PacsBill pacsbill) 
		{
			int Parm;
			Parm = this.UpdatePacsBill(pacsbill);
			if(Parm == 0)
				Parm = this.SavePacsBill(pacsbill);
			return Parm;
		}
		/// <summary>
		/// ��ѯ��鵥��Ϣ
		/// </summary>
		/// <param name="combNo"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Order.PacsBill  QueryPacsBill(string combNo) {
			# region ��ѯ��鵥��Ϣ
			// ��ѯ��鵥��Ϣ
			// Management.Order.SelectPacsBill
			// ���� 1 ���� 11
			# endregion
			string strSql = "";
			ArrayList al = null;
			if(this.Sql.GetSql("Management.Order.QueryResourceByPacsBillNo",ref strSql) == -1) { 
				this.Err="û���ҵ�Management.Order.QueryResourceByPacsBillNo�ֶ�!";
				return null;	 
			}
			strSql = string.Format(strSql,combNo);
			al = this.myPacsBillQuery(strSql);
			if(al == null || al.Count == 0) return null;
			return al[0] as Neusoft.HISFC.Models.Order.PacsBill;
		}
		
		/// <summary>
		/// ��ü�鵥��Ϣ
		/// </summary>
		/// <param name="pacsbill"></param>
		/// <returns></returns>
		public string  GetPacsBillInfo(string strSql,Neusoft.HISFC.Models.Order.PacsBill pacsbill) {
			# region "�ӿ�˵��"
			// 0 ��鵥��       1 ��鵥����      2 סԺ��ˮ�� 3 ���ұ��� 
			// 4 ��������       5 ��鲿λ/Ŀ��   6 ��ʷ������
			// 7 ʵ���Ҽ���� 8 ע������        9 ��� 10 ��ע
			// 11 ����Ա        12 ��������
			# endregion
			try{
				System.Object[] s = {pacsbill.ComboNO,//��鵥��
										pacsbill.BillName,//��鵥����
										pacsbill.PatientNO,//סԺ��ˮ��
										pacsbill.Dept.ID,//���Ҵ���
										pacsbill.Dept.Name,//��������
										pacsbill.CheckOrder,//��鲿λ/Ŀ��
										pacsbill.IllHistory,//��ʷ��鼰����
										pacsbill.LisResult,//�����
										pacsbill.Caution,//ע������
										pacsbill.DiagName,//���
										pacsbill.Memo,//��ע
										pacsbill.Oper.ID,//����Ա
										pacsbill.Oper.OperTime,//��������
					                    pacsbill.User01,//����ҽʦ����
					                    pacsbill.User02,//����ҽʦ����
					                    pacsbill.User03,//��Ŀ�۸�
					                    pacsbill.PatientNO.Substring(pacsbill.PatientNO.Length - 7)//�ӿ�
									};
				string myErr = "";
				if(Neusoft.FrameWork.Public.String.CheckObject(out myErr,s) == -1) {
					this.Err = myErr;
					this.WriteErr();
					return null;	 
				}
				strSql = string.Format(strSql,s);
			}
			catch(Exception ex){
				this.Err="����ֵʱ�����"+ex.Message;
				this.WriteErr();
				return null;
			}
			return  strSql;
		}

		#endregion

		#region ˽��
		/// <summary>
		/// ��ü�鵥��Ϣʵ��
		/// </summary>
		/// <param name="strSql"></param>
		/// <returns></returns>
		private ArrayList  myPacsBillQuery(string strSql) 
		{
			ArrayList al = new ArrayList();

			if(this.ExecQuery(strSql) == -1) return null;
			Neusoft.HISFC.Models.Order.PacsBill pacsbill = new Neusoft.HISFC.Models.Order.PacsBill();
			try{
				while(this.Reader.Read()) {
					try {
						pacsbill.ComboNO = this.Reader[0].ToString();//��鵥��
						pacsbill.BillName = this.Reader[1].ToString();//��鵥����
						pacsbill.PatientNO = this.Reader[2].ToString();//סԺ��ˮ��
						pacsbill.Dept.Name = this.Reader[3].ToString();//��������
						pacsbill.CheckOrder = this.Reader[4].ToString();//��鲿λ/Ŀ��
						pacsbill.IllHistory = this.Reader[5].ToString();//��ʷ��鼰����
						pacsbill.LisResult = this.Reader[6].ToString();//�����
						pacsbill.Caution = this.Reader[7].ToString();//ע������
						pacsbill.DiagName = this.Reader[8].ToString();//���
						pacsbill.Memo = this.Reader[9].ToString();//��ע
						pacsbill.Oper.ID = this.Reader[10].ToString();//����Ա
						pacsbill.Oper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[11].ToString());//��������
					}
					catch(Exception ex) {
						this.Err="��ü�鵥��Ϣ����"+ex.Message;
						this.WriteErr();
						return null;	   
					}
					al.Add(pacsbill);
				}
			}
			catch(Exception ex){
				this.Err="��ü�鵥��Ϣ����"+ex.Message;
				this.WriteErr();
				return null;
			}
			this.Reader.Close();
			return al;
		}
		#endregion
	}
}
