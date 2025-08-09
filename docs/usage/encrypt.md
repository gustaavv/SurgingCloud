---
title: Encrypt Operation
---

The encryption functionality of SurgingCloud is currently based on `rar.exe` (You can find it in the installation folder of WinRAR). You should install it first.

??? info "**Disclaimer**"

    This project is not affiliated with, endorsed by, or financially supported by WinRAR. The use of WinRAR within this project is solely for its intended functionality, and it is not intended to serve as an advertisement or promotion of the software.


## Encrypt a file/folder

```powershell
.\SurgingCloud.Cli.exe enc --sid 1 --src "D:\Pictures\selfie.png" --out "E:\backup\Pictures" --db "path/to/db"
```

**Arguments**

- `--sid 1`: specify which subject to store the encryption information of this file.
- `--src "D:\Pictures\selfie.png"`: the path to the file/folder you want to encrypt.
- `--out "E:\backup\Pictures"`: the folder to generate the encrypted file.
- (Optional) `--enc-folder`: encrypt the whole folder into a file instead of an empty folder. The default value is `false`.
- (Optional) `--ignore-dup`: ignore encryption if there is an item with the same filename and the same hashBefore in the database. The default value is `false`.

**Output**

```
Encryption succeeds:
Item id = 1
src: D:\Pictures\selfie.png
out: E:\backup\Pictures\6367a30064043fb1f227.rar
```