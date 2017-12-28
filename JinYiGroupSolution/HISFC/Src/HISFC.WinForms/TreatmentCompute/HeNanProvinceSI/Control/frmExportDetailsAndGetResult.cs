using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using Neusoft.HISFC.Models.RADT;

namespace HeNanProvinceSI.Control
{
    /// <summary>
    /// ʡ���ֹ��ϴ���ϸ wbo 2010-08-03
    /// </summary>
    public partial class frmExportDetailsAndGetResult : Form
    {
        public frmExportDetailsAndGetResult()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ���߻�����Ϣ�ۺ�ʵ��
        /// </summary>
        Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = new Neusoft.HISFC.Models.RADT.PatientInfo();

        /// <summary>
        /// ���תintegrate��
        /// </summary>
        Neusoft.HISFC.BizProcess.Integrate.RADT radtIntegrate = new Neusoft.HISFC.BizProcess.Integrate.RADT();

        /// <summary>
        /// �ӿ�ҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Fee.Interface interfaceMgr = new Neusoft.HISFC.BizLogic.Fee.Interface();

        /// <summary>
        /// ҽ��ҵ���
        /// </summary>
        private LocalManager localManager = new LocalManager();

        /// <summary>
        /// ���߷�����ϸ
        /// </summary>
        ArrayList alItemDetail = new ArrayList();

        /// <summary>
        /// ҽ�������ֵ�
        /// </summary>
        Hashtable htCompare = new Hashtable();

        /// <summary>
        /// ���ڸ�ʽ
        /// </summary>
        protected const string DATE_TIME_FORMAT = "yyyyMMddHHmmss";

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        private static string FILE_PATH = string.Empty;

        /// <summary>
        /// ��չ����KEY
        /// </summary>
        public const string EXTEND_PROPERTY_KEY = "HeNanProvinceSI";

        /// <summary>
        /// ���߻�����Ϣ
        /// </summary>
        private string patientBaseInfo = "������{0}  ���ţ�{1}  סԺ�ţ�{2}  ҽ����ˮ�ţ�{3} ��ͬ��λ��{4}  ���ң�{5}  �ܽ�{6}";

        private void ucQueryInpatientNo_myEvent()
        {
            this.Clear();
            if (this.ucQueryInpatientNo.InpatientNo == null || this.ucQueryInpatientNo.InpatientNo.Trim() == "")
            {
                if (this.ucQueryInpatientNo.Err == "")
                {
                    ucQueryInpatientNo.Err = "�˻��߲���Ժ!";
                }
                Neusoft.FrameWork.WinForms.Classes.Function.Msg(this.ucQueryInpatientNo.Err, 211);
                this.ucQueryInpatientNo.Focus();
                return;
            }
            PatientInfo temp = this.localManager.GetSIPersonInfo(this.patientInfo.ID, "0");
            if (temp == null)
            {
                MessageBox.Show("��ȡ�м������Ϣʧ�ܣ�");
                this.ucQueryInpatientNo.Focus();
                return;
            }
            this.patientInfo.SIMainInfo.RegNo = temp.SIMainInfo.RegNo;

            //��ȡסԺ�Ÿ�ֵ��lbl
            this.patientInfo = this.radtIntegrate.GetPatientInfomation(this.ucQueryInpatientNo.InpatientNo);
            this.lblPatientInfo.Text = string.Format(this.patientBaseInfo, this.patientInfo.Name, this.patientInfo.PVisit.PatientLocation.Bed.ID,
                this.patientInfo.ID.Substring(4), this.patientInfo.SIMainInfo.RegNo, this.patientInfo.Pact.Name, this.patientInfo.PVisit.PatientLocation.Dept.Name,
                this.patientInfo.FT.TotCost.ToString());
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        /// <returns></returns>
        public static int CreateSISetting()
        {
            try
            {
                XmlDocument docXml = new XmlDocument();
                XmlElement root = docXml.CreateElement("root");
                docXml.AppendChild(root);

                XmlElement elem1 = docXml.CreateElement("����Ŀ¼");
                string gxml = Application.StartupPath + "/HeNanProSI";//����Ŀ¼
                if (System.IO.Directory.Exists(gxml) == false)
                {
                    System.IO.Directory.CreateDirectory(gxml);
                }
                elem1.SetAttribute("path", Application.StartupPath + "/HeNanProSI");
                root.AppendChild(elem1);

                docXml.Save(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/" + EXTEND_PROPERTY_KEY + "SiSetting.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("д��������Ϣ����!" + ex.Message);
                return -1;
            }
            return 1;
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// </summary>
        private void ReadSISetting()
        {
            if (!System.IO.File.Exists(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/" + EXTEND_PROPERTY_KEY + "SiSetting.xml"))
            {
                if (CreateSISetting() == -1)
                {
                    return;
                }
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "/" + EXTEND_PROPERTY_KEY + "SiSetting.xml");
                XmlNode node = doc.SelectSingleNode("//����Ŀ¼");
                FILE_PATH = node.Attributes["path"].Value.ToString();
                if (string.IsNullOrEmpty(FILE_PATH.Trim()))
                {
                    MessageBox.Show("���������ļ���ά������Ŀ¼");
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡ������Ϣ����!" + e.Message);
                return;
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        private void Clear()
        {
            this.lblPatientInfo.Text = "�����뻼��סԺ�Żس���";
            this.neuSpread1.ActiveSheetIndex = 0;
            this.neuSpread1.Sheets[1].Reset();
            this.neuSpread1.Sheets[2].Reset();
        }

        /// <summary>
        /// ��ʾ������ϸ
        /// </summary>
        private void ShowFeeDetailsToFP()
        {
            if (this.patientInfo == null)
            {
                return;
            }

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("��������������ϸ�����Ժ�...");
            Application.DoEvents();

            #region ��ѯ��ϸ
            //���ҷ�ҩƷ��ϸ
            ArrayList alItemDetailTemp = this.localManager.QueryFeeItemLists(this.patientInfo.ID, this.patientInfo.Pact.ID, new DateTime(1900, 1, 1), 
                new DateTime(2100, 1, 1), "NO");

            //����ҩƷ��ϸ
            ArrayList alDrugDetailTemp = this.localManager.QueryMedItemLists(this.patientInfo.ID, this.patientInfo.Pact.ID, new DateTime(1900, 1, 1),
                new DateTime(2100, 1, 1), "NO");

            alItemDetail = new ArrayList();
            if (alItemDetailTemp != null && alItemDetailTemp.Count > 0)
            {
                alItemDetail.AddRange(alItemDetailTemp);
            }
            if (alDrugDetailTemp != null && alDrugDetailTemp.Count > 0)
            {
                alItemDetail.AddRange(alDrugDetailTemp);
            }
            if (alItemDetail.Count == 0)
            {
                MessageBox.Show("�޷�����Ϣ��");
                return;
            }
            #endregion

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾ������ϸ�����Ժ�...");

            #region ��ѯ��ϸ��Ӧ��ҽ����Ϣ ����ʾ������

            this.neuSpread1.Sheets[1].Reset();
            this.neuSpread1.Sheets[1].RowCount = alItemDetail.Count + 1;
            this.neuSpread1.Sheets[1].Columns.Count = 18;
            int rowCount = 0;
            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList item in alItemDetail)
            {
                if (htCompare.Contains(item.Compare.CenterItem.ID))
                {
                    item.Compare = htCompare[item.Compare.CenterItem.ID] as Neusoft.HISFC.Models.SIInterface.Compare;
                }
                else
                {
                    Neusoft.HISFC.Models.SIInterface.Compare com = new Neusoft.HISFC.Models.SIInterface.Compare();
                    this.interfaceMgr.GetCompareSingleItem(this.patientInfo.Pact.ID, item.Item.ID, ref com);
                    if (string.IsNullOrEmpty(item.Compare.CenterItem.ID) == true)
                    {
                        //û�ж�����Ŀ�Ĵ���
                    }
                    else
                    {
                        //�Ѿ����յ���Ŀ
                        item.Compare = com.Clone();
                        htCompare.Add(item.Compare.CenterItem.ID, item.Compare);
                    }
                }
                this.neuSpread1.Sheets[1].Cells[rowCount++, 0].Value = this.patientInfo.IDCard;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 1].Value = this.patientInfo.SIMainInfo.RegNo;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 2].Value = rowCount.ToString();
                this.neuSpread1.Sheets[1].Cells[rowCount++, 3].Value = item.Compare.CenterItem.ID;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 4].Value = item.Compare.CenterItem.Name;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 5].Value = item.Item.Name;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 6].Value = item.Compare.CenterItem.SysClass;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 7].Value = item.Compare.CenterItem.Specs;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 8].Value = item.Compare.CenterItem.DoseCode;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 9].Value = item.Item.Price.ToString("0.00");
                this.neuSpread1.Sheets[1].Cells[rowCount++, 10].Value = item.Item.Qty.ToString("0.00");
                decimal sum = item.Item.Price * item.Item.Qty;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 11].Value = sum.ToString("0.00");
                this.neuSpread1.Sheets[1].Cells[rowCount++, 12].Value = item.Compare.CenterItem.Rate;
                decimal ownFee = sum * item.Compare.CenterItem.Rate;
                this.neuSpread1.Sheets[1].Cells[rowCount++, 13].Value = ownFee.ToString("0.00");
                this.neuSpread1.Sheets[1].Cells[rowCount++, 14].Value = item.RecipeNO + item.SequenceNO.ToString().PadLeft(2, '0');
                this.neuSpread1.Sheets[1].Cells[rowCount++, 15].Value = item.FeeOper.OperTime.ToString("yyyy.MM.dd");
                this.neuSpread1.Sheets[1].Cells[rowCount++, 16].Value = "";
                this.neuSpread1.Sheets[1].Cells[rowCount++, 17].Value = item.FeeOper.OperTime.ToString("yyyyMMdd");

                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm(rowCount, alItemDetail.Count);
                Application.DoEvents();

            }

            this.neuSpread1.ActiveSheetIndex = 1;

            #endregion

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("������ʾ���ܣ����Ժ�...");
            Application.DoEvents();

            #region ��ʾ����

            this.neuSpread1.Sheets[1].Cells[alItemDetail.Count, 0].Value = "���ܣ�";
            this.neuSpread1.Sheets[1].Cells[alItemDetail.Count, 11].Formula = "sum(L1:L" + alItemDetail.Count.ToString() + ")";
            this.neuSpread1.Sheets[1].Cells[alItemDetail.Count, 13].Formula = "sum(N1:N" + alItemDetail.Count.ToString() + ")";

            #endregion

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        private void neuButton1_Click(object sender, EventArgs e)
        {
            this.ShowFeeDetailsToFP();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.patientInfo == null)
            {
                MessageBox.Show("�����뻼��סԺ�Żس���");
                return;
            }
            if (this.alItemDetail == null || this.alItemDetail.Count == 0)
            {
                MessageBox.Show("����û����Ҫ�ϴ�����ϸ��");
                return;
            }
            string errTxt = "";
            int result = 0;

            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("�����ϴ���ϸ�����Ժ�...");
            Application.DoEvents();

            result = Functions.ExportInpatientFeedetail(FILE_PATH, this.patientInfo.SIMainInfo.RegNo, this.patientInfo, this.alItemDetail, ref errTxt);

            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();

            if (result != 1)
            {
                MessageBox.Show("�ϴ���ϸʧ�ܣ�" + errTxt);
                return;
            }
            MessageBox.Show("�ϴ��ɹ���\r\n��һ������ʡҽ�����������㣬�ٵ�HIS�����������Ժ���㣡");
        }

        private void btnGetResult_Click(object sender, EventArgs e)
        {
            
        }
    }
}