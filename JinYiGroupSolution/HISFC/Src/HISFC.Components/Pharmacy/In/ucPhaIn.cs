using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Management;
using System.Collections;

namespace Neusoft.HISFC.Components.Pharmacy.In
{
    /// <summary>
    /// [��������: ҩƷ������]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-12]<br></br>
    /// </summary>
    public partial class ucPhaIn : Neusoft.HISFC.Components.Common.Controls.ucIMAInOutBase,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer,
                                        Neusoft.FrameWork.WinForms.Classes.IPreArrange
    {
        public ucPhaIn()
        {
            InitializeComponent();

            this.Load += new EventHandler(ucPhaIn_Load);

            this.ucDrugList1.ChooseDataEvent += new Neusoft.HISFC.Components.Common.Controls.ucDrugList.ChooseDataHandler(ucDrugList1_ChooseDataEvent);
        }

        #region �����

        private IPhaInManager IManager = null;

        private System.Collections.Hashtable hsIManager = new Hashtable();

        /// <summary>
        /// Ȩ�޿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// �����ң��������ң��Ƿ�ҩ��
        /// </summary>
        private bool isStockArk = false;       

        /// <summary>
        /// Ŀ������Ƿ�ҩ��
        /// </summary>
        private bool isTargetArk = false;

        /// <summary>
        /// ����ʵ��
        /// </summary>
        private IFactory phaInFactory = null;

        /// <summary>
        /// ��ⵥ��ӡ�ӿڱ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint iInPrint = null;
        #endregion

        #region ����

        /// <summary>
        /// FpSheet
        /// </summary>
        [Browsable(false)]
        public FarPoint.Win.Spread.SheetView FpSheetView
        {
            get
            {
                return this.neuSpread1.Sheets[0];
            }
        }

        /// <summary>
        /// Fp
        /// </summary>
        [Browsable(false)]
        public Neusoft.FrameWork.WinForms.Controls.NeuSpread Fp
        {
            get
            {
                return this.neuSpread1;
            }
        }

        /// <summary>
        /// �����ң��������ң��Ƿ�ҩ��
        /// </summary>
        [Browsable(false)]
        public bool IsStockArk
        {
            get
            {
                return isStockArk;
            }
            set
            {
                isStockArk = value;
            }
        }

        /// <summary>
        /// Ŀ������Ƿ�ҩ��
        /// </summary>
        [Browsable(false)]
        public bool IsTargetArk
        {
            get
            {
                return isTargetArk;
            }
            set
            {
                isTargetArk = value;
            }
        }

        /// <summary>
        /// ��ⵥ��ӡ�ӿڱ���
        /// </summary>
        public Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint IInPrint
        {
            get
            {
                if (this.iInPrint == null)
                {
                    this.InitPrintInterface();
                }

                return this.iInPrint;
            }
        }

        /// <summary>
        /// �����ϲ�Panel�߶�    {1DED4697-A590-47b3-B727-92A4AA05D2ED
        /// </summary>
        public int TopPanelHeight
        {
            set
            {
                this.panelItemManager.Height = value;
            }
        }

        #endregion

        #region ��������ť

        protected override int OnSave(object sender, object neuObject)
        {
            this.neuSpread1.StopCellEditing();

            this.IManager.Save();

            this.neuSpread1.StartCellEditing(null, false);

            return 1;
        }

        protected override int OnPrint(object sender, object neuObject)
        {
            if (this.iInPrint == null)
            {
                this.InitPrintInterface();
            }

            this.IManager.Print();
            return 1;
        }

        public override void OnApplyList()
        {
            this.IManager.ShowApplyList();
        }

        public override void OnInList()
        {
            this.IManager.ShowInList();
        }

        public override void OnStockList()
        {
            this.IManager.ShowStockList();
        }

        public override void OnOutList()
        {
            this.IManager.ShowOutList();
        }

        public override void OnDelete()
        {
            if (this.neuSpread1_Sheet1.Rows.Count > 0)
            {
                this.IManager.Delete(this.neuSpread1_Sheet1, this.neuSpread1_Sheet1.ActiveRowIndex);
            }
        }

        public override void OnImport()
        {
            if (this.IManager != null)
            {
                this.IManager.ImportData();
            }
        }

        #endregion

        /// <summary>
        /// ��ʼ����ӡ����
        /// </summary>
        internal virtual void InitPrintInterface()
        {
            this.iInPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint)) as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint;

            if (this.iInPrint == null)
            {
                //object[] o = new object[] { };

                //try
                //{
                //    //��ⱨ��
                //    Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

                //    string billValue = ctrlIntegrate.GetControlParam<string>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.Pha_Input_Bill, true, "Report.Pharmacy.ucPhaInputBill");

                //    System.Runtime.Remoting.ObjectHandle objHande = System.Activator.CreateInstance("Report", billValue, false, System.Reflection.BindingFlags.CreateInstance, null, o, null, null, null);

                //    object oLabel = objHande.Unwrap();

                //    this.iInPrint = oLabel as Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint;
                //}
                //catch (System.TypeLoadException ex)
                //{
                //    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                //    MessageBox.Show(Language.Msg("�����ռ���Ч\n" + ex.Message));
                //    return;
                //}

                MessageBox.Show("δ������ⵥ��ӡ��ʵ�֣����޷�������ⵥ�ݴ�ӡ", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// ���ô�ѡ������
        /// </summary>
        /// <param name="dataType">������� 0 ҩƷ�б� 1 Ŀ�굥λ���ҿ���б� 2 �����ҿ���б� 3 �Զ����б�</param>
        /// <param name="sqlIndex">Sql���� ���Ϊ3ʱ�ò�����������</param>
        /// <param name="filterField">�����ֶ� ���Ϊ3ʱ�ò�����������</param>
        /// <param name="formatStr">Sql���� ���Ϊ3ʱ�ò�����������</param>
        /// <returns></returns>
        public int SetSelectData(string dataType,bool isBatch,string[] sqlIndex,string[] filterField,params string[] formatStr)
        {
            this.ucDrugList1.ShowFpRowHeader = false;

            switch (dataType)
            {
                case "0":
                    this.ucDrugList1.ShowPharmacyList();
                    break;
                case "1":
                    if (this.TargetDept.ID == "")
                    {
                        MessageBox.Show("��ѡ�񹩻���λ");
                        return -1;
                    }
                    this.ucDrugList1.ShowDeptStorage(this.TargetDept.ID, isBatch, 1);
                    break;
                case "2":
                    this.ucDrugList1.ShowDeptStorageAndDict(this.DeptInfo.ID, isBatch, true);
                    break;
                case "3":
                    this.ucDrugList1.ShowInfoList(sqlIndex, filterField, formatStr);
                    break;
            }

            return 1;
        }

        /// <summary>
        /// ���ô�ѡ��������ʾ
        /// </summary>
        /// <param name="label"></param>
        /// <param name="width"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public void SetSelectFormat(string[] label, int[] width, bool[] visible)
        {
            this.ucDrugList1.SetFormat(label, width, visible);
        }

        /// <summary>
        /// ��ȡ��ʾ���ݵĵ�һ�е�ָ���п��
        /// </summary>
        /// <param name="columnNum">������������</param>
        /// <param name="width">���صĿ��</param>
        protected void GetColumnWidth(int iColumn, ref int iWidth)
        {
            this.ucDrugList1.GetColumnWidth(iColumn, ref iWidth);
        }

        /// <summary>
        /// �����б����ݿ�� ��ʾָ����
        /// </summary>
        /// <param name="showColumnCount">��ʾָ���и��� ������������</param>
        public void SetItemListWidth(int showColumnCount)
        {
            int iWidth = this.panelItemSelect.Width;

            this.ucDrugList1.GetColumnWidth(showColumnCount, ref iWidth);

            this.panelItemSelect.Width = iWidth + 30;
        }

        /// <summary>
        /// ���� 
        /// </summary>
        /// <param name="filterData"></param>
        protected override void Filter(string filterData)
        {
            this.IManager.Filter(filterData);
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            Neusoft.FrameWork.Management.DataBaseManger dataManager = new DataBaseManger();

            Neusoft.FrameWork.Models.NeuObject class2Priv = new Neusoft.FrameWork.Models.NeuObject();
            class2Priv.ID = "0310";
            class2Priv.Name = "���";
            this.Class2Priv = class2Priv;       //���
            
            //��Ȩ�޿��һ�ȡ
            //this.DeptInfo = ((Neusoft.HISFC.Models.Base.Employee)dataManager.Operator).Dept;
            this.OperInfo = dataManager.Operator;

            Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.HISFC.Models.Base.Department dept = managerIntegrate.GetDepartment(this.DeptInfo.ID);
            if (dept != null)
                this.DeptInfo.Memo = dept.DeptType.ID.ToString();

            if (this.FilePath == "")
            {
                this.FilePath = @"\Setting\PhaInSetting.xml";
            }

            if (this.SetPrivType(true) == -1)
            {
                return;
            }

            this.GetInterface(); 
        }

        protected override void FilterPriv(ref List<Neusoft.FrameWork.Models.NeuObject> privList)
        {        
            for (int i = privList.Count - 1; i >= 0; i--)
            {
                Neusoft.FrameWork.Models.NeuObject priv = privList[i] as Neusoft.FrameWork.Models.NeuObject;
             
                //ҩ�� ����һ����⡢������⡢��Ʊ���
                if (this.DeptInfo.Memo == "P")
                {
                    //if (priv.Memo == "11" || priv.Memo == "1C" || priv.Memo == "1A")
                    //{
                    //    privList.Remove(priv);
                    //}
                    if (priv.Memo == "11" || priv.Memo == "1A")
                    {
                        privList.Remove(priv);
                    }
                }
                //ҩ�� �����ڲ�������롢�ڲ�����˿�����
                if (this.DeptInfo.Memo == "PI")
                {
                    if (priv.Memo == "13" || priv.Memo == "18")
                    {
                        privList.Remove(priv);
                    }
                }
            }
        }

        /// <summary>
        /// ��ʼ��Fp��Ϣ
        /// </summary>
        private void InitFp()
        { 
            FarPoint.Win.Spread.InputMap im;

            im = this.neuSpread1.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);
        }

        /// <summary>
        /// ���ó�ʼ����
        /// </summary>
        public void SetFocus()
        {
            if (this.IsShowItemSelectpanel)
            {
                this.ucDrugList1.SetFocusSelect();
            }
            else
            {
                if (this.neuSpread1_Sheet1.Rows.Count > 0)
                {
                    this.IManager.SetFocusSelect();
                }
            }
        }

        /// <summary>
        /// ����Fp
        /// </summary>
        public void SetFpFocus()
        {
            this.neuSpread1.Select();
        }

        protected override void Clear()
        {
            base.Clear();

            this.ucDrugList1.Clear();

            this.FpSheetView.Reset();

            this.FpSheetView.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.FpSheetView.ActiveSkin = new FarPoint.Win.Spread.SheetSkin("CustomSkin3", System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.LightGray, FarPoint.Win.Spread.GridLines.Both, System.Drawing.Color.White, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, System.Drawing.Color.Empty, false, false, false, true, true);

            this.Fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.Fp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            this.InitFp();
        }

        /// <summary>
        /// ���ýӿ�ʵ��
        /// </summary>
        private void GetInterface()
        {        
            this.Clear();

            //ͨ�����䷽ʽ��ȡFactory�ļ� ���� ���Խ� Factory��������ص������ļ�ȫ��Ų��UFC ʵ���Զ���            
            if (this.phaInFactory == null)
            {
                this.phaInFactory = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject( this.GetType(), typeof( Neusoft.HISFC.Components.Pharmacy.IFactory ) ) as Neusoft.HISFC.Components.Pharmacy.IFactory;
            }

            if (this.phaInFactory == null)
            {
                this.phaInFactory = new PhaFactory() as IFactory;
            }

            //{9E282C1A-071F-4833-8AE3-EC64CA71FD8F} ���Ӷ���Դ�ͷź����ĵ���
            if (this.IManager != null)
            {
                this.IManager.Dispose();
            }

            this.IManager = this.phaInFactory.GetInInstance(this.PrivType, this);

            if (this.IManager == null)
            {
                System.Windows.Forms.MessageBox.Show("�����������ȡ��Ӧ�ӿ�ʵ��ʧ��");
                return;
            }

            this.neuSpread1_Sheet1.DataSource = null;
            //Ϊ��ʵ�ֹ��� ��ֵDefaultView
            DataTable dtTemp = this.IManager.InitDataTable();
            if (dtTemp != null)
                this.neuSpread1_Sheet1.DataSource = dtTemp.DefaultView;
            else
                this.neuSpread1_Sheet1.DataSource = dtTemp;

            this.neuPanel1.SuspendLayout();
            this.neuPanel3.SuspendLayout();
            this.neuPanel4.SuspendLayout();
            this.panelItemSelect.SuspendLayout();
            this.SuspendLayout();         

            this.AddItemInputUC(this.IManager.InputModualUC);

            this.neuPanel1.ResumeLayout();
            this.neuPanel3.ResumeLayout();
            this.neuPanel4.ResumeLayout();
            this.panelItemSelect.ResumeLayout();
            this.ResumeLayout();
        }


        private void ucPhaIn_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper() != "DEVENV")
            {
                //Neusoft.FrameWork.Models.NeuObject testPrivDept = new Neusoft.FrameWork.Models.NeuObject();
                //int parma = Neusoft.HISFC.Components.Common.Classes.Function.ChoosePivDept("0310", ref testPrivDept);

                //if (parma == -1)            //��Ȩ��
                //{
                //    MessageBox.Show(Language.Msg("���޴˴��ڲ���Ȩ��"));
                //    return;
                //}
                //else if (parma == 0)       //�û�ѡ��ȡ��
                //{
                //    return;
                //}

                //this.DeptInfo = testPrivDept;

                //base.OnStatusBarInfo(null, "�������ң� " + testPrivDept.Name);

                //��Ҫ�ڴ˴����� ��������                

                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڽ������ݳ�ʼ��...���Ժ�");
                Application.DoEvents();

                this.Init();

                this.InitFp();

                if (this.IManager != null)
                {
                    this.IManager.SetFocusSelect();
                }

                this.chkMinUnit.Visible = false;

                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
            return;
        }

        protected override void OnEndPrivChanged(Neusoft.FrameWork.Models.NeuObject changeData, object param)
        {
            if (this.PrivType.Memo == Neusoft.HISFC.Models.Base.EnumIMAInTypeService.GetNameFromEnum(Neusoft.HISFC.Models.Base.EnumIMAInType.BorrowApply) ||
               this.PrivType.Memo == Neusoft.HISFC.Models.Base.EnumIMAInTypeService.GetNameFromEnum(Neusoft.HISFC.Models.Base.EnumIMAInType.BorrowBack) ||
               this.PrivType.Memo == Neusoft.HISFC.Models.Base.EnumIMAInTypeService.GetNameFromEnum(Neusoft.HISFC.Models.Base.EnumIMAInType.ProduceInput))
            {
                MessageBox.Show(this.PrivType.Name + " ΪԤ������ Ŀǰ���ṩ����ҵ��ʵ��", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.PrivType = null;
                this.IManager = null;
                return;
            }

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڸ�����������ؽ��� ���Ժ�..");
            Application.DoEvents();

            this.GetInterface();

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            base.OnEndPrivChanged(changeData, param);
        }

        protected override void OnEndTargetChanged(Neusoft.FrameWork.Models.NeuObject changeData, object param)
        {
            if (this.IManager != null)
            {
                this.IManager.SetFocusSelect();
            }

            base.OnEndTargetChanged(changeData, param);
        }

        private void ucDrugList1_ChooseDataEvent(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            if (sv != null && activeRow >= 0)
            {
                if (this.IManager != null)
                {
                    if (this.IManager.AddItem(sv, activeRow) == -1)
                        this.ucDrugList1.SetFocusSelect();
                }
            }
        }

        #region IExtendInterfaceContainer ��Ա

        public object[] InterfaceObjects
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { 
                    typeof(Neusoft.HISFC.Components.Pharmacy.IFactory),
                    typeof(Neusoft.HISFC.BizProcess.Interface.Pharmacy.IBillPrint)
                };
            }
        }

        #endregion

        #region IPreArrange ��Ա

        bool isPreArrange = false;

        public int PreArrange()
        {
            this.isPreArrange = true;

            Neusoft.FrameWork.Models.NeuObject testPrivDept = new Neusoft.FrameWork.Models.NeuObject();
            int parma = Neusoft.HISFC.Components.Common.Classes.Function.ChoosePivDept("0310", ref testPrivDept);

            if (parma == -1)            //��Ȩ��
            {
                MessageBox.Show(Language.Msg("���޴˴��ڲ���Ȩ��"));
                return -1;
            }
            else if (parma == 0)       //�û�ѡ��ȡ��
            {
                return -1;
            }

            this.DeptInfo = testPrivDept;

            base.OnStatusBarInfo(null, "�������ң� " + testPrivDept.Name);

            return 1;
        }

        #endregion

    }
}
