using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.Models.Equipment
{
    [System.Serializable]
    public class PayMain:Neusoft.FrameWork.Models.NeuObject
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public PayMain() 
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        #region �ֶ�

        /// <summary>
        /// �����̽����ˮ��
        /// </summary>
        private string payNo;

        /// <summary>
        /// �豸������Ϣ
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject deptInfo = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ��浥�ݺ�
        /// </summary>
        private string payListCode;

        /// <summary>
        /// ������˾��Ϣ
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject companyInfo = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// Ӧ������
        /// </summary>
        private decimal dueCost;

        /// <summary>
        /// �Ѹ�����
        /// </summary>
        private decimal payCost;

        /// <summary>
        /// �Ƿ���0��1��
        /// </summary>
        private string offFlag;

        /// <summary>
        /// ����Ա��Ϣ
        /// </summary>
        private OperEnvironment operInfo = new OperEnvironment();

        #endregion

        #region ����

        /// <summary>
        /// �����̽����ˮ��
        /// </summary>
        public string PayNo
        {
            get { return payNo; }
            set { payNo = value; }
        }

        /// <summary>
        /// �豸������Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject DeptInfo
        {
            get { return deptInfo; }
            set { deptInfo = value; }
        }

        /// <summary>
        /// ��浥�ݺ�
        /// </summary>
        public string PayListCode
        {
            get { return payListCode; }
            set { payListCode = value; }
        }

        /// <summary>
        /// ������˾��Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject CompanyInfo
        {
            get { return companyInfo; }
            set { companyInfo = value; }
        }

        /// <summary>
        /// Ӧ������
        /// </summary>
        public decimal DueCost
        {
            get { return dueCost; }
            set { dueCost = value; }
        }

        /// <summary>
        /// �Ѹ�����
        /// </summary>
        public decimal PayCost
        {
            get { return payCost; }
            set { payCost = value; }
        }

        /// <summary>
        /// �Ƿ���0��1��
        /// </summary>
        public string OffFlag
        {
            get { return offFlag; }
            set { offFlag = value; }
        }

        /// <summary>
        /// ����Ա��Ϣ
        /// </summary>
        public OperEnvironment OperInfo
        {
            get { return operInfo; }
            set { operInfo = value; }
        }

        #endregion

        #region ����

        public new PayMain Clone() 
        {
            PayMain payMain = base.Clone() as PayMain;

            payMain.deptInfo = this.deptInfo.Clone();
            payMain.companyInfo = this.companyInfo.Clone();
            payMain.operInfo = this.operInfo.Clone();

            return payMain;
        }

        #endregion
    }
}
