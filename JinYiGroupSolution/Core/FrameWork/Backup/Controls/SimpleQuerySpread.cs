using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using Neusoft.FrameWork.WinForms.Forms;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// NeuSpread<br></br>
    /// [��������: �򵥲�ѯ���ؼ�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-01]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    [ToolboxBitmap(typeof(FarPoint.Win.Spread.FpSpread))]
    public partial class SimpleQuerySpread : Controls.NeuSpread, IMaintenanceControlable
    {
        private SimpleQuerySpread()
        {
            InitializeComponent();
        }

        private SimpleQuerySpread(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public SimpleQuerySpread(string sql)
            :this()
        {
            this.sql = sql;
        }


        #region Field

        private string sql;
        private IMaintenanceForm queryForm;
        
        #endregion
        

        #region IMaintenanceControlable ��Ա

        public IMaintenanceForm QueryForm
        {
            get
            {
                if (this.queryForm == null)
                {
                    throw new ApplicationException("δ����QueryForm!");
                }
                return this.queryForm;
            }
            set
            {
                this.queryForm = value;

                this.QueryForm.ShowAddButton = false;
                this.QueryForm.ShowSaveButton = false;
            }
        }

        public int Query()
        {
            return 0;
        }

        public int Add()
        {
            return 0;
        }

        public int Delete()
        {
            return 0;
        }

        public int Save()
        {
            return 0;
        }

        //public int Export()
        //{
        //    return 0;
        //}

        public int Print()
        {
            return 0;
        }

        public bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public int Init()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Modify()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Import()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int PrintPreview()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int PrintConfig()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Cut()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Copy()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Paste()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int NextRow()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int PreRow()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
