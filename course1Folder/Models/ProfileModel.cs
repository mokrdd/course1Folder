using course1Folder.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class ProfileModel
    {
        public UserModel User { get; set; }
        public SubsPanelModel SubsPanel { get; set; }
        public FeedModel UserPosts { get; set; }
    }

    public class UserModel
    {
        public UserModel() { }

        public UserModel(BLL.DTO.UserDTO dbUser)
        {
            Id = dbUser.Id;
            Login = dbUser.Login;
            Nickname = dbUser.Nickname;
            Description = dbUser.Description;
            SharedProfile = dbUser.SharedProfile;

        }

        public long Id { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }
        public string Description { get; set; }
        public bool SharedProfile { get; set; }

    }
}