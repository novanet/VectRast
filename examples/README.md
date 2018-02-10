## Example args

vectrast.exe -loadbmp inbmp.png -scale 1 1 -savelev outlev.lev >> log.txt

vectrast.exe -savebmp outbmp.png -loadlev inlev.lev -rotate 5 -scale 2209 2209 >> log.txt

vectrast.exe -loadlev inlev.lev -rotate 5 -scale -47 47 -savelev outlev.lev >> log.txt

vectrast.exe -loadlevbmp inlev.lev include.png -scale 2 2 -translate -5 -5 -savelev outlev.lev >> log.txt
