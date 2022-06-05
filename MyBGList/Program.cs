var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Controller service
builder.Services.AddControllers();

// Swagger services
// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
    opts.ResolveConflictingActions(apiDesc => apiDesc.First()));

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
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

// ASP.NET Core authorization middleware
app.UseAuthorization();

// Handles exceptions for end users
app.MapGet("/error", () => Results.Problem());

// For testing exception handling
app.MapGet("/error/test", () => { throw new Exception("test"); });

// Controller middleware
app.MapControllers();

app.Run();
