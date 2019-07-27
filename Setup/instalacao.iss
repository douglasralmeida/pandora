; Script para o instalador do Pandora
; requer InnoSetup

#define MyAppName "Pandora"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Douglas R. Almeida"
#define MyAppURL "https://github.com/douglasralmeida/pandora"

[Setup]
AppId={{0E15CFDE-8646-4730-A7E4-0B1796757C6C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
AllowNoIcons=yes
ChangesEnvironment=true
Compression=lzma
DefaultDirName={pf}\Pandora
DefaultGroupName=Pandora
DisableWelcomePage=False
MinVersion=0,5.01sp3
OutputBaseFilename=pandorainstala
SetupIconFile=..\img\setup.ico
SolidCompression=yes
ShowLanguageDialog=no
UninstallDisplayName=Pandora
UninstallDisplayIcon={app}\pandora.modelagem.exe
VersionInfoVersion=1.0.0
VersionInfoProductVersion=1.0
UninstallDisplaySize=5000000
OutputDir=saida
DisableReadyPage=True

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Files]
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\Modelagem\bin\Debug\modelagem.exe"; DestDir: "{app}\pandora.modelagem.exe"

[Icons]
Name: "{group}\Modelagem de Processos do Pandora"; Filename: "{app}\pandora.modelagem.exe"; WorkingDir: "{app}"; IconFilename: "{app}\pandora.modelagem.exe"; IconIndex: 0
;Name: "{commondesktop}\Pandora"; Filename: "{app}\pandora.exe"; WorkingDir: "{app}"; IconFilename: "{app}\pandora.exe"; IconIndex: 0