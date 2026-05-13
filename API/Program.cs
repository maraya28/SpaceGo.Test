using API.Configuration;
using API.Middlewares;
using Application;
using Application.Contracts;
using Application.Implementations;
using Infrastructure.Contracts;
using Infrastructure.Implementations;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();

var settings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.SecretKey))
        };
    });

builder.Services.AddEndpointsApiExplorer(); //
builder.Services.AddSwaggerGen(); //

builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<IWallet, WalletService>();

builder.Services.AddDbContext<SpaceGoDbContext>(dbContext => dbContext.UseInMemoryDatabase("SpaceGo"));
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

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
