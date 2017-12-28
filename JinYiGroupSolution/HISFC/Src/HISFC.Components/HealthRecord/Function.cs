using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.IO;
namespace Neusoft.HISFC.Components.HealthRecord
{
    class Function
    {
        #region ͨ��XML�����ͱ���DataTable����Ϣ

        /// <summary>
        /// ���ݱ����XML��Ϣ,��������Ϣ
        /// </summary>
        /// <param name="pathName">XML�ļ��洢λ��</param>
        /// <param name="table">Ҫ��ʼ����DataTable</param>
        /// <param name="dv">DataTable��DataView</param>
        /// <param name="sv">��DataView��FarPointSheet</param>
        public static void CreatColumnByXML(string pathName, DataTable table, ref DataView dv, FarPoint.Win.Spread.SheetView sv)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                StreamReader sr = new StreamReader(pathName, System.Text.Encoding.Default);
                string cleandown = sr.ReadToEnd();
                doc.LoadXml(cleandown);
                sr.Close();
            }
            catch
            {
                return;
            }

            XmlNodeList nodes = doc.SelectNodes("//Column");

            string tempString = "";

            foreach (XmlNode node in nodes)
            {
                if (node.Attributes["type"].Value == "TextCellType")
                {
                    tempString = "System.String";
                }
                else if (node.Attributes["type"].Value == "CheckBoxCellType")
                {
                    tempString = "System.Boolean";
                }
                else if (node.Attributes["type"].Value == "NumberCellType")
                {
                    tempString = "System.Decimal";
                }
                else if (node.Attributes["type"].Value == "DateTimeCellType")
                {
                    tempString = "System.DateTime";
                }
                else
                {
                    tempString = "System.String";
                }

                table.Columns.Add(new DataColumn(node.Attributes["displayname"].Value, Type.GetType(tempString)));
            }

            dv = new DataView(table);

            sv.DataSource = dv;
        }

        #endregion

        /// <summary>
        /// ��ӡ������ҳ
        /// </summary>
        /// <param name="info"></param>
        /// <returns>0����  ��-1 ����</returns>
        public static int PrintCaseFirstPage(Neusoft.HISFC.Models.RADT.PatientInfo info)
        {
            HealthRecord.ucCasePrint casePrint = new HealthRecord.ucCasePrint();
            casePrint.LoadInfo();
            Neusoft.HISFC.BizLogic.HealthRecord.Base baseDml = new Neusoft.HISFC.BizLogic.HealthRecord.Base();
            Neusoft.HISFC.BizProcess.Integrate.RADT RadtInpatient = new Neusoft.HISFC.BizProcess.Integrate.RADT();
            Neusoft.HISFC.Models.HealthRecord.Base caseBase = new Neusoft.HISFC.Models.HealthRecord.Base();
            //�ж��Ƿ��иû���
            if (info.ID == null || info.ID == "")
            {
                MessageBox.Show("סԺ��ˮ�Ų���Ϊ��");
                return -1;
            }
            //��ȡסԺ�Ÿ�ֵ��ʵ��
            Neusoft.HISFC.Models.RADT.PatientInfo patientInfo = RadtInpatient.GetPatientInfoByPatientNO(info.ID);
            if (patientInfo == null)
            {
                MessageBox.Show("��ȡ��Ա��Ϣ����");
                return -1;
            }
            caseBase.PatientInfo = patientInfo;
            //casePrint.contro = caseBase;
            //��ȡĬ�ϴ�ӡ��
            string errStr = "";
            ArrayList alSetting = Neusoft.FrameWork.WinForms.Classes.Function.GetDefaultValue("BAPrinter", out errStr);
            if (alSetting == null)
            {
                MessageBox.Show(errStr);
                return -1;
            }
            if (alSetting.Count == 0)
            {
                MessageBox.Show("����д��ӡ�����������ļ�");
                Neusoft.FrameWork.WinForms.Classes.Function.SaveDefaultValue("BAPrinter");
                return -1;
            }
            string printerSetting = alSetting[0] as string;
            Neusoft.FrameWork.WinForms.Classes.Print p = new Neusoft.FrameWork.WinForms.Classes.Print();

            for (int i = 0; i < System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count; i++)
            {
                if (System.Drawing.Printing.PrinterSettings.InstalledPrinters[i].IndexOf(printerSetting) != -1)
                    p.PrintDocument.PrinterSettings.PrinterName = System.Drawing.Printing.PrinterSettings.InstalledPrinters[i];
            }

            p.IsPrintInputBox = true;
            Common.Classes.Function.GetPageSize("case1", ref p);
            p.PrintPage(20, 80, casePrint);
            return 0;
        }
        /// <summary>
        /// ��ӡ������ҳ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int PrintCaseFPByFrm(Neusoft.HISFC.Models.RADT.PatientInfo info, object frmTag)
        {

            //			System.Windows.Forms.Form frmPrint = new Form();
            //			frmPrint.Size = new System.Drawing.Size(825,1070);
            //			casePrint.Dock= System.Windows.Forms.DockStyle.Fill;
            //			frmPrint.AutoScale = false;
            //			frmPrint.Controls.Add(casePrint);
            //			frmPrint.ShowDialog();
            //try
            //{
            //    HealthRecord.frmPrintCasePage frm = new frmPrintCasePage();
            //    frm.Tag = frmTag;
            //    frm.Show();
            //    //frm.Visible=false;
            //    return frm.Print(info);
            //}
            //catch
            //{
            return 0;
            //}


        }
        /// <summary>
        /// ��ӡ������ҳ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int PrintCaseFPByFrm(Neusoft.HISFC.Models.RADT.PatientInfo info)
        {

            //			System.Windows.Forms.Form frmPrint = new Form();
            //			frmPrint.Size = new System.Drawing.Size(825,1070);
            //			casePrint.Dock= System.Windows.Forms.DockStyle.Fill;
            //			frmPrint.AutoScale = false;
            //			frmPrint.Controls.Add(casePrint);
            //			frmPrint.ShowDialog();
            //try
            //{
            //    Case.frmPrintCasePage frm = new frmPrintCasePage();
            //    frm.Show();
            //    //frm.Visible=false;
            //    return frm.Print(info);
            //}
            //catch
            //{
            return 0;
            //}


        }
    }
}
