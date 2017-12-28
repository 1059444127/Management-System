using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace Neusoft.HISFC.Components.Order.Controls
{
    /// <summary>
    /// [��������: ҽ���ؼ�]<br></br>
    /// [�� �� ��: wolf]<br></br>
    /// [����ʱ��: 2004-10-12]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��=''
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    internal class Order
    {

        public string LONGSETTINGFILENAME = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath + Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "longordersetting.xml";
        public string SHORTSETTINGFILENAME = Neusoft.FrameWork.WinForms.Classes.Function.CurrentPath + Neusoft.FrameWork.WinForms.Classes.Function.SettingPath + "shortordersetting.xml";

        public DataSet dsAllLong = null;

        #region {1AF0EB93-27A8-462f-9A1E-E1A3ECA54ADE} �洢ҽ���Ĺ�ϣ�����ҽ����ѯ�ٶ�
        private System.Collections.Hashtable htOrder = new System.Collections.Hashtable();

        public System.Collections.Hashtable HtOrder
        {
            get
            {
                return htOrder;
            }
            set
            {
                htOrder = value;
            }
        }

        #endregion

        #region ��ʼ��
        /// <summary>
        /// ����DataSet
        /// </summary>
        /// <param name="dataSet"></param>
        public void  SetDataSet(ref System.Data.DataSet dataSet)
        {
            try
            {
                
                Type dtStr = System.Type.GetType("System.String");
                Type dtDbl = typeof(System.Double);
                Type dtInt = typeof(System.Int32);
                Type dtDecimal = typeof(System.Decimal);
                Type dtBoolean = typeof(System.Boolean);
                Type dtDate = typeof(System.DateTime);

                DataTable table = new DataTable("Table");
                table.Columns.AddRange(new DataColumn[] {
															new DataColumn("!",dtStr),     //0
															new DataColumn("��Ч",dtStr),     //0
															new DataColumn("ҽ������",dtStr),//1
															new DataColumn("ҽ����ˮ��",dtStr),//2
															new DataColumn("ҽ��״̬",dtStr),//�¿�������ˣ�ִ��
															new DataColumn("��Ϻ�",dtStr),//4
															new DataColumn("��ҩ",dtStr),//5
															new DataColumn("ҽ������",dtStr),//6
															new DataColumn("���",dtStr),     //0
															//new DataColumn("����",dtInt),//7
                                                            new DataColumn("����",dtDecimal),//7
															new DataColumn("������λ",dtStr),//8
															new DataColumn("ÿ������",dtDbl),//9
															new DataColumn("��λ",dtStr),//10
															new DataColumn("����",dtStr),//11
															new DataColumn("Ƶ�α���",dtStr),
															new DataColumn("Ƶ������",dtStr),
															new DataColumn("�÷�����",dtStr),
															new DataColumn("�÷�����",dtStr),//15
															new DataColumn("��ʼʱ��",dtStr),
															new DataColumn("ֹͣʱ��",dtStr),//25
                                                            #region {62770BA9-AA59-4550-9020-9ABB323544AA}
                                                            new DataColumn("����ʱ��",dtStr),
                                                            #endregion
															new DataColumn("����ҽ��",dtStr),
															new DataColumn("ִ�п��ұ���",dtStr),
															new DataColumn("ִ�п���",dtStr),
															new DataColumn("�Ӽ�",dtBoolean),
															new DataColumn("��鲿λ",dtStr),//31
															new DataColumn("��������/��鲿λ",dtStr),//32
															new DataColumn("�ۿ���ұ���",dtStr),//33
															new DataColumn("�ۿ����",dtStr),//34
															new DataColumn("��ע",dtStr),//20
															new DataColumn("¼���˱���",dtStr),
															new DataColumn("¼����",dtStr),
															new DataColumn("��������",dtStr),
															#region {62770BA9-AA59-4550-9020-9ABB323544AA}
                                                            //new DataColumn("����ʱ��",dtStr),
                                                            #endregion
															new DataColumn("ֹͣ�˱���",dtStr),
															new DataColumn("ֹͣ��",dtStr),
															new DataColumn("˳���",dtInt)//28
														});


                dataSet.Tables.Add(table);

                return ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ;
            }
        }

        /// <summary>
        /// �ж����ҽ��
        /// </summary>
        /// <param name="fpSpread1"></param>
        /// <returns></returns>
        public int ValidComboOrder(Neusoft.HISFC.BizLogic.Order.Order orderManagement)
        {
            Neusoft.HISFC.Models.Order.Frequency frequency = null;
            Neusoft.FrameWork.Models.NeuObject usage = null;
            Neusoft.FrameWork.Models.NeuObject exeDept = null;
            string sample = "";
            decimal amount = 0;
            int sysclass = -1;
            string sysClassID=string.Empty;
            DateTime dtBegin = new DateTime();
            for (int i = 0; i < fpSpread1.ActiveSheet.Rows.Count; i++)
            {
                if (fpSpread1.ActiveSheet.IsSelected(i, 0))
                {
                    Neusoft.HISFC.Models.Order.Inpatient.Order o = this.GetObjectFromFarPoint(i, fpSpread1.ActiveSheetIndex, orderManagement);

                    if (o.Status != 0)
                    {
                       
                        MessageBox.Show(string .Format("�����������������Ŀ{0}״̬�������޸ģ�������ѡ��",o.Item.Name));                       
                        return -1;
                    }
                    if (frequency == null)
                    {
                        frequency = o.Frequency.Clone();
                        usage = o.Usage.Clone();
                        sysclass = o.Item.SysClass.ID.GetHashCode();
                        sysClassID=o.Item.SysClass.ID.ToString();
                        exeDept = o.ExeDept.Clone();
                        sample = o.Sample.Name;
                        amount = o.Qty;
                        dtBegin = o.BeginTime;
                    }
                    else
                    {
                        o.BeginTime = dtBegin;
                        if (o.Frequency.ID != frequency.ID)
                        {
                            MessageBox.Show("Ƶ�β�ͬ������������ã�");
                            return -1;
                        }
                        //if (o.Item.IsPharmacy)		//ֻ��ҩƷ�ж��÷��Ƿ���ͬ
                        if (o.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)		//ֻ��ҩƷ�ж��÷��Ƿ���ͬ
                        {
                            if (o.Item.SysClass.ID.ToString() != "PCC" && o.Usage.ID != usage.ID)
                            {
                                MessageBox.Show("�÷���ͬ������������ã�");
                                return -1;
                            }
                            #region {B423CB4A-8E22-4aad-B847-76AAC7F9AD74}
                            if (sysClassID == "PCC")
                            {
                                if (o.Item.SysClass.ID.ToString() != sysClassID)
                                {
                                    MessageBox.Show("��ҩ�����Ժ�����ҩƷ����ã�");
                                    return -1;
                                }
                            }
                            else
                            {
                                if (o.Item.SysClass.ID.ToString() == "PCC")
                                {
                                    MessageBox.Show("��ҩ�����Ժ�����ҩƷ����ã�");
                                    return -1;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (o.Item.SysClass.ID.ToString() == "UL")//����
                            {
                                if (o.Qty != amount)
                                {
                                    MessageBox.Show("����������ͬ������������ã�");
                                    return -1;
                                }
                                if (o.Sample.Name != sample)
                                {
                                    MessageBox.Show("����������ͬ������������ã�");
                                    return -1;
                                }
                            }
                        }


                        if (o.ExeDept.ID != exeDept.ID)
                        {
                            MessageBox.Show("ִ�п��Ҳ�ͬ���������ʹ��!", "��ʾ");
                            return -1;
                        }
                    }
                }
               
            }
            return 0;

        }
        public FarPoint.Win.Spread.FpSpread fpSpread1 = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="SheetIndex"></param>
        /// <param name="OrderManagement"></param>
        /// <returns></returns>
        public Neusoft.HISFC.Models.Order.Inpatient.Order GetObjectFromFarPoint(int i, int SheetIndex,Neusoft.HISFC.BizLogic.Order.Order OrderManagement)
        {
            Neusoft.HISFC.Models.Order.Inpatient.Order order = null;
            if (this.fpSpread1.Sheets[SheetIndex].Rows[i].Tag != null)
            {
                order = this.fpSpread1.Sheets[SheetIndex].Rows[i].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
            }
            #region {1AF0EB93-27A8-462f-9A1E-E1A3ECA54ADE} �ٴӹ�ϣ����ȡֵ
            else if (this.htOrder != null && this.htOrder.ContainsKey(this.fpSpread1.Sheets[SheetIndex].Cells[i, iColumns[2]].Text))
            {
                order = this.htOrder[this.fpSpread1.Sheets[SheetIndex].Cells[i, iColumns[2]].Text] as Neusoft.HISFC.Models.Order.Inpatient.Order;
            }
            #endregion
            else
            {
                #region ��ֵ
                order = OrderManagement.QueryOneOrder(this.fpSpread1.Sheets[SheetIndex].Cells[i, iColumns[2]].Text);
                #endregion
            }
            return order;
        }

        #endregion

        #region "��Ӧ"
        public int[] iColumns;
        public int[] iColumnWidth;
        /// <summary>
        /// ����������
        /// </summary>
        public  void SetColumnProperty()
        {
            if (System.IO.File.Exists(LONGSETTINGFILENAME))
            {
                if (iColumnWidth == null || iColumnWidth.Length <= 0)
                {
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
                    Neusoft.FrameWork.WinForms.Classes.CustomerFp.ReadColumnProperty(this.fpSpread1.Sheets[1], SHORTSETTINGFILENAME);

                    iColumnWidth = new int[40];
                    for (int i = 0; i < this.fpSpread1.Sheets[0].Columns.Count; i++)
                    {
                        iColumnWidth[i] = (int)this.fpSpread1.Sheets[0].Columns[i].Width;
                    }
                }
                else
                {
                    for (int i = 0; i < this.fpSpread1.Sheets[0].Columns.Count; i++)
                    {
                        this.fpSpread1.Sheets[0].Columns[i].Width = iColumnWidth[i];
                        this.fpSpread1.Sheets[1].Columns[i].Width = iColumnWidth[i];
                    }
                }
            }
            else
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], LONGSETTINGFILENAME);
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], SHORTSETTINGFILENAME);
            }
        }
        public  void SetColumnWidth()
        {
            this.iColumnWidth = new int[40];
            this.iColumnWidth[0] = 56;
            this.iColumnWidth[1] = 10;
            this.iColumnWidth[2] = 56;
            this.iColumnWidth[3] = 10;
            this.iColumnWidth[4] = 10;
            this.iColumnWidth[5] = 10;
            this.iColumnWidth[6] = 10;
            this.iColumnWidth[7] = 185;
            this.iColumnWidth[8] = 15;
            this.iColumnWidth[9] = 31;
            this.iColumnWidth[10] = 31;
            this.iColumnWidth[11] = 46;
            this.iColumnWidth[12] = 31;
            this.iColumnWidth[13] = 33;
            this.iColumnWidth[14] = 33;
            this.iColumnWidth[15] = 10;
            this.iColumnWidth[16] = 10;
            this.iColumnWidth[17] = 31;
            this.iColumnWidth[18] = 76;//��ʼʱ��
            this.iColumnWidth[19] = 76;//ֹͣʱ��
            this.iColumnWidth[20] = 56;//����ҽ��
            this.iColumnWidth[21] = 10;//ִ�п��ұ���
            this.iColumnWidth[22] = 56;//ִ�п���
            this.iColumnWidth[23] = 19;//�Ӽ�
            this.iColumnWidth[24] = 56;//��鲿λ
            this.iColumnWidth[26] = 56;//��������
            this.iColumnWidth[27] = 10;//�ۿ���ұ���
            this.iColumnWidth[28] = 56;//�ۿ����
            this.iColumnWidth[29] = 56;
            this.iColumnWidth[30] = 56;
            this.iColumnWidth[31] = 56;
            this.iColumnWidth[32] = 56;
            this.iColumnWidth[33] = 56;
            this.iColumnWidth[34] = 56;
            this.iColumnWidth[35] = 56;
            this.iColumnWidth[36] = 56;
            this.iColumnWidth[37] = 10;
            this.iColumnWidth[38] = 10;
            this.iColumnWidth[39] = 10;
        }
        /// <summary>
        /// ͨ���������������
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int GetColumnIndexFromName(string Name)
        {
            for (int i = 0; i < dsAllLong.Tables[0].Columns.Count; i++)
            {
                if (dsAllLong.Tables[0].Columns[i].ColumnName == Name) return i;
            }
            MessageBox.Show("ȱ����" + Name);
            return -1;
        }
        public void ColumnSet()
        {
            iColumns = new int[40];
            iColumns[0] = this.GetColumnIndexFromName("��Ч");     //Type
            iColumns[1] = this.GetColumnIndexFromName("ҽ������");//OrderType
            iColumns[2] = this.GetColumnIndexFromName("ҽ����ˮ��");//ID
            iColumns[3] = this.GetColumnIndexFromName("ҽ��״̬");//�¿�������ˣ�ִ��State
            iColumns[4] = this.GetColumnIndexFromName("��Ϻ�");//4 ComboNo
            iColumns[5] = this.GetColumnIndexFromName("��ҩ");//5 MainDrug
            iColumns[6] = this.GetColumnIndexFromName("ҽ������");//6 Nameer	
            iColumns[7] = this.GetColumnIndexFromName("����");//7	Qty
            iColumns[8] = this.GetColumnIndexFromName("������λ");//8 PackUnit
            iColumns[9] = this.GetColumnIndexFromName("ÿ������");//9 DoseOnce
            iColumns[10] = this.GetColumnIndexFromName("��λ");//10 doseUnit
            iColumns[11] = this.GetColumnIndexFromName("����");//11 Fu
            iColumns[12] = this.GetColumnIndexFromName("Ƶ�α���"); //FrequencyCode
            iColumns[13] = this.GetColumnIndexFromName("Ƶ������"); //FrequecyName
            iColumns[14] = this.GetColumnIndexFromName("�÷�����"); //UsageCode
            iColumns[15] = this.GetColumnIndexFromName("�÷�����");//15
            iColumns[16] = this.GetColumnIndexFromName("��ʼʱ��");
            iColumns[17] = this.GetColumnIndexFromName("ִ�п��ұ���");
            iColumns[18] = this.GetColumnIndexFromName("ִ�п���");
            iColumns[19] = this.GetColumnIndexFromName("�Ӽ�");
            iColumns[20] = this.GetColumnIndexFromName("��ע");//20
            iColumns[21] = this.GetColumnIndexFromName("¼���˱���");
            iColumns[22] = this.GetColumnIndexFromName("¼����");
            iColumns[23] = this.GetColumnIndexFromName("��������");
            iColumns[24] = this.GetColumnIndexFromName("����ʱ��");
            iColumns[25] = this.GetColumnIndexFromName("ֹͣʱ��");//25
            iColumns[26] = this.GetColumnIndexFromName("ֹͣ�˱���");
            iColumns[27] = this.GetColumnIndexFromName("ֹͣ��");
            iColumns[28] = this.GetColumnIndexFromName("˳���");//28
            iColumns[29] = this.GetColumnIndexFromName("����ҽ��");
            iColumns[30] = this.GetColumnIndexFromName("���");
            iColumns[31] = this.GetColumnIndexFromName("��鲿λ");
            iColumns[32] = this.GetColumnIndexFromName("��������/��鲿λ");
            iColumns[33] = this.GetColumnIndexFromName("�ۿ���ұ���");
            iColumns[34] = this.GetColumnIndexFromName("�ۿ����");
            iColumns[35] = this.GetColumnIndexFromName("!");


        }

        public void SetColumnName(int k)
        {
            this.fpSpread1.Sheets[k].Columns.Count = 100;
            int i = 0;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("!");    //0
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��Ч");     //0
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ҽ������");//1
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ҽ����ˮ��");//2
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ҽ��״̬");//�¿�������ˣ�ִ��
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��Ϻ�");//4
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��ҩ");//5
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ҽ������");//6
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��");    //0
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("����");//7
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��λ");//8
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ÿ����");//9
            this.fpSpread1.Sheets[k].Columns[i].CellType = new FarPoint.Win.Spread.CellType.NumberCellType();
            ((FarPoint.Win.Spread.CellType.NumberCellType)this.fpSpread1.Sheets[k].Columns[i].CellType).DecimalPlaces = 3;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��λ");//10
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("����");//11
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("Ƶ��");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("Ƶ������");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("�÷�����");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("�÷�");//15
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��ʼʱ��");
            this.fpSpread1.Sheets[k].Columns[i].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ֹͣʱ��");//25
            this.fpSpread1.Sheets[k].Columns[i].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            i++;
            #region {62770BA9-AA59-4550-9020-9ABB323544AA}
            this.fpSpread1.Sheets[k].Columns[i].Label = ("����ʱ��");
            this.fpSpread1.Sheets[k].Columns[i].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            i++;
            #endregion
            this.fpSpread1.Sheets[k].Columns[i].Label = ("����ҽ��");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ִ�п��ұ���");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ִ�п���");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��鲿λ");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��������/��鲿λ");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("�ۿ���ұ���");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("�ۿ����");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��ע");//20
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("¼���˱���");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("¼����");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("��������");
            i++;
            #region {62770BA9-AA59-4550-9020-9ABB323544AA}
            //this.fpSpread1.Sheets[k].Columns[i].Label = ("����ʱ��");
            //this.fpSpread1.Sheets[k].Columns[i].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
            //i++;
            #endregion
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ֹͣ�˱���");
            this.fpSpread1.Sheets[k].Columns[i].Visible = false;
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("ֹͣ��");
            i++;
            this.fpSpread1.Sheets[k].Columns[i].Label = ("˳���");//28
            i++;
            this.fpSpread1.Sheets[k].Columns.Count = i;
        }

        #endregion

        #region ����
        /// <summary>
        /// �����ʽ
        /// </summary>
        public void SaveGrid()
        {
            try
            {
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[0], this.LONGSETTINGFILENAME);
                Neusoft.FrameWork.WinForms.Classes.CustomerFp.SaveColumnProperty(this.fpSpread1.Sheets[1], this.SHORTSETTINGFILENAME);
                MessageBox.Show("��ʾ��ʽ����ɹ��������µ�¼����Ч��");
            }
            catch { }
        }

        #endregion

        #region ��ʱ
        //#region ȫ��ɾ����ֹͣҽ�� Add By liangjz 2005-08
        ///// <summary>
        ///// ɾ����ǰ�ҽ�������������ˡ�ִ��ҽ��
        ///// </summary>
        ///// <param name="delSheet">��ɾ��ҽ�����0 ���� 1 ����</param>
        //public void DelAllApprove(int delSheet)
        //{
        //    if (this.ValidDel(delSheet, "1", false) == -1) return;
        //    DialogResult r;
        //    r = MessageBox.Show("�Ƿ�ֹͣ���г���ҽ��?  \n *�˲������ܳ�����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        //    if (r == DialogResult.OK)
        //        this.DelAll(delSheet, "1", false);
        //}
        ///// <summary>
        ///// ɾ����ǰ�ҽ�����������ѡ����¿�δ���ҽ��
        ///// </summary>
        //public void DelAllSelect()
        //{
        //    if (this.ValidDel(this.fpSpread1.ActiveSheetIndex, "0", true) == -1) return;
        //    DialogResult r;
        //    r = MessageBox.Show("�Ƿ�ɾ����ǰѡ��ҽ��?  \n *�˲������ܳ�����", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        //    if (r == DialogResult.OK)
        //        this.DelAll(this.fpSpread1.ActiveSheetIndex, "0", true);
        //}
        ///// <summary>
        ///// ȫ��ɾ����ֹͣҽ��
        ///// </summary>
        ///// <param name="orderType">����������־ 0 ���� 1 ���� 2 ȫ��ҽ��</param>
        ///// <param name="delFlag">�Ƿ�ɾ����־ 0 ɾ������δ��� 1 ֹͣ�������������ҽ���������¿�ҽ�� 2 ɾ����ֹͣ�����¿��������ҽ��</param>
        ///// <param name="isNeedSelection">�Ƿ���Ҫѡ����вſ��Բ���</param>
        //public void DelAll(int orderType, string delFlag, bool isNeedSelection)
        //{
        //    DateTime dtEnd = new DateTime();
        //    Neusoft.FrameWork.Models.NeuObject dcReason = new Neusoft.FrameWork.Models.NeuObject();
        //    if (delFlag == "1" || delFlag == "2")
        //    {		//��Ҫѡ��ֹͣʱ��
        //        Forms.frmDCOrder fTest = new Forms.frmDCOrder();
        //        fTest.ShowDialog();
        //        if (fTest.DialogResult != DialogResult.OK)
        //        {
        //            return;
        //        }
        //        dtEnd = fTest.DCDateTime;
        //        dcReason = fTest.DCReason.Clone();
        //    }
        //    switch (orderType)
        //    {
        //        case 0:
        //        case 1:
        //            {
        //                switch (delFlag)
        //                {
        //                    case "0":
        //                    case "1":
        //                        if (this.DelSheet(orderType, delFlag, isNeedSelection, dtEnd, dcReason) == -1) return;
        //                        break;
        //                    case "2":
        //                        if (this.DelSheet(orderType, "0", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                        if (this.DelSheet(orderType, "1", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                        break;
        //                }
        //            }
        //            break;
        //        case 2:
        //            switch (delFlag)
        //            {
        //                case "0":
        //                case "1":
        //                    if (this.DelSheet(0, delFlag, isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    if (this.DelSheet(1, delFlag, isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    break;
        //                case "2":
        //                    if (this.DelSheet(0, "0", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    if (this.DelSheet(1, "0", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    if (this.DelSheet(1, "1", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    if (this.DelSheet(0, "1", isNeedSelection, dtEnd, dcReason) == -1) return;
        //                    break;
        //            }
        //            break;
        //    }

        //}
        ///// <summary>
        ///// �Ƿ�����ֹͣ��ɾ��
        ///// </summary>
        ///// <param name="delSheet">0 ����  1 ����</param>
        ///// <param name="delFlag">0 �¿�δ��� 1 ����˻�ִ��</param>
        ///// <param name="isNeedSelection">�Ƿ���Ҫѡ�����Ч</param>
        ///// <returns>�ɹ�����1 ʧ�ܷ��أ�1</returns>
        //public int ValidDel(int delSheet, string delFlag, bool isNeedSelection)
        //{
        //    Neusoft.HISFC.Models.Order.Inpatient.Order info = new Neusoft.HISFC.Models.Order.Inpatient.Order();
        //    bool isApprove = false;		//��������˻�ִ������
        //    bool isSelect = false;		//����ѡ����

        //    for (int i = 0; i < this.fpSpread1.Sheets[delSheet].Rows.Count; i++)
        //    {
        //        info = this.fpSpread1.Sheets[delSheet].Rows[i].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
        //        if (info.Status == 1 || info.Status == 2)			//��������˻�ִ��
        //            isApprove = true;
        //        if (this.fpSpread1.Sheets[delSheet].IsSelected(i, 0) && info.Status == 0)	//����ѡ����
        //            isSelect = true;
        //        //ȫ���ڵ�ʱ����Ҫ���ж�
        //        if (isApprove && isSelect)
        //            break;
        //    }

        //    if (delFlag == "0" && isNeedSelection && !isSelect)
        //    {
        //        MessageBox.Show("��ѡ���ɾ�����¿�ҽ��");
        //        return -1;
        //    }
        //    if (delFlag == "1" && !isApprove)
        //    {
        //        MessageBox.Show("����������˻�ִ�е���Чҽ��");
        //        return -1;
        //    }
        //    return 1;
        //}
        ///// <summary>
        ///// ���ƶ���������ɾ����ֹͣҽ��
        ///// </summary>
        ///// <param name="delSheet">��ɾ����ֹͣҽ�����SheetIndex</param>
        ///// <param name="delFlag">�Ƿ�ɾ����־ 0 ɾ������δ��� 1 ֹͣ�������������ҽ��</param>
        /////<param name="isNeedSelection">�Ƿ���Ҫѡ����в���Ч</param>
        /////<param name="dtEnd">ҽ��ֹͣʱ��</param>
        /////<param name="dcReason">ҽ��ֹͣԭ��</param>
        ///// <returns>�ɹ�1 ʧ�ܣ�1</returns>
        //private int DelSheet(int delSheet, string delFlag, bool isNeedSelection, DateTime dtEnd, Neusoft.FrameWork.Models.NeuObject dcReason)
        //{
        //    if (this.fpSpread1.Sheets[delSheet].Rows.Count <= 0) return -1;
        //    Neusoft.HISFC.Models.Order.Inpatient.Order info;
        //    bool isDo = false;
        //    if (isNeedSelection && delFlag == "0")
        //    {
        //        for (int i = 0; i < this.fpSpread1.Sheets[delSheet].Rows.Count; i++)
        //        {
        //            //��־����ѡ����
        //            if (this.fpSpread1.Sheets[delSheet].IsSelected(i, 0))
        //            {

        //                this.fpSpread1.Sheets[delSheet].Cells[i, this.myOrderClass.iColumns[0]].Tag = "1";
        //            }
        //        }
        //    }

        //    //��������
        //    Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(this.OrderManagement.Connection);
        //    //Neusoft.HISFC.BizLogic.RADT.InPatient inPatient = new Neusoft.HISFC.BizLogic.RADT.InPatient();
        //    t.BeginTransaction();
        //    this.OrderManagement.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

        //    string nurseStation = "";
        //    int rowCount = this.fpSpread1.Sheets[delSheet].Rows.Count;
        //    for (int i = rowCount - 1; i >= 0; i--)
        //    {
        //        info = this.fpSpread1.Sheets[delSheet].Rows[i].Tag as Neusoft.HISFC.Models.Order.Inpatient.Order;
        //        if (info == null)
        //        {
        //            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //            MessageBox.Show("�����" + (i + 1).ToString() + "�г���\n����ת������");
        //            return -1;
        //        }
        //        nurseStation = info.NurseStation.ID;

        //        if (delFlag == "0")
        //        {				//ɾ������δ���ҽ��
        //            #region ɾ������δ���
        //            if ((string)this.fpSpread1.Sheets[delSheet].Cells[i, this.myOrderClass.iColumns[0]].Tag == "1" && info.Status == 0)
        //            {    //��ѡ����δ���
        //                if (info.ID == "")
        //                {		//�¿�����δ����
        //                    //��Ȼɾ��
        //                    isDo = true;
        //                    this.fpSpread1.Sheets[delSheet].Rows.Remove(i, 1);
        //                }
        //                else
        //                {					//�ѱ���δ���ҽ�� 
        //                    isDo = true;
        //                    int iParm = this.OrderManagement.DeleteOrder(info);
        //                    if (iParm == -1)
        //                    {
        //                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                        MessageBox.Show(this.OrderManagement.Err);
        //                        return -1;
        //                    }
        //                    else
        //                    {
        //                        if (iParm == 0)
        //                        {
        //                            Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                            MessageBox.Show("ҽ��״̬�ѷ����仯 ��ˢ����Ļ");
        //                            return -1;
        //                        }
        //                    }
        //                    //ɾ������
        //                    if (OrderManagement.DeleteOrderSubtbl(info.Combo.ID) == -1)
        //                    {
        //                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                        MessageBox.Show(this.OrderManagement.Err);
        //                        return -1;
        //                    }

        //                    this.fpSpread1.Sheets[delSheet].Rows.Remove(i, 1);
        //                }
        //            }

        //            #endregion
        //        }
        //        else if (delFlag == "1")
        //        {
        //            if (info.Status == 1 || info.Status == 2)
        //            {
        //                info.DCOper.OperTime = dtEnd;
        //                info.DcReason = dcReason.Clone();
        //                info.DCOper.ID = this.OrderManagement.Operator.ID;
        //                info.DCOper.Name = this.OrderManagement.Operator.Name;
        //                info.EndTime = info.DCOper.OperTime;

        //                isDo = true;
        //                #region ֹͣҽ��
        //                //Ԥֹͣʱ���趨
        //                if (dtEnd.Date > this.OrderManagement.GetDateTimeFromSysDateTime().Date)
        //                {
        //                    if (this.OrderManagement.UpdateOrder(info) == -1)
        //                    {
        //                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                        MessageBox.Show(this.OrderManagement.Err);
        //                        return -1;
        //                    }
        //                }
        //                else
        //                {			//ֱ��ֹͣ
        //                    string strReturn = "";
        //                    info.Status = 3;
        //                    if (this.OrderManagement.DcOrder(info, true, out strReturn) == -1)
        //                    {
        //                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                        MessageBox.Show(this.OrderManagement.Err);
        //                        return -1;
        //                    }

        //                    if (strReturn != "")
        //                    {
        //                        Neusoft.FrameWork.Management.PublicTrans.RollBack();;
        //                        MessageBox.Show(strReturn);
        //                        return -1;
        //                    }

        //                    this.fpSpread1.Sheets[delSheet].Rows[i].Tag = info;
        //                    this.fpSpread1.Sheets[delSheet].Cells[i, this.myOrderClass.iColumns[3]].Value = info.Status;
        //                }
        //            }
        //                #endregion
        //        }
        //    }

        //    Neusoft.FrameWork.Management.PublicTrans.Commit();


        //    //��ֹͣҽ������״̬
        //    if (delFlag != "0")
        //    {
        //        for (int i = 0; i < this.fpSpread1.Sheets[delSheet].Rows.Count; i++)
        //        {
        //            this.AddObjectToFarpoint(this.fpSpread1.Sheets[delSheet].Rows[i].Tag, i, delSheet);
        //        }
        //    }
        //    //ɾ��һ�к�ѡ����һ�� 
        //    if (this.fpSpread1.Sheets[delSheet].Rows.Count > 0)
        //    {
        //        this.SelectionChanged();
        //    }
        //    else
        //        this.ucItemSelect1.Clear();

        //    //ֹͣ������ҽ��ʱ������Ϣ������վ
        //    //if (delFlag != "0" && nurseStation != "")
        //    //    Neusoft.Common.Class.Message.SendMessage(this.PatientInfo.Patient.Name + "��ҽ���Ѿ�ȫ��ֹͣ", nurseStation);

        //    //����״̬			
        //    if (isDo)
        //    {


        //        this.RefreshCombo();
        //        this.RefreshOrderState();
        //    }
        //    return 1;
        //}
         //#endregion
        #endregion

    }
}
