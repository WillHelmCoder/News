using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models
{
    public class Advertise
    {
        [Display(Name = "Id")]
        public int AdvertiseId { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Imagen")]
        public string Image { get; set; }

        [Display(Name = "URL destino")]
        public string Url { get; set; }

        [Display(Name = "Fuente")]
        public string Source { get; set; }

        [Display(Name = "Medio")]
        public string Medium { get; set; }

        [Display(Name = "Nombre del campana")]
        public string Campaign { get; set; }

        [Display(Name = "Tags")]
        public string Term { get; set; }

        [Display(Name = "Contenido")]
        public string Content { get; set; }

        [Display(Name = "IFrame")]
        public string IFrame { get; set; }

        [Display(Name = "Fecha creado")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Orientacion")]
        public bool Horizontal { get; set; }

        [Display(Name = "Eliminado")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Grupo")]
        public int? AdCategoryId { get; set; }
        public virtual AdCategory AdCategory { get; set; }
    }

    public class AdCategory
    {
        [Display(Name = "Id")]
        public int AdCategoryId { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Tags")]
        public string Tags { set; get; }

        [Display(Name = "Fecha inicio")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha fin")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        [Display(Name = "Eliminado")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Revista")]
        public int MagazineId { get; set; }
        public virtual Magazine Magazine { get; set; }

        [Display(Name = "Anuncios")]
        public virtual List<Advertise> Ads { get; set; }
    }
}
