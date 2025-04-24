using SurgingCloud.Core.Model.Enum;

namespace SurgingCloud.Core.Model.Entity;

public record Item
{
    /// <summary>
    /// Item ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ID of the subject the item belongs to.
    /// </summary>
    public long SubjectId { get; set; }

    /// <summary>
    /// File/Folder name before encryption.
    /// </summary>
    public string NameBefore { get; set; }

    /// <summary>
    /// File/Folder name after encryption.
    /// </summary>
    public string NameAfter { get; set; }

    /// <summary>
    /// Item type.
    /// </summary>
    public ItemType ItemType { get; set; }

    /// <summary>
    /// File hash result before encryption. The value will be the same as NameBefore for folder.
    /// This field and SubjectId uniquely identify one item.
    /// </summary>
    public string HashBefore { get; set; }

    /// <summary>
    /// File hash result after encryption. Null for folder.
    /// </summary>
    public string? HashAfter { get; set; }

    /// <summary>
    /// File size before encryption. Null for folder.
    /// </summary>
    public long? SizeBefore { get; set; }

    /// <summary>
    /// File size after encryption. Null for folder.
    /// </summary>
    public long? SizeAfter { get; set; }

    /// <summary>
    /// When the item (not the file/folder) was created
    /// </summary>
    public DateTime CreateAt { get; set; }
};