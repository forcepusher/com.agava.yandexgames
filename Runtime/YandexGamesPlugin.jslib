const yandexGamesLibrary = {

  // Class definition.

  $yandexGames: {
    isInitialized: false,

    isAuthorized: false,

    sdk: undefined,

    leaderboard: undefined,

    playerAccount: undefined,

    billing: undefined,

    isInitializeCalled: false,

    yandexGamesSdkInitialize: function (successCallbackPtr) {
      if (yandexGames.isInitializeCalled) {
        return;
      }
      yandexGames.isInitializeCalled = true;

      const sdkScript = document.createElement('script');
      sdkScript.src = '/sdk.js';
      document.head.appendChild(sdkScript);

      sdkScript.onload = function () {
        window['YaGames'].init().then(function (sdk) {
          yandexGames.sdk = sdk;

          // The { scopes: false } ensures personal data permission request window won't pop up,
          const playerAccountInitializationPromise = sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
            if (playerAccount.getMode() !== 'lite') {
              yandexGames.isAuthorized = true;
            }

            // Always contains permission info. Contains personal data as well if permissions were granted before.
            yandexGames.playerAccount = playerAccount;
          }).catch(function () { throw new Error('PlayerAccount failed to initialize.'); });

          const leaderboardInitializationPromise = sdk.getLeaderboards().then(function (leaderboard) {
            yandexGames.leaderboard = leaderboard;
          }).catch(function () { throw new Error('Leaderboard failed to initialize.'); });

          const billingInitializationPromise = sdk.getPayments({ signed: true }).then(function (billing) {
            yandexGames.billing = billing;
          }).catch(function () { throw new Error('Billing failed to initialize.'); });

          Promise.allSettled([leaderboardInitializationPromise, playerAccountInitializationPromise, billingInitializationPromise]).then(function () {
            yandexGames.isInitialized = true;
            {{{ makeDynCall('v', 'successCallbackPtr') }}}();
          });
        });
      }
    },

    throwIfSdkNotInitialized: function () {
      if (!yandexGames.isInitialized) {
        throw new Error('SDK is not initialized. Invoke YandexGamesSdk.Initialize() coroutine and wait for it to finish.');
      }
    },

    gameReady: function() {
      yandexGames.sdk.features.LoadingAPI.ready();
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
      {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(errorUnmanagedStringPtr);
      _free(errorUnmanagedStringPtr);
    },

    invokeErrorCallbackIfNotAuthorized: function (errorCallbackPtr) {
      if (!yandexGames.isAuthorized) {
        yandexGames.invokeErrorCallback(new Error('Needs authorization.'), errorCallbackPtr);
        return true;
      }
      return false;
    },

    getYandexGamesSdkEnvironment: function () {
      const environmentJson = JSON.stringify(yandexGames.sdk.environment);
      const environmentJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(environmentJson);
      return environmentJsonUnmanagedStringPtr;
    },

    getDeviceType: function () {
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

    playerAccountStartAuthorizationPolling: function (repeatDelay, successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.isAuthorized) {
        console.error('Already authorized.');
        {{{ makeDynCall('v', 'errorCallbackPtr') }}}();
        return;
      }

      function authorizationPollingLoop() {
        if (yandexGames.isAuthorized) {
          {{{ makeDynCall('v', 'successCallbackPtr') }}}();
          return;
        }

        yandexGames.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
          if (playerAccount.getMode() !== 'lite') {
            yandexGames.isAuthorized = true;
            yandexGames.playerAccount = playerAccount;
            {{{ makeDynCall('v', 'successCallbackPtr') }}}();
          } else {
            setTimeout(authorizationPollingLoop, repeatDelay);
          }
        });
      };

      authorizationPollingLoop();
    },

    playerAccountAuthorize: function (successCallbackPtr, errorCallbackPtr) {
      if (yandexGames.isAuthorized) {
        console.error('Already authorized.');
        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
        return;
      }

      yandexGames.sdk.auth.openAuthDialog().then(function () {
        yandexGames.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
          yandexGames.isAuthorized = true;
          yandexGames.playerAccount = playerAccount;
          {{{ makeDynCall('v', 'successCallbackPtr') }}}();
        }).catch(function (error) {
          console.error('authorize failed to update playerAccount. Assuming authorization failed. Error was: ' + error.message);
          yandexGames.invokeErrorCallback(error, errorCallbackPtr);
        });
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    getPlayerAccountHasPersonalProfileDataPermission: function () {
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
      yandexGames.sdk.getPlayer({ scopes: true }).then(function (playerAccount) {
        yandexGames.playerAccount = playerAccount;

        if (yandexGames.getPlayerAccountHasPersonalProfileDataPermission()) {
          {{{ makeDynCall('v', 'successCallbackPtr') }}}();
        } else {
          yandexGames.invokeErrorCallback(new Error('User has refused the permission request.'), errorCallbackPtr);
        }
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr, pictureSize) {
      yandexGames.sdk.getPlayer({ scopes: false }).then(function (playerAccount) {
        yandexGames.playerAccount = playerAccount;

        playerAccount._personalInfo.profilePicture = playerAccount.getPhoto(pictureSize);

        const profileDataJson = JSON.stringify(playerAccount._personalInfo);
        const profileDataUnmanagedStringPtr = yandexGames.allocateUnmanagedString(profileDataJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(profileDataUnmanagedStringPtr);
        _free(profileDataUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountGetCloudSaveData: function (successCallbackPtr, errorCallbackPtr) {
      yandexGames.playerAccount.getData().then(function (сloudSaveData) {
        const сloudSaveDataUnmanagedStringPtr = yandexGames.allocateUnmanagedString(JSON.stringify(сloudSaveData));
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(сloudSaveDataUnmanagedStringPtr);
        _free(сloudSaveDataUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    playerAccountSetCloudSaveData: function (сloudSaveDataJson, successCallbackPtr, errorCallbackPtr) {
      var сloudSaveData;
      try {
        сloudSaveData = JSON.parse(сloudSaveDataJson);
      } catch (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
        return;
      }

      yandexGames.playerAccount.setData(сloudSaveData, true).then(function () {
        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    interstitialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
      yandexGames.sdk.adv.showFullscreenAdv({
        callbacks: {
          onOpen: function () {
            {{{ makeDynCall('v', 'openCallbackPtr') }}}();
          },
          onClose: function (wasShown) {
            {{{ makeDynCall('vi', 'closeCallbackPtr') }}}(wasShown);
          },
          onError: function (error) {
            yandexGames.invokeErrorCallback(error, errorCallbackPtr);
          },
          onOffline: function () {
            {{{ makeDynCall('v', 'offlineCallbackPtr') }}}();
          },
        }
      });
    },

    videoAdShow: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
      yandexGames.sdk.adv.showRewardedVideo({
        callbacks: {
          onOpen: function () {
            {{{ makeDynCall('v', 'openCallbackPtr') }}}();
          },
          onRewarded: function () {
            {{{ makeDynCall('v', 'rewardedCallbackPtr') }}}();
          },
          onClose: function () {
            {{{ makeDynCall('v', 'closeCallbackPtr') }}}();
          },
          onError: function (error) {
            yandexGames.invokeErrorCallback(error, errorCallbackPtr);
          },
        }
      });
    },

    stickyAdShow: function() {
      yandexGames.sdk.adv.showBannerAdv();
    },

    stickyAdHide: function() {
      yandexGames.sdk.adv.hideBannerAdv();
    },

    leaderboardSetScore: function (leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardSetScore requires authorization.');
        return;
      }

      yandexGames.leaderboard.setLeaderboardScore(leaderboardName, score, extraData).then(function () {
        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    leaderboardGetEntries: function (leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf, pictureSize) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardGetEntries requires authorization.');
        return;
      }

      yandexGames.leaderboard.getLeaderboardEntries(leaderboardName, {
        includeUser: includeSelf, quantityAround: competingPlayersCount, quantityTop: topPlayersCount
      }).then(function (response) {
        response.entries.forEach(function(entry) {
          entry.player.profilePicture = entry.player.getAvatarSrc({ size: pictureSize });
        });

        const entriesJson = JSON.stringify(response);
        const entriesUnmanagedStringPtr = yandexGames.allocateUnmanagedString(entriesJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(entriesUnmanagedStringPtr);
        _free(entriesUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    leaderboardGetPlayerEntry: function (leaderboardName, successCallbackPtr, errorCallbackPtr, pictureSize) {
      if (yandexGames.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
        console.error('leaderboardGetPlayerEntry requires authorization.');
        return;
      }

      yandexGames.leaderboard.getLeaderboardPlayerEntry(leaderboardName).then(function (response) {
        response.player.profilePicture = response.player.getAvatarSrc({ size: pictureSize });

        const entryJson = JSON.stringify(response);
        const entryJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(entryJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(entryJsonUnmanagedStringPtr);
        _free(entryJsonUnmanagedStringPtr);
      }).catch(function (error) {
        if (error.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
          const nullUnmanagedStringPtr = yandexGames.allocateUnmanagedString('null');
          {{{ makeDynCall('vi', 'successCallbackPtr') }}}(nullUnmanagedStringPtr);
          _free(nullUnmanagedStringPtr);
        } else {
          yandexGames.invokeErrorCallback(error, errorCallbackPtr);
        }
      });
    },

    billingPurchaseProduct: function (productId, successCallbackPtr, errorCallbackPtr, developerPayload) {
      yandexGames.billing.purchase({ id: productId, developerPayload: developerPayload }).then(function (purchaseResponse) {
        purchaseResponse = { purchaseData: purchaseResponse.purchaseData, signature: purchaseResponse.signature };

        const purchasedProductJson = JSON.stringify(purchaseResponse);
        const purchasedProductJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(purchasedProductJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(purchasedProductJsonUnmanagedStringPtr);
        _free(purchasedProductJsonUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    billingConsumeProduct: function (purchasedProductToken, successCallbackPtr, errorCallbackPtr) {
      yandexGames.billing.consumePurchase(purchasedProductToken).then(function (consumedProduct) {
        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    billingGetProductCatalog: function (successCallbackPtr, errorCallbackPtr) {
      yandexGames.billing.getCatalog().then(function (productCatalogResponse) {
        const products = [];

        for (var catalogIterator = 0; catalogIterator < productCatalogResponse.length; catalogIterator++) {
          products[catalogIterator] = {
            description: productCatalogResponse[catalogIterator].description,
            id: productCatalogResponse[catalogIterator].id,
            imageURI: productCatalogResponse[catalogIterator].imageURI,
            price: productCatalogResponse[catalogIterator].price,
            priceCurrencyCode: productCatalogResponse[catalogIterator].priceCurrencyCode,
            priceCurrencyImage: "https:" + productCatalogResponse[catalogIterator].getPriceCurrencyImage('medium'),
            priceValue: productCatalogResponse[catalogIterator].priceValue,
            title: productCatalogResponse[catalogIterator].title
          };
        }

        productCatalogResponse = {
          products: products,
          signature: productCatalogResponse.signature
        };

        const productCatalogJson = JSON.stringify(productCatalogResponse);
        const productCatalogJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(productCatalogJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(productCatalogJsonUnmanagedStringPtr);
        _free(productCatalogJsonUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    billingGetPurchasedProducts: function (successCallbackPtr, errorCallbackPtr) {
      yandexGames.billing.getPurchases().then(function (purchasesResponse) {
        purchasesResponse = { purchasedProducts: purchasesResponse, signature: purchasesResponse.signature };

        const purchasedProductsJson = JSON.stringify(purchasesResponse);
        const purchasedProductsJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(purchasedProductsJson);
        {{{ makeDynCall('vi', 'successCallbackPtr') }}}(purchasedProductsJsonUnmanagedStringPtr);
        _free(purchasedProductsJsonUnmanagedStringPtr);
      }).catch(function (error) {
        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
      });
    },

    shortcutCanSuggest: function(resultCallbackPtr) {
      yandexGames.sdk.shortcut.canShowPrompt().then(function(prompt) {
        {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(prompt.canShow);
      });
    },

    shortcutSuggest: function(resultCallbackPtr) {
      yandexGames.sdk.shortcut.showPrompt().then(function(result) {
        {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(result.outcome === 'accepted');
      });
    },

    reviewPopupCanOpen: function(resultCallbackPtr) {
      yandexGames.sdk.feedback.canReview().then(function(result, reason) {
        if (!reason) { reason = 'No reason'; }
        const reasonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(reason);
        {{{ makeDynCall('vii', 'resultCallbackPtr') }}}(result, reasonUnmanagedStringPtr);
        _free(reasonUnmanagedStringPtr);
      });
    },

    reviewPopupOpen: function(resultCallbackPtr) {
      yandexGames.sdk.feedback.requestReview().then(function(result) {
        {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(result);
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

  YandexGamesSdkInitialize: function (successCallbackPtr) {
    yandexGames.yandexGamesSdkInitialize(successCallbackPtr);
  },

  GetYandexGamesSdkIsInitialized: function () {
    return yandexGames.isInitialized;
  },

  GetYandexGamesSdkEnvironment: function () {
    yandexGames.throwIfSdkNotInitialized();

    return yandexGames.getYandexGamesSdkEnvironment();
  },

  GetDeviceType: function () {
    yandexGames.throwIfSdkNotInitialized();

    return yandexGames.getDeviceType();
  },

  PlayerAccountStartAuthorizationPolling: function (delay, successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountStartAuthorizationPolling(delay, successCallbackPtr, errorCallbackPtr);
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

  PlayerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr, pictureSizePtr) {
    yandexGames.throwIfSdkNotInitialized();

    const pictureSize = UTF8ToString(pictureSizePtr);

    yandexGames.playerAccountGetProfileData(successCallbackPtr, errorCallbackPtr, pictureSize);
  },

  PlayerAccountGetCloudSaveData: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.playerAccountGetCloudSaveData(successCallbackPtr, errorCallbackPtr);
  },

  PlayerAccountSetCloudSaveData: function (сloudSaveDataJsonPtr, successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const сloudSaveDataJson = UTF8ToString(сloudSaveDataJsonPtr);

    yandexGames.playerAccountSetCloudSaveData(сloudSaveDataJson, successCallbackPtr, errorCallbackPtr);
  },

  InterstitialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.interstitialAdShow(openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr);
  },

  VideoAdShow: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.videoAdShow(openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr);
  },

  StickyAdShow: function () {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.stickyAdShow();
  },

  StickyAdHide: function () {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.stickyAdHide();
  },

  LeaderboardSetScore: function (leaderboardNamePtr, score, successCallbackPtr, errorCallbackPtr, extraDataPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    var extraData = UTF8ToString(extraDataPtr);
    if (extraData.length === 0) { extraData = undefined; }

    yandexGames.leaderboardSetScore(leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData);
  },

  LeaderboardGetEntries: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf, pictureSizePtr) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    // Booleans are transferred as either 1 or 0, so using !! to convert them to true or false.
    includeSelf = !!includeSelf;
    const pictureSize = UTF8ToString(pictureSizePtr);

    yandexGames.leaderboardGetEntries(leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf, pictureSize);
  },

  LeaderboardGetPlayerEntry: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr, pictureSizePtr) {
    yandexGames.throwIfSdkNotInitialized();

    const leaderboardName = UTF8ToString(leaderboardNamePtr);
    const pictureSize = UTF8ToString(pictureSizePtr);

    yandexGames.leaderboardGetPlayerEntry(leaderboardName, successCallbackPtr, errorCallbackPtr, pictureSize);
  },

  BillingPurchaseProduct: function (productIdPtr, successCallbackPtr, errorCallbackPtr, developerPayloadPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const productId = UTF8ToString(productIdPtr);
    var developerPayload = UTF8ToString(developerPayloadPtr);
    if (developerPayload.length === 0) { developerPayload = undefined; }

    yandexGames.billingPurchaseProduct(productId, successCallbackPtr, errorCallbackPtr, developerPayload);
  },

  BillingConsumeProduct: function (purchasedProductTokenPtr, successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    const purchasedProductToken = UTF8ToString(purchasedProductTokenPtr);

    yandexGames.billingConsumeProduct(purchasedProductToken, successCallbackPtr, errorCallbackPtr);
  },

  BillingGetProductCatalog: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.billingGetProductCatalog(successCallbackPtr, errorCallbackPtr);
  },

  BillingGetPurchasedProducts: function (successCallbackPtr, errorCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.billingGetPurchasedProducts(successCallbackPtr, errorCallbackPtr);
  },

  ShortcutCanSuggest: function (resultCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.shortcutCanSuggest(resultCallbackPtr);
  },

  ShortcutSuggest: function (resultCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.shortcutSuggest(resultCallbackPtr);
  },

  ReviewPopupCanOpen: function (resultCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.reviewPopupCanOpen(resultCallbackPtr);
  },

  ReviewPopupOpen: function (resultCallbackPtr) {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.reviewPopupOpen(resultCallbackPtr);
  },

  YandexGamesSdkGameReady: function() {
    yandexGames.throwIfSdkNotInitialized();

    yandexGames.gameReady();
  },

  YandexGamesSdkIsRunningOnYandex: function() {
    const hostname = window.location.hostname;
    return hostname.includes('yandex') || hostname.includes('playhop');
  }
}

autoAddDeps(yandexGamesLibrary, '$yandexGames');
mergeInto(LibraryManager.library, yandexGamesLibrary);
