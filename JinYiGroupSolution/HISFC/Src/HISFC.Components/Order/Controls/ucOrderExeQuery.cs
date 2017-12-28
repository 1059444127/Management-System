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
    public partial class ucOrderExeQuery : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucOrderExeQuery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ���߹�����
        /// </summary>
        //private Neusoft.HISFC.BizLogic.RADT.InPatient patientManager = new Neusoft.HISFC.BizLogic.RADT.InPatient();

        /// <summary>
        /// �������ݱ���ȡ����
        /// </summary>
        private Neusoft.FrameWork.Public.ObjectHelper objHelper = new Neusoft.FrameWork.Public.ObjectHelper();

        /// <summary>
        /// ҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Order.Order myOrder = new Neusoft.HISFC.BizLogic.Order.Order();

        /// <summary>
        /// ҵ���
        /// </summary>
        private Neusoft.HISFC.BizLogic.Order.ChargeBill myCharegeBill = new Neusoft.HISFC.BizLogic.Order.ChargeBill();

        /// <summary>
        /// ҽ��ִ�е�����
        /// </summary>
        private Neusoft.HISFC.Models.Order.ExecOrder myExeOrder = null;

        private ArrayList alExeOrder = null;

        //���ݻ�����Ϣ��
        private Neusoft.HISFC.Models.RADT.PatientInfo patient;
        /// <summary>
        /// ҳ�����ԣ����մ������Ļ�����Ϣ
        /// </summary>
        public Neusoft.HISFC.Models.RADT.PatientInfo Patient
        {
            get
            {
                return this.patient;
            }
            set
            {
                this.patient = value;
                if (this.patient != null)
                {
                    this.ShowData(this.patient.ID);
                }
            }

        }

        private ArrayList myDeptList = null;
        private ArrayList myOrderTypeList = null;

        DataSet myDataSetDrug = new DataSet();
        DataSet myDataSetUndrug = new DataSet();

        DataView myDataViewDrug = new DataView();//ҩƷ����
        DataView myDataViewUndrug = new DataView();//��ҩƷ����

        string filterInput = "1=1";	//�������������
        string filterExec  = "1=1";
        string filterValid = "1=1";	//�Ƿ���Ч��������
        string filterType = "1=1";//ҽ������

        string drugQuery   = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @"\ucOrderExeQuery_Drug.xml";
        string undrugQuery = Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + @"\ucOrderExeQuery_UnDrug.xml";

        /// <summary>
        /// ���ݲ���ʵ��
        /// </summary>
        /// <param name="neuObject"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            this.Patient = neuObject as Neusoft.HISFC.Models.RADT.PatientInfo;
            return base.OnSetValue(neuObject, e);
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnQuery(object sender, object neuObject)
        {
            if (this.patient != null)
            {
                this.ShowData(this.patient.ID);
            }

            return base.OnQuery(sender, neuObject);
        }

        /// <summary>
        /// ����ҩƷִ�е���ʾ��ʽ
        /// </summary>
        protected void SetFormatForDrug()
        {
            if (System.IO.File.Exists(this.drugQuery))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.neuSpread1_Sheet1, this.drugQuery);
                this.RefreshDrugBackColor();
            }
            else
            {
                #region ȱʡ��
                this.neuSpread1_Sheet1.Columns.Get(0).Label = "���";
                this.neuSpread1_Sheet1.Columns.Get(0).Width = 56F;
                FarPoint.Win.Spread.CellType.CheckBoxCellType cellcbkBJ = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                this.neuSpread1_Sheet1.Columns[0].CellType = cellcbkBJ;
                this.neuSpread1_Sheet1.Columns[0].Locked = false;
                this.neuSpread1_Sheet1.Columns.Get(1).Label = "����ҽ��";
                this.neuSpread1_Sheet1.Columns.Get(1).Width = 56F;
                this.neuSpread1_Sheet1.Columns.Get(2).Label = "ҽ������";
                this.neuSpread1_Sheet1.Columns.Get(2).Width = 56F;
                this.neuSpread1_Sheet1.Columns.Get(3).Label = "��Ч";
                this.neuSpread1_Sheet1.Columns.Get(3).Width = 35F;
                this.neuSpread1_Sheet1.Columns.Get(4).Label = "����״̬";
                this.neuSpread1_Sheet1.Columns.Get(4).Width = 59F;
                this.neuSpread1_Sheet1.Columns.Get(5).Label = "ҩƷ����";
                this.neuSpread1_Sheet1.Columns.Get(5).Width = 117F;
                this.neuSpread1_Sheet1.Columns.Get(6).Label = "���";
                this.neuSpread1_Sheet1.Columns.Get(6).Width = 71F;
                this.neuSpread1_Sheet1.Columns.Get(7).Label = "����";
                this.neuSpread1_Sheet1.Columns.Get(7).Width = 60F;
                this.neuSpread1_Sheet1.Columns.Get(8).Label = "��λ";
                this.neuSpread1_Sheet1.Columns.Get(8).Width = 35F;
                this.neuSpread1_Sheet1.Columns.Get(9).Label = "Ӧִ��ʱ��";
                this.neuSpread1_Sheet1.Columns.Get(9).Width = 110F;
                this.neuSpread1_Sheet1.Columns.Get(10).Label = "�ֽ�ʱ��";
                this.neuSpread1_Sheet1.Columns.Get(10).Width = 60F;
                this.neuSpread1_Sheet1.Columns.Get(11).Label = "����ʱ��";
                this.neuSpread1_Sheet1.Columns.Get(11).Width = 110F;
                this.neuSpread1_Sheet1.Columns.Get(12).Label = "��ҩʱ��";
                this.neuSpread1_Sheet1.Columns.Get(12).Width = 110F;
                this.neuSpread1_Sheet1.Columns.Get(13).Label = "ҽ��ʱ��";
                this.neuSpread1_Sheet1.Columns.Get(13).Width = 110F;
                this.neuSpread1_Sheet1.Columns.Get(14).Label = "ֹͣʱ��";
                this.neuSpread1_Sheet1.Columns.Get(14).Width = 110F;
                this.neuSpread1_Sheet1.Columns.Get(15).Label = "Ƶ��";
                this.neuSpread1_Sheet1.Columns.Get(15).Width = 47F;
                this.neuSpread1_Sheet1.Columns.Get(16).Label = "ÿ�μ���";
                this.neuSpread1_Sheet1.Columns.Get(16).Width = 56F;
                this.neuSpread1_Sheet1.Columns.Get(17).Label = "��λ";
                this.neuSpread1_Sheet1.Columns.Get(17).Width = 35F;
                this.neuSpread1_Sheet1.Columns.Get(18).Label = "��װ��";
                this.neuSpread1_Sheet1.Columns.Get(18).Width = 53F;
                this.neuSpread1_Sheet1.Columns.Get(19).Label = "����";
                this.neuSpread1_Sheet1.Columns.Get(19).Width = 45F;
                this.neuSpread1_Sheet1.Columns.Get(20).Label = "�÷�";
                this.neuSpread1_Sheet1.Columns.Get(20).Width = 54F;
                this.neuSpread1_Sheet1.Columns.Get(21).Label = "ȡҩҩ��";
                this.neuSpread1_Sheet1.Columns.Get(21).Width = 111F;
                this.neuSpread1_Sheet1.Columns.Get(22).Label = "ҽ��˵��";
                this.neuSpread1_Sheet1.Columns.Get(22).Width = 74F;
                this.neuSpread1_Sheet1.Columns.Get(23).Label = "��ע";
                this.neuSpread1_Sheet1.Columns.Get(23).Width = 51F;
                this.neuSpread1_Sheet1.Columns.Get(24).Label = "ҽ����";
                this.neuSpread1_Sheet1.Columns.Get(24).Width = 70F;
                this.neuSpread1_Sheet1.Columns.Get(25).Label = "��Ϻ�";
                this.neuSpread1_Sheet1.Columns.Get(25).Width = 67F;
                this.neuSpread1_Sheet1.Columns.Get(26).Label = "ִ�к�";
                this.neuSpread1_Sheet1.Columns.Get(26).Width = 69F;
                this.neuSpread1_Sheet1.Columns.Get(27).Label = "����";
                this.neuSpread1_Sheet1.Columns.Get(27).Width = 38F;
                this.neuSpread1_Sheet1.Columns.Get(28).Label = "ִ�п���";
                this.neuSpread1_Sheet1.Columns.Get(28).Width = 127F;
                this.neuSpread1_Sheet1.Columns.Get(29).Label = "���˱��";
                this.neuSpread1_Sheet1.Columns.Get(29).Width = 56F;
                this.neuSpread1_Sheet1.Columns.Get(30).Label = "������";
                this.neuSpread1_Sheet1.Columns.Get(30).Width = 45F;
                this.neuSpread1_Sheet1.Columns.Get(31).Label = "��ҩ����";
                this.neuSpread1_Sheet1.Columns.Get(31).Width = 104F;
                this.neuSpread1_Sheet1.Columns.Get(32).Label = "��ҩ��";
                this.neuSpread1_Sheet1.Columns.Get(32).Width = 45F;
                this.neuSpread1_Sheet1.Columns.Get(33).Label = "ֹͣ��";
                this.neuSpread1_Sheet1.Columns.Get(33).Width = 45F;
                this.neuSpread1_Sheet1.Columns.Get(34).Label = "������";
                this.neuSpread1_Sheet1.Columns.Get(34).Width = 65F;
                this.neuSpread1_Sheet1.Columns.Get(35).Label = "��������ˮ��";
                //this.neuSpread1_Sheet1.Columns.Get(35).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(36).Label = "���͵���ӡ���";
                //this.neuSpread1_Sheet1.Columns.Get(36).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(37).Label = "��ӡʱ��";
                //this.neuSpread1_Sheet1.Columns.Get(37).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(38).Label = "���ʹ���";
                //this.neuSpread1_Sheet1.Columns.Get(38).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(39).Label = "ҩƷ����";
                //this.neuSpread1_Sheet1.Columns.Get(39).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(40).Label = "סԺ����";
                //this.neuSpread1_Sheet1.Columns.Get(40).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(41).Label = "����վ";
                //this.neuSpread1_Sheet1.Columns.Get(41).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(42).Label = "��������";
                //this.neuSpread1_Sheet1.Columns.Get(42).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(43).Label = "ƴ����";
                //this.neuSpread1_Sheet1.Columns.Get(43).Visible = false;
                this.neuSpread1_Sheet1.Columns.Get(44).Label = "�����";
                //this.neuSpread1_Sheet1.Columns.Get(44).Visible = false;

                this.RefreshDrugBackColor();

                this.neuSpread1_Sheet1.DefaultStyle.Locked = true;
                this.neuSpread1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
                this.neuSpread1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
                this.neuSpread1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
                this.neuSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
                this.neuSpread1_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
                //this.neuSpread1_Sheet1.SetColumnAllowAutoSort(-1, true);
                #endregion
            }
        }

        /// <summary>
        /// ���÷�ҩƷִ�е���ʾ��ʽ
        /// </summary>
        protected void SetFormatForUnDrug()
        {
            if (System.IO.File.Exists(this.undrugQuery))
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.neuSpread2_Sheet1, this.undrugQuery);
            }
            else
            {
                #region ȱʡ����

                this.neuSpread2_Sheet1.Columns.Get(0).Label = "����ҽ��";
                this.neuSpread2_Sheet1.Columns.Get(0).Width = 56F;
                this.neuSpread2_Sheet1.Columns.Get(1).Label = "ҽ������";
                this.neuSpread2_Sheet1.Columns.Get(1).Width = 56F;
                this.neuSpread2_Sheet1.Columns.Get(2).Label = "��Ч";
                this.neuSpread2_Sheet1.Columns.Get(2).Width = 35F;

                this.neuSpread2_Sheet1.Columns.Get(3).Label = "��Ŀ����";
                this.neuSpread2_Sheet1.Columns.Get(3).Width = 117F;
                this.neuSpread2_Sheet1.Columns.Get(4).Label = "����";
                this.neuSpread2_Sheet1.Columns.Get(4).Width = 60F;
                this.neuSpread2_Sheet1.Columns.Get(5).Label = "��λ";
                this.neuSpread2_Sheet1.Columns.Get(5).Width = 35F;
                this.neuSpread2_Sheet1.Columns.Get(6).Label = "Ӧִ��ʱ��";
                this.neuSpread2_Sheet1.Columns.Get(6).Width = 110F;
                this.neuSpread2_Sheet1.Columns.Get(7).Label = "�ֽ�ʱ��";
                this.neuSpread2_Sheet1.Columns.Get(7).Width = 60F;
                this.neuSpread2_Sheet1.Columns.Get(8).Label = "����ʱ��";
                this.neuSpread2_Sheet1.Columns.Get(8).Width = 110F;
                this.neuSpread2_Sheet1.Columns.Get(9).Label = "ҽ��ʱ��";
                this.neuSpread2_Sheet1.Columns.Get(9).Width = 110F;
                this.neuSpread2_Sheet1.Columns.Get(10).Label = "ֹͣʱ��";
                this.neuSpread2_Sheet1.Columns.Get(10).Width = 110F;
                this.neuSpread2_Sheet1.Columns.Get(11).Label = "Ƶ��";
                this.neuSpread2_Sheet1.Columns.Get(11).Width = 50F;
                this.neuSpread2_Sheet1.Columns.Get(12).Label = "ҽ��˵��";
                this.neuSpread2_Sheet1.Columns.Get(12).Width = 51F;

                this.neuSpread2_Sheet1.Columns.Get(13).Label = "��ע";
                this.neuSpread2_Sheet1.Columns.Get(13).Width = 51F;
                this.neuSpread2_Sheet1.Columns.Get(14).Label = "ҽ����";
                this.neuSpread2_Sheet1.Columns.Get(14).Width = 70F;

                this.neuSpread2_Sheet1.Columns.Get(15).Label = "��Ϻ�";
                this.neuSpread2_Sheet1.Columns.Get(15).Width = 67F;
                this.neuSpread2_Sheet1.Columns.Get(16).Label = "ִ�к�";
                this.neuSpread2_Sheet1.Columns.Get(16).Width = 69F;
                this.neuSpread2_Sheet1.Columns.Get(17).Label = "����";
                this.neuSpread2_Sheet1.Columns.Get(17).Width = 38F;
                this.neuSpread2_Sheet1.Columns.Get(18).Label = "ִ�п���";
                this.neuSpread2_Sheet1.Columns.Get(18).Width = 127F;
                this.neuSpread2_Sheet1.Columns.Get(19).Label = "���˱��";
                this.neuSpread2_Sheet1.Columns.Get(19).Width = 56F;
                this.neuSpread2_Sheet1.Columns.Get(20).Label = "������";
                this.neuSpread2_Sheet1.Columns.Get(20).Width = 45F;
                this.neuSpread2_Sheet1.Columns.Get(21).Label = "ֹͣ��";
                this.neuSpread2_Sheet1.Columns.Get(21).Width = 45F;
                this.neuSpread2_Sheet1.Columns.Get(22).Label = "������";
                this.neuSpread2_Sheet1.Columns.Get(22).Width = 65F;
                this.neuSpread2_Sheet1.Columns.Get(23).Label = "��������ˮ��";
                this.neuSpread2_Sheet1.Columns.Get(23).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(24).Label = "���͵���ӡ���";
                this.neuSpread2_Sheet1.Columns.Get(24).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(25).Label = "��ӡʱ��";
                this.neuSpread2_Sheet1.Columns.Get(25).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(26).Label = "��ҩƷ����";
                this.neuSpread2_Sheet1.Columns.Get(26).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(27).Label = "סԺ����";
                this.neuSpread2_Sheet1.Columns.Get(27).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(28).Label = "����վ";
                this.neuSpread2_Sheet1.Columns.Get(28).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(29).Label = "��������";
                this.neuSpread2_Sheet1.Columns.Get(29).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(30).Label = "ƴ����";
                this.neuSpread2_Sheet1.Columns.Get(30).Visible = false;
                this.neuSpread2_Sheet1.Columns.Get(31).Label = "�����";
                this.neuSpread2_Sheet1.Columns.Get(31).Visible = false;

                RefreshUndrugFlag();

                this.neuSpread2_Sheet1.DefaultStyle.Locked = true;
                this.neuSpread2_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
                this.neuSpread2_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
                this.neuSpread2_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
                this.neuSpread2_Sheet1.RowHeader.Columns.Default.Resizable = false;
                this.neuSpread2_Sheet1.RowHeader.Columns.Get(0).Width = 30F;
                //this.neuSpread1_Sheet1.SetColumnAllowAutoSort(-1, true);
                #endregion
            }
        }

        protected void RefreshUndrugFlag()
        {
            //this.neuSpread2_Sheet1.Columns.Add(0, 1);//�������
            //this.neuSpread2_Sheet1.Columns.Get(0).Label = "���";
            //this.neuSpread2_Sheet1.Columns.Get(0).Width = 60F;
            //for (int i = 0; i < this.neuSpread2.Sheets[0].RowCount; i++)
            //{
                //int iFee = int.Parse(this.neuSpread2.Sheets[0].Cells[i, 19].Text);
                //if (iFee == 1)
                //{
                //    this.neuSpread2.Sheets[0].Cells[i, 0].Text = "���շ�";
                //}
                //else
                //{
                //    if (this.neuSpread2.Sheets[0].Cells[i, 24].Text == "1")
                //    {
                //        this.neuSpread2.Sheets[0].Cells[i, 0].Text = "���շ�";
                //    }
                //    else
                //    {
                //        this.neuSpread2.Sheets[0].Cells[i, 0].Text = "����δ�շ�";
                //    }
                //}
            //}
        }

        protected void RefreshDrugBackColor()
        {
            //if (this.neuSpread1_Sheet1.Columns.Get(0).Label != "���")
            //{
            //    this.neuSpread1_Sheet1.Columns.Add(0, 1);
            //    this.neuSpread1_Sheet1.Columns.Get(0).Label = "���";
            //    this.neuSpread1_Sheet1.Columns.Get(0).Width = 40F;
            //    FarPoint.Win.Spread.CellType.CheckBoxCellType cmbCkbType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            //    this.neuSpread1_Sheet1.Columns.Get(0).CellType = cmbCkbType;
            //    this.neuSpread1_Sheet1.Columns[0].Locked = false;
            //}
            for (int i = 0; i < this.neuSpread1.Sheets[0].RowCount; i++)
            {
                string strValid = this.neuSpread1_Sheet1.Cells[i, 3].Text;
                this.neuSpread1_Sheet1.Rows[i].BackColor = Color.White;
                if (strValid == "��Ч")
                {
                    this.neuSpread1_Sheet1.Rows[i].BackColor = Color.Yellow;
                }
            }
        }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        public void ShowData(string inPatientNo)
        {
            //this.Patient = this.patientManager.PatientQuery(inPatientNo);
            if (this.Patient == null || this.Patient.ID == "")
            {
                //�������
                this.ClearData();
                return;
            }
            #region {04F3D275-F400-4b52-88E7-9F25F5451CD4} ��ʾ������Ϣadd by guanyx
            //this.lblPatientInfo.Text = "סԺ�ţ�" + this.patient.ID.Substring(5) + "   ������" + this.patient.Name +
            //    "   ���㷽ʽ��" + this.patient.Pact.Name + "   ��" + this.patient.FT.LeftCost.ToString() +
            //    "   �Ա�" + this.patient.Sex.Name + "   ���䣺" + this.patient.Age;
            //�������ͳһ�㷨 {DD9FDB7F-3F52-48e2-A0E9-3698B7B72A73} wbo 2011-1-13
            this.lblPatientInfo.Text = "סԺ�ţ�" + this.patient.ID.Substring(5) + "   ������" + this.patient.Name +
                "   ���㷽ʽ��" + this.patient.Pact.Name + "   ��" + this.patient.FT.LeftCost.ToString() +
                "   �Ա�" + this.patient.Sex.Name + "   ���䣺" + Neusoft.HISFC.BizProcess.Integrate.Function.GetAge(this.patient.Birthday);
            #endregion 
            //��ʾҩƷִ�е���Ϣ
            if (this.Patient.ID == "") return;

            #region ȡҩƷִ�е�����
            this.myDataSetDrug = this.myOrder.QueryExecDrugOrderByInpatientNo(this.Patient.ID);
            if (this.myDataSetDrug == null)
            {
                MessageBox.Show(this.myOrder.Err);
                return;
            }

            //�������еı���ת��Ϊ����
            for (int i = 0; i < this.myDataSetDrug.Tables[0].Rows.Count; i++)
            {
                this.objHelper.ArrayObject = this.myDeptList;
                this.myDataSetDrug.Tables[0].Rows[i]["ȡҩҩ��"] = objHelper.GetName(this.myDataSetDrug.Tables[0].Rows[i]["ȡҩҩ��"].ToString());
                this.myDataSetDrug.Tables[0].Rows[i]["ִ�п���"] = objHelper.GetName(this.myDataSetDrug.Tables[0].Rows[i]["ִ�п���"].ToString());
                this.myDataSetDrug.Tables[0].Rows[i]["��ҩ����"] = objHelper.GetName(this.myDataSetDrug.Tables[0].Rows[i]["��ҩ����"].ToString());
                this.objHelper.ArrayObject = this.myOrderTypeList;
                this.myDataSetDrug.Tables[0].Rows[i]["ҽ������"] = objHelper.GetName(this.myDataSetDrug.Tables[0].Rows[i]["ҽ������"].ToString());
            }
            //��ȡ�õ�������ʾ���ؼ���
            this.myDataViewDrug = new DataView(this.myDataSetDrug.Tables[0]);
            this.neuSpread1_Sheet1.DataSource = this.myDataViewDrug;
            //������ʾ��ʽ
            this.SetFormatForDrug();
            #endregion

            #region ȡ��ҩƷִ�е�����
            this.myDataSetUndrug = this.myOrder.QueryExecUndrugOrderByInpatientNo(this.Patient.ID);
            if (this.myDataSetUndrug == null)
            {
                MessageBox.Show(this.myOrder.Err);
                return;
            }
            //�������еı���ת��Ϊ����
            for (int i = 0; i < this.myDataSetUndrug.Tables[0].Rows.Count; i++)
            {
                this.objHelper.ArrayObject = this.myDeptList;
                this.myDataSetUndrug.Tables[0].Rows[i]["ִ�п���"] = objHelper.GetName(this.myDataSetUndrug.Tables[0].Rows[i]["ִ�п���"].ToString());
                //this.myDataSetUndrug.Tables[0].Rows[i]["סԺ����"] = objHelper.GetName(this.myDataSetUndrug.Tables[0].Rows[i]["סԺ����"].ToString());
                //this.myDataSetUndrug.Tables[0].Rows[i]["ҽ������վ"] = objHelper.GetName(this.myDataSetUndrug.Tables[0].Rows[i]["ҽ������վ"].ToString());
                //this.myDataSetUndrug.Tables[0].Rows[i]["��������"] = objHelper.GetName(this.myDataSetUndrug.Tables[0].Rows[i]["��������"].ToString());
                this.objHelper.ArrayObject = this.myOrderTypeList;
                this.myDataSetUndrug.Tables[0].Rows[i]["ҽ������"] = objHelper.GetName(this.myDataSetUndrug.Tables[0].Rows[i]["ҽ������"].ToString());
            }
            //��ȡ�õ�DataSet������ʾ�ؼ�
            this.myDataViewUndrug = new DataView(this.myDataSetUndrug.Tables[0]);
            this.neuSpread2_Sheet1.DataSource = this.myDataViewUndrug;
            //������ʾ��ʽ
            this.SetFormatForUnDrug();
            #endregion
        }

        /// <summary>
        /// �������
        /// </summary>
        public void ClearData()
        {
            this.neuSpread1_Sheet1.Rows.Count = 0;
            this.neuSpread2_Sheet1.Rows.Count = 0;
        }

        /// <summary>
        /// ������Ч��� addby xuewj 2009-8-24 �ָ��������������Ŀ���Ա�ʹ������������ҩ,����ִ�е� {01F18F48-887D-4d2a-A0F9-757B61A5B8A6}
        /// </summary>
        /// <param name="RowIndex"></param>
        private void SetValidFlag(int RowIndex, string flag)
        {
            //if (this.neuSpread1_Sheet1.Cells[RowIndex, 3].Text.Trim() == "��Ч")
            //{
            //    return;
            //}

            if (this.neuSpread1_Sheet1.Cells[RowIndex, 4].Text.Trim() != "δ����")
            {
                MessageBox.Show("ֻ��δ���͵�ҩƷ�ſ��Բ�����");
                return;
            }

            DialogResult r;

            if (flag == "0")
            {
                r = MessageBox.Show("ȷ��Ҫ�ָ�������¼����Ч����?", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                r = MessageBox.Show("ȷ��Ҫ���ϸ�����¼��?,�ò������ɳ���", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            if (r == DialogResult.Cancel)
            {
                return;
            }

            string execOrderID = this.neuSpread1_Sheet1.Cells[RowIndex, 26].Text.Trim();

            if (execOrderID == null || execOrderID.Length <= 0)
            {
                MessageBox.Show("ִ����ˮ��Ϊ�գ�");
                return;
            }
            if (flag == "1")
            {
                if (this.neuSpread1_Sheet1.Cells[RowIndex, 3].Text.Trim() != "��Ч")
                {
                    MessageBox.Show("������¼�Ѿ����ϣ�");
                    return;
                }

                this.myExeOrder = new Neusoft.HISFC.Models.Order.ExecOrder();
                Neusoft.HISFC.Models.Pharmacy.Item objPharmacy = new Neusoft.HISFC.Models.Pharmacy.Item();
                objPharmacy.ID = this.neuSpread1_Sheet1.Cells[RowIndex, 39].Text;//ҩƷ����
                objPharmacy.Name = this.neuSpread1_Sheet1.Cells[RowIndex, 5].Text;//ҩƷ����
                objPharmacy.Specs = this.neuSpread1_Sheet1.Cells[RowIndex, 6].Text;//ҩƷ���
                objPharmacy.Memo = this.neuSpread1_Sheet1.Cells[RowIndex, 21].Text;//ȡҩҩ��
                this.myExeOrder.Order.Item = objPharmacy;//ִ�е���Ŀ
                this.myExeOrder.Order.Qty = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[RowIndex, 7].Value);//ҩƷ����
                this.myExeOrder.Order.Unit = this.neuSpread1_Sheet1.Cells[RowIndex, 8].Text;//ҩƷ��λ
                this.myExeOrder.ID = execOrderID;

                int _Ret = this.myOrder.DcExecImmediate(this.myExeOrder, this.myOrder.Operator);

                //int _Ret = this.myOrder.UpdateExecValidFlag(execOrderID, true, flag);

                if (_Ret < 0)
                {
                    MessageBox.Show("���ϼ�¼����");
                    return;
                }

                //_Ret = this.myCharegeBill.DeleteChargeBill(execOrderID);

                //if (_Ret < 0)
                //{
                //    MessageBox.Show("ɾ��������¼����");
                //    return;
                //}

                MessageBox.Show("���ϼ�¼�ɹ���");
                this.ShowData(this.patient.ID);

                if (this.neuSpread1_Sheet1.Cells[RowIndex, 28].Text.Trim() == "�Ѽ�")
                {
                    MessageBox.Show("�ü�¼�Ѿ��շѣ����˷ѣ�");
                }
            }
            else
            {
                int _Ret = this.myOrder.UpdateExecValidFlag(execOrderID, true, "1");

                if (_Ret < 0)
                {
                    MessageBox.Show("�ָ���¼����");
                    return;
                }

                MessageBox.Show("�ָ���¼�ɹ���");
                this.ShowData(this.patient.ID);
            }
        }

        /// <summary>
        /// ���ù�������,��������
        /// </summary>
        private void SetFilter()
        {
            //��������
            //ҩƷ
            if (this.myDataViewDrug.Table != null && this.myDataViewDrug.Table.Rows.Count > 0)
            {
                this.myDataViewDrug.RowFilter = this.filterInput + " AND " + this.filterValid + " AND " + this.filterExec + " AND " + this.filterType;
                this.SetFormatForDrug();
            }

            //��ҩƷ
            if (this.myDataViewUndrug.Table != null && this.myDataViewUndrug.Table.Rows.Count > 0)
            {
                this.myDataViewUndrug.RowFilter = this.filterInput + " AND " + this.filterValid + " AND " + this.filterType;
                this.SetFormatForUnDrug();
            }
        }

        /// <summary>
        /// ������״̬����ҽ��
        /// </summary>
        /// <param name="State"></param>
        public void Filter1(int State)
        {
            if (this.Patient == null) return;
            if (this.Patient.ID == "") return;

            //��ѯʱ����ܹ���
            switch (State)
            {
                case 0://ȫ��
                    this.filterExec = "1=1";
                    break;
                case 1://����
                    this.filterExec = "����״̬ = '�ѷ���'";//3
                    break;
                case 2://��Ч
                    this.filterExec = "����״̬ = 'δ����'";
                    break;
                case 3:
                    this.filterExec = "����״̬ = '�ѷ�ҩ'";
                    break;
                default:
                    this.filterExec = "1=1";
                    this.filterValid = "1=1";
                    this.filterType = "1=1";
                    break;
            }
            this.SetFilter();
        }

        /// <summary>
        /// ����ҽ����ʾ
        /// </summary>
        /// <param name="State"></param>
        public void Filter2(int State)
        {
            if (this.Patient == null) return;
            if (this.Patient.ID == "") return;
            //��ѯʱ����ܹ���
            switch (State)
            {
                case 0://ȫ��
                    this.filterValid = "1=1";
                    break;
                case 1://����
                    this.filterValid = "��Ч = '��Ч'";//3
                    break;
                case 2://��Ч
                    this.filterValid = "��Ч = '��Ч'";
                    break;
                default:
                    this.filterExec = "1=1";
                    this.filterValid = "1=1";
                    this.filterType = "1=1";
                    break;
            }
            this.SetFilter();
        }

        /// <summary>
        /// ����ҽ����ʾ
        /// </summary>
        /// <param name="State"></param>
        public void Filter3(int State)
        {
            if (this.Patient == null) return;
            if (this.Patient.ID == "") return;
            //��ѯʱ����ܹ���
            switch (State)
            {
                case 0://ȫ��
                    this.filterType = "1=1";
                    break;
                case 1://����ҽ��
                    this.filterType = "ҽ������ = '����ҽ��'";
                    break;
                case 2://��ʱҽ��
                    this.filterType = "ҽ������ = '��ʱҽ��'";
                    break;
                default:
                    this.filterExec = "1=1";
                    this.filterValid = "1=1";
                    this.filterType = "1=1";
                    break;
            }
            this.SetFilter();
        }

        /// <summary>
        /// load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucOrderExeQuery_Load(object sender, EventArgs e)
        {
            Neusoft.HISFC.BizProcess.Integrate.Manager integrateManager = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            this.myDeptList = integrateManager.GetDepartment();
            if (this.myDeptList == null)
            {
                MessageBox.Show(integrateManager.Err);
                return;
            }
            this.myOrderTypeList = integrateManager.QueryOrderTypeList();
            if (this.myOrderTypeList == null)
            {
                MessageBox.Show(integrateManager.Err);
                return;
            }
        }

        /// <summary>
        /// ��Ч״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter2(this.neuComboBox2.SelectedIndex);
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter1(this.neuComboBox1.SelectedIndex);
        }

        /// <summary>
        /// ����ƴ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuTextBox1_TextChanged(object sender, EventArgs e)
        {
            //ȡ������
            string queryCode = this.neuTextBox1.Text;
            //if (this.chbMisty.Checked)
            //{
            //    queryCode = "%" + queryCode + "%";
            //}
            //else
            //{
                queryCode = queryCode + "%";
            //}

            //���ù�������
            this.filterInput = "((ƴ���� LIKE '" + queryCode + "') OR " +
                "(����� LIKE '" + queryCode + "') OR " +
                "(���� LIKE '" + queryCode + "') )";

            //����ҩƷ����
            this.SetFilter();
        }

        /// <summary>
        /// fp1����Ϊ xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.neuSpread1_Sheet1, this.drugQuery);
        }

        /// <summary>
        /// fp2����Ϊxml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread2_ColumnWidthChanged(object sender, FarPoint.Win.Spread.ColumnWidthChangedEventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.neuSpread2_Sheet1, this.undrugQuery);
        }

        /// <summary>
        /// ˫��ĳһ����Ϊ��Ч
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                if (this.neuSpread1_Sheet1.ActiveRowIndex >= 0)
                {
                    this.SetValidFlag(this.neuSpread1_Sheet1.ActiveRowIndex, "0");
                }
            }
        }

        /// <summary>
        /// ҽ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void neuComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter3(this.neuComboBox3.SelectedIndex);
        }

        /// <summary>
        /// ��ӡ����,ȡ�ӿڷ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnPrint(object sender, object neuObject)
        {
            if (this.GetItemInfo() == -1) return -1;
            Neusoft.HISFC.Components.Order.Classes.IOrderExeQuery printInterFace = null;
            printInterFace = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.Components.Order.Classes.IOrderExeQuery)) as Neusoft.HISFC.Components.Order.Classes.IOrderExeQuery;
            if (printInterFace != null)
            {
                printInterFace.patientInfoObj = this.patient;
                if (printInterFace.SetValue(this.alExeOrder) == 1)
                {
                    printInterFace.Print();
                }
            }
            return base.OnPrint(sender, neuObject);
        }
        private int GetItemInfo()
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                Hashtable myhashtable = new Hashtable();
                for (int a = 0; a < this.neuSpread1_Sheet1.RowCount; a++)
                {
                    if (Neusoft.FrameWork.Function.NConvert.ToBoolean(this.neuSpread1_Sheet1.Cells[a, 0].Value) == true)
                    {
                        string strDrugStoreName = this.neuSpread1_Sheet1.Cells[a, 21].Text;
                        if (!myhashtable.ContainsKey(strDrugStoreName))
                        {
                            myhashtable.Add(strDrugStoreName, strDrugStoreName);
                        }
                    }
                }
                if (myhashtable.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("��ѡ���˶��ҩ����ҩƷ��������˶Դ�ӡҩ����");
                    return -1;
                }
                this.alExeOrder = new ArrayList();
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    if (Neusoft.FrameWork.Function.NConvert.ToBoolean( this.neuSpread1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (this.neuSpread1_Sheet1.Cells[i, 2].Text == "����ҽ��")
                        {
                            System.Windows.Forms.MessageBox.Show("���ڵڡ�" + (i + 1).ToString() + " ����ѡ���˳���ҽ��,��ѡ����ʱҽ����ӡҩ����");
                            return -1;

                        }
                        if (this.neuSpread1_Sheet1.Cells[i, 21].Text == this.neuSpread1_Sheet1.Cells[i, 21].Text)
                        {
                            
                        }
                        try
                        {
                            this.myExeOrder = new Neusoft.HISFC.Models.Order.ExecOrder();
                            Neusoft.HISFC.Models.Pharmacy.Item objPharmacy = new Neusoft.HISFC.Models.Pharmacy.Item();
                            objPharmacy.ID    = this.neuSpread1_Sheet1.Cells[i, 39].Text;//ҩƷ����
                            objPharmacy.Name  = this.neuSpread1_Sheet1.Cells[i, 5].Text;//ҩƷ����
                            objPharmacy.Specs = this.neuSpread1_Sheet1.Cells[i, 6].Text;//ҩƷ���
                            objPharmacy.Memo  = this.neuSpread1_Sheet1.Cells[i, 21].Text;//ȡҩҩ��
                            this.myExeOrder.Order.Item = objPharmacy;//ִ�е���Ŀ
                            this.myExeOrder.Order.Qty  = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.neuSpread1_Sheet1.Cells[i, 7].Value);//ҩƷ����
                            this.myExeOrder.Order.Unit = this.neuSpread1_Sheet1.Cells[i, 8].Text;//ҩƷ��λ
                            this.alExeOrder.Add(this.myExeOrder);
                        }
                        catch(Exception ex)
                        {
                            return -1;
                        }
                        
                    }
                }
            }
            return 1;
        }

        #region addby xuewj 2009-8-24 �ָ��������������Ŀ���Ա�ʹ������������ҩ,����ִ�е� {01F18F48-887D-4d2a-A0F9-757B61A5B8A6}

        private void btnVaildExecOrder_Click(object sender, EventArgs e)
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                #region {6F8B4125-5B85-4fbd-A522-460D9F9ECC7D}
                //if (this.neuSpread1_Sheet1.ActiveRowIndex >= 0)
                if (this.neuSpread1_Sheet1.ActiveRowIndex >= 0 && this.neuSpread1_Sheet1.RowCount > 0)
                #endregion
                {
                    this.SetValidFlag(this.neuSpread1_Sheet1.ActiveRowIndex, "0");
                }
            }
        }

        private void btnUNVaildExecOrder_Click(object sender, EventArgs e)
        {
            if (this.neuTabControl1.SelectedTab == this.tabPage1)
            {
                #region {6F8B4125-5B85-4fbd-A522-460D9F9ECC7D}
                //if (this.neuSpread1_Sheet1.ActiveRowIndex >= 0)
                if (this.neuSpread1_Sheet1.ActiveRowIndex >= 0 && this.neuSpread1_Sheet1.RowCount > 0)
                #endregion
                {
                    this.SetValidFlag(this.neuSpread1_Sheet1.ActiveRowIndex, "1");
                }
            }
        }

        #endregion
    }
}
