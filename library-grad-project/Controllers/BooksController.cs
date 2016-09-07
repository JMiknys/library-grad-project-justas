using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryGradProject.Controllers
{
    public class BooksController : ApiController
    {
        private IRepository<Book> _bookRepo;
        
        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepo = bookRepository;
        }

        // GET api/books
        public HttpResponseMessage Get()
        {
           return Request.CreateResponse(HttpStatusCode.OK, _bookRepo.GetAll());
        }

        // GET api/values/{int}
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Book b = _bookRepo.Get(id);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, b);
                return response;
            }
            catch (ArgumentException)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to retrieve data");
                return response;
            }            
        }

        // POST api/values
        public HttpResponseMessage Post(Book newBook)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, "Sucessfully added to DB");
            try
            {
                _bookRepo.Add(newBook);
            }
            catch (ArgumentException a)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid parameters. "+a.Message);
            }
                        
            return response;
        }
        
        // DELETE api/values/{int}
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Book was removed");
            try
            {
                _bookRepo.Remove(id);
                
            }
            catch (ArgumentException)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to remove");
            }
            return response;
        }

        // PUT api/values/{int}
        public HttpResponseMessage Put(int id, Book newBook)
        {
            HttpResponseMessage response;
            try
            {
                ((BookRepository)_bookRepo).Update(id, newBook);
                response = Request.CreateResponse(HttpStatusCode.OK, "Updated existing book");
                return response;
            }
            catch (ArgumentException)
            {
                try
                {
                    _bookRepo.Add(newBook);
                    response = Request.CreateResponse(HttpStatusCode.Created, "Created a new book");
                    return response;
                }
                catch (ArgumentException e)
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Failed. "+e.Message);
                    return response;
                }
            }
        }
    }
}
