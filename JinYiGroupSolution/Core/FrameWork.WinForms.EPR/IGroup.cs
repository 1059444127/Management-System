using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace Neusoft.FrameWork.EPRControl
{
    public  delegate void NameChangedEventHandler(object sender, System.EventArgs e);
    public  delegate void GroupChangedEventHandler(object sender, System.EventArgs e); 
    public  delegate void IsGroupChangedEventHandler(object sender, System.EventArgs e);
    /// <summary>
    /// ��ӿ�
    /// </summary>
    public interface IGroup
    {
        event NameChangedEventHandler NameChanged;
        event IsGroupChangedEventHandler IsGroupChanged;
        event GroupChangedEventHandler GroupChanged;     

        [CategoryAttribute("���"), Browsable(true), DescriptionAttribute("���ÿؼ����ƣ�Ҳ�ǽ�����ƣ����ܰ���'�ո�\\,-,(,),,.%�������ַ�'")]
        string ����
        {
            get;
            set;
        }
        [TypeConverter(typeof(emrGroup)), CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("ѡ��ؼ�������")]
        string ��
        {
            get;
            set;
        }      
       
        [CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("�Ƿ��Ǹ����!")]
        bool �Ƿ���
        {
            get;
            set;
        }
        [ CategoryAttribute("���"), DefaultValueAttribute(""), DescriptionAttribute("Snomed����")]
        string Snomed
        {
            get;
            set;
        }    
    }
}
