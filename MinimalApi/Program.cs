using MinimalApi;

var builder = WebApplication.CreateBuilder(args).ConfigureBuilder();
await using var app = builder.Build().ConfigureApp().ConfigureEndpoints();
await app.RunAsync();