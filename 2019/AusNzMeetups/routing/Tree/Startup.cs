using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tree
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.Use(next => httpContext =>
            {
                if (httpContext.Request.Path == "/graph")
                {
                    var syncIOFeature = httpContext.Features.Get<IHttpBodyControlFeature>();
                    if (syncIOFeature != null)
                    {
                        syncIOFeature.AllowSynchronousIO = true;
                    }

                    using (var writer = new StreamWriter(httpContext.Response.Body, Encoding.UTF8, 1024, leaveOpen: true))
                    {
                        var graphWriter = httpContext.RequestServices.GetRequiredService<DfaGraphWriter>();
                        var dataSource = httpContext.RequestServices.GetRequiredService<EndpointDataSource>();
                        graphWriter.Write(dataSource, writer);
                    }

                    return Task.CompletedTask;
                }

                return next(httpContext);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
