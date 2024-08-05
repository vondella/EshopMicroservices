
var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCarter();
builder.Services.AddScoped<IItemRepository<Item>, ItemRepository<Item>>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

// Add services to the container.
//var config=builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));

var settings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddMongoSettings(builder.Configuration)
    .AddMongoRepository<Item>()
    .AddCatalogAuthentication();

builder.Services.AddMessageBroker(builder.Configuration, assembly);

builder.Services.AddGrpcClient<InventoryItemService.InventoryItemServiceClient>(option =>
{
    option.Address = new Uri(builder.Configuration["GrpcSettings:InventoryUrl"]!);
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.Read, policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("scope", "catalog.readaccess", "catalog.fullaccess");
    });

    options.AddPolicy(Policies.Write, policy =>
    {
        policy.RequireRole("Admin");
        policy.RequireClaim("scope", "catalog.writeaccess", "catalog.fullaccess");
    });
});
var app = builder.Build();
app.UseExceptionHandler(option => { });
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();

