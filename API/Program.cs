using System.Text;
using API.Data;
using API.Data.Migrations;
using API.Helpers;
using API.Interfaces;
using API.Middleware;
using API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register services before Build()
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddSingleton<CorsCacheService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration
                .GetSection("CloudinarySettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("TOken key not found");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                });



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// ✅ Load initial origins into cache
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var cache = scope.ServiceProvider.GetRequiredService<CorsCacheService>();

        await db.Database.MigrateAsync();
        await Seed.SeedUsers(db);

        var origins = db.Aggregators
            .Where(c => c.SubscriptionExpiry > DateTime.UtcNow)
            .Select(c => c.Url)
            .ToList();

        cache.LoadOrigins(origins);


    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }

}

// ✅ Configure CORS policy globally without BuildServiceProvider
app.UseCors(policy =>
{
    // Resolve CorsCacheService from the already built app.Services
    var cache = app.Services.GetRequiredService<CorsCacheService>();

    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed(origin => cache.IsAllowed(origin));
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
