---
title: Scripts
---

This section contains Python scripts based on SurgingCloud to provide advanced features.

`surging_cloud_exe` is a global variable of the path to `SurgingCloud.Cli.exe`.

```python
from os.path import isfile

surging_cloud_exe = r'path\to\exe'

if not isfile(surging_cloud_exe):
    print('SurgingCloud.Cli.exe not found.')
    exit(1)
```

## Incrementally encrypt a folder

If we constantly add files into a certain folder (`folder_path`), we may want to only generate the encrypted files of the newly-added ones.

```python title="cliScripts/incr_enc_folder.py"
--8<-- "cliScripts/incr_enc_folder.py:doc"
```

## Recursively encrypt a folder

It is recommended to create a subject for each folder like this script does. The names of the subjects created represent the relative paths of those corresponding subfolders to the top-level folder.

```python title="cliScripts/recursive_enc_folder.py"
--8<-- "cliScripts/recursive_enc_folder.py:doc"
```