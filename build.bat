   @echo off
   set gameDirectoryPlugins="C:\Program Files (x86)\Steam\steamapps\common\worldbox\BepInEx\plugins"
   set gameDirectoryPatchers="C:\Program Files (x86)\Steam\steamapps\common\worldbox\BepInEx\patchers"
   set dllWarLogger_Helper="WarLogger_Helper.dll"
   set pdbWarLogger_Helper="WarLogger_Helper.pdb"
   set dllWarLogger_BepInEx_Preloader="WarLogger_BepInEx_Preloader.dll"
   set pdbWarLogger_BepInEx_Preloader="WarLogger_BepInEx_Preloader.pdb"
   set dllWarLogger_BepInEx="WarLogger_BepInEx.dll"
   set pdbWarLogger_BepInEx="WarLogger_BepInEx.pdb"

   if exist %gameDirectoryPlugins% (ECHO "gameDirectoryPlugins directory found") ELSE (ECHO "gameDirectoryPlugins directory not found")
   if exist %dllWarLogger_Helper% (ECHO "dllWarLogger_Helper DLL found") ELSE (ECHO "dllWarLogger_Helper DLL not found")
   if exist %gameDirectoryPlugins% copy /y %dllWarLogger_Helper% %gameDirectoryPlugins%
   if exist %pdbWarLogger_Helper% (ECHO "pdbWarLogger_Helper PDB found") ELSE (ECHO "pdbWarLogger_Helper PDB not found")
   if exist %gameDirectoryPlugins% copy /y %pdbWarLogger_Helper% %gameDirectoryPlugins%

   if exist %gameDirectoryPatchers% (ECHO "gameDirectoryPatchers directory found") ELSE (ECHO "gameDirectoryPatchers directory not found")
   if exist %dllWarLogger_BepInEx_Preloader% (ECHO "dllWarLogger_BepInEx_Preloader DLL found") ELSE (ECHO "dllWarLogger_BepInEx_Preloader DLL not found")
   if exist %gameDirectoryPatchers% copy /y %dllWarLogger_BepInEx_Preloader% %gameDirectoryPatchers%
   if exist %pdbWarLogger_BepInEx_Preloader% (ECHO "pdbWarLogger_BepInEx_Preloader PDB found") ELSE (ECHO "pdbWarLogger_BepInEx_Preloader PDB not found")
   if exist %gameDirectoryPatchers% copy /y %pdbWarLogger_BepInEx_Preloader% %gameDirectoryPatchers%

   if exist %gameDirectoryPlugins% (ECHO "gameDirectoryPlugins directory found") ELSE (ECHO "gameDirectoryPlugins directory not found")
   if exist %dllWarLogger_BepInEx% (ECHO "dllWarLogger_BepInEx DLL found") ELSE (ECHO "dllWarLogger_BepInEx DLL not found")
   if exist %gameDirectoryPlugins% copy /y %dllWarLogger_BepInEx% %gameDirectoryPlugins%
   if exist %pdbWarLogger_BepInEx% (ECHO "pdbWarLogger_BepInEx PDB found") ELSE (ECHO "pdbWarLogger_BepInEx PDB not found")
   if exist %gameDirectoryPlugins% copy /y %pdbWarLogger_BepInEx% %gameDirectoryPlugins%

   start steam://rungameid/1206560