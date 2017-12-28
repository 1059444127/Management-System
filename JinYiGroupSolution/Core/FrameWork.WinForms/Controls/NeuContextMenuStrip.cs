using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// [��������: NeuContextMenuStrip�ؼ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-14]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    [ToolboxBitmap(typeof(ContextMenuStrip))]
    public class NeuContextMenuStrip : ContextMenuStrip
    {

        private StyleType styleType;

        #region INeuControl ��Ա

        public Neusoft.FrameWork.WinForms.Controls.StyleType Style
        {
            get
            {
                return this.styleType;
            }
            set
            {
                this.styleType = value;
            }
        }

        #endregion
    }
}
