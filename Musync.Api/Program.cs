using Musync.Api.Contracts.Exceptions;
using Musync.Api.ExceptionHandlers;
using Musync.Api.Middleware;
using Musync.Application;
using Musync.Persistance;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    // adding this to dev env to listen to requests on all network interfaces
    builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(5000));

builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

// Configure Swagger with JWT bearer support so the injected JS can preauthorize
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });

    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            new string[] { }
        }
    });
});

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
        // Inject custom JS that adds the Auto-Login button and performs login
        options.InjectJavascript("swagger-ui/swagger-auth.js");
    });
}

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
