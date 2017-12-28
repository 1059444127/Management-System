using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace HeNanProvinceSI
{
    /// <summary>
    /// [��������: ҽ����̬�⺯��������]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2006-10-12]<br></br>
    /// �޸ļ�¼
    /// �޸���='ţ��Ԫ'
    ///	�޸�ʱ��=''
    ///	�޸�Ŀ��='�ḻҽ����Ϣ'
    ///	�޸�����=''
    ///  >
    /// </summary>
    public class Functions
    {
        /// <summary>
        /// �������߷�����Ϣ
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="tablename">������ͬ�ļ���</param>
        /// <param name="idCard">��ݺ���</param>
        /// <param name="regNO">ҽ������ˮ��</param>
        /// <param name="alFeeDetail">������Ϣ</param>
        /// <param name="errTxt">������Ϣ</param>
        /// <returns>1�ɹ� -1ʧ��</returns>
        public static int ExportFeedetails(string path, string tablename, string idCard, string regNO, 
            ArrayList alFeeDetail, ref string errTxt)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (tablename.Substring(0, 1).ToUpper() != "Y")
            {
                tablename = "Y" + tablename;
            }

            string connect = @"Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277; Dbq=" + path;
            System.Data.Odbc.OdbcConnection myconn = new System.Data.Odbc.OdbcConnection(connect);

            string drop = "drop table " + tablename;
            string create = "create table " + tablename +
                        @"(GMSFHM CHAR(20) , ZYH CHAR(14) , XMXH NUMERIC , XMBH CHAR(20) , XMMC CHAR(50) , FLDM CHAR(10),
                        YPGG CHAR(30),YPJX CHAR(10), JG NUMERIC , MCYL NUMERIC , JE NUMERIC , ZFBL NUMERIC ,
                        ZFJE NUMERIC , BZ1 CHAR(20) , BZ2 CHAR(20) , BZ3 CHAR(20), FYRQ CHAR(20))";
            System.Data.Odbc.OdbcCommand cmDrop = new System.Data.Odbc.OdbcCommand(drop, myconn);
            System.Data.Odbc.OdbcCommand cmCreate = new System.Data.Odbc.OdbcCommand(create, myconn);
            System.Data.Odbc.OdbcTransaction trans = null;
            myconn.Open();
            try
            {
                cmDrop.ExecuteNonQuery();
            }
            catch (Exception e)
            { }
            try
            {
                cmCreate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errTxt = "�����ļ�����" + ex.Message;
                return -1;
            }
            trans = myconn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            System.Data.Odbc.OdbcCommand cmInsert = new System.Data.Odbc.OdbcCommand();
            cmInsert.Connection = myconn;
            cmInsert.Transaction = trans;
            int i = 1;

            foreach (Neusoft.HISFC.Models.Fee.Inpatient.FeeItemList f in alFeeDetail)
            {
                //������ݺ���	סԺ��	��Ŀ���	��Ŀ���	��Ŀ����	�������	�淶	ҩƷ����	�۸�	
                //ÿ������	���	�Էѱ���	�Էѽ��	������	��������	��־3	��������
                string insert = "insert into " + tablename +
                @"(GMSFHM, ZYH, XMXH, XMBH , XMMC , FLDM ,YPGG ,YPJX ,JG , MCYL, JE, ZFBL,ZFJE, BZ1, BZ2, BZ3, FYRQ
                )
                values
                (
                  '{0}','{1}',{2},'{3}', '{4}', '{5}','{6}','{7}',{8},{9},{10},{11},{12},'{13}','{14}','{15}','{16}'
                )";
                try
                {
                    insert = string.Format(insert, idCard, regNO, i.ToString(), f.Compare.CenterItem.ID, f.Compare.CenterItem.Name, 
                        f.Compare.CenterItem.SysClass, f.Compare.CenterItem.Specs, f.Compare.CenterItem.DoseCode, f.Item.Price,
                        f.Item.Qty, f.Item.Price * f.Item.Qty, f.Compare.CenterItem.Rate, f.Item.Price * f.Item.Qty * f.Compare.CenterItem.Rate,
                        f.RecipeNO+f.SequenceNO.ToString().PadLeft(2, '0'), f.FeeOper.OperTime.ToString("yyyy.MM.dd"), "", f.FeeOper.OperTime.ToString("yyyyMMdd"));
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errTxt = "�����ļ�����" + ex.Message;
                    return 0;
                }
                i++;
                cmInsert.CommandText = insert;
                try
                {
                    cmInsert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errTxt = "�����ļ�����" + ex.Message;
                    return -1;
                }

            }
            trans.Commit();
            cmInsert.Dispose();
            cmCreate.Dispose();
            cmDrop.Dispose();
            myconn.Close();
            try
            {
                string file = path + "\\" + tablename + ".dbf";
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                if (!System.IO.Directory.Exists(path + "\\Backup"))
                {
                    System.IO.Directory.CreateDirectory(path + "\\Backup");
                }
                fileInfo.CopyTo(path + "\\Backup\\" + tablename + ".dbf");
            }
            catch { }
            return 1;
        }

        /// <summary>
        /// �������߷�����Ϣ
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="tablename">������ͬ�ļ���</param>
        /// <param name="p">������Ϣ</param>
        /// <param name="alFeeDetail">������Ϣ</param>
        /// <param name="errTxt">������Ϣ</param>
        /// <returns>1�ɹ� -1ʧ��</returns>
        public static int ExportInpatientFeedetail(string path, string tablename, Neusoft.HISFC.Models.RADT.PatientInfo p, 
            ArrayList alFeeDetail, ref string errTxt)
        {
            return ExportFeedetails(path, tablename, p.IDCard, p.SIMainInfo.RegNo, alFeeDetail, ref errTxt);
        }

        /// <summary>
        /// �������߷�����Ϣ
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="tablename">������ͬ�ļ���</param>
        /// <param name="r">������Ϣ</param>
        /// <param name="alFeeDetail">������Ϣ</param>
        /// <param name="errTxt">������Ϣ</param>
        /// <returns>1�ɹ� -1ʧ��</returns>
        public static int ExportOutpatientFeedetail(string path, string tablename, Neusoft.HISFC.Models.Registration.Register r, 
            ArrayList alFeeDetail, ref string errTxt)
        {
            return ExportFeedetails(path, tablename, r.IDCard, r.SIMainInfo.RegNo, alFeeDetail, ref errTxt);
        }

        /// <summary>
        /// ��ȡҽ��������
        /// </summary>
        /// <returns></returns>
        public static int GetSiResult(string path, string tablename, ref Neusoft.HISFC.Models.RADT.PatientInfo p, ref string errTxt)
        {
            if (tablename.Substring(0, 1).ToUpper() != "S")
            {
                tablename = "S" + tablename;
            }
            string connect = @"Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277; Dbq=" + path;
            System.Data.Odbc.OdbcConnection myconn = new System.Data.Odbc.OdbcConnection(connect);
            //������ݺ���	סԺ��	��Ժ�ܽ��	�籣֧�����	�����Էѽ��		���Էѽ��		������	�������Ը�	ͳ����˽��	�����˽��	����Ա���˽��	�ʻ�֧�����	�ֽ�֧�����	ҽ�������ܶ�	ҽ���ʻ����	�����¼��
            //GMSFHM	ZYH	ZYZJE	SBZFJE	GRZFJE	ZFYY	CZFJE	BFZFJE	QFJE	ABLZF	TCJZJE	DEJZJE	GWYJZJE	ZHZFJE	XJZFJE	YBJZZE	YBZHYE	JZJLH
            string select = "select * from " + tablename;
            System.Data.Odbc.OdbcCommand cmSelect = new System.Data.Odbc.OdbcCommand(select, myconn);
            System.Data.Odbc.OdbcDataReader cmReader;
            try
            {
                myconn.Open();
                cmReader = cmSelect.ExecuteReader();
            }
            catch (Exception ex)
            {
                errTxt = "����ҽ����Ϣʧ�ܣ�" + ex.Message;
                return -1;
            }
            if (!cmReader.Read())
            {
                errTxt = "ҽ���������ݲ����ڣ�";
                return -2;
            }
            try
            {
                p.IDCard = cmReader["GMSFHM"].ToString();//������ݺ���
                p.SIMainInfo.RegNo = cmReader["ZYH"].ToString();//סԺ��
                p.SIMainInfo.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["ZYZJE"]);//סԺ�ܽ��
                p.SIMainInfo.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["XJZFJE"]);//�ֽ�֧��
                p.SIMainInfo.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["ZHZFJE"]);//�˻�֧��
                p.SIMainInfo.PubCost = p.SIMainInfo.TotCost - p.SIMainInfo.OwnCost - p.SIMainInfo.PayCost;//ͳ��֧��
                p.SIMainInfo.OverCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["DEJZJE"]);//�����˽��
                p.SIMainInfo.BaseCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["QFJE"]);//�𸶽��
                p.SIMainInfo.IndividualBalance = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["YBZHYE"]);//ҽ���˻����

                cmReader.Close();
                cmSelect.Dispose();
                myconn.Close();
            }
            catch (Exception e)
            {
                errTxt = e.ToString();
                return -1;
            }
            try
            {
                string file = path + "\\" + tablename + ".dbf";
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                if (!System.IO.Directory.Exists(path + "\\Backup"))
                {
                    System.IO.Directory.CreateDirectory(path + "\\Backup");
                }
                fileInfo.CopyTo(path + "\\Backup\\" + tablename + ".dbf");
            }
            catch { }

            return 1;
        }

        /// <summary>
        /// ��ȡҽ��������
        /// </summary>
        /// <returns></returns>
        public static int GetSiResult(string path, string tablename, ref Neusoft.HISFC.Models.Registration.Register r, ref string errTxt)
        {
            string connect = @"Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277; Dbq=" + path;
            System.Data.Odbc.OdbcConnection myconn = new System.Data.Odbc.OdbcConnection(connect);
            //������ݺ���	סԺ��	��Ժ�ܽ��	�籣֧�����	�����Էѽ��		���Էѽ��		������	�������Ը�	ͳ����˽��	�����˽��	����Ա���˽��	�ʻ�֧�����	�ֽ�֧�����	ҽ�������ܶ�	ҽ���ʻ����	�����¼��
            //GMSFHM	ZYH	ZYZJE	SBZFJE	GRZFJE	ZFYY	CZFJE	BFZFJE	QFJE	ABLZF	TCJZJE	DEJZJE	GWYJZJE	ZHZFJE	XJZFJE	YBJZZE	YBZHYE	JZJLH
            if (tablename.Substring(0, 1).ToUpper() != "S")
            {
                tablename = "S" + tablename;
            }
            string select = "select * from " + tablename;
            System.Data.Odbc.OdbcCommand cmSelect = new System.Data.Odbc.OdbcCommand(select, myconn);
            System.Data.Odbc.OdbcDataReader cmReader;
            try
            {
                myconn.Open();
                cmReader = cmSelect.ExecuteReader();
            }
            catch (Exception ex)
            {
                errTxt = "����ҽ����Ϣʧ�ܣ�" + ex.Message;
                return -1;
            }
            if (!cmReader.Read())
            {
                errTxt = "ҽ���������ݲ����ڣ�";
                return -2;
            }
            try
            {
                r.IDCard = cmReader["GMSFHM"].ToString();//������ݺ���
                r.SIMainInfo.RegNo = cmReader["ZYH"].ToString();//סԺ��
                r.SIMainInfo.TotCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["ZYZJE"]);//סԺ�ܽ��
                r.SIMainInfo.OwnCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["XJZFJE"]);//�ֽ�֧��
                r.SIMainInfo.PayCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["ZHZFJE"]);//�˻�֧��
                r.SIMainInfo.PubCost = r.SIMainInfo.TotCost - r.SIMainInfo.OwnCost - r.SIMainInfo.PayCost;//ͳ��֧��
                r.SIMainInfo.OverCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["DEJZJE"]);//�����˽��
                r.SIMainInfo.BaseCost = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["QFJE"]);//�𸶽��
                r.SIMainInfo.IndividualBalance = Neusoft.FrameWork.Function.NConvert.ToDecimal(cmReader["YBZHYE"]);//ҽ���˻����

                cmReader.Close();
                cmSelect.Dispose();
                myconn.Close();
            }
            catch (Exception e)
            {
                errTxt = e.ToString();
                return -1;
            }
            try
            {
                string file = path + "\\" + tablename + ".dbf";
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                if (!System.IO.Directory.Exists(path + "\\Backup"))
                {
                    System.IO.Directory.CreateDirectory(path + "\\Backup");
                }
                fileInfo.CopyTo(path + "\\Backup\\" + tablename + ".dbf");
            }
            catch { }

            return 1;
        }

    }
}
