using System;
using System.Collections;
using Neusoft.FrameWork.Models;

namespace Neusoft.FrameWork.Public
{/// <summary>
    /// ObjectHelper<br></br>
    /// [��������: ObjectHelper��������ֿ��Եõ�ID,����ID���Եõ�NAME]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
	public class ObjectHelper
    {
         
        /// <summary>
		/// ���캯��
		/// </summary>
		public ObjectHelper()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// ���ع��캯������al����ֵ
		/// </summary>
		/// <param name="arrayObject"></param>
		public ObjectHelper(ArrayList arrayObject) 
        {
            if (arrayObject != null)
            {
             
                al = arrayObject;
            }
        }

        #region ����

        //���洫��Ķ����б�
        private ArrayList al = new ArrayList();
        #endregion

        #region ����

        /// <summary>
        /// ������������
        /// </summary>
        public ArrayList ArrayObject
        {
            get 
            { 
                return this.al; 
            }
            set
            {
                if (value != null) this.al = value;
            }
        }
        #endregion

        #region ����

        /// <summary>
		/// ���ݴ����Name���ҵ���Ӧ��ID
		/// </summary>
		/// <param name="name">Name����</param>
		/// <returns>ID���ԡ����û���ҵ�ID���򷵻�null</returns>
		public string GetID(string name) 
        {
			for(int i=0;i<al.Count;i++)
            {
				NeuObject obj = al[i] as NeuObject;
                if (obj == null)
                {
                    return "";
                }
                if (obj.Name == name)
                {
                    return obj.ID;
                }
			}
			
            //���û���ҵ�ID���򷵻�null
			return null;
		}
		

		/// <summary>
		/// ���ݴ����ID���ҵ���Ӧ��Name
		/// </summary>
		/// <param name="ID">ID����</param>
		/// <returns>Name���ԡ����û���ҵ�ID���򷵻�null</returns>
		public string GetName(string ID) 
        {
			for(int i=0;i<al.Count;i++)
			{
				NeuObject obj = al[i] as NeuObject;
                if (obj == null)
                {
                    return "";
                }
                if (obj.ID == ID)
                {
                    return obj.Name;
                }
			}
			
            //���û���ҵ�ID���򷵻�null
			return null;
		}

        /// <summary>
		/// ���ݴ����ID���ҵ���Ӧ��Object
		/// </summary>
		/// <param name="ID">ID����</param>
		/// <returns>Object�����û���ҵ�Object���򷵻�null</returns>
		public Neusoft.FrameWork.Models.NeuObject GetObjectFromID(string ID) 
        {
			for(int i=0;i<al.Count;i++)
			{
				NeuObject obj = al[i] as NeuObject;
                if (obj == null)
                {
                    return null;
                }
                if (obj.ID == ID)
                {
                    return obj;
                }
			}

			//���û���ҵ�Object���򷵻�null
			return null;
		}

        /// <summary>
		/// ���ݴ����Name���ҵ���Ӧ��Object
		/// </summary>
		/// <param name="name">Name����</param>
		/// <returns>Object�����û���ҵ�Object���򷵻�null</returns>
		public Neusoft.FrameWork.Models.NeuObject GetObjectFromName(string name) 
		{
			for(int i=0;i<al.Count;i++)
			{
				NeuObject obj = al[i] as NeuObject;
                if (obj == null)
                {
                    return null;
                }
                if (obj.Name == name)
                {
                    return obj;
                }
			}

			//���û���ҵ�Object���򷵻�null
			return null;
        }
        #endregion

    }
}
