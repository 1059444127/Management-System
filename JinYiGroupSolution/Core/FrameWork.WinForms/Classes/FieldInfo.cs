using System;
using System.Collections.Generic;
using System.Text;

namespace Neusoft.FrameWork.WinForms.Classes
{
    /// <summary>
    /// [��������: �ֶ���Ϣ]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-11-24]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class FieldInfo
    {
        public string ID;
        public string Name;
        public bool IsPrimaryKey;
        public FieldType DataType;
        public bool Nullable;
        public short Length;
        public string Default;
    }

    /// <summary>
    /// �ֶ�����
    /// </summary>
    public enum FieldType
    {
        Varchar2,
        Number,
        Date,
    }

    /// <summary>
    /// DB2�ֶ�����
    /// </summary>
    public enum DB2FieldType
    {
        SMALLINT,
        INTEGER,
        BIGINT,
        REAL,
        DOUBLE,
        NUMERIC,
        Varchar,
        TimeStamp
    }
}
