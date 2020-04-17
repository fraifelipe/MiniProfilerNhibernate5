using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniProfilerNhibernate5.Infra;
using NHibernate;
using System.Data.SqlClient;

namespace MiniProfilerNhibernate5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<TodoDbContext>(x => x.UseSqlServer("server=.;database=Todo;Integrated Security=true;"));
            
            services.AddControllersWithViews();
            
            services.AddMiniProfiler(options => {
                // When the environment is Development the node js server link will be polling. No need to include those requests in our profiles.
                options.ShouldProfile = request => {
                    return !request.Path.Value.Contains("sockjs-node");
                };
            });
            
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
            
            services.AddSingleton(x => new NHibernateFactory("Data Source=.;Initial Catalog=Todo;Integrated Security=SSPI;").CreateSessionFactory());
            services.AddScoped<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(x.GetService<ISessionFactory>().OpenSession()));
            // services.AddScoped<IUnitOfWork, UnitOfWork>(x => new UnitOfWork(x.GetService<ISessionFactory>().WithOptions().Connection(new StackExchange.Profiling.Data.ProfiledDbConnection(x.GetService<ISessionFactory>().OpenSession().Connection, MiniProfiler.Current)).OpenSession()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiniProfiler();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller}/{action=Index}/{id?}");
            // });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
