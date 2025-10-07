using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.AppMetaData
{
    public static class Router
    {

        public const string SignleRoute = "/{id}";

        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";

        public static class ApplicationUser
        {

            public const string Prefix = Rule + "User";

            public const string Create = Prefix + "/Create";

            public const string Edit = Prefix + "/Edit" ;

            public const string Delete = Prefix + "/Delete" + SignleRoute;

            public const string ChangePassword = Prefix + "/ChangePassword" ;

            public const string GetUserById = Prefix + "/GetById" + SignleRoute;

            public const string SignIn = Prefix + "/SignIn";
           
            public const string GetUserPaginatedList = Prefix + "/GetUserPaginatedList";
        }
        public static class AuthorizationService
        {
            public const string Prefix = Rule + "AuthorizationService";

            public const string Create = Prefix + "/Create";
            public const string EditRole = Prefix + "/EditRole";
            public const string DeleteRole = Prefix + "/DeleteRole";
            public const string GetRoles = Prefix + "/GetRoles";
            public const string IsRoleExist = Prefix + "/IsRoleExist";
            public const string GetRoleById = Prefix + "/GetRoleById"+ SignleRoute;
            public const string AddRoleToUser = Prefix + "/AddRoleToUser";
            public const string EditRoleToUser = Prefix + "/EditRoleToUser";
            public const string DeleteRoleFromUser = Prefix + "/DeleteRoleFromUser";
            public const string ManageUserRoles = Prefix + "/ManageUserRoles";
            public const string UpdateUserRoles = Prefix + "/UpdateUserRoles";
        }
        public static class AuthenticationService
        {
            public const string Prefix = Rule + "AuthenticationService";

            public const string GenerateAccessToken = Prefix + "/GenerateAccessToken";
        }
    }
}
