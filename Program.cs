using Microsoft.EntityFrameworkCore;
using Npgsql;
using VeterinariaApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var rawConnection = builder.Configuration.GetConnectionString("Veterinaria")
    ?? throw new InvalidOperationException("Configure ConnectionStrings__Veterinaria como variable de entorno.");
builder.Services.AddDbContext<VeterinariaContext>(options => options.UseNpgsql(ToNpgsqlConnectionString(rawConnection)));
builder.Services.AddCors(options => options.AddPolicy("FrontendVeterinaria", policy =>
    policy.WithOrigins("https://veterinaria-web.example.com").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("FrontendVeterinaria");
app.MapControllers();
app.Run();

static string ToNpgsqlConnectionString(string value)
{
    // Render entrega PostgreSQL como URI; Npgsql usa pares clave=valor.
    if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
        (uri.Scheme != "postgres" && uri.Scheme != "postgresql")) return value;

    var userInfo = Uri.UnescapeDataString(uri.UserInfo).Split(':', 2);
    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port > 0 ? uri.Port : 5432,
        Database = uri.AbsolutePath.Trim('/'),
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : string.Empty
    };
    return builder.ConnectionString;
}
