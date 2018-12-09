using course1Folder.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace course1Folder.CustomAuthorization
{
    public class CustomMembershipUser : MembershipUser
    {

        #region User Properties

        public long userId { get; set; }
        public string nickname { get; set; }

        #endregion

        public CustomMembershipUser(UserDTO user) : base(
            "CustomMembership",
            user.Login,
            user.Id,
            user.Login,
            string.Empty,
            string.Empty,
            true,
            false,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now)
        {
            userId = user.Id;
            nickname = user.Nickname;
        }
    }
}