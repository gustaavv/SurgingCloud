using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Service;

public class ConfigService
{
    private readonly ConfigDao _configDao;

    public ConfigService(ConfigDao configDao)
    {
        _configDao = configDao;
    }

    public (bool, string) ValidateConfig()
    {
        var config = _configDao.Select();

        if (!File.Exists(config.RarPath))
        {
            return (false, $"Rar path is invalid: {config.RarPath}");
        }

        return (true, "");
    }

    public int UpdateConfig(Config config)
    {
        return _configDao.Update(config);
    }
}