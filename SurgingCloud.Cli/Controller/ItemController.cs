using SurgingCloud.Cli.CommandLineOptions;
using SurgingCloud.Core.Model.Vo;
using SurgingCloud.Core.Service;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Cli.Controller;

public class ItemController
{
    private readonly ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    public void GetItem(ItemOptions opt)
    {
        var id = opt.ItemId;
        var item = _itemService.SelectById(id);
        if (item != null)
        {
            Console.WriteLine(JsonUtils.ToStr(item, pretty: true));
        }
        else
        {
            opt.Cw(OperationResult<object>.Fail("No item found"));
        }
    }
}