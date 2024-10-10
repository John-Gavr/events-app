using Events.Application.DTOs.Events.Requests.GetAllEvents;
using Events.Application.Interfaces;
using Events.Application.Mapping;
using Events.Application.Services;
using Events.Application.Validation;
using Events.Core.Entities;
using Events.Core.Interfaces;
using Events.Infrastructure.Data.Repositories;
using Events.Infrastructure.Data;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost3000", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:5000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
        });

        services.AddHttpContextAccessor();
        services.AddAuthentication();
        services.AddAuthorization();

        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventParticipantService, EventParticipantService>();
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IUserDataService, UserDataService>();

        services.AddSingleton<IGuidValidator, GuidValidator>();

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<GetAllEventsRequestValidator>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
