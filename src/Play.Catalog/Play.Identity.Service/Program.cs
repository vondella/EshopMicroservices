
using buildingBlock.Exceptions;
using Play.Identity.Service.Infrastracture.HostedService;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection(nameof(IdentityServerSettings)));
builder.Services.Configure<IdentitySettings>(builder.Configuration.GetSection(nameof(IdentitySettings)));

var identitySettings = builder.Configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>();
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddIdentityMongo(builder.Configuration);
builder.Services.AddUserIdentityServer(identitySettings);
builder.Services.AddLocalApiAuthentication();
builder.Services.AddHostedService<IdentitySeedHostedService>();
builder.Services.AddMessageBroker(builder.Configuration, assembly,
    retryConfigurator =>
{
    retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
    retryConfigurator.Ignore(typeof(UnknownUserException));
    retryConfigurator.Ignore(typeof(InsufficientFundsException));

});


var app = builder.Build();
app.UseCors(build =>
{
    build.WithOrigins(builder.Configuration[AdminConstants.AllowedOriginSettings]).AllowAnyHeader().AllowAnyMethod();
});

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();
