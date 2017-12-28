using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Neusoft.HISFC.Components.Manager
{
    /// <summary>
    /// [��������: �������Ա��ʵ����]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007��04]<br></br>
    /// 
    /// <˵��>
    ///     1 �������Ա��ʵ����
    /// </˵��>
    /// </summary>
    public partial class ucAddShiftType : UserControl
    {
        public ucAddShiftType()
        {
            InitializeComponent();
        }

        #region �����

        /// <summary>
        /// ���
        /// </summary>
        private DialogResult result = DialogResult.Cancel;

        private System.Collections.Hashtable hsExtisClass = new System.Collections.Hashtable();

        #endregion

        #region ����

        /// <summary>
        /// ��������
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject ReflectedClass
        {
            get
            {
                Neusoft.FrameWork.Models.NeuObject refClass = new Neusoft.FrameWork.Models.NeuObject();

                refClass.ID = this.txtTypeStr.Text;
                refClass.Name = this.lbDescrip.Text;

                return refClass;
            }
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        public List<Neusoft.FrameWork.Models.NeuObject> Properties
        {
            get
            {
                List<Neusoft.FrameWork.Models.NeuObject> alPropertyList = new List<Neusoft.FrameWork.Models.NeuObject>();
                for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                {
                    alPropertyList.Add(this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.FrameWork.Models.NeuObject);
                }

                return alPropertyList;
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        public DialogResult Result
        {
            get
            {
                return this.result;
            }
            set
            {
                this.result = value;
            }
        }

        /// <summary>
        /// �Ѵ��ڵ�����Ϣ
        /// </summary>
        public System.Collections.Hashtable HsExitsClass
        {
            set
            {
                this.hsExtisClass = value;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ���
        /// </summary>
        private void Clear()
        {
            this.lbDescrip.Text = "";
            this.neuSpread1_Sheet1.Rows.Count = 0;
        }

        /// <summary>
        /// �������ڻ�ȡ��������
        /// </summary>
        /// <param name="t">��������</param>
        /// <returns>�ɹ����������ñ������Ϣ</returns>
        private List<Neusoft.FrameWork.Models.NeuObject> GetProperty(Type t)
        {
            //��ȡ���DisplayName���� ��������������
            object[] display = t.GetCustomAttributes(false);
            if (display != null)
            {
                foreach (object oDisplay in display)
                {
                    if (oDisplay is System.ComponentModel.DisplayNameAttribute)
                    {
                        System.ComponentModel.DisplayNameAttribute displayAttribute = oDisplay as System.ComponentModel.DisplayNameAttribute;

                        this.lbDescrip.Text = displayAttribute.DisplayName;

                        break;
                    }
                }
            }
            //��ȡ���������е�����Property
            PropertyInfo[] propertyCollection = t.GetProperties();

            List<Neusoft.FrameWork.Models.NeuObject> recordList = new List<Neusoft.FrameWork.Models.NeuObject>();
            //�����������Խ���ѭ���ж�
            foreach (PropertyInfo p in propertyCollection)
            {  
                //��ֻ����ֻд���Բ����д���
                if (p.CanRead && p.CanWrite)
                {
                    string propertyID = "";
                    string propertyName = "";
                    string propertyDescrip = "";
                    //��ȡ��ÿ���������õ����� (Property��Attribute)
                    foreach (Attribute a in p.GetCustomAttributes(true))
                    {
                        //��������������ʾ
                        if (a is System.ComponentModel.DisplayNameAttribute)
                        {
                            System.ComponentModel.DisplayNameAttribute displayName = a as System.ComponentModel.DisplayNameAttribute;

                            propertyName = displayName.DisplayName;
                        }
                        //��������
                        if (a is System.ComponentModel.DescriptionAttribute)
                        {
                            System.ComponentModel.DescriptionAttribute descrip = a as System.ComponentModel.DescriptionAttribute;

                            propertyDescrip = descrip.Description;
                        }                        
                    }                   
                    //�� ������Ч���� ���б��� ID Name����Ϊ��
                    if (propertyName != "")
                    {
                        Neusoft.FrameWork.Models.NeuObject shiftProperty = new Neusoft.FrameWork.Models.NeuObject();
                        shiftProperty.ID = p.Name;
                        shiftProperty.Name = propertyName;
                        shiftProperty.Memo = propertyDescrip;

                        recordList.Add(shiftProperty);
                    }
                }
            }

            return recordList;
        }

        /// <summary>
        /// �ر�
        /// </summary>
        protected void Close()
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.Close();
            }
        }

        /// <summary>
        /// ��ȡ�����õ���������
        /// </summary>
        protected void GetShiftProperty()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Neusoft.FrameWork.Management.Language.Msg("����������Ϣ ���Ժ�..."));
            Application.DoEvents();

            try
            {
                System.Runtime.Remoting.ObjectHandle oHandle = System.Activator.CreateInstance("HISFC.Object", this.txtTypeStr.Text);
                if (oHandle == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("δ����������Ϣ��ȡ���� �����Ƿ�������ȷ"));
                    return;
                }
                Type t = oHandle.Unwrap().GetType();
                if (t == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("δ����������Ϣ��ȡ���� �����Ƿ�������ȷ"));
                    return;
                }

                if (this.hsExtisClass.ContainsKey(t.ToString()))
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ʵ����Ϣ�Ѵ��� ��ɾ��ԭ��Ϣ���������"));
                    this.txtTypeStr.SelectAll();
                    return;
                }

                List<Neusoft.FrameWork.Models.NeuObject> alProperty = this.GetProperty(t);
                if (alProperty != null)
                {
                    if (alProperty.Count == 0)
                    {
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��ʵ����������������������Ϣ"));
                        return;
                    }

                    this.neuSpread1_Sheet1.Rows.Count = 0;
                    foreach (Neusoft.FrameWork.Models.NeuObject info in alProperty)
                    {
                        this.neuSpread1_Sheet1.Rows.Add(0, 1);
                        this.neuSpread1_Sheet1.Cells[0, 0].Text = info.Name;        //����������������
                        this.neuSpread1_Sheet1.Cells[0, 1].Text = info.Memo;        //��������

                        this.neuSpread1_Sheet1.Rows[0].Tag = info;
                    }
                }
            }
            catch
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("δ����������Ϣ��ȡ���� �����Ƿ�������ȷ"));
                return;
            }

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        #endregion

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.Clear();

            this.GetShiftProperty();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtTypeStr.Text != "")
            {
                this.result = DialogResult.OK;
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.Cancel;

            this.Close();
        }
    }
}
