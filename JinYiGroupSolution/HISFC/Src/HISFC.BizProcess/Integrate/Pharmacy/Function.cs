using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Neusoft.HISFC.BizProcess.Integrate
{
    /// <summary>
    /// [��������: ҩƷ��̬������ ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2008-07]<br></br>
    /// <�޸ļ�¼ 
    ///		�޸���='������' 
    ///		�޸�ʱ��='2008-10-16' 
    ///		�޸�Ŀ��='���Ӷ�ҩƷ�������ݰ�����Ŀ��������'
    ///		�޸�����='{1B35A424-0127-42ff-96A4-6835D5DB0151}'
    ///		/>
    /// <˵��>
    /// </˵��>
    /// </summary>
    public class PharmacyMethod
    {
        /// <summary>
        /// ��ҩƷ�������ݰ�����Ŀ��������
        /// </summary>
        /// <param name="alApplyOut"></param>
        public static void SortApplyOutByItemCode(ref System.Collections.ArrayList alApplyOut)
        {
            CompareApplyOutByItemCode compareInstance = new CompareApplyOutByItemCode();
            alApplyOut.Sort(compareInstance);
        }
    }

    /// <summary>
    /// ��Ŀ������
    /// </summary>
    internal class CompareApplyOutByItemCode : IComparer
    {
        public int Compare(object x, object y)
        {
            Neusoft.HISFC.Models.Pharmacy.ApplyOut o1 = (x as Neusoft.HISFC.Models.Pharmacy.ApplyOut).Clone();
            Neusoft.HISFC.Models.Pharmacy.ApplyOut o2 = (y as Neusoft.HISFC.Models.Pharmacy.ApplyOut).Clone();

            string oX = o1.Item.ID;
            string oY = o2.Item.ID;

            int nComp;

            if (oX == null)
            {
                nComp = (oY != null) ? -1 : 0;
            }
            else if (oY == null)
            {
                nComp = 1;
            }
            else
            {
                nComp = string.Compare(oX.ToString(), oY.ToString());
            }

            return nComp;
        }

    }

}
