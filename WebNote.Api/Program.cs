using Microsoft.OpenApi.Models;
using WebNote.Api.Configuration;
using WebNote.Domain.Repository.Logs;
using WebNote.Domain.Repository.Notes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<NotesDatabaseSettings>(builder.Configuration.GetSection("NotesDatabase"));
builder.Services.Configure<LogsDatabaseSettings>(builder.Configuration.GetSection("LogsDatabase"));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDependencyInjections(builder.Configuration);
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddAuth(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
app.Run();