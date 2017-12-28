using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;
using Neusoft.HISFC.Components.Common.Controls;

namespace Neusoft.HISFC.Components.Material.Out
{
    public partial class ucMatOut : Neusoft.HISFC.Components.Common.Controls.ucIMAInOutBase, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer, Neusoft.FrameWork.WinForms.Classes.IPreArrange
    {
        public ucMatOut()
        {
            InitializeComponent();

            this.Load += new EventHandler(ucMatOut_Load);
            this.ucMaterialItemList1.ChooseDataEvent += new Material.Base.ucMaterialItemList.ChooseDataHandler(ucMaterialItemList1_ChooseDataEvent);

        }

        #region �����

        public IMatManager IManager = null;

        private System.Collections.Hashtable hsIManager = new Hashtable();

        /// <summary>
        /// Ȩ�޿���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// �����ӿ�ʵ��
        /// </summary>
        private Neusoft.HISFC.Components.Material.IFactory matFactory = null;

        /// <summary>
        /// ���ʳ��ⵥ��ӡ����
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.Material.IBillPrint iOutPrint = null;

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
        public FarPoint.Win.Spread.FpSpread Fp
        {
            get
            {
                return this.neuSpread1;
            }
        }

        /// <summary>
        /// ���ʳ��ⵥ��ӡ����
        /// </summary>
        public Neusoft.HISFC.BizProcess.Interface.Material.IBillPrint IOutPrint
        {
            get
            {
                if (this.iOutPrint == null)
                {
                    this.InitPrintInterface();
                }
                return this.iOutPrint;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ����ӡ����
        /// </summary>
        internal virtual void InitPrintInterface()
        {
            this.iOutPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Material.IBillPrint)) as Neusoft.HISFC.BizProcess.Interface.Material.IBillPrint;
            if(this.iOutPrint == null)
            {
                MessageBox.Show("�����ô�ӡ�ӿ�!");
                return;
            }
        }

        /// <summary>
        /// ���ô�ѡ������
        /// </summary>
        /// <param name="dataType">������� 0 ��Ʒ�б� 1 Ŀ�굥λ���ҿ���б� 2 �����ҿ���б� 3 �Զ����б�</param>
        /// <param name="isPrice">�Ƿ񰴲�ͬ�����������ʾ</param>
        /// <param name="sqlIndex">Sql���� ���Ϊ3ʱ�ò�����������</param>
        /// <param name="filterField">�����ֶ� ���Ϊ3ʱ�ò�����������</param>
        /// <param name="formatStr">Sql���� ���Ϊ3ʱ�ò�����������</param>
        /// <returns></returns>
        public int SetSelectData(string dataType, bool isPrice, string[] sqlIndex, string[] filterField, params string[] formatStr)
        {
            this.ucMaterialItemList1.ShowFpRowHeader = false;

            switch (dataType)
            {
                case "0":
                    //{AFE629CC-8493-4344-9792-8611C0BFA1BD} 
                    this.ucMaterialItemList1.ShowMaterialList(this.privDept.ID);
                    break;
                case "1":
                    if (this.TargetDept.ID == "")
                    {
                        MessageBox.Show("��ѡ�񹩻���λ");
                        return -1;
                    }
                    this.ucMaterialItemList1.ShowDeptStorage(this.TargetDept.ID, isPrice);
                    break;
                case "2":
                    this.ucMaterialItemList1.ShowDeptStorage(this.DeptInfo.ID, isPrice);
                    break;
                case "3":
                    this.ucMaterialItemList1.ShowInfoList(sqlIndex, filterField, formatStr);
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
            this.ucMaterialItemList1.SetFormat(label, width, visible);
        }

        /// <summary>
        /// ��ȡ��ʾ���ݵĵ�һ�е�ָ���п��
        /// </summary>
        /// <param name="columnNum">������������</param>
        /// <param name="width">���صĿ��</param>
        protected void GetColumnWidth(int iColumn, ref int iWidth)
        {
            this.ucMaterialItemList1.GetColumnWidth(iColumn, ref iWidth);
        }

        /// <summary>
        /// �����б����ݿ�� ��ʾָ����
        /// </summary>
        /// <param name="showColumnCount">��ʾָ���и��� ������������</param>
        public void SetItemListWidth(int showColumnCount)
        {
            int iWidth = this.panelItemSelect.Width;

            this.ucMaterialItemList1.GetColumnWidth(showColumnCount, ref iWidth);

            this.panelItemSelect.Width = iWidth + 5;
        }

        /// <summary>
        /// ���� 
        /// </summary>
        /// <param name="filterData"></param>
        protected override void Filter(string filterData)
        {
            if (this.IManager != null)
            {
                this.IManager.Filter(filterData);
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            Neusoft.FrameWork.Management.DataBaseManger dataManager = new DataBaseManger();

            Neusoft.FrameWork.Models.NeuObject class2Priv = new Neusoft.FrameWork.Models.NeuObject();
            class2Priv.ID = "0520";
            class2Priv.Name = "����";
            this.Class2Priv = class2Priv;       //����

            //��Ȩ�޿��һ�ȡ
            //this.DeptInfo = ((Neusoft.HISFC.Models.Base.Employee)dataManager.Operator).Dept;
            this.OperInfo = dataManager.Operator;
            this.OperInfo.Memo = "out";

            Neusoft.HISFC.BizLogic.Manager.Department managerIntegrate = new Neusoft.HISFC.BizLogic.Manager.Department();
            Neusoft.HISFC.Models.Base.Department dept = managerIntegrate.GetDeptmentById(this.DeptInfo.ID);
            if (dept != null)
                this.DeptInfo.Memo = dept.DeptType.ID.ToString();

            if (this.FilePath == "")
            {
                this.FilePath = @"\Setting\PhaOutSetting.xml";
            }

            if (this.SetPrivType(true) == -1)
            {
                return;
            }
            this.SetCancelVisible(false);
            this.GetInterface();
        }

        private void SetCancelVisible(bool p)
        {

        }

        protected override void FilterPriv(ref List<Neusoft.FrameWork.Models.NeuObject> privList)
        {

        }

        /// <summary>
        /// ��ʼ��Fp��Ϣ
        /// </summary>
        public void InitFp()
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
                this.ucMaterialItemList1.SetFocusSelect();
            }
            else
            {
                if (this.FpSheetView.Rows.Count > 0)
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

            this.ucMaterialItemList1.Clear();

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

            //{9E7FB328-89B3-4f43-A417-2EC3ACFC7093}
            //���ͷŵ��¼���Դ
            if (this.IManager != null)
            {
                this.IManager.Dispose();
            }

            if (this.matFactory == null)
            {
                this.matFactory = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.Components.Material.IFactory)) as Neusoft.HISFC.Components.Material.IFactory;
            }

            if (this.matFactory == null)
            {
                MatFactory factory = new MatFactory();
                this.matFactory = factory as Neusoft.HISFC.Components.Material.IFactory;
            }

            this.IManager = this.matFactory.GetOutInstance(this.PrivType, this);

            if (this.IManager == null)
            {
                System.Windows.Forms.MessageBox.Show("���ݳ�������ȡ��Ӧ�ӿ�ʵ��ʧ��");
                return;
            }

            this.neuSpread1_Sheet1.DataAutoSizeColumns = false;

            this.neuSpread1_Sheet1.DataSource = null;
            //Ϊ��ʵ�ֹ��� ��ֵDefaultView
            DataTable dtTemp = this.IManager.InitDataTable();
            if (dtTemp != null)
                this.neuSpread1_Sheet1.DataSource = dtTemp.DefaultView;
            else
                this.neuSpread1_Sheet1.DataSource = dtTemp;

            this.IManager.SetFormat();				//��ʽ��

            this.IManager.SetFocusSelect();			//��������

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
        #endregion

        #region ������

        /*
        protected override int OnSave()
        {
            if (this.IManager != null)
            {
                this.IManager.Save();
            }

            return base.OnSave();
        }


        protected override int OnShowApplyList()
        {
            if (this.IManager != null)
            {
                this.IManager.ShowApplyList(false);
            }

            try
            {
                this.lbInfo.Text = (this.IManager as ExamOutPriv).showInfo;
            }
            catch
            { }
            return base.OnShowApplyList();
        }


        protected override int OnShowInList()
        {
            if (this.IManager != null)
            {
                this.IManager.ShowInList();
            }

            return base.OnShowInList();
        }


        protected override int OnShowOutList()
        {
            if (this.IManager != null)
            {
                this.IManager.ShowOutList();
            }

            return base.OnShowOutList();
        }


        protected override int OnDel()
        {
            if (this.IManager != null)
            {
                if (this.neuSpread1_Sheet1.Rows.Count > 0)
                {
                    this.IManager.Delete(this.neuSpread1_Sheet1, this.neuSpread1_Sheet1.ActiveRowIndex);
                }
            }

            return base.OnDel();
        }
        **/
        protected override int OnSave(object sender, object neuObject)
        {
            this.neuSpread1.StopCellEditing();

            this.IManager.Save();

            this.neuSpread1.StartCellEditing(null, false);

            return 1;
        }

        protected override int OnPrint(object sender, object neuObject)
        {
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
                this.IManager.Delete(this.neuSpread1_Sheet1, this.neuSpread1_Sheet1.ActiveRowIndex);
        }

        public override void OnImport()
        {
            if (this.IManager != null)
            {
                this.IManager.ImportData();
            }
        }

        #endregion

        #region �¼�

        private void ucMatOut_Load(object sender, EventArgs e)
        {
            if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper() != "DEVENV")
            {
                this.ucMaterialItemList1.ShowAdvanceFilter = false;

                //Neusoft.FrameWork.Models.NeuObject testPrivDept = new Neusoft.FrameWork.Models.NeuObject();
                //int parma = Neusoft.HISFC.Components.Common.Classes.Function.ChoosePivDept("0520", ref testPrivDept);

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

                this.ucMaterialItemList1.SetKind();

                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }

            return;
        }

        protected override void OnEndPrivChanged(Neusoft.FrameWork.Models.NeuObject changeData, object param)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڸ��ݳ��������ؽ��� ���Ժ�..");
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

        private void ucMaterialItemList1_ChooseDataEvent(FarPoint.Win.Spread.SheetView sv, int activeRow)
        {
            if (sv != null && activeRow >= 0)
            {
                if (this.IManager != null)
                {
                    if (this.IManager.AddItem(sv, activeRow) == -1)
                    {
                        this.ucMaterialItemList1.SetFocusSelect();
                    }
                }
            }
        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { 
                    typeof(Neusoft.HISFC.Components.Material.IFactory),
                    typeof(Neusoft.HISFC.BizProcess.Interface.Material.IBillPrint)
                };
            }
        }

        #endregion

        #region IPreArrange ��Ա

        public int PreArrange()
        {
            Neusoft.FrameWork.Models.NeuObject testPrivDept = new Neusoft.FrameWork.Models.NeuObject();
            int parma = Neusoft.HISFC.Components.Common.Classes.Function.ChoosePivDept("0520", ref testPrivDept);

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
