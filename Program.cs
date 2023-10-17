using Microsoft.EntityFrameworkCore;
using market;
using market.Data.Repository;
using market.SystemServices.Contracts;
using market.SystemServices;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using market.Extensions;
using Microsoft.Extensions.Options;
using ApiFramework.Middlewares;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using market.Configuration.Swagger;
using System.Text.Json.Serialization;
using market.Converters;

var builder = WebApplication.CreateBuilder(args);

var configer = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Local.json", true, true)
    .AddEnvironmentVariables();

IConfiguration configuration = configer.Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        var filePath = Path.Combine(path1: AppContext.BaseDirectory, path2: assemblyName + ".xml");
        c.IncludeXmlComments(filePath);

        c.SchemaGeneratorOptions = new SchemaGeneratorOptions
        {
            SchemaIdSelector = type => type.FullName
        };
        c.AddSecurityDefinition(
            name: "Bearer",
            securityScheme: new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            }
        );
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        c.OperationFilter<SecurityRequirementsOperationFilter>();
        // c.EnableAnnotations();
    }
);

builder.Services
    .AddControllers()
    .AddJsonOptions(
        options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.Converters.Add(new DateTimeIsoConverter());
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }
    );

var connectionString = configuration.GetConnectionString("MainDb");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Configure<JwtServiceSettings>(services: builder.Services, key: nameof(JwtServiceSettings));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<IWorkContext, WorkContext>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PanelService>();



builder.Services
    .AddControllers()
    .AddJsonOptions(
        options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        }
    );

builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<UnHandledExceptionMiddleware>();
app.UseMiddleware<HandledExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();


void Configure<T>(IServiceCollection services, string key) where T : class
{
    services.Configure<T>(configuration.GetSection(key));
    services.AddSingleton(sp => sp.GetRequiredService<IOptions<T>>().Value);

    services.Configure<IISServerOptions>(
        options => { options.MaxRequestBodySize = int.MaxValue; }
    );

    services.Configure<KestrelServerOptions>(
        options =>
        {
            options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
        }
    );

    services.Configure<FormOptions>(
        x =>
        {
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
            x.MultipartHeadersLengthLimit = int.MaxValue;
        }
    );
}