using System;
using System.Web.Mvc;
namespace Dal.Classes
{
    public class Validator
    {
        public Validator(ModelStateDictionary modelState)
        {
            ModelState = modelState;
        }

        private ModelStateDictionary ModelState;

        public void Name(String name)
        {
            if (String.IsNullOrEmpty(name)) ModelState.AddModelError("", "Nombre no puede ser nulo.");

        }
        public void Email(String email)
        {
            if (String.IsNullOrEmpty(email)) ModelState.AddModelError("", "Email no puede ser nulo.");
            //TODO VALIDATE EMAIL
        }
        public void WebSite(String webSite)
        {
            if (String.IsNullOrEmpty(webSite)) ModelState.AddModelError("", "Dirección URL inválida.");

            Uri uri;
            if (!Uri.TryCreate(webSite, UriKind.Absolute, out uri)) ModelState.AddModelError("", "Dirección URL invalida.");
        }
        public void Description(String description)
        {
            if (String.IsNullOrEmpty(description)) ModelState.AddModelError("", "Descripción no puede ser nula.");
        }
        public void Title(String title)
        {
            if (String.IsNullOrEmpty(title)) ModelState.AddModelError("", "Titulo no puede ser nulo.");
        }
    }
}