using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using reservacion.Data;
using Microsoft.EntityFrameworkCore;
    
    //app.UseDatabaseErrorPage(); cambió de ubicacion a aca:
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using reservacion.Areas.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using reservacion.Api.JwtCustomModels;
using System.Text;

namespace reservacion
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Environment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment()){
                services.AddDbContext<ReservacionDbContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("ReservacionDevConnection"))
                );
            }
            else{
                services.AddDbContext<ReservacionDbContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("ReservacionProdConnection"))
                );
            }
            services.AddControllers().AddNewtonsoftJson(options => {
                // Para evitar errores de serializacion en Reservacion Controller
                options.SerializerSettings.ReferenceLoopHandling 
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddRazorPages();
            services.AddDefaultIdentity<User>(x => {
                x.Password.RequireDigit=false;
                x.Password.RequiredLength=1;
                x.Password.RequireLowercase=false;
                x.Password.RequireNonAlphanumeric=false;
                x.Password.RequireUppercase=false;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ReservacionDbContext>();

            services.AddAuthentication()
                .AddCookie(options => options.SlidingExpiration = true)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters(){
                        ValidIssuer = JwtCustomConstants.Issuer,
                        ValidAudience = JwtCustomConstants.Audience,
                        IssuerSigningKey = 
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtCustomConstants.secretKey))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
