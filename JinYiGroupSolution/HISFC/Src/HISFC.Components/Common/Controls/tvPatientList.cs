using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
namespace Neusoft.HISFC.Components.Common.Controls
{
   
    /// <summary>
    /// [��������: �����б���]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class tvPatientList : Neusoft.FrameWork.WinForms.Controls.NeuTreeView, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {

        /// <summary>
        /// �޲ι��캯��
        /// </summary>
        public tvPatientList()
        {
            // Windows.Forms ��׫д�����֧���������
            InitializeComponent();
            //��ʼ��
            this.Init();
        }

        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.IContainer components;
        private Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath iClinicPath = null;//add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}

        #region �����������ɵĴ���


        ///// <summary>
        ///// �вι��캯��
        ///// </summary>
        ///// <param name="container">�ӿ�</param>
        //public tvPatientList(System.ComponentModel.IContainer container)
        //{
        //    // Windows.Forms ��׫д�����֧���������
        //    container.Add(this);
        //    InitializeComponent();
        //    this.Init();//��ʼ��
        //    //
        //    // TODO: �� InitializeComponent ���ú�����κι��캯������
        //    //
        //}

        /// <summary> 
        /// ������������ʹ�õ���Դ��
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tvPatientList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "dir_close.bmp");
            this.imageList1.Images.SetKeyName(1, "dir_open.bmp");
            this.imageList1.Images.SetKeyName(2, "hourse.bmp");
            this.imageList1.Images.SetKeyName(3, "hourse1.bmp");
            this.imageList1.Images.SetKeyName(4, "36-2.bmp");
            this.imageList1.Images.SetKeyName(5, "36-3.bmp");
            this.imageList1.Images.SetKeyName(6, "47-2.gif");
            this.imageList1.Images.SetKeyName(7, "47-1.gif");
            this.imageList1.Images.SetKeyName(8, "82-2.bmp");
            this.imageList1.Images.SetKeyName(9, "82.bmp");
            this.imageList1.Images.SetKeyName(10, "40-2.bmp");
            this.imageList1.Images.SetKeyName(11, "40.bmp");
            this.imageList1.Images.SetKeyName(12, "097.GIF");
            this.imageList1.Images.SetKeyName(13, "blank.JPG");//{839D3A8A-49FA-4d47-A022-6196EB1A5715}
            // 
            // tvPatientList
            // 
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.LineColor = System.Drawing.Color.Black;
            this.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvPatientList_AfterCheck);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvPatientList_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvPatientList_MouseDown);
            this.ResumeLayout(false);

        }
        #endregion
        
        #region ö��
        /// <summary>
        /// ��ʾ������Ϣ-סԺ�ţ����ң���������Ժ״̬
        /// </summary>
        public enum enuShowType
        {
            None = 0,
            InpatientNo = 1,
            Dept = 3,
            Bed = 5,
            Status = 7
        }

        /// <summary>
        /// ��ʾ��Ϣ����ǰ�棬����(���������෴�ķ���)
        /// </summary>
        public enum enuShowDirection
        {
            Ahead,
            Behind
        }

        /// <summary>
        /// ѡ������
        /// </summary>
        public enum enuChecked
        {
            None,
            Radio,
            MultiSelect
        }
        #endregion

        #region ����
        private ArrayList myPatients = new ArrayList();
        private enuShowType myShowType = enuShowType.Bed;   //Ĭ����ʾ����
        private enuChecked myChecked = enuChecked.None;     //Ĭ�ϲ���ʾCheckBox
        private enuShowDirection myDirection = enuShowDirection.Ahead; //Ĭ��������Ϣ����ǰ��,�������ں���
        private bool bIsShowNewPatient = true;  //Ĭ������ǵ�����Ժ�Ļ���,��ʾ���¡�
        private bool bControlChecked = false;
        private DateTime dtToday;
        protected bool bIsShowPatientNo = true;
        protected bool bIsShowCount = true;
        public int RootImageIndex = 0;
        public int RootSelectedImageIndex = 1;
        public int BranchImageIndex = 2;
        public int BranchSelectedImageIndex = 3;
        public int MaleImageIndex = 4;
        public int MaleSelectedImageIndex = 5;
        public int FemaleImageIndex = 6;
        public int FemaleSelectedImageIndex = 7;
        public int BabyImageIndex = 8;
        public int GirlImageIndex = 10;
        public int LeaveImageIndex = 12;
        public int BlankImageIndex = 13;//{839D3A8A-49FA-4d47-A022-6196EB1A5715}
      //  ZZLocal.HISFC.BizLogic.LocalManager local = new ZZLocal.HISFC.BizLogic.LocalManager();//����ҵ���
        #endregion

        #region ����
        /// <summary>
        /// �Ƿ���ʾ�µĻ���
        /// </summary>
        public bool IsShowNewPatient
        {
            get
            {
                return this.bIsShowNewPatient;
            }
            set
            {
                this.bIsShowNewPatient = value;
            }
        }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        public enuShowType ShowType
        {
            get
            {
                return this.myShowType;
            }
            set
            {
                this.myShowType = value;
            }
        }

        /// <summary>
        /// �������飬�����ָ�object
        /// </summary>
        public void SetPatient(ArrayList alPatients)
        {
            if (alPatients == null) return;
            this.myPatients = alPatients;
            this.RefreshList();
        
        }

        /// <summary>
        /// ��ʾѡ������
        /// </summary>
        public enuChecked Checked
        {
            get
            {
                return this.myChecked;
            }
            set
            {
                this.myChecked = value;
                if (this.myChecked == enuChecked.MultiSelect)
                {
                    this.CheckBoxes = true;
                }
                else
                {
                    this.CheckBoxes = false;
                }
            }
        }

        /// <summary>
        /// ��ʾ������Ϣλ��
        /// </summary>
        public enuShowDirection Direction
        {
            get
            {
                return this.myDirection;
            }
            set
            {
                this.myDirection = value;
            }
        }

        /// <summary>
        /// �Ƿ���ʾnodeCount
        /// </summary>
        public bool IsShowCount
        {
            get
            {
                return this.bIsShowCount;
            }
            set
            {
                this.bIsShowCount = value;
            }
        }
        /// <summary>
        /// �Ƿ���ʾtooltipסԺ��
        /// </summary>
        public bool IsShowPatientNo
        {
            get
            {
                return this.bIsShowPatientNo;
            }
            set
            {
                this.bIsShowPatientNo = value;
            }
        }
        // by zlw 2006-5-1
        private bool bIsShowContextMenu = true;
        /// <summary>
        /// �Ƿ񵯳��Ҽ��˵�����ʾ��������,Ĭ��ֵΪ true ��ʾ 
        /// </summary>
        public bool IsShowContextMenu
        {
            get
            {
                return this.bIsShowContextMenu;
            }
            set
            {
                this.bIsShowContextMenu = value;
            }
        }

        /// <summary>
        /// ˢ���б�
        /// </summary>
        private void RefreshList()
        {
            this.Nodes.Clear();
            int Branch = 0;
            if (this.myPatients.Count == 0) this.AddRootNode();
            for (int i = 0; i < this.myPatients.Count; i++)
            {
                System.Windows.Forms.TreeNode newNode = new System.Windows.Forms.TreeNode();
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                //����ΪҶ
                if (this.myPatients[i].GetType().ToString() == "Neusoft.HISFC.Models.RADT.PatientInfo")
                {
                    try
                    {
                        Neusoft.HISFC.Models.RADT.PatientInfo PatientInfo = (Neusoft.HISFC.Models.RADT.PatientInfo)this.myPatients[i];
                        obj.ID = PatientInfo.PID.PatientNO;
                        obj.Name = PatientInfo.Name;
                        try
                        {
                            obj.Memo = PatientInfo.PVisit.PatientLocation.Bed.ID;
                            try
                            {	//���
                                if (PatientInfo.PVisit.PatientLocation.Bed.Status.ID.ToString() == "R")
                                {
                                    obj.Name = obj.Name + "����١�";
                                }
                            }
                            catch { }
                        }
                        catch
                        {//�޲�����Ϣ
                        }
                        obj.User01 = PatientInfo.PVisit.PatientLocation.Dept.Name;
                        obj.User02 = PatientInfo.PVisit.InState.Name;
                        obj.User03 = PatientInfo.Sex.ID.ToString();
                        //��Ժ������24Сʱ,��ʾ(��)
                        if (this.bIsShowNewPatient)
                        {
                            if (dtToday < PatientInfo.PVisit.InTime.Date.AddDays(1)) obj.Name = obj.Name + "(��)";
                        }
                        this.AddTreeNode(Branch, obj, PatientInfo);
                    }
                    catch { }
                }
                else if (this.myPatients[i].GetType().ToString() == "Neusoft.HISFC.Models.RADT.Patient")
                {
                    Neusoft.HISFC.Models.RADT.Patient PatientInfo = (Neusoft.HISFC.Models.RADT.Patient)this.myPatients[i];
                    obj.ID = PatientInfo.PID.PatientNO;
                    obj.Name = PatientInfo.Name;
                    obj.Memo = "";
                    obj.User01 = "";
                    obj.User02 = "";
                    obj.User03 = PatientInfo.Sex.ID.ToString();
                    this.AddTreeNode(Branch, obj, PatientInfo);
                }
                else if (this.myPatients[i].GetType().ToString() == "Neusoft.FrameWork.Models.NeuObject")
                {
                    obj = (Neusoft.FrameWork.Models.NeuObject)this.myPatients[i];
                    this.AddTreeNode(Branch, obj, obj);
                }
                else
                {//Ϊ��
                    //�ָ��ַ��� text|tag ��ʶ���
                    string all = this.myPatients[i].ToString();
                    string[] s = all.Split('|');

                    newNode.Text = s[0];

                    try
                    {
                        newNode.Tag = s[1];
                    }
                    catch { newNode.Tag = ""; }
                    try
                    {
                        newNode.ImageIndex = this.BranchImageIndex;
                        newNode.SelectedImageIndex = this.BranchSelectedImageIndex;
                    }
                    catch { }
                    Branch = this.Nodes.Add(newNode);
                }
            }
            if (this.bIsShowCount)
            {
                foreach (System.Windows.Forms.TreeNode node in this.Nodes)
                {

                    if (node.Tag == null || node.Tag.GetType().ToString() == "System.String")
                    {//���
                        int count = 0;
                        count = node.GetNodeCount(false);
                        node.Text = node.Text + "(" + count.ToString() + ")";
                    }
                }
            }
            this.ExpandAll();
            try//wolf added ensure node visible 
            {
                if (this.SelectedNode == null)
                {
                    try
                    {
                        this.SelectedNode = this.Nodes[0];
                    }
                    catch { }
                }
                this.SelectedNode.EnsureVisible();
            }
            catch { }

        }


        /// <summary>
        /// ɾ���ڵ�
        /// </summary>
        /// <param name="branch">�����ڵ�����</param>
        /// <param name="nodeIndex">Ҫɾ���ڵ�����</param>
        public void DeleteNode(int branch, int nodeIndex)
        {
            //�Ƴ��ڵ�
            this.Nodes[branch].Nodes[nodeIndex].Remove();
        }


        /// <summary>
        /// ���ݴ������,�޸�ָ���Ľڵ���Ϣ
        /// </summary>
        /// <param name="node">���޸ĵĽڵ�</param>
        /// <param name="nodeTextInfo">�ڵ���Ϣ</param>
        /// <param name="nodeTag">�ڵ��tag����</param>
        public void ModifiyNode(System.Windows.Forms.TreeNode node, Neusoft.FrameWork.Models.NeuObject nodeTextInfo, object nodeTag)
        {
            try
            {
                //���ɽڵ���Ϣ
                this.CreateNodeInfo(nodeTextInfo, nodeTag, ref node);
            }
            catch { }
        }


        /// <summary>
        /// ���ݴ������,�޸�ָ���Ľڵ���Ϣ
        /// </summary>
        /// <param name="node">���޸ĵĽڵ�</param>
        /// <param name="patientInfo">������Ϣ</param>
        public void ModifiyNode(System.Windows.Forms.TreeNode node, Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            try
            {
                Neusoft.FrameWork.Models.NeuObject nodeTextInfo = new Neusoft.FrameWork.Models.NeuObject();
                nodeTextInfo.ID = patientInfo.PID.PatientNO;
                nodeTextInfo.Name = patientInfo.Name;
                try
                {
                    nodeTextInfo.Memo = patientInfo.PVisit.PatientLocation.Bed.ID;
                }
                catch
                {//�޲�����Ϣ
                }

                nodeTextInfo.User01 = patientInfo.PVisit.PatientLocation.Dept.Name;
                nodeTextInfo.User02 = patientInfo.PVisit.InState.Name;
                nodeTextInfo.User03 = patientInfo.Sex.ID.ToString();
                if (this.bIsShowNewPatient)
                {
                    if (dtToday.Date == patientInfo.PVisit.InTime.Date)
                        nodeTextInfo.Name = nodeTextInfo.Name + "(��)";
                }

                //����ڵ������,ָ��Ҫ�޸ĵĽڵ�
                this.ModifiyNode(node, nodeTextInfo, patientInfo);
            }
            catch { }
        }


        /// <summary>
        /// ���ݴ������Ϣ,����һ���½ڵ�
        /// </summary>
        /// <param name="branch">һ���ڵ�����</param>
        /// <param name="nodeTextInfo">�ڵ���Ϣ</param>
        /// <param name="nodeTag">�ڵ�Tag����</param>
        public void AddTreeNode(int branch, Neusoft.FrameWork.Models.NeuObject nodeTextInfo, object nodeTag)
        {
            System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
            //����Ҫ��ӵĽڵ�
            this.CreateNodeInfo(nodeTextInfo, nodeTag, ref node);

            //ָ����ǰѡ�еĽڵ�
            try
            {
                //this.SelectedNode=this.Nodes[branch];
                //�ڸ����ڵ��������½ڵ�
                this.Nodes[branch].Nodes.Add(node);
            }
            catch
            {
                this.Nodes.Add(new System.Windows.Forms.TreeNode("����"));
                //this.SelectedNode=this.Nodes[0];
                //��ѡ�еĽڵ��������½ڵ�
                this.Nodes[0].Nodes.Add(node);
            }

            //��ѡ�еĽڵ��������½ڵ�
            //this.SelectedNode.Nodes.Add(node);

        }


        /// <summary>
        /// ���ݴ������,�����½ڵ�
        /// </summary>
        /// <param name="branch">һ���ڵ�����</param>
        /// <param name="patientInfo">������Ϣ</param>
        public void AddTreeNode(int branch, Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            try
            {
                
                //�ڵ���Ϣ
                Neusoft.FrameWork.Models.NeuObject nodeTextInfo = new Neusoft.FrameWork.Models.NeuObject();
                nodeTextInfo.ID = patientInfo.PID.PatientNO;				//סԺ��
                nodeTextInfo.Name = patientInfo.Name;								//��������
                nodeTextInfo.Memo = patientInfo.PVisit.PatientLocation.Bed.ID;		//����
                nodeTextInfo.User01 = patientInfo.PVisit.PatientLocation.Dept.Name;	//��������
                nodeTextInfo.User02 = patientInfo.PVisit.InState.Name;				//��Ժ״̬
                nodeTextInfo.User03 = patientInfo.Sex.ID.ToString();		//�Ա�
                //���ݻ��ߵ���Ժ����,�ж��Ƿ���ʾ"(��)"
                if (this.bIsShowNewPatient)
                {
                    if (dtToday.Date == patientInfo.PVisit.InTime.Date)
                        nodeTextInfo.Name = nodeTextInfo.Name + "(��)";
                }

                //����ڵ������,ָ��Ҫ�޸ĵĽڵ�
                this.AddTreeNode(branch, nodeTextInfo, patientInfo);
            }
            catch { }
        }


        /// <summary>
        /// ���ݴ������,�����ڵ���Ϣ
        /// </summary>
        /// <param name="neuObj">�ڵ�Text��Ϣ:obj.id ,name,memo=bed,user01=dept,user02=status user03=sex </param>
        /// <param name="obj">�ڵ��Tag����</param>
        /// <param name="node">���ز���:�ڵ�</param>
        private void CreateNodeInfo(Neusoft.FrameWork.Models.NeuObject neuObj, object obj, ref System.Windows.Forms.TreeNode node)
        {
            //�������ڵ�Ϊ��,���½�һ���ڵ�
            if (node == null)
                node = new System.Windows.Forms.TreeNode();

            #region ���ɽڵ��Text
            string strText = neuObj.Name; //��������
            string strMemo = "";
            switch (this.myShowType.GetHashCode())
            {
                case 1:
                    //סԺ��
                    strMemo = "��" + neuObj.ID + "��";
                    break;
                case 3:
                    //����
                    if (neuObj.User01 != "" || neuObj.User01 != null) strMemo = "��" + neuObj.User01 + "��";
                    break;
                case 5:
                    //����
                    if (neuObj.Memo != "" || neuObj.Memo != null)
                    {
                        strMemo = neuObj.Memo;

                        if (strMemo.Length > 4)
                        {
                            strMemo = strMemo.Substring(4);
                        }
                        #region
                        
                        #endregion
                        strMemo = "��" + strMemo + "��";
                    }
                    break;
                case 7:
                    //״̬
                    strMemo = "��" + neuObj.User02 + "��";
                    break;
                case 4:
                    //����+סԺ��
                    strMemo = "��" + neuObj.User01 + "��" + "��" + neuObj.ID + "��";
                    break;
                case 6:
                    //����+סԺ��
                    if (neuObj.Memo != "" || neuObj.Memo != null)
                        strMemo = "��" + neuObj.Memo.Substring(4) + "��" + "��" + neuObj.ID + "��";
                    else
                        strMemo = "��" + neuObj.ID + "��";
                    break;
                case 8:
                    //סԺ��+״̬
                    strMemo = "��" + neuObj.ID + "��" + "��" + neuObj.User02 + "��";
                    break;
                case 10:
                    //����+״̬
                    strMemo = "��" + neuObj.User01 + "��" + "��" + neuObj.User02 + "��";
                    break;
                case 12:
                    //����+״̬
                    if (neuObj.Memo != "" || neuObj.Memo != null)
                        strMemo = "��" + neuObj.Memo.Substring(4) + "��" + "��" + neuObj.User02 + "��";
                    else
                        strMemo = "��" + neuObj.User02 + "��";
                    break;
                default:
                    strMemo = "";
                    break;
            }

            //������ʾλ��,ȷ�����յ�����
            if (this.myDirection == enuShowDirection.Behind)
            {
                strText = strText + strMemo;
            }
            else
            {
                strText = strMemo + strText;
            }
            node.Text = strText;
            #endregion

            //�����ڵ��ImageIndex
            switch (neuObj.User03)
            {
                case "F":
                    //��
                    if (((Neusoft.FrameWork.Models.NeuObject)obj).ID.IndexOf("B") > 0)
                        node.ImageIndex = this.GirlImageIndex;	//Ӥ��Ů
                    else
                        node.ImageIndex = this.FemaleImageIndex;	//����Ů
                    break;
                case "M":
                    if (((Neusoft.FrameWork.Models.NeuObject)obj).ID.IndexOf("B") > 0)
                        node.ImageIndex = this.BabyImageIndex;	//Ӥ����
                    else
                        node.ImageIndex = this.MaleImageIndex;	//������
                    break;
                default:
                    node.ImageIndex = this.MaleImageIndex;
                    break;
            }
            //�����ڵ��SelectedImageIndex
            node.SelectedImageIndex = node.ImageIndex + 1;
            #region addby xuewj 2010-10-1 {82580D4C-4299-4903-B631-10C37626A9FB} ҽ�������Բ�ͬ����ɫ��ʾ
            if (obj is Neusoft.HISFC.Models.RADT.PatientInfo)
            {
                #region ��ͯ��������ʾ{5624C940-0158-45a1-8FE6-7A5EF53E0BEF} by xizf20110214
                Neusoft.HISFC.Models.RADT.PatientInfo temp = (Neusoft.HISFC.Models.RADT.PatientInfo)obj;
                if (temp.Pact.ID == "13")
                {
                    //if (local.IsChildrenEase(temp.ID))
                    //{
                    //    node.ForeColor = Neusoft.HISFC.BizProcess.Integrate.Function.GetPactColor("04");
                    //}
                    //else {
                        node.ForeColor = Neusoft.HISFC.BizProcess.Integrate.Function.GetPactColor(temp.Pact.ID);
                    //}
                    
                }
                else {
                    node.ForeColor = Neusoft.HISFC.BizProcess.Integrate.Function.GetPactColor(((Neusoft.HISFC.Models.RADT.PatientInfo)obj).Pact.ID);
                }
               #endregion
                #region addby xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}
                if (this.iClinicPath != null)
                {
                    bool isInPath = iClinicPath.PatientIsSelectedPath(((Neusoft.HISFC.Models.RADT.PatientInfo)obj).ID);
                    if (isInPath)
                    {
                        node.NodeFont = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        node.Text = "*" + node.Text;
                    }
                }
                #endregion
            } 
            #endregion
            //�����ڵ��Tag����
            node.Tag = obj;
        }


        /// <summary>
        /// ���ݻ�����Ϣ��һ���ڵ�,�������ӽڵ��л������ڵĽڵ�Index
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="patientInfo"></param>
        /// <returns></returns>
        public System.Windows.Forms.TreeNode FindNode(int branch, Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            Neusoft.HISFC.Models.RADT.PatientInfo findPatient = null;
            foreach (System.Windows.Forms.TreeNode node in this.Nodes[branch].Nodes)
            {
                //ȡ�ڵ��ϵĻ�����Ϣ
                findPatient = node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
                //�������ת��Ϊ������Ϣ,�����������һ���ڵ�
                if (findPatient == null) continue;
                //����ҵ�,�򷵻ش˽ڵ�
                if (findPatient.ID == patientInfo.ID) return node;
            }

            //���û���ҵ�,�򷵻�null
            return null;
        }

        /// <summary>
        /// �������ﻼ����Ϣ��һ���ڵ�,�������ӽڵ��л������ڵĽڵ�Index
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="patientInfo"></param>
        /// <returns></returns>
        public System.Windows.Forms.TreeNode FindNode(int branch, Neusoft.HISFC.Models.Registration.Register patientInfo)
        {
            Neusoft.HISFC.Models.Registration.Register findPatient = null;
            foreach (System.Windows.Forms.TreeNode node in this.Nodes[branch].Nodes)
            {
                //ȡ�ڵ��ϵĻ�����Ϣ
                findPatient = node.Tag as Neusoft.HISFC.Models.Registration.Register;
                //�������ת��Ϊ������Ϣ,�����������һ���ڵ�
                if (findPatient == null) continue;
                //����ҵ�,�򷵻ش˽ڵ�
                if (findPatient.ID == patientInfo.ID) return node;
            }

            //���û���ҵ�,�򷵻�null
            return null;
        }

        /// <summary>
        /// ��Ӹ��ڵ�
        /// </summary>
        private void AddRootNode()
        {
            this.Nodes.Add(new System.Windows.Forms.TreeNode("����"));
        }


        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            this.ImageList = this.imageList1;
            this.HideSelection = false;

            try
            {
                if (this.IsShowContextMenu == true)//��ʾ����
                {
                    // �����Ҽ��˵�  by zlw 2006-5-1
                    System.Windows.Forms.ContextMenu cmPatientPro = new System.Windows.Forms.ContextMenu();
                    System.Windows.Forms.MenuItem miPatientPro = new System.Windows.Forms.MenuItem();
                    #region addby xuewj 2010-9-28 ��ѯ���߷�����ϸ {98057398-9233-4aec-8FAF-662A8E82BF74}
                    System.Windows.Forms.MenuItem miPatientFeeQuery = new System.Windows.Forms.MenuItem();
                    cmPatientPro.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { miPatientPro, miPatientFeeQuery }); 
                    #endregion

                    miPatientPro.Text = "�鿴������Ϣ";
                    miPatientFeeQuery.Text = "�鿴���߷�����ϸ";//addby xuewj 2010-9-28 ��ѯ���߷�����ϸ {98057398-9233-4aec-8FAF-662A8E82BF74}
                    this.ContextMenu = cmPatientPro;

                    miPatientPro.Click += new System.EventHandler(this.miPatientPro_Click);
                    miPatientFeeQuery.Click += new EventHandler(miPatientFeeQuery_Click);
                }

                Neusoft.HISFC.BizLogic.Manager.Spell dataBase = new Neusoft.HISFC.BizLogic.Manager.Spell();
                this.dtToday = dataBase.GetDateTimeFromSysDateTime();

                this.InitInterface();
            }
            catch
            {
                this.dtToday = DateTime.Today;
            }
        }

        #region addby xuewj 2010-9-28 ��ѯ���߷�����ϸ {98057398-9233-4aec-8FAF-662A8E82BF74}
        /// <summary>
        /// �鿴���߷�����ϸ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miPatientFeeQuery_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.RADT.PatientInfo findPatient = null;
            System.Windows.Forms.TreeNode node = this.SelectedNode;
            if (node == null) return;
            findPatient = node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
            if (findPatient == null)
            {
                return;
            }
            else
            {
                ucPatientFeeQuery ucPatientpro = new ucPatientFeeQuery();
                ucPatientpro.InitDataTable();
                ucPatientpro.InitContr();
                ucPatientpro.PatientInfo = findPatient;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucPatientpro);
            }
        } 
        #endregion
        #endregion

        #region �¼�
        private void miPatientPro_Click(object sender, System.EventArgs e)
        {
            Neusoft.HISFC.Models.RADT.PatientInfo findPatient = null;
            System.Windows.Forms.TreeNode node = this.SelectedNode;
            #region {93F17D80-F559-45f6-B380-23A8CC8A936D}
            if (node == null) return;
            #endregion
            findPatient = node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
            if (findPatient == null)
            {
                return;
            }
            else
            {
                ucPatientProperty ucPatientpro = new ucPatientProperty();
                ucPatientpro.Patient = findPatient;
                Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucPatientpro);
            }
        }

        private void tvPatientList_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (this.CheckBoxes && this.bControlChecked == false)
            {
                foreach (System.Windows.Forms.TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }
        System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();
        private void tvPatientList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.bIsShowPatientNo)
            {
                System.Windows.Forms.TreeNode node = null;
                Neusoft.HISFC.Models.RADT.PatientInfo info = null;
                System.Drawing.Point p = new System.Drawing.Point(e.X, e.Y);
                node = this.GetNodeAt(p);
                if (node == null) return;
                info = node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
                if (info == null) return;
                if(this.toolTip1.GetToolTip(this)!= info.PID.ID)
                    this.toolTip1.SetToolTip(this, info.PID.ID);
            }
        }

        private void tvPatientList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (this.bIsShowContextMenu == false && this.ContextMenu!=null)
                {
                    this.ContextMenu = null;
                    return;
                }

                System.Windows.Forms.TreeNode node = this.GetNodeAt(e.X, e.Y);
                this.SelectedNode = node;
            }

        }
        #endregion

        #region add by xuewj 2010-10-19 �ٴ�·���ӿ� {10962AE3-C0B9-4cf7-91B6-CA956C1AFC2D}

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] t = new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath) };
                return t;
            }
        }

        #endregion
        
        /// <summary>
        /// ��ʼ���ӿ�
        /// </summary>
        public void InitInterface()
        {
            if (this.iClinicPath == null)
            {
                this.iClinicPath = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(Neusoft.HISFC.Components.Common.Controls.tvPatientList), typeof(Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath)) as Neusoft.HISFC.BizProcess.Interface.ClinicPath.IClinicPath;
            }
        } 

        #endregion
    }
}
