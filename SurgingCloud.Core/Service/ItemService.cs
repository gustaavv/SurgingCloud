using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Core.Service;

public class ItemService
{
    private readonly ItemDao _itemDao;

    private readonly SubjectDao _subjectDao;

    private readonly HashService _hashService;

    private readonly EncryptService _encryptService;

    public ItemService(ItemDao itemDao, SubjectDao subjectDao, HashService hashService, EncryptService encryptService)
    {
        _itemDao = itemDao;
        _subjectDao = subjectDao;
        _hashService = hashService;
        _encryptService = encryptService;
    }

    /// <summary>
    /// Create an item, generate encrypted file and insert the item into database.
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="subjectId"></param>
    /// <param name="encOutputPath"></param>
    /// <returns></returns>
    public async Task<(bool, string)> CreateItem(string filepath, long subjectId, string encOutputPath)
    {
        if (!(File.Exists(filepath) || Directory.Exists(filepath)))
        {
            return (false, $"file/folder does not exist: {filepath}");
        }

        var subject = _subjectDao.SelectById(subjectId);
        if (subject == null)
        {
            return (false, $"Subject not found, id = {subjectId}");
        }

        var itemType = (ItemType)ItemTypeUtils.GetItemType(filepath)!;
        var nameBefore = FsUtils.GetLastEntry(filepath);
        var hashBefore = itemType == ItemType.File
            ? await HashUtils.ComputeFileHash(filepath, subject.HashAlg)
            : nameBefore;

        if (_itemDao.SelectByHashBefore(subjectId, hashBefore) != null)
        {
            return (false, "Item already exists, subject id = {subjectId}, hash = {hashBefore}");
        }

        var nameAfter = await _hashService.HashFilename(nameBefore, subject.HashAlg);

        string? hashAfter = null;
        long? sizeAfter = null;
        if (itemType == ItemType.File)
        {
            var targetPath = Path.Join(encOutputPath, $"{nameAfter}.rar");
            var result = await ArchiveUtils.CompressRar(
                new List<string> { filepath },
                targetPath,
                await _encryptService.CreateArchivePassword(subject)
            );
            if (result != 0)
            {
                return (false, $"Compress Rar failed with exit code {result}");
            }

            hashAfter = await HashUtils.ComputeFileHash(targetPath, subject.HashAlg);
            sizeAfter = new FileInfo(targetPath).Length;
        }

        var item = new Item
        {
            SubjectId = subjectId,
            NameBefore = nameBefore,
            NameAfter = nameAfter,
            ItemType = itemType,
            HashBefore = hashBefore,
            HashAfter = hashAfter,
            SizeBefore = itemType == ItemType.File ? new FileInfo(filepath).Length : null,
            SizeAfter = sizeAfter,
        };
        var b = _itemDao.Insert(item) > 0;
        return (b, b ? "" : "Insertion into database failed");
    }
}