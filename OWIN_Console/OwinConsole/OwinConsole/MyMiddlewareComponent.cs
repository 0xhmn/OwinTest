using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinConsole
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class MyMiddlewareComponent
    {
        private AppFunc _next;
        public MyMiddlewareComponent(AppFunc next)
        {
            _next = next;
        }
        public async Task Invoke(IDictionary<string, object> environment)
        {
            // If there is no next component, a 404 Not Found will be written as 
            // the response code here:
            await _next.Invoke(environment);

            IOwinContext context = new OwinContext(environment);
            var name = context.Request.QueryString.Value;
            // name should be == 'john' to pass the auth
            await context.Response.WriteAsync(string.Format("Hello {0}! You're in!", name));

            // Update the response code to 200 OK:
            context.Response.StatusCode = 200;
            context.Response.ReasonPhrase = "OK";
            
        }

    }
}
