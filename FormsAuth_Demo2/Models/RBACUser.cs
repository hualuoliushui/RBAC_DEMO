using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.DAOFactory;
using DAO.DAO;
using DAO.DAOVO;

namespace FormsAuth_Demo2.Models
{
    public class RolePermission{
        public int permissionID{set;get;}
        public string PermissionDescription{set;get;}
    }
    public class UserRole
    {
        public int roleID { set; get; }
        public string roleName{set;get;}
        public List<RolePermission> permissions = new List<RolePermission>();
    }
    public class RBACUser
    {
        public int userID { set; get; }
        public string userName { set; get; }
        private List<UserRole> roles = new List<UserRole>();

        public RBACUser(string _userName){
            this.userName = _userName;
            GetDatabaseUserRolesPermissions();
        }

        private void GetDatabaseUserRolesPermissions(){
            //根据用户名从数据库中填充角色列表和权限列表
            UserDAO userDAO = Factory.getUserDAOInstance();
            UserVO userVo= userDAO.getUserByUserName(this.userName);
            if(userVo == null) return;
            this.userID = userVo.userID;

            User_RoleDAO userRoleDAO = Factory.getUser_RoleDAOInstance();
            List<int> roleIDList = userRoleDAO.getRoleIDListByUserID(this.userID);
            if(roleIDList==null) return;
            foreach (int roleID in roleIDList)
            {
                UserRole userRole = new UserRole();

                userRole.roleID = roleID;
                RoleDAO roleDAO = Factory.getRoleDAOInstance();
                RoleVO roleVo = roleDAO.getRoleByRoleID(roleID);
                if(roleVo == null) continue;
                userRole.roleID = roleVo.roleID;
                userRole.roleName = roleVo.roleName;

                Role_PermissionDAO rolePermissionDAO = Factory.getRole_PermissionDAOInstance();
                List<int> permissionIDList = rolePermissionDAO.getPermissionIDListByRoleID(roleVo.roleID);
                if (permissionIDList == null) continue;
                foreach (int permissionID in permissionIDList)
                {
                    RolePermission rolePermission = new RolePermission();

                    rolePermission.permissionID = permissionID;
                    PermissionDAO permissionDAO = Factory.getPermissionDAOInstance();
                    PermissionVO permissionVo = permissionDAO.getPermissionByPermissionID(permissionID);
                    if (permissionVo == null) continue;
                    rolePermission.PermissionDescription = permissionVo.permissionDescription;

                    userRole.permissions.Add(rolePermission);
                }
                roles.Add(userRole);
            }
        }

        public bool HasPermission(string requirePermission){
            bool bFound = false;
            foreach(UserRole role in this.roles){
                bFound = (role.permissions.Where(
                    p => p.PermissionDescription == requirePermission).ToList().Count > 0);
                if(bFound){
                    break;
                }
            }
            return bFound;
        }

        public bool HasRole(string role){
            return (roles.Where(p=>p.roleName == role).ToList().Count>0);
        }

        public bool HasRoles(string roles){
            bool bFound = false;
            string[] _roles = roles.ToLower().Split(';');
            foreach(UserRole role in this.roles){
                try{
                    bFound = _roles.Contains(role.roleName.ToLower());
                    if(bFound){
                        return bFound;
                    }
                }catch(Exception){
                      
                }
            }
             return bFound;
        }
    }
}