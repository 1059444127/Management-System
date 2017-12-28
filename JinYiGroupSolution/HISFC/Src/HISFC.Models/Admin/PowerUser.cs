using System;
using System.Collections;


namespace Neusoft.HISFC.Models.Admin
{


    /// <summary>
    /// User ��ժҪ˵����
    /// </summary>
    /// 
    [System.Serializable]
    public class PowerUser : Neusoft.FrameWork.Models.NeuObject
    {
        public PowerUser()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�

            //

            Department = new Neusoft.FrameWork.Models.NeuObject();
            GrantDepartment = new Neusoft.FrameWork.Models.NeuObject();

            Department.ID = "";
            Department.Name = "";
        }

        //ID;
        //Name;

        /// <summary>
        /// Ȩ�޲���
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Department;

        public Neusoft.FrameWork.Models.NeuObject GrantDepartment;


        private IList powerDetails;
        private IList roleDetails;


        public string PowerClass1;
        public string PowerClass2;
        public string PowerClass3;


        /// <summary>
        /// ��Ա����չȨ��
        /// </summary>
        public IList PowerDetails
        {
            get
            {
                return this.powerDetails;
            }
            set
            {
                this.powerDetails = value;
            }
        }

        public IList RoleDetails
        {
            get
            {
                return this.roleDetails;
            }
            set
            {
                this.roleDetails = value;
            }
        }
    }
}
