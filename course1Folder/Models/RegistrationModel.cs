using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace course1Folder.Models
{
    public class RegistrationModel
    {
        [Required]
        [Display(Name = "Логин")]
        [EmailAddress(ErrorMessage = "Неверный формат")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordRetry { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        public bool isShared { get; set; }
    }
}