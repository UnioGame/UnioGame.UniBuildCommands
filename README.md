# How To Install


Add to your project manifiest by path [%UnityProject%]/Packages/manifiest.json new Scope:

```json
{
  "dependencies": {
      "com.unigame.buildpipeline.commands" : "https://github.com/UnioGame/UnioGame.UniBuildCommands.git",
      "com.littlebigfun.addressable-importer": "https://github.com/UnioGame/unity-addressable-importer.git",
      "com.unigame.unityspreadsheets" : "https://github.com/UnioGame/UniGame.GoogleSpreadsheetsImporter.git",
      "com.unigame.coremodules": "https://github.com/UnioGame/UniGame.CoreModules.git",
      "com.unigame.fluentftp": "https://github.com/UnioGame/FuentFTP.git",
      "com.unigame.unibuildpipeline" : "https://github.com/UnioGame/UniGame.UniBuild.git"
  }
}
```

# UniBuild.Commands
Additional Commands for commnads based build pipeline for Unity

Contents:

- Addressables Commands
- Distribution Commands
- WebRequest Commands
- FTP Commands
- AddressablesImporter Commands
- Android Commands



## Android Commands

### Android Debug Symbols Rezip 

Rezip your debug symbols into smaller one with only selected android architectures

- Fix This issue
https://forum.unity.com/threads/making-android-symbols-package-smaller.1109600/

![](https://github.com/UniGameTeam/UniBuild.Commands/blob/master/Editor/GitAssets/rezip_command.png)
