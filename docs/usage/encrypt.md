---
title: Encrypt Operation
---

The encryption functionality of SurgingCloud is currently based on `rar.exe` (You can find it in the installation folder of WinRAR). You should install it first.

??? info "**Disclaimer**"

    This project is not affiliated with, endorsed by, or financially supported by WinRAR. The use of WinRAR within this project is solely for its intended functionality, and it is not intended to serve as an advertisement or promotion of the software.


## Encrypt a file

```powershell
.\SurgingCloud.Cli.exe enc --sid 0 --byfile --src "src/file" --out "out/path" --db "path/to/db"
```

Arguments:

- `--sid 0`: specify which subject to store the encryption information of this file.
- `--byfile`: encrypt a single file (later SurgingCloud will have the ability to encrypt a folder and all the children in it).
- `--src "src/file"`: the path to the file you want to encrypt.
- `--out "out/path"`: the folder to generate the encrypted file.