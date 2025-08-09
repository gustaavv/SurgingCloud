---
title: Utility Operation
---

## Generate archive password

```powershell
.\SurgingCloud.Cli.exe util --genpwd --pwd "password" --hashAlg 0 --db "path/to/db"
```

## Hash filename

```powershell
.\SurgingCloud.Cli.exe util --hash-filename --filename "self.png" --hashAlg 0 --db "path/to/db"
```