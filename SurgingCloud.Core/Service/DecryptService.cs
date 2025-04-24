using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Service;

public class DecryptService
{
    private readonly ItemDao _itemDao;

    public DecryptService(ItemDao itemDao)
    {
        _itemDao = itemDao;
    }

    public string DecryptFilepath(string encFilePath, Subject subject)
    {
        var list = encFilePath.Split('/').Select(s =>
        {
            s = s.Trim();
            var item = _itemDao.SelectByNameAfter(subject.Id, s);
            return item == null ? $"<{s}>" : item.NameBefore;
        }).ToList();
        return string.Join('/', list);
    }
}