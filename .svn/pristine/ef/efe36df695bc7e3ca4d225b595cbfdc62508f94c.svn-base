using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models
{
    public class MediaViewModel
    {
        public Int32 MediaId { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Nombre")]
        public String Name { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Correo electrónico")]
        public String Email { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}