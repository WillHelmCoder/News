using System.Web.Mvc;
using System.Web.Routing;

namespace Dal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region administrador
            #region Revistas
            routes.MapRoute(
                "Magazines_Index",
                "administrador/sitios",
                new { controller = "Magazines", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Magazines_Create",
                "administrador/sitios/crear",
                new { controller = "Magazines", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Magazines_Delete",
                "administrador/sitios/eliminar/{id}",
                new { controller = "Magazines", action = "Delete", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Magazines_Edit",
                "administrador/sitios/editar/{id}",
                new { controller = "Magazines", action = "Edit", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/top-cien-influencer",
                new { controller = "Magazines", action = "TopHundred" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Noticias
            routes.MapRoute(
                null,
                "administrador/revista/{id}",
                new { controller = "News", action = "MagazineNews", magazineId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/crear-noticia",
                new { controller = "News", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
               null,
               "administrador/mis-noticias",
               new { controller = "News", action = "MyNews" },
               new[] { "Dal.Controllers" }
           );
            routes.MapRoute(
                null,
                "administrador/editar-noticia/{id}",
                new { controller = "News", action = "Edit", newsId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/borrar-noticia/{id}",
                new { controller = "News", action = "Delete", newsId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/estadisticas-de-noticia/{id}",
                new { controller = "News", action = "ViewNewStatistics", newsId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/mis-estadisticas",
                new { controller = "News", action = "NewsStatistics" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/clonar-noticia/{catId}/{newId}",
                new { controller = "News", action = "CloneNew" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "get-comments/{id}",
                new { controller = "Home", action = "GetComments", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "add-comment/{id}/{comment}",
                new { controller = "Home", action = "AddComment", id = UrlParameter.Optional, comment = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Categorias
            routes.MapRoute(
                null,
                "administrador/mis-categorias",
                new { controller = "Categories", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/crear-categoria",
                new { controller = "Categories", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/borrar-categoria/{id}",
                new { controller = "Categories", action = "Delete", categoryId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/editar-categoria/{id}",
                new { controller = "Categories", action = "Edit", categoryId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Datos
            routes.MapRoute(
                "KeyPoints_Index",
                "administrador/KeyPoint",
                new { controller = "KeyPoint", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "KeyPoints_Create",
                "administrador/KeyPoint/crear",
                new { controller = "KeyPoint", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "KeyPoints_Delete",
                "administrador/KeyPoint/borrar/{id}",
                new { controller = "KeyPoint", action = "Delete", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "KeyPoints_Edit",
                "administrador/KeyPoint/editar/{id}",
                new { controller = "KeyPoint", action = "Edit", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Galerias
            routes.MapRoute(
                "Galery_Index",
                "administrador/galerias",
                new { controller = "Galeries", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Galery_Create",
                "administrador/galerias/crear",
                new { controller = "Galeries", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Galery_Delete",
                "administrador/galerias/eliminar/{id}",
                new { controller = "Galeries", action = "Delete", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Galery_Edit",
                "administrador/galerias/editar/{id}",
                new { controller = "Galeries", action = "Edit", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Galery_Images_Index",
                "administrador/galerias/imagenes/{id}",
                new { controller = "Galeries", action = "Add", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Galery_Images_Delete",
                "administrador/galerias/imagenes/eliminar/{id}",
                new { controller = "Galeries", action = "DeleteGaleryImage", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Clonar
            routes.MapRoute(
                null,
                "administrador/replicar-contenido",
                new { controller = "Clone", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Slider
            routes.MapRoute(
                null,
                "administrador/mi-slider/",
                new { controller = "Sliders", action = "Index", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/agregar-a-slider/{id}",
                new { controller = "Sliders", action = "Create", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/borrar-de-slider/{id}",
                new { controller = "Sliders", action = "Delete", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/editar-slider/{id}",
                new { controller = "Sliders", action = "Edit", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Influencers
            routes.MapRoute(
                null,
                "administrador/compartir-en-redes",
                new { controller = "Influencer", action = "ListOfNews" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/top-cien-influencers",
                new { controller = "Influencer", action = "TopHundred" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/mis-influencers",
                new { controller = "Influencer", action = "GetMyInfluencers" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/mi-influencer/{id}",
                new { controller = "Influencer", action = "Stadistics", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/mis-redactores",
                new { controller = "Influencer", action = "GetMyEditors" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/mi-redactor/{id}",
                new { controller = "Influencer", action = "EditorDetails", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/ver-influencer/{id}",
                new { controller = "Influencer", action = "getNews" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Wizard
            routes.MapRoute(
                null,
                "administrador/mi-primer-noticia",
                new { controller = "Wizard", action = "FirstNew" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/wizard/crear-categoria/{id}",
                new { controller = "Wizard", action = "CreateCategory", magazineId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/wizard/crear-noticia/{id}",
                new { controller = "Wizard", action = "CreateNew", categoryId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region TrendsRelated
            routes.MapRoute(
                null,
                "administrador/trends-de-hoy",
                new { controller = "Admin", action = "Trends" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region MediaRelated
            routes.MapRoute(
                null,
                "administrador/medios-actuales",
                new { controller = "Media", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/crear-medio",
                new { controller = "Media", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/editar-medio/{id}",
                new { controller = "Media", action = "Edit", mediaId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region ReportRelated
            routes.MapRoute(
                null,
                "administrador/mis-reportes",
                new { controller = "Reports", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/crear-reporte",
                new { controller = "Reports", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/editar-reporte/{id}",
                new { controller = "Reports", action = "Edit", reportId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/borrar-reporte/{id}",
                new { controller = "Reports", action = "Delete", reportId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region AdminRelated
            routes.MapRoute(
                null,
                "administrador/invitar-a-revista",
                new { controller = "Admin", action = "Register" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region Advertises
            routes.MapRoute(
                "Advertises_Index",
                "administrador/ads",
                new { controller = "Advertises", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Advertises_Create",
                "administrador/ads/crear",
                new { controller = "Advertises", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Advertises_Delete",
                "administrador/ads/borrar/{id}",
                new { controller = "Advertises", action = "Delete", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Advertises_Edit",
                "administrador/ads/editar/{id}",
                new { controller = "Advertises", action = "Edit", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Advertises_Details",
                "administrador/ads/detalles/{id}",
                new { controller = "Advertises", action = "Details", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region AdGroup
            routes.MapRoute(
                "AdCategories_Index",
                "administrador/campanas",
                new { controller = "AdCategories", action = "Index" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "AdCategories_Create",
                "administrador/campanas/crear",
                new { controller = "AdCategories", action = "Create" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "AdCategories_Delete",
                "administrador/campanas/borrar/{id}",
                new { controller = "AdCategories", action = "Delete", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "AdCategories_Edit",
                "administrador/campanas/editar/{id}",
                new { controller = "AdCategories", action = "Edit", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "AdCategories_Details",
                "administrador/campanas/detalles/{id}",
                new { controller = "AdCategories", action = "Details", id = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion

            #region List
            routes.MapRoute(
                null,
                "administrador/listas",
                new { controller = "ItemLists", action = "Index", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/listas/crear",
                new { controller = "ItemLists", action = "Create", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/listas/eliminar/{id}",
                new { controller = "ItemLists", action = "Delete", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "administrador/listas/editar/{id}",
                new { controller = "ItemLists", action = "Edit", sliderId = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #endregion
            #region VotingSystemRelated
            routes.MapRoute(
                null,
                "vote-up/{Id}/{type}",
                new
                {
                    controller = "News",
                    action = "VoteUp",
                    Id = UrlParameter.Optional,
                    type = UrlParameter.Optional
                },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "vote-down/{Id}/{type}",
                new
                {
                    controller = "News",
                    action = "VoteDown",
                    Id = UrlParameter.Optional,
                    type = UrlParameter.Optional
                },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "get-votes/{Id}",
                new
                {
                    controller = "News",
                    action = "GetNewsVotes",
                    Id = UrlParameter.Optional
                },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "get-comment-votes/{Id}",
                new
                {
                    controller = "News",
                    action = "GetCommentVotes",
                    Id = UrlParameter.Optional
                },
                new[] { "Dal.Controllers" }
            );
            #endregion
            #region GeneralStuff
            routes.MapRoute(
                null,
                "noticia/{id}/{social}/{idInfluencer}",
                new { controller = "Home", action = "Noticia", idInfluencer = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "categoria/{id}/{social}/",
                new { controller = "Home", action = "Categories", idInfluencer = UrlParameter.Optional },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "buscar-fecha/{fecha}/",
                new { controller = "Home", action = "Buscarporfecha" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                null,
                "Influencer/Stadistics/{idInfluencer}",
                new { controller = "Influencer", action = "stadistics", idInfluencer = UrlParameter.Optional },
                new[] { "Influencer.Controllers" }
            );
            routes.MapRoute(
                null,
                "Influencer/Stadistics/{idInfluencer}",
                new { controller = "Influencer", action = "stadistics", idInfluencer = UrlParameter.Optional },
                new[] { "Influencer.Controllers" }
            );
            #endregion
            #region Account
            routes.MapRoute(
                "Iniciar-sesion",
                "inicia-sesion",
                new { controller = "Account", action = "Login" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Registro",
                "registro",
                new { controller = "Account", action = "Register" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Cambio_Contrasena",
                "cambio-de-contrasena/{code}",
                new { controller = "Account", action = "PasswordChange" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Validar_Usuario",
                "validar-usuario/{code}",
                new { controller = "Account", action = "ValidateUser" },
                new[] { "Dal.Controllers" }
            );
            routes.MapRoute(
                "Recuperar_Contrasena",
                "recuperar-contrasena",
                new { controller = "Account", action = "ForgotPassword" },
                new[] { "Dal.Controllers" }
            );
            #endregion
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}