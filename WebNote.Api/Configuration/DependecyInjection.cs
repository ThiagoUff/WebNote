using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Interfaces.Services;
using WebNote.Infra.Integration;
using WebNote.Infra.Repository;
using WebNote.Services.Mapper;
using WebNote.Services.Services;

namespace WebNote.Api.Configuration
{
    public static class DependecyInjection
    {
        public static void AddDependencyInjections(this IServiceCollection services, ConfigurationManager configuration)
        {
            //Services
            services.AddScoped<INotesServices, NotesServices>();
            services.AddScoped<IJobsServices, JobsServices>();

            //Mapper
            services.AddScoped<INotesMapper, NotesMapper>();

            //Repositories
            services.AddScoped<INotesRepository, NotesRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();
            
            //Integration            
            services.AddRefitClient<IFunctions>()
                                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["ApiGateway:Functions"]));
            

            //services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddSingleton<IConfiguration>(configuration);
        }

        public static void AddAuth(this IServiceCollection service, ConfigurationManager configuration)
        {
            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
        }
    }
}
