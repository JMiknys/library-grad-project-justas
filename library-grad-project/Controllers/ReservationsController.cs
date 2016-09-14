using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryGradProject.Controllers
{
    public class ReservationsController : ApiController
    {
        private IRepository<Reservation> _reservationRepo;

        public ReservationsController(IRepository<Reservation> reservationRepository)
        {
            _reservationRepo = reservationRepository;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _reservationRepo.GetAll().ToList());
        }

        // GET api/values/{int}
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Reservation r = _reservationRepo.Get(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, r);
                return response;
            }
            catch (ArgumentException)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to retrieve data");
                return response;
            }
        }

        // POST api/values
        public HttpResponseMessage Post(Reservation newReservation)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, "Reservation added");
            try
            {
                _reservationRepo.Add(newReservation);
            }
            catch (ArgumentException e)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to add Reservation: "+e.Message);
            }
            return response;
        }

        // DELETE api/values/{int}
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Removed");
            try
            {
                _reservationRepo.Remove(id);
            }
            catch
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to remove");
            }
            return response;
        }
    }
}