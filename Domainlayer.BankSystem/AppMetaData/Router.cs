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

            public const string Edit = Prefix + "/Edit";

            public const string Delete = Prefix + "/Delete" + SignleRoute;

            public const string ChangePassword = Prefix + "/ChangePassword";

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
            public const string GetRoleById = Prefix + "/GetRoleById" + SignleRoute;
            public const string AddRoleToUser = Prefix + "/AddRoleToUser";
            public const string EditRoleToUser = Prefix + "/EditRoleToUser";
            public const string DeleteRoleFromUser = Prefix + "/DeleteRoleFromUser";
            public const string ManageUserRoles = Prefix + "/ManageUserRoles";
            public const string UpdateUserRoles = Prefix + "/UpdateUserRoles";
            public const string ManageUserClaims = Prefix + "/ManageUserClaims" + SignleRoute;
            public const string UpdateUserClaims = Prefix + "/UpdateUserClaims";
        }
        public static class AuthenticationService
        {
            public const string Prefix = Rule + "AuthenticationService";

            public const string GenerateAccessToken = Prefix + "/GenerateAccessToken";
            public const string Logout = Prefix + "/Logout";
            public const string LogoutAll = Prefix + "/LogoutAll";
        }

        public static class BankAccount
        {
            public const string Prefix = Rule + "BankAccount";
            public const string GetAll = Prefix + "/GetAll";
            public const string GetById = Prefix + "/GetById" + SignleRoute;
            public const string GetByUserId = Prefix + "/GetByUserId" + SignleRoute;
            public const string GetByAccountNumber = Prefix + "/GetByAccountNumber/{accountNumber}";
            public const string Create = Prefix + "/Create";
            public const string UpdateAccountType = Prefix + "/UpdateAccountType";
            public const string Delete = Prefix + "/Delete" + SignleRoute;
            public const string GetMyAccounts = Prefix + "/GetMyAccounts";
        }
        public static class Transaction
        {
            public const string Prefix = Rule + "Transaction";
            public const string Transfer = Prefix + "/Transfer";
            public const string Deposit = Prefix + "/Deposit";
            public const string Withdraw = Prefix + "/Withdraw";
        }
    }
}
