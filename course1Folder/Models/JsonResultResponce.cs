using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class JsonResultResponce
    {
        public bool Success { get; set; }
        public object Result { get; set; }
    }

    public class JsonResultLikeSub
    {
        public bool Success { get; set; }
        public bool Added { get; set; }
        public object Result { get; set; }
    }
}