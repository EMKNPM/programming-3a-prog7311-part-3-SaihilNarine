using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PROG7311_POE_.API.Repository;
using PROG7311_POE_.Data;
using PROG7311_POE_API.Repository;
using PROG7311_POE_API.Services;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        //Add Swagger for API documentation
        builder.Services.AddSwaggerGen();
        builder.Services.AddOpenApi();
        //Contract API
        builder.Services.AddScoped<IContractRepository, ContractRepository>();
        builder.Services.AddScoped<IContractService, ContractService>();
        //Client API
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientService, ClientService>();
        //ServiceRequest API
        builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
        builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();

        builder.Services.AddDbContext<PROG7311_POE_Context>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("PROG7311_POE_Context")));

        //Configure JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
        options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "PROG7311",
                ValidAudience = "PROG7311",

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_KEY_12345"))
            };
        });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}