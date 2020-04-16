using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using reservacion.Areas.Identity.Data;
using reservacion.Data;

[assembly: HostingStartup(typeof(reservacion.Areas.Identity.IdentityHostingStartup))]
namespace reservacion.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ReservacionDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ReservacionDevConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ReservacionDbContext>();
            });
        }
    }
}