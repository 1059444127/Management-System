using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Neusoft.HISFC.BizLogic.HealthRecord
{
    public class CaseCard : Neusoft.FrameWork.Management.Database
    {
        #region ���Ŀ� ��������ά��
        /// <summary>
        /// ��ȡ���еĽ��Ŀ�����Ϣ 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public int GetCardInfo(ref System.Data.DataSet ds)
        {
            try
            {
                string strSql = GeCardSql();
                //��ѯ
                return this.ExecQuery(strSql, ref ds);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
        }
        private string GeCardSql()
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.GetCardInfo", ref strSql) == -1) return null;
            return strSql;
        }
        /// <summary>
        /// ���ݿ��Ż�ȡ��Ϣ 
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.HealthRecord.ReadCard GetCardInfo(string CardID)
        {
            Neusoft.HISFC.Models.HealthRecord.ReadCard info = new Neusoft.HISFC.Models.HealthRecord.ReadCard();
            try
            {
                string strSql = "";
                string strSql1 = GeCardSql();
                if (strSql1 == null)
                {
                    return null;
                }
                if (this.Sql.GetSql("Case.CaseCard.GetCardInfo.1", ref strSql) == -1) return null;
                strSql1 += strSql;
                strSql1 = string.Format(strSql1, CardID);
                //��ѯ
                this.ExecQuery(strSql1);
                while (this.Reader.Read())
                {
                    info.CardID = this.Reader[0].ToString(); //����
                    info.EmployeeInfo.ID = this.Reader[1].ToString(); //Ա����
                    info.EmployeeInfo.Name = this.Reader[2].ToString();//Ա������
                    info.DeptInfo.ID = this.Reader[3].ToString();//���Ҵ���
                    info.DeptInfo.Name = this.Reader[4].ToString();//��������
                    info.User01 = this.Reader[5].ToString();//����Ա
                    info.EmployeeInfo.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[6].ToString());//����ʱ��
                    info.ValidFlag = this.Reader[7].ToString();//��Ч
                    info.CancelOperInfo.Name = this.Reader[8].ToString();//������
                    info.CancelDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[9].ToString());//����ʱ��
                }
                this.Reader.Close();
                return info;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// ���Ŀ����Ƿ��Ѿ����� 
        /// </summary>
        /// <param name="CardID"></param>
        /// <returns> -1 ���� ��1 ���� ��2 ������ </returns>
        public int IsExist(string CardID)
        {
            try
            {
                string strSql = "";
                if (this.Sql.GetSql("Case.CaseCard.GetCardInfo.1", ref strSql) == -1) return -1;
                strSql = string.Format(strSql, CardID);
                //��ѯ
                this.ExecQuery(strSql);
                while (this.Reader.Read())
                {
                    return 1;
                }
                this.Reader.Close();
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
            return 2;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Insert(Neusoft.HISFC.Models.HealthRecord.ReadCard info)
        {
            try
            {
                string strSql = "";
                if (this.Sql.GetSql("Case.CaseCard.Insert", ref strSql) == -1) return -1;
                string[] Str = GetInfo(info);
                strSql = string.Format(strSql, Str);
                //��ѯ
                return this.ExecNoQuery(strSql);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int Update(Neusoft.HISFC.Models.HealthRecord.ReadCard info)
        {
            try
            {
                string strSql = "";
                if (this.Sql.GetSql("Case.CaseCard.Update", ref strSql) == -1) return -1;
                string[] Str = GetInfo(info);
                strSql = string.Format(strSql, Str);
                //��ѯ
                return this.ExecNoQuery(strSql);
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return -1;
            }
        }
        private string[] GetInfo(Neusoft.HISFC.Models.HealthRecord.ReadCard obj)
        {
            string[] str = new string[10];
            try
            {
                str[0] = obj.CardID; //����
                str[1] = obj.EmployeeInfo.ID; //Ա����
                str[2] = obj.EmployeeInfo.Name;//Ա������
                str[3] = obj.DeptInfo.ID;//���Ҵ���
                str[4] = obj.DeptInfo.Name;//��������
                str[5] = obj.User01;//����Ա
                str[6] = obj.EmployeeInfo.OperTime.ToString();//����ʱ��
                str[7] = obj.ValidFlag;//��Ч
                str[8] = obj.CancelOperInfo.Name;//������
                str[9] = obj.CancelDate.ToString();//����ʱ��
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
            return str;
        }
        #endregion

        #region  ��������
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int LendCase(Neusoft.HISFC.Models.HealthRecord.Lend info)
        {
            string[] arrStr = getInfo(info);
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.LendCase", ref strSql) == -1) return -1;
            strSql = string.Format(strSql, arrStr);
            return this.ExecNoQuery(strSql);
        }

        /// <summary>
        /// ���²����ı�־ 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="PaptientNO"></param>
        /// <returns></returns>
        public int UpdateBase(Neusoft.HISFC.Models.HealthRecord.EnumServer.LendType type, string CaseNO)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.UpdateBase", ref strSql) == -1) return -1;
            strSql = string.Format(strSql, type.ToString(), CaseNO);
            return this.ExecNoQuery(strSql);
        }
        /// <summary>
        /// �黹 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int ReturnCase(Neusoft.HISFC.Models.HealthRecord.Lend info)
        {
            string[] arrStr = getInfo(info);
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.ReturnCase", ref strSql) == -1) return -1;
            strSql = string.Format(strSql, arrStr);
            return this.ExecNoQuery(strSql);
        }
        private string[] getInfo(Neusoft.HISFC.Models.HealthRecord.Lend info)
        {
            string[] str = new string[26];
            str[0] = info.CaseBase.PatientInfo.ID;//סԺ��ˮ��
            str[1] = info.CaseBase.CaseNO;//����סԺ��
            str[2] = info.CaseBase.PatientInfo.Name; //��������
            str[3] = info.CaseBase.PatientInfo.Sex.ID.ToString();//�Ա�
            str[4] = info.CaseBase.PatientInfo.Birthday.ToString();//��������
            str[5] = info.CaseBase.PatientInfo.PVisit.InTime.ToString();//��Ժ����
            str[6] = info.CaseBase.PatientInfo.PVisit.OutTime.ToString();//��Ժ����
            str[7] = info.CaseBase.InDept.ID; //��Ժ���Ҵ���
            str[8] = info.CaseBase.InDept.Name; //��Ժ��������
            str[9] = info.CaseBase.OutDept.ID;  //��Ժ���Ҵ���
            str[10] = info.CaseBase.OutDept.Name; //��Ժ��������
            str[11] = info.EmployeeInfo.ID;//�����˴���
            str[12] = info.EmployeeInfo.Name;//����������
            str[13] = info.EmployeeDept.ID; //���������ڿ��Ҵ���
            str[14] = info.EmployeeDept.Name; //���������ڿ�������
            str[15] = info.LendDate.ToString(); //��������
            str[16] = info.PrerDate.ToString(); //Ԥ������
            str[17] = info.LendKind; //��������
            str[18] = info.LendStus;//����״̬ 1���/2����
            str[19] = info.ID; //����Ա����
            str[20] = info.OperInfo.OperTime.ToString(); //����ʱ��
            str[21] = info.ReturnOperInfo.ID;   //�黹����Ա����
            str[22] = info.ReturnDate.ToString();   //ʵ�ʹ黹����
            str[23] = info.CardNO;//����
            str[24] = info.Memo; //�������
            str[25] = info.SeqNO; //�������
            return str;
        }
        /// <summary>
        /// ���ݿ��Ų�ѯ��Ҫ�黹����Ϣ
        /// </summary>
        /// <param name="LendCardNo"></param>
        /// <returns></returns>
        public ArrayList QueryLendInfo(string LendCardNo)
        {
            string StrSql = GetLendSql();
            if (StrSql == null)
            {
                return null;
            }
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.GetLendSql.where", ref strSql) == -1) return null;
            StrSql += strSql;
            StrSql = string.Format(StrSql, LendCardNo);
            this.ExecQuery(StrSql);
            return QueryLendInfoBase();
        }

        /// <summary>
        /// ����where�����в�ѯ
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public ArrayList QueryLendInfoByWhereString(string strWhere)
        {
            string strSql = GetLendSql();
            if (strSql == null)
            {
                return null;
            }
            this.ExecQuery(strSql + " " + strWhere);
            return QueryLendInfoBase();
        }

        /// <summary>
        /// ���ݲ����Ų���������Ϣ
        /// </summary>
        /// <param name="LendCardNo"></param>
        /// <returns></returns>
        public ArrayList QueryLendInfoByCaseNO(string CaseNO) 
        {
            string StrSql = GetLendSql();
            if (StrSql == null)
            {
                return null;
            }
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.QueryLendInfo.where", ref strSql) == -1) return null;
            StrSql += strSql;
            StrSql = string.Format(StrSql, CaseNO);
            this.ExecQuery(StrSql);
            return QueryLendInfoBase();
        }
        /// <summary>
        /// ˽�к���
        /// </summary>
        /// <returns></returns>
        private ArrayList QueryLendInfoBase()
        {
            try
            {
                ArrayList list = new ArrayList();
                Neusoft.HISFC.Models.HealthRecord.Lend info = null;
                while (this.Reader.Read())
                {
                    info = new Neusoft.HISFC.Models.HealthRecord.Lend();
                    info.CaseBase.PatientInfo.ID = this.Reader[0].ToString();//סԺ��ˮ��
                    info.CaseBase.CaseNO = this.Reader[1].ToString();//����סԺ��
                    info.CaseBase.PatientInfo.Name = this.Reader[2].ToString(); //��������
                    info.CaseBase.PatientInfo.Sex.ID = this.Reader[3].ToString();//�Ա�
                    info.CaseBase.PatientInfo.Birthday = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[4].ToString());//��������
                    info.CaseBase.PatientInfo.PVisit.InTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[5].ToString());//��Ժ����
                    info.CaseBase.PatientInfo.PVisit.OutTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[6].ToString());//��Ժ����
                    info.CaseBase.InDept.ID = this.Reader[7].ToString(); //��Ժ���Ҵ���
                    info.CaseBase.InDept.Name = this.Reader[8].ToString(); //��Ժ��������
                    info.CaseBase.OutDept.ID = this.Reader[9].ToString();  //��Ժ���Ҵ���
                    info.CaseBase.OutDept.Name = this.Reader[10].ToString(); //��Ժ��������
                    info.EmployeeInfo.ID = this.Reader[11].ToString();//�����˴���
                    info.EmployeeInfo.Name = this.Reader[12].ToString();//����������
                    info.EmployeeDept.ID = this.Reader[13].ToString(); //���������ڿ��Ҵ���
                    info.EmployeeDept.Name = this.Reader[14].ToString(); //���������ڿ�������
                    info.LendDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[15].ToString()); //��������
                    info.PrerDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[16].ToString()); //Ԥ������
                    info.LendKind = this.Reader[17].ToString(); //��������
                    info.LendStus = this.Reader[18].ToString();//����״̬ 1���/2����
                    info.ID = this.Reader[19].ToString(); //����Ա����
                    info.OperInfo.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[20].ToString()); //����ʱ��
                    info.ReturnOperInfo.ID = this.Reader[21].ToString();   //�黹����Ա����
                    info.ReturnDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.Reader[22].ToString());   //ʵ�ʹ黹����
                    info.CardNO = this.Reader[23].ToString();//����
                    info.Memo = this.Reader[24].ToString(); //�������
                    info.SeqNO = this.Reader[25].ToString(); //�������
                    list.Add(info);
                }
                this.Reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }
        private string GetLendSql()
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.GetLendSql", ref strSql) == -1) return null;
            return strSql;
        }
        #endregion

        #region ����
        [Obsolete("����,�� QueryLendInfo ����")]
        public ArrayList GetLendInfo(string LendCardNo)
        {
            string StrSql = GetLendSql();
            if (StrSql == null)
            {
                return null;
            }
            string strSql = "";
            if (this.Sql.GetSql("Case.CaseCard.GetLendSql.where", ref strSql) == -1) return null;
            StrSql += strSql;
            StrSql = string.Format(StrSql, LendCardNo);
            this.ExecQuery(StrSql);
            return QueryLendInfoBase();
        }
        #endregion
    }
}
