# com.agava.yandexgames  
  
[Try the SDK demo here.](https://yandex.ru/games/app/223976?draft=true)  
  
Key difference: It's not vandalizing your project unlike other popular plugins. This SDK assumes you're publishing your project to multiple platforms (not only Yandex Games) while using their respective SDKs.  
  
Make sure you have the standalone [Git](https://git-scm.com/downloads) installed. Reboot after installation.  
In Unity, open "Window" -> "Package Manager".  
Click the "+" sign at the top left corner -> "Add package from git URL..."  
Paste this: `https://github.com/forcepusher/com.agava.yandexgames.git#16.1.1`  
See the minimum required Unity version in the `package.json` file.  
Find "Samples" in the package window and click the "Import" button.  
To update the package, simply add it again using a different version tag.  
  
This package automatically inserts YandexGames SDK script into HTML page when calling `YandexGamesSdk.Initialize()`. No need to modify WebGLTemplates, just use the SDK methods.  
  
This is a publishing repository. If you wish to create a pull request, please use the [Development Repo](https://github.com/forcepusher/YandexGamesUnity).
