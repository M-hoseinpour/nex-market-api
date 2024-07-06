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
using Amazon.S3;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using market.Configuration.Swagger;
using System.Text.Json.Serialization;
using market.Converters;
using Swashbuckle.AspNetCore.SwaggerUI;
using market.Models.DTO.File;
using market.Services.ProductService;
using market.Services.FileService;
using System.Security.Cryptography;
using market.Services;
using market.Services.BrandService;
using market.Services.CartService;
using market.Services.OrderService;

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

builder.Services.AddSingleton<IAmazonS3>(
sp =>
{
    var s3Config = configuration.GetSection(nameof(S3Config)).Get<S3Config>();
    var clientConfig = new AmazonS3Config
    {
        AuthenticationRegion = s3Config.Region,
        ServiceURL = s3Config.ServiceUrl,
        ForcePathStyle = true
    };
    return new AmazonS3Client(
        awsAccessKeyId: s3Config.AccessKey,
        awsSecretAccessKey: s3Config.SecretKey,
        clientConfig: clientConfig
    );
}
);

var connectionString = configuration.GetConnectionString("MainDb");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Configure<JwtServiceSettings>(services: builder.Services, key: nameof(JwtServiceSettings));
Configure<S3Config>(services: builder.Services, key: nameof(S3Config));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<RandomNumberGenerator, RNGCryptoServiceProvider>();
builder.Services.AddSingleton<IWorkContext, WorkContext>();
builder.Services.AddSingleton<IRandomService, RandomService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PanelService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<BannerService>();
builder.Services.AddScoped<FinancialTransactionService>();



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
    app.UseSwaggerUI(options =>
    {
        options.DocExpansion(DocExpansion.None);
        options.EnablePersistAuthorization();
    });
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