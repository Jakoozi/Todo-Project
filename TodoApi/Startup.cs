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


namespace TodoApi
{
    public class Startup
    {
        
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Server=CHICHI-PC;Database=TodoDB;Trusted_Connection=True;ConnectRetryCount=0";          
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(connection));
            services.AddMvc();

            services.AddHangfire(x => x.UseSqlServerStorage(connection));// Configuration.GetConnectionString(connection)));
            services.AddTransient<BackgroundTask>();      
            IServiceScopeFactory scopeFactory = services
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();
            services.AddHangfire(x =>
            {
                x.UseActivator(new ContainerJobActivator(scopeFactory));
                x.UseSqlServerStorage(connection);

            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceScopeFactory serviceScopeFactory)
        {
            var hangFireAuth = new DashboardOptions
            {
                Authorization = new[]
                {

                 new HangFireAuthorization(app.ApplicationServices.GetService<IAuthorizationService>(),
                 app.ApplicationServices.GetService<IHttpContextAccessor>())


                }
               
               };



            app.UseMvc();
            app.UseHangfireDashboard("/hangfire", options: hangFireAuth);
            app.UseHangfireServer();

            var todocontext = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TodoContext>();
            BackgroundTask hangfire = new BackgroundTask(todocontext);
        }
    }
}
