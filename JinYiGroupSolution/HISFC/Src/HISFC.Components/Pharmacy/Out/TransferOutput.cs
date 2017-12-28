using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;
using FarPoint.Win.Spread;
using System.Windows.Forms;
using Neusoft.HISFC.Components.Common.Controls;


namespace Neusoft.HISFC.Components.Pharmacy.Out
{
    /// <summary>
    /// [��������: ���ó���]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2007-08]<br></br>
    /// <˵��>
    ///     2�����ӿ��Ʋ������趨���ó���Ĭ�ϼӼ���
    ///     3������Ŀ��ҽԺ��ΪԺ�ڿ������
    ///     4�����ӿ��Ʋ������趨���ü۸��Ƿ�Ĭ��������
    /// </˵��>
    /// </summary>
    public class TransferOutput : Neusoft.HISFC.Components.Pharmacy.In.IPhaInManager
    {
        /// <summary>
        /// ���ó���
        /// </summary>
        /// <param name="ucPhaManager"></param>
        public TransferOutput(Neusoft.HISFC.Components.Pharmacy.Out.ucPhaOut ucPhaManager)
        {
            this.SetPhaManagerProperty(ucPhaManager);
        }

        #region �����

        private Neusoft.HISFC.Components.Pharmacy.Out.ucPhaOut phaOutManager = null;

        private DataTable dt = null;

        /// <summary>
        /// ֻ��Fp��Ԫ������
        /// </summary>
        private FarPoint.Win.Spread.CellType.NumberCellType numCellType = new FarPoint.Win.Spread.CellType.NumberCellType();

        /// <summary>
        /// �洢����ʵ����Ϣ
        /// </summary>
        private System.Collections.Hashtable hsOutData = new Hashtable();
    
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// ������ʾʱ�Ƿ�ʹ����С��λ
        /// </summary>
        private bool isUseMinUnit = false;

        /// <summary>
        /// ����ҩƷ���
        /// </summary>
        private System.Collections.Hashtable hsRestrainedQualityHelper = new Hashtable();

        /// <summary>
        /// Ĭ�ϼӼ���
        /// </summary>
        private decimal defaultPriceRate = 1.05M;

        /// <summary>
        /// �Ƿ�ʹ��������
        /// </summary>
        private bool useWholePrice = false;

        /// <summary>
        /// ����ӡ����
        /// </summary>
        private ArrayList alPrintData = null;
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
                this.phaOutManager.SetTargetPerson(true, Neusoft.HISFC.Models.Base.EnumEmployeeType.P);
                //���ù�������ť��ʾ
                this.phaOutManager.SetToolBarButtonVisible(false, false, false, false, true, true, false);
                //������ʾ�Ĵ�ѡ������
                this.phaOutManager.SetSelectData("2", Function.IsOutByBatchNO, null, null, null);

                this.phaOutManager.Fp.EditModeReplace = true;
                this.phaOutManager.FpSheetView.DataAutoSizeColumns = false;

                this.phaOutManager.EndTargetChanged -= new ucIMAInOutBase.DataChangedHandler(phaOutManager_EndTargetChanged);
                this.phaOutManager.EndTargetChanged += new ucIMAInOutBase.DataChangedHandler(phaOutManager_EndTargetChanged);

                this.phaOutManager.FpKeyEvent -= new ucIMAInOutBase.FpKeyHandler(phaOutManager_FpKeyEvent);
                this.phaOutManager.FpKeyEvent += new ucIMAInOutBase.FpKeyHandler(phaOutManager_FpKeyEvent);

                this.phaOutManager.Fp.EditModeOff -= new EventHandler(Fp_EditModeOff);
                this.phaOutManager.Fp.EditModeOff += new EventHandler(Fp_EditModeOff);

                this.phaOutManager.FpSheetView.DataAutoCellTypes = false;
                this.SetFormat();

                this.InitControlParam();

                //��ʾ��Ϣ����
                this.phaOutManager.ShowInfo = "��ǰ���üӼ��ʣ�" + this.defaultPriceRate.ToString("N");

                System.EventHandler eFun = new EventHandler(SetTransferRate);

                this.phaOutManager.AddToolBarButton("�Ӽ�����", "���ü��ó���Ӽ���", Neusoft.FrameWork.WinForms.Classes.EnumImageList.S����, 2, true, eFun);

                this.phaOutManager.SetItemListWidth(2);
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        private int InitControlParam()
        {
            Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

            this.defaultPriceRate = ctrlParamIntegrate.GetControlParam<decimal>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Out_Transfer_DefaultRate, true, 1.05M);
            this.useWholePrice = ctrlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Out_Transfer_UseWholePrice, true, false);

            return 1;
        }

        /// <summary>
        /// ��ʵ����Ϣ����DataTable��
        /// </summary>
        /// <param name="input">�����Ϣ Input.User01�洢������Դ</param>
        /// <returns></returns>
        protected virtual int AddDataToTable(Neusoft.HISFC.Models.Pharmacy.Output output)
        {
            if (this.dt == null)
            {
                this.InitDataTable();
            }

            try
            {
                output.RetailCost = output.Quantity / output.Item.PackQty * output.Item.PriceCollection.RetailPrice;

                if (this.isUseMinUnit)
                {
                    this.dt.Rows.Add(new object[] { 
                                                output.Item.Name,                            //��Ʒ����
                                                output.Item.Specs,                           //���
                                                output.BatchNO,                              //����
                                                output.Item.PriceCollection.PurchasePrice,
                                                output.Item.PriceCollection.RetailPrice,     //���ۼ�
                                                output.Item.PackUnit,                        //��װ��λ
                                                output.Item.MinUnit,                         //��С��λ
                                                output.StoreQty,                             //�������
                                                output.Quantity,                             //��������
                                                output.RetailCost,                           //������
                                                output.Memo,                                 //��ע
                                                output.Item.ID,                              //ҩƷ����
                                                output.Item.NameCollection.SpellCode,        //ƴ����
                                                output.Item.NameCollection.WBCode,           //�����
                                                output.Item.NameCollection.UserCode          //�Զ�����
                            
                                           }
                );
                }
                else
                {
                    this.dt.Rows.Add(new object[] { 
                                                output.Item.Name,                            //��Ʒ����
                                                output.Item.Specs,                           //���
                                                output.BatchNO,                              //����
                                                output.Item.PriceCollection.PurchasePrice,
                                                output.Item.PriceCollection.RetailPrice,     //���ۼ�
                                                output.Item.PackUnit,                        //��װ��λ
                                                output.Item.MinUnit,                         //��С��λ
                                                Math.Round(output.StoreQty / output.Item.PackQty,2),//�������
                                                output.Quantity / output.Item.PackQty,       //��������
                                                output.RetailCost,                           //������
                                                output.Memo,                                 //��ע
                                                output.Item.ID,                              //ҩƷ����
                                                output.Item.NameCollection.SpellCode,        //ƴ����
                                                output.Item.NameCollection.WBCode,           //�����
                                                output.Item.NameCollection.UserCode          //�Զ�����
                            
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
        /// ��ʽ��
        /// </summary>
        /// <param name="sv"></param>
        protected virtual void SetFormat()
        {
            this.phaOutManager.FpSheetView.DefaultStyle.Locked = true;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColTradeName].Width = 120F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColSpecs].Width = 70F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].Width = 65F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].Width = 65F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Width = 60F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Width = 60F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColStoreQty].Width = 80F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].Width = 70F;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].Width = 70F;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColBatchNO].Visible = Function.IsOutByBatchNO;          //���� 
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColDrugNO].Visible = false;           //ҩƷ����
            
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColSpellCode].Visible = false;        //ƴ����
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColWBCode].Visible = false;           //�����
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColUserCode].Visible = false;         //�Զ�����

            if (this.isUseMinUnit)
                this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Visible = false;
            else
                this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Visible = false;

            this.numCellType.DecimalPlaces = 2;
            this.numCellType.MinimumValue = 0;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].Locked = false;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].BackColor = System.Drawing.Color.SeaShell;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].CellType = this.numCellType;

            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Locked = false;
            this.phaOutManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Width = 150F;
        }     

        /// <summary>
        /// ����ҩƷ��Ϣ��ӳ����¼
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

            if (this.hsOutData.ContainsKey(drugNO + batchNO))
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

            //���ü��ó���۸� �ڵ�ǰ����ۻ����ϳ��԰ٷֱ�
            if (this.useWholePrice)
            {
                item.PriceCollection.RetailPrice = item.PriceCollection.WholeSalePrice;
            }
            else
            {
                item.PriceCollection.RetailPrice = this.defaultPriceRate * item.PriceCollection.PurchasePrice;
                item.PriceCollection.WholeSalePrice = item.PriceCollection.RetailPrice;
            }

            Neusoft.HISFC.Models.Pharmacy.Output output = new Neusoft.HISFC.Models.Pharmacy.Output();

            output.Item = item;                                             //ҩƷ��Ϣ
            output.BatchNO = batchNO;                                       //����
            output.PrivType = this.phaOutManager.PrivType.ID;               //��������
            output.SystemType = this.phaOutManager.PrivType.Memo;           //ϵͳ����
            output.StockDept = this.phaOutManager.DeptInfo;                 //��ǰ����
            output.TargetDept = this.phaOutManager.TargetDept;              //Ŀ�����
            output.StoreQty = storageQty;                                   //�����

            output.User01 = "0";                                            //������Դ

            if (this.AddDataToTable(output) == 1)
            {
                this.hsOutData.Add(drugNO + batchNO, output);
            }

            return 1;
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
                    retailCost += NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["���ۼ�"]);
                }
                this.phaOutManager.TotCostInfo = string.Format("������:{0}", retailCost.ToString("N"));
            }
        }

        /// <summary>
        /// �Ӽ�������
        /// </summary>
        protected void SetTransferRate(object sender, System.EventArgs args)
        {
            frmEasyData frm = new frmEasyData();

            frm.EasyLabel = "�Ӽ���";
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                try
                {
                    if (NConvert.ToDecimal(frm.EasyData) <= 0)
                    {
                        MessageBox.Show(Language.Msg("����ȷ���ü�����"),"��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);

                        this.SetTransferRate(null, System.EventArgs.Empty);
                    }
                    else
                    {
                        this.defaultPriceRate = NConvert.ToDecimal(frm.EasyData) + 1;

                        this.phaOutManager.ShowInfo = "��ǰ���üӼ��ʣ�" + this.defaultPriceRate.ToString("N");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(Language.Msg("����ȷ���ü�����"), "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.SetTransferRate(null, System.EventArgs.Empty);
                }
            }
        }

        #region IPhaInManager ��Ա

        public Neusoft.FrameWork.WinForms.Controls.ucBaseControl InputModualUC
        {
            get
            {
                return null;
            }
        }

        public DataTable InitDataTable()
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
                                                                    new DataColumn("�����",    dtDec),
                                                                    new DataColumn("���ۼ�",    dtDec),
                                                                    new DataColumn("��װ��λ",  dtStr),
                                                                    new DataColumn("��С��λ",  dtStr),
                                                                    new DataColumn("�������",  dtDec),
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
            decimal storeQty = 0;

            this.itemManager.GetStorageNum(this.phaOutManager.DeptInfo.ID, drugNO, out storeQty);

            if (this.AddDrugData(drugNO, batchNO, storeQty) == 1)
            {
                this.SetFormat();

                this.SetFocusSelect();
            }
            return 1;
        }

        public int ShowApplyList()
        {           
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
            try
            {

                foreach (DataRow dr in this.dt.Rows)
                {
                    if (NConvert.ToDecimal(dr["��������"]) <= 0)
                    {
                        MessageBox.Show(Language.Msg(dr["��Ʒ����"].ToString() + " ������������С�ڵ�����"));
                        return false;
                    }
                    if (NConvert.ToDecimal(dr["�������"]) < NConvert.ToDecimal(dr["��������"]))
                    {
                        MessageBox.Show(Language.Msg(dr["��Ʒ����"].ToString() + " �����������ܴ��ڵ�ǰ�����"));
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
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
                        this.phaOutManager.Fp.StopCellEditing();                       

                        this.hsOutData.Remove(dr["ҩƷ����"].ToString() + dr["����"].ToString());

                        this.dt.Rows.Remove(dr);

                        this.phaOutManager.Fp.StartCellEditing(null, false);
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

                this.hsOutData.Clear();
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
                    this.phaOutManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColOutQty;
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

            DialogResult rs = MessageBox.Show(Language.Msg("ȷ����" + this.phaOutManager.TargetDept.Name + "���г��������?"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
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

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ��б������..���Ժ�");
            System.Windows.Forms.Application.DoEvents();

            #region ������
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            Neusoft.HISFC.BizLogic.Pharmacy.Constant phaCons = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
            Neusoft.HISFC.BizProcess.Integrate.Pharmacy phaIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Pharmacy();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            this.itemManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            //phaIntegrate.SetTrans(t.Trans);
            //phaCons.SetTrans(t.Trans);

            #endregion

            DateTime sysTime = this.itemManager.GetDateTimeFromSysDateTime();
            //�ж����ÿ����Ƿ������
            string outListNO = "";
            bool isManagerStore = phaCons.IsManageStore(this.phaOutManager.TargetDept.ID);

            this.alPrintData = new ArrayList();

            //���۳��� ֻ�ۼ������ҿ�� �Թ���۳��԰ٷֱ���Ϊ���۳���
            foreach (DataRow dr in dtAddMofity.Rows)
            {
                string key = dr["ҩƷ����"].ToString() + dr["����"].ToString();
                Neusoft.HISFC.Models.Pharmacy.Output output = this.hsOutData[key] as Neusoft.HISFC.Models.Pharmacy.Output;

                output.Operation.ExamOper.ID = this.phaOutManager.OperInfo.ID;  //�����
                output.Operation.ExamOper.OperTime = sysTime;                   //�������
                output.Operation.Oper = output.Operation.ExamOper;              //������Ϣ                

                #region ��ȡ���ݺ�

                if (outListNO == "")
                {
                    // //{59C9BD46-05E6-43f6-82F3-C0E3B53155CB} ������ⵥ�Ż�ȡ��ʽ
                    outListNO = phaIntegrate.GetInOutListNO(this.phaOutManager.DeptInfo.ID, false);
                    if (outListNO == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg("��ȡ�³��ⵥ�ų���" + phaIntegrate.Err);
                        return;
                    }
                }

                output.OutListNO = outListNO;

                #endregion

                #region Outputʵ���Ҫ��Ϣ��ֵ

                if (this.isUseMinUnit)                  //ʹ����С��λ
                    output.Quantity = NConvert.ToDecimal(dr["��������"]);                       //��������
                else                                    //ʹ�ð�װ��λ
                    output.Quantity = NConvert.ToDecimal(dr["��������"]) * output.Item.PackQty; //��������

                output.StoreQty = output.StoreQty - output.Quantity;
                output.StoreCost = output.StoreQty * output.Item.PriceCollection.RetailPrice / output.Item.PackQty;

                output.Operation.ExamQty = output.Quantity;                     //�������
                output.Memo = dr["��ע"].ToString();
                output.DrugedBillNO = "0";                                      //��ҩ���� ����Ϊ��

                output.GetPerson = this.phaOutManager.TargetPerson.ID;          //��ҩ��

                //״̬�̶���ֵΪ2
                output.State = "2";         //��׼ 
                output.Operation.ApproveOper = output.Operation.Oper;

                #endregion

                #region ������Ϣ��ÿ�����������ʱ�Զ�����

                output.PrivType = this.phaOutManager.PrivType.ID;               //��������
                output.SystemType = this.phaOutManager.PrivType.Memo;           //ϵͳ����
                output.StockDept = this.phaOutManager.DeptInfo;                 //��ǰ����
                output.TargetDept = this.phaOutManager.TargetDept;              //Ŀ�����

                #endregion

                if (this.itemManager.Output(output, null, false) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Function.ShowMsg("���Ᵽ�淢������" + this.itemManager.Err);
                    return;
                }

                this.alPrintData.Add(output);
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            Function.ShowMsg("����ɹ�");

            DialogResult rsPrint = MessageBox.Show(Language.Msg("�Ƿ��ӡ���ⵥ��"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rsPrint == DialogResult.Yes)
            {
                this.Print();
            }

            this.Clear();
        }

        public int Print()
        {
            if (this.phaOutManager.IOutPrint != null)
            {
                this.phaOutManager.IOutPrint.SetData(this.alPrintData, this.phaOutManager.PrivType.Memo);
                this.phaOutManager.IOutPrint.Print();
            }

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
            if (this.phaOutManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColOutQty)
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
                    if (this.phaOutManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColOutQty)
                    {
                        if (this.phaOutManager.FpSheetView.ActiveRowIndex == this.phaOutManager.FpSheetView.Rows.Count - 1)
                        {
                            this.phaOutManager.SetFocus();
                        }
                        else
                        {
                            this.phaOutManager.FpSheetView.ActiveRowIndex++;
                            this.phaOutManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColOutQty;
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
            /// �����
            /// </summary>
            ColPurchasePrice,
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
            /// �������	
            /// </summary>
            ColStoreQty,
            /// <summary>
            /// ��������	
            /// </summary>
            ColOutQty,
            /// <summary>
            /// ������	
            /// </summary>
            ColOutCost,
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
