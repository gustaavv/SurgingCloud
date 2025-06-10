from os.path import isfile

surging_cloud_exe = r'.\SurgingCloud.Cli\bin\Debug\net6.0\SurgingCloud.Cli.exe'

if not isfile(surging_cloud_exe):
    print('SurgingCloud.Cli.exe not found.')
    exit(1)

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


if __name__ == '__main__':
    # incr_enc_folder(r'.\test\src\', r'test1.db', r'.\test\out\', 3)
    pass
