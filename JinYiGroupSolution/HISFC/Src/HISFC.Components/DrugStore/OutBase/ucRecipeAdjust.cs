using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;

namespace Neusoft.HISFC.Components.DrugStore.OutBase
{
    /// <summary>
    /// [��������: ���ﴦ������]<br></br>
    /// [�� �� ��: ������]<br></br>
    /// [����ʱ��: 2006-11-30]<br></br>
    /// [˵��:ƽ��������Ҫ�������ѷ��ʹ���Ʒ���������ھ���������Ҫ�����մ���ҩ����Ʒ���������վ��ִ���]
    /// <�޸ļ�¼ 
    ///		�޸���='������' 
    ///		�޸�ʱ��='yyyy-mm-dd' 
    ///		�޸�Ŀ��='���ӵ�������/������ʽ'
    ///		�޸�����=''
    ///  />
    /// <�޸ļ�¼>
    ///    1.ȡ�����·�ʽ���ն˴��������������´���������ʽby Sunjh 2010-12-9 {61D29CAF-7EA1-4949-B9D6-F14C54AD9B2F} 
    /// </�޸ļ�¼>
    /// </summary>
    public partial class ucRecipeAdjust : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucRecipeAdjust( )
        {
            InitializeComponent( );
        }

        #region ����

        /// <summary>
        /// ҵ�������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Pharmacy.DrugStore drugStoreManager = new Neusoft.HISFC.BizLogic.Pharmacy.DrugStore( );

        /// <summary>
        /// ����ԱȨ�޿���
        /// </summary>
        Neusoft.FrameWork.Models.NeuObject privDept = new Neusoft.FrameWork.Models.NeuObject( );

        /// <summary>
        /// �Ƿ���Ȩ�ޱ༭
        /// </summary>
        private bool isPrivilegeEdit = false;

        #endregion

        #region ����

        /// <summary>
        /// �Ƿ�����༭
        /// </summary>
        public bool IsEdit
        {
            get
            {
                return this.isPrivilegeEdit;
            }
            set
            {
                this.isPrivilegeEdit = value;
                this.toolBarService.SetToolButtonEnabled( "����" , value );
            }
        }

        #endregion

        #region ����

        /// <summary>
        /// ��ʼ����ҩ������ʽ 0 ƽ�� 1 ����
        /// </summary>
        public void ShowAdjustType( )
        {
            //Neusoft.HISFC.BizLogic.Manager.Controler controlerManager = new Neusoft.HISFC.BizLogic.Manager.Controler( );
            //string ctrl = controlerManager.QueryControlerInfo( "500006" );
            //if( ctrl == null || ctrl == "-1" || ctrl == "0" )
            //{
            //    this.rbAverage.Checked = true;
            //    this.adjustType = "0";
            //}
            //else
            //{
            //    this.rbCompete.Checked = true;
            //    this.adjustType = "1";
            //}

            Neusoft.FrameWork.Management.ExtendParam extManager = new ExtendParam();
            Neusoft.HISFC.Models.Base.ExtendInfo deptExt = extManager.GetComExtInfo(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT, "TerminalAdjust", this.privDept.ID);
            if (deptExt == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������չ��������ҩ��������ʧ�ܣ�"));
                return;
            }

            if (deptExt.StringProperty == "1")		//����
            {
                this.rbCompete.Checked = true;
            }
            else
            {
                this.rbAverage.Checked = true;
            }

            deptExt = extManager.GetComExtInfo(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT,"AdjustGist", this.privDept.ID);
            if (deptExt == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������չ��������ҩ���������趨ʧ�ܣ�"));
                return;
            }

            if (deptExt.StringProperty == "1")		//��ҩ
            {
                this.rbSend.Checked = true;
            }
            else
            {
                this.rbDrug.Checked = true;
            }
        }

        /// <summary>
        /// ��ʼ����ҩ̨��Ϣ
        /// </summary>
        public void ShowTerminalInfo( )
        {
            //��ѯ�ն˴���״̬���´���������ʽby Sunjh 2010-12-9 {61D29CAF-7EA1-4949-B9D6-F14C54AD9B2F}
            //ArrayList al = this.drugStoreManager.QueryDrugTerminalByDeptCode( this.privDept.ID , Neusoft.FrameWork.Function.NConvert.ToInt32(Neusoft.HISFC.Models.Pharmacy.EnumTerminalType.��ҩ̨).ToString());
            ArrayList al = this.drugStoreManager.QueryDrugTerminalByDeptCodeNew(this.privDept.ID, Neusoft.FrameWork.Function.NConvert.ToInt32(Neusoft.HISFC.Models.Pharmacy.EnumTerminalType.��ҩ̨).ToString());
            
            if( al == null )
            {
                MessageBox.Show( Language.Msg("��ȡ��ҩ̨�б����") + this.drugStoreManager.Err );
                return;
            }

            this.neuSpread1_Sheet1.Rows.Count = al.Count;
            Neusoft.HISFC.Models.Pharmacy.DrugTerminal info = null;
            for( int i = 0 ; i < al.Count ; i++ )
            {
                info = al[ i ] as Neusoft.HISFC.Models.Pharmacy.DrugTerminal;
                //��ҩ̨����
                this.neuSpread1_Sheet1.Cells[ i , 0 ].Text = info.ID;
                //��ҩ̨����
                this.neuSpread1_Sheet1.Cells[ i , 1 ].Text = info.Name;
                //�Ƿ�ر�
                this.neuSpread1_Sheet1.Cells[ i , 2 ].Text = info.IsClose ? "��" : "��";

                if( info.IsClose )
                {
                    this.neuSpread1_Sheet1.Rows[ i ].ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.neuSpread1_Sheet1.Rows[ i ].ForeColor = System.Drawing.SystemColors.WindowText;
                }
                //�ѷ���Ʒ����
                this.neuSpread1_Sheet1.Cells[ i , 3 ].Text = info.SendQty.ToString( );
                //����ҩƷ����
                this.neuSpread1_Sheet1.Cells[ i , 4 ].Text = info.DrugQty.ToString( );
                //���������ľ��ִ���
                this.neuSpread1_Sheet1.Cells[ i , 5 ].Text = info.Average.ToString( );

                this.neuSpread1_Sheet1.Rows[ i ].Tag = info;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void SaveData( )
        {
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            Neusoft.FrameWork.Management.ExtendParam extManager = new ExtendParam();

            //Transaction t = new Transaction( Connection.Instance );
            //t.BeginTransaction( );

            this.drugStoreManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            //extManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            //���µ�����ʽ
            if (!this.SaveAdjustParam(extManager))
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return;
            }
            //������ҩ̨
            if( !this.SaveTerminalParam( ) )
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                return;
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show(Language.Msg( "����ɹ�" ));
        }

        /// <summary>
        /// ������ҩ̨
        /// </summary>
        /// <returns></returns>
        private bool SaveTerminalParam(  )
        {
            Neusoft.HISFC.Models.Pharmacy.DrugTerminal info = null;

            decimal sendQty = 0;
            decimal drugQty = 0;
            decimal averageNum = 0;

            for( int i = 0 ; i < this.neuSpread1_Sheet1.Rows.Count ; i++ )
            {
                info = this.neuSpread1_Sheet1.Rows[ i ].Tag as Neusoft.HISFC.Models.Pharmacy.DrugTerminal;
                //�ѷ���Ʒ����
                sendQty = Neusoft.FrameWork.Function.NConvert.ToDecimal( this.neuSpread1_Sheet1.Cells[ i , 3 ].Text ) - info.SendQty;
                if (sendQty > 999)
                {
                    MessageBox.Show(Language.Msg(info.Name + " �ѷ��������������0��999֮��"));
                    return false;
                }
                //����ҩƷ����
                drugQty = Neusoft.FrameWork.Function.NConvert.ToDecimal( this.neuSpread1_Sheet1.Cells[ i , 4 ].Text ) - info.DrugQty;
                if (drugQty > 999)
                {
                    MessageBox.Show(Language.Msg(info.Name + " ����ҩƷ�����������0��999֮��"));
                    return false;
                }
                //���������ľ��ִ���
                averageNum = Neusoft.FrameWork.Function.NConvert.ToDecimal( this.neuSpread1_Sheet1.Cells[ i , 5 ].Text ) - info.Average;
                if (averageNum > 999)
                {
                    MessageBox.Show(Language.Msg(info.Name + " ���������ľ��ִ����������0��999֮��"));
                    return false;
                }
                //������ҩ̨��Ϣ
                if( this.drugStoreManager.UpdateTerminalAdjustInfo( info.ID , sendQty , drugQty , averageNum ) == -1 )
                {
                    MessageBox.Show(Language.Msg( "���µ�") + ( i + 1 ).ToString( ) + Language.Msg("����ҩ̨��¼ʧ��") + this.drugStoreManager.Err );
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// ���µ�����ʽ
        /// </summary>
        /// <param name="controlerManager"></param>
        /// <returns></returns>
        private bool SaveAdjustParam(Neusoft.FrameWork.Management.ExtendParam extManager)
        {
            #region ���������ʽ

            Neusoft.HISFC.Models.Base.ExtendInfo extAdjustType = extManager.GetComExtInfo(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT,"TerminalAdjust", this.privDept.ID);
            if (extAdjustType == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������չ��������ҩ��������ʧ�ܣ�"));
                return false;
            }            

            if (extAdjustType.Item.ID == "")
            {
                #region �޴˲��� ���β���
                extAdjustType.Item.ID = this.privDept.ID;
                extAdjustType.StringProperty = this.rbAverage.Checked ? "0" : "1"; ;
                extAdjustType.PropertyCode = "TerminalAdjust";
                extAdjustType.PropertyName = "������ҩ�ն˵�����ʽ0ƽ�� 1����";

                if (extManager.SetComExtInfo(extAdjustType) == -1)
                {
                    MessageBox.Show(Language.Msg("���¿�����չ��������ҩ�ն˵�����ʽʧ�ܣ�"));
                    return false;
                }
                #endregion
            }
            else
            {
                extAdjustType.Item.ID = this.privDept.ID;
                extAdjustType.StringProperty = this.rbAverage.Checked ? "0" : "1"; ;
                extAdjustType.PropertyCode = "TerminalAdjust";
                extAdjustType.PropertyName = "������ҩ�ն˵�����ʽ0ƽ�� 1����";

                if (extManager.SetComExtInfo(extAdjustType) == -1)
                {
                    MessageBox.Show(Language.Msg("���¿�����չ��������ҩ�ն˵�����ʽʧ�ܣ�"));
                    return false;
                }
            }

            #endregion

            #region �����������

            Neusoft.HISFC.Models.Base.ExtendInfo extAdjustGist = extManager.GetComExtInfo(Neusoft.HISFC.Models.Base.EnumExtendClass.DEPT,"AdjustGist", this.privDept.ID);
            if (extAdjustGist == null)
            {
                MessageBox.Show(Language.Msg("��ȡ������չ��������ҩ��������ʧ�ܣ�"));
                return false;
            }

            if (extAdjustGist.Item.ID == "")
            {
                #region �޴˲��� ���β���

                extAdjustGist.Item.ID = this.privDept.ID;
                extAdjustGist.StringProperty = this.rbDrug.Checked ? "0" : "1"; ;
                extAdjustGist.PropertyCode = "AdjustGist";
                extAdjustGist.PropertyName = "������ҩ�ն˵������� 1 ��ҩ 0 ��ҩ";

                if (extManager.SetComExtInfo(extAdjustGist) == -1)
                {
                    MessageBox.Show("���¿�����չ��������ҩ�ն˵�������ʧ�ܣ�");
                    return false;
                }
                #endregion
            }
            else
            {
                extAdjustGist.Item.ID = this.privDept.ID;
                extAdjustGist.StringProperty = this.rbDrug.Checked ? "0" : "1"; ;
                extAdjustGist.PropertyCode = "AdjustGist";
                extAdjustGist.PropertyName = "������ҩ�ն˵������� 1 ��ҩ 0 ��ҩ";

                if (extManager.SetComExtInfo(extAdjustGist) == -1)
                {
                    MessageBox.Show("���¿�����չ��������ҩ�ն˵�������ʧ�ܣ�");
                    return false;
                }
            }

            #endregion

            return true;

            #region ����ԭ���淽ʽ

            ////�жϵ�����ʽ�Ƿ�仯��û�б仯����
            //bool isChange = false;
            //if( ( this.adjustType == "0" ) && this.rbCompete.Checked )
            //{
            //    isChange = true;
            //}
            //if( ( this.adjustType == "1" ) && this.rbAverage.Checked )
            //{
            //    isChange = true;
            //}
            //if( isChange )
            //{
            //    Neusoft.HISFC.Models.Base.Controler controler = new Neusoft.HISFC.Models.Base.Controler( );
            //    controler.ID = "500006";
            //    controler.Name = "��ҩ������ʽ 0 ƽ�� 1 ����";
            //    controler.ControlerValue = this.rbAverage.Checked ? "0" : "1";
            //    controler.VisibleFlag = true;
            //    int parm = controlerManager.UpdateControlerInfo( controler );
            //    if( parm == -1 )
            //    {
            //        MessageBox.Show( Language.Msg("������ҩ������ʽ����") + controlerManager.Err );
            //        return false;
            //    }
            //    else if( parm == 0 )
            //    {
            //        if( controlerManager.AddControlerInfo( controler ) == -1 )
            //        {
            //            MessageBox.Show( Language.Msg("������ҩ������ʽ����") + controlerManager.Err );
            //            return false;
            //        }
            //    }

            //}
            //return true;

            #endregion
        }

        #endregion

        #region �¼�

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad( EventArgs e )
        {
            //ȡ����ԱȨ�޿��ң���ʱ�����ڿ��Ҵ��� ��
            this.privDept = ( ( Neusoft.HISFC.Models.Base.Employee )this.drugStoreManager.Operator ).Dept;

            //�ж��Ƿ���ģ��ά��Ȩ��
            Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager( );
            List<Neusoft.FrameWork.Models.NeuObject> alPrivDetail = userManager.QueryUserPrivCollection( this.drugStoreManager.Operator.ID , "0350" , this.privDept.ID );
            if( alPrivDetail != null )
            {
                foreach( Neusoft.FrameWork.Models.NeuObject privInfo in alPrivDetail )
                {
                    //�����ն�ά��Ȩ��
                    if( privInfo.ID == "01" )
                    {
                        this.isPrivilegeEdit = true;
                        break;
                    }
                }
            }
            else
            {
                this.isPrivilegeEdit = true;
            }

            this.IsEdit = this.isPrivilegeEdit;

            this.ShowAdjustType( );
            this.ShowTerminalInfo( );

            //�ı�FarPoint�Ļس��¼���ʹ�س�ת����һ�е�ǰ��
            FarPoint.Win.Spread.InputMap im;
            im = this.neuSpread1.GetInputMap( FarPoint.Win.Spread.InputMapMode.WhenAncestorOfFocused );
            im.Put( new FarPoint.Win.Spread.Keystroke( Keys.Enter , Keys.None ) , FarPoint.Win.Spread.SpreadActions.MoveToNextRow );

            this.neuSpread1.EditModeReplace = true;

            Neusoft.FrameWork.WinForms.Classes.MarkCellType.NumCellType markNumCell = new Neusoft.FrameWork.WinForms.Classes.MarkCellType.NumCellType();
            markNumCell.DecimalPlaces = 0;
            markNumCell.MinimumValue = 0;
            markNumCell.MaximumValue = 9999;

            this.neuSpread1_Sheet1.Columns[3].CellType = markNumCell;
            this.neuSpread1_Sheet1.Columns[4].CellType = markNumCell;
            this.neuSpread1_Sheet1.Columns[5].CellType = markNumCell;

            base.OnLoad( e );
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        protected override int OnSave( object sender , object neuObject )
        {
            this.SaveData( );

            return base.OnSave( sender , neuObject );
        }

        /// <summary>
        /// ƽ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbAverage_CheckedChanged( object sender , EventArgs e )
        {
            if( this.rbAverage.Checked )
            {
                //�ѷ���Ʒ����
                this.neuSpread1_Sheet1.Columns.Get( 3 ).Locked = false;
                //����ҩƷ����
                this.neuSpread1_Sheet1.Columns.Get( 4 ).Locked = true;
                //���������ľ��ִ���
                this.neuSpread1_Sheet1.Columns.Get( 5 ).Locked = true;
            }

        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCompete_CheckedChanged( object sender , EventArgs e )
        {
            if( this.rbCompete.Checked )
            {
                //�ѷ���Ʒ����
                this.neuSpread1_Sheet1.Columns.Get( 3 ).Locked = true;
                //����ҩƷ����
                this.neuSpread1_Sheet1.Columns.Get( 4 ).Locked = false;
                //���������ľ��ִ���
                this.neuSpread1_Sheet1.Columns.Get( 5 ).Locked = false;
            }
        }

        #endregion

        #region ��������Ϣ

        /// <summary>
        /// ���幤��������
        /// </summary>
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService( );

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="NeuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit( object sender , object NeuObject , object param )
        {
            this.toolBarService.AddToolButton( "ˢ��" , "ˢ��" , 0 , true , false , null );
            return this.toolBarService;
        }

        /// <summary>
        /// ��������ť�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ToolStrip_ItemClicked( object sender , ToolStripItemClickedEventArgs e )
        {
            switch( e.ClickedItem.Text )
            {
                case "ˢ��":

                    this.ShowTerminalInfo( );

                    this.ShowAdjustType();

                    break;
            }

        }

        #endregion


    }
}
