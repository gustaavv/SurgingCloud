using System.Data;
using SurgingCloud.Core.Dao;
using SurgingCloud.Core.Model.Entity;
using SurgingCloud.Core.Model.Enum;
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
    public async Task<(bool b, string msg)> EncryptItem(string srcPath, long subjectId, string encOutputPath,
        bool ignoreIfDuplicateInDb)
    {
        if (!(File.Exists(srcPath) || Directory.Exists(srcPath)))
        {
            return (false, $"file/folder does not exist: {srcPath}");
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
                try
                {
                    var validationResult = _configService.ValidateConfig(tx: tx);
                    if (!validationResult.b)
                    {
                        throw new Exception(validationResult.msg);
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
                        return (true,
                            "Item with same hashBefore already exists in database. No encrypted file generated.");
                    }

                    string? hashAfter = null;
                    long? sizeAfter = null;
                    string targetPath;
                    if (itemType == ItemType.File)
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
                        return (true,
                            $"Encryption succeeds, but item already exists, so no update to database.\n{encDigest}");
                    }

                    var b = _itemDao.Insert(item, tx: tx) > 0;
                    if (!b)
                    {
                        throw new Exception("Insertion into database failed");
                    }

                    tx.Commit();
                    return (true, $"Encryption succeeds:\n{encDigest}");
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    return (false, "Encryption fails: " + ex.Message);
                }
            }
        }
    }
}