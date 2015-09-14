using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Add Owing
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace OwinConsole
{
    // alias for owin AppFunc
    // getting a dictionary of <string, object> and returning a task
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// http://typecastexception.com/post/2015/01/04/ASPNET-Understanding-OWIN-Katana-and-the-Middleware-Pipeline.aspx#What-is-OWIN--and-Why-Do-I-Care-
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:8080");
            Console.WriteLine("server started");
            Console.Write("Listening to http://localhost:8080");
            Console.ReadLine();
        }


    }

    // owin startup
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseMyMiddlewareComponent();
            app.UseMyOtherMiddlewareComponent();
        }
    }

    // static extension class/method
    public static class AppBuilderExtensions
    {
        public static void UseMyMiddlewareComponent(this IAppBuilder app)
        {
            app.Use<MyMiddlewareComponent>();
        }

        public static void UseMyOtherMiddlewareComponent(this IAppBuilder app)
        {
            app.Use<MyOtherMiddlewareComponent>();
        }
    }

    public class MyMiddlewareComponent
    {
        private AppFunc _next;

        public MyMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<p>Hello World</p>");
            await _next.Invoke(environment);
        }

    }

    public class MyOtherMiddlewareComponent
    {
        AppFunc _next;

        public MyOtherMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            await context.Response.WriteAsync("<p>from second middleware</p>");
            await _next.Invoke(environment);
        }
    }
}
