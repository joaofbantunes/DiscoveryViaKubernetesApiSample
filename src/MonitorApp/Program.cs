using MonitorApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<KubernetesMonitor>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();