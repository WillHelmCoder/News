using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;

namespace Dal.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Es necesario ingresar su nombre")]

        public String Name { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese su correo")]
        [Email(ErrorMessage = "Ingresa un Email valido")]
        public String Email { get; set; }


        [Required(ErrorMessage = "Ingrese algun comentario porfavor")]
        [MinLength(10, ErrorMessage = "Es necesario que ingrese al menos 10 caracteres")]
        public String Comments { get; set; }

    }
}