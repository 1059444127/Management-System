using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;

namespace Neusoft.HISFC.Components.EPR.QC
{
    public partial class ucQCStat : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucQCStat()
        {
            InitializeComponent();
        }

        #region ����
        //Neusoft.HISFC.Management.EPR.QC qcManager = new Neusoft.HISFC.Management.EPR.QC();
        //Neusoft.HISFC.Management.Manager.Department deptManager = new Neusoft.HISFC.Management.Manager.Department();
        //Neusoft.HISFC.Management.RADT.InPatient patientManager = new Neusoft.HISFC.Management.RADT.InPatient();
        //Neusoft.HISFC.Management.EPR.QCInfo qcInfoManager = new Neusoft.HISFC.Management.EPR.QCInfo();

        private ArrayList alConditions = null;
        private ArrayList alDepts = null;
        /// <summary>
        /// ����ѡ��������
        /// </summary>
        private ArrayList alSelectedCondition;//add by pantiejun 2008-4-1
        #endregion

        #region ��ʼ��
        protected override void OnLoad(EventArgs e)
        {
            this.alDepts=Neusoft.HISFC.BizProcess.Factory.Function.IntegrateManager.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.I);
            this.cmbDept.AddItems(this.alDepts);
            Neusoft.HISFC.Models.RADT.InStateEnumService instate = new Neusoft.HISFC.Models.RADT.InStateEnumService();

            this.cmbState.AddItems(Neusoft.HISFC.Models.RADT.InStateEnumService.List());

            this.alConditions = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateEPR.GetQCConditionList();

            #region modified by pantiejun 2008-4-1 begin
            
            string conditionXml = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateEPR.GetSetting("1");
            if (!string.IsNullOrEmpty(conditionXml))
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ArrayList), new Type[] { typeof(Neusoft.HISFC.Models.EPR.QCCondition), typeof(Neusoft.HISFC.Models.EPR.QCConditions), typeof(Neusoft.FrameWork.Models.NeuObject) });
                    System.IO.StringReader sr = new System.IO.StringReader(conditionXml);
                    this.alSelectedCondition = xs.Deserialize(sr) as ArrayList;
                    this.InitFp();
                }catch{}
            }
            else
            {

                if (this.alConditions != null)
                {
                    this.InitFp();
                }
                else
                {
                    MessageBox.Show("��ȡ�ʿ���������");
                }
            }
            #endregion modified by pantiejun 2008-4-1 end
            base.OnLoad(e);
        }

        #region add by pantiejun 2008-4-1 begin
        /// <summary>
        /// ��ʼ���˵��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "��ʾ����":
                    this.SetSetting();
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// �����ʿ�����
        /// </summary>
        private void SetSetting()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "������ʾ����";
            ArrayList alNewSelectionCondition = new ArrayList();
            DialogResult dr = Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(new ucQCSetting(this.alConditions, this.alSelectedCondition, ref alNewSelectionCondition));
            if (dr == DialogResult.OK)
            {
                if (alNewSelectionCondition.Count > 0)
                {
                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ArrayList), new Type[] { typeof(Neusoft.HISFC.Models.EPR.QCCondition),typeof(Neusoft.HISFC.Models.EPR.QCConditions), typeof(Neusoft.FrameWork.Models.NeuObject) });
                        StringBuilder sb = new StringBuilder();
                        System.IO.StringWriter sw = new System.IO.StringWriter(sb);
                        xs.Serialize(sw, alNewSelectionCondition);
                        Neusoft.HISFC.BizProcess.Factory.Function.IntegrateEPR.SaveSetting(new Neusoft.FrameWork.Models.NeuObject("1", "�ʿ�ͳ������", ""), sb.ToString());
                        this.alSelectedCondition = alNewSelectionCondition;
                        this.InitFp();
                    }
                    catch
                    {
                        MessageBox.Show("�������ó��ִ������Ժ����ԡ�");
                    }
                }
            }

        }
        #endregion add by pantiejun 2008-4-1 end

        private void InitFp()
        {
            this.fpSheetView.ColumnCount = (this.alSelectedCondition == null ? this.alConditions.Count : this.alSelectedCondition.Count) + 2;////add by pantiejun 2008-4-1
            this.fpSheetView.Columns[-1].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            this.fpSheetView.Columns.Count = this.alConditions.Count + 2;
            this.fpSheetView.Columns[0].Label = "סԺ��";
            this.fpSheetView.Columns[1].Label = "����";
            this.fpSheetView.ColumnHeader.Rows.Get(0).Height = 59F;
            this.fpSheetView.Columns[-1].Width = 100;

            #region modified by pantiejun 2008-4-1 begin
            if (this.alSelectedCondition != null && this.alSelectedCondition.Count>0)
            {
                for (int i = 0; i < this.alSelectedCondition.Count; i++)
                {
                    this.fpSheetView.Columns[i + 2].Label = alSelectedCondition[i].ToString();
                }
            }
            else
            {
                for (int i = 0; i < this.alConditions.Count; i++)
                {
                    this.fpSheetView.Columns[i + 2].Label = this.alConditions[i].ToString();
                }
            }
            #endregion modified by pantiejun 2008-4-1 end

            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread1_CellDoubleClick);
        }

        void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.fpSheetView.Rows[e.Row].Tag == null) return;//add by pantiejun 2008-4-1
            if (this.fpSheetView.Rows[e.Row].Tag.GetType() ==typeof( Neusoft.HISFC.Models.RADT.PatientInfo))
            {
                Neusoft.HISFC.Models.RADT.PatientInfo curPatient = this.fpSheetView.Rows[e.Row].Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
                System.Windows.Forms.Panel emrPanel=new Panel();
                emrPanel.Size=new Size(800,600);
                Common.Classes.Function.EMRShow(emrPanel, curPatient, "0", false);
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(emrPanel);
            }
        }

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();
            toolBarService.AddToolButton("��ʾ����", "��ʾ����", 0, true, false, null);//add by pantiejun 2008-4-1
            //toolBarService.AddToolButton("����", "�����������ļ�", Neusoft.FrameWork.WinForms.Classes.EnumImageList.MĬ��, true, false, null);
            return toolBarService;
        }

       

        #endregion

        #region ����
        public override int Export(object sender, object neuObject)
        {
            SaveFileDialog form = new SaveFileDialog();
            form.Filter = "*.xls|*.xls";
            form.ShowDialog();
            this.fpSpread1.SaveExcel(form.FileName);
            return 0;
        }
       
        protected override int OnQuery(object sender, object neuObject)
        {
            if (this.radDept.Checked)
            {
                if (this.cmbDept.Tag == null)
                {
                    MessageBox.Show("��ѡ����ң�");
                    this.cmbDept.Focus();
                    return -1;
                }
                else
                {
                    //modify by pantiejun 2008-4-8
                    Neusoft.HISFC.Models.RADT.InStateEnumService state = new Neusoft.HISFC.Models.RADT.InStateEnumService();
                    state.ID = this.cmbState.SelectedItem.ID;
                    //if (this.QueryByDept(this.cmbDept.Tag.ToString(), this.cmbState.SelectedItem as Neusoft.HISFC.Models.RADT.InStateEnumService) == -1)
                    if (this.QueryByDept(this.cmbDept.Tag.ToString(), state) == -1)
                    {
                        MessageBox.Show("û�з��������Ļ��ߣ�");
                        this.cmbDept.Focus();
                        return -1;
                    }
                }
            }

            else if (this.radInDate.Checked)
            {
                if (this.dtpBegin.Value > this.dtpEnd.Value)
                {
                    MessageBox.Show("��ѯ��ʼʱ�䲻�ܴ��ڽ���ʱ�䣡");
                    this.dtpBegin.Focus();
                    return -1;
                }
                if (this.QueryByInDate() == -1)
                {
                    MessageBox.Show("û�з��������Ļ��ߣ�");
                    this.dtpBegin.Focus();
                    return -1;
                }
            }

            else if (this.radInpatientNo.Checked)
            {
                if (this.txtInpatientNo.Text == "")
                {
                    MessageBox.Show("������סԺ�ţ�");
                    this.txtInpatientNo.Focus();
                    return -1;
                }
                this.QueryByPatientNo();
            }

            return base.OnQuery(sender, neuObject);
        }

        /// <summary>
        /// �����Ҳ�ѯ
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="state"></param>
        /// <returns>0 �ɹ��� -1 ʧ�� �� -2 û�з��ϵĻ���</returns>
        private int  QueryByDept(string deptCode, Neusoft.HISFC.Models.RADT.InStateEnumService state)
        {
            this.fpSheetView.Columns[0].Label = "סԺ��";
            this.fpSheetView.Columns[1].Label = "����";
            ArrayList alPatients = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QueryPatientByDept(deptCode, state);
            if (alPatients == null || alPatients.Count == 0)
            {
                return -1;
            }

            this.QueryQcDate(alPatients);
            return 0;
        }

        /// <summary>
        /// ����Ժ����Ժʱ���ѯ
        /// </summary>
        /// <param name="alPatients"></param>
        private int QueryByInDate()
        {
            ArrayList alPatients = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QuereyPatientByDate(this.dtpBegin.Value, this.dtpEnd.Value);
            if (alPatients == null || alPatients.Count == 0)
            {
                return -1;
            }
            this.QueryQcDate(alPatients);
            return 0;
        }

        /// <summary>
        /// ��סԺ�Ų�ѯ
        /// </summary>
        private void QueryByPatientNo()
        {
            Neusoft.HISFC.Models.RADT.PatientInfo patient = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateRADT.QueryPatientInfoByInpatientNO(this.txtInpatientNo.Text);
            if (patient != null)
            {

                this.fpSheetView.Rows.Add(0, 1);
                this.fpSheetView.Rows[0].Tag = patient;
                this.fpSheetView.Cells[0, 0].Text = patient.PID.PatientNO;
                this.fpSheetView.Cells[0, 1].Text = patient.Name;

                int column = 2;
                foreach (Neusoft.HISFC.Models.EPR.QCConditions condition in this.alConditions)
                {
                    bool isAccord = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateEPR.ExecQCInfo(patient.PID.ID, Common.Classes.Function.ISql, condition);
                    if (isAccord)
                    {

                        this.fpSheetView.Cells[0, column].ForeColor = Color.Red;
                        try
                        {
                            this.fpSheetView.Cells[0, column].Text = "��";
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        this.fpSheetView.Cells[0, column].Text = "��";
                    }

                    column++;
                }
            }
        }

        /// <summary>
        /// �ʿ���Ϣ��ѯ
        /// </summary>
        /// <param name="alPatients"></param>
        private void QueryQcDate(ArrayList alPatients)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯ�ʿ���Ϣ�����Ժ�");
            Application.DoEvents();

            int count = 0;
            this.fpSheetView.RowCount = 0;

            foreach (Neusoft.HISFC.Models.RADT.PatientInfo patient in alPatients)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(count, alPatients.Count);
                count++;
                Application.DoEvents();

                this.fpSheetView.Rows.Add(0, 1);
                this.fpSheetView.Rows[0].Tag = patient;
                this.fpSheetView.Cells[0, 0].Text = patient.PID.PatientNO;
                this.fpSheetView.Cells[0, 1].Text = patient.Name;

                int column = 2;
                //add by pantiejun 2008-4-1
                ArrayList alSearchCondition = this.alSelectedCondition == null ? this.alConditions : this.alSelectedCondition;
                //foreach (Neusoft.HISFC.Models.EPR.QCConditions condition in this.alConditions)
                foreach (Neusoft.HISFC.Models.EPR.QCConditions condition in alSearchCondition)////modify by pantiejun 2008-4-1
                {
                    bool isAccord = Neusoft.HISFC.BizProcess.Factory.Function.IntegrateEPR.ExecQCInfo(patient.PID.ID, Common.Classes.Function.ISql, condition);
                    if (isAccord)
                    {

                        this.fpSheetView.Cells[0, column].ForeColor = Color.Red;
                        try
                        {
                            this.fpSheetView.Cells[0, column].Text = "��";
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        this.fpSheetView.Cells[0,column].Text="��";
                    }

                    column++;
                }
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();//add by pantiejun 2008-4-1
        }


        #endregion
    }
}
