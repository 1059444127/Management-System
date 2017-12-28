using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Neusoft.WinForms.Report.FinIpb
{
    public partial class ucFinIpbInDeptWork : Report.Common.ucQueryBaseForDataWindow
    {
        /// <summary>
        /// סԺ���ҹ�����ͳ��
        /// </summary>
        public ucFinIpbInDeptWork()
        {
            InitializeComponent();
        }

        Neusoft.HISFC.BizProcess.Integrate.Manager managerIntegrate = new Neusoft.HISFC.BizProcess.Integrate.Manager();

        protected override int OnDrawTree()
        {
            if (tvLeft == null)
            {
                return -1;
            }
            ArrayList deptList = managerIntegrate.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.N);

            TreeNode parentTreeNode = new TreeNode("ȫ��");
            tvLeft.Nodes.Add(parentTreeNode);
            foreach (Neusoft.HISFC.Models.Base.Department dept in deptList)
            {
                TreeNode deptNode = new TreeNode();
                deptNode.Tag = dept.ID;
                deptNode.Text = dept.Name;
                parentTreeNode.Nodes.Add(deptNode);
            }

            //deptList = managerIntegrate.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.P);
            //foreach (Neusoft.HISFC.Models.Base.Department dept in deptList)
            //{
            //    TreeNode deptNode = new TreeNode();
            //    deptNode.Tag = dept.ID;
            //    deptNode.Text = dept.Name;
            //    parentTreeNode.Nodes.Add(deptNode);
            //}

            parentTreeNode.ExpandAll();

            return base.OnDrawTree();
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        protected override int OnRetrieve(params object[] objects)
        {
            if (base.GetQueryTime() == -1)
            {
                return -1;
            }

            TreeNode selectNode = tvLeft.SelectedNode;

            if (selectNode.Level == 0)
            {
                return -1;
            }
            string deptCode = selectNode.Tag.ToString();

            return base.OnRetrieve(base.beginTime, base.endTime, deptCode);
        }

    }
}

