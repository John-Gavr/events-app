using Events.Core.Entities;
using Events.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowLocalhost3000");

app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DataSeeder.SeedAsync(app.Services);

app.Run();
