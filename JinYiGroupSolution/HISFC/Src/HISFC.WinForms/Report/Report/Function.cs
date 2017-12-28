using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Neusoft.WinForms.Report.Function
{
    public class DrugDosage
    {
        private static Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// ���Ͱ�����
        /// </summary>
        private static Neusoft.FrameWork.Public.ObjectHelper dosageHelper = null;

        /// <summary>
        /// ����
        /// </summary>
        private static System.Collections.Hashtable hsDosage = new Hashtable();

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="drugCode"></param>
        /// <returns></returns>
        public static string GetStaticDosage(string drugCode)
        {
            if (hsDosage.ContainsKey(drugCode))
            {
                if (hsDosage[drugCode] != null)
                {
                    return hsDosage[drugCode].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if (dosageHelper == null)
                {
                    InitData();
                }
                Neusoft.HISFC.Models.Pharmacy.Item item = itemManager.GetItem(drugCode);
                hsDosage.Add(drugCode, dosageHelper.GetName(item.DosageForm.ID));
                return dosageHelper.GetName(item.DosageForm.ID);
            }
        }

        /// <summary>
        /// �������ݳ�ʼ��
        /// </summary>
        private static void InitData()
        {
            Neusoft.HISFC.BizLogic.Manager.Constant consManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
            ArrayList alList = consManager.GetAllList(Neusoft.HISFC.Models.Base.EnumConstant.DOSAGEFORM);
            if (alList != null)
            {
                dosageHelper = new Neusoft.FrameWork.Public.ObjectHelper(alList);
            }
        }
    }
}
