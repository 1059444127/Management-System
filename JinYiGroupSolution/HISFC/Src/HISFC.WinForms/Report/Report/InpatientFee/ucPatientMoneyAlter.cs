using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.WinForms.Report.InpatientFee
{
    /// <summary>
    /// �������������Ƿ�ѱ����ĵ��ݴ�ӡʵ��
    /// �˽ӿڷ��䲻��ͨ�������ݶ�ȡ��
    /// ��HISFC.Components�������д����
    /// </summary>
    public partial class ucPatientMoneyAlter : UserControl,Neusoft.HISFC.BizProcess.Interface.FeeInterface.IMoneyAlert
    {
        /// <summary>
        /// ����
        /// </summary>
        public ucPatientMoneyAlter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ��ǰ������Ϣ
        /// </summary>
        protected Neusoft.HISFC.Models.RADT.PatientInfo curPatientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();

        private Neusoft.HISFC.BizLogic.Manager.Constant constantMgr = new Neusoft.HISFC.BizLogic.Manager.Constant();

        #region IMoneyAlert ��Ա

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
        {
            get
            {
                return curPatientInfo ;
            }
            set
            {
                this.curPatientInfo = value;
            }
        }

        /// <summary>
        /// ���ý�����Ϣ
        /// </summary>
        public void SetPatientInfo()
        {
            this.lbl����.Text = this.PatientInfo.Name;
            this.lbl����.Text = this.PatientInfo.PVisit.PatientLocation.Bed.ID.Substring(4);
            this.lblסԺ��.Text = this.PatientInfo.PID.PatientNO;
            this.lbl����.Text = this.PatientInfo.PVisit.PatientLocation.Dept.Name;

            this.lblԤ�����ܶ�.Text = this.PatientInfo.FT.PrepayCost.ToString();
            decimal TotCost = this.PatientInfo.FT.TotCost + this.PatientInfo.FT.BalancedCost;
            this.lbl�ѻ����ܽ��.Text = TotCost.ToString();
            
            this.nlb��ӡʱ��.Text = this.constantMgr.GetSysDate().ToString();

            if (this.PatientInfo.PVisit.AdmittingDoctor.User02 == "2")
            {
                if (string.IsNullOrEmpty(this.PatientInfo.PVisit.AdmittingDoctor.User01))
                {
                    this.lbl�������.Text = "__________";
                }
                else
                {
                    this.lbl�������.Text = this.PatientInfo.PVisit.AdmittingDoctor.User01;
                }
            }
            else if (this.PatientInfo.PVisit.AdmittingDoctor.User02 == "1")
            {
                ucInputPrepayNum uc = new ucInputPrepayNum();
                Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = this.PatientInfo.Name;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);

                string inputValue = uc.InputValue;

                if (Neusoft.FrameWork.Function.NConvert.ToDecimal(inputValue) > 0)
                {
                    this.lbl�������.Text = inputValue;
                }
                else
                {
                    this.lbl�������.Text = "__________";
                }
            }
            else
            {
                this.lbl�������.Text = "__________";
            }
            
            Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();
            //Neusoft.HISFC.Models.Base.PageSize page = new Neusoft.HISFC.Models.Base.PageSize();
            //page.Height = 342;
            //page.Width = 342;
            //page.Name = "PhaInput";
            //p.SetPageSize(page);

            #region ֣�����--{0E4B7A1C-6F83-44eb-90B3-A57637C27D3A}

            try
            {
                Neusoft.HISFC.Models.Base.PageSize page = new Neusoft.HISFC.Models.Base.PageSize();
                Neusoft.HISFC.BizLogic.Manager.PageSize psMgr = new Neusoft.HISFC.BizLogic.Manager.PageSize();
                page = psMgr.GetPageSize("165");
                p.SetPageSize(page);
            }
            catch 
            {
                MessageBox.Show("��ӡ������ȡʧ��!");
                return ;
            }

            #endregion


            p.PrintPage(0, 0, this);
        }

        #endregion

    }
}
