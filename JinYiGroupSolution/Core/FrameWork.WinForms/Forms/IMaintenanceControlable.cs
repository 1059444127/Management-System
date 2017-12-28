using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: ������ѯ�ؼ��ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-10-31]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IMaintenanceControlable
    {
        /// <summary>
        /// ��ѯ���尴��
        /// </summary>
        IMaintenanceForm QueryForm
        {
            get;
            set;
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        int Init();
        /// <summary>
        /// ��ѯ��¼
        /// </summary>
        /// <returns></returns>
        int Query();
        /// <summary>
        /// ���Ӽ�¼
        /// </summary>
        /// <returns></returns>
        int Add();
        /// <summary>
        /// ɾ����¼
        /// </summary>
        /// <returns></returns>
        int Delete();
        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <returns></returns>
        int Modify();
        /// <summary>
        /// �����¼
        /// </summary>
        /// <returns>0 �ɹ���-1 ʧ��</returns>
        int Save();
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Import();
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Export();
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
        /// ��ӡ����
        /// </summary>
        /// <returns></returns>
        int PrintConfig();
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Cut();
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Copy();
        /// <summary>
        /// ճ��
        /// </summary>
        /// <returns></returns>
        int Paste();
        /// <summary>
        /// ��һ��
        /// </summary>
        /// <returns></returns>
        int NextRow();
        /// <summary>
        /// ��һ��
        /// </summary>
        /// <returns></returns>
        int PreRow();
        /// <summary>
        /// �Ƿ��޸Ĺ�����δ����
        /// </summary>        
        bool IsDirty
        {
            get;
            set;
        }
    }

    /// <summary>
    /// ά���ؼ�����ʹ�õ�һ����
    /// </summary>
    public class XmlUtil
    {
        public static XmlAttribute GetXmlAttribute(XmlNode node, string name)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name == name)
                {
                    return attribute;
                }
            }

            return null;
        }
    };
}
