using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class UserModel
    {
        public Int32 Id { get; set; }


        [Required]
        [EmailAddress]
        public String Email { get; set; }

        public String Code { get; set; }

    }

    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    
    }
}