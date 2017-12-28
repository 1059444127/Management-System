using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;
using Neusoft.HISFC.Models.Registration;
using System.Collections;

namespace Neusoft.WinForms.Report.OutpatientFee.Controls
{
    public partial class ucRegBillPrint : UserControl, Neusoft.HISFC.BizProcess.Interface.Registration.IRegPrint
    {
        public ucRegBillPrint()
        {
            InitializeComponent();
        }

        private Neusoft.FrameWork.Management.Transaction trans = new Neusoft.FrameWork.Management.Transaction();
        /// <summary>
        /// ���ô�ӡֵ
        /// </summary>
        /// <param name="register">�Һ�ʵ��</param>
        /// <returns></returns>
        public int SetPrintValue(Neusoft.HISFC.Models.Registration.Register register)
        {
            try
            {
                this.InitReceipt();
                //�����
                this.lblCardNo.Text = register.PID.CardNO;
                //�Һſ���
                this.lblDeptName.Text = register.DoctorInfo.Templet.Dept.Name;
                //�ű�
                this.lblRegLevel.Text = register.DoctorInfo.Templet.RegLevel.Name;
                //����
                this.lblPatientName.Text = register.Name;
                //�Һ�Ա��
                this.lblRegOper.Text = register.InputOper.ID;
                //С��                
                this.lblCostsum.Text = Neusoft.FrameWork.Public.String.FormatNumberReturnString(
                    register.PubCost + register.PayCost + register.OwnCost, 2) +
                    "Ԫ";
                //��д
                this.lblUpperCostSum.Text = Neusoft.FrameWork.Public.String.LowerMoneyToUpper(
                   register.PubCost + register.PayCost + register.OwnCost
                    );
                //�Һ�����
                this.lblRegDate.Text = register.DoctorInfo.SeeDate.ToShortDateString();
                //ҽ�����
                this.lblPactName.Text = register.Pact.Name;
                //�Һŷ�
                this.lblRegFee.Text = Neusoft.FrameWork.Public.String.FormatNumberReturnString(
                    register.RegLvlFee.RegFee, 2) +
                    "Ԫ";
                //����
                this.lblChkFee.Text = Neusoft.FrameWork.Public.String.FormatNumberReturnString(
                    register.RegLvlFee.ChkFee + register.RegLvlFee.PubDigFee + register.RegLvlFee.OwnDigFee, 2) +
                    "Ԫ";
                //�����ֲ� ���ޡ�����
                this.lblCaseBookCost.Text = Neusoft.FrameWork.Public.String.FormatNumberReturnString(
                    0m, 2) +
                    "Ԫ";
                //���Ƹ��ݴ�ӡ��Ԥ����ʾѡ��
                if (IsPreview)
                {
                    SetToPreviewMode();
                }
                else
                {
                    SetToPrintMode();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }
        private bool _isPreview = false;

        private bool IsPreview
        {
            get { return _isPreview; }
            set { _isPreview = value; }
        }

        public int Print()
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.PrintPage(0, 0, this);
            return 0;
        }
        public int Clear()
        {
            return 0;
        }

        public void SetTrans(System.Data.IDbTransaction trans)
        {
            //this.trans.Trans = trans;
        }

        public System.Data.IDbTransaction Trans
        {
            get
            {
                return Neusoft.FrameWork.Management.PublicTrans.Trans;
            }
            set
            {
                //this.trans.Trans = value;
            }
        }
        public int PrintView()
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.PrintPreview(0, 0, this);
            return 0;
        }
        #region �����ӡ�ú���
        /// <summary>
        /// ����Ϊ��ӡģʽ
        /// </summary>
        public void SetToPrintMode()
        {
            //��Ԥ������Ϊ���ɼ�
            SetLableVisible(false, lblPreview);
            foreach (Label lbl in lblPrint)
            {
                lbl.BorderStyle = BorderStyle.None;
                lbl.BackColor = SystemColors.ControlLightLight;
            }
        }
        /// <summary>
        /// ����ΪԤ��ģʽ
        /// </summary>
        public void SetToPreviewMode()
        {
            //��Ԥ������Ϊ�ɼ�
            SetLableVisible(true, lblPreview);
            foreach (Label lbl in lblPrint)
            {
                lbl.BorderStyle = BorderStyle.None;
                lbl.BackColor = SystemColors.ControlLightLight;
            }
        }

        /// <summary>
        /// ��ӡ�õı�ǩ����
        /// </summary>
        public Collection<Label> lblPrint;
        /// <summary>
        /// Ԥ���õı�ǩ����
        /// </summary>
        public Collection<Label> lblPreview;

        /// <summary>
        /// ��ʼ���վ�
        /// </summary>
        /// <remarks>
        /// �Ѵ�ӡ���Ԥ������ݣ�����ǩ��ֵ���ֿ�
        /// </remarks>
        private void InitReceipt(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c.GetType().FullName == "System.Windows.Forms.Label")
                {
                    Label l = (Label)c;
                    if (l.Tag != null)
                    {
                        if (l.Tag.ToString() == "print")
                        {
                            if (!string.IsNullOrEmpty(l.Text) || l.Text == "ӡ")
                            {
                                l.Text = "";
                            }
                            lblPrint.Add(l);
                        }
                        else
                        {
                            lblPreview.Add(l);
                        }
                    }
                    else
                    {
                        lblPreview.Add(l);
                    }
                }
            }
        }


        /// <summary>
        /// ��ʼ���վ�
        /// </summary>
        /// <remarks>
        /// �Ѵ�ӡ���Ԥ������ݣ�����ǩ��ֵ���ֿ�
        /// </remarks>
        private void InitReceipt()
        {

            lblPreview = new Collection<Label>();
            lblPrint = new Collection<Label>();
            foreach (Control c in this.Controls[0].Controls)
            {
                if (c.GetType().FullName == "System.Windows.Forms.Label")
                {
                    Label l = (Label)c;
                    if (l.Tag != null)
                    {
                        if (l.Tag.ToString() == "print")
                        {
                            if (!string.IsNullOrEmpty(l.Text) || l.Text == "ӡ")
                            {
                                l.Text = "";
                            }
                            lblPrint.Add(l);
                        }
                        else
                        {
                            lblPreview.Add(l);
                        }
                    }
                    else
                    {
                        lblPreview.Add(l);
                    }
                }
            }
        }
        /// <summary>
        /// ���ñ�ǩ���ϵĿɼ���
        /// </summary>
        /// <param name="v">�Ƿ�ɼ�</param>
        /// <param name="l">��ǩ����</param>
        private void SetLableVisible(bool v, Collection<Label> l)
        {
            foreach (Label lbl in l)
            {
                lbl.Visible = v;
            }
        }


        /// <summary>
        /// ���ô�ӡ���ϵ�ֵ
        /// </summary>
        /// <param name="t">ֵ����</param>
        /// <param name="l">��ǩ����</param>
        private void SetLableText(string[] t, Collection<Label> l)
        {
            foreach (Label lbl in l)
            {
                lbl.Text = "";
            }
            if (t != null)
            {
                if (t.Length <= l.Count)
                {
                    int i = 0;
                    foreach (string s in t)
                    {
                        l[i].Text = s;
                        i++;
                    }
                }
                else
                {
                    if (t.Length > l.Count)
                    {
                        int i = 0;
                        foreach (Label lbl in l)
                        {
                            lbl.Text = t[i];
                            i++;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
