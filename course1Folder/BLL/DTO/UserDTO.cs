using System;

namespace course1Folder.BLL.DTO
{
    public class UserDTO
    {
        public UserDTO() { }

        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Nickname { get; set; }
        public System.DateTime RegDate { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Description { get; set; }
        public bool SharedProfile { get; set; }
      
    }
}