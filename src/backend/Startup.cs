using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using backend.Data;
using backend.Exceptions;
using backend.Hubs;
using backend.Interceptors;
using backend.Middleware;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(builder =>
                builder
                    .UseLazyLoadingProxies()
                    .UseNpgsql(Configuration.GetConnectionString("Postgres"))
                    .UseSnakeCaseNamingConvention());
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.EventsType = typeof(CustomCookieAuthEvents); });

            services.AddScoped<CustomCookieAuthEvents>();
            
            services.AddAutoMapper(typeof(Startup));
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddScoped<IOffersRepository, OffersRepository>();
            services.AddScoped<IReservationsService, ReservationsService>();
            services.AddScoped<IReservationsRepository, ReservationsRepository>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<UsersRepository>();
            
            services.AddScoped<IPushService, PushService>();
            services.AddSingleton<IFileService, FileService>();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "../frontend/build"; });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<OffersChangedNotifier>();
            builder.RegisterType<OffersService>().As<IOffersService>().InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(OffersChangedNotifier));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

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

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorResponseMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ReservationsHub>("/hub");
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../frontend";

                if (env.IsDevelopment()) spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            });
        }
    }
}