using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Neusoft.FrameWork.Public;

namespace Neusoft.HISFC.BizLogic.Privilege.Model
{

    [Serializable]
    public enum OrganizationType
    {
        //isDepTrue������������ͬ�Ŀ��ң�isDepFalse�������������ͬ�Ŀ���

         /// <summary>
        /// clinic����
        /// </summary>
        [Description("����|isDepTrue")]
        C = 1,
        /// <summary>
        /// InhospitalסԺ
        /// </summary>
        [Description("סԺ|isDepFalse")]
        I = 2,
    
    }

}
