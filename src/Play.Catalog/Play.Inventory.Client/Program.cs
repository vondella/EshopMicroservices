


var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddMongoSettings(builder.Configuration)
    .AddMongoRepository<CatalogItem>()
    .AddMongoRepository<InventoryItem>()
    .AddInventoryAuthentication();

builder.Services.AddAuthorization();
builder.Services.AddMessageBroker(builder.Configuration, assembly,
    retryConfigurator => {
        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
        retryConfigurator.Ignore(typeof(UnknownItemException));
        });

var app = builder.Build();
app.UseExceptionHandler(option => { });
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.MapGet("/", () => "Hello World!");

app.Run();
