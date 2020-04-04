#!/usr/bin/python

import sys
from mega import Mega

def downloadFileMega(link):
    # instancio API
    mega = Mega()

    # login anonimo
    m = mega.login()
    
    # download file from url
    try:
        m.download_url(link)
    except PermissionError: 
        return 'true'
    except ValueError as err: 
        return err
    except:
        return 'false'

    return 'true'
    
#pip install mega.py
#python -m pip install --upgrade pip
if __name__ == '__main__':
    link = str(sys.argv[1])
    print(downloadFileMega(link))