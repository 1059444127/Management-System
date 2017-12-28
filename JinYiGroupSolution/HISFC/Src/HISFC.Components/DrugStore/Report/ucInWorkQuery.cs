using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.DrugStore.Report
{
    /// <summary>
    /// [��������: סԺ��������ѯ]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007-03]<br></br>
    /// <�޸ļ�¼ 
    ///		 ��ʵ�� Ȩ��ϵͳ����
    ///  />
    /// </summary>
    public partial class ucInWorkQuery : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucInWorkQuery()
        {
            InitializeComponent();
        }

        #region �����

        /// <summary>
        /// ҩƷ����ҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        #endregion

        #region ����

        /// <summary>
        /// ��ѯ��ʼʱ��
        /// </summary>
        public DateTime BeginTime
        {
            get
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtBegin.Text);
            }
        }

        /// <summary>
        /// ��ѯ��ֹʱ��
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return Neusoft.FrameWork.Function.NConvert.ToDateTime(this.dtEnd.Text);
            }
        }

        #endregion

        #region ��������Ϣ

        /// <summary>
        /// ���幤��������
        /// </summary>
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="NeuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object NeuObject, object param)
        {
            //���ӹ�����
            //this.toolBarService.AddToolButton("����", "������ǰ��������Ϣ", 0, true, false, null);
            return this.toolBarService;
        }

        protected override int OnQuery(object sender, object neuObject)
        {
            this.Query();

            return 1;
        }

        public override int Export(object sender, object neuObject)
        {
            this.Export();

            return 1;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "����")
            {
                this.Export();
            }
            base.ToolStrip_ItemClicked(sender, e);
        }

        #endregion

        /// <summary>
        /// ���ݲ�ѯ
        /// </summary>
        private void Query()
        {
            if (this.cmbDept.SelectedValue == null || this.cmbDept.SelectedValue.ToString() == "")
            {
                MessageBox.Show(Language.Msg("��ѡ���ѯҩ��"));
                return;
            }

            try
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯ�����Ժ�...");
                Application.DoEvents();

                //��ѯͳ��
                DataSet ds = new DataSet();
                if (this.itemManager.ExecQuery("Pharmarcy.Report.Inpatient.Query", ref ds, this.cmbDept.SelectedValue.ToString(), this.BeginTime.ToString(), this.EndTime.ToString()) == -1)
                {
                    Function.ShowMsg("���ݲ�ѯʧ�ܣ��������Ա��ϵ��" + this.itemManager.Err);
                    return;
                }

                //��ʾͳ�ƽ��
                this.neuSpread1_Sheet1.DataSource = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
        }

        /// <summary>
        /// ��ϸ��ѯ
        /// </summary>
        /// <param name="operInfo">����Ա</param>
        /// <param name="class3MeaningCode">���ͱ���</param>
        private void QueryDetail(Neusoft.FrameWork.Models.NeuObject operInfo,string class3MeaningCode)
        {
            try
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(Language.Msg("���ڲ�ѯ�����Ժ�..."));
                Application.DoEvents();

                DataSet ds = new DataSet();
                if (this.itemManager.ExecQuery("Pharmarcy.Report.Inpatient.DetailQuery", ref ds, this.cmbDept.SelectedValue.ToString(),operInfo.ID, this.BeginTime.ToString(), this.EndTime.ToString(),class3MeaningCode) == -1)
                {
                    Function.ShowMsg("���ݲ�ѯʧ�ܣ��������Ա��ϵ��" + this.itemManager.Err);
                    return;
                }

                if (ds.Tables.Count > 0)
                {
                    //����¼��Чʱ ��ʱ��Ч���ڴ洢ҽ����ҩ���� ���δ�ʱ��������ʾ
                    if (ds.Tables[0].Columns.Contains("ȡ������") && ds.Tables[0].Columns.Contains("��Ч���") )
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["��Ч���"].ToString() == "��Ч")
                            {
                                dr["ȡ������"] = "";
                            }
                        }
                    }
                }

                this.neuSpread1_Sheet2.DataSource = ds;

                this.neuSpread1_Sheet2.SheetName = operInfo.Name + " ��ϸ��ҩ��Ϣ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Export()
        {
            if (this.neuSpread1.Export() == 1)
            {
                MessageBox.Show(Language.Msg("�����ɹ�"));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ������� ���Ժ�...");
                Application.DoEvents();

                //Ĭ��ʱ���				
                System.DateTime sysTime = this.itemManager.GetDateTimeFromSysDateTime();

                this.dtBegin.Value = new DateTime(sysTime.Year, sysTime.Month, sysTime.Day, 0, 0, 0);
                this.dtEnd.Value = new DateTime(sysTime.Year, sysTime.Month, sysTime.Day, 23, 59, 59);

                #region ����ҩ���б�

                //ҩ��ѡ��
                Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
                ArrayList al = deptManager.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
                if (al == null)
                {
                    Function.ShowMsg("����ҩ���б�ʧ��\n" + this.itemManager.Err);
                    return;
                }

                this.cmbDept.DataSource = al;
                this.cmbDept.DisplayMember = "Name";
                this.cmbDept.ValueMember = "ID";

                #endregion

                this.neuSpread1_Sheet1.DefaultStyle.Locked = true;
                this.neuSpread1_Sheet2.DefaultStyle.Locked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }

            base.OnLoad(e);
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                if (this.neuSpread1_Sheet1.RowCount == 0)
                {
                    return;
                }

                if (!this.neuSpread1.Sheets.Contains(this.neuSpread1_Sheet2))
                    this.neuSpread1.Sheets.Add(this.neuSpread1_Sheet2);

                if (this.neuSpread1.ActiveSheet == this.neuSpread1_Sheet1)
                {
                    string operCode = this.neuSpread1_Sheet1.Cells[e.Row, 0].Text;
                    string operName = this.neuSpread1_Sheet1.Cells[e.Row, 1].Text;
                    string class3Meaning = this.neuSpread1_Sheet1.Cells[e.Row, 2].Text == "��ҩ"?"Z2":"Z1";


                    Neusoft.FrameWork.Models.NeuObject operInfo = new Neusoft.FrameWork.Models.NeuObject();
                    operInfo.ID = operCode;
                    operInfo.Name = operName;

                    this.QueryDetail(operInfo,class3Meaning);
                }

                this.neuSpread1.ActiveSheet = this.neuSpread1_Sheet2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}