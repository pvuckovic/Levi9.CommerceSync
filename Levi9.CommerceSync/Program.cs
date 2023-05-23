using Levi9.CommerceSync;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Domain;
using Levi9.CommerceSync.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SyncDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("SyncDatabase")));

builder.Services.AddScoped<IErpConnectionService,  ErpConnectionService>();
builder.Services.AddScoped<IErpConnection, ErpConnection>();

builder.Services.AddScoped<IPosConnectionService, PosConnectionService>();
builder.Services.AddScoped<IPosConnection, PosConnection>();

builder.Services.AddScoped<ISyncRepository, SyncRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<SyncDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        //var logger = services.GetRequiredService<ILogger<Program>>();
        //logger.LogError(ex, "An error occurred while migrating the database.");
    }
}


app.MapControllers();


app.Run();

