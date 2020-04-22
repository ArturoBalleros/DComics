import sys
from mega import Mega

mega = Mega()

m = mega.login()
m.download_url('https://mega.nz/#!bR1nUBJD!EZynCJ8eFM-i5yD6tIDedbxRR9EV2yo7oYtXoYmmqXI')
m.download_url('https://mega.nz/file/UEs2VYYb#yl7y2arlm5uiB14odydnHRFkeUcQ03WhnBFYF2d2im8')
