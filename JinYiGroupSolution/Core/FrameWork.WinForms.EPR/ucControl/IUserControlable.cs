using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.EPRControl
{
    /// <summary>
    /// �û��ؼ��ӿ�
    /// </summary>
    public interface IUserControlable
    {

        //װ��
        void Init(object sender, string[] @params);

        //����
        int Save(object sender);

        //��ǰ�Ƿ��ӡ
        bool IsPrint
        {
            get;
            set;
        }

        //ˢ��
        void RefreshUC(object sender, string[] @params);

        //�ж�
        int Valid(object sender);

        System.Windows.Forms.Control FocusedControl { get;}

       
    }


}
