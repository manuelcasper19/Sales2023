using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.API.Services;
using Sales.Shared.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Para evitar la redundancia ciclica en la respuesta de los JSON vamos a agregar
//la siguiente configuraci�n, modificamos el Program del API
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>( c => c.UseSqlServer("name=ConecctionBD"));
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, ApiService>();

//Inyectamos User, interface IUserHelper y su implementaci�n
//le decimos a la aplicaci�n como se manejraron los usuarios
builder.Services.AddIdentity<User, IdentityRole>(u =>
{
    u.User.RequireUniqueEmail = true;
    u.Password.RequireDigit = false;
    u.Password.RequiredUniqueChars = 0;
    u.Password.RequireLowercase = false;
    u.Password.RequireNonAlphanumeric = false;
    u.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserHelper, UserHelper>();


var app = builder.Build();

SeedData(app);
//metodo que crea los dato en bd, usnado inyeccion manual, esta clase no tien para inyectar por constructor
void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//habilitamos autenticac�n
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());





app.Run();
