using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Models.Base
{
    /// <summary>
    /// Const<br></br>
    /// [��������: ����ֶ���]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007-04]<br></br>
    /// </summary>
    [System.Serializable]
    public class ShiftProperty : Neusoft.FrameWork.Models.NeuObject
    {
        public ShiftProperty()
        {

        }

        #region �����

        /// <summary>
        /// ��ȫ�� �����ռ�
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject reflectType = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ���¼�������
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject property = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ��������
        /// </summary>
        private string propertyDescription;

        /// <summary>
        /// �Ƿ��¼���
        /// </summary>
        private bool isRecord = false;

        /// <summary>
        /// ���ԭ��
        /// </summary>
        private string shiftCause;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.Base.OperEnvironment oper = new OperEnvironment();

        #endregion

        #region ����

        /// <summary>
        /// ������ȫ�� �����ռ�
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject ReflectClass
        {
            get
            {
                return reflectType;
            }
            set
            {
                reflectType = value;

                if (value != null)
                {
                    base.ID = value.ID;
                    base.Name = value.Name;
                }
            }
        }

        /// <summary>
        /// ���¼�������
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Property
        {
            get
            {
                return this.property;
            }
            set
            {
                this.property = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string PropertyDescription
        {
            get
            {
                return this.propertyDescription;
            }
            set
            {
                this.propertyDescription = value;
            }
        }

        /// <summary>
        /// �Ƿ��¼���
        /// </summary>
        public bool IsRecord
        {
            get
            {
                return this.isRecord;
            }
            set
            {
                this.isRecord = value;
            }
        }

        /// <summary>
        /// ���ԭ��
        /// </summary>
        public string ShiftCause
        {
            get
            {
                return this.shiftCause;
            }
            set
            {
                this.shiftCause = value;
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.Base.OperEnvironment Oper
        {
            get
            {
                return this.oper;
            }
            set
            {
                this.oper = value;
            }
        }
        #endregion

        #region ����

        /// <summary>
        /// ��¡
        /// </summary>
        /// <returns></returns>
        public new ShiftProperty Clone()
        {
            ShiftProperty sf = base.Clone() as ShiftProperty;

            sf.reflectType = reflectType.Clone();

            sf.property = property.Clone();

            sf.oper = oper.Clone();

            return sf;
        }

        #endregion
    }
}
