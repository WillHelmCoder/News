using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Dal.ApiControllers
{
    [EnableCors("*", "Accept,Origin,Content-Type", "GET,POST,PUT,DELETE")]
    public class UsersController : BaseApiController
    {
        public UsersController(UserService userService, MagazineService restaurantService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService) 
            : base(userService, restaurantService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();

        [Route("api/Users")]
        public IQueryable<User> GetUsersList()
        {
            return db.UsersList;
        }

        // GET: api/Users/5
        public HttpResponseMessage GetUser(int id)
        {
            var user = userService.GetById(id);

            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontro el usuario");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(user));
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        public HttpResponseMessage PostUser([FromBody] RegisterUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Datos inválidos.");
                }

                var user = userService.GetUserbyEmail(model.Email);

                if (user != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email en uso");
                }


                var create = userService.CreateUser_And_MembershipAccount(model.Email, model.Password, "");

                if (create == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se pudo guardar usuario en base de datos");
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Usuario registrado.", UserCode = create.Code, UserId = create.UserId });
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Users/GetCurrentUser/{email}")]
        public HttpResponseMessage GetCurrentUser(string mGuid, string email)
        {
            var user = userService.GetCurrentUserByEmail(email);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(user));
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.UsersList.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.UsersList.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.UsersList.Count(e => e.UserId == id) > 0;
        }
    }
}