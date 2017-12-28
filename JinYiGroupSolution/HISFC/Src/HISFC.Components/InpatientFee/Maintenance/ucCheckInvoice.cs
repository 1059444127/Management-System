using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Models;
namespace Neusoft.HISFC.Components.InpatientFee.Maintenance
{
    public partial class ucCheckInvoice : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucCheckInvoice()
        {
            InitializeComponent();
        }

        #region ����
        /// <summary>
        /// ��Ʊҵ���
        /// </summary>
        //{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
        //protected Neusoft.HISFC.BizLogic.Fee.InvoiceService invoiceServiceManager = new Neusoft.HISFC.BizLogic.Fee.InvoiceService();
        protected Neusoft.HISFC.BizLogic.Fee.InvoiceServiceNoEnum invoiceServiceManager = new Neusoft.HISFC.BizLogic.Fee.InvoiceServiceNoEnum();
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();
        /// <summary>
        /// ��Ʊ����
        /// </summary>
        /// 
        //{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
        //Neusoft.HISFC.Models.Fee.EnumInvoiceType enumType ;
        private string enumType = string.Empty;
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        private string begin = string.Empty;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        private string end = string.Empty;
        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager managerInteger = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        /// <summary>
        /// �տ�ԱID
        /// </summary>
        string casher = string.Empty;
        #endregion

        #region ������
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("ȫѡ", "��ȫ����������Ϊѡ��״̬", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȫѡ, true, false, null);
            this.toolBarService.AddToolButton("ȡ��", "��ȫ����������Ϊδѡ��״̬", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȡ��, true, false, null);
            return toolBarService;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "ȫѡ":
                    {
                        IsSelectAll(true);
                        break;
                    }
                case "ȡ��":
                    {
                        IsSelectAll(false);
                        break;
                    }
            }
            base.ToolStrip_ItemClicked(sender, e);
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.Save();
            return base.OnSave(sender, neuObject);
        }

        #endregion

        #region ����
        /// <summary>
        /// �Ƿ�Farpoint��������Ϊѡ��״̬
        /// </summary>
        /// <param name="bl">true :ѡ�� false:δѡ��</param>
        protected virtual void IsSelectAll(bool bl)
        {
            int count = this.neuSpread1_Sheet1.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    this.neuSpread1_Sheet1.Cells[i, 0].Text = bl.ToString();
                }
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            InitTree();
            //��ʼ����Ա��Ϣ
            ArrayList al = managerInteger.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.F);
            if (al != null)
            {
                NeuObject obj = new NeuObject();
                obj.ID = "ALL";
                obj.Name = "ȫ��";
                al.Insert(0, obj);
                this.cbsky.AddItems(al);
                this.cbsky.Tag = "ALL";
            }
            
        }

        /// <summary>
        /// ��ʼ��TreeView
        /// </summary>
        private void InitTree()
        {
            if (tree.Nodes.Count > 0)
            {
                tree.Nodes.Clear();
            }
            TreeNode root = new TreeNode();
            root.Text = "��Ʊ����";
            root.ImageIndex = 2;
            tree.Nodes.Add(root);
            //{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
            //ArrayList al = Neusoft.HISFC.Models.Fee.InvoiceTypeEnumService.List();
            Neusoft.HISFC.BizLogic.Manager.Constant myCont = new Neusoft.HISFC.BizLogic.Manager.Constant ();
            ArrayList al = myCont.GetList("GetInvoiceType");
            if (al == null || al.Count == 0)
            {
                return;
            }
            TreeNode node ;
            foreach (NeuObject obj in al)
            {
                //{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
                //if (obj.ID == Neusoft.HISFC.Models.Fee.EnumInvoiceType.C.ToString() || obj.ID == Neusoft.HISFC.Models.Fee.EnumInvoiceType.I.ToString())
                if (obj.ID == "C" || obj.ID == "I")
                {
                    node = new TreeNode();
                    node.Text = obj.Name;
                    node.Tag = obj;
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                    root.Nodes.Add(node);
                }
            }
            this.tree.ExpandAll();

        }

        /// <summary>
        /// ��ȡ���﷢Ʊ����
        /// </summary>
        private void GetOutpatientFeeInvoice(string begin,string end,string casher)
        {
            
            List<NeuObject> list = new List<NeuObject>();
            if (invoiceServiceManager.GetOutpatientFeeInvoice(begin, end,casher, ref list) == -1)
            {
                MessageBox.Show(invoiceServiceManager.Err);
                return;
            }
            int count = list.Count;
            this.neuSpread1_Sheet1.Rows.Count = count;
            for (int i = 0; i < count; i++)
            {
                this.neuSpread1_Sheet1.Cells[i, 0].Text = "true";
                this.neuSpread1_Sheet1.Cells[i, 1].Text = list[i].ID;
                this.neuSpread1_Sheet1.Cells[i, 2].Text = list[i].Name;
                this.neuSpread1_Sheet1.Cells[i, 3].Text = list[i].Memo;
                this.neuSpread1_Sheet1.Rows[i].Tag = list[i];
            }
        }

        /// <summary>
        /// ��ȡסԺ��Ʊ����
        /// </summary>
        private void GetInpatientFeeInvoice(string begin, string end, string casher)
        {
            List<NeuObject> list = new List<NeuObject>();
            if (invoiceServiceManager.GetInpatientFeeInvoice(begin, end,casher, ref list) == -1)
            {
                MessageBox.Show(invoiceServiceManager.Err);
                return;
            }
            int count = list.Count;
            this.neuSpread1_Sheet1.Rows.Count = count;
            for (int i = 0; i < count; i++)
            {
                this.neuSpread1_Sheet1.Cells[i, 0].Text = "true";
                this.neuSpread1_Sheet1.Cells[i, 1].Text = list[i].ID;
                this.neuSpread1_Sheet1.Cells[i, 2].Text = list[i].Name;
                this.neuSpread1_Sheet1.Cells[i, 3].Text = list[i].Memo;
                this.neuSpread1_Sheet1.Rows[i].Tag = list[i];
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        protected virtual void Save()
        {
            if (this.tree.SelectedNode.Tag == null) return;
            int count=this.neuSpread1_Sheet1.Rows.Count;
            if (count == 0) return;
            //����Ա
            NeuObject oper = invoiceServiceManager.Operator;
            //����ʱ��
            string operTime = invoiceServiceManager.GetDateTimeFromSysDateTime().ToString();
            int resultValue = 0;
            //��ʼ����
            //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            this.invoiceServiceManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            try
            {
                for (int i = 0; i < count; i++)
                {
                    if (this.neuSpread1_Sheet1.Cells[i, 0].Text.ToLower() == "false") continue;
                    NeuObject obj = this.neuSpread1_Sheet1.Rows[i].Tag as NeuObject;
                    //���﷢Ʊ����{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
                    //if (enumType == Neusoft.HISFC.Models.Fee.EnumInvoiceType.C)
                    if (enumType == "C")
                    {
                        resultValue=this.invoiceServiceManager.SaveCheckOutPatientFeeInvoice(obj, operTime.ToString(), oper.ID, begin, end);
                    }
                    //סԺ��Ʊ����{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
                    //if (enumType == Neusoft.HISFC.Models.Fee.EnumInvoiceType.I)
                    if (enumType == "I")
                    {
                        resultValue=this.invoiceServiceManager.SaveCheckInpatientFeeInvoice(obj, operTime.ToString(), oper.ID, begin, end);
                    }
                    if (resultValue <= 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("��������ʧ�ܣ�" + this.invoiceServiceManager.Err);
                        return;
                    }
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                MessageBox.Show("�������ݳɹ���");

                this.GetInvoiceData(begin, end, casher);
            }
            catch(Exception ex)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ���ط�Ʊ����
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        private void GetInvoiceData(string begin, string end,string casher)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ����������Ժ�^-^");
            Application.DoEvents();
            switch (enumType)
            {
                //���﷢Ʊ {6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
                //case Neusoft.HISFC.Models.Fee.EnumInvoiceType.C:
                //    {
                //        GetOutpatientFeeInvoice(begin, end, casher);
                //        break;
                //    }
                //case Neusoft.HISFC.Models.Fee.EnumInvoiceType.I:
                //    {
                //        GetInpatientFeeInvoice(begin, end, casher);
                //        break;
                //    }
                case "C":
                    {
                        GetOutpatientFeeInvoice(begin, end, casher);
                        break;
                    }
                case "I":
                    {
                        GetInpatientFeeInvoice(begin, end, casher);
                        break;
                    }
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        #endregion

        #region �¼�
        private void ucCheckInvoice_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            object obj = e.Node.Tag;
            if (obj == null) return;
            //��ֹʱ��
            begin = this.dtBegin.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00";
            end = this.dtEnd.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59";

            string indexStr = (obj as NeuObject).ID;
            enumType = indexStr;
            //{6A6FDD3F-ACBB-4ff7-80BC-6AD0FF69360E}
                //(Neusoft.HISFC.Models.Fee.EnumInvoiceType)Enum.Parse(typeof(Neusoft.HISFC.Models.Fee.EnumInvoiceType),indexStr);
            if (this.cbsky.Tag == null)
            {
                casher = "ALL";
            }
            else
            {
                casher = cbsky.Tag.ToString();
            }
            GetInvoiceData(begin, end, casher);
        }
        #endregion

        private void cbsky_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.casher = this.cbsky.Tag.ToString();
            begin = this.dtBegin.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00";
            end = this.dtEnd.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59";
            this.GetInvoiceData(this.begin, this.end, casher);
        }
    }
}
