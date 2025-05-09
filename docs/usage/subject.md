---
title: Subject Operation
---


## Create new subject

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --new --name "subject name" --pwd "subject password" --hashAlg 0
```

## List all subjects

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --list
```

## Get subject detail

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --get --sid 0
```