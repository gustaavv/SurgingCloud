using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Enum;
using SurgingCloud.Core.Model.Vo;
using SurgingCloud.Core.Util;

namespace SurgingCloud.Core.Service;

public class EncryptService
{
    private const string ARCHIVE_PASSWORD_PREFIX = "SurgingCloud";

    private const int ARCHIVE_PASSWORD_LENGTH = 64;

    private readonly ItemDao _itemDao;

    private readonly SubjectDao _subjectDao;

    private readonly HashService _hashService;

    private readonly ConfigService _configService;

    public EncryptService(ItemDao itemDao, SubjectDao subjectDao, HashService hashService, ConfigService configService)
    {
        _itemDao = itemDao;
        _subjectDao = subjectDao;
        _hashService = hashService;
        _configService = configService;

        ArchiveUtils.RarPath = _configService.GetConfig().RarPath;
    }

    public async Task<string> CreateArchivePassword(Subject subject)
    {
        var pwd = ARCHIVE_PASSWORD_PREFIX + subject.Password;
        pwd = await HashUtils.ComputeStringHash(pwd, subject.HashAlg);
        return pwd.Substring(0, Math.Min(pwd.Length, ARCHIVE_PASSWORD_LENGTH));
    }

    /// <summary>
    /// Create an item, generate encrypted file/folder and insert the item into database.
    /// </summary>
    /// <param name="ignoreIfDuplicateInDb">useful for incremental encryption in a same subject</param>
    /// <param name="encWholeFolder"></param>
    public async Task<OperationResult<long>> EncryptItem(string srcPath, long subjectId, string encOutputPath,
        bool ignoreIfDuplicateInDb, bool encWholeFolder)
    {
        if (!(File.Exists(srcPath) || Directory.Exists(srcPath)))
        {
            return OperationResult<long>.Fail($"file/folder does not exist: {srcPath}");
        }

        if (!Directory.Exists(encOutputPath))
        {
            Directory.CreateDirectory(encOutputPath);
        }

        // It is fine whether _subjectDao or _itemDao calls CreateConnection(), because they point to the
        // same database file.
        using (var conn = _subjectDao.CreateConnection())
        {
            conn.Open();
            using (var tx = conn.BeginTransaction())
            {
                string? targetPath = null;
                var archiveMade = false;
                try
                {
                    var validationResult = _configService.ValidateConfig(tx: tx);
                    if (!validationResult.Success)
                    {
                        throw new Exception(validationResult.Message);
                    }

                    var subject = _subjectDao.SelectById(subjectId, tx: tx);
                    if (subject == null)
                    {
                        throw new Exception($"Subject not found, id = {subjectId}");
                    }

                    var itemType = (ItemType)ItemTypeUtils.GetItemType(srcPath)!;
                    var nameBefore = FsUtils.GetLastEntry(srcPath);
                    var hashBefore = itemType == ItemType.File
                        ? await HashUtils.ComputeFileHash(srcPath, subject.HashAlg)
                        : nameBefore;
                    var nameAfter = await _hashService.HashFilename(nameBefore, subject.HashAlg);

                    var itemWithSameHashBefore = _itemDao.SelectByHashBefore(subject.Id, hashBefore, tx: tx);

                    if (itemWithSameHashBefore != null && ignoreIfDuplicateInDb)
                    {
                        tx.Commit();
                        return OperationResult<long>.Ok(
                            $"Item with same hashBefore (id = {itemWithSameHashBefore.Id}) already exists in database. No encrypted file generated.",
                            itemWithSameHashBefore.Id);
                    }

                    string? hashAfter = null;
                    long? sizeAfter = null;
                    if (itemType == ItemType.File || encWholeFolder)
                    {
                        targetPath = Path.Join(encOutputPath, $"{nameAfter}.rar");
                        var result = await ArchiveUtils.CompressRar(
                            new List<string> { srcPath },
                            targetPath,
                            await CreateArchivePassword(subject)
                        );
                        if (result != 0)
                        {
                            throw new Exception($"Compressing rar archive fails with exit code {result}");
                        }
                        archiveMade = true;

                        hashAfter = await HashUtils.ComputeFileHash(targetPath, subject.HashAlg);
                        sizeAfter = new FileInfo(targetPath).Length;
                    }
                    else if (itemType == ItemType.Folder)
                    {
                        targetPath = Path.Join(encOutputPath, nameAfter);
                        Directory.CreateDirectory(targetPath);
                    }
                    else
                    {
                        throw new Exception($"Unknown item type {itemType}");
                    }

                    var item = new Item
                    {
                        SubjectId = subject.Id,
                        NameBefore = nameBefore,
                        NameAfter = nameAfter,
                        ItemType = itemType,
                        HashBefore = hashBefore,
                        HashAfter = hashAfter,
                        SizeBefore = itemType == ItemType.File ? new FileInfo(srcPath).Length : null,
                        SizeAfter = sizeAfter,
                    };

                    var encDigest = $"src: {Path.GetFullPath(srcPath)}\nout: {Path.GetFullPath(targetPath)}";

                    if (itemWithSameHashBefore != null)
                    {
                        tx.Commit();
                        return OperationResult<long>.Ok(
                            $"Encryption succeeds, but item (id = {itemWithSameHashBefore.Id}) already exists, so no update to database.\n{encDigest}",
                            itemWithSameHashBefore.Id);
                    }

                    var b = _itemDao.Insert(item, tx: tx) > 0;
                    if (!b)
                    {
                        throw new Exception("Insertion into database failed");
                    }

                    item = _itemDao.SelectByHashBefore(subject.Id, hashBefore, tx: tx)!;

                    tx.Commit();
                    return OperationResult<long>.Ok($"Encryption succeeds:\nItem id = {item.Id}\n{encDigest}", item.Id);
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    if (archiveMade && File.Exists(targetPath))
                    {
                        File.Delete(targetPath);
                    }

                    return OperationResult<long>.Fail($"Encryption fails: {ex.Message}");
                }
            }
        }
    }
}