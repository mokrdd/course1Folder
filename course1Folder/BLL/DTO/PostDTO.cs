using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.BLL.DTO
{
    public class PostDTO
    {
        public long Id { get; set; }
        public DateTime LoadDate { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public string LocationName { get; set; }
        public DateTime? PublicateDate { get; set; }
        public bool CurrentUserLiked { get; set; }

        public virtual List<PhotoDTO> Photos { get; set; }
        public virtual List<LikesDTO> Likes { get; set; }
        public virtual List<CommentDTO> Comments { get; set; }
    }
}