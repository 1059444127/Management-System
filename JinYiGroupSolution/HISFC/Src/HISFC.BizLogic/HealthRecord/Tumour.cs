using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Neusoft.HISFC.BizLogic.HealthRecord
{
    public class Tumour : Neusoft.FrameWork.Management.Database
    {
        #region  ��������
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        /// <param name="inpatientNo"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.HealthRecord.Tumour GetTumour(string inpatientNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.GetTumour", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, inpatientNo);
                //��ѯ
                this.ExecQuery(strSql);
                Neusoft.HISFC.Models.HealthRecord.Tumour info = new Neusoft.HISFC.Models.HealthRecord.Tumour();
                while (this.Reader.Read())
                {
                    info.InpatientNo = Reader[0].ToString();								//סԺ��ˮ�� 
                    info.Rmodeid = Reader[1].ToString();									//���Ʒ�ʽ
                    info.Rprocessid = Reader[2].ToString();									//���Ƴ�ʽ
                    info.Rdeviceid = Reader[3].ToString();									//����װ��
                    info.Cmodeid = Reader[4].ToString();									//���Ʒ�ʽ
                    info.Cmethod = Reader[5].ToString();									//���Ʒ���
                    info.Gy1 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[6]);		//ԭ����gy����
                    info.Time1 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[7]);		//ԭ�������
                    info.Day1 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[8]);		//ԭ��������
                    info.BeginDate1 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[9]);//ԭ���ʼʱ��
                    info.EndDate1 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[10]);  //ԭ�������ʱ��
                    info.Gy2 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[11]);		//�����ܰͽ�gy����
                    info.Time2 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[12]);		//�����ܰͽ����
                    info.Day2 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[13]);		//�����ܰͽ�����
                    info.BeginDate2 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[14]);//�����ܰͽῪʼʱ��
                    info.EndDate2 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[15]);  //�����ܰͽ����ʱ��
                    info.Gy3 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[16]);		//ת����gy����
                    info.Time3 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[17]);		//�����ܰͽ����
                    info.Day3 = Neusoft.FrameWork.Function.NConvert.ToDecimal(Reader[18]);		//�����ܰͽ�����
                    info.BeginDate3 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[19]);//�����ܰͽῪʼʱ��
                    info.EndDate3 = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[20]);  //�����ܰͽ����ʱ��
                    info.OperInfo.ID = Reader[21].ToString();								 //����Ա 
                    info.User01 = Reader[22].ToString();//����ʱ��           
                }
                return info;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateTumour(Neusoft.HISFC.Models.HealthRecord.Tumour info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.UpdateTumour", ref strSql) == -1) return -1;
            try
            {
                object[] mm = GetTumourInfo(info);
                if (mm == null)
                {
                    this.Err = "ҵ����ʵ���л�ȡ�ַ��������";
                    return -1;
                }
                strSql = string.Format(strSql, mm);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        private string[] GetTumourInfo(Neusoft.HISFC.Models.HealthRecord.Tumour info)
        {
            string[] ss = new string[23];
            ss[0] = info.InpatientNo;	//סԺ��ˮ�� 
            ss[1] = info.Rmodeid;		//���Ʒ�ʽ
            ss[2] = info.Rprocessid;	//���Ƴ�ʽ
            ss[3] = info.Rdeviceid;		//����װ��
            ss[4] = info.Cmodeid;		//���Ʒ���
            ss[5] = info.Cmethod;		//���Ʒ���
            ss[6] = info.Gy1.ToString();			//ԭ����gy����
            ss[7] = info.Time1.ToString();			//ԭ�������
            ss[8] = info.Day1.ToString();			//ԭ��������
            ss[9] = info.BeginDate1.ToString();	//ԭ���ʼʱ��
            ss[10] = info.EndDate1.ToString();		//ԭ�������ʱ��
            ss[11] = info.Gy2.ToString();			//�����ܰͽ�gy����
            ss[12] = info.Time2.ToString();		//�����ܰͽ����
            ss[13] = info.Day2.ToString();			//�����ܰͽ�����
            ss[14] = info.BeginDate2.ToString();	//�����ܰͽῪʼʱ��
            ss[15] = info.EndDate2.ToString();		//�����ܰͽ����ʱ��
            ss[16] = info.Gy3.ToString();			//ת����gy����
            ss[17] = info.Time3.ToString();		//�����ܰͽ����
            ss[18] = info.Day3.ToString();		//�����ܰͽ�����
            ss[19] = info.BeginDate3.ToString();	//�����ܰͽῪʼʱ��
            ss[20] = info.EndDate3.ToString();		//�����ܰͽ����ʱ��
            ss[21] = this.Operator.ID;	//����Ա 
            //			ss[21] = info.User01 = Reader[21].ToString();//����ʱ��           
            return ss;
        }
        /// <summary>
        /// ��������ϸ���в���һ������
        /// </summary>
        /// <param name="info"></param>
        /// <returns>�����쳣���أ�1 �ɹ�����1 ����ʧ�ܷ��� 0</returns>
        public int InsertTumour(Neusoft.HISFC.Models.HealthRecord.Tumour info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.InsertTumour", ref strSql) == -1) return -1;
            try
            {
                //��ȡ����ֵ
                object[] mm = GetTumourInfo(info);
                if (mm == null)
                {
                    this.Err = "ҵ����ʵ���л�ȡ�ַ��������";
                    return -1;
                }
                strSql = string.Format(strSql, mm);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        /// <summary>
        /// ��������ϸ����ɾ��һ������
        /// </summary>
        /// <param name="InpatientNo"></param>
        /// <returns>�����쳣���أ�1 �ɹ�����1 ����ʧ�ܷ��� 0</returns>
        public int DeleteTumour(string InpatientNo)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.DeleteTumour", ref strSql) == -1) return -1;
            try
            {
                //��ȡ����ֵ
                strSql = string.Format(strSql, InpatientNo);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        #endregion

        #region ������ϸ��
        /// <summary>
        /// ��ȡ������ϸ���е�����
        /// </summary>
        /// <param name="inpatienNo">סԺ��ˮ��</param>
        /// <returns>������null</returns>
        public ArrayList QueryTumourDetail(string inpatienNo)
        {
            ArrayList List = null;
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.GetTumourDetail", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, inpatienNo);
                //��ѯ
                this.ExecQuery(strSql);
                Neusoft.HISFC.Models.HealthRecord.TumourDetail info = null;
                List = new ArrayList();
                while (this.Reader.Read())
                {
                    info = new Neusoft.HISFC.Models.HealthRecord.TumourDetail();
                    info.InpatientNO = Reader[0].ToString();
                    info.HappenNO = Neusoft.FrameWork.Function.NConvert.ToInt32(Reader[1].ToString());
                    info.CureDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[2].ToString());
                    info.DrugInfo.ID = Reader[3].ToString();
                    info.DrugInfo.Name = Reader[4].ToString();
                    info.Qty = Neusoft.FrameWork.Function.NConvert.ToInt32(Reader[5].ToString());
                    info.Unit = Reader[6].ToString();
                    info.Period = Reader[7].ToString();
                    info.Result = Reader[8].ToString();
                    info.OperInfo.ID = Reader[9].ToString();
                    List.Add(info);
                    info = null;
                }
                this.Reader.Close();
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }
                List = null;
            }
            return List;
        }
        /// <summary>
        /// ����������ϸ���е�����
        /// </summary>
        /// <param name="info"></param>
        /// <returns>�����쳣���أ�1 �ɹ�����1����ʧ�ܷ��� 0 </returns>
        public int UpdateTumourDetail(Neusoft.HISFC.Models.HealthRecord.TumourDetail info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.UpdateTumourDetail", ref strSql) == -1) return -1;
            try
            {
                info.OperInfo.ID = this.Operator.ID;
                object[] mm = GetInfo(info);
                if (mm == null)
                {
                    this.Err = "ҵ����ʵ���л�ȡ�ַ��������";
                    return -1;
                }
                strSql = string.Format(strSql, mm);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        /// <summary>
        /// ��������ϸ���в���һ������
        /// </summary>
        /// <param name="info"></param>
        /// <returns>�����쳣���أ�1 �ɹ�����1 ����ʧ�ܷ��� 0</returns>
        public int InsertTumourDetail(Neusoft.HISFC.Models.HealthRecord.TumourDetail info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.InsertTumourDetail", ref strSql) == -1) return -1;
            try
            {
                info.OperInfo.ID = this.Operator.ID;
                //��ȡ����ֵ
                info.HappenNO = Neusoft.FrameWork.Function.NConvert.ToInt32(this.GetSequence("Case.Tumour.GetSequence"));
                object[] mm = GetInfo(info);
                if (mm == null)
                {
                    this.Err = "ҵ����ʵ���л�ȡ�ַ��������";
                    return -1;
                }
                strSql = string.Format(strSql, mm);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        private object[] GetInfo(Neusoft.HISFC.Models.HealthRecord.TumourDetail info)
        {
            try
            {
                object[] s = new object[10];
                s[0] = info.InpatientNO;		//סԺ��ˮ��
                s[1] = info.HappenNO; //�������      
                s[2] = info.CureDate.ToString(); //��������
                s[3] = info.DrugInfo.ID;//ҩ�����       
                s[4] = info.DrugInfo.Name;//ҩ������         
                s[5] = info.Qty.ToString();//����   
                s[6] = info.Unit;//��λ 
                s[7] = info.Period;//�Ƴ�
                s[8] = info.Result;// ��Ч		
                s[9] = info.OperInfo.ID;//  ����Ա����       
                return s;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int DeleteTumourDetail(Neusoft.HISFC.Models.HealthRecord.TumourDetail info)
        {
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.DeleteTumourDetail", ref strSql) == -1) return -1;
            try
            {
                //��ʽ���ַ���
                strSql = string.Format(strSql, info.InpatientNO, info.HappenNO);
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                return -1;
            }
            return this.ExecNoQuery(strSql);
        }
        #endregion

        #region  ����
        /// <summary>
        /// ��ȡ������ϸ���е�����
        /// </summary>
        /// <param name="inpatienNo">סԺ��ˮ��</param>
        /// <returns>������null</returns>
        [Obsolete("����,�� QueryTumourDetail ����", true)]
        public ArrayList GetTumourDetail(string inpatienNo)
        {
            ArrayList List = null;
            string strSql = "";
            if (this.Sql.GetSql("Case.Tumour.GetTumourDetail", ref strSql) == -1) return null;
            try
            {
                strSql = string.Format(strSql, inpatienNo);
                //��ѯ
                this.ExecQuery(strSql);
                Neusoft.HISFC.Models.HealthRecord.TumourDetail info = null;
                List = new ArrayList();
                while (this.Reader.Read())
                {
                    info = new Neusoft.HISFC.Models.HealthRecord.TumourDetail();
                    info.InpatientNO = Reader[0].ToString();
                    info.HappenNO = Neusoft.FrameWork.Function.NConvert.ToInt32(Reader[1].ToString());
                    info.CureDate = Neusoft.FrameWork.Function.NConvert.ToDateTime(Reader[2].ToString());
                    info.DrugInfo.ID = Reader[3].ToString();
                    info.DrugInfo.Name = Reader[4].ToString();
                    info.Qty = Neusoft.FrameWork.Function.NConvert.ToInt32(Reader[5].ToString());
                    info.Unit = Reader[6].ToString();
                    info.Period = Reader[7].ToString();
                    info.Result = Reader[8].ToString();
                    info.OperInfo.ID = Reader[9].ToString();
                    List.Add(info);
                    info = null;
                }
                this.Reader.Close();
            }
            catch (Exception ee)
            {
                this.Err = ee.Message;
                if (!this.Reader.IsClosed)
                {
                    this.Reader.Close();
                }
                List = null;
            }
            return List;
        }
        /// <summary>
        /// �Ƴ��б�
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ����PERIODOFTREATMENT����", true)]
        public ArrayList GetPeriodList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "�Ƴ�I";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "�Ƴ�II";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "�Ƴ�III";
            //list.Add(info);

            return list;
        }
        /// <summary>
        /// ��ȡ����б�
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ����RADIATERESULT ����", true)]
        public ArrayList GetResultList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "CR";
            //info.Name = "��ʧ";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "PR";
            //info.Name = "��Ч";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "MR";
            //info.Name = "��ת";
            //list.Add(info);

            //info.ID = "S";
            //info.Name = "����";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "P";
            //info.Name = "��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "NA";
            //info.Name = "δ��";

            //list.Add(info);
            return list;
        }
        /// <summary>
        /// ���Ʒ�ʽ 
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ���� RADIATETYPE ����", true)]
        public ArrayList GetRmodeidList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "������";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "��Ϣ��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "������";
            //list.Add(info);
            return list;
        }
        /// <summary>
        /// ���Ƴ�ʽ 
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ����RADIATEPERIOD ����", true)]
        public ArrayList GetRprocessidList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "����";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "���";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "�ֶ�";
            //list.Add(info);
            return list;
        }
        /// <summary>
        /// ����װ��
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ���� RADIATEDEVICE ����", true)]
        public ArrayList GetRdeviceidList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "ֱ��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "X��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "4";
            //info.Name = "��װ";
            //list.Add(info);

            return list;
        }
        /// <summary>
        /// ���Ʒ�ʽ
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ���� CHEMOTHERAPY ����", true)]
        public ArrayList GetCmodeidList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "������";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "��Ϣ��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "�¸�����";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "4";
            //info.Name = "������";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "5";
            //info.Name = "��ҩ����";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "6";
            //info.Name = "����";
            //list.Add(info);

            return list;
        }
        /// <summary>
        /// ���Ʒ���
        /// </summary>
        /// <returns></returns>
        [Obsolete("���� �� ���� CHEMOTHERAPYWAY ����", true)]
        public ArrayList GetCmethodList()
        {
            ArrayList list = new ArrayList();
            //neusoft.HISFC.Object.Base.SpellCode info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "1";
            //info.Name = "ȫ��";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "2";
            //info.Name = "�뻯";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "3";
            //info.Name = "A���";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "4";
            //info.Name = "��ǻע";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "5";
            //info.Name = "��ǻע";
            //list.Add(info);

            //info = new neusoft.HISFC.Object.Base.SpellCode();
            //info.ID = "6";
            //info.Name = "����";
            //list.Add(info);

            return list;
        }
        #endregion 
    }
}
