using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controller service
builder.Services.AddControllers();

// Implementing CORS
builder.Services.AddCors(options => {

    // A default policy which accepts every HTTP header and
    // method only from a restricted set of known origins
    options.AddDefaultPolicy(cfg => {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });

    // Named policy that accepts everything from everyone
    options.AddPolicy(name: "AnyOrigin",
        cfg => {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});

// SemVer-based API versioning

builder.Services.AddApiVersioning(options => {
    // Enables URI versioning
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(options => {

    // Sets the API versioning format as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Replaces the {apiVersion} placeholder with version number
    options.SubstituteApiVersionInUrl = true;
});

// Swagger services
// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// The Swagger Generator service will create a swagger.json file
// that will describe all the endpoints available within
// our Web API using the OpenAPI specification. The service is
// configured to create a JSON documentation file for each version
// we want to support.
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo { Title = "MyBGList", Version = "v1.0" });
    options.SwaggerDoc(
        "v2",
        new OpenApiInfo { Title = "MyBGList", Version = "v2.0" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    // Swagger middlewares

    // Expose swagger.json file using a configurable endpoint
    // (default is /swagger/v1/swagger.json)
    app.UseSwagger();

    // Enable a handy User Interface that can be used to visually
    // see and interactively browse the documentation and make sure
    // that SwaggerUI will load the different swagger.json files
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
            $"/swagger/v1/swagger.json",
            $"MyBGList v1");
        options.SwaggerEndpoint(
            $"/swagger/v2/swagger.json",
            $"MyBGList v2");
    });
}

// Retrieve the literal value from the appsettings.json file(s)
if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
    // HTTP pipeline exception handling middleware
    app.UseDeveloperExceptionPage();
else
    // HTTP pipeline exception handling for end users middleware
    app.UseExceptionHandler("/error");

// HTTP to HTTPS redirection middleware
app.UseHttpsRedirection();

// Applying CORS
app.UseCors();

// ASP.NET Core authorization middleware
app.UseAuthorization();

// Handles exceptions for end users
app.MapGet("/v{version:ApiVersion}/error",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
        Results.Problem());

// For testing exceptions handling
app.MapGet("/v{version:ApiVersion}/error/test",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
        { throw new Exception("test"); });

app.MapGet("/v{version:ApiVersion}/cod/test",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
        Results.Text("<script>" +
            "window.alert('Your client supports JavaScript!" +
            "\\r\\n\\r\\n" +
            $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
            "\\r\\n" +
            "Client time (UTC): ' + new Date().toISOString());" +
            "</script>" +
            "<noscript>Your client does not support JavaScript</noscript>",
            "text/html"));

// Controller middleware
app.MapControllers();

app.Run();
