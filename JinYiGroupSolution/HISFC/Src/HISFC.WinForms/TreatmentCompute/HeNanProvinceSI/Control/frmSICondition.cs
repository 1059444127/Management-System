using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace HeNanProvinceSI.Control
{
    public partial class frmSICondition : Form
    {
        public frmSICondition()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;
            try
            {
                this.InitDiagnose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private Neusoft.FrameWork.Models.NeuObject desease = new Neusoft.FrameWork.Models.NeuObject();

        private HeNanProvinceSI.LocalManager localManager = new LocalManager();

        private Neusoft.HISFC.Models.RADT.PatientInfo p = null;

        private Neusoft.HISFC.Models.Registration.Register r = null;

        private bool isSaveDiag = false;

        /// <summary>
        /// ��ͬ��λ
        /// </summary>
        private string pactCode = "";

        /// <summary>
        /// ��ͬ��λ
        /// </summary>
        public string PactCode
        {
            get { return this.pactCode; }
            set { this.pactCode = value; }
        }

        /// <summary>
        /// �Ƿ��ڵ��������ڱ���ҽ�����true��ȷ����ֱ�ӱ���false��ȷ���˲����棬����ѡ������
        /// </summary>
        public bool IsSaveDiag
        {
            get { return this.isSaveDiag; }
            set { this.isSaveDiag = value; }
        }

        private string type = "";

        /// <summary>
        /// ���û�����Ϣ 1����2סԺ
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        public void SetPatient(Neusoft.FrameWork.Models.NeuObject o, string type)
        {
            p = null;
            r = null;
            this.type = type;
            if (type == "1")
            {
                r = o as Neusoft.HISFC.Models.Registration.Register;
            }
            if (type == "2")
            {
                p = o as Neusoft.HISFC.Models.RADT.PatientInfo;
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject Desease
        {
            get { return this.desease; }
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <returns></returns>
        private int InitDiagnose()
        {
            ArrayList al = new ArrayList();
            al = this.localManager.GetDiagnoseByPactCode(this.pactCode);
            if (al != null && al.Count != 0)
            {
                this.cmbDesease.AddItems(al);
            }
            return 1;
        }

        private void ShowHistoryDiag()
        {
            if (this.type == "1")
            {
                r = this.localManager.GetSIPersonInfoOutPatient(r.ID);
                if (r != null)
                {
                    this.cmbDesease.Tag = r.SIMainInfo.OutDiagnose.ID;
                }
            }
            if (this.type == "2")
            {
                p = this.localManager.GetSIPersonInfo(p.ID, "0");
                if (p != null)
                {
                    this.cmbDesease.Tag = p.SIMainInfo.OutDiagnose.ID;
                }
            }
        }

        private int SaveDiag()
        {
            if (this.cmbDesease.SelectedItem == null)
            {
                MessageBox.Show("��ѡ����Ŀ��");
                return -1;
            }
            if (this.type == "1")
            {
                try
                {
                    //string sql = "update fin_ipr_siinmaininfo i set i.out_diagnose='{2}',i.out_diagnosename='{3}' where i.inpatient_no='{0}' and i.balance_no='{1}'";
                    //sql = string.Format(sql, r.ID, r.SIMainInfo.BalNo, this.cmbDesease.SelectedItem.ID, this.cmbDesease.SelectedItem.Name);
                    string sql = "update fin_ipr_siinmaininfo i set i.out_diagnose='{1}',i.out_diagnosename='{2}' where i.inpatient_no='{0}'";
                    sql = string.Format(sql, r.ID, this.cmbDesease.SelectedItem.ID, this.cmbDesease.SelectedItem.Name);
                    return this.localManager.ExecNoQuery(sql);
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
            if (this.type == "2")
            {
                try
                {
                    string sql = "update fin_ipr_siinmaininfo i set i.out_diagnose='{2}',i.out_diagnosename='{3}' where i.inpatient_no='{0}' and i.balance_no='{1}'";
                    sql = string.Format(sql, p.ID, p.SIMainInfo.BalNo, this.cmbDesease.SelectedItem.ID, this.cmbDesease.SelectedItem.Name);
                    return this.localManager.ExecNoQuery(sql);
                }
                catch (Exception e)
                {
                    return -1;
                }
            }

            return 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            desease = new Neusoft.FrameWork.Models.NeuObject();
            desease.ID = "";
            desease.Name = "";
            if (this.cmbDesease.SelectedItem != null)
            {
                desease.ID = this.cmbDesease.SelectedItem.ID; 
                desease.Name = this.cmbDesease.SelectedItem.Name;
            }
            else
            {
                MessageBox.Show("���벻��Ϊ�գ�");
                return;
            }
            if (this.isSaveDiag == false)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (this.SaveDiag() != 1)
                {
                    MessageBox.Show("����ҽ�����ʧ�ܣ�");
                }
                else
                {
                    //MessageBox.Show("����ҽ����ϳɹ���");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmSICondition_Load(object sender, EventArgs e)
        {
            try
            {
                this.ShowHistoryDiag();
            }
            catch (Exception ee)
            {
                MessageBox.Show("��ȡҽ����ϳ���:" + ee.ToString());
            }
        }
    }
}