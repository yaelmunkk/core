using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.ComponentModel;
using MyMiddleware;
namespace core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services
               .AddAuthentication(options =>
               {
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(cfg =>
               {
                   cfg.RequireHttpsMetadata = false;
                   cfg.TokenValidationParameters = TaskTokenService.GetTokenValidationParameters();
               });

            services.AddAuthorization(cfg =>
                {
                    cfg.AddPolicy("Manager", policy => policy.RequireClaim("type", "Manager"));
                    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
                });

            services.AddControllers();
            services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "core", Version = "v1" });
               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                   In = ParameterLocation.Header,
                   Description = "Please enter you task JWT with Bearer into field",
                   Name = "Authorization",
                   Type = SecuritySchemeType.ApiKey
               });
               c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { new OpenApiSecurityScheme
                        {
                         Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                    new string[] {}
                }});
           });
            services.AddSingleton<Interfaces.ITaskService, Services.TaskService>();
            services.AddSingleton<Interfaces.IUserService, Services.UserService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "core v1"));
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            // app.UseHttpsRedirection();
            app.UseMyLogMiddleware("log.txt");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
