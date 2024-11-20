using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Client;
using Client.Auth;
using Rise.Shared.Products;
using Rise.Client.Products;
using Rise.Shared.Cart;
using Rise.Client.Cart;
using Blazored.Modal;
using Blazored.LocalStorage;
using Blazored.Toast;
using Rise.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<BarcodeService>();
builder.Services.AddHttpClient<UserService,UserService>("360zorg", client =>client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/")).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddBlazoredToast();

builder.Services.AddHttpClient<IProductService, ProductService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
       .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<ICategoryService, CategoryService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
       .CreateClient("360zorg"));

builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOidcAuthentication(options =>
{
       builder.Configuration.Bind("Auth0", options.ProviderOptions);
       options.ProviderOptions.ResponseType = "code";
       options.ProviderOptions.PostLogoutRedirectUri = builder.HostEnvironment.BaseAddress;
       options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]!);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();
builder.Services.AddBlazoredModal();

await builder.Build().RunAsync();
