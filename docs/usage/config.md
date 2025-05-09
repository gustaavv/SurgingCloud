---
title: Config Operation
---

The configuration of SurgingCloud is also stored in the database file. That's why you should always set `--db "path/to/db"`.

## Get current config

```powershell
.\SurgingCloud.Cli.exe config --db "path/to/db" --get
```

## Update config

```powershell
.\SurgingCloud.Cli.exe config --db "path/to/db" --update --rar "path/to/rar.exe"
```