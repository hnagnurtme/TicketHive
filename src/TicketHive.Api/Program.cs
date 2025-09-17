using FluentValidation;
using TicketHive.Application;
using TicketHive.Domain;
using TicketHive.Infrastructure;
using TicketHive.API;
using TicketHive.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi();
builder.Services.AddMappings();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
// automapper


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }