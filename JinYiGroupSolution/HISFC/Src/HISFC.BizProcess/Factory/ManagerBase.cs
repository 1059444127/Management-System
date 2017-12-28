using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
namespace Neusoft.HISFC.BizProcess.Factory
{
    /// <summary>
    /// ϵͳ����
    /// </summary>
    public  abstract class ManagerBase:FactoryBase
    {
        #region ���ҹ���

        public virtual System.Collections.ArrayList QueryDeptment()
        {
            Neusoft.HISFC.BizLogic.Manager.Department departManager = new Neusoft.HISFC.BizLogic.Manager.Department();

            this.SetDB(departManager);

            return departManager.GetDeptmentAll();
        }

        public virtual System.Collections.ArrayList QueryDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType deptType)
        {
            Neusoft.HISFC.BizLogic.Manager.Department departManager = new Neusoft.HISFC.BizLogic.Manager.Department();

            this.SetDB(departManager);

            return departManager.GetDeptment(deptType);

        }

        ///// <summary>
        ///// ���������Ϣ
        ///// </summary>
        ///// <param name="info">���һ�����Ϣ</param>
        ///// <returns>�ɹ� 1 ʧ�� -1</returns>
        //public virtual int InsertDept(Neusoft.HISFC.Models.Base.Department info)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Department departManager = new Neusoft.HISFC.BizLogic.Manager.Department();

        //    this.SetDB(departManager);

        //    return departManager.Insert(info);
        //}

        ///// <summary>
        ///// ���¿�����Ϣ
        ///// </summary>
        ///// <param name="info">���һ�����Ϣ</param>
        ///// <returns>�ɹ� 1 ʧ�� -1</returns>
        //public virtual int UpdateDept(Neusoft.HISFC.Models.Base.Department info)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();

        //    this.SetDB(deptManager);

        //    return deptManager.Update(info);
        //}

        ///// <summary>
        ///// ��ó��˻�ʿվ�����п���
        ///// </summary>
        ///// <returns>�ɹ� ���п���  ʧ�� null</returns>
        //public virtual ArrayList GetDeptNoNurse()
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();

        //    this.SetDB(deptManager);

        //    return deptManager.GetDeptNoNurse();
        //}

        /// <summary>
        /// ���ݿ������ͻ�ÿ����б�
        /// </summary>
        /// <param name="Type">��������</param>
        /// <returns>�ɹ� ���ݿ������ͻ�ÿ����б� ʧ�� null</returns>
        public virtual ArrayList GetDeptment(Neusoft.HISFC.Models.Base.EnumDepartmentType Type)
        {
            Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();

            this.SetDB(deptManager);

            return deptManager.GetDeptment(Type);
        }
        
        #endregion

        #region Manager ��Ա

        #region ƴ������ --Leiyj
        /// <summary>
        /// ���ݴ����ַ������ƴ����
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public virtual Neusoft.HISFC.Models.Base.ISpell GetSpell(string words)
        {
            Neusoft.HISFC.BizLogic.Manager.Spell spMgr = new Neusoft.HISFC.BizLogic.Manager.Spell();
            this.SetDB(spMgr);
            return spMgr.Get(words);
        }
        #endregion
        public virtual DateTime GetDateTimeFromSysDateTime()
        {
            Neusoft.HISFC.BizLogic.Manager.Department dept = new Neusoft.HISFC.BizLogic.Manager.Department();
            this.SetDB(dept);
            return dept.GetDateTimeFromSysDateTime();
        }

        #endregion

        #region ϵͳ���ܹ���

        public virtual System.Collections.ArrayList LoadModelFunction()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// ��ѯϵͳ����ģ��
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.ArrayList QuerySysModelFunction()
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.QuerySysModelFunction();
        }

        public virtual System.Collections.ArrayList QuerySysModelFunction(string sysCode)
        {

            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.QuerySysModelFunction(sysCode);
        }

        public virtual System.Collections.ArrayList QuerySysModelFunctionByType(string FormType)
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.QuerySysModelFunctionByType(FormType);
        }

        public virtual Neusoft.HISFC.Models.Admin.SysModelFunction QuerySysModelFunctionByID(string id)
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.QuerySysModelFunctionByID(id);
        }
        /// <summary>
        ///  ����ϵͳ����ģ����Ϣ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual int InsertSysModelFunction(Neusoft.HISFC.Models.Admin.SysModelFunction info)
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.InsertSysModelFunction(info);
        }
        /// <summary>
        /// ����ϵͳ����ģ����Ϣ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual int UpdateSysModelFunction(Neusoft.HISFC.Models.Admin.SysModelFunction info)
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.UpdateSysModelFunction(info);
        }
        /// <summary>
        /// ɾ��ϵͳ����ģ����Ϣ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual int DeleteSysModelFunction(Neusoft.HISFC.Models.Admin.SysModelFunction info)
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.DeleteSysModelFunction(info);
        }
        /// <summary>
        /// �õ������¼����id
        /// </summary>
        /// <returns></returns>
        public virtual string GetNewID()
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager functionManager = new Neusoft.HISFC.BizLogic.Manager.SysModelFunctionManager();

            this.SetDB(functionManager);

            return functionManager.GetNewID();
        }

        #endregion


        #region �˵�����

        public virtual System.Collections.ArrayList LoadAllMenu()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public virtual System.Collections.ArrayList LoadAllMenu(string groupID)
        {
            Neusoft.HISFC.BizLogic.Manager.SysMenuManager manager = new Neusoft.HISFC.BizLogic.Manager.SysMenuManager();
            this.SetDB(manager);
            return manager.LoadAll(groupID);
        }

        public virtual System.Collections.ArrayList LoadAllParentMenu(string groupID)
        {
            Neusoft.HISFC.BizLogic.Manager.SysMenuManager manager = new Neusoft.HISFC.BizLogic.Manager.SysMenuManager();
            this.SetDB(manager);
            return manager.LoadAllParentMenu(groupID);
        }
        public virtual System.Collections.ArrayList QueryMenu(Neusoft.HISFC.Models.Base.Hospital hospital)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int AddMenu(Neusoft.HISFC.Models.Base.Hospital hospital, Neusoft.HISFC.Models.Admin.SysMenu menu)
        {
            Neusoft.HISFC.BizLogic.Manager.SysMenuManager manager = new Neusoft.HISFC.BizLogic.Manager.SysMenuManager();
            this.SetDB(manager);
            return manager.InsertSysMenu(menu);
        }

        public virtual int AddMenu(Neusoft.HISFC.Models.Admin.SysMenu parantMenu)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int DeleteMenu(Neusoft.HISFC.Models.Admin.SysMenu menu)
        {
            return 0;
        }
        public virtual int DeleteMenu(Neusoft.HISFC.Models.Admin.SysGroup group)
        {
            Neusoft.HISFC.BizLogic.Manager.SysMenuManager manager = new Neusoft.HISFC.BizLogic.Manager.SysMenuManager();
            this.SetDB(manager);
            return manager.Delete(group.ID);
        }

        public virtual int DeleteMenu(Neusoft.HISFC.Models.Admin.SysGroup group, int x)
        {
            Neusoft.HISFC.BizLogic.Manager.SysMenuManager manager = new Neusoft.HISFC.BizLogic.Manager.SysMenuManager();
            this.SetDB(manager);
            return manager.Delete(group.ID, x);
        }
        public virtual int ModifyMenu(Neusoft.HISFC.Models.Admin.SysMenu menu)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int MoveMenu(Neusoft.HISFC.Models.Admin.SysMenu parantMenu, Neusoft.HISFC.Models.Admin.SysMenu menu)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public virtual System.Collections.ArrayList LoadAllModel()
        {
            Neusoft.HISFC.BizLogic.Manager.SysModelManager sysModelManager = new Neusoft.HISFC.BizLogic.Manager.SysModelManager();
            this.SetDB(sysModelManager);
            return sysModelManager.LoadAll();
        }
        #endregion

        #region �����

        public virtual System.Collections.ArrayList LoadAllGroup()
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            this.SetDB(sysGroupManager);
            return sysGroupManager.GetList();
        }

        public virtual System.Collections.ArrayList QueryGroup(Neusoft.HISFC.Models.Base.Hospital hospital)
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            this.SetDB(sysGroupManager);
            return sysGroupManager.GetList();
        }

        public virtual int AddGroup(Neusoft.HISFC.Models.Base.Hospital hospital, Neusoft.HISFC.Models.Admin.SysGroup sysGroup)
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            this.SetDB(sysGroupManager);
            return sysGroupManager.Insert(sysGroup);
        }

        public virtual int ModifyGroup(Neusoft.HISFC.Models.Admin.SysGroup sysGroup)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public virtual int DeleteGroup(Neusoft.HISFC.Models.Admin.SysGroup sysGroup)
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            this.SetDB(sysGroupManager);
            return sysGroupManager.Del(sysGroup);
        }

        //public virtual Neusoft.FrameWork.Object.NeuObject GetSysGroup(Neusoft.HISFC.Models.Admin.SysGroup sysGroup)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
        //    this.SetDB(sysGroupManager);
        //    return sysGroupManager.Get(null);
        //}
        public virtual Neusoft.FrameWork.Models.NeuObject GetSingleGroup(Neusoft.HISFC.Models.Admin.SysGroup sysGroup)
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
            this.SetDB(sysGroupManager);
            return sysGroupManager.Get(sysGroup);
        }
        #endregion


        #region ��Ա����

        /// <summary>
        /// ���ݿ��һ�õ�ǰ���ҵ�������Ա
        /// </summary>
        /// <param name="deptCode">���ұ���</param>
        /// <returns>�ɹ� ������Ա����  ʧ�� null</returns>
        public virtual ArrayList GetPersonsByDeptIDAll(string deptCode)
        {
            Neusoft.HISFC.BizLogic.Manager.Person personManager = new Neusoft.HISFC.BizLogic.Manager.Person();

            this.SetDB(personManager);

            return personManager.GetPersonsByDeptIDAll(deptCode);
        }



      
        #endregion


        #region Manager ��Ա


        public virtual int ChangePassword(string operatorID, string oldPassword, string newPassword)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IManager ��Ա

        public virtual Neusoft.HISFC.Models.Base.Department GetDeptmentById(string id)
        {
            Neusoft.HISFC.BizLogic.Manager.Department manager = new Neusoft.HISFC.BizLogic.Manager.Department();
            this.SetDB(manager);
            return manager.GetDeptmentById(id);
        }

       

        public virtual System.Collections.ArrayList GetDeptmentAll()
        {
            Neusoft.HISFC.BizLogic.Manager.Department dept = new Neusoft.HISFC.BizLogic.Manager.Department();
            this.SetDB(dept);
            return dept.GetDeptmentAll();
        }

        //public virtual int Insert(Neusoft.HISFC.Models.Base.Department info)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int Update(Neusoft.HISFC.Models.Base.Department info)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual string GetMaxDeptID()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int SelectDepartMentIsExist(string DepartmentId)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public virtual Neusoft.HISFC.Models.Base.Employee GetPersonByID(string personID)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager manager = new Neusoft.HISFC.BizLogic.Manager.UserManager();
            this.SetDB(manager);
            return manager.GetPerson(personID);
        }

        #endregion

        #region IManager ��Ա


        public virtual string CheckPwd(string userID, string PWD)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager manager = new Neusoft.HISFC.BizLogic.Manager.UserManager();
            this.SetDB(manager);
            return manager.CheckPwd(userID, PWD);

        }

       

        public virtual System.Collections.ArrayList GetMultiDept(string userID)
        {
            Neusoft.HISFC.BizLogic.Manager.DepartmentStatManager manager = new Neusoft.HISFC.BizLogic.Manager.DepartmentStatManager();
            this.SetDB(manager);
            return manager.GetMultiDept(userID);
        }

        #endregion


        #region ����������USERTEXT���׳�Ա  --ZGX

        /// <summary>
        /// �����Ժ�����б�
        /// </summary>
        /// <param name="tr"></param>
        /// <returns></returns>
        public virtual System.Collections.ArrayList QueryDeptmentsInHos(bool tr)
        {
            Neusoft.HISFC.BizLogic.Manager.Department manger = new Neusoft.HISFC.BizLogic.Manager.Department();
            this.SetDB(manger);
            return manger.GetInHosDepartment(tr);
        }

        /// <summary>
        /// ���ȫ����Ա�б�
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.ArrayList QueryEmployeeAll()
        {
            Neusoft.HISFC.BizLogic.Manager.Person pMgr = new Neusoft.HISFC.BizLogic.Manager.Person();
            this.SetDB(pMgr);
            return pMgr.GetEmployeeAll();
        }

       

        #region UserText   --Zgx

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public virtual System.Collections.ArrayList GetGroupList(string code, string Type)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText usrTexMgr = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(usrTexMgr);
            return usrTexMgr.GetGroupList(code, Type);
        }

        public virtual System.Collections.ArrayList GetUserTextList(string code, int type)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText userText = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(userText);
            return userText.GetList(code, type);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int InsertUserText(Neusoft.HISFC.Models.Base.UserText obj)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText usrTextMgr = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(usrTextMgr);
            return usrTextMgr.Insert(obj);
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int UpdateUserText(Neusoft.HISFC.Models.Base.UserText obj)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText usrTextMgr = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(usrTextMgr);
            return usrTextMgr.Update(obj);
        }

        //public virtual int ReplaceSql(Neusoft.HISFC.Models.Base.UserText obj, ref string sql)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}


        public virtual int UpdateFrequency(string id, string operId)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText userText = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(userText);
            return userText.UpdateFrequency(id, operId);
        }

        public virtual int DeleteUserText(string ID)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText userText = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(userText);
            return userText.Delete(ID);
        }

        public virtual ArrayList GetUserTextList(string GroupId, string Code, int Type)
        {
            Neusoft.HISFC.BizLogic.Manager.UserText userText = new Neusoft.HISFC.BizLogic.Manager.UserText();
            this.SetDB(userText);
            return userText.GetList(GroupId, Code, Type);
        }

        #endregion


        #endregion

        #region ��ѯ�������� --Leiyj
        /// <summary>
        /// ��ò�ѯ����
        /// </summary>
        /// <returns></returns>
        public virtual string GetQueryCondtion(string formName)
        {
            Neusoft.HISFC.BizLogic.Manager.QueryCondition qc = new Neusoft.HISFC.BizLogic.Manager.QueryCondition();
            this.SetDB(qc);
            return qc.GetQueryCondtion(formName);
        }

        /// <summary>
        /// ��ò�ѯ����
        /// </summary>
        /// <returns></returns>
        public virtual string GetQueryCondtion(string formName, bool isDefault)
        {
            Neusoft.HISFC.BizLogic.Manager.QueryCondition qc = new Neusoft.HISFC.BizLogic.Manager.QueryCondition();
            this.SetDB(qc);
            return qc.GetQueryCondtion(formName, isDefault);
        }

        /// <summary>
        /// ���ò�ѯ����
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public virtual int SetQueryCondition(string formName, string xml)
        {
            Neusoft.HISFC.BizLogic.Manager.QueryCondition qc = new Neusoft.HISFC.BizLogic.Manager.QueryCondition();
            this.SetDB(qc);
            return qc.SetQueryCondition(formName, xml);
        }

        /// <summary>
        /// ���ò�ѯ����
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="xml"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public virtual int SetQueryCondition(string formName, string xml, bool isDefault)
        {
            Neusoft.HISFC.BizLogic.Manager.QueryCondition qc = new Neusoft.HISFC.BizLogic.Manager.QueryCondition();
            this.SetDB(qc);
            return qc.SetQueryCondition(formName, xml, isDefault);
        }
        #endregion

        #region ֽ�Ź��� --Leiyj
        /// <summary>
        /// ���ֽ�Ŵ�С
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Neusoft.HISFC.Models.Base.PageSize GetPageSize(string pageName, string deptName)
        {
            Neusoft.HISFC.BizLogic.Manager.PageSize pageSize = new Neusoft.HISFC.BizLogic.Manager.PageSize();
            this.SetDB(pageSize);
            return pageSize.GetPageSize(pageName, deptName);
        }

        /// <summary>
        /// ���ֽ�Ŵ�С
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Neusoft.HISFC.Models.Base.PageSize GetPageSize(string ID)
        {
            Neusoft.HISFC.BizLogic.Manager.PageSize pageSize = new Neusoft.HISFC.BizLogic.Manager.PageSize();
            this.SetDB(pageSize);
            return pageSize.GetPageSize(ID);
        }

        #endregion

        #region ��Ա�����

        /// <summary>
        /// ͨ����Ա�������Ѿ�ά���ĵ�½����Ϣ
        /// </summary>
        /// <param name="personID">��ԱID</param>
        /// <returns>�ɹ� �Ѿ�ά���ĵ�½����Ϣ ʧ�� null</returns>
        public virtual ArrayList GetPersonGroupList(string personID)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.GetPersonGroupList(personID);
        }

        /// <summary>
        /// ������е�½����Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList QueryLogOnGroupList()
        {
            Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroupManager = new Neusoft.HISFC.BizLogic.Manager.SysGroup();

            this.SetDB(sysGroupManager);

            return sysGroupManager.GetList();
        }

        /// <summary>
        /// ������Ա��
        /// </summary>
        /// <param name="Person">��Ա������Ϣ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public virtual int UpdatePersonGroup(Neusoft.HISFC.Models.Base.Employee person)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.UpdatePersonGroup(person);
        }

        /// <summary>
        /// �Ƿ���ڵ�½����
        /// </summary>
        /// <param name="loginName">��½��</param>
        /// <param name="operCode">��Ա����</param>
        /// <returns>1���� ����������</returns>
        public virtual int IsExistLoginName(string loginName, string operCode)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.IsExistLoginName(loginName, operCode);
        }

        /// <summary>
        /// ����Ա���Ż�õ�½��Ϣ
        /// </summary>
        /// <param name="emplCode">Ա������</param>
        /// <returns>��½��Ϣ����</returns>
        public virtual ArrayList GetLoginInfoByEmplCode(string emplCode)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.GetLoginInfoByEmplCode(emplCode);
        }

        /// <summary>
        /// ������Ա��
        /// </summary>
        /// <param name="person">��Ա��Ϣ</param>
        /// <param name="group">����Ϣ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public virtual int InsertPersonGroup(Neusoft.HISFC.Models.Base.Employee person, Neusoft.FrameWork.Models.NeuObject group)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.InsertPersonGroup(person, group);
        }

        /// <summary>
        /// ɾ����Ա��
        /// </summary>
        /// <param name="person">��Ա��Ϣ</param>
        /// <param name="group">����Ϣ</param>
        /// <returns>�ɹ� 1 ʧ�� -1</returns>
        public virtual int DeletePersonGroup(Neusoft.HISFC.Models.Base.Employee person, Neusoft.FrameWork.Models.NeuObject group)
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();


            this.SetDB(userManager);

            return userManager.DeletePersonGroup(person, group);
        }
        /// <summary>
        /// ���ݳ������ڻ�ȡ����
        /// </summary>
        /// <param name="birthday">��������</param>
        /// <returns>����</returns>
        public virtual string GetAge(DateTime birthday)
        {
            Neusoft.HISFC.BizLogic.Manager.Person personManager = new Neusoft.HISFC.BizLogic.Manager.Person();
            this.SetDB(personManager);
            return personManager.GetAge(birthday);
        }
        /// <summary>
        /// ��ȡ������Ա���б�
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList GetPeronList()
        {
            Neusoft.HISFC.BizLogic.Manager.UserManager userManager = new Neusoft.HISFC.BizLogic.Manager.UserManager();

            this.SetDB(userManager);

            return userManager.GetPeronList();
        }
        #endregion

        #region ���ȫ�����ÿ����б�

        /// <summary>
        ///  ���ȫ�����ÿ����б�
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.ArrayList GetDepartment()
        {

            Neusoft.HISFC.BizLogic.Manager.Department deptMgr = new Neusoft.HISFC.BizLogic.Manager.Department();
            this.SetDB(deptMgr);
            return deptMgr.GetDeptmentAll();

        }

        #endregion

        #region ����
        //#region ��������
        ///// <summary>
        /////  ��ѯ�����������������
        ///// </summary>
        ///// <param name="t">������� ����/סԺ</param>
        ///// <param name="deptCode">  ���Ҵ���</param>
        ///// <param name="docCode"> ҽ������</param>
        ///// <returns>�ɹ����������б� ʧ�ܷ���null</returns>
        //public virtual System.Collections.ArrayList GetDeptOrderGroup(Neusoft.HISFC.Models.Base.ServiceTypes t, string deptCode, string docCode)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group gMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(gMgr);
        //    return gMgr.GetDeptOrderGroup(t, deptCode, docCode);
        //}

        ///// <summary>
        /////  ���ҽ���������
        ///// </summary>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public virtual System.Collections.ArrayList GetAllOrderGroup(Neusoft.HISFC.Models.Base.ServiceTypes t)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group gMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(gMgr);
        //    return gMgr.GetAllOrderGroup(t);
        //}

        ///// <summary>
        ///// ������������
        ///// </summary>
        ///// <param name="GroupId">����ID</param>
        ///// <param name="GroupName">��������</param>
        ///// <returns></returns>
        //public virtual int UpdateGroupName(string GroupId, string GroupName)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group gMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(gMgr);
        //    return gMgr.UpdateGroupName(GroupId, GroupName);
        //}

        ///// <summary>
        ///// ɾ��һ������
        ///// </summary>
        ///// <param name="gr"></param>
        ///// <returns></returns>
        //public virtual int DeleteGroup(Neusoft.HISFC.Models.Base.Group gr)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group gMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(gMgr);
        //    return gMgr.DeleteGroup(gr);
        //}

        ///// <summary>
        ///// ɾ��������ϸ
        ///// </summary>
        ///// <param name="group"></param>
        ///// <returns></returns>
        //public virtual int DeleteGroupOrder(Neusoft.HISFC.Models.Base.Group group)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.DeleteGroupOrder(group);
        //}

        ///// <summary>
        /////  �����Ŀ
        ///// </summary>
        ///// <param name="group"></param>
        ///// <returns></returns>
        //public virtual System.Collections.ArrayList GetAllItem(Neusoft.HISFC.Models.Base.Group group)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.GetAllItem(group);
        //}

        ///// <summary>
        ///// ����µ���ID
        ///// </summary>
        ///// <returns></returns>
        //public virtual String GetNewGroupID()
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.GetNewGroupID();
        //}

        ///// <summary>
        ///// ����һ����
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public virtual int UpdateGroup(Neusoft.HISFC.Models.Base.Group group)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.UpdateGroup(group);
        //}

        ///// <summary>
        ///// ����һ������Ŀ ����
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="order"></param>
        ///// <returns></returns>
        //public virtual int UpdateGroupItem(Neusoft.HISFC.Models.Base.Group group, Neusoft.HISFC.Models.Order.OutPatient.Order order)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.UpdateGroupItem(group, order);
        //}

        ///// <summary>
        ///// ����һ������Ŀ סԺ
        ///// </summary>
        ///// <param name="group"></param>
        ///// <param name="order"></param>
        ///// <returns></returns>
        //public virtual int UpdateGroupItem(Neusoft.HISFC.Models.Base.Group group, Neusoft.HISFC.Models.Order.Inpatient.Order order)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Group grMgr = new Neusoft.HISFC.BizLogic.Manager.Group();
        //    this.SetDB(grMgr);
        //    return grMgr.UpdateGroupItem(group, order);
        //}
        //#endregion
        //#region ҩƷ����
        ///// <summary>
        ///// ��ȡҩ�������б�
        ///// </summary>
        ///// <returns></returns>
        //public virtual ArrayList QueryPhaFunction()
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Constant manager = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
        //    this.SetDB(manager);
        //    return manager.QueryPhaFunction();
        //}
        //public virtual System.Collections.ArrayList LoadAllDrug()
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item manager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(manager);
        //    return new ArrayList(manager.QueryItemList(false).ToArray());
        //}

        //public virtual System.Collections.ArrayList QueryDrug(Neusoft.HISFC.Models.Base.Hospital hospital)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual System.Collections.ArrayList QueryDrug(Neusoft.HISFC.Models.Base.Hospital hospital, string drugType)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int AddDrug(Neusoft.HISFC.Models.Base.Hospital hospital, Neusoft.HISFC.Models.Pharmacy.Item item)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int ModifyDrug(Neusoft.HISFC.Models.Pharmacy.Item item)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int DeleteDrug()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual System.Collections.ArrayList GetOrderTypeList()
        //{
        //    Neusoft.HISFC.BizLogic.Manager.OrderType orderType = new Neusoft.HISFC.BizLogic.Manager.OrderType();
        //    this.SetDB(orderType);
        //    return orderType.GetList();
        //}
        ///// <summary>
        ///// ��ȡҩƷ��Ϣ
        ///// </summary>
        ///// <returns></returns>
        //public virtual ArrayList QueryItemList()
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    return new ArrayList(itemManager.QueryItemList());
        //}
        ///// <summary>
        ///// ���ݱ����ȡҩƷ
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public virtual Neusoft.HISFC.Models.Pharmacy.Item GetItem(string code)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    return itemManager.GetItem(code);
        //}

        ///// <summary>
        ///// ���ݱ�Ż�ȡҩƷ�洢���к�
        ///// </summary>
        ///// <param name="drugNo"></param>
        ///// <returns></returns>
        //public virtual int GetDrugStorageRowNum(string drugNo)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    return itemManager.GetDrugStorageRowNum(drugNo);
        //}

        ///// <summary>
        ///// ���ݱ��ɾ��ҩƷ��Ϣ
        ///// </summary>
        ///// <param name="drugNo"></param>
        ///// <returns></returns>
        //public virtual int DeleteItem(string drugNo)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    return itemManager.DeleteItem(drugNo);
        //}

        ///// <summary>
        ///// ����ҩƷ��Ϣ
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public virtual string SetItem(Neusoft.HISFC.Models.Pharmacy.Item item)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    int i = itemManager.SetItem(item);

        //    if (i == -1)
        //        return i.ToString();
        //    else
        //        return item.ID;

        //}
        ///// <summary>
        ///// �����û������ȡ��Ч��ҩƷ��Ϣ
        ///// </summary>
        ///// <param name="userCode"></param>
        ///// <returns></returns>
        //public virtual ArrayList QueryValidDrugByCustomCode(string userCode)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemManager = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemManager);
        //    return new ArrayList(itemManager.QueryValidDrugByCustomCode(userCode));
        //}
        ///// <summary>
        ///// ��ȡҩ��Ҷ�ӽڵ�����
        ///// </summary>
        ///// <returns></returns>
        //public virtual ArrayList QueryPhaFunctionLeafage()
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Constant itemConsManager = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
        //    this.SetDB(itemConsManager);
        //    return itemConsManager.QueryPhaFunctionLeafage();
        //}
        ///// <summary>
        ///// ��ȡҩƷ��Ӧ����Ϣ
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public virtual ArrayList QueryCompany(string type)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Constant company = new Neusoft.HISFC.BizLogic.Pharmacy.Constant();
        //    this.SetDB(company);
        //    return company.QueryCompany(type);
        //}
        //#endregion

        //#region ��ҩƷ����
        //public virtual System.Collections.ArrayList LoadAllUnDrug()
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item manager = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(manager);
        //    return new ArrayList(manager.QueryAllItemList());
        //}
        //public virtual bool UnDrugIsUsed(string code)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item manager = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(manager);
        //    return manager.IsUsed(code);
        //}
        //public virtual int DeleteUndrugItemByCode(string code)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item manager = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(manager);
        //    return manager.DeleteUndrugItemByCode(code);
        //}
        //public virtual System.Collections.ArrayList QueryUnDrug(Neusoft.HISFC.Models.Base.Hospital hospital)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int AddUnDrug(Neusoft.HISFC.Models.Base.Hospital hospital, Neusoft.HISFC.Models.Fee.Item.Undrug unDrug)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int ModifyUnDrug(Neusoft.HISFC.Models.Fee.Item.Undrug unDrug)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int DeleteUnDrug(Neusoft.HISFC.Models.Fee.Item.Undrug unDrug)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}
        ///// <summary>
        ///// ���ӷ�ҩƷ
        ///// </summary>
        ///// <param name="unDrug"></param>
        ///// <returns></returns>
        //public virtual int InsertUndrugItem(Neusoft.HISFC.Models.Fee.Item.Undrug unDrug)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item item = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(item);
        //    return item.InsertUndrugItem(unDrug);
        //}
        ///// <summary>
        ///// �޸ķ�ҩƷ
        ///// </summary>
        ///// <param name="unDrug"></param>
        ///// <returns></returns>
        //public virtual int UpdateUndrugItem(Neusoft.HISFC.Models.Fee.Item.Undrug unDrug)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item item = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(item);
        //    return item.UpdateUndrugItem(unDrug);
        //}
        ///// <summary>
        ///// �õ�һ������ˮ��
        ///// </summary>
        ///// <returns></returns>
        //public virtual string GetUndrugCode()
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item item = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(item);
        //    return item.GetUndrugCode();
        //}
        //#endregion

        //#region �ֵ����
        //public virtual System.Collections.ArrayList GetConstByGroup(string groupID)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.SysGroup sysGroup = new Neusoft.HISFC.BizLogic.Manager.SysGroup();
        //    this.SetDB(sysGroup);
        //    return this.GetConstByGroup(groupID);
        //}
        //public virtual System.Collections.ArrayList LoadAllDictionary()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual System.Collections.ArrayList QueryDictinary(Neusoft.HISFC.Models.Base.Hospital hospital)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual System.Collections.ArrayList QueryDictionary(Neusoft.HISFC.Models.Base.Hospital hospital, string dictionaryType)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int AddDictionary(Neusoft.HISFC.Models.Base.Hospital hospital)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int ModifyDictionary()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public virtual int DeleteDictionary()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}
        //public virtual ArrayList GetConstantList(string constant)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Constant con = new Neusoft.HISFC.BizLogic.Manager.Constant();
        //    this.SetDB(con);
        //    return con.GetList(constant);
        //}
        //#endregion

        //#region Ƶ�ι���
        //public virtual int AddFrequency(Neusoft.HISFC.Models.Order.Frequency frequency)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManager = new Neusoft.HISFC.BizLogic.Manager.Frequency();
        //    this.SetDB(frequencyManager);
        //    return frequencyManager.Set(frequency);
        //}

        //public virtual int DelFrequenty(Neusoft.HISFC.Models.Order.Frequency frequency)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManager = new Neusoft.HISFC.BizLogic.Manager.Frequency();
        //    this.SetDB(frequencyManager);
        //    return frequencyManager.Del(frequency);
        //}
        //public virtual ArrayList GetFrequencyList(string depCode)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManager = new Neusoft.HISFC.BizLogic.Manager.Frequency();
        //    this.SetDB(frequencyManager);
        //    return frequencyManager.GetList(depCode);
        //}

        //public virtual int ExistFrequencyCounts(Neusoft.HISFC.Models.Order.Frequency frequency)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManager = new Neusoft.HISFC.BizLogic.Manager.Frequency();
        //    this.SetDB(frequencyManager);
        //    return frequencyManager.ExistFrequencyCounts(frequency);
        //}
        //public virtual int DelFrequency(Neusoft.HISFC.Models.Order.Frequency frequency)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Frequency frequencyManager = new Neusoft.HISFC.BizLogic.Manager.Frequency();
        //    this.SetDB(frequencyManager);
        //    return frequencyManager.Del(frequency);
        //}
        //#endregion

      

     

        //#region �������� --Leiyj
        //public virtual System.Collections.ArrayList GetConstantList(Neusoft.HISFC.Models.Base.EnumConstant constType)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Constant con = new Neusoft.HISFC.BizLogic.Manager.Constant();
        //    this.SetDB(con);
        //    string type = constType.ToString();
        //    return con.GetList(type);
        //}
        ///// <summary>
        ///// ���ݴ���TYPE������г����б�
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public virtual ArrayList GetConstantListFromType(string type)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Constant con = new Neusoft.HISFC.BizLogic.Manager.Constant();
        //    this.SetDB(con);
        //    return con.GetAllList(type);

        //}
        ///// <summary>
        ///// ���³������е�һ������
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="cost"></param>
        ///// <returns></returns>
        //public virtual int UpdateConst(string type, Neusoft.HISFC.Models.Base.Const cost)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Constant con = new Neusoft.HISFC.BizLogic.Manager.Constant();
        //    this.SetDB(con);
        //    return con.UpdateItem(type, cost);
        //}
        ///// <summary>
        ///// ���볣�����е�һ������
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="cost"></param>
        ///// <returns></returns>
        //public virtual int InsertConst(string type, Neusoft.HISFC.Models.Base.Const cost)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Constant con = new Neusoft.HISFC.BizLogic.Manager.Constant();
        //    this.SetDB(con);
        //    return con.InsertItem(type, cost);
        //}

        //#endregion

        //#region ��λ���� --Leiyj
        ///// <summary>
        ///// ��ȡ���д�λ��Ϣ
        ///// </summary>
        ///// <param name="dv">���ص�������ͼ</param>
        ///// <returns>1 �ɹ� ;-1 ʧ��</returns>
        //public virtual int QueryBedInfo(ref System.Data.DataView dv)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Bed bedMgr = new Neusoft.HISFC.BizLogic.Manager.Bed();
        //    this.SetDB(bedMgr);
        //    return bedMgr.QueryBedInfo(ref dv);
        //}

        ///// <summary>
        ///// ��ȡ��λ��Ϣ,���ݻ�ʿվID
        ///// </summary>
        ///// <param name="id">��ʿվID</param>
        ///// <param name="dv">���ص�������ͼ</param>
        ///// <returns>-1,ʧ��; 1,�ɹ�</returns>
        //public virtual int QueryBedInfoByNurseStationID(string id, ref System.Data.DataView dv)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.Bed bed = new Neusoft.HISFC.BizLogic.Manager.Bed();
        //    this.SetDB(bed);
        //    return bed.QueryBedInfoByNurseStationID(id, ref dv);
        //}
        //#endregion

       

       

        //#region Ȩ�޹��� --Leiyj
        ///// <summary>
        ///// ������Ա���룬����Ȩ�ޱ���ȡ��Աӵ��Ȩ�޵Ĳ���
        ///// </summary>
        ///// <param name="userCode">����Ա����</param>
        ///// <param name="class2Code">����Ȩ����</param>
        ///// <returns>�ɹ����ؾ���Ȩ�޵Ŀ��Ҽ��� ʧ�ܷ���null</returns>   
        //public virtual System.Collections.Generic.List<Neusoft.FrameWork.Object.NeuObject> QueryUserPriv(string userCode, string class2Code)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager userPorwerMgr = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager();
        //    this.SetDB(userPorwerMgr);
        //    return userPorwerMgr.QueryUserPriv(userCode, class2Code);
        //}

        ///// <summary>
        ///// ȡ����Ա��ӵ�е�Ȩ�޲�������
        ///// </summary>
        ///// <param name="userCode">����Ա����</param>
        ///// <param name="class2Code">����Ȩ����</param>
        ///// <param name="class3Code">����Ȩ����</param>
        ///// <returns>�ɹ����ؾ���Ȩ�޵Ŀ��Ҽ��� ʧ�ܷ���null</returns>
        //public virtual System.Collections.Generic.List<Neusoft.FrameWork.Object.NeuObject> QueryUserPriv(string userCode, string class2Code, string class3Code)
        //{
        //    Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager userPowerMgr = new Neusoft.HISFC.BizLogic.Manager.UserPowerDetailManager();
        //    this.SetDB(userPowerMgr);
        //    return userPowerMgr.QueryUserPriv(userCode, class2Code, class3Code);
        //}
        //#endregion

       
        #region IManager ��Ա

        ///// <summary>
        ///// ����ҩƷ������ĳһҩƷ��Ϣ
        ///// </summary>
        ///// <param name="ID">ҩƷ����</param>
        ///// <returns>�ɹ�����ҩƷʵ�� ʧ�ܷ���null</returns>
        //public virtual Neusoft.HISFC.Models.Pharmacy.Item QueryDrug(string ItemCode)
        //{
        //    Neusoft.HISFC.BizLogic.Pharmacy.Item itemMgr = new Neusoft.HISFC.BizLogic.Pharmacy.Item();
        //    this.SetDB(itemMgr);
        //    return itemMgr.GetItem(ItemCode);
        //}

        //#region �ݷ�ҩƷ�����ø���Ŀ��Ϣ(����Ŀ������Ч)
        ////
        //// ժҪ:
        ////     ���ݷ�ҩƷ�����ø���Ŀ��Ϣ(����Ŀ������Ч)
        ////
        //// ����:
        ////   undrugCode:
        ////     ��ҩƷ����
        ////
        //// ���ؽ��:
        ////     �ɹ�:���ط�ҩƷʵ�� ʧ��:����null
        //public virtual Neusoft.HISFC.Models.Fee.Item.Undrug GetValidItemByUndrugCode(string undrugCode)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item itemMgr = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(itemMgr);
        //    return itemMgr.GetValidItemByUndrugCode(undrugCode);
        //}

        //#endregion
        //public virtual Neusoft.HISFC.Models.Fee.Item.Undrug QueryUnDrug(string itemCode)
        //{
        //    Neusoft.HISFC.BizLogic.Fee.Item itemMgr = new Neusoft.HISFC.BizLogic.Fee.Item();
        //    this.SetDB(itemMgr);
        //    return itemMgr.GetValidItemByUndrugCode(itemCode);
        //}

        //public virtual string QueryControlerInfo(string controlID)
        //{
        //    return null;
        //}

        #endregion

       

       

      

        //#region IManager ��Ա


        //public virtual string GetNewOrderComboID()
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //#endregion

      



      

       
       

        #endregion
        #region ��Ϣ����
        /// <summary>
        /// ��ѯ��Ϣ
        /// </summary>
        /// <returns></returns>
        private ArrayList QueryMessage(string oper)
        {
            //Neusoft.HISFC.BizLogic.EPR.Message messageManager = new Neusoft.HISFC.BizLogic.EPR.Message();

            //this.SetDB(messageManager);

            return null;
           
        }
        /// <summary>
        /// ����id��ѯ
        /// </summary>
        /// <returns></returns>
        private Neusoft.HISFC.Models.Base.Message QueryMessageById(string id)
        {
            //Neusoft.HISFC.BizLogic.EPR.Message messageManager = new Neusoft.HISFC.BizLogic.EPR.Message();

            //this.SetDB(messageManager);

            return null;


        }
        /// <summary>
        /// ����һ����Ϣ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private int InsertMessage(Neusoft.HISFC.Models.Base.Message message)
        {
            Neusoft.HISFC.BizLogic.EPR.Message messageManager = new Neusoft.HISFC.BizLogic.EPR.Message();

            this.SetDB(messageManager);

            return messageManager.InsertMessage(message);

        }
        private int UpdateMessage(Neusoft.HISFC.Models.Base.Message message)
        {
            Neusoft.HISFC.BizLogic.EPR.Message messageManager = new Neusoft.HISFC.BizLogic.EPR.Message();

            this.SetDB(messageManager);

            return messageManager.UpdateMessage(message);

        }
        #endregion 
        #region �ճ̹���
        /// <summary>
        /// ��ѯȫ���ճ�
        /// </summary>
        /// <returns></returns>
        private ArrayList QueryCalendar()
        {
            Neusoft.HISFC.BizLogic.EPR.Calendar calendarManager = new Neusoft.HISFC.BizLogic.EPR.Calendar();

            this.SetDB(calendarManager);

            return calendarManager.QueryCalendar();
            
        }
        /// <summary>
        /// �����ճ�
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        private int AddCalender(Neusoft.HISFC.Models.Base.Calendar calendar)
        {
            Neusoft.HISFC.BizLogic.EPR.Calendar calendarManager = new Neusoft.HISFC.BizLogic.EPR.Calendar();

            this.SetDB(calendarManager);

            return calendarManager.InsertCalendar(calendar);

        }
        /// <summary>
        /// ��ʱ��β�ѯ
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        private ArrayList QueryCalendar(DateTime dtBegin, DateTime dtEnd)
        {
            Neusoft.HISFC.BizLogic.EPR.Calendar calendarManager = new Neusoft.HISFC.BizLogic.EPR.Calendar();

            this.SetDB(calendarManager);

            return calendarManager.QueryCalendar(dtBegin,dtEnd);

        }
        #endregion 



    }
}