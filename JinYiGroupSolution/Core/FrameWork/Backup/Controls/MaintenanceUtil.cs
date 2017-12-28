using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// [��������: ά���ؼ�����]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-10]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    internal class MaintenanceUtil
    {
        static MaintenanceUtil()
        {
            dict.Add("NeuGroupID", HIS.GroupID);
        }
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        public static string GenSQL(string sql)
        {
            string ret=sql;
            foreach(string s in dict.Keys)
            {
                ret = ret.Replace(s, dict[s]);
            }

            return ret;
        }
    }

    internal static class HIS
    {
        public static string GroupID = "REG";
        public static string OperatorID = "Robin";
        public static string OperateDate = "sysdate";
    };
}
