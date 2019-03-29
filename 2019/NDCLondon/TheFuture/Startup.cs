using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheFuture.Errors;

namespace TheFuture
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<JsonExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting(routes =>
            {
                routes.MapApplication();
                routes.MapControllerRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapGet("/hello", context =>
                {
                    return context.Response.WriteAsync("Hello, world!");
                });
                
                routes.MapGet("/graph", "DFA Graph", Graph);

                routes.MapHealthChecks("/healthz").AddMetadata(new JsonExceptionMetadataAttribute());
            });
        }

        private static Task Graph(HttpContext httpContext)
        {
            using (var writer = new StreamWriter(httpContext.Response.Body, Encoding.UTF8, 1024, leaveOpen: true))
            {
                var graphWriter = httpContext.RequestServices.GetRequiredService<DfaGraphWriter>();
                var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();
                graphWriter.Write(dataSource, writer);
            }

            return Task.CompletedTask;
        }

        #region DO NOT OPEN

        private static Task CoolButUnreliableThing(HttpContext httpContext)
        {
            throw null;
        }

        #endregion
    }
}
