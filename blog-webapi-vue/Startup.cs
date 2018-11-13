using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using blog_webapi_vue.AuthHelper;
using blog_webapi_vue.IServices;
using blog_webapi_vue.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Swashbuckle.AspNetCore.Swagger;

namespace blog_webapi_vue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "Blog.Core API",
                    Description = "Framework Documentation",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Blog.Core",
                        Email = "xxxx@xxx.com",
                        Url = "https://www.something.com"
                    }
                });

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "blog-webapi-vue.xml");
                c.IncludeXmlComments(xmlPath, true);

                var xmlModelPath = Path.Combine(basePath, "blog-webapi-vue.Model.xml");
                c.IncludeXmlComments(xmlModelPath);

                #region TokenBinding

                var security = new Dictionary<string, IEnumerable<string>>
                    { { "Blog.Core", new string[] { } }
                    };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
                {
                    Description = "JWT auth (data will be in the request header) enter Bearer {token} below.",
                    Name = "Authorization", // jwt default parameter name
                    In = "header",
                    Type = "apiKey"
                });

                #endregion
            });
            #endregion

            #region [TokenService]
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddAuthentication(x =>
                {
                    // options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                    // options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                    // options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());

                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Blog.Core",
                        ValidAudience = "wr",
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtHelper.secretKey)),
                        // cache expiry time, total = this time + jwt expiry time
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });
            #endregion


            var builder = new ContainerBuilder();
            // builder.RegisterType<AdvertisementServices>()
            //         .As<IAdvertisementServices>();
            var assemblyServices = Assembly.Load("blog-webapi-vue.Services");
            builder.RegisterAssemblyTypes(assemblyServices).AsImplementedInterfaces();
            var assemblyRepositories = Assembly.Load("blog-webapi-vue.Repository");
            builder.RegisterAssemblyTypes(assemblyRepositories).AsImplementedInterfaces();
            builder.Populate(services);
            var ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
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

            #region Swagger
            // This can be put into env.IsDevelopment so that it can only be reached in dev env.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                // By using the following line, when F5 is pressed, swagger page will be shown directly without manuanlly typing /swagger.
                c.RoutePrefix = "";
                // if we need to http://localhost:5001 to show swagger directly, we can change it in launchSettings.json.
            });
            #endregion
            // discard this costum middleware and use Microsoft method
            // but if you still need to do something special like write user info to global, keep using it.
            // app.UseMiddleware<JwtTokenAuth>();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}