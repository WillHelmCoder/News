using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Dal.Models;
using Dal.Interfaces;
using Dal.Repositories;
using WebMatrix.WebData;

namespace Dal.Services
{
    public class UserService : BaseService
    {
        public UserService(Repository repository, ILog logger)
            : base(repository, logger)
        {
        }


        public Boolean CreateSmartLink(String email, String code, DateTime expirationDate, SmartLinkTypes type)
        {
            var smartLinkSearch = Repository.SmartLinks()
                .Where(x => !x.ActivatonDate.HasValue)
                .Where(x => x.ExpirationDate > DateTime.Now)
                .Where(x => x.SmartLinkType == SmartLinkTypes.PasswordRecovery)
                .SingleOrDefault(x => x.Code.Equals(code) &&
                    x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (smartLinkSearch != null)
            {
                smartLinkSearch.ExpirationDate = DateTime.Now;

                Repository.Save();
            }

            var smartLink = new SmartLink
            {
                Code = code,

                Email = email,

                CreationDate = DateTime.Now,

                ExpirationDate = expirationDate,

                SmartLinkType = type,

                ActivatonDate = null,
            };

            try
            {
                Repository.Add(smartLink);
                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                //log

                ServiceModelState.AddModelError("", ex.Message);

                return false;
            }
        }

        public Boolean UserInRol(Int32 UserId, Int32 rol)
        {
            var userInRol = new UserInRolModel
            {
                UserId = UserId,
                Rol = rol,
            };

            try
            {
                Repository.Add(userInRol);
                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                //log

                ServiceModelState.AddModelError("", ex.Message);

                return false;
            }
        }

        public User CreateUser_And_MembershipAccount(String email, String password, String name)
        {
            try
            {
                var user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user != null)
                {
                    ServiceModelState.AddModelError("", "Email en uso");
                    return null;
                }

                var code = Guid.NewGuid().ToString();

                try
                {
                    WebSecurity.CreateUserAndAccount(email, password, new
                    {
                        Email = email,
                        Code = code,
                        CreateDate = DateTime.Now.ToString(),
                        ActivationDate = DateTime.Now,
                        UserName = name,
                        StageId = 0,
                        Stage = PaymentStages.Nuevo
                    });

                    user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                    //if (!SendEmailConfirmation(user))
                    //{
                    //    ServiceModelState.AddModelError("", "No se pudo crear usuario debido a que no se pudo enviar correo de validacion");
                    //    return null;
                    //}

                }
                catch (MembershipCreateUserException e)
                {

                    ServiceModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }

        public User CreateAdmin(String email, String password)
        {
            try
            {
                var user = Repository.Users()
              .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));


                if (user != null)
                {
                    ServiceModelState.AddModelError("", "Email en uso");
                    return null;
                }

                var code = Guid.NewGuid().ToString();

                try
                {
                    WebSecurity.CreateUserAndAccount(email, password, new
                    {
                        Email = email,
                        Code = code,
                        CreateDate = DateTime.Now,
                        UserRol = 1,
                    });

                    user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));



                    if (!SendEmailConfirmation(user))
                    {
                        ServiceModelState.AddModelError("", "No se pudo crear usuario debido a que no se pudo enviar correo de validacion");
                        return null;
                    }

                }
                catch (MembershipCreateUserException e)
                {

                    ServiceModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }

        public Boolean UpdateUserPaymentToken(String token)
        {
            try
            {
                var user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    SetMessage("No se encontró el usuario.", BootstrapAlertTypes.Danger);
                    return false;
                }

                user.PayPalToken = token;

                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message, BootstrapAlertTypes.Danger);
                return false;
            }
        }

        public Boolean SendEmailConfirmation(User user)
        {
            try
            {
                using (var emailService = new EmailService())
                {
                    emailService.SetRecipient(user.Email);

                    emailService.Validate(user.Code, user.Email);

                    if (!emailService.Send())
                    {
                        DeleteAccount(user.Email);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public Boolean DeleteAccount(string email)
        {
            try
            {
                if (Roles.GetRolesForUser(email).Count() > 0)
                {
                    Roles.RemoveUserFromRoles(email, Roles.GetRolesForUser(email));
                }
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(email); // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(email, true); // deletes record from UserProfile table

                return true;
            }
            catch
            {
                return false;
            }


        }

        public User GetCurrentUser()
        {
            var item = Repository.Users()
                .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase));

            return item;
        }

        public User GetCurrentUserByEmail(string email)
        {
            var item = Repository.Users()
                .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return item;
        }

        public Boolean AsignProfilePicture(String imageCode)
        {
            try
            {
                var currentUser = GetCurrentUser();

                currentUser.ImageProfile = imageCode;

                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", "Error.");
                return false;
            }
        }

        public Boolean UpdateInfo(String name, String email)
        {
            try
            {
                var alreadyUsed = Repository.Users().SingleOrDefault(x => x.Email.Equals(email));

                var currentUser = GetCurrentUser();

                if (alreadyUsed != null && currentUser != alreadyUsed)
                    return false;

                currentUser.Email = email;
                currentUser.UserName = name;

                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", "Error.");
                return false;
            }
        }

        public User GetUserbyEmail(String email)
        {
            var user = Repository.Users()
                .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            return user;
        }

        public User GetRolUser()
        {
            var rol = Repository.Users()
                         .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase));

            return rol;
        }

        public User GetById(Int32 id)
        {
            var user = Repository.Users()
                .SingleOrDefault(x => x.UserId == id);

            return user;
        }

        public SmartLink GetPasswordRecoveryLink(String code)
        {
            return Repository.SmartLinks()
                .Where(x => !x.ActivatonDate.HasValue)
                .Where(x => x.ExpirationDate > DateTime.Now)
                .SingleOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
        }

        public Boolean ActivateUser(String userCode)
        {
            try
            {
                var user = Repository.Users()
                    .SingleOrDefault(x => x.Code.Equals(userCode, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    ServiceModelState.AddModelError("", "Enlace de validación inválido.");
                    return false;
                }

                if (user.ActivationDate.HasValue)
                {
                    ServiceModelState.AddModelError("", "Usuario validado previamente.");
                    return false;
                }

                user.ActivationDate = DateTime.Now;
                Repository.Save();



                SetMessage("Bienvenido " + user.Email + ". Ya puedes iniciar sesión.", BootstrapAlertTypes.Success);
                return true;
            }
            catch (Exception ex)
            {
                //log

                ServiceModelState.AddModelError("", "Error.");

                return false;
            }
        }

        public Boolean ConsumePasswordRecoverySmartLink(String code)
        {
            var smartLinkSearch = Repository.SmartLinks()
                .Where(x => !x.ActivatonDate.HasValue)
                .Where(x => x.ExpirationDate > DateTime.Now)
                .Where(x => x.SmartLinkType == SmartLinkTypes.PasswordRecovery)
                .SingleOrDefault(x => x.Code.Equals(code));

            if (smartLinkSearch == null)
            {
                ServiceModelState.AddModelError("", "Error");

                return false;
            }

            smartLinkSearch.ExpirationDate = DateTime.Now;

            var user = Repository.Users()
                .SingleOrDefault(x => x.Email.Equals(smartLinkSearch.Email));

            if (user == null)
            {
                ServiceModelState.AddModelError("", "Error");

                return false;
            }

            try
            {
                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                //log

                ServiceModelState.AddModelError("", ex.Message);

                return false;
            }
        }

        private static String ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for a full list of status codes.

            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "E-mail no disponible.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "E-mail no disponible.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Contraseña invalida.";

                case MembershipCreateStatus.InvalidEmail:
                    return "E-mail invalido.";

                case MembershipCreateStatus.InvalidUserName:
                    return "E-mail invalido.";

                default:
                    return "Ha sucedido un error inesperado al procesar tu petición.";
            }
        }

        public Boolean UserInMagazine(Int32 magId, Int32 usrId)
        {
            var relation = Repository.UserMagazines()
                .Where(x => x.MagazineId == magId)
                .Where(x => x.UserId == usrId)
                .SingleOrDefault();

            if (relation != null) { return true; } else { return false; }
        }
    }
}