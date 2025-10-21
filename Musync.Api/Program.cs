using Musync.Api.Contracts.Exceptions;
using Musync.Api.ExceptionHandlers;
using Musync.Api.Middleware;
using Musync.Application;
using Musync.Persistance;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsDevelopment())
    // adding this to dev env to listen to requests on all network interfaces
    builder.WebHost.UseUrls("https://0.0.0.0:7154", "http://0.0.0.0:5000");

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddSwaggerGen();

// Exception handlers
builder.Services.AddScoped<IExceptionHandler, BadRequestExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, NotFoundExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, DefaultExceptionHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty;
        options.InjectStylesheet("swagger-ui/SwaggerDark.css");
    });
}

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
