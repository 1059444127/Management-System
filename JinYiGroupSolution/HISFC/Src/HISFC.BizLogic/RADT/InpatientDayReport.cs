using System;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.HISFC.BizLogic.Manager;


namespace Neusoft.HISFC.BizLogic.RADT 
{
	/// <summary>
	/// סԺ��λ�ձ�������
	/// writed by cuipeng
	/// 2005-3
	/// </summary>
	public class InpatientDayReport : Neusoft.FrameWork.Management.Database 
	{
		public InpatientDayReport() 
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        #region {32357656-B32A-4fcc-BE5D-6FA1789CD5C9} ��λ�ձ���˹���

        private string sqlSelect = @"
                                        SELECT DATE_STAT, --ͳ������
                                               DEPT_CODE, --���Ҵ��� 
                                               BED_STAND, --�����ڲ����� 
                                               BED_ADD, --�Ӵ��� 
                                               BED_FREE, --�մ���
                                               BEGINNING_NUM, --�ڳ������� 
                                               IN_NORMAL, --������Ժ�� 
                                               IN_EMERGENCY, --������Ժ�� 
                                               IN_TRANSFER, --������ת���� 
                                               IN_RETURN, --�л���Ժ���� 
                                               OUT_NORMAL, --�����Ժ�� 
                                               OUT_TRANSFER, --ת���������� 
                                               OUT_WITHDRAWAL, --��Ժ����
                                               END_NUM, --��ĩ������ 
                                               DEAD_IN24, --24Сʱ�������� 
                                               DEAD_OUT24, --24Сʱ�������� 
                                               BED_RATE, --��λʹ���� 
                                               OTHER1_NUM, --����1���� 
                                               OTHER2_NUM, --����2���� 
                                               OPER_CODE, --������ 
                                               OPER_DATE, --�������� 
                                               MARK, --��ע 
                                               Nurse_Cell_Code, --��ʿվ���� 
                                               '', --��������
                                               IN_TRANSFER_INNER, --�ڲ�ת����
                                               OUT_TRANSFER_INNER --�ڲ�ת����
                                        FROM MET_CAS_INPATIENTDAYREPORT       
                                        where DATE_STAT >= to_date('{0}', 'yyyy-mm-dd HH24:mi:ss')
                                         and DATE_STAT <= to_date('{1}', 'yyyy-mm-dd HH24:mi:ss')
                                         and (DEPT_CODE = '{2}' OR '{2}' = 'AAAA') 
                                        order by DATE_STAT desc ";

        #endregion
	
		#region סԺ�ձ����ܱ�
		/// <summary>
		/// ȡȫԺĳһ���סԺ�ձ�����
		/// </summary>
		/// <param name="dateBegin">��ʼ����</param>
		/// <param name="dateEnd">��ֹ����</param>
		/// <returns>סԺ�ձ����飬������null</returns>
		public ArrayList GetInpatientDayReportList(DateTime dateBegin, DateTime dateEnd) 
		{
			string strSQL = "";
			//string strWhere = "";
			//ȡSELECT���
			if (this.Sql.GetSql("Case.InpatientDayReport.GetInpatientDayReportList",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.GetInpatientDayReportList�ֶ�!";
				return null;
			}

			//��ʽ��SQL���
			try 
			{
				strSQL = string.Format(strSQL, dateBegin.ToString(), dateEnd.ToString(),"AAAA");
			}
			catch (Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.GetInpatientDayReportList:" + ex.Message;
				return null;
			}

			//ȡסԺ�ձ�����
			return this.myGetInpatientDayReport(strSQL);
        }

        #region {32357656-B32A-4fcc-BE5D-6FA1789CD5C9} ��λ�ձ���˹���
        /// <summary>
        /// ȡȫԺĳһ���סԺ�ձ�����
        /// </summary>
        /// <param name="dateBegin">��ʼ����</param>
        /// <param name="dateEnd">��ֹ����</param>
        /// <returns>סԺ�ձ����飬������null</returns>
        public ArrayList GetInpatientDayReportList(DateTime dateBegin, DateTime dateEnd, string deptCode)
        {
            //string strSQL = "";
            ////string strWhere = "";
            ////ȡSELECT���
            //if (this.Sql.GetSql("Case.InpatientDayReport.GetInpatientDayReportList", ref strSQL) == -1)
            //{
            //    this.Err = "û���ҵ�Case.InpatientDayReport.GetInpatientDayReportList�ֶ�!";
            //    return null;
            //}

            ////��ʽ��SQL���
            //try
            //{
            //    strSQL = string.Format(strSQL, dateBegin.ToString(), dateEnd.ToString(), deptCode);
            //}
            //catch (Exception ex)
            //{
            //    this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.GetInpatientDayReportList:" + ex.Message;
            //    return null;
            //}

            ////ȡסԺ�ձ�����
            //return this.myGetInpatientDayReport(strSQL);
            return this.myGetInpatientDayReport(string.Format(this.sqlSelect, dateBegin.ToString("yyyy-MM-dd") + " 00:00:00", dateEnd.ToString("yyyy-MM-dd") + " 00:00:00", deptCode));
        }

        public ArrayList GetInpatientDayReportList(DateTime dateStat, string deptCode)
        {
            //return this.GetInpatientDayReportList(Convert.ToDateTime(dateStat.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(dateStat.ToShortDateString() + " 23:59:59"));
            //return this.GetInpatientDayReportList(Convert.ToDateTime(dateStat.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(dateStat.AddDays(1).ToShortDateString() + " 00:00:00"), deptCode);

            //ȡסԺ�ձ�����
            return this.myGetInpatientDayReport(string.Format(this.sqlSelect, dateStat.ToString("yyyy-MM-dd") + " 00:00:00", dateStat.ToString("yyyy-MM-dd") + " 00:00:00", deptCode));
        }

        /// <summary>
        /// ��ĩ���仯�Ļ�ͬʱ����֮����ձ�����
        /// </summary>
        /// <param name="reportDetail"></param>
        /// <returns></returns>
        public int UpdateAfterDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail)
        {
            ArrayList alOldDetail = this.GetInpatientDayReportList(reportDetail.DateStat, reportDetail.ID);
            if (alOldDetail == null)
            {
                return -1;
            }
            if (alOldDetail.Count == 0)
            {
                return 1;
            }
            Neusoft.HISFC.Models.HealthRecord.InpatientDayReport oldDetail = alOldDetail[0] as Neusoft.HISFC.Models.HealthRecord.InpatientDayReport;
            //int changeNum = reportDetail.EndNum - oldDetail.EndNum;
            //if (changeNum == 0)
            //{
            //    return 1;
            //}

            ArrayList alDetail = this.GetInpatientDayReportList(reportDetail.DateStat, new DateTime(2100, 1, 1), reportDetail.ID);
            if (alDetail == null)
            {
                return -1;
            }
            alDetail.Sort(new DayReportSortByDate());
            //int changeNum = 0;
            //foreach (Neusoft.HISFC.Models.HealthRecord.InpatientDayReport tmpDetail in alDetail)
            //{
            //    if (tmpDetail.DateStat.Date == reportDetail.DateStat.Date)
            //    {
            //        continue;
            //    }
            //    changeNum = reportDetail.EndNum - tmpDetail.BeginningNum;
            //    break;
            //}
            int lastEndNum = 0;
            foreach (Neusoft.HISFC.Models.HealthRecord.InpatientDayReport detail in alDetail)
            {
                if (detail.DateStat.Date == reportDetail.DateStat.Date)
                {
                    lastEndNum = detail.EndNum;
                    continue;
                }
                int changeNum = lastEndNum - detail.BeginningNum;
                detail.BeginningNum += changeNum;
                detail.EndNum += changeNum;
                if (this.UpdateInpatientDayReport(detail) < 0)
                {
                    return -1;
                }
                lastEndNum = detail.EndNum;
            }
            return 1;
        }

        #endregion {32357656-B32A-4fcc-BE5D-6FA1789CD5C9} ��λ�ձ���˹���

        /// <summary>
		/// ȡȫԺĳһ���סԺ�ձ�����
		/// </summary>
		/// <param name="dateStat">�ձ�������</param>
		/// <returns>סԺ�ձ����飬������null</returns>
		public ArrayList GetInpatientDayReportList(DateTime dateStat) 
		{
            //return this.GetInpatientDayReportList(Convert.ToDateTime(dateStat.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(dateStat.ToShortDateString() + " 23:59:59"));
            return this.GetInpatientDayReportList(Convert.ToDateTime(dateStat.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(dateStat.AddDays(1).ToShortDateString() + " 00:00:00"));

		}

       
       


		/// <summary>
		/// ȡĳһ����ĳһ���סԺ�ձ�����
		/// </summary>
		/// <param name="dateStat">ͳ�ƴ���</param>
		/// <param name="deptCode">���ұ���</param>
		/// <param name="nurseStation">����վ����</param>
		/// <returns>סԺ�ձ�ʵ��</returns>
		public Neusoft.HISFC.Models.HealthRecord.InpatientDayReport GetInpatientDayReport(DateTime dateStat, string deptCode, string nurseStation) 
		{
			string strSQL = "";
			//ȡSELECT���
			if (this.Sql.GetSql("Case.InpatientDayReport.GetInpatientDayReport",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.GetInpatientDayReport�ֶ�!";
				return null;
			}

			string strWhere= "";
			//ȡWhere���
			if (this.Sql.GetSql("Case.InpatientDayReport.GetInpatientDayReport.ByDept",ref strWhere) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.GetInpatientDayReport.ByDept�ֶ�!";
				return null;
			}

			//��ʽ��SQL���
			try 
			{
				strSQL = string.Format(strSQL + strWhere, Convert.ToDateTime(dateStat.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(dateStat.AddDays(1).ToShortDateString()+ " 00:00:00"), deptCode, nurseStation);
			}
			catch (Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.GetdayReportBill:" + ex.Message;
				return null;
			}

			//ִ��SQL��䣬ȡסԺ�ձ�������
			ArrayList al = this.myGetInpatientDayReport(strSQL);
			if (al == null) 
			{
				this.Err = "ȡסԺ�ձ�����ʱ����" + this.Err;
				return null;
			}

			//���û���ҵ����ݣ��򷵻��½��Ķ��󣬷��򷵻���������
			if (al.Count == 0) 
				return new Neusoft.HISFC.Models.HealthRecord.InpatientDayReport();
			else
				return al[0] as Neusoft.HISFC.Models.HealthRecord.InpatientDayReport;
		}


		/// <summary>
		/// ��סԺ�ձ����ܱ��в���һ����¼
		/// </summary>
		/// <param name="dayReport">סԺ�ձ�ʵ��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int InsertInpatientDayReport(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.InpatientDayReport.InsertInpatientDayReport",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.InsertInpatientDayReport�ֶ�!";
				return -1;
			}
			try 
			{  
				string[] strParm = myGetParmInpatientDayReport( dayReport );     //ȡ�����б�
				strSQL=string.Format(strSQL, strParm);            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.InsertInpatientDayReport:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		
		
		/// <summary>
		/// ����סԺ�ձ����ܱ���һ����¼
		/// </summary>
		/// <param name="dayReport">סԺ�ձ�ʵ��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int UpdateInpatientDayReport(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.InpatientDayReport.UpdateInpatientDayReport",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.UpdateInpatientDayReport�ֶ�!";
				return -1;
			}
			try 
			{  
				string[] strParm = myGetParmInpatientDayReport( dayReport );     //ȡ�����б�
				strSQL=string.Format(strSQL, strParm);            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.UpdateInpatientDayReport:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		
		
		/// <summary>
		/// ɾ��סԺ�ձ����ܱ���һ����¼
		/// </summary>
		/// <param name="dayReportID">����¼��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int DeleteInpatientDayReport(string dayReportID) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.InpatientDayReport.DeleteInpatientDayReport",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.InpatientDayReport.DeleteInpatientDayReport�ֶ�!";
				return -1;
			}
			try 
			{  
				//����������ӵ�סԺ�ձ�������ֱ�ӷ���
				strSQL=string.Format(strSQL, dayReportID);            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.DeleteInpatientDayReport:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		

		/// <summary>
		/// �ȸ���סԺ�ձ������û���ҵ����������һ��������
		/// </summary>
		/// <returns></returns>
		public int SetInpatientDayReport(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			int parm;
			//�ȸ���סԺ�ձ�
			parm = this.UpdateInpatientDayReport(dayReport);
			if (parm == 0) 
			{
				//���û���ҵ����������һ��������
				parm = this.InsertInpatientDayReport(dayReport);
			}
			return parm;
		}
		
		Neusoft.FrameWork.Management.ExtendParam managerExtendParam = new Neusoft.FrameWork.Management.ExtendParam();

		/// <summary>
		/// �ձ���̬���£���ÿ�η�����λ�䶯��ʱ�����
		/// �ȸ���סԺ�ձ������û���ҵ��������������ձ��е���ĩ���͹̶���λ��������һ��������
		/// </summary>
		/// <returns></returns>
		public int DynamicUpdate(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			try 
			{
				int parm;
				//����סԺ�ձ�
				parm = this.UpdateInpatientDayReport(dayReport);
				//���û���ҵ��������ݣ�˵���Ǳ��յ�һ�θ��£���ȡ�����ձ�����ĩ����Ϊ�����ڳ����͹̶���λ�������ݲ������ݿ�
				if(parm == 0) 
				{		
					//ȡ����סԺ�ձ�����ĩ��
					Neusoft.HISFC.Models.HealthRecord.InpatientDayReport lastReport = this.GetInpatientDayReport(dayReport.DateStat.AddDays(-1), dayReport.ID, dayReport.NurseStation.ID);
					if(lastReport == null) return -1;
					
					//���û���ҵ��������������Ϊ������ĩ��Ϊ0
					if (lastReport.ID == "")  lastReport.EndNum = GetDeptPatientNum(dayReport.ID, dayReport.NurseStation.ID);

					//�����ڳ�����������ĩ��
					dayReport.BeginningNum = lastReport.EndNum;
					//������ĩ����������ĩ�� �� ������ĩ���������ڳ�����
					dayReport.EndNum = dayReport.EndNum + lastReport.EndNum;
				
					//ȡ�̶���λ��
					Neusoft.HISFC.Models.Base.ExtendInfo deptExt = managerExtendParam.GetComExtInfo(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT,"CASE_BED_STAND",dayReport.ID);
					if(deptExt == null) return -1;
					dayReport.BedStand = Convert.ToInt32(deptExt.NumberProperty);

					//����סԺ�ձ�����һ���¼�¼
					parm = this.InsertDayReportDetail(dayReport);
				}

				
				if (parm == -1) return parm;
				 
				//�����ձ���ϸ���뺯��
				return this.SetDayReportDetail(dayReport);
				//return parm;
			}
			catch(Exception ex) 
			{
				this.Err = ex.Message;
				return -1;
			}
		}


		/// <summary>
		/// �ձ���̬���£���ÿ�η�����λ�䶯��ʱ�����
		/// �ȸ���סԺ�ձ������û���ҵ��������������ձ��е���ĩ���͹̶���λ��������һ��������
		/// </summary>
		/// <param name="patientInfo">����ʵ��</param>
		/// <param name="type">��λ�䶯����</param>
		/// <returns></returns>
		public int DynamicUpdate(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo, string type) 
		{
			
			DateTime sysDate = Convert.ToDateTime(this.GetSysDate() + " 00:00:00");

			//���¶�̬�ձ�
			//ȡ����סԺ�ձ����ݣ���Ϊ���µ�ʵ��
			Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport = this.GetInpatientDayReport(sysDate, patientInfo.PVisit.PatientLocation.Dept.ID, patientInfo.PVisit.PatientLocation.NurseCell.ID);
			if(dayReport == null) return -1;

			
			//���ձ�ʵ�帳ֵ
			//��̬�ձ�������Ҫ��������,������ˮ��,����
			dayReport.User01 = type;
			dayReport.User02 = patientInfo.ID;
			dayReport.User03 = patientInfo.PVisit.PatientLocation.Bed.ID;
			
			//���û���ҵ����յ��ձ���¼��dayReport��û�п�������
			//�˴�ID���ܸ�ֵ����Ϊ��������ô�ID�ж��Ƿ���Ҫ�����¼�¼��dayReport.ID = patientInfo.PVisit.PatientLocation.Dept.ID;  //�ձ����ұ���
			dayReport.NurseStation.ID = patientInfo.PVisit.PatientLocation.NurseCell.ID; //�ձ���������
			dayReport.DateStat = sysDate; //�ձ�����

			switch (type.ToString()) 
			{
				case "K":
					//���䴦��������Ժ��1����ĩ������1
					//if (Type.ToString()=="K") 
				{
					dayReport.InNormal = dayReport.InNormal + 1;
					dayReport.EndNum   = dayReport.EndNum + 1;
				}
					break;

				case "RI": 
				case "RO": 
					//ת�봦��RI������ת���1����ĩ������1
					//ת������RO��ת�����Ƽ�1����ĩ������1
				{
					//ȡԭ���ҡ��ֿ�����Ϣ
                    Neusoft.HISFC.BizLogic.Manager.Department department = new Neusoft.HISFC.BizLogic.Manager.Department();
					department.SetTrans(this.Trans);

					//ȡԭ������Ϣ
					Neusoft.HISFC.Models.Base.Department oldDept = department.GetDeptmentById(patientInfo.PVisit.PatientLocation.Dept.ID);
					if (oldDept == null) 
					{
						this.Err = department.Err;
						return -1;
					}

					//ȡ�¿�����Ϣ
					Neusoft.HISFC.Models.Base.Department newDept = department.GetDeptmentById(patientInfo.PVisit.PatientLocation.Dept.User03);
					if (newDept == null) 
					{
						this.Err = department.Err;
						return -1;
					}

					//ת�봦������ת���1����ĩ������1
					if(type.ToString()=="RI") 
							
					{
							
						if (oldDept.SpecialFlag == "C" && newDept.SpecialFlag == "C") 
						{	
							//������������Ϊ���ƣ���ɽ���������ڲ�ת��
							//�ڲ�ת������1
							dayReport.InTransferInner = dayReport.InTransferInner +1;
						}
						else 
						{
							//����ת������1
							dayReport.InTransfer = dayReport.InTransfer +1;
						}
						//��ĩ������1
						dayReport.EndNum   = dayReport.EndNum + 1;
					}
					else
					{
						//ת������ת����1����ĩ������1
						if(oldDept.SpecialFlag == "C" && newDept.SpecialFlag == "C") 
						{
							//������������Ϊ���ƣ���ɽ���������ڲ�ת��
							//�ڲ�ת������1
							dayReport.OutTransferInner = dayReport.OutTransferInner + 1;
						}
						else 
						{
							//ת����������1
							dayReport.OutTransfer = dayReport.OutTransfer + 1;
						}
						dayReport.EndNum   = dayReport.EndNum - 1;
					}
				}
					break;
				case "C":
					//סԺ�ٻأ�סԺ�ٻؼ�1����ĩ������1

				{
					if(patientInfo.PVisit.OutTime.Date>= sysDate.Date)  
					{	
						//dayReport.DateStat = Convert.ToDateTime(patientInfo.PVisit.OutTime.Date + " 00:00:00");			
                        dayReport.DateStat = patientInfo.PVisit.OutTime.Date;			

						//���Ҵ˻����Ƿ��д��ڵ��ڽ��յĳ�Ժ�ǼǼ�¼�������������������Ժ�Ǽ��ձ���ϸ��¼
                        //{8997C648-0AE4-42f4-943A-4E34EC127B39}
						if (this.CancelDayReportDetail(dayReport) >= 1) 
						{
							//���û��߳�Ժ���ڵ��ڽ��գ��򽫳�Ժ������1����ϸ�Ѿ����ϡ����ٲ�����ϸ��¼
							if(patientInfo.PVisit.OutTime.Date == sysDate.Date) 
							{
								dayReport.OutNormal = dayReport.OutNormal - 1;
								//��NurseStation.User03 �� ��1������ʾ����Ҫ�����ձ���ϸ��¼
								dayReport.NurseStation.User03  =  "N";
							}
							else 
							{
								//������߳�Ժ���ڴ��ڽ��죬�򲻴����ձ����ܱ���ϸ�Ѿ�����
								return 1;
							}
						}
						else 
						{
							//���û�п������ϵĳ�Ժ��¼��˵���ٻص�����ǰ�����ݣ����ٻ�������1
							dayReport.InReturn = dayReport.InReturn + 1;
						}
					}
					
					dayReport.EndNum   = dayReport.EndNum + 1;
				}
					break;
				case "O": 
				{
					//������߳�Ժ���ڴ��ڽ��죬�ձ�ͳ�����ڵ��ڻ��߳�Ժ���ڡ��������ձ����ܱ�ֻ����ϸ��
					if(patientInfo.PVisit.OutTime >= sysDate.AddDays(1))
                    {
                        #region {BDB95DB2-E42B-4ee5-82F1-6DB5CDA072FA}
                        //dayReport.DateStat = Convert.ToDateTime(patientInfo.PVisit.OutTime + " 00:00:00");
                        dayReport.DateStat = patientInfo.PVisit.OutTime.Date;
                        #endregion
                        dayReport.ID = patientInfo.PVisit.PatientLocation.Dept.ID;  //�ձ����ұ���
						//�����ձ���ϸ���뺯��
						return this.SetDayReportDetail(dayReport);
					}

					//�����Ժ���ڴ��ڽ��գ��򲻸����ձ���ֻ����һ���ձ���ϸ��¼��ÿ���̨�ձ���������У�������ĳ�Ժ������1
					//��Ժ�Ǽǣ������Ժ��1����ĩ������1
					dayReport.OutNormal = dayReport.OutNormal + 1;
					dayReport.EndNum    = dayReport.EndNum - 1; //�˴��п�����-1
					
				}
					break;

				case "OF":
					//�޷���Ժ���޷���Ժ��1����ĩ������1
					//if (Type.ToString()=="OF") 
				{
					if (patientInfo.PVisit.PatientLocation.Bed.ID == "") return 1; //�������û�з��䴲λ����дסԺ��־
					dayReport.OutWithdrawal = dayReport.OutWithdrawal + 1;
					dayReport.EndNum   = dayReport.EndNum - 1; //�˴��п�����-1
				}
					break;
			}


			//��λ�ձ����������»��߲���
			//���û���ҵ��������ݣ�˵���Ǳ��յ�һ�θ��£�����ձ�ʵ�帳��ʼֵ��������һ����¼
			if(dayReport.ID == "") 
			{					
				//���ÿ���賿����̨�洢�����Զ�������һ����ձ����ݣ��Ͳ��������δ���

                //�����ұ��븳ֵ
                #region {275A3935-042C-4e62-8717-3B21ADB77D6E}
                dayReport.ID = patientInfo.PVisit.PatientLocation.Dept.ID;  //�ձ����ұ���
                //dayReport.ID = patientInfo.PVisit.PatientLocation.NurseCell.ID;  //�ձ����ұ���
                #endregion
                dayReport.NurseStation.ID = patientInfo.PVisit.PatientLocation.NurseCell.ID;

				//ȡ�̶���λ��
                //Neusoft.HISFC.Models.Base.DepartmentExt deptExt = this.GetDepartmentExt("CASE_BED_STAND",dayReport.ID);
                //if(deptExt == null) return -1;
                //dayReport.BedStand = Convert.ToInt32(deptExt.NumberProperty);
                //------------------
                Neusoft.FrameWork.Management.ExtendParam deptExtThing = new Neusoft.FrameWork.Management.ExtendParam();
                deptExtThing.SetTrans(this.Trans);
                decimal decBedNumber = deptExtThing.GetComExtInfoNumber(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT, "CASE_BED_STAND", dayReport.ID);
                dayReport.BedStand = Convert.ToInt32(decBedNumber);
                //------------------

				//ȡ����סԺ�ձ�����ĩ��
				Neusoft.HISFC.Models.HealthRecord.InpatientDayReport lastReport = this.GetInpatientDayReport(dayReport.DateStat.AddDays(-1), dayReport.ID, dayReport.NurseStation.ID);
				if(lastReport == null) return -1;
					
				//���û���ҵ������������ȡ��ʱ���µĻ�����Ժ����Ϊ��ʼ����ĩ��
				//����û�г�ʼ�ڳ�����ֻ�ܸ��ݵ�ǰ�Ļ�������Ϊ��ĩ����Ȼ����㱾���ڳ�����
				if (lastReport.ID == "")  
				{
					//ȡ������Ժ������Ϊ��ʱ��ĩ��
					dayReport.EndNum = GetDeptPatientNum(dayReport.ID, dayReport.NurseStation.ID);
					//��������ڳ���
					this.ComputeBeginingNum(ref dayReport);
				}
				else 
				{
					//�����ڳ�����������ĩ��
					dayReport.BeginningNum = lastReport.EndNum;
					//���������ĩ��
					this.ComputeEndNum(ref dayReport);
				}
		

				//����סԺ�ձ�����һ���¼�¼
				if (this.InsertInpatientDayReport(dayReport) == -1) return -1;

			}
			else  
			{
				//����סԺ�ձ�
				if (this.UpdateInpatientDayReport(dayReport) != 1) return -1;				
			}
				 
			//���NurseStation.User03 == "N"����ʾ����Ҫ������ϸ����
			if (dayReport.NurseStation.User03 == "N") 
			{
				return 1;
			}

			//�����ձ���ϸ���뺯��
			return this.SetDayReportDetail(dayReport);
			return 1;
		}


		/// <summary>
		/// �����ձ�ʵ���еĸ��������ݼ�����ĩ����
		/// </summary>
		/// <param name="dayReport"></param>
		/// <returns></returns>
		public void ComputeEndNum(ref Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			if (dayReport == null) return;
			dayReport.EndNum = dayReport.BeginningNum  
				+ dayReport.InNormal  
				+ dayReport.InEmergency 
				+ dayReport.InReturn
				+ dayReport.InTransfer 
				- dayReport.OutNormal
				- dayReport.OutTransfer
				- dayReport.OutWithdrawal;
		}


		/// <summary>
		/// �����ձ�ʵ���еĸ��������ݼ����ڳ�����
		/// </summary>
		/// <param name="dayReport"></param>
		/// <returns></returns>
		public void ComputeBeginingNum(ref Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			if (dayReport == null) return;
			dayReport.BeginningNum = dayReport.EndNum  
				- dayReport.InNormal  
				- dayReport.InEmergency 
				- dayReport.InReturn
				- dayReport.InTransfer 
				+ dayReport.OutNormal
				+ dayReport.OutTransfer
				+ dayReport.OutWithdrawal;
		}


		/// <summary>
		/// ����ձ��еĸ�����ֵ(�����ڳ���,��ĩ�������ڳ���)
		/// </summary>
		/// <param name="dayReport"></param>
		public void Clear(ref Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport)  
		{
			if (dayReport == null) return;
			
			dayReport.InNormal  = 0;
			dayReport.InEmergency = 0;
			dayReport.InReturn= 0;
			dayReport.InTransfer = 0;
			dayReport.OutNormal= 0;
			dayReport.OutTransfer= 0;
			dayReport.OutWithdrawal= 0;
			dayReport.EndNum = dayReport.BeginningNum;
		}


		/// <summary>
		/// ȡסԺ�ձ���Ϣ�б�������һ�����߶�������¼
		/// ˽�з����������������е���
		/// writed by cuipeng
		/// 2005-1
		/// </summary>
		/// <param name="SQLString">SQL���</param>
		/// <returns>סԺ�ձ���Ϣ��������</returns>
		private ArrayList myGetInpatientDayReport(string SQLString) 
		{
			ArrayList al=new ArrayList();                
			Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport; //סԺ�ձ���Ϣʵ��
			this.ProgressBarText="���ڼ���סԺ�ձ�����Ϣ...";
			this.ProgressBarValue=0;
			
			//ִ�в�ѯ���
			if (this.ExecQuery(SQLString)==-1) 
			{
				this.Err="��ÿ����Ϣʱ��ִ��SQL������"+this.Err;
				this.ErrCode="-1";
				return null;
			}
			try 
			{
				while (this.Reader.Read()) 
				{
					//ȡ��ѯ����еļ�¼
					dayReport = new Neusoft.HISFC.Models.HealthRecord.InpatientDayReport();
					dayReport.DateStat =    NConvert.ToDateTime(this.Reader[0].ToString()); //0 ͳ������
					dayReport.ID =          this.Reader[1].ToString();                      //1 סԺ�ձ�����
					dayReport.BedStand =    NConvert.ToInt32(this.Reader[2].ToString());    //2 �����ڲ�����
					dayReport.BedAdd =      NConvert.ToInt32(this.Reader[3].ToString());    //3 �Ӵ��� 
					dayReport.BedFree =     NConvert.ToInt32(this.Reader[4].ToString());    //4 �մ���
					dayReport.BeginningNum= NConvert.ToInt32(this.Reader[5].ToString());    //5 �ڳ�������
					dayReport.InNormal =    NConvert.ToInt32(this.Reader[6].ToString());    //6 ������Ժ��
					dayReport.InEmergency = NConvert.ToInt32(this.Reader[7].ToString());    //7 ������Ժ��
					dayReport.InTransfer =  NConvert.ToInt32(this.Reader[8].ToString());    //8 ������ת����
					dayReport.InReturn =    NConvert.ToInt32(this.Reader[9].ToString());    //9  �л���Ժ����
					dayReport.OutNormal =   NConvert.ToInt32(this.Reader[10].ToString());   //10 �����Ժ��
					dayReport.OutTransfer = NConvert.ToInt32(this.Reader[11].ToString());   //11 ת����������
					dayReport.OutWithdrawal=NConvert.ToInt32(this.Reader[12].ToString());   //12 ��Ժ����
					dayReport.EndNum =      NConvert.ToInt32(this.Reader[13].ToString());   //13 ��ĩ������
					dayReport.DeadIn24 =    NConvert.ToInt32(this.Reader[14].ToString());   //14 24Сʱ��������
					dayReport.DeadOut24 =   NConvert.ToInt32(this.Reader[15].ToString());   //15 24Сʱ��������
					dayReport.BedRate =     NConvert.ToDecimal(this.Reader[16].ToString()); //16 ��λʹ����
					dayReport.Other1Num =   NConvert.ToInt32(this.Reader[17].ToString());   //17 ����1����
					dayReport.Other2Num =   NConvert.ToInt32(this.Reader[18].ToString());   //18 ����2����
					dayReport.OperInfo.ID =    this.Reader[19].ToString();                     //19 ����Ա����
					dayReport.OperInfo.OperTime=     NConvert.ToDateTime(this.Reader[20].ToString());//20 ����ʱ��
					dayReport.Memo =        this.Reader[21].ToString();                     //21 ��ע
					dayReport.NurseStation.ID = this.Reader[22].ToString();                 //22 ��ʿվ����
					dayReport.Name =        this.Reader[23].ToString();                     //23 ��������
					dayReport.InTransferInner = NConvert.ToInt32(this.Reader[24].ToString());//24 �ڲ�ת����
					dayReport.OutTransferInner = NConvert.ToInt32(this.Reader[25].ToString());//25 �ڲ�ת����
				
                    this.ProgressBarValue++;
					al.Add(dayReport);
				}
			}//�׳�����
			catch(Exception ex) 
			{
				this.Err="���סԺ�ձ���Ϣʱ����"+ex.Message;
				this.ErrCode="-1";
				return null;
			}
			this.Reader.Close();

			this.ProgressBarValue=-1;
			return al;
		}


		/// <summary>
		/// ���update����insert����Ĵ����������
		/// </summary>
		/// <param name="dayReport">�����</param>
		/// <returns>�ַ�������</returns>
		private string[] myGetParmInpatientDayReport(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{

			string[] strParm={   
								 dayReport.DateStat.ToString(),      //0 ͳ������
								 dayReport.ID,                       //1 ���ұ���
								 dayReport.NurseStation.ID,          //2 ��ʿվ����
								 dayReport.BedStand.ToString(),      //3 �����ڲ�����
								 dayReport.BedAdd.ToString(),        //4 �Ӵ��� 
								 dayReport.BedFree.ToString(),       //5 �մ���
								 dayReport.BeginningNum.ToString(),  //6 �ڳ�������
								 dayReport.InNormal.ToString(),      //7 ������Ժ��
								 dayReport.InEmergency.ToString(),   //8 ������Ժ��
								 dayReport.InTransfer.ToString(),    //9 ������ת����
								 dayReport.InReturn.ToString(),      //10 �л���Ժ����
								 dayReport.OutNormal.ToString(),     //11 �����Ժ��
								 dayReport.OutTransfer.ToString(),   //12 ת����������
								 dayReport.OutWithdrawal.ToString(), //13 ��Ժ����
								 dayReport.EndNum.ToString(),        //14 ��ĩ������
								 dayReport.DeadIn24.ToString(),      //15 24Сʱ��������
								 dayReport.DeadOut24.ToString(),     //16 24Сʱ��������
								 dayReport.BedRate.ToString(),       //17 ��λʹ����
								 dayReport.Other1Num.ToString(),     //18 ����1����
								 dayReport.Other2Num.ToString(),     //19 ����2����
								 dayReport.Memo.ToString(),          //20 ��ע
								 this.Operator.ID,                    //21 ����Ա����
								 dayReport.InTransferInner.ToString(),//22 �ڲ�ת���� ����ɽ����
								 dayReport.OutTransferInner.ToString()//23 �ڲ�ת���� ����ɽ����
							 };								 
			return strParm;
		}

		
//		/// <summary>
//		/// ȡĳһ���ң��ض��������չ����
//		/// �˷�����Neusoft.HisFC.Management.Manager.DepartmentExt�е�GetDepartmentExt������ͬ
//		/// </summary>
//		/// <param name="PropertyCode">���Ա���</param>
//		/// <param name="DeptID">���ұ���</param>
//		/// <returns>��������</returns>
		private Neusoft.HISFC.Models.Base.DepartmentExt GetDepartmentExt(string PropertyCode,string DeptID) 
		{
			string strSQL = "";
			string strWhere = "";
			//ȡSELECT���
			if (this.Sql.GetSql("Manager.DepartmentExt.GetDepartmentExtList",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Manager.DepartmentExt.GetDepartmentExtList�ֶ�!";
				return null;
			}
			if (this.Sql.GetSql("Manager.DepartmentExt.And.DeptID",ref strWhere) == -1) 
			{
				this.Err="û���ҵ�Manager.DepartmentExt.And.DeptID�ֶ�!";
				return null;
			}
			//��ʽ��SQL���
			try 
			{
				strSQL += " " +strWhere;
				strSQL = string.Format(strSQL, PropertyCode,DeptID);
			}
			catch (Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Manager.DepartmentExt.And.DeptID:" + ex.Message;
				return null;
			}

			//ȡ������������
			Neusoft.HISFC.Models.Base.DepartmentExt departmentExt = new Neusoft.HISFC.Models.Base.DepartmentExt();
			
			//ִ�в�ѯ���
			if (this.ExecQuery(strSQL)==-1) 
			{
				this.Err="��ÿ���������Ϣʱ��ִ��SQL������"+this.Err;
				this.ErrCode="-1";
				return null;
			}
			try 
			{
				while (this.Reader.Read()) 
				{
					//ȡ��ѯ����еļ�¼
					departmentExt = new Neusoft.HISFC.Models.Base.DepartmentExt();
					departmentExt.Dept.ID   = this.Reader[0].ToString();          //0 ���ұ���
					departmentExt.Dept.Name = this.Reader[1].ToString();          //1 ��������
					departmentExt.PropertyCode   = this.Reader[2].ToString();     //2 ���Ա���
					departmentExt.PropertyName   = this.Reader[3].ToString();     //3 ��������
					departmentExt.StringProperty = this.Reader[4].ToString();     //4 �ַ����� 
					departmentExt.NumberProperty = NConvert.ToDecimal(this.Reader[5].ToString()); //5 ��ֵ����
					departmentExt.DateProperty   = NConvert.ToDateTime(this.Reader[6].ToString());//6 ��������
					departmentExt.Memo      = this.Reader[7].ToString();          //7 ��ע
					departmentExt.OperEnvironment.ID  = this.Reader[8].ToString();          //8 ��������
					departmentExt.OperEnvironment.OperTime  = NConvert.ToDateTime(this.Reader[9].ToString());     //9 ����ʱ��
					departmentExt.User01    = this.Reader[10].ToString();         //��������
				}
				this.Reader.Close();
			}//�׳�����
			catch(Exception ex) 
			{
				this.Err="��ÿ���������Ϣʱ����"+ex.Message;
				this.ErrCode="-1";
				return null;
			}

			return departmentExt;
		}


		/// <summary>
		/// ��ȡĳ�������е�סԺ������
		/// </summary>
		/// <param name="DeptId"></param>
		/// <returns></returns>
		private int GetDeptPatientNum(string DeptId,string NurseID)
		{
			try 
			{ 
				string strSQL="";
				if(this.Sql.GetSql("Case.InpatientDayReport.GetDeptPatientNum",ref strSQL) == -1) 
				{
					this.Err="û���ҵ�Case.InpatientDayReport.GetDeptPatientNum�ֶ�!";
					return -1;
				}
 
				//����������ӵ�סԺ�ձ�������ֱ�ӷ���
				strSQL=string.Format(strSQL, DeptId, NurseID);            //�滻SQL����еĲ�����
			
				return Neusoft.FrameWork.Function.NConvert.ToInt32(this.ExecSqlReturnOne(strSQL));
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.InpatientDayReport.GetDeptPatientNum:" + ex.Message;
				this.WriteErr();
				return -1;
			}
		}

		#endregion 

		#region �ձ���ϸ����
		/// <summary>
		/// ȡȫԺĳһ���סԺ�ձ�����
		/// </summary>
		/// <param name="dateBegin">��ʼ����</param>
		/// <param name="dateEnd">��ֹ����</param>
		/// <returns>סԺ�ձ����飬������null</returns>
		public ArrayList GetDayReportDetailList(DateTime dateBegin, DateTime dateEnd) 
		{
			return this.GetDayReportDetailList(dateBegin, dateEnd, "ALL", "ALL");
		}


		/// <summary>
		/// ȡȫԺĳһ���סԺ�ձ�����
		/// </summary>
		/// <param name="dateBegin">��ʼ����</param>
		/// <param name="dateEnd">��ֹ����</param>
		/// <returns>סԺ�ձ����飬������null</returns>
		public ArrayList GetDayReportDetailList(DateTime dateBegin, DateTime dateEnd, string deptCode, string nurseCellCode) 
		{
			string strSQL = "";
			//string strWhere = "";
			//ȡSELECT���
			if (this.Sql.GetSql("Case.DayReport.GetDayReportDetailList",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.DayReport.GetDayReportDetailList�ֶ�!";
				return null;
			}

			//��ʽ��SQL���
			try 
			{
				strSQL = string.Format(strSQL, dateBegin.ToString(), dateEnd.ToString(), deptCode);
			}
			catch (Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.DayReport.GetDayReportDetailList:" + ex.Message;
				return null;
			}

			//ȡסԺ�ձ�����
			return this.myGetDayReportDetail(strSQL);
		}


		/// <summary>
		/// ���ձ���ϸ���в���һ����¼
		/// </summary>
		/// <param name="reportDetail">סԺ�ձ�ʵ��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int InsertDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.DayReport.InsertDayReportDetail",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.DayReport.InsertDayReportDetail�ֶ�!";
				return -1;
			}
			try 
			{  
				string[] strParm = myGetParmDayReportDetail( reportDetail );     //ȡ�����б�
				strSQL=string.Format(strSQL, strParm);            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.DayReport.InsertDayReportDetail:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		
		
		/// <summary>
		/// �����ձ���ϸ����һ����¼
		/// </summary>
		/// <param name="reportDetail">סԺ�ձ�ʵ��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int UpdateDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.DayReport.UpdateDayReportDetail",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.DayReport.UpdateDayReportDetail�ֶ�!";
				return -1;
			}
			try 
			{  
				string[] strParm = myGetParmDayReportDetail( reportDetail );     //ȡ�����б�
				strSQL=string.Format(strSQL, strParm);            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.DayReport.UpdateDayReportDetail:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		
				
		/// <summary>
		/// �����ձ���ϸ����һ����¼
		/// </summary>
		/// <param name="reportDetail">סԺ�ձ�ʵ��</param>
		/// <returns>0û�и��� 1�ɹ� -1ʧ��</returns>
		public int CancelDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail) 
		{
			string strSQL="";
			if(this.Sql.GetSql("Case.DayReport.CancelDayReportDetail",ref strSQL) == -1) 
			{
				this.Err="û���ҵ�Case.DayReport.CancelDayReportDetail�ֶ�!";
				return -1;
			}
			try 
			{  
				//����˵����0����סԺ��ˮ�ţ�1�ձ�ͳ�����ͣ�Ŀǰֻ�г�Ժ�������ϣ���2ͳ������
				strSQL=string.Format(strSQL, reportDetail.User02, "O", reportDetail.DateStat.ToString());            //�滻SQL����еĲ�����
			}
			catch(Exception ex) 
			{
				this.Err = "��ʽ��SQL���ʱ����Case.DayReport.CancelDayReportDetail:" + ex.Message;
				this.WriteErr();
				return -1;
			}
			return this.ExecNoQuery(strSQL);
		}
		

		/// <summary>
		/// �ȸ���סԺ�ձ������û���ҵ����������һ��������
		/// </summary>
		/// <returns></returns>
		public int SetDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport) 
		{
			//����dayReport�е�user01�������Ǻ�������
			//1 K �����䣬
			//2 RB��ת�룬
			//3 RD��ת����
			//4 O ����Ժ�Ǽǣ�
			//5 C ���ٻأ�
			//6 OF���޷���Ժ
			//			switch (dayReport.User01) {
			//				case "K":	//����
			//					dayReport.User01 = "I_NORMAL";
			//					break;
			//				case "RI":	//ת��
			//					dayReport.User01 = "I_TRANSFER";
			//					break;
			//				case "C":	//�ٻ�
			//					dayReport.User01 = "I_RETURN";
			//					break;
			//				case "RO":	//ת��
			//					dayReport.User01 = "O_TRANSFER";
			//					break;
			//				case "O":	//��Ժ�Ǽ�
			//					dayReport.User01 = "O_NORMAL";
			//					break;
			//				case "OF":	//�޷���Ժ
			//					dayReport.User01 = "O_WITHDRAWAL";
			//					break;
			//			}
			//			int parm;
			//			//�ȸ���סԺ�ձ�
			//			parm = this.InsertInpatientDayReportDetail(dayReport);
			//			return parm;
			return this.InsertDayReportDetail(dayReport);
		}


		/// <summary>
		/// ȡ�ձ���ϸ��Ϣ�б�������һ�����߶�������¼
		/// ˽�з����������������е���
		/// writed by cuipeng
		/// 2006-3
		/// </summary>
		/// <param name="SQLString">SQL���</param>
		/// <returns>NeuObject��������</returns>
		private ArrayList myGetDayReportDetail(string SQLString) 
		{
			ArrayList al=new ArrayList();                
			Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail; //סԺ�ձ���Ϣʵ��
			this.ProgressBarText="���ڼ���סԺ�ձ�����Ϣ...";
			this.ProgressBarValue=0;
			
			//ִ�в�ѯ���
			if (this.ExecQuery(SQLString)==-1) 
			{
				this.Err="��ÿ����Ϣʱ��ִ��SQL������"+this.Err;
				this.ErrCode="-1";
				return null;
			}
			try 
			{
				while (this.Reader.Read()) 
				{
					//ȡ��ѯ����еļ�¼
					reportDetail = new Neusoft.HISFC.Models.HealthRecord.InpatientDayReport();
					reportDetail.DateStat = NConvert.ToDateTime(this.Reader[0].ToString()); //0 ͳ������
					reportDetail.ID       = this.Reader[1].ToString();                      //1 ���ұ���
					reportDetail.NurseStation.ID = this.Reader[2].ToString();               //2 ����վ����
					reportDetail.User01   = this.Reader[3].ToString();                      //3 ͳ������
					reportDetail.User02   = this.Reader[4].ToString();                      //4 סԺ��ˮ��
					reportDetail.User03   = this.Reader[5].ToString();                      //5 ����
					reportDetail.Memo     = this.Reader[6].ToString();                      //6 ��ע
					reportDetail.OperInfo.ID = this.Reader[7].ToString();                      //7 ����Ա
					reportDetail.OperInfo.OperTime = NConvert.ToDateTime(this.Reader[8].ToString()); //8 ����ʱ��
					
					this.ProgressBarValue++;
					al.Add(reportDetail);
				}
			}//�׳�����
			catch(Exception ex) 
			{
				this.Err="���סԺ�ձ���Ϣʱ����"+ex.Message;
				this.ErrCode="-1";
				return null;
			}
			this.Reader.Close();

			this.ProgressBarValue=-1;
			return al;
		}


		/// <summary>
		/// ���update����insert����Ĵ����������
		/// </summary>
		/// <param name="reportDetail">�����</param>
		/// <returns>�ַ�������</returns>
		private string[] myGetParmDayReportDetail(Neusoft.HISFC.Models.HealthRecord.InpatientDayReport reportDetail) 
		{
            string strSQL = "";
            string patientNo = reportDetail.User02;
            string zg = "";
            if (this.Sql.GetSql("Case.DayReport.GetDayReportDetailZg", ref strSQL) == -1)
            {
                this.Err = "û���ҵ�Case.DayReport.GetDayReportDetailZg�ֶ�!";
                this.ErrCode = "-1";
                return null;
            }
            try
            {
                strSQL = string.Format(strSQL, patientNo);            //�滻SQL����еĲ�����
            }
            catch (Exception ex)
            {
                this.Err = "��ʽ��SQL���ʱ����Case.DayReport.GetDayReportDetailZg:" + ex.Message;
                this.WriteErr();
                this.ErrCode = "-1";
                return null;
            }
           
     
         
          
            if (this.ExecQuery(strSQL) == -1)
            {
                this.Err = "���ת�����ʱ����ִ��SQL������" + this.Err;
                this.ErrCode = "-1";
                return null;
            }
            try
            {
                while (this.Reader.Read())
                {
                    //ȡ��ѯ����еļ�¼
                    zg = this.Reader[0].ToString();          //0 ת�����                 
                }
                this.Reader.Close();
            }//�׳�����
            catch (Exception ex)
            {
                this.Err = "���ת�����ʱ����" + ex.Message;
                this.ErrCode = "-1";
                return null;
            }
            string[] strParm={   
								 reportDetail.DateStat.ToString(),      //0 ͳ������
								 reportDetail.ID,                       //1 ���ұ���
								 reportDetail.NurseStation.ID,          //2 ��ʿվ����
								 reportDetail.User02,					//3 סԺ��ˮ��
								 reportDetail.User03,					//4 ����
								 reportDetail.User01,					//5 ͳ������
								 this.Operator.ID,						//6 ����Ա����
								 reportDetail.Memo,					//7 ��ע
                                 zg                                 //8 ת�����
                                
							 };								 
			return strParm;
		}

		#endregion 
		
	}

    internal class DayReportSortByDate : IComparer
    {
        #region IComparer ��Ա

        public int Compare(object x, object y)
        {
            Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport1 = x as Neusoft.HISFC.Models.HealthRecord.InpatientDayReport;
            Neusoft.HISFC.Models.HealthRecord.InpatientDayReport dayReport2 = y as Neusoft.HISFC.Models.HealthRecord.InpatientDayReport;
            if (dayReport1 == null || dayReport2 == null)
            {
                return 0;
            }
            if (dayReport2.DateStat > dayReport1.DateStat)
            {
                return -1;
            }
            if (dayReport2.DateStat < dayReport1.DateStat)
            {
                return 1;
            } 
            return 0;
        }

        #endregion
    }


}
