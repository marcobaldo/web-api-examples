using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace api.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public string Label { get; set; }
        [ForeignKey("Poll"), Column("Poll_Id")]
        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }
        public List<User> VotedBy { get; set; }
        public User AddedBy { get; set; }
    }
}
