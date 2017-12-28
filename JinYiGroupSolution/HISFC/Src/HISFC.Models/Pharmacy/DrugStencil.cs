using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.Models.Pharmacy
{
    /// <summary>
    /// [��������: ҩƷģ����]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-12]<br></br>
    /// </summary>
    [Serializable]
    public class DrugStencil : Neusoft.FrameWork.Models.NeuObject
    {
        public DrugStencil()
        {

        }

        #region ����

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject dept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ģ������
        /// </summary>
        private Pharmacy.DrugStencilEnumService openType = new DrugStencilEnumService();

        /// <summary>
        /// ģ����Ϣ
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject stencil = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ҩƷ��Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.Pharmacy.Item item = new Item();

        /// <summary>
        /// ˳���
        /// </summary>
        private int sortNO;

        /// <summary>
        /// ����Ա
        /// </summary>
        private Neusoft.HISFC.Models.Base.OperEnvironment oper = new Neusoft.HISFC.Models.Base.OperEnvironment();

        /// <summary>
        /// ��չ��Ϣ
        /// </summary>
        private string extend;

        #endregion

        #region ����

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Dept
        {
            get
            {
                return this.dept;
            }
            set
            {
                this.dept = value;
            }
        }

        /// <summary>
        /// ģ������
        /// </summary>
        public DrugStencilEnumService OpenType
        {
            get
            {
                return this.openType;
            }
            set
            {
                this.openType = value;
                if (value.ID != null)
                    base.Memo = value.ID.ToString();
            }
        }

        /// <summary>
        /// ģ����Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Stencil
        {
            get
            {
                return this.stencil;
            }
            set
            {
                this.stencil = value;
                base.ID = value.ID;
                base.Name = value.Name;
            }
        }
        /// <summary>
        /// ҩƷ��Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.Pharmacy.Item Item
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
        /// ˳���
        /// </summary>
        public int SortNO
        {
            get
            {
                return this.sortNO;
            }
            set
            {
                this.sortNO = value;
            }
        }

        /// <summary>
        /// ����Ա
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

        /// <summary>
        /// ��չ�ֶ�
        /// </summary>
        public string Extend
        {
            get
            {
                return this.extend;
            }
            set
            {
                this.extend = value;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��¡����
        /// </summary>
        /// <returns>�ɹ����ص�ǰʵ���Ŀ�¡��Ϣ</returns>
        public new DrugStencil Clone()
        {
            DrugStencil drugStencil = base.Clone() as DrugStencil;
            drugStencil.dept = this.dept.Clone();
            drugStencil.stencil = this.stencil.Clone();
            drugStencil.item = this.item.Clone();
            drugStencil.oper = this.oper.Clone();

            return drugStencil;
        }

        #endregion
    }
}
