using System;
using Neusoft.FrameWork.Models;
namespace Neusoft.HISFC.Models.HealthRecord
{
    /// <summary>
    /// Baby<br></br>
    /// [��������:Ӥ��������Ϣ�Ǽ�]<br></br>
    /// [�� �� ��: �ſ���]<br></br>
    /// [����ʱ��: 2007-04-2]<br></br>
    /// <�޸ļ�¼ 
    ///		�޸���='' 
    ///		�޸�ʱ��='yyyy-mm-dd' 
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
 
    [Serializable]
    public class Baby : NeuObject
    {
        public Baby()
        {
            //
            // TODO: �ڴ˴����ӹ��캯���߼�
            //
        }
        #region ˽�б���

        //סԺ��ˮ��
        private string inpatientNo;
        //Ӥ����� 
        private int happenNum;
        //�Ա�  
        private string sexCode;
        //�ѳ����  
        private string birthEnd;
        //����
        private float weight;
        //ת��  
        private string babyState;
        //����
        private string breath;
        //  ��Ⱦ  ID���� name ���� 
        private NeuObject infect = new NeuObject();
        //��Ⱦ����   
        private int infectNum;
        //���ȴ���  
        private int salvNum;
        //�ɹ����� 
        private int succNum;
        //������ʽ
        private string birthMod;
        //����Ա ID ���� ��name ����
        private NeuObject oper = new NeuObject();

        #endregion

        #region ����
        /// <summary>
        /// ����Ա ID ���� ��name ����
        /// </summary>
        public NeuObject OperInfo
        {
            get
            {
                if (oper == null)
                {
                    oper = new NeuObject();
                }
                return oper;
            }
            set
            {
                oper = value;
            }
        }

        /// <summary>
        /// ������ʽ
        /// </summary>
        public string BirthMod
        {
            get
            {
                if (birthMod == null)
                {
                    birthMod = "";
                }
                return birthMod;
            }
            set
            {
                birthMod = value;
            }
        }

        /// <summary>
        /// �ɹ�����
        /// </summary>
        public int SuccNum
        {
            get
            {
                return succNum;
            }
            set
            {
                succNum = value;
            }
        }

        /// <summary>
        /// ���ȴ���
        /// </summary>
        public int SalvNum
        {
            get
            {
                return salvNum;
            }
            set
            {
                salvNum = value;
            }
        }

        /// <summary>
        /// ��Ⱦ����
        /// </summary>
        public int InfectNum
        {
            get
            {
                return infectNum;
            }
            set
            {
                infectNum = value;
            }
        }

        /// <summary>
        /// ��Ⱦ
        /// </summary>
        public NeuObject Infect
        {
            get
            {
                if (infect == null)
                {
                    infect = new NeuObject();
                }
                return infect;
            }
            set
            {
                infect = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Breath
        {
            get
            {
                if (breath == null)
                {
                    breath = "";
                }
                return breath;
            }
            set
            {
                breath = value;
            }
        }

        /// <summary>
        /// ת�� 
        /// </summary>
        public string BabyState
        {
            get
            {
                if (babyState == null)
                {
                    babyState = "";
                }
                return babyState;
            }
            set
            {
                babyState = value;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        /// <summary>
        /// �ѳ����  
        /// </summary>
        public string BirthEnd
        {
            get
            {
                if (birthEnd == null)
                {
                    birthEnd = "";
                }
                return birthEnd;
            }
            set
            {
                birthEnd = value;
            }
        }

        /// <summary>
        /// �Ա�  
        /// </summary>
        public string SexCode
        {
            get
            {
                if (sexCode == null)
                {
                    sexCode = "";
                }
                return sexCode;
            }
            set
            {
                sexCode = value;
            }
        }

        /// <summary>
        /// Ӥ�����
        /// </summary>
        public int HappenNum
        {
            get
            {
                return happenNum;
            }
            set
            {
                happenNum = value;
            }
        }

        /// <summary>
        /// סԺ��ˮ��
        /// </summary>
        public string InpatientNo
        {
            get
            {
                if (inpatientNo == null)
                {
                    inpatientNo = "";
                }
                return inpatientNo;
            }
            set
            {
                inpatientNo = value;
            }
        }
        #endregion

        #region  ��¡����


        public new Baby Clone()
        {
            Baby bb = base.Clone() as Baby;
            bb.OperInfo = oper.Clone();
            bb.infect = infect.Clone();
            return bb;
        }
        #endregion

    }
}