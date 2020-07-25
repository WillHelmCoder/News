using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class ComentarioViewModel
    {
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Comentario { get; set; }

        public Int32 NewsId { get; set; }
    }
}