using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Englishmania.BLL.Interfaces;
using Englishmania.BLL.Services;
using Englishmania.DAL.EF;
using Englishmania.DAL.Interfaces;
using Englishmania.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Englishmania.Web
{
    public class Startup
    {
        public static readonly string Issuer = "EnglishmaniaServer";
        public static readonly TimeSpan LifeTime = new TimeSpan(24, 0, 0);
        public static readonly string Key = "Pax7YTNcCnW0YcmUsPG4NKxIunK4aPC5yZYLhNdQGY4/KN+pQSnMzonUR5uLzVXycvI5DKWFGHePXbq0TKaIRg==";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    byte[] symmetricKey = Convert.FromBase64String(Key);
                    //options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Issuer,
                        
                        ValidateLifetime = true,
                        
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),

                        //RoleClaimType = "role",
                        RequireExpirationTime = true,
                        ValidateAudience = false
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<EnglishmaniaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType<VocabularyService>().As<IVocabularyService>();
            builder.RegisterType<WordService>().As<IWordService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().PropertiesAutowired();
            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
