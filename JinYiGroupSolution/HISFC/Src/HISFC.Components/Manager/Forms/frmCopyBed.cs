using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Manager.Forms
{
    /// <summary>
    /// ��λά��
    /// </summary>
    public partial class frmCopyBed : Form
    {
        //����վ���  ������ʱ��
        protected string bedRoomNO = string.Empty;
        public string BedRoomNO
        {
            set
            {
                bedRoomNO = value;
            }
        }

        //����վ���  ������ʱ��
        protected string nurseStation = string.Empty;
        public string NurseStation
        {
            set
            {
                nurseStation = value;
            }
        }

        public frmCopyBed(bool isUpdate)
        {
            InitializeComponent();
            if (isUpdate)
            {
                txtBedNo.Enabled = true ;
                this.cmbNurse.Enabled = true ;
            }
            this.isUpdate = isUpdate;
            this.Init();
        }

        protected void Init()
        {
            Neusoft.HISFC.BizLogic.Manager.Department Dept = new Neusoft.HISFC.BizLogic.Manager.Department();
            Neusoft.HISFC.BizLogic.Manager.Constant content = new Neusoft.HISFC.BizLogic.Manager.Constant();
            this.cmbNurse.AddItems(Dept.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.N));//��ʿվ�б�
            this.cmdBedGrade.AddItems(content.GetList(Neusoft.HISFC.Models.Base.EnumConstant.BEDGRADE));//��λ�ȼ�
            this.cmbBedWeave.AddItems(Neusoft.HISFC.Models.Base.BedRankEnumService.List());//��λ����
            this.cmbBedStatus.AddItems(Neusoft.HISFC.Models.Base.BedStatusEnumService.List());//��λ״̬
        }
        protected bool isUpdate = false;
        public string Err = "";
        Neusoft.HISFC.BizLogic.Manager.Bed bed = new Neusoft.HISFC.BizLogic.Manager.Bed();
        protected int CheckValid()
        {
            if (this.cmbNurse.SelectedItem == null)
            {
                this.Err = "����վ�Ų�����";
                return -1;
            }
            if (this.txtBedNo.Text == "")
            {
                this.Err = "����Ϊ�գ�����д��";
                return -1;
            }
            if (txtBedNo.Enabled)
            {
                if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtBedNo.Text, 6))
                {
                    this.Err = "���Ź�������������д��";
                    return -1;
                }
            }
            
            if (txtBedNo.Text != "")
            {
                int temp = bed.IsExistBedNo(this.cmbNurse.SelectedItem.ID + txtBedNo.Text);
                if (temp == 0)
                {
                    //û��
                }
                else if (temp == 1)
                {
                    this.Err = "�Ѿ����ڴ�λ�� " + txtBedNo.Text;
                    txtBedNo.Focus();
                    return -1;
                }
            }

            if (this.txtWardNo.Text == "")
            {
                this.Err = "������Ϊ�գ�����д��";
                return -1;
            }
            if (this.cmdBedGrade.Text == "")
            {
                this.Err = "��λ�ȼ�Ϊ�գ���ѡ��";
                return -1;
            }
            if (this.cmbBedWeave.Text == "")
            {
                this.Err = "��λ����Ϊ�գ���ѡ��";
                return -1;
            }
            if (this.cmbBedStatus.Text == "")
            {
                this.Err = "��λ״̬Ϊ�գ���ѡ��";
                return -1;
            }
            
            if(!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtPhone.Text,14))
            {
                this.Err = "��λ�绰�Ϊ14λ,����������";
                return -1;
            }
            if (!Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtWardNo.Text, 10))
            {
                this.Err = "���ҺŹ���,����������";
                return -1;
            }
            return 0;
        }

        public void SetBedInfo(Neusoft.HISFC.Models.Base.Bed bedInfo)
        {
            if (bedInfo != null)
            {

                this.cmbNurse.Tag = bedInfo.NurseStation.ID;//��ʿվ���
                this.txtWardNo.Text = bedInfo.SickRoom.ID;//������
                this.txtBedNo.Text = bedInfo.ID;//������
                this.cmdBedGrade.Tag = bedInfo.BedGrade.ID.ToString();//�����ȼ�
                this.cmdBedGrade.Text = bedInfo.BedGrade.Name;
                this.cmbBedStatus.Tag = bedInfo.Status.ID.ToString();//����״̬
                this.cmbBedStatus.Text = bedInfo.Status.Name;
                this.cmbBedWeave.Tag = bedInfo.BedRankEnumService.ID.ToString();//��������
                this.cmbBedWeave.Text = bedInfo.BedRankEnumService.Name;
                this.txtPhone.Text = bedInfo.Phone;//�绰
                this.txtSort.Text = bedInfo.SortID.ToString();//˳���
                this.txtOwn.Text = bedInfo.OwnerPc.Trim();//����
                if (isUpdate)
                {
                    if (bedInfo.Status.ID.ToString() == "O" ||
                        bedInfo.Status.ID.ToString() == "R" ||
                        bedInfo.Status.ID.ToString() == "W") //ռ�ô�λ�����޸�״̬
                    {
                        this.cmbBedStatus.Enabled = true ;
                    }
                }
            }
        }
        Neusoft.HISFC.Models.Base.Bed BedInfo = null;
        public void GetBedInfo()
        {
            if (BedInfo == null)
            {
                BedInfo = new Neusoft.HISFC.Models.Base.Bed();
            }
            if (BedInfo.InpatientNO == "" || BedInfo.InpatientNO == null)
            {
                BedInfo.InpatientNO = "N";
            }
            BedInfo.NurseStation.ID = cmbNurse.Tag.ToString();//��ʿվ���
            
            BedInfo.SickRoom.ID = this.txtWardNo.Text.Trim();//������
            
            BedInfo.ID = txtBedNo.Text.Trim();//������
            BedInfo.BedGrade.ID = this.cmdBedGrade.Tag.ToString();//�����ȼ�
            BedInfo.Status.ID = this.cmbBedStatus.Tag.ToString();//����״̬
            BedInfo.BedRankEnumService.ID = this.cmbBedWeave.Tag.ToString();//��������
            BedInfo.Phone = txtPhone.Text.Trim();//�绰
            BedInfo.SortID = int.Parse(this.txtSort.Text);//˳���
            BedInfo.OwnerPc = this.txtOwn.Text.Trim();//����


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValid() != -1)
                {
                    
                    int iParm;

                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                    //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                    bed.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                    this.GetBedInfo();
                    //if (isUpdate)
                    //{
                    //    iParm = bed.UpdateBedInfo(BedInfo);
                    //}
                    //else
                    //{
                    iParm = bed.CreatBedInfo(BedInfo);
                    //}
                    if (iParm <= 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                        MessageBox.Show(this.bed.Err);
                    }
                    else
                    {
                        Neusoft.FrameWork.Management.PublicTrans.Commit();;
                        MessageBox.Show("����ɹ���");
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(Err);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                System.Windows.Forms.SendKeys.Send("{tab}");
            }
        }

        private void frmBedManager_Load(object sender, EventArgs e)
        {
            if (!isUpdate)
            {
                this.cmbNurse.Tag = this.nurseStation;
                this.txtWardNo.Text = this.bedRoomNO;
            }  
        }
    }
}