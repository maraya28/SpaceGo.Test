using API.Configuration;
using API.Exceptions;
using API.Extensions;
using API.Middlewares;
using Application.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Persistance;
using System.Text.Json.Serialization;
using static Domain.SlotDefinition;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter<Symbol>());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var settings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthorizationSetup(settings!);

builder.Services.AddEndpointsApiExplorer(); //
builder.Services.AddSwaggerGen(); //

builder.Services.AddAplicationSetup();
builder.Services.AddInfrastructureSetup();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMiddleware>();

app.UseExceptionHandler();

await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbcontext = serviceScope.ServiceProvider.GetRequiredService<SpaceGoDbContext>())
{
    await dbcontext.Database.EnsureCreatedAsync();
}

app.MapControllers();

app.Run();
