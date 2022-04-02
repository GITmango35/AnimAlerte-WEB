using AnimAlerte.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AnimAlerte
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
            // Ajouter pour la langues FR et En
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddControllersWithViews().AddDataAnnotationsLocalization();
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<RequestLocalizationOptions>(
                opt =>
                {
                    var supportedCultures = new List<CultureInfo>
                    { 
                        new CultureInfo("en"),
                        new CultureInfo("fr") 
                    };
                    opt.DefaultRequestCulture = new RequestCulture("en");
                    opt.SupportedCultures = supportedCultures;
                    opt.SupportedUICultures = supportedCultures;

                });

            services.AddDbContext<AnimAlerteContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbAnimalerte")));
            services.AddSession();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Utilisateurs}/{action=Login}/{id?}");
            });
        }
    }
}