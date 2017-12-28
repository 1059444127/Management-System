using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Neusoft.FrameWork.Function;
using System.Collections;
using Neusoft.HISFC.Components.Common.Controls;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.Material.Out
{
    public class CommonOutPriv : IMatManager
    {
        /// <summary>
        /// ʵ������
        /// </summary>
        /// <param name="isSpecialOut">�Ƿ��������</param>
        /// <param name="ucMatOutManager">�������</param>
        public CommonOutPriv(bool isSpecialOut, Out.ucMatOut ucMatOutManager)
        {
            this.isSpecialOut = isSpecialOut;

            this.SetMatManagerProperty(ucMatOutManager);
        }

        #region �����

        /// <summary>
        /// �������
        /// </summary>
        private Out.ucMatOut outManager = null;

        /// <summary>
        /// ���ݱ�
        /// </summary>
        private DataTable dt = null;

        /// <summary>
        /// �洢����ʵ����Ϣ
        /// </summary>
        private System.Collections.Hashtable hsOutData = new Hashtable();

        /// <summary>
        /// �洢����ӵ���Ŀ��Ϣ ��ֹ�ظ����
        /// </summary>
        private System.Collections.Hashtable hsItemData = new Hashtable();

        /// <summary>
        /// ��������Ȩ��
        /// </summary>
        private Neusoft.HISFC.Models.Admin.PowerLevelClass3 privJoinClass3 = null;

        /// <summary>
        /// �洢����ʵ����Ϣ
        /// </summary>
        private System.Collections.Hashtable hsApplyData = new Hashtable();

        /// <summary>
        /// ���ʿ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Material.Store storeManager = new Neusoft.HISFC.BizLogic.Material.Store();

        /// <summary>
        /// ������Ŀ������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Material.MetItem itemManager = new Neusoft.HISFC.BizLogic.Material.MetItem();

        /// <summary>
        /// ����ѡ��ؼ�
        /// {ACF15225-76EA-4760-A8C4-93D2A2CDFA3A}
        /// </summary>
        private Material.ucMatListSelect ucListSelect = null;
        /// <summary>
        /// ��������ʱ�Ƿ�ʹ����С��λ
        /// </summary>
        private bool isUseMinUnit = true;

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        private bool isSpecialOut = false;

        /// <summary>
        /// ����ӡ����
        /// </summary>
        private List<Neusoft.HISFC.Models.Material.Output> alOutPut = null;

        #endregion

        #region ����

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="ucPhaManager"></param>
        private void SetMatManagerProperty(Out.ucMatOut ucOutManager)
        {
            this.outManager = ucOutManager;

            if (this.outManager != null)
            {
                //���ý�����ʾ
                this.outManager.IsShowItemSelectpanel = true;
                this.outManager.IsShowInputPanel = false;
                //����Ŀ�������Ϣ Ŀ����Ա��Ϣ
                this.outManager.SetTargetDept(false, true, Neusoft.HISFC.Models.IMA.EnumModuelType.Material, Neusoft.HISFC.Models.Base.EnumDepartmentType.L);
                this.outManager.SetTargetPerson(true, Neusoft.HISFC.Models.Base.EnumEmployeeType.P);
                //���ù�������ť��ʾ{ACF15225-76EA-4760-A8C4-93D2A2CDFA3A}
                //���ڶ����ĳ�true �ṩ��ⵥ��ѯ�������뼴�� by yuyun 08-7-29
                //����һ�����ĸ��ĳ�false һ������ʱ�򲻲�ѯ���뵥�Ͳɹ���
                this.outManager.SetToolBarButtonVisible(false, true, false, false, true, true, false);
                //������ʾ�Ĵ�ѡ������
                this.outManager.SetSelectData("2", false, null, null, null);
                //������Ŀ�б���
                this.outManager.SetItemListWidth(4);
                //��ʾ��Ϣ����
                this.outManager.ShowInfo = "";

                if (this.isSpecialOut)
                {
                    this.outManager.TargetDept = this.outManager.DeptInfo;
                }

                this.outManager.Fp.EditModeReplace = true;

                this.outManager.EndTargetChanged -= new ucIMAInOutBase.DataChangedHandler(outManager_EndTargetChanged);
                this.outManager.EndTargetChanged += new ucIMAInOutBase.DataChangedHandler(outManager_EndTargetChanged);

                this.outManager.FpKeyEvent -= new ucIMAInOutBase.FpKeyHandler(outManager_FpKeyEvent);
                this.outManager.FpKeyEvent += new ucIMAInOutBase.FpKeyHandler(outManager_FpKeyEvent);

                this.outManager.Fp.EditModeOff -= new EventHandler(Fp_EditModeOff);
                this.outManager.Fp.EditModeOff += new EventHandler(Fp_EditModeOff);

                Neusoft.HISFC.BizLogic.Manager.PowerLevelManager powerLevelManager = new Neusoft.HISFC.BizLogic.Manager.PowerLevelManager();

                Neusoft.HISFC.Models.Admin.PowerLevelClass3 privClass3 = powerLevelManager.LoadLevel3ByPrimaryKey(this.outManager.Class2Priv.ID, this.outManager.PrivType.ID);

                if (privClass3 != null)
                {
                    privJoinClass3 = powerLevelManager.LoadLevel3ByPrimaryKey("0510", privClass3.Class3Code);
                }
            }
        }

        /// <summary>
        /// ��ʵ����Ϣ����DataTable��
        /// </summary>
        /// <param name="output">������Ϣ Output.User01�洢������Դ</param>
        /// <returns></returns>
        protected virtual int AddDataToTable(Neusoft.HISFC.Models.Material.Output output)
        {
            if (this.dt == null)
            {
                this.InitDataTable();
            }

            try
            {
                decimal storeQty = output.StoreBase.StoreQty;
                decimal outQty = output.StoreBase.Quantity;
                decimal outCost = output.StoreBase.Quantity * output.StoreBase.PriceCollection.PurchasePrice;

                if (!this.isUseMinUnit)			//ʹ�ð�װ��λ���г���
                {
                    storeQty = output.StoreBase.StoreQty / output.StoreBase.Item.PackQty;
                    outQty = output.StoreBase.Quantity / output.StoreBase.Item.PackQty;
                }

                if (this.isUseMinUnit)
                {
                    this.dt.Rows.Add(new object[] { 
													  output.StoreBase.Item.Name,                           //��Ʒ����
													  output.StoreBase.Item.Specs,                          //���
													  output.StoreBase.BatchNO,                             //����
													  output.StoreBase.PriceCollection.PurchasePrice,		//�����
													  output.StoreBase.PriceCollection.RetailPrice,			//���ۼ�
													  output.StoreBase.Item.PackUnit,						//��װ��λ
													  output.StoreBase.Item.MinUnit,						//��С��λ
													  storeQty,												//�������
													  outQty,												//��������
													  outCost,												//������
													  output.Memo,											//��ע
													  output.StoreBase.Item.ID,								//��Ŀ����
													  output.User01,										//������Դ
													  output.StoreBase.Item.SpellCode,						//ƴ����
													  output.StoreBase.Item.WbCode,							//�����
													  output.StoreBase.Item.UserCode,						//�Զ�����
													  output.User03											//����                        
												  }
                        );
                }
            }
            catch (System.Data.DataException e)
            {
                System.Windows.Forms.MessageBox.Show("DataTable�ڸ�ֵ��������" + e.Message);

                return -1;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("DataTable�ڸ�ֵ��������" + ex.Message);

                return -1;
            }

            return 1;
        }

        /// <summary>
        /// ����Dr�����ݶ�ʵ����и�ֵ
        /// </summary>
        /// <param name="dr">���ݱ�</param>
        /// <param name="sysTime">��ǰʱ��</param>
        /// <param name="output">ref ����ʵ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        protected virtual int GetOutputFormDataRow(DataRow dr, DateTime sysTime, ref Neusoft.HISFC.Models.Material.Output output)
        {
            //��������
            if (this.isUseMinUnit)			//ʹ����С��λ
            {
                output.StoreBase.Quantity = NConvert.ToDecimal(dr["��������"]);
            }
            else
            {
                output.StoreBase.Quantity = NConvert.ToDecimal(dr["��������"]) * output.StoreBase.Item.PackQty;
            }

            //{7A107ECA-0534-486b-AA74-8CFC0A1E01F2} д�������
            output.OutCost = output.StoreBase.PriceCollection.PurchasePrice * output.StoreBase.Quantity;

            output.StoreBase.Operation.Oper.ID = this.outManager.OperInfo.ID;
            output.StoreBase.Operation.Oper.OperTime = sysTime;

            output.StoreBase.Operation.ExamOper = output.StoreBase.Operation.Oper;		//������
            output.StoreBase.Operation.ExamQty = output.StoreBase.Quantity;

            output.Memo = dr["��ע"].ToString();										//��ע
            if (this.outManager.TargetPerson != null)									//��ҩ��
            {
                output.DrawOper = this.outManager.TargetPerson;
            }

            output.StoreBase.PrivType = this.outManager.PrivType.ID;               //��������
            output.StoreBase.SystemType = this.outManager.PrivType.Memo;           //ϵͳ����
            output.StoreBase.StockDept = this.outManager.DeptInfo;                 //��ǰ����
            output.StoreBase.TargetDept = this.outManager.TargetDept;              //Ŀ�����

            //�跽��Ŀ ��ʱ��ֵΪ��
            output.Debit = "";

            return 1;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="listCode">���뵥��</param>
        /// <param name="state">״̬</param>
        /// <returns>�ɹ�����1 </ʧ�ܷ���-1returns>
        protected virtual int AddApplyData(string listCode, string state)
        {
            this.Clear();

            ArrayList alDetail = this.storeManager.QueryApplyDetailByListNO(this.outManager.DeptInfo.ID, listCode, "0");
            if (alDetail == null)
            {
                MessageBox.Show(this.storeManager.Err);
                return -1;
            }

            ((System.ComponentModel.ISupportInitialize)(this.outManager.Fp)).BeginInit();



            foreach (Neusoft.HISFC.Models.Material.Apply apply in alDetail)
            {
                Neusoft.HISFC.Models.Material.Output output = new Neusoft.HISFC.Models.Material.Output();
                Neusoft.HISFC.Models.Material.MaterialItem item = new Neusoft.HISFC.Models.Material.MaterialItem();

                item = this.itemManager.GetMetItemByMetID(apply.Item.ID);

                if (output.StoreBase.Item == null)
                {
                    MessageBox.Show("��������ʱ �������ʱ������������Ŀ�ֵ���Ϣʧ��" + apply.Item.ID);
                    return -1;
                }

                output.StoreBase.Item = item;

                output.StoreBase.Quantity = apply.Operation.ApplyQty;			 //������
                output.Memo = apply.Memo;										 //��ע��Ϣ

                decimal storeQty = 0;
                if (this.storeManager.GetStoreQty(this.outManager.DeptInfo.ID, apply.Item.ID, out storeQty) == -1)
                {
                    MessageBox.Show("��ȡ" + apply.Item.Name + "�������ʱ��������" + this.itemManager.Err);
                    return -1;
                }
                output.StoreBase.StoreQty = storeQty;							 //�����

                output.StoreBase.PrivType = this.outManager.PrivType.ID;		 //��������
                output.StoreBase.SystemType = this.outManager.PrivType.Memo;     //ϵͳ����
                output.StoreBase.StockDept = this.outManager.DeptInfo;			 //��ǰ����
                output.StoreBase.TargetDept = this.outManager.TargetDept;		 //Ŀ�����
                output.StoreBase.PriceCollection.PurchasePrice = item.UnitPrice;
                //				output.StoreBase.Quantity = apply.OutQty;

                output.User01 = "1";											//������Դ ����
                output.User02 = apply.ID;										//���뵥��ˮ��

                //output.User03 = this.GetKey();												//��������
                output.User03 = output.StoreBase.Item.ID.ToString() + output.StoreBase.PriceCollection.RetailPrice.ToString();	//��������(�����۸���ͬ)

                if (this.AddDataToTable(output) == 1)
                {
                    this.hsOutData.Add(output.User03, output);

                    this.hsApplyData.Add(apply.ID, apply);

                    this.hsItemData.Add(output.StoreBase.Item.ID, null);			//�����������Ŀ
                }
            }

            ((System.ComponentModel.ISupportInitialize)(this.outManager.Fp)).EndInit();

            //������ܳ�����
            this.CompuateSum();

            return 1;
        }

        //		/// <summary>
        //		/// ������Ʒ��Ϣ��ӳ����¼
        //		/// </summary>
        //		/// <param name="itemNO">������Ŀ����</param>
        //		/// <param name="storageQty">�������</param>
        //		/// <returns></returns>
        //		protected virtual int AddDrugData(string itemNO,decimal storageQty)
        //		{
        //			this.AddApplyData(itemNO,storageQty,0);
        //		}
        /// <summary>
        /// ������Ʒ��Ϣ��ӳ����¼
        /// </summary>
        /// <param name="itemNO">������Ŀ����</param>
        /// <param name="storageQty">�������</param>
        /// <returns></returns>
        protected virtual int AddDrugData(string itemNO, decimal storageQty, decimal price)
        {
            if (this.outManager.TargetDept.ID == "")
            {
                MessageBox.Show("��ѡ�����õ�λ!");
                return 0;
            }

            if (this.hsItemData.ContainsKey(itemNO + price.ToString()))
            {
                MessageBox.Show("����Ʒ�����");
                return 0;
            }

            Neusoft.HISFC.Models.Material.MaterialItem item = this.itemManager.GetMetItemByMetID(itemNO);
            if (item == null)
            {
                MessageBox.Show("���ݱ����ȡ�����ֵ���Ϣʱ��������" + this.itemManager.Err);
                return -1;
            }

            Neusoft.HISFC.Models.Material.Output output = new Neusoft.HISFC.Models.Material.Output();

            output.StoreBase.Item = item;												//��Ʒ��Ϣ

            output.StoreBase.PrivType = this.outManager.PrivType.ID;					//��������
            output.StoreBase.SystemType = this.outManager.PrivType.Memo;				//ϵͳ����
            output.StoreBase.StockDept = this.outManager.DeptInfo;						//��ǰ����
            output.StoreBase.TargetDept = this.outManager.TargetDept;					//Ŀ�����
            output.StoreBase.StoreQty = storageQty;										//�����
            if (price <= 0)
            {
                output.StoreBase.PriceCollection.PurchasePrice = item.UnitPrice;
            }
            else
            {
                output.StoreBase.PriceCollection.PurchasePrice = price;
                output.StoreBase.PriceCollection.RetailPrice = price;
            }

            output.User01 = "0";														//������Դ
            //output.User03 = this.GetKey();												//��������
            output.User03 = output.StoreBase.Item.ID.ToString() + output.StoreBase.PriceCollection.RetailPrice.ToString();

            if (this.AddDataToTable(output) == 1)
            {
                this.hsOutData.Add(output.User03, output);

                //this.hsItemData.Add(output.StoreBase.Item.ID, null);						//�洢����ӵ���Ŀ ��ֹ�ظ����
                this.hsItemData.Add(output.User03, null);
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
                this.outManager.TotCostInfo = string.Format("������:{0}", retailCost.ToString("C4"));
            }
        }

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <returns></returns>
        private string GetKey()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetKey(DataRow dr)
        {
            return dr["����"].ToString();
        }

        /// <summary>
        /// ������Ŀʵ���ȡ����
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private string GetKey(Neusoft.HISFC.Models.Material.Output output)
        {
            return output.User03;
        }

        /// <summary>
        /// ��Fp�ڻ�ȡ����
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="activeRow"></param>
        /// <returns></returns>
        private string GetKey(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            return sv.Cells[activeRow, (int)ColumnSet.ColKey].Text;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="activeRow"></param>
        /// <returns></returns>
        private string[] GetFindKey(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            return new string[] { sv.Cells[activeRow, (int)ColumnSet.ColKey].Text };
        }

        #endregion

        #region IMatManager ��Ա

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
												 new DataColumn("�����",    dtDec),
												 new DataColumn("���ۼ�",    dtDec),
												 new DataColumn("��װ��λ",  dtStr),
												 new DataColumn("��С��λ",  dtStr),
												 new DataColumn("�������",  dtDec),
												 new DataColumn("��������",  dtDec),
												 new DataColumn("������",  dtDec),
												 new DataColumn("��ע",      dtStr),
												 new DataColumn("��Ŀ����",  dtStr),
												 new DataColumn("������Դ",  dtStr),
												 new DataColumn("ƴ����",    dtStr),
												 new DataColumn("�����",    dtStr),
												 new DataColumn("�Զ�����",  dtStr),
												 new DataColumn("����",		 dtStr)
											 }
                );

            DataColumn[] keys = new DataColumn[1];

            keys[0] = this.dt.Columns["����"];

            this.dt.PrimaryKey = keys;

            this.dt.DefaultView.AllowDelete = true;
            this.dt.DefaultView.AllowEdit = true;
            this.dt.DefaultView.AllowNew = true;

            return this.dt;
        }

        /// <summary>
        /// ��Ŀ��Ϣ���
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="activeRow">������</param>
        /// <returns></returns>
        public int AddItem(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            //-----by yuyun 08-7-25 ��һ�б���Զ�����  ԭ�Զ������г����ʱ���{7019A2A6-ADCA-4984-944B-C4F1A312449A}
            //string itemNO = sv.Cells[activeRow, 0].Text;
            string itemNO = sv.Cells[activeRow, 11].Text;

            decimal storeQty = NConvert.ToDecimal(sv.Cells[activeRow, 4].Text);
            decimal storePrice = NConvert.ToDecimal(sv.Cells[activeRow, 3].Text);

            if (this.AddDrugData(itemNO, storeQty, storePrice) == 1)
            {
                this.SetFocusSelect();
            }
            return 1;
        }

        /// <summary>
        /// ������Ʒ��Ŀ
        /// </summary>
        /// <param name="item"></param>
        /// <param name="parms"></param>
        public int AddItem(FarPoint.Win.Spread.SheetView sv, Neusoft.HISFC.Models.Material.Input input)
        {
            return 0;
        }

        /// <summary>
        /// �����б���ʾ ����
        /// </summary>
        /// <returns></returns>

        public int ShowApplyList()
        {
            ArrayList alTemp = new ArrayList();
            //��ȡ������Ϣ{CAC9F782-773F-4507-AD2D-C0F73513FF42}
            string currentDeptID = string.Empty;
            currentDeptID = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;

            alTemp = this.storeManager.QueryApplySimple(this.outManager.TargetDept.ID, currentDeptID, "0510", "0", "13");

            if (alTemp == null)
            {
                System.Windows.Forms.MessageBox.Show("��ȡ������Ϣʧ��" + this.storeManager.Err);
                return -1;
            }

            Neusoft.FrameWork.Models.NeuObject selectObject = new Neusoft.FrameWork.Models.NeuObject();

            if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(alTemp, ref selectObject) == 1)
            {
                this.Clear();

                Neusoft.FrameWork.Models.NeuObject targeDept = new Neusoft.FrameWork.Models.NeuObject();

                this.AddApplyData(selectObject.ID, "0");
                this.SetFocusSelect();

                if (this.outManager.FpSheetView != null)
                    this.outManager.FpSheetView.ActiveRowIndex = 0;
            }

            return 1;
        }

        /// <summary>
        /// ��ʾ��ⵥ{ACF15225-76EA-4760-A8C4-93D2A2CDFA3A}
        /// </summary>
        /// <returns></returns>
        public int ShowInList()
        {
            try
            {
                if (this.ucListSelect == null)
                    this.ucListSelect = new ucMatListSelect();

                this.ucListSelect.Init();
                this.ucListSelect.DeptInfo = this.outManager.DeptInfo;
                System.Collections.Hashtable hsState = new Hashtable();
                hsState.Add("0", "δ¼��Ʊ");
                hsState.Add("1", "��¼��Ʊδ��׼");
                hsState.Add("2", "�Ѻ�׼");
                this.ucListSelect.InOutStateCollection = hsState;

                this.ucListSelect.State = "2";                  //�����״̬ 
                System.Collections.Hashtable hs = new Hashtable();
                hs.Add("06", null);                             //��������˿�ĵ���
                this.ucListSelect.MarkPrivType = hs;

                this.ucListSelect.Class2Priv = "0510";          //���

                this.ucListSelect.SelecctListEvent -= new Neusoft.HISFC.Components.Common.Controls.ucIMAListSelecct.SelectListHandler(ucListSelect_SelecctListEvent);
                this.ucListSelect.SelecctListEvent += new Neusoft.HISFC.Components.Common.Controls.ucIMAListSelecct.SelectListHandler(ucListSelect_SelecctListEvent);

                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(this.ucListSelect);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return -1;
            }

            return 1;
        }

        public int ShowOutList()
        {
            // TODO:  ��� CommonOutPriv.ShowOutList ʵ��
            return 0;
        }

        public int ShowStockList()
        {
            // TODO:  ��� CommonOutPriv.ShowStockList ʵ��
            return 0;
        }

        public bool Valid()
        {
            // TODO:  ��� CommonOutPriv.Valid ʵ��
            for (int i = 0; i < this.outManager.FpSheetView.Rows.Count; i++)
            {
                if (((decimal)this.outManager.FpSheetView.Cells[i, (int)ColumnSet.ColStoreQty].Value) < ((decimal)this.outManager.FpSheetView.Cells[i, (int)ColumnSet.ColOutQty].Value))
                {
                    MessageBox.Show("�����������ڿ������������������!", "��ʾ");
                    return false;
                }
            }
            //{30496509-D9AD-4049-A4AE-439BAFC0A704}
            if (this.outManager.TargetDept == null || string.IsNullOrEmpty(this.outManager.TargetDept.ID))
            {
                MessageBox.Show("��ѡ������Ŀ����ң�", "��ʾ");

                return false;
            }
            return true;
        }

        public int Delete(FarPoint.Win.Spread.SheetView sv, int delRowIndex)
        {
            try
            {
                if (sv != null && delRowIndex >= 0)
                {
                    string keys = string.Format(sv.Cells[delRowIndex, (int)ColumnSet.ColItemNO].Text.ToString() + sv.Cells[delRowIndex, (int)ColumnSet.ColRetailPrice].Text.ToString());

                    DataRow dr = this.dt.Rows.Find(keys);
                    if (dr != null)
                    {
                        this.outManager.Fp.StopCellEditing();

                        this.hsOutData.Remove(dr["��Ŀ����"].ToString() + dr["���ۼ�"].ToString());
                        this.hsItemData.Remove(dr["��Ŀ����"].ToString() + dr["���ۼ�"].ToString());

                        this.dt.Rows.Remove(dr);

                        this.outManager.Fp.StartCellEditing(null, false);
                        //�ϼƼ���
                        this.CompuateSum();
                    }
                }
            }
            catch (System.Data.DataException e)
            {
                System.Windows.Forms.MessageBox.Show("�����ݱ�ִ��ɾ��������������" + e.Message);
                return -1;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("�����ݱ�ִ��ɾ��������������" + ex.Message);
                return -1;
            }
            return 1;
        }

        public virtual void SetFormat()
        {
            this.outManager.FpSheetView.DefaultStyle.Locked = true;
            this.outManager.FpSheetView.DataAutoSizeColumns = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColTradeName].Width = 120F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColSpecs].Width = 70F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].Width = 65F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Width = 60F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Width = 60F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColStoreQty].Width = 80F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].Width = 70F;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].Width = 70F;

            //����ۺͳ�������ʾС�����4λ
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            numberCellType.DecimalPlaces = 4;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].CellType = numberCellType;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColBatchNO].Visible = false;          //���� 
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColItemNO].Visible = false;           //��Ʒ����
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColDataSource].Visible = false;       //������Դ
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColSpellCode].Visible = false;        //ƴ����
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColWBCode].Visible = false;           //�����
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColUserCode].Visible = false;         //�Զ�����
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColKey].Visible = false;                  //����

            if (this.isUseMinUnit)
                this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Visible = false;
            else
                this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Visible = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].Visible = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].Locked = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].BackColor = System.Drawing.Color.SeaShell;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Locked = false;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Width = 150F;
        }

        public int Clear()
        {
            this.hsOutData.Clear();
            this.hsApplyData.Clear();
            this.hsItemData.Clear();
            this.dt.Rows.Clear();
            this.dt.AcceptChanges();
            return 1;
        }

        public void Filter(string filterStr)
        {
            // TODO:  ��� CommonOutPriv.Filter ʵ��
        }

        public void SetFocusSelect()
        {
            if (this.outManager.FpSheetView != null)
            {
                if (this.outManager.FpSheetView.Rows.Count > 0)
                {
                    this.outManager.SetFpFocus();

                    this.outManager.FpSheetView.ActiveRowIndex = this.outManager.FpSheetView.Rows.Count - 1;
                    this.outManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColOutQty;
                }
                else
                {
                    this.outManager.SetFocus();
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
            //���Ϸ���
            if (!this.Valid())
            {
                return;
            }

            DialogResult rs = MessageBox.Show("ȷ����" + this.outManager.TargetDept.Name + "���г��������?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (rs == DialogResult.No)
                return;

            this.dt.DefaultView.RowFilter = "1=1";

            for (int i = 0; i < this.dt.DefaultView.Count; i++)
            {
                this.dt.DefaultView[i].EndEdit();
            }

            FarPoint.Win.Spread.CellType.NumberCellType numberCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            numberCellType.DecimalPlaces = 4;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].CellType = numberCellType;

            DataTable dtAddMofity = this.dt.GetChanges(DataRowState.Added | DataRowState.Modified);

            if (dtAddMofity == null || dtAddMofity.Rows.Count <= 0)
                return;

            //Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ��б������..���Ժ�");
            System.Windows.Forms.Application.DoEvents();

            #region ������

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            Neusoft.HISFC.BizLogic.Material.Baseset matConstant = new Neusoft.HISFC.BizLogic.Material.Baseset();
            this.storeManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            //matConstant.SetTrans(t.Trans);
            matConstant.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            #endregion
            //ȡϵͳʱ��
            DateTime sysTime = this.storeManager.GetDateTimeFromSysDateTime();

            #region �ж����ÿ����Ƿ�������

            bool isManagerStore = false;
            Neusoft.HISFC.Models.Material.MaterialStorage matStorage = matConstant.QueryStorageInfo(this.outManager.TargetDept.ID);
            if (matStorage != null && matStorage.IsStoreManage)
            {
                isManagerStore = true;
                DialogResult reResult = MessageBox.Show(Language.Msg(this.outManager.TargetDept.Name + "�����档ȷ�Ͻ��г��������?\n����ʱ��ֱ�Ӹ��¶Է����"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (reResult == DialogResult.No)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    return;
                }

                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ��б������..���Ժ�");
                System.Windows.Forms.Application.DoEvents();
            }

            #endregion

            //һ������Ӧ������¼
            Neusoft.HISFC.Models.Material.Input input = null;
            //���ⵥ�ݺ�
            string outListNO = null;
            //�������
            int serialNO = 0;

            this.alOutPut = new List<Neusoft.HISFC.Models.Material.Output>();

            Neusoft.HISFC.Models.Material.Output output;
            foreach (DataRow dr in dtAddMofity.Rows)
            {
                string key = this.GetKey(dr);

                output = this.hsOutData[key] as Neusoft.HISFC.Models.Material.Output;

                output.StoreBase.Operation.ExamOper.ID = this.outManager.OperInfo.ID;        //�����
                output.StoreBase.Operation.ExamOper.OperTime = sysTime;                         //�������
                output.StoreBase.Operation.Oper = output.StoreBase.Operation.ExamOper;     //������Ϣ
                output.StoreBase.PriceCollection.RetailPrice = NConvert.ToDecimal(dr["���ۼ�"].ToString());
                //output.StoreBase.RetailCost = 

                if(this.GetOutputFormDataRow(dr, sysTime, ref output) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Function.ShowMsg("��ȡ������ϸ��Ϣʧ��");
                    return;
                }

                serialNO++;
                output.StoreBase.SerialNO = serialNO;

                #region ��ȡ���ⵥ�ݺ�

                if (outListNO == null)
                {
                    outListNO = output.OutListNO;
                }
                if (outListNO == null || outListNO == "")
                {
                    //ȡ�³��ⵥ��
                    outListNO = this.storeManager.GetOutListNO(this.outManager.DeptInfo.ID);
                    if (outListNO == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg("��ȡ���ⵥ�ݺŷ�������");
                        return;
                    }
                }

                output.OutListNO = outListNO;

                #endregion

                #region ��ȡ�����

                decimal storeQty = 0;
                if (this.storeManager.GetStoreQty(output.StoreBase.StockDept.ID, output.StoreBase.Item.ID, out storeQty) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Function.ShowMsg("��ȡ�������ʱ����" + storeManager.Err);
                    return;
                }
                output.StoreBase.StoreQty = storeQty;    //����ǰ�������
                output.StoreBase.StoreCost = Math.Round(output.StoreBase.StoreQty / output.StoreBase.Item.PackQty * output.StoreBase.PriceCollection.PurchasePrice, 3);

                #endregion

                #region ���ݲ�ͬ�������ó�������״̬

                //if (isManagerStore)						//Ŀ��(����)���ҹ�����
                //    output.StoreBase.State = "1";       //���
                //else
                //    output.StoreBase.State = "2";       //��׼
                output.StoreBase.State = "2";

                if (this.isSpecialOut)					//������� ֱ�Ӹ���״̬Ϊ��׼ 
                {
                    output.StoreBase.SpecialFlag = "1";
                    output.StoreBase.State = "2";
                }

                if (output.StoreBase.State == "2")
                {
                    output.StoreBase.Operation.ApproveOper = output.StoreBase.Operation.Oper;
                }

                output.StoreBase.Returns = 0.0000M;
                #endregion
                if (!this.isSpecialOut)
                {
                    input = new Neusoft.HISFC.Models.Material.Input();

                    if (input.InListNO == "" || input.InListNO == null)
                    {
                        input.InListNO = this.storeManager.GetInListNO(this.outManager.TargetDept.ID);
                    }

                    if (isManagerStore && input.InListNO == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Function.ShowMsg("��Ŀ������Ҳ�������¼ʱ��ȡ��ⵥ�ų���");
                        return;
                    }

                    if (this.privJoinClass3 != null)
                    {
                        input.StoreBase.PrivType = this.privJoinClass3.Class3Code;
                        input.StoreBase.SystemType = this.privJoinClass3.Class3MeaningCode;
                    }
                    else
                    {
                        input.StoreBase.PrivType = "01";											     //һ������Ӧ���û�����
                        input.StoreBase.SystemType = "11";										 //һ�����
                    }

                    input.StoreBase.State = "2";

                    input.StoreBase.StockDept = this.outManager.TargetDept;
                    input.StoreBase.TargetDept = this.outManager.DeptInfo;

                    input.StoreBase.Operation.ExamOper.ID = this.outManager.OperInfo.ID;
                    input.StoreBase.Operation.ExamOper.OperTime = sysTime;

                    input.StoreBase.Operation.ApplyOper = input.StoreBase.Operation.ExamOper;
                    input.StoreBase.Operation.ApproveOper = input.StoreBase.Operation.ExamOper;

                    decimal matStoreQty = 0;

                    if (this.storeManager.GetStoreQty(input.StoreBase.StockDept.ID, output.StoreBase.Item.ID, out matStoreQty) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(Language.Msg("��ȡ" + output.StoreBase.Item.Name + "�������ʱ��������"));
                        return;
                    }
                    input.StoreBase.StoreQty = matStoreQty;
                }
                else
                {
                    input = null;
                }

                if (this.storeManager.Output(output, input, isManagerStore) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Function.ShowMsg("���Ᵽ�淢������" + this.storeManager.Err);
                    return;
                }
                alOutPut.Add(output);
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            Function.ShowMsg("����ɹ�");

            if (alOutPut.Count > 0)
            {
                if (MessageBox.Show("�Ƿ��ӡ?", "��ʾ:", System.Windows.Forms.MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {/*
                    Local.GyHis.Material.ucMatOutput ucMat = new Local.GyHis.Material.ucMatOutput();
                    ucMat.Decimals = 2;
                    ucMat.MaxRowNo = 17;

                    ucMat.SetDataForInput(alOutPut, 1, this.itemManager.Operator.ID, "1");
                  * */
                    this.Print();
                }

            }
            this.Clear();

            this.outManager.Init();

            FarPoint.Win.Spread.CellType.NumberCellType noCellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            noCellType.DecimalPlaces = 4;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].CellType = noCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].CellType = noCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].CellType = noCellType;
        }

        public void SaveCheck(bool IsHeaderCheck)
        {

        }

        public int Print()
        {
            if (this.outManager.IOutPrint != null)
            {
                this.outManager.IOutPrint.SetData(this.alOutPut);
            }
            return 1;
        }

        public int Cancel()
        {
            // TODO:  ��� InApplyPriv.Print ʵ��
            return 1;
        }

        public int ImportData()
        {
            return 1;
        }

        #endregion

        #region IMatManager ��Ա

        //{9E7FB328-89B3-4f43-A417-2EC3ACFC7093}
        //���ͷŵ��¼���Դ
        public void Dispose()
        {
            this.outManager.EndTargetChanged -= new ucIMAInOutBase.DataChangedHandler(outManager_EndTargetChanged);


            this.outManager.FpKeyEvent -= new ucIMAInOutBase.FpKeyHandler(outManager_FpKeyEvent);

            this.outManager.Fp.EditModeOff -= new EventHandler(Fp_EditModeOff);

        }

        #endregion

        #region �¼�
        //ѡ����ⵥ{ACF15225-76EA-4760-A8C4-93D2A2CDFA3A}
        private void ucListSelect_SelecctListEvent(string listCode, string state, Neusoft.FrameWork.Models.NeuObject targetDept)
        {
            //this.outManager.TargetDept = targetDept;

            this.Clear();

            this.AddInData(listCode, state);
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="listNO">��ⵥ��</param>
        /// <param name="state">״̬</param>
        /// <returns></returns>
        protected virtual int AddInData(string listCode, string state)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڸ��ݵ��ݼ������� ���Ժ�...");
            Application.DoEvents();

            ArrayList alDetail = this.storeManager.QueryInputDetailByListNO(this.outManager.DeptInfo.ID, listCode, "AAAA", state);
            if (alDetail == null)
            {
                Function.ShowMsg(this.storeManager.Err);

                return -1;
            }

            ((System.ComponentModel.ISupportInitialize)(this.outManager.Fp)).BeginInit();

            foreach (Neusoft.HISFC.Models.Material.Input input in alDetail)
            {
                Neusoft.HISFC.Models.Material.Output output = new Neusoft.HISFC.Models.Material.Output();
                input.StoreBase.PrivType = this.outManager.PrivType.ID;             //�������
                input.StoreBase.SystemType = this.outManager.PrivType.Memo;         //ϵͳ����
                //{30496509-D9AD-4049-A4AE-439BAFC0A704}
                if (this.ConvertInputToOutput(input, ref output) == -1)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

                    return -1;
                }

                if (this.AddDataToTable(output) == 1)
                {
                    this.hsOutData.Add(this.GetKey(output), output);
                }
                else
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

                    return -1;
                }
            }

            this.SetFormat();

            ((System.ComponentModel.ISupportInitialize)(this.outManager.Fp)).EndInit();

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            this.SetFocusSelect();

            return 1;
        }
        /// <summary>
        /// �������input������ת����output���
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private int ConvertInputToOutput(Neusoft.HISFC.Models.Material.Input input, ref Neusoft.HISFC.Models.Material.Output output)
        {
            Neusoft.HISFC.Models.Material.MaterialItem item = new Neusoft.HISFC.Models.Material.MaterialItem();

            item = this.itemManager.GetMetItemByMetID(input.StoreBase.Item.ID);
            if (item == null)
            {
                MessageBox.Show("��������ʱ �������ʱ������������Ŀ�ֵ���Ϣʧ��" + input.StoreBase.Item.ID);
                return -1;
            }

            output.StoreBase.Item = item;
            //by yuyun ������=�����-�˿���{ACF15225-76EA-4760-A8C4-93D2A2CDFA3A}
            output.StoreBase.Quantity = input.StoreBase.Quantity - input.StoreBase.Returns;
            //�����Ӧ���Ƕ�Ӧ�����ϸ�����ۼ�{30496509-D9AD-4049-A4AE-439BAFC0A704}
            Neusoft.HISFC.Models.Material.StoreDetail storeDetail = storeManager.GetStoreDetail(this.outManager.DeptInfo.ID, item.ID, input.StoreBase.StockNO);
            if (storeDetail.StoreBase.PriceCollection.RetailPrice == 0)
            {
                MessageBox.Show("�����û���ҵ���Ӧ���ε�������Ŀ��");

                return -1;
            }

            output.StoreBase.PriceCollection.RetailPrice = storeDetail.StoreBase.PriceCollection.RetailPrice;
            //output.StoreBase.PriceCollection.RetailPrice = input.StoreBase.PriceCollection.RetailPrice;
            //-----------------------------
            output.StoreBase.RetailCost = output.StoreBase.PriceCollection.RetailPrice * output.StoreBase.Quantity;
            output.Memo = input.Memo;

            decimal storeQty = 0;
            if (this.storeManager.GetStoreQty(this.outManager.DeptInfo.ID, input.StoreBase.Item.ID, out storeQty) == -1)
            {
                MessageBox.Show("��ȡ" + input.StoreBase.Item.Name + "�������ʱ��������" + this.itemManager.Err);
                return -1;
            }
            output.StoreBase.StoreQty = storeQty;							 //�����

            output.StoreBase.PrivType = this.outManager.PrivType.ID;		 //��������
            output.StoreBase.SystemType = this.outManager.PrivType.Memo;     //ϵͳ����
            output.StoreBase.StockDept = this.outManager.DeptInfo;			 //��ǰ����
            //if (this.outManager.TargetDept != null && !string.IsNullOrEmpty(this.outManager.TargetDept.ID))
            //{
            //    output.StoreBase.TargetDept = this.outManager.TargetDept;	//Ŀ����� 
            //}
            //else
            //{
            output.StoreBase.TargetDept = this.outManager.TargetDept;//Ŀ����� 
            //}
            output.StoreBase.PriceCollection.PurchasePrice = item.UnitPrice;
            output.OutCost = output.StoreBase.PriceCollection.PurchasePrice * output.StoreBase.Quantity;
            output.ApplyListCode = "";
            output.ApplySerialNO = 0;

            output.User01 = "2";											//������Դ
            output.User02 = "";										//���뵥��ˮ��

            output.User03 = output.StoreBase.Item.ID.ToString() + output.StoreBase.PriceCollection.PurchasePrice.ToString() + input.ID;

            return 1;
        }

        private void Fp_EditModeOff(object sender, EventArgs e)
        {
            if (this.outManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColOutQty)
            {
                DataRow dr = this.dt.Rows.Find(this.GetFindKey(this.outManager.FpSheetView, this.outManager.FpSheetView.ActiveRowIndex));
                if (dr != null)
                {
                    dr["������"] = NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["���ۼ�"]);

                    dr.EndEdit();

                    this.CompuateSum();
                }
            }
        }

        private void outManager_EndTargetChanged(Neusoft.FrameWork.Models.NeuObject changeData, object param)
        {

        }

        private void outManager_FpKeyEvent(System.Windows.Forms.Keys key)
        {
            if (this.outManager.FpSheetView != null)
            {
                if (key == Keys.Enter)
                {
                    if (this.outManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColOutQty)
                    {
                        if (this.outManager.FpSheetView.ActiveRowIndex == this.outManager.FpSheetView.Rows.Count - 1)
                        {
                            this.outManager.SetFocus();
                        }
                        else
                        {
                            this.outManager.FpSheetView.ActiveRowIndex++;
                            this.outManager.FpSheetView.ActiveColumnIndex = (int)ColumnSet.ColOutQty;
                        }
                    }
                }
            }
        }

        #endregion

        #region ��ö��

        /// <summary>
        /// ������
        /// </summary>
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
            /// ��Ŀ����	
            /// </summary>
            ColItemNO,
            /// <summary>
            /// ������Դ
            /// </summary>
            ColDataSource,
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
            ColUserCode,
            /// <summary>
            /// ����
            /// </summary>
            ColKey
        }

        #endregion
    }
}
