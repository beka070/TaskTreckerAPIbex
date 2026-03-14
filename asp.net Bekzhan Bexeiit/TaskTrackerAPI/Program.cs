using TaskTrackerAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// ── DI registrations ─────────────────────────────────────────────────────────
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddControllers();

// Swagger for easy manual testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
