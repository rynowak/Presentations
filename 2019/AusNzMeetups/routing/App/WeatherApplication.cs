using App;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WeatherApplication : Application
{
    public static void Main(string[] args) => Run<WeatherApplication>(args);

    public override void ConfigureServices(IServiceCollection services) => services.AddSingleton<WeatherSuperComputer>();

    public override void Configure(IApplicationBuilder app)
    {
        if (Env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            var superComputer = Services.GetRequiredService<WeatherSuperComputer>();

            endpoints.MapGet("/weather/{place}", context =>
            {
                var forecast = superComputer.GetForecast(context.Request.RouteValues.Get<string>("place"));
                return context.Response.WriteJsonAsync(forecast);
            });

            endpoints.MapPost("/weather/{place}", async context =>
            {
                var forecast = await context.Request.ReadJsonAsync<WeatherForecast>();
                superComputer.UpdateForecast(context.Request.RouteValues.Get<string>("place"), forecast);
            });
        });
    }
}