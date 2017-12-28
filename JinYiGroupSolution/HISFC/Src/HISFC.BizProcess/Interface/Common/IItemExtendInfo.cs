using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Neusoft.HISFC.BizProcess.Interface.Common
{
    /// <summary>
    /// ��ȡ��Ŀ��չ��Ϣ�ӿ�
    /// �˽ӿ�ʹ����ҽ����Ŀ�б���
    /// </summary>
    public interface IItemExtendInfo
    {
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        Neusoft.HISFC.Models.Base.EnumItemType ItemType
        {
            get;
            set;
        }

        /// <summary>
        /// ��ͬ��λ��Ϣ
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject PactInfo
        {
            get;
            set;
        }

        /// <summary>
        /// ȡ��չ��Ϣ
        /// </summary>
        /// <param name="ItemID">��Ŀ����</param>
        /// <param name="ExtendInfoTxt">������չ��Ϣ�ı�</param>
        /// <param name="AlExtendInfo">������չ��Ϣ����</param>
        /// <returns></returns>
        int GetItemExtendInfo(string ItemID, ref string ExtendInfoTxt, ref ArrayList AlExtendInfo);

    }
}
