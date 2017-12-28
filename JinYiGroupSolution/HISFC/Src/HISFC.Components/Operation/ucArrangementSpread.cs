using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using Neusoft.HISFC.Models.Operation;
using Neusoft.FrameWork.Models;
using Neusoft.FrameWork;

namespace Neusoft.HISFC.Components.Operation
{
    public delegate void ApplicationSelectedEventHandler(object sender, OperationAppllication e);
    /// <summary>
    /// [��������: ��������������Ϣ]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-04]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucArrangementSpread : UserControl, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucArrangementSpread()
        {
            InitializeComponent();
            if (!Environment.DesignMode)
            {
                this.Init();
                this.InitNurseListBox();
                this.InitRoomListBox();
                //this.InitTableListBox();
            }
        }

        #region �ֶ�

        public event ApplicationSelectedEventHandler applictionSelected;
        Neusoft.HISFC.BizProcess.Integrate.Manager deptManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        Neusoft.FrameWork.Management.DataBaseManger dataManager = new Neusoft.FrameWork.Management.DataBaseManger();

        /// <summary>
        /// �����б�
        /// </summary>
        private ArrayList alApplys = new ArrayList();
        private ArrayList alRooms = new ArrayList();
        //private ArrayList alAnes;   //����ʽ�б�

        private Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup lbNurse = new Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup();
        private Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup lbRoom = new Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup();
        private Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup lbTable = new Neusoft.FrameWork.WinForms.Controls.NeuListBoxPopup();

        private Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint arrangePrint;
        private DateTime date;

        private EnumFilter filter = EnumFilter.All;
        private int rowindex;

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

        public EnumFilter Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
                if (value == EnumFilter.All)
                {
                    for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
                    {
                        this.fpSpread1_Sheet1.Rows[i].Visible = true;
                    }
                }
                else if (value == EnumFilter.NotYet)
                {
                    for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
                    {
                        if ((this.fpSpread1_Sheet1.Rows[i].Tag as OperationAppllication).ExecStatus != "3")
                            this.fpSpread1_Sheet1.Rows[i].Visible = true;
                        else
                            this.fpSpread1_Sheet1.Rows[i].Visible = false;
                    }
                }
                else
                {
                    for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
                    {
                        if ((this.fpSpread1_Sheet1.Rows[i].Tag as OperationAppllication).ExecStatus == "3")
                            this.fpSpread1_Sheet1.Rows[i].Visible = true;
                        else
                            this.fpSpread1_Sheet1.Rows[i].Visible = false;
                    }
                }
            }
        }
        public enum EnumFilter
        {
            /// <summary>
            /// ȫ��
            /// </summary>
            All,
            /// <summary>
            /// δ����
            /// </summary>
            NotYet,
            /// <summary>
            /// �Ѱ���
            /// </summary>
            Already
        }


        #endregion

        #region ����

        private void Init()
        {

            this.fpSpread1.SetInputMap();
            //this.fpSpread1.AddListBoxPopup(lbNurse, 10);
            //this.fpSpread1.AddListBoxPopup(lbNurse, 11);
            //this.fpSpread1.AddListBoxPopup(this.lbRoom, 12);
            //this.fpSpread1.AddListBoxPopup(lbTable, 13);
            //this.fpSpread1.AddListBoxPopup(lbNurse, 15);
            //this.fpSpread1.AddListBoxPopup(lbNurse, 16);

            this.fpSpread1.AddListBoxPopup(lbNurse, (int)Cols.WNR);
            this.fpSpread1.AddListBoxPopup(lbNurse, (int)Cols.INR);
            this.fpSpread1.AddListBoxPopup(this.lbRoom, (int)Cols.RoomID);
            //this.fpSpread1.AddListBoxPopup(lbTable, (int)Cols.TableID);
            this.fpSpread1.AddListBoxPopup(lbNurse, (int)Cols.WNR2);
            this.fpSpread1.AddListBoxPopup(lbNurse, (int)Cols.INR2);
            Neusoft.FrameWork.WinForms.Classes.MarkCellType.DateTimeCellType dtCellType = new Neusoft.FrameWork.WinForms.Classes.MarkCellType.DateTimeCellType();
            dtCellType.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.TimeOnly;
            this.fpSpread1_Sheet1.Columns[(int)Cols.opDate].CellType = dtCellType;

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            System.Collections.ArrayList al = this.managerIntegrate.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.ANESWAY);
            this.anneObjectHelper.ArrayObject = al;
        }
        /// <summary>
        /// ��ӻ�ʿlistbox�б�
        /// </summary>
        /// <returns></returns>
        private int InitNurseListBox()
        {
            ArrayList nurses = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.N, Environment.OperatorDeptID);

            //ArrayList al = new ArrayList();
            //if (nurses != null)
            //{
            //    foreach (neusoft.HISFC.Object.RADT.Person nurse in nurses)
            //    {
            //        NeuObject obj = (NeuObject)nurse;
            //        al.Add(obj);
            //    }
            //}
            lbNurse.AddItems(nurses);

            this.Controls.Add(lbNurse);
            this.lbNurse.Hide();
            this.lbNurse.BorderStyle = BorderStyle.FixedSingle;
            this.lbNurse.BringToFront();
            this.lbNurse.ItemSelected += new System.EventHandler(this.lbNurse_ItemSelected);

            return 0;
        }

        private void InitRoomListBox()
        {
            this.RefreshRoomListBox();

            this.Controls.Add(this.lbRoom);
            this.lbRoom.Hide();
            this.lbRoom.BorderStyle = BorderStyle.FixedSingle;
            this.lbRoom.BringToFront();
            this.lbRoom.ItemSelected += new System.EventHandler(this.lbRoom_ItemSelected);

        }
        //�����������б�
        private void RefreshRoomListBox()
        {
            ArrayList al = new ArrayList();

            ArrayList rooms = Environment.TableManager.GetRoomsByDept(Environment.OperatorDeptID);
            if (rooms != null)
            {
                foreach (Neusoft.HISFC.Models.Operation.OpsRoom room in rooms)
                {
                    alRooms.Add(room);
                    if (room.IsValid == false)
                        continue;
                    //��Ϊtable��û��ʵ��ISpell�ӿڣ����Խ���department��
                    Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
                    //dept.ID = room.ID;
                    //dept.Name = room.Name;
                    //dept.SpellCode = room.InputCode;
                    dept.ID = room.Name;
                    dept.Name = room.Name;
                    dept.Memo = room.ID;
                    al.Add(dept);
                }
            }
            lbRoom.AddItems(al);
        }

        //private void InitTableListBox()
        //{


        //    this.Controls.Add(this.lbTable);
        //    this.lbTable.Hide();
        //    this.lbTable.BorderStyle = BorderStyle.FixedSingle;
        //    this.lbTable.BringToFront();
        //    this.lbTable.ItemSelected += new System.EventHandler(this.lbTable_ItemSelected);

        //}
        //�������̨listbox�б�
        //private int RefreshTableListBox(string roomID)
        //{
        //    ArrayList al = Environment.TableManager.GetOpsTable(roomID);
        //    ArrayList tables = new ArrayList();
        //    if (al != null)
        //    {
        //        foreach (OpsTable table in al)
        //        {
        //            if (table.IsValid == false) continue;
        //            //��Ϊtable��û��ʵ��ISpell�ӿڣ����Խ���department��
        //            Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
        //            dept.ID = table.ID;
        //            dept.Name = table.Name;
        //            dept.SpellCode = table.InputCode;
        //            tables.Add(dept);
        //        }
        //    }

        //    lbTable.AddItems(tables);

        //    return 0;
        //}
        /// <summary>
        ///  ѡ��ʿ
        /// </summary>
        /// <param name="Column"></param>
        /// <returns></returns>
        private int SelectNurse(int Column)
        {
            int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
            if (CurrentRow < 0) return 0;

            fpSpread1.StopCellEditing();
            NeuObject item = this.lbNurse.GetSelectedItem();

            if (item == null)
                return -1;

            fpSpread1_Sheet1.Cells[CurrentRow, Column].Tag = item;
            fpSpread1_Sheet1.SetValue(CurrentRow, Column, item.Name, false);

            lbNurse.Visible = false;

            return 0;
        }

        //ѡ�񷿺�
        private int SelectRoom()
        {
            int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
            if (CurrentRow < 0) return 0;

            fpSpread1.StopCellEditing();
            NeuObject item = null;
            item = lbRoom.GetSelectedItem();

            if (item == null) return -1;

            NeuObject tmp = new NeuObject(item.Memo, item.Name, "");

            fpSpread1_Sheet1.Cells[CurrentRow, (int)Cols.RoomID].Tag = tmp;
            fpSpread1_Sheet1.SetValue(CurrentRow, (int)Cols.RoomID, tmp.Name, false);

            //this.refreshTableListBox(item.ID);

            lbRoom.Visible = false;

            return 0;
        }

        //ѡ������̨
        //private int SelectTable()
        //{
        //    int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;

        //    if (CurrentRow < 0) return 0;

        //    fpSpread1.StopCellEditing();
        //    NeuObject item = null;
        //    item = lbTable.GetSelectedItem();

        //    if (item == null) return -1;

        //    fpSpread1_Sheet1.Cells[CurrentRow, (int)Cols.TableID].Tag = item;
        //    fpSpread1_Sheet1.SetValue(CurrentRow, (int)Cols.TableID, item.Name, false);

        //    lbTable.Visible = false;

        //    NeuObject tab = null;
        //    fpSpread1_Sheet1.Cells[CurrentRow, (int)Cols.TableID].Tag = tab;
        //    fpSpread1_Sheet1.SetValue(CurrentRow, (int)Cols.TableID, tab.Name, false);


        //    ArrayList al = Environment.TableManager.GetOpsTable(roomID);
        //    ArrayList tables = new ArrayList();
        //    if (al != null)
        //    {
        //        foreach (OpsTable table in al)
        //        {
        //            if (table.IsValid == false) continue;
        //            //��Ϊtable��û��ʵ��ISpell�ӿڣ����Խ���department��
        //            Neusoft.HISFC.Models.Base.Department dept = new Neusoft.HISFC.Models.Base.Department();
        //            dept.ID = table.ID;
        //            dept.Name = table.Name;
        //            dept.SpellCode = table.InputCode;
        //            tables.Add(dept);
        //        }
        //    }

        //    lbTable.AddItems(tables);

        //    return 0;
        //}


        /// <summary>
        /// ���û�ʿ������̨�б�λ��
        /// </summary>
        /// <returns></returns>
        private int SetLocation()
        {

            Control _cell = fpSpread1.EditingControl;
            if (_cell == null) return 0;

            //ϴ�֡�Ѳ�ػ�ʿ
            int Column = fpSpread1_Sheet1.ActiveColumnIndex;
            if (Column == (int)Cols.WNR || Column == (int)Cols.INR || Column == (int)Cols.WNR2)
            {
                lbNurse.Location = new Point(_cell.Location.X,
                    _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
                lbNurse.Size = new Size(110, 150);
            }
            else if (Column == (int)Cols.INR2)
            {
                lbNurse.Location = new Point(_cell.Location.X + _cell.Width - 110,
                     _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
                lbNurse.Size = new Size(110, 150);
            }
            else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.TableID)
            {
                lbTable.Location = new Point(_cell.Location.X,
                    _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
                lbTable.Size = new Size(110, 150);
            }
            else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.RoomID)
            {
                lbRoom.Location = new Point(_cell.Location.X,
                    _cell.Location.Y + _cell.Height + SystemInformation.Border3DSize.Height * 2);
                lbRoom.Size = new Size(110, 150);
            }

            return 0;
        }
        /// <summary>
        /// �������������Ϣ
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        public int AddOperationApplication(Neusoft.HISFC.Models.Operation.OperationAppllication apply)
        {
            //���붯̬����
            this.alApplys.Add(apply);

            fpSpread1_Sheet1.Rows.Add(fpSpread1_Sheet1.RowCount, 1);
            int row = fpSpread1_Sheet1.RowCount - 1;

            FarPoint.Win.Spread.CellType.TextCellType txtType = new FarPoint.Win.Spread.CellType.TextCellType();
            txtType.StringTrim = System.Drawing.StringTrimming.EllipsisWord;
            txtType.ReadOnly = true;
            fpSpread1_Sheet1.Rows[row].Tag = apply;
            //��ʿվ
            if (deptManager == null)
                deptManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();

            Neusoft.HISFC.Models.Base.Department dept = deptManager.GetDepartment(apply.PatientInfo.PVisit.PatientLocation.Dept.ID);
            apply.PatientInfo.PVisit.PatientLocation.Name = dept.Name;
            if (dept != null)
            {
                fpSpread1_Sheet1.SetValue(row, (int)Cols.nurseID, dept.Name, false);
            }
            //��������
            fpSpread1_Sheet1.SetValue(row, (int)Cols.Name, apply.PatientInfo.Name, false);
            //�Ա�
            fpSpread1_Sheet1.SetValue(row, (int)Cols.Sex, apply.PatientInfo.Sex.Name, false);
            //����
            //int age = this.dataManager.GetDateTimeFromSysDateTime().Year - apply.PatientInfo.Birthday.Year;
            //if (age == 0) age = 1;
            string age = Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(apply.PatientInfo.Birthday);
            fpSpread1_Sheet1.SetValue(row, (int)Cols.Age, age, false);
            //�Ƿ��Ѱ���
            if (apply.ExecStatus == "3")
                fpSpread1_Sheet1.Cells[row, (int)Cols.Name].Note = "�Ѱ���";
            else
                fpSpread1_Sheet1.Cells[row, (int)Cols.Name].Note = "";
            //̨��
            //fpSpread1_Sheet1.SetValue(row, (int)Cols.Order, apply.BloodUnit, false);
            //����̨����
            //switch (apply.TableType)
            //{
            //    case "1":
            //        fpSpread1_Sheet1.SetValue(row, (int)Cols.Desk, "��̨", false);
            //        break;
            //    case "2":
            //        fpSpread1_Sheet1.SetValue(row, (int)Cols.Desk, "��̨", false);
            //        break;
            //    case "3":
            //        fpSpread1_Sheet1.SetValue(row, (int)Cols.Desk, "��̨", false);
            //        break;
            //}

            //����������            
            fpSpread1_Sheet1.SetValue(row, (int)Cols.opItemName, apply.MainOperationName, false);

            Neusoft.FrameWork.Models.NeuObject obj = null;
            //����ʽ
            if (apply.AnesType.ID != null && apply.AnesType.ID.Length != 0)
            {
                obj = Environment.GetAnes(apply.AnesType.ID);
                if (obj != null)
                {
                    fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeType, obj.Name, false);
                    apply.AnesType.Name = obj.Name;
                }
            }
            else
            {
                fpSpread1_Sheet1.Cells[row, (int)Cols.anaeType].Note = "δ��������";
            }

            //����ҽ��
            fpSpread1_Sheet1.SetValue(row, (int)Cols.opDoctID, apply.OperationDoctor.Name, false);
            //�Ƿ���
            if (apply.OperateKind == "2")
            {
                fpSpread1_Sheet1.RowHeader.Cells[row, 0].BackColor = Color.Red;
                fpSpread1_Sheet1.RowHeader.Cells[row, 0].Text = "��";
            }
            fpSpread1_Sheet1.Cells[row, 0, row, 7].CellType = txtType;

            //����ʱ��
            fpSpread1_Sheet1.SetValue(row, (int)Cols.opDate, apply.PreDate, false);
            #region ��ʿ
            if (apply.RoleAl != null && apply.RoleAl.Count != 0)
            {
                foreach (Neusoft.HISFC.Models.Operation.ArrangeRole role in apply.RoleAl)
                {
                    if (role.RoleType.ID.ToString() == EnumOperationRole.WashingHandNurse.ToString()) //ϴ�ֻ�ʿ                        
                    {
                        if (fpSpread1_Sheet1.Cells[row, (int)Cols.WNR].Text == "") //��һϴ�ֻ�ʿ
                        {
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.WNR, role.Name, false);
                            obj = (Neusoft.FrameWork.Models.NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.WNR, obj);
                        }
                        else
                        {   //�ڶ�ϴ�ֻ�ʿ
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.WNR2, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.WNR2, obj);
                        }
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.ItinerantNurse.ToString())//Ѳ�ػ�ʿ
                    {
                        if (fpSpread1_Sheet1.Cells[row, (int)Cols.INR].Text == "")
                        {   //��һѲ�ػ�ʿ 
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.INR, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.INR, obj);
                        }
                        else
                        {
                            //�ڶ�Ѳ�ػ�ʿ
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.INR2, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.INR2, obj);
                        }

                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper1.ToString())
                    {
                        if (string.IsNullOrEmpty(fpSpread1_Sheet1.Cells[row, (int)Cols.TmpHelper1].Text))
                        {
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.TmpHelper1, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.TmpHelper1, obj);
                        }
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpHelper2.ToString())
                    {
                        if (string.IsNullOrEmpty(fpSpread1_Sheet1.Cells[row, (int)Cols.TmpHelper2].Text))
                        {
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.TmpHelper2, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.TmpHelper2, obj);
                        }
                    }
                    #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpStudent1.ToString())
                    {
                        if (string.IsNullOrEmpty(fpSpread1_Sheet1.Cells[row, (int)Cols.TmpStudent1].Text))
                        {
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.TmpStudent1, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.TmpStudent1, obj);
                        }
                    }
                    else if (role.RoleType.ID.ToString() == EnumOperationRole.TmpStudent2.ToString())
                    {
                        if (string.IsNullOrEmpty(fpSpread1_Sheet1.Cells[row, (int)Cols.TmpStudent2].Text))
                        {
                            fpSpread1_Sheet1.SetValue(row, (int)Cols.TmpStudent2, role.Name, false);
                            obj = (NeuObject)role;
                            fpSpread1_Sheet1.SetTag(row, (int)Cols.TmpStudent2, obj);
                        }
                    } 
                    #endregion
                }
            }
            #endregion
            //������
            if (apply.RoomID != null && apply.RoomID != "")
            {
                obj = GetRoom(apply.RoomID);
                fpSpread1_Sheet1.SetValue(row, (int)Cols.RoomID, obj.Name, false);
                fpSpread1_Sheet1.SetTag(row, (int)Cols.RoomID, obj);
            }
            #region ����̨
            if (apply.OpsTable.ID != null && apply.OpsTable.ID != "")
            {
                fpSpread1_Sheet1.SetValue(row, (int)Cols.TableID, apply.OpsTable.Name, false);
                obj = new NeuObject();
                obj.ID = apply.OpsTable.ID;
                obj.Name = apply.OpsTable.Name;
                fpSpread1_Sheet1.SetTag(row, (int)Cols.TableID, obj);
            }

            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            fpSpread1_Sheet1.Cells[row, (int)Cols.anaeWay].CellType = txtType;
            fpSpread1_Sheet1.SetValue(row, (int)Cols.anaeWay, this.anneObjectHelper.GetName(apply.AnesWay));
            fpSpread1_Sheet1.Cells[row, (int)Cols.TableID].CellType = txtType;

            #endregion

            return 0;
        }

        /// <summary>
        /// ���
        /// </summary>
        public void Reset()
        {
            this.fpSpread1_Sheet1.RowCount = 0;
            this.alApplys.Clear();
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        private NeuObject GetRoom(string roomID)
        {
            NeuObject obj = new NeuObject();
            foreach (OpsRoom room in alRooms)
            {
                if (roomID == room.ID)
                {
                    obj.ID = room.ID;
                    obj.Name = room.Name;
                    return obj;
                }
            }
            obj.ID = roomID;
            obj.Name = "��";
            return obj;
        }

        public int SetStop() 
        {
            OperationAppllication apply = fpSpread1_Sheet1.Rows[fpSpread1_Sheet1.ActiveRowIndex].Tag as OperationAppllication;

            if (apply != null)
            {
                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
                Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                try
                {

                    if (Environment.OperationManager.SetOpsStop(apply.ID) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("������(" + apply.ID + ")ͣ��Ϣʧ�ܣ�\n����ϵͳ����Ա��ϵ��" + Environment.OperationManager.Err, "��ʾ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return -1;
                    }
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                    

                }
                catch (Exception e)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("������(" + apply.ID + ")ͣ��Ϣ����!" + e.Message, "��ʾ");
                    return -1;
                }
            }
            else 
            {
                MessageBox.Show("û��������Ϣ");
                return -1;
            }

            return 1;
        }



        /// <summary>
        /// У������ 
        /// </summary>
        /// <returns></returns>
        private int ValueState()
        {
            for (int row = 0; row < this.fpSpread1_Sheet1.RowCount; row++)
            {
                NeuObject obj = fpSpread1_Sheet1.GetTag(row, (int)Cols.WNR) as NeuObject;
                NeuObject obj2 = fpSpread1_Sheet1.GetTag(row, (int)Cols.WNR2) as NeuObject;
                if (obj != null && obj2 != null)
                {
                    if (obj.ID == obj2.ID)
                    {
                        MessageBox.Show("ϴ�ֻ�ʿ�����ظ�");
                        return -1;
                    }
                }
                NeuObject obj3 = fpSpread1_Sheet1.GetTag(row, (int)Cols.INR) as NeuObject;
                NeuObject obj4 = fpSpread1_Sheet1.GetTag(row, (int)Cols.INR2) as NeuObject;
                if (obj3 != null && obj4 != null)
                {
                    if (obj3.ID == obj4.ID)
                    {
                        MessageBox.Show("Ѳ�ػ�ʿ�����ظ�");
                        return -1;
                    }
                }
                NeuObject tabRoom = fpSpread1_Sheet1.GetTag(row, (int)Cols.RoomID) as NeuObject;
                NeuObject tabTable = null; //= fpSpread1_Sheet1.GetTag(row, (int)Cols.TableID) as NeuObject;

                //if (tabRoom == null)
                //{
                //    MessageBox.Show("�������䲻��Ϊ��");
                //    return -1;
                //}

                

                #region {42CDE890-24B3-4d6f-A52B-988F62E226B8}
                if (obj != null || obj2 != null || obj3 != null || obj4 != null || tabRoom != null || tabTable != null)
                {

                    //û��¼������̨��������
                    if (tabRoom == null)
                    {
                        MessageBox.Show("��ѡ����������");
                        return -1;
                    }

                    ArrayList al = Environment.TableManager.GetOpsTable(tabRoom.ID);
                    if (al != null && al.Count > 0)
                    {
                        tabTable = al[0] as OpsTable;
                    }


                    //û��¼������̨��������
                    if (tabTable == null)
                    {
                        MessageBox.Show("��ѡ������̨��");
                        return -1;
                    }
                }
                #endregion
            }
            return 0;
        }

        /// <summary>
        /// ��Ӹ��໤ʿ
        /// </summary>
        /// <param name="apply"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private int AddRole(OperationAppllication apply, int row)
        {
            ArrayList roles = new ArrayList();
            //����ջ�ʿ
            for (int i = 0; i < apply.RoleAl.Count; i++)
            {
                ArrangeRole role = apply.RoleAl[i] as ArrangeRole;
                #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
                if (
                            role.RoleType.ID.ToString() != EnumOperationRole.WashingHandNurse.ToString()
                             && role.RoleType.ID.ToString() != EnumOperationRole.ItinerantNurse.ToString()
                             && role.RoleType.ID.ToString() != EnumOperationRole.TmpHelper1.ToString()
                             && role.RoleType.ID.ToString() != EnumOperationRole.TmpHelper2.ToString()
                             && role.RoleType.ID.ToString() != EnumOperationRole.TmpStudent1.ToString()
                             && role.RoleType.ID.ToString() != EnumOperationRole.TmpStudent2.ToString()
                            )
                {
                    roles.Add(role.Clone());
                } 
                #endregion
            }

            //���ϴ�ֻ�ʿ
            NeuObject obj = fpSpread1_Sheet1.GetTag(row, (int)Cols.WNR) as NeuObject;
            if (obj != null)
            {
                ArrangeRole role = new ArrangeRole(obj);
                role.RoleType.ID = EnumOperationRole.WashingHandNurse;//��ɫ����
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����	
                //�ڶ�ϴ�ֻ�ʿ  ���Բ���
                if (fpSpread1_Sheet1.Cells[row, (int)Cols.WNR2].Tag != null)
                {
                    NeuObject obj2 = fpSpread1_Sheet1.GetTag(row, (int)Cols.WNR2) as NeuObject;
                    if (obj2 != null)
                    {
                        ArrangeRole role2 = new ArrangeRole(obj2);
                        role2.RoleType.ID = EnumOperationRole.WashingHandNurse;//��ɫ����
                        role2.OperationNo = apply.ID;
                        role2.ForeFlag = "0";//��ǰ����				
                        roles.Add(role2);//������Ա��ɫ����	
                    }
                }
            }
            //���Ѳ�ػ�ʿ
            NeuObject obj3 = fpSpread1_Sheet1.GetTag(row, (int)Cols.INR) as NeuObject;
            if (obj3 != null)
            {
                ArrangeRole role = new ArrangeRole(obj3);
                role.RoleType.ID = EnumOperationRole.ItinerantNurse;//��ɫ����
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
                if (fpSpread1_Sheet1.Cells[row, (int)Cols.INR2].Tag != null)
                {
                    //��ӵڶ�Ѳ�ػ�ʿ
                    NeuObject obj4 = fpSpread1_Sheet1.GetTag(row, (int)Cols.INR2) as NeuObject;
                    if (obj4 != null)
                    {
                        ArrangeRole role2 = new ArrangeRole(obj4);
                        role2.RoleType.ID = role.RoleType.ID = EnumOperationRole.ItinerantNurse; ;//��ɫ����
                        role2.OperationNo = apply.ID;
                        role2.ForeFlag = "0";//��ǰ����				
                        roles.Add(role2);//������Ա��ɫ����
                    }
                }
            }

            //�����ʱ����1
            string tmpHelper1 = fpSpread1_Sheet1.GetText(row, (int)Cols.TmpHelper1);
            if (tmpHelper1 != null && tmpHelper1 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.TmpHelper1;//��ɫ����
                role.Name = tmpHelper1;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            }

            //�����ʱ����2
            string tmpHelper2 = fpSpread1_Sheet1.GetText(row, (int)Cols.TmpHelper2);
            if (tmpHelper2 != null && tmpHelper2 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.TmpHelper2;//��ɫ����
                role.Name = tmpHelper2;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            }

            #region {3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
            //�����ʱ����1
            string tmpStudent1 = fpSpread1_Sheet1.GetText(row, (int)Cols.TmpStudent1);
            if (tmpStudent1 != null && tmpStudent1 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.TmpStudent1;//��ɫ����
                role.Name = tmpStudent1;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            }

            //�����ʱ����2
            string tmpStudent2 = fpSpread1_Sheet1.GetText(row, (int)Cols.TmpStudent2);
            if (tmpStudent2 != null && tmpStudent2 != "")
            {
                ArrangeRole role = new ArrangeRole();
                role.ID = "777777";
                role.RoleType.ID = EnumOperationRole.TmpStudent2;//��ɫ����
                role.Name = tmpStudent2;
                role.OperationNo = apply.ID;
                role.ForeFlag = "0";//��ǰ����				
                roles.Add(role);//������Ա��ɫ����
            } 
            #endregion



            apply.RoleAl = roles;

            return 0;
        }

        /// <summary>
        /// ����ʵ��İ��ű�־
        /// </summary>
        /// <param name="apply"></param>
        /// <returns></returns>
        private int UpdateFlag(OperationAppllication apply)
        {
            for (int index = 0; index < alApplys.Count; index++)
            {
                OperationAppllication obj = alApplys[index] as OperationAppllication;
                if (obj.ID == apply.ID)
                {
                    alApplys.Remove(obj);
                    alApplys.Insert(index, apply);
                    break;
                }
            }
            return 0;
        }

        /// <summary>
        /// ��������
        /// </summary>
        private OperationAppllication UpdateData(int rowIndex)
        {
            NeuObject tabRoom = fpSpread1_Sheet1.GetTag(rowIndex, (int)Cols.RoomID) as NeuObject;
            //û��¼������̨��������
            if (tabRoom == null)
                return null;

            OperationAppllication apply = fpSpread1_Sheet1.Rows[rowIndex].Tag as OperationAppllication;

            try
            {
                OpsTable tabTable = null; //= fpSpread1_Sheet1.GetTag(row, (int)Cols.TableID) as NeuObject;

                ArrayList al = Environment.TableManager.GetOpsTable(tabRoom.ID);
                if (al != null && al.Count > 0)
                {
                    tabTable = al[0] as OpsTable;
                }

                //�������̨

                apply.OpsTable = tabTable;
                //�����������
                NeuObject tab = fpSpread1_Sheet1.GetTag(rowIndex, (int)Cols.RoomID) as NeuObject;
                if (tab != null)
                    apply.RoomID = tab.ID;
                else
                    fpSpread1_Sheet1.SetValue(rowIndex, (int)Cols.RoomID, "", false);

                //����ʱ��
                string dt = fpSpread1_Sheet1.GetText(rowIndex, (int)Cols.opDate);
                dt = apply.PreDate.Year.ToString() + "-" + apply.PreDate.Month.ToString() + "-" + apply.PreDate.Day.ToString() + " " + dt;
                apply.PreDate = DateTime.Parse(dt);
                //��Ӹ��໤ʿ
                this.AddRole(apply, rowIndex);

            }
            catch (Exception e)
            {

                return null;
            }

            return apply;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            if (this.ValueState() == -1)
            {
                return -1;

            }

            //���ݿ�����
            //Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);


            List<int> succeed = new List<int>();        //�ɹ����ŵ�
            for (int i = 0; i < fpSpread1_Sheet1.RowCount; i++)
            {

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


                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                //trans.BeginTransaction();
                Environment.OperationManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                try
                {

                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    //�����������
                    NeuObject room = fpSpread1_Sheet1.GetTag(i, (int)Cols.RoomID) as NeuObject;
                    if (room != null)
                        apply.RoomID = room.ID;
                    else
                        fpSpread1_Sheet1.SetValue(i, (int)Cols.RoomID, "", false);

                    //�������̨
                    OpsTable tab = null;
                    if (room != null && !string.IsNullOrEmpty(room.ID))
                    {
                        ArrayList al = Environment.TableManager.GetOpsTable(room.ID);
                        if (al != null && al.Count > 0)
                        {
                            tab = al[0] as OpsTable;
                            apply.OpsTable = tab; //�������̨
                        }
                        else //û��¼������̨��������
                        {
                            fpSpread1_Sheet1.Cells[i, (int)Cols.TableID].Text = "";
                            continue;
                        }
                    }
                    else 
                    {
                            fpSpread1_Sheet1.Cells[i, (int)Cols.TableID].Text = "";
                            continue;
                    }

                    //����ʱ��
                    string dt = fpSpread1_Sheet1.GetText(i, (int)Cols.opDate);
                    dt = apply.PreDate.Year.ToString() + "-" + apply.PreDate.Month.ToString() + "-" + apply.PreDate.Day.ToString() + " " + dt;
                    apply.PreDate = DateTime.Parse(dt);
                    //��Ӹ��໤ʿ
                    this.AddRole(apply, i);
                    //��־Ϊ�Ѱ���
                    apply.ExecStatus = "3";

                    if (Environment.OperationManager.UpdateApplication(apply) == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("��������(" + apply.ID + ")������Ϣʧ�ܣ�\n����ϵͳ����Ա��ϵ��" + Environment.OperationManager.Err, "��ʾ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return -1;
                    }
                    succeed.Add(i);
                    Neusoft.FrameWork.Management.PublicTrans.Commit();
                }
                catch (Exception e)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                    MessageBox.Show("��������(" + apply.ID + ")������Ϣ����!" + e.Message, "��ʾ");
                    return -1;
                }
                //���½�����ʾ
                fpSpread1_Sheet1.Rows[i].Tag = apply;
                fpSpread1_Sheet1.Cells[i, (int)Cols.Name].Note = "�Ѱ���";
                this.UpdateFlag(apply);

            }

            if (succeed.Count > 0)
            {
                string line = string.Empty;
                for (int i = 0; i < succeed.Count; i++)
                {
                    line += i.ToString() + ",";
                }
                line = line.Substring(0, line.Length - 1);
                MessageBox.Show(string.Format("�������ųɹ���", line), "��ʾ");
                fpSpread1.Focus();
                if (lbTable != null)
                {
                    lbTable.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("û�����밲����Ϣ�����豣��");
            }

            return 0;
        }

        public int Print()
        {
            if (this.arrangePrint == null)
            {
                this.arrangePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint)) as Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint;
                if (this.arrangePrint == null)
                {
                    MessageBox.Show("��ýӿ�IArrangePrint��������ϵͳ����Ա��ϵ��");

                    return -1;
                }
            }

            this.arrangePrint.Title = "��������һ����";
            this.arrangePrint.Date = this.date;
            this.arrangePrint.ArrangeType = Neusoft.HISFC.BizProcess.Interface.Operation.EnumArrangeType.Operation;
            this.arrangePrint.Reset();
            for (int i = 0; i < this.fpSpread1_Sheet1.RowCount; i++)
            {
                this.arrangePrint.AddAppliction(this.UpdateData(i));
            }
            return this.arrangePrint.PrintPreview();

        }

        /// <summary>
        /// ����
        /// </summary>
        public void SetFilter()
        {
            this.Filter = this.filter;
        }
        #endregion


        #region �¼�
        protected override bool ProcessDialogKey(Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                #region enter
                if (fpSpread1.ContainsFocus)
                {
                    //ϴ��
                    if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.WNR)
                    {
                        if (lbNurse.Visible)
                            SelectNurse((int)Cols.WNR);

                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.INR, false);
                    }
                    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.WNR2) //ϴ�ֻ�ʿ2 
                    {
                        if (lbNurse.Visible)
                            SelectNurse((int)Cols.WNR2);
                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.INR2, false);
                    }
                    //Ѳ��
                    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.INR)
                    {
                        if (lbNurse.Visible)
                            SelectNurse((int)Cols.INR);
                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.RoomID, false);
                    }
                    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.INR2) //Ѳ�ػ�ʿ2 
                    {
                        if (lbNurse.Visible)
                            SelectNurse((int)Cols.INR2);
                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.RoomID, false);
                    }
                    //����
                    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.RoomID)
                    {
                        //if (lbRoom.Visible) 
                            SelectRoom();
                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.TableID, false);
                    }
                    //����̨
                    //else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.TableID)
                    //{
                    //    if (lbTable.Visible) SelectTable();
                    //    if (fpSpread1_Sheet1.RowCount != fpSpread1_Sheet1.ActiveRowIndex + 1)
                    //    {
                    //        fpSpread1_Sheet1.ActiveRowIndex++;
                    //        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.opDate, true);
                    //        FarPoint.Win.Spread.LeaveCellEventArgs e = new FarPoint.Win.Spread.LeaveCellEventArgs
                    //            (new FarPoint.Win.Spread.SpreadView(fpSpread1), 0, 0,
                    //            fpSpread1_Sheet1.ActiveRowIndex, fpSpread1_Sheet1.ActiveColumnIndex);
                    //        fpSpread1_LeaveCell(fpSpread1, e);
                    //    }
                    //}
                    else if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.opDate)
                    {
                        fpSpread1_Sheet1.SetActiveCell(fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.WNR, false);
                    }
                }
                #endregion
            }

            else if (keyData == Keys.Up)
            {

                //#region Up
                //if (fpSpread1.ContainsFocus)
                //{                  

                //    if (lbNurse.Visible)
                //    {                                       
                //        lbNurse.PriorRow();
                //    }
                //    else if (lbTable.Visible)
                //    {
                //        lbTable.PriorRow();

                //    }
                //    else if (lbRoom.Visible)
                //    {
                //        lbRoom.PriorRow();
                //    }
                //    else
                //    {
                //        int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
                //        if (CurrentRow > 0)
                //        {
                //            fpSpread1_Sheet1.ActiveRowIndex = CurrentRow - 1;
                //            fpSpread1_Sheet1.AddSelection(CurrentRow - 1, 0, 1, 0);
                //            FarPoint.Win.Spread.LeaveCellEventArgs e = new FarPoint.Win.Spread.LeaveCellEventArgs
                //                (new FarPoint.Win.Spread.SpreadView(fpSpread1), 0, 0, CurrentRow - 1, fpSpread1_Sheet1.ActiveColumnIndex);
                //            fpSpread1_LeaveCell(fpSpread1, e);
                //        }
                //        //fpSpread1_Sheet1.ActiveRowIndex++;
                //    }
                //    return true;
                //}
                //#endregion
            }
            else if (keyData == Keys.Down)
            {
                //#region Down
                //if (fpSpread1.ContainsFocus)
                //{
                //    if (lbNurse.Visible)
                //    {
                //        lbNurse.NextRow();

                //    }
                //    else if (lbTable.Visible)
                //    {
                //        lbTable.NextRow();
                //    }
                //    else if (lbRoom.Visible)
                //    {
                //        lbRoom.NextRow();
                //    }
                //    else
                //    {
                //        int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
                //        if (CurrentRow < fpSpread1_Sheet1.RowCount - 1)
                //        {
                //            fpSpread1_Sheet1.ActiveRowIndex = CurrentRow + 1;
                //            fpSpread1_Sheet1.AddSelection(CurrentRow + 1, 0, 1, 0);
                //            FarPoint.Win.Spread.LeaveCellEventArgs e = new FarPoint.Win.Spread.LeaveCellEventArgs
                //                (new FarPoint.Win.Spread.SpreadView(fpSpread1), 0, 0, CurrentRow + 1, fpSpread1_Sheet1.ActiveColumnIndex);
                //            fpSpread1_LeaveCell(fpSpread1, e);
                //        }
                //        fpSpread1_Sheet1.ActiveRowIndex--;

                //        //int CurrentRow = fpSpread1_Sheet1.ActiveRowIndex;
                //        //if (CurrentRow >= 0 && CurrentRow <= fpSpread1_Sheet1.RowCount - 2)
                //        //{
                //        //    fpSpread1_Sheet1.ActiveRowIndex = CurrentRow + 1;
                //        //    fpSpread1_Sheet1.AddSelection(CurrentRow + 1, 0, 1, 0);
                //        //}
                //    }
                //    return true;
                //}
                //#endregion
            }
            else if (keyData == Keys.Escape)
            {
                if (lbNurse.Visible)
                    lbNurse.Visible = false;
                if (lbTable.Visible)
                    lbTable.Visible = false;
                if (lbRoom.Visible)
                    lbRoom.Visible = false;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void fpSpread1_EditModeOn(object sender, System.EventArgs e)
        {
            fpSpread1.EditingControl.KeyDown += new KeyEventHandler(EditingControl_KeyDown);
            SetLocation();

            //if (fpSpread1_Sheet1.ActiveColumnIndex == (int)Cols.TableID)
            //{
            //    string roomid = "";

            //    if (this.fpSpread1_Sheet1.GetTag(this.fpSpread1_Sheet1.ActiveRowIndex, (int)Cols.RoomID)
            //        != null)
            //    {
            //        roomid = (this.fpSpread1_Sheet1.GetTag(this.fpSpread1_Sheet1.ActiveRowIndex,
            //            (int)Cols.RoomID) as NeuObject).ID;
            //    }
            //    else
            //    {
            //        roomid = "no_room";
            //    }
            //    this.RefreshTableListBox(roomid);
            //}
            try
            {
                if (fpSpread1_Sheet1.ActiveColumnIndex != (int)Cols.TableID)
                {
                    lbTable.Visible = false;
                }
                else
                {
                    lbTable.Visible = true;
                    lbTable.Filter(fpSpread1_Sheet1.ActiveCell.Text);
                }
                int ColumnIndex = fpSpread1_Sheet1.ActiveColumnIndex;
                if (ColumnIndex != (int)Cols.WNR && ColumnIndex != (int)Cols.INR && ColumnIndex != (int)Cols.WNR2 && ColumnIndex != (int)Cols.INR2)
                {
                    lbNurse.Visible = false;
                }
                else
                {
                    lbNurse.Visible = true;
                    lbNurse.Filter(fpSpread1_Sheet1.ActiveCell.Text);
                }

                if (fpSpread1_Sheet1.ActiveColumnIndex != (int)Cols.RoomID)
                {
                    lbRoom.Visible = false;
                }
                else
                {
                    lbRoom.Visible = true;
                    lbRoom.Filter(fpSpread1_Sheet1.ActiveCell.Text);

                }
            }
            catch { }
        }

        private void fpSpread1_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

            string _Text;
            if (e.Column == (int)Cols.TableID)
            {
                _Text = fpSpread1_Sheet1.ActiveCell.Text;
                lbTable.Filter(_Text);

                if (lbTable.Visible == false) lbTable.Visible = true;
                fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            }
            //ϴ�֡�ѭ����ʿ
            else if (e.Column == (int)Cols.INR || e.Column == (int)Cols.INR2 || e.Column == (int)Cols.WNR2 ||
                e.Column == (int)Cols.WNR)
            {
                _Text = fpSpread1_Sheet1.ActiveCell.Text;
                lbNurse.Filter(_Text);

                if (lbNurse.Visible == false)
                    lbNurse.Visible = true;
                fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            }
            else if (e.Column == (int)Cols.RoomID)
            {
                _Text = fpSpread1_Sheet1.ActiveCell.Text;
                lbRoom.Filter(_Text);

                if (lbRoom.Visible == false) lbRoom.Visible = true;
                fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            }
            //else if (e.Column == (int)Cols.TmpHelper1 || e.Column == (int)Cols.TmpHelper2)
            //{

            //    fpSpread1_Sheet1.SetTag(e.Row, e.Column, null);
            //}
        }

        private void fpSpread1_EditModeOff(object sender, System.EventArgs e)
        {
            int col = this.fpSpread1_Sheet1.ActiveColumnIndex;
            if (col == (int)Cols.WNR || col == (int)Cols.INR || col == (int)Cols.WNR2 || col == (int)Cols.INR2)
            {
                //TODO: ˢ�»�ʿ�б�
                //this.RefreshNurseList(this.alTabulars);
            }
        }
        //���Ҽ�ʵ�ֹ����cell����ת
        private void EditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Left)
            //{
            //    FarPoint.Win.Spread.CellType.GeneralEditor t = fpSpread1.EditingControl as FarPoint.Win.Spread.CellType.GeneralEditor;
            //    if (t.SelectionStart == 0 && t.SelectionLength == 0)
            //    {
            //        int row = 0, column = 0;
            //        if (fpSpread1_Sheet1.ActiveColumnIndex == 0 && fpSpread1_Sheet1.ActiveRowIndex != 0)
            //        {
            //            row = fpSpread1_Sheet1.ActiveRowIndex - 1;
            //            column = fpSpread1_Sheet1.Columns.Count - 1;
            //        }
            //        else if (fpSpread1_Sheet1.ActiveColumnIndex != 0)
            //        {
            //            row = fpSpread1_Sheet1.ActiveRowIndex;
            //            column = fpSpread1_Sheet1.ActiveColumnIndex - 1;
            //        }
            //        fpSpread1_Sheet1.SetActiveCell(row, column, true);
            //    }
            //}
            //if (e.KeyCode == Keys.Right)
            //{
            //    FarPoint.Win.Spread.CellType.GeneralEditor t = fpSpread1.EditingControl as FarPoint.Win.Spread.CellType.GeneralEditor;

            //    if (t.Text == null || t.Text == "" || t.SelectionStart == t.Text.Length)
            //    {
            //        int row = fpSpread1_Sheet1.RowCount - 1, column = fpSpread1_Sheet1.ColumnCount - 1;
            //        if (fpSpread1_Sheet1.ActiveColumnIndex == column && fpSpread1_Sheet1.ActiveRowIndex != row)
            //        {
            //            row = fpSpread1_Sheet1.ActiveRowIndex + 1;
            //            column = 0;
            //        }
            //        else if (fpSpread1_Sheet1.ActiveColumnIndex != column)
            //        {
            //            row = fpSpread1_Sheet1.ActiveRowIndex;
            //            column = fpSpread1_Sheet1.ActiveColumnIndex + 1;
            //        }
            //        fpSpread1_Sheet1.SetActiveCell(row, column, true);
            //    }
            //}
        }

        private void lbNurse_ItemSelected(object sender, System.EventArgs e)
        {
            this.SelectNurse(fpSpread1_Sheet1.ActiveColumnIndex);
        }
        private void lbTable_ItemSelected(object sender, System.EventArgs e)
        {
            //SelectTable();

        }

        private void lbRoom_ItemSelected(object sender, System.EventArgs e)
        {
            SelectRoom();

        }
        #endregion

        #region columns--{3D5AAF4F-8EA3-46b7-8E5C-FFA6EBA20527}
        private enum Cols
        {
            nurseID,
            Name,
            Sex,
            Age,
            /// <summary>
            /// ̨��
            /// </summary>
            //Order,
            /// <summary>
            /// �Ƿ���̨
            /// </summary>
            //Desk,
            opItemName,
            //{B9DDCC10-3380-4212-99E5-BB909643F11B}
            /// <summary>
            /// ����ʽ
            /// </summary>
            anaeWay,
            anaeType,
            opDoctID,
            opDate,
            /// <summary>
            /// ����
            /// </summary>
            RoomID,
            /// <summary>
            /// Ѳ�ػ�ʿ
            /// </summary>
            INR,
            /// <summary>
            /// ϴ�ֻ�ʿ
            /// </summary>
            WNR,
            /// <summary>
            /// ����1
            /// </summary>
            TmpStudent1,
            
            /// <summary>
            /// Ѳ�ػ�ʿ2 
            /// </summary>
            INR2,
            /// <summary>
            /// ϴ�ֻ�ʿ2
            /// </summary>
            WNR2,
            /// <summary>
            /// ����2
            /// </summary>
            TmpStudent2,
            /// <summary>
            /// ����̨
            /// </summary>
            TableID,
            /// <summary>
            /// ��ʱ1
            /// </summary>
            TmpHelper1,
            /// <summary>
            /// ��ʱ2
            /// </summary>
            TmpHelper2
             
        }

        #endregion

        private void fpSpread1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            //if (this.applictionSelected != null)
            //{
            //    this.applictionSelected(this, this.fpSpread1_Sheet1.Rows[e.Row].Tag as OperationAppllication);
            //}
        }


        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get { return new Type[] { typeof(Neusoft.HISFC.BizProcess.Interface.Operation.IArrangePrint) }; }
        }

        #endregion



        private void fpSpread1_CellClick(object sender, CellClickEventArgs e)
        {
            if (this.applictionSelected != null)
            {
                this.applictionSelected(this, this.fpSpread1_Sheet1.Rows[e.Row].Tag as OperationAppllication);
            }
        }

        /// <summary>
        ///����������
        /// </summary>
        /// <returns></returns>
        public int ChangeDept()
        {

            try
            {
                int row = fpSpread1_Sheet1.ActiveRowIndex;
                if (fpSpread1_Sheet1.RowCount == 0) return -1;

                //neusoft.HISFC.Object.Operator.OpsApplication apply = new neusoft.HISFC.Object.Operator.OpsApplication();
                Neusoft.HISFC.Models.Operation.OperationAppllication apply = new OperationAppllication();
                apply = fpSpread1_Sheet1.Rows[row].Tag as Neusoft.HISFC.Models.Operation.OperationAppllication;
                string strOldOpsRoom = apply.OperateRoom.ID;//ִ�п���

                frmChangeOpsRoom dlg = new frmChangeOpsRoom(apply);
                //dlg.m_objOpsApp = apply;
                dlg.InitWin();
                DialogResult result = dlg.ShowDialog();
                //���ڵ��ˡ�ȷ������ť
                if (result == DialogResult.OK)
                {
                    //����ֵ��ͬ����������������
                    if (dlg.strNewOpsRoomID != strOldOpsRoom)
                    {
                        //ˢ�´��ڵĿؼ��б�(ͨ�����Ĳ�ѯ����ʱ���ֵ��������¼��ﵽ���Ŀ��)
                        //�������������ҵ����뵥���б�����ʧ��ʾ��
                        //RefreshApplys();
                        dlg.Dispose();
                    }
                }
                else
                {
                    return -1;
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }




    }




}
