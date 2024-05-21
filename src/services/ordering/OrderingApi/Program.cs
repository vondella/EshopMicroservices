using ordering.application;
using ordering.infrastracture;
using ordering.infrastracture.Data.Extensions;
using OrderingApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationService(builder.Configuration)
    .AddInfrastractureServices(builder.Configuration)
    .AddApiServices();
    
var app = builder.Build();
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}

app.MapGet("/", () => "Hello World!");

app.Run();
