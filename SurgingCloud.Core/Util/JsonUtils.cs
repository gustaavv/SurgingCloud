using System.Text.Json;
using System.Text.Json.Serialization;

namespace SurgingCloud.Core.Util;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = false,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    // same to Options except WriteIndented = true
    private static readonly JsonSerializerOptions PrettyPrintOptions = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static string ToStr(object obj, bool pretty = false)
    {
        return JsonSerializer.Serialize(obj, pretty ? PrettyPrintOptions : Options);
    }

    public static T? ToObj<T>(string s)
    {
        return JsonSerializer.Deserialize<T>(s, Options);
    }


    public static async Task<T?> Read<T>(string file)
    {
        await using var stream = File.OpenRead(file);
        return await JsonSerializer.DeserializeAsync<T>(stream, options: Options);
    }

    public static async Task Write(string file, object obj, bool force = false, bool pretty = false)
    {
        if (File.Exists(file) && !force)
        {
            throw new Exception($"file already exists and force is set to false: {file}");
        }

        await using var stream = File.Create(file);
        await JsonSerializer.SerializeAsync(stream, obj, pretty ? PrettyPrintOptions : Options);
    }
}