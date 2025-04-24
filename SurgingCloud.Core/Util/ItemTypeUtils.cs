using SurgingCloud.Core.Model.Enum;

namespace SurgingCloud.Core.Util;

public static class ItemTypeUtils
{
    public static ItemType? GetItemType(string filepath)
    {
        if (File.Exists(filepath))
        {
            return ItemType.File;
        }

        if (Directory.Exists(filepath))
        {
            return ItemType.Folder;
        }

        return null;
    }
}