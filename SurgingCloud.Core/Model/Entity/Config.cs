namespace SurgingCloud.Core.Model.Entity;

public record Config
{
    /// <summary>
    /// There will be only one record in the config table
    /// </summary>
    public long Id { get; set; } = 1;

    /// <summary>
    /// Path to rar.exe
    /// </summary>
    public string? RarPath { get; set; }
    
    /// <summary>
    /// Values <= 0 means do not check updates
    /// </summary>
    public int CheckUpdateFrequencyInHours { get; set; } = 24;

    /// <summary>
    /// 
    /// </summary>
    public int BackupFrequencyInDays { get; set; } = 7;

    /// <summary>
    /// Backup DB file to another folder
    /// </summary>
    public string? BackupFolderPath { get; set; }
}