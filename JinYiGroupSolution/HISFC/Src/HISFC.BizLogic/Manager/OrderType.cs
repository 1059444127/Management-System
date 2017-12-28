using System;
using System.Collections;
namespace Neusoft.HISFC.BizLogic.Manager
{
	/// <summary>
	/// OrderType ��ժҪ˵����
	/// </summary>
	public class OrderType:Neusoft.FrameWork.Management.Database,Neusoft.HISFC.Models.Base.IManagement
	{
		public OrderType()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
		#region IManagement ��Ա
		public ArrayList GetList()
		{
			// TODO:  ��� Frequency.GetList ʵ��
			string sql="";
			if(this.Sql.GetSql("Manager.OrderType.GetList.1",ref sql)==-1) return null;
//			try
//			{
//				sql=string.Format(sql);
//			}
//			catch{return null;}
			if(this.ExecQuery(sql)==-1) return null;
			ArrayList al=new ArrayList();
			try
			{
				while(this.Reader.Read())
				{
					Neusoft.HISFC.Models.Order.OrderType obj=new Neusoft.HISFC.Models.Order.OrderType();
					
					try
					{
						obj.ID=this.Reader[0].ToString();//idƵ��id
					}
					catch
					{}
					try
					{
						obj.Name =this.Reader[1].ToString();//nameƵ������
					}
					catch
					{}
					try
					{
						obj.IsCharge =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[2].ToString());//
					}
					catch
					{}
					try
					{
						obj.IsConfirm =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[3].ToString());
					}
					catch
					{}
					try
					{
						obj.IsDecompose=Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[4].ToString());//
					}
					catch
					{}
					try
					{
						obj.IsNeedPharmacy  =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[5].ToString());//
					}
					catch
					{}
					try
					{
						obj.IsPrint =Neusoft.FrameWork.Function.NConvert.ToBoolean(this.Reader[6].ToString());//
					}
					catch
					{}
					al.Add(obj);
				}
				return al;
			}
			catch{return null;}
		}

		public int Del(object obj)
		{
			// TODO:  ��� OrderType.Del ʵ��
			string strSql="";
			if(this.Sql.GetSql("Manager.OrderType.Delete.1",ref strSql)==-1) return -1;
			if(this.GetStrings(obj,ref strSql)==-1) return -1;
			if(this.ExecNoQuery(strSql)<=0)
			{
				return -1;
			}
			return 0;
		}
		private int GetStrings(object obj,ref string strSql)
		{
			#region "�ӿ�"
			//<!--0 idҽ������id, 1 nameҽ������, 2 �Ƿ�Ƿ�, 3 ȷ��, 4 �Ƿ�ֽ�, 5 �Ƿ���Ҫ��ҩ,6 �Ƿ��ӡ,
			//	 7 operator id, 8 operator name,9 operator time -->
			#endregion
			Neusoft.HISFC.Models.Order.OrderType o=obj as Neusoft.HISFC.Models.Order.OrderType;
			try
			{
				string[] s=new string[10];
				try
				{
					s[0]=o.ID ;//idƵ��id
				}
				catch{}
				try
				{
					s[1]=o.Name ;//nameƵ������
				}
				catch{}
				try
				{
					s[2]=System.Convert.ToInt16(o.IsCharge).ToString();//�Ƿ�Ƿ�
				}
				catch{}
				try
				{
					s[3]=System.Convert.ToInt16(o.IsConfirm).ToString() ;//ȷ��
				}
				catch{}
				try
				{
					s[4]=System.Convert.ToInt16(o.IsDecompose).ToString() ;//�Ƿ�ֽ�
				}
				catch{}
				try
				{
					s[5]=System.Convert.ToInt16(o.IsNeedPharmacy).ToString();//�Ƿ���Ҫ��ҩ
				}
				catch{}
				try
				{
					s[6]=System.Convert.ToInt16(o.IsPrint).ToString();//�Ƿ��ӡ
				}
				catch{}
				try
				{
					s[7]=this.Operator.ID ;//operator id
				}
				catch{}
				try
				{
					s[8]=this.Operator.Name ;//operator name
				}
				catch{}
				try
				{
					s[9]=this.GetSysDate();//operator time
				}
				catch{}
				strSql=string.Format(strSql,s);
			
			}
			catch(Exception ex)
			{
				this.Err="��ֵʱ�����"+ex.Message;
				this.WriteErr();
				return -1;
			}
			return 0;
		}
		public int SetList(ArrayList al)
		{
			// TODO:  ��� Frequency.SetList ʵ��
			return 0;
		}

		public Neusoft.FrameWork.Models.NeuObject Get(object obj)
		{
			// TODO:  ��� Frequency.Get ʵ��
			Neusoft.FrameWork.Models.NeuObject obj1 = new Neusoft.FrameWork.Models.NeuObject();
			return obj1;
		}

		public int Set(Neusoft.FrameWork.Models.NeuObject obj)
		{
			// TODO:  ��� OrderType.Set ʵ��
			#region "�ӿ�"
			//�ӿ����� Manager.OrderType.Update.1
			//<!--0 idҽ������id, 1 nameҽ������, 2 �Ƿ�Ƿ�, 3 ȷ��, 4 �Ƿ�ֽ�, 5 �Ƿ���Ҫ��ҩ,6 �Ƿ��ӡ,
			//	 7 operator id, 8 operator name,9 operator time -->
			#endregion
			string strSql="",strSql1="";
			if(this.Sql.GetSql("Manager.OrderType.Update.1",ref strSql)==-1) return -1;
			if(this.Sql.GetSql("Manager.OrderType.Insert.1",ref strSql1)==-1) return -1;
			if(this.GetStrings(obj,ref strSql)==-1) return -1;
			if(this.GetStrings(obj,ref strSql1)==-1) return -1;
			if(this.ExecNoQuery(strSql)<=0)
			{
				if(this.ExecNoQuery(strSql1)<=0)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
			return 0;
		}

		#endregion
	}
}
