using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Add Owing
using Microsoft.Owin;

namespace OwinConsole
{
    // alias for Owin AppFunc
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// http://goo.gl/tjRdNU
    /// </summary>
    public class AuthenticationComponent
    {
        AppFunc _next;

        public AuthenticationComponent(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            // In the real world we would do REAL auth processing here...
            // we are setting isAuthorize just based on one person name
            var isAuthorized = context.Request.QueryString.Value == "john";
            if (!isAuthorized)
            {
                context.Response.StatusCode = 401;
                context.Response.ReasonPhrase = "Not John -> Not Authorized!!!";

                // redirect to an error page
                await
                    context.Response.WriteAsync(string.Format("Error - {0}-{1}", context.Response.StatusCode,
                        context.Response.ReasonPhrase));
            }
            else
            {
                context.Response.StatusCode = 200;
                context.Response.ReasonPhrase = "OK!";
                await _next.Invoke(environment);
            }
        }

    }
}
