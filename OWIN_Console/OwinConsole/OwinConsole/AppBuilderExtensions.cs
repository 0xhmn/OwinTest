using Owin;

namespace OwinConsole
{
    public static class AppBuilderExtensions
    {
        // middleware
        public static void UseMyMiddlewareComponent(this IAppBuilder app)
        {
            app.Use<MyMiddlewareComponent>();
        }
        // auth and mock
        public static void UseAuthentication(this IAppBuilder app)
        {
            app.Use<AuthenticationComponent>();
        }
        // logging
        public static void UseLoggingComponent(this IAppBuilder app)
        {
            app.Use<LoggingComponent>();
        }
    }
}
