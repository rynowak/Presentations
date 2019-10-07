using WeatherLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WeatherApplication : Application
{
    public static void Main(string[] args) => Run<WeatherApplication>(args);

    public override void Configure(IApplicationBuilder app)
    {
        if (Env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            var computer = new WeatherSuperComputer();

            endpoints.MapGet("/weather/{place}", async context =>
            {
                var place = (string)context.Request.RouteValues["place"];
                var weather = computer.GetForecast(place);
                await context.Response.WriteJsonAsync(weather);
            });

            endpoints.MapPost("/weather/{place}", async context =>
            {
                var place = (string)context.Request.RouteValues["place"];
                var weather = await context.Request.ReadJsonAsync<WeatherForecast>();
                computer.UpdateForecast(place, weather);
            });
        });
    }
}