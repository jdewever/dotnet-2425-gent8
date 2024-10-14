using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Client;
using Client.Auth;
using Rise.Shared.Products;
using Rise.Client.Products;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IProductService, ProductService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
       .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
       .CreateClient("360zorg"));

builder.Services.AddOidcAuthentication(options =>
{
       builder.Configuration.Bind("Auth0", options.ProviderOptions);
       options.ProviderOptions.ResponseType = "code";
       options.ProviderOptions.PostLogoutRedirectUri = builder.HostEnvironment.BaseAddress;
       options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]!);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

await builder.Build().RunAsync();
