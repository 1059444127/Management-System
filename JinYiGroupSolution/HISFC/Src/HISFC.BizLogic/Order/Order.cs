using System;
using Neusoft.HISFC.Models;
using System.Collections;
using Neusoft.FrameWork.Models;
using System.Data;
namespace Neusoft.HISFC.BizLogic.Order
{
	/// <summary>
	/// ҽ�������ࡣ
    /// 
    /// <˵��>
    ///     1 2007-04 ������Һ���Ĵ���
    ///         ִ�е����뺯�����Ӳ����Ƿ�����Һ
    ///     2 ���Ӳ�ѯ�����º���
    /// </˵��>
	/// </summary>
	public class Order:Neusoft.FrameWork.Management.Database 
	{
		public Order()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
        }

        #region ��̬��

        /// <summary>
        /// ����Һ�÷�
        /// </summary>
        private static System.Collections.Hashtable hsCompoundUsage = null;

        #endregion

        #region ����
        /// <summary>
		/// 
		/// </summary>
        /// <param name="order"></param>
		/// <returns></returns>
		[Obsolete("��InsertOrder���棡",true)]
		public int CreateOrder(Neusoft.HISFC.Models.Order.Inpatient.Order  order)
		{
			return this.InsertOrder(order);
		}


		[Obsolete("QueryOrderSubtbl������",true)]
		public ArrayList QueryOrderSub(string InPatientNo)
		{
			return this.QueryOrderSubtbl(InPatientNo);
		}
        [Obsolete("��InsertExecOrder������",true)]
        public int CreateExec(Neusoft.HISFC.Models.Order.ExecOrder ExecOrder)
		{
			return this.InsertExecOrder(ExecOrder);
		}

		[Obsolete("UpdateRecordExec����",true)]
        public int RecordExec(Neusoft.HISFC.Models.Order.ExecOrder ExecOrder)
		{
			return this.UpdateRecordExec(ExecOrder);
		}
		
		[Obsolete("��UpdateChargeExec����",true)]
        public int ChargeExec(Neusoft.HISFC.Models.Order.ExecOrder ExecOrder)
		{
			return UpdateChargeExec(ExecOrder);
		}
		[Obsolete("UpdateDrugExec����",true)]
        public int DrugExec(Neusoft.HISFC.Models.Order.ExecOrder ExecOrder)
		{
			return this.UpdateDrugExec(ExecOrder);
		}

        [Obsolete("QueryExecOrder(inpatientNo,itemType);����",true)]
		public ArrayList QueryPatientExec(string inpatientNo,string itemType)
		{
			return this.QueryExecOrder(inpatientNo,itemType);
		}
		[Obsolete("��QueryExecOrder(InPatientNo,ItemType,IsValid)����",true)]
		public ArrayList QueryValidOrder(string InPatientNo,string ItemType,bool IsValid)
		{
			return this.QueryExecOrder(InPatientNo,ItemType,IsValid);
		}
		[Obsolete("QueryExecOrder(inpatientNo,itemType,isCharge)����",true)]
		public ArrayList QueryChargeOrder(string inpatientNo,string itemType,bool isCharge)
		{
			return QueryExecOrder(inpatientNo,itemType,isCharge);
		}
		[Obsolete("QueryExecOrderByDrugFlag(InPatientNo,DateTimeBegin,DateTimeEnd,  DrugFlag)����",true)]
		public ArrayList QueryOrderDrugFlag(string InPatientNo,DateTime DateTimeBegin,DateTime DateTimeEnd, int DrugFlag)
		{
			return this.QueryExecOrderByDrugFlag(InPatientNo,DateTimeBegin,DateTimeEnd,  DrugFlag);
		}
		[Obsolete("QueryExecOrderByDrugFlag(InPatientNo,DrugFlag)����",true)]
		public ArrayList QueryOrderDrugFlag(string InPatientNo,int DrugFlag)
		{
			return this.QueryExecOrderByDrugFlag(InPatientNo,DrugFlag);
		}
		#endregion

		#region ��ɾ��
		
		/// <summary>
		/// ������ҽ��(������ҽ����¼)
		/// </summary>
		/// <param name="order"></param>
		/// <returns>0 success -1 fail</returns>
        public int InsertOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order)
		{
			#region ������ҽ��
			//������ҽ��
			//Order.Order.CreateOrder.1
			//���룺71
			//			//������0 
			#endregion

			string strSql = "";

			if(this.Sql.GetSql("Order.Order.CreateOrder.1",ref strSql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			strSql = getOrderInfo(strSql, order);
			if (strSql == null) return -1;
			
			return this.ExecNoQuery(strSql);
		}
		

		/// <summary>
		/// ����ҽ��
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public int UpdateOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order)
		{
			#region ����ҽ��
			//����ҽ��
			//Order.Order.CreateOrder.1
			//���룺71
			//������0 
			#endregion
			string strSql="";

			if(this.Sql.GetSql("Order.Order.updateOrder.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			strSql = getOrderInfo(strSql, order);
			if (strSql == null) return -1;
			return this.ExecNoQuery(strSql);
		}

		
		/// <summary>
		/// ɾ��ҽ��
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public int DeleteOrder(Neusoft.HISFC.Models.Order.Order order)
		{
		
			#region ɾ��ҽ��
			//ɾ��ҽ��(ҽ��δ��Ч״̬)
			//Order.Order.delOrder.1
			//���룺0 id
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.delOrder.1",ref strSql)==-1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,order.ID);
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.delOrder.1";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}


		/// <summary>
		/// ����ִ�е�(����ִ�е���¼)
		/// </summary>
		/// <param name="execOrder"></param>
		/// <returns>0 success -1 fail</returns>
        public int InsertExecOrder(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
		{
			#region insertִ�е�
			//����ִ�е�
			//Order.ExecOrder.CreateExec.1
			//���룺71
			//������0 
			#endregion
			string strSql = "";
			string strItemType = "";

			strItemType = JudgeItemType(execOrder.Order);
			if (strItemType == "") return -1;

			#region "ҩ��ִ�е�"
			if (strItemType == "1")
			{
				Neusoft.HISFC.Models.Pharmacy.Item objPharmacy ;
				objPharmacy = (Neusoft.HISFC.Models.Pharmacy.Item)execOrder.Order.Item;

				if(this.Sql.GetSql("Order.ExecOrder.CreateExec.Drug.1",ref strSql) == -1) return -1;

                #region ��Һ�ж�  ��� ��������Manager

                if (execOrder.Order.OrderType.IsDecompose)      //����Ҫ�ֽ��ҽ���������´���
                {
                    if (Order.hsCompoundUsage == null)
                    {
                        Order.hsCompoundUsage = new Hashtable();

                        Neusoft.HISFC.BizLogic.Manager.Constant consManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
                        consManager.SetTrans(this.Trans);

                        ArrayList alList = consManager.GetList("CompoundUsage");
                        if (alList == null)             //�����д��󷵻�
                        {
                            Order.hsCompoundUsage = new Hashtable();
                        }
                        foreach (Neusoft.HISFC.Models.Base.Const cons in alList)
                        {
                            Order.hsCompoundUsage.Add(cons.ID, null);
                        }
                    }

                    if (Order.hsCompoundUsage.ContainsKey(execOrder.Order.Usage.ID))
                    {
                        execOrder.Order.Compound.IsNeedCompound = true;
                    }
                }

                #endregion

                #region "ҩ���ӿ�˵��"
                //0 IDִ����ˮ��
				//������Ϣ����  
				//			1 סԺ��ˮ��   2סԺ������     3���߿���id      4���߻���id
				//ҽ��������Ϣ
				// ������Ŀ��Ϣ
				//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
				//	       10��Ŀ������ 11��Ŀ�������  12ҩƷ���     13ҩƷ��������  14������λ       
				//         15��С��λ     16��װ����,     17���ʹ���     18ҩƷ���  ,   19ҩƷ����
				//         20���ۼ�       21�÷�����      22�÷�����     23�÷�Ӣ����д  24Ƶ�δ���  
				//         25Ƶ������     26ÿ�μ���      27��Ŀ����     28�Ƽ۵�λ      29ʹ������			  
				// ����ҽ������
				//		   30ҽ�������� 31ҽ����ˮ��  32ҽ���Ƿ�ֽ�:1����/2��ʱ     33�Ƿ�Ʒ� 
				//		   34ҩ���Ƿ���ҩ 35��ӡִ�е�    36�Ƿ���Ҫȷ��  
				// ����ִ�����
				//		   37����ҽʦId   38����ҽʦname  39Ҫ��ִ��ʱ��  40����ʱ��     41��������
				//		   42����ʱ��     43�����˴���    44�����˴���    45���˿��Ҵ��� 46����ʱ��       
				//		   47ȡҩҩ��     48ִ�п���      49ִ�л�ʿ����  50ִ�п��Ҵ��� 51ִ��ʱ��
				//         52�ֽ�ʱ�� 
				// ����ҽ������
				//		   64�Ƿ�Ӥ����1��/2��          53�������  	  54������     55��ҩ��� 
				//		   56�Ƿ��������                 57�Ƿ���Ч      58�ۿ���     59�Ƿ�ִ�� 
				//		   60��ҩ���                     61�շѱ��      62ҽ��˵��     63��ע 
				//         65������                       66������ˮ���
                // ������Һ��Ϣ
                //        67�Ƿ�����Һ                  
				#endregion 
				try
				{
					string[] s={execOrder.ID,execOrder.Order.Patient.ID,execOrder.Order.Patient.PID.PatientNO,execOrder.Order.Patient.PVisit.PatientLocation.Dept.ID,execOrder.Order.Patient.PVisit.PatientLocation.NurseCell.ID,
								   strItemType,execOrder.Order.Item.ID,execOrder.Order.Item.Name,execOrder.Order.Item.UserCode,execOrder.Order.Item.SpellCode,
								   execOrder.Order.Item.SysClass.ID.ToString(),execOrder.Order.Item.SysClass.Name,objPharmacy.Specs,objPharmacy.BaseDose.ToString(),objPharmacy.DoseUnit,objPharmacy.MinUnit,objPharmacy.PackQty.ToString(),
								   objPharmacy.DosageForm.ID,objPharmacy.Type.ID,objPharmacy.Quality.ID.ToString(),objPharmacy.PriceCollection.RetailPrice.ToString(),
								   execOrder.Order.Usage.ID,execOrder.Order.Usage.Name,execOrder.Order.Usage.Memo,execOrder.Order.Frequency.ID,execOrder.Order.Frequency.Name,
								   execOrder.Order.DoseOnce.ToString(),execOrder.Order.Qty.ToString(),execOrder.Order.Unit,execOrder.Order.HerbalQty.ToString(),
								   execOrder.Order.OrderType.ID,execOrder.Order.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsDecompose).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsCharge).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsNeedPharmacy).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsPrint).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsConfirm).ToString(),
								   execOrder.Order.ReciptDoctor.ID,execOrder.Order.ReciptDoctor.Name,execOrder.DateUse.ToString(),execOrder.DCExecOper.OperTime.ToString(),execOrder.Order.ReciptDept.ID,
								   execOrder.Order.BeginTime.ToString(),execOrder.DCExecOper.ID,execOrder.ChargeOper.ID,execOrder.ChargeOper.Dept.ID,execOrder.ChargeOper.OperTime.ToString(),
								   execOrder.Order.StockDept.ID,execOrder.Order.ExeDept.ID,execOrder.Order.ExecOper.ID,execOrder.ExecOper.Dept.ID,execOrder.ExecOper.OperTime.ToString(),
								   execOrder.DateDeco.ToString(),
								   execOrder.Order.BabyNO.ToString(),execOrder.Order.Combo.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.Combo.IsMainDrug).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsHaveSubtbl).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsValid).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsStock).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsExec).ToString(),
								   execOrder.DrugFlag.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsCharge).ToString(),execOrder.Order.Note,execOrder.Order.Memo,Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsBaby).ToString(),
								   execOrder.Order.ReciptNO,execOrder.Order.SequenceNO.ToString(),
                                   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.Compound.IsNeedCompound).ToString() };
					strSql=string.Format(strSql,s);
				}
				catch(Exception ex)
				{
					this.Err="����ֵʱ�����"+ex.Message;
					this.WriteErr();
					return -1;
				}
			}			
				#endregion
				#region "��ҩ��ִ�е�"
			else if (strItemType== "2")
			{
				Neusoft.HISFC.Models.Fee.Item.Undrug objAssets;
				objAssets = (Neusoft.HISFC.Models.Fee.Item.Undrug)execOrder.Order.Item;

				if(this.Sql.GetSql("Order.ExecOrder.CreateExec.Undrug.1",ref strSql)==-1) return -1;	
				#region "��ҩ���ӿ�˵��"
				//0 IDִ����ˮ��
				//������Ϣ����  
				//			1 סԺ��ˮ��   2סԺ������     3���߿���id      4���߻���id
				//ҽ��������Ϣ
				// ������Ŀ��Ϣ
				//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
				//	       10��Ŀ������ 11��Ŀ�������  12���         13���ۼ�        14�÷�����   
				//         15�÷�����     16�÷�Ӣ����д  17Ƶ�δ���     18Ƶ������      19ÿ������
				//         20��Ŀ����     21�Ƽ۵�λ      22ʹ�ô���			  
				// ����ҽ������
				//		   23ҽ�������� 24ҽ����ˮ��    25ҽ���Ƿ�ֽ�:1����/2��ʱ     26�Ƿ�Ʒ� 
				//		   27ҩ���Ƿ���ҩ 28��ӡִ�е�    29�Ƿ���Ҫȷ��  
				// ����ִ�����
				//		   30����ҽʦId   31����ҽʦname  32Ҫ��ִ��ʱ��  33����ʱ��     34��������
				//		   35����ʱ��     36�����˴���    37�����˴���    38���˿��Ҵ��� 39����ʱ��       
				//		   40ȡҩҩ��     41ִ�п���      42ִ�л�ʿ����  43ִ�п��Ҵ��� 44ִ��ʱ��
				//       45�ֽ�ʱ��     46ִ�п�������
				// ����ҽ������
				//		   47�Ƿ�Ӥ����1��/2��          48�������  	  49������     50������ 
				//		   51�Ƿ񸽲�                     52�Ƿ��������  53�Ƿ���Ч     54�Ƿ�ִ�� 
				//		   55�շѱ��     56�Ӽ����      57��鲿λ����  58ҽ��˵��     59��ע 
				//         60������       61������ˮ���
				#endregion 
				try
				{
					string[] s={execOrder.ID,execOrder.Order.Patient.ID,execOrder.Order.Patient.PID.PatientNO,execOrder.Order.Patient.PVisit.PatientLocation.Dept.ID,execOrder.Order.Patient.PVisit.PatientLocation.NurseCell.ID,
								   strItemType,execOrder.Order.Item.ID,execOrder.Order.Item.Name,execOrder.Order.Item.UserCode,execOrder.Order.Item.SpellCode,
								   execOrder.Order.Item.SysClass.ID.ToString(),execOrder.Order.Item.SysClass.Name,objAssets.Specs,objAssets.Price.ToString(),execOrder.Order.Usage.ID,
								   execOrder.Order.Usage.Name,execOrder.Order.Usage.Memo,execOrder.Order.Frequency.ID,execOrder.Order.Frequency.Name,execOrder.Order.DoseOnce.ToString(),
								   execOrder.Order.Qty.ToString(),execOrder.Order.Unit,execOrder.Order.HerbalQty.ToString(),
								   execOrder.Order.OrderType.ID,execOrder.Order.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsDecompose).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsCharge).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsNeedPharmacy).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsPrint).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.OrderType.IsConfirm).ToString(),
								   execOrder.Order.ReciptDoctor.ID,execOrder.Order.ReciptDoctor.Name,execOrder.DateUse.ToString(),execOrder.DCExecOper.OperTime.ToString(),execOrder.Order.ReciptDept.ID,
								   execOrder.Order.BeginTime.ToString(),execOrder.DCExecOper.ID,execOrder.ChargeOper.ID,execOrder.ChargeOper.Dept.ID,execOrder.ChargeOper.OperTime.ToString(),
								   execOrder.Order.StockDept.ID,execOrder.Order.ExeDept.ID,execOrder.ExecOper.ID,execOrder.ExecOper.Dept.ID,execOrder.ExecOper.OperTime.ToString(),
								   execOrder.DateDeco.ToString(),execOrder.Order.ExeDept.Name,
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsBaby).ToString(),execOrder.Order.BabyNO.ToString(),execOrder.Order.Combo.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.Combo.IsMainDrug).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsSubtbl).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsHaveSubtbl).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsValid).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsExec).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsCharge).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.Order.IsEmergency).ToString(),execOrder.Order.CheckPartRecord,execOrder.Order.Note,execOrder.Order.Memo,
								   execOrder.Order.ReciptNO,execOrder.Order.SequenceNO.ToString(),execOrder.Order.Sample.Name,execOrder.IsConfirm?"1":"0"/*ȷ�ϱ��{DA77B01B-63DF-4559-B264-798E54F24ABB}*/};
					strSql=string.Format(strSql,s);
				}
				catch(Exception ex)
				{
					this.Err="����ֵʱ�����"+ex.Message;
					this.WriteErr();
					return -1;
				}
			}
			#endregion
		
			if (strSql == null) return -1;
			
			return this.ExecNoQuery(strSql);
		}

		#endregion

		#region "ҽ������"
		
		#region ҽ������
		
		/// <summary>
		/// ����ҽ�� -����Order.ID =="" or =="-1" ���µ� ���룬�����ĸ���
		/// ����ҽ��
		/// </summary>
		/// <param name="Order"></param>
		/// <returns></returns>
        public int SetOrder(Neusoft.HISFC.Models.Order.Inpatient.Order Order)
		{
			if(Order.ID =="" || Order.ID =="-1")
			{
				string s = this.GetNewOrderID();
				if(s == null || s == "-1") return -1;
				Order.ID = s;
				return this.InsertOrder(Order);
			}
			else
			{
				return this.UpdateOrder(Order);
			}
		}
		
		/// <summary>
		/// ֹͣҽ��
		/// Order.Status = 1Ԥֹͣ;Order.Status = 3ֱ��ֹͣ
		/// </summary>
		/// <param name="Order">ҽ����Ϣ</param>
		/// <returns>0 success -1 fail</returns>
        public int DcOneOrder(Neusoft.HISFC.Models.Order.Order Order)
		{
			#region ֹͣҽ��
			//ֹͣҽ��(ҽ������Ч״̬)
			//Order.Order.dcOrder.1
			//���룺0 id��1 ֹͣ��id,2ֹͣ��������3ֹͣʱ��,4ҽ��״̬ ,5ֹͣԭ����룬6ֹͣԭ������ 
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.dcOrder.1",ref strSql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				if(Order.EndTime == DateTime.MinValue)//�ж�ֹͣʱ��
					Order.EndTime = this.GetDateTimeFromSysDateTime();

				strSql=string.Format(strSql,Order.ID,Order.DCOper.ID,Order.DCOper.Name,Order.EndTime.ToString(),Order.Status.ToString(),Order.DcReason.ID,Order.DcReason.Name);
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.dcOrder.1";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
	
		/// <summary>
		/// ɾ��ҽ������
		/// </summary>
		/// <param name="ComboID"></param>
		/// <returns></returns>
		public int DeleteOrderSubtbl(string ComboID)
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.delOrder.2",ref strSql)==-1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,ComboID);
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.delOrder.2";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		
		/// <summary>
		/// '���
		///��ҽ����ϳ����ҽ��һ��ִ��
		///������usage�÷���ͬ
		///      frq  Ƶ����ͬ
		/// </summary>
		/// <param name="alOrder"></param>
		/// <returns></returns>
		public int ComboOrder(ArrayList alOrder)
		{
			string strUsage="",strFrq="";
			string strSql="";
			string strCombNo="";
			#region ���
			//���
			//Order.Order.ComboOrder.1
			//���룺0 orderid 1��Ϻ� 2�Ƿ���ҩ
			//������0 
			#endregion

			if (alOrder == null) return -1;
			strCombNo = this.GetNewOrderComboID();
			if (strCombNo == "" || strCombNo == null) 
			{
				this.Err = Neusoft.FrameWork.Management.Language.Msg("ҽ����Ϻ�Ϊ�գ�������ϣ�");
				return -1;
			}
			for (int i=0;i<alOrder.Count;i++)
			{
                Neusoft.HISFC.Models.Order.Order objOrder = new Neusoft.HISFC.Models.Order.Order();
                objOrder = (Neusoft.HISFC.Models.Order.Order)alOrder[i];
				if (i==0)
				{
					strUsage=objOrder.Usage.ID; 
					strFrq=objOrder.Frequency.ID;
				}
				if (strUsage != objOrder.Usage.ID) 
				{
                    this.Err = objOrder.Item.Name + Neusoft.FrameWork.Management.Language.Msg("�÷���һ��");
					return -1;
				}
				if (strFrq != objOrder.Frequency.ID) 
				{
                    this.Err = objOrder.Item.Name + Neusoft.FrameWork.Management.Language.Msg("Ƶ�β�һ��");
					return -1;
				}
				
				if(this.Sql.GetSql("Order.Order.ComboOrder.1",ref strSql)==-1) 
				{
					this.Err = this.Sql.Err;
					return -1;
				}
				try
				{
					strSql=string.Format(strSql,objOrder.ID,strCombNo,Neusoft.FrameWork.Function.NConvert.ToInt32(objOrder.Combo.IsMainDrug).ToString());
				}
				catch
				{
					this.Err="����������ԣ�Order.Order.ComboOrder.1";
					return -1;
				}
				if( this.ExecNoQuery( strSql ) <= 0)
				{
					return -1;
				}
			}
			return 0;
		}

		#endregion

		#region "��ѯҽ��"
		/// <summary>
		/// ��ѯ����ҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO )
		{
			#region ��ѯ����ҽ��
			//��ѯ����ҽ��
			//Order.Order.QueryOrder.1
			//���룺0 inpatientno
			//������ArrayList
			#endregion

			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if ( sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.1", ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.1�ֶ�!";
				return null;
			}
			sql = sql +" " +string.Format(sql1,inpatientNO);
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯ����ҽ���ĸ���
		/// </summary>
		/// <returns></returns>
		public ArrayList QueryOrderSubtbl( string inpatientNO )
		{
			#region ��ѯ����ҽ���ĸ���
				//��ѯ����ҽ��
				//Order.Order.QueryOrder.1
				//���룺0 inpatientno
				//������ArrayList
				#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql==null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.Sub.1",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1, inpatientNO);
			return this.myOrderQuery(sql);
		}

        /// <summary>
        /// ����ĳһ�����ߵ���Ч���� {24F859D1-3399-4950-A79D-BCCFBEEAB939}
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <returns></returns>
        public ArrayList QueryOrdeSub(string inpatientNO, string itemcode)
        {
            #region ��ѯ����ҽ��
            //��ѯ����ҽ��
            //Order.Order.QueryOrder.1
            //���룺0 inpatientno
            //������ArrayList
            #endregion

            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();
            sql = OrderQuerySelect();
            if (sql == null) return null;
            if (this.Sql.GetSql("Order.Order.QueryOrder.6", ref sql1) == -1)
            {
                this.Err = "û���ҵ�Order.Order.QueryOrder.1�ֶ�!";
                return null;
            }
            sql = sql + " " + string.Format(sql1, inpatientNO, itemcode);
            return this.myOrderQuery(sql);
        }
		/// <summary>
		/// ��״̬��ѯҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO, int status )
		{
			#region ��״̬��ѯҽ��
			//��״̬��ѯҽ��
			//Order.Order.QueryOrder.2
			//���룺0 inpatientno 2 status
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql==null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.2",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.2�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,status.ToString());
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯ��˵ķǸ���ҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="status"></param>
		/// <param name="isSubtbl"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO, int status, bool isSubtbl )
		{
			#region ��״̬��ѯҽ��
			//��״̬��ѯҽ��
			//Order.Order.QueryOrder.2
			//���룺0 inpatientno 2 status
			//������ArrayList
			#endregion
			string sql="",sql1="";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.ForConfirmQuery",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.ForConfirmQuery�ֶ�!";
				return null;
			}
			string flag = "";
			if(isSubtbl)
			{
				flag = "1";
			}
			else
			{
				flag = "0";	
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,status.ToString(), flag);
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ������ʱ���ѯҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO, DateTime beginTime, DateTime endTime )
		{
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			#region ������ʱ���ѯҽ��
			//������ʱ���ѯҽ��
			//Order.Order.QueryOrder.3
			//���룺0 inpatientno 1BeginTime 2EndTime
			//������ArrayList
			#endregion
			
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.3",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.3�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,beginTime,endTime);
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ������Ч��Ҫ��ӡ�Ļ���Ѳ�ؿ���Ϣ
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="usage"></param>
		/// <param name="isPrint"></param>
		/// <returns></returns>
		public ArrayList QueryCircuitCard( string inpatientNO, string usage, bool isPrint )
		{
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			#region ������Ч��Ҫ��ӡ�Ļ���Ѳ�ؿ���Ϣ
			//������Ч��Ҫ��ӡ�Ļ���Ѳ�ؿ���Ϣ
			//���룺0 inpatientno 1Usage, 2 Isprint
			//������ArrayList
			#endregion

			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryCircuitCard.1",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryCircuitCard.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,usage,Neusoft.FrameWork.Function.NConvert.ToInt32(isPrint).ToString());
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ��ҽ�����Ͳ�ѯҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO, string type )
		{
			#region ��ҽ�����Ͳ�ѯҽ��
			//��ҽ�����Ͳ�ѯҽ��
			//Order.Order.QueryOrder.4
			//���룺0 inpatientno 1Type
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql==null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.4",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.4�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,type);
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯ��Ժ��ҩ��
		/// </summary>
		/// <param name="inpatientNO">������ˮ��</param>
		/// <returns></returns>
		public System.Data.DataSet QueryOutHosDrug(string inpatientNO)
		{
			string sql = "Order.Order.Query.QueryOutHosDrug";
			if(this.Sql.GetSql(sql,ref sql) == -1) 
			{
				this.Err = this.Sql.Err;
				return null;
			}
			sql= string.Format(sql,inpatientNO);
			System.Data.DataSet ds = new System.Data.DataSet();
			if( this.ExecQuery(sql,ref ds)==-1) return null;
			return ds;
		}
		/// <summary>
		/// ��ѯ��ٴ�ҩ��
		/// </summary>
		/// <param name="inpatientNO">������ˮ��</param>
		/// <returns></returns>
		public System.Data.DataSet QueryTempOutHosDrug(string inpatientNO)
		{
			string sql = "Order.Order.Query.QueryTempOutHosDrug";
			if(this.Sql.GetSql(sql,ref sql) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			sql = string.Format(sql,inpatientNO);
			System.Data.DataSet ds = new System.Data.DataSet();
			if( this.ExecQuery(sql, ref ds)==-1) return null;
			return ds;
		}
		/// <summary>
		/// ��ѯ��Ժ������Ŀ
		/// </summary>
		/// <param name="inpatientNO">������ˮ��</param>
		/// <returns></returns>
		public ArrayList QueryOutHosCure(string inpatientNO) {
			string sql = "Order.Order.Query.QueryOutHosCure";
			if(this.Sql.GetSql(sql,ref sql)==-1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			sql = string.Format(sql,inpatientNO);
			ArrayList alCure = new ArrayList();
			try {
				alCure = this.myGetOutHosCure(sql);
			}
			catch {
				 return null;
			}
			return alCure;
		}
		
		/// <summary>
		/// ��ҽ����ˮ�Ų�ѯҽ����Ϣ-������Ч����
		/// </summary>
		/// <param name="OrderNO"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Order.Inpatient.Order QueryOneOrder(string OrderNO)
		{
			string sql = "",sql1 = "";
			ArrayList al = null;
			#region ��ҽ����ˮ�Ų�ѯҽ����Ϣ
			//��ҽ����ˮ�Ų�ѯҽ����Ϣ
			//Order.Order.QueryOrder.5
			//���룺0 OrderNo
			//������ArrayList
			#endregion
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.5",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.5�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,OrderNO);
			al = this.myOrderQuery(sql);
			if(al ==null || al.Count ==0 || al.Count >1) return null;
			return al[0] as Neusoft.HISFC.Models.Order.Inpatient.Order;
		}

		/// <summary>
		/// ͨ��ҽ���Ų�ѯҽ��״̬
		/// </summary>
		/// <param name="OrderNO"></param>
		/// <returns></returns>
		public int QueryOneOrderState(string OrderNO)
		{
			string sql = string.Empty;
			if(this.Sql.GetSql("Order.Order.QueryOneOrderState.1",ref sql)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOneOrderState.1�ֶ�!";
				return -1;
			}
			try
			{
				sql= string.Format(sql,OrderNO);
			}
			catch{this.Err ="����ֵ����";this.WriteErr();return -1;}

			return Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(sql));
		}
		
		/// <summary>
		/// ��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�������������ģ�
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="type"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public ArrayList QueryOrder( string inpatientNO, Neusoft.HISFC.Models.Order.EnumType type, int status )
		{
			#region ��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�������������ģ�
			//��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�������������ģ�
			//Order.Order.QueryOrder.where.6
			//���룺0 inpatientno 1 Type 2 status
			//������ArrayList
			#endregion

			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.where.6", ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.where.6�ֶ�!";
				return null;
			}
			string strType = "1";
			if(type == Neusoft.HISFC.Models.Order.EnumType.LONG)
			{
				strType ="1";
			}
			else
			{
				strType ="0";
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,strType,status.ToString());
			return this.myOrderQuery(sql);
		}

		/// <summary>
		/// ��ѯ�Ƿ���˵�ҽ�� - ����������
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="type"></param>
		/// <param name="isConfirmed"></param>
		/// <returns></returns>
		public ArrayList QueryIsConfirmOrder( string inpatientNO, Neusoft.HISFC.Models.Order.EnumType type, bool isConfirmed )
		{
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryIsConfirmOrder.where.1",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryIsConfirmOrder.where.1�ֶ�!";
				return null;
			}
			string strType ="1";
			if(type == Neusoft.HISFC.Models.Order.EnumType.LONG)
			{
				strType ="1";
			}
			else
			{
				strType ="0";
			}
			try
			{
				sql= sql +" " +string.Format(sql1,inpatientNO,Neusoft.FrameWork.Function.NConvert.ToInt32(isConfirmed).ToString(),strType);
			}
			catch{return null;}
			return this.myOrderQuery(sql);
		}

        /// <summary>
        /// ��ѯ����ҽ��
        /// </summary>
        /// <param name="InPatientNO"></param>
        /// <returns></returns>
        public ArrayList QueryDcOrder(string InPatientNO)
        {
            #region ��ѯ����ҽ��
            //��ѯ����ҽ��
            //Order.Order.QueryOrder.1
            //���룺0 inpatientno
            //������ArrayList
            #endregion
            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();
            sql = OrderQuerySelect();
            if (sql == null) return null;
            if (this.Sql.GetSql("Order.Order.QueryOrder.OrderPrint", ref sql1) == -1)
            {
                this.Err = "û���ҵ�Order.Order.QueryOrder.1�ֶ�!";
                this.ErrCode = "-1";
                this.WriteErr();
                return null;
            }
            sql = sql + " " + string.Format(sql1, InPatientNO);
            return this.myOrderQuery(sql);
        }
		/// <summary>
		/// ��ҽ��״̬,ֹͣ��˱�־ ��ѯҽ�������������ģ�
		/// ֹͣδ���ҽ����ѯ
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="isConfirm"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public ArrayList QueryDcOrder(string inpatientNO,int status,bool isConfirm)
		{
			#region  ��ҽ��״̬,��˱�־ ��ѯҽ�������������ģ�
			// ��ҽ��״̬,��˱�־ ��ѯҽ�������������ģ�
			//Order.Order.QueryDcOrder.where.1
			//���룺0 inpatientno 1 status 2 IsConfirm
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql==null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryDcOrder.where.1",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryDcOrder.where.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,status.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(isConfirm));
			return this.myOrderQuery(sql);
		}

		/// <summary>
		/// ���ҽ����Ŀ�ĸ�����Ϣ(��Ϻţ�
		/// </summary>
		/// <param name="combNo"></param>
		/// <returns></returns>
		public ArrayList QuerySubtbl(string combNo)
		{
			#region ���ҽ����Ŀ�ĸ�����Ϣ(��Ϻţ�
			//���ҽ����Ŀ�ĸ�����Ϣ(��Ϻţ�
			//Order.Order.QueryOrder.where.7
			//���룺0 inpatientno 1 CombNo 
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql==null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrder.where.7",ref sql1)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.where.7�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,combNo);
			return this.myOrderQuery(sql);
		}
        /// <summary>
        /// �ӷ�ҩƷִ�е�����ĳ����ҩƷ�����Ƿ����ִ�м�¼
        /// {24F859D1-3399-4950-A79D-BCCFBEEAB939}
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="undrugID"></param>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public ArrayList QueryExecOrderSubtblCurrentDay(string inpatientNo, string undrugID, string deptID)
        {

            string[] s;
            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();

            s = ExecOrderQuerySelect("2");

            for (int i = 0; i <= s.GetUpperBound(0); i++)
            {
                sql = s[i];
                if (sql == null) return null;
                if (this.Sql.GetSql("Order.ExecOrder.QueryExecOrderSubtblCurrentDay", ref sql1) == -1)
                {
                    this.Err = "û���ҵ�Order.ExecOrder.QueryExecOrderBySubtblFeeMode.1�ֶ�!";
                    return null;
                }
                sql = sql + " " + string.Format(sql1, inpatientNo, undrugID, deptID);
                addExecOrder(al, sql);
            }
            return al;
        }
		/// <summary>
		/// ���ҽ��Ƥ����Ϣ
		/// </summary>
		/// <param name="orderNo"></param>
		/// <returns>-1���� 1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ����</returns>
		public int QueryOrderHypotest(string orderNo)
		{
			#region ���ҽ��Ƥ����Ϣ
			//Order.Order.QueryOrderHypotest.1
			//���룺0 OrderNo 
			//������int 1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ����
			#endregion
			string sql = "";
			int Hypotest = -1;
			
			if(this.Sql.GetSql("Order.Order.QueryOrderHypotest.1",ref sql)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrderHypotest.1�ֶ�!";
				return -1;
			}
			sql = string.Format(sql,orderNo);
			if (this.ExecQuery (sql) < 0) return -1;
			
			if(this.Reader.Read())
			{
				Hypotest=Neusoft.FrameWork.Function.NConvert.ToInt32 (this.Reader[0]);
			}
			else
			{
				Hypotest = 1;
			}
		
			this.Reader.Close();

			return Hypotest;
		}

		/// <summary>
		/// ���ҽ����ע��Ϣ
		/// </summary>
		/// <param name="orderNo"></param>
		/// <returns></returns>
		public string QueryOrderNote(string orderNo)
		{
			#region ���ҽ��������ע��Ϣ
			//Order.Order.QueryOrderNote.1
			//���룺0 OrderNo 
			//������string
			#endregion
			string sql = "";
			string Note = "";
			
			if(this.Sql.GetSql("Order.Order.QueryOrderNote.1",ref sql)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrderNote.1�ֶ�!";
				return "";
			}
			sql= string.Format(sql,orderNo);
			if (this.ExecQuery(sql) < 0) return "";
			
			if(this.Reader.Read())
			{
				Note=this.Reader[0].ToString();
			}
			this.Reader.Close();
		
			return Note;
		}

		/// <summary>
		/// ��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�����������ģ�
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="type"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public ArrayList QueryOrderWithSubtbl( string inpatientNO, Neusoft.HISFC.Models.Order.EnumType type, int status )
		{
			#region ��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�����������ģ�
			//��ҽ�����ͣ�����/��ʱ����״̬����ѯҽ�����������ģ�
			//Order.Order.QueryOrder.where.6
			//���룺0 inpatientno 1 Type 2 status
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrderWithSubtbl.where.1",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrderWithSubtbl.where.1�ֶ�!";
				return null;
			}
			string strType = "1";
			if(type == Neusoft.HISFC.Models.Order.EnumType.LONG)
			{
				strType ="1";
			}
			else
			{
				strType ="0";
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,strType,status.ToString());
			return this.myOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯ��Ч��ҽ��
		/// </summary>
		/// <param name="inpatientNO"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public ArrayList QueryValidOrderWithSubtbl( string inpatientNO, Neusoft.HISFC.Models.Order.EnumType type )
		{
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrderWithSubtbl.where.2",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrderWithSubtbl.where.2�ֶ�!";
				return null;
			}
			string strType ="1";
			if(type == Neusoft.HISFC.Models.Order.EnumType.LONG)
			{
				strType ="1";
			}
			else
			{
				strType ="0";
			}
			sql= sql +" " +string.Format(sql1,inpatientNO,strType);
			return this.myOrderQuery(sql);
		}
		#endregion 

		#region ��ˮ��
		/// <summary>
		/// ���ҽ����ˮ��
		/// </summary>
		/// <returns></returns>
		public string GetNewOrderID()
		{
			string sql = "";
			if(this.Sql.GetSql("Management.Order.GetNewOrderID",ref sql) == -1) return null;
			string strReturn = this.ExecSqlReturnOne(sql);
			if(strReturn == "-1" || strReturn == "") return null;
			return strReturn;
		}
		/// <summary>
		/// ���ҽ��ִ����ˮ��
		/// </summary>
		/// <returns></returns>
		public string GetNewOrderExecID()
		{
			string sql="";
			if(this.Sql.GetSql("Management.Order.GetNewOrderExecID",ref sql)==-1) return null;
			string strReturn = this.ExecSqlReturnOne(sql);
			if(strReturn == "-1" || strReturn == "") return null;
			return strReturn;
		}
		/// <summary>
		/// �����ҽ��������
		/// </summary>
		/// <returns></returns>
		public string GetNewOrderComboID()
		{
			string sql="";
			if(this.Sql.GetSql("Management.Order.GetComboID",ref sql)==-1) return null;
			string strReturn = this.ExecSqlReturnOne(sql);
			if(strReturn == "-1" || strReturn == "") return null;
			return strReturn;
		}
		#endregion

		#region "ҽ�����"

		/// <summary>   
		/// ����ҽ��������Ϣ����ʿ��ע��Ƥ�Խ����
		/// ����Ƥ��ǰ���жϸñ�־�Ƿ���ҪƤ�ԣ�1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ������
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="orderNo"></param>
		/// <param name="notes">��ע</param>
		/// <param name="hypotest">Ƥ�ԣ�1����ҪƤ��/2��ҪƤ�ԣ�δ��/3Ƥ����/4Ƥ������</param>
		/// <returns></returns>
		public int UpdateFeedback(string inpatientNo,string orderNo,string notes,int hypotest)
		{
			#region ����ҽ��������Ϣ
			//����ҽ��������Ϣ
			//Order.Order.Updatefeedback.1
			//���룺0 inpatientNo,1orderID,2 NOTES,3 hypotest
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.Updatefeedback.1",ref strSql)==-1) return -1;
			try
			{
				strSql = string.Format(strSql,inpatientNo,orderNo,notes,hypotest.ToString());
			}
			catch
			{
				this.Err = "����������ԣ�Order.Order.Updatefeedback.1";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}

		/// <summary>
		/// ����ҽ��ִ�б��
		/// </summary>
		/// <param name="orderNo"></param>
		/// <returns></returns>
		public int UpdateOrderExecuted(string orderNo)
		{
			#region ����ҽ��ִ�����
			//����ҽ��ִ�����
			//Order.Order.UpdateExecOrder.1
			//���룺0 orderID 1 ����Ա 2 ����ʱ��
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.UpdateExecOrder.1",ref strSql)==-1) return -1;
			try
			{
				strSql = string.Format(strSql,orderNo,this.Operator.ID,this.GetSysDateTime().ToString());
			}
			catch
			{
				this.Err = "����������ԣ�Order.Order.UpdateExecOrder.1";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}

		/// <summary>
		/// ����ҽ���������
		/// ����Ϊ����
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="orderNo"></param>
		/// <returns></returns>
		public int UpdateChargeOrder(string inpatientNo,string orderNo)
		{
			#region ����ҽ���������
			//����ҽ���������
			//Order.Order.UpdateChargeOrder.1
			//���룺0 inpatientNo,1orderID 2 ����Ա 3 ����ʱ��
			//������0 
			#endregion
			string strSql="";
			if(this.Sql.GetSql("Order.Order.UpdateChargeOrder.1",ref strSql)==-1) return -1;
			try
			{
				strSql=string.Format(strSql,inpatientNo,orderNo,this.Operator.ID,this.GetSysDateTime().ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.UpdateChargeOrder.1";
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}

		/// <summary>
		/// ����ҽ����������
		/// </summary>
		/// <param name="orderNo">����ҽ������</param>
		/// <param name="num"></param>
		/// <returns></returns>
		public int UpdateSubtblNum(string orderNo, decimal num)
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.UpdateSubNum.1", ref strSql) == -1)
			{
				return -1;
			}
			try
			{
				strSql = string.Format(strSql, orderNo, num);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
				this.ErrCode = "-1";
				this.WriteErr();
				return -1;
			}

			return this.ExecNoQuery(strSql);
			
		}
		#endregion

	
		#endregion

		#region "ҽ��ִ�д���
		
		#region "����ִ�е�"

		/// <summary>
		/// ��ʿվ������ʱҽ����ע��Ϣ
		/// </summary>
		/// <param name="orderNo"></param>
		/// <param name="txt"></param>
		/// <returns></returns>
		public int UpdateExecTime(string orderNo,string txt)
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.ExecOrder.UpdateExecTime",ref strSql) == -1)
			{
				this.Err = "û���ҵ�Order.ExecOrder.UpdateExecTimeDrug�ֶ�";
				return -1;
			}
			strSql = string.Format(strSql, orderNo, txt);
			return this.ExecNoQuery(strSql);
		}

        /// <summary>
        /// ����ִ�е���Ч���
        /// </summary>
        /// <param name="execOrderID"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public int UpdateExecValidFlag(string execOrderID, bool isPharmacy, string flag)
        {
            string strSql = "";
            string strIndex = "";

            if (isPharmacy)			//ҩƷִ�е�
                strIndex = "Order.Update.UpdateExecValidFlag.1";
            else					//��ҩƷִ�е�
                strIndex = "Order.Update.UpdateExecValidFlag.2";

            if (this.Sql.GetSql(strIndex, ref strSql) == -1)
            {
                return -1;
            }

            try
            {
                strSql = string.Format(strSql, execOrderID, flag);
            }
            catch (Exception ex)
            {
                this.Err = "�����������!" + strIndex + ex.Message;
                return -1;
            }
            return ExecNoQuery(strSql);
        }

		/// <summary>
		/// ����ִ�е� �ǳ������
		/// </summary>
		/// <param name="SqnNo"></param>
		/// <param name="isDrug"></param>
		/// <param name="dcPerson"></param>
		/// <returns></returns>
		public int DcExecImmediate(string SqnNo,bool isDrug,Neusoft.FrameWork.Models.NeuObject dcPerson) {
		    string strSql = "";
			if(isDrug) {
				if(this.Sql.GetSql("Order.ExecOrder.DcExecImmediate.UnNormal.Drug",ref strSql) == -1) {
				 	this.Err = "Can't Find Sql";
					return -1;
				}
			}
			else {
				if(this.Sql.GetSql("Order.ExecOrder.DcExecImmediate.UnNormal.UnDrug",ref strSql) == -1) {
				 	this.Err = "Can't Find Sql";
					return -1; 
				}
			}
			try{
				strSql = System.String.Format(strSql,SqnNo,dcPerson.ID,dcPerson.Name);
		    }
		    catch {
		       this.Err="�����������";
		       return -1;
	        }
            return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ����ִ�е�
		/// </summary>
		/// <param name="dcPerson">ִ�е���Ϣ</param>
		/// <returns>0 success -1 fail</returns>
        public int DcExecImmediate(Neusoft.HISFC.Models.Order.Order Order, Neusoft.FrameWork.Models.NeuObject dcPerson)
		{
			#region ����ִ�е�
			//����ִ�е�(ҽ��ֹͣ��ֱ������)
			//Order.ExecOrder.DcExecImmediate
			//���룺0 id��1 ֹͣ��id,2ֹͣ��������3ֹͣʱ��,4���ϱ�־ 
			//������0 
			#endregion

			string strSql = "",strSqlName = "Order.ExecOrder.DcExecImmediate.";
			string strType = "";
			strType = this.JudgeItemType(Order);
			if (strType == "" ) return -1;

			strSqlName= strSqlName + strType;

			if(this.Sql.GetSql(strSqlName,ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql = string.Format(strSql,Order.ID,dcPerson.ID,dcPerson.Name);
			}
			catch
			{
				this.Err="�����������"+strSqlName;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ָֹͣ��ִ�е�
		/// </summary>
		/// <param name="execOrder"></param>
		/// <param name="dcPerson"></param>
		/// <returns></returns>
        public int DcExecImmediate(Neusoft.HISFC.Models.Order.ExecOrder execOrder, Neusoft.FrameWork.Models.NeuObject dcPerson)
		{
			#region ����ִ�е�
			//����ִ�е�(ҽ��ֹͣ��ֱ������)
			//Order.ExecOrder.DcExecImmediate
			//���룺0 id��1 ֹͣ��id,2ֹͣ��������3ֹͣʱ��,4���ϱ�־ 
			//������0 
			#endregion
			string strSql = "",strSqlName = "Order.ExecOrder.DcExecImmediateByExecOrderID.";
			string strType = "";

			strType = this.JudgeItemType (execOrder.Order );
			if (strType == "" ) return -1;

			strSqlName= strSqlName + strType;
			
			if(this.Sql.GetSql(strSqlName,ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrder.ID,dcPerson.ID,dcPerson.Name);
			}
			catch
			{
				this.Err="�����������"+strSqlName;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ��ҽ����ˮ������ִ�е�
		/// </summary>
		/// <param name="Order"></param>
		/// <returns>0 success -1 fail</returns>
        public int DcExecLater(Neusoft.HISFC.Models.Order.Order Order, Neusoft.FrameWork.Models.NeuObject dcPerson, DateTime EndTime)
		{
			#region ��ҽ����ˮ������ִ�е�
			//����ִ�е�(ҽ��ֹͣ��ֱ������)
			//Order.ExecOrder.DcExecLater
			//���룺0 orderid��1 ֹͣ��id,2ֹͣ��������3ֹͣʱ��
			//������0 
			#endregion

			string strSql="",strSqlName="Order.ExecOrder.DcExecLater.";
			
			string strType = "";
			strType = this.JudgeItemType (Order);
			if (strType == "" ) return -1;

			strSqlName= strSqlName + strType;

			if(this.Sql.GetSql(strSqlName,ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,Order.ID,dcPerson.ID,dcPerson.Name,EndTime);
			}
			catch
			{
				this.Err="�����������"+strSqlName;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		

		/// <summary>
		/// ����ҽ��״̬
		/// Ϊ�Ѿ�ִ��
		/// </summary>
		/// <param name="orderNo"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public int UpdateOrderStatus(string orderNo,int status)
		{
			string strSql="";

			if(this.Sql.GetSql("Order.Update.OrderStatus",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,orderNo,status.ToString());
			}
			catch(Exception ex)
			{
				this.Err="����������ԣ�Order.Update.OrderStatus"+ex.Message;
				this.WriteErr();
				return -1;
			}
			if(this.ExecNoQuery(strSql) <= 0) return -1;
			return 0;
		}


		/// <summary>
		/// ����ҽ�������
		/// </summary>
		/// <param name="orderID"></param>
		/// <param name="sortID"></param>
		/// <returns></returns>
		public int UpdateOrderSortID(string orderID,string sortID)
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.updateOrderSort.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			return this.ExecNoQuery(strSql,orderID,sortID);
		}
		#endregion 

		#region ���´�ӡ���
		
		/// <summary>
		/// ����ִ�е���ӡ���
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <param name="itemType">1 ҩƷ ,2 ��ҩƷ</param>
		/// <returns></returns>
		public int UpdateExecOrderPrinted(string execOrderID,string itemType)
		{
			string strSql = "";
			if(itemType == "2")
			{
				//Order.ExecOrder.UpdateExecUndrugPrintFlag
				if(this.Sql.GetSql("Order.ExecOrder.UpdateExecUndrugPrintFlag",ref strSql) == -1)
				{
					this.Err = this.Sql.Err;
					return -1;
				}
				try
				{
					strSql=string.Format(strSql,execOrderID,this.Operator.ID);
				}
				catch
				{
					this.Err="����������ԣ�Order.ExecOrder.UpdateExecUndrugPrintFlag";
					this.WriteErr();
					return -1;
				}
			}
			else if(itemType == "1")
			{
				//Order.ExecOrder.UpdateExecDrugPrintFlag
				if(this.Sql.GetSql("Order.ExecOrder.UpdateExecDrugPrintFlag",ref strSql) == -1)
				{
					this.Err = this.Sql.Err;
					return -1;
				}
				try
				{
					strSql=string.Format(strSql,execOrderID,this.Operator.ID);
				}
				catch
				{
					this.Err="����������ԣ�Order.ExecOrder.UpdateExecDrugPrintFlag";
					this.WriteErr();
					return -1;
				}
			}
			return this.ExecNoQuery(strSql);
		}

		/// <summary>
		/// �����շѴ�ӡ
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <param name="itemType"></param>
		/// <returns></returns>
		public int UpdateExecNeedFeePrinted(string execOrderID,string itemType)
		{
			string strSql = "";
			if(itemType == "1")
			{
				//Order.ExecOrder.Drug.UpdateExecNeedFeePrinted
				if(this.Sql.GetSql("Order.ExecOrder.Drug.UpdateExecNeedFeePrinted",ref strSql) == -1)
				{
					this.Err = this.Sql.Err;
					return -1;
				}
				try
				{
					strSql=string.Format(strSql,execOrderID,this.Operator.ID);
				}
				catch
				{
					this.Err="����������ԣ�Order.ExecOrder.Drug.UpdateExecNeedFeePrinted";
					this.WriteErr();
					return -1;
				}
			}
			else if(itemType == "2")
			{
				//Order.ExecOrder.Undrug.UpdateExecNeedFeePrinted
				if(this.Sql.GetSql("Order.ExecOrder.Undrug.UpdateExecNeedFeePrinted",ref strSql) == -1)
				{
					this.Err = this.Sql.Err;
					return -1;
				}
				try
				{
					strSql=string.Format(strSql,execOrderID,this.Operator.ID);
				}
				catch
				{
					this.Err="����������ԣ�Order.ExecOrder.Undrug.UpdateExecNeedFeePrinted";
					this.WriteErr();
					return -1;
				}
			}
			return this.ExecNoQuery(strSql);
		}
		///<summary>
		/// �����շѴ�ӡ
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <returns></returns>
		public int UpdateTransfusionPrinted(string execOrderID)
		{
			string strSql = "";
			//Order.ExecOrder.Drug.UpdateExecNeedFeePrinted
			if(this.Sql.GetSql("Order.ExecOrder.UpdateTransfusionPrinted",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrderID,this.Operator.ID);
			}
			catch
			{
				this.Err="����������ԣ�Order.ExecOrder.UpdateTransfusionPrinted";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ����Ѳ�ؿ���ӡ���
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <returns></returns>
		public int UpdateCircultPrinted(string execOrderID)
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.ExecOrder.UpdateCircultPrinted",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql, execOrderID,this.Operator.ID);
			}
			catch
			{
				this.Err="����������ԣ�Order.ExecOrder.UpdateCircultPrinted";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		#endregion

		#region "��ѯҽ��ִ����Ϣ"
		/// <summary>
		/// ��ѯ����ҽ��ִ�����
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType">"" ȫ����1ҩ2��ҩ</param>
		/// <returns></returns>
		public ArrayList QueryExecOrder(string inpatientNo,string itemType)
		{
			#region ��ѯ����ҽ��ִ�����
			//��ѯ����ҽ��ִ�������ҩ����ҩ��
			//Order.ExecOrder.QueryPatientExec.1
			//���룺0 inpatientno
			//������ArrayList
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect (itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql= s[i];
				if (sql == null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.QueryPatientExec.1",ref sql1)==-1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryPatientExec.1�ֶ�!";
					return null;
				}
				sql= sql +" " +string.Format(sql1,inpatientNo);
				addExecOrder(al,sql);
			}
			return al;
		}
		/// <summary>
		/// ����ѯ�Ƿ���Чִ��ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType">"" ȫ����1ҩ2��ҩ</param>
		/// <param name="isValid">�Ƿ���Ч</param>
		/// <returns></returns>
		public ArrayList QueryExecOrder(string inpatientNo,string itemType,bool isValid)
		{
			#region ����ѯ��Чִ��ҽ��
			//����ѯ��Чִ��ҽ��
			//Order.ExecOrder.QueryValidOrder.1
			//���룺0 inpatientno 1  IsValid
			//������ArrayList
			#endregion

			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect(itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql= s[i];
				if (sql == null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.QueryValidOrder.1",ref sql1) == -1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryValidOrder.1�ֶ�!";
					return null;
				}
				sql= sql +" " +string.Format(sql1,inpatientNo,Neusoft.FrameWork.Function.NConvert.ToInt32(isValid).ToString());
				addExecOrder(al,sql);
			}
			return al;
		}

		/// <summary>
		/// ����ѯ�Ƿ�ִ��ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType">"" ȫ����1ҩ2��ҩ</param>
		/// <param name="isExec"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderIsExec(string inpatientNo,string itemType,bool isExec)
		{
			#region ����ѯ�Ƿ�ִ��ҽ��
			//����ѯ�Ƿ�ִ��ҽ��
			//Order.ExecOrder.QueryExecOrder.1
			//���룺0 inpatientno 1 IsExec
			//������ArrayList
			#endregion

			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			s = ExecOrderQuerySelect(itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql= s[i];
				if (sql == null ) return null;

                //{DA77B01B-63DF-4559-B264-798E54F24ABB}
                if (itemType == "")
                {
                    return null;
                }
                string strSqlName = "Order.ExecOrder.QueryExecOrder." + itemType;
                //{DA77B01B-63DF-4559-B264-798E54F24ABB}
                if (this.Sql.GetSql(strSqlName, ref sql1) == -1)
				{
                    this.Err = "û���ҵ�" + strSqlName + "�ֶ�!";
					return null;
				}
				sql= sql +" " +string.Format(sql1,inpatientNo,Neusoft.FrameWork.Function.NConvert.ToInt32(isExec).ToString());
				addExecOrder(al,sql);
			}
			return al;
		}
		/// <summary>
		/// ����ѯִ��ҽ����Ϣ
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <param name="itemType"></param>
		/// <returns>Neusoft.HISFC.Models.Order.ExecOrder</returns>
		public Neusoft.HISFC.Models.Order.ExecOrder  QueryExecOrderByExecOrderID(string execOrderID,string itemType)
		{
			#region ����ѯ�Ƿ�ִ��ҽ��
			//����ѯ�Ƿ�ִ��ҽ��
			//Order.ExecOrder.QueryExecOrder.0
			//���룺0 ExecOrderID
			//������Neusoft.HISFC.Models.Order.ExecOrder
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			s = ExecOrderQuerySelect (itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql= s[i];
				if (sql==null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.QueryExecOrder.0",ref sql1)==-1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryExecOrder.0�ֶ�!";
					return null;
				}
				sql= sql +" " +string.Format(sql1,execOrderID);
				addExecOrder(al,sql);
			}
			if(al.Count>0) return al[0] as Neusoft.HISFC.Models.Order.ExecOrder;
			return null;
		}

		/// <summary>
		/// ��ִ�в��Ų�ѯ�Ƿ�ִ��ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType"></param>
		/// <param name="isExec"></param>
		/// <param name="deptCode"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderByDept(string inpatientNo,string itemType,bool isExec,string deptCode)
		{
			#region ��ִ�в��Ų�ѯ�Ƿ�ִ��ҽ��
			//Order.ExecOrder.QueryExecOrderByDept.1
			//���룺0 inpatientno 1 IsExec 2 deptid
			//������ArrayList
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			s= ExecOrderQuerySelect(itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql = s[i];
				if (sql==null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.QueryExecOrderByDept.1",ref sql1)==-1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryExecOrder.1�ֶ�!";
					return null;
				}
				sql= sql +" " +string.Format(sql1,inpatientNo,Neusoft.FrameWork.Function.NConvert.ToInt32(isExec).ToString(),deptCode);
				addExecOrder(al,sql);
			}
			return al;
		}
		/// <summary>
		/// ����ѯ�Ƿ��շ�ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType"></param>
		/// <param name="isCharge"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderIsCharg(string inpatientNo,string itemType,bool isCharge)
		{
			#region ����ѯ�Ƿ��շ�ҽ��
			//����ѯ�Ƿ��շ�ҽ��
			//Order.ExecOrder.QueryChargeOrder.1
			//���룺0 inpatientno 1  IsCharge
			//������ArrayList
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect (itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql= s[i];
				if (sql == null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.QueryChargeOrder.1",ref sql1) == -1)
				{
					this.Err = "û���ҵ�Order.ExecOrder.QueryChargeOrder.1�ֶ�!";
					return null;
				}
				sql= sql + " " + string.Format(sql1,inpatientNo,Neusoft.FrameWork.Function.NConvert.ToInt32(isCharge).ToString());
				addExecOrder(al,sql);
			}
			return al;
		}

		/// <summary>
		/// ����ѯ��ҩ״̬ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="drugFlag"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderByDrugFlag(string inpatientNo,DateTime beginTime,DateTime endTime, int drugFlag)
		{
			#region ����ѯ��ҩ״̬ҽ��
			//����ѯ��ҩ״̬ҽ��
			//Order.ExecOrder.QueryOrderDrugFlag.1
			//���룺0 inpatientno 1  DrugFlag
			//������ArrayList
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect("1");
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderDrugFlag.1",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderDrugFlag.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNo,drugFlag.ToString(),beginTime,endTime);
			return this.myExecOrderQuery(sql);
		}
		/// <summary>
		/// ����ѯ��ҩ״̬ҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="drugFlag"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderByDrugFlag(string inpatientNo,int drugFlag)
		{
			#region ����ѯ��ҩ״̬ҽ��
			//����ѯ��ҩ״̬ҽ��
			//Order.ExecOrder.QueryOrderDrugFlag.1
			//���룺0 inpatientno 1  DrugFlag
			//������ArrayList
			#endregion

			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect("1");
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderDrugFlag.2",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderDrugFlag.2�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNo,drugFlag.ToString());
			return this.myExecOrderQuery(sql);
		}	
		/// <summary>
		/// ��ҽ����ˮ�Ų�ѯҽ��ִ����Ϣ
		/// </summary>
		/// <param name="orderNo"></param>
		/// <param name="itemType">1ҩ2��ҩ""ȫ��</param>
		/// <returns></returns>
		public ArrayList QueryExecOrderByOneOrder(string orderNo,string itemType)
		{
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			#region ��ҽ����ˮ�Ų�ѯҽ��ִ����Ϣ
			//��ҽ����ˮ�Ų�ѯҽ��ִ����Ϣ
			//Order.ExecOrder.QueryOrder.where.5
			//���룺0 OrderNo
			//������ArrayList
			#endregion
			s = ExecOrderQuerySelect(itemType);
			for (int i = 0;i <= s.GetUpperBound(0);i++)
			{
				sql = s[i];
				if (sql == null ) return null;
				if(this.Sql.GetSql("Order.ExecOrder.Query.where.5",ref sql1) == -1)
				{
					this.Err="û���ҵ�Order.ExecOrder.Query.where.5�ֶ�!";
					return null;
				}
				sql = sql +" " +string.Format(sql1,orderNo);
				addExecOrder(al,sql);
			}
			return al;
		}
	/// <summary>
	///  ������Һ����ѯ
	/// </summary>
	/// <param name="inpatientNo"></param>
	/// <param name="beginTime"></param>
	/// <param name="endTime"></param>
	/// <param name="usageCode"></param>
	/// <param name="isPrinted"></param>
	/// <returns></returns>
		public ArrayList QueryOrderExec(string inpatientNo,DateTime beginTime,DateTime endTime,string usageCode,bool isPrinted)
		{
			#region ������Һ����ѯ
			//������Һ����ѯ
			//Order.ExecOrder.QueryOrderExec.1
			//���룺0InpatientNo,1 DateTimeBegin,2 DateTimeEnd,3 UsageCode,4 PrintFlag
			//������ArrayList
			#endregion
			string[] s;
			string sql = "",sql1 = "";
			
			s = ExecOrderQuerySelect("1");
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderExec.1",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderExec.1�ֶ�!";
				this.WriteErr();
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNo,beginTime.ToString(),endTime.ToString(),usageCode,Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			return this.myExecOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯѲ�ؿ���Ϣ
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="usageCode"></param>
		/// <param name="isPrinted"></param>
		/// <returns></returns>
		public ArrayList QueryOrderCircult(string inpatientNo,DateTime beginTime,DateTime endTime,string usageCode,bool isPrinted)
		{
			string[] s;
			string sql = "",sql1 = "";
			
			s = ExecOrderQuerySelect("1");
			
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderExec.Circlue",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderExec.Circlue�ֶ�!";
				this.WriteErr();
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNo,beginTime.ToString(),endTime.ToString(),usageCode,Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			return this.myExecOrderQuery(sql);
		}
		/// <summary>
		/// ��û���ִ�е�-ҩƷ�ͷ�ҩƷ
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="isPrinted"></param>
		/// <returns></returns>
		public ArrayList QueryOrderExecBill(string inpatientNo,DateTime beginTime,DateTime endTime,string billNo,bool isPrinted)
		{
			#region ����ִ�е���ѯ
			//����ִ�е���ѯ
			//Order.ExecOrder.QueryOrderExecBill
			//���룺0InpatientNo,1 ִ�е���ˮ�� 2DateTimeBegin,3 DateTimeEnd,4 PrintFlag
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();

			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderExecBill.1",ref sql) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderExecBill.1�ֶ�!";
				return null;
			}
			sql = string.Format(sql,inpatientNo,billNo,beginTime.ToString(),endTime.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			addExecOrder(al,sql);
			//
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderExecBill.2",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderExecBill.2�ֶ�!";
				return null;
			}
			sql1=string.Format(sql1,inpatientNo,billNo,beginTime.ToString(),endTime.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			addExecOrder(al,sql1);
			return al;
		}
        #region {D05A3C7C-1CA1-4b9a-96B6-5D3018CF8FD7}
        /// <summary>
        /// ��û���ִ�е�-ҩƷ�ͷ�ҩƷ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="isPrinted"></param>
        /// <returns></returns>
        public ArrayList QueryOrderExecBillForSingle(string inpatientNo, DateTime beginTime, DateTime endTime, string billNo, bool isPrinted)
        {
            #region ����ִ�е���ѯ
            //����ִ�е���ѯ
            //Order.ExecOrder.QueryOrderExecBill
            //���룺0InpatientNo,1 ִ�е���ˮ�� 2DateTimeBegin,3 DateTimeEnd,4 PrintFlag
            //������ArrayList
            #endregion
            string sql = "";
            ArrayList al = new ArrayList();
            if (this.Sql.GetSql("Order.ExecOrder.QueryOrderExecBill.3", ref sql) == -1)
            {
                this.Err = "û���ҵ�Order.ExecOrder.QueryOrderExecBill.3�ֶ�!";
                return null;
            }
            sql = string.Format(sql, inpatientNo, billNo, beginTime.ToString(), endTime.ToString(), Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
            addExecOrder(al, sql);
            return al;
        } 
        #endregion
		/// <summary>
		/// ��û��߼��鵥��Ϣ
		/// </summary>
		/// <param name="inpatientNo">����סԺ��</param>
		/// <param name="beginTime">��ʼʱ��</param>
		/// <param name="endTime">����ʱ��</param>
		/// <param name="isPrinted">��ӡ���</param>
		/// <returns></returns>
		public ArrayList QueryOrderLisApplyBill(string inpatientNo,DateTime beginTime,DateTime endTime,bool isPrinted)
		{
			#region ����ִ�е���ѯ
			//����ִ�е���ѯ
			//Order.ExecOrder.QueryOrderLisApplyBill
			//���룺0 InpatientNo,1 DateTimeBegin,2 DateTimeEnd,4 PrintFlag
			//������ArrayList
			#endregion

			string sql="",sql1="";
			ArrayList al = new ArrayList();
			string[] s = ExecOrderQuerySelect("2");
			if(s.Length>0)
			{
				sql = s[0];
			}
			else
			{
				return null;
			}
			if(this.Sql.GetSql("Order.ExecOrder.QueryOrderLisApplyBill",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryOrderLisApplyBill�ֶ�!";
				return null;
			}
			sql1=string.Format(sql1,inpatientNo,beginTime.ToString(),endTime.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			sql1= sql +" "+sql1;
			addExecOrder(al,sql1);
			return al;
		}
		/// <summary>
		/// ��ѯҽ���շѵ�
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="itemType"></param>
		/// <param name="isPrinted"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderBillNeedCharge(string inpatientNo,string itemType,bool isPrinted)
		{
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect(itemType);
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.ExecDrugUnDrug.QueryNoCharged.1",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecDrugUnDrug.QueryNoCharged.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,inpatientNo,Neusoft.FrameWork.Function.NConvert.ToInt32(isPrinted).ToString());
			return this.myExecOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯҽ��ִ�е����ݴ�����
		/// </summary>
		/// <param name="itemType"></param>
		/// <param name="receiptNo"></param>
		/// <returns></returns>
		public ArrayList QueryExecOrderBillByReceiptNo(string itemType,string receiptNo)
		{
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect(itemType);
			sql = s[0];
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.QueryOrderExecBill.ReceiptNo",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.QueryOrderExecBill.ReceiptNo�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,receiptNo);
			return this.myExecOrderQuery(sql);
		}
		/// <summary>
		/// ��ѯ��Ҫ��ҩ��ҽ����Ϣ
		/// </summary>
		/// <param name="deptcode"></param>
		/// <returns></returns>
		public ArrayList QureyExecOrderNeedSendDrug(string deptcode)
		{
			string[] s;
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			
			s = ExecOrderQuerySelect("1");
			
			sql = s[0];
			if (sql == null ) return null;
			
			if(this.Sql.GetSql("Order.ExecOrder.QueryNeedDrug",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.ExecOrder.QueryNeedDrug�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,deptcode);
			addExecOrder(al,sql);
			
			return al;
		}
		/// <summary>
		/// ��ѯҩƷִ��ҽ��ͨ��סԺ��ˮ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <returns></returns>
		public DataSet QueryExecDrugOrderByInpatientNo(string inpatientNo) 
		{
			string strSql ="";  
			DataSet dataSet = new DataSet();

			//ȡSQL���
			if (this.Sql.GetSql("Order.Report.ExecDrugByInpatientNo",ref strSql) == -1) 
			{
				this.Err="û���ҵ�Order.Report.ExecDrugByInpatientNo�ֶ�!";
				return null;
			}
			try 
			{  
				strSql=string.Format(strSql, inpatientNo);    //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err="����ֵʱ�����Order.Report.ExecDrugByInpatientNo��"+ex.Message;
				this.WriteErr();
				return null;
			}

			//����SQL���ȡ��ѯ���
			if (this.ExecQuery(strSql, ref dataSet) == -1) return null;

			return dataSet;
		}

		
		/// <summary>
		/// ���ݻ���סԺ��ˮ�ţ���ѯ��ҩƷҽ��ִ�е���Ϣ
		/// writed by cuipeng
		/// 2005-06
		/// </summary>
		/// <param name="inpatientNo">����סԺ��ˮ��</param>
		/// <returns></returns>
		public DataSet QueryExecUndrugOrderByInpatientNo(string inpatientNo) 
		{
			string strSql ="";  
			DataSet dataSet = new DataSet();

			//ȡSQL���
			if (this.Sql.GetSql("Order.Report.ExecUndrugByInpatientNo",ref strSql) == -1) 
			{
				this.Err="û���ҵ�Order.Report.ExecUndrugByInpatientNo�ֶ�!";
				return null;
			}
			try 
			{  
				strSql=string.Format(strSql, inpatientNo);    //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err="����ֵʱ�����Order.Report.ExecUndrugByInpatientNo��"+ex.Message;
				this.WriteErr();
				return null;
			}

			//����SQL���ȡ��ѯ���
			if (this.ExecQuery(strSql, ref dataSet) == -1) return null;

			return dataSet;
		}


		/// <summary>
		/// ����סԺ��ˮ�š�ҩƷ���롢ʱ��μ��������ۼ���ҩ���  
		/// writed by liangjz 2005-06
		/// </summary>
		/// <param name="inpatientNo">סԺ��ˮ��</param>
		/// <param name="drugCode">ҩƷ����</param>
		/// <param name="myBeginTime">��ʼʱ��</param>
		/// <param name="myEndTime">��ֹʱ��</param>
		/// <returns>dataset</returns>
		public DataSet QueryTotalUseDrug(string inpatientNo,string drugCode,DateTime myBeginTime,DateTime myEndTime) 
		{
			string strSql = "";
			DataSet dataSet = new DataSet();
			
			//ȡSQL���
			if (this.Sql.GetSql("Order.Report.TotalUseDrug",ref strSql) == -1) 
			{
				this.Err="û���ҵ�Order.Report.TotalUseDrug�ֶ�!";
				return null;
			}
			try 
			{  
				strSql=string.Format(strSql, inpatientNo,drugCode,myBeginTime.ToString(),myEndTime.ToString());    //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err="����ֵʱ�����Order.Report.TotalUseDrug��"+ex.Message;
				this.WriteErr();
				return null;
			}

			//����SQL���ȡ��ѯ���
			if (this.ExecQuery(strSql, ref dataSet) == -1) return null;

			return dataSet;			
		}
		/// <summary>
		/// ����ʱ�䣬��С���ã����Ҳ�ѯ�շѵ�ҩƷ��Ϣ
		/// </summary>
		/// <param name="minFee"></param>
		/// <param name="deptCode"></param>
		/// <param name="dtBegin"></param>
		/// <param name="dtEnd"></param>
		/// <returns></returns>
		public DataSet QueryChargedMedicine(string minFee,string deptCode,string dtBegin,string dtEnd) 
		{
			string strSql = "";
			DataSet dsMedicine = new DataSet();
			if(this.Sql.GetSql("Fee.Item.QueryChargedMedicine",ref strSql) == -1) 
			{
				this.Err = "Can't Find Sql";
				return null;
			}
			strSql = System.String.Format(strSql,minFee,deptCode,dtBegin,dtEnd);
			this.ExecQuery(strSql,ref dsMedicine);
			return dsMedicine;
		}
		/// <summary>
		/// ����ʱ�䣬��С���ã����Ҳ�ѯ�շѵķ�ҩƷ��Ϣ
		/// </summary>
		/// <param name="minFee"></param>
		/// <param name="deptCode"></param>
		/// <param name="dtBegin"></param>
		/// <param name="dtEnd"></param>
		/// <returns></returns>
		public DataSet QueryChargedItem(string minFee,string deptCode,string dtBegin,string dtEnd) 
		{
			string strSql = "";
			DataSet dsItem = new DataSet();
			if(this.Sql.GetSql("Fee.Item.QueryChargedItem",ref strSql) == -1) 
			{
				this.Err = "Can't Find Sql";
				return null;
			}
			strSql = System.String.Format(strSql,minFee,deptCode,dtBegin,dtEnd);
			this.ExecQuery(strSql,ref dsItem);
			return dsItem;
		}
		/// <summary>
		/// ����ʱ�䣬�����ѯ�շѵ�ҩƷ��ϸ��Ϣ
		/// </summary>
		/// <param name="code"></param>
		/// <param name="dtBegin"></param>
		/// <param name="dtEnd"></param>
		/// <returns></returns>
		public DataSet QueryChargedMedicineDetail(string code,string deptCode,string dtBegin,string dtEnd) 
		{
			string strSql = "";
			DataSet dsMedicine = new DataSet();
			if(this.Sql.GetSql("Fee.Item.QueryChargedMedicineDetail",ref strSql) == -1) 
			{
				this.Err = "Can't Find Sql";
				return null;
			}
			strSql = System.String.Format(strSql,code,deptCode,dtBegin,dtEnd);
			this.ExecQuery(strSql,ref dsMedicine);
			return dsMedicine;
		}
		/// <summary>
		/// ����ʱ�䣬�����ѯ�շѵķ�ҩƷ��ϸ��Ϣ
		/// </summary>
		/// <param name="code"></param>
		/// <param name="dtBegin"></param>
		/// <param name="dtEnd"></param>
		/// <returns></returns>
		public DataSet QueryChargedItemDetail(string code,string deptCode,string dtBegin,string dtEnd) 
		{
			string strSql = "";
			DataSet dsItem = new DataSet();
			if(this.Sql.GetSql("Fee.Item.QueryChargedItemDetail",ref strSql) == -1) 
			{
				this.Err = "Can't Find Sql";
				return null;
			}
			strSql = System.String.Format(strSql,code,deptCode,dtBegin,dtEnd);
			this.ExecQuery(strSql,ref dsItem);
			return dsItem;
		}
		#endregion 

		#region �жϳ�Ժ��ҩ���Ƿ�ȫ���շ�
		/// <summary>
		/// �жϳ�Ժ��ҩ���Ƿ�ȫ���շ�
		/// </summary>
		/// <param name="inpatient_no"></param>
		/// <returns></returns>
		public int IsCanPrintOutHosDrug(string inpatient_no,ref bool bReturn)
		{
			bReturn = false;
			string sql = "Order.ExecOrder.QueryIsCanPrintOutHosDrug";
			if(this.Sql.GetSql(sql,ref sql)==-1)
			{
				this.Err = "�޷��鵽Order.ExecOrder.QueryIsCanPrintOutHosDrug";
				return -1;
			}
			try
			{
				sql = string.Format(sql,inpatient_no);
			}
			catch
			{
				this.Err = "Order.ExecOrder.QueryIsCanPrintOutHosDrug��������!";
				return -1;}
			int i= Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(sql));
			if(i<0)
			{

				return -1;
			}
			else if(i==0)
			{
				bReturn = true;//���Գ�Ժ
				return 0;
			}
			else
			{
				return 0;
			}
		}
		#endregion

		#endregion 

		#region "ҽ����˱���"
		/// <summary>
		/// ���ҽ�� -���δ��˵ĺ����ϵ�ҽ��
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public int ConfirmOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, bool IsCharge, System.DateTime dt)
		{
			#region ���ҽ��
			//���ҽ����ʹҽ��������Ч״̬
			//Order.Order.ConfirmOrder.1
			//���룺0 id,1 confirmcode,2 status 3confirmtime
			//������0 
			#endregion

			string strSql = "";
			if(order.Status == 0)//δ��˵�ҽ��
			{
				if(this.Sql.GetSql("Order.Order.ConfirmOrder.1",ref strSql)==-1) return -1;
				//if(order.Item.IsPharmacy==false && IsCharge)//�շѵķ�ҩƷ
                if (order.Item.ItemType != Neusoft.HISFC.Models.Base.EnumItemType.Drug && IsCharge)//�շѵķ�ҩƷ    
				{
					order.Status = 2;//��ִ�б��
				}
				else
				{
					order.Status = 1;//����˱��
				}
			}
			else if(order.Status == 3)//ֹͣ����ҽ��
			{
				if(this.Sql.GetSql("Order.Order.ConfirmOrder.2",ref strSql)==-1) return -1;
			}
			else//����
			{
                this.Err = Neusoft.FrameWork.Management.Language.Msg("ҽ��״̬�����ϣ�������ˣ�");
				return -1;
			}
			try//��ֵ
			{
				strSql=string.Format(strSql,order.ID,this.Operator.ID,order.Status.ToString(),dt);
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.ConfirmOrder.1";
				this.WriteErr();
				return -1;
			}
			int intErr = 0;
			intErr =  this.ExecNoQuery(strSql);//ִ��ҽ��
			if ( intErr == 0 )
			{
                this.Err = order.Item.Name + Neusoft.FrameWork.Management.Language.Msg(" ҽ�������仯��������ˣ�\n ��ˢ�½������¼���ҽ����Ϣ��");
				return -1;
			}
			else if (intErr < 0)
			{
                this.Err = Neusoft.FrameWork.Management.Language.Msg("���ʧ�ܣ�");
				return -1;
			}
			return 1;

		}

		/// <summary>
		/// ���ҽ�������漰ִ�е����շѽӿ��ɱ��ֲ���ɣ�
		///(��ˡ�ִ�б�����;ͨ���Ƿ��շѸ����շѱ�ǣ�
		/// </summary>
		/// <param name="order"></param>
		/// <param name="isCharge"></param>
		/// <param name="execID"></param>
		/// <returns></returns>
        public int ConfirmAndExecOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, bool isCharge, string execID)
		{
			Neusoft.HISFC.Models.Base.Employee CurUser = new Neusoft.HISFC.Models.Base.Employee();
            CurUser = (Neusoft.HISFC.Models.Base.Employee)this.Operator;
            Neusoft.HISFC.Models.Order.ExecOrder objExec = new Neusoft.HISFC.Models.Order.ExecOrder();

			//��ֵҽ����Ŀ
			objExec.Order = order;
			
			//����ҽ��
			if (order.OrderType.IsDecompose == false) //��ʱҽ��
			{
				//ִ�п����Ǳ���ʿվ �� �����ն�����
				if ((order.ExeDept.ID == order.Patient.PVisit.PatientLocation.Dept.ID ) || (JudgeItemType(objExec.Order) == "2" && 
						((Neusoft.HISFC.Models.Fee.Item.Undrug)order.Item).IsNeedConfirm == false ) )
				{
					//��ҩƷ Order.OrderType.IsCharge == false
					if (JudgeItemType(objExec.Order) == "2" && 
						((Neusoft.HISFC.Models.Fee.Item.Undrug)order.Item).IsNeedConfirm == false )
					{
						//����ִ�е�ִ�б�־
						objExec.IsExec = true;
						objExec.ExecOper.ID = CurUser.ID;
						objExec.Order.ExeDept.ID = order.ExeDept.ID;
						objExec.ExecOper.OperTime = dtCurTime;		
						//����ҽ������ִ�б�־
						if (this.UpdateOrderExecuted(order.ID) < 0 ) return -1;
					}
				}

                //�Բ���Ҫ�ն�ȷ�ϵ� ��Ȼ��Ҫ��ִ�п��ҽ��и�ֵ
                objExec.ExecOper.ID = CurUser.ID;
                objExec.Order.ExeDept.ID = order.ExeDept.ID;
                objExec.ExecOper.Dept = order.ExeDept;
                objExec.ExecOper.OperTime = dtCurTime;	
				//----���ֲ�Ƿ�----
				if (isCharge)
				{
					//����ִ�е����ʱ�־
					objExec.IsCharge=true;
					objExec.ChargeOper.ID = CurUser.ID;
					objExec.ChargeOper.Dept.ID = CurUser.Dept.ID;
					objExec.ChargeOper.OperTime = dtCurTime;

                    //�������շ���Ŀ ����ִ�б��Ϊ��ִ�С�
                    objExec.IsExec = true;
                    objExec.ExecOper.ID = CurUser.ID;
                    objExec.Order.ExeDept.ID = order.ExeDept.ID;
                    objExec.ExecOper.OperTime = dtCurTime;	
				}

				//����ִ�е�
				if(execID == "")
				{
					try{ objExec.ID =GetNewOrderExecID();}
					catch{}
				}
				else
				{
					objExec.ID = execID;
				}
				
				if(objExec.ID=="-1" || objExec.ID =="") return -1;
				

				if(JudgeItemType(objExec.Order) == "1" )//ҩƷ��ִ�б��
				{
					objExec.IsExec = true;
					objExec.ExecOper.ID = CurUser.ID;
					objExec.Order.ExeDept.ID = order.ExeDept.ID;
					objExec.ExecOper.OperTime = dtCurTime;	
					//ҩƷ����С��λ
					if(objExec.Order.Unit ==((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).MinUnit)//��С��λ
					{
						
					}
					else
					{
						objExec.Order.Qty = objExec.Order.Qty * objExec.Order.Item.PackQty;;//�����С��λ
						objExec.Order.Unit = ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).MinUnit;
					}
				}
				else//��ҩƷ
				{

				}

				objExec.IsValid = true;
				objExec.DateUse = order.BeginTime;
				objExec.DateDeco= dtCurTime;
				objExec.DrugFlag = 0; //Ĭ��Ϊ����Ҫ����
				
				if (this.InsertExecOrder(objExec) < 0 ) return -1;

			}
			
			//���ҽ��
			if (this.ConfirmOrder(order,isCharge,dtCurTime) < 0 ) return -1;
			return 0;
			
		}
        /// <summary>
        /// ������˺���
        /// </summary>
        /// <param name="order"></param>
        /// <param name="isCharge"></param>
        /// <param name="execID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int ConfirmAndExecOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, bool isCharge, string execID, DateTime dt)
        {
		    this.dtCurTime = dt;
			return this.ConfirmAndExecOrder(order,isCharge,execID);
		}
		#endregion

		#region "ҽ���ֽ�"

        #region  {DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
        /// <summary>
        /// ��ȡ���ҵķֽ�ʱ��
        /// </summary>
        /// <returns></returns>
        public int GetDecomposeTime()
        {
            //��ȡ�ֽ�ʱ�����
            Neusoft.HISFC.BizLogic.Manager.Constant constantManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
            Neusoft.FrameWork.Models.NeuObject constant = new Neusoft.FrameWork.Models.NeuObject();
            string deptCode = ((Neusoft.HISFC.Models.Base.Employee)controler.Operator).Dept.ID;
            try
            {
                constant = constantManager.GetConstant("SETEXECTIME", deptCode);
            }
            catch (Exception e)
            {
                this.Err = "��ȡ�ֽ�ʱ�����~~" + e.Message.ToString();
                this.WriteErr();
                return 12;
            }
            if (constant.Memo == "")
            {
                return 12;
            }
            else
            {
                return Convert.ToInt32(constant.Memo);
            }
        }
        #endregion
		/// <summary>
		/// �ֽ�ʱ��
		/// </summary>
		/// <returns></returns>
		public void DecomposeTime( string nurseStationCode )
		{
			if ( nurseStationCode == strNurseStationCode)
			{

			}
			else //�仯
            {
                //������
				controler.SetTrans(this.Trans);
                //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
                //string s = controler.QueryControlerInfo("200011");\
                string s = "";

				this.strNurseStationCode = nurseStationCode;
				if(s=="-1" || s=="") 
				{
                    //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
                    //iHour = 12;//Ĭ��12��
                    iHour = GetDecomposeTime();
					iMinute = 01;
					return;
				}
				iHour = Neusoft.FrameWork.Function.NConvert.ToDateTime(s).Hour;
				iMinute = Neusoft.FrameWork.Function.NConvert.ToDateTime(s).Minute;
			}
			return;
		}

        protected int iHour = 12;
		protected int iMinute = 1;
		protected string strNurseStationCode = "";
		public int iNum = 0;
		protected Neusoft.FrameWork.Management.ControlParam controler = new Neusoft.FrameWork.Management.ControlParam();
		DateTime dtCurTime = new DateTime();//ϵͳʱ��
        /// <summary>
        /// {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF} СʱƵ��
        /// </summary>
        private string hourFerquenceID = "";

        /// <summary>
        /// {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF} �ֽⷽ��
        /// </summary>
        /// <param name="order"></param>
        /// <param name="days"></param>
        /// <param name="isCharge"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int DecomposeOrderToNow(Neusoft.HISFC.Models.Order.Inpatient.Order order, int days, bool isCharge, DateTime dt)
        {
            dtCurTime = dt;
            int myDays = 0;
            myDays = System.DateTime.Compare(order.NextMOTime.Date, dt.Date);
            return DecomposeOrder(order, days, isCharge);
        }


		/// <summary>
		/// ���طֽ⺯��
		/// </summary>
		/// <param name="order"></param>
		/// <param name="days"></param>
		/// <param name="isCharge"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
        public int DecomposeOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, int days, bool isCharge, DateTime dt)
        {
		    //dtCurTime = dt;
            //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
            //dtCurTime = new DateTime(dt.Year,dt.Month,dt.Day,12,01,0);
            dtCurTime = new DateTime(dt.Year, dt.Month, dt.Day, GetDecomposeTime(), 01, 0);
			return DecomposeOrder(order,days,isCharge);
		}
		/// <summary>
		///  ҩ����ҩ���ֽ�
		///  Days�ֽ�����IsCharge �Ƿ��շ�
		/// </summary>
		/// <param name="order"></param>
		/// <param name="days">�ֽ�����</param>
		/// <param name="isCharge">�Ƿ��շ�</param>
		/// <returns></returns>
        public int DecomposeOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, int days, bool isCharge)
        {
            Neusoft.HISFC.Models.Base.Employee CurUser = new Neusoft.HISFC.Models.Base.Employee();
            if (this.Operator == null)
            {
                this.Err = "��ǰ����ԱΪ�գ�";
                return -1;
            }

            //��õ�ǰ����Ա			
            CurUser = (Neusoft.HISFC.Models.Base.Employee)this.Operator;

            //��÷ֽ�ʱ���
            iNum = iNum + 1;
            DecomposeTime(CurUser.Nurse.ID);
            Neusoft.HISFC.Models.Order.ExecOrder objExec = new Neusoft.HISFC.Models.Order.ExecOrder();

            //��ֵҽ����Ŀ
            objExec.Order = order;

            //�ֽ�ʱ��
            DateTime dtCurDeco = dtCurTime;
            decimal DecAmount = -1;

            //���ڡ�����Ч���ֽ�ʱ��С��ָ��ʱ���ҽ���͸���(���г����ֽⲻ�շ�)
            if (order.OrderType.IsDecompose && (order.Status == 1 || order.Status == 2) && order.NextMOTime <= dtCurDeco.AddDays(days))//0������1 ���
            {
                int Cycle = 0;
                Cycle = System.Convert.ToInt16(order.Frequency.Days[0]);//ѭ��ʱ�� 
                //-------����ÿ������--------	
                if (order.OrderType.IsCharge)
                {
                    //��Ҫ�Ʒѡ�ҩƷҽ����Ҫ����ҩƷȡ���������
                    if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)//(order.Item.GetType().ToString() == "Neusoft.HISFC.Models.Pharmacy.Item")
                    {
                        Neusoft.HISFC.Models.Pharmacy.Item objPharmacy;
                        objPharmacy = (Neusoft.HISFC.Models.Pharmacy.Item)order.Item;

                        DecAmount = ComputeAmount(objPharmacy.ID, objPharmacy.DosageForm.ID, order.DoseOnce, objPharmacy.BaseDose, order.Patient.PVisit.PatientLocation.Dept.ID);
                        if (DecAmount >= 0)
                        {
                            order.Item.Qty = DecAmount;
                        }
                    }
                    //����Ʒѡ�ҩƷҽ��ֱ�ӻ��ÿ������
                    //��ҩƷ�����ģ�ֱ�ӻ��ÿ��ִ������		
                }

                if (order.Frequency.Days.Length > 1)//�ֽ�����Ϊ����
                {
                    Cycle = 1;
                }


                //����ʱ��
                DateTime dtTemp = dtCurDeco.AddDays(days);//+ Cycle - 1);
                DateTime dtEndTime;
                //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
                //int endHour = 12;
                int endHour = GetDecomposeTime();
                int endMinute = 01;

                bool bIsHaveDecompose = false;//{F1C96C96-F829-4ea1-AD07-10DBCC091C16}
                int iMOCount = 0;//�ֽ����ִ�е�����{F1C96C96-F829-4ea1-AD07-10DBCC091C16}
                //�Ƿ�ӷ�ҩʱ�䣿����������
                //��ҩƷ�����ĵڶ�������շ�
                //if(order.Item.IsPharmacy == false && order.IsSubtbl ==false)//��ҩƷ�����Ǹ���


                if (order.Item.ItemType != Neusoft.HISFC.Models.Base.EnumItemType.Drug && order.IsSubtbl == false)//��ҩƷ�����Ǹ���
                {
                    //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
                    //endHour = 12;//��ҩƷʱ�䣨ͬҩƷ�ֽ�ʱ�䣩
                    endHour = GetDecomposeTime();
                    endMinute = 01;
                }
                else
                {
                    //�����ҩƷ�ĸ��ģ��ֽ�ʱ�䰴��ҩƷ�ֽ��
                    if (order.ExtendFlag2 == ("����ҩƷ���ı�ǡ�"))
                    {
                        //{DAE0F990-2FA5-4708-A3D8-B08CE0C6ADA7} ���ݸ��Կ����趨��ʱ�����ֽ� made by guanyx
                        //endHour = 12;//��ҩƷʱ�䣨ͬҩƷ�ֽ�ʱ�䣩
                        endHour = GetDecomposeTime();
                        endMinute = 01;
                    }
                    else//ҩƷ��ҩƷ���İ�ҩƷ��
                    {
                        endHour = iHour;
                        endMinute = iMinute;
                    }
                }


                //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF} СʱƵ��
                if (string.IsNullOrEmpty(this.hourFerquenceID) == true)
                {
                    this.hourFerquenceID = this.controler.QueryControlerInfo("200042", false);
                    if (string.IsNullOrEmpty(hourFerquenceID) == true)
                    {
                        this.hourFerquenceID = "NONE";
                    }
                }

                //��ʱҽ���ֽ�ʱ�䵽���ηֽ�ʱ�䣫days
                if (order.Frequency.ID == this.hourFerquenceID)
                {
                    endHour = dtCurDeco.Hour;
                    endMinute = dtCurDeco.Minute;
                }

                dtEndTime = new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day, endHour, endMinute, 0);//Ĭ�Ϸֽ⵽�ڶ��� 12:01

                //�´�ִ������<ָ�������ڣ��ֽ�������
                int Count = System.Convert.ToInt16(new TimeSpan(order.NextMOTime.Date.Ticks - dtCurDeco.Date.Ticks).TotalDays);

                if (order.Frequency.Days.Length > 1)
                {
                    #region �ֽ�ҽ��Ϊ���ڵ�
                    for (int i = 0; i <= days; i++)
                    {
                        for (int k = 1; k < order.Frequency.Days.Length; k++)//ѭ��������
                        {
                            int week = dtCurTime.AddDays(i).DayOfWeek.GetHashCode();
                            if (week == 0) week = 7;
                            if (order.Frequency.Days[k] == week.ToString())//�ҵ����µ�
                            {
                                if (this.DecomposeTime(order, dtCurDeco, dtEndTime, isCharge, objExec, dtCurTime, CurUser, i) == -1)
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region �ֽ�ҽ��Ϊ����ÿ���
                    while (Count <= (days + Cycle - 1))
                    {
                        #region �ֽ�ʱ��
                        for (int i = 0; i <= order.Frequency.Times.GetUpperBound(0); i++)
                        {
                            DateTime dt = new DateTime();
                            try { dt = Neusoft.FrameWork.Function.NConvert.ToDateTime(order.Frequency.Times[i]); }
                            catch { }
                            if (dt.GetType().ToString() != "System.DateTime")
                            {
                                return -1;
                            }
                            DateTime dtUseTime = new DateTime(dtCurDeco.AddDays(Count).Year, dtCurDeco.AddDays(Count).Month, dtCurDeco.AddDays(Count).Day, dt.Hour, dt.Minute, dt.Second);
                            //��ҩʱ��>=�´�ִ������and��ҩʱ��<�ֽ����ʱ�䣿
                            if (dtUseTime >= order.CurMOTime && dtUseTime < dtEndTime)//wolf �����ˣ���֪���᲻������,���뿿Ψһ���������ظ���¼Date_NexMO
                            {
                                //��ҩʱ���Ƿ����ҽ��ֹͣʱ��?
                                if (dtUseTime > order.EndTime && order.EndTime != DateTime.MinValue)
                                {
                                    //ֹͣ����ҽ��
                                    order.Status = 3;
                                    order.DCOper.OperTime = order.EndTime;
                                    if (DcOneOrder(order) < 0) return -1;
                                }
                                else
                                {
                                    //----���ֲ�Ƿ�----
                                    if (isCharge)
                                    {
                                        //����ִ�е����ʱ�־
                                        objExec.IsCharge = true;
                                        objExec.ChargeOper.ID = CurUser.ID;
                                        objExec.ChargeOper.Dept.ID = CurUser.Dept.ID;
                                        objExec.ChargeOper.OperTime = dtCurTime;
                                    }
                                    //����ִ�е�
                                    try { objExec.ID = GetNewOrderExecID(); }
                                    catch { }
                                    if (objExec.ID == "-1" || objExec.ID == "") return -1;
                                    objExec.IsValid = true;
                                    objExec.DateUse = dtUseTime;
                                    objExec.DateDeco = dtCurTime;
                                    objExec.DrugFlag = 0; //Ĭ��Ϊ����Ҫ����

                                    if (objExec.Order.Item.GetType() == typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
                                    {
                                        try
                                        {   //����ͨ��������������С��λ���ڼ������
                                            //ԭ�����жϵ�Frequency.User01 ����Ϊʹ�� ExecDose����
                                            //if(objExec.Order.Frequency.User01 != "" && objExec.Order.Frequency.User01 != null) //����Ƶ��
                                            if (objExec.Order.ExecDose != "" && objExec.Order.ExecDose != null) //����Ƶ��
                                            {
                                                string[] tempDoseOnce = objExec.Order.ExecDose.Split('-');
                                                decimal tempDoseOnceDec = Convert.ToDecimal(tempDoseOnce[i]);
                                                objExec.Order.Qty = tempDoseOnceDec / ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).BaseDose;
                                                //���������ֽ����С����С��λ����ͷ���� {276ED20A-E9FD-495f-BFE2-6F38265982BF} wbo 2010-10-21
                                                if (objExec.Order.Qty % 1 != 0)
                                                {
                                                    objExec.Order.Qty = decimal.Ceiling(objExec.Order.Qty);
                                                }
                                                objExec.Order.DoseOnce = tempDoseOnceDec;
                                                if (objExec.Order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)//(objExec.Order.Item.GetType().ToString() == "Neusoft.HISFC.Models.Pharmacy.Item")
                                                {
                                                    decimal decAmount = 0;
                                                    Neusoft.HISFC.Models.Pharmacy.Item objPharmacy;
                                                    objPharmacy = (Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item;

                                                    decAmount = ComputeAmount(objPharmacy.ID, objPharmacy.DosageForm.ID, tempDoseOnceDec, objPharmacy.BaseDose, order.Patient.PVisit.PatientLocation.Dept.ID);
                                                    if (decAmount >= 0)
                                                    {
                                                        objExec.Order.Item.Qty = decAmount;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (DecAmount != -1)
                                                    objExec.Order.Qty = DecAmount;
                                                else
                                                {
                                                    objExec.Order.Qty = objExec.Order.DoseOnce / ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).BaseDose;
                                                    //���������ֽ����С����С��λ����ͷ���� {276ED20A-E9FD-495f-BFE2-6F38265982BF} wbo 2010-10-21
                                                    if (objExec.Order.Qty % 1 != 0)
                                                    {
                                                        objExec.Order.Qty = decimal.Ceiling(objExec.Order.Qty);
                                                    }
                                                }
                                            }
                                            objExec.Order.Unit = ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).MinUnit;
                                        }
                                        catch
                                        {
                                            this.Err = "����Ϊ�㡣";
                                            this.WriteErr();
                                        }
                                    }
                                    //����ִ�е�
                                    if (this.InsertExecOrder(objExec) == -1)
                                    {
                                        if (this.DBErrCode != 1) return -1;
                                    }
                                    iMOCount++;//{F1C96C96-F829-4ea1-AD07-10DBCC091C16}�ֽ����
                                    bIsHaveDecompose = true;//{F1C96C96-F829-4ea1-AD07-10DBCC091C16}
                                }
                            }
                        }
                        #endregion
                        Count = Count + Cycle;
                    }
                    #endregion
                }

                //{97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF}
                //��ҽ�����´ηֽ�ʱ��Days + Cycle
                if (days < Cycle)
                {
                    if (days == 0)
                    {
                        //ֻ�ֽ⵽����,�´ηֽ�ʱ�丳�ɵ���,
                    }
                    else
                    {
                        days = Cycle;
                    }
                }

                order.CurMOTime = dtCurDeco;
                DateTime dtNex = new DateTime();

                if (Cycle > 1)
                    //{F1C96C96-F829-4ea1-AD07-10DBCC091C16}�޸�QOD��20������
                    if (bIsHaveDecompose)
                    {
                        //���Ƶ�εĻ���������1ҽ�����´ηֽ�ʱ��Ӧ�ü��ϣ����ηֽ����*Ƶ�εĻ�����������

                        dtNex = order.NextMOTime.AddDays(iMOCount * Cycle);
                    }
                    else
                    {
                        dtNex = order.NextMOTime;
                    }
                else
                    dtNex = dtCurDeco.AddDays(days);

                order.NextMOTime = new DateTime(dtNex.Year, dtNex.Month, dtNex.Day, endHour, endMinute, 0);//

                //���·ֽ�ʱ�䣨���Σ��´Σ�
                if (UpdateDecoTime(order) <= 0) return -1;
            }
            return 0;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="dtCurDeco"></param>
		/// <param name="dtEndTime"></param>
		/// <param name="isCharge"></param>
		/// <param name="objExec"></param>
		/// <param name="dtCurTime"></param>
		/// <param name="curUser"></param>
		/// <param name="addDays"></param>
		/// <returns></returns>
        private int DecomposeTime(Neusoft.HISFC.Models.Order.Inpatient.Order order, DateTime dtCurDeco, DateTime dtEndTime, bool isCharge, Neusoft.HISFC.Models.Order.ExecOrder objExec, DateTime dtCurTime, Neusoft.HISFC.Models.Base.Employee curUser, int addDays)
		{
			#region �ֽ�ʱ��
			for (int i=0;i<=order.Frequency.Times.GetUpperBound(0);i++)
			{
				DateTime dt = new DateTime();	
				try{dt =Neusoft.FrameWork.Function.NConvert.ToDateTime(order.Frequency.Times[i]);}
				catch{}
				if (dt.GetType().ToString() != "System.DateTime") 
				{
					return -1;
				}
				DateTime dtUseTime = new DateTime(dtCurDeco.AddDays(addDays).Year,dtCurDeco.AddDays(addDays).Month,dtCurDeco.AddDays(addDays).Day,dt.Hour,dt.Minute,dt.Second);
				//��ҩʱ��>=�´�ִ������and��ҩʱ��<�ֽ����ʱ�䣿
				if ( dtUseTime >= order.NextMOTime  && dtUseTime < dtEndTime)
				{
					//��ҩʱ���Ƿ����ҽ��ֹͣʱ��?
					if (dtUseTime > order.EndTime  && order.EndTime !=DateTime.MinValue)
					{
						//ֹͣ����ҽ��
						order.Status = 3;
						order.DCOper.OperTime = order.EndTime;
						if (DcOneOrder(order) < 0) return 0;
					}
					else
					{
						//----���ֲ�Ƿ�----
						if (isCharge)
						{
							//����ִ�е����ʱ�־
							objExec.IsCharge=true;
							objExec.ChargeOper.ID = curUser.ID;
							objExec.ChargeOper.Dept.ID = curUser.Dept.ID;
							objExec.ChargeOper.OperTime = dtCurTime;
						}
						//����ִ�е�
						try{ objExec.ID =GetNewOrderExecID();}
						catch{}
						if(objExec.ID=="-1" || objExec.ID =="") return 0;
						objExec.IsValid = true;
						objExec.DateUse = dtUseTime;
						objExec.DateDeco= dtCurTime;
						objExec.DrugFlag = 0; //Ĭ��Ϊ����Ҫ����
								
						if(objExec.Order.Item.GetType()== typeof(Neusoft.HISFC.Models.Pharmacy.Item))//ҩƷ
						{
							try
							{   //����ͨ��������������С��λ���ڼ������
								objExec.Order.Qty = objExec.Order.DoseOnce / ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).BaseDose;
                                //���������ֽ����С����С��λ����ͷ���� {276ED20A-E9FD-495f-BFE2-6F38265982BF} wbo 2010-10-21
                                if (objExec.Order.Qty % 1 != 0)
                                {
                                    objExec.Order.Qty = decimal.Ceiling(objExec.Order.Qty);
                                }
								objExec.Order.Unit = ((Neusoft.HISFC.Models.Pharmacy.Item)objExec.Order.Item).MinUnit;
							}
							catch
							{
								this.Err = Neusoft.FrameWork.Management.Language.Msg("����Ϊ�㡣");
								this.WriteErr();}
						}
						if (this.InsertExecOrder(objExec) < 0 ) return 0;
					}
				}
			}
			return 0;
			#endregion
		}
		/// <summary>
		/// ��÷ֽ�ȡ����׼
		/// </summary>
		/// <param name="drugCode"></param>
		/// <param name="doseCode"></param>
		/// <param name="doseOnce"></param>
		/// <param name="baseDose"></param>
		/// <param name="deptCode"></param>
		/// <returns></returns>
		public decimal ComputeAmount( string drugCode, string doseCode, decimal doseOnce, decimal baseDose, string deptCode )
		{
			#region ��÷ֽ�ȡ����׼
			//��÷ֽ�ȡ����׼
			//Order.Order.ComputeAmount.1
			//���룺0 DrugCode
			//������0 UNIT_STAT
			#endregion		

            #region {AD50C155-BE2D-47b8-8AF9-4AF3548A2726}
            //�����Ż�
            string UnitSate = string.Empty;

            if (htDrugedProperty.Contains(drugCode + deptCode))
            {
                UnitSate = htDrugedProperty[drugCode + deptCode].ToString();
            }
            else
            {
                UnitSate = this.GetDrugProperty(drugCode, doseCode, deptCode);

                htDrugedProperty.Add(drugCode + deptCode, UnitSate);
            }
            #endregion

            decimal Amount=0;
			if(baseDose==0) return Amount;
			switch (UnitSate)
			{
				case "0"://�����ԣ�����ȡ��
                    //Amount = (decimal)System.Math.Ceiling((decimal)doseOnce / (decimal)baseDose);
                    Amount = (decimal)System.Math.Ceiling((double)((decimal)doseOnce / (decimal)baseDose));
					break;
				case "1"://���ԣ���ҩʱ��ȡ��
					Amount=System.Convert.ToDecimal(doseOnce) / baseDose;
					break;
				case "2"://���ԣ���ҩʱ��ȡ�� 
					Amount=System.Convert.ToDecimal(doseOnce) / baseDose;
					break;
				default://
					Amount=System.Convert.ToDecimal(doseOnce) / baseDose;
					break;
			}
			return Amount;
		}
		/// <summary>
		/// ��ȡҩƷ��ҩ����
		/// </summary>
		/// <param name="drugCode">ҩƷ����</param>
		/// <param name="deptCode">���ұ���</param>
		/// <returns>�ɹ�������ҩ���� 0 ���ɲ�� 1 �ɲ�ֲ�ȡ�� 2 �ɲ����ȡ����ʧ�ܷ���NULL</returns>
		public string GetDrugProperty(string drugCode,string doseCode,string deptCode) 
		{
			string strSQL = "";
			//ȡSELECT���
			if (this.Sql.GetSql("Pharmacy.Item.GetDrugProperty",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Pharmacy.Item.GetDrugProperty�ֶ�!";
				return null;
			}

			//��ʽ��SQL���
			try 
			{
				strSQL = string.Format(strSQL, drugCode, doseCode,deptCode);
			}
			catch (Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Pharmacy.Item.GetDrugProperty:" + ex.Message;
				return null;
			}		

			//ִ�в�ѯ���
			if (this.ExecQuery(strSQL)==-1) 
			{
				this.Err="�����ҩ������Ϣʱ��ִ��SQL������"+this.Err;
				this.ErrCode="-1";
				return null;
			}
			string drugProperty = "";
			if ( this.Reader.Read()) 
			{
				drugProperty = this.Reader[0].ToString();
			}
			else 
			{
				drugProperty = "0";
			}
			this.Reader.Close();
			return drugProperty;
		}
        
		/// <summary>
		/// ���·ֽ�ʱ�䣨���Σ��´Σ�
		/// </summary>
		/// <param name="order">ҽ��id,(���Σ��´�)�ֽ�ʱ��</param>
		/// <returns></returns>
        public int UpdateDecoTime(Neusoft.HISFC.Models.Order.Order order)
		{
			#region ���·ֽ�ʱ��
			//���·ֽ�ʱ��
			//Order.Order.UpdateDecoTime.1
			//���룺0 orderID 1 Date_CurMO 2 Date_NexMO
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.UpdateDecoTime.1",ref strSql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,order.ID,order.CurMOTime.ToString(),order.NextMOTime.ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.UpdateDecoTime.1";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// �´ηֽ�ʱ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="days">+_����</param>
		/// <returns></returns>
		public int UpdateDecoTime( string inpatientNo, int days )
		{
			#region ���·ֽ�ʱ��
			//���·ֽ�ʱ��
			//Order.Order.UpdateDecoTime.2
			//���룺0 inpatientNo 1 days
			//������0 
			#endregion
			string strSql="";
			if(this.Sql.GetSql("Order.Order.UpdateDecoTime.2",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				if(days>0)
					strSql=string.Format(strSql,inpatientNo,"+"+days.ToString());
				else
					strSql=string.Format(strSql,inpatientNo,days.ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.UpdateDecoTime.2";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// �´ηֽ�ʱ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="nextDecoDateTime">�´ηֽ�ʱ��</param>
		/// <returns></returns>
		public int UpdateDecoTime(string inpatientNo,DateTime nextDecoDateTime)
		{
			string strSql="";
			if(this.Sql.GetSql("Order.Order.UpdateDecoTime.3",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,inpatientNo,nextDecoDateTime.ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.UpdateDecoTime.3";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}

        #region {AD50C155-BE2D-47b8-8AF9-4AF3548A2726}

        /// <summary>
        /// ��ҩ���Ա�
        /// </summary>
        private Hashtable htDrugedProperty = new Hashtable();

        /// <summary>
        /// Ϊ���Ż��������ϵ�ҽ���ֽⱣ��ʱ����ҩƷִ�е��õĺ���
        /// </summary>
        /// <param name="execOrder">ҩƷִ�е�</param>
        /// <returns></returns>
        public int UpdateForConfirmExecDrug(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.Order.UpdateForConfirmExecDrug.1", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            try
            {
                strSql = string.Format(strSql, execOrder.ID,//ִ�е�ID
                                         execOrder.DrugFlag,//ҩƷ���ͱ��
                                         execOrder.ChargeOper.ID,//�����˴���
                                         execOrder.ChargeOper.Dept.ID,//���˿��Ҵ���
                                         execOrder.ChargeOper.OperTime.ToString(),//����ʱ��
                                         Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsCharge).ToString(),//���˱��
                                         execOrder.Order.ReciptNO,//������
                                         execOrder.Order.SequenceNO,//������ˮ�� 
                                         execOrder.ExecOper.ID, //ִ�л�ʿ����
                                         execOrder.Order.ExeDept.ID, //ִ�п��Ҵ���
                                         execOrder.ExecOper.OperTime.ToString(), //ִ��ʱ��
                                         Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsExec).ToString());//ִ�б��
            }
            catch
            {
                this.Err = "����������ԣ�Order.Order.UpdateForConfirmExecDrug.1";
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// Ϊ���Ż��������ϵ�ҽ���ֽⱣ��ʱ���·�ҩƷִ�е��õĺ���
        /// </summary>
        /// <param name="execOrder">��ҩƷִ�е�</param>
        /// <returns></returns>
        public int UpdateForConfirmExecUnDrug(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.Order.UpdateForConfirmExecUnDrug.1", ref strSql) == -1)
            {
                this.Err = this.Sql.Err;
                return -1;
            }
            try
            {
                strSql = string.Format(strSql, execOrder.ID,//ִ�е�ID
                                       execOrder.ChargeOper.ID,//�����˴���
                                       execOrder.ChargeOper.Dept.ID,//���˿��Ҵ���
                                       execOrder.ChargeOper.OperTime.ToString(),//����ʱ��
                                       Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsCharge).ToString(),//���˱��
                                       execOrder.Order.ReciptNO,//������
                                       execOrder.Order.SequenceNO,//������ˮ�� 
                                       execOrder.ExecOper.ID, //ִ�л�ʿ����
                                       execOrder.Order.ExeDept.ID, //ִ�п��Ҵ���
                                       execOrder.ExecOper.OperTime.ToString(), //ִ��ʱ��
                                       Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsExec).ToString(),//ִ�б��
                                       Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsConfirm).ToString());//����ȷ�ϱ��
                
            }
            catch
            {
                this.Err = "����������ԣ�Order.Order.UpdateForConfirmExecUnDrug.1";
                this.WriteErr();
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }

        #endregion

		#endregion

		#region "ҽ��ֹͣ����"
		/// <summary>
		/// ֹͣҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="sysClass"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public int DcOrder(string inpatientNo,Neusoft.HISFC.Models.Base.SysClassEnumService sysClass,DateTime dt)
		{
			ArrayList al = new ArrayList();
			DateTime dtBegin = new DateTime(2005,1,1);
			al = this.QueryOrder(inpatientNo,dtBegin,dt);
			if(al ==null) return -1;
			foreach(Neusoft.HISFC.Models.Order.Inpatient.Order order in al)
			{
				if(order.Status ==1 || order.Status == 0 )//��Ч��������,��ִ��ҽ����
				{
					if(sysClass == null)//ȫ����ֹͣ
					{	
						order.Status =3;
						order.DCOper.OperTime =dt;
						order.DCOper.ID  = this.Operator.ID ;
						order.DCOper.Name = this.Operator.Name;
						order.DcReason.ID ="0";
						if(order.OrderType.IsDecompose)
						{
							if(this.DcOneOrder(order)==-1) 
							{
								return -1;
							}
						}
					}
					else //���ֹͣ //ָֹͣ���ĳ���ҽ��  �磺����ȵȡ�
					{
						if(order.Item.SysClass.ID.ToString() == sysClass.ID.ToString() && order.OrderType.IsDecompose)
						{
							order.Status =3;
							order.DCOper.OperTime  =dt;
							order.DCOper.ID  = this.Operator.ID ;
							order.DCOper.Name = this.Operator.Name;
							order.DcReason.ID ="0";
							if(this.DcOneOrder(order)==-1) 
							{
								return -1;
							}
						}
					}
				}
			}
			this.Err ="ҽ��ֹͣ�ɹ���";
			return 0;
		}
		/// <summary>
		/// ֹͣҽ��
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public int DcOrder(string inpatientNo,DateTime dt)
		{
			return DcOrder(inpatientNo,null,dt);
		}
		/// <summary>
		/// ֹͣҽ����������ֱ��ֹͣ��Ԥֹͣ��
		/// ֹͣԭ��Order.DcReason
		/// </summary>
		/// <param name="order"></param>
		/// <param name="isAllDc">���ҽ���Ƿ�ȫ��ֹͣ</param>
		/// <param name="ReturnMemo">����ҽ��ԭʼ״̬</param>
		/// <returns></returns>
        public int DcOrder(Neusoft.HISFC.Models.Order.Inpatient.Order order, bool isAllDc, out string ReturnMemo)
		{
			//����ҽ��ִ�����
			ReturnMemo = "";
			if (order.Status == 2  )
			{
				ReturnMemo = "����ҽ���Ѿ��Ѿ�ִ�У���ȷ�Ϻ��˷ѣ�";
			}
			//�Ƿ������ҽ��
			ArrayList al = new ArrayList();
			al = this.QueryOrderByCombNO(order.Combo.ID,false);
			//�Ƿ������ҽ��
			if (al.Count > 1)
			{
				//�Ƿ�ֹͣ�����Ŀ�еĵ���
				if (isAllDc)
				{
					for (int i=0;i<al.Count;i++)
					{
                        Neusoft.HISFC.Models.Order.Inpatient.Order obj;
                        obj = (Neusoft.HISFC.Models.Order.Inpatient.Order)al[i];
						//����¼�����洦��
						if (obj.ID == order.ID) continue;
                        //ִ�е���ҽ��DC
                        #region {D028C0B7-014F-4c60-883D-B49A0BD3399A}
                        obj.DCOper = order.DCOper;
                        obj.DcReason = order.DcReason;
                        #endregion
                        if (DcOneOrderDeal(obj) < 0) return -1;
					}
				}	    
				else
				{
					//�Ƿ���ҩ
					if (order.Combo.IsMainDrug)
					{
						//��ҩҽ�����ܵ���DC
						this.Err = "���ܵ���������ҩҽ����";
						return -1;
					}
				}
			}
			//else�������Ŀִ�е���ҽ������
			//ִ�е���ҽ��DC
			if (DcOneOrderDeal(order) < 0) return -1;
			return 0;

		}
		/// <summary>
		/// ���ݻ���סԺ�š�ϵͳ����ֹͣԭ��ֹͣ���ߴ˴����ҽ�����������û�д�����Ҫֹͣҽ��ִ�е�
		/// 0 ����סԺ��ˮ�ţ�1 ϵͳ���2ֹͣʱ�� ,3ֹͣԭ����룬4ֹͣԭ������ 
		/// </summary>
		/// <param name="inpatientNo"></param>
		/// <param name="sysClassCode"></param>
		/// <param name="dcDate"></param>
		/// <param name="dcReasonCode"></param>
		/// <param name="dcReasonName"></param>
		/// <returns></returns>
		public int DcOrder( string inpatientNo, string sysClassCode, DateTime dcDate, string dcReasonCode, string dcReasonName ) 
		{
			string strSql="";
			//ֹͣҽ������
			if(this.Sql.GetSql("Order.Order.DcOrder.All",ref strSql)==-1) {
				this.Err = "�Ҳ���SQL���:Order.Order.DcOrder.All";
				return -1;
			}
			try {
				strSql=string.Format(   strSql, 
										inpatientNo,        //����סԺ��ˮ��
										sysClassCode,       //ϵͳ���
										dcDate.ToString(),  //ֹͣ����
										dcReasonCode,       //ֹͣԭ�����
										dcReasonName,       //ֹͣԭ������
										this.Operator.ID,   //ֹͣ��
										this.Operator.Name  //ֹͣ������
									);
			}
			catch {
				this.Err="����������ԣ�Order.Order.DcOrder.All";
				return -1;
			}
			int parm = this.ExecNoQuery(strSql);
			if (parm == -1) return -1;

			//���sysClassCode��Ч��������"00"������ֻ������ֹͣҽ����������ִ�е����ݣ�����ִֹͣ�е�
			if (sysClassCode == "00") {
				//ֹͣҩƷҽ��ִ�е�
				if(this.Sql.GetSql("Order.ExecOrder.DcExecAll.Drug",ref strSql)==-1) {
					this.Err = "�Ҳ���SQL���:Order.ExecOrder.DcExecAll.Drug";
					return -1;
				}
				try {
					strSql=string.Format(strSql, inpatientNo, dcDate.ToString(), this.Operator.ID, dcReasonName);
				}
				catch {
					this.Err="����������ԣ�Order.ExecOrder.DcExecAll.Drug";
					return -1;
				}
				parm = this.ExecNoQuery(strSql);
				if (parm == -1) return -1;

				//ֹͣ��ҩƷҽ��ִ�е�
				if(this.Sql.GetSql("Order.ExecOrder.DcExecAll.Undrug",ref strSql)==-1) {
					this.Err = "�Ҳ���SQL���:Order.ExecOrder.DcExecAll.Undrug";
					return -1;
				}
				try {
					strSql=string.Format(strSql, inpatientNo, dcDate.ToString(), this.Operator.ID, dcReasonName);
				}
				catch {
					this.Err="����������ԣ�Order.ExecOrder.DcExecAll.Undrug";
					return -1;
				}
				parm = this.ExecNoQuery(strSql);
				if (parm == -1) return -1;
			}

			return parm;
		}

		/// <summary>
		/// /// ���ݻ���סԺ�ź�ֹͣԭ��ֹͣ��������ҽ��������ҽ��ִ�е�
		/// </summary>
		/// <param name="inpatientNo">0 ����סԺ��ˮ��,1 ֹͣʱ�� ,2 ֹͣԭ����룬3 ֹͣԭ������ </param>
		/// <param name="dcDate"></param>
		/// <param name="dcReasonCode"></param>
		/// <param name="dcReasonName"></param>
		/// <returns></returns>
		public int DcOrder( string inpatientNo, DateTime dcDate, string dcReasonCode, string dcReasonName ) 
		{
			return this.DcOrder(inpatientNo, "00", dcDate, dcReasonCode, dcReasonName);
		}

		/// <summary>
		/// ִ�е���ҽ��DC����
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        private int DcOneOrderDeal(Neusoft.HISFC.Models.Order.Inpatient.Order order)
		{
			//��ȡϵͳʱ��
			DateTime CurTime =this.GetDateTimeFromSysDateTime();
			//����ֹͣ����ʱҽ�� or ֹͣʱ��С�ڵ��ڵ�ǰʱ�䣩
			if (order.OrderType.IsDecompose == false || order.EndTime <= CurTime) 
			{
				//�����ϱ��
				order.Status = 3;
				if (this.DcExecImmediate(order,this.Operator) <  0) 
				{
                    this.Err = Neusoft.FrameWork.Management.Language.Msg("����ҽ��ִ����Ϣʧ�ܣ�");
					return -1;
				}
			}
			//Ԥֹͣ������ҽ������ֹͣʱ����ڵ��ڵ�ǰʱ�䣩
			else
			{
				if (this.DcExecLater(order,this.Operator,order.EndTime) <  0) 
				{
                    this.Err = Neusoft.FrameWork.Management.Language.Msg("����ҽ��ִ����Ϣʧ�ܣ�");
					return -1;
				}
			}
			//ִ��ֹͣҽ����
			if (this.DcOneOrder(order) < 0 ) 
			{
                this.Err = Neusoft.FrameWork.Management.Language.Msg("ֹͣҽ��ʧ�ܣ�");
				return -1;
			}
			//ҩ��
			//if (order.Item.IsPharmacy)
            if(order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
			{
				//������ҩ����
			}
			//��ҩ��
			else
			{
				//ֹͣ������Ŀ�����»�����
				//�����˷�����

			}
			return 0;
		}
		/// <summary>
        /// ����ϺŲ�ѯҽ��
        /// �������ݿ��SQL���������ĿǰisSubtbl����û����
		/// </summary>
		/// <param name="combno">��Ϻ�</param>
		/// <param name="isSubtbl">�������ݿ��SQL���������Ŀǰ�������û����</param>
		/// <returns></returns>
		public ArrayList QueryOrderByCombNO( string combno, bool isSubtbl )
		{
			#region ��״̬��ѯҽ��
			//����ϺŲ�ѯҽ��
			//Order.Order.QueryOrderByCombno.where.1
			//���룺0 combno 1 IsSub
			//������ArrayList
			#endregion
			string sql = "",sql1 = "";
			ArrayList al = new ArrayList();
			sql = OrderQuerySelect();
			if (sql == null ) return null;
			if(this.Sql.GetSql("Order.Order.QueryOrderByCombno.where.1",ref sql1) == -1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrderByCombno.where.1�ֶ�!";
				return null;
			}
			sql= sql +" " +string.Format(sql1,combno,isSubtbl.ToString());
			return this.myOrderQuery(sql);

		}

		#endregion

        #region ��Һ��Ϣ

        /// <summary>
        /// ��������Һ��Ϣ
        /// </summary>
        /// <param name="isNurse">�Ƿ񰴲������� ��deptCode����Ϊ��������</param>
        /// <param name="deptCode">����</param>
        /// <param name="dtBegin">��ʼִ��ʱ��</param>
        /// <param name="dtEnd">��ִֹ��ʱ��</param>
        /// <param name="isExec">״̬ �Ƿ��ѯ����Һ��</param>
        /// <returns>�ɹ����ش���Һ��Ϣ ʧ�ܷ���null</returns>
        public ArrayList QueryExecOrderForCompound(bool isNurse,string deptCode, DateTime dtBegin, DateTime dtEnd,bool isExec)
        {
            string[] s;
            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();

            s = ExecOrderQuerySelect("1");
            sql = s[0];
            if (sql == null)
            {
                return null;
            }

            if (isNurse)
            {
                if (this.Sql.GetSql("Order.QueryOrderExecCompound.NurseCell", ref sql1) == -1)
                {
                    this.Err = "û���ҵ�Order.QueryOrderExecCompound.NurseCell�ֶ�!";
                    return null;
                }
            }
            else
            {
                if (this.Sql.GetSql("Order.QueryOrderExecCompound.DeptCode", ref sql1) == -1)
                {
                    this.Err = "û���ҵ�Order.QueryOrderExecCompound.DeptCode�ֶ�!";
                    return null;
                }
            }

            string state = "0";
            if (isExec)
            {
                state = "1";
            }
            else
            {
                state = "0";
            }

            sql = sql + " " + string.Format(sql1, deptCode,dtBegin.ToString(),dtEnd.ToString(),state);

            return this.myExecOrderQuery(sql);
        }

        /// <summary>
        /// ��Һ��Ϣ����
        /// </summary>
        /// <param name="execOrderID">ִ�е���ˮ��</param>
        /// <param name="compound">��Һ��Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1 �޼�¼���·���0</returns>
        public int UpdateOrderCompound(string execOrderID, Neusoft.HISFC.Models.Order.Compound compound)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.ExecOrder.UpdateOrderCompound", ref strSql) == -1)
            {
                this.Err = "û���ҵ�Order.ExecOrder.UpdateOrderCompound�ֶ�";
                return -1;
            }
            strSql = string.Format(strSql, execOrderID,
                                           Neusoft.FrameWork.Function.NConvert.ToInt32(compound.IsExec).ToString(),
                                           compound.CompoundOper.ID,compound.CompoundOper.Dept.ID,
                                           compound.CompoundOper.OperTime.ToString());

            return this.ExecNoQuery(strSql);
        }

        #endregion

        #region ����
        /// <summary>
		/// ��û���
		/// </summary>
		/// <param name="sysClassID"></param>
		/// <returns></returns>
		public Neusoft.HISFC.Models.Order.EnumMutex QueryMutex( string sysClassID )
		{
			string sql = "";
			Neusoft.HISFC.Models.Order.EnumMutex mutex = Neusoft.HISFC.Models.Order.EnumMutex.None;
			if(this.Sql.GetSql("Order.QueryMutex.1",ref sql) == -1) 
			{
				this.Err = this.Sql.Err;
				return mutex;
			}
			sql = string.Format(sql,sysClassID);
			string strMutex ="";
			strMutex = this.ExecSqlReturnOne(sql);
			try
			{
				mutex = (Neusoft.HISFC.Models.Order.EnumMutex)Neusoft.FrameWork.Function.NConvert.ToInt32(strMutex);
				return mutex;
			}
			catch
			{
				return mutex;
			}
		}
		#endregion

		#region ˽�к���
		
		/// <summary>
		/// �����Ŀ����
		/// </summary>
		/// <param name="strSql"></param>
		/// <returns></returns>
		private ArrayList myGetOutHosCure(string strSql) 
		{
			ArrayList al = new ArrayList();
			if(this.ExecQuery(strSql) == -1) return null;
			while(this.Reader.Read()) 
			{
				Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList item = new Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList();
				try 
				{
					item.Name = this.Reader[0].ToString();//��Ŀ����
					item.Item.Price = System.Convert.ToDecimal(this.Reader[1].ToString());//�۸�
					item.Order.Qty = System.Convert.ToDecimal(this.Reader[2].ToString());//����
					item.Order.Unit = this.Reader[3].ToString();//��λ
					item.Order.ExeDept.ID = this.Reader[4].ToString();//ִ�п���
					item.User01 = this.Reader[5].ToString();//ִ�����
				}
				catch(Exception ex) 
				{
					this.Err="��ü�鵥��Ϣ����"+ex.Message;
					this.WriteErr();
					return null;	   
				}
				al.Add(item);	 
			}
			this.Reader.Close();
			return al;
		}

		/// <summary>
		/// ���ҽ����Ϣ
		/// </summary>
		/// <param name="sqlOrder"></param>
		/// <param name="Order"></param>
		/// <returns></returns>
        private string getOrderInfo(string sqlOrder, Neusoft.HISFC.Models.Order.Inpatient.Order Order)
		{
			#region "�ӿ�˵��"
			//0 IDҽ����ˮ��
			//������Ϣ����  
			//			1 סԺ��ˮ��   2סԺ������     3���߿���id      4���߻���id
			//ҽ��������Ϣ
			// ������Ŀ��Ϣ
			//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
			//	       10��Ŀ������ 11��Ŀ�������  12ҩƷ���     13ҩƷ��������  14������λ       
			//         15��С��λ     16��װ����,     17���ʹ���     18ҩƷ���  ,   19ҩƷ����
			//         20���ۼ�       21�÷�����      22�÷�����     23�÷�Ӣ����д  24Ƶ�δ���  
			//         25Ƶ������     26ÿ�μ���      27��Ŀ����     28�Ƽ۵�λ      29ʹ������			  
			// ����ҽ������
			//		   30ҽ�������� 31ҽ���������  32ҽ���Ƿ�ֽ�:1����/2��ʱ     33�Ƿ�Ʒ� 
			//		   34ҩ���Ƿ���ҩ 35��ӡִ�е�    36�Ƿ���Ҫȷ��  
			// ����ִ�����
			//		   37����ҽʦId   38����ҽʦname  39��ʼʱ��      40����ʱ��     41��������
			//		   42����ʱ��     43¼����Ա����  44¼����Ա����  45�����ID     46���ʱ��       
			//		   47DCԭ�����   48DCԭ������    49DCҽʦ����    50DCҽʦ����   51Dcʱ��
			//         52ִ����Ա���� 53ִ��ʱ��      54ִ�п��Ҵ���  55ִ�п������� 
			//		   56���ηֽ�ʱ�� 57�´ηֽ�ʱ��
			// ����ҽ������
			//		   58�Ƿ�Ӥ����1��/2��          59�������  	  60������     61��ҩ��� 
			//		   62�Ƿ񸽲�'1'  63�Ƿ��������  64ҽ��״̬      65�ۿ���     66ִ�б��1δִ��/2��ִ��/3DCִ�� 
			//		   67ҽ��˵��                     68�Ӽ����:1��ͨ/2�Ӽ�         69�������
			//         70��鲿λ��ע                 71��ע          72����,          73 ��������/��鲿λ����,
			//         74 ȡҩҩ������ 
			#endregion 
			string strItemType= "";
			if(Order.CurMOTime==DateTime.MinValue)
			{
				Order.CurMOTime = Order.BeginTime ;
			}
			if(Order.NextMOTime  ==DateTime.MinValue)
			{
				Order.NextMOTime = Order.BeginTime;
			}
			//�ж�ҩƷ/��ҩƷ
		   
			if (Order.Item.GetType()== typeof(Neusoft.HISFC.Models.Pharmacy.Item))
			{
				Neusoft.HISFC.Models.Pharmacy.Item objPharmacy ;
				objPharmacy = (Neusoft.HISFC.Models.Pharmacy.Item)Order.Item;
				strItemType = "1";
				try
				{
					System.Object[] s={Order.ID,Order.Patient.ID,Order.Patient.PID.PatientNO,Order.Patient.PVisit.PatientLocation.Dept.ID,Order.Patient.PVisit.PatientLocation.NurseCell.ID,
										  strItemType,Order.Item.ID,Order.Item.Name,Order.Item.UserCode,Order.Item.SpellCode,
										  Order.Item.SysClass.ID.ToString(),Order.Item.SysClass.Name,objPharmacy.Specs,objPharmacy.BaseDose,objPharmacy.DoseUnit,objPharmacy.MinUnit,objPharmacy.PackQty,
										  objPharmacy.DosageForm.ID,objPharmacy.Type.ID,objPharmacy.Quality.ID,objPharmacy.PriceCollection.RetailPrice,
										  Order.Usage.ID,Order.Usage.Name,Order.Usage.Memo,Order.Frequency.ID,Order.Frequency.Name,
										  Order.DoseOnce,Order.Qty,Order.Unit,Order.HerbalQty.ToString(),
										  Order.OrderType.ID,Order.OrderType.Name,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsDecompose),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsCharge),
										  Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsNeedPharmacy),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsPrint),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsConfirm),
										  Order.ReciptDoctor.ID,Order.ReciptDoctor.Name,Order.BeginTime,Order.EndTime,Order.ReciptDept.ID,
										  Order.MOTime,Order.Oper.ID,Order.Oper.Name,Order.Nurse.ID,Order.ConfirmTime,
										  Order.DcReason.ID,Order.DcReason.Name,Order.DCOper.ID,Order.DCOper.Name,Order.DCOper.OperTime,
										  Order.ExecOper.ID,Order.ExecOper.OperTime,Order.ExeDept.ID,Order.ExeDept.Name,
										  Order.CurMOTime,Order.NextMOTime,
										  Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsBaby),Order.BabyNO,Order.Combo.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Combo.IsMainDrug),
										  Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsSubtbl),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsHaveSubtbl),Order.Status,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsStock),Order.ExecStatus,
										  Order.Note,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsEmergency),Order.SortID,Order.Memo,Order.CheckPartRecord,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Reorder),Order.Sample.Name,Order.StockDept.ID,
										  objPharmacy.IsAllergy==true ?"2":"1",Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsPermission),Order.Package.ID,Order.Package.Name,Order.ExtendFlag1,Order.ExtendFlag2,Order.ExtendFlag3,
                                          Order.Frequency.Time,Order.ExecDose,Order.ApplyNo};//�¼�����Ƶ��
		
					string myErr = "";
					if(Neusoft.FrameWork.Public.String.CheckObject(out myErr,s)==-1)
					{
						this.Err = myErr;
						this.WriteErr();
						return null;
					}
					sqlOrder=string.Format(sqlOrder,s);
				}
				catch(Exception ex)
				{
					this.Err="����ֵʱ�����"+ex.Message;
					this.WriteErr();
					return null;
				}
			}
			else if (Order.Item.GetType()== typeof(Neusoft.HISFC.Models.Fee.Item.Undrug))
			{
				Neusoft.HISFC.Models.Fee.Item.Undrug objAssets;
				objAssets = (Neusoft.HISFC.Models.Fee.Item.Undrug)Order.Item;
				strItemType = "2";
				
				try
				{
					string[] s={Order.ID,Order.Patient.ID,Order.Patient.PID.PatientNO,Order.Patient.PVisit.PatientLocation.Dept.ID,Order.Patient.PVisit.PatientLocation.NurseCell.ID,
								   strItemType,Order.Item.ID,Order.Item.Name,Order.Item.UserCode,Order.Item.SpellCode,
								   Order.Item.SysClass.ID.ToString(),Order.Item.SysClass.Name,objAssets.Specs,"0","","","0","","","",objAssets.Price.ToString(),
								   Order.Usage.ID,Order.Usage.Name,Order.Usage.Memo,Order.Frequency.ID,Order.Frequency.Name,
								   Order.DoseOnce.ToString(),Order.Qty.ToString(),Order.Unit,Order.HerbalQty.ToString(),
								   Order.OrderType.ID,Order.OrderType.Name,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsDecompose).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsCharge).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsNeedPharmacy).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsPrint).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsConfirm).ToString(),
								   Order.ReciptDoctor.ID,Order.ReciptDoctor.Name,Order.BeginTime.ToString(),Order.EndTime.ToString(),Order.ReciptDept.ID,
								   Order.MOTime.ToString(),Order.Oper.ID,Order.Oper.Name,Order.Nurse.ID,Order.ConfirmTime.ToString(),
								   Order.DcReason.ID,Order.DcReason.Name,Order.DCOper.ID,Order.DCOper.Name,Order.DCOper.OperTime.ToString(),
								   Order.ExecOper.ID,Order.ExecOper.OperTime.ToString(),Order.ExeDept.ID,Order.ExeDept.Name,
								   Order.CurMOTime.ToString(),Order.NextMOTime.ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsBaby).ToString(),Order.BabyNO.ToString(),Order.Combo.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Combo.IsMainDrug).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsSubtbl).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsHaveSubtbl).ToString(),Order.Status.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsStock).ToString(),Order.ExecStatus.ToString(),
								   Order.Note,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsEmergency).ToString(),Order.SortID.ToString(),Order.Memo,Order.CheckPartRecord,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Reorder).ToString(),Order.Sample.Name,Order.StockDept.ID,
								   "",Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsPermission).ToString(),Order.Package.ID,Order.Package.Name,Order.ExtendFlag1,Order.ExtendFlag2,Order.ExtendFlag3,
                                   Order.Frequency.Time,Order.ExecDose,Order.ApplyNo};//�¼�����Ƶ��
					sqlOrder=string.Format(sqlOrder,s);
				}
				catch(Exception ex)
				{
					this.Err="����ֵʱ�����"+ex.Message;
					this.WriteErr();
					return null;
				}
            }
            //{8F86BB0D-9BB4-4c63-965D-969F1FD6D6B2} ҽ�����İ����� by gengxl //{BE3C743B-7343-4e12-A1AF-4B2793C8F9BB}
            else if (Order.Item.GetType() == typeof(Neusoft.HISFC.Models.FeeStuff.MaterialItem))
            {
                //{BE3C743B-7343-4e12-A1AF-4B2793C8F9BB}
                Neusoft.HISFC.Models.FeeStuff.MaterialItem objAssets;
                //{BE3C743B-7343-4e12-A1AF-4B2793C8F9BB}
                objAssets = (Neusoft.HISFC.Models.FeeStuff.MaterialItem)Order.Item;
                strItemType = "2";

                try
                {
                    string[] s ={Order.ID,Order.Patient.ID,Order.Patient.PID.PatientNO,Order.Patient.PVisit.PatientLocation.Dept.ID,Order.Patient.PVisit.PatientLocation.NurseCell.ID,
								   strItemType,Order.Item.ID,Order.Item.Name,Order.Item.UserCode,Order.Item.SpellCode,
								   Order.Item.SysClass.ID.ToString(),Order.Item.SysClass.Name,objAssets.Specs,"0","","","0","","","",objAssets.Price.ToString(),
								   Order.Usage.ID,Order.Usage.Name,Order.Usage.Memo,Order.Frequency.ID,Order.Frequency.Name,
								   Order.DoseOnce.ToString(),Order.Qty.ToString(),Order.Unit,Order.HerbalQty.ToString(),
								   Order.OrderType.ID,Order.OrderType.Name,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsDecompose).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsCharge).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsNeedPharmacy).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsPrint).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.OrderType.IsConfirm).ToString(),
								   Order.ReciptDoctor.ID,Order.ReciptDoctor.Name,Order.BeginTime.ToString(),Order.EndTime.ToString(),Order.ReciptDept.ID,
								   Order.MOTime.ToString(),Order.Oper.ID,Order.Oper.Name,Order.Nurse.ID,Order.ConfirmTime.ToString(),
								   Order.DcReason.ID,Order.DcReason.Name,Order.DCOper.ID,Order.DCOper.Name,Order.DCOper.OperTime.ToString(),
								   Order.ExecOper.ID,Order.ExecOper.OperTime.ToString(),Order.ExeDept.ID,Order.ExeDept.Name,
								   Order.CurMOTime.ToString(),Order.NextMOTime.ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsBaby).ToString(),Order.BabyNO.ToString(),Order.Combo.ID,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Combo.IsMainDrug).ToString(),
								   Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsSubtbl).ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsHaveSubtbl).ToString(),Order.Status.ToString(),Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsStock).ToString(),Order.ExecStatus.ToString(),
								   Order.Note,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsEmergency).ToString(),Order.SortID.ToString(),Order.Memo,Order.CheckPartRecord,Neusoft.FrameWork.Function.NConvert.ToInt32(Order.Reorder).ToString(),Order.Sample.Name,Order.StockDept.ID,
								   "",Neusoft.FrameWork.Function.NConvert.ToInt32(Order.IsPermission).ToString(),Order.Package.ID,Order.Package.Name,Order.ExtendFlag1,Order.ExtendFlag2,Order.ExtendFlag3,
                                   Order.Frequency.Time,Order.ExecDose,Order.ApplyNo};//�¼�����Ƶ��
                    sqlOrder = string.Format(sqlOrder, s);
                }
                catch (Exception ex)
                {
                    this.Err = "����ֵʱ�����" + ex.Message;
                    this.WriteErr();
                    return null;
                }
            }
			else 
			{
				this.Err = "��Ŀ���ͳ���";
				return null;
			}
			return sqlOrder;
		}

		/// ��ѯ������Ϣ��select��䣨��where������
		private string OrderQuerySelect()
		{
			#region �ӿ�˵��
			//Order.Order.QueryOrder.Select.1
			//���룺0
			//������sql.select
			#endregion
			string sql="";
			if(this.Sql.GetSql("Order.Order.QueryOrder.Select.1",ref sql)==-1)
			{
				this.Err="û���ҵ�Order.Order.QueryOrder.Select.1�ֶ�!";
				this.ErrCode="-1";
				this.WriteErr();
				return null;
			}
			return sql;
		}
		//˽�к�������ѯҽ����Ϣ
		private ArrayList myOrderQuery(string SQLPatient)
		{
			
			ArrayList al=new ArrayList();

			if(this.ExecQuery(SQLPatient)==-1) return null;
			try
			{
				while (this.Reader.Read())
				{
					Neusoft.HISFC.Models.Order.Inpatient.Order objOrder = new Neusoft.HISFC.Models.Order.Inpatient.Order();
					#region "������Ϣ"
					//������Ϣ����  
					//			1 סԺ��ˮ��   2סԺ������     3���߿���id      4���߻���id
					try
					{
						objOrder.Patient.ID =this.Reader[1].ToString();
						objOrder.Patient.PID.PatientNO = this.Reader[2].ToString(); 
						objOrder.Patient.PVisit.PatientLocation.Dept.ID = this.Reader[3].ToString(); 
						objOrder.InDept.ID = this.Reader[3].ToString();
						objOrder.Patient.PVisit.PatientLocation.NurseCell.ID = this.Reader[4].ToString(); 

					}
					catch(Exception ex)
					{
						this.Err="��û��߻�����Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					#endregion
					#region "��Ŀ��Ϣ"
					//ҽ��������Ϣ
					// ������Ŀ��Ϣ
					//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
					//	       10��Ŀ������ 11��Ŀ�������  12ҩƷ���     13ҩƷ��������  14������λ       
					//         15��С��λ     16��װ����,     17���ʹ���     18ҩƷ���  ,   19ҩƷ����
					//         20���ۼ�       21�÷�����      22�÷�����     23�÷�Ӣ����д  24Ƶ�δ���  
					//         25Ƶ������     26ÿ�μ���      27��Ŀ����     28�Ƽ۵�λ      29ʹ������			  
					// �ж�ҩƷ/��ҩƷ
					//         25Ƶ������     26ÿ�μ���      27��Ŀ����     28�Ƽ۵�λ      29ʹ������			  
					// 73 ��������/��鲿λ ����
					if (this.Reader[5].ToString() == "1")//ҩƷ
					{
						Neusoft.HISFC.Models.Pharmacy.Item objPharmacy= new Neusoft.HISFC.Models.Pharmacy.Item();
						try
						{
							objPharmacy.ID = this.Reader[6].ToString();
							objPharmacy.Name = this.Reader[7].ToString();
							objPharmacy.UserCode = this.Reader[8].ToString();
							objPharmacy.SpellCode = this.Reader[9].ToString();
							objPharmacy.SysClass.ID = this.Reader[10].ToString();
							//objPharmacy.SysClass.Name = this.Reader[11].ToString();
							objPharmacy.Specs     = this.Reader[12].ToString();
							//							try
							//							{
							if(!this.Reader.IsDBNull(13)) objPharmacy.BaseDose  = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[13].ToString());
							//}
							//							catch{}
							objPharmacy.DoseUnit = this.Reader[14].ToString();
							objPharmacy.MinUnit = this.Reader[15].ToString();
							//try
							//{
							if(!this.Reader.IsDBNull(16))objPharmacy.PackQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[16].ToString());
							//}
							//catch{}
							objPharmacy.DosageForm.ID = this.Reader[17].ToString();
							objPharmacy.Type.ID    = this.Reader[18].ToString();
							objPharmacy.Quality.ID = this.Reader[19].ToString();
							//try
							//{
							if(!this.Reader.IsDBNull(20))objPharmacy.PriceCollection.RetailPrice= Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[20].ToString());
							//}
							//catch{}					
							objOrder.Usage.ID  =this.Reader[21].ToString();
							objOrder.Usage.Name=this.Reader[22].ToString();
							objOrder.Usage.Memo=this.Reader[23].ToString();
						}
						catch(Exception ex)
						{
							this.Err="���ҽ����Ŀ��Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						objOrder.Item = objPharmacy;
					}
					else if (this.Reader[5].ToString() == "2")//��ҩƷ
					{
						Neusoft.HISFC.Models.Fee.Item.Undrug objAssets=new Neusoft.HISFC.Models.Fee.Item.Undrug();
						try
						{
							objAssets.ID = this.Reader[6].ToString();
							objAssets.Name = this.Reader[7].ToString();
							objAssets.UserCode = this.Reader[8].ToString();
							objAssets.SpellCode = this.Reader[9].ToString();
							objAssets.SysClass.ID = this.Reader[10].ToString();
							//objAssets.SysClass.Name = this.Reader[11].ToString();
							objAssets.Specs     = this.Reader[12].ToString();
							//							try
							//							{
							if(!this.Reader.IsDBNull(20))objAssets.Price= Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[20].ToString());
							//}
							//							catch{}	
							objAssets.PriceUnit = this.Reader[28].ToString();
							//��������/��鲿λ����
							objOrder.Sample.Name = this.Reader[73].ToString();
						}
						catch(Exception ex)
						{
							this.Err="���ҽ����Ŀ��Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						objOrder.Item = objAssets;
					}

				
					objOrder.Frequency.ID  =this.Reader[24].ToString();
					objOrder.Frequency.Name=this.Reader[25].ToString();
					//try
					//{
					if(!this.Reader.IsDBNull(26))objOrder.DoseOnce=Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[26].ToString());//}
					//catch{}
					//try
					//{
					if(!this.Reader.IsDBNull(27))objOrder.Qty =Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[27].ToString());//}
					//catch{}
					objOrder.Unit=this.Reader[28].ToString();
					//try
					//{
					if(!this.Reader.IsDBNull(29))objOrder.HerbalQty=Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[29].ToString());//}
					//catch{}
					
					#endregion
					#region "ҽ������"
					// ����ҽ������
					//		   30ҽ�������� 31ҽ���������  32ҽ���Ƿ�ֽ�:1����/2��ʱ     33�Ƿ�Ʒ� 
					//		   34ҩ���Ƿ���ҩ 35��ӡִ�е�    36�Ƿ���Ҫȷ��   
					try
					{
						objOrder.ID = this.Reader[0].ToString();
						objOrder.ExtendFlag1 = this.Reader[78].ToString();//��ʱҽ��ִ��ʱ�䣭���Զ���
						objOrder.OrderType.ID = this.Reader[30].ToString();
						objOrder.OrderType.Name = this.Reader[31].ToString();
						//try
						//{
						if(!this.Reader.IsDBNull(32))objOrder.OrderType.IsDecompose = System.Convert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[32].ToString()));//}
						//catch{}
						//try
						//{
						if(!this.Reader.IsDBNull(33))objOrder.OrderType.IsCharge = System.Convert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[33].ToString()));//}
						//catch{}
						//try
						//{
						if(!this.Reader.IsDBNull(34))objOrder.OrderType.IsNeedPharmacy = System.Convert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[34].ToString()));//}
						//catch{}
						//try
						//{
						if(!this.Reader.IsDBNull(35))objOrder.OrderType.IsPrint = System.Convert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[35].ToString()));//}
						//catch{}
						//try
						//{
						if(!this.Reader.IsDBNull(36))objOrder.OrderType.IsConfirm = System.Convert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[36].ToString()));//}
						//catch{}
					}
					catch(Exception ex)
					{
						this.Err="���ҽ��������Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					#endregion
					#region "ִ�����"
					// ����ִ�����
					//		   37����ҽʦId   38����ҽʦname  39��ʼʱ��      40����ʱ��     41��������
					//		   42����ʱ��     43¼����Ա����  44¼����Ա����  45�����ID     46���ʱ��       
					//		   47DCԭ�����   48DCԭ������    49DCҽʦ����    50DCҽʦ����   51Dcʱ��
					//         52ִ����Ա���� 53ִ��ʱ��      54ִ�п��Ҵ���  55ִ�п������� 
					//		   56���ηֽ�ʱ�� 57�´ηֽ�ʱ��
					try
					{						  
						objOrder.ReciptDoctor.ID = this.Reader[37].ToString();
						objOrder.ReciptDoctor.Name = this.Reader[38].ToString();
						//try{
						if(!this.Reader.IsDBNull(39))objOrder.BeginTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[39].ToString());
						//}
						//catch{}
						//try{
						if(!this.Reader.IsDBNull(40))objOrder.EndTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[40].ToString());
						//}
						//catch{}
						objOrder.ReciptDept.ID = this.Reader[41].ToString();//InDept.ID
						//try{
						if(!this.Reader.IsDBNull(42))objOrder.MOTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[42].ToString());
						//}
						//catch{}
						objOrder.Oper.ID = this.Reader[43].ToString();
						objOrder.Oper.Name = this.Reader[44].ToString();
						objOrder.Nurse.ID = this.Reader[45].ToString();
						//try{
						if(!this.Reader.IsDBNull(46))objOrder.ConfirmTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[46].ToString());
						//}
						//catch{}
						objOrder.DcReason.ID = this.Reader[47].ToString();
						objOrder.DcReason.Name = this.Reader[48].ToString();
						objOrder.DCOper.ID = this.Reader[49].ToString();
						objOrder.DCOper.Name = this.Reader[50].ToString();
						//try{
						if(!this.Reader.IsDBNull(51))objOrder.DCOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[51].ToString());
						//}
						//catch{}
						objOrder.ExecOper.ID = this.Reader[52].ToString();
						//try{
						if(!this.Reader.IsDBNull(53))objOrder.ExecOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[53].ToString());
						
						objOrder.ExeDept.ID = this.Reader[54].ToString();
						objOrder.ExeDept.Name = this.Reader[55].ToString();

                        objOrder.ExecOper.Dept.ID = this.Reader[54].ToString();
                        objOrder.ExecOper.Dept.Name = this.Reader[55].ToString();
						
						if(!this.Reader.IsDBNull(56))objOrder.CurMOTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[56].ToString());
					
						if(!this.Reader.IsDBNull(57))objOrder.NextMOTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[57].ToString());
						
					}
					catch(Exception ex)
					{
						this.Err="���ҽ��ִ�������Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					#endregion
					#region "ҽ������"
					// ����ҽ������
					//		   58�Ƿ�Ӥ����1��/2��          59�������  	  60������     61��ҩ��� 
					//		   62�Ƿ񸽲�'1'  63�Ƿ��������  64ҽ��״̬      65�ۿ���     66ִ�б��1δִ��/2��ִ��/3DCִ�� 
					//		   67ҽ��˵��                     68�Ӽ����:1��ͨ/2�Ӽ�         69�������
					//         70 ��ע       ,       71��鲿λ��ע    ,72 �������,74 ȡҩҩ������ 81 �Ƿ�Ƥ�� 
                    //         84 ���뵥��
					try
					{
						
						if(!this.Reader.IsDBNull(58))objOrder.IsBaby = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[58].ToString()));
						
						if(!this.Reader.IsDBNull(59))objOrder.BabyNO = this.Reader[59].ToString();
						
						objOrder.Combo.ID = this.Reader[60].ToString();
						
						if(!this.Reader.IsDBNull(61))objOrder.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[61].ToString()));
						
						if(!this.Reader.IsDBNull(62))objOrder.IsSubtbl = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[62].ToString()));
						
						if(!this.Reader.IsDBNull(63))objOrder.IsHaveSubtbl = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[63].ToString()));
						
						if(!this.Reader.IsDBNull(64))objOrder.Status = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[64].ToString());
						
						if(!this.Reader.IsDBNull(65))objOrder.IsStock = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[65].ToString()));

						
						if(!this.Reader.IsDBNull(66))objOrder.ExecStatus = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[66].ToString());
						
						objOrder.Note = this.Reader[67].ToString();
						
						if(!this.Reader.IsDBNull(68))objOrder.IsEmergency = Neusoft.FrameWork.Function.NConvert.ToBoolean(Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[68].ToString()));
						
						if(!this.Reader.IsDBNull(69))objOrder.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[69]);
						
						objOrder.Memo = this.Reader[70].ToString();
						objOrder.CheckPartRecord = this.Reader[71].ToString();
						objOrder.Reorder = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[72].ToString());
						objOrder.StockDept.ID = this.Reader[74].ToString();//ȡҩҩ������
						try
						{
							if(!this.Reader.IsDBNull(75)) objOrder.IsPermission = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[75]);//������ҩ֪��
						}
						catch{}
						objOrder.Package.ID = this.Reader[76].ToString();
						objOrder.Package.Name = this.Reader[77].ToString();
						objOrder.ExtendFlag2 = this.Reader[79].ToString();
						objOrder.ExtendFlag3 = this.Reader[80].ToString();
						try
						{
							if (!this.Reader.IsDBNull(81))
								objOrder.HypoTest = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[81].ToString());//1 ����Ƥ�� 2 ��Ƥ�� 3 �� 4 ��
						}
						catch
						{
							objOrder.HypoTest = 1;
						}
                        
                        objOrder.Frequency.Time = this.Reader[82].ToString(); //ִ��ʱ��
                        objOrder.ExecDose = this.Reader[83].ToString();//����Ƶ�μ���
                        if (!this.Reader.IsDBNull(84))  objOrder.ApplyNo = this.Reader[84].ToString(); //���뵥��

                        #region {16EE9720-A7C1-4f07-8262-2F0A1567C78F}
                        if (!this.Reader.IsDBNull(85))
                        {
                            objOrder.DCNurse.ID = this.Reader[85].ToString();
                        }
                        if (!this.Reader.IsDBNull(86))
                        {
                            objOrder.DCNurse.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[86].ToString());
                        }
                        #endregion

					}
					catch(Exception ex)
					{
						this.Err="���ҽ��������Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					#endregion
					al.Add(objOrder);
				}
			}
			catch(Exception ex)
			{
				this.Err="���ҽ����Ϣ����"+ex.Message;
				this.ErrCode="-1";
				this.WriteErr();
				return null;
			}
			this.Reader.Close();
			return al;
		}
		/// <summary>
		/// �ж�ҽ����Ŀ���ҩƷ/��ҩƷ
		/// </summary>
		/// <param name="Order"></param>
		/// <returns></returns>
        private string JudgeItemType(Neusoft.HISFC.Models.Order.Order Order)
		{
			string strItem="";
			//�ж�ҩƷ/��ҩƷ 
			//if (Order.Item.IsPharmacy)
            if(Order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
			{
				strItem="1";
			}
			else 
			{
				strItem="2";
			}
			return strItem;
		}

		
		//��Ӳ�ѯ��Ϣ
		private void addExecOrder(ArrayList al,string sqlOrder)
		{
			ArrayList al1=null;
			try
			{
				al1=this.myExecOrderQuery(sqlOrder);
				//al1 = myExecOrderQueryByDataSet(sqlOrder);
			}
			catch(Exception ex)
			{
				this.Err = ex.Message;
			}
			if(al1 == null) return;
			
			al.AddRange(al1);
			//			foreach(Object.Order.ExecOrder ExecOrder in al1)
			//			{
			//				al.Add(ExecOrder);
			//			}
		}
		
		/// <summary>
		/// ��ѯ������Ϣ��select��䣨��where������
		/// </summary>
		/// <param name="Type">"" ҩƷ��ҩƷ���飬1 ҩƷ�� 2 ��ҩƷ</param>
		/// <returns></returns>
		private string[] ExecOrderQuerySelect(string Type)
		{
			#region �ӿ�˵��
			//Order.ExecOrder.QueryOrder.Select.1
			//���룺0
			//������sql.select
			#endregion
			string[] s = null;
			string sql="";
			if(Type =="") 
			{
				s = new string[2];
			}
			else
			{
				s = new string[1];
			}
			if (Type=="1" || Type =="")
			{
				if(this.Sql.GetSql("Order.ExecOrder.QueryOrder.Select.1",ref sql)==-1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryOrder.Select.1�ֶ�!";
					this.ErrCode="-1";
					this.WriteErr();
					return null;
				}
				s[0]=sql;
			}
			else if (Type=="2" || Type =="")
			{
				if(this.Sql.GetSql("Order.ExecOrder.QueryOrder.Select.2",ref sql)==-1)
				{
					this.Err="û���ҵ�Order.ExecOrder.QueryOrder.Select.2�ֶ�!";
					this.ErrCode="-1";
					this.WriteErr();
					return null;
				}
				if(Type =="") 
				{
					s[1] = sql;
				}
				else
				{
					s[0] = sql;
				}
			}
			return s;
		}

		private ArrayList myExecOrderQueryByDataSet(string sql)
		{
			DataSet ds = new DataSet();
			if(this.ExecQuery(sql, ref ds) == -1)
			{
				return null;
			}
			if(ds == null)
			{
				return null;
			}
			if(ds.Tables[0] == null)
			{
				return null;
			}

			ArrayList al = new ArrayList();

            Neusoft.HISFC.Models.Order.ExecOrder objOrder;
	
			foreach(DataRow row in ds.Tables[0].Rows)
			{
				objOrder = new Neusoft.HISFC.Models.Order.ExecOrder();

				objOrder.Order.Patient.ID = row[0].ToString();
				objOrder.Order.Patient.PID.PatientNO = row[1].ToString();
				objOrder.Order.Patient.PVisit.PatientLocation.Dept.ID = row[3].ToString(); 
				objOrder.Order.Patient.PVisit.PatientLocation.NurseCell.ID = row[4].ToString(); 
				objOrder.Order.NurseStation.ID = row[4].ToString();
				objOrder.Order.InDept.ID=row[3].ToString();

				if (row[5].ToString() == "1")//ҩƷ
				{
					Neusoft.HISFC.Models.Pharmacy.Item objPharmacy= new Neusoft.HISFC.Models.Pharmacy.Item();

					#region ҩƷ
					objPharmacy.ID = row[6].ToString();
					objPharmacy.Name = row[7].ToString();
					objPharmacy.UserCode = row[8].ToString();
					objPharmacy.SpellCode = row[9].ToString();
					objPharmacy.SysClass.ID = row[10].ToString();
					//objPharmacy.SysClass.Name = row[11].ToString();
					objPharmacy.Specs     = row[12].ToString();
					//try
					//{
					objPharmacy.BaseDose  = Neusoft.FrameWork.Function.NConvert.ToDecimal(row[13]);
					//}
					//catch{}
					objPharmacy.DoseUnit = row[14].ToString();
					objPharmacy.MinUnit = row[15].ToString();
					//try
					//{
					objPharmacy.PackQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(row[16]);
					//}
					//catch{}
					objPharmacy.DosageForm.ID = row[17].ToString();
					objPharmacy.Type.ID    = row[18].ToString();
					objPharmacy.Quality.ID = row[19].ToString();
					//try
					//{
					objPharmacy.PriceCollection.RetailPrice = Neusoft.FrameWork.Function.NConvert.ToDecimal(row[20]);
					//}
					//catch{}	
					objOrder.Order.Item = objPharmacy;
					

					objOrder.Order.Usage.ID  =row[21].ToString();
					objOrder.Order.Usage.Name=row[22].ToString();
					objOrder.Order.Usage.Memo=row[23].ToString();
					objOrder.Order.Frequency.ID  =row[24].ToString();
					objOrder.Order.Frequency.Name=row[25].ToString();
					//try
					//{
					objOrder.Order.DoseOnce=Neusoft.FrameWork.Function.NConvert.ToDecimal(row[26]);
					//}
					//catch{}
					objOrder.Order.DoseUnit =objPharmacy.DoseUnit;
					//try
					//{
					objOrder.Order.Qty =Neusoft.FrameWork.Function.NConvert.ToDecimal(row[27]);
					//}
					//catch{}
					objOrder.Order.Unit=row[28].ToString();
					//try
					//{
					objOrder.Order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(row[29]);

					objOrder.ID = row[0].ToString();
					objOrder.Order.ID = row[30].ToString();
					objOrder.Order.OrderType.ID = row[31].ToString();
					objOrder.Order.OrderType.IsDecompose = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[32]);
					objOrder.Order.OrderType.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[33]);
					objOrder.Order.OrderType.IsNeedPharmacy = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[34]);
					objOrder.Order.OrderType.IsPrint = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[35]);
					objOrder.Order.OrderType.IsConfirm =  Neusoft.FrameWork.Function.NConvert.ToBoolean(row[36]);
					objOrder.Order.ReciptDoctor.ID = row[37].ToString();
					objOrder.Order.ReciptDoctor.Name = row[38].ToString();
					objOrder.DateUse =  Neusoft.FrameWork.Function.NConvert.ToDateTime(row[39]);
					objOrder.DCExecOper.OperTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(row[40]);
					objOrder.Order.ReciptDept.ID = row[41].ToString();
					objOrder.Order.BeginTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(row[42]);
					objOrder.DCExecOper.ID = row[43].ToString();
					objOrder.ChargeOper.ID = row[44].ToString();
                    objOrder.ChargeOper.Dept.ID = row[45].ToString();
					objOrder.ChargeOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[46]);
					objOrder.Order.StockDept.ID = row[47].ToString();
					objOrder.Order.ExeDept.ID = row[48].ToString();
					objOrder.ExecOper.ID = row[49].ToString();

					if(row[50].ToString()!="") 
						objOrder.Order.ExeDept.ID = row[50].ToString();
					objOrder.Order.BeginTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(row[51]);
					objOrder.DateDeco =  Neusoft.FrameWork.Function.NConvert.ToDateTime(row[52]);
					objOrder.Order.IsBaby = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[53]);
					objOrder.Order.BabyNO = row[54].ToString();
					objOrder.Order.Combo.ID = row[55].ToString();
					objOrder.Order.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[56]);
					objOrder.Order.IsHaveSubtbl =Neusoft.FrameWork.Function.NConvert.ToBoolean(row[57]);
					objOrder.IsValid= Neusoft.FrameWork.Function.NConvert.ToBoolean(row[58]);
					objOrder.Order.IsStock = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[59]);
					objOrder.IsExec = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[60]);
					objOrder.DrugFlag = Neusoft.FrameWork.Function.NConvert.ToInt32(row[61]);
					objOrder.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[62]);
					objOrder.Order.Note = row[63].ToString();
					objOrder.Order.Memo = row[64].ToString();
					objOrder.Order.ReciptNO = row[65].ToString();
					objOrder.Order.SequenceNO =Neusoft.FrameWork.Function.NConvert.ToInt32(row[66]);
					#endregion
				}
				if (row[5].ToString() == "2")//��ҩƷ
				{
					Neusoft.HISFC.Models.Fee.Item.Undrug objAssets=new Neusoft.HISFC.Models.Fee.Item.Undrug();
					#region ��ҩƷ
						
					// ������Ŀ��Ϣ
					//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
					//	       10��Ŀ������ 11��Ŀ�������  12���         13���ۼ�        14�÷�����   
					//         15�÷�����     16�÷�Ӣ����д  17Ƶ�δ���     18Ƶ������      19ÿ������
					//         20��Ŀ����     21�Ƽ۵�λ      22ʹ�ô���	
					objAssets.ID = row[6].ToString();
					objAssets.Name = row[7].ToString();
					objAssets.UserCode = row[8].ToString();
					objAssets.SpellCode = row[9].ToString();
					objAssets.SysClass.ID = row[10].ToString();
					//objAssets.SysClass.Name = row[11].ToString();
					objAssets.Specs     = row[12].ToString();
					objAssets.Price= Neusoft.FrameWork.Function.NConvert.ToDecimal(row[13].ToString());
					objAssets.PriceUnit = row[21].ToString();
					objOrder.Order.Item = objAssets;								
					objOrder.Order.Usage.ID  =row[14].ToString();
					objOrder.Order.Usage.Name=row[15].ToString();
					objOrder.Order.Usage.Memo=row[16].ToString();
					objOrder.Order.Frequency.ID  =row[17].ToString();
					objOrder.Order.Frequency.Name=row[18].ToString();
					objOrder.Order.DoseOnce=Neusoft.FrameWork.Function.NConvert.ToDecimal(row[19]);
					objOrder.Order.Qty =Neusoft.FrameWork.Function.NConvert.ToDecimal(row[20]);
					objOrder.Order.Unit =row[21].ToString();
					objOrder.Order.DoseUnit = objOrder.Order.Unit;
					objOrder.Order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(row[22]);

					objOrder.ID = row[0].ToString();
					objOrder.Order.OrderType.ID = row[23].ToString();
					objOrder.Order.ID = row[24].ToString();
					objOrder.Order.OrderType.IsDecompose = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[25]);
					objOrder.Order.OrderType.IsCharge =Neusoft.FrameWork.Function.NConvert.ToBoolean(row[26]);
					objOrder.Order.OrderType.IsNeedPharmacy = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[27]);
					objOrder.Order.OrderType.IsPrint = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[28]);
					objOrder.Order.OrderType.IsConfirm = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[29]);
					objOrder.Order.ReciptDoctor.ID = row[30].ToString();
					objOrder.Order.ReciptDoctor.Name = row[31].ToString();
					objOrder.DateUse = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[32]);
					objOrder.DCExecOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[33]);
					objOrder.Order.ReciptDept.ID = row[34].ToString();
					objOrder.Order.BeginTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[35]);
					objOrder.DCExecOper.ID = row[36].ToString();
					objOrder.ChargeOper.ID = row[37].ToString();
					objOrder.ChargeOper.Dept.ID = row[38].ToString();
					objOrder.ChargeOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[39]);
					objOrder.Order.StockDept.ID = row[40].ToString();
					objOrder.Order.ExeDept.ID = row[41].ToString();
					objOrder.ExecOper.ID = row[42].ToString();
					objOrder.Order.ExeDept.ID = row[43].ToString();
					objOrder.ExecOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[44]);
					objOrder.DateDeco = Neusoft.FrameWork.Function.NConvert.ToDateTime(row[45]);
					objOrder.Order.ExeDept.Name = row[46].ToString();
					objOrder.Order.IsBaby = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[47]);
					objOrder.Order.BabyNO = row[48].ToString();
					objOrder.Order.Combo.ID = row[49].ToString();
					objOrder.Order.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[50]);
					objOrder.Order.IsSubtbl = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[51]);
					objOrder.Order.IsHaveSubtbl =Neusoft.FrameWork.Function.NConvert.ToBoolean(row[52]);
					objOrder.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[53]);
					objOrder.IsExec =Neusoft.FrameWork.Function.NConvert.ToBoolean(row[54]);
					objOrder.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[55]);
					objOrder.Order.IsEmergency = Neusoft.FrameWork.Function.NConvert.ToBoolean(row[56]);
					objOrder.Order.CheckPartRecord = row[57].ToString();

					objOrder.Order.Note = row[58].ToString();
					objOrder.Order.Memo = row[59].ToString();
					objOrder.Order.ReciptNO = row[60].ToString();
					objOrder.Order.SequenceNO = Neusoft.FrameWork.Function.NConvert.ToInt32(row[61]);
					objOrder.Order.StockDept.ID = row[62].ToString();
					try
					{
						objOrder.Order.Sample.Name = row[63].ToString();			//��������/��鲿λ
						objOrder.Order.Sample.Memo = row[64].ToString();			//���������
					}
					catch {}
					#endregion
				}

				al.Add(objOrder);
			}
			return al;
		}

		//˽�к�������ѯҽ����Ϣ
		private ArrayList myExecOrderQuery(string SQLPatient)
		{
			
			ArrayList al=new ArrayList();

			if(this.ExecQuery(SQLPatient)==-1) return null;
			try
			{
				while (this.Reader.Read())
				{
                    Neusoft.HISFC.Models.Order.ExecOrder objOrder = new Neusoft.HISFC.Models.Order.ExecOrder();
					    
					#region "������Ϣ"
					//������Ϣ����  
					//			1 סԺ��ˮ��   2סԺ������     3���߿���id      4���߻���id
					try
					{
						objOrder.Order.Patient.ID =this.Reader[1].ToString();
						objOrder.Order.Patient.PID.PatientNO = this.Reader[2].ToString(); 
						objOrder.Order.Patient.PVisit.PatientLocation.Dept.ID = this.Reader[3].ToString(); 
						objOrder.Order.Patient.PVisit.PatientLocation.NurseCell.ID = this.Reader[4].ToString(); 
						objOrder.Order.NurseStation.ID = this.Reader[4].ToString();
						objOrder.Order.InDept.ID=this.Reader[3].ToString();
					}
					catch(Exception ex)
					{
						this.Err="��û��߻�����Ϣ����"+ex.Message;
						this.WriteErr();
						return null;
					}
					#endregion
					  
					if (this.Reader[5].ToString() == "1")//ҩƷ
					{
						Neusoft.HISFC.Models.Pharmacy.Item objPharmacy= new Neusoft.HISFC.Models.Pharmacy.Item();
						try
						{
							#region "��Ŀ��Ϣ"
							//ҽ��������Ϣ
							// ������Ŀ��Ϣ
							//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
							//	       10��Ŀ������ 11��Ŀ�������  12ҩƷ���     13ҩƷ��������  14������λ       
							//         15��С��λ     16��װ����,     17���ʹ���     18ҩƷ���  ,   19ҩƷ����
							//         20���ۼ�       21�÷�����      22�÷�����     23�÷�Ӣ����д  24Ƶ�δ���  
							//         25Ƶ������     26ÿ�μ���      27��Ŀ����     28�Ƽ۵�λ      29ʹ������			
							objPharmacy.ID = this.Reader[6].ToString();
							objPharmacy.Name = this.Reader[7].ToString();
							objPharmacy.UserCode = this.Reader[8].ToString();
							objPharmacy.SpellCode = this.Reader[9].ToString();
							objPharmacy.SysClass.ID = this.Reader[10].ToString();
							//objPharmacy.SysClass.Name = this.Reader[11].ToString();
							objPharmacy.Specs     = this.Reader[12].ToString();
							//try
							//{
							objPharmacy.BaseDose  = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[13]);
							//}
							//catch{}
							objPharmacy.DoseUnit = this.Reader[14].ToString();
							objPharmacy.MinUnit = this.Reader[15].ToString();
							//try
							//{
							objPharmacy.PackQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[16]);
							//}
							//catch{}
							objPharmacy.DosageForm.ID = this.Reader[17].ToString();
							objPharmacy.Type.ID    = this.Reader[18].ToString();
							objPharmacy.Quality.ID = this.Reader[19].ToString();
							//try
							//{
							objPharmacy.PriceCollection.RetailPrice = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[20]);
							//}
							//catch{}	
							objOrder.Order.Item = objPharmacy;
							#endregion

							objOrder.Order.Usage.ID  =this.Reader[21].ToString();
							objOrder.Order.Usage.Name=this.Reader[22].ToString();
							objOrder.Order.Usage.Memo=this.Reader[23].ToString();
							objOrder.Order.Frequency.ID  =this.Reader[24].ToString();
							objOrder.Order.Frequency.Name=this.Reader[25].ToString();
							//try
							//{
							objOrder.Order.DoseOnce=Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[26]);
							//}
							//catch{}
							objOrder.Order.DoseUnit =objPharmacy.DoseUnit;
							//try
							//{
							objOrder.Order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[27]);
							//}
							//catch{}
							objOrder.Order.Unit=this.Reader[28].ToString();
							//try
							//{
							objOrder.Order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[29]);
							//}
							//catch{}
						}
						catch(Exception ex)
						{
							this.Err="���ҽ����Ŀ��Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						//objOrder.Order.Item = objPharmacy;

						#region "ҽ������"
						// ����ҽ������
						//		  30ҽ����ˮ�� , 31ҽ��������  32ҽ���Ƿ�ֽ�:1����/2��ʱ     33�Ƿ�Ʒ� 
						//		   34ҩ���Ƿ���ҩ 35��ӡִ�е�    36�Ƿ���Ҫȷ��  
						try
						{
							objOrder.ID = this.Reader[0].ToString();
							objOrder.Order.ID = this.Reader[30].ToString();
							objOrder.Order.OrderType.ID = this.Reader[31].ToString();
							//try
							//{
							objOrder.Order.OrderType.IsDecompose = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[32]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[33]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsNeedPharmacy = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[34]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsPrint = Neusoft.FrameWork.Function.NConvert.ToBoolean(Reader[35]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsConfirm =  Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[36]);
							//}
							//catch{}
						}
						catch(Exception ex)
						{
							this.Err="���ҽ��������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
						#region "ִ�����"
						// ����ִ�����
						//		   37����ҽʦId   38����ҽʦname  39Ҫ��ִ��ʱ��  40����ʱ��     41��������
						//		   42����ʱ��     43�����˴���    44�����˴���    45���˿��Ҵ��� 46����ʱ��       
						//		   47ȡҩҩ��     48ִ�п���      49ִ�л�ʿ����  50ִ�п��Ҵ��� 51ִ��ʱ��
						//         52�ֽ�ʱ��
						try
						{						  
							objOrder.Order.ReciptDoctor.ID = this.Reader[37].ToString();
							objOrder.Order.ReciptDoctor.Name = this.Reader[38].ToString();
							//try{
							objOrder.DateUse =  Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[39]);
							//}
							//catch{}
							//try{
							objOrder.DCExecOper.OperTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[40]);
							//}
							//catch{}
							objOrder.Order.ReciptDept.ID = this.Reader[41].ToString();
							//try{
							objOrder.Order.BeginTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[42]);
							//}
							//catch{}
							objOrder.DCExecOper.ID = this.Reader[43].ToString();
							objOrder.ChargeOper.ID = this.Reader[44].ToString();
							objOrder.ChargeOper.Dept.ID = this.Reader[45].ToString();
							//try{
							objOrder.ChargeOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[46]);
							//}
							//catch{}
							objOrder.Order.StockDept.ID = this.Reader[47].ToString();
							objOrder.Order.ExeDept.ID = this.Reader[48].ToString();
							objOrder.ExecOper.ID = this.Reader[49].ToString();
							//try
							//{
							if(this.Reader[50].ToString()!="") 
								objOrder.Order.ExeDept.ID = this.Reader[50].ToString();
							//}
							//catch{}
							//try{
							objOrder.ExecOper.OperTime =  Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[51]);
							//}
							//catch{}
							objOrder.DateDeco =  Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[52]);

							if(!Reader.IsDBNull(68))
							{
								objOrder.DrugedTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[68].ToString());
							}
						
						}
						catch(Exception ex)
						{
							this.Err="���ҽ��ִ�������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
						#region "ҽ������"
						// ����ҽ������
						//		   64�Ƿ�Ӥ����1��/2��          53�������  	  54������     55��ҩ��� 
						//		   56�Ƿ��������                 57�Ƿ���Ч      58�ۿ���     59�Ƿ�ִ�� 
						//		   60��ҩ���                     61�շѱ��      62ҽ��˵��     63��ע
						//         65������                       66�������
						try
						{
							//try{
							objOrder.Order.IsBaby = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[53]);
							//}
							//catch{}
							//try{
							objOrder.Order.BabyNO = this.Reader[54].ToString();
							//}
							//catch{}
							objOrder.Order.Combo.ID = this.Reader[55].ToString();
							//try{
							objOrder.Order.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[56]);
							//}
							//catch{}
							//try{
							objOrder.Order.IsHaveSubtbl =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[57]);
							//}
							//catch{}
							//try{
							objOrder.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[58]);
							//}
							//catch{}
							//try{
							objOrder.Order.IsStock = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[59]);
							//}
							//catch{}
							//try{
							objOrder.IsExec = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[60]);
							//}
							//catch{}
							//try{
							objOrder.DrugFlag = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[61]);
							//}
							//catch{}
							//try{
							objOrder.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[62]);
							//}
							//catch{}
							objOrder.Order.Note = this.Reader[63].ToString();
							objOrder.Order.Memo = this.Reader[64].ToString();
							objOrder.Order.ReciptNO = this.Reader[65].ToString();
							//try{
							objOrder.Order.SequenceNO =Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[66]);
							//}
							//catch{}
						}
						catch(Exception ex)
						{
							this.Err="���ҽ��������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
					} 	
					else if (this.Reader[5].ToString() == "2")//��ҩƷ
					{
						Neusoft.HISFC.Models.Fee.Item.Undrug objAssets=new Neusoft.HISFC.Models.Fee.Item.Undrug();
						try
						{
							#region "��Ŀ��Ϣ"
							// ������Ŀ��Ϣ
							//	       5��Ŀ���      6��Ŀ����       7��Ŀ����      8��Ŀ������,    9��Ŀƴ���� 
							//	       10��Ŀ������ 11��Ŀ�������  12���         13���ۼ�        14�÷�����   
							//         15�÷�����     16�÷�Ӣ����д  17Ƶ�δ���     18Ƶ������      19ÿ������
							//         20��Ŀ����     21�Ƽ۵�λ      22ʹ�ô���	
							objAssets.ID = this.Reader[6].ToString();
							objAssets.Name = this.Reader[7].ToString();
							objAssets.UserCode = this.Reader[8].ToString();
							objAssets.SpellCode = this.Reader[9].ToString();
							objAssets.SysClass.ID = this.Reader[10].ToString();
							//objAssets.SysClass.Name = this.Reader[11].ToString();
							objAssets.Specs     = this.Reader[12].ToString();
							//try
							//{
							objAssets.Price= Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[13].ToString());
							//}
							//catch{}	
							objAssets.PriceUnit = this.Reader[21].ToString();
							objOrder.Order.Item = objAssets;	
							#endregion

							objOrder.Order.Usage.ID  =this.Reader[14].ToString();
							objOrder.Order.Usage.Name=this.Reader[15].ToString();
							objOrder.Order.Usage.Memo=this.Reader[16].ToString();
							objOrder.Order.Frequency.ID  =this.Reader[17].ToString();
							objOrder.Order.Frequency.Name=this.Reader[18].ToString();
							//try
							//{
							objOrder.Order.DoseOnce=Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[19]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.Qty =Neusoft.FrameWork.Function.NConvert.ToDecimal(this.Reader[20]);
							//}
							//catch{}
							objOrder.Order.Unit =this.Reader[21].ToString();
							objOrder.Order.DoseUnit = objOrder.Order.Unit;
							//try
							//{
							objOrder.Order.HerbalQty = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[22]);
							//}
							//catch{}
						}
						catch(Exception ex)
						{
							this.Err="���ҽ����Ŀ��Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						
						#region "ҽ������"
						// ����ҽ������
						//		   23ҽ�������� 24ҽ����ˮ��    25ҽ���Ƿ�ֽ�:1����/2��ʱ     26�Ƿ�Ʒ� 
						//		   27ҩ���Ƿ���ҩ 28��ӡִ�е�    29�Ƿ���Ҫȷ��    
						try
						{
							objOrder.ID = this.Reader[0].ToString();
							objOrder.Order.OrderType.ID = this.Reader[23].ToString();
							objOrder.Order.ID = this.Reader[24].ToString();
							//							try
							//							{
							objOrder.Order.OrderType.IsDecompose = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[25]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsCharge =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[26]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsNeedPharmacy = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[27]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsPrint = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[28]);
							//}
							//catch{}
							//try
							//{
							objOrder.Order.OrderType.IsConfirm = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[29]);
							//}
							//catch{}
						}
						catch(Exception ex)
						{
							this.Err="���ҽ��������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
						#region "ִ�����"
						// ����ִ�����
						//		   30����ҽʦId   31����ҽʦname  32Ҫ��ִ��ʱ��  33����ʱ��     34��������
						//		   35����ʱ��     36�����˴���    37�����˴���    38���˿��Ҵ��� 39����ʱ��       
						//		   40ȡҩҩ��     41ִ�п���      42ִ�л�ʿ����  43ִ�п��Ҵ��� 44ִ��ʱ��
						//         45�ֽ�ʱ��     46ִ�п�������
						try
						{						  
							objOrder.Order.ReciptDoctor.ID = this.Reader[30].ToString();
							objOrder.Order.ReciptDoctor.Name = this.Reader[31].ToString();
							//try{
							objOrder.DateUse = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[32]);
							//}
							//catch{}
							//try{
							objOrder.DCExecOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[33]);
							//}
							//catch{}
							objOrder.Order.ReciptDept.ID = this.Reader[34].ToString();
							//try{
							objOrder.Order.BeginTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[35]);
							//}
							//catch{}
							objOrder.DCExecOper.ID = this.Reader[36].ToString();
							objOrder.ChargeOper.ID = this.Reader[37].ToString();
                            objOrder.ChargeOper.Dept.ID = this.Reader[38].ToString();
							//try{
							objOrder.ChargeOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[39]);
							//}
							//catch{}
							objOrder.Order.StockDept.ID = this.Reader[40].ToString();
							objOrder.Order.ExeDept.ID = this.Reader[41].ToString();//ִ�п�������Ŀִ�п���
							objOrder.ExecOper.ID = this.Reader[42].ToString();
							//objOrder.ExeDept.ID = this.Reader[43].ToString();//����ֶξ���û�õ�
							//try{
							objOrder.ExecOper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[44]);
							//}
							//catch{}
							objOrder.DateDeco = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[45]);
							objOrder.Order.ExeDept.Name = this.Reader[46].ToString();
						
						}
						catch(Exception ex)
						{
							this.Err="���ҽ��ִ�������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
						#region "ҽ������"
						// ����ҽ������
						//		   47�Ƿ�Ӥ����1��/2��          48�������  	  49������     50������ 
						//		   51�Ƿ񸽲�                     52�Ƿ��������  53�Ƿ���Ч     54�Ƿ�ִ�� 
						//		   55�շѱ��     56�Ӽ����      57��鲿λ����  58ҽ��˵��     59��ע 
						//         60������                       61�������      62��ҩ����ID
						try
						{
							//try{
							objOrder.Order.IsBaby = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[47]);
							//}
							//catch{}
							//try{
							objOrder.Order.BabyNO = this.Reader[48].ToString();
							//}
							//catch{}
							objOrder.Order.Combo.ID = this.Reader[49].ToString();
							//try{
							objOrder.Order.Combo.IsMainDrug = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[50]);
							//}
							//catch{}
							//try{
							objOrder.Order.IsSubtbl = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[51]);
							//}
							//catch{}
							//try{
							objOrder.Order.IsHaveSubtbl =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[52]);
							//}
							//catch{}
							//try{
							objOrder.IsValid = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[53]);
							//}
							//catch{}
							//try{
							objOrder.IsExec =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[54]);
							//}
							//catch{}
							//try{
							objOrder.IsCharge = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[55]);
							//}
							//catch{}
							//try{
							objOrder.Order.IsEmergency = Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[56]);
							//}
							//catch{}
							objOrder.Order.CheckPartRecord = this.Reader[57].ToString();

							objOrder.Order.Note = this.Reader[58].ToString();
							objOrder.Order.Memo = this.Reader[59].ToString();
							objOrder.Order.ReciptNO = this.Reader[60].ToString();
							//try{
							objOrder.Order.SequenceNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.Reader[61]);
							//}
							//catch{}
							objOrder.Order.StockDept.ID = this.Reader[62].ToString();
							
							try
							{
								objOrder.Order.Sample.Name = this.Reader[63].ToString();			//��������/��鲿λ
								objOrder.Order.Sample.Memo = this.Reader[64].ToString();			//���������
							}
							catch {}
							
							

						}
						catch(Exception ex)
						{
							this.Err="���ҽ��������Ϣ����"+ex.Message;
							this.WriteErr();
							return null;
						}
						#endregion
					}
					al.Add(objOrder);
				}
			}
			catch(Exception ex)
			{
				this.Err="���ҽ����Ϣ����"+ex.Message;
				this.ErrCode="-1";
				this.WriteErr();
				return null;
			}
			finally
			{
				this.Reader.Close();
			}
			return al;
		}
		#endregion

		#region ��ҩ����ҽ���ӿ�
		/// <summary>
		/// ִ�м�¼
		/// ����ҽ��ִ����Ϣ
		/// ��ҽ������ʹ��
		/// </summary>
		/// <param name="execOrder">ִ�е���Ϣ</param>
		/// <returns>0 success -1 fail</returns>
        public int UpdateRecordExec(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
		{
			#region ִ�м�¼
			//ִ�м�¼
			//Order.ExecOrder.RecordExec.1
			//���룺0 id��1 ִ����id,2ִ�п��ң�3ִ�п������� 4ִ��ʱ��,5ִ�б�־ 
			//������0 
			#endregion

			string strSql = "",strSqlName = "Order.ExecOrder.RecordExec.";
			string strItemType = "";

			strItemType = JudgeItemType(execOrder.Order);
			if (strItemType == "") return -1;
			strSqlName = strSqlName + strItemType;

			if(this.Sql.GetSql(strSqlName,ref strSql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
                strSql = string.Format(strSql, execOrder.ID, execOrder.Order.Oper.ID, execOrder.Order.ExeDept.ID, execOrder.Order.ExeDept.Name, execOrder.Order.ExecOper.OperTime.ToString(), Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsExec).ToString(), Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsConfirm).ToString()/*ȷ�ϱ��{DA77B01B-63DF-4559-B264-798E54F24ABB}*/);
			}
			catch
			{
				this.Err="����������ԣ�"+strSqlName;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}

		/// <summary>
		/// �շѼ�¼
		/// ����ִ��ҽ���շ��ˣ��շѱ�ǣ���Ʊ�ŵ�
		/// �Է��ÿ���ʹ��
		/// </summary>
		/// <param name="execOrder">ִ�е���Ϣ</param>
		/// <returns>0 success -1 fail</returns>
        public int UpdateChargeExec(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
		{
			#region �շѼ�¼
			//�շѼ�¼
			//Order.ExecOrder.Charge.
			//���룺0 id��1 �շ���id,2�շѿ���ID��3�շ�ʱ��,5�շѱ�־ 6 ������ 7������ˮ���
			//������0 
			#endregion
			string strSql = "",strSqlName = "Order.ExecOrder.Charge.";
			string strItemType = "";

			strItemType = JudgeItemType (execOrder.Order);
			if (strItemType == "") return -1;
			strSqlName= strSqlName + strItemType;

			if(this.Sql.GetSql(strSqlName,ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrder.ID,
					execOrder.ChargeOper.ID,execOrder.ChargeOper.Dept.ID,execOrder.ChargeOper.OperTime.ToString(),
					Neusoft.FrameWork.Function.NConvert.ToInt32(execOrder.IsCharge).ToString(),
					execOrder.Order.ReciptNO,execOrder.Order.SequenceNO);
			}
			catch(Exception ex)
			{
				this.Err = "����������ԣ�" + strSqlName + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ��ҩ��¼
		/// ��ҩ������ʹ��,����DrugFlag
		/// </summary>
		/// <param name="execOrder">ִ�е���Ϣ</param>
		/// <returns>0 success -1 fail</returns>
        public int UpdateDrugExec(Neusoft.HISFC.Models.Order.ExecOrder execOrder)
		{
			#region ��ҩ��¼
			//��ҩ��¼
			//Order.ExecOrder.DrugExec.
			//���룺0 id��1 ��ҩ״̬ 
			//������0 
			#endregion
			string strSql = "";
			string strItemType = "";

			strItemType = JudgeItemType(execOrder.Order);
			if (strItemType != "1") 
			{
                this.Err = Neusoft.FrameWork.Management.Language.Msg("�봫��ҩƷҽ��!");
				return -1;
			}

			if(this.Sql.GetSql("Order.ExecOrder.DrugExec.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql = string.Format(strSql,execOrder.ID,execOrder.DrugFlag.ToString());
			}
			catch(Exception ex)
			{
				this.Err="����������ԣ�Order.ExecOrder.DrugExec.1"+ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ����ҽ����ҩ���
		/// ��ҩ���Ľӿ�
		/// ��ҩ������ʹ��
		/// </summary>
		/// <param name="execOrderID">ִ�е�ID</param>
		/// <param name="orderNo">����ID</param>
		/// <param name="userID">����Ա</param>
		/// <param name="deptID">��ҩ����</param>
		/// <returns>-1 ʧ�� 0 �ɹ�</returns>
		public int UpdateOrderDruged( string execOrderID, string orderNo, string userID, string deptID )
		{
			string strSql = "";
			if(this.Sql.GetSql("Order.Update.ExecOrder.Druged",ref strSql) == -1) 
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql = string.Format(strSql,execOrderID,userID,deptID);
			}
			catch(Exception ex)
			{
				this.Err="����������ԣ�Order.Update.ExecOrder.Druged"+ex.Message;
				this.WriteErr();
				return -1;
			}
			if(this.ExecNoQuery(strSql) <= 0) return -1;//����ִ�е���ҩ���

			if(orderNo=="")
			{
				Neusoft.HISFC.Models.Order.ExecOrder obj = this.QueryExecOrderByExecOrderID(execOrderID,"1");//���������Ϣ
				if(obj == null) 
				{
					this.Err ="������ҩ��ǳ���"+this.Err;
					this.WriteErr();
					return -1;
				}
				return this.UpdateOrderStatus(obj.Order.ID,3);
			}
			else
			{
				return this.UpdateOrderStatus(orderNo,3);
			}
		}
		/// <summary>
		/// ����ҽ����ҩ���
		/// ��ҩ������ʹ��
		/// </summary>
		/// <param name="execOrderID">ִ�е�ID</param>
		/// <param name="userID">����Ա</param>
		/// <param name="deptID">��ҩ����</param>
		/// <returns>-1 ʧ�� 0 �ɹ�</returns>
		public int UpdateOrderDruged(string execOrderID,string userID,string deptID)
		{
			return UpdateOrderDruged(  execOrderID,"" ,userID, deptID);
		}


		/// <summary>
		/// ���Ͱ�ҩ֪ͨ\���÷�ҩ��ʽ
		/// 0���跢��/1���з���/2��ɢ����/3����ҩ
		/// ��ҩ������ʹ��
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <param name="drugFlag">0���跢��/1���з���/2��ɢ����/3����ҩ</param>
		/// <returns></returns>
		public int SetDrugFlag(string execOrderID,int drugFlag)
		{
			#region ���·��ͷ�ʽ
			//���·��ͷ�ʽ
			//Order.Order.SetDrugFlag.1
			//���룺0 OrderExecID��1 DrugFlag
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.SetDrugFlag.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrderID,drugFlag.ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.SetDrugFlag.1";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ���·���֪ͨ
		/// ��ҩ������ʹ��
		/// </summary>
		/// <param name="nurse"></param>
		/// <returns></returns>
		public int SendInformation(Neusoft.FrameWork.Models.NeuObject nurse)
		{
			#region ���·���֪ͨ
			//���·���֪ͨ
			//���룺0 nurseid��1 nursename,2������ID 3 ������������3����ʱ��
			//������0 
			#endregion
			string strSql = "";
			if(this.Sql.GetSql("Order.Order.send.insert.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,nurse.ID,nurse.Name,this.Operator.ID,this.Operator.Name,(this.GetDateTimeFromSysDateTime().Date).ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.send.insert.1";
				return -1;
			}
			if ( this.ExecNoQuery(strSql) >= 0) return 0;

			if(this.Sql.GetSql("Order.Order.send.update.1",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,nurse.ID,nurse.Name,this.Operator.ID,this.Operator.Name,(this.GetDateTimeFromSysDateTime().Date).ToString());
			}
			catch
			{
				this.Err="����������ԣ�Order.Order.send.update.1";
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSql);

		}

      
      
		#endregion

		#region "Lis��������"
		/// <summary>
		/// ����lis�����
		/// ��LIS����ʹ��
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <param name="barCode"></param>
		/// <returns></returns>
		public int UpdateExecOrderLisBarCode(string execOrderID,string barCode)
		{
			string strSql = "";	
			//Order.ExecOrder.UpdateLisBarCode
			if(this.Sql.GetSql("Order.ExecOrder.UpdateLisBarCode",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrderID,barCode);
			}
			catch
			{
				this.Err="����������ԣ�Order.ExecOrder.UpdateLisBarCode";
				this.WriteErr();
				return -1;
			}
		
			return this.ExecNoQuery(strSql);
		}
		/// <summary>
		/// ����LIS�Ѿ���ӡ
		/// ��LIS����ʹ��
		/// </summary>
		/// <param name="execOrderID"></param>
		/// <returns></returns>
		public int UpdateExecOrderLisPrint(string execOrderID)
		{
			string strSql = "";
		
			if(this.Sql.GetSql("Order.ExecOrder.UpdateLisPrinted",ref strSql) == -1)
			{
				this.Err = this.Sql.Err;
				return -1;
			}
			try
			{
				strSql=string.Format(strSql,execOrderID);
			}
			catch
			{
				this.Err="����������ԣ�UpdateExecOrderLisPrint";
				this.WriteErr();
				return -1;
			}
		
			return this.ExecNoQuery(strSql);
		}
	
		#endregion

        #region ҽ����ѯ

        /// <summary>
        /// ����ִ�п��Ҳ�ѯ��Ҫȷ����Ŀ�Ļ��ߵ����ڿ���
        /// </summary>
        /// <param name="deptID">ִ�п���</param>
        /// <returns></returns>
        public ArrayList QueryPatientDeptByConfirmDeptID(string deptID)
        {
            ArrayList alDept = new ArrayList();
            string strSQL = "";

            if (this.Sql.GetSql("Order.ExecOrder.QueryPatientDept.NeedConfirm.1", ref strSQL) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			try
			{
                strSQL = string.Format(strSQL, deptID);
			}
			catch
			{
                this.Err = "����������ԣ�Order.ExecOrder.QueryPatientDept.NeedConfirm.1";
				return null;
			}

            if (this.ExecQuery(strSQL) == -1)
            {
                return null;
            }

            try
            {
                while (this.Reader.Read())
                {
                    Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();

                    obj.ID = this.Reader[0].ToString();

                    alDept.Add(obj);
                }
            }
            catch (Exception ex)
            {
                this.Err = "��ѯ����" + ex.Message;
                this.ErrCode = "-1";
                this.WriteErr();
                return null;
            }
            this.Reader.Close();

            return alDept;
        }

        /// <summary>
        /// ����ִ�п��ҡ��������ڿ��Ҳ�ѯ��Ҫȷ����Ŀ�Ļ���
        /// </summary>
        /// <param name="confirmDept">ִ�п���</param>
        /// <param name="patientDept">�������ڿ���</param>
        /// <returns></returns>
        public ArrayList QueryPatientByConfirmDeptAndPatDept(string confirmDept,string patientDept)
        {
            ArrayList alPatient = new ArrayList();
            string strSQL = "";

            if (this.Sql.GetSql("Order.ExecOrder.QueryPatient.NeedConfirm.1", ref strSQL) == -1)
			{
				this.Err = this.Sql.Err;
				return null;
			}
			try
			{
                strSQL = string.Format(strSQL, confirmDept,patientDept);
			}
			catch
			{
                this.Err = "����������ԣ�Order.ExecOrder.QueryPatient.NeedConfirm.1";
				return null;
			}

            if (this.ExecQuery(strSQL) == -1)
            {
                return null;
            }

            try
            {
                while (this.Reader.Read())
                {
                    Neusoft.FrameWork.Models.NeuObject obj = new NeuObject();

                    obj.ID = this.Reader[0].ToString();

                    alPatient.Add(obj);
                }
            }
            catch (Exception ex)
            {
                this.Err = "��ѯ����" + ex.Message;
                this.ErrCode = "-1";
                this.WriteErr();
                return null;
            }
            this.Reader.Close();

            return alPatient;
        }

        #region {5197289A-AB55-410b-81EE-FC7C1B7CB5D7}
        /// <summary>
        /// У�鳤�ڷ�ҩƷҽ��ִ�е���ʿ�Ƿ�ֽⱣ��
        /// </summary>
        /// <param name="execOrderID">ִ�е���ˮ��</param>
        /// <returns></returns>
        public bool CheckLongUndrugIsConfirm(string execOrderID)
        {
            string strSQL = "";

            if (this.Sql.GetSql("Order.ExecOrder.LongUndrug.CheckIsConfirm.1", ref strSQL) == -1)
            {
                this.Err = this.Sql.Err;
                return false;
            }

            try
            {
                strSQL = string.Format(strSQL, execOrderID);
            }
            catch
            {
                this.Err = "����������ԣ�Order.ExecOrder.LongUndrug.CheckIsConfirm.1";
                return false;
            }

            string flag = this.ExecSqlReturnOne(strSQL, "0");

            return Neusoft.FrameWork.Function.NConvert.ToBoolean(flag);
        }
        #endregion

        #endregion

        #region ҽ������ӡ��ѯ

        /// <summary>
        /// ��ѯҽ������ӡ��ҽ��
        /// </summary>
        /// <param name="inpatientNO">סԺ��ˮ��</param>
        /// <returns></returns>
        public ArrayList QueryPrnOrder(string inpatientNO)
        {
            
            string sql = "";
            ArrayList al = new ArrayList();
            //sql = OrderQuerySelect();
            //if (sql == null) return null;
            if (this.Sql.GetSql("Order.OrderPrn.QueryOrderByPatient", ref sql) == -1)
            {
                this.Err = "û���ҵ�Order.OrderPrn.QueryOrderByPatient�ֶ�!";
                return null;
            }
            sql = string.Format(sql, inpatientNO);
            return this.myOrderQuery(sql);
        }
        #endregion

        #region 


        /// <summary>
        /// {97FA5C9D-F454-4aba-9C36-8AF81B7C9CCF} ��չҽ��
        /// ��ҽ����ˮ�Ų�ѯһ��ʱ����Ӧ���շѵ���δ�շѵ�ҽ��
        /// </summary>
        /// <param name="inpatientNo">סԺ��ˮ��</param>
        /// <param name="itemType">��Ŀ����</param>
        /// <param name="orderID">ҽ����ˮ��</param>
        /// <param name="beginDate">��ʼʱ��</param>
        /// <param name="endDate">����ʱ��</param>
        /// <returns></returns>
        public ArrayList QueryUnFeeExecOrderByOrderID(string inpatientNo, string itemType, string orderID, DateTime beginDate, DateTime endDate)
        {

            string[] s;
            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();

            s = ExecOrderQuerySelect(itemType);
            for (int i = 0; i <= s.GetUpperBound(0); i++)
            {
                sql = s[i];
                if (sql == null)
                    return null;
                if (this.Sql.GetSql("Order.ExecOrder.QueryUnFeeExecOrderByOrderID.1", ref sql1) == -1)
                {
                    this.Err = "û���ҵ�Order.ExecOrder.QueryUnFeeExecOrderByOrderID.1�ֶ�!";
                    return null;
                }
                sql = sql + " " + string.Format(sql1, inpatientNo, orderID, beginDate.ToString(), endDate.ToString());
                addExecOrder(al, sql);
            }
            return al;
        }



        #endregion

        #region ҽ������  {FB86E7D8-A148-4147-B729-FD0348A3D670} ���Ӻ���

        /// <summary>
        /// ҽ����������һ����ҽ��״̬��4����ʾ��ҽ���鵵
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public int OrderReform(string OrderID)
        {
            string strSql = "";
            if (this.Sql.GetSql("Order.Update.Reform", ref strSql) == -1)
            {
                return -1;
            }

            return this.ExecNoQuery(strSql, OrderID);
        }

        #endregion

        #region addby xuewj 2010-10-23 PDA {D81BC4C8-FDD1-42ab-93A0-56049C99DF9D}

        /// <summary>
        /// ������ִ�е�
        /// </summary>
        /// <param name="exc"></param>
        public int InsertPCCBill(Neusoft.HISFC.Models.Order.ExcBill exc)
        {
            string sql = string.Empty;

            if (Sql.GetSql("PPC.Order.InsertExecbillPPC", ref sql) == -1)
            {
                this.Err = "δ�ҵ�SQL:PPC.Order.InsertExecbillPPC";
                return -1;
            }

            try
            {
                sql = string.Format(sql,
                                exc.ExecSqn,
                                exc.InpatientNo,
                                exc.BarCode,
                                "0",
                                "0",
                                exc.UseTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                exc.ExecName,
                                exc.UseName,
                                exc.QtyTot,
                                exc.FqName,
                                exc.DoseOnce,
                                exc.DoseUnit);
            }
            catch
            {
                this.Err = "ִ�в���PDA�м�����ݳ���" + this.Err;
                return -1;
            }

            return this.ExecNoQuery(sql);
        }

        #endregion

        #region addby xuewj 2010-10-24 ��סԺ�ţ�ʱ���ѯ��Чҽ��

        /// <summary>
        /// ������ʱ���ѯҽ��
        /// </summary>
        /// <param name="inpatientNO"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ArrayList QueryValidOrder(string inpatientNO, DateTime beginTime, DateTime endTime)
        {
            string sql = "", sql1 = "";
            ArrayList al = new ArrayList();
            #region ������ʱ���ѯҽ��
            //������ʱ���ѯҽ��
            //Order.Order.QueryOrder.3
            //���룺0 inpatientno 1BeginTime 2EndTime
            //������ArrayList
            #endregion

            sql = OrderQuerySelect();
            if (sql == null) return null;
            if (this.Sql.GetSql("Order.Order.QueryOrder.7", ref sql1) == -1)
            {
                this.Err = "û���ҵ�Order.Order.QueryOrder.3�ֶ�!";
                return null;
            }
            sql = sql + " " + string.Format(sql1, inpatientNO, beginTime, endTime);
            return this.myOrderQuery(sql);
        }

        #endregion
    }
}
