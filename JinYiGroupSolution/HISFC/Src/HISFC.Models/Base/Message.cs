using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Models.Base
{
    /// <summary>
    /// ID���룬Name��Ϣ����
    /// </summary>
    [Serializable]
    public class Message : Neusoft.FrameWork.Models.NeuObject
    {
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject sender = new Neusoft.FrameWork.Models.NeuObject();

        public Neusoft.FrameWork.Models.NeuObject Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject receiver = new Neusoft.FrameWork.Models.NeuObject();

        public Neusoft.FrameWork.Models.NeuObject Receiver
        {
            get { return receiver; }
            set { receiver = value; }
        }
        /// <summary>
        /// �����˿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject senderDept = new Neusoft.FrameWork.Models.NeuObject();

        public Neusoft.FrameWork.Models.NeuObject SenderDept
        {
            get { return senderDept; }
            set { senderDept = value; }
        }
        /// <summary>
        /// �����˿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject receiverDept = new Neusoft.FrameWork.Models.NeuObject();

        public Neusoft.FrameWork.Models.NeuObject ReceiverDept
        {
            get { return receiverDept; }
            set { receiverDept = value; }
        }
        /// <summary>
        /// �Ƿ����״̬ 1���Ķ� 0δ�Ķ�
        /// </summary>
        private bool isRecieved = false;

        public bool IsRecieved
        {
            get { return isRecieved; }
            set { isRecieved = value; }
        }
        /// <summary>
        /// ��Ϣ��������  0���Ķ� 1 �ѻظ� 2 �Ѵ���
        /// </summary>
        private int replyType;

        public int ReplyType
        {
            get { return replyType; }
            set { replyType = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        private OperEnvironment oper = new OperEnvironment();

        public OperEnvironment Oper
        {
            get { return oper; }
            set { oper = value; }
        }

        /// <summary>
        /// ���߲���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject emr = new Neusoft.FrameWork.Models.NeuObject();

        public Neusoft.FrameWork.Models.NeuObject Emr
        {
            get { return emr; }
            set { emr = value; }
        }
        /// <summary>
        /// ����סԺ��ˮ��
        /// </summary>
        private string inpatientNo;

        public string InpatientNo
        {
            get { return inpatientNo; }
            set { inpatientNo = value; }
        }

        
    }
}
