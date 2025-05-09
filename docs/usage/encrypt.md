---
title: Encrypt Operation
---

The encryption functionality of SurgingCloud is currently based on `rar.exe` (You can find it in the installation folder of WinRAR). You should install it first.

??? info "**Disclaimer**"

    This project is not affiliated with, endorsed by, or financially supported by WinRAR. The use of WinRAR within this project is solely for its intended functionality, and it is not intended to serve as an advertisement or promotion of the software.


## Encrypt a file

```powershell
.\SurgingCloud.Cli.exe enc --db "path/to/db" --sid 0 --byfile --src "src/file" --out "out/path"
```