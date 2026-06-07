using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROG7311_POE_.Data;
using PROG7311_POE_.Services;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<PROG7311_POE_Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("PROG7311_POE_Context") ?? throw new InvalidOperationException("Connection string 'PROG7311_POE_Context' not found.")));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //Add HttpClient for API calls for Contracts
        builder.Services.AddHttpClient<ContractApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7224/");
        });

        //Add HttpClient for API calls for Clients
        builder.Services.AddHttpClient<ClientApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7224/");
        });

        //Add HttpClient for API calls for Service Requests
        builder.Services.AddHttpClient<ServiceRequestApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7224/");
        });

        //Add Currency Services
        builder.Services.AddHttpClient<CurrencyService>();

        //Add Session services
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        builder.Services.AddHttpClient<AuthenticationApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7224/");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseSession();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();


        app.Run();
    }
}