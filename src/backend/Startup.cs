using System;
using System.Net;
using System.Threading.Tasks;
using backend.Data;
using backend.Exceptions;
using backend.Hubs;
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

            services.AddScoped<OffersService>();
            services.AddScoped<OffersRepository>();
            services.AddScoped<ReservationsService>();
            services.AddScoped<ReservationsRepository>();
            services.AddScoped<UsersService>();
            services.AddScoped<UsersRepository>();
            
            services.AddSingleton<OfferEvents>();
            services.AddSingleton<HubInvoker>();
            services.AddScoped<PushService>();
            services.AddSingleton<FileUploadService>();

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "../frontend/build"; });
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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async delegate(HttpContext context, Func<Task> next)
            {
                try
                {
                    await next.Invoke();
                }
                catch (EntityNotFoundException e)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    await context.Response.WriteAsync($"{e.EntityName} with ID = {e.EntityId} could not be found");
                }
            });

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