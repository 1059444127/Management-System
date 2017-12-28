using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Operation;

namespace Neusoft.WinForms.Report.Operation
{
    /// <summary>
    /// [��������: ��������֪ͨ����ӡ�ؼ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2007-01-04]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucArrangeNotifyPrint : UserControl, Neusoft.HISFC.BizProcess.Interface.Operation.IArrangeNotifyFormPrint
    {
        /// <summary>
        /// [��������: �������Ŵ�ӡ]<br></br>
        /// [�� �� ��: ����ȫ]<br></br>
        /// [����ʱ��: 2007-01-04]<br></br>
        /// <�޸ļ�¼
        ///		�޸���=''
        ///		�޸�ʱ��='yyyy-mm-dd'
        ///		�޸�Ŀ��=''
        ///		�޸�����=''
        ///  />
        /// </summary>
        public ucArrangeNotifyPrint()
        {
            InitializeComponent();
            if(!Environment.DesignMode)
            {
                this.Init();
            }
        }

#region �ֶ�

        Neusoft.HISFC.BizLogic.Manager.Constant constManager = new Neusoft.HISFC.BizLogic.Manager.Constant();
        Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();

#endregion

#region ����

#endregion

#region ����

        private void Init()
        {
            print.ControlBorder = Neusoft.FrameWork.WinForms.Classes.enuControlBorder.None;
        }


#endregion

        #region IApplicationFormPrint ��Ա

        /// <summary>
        /// �������뵥����
        /// </summary>
        public Neusoft.HISFC.Models.Operation.OperationAppllication OperationApplicationForm
        {
            set 
            {
                Neusoft.HISFC.Models.Operation.OperationAppllication thisOpsApp = value;
                if (thisOpsApp == null) 
                    return;
                this.a0.Text = "�����ң�";
                this.a1.Text = string.Empty;
                this.a2.Text = string.Empty;
                this.a3.Text = string.Empty;
                this.a4.Text = string.Empty;
                this.a5.Text = string.Empty;

                this.a6.Text = string.Empty;
                this.a7.Text = string.Empty;
                this.a8.Text = string.Empty;
                this.a9.Text = string.Empty;
                this.a10.Text = string.Empty;

                this.a11.Text = string.Empty;
                this.a12.Text = string.Empty;
                this.a13.Text = string.Empty;
                this.a14.Text = string.Empty;
                this.a15.Text = string.Empty;

                this.a16.Text = string.Empty;
                this.a17.Text = string.Empty;
                this.a18.Text = string.Empty;
                this.a19.Text = string.Empty;
                this.a20.Text = string.Empty;
                
                
                this.b1.Text = string.Empty;
                this.b2.Text = string.Empty;
                this.b3.Text = string.Empty;
                this.b4.Text = string.Empty;
                this.b5.Text = string.Empty;

                this.b6.Text = string.Empty;
                this.b7.Text = string.Empty;
                this.b8.Text = string.Empty;
                this.b9.Text = string.Empty;
                this.b10.Text = string.Empty;

                this.b11.Text = string.Empty;
                this.b12.Text = string.Empty;
                this.b13.Text = string.Empty;
                this.b14.Text = string.Empty;
                this.b15.Text = string.Empty;

                this.b16.Text = string.Empty;
                this.b17.Text = string.Empty;

                this.a0.Text = string.Format("�����ң�{0}",thisOpsApp.ExeDept.Name);

                //��������
                this.a1.Text = thisOpsApp.PreDate.ToString();
                this.b13.Text = thisOpsApp.PreDate.ToString("yyyy-MM-dd");					
                this.b15.Text = thisOpsApp.PreDate.ToString("HH:mm");

                //����
                this.a2.Text = thisOpsApp.PatientInfo.PVisit.PatientLocation.Dept.Name;
                this.b1.Text = thisOpsApp.PatientInfo.PVisit.PatientLocation.Dept.Name;

                //����
                this.a3.Text = thisOpsApp.PatientInfo.Name;
                this.b2.Text = thisOpsApp.PatientInfo.Name;		

                //����				
                //int li_thisYear = this.constManager.GetDateTimeFromSysDateTime().Year;//��ǰ��
                //int li_BirYear = thisOpsApp.PatientInfo.Birthday.Year;//������
                //int li_age = li_thisYear - li_BirYear;
                //if (li_age == 0) li_age = 1;
                //this.a4.Text = li_age.ToString();
                //this.b3.Text = li_age.ToString();

                this.a4.Text = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(thisOpsApp.PatientInfo.Birthday);
                this.b3.Text = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(thisOpsApp.PatientInfo.Birthday);

                //�Ա�
                this.a5.Text = thisOpsApp.PatientInfo.Sex.Name;
                this.b4.Text = thisOpsApp.PatientInfo.Sex.Name;

                //����
                this.a6.Text = thisOpsApp.OpsTable.Name;
                this.b5.Text = thisOpsApp.OpsTable.Name;

                //����
                this.a7.Text = thisOpsApp.PatientInfo.PVisit.PatientLocation.Bed.Name;
                this.b6.Text = thisOpsApp.PatientInfo.PVisit.PatientLocation.Bed.Name;

                //סԺ��
                this.a8.Text = thisOpsApp.PatientInfo.PID.PatientNO;
                this.b7.Text = thisOpsApp.PatientInfo.PID.PatientNO;
                
                #region ���
                //this.lbDiagnose.Text = thisOpsApp.DiagnoseAl[0].ToString();

                StringBuilder diagnose = new StringBuilder();

                for (int i = 0; i < thisOpsApp.DiagnoseAl.Count; i++)
                {

                    diagnose.Append(thisOpsApp.DiagnoseAl[i].ToString()+"��");
                }

                this.a9.Text = diagnose.ToString();
                this.b8.Text = diagnose.ToString();
                
                #endregion


                this.b9.Text = thisOpsApp.AneNote;

                #region ������Ŀ

                //this.lbItemName.Text = thisOpsApp.MainOperationName;//������Ŀ

                StringBuilder opitem = new StringBuilder();
                foreach (Neusoft.HISFC.Models.Operation.OperationInfo myOpsInfo in thisOpsApp.OperationInfos)
                {
                    opitem.Append(myOpsInfo.OperationItem.Name+"��");
                }

                this.a11.Text = opitem.ToString();
                this.b10.Text = opitem.ToString();

                #endregion
                
                
                //����ҽʦ
                this.a10.Text = thisOpsApp.OperationDoctor.Name;
                this.b14.Text = thisOpsApp.OperationDoctor.Name;		

                //��������
                this.a14.Text = thisOpsApp.SpecialItem;

                //����ʽ
                NeuObject obj = new NeuObject();
                if ( !string.IsNullOrEmpty( thisOpsApp.AnesWay))//.ID != null && thisOpsApp.AnesType.ID != "")
                {
                    obj = this.constManager.GetConstant(Neusoft.HISFC.Models.Base.EnumConstant.ANESWAY, thisOpsApp.AnesWay);
                    if (obj != null)
                    {
                        this.a16.Text = obj.Name;
                        this.b12.Text = obj.Name;
                    }
                }

                					

                for (int i = 1; i < thisOpsApp.RoleAl.Count; i++)
                {
                    obj = (NeuObject)(thisOpsApp.RoleAl[i]);
                    switch (i)
                    {
                        case 1:
                            this.a12.Text = obj.Name;											//һ����
                            break;
                        case 2:
                            this.a13.Text = obj.Name;											//������
                            break;
                        case 3:
                            this.a15.Text = obj.Name;											//������
                            break;
                    }
                }
                
                //this.lbOpsNote.Text = thisOpsApp.OpsNote;								//����ע������
                
                //����ҽʦ
                this.a18.Text = thisOpsApp.ApplyDoctor.Name;
                this.b16.Text = thisOpsApp.ApplyDoctor.Name;
	
                //����ʱ��
                this.a19.Text = thisOpsApp.ApplyDate.ToString();
                this.b17.Text = thisOpsApp.ApplyDate.ToString();


                //�����þ�
                this.a20.Text = thisOpsApp.ApplyNote.Trim();
                this.b18.Text = thisOpsApp.ApplyNote.Trim();

                //////�Ƿ���Ҫ��е��ʿ
                //if (thisOpsApp.IsAccoNurse == true)
                //{
                //    this.lbIsAccoNurse.Text = "���ǡ���";
                //}
                //else
                //{
                //    this.lbIsAccoNurse.Text = "���ǡ���";
                //}

                ////�Ƿ���ҪѲ�ػ�ʿ
                //if (thisOpsApp.IsPrepNurse == true)
                //{
                //    this.lbIsPrepNurse.Text = "���ǡ���";
                //}
                //else
                //{
                //    this.lbIsPrepNurse.Text = "���ǡ���";
                //}


               
			
            }
        }


        #endregion

        #region IReportPrinter ��Ա

        public int Export()
        {
            return 0;
        }

        public int Print()
        {
            #region MyRegion֣�����--{CF9C3E37-1B84-48c6-BBE8-DAEF847B7BAC}
            this.print.PrintPage(60, 40, this); //this.print.PrintPage(0, 0, this); 
            #endregion
            return 0;
        }

        public int PrintPreview()
        {
            this.print.PrintPreview(this);
            return 0;
        }

        #endregion

        #region IArrangeFormPrint ��Ա

        private bool isPrintExTable;
        /// <summary>
        /// �Ƿ��ӡ������̨�����
        /// </summary>
        public bool IsPrintExtendTable
        {
            set
            {
                this.isPrintExTable = value;
            }
        }

        #endregion


    }
}
