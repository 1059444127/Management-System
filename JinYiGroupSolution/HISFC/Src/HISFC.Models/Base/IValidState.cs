using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Models.Base
{
    /// <summary>
    /// IInvalid<br></br>
    /// [��������: ʵ����Ч��״̬��ʶ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007-09-14]<br></br>
    /// </summary>
    //[System.Serializable]
    public interface IValidState
    {
        /// <summary>
        /// ��Ч�Ա�ʶö��
        /// </summary>
        Neusoft.HISFC.Models.Base.EnumValidState ValidState
        {
            get;
            set;
        }
    }
}
