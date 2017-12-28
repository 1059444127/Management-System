using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Manager.Controls
{
    internal delegate void InsertSuccessedHandler();
    /// <summary>
    /// ����ؼ��Ǹ�(���ҳ�����Ŀά��ʱ�õ�,�ڳ����ⲿû��,����internal
    /// </summary>
    internal partial class ucDeptItem : UserControl
    {
        private Neusoft.HISFC.BizLogic.Manager.DeptItem diBusiness = new Neusoft.HISFC.BizLogic.Manager.DeptItem();
        public event InsertSuccessedHandler InsertSuccessed;

        /// <summary>
        /// ��ŵ�λ��ʶ
        /// </summary>
        private string UnitFlag = "";

        public ucDeptItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ���ⲿ����,���ݲ�����ʼ������
        /// </summary>
        /// <param name="deptItem">�Ƴ�����Ŀ����</param>
        public void ShowWindow(Neusoft.HISFC.Models.Base.DeptItem deptItem)
        {
            this.tbItemCode.Text = deptItem.ItemProperty.ID;
            this.tbItemName.Text = deptItem.ItemProperty.Name;

            this.UnitFlag = deptItem.UnitFlag;

            //this.ckbUnitFlag.Checked = deptItem.UnitFlag.Trim().Equals("��ϸ") ? true : false;
            this.tbBookLocate.Text = deptItem.BookLocate;

            if (deptItem.BookTime == "" || deptItem.BookTime == null)
            {
                this.dtBookTime.Value = DateTime.Now;
            }
            else
            {
                this.dtBookTime.Value = Neusoft.FrameWork.Function.NConvert.ToDateTime(deptItem.BookTime);
            }

            this.tbExecLocate.Text = deptItem.ExecuteLocate;

            if (deptItem.ReportDate == "" || deptItem.ReportDate == null)
            {
                this.dtReportTime.Value = DateTime.Now;
            }
            else
            {
                this.dtReportTime.Value = Neusoft.FrameWork.Function.NConvert.ToDateTime(deptItem.ReportDate);
            }
            
            this.ckbHurtFlag.Checked = deptItem.HurtFlag.Trim().Equals("��") ? true : false;
            this.ckbSelfBookFlag.Checked = deptItem.SelfBookFlag.Trim().Equals("��") ? true : false;
            this.ckbReasonableFlag.Checked = deptItem.ReasonableFlag.Trim().Equals("��Ҫ") ? true : false;
            this.ckbStat.Checked = deptItem.IsStat.Trim().Equals("��Ҫ") ? true : false;
            this.ckbAutoBook.Checked = deptItem.IsAutoBook.Trim().Equals("��Ҫ") ? true : false;
            this.tbSpeciality.Text = deptItem.Speciality;
            this.tbMeaning.Text = deptItem.ClinicMeaning;
            this.tbSampleKind.Text = deptItem.SampleKind;
            this.tbSampleWay.Text = deptItem.SampleWay;
            this.tbSampleUnit.Text = deptItem.SampleUnit;
            this.ntSampleQty.Text = deptItem.SampleQty.ToString();
            this.tbContainer.Text = deptItem.SampleContainer;
            this.tbScope.Text = deptItem.Scope;
            this.tbItemTime.Text = deptItem.ItemTime;
            this.tbMemo.Text = deptItem.Memo;
            //
            this.tbCustomName.Text = deptItem.CustomName;
        }
        /// <summary>
        /// У�� 
        /// by niuxy
        /// </summary>
        /// <returns></returns>
        private int valid()
        {
            //��������
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbSampleWay.Text, 200) == false)
            {
                MessageBox.Show("���������������֧��100������");
                return -1;
            }
            //�ٴ�����
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbMeaning.Text, 200) == false)
            {
                MessageBox.Show("�ٴ�����������֧��100������");
                return -1;
            }

            //�걾
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbSampleKind.Text,200) == false)
            {
                MessageBox.Show("�걾�������֧��100������");
                return -1;
            }
            //����רҵ
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbSpeciality.Text, 20) == false)
            {
                MessageBox.Show("����רҵ�ֶι������֧��10������");
                return -1;
            }

            //�걾��λ
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbSampleUnit.Text, 20) == false)
            {
                MessageBox.Show("�걾��λ�������֧��10������");
                return -1;
            }

            //�걾����
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbContainer.Text, 200) == false)
            {
                MessageBox.Show("�걾�����������֧��100������");
                return -1;
            }
            //������Χ
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbScope.Text, 200) == false)
            {
                MessageBox.Show("������Χ�������֧��100������");
                return -1;
            }
            //ע������
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbMemo.Text, 4000) == false)
            {
                MessageBox.Show("ע������������֧��2000������");
                return -1;
            }
            //������Χ
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbScope.Text, 200) == false)
            {
                MessageBox.Show("������Χ�������֧��100������");
                return -1;
            }
            //��������
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbCustomName.Text, 100) == false)
            {
                MessageBox.Show("�������ƹ������֧��50������");
                return -1;
            }
            //ԤԼ�ص�
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbBookLocate.Text, 100) == false)
            {
                MessageBox.Show("ԤԼ�ص�������֧��50������");
                return -1;
            }
            
            //ִ�еص�
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbBookLocate.Text, 100) == false)
            {
                MessageBox.Show("ִ�еص�������֧��50������");
                return -1;
            }
            

            //ִ�еص�
            if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbItemTime.Text, 100) == false)
            {
                MessageBox.Show("��Ŀִ������Ҫʱ�䣬�֧��25λ");
                return -1;
            }
            return 0;
        }
        private Neusoft.HISFC.Models.Base.DeptItem SaveButtonHandler()
        {
            if (this.valid() == -1)
            {
                return null;
            }
            Neusoft.HISFC.Models.Base.DeptItem deptItem = new Neusoft.HISFC.Models.Base.DeptItem();

            deptItem.Dept.ID = this.Tag.ToString();//�������Ͼ����Ǹ��»��Ǳ���,���ұ��

            deptItem.ItemProperty.ID = this.tbItemCode.Text;
            deptItem.ItemProperty.Name = this.tbItemName.Text;

            deptItem.UnitFlag = this.UnitFlag;

            //deptItem.UnitFlag = this.ckbUnitFlag.Checked ? "1" : "2";
            deptItem.BookLocate = this.tbBookLocate.Text;

            deptItem.BookTime = this.dtBookTime.Value.ToString();

            deptItem.ExecuteLocate = this.tbExecLocate.Text;

            deptItem.ReportDate = this.dtReportTime.Value.ToString();

            deptItem.HurtFlag = this.ckbHurtFlag.Checked ? "0" : "1";
            deptItem.SelfBookFlag = this.ckbSelfBookFlag.Checked ? "0" : "1";
            deptItem.ReasonableFlag = this.ckbReasonableFlag.Checked ? "0" : "1";
            deptItem.IsStat = this.ckbStat.Checked ? "0" : "1";
            deptItem.IsAutoBook = this.ckbAutoBook.Checked ? "0" : "1";
            //if (Neusoft.FrameWork.Public.String.ValidMaxLengh(this.tbSpeciality.Text, 20))
            //{
                deptItem.Speciality = this.tbSpeciality.Text;
            //}
            //else
            //{
            //    MessageBox.Show("����רҵ�ֶι���", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.tbSpeciality.Focus();
            //    return null;
            //}
            
            deptItem.ClinicMeaning = this.tbMeaning.Text;
            deptItem.SampleKind = this.tbSampleKind.Text;
            deptItem.SampleWay = this.tbSampleWay.Text;
            deptItem.SampleUnit = this.tbSampleUnit.Text;

            deptItem.SampleQty = Convert.ToDecimal(this.ntSampleQty.NumericValue);

            deptItem.SampleContainer = this.tbContainer.Text;
            deptItem.Scope = this.tbScope.Text;
            deptItem.ItemTime = this.tbItemTime.Text;
            deptItem.Memo = this.tbMemo.Text;
            //
            deptItem.CustomName = this.tbCustomName.Text;

            return deptItem;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Models.Base.DeptItem deptItem = this.SaveButtonHandler();

            if (deptItem == null)
            {
                return;
            }

            if (deptItem.Dept.ID == "" || deptItem.Dept.Name == null)
            {

                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                //t.BeginTransaction();

                this.diBusiness.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                if (this.diBusiness.InsertItem(deptItem) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��������ʧ��"));
                    return;
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();;
                if (this.InsertSuccessed != null)
                {
                    InsertSuccessed();
                }
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�������ݳɹ�"));
                this.FindForm().Close();
            }
            else
            {
                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
                //t.BeginTransaction();

                this.diBusiness.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                if (this.diBusiness.UpdateItem(deptItem) == -1)
                {
                    Neusoft.FrameWork.Management.PublicTrans.RollBack();;
                    MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("��������ʧ��"));
                    return;
                }
                Neusoft.FrameWork.Management.PublicTrans.Commit();;
                if (this.InsertSuccessed != null)
                {
                    InsertSuccessed();
                }
                MessageBox.Show(Neusoft.FrameWork.Management.Language.Msg("�������ݳɹ�"));
                this.FindForm().Close();
            }
            //���²���
        }
    }
}
