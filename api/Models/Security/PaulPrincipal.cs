using System.Security.Principal;

namespace api.Models.Security
{
    public class PaulPrincipal : IPrincipal
    {
        public PaulPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
            Username = username;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string FBId { get; set; }
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}