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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    // Swagger middlewares
    app.UseSwagger();
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
app.MapGet("/error", [EnableCors("AnyOrigin")] () => 
    Results.Problem());

// For testing exceptions handling
app.MapGet("/error/test", [EnableCors("AnyOrigin")] () => 
    { throw new Exception("test"); });

// Controller middleware
app.MapControllers();

app.Run();
