using System;

namespace course1Folder.BLL.DTO
{
    public class PhotoDTO
    {
        public long Id { get; set; }
        public System.DateTime LoadDate { get; set; }
        public long UserId { get; set; }
        public long? PostId { get; set; }
    }
}