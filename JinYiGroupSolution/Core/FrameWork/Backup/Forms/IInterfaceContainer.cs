using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Forms
{
    /// <summary>
    /// [��������: �ӿ������ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-29]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IInterfaceContainer
    {
        /// <summary>
        /// �����ɵĽӿڵ�����
        /// </summary>
        Type[] InterfaceTypes
        {
            get;
        }
    }
    /// <summary>
    /// [��������: ��չ�ӿ������ӿ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-24]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public interface IExtendInterfaceContainer : IInterfaceContainer
    {
        /// <summary>
        /// �����ɵı���
        /// </summary>
        object[] InterfaceObjects
        {
            set;
        }
    }
}
