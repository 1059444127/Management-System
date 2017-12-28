using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Models.Pharmacy
{
    /// <summary>
    /// [��������: ҩƷ��������]<br></br>
    /// [�� �� ��: liangjz]<br></br>
    /// [����ʱ��: 2008-01]<br></br>
    /// </summary>
    [Serializable]
    public class DrugConstant : Neusoft.FrameWork.Models.NeuObject,Neusoft.HISFC.Models.Base.IValid
    {
        public DrugConstant()
        {

        }

        #region �����

        /// <summary>
        /// �������
        /// </summary>
        private string consType;

        /// <summary>
        /// ����
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject dept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ҩƷ���
        /// </summary>
        private string drugType;

        /// <summary>
        /// ����Ȩ����
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject class2Priv = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ����Ȩ����
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject class3Priv = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ������Ŀ
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject item = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.Base.OperEnvironment oper = new Neusoft.HISFC.Models.Base.OperEnvironment();

        #endregion

        #region ����

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.Base.OperEnvironment Oper
        {
            get
            {
                return oper;
            }
            set
            {
                oper = value;
            }
        }

        /// <summary>
        /// ������Ŀ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        /// <summary>
        /// ����Ȩ����
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Class3Priv
        {
            get
            {
                return class3Priv;
            }
            set
            {
                class3Priv = value;
            }
        }

        /// <summary>
        /// ����Ȩ����
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Class2Priv
        {
            get
            {
                return class2Priv;
            }
            set
            {
                class2Priv = value;
            }
        }

        /// <summary>
        /// ҩƷ���
        /// </summary>
        public string DrugType
        {
            get
            {
                return drugType;
            }
            set
            {
                drugType = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Dept
        {
            get
            {
                return dept;
            }
            set
            {
                dept = value;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        public string ConsType
        {
            get
            {
                return consType;
            }
            set
            {
                consType = value;
            }
        }

        #endregion

        #region IValid ��Ա

        /// <summary>
        /// ��Ч��
        /// </summary>
        private bool isValid = true;

        /// <summary>
        /// ��Ч��
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
            set
            {
                this.isValid = value;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��¡
        /// </summary>
        /// <returns></returns>
        public new DrugConstant Clone()
        {
            DrugConstant info = base.Clone() as DrugConstant;

            info.oper = this.oper.Clone();
            info.item = this.item.Clone();
            info.class2Priv = this.class2Priv.Clone();
            info.class3Priv = this.class3Priv.Clone();
            info.dept = this.dept.Clone();

            return info;
        }

        #endregion
    }
}
