using ASP.StartApp.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.StartApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public static IWebHostEnvironment env;
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging() )
            {
                app.UseDeveloperExceptionPage();
            }
            Startup.env = env;
            

            app.UseRouting();

            app.UseStaticFiles();

            //app.Use(async (context, next) =>
            //{
            //    // Строка для публикации в лог
            //    string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            //    // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
            //    string logFilePath = Path.Combine(env.ContentRootPath, "Logs", "RequestLog.txt");
            //    await File.AppendAllTextAsync(logFilePath, logMessage);
            //    await next.Invoke();
            //});
            app.UseMiddleware<LoggingMiddleware>();
            //app.Use(async (context, next) =>
            //{
            //    // Для логирования данных о запросе используем свойства объекта HttpContext
            //    Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
            //    await next.Invoke();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
            });
            // Все прочие страницы имеют отдельные обработчики
            app.Map("/about", About);
            app.Map("/config", Config);

            // Обработчик для ошибки "страница не найдена"
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });
        }
        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  Обработчик для главной страницы
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            //Console.WriteLine($"Launching project from: {env.ContentRootPath}");
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
            });
        }
    }
}
