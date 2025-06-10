from os.path import isfile

surging_cloud_exe = r'.\SurgingCloud.Cli\bin\Debug\net6.0\SurgingCloud.Cli.exe'

if not isfile(surging_cloud_exe):
    print('SurgingCloud.Cli.exe not found.')
    exit(1)
# --8<-- [start:doc]
import os
from os.path import isdir, isfile, join
import subprocess
import json


def get_subject(sid: int, db_path: str):
    result = subprocess.run([
        surging_cloud_exe, 'subject', '--db', db_path, '--out-json',
        '--get', '--sid', str(sid),
    ], capture_output=True, text=True, shell=True)
    subject = json.loads(result.stdout)
    return subject if subject['Id'] else None


def create_subject(db_path: str, name: str, pwd: str):
    result = subprocess.run([
        surging_cloud_exe, 'subject', '--db', db_path, '--out-json',
        '--new', '--name', name, '--pwd', pwd,
    ], capture_output=True, text=True, shell=True)
    # No matter a subject with the same name exist or not,
    # we will get its id after calling create
    result = json.loads(result.stdout)
    sid = result['Data']
    if not sid:
        print(result['Message'])
    return sid


def get_item(iid: int, db_path: str):
    result = subprocess.run([
        surging_cloud_exe, 'item', '--db', db_path, '--out-json',
        '--get', '--iid', str(iid),
    ], capture_output=True, text=True, shell=True)
    item = json.loads(result.stdout)
    return item if item['Id'] else None


def recursive_enc_folder(folder_path: str, db_path: str, output_path: str, sid: int):
    folder_path = folder_path.replace('\\', '/')
    print('Encrypting', folder_path)
    if not isdir(folder_path):
        print('Folder not found:', folder_path)
        return

    parent_subject = get_subject(sid, db_path)
    if not parent_subject:
        print('Parent subject not found, sid =', sid)
        return
    parent_name = parent_subject['Name']
    pwd = parent_subject['Password']  # every subject will have the same password

    for f in os.listdir(folder_path):
        f_abspath = join(folder_path, f)
        if isfile(f_abspath):
            result = subprocess.run([
                surging_cloud_exe, 'enc', '--db', db_path, '--out-json', '--sid', str(sid),
                '--byfile', '--src', f_abspath, '--out', output_path, '--ignore-dup',
            ], capture_output=True, text=True, shell=True)
            # TODO: print something with the result if you wish
        elif isdir(f_abspath):
            new_subject_name = parent_name + '/' + f
            new_sid = create_subject(db_path, new_subject_name, pwd)
            if not new_sid:
                return
            result = subprocess.run([
                surging_cloud_exe, 'enc', '--db', db_path, '--out-json', '--sid', str(sid),
                '--byfile', '--src', f_abspath, '--out', output_path,
            ], capture_output=True, text=True, shell=True)
            result = json.loads(result.stdout)
            # TODO: print something with the result if you wish
            iid = result['Data']
            if not iid:
                print(result['Message'])
                return
            item = get_item(iid, db_path)
            recursive_enc_folder(f_abspath, db_path, join(output_path, item['NameAfter']), new_sid)
# --8<-- [end:doc]

if __name__ == '__main__':
    # recursive_enc_folder(r'.\test\src', r'test1.db', r'.\test\out', 2)
    pass
