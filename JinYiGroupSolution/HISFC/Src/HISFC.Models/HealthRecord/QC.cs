using System;


namespace Neusoft.HISFC.Models.HealthRecord
{


    /// <summary>
    /// ������������ʵ�塣
    /// �̳�Neusoft.FrameWork.Models.NeuObject
    /// Neusoft.FrameWork.Models.NeuObject.ID ����Ա���� Neusoft.FrameWork.Models.NeuObject.Name ����Ա����
    ///
    /// 
    /// </summary>
    [Serializable]
    public class QC : Neusoft.FrameWork.Models.NeuObject
    {
        public QC()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        //˽���ֶ�
        private string myInpatientNO;
        private Neusoft.FrameWork.Models.NeuObject myRuleInfo = new Neusoft.FrameWork.Models.NeuObject();
        private decimal myMark;
        private string myDenyFlag;
        private DateTime myOperDate;
        private Neusoft.HISFC.Models.Base.OperEnvironment operInfo = new Neusoft.HISFC.Models.Base.OperEnvironment();

        /// <summary>
        /// ����Ա��
        /// </summary>
        public Neusoft.HISFC.Models.Base.OperEnvironment OperInfo
        {
            get
            {
                return operInfo;
            }
            set
            {
                operInfo = value;
            }
        }
        /// <summary>
        /// סԺ��ˮ��
        /// </summary>
        public string InpatientNO
        {
            get
            {
                return myInpatientNO;
            }
            set
            {
                myInpatientNO = value;
            }
        }

        /// <summary>
        /// ������Ϣ ID ������� Name ������Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject RuleInfo
        {
            get
            {
                return myRuleInfo;
            }
            set
            {
                myRuleInfo = value;
            }
        }

        /// <summary>
        /// �÷�
        /// </summary>
        public decimal Mark
        {
            get
            {
                return myMark;
            }
            set
            {
                myMark = value;
            }
        }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public string DenyFlag
        {
            get
            {
                return myDenyFlag;
            }
            set
            {
                myDenyFlag = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime OperDate
        {
            get
            {
                return myOperDate;
            }
            set
            {
                myOperDate = value;
            }
        }

        public new QC Clone()
        {
            QC QCCLone = base.MemberwiseClone() as QC;

            QCCLone.myRuleInfo = this.myRuleInfo.Clone();

            return QCCLone;
        }

    }
}
