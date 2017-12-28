using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// �û����������ܷ���ӿ�
    /// ������ӹ�������ť����
    /// ����Ŀ¼������Ŀ¼/
    /// </summary>
    public interface IToolBarService
    {
        /// <summary>
        /// ��ʼ��������ToolStrip
        /// ��֧��ToolBar
        /// </summary>
        /// <param name="toolbar"></param>
        /// <returns></returns>
        int Init(System.Windows.Forms.ToolStrip toolbar);


        /// <summary>
        /// ������Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="param"></param>
        void ToolBarClick(object sender, object param);

        /// <summary>
        /// �����仯
        /// </summary>
        /// <param name="param"></param>
        void InfoChanged(object param);

    }

}


