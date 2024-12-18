using System.Data;
using Services;
using Services.Interfaces;
using Db;
using Microsoft.Data.Sqlite;
using Repositories;
using FluentValidation;
using Models;
using Validators;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


{
    var Services = builder.Services;

    // Database connection
    var connectionString =
        builder.Configuration.GetConnectionString("ReservationsDb")
        ?? "Data Source=reservations.db;Cache=Shared";

    Services.AddTransient(_ => new SqliteConnection(connectionString));
    Services.AddTransient<IDbConnection>(sp => sp.GetRequiredService<SqliteConnection>());


    // Repositories
    Services.AddScoped<IGuestRepository, GuestRepository>();
    Services.AddScoped<IRoomRepository, RoomRepository>();
    Services.AddScoped<IReservationRepository, ReservationRepository>();

    // Services
    Services.AddScoped<IReservationService, ReservationService>();
    Services.AddScoped<IRoomService, RoomService>();
    Services.AddScoped<IGuestService, GuestService>();

    // Validators 
    Services.AddScoped<IValidator<ReservationRequest>, ReservationValidator>();

    // Authentication
    Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/staff/login";
            options.Cookie.Name = "access";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

    Services.AddAuthorization();

    // Other services
    Services.AddMvc(opt =>
    {
        opt.EnableEndpointRouting = false;
    });
    Services.AddCors();
    Services.AddEndpointsApiExplorer();
    Services.AddSwaggerGen();
}

var app = builder.Build();


{
    try
    {
        Setup.EnsureDb(app.Services.CreateScope());
    }
    catch (Exception ex)
    {
        Console.WriteLine("Failed to setup the database, aborting");
        Console.WriteLine(ex.ToString());
        Environment.Exit(1);
        return;
    }

    app.UsePathBase("/api")
        .UseMvc()
        .UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
        .UseAuthentication()
        .UseAuthorization()
        .UseSwagger()
        .UseSwaggerUI();
}

app.Run();
