using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

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

// Swagger services
// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// The Swagger Generator service will create a swagger.json file
// that will describe all the endpoints available within
// our Web API using the OpenAPI specification
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    // Swagger middlewares

    // Expose swagger.json file using a configurable endpoint
    // (default is /swagger/v1/swagger.json)
    app.UseSwagger();

    // Enable a handy User Interface (/swagger) that can be used 
    // to visually see and interactively browse the documentation
    app.UseSwaggerUI();
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
app.MapGet("/error", 
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () => 
    Results.Problem());

// For testing exceptions handling
app.MapGet("/error/test", 
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () => 
    { throw new Exception("test"); });

app.MapGet("/cod/test",
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
