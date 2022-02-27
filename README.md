# com.agava.yandexgames  
  
Make sure you have standalone [Git](https://git-scm.com/downloads) installed first.  
In Unity, open "Window" -> "Package Manager".  
Click the "+" sign on top left corner -> "Add package from git URL..."  
Paste this: `https://github.com/forcepusher/com.agava.yandexgames.git#8.2.0`  
See minimum required Unity version in the `package.json` file.  
Find "Samples" in the package window and click the "Import" button. Use it as a guide.  
  
This package automatically inserts YandexGames SDK script into HTML page at runtime and initializes itself. No need to mess with WebGLTemplates - just use the SDK methods.  
  
This is a publishing repo. If you need to create a pull request, use the [Development Repo](https://github.com/forcepusher/YandexGamesUnity).
