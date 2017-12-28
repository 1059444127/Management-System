using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;
using Neusoft.HISFC.Components.Common.Controls;

namespace Neusoft.HISFC.Components.Pharmacy.Out
{
    /// <summary>
    /// [��������: ��������ҵ����]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-12]<br></br>
    /// </summary>
    public class ApplyOutPriv : Neusoft.HISFC.Components.Pharmacy.In.IPhaInManager
    {
        public ApplyOutPriv(Neusoft.HISFC.Components.Pharmacy.Out.ucPhaOut ucPhaManager)
        {
            this.SetPhaManagerProperty(ucPhaManager);
        }

        #region �����

        private Neusoft.HISFC.Components.Pharmacy.Out.ucPhaOut phaOutManager = null;

        private DataTable dt = null;

        /// <summary>
        /// ֻ��Fp��Ԫ������
        /// </summary>
        private FarPoint.Win.Spread.CellType.TextCellType tReadOnly = new FarPoint.Win.Spread.CellType.TextCellType();

        /// <summary>
        /// �洢����ʵ����Ϣ
        /// </summary>
        private System.Collections.Hashtable hsApplyData = new Hashtable();

        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// ������ʾʱ�Ƿ�ʹ����С��λ
        /// </summary>
        private bool isUseMinUnit = false;

        #endregion

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="ucPhaManager"></param>
        private void SetPhaManagerProperty(Neusoft.HISFC.Components.Pharmacy.Out.ucPhaOut ucPhaManager)
        {
            this.phaOutManager = ucPhaManager;

            if (this.phaOutManager != null)
            {
                //���ý�����ʾ
                this.phaOutManager.IsShowItemSelectpanel = true;
                this.phaOutManager.IsShowInputPanel = false;
                //����Ŀ�������Ϣ Ŀ����Ա��Ϣ
                this.phaOutManager.SetTargetDept(false, true, Neusoft.HISFC.Models.IMA.EnumModuelType.Phamacy, Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
                //���ù�������ť��ʾ
                this.phaOutManager.SetToolBarButton(true, false, false, false, true);
                this.phaOutManager.SetToolBarButtonVisible(true, false, false, false, true, true, false);
                //������ʾ�Ĵ�ѡ������
                this.phaOutManager.SetSelectData("2", false, null, null, null);
                //������ʾ��Ϣ
                this.phaOutManager.ShowInfo = "";

                this.phaOutManager.Fp.EditModeReplace = true;
                this.phaOutManager.FpSheetView.DataAutoSizeColumns = false;

                this.phaOutManager.EndTargetChanged -= new ucIMAInOutBase.DataChangedHandler(phaOutManager_EndTargetChanged);
                this.phaOutManager.EndTargetChanged += new ucIMAInOutBase.DataChangedHandler(phaOutManager_EndTargetChanged);

                this.phaOutManager.FpKeyEvent -= new ucIMAInOutBase.FpKeyHandler(phaOutManager_FpKeyEvent);
                this.phaOutManager.FpKeyEvent += new ucIMAInOutBase.FpKeyHandler(phaOutManager_FpKeyEvent);

                this.phaOutManager.Fp.EditModeOff -= new EventHandler(Fp_EditModeOff);
                this.phaOutManager.Fp.EditModeOff += new EventHandler(Fp_EditModeOff);

                this.phaOutManager.SetItemListWidth(2);
            }
        }

        /// <summary>
        /// ���ر��ŵ��ݲ��
        /// </summary>
        public virtual void CompuateSum()
        {
            decimal retailCost = 0;

            if (this.dt != null)
            {
                foreach (DataRow dr in this.dt.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                        retailCost += NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["���ۼ�"]);
                }
                this.phaOutManager.TotCostInfo = string.Format("������:{0}", retailCost.ToString("N"));
            }
        }

        /// <summary>
        /// ��ʽ��
        /// </summary>
        /// <param name="sv"></param>
        protected virtual void SetFormat()
        {
            this.tReadOnly.ReadOnly = true;

            this.phaOutManager.FpSheetView.DefaultStyle.Locked = true;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColTradeName].Width = 120F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColSpecs].Width = 70F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].Width = 65F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Width = 60F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Width = 60F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColApplyQty].Width = 70F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColApplyCost].Width = 70F;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColDrugNO].Visible = false;           //ҩƷ����
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColSpellCode].Visible = false;        //ƴ����
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColWBCode].Visible = false;           //�����
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColUserCode].Visible = false;         //�Զ�����

            if (this.isUseMinUnit)
                this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Visible = false;
            else
                this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Visible = false;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColApplyQty].Locked = false;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColApplyQty].BackColor = System.Drawing.Color.SeaShell;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Width = 150F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Locked = false;
        }

        /// <summary>
        /// ��Datatable����������
        /// </summary>
        /// <param name="applyOut"></param>
        /// <returns></returns>
        private int AddDataToTable(Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut)
        {
            if (this.dt == null)
            {
                this.InitDataTable();
            }

            try
            {
                decimal applyCost = applyOut.Operation.ApplyQty * applyOut.Item.PriceCollection.RetailPrice / applyOut.Item.PackQty;

                if (this.isUseMinUnit)
                {
                    this.dt.Rows.Add(new object[] { 
                                                applyOut.Item.Name,                            //��Ʒ����
                                                applyOut.Item.Specs,                           //���
                                                applyOut.BatchNO,                              //����
                                                applyOut.Item.PriceCollection.RetailPrice,     //���ۼ�
                                                applyOut.Item.PackUnit,                        //��װ��λ
                                                applyOut.Item.MinUnit,                         //��С��λ
                                                applyOut.Operation.ApplyQty,                   //��������
                                                applyCost,                                     //������
                                                applyOut.Memo,                                 //��ע
                                                applyOut.Item.ID,                              //ҩƷ����
                                                applyOut.Item.NameCollection.SpellCode,        //ƴ����
                                                applyOut.Item.NameCollection.WBCode,           //�����
                                                applyOut.Item.NameCollection.UserCode          //�Զ�����
                            
                                           }
                );
                }
                else
                {
                    this.dt.Rows.Add(new object[] { 
                                                applyOut.Item.Name,                            //��Ʒ����
                                                applyOut.Item.Specs,                           //���
                                                applyOut.BatchNO,                              //����
                                                applyOut.Item.PriceCollection.RetailPrice,     //���ۼ�
                                                applyOut.Item.PackUnit,                        //��װ��λ
                                                applyOut.Item.MinUnit,                         //��С��λ
                                                applyOut.Operation.ApplyQty / applyOut.Item.PackQty,//��������
                                                applyCost,                                      //������
                                                applyOut.Memo,                                 //��ע
                                                applyOut.Item.ID,                              //ҩƷ����
                                                applyOut.Item.NameCollection.SpellCode,        //ƴ����
                                                applyOut.Item.NameCollection.WBCode,           //�����
                                                applyOut.Item.NameCollection.UserCode          //�Զ�����
                            
                                           }
                                    );
                }
            }
            catch (System.Data.DataException e)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("DataTable�ڸ�ֵ��������" + e.Message));

                return -1;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("DataTable�ڸ�ֵ��������" + ex.Message));

                return -1;
            }

            return 1;
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="listNO"></param>
        private void AddApplyData(string listNO)
        {
            this.Clear();

            ArrayList alDetail = this.itemManager.QueryApplyOutInfoByListCode(this.phaOutManager.TargetDept.ID,listNO, "0");
            if (alDetail == null)
            {
                MessageBox.Show(Language.Msg(this.itemManager.Err));
                return;
            }

            ((System.ComponentModel.ISupportInitialize)(this.phaOutManager.Fp)).BeginInit();

            foreach (Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut in alDetail)
            {
                if (this.AddDataToTable(applyOut) == 1)
                {                   
                    this.hsApplyData.Add(applyOut.Item.ID + applyOut.BatchNO, applyOut);
                }
            }

            this.dt.AcceptChanges();

            ((System.ComponentModel.ISupportInitialize)(this.phaOutManager.Fp)).EndInit();

            this.SetFormat();

            this.SetFocusSelect();

            //������ܳ�����
            this.CompuateSum();
        }

        /// <summary>
        /// ����ҩƷ��Ϣ���������Ϣ
        /// </summary>
        /// <param name="drugNO"></param>
        /// <param name="batchNO"></param>
        /// <param name="storageQty"></param>
        /// <returns></returns>
        protected virtual int AddDrugData(string drugNO, string batchNO, decimal storageQty)
        {
            if (this.phaOutManager.TargetDept.ID == "")
            {
                MessageBox.Show(Language.Msg("��ѡ����ҩ��λ!"));
                return 0;
            }

            if (this.hsApplyData.ContainsKey(drugNO + batchNO))
            {
                MessageBox.Show(Language.Msg("��ҩƷ�����"));
                return 0;
            }

            Neusoft.HISFC.Models.Pharmacy.Item item = this.itemManager.GetItem(drugNO);
            if (item == null)
            {
                MessageBox.Show(Language.Msg("����ҩƷ�����ȡҩƷ�ֵ���Ϣʱ��������" + this.itemManager.Err));
                return -1;
            }

            Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut = new Neusoft.HISFC.Models.Pharmacy.ApplyOut();

            applyOut.Item = item;                                             //ҩƷ��Ϣ
            applyOut.BatchNO = batchNO;                                       //����
            applyOut.SystemType = this.phaOutManager.PrivType.Memo;           //ϵͳ����

            applyOut.StockDept = this.phaOutManager.DeptInfo;                //��ǰ����
            applyOut.ApplyDept = this.phaOutManager.TargetDept;              //Ŀ�����

            //applyOut.StockDept = this.phaOutManager.TargetDept;                //Ŀ�����
            //applyOut.ApplyDept = this.phaOutManager.DeptInfo;                  //��ǰ����

            if (this.AddDataToTable(applyOut) == 1)
            {
                this.hsApplyData.Add(drugNO + batchNO, applyOut);
            }

            return 1;
        }

        #region IPhaInManager ��Ա

        public Neusoft.FrameWork.WinForms.Controls.ucBaseControl InputModualUC
        {
            get
            {
                return null;
            }
        }

        public System.Data.DataTable InitDataTable()
        {
            System.Type dtBol = System.Type.GetType("System.Boolean");
            System.Type dtStr = System.Type.GetType("System.String");
            System.Type dtDec = System.Type.GetType("System.Decimal");
            System.Type dtDate = System.Type.GetType("System.DateTime");

            this.dt = new DataTable();

            this.dt.Columns.AddRange(
                                    new System.Data.DataColumn[] {
                                                                    new DataColumn("��Ʒ����",  dtStr),
                                                                    new DataColumn("���",      dtStr),
                                                                    new DataColumn("����",      dtStr),
                                                                    new DataColumn("���ۼ�",    dtDec),
                                                                    new DataColumn("��װ��λ",  dtStr),
                                                                    new DataColumn("��С��λ",  dtStr),
                                                                    new DataColumn("��������",  dtDec),
                                                                    new DataColumn("������",  dtDec),
                                                                    new DataColumn("��ע",      dtStr),
                                                                    new DataColumn("ҩƷ����",  dtStr),
                                                                    new DataColumn("ƴ����",    dtStr),
                                                                    new DataColumn("�����",    dtStr),
                                                                    new DataColumn("�Զ�����",  dtStr)
                                                                   }
                                  );

            DataColumn[] keys = new DataColumn[2];

            keys[0] = this.dt.Columns["ҩƷ����"];
            keys[1] = this.dt.Columns["����"];

            this.dt.PrimaryKey = keys;

            this.dt.DefaultView.AllowDelete = true;
            this.dt.DefaultView.AllowEdit = true;
            this.dt.DefaultView.AllowNew = true;

            return this.dt;
        }

        public int AddItem(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            string drugNO = sv.Cells[activeRow, 0].Text;
            string batchNO = sv.Cells[activeRow, 3].Text;
            decimal storeQty = NConvert.ToDecimal(sv.Cells[activeRow, 5].Text);

            if (this.AddDrugData(drugNO, batchNO, storeQty) == 1)
            {
                this.SetFormat();

                this.SetFocusSelect();
            }
            return 1;
        }

        public int ShowApplyList()
        {
            ArrayList alAllList = this.itemManager.QueryApplyOutListByTargetDept(this.phaOutManager.DeptInfo.ID, "24", "0");
            if (alAllList == null)
            {
                MessageBox.Show(Language.Msg("��ȡ���������б�������" + this.itemManager.Err));
                return -1;
            }

            ArrayList alList = new ArrayList();
            if (this.phaOutManager.TargetDept.ID != "")
            {
                foreach (Neusoft.FrameWork.Models.NeuObject info in alAllList)
                {
                    if (info.Memo != this.phaOutManager.TargetDept.ID)
                        continue;
                    alList.Add(info);
                }
            }
            else
            {
                alList = alAllList;
            }

            //��������ѡ�񵥾�
            Neusoft.FrameWork.Models.NeuObject selectObj = new Neusoft.FrameWork.Models.NeuObject();
            string[] fpLabel = { "���뵥��", "�������" };
            float[] fpWidth = { 120F, 120F };
            bool[] fpVisible = { true, true, false, false, false, false };

            if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(alList, ref selectObj) == 1)
            {
                this.Clear();

                Neusoft.FrameWork.Models.NeuObject targeDept = new Neusoft.FrameWork.Models.NeuObject();

                targeDept.ID = selectObj.Memo;              //������ұ���
                targeDept.Name = selectObj.Name;            //�����������
                targeDept.Memo = "0";                       //Ŀ�굥λ���� �ڲ�����       

                if (this.phaOutManager.TargetDept.ID != targeDept.ID)
                    this.phaOutManager.TargetDept = targeDept;

                this.AddApplyData(selectObj.ID);

                this.SetFocusSelect();

                if (this.phaOutManager.FpSheetView != null)
                    this.phaOutManager.FpSheetView.ActiveRowIndex = 0;
            }

            return 1;

        }

        public int ShowInList()
        {
            return 1;
        }

        public int ShowOutList()
        {
            return 1;
        }

        public int ShowStockList()
        {
            return 1;
        }

        public int ImportData()
        {
            return 1;
        }

        public bool Valid()
        {
            foreach (DataRow dr in this.dt.Rows)
            {
                if (NConvert.ToDecimal(dr["��������"]) <= 0)
                {
                    MessageBox.Show(Language.Msg(dr["��Ʒ����"].ToString() + "���������������Ϊ��"));
                    return false;
                }
            }

            return true;
        }

        public int Delete(FarPoint.Win.Spread.SheetView sv, int delRowIndex)
        {
            try
            {
                if (sv != null && delRowIndex >= 0)
                {
                    DialogResult rs = MessageBox.Show(Language.Msg("ȷ��ɾ������������?"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (rs == DialogResult.No)
                        return 0;

                    string[] keys = new string[]{
                                                sv.Cells[delRowIndex, (int)ColumnSet.ColDrugNO].Text,
                                                sv.Cells[delRowIndex, (int)ColumnSet.ColBatchNO].Text
                                            };
                    DataRow dr = this.dt.Rows.Find(keys);
                    if (dr != null)
                    {
                        Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut = this.hsApplyData[dr["ҩƷ����"].ToString() + dr["����"].ToString()] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;

                        //{58A3A4C6-6850-4eb7-AD66-7C1688FB1E43}  ���ж��Ƿ����ApplyOut.ID
                        if (string.IsNullOrEmpty(applyOut.ID) == false)
                        {
                            int parm = this.itemManager.DeleteApplyOut(applyOut.ID);
                            if (parm == -1)
                            {
                                Function.ShowMsg(this.itemManager.Err);
                                return -1;
                            }
                            if (parm == 0)
                            {
                                Function.ShowMsg("��������ѱ����⣬������");
                                return -1;
                            }
                        }

                        this.phaOutManager.Fp.StopCellEditing();

                        this.hsApplyData.Remove(dr["ҩƷ����"].ToString() + dr["����"].ToString());

                        this.dt.Rows.Remove(dr);

                        this.phaOutManager.Fp.StartCellEditing(null,false);

                        this.CompuateSum();
                    }
                }
            }
            catch (System.Data.DataException e)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����ݱ�ִ��ɾ��������������" + e.Message));
                return -1;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�����ݱ�ִ��ɾ��������������" + ex.Message));
                return -1;
            }

            return 1;
        }

        public int Clear()
        {
            try
            {
                this.dt.Rows.Clear();

                this.dt.AcceptChanges();

                this.hsApplyData.Clear();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("ִ����ղ�����������" + ex.Message));
                return -1;
            }

            return 1;
        }

        public void Filter(string filterStr)
        {
            if (this.dt == null)
                return;

            //��ù�������
            string queryCode = "%" + filterStr + "%";

            string filter = Function.GetFilterStr(this.dt.DefaultView, queryCode);

            try
            {
                this.dt.DefaultView.RowFilter = filter;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("���˷����쳣 " + ex.Message));
            }
            this.SetFormat();
        }

        public void SetFocusSelect()
        {
            if (this.phaOutManager.FpSheetView != null)
            {
                if (this.phaOutManager.FpSheetView.Rows.Count > 0)
                {
                    this.phaOutManager.SetFpFocus();

                    this.phaOutManager.FpSheetView.ActiveRowIndex = this.phaOutManager.FpSheetView.Rows.Count - 1;
                    this.phaOutManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColApplyQty;
                }
                else
                {
                    this.phaOutManager.SetFocus();
                }
            }
        }

        public void Save()
        {
            if (!this.Valid())
            {
                return;
            }

            DialogResult rs = MessageBox.Show(Language.Msg("ȷ����" + this.phaOutManager.TargetDept.Name + "����������������?"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (rs == DialogResult.No)
                return;          

            this.dt.DefaultView.RowFilter = "1=1";
            for (int i = 0; i < this.dt.DefaultView.Count; i++)
            {
                this.dt.DefaultView[i].EndEdit();
            }

            DataTable dtAddMofity = this.dt.GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dtAddMofity == null || dtAddMofity.Rows.Count <= 0)
                return;

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڱ���������Ϣ ���Ժ�...");
            Application.DoEvents();

            #region ������

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            Neusoft.HISFC.BizProcess.Integrate.Pharmacy phaIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            this.itemManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            //phaIntegrate.SetTrans(t.Trans);

            #endregion

            DateTime sysTime = this.itemManager.GetDateTimeFromSysDateTime();

            string applyListNO = "";

            foreach (DataRow dr in dtAddMofity.Rows)
            {
                string key = dr["ҩƷ����"].ToString() + dr["����"].ToString();

                Neusoft.HISFC.Models.Pharmacy.ApplyOut applyOut = this.hsApplyData[key] as Neusoft.HISFC.Models.Pharmacy.ApplyOut;

                if (applyOut.ID != "")
                {
                    applyListNO = applyOut.BillNO;          //���뵥��

                    #region ��ԭʼ������Ϣ�����޸� ֻ�������������

                    if (this.isUseMinUnit)
                        applyOut.Operation.ApplyQty = NConvert.ToDecimal(dr["��������"]);       //��������
                    else
                        applyOut.Operation.ApplyQty = NConvert.ToDecimal(dr["��������"]) * applyOut.Item.PackQty;       //��������

                    applyOut.Memo = dr["��ע"].ToString();                                  //��ע

                    int parm = this.itemManager.UpdateApplyOutNum(applyOut.ID, applyOut.Operation.ApplyQty);
                    if (parm == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg(this.itemManager.Err);
                        return;
                    }
                    else if (parm == 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg("�����뵥�ѱ���ˣ��޷������޸�!��ˢ������");
                        return;
                    }

                    #endregion
                }
                else
                {
                    #region ������������Ϣ

                    if (applyListNO == "")
                    {
                        // //{59C9BD46-05E6-43f6-82F3-C0E3B53155CB} ������ⵥ�Ż�ȡ��ʽ
                        applyListNO = phaIntegrate.GetInOutListNO(this.phaOutManager.DeptInfo.ID, false);
                        if (applyListNO == null)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            Function.ShowMsg("��ȡ���뵥��Ϣ��������" + phaIntegrate.Err);
                            return;
                        }
                    }

                    applyOut.BillNO = applyListNO;                                          //���뵥�ݺ�
                    applyOut.Operation.ApplyOper.ID = this.phaOutManager.OperInfo.ID;       //��������Ϣ
                    applyOut.Operation.ApplyOper.OperTime = sysTime;
                    applyOut.State = "0";                                                   //���뵥״̬

                    applyOut.Operation.Oper = applyOut.Operation.ApplyOper;

                    if (this.isUseMinUnit)
                        applyOut.Operation.ApplyQty = NConvert.ToDecimal(dr["��������"]);       //��������
                    else
                        applyOut.Operation.ApplyQty = NConvert.ToDecimal(dr["��������"]) * applyOut.Item.PackQty;       //��������

                    applyOut.Memo = dr["��ע"].ToString();                                  //��ע

                    if (this.itemManager.InsertApplyOut(applyOut) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg(this.itemManager.Err);
                        return;
                    }

                    #endregion
                }
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            //for (int i = 0; i < this.dt.DefaultView.Count; i++)
            //{
            //    this.dt.DefaultView[i].BeginEdit();
            //}

            Function.ShowMsg("����ɹ�");

            this.Clear();	
        }

        public int Print()
        {
            return 1;
        }

        #endregion

        #region IPhaInManager ��Ա

        public int Dispose()
        {
            return 1;
        }

        #endregion

        private void Fp_EditModeOff(object sender, EventArgs e)
        {
            if (this.phaOutManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColApplyQty)
            {
                string[] keys = new string[] { this.phaOutManager.FpSheetView.Cells[this.phaOutManager.FpSheetView.ActiveRowIndex, (int)ColumnSet.ColDrugNO].Text, this.phaOutManager.FpSheetView.Cells[this.phaOutManager.FpSheetView.ActiveRowIndex, (int)ColumnSet.ColBatchNO].Text };
                DataRow dr = this.dt.Rows.Find(keys);
                if (dr != null)
                {
                    dr["������"] = NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["���ۼ�"]);

                    dr.EndEdit();

                    this.CompuateSum();
                }
            }
        }

        private void phaOutManager_EndTargetChanged(Neusoft.FrameWork.Models.NeuObject changeData, object param)
        {
            return;
        }

        private void phaOutManager_FpKeyEvent(Keys key)
        {
            if (this.phaOutManager.FpSheetView != null)
            {
                if (key == Keys.Enter)
                {
                    if (this.phaOutManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColApplyQty)
                    {
                        if (this.phaOutManager.FpSheetView.ActiveRowIndex == this.phaOutManager.FpSheetView.Rows.Count - 1)
                        {
                            this.phaOutManager.SetFocus();
                        }
                        else
                        {
                            this.phaOutManager.FpSheetView.ActiveRowIndex++;
                            this.phaOutManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColApplyQty;
                        }
                    }
                }
            }
        }

        private enum ColumnSet
        {
            /// <summary>
            /// ��Ʒ����	
            /// </summary>
            ColTradeName,
            /// <summary>
            /// ���		
            /// </summary>
            ColSpecs,
            /// <summary>
            /// ����
            /// </summary>
            ColBatchNO,
            /// <summary>
            /// ���ۼ�		
            /// </summary>
            ColRetailPrice,
            /// <summary>
            /// ��װ��λ	
            /// </summary>
            ColPackUnit,
            /// <summary>
            /// ��С��λ
            /// </summary>
            ColMinUnit,
            /// <summary>
            /// ��������	
            /// </summary>
            ColApplyQty,
            /// <summary>
            /// ������	
            /// </summary>
            ColApplyCost,
            /// <summary>
            /// ��ע
            /// </summary>
            ColMemo,
            /// <summary>
            /// ҩƷ����	
            /// </summary>
            ColDrugNO,
            /// <summary>
            /// ƴ����
            /// </summary>
            ColSpellCode,
            /// <summary>
            /// �����
            /// </summary>
            ColWBCode,
            /// <summary>
            /// �Զ�����
            /// </summary>
            ColUserCode
        }

    }
}
