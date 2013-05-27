using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Security;
using api.Models;
using api.Models.Security;

namespace api.Controllers
{
    public class SessionsController : ApiController
    {
        [HttpPost]
        public object Login(string username, string password)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (User.Identity.IsAuthenticated)
            {
                return new { Principal = User, IsAuthenticated = true };
            }

            if (ModelState.IsValid)
            {
                User user = null;
                if (ValidateUser(user.Username, user.Password, out user) && null != user)
                {
                    FormsAuthenticationTicket ticket = GenerateTicket(user);

                    HttpCookie cookie = GenerateCookie(ticket);
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return new { Principal = User, IsAuthenticated = true };
                }
            }

            return new { Principal = User, IsAuthenticated = false };
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
