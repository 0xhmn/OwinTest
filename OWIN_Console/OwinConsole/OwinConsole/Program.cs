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
    /// part two:
    /// using Katana:
    /// IOwinContext
    /// OwinContext
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
            var middleware = new Func<AppFunc, AppFunc>(MyMiddleWare);
            app.Use(middleware);
        }

        // middleware signature: Func<AppFunc, AppFunc>
        public AppFunc MyMiddleWare(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                IOwinContext context = new OwinContext(environment);
                await context.Response.WriteAsync("<h1>using IOwinContext to send the response</h1>");
                await next.Invoke(environment);
            };
            return appFunc;
        }


    }
}
