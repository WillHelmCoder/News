using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dal.Models
{
        public class PasswordChangeViewModel
        {
            [DataType(DataType.Password)]
            [StringLength(15, ErrorMessage = "Debe tener entre {2} y {1} caracteres", MinimumLength = 6)]
            [Display(Name = "Contraseña")]
            [Required(ErrorMessage = "{0} es requerido.")]
            public String Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            public String ConfirmPassword { get; set; }

            [Required]
            public String Code { set; get; }
        }
}