

using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<PostEventCreated>();
BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
BsonClassMap.RegisterClassMap<PostLikedEvent>();
BsonClassMap.RegisterClassMap<CommentAddedEvent>();
BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
BsonClassMap.RegisterClassMap<PostRemovedEvent>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
//builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer,EventProducer>();
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregates>, EventSourcingHandler>();

//builder.Services.AddScoped<IEventCommandHandler, EventCommandHandler>();

// Add services to the container.
//var eventCommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<IEventCommandHandler>();
//var dispatcher = new CommandDispatcher();
//dispatcher.RegisterHandler<NewPostCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<EditMessageCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<LikePostCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<AddCommentCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<EditCommentCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<RemoveCommentCommand>(eventCommandHandler.HandleAsync);
//dispatcher.RegisterHandler<DeletePostCommand>(eventCommandHandler.HandleAsync);
//builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);


var app = builder.Build();
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();

