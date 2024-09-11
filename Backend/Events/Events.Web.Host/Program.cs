using Events.Application.DTOs.Events.Requests.GetAllEvents;
using Events.Application.Interfaces;
using Events.Application.Services;
using Events.Application.Validation;
using Events.Core.Entities;
using Events.Core.Interfaces;
using Events.Infrastructure.Data;
using Events.Infrastructure.Data.Repositories;
using Events.Application.Mapping;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventParticipantService, EventParticipantService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();

builder.Services.AddSingleton<IGuidValidator, GuidValidator>();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<GetAllEventsRequestValidator>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
