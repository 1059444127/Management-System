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
    /// [��������: ��Һ���ؼ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucDrugCardControl : Neusoft.FrameWork.WinForms.Controls.ucBaseControl,Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucDrugCardControl()
        {
            InitializeComponent();
            btnQuery.Visible = false;
        }



        Neusoft.HISFC.BizLogic.Order.Order orderManager = new Neusoft.HISFC.BizLogic.Order.Order();
        Neusoft.HISFC.BizLogic.Order.TransFusion manager = new Neusoft.HISFC.BizLogic.Order.TransFusion();
        Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion ip = null;//��ǰ�ӿ�
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService  OnInit(object sender, object NeuObject, object param)
        {
            if (tv != null && tv.CheckBoxes == false)
                tv.CheckBoxes = true;
            try
            {
                DateTime dt = orderManager.GetDateTimeFromSysDateTime();
                DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                DateTime dt2 = new DateTime(dt.AddDays(1).Year, dt.AddDays(1).Month, dt.AddDays(1).Day, 12, 00, 00);
                this.dateTimePicker1.Value = dt1;
                this.dateTimePicker2.Value = dt2;
            }
            catch { }
            ResetPanel();
            return null;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void ResetPanel()
        {
            ArrayList alUsage = null;
            Neusoft.HISFC.BizProcess.Integrate.Manager f = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            Neusoft.FrameWork.Public.ObjectHelper helper = null;
            try
            {
                //ϵͳ�÷�
                alUsage = f.GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant.USAGE);
                helper = new Neusoft.FrameWork.Public.ObjectHelper(alUsage);
            }
            catch 
            {
                MessageBox.Show("����÷�����");
                return;
            }

            Neusoft.HISFC.Models.Base.Employee empl = Neusoft.FrameWork.Management.Connection.Operator as Neusoft.HISFC.Models.Base.Employee;
            
            ArrayList al = manager.QueryTransFusion(empl.Nurse.ID);
            if (al == null)
            {
                MessageBox.Show("�����Һ�����ó���");
                return;
            }
            this.neuTabControl1.TabPages.Clear();

            for (int i = 0; i < al.Count; i++)
            {
                TabPage tp = new TabPage(helper.GetName(al[i].ToString()));
                tp.Tag = helper.GetObjectFromID(al[i].ToString());
                Panel p = new Panel();
                p.AutoScroll = true;
                p.Dock = DockStyle.Fill;
                p.BackColor = Color.White;
                tp.Controls.Add(p);
                this.neuTabControl1.TabPages.Add(tp);  
            }
        }

        #region ����

        protected List<Neusoft.HISFC.Models.RADT.PatientInfo> myPatients = null;
        

        #endregion

       
        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <returns></returns>
        public int Retrieve()
        {
            // TODO:  ��� ucDrugCardPanel.Retrieve ʵ��
            if (this.neuTabControl1.TabPages.Count <= 0) return 0;
            string CardCode = ((Neusoft.FrameWork.Models.NeuObject)this.neuTabControl1.SelectedTab.Tag).ID;
            this.Query(CardCode);
            return 0;
        }

        bool bPrint = true;
        private void Query(string usageCode)
        {
            if (this.neuTabControl1.TabPages.Count <= 0 || this.myPatients == null)
            {
                return;
            }

            bPrint = this.chkRePrint.Checked;
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm( "���ڲ�ѯ��Һ����Ϣ..." );
            Application.DoEvents();

            if (this.neuTabControl1.SelectedTab.Controls[0].Controls.Count == 0)
            {
                //��ǰTabҳ���滹û����Һ��
                object o = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject( typeof( HISFC.Components.Order.Controls.ucDrugCardControl ), typeof( Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion ) );
                if (o == null)
                {
                    Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                    MessageBox.Show( "��ά��HISFC.Components.Order.Controls.ucDrugCardControl����ӿ�Neusoft.HISFC.BizProcess.Integrate.IPrintTransFusion��ʵ�����գ�" );
                    return;
                }
                this.ip = o as Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion;
                ((Control)o).Visible = true;
                ((Control)o).Dock = DockStyle.Fill;
                this.neuTabControl1.SelectedTab.Controls[0].Controls.Add( (Control)o );
            }

            ip = this.neuTabControl1.SelectedTab.Controls[0].Controls[0] as Neusoft.HISFC.BizProcess.Interface.IPrintTransFusion;

            if (ip == null)
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
                MessageBox.Show( "ά����ʵ�����߱�Neusoft.HISFC.BizProcess.Integrate.IPrintTransFusion�ӿ�" );
                return;
            }

            try
            {
                ip.Query( this.myPatients, usageCode, this.dateTimePicker1.Value, this.dateTimePicker2.Value, bPrint );
            }
            catch
            {
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.neuTabControl1.SelectedTab == null) return;
            string CardCode = ((Neusoft.FrameWork.Models.NeuObject)this.neuTabControl1.SelectedTab.Tag).ID;
            this.Query(CardCode);
        }

        #region ��д
       

        protected override int OnSetValues(ArrayList alValues, object e)
        {
            this.myPatients = new List<Neusoft.HISFC.Models.RADT.PatientInfo>();
            foreach (Neusoft.HISFC.Models.RADT.PatientInfo p in alValues)
            {
                myPatients.Add(p);
            }
            #region donggq--20101118--{7DC99247-EB4B-4660-87D0-E581F9247F51}
            
            this.Retrieve();
            
            #endregion
            return 0;
        }
        
        #region donggq--20101118--{7DC99247-EB4B-4660-87D0-E581F9247F51}

        //protected override int OnSetValue(object neuObject, TreeNode e)
        //{
        //    if (tv != null && this.tv.CheckBoxes == false)
        //        tv.CheckBoxes = true;
        //    return base.OnSetValue(neuObject, e);
        //}
        
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


        protected override int OnQuery(object sender, object neuObject)
        {
            return this.Retrieve();
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
        #endregion

        private void neuLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forms.frmDrugCardSet f = new HISFC.Components.Order.Forms.frmDrugCardSet();
            if (f.ShowDialog() == DialogResult.OK)
            {
                this.ResetPanel();
            }
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Retrieve();
        }
    }
}
