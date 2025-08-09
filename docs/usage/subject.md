---
title: Subject Operation
---


## Create a new subject

```powershell
.\SurgingCloud.Cli.exe subject --new --name "subject name" --pwd "subject password" --db "path/to/db"
```

**Arguments**

- `--name "subject name"`: set the subject name. Common names are like `photos` and `videos`. But detailed names are recommended, and you should probably use folder structure names for better management as the items increase. For example, `/photos/travel/2020` and `/videos/drama/1980`.
- `--pwd "subject password"`: set the subject password. This password is what you should remember. The actual password for each encrypted file is made by a hash algorithm (see `--hashAlg`) given the input of this password. So no encrypted file can be cracked by brute force.
- (Optional) `--hashAlg 0`: the hash algorithm for generating the actual password for each encrypted file. `0` (default) means SHA256, `1` means SHA1 and `2` means MD5. You can just omit this argument because SHA256 is the best among them.

**Output**

```
Creation succeeds, new subject id = 1
```

## List all subjects

```powershell
.\SurgingCloud.Cli.exe subject --list --db "path/to/db"
```

**Output**

```
 ------------------------------------------- 
 | id | name   | password | hash algorithm |
 -------------------------------------------
 | 1  | Photos | 123      | Sha256         |
 -------------------------------------------
 | 2  | Videos | hello    | Sha1           |
 -------------------------------------------

 Count: 2
```

## Get subject detail

```powershell
.\SurgingCloud.Cli.exe subject --get --sid 1 --db "path/to/db"
```

**Arguments**

- `--sid 1`: subject id.

**Output**

```json
{
  "Id": 1,
  "Name": "Photos",
  "Password": "123",
  "HashAlg": "Sha256",
  "CreateAt": "2025-06-26T08:55:04",
  "UpdateAt": "2025-06-26T08:55:04",
  "Others": null,
  "ActualPassword": "8f6f505d8504ae01cd877fb9405e6c18d19c5b3bff8aa6c9ff723300866930c6"
}
```

The output is in json format.

## Delete a subject

Note that all the items in the subject will also be deleted.

```powershell
.\SurgingCloud.Cli.exe subject --delete --sid 2 --db "path/to/db"
```

**Arguments**

- `--sid 2`: subject id.

**Output**

```
Delete subject 2 succeeds
```

## List all items in a subject

```powershell
.\SurgingCloud.Cli.exe subject --list-items --sid 1 --db "path/to/db"
```

**Arguments**

- `--sid 1`: subject id.

**Output**

```
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 | id | name before | name after           | item type | hash before                                                      | hash after                                                       |
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 | 1  | selfie.png  | 6367a30064043fb1f227 | File      | 631422a192ad34bb8945034aa7fb21e19fa052128ee86f191c0521f723c17dbd | 61d5b8ce7637106ba6ebaea3f8d02720b7ab82ad7c833d0e45dd0665c226ce8f |
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

 Count: 1
```