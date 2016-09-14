using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryGradProject.Controllers
{
    public class UsersController : ApiController
    {
        private UserRepository _userRepo;

        public UsersController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public HttpResponseMessage Get()
        {
            //IEnumerable<Book> books = _bookRepo.GetAll().to;
            return Request.CreateResponse(HttpStatusCode.OK, _userRepo.GetAll().ToList());
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                User u = _userRepo.Get(id);
                
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, u);
                return response;
            }
            catch (ArgumentException)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to retrieve data");
                return response;
            }
        }


        // POST api/values
        public HttpResponseMessage Post(User newUser)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, "Sucessfully added to DB");
            try
            {
                _userRepo.Add(newUser);
            }
            catch (ArgumentException a)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid parameters. " + a.Message);
            }
            catch(DbUpdateException a)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to create user: " + a.Message);
            }
            return response;
        }
    }
}