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
    /// a
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
            var otherMiddleware = new Func<AppFunc, AppFunc>(MyOtherMiddleWare);
            app.Use(middleware);
            app.Use(otherMiddleware);
        }

        // middleware signature: Func<AppFunc, AppFunc>
        public AppFunc MyMiddleWare(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                // do something with incomming request
                var response = environment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync(("<h1>Hello from middleware</h1>"));
                }
                // Call next middleware in chain
                // middlewares are responsible for calling eachother
                await next.Invoke(environment);
            };
            return appFunc;
        }

        // add another middleware
        public AppFunc MyOtherMiddleWare(AppFunc next)
        {
            AppFunc appfunc = async (IDictionary<string, object> environment) =>
            {
                var response = environment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync(("<p>anoter res from second middleware</p>"));
                    // we should call next middleware in chain
                    await next.Invoke(environment);
                }
            };

            return appfunc;
        }

    }
}
