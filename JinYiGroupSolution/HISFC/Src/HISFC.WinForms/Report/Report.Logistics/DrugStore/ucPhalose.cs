using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;
using Neusoft.FrameWork.Function;
using System.Collections;

namespace Neusoft.Report.Logistics.DrugStore
{
    public partial class ucPhalose : NeuDataWindow.Controls.ucQueryBaseForDataWindow
    {
        public ucPhalose()
        {
            InitializeComponent();

        }

           /// <summary>
        /// ��¼��Ա��Ϣ
        /// </summary>
        protected Neusoft.HISFC.Models.Base.Employee employee = null;

         
        
        
        protected override int OnRetrieve(params object[] objects)
        {
            this.employee = (Neusoft.HISFC.Models.Base.Employee)this.dataBaseManager.Operator;
            if (base.GetQueryTime() == -1)
            {
                return -1;
            }
       
            return base.OnRetrieve(base.beginTime, base.endTime, employee.Dept.ID);
        }
       
    }
}

