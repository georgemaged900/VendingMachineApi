using FlapKap;
using FlapKap.Middleware;
using FlapKap.Repository.Authentication;
using FlapKap.Repository.ProductRepo;
using FlapKap.Repository.RoleManagement;
using FlapKap.Repository.UserManagement;
using FlapKap.Service.Deposit;
using FlapKap.Service.ProductService;
using FlapKap.Service.RoleManagement;
using FlapKap.Service.UserManagement;
using FlapKapBackendChallenge;
using FlapKapBackendChallenge.Models;
using FlapKapBackendChallenge.Service.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using FlapKap.Models;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(); // Serilog

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("VendingMachineDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlapKap", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
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

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               .AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = "FlapKapSystem",
                       ValidAudience = "FlapKap",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3409fdusfj8j82091JS8D021MDM@I3RJ$JMIL[P-012I3U9045UJdskopk340j09jBNO8560FD91dsjsLPD-02349F9Jsjdhu328u4ubnl91238RHVNKALA01923ncz920v,b*"))
                   };
               });
builder.Services.AddAuthorization();

var app = builder.Build();


// DB Seed Roles
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { Id = 1, Name = "buyer" },
            new Role { Id = 2, Name = "seller" }
        );

        db.SaveChanges();

    }
    if (!db.Users.Any(u => u.UserName == "testbuyer"))
    {
        var hasher = new PasswordHasher<User>();
        var user = new User
        {
            UserName = "testbuyer",
            Password = hasher.HashPassword(null, "123456"),
            Deposit = 0,
            UserRoles = new List<UserRole>
        {
            new UserRole { roleId = 1 }
        }
        };
        db.Users.Add(user);
    }
    if (!db.Users.Any(u => u.UserName == "testseller"))
    {
        var hasher = new PasswordHasher<User>();
        var user = new User
        {
            UserName = "testseller",
            Password = hasher.HashPassword(null, "123456"),
            Deposit = 0,
            UserRoles = new List<UserRole>
        {
            new UserRole { roleId  =2 }
        }
        };
        db.Users.Add(user);
    }
    if(!db.Products.Any(x=>x.Id == 1))
    {
        Product product = new Product()
        {
            Id = 1,
            AmountAvailable = 10,
            Cost = 20,
            ProductName = "Kitkat",
            SellerId = 1
        };
        db.Products.Add(product);
    }
    db.SaveChanges();

}



app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


namespace FlapKapBackendChallenge
{
    public partial class Program { } // Required for WebApplicationFactory in tests
}
