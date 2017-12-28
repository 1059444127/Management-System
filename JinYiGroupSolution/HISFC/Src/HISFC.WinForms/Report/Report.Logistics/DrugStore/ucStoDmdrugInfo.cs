using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Report.Logistics.DrugStore
{
    public partial class ucStoDmdrugInfo : NeuDataWindow.Controls.ucQueryBaseForDataWindow
    {
        public ucStoDmdrugInfo()
        {
            InitializeComponent();
        }

        Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
        Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        ArrayList alDept = new ArrayList();
        ArrayList alDrugQuality = new ArrayList();
        string deptId = "ALL";
        string drugQua = "ALL";

        #region ��ʼ��
        protected override void OnLoad(EventArgs e)
        {
            
           //ҩ������
            ArrayList list = new ArrayList();
            Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();

            obj.ID = "ALL";
            obj.Name = "ȫ��";
            alDept.Add(obj);

            list = deptManager.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
            alDept.AddRange(list);

            cmbDeptName.AddItems(alDept);
            cmbDeptName.SelectedIndex = 0;

            //ҩƷ����
            obj = new Neusoft.FrameWork.Models.NeuObject();
            list = new ArrayList();

            obj.ID = "ALL";
            obj.Name = "ȫ��";
            alDrugQuality.Add(obj);

            list = manager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.DRUGQUALITY);
            alDrugQuality.AddRange(list);
            cmbDrugQua.AddItems(alDrugQuality);
            cmbDrugQua.SelectedIndex = 0;

            base.OnLoad(e);
        }
        #endregion 

        #region ��ѯ
        protected override int OnRetrieve(params object[] objects)
        {
            if (this.GetQueryTime() == -1)
            {
                return -1;
            }

            deptId = cmbDeptName.SelectedItem.ID;
            drugQua = cmbDrugQua.SelectedItem.ID;
            return base.OnRetrieve(deptId,this.beginTime,this.endTime,drugQua);
           
        }
        #endregion 
    }
}