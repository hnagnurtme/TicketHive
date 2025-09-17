using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketHive.Application.Interfaces.Repositories;
using TicketHive.Application.Interfaces;
using TicketHive.Infrastructure.Persistence;
using TicketHive.Infrastructure.Persistence.Repositories;
using TicketHive.Infrastructure.Persistence.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core - PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsql =>
    {
        npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
    });
});

// MediatR - scan application assembly (v11 registration style)
builder.Services.AddMediatR(typeof(IUserRepository).Assembly);

// Repositories / DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();


