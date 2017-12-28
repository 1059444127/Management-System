using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;

namespace Neusoft.FrameWork.WinForms.Controls
{
    /// <summary>
    /// [��������: ���ڵ������б��]<br></br>
    /// [�� �� ��: ����ȫ]<br></br>
    /// [����ʱ��: 2006-12-08]<br></br>
    /// <�޸ļ�¼
    ///		�޸���=''
    ///		�޸�ʱ��='yyyy-mm-dd'
    ///		�޸�Ŀ��=''
    ///		�޸�����=''
    ///  />
    /// </summary>
    public class NeuListBoxPopup : NeuListBox
    {
        public NeuListBoxPopup()
        {
            this.Init();
        }
#region �ֶ�
        public event System.EventHandler ItemSelected;
        private ArrayList alItems = new ArrayList();
        private DataSet dsItems = new DataSet();
        private int spell = 0;
#endregion
#region ����
        /// <summary>
        /// �������뷨
        /// </summary>
        public int InputCode
        {
            get { return spell; }
            set
            {
                spell = value;
                if (spell > 2 || spell < 0) spell = 0;
            }
        }

        /// <summary>
        /// ��Ŀ�б�
        /// </summary>
        public ArrayList NeuItems
        {
            get 
            {
                if (alItems == null)
                {
                    alItems = new ArrayList();
                }

                return alItems;
            }
        }
#endregion

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        private int Init()
        {
            dsItems.Tables.Add("items");
            dsItems.Tables["items"].Columns.AddRange(new DataColumn[]
				{
					new DataColumn("ID",Type.GetType("System.String")),//ID
					new DataColumn("Name",Type.GetType("System.String")),//����
					new DataColumn("spell_code",Type.GetType("System.String")),//ƴ����
					new DataColumn("input_code",Type.GetType("System.String")),//������
					new DataColumn("wb_code",Type.GetType("System.String"))//�����
				});
            dsItems.CaseSensitive = false;
            return 1;
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        public void ClearItems()
        {
            this.alItems = new ArrayList();
            this.Items.Clear();
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int AddItems(ArrayList items)
        {
            base.Items.Clear();

            alItems = items;
            dsItems.Tables["items"].Rows.Clear();
            NeuObject objItem;
            Neusoft.HISFC.Models.Base.ISpell objspell;

            try
            {
                for (int i = 0; i < alItems.Count; i++)
                {                    
                    objItem = alItems[i] as NeuObject;
                    objspell = objItem as Neusoft.HISFC.Models.Base.ISpell;

                    base.Items.Add(objItem.ID + ". " + objItem.Name);
                    dsItems.Tables["items"].Rows.Add(new object[]{
																	 objItem.ID,objItem.Name,objspell.SpellCode,
																	 objspell.UserCode,objspell.WBCode});
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("�����Ŀ�б����!" + error.Message, "ListBox");
                return -1;
            }
            return 1;

        }

        /// <summary>
        /// ���ѡ����
        /// </summary>        
        /// <returns></returns>
        public NeuObject GetSelectedItem()
        {
            int index = base.SelectedIndex;
            if (index < 0 || index > base.Items.Count - 1)
            {                
                return null;
            }

            //���ID
            string itemname = base.SelectedItem.ToString();
            string ID = itemname.Substring(0, itemname.IndexOf(". ", 0));
            for (int i = 0; i < alItems.Count; i++)
            {
                NeuObject obj = (NeuObject)alItems[i];
                if (obj.ID == ID)
                {                    
                    return obj.Clone();
                }
            }            
            return null;
        }

        /// <summary>
        /// ������Ŀ
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Filter(string where)
        {
            //{1641980F-31D0-4176-A100-D7943BBD1617}
            //where = Neusoft.FrameWork.Public.String.TakeOffSpecialChar(where, "'", "\"", ";", ":", "(", ")", "%", "&", "#", "@");

            //if (string.IsNullOrEmpty(where))
            //{
            //    return 1;
            //}
            //{1641980F-31D0-4176-A100-D7943BBD1617}
            DataView _dv = new DataView(dsItems.Tables["items"]);
            try
            {
                if (spell == 0)
                    _dv.RowFilter = "(ID like '%" + where + "%') or (Name like '%" + where + "%') or (spell_code like '%" + where + "%')";
                else if (spell == 1)
                    _dv.RowFilter = "(ID like '%" + where + "%') or (Name like '%" + where + "%') or (input_code like '%" + where + "%')";
                else
                    _dv.RowFilter = "(ID like '%" + where + "%') or (Name like '%" + where + "%') or (wb_code like '%" + where + "%')";

                base.Items.Clear();
                for (int i = 0; i < _dv.Count; i++)
                {
                    DataRowView _row = _dv[i];
                    base.Items.Add(_row["ID"].ToString() + ". " + _row["Name"].ToString());
                }
                if (base.Items.Count > 0)
                    base.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                _dv.RowFilter = " 1 = 1";
            }
            

            return 1;
        }
        /// <summary>
        /// �ƶ���һ��
        /// </summary>
        /// <returns></returns>
        public int NextRow()
        {
            int index = base.SelectedIndex;
            if (index >= base.Items.Count - 1) return 1;

            base.SelectedIndex = index + 1;
            return 1;
        }
        /// <summary>
        /// �ƶ���һ��
        /// </summary>
        /// <returns></returns>
        public int PriorRow()
        {
            int index = base.SelectedIndex;
            if (index <= 0) return 1;

            base.SelectedIndex = index - 1;
            return 1;
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Enter)
            {
                //if (SelectItem != null)
                //    SelectItem(Keys.Enter);

                if (ItemSelected != null)
                    ItemSelected(this, e);
            }
        }
        
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            //if (SelectItem != null)
            //    SelectItem(Keys.Enter);

            if (ItemSelected != null)
                ItemSelected(this, e);
        }

    }
}
