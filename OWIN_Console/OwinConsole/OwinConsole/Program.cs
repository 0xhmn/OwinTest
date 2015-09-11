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
    using AppFunc = Func<IDictionary<string, object>, Task>;


    public class Program
    {
        static void Main(string[] args)
        {
            // WebApp.Start<>("http://localhost:8080");

        }


    }

    // owin startup
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }

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

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                await writer.WriteAsync("this is from the second middleware without lambda");
            }

        }
    }
}
