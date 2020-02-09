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
        print("Oops!  That was no valid number.  Try again...")
    except:
        return 'false'

    return 'true'
    
#pip install mega.py
#python -m pip install --upgrade pip
#https://stackoverflow.com/questions/42012140/if-name-main-function-call/42013148
if __name__ == '__main__':
    print(downloadFileMega(link))