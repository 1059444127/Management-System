using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Components.Terminal.Booking
{
    /// <summary>
    /// ҽ��ԤԼ����ӡ�ӿ�
    /// </summary>
    public interface IBookingPrint : Neusoft.FrameWork.WinForms.Forms.IReportPrinter
    {
        /// <summary>
        /// ҽ��ԤԼʵ��
        /// </summary>
        /// <param name="obj"></param>
        void SetValue(Neusoft.HISFC.Models.Terminal.MedTechBookApply obj);
        /// <summary>
        /// ��ս���
        /// </summary>
        void Reset();
    }
}
