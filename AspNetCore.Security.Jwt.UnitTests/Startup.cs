using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AspNetCore.Security.Jwt.UnitTests
{
    using AspNetCore.Security.Jwt;
    using System;
    using System.IO;
    using System.Reflection;

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
            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "XXX API", Version = "v1" });
                // Enable XML comments for Swagger, according to https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var securitySettings = new SecuritySettings();
            this.Configuration.Bind("SecuritySettings", securitySettings);

            //Default Auth + Facebook

            //services
            //        .AddSecurity(securitySettings, true)
            //        .AddSecurity<Authenticator>()
            //        .AddFacebookSecurity(builder =>
            //            builder.AddClaim("FacebookUser", userModel => userModel.UserAccessToken))
            //        .AddAzureADSecurity();

            //OR

            //Custom User model auth + Facebook

            services
                   .AddSecurity(securitySettings, true)
                   .AddSecurity<CustomAuthenticator, UserModel>(builder =>
                       builder.AddClaim(IdType.Name, userModel => userModel.Id)
                              .AddClaim(IdType.Role, userModel => userModel.Role)
                              .AddClaim("DOB", userModel => userModel.DOB))
                   .AddFacebookSecurity(builder =>
                       builder.AddClaim("FacebookUser", userModel => userModel.UserAccessToken.ToString()))
                   .AddAzureADSecurity()
                   .AddGoogleSecurity();

            //services.AddMvc().AddSecurity().AddFacebookSecurity().AddAzureADSecurity();
            services.AddMvc()
                    .AddSecurity<UserModel>()
                    .AddFacebookSecurity()
                    .AddAzureADSecurity()
                    .AddGoogleSecurity();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(builder =>
                            builder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader()
                        );

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "XXX API V1");
            });

            app.UseSecurity(true);

            app.UseMvc();
        }
    }
}
