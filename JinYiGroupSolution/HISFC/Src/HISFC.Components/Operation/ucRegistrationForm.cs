using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.Operation;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Base;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: �����Ǽǵ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucRegistrationForm : UserControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucRegistrationForm()
        {
            InitializeComponent();
            if (!Environment.DesignMode)
            {
                this.InitControl();
                this.Clear();
            }
        }

        #region �ֶ�

        /// <summary>
        /// ��ǰ����Ա
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee currentOperator = new Employee(); 

        private OperationRecord operationRecord = new OperationRecord();
        /// <summary>
        /// ���������Ǽǿ���
        /// </summary>
        //private string dept;
        /// <summary>
        /// �Ƿ���¼��
        /// </summary>
        public bool IsNew = true;
        /// <summary>
        /// �Ƿ�¼
        /// </summary>
        private bool isRenew = false;

        private bool isCancled = false;
        private Neusoft.HISFC.BizProcess.Interface.Operation.IRecordFormPrint recordFormPrint;
        Neusoft.FrameWork.Public.ObjectHelper employHelper = new Neusoft.FrameWork.Public.ObjectHelper();
        Neusoft.HISFC.BizLogic.Operation.OpsTableManage opsTableMgr = new Neusoft.HISFC.BizLogic.Operation.OpsTableManage();
        Neusoft.HISFC.BizProcess.Integrate.Operation.OpsDiagnose opsDiagnose = new Neusoft.HISFC.BizProcess.Integrate.Operation.OpsDiagnose();
       
        
        
        #endregion

        #region ����

        /// <summary>
        /// �����Ƿ��ѱ�ȡ��
        /// </summary>
        public bool IsCancled
        {
            set
            {
                isCancled = value;
            }
        }
        /// <summary>
        /// �Ƿ����ֹ��Ǽ����� 
        /// </summary>
        public bool HandInput
        {
            get
            {
                return isRenew;
            }
            set
            {
                try
                {
                    this.Clear();
                    isRenew = value;
                    this.SetEnable(isRenew);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        /// <summary>
        ///  �½�����
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OperationAppllication OperationApplication
        {
            set
            {
                this.Clear();//���
                if (this.IsNew)
                    this.operationRecord = new OperationRecord();

                OperationAppllication apply = value;

                if (apply.PatientInfo.ID.Length == 0)
                {
                    MessageBox.Show("�������뵥Ϊ��!", "��ʾ");
                    return;
                }
                #region ��ֵ
                this.lbName.Text = apply.PatientInfo.Name;//����
                this.lbSex.Text = apply.PatientInfo.Sex.Name;//�Ա�

                //int age = Environment.OperationManager.GetDateTimeFromSysDateTime().Year - apply.PatientInfo.Birthday.Year;
                //if (age == 0)
                //    age = 1;
                this.lbAge.Text = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(apply.PatientInfo.Birthday);//age.ToString() + "��";//����

                this.lbPatient.Text = apply.PatientInfo.PID.PatientNO;//סԺ��

                Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = Environment.RadtManager.GetPatientInfomation(apply.PatientInfo.ID);
                if (patientInfo == null || patientInfo.ID.Length == 0)
                {
                    MessageBox.Show("�޴˻�����Ϣ!", "��ʾ");
                    return;
                }
                #region �������

                NeuObject kind = Environment.GetPayKind(patientInfo.Pact.PayKind.ID);
                if (kind == null)
                    this.lbPaykind.Text = patientInfo.Pact.PayKind.ID;
                else
                    this.lbPaykind.Text = kind.Name;
                #endregion
                //by zlw 2006-5-24 ȡ�����������
                this.lbDept.Text = apply.PatientInfo.PVisit.PatientLocation.Dept.Name;
                this.lbDept.Tag = apply.PatientInfo.PVisit.PatientLocation.Dept.ID;
                //this.lbDept.Tag = this.operationRecord.OperationAppllication.PatientInfo.PVisit.PatientLocation.Dept.ID;
                //			this.lbDept.Text=p.PVisit.PatientLocation.Dept.Name;//סԺ����
                this.lbBed.Text = apply.PatientInfo.PVisit.PatientLocation.Bed.ID;//����
                this.lbFree.Text = patientInfo.FT.LeftCost.ToString();//���
                this.lbOpsDept.Text = apply.ExeDept.Name;//������
                
                //#region ̨����
                //if (apply.TableType == "1")
                //{
                //    this.lbTableType.Text = "��̨";
                //}//��̨
                //else if (apply.TableType == "2")
                //{
                //    this.lbTableType.Text = "��̨";
                //}//��̨
                //else
                //{
                //    this.lbTableType.Text = "��̨";
                //}//��̨
                //#endregion
                
                
                this.lbOpsDept.Text = Environment.GetDept(apply.ExeDept.ID).Name;//������
                lbOpsDept.Tag = apply.ExeDept.ID;//���ұ���
                neuLabel27.Tag = apply.ApplyDoctor.ID;
                this.lbApplyDoct.Text = employHelper.GetName(apply.ApplyDoctor.ID);//����ҽ��
                this.lbPreDate.Text = apply.PreDate.Date.ToString("yyyy-MM-dd");//Ԥ������ʱ��
                #region ���
                // TODO: ������
                if (apply.DiagnoseAl.Count > 0)
                {
                    this.txtDiag.Text = (apply.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10.Name;//���
                    this.txtDiag.Tag = (apply.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10;//���

                    if (apply.DiagnoseAl.Count > 1)
                    {
                        this.txtDiag2.Text = (apply.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10.Name;//���
                        this.txtDiag2.Tag = (apply.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10;//���
                        if (apply.DiagnoseAl.Count > 2)
                        {
                            this.txtDiag3.Text = (apply.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10.Name;//���
                            this.txtDiag3.Tag = (apply.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10;//���
                        }
                    }
                }

                #endregion
                #region ����ʽ
                if (apply.AnesType.ID != null && apply.AnesType.ID != "")
                {
                    NeuObject obj = Environment.GetAnes(apply.AnesType.ID);
                    if (obj != null)
                    {
                        this.lbAnae.Text = obj.Name;
                        this.lbAnae.Tag = obj.ID;
                    }
                }
                #endregion
                #region ��������
                if (apply.OperationInfos.Count > 0)
                {
                    this.txtOperation.Text = (apply.OperationInfos[0] as OperationInfo).OperationItem.Name;
                    this.txtOperation.Tag = (OperationInfo)apply.OperationInfos[0];

                    if (apply.OperationInfos.Count > 1)
                    {
                        this.txtOperation2.Text = (apply.OperationInfos[1] as OperationInfo).OperationItem.Name;
                        this.txtOperation2.Tag = (OperationInfo)apply.OperationInfos[1];
                        if (apply.OperationInfos.Count > 2)
                        {
                            this.txtOperation3.Text = (apply.OperationInfos[2] as OperationInfo).OperationItem.Name;
                            this.txtOperation3.Tag = (OperationInfo)apply.OperationInfos[2];
                        }
                    }
                }
                #endregion

                if (this.isCancled==false && this.IsNew==false && this.HandInput==false)
                {
                    this.dtBeginDate.Value = this.operationRecord.OpsDate;
                }
                else 
                {
                    this.dtBeginDate.Value = apply.PreDate;//������ʼʱ��
                }
                this.dtEndDate.Value = System.DateTime.Now;
                this.cmbRoom.Tag = apply.RoomID;//����
                ArrayList al = Environment.TableManager.GetOpsTable(apply.RoomID);
                if (al == null)
                {
                    MessageBox.Show("��ȡ����" + apply.RoomID + "�ڵ�����̨�ų���");
                }
                this.cmbOrder.Items.Clear();
                this.cmbOrder.AddItems(al);

                this.cmbOrder.Tag = apply.OpsTable.ID;//̨��

                this.cmbDoctor.Tag = apply.OperationDoctor.ID;//������
                #region ��ʿ
                foreach (ArrangeRole role in apply.RoleAl)
                {
                    if (role.RoleType.ID.ToString() == EnumOperationRole.Helper1.ToString())
                    {
                        this.cmbHelper1.Tag = role.ID;//һ��
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.Helper2.ToString())
                    {
                        this.cmbHelper2.Tag = role.ID;//����
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.Helper3.ToString())
                    {
                        this.cmbHelper3.Tag = role.ID;//����
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.WashingHandNurse.ToString())
                    {
                        if (this.cmbWash1.Tag == null || this.cmbWash1.Tag.ToString() == "")
                        {
                            this.cmbWash1.Tag = role.ID;
                        }//ϴ�ֻ�ʿ}
                        else if (this.cmbWash2.Tag == null || this.cmbWash2.Tag.ToString() == "")
                        {
                            this.cmbWash2.Tag = role.ID;
                        }
                        else if (this.cmbWash3.Tag == null || this.cmbWash3.Tag.ToString() == "")
                        {
                            this.cmbWash3.Tag = role.ID;
                        }
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.ItinerantNurse.ToString())
                    {
                        if (this.cmbXH1.Tag == null || this.cmbXH1.Tag.ToString() == "")
                        {
                            this.cmbXH1.Tag = role.ID;
                        }//Ѳ�ػ�ʿ
                        else if (this.cmbXH2.Tag == null || this.cmbXH2.Tag.ToString() == "")
                        {
                            this.cmbXH2.Tag = role.ID;
                        }
                        else if (this.cmbXH3.Tag == null || this.cmbXH3.Tag.ToString() == "")
                        {
                            this.cmbXH3.Tag = role.ID;
                        }
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper1.ToString())
                    {
                        if (this.txtTmpHelper1.Tag == null || this.txtTmpHelper1.Tag.ToString() == "")
                        {
                            this.txtTmpHelper1.Tag = role.ID;
                            this.txtTmpHelper1.Text = role.Name;
                        }
                       
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper2.ToString())
                    {
                        if (this.txtTmpHelper2.Tag == null || this.txtTmpHelper2.Tag.ToString() == "")
                        {
                            this.txtTmpHelper2.Tag = role.ID;
                            this.txtTmpHelper2.Text = role.Name;
                        }

                    }
                    #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpStudent1.ToString())
                    {
                        if (this.txtTmpStudent1.Tag == null || this.txtTmpStudent1.Tag.ToString() == "")
                        {
                            this.txtTmpStudent1.Tag = role.ID;
                            this.txtTmpStudent1.Text = role.Name;
                        }

                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpStudent2.ToString())
                    {
                        if (this.txtTmpStudent2.Tag == null || this.txtTmpStudent2.Tag.ToString() == "")
                        {
                            this.txtTmpStudent2.Tag = role.ID;
                            this.txtTmpStudent2.Text = role.Name;
                        }

                    } 
                    #endregion
                }
                #endregion
                //������ģ
                this.cmbOpType.Tag = apply.OperationType.ID;
                //��������
                this.cmbOperKind.Tag = apply.OperateKind;

                //if ( == "1")
                //{ this.cmbOperKind.SelectedIndex = 0; }//��ͨ
                //else if (apply.OperateKind == "2")
                //{ this.cmbOperKind.SelectedIndex = 1; }//����
                //else
                //{
                //    this.cmbOperKind.SelectedIndex = 0;//��Ⱦ
                //    this.cbxInfect.Checked = true;
                //}
                 
                this.rtbApplyNote.Text = apply.ApplyNote; 
                #endregion

                this.operationRecord.OperationAppllication = apply;
                comDept.Tag = this.operationRecord.OperationAppllication.OperationDoctor.Dept.ID;
                //this.IsNew = true;//���� 


                this.isRenew = false;
                this.ucDiag1.Visible = false;
                this.ucOpItem1.Visible = false;
                //{B9DDCC10-3380-4212-99E5-BB909643F11B}
                this.cmbAnseWay.Tag = apply.AnesWay;
                //{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
                this.cmbIncityep.Tag = this.operationRecord.OperationAppllication.InciType.ID;


            }
        }

        /// <summary>
        /// �޸����뵥
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OperationRecord OperationRecord
        {
            set
            {

                this.operationRecord = value;
                this.OperationApplication = value.OperationAppllication;
                if (value.OutDate != System.DateTime.MinValue)
                {
                    this.dtEndDate.Value = value.OutDate;
                }
                this.cbxInfect.Checked = this.operationRecord.IsInfected;
                this.cbxGerm.Checked = this.operationRecord.OperationAppllication.IsGermCarrying;
            }
        }
        #endregion

        #region ����


        public void InitControl()
        {
            currentOperator = (Neusoft.HISFC.Models.Base.Employee)opsTableMgr.Operator;

            Neusoft.HISFC.BizProcess.Integrate.Manager managerMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            employHelper.ArrayObject = managerMgr.QueryEmployeeAll();

            //�����
            this.cmbRoom.Items.Clear();
            ArrayList al = Environment.TableManager.GetRoomsByDept(Environment.OperatorDeptID);
            if (al != null)
            {
                this.cmbRoom.AddItems(al);
                this.cmbRoom.IsListOnly = true;
            }

            //������ 
            lbOpsDept.AddItems(Environment.IntegrateManager.QueryDepartment("1"));//������
            
            //��������
            this.lbAnae.AddItems(Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESTYPE));
            
            //���ؿ���
            ArrayList deptList = Environment.IntegrateManager.QueryDeptmentsInHos(true);
            this.comDept.AddItems(deptList); 
            
            //����ҽ��
            al = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D);

            //����
            this.cmbDoctor.Items.Clear();
            this.cmbDoctor.AddItems(al);
            this.cmbDoctor.IsListOnly = true;
            //һ��
            this.cmbHelper1.Items.Clear();
            this.cmbHelper1.AddItems(al);
            this.cmbHelper1.IsListOnly = true;
            //����
            this.cmbHelper2.Items.Clear();
            this.cmbHelper2.AddItems(al);
            this.cmbHelper2.IsListOnly = true;
            //������
            this.cmbHelper3.Items.Clear();
            this.cmbHelper3.AddItems(al);
            this.cmbHelper3.IsListOnly = true;

            //ϴ��1
            al = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.N, currentOperator.Dept.ID);
            if (al == null) al = new ArrayList();
            this.cmbWash1.Items.Clear();
            this.cmbWash1.AddItems(al);
            this.cmbWash1.IsListOnly = true;

            //ϴ��2
            this.cmbWash2.AddItems(al);
            this.cmbWash2.IsListOnly = true;

            //ϴ��3
            this.cmbWash3.AddItems(al);
            this.cmbWash3.IsListOnly = true;

            //Ѳ��1
            this.cmbXH1.AddItems(al);
            this.cmbXH1.IsListOnly = true;

            //Ѳ��2
            this.cmbXH2.AddItems(al);
            this.cmbXH2.IsListOnly = true;

            //Ѳ��3
            this.cmbXH3.AddItems(al);
            this.cmbXH3.IsListOnly = true;

            //������ģ
            al = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.OPERATETYPE);
            if (al == null) al = new ArrayList();
            this.cmbOpType.AddItems(al);
            this.cmbOpType.IsListOnly = true;

            //��������
            ArrayList alKind = Environment.IntegrateManager.GetConstantList("OPERATEKIND");
            if (alKind == null) alKind = new ArrayList();
            this.cmbOperKind.AddItems(alKind);
            this.cmbOperKind.IsListOnly = true;



            #region ���
            ucDiag1 = new Neusoft.HISFC.Components.Common.Controls.ucDiagnose();
            this.Controls.Add(ucDiag1);
            ucDiag1.Size = new Size(456, 312);
            ucDiag1.SelectItem += new Neusoft.HISFC.Components.Common.Controls.ucDiagnose.MyDelegate(ucDiag1_SelectItem);
            ucDiag1.Init();
            ucDiag1.Visible = false;
            #endregion

            #region ����
            ucOpItem1 = new ucOpItem();
            this.Controls.Add(ucOpItem1);
            ucOpItem1.Size = new Size(518, 338);
            ucOpItem1.SelectItem += new ucOpItem.MyDelegate(ucOpItem1_SelectItem);
            ucOpItem1.Init();
            ucOpItem1.Visible = false;
            #endregion

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            //�������'������𣨾����ѡ�飬ҽ������ʱ��д��//{B9DDCC10-3380-4212-99E5-BB909643F11B}
            ArrayList alRet = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESWAY);
            this.cmbAnseWay.AddItems(alRet);
            this.cmbAnseWay.IsListOnly = true;

            alRet = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.INCITYPE);

            this.cmbIncityep.AddItems(alRet);
            this.cmbIncityep.IsListOnly = true;
        }


        #region ����

        Neusoft.HISFC.Components.Operation.ucOpItem ucOpItem1 = null;
        private System.Windows.Forms.Control contralActive = new Control();
        int ucOpItem1_SelectItem(Keys key)
        {
            this.ProcessOps();
            this.txtOperation.Focus(); 
            return 1;
        }
        private int ProcessOps()
        {
            Neusoft.HISFC.Models.Fee.Item.Undrug item = null;
            if (this.ucOpItem1.GetItem(ref item) == -1)
            {
                //MessageBox.Show("��ȡ��Ŀ����!","��ʾ");
                return -1;
            }
            this.contralActive.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;
            this.contralActive.Tag = item;
            this.ucOpItem1.Visible = false;
            return 0;
        }
        private void txtOperation_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation2_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation2;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation3_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtOperation3;
            this.ucDiag1.Visible = false;
        }

        private void txtOperation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                this.txtOperation2.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                this.txtOperation3.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.ucOpItem1.Visible)
                {
                    if (this.ProcessOps() == -1)
                        return;
                }

                //{B9DDCC10-3380-4212-99E5-BB909643F11B}
                //this.lbAnae.Focus();
                this.cmbAnseWay.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucOpItem1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucOpItem1.NextRow();
            }
        }

        private void txtOperation_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation.Focused)
            {
                return;
            }
            string text = this.txtOperation.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation.Location.X, txtOperation.Location.Y + txtOperation.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation.Tag = null;
        }

        private void txtOperation2_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation2.Focused)
            {
                return;
            }
            string text = this.txtOperation2.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation2.Location.X, txtOperation2.Location.Y + txtOperation2.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation2.Tag = null;
        }

        private void txtOperation3_TextChanged(object sender, EventArgs e)
        {
            if (!txtOperation3.Focused)
            {
                return;
            }
            string text = this.txtOperation3.Text;

            if (this.ucOpItem1.Visible == false) this.ucOpItem1.Visible = true;
            this.ucOpItem1.Location = new System.Drawing.Point(txtOperation3.Location.X, txtOperation3.Location.Y + txtOperation3.Height + 2);
            ucOpItem1.BringToFront();
            this.ucOpItem1.Filter(text);
            this.txtOperation3.Tag = null;
        }
        private void txtOperation3_Leave(object sender, EventArgs e)
        {
            //if (!ucOpItem1.Focused)
            //{
            //    this.ucOpItem1.Visible = false;
            //}
        }
        #endregion 
        #region ���
        Neusoft.HISFC.Components.Common.Controls.ucDiagnose ucDiag1 = null;
        int ucDiag1_SelectItem(Keys key)
        {
            return 1;
        }
        private void txtDiag1_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag;
            this.ucOpItem1.Visible = false;
        }

        private void txtDiag2_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag2;
            this.ucOpItem1.Visible = false;
        }

        private void txtDiag3_Enter(object sender, EventArgs e)
        {
            contralActive = this.txtDiag3;
            this.ucOpItem1.Visible = false;
        }
        private void txtDiag1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtDiag.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtDiag2.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }

        private void txtDiag2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtDiag2.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtDiag3.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }

        private void txtDiag3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtDiag3.Visible)
                {
                    if (this.ProcessDiag() == -1) return;
                }

                this.txtOperation.Focus();
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.ucDiag1.PriorRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                this.ucDiag1.NextRow();
            }
        }
        private void txtDiag1_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag.Focused)
            {
                return;
            }
            contralActive = this.txtDiag;
            string text = this.txtDiag.Text;
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag.Location.X, txtDiag.Location.Y + txtDiag.Height + 2);
            ucDiag1.BringToFront();
            if (this.ucDiag1.Visible == false) this.ucDiag1.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag.Tag = null;
        }

        private void txtDiag2_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag2.Focused)
            {
                return;
            }
            contralActive = this.txtDiag;
            string text = this.txtDiag2.Text;
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag2.Location.X, txtDiag2.Location.Y + txtDiag2.Height + 2);
            txtDiag2.BringToFront();
            if (this.txtDiag2.Visible == false) this.txtDiag2.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag2.Tag = null;
        }

        private void txtDiag3_TextChanged(object sender, EventArgs e)
        {
            if (!txtDiag3.Focused)
            {
                return;
            }
            contralActive = this.txtDiag3;
            string text = this.txtDiag3.Text;
            this.ucDiag1.Location = new System.Drawing.Point(txtDiag3.Location.X, txtDiag3.Location.Y + txtDiag3.Height + 2);
            txtDiag2.BringToFront();
            if (this.txtDiag3.Visible == false) this.txtDiag3.Visible = true;

            this.ucDiag1.Filter(text);
            this.txtDiag3.Tag = null;
        }
        private int ProcessDiag()
        {
            Neusoft.HISFC.Models.HealthRecord.ICD item = null;
            if (this.ucDiag1.GetItem(ref item) == -1)
            {
                //MessageBox.Show("��ȡ��Ŀ����!","��ʾ");
                return -1;
            } 
            this.contralActive.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name; 
            this.contralActive.Tag = item;
            this.ucDiag1.Visible = false;

            return 0;
        }
        #endregion 
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Clear()
        {
            this.lbAge.Text = "";//����
            this.lbBed.Text = "";//����
            this.lbDept.Text = "";//����
            this.lbFree.Text = "";//���
            this.lbName.Text = "";//����
            this.lbPaykind.Text = "";
            this.lbSex.Text = "";
            this.lbPatient.Text = "";//סԺ��
            this.lbOpsDept.Text = "";//������
            //this.lbTableType.Text = "";//̨����
            this.lbApplyDoct.Text = "";//����ҽ��
            this.lbPreDate.Text = "";//ԤԼ����
            this.lbAnae.Text = "";//����ʽ
            txtDiag.Text = "";
            txtDiag.Tag = null;
            txtDiag2.Text = "";
            txtDiag2.Tag = null;
            txtDiag3.Text = "";
            txtDiag3.Tag = null;
            this.txtDiag.Text = "";
            this.txtDiag2.Text = "";
            this.txtDiag3.Text = "";
            this.txtDiag.Tag = null;
            this.txtDiag2.Tag = null;
            this.txtDiag3.Tag = null; 
            this.txtOperation.Text = "";//��������
            this.txtOperation.Tag = null;
            this.txtOperation2.Text = "";
            this.txtOperation2.Tag = null;
            this.txtOperation3.Text = "";
            this.txtOperation3.Tag = null; 

            DateTime dtNow = Environment.OperationManager.GetDateTimeFromSysDateTime();
            this.dtBeginDate.Value = dtNow;//��ʼʱ��
            this.dtEndDate.Value = dtNow;//����ʱ��

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            this.cmbAnseWay.Text = "";//����ʽ
            this.cmbAnseWay.Tag = null;

            this.lbOpsDept.Text = "";//����
            this.lbOpsDept.Tag = null;
            //this.cmbOrder.Text = "";//̨��

            this.cmbDoctor.Text = "";//������
            this.cmbDoctor.Tag = null;
            this.cmbHelper1.Text = "";//һ��
            this.cmbHelper1.Tag = null;
            this.cmbHelper2.Text = "";//����
            this.cmbHelper2.Tag = null;
            this.cmbHelper3.Text = "";//����
            this.cmbHelper3.Tag = null;
            this.cmbWash1.Text = "";//ϴ��1
            this.cmbWash1.Tag = null;
            this.cmbWash2.Text = "";//ϴ��2
            this.cmbWash2.Tag = null;
            this.cmbWash3.Text = "";//ϴ��3
            this.cmbWash3.Tag = null;
            this.cmbXH1.Text = "";//Ѳ��1
            this.cmbXH1.Tag = null;
            this.cmbXH2.Text = "";//Ѳ��2
            this.cmbXH2.Tag = null;
            this.cmbXH3.Text = "";//Ѳ��3
            this.cmbXH3.Tag = null;


            this.txtTmpHelper1.Text = string.Empty;
            this.txtTmpHelper1.Tag = null;
            this.txtTmpHelper2.Text = string.Empty;
            this.txtTmpHelper2.Tag = null;

            #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
            this.txtTmpStudent1.Text = string.Empty;
            this.txtTmpStudent1.Tag = null;
            this.txtTmpStudent2.Text = string.Empty;
            this.txtTmpStudent2.Tag = null;
            
            #endregion

            this.cmbOpType.Text = "";//������ģ
            this.cmbOpType.Tag = null;
            this.cmbOperKind.Text = "";//����			
            this.cbxInfect.Checked = false;
            //�о�
            this.cbxGerm.Checked = true;
            
            

            this.cmbIncityep.Tag = null;

            this.rtbApplyNote.Text = "";//

            return 0;
        }


        /// <summary>
        /// ʵ�帳ֵ
        /// </summary>
        /// <returns></returns>
        private int GetValue()
        {
            if (HandInput)
            {

                if (this.lbPatient.Text == "")
                {
                    MessageBox.Show("������סԺ/�����");
                }

                //if (this.lbTableType.Text == "��̨")
                //{
                //    this.operationRecord.OperationAppllication.TableType = "1";
                //}
                //else if (this.lbTableType.Text == "��̨")
                //{
                //    this.operationRecord.OperationAppllication.TableType = "2";
                //}
                if (lbOpsDept.Tag != null)
                {
                    this.operationRecord.OperationAppllication.ExeDept.Name = this.lbOpsDept.Text;//������
                    this.operationRecord.OperationAppllication.ExeDept.ID = this.lbOpsDept.Tag.ToString();//������
                }

                isRenew = true;
            }
            operationRecord.OperationAppllication.ApplyDoctor.ID = neuLabel27.Tag.ToString();
            operationRecord.OperationAppllication.ApplyDoctor.Name = this.lbApplyDoct.Text;//����ҽ��
            if (this.IsNew && this.isRenew)
            {
                this.operationRecord.OperationAppllication.OperationDoctor.Dept = Environment.IntegrateManager.GetEmployeeInfo(cmbDoctor.Tag.ToString()).Dept;
                this.operationRecord.OperationAppllication.ID = Environment.OperationManager.GetNewOperationNo();
            }
            //�µǼǡ��ǲ�¼�����޸����������
            //if (this.IsNew && !this.isRenew)
            //{
            //    foreach (Neusoft.HISFC.Models.HealthRecord.DiagnoseBase diag in this.operationRecord.OperationAppllication.DiagnoseAl)
            //    {
            //        //����ǰѪѹ���Լ�¼�������
            //        if (diag.IsMain) this.opRecord.ForePress = diag.Name;
            //    }
            //}
            //if (this.HandInput)
            //{
            // TODO: ������
            #region ���
            Neusoft.HISFC.Models.HealthRecord.DiagnoseBase diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();
            diag.OperationNo = operationRecord.OperationAppllication.ID;//�����
            diag.ICD10 = (Neusoft.HISFC.Models.HealthRecord.ICD)this.txtDiag.Tag;
            diag.ID = this.txtDiag.Tag.ToString(); 
            diag.Name = this.txtDiag.Text; 
            diag.Patient = operationRecord.OperationAppllication.PatientInfo.Clone();
            diag.DiagType.ID = "7";//�������
            diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
            diag.DiagDate = opsTableMgr.GetDateTimeFromSysDateTime();//���ʱ��
            diag.Doctor.ID = currentOperator.ID;//���ҽ��
            diag.Doctor.Name = currentOperator.Name;//���ҽ��
            diag.Dept.ID = currentOperator.Dept.ID;//��Ͽ���
            diag.IsValid = true;//�Ƿ���Ч
            diag.IsMain = true;//�����

            if (operationRecord.OperationAppllication.DiagnoseAl.Count == 0)
                diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
            else
                diag.HappenNo = (operationRecord.OperationAppllication.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;

            operationRecord.OperationAppllication.DiagnoseAl.Clear();
            operationRecord.OperationAppllication.DiagnoseAl.Add(diag);
            #region �ڶ����
            if (txtDiag2.Tag != null)
            {
                diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();
                diag.OperationNo = operationRecord.OperationAppllication.ID;//�����
                diag.ICD10 = (Neusoft.HISFC.Models.HealthRecord.ICD)this.txtDiag2.Tag;
                diag.ID = (this.txtDiag2.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).ID;
                diag.Name = (this.txtDiag2.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                diag.Patient = operationRecord.OperationAppllication.PatientInfo.Clone();
                diag.DiagType.ID = "7";//�������
                diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
                diag.DiagDate = opsTableMgr.GetDateTimeFromSysDateTime();//���ʱ��
                diag.Doctor.ID = currentOperator.ID;//���ҽ��
                diag.Doctor.Name = currentOperator.Name;//���ҽ��
                diag.Dept.ID = currentOperator.Dept.ID;//��Ͽ���
                diag.IsValid = true;//�Ƿ���Ч
                diag.IsMain = false;//�����

                if (operationRecord.OperationAppllication.DiagnoseAl.Count == 1)
                    diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
                else
                    diag.HappenNo = (operationRecord.OperationAppllication.DiagnoseAl[1] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;
                operationRecord.OperationAppllication.DiagnoseAl.Add(diag);
            }
            #endregion
            #region �������
            if (txtDiag3.Tag != null)
            {
                diag = new Neusoft.HISFC.Models.HealthRecord.DiagnoseBase();
                diag.OperationNo = operationRecord.OperationAppllication.ID;//�����
                diag.ICD10 = (Neusoft.HISFC.Models.HealthRecord.ICD)this.txtDiag3.Tag;
                diag.ID = (this.txtDiag3.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).ID;
                diag.Name = (this.txtDiag3.Tag as Neusoft.HISFC.Models.HealthRecord.ICD).Name;
                diag.Patient = operationRecord.OperationAppllication.PatientInfo.Clone();
                diag.DiagType.ID = "7";//�������
                diag.DiagType.Name = Neusoft.HISFC.Models.HealthRecord.DiagnoseType.enuDiagnoseType.OTHER.ToString();//��ǰ���
                diag.DiagDate = opsTableMgr.GetDateTimeFromSysDateTime();//���ʱ��
                diag.Doctor.ID = currentOperator.ID;//���ҽ��
                diag.Doctor.Name = currentOperator.Name;//���ҽ��
                diag.Dept.ID = currentOperator.Dept.ID;//��Ͽ���
                diag.IsValid = true;//�Ƿ���Ч
                diag.IsMain = false;//�����

                if (operationRecord.OperationAppllication.DiagnoseAl.Count == 2)
                    diag.HappenNo = opsDiagnose.GetNewDignoseNo();//���
                else
                    diag.HappenNo = (operationRecord.OperationAppllication.DiagnoseAl[2] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).HappenNo;
                operationRecord.OperationAppllication.DiagnoseAl.Add(diag);
            }
            #endregion

            #endregion
            //}

            #region ����ʽ
            //����ʽ
            this.operationRecord.OperationAppllication.AnesType.ID = this.lbAnae.Tag.ToString();
            this.operationRecord.OperationAppllication.AnesType.Name = this.lbAnae.Text;
            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            this.operationRecord.OperationAppllication.AnesWay = this.cmbAnseWay.Tag.ToString();
            #endregion

            #region ������Ŀ
            this.operationRecord.OperationAppllication.OperationInfos.Clear();

            if (this.txtOperation.Tag != null)
            {
                this.operationRecord.OperationAppllication.AddOperation(this.txtOperation.Tag, true);
            }

            if (this.txtOperation2.Tag != null)
            {
                this.operationRecord.OperationAppllication.AddOperation(this.txtOperation2.Tag);
            }
            if (this.txtOperation3.Tag != null)
            {
                this.operationRecord.OperationAppllication.AddOperation(this.txtOperation3.Tag);
            }

            this.operationRecord.OperationAppllication.OperationType.ID = this.cmbOpType.Tag.ToString();
            #endregion

            this.operationRecord.OpsDate = this.dtBeginDate.Value;//��ʼʱ�� 
            this.operationRecord.OperationAppllication.RoomID = this.cmbRoom.Tag.ToString();//����
            if (this.cmbOrder.Tag != null)
            {
                this.operationRecord.OperationAppllication.BloodUnit = this.cmbOrder.Tag.ToString();
            }
            else
            {
                this.operationRecord.OperationAppllication.BloodUnit = "";
            }

            //����˵��
            this.operationRecord.Memo = this.rtbApplyNote.Text.Trim();
            this.operationRecord.OutDate = this.dtEndDate.Value;

            ArrayList roleArrayList = new ArrayList();

            for (int i = 0; i < this.operationRecord.OperationAppllication.RoleAl.Count; i++) 
            {
                ArrangeRole tmprole = this.operationRecord.OperationAppllication.RoleAl[i] as ArrangeRole;
                #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
                if (
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.Operator.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.Helper1.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.Helper2.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.Helper3.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.WashingHandNurse.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.ItinerantNurse.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.TmpHelper1.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.TmpHelper2.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.TmpStudent1.ToString() &&
                            tmprole.RoleType.ID.ToString() != EnumOperationRole.TmpStudent2.ToString()
                            )
                {
                    roleArrayList.Add(tmprole);
                } 
                #endregion
            }

            this.operationRecord.OperationAppllication.RoleAl.Clear();

            #region ����
            ArrangeRole role = new ArrangeRole();
            role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
            role.ID = this.cmbDoctor.Tag.ToString();//��Ա����
            role.Name = this.cmbDoctor.Text;
            role.RoleType.ID = EnumOperationRole.Operator;//��ɫ����
            role.ForeFlag = "1";//����¼��
           
            //this.operationRecord.OperationAppllication.RoleAl.Add(role);

            roleArrayList.Add(role);

            this.operationRecord.OperationAppllication.OperationDoctor.ID = role.ID;
            this.operationRecord.OperationAppllication.OperationDoctor.Name = role.Name;
            this.operationRecord.BloodPressureIn = this.txtDiag.Text; //��һ���
            #endregion

            #region һ��
            role = new ArrangeRole();
            role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
            role.ID = this.cmbHelper1.Tag.ToString();//��Ա����
            role.Name = this.cmbHelper1.Text;
            role.RoleType.ID = EnumOperationRole.Helper1;//��ɫ����
            role.ForeFlag = "1";//����¼��
            //this.operationRecord.OperationAppllication.RoleAl.Add(role);
            roleArrayList.Add(role);

            this.operationRecord.OperationAppllication.HelperAl.Clear();
            this.operationRecord.OperationAppllication.HelperAl.Add(role);
            #endregion

            #region ����
            if (this.cmbHelper2.Tag != null && this.cmbHelper2.Tag.ToString() != "")
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = this.cmbHelper2.Tag.ToString();//��Ա����
                role.Name = this.cmbHelper2.Text;
                role.RoleType.ID = EnumOperationRole.Helper2;//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);
                roleArrayList.Add(role);

                this.operationRecord.OperationAppllication.HelperAl.Add(role);
            }
            #endregion

            #region ����
            if (this.cmbHelper3.Tag != null && this.cmbHelper3.Tag.ToString() != "")
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = this.cmbHelper3.Tag.ToString();//��Ա����
                role.Name = this.cmbHelper3.Text;
                role.RoleType.ID = EnumOperationRole.Helper3;//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);
                roleArrayList.Add(role);

                this.operationRecord.OperationAppllication.HelperAl.Add(role);
            }
            #endregion

            #region ϴ�ֻ�ʿ
            if (this.cmbWash1.Tag != null && this.cmbWash1.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbWash1.Tag.ToString(), this.cmbWash1.Text, "1",
                    EnumOperationRole.WashingHandNurse);
            }
            if (this.cmbWash2.Tag != null && this.cmbWash2.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbWash2.Tag.ToString(), this.cmbWash2.Text, "1",
                    EnumOperationRole.WashingHandNurse);
            }
            if (this.cmbWash3.Tag != null && this.cmbWash3.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbWash3.Tag.ToString(), this.cmbWash3.Text, "1",
                    EnumOperationRole.WashingHandNurse);
            }
            #endregion

            #region Ѳ�ػ�ʿ
            if (this.cmbXH1.Tag != null && this.cmbXH1.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbXH1.Tag.ToString(), this.cmbXH1.Text, "1",
                    EnumOperationRole.ItinerantNurse);
            }
            if (this.cmbXH2.Tag != null && this.cmbXH2.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbXH2.Tag.ToString(), this.cmbXH2.Text, "1",
                    EnumOperationRole.ItinerantNurse);
            }
            if (this.cmbXH3.Tag != null && this.cmbXH3.Tag.ToString() != "")
            {
                this.operationRecord.OperationAppllication.AddRole(this.cmbXH3.Tag.ToString(), this.cmbXH3.Text, "1",
                    EnumOperationRole.ItinerantNurse);
            }

            if(!string.IsNullOrEmpty(txtTmpHelper1.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = "888888";//��Ա����
                role.Name = this.txtTmpHelper1.Text;
                role.RoleType.ID = EnumOperationRole.TmpHelper1.ToString();//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);

                //this.operationRecord.OperationAppllication.HelperAl.Add(role);
                roleArrayList.Add(role);
            }

            if (!string.IsNullOrEmpty(txtTmpHelper2.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = "888888";//��Ա����
                role.Name = this.txtTmpHelper2.Text;
                role.RoleType.ID = EnumOperationRole.TmpHelper2.ToString();//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);

                //this.operationRecord.OperationAppllication.HelperAl.Add(role);
                roleArrayList.Add(role);
            }

            #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
            if (!string.IsNullOrEmpty(txtTmpStudent1.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = "888888";//��Ա����
                role.Name = this.txtTmpStudent1.Text;
                role.RoleType.ID = EnumOperationRole.TmpStudent1.ToString();//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);

                //this.operationRecord.OperationAppllication.HelperAl.Add(role);
                roleArrayList.Add(role);
            }

            if (!string.IsNullOrEmpty(txtTmpStudent2.Text))
            {
                role = new ArrangeRole();
                role.OperationNo = this.operationRecord.OperationAppllication.ID;//�����
                role.ID = "888888";//��Ա����
                role.Name = this.txtTmpStudent2.Text;
                role.RoleType.ID = EnumOperationRole.TmpStudent2.ToString();//��ɫ����
                role.ForeFlag = "1";//����¼��
                //this.operationRecord.OperationAppllication.RoleAl.Add(role);

                //this.operationRecord.OperationAppllication.HelperAl.Add(role);
                roleArrayList.Add(role);
            } 
            #endregion


            this.operationRecord.OperationAppllication.RoleAl.AddRange(roleArrayList);

            #endregion
            //��������
            this.operationRecord.OperationAppllication.OperateKind = System.Convert.ToString(this.cmbOperKind.SelectedItem.ID);
            //�Ƿ��Ⱦ
            this.operationRecord.IsInfected = this.cbxInfect.Checked;
            //�Ƿ��о�
            this.operationRecord.OperationAppllication.IsGermCarrying = this.cbxGerm.Checked;

            this.operationRecord.OperationAppllication.IsFinished = true;
            this.operationRecord.OperationAppllication.PatientInfo.Weight = "0";//����
            this.operationRecord.OperationAppllication.ExecStatus = "4";//�Ǽ����
            this.operationRecord.OperationAppllication.OperationDoctor.Dept.ID  = this.comDept.Tag.ToString();
            //{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
            this.operationRecord.OperationAppllication.InciType.ID = this.cmbIncityep.Tag.ToString();

            return 0;
        }

        /// <summary>
        /// ��Ч����֤
        /// </summary>
        /// <returns></returns>
        private int Valid()
        {
            if (this.IsNew)
            {
                if (this.operationRecord.OperationAppllication.IsValid == false)
                {
                    MessageBox.Show("�����뵥�Ѿ�����!", "��ʾ");
                    return -1;
                }
            }

            if (operationRecord.OperationAppllication.PatientInfo.ID.Length == 0)
            {
                MessageBox.Show("��ѡ�����뻼��!", "��ʾ");
                return -1;
            }

            if (this.txtOperation.Tag == null && this.txtOperation2.Tag == null && this.txtOperation3.Tag == null)
            {
                MessageBox.Show("���������Ʋ���Ϊ��!", "��ʾ");
                txtOperation.Focus();
                return -1;
            }
            if (dtBeginDate.Value > dtEndDate.Value)
            {
                MessageBox.Show("��ʼʱ�䲻�ܴ��ڽ���ʱ��");
                dtBeginDate.Focus();
                return -1;
            }
            if (this.cmbRoom.Tag == null || this.cmbRoom.Tag.ToString() == "")
            {
                MessageBox.Show("���Ų���Ϊ��!", "��ʾ");
                cmbRoom.Focus();
                return -1;
            }

            if (this.dtBeginDate.Value > this.dtEndDate.Value)
            {
                MessageBox.Show("������ʼʱ�����С�ڽ���ʱ��!", "��ʾ");
                return -1;
            }

            //if (this.cmbOrder.Text == "")
            //{
            //    MessageBox.Show("̨����Ϊ��!", "��ʾ");
            //    cmbOrder.Focus();
            //    return -1;
            //}
            if (this.cmbDoctor.Tag == null || this.cmbDoctor.Tag.ToString() == "")
            {
                MessageBox.Show("���߲���Ϊ��!", "��ʾ");
                cmbDoctor.Focus();
                return -1;
            }
            if (this.comDept.Tag == null || this.comDept.Tag.ToString() == "" || this.comDept.Text.Trim() == "")
            {
                MessageBox.Show("���߿��Ҳ���Ϊ��");
                comDept.Focus();
                return -1;
            }
            if (this.cmbHelper1.Tag == null || this.cmbHelper1.Tag.ToString() == "")
            {
                MessageBox.Show("һ������Ϊ��!", "��ʾ");
                return -1;
            }

            //if ((this.cmbWash1.Tag == null || this.cmbWash1.Tag.ToString() == "") &&
            //    (this.cmbWash2.Tag == null || this.cmbWash2.Tag.ToString() == "") &&
            //    (this.cmbWash3.Tag == null || this.cmbWash3.Tag.ToString() == ""))
            //{
            //    MessageBox.Show("ϴ�ֻ�ʿ����Ϊ��!", "��ʾ");
            //    cmbWash1.Focus();
            //    return -1;
            //}

            if (this.cmbOpType.Tag == null || this.cmbOpType.Tag.ToString() == "")
            {
                MessageBox.Show("������ģ����Ϊ��!", "��ʾ");
                cmbOpType.Focus();
                return -1;
            }

            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.rtbApplyNote.Text.Trim(), 200) == false)
            {
                MessageBox.Show("����˵������С��100������!", "��ʾ");
                rtbApplyNote.Focus();
                return -1;
            }
            string Oper1 = Conobj(txtOperation.Tag); //��һ����
            string Oper2 = Conobj(txtOperation2.Tag); //�ڶ�����
            string Oper3 = Conobj(txtOperation3.Tag); //��������
            if ((Oper1 == Oper2 && Oper1 != "") || (Oper1 == Oper3 && Oper3 != "") || (Oper2 == Oper3 && Oper3 != ""))
            {
                MessageBox.Show("������Ŀ�����ظ�");
                txtOperation.Focus();
                return -1;
            }
            string Oper11 = Conobj(txtOperation.Text); //��һ����
            string Oper12 = Conobj(txtOperation2.Text); //�ڶ�����
            string Oper13 = Conobj(txtOperation3.Text); //��������
            if ((Oper11 == Oper12 && Oper11 != "") || (Oper11 == Oper13 && Oper13 != "") || (Oper12 == Oper13 && Oper13 != ""))
            {
                MessageBox.Show("������Ŀ�����ظ�");
                txtOperation.Focus();
                return -1;
            }
            string Helper1 = Conobj(cmbHelper1.Tag); //һ��
            string Helper2 = Conobj(cmbHelper2.Tag); //һ��
            string Helper3 = Conobj(cmbHelper3.Tag); //һ��
            if ((Helper1 == Helper2 && Helper1 != "") || (Helper1 == Helper3 && Helper3 != "") || (Helper2 == Helper3 && Helper3 != ""))
            {
                MessageBox.Show("һ���������������ظ�");
                cmbHelper2.Focus();
                return -1;
            }
            string Wash1 = Conobj(cmbWash1.Tag); //ϴ�ֻ�ʿ
            string Wash2 = Conobj(cmbWash2.Tag); //ϴ�ֻ�ʿ
            string Wash3 = Conobj(cmbWash3.Tag); //ϴ�ֻ�ʿ
            if ((Wash1 == Wash2 && Wash1 != "") || (Wash1 == Wash3 && Wash3 != "") || (Wash2 == Wash3 && Wash3 != ""))
            {
                MessageBox.Show("����ϴ�ֻ�ʿ�����ظ�");
                cmbWash2.Focus();
                return -1;
            }

            if (cmbXH1.Tag == null || string.IsNullOrEmpty(cmbXH1.Tag.ToString())) 
            {
                MessageBox.Show("Ѳ��һ����Ϊ��!", "��ʾ");
                return -1;
            }

            if ((!string.IsNullOrEmpty(txtTmpHelper1.Text)) && (!string.IsNullOrEmpty(txtTmpHelper2.Text)))
            {
                if (txtTmpHelper1.Text == txtTmpHelper2.Text)
                {
                    MessageBox.Show("��ʱ���ֲ����ظ�!", "��ʾ");
                    return -1;
                }
            }

            #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
            if ((!string.IsNullOrEmpty(txtTmpStudent1.Text)) && (!string.IsNullOrEmpty(txtTmpStudent2.Text)))
            {
                if (txtTmpStudent1.Text == txtTmpStudent2.Text)
                {
                    MessageBox.Show("������Ա�����ظ�!", "��ʾ");
                    return -1;
                }
            } 
            #endregion

            string XH1 = Conobj(cmbXH1.Tag); //Ѳ�ػ�ʿ
            string XH2 = Conobj(cmbXH2.Tag); //Ѳ�ػ�ʿ
            string XH3 = Conobj(cmbXH3.Tag); //Ѳ�ػ�ʿ
            if ((XH1 == XH2 && XH1 != "") || (XH1 == XH3 && XH3 != "") || (XH2 == XH3 && XH3 != ""))
            {
                MessageBox.Show("����Ѳ�ػ�ʿ�����ظ�");
                cmbXH2.Focus();
                return -1;
            }
            return 0;
        }

        private string Conobj(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            try
            {
                PreSave();

                //int CaseReturn = 0;


                if (Valid() == -1)
                    return -1;

                if (this.GetValue() == -1)
                    return -1;

                //���ݿ�����
                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                //Neusoft.FrameWork.Management.Transaction trans = new
                //    Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                //trans.BeginTransaction();

                Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
                Environment.RecordManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                //this.opdMgr.SetTrans(trans.Trans);
                //this.icdMgr.SetTrans(trans.Trans);
                //this.iteMgr.SetTrans(trans.Trans);
                int rtn = 0;

                //��ȡ���ݿ�ϵͳʱ�䣬ʹ�����ǼǺͲ����ǼǵĲ���ʱ����һ�¡�����Add By Maokb
                DateTime inTime;
                inTime = Environment.OperationManager.GetDateTimeFromSysDateTime();
                // TODO: ��Ӳ���
                //this.opDetail.OperDate = inTime;
                this.operationRecord.OperDate = inTime;

                //�ж��Ƿ���벡��������Ϣ
                //CaseReturn = GetDetail(inTime);

                try
                {
                    this.operationRecord.OperationAppllication.PatientInfo.PVisit.PatientLocation.Dept.ID = this.lbDept.Tag.ToString();
                    if (this.IsNew)//����
                    {
                        #region new
                        if (Environment.RecordManager.AddOperatorRecord(this.operationRecord) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.RecordManager.Err, "��ʾ");
                            return -1;
                        }

                        //�������뵥״̬
                        if (this.isRenew == false)
                        {
                            rtn = Environment.OperationManager.DoOperatorRecord(this.operationRecord.OperationAppllication.ID);
                            if (rtn == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                                return -1;
                            }
                            if (rtn == 0)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("�����뵥״̬�Ѿ��ı�,��ˢ����Ļ����¼��!", "��ʾ");
                                return -1;
                            }
                        }
                        #region �Ǽ�������Ŀ
                        if (Environment.OperationManager.DelOperationInfo(this.operationRecord.OperationAppllication) == -1)//ɾ��������Ŀ
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                            return -1;
                        }
                        //��Ա����뵥���漰�����������������Ŀ��Ϣ
                        foreach (OperationInfo OperateInfo in this.operationRecord.OperationAppllication.OperationInfos)
                        {
                            //���������Ŀ��Ϣ
                            if (Environment.OperationManager.AddOperationInfo(this.operationRecord.OperationAppllication, OperateInfo) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                                return -1;
                            }
                        }
                        #endregion
                        #region �Ǽ���Ա��Ϣ
                        if (Environment.OperationManager.ProcessRoleForApply(this.operationRecord.OperationAppllication) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                            return -1;
                        }
                        #endregion 
                        #endregion
                    }
                    else//�޸�
                    {
                        #region modify

                        if (Environment.RecordManager.GetModifyEnabled() != "1")
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("��û���޸������ǼǼ�¼��Ȩ��!", "��ʾ");
                            return -1;
                        }

                        //���ж�״̬
                        if (this.operationRecord.IsValid == false)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("�����뵥�Ѿ�����!", "��ʾ");
                            return -1;
                        }

                        if (Environment.RecordManager.UpdateOperatorRecord(this.operationRecord) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.RecordManager.Err, "��ʾ");
                            return -1;
                        }

                        #region �Ǽ�������Ŀ
                        if (Environment.OperationManager.DelOperationInfo(this.operationRecord.OperationAppllication) == -1)//ɾ��������Ŀ
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                            return -1;
                        }
                        //��Ա����뵥���漰�����������������Ŀ��Ϣ
                        foreach (OperationInfo OperateInfo in this.operationRecord.OperationAppllication.OperationInfos)
                        {
                            //���������Ŀ��Ϣ
                            if (Environment.OperationManager.AddOperationInfo(this.operationRecord.OperationAppllication, OperateInfo) == -1)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                                return -1;
                            }
                        }
                        #endregion
                        #region �Ǽ���Ա��Ϣ
                        if (Environment.OperationManager.ProcessRoleForApply(this.operationRecord.OperationAppllication) == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(Environment.OperationManager.Err, "��ʾ");
                            return -1;
                        }
                        #endregion

                        // TODO: ��Ӳ���
                        #region �Ǽǲ�����Ϣ --Add By maokb

                        //if (CaseReturn == 0)
                        //{
                        //    //ɾ��ԭ����¼
                        //    if (opdMgr.DeleteByCodeAndTime(operDate, this.opDetail.InpatientNO) == -1)
                        //    {
                        //        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //        MessageBox.Show(this.opdMgr.Err, "��ʾ");
                        //        return -1;
                        //    }

                        //    //��Ӹ��ĺ�ļ�¼
                        //    if (this.alDetail != null)
                        //    {
                        //        foreach (neusoft.HISFC.Object.Case.OperationDetail opdinfo in alDetail)
                        //        {
                        //            if (opdMgr.Insert(neusoft.HISFC.Management.Case.frmTypes.DOC, opdinfo) == -1)
                        //            {
                        //                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //                MessageBox.Show(this.opdMgr.Err, "��ʾ");
                        //                return -1;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        //        MessageBox.Show("û��Ҫ�Ǽǵ�������Ŀ", "��ʾ");
                        //        return -1;
                        //    }
                        //}

                        #endregion
                        #endregion
                    }
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    this.isRenew = false;
                }
                catch (Exception e)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(e.Message, "��ʾ");
                    return -1;
                }

                MessageBox.Show("�Ǽǳɹ�!", "��ʾ");
                this.ucDiag1.Visible = false;
                this.ucOpItem1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// ���ϵǼǵ�
        /// </summary>
        /// <returns></returns>
        public int Cancel()
        {


            if (this.isCancled)
            {
                MessageBox.Show("����������������!", "��ʾ");
                return -1;
            }
            if (this.IsNew)
            {
                MessageBox.Show("��������û�����Ǽ�,��������,����û��˫������������Ϣ!", "��ʾ");
                return -1;
            }

            DialogResult dr = MessageBox.Show("�����ϡ��������Ѹ�������Ϊ�����ϡ�״̬����״̬���ɻָ�\n���Ƿ����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.No)
            {
                return -1;
            }

            //��ʼ����

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //t.BeginTransaction();

            int rtn = 0;
            Environment.RecordManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            try
            {
                //û������������Ŀ,���ͳ����Ҫ�Լ�������Ϲ���,���߹���һ��
                rtn = Environment.RecordManager.CancelRecord(this.operationRecord.OperationAppllication.ID);
                if (rtn == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show(Environment.RecordManager.Err, "��ʾ");
                    return -1;
                }

                rtn = Environment.RecordManager.CacelApply(this.operationRecord.OperationAppllication.ID);
                if (rtn == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("�õǼǵ�����ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                if (rtn == 0)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("�õǼǵ��Ѿ�����!", "��ʾ");
                    return -1;
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();
            }
            catch (Exception e)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show(e.Message, "��ʾ");
                return -1;
            }
            MessageBox.Show("���ϳɹ�!", "��ʾ");

            return 0;
        }
        //{80D89813-7B64-4acf-A2CD-55BFD9F1E7C6}
        public int DeleleteRegInfo()
        {
            //ɾ���Ǽ���Ϣ

            DialogResult dr = MessageBox.Show("��ȡ�����������Ѹ���������ָ�����δ�Ǽǡ�״̬\n���Ƿ����", "��ʾ", MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.No)
            {
                return -1;
            }

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            int returnValue = Environment.RecordManager.DeleteOpsRecord(this.operationRecord.OperationAppllication.ID);

            //returnValue =  Environment.OperationManager.DelOperationInfo(this.operationRecord.OperationAppllication);
            //returnValue =  Environment.OperationManager.DelArrangeRoleAll(this.operationRecord.OperationAppllication);

            returnValue = Environment.OperationManager.DoAnaeRecord(this.operationRecord.OperationAppllication.ID,"3");

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            return 1;

        }
        /// <summary>
        /// ����
        /// </summary>
        public void SetEnable(bool type)
        {
            lbName.Enabled = type;
            //			lbName.BackColor = System.Drawing.Color.MintCream;
            lbSex.Enabled = type;
            //			lbSex.BackColor = System.Drawing.Color.MintCream;
            lbAge.Enabled = type;
            //			lbAge.BackColor = System.Drawing.Color.MintCream;
            lbPatient.Enabled = type;
            //			lbPatient.BackColor = System.Drawing.Color.MintCream;
            lbPaykind.Enabled = type;
            //			lbPaykind.BackColor = System.Drawing.Color.MintCream;

            //������п����޸�
            //			lbDept.Enabled = type;
            //			lbDept.BackColor = System.Drawing.Color.MintCream;

            lbBed.Enabled = type;
            //			lbBed.BackColor = System.Drawing.Color.MintCream;
            lbFree.Enabled = type;
            //			lbFree.BackColor = System.Drawing.Color.MintCream;
            lbOpsDept.Enabled = type;
            //			lbOpsDept.BackColor = System.Drawing.Color.MintCream;
            //lbTableType.Enabled = type;
            //			lbTableType.BackColor = System.Drawing.Color.MintCream;
            lbApplyDoct.Enabled = type;
            //			lbApplyDoct.BackColor = System.Drawing.Color.MintCream;
            lbPreDate.Enabled = type;
            //			lbPreDate.BackColor = System.Drawing.Color.MintCream;
            txtDiag.Enabled = type;
            txtDiag.BackColor = System.Drawing.Color.MintCream;
            txtDiag2.Enabled = type;
            txtDiag2.BackColor = System.Drawing.Color.MintCream;
            txtDiag3.Enabled = type;
            txtDiag3.BackColor = System.Drawing.Color.MintCream;
        }

        public int Print()
        {
            if (this.recordFormPrint == null)
            {
                this.recordFormPrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IRecordFormPrint)) as Neusoft.HISFC.BizProcess.Interface.Operation.IRecordFormPrint;
                if (this.recordFormPrint == null)
                {
                    MessageBox.Show("��ýӿ�IRecordFormPrint��������ϵͳ����Ա��ϵ��");

                    return -1;
                }
            }
            if (this.GetValue() == -1)
                return -1;

            this.recordFormPrint.OperationRecord = this.operationRecord;
            return this.recordFormPrint.Print();
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData.GetHashCode() == Keys.Escape.GetHashCode())
            {
                this.ucOpItem1.Visible = false;
            }
            return base.ProcessDialogKey(keyData);
        }
        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                return new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IRecordFormPrint) };
            }
        }

        #endregion   


        #region ����˳��

        private void lbAnae_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtBeginDate.Focus();
            }
        }

        private void dtBeginDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtEndDate.Focus();
            }
        }

        private void dtEndDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbRoom.Focus();
            }
        }

        private void cmbRoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbOrder.Focus();
            }
        }

        private void cmbOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbDoctor.Focus();
            }
        }

        private void cmbDoctor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comDept.Focus();
            }
        }

        private void comDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper1.Focus();
            }
        }

        private void cmbHelper1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper2.Focus();
            }
        }

        private void cmbHelper2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbHelper3.Focus();
            }
        }

        private void cmbHelper3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbWash1.Focus();
            }
        }

        private void cmbWash1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbWash2.Focus();
            }
        }

        private void cmbWash2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbWash3.Focus();
            }
        }

        private void cmbWash3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbXH1.Focus();
            }
        }

        private void cmbXH1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbXH2.Focus();
            }
        }

        private void cmbXH2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbXH3.Focus();
            }
        }

        private void cmbXH3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbOpType.Focus();
            }
        }

        private void cmbOpType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbOperKind.Focus();
            }
        }

        private void cmbOperKind_KeyDown(object sender, KeyEventArgs e)
        {
            ////{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
            if (e.KeyCode == Keys.Enter)
            {
                this.cmbIncityep.Focus();
            }
        }

        private void cbxInfect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                rtbApplyNote.Focus();
            }
        }
        //{B9DDCC10-3380-4212-99E5-BB909643F11B}
        private void cmbAnseWay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.lbAnae.Focus();
            }
        }

        private void cmbIncityep_KeyDown(object sender, KeyEventArgs e)
        {
            ////{37A0B524-70DB-413c-8C33-AAC69C40EAC6}
            if (e.KeyCode == Keys.Enter)
            {
                cbxInfect.Focus();
            }
        } 

        #endregion


        #region  �Զ��� ���� �� ���

        private void cbxCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxCustom.Checked)
            {
                this.txtDiag.IsEnter2Tab = true;
                this.txtDiag.TextChanged -= new EventHandler(txtDiag1_TextChanged);
                this.txtDiag.KeyDown -= new KeyEventHandler(txtDiag1_KeyDown);

                this.txtDiag2.IsEnter2Tab = true;
                this.txtDiag2.TextChanged -= new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown -= new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.IsEnter2Tab = true;
                this.txtDiag3.TextChanged -= new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown -= new KeyEventHandler(txtDiag3_KeyDown);



                this.txtOperation.IsEnter2Tab = true;
                this.txtOperation.TextChanged -= new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown -= new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.IsEnter2Tab = true;
                this.txtOperation2.TextChanged -= new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown -= new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.IsEnter2Tab = true;
                this.txtOperation3.TextChanged -= new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown -= new KeyEventHandler(txtOperation3_KeyDown);
            }
            else
            {
                this.txtDiag.IsEnter2Tab = false;
                this.txtDiag2.IsEnter2Tab = false;
                this.txtDiag3.IsEnter2Tab = false;

                this.txtOperation.IsEnter2Tab = false;
                this.txtOperation2.IsEnter2Tab = false;
                this.txtOperation3.IsEnter2Tab = false;

                this.txtDiag.TextChanged -= new EventHandler(txtDiag1_TextChanged);
                this.txtDiag.KeyDown -= new KeyEventHandler(txtDiag1_KeyDown);

                this.txtDiag2.TextChanged -= new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown -= new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.TextChanged -= new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown -= new KeyEventHandler(txtDiag3_KeyDown);


                this.txtOperation.TextChanged -= new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown -= new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.TextChanged -= new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown -= new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.TextChanged -= new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown -= new KeyEventHandler(txtOperation3_KeyDown);


                this.txtDiag.TextChanged += new EventHandler(txtDiag1_TextChanged);
                this.txtDiag.KeyDown += new KeyEventHandler(txtDiag1_KeyDown);

                this.txtDiag2.TextChanged += new EventHandler(txtDiag2_TextChanged);
                this.txtDiag2.KeyDown += new KeyEventHandler(txtDiag2_KeyDown);

                this.txtDiag3.TextChanged += new EventHandler(txtDiag3_TextChanged);
                this.txtDiag3.KeyDown += new KeyEventHandler(txtDiag3_KeyDown);


                this.txtOperation.TextChanged += new EventHandler(txtOperation_TextChanged);
                this.txtOperation.KeyDown += new KeyEventHandler(txtOperation_KeyDown);

                this.txtOperation2.TextChanged += new EventHandler(txtOperation2_TextChanged);
                this.txtOperation2.KeyDown += new KeyEventHandler(txtOperation2_KeyDown);

                this.txtOperation3.TextChanged += new EventHandler(txtOperation3_TextChanged);
                this.txtOperation3.KeyDown += new KeyEventHandler(txtOperation3_KeyDown);
            }
        }


        private void PreSave()
        {
            if (!this.cbxCustom.Checked)
            {
                return;
            }

            if (!string.IsNullOrEmpty(txtDiag.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtDiag.Text;

                this.txtDiag.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;

                this.txtDiag.Tag = item;

                #endregion
            }

            if (!string.IsNullOrEmpty(txtDiag2.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtDiag2.Text;

                this.txtDiag2.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;

                this.txtDiag2.Tag = item;

                #endregion
            }


            if (!string.IsNullOrEmpty(txtDiag3.Text.Trim()))
            {
                #region donggq

                Neusoft.HISFC.Models.HealthRecord.ICD item = new Neusoft.HISFC.Models.HealthRecord.ICD();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtDiag3.Text;

                this.txtDiag3.Text = (item as Neusoft.HISFC.Models.HealthRecord.ICD).Name;

                this.txtDiag3.Tag = item;

                #endregion
            }


            if (!string.IsNullOrEmpty(txtOperation.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation.Text;

                this.txtOperation.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;

                this.txtOperation.Tag = item;

                #endregion
            }

            if (!string.IsNullOrEmpty(txtOperation2.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();



                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation2.Text;

                this.txtOperation2.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;

                this.txtOperation2.Tag = item;

                #endregion
            }

            if (!string.IsNullOrEmpty(txtOperation3.Text.Trim()))
            {
                #region donggq
                Neusoft.HISFC.Models.Fee.Item.Undrug item = new Neusoft.HISFC.Models.Fee.Item.Undrug();

                item.ID = GetCustomOpitemNo();
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = GetCustomOpitemNo();
                }
                item.Name = this.txtOperation3.Text;

                this.txtOperation3.Text = (item as Neusoft.HISFC.Models.Fee.Item.Undrug).Name;

                this.txtOperation3.Tag = item;

                #endregion
            }

        }



        public string GetCustomOpitemNo()
        {
            Neusoft.HISFC.BizProcess.Integrate.Operation.Operation op = new Neusoft.HISFC.BizProcess.Integrate.Operation.Operation();

            string strSql = "SELECT  Seq_local_itemno.NEXTVAL FROM dual";
            string val = string.Empty;

            if (op.ExecQuery(strSql) == -1)
            {
                return val;
            }
            try
            {
                if (op.Reader.Read())
                {
                    return op.Reader.GetValue(0).ToString();
                }
                else
                {
                    return val;
                }
            }
            catch
            {
                return val;
            }

            return val;
        }

        private void ucRegistrationForm_Load(object sender, EventArgs e)
        {
            this.cbxCustom.Checked = true;
        }


        #endregion

    }
}
