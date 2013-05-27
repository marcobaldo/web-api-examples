using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using api.Models;
using api.Models.Forms;
using api.Models.Security;

namespace api.Controllers
{
    public class SessionsController : ApiController
    {
        [HttpGet]
        public object Status()
        {
            if (User.Identity.IsAuthenticated)
            {
                return new { Principal = UserToSerializable(GetUser(User.Identity.Name)), IsAuthenticated = true };
            }

            return new { IsAuthenticated = false };
        }

        [HttpPost]
        public object Login(LoginModel credentials)
        {
            if (User.Identity.IsAuthenticated)
            {
                return new { Principal = UserToSerializable(GetUser(User.Identity.Name)), IsAuthenticated = true };
            }

            if (ModelState.IsValid)
            {
                User user;
                if (ValidateUser(credentials.Username, credentials.Password, out user) && null != user)
                {
                    FormsAuthenticationTicket ticket = GenerateTicket(user);

                    HttpCookie cookie = GenerateCookie(ticket);
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return new { Principal = UserToSerializable(user), IsAuthenticated = true };
                }
            }

            return new { IsAuthenticated = false };
        }

        [NonAction]
        private PaulPrincipalSerializableModel UserToSerializable(User model)
        {
            PaulPrincipalSerializableModel serializedModel = new PaulPrincipalSerializableModel();
            serializedModel.Id = model.Id;
            serializedModel.Username = model.Username;
            serializedModel.DisplayName = model.DisplayName;
            serializedModel.Name = model.Name;
            serializedModel.FBId = model.FbId;

            return serializedModel;
        }

        [NonAction]
        private FormsAuthenticationTicket GenerateTicket(User model)
        {
            PaulPrincipalSerializableModel serializedModel = new PaulPrincipalSerializableModel();
            serializedModel.Id = model.Id;
            serializedModel.Username = model.Username;
            serializedModel.DisplayName = model.DisplayName;
            serializedModel.Name = model.Name;
            serializedModel.FBId = model.FbId;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(serializedModel);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now, DateTime.Now.AddHours(1), false, userData);

            return ticket;
        }

        [NonAction]
        private HttpCookie GenerateCookie(FormsAuthenticationTicket ticket)
        {
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            return cookie;
        }

        [NonAction]
        private User GetUser(string username)
        {
            using (var context = new WebAPIExamplesContext())
            {
                return context.Users.FirstOrDefault(u => u.Username == username);
            }
        }

        [NonAction]
        private bool ValidateUser(string username, string password, out User user)
        {
            using (var context = new WebAPIExamplesContext())
            {
                user = context.Users.FirstOrDefault(u => u.Username == username && u.Status == UserStatus.Active);
                if (null == user)
                {
                    return false;
                }

                // Use bcrypt and compare the password
                // The salt is already contained in the hash
                bool validPassword = password == user.Password;
                return validPassword;
            }
        }
    }
}
