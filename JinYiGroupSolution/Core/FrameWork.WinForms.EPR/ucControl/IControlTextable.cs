using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace Neusoft.FrameWork.EPRControl
{
    public interface IControlTextable
    {

        Control  FocedControl
        {
            get;
        }

        //����QCʵ��
        ArrayList GetQCData();

        event EventHandler Enter;

        //��ǰrtf
        string Rtf
        {
            get;
            set;
        }

        //Super�ı�
        string SuperText
        {
            get;
            set;
        }

        //һ��ҽ���޸�
        string Level1Rtf
        {
            get;
            set;
        }

        //����ҽ���޸�
        string Level2Rtf
        {
            get;
            set;
        }

        //��ʾһ��ҽ���޸���Ϣ
        void ShowLevel1Text();

        //��ʾ����ҽ���޸���Ϣ
        void ShowLevel2Text();

        //��ʾ����ҽ���޸���Ϣ
        void ShowLevel3Text();

        //�Ƿ񱣴�Super�ı�
        bool IsOnSaveSuperText
        {
            get;
            set;
        }
    }

}
