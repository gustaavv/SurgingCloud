---
title: Hello World
---

This tutorial will walk you through the basic steps to use SurgingCloud.

## Step 1. Initialization

The command below does two things:

- Create a database file. Put it in a safe location as all the encryption information is stored in it. You can do backups on a regular basis.
- Config SurgingCloud to let it know the path to `rar.exe` installed on your computer.

```powershell
.\SurgingCloud.Cli.exe config --db "path/to/db" --update --rar "path/to/rar.exe"
```

## Step 2. Create a subject

A subject is like a folder, containing all the encryption information of files stored in it. Given a file on your filesystem, SurgingCloud encrypts it as well as generate an item into the subject. Both the subjects and items are stored in the database specified by the `--db "path/to/db"` argument.

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --new --name "subject name" --pwd "subject password" --hashAlg 0
```

- `--name "subject name"`: set the subject name. Common names are like `photos`, `videos`. But detailed names are recommended and you should probably use folder structure names for better management as the items increase. For example, `/photos/travel/2020` and `/videos/drama/1980`.
- `--pwd "subject password"`: set the subject password. This password is what you should remember. The actual password for each encrypted file is made by a hash algorithm (see `--hashAlg`) given the input of this password. So no encrypted file can be cracked by brute force.
- `--hashAlg 0`: the hash algorithm for generating the actual password for each encrypted file. `0` (default) means SHA256, `1` means SHA1 and `2` means MD5. You can just omit this argument because SHA256 is the best among them.

Let's have a look at the subject we just created.

```powershell
.\SurgingCloud.Cli.exe subject --db "path/to/db" --list
```

Remember the id of the subject. We will need it in the next step.


## Step 3. Encrypt a file

```powershell
.\SurgingCloud.Cli.exe enc --db "path/to/db" --sid 0 --byfile --src "src/file" --out "out/path"
```

- `--sid 0`: specify which subject to store the encryption information of this file.
- `--byfile`: encrypt a single file (later SurgingCloud will have the ability to encrypt a folder and all the children in it).
- `--src "src/file"`: the path to the file you want to encrypt.
- `--out "out/path"`: the folder to generate the encrypted file.

## Step 4. Upload the encrypted file

Well done! You have successfully encrypted a file. Now, feel free to upload the encrypted file to cloud drives.