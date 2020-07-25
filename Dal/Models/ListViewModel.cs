using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dal.Models
{
    public class ListViewModel
    {
        [Display(Name = "Id")]
        public int ItemListId { set; get; }

        [Display(Name = "Nombre")]
        public string Name { set; get; }

        [Display(Name = "Contenido")]
        public string Content { set; get; }

        [Display(Name = "Eliminado")]
        public bool IsDeleted { set; get; }

        [Display(Name = "Sitio")]
        public int MagazineId { set; get; }
        public virtual Magazine Magazine { set; get; }

        [Display(Name = "Elementos")]
        public List<ItemList> ItemsList { set; get; }
    }
}