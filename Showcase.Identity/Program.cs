using Showcase.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.BootstrapBuilder();

var app = builder.Build();
app.BootstrapApplication();
app.Run();