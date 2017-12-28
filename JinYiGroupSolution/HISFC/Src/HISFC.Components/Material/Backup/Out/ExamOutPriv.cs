using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Neusoft.FrameWork.Function;
using Neusoft.FrameWork.Management;
using Neusoft.HISFC.Components.Common.Controls;

namespace Neusoft.HISFC.Components.Material.Out
{
    /// <summary>
    /// [��������: ������������]
    /// [�� �� ��: ���S]
    /// [����ʱ��: 2008-7-30]
    /// <˵��>
    ///     1�������Ƿ������ ��ǰ�ɳ����ڽ��и�ֵ
    /// </˵��>
    /// </summary>
    class ExamOutPriv : Neusoft.HISFC.Components.Material.IMatManager
    {
        /// <summary>
        /// ʵ������
        /// </summary>
        /// <param name="isSpecialOut">�Ƿ��������</param>
        /// <param name="ucMatOutManager">�������</param>
        public ExamOutPriv(Neusoft.HISFC.Components.Material.Out.ucMatOut ucMatOutManager)
        {
            this.SetMatManagerProperty(ucMatOutManager);
        }

        private event System.EventHandler OnExpand;

        #region �����

        /// <summary>
        /// �������
        /// </summary>
        private Neusoft.HISFC.Components.Material.Out.ucMatOut outManager = null;

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

        private Neusoft.HISFC.BizLogic.Manager.Department myDept = new Neusoft.HISFC.BizLogic.Manager.Department();
        private Neusoft.HISFC.BizLogic.Manager.Person myPerson = new Neusoft.HISFC.BizLogic.Manager.Person();

        /// <summary>
        /// ��������ʱ�Ƿ�ʹ����С��λ
        /// </summary>
        private bool isUseMinUnit = true;

        public string showInfo = "";

        #endregion

        #region ����

        #endregion

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="ucPhaManager"></param>
        private void SetMatManagerProperty(Neusoft.HISFC.Components.Material.Out.ucMatOut ucOutManager)
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
                //���ù�������ť��ʾ
                this.OnExpand += new EventHandler(ExamOutPriv_OnExpand);
                //this.outManager.SetToolButton(true, false, false, true, false);
                this.outManager.SetToolBarButtonVisible(true, false, false, false, true, true, false);
                //{8402BFFB-C9CD-4418-BE02-0B3F45850CD3}
                //������ʾ�Ĵ�ѡ������//{8402BFFB-C9CD-4418-BE02-0B3F45850CD3}��������ֻ�ܰ����뵥����
                //this.outManager.SetSelectData("2", false, null, null, null);
                this.outManager.SetSelectData("4", false, null, null, null);
                //������Ŀ�б���
                this.outManager.SetItemListWidth(4);
                //��ʾ��Ϣ����
                this.outManager.ShowInfo = "�����������뵥����������";

                this.outManager.Fp.EditModePermanent = false;
                this.outManager.Fp.EditModeReplace = true;
                this.outManager.FpSheetView.DataAutoSizeColumns = false;

                this.outManager.Fp.EditModeReplace = true;

                this.outManager.EndTargetChanged -= new ucIMAInOutBase.DataChangedHandler(outManager_EndTargetChanged);
                this.outManager.EndTargetChanged += new ucIMAInOutBase.DataChangedHandler(outManager_EndTargetChanged);

                this.outManager.FpKeyEvent -= new ucIMAInOutBase.FpKeyHandler(outManager_FpKeyEvent);
                this.outManager.FpKeyEvent += new ucIMAInOutBase.FpKeyHandler(outManager_FpKeyEvent);

                this.outManager.Fp.EditModeOff -= new EventHandler(Fp_EditModeOff);
                this.outManager.Fp.EditModeOff += new EventHandler(Fp_EditModeOff);
                this.outManager.FpSheetView.DataAutoCellTypes = false;

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
													  true,
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
													  output.User03,										//����                        
                                                      output.StoreBase.TargetDept.ID                      //Ŀ�����
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

            output.StoreBase.Operation.Oper.ID = this.outManager.OperInfo.ID;
            output.StoreBase.Operation.Oper.OperTime = sysTime;

            output.StoreBase.Operation.ExamOper = output.StoreBase.Operation.Oper;		//������
            output.StoreBase.Operation.ExamQty = output.StoreBase.Quantity;

            output.Memo = dr["��ע"].ToString();										//��ע
            if (this.outManager.TargetPerson != null)									//��ҩ��
            {
                output.GetPerson = this.outManager.TargetPerson;
            }

            output.StoreBase.PrivType = this.outManager.PrivType.ID;               //��������
            output.StoreBase.SystemType = this.outManager.PrivType.Memo;           //ϵͳ����
            output.StoreBase.StockDept = this.outManager.DeptInfo;                 //��ǰ����
            output.StoreBase.TargetDept = myDept.GetDeptmentById(dr["Ŀ�����"].ToString());
            //this.outManager.TargetDept;              //Ŀ�����

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
        protected virtual int AddApplyData(string listCode, string deptCode, string state)
        {
            //this.Clear();

            ArrayList alDetail = new ArrayList();

            alDetail = this.storeManager.QueryApplyDetailByListNO(deptCode, listCode, state);

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
                output.StoreBase.Quantity = apply.Operation.ApplyQty - apply.OutQty; //by yuyun ��Ϊ������ - �������� 


                //liuxq��Ϊ������      //apply.Operation.ApplyQty;			 //������
                output.StoreBase.PriceCollection.RetailPrice = apply.ApplyPrice;    //wangw
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
                //if (this.outManager.TargetDept != null && !string.IsNullOrEmpty(this.outManager.TargetDept.ID))
                //{
                //    output.StoreBase.TargetDept = this.outManager.TargetDept;	//Ŀ����� 
                //}
                //else
                //{
                output.StoreBase.TargetDept = myDept.GetDeptmentById(apply.StockDept.ID);//Ŀ����� 
                //}
                output.StoreBase.PriceCollection.PurchasePrice = item.UnitPrice;
                output.ApplyListCode = apply.ApplyListNO;//���뵥��liuxq add
                output.ApplySerialNO = apply.SerialNO;//���뵥�����liuxq add
                //����������������չ�ֶ� ���������������� by yuyun 08-7-30
                output.StoreBase.Extend = output.StoreBase.Quantity.ToString();
                output.User01 = "1";											//������Դ ����
                output.User02 = apply.ID;										//���뵥��ˮ��

                output.User03 = output.StoreBase.Item.ID.ToString() + output.StoreBase.PriceCollection.PurchasePrice.ToString() + apply.ApplyListNO;									//��������

                if (this.AddDataToTable(output) == 1)
                {
                    this.hsOutData.Add(output.User03, output);

                    this.hsApplyData.Add(apply.ID, apply);

                    this.hsItemData.Add(output.User03, null);			//�����������Ŀ
                }
                if (apply == alDetail[0])
                {
                    Neusoft.HISFC.Models.Base.Employee person = myPerson.GetPersonByID(apply.Operation.ApproveOper.ID);

                    Neusoft.HISFC.Models.Base.Department dept = myDept.GetDeptmentById(apply.StockDept.ID);

                    if (person != null && dept != null)
                    {
                        this.showInfo = "���뵥:" + apply.ApplyListNO + " �������:" + dept.Name + " ��������:" + person.Name;
                    }
                }
            }

            ((System.ComponentModel.ISupportInitialize)(this.outManager.Fp)).EndInit();

            //������ܳ�����
            this.CompuateSum();

            return 1;
        }


        /// <summary>
        /// ������Ʒ��Ϣ��ӳ����¼
        /// </summary>
        /// <param name="itemNO">������Ŀ����</param>
        /// <param name="storageQty">�������</param>
        /// <returns></returns>
        protected virtual int AddDrugData(string itemNO, decimal storageQty, decimal price)
        {
            if (this.outManager.TargetDept.ID == "" || this.outManager.TargetDept.ID == "A")
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
            output.StoreBase.PriceCollection.PurchasePrice = price;
            output.StoreBase.PriceCollection.RetailPrice = price;

            output.User01 = "0";														//������Դ
            output.User03 = output.StoreBase.Item.ID.ToString() + output.StoreBase.PriceCollection.RetailPrice.ToString();												//��������

            if (this.AddDataToTable(output) == 1)
            {
                this.hsOutData.Add(output.User03, output);

                this.hsItemData.Add(output.User03, null);						//�洢����ӵ���Ŀ ��ֹ�ظ����
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
                    retailCost += NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["�����"]);
                }
                this.outManager.TotCostInfo = string.Format("������:{0}", retailCost.ToString("N"));
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


        #region IMatManager ��Ա

        public Neusoft.FrameWork.WinForms.Controls.ucBaseControl InputModualUC
        {
            get
            {
                return null;
            }
        }

        void ExamOutPriv_OnExpand(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
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
											     new DataColumn("����",      dtBol),
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
												 new DataColumn("����",		 dtStr),
                                                 new DataColumn("Ŀ�����",  dtStr)
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
            //-----by yuyun 08-7-25 ��һ�б���Զ�����  ԭ�Զ������г����ʱ���
            //string itemNO = sv.Cells[activeRow, 0].Text;
            string itemNO = sv.Cells[activeRow, 11].Text;

            decimal storeQty = NConvert.ToDecimal(sv.Cells[activeRow, 4].Text);
            Decimal price = NConvert.ToDecimal(sv.Cells[activeRow, 3].Text);

            if (this.AddDrugData(itemNO, storeQty, price) == 1)
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
            this.Clear();
            ArrayList alTemp = new ArrayList();
            //��������б���ѡ���ֵ����Ĭ�ϲ������������¼ by yuyun 08-7-28
            if (string.IsNullOrEmpty(this.outManager.TargetDept.ID))
            {
                this.outManager.TargetDept.ID = "A";
            }
            //------------
            //��Ҫ�ж����뵥��Ŀ������ǵ�ǰ��½���� by yuyun 08-7-28 
            string currentDeptID = string.Empty;
            currentDeptID = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.ID;
            //--------------------------------------------------------targetdept-----------currentdept----priv-----extend1-----inclass-
            //extend1��"0"����״̬ "1"���ƻ� "9" �������� "3" ȫ������
            alTemp = this.storeManager.QueryApplyListByDept(this.outManager.TargetDept.ID, this.outManager.DeptInfo.ID, "0510", "0','1','9", "13");
            //------------
            if (alTemp == null)
            {
                System.Windows.Forms.MessageBox.Show("��ȡ������Ϣʧ��" + this.storeManager.Err);
                return -1;
            }
            //����д�����뵥ѡ��Ŀؼ� by yuyun 08-7-28 
            Neusoft.HISFC.Components.Material.Base.ucApplyLists ucLists = new Neusoft.HISFC.Components.Material.Base.ucApplyLists();

            ArrayList selectApply = new ArrayList();

            ucLists.Init(alTemp);
            Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(ucLists);

            if (ucLists.ListApply.Count > 0)
            {
                this.Clear();

                foreach (ArrayList ar in ucLists.ListApply)
                {
                    this.AddApplyData(ar[1].ToString(), ar[3].ToString(), "0");
                }

                this.SetFocusSelect();

                if (this.outManager.FpSheetView != null)
                {
                    this.outManager.FpSheetView.ActiveRowIndex = 0;
                }
            }
            //---------
            //if (Neusoft.FrameWork.WinForms.Classes.Function.ChooseItem(alTemp, ref selectObject) == 1)
            //{
            //    this.Clear();

            //    Neusoft.FrameWork.Models.NeuObject targeDept = new Neusoft.FrameWork.Models.NeuObject();

            //    this.AddApplyData(selectObject.ID, "0");
            //    this.SetFocusSelect();

            //    if (this.outManager.FpSheetView != null)
            //        this.outManager.FpSheetView.ActiveRowIndex = 0;
            //}

            return 1;
        }


        public int ShowInList()
        {
            // TODO:  ��� CommonOutPriv.ShowInList ʵ��
            return 0;
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
            return true;
        }


        public int Delete(FarPoint.Win.Spread.SheetView sv, int delRowIndex)
        {
            try
            {
                if (sv != null && delRowIndex >= 0)
                {
                    DialogResult rs = MessageBox.Show("ȷ��ɾ������������?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (rs == DialogResult.No)
                    {
                        return 0;
                    }
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
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColKey].Visible = false;

            if (this.isUseMinUnit)
                this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPackUnit].Visible = false;
            else
                this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMinUnit].Visible = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].Visible = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].Locked = false;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColIsExam].Locked = false;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutQty].BackColor = System.Drawing.Color.SeaShell;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColIsExam].BackColor = System.Drawing.Color.SeaShell;

            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Locked = false;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColMemo].Width = 150F;

            FarPoint.Win.Spread.CellType.CheckBoxCellType ckBoxCellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColIsExam].CellType = ckBoxCellType;
        }


        public int Clear()
        {
            this.hsItemData.Clear();

            this.hsOutData.Clear();

            this.hsApplyData.Clear();

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
            //��������������ڿ������ʱ ������ɫ��� ����������
            foreach (FarPoint.Win.Spread.Row r in this.outManager.FpSheetView.Rows)
            {
                this.SetFpForeColor(r.Index);
            }
        }


        /// <summary>
        /// ����
        /// </summary>
        public void Save()
        {
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
            {
                return;
            }

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ��б������..���Ժ�");
            System.Windows.Forms.Application.DoEvents();

            #region ������
            //����ά��
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();
            Neusoft.HISFC.BizLogic.Material.Baseset matConstant = new Neusoft.HISFC.BizLogic.Material.Baseset();
            this.storeManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            //matConstant.SetTrans(t.Trans);

            #endregion

            DateTime sysTime = this.storeManager.GetDateTimeFromSysDateTime();

            #region �ж����ÿ����Ƿ�������

            bool isManagerStore = false;
            Neusoft.HISFC.Models.Material.MaterialStorage matStorage = matConstant.QueryStorageInfo(this.outManager.TargetDept.ID);
            if (matStorage != null && matStorage.IsStoreManage)
            {
                isManagerStore = true;
            }

            isManagerStore = true;

            if (!isManagerStore)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show(this.outManager.TargetDept.Name + " �������棬����ͨ�������������г���");
                return;
            }

            #endregion

            //���ⵥ�ݺ�
            string outListNO = null;
            int serialNO = 0;
            Neusoft.HISFC.Models.Material.Output output;
            List<Neusoft.HISFC.Models.Material.Output> alOutput = new List<Neusoft.HISFC.Models.Material.Output>();
            foreach (DataRow dr in dtAddMofity.Rows)
            {
                string key = this.GetKey(dr);

                output = this.hsOutData[key] as Neusoft.HISFC.Models.Material.Output;

                if (this.GetOutputFormDataRow(dr, sysTime, ref output) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("��ȡ������ϸ��Ϣʧ��");
                    return;
                }
                //�ж��Ƿ�ѡ�и���
                if (!Neusoft.FrameWork.Function.NConvert.ToBoolean(dr["����"]))
                {
                    continue;
                }
                //�жϳ��������Ƿ�Ϊ0
                if (output.StoreBase.Quantity == 0)
                {
                    continue;
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
                    outListNO = this.storeManager.GetOutListNO(this.outManager.DeptInfo.ID);
                    if (outListNO == null)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                        MessageBox.Show("��ȡ���ⵥ�ݺŷ�������");
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
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("��ȡ�������ʱ����" + storeManager.Err);
                    return;
                }
                output.StoreBase.StoreQty = storeQty;               //����ǰ�������
                output.StoreBase.StoreCost = Math.Round(output.StoreBase.StoreQty / output.StoreBase.Item.PackQty * output.StoreBase.PriceCollection.PurchasePrice, 3);

                #endregion

                #region ���ݲ�ͬ�������ó�������״̬

                if (isManagerStore)             //Ŀ��(����)���ҹ�����
                    output.StoreBase.State = "1";         //���
                else
                    output.StoreBase.State = "2";         //��׼

                if (output.StoreBase.State == "2")
                {
                    output.StoreBase.Operation.ApproveOper = output.StoreBase.Operation.Oper;
                }

                #endregion
                if (this.storeManager.Output(output) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("���Ᵽ�淢������" + this.storeManager.Err);
                    return;
                }
                if (output.StoreBase.PrivType == "04")
                {
                    //�жϳ����������������� �����������С���������� ��Ϊ��������  by yuyun 08-7-31
                    if (output.StoreBase.Quantity >= Neusoft.FrameWork.Function.NConvert.ToDecimal(output.StoreBase.Extend))
                    {
                        //ȫ������
                        //��state���³�P  extend1���³�3  approve_num���³�approve_num+��������
                        if (this.storeManager.UpdateApplyState(output.StoreBase.TargetDept.ID, output.ApplyListCode, output.ApplySerialNO, "P", output.StoreBase.Operation.Oper.ID, output.StoreBase.Operation.Oper.OperTime, output.StoreBase.Quantity, "3", output.StoreBase.Memo) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            MessageBox.Show("������·������� " + this.storeManager.Err);
                            return;
                        }
                    }
                    else
                    {
                        //��������
                        //��state���³�0  extend1���³�9  approve_num���³�approve_num+��������
                        if (this.storeManager.UpdateApplyState(output.StoreBase.TargetDept.ID, output.ApplyListCode, output.ApplySerialNO, "0", output.StoreBase.Operation.Oper.ID, output.StoreBase.Operation.Oper.OperTime, output.StoreBase.Quantity, "9", output.StoreBase.Memo) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            MessageBox.Show("������·������� " + this.storeManager.Err);
                            return;
                        }
                    }
                }

                alOutput.Add(output);

            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            MessageBox.Show("����ɹ�");
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            if (alOutput.Count > 0)
            {
                if (MessageBox.Show("�Ƿ��ӡ?", "��ʾ:", System.Windows.Forms.MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Print(alOutput);
                }
                /*
                if (MessageBox.Show("�Ƿ��ӡ?", "��ʾ:", System.Windows.Forms.MessageBoxButtons.YesNo)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    Local.GyHis.Material.ucMatOutput ucMat = new Local.GyHis.Material.ucMatOutput();
                    ucMat.Decimals = 2;
                    ucMat.MaxRowNo = 17;

                    ucMat.SetDataForInput(alOutput, 1, this.itemManager.Operator.ID, "1");
                }
              * */

            }

            this.Clear();
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColPurchasePrice].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColOutCost].CellType = numberCellType;
            this.outManager.FpSheetView.Columns[(int)ColumnSet.ColRetailPrice].CellType = numberCellType;
        }
        /// <summary>
        /// ��ӡ���ⵥ
        /// </summary>
        /// <param name="alOutput"></param>
        private void Print(List<Neusoft.HISFC.Models.Material.Output> alOutput)
        {
            if (this.outManager.IOutPrint != null)
            {
                this.outManager.IOutPrint.SetData(alOutput);
            }
        }


        public void SaveCheck(bool IsHeaderCheck)
        {

        }

        public int Print()
        {

            return 0;
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

        private void SetFpForeColor(int rowIndex)
        {
            //��������������ڿ������ʱ ������ɫ��� ����������
            if (Neusoft.FrameWork.Function.NConvert.ToDecimal(this.outManager.FpSheetView.Cells[rowIndex, (int)ColumnSet.ColOutQty].Text) >
                    Neusoft.FrameWork.Function.NConvert.ToDecimal(this.outManager.FpSheetView.Cells[rowIndex, (int)ColumnSet.ColStoreQty].Text))
            {
                this.outManager.FpSheetView.Rows[rowIndex].ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.outManager.FpSheetView.Rows[rowIndex].ForeColor = System.Drawing.Color.Black;
            }
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

        private void Fp_EditModeOff(object sender, EventArgs e)
        {
            if (this.outManager.FpSheetView.ActiveColumnIndex == (int)ColumnSet.ColOutQty)
            {
                DataRow dr = this.dt.Rows.Find(this.GetFindKey(this.outManager.FpSheetView, this.outManager.FpSheetView.ActiveRowIndex));
                if (dr != null)
                {
                    dr["������"] = NConvert.ToDecimal(dr["��������"]) * NConvert.ToDecimal(dr["�����"]);

                    dr.EndEdit();

                    this.CompuateSum();
                }
            }
            this.SetFpForeColor(this.outManager.FpSheetView.ActiveRowIndex);
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


        /// <summary>
        /// ������
        /// </summary>
        private enum ColumnSet
        {
            //<summary>
            //����
            //</summary>
            ColIsExam,
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
    }
}
