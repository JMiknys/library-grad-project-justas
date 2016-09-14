using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryGradProject.Controllers
{
    public class RatingsController : ApiController
    {
        private RatingRepository _ratingRepo;

        public RatingsController(RatingRepository ratingRepo)
        {
            _ratingRepo = ratingRepo;
        }

        public HttpResponseMessage Post(BookRating newBookRating)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, "Sucessfully added to DB");
            try
            {
                _ratingRepo.Add(newBookRating);
            }
            catch (ArgumentException a)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid parameters. " + a.Message);
            }
            catch (DbUpdateException a)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, a.Message);
            }
            return response;
        }

        public HttpResponseMessage Put(BookRating bookRating)
        {
            HttpResponseMessage response;
            try
            {
                ((RatingRepository)_ratingRepo).Update(bookRating);
                response = Request.CreateResponse(HttpStatusCode.OK, "Rating updated");
                return response;
            }
            catch (ArgumentException)
            {
                try
                {
                    _ratingRepo.Add(bookRating);
                    response = Request.CreateResponse(HttpStatusCode.Created, "Created a new rating");
                    return response;
                }
                catch (ArgumentException e)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Failed. " + e.Message);
                    return response;
                }
            }
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _ratingRepo.GetAll());
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                List<BookRating> br = _ratingRepo.Get(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, br);
                return response;
            }
            catch (ArgumentException)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to retrieve data");
                return response;
            }
        }
    }
}