using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Threading;

namespace Dal.Services
{
    public class EmailService : IDisposable
    {
        protected String CurrentUserEmail = Thread.CurrentPrincipal.Identity.Name;
        private String Recipient { get; set; }
        private String EmailBody { get; set; }
        private String Subject { get; set; }
        private String Sender { set; get; }
        private String Domain { set; get; }
        private String Title { set; get; }

        private String Header { set; get; }
        private String Footer { set; get; }

        public EmailService()
        {
            EmailBody = String.Empty;

            Sender = "The Content";
            Domain = HttpContext.Current.Request.IsLocal ? "http://localhost:35786/" : "http://thecontent.mx/";

            Header = "<br />";
            Footer = "<br /><br />";
        }

        public void SetRecipient(String email)
        {
            Recipient = email;
        }

        public Boolean Send()
        {
            try
            {
                const string credential = "hola@thecontent.mx";
                const string pass = "Drakkar77!";
                const string host = "132.148.22.245";
                const int port = 25;

                var RemoteMail = new System.Net.Mail.MailMessage();

                RemoteMail.Bcc.Add(Recipient);
                RemoteMail.Priority = System.Net.Mail.MailPriority.High;
                RemoteMail.From = new System.Net.Mail.MailAddress("no-reply@thecontent.mx", Sender);

                RemoteMail.Body = EmailBody;
                RemoteMail.Subject = Subject;
                RemoteMail.IsBodyHtml = true;

                var Client = new System.Net.Mail.SmtpClient(host, port);

                Client.EnableSsl = false;
                Client.UseDefaultCredentials = false;
                Client.Credentials = new NetworkCredential(credential, pass);
                Client.Send(RemoteMail);

                return true;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return false;
            }
        }

        private String GetTemplateHtml(String templateName)
        {
            var templatePath = HttpContext.Current.Server.MapPath("~/content/templates/" + templateName);

            var templateText = String.Empty;

            using (var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                var streamReader = new StreamReader(fileStream);
                while (!streamReader.EndOfStream) templateText += streamReader.ReadLine();
            }

            return String.Format("{0}{1}{2}",
                Header.Replace("{MailTitle}", Title),
                templateText,
                Footer);
        }

        public void HtmlBody(String sender, String subject, String body)
        {
            Sender = sender;
            Subject = subject;
            EmailBody = body;
        }
        public void Contacto(String Name, String Email, String Comments)
        {
            Subject = "Mensaje de Contacto";

            var templateText = GetTemplateHtml("Contacto.html");


            templateText = templateText
                .Replace("{Comentario}", Comments)
                .Replace("{Email}", Email)
                .Replace("{Name}", Name);

            EmailBody = templateText;
        }

        public void Password(String code, String userName)
        {
            Subject = "Solicitud para cambio de contraseña";

            var link = Domain + "cambio-de-contrasena/" + code;

            var templateText = GetTemplateHtml("Password.html");

            templateText = templateText
                .Replace("{UserName}", userName)
                .Replace("{Link}", link);

            EmailBody = templateText;
        }

        public void Validate(String code, String email)
        {
            Subject = "Validación de cuenta";

            var link = Domain + "validar-usuario/" + code;

            if (Roles.IsUserInRole(CurrentUserEmail, "Admin") || Roles.IsUserInRole(CurrentUserEmail, "SuperAdmin"))
            {
                var templateText = GetTemplateHtml("AdminValidate.html");

                templateText = templateText
                    .Replace("{Link}", link);

                EmailBody = templateText;
            }
            else
            {
                var templateText = GetTemplateHtml("Validate.html");

                templateText = templateText
                    .Replace("{Link}", link);

                EmailBody = templateText;
            }
        }

        public void Dummy(String subject, String message, String email)
        {
            Subject = subject;
            Title = MvcHtmlString.Create("Mensaje de Ezpendo").ToString();

            EmailBody =
                Header.Replace("{MailTitle}", Title) +
                String.Format("<p>Email involucrado: {0}</p><br /><p>{1}</p>", email, message) +
                Footer;
        }

        public void Dispose()
        {
            EmailBody = null;
            Subject = null;
            Sender = null;
            Recipient = null;

            //GC.SuppressFinalize(this); ????????????????????
        }
    }
}