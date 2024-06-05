



var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();
 //await ApplyMigrations();
app.MapCarter();
app.Run();

 async Task  ApplyMigrations()
{
    using(var scope =app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var results = await _db.Database.GetAppliedMigrationsAsync();
        if (results.Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}

