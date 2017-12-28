using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.BizLogic.HL7
{
    /// <summary>
    /// [��������: LISҽ���ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2007-05-11]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface ILis
    {
        int PlaceOrder(Neusoft.HISFC.Models.Order.Order order);
        int PlaceOrder(ICollection<Neusoft.HISFC.Models.Order.Order> orders);
        bool CheckOrder(Neusoft.HISFC.Models.Order.Order order);
        int SetPatient(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo);
        int Commit();
        int Rollback();

    }

}