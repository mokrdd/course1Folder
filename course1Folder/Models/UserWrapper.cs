using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class UserWrapper
    {
        public byte[] AvatarContent { get; set; }
        public string AvatarMime { get; set; }
        public long Id { get; set; }
        public string Nickname { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public bool SharedProfile { get; set; }
        public long SubscribersCount { get; set; }
        public bool CurrUserSubed { get; set; }
    }

    public class SubModel
    {
        public long Id { get; set; }
        public long FollowingUserId { get; set; }
        public string FollowingUserNickname { get; set; }
    }

    public class SubsPanelModel
    {
        public List<SubModel> Subs { get; set; }
    }
}