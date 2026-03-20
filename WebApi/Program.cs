using Application.Common.Behaviors;
using Application.Features.Email.Commands;
using Application.Features.Email.Validators;
using FluentValidation;
using Hangfire;
using Infrastructure.DependencyInjection;
using MediatR;
using Serilog;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddHangfire(cfg =>
{
    cfg.UseSqlServerStorage(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddHangfireServer();
builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(SendEmailCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(SendEmailValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
builder.Host.UseSerilog();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
