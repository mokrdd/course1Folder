using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.BLL.DTO
{
    public class SubDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long FollowingId { get; set; }
        public System.DateTime Date { get; set; }
        public bool isActive { get; set; }
    }
}