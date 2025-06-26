---
title: Subject Operation
---


## Create a new subject

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --new --name "subject name" --pwd "subject password" --hashAlg 0
```

Arguments:

- `--name "subject name"`: set the subject name. Common names are like `photos` and `videos`. But detailed names are recommended, and you should probably use folder structure names for better management as the items increase. For example, `/photos/travel/2020` and `/videos/drama/1980`.
- `--pwd "subject password"`: set the subject password. This password is what you should remember. The actual password for each encrypted file is made by a hash algorithm (see `--hashAlg`) given the input of this password. So no encrypted file can be cracked by brute force.
- `--hashAlg 0`: the hash algorithm for generating the actual password for each encrypted file. `0` (default) means SHA256, `1` means SHA1 and `2` means MD5. You can just omit this argument because SHA256 is the best among them.

## List all subjects

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --list
```

## Get subject detail

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --get --sid 0
```

## Delete a subject

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --delete --sid 0
```

## List all items in a subject

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --list-items --sid 0
```