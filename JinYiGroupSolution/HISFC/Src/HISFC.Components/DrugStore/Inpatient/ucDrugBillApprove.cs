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
    /// <summary>
    /// [�ؼ�����: ucDrugBillApprove]<br></br>
    /// [��������: ��ҩ����׼]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-11-22]<br></br>
    /// <�޸ļ�¼ 
    ///		�޸���='' 
    ///		�޸�ʱ��='yyyy-mm-dd' 
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public partial class ucDrugBillApprove : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucDrugBillApprove( )
        {
            InitializeComponent( );
        }

        public event System.EventHandler SaveFinished;

        #region ����

        // ������
        private Neusoft.FrameWork.Public.ObjectHelper objectHelper = new Neusoft.FrameWork.Public.ObjectHelper( );

        //ҩƷ������
        private Neusoft.HISFC.BizLogic.Pharmacy.Item item = new Neusoft.HISFC.BizLogic.Pharmacy.Item( );

        //��ҩ��ʵ��
        Neusoft.HISFC.Models.Pharmacy.DrugBillClass drugBillClass = new Neusoft.HISFC.Models.Pharmacy.DrugBillClass();

        //�Ƿ�ֻ��ҩ����Ա��ҩʦ��ҩ��ʦ�����
        private bool isPharmaceutistOnly = true;

        /// <summary>
        /// �Ƿ������ֺ�׼
        /// </summary>
        private bool isPartialApprove = false;

        /// <summary>
        /// ��������
        /// </summary>
        private Neusoft.FrameWork.Models.NeuObject operDept = null;

        #endregion

        #region ����

        /// <summary>
        /// �Ƿ�ֻ��ҩʦ��ҩ��ʦ���
        /// </summary>
        [Description( "�Ƿ�ֻ��ҩʦ��ҩ��ʦ���"),Category("����"),DefaultValue(true)]
        public bool IsPharmaceutistOnly
        {
            get
            {
                return isPharmaceutistOnly; 
            }
            set
            {
                isPharmaceutistOnly = value; 
            }
        }

        ///// <summary>
        ///// �Ƿ��һ�ŵ��ݽ��в��ֺ�׼
        ///// </summary>
        //[Description("�Ƿ��һ�ŵ��ݽ��в��ֺ�׼"), Category("����"), DefaultValue(false)]
        //public bool IsPartialApprove 
        //{
        //    get
        //    {
        //        return this.isPartialApprove;
        //    }
        //    set
        //    {
        //        this.isPartialApprove = value;
        //    }
        //}

        /// <summary>
        /// ��������
        /// </summary>
        public Neusoft.FrameWork.Models.NeuObject OperDept
        {
            get
            {
                return this.operDept;
            }
            set
            {
                this.operDept = value;
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ����¼���Ա������ȡԱ������
        /// </summary>
        /// <returns></returns>
        private bool GetOperName( )
        {
            //¼��Ա��������ж�¼���Ƿ���ȷ
            string operCode = this.txtOperCode.Text.PadLeft( 6 , '0' );
            string operName = "";
            this.txtOperCode.Text = operCode;
            operName = this.objectHelper.GetName( operCode );
            if( operName == null )
            {
                MessageBox.Show( Language.Msg( "��Ч��Ա������,������¼��!" ) );
                this.txtOperCode.SelectAll( );
                this.txtOperCode.Focus( );
                return false;
            }
            this.txtOperName.Text = operName;
            return true;
        }

        /// <summary>
        /// ���Ʋ�����ʼ��
        /// </summary>
        private void InitControlParam()
        {
            Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlParamIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

            this.IsPharmaceutistOnly = ctrlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.InDrug_Need_Priv, true, false);
           
            //this.IsPartialApprove = ctrlParamIntegrate.GetControlParam<bool>(Neusoft.HISFC.BizProcess.Integrate.PharmacyConstant.InDrug_Part_Approve, true, false);
        }

        #endregion

        #region ���з���

        /// <summary>
        /// ��Ӱ�ҩ����ӡ��ʾ�ؼ�
        /// </summary>
        /// <param name="ucBill">�ؼ�����</param>
        /// <returns>�ɹ�����1 ʧ�ܷ���-1</returns>
        public int AddDrugBill(object ucBill,bool isAddToContainer)
        {
            return this.ucDrugDetail1.AddDrugBill(ucBill,isAddToContainer);
        }

        /// <summary>
        /// ��������������ȷ�ϰ�ť��״̬
        /// </summary>
        /// <param name="isTrue"></param>
        public void SetEnabled( bool isTrue )
        {
            this.txtOperCode.Enabled = isTrue;
            this.btnOk.Enabled = isTrue;

            //this.ucDrugDetail1.IsShowCheckColumn = false;
            this.ucDrugDetail1.IsAutoCheck = false;
        }

        /// <summary>
        /// ��ҩ����׼
        /// </summary>
        /// <returns></returns>
        public virtual int ApproveDrugBill( )
        {
            //���û��ѡ�а�ҩ��������ʾ
            if( drugBillClass ==null )
            {
                MessageBox.Show( Language.Msg( "��ѡ��Ҫ��׼�İ�ҩ��" ) );
                return 0;
            }
            //����ð�ҩ���Ѻ�׼�������ϣ��򷵻�
            if( this.drugBillClass.ApplyState != "1" )
            {
                return 0;
            }
            if( this.txtOperCode.Text == "" || this.txtOperCode.Text == null )
            {
                MessageBox.Show( Language.Msg( "����������˱�ţ�" ) );
                return -1;
            }

            string deptCode = ((Neusoft.HISFC.Models.Base.Employee)item.Operator).Dept.ID;

            //isPartialApprove �������� ����ѡ�����ݽ��к�׼����
            ArrayList al = this.ucDrugDetail1.GetCheckData();
            if (al.Count <= 0)
            {
                MessageBox.Show(Language.Msg("��ѡ�����׼����"));
                return -1;
            }
            //��ҩ����׼����
            if (Function.DrugApprove(al, this.txtOperCode.Text, deptCode) == -1)
            {
                return -1;
            }

            ////�˴�Ӧ��ʹ�ò���Ա��Ȩ�޿��ң�Ŀǰ��ʹ�����ڿ���
            //if (!this.isPartialApprove)
            //{                
            //    //ȡ��ҩ���е���ϸ����
            //    string billCodes = "'" + drugBillClass.DrugBillNO + "'";
            //    ArrayList al = this.item.QueryApplyOutListByBill(billCodes);
            //    if (al == null)
            //    {
            //        MessageBox.Show(this.item.Err);
            //        return -1;
            //    }
            //    //��ҩ����׼����
            //    if (Function.DrugApprove(al, this.txtOperCode.Text, deptCode) == -1)
            //    {
            //        return -1;
            //    }
            //}
            //else
            //{
                
            //}

            return 1;
        }

        /// <summary>
        /// ��ʾ��ҩ������
        /// </summary>
        /// <param name="billClass"></param>
        public virtual void ShowData( Neusoft.HISFC.Models.Pharmacy.DrugBillClass billClass )
        {
            this.drugBillClass = billClass;
            this.ShowData( billClass.DrugBillNO , billClass.ApplyState == "2" ? false : true );
        }

        /// <summary>
        /// ��ʾ��ҩ����ҩ����
        /// </summary>
        /// <param name="drugBillCodes">��ҩ���ţ��������ϣ����ԡ���������</param>
        /// <param name="isNeedApprove">�Ƿ���Ҫ��׼</param>
        public virtual void ShowData( string drugBillCodes , bool isNeedApprove )
        {         
            //�����ϸ����
            this.Clear( );

            if (drugBillCodes.IndexOf(',') == -1)
            {
                drugBillCodes = "'" + drugBillCodes + "'";
            }

            //���浱ǰ��ҩ�����뼰�Ƿ���Ҫ��׼
            string originalBillNO = this.drugBillClass.DrugBillNO;

            this.drugBillClass.DrugBillNO = drugBillCodes;
            this.drugBillClass.ApplyState = isNeedApprove == true ? "1" : "2";

            #region �����Ƿ���Ա���׼��ǣ����ÿؼ��Ƿ������ʾ

            if ( isNeedApprove )
            {
                this.SetEnabled( true);
                this.txtOperCode.Text = "";
                this.txtOperName.Text = "";
            }
            else
            {
                this.SetEnabled( false );
                this.txtOperCode.Text = drugBillClass.Oper.ID;
                this.txtOperName.Text = this.objectHelper.GetName( drugBillClass.Oper.ID );
            }

            #endregion

            //���û��ѡ�а�ҩ�������������
            if( drugBillCodes == "''" )
            {
                this.Clear( );
                this.SetEnabled( false );
                return;
            }

            #region ȡ��ҩ���е���ϸ���� 

            ArrayList al = this.item.QueryApplyOutListByBill( drugBillCodes );
            if( al == null )
            {
                MessageBox.Show( this.item.Err );
                return;
            }

            ArrayList alState = new ArrayList();
            if (this.isPartialApprove)
            {
                foreach (Neusoft.HISFC.Models.Pharmacy.ApplyOut info in al)
                {
                    if (isNeedApprove && info.State == "2")
                        continue;
                    if (!isNeedApprove && info.State == "1")
                        continue;

                    alState.Add(info);
                }
            }
            else
            {
                alState = al;
            }
            //��ʾ��ϸ����
            this.ucDrugDetail1.ShowData( alState , drugBillClass );
            this.txtOperCode.Focus( );

            #endregion

            //����ԭ��ҩ����
            this.drugBillClass.DrugBillNO = originalBillNO;
        }

        /// <summary>
        /// ��յ�ǰ����
        /// </summary>
        public virtual void Clear( )
        {
            this.ucDrugDetail1.Clear( );
        }

        /// <summary>
        /// ��ӡ
        /// </summary>
        public virtual void Print( )
        {
            #region ���Ӻ�׼�Ĵ�ӡȨ�� wbo 2010-10-11

            Neusoft.HISFC.BizLogic.Manager.Constant consMgr = new Neusoft.HISFC.BizLogic.Manager.Constant();
            Neusoft.FrameWork.Models.NeuObject obj = consMgr.GetConstant("PhaPrintPriv", consMgr.Operator.ID);
            Neusoft.HISFC.Models.Base.Const cons = obj as Neusoft.HISFC.Models.Base.Const;
            if (cons == null || string.IsNullOrEmpty(cons.ID) || cons.IsValid == false)
            {
                MessageBox.Show("�ް�ҩ������Ȩ�ޣ�����ϵҩѧ������Ϣ�ƣ�");
                return;
            }
            if (cons.IsValid == false)
            {
                MessageBox.Show("��ҩ������Ȩ����Ч������ϵҩѧ������Ϣ�ƣ�");
                return;
            }

            #endregion

            if (MessageBox.Show("�Ƿ��ӡ?", "��ʾ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.ucDrugDetail1.Print();
                MessageBox.Show("��ӡ��ϣ�");
            }
        }

         /// <summary>
         /// ��ӡԤ��
         /// </summary>
        public virtual void PrintPreview( )
        {
            this.ucDrugDetail1.Preview( );
        }
        #endregion

        #region �¼�

        /// <summary>
        /// ȷ����׼�¼��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click( object sender , EventArgs e )
        {

            if( this.GetOperName( ) )
            {
                if (this.ApproveDrugBill() == 1)
                {
                    MessageBox.Show(Language.Msg("��׼�ɹ�"));

                    if (this.SaveFinished != null)
                    {
                        this.SaveFinished(null, System.EventArgs.Empty);
                    }
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// ����������س��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOperCode_KeyDown( object sender , KeyEventArgs e )
        {
            if( e.KeyCode == Keys.Enter )
            {
                if( this.GetOperName( ) )
                {
                    this.btnOk.Focus( );
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// �ؼ�װ���¼����ؼ���Ϣ��ʼ��
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e )
        {
            try
            {
                this.InitControlParam();

                this.ucDrugDetail1.IsShowBillPreview = true;

                //ȡ��Ա�����б������û�¼�����Ա�����������
                Neusoft.HISFC.BizProcess.Integrate.Manager manager = new Neusoft.HISFC.BizProcess.Integrate.Manager( );
                //�����������ã��Ƿ�ֻ��ҩ��ʦ���ܺ�׼
                if( this.isPharmaceutistOnly )
                {
                    objectHelper.ArrayObject = manager.QueryEmployee( Neusoft.HISFC.Models.Base.EnumEmployeeType.P );
                }
                else
                {
                    objectHelper.ArrayObject = manager.QueryEmployeeAll( );
                }
                //���ÿؼ���ʼ״̬
                this.SetEnabled( false );
            }
            catch
            {

            }
            base.OnLoad( e );
        }

        #endregion

    }
}
