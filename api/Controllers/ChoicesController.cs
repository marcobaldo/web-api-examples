using api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models.Forms;
using api.Models.Security;

namespace api.Controllers
{
    public class ChoicesController : ApiController
    {
        private WebAPIExamplesContext db = new WebAPIExamplesContext();

        // Custom methods
        // GET api/choices/from
        [HttpGet]
        public IEnumerable<Choice> From(int id)
        {
            return db.Choices.Include("AddedBy").Where(x => x.PollId == id);
        }

        // POST api/choices/vote
        [HttpPost]
        public object Vote(VoteModel model)
        {
            Choice choice = db.Choices.Include("Poll").Include("VotedBy").FirstOrDefault(c => c.Id == model.ChoiceId);
            User user = db.Users.FirstOrDefault(u => u.Id == model.UserId);

            if (choice == null || user == null)
            {
                return new { Success = false, Message = "Invalid User and/or Choice." };
            }

            PaulPrincipal paul = User as PaulPrincipal;
            if (paul != null && user.Id != paul.Id)
            {
                return new {Success = false, Message = "You can only vote as the currently logged in user."};
            }

            // Toggle off
            if (choice.VotedBy.Count(u => u.Id == model.UserId) > 0)
            {
                User userToRemove = choice.VotedBy.FirstOrDefault(u => u.Id == model.UserId);
                choice.VotedBy.Remove(userToRemove);

                db.Entry(choice).State = EntityState.Modified;
                db.SaveChanges();

                return new { Success = true, Choice = choice, Action = "Removed" };
            }

            // Check if max
            Poll poll = choice.Poll;
            int userVoteCount = 0;
            foreach (Choice pollChoice in poll.Choices)
            {
                userVoteCount += pollChoice.VotedBy.Count(u => u.Id == model.UserId);

                if (userVoteCount >= poll.MaxVotes)
                {
                    return
                        new
                            {
                                Success = false,
                                Message = "You are only allowed to vote " + poll.MaxVotes + " times in this poll."
                            };
                }
            }

            choice.VotedBy.Add(user);
            db.Entry(choice).State = EntityState.Modified;
            db.SaveChanges();

            return new { Success = true, Choice = choice, Action = "Added" };
        }

        // GET api/choices
        public IEnumerable<Choice> Get()
        {
            return db.Choices.Include("AddedBy");
        }

        // GET api/choices/5
        public Choice Get(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return choice;
        }

        // POST api/choices
        public object Post(Choice choice)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(choice.AddedBy).State = EntityState.Unchanged;
                    db.Choices.Add(choice);
                    db.SaveChanges();

                    choice = db.Choices.Include("AddedBy").Where(x => x.Id == choice.Id).FirstOrDefault();
                }
                catch
                {
                    return new { Success = false };
                }

                return new { Success = true, Choice = choice };
            }
            else
            {
                return new { Success = false };
            }
        }

        // PUT api/choices/5
        public HttpResponseMessage Put(int id, Choice choice)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != choice.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(choice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/choices/5
        public HttpResponseMessage Delete(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Choices.Remove(choice);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, choice);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
