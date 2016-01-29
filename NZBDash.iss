; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=NZBDash
AppVersion=1.0
DefaultDirName={pf}\NZBDash
DefaultGroupName=NZBDash
UninstallDisplayIcon={app}\NZBDash.exe
Compression=lzma2
SolidCompression=yes
OutputDir=Installer
PrivilegesRequired=admin
UninstallRestartComputer=false


[Files]
Source: "NZBDash.UI\bin\*"; DestDir: "{app}\UI\bin"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\Content\*"; DestDir: "{app}\UI\Content"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\fonts\*"; DestDir: "{app}\UI\fonts"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\Scripts\*"; DestDir: "{app}\UI\Scripts"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\Styles\*"; DestDir: "{app}\UI\Styles"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\Views\*"; DestDir: "{app}\UI\Views"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NZBDash.UI\favicon.ico"; DestDir: "{app}\UI"; Flags: ignoreversion  
Source: "NZBDash.UI\NLog.config"; DestDir: "{app}\UI"; Flags: ignoreversion  
Source: "NZBDash.UI\Web.config"; DestDir: "{app}\UI"; Flags: ignoreversion  
Source: "NZBDash.UI\Global.asax"; DestDir: "{app}\UI"; Flags: ignoreversion
Source: "NZBDash.Services.HardwareMonitor\bin\*"; DestDir: "{app}\HardwareMonitor"; Flags: ignoreversion recursesubdirs createallsubdirs

[Run]
Filename: {cmd}; Parameters: "/C ""C:\Windows\System32\inetsrv\appcmd.exe add site /name:NZBDash /bindings:http/*:7500 /physicalPath:{pf}/NZBDash/"""; Flags: runhidden; StatusMsg: "Settings Up Website"
Filename: {cmd}; Parameters: "/C ""{app}\NZBDash.Services.HardwareMonitor\bin\Release\NZBDash.Services.HardwareMonitor.exe install"; Flags: runhidden; StatusMsg: "Settings Up Website"