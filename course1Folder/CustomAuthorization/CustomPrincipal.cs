using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace course1Folder.CustomAuthorization
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties
        public long UserId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        #endregion

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public IIdentity Identity { get; private set; }


        public bool IsInRole(string role)
        {
            return true;
        }


    }
}