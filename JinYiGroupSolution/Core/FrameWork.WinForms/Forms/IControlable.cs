using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: ���ܿؼ��Ľӿ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IControlable
    {

        /// <summary>
        /// ��ʼ���ؼ�����
        /// </summary>
        /// <param name="sender">����TreeView</param>
        /// <param name="neuObject">���뵱ǰѡ����Ϣ,���û����ΪNull</param>
        /// <param name="param">����Tag</param>
        /// <returns></returns>
        ToolBarService Init(object sender, object  neuObject, object param);

        /// <summary>
        /// toolbar Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStrip_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e);

        /// <summary>
        /// ��ʼ����ʼ
        /// </summary>
        event System.EventHandler BeginInit;

        /// <summary>
        /// ��ʼ������
        /// </summary>
        event System.EventHandler EndInit;

        /// <summary>
        /// ������ֵǰ���õĺ���
        /// </summary>
        /// <param name="neuObject">���뵱ǰѡ����Ϣ,���û����ΪNull</param>
        /// <param name="e">����Tag</param>
        /// <returns></returns>
        int BeforSetValue(object neuObject, System.Windows.Forms.TreeViewCancelEventArgs e);

        /// <summary>
        /// ���õ�����ֵ
        /// </summary>
        /// <param name="neuObject">���뵱ǰѡ����Ϣ,���û����ΪNull</param>
        /// <param name="e">����Tag</param>
        /// <returns></returns>
        int SetValue(object neuObject, System.Windows.Forms.TreeNode e);

        /// <summary>
        /// ������ֵ��ʼ
        /// </summary>
        event System.EventHandler BeginSetValue;

        /// <summary>
        /// ������ֵ����
        /// </summary>
        event System.EventHandler EndSetValue;

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="alValues"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        int SetValues(System.Collections.ArrayList alValues,object param);

        /// <summary>
        /// ˢ�����¼�
        /// </summary>
        event System.EventHandler RefreshTree;

        /// <summary>
        /// ��ǰϵͳ��Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        int GetMessage(object sender, string msg);

        /// <summary>
        /// ������Ϣ
        /// </summary>
        event MessageEventHandle SendMessage;

        /// <summary>
        /// ���״̬����ʾ��Ϣ
        /// </summary>
        event MessageEventHandle StatusBarInfo;

        /// <summary>
        /// ���Ͳ������ؼ�
        /// </summary>
        event SendParamToControlHandle SendParamToControl;

        #region addby xuewj 2010-10-5 ����StatusBarPanel {C0E71DA8-F246-4ff2-98CB-7EC72A767453}
        /// <summary>
        /// ������������StatusBarPanel����Ϣ 
        /// </summary>
        event SendIconToStatusBar AddStastusBarPanel;

        #endregion

    }

    #region addby xuewj 2010-10-5 ����StatusBarPanel {C0E71DA8-F246-4ff2-98CB-7EC72A767453}
    /// <summary>
    /// ���Ӵ�ͼ���״̬��
    /// </summary>
    /// <param name="icon">ͼ���ļ�</param>
    /// <param name="msg">��Ϣ</param>
    /// <param name="Index">����λ�� 0,1,2,3</param>
    public delegate void SendIconToStatusBar(System.Drawing.Icon icon, string msg, int Index); 
    #endregion

    /// <summary>
    /// ��Ϣ�¼�����
    /// </summary>
    public delegate void MessageEventHandle(object sender, string msg);
    
    /// <summary>
    /// ���Ͳ������ؼ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="dllName"></param>
    /// <param name="controlName"></param>
    /// <param name="objParams"></param>
    public delegate void SendParamToControlHandle(object sender, string dllName, string controlName,object objParams);
}
