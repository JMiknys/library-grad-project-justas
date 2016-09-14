using System.Net.Http.Headers;
using System.Web.Http;

namespace LibraryGradProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            // Return JSON when we access the api via a web browser
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Set up default routing
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/login",
                defaults: new { controller = "Login" }
            );


            config.Routes.MapHttpRoute(
                name: "RatingApi",
                routeTemplate: "api/ratings",
                defaults: new { controller = "Ratings" }
            );

        }
    }
}
