using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Neusoft.HISFC.Components.Registration.SelfReg
{ 
    /// <summary>
    /// [��������: �����Һŵ���ѡ�񴰿�]<br></br>
    /// [�� �� ��: ţ��Ԫ]<br></br>
    /// [����ʱ��: 2009-9]<br></br>
    /// <˵��
    ///		��۱��ػ�
    ///  />
    /// </summary>
    public partial class frmSelfHelpSelectPop : Form
    {
        public event EventHandler ChooseItem;
        public frmSelfHelpSelectPop()
        {
            InitializeComponent();
        }

        #region ������

        /// <summary>
        /// ������������
        /// </summary>
        private EnumPopType enumPopType;

        /// <summary>
        /// �ۺϹ���ҵ���
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        
        #endregion

        #region ����
        /// <summary>
        /// ������������
        /// </summary>
        public EnumPopType EnumPopType
        {
            set
            {
                this.enumPopType = value;
                this.ShowInfo();
            }
        }
        #endregion

        #region ����

        /// <summary>
        /// ���ùҺ���Ϣ
        /// </summary>
        /// <returns></returns>
        private int ShowDeptInfo()
        {

            ArrayList alDept = this.managerIntegrate.QueryRegDepartment();
            if (alDept == null)
            {
                MessageBox.Show("��ѯ�Һſ��ҳ���" + this.managerIntegrate.Err);
            }

            this.SetFarpointValue(alDept);
            return 1;
        }


        /// <summary>
        /// ����farpoint
        /// </summary>
        /// <param name="alColections"></param>
        private void SetFarpointValue(ArrayList alColections)
        {
          

            decimal rowCount = Math.Ceiling(Neusoft.FrameWork.Function.NConvert.ToDecimal( alColections.Count / 5)); //�������
 

            this.neuSpread1_Sheet1.RowCount = Neusoft.FrameWork.Function.NConvert.ToInt32(rowCount);
            this.neuSpread1_Sheet1.ColumnCount = 5;

            int j = 0;
            for (int i = 0; i < alColections.Count; i++)
            {
                int k = Neusoft.FrameWork.Function.NConvert.ToInt32(Math.Ceiling(Neusoft.FrameWork.Function.NConvert.ToDecimal(i / 5))) ; //�������

                int mod = 0;

                Math.DivRem(i, 5, out mod);
                

                Neusoft.FrameWork.Models.NeuObject obj = alColections[i] as Neusoft.FrameWork.Models.NeuObject;

                FarPoint.Win.Spread.CellType.ButtonCellType btCell = new FarPoint.Win.Spread.CellType.ButtonCellType();
                btCell.Text = obj.Name + "\n(" + obj.ID + ")";
                this.neuSpread1_Sheet1.Cells[k, mod].CellType = btCell;


                this.neuSpread1_Sheet1.Cells[k, mod].Tag = obj; 
            }
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <returns></returns>
        private int ShowInfo()
        {
            if (this.enumPopType == EnumPopType.Dept)
            {
                this.ShowDeptInfo();
            }
            return 1;
        }
        #endregion

        private void neuSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            Neusoft.FrameWork.Models.NeuObject obj = this.neuSpread1_Sheet1.ActiveCell.Tag as Neusoft.FrameWork.Models.NeuObject;
            if (this.ChooseItem != null)
            {
                this.ChooseItem(obj, e);
                this.DialogResult = DialogResult.OK;
                this.Close();
            } 
        }

    }

    /// <summary>
    /// ����ѡ�Ŵ�������
    /// </summary>
    public enum EnumPopType
    {
        /// <summary>
        /// ����
        /// </summary>
        Dept = 1,

        /// <summary>
        /// �Һż���
        /// </summary>
        RegLevel
    }
}