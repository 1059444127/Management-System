using System;
using System.Collections;
using System.Collections.Generic;
namespace Neusoft.FrameWork.Models
{
    /// <summary>
    /// NeuObject <br></br>
    /// [��������: NeuObject�����������ж��󶼼̳����]<br></br>
    /// [�� �� ��: ���Ʒ�]<br></br>
    /// [����ʱ��: 2006-08-28]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    [Serializable]
    public class NeuObject : IDisposable//:System.ICloneable
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		public NeuObject()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			strID="";
			strName="";
			strMemo="";
		}

        public NeuObject(string id, string name, string memo)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            strID = id;
            strName = name;
            strMemo = memo;
        }

		//finalizer
        //~NeuObject()
        //{
        //    Dispose( false );
        //}

        #region ����
        /// <summary>
		/// �ֲ�����
		/// </summary>
		private string strID;
		private string strName;
		private string strMemo;
		
        [Obsolete("���ϣ�5.0����ʹ��",false)]
		public string User01 = string.Empty;
        [Obsolete("���ϣ�5.0����ʹ��", false)]
        public string User02 = string.Empty;
        [Obsolete("���ϣ�5.0����ʹ��", false)]
        public string User03 = string.Empty;

		private bool alreadyDisposed = false;
        #endregion

        #region ����
        /// <summary>
		/// ID
		/// </summary>
		public virtual string ID
		{
			get
			{
				return strID + "";
			}
			set
			{
				strID=value;
			}
		}
		/// <summary>
		/// ����Name
		/// </summary>
		/// <returns>Name</returns>
		public virtual string Name
		{
			get
			{
				return strName + "";
			}
			set
			{
				strName=value;
			}
		}
		/// <summary>
		/// ����Memo
		/// </summary>
		/// <returns>Memo</returns>
		public virtual string Memo
		{	
			get
			{
				return strMemo + "";
			}
			set
			{
				strMemo=value;
			}
        }

        #endregion

        #region LIST����
        //private Dictionary<string, object> propertyCollection;

        ///// <summary>
        ///// ������-����ֵ������
        ///// </summary>
        //public Dictionary<string, object> PropertyCollection
        //{
        //    get
        //    {
        //        if (propertyCollection == null)
        //        {
        //            propertyCollection = new Dictionary<string, object>();
        //        }
        //        return propertyCollection;
        //    }

        //    set
        //    {
        //        propertyCollection = value;
        //    }
        //}

        ///// <summary>
        ///// ��ȡ���Ե�ֵ
        ///// </summary>
        ///// <param name="property"></param>
        ///// <returns></returns>
        //public object GetProperty(string property)
        //{
        //    object propertyValue = null;
        //    if (PropertyCollection.ContainsKey(property))
        //        propertyValue = PropertyCollection[property];
        //    return propertyValue;
        //}

        ///// <summary>
        ///// ��ӡ�����-����ֵ��
        ///// </summary>
        ///// <param name="property">����</param>
        ///// <param name="value">����ֵ</param>
        ///// <param name="isOverride">����Ѵ��ڸ����ԣ��Ƿ񸲸Ǹ����ԡ�trueΪ����;falseΪ�����ǣ����ǻ��׳��쳣</param>
        //public void SetProperty(string property, object value, bool isOverride)
        //{
        //    if (PropertyCollection.ContainsKey(property))
        //    {
        //        if (isOverride)
        //        {
        //            PropertyCollection.Remove(property);
        //        }
        //    }

        //    PropertyCollection.Add(property, value);
        //}

        ///// <summary>
        ///// ��ӡ�����-����ֵ�������Ѿ����ڵ����ԣ�Ĭ�ϸ��Ǹ�����ֵ
        ///// value:�����㣬��Ҫ�ڷ�����Լ�ӿ��Ͼ�̬���ServiceKnowType(typeof(value-type))����
        ///// </summary>
        ///// <param name="property">����</param>
        ///// <param name="value">����ֵ</param>
        //public void SetProperty(string property, object value)
        //{
        //    SetProperty(property, value, true);
        //}

        ///// <summary>
        ///// �������Ƿ����
        ///// </summary>
        ///// <param name="property">����</param>
        ///// <returns>����Ϊtrue������false</returns>
        //public bool IsContainsProperty(string property)
        //{
        //    return PropertyCollection.ContainsKey(property);
        //}
        #endregion
        #region ����
        /// <summary>
		/// ����ToString
		/// </summary>
		/// <returns>Name</returns>
		public override string ToString()
		{
			return this.Name;
        }
        #endregion


        #region ICloneable ��Ա

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public NeuObject Clone()
		{
			return  this.MemberwiseClone() as FrameWork.Models.NeuObject;
		}
		#endregion

        protected virtual void Dispose(bool isDisposing)
        {
            if (alreadyDisposed)
                return;
            if (isDisposing)
            {
                // TODO: free managed resources here
            }
            //TODO: free unmanaged resources here
            alreadyDisposed = true;
        }


        #region IDisposable ��Ա

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(true);
        }

        #endregion
	}
}
