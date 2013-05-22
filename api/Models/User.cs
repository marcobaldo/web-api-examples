using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string FBId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Active,
        Deleted
    }
}
