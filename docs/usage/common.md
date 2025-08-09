---
title: Common Operation
---

This section talks about some common things that the operations above share, e.g., common arguments and common output.

## Database path verbose

Every command will begin with one of the following outputs. They serve as hints on which database we are currently using.

```
Create a new database file at /path/to/db
```

```
Using existing database file at /path/to/db
```

This verbose is disabled if `--out-json` is set.

## Output in json format

Every command can have an argument `--out-json`. When it is enabled, the output will be in json format so that the scripts using SurgingCloud can parse the output easier.

Here is the output of encrypting a file:

`--out-json` disabled (default):

```
Using existing database file at data.db
Encryption succeeds:
Item id = 1
src: D:\Pictures\selfie.png
out: E:\backup\Pictures\6367a30064043fb1f227.rar
```

`--out-json` enabled:

```json
{
  "Success": true,
  "Message": "Encryption succeeds:\nItem id = 1\nsrc: D:\\Pictures\\selfie.png\nout: E:\\backup\\Pictures\\6367a30064043fb1f227.rar",
  "Data": 1
}
```

- `Success`: a boolean value, indicating whether the operation succeeds.
- `Message`: a string value, whose value is normally the same as the value when `--out-json` is disabled except for the database path verbose.
- `Data`: value of any type, whose value may be useful to the scripts and is dependent on the operation. This field is `null` if no such data is provided. 

## Backup database

The argument `--bkp-db` allows you to back up the database file before doing any operation. It will duplicate the database file (suppose it is `data.db`) in the same folder, and rename it to `data.db.20250704060547283.bkp`. `20250704060547283` is the timestamp now, whose precision is millisecond.

This argument will add the following output if `--out-json` is disabled:

```
Backup database file at E:\bkp\data.db.20250704060547283.bkp
```