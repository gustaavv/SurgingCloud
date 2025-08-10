---
title: Utility Operation
---

## Generate archive password

Generate the actual password for an encrypted file, namely, an archive.

```powershell
.\SurgingCloud.Cli.exe util --genpwd --pwd "hello" --db "path/to/db"
```

**Arguments**

- `--pwd "hello"`: the subject password, which is the base password for generating the actual password. This subject is just a conceptual one, which does not necessarily exist.
- (Optional) `--hashAlg 0`: the hash algorithm for generating the actual password. `0` (default) means SHA256, `1` means SHA1 and `2` means MD5.

**Output**

```
Subject password: hello
Hash algorithm: Sha256
Generated archive password: ef1a7adb182609c7c59170bdc7719516afb6514e539c1bbed5968d2fa5078258
```

## Hash filename

This is the same algorithm to generate the filename of an encrypted file. 

```powershell
.\SurgingCloud.Cli.exe util --hash-filename --filename "selfie.png" --db "path/to/db"
```

**Arguments**

- `--filename "selfie.png"`: the filename to be hashed.
- (Optional) `--hashAlg 0`: the hash algorithm for generating the actual password. `0` (default) means SHA256, `1` means SHA1 and `2` means MD5.

**Output**

```
Filename: selfie.png
Hash algorithm: Sha256
Hashed filename: 6367a30064043fb1f227
```