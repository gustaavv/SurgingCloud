using System.Data;
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

    public (bool b, string msg) ValidateConfig(IDbTransaction? tx = null)
    {
        var config = _configDao.Select(tx: tx);

        if (!File.Exists(config.RarPath))
        {
            return (false, $"Rar path is invalid: {config.RarPath}");
        }

        return (true, "Config validation succeeds");
    }

    public Config GetConfig()
    {
        return _configDao.Select();
    }

    public (bool b, string msg) UpdateConfig(Config config)
    {
        var b = _configDao.Update(config) > 0;
        return (b, b ? "Update succeeds" : "Update fails");
    }
}