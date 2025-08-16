using Ambev.Ominia.Api;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

Bootstrap.ConfigureServices(builder);

var app = builder.Build();

Bootstrap.ConfigureMiddleware(app);

app.Run();