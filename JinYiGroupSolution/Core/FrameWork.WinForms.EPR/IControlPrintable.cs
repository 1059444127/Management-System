using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace Neusoft.FrameWork.EPRControl
{
    /// <summary>
    /// �µĴ�ӡ�ӿ�
    /// Ϊ���Ӳ����ṩ
    /// </summary>
    public interface IControlPrintable
    {
        System.Windows.Forms.Control PrintControl();
        System.Collections.ArrayList arrSortedControl();
        //string CheckPrint();
        void continuePrint(PrintPageEventArgs e, Rectangle rectangle, Graphics grap);
        void Print(PrintPageEventArgs e, Rectangle rectangle, Graphics grap);
        //void SetText(string fileID);
    }
}
