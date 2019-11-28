using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Hangfire;
using Microsoft.Extensions.Configuration;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TodoApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //this is where i initialize my add cors object;

        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=.;Database=TodoDB;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(connection));
            //services.AddMvc();

            services.AddHangfire(x => x.UseSqlServerStorage(connection));// Configuration.GetConnectionString(connection)));
            services.AddScoped<TodoContext>();
            //services.AddScoped<Admin>();
            services.AddTransient<BackgroundTask>();
            services.AddTransient<AdminControllerExtension>();
            IServiceScopeFactory scopeFactory = services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();
            services.AddHangfire(x =>
            {
                x.UseActivator(new ContainerJobActivator(scopeFactory));
                x.UseSqlServerStorage(connection);

            });

            services.AddCors();
            //i changed my Service.AddMvc() to this.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ////"http://www.contoso.com"
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceScopeFactory serviceScopeFactory, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(build => build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //i added this to fix the cors problem also
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}
            //this too. for the cors
            //app.UseHttpsRedirection();



            var hangFireAuth = new DashboardOptions
            {
                Authorization = new[]
                {
                 new HangFireAuthorization(app.ApplicationServices.GetService<IAuthorizationService>(),
                 app.ApplicationServices.GetService<IHttpContextAccessor>())


                }

            };




            app.UseHangfireDashboard("/hangfire", options: hangFireAuth);

            app.UseHangfireServer();

            var todocontext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TodoContext>();
            BackgroundTask hangfire = new BackgroundTask(todocontext);

            app.UseMvcWithDefaultRoute();



        }
    }
}

//routes => {
//        routes.MapRoute(
//        name: "default",
//        template: "{controller=Home}/{action=Index}/{id?}");

//       routes.MapRoute(
//       name: "about-route",
//       template: "about",
//       defaults: new { controller = "Home", action = "About" }
//        );
// }