using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: ToolBarButton������]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class ToolBarButtonService
    {
        /// <summary>
        /// �����ı�������  ���������ı���ԭʼ�ı�����  {1B10BCB7-8133-4282-8479-9C41FE5A23FD}
        /// </summary>
        internal static Dictionary<string, string> translateTextDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Ĭ�ϣ���
        /// </summary>
        protected static int ImageSize = 24;

        /// <summary>
        /// 0 �£�������...
        /// </summary>
        protected static int TextPosition = 0;

        /// <summary>
        /// �����ı�������  ���������ı���ԭʼ�ı�����  {1B10BCB7-8133-4282-8479-9C41FE5A23FD}
        /// </summary>
        public static Dictionary<string, string> TranslateTextDictionary
        {
            get
            {
                return translateTextDictionary;
            }
        }

        /// <summary>
        /// ��հ�ť
        /// </summary>
        /// <param name="toolbar"></param>
        public static void ClearButton(ToolStrip toolbar)
        {
            for (int i = toolbar.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem tb = toolbar.Items[i];
                if (tb.Tag != null && tb.Tag.ToString() == "Default")
                {
                }
                else
                {
                    toolbar.Items.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// ���ð�ť����
        /// </summary>
        /// <param name="imageSize"></param>
        /// <param name="textPosition"></param>
        public static void SetButtonProperty(int imageSize, int textPosition)
        {
            ImageSize = imageSize;
            TextPosition = textPosition;
        }

        /// <summary>
        /// ���ð�ť����
        /// </summary>
        /// <param name="tb"></param>
        public static void SetButtonProperty(ToolStripItem tb)
        {
            tb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tb.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            switch (TextPosition)
            {
                case 0:
                    tb.TextImageRelation = TextImageRelation.ImageAboveText;
                    break;
                case 1:
                    tb.TextImageRelation = TextImageRelation.ImageBeforeText;
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    tb.TextImageRelation = TextImageRelation.ImageAboveText;
                    break;
            }
        }

        /// <summary>
        /// ����ToolBar
        /// </summary>
        /// <param name="tb1"></param>
        /// <param name="tb2"></param>
        /// <param name="al1"></param>
        /// <param name="al2"></param>
        public static void ChangeButton(ToolStrip tb1, ToolStrip tb2, ArrayList al1, ArrayList al2)
        {
            if (al1.Count == 0 && al2.Count == 0)
            {
                return;
            }

            tb1.SuspendLayout();
            tb2.SuspendLayout();
            ArrayList tbAll = new ArrayList();
            foreach (ToolStripItem tb in tb1.Items)
            {
                tbAll.Add(tb);
            }
            foreach (ToolStripItem tb in tb2.Items)
            {
                tbAll.Add(tb);
            }
            tb1.Items.Clear();
            tb2.Items.Clear();
            int index = 0;
            string tempText = string.Empty;
            string textValue = string.Empty;
            foreach (string text in al1)
            {
                if (text.Trim() == "-")
                {
                    ToolStripSeparator tb = new ToolStripSeparator();
                    tb1.Items.Add(tb);
                }
                else
                {
                    foreach (ToolStripItem tb in tbAll)
                    {
                        //��ݼ�
                        //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ��������ת��
                        string shortKeyStr = string.Empty;

                        index = text.Trim().IndexOf( '(' );
                        if (index > 0)
                        {
                            tempText = text.Trim().Substring( 0, index );
                            shortKeyStr = text.Trim().Substring( index );
                        }
                        else
                        {
                            tempText = text.Trim();
                        }

                        //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ��������ת��
                        tempText = Neusoft.FrameWork.Management.Language.Msg( tempText );

                        textValue = tb.Text.Trim();
                        index = textValue.IndexOf('(');
                        if (index > 0)
                        {
                            textValue = textValue.Substring(0, index);
                        }

                        //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ��������ת��
                        textValue = Neusoft.FrameWork.Management.Language.Msg( textValue );

                        //if (tb.Text.Trim() == tempText.Trim())
                        if (textValue == tempText.Trim())
                        {
                            tb.Text = text.Trim();

                            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ��������ת��
                            if (translateTextDictionary.ContainsKey( textValue ) == false)      //���������ı�
                            {
                                translateTextDictionary.Add( textValue, text.Trim() );        //Key ���������ı� Value ԭʼ�ı�
                            }
                            tb.Text = textValue + shortKeyStr;
                            //{1B10BCB7-8133-4282-8479-9C41FE5A23FD} ��������ת��

                            SetButtonProperty(tb);
                            tb1.Items.Add(tb);
                            break;
                        }
                    }

                }
            }

            foreach (string text in al2)
            {
                if (text.Trim() == "-")
                {
                    ToolStripSeparator tb = new ToolStripSeparator();
                    tb2.Items.Add(tb);
                }
                else
                {
                    foreach (ToolStripItem tb in tbAll)
                    {
                        if (tb.Text.Trim() == text.Trim())
                        {
                            SetButtonProperty(tb);
                            tb2.Items.Add(tb);
                            break;
                        }
                    }
                }
            }
            tb1.ResumeLayout(true);
            tb2.ResumeLayout(true);
        }


    }
}
