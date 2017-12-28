/*----------------------------------------------------------------
            // Copyright (C) ������������ɷ����޹�˾
            // ��Ȩ���С� 
            //
            // �ļ�����			HomeBedRate.cs
            // �ļ�����������	��ͥ����������
            //
            // 
            // ������ʶ��		2006-5-16
            //
            // �޸ı�ʶ��
            // �޸�������
            //
            // �޸ı�ʶ��
            // �޸�������
//----------------------------------------------------------------*/

using System;
using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Models.Fee.Outpatient
{
    /// <summary>
    /// ��ͥ����������
    /// </summary>
    /// 
    [System.Serializable]
    public class EcoRate : Neusoft.FrameWork.Models.NeuObject
    {
        public EcoRate()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /// <summary>
        /// ҽԺ�������
        /// [ID - ��������]
        /// [Name - ��������]
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject hospital = new NeuObject();
        /// <summary>
        /// �������
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject rateType = new NeuObject();
        /// <summary>
        /// ��Ŀ����
        /// [0 - ����]
        /// [1 - ��С����]
        /// [2 - ��Ŀ]
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject itemType = new NeuObject();
        /// <summary>
        /// ��Ŀ
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject item = new NeuObject();
        /// <summary>
        /// �Żݱ���
        /// </summary>
        Neusoft.HISFC.Models.Base.FTRate rate = new Neusoft.HISFC.Models.Base.FTRate();
        /// <summary>
        /// ����Ա
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject currentOperator = new NeuObject();
        /// <summary>
        /// ����ʱ��
        /// </summary>
        System.DateTime operateDateTime = DateTime.MinValue;

        /// <summary>
        /// ҽԺ�������
        /// [ID - ��������]
        /// [Name - ��������]
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Hospital
        {
            get
            {
                return this.hospital;
            }
            set
            {
                this.hospital = value;
            }
        }
        /// <summary>
        /// ������������
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject RateType
        {
            get
            {
                return this.rateType;
            }
            set
            {
                this.rateType = value;
            }
        }
        /// <summary>
        /// ��Ŀ����
        /// [0 - ����]
        /// [1 - ��С����]
        /// [2 - ��Ŀ]
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject ItemType
        {
            get
            {
                return this.itemType;
            }
            set
            {
                this.itemType = value;
            }
        }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Item
        {
            get
            {
                return this.item;
            }
            set
            {
                this.item = value;
            }
        }
        /// <summary>
        /// �����Żݱ���
        /// </summary>
        public Neusoft.HISFC.Models.Base.FTRate Rate
        {
            get
            {
                return this.rate;
            }
            set
            {
                this.rate = value;
            }
        }
        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject CurrentOperator
        {
            get
            {
                return this.currentOperator;
            }
            set
            {
                this.currentOperator = value;
            }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public System.DateTime OperateDateTime
        {
            get
            {
                return this.operateDateTime;
            }
            set
            {
                this.operateDateTime = value;
            }
        }


        public new EcoRate Clone()
        {
            EcoRate ecoRate = base.Clone() as EcoRate;
            ecoRate.CurrentOperator = this.CurrentOperator.Clone();
            ecoRate.Hospital = this.Hospital.Clone();
            ecoRate.Item = this.Item.Clone();
            ecoRate.ItemType = this.ItemType.Clone();
            ecoRate.RateType = this.RateType.Clone();
            return ecoRate;
        }
    }
}
