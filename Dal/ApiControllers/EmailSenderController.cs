using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Dal.Repositories;
using Dal.Services;


namespace Dal.ApiControllers
{
    [EnableCors("*", "Accept,Origin,Content-Type", "GET,POST,PUT,DELETE")]
    public class EmailSenderController : BaseApiController
    {
        public EmailSenderController(UserService userService, MagazineService restaurantService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
            : base(userService, restaurantService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();



        [System.Web.Http.Route("api/EmailSender/Send")]
        public HttpResponseMessage SendEmail(SendMailModel model)
        {
            using (var emailServices = new EmailService())
            {
                emailServices.SetRecipient(model.To);

                emailServices.HtmlBody(model.FromName, model.Subject, model.Body);

                if (!emailServices.Send())
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hubo un problema con el envío del email.");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.OK, "Correo enviado");
        }

        public class SendMailModel
        {
            public String From { set; get; }
            public String FromName { set; get; }
            public String To { set; get; }
            public String Subject { set; get; }
            public String Body { set; get; }
            public Boolean isHtml { set; get; }

        }
    }
}
