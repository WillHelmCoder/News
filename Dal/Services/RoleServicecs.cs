using System;
using System.Web.Security;
using Dal.Interfaces;

namespace Dal.Services
{
    public class RoleServicecs : IRoleService
    {
        public Boolean RoleExists(String roleName)
        {
            return Roles.RoleExists(roleName);
        }

        public void CreateRole(String roleName)
        {
            Roles.CreateRole(roleName);
        }

        public void AddUserToRole(String userName, String roleName)
        {
            Roles.AddUserToRole(userName, roleName);
        }

        public Boolean IsUserInRole(String userName, String role)
        {
            return Roles.IsUserInRole(userName, role);
        }
    }
}