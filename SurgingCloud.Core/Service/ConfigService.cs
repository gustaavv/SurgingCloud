using System.Data;
using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Vo;

namespace SurgingCloud.Core.Service;

public class ConfigService
{
    private readonly ConfigDao _configDao;

    public ConfigService(ConfigDao configDao)
    {
        _configDao = configDao;
    }

    public OperationResult<object> ValidateConfig(IDbTransaction? tx = null)
    {
        var config = _configDao.Select(tx: tx);

        if (!File.Exists(config.RarPath))
        {
            return OperationResult<object>.Fail($"Rar path is invalid: {config.RarPath}");
        }

        return OperationResult<object>.Ok("Config validation succeeds");
    }

    public Config GetConfig()
    {
        return _configDao.Select();
    }

    public OperationResult<object> UpdateConfig(Config config)
    {
        var b = _configDao.Update(config) > 0;
        return b ? OperationResult<object>.Ok("Update succeeds") : OperationResult<object>.Fail("Update fails");
    }
}