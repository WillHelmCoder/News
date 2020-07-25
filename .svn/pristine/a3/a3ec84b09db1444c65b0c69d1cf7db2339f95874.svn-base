using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Dal.Models;
using Dal.Services;
using WebMatrix.WebData;

namespace Dal.ApiControllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET,POST,PUT,DELETE")]
    public class AccountController : BaseApiController
    {
        public AccountController(UserService userService, MagazineService restaurantService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService) 
            : base(userService, restaurantService, resourceService, influencerService, galeryService, listService) { }

        [Route("api/Account/login")]
        public HttpResponseMessage Post([FromBody] ApiLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Datos invalidos");
                }

                if (!WebSecurity.Login(model.Email, model.Password))
                {
                    throw new Exception("Usuario o contraseña incorrectos.");
                }

                var user = userService.GetUserbyEmail(model.Email);

                if (user == null)
                {
                    throw new Exception("No se encontró el usuario.");
                }


                return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Se ha iniciado sesión.", UserCode = user.Code, UserId = user.UserId });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [Route("api/Account/RestaurantLogin")]
        public HttpResponseMessage Post([FromBody] RestaurantLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Datos invalidos");
                }

                var restaurant = magazineService.GetMagazineById(model.Code);


                if (restaurant == null)
                {
                    throw new Exception("No se encontró el restaurant.");
                }


                return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "Se ha iniciado sesión.", RestaurantId = restaurant.MagazineId, RestaurantCode = restaurant.Code, Name = restaurant.Title, Logo = restaurant.Logo });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [Route("api/Account/PasswordRecovery")]
        public HttpResponseMessage Post([FromBody] ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Datos invalidos");
                }

                var user = userService.GetUserbyEmail(model.Email);

                if (user == null)
                {
                    throw new Exception("No se encontró el usuario.");
                }

                var code = Guid.NewGuid().ToString();

                if (!userService.CreateSmartLink(user.Email, code, DateTime.Now.AddHours(1), SmartLinkTypes.PasswordRecovery))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hubo un problema con el envío del email.");
                }

                using (var emailServices = new EmailService())
                {
                    emailServices.SetRecipient(user.Email);

                    emailServices.Password(code, user.Email);

                    if (!emailServices.Send())
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Hubo un problema con el envío del email.");

                    }
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { Message = "El email de recuperación ha sido enviado.", UserCode = user.Code, UserId = user.UserId });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
