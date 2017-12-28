using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.ComponentModel;
using Neusoft.FrameWork.Models;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Operation
{
    /// <summary>
    /// [��������: ������Ա�б�]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-26]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class ListViewEmployee : Neusoft.FrameWork.WinForms.Controls.NeuListView
    {

        public ListViewEmployee()
        {
            this.imageList.Images.Add(Neusoft.FrameWork.WinForms.Classes.Function.GetImage(Neusoft.FrameWork.WinForms.Classes.EnumImageList.R��Ա));
            this.LargeImageList = this.imageList;
            this.SmallImageList = this.imageList;
            this.StateImageList = this.imageList;
        }
#region �ֶ�
        private string deptID;
        private ImageList imageList = new ImageList();
#endregion

#region ����
        /// <summary>
        /// ���ұ���
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DeptID
        {
            get
            {
                return this.deptID;
            }

            set
            {
                this.deptID = value;
                this.LoadData(value);
            }
        }
#endregion


#region ����


        #region donggq--2010.10.05--{DFFAAA32-367B-496b-B08E-9BB19925E795}

        private void LoadData()
        {
            ArrayList al = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D, "2600");
            al.AddRange(Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D, "2603"));

            foreach (NeuObject obj in al)
            {
                ListViewItem item = new ListViewItem();
                item.Text = obj.Name;
                item.ImageIndex = 0;
                item.Tag = obj;
                this.Items.Add(item);
            }
        } 

        #endregion

        private void LoadData(string deptID)
        {
            ArrayList al = Environment.IntegrateManager.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D, deptID);
            foreach(NeuObject obj in al)
            {
                ListViewItem item = new ListViewItem();
                item.Text = obj.Name;
                item.ImageIndex = 0;
                item.Tag = obj;
                this.Items.Add(item);
            }
        }

        public void Refresh()
        {
            this.Items.Clear();


            #region donggq--2010.10.05--{DFFAAA32-367B-496b-B08E-9BB19925E795}


            //this.LoadData(this.deptID);

            this.LoadData();

            #endregion
        }

        /// <summary>
        /// �Ƴ���Ա
        /// </summary>
        /// <param name="id">��ԱID</param>
        /// <returns></returns>
        public int RemoveEmployee(string id)
        {
            foreach(ListViewItem item in this.Items)
            {
                if((item.Tag as NeuObject).ID==id)
                {
                    this.Items.Remove(item);
                    return 0;
                }
            }

            return -1;
        }
#endregion
    }
}
