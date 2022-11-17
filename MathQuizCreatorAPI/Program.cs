using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MathQuizCreatorAPI.Authentication;
using MathQuizCreatorAPI.Data;
using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var MyAllowSpecificOrigins = "ReactApp";

var builder = WebApplication.CreateBuilder(args);

var uri = builder.Configuration["AzureAd:Uri"];
var tenantId = builder.Configuration["AzureAd:TenantId"];
var clientId = builder.Configuration["AzureAd:ClientId"];
var secret = builder.Configuration["AzureAd:Secret"];

ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, secret);
SecretClient client = new SecretClient(new Uri(uri), credential);

var connectionString = client.GetSecret("MathQuizCreatorDB").Value.Value;
var secretKey = client.GetSecret("JWTSecretKey").Value.Value;

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("MathQuizCreatorDB");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy( MyAllowSpecificOrigins,
        c =>
        {
            //c.WithOrigins("http://localhost:3000")
            c.WithOrigins("https://mathquizcreatorfe.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
                //.AllowCredentials();
        }
    );
});

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));
//var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:SecretKey").Value);
var key = Encoding.ASCII.GetBytes(secretKey);

var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
    ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.RequireHttpsMetadata = true;
    jwtBearerOptions.SaveToken = true;
    jwtBearerOptions.TokenValidationParameters = tokenValidationParams;
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// TO DO: FIGURE OUT AL THIS SO I CAN ACCESS MY API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MathQuizCreator",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
        "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
        "Example: \"Bearer 1safsfsdfdfd\""
    });

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
            new string[] { }
        }
    });
});


var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    SeedData.CreateRoles(scope.ServiceProvider).Wait();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
