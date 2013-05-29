using System.Data;
using api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class PollsController : ApiController
    {
        private WebAPIExamplesContext db = new WebAPIExamplesContext();

        [HttpGet]
        public string RandomMethod()
        {
            return "Pizza";
        }
        // GET api/polls
        [HttpGet]
        public IEnumerable<Poll> All(PollStatus? pollStatus)
        {
            return db.Polls.Include("Choices").Include("Choices.VotedBy").Include("CreatedBy").Where(x => x.Status == (pollStatus.HasValue ? pollStatus.Value : PollStatus.Active)).OrderBy(x => x.Closing);
        }

        // GET api/polls/5
        [HttpGet]
        public Poll Get(int id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return poll;
        }

        // POST api/polls
        public object Add(Poll poll)
        {
            // Weird.. time zones are setup properly lol
            poll.Closing = poll.Closing.AddHours(-4);

            db.Entry(poll.CreatedBy).State = EntityState.Unchanged;
            foreach (Choice choice in poll.Choices)
            {
                choice.AddedBy = poll.CreatedBy;
                //db.Entry(choice.AddedBy).State = EntityState.Unchanged;
            }
            db.Polls.Add(poll);
            db.SaveChanges();

            poll = db.Polls.Include("Choices").Include("CreatedBy").FirstOrDefault(p => p.Id == poll.Id);
            return new { Success = true, Poll = poll };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
