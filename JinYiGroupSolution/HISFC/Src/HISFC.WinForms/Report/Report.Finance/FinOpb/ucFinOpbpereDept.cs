using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.Report.Finance.FinOpb
{
    public partial class ucFinOpbpereDept : NeuDataWindow.Controls.ucQueryBaseForDataWindow 
    {
        public ucFinOpbpereDept()
        {
            InitializeComponent();
        }
        private string as_reportcodeCode = string.Empty;
        private string as_reportcodeName = string.Empty;
        protected override void  OnLoad()
        {
            this.isAcross = true;
            this.isSort = false;
            this.Init();

 	        base.OnLoad();
            //����ʱ�䷶Χ
            DateTime now = DateTime.Now;
            DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);
            this.dtpBeginTime.Value = dt;

            //�������
            Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            System.Collections.ArrayList constantList = manager.GetConstantList("FEECODESTAT");
            foreach (Neusoft.HISFC.Models.Base.Const con in constantList)
            {
                cboReportCode.Items.Add(con);
            }
            if (cboReportCode.Items.Count > 0)
            {
                cboReportCode.SelectedIndex = 0;
                as_reportcodeCode = ((Neusoft.HISFC.Models.Base.Const)cboReportCode.Items[0]).ID;
                as_reportcodeName = ((Neusoft.HISFC.Models.Base.Const)cboReportCode.Items[0]).Name;
            }

        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        protected override int OnRetrieve(params object[] objects)
        {
            //InitializeComponent();
            //dwMain.Size = plRightTop.Size;
            //OnLoad();
            
            if (base.GetQueryTime() == -1)
            {
                return -1;
            }
            return base.OnRetrieve(this.dtpBeginTime.Value, this.dtpEndTime.Value, as_reportcodeCode);

        }

        private void cboReportCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboReportCode.SelectedIndex >= 0)
            {
                as_reportcodeCode = ((Neusoft.HISFC.Models.Base.Const)cboReportCode.Items[this.cboReportCode.SelectedIndex]).ID;
                as_reportcodeName = ((Neusoft.HISFC.Models.Base.Const)cboReportCode.Items[this.cboReportCode.SelectedIndex]).Name;
            }
        }

      
    }
}