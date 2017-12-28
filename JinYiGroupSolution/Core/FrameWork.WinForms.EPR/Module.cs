using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Neusoft.FrameWork.EPRControl
{
    public class Module
    {
        //�ж��ַ��Ƿ���Ϲ���
        //true ����,false ������
        public static bool ValidName(string s)
        {
            bool wrong=false;
            if ((s.Trim() == "")) wrong = true;
            if ((s.IndexOf("\\") >= 0)) wrong = true;
            if ((s.IndexOf("/") >= 0)) wrong = true;
            if ((s.IndexOf(">") >= 0)) wrong = true;
            if ((s.IndexOf("<") >= 0)) wrong = true;
            if ((s.IndexOf("=") >= 0)) wrong = true;
            if ((s.IndexOf(".") >= 0)) wrong = true;
            if ((s.IndexOf(",") >= 0)) wrong = true;
            if ((s.IndexOf("%") >= 0)) wrong = true;
            if (wrong)
            {
                MessageBox.Show("���Ʋ��ܰ����Ƿ��ַ���", "��ʾ", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }
        //�жϺϷ���
        public static bool ValidText(string s)
        {
            if ((s.Trim() == "-" | s.Trim() == ""))
            {
                return false;
            }
            return true;
        }
    }

}
