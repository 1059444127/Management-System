using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.DrugStore.Inpatient
{
    public partial class ucChooseDrugControl : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucChooseDrugControl()
        {
            InitializeComponent();

            this.neuSpread1.ButtonClicked -= new FarPoint.Win.Spread.EditorNotifyEventHandler(neuSpread1_ButtonClicked);
            this.neuSpread1.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(neuSpread1_ButtonClicked);
        }

        public delegate void SelectControlDelegate(Neusoft.HISFC.Models.Pharmacy.DrugControl drugControl);

        public event SelectControlDelegate SelectControlEvent;

        /// <summary>
        /// ��ǰѡ��İ�ҩ̨
        /// </summary>
        private Neusoft.HISFC.Models.Pharmacy.DrugControl drugControl = new Neusoft.HISFC.Models.Pharmacy.DrugControl();

        /// <summary>
        /// ��ǰѡ��Ŀ���
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject selectOperDept = null;

        /// <summary>
        /// �Ƿ���ʾ�����б�
        /// </summary>
        [Description("�Ƿ���ʾ�����б�"),Category("����"),DefaultValue(false)]
        public bool IsShowDept
        {
            get
            {
                return this.panelTree.Visible;
            }
            set
            {
                this.panelTree.Visible = value;
            }
        }

        /// <summary>
        /// ��ǰѡ��Ŀ���
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject SelectOperDept
        {
            get
            {
                return this.selectOperDept;
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public virtual void InitDeptList()
        {
            try
            {
                Neusoft.FrameWork.Management.DataBaseManger dataManager = new Neusoft.FrameWork.Management.DataBaseManger();
                this.ShowControlList(((Neusoft.HISFC.Models.Base.Employee)dataManager.Operator).Dept.ID);

                this.tvDeptTree1.IsShowPI = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("��ʼ����ҩ̨�б�������" + ex.Message);
            }
        }

        #region {50CAFFB7-1E18-4b0d-95D6-CEC019D4C35D} Ȩ�޿��ư�ҩ̨ add by guanyx
        /// <summary>
        /// ���ݵ�½�˵�Ȩ�ޣ����˰�ҩ̨
        /// </summary>
        /// <param name="al"></param>
        /// <returns></returns>
        private ArrayList FliterControl(ArrayList al)
        {
            
            //��ԱȨ�޷�����ϸ����
            Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager privManager = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager();

            string operCode = privManager.Operator.ID;
            string deptCode = ((Neusoft.HISFC.Models.Base.Employee)privManager.Operator).Dept.ID;

            //����ҩ��������
            Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();           

            //ȡ����Ա��ҩ��Ȩ��
            ArrayList alPriv = privManager.LoadByUserCode(operCode, "03", deptCode);
            
            string priv = "";
            for (int i = 0; i < alPriv.Count; i++)
            {
                Neusoft.HISFC.Models.Admin.UserPowerDetail no = alPriv[i] as Neusoft.HISFC.Models.Admin.UserPowerDetail;
                if (no.PowerLevelClass.Class3Code == "Z1")
                {
                    priv += "B";
                }
                if (no.PowerLevelClass.Class3Code == "Z2")
                {
                    priv += "T";
                }
            }
            if (al == null)
            {
                MessageBox.Show(drugStoreManager.Err);
                return al;
            }
            if (al.Count == 0)
            {
                MessageBox.Show(Language.Msg("�����ڵĿ���û�����ð�ҩ̨���������ñ����ҵİ�ҩ̨��"));
                return al;
            }
            Neusoft.HISFC.Models.Pharmacy.DrugControl control;
            Neusoft.HISFC.Models.Pharmacy.DrugControl QuitDrugControl = new Neusoft.HISFC.Models.Pharmacy.DrugControl();
            for (int i = 0; i < al.Count; i++)
            {
                control = al[i] as Neusoft.HISFC.Models.Pharmacy.DrugControl;
                if (control.Name == "��ҩ̨")
                {
                    QuitDrugControl = control;
                }
            }
            if (priv.Length == 1)
            {
                if (priv == "B")
                {
                    al.Remove(QuitDrugControl);
                }
                else
                {
                    al.Clear();
                    al.Add(QuitDrugControl);
                }
                return al;
            }
            else if (priv.Length == 0)
            {
                al.Clear();
                return al;
            }

            //{4DD1822D-1CB6-4561-9EBD-FE14DC4FCBC0} ��ҩ̨���� by guanyx
            ControlCompare controlCompare = new ControlCompare();
            al.Sort(controlCompare);

            return al;
        }

        /// <summary>
        /// {4DD1822D-1CB6-4561-9EBD-FE14DC4FCBC0} by guanyx
        /// ��ҩ̨����
        /// </summary>
        public class ControlCompare : System.Collections.IComparer
        {
            #region Comparer ��Ա

            public int Compare(object x, object y)
            {
                if (((Neusoft.HISFC.Models.Pharmacy.DrugControl)x).ExtendFlag != "" && ((Neusoft.HISFC.Models.Pharmacy.DrugControl)y).ExtendFlag != "")
                {
                    int i = Convert.ToInt32(((Neusoft.HISFC.Models.Pharmacy.DrugControl)x).ExtendFlag);
                    int j = Convert.ToInt32(((Neusoft.HISFC.Models.Pharmacy.DrugControl)y).ExtendFlag);
                    return i - j;
                }
                else
                {
                    return 0;
                }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// ��ʾ������ȫ����ҩ̨�б�
        /// </summary>
        public virtual void ShowControlList(string deptCode)
        {
            //�����ǰ��ʾ�İ�ҩ̨
            this.neuSpread1_Sheet1.Rows.Count = 0;

            //�жϿ��ұ����Ƿ����
            if (deptCode == "")
            {
                MessageBox.Show(Language.Msg("��Ч�İ�ҩ���ң�û�п���ѡ��İ�ҩ̨"));
                return;
            }

            //����ҩ��������
            Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore();           

            //ȡ������ȫ����ҩ̨�б�
            ArrayList al = drugStoreManager.QueryDrugControlList(deptCode);

            //{50CAFFB7-1E18-4b0d-95D6-CEC019D4C35D} Ȩ�޿��ư�ҩ̨ add by guanyx
            al = this.FliterControl(al);        

            if (al == null)
            {
                MessageBox.Show(drugStoreManager.Err);
                return;
            }
            if (al.Count == 0)
            {
                MessageBox.Show(Language.Msg("�����ڵĿ���û�����ð�ҩ̨�����ȼ�鱾���ҵİ�ҩ̨��\n\r������û�в�����ҩ̨��Ȩ�ޣ���������Ȩ��"));
                return;
            }
           
            this.neuSpread1_Sheet1.Rows.Add(0, al.Count);
            Neusoft.HISFC.Models.Pharmacy.DrugControl drugControl;
            for (int i = 0; i < al.Count; i++)
            {
                drugControl = al[i] as Neusoft.HISFC.Models.Pharmacy.DrugControl;

                FarPoint.Win.Spread.CellType.ButtonCellType btnType = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btnType.ButtonColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(225)), ((System.Byte)(243)));
                btnType.Text = drugControl.Name;
                btnType.TextDown = drugControl.Name;
                this.neuSpread1_Sheet1.Cells[i, 0].CellType = btnType;
                this.neuSpread1_Sheet1.Cells[i, 1].Text = drugControl.SendType == 0 ? "ȫ��" : (drugControl.SendType == 1 ? "����" : "��ʱ");
                this.neuSpread1_Sheet1.Cells[i, 2].Text = drugControl.ShowLevel == 0 ? "��ʾ���һ���" : (drugControl.ShowLevel == 1 ? "��ʾ������ϸ" : "��ʾ������ϸ");
                this.neuSpread1_Sheet1.Rows[i].Tag = drugControl;
            }

            if (al.Count == 1)
            {
                this.drugControl = al[0] as Neusoft.HISFC.Models.Pharmacy.DrugControl;

                if (this.SelectControlEvent != null)
                {
                    this.SelectControlEvent(this.drugControl);
                }
                return;
            }
        }

         private void tvDeptTree1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                this.ShowControlList((e.Node.Tag as Neusoft.HISFC.Models.Base.Department).ID);

                this.selectOperDept = e.Node.Tag as Neusoft.HISFC.Models.Base.Department;
            }
        }

        private void neuSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                this.drugControl = this.neuSpread1_Sheet1.Rows[e.Row].Tag as Neusoft.HISFC.Models.Pharmacy.DrugControl;

                if (this.SelectControlEvent != null)
                {
                    this.SelectControlEvent(this.drugControl);
                }
            }
        }

    }
}
