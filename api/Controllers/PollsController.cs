using api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class PollsController : ApiController
    {
        private WebAPIExamplesContext db = new WebAPIExamplesContext();

        // GET api/polls
        public IEnumerable<Poll> Get(PollStatus? type)
        {
            return db.Polls.Include("Choices").Include("CreatedBy").Where(x => x.Status == (type.HasValue ? type.Value : PollStatus.Active));
        }

        // GET api/polls/5
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
        public HttpResponseMessage Post(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, poll);
                response.Headers.Location = new Uri(Url.Link("apiCustom", new { id = poll.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT api/polls/5
        public HttpResponseMessage Put(int id, Poll poll)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != poll.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(poll).State = EntityState.Modified;

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

        // DELETE api/polls/5
        public HttpResponseMessage Delete(int id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Polls.Remove(poll);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, poll);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
