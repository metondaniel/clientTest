using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Product.Middleware;
using Product.Repository;
using Product.Repository.Data;
using Product.Repository.EventSourcing;
using Product.Repository.Services;
using Product.Service;
using Product.Service.Entities;
using Product.Service.Interfaces.EventSourcing;
using Product.Service.Interfaces;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces.Services;
using Product.Service.Validators;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using Product.Service.Commands;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString,
    sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// Configuração do CQRS com MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(CreateClienteCommand).Assembly,
        typeof(Program).Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Configuração do Event Sourcing
builder.Services.AddScoped<IEventStore, SqlEventStore>();

// Configuração de repositórios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpClient<ICpfCnpjService, BrasilApiCpfCnpjService>(client =>
{
    client.BaseAddress = new Uri("https://brasilapi.com.br/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
// Configuração de serviços de domínio
builder.Services.AddSingleton<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<IRequestHandler<CreateClienteCommand, Guid>, ClienteCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateClienteCommand>, UpdateClienteCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteClienteCommand>, DeleteClienteCommandHandler>();
// Unit of Work e Repositório
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();


builder.Services.AddScoped<IValidator<DeleteClienteCommand>, DeleteClienteValidator>();
builder.Services.AddScoped<IValidator<UpdateClienteCommand>, UpdateClienteValidator>();

// Ou registre todos os validadores de uma vez
builder.Services.AddValidatorsFromAssemblyContaining<DeleteClienteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateClienteValidator>();

// Serviços de Domínio
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// Configuração do MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateClienteCommand).Assembly));

// Configurar pipeline de validação do MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cliente API", Version = "v1" });

});

// Configuração dos Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });



var app = builder.Build();

// Executar migrações do banco de dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao executar migrações do banco de dados");
    }
}

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cliente API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AngularClient");
app.UseRouting();

// Middleware de tratamento global de erros
app.UseExceptionHandler("/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de health check
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

app.Run();