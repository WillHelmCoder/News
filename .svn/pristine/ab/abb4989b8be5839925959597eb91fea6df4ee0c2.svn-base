using System;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models
{
    public class SearchByTagViewModel
    {
        [Display(Name = "Tag")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Tag { get; set; }

        [Display(Name = "Desde")]
        public DateTime From { get; set; }
        
        [Display(Name = "Hasta")]
        public DateTime To { get; set; }
    }
}