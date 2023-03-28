using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sales.WEB;
using Sales.WEB.Auth;
using Sales.WEB.Repository;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//Http lo cambiamos de scope a singleton para no estar enviando token de acceso
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7295/") });
builder.Services.AddScoped<IRepository,  Repository>();
builder.Services.AddSweetAlert2();

builder.Services.AddBlazoredModal();

builder.Services.AddAuthorizationCore();
//Inyecatmos las autenticaciones del usuario
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());



await builder.Build().RunAsync();
