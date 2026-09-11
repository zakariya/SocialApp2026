using API.Data;
using API.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register services before Build()
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton<CorsCacheService>();

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

app.MapControllers();
app.Run();
