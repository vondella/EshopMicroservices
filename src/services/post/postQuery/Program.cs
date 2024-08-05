


using buildingBlock.Behaviours;
using buildingBlock.Exceptions.Handler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using postQuery.Infrastractur.Handlers;

var builder = WebApplication.CreateBuilder(args);
Action<DbContextOptionsBuilder> configureDbContext;
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if(env.Equals("Development.PostgreSQL"))
{

    configureDbContext = (o => o.UseNpgsql(builder.Configuration.GetConnectionString("SqlServer2")));
}else
{
    configureDbContext=(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
}
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

});

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, posQuery.Infrastracture.Handlers.EventHandler>();
builder.Services.AddScoped<IEventPubHandler, EventPubHandler>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventHandler, posQuery.Infrastracture.Handlers.EventHandler>();
//builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());
var app = builder.Build();
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();
