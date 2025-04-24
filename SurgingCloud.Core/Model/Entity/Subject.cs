using SurgingCloud.Core.Model.Enum;

namespace SurgingCloud.Core.Model.Entity;

public record Subject
{
    /// <summary>
    /// Subject ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Subject name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Subject password. Stored in plan text
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Hash algorithm for all the items of the subject.
    /// </summary>
    public HashAlg HashAlg { get; set; } = HashAlg.Sha256;

    /// <summary>
    /// When the subject was created
    /// </summary>
    public DateTime CreateAt { get; set; }

    /// <summary>
    /// When the subject was last updated
    /// </summary>
    public DateTime UpdateAt { get; set; }
}