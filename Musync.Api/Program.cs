using HR.LeaveManagement.Api.Middleware;
using Musync.Api.Contracts.Exceptions;
using Musync.Api.ExceptionHandlers;
using Musync.Application;
using Musync.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// Exception handlers
builder.Services.AddScoped<IExceptionHandler, BadRequestExceptionHandler>();
builder.Services.AddScoped<IExceptionHandler, DefaultExceptionHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
