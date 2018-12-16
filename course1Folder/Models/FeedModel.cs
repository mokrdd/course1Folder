using course1Folder.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class FeedModel
    {
        public bool NextExist { get; set; }

        public List<PostDTO> Posts { get; set; }
    }

    public enum sortMode { newest, liked };

}