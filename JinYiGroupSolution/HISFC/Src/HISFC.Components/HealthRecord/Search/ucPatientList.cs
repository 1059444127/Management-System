using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.HISFC.Components.HealthRecord.Search
{
    public partial class ucPatientList : UserControl
    {
        public delegate void ListShowdelegate(Neusoft.HISFC.Models.HealthRecord.Base obj);

        public event ListShowdelegate SelectItem;

        public ucPatientList()
        {
            InitializeComponent();
        }

        #region ȫ�ֱ���
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string strErr = "";
        Neusoft.HISFC.BizLogic.HealthRecord.Base baseMgr = new Neusoft.HISFC.BizLogic.HealthRecord.Base();
        Neusoft.HISFC.Models.HealthRecord.Base baseObj = new Neusoft.HISFC.Models.HealthRecord.Base();
        Neusoft.HISFC.BizProcess.Integrate.RADT radtMgr = new Neusoft.HISFC.BizProcess.Integrate.RADT();
        #endregion 

        public Neusoft.HISFC.Models.HealthRecord.Base CaseBase
        {
            get
            {
                return baseObj;
            }
        }

        #region ö��
        private enum Cols
        {
            outDept, //��Ժ����
            outTime,//��Ժ����
            strName,//����
            sexName,//�Ա�
            inpatientNO,//סԺ��ˮ��
            caseNo,//������
            patientNO,//סԺ��
            times,//�ڼ���
            //Memo,
            //�����޸Ĳ���������{C80E9978-D3E3-4af7-92F3-D91ED5288419}
            birthday,
            address
        }
        #endregion 

        #region ����סԺ�Ų�ѯ ,���ز�ѯ�����������
        /// <summary>
        /// ����סԺ�Ų�ѯ
        /// </summary>
        /// <param name="PatientNO">����</param>
        /// <param name="CardNOType">1,������,2 סԺ��</param>
        /// <returns></returns>
        public ArrayList Init(string PatientNO ,string CardNOType)
        {
            try
            {
                this.fpSpread1_Sheet1.RowCount = 0;
                ArrayList list = null;
                if (CardNOType == "1")
                { 
                    //list = baseMgr.QueryCaseBaseInfoByCaseNO(PatientNO);//����סԺ�Ų�ѯ
                    list = baseMgr.QueryCasInfoByCasNo(PatientNO);
                }
                else if (CardNOType == "2") //����סԺ�Ų�ѯ
                { 
                    //list = baseMgr.QueryPatientInfo(PatientNO);
                    list = baseMgr.QueryCasInfoByPatientNo(PatientNO);
                    //������ͬ����סԺ�ſ��ܲ�ͬ�������ٰ�����������һ��{C80E9978-D3E3-4af7-92F3-D91ED5288419}
                    //if (list != null && list.Count > 0)
                    //{
                    //    list = baseMgr.QueryPatientInfoByName((list[0] as Neusoft.HISFC.Models.HealthRecord.Base).PatientInfo.Name);
                    //}
                }
                //������������һ��{C80E9978-D3E3-4af7-92F3-D91ED5288419}
                else if (CardNOType == "3")
                {
                    list = baseMgr.QueryCasInfoByName(PatientNO);
                }
                if (list == null)
                {
                    this.strErr = baseMgr.Err;
                    return null;
                }
                foreach (Neusoft.HISFC.Models.HealthRecord.Base obj in list)
                {
                    int row = this.fpSpread1_Sheet1.Rows.Count;
                    this.fpSpread1_Sheet1.Rows.Add(row, 1);
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.outDept].Text = obj.OutDept.Name;//��Ժ����
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.outTime].Text = obj.PatientInfo.PVisit.OutTime.ToString("yyyy-MM-dd");//��Ժʱ��
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.strName].Text = obj.PatientInfo.Name;//����
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.sexName].Text = obj.PatientInfo.Sex.Name;//�Ա�
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.inpatientNO].Text = obj.PatientInfo.ID;//סԺ��ˮ��
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.caseNo].Text = obj.CaseNO; //������
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.caseNo].Tag = obj; //������
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.patientNO].Text = obj.PatientInfo.PID.PatientNO;//סԺ��
                    this.fpSpread1_Sheet1.Cells[row,(int)Cols.times].Text = obj.PatientInfo.InTimes.ToString();//��Ժ����
                    if (obj.PatientInfo.User01 == "������Ϣ" || CardNOType =="1" )
                    {
                        this.fpSpread1_Sheet1.Rows[row].BackColor = System.Drawing.Color.LightGreen;
                    }
                    //�ѻ���
                    if (obj.CaseStat == "5")
                    {
                        this.fpSpread1_Sheet1.Rows[row].BackColor = System.Drawing.Color.LightBlue;
                    }
                    if (obj.PatientInfo.PVisit.InState.ID.ToString() != "O")
                    {
                        this.fpSpread1_Sheet1.Rows[row].BackColor = System.Drawing.Color.LightGray;
                    }
                    //�����޸Ĳ���������{C80E9978-D3E3-4af7-92F3-D91ED5288419}
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.birthday].Text = obj.PatientInfo.Birthday.ToString("yyyy-MM-dd");
                    this.fpSpread1_Sheet1.Cells[row, (int)Cols.address].Text = obj.PatientInfo.AddressHome;
                }
                return list;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                return null;
            }
        }
        #endregion 

        #region ˫��ʱѡȡ��Ŀ
        private void fpSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            baseObj = GetCaseInfo();
            if (baseObj != null)
            {
                this.Visible = false;
                SelectItem(baseObj);
            }
        }
        #endregion 

        #region ��ȡ��ǰѡ�����
        public Neusoft.HISFC.Models.HealthRecord.Base GetCaseInfo()
        {
            int Row = this.fpSpread1_Sheet1.ActiveRowIndex;
            if (Row == -1)
            {
                return null;
            }
            //�����޸Ĳ���������{C80E9978-D3E3-4af7-92F3-D91ED5288419}
            if (this.fpSpread1_Sheet1.Rows[Row].BackColor == System.Drawing.Color.LightGray)
            {
                MessageBox.Show("�û���δ��Ժ");
                return null;
            }
            if (this.fpSpread1_Sheet1.Rows[Row].BackColor == System.Drawing.Color.LightGreen)
            {
                //if (MessageBox.Show("�û����Ѵ��ڲ�����¼���Ƿ�����޸ģ�", "��ʾ", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                //{
                //    return null;
                //}
            }

            baseObj = (Neusoft.HISFC.Models.HealthRecord.Base)this.fpSpread1_Sheet1.Cells[Row, (int)Cols.caseNo].Tag;
            return baseObj;
        }
        #endregion 
        #region  ��������
        /// <summary>
        /// ��һ��
        /// </summary>
        public void NextRow()
        {
            if (this.fpSpread1_Sheet1.RowCount == 0)
            {
                return;
            }
            int _Row = fpSpread1_Sheet1.ActiveRowIndex;
            if (_Row < this.fpSpread1_Sheet1.RowCount-1)
            {
                _Row = _Row + 1;
                fpSpread1_Sheet1.ActiveRowIndex = _Row;
                fpSpread1_Sheet1.AddSelection(_Row, 0, 1, 0);
            }
        }
        /// <summary>
        /// ǰһ��
        /// </summary>
        public void PriorRow()
        {
            if (this.fpSpread1_Sheet1.RowCount == 0)
            {
                return;
            }
            int _Row = fpSpread1_Sheet1.ActiveRowIndex;
            if (_Row > 0)
            {
                _Row = _Row - 1;
                fpSpread1_Sheet1.ActiveRowIndex = _Row;
                fpSpread1_Sheet1.AddSelection(_Row, 0, 1, 0);
            }
        }
        #endregion 

        private void ucPatientList_Load(object sender, EventArgs e)
        {
            this.fpSpread1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
        }

    }
}
