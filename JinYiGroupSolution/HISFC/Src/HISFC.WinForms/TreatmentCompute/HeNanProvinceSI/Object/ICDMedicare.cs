using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.HISFC.Models.Base;

namespace HeNanProvinceSI.Object
{

    /// <summary>
    /// [��������: ҽ��ICD]<br></br>
    /// [������:   ������]<br></br>
    /// [����ʱ��: 2007-08-14]<br></br>
    /// <˵��>
    ///    
    /// </˵��>
    /// <�޸ļ�¼>
    ///     <�޸�ʱ��>20090212</�޸�ʱ��>
    ///     <�޸�����>
    ///            ���ػ�
    ///     </�޸�����>
    /// </�޸ļ�¼>
    /// </summary>
    public class ICDMedicare : Spell, IValid
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public ICDMedicare()
        {
        }

        #region ˽��
        /// <summary>
        /// ���к�
        /// </summary>
        private String seqID;

        /// <summary>
        /// ��ͬ��λ
        /// </summary>
        private String icdType;

        /// <summary>
        /// ���ַ���
        /// </summary>
        private string disKind;

        /// <summary>
        /// ʹ�÷�Χ
        /// </summary>
        private string useArea;

        /// <summary>
        /// �������
        /// </summary>
        private string changeDate;
        /// <summary>
        /// ��ϵ���
        /// </summary>
        private string deptKInd;

        /// <summary>
        /// ��Ч��
        /// </summary>
        private bool isValid = true;
        #endregion

        #region ����
        /// <summary>
        /// ���к�
        /// </summary>
        public String SeqID
        {
            get
            {
                return seqID;
            }
            set
            {
                seqID = value;
            }
        }

        /// <summary>
        /// ��ͬ��λ
        /// </summary>
        public String IcdType
        {
            get
            {
                return icdType;
            }
            set
            {
                icdType = value;
            }
        }

        #region {B1EC3100-3E7F-4818-843C-98DE69F03CC2} �¼�����


        /// <summary>
        /// ���ַ���
        /// </summary>
        public string DisKind
        {
            get
            {
                return disKind;
            }
            set
            {
                disKind = value;
            }
        }

        /// <summary>
        /// ʹ�÷�Χ
        /// </summary>
        public string UseArea
        {
            get
            {
                return useArea;
            }
            set
            {
                useArea = value;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        public string ChangeDate
        {
            get
            {
                return changeDate;
            }
            set
            {
                changeDate = value;
            }
        }

        /// <summary>
        /// ��ϵ���
        /// </summary>
        public string DeptKInd
        {
            get
            {
                return deptKInd;
            }
            set
            {
                deptKInd = value;
            }
        }

        #endregion
        #endregion

        #region IValid ��Ա

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
    }
}
