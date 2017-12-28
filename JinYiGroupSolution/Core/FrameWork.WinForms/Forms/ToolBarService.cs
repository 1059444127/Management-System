using System;
using System.Windows.Forms;
namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: ���ര���õ�ToolBar������]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class ToolBarService
    {
        public ToolBarService()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        protected System.Collections.ArrayList toolButtons = new System.Collections.ArrayList();
        
        /// <summary>
        /// ���ToolButton
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tooltip"></param>
        /// <param name="imageIndex"></param>
        /// <param name="e"></param>
        public void AddToolButton(string text, string tooltip, int imageIndex, bool enabled, bool isChecked, System.EventHandler e)
        {
            ToolStripButton tb = new ToolStripButton(text);
            tb.Tag = e;
            tb.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage((Neusoft.FrameWork.WinForms.Classes.EnumImageList)imageIndex);
            tb.Enabled = enabled;
            tb.Checked = isChecked;         //Robin Add
            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD}  ��������ת��
            tb.ToolTipText = Neusoft.FrameWork.Management.Language.Msg( tooltip );

            tb.ImageScaling = ToolStripItemImageScaling.SizeToFit;   //Robin AddSizeToFit
            this.toolButtons.Add(tb);

        }

        /// <summary>
        /// ���ToolButton
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tooltip"></param>
        /// <param name="imageIndex"></param>
        /// <param name="enabled"></param>
        /// <param name="isChecked"></param>
        /// <param name="e"></param>
        /// Robin   2007-01-05 
        public void AddToolButton(string text, string tooltip, Neusoft.FrameWork.WinForms.Classes.EnumImageList imageIndex, bool enabled, bool isChecked, System.EventHandler e)
        {
            ToolStripButton tb = new ToolStripButton(text);
            tb.Tag = e;
            tb.Image = Neusoft.FrameWork.WinForms.Classes.Function.GetImage(imageIndex);
            tb.Enabled = enabled;
            tb.Checked = isChecked;         //Robin Add
            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD}  ��������ת��
            tb.ToolTipText = Neusoft.FrameWork.Management.Language.Msg( tooltip );
            tb.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            this.toolButtons.Add(tb);

        }

        /// <summary>
        /// ���Ĭ��ToolButton
        /// 
        /// {BA871A23-D5AB-42c8-9C8A-B19339B991FC}
        /// </summary>
        /// <param name="defaultButton">Ĭ��Button</param>
        public void AddToolButton(ToolStripButton defaultButton)
        {
            this.toolButtons.Add( defaultButton );
        }

        /// <summary>
        /// ���ToolButton
        /// </summary>
        public void Clear()
        {
            this.toolButtons.Clear();
        }

        /// <summary>
        /// ����toolButton��ť�ɲ�����
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enabled"></param>
        public void SetToolButtonEnabled(string text, bool enabled)
        {
            foreach (ToolStripButton tb in this.toolButtons)
            {
                //ȥ��ToolButton�еĿ�ݼ�
                if (GetToolButtonText(tb.Text) == text)
                    tb.Enabled = enabled;

            }
        }
        /// <summary>
        /// ȥ��ToolButton�еĿ�ݼ��Լ�������ת��
        /// </summary>
        /// <param name="text">��ǰbutton�Ĳ˵�����</param>
        /// <returns>ȥ����ݼ��������</returns>
        private string GetToolButtonText(string text)
        {
            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ת��������
            string originalStr = text;

            int index = text.IndexOf('(');
            if (index < 0)
            {
                originalStr = text;
            }
            else
            {
                originalStr = text.Substring( 0, index );
            }

            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ת��������
            if (ToolBarButtonService.TranslateTextDictionary.ContainsKey( originalStr ) == true)
            {
                originalStr = ToolBarButtonService.TranslateTextDictionary[originalStr];
            }

            return originalStr;
        }

        /// <summary>
        /// �������ToolButton
        /// </summary>
        /// <returns></returns>
        public System.Collections.ArrayList GetToolButtons()
        {
            
            return this.toolButtons;
        }

        public System.Windows.Forms.ToolStripButton GetToolButton(string text)
        {
            foreach (ToolStripButton tb in this.toolButtons)
            {
                //ȥ����ݼ�
                if (GetToolButtonText(tb.Text) == text)
                    return tb;

            }
            return null;
        }

        protected bool IsHaveButton(string text)
        {
            return false;
        }

        /// <summary>
        /// ���ӷָ���
        /// </summary>
        public void AddToolSeparator()
        {
            ToolStripSeparator _sp = new ToolStripSeparator();
            this.toolButtons.Add(_sp);
        }
    }
}
