using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PROG7311_POE_.Data;
using PROG7311_POE_.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PROG7311_POE_Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROG7311_POE_Context") ?? throw new InvalidOperationException("Connection string 'PROG7311_POE_Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Currency Services
builder.Services.AddHttpClient<CurrencyService>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
