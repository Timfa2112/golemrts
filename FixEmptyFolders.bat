@ECHO off
echo "Marking empty directories so Git won't complain..."
util\MarkEmptyDirs.exe -v Source/
util\MarkEmptyDirs.exe -v Assets/
ping 1.1.1.1 -n 1 -w 3000 > nul