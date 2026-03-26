
using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Middleware;
using MaintenanceApi.Service;
using MaintenanceApi.Util;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuestPDF.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

namespace MaintenanceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            QuestPDF.Settings.License = LicenseType.Community;
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<Ldap>();
            builder.Services.AddScoped<LdapService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AssetsService>();
            builder.Services.AddScoped<AssetsRepo>();
            builder.Services.AddScoped<MechanicsRepo>();
            builder.Services.AddScoped<MechanicService>();
            builder.Services.AddScoped<WorkOrdersService>();
            builder.Services.AddScoped<WorkOrdersRepo>();
            builder.Services.AddScoped<WorkOrdersImagesRepo>();
            builder.Services.AddScoped<PdfService>();
            // Use Serilog as the logging provider
      

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.Cookie.Name = "maintenance_auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;

                    options.LoginPath = "/api/auth/login";
                    options.AccessDeniedPath = "/api/auth/denied";

                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigin",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                        .AllowCredentials()
                                          
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
