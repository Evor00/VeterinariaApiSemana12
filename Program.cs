using Microsoft.EntityFrameworkCore;
using VeterinariaApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VeterinariaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Veterinaria")
        ?? throw new InvalidOperationException("Configure ConnectionStrings__Veterinaria como variable de entorno.")));
builder.Services.AddCors(options => options.AddPolicy("FrontendVeterinaria", policy =>
    policy.WithOrigins("https://veterinaria-web.example.com").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("FrontendVeterinaria");
app.MapControllers();
app.Run();
