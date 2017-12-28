using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.Operation;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: ���������еĻ���������Ϣ]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-04]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucArrangementInfo : UserControl
    {
        public ucArrangementInfo()
        {
            InitializeComponent();
        }

#region ����

        public OperationAppllication OperationApplication
        {
            set
            {
                Neusoft.HISFC.Models.Operation.OperationAppllication apply = value;

                if (apply == null)
                {
                    this.ClearSpread();
                    return;
                }
                //��ӵ���ϸ��ʾ
                //����/����
                this.PatientLocation = string.Concat(apply.PatientInfo.PVisit.PatientLocation.Name, "[",
                    apply.PatientInfo.PVisit.PatientLocation.Bed.ID, "]");
                
                //�����������Ա�����                
                
                int age = Environment.OperationManager.GetDateTimeFromSysDateTime().Year - apply.PatientInfo.Birthday.Year;
                if (age == 0) 
                    age = 1;
                fpSpread1_Sheet1.SetValue(0, 3, apply.PatientInfo.Name + " [" + apply.PatientInfo.Sex.Name +
                                            ", " + age.ToString() + "��]", false);
                //סԺ��
                fpSpread1_Sheet1.SetValue(1, 1, apply.PatientInfo.PID.PatientNO, false);
                //��������
                switch (apply.OperateKind)
                {
                    case "1":
                        fpSpread1_Sheet1.SetValue(1, 3, "����", false);
                        break;
                    case "2":
                        fpSpread1_Sheet1.SetValue(1, 3, "����", false);
                        break;
                    case "3":
                        fpSpread1_Sheet1.SetValue(1, 3, "��Ⱦ", false);
                        break;
                }
                //����̨����
                switch (apply.TableType)
                {
                    case "1":
                        fpSpread1_Sheet1.SetValue(2, 1, "��̨", false);
                        break;
                    case "2":
                        fpSpread1_Sheet1.SetValue(2, 1, "��̨", false);
                        break;
                    case "3":
                        fpSpread1_Sheet1.SetValue(2, 1, "��̨", false);
                        break;
                }
                //
                ////TODO: ��ǰ���
                if (apply.DiagnoseAl != null && apply.DiagnoseAl.Count > 0)
                    fpSpread1_Sheet1.SetValue(2, 3, (apply.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10.Name, false);
                else
                    fpSpread1_Sheet1.SetValue(2, 3, "", false);
                ////��������
                fpSpread1_Sheet1.SetValue(3, 1, apply.MainOperationName);
                //����ʱ��
                fpSpread1_Sheet1.SetValue(3, 3, apply.PreDate.ToString(), false);
                //����ʽ
                //fpSpread1_Sheet1.SetValue(4, 1, apply.AnesType.Name, false);
                if (apply.AnesType.ID != null && apply.AnesType.ID != "")
                {
                    Neusoft.FrameWork.Models.NeuObject obj = Environment.GetAnes(apply.AnesType.ID.ToString());

                    if (obj != null)
                    {
                        fpSpread1_Sheet1.SetValue(4, 1, obj.Name, false);
                       
                    }
                }
                //����ҽ��
                fpSpread1_Sheet1.SetValue(4, 3, apply.ApplyDoctor.Name, false);
                //����ҽ��
                if (apply.OperationDoctor != null)
                    fpSpread1_Sheet1.SetValue(5, 1, apply.OperationDoctor.Name, false);
                else
                    fpSpread1_Sheet1.SetValue(5, 1, "", false);
                //�Ƿ���������

                string txt = "";
                if (apply.BloodNum == 0)
                { 
                    txt = "��"; 
                }
                else if (apply.BloodNum == 1)
                { 
                    txt = "HAV"; 
                }
                else if (apply.BloodNum == 2)
                { 
                    txt = "HBV"; 
                }
                else if (apply.BloodNum == 3)
                { 
                    txt = "HCV"; 
                }
                else if (apply.BloodNum == 4)
                { 
                    txt = "HDV"; 
                }
                else if (apply.BloodNum == 5)
                { 
                    txt = "HIV"; 
                }
                else if (apply.BloodNum == 6)
                { 
                    txt = "����"; 
                }
                fpSpread1_Sheet1.SetValue(5, 3, txt, false);

                //����
                if (apply.HelperAl != null && apply.HelperAl.Count != 0)
                    fpSpread1_Sheet1.SetValue(6, 1, (apply.HelperAl[0] as Neusoft.HISFC.Models.Base.Employee).Name, false);
                else
                    fpSpread1_Sheet1.SetValue(6, 1, "", false);
                //��ע
                fpSpread1_Sheet1.SetValue(6, 3, apply.ApplyNote, false);

                fpSpread1_Sheet1.Tag = apply;
            }
        }

        //·־����ʱ�䣺��������-��-����
        //Ŀ�ģ����������뵥ʵ��OperationAppllicationΪnullʱ���fpSpread1_Sheet1������
        private void ClearSpread()
        {
            try
            {
                for (int i = 0; i < this.fpSpread1_Sheet1.Rows.Count; i++)
                {
                    this.fpSpread1_Sheet1.Cells[i, 1].Text = "";
                    this.fpSpread1_Sheet1.Cells[i, 3].Text = "";
                }
            }
            catch { }
        }

        /// <summary>
        /// ����/����
        /// </summary>
        public string PatientLocation
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[0, 1].Text = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string PatientName
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[0, 3].Text = value;
            }
        }

        /// <summary>
        /// ����סԺ��
        /// </summary>
        public string PatientID
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[1, 1].Text = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string OperationType
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[1, 3].Text = value;
            }
        }


        /// <summary>
        /// ����̨����
        /// </summary>
        public string TableType
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[2, 1].Text = value;
            }
        }
        
        /// <summary>
        /// ��ǰ���
        /// </summary>
        public string Diagnosis
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[2, 3].Text = value;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string OperationName
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[3, 1].Text = value;
            }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public string OperationTime
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[3, 3].Text = value;
            }
        }

        /// <summary>
        /// ����ʽ
        /// </summary>
        public string AnseType
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[4, 1].Text = value;
            }
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        public string ApplyDoctorName
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[4, 3].Text = value;
            }
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        public string OperatorName
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[5, 1].Text = value;
            }
        }

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsSpecialOperation
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[5, 3].Value = value;
            }
        }

        /// <summary>
        /// ����ҽ��
        /// </summary>
        public string HelperDoctorName
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[6, 1].Text = value;
            }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Memo
        {
            set
            {
                this.fpSpread1_Sheet1.Cells[6, 3].Text = value;
            }
        }
#endregion
    }
}
