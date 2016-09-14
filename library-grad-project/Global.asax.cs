using Autofac;
using Autofac.Integration.WebApi;
using LibraryGradProject.Models;
using LibraryGradProject.Repos;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace LibraryGradProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            // Autofac
            var builder = new ContainerBuilder();            

            // Register Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register types
            //builder.RegisterType<BookRepository>().As<IRepository<Book>>().SingleInstance();
            builder.RegisterType<BookRepository>().As<IRepository<Book>>().SingleInstance();
            builder.RegisterType<ReservationRepository>().As<IRepository<Reservation>>().SingleInstance();
            builder.RegisterType<UserRepository>().As<UserRepository>().SingleInstance();
            builder.RegisterType<RatingRepository>().As<RatingRepository>().SingleInstance();

            // Set the dependency resolver to be Autofac
            var container = builder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}
