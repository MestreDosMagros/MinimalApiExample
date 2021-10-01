using Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Context>(opts =>
{
    var connString = builder.Configuration.GetConnectionString("MyConnectionString");
    opts.UseSqlServer(connString, options =>
    {
        options.MigrationsAssembly(typeof(Context).Assembly.FullName.Split(',')[0]);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MinimalApi", Version = "v1" });
});

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApi");
});

app.MapGet("/", (Func<string>)(() => "Hello World!"));

await app.RunAsync();