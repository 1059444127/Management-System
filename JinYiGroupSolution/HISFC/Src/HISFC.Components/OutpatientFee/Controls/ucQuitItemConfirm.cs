using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.HISFC.Models.Fee;
using System.Collections;

namespace Neusoft.HISFC.Components.OutpatientFee.Controls
{
    public partial class ucQuitItemConfirm : ucQuitItemApply, Neusoft.FrameWork.WinForms.Forms.IInterfaceContainer
    {
        public ucQuitItemConfirm()
        {
            InitializeComponent();
            Filter();
        }

        /// <summary>
        /// �˷ѵ���ӡ�ӿ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Interface.FeeInterface.IBackFeeRecipePrint IBackFeePrint = null;

        #region ����

        /// <summary>
        /// ������Fp���ص�{E24AFB64-593E-4001-AB56-3560DEB89A37}
        /// </summary>
        private void Filter() {
            //this.panel2.Visible = false;
            //this.panel1.Location = new System.Drawing.Point(0, 0);
            //this.fpSpread2.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <returns></returns>
        protected override int Save()
        {
            int infoCounts = 0;

            foreach (FarPoint.Win.Spread.SheetView sv in this.fpSpread2.Sheets)
            {
                for (int i = 0; i < sv.RowCount; i++)
                {
                    if (sv.Rows[i].Tag is ReturnApply)
                    {
                        infoCounts++;
                    }
                }
            }

            if (infoCounts == 0) 
            {
                MessageBox.Show("û����Ҫ��˵ķ���!");

                return -1;
            }

            DateTime nowTime = this.outpatientManager.GetDateTimeFromSysDateTime();

            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

            this.outpatientManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.pharmacyIntegrate.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            this.returnApplyManager.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

            int returnValue = 0;
            ArrayList alBackFeeList = new ArrayList();

            foreach (FarPoint.Win.Spread.SheetView sv in this.fpSpread2.Sheets)
            {
                for (int i = 0; i < sv.RowCount; i++)
                {
                    if (sv.Rows[i].Tag is ReturnApply)
                    {
                        ReturnApply tempInsert = sv.Rows[i].Tag as ReturnApply;

                        ReturnApply tempExist = this.returnApplyManager.GetReturnApplyByApplySequence(tempInsert.Patient.ID, tempInsert.ID);
                        //�ҵ��Ѿ��������ݿ���˷�������Ϣ
                        if (tempExist != null)
                        {
                            //if (tempExist.CancelType != Neusoft.HISFC.Models.Base.CancelTypes.Valid)
                            //{
                            //    Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            //    MessageBox.Show(tempExist.Item.Name + "�Ѿ���ȷ�ϻ�������,��ˢ��");

                            //    return -1;
                            //}
                            if (tempExist.IsConfirmed)
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show(tempExist.Item.Name + "�Ѿ���ȷ�ϻ�������,��ˢ��");

                                return -1;
                            }
                        }

                        returnValue = this.returnApplyManager.DeleteReturnApply(tempInsert.ID);
                        if (returnValue == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(tempExist.Item.Name + "ɾ��ʧ��!" + this.returnApplyManager.Err);

                            return -1;
                        }

                        tempInsert.ID = this.returnApplyManager.GetReturnApplySequence();
                        tempInsert.IsConfirmed = false;//��true��Ϊfalse,ҩƷ��˺�Ϊ δȷ�� δ�˷� ״̬ modified by xizf 20110301
                        tempInsert.CancelType = Neusoft.HISFC.Models.Base.CancelTypes.Canceled;

                        returnValue = this.returnApplyManager.InsertReturnApply(tempInsert);

                        if (returnValue == -1)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(tempInsert.Item.Name + "���ʧ��!" + this.returnApplyManager.Err);

                            return -1;
                        }

                        Neusoft.HISFC.Models.Fee.Outpatient.FeeItemList feeItemList = this.outpatientManager.GetFeeItemListBalanced(tempInsert.RecipeNO, tempInsert.SequenceNO);
                        if(feeItemList == null)
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show(tempInsert.Item.Name + "�����Ŀʧ��!" + this.outpatientManager.Err);

                            return -1;
                        }

                        if (feeItemList.Item.Qty < feeItemList.NoBackQty + tempInsert.Item.Qty) 
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("�����Ĳ���Ա�����Ѿ������" + feeItemList.Item.Name + "��ˢ��!");

                            return -1;
                        }

                        //���¿���������ȷ������
                        returnValue = this.outpatientManager.UpdateConfirmFlag(tempInsert.RecipeNO, tempInsert.SequenceNO, "1", feeItemList.ConfirmOper.ID, feeItemList.ConfirmOper.Dept.ID, feeItemList.ConfirmOper.OperTime, feeItemList.NoBackQty + tempInsert.Item.Qty,
                            feeItemList.ConfirmedQty - tempInsert.Item.Qty);
                        if (returnValue <= 0) 
                        {
                            Neusoft.FrameWork.Management.PublicTrans.RollBack();
                            MessageBox.Show("������Ŀ:" + feeItemList.Item.Name + "ʧ��!" + this.outpatientManager.Err);

                            return -1;
                        }

                        //if (tempInsert.Item.IsPharmacy) 
                        if (tempInsert.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug) 
                        {
                            feeItemList.Item.Qty = tempInsert.Item.Qty;
                            
                            returnValue = this.pharmacyIntegrate.OutputReturn(feeItemList, this.outpatientManager.Operator.ID, nowTime);
                            if (returnValue < 0) 
                            {
                                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                                MessageBox.Show("ҩƷ�˿�ʧ��!" + this.pharmacyIntegrate.Err);

                                return -1;
                            }

                            alBackFeeList.Add(feeItemList);

                        }
                    }
                }
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            MessageBox.Show("��˳ɹ�!");

            base.GetItemList();

            if (alBackFeeList.Count > 0)
            {
                if (this.IBackFeePrint == null)
                {
                    this.IBackFeePrint = Neusoft.FrameWork.WinForms.Classes.UtilInterface.CreateObject(this.GetType(), typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IBackFeeRecipePrint)) as Neusoft.HISFC.BizProcess.Interface.FeeInterface.IBackFeeRecipePrint;
                }

                if (this.IBackFeePrint != null)
                {
                    this.IBackFeePrint.Patient = this.patient;

                    this.IBackFeePrint.SetData(alBackFeeList);

                    //this.IBackFeePrint.Print();
                }
            }
            return 1;
        }

        #endregion

        #region �¼�

        #region {6D62088D-7F21-4043-A80B-FBA10379B9BC} �����˷���˶������� by guanyx
        /// <summary>
        /// �����¼�
        /// </summary>
        private event System.EventHandler ReadCardEvent;
        #endregion

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            base.tbQuitCost.Visible = false;
            base.tbReturnCost.Visible = false;
            base.tbQuitCash.Visible = false;
            base.lbLeftCost.Visible = false;
            base.lbQuitCash.Visible = false;
            base.lbReturnCost.Visible = false;

            this.fpSpread1_Sheet1.Columns[(int)DrugList.Cost].Visible = false;
            this.fpSpread1_Sheet2.Columns[(int)UndrugList.Cost].Visible = false;
            
            this.FindForm().Text = "�˷����";

            toolBarService.AddToolButton("�˷����", "���������Ϣ", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.B����, true, false, null);
            toolBarService.AddToolButton("ˢ��", "����ˢ����Ŀ���˷�������Ϣ", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.B����, true, false, null);
            toolBarService.AddToolButton("���", "���¼����Ϣ", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Q���, true, false, null);
            toolBarService.AddToolButton("ȫ��", "ȫ���˳����з���", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȫ��, true, false, null);
            #region {6D62088D-7F21-4043-A80B-FBA10379B9BC} �����˷���˶������� by guanyx
            ReadCardEvent += new EventHandler(ucQuitItemConfirm_ReadCardEvent);
            toolBarService.AddToolButton("����", "��Ժ�ڿ�", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.C������Ա, true, false, this.ReadCardEvent);
            #endregion
            return toolBarService;
        }
        #region {6D62088D-7F21-4043-A80B-FBA10379B9BC} �����˷���˶������� by guanyx
        private string cardno = "";
        private bool isNewCard = false;
        ZZlocal.Clinic.HISFC.OuterConnector.ICCard.ICReader icreader = new ZZlocal.Clinic.HISFC.OuterConnector.ICCard.ICReader();
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucQuitItemConfirm_ReadCardEvent(object sender, EventArgs e)
        {
            if (icreader.GetConnect())
            {
                cardno = icreader.ReaderICCard();
                if (cardno == "0000000000")
                {
                    isNewCard = true;
                    MessageBox.Show("�ÿ�δд�뿨�ţ����ֹ����뻼�߿��Ų��á��س�����ȡ������Ϣ��");
                }
                else
                {
                    this.tbCardNo.Text = cardno;
                    this.tbCardNo_KeyDown(this.tbCardNo, new KeyEventArgs(Keys.Enter));
                }
                icreader.CloseConnection();
            }
            else
            {
                MessageBox.Show("����ʧ�ܣ�");
            }
        }
        #endregion
        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "�˷����":
                    this.Save();
                    break;
                case "ˢ��":
                    base.GetItemList();
                    break;
                case "���":
                    base.Clear();
                    break;
            }

            base.ToolStrip_ItemClicked(sender, e);
        }

        protected override void fpSpread2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            return;
        }

        #endregion

        #region IInterfaceContainer ��Ա

        public Type[] InterfaceTypes
        {
            get
            {
                Type[] printType = new Type[1];
                printType[0] = typeof(Neusoft.HISFC.BizProcess.Interface.FeeInterface.IBackFeeRecipePrint);

                return printType;
            }
        }

        #endregion

    }
}
