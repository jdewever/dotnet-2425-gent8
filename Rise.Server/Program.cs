using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Persistence.Triggers;
using Rise.Services.Products;
using Rise.Services.Barcodes;
using Rise.Shared.Products;
using Rise.Shared.Barcodes;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rise.Shared.Cart;
using Rise.Services.Cart;
using Rise.Shared.Transaction;
using Rise.Services.Transaction;
using Auth0Net.DependencyInjection;
using Rise.Server.Auth;
using Rise.Services.Auth;
using Rise.Services.Booking;
using Rise.Shared.Booking;
using Rise.Services.User;
using Rise.Shared.User;
using Rise.Services.Minio;
using Rise.Shared.Minio;
using Microsoft.AspNetCore.Http.Features;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["Auth0:Authority"]}/oauth/token"),
                AuthorizationUrl = new Uri($"{builder.Configuration["Auth0:Authority"]}/authorize?audience={builder.Configuration["Auth0:Audience"]}"),
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new string[] { "openid" }
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>());
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBarcodeService, BarcodeService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpContextAccessor()
    .AddScoped<IAuthContextProvider, HttpContextAuthProvider>();

builder.Services.AddAuth0AuthenticationClient(config =>
{
    config.Domain = builder.Configuration["Auth0:Authority"]!;
    config.ClientId = builder.Configuration["Auth0:M2MClientId"];
    config.ClientSecret = builder.Configuration["Auth0:M2MClientSecret"];
});
builder.Services.AddAuth0ManagementClient().AddManagementAccessToken();

// minio
string endpoint = "minio.xpandity.com";
string region = "eu-central";
string accessKey = "K6i0NRfkTPfEqTTc5c6s";
string secretKey = "HZMubuMSMesQEsLGxdxEdDc0pgvWGKOU5XdhN8cE";
bool useSSL = true;
string bucket = "rise";
string domain = builder.Configuration["Minio:PublicDomain"] ?? "";

if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(bucket))
{
    throw new ArgumentNullException("Minio configuration is invalid, please check your appsettings.json");
}
if (string.IsNullOrWhiteSpace(domain))
{
    domain = useSSL ? $"https://{endpoint}" : $"http://{endpoint}";
}

builder.Services.AddScoped<IMinioService>(provider =>
    new MinioService(endpoint, region, accessKey, secretKey, useSSL, bucket, domain)
);

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1.0");
        options.OAuthClientId(builder.Configuration["Auth0:BlazorClientId"]);
        options.OAuthClientSecret(builder.Configuration["Auth0:BlazorClientSecret"]);
    });
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{ // Require a DbContext from the service provider and seed the database.
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    bool isDevelopment = env.IsDevelopment();

    // drop and recreate the database if date is before 11-12-2024 at 12:35 for client demo
    if (DateTime.Now < new DateTime(2024, 12, 11, 12, 35, 0))
    {
        dbContext.Database.EnsureDeleted();
    }
    dbContext.Database.Migrate();

    IMinioClient minioClient = new MinioClient().WithEndpoint(endpoint).WithRegion(region).WithCredentials(accessKey, secretKey).WithSSL(useSSL).Build(); 
    Seeder seeder = new(dbContext, minioClient, domain, bucket, isDevelopment);
    await seeder.Seed();
}

app.Run();
