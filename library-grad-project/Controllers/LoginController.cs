using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LibraryGradProject.Controllers
{
    public class LoginController : ApiController
    {
        private UserRepository _userRepo;

        public LoginController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public HttpResponseMessage Post()
        {
            var json = Request.Content.ReadAsStringAsync().Result;
            User user = JsonConvert.DeserializeObject<User>(json);
            var jo = JObject.Parse(json);
            // If session key exists
            if (jo["SessionKey"] != null)
            {
                // Try login using session key

                // Check the already existing session
                Tuple<string, double> session = (Tuple<string, double>)HttpContext.Current.Session[user.Name];
                // Check if session exists
                if (session == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Session has expired");
                }
                // Check if session is fresh (haven't timed out)
                if ((session.Item2 - (DateTime.Now - DateTime.MinValue).TotalMilliseconds > 0) && (session.Item1 == jo["SessionKey"].ToString()))
                {
                    string responseJson = JsonConvert.SerializeObject(new { Name = user.Name, Id = _userRepo.GetByName(user.Name).Id });
                    return Request.CreateResponse(HttpStatusCode.OK, responseJson);
                }
                else
                {
                    // Invalid session
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Session key is not valid");
                }
                /*
                if (HttpContext.Current.Session[jo["Name"].ToString()] == jo["SessionKey"])
                {
                    return Request.CreateResponse(HttpStatusCode.OK, _userRepo.GetByName(user.Name));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Session key is not valid");
                }
                */
            }
            else
            {
                // Normal user/password login
                if (_userRepo.verifyUser(user))
                {
                    //HttpContext.Current.Session["dsa"] = "das";
                    //System.Diagnostics.Debug.Write(HttpContext.Current.Session["dsa"]);

                    // Generate a new session
                    byte[] byteArray = _userRepo.getRandomBytes(128);
                    string sessionHash = _userRepo.computeHash(user.Name, byteArray);
                    double timeout = (DateTime.Now.AddMinutes(30) - DateTime.MinValue).TotalMilliseconds;

                    Tuple<string, double> tuple = new Tuple<string, double>(sessionHash, timeout);
                    // Save session
                    HttpContext.Current.Session[user.Name] = tuple;
                    string responseJson = JsonConvert.SerializeObject(new { Name = user.Name, SessionKey = sessionHash, Id = _userRepo.GetByName(user.Name).Id });
                    return Request.CreateResponse(HttpStatusCode.OK, responseJson);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User is not valid");
                }
            }
            
            /*
            if (HttpContext.Current.Session[user.Name] == null)
            {
                // Generate a new session
                byte[] byteArray = _userRepo.getRandomBytes(128);
                string hash = _userRepo.computeHash(user.Name, byteArray);
                double timeout = (DateTime.Now.AddMinutes(30) - DateTime.MinValue).TotalMilliseconds;

                Tuple<string, double> tuple = new Tuple<string, double>(hash, timeout);
                HttpContext.Current.Session[user.Name] = tuple;
            }
            else
            {
                // Check the already existing session
                Tuple<string, double> session = (Tuple<string,double>) HttpContext.Current.Session[user.Name];
                // Check if session is fresh
                if (session.Item2 - (DateTime.Now - DateTime.MinValue).TotalMilliseconds > 0)
                {
                    // Session valid
                }
                else
                {
                    // Invalid session
                }

            }
            */
        }
    }
}