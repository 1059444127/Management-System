using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Neusoft.HISFC.Components.Order.OutPatient.Controls
{
    public partial class ucUndrugDictionary : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucUndrugDictionary()
        {
            InitializeComponent();
        }

        #region ����
        /// <summary>
        /// ��ҩƷҵ���
        /// </summary>
        protected Neusoft.HISFC.BizProcess.Integrate.Fee undrugManagement = new Neusoft.HISFC.BizProcess.Integrate.Fee();

        private List<Neusoft.HISFC.Models.Fee.Item.Undrug> undrugList = new List<Neusoft.HISFC.Models.Fee.Item.Undrug>();

        private DataSet dsUndrug = new DataSet();
        private DataTable dtUndrug = new DataTable();
        private DataView dvUndrug = new DataView();
        private string filterInput = "";
        private string mainSettingFilePath = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath + @".\UndrugDictionary.xml";
        #endregion

        #region ˽�з���

        /// <summary>
        /// ���ã���
        /// </summary>
        private void InitFrp()
        {
            dsUndrug = new DataSet();
            dtUndrug = new DataTable();
            dvUndrug = new DataView();
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڳ�ʼ��������Ժ�.....");
            if (File.Exists(this.mainSettingFilePath))
            {
                
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.CreatColumnByXML(this.mainSettingFilePath, this.dtUndrug, ref this.dvUndrug, this.fpSpread1_Sheet1);

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpSpread1_Sheet1, this.mainSettingFilePath);
            }
            else
            {
                this.dtUndrug.Columns.AddRange(new DataColumn[] 
                {
                    new DataColumn("��ҩƷ����", typeof(string)),
                    new DataColumn("��ҩƷ����", typeof(string)),
                    new DataColumn("������", typeof(string)),
                    new DataColumn("���ʱ���", typeof(string)),
                    new DataColumn("ϵͳ���", typeof(string)),
                    new DataColumn("��С������", typeof(string)),
                    new DataColumn("ƴ����", typeof(string)),
                    new DataColumn("�����", typeof(string)),
                    new DataColumn("������", typeof(string)),
                    new DataColumn("�Ƽ۵�λ", typeof(string)),
                    new DataColumn("��Ч�Ա�־", typeof(string)),
                    new DataColumn("���", typeof(string)),
                    new DataColumn("ִ�п���", typeof(string)),
                    new DataColumn("Ĭ�ϼ�鲿λ", typeof(string)),
                    new DataColumn("�۸�", typeof(decimal)),
                    new DataColumn("�����", typeof(decimal)),
                    new DataColumn("��ͯ��", typeof(decimal)),
                    new DataColumn("ȷ�ϱ�־", typeof(bool))
                    
                });

                this.dvUndrug = new DataView(this.dtUndrug);

                this.fpSpread1.DataSource = this.dvUndrug;

                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1_Sheet1, this.mainSettingFilePath);
            }
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        /// <summary>
        /// ���ط�ҩƷ��Ϣ
        /// </summary>
        private void QueryUndrug()
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ��ط�ҩƷ��Ϣ�����Ժ�.....");
            Application.DoEvents();
            undrugList = undrugManagement.QueryAllItemsList();
            
            this.dtUndrug.Clear();
            foreach (Neusoft.HISFC.Models.Fee.Item.Undrug undrug in undrugList)
            {
                DataRow row = this.dtUndrug.NewRow();

                row["��ҩƷ����"] = undrug.ID;
                row["��ҩƷ����"] = undrug.Name;
                row["������"] = undrug.GBCode;
                row["���ʱ���"] = undrug.NationCode;
                row["ϵͳ���"] = undrug.SysClass.Name;
                row["��С������"] = undrug.MinFee.Name;
                row["ƴ����"] = undrug.SpellCode;
                row["�����"] = undrug.WBCode;
                row["������"] = undrug.UserCode;
                row["�Ƽ۵�λ"] = undrug.PriceUnit;
                row["��Ч�Ա�־"] = undrug.ValidState;
                row["���"] = undrug.Specs;
                row["ִ�п���"] = undrug.ExecDept;
                row["Ĭ�ϼ�鲿λ"] = undrug.CheckBody;
                row["�۸�"] = undrug.Price;
                row["�����"] = undrug.SpecialPrice;
                row["��ͯ��"] = undrug.ChildPrice;
                row["ȷ�ϱ�־"] = undrug.IsNeedConfirm;

                this.dtUndrug.Rows.Add(row);
            }
            this.dtUndrug.AcceptChanges();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }
                
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            if (!this.DesignMode)
            {
                this.InitFrp();
                this.QueryUndrug();
            }
            return base.OnInit(sender, neuObject, param);
        }

        #endregion
                
        #region �¼�
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string queryCode = Neusoft.FrameWork.Public.String.TakeOffSpecialChar(this.txtFilter.Text);

            queryCode = Neusoft.FrameWork.Public.String.TakeOffSpecialChar(queryCode, "'", "%");
            
            queryCode = queryCode + "%";
            this.filterInput = "((ƴ���� LIKE '" + queryCode + "') OR " +
                "(����� LIKE '" + queryCode + "') OR " +
                "(������ LIKE '" + queryCode + "') OR " +
                "(������ LIKE '" + queryCode + "') )" ;

            this.dvUndrug.RowFilter = filterInput;
        }

        private void linkLblSet_Click(object sender, EventArgs e)
        {
            Neusoft.HISFC.Components.Common.Controls.ucSetColumn uc = new Neusoft.HISFC.Components.Common.Controls.ucSetColumn();
            uc.FilePath = this.mainSettingFilePath;
            uc.SetDataTable(this.mainSettingFilePath, this.fpSpread1_Sheet1);
            Neusoft.FrameWork.WinForms.Classes.Function.PopForm.Text = "��ʾ����";
            Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);
            uc.DisplayEvent += new EventHandler(ucSetColumn_DisplayEvent);
            this.ucSetColumn_DisplayEvent(null, null);
        }

        private void ucSetColumn_DisplayEvent(object sender, EventArgs e)
        {
            this.InitFrp();
            this.QueryUndrug();
        }
        
        #endregion

        

    }
}

