using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.BizProcess.Interface.InterFacePassWord
{
    public interface IPassWord
    {
        
        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        bool ValidPassWord
        {
            get;
        }
        /// <summary>
        /// ���￨��
        /// </summary>
        Neusoft.HISFC.Models.RADT.Patient Patient
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���֤����
        /// </summary>
        bool IsOK
        {
            get;
        }
    }
}
