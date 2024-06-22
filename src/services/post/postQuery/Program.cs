


using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

//var connectionstring = builder.Configuration.GetConnectionString("SqlServer");

Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();

dataContext.Database.EnsureCreated();
builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, posQuery.Infrastracture.Handlers.EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddScoped<IEventHandler, posQuery.Infrastracture.Handlers.EventHandler>();
builder.Services.AddHostedService<ConsumerHostedService>();
builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

// Add services to the container.
var app = builder.Build();
// Configure the HTTP request pipeline.
app.Run();
