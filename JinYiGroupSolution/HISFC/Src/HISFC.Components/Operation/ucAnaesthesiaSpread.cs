using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Base;
using Neusoft.HISFC.Models.Operation;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: �����ű��ؼ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-11]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucAnaesthesiaSpread : UserControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucAnaesthesiaSpread()
        {
            InitializeComponent();
            if (!Environment.DesignMode)
            {
                this.Init();
            }
        }

        #region �ֶ�
        /// <summary>
        /// ������Ա�б�
        /// </summary>
        private Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup lbDoctor = new Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup();
        private Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup lbAnaetype = new Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup();

        private Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint arrangePrint;
        private DateTime date;

        public event ApplicationSelectedEventHandler applictionSelected;

        //{B9DDCC10-3380-4212-99E5-BB909643F11B}
        Neusoft.FrameWork.Public.ObjectHelper anneObjectHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        #endregion

        #region ����
        public DateTime Date
        {
            set
            {
                this.date = value;
            }
        }

        private string queryTime;
        public string QueryTime 
        {
            get 
            {
                return this.queryTime;
            }
            set 
            {
                this.queryTime = value;
                this.fpSpread1_Sheet1.ColumnHeader.Cells[1, 0].Text = this.queryTime;
                this.fpSpread1_Sheet1.ColumnHeader.Cells[1, 8].Text = string.Format("���ң�{0}", ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.Name);
            }
        }


        #endregion


        #region ����

        private void Init()
        {
            this.fpSpread1.SetInputMap();
            this.RefreshEmployeeList();
            this.InitTypeList();
            this.fpSpread1.AddListBoxPopup(this.lbDoctor, (int)Cols.anaeDoct);
            this.fpSpread1.AddListBoxPopup(this.lbDoctor, (int)Cols.anaeHelper);
            this.fpSpread1.AddListBoxPopup(this.lbAnaetype, (int)Cols.anaeType);

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
           System.Collections.ArrayList al= this.managerIntegrate.GetConstantList(EnumConstant.ANESWAY);
           this.anneObjectHelper.ArrayObject = al;
            
        }
        //ˢ�±�����Ա�б�
        private int RefreshEmployeeList()
        {
            //treeView2.Nodes.Clear();
            //TreeNode root = new TreeNode();
            //root.Text = "������Ա";
            //root.ImageIndex = 22;
            //root.SelectedImageIndex = 22;
            //treeView2.Nodes.Add(root);

            //��ȡ������Ա��
            //MessageBox.Show(EnumEmployeeType.D.ToString(),Environment.OperatorDeptID);
            //ArrayList persons = Environment.IntegrateManager.QueryEmployee(EnumEmployeeType.D, Environment.OperatorDeptID);

            //ArrayList persons = Environment.IntegrateManager.QueryEmployee(EnumEmployeeType.D, Environment.OperatorDeptID);

            #region donggq--2010.10.04--{C031EA11-9A8C-4c97-B9B3-026320DE2BD7}

            ArrayList persons = Environment.IntegrateManager.QueryEmployee(EnumEmployeeType.D, "2600");
            persons.AddRange(Environment.IntegrateManager.QueryEmployee(EnumEmployeeType.D, "2603")); 

            #endregion

            //ArrayList persons = Environment.IntegrateManager.QueryEmployeeAll();
            //��ӵ������б�
            //if (persons != null)
            //{
            //    foreach (Neusoft.HISFC.Models.Base.Employee person in persons)
            //    {
            //        TreeNode node = new TreeNode();
            //        node.Tag = person;
            //        node.Text = "[" + person.ID + "]" + person.Name;
            //        node.ImageIndex = 25;
            //        node.SelectedImageIndex = 25;
            //        root.Nodes.Add(node);
            //    }
            //}
            //root.Expand();
            //����ҽ��listbox�б�
            this.InitDoctListBox(persons);
            persons = null;

            return 0;
        }

        /// <summary>
        /// ���ҽ��listbox�б�
        /// </summary>
        /// <param name="doctors"></param>
        /// <returns></returns>
        private int InitDoctListBox(ArrayList doctors)
        {
            //ArrayList al = new ArrayList();
            //if (doctors != null)
            //{
            //    foreach (Neusoft.HISFC.Models.Base.Employee doctor in doctors)
            //    {
            //        Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
            //        obj = (Neusoft.FrameWork.Models.NeuObject)doctor;
            //        al.Add(obj);
            //    }
            //}
            lbDoctor.AddItems(doctors);

            //this.Controls.Add(this.lbDoctor);
            //this.lbDoctor.Visible = false;
            //lbDoctor.BorderStyle = BorderStyle.FixedSingle;
            //lbDoctor.BringToFront();
            //lbDoctor.ItemSelected += new EventHandler(lbDoctor_ItemSelected);
            return 0;
        }

        /// <summary>
        /// ������������б�
        /// </summary>
        /// <returns></returns>
        private int InitTypeList()
        {
            ArrayList types = Environment.IntegrateManager.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESTYPE);

            lbAnaetype.AddItems(types);

            //this.Controls.Add(this.lbAnaetype);
            //this.lbAnaetype.Visible = false;
            //this.lbAnaetype.BorderStyle = BorderStyle.FixedSingle;
            //this.lbAnaetype.BringToFront();
            //this.lbAnaetype.ItemSelected+=new EventHandler(lbAnaetype_ItemSelected);
            return 0;
        }
        /// <summary>
        /// ���û�ʿ������̨�б�λ��
        /// </summary>
        /// <returns></returns>
        //private int SetLocation()
        //{
        //    Control _cell = fpSpread1.EditingControl;
        //    if (_cell == null) return 0;

        //    //���֡�����
        //    if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeDoct ||
        //        fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeHelper)
        //    {
        //        lbDoctor.Location = new Point( _cell.Location.X,
        //            _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
        //        lbDoctor.Size = new Size(110, 150);
        //    }
        //    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeType)
        //    {
        //        lbAnaetype.Location = new Point(_cell.Location.X,
        //            _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
        //        lbAnaetype.Size = new Size(110, 150);
        //    }
        //    return 0;
        //}
        /// <summary>
        /// ѡ��ҽ��
        /// </summary>
        /// <param name="Column"></param>
        /// <returns></returns>
        //private int SelectDoctor(int Column)
        //{
        //    int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
        //    if (CurrentRow < 0) return 0;

        //    fpSpread1.StopCellEditing();
        //    NeuObject item = null;
        //    item = lbDoctor.GetSelectedItem();

        //    if (item == null) return -1;

        //    fpSpread1_Sheet1.Cells[CurrentRow, Column].Tag = item;
        //    fpSpread1_Sheet1.SetValue(CurrentRow, Column, item.Name, false);

        //    lbDoctor.Visible = false;

        //    return 0;
        //}

        /// <summary>
        /// ѡ����������
        /// </summary>
        /// <returns></returns>
        //private int SelectType()
        //{
        //    int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
        //    if (CurrentRow < 0) return 0;

        //    fpSpread1.StopCellEditing();
        //    NeuObject item = null;
        //    item = lbAnaetype.GetSelectedItem();

        //    if (item == null) return -1;

        //    fpSpread1_Sheet1.Cells[CurrentRow, (int)Cols.anaeType].Tag= item;
        //    fpSpread1_Sheet1.SetValue(CurrentRow, (int)Cols.anaeType, item.Name, false);

        //    lbAnaetype.Visible = false;

        //    return 0;
        //}
        /// <summary>
        /// �������������Ϣ
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public int AddOperationApplication(Neusoft.HISFC.Models.Operation.OperationAppllication apply)
        {
            this.fpSpread1_Sheet1.Rows.Add(fpSpread1_Sheet1.RowCount, 1);
            int row = fpSpread1_Sheet1.RowCount - 1;

            FarPoint.Win.Spread.CellType.TextCellType txtType = new FarPoint.Win.Spread.CellType.TextCellType();
            txtType.StringTrim = System.Drawing.StringTrimming.EllipsisWord;
            txtType.ReadOnly = true;
            fpSpread1_Sheet1.Rows[row].Tag = apply;
            //��ʿվ            
            Department dept = Environment.IntegrateManager.GetDepartment(apply.PatientInfo.PVisit.PatientLocation.Dept.ID);
            if (dept != null)
            {
                fpSpread1_Sheet1.SetValue(row, (int)Cols.nurseID, dept.Name, false);
            }
            //����
            fpSpread1_Sheet1.SetValue(row, (int)Cols.bedID, apply.PatientInfo.PVisit.PatientLocation.Bed.ID, false);
            //��������
            fpSpread1_Sheet1.SetValue(row, (int)Cols.Name, apply.PatientInfo.Name, false);
            //�Ƿ��Ѱ���
            if (apply.IsAnesth)
                fpSpread1_Sheet1.Cells[row, (int)Cols.Name].Note = "�Ѱ���";
            else
                fpSpread1_Sheet1.Cells[row, (int)Cols.Name].Note = "";
            //�Ա�
            fpSpread1_Sheet1.SetValue(row, (int)Cols.Sex, apply.PatientInfo.Sex.Name, false);
            //����
            //int interval = DateTime.Now.Year - apply.PatientInfo.Birthday.Year;
            string age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(apply.PatientInfo.Birthday);//interval + "��";
            //if (interval == 0)
            //{
            //    interval = DateTime.Now.Month - apply.PatientInfo.Birthday.Month;
            //    age = interval + "��";
            //}
            //if (interval == 0)
            //{
            //    interval = DateTime.Now.Day - apply.PatientInfo.Birthday.Day;
            //    age = interval + "��";
            //}
            fpSpread1_Sheet1.SetValue(row, (int)Cols.age, age, false);
            //��ǰ���
            if (apply.DiagnoseAl != null && apply.DiagnoseAl.Count > 0)
            {
                // TODO: �����ǰ���

                fpSpread1_Sheet1.SetValue(row, (int)Cols.Diagnose, (apply.DiagnoseAl[0] as Neusoft.HISFC.Models.HealthRecord.DiagnoseBase).ICD10.Name, false);
            }
            else
                fpSpread1_Sheet1.SetValue(row, (int)Cols.Diagnose, "", false);
            //����������
            string opName = "";
            if (apply.OperationInfos != null && apply.OperationInfos.Count > 0)
            {
                foreach (OperationInfo item in apply.OperationInfos)
                {
                    if (item.IsMainFlag)
                    {
                        opName = item.OperationItem.Name;
                        break;
                    }
                }
                if (opName == "")
                    opName = (apply.OperationInfos[0] as OperationInfo).OperationItem.Name;
            }
            fpSpread1_Sheet1.SetValue(row, (int)Cols.opItemName, opName, false);

            //����ҽ��
            fpSpread1_Sheet1.SetValue(row, (int)Cols.opDoc, apply.OperationDoctor.Name, false);
            //�ϲ�����
            fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeNote, apply.AneNote, false);


            //����ʽ
            if (apply.AnesType.ID != null && apply.AnesType.ID != "")
            {
                NeuObject obj = Environment.GetAnes(apply.AnesType.ID.ToString());

                if (obj != null)
                {
                    fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeType, obj.Name, false);
                    fpSpread1_Sheet1.SetTag(row, (int)Cols.anaeType, obj);
                }
            }



            //�Ƿ���
            if (apply.OperateKind == "2")
            {
                fpSpread1_Sheet1.RowHeader.Cells[row, 0].BackColor = Color.Red;
                fpSpread1_Sheet1.RowHeader.Cells[row, 0].Text = "��";
            }
            fpSpread1_Sheet1.Cells[row, 0, row, 8].CellType = txtType;

            if (apply.RoleAl != null && apply.RoleAl.Count != 0)
            {
                foreach (ArrangeRole role in apply.RoleAl)
                {
                    if (role.RoleType.ID.ToString() == EnumOperationRole.Anaesthetist.ToString())//����
                    {
                        string name = role.Name;
                        if (role.RoleOperKind.ID != null)
                        {
                            //ֱ��
                            if (role.RoleOperKind.ID.ToString() == EnumRoleOperKind.ZL.ToString())
                                name = name + "|��";
                            //�Ӱ�
                            else if (role.RoleOperKind.ID.ToString() == EnumRoleOperKind.JB.ToString())
                                name = name + "|��";
                        }
                        fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeDoct, name, false);
                        NeuObject obj = (NeuObject)role;
                        obj.Memo = role.RoleOperKind.ID.ToString();
                        fpSpread1_Sheet1.Cells[row, (int)Cols.anaeDoct].Tag = obj;
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.AnaesthesiaHelper.ToString())//����
                    {
                        string name = role.Name;
                        if (role.RoleOperKind.ID != null)
                        {
                            //ֱ��
                            if (role.RoleOperKind.ID.ToString() == EnumRoleOperKind.ZL.ToString())
                                name = name + "|��";
                            //�Ӱ�
                            else if (role.RoleOperKind.ID.ToString() == EnumRoleOperKind.JB.ToString())
                                name = name + "|��";
                        }
                        fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeHelper, name, false);
                        NeuObject obj = (NeuObject)role;
                        obj.Memo = role.RoleOperKind.ID.ToString();
                        fpSpread1_Sheet1.Cells[row, (int)Cols.anaeHelper].Tag = obj;
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.AnaeTmpHelper1.ToString()) 
                    {
                        fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeTmpHelper1, role.Name, false);
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.AnaeTmpHelper2.ToString())
                    {
                        fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeTmpHelper2, role.Name, false);

                    }
                }
            }
            //����̨
            if (apply.OpsTable != null)
            {
                fpSpread1_Sheet1.SetValue(row, (int)Cols.TableID, apply.OpsTable.Name, false);
                NeuObject obj = new NeuObject();
                obj.ID = apply.OpsTable.ID;
                obj.Name = apply.OpsTable.Name;
                fpSpread1_Sheet1.Cells[row, (int)Cols.TableID].Tag = obj;
            }

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            fpSpread1_Sheet1.Cells[row, (int)Cols.anaeWay].CellType = txtType;
            fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeWay, this.anneObjectHelper.GetName(apply.AnesWay));
            fpSpread1_Sheet1.Cells[row, (int)Cols.TableID].CellType = txtType;

            return 0;
        }

        /// <summary>
        /// ���
        /// </summary>
        public void Reset()
        {
            this.fpSpread1_Sheet1.RowCount = 0;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Save()
        {

            //���ݿ�����
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            //Neusoft.FrameWork.Management.Transaction trans = new 
            //    Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            //trans.BeginTransaction();

            List<int> succeed = new List<int>();        //�ɹ����ŵ�

            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {
                //����ʽ
                NeuObject type = fpSpread1_Sheet1.GetTag(i, (int)Cols.anaeType) as NeuObject;
                //û��¼������ʽ��������
                if (type == null || type.ID.Length == 0)
                    continue;
                //����
                NeuObject anaeDoct = fpSpread1_Sheet1.GetTag(i, (int)Cols.anaeDoct) as NeuObject;
                //û�����飬������
                if (anaeDoct == null || anaeDoct.ID.Length == 0)
                    continue;

                NeuObject tt = fpSpread1_Sheet1.GetTag(i, (int)Cols.anaeHelper) as NeuObject;
                if (tt != null && tt.ID != "" && anaeDoct.ID == tt.ID)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("��������ֲ�����ͬһ����");
                    return -1;
                }
                //��������ʵ��
                //OperationAppllication apply = fpSpread1_Sheet1.Rows[i].Tag as OperationAppllication;
                //{3DC153BD-1E9B-40c4-AAFC-3C27607A8945}
                OperationAppllication applyOriginal = fpSpread1_Sheet1.Rows[i].Tag as OperationAppllication;
                if (applyOriginal == null)
                {
                    MessageBox.Show("ʵ��ת������");
                    return -1;
                }
                OperationAppllication apply = Environment.OperationManager.GetOpsApp(applyOriginal.ID);
                if (apply == null)
                {
                    MessageBox.Show("��ȡ������Ϣ����");
                    return -1;
                }
                if (apply.ID == "")
                {
                    continue;
                }

                //trans.BeginTransaction();
                Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                try
                {
                    //�������ʽ������ʵ��
                    apply.AnesType = type;
                    //������顢����
                    this.AddRole(apply, i);
                    //��־Ϊ�Ѱ�������
                    //apply.bAnesth=true;

                    if (Environment.OperationManager.UpdateApplication(apply) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("��������(" + apply.ID + ")������Ϣʧ�ܣ�\n����ϵͳ����Ա��ϵ��" + Environment.OperationManager.Err, "��ʾ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return -1;
                    }
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    succeed.Add(i);
                }
                catch (Exception e)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("��������(" + apply.ID + ")������Ϣ����!" + e.Message, "��ʾ");
                    return -1;
                }
                //���½�����ʾ
                fpSpread1_Sheet1.Rows[i].Tag = apply;
                //fpSpread1_Sheet1.Cells[i,(int)Cols.Name].Note="�Ѱ���";

                //////////////////////////////////////////////////////////////////////////                
                // Robin��Ϊ�����������û����
                //this.UpdateFlag(apply);
                //////////////////////////////////////////////////////////////////////////


            }

            if (succeed.Count > 0)
            {
                string line = string.Empty;
                int temp = 0;
                for (int i = 0; i < succeed.Count; i++)
                {
                    line += i.ToString() + ",";
                }
                line = line.Substring(0, line.Length - 1);
                //temp = Neusoft.FrameWork.Function.NConvert.ToInt32 (line) + 1;
                //MessageBox.Show(string.Format("��{0}���������ųɹ���", temp.ToString()), "��ʾ");
                MessageBox.Show("�����ųɹ�", "��ʾ");
                fpSpread1.Focus();
            }
            else
            {
                MessageBox.Show("û�пɰ��ŵ���������!", "��ʾ");
                fpSpread1.Focus();
            }

            return 0;
        }

        /// <summary>
        /// ������顢����
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private int AddRole(OperationAppllication apply, int row)
        {
            ArrayList roles = new ArrayList();
            //��������顢����
            for (int i = 0; i < apply.RoleAl.Count; i++)
            {
                ArrangeRole role = apply.RoleAl[i] as ArrangeRole;
                if (
                    role.RoleType.ID.ToString() != EnumOperationRole.Anaesthetist.ToString()      &&
                    role.RoleType.ID.ToString() != EnumOperationRole.AnaesthesiaHelper.ToString() &&
                    role.RoleType.ID.ToString() != EnumOperationRole.AnaeTmpHelper1.ToString()    &&
                    role.RoleType.ID.ToString() != EnumOperationRole.AnaeTmpHelper2.ToString()
                    )
                {
                    roles.Add(role.Clone());
                }
            }

            //�������
            NeuObject obj = fpSpread1_Sheet1.GetTag(row, (int)Cols.anaeDoct) as NeuObject;
            if (obj != null)
            {
                ArrangeRole role = new ArrangeRole(obj);
                role.RoleType.ID = EnumOperationRole.Anaesthetist;//��ɫ����
                if (obj.Memo == "ZL")
                    role.RoleOperKind.ID = EnumRoleOperKind.ZL;
                else if (obj.Memo == "JB")
                    role.RoleOperKind.ID = EnumRoleOperKind.JB;

                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����	
            }
            //�������
            obj = fpSpread1_Sheet1.GetTag(row, (int)Cols.anaeHelper) as NeuObject;
            if (obj != null)
            {
                ArrangeRole role = new ArrangeRole(obj);

                role.RoleType.ID = EnumOperationRole.AnaesthesiaHelper;//��ɫ����
                if (obj.Memo == "ZL")
                    role.RoleOperKind.ID = EnumRoleOperKind.ZL;
                else if (obj.Memo == "JB")
                    role.RoleOperKind.ID = EnumRoleOperKind.JB;

                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����	
            }

            //�����ʱ��������1
            string tmpHelper1 = fpSpread1_Sheet1.GetText(row, (int)Cols.anaeTmpHelper1);
            if (tmpHelper1 != null && tmpHelper1 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.AnaeTmpHelper1;//��ɫ����
                role.Name = tmpHelper1;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            }

            //�����ʱ��������2
            string tmpHelper2 = fpSpread1_Sheet1.GetText(row, (int)Cols.anaeTmpHelper2);
            if (tmpHelper2 != null && tmpHelper2 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.AnaeTmpHelper2;//��ɫ����
                role.Name = tmpHelper2;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            }


            apply.RoleAl = roles;

            return 0;
        }
        //����ʵ��İ��ű�־
        //private int UpdateFlag(OpsApplication apply)
        //{
        //    for (int index = 0; index < alApplys.Count; index++)
        //    {
        //        neusoft.HISFC.Object.Operator.OpsApplication obj = alApplys[index] as neusoft.HISFC.Object.Operator.OpsApplication;
        //        if (obj.OperationNo == apply.OperationNo)
        //        {
        //            alApplys.Remove(obj);
        //            alApplys.Insert(index, apply);
        //            break;
        //        }
        //    }
        //    return 0;
        //}
        /// <summary>
        /// ��������
        /// </summary>
        private OperationAppllication UpdateData(int rowIndex)
        {
            //����ʽ
            NeuObject type = fpSpread1_Sheet1.GetTag(rowIndex, (int)Cols.anaeType) as NeuObject;
            //û��¼������ʽ��������
            //if (type == null || type.ID.Length == 0)
            //return null;
            //����
            //NeuObject anaeDoct = fpSpread1_Sheet1.GetTag(rowIndex, (int)Cols.anaeDoct) as NeuObject;
            //û�����飬������
            //if (anaeDoct == null || anaeDoct.ID.Length == 0)
            //return null;

            //NeuObject tt = fpSpread1_Sheet1.GetTag(rowIndex, (int)Cols.anaeHelper) as NeuObject;
            //if (tt != null && tt.ID != "" && anaeDoct.ID == tt.ID)
            //{
            //    MessageBox.Show("��������ֲ�����ͬһ����");
            //    return null;
            //}
            //��������ʵ��
            OperationAppllication apply = fpSpread1_Sheet1.Rows[rowIndex].Tag as OperationAppllication;


            try
            {
                //�������ʽ������ʵ��
                apply.AnesType = type;
                //������顢����
                this.AddRole(apply, rowIndex);
            }
            catch
            {
                return null;
            }
            //���½�����ʾ
            fpSpread1_Sheet1.Rows[rowIndex].Tag = apply;

            return apply;
        }
        public int Print()
        {
            //if (this.arrangePrint == null)
            //{
            //    //this.arrangePrint = new ucArrangePrint();
            //    this.arrangePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint)) as Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint;
            //    if (this.arrangePrint == null)
            //    {
            //        MessageBox.Show("��ýӿ�IArrangePrint��������ϵͳ����Ա��ϵ��");

            //        return -1;
            //    }
            //}

            //this.arrangePrint.Title = "������һ����";
            //this.arrangePrint.Date = this.date;
            //this.arrangePrint.ArrangeType = Neusoft.HISFC.BizProcess.Interface.Operation.EnumArrangeType.Anaesthesia;
            //this.arrangePrint.Reset();
            //for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
            //{
            //    this.arrangePrint.AddAppliction(this.UpdateData(i));
            //}
            //return this.arrangePrint.PrintPreview();
            if (this.fpSpread1_Sheet1.RowCount > 0)
            {
                try
                {
                    this.fpSpread1_Sheet1.PrintInfo.ShowBorder = false;
                    this.fpSpread1_Sheet1.PrintInfo.Preview = true;

                    this.fpSpread1_Sheet1.PrintInfo.UseMax = false;
                    //this.fpSpread1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                    
                    this.fpSpread1.PrintSheet(this.fpSpread1.ActiveSheetIndex);
                    //MessageBox.Show("��ӡ�ɹ�");
                    return 1;
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("û�п��Դ�ӡ�����ݣ�");
                return -1;
            }
        }

        public int Export()
        {
            if (this.fpSpread1_Sheet1.RowCount > 0)
            {
                if (this.fpSpread1.Export() == 1)
                {
                    MessageBox.Show("�����ɹ�");
                    return 1;
                }

                return -1;
            }
            else 
            {
                MessageBox.Show("û�п��Ե��������ݣ�");
                return -1;
            }
        }
        #endregion

        #region columns
        private enum Cols
        {
            nurseID,
            bedID,
            Name,
            Sex,
            age,
            Diagnose,
            opItemName,
            opDoc,
            anaeNote,
            //��������
            anaeWay,
            /// <summary>
            /// ����ʽ
            /// </summary>
            anaeType,
            /// <summary>
            /// ����
            /// </summary>
            anaeDoct,
            /// <summary>
            /// ����
            /// </summary>
            anaeHelper,
            /// <summary>
            /// ����̨
            /// </summary>
            TableID,
            /// <summary>
            /// ������ʱ����1
            /// </summary>
            anaeTmpHelper1,
            /// <summary>
            /// ������ʱ����2
            /// </summary>
            anaeTmpHelper2
        }
        #endregion

        #region �¼�
        private void fpSpread1_EditModeOn(object sender, EventArgs e)
        {
            //fpSpread1.EditingControl.KeyDown += new KeyEventHandler(EditingControl_KeyDown);
            //this.SetLocation();
            //if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeType)
            //    this.lbAnaetype.Visible = true;
            //else
            //    lbAnaetype.Visible = false;

            //if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeDoct ||
            //    fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.anaeHelper)
            //    this.lbDoctor.Visible = true;
            //else
            //    lbDoctor.Visible = false;
        }


        private void fpSpread1_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //string _Text;
            ////����ʽ
            //if (e.Column == (int)Cols.anaeType)
            //{
            //    _Text = fpSpread1_Sheet1.ActiveCell.Text;
            //    lbAnaetype.Filter(_Text);

            //    if (lbAnaetype.Visible == false) lbAnaetype.Visible = true;
            //    fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            //}
            ////���֡�����
            //else if (e.Column == (int)Cols.anaeDoct || e.Column == (int)Cols.anaeHelper)
            //{
            //    //if (IsChange) return;
            //    _Text = fpSpread1_Sheet1.ActiveCell.Text;
            //    lbDoctor.Filter(_Text);

            //    if (lbDoctor.Visible == false) 
            //        lbDoctor.Visible = true;
            //    fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            //}
        }
        #endregion

        private void fpSpread1_EditModeOff(object sender, EventArgs e)
        {

        }

        private void fpSpread1_EnterCell(object sender, FarPoint.Win.Spread.EnterCellEventArgs e)
        {
            if (this.applictionSelected != null)
            {
                this.applictionSelected(this, this.fpSpread1_Sheet1.Rows[e.Row].Tag as OperationAppllication);
            }
        }

        //private void lbDoctor_ItemSelected(object sender, System.EventArgs e)
        //{
        //    this.SelectDoctor(fpSpread1_Sheet1.ActiveColumnIndex);

        //}

        //private void lbAnaetype_ItemSelected(object sender, System.EventArgs e)
        //{
        //    this.SelectType();
        //}

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get { return new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint) }; }
        }

        #endregion
    }
}
