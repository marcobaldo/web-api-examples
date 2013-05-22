using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Closing { get; set; }
        public int MaxVotes { get; set; }
        public bool AllowEdit { get; set; }
        public List<Choice> Choices { get; set; }
        public User CreatedBy { get; set; }
        public PollStatus Status { get; set; }
    }

    public enum PollStatus
    {
        Active,
        Completed
    }
}