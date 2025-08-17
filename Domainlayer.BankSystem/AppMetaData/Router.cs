﻿using System;
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
        }


    }
}
