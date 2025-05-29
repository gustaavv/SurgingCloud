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

```python
import os
import subprocess
from os.path import isdir, join

def incr_enc_folder(folder_path: str, db_path: str, output_path: str, sid: int):
    if not isdir(folder_path):
        print(f'No such folder: {folder_path}')
        return

    for f in os.listdir(folder_path):
        f = join(folder_path, f)
        print('Encrypting', f)
        result = subprocess.run([
            surging_cloud_exe, 'enc',
            '--db', db_path,
            '--sid', str(sid),
            '--byfile',
            '--ignore-dup',
            '--src', f,
            '--out', output_path,
        ], capture_output=True, text=True, shell=True)
        if result.returncode == 0:
            print(result.stdout)
        else:
            print(result.stderr)
        print('-------------------------------------------------')
```