using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.RADT;
using Neusoft.HISFC.Models.Operation;

namespace Neusoft.HISFC.Components.Operation
{
    public partial class ucFee : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucFee()
        {
            InitializeComponent();
            this.Load += new EventHandler(ucFee_Load);
        }

        void ucFee_Load(object sender, EventArgs e)
        {
            if (!Environment.DesignMode)
                this.RefreshGroupList();
            //this.ucRegistrationTree1.ShowCanceled = false;
            //{9B275235-0854-461f-8B7B-C4FE6EC6CC0B}
            this.ucRegistrationTree1.ListType = this.ListType;
            this.ucFeeForm1.ListType = this.ListType;
            this.ucRegistrationTree1.Init();
            this.ucRegistrationTree1.ShowCanceled = false;

        }

        #region �ֶ�
        Neusoft.HISFC.BizProcess.Integrate.Manager groupManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        #endregion
        #region  ����

        #region {52AD1997-8BC0-4f22-97CA-2CF10B10C5F3} ���ò����ܹ���������п� by guanyx
        private int leftWidth = 80;

        [Category("�ؼ�����"), Description("��������� ")]
        public int LeftWidth
        {
            get
            {
                return this.ucFeeForm1.LeftWidth;
            }
            set
            {
                this.ucFeeForm1.LeftWidth = value;
            }
        }

        #endregion

        [Category("�ؼ�����"), Description("���øÿؼ����ص���Ŀ��� ҩƷ:drug ��ҩƷ undrug ����: all")]
        public Neusoft.HISFC.Components.Common.Controls.EnumShowItemType ������Ŀ���
        {
            get
            {
                return ucFeeForm1.������Ŀ���;
            }
            set
            {
                ucFeeForm1.������Ŀ��� = value;
            }
        }
        /// <summary>
        /// �ؼ�����
        /// </summary>
        [Category("�ؼ�����"), Description("��û������øÿؼ�����Ҫ����"), DefaultValue(1)]
        public Neusoft.HISFC.Components.Common.Controls.ucInpatientCharge.FeeTypes �ؼ�����
        {
            get
            {
                return this.ucFeeForm1.�ؼ�����;
            }
            set
            {
                this.ucFeeForm1.�ؼ����� = value;
            }
        }

        /// <summary>
        /// �Ƿ�����շѻ��߻���0���۵���Ŀ
        /// </summary>
        [Category("�ؼ�����"), Description("��û��������Ƿ�����շѻ��߻���"), DefaultValue(false)]
        public bool IsChargeZero
        {
            get
            {
                return this.ucFeeForm1.IsChargeZero;
            }
            set
            {
                this.ucFeeForm1.IsChargeZero = value;
            }
        }

        [Category("�ؼ�����"), Description("�Ƿ��ж�Ƿ��,Y���ж�Ƿ�ѣ�����������շ�,M���ж�Ƿ�ѣ���ʾ�Ƿ�����շ�,N�����ж�Ƿ��")]
        public Neusoft.HISFC.Models.Base.MessType MessageType
        {
            get
            {
                return this.ucFeeForm1.MessageType;
            }
            set
            {
                this.ucFeeForm1.MessageType = value;
            }
        }
        [Category("�ؼ�����"), Description("����Ϊ���Ƿ���ʾ��������")]
        public bool IsJudgeQty
        {
            get
            {
                return this.ucFeeForm1.IsJudgeQty;
            }
            set
            {
                this.ucFeeForm1.IsJudgeQty = value;
            }
        }
        [Category("�ؼ�����"), Description("ִ�п����Ƿ�Ĭ��Ϊ��½����")]
        public bool DefaultExeDeptIsDeptIn
        {
            get
            {
                return this.ucFeeForm1.DefaultExeDeptIsDeptIn;
            }
            set
            {
                this.ucFeeForm1.DefaultExeDeptIsDeptIn = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾ������{2C7FCD3D-D9B4-44f5-A2EE-A7E8C6D85576}
        /// </summary>
        [Category("�ؼ�����"), Description("�Ƿ���ʾ������"), DefaultValue(false)]
        public bool IsShowFeeRate
        {
            get { return this.ucFeeForm1.IsShowFeeRate; }
            set
            {

                this.ucFeeForm1.IsShowFeeRate = value;
            }
        }
        //{9B275235-0854-461f-8B7B-C4FE6EC6CC0B}
        [Category("�ؼ�����"), Description("�ؼ����ͣ��������շ�")]
        public ucRegistrationTree.EnumListType ListType
        {
            get
            {
                return this.ucRegistrationTree1.ListType;
            }
            set
            {
                this.ucRegistrationTree1.ListType = value;
            }
        }
        #region donggq--20101118--{E64BCA09-C312-4488-BED3-1B0149E8B3E9}

        [Category("�ؼ�����"), Description("���οؼ�����ͳ�ƴ�����𣬸�ʽ���£�'04','05'")]
        public string ArrFeeGate
        {
            get { return this.ucFeeForm1.ArrFeeGate; }
            set { this.ucFeeForm1.ArrFeeGate = value; }
        }

        [Category("�ؼ�����"), Description("�Ƿ���طѱ����οؼ�")]
        public bool IsShowItemTree
        {
            get { return this.ucFeeForm1.IsShowItemTree; }
            set { this.ucFeeForm1.IsShowItemTree = value; }
        }

        
        #endregion
        #endregion

        #region ����
        /// <summary>
        /// ���ɿ����շ������б�
        /// </summary>
        /// <returns></returns>
        private int RefreshGroupList()
        {
            this.tvGroup.Nodes.Clear();

            TreeNode root = new TreeNode();
            root.Text = "ģ��";
            root.ImageIndex = 22;
            root.SelectedImageIndex = 22;
            tvGroup.Nodes.Add(root);

            //{9F3CF1C0-AF96-4d17-96B1-6B34636A42A7}
            //ArrayList groups = this.groupManager.GetValidGroupList(Environment.OperatorDeptID);
            ArrayList groups = this.groupManager.GetValidGroupListByRoot(Environment.OperatorDeptID);
            if (groups != null)
            {
                foreach (Neusoft.HISFC.Models.Fee.ComGroup group in groups)
                {
                    TreeNode Node = new TreeNode();
                    Node.Text = group.Name;//ģ������
                    Node.Tag = group.ID;//ģ�����
                    Node.ImageIndex = 11;
                    Node.SelectedImageIndex = 11;
                    root.Nodes.Add(Node);
                    this.AddGroupsRecursion(Node, group);
                }
            }
            root.Expand();

            return 0;
        }

        //{9F3CF1C0-AF96-4d17-96B1-6B34636A42A7}
        private int AddGroupsRecursion(TreeNode parent, Neusoft.HISFC.Models.Fee.ComGroup group)
        {

            ArrayList al = this.groupManager.GetGroupsByDeptParent("1", group.deptCode, group.ID);
            if (al.Count == 0)
            {
                //TreeNode newNode = new TreeNode();
                //newNode.Tag = group;
                //newNode.Text = group.Name;// +"[" + group.ID + "]";
                //parent.Nodes.Add(newNode);

                //return -1;
            }
            else
            {

                foreach (Neusoft.HISFC.Models.Fee.ComGroup item in al)
                {
                    //if (item.ID == "aaa")
                    //{
                    //    MessageBox.Show("aaa");
                    //}
                    TreeNode newNode = new TreeNode();
                    //newNode.Tag = group;
                    //newNode.Text = group.Name;// +"[" + group.ID + "]";
                    //parent.Nodes.Add(newNode);
                    newNode.Tag = item;
                    newNode.Text = item.Name;
                    parent.Nodes.Add(newNode);
                    this.AddGroupsRecursion(newNode, item);
                }
            }


            return 1;
        }

        #endregion
        #region �¼�

        private void ucFeeForm1_Load(object sender, EventArgs e)
        {
           
        }

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("����", "����", 1, true, false, null);
            this.toolBarService.AddToolButton("ɾ��", "ɾ��", 5, true, false, null);
            return this.toolBarService;
        }
        protected override int OnQuery(object sender, object neuObject)
        {
            DateTime BeginTime = Convert.ToDateTime(this.neuDateTimePicker1.Value.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime EndTime = Convert.ToDateTime(this.neuDateTimePicker2.Value.ToString("yyyy-MM-dd") + " 23:59:59");
            //·־����2007-4-13 ȡ��С��ʼʱ���������ʱ��
            this.ucRegistrationTree1.RefreshList(BeginTime, EndTime);
            //this.ucRegistrationTree1.RefreshList(this.neuDateTimePicker1.Value, this.neuDateTimePicker2.Value);
            return base.OnQuery(sender, neuObject);
        }

        protected override int OnSave(object sender, object neuObject)
        {
            this.ucFeeForm1.Save();

            return base.OnSave(sender, neuObject);
        }


        protected override int OnPrint(object sender, object neuObject)
        {
            this.ucFeeForm1.Print();
            return base.OnPrint(sender, neuObject);
        }
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "����")
            {
                this.ucFeeForm1.Clear();
            }
            else if (e.ClickedItem.Text == "ɾ��")
            {
                this.ucFeeForm1.DelRow();
            }
            base.ToolStrip_ItemClicked(sender, e);
        }


        private void ucRegistrationTree1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.GetType() == typeof(OperationAppllication))
            {
                PatientInfo patient = (e.Node.Tag as OperationAppllication).PatientInfo;
                this.ucQueryInpatientNo1.Text = patient.PID.PatientNO;
                this.ucFeeForm1.OperationAppllication = e.Node.Tag as OperationAppllication;

            }
            else if (e.Node.Tag.GetType() == typeof(OperationRecord))
            {
                OperationAppllication application = (e.Node.Tag as OperationRecord).OperationAppllication;
                this.ucQueryInpatientNo1.Text = application.PatientInfo.PID.PatientNO;
                this.ucFeeForm1.OperationAppllication = application;
            }
            else if (e.Node.Tag.GetType() == typeof(AnaeRecord))
            {
                //OperationAppllication application = (e.Node.Tag as AnaeRecord).OperationAppllication;
                OperationAppllication application = (e.Node.Tag as AnaeRecord).OperationApplication;
                this.ucQueryInpatientNo1.Text = application.PatientInfo.PID.PatientNO;
                this.ucFeeForm1.OperationAppllication = application;
            }
        }

        private void tvGroup_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Neusoft.HISFC.Models.Fee.ComGroup comGroup = new Neusoft.HISFC.Models.Fee.ComGroup();

            try
            {
                comGroup = e.Node.Tag as Neusoft.HISFC.Models.Fee.ComGroup;
                if (comGroup == null)
                {
                    return;
                }
            }
            catch (Exception)
            {

                return;
            }

            this.ucFeeForm1.InsertGroup(comGroup.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ucQueryInpatientNo1_myEvent()
        {
            if (ucQueryInpatientNo1.InpatientNo == "")
            {
                MessageBox.Show("û�иû�����Ϣ!", "��ʾ");
                ucQueryInpatientNo1.Focus();
                return;
            }
            try
            {
                //��������ʵ����
                Neusoft.HISFC.Models.Operation.OperationAppllication apply = new Neusoft.HISFC.Models.Operation.OperationAppllication();
                //��ȡ�û���������Ϣ


                frmSelectOps sel = new frmSelectOps(ucQueryInpatientNo1.InpatientNo);
                if (sel.ShowDialog() == DialogResult.OK)
                {
                    //if (sel.IsReg) //�����Ѿ��ȹ��ǵĻ���---���ò�¼
                    //{
                    //    if (!string.IsNullOrEmpty(sel.OpNo))
                    //    {
                    //        string operationNo = sel.OpNo; //Environment.OperationManager.GetMaxByPatient(ucQueryInpatientNo1.InpatientNo);
                    //        if (operationNo == null || operationNo == "")
                    //        {
                    //            MessageBox.Show("�û���û�н�������!", "��ʾ");
                    //            ucQueryInpatientNo1.Focus();
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            //����������Ż������ʵ��
                    //            apply = Environment.OperationManager.GetOpsApp(operationNo);
                    //            //this.lblOpName.Text = string.Format("������{0}",apply.o
                    //        }
                    //        if (apply == null) return;
                    //        //��ȡ������Ŀ��Ϣ
                    //        ucFeeForm1.OperationAppllication = apply;
                    //    }
                    //}
                    //else   //����û�еǼǵ�
                    //{

                        
                    //}

                    if (!string.IsNullOrEmpty(sel.OpNo))
                    {
                        string operationNo = sel.OpNo; //Environment.OperationManager.GetMaxByPatient(ucQueryInpatientNo1.InpatientNo);
                        if (operationNo == null || operationNo == "")
                        {
                            MessageBox.Show("�û���û�н��������Ű�,���Ƚ��������Ű�!", "��ʾ");
                            ucQueryInpatientNo1.Focus();
                            return;
                        }
                        else
                        {
                            //����������Ż������ʵ��
                            apply = Environment.OperationManager.GetOpsApp(operationNo);
                            //this.lblOpName.Text = string.Format("������{0}",apply.o
                        }
                        if (apply == null) return;
                        //��ȡ������Ŀ��Ϣ
                        ucFeeForm1.IsReg = sel.IsReg;
                        ucFeeForm1.OperationAppllication = apply;
                    }
                }
                else { }
                //neusoft.HISFC.Management.Operator.Operator opMgr = new neusoft.HISFC.Management.Operator.Operator();

            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡ����������Ϣʱ����!" + e.Message, "��ʾ");
                ucQueryInpatientNo1.Focus();
                return;
            }
        }

        #endregion
    }
}
