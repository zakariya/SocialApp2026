using System.Text;
using API.Controllers;
using API.Data;
using API.Data.Migrations;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Middleware;
using API.Service;
using API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register services before Build()
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    opt.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
});


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddSingleton<CorsCacheService>();
builder.Services.AddSingleton<LogUserActivity>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration
                .GetSection("CloudinarySettings"));
builder.Services.AddSignalR();
builder.Services.AddSingleton<PresenceTracker>();

builder.Services.AddIdentityCore<AppUser>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

builder.Services.AddAuthorizationBuilder()
       .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
       .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// ✅ Load initial origins into cache
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var cache = scope.ServiceProvider.GetRequiredService<CorsCacheService>();

        await db.Database.MigrateAsync();
        await db.Connections.ExecuteDeleteAsync();
        await Seed.SeedUsers(userManager);

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
          .AllowCredentials()
          .SetIsOriginAllowed(origin => cache.IsAllowed(origin));
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/messages");
app.MapFallbackToController("Index", "FallBack");
app.Run();
