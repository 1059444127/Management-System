using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.NFC.Interface.Forms
{
    /// <summary>
    /// [��������: ˫����ѯ���ڵ������޸Ŀؼ��ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-03]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IQueryModifyControl
    {
        /// <summary>
        /// ����
        /// </summary>
        List<string> Data
        {
            get;
            set;
        }
    }
}
