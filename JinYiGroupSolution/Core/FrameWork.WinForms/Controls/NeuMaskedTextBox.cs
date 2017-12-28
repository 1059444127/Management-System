using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// [��������: Mask�ؼ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-06]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    [ToolboxBitmap(typeof(System.Windows.Forms.MaskedTextBox))]
    public class NeuMaskedTextBox : System.Windows.Forms.MaskedTextBox, INeuControl
    {
        public NeuMaskedTextBox()
        {

        }
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
