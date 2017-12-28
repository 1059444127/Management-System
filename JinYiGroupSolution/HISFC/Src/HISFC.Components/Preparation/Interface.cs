using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Components.Preparation
{
    /// <summary>
    /// ��������ά���ӿ�
    /// </summary>
    public interface IPrescription
    {
        /// <summary>
        /// ��ʾ����
        /// </summary>
        string DisplayTitle
        {
            get;
        }

        Neusoft.FrameWork.WinForms.Controls.ucBaseControl Control
        {
            get;
        }

        Neusoft.HISFC.Models.Pharmacy.Item Drug
        {
            set;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        int Init();

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        int Save(Neusoft.HISFC.Models.Pharmacy.Item item,ref string information);

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        int Delete();

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        /// <returns></returns>
        int Query();

        /// <summary>
        /// ������ϸ
        /// </summary>
        /// <returns></returns>
        int AddNewItem();
    }

    /// <summary>
    /// �Ƽ���ʵ�ֽӿ�
    /// </summary>
    public interface IPreparation
    {
        decimal GetStore(string deptCode,string itemID);

        /// <summary>
        /// ԭ���ϳ���
        /// </summary>
        /// <param name="matierialObj"></param>
        /// <param name="expand"></param>
        /// <param name="stockDept"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        int Output(Neusoft.FrameWork.Models.NeuObject matierialObj, Neusoft.HISFC.Models.Preparation.Expand expand, Neusoft.FrameWork.Models.NeuObject stockDept, System.Data.IDbTransaction t);

        /// <summary>
        /// ԭ��������
        /// </summary>
        /// <param name="materialObj"></param>
        /// <param name="expand"></param>
        /// <param name="applyDept"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        int Apply(Neusoft.FrameWork.Models.NeuObject materialObj, Neusoft.HISFC.Models.Preparation.Expand expand, Neusoft.FrameWork.Models.NeuObject applyDept, System.Data.IDbTransaction t);
    }

    /// <summary>
    /// �������̼�¼
    /// </summary>
    public interface IProcess
    {
        int SetProcess(Neusoft.HISFC.Models.Preparation.EnumState state,Neusoft.HISFC.Models.Preparation.Preparation preparation);
    }

}
