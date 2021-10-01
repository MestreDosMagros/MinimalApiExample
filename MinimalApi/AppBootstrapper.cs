using Application;
using Infra;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinimalApi
{
    public static class AppBootstrapper
    {
        public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(opts =>
            {
                var connString = builder.Configuration.GetConnectionString("MyConnectionString");
                opts.UseSqlServer(connString, options =>
                {
                    options.MigrationsAssembly(typeof(Context).Assembly?.FullName?.Split(',')[0]);
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "MinimalApi", Version = "v1" });
            });

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddTransient<IMyEntityService, MyEntityService>();

            return builder;
        }

        public static WebApplication ConfigureApp(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinimalApi");
            });

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseHttpsRedirection();
            app.UseResponseCaching();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        var exception = exceptionHandlerFeature.Error;

                        var problemDetails = new ProblemDetails
                        {
                            Instance = context.Request.HttpContext.Request.Path
                        };

                        if (exception is BadHttpRequestException badHttpRequestException)
                        {
                            problemDetails.Title = "The request is invalid";
                            problemDetails.Status = StatusCodes.Status400BadRequest;
                            problemDetails.Detail = badHttpRequestException.Message;
                        }
                        else
                        {
                            problemDetails.Title = exception.Message;
                            problemDetails.Status = StatusCodes.Status500InternalServerError;
                            problemDetails.Detail = exception.ToString();
                        }

                        context.Response.StatusCode = problemDetails.Status.Value;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    }
                });
            });

            return app;
        }

        public static WebApplication ConfigureEndpoints(this WebApplication app)
        {
            app.MapGet("/entities", ([FromServices] IMyEntityService myEntityService) => MyEntityControllerDelegates.GetAll(myEntityService))
                .Produces(statusCode: 200, responseType: typeof(IEnumerable<MyEntityDto>), contentType: "application/json");

            app.MapPost("/entities", ([FromServices] IMyEntityService myEntityService, MyEntityDto entityDto) => MyEntityControllerDelegates.Add(myEntityService, entityDto))
                .Accepts(requestType: typeof(MyEntityDto), contentType: "application/json")
                .Produces(statusCode: 200, responseType: typeof(IEnumerable<MyEntityDto>), contentType: "application/json");

            app.MapPut("/entities", ([FromServices] IMyEntityService myEntityService, [FromBody] MyEntityDto entityDto) => MyEntityControllerDelegates.Update(myEntityService, entityDto))
                .Accepts(requestType: typeof(MyEntityDto), contentType: "application/json")
                .Produces(statusCode: 200, responseType: typeof(IEnumerable<MyEntityDto>), contentType: "application/json");

            app.MapDelete("/entities/{id}", ([FromServices] IMyEntityService myEntityService, [FromRoute] Guid id) => MyEntityControllerDelegates.Delete(myEntityService, id))
                .Produces(statusCode: 200, responseType: typeof(IEnumerable<MyEntityDto>), contentType: "application/json");

            return app;
        }
    }
}
