using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace DysonSphereBlueprints.Web.Code;

public record BlueprintSample(string Name, string Path, bool UsesGz);

public static class BlueprintsSamples
{
    private static BlueprintSample[] _samples =
    [
        new BlueprintSample("Huge", "Resources/Samples/Huge.txt.gz", true),
        new BlueprintSample("FFF02", "Resources/Samples/FFF02.txt.gz", true),
        new BlueprintSample("Dense01", "Resources/Samples/Dense01.txt.gz", true),
    ];

    public static IEnumerable<BlueprintSample> GetSamples() => _samples;

    public static async Task<string> GetBlueprintSampleContent(HttpClient http, BlueprintSample sample)
    {
        await using Stream res = await http.GetStreamAsync(sample.Path);

        Stream decompStream = res;
        if (sample.UsesGz)
            decompStream = new GZipStream(decompStream, CompressionMode.Decompress);

        await using (decompStream)
        {
            using StreamReader sr = new StreamReader(decompStream);
            return await sr.ReadToEndAsync();
        }
    }
}