using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.NFC.Interface.Forms
{
    /// <summary>
    /// [��������: ������ѯ����ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-10-31]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IQueryForm
    {
        /// <summary>
        /// �Ƿ���ʾ��Ӱ�ť
        /// </summary>
        bool ShowAddButton
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���ʾ���水ť
        /// </summary>
        bool ShowSaveButton
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���ʾ��ӡ��ť
        /// </summary>
        bool ShowPrintButton
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ���ʾ������ť
        /// </summary>
        bool ShowExportButton
        {
            get;
            set;
        }
    }
}
