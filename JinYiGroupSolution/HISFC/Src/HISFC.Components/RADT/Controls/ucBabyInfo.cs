using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.Components.RADT.Controls
{
    /// <summary>
    /// [��������: Ӥ���Ǽ����]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2006-11-30]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucBabyInfo : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        /// <summary>
        /// ����
        /// </summary>
        public ucBabyInfo()
        {
            InitializeComponent();
        }

        #region ����
        Neusoft.HISFC.BizLogic.RADT.InPatient inpatientManager = new Neusoft.HISFC.BizLogic.RADT.InPatient();
        Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        Neusoft.HISFC.Models.RADT.PatientInfo MomInfo = null;
        Neusoft.HISFC.Models.RADT.PatientInfo BabyInfo = null;
        bool isNew = false;
        private string InpatientNo = "";

        #region {FEA519C4-2379-40a9-8271-829A76E04EF6}

        private int babyNo = 0;

        #endregion

        #region addby xuewj 2010-10-9 Ӥ���ǼǸ��ݲ����Ƿ����Ӥ�������� {4759B4FB-BA7B-4ec1-BCAA-BB456A714776}

        /// <summary>
        /// �Ƿ����Ӥ��������
        /// </summary>
        private string isUpdatePatientAlert = "0";
        /// <summary>
        /// Ӥ���Ǽ�ʱ�ľ�����
        /// </summary>
        private decimal patientAlert = 0m;

        /// <summary>
        /// ���Ʋ���������
        /// </summary>
        private Neusoft.FrameWork.Management.ControlParam controlMgr = new Neusoft.FrameWork.Management.ControlParam();

        #endregion

        #endregion

        #region ����
        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        private void InitControl()
        {
            #region addby xuewj 2010-10-9 Ӥ���ǼǸ��ݲ����Ƿ����Ӥ�������� {4759B4FB-BA7B-4ec1-BCAA-BB456A714776}

            this.isUpdatePatientAlert = this.controlMgr.QueryControlerInfo("200215");
            string babyAlert = this.controlMgr.QueryControlerInfo("200216");
            if (!string.IsNullOrEmpty(babyAlert))
            {
                this.patientAlert = Neusoft.FrameWork.Function.NConvert.ToDecimal(babyAlert);
            }
            #endregion
            this.cmbSex.AddItems(Neusoft.HISFC.Models.Base.SexEnumService.List());				//ȡ�Ա��б�
            this.cmbBlood.AddItems(Neusoft.HISFC.Models.RADT.BloodTypeEnumService.List());	//ȡѪ���б�
            this.cmbBlood.IsListOnly = true;
            this.cmbSex.IsListOnly= true;
            this.dtBirthday.Value = this.inpatientManager.GetDateTimeFromSysDateTime();		//Ĭ��Ӥ����������Ϊϵͳʱ��
            this.dtOperatedate.Value = dtBirthday.Value;	//Ĭ�ϲ�������Ϊϵͳʱ��
            ClearInfo();
        }

        
        /// <summary>
        /// ���û�����Ϣ���ؼ�
        /// </summary>
        /// <param name="patientInfo"></param>
        private void ShowBabyInfo(Neusoft.HISFC.Models.RADT.PatientInfo patientInfo)
        {
            //���û�л�����Ϣ,������
            if (patientInfo == null)
            {
                this.ClearInfo();
                return;
            }
            this.txtInpatientNo.Text = patientInfo.PID.ID;	//סԺ��
            this.txtInpatientNo.Tag = patientInfo.User01;			//�������
            this.txtName.Text = patientInfo.Name;					//����
            this.cmbSex.Tag = patientInfo.Sex.ID;			//�Ա�
            this.cmbBlood.Tag = patientInfo.BloodType.ID;	//Ѫ��
            this.txtHeight.Text = patientInfo.Height;		//���
            this.txtWeight.Text = patientInfo.Weight;			//����
            this.dtBirthday.Value = patientInfo.Birthday;		//��������
            this.dtOperatedate.Text = patientInfo.User03;			//�Ǽ�����
            this.InpatientNo = patientInfo.ID; //סԺ��ˮ��
        }

        /// <summary>
        /// ��տؼ�
        /// </summary>
        private void ClearInfo()
        {
            
            this.txtInpatientNo.Text = "";	//�������
            this.txtInpatientNo.Tag = "";	//
            this.txtName.Text = "";		//����
            this.cmbSex.Tag = "M";		//Ĭ���Ա�Ϊ����
            this.cmbBlood.Tag = "";	//Ѫ��Ϊ��
            this.cmbBlood.Text = "";	//Ѫ��Ϊ��
        
            try
            {
                this.cmbDept.Text = this.MomInfo.PVisit.PatientLocation.Dept.Name;	//Ӥ������=ĸ�׿���
                this.cmbDept.Tag = this.MomInfo.PVisit.PatientLocation.Dept.ID;	//Ӥ������=ĸ�׿���
                
            }
            catch { }
            this.txtHeight.Text = "";   //���
            this.txtWeight.Text = "";		//����
            this.dtBirthday.Value = this.inpatientManager.GetDateTimeFromSysDateTime();		//��������Ĭ�ϵ���
            this.dtOperatedate.Value = this.inpatientManager.GetDateTimeFromSysDateTime();	//����ʱ��
            this.BabyInfo = null;
                        
        }


        /// <summary>
        /// ȡӤ����Ϣ
        /// </summary>
        /// <returns></returns>
        private Neusoft.HISFC.Models.RADT.PatientInfo GetBabyInfo()
        {			//�������
            if (this.BabyInfo == null)
            {
                //���������Ӥ����û����������,������Ӥ������,��ȡ��Ӥ�������
                this.BabyInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();
                //���Ҵ�λ��Ϣ
                this.BabyInfo.PVisit.PatientLocation = this.MomInfo.PVisit.PatientLocation.Clone();
                //ȡ�ؼ���סԺҽ��
                this.BabyInfo.PVisit.AdmittingDoctor = this.MomInfo.PVisit.AdmittingDoctor.Clone();
                //ȡ�ؼ�������ҽ��
                this.BabyInfo.PVisit.AttendingDoctor = this.MomInfo.PVisit.AttendingDoctor.Clone();
                //ȡ�ؼ�������ҽ��
                this.BabyInfo.PVisit.ConsultingDoctor = this.MomInfo.PVisit.ConsultingDoctor.Clone();
                //ȡ�ؼ������λ�ʿ
                this.BabyInfo.PVisit.AdmittingNurse = this.MomInfo.PVisit.AdmittingNurse.Clone();
                //��Ժ���
                this.BabyInfo.PVisit.InSource = this.MomInfo.PVisit.InSource.Clone();
                //��Ժ;��
                this.BabyInfo.PVisit.Circs = this.MomInfo.PVisit.Circs.Clone();
                //��Ժ��Դ
                this.BabyInfo.PVisit.AdmitSource = this.MomInfo.PVisit.AdmitSource.Clone();


                //��Ӥ��
                isNew = true;

                //ȡӤ��������
                string happenNo = this.txtInpatientNo.Tag.ToString();
                happenNo = this.inpatientManager.GetMaxBabyNO(this.MomInfo.ID);
                if (happenNo == "-1")
                {
                    MessageBox.Show("ȡӤ��������ʱ����:" + this.inpatientManager.Err, "������ʾ");
                    return null;
                }

                //��1�õ���ǰӤ�����
                happenNo = (Neusoft.FrameWork.Function.NConvert.ToInt32(happenNo) + 1).ToString();

                this.BabyInfo.User01 = happenNo; //��User01������Ӥ�����
                //ȡӤ����
                if (this.txtName.Text == "")
                {
                    //����Ŀǰ���ֺ�Ӥ���Ա�����Ӥ������
                    this.BabyInfo.Name = CreatBabyName(this.MomInfo.Name, this.cmbSex.Tag.ToString(), int.Parse(happenNo));
                }
                else
                {
                    this.BabyInfo.Name = this.txtName.Text;
                }


                //��Ժ���ڵ���ϵͳ��ǰʱ��
                this.BabyInfo.PVisit.InTime = this.inpatientManager.GetDateTimeFromSysDateTime();

                //����סԺ��
                this.BabyInfo.PID.ID = "B" + happenNo + MomInfo.PID.PatientNO.Substring(2);

                //����סԺ��ˮ��
                this.BabyInfo.ID = MomInfo.ID.Substring(0, 4) + "B" + happenNo + MomInfo.PID.PatientNO.Substring(2);

                //�������￨��
                this.BabyInfo.PID.CardNO = "TB" + happenNo + MomInfo.PID.PatientNO.Substring(3);

                this.BabyInfo.Pact.PayKind.ID = "01";			//�Է�
                this.BabyInfo.Pact.ID = "01";		//�Է�{5CAAEFE3-1A06-46ac-9645-5A3A7175C617}
                this.BabyInfo.Pact.Name = "�ԷѶ�ͯ";//�ԷѶ�ͯ
                this.BabyInfo.PVisit.InState.ID = "R";		//��Ժ�Ǽ�
            }
            else
            {
                //�޸�Ӥ����Ϣ
                isNew = false;
                //������ѵǼǵ�Ӥ��,��ʾ�û�����Ӥ������
                if (this.txtName.Text == "")
                {
                    MessageBox.Show("������Ӥ������", "��ʾ");
                    return null;
                }

                this.BabyInfo.Name = this.txtName.Text;									//Ӥ������
            }


            this.BabyInfo.Sex.ID = this.cmbSex.Tag.ToString();				//�Ա�
            this.BabyInfo.BloodType.ID = this.cmbBlood.Tag.ToString();	//Ѫ��
            this.BabyInfo.Height = this.txtHeight.Text;						//���
            this.BabyInfo.Weight = this.txtWeight.Text;						//����
            this.BabyInfo.Birthday = this.dtBirthday.Value;					//��������

            //�ж������ĳ���
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.BabyInfo.Name, 20) == false)
            {
                txtName.Text = this.BabyInfo.Name;
                MessageBox.Show("���ƹ������������룡");
                return null;
            }

            //�����סԺ��ˮ��
            this.BabyInfo.PID.MotherInpatientNO = this.MomInfo.ID;

            return this.BabyInfo;
        }

        int BabyNum = 0;
        /// <summary>
        /// ����Ӥ������
        /// </summary>
        /// <param name="MumName">��������</param>
        /// <param name="SexId">�Ա�</param>
        /// <param name="HappenNo">�������</param>
        /// <returns></returns>
        private string CreatBabyName(string MumName, string SexId, int HappenNo)
        {
            string BabyName;
            //{FEA519C4-2379-40a9-8271-829A76E04EF6}��������
            //BabyNum++;

            switch (HappenNo)
            {
                case 1:
                    BabyName = "��";
                    break;
                case 2:
                    BabyName = "��";
                    break;
                case 3:
                    BabyName = "��";
                    break;
                case 4:
                    BabyName = "��";
                    break;
                default:
                    BabyName = "";
                    break;
            }

            #region {FEA519C4-2379-40a9-8271-829A76E04EF6}

            if (SexId == Neusoft.HISFC.Models.Base.EnumSex.M.ToString())
            {

                return MumName + "֮" + BabyName + "��";
            }
            else
            {
                return MumName + "֮" + BabyName + "Ů";
            }

            #endregion
        }


        /// <summary>
        /// ˢ������
        /// </summary>
        /// <param name="inPatientNo"></param>
        public virtual void RefreshList(string inPatientNo)
        {
            //����ؼ�
            ClearInfo();
            //��ʾӤ���б�
            ShowTreeView();
        }


        /// <summary>
        /// ��ʾӤ���б�
        /// </summary>
        private void ShowTreeView()
        {
            ArrayList al;
            al = new ArrayList();

            //���ڵ�
            al.Add("Ӥ���б�");

            //ȡӤ���б�
            al = this.inpatientManager.QueryBabiesByMother(this.MomInfo.ID);
            if (al == null)
            {
                MessageBox.Show(inpatientManager.Err);
                return;
            }

            //����Ӥ�����Ա�,����ÿ���Ա��������.��ȥӤ����סԺ�����е���Ϣ
          
            BabyNum = 0;
            foreach (Neusoft.HISFC.Models.RADT.PatientInfo baby in al)
            {
               
                BabyNum++;

              
            }


            //��ʾ�����Ϳؼ���
            this.tvPatientList1.BeginUpdate();
            this.tvPatientList1.SetPatient( al);
            this.tvPatientList1.EndUpdate();

            //չ���ڵ�,��ʾ���ڵ�
            this.tvPatientList1.ExpandAll();
            this.tvPatientList1.SelectedNode = this.tvPatientList1.Nodes[0];

        }

        #endregion

        #region �¼�
        private void btAdd_Click(object sender, System.EventArgs e)
        {
            this.ClearInfo();
            #region {FEA519C4-2379-40a9-8271-829A76E04EF6}
            
            this.babyNo = this.BabyNum + 1;

            this.txtName.Text = this.CreatBabyName(this.MomInfo.Name, this.cmbSex.Tag.ToString(), babyNo);

            #endregion
        }


        private void btCancel_Click(object sender, System.EventArgs e)
        {
            if (this.txtInpatientNo.Text == "")
            {
                MessageBox.Show("��ѡ��Ԥȡ����Ӥ����", "��ʾ");
                return;
            }

            try
            {
                string sPatientNo = this.MomInfo.PID.PatientNO;
                sPatientNo = "B" + this.txtInpatientNo.Tag.ToString() + sPatientNo.Substring(2);
                sPatientNo = MomInfo.ID.Substring(0, 4) + sPatientNo;

                Neusoft.HISFC.Models.RADT.PatientInfo p = this.inpatientManager.QueryPatientInfoByInpatientNO(sPatientNo);
                if ((p.FT.TotCost + p.FT.BalancedCost) > 0)
                {
                    MessageBox.Show("��Ӥ���Ѿ��������ã�����ȡ����");
                    return;
                }

                #region {23EE5EA6-27CB-49c9-810A-310A1515D85E}
                if (p.FT.PrepayCost > 0)
                {
                    MessageBox.Show("��Ӥ��Ԥ��������0������ȡ����");
                    return;
                }
                #endregion

                //ȡ��Ӥ��

                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.inpatientManager.Connection);
                //t.BeginTransaction();

                this.inpatientManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                if (this.inpatientManager.DiscardBabyRegister(this.BabyInfo.ID) < 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("ȡ��ʧ�ܣ�" + this.inpatientManager.Err);
                }
                else
                {
                    Neusoft.HISFC.Models.RADT.InStateEnumService status = new Neusoft.HISFC.Models.RADT.InStateEnumService();
                    status.ID = "C";
                    p.ID = sPatientNo;
                    if (this.inpatientManager.UpdatePatientStatus(p,status) == -1)
                    {//����ΪסԺ
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("ȡ���Ǽ�ʧ�ܣ�" + this.inpatientManager.Err);
                    }
                    else
                    {
                        Neusoft.FrameWork.Management.PublicTrans.Commit();
                        //����Ӥ�����ڵĽڵ�,��ɾ���˽ڵ�
                        TreeNode node = this.tv.FindNode(0, this.BabyInfo);
                        if (node != null) node.Remove();

                        //ˢ��Ӥ���б�
                        RefreshList(this.MomInfo.ID);
                        this.BabyInfo = null;
                        MessageBox.Show("ȡ���Ǽǳɹ���", "��ʾ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Neusoft.HISFC.Components.Common.Controls.tvPatientList tv = null;

        private void btSave_Click(object sender, System.EventArgs e)
        {
            //ȡ�������������µ���Ϣ,�����жϲ���
            Neusoft.HISFC.Models.RADT.PatientInfo patient = this.inpatientManager.QueryPatientInfoByInpatientNO(this.MomInfo.ID);
            if (patient == null)
            {
                MessageBox.Show(this.inpatientManager.Err);
                return;
            }
            //��������Ѳ�����Ժ״̬,���������
            if (patient.PVisit.InState.ID.ToString() != this.MomInfo.PVisit.InState.ID.ToString())
            {
                MessageBox.Show("������Ϣ�ѷ����仯,��ˢ�µ�ǰ����", "��ʾ");
                return;
            }

            #region �ж��Ƿ�������{E6D400EC-44C8-42f7-B4EE-E4D05A7D1E2C}
            try
            {
                Convert.ToDecimal(this.txtHeight.Text);
                Convert.ToDecimal(this.txtWeight.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("��ߣ�����ֻ��¼������");
                return;
            }
            #endregion
            //����ֻ��ĸ�ײ�����Ӥ���Ǽ�
            if (this.MomInfo.Sex.ID.ToString() != "F" || this.MomInfo.ID.Substring(4, 1) == "B")
            {
                MessageBox.Show("ֻ��ĸ�ײ�����Ӥ���Ǽǣ�", "��ʾ");
                return;
            }

            if (cmbSex.Text == "")
            {
                MessageBox.Show("��ѡ���Ա�", "��ʾ");
                return;
            }

            //ȡ�ؼ�����д��Ӥ����Ϣ
            //Neusoft.HISFC.Models.RADT.PatientInfo  objBaby = new Neusoft.HISFC.Models.RADT.PatientInfo();
            this.GetBabyInfo();

            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.BabyInfo.Name, 20) == false)
            {
                MessageBox.Show("���Ʋ��ܳ���10�����ֻ���20��Ӣ���ַ�,���������룡", "���ƹ���");
                return;
            }
            ////�ж�����Ƿ�Ϊ����
            //for (int i = 0, j = this.txtHeight.Text.Length; i < j; i++)
            //{
            //    if (!char.IsDigit(this.txtHeight.Text, i))
            //    {
            //        //����˵���ǵڼ����ַ�������
            //        MessageBox.Show("��߱���������", "��ʾ", MessageBoxButtons.OK);
            //        return;
            //    }
            //}
            ////�ж������Ƿ�Ϊ����
            //for (int i = 0, j = this.txtWeight.Text.Length; i < j; i++)
            //{
            //    if (!char.IsDigit(this.txtWeight.Text, i))
            //    {
            //        //����˵���ǵڼ����ַ�������
            //        MessageBox.Show("���ر���������", "��ʾ", MessageBoxButtons.OK);
            //        return;
            //    }
            //}
            //�Ա�Ϊ��ʱ

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.inpatientManager.Connection);
            //t.BeginTransaction();

            this.inpatientManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            try
            {
                this.InpatientNo = this.BabyInfo.ID;
                //������µǼǵ�Ӥ��,��Ǽ�Ӥ����ͻ���סԺ������Ϣ,�������Ӥ����ͻ���סԺ����
                if (this.isNew)
                {
                    //�Ǽ�Ӥ����
                    if (this.inpatientManager.InsertNewBabyInfo(this.BabyInfo) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //�Ǽǻ�������
                    if (this.inpatientManager.InsertPatient(this.BabyInfo) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //���±����¼����
                    if (this.inpatientManager.SetShiftData(this.BabyInfo.ID, Neusoft.HISFC.Models.Base.EnumShiftType.B, "��Ժ�Ǽ�",
                        this.BabyInfo.PVisit.PatientLocation.Dept, this.BabyInfo.PVisit.PatientLocation.Dept) < 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //���±����¼
                    this.BabyInfo.PVisit.PatientLocation.Bed.Name = this.BabyInfo.PVisit.PatientLocation.Bed.ID;
                    if (this.inpatientManager.SetShiftData(this.BabyInfo.ID, Neusoft.HISFC.Models.Base.EnumShiftType.K, "����",
                        this.BabyInfo.PVisit.PatientLocation.NurseCell, this.BabyInfo.PVisit.PatientLocation.Bed) < 0)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //���²������,Ӥ�����Ǽǲ���
                    if (this.inpatientManager.UpdateCaseSend(this.BabyInfo.ID, false) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //����ĸ���Ƿ���Ӥ�����
                    if (this.inpatientManager.UpdateMumBabyFlag(this.MomInfo.ID, true) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    //�Ǽ�Ӥ��סԺ�����е���Ժ״̬
                    Neusoft.HISFC.Models.RADT.InStateEnumService status = new Neusoft.HISFC.Models.RADT.InStateEnumService();
                    status.ID = "I"; //סԺ�Ǽ�
                    if (this.inpatientManager.UpdatePatientStatus(this.BabyInfo, status) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }

                    #region addby xuewj 2010-10-9 Ӥ���ǼǸ��ݲ����Ƿ����Ӥ�������� {4759B4FB-BA7B-4ec1-BCAA-BB456A714776}
                    if (this.isUpdatePatientAlert == "1"&&this.patientAlert!=0m)
                    {
                        if (this.inpatientManager.UpdatePatientAlert(this.BabyInfo.ID,patientAlert, "M", DateTime.MinValue, DateTime.MinValue) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1);
                        }
                    }
                    #endregion
                }
                else
                {
                   

                    //���»���סԺ����(���������ͬʱ,�����Ӥ����)
                    if (this.inpatientManager.UpdatePatient(this.BabyInfo) != 1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("����ʧ�ܣ�" + this.inpatientManager.Err, "��ʾ");
                        return;
                    }
                }

                //�ύ���ݿ�
                Neusoft.FrameWork.Management.PublicTrans.Commit();
                //������µǼ�Ӥ��,���ڻ����б��в���һ���½ڵ�,������½ڵ�
                if (this.isNew)
                {
                    this.tv.AddTreeNode(0, this.BabyInfo);
                }
                else
                {
                    //����Ӥ�����ڵĽڵ�,���޸Ĵ˽ڵ�
                    TreeNode node = this.tv.FindNode(0, this.BabyInfo);
                    if (node != null) this.tv.ModifiyNode(node, this.BabyInfo);
                }
                MessageBox.Show("����ɹ���");
                
                //ˢ��Ӥ���б�
                RefreshList(this.MomInfo.ID);

                ShowBabyInfo(this.BabyInfo);
                base.OnRefreshTree();
            }
            catch (Exception ex)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void tvPatientList1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            //ȡ�ڵ��ϵ���Ϣ
            this.BabyInfo = e.Node.Tag as Neusoft.HISFC.Models.RADT.PatientInfo;
            //��Ӥ����Ϣ��ʾ�ڿؼ���
            this.ShowBabyInfo(this.BabyInfo);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            tv = sender as Neusoft.HISFC.Components.Common.Controls.tvPatientList;
            this.InitControl();
            return null;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="neuObject"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            this.MomInfo = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;
            this.BabyInfo = MomInfo.Clone();
            if (this.MomInfo.ID != null || this.MomInfo.ID != "")
            {
                try
                {
                    this.txtMomInfo.Text = "[" + MomInfo.PVisit.PatientLocation.Bed.ID.Substring(4) + "]" + MomInfo.Name;
                    RefreshList(MomInfo.ID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return 0;
        }

        #region {FEA519C4-2379-40a9-8271-829A76E04EF6}

        private void cmbSex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.MomInfo != null && this.MomInfo.ID != "")
            {
                this.txtName.Text = this.CreatBabyName(this.MomInfo.Name, this.cmbSex.Tag.ToString(), this.babyNo);
            }
        }
        #endregion

        #endregion


    }
}
