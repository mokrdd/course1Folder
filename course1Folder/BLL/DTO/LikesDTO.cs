using System;

namespace course1Folder.BLL.DTO
{
    public class LikesDTO
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public System.DateTime Date { get; set; }
        public long UserId { get; set; }
        public UserDTO User { get; set; }
    }
}