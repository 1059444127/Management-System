using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.IO;
using System.Data;
namespace AutoUpdate
{
	/// <summary>
	/// frmInfo 的摘要说明。
	/// zhangjunyi@neusoft.com 
	/// </summary>
	public class frmInfo : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btFileContent;
        private CheckBox cbContinue;
        private Button btClose;
        private Button btOk;
        private Label lbID;
        private Label label3;
        private TextBox txtFileName;
        private Label label4;
        private TextBox txtFilePath;
        private Label label1;
        private Label label6;
        private TextBox txtVision;
	
		public delegate void SaveDelegate(FtpFile obj);
		public event SaveDelegate SaveHandle;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmInfo()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInfo));
            this.btFileContent = new System.Windows.Forms.Button();
            this.cbContinue = new System.Windows.Forms.CheckBox();
            this.btClose = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.lbID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVision = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btFileContent
            // 
            this.btFileContent.Image = ((System.Drawing.Image)(resources.GetObject("btFileContent.Image")));
            this.btFileContent.Location = new System.Drawing.Point(216, 24);
            this.btFileContent.Name = "btFileContent";
            this.btFileContent.Size = new System.Drawing.Size(112, 136);
            this.btFileContent.TabIndex = 2;
            this.btFileContent.Click += new System.EventHandler(this.btFileContent_Click);
            // 
            // cbContinue
            // 
            this.cbContinue.Location = new System.Drawing.Point(21, 195);
            this.cbContinue.Name = "cbContinue";
            this.cbContinue.Size = new System.Drawing.Size(80, 24);
            this.cbContinue.TabIndex = 8;
            this.cbContinue.Text = "连续录入";
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(237, 195);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 7;
            this.btClose.Text = "关闭";
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(133, 195);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 6;
            this.btOk.Text = "保存";
            this.btOk.Click += new System.EventHandler(this.btOk_Click_1);
            // 
            // lbID
            // 
            this.lbID.Location = new System.Drawing.Point(96, 41);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(104, 17);
            this.lbID.TabIndex = 12;
            this.lbID.Text = "主键列";
            this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "文件名";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(80, 73);
            this.txtFileName.MaxLength = 50;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(120, 21);
            this.txtFileName.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "相对路径";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(80, 105);
            this.txtFilePath.MaxLength = 100;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(120, 21);
            this.txtFilePath.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "主键列";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "版本号";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtVision
            // 
            this.txtVision.Location = new System.Drawing.Point(80, 137);
            this.txtVision.MaxLength = 20;
            this.txtVision.Name = "txtVision";
            this.txtVision.Size = new System.Drawing.Size(120, 21);
            this.txtVision.TabIndex = 14;
            this.txtVision.Text = "JinYi1.0";
            // 
            // frmInfo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(336, 226);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtVision);
            this.Controls.Add(this.cbContinue);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btFileContent);
            this.Controls.Add(this.btOk);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(352, 264);
            this.MinimumSize = new System.Drawing.Size(352, 264);
            this.Name = "frmInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "录入界面";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region 全局变量
		SystemUpdate mgr = new SystemUpdate();
		ArrayList FileList = null;
		private EditType editType = EditType.None;
		private bool ISBool = true;
		#endregion 
		/// <summary>
		/// 编辑类型
		/// </summary>
		public EditType myEditType
		{
			get
			{
				return editType ;
			}
			set
			{
				editType = value;
			}
		}
		private  System.Data.OracleClient.OracleConnection  Oracon = null;
		#region 属性
		/// <summary>
		/// 
		/// </summary>
		public  System.Data.OracleClient.OracleConnection Con
		{
			get
			{
				return  Oracon;
			}
			set
			{
				Oracon = value;
			}
		}


		#endregion 
		/// <summary>
		/// 赋值
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public int SetInfo(FtpFile file)
		{
			FileList = new ArrayList();
			this.lbID.Text = file.PrimaryID;
			this.txtFileName.Text = file.FileName;
			this.txtFilePath.Text = file.LocalDirectory;
			this.txtVision.Text = file.FileVersion;
			FileList.Add(file); //
			return 1;
		}
		public void  CreatPrimarKey()
		{
			string strSql = "select seq_ftp_seq.nextval from dual";
			this.lbID.Text = mgr.ReturnOne(Oracon,strSql);
		}
		/// <summary>
		/// 清空
		/// </summary>
		private void Clear()
		{
			this.txtFileName.Text = "";
//			this.txtFilePath.Text = "";
//			this.txtVision.Text = "";
			FileList = null;
		}

		/// <summary>
		/// 加载文件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btFileContent_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog myDialog=new OpenFileDialog();
			//新生成列表
			FileList = new ArrayList();
			Stream fs;
			if(this.myEditType == EditType.Add)
			{
				myDialog.Multiselect  = true;
			}
			else
			{
				myDialog.Multiselect = false;
			}
			if (myDialog.ShowDialog()==DialogResult.OK)
			{
				foreach(string fileName in myDialog.FileNames)
				{
					FtpFile obj = new FtpFile();
					string str = fileName;
					int i = str.LastIndexOf("\\");
					if(i == -1)
					{
						i = 0;
					}
					#region 获取基本信息
					
					if(this.myEditType == EditType.Add)
					{
						obj.FileName = fileName.Substring(i+1);
						obj.LocalDirectory = this.txtFilePath.Text;
						obj.FileVersion = this.txtVision.Text;
						if(txtFileName.Text.Length == 0)
						{
							txtFileName.Text = obj.FileName;
						}
						else
						{
							txtFileName.Text += " " + obj.FileName;
						}
					}
					else if(this.myEditType == EditType.Modify)
					{
						if(fileName.Substring(i+1) != txtFileName.Text)
						{
							ISBool = false;
							MessageBox.Show("要修改的文件名跟加载的文件不一致" ,"警告",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
								return ;
						}
						else
						{
							ISBool = true;
						}
						obj.FileName = this.txtFileName.Text;
						obj.LocalDirectory = this.txtFilePath.Text;
						obj.FileVersion = this.txtVision.Text;
						obj.PrimaryID = this.lbID.Text;
					}
					obj.OperCode = "wx";
					obj.OperDate = System.DateTime.Now;
//					obj.PrimaryID = lbID.Text;
					#endregion 

					#region  获取文件内容
					OpenFileDialog myDialog11 = new OpenFileDialog();
					myDialog11.FileName = fileName;
					if ((fs=myDialog11.OpenFile())!=null)
					{
						byte[] blob = null;
						if(fs.Length ==0)
						{
							blob = new byte[1];
							blob[0] = 32;
						}
						else
						{
							blob = new byte[fs.Length];
							int n=fs.Read(blob,0,int.Parse(blob.Length.ToString()));
							fs.Close();
						}
						obj.FileContent = blob; //内容
					}
					#endregion 

					FileList.Add(obj);
				}
			}
		}
		/// <summary>
		/// 校验
		/// </summary>
		/// <returns></returns>
		private bool ValidState(ArrayList list)
		{
			foreach(FtpFile obj in list)
			{
				if(obj.FileName.Length >50)
				{
					MessageBox.Show("文件名过长");
					this.txtFileName.Focus();
					return false;
				}
				if(obj.LocalDirectory.Length >100)
				{
					MessageBox.Show("路径名过长");
					this.txtFilePath.Focus();
					return false;
				}
				if(obj.LocalDirectory == null ||obj.LocalDirectory.Length == 0 )
				{
					if(txtFilePath.Text.Length == 0)
					{
						MessageBox.Show("请输入路径名");
						this.txtFilePath.Focus();
						return false;
					}
					else
					{
						obj.LocalDirectory = txtFilePath.Text;
					}
				}
				if(obj.FileVersion.Length >20)
				{
					MessageBox.Show("版本名称过长");
					this.txtVision.Focus();
					return false;
				}
				if(obj.FileContent == null)
				{
					MessageBox.Show("请选择要加载的文件");
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// 获取信息
		/// </summary>
		/// <returns></returns>
		private FtpFile GetInfo()
		{
			FtpFile obj = new FtpFile();
			obj.PrimaryID = this.lbID.Text;
			obj.FileName = this.txtFileName.Text;
			obj.FileVersion = this.txtVision.Text;
			obj.LocalDirectory = this.txtFilePath.Text;
			obj.OperCode = "man";
			return obj;
		}
		private void btClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		/// <summary>
		/// 保存图片
		/// </summary>
		/// <param name="info"></param>
		/// <param name="trans"></param>
		private int SaveInfo(FtpFile info,OracleTransaction trans)
		{
			//			string sConn = SystemUpdate.ReadConfig("MSSQLCLIENT");
			//			Oracon = mgr.ConnectOracle(sConn);
			//			Oracon.Open();
			DataTable m_Dt=new DataTable();
			string m_Sql = "";
			try
			{
				

				//					int i = mgr.IsExistPrimaryKey(Oracon,info.PrimaryID);
				//					if(i == -1)
				//					{
				//						trans.Rollback();
				//						MessageBox.Show("查询是否存在主键失败");
				//						return -1;
				//					}
				int j = mgr.IsExistPrimaryKey(Oracon,info);
				if(j == -1)
				{
					trans.Rollback();
					MessageBox.Show("查询同一目录下重复文件名失败");
					return -1;
				}
				if(j == 1)
				{
					trans.Rollback();
					MessageBox.Show("同一目录下不能存在重复文件名");
					return -1;
				}
				if(myEditType == EditType.Modify)
				{
					m_Sql = "UPDATE com_downloadfile  SET file_name= '{1}',local_directory= '{2}',file_version= '{3}', oper_code= '{4}',oper_date=  sysdate  WHERE primary_id= '{0}'";
				}
				else if(myEditType == EditType.Add)
				{
					#region  插入一条空值
					m_Sql += "INSERT INTO com_downloadfile "
						+ "(	primary_id,"
						+ "file_name,"
						+ "local_directory,"
						+ "	file_content, "
						+ "	file_version,"
						+ "	oper_code,  "
						+ "	oper_date ) "
						+"VALUES " 
						+"( '{0}',"
						+"	'{1}',"
						+"	'{2}',"
						+"	empty_blob(),"
						+"	'{3}',"
						+"	'{4}',"
						+"	SYSDATE"
						+")";

					#endregion 
				}
				m_Sql = string.Format(m_Sql,info.PrimaryID,info.FileName,info.LocalDirectory,info.FileVersion,info.OperCode);
				OracleCommand mCmd=new OracleCommand(m_Sql,Oracon);
				mCmd.Transaction = trans;
				mCmd.ExecuteNonQuery();
				return SavePhoto(info.PrimaryID,info.FileContent,"primary_id","file_content","com_downloadfile",trans);
			}
			catch(Exception ex)
			{
				trans.Rollback();
				MessageBox.Show(ex.Message);
				return -1;
			}
		}

		/// <summary>
		/// save photo into db
		/// 保存图片到数据库（2005-5-14）
		/// </summary>
		/// <param name="data_id">ID的数值</param>
		/// <param name="p_Blob">图片BLOB</param>
		/// <param name="id">ID键</param>
		/// <param name="photo">图片键</param>
		/// <param name="tablename">表名</param>
		private int  SavePhoto(string PrimaryID,byte[] p_Blob,string id,string filecontent,string tablename,OracleTransaction trans)
		{
			try
			{
				OracleDataAdapter photoAdapter;
				DataSet photoDataSet;
				DataTable photoTable;
				DataRow photoRow;
				string strSql =  "SELECT primary_id, file_name,local_directory,file_content, file_version, oper_code,  oper_date   FROM com_downloadfile t WHERE t.primary_id = '{0}'";
				strSql = string.Format(strSql,PrimaryID);
				photoAdapter = new OracleDataAdapter(strSql,Oracon);

				photoDataSet= new DataSet(tablename);
				string strSQL  = "UPDATE com_downloadfile SET file_content= :fileContent ,oper_date= sysdate  WHERE primary_id= '{0}'" ;
				strSQL = string.Format(strSQL,PrimaryID);
				photoAdapter.UpdateCommand = new OracleCommand(strSQL,Oracon);
				//设置事务
				photoAdapter.UpdateCommand.Transaction = trans; 
				photoAdapter.UpdateCommand.Parameters.Add(":fileContent",
					OracleType.Blob, p_Blob.Length, filecontent);
				//
				//				photoAdapter.UpdateCommand.Parameters.Add(":ID",
				//					OracleType.VarChar, PrimaryID.Length, PrimaryID);

				photoAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
//				photoAdapter.InsertCommand.Transaction = trans;
				photoAdapter.SelectCommand.Transaction = trans;
//				photoAdapter.DeleteCommand.Transaction = trans;
				// Configures the schema to match with Data Source
				photoAdapter.FillSchema(photoDataSet, SchemaType.Source, tablename);

				// Fills the DataSet with 'drivers' table data
				photoAdapter.Fill(photoDataSet,tablename);

				// Get the current driver ID row for updation
				photoTable = photoDataSet.Tables[tablename];
				photoRow = photoTable.Rows.Find(PrimaryID);

				// Start the edit operation on the current row in
				// the 'drivvers' table within the dataset.
				photoRow.BeginEdit();
				// Assign the value of the Photo if not empty
				if (p_Blob.Length != 0)
				{
					photoRow[filecontent] = p_Blob;
				}
				// End the editing current row operation
				photoRow.EndEdit();

				// Update the database table 'drivers'
				photoAdapter.Update(photoDataSet,tablename);
				return 1;

			}
			catch(Exception e)
			{
				trans.Rollback();
				MessageBox.Show(e.Message);
				return -1;
			}
		}

		private void btOk_Click(object sender, System.EventArgs e)
		{
			SaveSingle();
		}
		/// <summary>
		/// 保存整个目录下的文件
		/// </summary>
		private void SaveMulti()
		{
		}
		/// <summary>
		///保存一个文件
		/// </summary>
		private void SaveSingle()
		{
			if(!ISBool)
			{
				MessageBox.Show("要修改的文件跟加载的文件不一致，请重新加载文件");
				return ;
			}
			if(FileList == null)
			{
				MessageBox.Show("请选择要保存的文件");
				return ;
			}
			if(!ValidState(FileList))
			{
				return ;
			}
			//
			OracleTransaction trans = Oracon.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
			mgr.SetTrans(trans);
			string strSql = "select seq_ftp_seq.nextval from dual";
			foreach(FtpFile obj in FileList)
			{
				if(this.myEditType == EditType.Add)
				{
					obj.PrimaryID =  mgr.ReturnOne(Oracon,strSql);
				}
				obj.LocalDirectory = this.txtFilePath.Text;
				if(SaveInfo(obj,trans) <= 0)
				{
					MessageBox.Show("保存失败");
					return ;
				}
				SaveHandle(obj);
			}
			trans.Commit();
			if(cbContinue.Checked)
			{
				this.Clear();
//				string strSql = "select seq_ftp_seq.nextval from dual";
				this.lbID.Text = mgr.ReturnOne(Oracon,strSql);
			}
			else
			{
				this.Close();
			}
		}

        private void btOk_Click_1(object sender, EventArgs e)
        {
            SaveSingle();
        }


        

	}
}
