using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.Components.Manager.Controls
{
    /// <summary>
    /// [��������: ��Աά��]<br></br>
    /// [�� �� ��: Ѧռ��]<br></br>
    /// [����ʱ��: 2006��12��11]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucEmployeeInfoPanel : UserControl
    {   
        //��Ա������
        Neusoft.HISFC.BizLogic.Manager.Person personMgr = new Neusoft.HISFC.BizLogic.Manager.Person();
        //ƴ��������
        Neusoft.HISFC.BizLogic.Manager.Spell spellMgr = new Neusoft.HISFC.BizLogic.Manager.Spell();
        //��Աʵ����
        Neusoft.HISFC.Models.Base.Employee employee = new Neusoft.HISFC.Models.Base.Employee();
        public bool tr = false;//�ж�ˢ��
        private bool isModify = false;//�Ƿ��޸�
        /// <summary>
        /// �Ƿ�����޸�
        /// </summary>
        public bool IsModify
        {
            get
            {
                return this.isModify;
            }
            set
            {
                this.txtEmployeeCode.ReadOnly = value;
                this.btAutoID.Visible = !value;
            }
        }
        //{3BAA59AB-14DE-496e-B77A-E7C298D3245B}
        private bool isModifedlevelAndRemark = false;
        //{3BAA59AB-14DE-496e-B77A-E7C298D3245B}
        public bool IsModifedlevelAndRemark
        {
            get { return isModifedlevelAndRemark; }
            set
            {
                isModifedlevelAndRemark = value;
                //{3BAA59AB-14DE-496e-B77A-E7C298D3245B}
                if (this.isModifedlevelAndRemark == true )
                {
                    this.SetEnable();
                }
            }
        }

        
     
        public ucEmployeeInfoPanel()
        {
            InitializeComponent();
            InitialCombox();
        }
        /// <summary>
        /// �вι��캯��
        /// </summary>
        /// <param name="empl"></param>
        public ucEmployeeInfoPanel(Neusoft.HISFC.Models.Base.Employee empl)
        {
            InitializeComponent();
            this.bttAdd.Visible = false;
            this.txtEmployeeCode.ReadOnly = false;
            this.employee = empl;
            InitialCombox();
            setInfoToControls();
        }

        /// <summary>
        /// ��ʼ��ComboBoxѡ��
        /// </summary>
        private void InitialCombox()
        {
            Neusoft.HISFC.BizLogic.Manager.Department deptMgr=new Neusoft.HISFC.BizLogic.Manager.Department();
            //ֻ��ʾ���˻�ʿվ����Ŀ����б�
            ArrayList aldepartments = deptMgr.GetDeptNoNurse();
            this.comboDeptType.IsListOnly = true;
            this.comboDeptType.AddItems(aldepartments);//�����������ComboBox

            this.comboPersonType.IsListOnly = true;
            this.comboPersonType.AddItems(Neusoft.HISFC.Models.Base.EmployeeTypeEnumService.List());//�����Ա����������

            this.comboSex.IsListOnly = true;
            this.comboSex.AddItems(Neusoft.HISFC.Models.Base.SexEnumService.List());//�Ա�

            ArrayList alNurseDept = deptMgr.GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType.N);
            this.comboNurse.IsListOnly = true;
            this.comboNurse.AddItems(alNurseDept);//��������վ������

            this.comboDuty.IsListOnly = true;
            this.comboDuty.AddItems(GetConstant(Neusoft.HISFC.Models.Base.EnumConstant.POSITION));//ְ��

            this.comboLevel.IsListOnly = true;
            this.comboLevel.AddItems(GetConstant(Neusoft.HISFC.Models.Base.EnumConstant.LEVEL));//ְ��

            this.comboPersonEdu.IsListOnly = true;
            this.comboPersonEdu.AddItems(GetConstant(Neusoft.HISFC.Models.Base.EnumConstant.EDUCATION));//ѧ��
            
        }

        /// <summary>
        /// ���ݲ������ͻ��ArrayList
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ArrayList GetConstant(Neusoft.HISFC.Models.Base.EnumConstant type)
        {
            //����������
            Neusoft.HISFC.BizLogic.Manager.Constant consManager = new Neusoft.HISFC.BizLogic.Manager.Constant();

            ArrayList constList = consManager.GetList(type);
            if (constList == null)
                throw new Neusoft.FrameWork.Exceptions.ReturnNullValueException();
            
            
            return constList;
         
        }
        
        /// <summary>
        /// ���ݴ��������Ϣ���UC
        /// </summary>
        private void setInfoToControls()
        {
            this.txtEmployeeCode.Text = this.employee.ID;//��Ա���� 
            this.txtEmployeeName.Text = this.employee.Name;//��Ա����
            this.txtSpell_Code.Text = this.employee.SpellCode;//ƴ����
            this.txtWB_Code.Text = this.employee.WBCode;//�����
            this.comboSex.Tag = this.employee.Sex.ID;//�Ա�

            //[2007-01-25]��ֹ.NET��ORACLE��ʾ��Χ��ͬ
            //{997CB50F-1CE4-40d3-9A67-5128EAA10FD7}
            this.comboBirthday.Value = this.employee.Birthday;//��������
            //if (this.employee.Birthday.CompareTo(DateTime.MinValue.AddYears(1969)) <= 0 )
            //{
            //    this.comboBirthday.Value = DateTime.MinValue.AddYears(1969);
            //}
            //else
            //{
            //    this.comboBirthday.Value = this.employee.Birthday;//��������
            //}

            this.comboPersonEdu.Tag = this.employee.GraduateSchool.ID;//��ҵѧУ
            this.txtIdentity.Text = this.employee.IDCard;//���֤
            this.comboDuty.Tag = this.employee.Duty.ID;//ְ��
            this.comboLevel.Tag = this.employee.Level.ID;//ְ��
            this.comboDeptType.Tag = this.employee.Dept.ID;//��������
            this.comboNurse.Tag = this.employee.Nurse.ID;//��������վ
            this.comboPersonType.Tag = this.employee.EmployeeType.ID;//��Ա����
            this.numSortId.Text = this.employee.SortID.ToString();//˳���
            if (this.employee.IsExpert)//�Ƿ�Ϊר��
            {
                this.neuRadioButton1.Checked=true;
            }
            else
            {
                this.neuRadioButton2.Checked=true;
            }
            if (this.employee.IsCanModify)//�Ƿ��ܸ�Ʊ��
            {
                this.neuRadioButton3.Checked=true;
            }
            else
            {
                this.neuRadioButton4.Checked=true;
            }
            if (this.employee.IsNoRegCanCharge)//�Ƿ�ֱ���շ�
            {
                this.neuRadioButton5.Checked=true;
            }
            else
            {
                this.neuRadioButton6.Checked=true;
            }
            switch (this.employee.ValidState)//��Ч��
            {
                case Neusoft.HISFC.Models.Base.EnumValidState.Valid:
                    this.radioValidate1.Checked=true;
                    break;
                case Neusoft.HISFC.Models.Base.EnumValidState.Invalid:
                    this.radioValidate2.Checked=true;
                    break;
                case Neusoft.HISFC.Models.Base.EnumValidState.Ignore:
                    this.radioValidate3.Checked=true;
                    break;
                default:
                    break;
            }
            this.txtUser_Code.Text = this.employee.UserCode;
            //{6A8C59DC-91FE-4246-A923-06A011918614}
            this.rtxtMark.SuperText = this.employee.Memo;

            #region donggq--20101118--{45E71A4E-803A-47fd-AC24-9BED6E530F16}--����ǩ��

            byte[] digitalSign = personMgr.GetEmplDigitalSignByID(employee.ID);
            if (digitalSign != null)
            {
                System.IO.MemoryStream msPic = new System.IO.MemoryStream(digitalSign);
                Bitmap bmpt = new Bitmap(msPic);
                this.picBox.Image = bmpt;
            } 

            #endregion


        }

        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        private bool ValueValidated()
        {   
            //��Ա���벻��Ϊ��
            if (this.txtEmployeeCode.Text.Trim() == "")
            {
                MessageBox.Show("��Ա���벻��Ϊ�գ�", "��ʾ", MessageBoxButtons.OK);
                this.txtEmployeeCode.Focus();
                return false;
            }
            //��Ա���볤�Ȳ��ܳ���6λ�ַ�
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtEmployeeCode.Text.Trim(), 6) == false)
            {
                MessageBox.Show("��Ա�������,�뱣����λ�ַ�!", "��ʾ", MessageBoxButtons.OK);
                this.txtEmployeeCode.Focus();
                return false;
            }
            //��Ա���Ʋ���Ϊ��
            if (this.txtEmployeeName.Text.Trim() == "")
            {
                MessageBox.Show("��Ա���Ʋ���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK);
                this.txtEmployeeName.Focus();
                return false;
            }
            //��Ա���Ƴ��Ȳ��ܳ���10λ�ַ�
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.txtEmployeeName.Text.Trim(), 10) == false)
            {
                MessageBox.Show("��Ա���ƹ���,�뱣��5λ�ֺ��ֻ�10λ�ַ�!", "��ʾ", MessageBoxButtons.OK);
                this.txtEmployeeName.Focus();
                return false;
            }
            //��Ա�Ա���Ϊ��
            if (this.comboSex.Text == "")
            {
                MessageBox.Show("��Ա�Ա���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK);
                this.comboSex.Focus();
                return false;
            }
            //ְ����Ų���Ϊ��
            if (this.comboDuty.Text == "")
            {
                MessageBox.Show("ְ����Ų���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK);
                this.comboDuty.Focus();
                return false;
            }
            //ְ�����Ų���Ϊ��
            if (this.comboLevel.Text == "")            
            {
                MessageBox.Show("ְ�����Ų���Ϊ�գ�","��ʾ",MessageBoxButtons.OK);
                this.comboLevel.Focus();
                return false;
            }
            //��Ա���Ͳ���Ϊ��
            if (this.comboPersonType.Text == "")
            {
                MessageBox.Show("��Ա���Ͳ���Ϊ�գ�","��ʾ",MessageBoxButtons.OK);
                this.comboPersonType.Focus();
                return false;
            }
            //�������Ҳ���Ϊ��
            if (this.comboDeptType.Text == "")
            {
                MessageBox.Show("�������Ҳ���Ϊ�գ�", "��ʾ", MessageBoxButtons.OK);
                this.comboDeptType.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��տؼ����ݻָ�ԭ״̬
        /// </summary>
        private void ClearUp()
        {
            this.txtEmployeeCode.Text = "";
            this.txtEmployeeName.Text = "";
            this.txtIdentity.Text = "";
            this.txtSpell_Code.Text = "";
            this.txtWB_Code.Text = "";
            this.txtUser_Code.Text = "";
            this.comboBirthday.Value = DateTime.Now;
            this.comboDeptType.SelectedIndex = -1;
            this.comboDuty.SelectedIndex = -1;
            this.comboLevel.SelectedIndex = -1;
            this.comboNurse.SelectedIndex = -1;
            this.comboSex.SelectedIndex = -1;
            this.comboPersonType.SelectedIndex = -1;
            this.comboPersonEdu.SelectedIndex = -1;
            this.radioValidate1.Checked = true;
            this.neuRadioButton2.Checked = true;
            this.neuRadioButton4.Checked = true;
            this.neuRadioButton6.Checked = true;

            #region donggq--20101118--{45E71A4E-803A-47fd-AC24-9BED6E530F16}--����ǩ��

            this.picBox.ImageLocation = string.Empty;
            this.picBox.Image = null;

            #endregion
        }

        /// <summary>
        /// ���ؼ�����ת������Աʵ��
        /// </summary>
        private Neusoft.HISFC.Models.Base.Employee ConvertUcContextToObject()
        {
            Neusoft.HISFC.Models.Base.Employee employee = new Neusoft.HISFC.Models.Base.Employee();
            employee.ID = this.txtEmployeeCode.Text.Trim();//��Ա����
            employee.Name = this.txtEmployeeName.Text.Trim();//��Ա����
            employee.SpellCode = this.txtSpell_Code.Text.Trim();//ƴ����
            employee.WBCode = this.txtWB_Code.Text.Trim();//�����
            employee.Sex.ID = this.comboSex.Tag.ToString();//�Ա�

            //[2007-01-25]��ֹ.NET��ORACLE��ʾ��Χ��ͬ
            if (this.comboBirthday.Checked)
            {
                employee.Birthday = this.comboBirthday.Value;//��������
            }
            else
            {
                employee.Birthday = DateTime.MinValue.AddYears(1969);//Ĭ��Ϊ0001��
            }

            employee.GraduateSchool.ID = this.comboPersonEdu.Tag.ToString();//��ҵѧУ
            employee.IDCard = this.txtIdentity.Text.Trim();//���֤
            employee.Duty.ID = this.comboDuty.Tag.ToString();//ְ��
            employee.Level.ID = this.comboLevel.Tag.ToString();//ְ��
            employee.Dept.ID = this.comboDeptType.Tag.ToString();//��������
            employee.Nurse.ID = this.comboNurse.Tag.ToString();//��������վ
            employee.SortID = Neusoft.FrameWork.Function.NConvert.ToInt32(this.numSortId.Text.Trim());//˳���
            employee.EmployeeType.ID = this.comboPersonType.Tag.ToString();//��Ա���
            if (this.neuRadioButton1.Checked)
            {
                employee.IsExpert = true;//�Ƿ�Ϊר��
            }
            else
            {
                employee.IsExpert = false;
            }
            if (this.neuRadioButton3.Checked)
            {
                employee.IsCanModify = true;//�ܷ��Ʊ��
            }
            else
            {
                employee.IsCanModify = false;
            }
            if (this.neuRadioButton5.Checked)
            {
                employee.IsNoRegCanCharge = true;//�ܷ�ֱ���շ�
            }
            else
            {
                employee.IsNoRegCanCharge = false;
            }
            if (this.radioValidate1.Checked)
            {
                employee.ValidState = Neusoft.HISFC.Models.Base.EnumValidState.Valid;
            }
            else if (this.radioValidate2.Checked)
            {
                employee.ValidState = Neusoft.HISFC.Models.Base.EnumValidState.Invalid;
            }
            else
            {
                employee.ValidState = Neusoft.HISFC.Models.Base.EnumValidState.Ignore;
            }
            employee.UserCode = this.txtUser_Code.Text;
            //{6A8C59DC-91FE-4246-A923-06A011918614}
            employee.Memo = this.rtxtMark.Text;

            #region donggq--20101118--{45E71A4E-803A-47fd-AC24-9BED6E530F16}--����ǩ��

            if (!string.IsNullOrEmpty(this.picBox.ImageLocation)) 
            {
                System.IO.FileStream stream = new System.IO.FileStream(this.picBox.ImageLocation, System.IO.FileMode.Open);
                byte[] byteimg = new byte[stream.Length];
                stream.Read(byteimg, 0, (int)stream.Length);
                stream.Close();
                employee.EmployeeExt.DigitalSign = byteimg;
            }
            else
            {
                employee.EmployeeExt.DigitalSign = null;
            }

            #endregion

            return employee;
        }

        private void bttCancle_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        //private void txtEmployeeCode_Leave(object sender, EventArgs e)
        //{
        //    if (this.txtEmployeeCode.Text != "")
        //    {
        //        if (this.txtEmployeeCode.ReadOnly == false)
        //        {
        //            this.txtEmployeeCode.Text = this.txtEmployeeCode.Text.PadLeft(6, '0');
        //            Neusoft.HISFC.BizLogic.Manager.Person ps = new Neusoft.HISFC.BizLogic.Manager.Person();
        //            int temp = ps.SelectEmployIsExist(this.txtEmployeeCode.Text);
        //            if (temp == -1)
        //            {
        //                MessageBox.Show("��ѯ����,������ܻ��ظ�");
        //            }
        //            else if (temp == 1)
        //            {
        //                MessageBox.Show("�����Ѿ����ڣ�����������");
        //                this.txtEmployeeCode.Focus();
        //                this.txtEmployeeCode.Text = "";
        //            }
        //        }
        //    }
          
        //}

        private void txtEmployeeCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtEmployeeCode.Text != "")
                {
                    if (this.txtEmployeeCode.ReadOnly == false)
                    {
                        this.txtEmployeeCode.Text = this.txtEmployeeCode.Text.PadLeft(6, '0');
                        Neusoft.HISFC.BizLogic.Manager.Person ps = new Neusoft.HISFC.BizLogic.Manager.Person();
                        int temp = ps.SelectEmployIsExist(this.txtEmployeeCode.Text);
                        if (temp == -1)
                        {
                            MessageBox.Show("��ѯ����,������ܻ��ظ�");
                        }
                        else if (temp == 1)
                        {
                            MessageBox.Show("�����Ѿ����ڣ�����������");
                            this.txtEmployeeCode.Focus();
                            this.txtEmployeeCode.Text = "";
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ������Ա�����Զ����������ƴ����
        /// </summary>
        private void CreateSpell()
        {
            if (this.txtSpell_Code.Text.Trim() == "" || this.txtWB_Code.Text.Trim() == "")
            {
                Neusoft.HISFC.Models.Base.Spell spell = new Neusoft.HISFC.Models.Base.Spell();
                spell = (Neusoft.HISFC.Models.Base.Spell)spellMgr.Get(this.txtEmployeeName.Text.Trim());
                this.txtSpell_Code.Text = spell.SpellCode;
                this.txtWB_Code.Text = spell.WBCode;
            }
        }
        private void txtEmployeeName_Leave(object sender, EventArgs e)
        {
            CreateSpell();//���������ƴ����
            
        }

        private void bttConfrim_Click(object sender, EventArgs e)
        {
            if (Save() == 0)
            this.FindForm().DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// ���淽��
        /// </summary>
        /// <returns></returns>
        public int Save()
        {   
            //��֤�ؼ����ݷ���Ҫ��
            if (ValueValidated())
            {
                Neusoft.HISFC.Models.Base.Employee empl = ConvertUcContextToObject();
                if (empl == null) return -1;
                //����ƴ����������
                CreateSpell();
                try
                {
                    Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                    Neusoft.HISFC.BizLogic.Manager.Person perMgr = new Neusoft.HISFC.BizLogic.Manager.Person();
                    //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(perMgr.Connection);
                    ////����ʼ
                    //trans.BeginTransaction();
                    ////��������
                    //perMgr.SetTrans(trans.Trans);
                    if (perMgr.Insert(empl) == -1)
                    {
                        if (perMgr.DBErrCode == 1)
                        {
                            if (perMgr.Update(empl) == -1 || perMgr.Update(empl) == 0)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("������Աʧ�ܣ�");
                                return -1;
                            }
                        }
                        else
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("������Աʧ�ܣ�");
                            return -1;
                        }
                    }

                    if (empl.EmployeeExt.DigitalSign != null) 
                    {
                        if (perMgr.InsertEmpExinfo(empl) == 1)
                        {
                            int val = perMgr.UpdateEmplDigitalSignByID(empl.ID, empl.EmployeeExt.DigitalSign);
                            if (val == -1 || val == 0)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("������Ա��չ��Ϣʧ�ܣ�");
                                return -1;
                            }
                        }
                        else if (perMgr.DBErrCode == 1) 
                        {
                            int val = perMgr.UpdateEmplDigitalSignByID(empl.ID, empl.EmployeeExt.DigitalSign);
                            if ( val == -1 || val == 0 )
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("������Ա��չ��Ϣʧ�ܣ�");
                                return -1;
                            }
                        }
                        else
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("������Ա��չ��Ϣʧ�ܣ�");
                            return -1;
                        }
                    }


                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    MessageBox.Show("����ɹ���");
                    tr = true;
                    this.txtEmployeeCode.Focus();
                    return 0;
                   
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);     
                    return -1;                                   
                }
              
            }
            else 
            {
                return -1;
            }
          
        }

        private void txtEmployeeCode_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            System.Windows.Forms.SendKeys.Send("{Tab}");
        }

        private void numSortId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //this.neuRadioButton1.Focus();
                this.txtUser_Code.Focus();
            }
        }

        private void neuRadioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.neuRadioButton3.Focus();
            }
        }

        private void neuRadioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.neuRadioButton5.Focus();
            }
        }

        private void neuRadioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.neuRadioButton3.Focus();
            }
        }

        private void neuRadioButton4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.neuRadioButton5.Focus();
            }
        }

        private void neuRadioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.radioValidate1.Focus();
            }
        }

        private void bttAdd_Click(object sender, EventArgs e)
        {
            if (Save() == 0)//�������ɹ�����տؼ����ݽ�������
                ClearUp();
        }

        private void txtUser_Code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.neuRadioButton1.Focus();
                
            }
        }

        /// <summary>
        /// [2007/08/16]ʧȥ����ʱ�ж�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtEmployeeCode_Leave(object sender, EventArgs e)
        {
            if (this.txtEmployeeCode.Text != "")
            {
                if (this.txtEmployeeCode.ReadOnly == false)
                {
                    this.txtEmployeeCode.Text = this.txtEmployeeCode.Text.PadLeft(6, '0');
                    Neusoft.HISFC.BizLogic.Manager.Person ps = new Neusoft.HISFC.BizLogic.Manager.Person();
                    int temp = ps.SelectEmployIsExist(this.txtEmployeeCode.Text);
                    if (temp == -1)
                    {
                        MessageBox.Show("��ѯ����,������ܻ��ظ�");
                    }
                    else if (temp == 1)
                    {
                        MessageBox.Show("�����Ѿ����ڣ�����������");
                        this.txtEmployeeCode.Focus();
                        this.txtEmployeeCode.Text = "";
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ���ݿ��е������Ա����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuButton1_Click(object sender, EventArgs e)
        {
            string  MaxEmplID = "";
            int NextEmplID;
            MaxEmplID = this.personMgr.GetMaxEmployeeID();
            if (MaxEmplID == "" || MaxEmplID == null)
            {
                MessageBox.Show("���ݿ���δ�洢��Ա��Ϣ��������������Ա����");
            }
            else
            {
                NextEmplID = Neusoft.FrameWork.Function.NConvert.ToInt32(MaxEmplID) + 1;
                if (NextEmplID == 1)
                {
                    MessageBox.Show("δ�ܳɹ��������ݿ��д洢����Ա��Ϣ��������������Ա����");
                }
                else
                {
                    this.txtEmployeeCode.Text = NextEmplID.ToString().PadLeft(6, '0');
                }
            }
            this.txtEmployeeCode.Focus();
        }

        private void radioValidate1_KeyDown(object sender, KeyEventArgs e)
        {
            this.rtxtMark.Focus();
        }

        #region donggq--20101118--{45E71A4E-803A-47fd-AC24-9BED6E530F16}--����ǩ��

        /// <summary>
        /// ѡ������ǩ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "bmp�ļ�|*.bmp"; //jpg�ļ�|*.jpg|gif�ļ�|*.gif|png�ļ�|*.png|
            open.ShowDialog();
            if (!string.IsNullOrEmpty(open.FileName))
            {
                if (System.IO.File.Exists(open.FileName))
                {
                    picBox.ImageLocation = open.FileName;
                }
                else
                {
                    MessageBox.Show("ѡ�����ļ������ڣ�������ѡ��");
                }
            }
            else
            {
                MessageBox.Show("��ѡ���ļ���");
            }
        }

        /// <summary>
        /// �������ǩ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDele_Click(object sender, EventArgs e)
        {
            this.picBox.Image = null;
            this.picBox.ImageLocation = null;
            this.employee.EmployeeExt.DigitalSign = null;

            Neusoft.HISFC.Models.Base.Employee empl = ConvertUcContextToObject();

            if (empl == null)
            {
                return;
            }

            try
            {
                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                Neusoft.HISFC.BizLogic.Manager.Person perMgr = new Neusoft.HISFC.BizLogic.Manager.Person();

                if (perMgr.DeleEmplDigitalSignByID(empl.ID) == -1)
                {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("ɾ��ʧ�ܣ�");
                        return;
                }

                Neusoft.FrameWork.Management.PublicTrans.Commit();
                MessageBox.Show("ɾ���ɹ���");
                tr = true;
                return;

            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                return;
            }

        }
        //{3BAA59AB-14DE-496e-B77A-E7C298D3245B}
        protected virtual int SetEnable()
        {
            
            foreach (Control item in this.neuGroupBox1.Controls)
            {
                if (item.Name == "comboLevel")
                {
                    continue;
                }
                if (item.Name == "rtxtMark")
                {
                    continue;
                }
                
                item.Enabled = false;
            }
            return 1;
        }
        #endregion


        
    }
}
