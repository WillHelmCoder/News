using Dal.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Dal.ViewModels
{
    public class DataViewModel
    {
        public Int32? DataId { get; set; }
        public Int32? DataParentId { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} es requerido")]
        public String Title { get; set; }

        public String Description { get; set; }

        [ValidImage]
        [Display(Name = "Imagen de fondo")]
        public HttpPostedFileBase Image { set; get; }
        public String ImageString { get; set; }

        [ValidImage]
        [Display(Name = "Imagen de sello")]
        public HttpPostedFileBase ImageSign { set; get; }
        public String ImageSignString { get; set; }

        public String Url { get; set; }
        public Int32 Order { get; set; }
        public Int32? MagazineId { get; set; }
    }
}