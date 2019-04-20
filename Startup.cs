/** 
* Here, we add features and services to our server.
*
* https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.2
* ...
* A common use of "dependency injection" into the Startup class is to inject:
* IHostingEnvironment to configure services by environment.
* IConfiguration to read configuration.
* ILoggerFactory to create a logger in Startup.ConfigureServices
*
* https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
* ASP.NET Core supports the dependency injection (DI) software design pattern,
* which is a technique for achieving inversion of control (IoC) between classes
* and their dependencies.
*/

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace shortest_path_UARK
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /** This method gets called by the runtime. Use this method to add 
        * services to the container. These services can then be requested via
        * dependency injection in our classes constructors like Angular.
        * 
        * Framework-provided services
        * The Startup.ConfigureServices method is responsible for defining the services
        * the app uses, including platform features, such as Entity Framework Core and
        * ASP.NET Core MVC
        * 
        * Default service container replacement
        * The built-in service container is meant to serve the needs of the framework and
        * most consumer apps. It is recommended to use the built-in container unless a
        * specific feature is not supported. Some of the features supported in 3rd party
        * containers NOT found in the built-in container:
        * Property injection
        * Injection based on name
        * Child containers
        * Custom lifetime management
        * 
        * A sample that replaces the built-in container w/ Autofac
        * ...
        * To use a 3rd party container, Startup.ConfigureServices must return IServiceProvider
        */
        public void ConfigureServices(IServiceCollection services)
        {
            /** MVC-Service (Model-View-Controller). This service is responsible
             * for routing the HTTP-requests to our controllers.
            */
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        /** This method gets called by the runtime. Use this method to configure 
         * the HTTP request pipeline. With this application-builder, we can add
         * middleware to our HTTP-request pipeline.       
        */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            /**            
             * MVC-middleware is responsible for forwarding the requests to the
             * responsible controller.
             *            
            */
            app.UseMvc(routes =>
            {
                /** 
                 * https://docs.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-2.2
                 * Convention-based routing enables you to globally define the URL
                 * formats that your application accepts and how each of those
                 * formats maps to a specific action method on given controller.
                */
                routes.MapRoute(
                    name: "default",
                    /** URI for the list of all the classrooms */
                    template: "{controller}/{action}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    /** 
                     * https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2&tabs=visual-studio
                     * Won't launch an Angular ClI server, but instead use the
                     * instance you start manually. Enables it to start and restart faster.
                    */
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
