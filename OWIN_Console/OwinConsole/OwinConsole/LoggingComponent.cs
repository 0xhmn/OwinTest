using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinConsole
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    /// <summary>
    /// logging the uri and status code for each request
    /// Since we want to know the status code AFTER the request has been processed, 
    /// we are going to place this component first in the pipeline, 
    /// but do no processing until after the call to _next.Invoke() returns. 
    /// In other words, we want to log status after all subsequent processing happens.
    /// </summary>
    public class LoggingComponent
    {
        private AppFunc _next;

        public LoggingComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            // pass everything through pipeline first
            await _next.Invoke(environment);
            IOwinContext context = new OwinContext(environment);
            Console.WriteLine("URI: {0} Status Code: {1}",
                context.Request.Uri, context.Response.StatusCode);
        }
    }
}
