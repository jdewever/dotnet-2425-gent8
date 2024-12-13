using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Rise.Client;
using Client.Auth;
using Rise.Shared.Products;
using Rise.Client.Products;
using Rise.Shared.Barcodes;
using Rise.Shared.Cart;
using Rise.Client.Cart;
using Blazored.Modal;
using Blazored.LocalStorage;
using Blazored.Toast;
using Rise.Client.Scan;
using Rise.Client.Auth;
using Rise.Client.Reservation;
using Rise.Shared.Booking;
using Rise.Shared.Transaction;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Rise.Shared.User;
using Rise.Client.Manage;
using Rise.Client.Transactions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

try
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddBlazoredLocalStorage();

    builder.Services.AddSingleton<BarcodeService>();
    builder.Services.AddSingleton<ScanService>();

    builder.Services.AddBlazoredToast();

    builder.Services.AddHttpClient<IProductService, ProductService>("360zorg",
        client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<ICategoryService, CategoryService>("360zorg",
        client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IBookingService, BookingService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
builder.Services.AddHttpClient<IUserService, UserService>("360zorg",
        client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<IBookingService, BookingService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
       .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

    builder.Services.AddHttpClient<IBarcodeService, BarcodeService>("360zorg", client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
        .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
    builder.Services.AddHttpClient<ITransactionService, TransactionService>("360zorg",
        client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/"))
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
    builder.Services.AddScoped<Rise.Client.Auth.IUserService, Rise.Client.Auth.UserService>();

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
