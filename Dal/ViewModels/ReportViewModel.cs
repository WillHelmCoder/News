using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models
{
    public class ReportViewModel
    {
        public Int32 ReportId { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Articulo")]
        public String Title { get; set; }

        [Display(Name = "Positivo")]
        public Boolean IsPositive { get; set; }

        [Display(Name = "Pagado")]
        public Boolean IsPaid { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Impacto")]
        public Int32 Impact { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Comentarios")]
        public String Comments { get; set; }

        [Required(ErrorMessage = "{0} es requerido.")]
        [Display(Name = "Medio")]
        public Int32 MediaId { get; set; }
        public Media Media { get; set; }

        public String MediaName { get; set; }
        
        
        public Int32? MagazineId { get; set; }
        public Magazine Magazine { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}