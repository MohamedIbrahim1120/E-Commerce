using Domain.Contracts;
using Domain.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Identity;
using Services;
using Shared;
using Shared.ErrorsModels;
using Store.APi.MiddlesWares;
using System.Text;

namespace Store.APi.Extensions
{
    public static  class Extensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddBulitInServices();
            services.AddSwaggerServices();


            services.ConfigureServices();
           services.AddInfrastructureServices(configuration); 


            services.AddIentityServices();

            services.AddApplicationServices(configuration);
            services.configureJwtServices(configuration);


            return services;
        }

        private static IServiceCollection AddBulitInServices(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var error = actionContext.ModelState.Where(m => m.Value.Errors.Any())
                                              .Select(m => new ValidationError()
                                              {
                                                  Field = m.Key,
                                                  Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)
                                              });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = error
                    };
                    return new BadRequestObjectResult(response);
                };
            });



            return services;
        }

        public static async Task<WebApplication> ConfigureMiddleWares(this WebApplication app)
        {
            await app.InitializeDataBaseAsyn();

             app.UseGlobalErrorHandling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            return app;
        }

        private static async Task<WebApplication> InitializeDataBaseAsyn(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>(); // Ask CLR Create Object From DbInitializer 
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();

            return app;
        }

        private static  WebApplication UseGlobalErrorHandling(this WebApplication app)
        {

            app.UseMiddleware<GlobalErrorHandlingMiddlewares>();
            return app;
        }

        private static IServiceCollection AddIentityServices(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return services;
        }

        private static IServiceCollection configureJwtServices(this IServiceCollection services,IConfiguration configuration)
        {

            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });




            return services;
        }

    }
}
