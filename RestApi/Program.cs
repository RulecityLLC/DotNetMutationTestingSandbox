using DotNetMutationTestingSandbox.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection: Register layers
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

Console.WriteLine("Starting server...");
Console.WriteLine("Available endpoints:");
Console.WriteLine("  GET    /api/users - Get all users");
Console.WriteLine("  GET    /api/users/{id} - Get user by ID");
Console.WriteLine("  POST   /api/users - Create new user");
Console.WriteLine("  GET    /health - Health check");
Console.WriteLine("  Swagger UI: /swagger");

app.Run();
