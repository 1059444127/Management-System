using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.HISFC.BizProcess.Integrate
{
    public abstract class IntegrateBase
    {
        public void SetDB(Neusoft.FrameWork.Management.Database db)
        {
            this.db = db;
        }
        private Neusoft.FrameWork.Management.Database db = null;
        protected System.Data.IDbTransaction trans = null;

        /// <summary>
        /// �����������һ��manager�Ͳ���Ҫ����д
        /// ����漰���manager��Ϊ�˽����񴫵�������override
        /// Ȼ����������
        /// </summary>
        /// <param name="trans"></param>
        public virtual void SetTrans(System.Data.IDbTransaction trans)
        {
            if(db!=null) this.db.SetTrans(trans);
            this.trans = trans;
        }
        private string err = string.Empty;
        public  string Err
        {
            get
            {
                if (db == null) return err;
                return this.db.Err;
            }
            set
            {
                if (db == null)
                    err = value;
                else
                    this.db.Err = value;

            }
        }
        public  string ErrCode
        {
            get
            {
                if (db == null) return "";
                return this.db.ErrCode;
            }
            set
            {
                this.db.ErrCode = value;
            }
        }
        public  int DBErrCode
        {
            get
            {
                if (db == null) return 0;
                return this.db.DBErrCode;
            }
            set
            {
                this.db.DBErrCode = value;
            }
        }
    }
}
