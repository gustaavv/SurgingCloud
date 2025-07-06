---
title: Config Operation
---

The configuration of SurgingCloud is also stored in the database file. That's why you should always set `--db "path/to/db"`.

## Get current config

```powershell
.\SurgingCloud.Cli.exe config --get --db "path/to/db"
```

## Update config

```powershell
.\SurgingCloud.Cli.exe config --update --rar "path/to/rar.exe" --db "path/to/db"
```

## Validate config

```powershell
.\SurgingCloud.Cli.exe config --validate --db "path/to/db"
```