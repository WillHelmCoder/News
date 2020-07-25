using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dal.Attributes;

namespace Dal.Models
{
    public class CategoryViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Name { set; get; }

        [Display(Name = "Permalink")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public String Permalink { set; get; }

        [Display(Name = "Revista")]
        [Required(ErrorMessage = "{0} es requerido.")]
        public Int32 MagazineId { set; get; }

        public String LogoImage { set; get; }

        [ValidImage]
        [Display(Name = "Logotipo")]
        public HttpPostedFileBase Image { set; get; }

        //HANDLEN ON THE BACK (ALWAYS HIDDEN)
        public Int32 CategoryId { get; set; }

        [Display(Name = "Medidas")]
        public String Medidas { set; get; }
        [Display(Name = "Ancho")]
        public Int32 Width { set; get; }
        [Display(Name = "Alto")]
        public Int32 Height { set; get; }

        [Display(Name = "Categoria padre")]
        public Int32? ParentCategoryId { set; get; }

        public List<Category> ParentsCategories { set; get; }
        [Display(Name = "¿Quieres mostrar imagen de portade en tus articulos?")]
        public bool? showImage { set; get; }
    }
}