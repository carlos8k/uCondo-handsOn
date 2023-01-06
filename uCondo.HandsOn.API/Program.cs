using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using uCondo.HandsOn.Business.Services;
using uCondo.HandsOn.Domain.Interfaces.Repositories;
using uCondo.HandsOn.Domain.Interfaces.Services;
using uCondo.HandsOn.Infra.Context;
using uCondo.HandsOn.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<HandsOnDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HandsOnDatabase"));
    options.EnableSensitiveDataLogging();
}, contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);

builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
builder.Services.AddTransient<IAccountsService, AccountsService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HandsOnDbContext>();
    db.Database.Migrate();
}

app.Run();