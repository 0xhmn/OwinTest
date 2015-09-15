using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace OwinConsole
{
    // Alias
    // using AppFunc = Func<IDictionary<string, object>, Task>; 

    public class Program
    {
        static void Main(string[] args)
        {
            WebApp.Start<Startup>("http://localhost:8080");
            Console.Write("Listening to http://localhost:8080");
            Console.ReadLine();
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseLoggingComponent();
            app.UseAuthentication();
            app.UseMyMiddlewareComponent();

        }
    }





}
