using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.Preparation.Disinfect
{
    /// <summary>
    /// ��Ʒ����ά����ԭ����
    /// </summary>
    public partial class ucDisinfectMaterial : Neusoft.FrameWork.WinForms.Controls.ucBaseControl, Prescription.IPrescriptionMaterial
    {
        public ucDisinfectMaterial()
        {
            InitializeComponent();
        }

        #region ����

        /// <summary>
        /// �Ƽ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.Preparation preparationManager = new Neusoft.HISFC.BizLogic.Pharmacy.Preparation();

        /// <summary>
        /// ҩƷ������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Fee feeManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        /// <summary>
        /// ��ǰ��ά���õ����ƴ���
        /// </summary>
        private System.Collections.Hashtable hsPrescription = new Hashtable();

        /// <summary>
        /// ��ǰ�����ĳ�Ʒ����
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject operProduct = new Neusoft.FrameWork.Models.NeuObject();

        /// <summary>
        /// ��Ŀ���
        /// </summary>
        private Neusoft.HISFC.Models.Base.EnumItemType itemType = Neusoft.HISFC.Models.Base.EnumItemType.UnDrug;
        #endregion

        #region ��ʼ��

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            FarPoint.Win.Spread.InputMap im;
            im = this.fsMaterial.GetInputMap(FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused);
            im.Put(new FarPoint.Win.Spread.Keystroke(Keys.Enter, Keys.None), FarPoint.Win.Spread.SpreadActions.None);

            Neusoft.FrameWork.WinForms.Classes.MarkCellType.NumCellType markNumCell = new Neusoft.FrameWork.WinForms.Classes.MarkCellType.NumCellType();
            this.fsMaterial_Sheet1.Columns[(int)MaterialColumnSet.ColQty].CellType = markNumCell;

            List<Neusoft.HISFC.Models.Fee.Item.Undrug> undrugList = this.feeManager.QueryAllItemsList();
            if (undrugList == null)
            {
                MessageBox.Show("��ȡ��ҩƷ��Ŀ�б�������" + feeManager.Err);
                return;
            }

            this.fsMaterial.SetColumnList(this.fsMaterial_Sheet1, new ArrayList(undrugList.ToArray()), 1);
        }

        #endregion

        /// <summary>
        /// ��Ӵ�����ϸ��Ϣ
        /// </summary>
        /// <param name="item"></param>
        public int AddItemDetail(Neusoft.HISFC.Models.Fee.Item.Undrug item)
        {
            int i = this.fsMaterial_Sheet1.ActiveRowIndex;

            this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialID].Text = item.ID;
            this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialName].Text = item.Name;
            this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialSpecs].Text = item.Specs;
            this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColPrice].Text = item.Price.ToString();
            this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColUnit].Text = item.PriceUnit;

            this.fsMaterial_Sheet1.Rows[i].Tag = item;

            return 1;
        }

        private void fsMaterial_SelectItem(object sender, EventArgs e)
        {
            if (this.AddItemDetail(sender as Neusoft.HISFC.Models.Fee.Item.Undrug) == 1)
            {
                this.fsMaterial_Sheet1.ActiveColumnIndex = (int)MaterialColumnSet.ColQty;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (this.fsMaterial.ContainsFocus)
            {
                if (keyData == Keys.Enter)
                {
                    if (this.fsMaterial_Sheet1.ActiveColumnIndex == (int)MaterialColumnSet.ColQty)
                    {
                        this.fsMaterial_Sheet1.ActiveColumnIndex = (int)MaterialColumnSet.ColMemo;
                        return base.ProcessDialogKey(keyData);
                    }
                    if (this.fsMaterial_Sheet1.ActiveColumnIndex == (int)MaterialColumnSet.ColMemo)
                    {
                        this.fsMaterial_Sheet1.Rows.Add(this.fsMaterial_Sheet1.Rows.Count, 1);
                        this.fsMaterial_Sheet1.ActiveRowIndex = this.fsMaterial_Sheet1.Rows.Count;
                        this.fsMaterial_Sheet1.ActiveColumnIndex = (int)MaterialColumnSet.ColMaterialName;
                    }
                }
            }

            return base.ProcessDialogKey(keyData);
        }

        #region IPrescriptionMaterial ��Ա

        /// <summary>
        /// ���ԭ�ϡ�������ϸ
        /// </summary>
        /// <returns>�ɹ�����1 ʧ�ܷ��أ�1</returns>
        public int AddMaterial()
        {
            int rowCount = this.fsMaterial_Sheet1.Rows.Count;

            this.fsMaterial_Sheet1.Rows.Add(rowCount, 1);

            return 1;
        }

        public int ShowMaterial(Neusoft.FrameWork.Models.NeuObject product)
        {
            this.fsMaterial_Sheet1.Rows.Count = 0;

            List<Neusoft.HISFC.Models.Preparation.PrescriptionBase> al = this.preparationManager.QueryPrescription(product.ID, this.itemType, Neusoft.HISFC.Models.Preparation.EnumMaterialType.Material);
            if (al == null)
            {
                MessageBox.Show(Language.Msg("��ȡ��ǰѡ���Ʒ�����ƴ�����Ϣ����\n" + product.ID));
                return -1;
            }
            foreach (Neusoft.HISFC.Models.Preparation.PrescriptionBase info in al)
            {
                int i = this.fsMaterial_Sheet1.Rows.Count;

                this.fsMaterial_Sheet1.Rows.Add(i, 1);

                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialID].Text = info.Material.ID;
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialName].Text = info.Material.Name;
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialSpecs].Text = info.Specs;
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColPrice].Text = info.Price.ToString();
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColQty].Text = info.NormativeQty.ToString();
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMemo].Text = info.Memo;
                this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColUnit].Text = info.NormativeUnit;

                this.fsMaterial_Sheet1.Rows[i].Tag = info.Material;
            }

            this.operProduct = product.Clone();

            return 1;
        }

        public int DeleteMaterial()
        {
            DialogResult rs = MessageBox.Show(Language.Msg("ɾ����ǰѡ��ĳ�Ʒ���ƴ�����Ϣ��"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (rs == DialogResult.No)
            {
                return 1;
            }

            if (this.fsMaterial_Sheet1.Rows.Count <= 0)
            {
                return 1;
            }
            int iIndex = this.fsMaterial_Sheet1.ActiveRowIndex;
            Neusoft.FrameWork.Models.NeuObject material = this.fsMaterial_Sheet1.Rows[iIndex].Tag as Neusoft.FrameWork.Models.NeuObject;
            if (material == null)
            {
                return 1;
            }
            if (this.preparationManager.DelPrescription(this.operProduct.ID, this.itemType, material.ID) == -1)
            {
                MessageBox.Show("�Ե�ǰѡ�񴦷���¼����ɾ������ʧ��\n" + this.preparationManager.Err);
                return -1;
            }

            this.fsMaterial_Sheet1.Rows.Remove(iIndex, 1);

            return 1;
        }

        public int Clear()
        {
            this.fsMaterial_Sheet1.Rows.Count = 0;

            return 1;
        }

        public List<Neusoft.HISFC.Models.Preparation.PrescriptionBase> GetMaterial()
        {
            Neusoft.HISFC.Models.Preparation.DisinfectPrescription info = null;

            List<Neusoft.HISFC.Models.Preparation.PrescriptionBase> prescriptionList = new List<Neusoft.HISFC.Models.Preparation.PrescriptionBase>();
            DateTime sysTime = this.preparationManager.GetDateTimeFromSysDateTime();
            Neusoft.HISFC.Models.Fee.Item.Undrug unDrug = this.feeManager.GetItem(this.operProduct.ID);
            if (unDrug == null)
            {
                MessageBox.Show(this.feeManager.Err);
                return null;
            }

            for (int i = 0; i < this.fsMaterial_Sheet1.Rows.Count; i++)
            {
                if (this.fsMaterial_Sheet1.Cells[i, 0].Text == "")
                {
                    continue;
                }

                info = new Neusoft.HISFC.Models.Preparation.DisinfectPrescription();

                info.Material = this.fsMaterial_Sheet1.Rows[i].Tag as Neusoft.FrameWork.Models.NeuObject;
                if (info.Material == null)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("����ת������");
                    return null;
                }

                info.Drug = unDrug;

                info.Specs = this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMaterialSpecs].Text;
                info.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColPrice].Text);
                info.NormativeUnit = this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColUnit].Text;

                info.MaterialType = Neusoft.HISFC.Models.Preparation.EnumMaterialType.Material;
                info.NormativeQty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColQty].Text);
                info.Memo = this.fsMaterial_Sheet1.Cells[i, (int)MaterialColumnSet.ColMemo].Text;
                info.OperEnv.ID = this.preparationManager.Operator.ID;
                info.OperEnv.OperTime = sysTime;

                prescriptionList.Add(info);
            }

            return prescriptionList;

        }

        public Neusoft.FrameWork.WinForms.Controls.ucBaseControl ProductControl
        {
            get
            {
                return this;
            }
        }

        public Neusoft.HISFC.Models.Base.EnumItemType ItemType
        {
            set
            {
                this.itemType = value;
            }
        }

        #endregion

        #region ö��

        protected enum MaterialColumnSet
        {
            /// <summary>
            /// ԭ�ϱ���
            /// </summary>
            ColMaterialID,
            /// <summary>
            /// ����
            /// </summary>
            ColMaterialName,
            /// <summary>
            /// ���
            /// </summary>
            ColMaterialSpecs,
            /// <summary>
            /// �۸�
            /// </summary>
            ColPrice,
            /// <summary>
            /// ������
            /// </summary>
            ColQty,
            /// <summary>
            /// ��λ
            /// </summary>
            ColUnit,
            /// <summary>
            /// ��ע
            /// </summary>
            ColMemo
        }

        #endregion
    }
}
