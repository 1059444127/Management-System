using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.HISFC.Models.HealthRecord;
using Neusoft.FrameWork.Function;
using System.Data;
using System.Collections;

namespace Neusoft.HISFC.BizLogic.HealthRecord.Case
{
    /// <summary>
    /// Visit<br></br>
    /// [��������: ������ѯ����]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2007-08-27]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
        public class CaseLend : Neusoft.FrameWork.Management.Database
        {


        #region ��ѯ

        /// <summary>
        ///�����ݲ��������ѯ��������Ϣ
        /// </summary>
        /// <param name="">������ˮ��</param>
        /// <returns>��Ϣ����Ԫ����: Neusoft.HISFC.Models.HealthRecord.Case.CaseLend</returns>

        
        public ArrayList Query(string billID)
        {
            ArrayList List = null;
            string strSql = "";
            if (this.Sql.GetSql("HealthReacord.Case.CaseLend.Select", ref strSql) == -1) return null;
            try
            {
                //��ѯ
                strSql = string.Format(strSql, billID);
                this.ExecQuery(strSql);
                Neusoft.HISFC.Models.HealthRecord.Case.CaseLend caseLend = null;
                List = new ArrayList();
                while (this.Reader.Read())
                {
                    caseLend = new Neusoft.HISFC.Models.HealthRecord.Case.CaseLend();
                    caseLend.ID = this.Reader[0].ToString();         //������ 
                    caseLend.LendEmpl.ID= this.Reader[2].ToString(); //����Ա����
                    caseLend.StartingTime =NConvert.ToDateTime(this.Reader[3].ToString()); //��ʼ����ʱ��
                    caseLend.EndTime = NConvert.ToDateTime(this.Reader[4].ToString());           //�黹ʱ�� 
                    caseLend.AuditingOper.ID = this.Reader[6].ToString(); //���Ա����
                    caseLend.AuditingOper.OperTime = NConvert.ToDateTime(this.Reader[7].ToString()); //�������ʱ��
                    caseLend.IsAuditing = NConvert.ToBoolean(this.Reader[8].ToString()); //�Ƿ������� 
                    caseLend.IsReturn = NConvert.ToBoolean(this.Reader[9].ToString()); //�Ƿ��Ѿ��黹
                    caseLend.ReturnOper.ID = this.Reader[10].ToString(); //�黹Ա����
                    caseLend.ReturnOper.OperTime = NConvert.ToDateTime(this.Reader[11].ToString()); //ʵ�ʹ黹ʱ��
                    caseLend.ReturnConfirmOper.ID = this.Reader[12].ToString();           //�黹ȷ���˹��� 
                    caseLend.ReturnConfirmOper.OperTime = NConvert.ToDateTime(this.Reader[13].ToString()); //�黹ȷ��ʱ��
                    if (this.Reader[14].ToString().Equals("0"))        //ҵ������
                    {
                        caseLend.LendType = Neusoft.HISFC.Models.HealthRecord.Case.EnumLendType.Lend;
                    }
                    else
                    {
                        caseLend.LendType = Neusoft.HISFC.Models.HealthRecord.Case.EnumLendType.Refer;
                    }                  
                               
                    caseLend = null;
                }

                return List;
            }
            catch (Exception ex)
            {
                this.Err = ex.Message;
                return null;
            }
        }
        #endregion
    
    }
}
