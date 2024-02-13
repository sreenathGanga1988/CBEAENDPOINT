using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore.UnitOfWorks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebApi.Rules;
using WebApi.ViewModels;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
             BuildAppSettingsProvider();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });


            //services.AddControllersWithViews().AddRazorRuntimeCompilation()
            //  .AddNewtonsoftJson(options =>
            //  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
            //  .Json.ReferenceLoopHandling.Ignore)
            //  .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
            //  = new DefaultContractResolver());


            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddSession();
            services.AddMemoryCache();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options => {
           options.LoginPath = "/Home/login/";
           options.AccessDeniedPath = "/Home/Login/";
       })
       .AddJwtBearer(options => {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = Configuration["Jwt:Issuer"],
               ValidAudience = Configuration["Jwt:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey
             (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
           };
       });

            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));


            services.AddTransient<IDbConnection>((sp) =>
             new SqlConnection(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpContextAccessor();

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IUserTypeRepository, UserTypeRepository >();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<ICircleRepository , CircleRepository>();
            services.AddTransient<IDesignationRepository , DesignationRepository>();
            services.AddTransient<IEmployeeProfileRepository, EmployeeProfileRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            #endregion
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            //app.UseRewriter(new RewriteOptions().Add(new RedirectWwwRule()));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                AppSettingsProvider.BaseApiUri = Configuration["BaseApiUriDEV"];
            }
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapAreaControllerRoute(
            name: "MyAreaProducts",
            areaName: "Admin",
            pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

               

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });
        }
        private void BuildAppSettingsProvider()
        {
            AppSettingsProvider.BaseApiUri  = Configuration["BaseApiUri"];
          
        }
    }
}
