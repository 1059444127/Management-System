using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Registration
{
    public partial class frmQueryPatientByName : Form
    {
        public frmQueryPatientByName()
        {
            InitializeComponent();

            this.fpSpread1.KeyDown += new KeyEventHandler(fpSpread1_KeyDown);
            this.fpSpread1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSpread1_CellDoubleClick);
        }

        /// <summary>
        /// ������������ѯ���߹Һ���Ϣ
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int QueryByName(string Name)
        {
            Neusoft.HISFC.BizProcess.Integrate.RADT Mgr = new Neusoft.HISFC.BizProcess.Integrate.RADT();

            ArrayList al = Mgr.QueryPatientByName(Name);

            if (al == null)
            {
                MessageBox.Show("��ѯ���߹Һ���Ϣʱ����!" + Mgr.Err, "��ʾ");
                return -1;
            }

            if (this.fpSpread1_Sheet1.RowCount > 0)
                this.fpSpread1_Sheet1.Rows.Remove(0, this.fpSpread1_Sheet1.RowCount);

            foreach (Neusoft.HISFC.Models.RADT.PatientInfo obj in al)
            {
                this.fpSpread1_Sheet1.Rows.Add(this.fpSpread1_Sheet1.RowCount, 1);

                int row = this.fpSpread1_Sheet1.RowCount - 1;

                this.fpSpread1_Sheet1.SetValue(row, 0, obj.PID.CardNO, false);
                this.fpSpread1_Sheet1.SetValue(row, 1, obj.Name, false);
                this.fpSpread1_Sheet1.SetValue(row, 2, obj.Sex.Name, false);
                this.fpSpread1_Sheet1.SetValue(row, 3, obj.PhoneHome, false);
                this.fpSpread1_Sheet1.SetValue(row, 4, obj.AddressHome, false);
            }

            return 0;
        }

        /// <summary>
        /// ��ȡѡ��Ļ��߲�����
        /// </summary>
        public string SelectedCardNo
        {
            get
            {
                if (this.fpSpread1_Sheet1.RowCount == 0) return "";

                int Row = this.fpSpread1_Sheet1.ActiveRowIndex;

                return this.fpSpread1_Sheet1.GetText(Row, 0);
            }
        }

        private void fpSpread1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader || this.fpSpread1_Sheet1.RowCount == 0) return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}