using AnimedApi.Data;
using AnimedApi.Repositories;
using AnimedApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnimedDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("AnimedDatabase"));
});

builder.Services.AddScoped<TutorRepository>();
builder.Services.AddScoped<PetRepository>();
builder.Services.AddScoped<ConsultaRepository>();
builder.Services.AddScoped<VacinaRepository>();

builder.Services.AddScoped<TutorService>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<VacinaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTudo");

app.UseAuthorization();

app.MapControllers();

app.Run();