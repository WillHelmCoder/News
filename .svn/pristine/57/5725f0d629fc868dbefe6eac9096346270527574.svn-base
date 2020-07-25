using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Dal.Attributes;

namespace Dal.Models
{
    public class MagazineViewModel
    {
        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Title { set; get; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Description { set; get; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Address { set; get; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "{0} es requerida.")]
        public Int32 StateId { set; get; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public Int32 CityId { set; get; }

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Email { set; get; }

        [Display(Name = "Privada")]
        public Boolean IsPrivate { get; set; }

        public String LogoImage { set; get; }

        [ValidImage]
        [Display(Name = "Logotipo")]
        public HttpPostedFileBase Image { set; get; }

        [Display(Name = "Dominio")]
        public String Domain { get; set; }

        public Int32 MagazineId { get; set; }

        //SOCIAL
        public String FacebookAccount { get; set; }
        public String TwitterAccount { get; set; }
        public String GoogleAnlyticsId { get; set; }
        //API
        public String Guid { get; set; }
        public String MainImage { get; set; }
    }
}