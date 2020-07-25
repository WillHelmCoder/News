using Dal.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Dal.Models
{
    public class NewsViewModel
    {
        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Title { set; get; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Description { set; get; }

        [AllowHtml]
        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Body { set; get; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public Int32 CategoryId { set; get; }

        public String LogoImage { set; get; }

        [ValidImage]
        [Display(Name = "Imagen principal")]
        public HttpPostedFileBase Image { set; get; }

        [Display(Name = "Alt de imagen")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Alt { set; get; }

        [Display(Name = "Meta Descripción")]
        public String MetaDesc { set; get; }

        [Display(Name = "Keywords o Tags")]
        public String MetaTags { set; get; }

        [Display(Name = "Permalink")]
        [Required(ErrorMessage = "{0} es requerido")]
        public String Permalink { set; get; }

        [AllowHtml]
        [Display(Name = "URL de video")]
        public String VideoEmbed { set; get; }

        [Display(Name = "Fecha de publicación")]
        public String CreationDate { set; get; }

        public Int32 NewsId { set; get; }
        public Boolean IsClon { get; set; }
        public String ThankNote { get; set; }
        public String MainImage { get; set; }
        public String Thumbnail { get; set; }
    }
}