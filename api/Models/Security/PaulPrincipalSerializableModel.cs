using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.Security
{
    public class PaulPrincipalSerializableModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string FBId { get; set; }
    }
}