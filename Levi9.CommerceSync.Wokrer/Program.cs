using Levi9.CommerceSync;
using Levi9.CommerceSync.Connection;
using Levi9.CommerceSync.Connections;
using Levi9.CommerceSync.ConnectionServices;
using Levi9.CommerceSync.Domain;
using Levi9.CommerceSync.Domain.Repositories;
using Levi9.CommerceSync.Worker.Jobs;
using Levi9.CommerceSync.Worker.Options;
using Microsoft.EntityFrameworkCore;
using Quartz;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<SyncDbContext>(options =>
        {
            options.UseSqlServer(context.Configuration.GetConnectionString("SyncDatabase"));
        });
        services.AddQuartz(opt =>
        {
            opt.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("SyncData");
            opt.AddJob<SyncDataJob>(options => options.WithIdentity(jobKey));
            opt.AddTrigger(options =>
            {
                options.ForJob(jobKey)
                .WithIdentity("SyncData-trigger")
                .WithCronSchedule(context.Configuration.GetSection("SyncData:CronSchedule").Value ?? "0 0/5 * ? * *");
            });
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddScoped<IErpConnectionService, ErpConnectionService>();
        services.AddScoped<IPosConnectionService, PosConnectionService>();
        services.AddScoped<IErpConnection, ErpConnection>();
        services.AddScoped<IPosConnection, PosConnection>();
        services.AddScoped<ISyncRepository, SyncRepository>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<SyncDataJobOptions>(context.Configuration.GetSection(SyncDataJobOptions.SyncDataJobOptionsKey));
    })
    .Build();

    using var scope = host.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetService<SyncDbContext>();
host.Run();

