using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;

namespace SurgingCloud.Core.Service;

public class ItemService
{
    private readonly ItemDao _itemDao;

    public ItemService(ItemDao itemDao)
    {
        _itemDao = itemDao;
    }

    public Item? SelectById(long id)
    {
        return _itemDao.SelectById(id);
    }
}