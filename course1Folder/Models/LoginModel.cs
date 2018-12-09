using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class LoginModel
    {
        [Required]
        public string login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    public class CustomSerializeModel
    {
        public long userId { get; set; }
        public string nick { get; set; }
        public string email { get; set; }

    }
}