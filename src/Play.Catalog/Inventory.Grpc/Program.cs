

using Inventory.Grpc;
using Polly;
using Polly.Timeout;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddHttpClient<CatalogClient>(cleint =>
{
    cleint.BaseAddress = new Uri("htto://localhost:7002");
}).AddTransientHttpErrorPolicy(building=>building.Or<TimeoutRejectedException>().WaitAndRetryAsync(5,
retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
onRetry: (outcome,timespan,retryAttempt) =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClient>>()?
    .LogWarning($"Delaying for {timespan.TotalSeconds} seconds then making retry {retryAttempt}");
    
}))
.AddTransientHttpErrorPolicy(building => building.Or<TimeoutRejectedException>()
.CircuitBreakerAsync(3, TimeSpan.FromSeconds(15), 
onBreak: (outcome,timespan) => 
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClient>>()?.LogWarning($"closing the circuit for {timespan.TotalSeconds} seconds ...");
    
},
onReset:() =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    serviceProvider.GetService<ILogger<CatalogClient>>()?
    .LogWarning($"closing the circuit");

}
))
  .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
builder.Services.AddInventoryAUthentication();
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapGrpcService<GrpcInventoryService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
