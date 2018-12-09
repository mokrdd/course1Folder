using System;

namespace course1Folder.BLL.DTO
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public System.DateTime Date { get; set; }
        public string DateString { get; set; }
        public string ContentText { get; set; }
        public string UserNick { get; set; }
    }
}