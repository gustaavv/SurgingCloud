---
title: Hello SurgingCloud
---

This tutorial will walk you through the basic steps to use SurgingCloud.

## Step 1. Initialization

After installation, the first thing to do is to initialize. Execute the following command:

```powershell
.\SurgingCloud.Cli.exe config --db "data.db" --update --rar "D:\WinRAR\Rar.exe"
```

The output will be like:

```
Create a new database file at data.db
Update succeeds
```

The command does two things:

- Create a database file at `data.db`. You should put the database in a safe location because it stores all the encryption information. You should also do regular backups.
- Config SurgingCloud to let it know the path to `rar.exe` installed on your computer.



## Step 2. Create a subject

A subject is like a folder, containing all the encryption information of files stored in it. Let's create one subject called `Photos` with password `123`:

```powershell
.\SurgingCloud.Cli.exe subject --db "data.db" --new --name "Photos" --pwd "123"
```

The output will be like:
```
Using existing database file at data.db
Creation succeeds, new subject id = 1
```

Each subject is uniquely identified by its id. Take note of this id. We will need it in the following steps.


## Step 3. Encrypt a file

SurgingCloud encrypts a file by storing its relevant information into a user-specified subject and generating the encrypted file.

Let's encrypt a file `D:\Pictures\selfie.png` using the subject with id `1`, i.e. the one we just created, and make the encrypted file generated in folder `E:\backup\Pictures`: 

```powershell
.\SurgingCloud.Cli.exe enc --db "data.db" --sid 1 --byfile --src "D:\Pictures\selfie.png" --out "E:\backup\Pictures"
```

The output will be like:
```
Using existing database file at data.db
Encryption succeeds:
Item id = 1
src: D:\Pictures\selfie.png
out: E:\backup\Pictures\6367a30064043fb1f227.rar
```

Each item is uniquely identified by its id.

The encrypted file is at `E:\backup\Pictures\6367a30064043fb1f227.rar`. 

## Step 4. Upload the encrypted file

Well done! You have successfully encrypted a file. Now feel free to upload the encrypted file to (various) cloud drives.