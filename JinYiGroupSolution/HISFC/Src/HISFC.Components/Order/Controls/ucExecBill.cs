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
    /// <summary>
    /// [��������: ִ�е���ӡ]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucExecBill : Neusoft.FrameWork.WinForms.Controls.ucBaseControl,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucExecBill()
        {
            InitializeComponent();
        }

        private Neusoft.HISFC.BizLogic.Order.Order Order = new Neusoft.HISFC.BizLogic.Order.Order();
        private Neusoft.HISFC.BizLogic.Order.ExecBill Bill = new Neusoft.HISFC.BizLogic.Order.ExecBill();
        private Neusoft.HISFC.BizProcess.Integrate.Manager bed = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        bool IsPrint = false;//�Ƿ��ӡ״̬
        string Memo = "";//ִ�е�ִ��ʱ��

        DateTime tempDate = new DateTime();


        protected List<Neusoft.HISFC.Models.RADT.PatientInfo> myPatients = null;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public void Init()
        {
            ResetPanel();
            tempDate = this.Order.GetDateTimeFromSysDateTime();
            this.dateTimePicker1.Value = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 00, 00, 00);
            this.dateTimePicker2.Value = new DateTime(tempDate.AddDays(1).Year, tempDate.AddDays(1).Month, tempDate.AddDays(1).Day, 12, 01, 00);

        }

        Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion ip = null;//��ǰ�ӿ�


        protected override void OnLoad(EventArgs e)
        {
            Init();

        }
        public int Retrieve()
        {
            // TODO:  ��� ucDrugCardPanel.Retrieve ʵ��
            if (this.tabControl1.TabPages.Count <= 0) return 0;
            Neusoft.FrameWork.Models.NeuObject obj = ((Neusoft.FrameWork.Models.NeuObject)this.tabControl1.SelectedTab.Tag);
            string BillNo = ((Neusoft.FrameWork.Models.NeuObject)this.tabControl1.SelectedTab.Tag).ID;
            this.IsPrint = false;
            this.Memo = ((Neusoft.FrameWork.Models.NeuObject)this.tabControl1.SelectedTab.Tag).User01;
            this.Query(BillNo);
            return 0;
        }

        private void Query(string billNo)
        {

            if (this.tabControl1.TabPages.Count <= 0 || this.myPatients == null) return;

            IsPrint = this.chkRePrint.Checked;
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯִ�е���Ϣ...");
            if (this.tabControl1.SelectedTab.Controls[0].Controls.Count == 0)
            {
                //��ǰTabҳ���滹û����Һ��
                object o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(HISFC.Components.Order.Controls.ucExecBill), typeof(Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion));
                //object o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(typeof(), typeof(Neusoft.HISFC.BizProcess.Integrate.IPrintTransFusion));
                if (o == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show("��ά��HISFC.Components.Order.Controls.ucExecBill����ӿ�Neusoft.HISFC.BizProcess.Integrate.IPrintTransFusion��ʵ�����գ�");
                    return;
                }
                ip = o as Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion;
                ((Control)o).Tag = tabControl1.SelectedTab.Text;
                ((Control)o).Visible = true;
                ((Control)o).Dock = DockStyle.Fill;
                this.tabControl1.SelectedTab.Controls[0].Controls.Add((Control)o);

            }
            else
            {
                ip = this.tabControl1.SelectedTab.Controls[0].Controls[0] as Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion;
            }
            if (ip == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show("ά����ʵ�����߱�Neusoft.HISFC.BizProcess.Integrate.IPrintTransFusion�ӿ�");
                return;
            }

            try
            {
                ip.Query(this.myPatients, billNo, this.dateTimePicker1.Value, this.dateTimePicker2.Value, this.IsPrint);
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }



        public void ResetPanel()
        {
            ArrayList alBill = new ArrayList();

            try
            {
                //���ִ�е�����
                alBill = Bill.QueryExecBill(((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse.ID);
            }
            catch { MessageBox.Show("���ִ�е��������"); }

            if (alBill == null)
            {
                MessageBox.Show("���ִ�е����ó���");
                return;
            }
            this.tabControl1.TabPages.Clear();

            for (int i = 0; i < alBill.Count; i++)
            {
                TabPage t = new TabPage();
                t.Text = ((Neusoft.FrameWork.Models.NeuObject)alBill[i]).Name;
                t.Tag = alBill[i];

                Panel p = new Panel();
                p.AutoScroll = true;
                p.Dock = DockStyle.Fill;
                p.BackColor = Color.White;

                t.Controls.Add(p);

                this.tabControl1.TabPages.Add(t);
            }


        }



        /// <summary>
        /// ����ִ�е�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            //frmSetExecBill f = new frmSetExecBill(Neusoft.Common.Class.Main.var);
            //f.ShowDialog();
            this.ResetPanel();
        }


        private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.myPatients != null && this.myPatients.Count > 0 && this.tabControl1.TabPages.Count > 0)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾִ�е���Ϣ�����Ժ�........");
                Application.DoEvents();
                string BillNo = ((Neusoft.FrameWork.Models.NeuObject)this.tabControl1.SelectedTab.Tag).ID;
                this.IsPrint = false;

                this.Memo = ((Neusoft.FrameWork.Models.NeuObject)this.tabControl1.SelectedTab.Tag).User01;
                this.Query(BillNo);
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
        }

        /// <summary>
        /// ����仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.Retrieve();
            }
            catch
            {
                MessageBox.Show("���ȵ��ѯ��ť���в�ѯ��");
            }
        }




        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override Neusoft.FrameWork.WinForms.Forms.ToolBarService Init(object sender, object neuObject, object param)
        {
            TreeView tv = sender as TreeView;
            if (tv != null && tv.CheckBoxes == false) tv.CheckBoxes = true;
            this.ResetPanel();
            return null;
        }


        #region donggq--20101118--{7DC99247-EB4B-4660-87D0-E581F9247F51}

        protected override int OnSetValue(object neuObject, TreeNode e)
        {
            if (tv != null && this.tv.CheckBoxes == false)
            {
                tv.CheckBoxes = true;
            }

            if (e != null && e.Tag.ToString() != "In")
            {
                ArrayList patientList = new ArrayList();
                patientList.Add((Neusoft.HISFC.Models.RADT.PatientInfo)e.Tag);
                return this.SetValues(patientList, e);
            }

            return base.OnSetValue(neuObject, e);
        }

        #endregion

        protected override int OnSetValues(ArrayList alValues, object e)
        {
            this.myPatients = new List<Neusoft.HISFC.Models.RADT.PatientInfo>();
            foreach (Neusoft.HISFC.Models.RADT.PatientInfo p in alValues)
            {
                myPatients.Add(p);
            }
            this.Retrieve();
            return 0;
        }

        protected override int OnQuery(object sender, object neuObject)
        {
           // return this.Retrieve();
            return 0;
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int Print(object sender, object neuObject)
        {
            if (ip != null)
                ip.Print();
            return 0;
        }

        /// <summary>
        /// ���ô�ӡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int SetPrint(object sender, object neuObject)
        {
            if (ip != null)
                ip.PrintSet();
            return 0;
        }
         #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get {
                 Type[]  type = new Type[1];
                 type[0] = typeof(Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion);
                return type;
            }
        }

        #endregion

    }
}
