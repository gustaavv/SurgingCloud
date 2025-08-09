---
title: Item Operation
---

## Get Item detail

```powershell
.\SurgingCloud.Cli.exe item --get --iid 1 --db "path/to/db"
```

**Arguments**

- `--iid 1`: item id.

**Output**

```json
{
  "Id": 1,
  "SubjectId": 1,
  "NameBefore": "selfie.png",
  "NameAfter": "6367a30064043fb1f227",
  "ItemType": "File",
  "HashBefore": "5ccbd62474fc77ca19d4c834e52da036fc54fc11b4bb902d2a92162432baf0ba",
  "HashAfter": "8437d8c1720a03ea15253cfb58b663006efba51f7b15bffa4ba3c6b54af9fc29",
  "SizeBefore": 77472,
  "SizeAfter": 89944,
  "CreateAt": "2025-08-09T08:13:38",
  "Others": null
}
```

The output is in json format.