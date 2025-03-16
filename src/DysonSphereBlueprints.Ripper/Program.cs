using DysonSphereBlueprints.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace DysonSphereBlueprints.Ripper;

static class Program
{
    static async Task Main()
    {
        using ServiceProvider sp = new ServiceCollection()
            .AddLogging(s => s
                .AddSimpleConsole(s =>
                {
                    s.IncludeScopes = false;
                    s.SingleLine = true;
                })
                .AddFilter("DysonSphereBlueprintsRipper", LogLevel.Debug)
                .AddFilter("System.Net.Http.HttpClient", LogLevel.Warning)
                .AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.Warning)
            )
            .AddHttpClient("polly")
            .AddTransientHttpErrorPolicy(policyBuilder =>
                policyBuilder.WaitAndRetryAsync(
                    3, retryNumber => TimeSpan.FromMilliseconds(600)))
            .Services
            .AddDbContext<BlueprintContext>(s => s.UseSqlite("Data Source=.\\..\\..\\..\\..\\..\\_Local\\blueprints.db"), ServiceLifetime.Singleton)
            .AddSingleton<Runner>()
            .BuildServiceProvider();

        BlueprintContext db = sp.GetRequiredService<BlueprintContext>();
        await db.Database.EnsureCreatedAsync();

        Runner runner = sp.GetRequiredService<Runner>();
        await runner.Run(CancellationToken.None);
    }
}