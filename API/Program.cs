using System.Text;
using API.Data;
using API.Interfaces;
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
builder.Services.AddSingleton<CorsCacheService>();
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

// ✅ Load initial origins into cache
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var cache = scope.ServiceProvider.GetRequiredService<CorsCacheService>();

    var origins = db.Aggregators
        .Where(c => c.SubscriptionExpiry > DateTime.UtcNow)
        .Select(c => c.Url)
        .ToList();

    cache.LoadOrigins(origins);
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
