using System;
using HoursTracker.Web.Areas.Identity.Data;
using HoursTracker.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: HostingStartup(typeof(HoursTracker.Web.Areas.Identity.IdentityHostingStartup))]
namespace HoursTracker.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DefaultConnection")));

                services.AddScoped<IdentityErrorDescriber, CustomIdentityErrorDescriber>();

                services.AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 3;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
                //.AddErrorDescriber<SpanishIdentityErrorDescriber>()
                .AddEntityFrameworkStores<IdentityContext>();
            });
        }
    }
}