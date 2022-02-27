const library = {

  // Class definition.

  $yandexGames: {
    isInitialized: false,

    isAuthorized: false,

    sdk: undefined,

    leaderboard: undefined,

    playerAccount: undefined,

    yandexGamesSdkInitialize: function () {
      const sdkScript = document.createElement('script');
      sdkScript.src = 'https://yandex.ru/games/sdk/v2';
      document.head.appendChild(sdkScript);

      sdkScript.onload = function () {
        window['YaGames'].init().then(function (sdk) {
          yandexGames.sdk = sdk;

          // The { scopes: false } ensures personal data permission request window won't pop up,
          const playerAccountInitializationPromise = sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
            yandexGames.isAuthorized = true;
            // Always contains permission info. Contains personal data as well if permissions were granted before.
            yandexGames.playerAccount = playerAccount;

            // Catch the error that gets thrown when user is not authorized.
            // This IS the intended way to check for player authorization, not even kidding:
            // https://yandex.ru/dev/games/doc/dg/sdk/sdk-player.html#sdk-player__auth
          }).catch(function () { });

          const leaderboardInitializationPromise = sdk.getLeaderboards().then(function (leaderboard) {
            yandexGames.leaderboard = leaderboard;
          }).catch(function () { });

          Promise.allSettled([leaderboardInitializationPromise, playerAccountInitializationPromise]).then(function () {
            if (yandexGames.leaderboard === undefined) {
              throw new Error('Leaderboard caused Yandex Games SDK to fail initialization.');
            }

            yandexGames.isInitialized = true;
          });
        });
      }
    },

    throwIfSdkNotInitialized: function () {
      if (!yandexGames.isInitialized) {
        throw new Error('SDK was not fast enough to initialize. Use YandexGamesSdk.Initialized or WaitForInitialization.');
      }
    },

    invokeErrorCallback: function (error, errorCallbackPtr) {
      var errorMessage;
      if (error instanceof Error) {
        errorMessage = error.message;
        if (errorMessage === null) { errorMessage = 'SDK API thrown an error with null message.' }
        if (errorMessage === undefined) { errorMessage = 'SDK API thrown an error with undefined message.' }
      } else if (typeof error === 'string') {
        errorMessage = error;
      } else if (error) {
        errorMessage = 'SDK API thrown an unexpected type as error: ' + JSON.stringify(error);
      } else if (error === null) {
        errorMessage = 'SDK API thrown a null as error.';
      } else {
        errorMessage = 'SDK API thrown an undefined as error.';
      }

      const errorUnmanagedStringPtr = yandexGames.allocateUnmanagedString(errorMessage);
      dynCall('vi', errorCallbackPtr, [errorUnmanagedStringPtr]);
      _free(errorUnmanagedStringPtr);
    },

    invokeErrorCallbackIfNotAuthorized: function (errorCallbackPtr) {
      if (!yandexGames.isAuthorized) {
        yandexGames.invokeErrorCallback(new Error('Needs authorization.'), errorCallbackPtr);
        return true;
      }
      return false;
    },

    deviceGetType: function () {
      const deviceType = yandexGames.sdk.deviceInfo.type;

      switch (deviceType) {
        case 'desktop':
          return 0;
        case 'mobile':
          return 1;
        case 'tablet':
          return 2;
        case 'tv':
          return 3;
        default:
          console.error('Unexpected ysdk.deviceInfo response from Yandex. Assuming that it is desktop. deviceType = '
            + JSON.stringify(deviceType));
          return 0;
      }
    },

    playerAccountAuthorize: function (successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.isAuthorized) {
        console.error('Already authorized.');
        dynCall('v', successCallbackPtr, []);
        return;
      }

      yandexGames.sdk.auth.openAuthDialog().then(function () {
        yandexGames.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
          yandexGames.isAuthorized = true;
          yandexGames.playerAccount = playerAccount;
          dynCall('v', successCallbackPtr, []);
        }).catch(function (error) {
          console.error('authorize failed to update playerAccount. Assuming authorization failed. Error was: ' + error.message);
          yandexGames.invokeErrorCallback(error, errorCallbackPtr);
        });
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    getPlayerAccountHasPersonalProfileDataPermission: function () {
      if (!yandexGames.isAuthorized) {
        console.error('getPlayerAccountHasPersonalProfileDataPermission requires authorization. Assuming profile data permissions were not granted.');
        return false;
      }

      var publicNamePermission = undefined;
      if ('_personalInfo' in yandexGames.playerAccount && 'scopePermissions' in yandexGames.playerAccount._personalInfo) {
        publicNamePermission = yandexGames.playerAccount._personalInfo.scopePermissions.public_name;
      }

      switch (publicNamePermission) {
        case 'forbid':
          return false;
        case 'not_set':
          return false;
        case 'allow':
          return true;
        default:
          console.error('Unexpected response from Yandex. Assuming profile data permissions were not granted. playerAccount = '
            + JSON.stringify(yandexGames.playerAccount));
          return false;
      }
    },

    playerAccountRequestPersonalProfileDataPermission: function (successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('playerAccountRequestPersonalProfileDataPermission requires authorization. Assuming profile data permissions were not granted.');
        return;
      }

      yandexGames.sdk.getPlayer({ scopes: true }).then(function (playerAccount) {
        yandexGames.playerAccount = playerAccount;

        if (yandexGames.getPlayerAccountHasPersonalProfileDataPermission()) {
          dynCall('v', successCallbackPtr, []);
        } else {
          yandexGames.invokeErrorCallback(new Error('User has refused the permission request.'), errorCallbackPtr);
        }
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('playerAccountGetProfileData requires authorization. Assuming profile data permissions were not granted.');
        return;
      }

      yandexGames.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
        yandexGames.playerAccount = playerAccount;
        const profileDataJson = JSON.stringify(playerAccount._personalInfo);
        const profileDataUnmanagedStringPtr = yandexGames.allocateUnmanagedString(profileDataJson);
        dynCall('vi', successCallbackPtr, [profileDataUnmanagedStringPtr]);
        _free(profileDataUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountGetPlayerData: function (successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('playerAccountGetPlayerData requires authorization.');
        return;
      }

      yandexGames.playerAccount.getData().then(function (playerData) {
        const playerDataUnmanagedStringPtr = yandexGames.allocateUnmanagedString(JSON.stringify(playerData));
        dynCall('vi', successCallbackPtr, [playerDataUnmanagedStringPtr]);
        _free(playerDataUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountSetPlayerData: function (playerDataJson, successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('playerAccountSetPlayerData requires authorization.');
        return;
      }

      var playerData = JSON.parse(playerDataJson);
      yandexGames.playerAccount.setData(playerData, true).then(function () {
        dynCall('v', successCallbackPtr, []);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    interestialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
      yandexGames.sdk.adv.showFullscreenAdv({
        callbacks: {
          onOpen: function () {
            dynCall('v', openCallbackPtr, []);
          },
          onClose: function (wasShown) {
            dynCall('vi', closeCallbackPtr, [wasShown]);
          },
          onError: function (error) {
            yandexGames.invokeErrorCallback(error, errorCallbackPtr);
          },
          onOffline: function () {
            dynCall('v', offlineCallbackPtr, []);
          },
        }
      });
    },

    videoAdShow: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
      yandexGames.sdk.adv.showRewardedVideo({
        callbacks: {
          onOpen: function () {
            dynCall('v', openCallbackPtr, []);
          },
          onRewarded: function () {
            dynCall('v', rewardedCallbackPtr, []);
          },
          onClose: function () {
            dynCall('v', closeCallbackPtr, []);
          },
          onError: function (error) {
            yandexGames.invokeErrorCallback(error, errorCallbackPtr);
          },
        }
      });
    },

    leaderboardSetScore: function (leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardSetScore requires authorization.');
        return;
      }

      yandexGames.leaderboard.setLeaderboardScore(leaderboardName, score, extraData).then(function () {
        dynCall('v', successCallbackPtr, []);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    leaderboardGetEntries: function (leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardGetEntries requires authorization.');
        return;
      }

      yandexGames.leaderboard.getLeaderboardEntries(leaderboardName, {
        includeUser: includeSelf, quantityAround: competingPlayersCount, quantityTop: topPlayersCount
      }).then(function (response) {
        const entriesJson = JSON.stringify(response);
        const entriesUnmanagedStringPtr = yandexGames.allocateUnmanagedString(entriesJson);
        dynCall('vi', successCallbackPtr, [entriesUnmanagedStringPtr]);
        _free(entriesUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    leaderboardGetPlayerEntry: function (leaderboardName, successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardGetPlayerEntry requires authorization.');
        return;
      }

      yandexGames.leaderboard.getLeaderboardPlayerEntry(leaderboardName).then(function (response) {
        const entryJson = JSON.stringify(response);
        const entryUnmanagedStringPtr = yandexGames.allocateUnmanagedString(entryJson);
        dynCall('vi', successCallbackPtr, [entryUnmanagedStringPtr]);
        _free(entryUnmanagedStringPtr);
      }).catch(function (error) {
        if (error.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
          const nullUnmanagedStringPtr = yandexGames.allocateUnmanagedString('null');
          dynCall('vi', successCallbackPtr, [nullUnmanagedStringPtr]);
          _free(nullUnmanagedStringPtr);
        } else {
          yandexGames.invokeErrorCallback(error, errorCallbackPtr);
        }
      });
    },

    allocateUnmanagedString: function (string) {
      const stringBufferSize = lengthBytesUTF8(string) + 1;
      const stringBufferPtr = _malloc(stringBufferSize);
      stringToUTF8(string, stringBufferPtr, stringBufferSize);
      return stringBufferPtr;
    },
  },


  // External C# calls.

  YandexGamesSdkInitialize: function () {
    yandexGames.yandexGamesSdkInitialize();
  },

  GetYandexGamesSdkIsInitialized: function () {
    return yandexGames.isInitialized;
  },

  DeviceGetType: function () {
    yandexGames.throwIfSdkNotInitialized();

    return yandexGames.deviceGetType();
  },

  PlayerAccountAuthorize: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountAuthorize(successCallbackPtr, errorCallbackPtr);
  },

  GetPlayerAccountIsAuthorized: function () {
    yandexGames.throwIfSdkNotInitialized();

    return yandexGames.isAuthorized;
  },

  GetPlayerAccountHasPersonalProfileDataPermission: function () {
    yandexGames.throwIfSdkNotInitialized();

    return yandexGames.getPlayerAccountHasPersonalProfileDataPermission();
  },

  PlayerAccountRequestPersonalProfileDataPermission: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountRequestPersonalProfileDataPermission(successCallbackPtr, errorCallbackPtr);
  },

  PlayerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountGetProfileData(successCallbackPtr, errorCallbackPtr);
  },

  PlayerAccountGetPlayerData: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountGetPlayerData(successCallbackPtr, errorCallbackPtr);
  },

  PlayerAccountSetPlayerData: function (playerDataJsonPtr, successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const playerDataJson = UTF8ToString(playerDataJsonPtr);
    yandexGames.playerAccountSetPlayerData(playerDataJson, successCallbackPtr, errorCallbackPtr);
  },

  InterestialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.interestialAdShow(openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr);
  },

  VideoAdShow: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.videoAdShow(openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr);
  },

  LeaderboardSetScore: function (leaderboardNamePtr, score, successCallbackPtr, errorCallbackPtr, extraDataPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    var extraData = UTF8ToString(extraDataPtr);
    if (extraData.length === 0) { extraData = undefined; }
    yandexGames.leaderboardSetScore(leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData);
  },

  LeaderboardGetEntries: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    // Booleans are transferred as either 1 or 0, so using !! to convert them to true or false.
    includeSelf = !!includeSelf;
    yandexGames.leaderboardGetEntries(leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf);
  },

  LeaderboardGetPlayerEntry: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    yandexGames.leaderboardGetPlayerEntry(leaderboardName, successCallbackPtr, errorCallbackPtr);
  },
}

autoAddDeps(library, '$yandexGames');
mergeInto(LibraryManager.library, library);
