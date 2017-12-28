using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Neusoft.HISFC.Components.Order.Controls
{
    public partial class ucSetExecBill : Neusoft.FrameWork.WinForms.Forms.IMaintenanceControlable
    {
        private Neusoft.FrameWork.Models.NeuObject objExecBill = new Neusoft.FrameWork.Models.NeuObject();
        private Neusoft.FrameWork.Public.ObjectHelper helper;
        private Neusoft.HISFC.BizProcess.Integrate.Manager costManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        private Neusoft.HISFC.BizLogic.Order.ExecBill oCExecBill = new Neusoft.HISFC.BizLogic.Order.ExecBill();
        private ArrayList alItem = new ArrayList();
        private TreeNode tnItemList = new TreeNode();
        private TreeNode tnDragType = new TreeNode();
        private TreeNode tnConstant = new TreeNode();
        private int icont = 0;

        /// <summary>
        /// ��ʵ�ֽӿ���ʹ�õı���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Order.ExecBill oExecBill = new Neusoft.HISFC.BizLogic.Order.ExecBill();

        //*********************************
        public static string strName = "";
        //*********************************

        public ucSetExecBill()
        {
            InitializeComponent();
        }

        #region ����
        private void LoadTab()
        {
        }
        private void AddInfo(int Branch, Neusoft.FrameWork.Models.NeuObject neuObj, object obj)
        {
            string strText = neuObj.Name;

        }
        private void AddTreeNode(int root, string Name, object obj, int ImageIndex)
        {
            System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode();
            try
            {
                Node.Text = Name;
                Node.Tag = obj;
            }
            catch { }
        }
        private void AddRootNode()
        {
            this.tvDrug.Nodes.Add("ҩ��ִ�е���Ŀѡ��");
            this.tvUndrug.Nodes.Add("��ҩ��ִ�е���Ŀѡ��");
        }
        #endregion

        #region ����

        private void Filter(int index)
        {

            for (int i = 0; i < this.neuSpread1.Sheets[index].RowCount; i++)
            {
                string str1 = this.neuSpread1.Sheets[index].Cells[i, 1].Text;
                string str2 = this.neuSpread1.Sheets[index].Cells[i, 2].Text;
                string str3 = this.neuSpread1.Sheets[index].Cells[i, 3].Text;
                if (str3 != "")//ҩƷ
                {
                    foreach (TreeNode node in this.tvDrug.Nodes)
                    {
                        if (str1 == node.Text)
                        {
                            foreach (TreeNode childnode in node.Nodes)
                            {
                                if (str2 == childnode.Text)
                                {
                                    foreach (TreeNode n in childnode.Nodes)
                                    {
                                        if (str3 == n.Text)
                                        {
                                            n.Remove();
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else//��ҩƷ
                {
                    foreach (TreeNode node in this.tvUndrug.Nodes)
                    {
                        if (str1 == node.Text)
                        {
                            foreach (TreeNode childnode in node.Nodes)
                            {
                                if (str2 == childnode.Text)
                                {
                                    childnode.Remove();
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private ArrayList GetFpSheet()
        {
            ArrayList al = new ArrayList();
            for (int i = 0; i < this.neuSpread1.Sheets.Count; i++)
            {
                al.Add(this.neuSpread1.Sheets[i].SheetName.Trim());
            }
            return al;
        }

        private bool IsRepeat()
        {
            bool bRet = true;
            for (int i = 0; i < this.neuSpread1.Sheets.Count; i++)
            {
                if (txtExecBillName.Text.Trim() == this.neuSpread1.Sheets[i].SheetName.Trim())
                {
                    MessageBox.Show("�����ظ���");
                    bRet = false;
                    grpExecBillD.Visible = true;
                }
            }
            return bRet;
        }

        /// <summary>
        /// ��ʼ��Tree
        /// </summary>
        private void InitTree()
        {
            Neusoft.HISFC.BizProcess.Integrate.Manager oOrderType = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.HISFC.BizProcess.Integrate.Manager oConstant = new Neusoft.HISFC.BizProcess.Integrate.Manager();

            Enum enDrug = Neusoft.HISFC.Models.Base.EnumSysClass.P;
            //enDrug.ToString();
            arrDrugType.AddRange(this.alItem);

            //�����Ŀ����б�
            arrOrderType = oOrderType.QueryOrderTypeList();

            //����÷��б�
            arrConstant = oConstant.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.USAGE);
            arrItemList = Neusoft.HISFC.Models.Base.SysClassEnumService.List();

            //ˢ����ʾ
            this.RefreshList();
            //���������Ѿ��е�
            for (int i = 0; i < this.neuSpread1.Sheets.Count; i++)
                this.Filter(i);
        }

        private void RefreshList()
        {
            this.tvDrug.Nodes.Clear();
            this.tvUndrug.Nodes.Clear();
            //��ҩ��ִ�е�
            try
            {
                for (int i = 0; i < arrOrderType.Count; i++)
                {
                    TreeNode node = new TreeNode(arrOrderType[i].ToString());
                    node.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrOrderType[i]).ID.ToString();

                    for (int j = 0; j < arrItemList.Count; j++)
                    {
                        if (((Neusoft.FrameWork.Models.NeuObject)arrItemList[j]).ID.Substring(0, 1) != "P")
                        {
                            tnItemList = new TreeNode(arrItemList[j].ToString());
                            //tnItemList.Tag = ((Neusoft.HISFC.Models.Base.EnumSysClass)arrItemList[j]).ID.ToString();
                            tnItemList.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrItemList[j]).ID.ToString();
                            node.Nodes.Add(tnItemList);
                        }
                    }
                    this.tvUndrug.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����" + ex.Message);
            }

            //ҩ��ִ�е�
            try
            {
                for (int i = 0; i < arrOrderType.Count; i++)
                {
                    TreeNode node = new TreeNode(arrOrderType[i].ToString());
                    node.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrOrderType[i]).ID.ToString();
                    //node.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrConstant[i]).ID.ToString();//[2007/01/15]xuweizhe

                    for (int j = 0; j < arrDrugType.Count; j++)
                    {
                        tnDragType = new TreeNode(arrDrugType[j].ToString());
                        tnDragType.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrDrugType[j]).ID.ToString();
                        for (int k = 0; k < arrConstant.Count; k++)
                        {
                            tnConstant = new TreeNode(arrConstant[k].ToString());
                            tnConstant.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrConstant[k]).ID.ToString();
                            tnDragType.Nodes.Add(tnConstant);
                        }
                        node.Nodes.Add(tnDragType);
                    }
                    this.tvDrug.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����" + ex.Message);
            }

            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            //��ҩ����Ŀִ�е�
            try
            {
                for (int i = 0; i < arrOrderType.Count; i++)
                {
                    TreeNode node = new TreeNode(arrOrderType[i].ToString());
                    node.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrOrderType[i]).ID.ToString();

                    for (int j = 0; j < arrItemList.Count; j++)
                    {
                        if (((Neusoft.FrameWork.Models.NeuObject)arrItemList[j]).ID.Substring(0, 1) != "P")
                        {
                            tnItemList = new TreeNode(arrItemList[j].ToString());
                            //tnItemList.Tag = ((Neusoft.HISFC.Object.Base.EnumSysClass)arrItemList[j]).ID.ToString();
                            tnItemList.Tag = ((Neusoft.FrameWork.Models.NeuObject)arrItemList[j]).ID.ToString();
                            node.Nodes.Add(tnItemList);
                        }
                    }
                    this.tvUndrugItem.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("����" + ex.Message);
            }
            #endregion
        }

        private void InitControl()
        {
            this.alItem = this.costManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ITEMTYPE);
            helper = new Neusoft.FrameWork.Public.ObjectHelper(alItem);
            this.InitTree();

        }

        private int SaveBill()
        {
            if (this.neuSpread1.Sheets.Count == 0) return -1;

            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            //�������Ŀִ�е����Ѿ��������
            Neusoft.FrameWork.Models.NeuObject bill = this.neuSpread1.ActiveSheet.Tag as Neusoft.FrameWork.Models.NeuObject;
            if (bill.Memo == "1")
            {
                return 0;
            }
            #endregion

            if (this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim() == "" || this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim() == null)
            {
                if (this.txtExecBillName.Text.Trim() == "" || this.txtExecBillName.Text.Trim() == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.Msg("�����뵥�ӵ�����", 211);
                    return -1;
                }
                this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName = this.txtExecBillName.Text.Trim();
            }

            Neusoft.HISFC.BizLogic.Order.ExecBill oExecBill = new Neusoft.HISFC.BizLogic.Order.ExecBill();
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(oExecBill.Connection);
            //t.BeginTransaction();
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            oExecBill.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            ArrayList al = new ArrayList();
            oExecBill.Name = neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim();
            for (int i = 0; i < this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Rows.Count; i++)
            {
                try
                {
                    Neusoft.FrameWork.Models.NeuObject objBill = new Neusoft.FrameWork.Models.NeuObject();
                    //������д��objBill.ID ִ�е���ˮ�ţ�objBill.Memoִ�е����ͣ�1ҩ/2��ҩ,objBill.user01 ҽ������,

                    objBill.Name = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim();//ִ�е���		
                    if (this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Tag != null)
                        #region {46983F5B-E184-4b8b-B819-AA1C34993F1B}
                        //objBill.ID = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Tag.ToString();
                        objBill.ID = ((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Tag).ID;
                        #endregion
                    objBill.Memo = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[i, 0].Text.Trim();//ִ�е����ͣ�
                    //					objBill.Memo = oExecBill.Memo;
                    objBill.User01 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[i, 1].Tag.ToString(); //ҽ������,

                    objBill.User02 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[i, 2].Tag.ToString();//��ҩϵͳ���ҩƷ���,
                    objBill.User03 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[i, 3].Tag.ToString();//ҩƷ�÷���
                    al.Add(objBill);
                    objBill = null;
                }
                catch { }
            }
            //Neusoft.HISFC.BizProcess.Integrate.Manager personMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();

            //personMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            string personId = oExecBill.Operator.ID;
            //Neusoft.HISFC.Models.Base.Employee person = personMgr.GetEmployeeInfo(personId);
            string strNurse = (oExecBill.Operator as Neusoft.HISFC.Models.Base.Employee).Nurse.ID; //person.Nurse.ID.ToString();
            string BillID = "";//ִ�е���

            if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag == null)
            {
                //				fpSpread1.Sheets[fpSpread1.ActiveSheetIndex].Tag = oExecBill.GetNewExecBillID();
                this.objExecBill.Name = neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim();
                if (oExecBill.SetExecBill(al, this.objExecBill, strNurse, ref BillID) == 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag = BillID;
                    //MessageBox.Show("����ɹ�!");
                    return 0;
                }
                else
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                    //MessageBox.Show("����!" + oExecBill.Err);
                    return -1;
                }
            }
            else
            {
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                for (int i = 0; i < al.Count; i++)
                {
                    obj = (Neusoft.FrameWork.Models.NeuObject)al[i];
                    if (oExecBill.UpdateExecBill(obj, strNurse/*, obj.Memo*/) != 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                        //MessageBox.Show("�������!" + oExecBill.Err);
                        return -1;
                    }
                }
                if (this.txtExecBillName.Text.Trim() != "" || this.txtExecBillName.Text.Trim() != null)
                {
                    oExecBill.UpdateExecBillName(obj.ID, this.txtExecBillName.Text.Trim(), obj.User01, obj.User02);
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                //MessageBox.Show("����ɹ�!");

                return 0;
            }
        }


        private ArrayList arrOrderType = new ArrayList();//ҽ������
        private ArrayList arrConstant = new ArrayList();//�����÷�����
        private ArrayList arrItemList = new ArrayList();//��Ŀ����
        private ArrayList arrDrugType = new ArrayList();//ҩƷ���
        private ArrayList alExecBill = new ArrayList();//ִ�е�����

        public bool IsNull(Neusoft.FrameWork.WinForms.Controls.NeuTextBox obj)
        {
            if (obj.Text.Trim() != "")
                return true;
            else
                return false;
        }


        /// <summary>
        /// farP����
        /// </summary>
        /// <param name="obj">ҽ������</param>
        /// <param name="i">���Ӹ���</param>
        protected void AddExecBill(Neusoft.HISFC.Models.Order.Inpatient.Order obj, int i)
        {
            this.neuSpread1.Sheets[i].Rows.Add(0, 1);
            this.neuSpread1.Sheets[i].SetValue(0, 0, obj.Memo, false);
            //fpSpread1.Sheets[i].Cells[0,0].Text = obj.Memo.ToString();//ִ�е�����		
            this.neuSpread1.Sheets[i].SetValue(0, 1, obj.OrderType.Name, false);
            //fpSpread1.Sheets[i].Cells[0,1].Value  = obj.OrderType.Name;//ҽ������
            this.neuSpread1.Sheets[i].Cells[0, 1].Tag = obj.OrderType.ID;
            if (obj.Memo == "2")
            {
                this.neuSpread1.Sheets[i].SetValue(0, 2, obj.Item.SysClass.Name, false);
                //fpSpread1.Sheets[i].Cells[0,2].Value = obj.Item.SysClass.Name;//ҩƷ����
                this.neuSpread1.Sheets[i].Cells[0, 2].Tag = obj.Item.SysClass.ID;
            }
            else
            {
                this.neuSpread1.Sheets[i].SetValue(0, 2, helper.GetName(obj.Item.User01), false);
                //fpSpread1.Sheets[i].Cells[0,2].Value = obj.Item.SysClass.Name;//ҩƷ����
                this.neuSpread1.Sheets[i].Cells[0, 2].Tag = obj.Item.User01;
            }
            this.neuSpread1.Sheets[i].SetValue(0, 3, obj.Usage.Name, false);
            //fpSpread1.Sheets[i].Cells[0,3].Text = obj.Usage.Name;//���÷���
            this.neuSpread1.Sheets[i].Cells[0, 3].Tag = obj.Usage.ID;
            this.neuSpread1.Sheets[i].SetValue(0, 4, oCExecBill.Operator.Name, false);
            //fpSpread1.Sheets[i].Cells[0,4].Text = oCExecBill.Operator.Name.ToString();//����Ա
            this.neuSpread1.Sheets[i].Cells[0, 4].Tag = oCExecBill.Operator.ID.ToString();
            this.neuSpread1.Sheets[i].SetValue(0, 5, DateTime.Now.ToString(), false);
            //fpSpread1.Sheets[i].Cells[0,5].Text = oCExecBill.GetSysDate();//����ʱ��						
        }

        /// <summary>
        /// ����ϸ��ӵ�farpoint����
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <param name="isItemDetail">�Ƿ���Ŀ��ϸִ�е� </param>
        protected void AddExecBill(Neusoft.HISFC.Models.Order.Inpatient.Order obj, int i, bool isItemDetail)
        {
            this.neuSpread1.Sheets[i].Rows.Add(0, 1);

            this.neuSpread1.Sheets[i].SetValue(0, 0, obj.Memo, false);	//ҩƷ/��ҩƷ
            this.neuSpread1.Sheets[i].SetValue(0, 1, obj.OrderType.Name, false); //ҽ�����
            this.neuSpread1.Sheets[i].Cells[0, 1].Tag = obj.OrderType.ID;

            if (obj.Memo == "2")
            {
                this.neuSpread1.Sheets[i].SetValue(0, 2, obj.Item.SysClass.Name, false); //ϵͳ���
                this.neuSpread1.Sheets[i].Cells[0, 2].Tag = obj.Item.SysClass.ID;
            }
            else
            {
                this.neuSpread1.Sheets[i].SetValue(0, 2, helper.GetName(obj.Item.User01), false); //ҩƷ���
                this.neuSpread1.Sheets[i].Cells[0, 2].Tag = obj.Item.User01;
            }

            if (isItemDetail)
            {
                this.neuSpread1.Sheets[i].SetValue(0, 3, obj.Item.Name, false); //��Ŀ����
                this.neuSpread1.Sheets[i].Cells[0, 3].Tag = obj.Item.ID;
                this.neuSpread1.Sheets[i].Rows[0].Tag = obj.Item;
            }
            else
            {
                this.neuSpread1.Sheets[i].SetValue(0, 3, obj.Usage.Name, false); //��������
                this.neuSpread1.Sheets[i].Cells[0, 3].Tag = obj.Usage.ID;
            }

            this.neuSpread1.Sheets[i].SetValue(0, 4, oCExecBill.Operator.Name, false);
            this.neuSpread1.Sheets[i].Cells[0, 4].Tag = oCExecBill.Operator.ID.ToString();
            this.neuSpread1.Sheets[i].SetValue(0, 5, DateTime.Now.ToString(), false);
        }


        //private void InitFp(string strBillName, int i, string strID)
        //{46983F5B-E184-4b8b-B819-AA1C34993F1B}
        private void InitFp(Neusoft.FrameWork.Models.NeuObject execBill, int i)
        {
            //			this.fpTreeView1.sv_CellChanged+=new FarPoint.Win.Spread.SheetViewEventHandler(fpTreeView1_sv_CellChanged);

            this.neuSpread1.Sheets.Count = i + 1;
            if (execBill.Name == "")
                this.neuSpread1.Sheets[i].SheetName = " ";
            else
                this.neuSpread1.Sheets[i].SheetName = execBill.Name;
            //			this.fpSpread1.Sheets[i].Tag = 
            this.neuSpread1.Sheets[i].Columns[0].Visible = false;
            this.neuSpread1.Sheets[i].Columns[1].Label = "ҽ������";
            this.neuSpread1.Sheets[i].Columns[2].Label = "��Ŀ���";
            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            //this.neuSpread1.Sheets[i].Columns[3].Label = "���÷���";
            //��ҩƷ��Ŀ��ϸִ�е�
            if (Neusoft.FrameWork.Function.NConvert.ToBoolean(execBill.Memo))
            {
                this.neuSpread1.Sheets[i].Columns[3].Label = "��Ŀ����";
            }
            else
            {
                this.neuSpread1.Sheets[i].Columns[3].Label = "���÷���";
            }
            #endregion
            this.neuSpread1.Sheets[i].Columns[4].Label = "��ǰ����Ա";
            this.neuSpread1.Sheets[i].Columns[5].Label = "����ʱ��";

            this.neuSpread1.Sheets[i].Columns[1].Width = 150;
            this.neuSpread1.Sheets[i].Columns[2].Width = 150;
            this.neuSpread1.Sheets[i].Columns[3].Width = 150;
            this.neuSpread1.Sheets[i].Columns[4].Width = 150;
            this.neuSpread1.Sheets[i].Columns[5].Width = 150;

            this.neuSpread1.Sheets[i].RowCount = 0;
            this.neuSpread1.Sheets[i].ColumnCount = 6;
            this.neuSpread1.Sheets[i].GrayAreaBackColor = Color.WhiteSmoke;

            this.neuSpread1.ActiveSheetIndex = i;
            #region  xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            //if (strID != "")
            //    this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag = strID;
            //else
            //    this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag = null;

            this.neuSpread1.ActiveSheet.Tag = execBill;
            #endregion
            int im = 3;
            this.neuSpread1.Sheets[i].OperationMode = (FarPoint.Win.Spread.OperationMode)im;
            this.neuSpread1.Sheets[i].SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);
        }
        private void BindFp()
        {
            Neusoft.HISFC.BizProcess.Integrate.Manager personMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.HISFC.BizLogic.Order.ExecBill oExecBill = new Neusoft.HISFC.BizLogic.Order.ExecBill();
            //			ArrayList alExecBill = new ArrayList();
            string personId = oExecBill.Operator.ID;
            Neusoft.HISFC.Models.Base.Employee person = (oExecBill.Operator as Neusoft.HISFC.Models.Base.Employee);//personMgr.GetPersonByID(personId);
            string strNurse = person.Nurse.ID.ToString();
            alExecBill = oExecBill.QueryExecBill(strNurse);
            Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
            ArrayList arrDetail = new ArrayList();
            for (int i = 0; i < alExecBill.Count; i++)
            {
                #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
                //string strName = ((Neusoft.FrameWork.Models.NeuObject)alExecBill[i]).Name;
                //string strID = ((Neusoft.FrameWork.Models.NeuObject)alExecBill[i]).ID;
                //InitFp(strName, i, strID);
                //arrDetail = oExecBill.QueryExecBillDetail(strID); 
                obj = alExecBill[i] as Neusoft.FrameWork.Models.NeuObject;
                InitFp(obj, i);
                arrDetail = oExecBill.QueryExecBillDetail(obj.ID);
                #endregion
                if (arrDetail != null)
                {
                    for (int j = 0; j < arrDetail.Count; j++)
                    {
                        Neusoft.HISFC.Models.Order.Inpatient.Order objDetail = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                        #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��

                        //objDetail.ID = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).ID;//ִ�е�id
                        //objDetail.Memo = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).Memo;//ҩƷ��ҩƷ
                        //objDetail.OrderType.ID = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).OrderType.ID;//ҽ�����id
                        //objDetail.OrderType.Name = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).OrderType.Name;//ҽ���������
                        //if (objDetail.Memo == "1")                        
                        //    objDetail.Item.User01 = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).Item.User01;//ҩƷ����                        
                        //else                        
                        //    objDetail.Item.SysClass.ID = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).Item.SysClass.ID;//ϵͳ���


                        ////						objDetail.Item.SysClass.Name = ((Neusoft.HISFC.Models.Order.Order)arrDetail[j]).Item.SysClass.Name;//ϵͳ���
                        //objDetail.Usage.ID = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).Usage.ID;//�÷�id
                        //objDetail.Usage.Name = ((Neusoft.HISFC.Models.Order.Inpatient.Order)arrDetail[j]).Usage.Name;//�÷�name

                        //AddExecBill(objDetail, i);

                        objDetail = arrDetail[j] as Neusoft.HISFC.Models.Order.Inpatient.Order;
                        AddExecBill(objDetail, i, Neusoft.FrameWork.Function.NConvert.ToBoolean(obj.Memo));

                        #endregion
                    }
                }
                icont = alExecBill.Count;
            }

        }


        #endregion

        #region ����

        public string ExeBillName
        {
            get
            {
                return strName;
            }
            set
            {
                strName = value;
            }
        }
        #endregion

        #region �¼�

        private void EventResultChanged(ArrayList al)
        {

        }

        private void PrintInfo()
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.ControlBorder = Neusoft.FrameWork.WinForms.Classes.enuControlBorder.None;
            print.PrintPreview(this.neuPanel2);
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.neuSpread1.ActiveSheet == null) return;
            if (this.neuSpread1.ActiveSheet.ActiveRow == null) return;

            DialogResult result;
            result = MessageBox.Show("�Ƿ�ɾ������", "ȷ��", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //ҽ������
                string orderType = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 1].Text;
                //ϵͳ���
                string sysClass = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 2].Text;
                //ʹ�÷���
                string usage = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 3].Text;

                //�ָ��������б���
                if (this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 0].Text == "1")
                {
                    foreach (TreeNode node in this.tvDrug.Nodes)
                    {
                        if (orderType == node.Text)
                        {
                            foreach (TreeNode childnode in node.Nodes)
                            {
                                if (sysClass == childnode.Text)
                                {
                                    TreeNode obj = new TreeNode(usage);
                                    obj.Tag = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 3].Tag.ToString();
                                    childnode.Nodes.Add(obj);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    foreach (TreeNode node in this.tvUndrug.Nodes)
                    {
                        if (orderType == node.Text)
                        {
                            TreeNode obj = new TreeNode(sysClass);
                            obj.Tag = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 2].Tag.ToString();
                            node.Nodes.Add(obj);
                            break;
                        }
                    }
                }
                if (neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Tag != null)
                {
                    string exeBillID = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Tag.ToString();//ִ�е���
                    if (exeBillID != null && exeBillID != "")
                    {
                        Neusoft.HISFC.BizLogic.Order.ExecBill billMgr = new Neusoft.HISFC.BizLogic.Order.ExecBill();
                        //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(billMgr.Connection);
                        //t.BeginTransaction();
                        Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        billMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                        Neusoft.FrameWork.Models.NeuObject objBill = new Neusoft.FrameWork.Models.NeuObject();
                        //������д��objBill.ID ִ�е���ˮ�ţ�objBill.Memoִ�е����ͣ�1ҩ/2��ҩ,objBill.user01 ҽ������,
                        // objBill.user02��ҩϵͳ���ҩƷ���,objBill.user03 ҩƷ�÷���			
                        objBill.Name = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].SheetName.Trim();//ִ�е���		
                        objBill.ID = exeBillID;
                        objBill.Memo = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 0].Text.Trim();//ִ�е����ͣ�
                        objBill.User01 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 1].Tag.ToString(); //ҽ������,
                        objBill.User02 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 2].Tag.ToString();//��ҩϵͳ���ҩƷ���,
                        objBill.User03 = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 3].Tag.ToString();//ҩƷ�÷���

                        if (billMgr.DeleteExecBill(objBill) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                            MessageBox.Show(billMgr.Err, "��ʾ");
                            return;
                        }
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                    }
                }
                neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Rows.Remove(neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].ActiveRowIndex, 1);
            }
        }

        private void ucSetExecBill_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitControl();
                //if((Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee).IsManager)
                //    this.btnDelete.Visible = true;
                //else
                //    this.btnDelete.Visible = false;
                grpExecBillD.Visible = false;
                grpExecBillName.Visible = true;
                tabItemType.Visible = true;
                BindFp();
                grpExecBillD.Visible = true;
                if (this.neuSpread1.Sheets.Count > 0)
                {
                    for (int index = 0; index < this.neuSpread1.Sheets.Count; index++)
                    {
                        #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��

                        if (this.neuSpread1.Sheets[index].Tag == null) { return; }
                        Neusoft.FrameWork.Models.NeuObject execBill = this.neuSpread1.Sheets[index].Tag as Neusoft.FrameWork.Models.NeuObject;
                        if (Neusoft.FrameWork.Function.NConvert.ToBoolean(execBill.Memo))
                        {
                        }
                        else
                        {
                            this.Filter(index);
                        }

                        #endregion
                    }
                    this.neuSpread1.ActiveSheetIndex = 0;
                    this.txtExecBillName.Text = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName;
                    #region  {46983F5B-E184-4b8b-B819-AA1C34993F1B}
                    if (this.neuSpread1.Sheets.Count == 1)
                    {
                        this.SetTabVisible();
                    }
                    #endregion
                }
            }
            catch { }

        }

        private void tvDrug_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.neuSpread1.Sheets.Count == 0) return;
            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            if (this.neuSpread1.ActiveSheet.Tag == null)
            {
                MessageBox.Show("��ѡ��ִ�е�!");
                return;
            }
            if (this.tvDrug.SelectedNode == null)
            {
                return;
            }
            #endregion
            Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();
            if (this.tvDrug.SelectedNode.Parent != null)
            {
                //Ҷ�ӽڵ�---���÷���
                if (this.tvDrug.SelectedNode.Parent.Parent != null && this.tvDrug.SelectedNode.Parent != null)
                {
                    ArrayList alTree = new ArrayList();
                    obj.ID = "";//ִ�е�id
                    obj.Memo = "1";//ҩƷ��ҩƷ
                    obj.OrderType.ID = this.tvDrug.SelectedNode.Parent.Parent.Tag.ToString();//ҽ�����id
                    obj.OrderType.Name = this.tvDrug.SelectedNode.Parent.Parent.Text;//ҽ���������
                    //[xuweizhe]obj.Item.SysClass.ID = this.tvDrug.SelectedNode.Parent.Tag.ToString();//ϵͳ���
                    obj.Item.User01 = this.tvDrug.SelectedNode.Parent.Tag.ToString();
                    obj.Usage.ID = this.tvDrug.SelectedNode.Tag.ToString();//�÷�id
                    obj.Usage.Name = this.tvDrug.SelectedNode.Text;
                    AddExecBill(obj, this.neuSpread1.ActiveSheetIndex);
                    this.tvDrug.SelectedNode.Parent.Nodes.RemoveAt(this.tvDrug.SelectedNode.Index);
                }
                else if (this.tvDrug.SelectedNode.Parent != null)
                {
                    //ҩƷ���ͽڵ�
                    string[] arrAll = new string[this.tvDrug.SelectedNode.Nodes.Count];
                    for (int i = this.tvDrug.SelectedNode.Nodes.Count - 1; i >= 0; i--)
                    {
                        obj.ID = "";//ִ�е�id
                        obj.Memo = "1";//ҩƷ��ҩƷ
                        obj.OrderType.ID = this.tvDrug.SelectedNode.Parent.Tag.ToString();//ҽ�����id
                        obj.OrderType.Name = this.tvDrug.SelectedNode.Parent.Text;//ҽ���������
                        //[xuweizhe]obj.Item.SysClass.ID = this.tvDrug.SelectedNode.Tag.ToString();//ϵͳ���
                        obj.Usage.ID = this.tvDrug.SelectedNode.Nodes[i].Tag.ToString();//�÷�id
                        obj.Usage.Name = this.tvDrug.SelectedNode.Nodes[i].Text;
                        obj.Item.User01 = this.tvDrug.SelectedNode.Tag.ToString();
                        AddExecBill(obj, this.neuSpread1.ActiveSheetIndex);
                        this.tvDrug.SelectedNode.Nodes[i].Remove();
                    }
                }
            }
        }

        private void tvUndrug_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.neuSpread1.Sheets.Count == 0) return;
            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            if (this.neuSpread1.ActiveSheet.Tag == null)
            {
                MessageBox.Show("��ѡ��ִ�е�!");
                return;
            }

            if (this.tvUndrug.SelectedNode == null)
            {
                return;
            }
            #endregion
            Neusoft.HISFC.Models.Order.Inpatient.Order obj = new Neusoft.HISFC.Models.Order.Inpatient.Order();

            if (this.tvUndrug.SelectedNode.Parent != null)
            {
                ArrayList alTree = new ArrayList();
                obj.ID = "";//ִ�е�id
                obj.Memo = "2";//ҩƷ��ҩƷ
                obj.OrderType.ID = this.tvUndrug.SelectedNode.Parent.Tag.ToString();//ҽ�����id
                obj.OrderType.Name = tvUndrug.SelectedNode.Parent.Text;//ҽ���������
                obj.Item.SysClass.ID = tvUndrug.SelectedNode.Tag.ToString();//ϵͳ���
                AddExecBill(obj, this.neuSpread1.ActiveSheetIndex);
                tvUndrug.SelectedNode.Parent.Nodes.RemoveAt(tvUndrug.SelectedNode.Index);
            }
            else
            {
                for (int i = this.tvUndrug.SelectedNode.Nodes.Count - 1; i >= 0; i--)
                {
                    obj.ID = "";//ִ�е�id
                    obj.Memo = "2";//ҩƷ��ҩƷ
                    obj.OrderType.ID = this.tvUndrug.SelectedNode.Tag.ToString();//ҽ�����id
                    obj.OrderType.Name = tvUndrug.SelectedNode.Text;//ҽ���������
                    obj.Item.SysClass.ID = tvUndrug.SelectedNode.Nodes[i].Tag.ToString();//ϵͳ���

                    AddExecBill(obj, this.neuSpread1.ActiveSheetIndex);
                    this.tvUndrug.SelectedNode.Nodes[i].Remove();
                }
            }
        }

        private void neuSpread1_ActiveSheetChanging(object sender, FarPoint.Win.Spread.ActiveSheetChangingEventArgs e)
        {
            if (this.neuSpread1.ActiveSheet != null)
            {
                if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag == null)
                {
                    DialogResult result;
                    if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Rows.Count > 0)
                    {
                        result = MessageBox.Show("�����ѱ��޸�����δ���̣����ڱ�����", "ȷ��", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            SaveBill();
                        }
                        if (result == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            this.txtExecBillName.Text = this.neuSpread1.Sheets[e.ActivatedSheetIndex].SheetName;
        }

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.neuSpread1.ActiveSheet == null) return;
            if (this.neuSpread1.ActiveSheet.ActiveRow == null) return;

            DialogResult result;
            result = MessageBox.Show("�Ƿ�ɾ������", "ȷ��", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //ҽ������
                string orderType = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 1].Text;
                //ϵͳ���
                string sysClass = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 2].Text;
                //ʹ�÷���
                string usage = this.neuSpread1.Sheets[neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 3].Text;
                #region {46983F5B-E184-4b8b-B819-AA1C34993F1B}
                if (((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag).Memo != "1")
                {//��ǰҳ���ǵ���Ŀ����
                    //�ָ��������б���
                    if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 0].Text == "1")
                    {
                        foreach (TreeNode node in this.tvDrug.Nodes)
                        {
                            if (orderType == node.Text)
                            {
                                foreach (TreeNode childnode in node.Nodes)
                                {
                                    if (sysClass == childnode.Text)
                                    {
                                        TreeNode obj = new TreeNode(usage);
                                        obj.Tag = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 3].Tag.ToString();
                                        childnode.Nodes.Add(obj);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeNode node in this.tvUndrug.Nodes)
                        {
                            if (orderType == node.Text)
                            {
                                TreeNode obj = new TreeNode(sysClass);
                                obj.Tag = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[this.neuSpread1.ActiveSheet.ActiveRowIndex, 2].Tag.ToString();
                                node.Nodes.Add(obj);
                                break;
                            }
                        }
                    }
                }
                #endregion
                if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag != null)
                {
                    #region {46983F5B-E184-4b8b-B819-AA1C34993F1B}
                    //string exeBillID = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag.ToString();//ִ�е���
                    string exeBillID = ((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag).ID;//ִ�е���
                    #endregion
                    if (exeBillID != null && exeBillID != "")
                    {
                        Neusoft.HISFC.BizLogic.Order.ExecBill billMgr = new Neusoft.HISFC.BizLogic.Order.ExecBill();
                        //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(billMgr.Connection);
                        //t.BeginTransaction();
                        Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                        billMgr.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                        Neusoft.FrameWork.Models.NeuObject objBill = new Neusoft.FrameWork.Models.NeuObject();
                        //������д��objBill.ID ִ�е���ˮ�ţ�objBill.Memoִ�е����ͣ�1ҩ/2��ҩ,objBill.user01 ҽ������,
                        // objBill.user02��ҩϵͳ���ҩƷ���,objBill.user03 ҩƷ�÷���			
                        objBill.Name = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName.Trim();//ִ�е���		
                        objBill.ID = exeBillID;
                        objBill.Memo = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 0].Text.Trim();//ִ�е����ͣ�
                        objBill.User01 = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 1].Tag.ToString(); //ҽ������,
                        objBill.User02 = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 2].Tag.ToString();//��ҩϵͳ���ҩƷ���,
                        objBill.User03 = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Cells[neuSpread1.ActiveSheet.ActiveRowIndex, 3].Tag.ToString();//ҩƷ�÷���
                        #region {46983F5B-E184-4b8b-B819-AA1C34993F1B}
                        if (((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag).Memo != "1")
                        {//��ǰҳ���ǵ���Ŀ����
                            if (billMgr.DeleteExecBill(objBill) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                MessageBox.Show(billMgr.Err, "��ʾ");
                                return;
                            }
                        }
                        else
                        {//��ǰҳ�ǵ���Ŀ���ͣ�ɾ��һ����Ŀ
                            //��DataSet���д���
                            if (this.unDrugItemSelect != null)
                            {
                                DataRow delItemRow = this.unDrugItemSelect.ucInputUndrug.DsUndrugItem.Tables[objBill.User01].NewRow();
                                Neusoft.HISFC.Models.Base.Item delItem = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Rows[this.neuSpread1.ActiveSheet.ActiveRowIndex].Tag as Neusoft.HISFC.Models.Base.Item;

                                delItemRow["����"] = delItem.ID;
                                delItemRow["����"] = delItem.Name;
                                delItemRow["���"] = delItem.Specs;
                                delItemRow["�۸�"] = delItem.Price;
                                delItemRow["��λ"] = delItem.PriceUnit;
                                delItemRow["���"] = delItem.SysClass.ID;
                                delItemRow["������"] = delItem.SysClass.ID;
                                delItemRow["ƴ����"] = delItem.SpellCode;
                                delItemRow["�����"] = delItem.WBCode;
                                delItemRow["�Զ�����"] = delItem.UserCode;

                                this.unDrugItemSelect.ucInputUndrug.DsUndrugItem.Tables[objBill.User01].Rows.Add(delItemRow);
                            }
                            if (billMgr.DeleteExecBillOneItem(objBill) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack(); ;
                                MessageBox.Show(billMgr.Err, "��ʾ");
                                return;
                            }
                        }
                        #endregion
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                    }
                }
                this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Rows.Remove(this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].ActiveRowIndex, 1);
            }
        }

        #endregion

        #region IMaintenanceControlable ��Ա

        public int Add()
        {
            try
            {
                cResult r = new cResult();
                r.TextChanged += new TextChangedHandler(this.EventResultChanged);
                r.al = GetFpSheet();

                ucBillAdd ba = new ucBillAdd(r);
                ba.alExecBill = this.alExecBill;
                Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(ba);
                this.objExecBill = ba.objExecBill;
                if (r.Result1 != "")
                {
                    grpExecBillName.Visible = true;
                    tabItemType.Visible = true;
                    txtExecBillName.Text = r.Result1;
                    txtExecBillName.Tag = "Add";
                    #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��

                    //InitFp(txtExecBillName.Text, icont, ""); 
                    InitFp(objExecBill, icont);
                    this.SetTabVisible();
                    #endregion
                    icont++;
                    grpExecBillName.Visible = true;
                    //					txtExecBillName.Text = "";
                    grpExecBillD.Visible = true;
                }
                return 1;
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
                return -1;
            }
        }

        public int Copy()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Cut()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Delete()
        {
            try
            {
                if (this.txtExecBillName.Text.Trim() == "") return -1;
                if (this.neuSpread1.ActiveSheet == null) return -1;

                if (MessageBox.Show("�Ƿ�ɾ��ִ�е���" + this.txtExecBillName.Text + "��", "��ʾ", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return -1;

                if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag != null)
                {
                    #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
                    //if (oExecBill.DeleteExecBill(this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag.ToString()) != -1)
                    if (oExecBill.DeleteExecBill(((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag).ID) != -1)
                    #endregion
                    {
                        MessageBox.Show("ɾ���ɹ���");
                    }
                    else
                    {
                        MessageBox.Show("ɾ��ʧ��!" + oExecBill.Err);
                    }
                    #region {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
                    try
                    {
                        this.neuSpread1.Sheets.RemoveAt(this.neuSpread1.ActiveSheetIndex);
                    }
                    catch//catchֻ��һ��Sheetʱ���쳣
                    {

                    }
                    #endregion
                    icont--;
                    #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��

                    if (neuSpread1.ActiveSheetIndex != -1 && neuSpread1.ActiveSheet.Tag != null)
                    {
                        if (((Neusoft.FrameWork.Models.NeuObject)this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag).Memo == "1")
                        {//��Ŀ����ִ�е�
                            if (this.unDrugItemSelect != null)
                            {
                                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("�������³�ʼ����Ŀ�б���ȴ���"); ;
                                Application.DoEvents();
                                if (this.unDrugItemSelect.ucInputUndrug.GetUndrugDS() == -1) //this.unDrugItemSelect.Init() == -1)
                                {
                                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                                    return -1;
                                }
                                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    this.neuSpread1.Sheets.RemoveAt(this.neuSpread1.ActiveSheetIndex);
                    icont--;
                }
                this.RefreshList();
                if (this.neuSpread1.Sheets.Count > 0)
                {
                    for (int index = 0; index < this.neuSpread1.Sheets.Count; index++)
                    {
                        this.Filter(index);
                    }
                    txtExecBillName.Text = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName;
                }
                else
                {
                     //{46983F5B-E184-4b8b-B819-AA1C34993F1B} 
                    txtExecBillName.Text = "";
                }
                return 1;
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
                MessageBox.Show(Error);
                return -1;
            }
        }

        public int Export()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Import()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Init()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public bool IsDirty
        {
            get
            {
                //throw new Exception("The method or operation is not implemented.");
                return false;
            }
            set
            {
                //throw new Exception("The method or operation is not implemented.");
            }
        }

        public int Modify()
        {
            try
            {
                cResult r = new cResult();
                r.TextChanged += new TextChangedHandler(this.EventResultChanged);
                r.al = GetFpSheet();
                r.Result1 = txtExecBillName.Text;

                if (this.neuSpread1.ActiveSheetIndex < 0)
                    return -1;

                if (this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag == null)
                {
                    r.Result2 = "";
                }
                else
                {
                    r.Result2 = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].Tag.ToString();
                }
                ucBillAdd ba = new ucBillAdd(r);
                ba.alExecBill = this.alExecBill;
                Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(ba);

                if (r.Result1 != "")
                {
                    txtExecBillName.Text = r.Result1;
                    this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName = r.Result1;
                    grpExecBillName.Visible = true;
                }
                else
                {
                    txtExecBillName.Text = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName;
                }
                return 1;
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
                MessageBox.Show(Error);
                return -1;
            }
        }

        public int NextRow()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Paste()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int PreRow()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int Print()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int PrintConfig()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public int PrintPreview()
        {
            PrintInfo();
            return 1;
        }

        public int Query()
        {
            //throw new Exception("The method or operation is not implemented.");
            return 1;
        }

        public Neusoft.FrameWork.WinForms.Forms.IMaintenanceForm QueryForm
        {
            get
            {
                //throw new Exception("The method or operation is not implemented.");
                return null;
            }
            set
            {
                //throw new Exception("The method or operation is not implemented.");
            }
        }

        public int Save()
        {
            if (SaveBill() >= 0)
            {
                txtExecBillName.Text = this.neuSpread1.Sheets[this.neuSpread1.ActiveSheetIndex].SheetName;//= 
                MessageBox.Show("����ɹ���");
                return 1;
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ�");
                return 1;
            }
            return -1;
        }

        #endregion

        #region ��ҩƷ��Ŀִ�е�
        Neusoft.HISFC.Components.Order.Controls.ucUndrugItemSelect unDrugItemSelect = null;

        //��ҩƷ��Ŀ��ǩҳѡ��
        private void tvUndrugItem_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.neuSpread1.Sheets.Count == 0)
            {
                return;
            }

            #region addby xuewj 2010-9-2 {46983F5B-E184-4b8b-B819-AA1C34993F1B} ��ҩ��ִ�е�����Ŀά��
            if (this.neuSpread1.ActiveSheet.Tag == null)
            {
                MessageBox.Show("��ѡ��ִ�е�!");
                return;
            }
            if (this.tvUndrugItem.SelectedNode == null)
            {
                return;
            }
            #endregion

            if (this.tvUndrugItem.SelectedNode == null) return;

            Neusoft.FrameWork.Models.NeuObject execBill = this.neuSpread1.ActiveSheet.Tag as Neusoft.FrameWork.Models.NeuObject;

            if (execBill == null || !Neusoft.FrameWork.Function.NConvert.ToBoolean(execBill.Memo))
            {
                MessageBox.Show("��ѡ���ҩƷ��Ŀִ�е���");
                return;
            }

            if (e.Node.Parent == null)
            {
                MessageBox.Show("��ѡ��ҽ����Ŀ���");
                return;
            }

            if (this.unDrugItemSelect == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڳ�ʼ������ȴ���");
                Application.DoEvents();
                this.unDrugItemSelect = new ucUndrugItemSelect();
                Neusoft.HISFC.Models.Base.Employee empl = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
                this.unDrugItemSelect.NurseID = empl.Nurse.ID;

                if (this.unDrugItemSelect.Init() == -1)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    return;
                }
                this.unDrugItemSelect.ItemAllUpdate += new ucUndrugItemSelect.AllItemHandle(unDrugItemSelect_ItemAllUpdate);
                this.unDrugItemSelect.ItemOtherInsert += new ucUndrugItemSelect.ItemHandle(unDrugItemSelect_ItemOtherInsert);
                this.unDrugItemSelect.ItemInsert += new ucUndrugItemSelect.ItemHandle(unDrugItemSelect_ItemInsert);
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }

            this.unDrugItemSelect.BillNO = execBill.ID;
            this.unDrugItemSelect.MyOrderType = e.Node.Parent.Tag.ToString();
            this.unDrugItemSelect.MySysClass = e.Node.Tag.ToString();
            Neusoft.FrameWork.WinForms.Classes.Function.ShowControl(this.unDrugItemSelect);

        }

        //ѡ��ʣ����Ŀ�¼�
        int unDrugItemSelect_ItemOtherInsert(ArrayList items)
        {
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in items)
            {
                this.AddExecBill(order, this.neuSpread1.ActiveSheetIndex, true);
            }
            return 0;
        }

        //ѡ�񵥸�������Ŀ�¼�
        int unDrugItemSelect_ItemInsert(ArrayList items)
        {
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in items)
            {
                this.AddExecBill(order, this.neuSpread1.ActiveSheetIndex, true);
            }
            return 0;
        }

        //ѡ��ȫ����Ŀ�¼�
        int unDrugItemSelect_ItemAllUpdate(string orderType, string sysClass, ArrayList items)
        {
            Neusoft.FrameWork.Models.NeuObject activeExecBill = this.neuSpread1.ActiveSheet.Tag as Neusoft.FrameWork.Models.NeuObject;
            //�����
            ArrayList alOrder = new ArrayList();
            for (int i = 0; i < this.neuSpread1.Sheets.Count; i++)
            {
                if (i != this.neuSpread1.ActiveSheetIndex)
                {
                    Neusoft.FrameWork.Models.NeuObject execBill = this.neuSpread1.Sheets[i].Tag as Neusoft.FrameWork.Models.NeuObject;
                    if (execBill != null)
                    {
                        if (Neusoft.FrameWork.Function.NConvert.ToBoolean(execBill.Memo))
                        {
                            for (int j = this.neuSpread1.Sheets[i].RowCount - 1; j >= 0; j--)
                            {
                                if (this.neuSpread1.Sheets[i].Cells[j, 1].Tag.ToString() == orderType)
                                {
                                    object obj = this.neuSpread1.Sheets[i].Cells[j, 2].Tag as object;
                                    if (obj.ToString() == sysClass)
                                    {
                                        Neusoft.HISFC.Models.Order.Inpatient.Order order = new Neusoft.HISFC.Models.Order.Inpatient.Order();
                                        order.ID = activeExecBill.ID;
                                        order.Memo = "2";
                                        order.OrderType.ID = orderType;
                                        order.OrderType.Name = this.neuSpread1.Sheets[i].Cells[j, 1].Text;
                                        order.Item.SysClass.ID = sysClass;
                                        order.Item.ID = this.neuSpread1.Sheets[i].Cells[j, 3].Tag.ToString();
                                        order.Item.Name = this.neuSpread1.Sheets[i].Cells[j, 3].Text;
                                        alOrder.Add(order);
                                        this.neuSpread1.Sheets[i].Rows.Remove(j, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            items.AddRange(alOrder);
            //��ӵ���ǰsheet
            foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in items)
            {
                this.AddExecBill(order, this.neuSpread1.ActiveSheetIndex, true);
            }
            return 0;
        }

        //sheet�任
        private void neuSpread1_ActiveSheetChanged(object sender, EventArgs e)
        {
            this.SetTabVisible();
        }

        //tabҳ�任
        private void SetTabVisible()
        {
            Neusoft.FrameWork.Models.NeuObject execBill = this.neuSpread1.ActiveSheet.Tag as Neusoft.FrameWork.Models.NeuObject;
            if (execBill != null)
            {
                if (Neusoft.FrameWork.Function.NConvert.ToBoolean(execBill.Memo))
                {
                    if (this.tabItemType.TabPages.Contains(this.tabdrug))
                    {
                        this.tabItemType.TabPages.Remove(this.tabdrug);
                    }

                    if (this.tabItemType.TabPages.Contains(this.tabUndrag))
                    {
                        this.tabItemType.TabPages.Remove(this.tabUndrag);
                    }

                    if (!this.tabItemType.TabPages.Contains(this.tabUndrugItem))
                    {
                        this.tabItemType.TabPages.Add(this.tabUndrugItem);
                    }
                }
                else
                {
                    if (!this.tabItemType.TabPages.Contains(this.tabdrug))
                    {
                        this.tabItemType.TabPages.Add(this.tabdrug);
                    }

                    if (!this.tabItemType.TabPages.Contains(this.tabUndrag))
                    {
                        this.tabItemType.TabPages.Add(this.tabUndrag);
                    }

                    if (this.tabItemType.TabPages.Contains(this.tabUndrugItem))
                    {
                        this.tabItemType.TabPages.Remove(this.tabUndrugItem);
                    }
                }
            }
            else
            {
            }
        }
        #endregion
    }

    public delegate void TextChangedHandler(ArrayList s);

    public class cResult
    {
        public string Result1 = "";
        public string Result2 = "";

        public event TextChangedHandler TextChanged;
        public ArrayList al = new ArrayList();
        public void ChangeText(ArrayList al)
        {
            if (al != null)
                TextChanged(al);
        }
    }
}
