using System;
using System.Collections.Generic;
using System.Text;
using Neusoft.HISFC.Models.Base;

//{DF8058FF-72C0-404f-8F36-6B4057B6F6CD}
namespace Neusoft.HISFC.Components.Order.Classes
{
    
    #region ������

    /// <summary>
    /// ������
    /// </summary>
    internal class Clipboard
    {
        #region ����

        List<object> instanceCollection = new List<object>();

        /// <summary>
        /// ������
        /// </summary>
        IClipboard clipboard = null;

        #endregion

        #region ����

        /// <summary>
        /// Ĭ�ϲ���xml������
        /// </summary>
        public Clipboard()
        {
            clipboard = new BinaryClipboard();
        }

        /// <summary>
        /// ��������Ϊtype�ļ�����
        /// </summary>
        /// <param name="type"></param>
        public Clipboard(EnumClipboard type)
        {
            ClipboardEnumService service = new ClipboardEnumService();
            string classFullName = service.GetName(type);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            clipboard = assembly.CreateInstance(classFullName) as IClipboard;
            //����һ��������
            if (clipboard == null)
            {
                clipboard = new XmlClipboard();
            }
        }

        #endregion

        #region ��������

        /// <summary>
        /// ����������в��뵱ǰʵ��,����ֱ��ִ��copy
        /// Ҫcopy��ʹ��Copy();
        /// </summary>
        /// <param name="instance"></param>
        public void Add(object instance)
        {
            instanceCollection.Add(instance);
        }

        /// <summary>
        /// ��ռ�����
        /// </summary>
        public void Clear()
        {
            instanceCollection.Clear();
        }

        /// <summary>
        /// ����
        /// </summary>
        public bool Copy()
        {
            bool result = clipboard.Copy(instanceCollection);
            instanceCollection.Clear();
            return result; ;
        }

        /// <summary>
        /// ճ��
        /// ճ�����ݱ���װ��List-object�����б���
        /// </summary>
        /// <returns>����ճ������</returns>
        public object Paste()
        {
            return clipboard.Paste(instanceCollection.GetType());
        }

        #endregion
    }

    #endregion

    #region ö�ٺ�ö�ٷ�����

    /// <summary>
    /// ��������ö��
    /// </summary>
    enum EnumClipboard
    {
        Xml,
        Binary
    }

    /// <summary>
    /// ����ö�ٷ�����
    /// </summary>
    class ClipboardEnumService : EnumServiceBase
    {
        static System.Collections.Hashtable hashtable = new System.Collections.Hashtable();

        static ClipboardEnumService()
        {
            hashtable[EnumClipboard.Xml] = "Neusoft.HISFC.Components.Order.Classes.XmlClipboard";
            hashtable[EnumClipboard.Binary] = "Neusoft.HISFC.Components.Order.Classes.BinaryClipboard";
        }

        EnumClipboard myEnum;

        protected override Enum EnumItem
        {
            get
            {
                return myEnum;
            }
        }

        protected override System.Collections.Hashtable Items
        {
            get
            {
                return hashtable;
            }
        }
    }

    #endregion

    #region ������

    /// <summary>
    /// �����ӿڣ�һ��ֻ����һ��ʵ��
    /// </summary>
    interface IClipboard
    {
        object Paste(Type type);
        bool Copy(object parameter);
    }

    /// <summary>
    /// xml������
    /// </summary>
    class XmlClipboard : IClipboard
    {
        const string file = "clipboard.xml";

        public object Paste(Type type)
        {
            System.IO.Stream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
                object instance = serializer.Deserialize(stream);
                stream.Flush();
                stream.Close();
                return instance;
            }
            catch
            {
                return null;
            }
            finally
            {
                stream.Close();
            }
        }

        public bool Copy(object parameter)
        {
            System.IO.Stream stream = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(parameter.GetType());
                serializer.Serialize(stream, parameter);
                stream.Flush();
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                stream.Close();
            }
        }
    }

    /// <summary>
    /// �����Ƽ�����
    /// </summary>
    class BinaryClipboard : IClipboard
    {
        const string file = "clipboard.dat";

        public object Paste(Type type)
        {
            if (System.IO.File.Exists( file ) == false)
            {
                return null;
            }
            System.IO.Stream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                object instance = formater.Deserialize(stream);
                stream.Flush();
                stream.Close();
                return instance;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                stream.Close();
            }
        }

        public bool Copy(object parameter)
        {
            System.IO.Stream stream = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formater.Serialize(stream, parameter);
                stream.Flush();
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                stream.Close();
            }
        }
    }

    #endregion

    #region ��ʷҽ����ѯճ����

    /// <summary>
    /// ��ʷҽ����ѯճ����
    /// </summary>
    public class HistoryOrderClipboard
    {
        static List<string> data = new List<string>();
        static bool isReaded = false;

        private static void GetContent()
        {
            data.Clear();
            List<object> objdata = clipboard.Paste() as List<object>;
            if ((objdata != null) && (objdata.Count > 0))
            {
                for (int count = 0; count < objdata.Count; count++)
                {
                    data.Add(objdata[count].ToString());
                }
            }
            isReaded = true;
        }

        /// <summary>
        /// �Ӽ�������ҽ��ID�б�
        /// </summary>
        public static List<string> OrderList
        {
            get
            {
                if (!isReaded)
                {
                    GetContent();
                }
                if (data.Count <= 0)
                {
                    return null;
                }
                string[] array = new string[data.Count - 1];
                data.CopyTo(0, array, 0, array.Length);
                data.CopyTo(0, array, 0, array.Length);
                List<string> list = new List<string>(array);
                type = (ServiceTypes)Convert.ToInt32(data[data.Count - 1]);
                data.Clear();
                return list;
            }
        }

        private static Neusoft.HISFC.Models.Base.ServiceTypes type = ServiceTypes.I;

        /// <summary>
        /// ��ʶ���ﻹ��סԺ
        /// </summary>
        public static Neusoft.HISFC.Models.Base.ServiceTypes Type
        {
            get { return type; }
        }


        static Clipboard clipboard = new Clipboard();

        /// <summary>
        /// ִ����ӣ�������
        /// </summary>
        /// <param name="instance"></param>
        public static void Add(object instance)
        {
            clipboard.Add(instance);
        }

        /// <summary>
        /// ִ�и���
        /// </summary>
        public static void Copy()
        {
            clipboard.Copy();
            isReaded = false;
        }
    }

    #endregion
}