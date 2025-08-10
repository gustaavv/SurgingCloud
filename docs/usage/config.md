---
title: Config Operation
---

The configuration of SurgingCloud is also stored in the database file. That's why you should always set `--db "path/to/db"`.

## Get current config

```powershell
.\SurgingCloud.Cli.exe config --get --db "path/to/db"
```

**Output**

```json
{
  "Id": 1,
  "RarPath": "D:\\WinRAR\\Rar.exe",
  "CheckUpdateFrequencyInHours": 24,
  "LastCheckUpdateAt": "2025-08-09T07:54:34",
  "Others": null
}
```

The output is in json format.

Currently, only `RarPath` field is in use. You can ignore other fields.


## Update config

```powershell
.\SurgingCloud.Cli.exe config --update --rar "D:\WinRAR\Rar.exe" --db "path/to/db"
```

**Arguments**

- `--rar "D:\WinRAR\Rar.exe"`: set the path to `Rar.exe` installed on your computer.

**Output**

```
Update succeeds
```

## Validate config

```powershell
.\SurgingCloud.Cli.exe config --validate --db "path/to/db"
```

**Output**

```
Config validation succeeds
```