using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: �����ӡ�ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-24]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IReportPrinter
    {
        /// <summary>
        /// ��ӡ
        /// </summary>
        /// <returns></returns>
        int Print();

        /// <summary>
        /// ��ӡԤ��
        /// </summary>
        /// <returns></returns>
        int PrintPreview();

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Export();
    }
}
