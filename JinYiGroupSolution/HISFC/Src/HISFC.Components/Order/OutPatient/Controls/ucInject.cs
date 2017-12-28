using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Neusoft.HISFC.Components.Order.OutPatient.Controls
{
    public partial class ucInject : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucInject()
        {
            InitializeComponent();
        }

        //·־��2007-4-5////////
        //ҽע�÷���Ŀά��//////

        #region ϵͳ�������

        //Neusoft.HISFC.BizLogic.Manager.Constant myConstant = null;
        //Neusoft.HISFC.BizLogic.Fee.Outpatient myOutPatient = new Neusoft.HISFC.BizLogic.Fee.Outpatient();
        Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();
        Neusoft.HISFC.BizProcess.Integrate.Fee feeManager = new Neusoft.HISFC.BizProcess.Integrate.Fee();
        Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        #endregion


        #region ����
        ArrayList al = new ArrayList();
        Neusoft.FrameWork.WinForms.Forms.ToolBarService toolbar = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();
        ArrayList alItem = new ArrayList();
        #endregion 


        #region ��ʼ��
        /// <summary>
        /// ��ʼ����������
        /// </summary>
        private void Init()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ����������Ժ�....");
            Application.DoEvents();
            this.initTree();
            this.ucInputItem1.Init();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        /// <summary>
        /// ��ʼ���÷�TreeView
        /// </summary>
        private void initTree()
        {
            this.tvPatientList1.Nodes.Clear();
            TreeNode root = new TreeNode("�÷�");
            root.ImageIndex = 40;
            this.tvPatientList1.Nodes.Add(root);
            //����÷��б�
            if (al != null) al = managerIntegrate.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.USAGE);
            if (al != null)
            {
                foreach (Neusoft.FrameWork.Models.NeuObject obj in al)
                {
                    TreeNode node = new TreeNode(obj.Name);
                    node.Tag = obj;
                    node.ImageIndex = 41;
                    root.Nodes.Add(node);
                }
            }
            root.Expand();
        }
        #endregion

        
        #region �¼�

        /// <summary>
        /// ��ʼ��ToolBar
        /// </summary>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolbar.AddToolButton("ɾ��", "ɾ������", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sɾ��, true, false, null);
            return this.toolbar;
        }

        private void ucInject_Load(object sender, EventArgs e)
        {
            try
            {
                this.Init();

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.saveData();
            return 1;
            
        }
        
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "ɾ��")
            {
                if (this.neuSpread1_Sheet1.Rows.Count <= 0) return;
                int row = this.neuSpread1_Sheet1.ActiveRowIndex;
                if (row < 0) return;
                DialogResult Result = MessageBox.Show("ȷ��ɾ�������ݣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (Result == DialogResult.OK)
                {
                    this.neuSpread1_Sheet1.Rows.Remove(row, 1);
                }
            }
            
            base.ToolStrip_ItemClicked(sender, e);
        }

        private void tvPatientList1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode current = this.tvPatientList1.SelectedNode;

            if (current == null || current.Parent == null)
            {
                if (this.neuSpread1_Sheet1.RowCount > 0)
                    this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);

                this.neuSpread1_Sheet1.Tag = null;
            }
            else
            {
                Neusoft.FrameWork.Models.NeuObject usage = current.Tag as Neusoft.FrameWork.Models.NeuObject;
            
                this.refresh(usage);
            }
        }

        private void ucInputItem1_SelectedItem(Neusoft.FrameWork.Models.NeuObject sender)
        {
            if (this.ucInputItem1.FeeItem == null) return;
            for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)//����Ƿ��ظ�
            {
                //neusoft.neHISFC.Components.Object.neuObject obj = this.neuSpread1_Sheet1.Rows[i].Tag as neusoft.neHISFC.Components.Object.neuObject;
                Neusoft.FrameWork.Models.NeuObject obj = this.neuSpread1.ActiveSheet.Rows[i].Tag as Neusoft.FrameWork.Models.NeuObject;
                if (obj.Memo == this.ucInputItem1.FeeItem.ID)
                {
                    return;//����ظ� ����
                }
            }
            this.AddItemToFp(this.ucInputItem1.FeeItem, 0);
        }

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DialogResult Result = MessageBox.Show("ȷ��ɾ�������ݣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (Result == DialogResult.OK)
            {
                this.neuSpread1_Sheet1.Rows.Remove(e.Row,1);
            }
        }
        
       
        #endregion


        #region ����
        /// <summary>
        /// �����Ŀ��farpoint
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="row"></param>
        private void AddItemToFp(object Item, int row)
        {
            if (this.neuSpread1_Sheet1.Tag == null)
            {
                MessageBox.Show("����ѡ���÷�!", "��ʾ");
                return;
                
            }
            
            if (Item.GetType()==typeof(Neusoft.HISFC.Models.Fee.Item.Undrug)|| Item.GetType() == typeof(Neusoft.HISFC.Models.Base.Item))
            {
                
                Neusoft.HISFC.Models.Base.Item myItem = Item as Neusoft.HISFC.Models.Base.Item;
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                //�жϸ���Ŀ�Ƿ����
                if (!myItem.IsValid)
                {
                    MessageBox.Show("����Ŀ�����ò���ѡ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //��֤�����Ƿ����ظ�����Ŀ
                if (!Valid(myItem.ID))
                {
                    return;
                }
                //��neuSpread1_Sheet1�м�������
                obj.ID = myItem.ID;
                obj.Name = myItem.Name;
                this.neuSpread1.ActiveSheet.Rows.Add(row, 1);
                this.neuSpread1_Sheet1.Cells[row, 0].Text = myItem.Name;
                this.neuSpread1_Sheet1.Cells[row, 0].Tag = myItem.ID;
                this.neuSpread1_Sheet1.Rows[row].Tag = obj;
                this.neuSpread1_Sheet1.Cells[row, 1].Text = (this.orderManager.Operator as Neusoft.HISFC.Models.Base.Employee).Name;
                this.neuSpread1_Sheet1.Cells[row, 1].Tag = (this.orderManager.Operator as Neusoft.HISFC.Models.Base.Employee).ID;
                this.neuSpread1_Sheet1.Cells[row, 2].Text =  this.orderManager.GetSysDateTime();
            }
        }

        /// <summary>
        /// ˢ���б�
        /// </summary>
        /// <param name="person"></param>
        private void refresh(Neusoft.FrameWork.Models.NeuObject usage)
        {
            if (this.neuSpread1_Sheet1.RowCount > 0)
                this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);
            try
            {
                //this.tabPage1.Title = usage.Name;
                this.tabPage1.Text = usage.Name;
                this.neuSpread1_Sheet1.Tag = usage;
                //����������ά����Ϣ
                //if (this.myOutPatient == null) this.myOutPatient = new Neusoft.HISFC.BizLogic.Fee.Outpatient();
                alItem.Clear();
                alItem = this.feeManager.GetInjectInfoByUsage(usage.ID);// this.myOutPatient.GetInjectInfoByUsage(usage.ID);
                if (alItem != null && alItem.Count>0)
                {
                    //Neusoft.HISFC.Models.Base.Employee em = user.GetPerson( (alItem[0] as Neusoft.FrameWork.Models.NeuObject).User02);
                    //Neusoft.HISFC.Models.Base.Employee em = feeManager.GetPerson((alItem[0] as Neusoft.FrameWork.Models.NeuObject).User02);
                    Neusoft.HISFC.Models.Base.Employee em = managerIntegrate.GetEmployeeInfo((alItem[0] as Neusoft.FrameWork.Models.NeuObject).User02);
                    foreach (Neusoft.FrameWork.Models.NeuObject obj in alItem)
                    {
                        this.neuSpread1_Sheet1.Rows.Add(this.neuSpread1_Sheet1.RowCount, 1);
                        int row = this.neuSpread1_Sheet1.RowCount - 1;
                        this.neuSpread1_Sheet1.Rows[row].Tag = obj;
                        //��Ŀ����
                        this.neuSpread1_Sheet1.Cells[row, 0].Text = obj.Name;//(row,0,obj.User01);
                        this.neuSpread1_Sheet1.Cells[row, 0].Tag = obj.ID;//(row,0,obj.Memo);						
                        //����Ա
                        this.neuSpread1_Sheet1.SetValue(row, 1, em.Name, false);
                        //����ʱ��
                        //this.neuSpread1_Sheet1.SetValue(row, 2, this.myOutPatient.GetSysDateTime(), false);
                        this.neuSpread1_Sheet1.SetValue(row, 2, obj.User03);

                        
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + this.feeManager.Err);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void saveData()
        {
            #region ��֤���ݷ���ѡ����Ŀ��
            //��֤����
            //if (!this.Valid())
            //{
            //    this.neuSpread1.Focus();
            //    return;
            //}
            #endregion

            this.neuSpread1.StopCellEditing();

            if (this.neuSpread1_Sheet1.Tag == null)
            {
                MessageBox.Show("����ѡ����Ŀ!", "��ʾ");
                return;
            }
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            //Neusoft.FrameWork.Management.Transaction tran = new Neusoft.FrameWork.Management.Transaction(this.orderManager.Connection);// this.myOutPatient.Connection);

            //��ʼ����
            try
            {
                //tran.BeginTransaction();
                this.feeManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                //this.myOutPatient.SetTrans(tran.Trans);
                //this.orderManager.SetTrans(tran.Trans);
                if (alItem.Count > 0)
                {
                    //if (this.myOutPatient.DelInjectInfo((this.neuSpread1_Sheet1.Tag as Neusoft.FrameWork.Models.NeuObject).ID) == -1)
                    if (this.feeManager.DelInjectInfo((this.neuSpread1_Sheet1.Tag as Neusoft.FrameWork.Models.NeuObject).ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(this.feeManager.Err, "��ʾ");
                        return;
                    }
                }
                //ѭ������
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    Neusoft.FrameWork.Models.NeuObject objInject = new Neusoft.FrameWork.Models.NeuObject();
                    //�÷�����
                    objInject.Memo = (this.neuSpread1_Sheet1.Tag as Neusoft.FrameWork.Models.NeuObject).ID;
                    //�÷�����
                    objInject.User01 = (this.neuSpread1_Sheet1.Tag as Neusoft.FrameWork.Models.NeuObject).Name;
                    //��Ŀ����
                    objInject.ID = (this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.FrameWork.Models.NeuObject).ID;
                    //��Ŀ����
                    objInject.Name = this.neuSpread1_Sheet1.Cells[i, 0].Text.ToString();
                    //����Ա					
                    objInject.User02 = (this.orderManager.Operator as Neusoft.HISFC.Models.Base.Employee).ID;

                    if (this.feeManager.InsertInjectInfo(objInject) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show(this.feeManager.Err, "��ʾ");
                        return;
                    }
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                #region addby xuewj 2010-3-22 {4D892436-DEF3-4d27-9692-5C3D45ABBD1B} 
                TreeNode current = this.tvPatientList1.SelectedNode;
                Neusoft.FrameWork.Models.NeuObject usage = current.Tag as Neusoft.FrameWork.Models.NeuObject;
                alItem.Clear();
                alItem = this.feeManager.GetInjectInfoByUsage(usage.ID);
                #endregion
            }
            catch (Exception e)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(e.Message, "��ʾ");
                return;
            }
            MessageBox.Show("����ɹ�!", "��ʾ");
        }

        /// <summary>
        /// ��֤���ݿ��Ƿ����ظ�������()
        /// </summary>
        /// <param name="ItemID">��ĿID</param>
        /// <returns></returns>
        private bool Valid(string Item_ID)
        {
            try
            {
                //Hashtable hash = new Hashtable();
                if (this.neuSpread1_Sheet1.Rows.Count > 0)
                {
                    this.neuSpread1.StopCellEditing();
                    for (int i = 0; i < this.neuSpread1_Sheet1.Rows.Count; i++)
                    {
                        string ItemID = this.neuSpread1_Sheet1.Cells[i, 0].Tag.ToString();// as neusoft.neHISFC.Components.Object.neuObject;
                        
                        if (ItemID==Item_ID)
                        {
                            this.neuSpread1_Sheet1.Rows[i].BackColor = Color.Red;
                            MessageBox.Show("����Ŀ�Ѵ��ڣ���Ŀ�����ظ���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.neuSpread1_Sheet1.Rows[i].BackColor = Color.White;
                            return false;
                        }
                        //else
                        //{
                        //    hash.Add(ItemID, 0);
                        //}
                    }
                }
            }
            catch { return false; }
            return true;
        }

        #endregion

       

        
    }
}
