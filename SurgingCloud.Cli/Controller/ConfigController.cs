using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Service;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli.Controller;

public class ConfigController
{
    private readonly ConfigService _configService;

    public ConfigController(ConfigService configService)
    {
        _configService = configService;
    }

    public void GetConfig()
    {
        var config = _configService.GetConfig();
        var json = JsonUtils.ToStr(config, pretty: true);
        Console.WriteLine(json);
    }

    public void UpdateConfig(ConfigOptions opt)
    {
        var config = _configService.GetConfig();
        if (!string.IsNullOrWhiteSpace(opt.RarPath))
        {
            config.RarPath = opt.RarPath;
        }

        var (_, msg) = _configService.UpdateConfig(config);
        Console.WriteLine(msg);
    }
}