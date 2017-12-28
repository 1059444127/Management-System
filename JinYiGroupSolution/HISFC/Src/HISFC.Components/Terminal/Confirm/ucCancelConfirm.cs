using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Neusoft.HISFC.Components.Terminal.Confirm
{
    [System.Obsolete("�������� ͨ��ucCancelInpatientConfirm����",true)]
    public partial class ucCancelConfirm : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucCancelConfirm()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// ����ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Fee feeManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ҽ��ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Order orderManager = new Neusoft.HISFC.BizProcess.Integrate.Order();

        /// <summary>
        /// ����ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.RADT radtManager = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// ϵͳ����ҵ��
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager deptManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        /// <summary>
        /// �ն�ҵ��
        /// </summary>
        private Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm tecManager = new Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        private Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();

        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee oper = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
        private bool seeAll= false;
        #endregion

        #region ����
        /// <summary>
        /// �鿴���п����ն�ȷ����Ŀ
        /// </summary>
        [Category("�ؼ�����"), Description("�鿴���п����ն�ȷ����Ŀ")]
        public bool SeeAll
        {
            get
            {
                return seeAll;
            }
            set
            {
                seeAll = value;
            }
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo
        {
            set
            {
                this.patientInfo = value;
                this.txtName.Text = this.patientInfo.Name;
                this.txtPact.Text = this.patientInfo.Pact.Name;
                this.AddDataToFp(this.QueryExeData());
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected virtual void Init()
        {
            this.neuSpread1_Sheet1.Columns[0].Visible = false;
            this.neuSpread1_Sheet1.Columns[1].Visible = false;
            this.neuSpread1_Sheet1.Columns[6].Visible = false;
            this.neuSpread1_Sheet1.Columns[7].Visible = false;
            for (int i = 0; i < this.fpSpread1_Sheet1.ColumnCount; i++)
            {
                this.fpSpread1_Sheet1.Columns[i].Locked = true;
            }
            this.ucQueryInpatientNo1.myEvent += new Neusoft.HISFC.Components.Common.Controls.myEventDelegate(this.ucQueryInpatientNo1_myEvent);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <returns></returns>
        private ArrayList QueryExeData()
        {
            string OperDept = this.oper.Dept.ID;
            if (seeAll)
            {
                OperDept = "all";
            }

            ArrayList al = new ArrayList();
            al = this.feeManager.QueryExeFeeItemListsByInpatientNOAndDept(patientInfo.ID, OperDept);
            return al;
        }

        /// <summary>
        /// ������ݵ����
        /// </summary>
        /// <param name="al"></param>
        protected virtual void AddDataToFp(ArrayList al)
        {
            this.neuSpread1_Sheet1.RowCount = 0;
            if (al != null && al.Count > 0)
            {
                foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList feeItem in al)
                {
                    this.neuSpread1_Sheet1.Rows.Add(0, 1);
                    Neusoft.HISFC.Models.Base.Employee employee = new Neusoft.HISFC.Models.Base.Employee();
                    Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();

                    this.neuSpread1_Sheet1.Cells[0, 1].Text = feeItem.Item.ID;
                    this.neuSpread1_Sheet1.Cells[0, 2].Text = feeItem.Item.Name;
                    this.neuSpread1_Sheet1.Cells[0, 3].Text = feeItem.Item.Qty.ToString();
                    this.neuSpread1_Sheet1.Cells[0, 4].Text = feeItem.Item.PriceUnit;
                    this.neuSpread1_Sheet1.Cells[0, 5].Text = feeItem.Item.Price.ToString();
                    this.neuSpread1_Sheet1.Cells[0, 6].Text = feeItem.Order.ID;
                    this.neuSpread1_Sheet1.Cells[0, 7].Text = feeItem.ExecOrder.ID;
                    this.neuSpread1_Sheet1.Cells[0, 8].Tag = feeItem.ExecOper.ID;
                    employee = this.deptManager.GetEmployeeInfo(feeItem.ExecOper.ID);
                    this.neuSpread1_Sheet1.Cells[0, 8].Text = employee.Name;
                    this.neuSpread1_Sheet1.Cells[0, 9].Text = feeItem.ExecOper.OperTime.ToString();
                    employee = this.deptManager.GetEmployeeInfo(feeItem.RecipeOper.ID);
                    this.neuSpread1_Sheet1.Cells[0, 10].Tag = feeItem.RecipeOper.ID;
                    this.neuSpread1_Sheet1.Cells[0, 10].Text = employee.Name;
                    dept = this.deptManager.GetDepartment(feeItem.RecipeOper.Dept.ID);
                    this.neuSpread1_Sheet1.Cells[0, 11].Tag = feeItem.RecipeOper.Dept.ID;
                    this.neuSpread1_Sheet1.Cells[0, 11].Text = dept.Name;
                    Neusoft.HISFC.Models.Base.Department exeDept = this.deptManager.GetDepartment(feeItem.ExecOper.Dept.ID);
                    this.neuSpread1_Sheet1.Cells[0, 12].Text = exeDept.Name;
                    this.neuSpread1_Sheet1.Rows[0].Tag = feeItem;
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        private int Save()
        {
            if (this.neuSpread1_Sheet1.RowCount <= 0)
            {
                return 0;
            }
            if (this.fpSpread1_Sheet1.RowCount <= 0)
            {
                MessageBox.Show("��ѡ����ȡ������ϸ");
                return 0;
            }

            if (MessageBox.Show("�Ƿ�ȡ���ô��ն�ȷ�ϣ�\r\n ȡ��ȷ�ϲ������ɻ���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return 0;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm terMgr = new Neusoft.HISFC.BizLogic.Terminal.TerminalConfirm();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.tecManager.Connection);
            //t.BeginTransaction();
            this.feeManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.orderManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            //terMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail obj = ((Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail)this.fpSpread1_Sheet1.Cells[this.fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.MoOrder].Tag).Clone();
            #region ҽ��
            if (this.fpSpread1_Sheet1.RowCount == 1)//�����ʣһ������˵�����еĶ�ȡ����
            {
                if (terMgr.CancelInpatientConfirmMoOrder(obj) <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("����ҽ��ʧ��" + terMgr.Err);
                    return -1;
                }
            }
            #endregion 
            #region ����
            #region {2CE2BB1D-9DEB-4afa-90CF-15E3E44285E1} ��ȡ��ϸ��Ŀ����ֹ��ϸ��Ŀ��������Ŀ��Ŀ��һ��ʱ����
            //if (terMgr.CancelInpatientItemList(obj) <= 0)
            //{
            //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
            //    MessageBox.Show("���·�����ϸʧ��" + terMgr.Err);
            //    return -1;
            //}
            foreach (FarPoint.Win.Spread.Row r in this.fpSpread1_Sheet1.Rows)
            {

                Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail tcd =
                    ((Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail)this.fpSpread1_Sheet1.Cells[r.Index, (int)Cols.MoOrder].Tag).Clone();

                if (terMgr.CancelInpatientItemListWithSeq(tcd) <= 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("���·�����ϸʧ��" + terMgr.Err);
                    return -1;
                }
            } 
            #endregion
            #endregion 
            #region ȷ����ϸ
            if (terMgr.CancelInpatientConfirmDetail(obj) <= 0)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("����ȷ����ϸʧ��" + terMgr.Err);
                return -1;
            }
            #endregion  

            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show("ȡ���ɹ�");
            this.fpSpread1_Sheet1.Rows.Remove(this.fpSpread1_Sheet1.ActiveRowIndex,1);
            return 1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Init();
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            this.AddDataToFp(this.QueryExeData());
            return base.OnSave(sender, neuObject);
        }

        #endregion

        #region �¼�

        private void ucQueryInpatientNo1_myEvent()
        {
                        
            if (this.ucQueryInpatientNo1.InpatientNo == null || this.ucQueryInpatientNo1.InpatientNo == string.Empty)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�û��߲�����!����֤������"));

                return;
            }

            Neusoft.HISFC.Models.RADT.PatientInfo patientTemp = this.radtManager.GetPatientInfomation(this.ucQueryInpatientNo1.InpatientNo);
            if (patientTemp == null || patientTemp.ID == null || patientTemp.ID == string.Empty)
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�û��߲�����!����֤������"));

                return;
            }

            if (patientTemp.PVisit.InState.ID.ToString() == "N")
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�û����Ѿ��޷���Ժ���������շ�!"));

                //this.Clear();
                this.ucQueryInpatientNo1.Focus();

                return;
            }

            if (patientTemp.PVisit.InState.ID.ToString() == "O")
            {
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�û����Ѿ���Ժ���㣬�������շ�!"));

                //this.Clear();
                this.ucQueryInpatientNo1.Focus();

                return;
            }
            this.patientInfo = patientTemp;
            
            this.txtName.Text = this.patientInfo.Name;
            this.txtPact.Text = this.patientInfo.Pact.Name;
            this.AddDataToFp(this.QueryExeData());
        }


        #endregion

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            this.fpSpread1_Sheet1.RowCount = 0;
            int RowIndex = this.neuSpread1_Sheet1.ActiveRowIndex;
            string MoOrder = this.neuSpread1_Sheet1.Cells[RowIndex, 6].Text ;
            string ExecMoOrder = this.neuSpread1_Sheet1.Cells[RowIndex, 7].Text;
            List<Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail> list = tecManager.QueryTerminalConfirmDetailByMoOrderAndExeMoOrder(MoOrder, ExecMoOrder);
            if (list == null)
            {
                MessageBox.Show("��ȡȷ����ϸʧ��");
                return;
            }
            AddDetailToFp(list);
        }
        private int AddDetailToFp(List<Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail> list)
        {
            foreach (Neusoft.HISFC.Models.Terminal.TerminalConfirmDetail obj in list)
            {
                this.fpSpread1_Sheet1.Rows.Add(0, 1);
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.MoOrder].Text = obj.MoOrder;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.ExecMoOrder].Text = obj.ExecMoOrder;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.ApplyNum].Text = obj.Sequence;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.ItemID].Text = obj.Apply.Item.ID;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.ItemName].Text = obj.Apply.Item.Name;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.ConfirmQty].Text = obj.Apply.Item.ConfirmedQty.ToString();
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.OperCode].Text = obj.Apply.ConfirmOperEnvironment.ID;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.OperDept].Text = obj.Apply.ConfirmOperEnvironment.Dept.ID;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.OperTime].Text = obj.Apply.ConfirmOperEnvironment.OperTime.ToString();
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.RecipeNo].Text = obj.Apply.Item.RecipeNO;
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.SequenceNo].Text = obj.Apply.Item.SequenceNO.ToString();
                this.fpSpread1_Sheet1.Cells[0, (int)Cols.MoOrder].Tag = obj;
            }
            return 1;
        }
        private enum Cols
        {
            MoOrder ,//0
            ExecMoOrder,//1
            ApplyNum,//2
            ItemID,//3
            ItemName,//4
            ConfirmQty,//5
            OperCode,//6
            OperDept,//7
            OperTime,//8
            RecipeNo,//9
            SequenceNo//10
        }

    }
}

