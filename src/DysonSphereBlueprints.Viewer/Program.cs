using System.Globalization;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DysonSphereBlueprints.Analysis;
using DysonSphereBlueprints.Analysis.Analysis;
using DysonSphereBlueprints.Db;
using DysonSphereBlueprints.Gamelibs.Code;
using DysonSphereBlueprints.Gamelibs.Code.Patchwork;
using DysonSphereBlueprints.ItemStore.Enums;
using DysonSphereBlueprints.Viewer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DysonSphereBlueprints.Viewer;

static class Program
{
    static void Main()
    {
        AA();

        using ServiceProvider sp = new ServiceCollection()
            .AddLogging(s => s
                .AddSimpleConsole(s => s.SingleLine = true)
                .AddFilter("DysonSphereBlueprintsRipper", LogLevel.Debug)
                .AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.Warning)
                .AddFilter("System.Net.Http.HttpClient", LogLevel.Warning))
            .AddDbContext<BlueprintContext>(
                s => s.UseSqlite("Data Source=.\\..\\..\\..\\..\\..\\_Local\\blueprints.db"), ServiceLifetime.Singleton)
            .BuildServiceProvider();

        ILogger logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(Program));

        BlueprintContext db = sp.GetRequiredService<BlueprintContext>();

        List<BlueprintAnalysisPair> analyzed = Load(db.Blueprints, logger)
            .ToList();

        var transformed = analyzed.Select(analysis =>
        {
            Blueprint original = db.Blueprints.Find(analysis.Blueprint.Id) ?? throw new InvalidOperationException();

            return new
            {
                Id = analysis.Blueprint.Id,
                Title = analysis.Blueprint.Title,
                Description = analysis.Blueprint.Description,
                Url = original.Url,
                BuildingsCount = analysis.Analysis.BuildingCount,
                Produces = string.Join(" ", analysis.Analysis.RecipeCounts.Keys),
                Buildings = string.Join(" ", analysis.Analysis.BuildingCounts.Keys),
                PowerUsageLow = analysis.Analysis.PowerUsage.Low,
                PowerUsageHigh = analysis.Analysis.PowerUsage.High,
                PowerProvidesLow = analysis.Analysis.PowerProvides.Low,
                PowerProvidesHigh = analysis.Analysis.PowerProvides.High,
            };
        });

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Blueprints");

            // worksheet.Cell("A1").Value = "Hello World!";
            worksheet.Cell("A1").InsertTable(transformed);

            workbook.SaveAs(
                @"C:\Users\MichaelBisbjerg\OneDrive - MBWarez\Documents\Dyson Sphere Program\DSP Blueprints.xlsx");
        }

        // using (FileStream fsOut = File.Create("out.csv"))
        // using (StreamWriter tw = new StreamWriter(fsOut, new UTF8Encoding(false)))
        //     new CsvWriter(tw, new CsvConfiguration(CultureInfo.InvariantCulture)
        //     {
        //         NewLine = "\n",
        //         Delimiter = "\t"
        //     }).WriteRecords(transformed);

        IEnumerable<BlueprintAnalysisPair> candidates = analyzed
            .ProducesOneOf(DspRecipe.PlanetaryLogisticsStation, DspRecipe.InterstellarLogisticsStation)
            .Produces(DspRecipe.IronIngot);

        foreach (BlueprintAnalysisPair analysis in candidates)
        {
            Blueprint original = db.Blueprints.Find(analysis.Blueprint.Id) ?? throw new InvalidOperationException();
            Console.WriteLine(original.Title);
            Console.WriteLine(original.Url);

            Console.WriteLine();
        }
    }

    static void AA()
    {
       // var bp1= BlueprintData.CreateNew(
       //      "BLUEPRINT:0,32,1210,6005,2104,0,0,0,638315249168278514,0.9.27.15466,Warpers%2080x%20Proliferated,\"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\"DF410B297DFE06BE4A49F5D6477A842A");
       // bp1.desc = "desc1";
       // bp1.shortDesc = "desc2";
       // bp1.gameVersion = "v1";
       //
       // var bp2 = bp1.ToBase64String(); 
       //
       //  var fld = typeof(GameConfig).GetField("gameSavePath", BindingFlags.Static | BindingFlags.NonPublic);
       //  fld.SetValue(null, @"C:\Users\MichaelBisbjerg\OneDrive - MBWarez\Documents\Dyson Sphere Program\Save\");
       //
       //  Directory.SetCurrentDirectory(@"C:\Users\MichaelBisbjerg\OneDrive - MBWarez\Documents");
       //  var p2 = Path.GetFullPath(GameConfig.gameSaveFolder);
       //
       //  //"C:\Users\MichaelBisbjerg\OneDrive - MBWarez\Documents\Dyson Sphere Program\Save\Bigly5-uncomp.dsv"
       //  // C:\Users\MichaelBisbjerg\OneDrive - MBWarez\Documents\Dyson Sphere Program\Save\Bigly5-uncomp.dsv
       //  Console.WriteLine(GameConfig.gameSaveFolder);
       //  var p1 = GameSave.LoadGameDesc("Bigly5-uncomp");
       //
       //  GameSave.LoadCurrentGame("Bigly5-uncomp");
    }

    private static IEnumerable<BlueprintAnalysisPair> Load(IEnumerable<Blueprint> source, ILogger logger)
    {
        foreach (Blueprint blueprintRow in source)
        {
            BlueprintData blueprint;
            try
            {
                blueprint = BlueprintData.CreateNew(blueprintRow.BlueprintString);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Unable to load {Id}, error: {Error}", blueprintRow.Id, e.Message);
                continue;
            }

            if (blueprint == null)
            {
                logger.LogWarning("Unable to load {Id}, invalid", blueprintRow.Id);
                continue;
            }

            BlueprintAnalysis analysis = BlueprintAnalyzer.AnalyzeBlueprint(blueprint);
            yield return new BlueprintAnalysisPair(blueprintRow, analysis);
        }
    }
}