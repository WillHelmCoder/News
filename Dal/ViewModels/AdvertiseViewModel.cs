using Dal.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Dal.ViewModels
{
    public class AdvertiseViewModel
    {
        public int? AdvertiseId { get; set; }
        public int AdCategoryId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es requerido")]
        public string Name { get; set; }

        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Display(Name = "Url destino")]
        public string Url { get; set; }

        [Display(Name = "Fuente")]
        public string Source { get; set; }

        [Display(Name = "Medio")]
        public string Medium { get; set; }

        [Display(Name = "Nombre de la campaña")]
        public string Campaign { get; set; }

        [Display(Name = "Término")]
        public string Term { get; set; }

        [AllowHtml]
        [Display(Name = "iframe")]
        public string IFrame { get; set; }

        public string ImageString { get; set; }

        [ValidImage]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase Image { set; get; }

        [Display(Name = "Posición horizontal")]
        public bool Horizontal { get; set; }
    }
}
