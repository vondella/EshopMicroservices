
var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
var mongoConfig = builder.Configuration.GetSection(nameof(MongoConfig)).Get<MongoConfig>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCarter();
builder.Services.AddMongoSettings(builder.Configuration)
    .AddMongoRepository<CatalogItem>()
    .AddMongoRepository<InventoryItem>()
    .AddMongoRepository<ApplicationUser>()
    .AddTradingAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddMessageBroker(builder.Configuration,mongoConfig,assembly);

var app = builder.Build();
app.UseExceptionHandler(option => { });
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();
